using System;
using System.Collections.Generic;

namespace G.Util
{
	public class ListEx
	{
		public static bool IsUnique<T>(List<T> list)
		{
			HashSet<T> set = new HashSet<T>();
			foreach (var i in list)
			{
				if (set.Contains(i)) return false;
				set.Add(i);
			}
			return true;
		}

		public static bool IsUnique<T>(T[] array)
		{
			HashSet<T> set = new HashSet<T>();
			foreach (var i in array)
			{
				if (set.Contains(i)) return false;
				set.Add(i);
			}
			return true;
		}

		public static bool IsUnique<T>(List<T> list, T availableMin) where T : struct, IComparable, IComparable<T>, IEquatable<T>
		{
			HashSet<T> set = new HashSet<T>();
			foreach (var i in list)
			{
				if (i.CompareTo(availableMin) < 0) continue;
				if (set.Contains(i)) return false;
				set.Add(i);
			}
			return true;
		}

		public static bool IsUnique<T>(T[] list, T availableMin) where T : struct, IComparable, IComparable<T>, IEquatable<T>
		{
			HashSet<T> set = new HashSet<T>();
			foreach (var i in list)
			{
				if (i.CompareTo(availableMin) < 0) continue;
				if (set.Contains(i)) return false;
				set.Add(i);
			}
			return true;
		}
	}
}
