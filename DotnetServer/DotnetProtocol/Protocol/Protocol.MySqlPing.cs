using ProtoBuf;

namespace DotnetPJ
{
	[ProtoContract]
	public class MySqlPingReq : Protocol
	{
		public MySqlPingReq() : base(ProtocolId.MySqlPing) {}
	}

	[ProtoContract]
	public class MySqlPingRes : ProtocolRes
	{
		public MySqlPingRes() : base(ProtocolId.MySqlPing) {}
	}
}