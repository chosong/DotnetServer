using System;
using System.Text;
using System.IO;

namespace G.Util
{
	public class XXTea
	{
		private static readonly uint DELTA = 0x9e3779b9;

		private uint[] k;
		public uint[] Key { get { return k; } }

		public XXTea()
		{
			SetKey();
		}

		public XXTea(uint[] key)
		{
			SetKey(key);
		}

		public XXTea(string key)
		{
			SetKey(key);
		}

		public void SetKey()
		{
			k = new uint[4];

			Random random = new Random();
			k[0] = (uint)random.Next();
			k[1] = (uint)random.Next();
			k[2] = (uint)random.Next();
			k[3] = (uint)random.Next();
		}

		public void SetKey(uint[] key)
		{
			int len = key.Length;
			int fixedLen = 4;

			k = new uint[fixedLen];
			for (int i = 0; i < fixedLen; i++)
			{
				if (i < len)
					k[i] = key[i];
				else
					k[i] = 0;
			}
		}

		public bool SetKey(string base62Key)
		{
			if (base62Key == null) return false;

			try
			{
				SetKey(ConvertEx.ToKey(base62Key));
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		public byte[] Encrypt(byte[] data, int offset, int count)
		{
			uint[] v = new uint[(int)Math.Ceiling((double)(count + 4) / 4)];
			v[0] = (uint)count;
			Buffer.BlockCopy(data, offset, v, 4, count);

			_Encrypt(v, k);

			byte[] enc = new byte[v.Length * 4];
			Buffer.BlockCopy(v, 0, enc, 0, enc.Length);

			return enc;
		}

		public byte[] Decrypt(byte[] data, int offset, int count)
		{
			uint[] v = new uint[(int)Math.Ceiling((double)count / 4)];
			Buffer.BlockCopy(data, offset, v, 0, count);

			_Decrypt(v, k);

			byte[] dec = new byte[v[0]];
			Buffer.BlockCopy(v, 4, dec, 0, dec.Length);

			return dec;
		}

		public byte[] Encrypt(byte[] data)
		{
			return Encrypt(data, 0, data.Length);
		}

		public byte[] Decrypt(byte[] data)
		{
			return Decrypt(data, 0, data.Length);
		}

		public byte[] Encrypt(string text, Encoding encoding)
		{
			byte[] data = encoding.GetBytes(text);
			return Encrypt(data, 0, data.Length);
		}

		public string Decrypt(byte[] data, Encoding encoding)
		{
			byte[] decrypted = Decrypt(data, 0, data.Length);
			return encoding.GetString(decrypted);
		}

		public string Decrypt(byte[] data, int offset, int count, Encoding encoding)
		{
			byte[] decrypted = Decrypt(data, offset, count);
			return encoding.GetString(decrypted);
		}

		public void EncryptToFile(string path, byte[] data)
		{
			byte[] encrypted = Encrypt(data, 0, data.Length);
			File.WriteAllBytes(path, encrypted);
		}

		public void EncryptToFile(string path, byte[] data, int offset, int count)
		{
			byte[] encrypted = Encrypt(data, offset, count);
			File.WriteAllBytes(path, encrypted);
		}

		public byte[] DecryptFromFile(string path)
		{
			byte[] encrypted = File.ReadAllBytes(path);
			return Decrypt(encrypted, 0, encrypted.Length);
		}

		public string EncryptString(string text)
		{
			if (text == null) return null;
			return Convert.ToBase64String(Encrypt(text, Encoding.UTF8));
		}

		public string DecryptString(string text)
		{
			if (text == null) return null;
			return Decrypt(Convert.FromBase64String(text), Encoding.UTF8);
		}

		private static void _Encrypt(uint[] v, uint[] k)
		{
			int n = v.Length;
			uint y = 0, sum = 0, p = 0, e = 0;
			uint rounds = (uint)(6 + 52 / n);
			uint z = v[n - 1];

			do
			{
				sum += DELTA;
				e = (sum >> 2) & 3;
				for (p = 0; p < n - 1; p++)
				{
					y = v[p + 1];
					z = v[p] += (((z >> 5 ^ y << 2) + (y >> 3 ^ z << 4)) ^ ((sum ^ y) + (k[(p & 3) ^ e] ^ z)));
				}
				y = v[0];
				z = v[n - 1] += (((z >> 5 ^ y << 2) + (y >> 3 ^ z << 4)) ^ ((sum ^ y) + (k[(p & 3) ^ e] ^ z)));
			} while (--rounds > 0);
		}

		private static void _Decrypt(uint[] v, uint[] k)
		{
			int n = v.Length;
			uint z = 0, p = 0, e = 0;
			uint rounds = (uint)(6 + 52 / n);
			uint sum = rounds * DELTA;
			uint y = v[0];

			do
			{
				e = (sum >> 2) & 3;
				for (p = (uint)(n - 1); p > 0; p--)
				{
					z = v[p - 1];
					y = v[p] -= (((z >> 5 ^ y << 2) + (y >> 3 ^ z << 4)) ^ ((sum ^ y) + (k[(p & 3) ^ e] ^ z)));
				}
				z = v[n - 1];
				y = v[0] -= (((z >> 5 ^ y << 2) + (y >> 3 ^ z << 4)) ^ ((sum ^ y) + (k[(p & 3) ^ e] ^ z)));
			} while ((sum -= DELTA) != 0);
		}
	}
}
