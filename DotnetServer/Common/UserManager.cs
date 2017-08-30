using System;
using System.Threading.Tasks;
using G.Util;

public class User
{
    public static async Task<User> CreateAsync(AccountType accountType)
    {
        for (int i = 0; i < 100; i++)
        {
            var acc = Account.New;
            var suid = acc.Suid;
            var account = acc.ToString();
            var password = Account.New.ToString();

            int gameServerId = await Serverma
        }
    }
}