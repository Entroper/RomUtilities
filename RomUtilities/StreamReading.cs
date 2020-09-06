using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RomUtilities
{
	public static class StreamReading
	{
		public static int ReadLength(this Stream readStream, byte[] buffer, int offset, int length)
		{
			int readSize = 0;
			int thisReadSize;
			do
			{
				thisReadSize = readStream.Read(buffer, offset, length);
				offset += thisReadSize;
				readSize += thisReadSize;
				length -= thisReadSize;
			} while (length > 0 && thisReadSize > 0);

			return readSize;
		}

		public static async Task<int> ReadLengthAsync(this Stream readStream, byte[] buffer, int offset, int length)
		{
			int readSize = 0;
			int thisReadSize;
			do
			{
				thisReadSize = await readStream.ReadAsync(buffer, offset, length);
				offset += thisReadSize;
				readSize += thisReadSize;
				length -= thisReadSize;
			} while (length > 0 && thisReadSize > 0);

			return readSize;
		}
	}
}
