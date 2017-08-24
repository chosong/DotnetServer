using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class Test : IMiddleware
{
    public async Task Invoke(HttpContext context)
    {
        using(var write = new StreamWriter(context.Response.Body))
        {
            await write.WriteLineAsync("Hi I'm Test Page");
        }
    }
}