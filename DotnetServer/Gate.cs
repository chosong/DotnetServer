using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProtoBuf;
using G.Util;
using DotnetPJ;

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
                    xxtea = new XXTea(new uint[] { hash ^ ServerConfig.Key1, hash ^ ServerConfig.Key2, hash ^ ServerConfig.Key3, hash ^ ServerConfig.Key4});

                }
                else if (encrypted == 1)
                {
                    xxtea = new XXTea(new uint[] { ServerConfig.Key1, ServerConfig.Key2, ServerConfig.Key3, ServerConfig.Key4 });
                    req = DecryptAndDeserialize(xxtea, inBuffer, 5, length - 5);
                    protocolId = req.Protocol.ProtocolId;

                    if (protocolId == ProtocolId.Auth) {}
                    else
                    {
                        throw new Exception("encrypted == 1 is allowed to only Protocol Auth");
                    }
                    
                }
                else if (encrypted == 0)
                {
                    req = Serializer.Deserialize<ProtocolReq>(stream);
                    protocolId = req.Protocol.ProtocolId;

                    if(protocolId == ProtocolId.HandShake) {}
                    else
                    {
                        throw new Exception("encrypted == 0 is allowed to Protocol HandShake");
                    }
                }
                else
                {
                    throw new Exception("Encrypted : Unavaiable Value");
                }
            }


            byte[] data = null;
            ProtocolRes response = await Service.ProcessAsync(context, req.Suid, hash, req.Protocol);
            if (response.ProtocolId == ProtocolId.Error)
            {
                encrypted = 0;
                xxtea = null;
            }

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(encrypted);

                if (xxtea != null)
                {
                    writer.Write(SerializeAndEncrypt(xxtea, response));
                }
                else
                {
                    Serializer.Serialize(stream, response);
                }

                data = stream.ToArray();

                httpResponse.ContentLength = data.Length;
                using(Stream outStream = httpResponse.Body)
                {
                    await outStream.WriteAsync(data, 0, data.Length);
                }
            }
        }
        catch(Exception ex)
        {

        }
    }
    public byte[] SerializeAndEncrypt(XXTea xxTea, ProtocolRes response)
    {
        try
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, response);
                return xxTea.Encrypt(ms.ToArray());
            }
        }
        catch(Exception ex)
        {
            return null;
        }
    }

    public ProtocolReq DecryptAndDeserialize(XXTea xxTea, byte[] buffer, int offset, int count)
    {
        try
        {
            byte[] decrpted = xxTea.Decrypt(buffer, offset, count);

            using (MemoryStream ms = new MemoryStream(decrpted, 0, decrpted.Length, true, true))
            {
                return Serializer.Deserialize<ProtocolReq>(ms);
            }
        }
        catch(Exception ex)
        {
            return null;
        }
    }
}