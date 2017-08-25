using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

#pragma warning disable 1998

namespace DotnetPJ
{
    public class ServiceHandShake : IService
    {
        public async Task<ProtocolRes> ProcessAsync(HttpContext context, User user, Protocol request)
		{
			var req = (HandShakeReq)request;
			var res = new HandShakeRes();

            res.VersionOK = true;
            res.Key1 = ServerConfig.Key1;
            res.Key2 = ServerConfig.Key2;
            res.Key3 = ServerConfig.Key3;
            res.Key4 = ServerConfig.Key4;
        
			return res;
		}
    }
}