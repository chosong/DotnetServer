using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace G.Util
{
	public class JsonEx
	{
		public static string SerializeForWeb(object obj)
		{
			var jo = JObject.FromObject(obj);
			ConvertIntegerToString(jo);
			return jo.ToString();
		}

		public static string SerializeForWebArrayObject(object obj)
		{
			var ja = JArray.FromObject(obj);
			ConvertIntegerToString(ja);
			return ja.ToString();
		}

		private static void ConvertIntegerToString(JToken jt)
		{
			if (jt == null) return;

			if (jt.HasValues)
			{
				int count = jt.Count();
				for (int i = 0; i < count; i++)
				{
					JToken token = jt.ElementAtOrDefault(i);
					ConvertIntegerToString(token);
				}
			}
			else if (jt.Type == JTokenType.Integer)
			{
				var newToken = new JValue(jt.ToString());
				jt.Replace(newToken);
			}
		}

		public static List<string> GetProperties(object obj)
		{
			var token = JToken.FromObject(obj);
			var list = new List<string>();
			GetChildren(list, token);
			return list;
		}

		private static void GetChildren(List<string> list, JToken token)
		{
			var children = token.Children().ToArray();
			if (children != null && children.Length > 0)
				foreach (var c in children) GetChildren(list, c);
			else
				list.Add(token.ToString());
		}


		public static object GetPropertiesToObject(object obj)
		{
			var token = JToken.FromObject(obj);
			GetChildrenToObject(token);
			return token.ToObject<object>();
		}

		private static void GetChildrenToObject(JToken token)
		{
			var children = token.Children().ToArray();
			if (children != null && children.Length > 0)
				foreach (var c in children) GetChildrenToObject(c);
			else
			{
				var newToken = new JValue(token.ToString());
				token.Replace(newToken);
			}
		}
	}
}
