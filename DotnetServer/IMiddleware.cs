using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public interface IMiddleware
{
    Task Invoke(HttpContext context);
}