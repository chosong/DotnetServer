using System.Collections.Generic;

namespace G.Util
{
	public class Dictionary2<K1, K2, T> where T : class
	{
		private Dictionary<K1, Dictionary<K2, T>> dic = new Dictionary<K1, Dictionary<K2, T>>();

		public void Clear()
		{
			dic = new Dictionary<K1, Dictionary<K2, T>>();
		}

		public Dictionary<K2, T> Find(K1 k1)
		{
			Dictionary<K2, T> subDic = null;
			dic.TryGetValue(k1, out subDic);
			return subDic;
		}

		public T Find(K1 k1, K2 k2)
		{
			Dictionary<K2, T> subDic;
			if (dic.TryGetValue(k1, out subDic))
			{
				T t = null;
				subDic.TryGetValue(k2, out t);
				return t;
			}
			return null;
		}

		public void Add(K1 k1, K2 k2, T t)
		{
			Dictionary<K2, T> subDic = null;
			dic.TryGetValue(k1, out subDic);
			if (subDic == null)
			{
				subDic = new Dictionary<K2, T>();
				dic.Add(k1, subDic);
			}

			subDic[k2] = t;
		}
	}
}
