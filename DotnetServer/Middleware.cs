using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class Middleware
{
    private readonly RequestDelegate next;
    private readonly Dictionary<string, IMiddleware> routes = new Dictionary<string, IMiddleware>
    {
        { "/Gate", new Gate() },
        { "/Test", new Test() }
    };
    public Middleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        IMiddleware middleware = null;

        if(routes.TryGetValue(context.Request.Path.Value, out middleware))
            await middleware.Invoke(context);
        else
            await next(context);
    }
}