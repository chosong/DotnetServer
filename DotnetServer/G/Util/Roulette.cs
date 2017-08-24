using System.Collections.Generic;

namespace G.Util
{
	public interface IRoulette
	{
		int Probability { get; }
	}

	public class Roulette<T>
	{
		private readonly List<T> listKey = new List<T>();
		private readonly List<int> listValue = new List<int>();
		private int totalValue;

		public Roulette() {}

		public Roulette(List<T> list)
		{
			Add(list);
		}

		public Roulette(T[] array)
		{
			Add(array);
		}

		public Roulette(HashSet<T> set)
		{
			Add(set);
		}

		public int Count
		{
			get
			{
				lock (this) { return listKey.Count; }
			}
		}

		public T[] Keys
		{
			get
			{
				lock (this) { return listKey.ToArray(); }
			}
		}

		public int[] Values
		{
			get
			{
				lock (this) { return listValue.ToArray(); }
			}
		}

		public KeyValuePair<T, int> this[int i]
		{
			get
			{
				lock (this) { return new KeyValuePair<T, int>(listKey[i], listValue[i]); }
			}
		}

		public void Clear()
		{
			lock (this)
			{
				listKey.Clear();
				listValue.Clear();
				totalValue = 0;
			}
		}

		private void _Add(T key, int value)
		{
			if (value < 0) return;
			totalValue += value;

			listKey.Add(key);
			listValue.Add(value);
		}

		public void Add(T key, int value)
		{
			lock (this)
			{
				_Add(key, value);
			}
		}

		public void Add(T obj)
		{
			lock (this)
			{
				_Add(obj, ((IRoulette)obj).Probability);
			}
		}

		public void Add(List<T> list)
		{
			lock (this)
			{
				foreach (var item in list)
				{
					_Add(item, ((IRoulette)item).Probability);
				}
			}
		}

		public void Add(T[] array)
		{
			lock (this)
			{
				foreach (var item in array)
				{
					_Add(item, ((IRoulette)item).Probability);
				}
			}
		}

		public void Add(HashSet<T> set)
		{
			lock (this)
			{
				foreach (var item in set)
				{
					_Add(item, ((IRoulette)item).Probability);
				}
			}
		}

		public void RemoveAt(int index)
		{
			lock (this)
			{
				if (index >= listKey.Count) return;
				totalValue -= listValue[index];

				listKey.RemoveAt(index);
				listValue.RemoveAt(index);
			}
		}

		public T GetNext(bool remove = false)
		{
			lock (this)
			{
				int n = Randomizer.Next(totalValue);
				int count = listKey.Count;
				int sum = 0;

				for (int i = 0; i < count; i++)
				{
					sum += listValue[i];
					if (n < sum)
					{
						T key = listKey[i];
						if (remove) RemoveAt(i);
						return key;
					}
				}

				return default(T);
			}
		}

		public IEnumerator<KeyValuePair<T, int>> GetEnumerator()
		{
			lock (this)
			{
				int count = listKey.Count;
				for (int i = 0; i < count; i++)
				{
					yield return new KeyValuePair<T, int>(listKey[i], listValue[i]);
				}
			}
		}

		public Roulette<T> Clone()
		{
			Roulette<T> newRoulette = new Roulette<T>();
			newRoulette.listKey.AddRange(listKey);
			newRoulette.listValue.AddRange(listValue);
			newRoulette.totalValue = totalValue;

			return newRoulette;
		}

		public void IncreaseProbability(int percent, params T[] exceptedKeys)
		{
			int count = listValue.Count;

			for (int i = 0; i < count; i++)
			{
				foreach (T t in exceptedKeys)
				{
					if (t.Equals(listKey[i])) goto label1;
				}

				int n = listValue[i] * percent / 100;
				listValue[i] += n;
				totalValue += n;

				label1:;
			}
		}
	}
}
