using ProtoBuf;

namespace DotnetProtocol
{
	[ProtoContract]
	public class HandShakeReq : Protocol
	{
		[ProtoMember(1)] public ushort[] Version { get; set; }
		[ProtoMember(2)] public ushort[] ProtocolVersion { get; set; }

		public HandShakeReq() : base(ProtocolId.HandShake) {}
	}

	[ProtoContract]
	public class HandShakeRes : ProtocolRes
	{
		[ProtoMember(1)] public bool VersionOK { get; set; }
		[ProtoMember(2)] public uint Key1 { get; set; }
		[ProtoMember(3)] public uint Key2 { get; set; }
		[ProtoMember(4)] public uint Key3 { get; set; }
		[ProtoMember(5)] public uint Key4 { get; set; }
		[ProtoMember(6)] public string PatchServerUrl { get; set; }
		[ProtoMember(7)] public ushort[] Version { get; set; }

		public HandShakeRes() : base(ProtocolId.HandShake) {}
	}
}