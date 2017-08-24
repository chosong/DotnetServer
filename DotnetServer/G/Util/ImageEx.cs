using System;

namespace G.Util
{
	public class ImageEx
	{
		private static readonly byte[] pngMagic = { 137, 80, 78, 71, 13, 10, 26, 10 };
		private static readonly byte[] jpgMagic = { 74, 70, 73, 70, 0 };

		public static bool IsPng(byte[] data)
		{
			try
			{
				for (int i = 0; i < pngMagic.Length; i++)
				{
					if (data[i] != pngMagic[i]) return false;
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool IsJpg(byte[] data)
		{
			try
			{
				for (int i = 0; i < jpgMagic.Length; i++)
				{
					if (data[i + 6] != jpgMagic[i]) return false;
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
