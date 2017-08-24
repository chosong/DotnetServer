using System;
using System.Collections.Generic;
using System.Text;

namespace G.Util
{
    public static class Randomizer
    {
		private static Random random;
		private static readonly char[] characters;

		static Randomizer()
		{
			random = new Random();

			StringBuilder sb = new StringBuilder();
			for (char ch = 'A'; ch <= 'Z'; ch++) sb.Append(ch);
			for (char ch = 'a'; ch <= 'z'; ch++) sb.Append(ch);
			for (char ch = '0'; ch <= '9'; ch++) sb.Append(ch);
			characters = sb.ToString().ToCharArray();
		}

		public static int Next() { return random.Next(); }
		public static int Next(int max) { return random.Next(max); }
		public static int Next(int min, int max) { return random.Next(min, max); }

		public static uint NextUInt32() { return (uint)random.Next(); }

		public static uint NextUInt32WithoutZero()
		{
			return (uint)NextWithoutZero();
		}

		public static int NextPInt32()
		{
			byte[] buffer = new byte[4];
			Randomizer.NextBytes(buffer);
			buffer[3] &= 0x7F;
			return BitConverter.ToInt32(buffer, 0);
		}

		public static long NextInt64()
		{
			byte[] buffer = new byte[8];
			Randomizer.NextBytes(buffer);
			return BitConverter.ToInt64(buffer, 0);
		}

		public static ulong NextUInt64()
		{
			byte[] buffer = new byte[8];
			Randomizer.NextBytes(buffer);
			return BitConverter.ToUInt64(buffer, 0);
		}

		public static long NextPInt64()
		{
			byte[] buffer = new byte[8];
			Randomizer.NextBytes(buffer);
			buffer[7] &= 0x7F;
			return BitConverter.ToInt64(buffer, 0);
		}

		public static int NextPositiveInt() { return random.Next(0, int.MaxValue); }
		public static int NextNegativeInt() { return random.Next(int.MinValue, 0); }

		public static int NextWithoutZero()
		{
			while (true)
			{
				int n = random.Next();
				if (n != 0) return n;
			}
		}

		public static void NextBytes(byte[] buffer) { random.NextBytes(buffer); }

		public static double NextDouble() { return random.NextDouble(); }

		public static string NextString(int length)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < length; i++)
				sb.Append(characters[Next(characters.Length)]);
			return sb.ToString();
		}

		public static string NextStringUpper(int length)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < length; i++)
				sb.Append((char)Next('A', 'Z' + 1));
			return sb.ToString();
		}

		public static string NextStringLower(int length)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < length; i++)
				sb.Append((char)Next('a', 'z' + 1));
			return sb.ToString();
		}

		public static void Shuffle<T>(T[] array)
		{
			for (int i = array.Length - 1; i >= 0; i--)
			{
				int index = random.Next(i + 1);
				T tmp = array[i];
				array[i] = array[index];
				array[index] = tmp;
			}
		}

		public static void Shuffle<T>(List<T> list)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				int index = random.Next(i + 1);
				T tmp = list[i];
				list[i] = list[index];
				list[index] = tmp;
			}
		}
	}
}
