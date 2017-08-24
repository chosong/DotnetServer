using System;
using System.Text;

namespace G.Util
{
	// For Int64 (0 ~ 0x00FFFFFFFFFFFFFF)
	public sealed class SimpleEncrypter64
	{
		private static long seed = 0x007A5ABC6247A5B9L;

		private static readonly string codeTable = "23456789ABCDEFGHJKLMNPQRSTUVWXYZ";
		private static readonly byte[] codeTableReverse;

		static SimpleEncrypter64()
		{
			int num = 0;
			codeTableReverse = new byte[41];
			foreach (char ch in codeTable)
			{
				codeTableReverse[ch - 50] = (byte)num++;
			}
		}

		public static long Encrypt(long n)
		{
			if (n < 0 || n > 0x00FFFFFFFFFFFFFF) return -1;

			byte[] bytes = new byte[9];
			Buffer.BlockCopy(BitConverter.GetBytes(n), 0, bytes, 1, 8);
			bytes[0] = 0xB5;

			for (int i = 0; i < 7; i++)
			{
				bytes[i + 1] ^= bytes[i];
			}

			for (int i = 1; i < 8; i++)
			{
				bytes[i] = Exchange(bytes[i]);
			}

			n = BitConverter.ToInt64(bytes, 1);
			n ^= seed;

			return n;
		}

		public static long Decrypt(long n)
		{
			if (n < 0 || n > 0x00FFFFFFFFFFFFFF) return -1;
			n ^= seed;

			byte[] bytes = new byte[9];
			Buffer.BlockCopy(BitConverter.GetBytes(n), 0, bytes, 1, 8);
			bytes[0] = 0xB5;

			for (int i = 1; i < 8; i++)
			{
				bytes[i] = Exchange(bytes[i]);
			}

			for (int i = 6; i >= 0; i--)
			{
				bytes[i + 1] ^= bytes[i];
			}

			n = BitConverter.ToInt64(bytes, 1);

			return n;
		}

//		public static string ToString(long n)
//		{
//			string s = String.Format("{0:D017}", n);
//
//			StringBuilder sb = new StringBuilder(20);
//			sb.Append(s.Substring(0, 4));
//			sb.Append(' ');
//			sb.Append(s.Substring(4, 4));
//			sb.Append(' ');
//			sb.Append(s.Substring(8, 4));
//			sb.Append(' ');
//			sb.Append(s.Substring(12, 5));
//
//			return sb.ToString();
//		}
//
//		public static long ToInt64(string s)
//		{
//			return Int64.Parse(s.Replace(" ", ""));
//		}

		public static string ToString(long n)
		{
			if (n < 0 || n > 0x00FFFFFFFFFFFFFF) return  null;

			byte[] bytes = BitConverter.GetBytes(n);

			byte[] buffer = new byte[8];
			Buffer.BlockCopy(bytes, 0, buffer, 0, 7);

			int sum = 0;
			foreach (byte b in bytes)
			{
				sum += ((b >> 4) + b) & 0x0F;
			}
			buffer[7] = (byte)(sum << 4);

			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < 60; i += 5)
			{
				int index = i / 8;
				int rest = i % 8;
				int b;

				switch (rest)
				{
				case 0:
					b = (buffer[index] >> 3) & 0x1F;
					break;
				case 1:
					b = (buffer[index] >> 2) & 0x1F;
					break;
				case 2:
					b = (buffer[index] >> 1) & 0x1F;
					break;
				case 3:
					b = buffer[index] & 0x1F;
					break;
				case 4:
					b = ((buffer[index] << 1) | (buffer[index + 1] >> 7)) & 0x1F;
					break;
				case 5:
					b = ((buffer[index] << 2) | (buffer[index + 1] >> 6)) & 0x1F;
					break;
				case 6:
					b = ((buffer[index] << 3) | (buffer[index + 1] >> 5)) & 0x1F;
					break;
				case 7:
					b = ((buffer[index] << 4) | (buffer[index + 1] >> 4)) & 0x1F;
					break;
				default:
					b = 0;
					break;
				}

				sb.Append(codeTable[b]);
			}

