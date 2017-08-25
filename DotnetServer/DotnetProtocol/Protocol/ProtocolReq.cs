using System;
using ProtoBuf;

namespace DotnetPJ
{
    [ProtoContract]
	public class ProtocolReq
	{
		[ProtoMember(1)] public long Suid { get; set; }
		[ProtoMember(2)] public long Ticks { get; set; }
		[ProtoMember(3)] public byte Retry { get; set; }
		[ProtoMember(4)] public Protocol Protocol { get; set; }

		private static byte sequence;

		public ProtocolReq() { }

		public ProtocolReq(long suid, Protocol protocol)
		{
			Suid = suid;
			Retry = 0;
			Protocol = protocol;

			MakeUniqueTicks();
		}

		private void MakeUniqueTicks()
		{
			byte[] bytes = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
			Buffer.BlockCopy(bytes, 0, bytes, 1, 7);
			bytes[0] = sequence;
			bytes[7] &= 0x7F;
			Ticks = BitConverter.ToInt64(bytes, 0);

			if (sequence < 0xff)
				sequence++;
			else
				sequence = 0;
		}
	}
}
