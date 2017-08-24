using System;

namespace G.MySql
{
	[AttributeUsage(AttributeTargets.Property)]
	public class MySqlFieldAttribute : Attribute
	{
		public int Index { get; set; }
		public string FieldName { get; set; }
		public string FieldType { get; set; }
		public bool IsNullable { get; set; }
		public bool IsAutoIncrement { get; set; }

		public MySqlFieldAttribute(int index, string fieldName = null, string fieldType = null, bool isNullable = false, bool isAutoIncrement = false)
		{
			Index = index;
			FieldName = fieldName;
			FieldType = fieldType;
			IsNullable = isNullable;
			IsAutoIncrement = isAutoIncrement;
		}
	}
}
