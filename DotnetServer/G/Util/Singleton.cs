namespace G.Util
{
	public class Singleton<T> where T : class, new()
	{
		private static T t = null;
		
		public static T Instance
		{
			get
			{
				lock (typeof(T))
				{
					if (t == null)
					{
						t = new T();
					}
					return t;
				}
			}
		}
	}
}
