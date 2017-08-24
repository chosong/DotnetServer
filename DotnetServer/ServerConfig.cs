using G.Util;

public class _ServerConfig
{
    public string ServerUrl { get; set; }
    public string EncryptionKey { get; set; }
    public string AdminDB { get; set; }
    public string CommonDB { get; set; }
    public string LogDB { get; set; }
    public string RedisAdmin { get; set; }
    public string RedisAuth { get; set; }
    public string RedisResponse { get; set; }
}

public class ServerConfig
{
    public static string ServerUrl { get; private set; }
    public static bool Encryption { get; private set; }
    public static uint Key1 { get; private set; }
	public static uint Key2 { get; private set; }
	public static uint Key3 { get; private set; }
	public static uint Key4 { get; private set; }
    public static string AdminDB { get; private set; }
    public static string CommonDB { get; private set; }
    public static string LogDB { get; private set; }
    public static string RedisAdmin { get; private set; }
    public static string RedisAuth { get; private set; }
    public static string RedisResponse { get; private set; }

    public static void Load(string filePath)
    {
        var config = JsonAppConfig.Load<_ServerConfig>(filePath, 4);

        ServerUrl = config.ServerUrl;

        uint[] keyArray = ConvertEx.ToKey(config.EncryptionKey);
        Key1 = keyArray[0];
        Key2 = keyArray[1];
        Key3 = keyArray[2];
        Key4 = keyArray[3];

        AdminDB = config.AdminDB;
        CommonDB = config.CommonDB;
        LogDB = config.LogDB;

        RedisAdmin = config.RedisAdmin;
        RedisAuth = config.RedisAuth;
        RedisResponse = config.RedisResponse;
    }
}ß