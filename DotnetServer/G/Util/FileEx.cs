using System.IO;

namespace G.Util
{
	public class FileEx
	{
		public static string SearchParentDirectory(string filePath, int retryParentDirectory = 2)
		{
			if (File.Exists(filePath))
				return filePath;

			string dir = Path.GetDirectoryName(filePath);
			string fileName = Path.GetFileName(filePath);

			for (int i = 1; i <= retryParentDirectory; i++)
			{
				dir = Path.Combine(dir, "..");
				filePath = Path.Combine(dir, fileName);

				if (File.Exists(filePath))
					return filePath;
			}

			return null;
		}
	}
}
