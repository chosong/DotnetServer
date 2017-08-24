using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using MySql.Data.MySqlClient;
using Dapper;
using G.Util;

namespace G.MySql
{
	public class MySqlDapper
	{
		public static MySqlTableInfo ParseTable<T>() where T : class
		{
			return ParseTable(typeof(T));
		}

		public static MySqlTableInfo ParseTable(Type type)
		{
			MySqlTableInfo tableInfo = new MySqlTableInfo();
			tableInfo.Indexes = new List<MySqlIndexInfo>();

			TypeInfo typeInfo = type.GetTypeInfo();

			foreach (var attr in typeInfo.GetCustomAttributes<MySqlTableAttribute>())
			{
				tableInfo.TableName = attr.TableName;
				tableInfo.PrimaryKey = attr.PrimaryKey;
				break;
			}

			foreach (var attr in typeInfo.GetCustomAttributes<MySqlIndexAttribute>())
			{
				var indexInfo = new MySqlIndexInfo();
				indexInfo.FieldNames = attr.FieldNames;

				if (string.IsNullOrEmpty(attr.Name))
				{
					StringBuilder sb = new StringBuilder();
					sb.Append("idx");
					foreach (var fieldName in indexInfo.FieldNames)
						sb.Append(StringEx.ToUpperFirstCharacter(fieldName));
					indexInfo.Name = sb.ToString();
				}
				else
				{
					indexInfo.Name = attr.Name;
				}

				indexInfo.IsUnique = attr.IsUnique;

				tableInfo.Indexes.Add(indexInfo);
			}

			if (string.IsNullOrEmpty(tableInfo.TableName))
				tableInfo.TableName = StringEx.ToLowerFirstCharacter(type.Name);

			var fields = new SortedDictionary<long, MySqlFieldInfo>();

			foreach (var p in type.GetProperties())
			{
				MySqlFieldInfo fieldInfo = new MySqlFieldInfo();

				var attrs = p.GetCustomAttributes(false);
				foreach (var attr in attrs)
				{
					var attr1 = attr as MySqlFieldAttribute;
					if (attr1 != null)
					{
						fieldInfo.Index = attr1.Index;

						if (string.IsNullOrEmpty(attr1.FieldName))
							fieldInfo.FieldName = StringEx.ToLowerFirstCharacter(p.Name);
						else
							fieldInfo.FieldName = attr1.FieldName;

						if (string.IsNullOrEmpty(attr1.FieldType))
							fieldInfo.FieldType = MySqlType.ToMySqlType(p.PropertyType);
						else
							fieldInfo.FieldType = attr1.FieldType;

						fieldInfo.IsNullable = attr1.IsNullable;
						fieldInfo.IsAutoIncrement = attr1.IsAutoIncrement;
						fieldInfo.Name = p.Name;
						fieldInfo.Type = p.PropertyType;

						if (fieldInfo.IsNullable == false)
						{
							var t = p.PropertyType.GetTypeInfo();
							if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
								fieldInfo.IsNullable = true;
						}
					}
				}

				if (!string.IsNullOrEmpty(fieldInfo.FieldName))
					fields.Add(fieldInfo.Index, fieldInfo);
			}

			tableInfo.Fields = fields.Values.ToList();

			return tableInfo;
		}

		public static string GetSqlToCreate<T>()
		{
			var tableInfo = ParseTable(typeof(T));
			return GetSqlToCreate(tableInfo);
		}

		public static string GetSqlToCreate(MySqlTableInfo tableInfo)
		{
			var sb = new StringBuilder();

			sb.AppendFormat("create table if not exists `{0}` (", tableInfo.TableName);

			bool isFirst = true;
			foreach (var f in tableInfo.Fields)
			{
				if (isFirst)
					isFirst = false;
				else
					sb.Append(", ");

				sb.Append("`" + f.FieldName + "`");
				sb.Append(" " + f.FieldType);
				if (f.IsNullable)
					sb.Append(" null");
				else
					sb.Append(" not null");
				if (f.IsAutoIncrement)
					sb.Append(" auto_increment");
			}

			if (tableInfo.PrimaryKey != null && tableInfo.PrimaryKey.Length > 0)
			{
				sb.Append(", primary key (");
				bool isFirst2 = true;
				foreach (var p in tableInfo.PrimaryKey)
				{
					if (isFirst2)
						isFirst2 = false;
					else
						sb.Append(", ");
						
					sb.Append("`" + p + "`");
				}
				sb.Append(")");
			}

			if (tableInfo.Indexes != null && tableInfo.Indexes.Count > 0)
			{
				foreach (var idx in tableInfo.Indexes)
				{
					sb.Append(", ");

					if (idx.IsUnique) sb.Append(" unique");

					sb.Append(" index ");
					sb.Append(idx.Name);
					sb.Append("(");

					bool isFirst2 = true;
					foreach (var fieldName in idx.FieldNames)
					{
						if (isFirst2)
							isFirst2 = false;
						else
							sb.Append(", ");

						sb.Append("`" + fieldName + "`");
					}

					sb.Append(")");
				}
			}

			sb.Append(")");

			return sb.ToString();
		}

		public static string GetSqlToInsert<T>()
		{
			var tableInfo = ParseTable(typeof(T));
			return GetSqlToInsert(tableInfo);
		}

