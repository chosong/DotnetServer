using System;
using System.IO;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using G.Util;

namespace G.Web
{
	public class Http
	{
		//private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

		public static async Task<Result> RequestAsync(
			string method,
			string url,
			NameValueCollection queryParameters = null,
			WebHeaderCollection headers = null,
			byte[] body = null,
			string contentType = "application/x-www-form-urlencoded",
			XXTea xxtea = null
		)
		{
			string parameterString = GetParameterString(queryParameters, true);

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + parameterString);
			request.Method = method;

			if (headers != null && headers.Count > 0)
			{
				foreach (var key in headers.AllKeys)
					request.Headers[key] = headers[key];
			}

			if (body != null && body.Length > 0)
			{
				request.ContentType = contentType;

				using (Stream reqStream = await request.GetRequestStreamAsync())
				{
					byte[] reqBytes;
					if (xxtea != null)
						reqBytes = xxtea.Encrypt(body);
					else
						reqBytes = body;

					await reqStream.WriteAsync(reqBytes, 0, reqBytes.Length);
				}
			}

			using (HttpWebResponse response = (HttpWebResponse)await GetResponseWithoutExceptionAsync(request))
			using (Stream resStream = response.GetResponseStream())
			using (MemoryStream stream = new MemoryStream())
			{
				await resStream.CopyToAsync(stream);
				byte[] data = stream.ToArray();

				byte[] result;
				if (xxtea != null)
					result = xxtea.Decrypt(data, 0, data.Length);
				else
					result = data;

				return new Result()
				{
					StatusCode = response.StatusCode,
					Response = result
				};
			}
		}

		private static async Task<WebResponse> GetResponseWithoutExceptionAsync(WebRequest request)
		{
			try
			{
				return await request.GetResponseAsync();
			}
			catch (WebException ex)
			{
				return ex.Response;
			}
		}

		public static async Task<Result> GetAsync(string url, NameValueCollection queryParameters = null, WebHeaderCollection headers = null)
		{
			return await RequestAsync("GET", url, queryParameters, headers);
		}

		public static async Task<Result> PostAsync(string url, NameValueCollection queryParameters = null, WebHeaderCollection headers = null, byte[] body = null, string contentType = null, XXTea xxtea = null)
		{
			if (contentType == null) contentType = "application/x-www-form-urlencoded";
			return await RequestAsync("POST", url, queryParameters, headers, body, contentType, xxtea);
		}

		public static async Task<Result> PutAsync(string url, NameValueCollection queryParameters = null, WebHeaderCollection headers = null, byte[] body = null, string contentType = null)
		{
			if (contentType == null) contentType = "application/x-www-form-urlencoded";
			return await RequestAsync("PUT", url, queryParameters, headers, body, contentType);
		}

		public static async Task<Result> DeleteAsync(string url, NameValueCollection queryParameters = null, WebHeaderCollection headers = null)
		{
			return await RequestAsync("DELETE", url, queryParameters, headers);
		}

		private static string GetParameterString(NameValueCollection queryParameters, bool withQuestion)
		{
			if (queryParameters == null || queryParameters.Count == 0)
				return String.Empty;

			StringBuilder sb = new StringBuilder();

			bool isFirst = true;
			foreach (var key in queryParameters.AllKeys)
			{
				if (isFirst)
				{
					isFirst = false;
					if (withQuestion) sb.Append("?");
				}
				else
					sb.Append("&");

				sb.Append(key + "=" + queryParameters.Get(key));
			}

			return sb.ToString();
		}
	}
}
