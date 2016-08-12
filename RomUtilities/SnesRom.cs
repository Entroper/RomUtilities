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
		public SnesRom(string filename)
		{
			Load(filename);
		}

		public sealed override void Load(string filename)
		{
			using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				var headerLength = (int)fs.Length % 131072;
				var dataLength = (int)fs.Length - headerLength;

				Data = new byte[dataLength];
				Header = new byte[headerLength];

				fs.Read(Header, 0, headerLength);
				fs.Read(Data, 0, dataLength);
			}
		}
	}
}
