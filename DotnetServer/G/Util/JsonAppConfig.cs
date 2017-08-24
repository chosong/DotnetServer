using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace G.Util
{
	public class JsonAppConfig
	{
		public static T Load<T>(string filePath, int retryParentDirectory = 2)
		{
			var appDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			string path = Path.Combine(appDirectory, filePath);

			path = FileEx.SearchParentDirectory(path, retryParentDirectory);
			if (path == null) return default(T);

			string json = File.ReadAllText(path);
			if (json == null)
				throw new FileNotFoundException(filePath);

			return JsonConvert.DeserializeObject<T>(json);
		}
	}
}
