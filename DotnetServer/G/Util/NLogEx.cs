using System.IO;
using System.Reflection;

namespace G.Util
{
	public class NLogEx
	{
		public static bool SetConfiguration(string filePath, int retryParentDirectory = 2)
		{
			string path = FileEx.SearchParentDirectory(filePath, retryParentDirectory);
			if (path == null)
				throw new FileNotFoundException(filePath);

			NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(path);

			return true;
		}

		public static bool SetAppConfiguration(string filePath, int retryParentDirectory = 2)
		{
			var appDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			filePath = Path.Combine(appDirectory, filePath);
			return SetConfiguration(filePath, retryParentDirectory);
		}
	}
}