			return sb.ToString();
		}

		public static long ToInt64(string s)
		{
			int length = s.Length;
			if (length != 12) throw new Exception("Wrong Format");

			byte[] buffer = new byte[8];

			int i = 0;
			foreach (char ch in s)
			{
				int n = codeTableReverse[ch - 50];

				int index = i / 8;
				int rest = i % 8;

				switch (rest)
				{
				case 0:
					buffer[index] |= (byte)(n << 3);
					break;
				case 1:
					buffer[index] |= (byte)(n << 2);
					break;
				case 2:
					buffer[index] |= (byte)(n << 1);
					break;
				case 3:
					buffer[index] |= (byte)n;
					break;
				case 4:
					buffer[index] |= (byte)(n >> 1);
					buffer[index + 1] |= (byte)(n << 7);
					break;
				case 5:
					buffer[index] |= (byte)(n >> 2);
					buffer[index + 1] |= (byte)(n << 6);
					break;
				case 6:
					buffer[index] |= (byte)(n >> 3);
					buffer[index + 1] |= (byte)(n << 5);
					break;
				case 7:
					buffer[index] |= (byte)(n >> 4);
					buffer[index + 1] |= (byte)(n << 4);
					break;
				default:
					throw new Exception("Out of the Range");
				}

				i += 5;
			}

			int sum = 0;
			for (int j = 0; j < 7; j++)
			{
				byte b = buffer[j];
				sum += ((b >> 4) + (b & 0x0F));
			}

			byte b1 = (byte)(buffer[7] >> 4);
			byte b2 = (byte)(sum & 0x0F);
			if (b1 != b2)
				throw new Exception("Wrong Checksum");

			buffer[7] = 0;
			return BitConverter.ToInt64(buffer, 0);
		}

		private static byte Exchange(byte n)
		{
			return (byte)(((n & 0xF0) >> 4) | ((n & 0X0F) << 4));
		}
	}

	// For UInt32
	public sealed class SimpleEncrypter32
	{
		private static long seed = 0x000000006247A5B9L;

		public static long Encrypt(uint n)
		{
			byte[] bytes = new byte[9];
			Buffer.BlockCopy(BitConverter.GetBytes(n), 0, bytes, 1, 4);
			bytes[0] = 0xB5;

			for (int i = 0; i < 4; i++)
			{
				bytes[i + 1] ^= bytes[i];
			}

			for (int i = 1; i < 5; i++)
			{
				bytes[i] = Exchange(bytes[i]);
			}

			long result = BitConverter.ToInt64(bytes, 1);
			result ^= seed;

			return result;
		}

		public static uint Decrypt(long n)
		{
			n ^= (uint)(seed & 0xFFFFFFFFU);

			byte[] bytes = new byte[9];
			Buffer.BlockCopy(BitConverter.GetBytes(n), 0, bytes, 1, 4);
			bytes[0] = 0xB5;

			for (int i = 1; i < 5; i++)
			{
				bytes[i] = Exchange(bytes[i]);
			}

			for (int i = 3; i >= 0; i--)
			{
				bytes[i + 1] ^= bytes[i];
			}

			return BitConverter.ToUInt32(bytes, 1);
		}

		public static string ToString(long n)
		{
			string s = String.Format("{0:D010}", n);

			StringBuilder sb = new StringBuilder(12);
			sb.Append(s.Substring(0, 3));
			sb.Append(' ');
			sb.Append(s.Substring(3, 3));
			sb.Append(' ');
			sb.Append(s.Substring(6, 4));

			return sb.ToString();
		}

		public static long ToUInt32(string s)
		{
			return UInt32.Parse(s.Replace(" ", ""));
		}

		private static byte Exchange(byte n)
		{
			return (byte)(((n & 0xF0) >> 4) | ((n & 0X0F) << 4));
		}
	}
}
