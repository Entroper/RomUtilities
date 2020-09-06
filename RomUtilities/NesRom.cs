using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomUtilities
{
	public abstract class NesRom : Rom
	{
		protected NesRom()
		{ }

		public NesRom(Stream romStream)
		{
			Load(romStream);
		}

		public NesRom(string filename)
		{
			Load(filename);
		}

		public sealed override void Load(Stream readStream)
		{
			var headerLength = (int)readStream.Length % 8192;
			var dataLength = (int)readStream.Length - headerLength;

			Data = new byte[dataLength];
			Header = new byte[headerLength];

			readStream.ReadLength(Header, 0, headerLength);
			readStream.ReadLength(Data, 0, dataLength);
		}

		public sealed override async Task LoadAsync(Stream readStream)
		{
			var headerLength = (int)readStream.Length % 8192;
			var dataLength = (int)readStream.Length - headerLength;

			Data = new byte[dataLength];
			Header = new byte[headerLength];

			await readStream.ReadLengthAsync(Header, 0, headerLength);
			await readStream.ReadLengthAsync(Data, 0, dataLength);
		}
	}
}
