using System;
using System.Text;
using System.Net;
using Newtonsoft.Json;

namespace G.Web
{
	public class Result
	{
		public HttpStatusCode StatusCode { get; set; }
		public byte[] Response { get; set; }

		public string ResponseString
		{
			get
			{
				try
				{
					return Encoding.UTF8.GetString(Response);
				}
				catch (Exception)
				{
					return null;
				}
			}
		}

		public Result() { }

		public Result(HttpStatusCode statusCode, byte[] response)
		{
			StatusCode = statusCode;
			Response = response;
		}

		public Result(Result result)
		{
			StatusCode = result.StatusCode;
			Response = result.Response;
		}

		public virtual void Set(HttpStatusCode statusCode, byte[] response)
		{
			StatusCode = statusCode;
			Response = response;
		}

		public virtual void Set(Result result)
		{
			StatusCode = result.StatusCode;
			Response = result.Response;
		}

		public T ToObject<T>()
		{
			try
			{
				string json = Encoding.UTF8.GetString(Response);
				return JsonConvert.DeserializeObject<T>(json);
			}
			catch (Exception)
			{
				return default(T);
			}
		}

		public override string ToString()
		{
			return string.Format("StatusCode={0}, Response={1}", StatusCode, ResponseString);
		}
	}
}
