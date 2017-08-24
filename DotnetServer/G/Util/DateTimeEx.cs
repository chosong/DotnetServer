using System;
using System.Globalization;

namespace G.Util
{
	public static class DateTimeEx
	{
		public static long TicksForMillisecond { get; } = 10000;
		public static long TicksForSecond { get; } = 10000000;
		public static long TicksForMinute { get; } = 600000000;
		public static long TicksForHour { get; } = 36000000000;

		private static readonly DateTime dt1970 = new DateTime(1970, 1, 1);

		public static int ToUnixTime(DateTime dt)
		{
			return (int)(dt - dt1970).TotalSeconds;
		}

		public static DateTime FromUnixTime(int unixTime)
		{
			return dt1970 + TimeSpan.FromSeconds(unixTime);
		}

		public static string ToRfc3339(DateTime dt)
		{
			return dt.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffZ");
		}
		
		public static string ToRfc3339Modified(DateTime dt)
		{
			return dt.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ssZ");
		}

		public static DateTime FromRfc3339(string dt)
		{
			return DateTime.ParseExact(dt, "yyyy-MM-dd'T'HH:mm:ss.fffZ", CultureInfo.InvariantCulture);
		}

		public static bool IsSameYear(DateTime dt1, DateTime dt2)
		{
			return (dt1.Year == dt2.Year);
		}

		public static bool IsSameMonth(DateTime dt1, DateTime dt2)
		{
			return (dt1.Year == dt2.Year) && (dt1.Month == dt2.Month);
		}

		public static bool IsSameWeek(DateTime dt1, DateTime dt2)
		{
			if (dt1 == default(DateTime) || dt2 == default(DateTime)) return false;
			
			DateTime date1 = dt1.Date;
			DateTime date2 = dt2.Date;
			DateTime week1 = date1 - TimeSpan.FromDays((int)date1.DayOfWeek);
			DateTime week2 = date2 - TimeSpan.FromDays((int)date2.DayOfWeek);
			return (week1 == week2);
		}

		public static bool IsSameDay(DateTime dt1, DateTime dt2)
		{
			return (dt1.Date == dt2.Date);
		}

		public static bool IsSameHour(DateTime dt1, DateTime dt2)
		{
			return (dt1.Date == dt2.Date) && (dt1.Hour == dt2.Hour);
		}

		public static bool IsSameMinute(DateTime dt1, DateTime dt2)
		{
			return (dt1.Ticks / TicksForMinute * TicksForMinute) == (dt2.Ticks / TicksForMinute * TicksForMinute);
		}

		public static bool IsSameSecond(DateTime dt1, DateTime dt2)
		{
			return (dt1.Ticks / TicksForSecond * TicksForSecond) == (dt2.Ticks / TicksForSecond * TicksForSecond);
		}

		public static DateTime GetNextYear(DateTime dt)
		{
			return new DateTime(dt.Year + 1, 1, 1);
		}

		public static DateTime GetNextMonth(DateTime dt)
		{
			dt = dt.AddMonths(1);
			return new DateTime(dt.Year, dt.Month, 1);
		}

		public static DateTime GetNextWeek(DateTime dt)
		{
			return dt.Date.AddDays(7 - (int)dt.DayOfWeek);
		}

		public static DateTime GetNextDay(DateTime dt)
		{
			return dt.Date.AddDays(1);
		}

		public static DateTime GetNextHour(DateTime dt)
		{
			dt = dt.AddHours(1);
			return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
		}

		public static DateTime FilterSecond(this DateTime dt)
		{
			return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
		}

		public static DateTime FilterMinute(this DateTime dt)
		{
			return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
		}
	}
}
