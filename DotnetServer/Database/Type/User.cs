using ProtoBuf;
using DotnetPJ;

[ProtoContract]
public class User
{
	[ProtoMember(1)] public long Suid { get; set; }
	[ProtoMember(2)] public AccountType AccountType { get; set; }
	[ProtoMember(3)] public string AccountId { get; set; }
	[ProtoMember(4)] public string OnlineId { get; set; }
	[ProtoMember(5)] public int GameServerId { get; set; }
	[ProtoMember(6)] public uint Hash { get; set; }

	[ProtoIgnore] public long PlayerId { get { return Suid; } }

	public User() {}

	public User(long suid, AccountType accountType, string accountId, string onlineId, int gameServerId, uint hash)
	{
		Suid = suid;
		AccountType = accountType;
		AccountId = accountId;
		OnlineId = onlineId;
		GameServerId = gameServerId;
		Hash = hash;
	}
}
