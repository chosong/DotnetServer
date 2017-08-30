using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using G.Util;

public class ServerManager
{
    private enum ServerKind
    {
        Common = 1,
        Game = 2
    }

    private static readonly TimeSpan refreshTimeLimit = TimeSpan.FromSeconds(20);
    private static DateTime refreshTime = DateTime.MinValue;

    private static SemaphoreLock semaphoreCommon = new SemaphoreLock(10000);
    private static SemaphoreLock semaphoreGame = new SemaphoreLock(10000);

    private static List<TblServer> = new List<TblServer>();
    
}