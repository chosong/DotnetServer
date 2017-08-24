namespace G.Util
{
	public class UrlEx
	{
		public string Command { get; private set; }
		public string[] Tokens { get; private set; }
		public DictionaryList<string, string> Params { get; private set; } = new DictionaryList<string, string>();

		public UrlEx(string url)
		{
			if (url == null) return;

			Tokens = url.TrimStart('/').Split('/', '?');
			Command = Tokens[0];

			foreach (var t in Tokens)
			{
				int index = t.IndexOf('=');
				if (index < 0) continue;

				string key = t.Substring(0, index);
				string[] values = t.Substring(index + 1).Split(',');

				foreach (var v in values)
				{
					Params.Add(key, v);
				}
			}
		}

		public static UrlEx Parse(string url)
		{
			return new UrlEx(url);
		}

		public static string ParseCommand(string url)
		{
			if (url == null) return null;

			string[] tokens = url.TrimStart('/').Split('/', '?');
			return tokens[0];
		}
	}
}