		public static string GetSqlToInsert(MySqlTableInfo tableInfo)
		{
			var sb = new StringBuilder();

			sb.AppendFormat("insert ignore into `{0}` (", tableInfo.TableName);
			bool isFirst = true;
			foreach (var f in tableInfo.Fields)
			{
				if (f.IsAutoIncrement) continue;

				if (isFirst)
					isFirst = false;
				else
					sb.Append(", ");

				sb.AppendFormat("`{0}`", f.FieldName);
			}
			sb.Append(") values (");
			isFirst = true;
			foreach (var f in tableInfo.Fields)
			{
				if (f.IsAutoIncrement) continue;

				if (isFirst)
					isFirst = false;
				else
					sb.Append(", ");

				sb.AppendFormat("@" + f.Name);
			}
			sb.Append(")");

			return sb.ToString();
		}

		public static string GetSqlToUpsert(MySqlTableInfo tableInfo)
		{
			var sb = new StringBuilder();

			sb.AppendFormat("insert into `{0}` (", tableInfo.TableName);
			bool isFirst = true;
			foreach (var f in tableInfo.Fields)
			{
				if (f.IsAutoIncrement) continue;

				if (isFirst)
					isFirst = false;
				else
					sb.Append(", ");

				sb.AppendFormat("`{0}`", f.FieldName);
			}
			sb.Append(") values (");
			isFirst = true;
			foreach (var f in tableInfo.Fields)
			{
				if (f.IsAutoIncrement) continue;

				if (isFirst)
					isFirst = false;
				else
					sb.Append(", ");

				sb.AppendFormat("@" + f.Name);
			}
			sb.Append(")");

			sb.Append(" on duplicate key update ");
			isFirst = true;
			foreach (var f in tableInfo.Fields)
			{
				if (tableInfo.PrimaryKey.Contains(f.FieldName)) continue;

				if (isFirst)
					isFirst = false;
				else
					sb.Append(", ");

				sb.AppendFormat("`{0}` = @{1}", f.FieldName, f.Name);
			}

			return sb.ToString();
		}

		public static string GetSqlToQuery<T>(string where = null, string orderBy = null)
		{
			var tableInfo = ParseTable(typeof(T));
			return GetSqlToQuery(tableInfo, where, orderBy);
		}

		public static string GetSqlToQuery(MySqlTableInfo tableInfo, string where = null, string orderBy = null)
		{
			var sb = new StringBuilder();
			sb.Append("select * from " + tableInfo.TableName);

			if (!string.IsNullOrEmpty(where))
				sb.Append(" where " + where);
			
			if (!string.IsNullOrEmpty(orderBy))
				sb.Append(" order by " + orderBy);

			return sb.ToString();
		}

		public static string GetSqlToQueryTotalRow<T>(string where = null)
		{
			var tableInfo = ParseTable(typeof(T));
			return GetSqlToQueryTotalRow(tableInfo, where);
		}

		public static string GetSqlToQueryTotalRow(MySqlTableInfo tableInfo, string where = null)
		{
			var sb = new StringBuilder();
			sb.Append("select count(*) from ");
			sb.Append(tableInfo.TableName);

			if (!string.IsNullOrEmpty(where))
				sb.Append(" where " + where);

			return sb.ToString();
		}

		public static string GetSqlToQueryPage<T>(int offset, int limit, string where = null, string orderBy = null)
		{
			var tableInfo = ParseTable(typeof(T));
			return GetSqlToQueryPage(tableInfo, offset, limit, where, orderBy);
		}

		public static string GetSqlToQueryPage(MySqlTableInfo tableInfo, int offset, int limit, string where = null, string orderBy = null)
		{
			var sb = new StringBuilder();
			sb.Append("select * from " + tableInfo.TableName);

			if (!string.IsNullOrEmpty(where))
				sb.Append(" where " + where);
			
			if (!string.IsNullOrEmpty(orderBy))
				sb.Append(" order by " + orderBy);

			sb.AppendFormat(" limit {0}, {1}", offset, limit);

			return sb.ToString();
		}

		public static List<T> Query<T>(string connectionString, string sql)
		{
			using (var conn = new MySqlConnection(connectionString))
			{
				var list = conn.Query<T>(sql);
				return list.AsList();
			}
		}

		public static List<T> Query<T>(MySqlConnection conn, string sql)
		{
			var list = conn.Query<T>(sql);
			return list.AsList();
		}

		public static List<T> Query<T>(DbTransaction dbt, string sql)
		{
			var list = dbt.Connection.Query<T>(sql, transaction: dbt.Transaction);
			return list.AsList();
		}

		public static async Task<List<T>> QueryAsync<T>(string connectionString, string sql)
		{
			using (var conn = new MySqlConnection(connectionString))
			{
				var list = await conn.QueryAsync<T>(sql);
				return list.AsList();
			}
		}

		public static async Task<List<T>> QueryAsync<T>(MySqlConnection conn, string sql)
		{
			var list = await conn.QueryAsync<T>(sql);
			return list.AsList();
		}

		public static async Task<List<T>> QueryAsync<T>(DbTransaction dbt, string sql)
		{
			var list = await dbt.Connection.QueryAsync<T>(sql, transaction: dbt.Transaction);
			return list.AsList();
		}
	}
}
