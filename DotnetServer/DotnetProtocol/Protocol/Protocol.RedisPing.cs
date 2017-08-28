using ProtoBuf;

namespace DotnetPJ
{
	[ProtoContract]
	public class RedisPingReq : Protocol
	{
		public RedisPingReq() : base(ProtocolId.RedisPing) {}
	}

	[ProtoContract]
	public class RedisPingRes : ProtocolRes
	{
		public RedisPingRes() : base(ProtocolId.RedisPing) {}
	}
}