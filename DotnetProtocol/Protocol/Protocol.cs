using ProtoBuf;

namespace DotnetProtocol
{
	public enum ProtocolId
	{
		None = 0,
		Error = 99,

		HandShake = 101,
		Auth = 102,
		Alive = 103,
	}

	[ProtoContract]
	
	[ProtoInclude(101, typeof(HandShakeReq))]
	[ProtoInclude(102, typeof(AuthReq))]
	[ProtoInclude(103, typeof(AliveReq))]

	public partial class Protocol
	{
		[ProtoMember(1)] public ProtocolId ProtocolId { get; set; }

		public Protocol() { }
		public Protocol(ProtocolId protocolId) { ProtocolId = protocolId; }
	}

	[ProtoContract]
	[ProtoInclude(99, typeof(ErrorRes))]
	[ProtoInclude(101, typeof(HandShakeRes))]
	[ProtoInclude(102, typeof(AuthRes))]
	[ProtoInclude(103, typeof(AliveRes))]

	public partial class ProtocolRes
	{
		[ProtoMember(1)] public ProtocolId ProtocolId { get; set; }
		[ProtoMember(2)] public Result Result { get; set; }

		public ProtocolRes() { }
		public ProtocolRes(ProtocolId protocolId) { ProtocolId = protocolId; }
	}
}
