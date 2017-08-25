using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DotnetPJ
{
    public interface IService
    {
        Task<ProtocolRes> ProcessAsync(HttpContext context, User user, Protocol request);
    }

    public class Service
    {
        private static readonly Dictionary<ProtocolId, IService> dicServices = new Dictionary<ProtocolId, IService>
        {
            { ProtocolId.HandShake, new ServiceHandShake() },
            { ProtocolId.Auth, new ServiceAuth() }
        };

        public static async Task<ProtocolRes> ProcessAsync(HttpContext context, long suid, uint hash, Protocol request)
        {
            User user = null;

            if (request.ProtocolId <= ProtocolId.Auth) {}
            else
            {

            }

            IService service;
            if (dicServices.TryGetValue(request.ProtocolId, out service))
            {
                return await service.ProcessAsync(context, user, request);
            }
            else
            {
                return null;
            }
        }
    }
}