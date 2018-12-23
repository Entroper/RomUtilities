using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RomUtilities
{
	public sealed class Blob : IEquatable<Blob>
	{
		private readonly byte[] _data;

		private Blob(byte[] data)
		{
			_data = data;
		}

		public int Length => _data.Length;

		public byte[] ToBytes()
		{
			var copy = new byte[_data.Length];
			_data.CopyTo(copy, 0); // Array.CopyTo is ever so slightly faster than Buffer.BlockCopy.

			return copy;
		}

		public sbyte[] ToSBytes()
		{
			var copy = new sbyte[_data.Length];
			Buffer.BlockCopy(_data, 0, copy, 0, copy.Length);

			return copy;
		}

		public ushort[] ToUShorts()
		{
			var copy = new ushort[_data.Length / 2];
			Buffer.BlockCopy(_data, 0, copy, 0, copy.Length * 2);

			return copy;
		}

		public short[] ToShorts()
		{
			var copy = new short[_data.Length / 2];
			Buffer.BlockCopy(_data, 0, copy, 0, copy.Length * 2);

			return copy;
		}

		public uint[] ToUInts()
		{
			var copy = new uint[_data.Length / 4];
			Buffer.BlockCopy(_data, 0, copy, 0, copy.Length * 4);

			return copy;
		}

		public int[] ToInts()
		{
			var copy = new int[_data.Length / 4];
			Buffer.BlockCopy(_data, 0, copy, 0, copy.Length * 4);

			return copy;
		}

		public ulong[] ToULongs()
		{
			var copy = new ulong[_data.Length / 8];
			Buffer.BlockCopy(_data, 0, copy, 0, copy.Length * 8);

			return copy;
		}

		public long[] ToLongs()
		{
			var copy = new long[_data.Length / 8];
			Buffer.BlockCopy(_data, 0, copy, 0, copy.Length * 8);

			return copy;
		}

		public static Blob FromSBytes(sbyte[] values)
		{
			var bytes = new byte[values.Length];
			Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);

			return bytes;
		}

		public static Blob FromUShorts(ushort[] values)
		{
			var bytes = new byte[values.Length * 2];
			Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);

			return bytes;
		}

		public static Blob FromShorts(short[] values)
		{
			var bytes = new byte[values.Length * 2];
			Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);

			return bytes;
		}

		public static Blob FromUInts(uint[] values)
		{
			var bytes = new byte[values.Length * 4];
			Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);

			return bytes;
		}

		public static Blob FromInts(int[] values)
		{
			var bytes = new byte[values.Length * 4];
			Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);

			return bytes;
		}

		public static Blob FromULongs(ulong[] values)
		{
			var bytes = new byte[values.Length*8];
			Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);

			return bytes;
		}

		public static Blob FromLongs(long[] values)
		{
			var bytes = new byte[values.Length*8];
			Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);

			return bytes;
		}

		//
		// Equality
		//

		public override bool Equals(object obj) => Equals(obj as Blob);
		public bool Equals(Blob other) => !ReferenceEquals(other, null) && _data.SequenceEqual(other._data);

		public override int GetHashCode() => _data.GetHashCode();

		//
		// Operators
		//

		public byte this[int index]
		{
			get { return _data[index]; }
			set { _data[index] = value; }
		}

		public static bool operator ==(Blob x, Blob y) => ReferenceEquals(x, y) || (!ReferenceEquals(x, null) && x.Equals(y));
		public static bool operator !=(Blob x, Blob y) => !(x == y);

		public static implicit operator Blob(byte[] data) => new Blob(data);
		public static implicit operator byte[] (Blob value) => value._data;

		public static Blob operator +(Blob lhs, Blob rhs)
		{
			byte[] data = new byte[lhs.Length + rhs.Length];
			lhs._data.CopyTo(data, 0);
			rhs._data.CopyTo(data, lhs.Length);

			return new Blob(data);
		}

		//
		// Utility
		//

		public Blob SubBlob(int start, int length)
		{
			byte[] data = new byte[length];
			Array.Copy(_data, start, data, 0, length);

			return new Blob(data);
		}

		public Blob SubBlob(int start)
		{
			int length = _data.Length - start;

			byte[] data = new byte[length];
			Array.Copy(_data, start, data, 0, length);

			return new Blob(data);
		}

		// Prefer strongly-typed version of IndexOf to protect against calls that LOOK like implicit conversions, but aren't.
		public int IndexOf(byte find)
		{
			return Array.IndexOf(_data, find);
		}

		public List<Blob> Split(byte separator)
		{
			var segments = new List<Blob>();
			var copy = new Blob(ToBytes());
			int index;
			do
			{
				index = Array.IndexOf(copy, separator);
                if (index >= 0)
                {
                    segments.Add(copy.SubBlob(0, index));
                    copy = copy.SubBlob(index + 1);
                }
            } while (index != -1);

            if (copy.Length > 0)
            {
                segments.Add(copy);
            }

			return segments;
		}

		public List<Blob> Chunk(int size)
		{
			var chunks = Enumerable.Range(0, _data.Length/size).Select(i => SubBlob(i*size, size)).ToList();
			if (_data.Length%size != 0)
			{
				chunks.Add(SubBlob(_data.Length - _data.Length%size));
			}

			return chunks;
		}

		//
		// Factory
		//

		public static Blob Random(int byteCount)
		{
			var data = new byte[byteCount];
            RandomNumberGenerator.Create().GetBytes(data);

			return new Blob(data);
		}

		public string ToBase64() => Convert.ToBase64String(_data);
		public static Blob FromBase64(string value) => Convert.FromBase64String(value);

		public string ToHex() => BitConverter.ToString(_data).Replace("-", "");
		public static Blob FromHex(string value) => Enumerable.Range(0, value.Length/2)
			.Select(x => Convert.ToByte(value.Substring(2*x, 2), 16))
			.ToArray();

		public static Blob Concat(IEnumerable<Blob> values) => Concat(values.ToArray());
		public static Blob Concat(params Blob[] values)
		{
			var blob = new byte[values.Sum(value => value.Length)];

			int offset = 0;
			for (int i = 0; i < values.Length; i++)
			{
				Array.Copy(values[i], 0, blob, offset, values[i].Length);
				offset += values[i].Length;
			}

			return blob;
		}
	}
}
