using System;
using System.IO;
using System.Threading.Tasks;
using StackExchange.Redis;
using ProtoBuf;

namespace G.Util
{
	public class RedisEx
	{
		public static T Get<T>(ConnectionMultiplexer conn, string key)
		{
			var db = conn.GetDatabase();
			var data = db.StringGet(key);
			if (data.IsNull) return default(T);

			using (var stream = new MemoryStream((byte[])data))
			{
				return Serializer.Deserialize<T>(stream);
			}
		}

		public static async Task<T> GetAsync<T>(ConnectionMultiplexer conn, string key)
		{
			var db = conn.GetDatabase();
			var data = await db.StringGetAsync(key);
			if (data.IsNull) return default(T);

			using (var stream = new MemoryStream((byte[])data))
			{
				return Serializer.Deserialize<T>(stream);
			}
		}

		public static void Set<T>(ConnectionMultiplexer conn, string key, T t, TimeSpan? expiry = null)
		{
			using (var stream = new MemoryStream())
			{
				Serializer.Serialize(stream, t);

				var db = conn.GetDatabase();
				db.StringSet(key, stream.ToArray(), expiry);
			}
		}

		public static async Task SetAsync<T>(ConnectionMultiplexer conn, string key, T t, TimeSpan? expiry = null)
		{
			using (var stream = new MemoryStream())
			{
				Serializer.Serialize(stream, t);

				var db = conn.GetDatabase();
				await db.StringSetAsync(key, stream.ToArray(), expiry);
			}
		}

		public static bool Delete(ConnectionMultiplexer conn, string key)
		{
			var db = conn.GetDatabase();
			return db.KeyDelete(key);
		}

		public static async Task<bool> DeleteAsync(ConnectionMultiplexer conn, string key)
		{
			var db = conn.GetDatabase();
			return await db.KeyDeleteAsync(key);
		}
	}
}