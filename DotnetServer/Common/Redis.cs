using System;
using StackExchange.Redis;

public class Redis
{
    private static ConnectionMultiplexer connAuth;
    private static ConnectionMultiplexer connResponse;

    static Redis()
    {
        connAuth = ConnectionMultiplexer.Connect(ServerConfig.RedisAuth);
        connResponse = ConnectionMultiplexer.Connect(ServerConfig.RedisResponse);
    }

    public static void Ping(bool verbose)
    {
        var elapsedTimeAuth = connAuth.GetDatabase().Ping();
        var elapsedTimeResponse = connResponse.GetDatabase().Ping();
    }
}