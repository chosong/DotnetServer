using System;
using System.Collections.Generic;

namespace G.MySql
{
	public class MySqlTableInfo
	{
		public string TableName { get; set; }
		public string[] PrimaryKey { get; set; }

		public List<MySqlIndexInfo> Indexes { get; set; }
		public List<MySqlFieldInfo> Fields { get; set; }
	}
}
