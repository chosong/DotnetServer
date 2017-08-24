using System;
using G.MySql;

namespace G.MySql
{
	public class MySqlFieldInfo
	{
		public int Index { get; set; }
		public string FieldName { get; set; }

		private string fieldType;
		public string FieldType
		{
			get
			{
				if (string.IsNullOrEmpty(fieldType))
				{
					fieldType = MySqlType.ToMySqlDbType(Type).ToString();
					return fieldType;
				}
				else
				{
					return fieldType;
				}
			}

			set
			{
				fieldType = value;
			}
		}

		public bool IsNullable { get; set; }
		public bool IsAutoIncrement { get; set; }
		public string Name { get; set; }
		public Type Type { get; set; }
	}
}
