using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace G.MySql
{
	public class DbExecutor
	{
		#region Synchronous
		public static int ExecuteNonQuery(string connectionString, string sql)
		{
			using (var conn = new MySqlConnection(connectionString))
			using (var cmd = new MySqlCommand(sql, conn))
			{
				conn.Open();
				return cmd.ExecuteNonQuery();
			}
		}

		public static object ExecuteScalar(string connectionString, string sql)
		{
			using (var conn = new MySqlConnection(connectionString))
			using (var cmd = new MySqlCommand(sql, conn))
			{
				conn.Open();
				return cmd.ExecuteScalar();
			}
		}

		public static DbReader ExecuteReader(string connectionString, string sql)
		{
			return DbReader.Create(connectionString, sql);
		}

		public static DbTransaction CreateDbTransaction(string connectionString)
		{
			return DbTransaction.Create(connectionString);
		}

		public static int ExecuteNonQuery(DbTransaction trxn, string sql)
		{
			using (var cmd = new MySqlCommand(sql, trxn.Connection))
			{
				return cmd.ExecuteNonQuery();
			}
		}

		public static object ExecuteScalar(DbTransaction trxn, string sql)
		{
			using (var cmd = new MySqlCommand(sql, trxn.Connection))
			{
				return cmd.ExecuteScalar();
			}
		}
		#endregion

		#region Asynchronous
		public static async Task<int> ExecuteNonQueryAsync(string connectionString, string sql)
		{
			using (var conn = new MySqlConnection(connectionString))
			using (var cmd = new MySqlCommand(sql, conn))
			{
				await conn.OpenAsync();
				return await cmd.ExecuteNonQueryAsync();
			}
		}

		public static async Task<object> ExecuteScalarAsync(string connectionString, string sql)
		{
			using (var conn = new MySqlConnection(connectionString))
			using (var cmd = new MySqlCommand(sql, conn))
			{
				await conn.OpenAsync();
				return await cmd.ExecuteScalarAsync();
			}
		}

		public static async Task<DbReader> ExecuteReaderAsync(string connectionString, string sql)
		{
			return await DbReader.CreateAsync(connectionString, sql);
		}

		public static async Task<DbTransaction> CreateDbTransactionAsync(string connectionString)
		{
			return await DbTransaction.CreateAsync(connectionString);
		}

		public static async Task<int> ExecuteNonQueryAsync(DbTransaction trxn, string sql)
		{
			using (var cmd = new MySqlCommand(sql, trxn.Connection))
			{
				return await cmd.ExecuteNonQueryAsync();
			}
		}

		public static async Task<object> ExecuteScalarAsync(DbTransaction trxn, string sql)
		{
			using (var cmd = new MySqlCommand(sql, trxn.Connection))
			{
				return await cmd.ExecuteScalarAsync();
			}
		}
		#endregion
	}
}
