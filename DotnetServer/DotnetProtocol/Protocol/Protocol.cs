using ProtoBuf;

namespace DotnetPJ
{
	public enum ProtocolId
	{
		None = 0,
		Error = 99,
		HandShake = 101,
		Auth = 102,
		Alive = 103,

		RedisPing = 200,
		MySqlPing = 201,
	}

	[ProtoContract]
	
	[ProtoInclude(101, typeof(HandShakeReq))]
	[ProtoInclude(102, typeof(AuthReq))]
	[ProtoInclude(103, typeof(AliveReq))]
	[ProtoInclude(200, typeof(RedisPingReq))]
	[ProtoInclude(201, typeof(MySqlPingReq))]

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
	[ProtoInclude(200, typeof(RedisPingRes))]
	[ProtoInclude(201, typeof(MySqlPingRes))]

	public partial class ProtocolRes
	{
		[ProtoMember(1)] public ProtocolId ProtocolId { get; set; }
		[ProtoMember(2)] public Result Result { get; set; }

		public ProtocolRes() { }
		public ProtocolRes(ProtocolId protocolId) { ProtocolId = protocolId; }
	}
}
