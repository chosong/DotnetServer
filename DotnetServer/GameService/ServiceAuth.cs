using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DotnetPJ
{
    public class ServiceAuth : IService
    {
        public async Task<ProtocolRes> ProcessAsync(HttpContext context, User user, Protocol request)
        {
            var req = (AuthReq)request;
            var res = new AuthRes();


            return res;
        }
    }
}