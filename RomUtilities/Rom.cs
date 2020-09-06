using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomUtilities
{
	public abstract class Rom
	{
		protected Blob Data;
		protected Blob Header;

		public byte this[int index]
		{
			get { return Data[index]; }
			set { Data[index] = value; }
		}

		public int HeaderLength => Header.Length;
		public int DataLength => Data.Length;
		public int TotalLength => Header.Length + Data.Length;

		public abstract void Load(Stream readStream);
		public abstract Task LoadAsync(Stream readStream);

		public void Load(string filename)
		{
			using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				Load(fs);
			}
		}

		public async Task LoadAsync(string filename)
		{
			using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				await LoadAsync(fs);
			}
		}

		public void Save(Stream writeStream)
		{
			writeStream.Write(Header, 0, Header.Length);
			writeStream.Write(Data, 0, Data.Length);
		}

		public async Task SaveAsync(Stream writeStream)
		{
			await writeStream.WriteAsync(Header, 0, Header.Length);
			await writeStream.WriteAsync(Data, 0, Data.Length);
		}

		public void Save(string filename)
		{
			using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				Save(fs);
			}
		}

		public async Task SaveAsync(string filename)
		{
			using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				await SaveAsync(fs);
			}
		}

		public abstract bool Validate();

		public Blob Get(int index, int length)
		{
			return Data.SubBlob(index, length);
		}

		public void Put(int index, Blob data)
		{
			Array.Copy(data, 0, Data, index, data.Length);
		}
	}
}
