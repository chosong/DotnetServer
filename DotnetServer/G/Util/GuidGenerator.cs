using System;

namespace G.Util
{
	public class GuidGenerator
	{
		static public long Generate()
		{
			byte[] buffer = Guid.NewGuid().ToByteArray();
			return BitConverter.ToInt64(buffer, 0);
		}
	}
}

