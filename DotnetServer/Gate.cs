using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProtoBuf;
using G.Util;
using DotnetProtocol;

public class Gate : IMiddleware
{
    public async Task Invoke(HttpContext context)
    {
        ProtocolId protocolId = ProtocolId.None;

        try
        {
            var httpRequest = context.Request;
            var httpResponse = context.Response;

            XXTea xxtea = null;
            ProtocolReq req = null;
            byte encrypted;
            uint hash;

            int length = (int)httpRequest.ContentLength;
            byte[] inBuffer = new byte[length];

            using (var inStream = httpRequest.Body)
            using (var stream = new MemoryStream(inBuffer))
            using (var reader = new BinaryReader(stream))
            {
                await inStream.CopyToAsync(stream);
                stream.Position = 0;

                encrypted = reader.ReadByte();
                hash = reader.ReadUInt32();

                if (encrypted == 2)
                {
                    xxtea = new XXTea(hash ^)

                }
            }

        }
        catch(Exception ex)
        {

        }
        using(var write = new StreamWriter(context.Response.Body))
        {
            await write.WriteLineAsync("Hi I'm Gate Page");
        }
    }
}