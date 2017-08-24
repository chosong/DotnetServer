using System;

namespace G.MySql
{
	[AttributeUsage(AttributeTargets.Class)]
	public class MySqlTableAttribute : Attribute
	{
		public string TableName { get; set; }
		public string[] PrimaryKey { get; set; }

		public MySqlTableAttribute(string tableName, string primaryKey = null)
		{
			TableName = tableName;

			if (!string.IsNullOrEmpty(primaryKey))
			{
				PrimaryKey = primaryKey.Split(',');
				for (int i = 0; i < PrimaryKey.Length; i++)
				{
					PrimaryKey[i] = PrimaryKey[i].Trim();
				}
			}
		}
	}
}
