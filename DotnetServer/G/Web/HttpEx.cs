using System;
using Microsoft.AspNetCore.Http;

namespace G.Web
{
	public class HttpEx
	{
		public static string GetRemoteIp(HttpContext context)
		{
			try
			{
				return context.Request.Headers["X-Forwarded-For"][0];
			}
			catch (Exception)
			{
				return context.Connection.RemoteIpAddress.ToString();
			}
		}
	}
}