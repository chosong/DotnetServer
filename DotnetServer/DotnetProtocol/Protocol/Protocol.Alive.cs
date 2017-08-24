using ProtoBuf;

namespace DotnetProtocol
{
	[ProtoContract]
	public class AliveReq : Protocol
	{
		public AliveReq() : base(ProtocolId.Alive) {}
	}

	[ProtoContract]
	public class AliveRes : ProtocolRes
	{
		public AliveRes() : base(ProtocolId.Alive) {}
	}
}
