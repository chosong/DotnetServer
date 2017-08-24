using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;
using G.MySql;

namespace G.Log
{
	public class LogBase<T> : Log
	{
		protected static MySqlTableInfo tableInfo;
		protected static string sqlToInsert;
		protected static string sqlToUpsert;

		[MySqlField(int.MaxValue, "time", "datetime")]
		public DateTime Time { get; set; }

		public LogBase()
		{
			Time = DateTime.UtcNow;
		}

		static LogBase()
		{
			Initialize();
		}

		public static void Initialize()
		{
			using (var conn = new MySqlConnection(ConnectionString))
			{
				Initialize(conn);
			}
		}

		public static void Initialize(MySqlConnection conn)
		{
			tableInfo = MySqlDapper.ParseTable(typeof(T));
			sqlToInsert = MySqlDapper.GetSqlToInsert(tableInfo);
			sqlToUpsert = MySqlDapper.GetSqlToUpsert(tableInfo);

			string sqlToCreate = MySqlDapper.GetSqlToCreate(tableInfo);
			conn.Execute(sqlToCreate);
		}

		public bool Insert()
		{
			using (var conn = new MySqlConnection(ConnectionString))
			{
				int result = conn.Execute(sqlToInsert, this);
				return (result > 0);
			}
		}

		public bool Insert(MySqlConnection conn)
		{
			int result = conn.Execute(sqlToInsert, this);
			return (result > 0);
		}

		public bool Insert(DbTransaction dbt)
		{
			int result = dbt.Connection.Execute(sqlToInsert, this, dbt.Transaction);
			return (result > 0);
		}

		public async Task<bool> InsertAsync()
		{
			using (var conn = new MySqlConnection(ConnectionString))
			{
				int result = await conn.ExecuteAsync(sqlToInsert, this);
				return (result > 0);
			}
		}

		public async Task<bool> InsertAsync(MySqlConnection conn)
		{
			int result = await conn.ExecuteAsync(sqlToInsert, this);
			return (result > 0);
		}

		public async Task<bool> InsertAsync(DbTransaction dbt)
		{
			int result = await dbt.Connection.ExecuteAsync(sqlToInsert, this, dbt.Transaction);
			return (result > 0);
		}

		public bool Upsert()
		{
			using (var conn = new MySqlConnection(ConnectionString))
			{
				int result = conn.Execute(sqlToUpsert, this);
				return (result > 0);
			}
		}

		public bool Upsert(MySqlConnection conn)
		{
			int result = conn.Execute(sqlToUpsert, this);
			return (result > 0);
		}

		public bool Upsert(DbTransaction dbt)
		{
			int result = dbt.Connection.Execute(sqlToUpsert, this, dbt.Transaction);
			return (result > 0);
		}

		public async Task<bool> UpsertAsync()
		{
			using (var conn = new MySqlConnection(ConnectionString))
			{
				int result = await conn.ExecuteAsync(sqlToUpsert, this);
				return (result > 0);
			}
		}

		public async Task<bool> UpsertAsync(MySqlConnection conn)
		{
			int result = await conn.ExecuteAsync(sqlToUpsert, this);
			return (result > 0);
		}

		public async Task<bool> UpsertAsync(DbTransaction dbt)
		{
			int result = await dbt.Connection.ExecuteAsync(sqlToUpsert, this, dbt.Transaction);
			return (result > 0);
		}

		public static List<T> Query(string where = null, string orderBy = null)
		{
			string sql = MySqlDapper.GetSqlToQuery(tableInfo, where, orderBy);
			return MySqlDapper.Query<T>(ConnectionString4Slave, sql);
		}

		public static async Task<List<T>> QueryAsync(string where = null, string orderBy = null)
		{
			string sql = MySqlDapper.GetSqlToQuery(tableInfo, where, orderBy);
			return await MySqlDapper.QueryAsync<T>(ConnectionString4Slave, sql);
		}

		public class PageData<S>
		{
			public int TotalRow { get; set; }
			public int TotalPage { get; set; }
			public List<S> List { get; set; }
		}

		public static PageData<T> QueryPage(int page, int size, string where = null, string orderBy = null)
		{
			using (var conn = new MySqlConnection(ConnectionString4Slave))
			{
				string sql1 = MySqlDapper.GetSqlToQueryTotalRow(tableInfo, where);
				int totalRow = conn.ExecuteScalar<int>(sql1);

				int totalPage = (int)Math.Floor((decimal)(totalRow + size - 1) / size);
				int offset = page * size;

				string sql2 = MySqlDapper.GetSqlToQueryPage(tableInfo, offset, size, where, orderBy);
				var list = MySqlDapper.Query<T>(ConnectionString4Slave, sql2);

				return new PageData<T>()
				{
					TotalRow = totalRow,
					TotalPage = totalPage,
					List = list
				};
			}
		}

		public static async Task<PageData<T>> QueryPageAsync(int page, int size, string where = null, string orderBy = null)
		{
			using (var conn = new MySqlConnection(ConnectionString4Slave))
			{
				string sql1 = MySqlDapper.GetSqlToQueryTotalRow(tableInfo, where);
				int totalRow = await conn.ExecuteScalarAsync<int>(sql1);

				int totalPage = (int)Math.Floor((decimal)(totalRow + size - 1) / size);
				int offset = page * size;

				string sql2 = MySqlDapper.GetSqlToQueryPage(tableInfo, offset, size, where, orderBy);
				var list = await MySqlDapper.QueryAsync<T>(ConnectionString4Slave, sql2);

				return new PageData<T>()
				{
					TotalRow = totalRow,
					TotalPage = totalPage,
					List = list
				};
			}
		}
	}
}
