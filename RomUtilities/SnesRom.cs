using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomUtilities
{
	public abstract class SnesRom : Rom
	{
		protected SnesRom()
		{ }

		public SnesRom(Stream readStream)
		{
			Load(readStream);
		}

		public SnesRom(string filename)
		{
			Load(filename);
		}

		public sealed override void Load(Stream readStream)
		{
			var headerLength = (int)readStream.Length % 131072;
			var dataLength = (int)readStream.Length - headerLength;

			Data = new byte[dataLength];
			Header = new byte[headerLength];

			readStream.Read(Header, 0, headerLength);
			readStream.Read(Data, 0, dataLength);
		}

		public sealed override async Task LoadAsync(Stream readStream)
		{
			var headerLength = (int)readStream.Length % 131072;
			var dataLength = (int)readStream.Length - headerLength;

			Data = new byte[dataLength];
			Header = new byte[headerLength];

			await readStream.ReadAsync(Header, 0, headerLength);
			await readStream.ReadAsync(Data, 0, dataLength);
		}
	}
}
