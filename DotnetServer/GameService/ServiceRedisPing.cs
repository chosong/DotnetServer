using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

#pragma warning disable 1998

namespace DotnetPJ
{
    public class ServiceMySqlPing : IService
    {
        public async Task<ProtocolRes> ProcessAsync(HttpContext context, User user, Protocol request)
		{
			var req = (MySqlPingReq)request;
			var res = new MySqlPingRes();
        
			return res;
		}
    }
}