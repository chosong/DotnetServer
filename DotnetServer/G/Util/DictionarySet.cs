using System.Collections.Generic;

namespace G.Util
{
	public class DictionarySet<K, T>
	{
		private Dictionary<K, HashSet<T>> dic = new Dictionary<K, HashSet<T>>();

		public Dictionary<K, HashSet<T>>.KeyCollection Keys
		{
			get { return dic.Keys; }
		}

		public Dictionary<K, HashSet<T>>.ValueCollection Values
		{
			get { return dic.Values; }
		}

		public int Count { get { return dic.Count; } }

		public void Clear()
		{
			dic = new Dictionary<K, HashSet<T>>();
		}

		public HashSet<T> Find(K k)
		{
			HashSet<T> set;
			if (dic.TryGetValue(k, out set))
				return set;
			else
				return null;
		}

		public bool Contains(K k, T t)
		{
			HashSet<T> set;
			if (dic.TryGetValue(k, out set))
				return set.Contains(t);
			else
				return false;
		}

		public int Contains(K k, T[] array)
		{
			int count = 0;

			HashSet<T> set;
			if (dic.TryGetValue(k, out set))
			{
				foreach (var t in array)
				{
					if (set.Contains(t)) count++;
				}
			}

			return count;
		}

		public int Contains(K k, List<T> list)
		{
			int count = 0;

			HashSet<T> set;
			if (dic.TryGetValue(k, out set))
			{
				foreach (var t in list)
				{
					if (set.Contains(t)) count++;
				}
			}

			return count;
		}

		public void Add(K k, T t)
		{
			HashSet<T> set;
			if (dic.TryGetValue(k, out set) == false)
			{
				set = new HashSet<T>();
				dic.Add(k, set);
			}

			set.Add(t);
		}

		public bool Remove(K k)
		{
			return dic.Remove(k);
		}

		public bool Remove(K k, T t)
		{
			HashSet<T> set;
			if (dic.TryGetValue(k, out set) == false)
				return false;

			if (set.Remove(t) == false)
				return false;

			if (set.Count == 0)
				dic.Remove(k);

			return true;
		}
	}
}
