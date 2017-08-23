using ProtoBuf;

namespace DotnetProtocol
{
	[ProtoContract]
	public class AuthReq : Protocol
	{
		[ProtoMember(1)] public AccountType AccountType { get; set; }
		[ProtoMember(2)] public string AccountId { get; set; }			// NewUser:Knock!Knock! OR AccountId received from server
		[ProtoMember(3)] public string Nickname { get; set; }

		public AuthReq() : base(ProtocolId.Auth) {}
	}

	[ProtoContract]
	public class AuthRes : ProtocolRes
	{
		[ProtoMember(1)] public string AccountId { get; set; }
		[ProtoMember(2)] public long Suid { get; set; }
		[ProtoMember(3)] public uint Hash { get; set; }
		// ServerVariable
		[ProtoMember(5)] public int InventoryMaxValue { get; set; }

		public AuthRes() : base(ProtocolId.Auth) {}
	}
}
