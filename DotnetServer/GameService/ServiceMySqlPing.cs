using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

#pragma warning disable 1998

namespace DotnetPJ
{
    public class ServiceRedisPing : IService
    {
        public async Task<ProtocolRes> ProcessAsync(HttpContext context, User user, Protocol request)
		{
            var req = (RedisPingReq) request;
            var res = new RedisPingRes();

			Redis.Ping(false);
            
			return res;
		}
    }
}