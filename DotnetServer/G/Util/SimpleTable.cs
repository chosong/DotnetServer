using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using OfficeOpenXml;

namespace G.Util
{
	public class SimpleTable
	{
		private static object splitLine = new object();

		public bool HasColumnName { get; private set; }
		public string[] ColumnNames { get; private set; }
		public int[] ColumnLengths { get; private set; }

		private List<object[]> rows = new List<object[]>();

		public int ColumnCount { get; private set; }

		public SimpleTable(int columnCount)
		{
			Initialize(new string[columnCount]);
		}

		public SimpleTable(params string[] columnNames)
		{
			Initialize(columnNames);
		}

		private void Initialize(string[] columnNames)
		{
			ColumnCount = columnNames.Length;
			ColumnNames = columnNames;

			ColumnNames = new string[ColumnCount];
			ColumnLengths = new int[ColumnCount];

			for (int i = 0; i < ColumnCount; i++)
			{
				if (string.IsNullOrEmpty(columnNames[i]))
				{
					ColumnNames[i] = "";
				}
				else
				{
					HasColumnName = true;
					ColumnNames[i] = columnNames[i];
				}
				ColumnLengths[i] = ColumnNames[i].Length;
			}
		}

		public void AddLine()
		{
			object[] columns = new object[ColumnCount];
			columns[0] = splitLine;
			Add(columns);
		}

		public void Add(params object[] columns)
		{
			if (columns.Length != ColumnCount)
				throw new Exception("Columns count is not matched");

			for (int i = 0; i < columns.Length; i++)
			{
				object obj = columns[i];
				if (obj == null) continue;

				int len = obj.ToString().Length;
				if (len > ColumnLengths[i]) ColumnLengths[i] = len;
			}

			rows.Add(columns);
		}

		public void Add(char splitter, string columnsWithSplitter)
		{
			string[] tokens = columnsWithSplitter.Split(splitter);
			var columns = new string[ColumnCount];
			for (int i = 0; i < columns.Length; i++)
			{
				columns[i] = tokens[i].Trim();
			}

			Add(columns);
		}

		public override string ToString()
		{
			List<string> lines = new List<string>();

			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < ColumnCount; i++)
			{
				if (i == 0)
					sb.Append(" ");
				else
					sb.Append(" | ");

				int len = ColumnLengths[i];
				string text = ColumnNames[i];
				string space = new string(' ', len - text.Length);

				sb.Append(text);
				sb.Append(space);
			}
			sb.Append(" ");
			lines.Add(sb.ToString());

			foreach (var r in rows)
			{
				if (ColumnCount > 0 && r[0] == splitLine)
				{
					lines.Add("");
				}
				else
				{
					sb.Clear();

					for (int i = 0; i < ColumnCount; i++)
					{
						if (i == 0)
							sb.Append(" ");
						else
							sb.Append(" | ");

						int len = ColumnLengths[i];
						var obj = r[i];
						string text = (obj == null) ? "" : obj.ToString();
						string space = new string(' ', len - text.Length);

						if (TypeEx.IsNumeric(obj.GetType()))
						{
							sb.Append(space);
							sb.Append(text);
						}
						else
						{
							sb.Append(text);
							sb.Append(space);
						}
					}

					sb.Append(" ");
					lines.Add(sb.ToString());
				}
			}

			sb.Clear();

			if (lines.Count > 0)
			{
				int lineLength = lines[0].Length;

				if (HasColumnName)
				{
					sb.AppendLine(StringEx.LineSingle(lineLength));
					sb.AppendLine(lines[0]);
				}

				sb.AppendLine(StringEx.LineSingle(lineLength));

				for (int i = 1; i < lines.Count; i++)
				{
					if (lines[i] == "")
						sb.AppendLine(StringEx.LineSingle(lineLength));
					else
						sb.AppendLine(lines[i]);
				}

				sb.AppendLine(StringEx.LineSingle(lineLength));
			}

			return sb.ToString();
		}

		public void ToExcel(string filePath, string sheetName)
		{
			FileInfo fileInfo = new FileInfo(filePath);

			using (ExcelPackage package = new ExcelPackage(fileInfo))
			using (ExcelWorksheet sheet = package.Workbook.Worksheets.Add(sheetName))
			{
				for (int i = 0; i < ColumnCount; i++)
				{
					string pos = ExcelEx.ToPosition(0, i);
					sheet.Cells[pos].Value = ColumnNames[i];
					sheet.Cells[pos].Style.Font.Bold = true;
				}

				for (int j = 0; j < rows.Count; j++)
				{
					object[] r = rows[j];

					if (ColumnCount > 0 && r[0] == splitLine)
					{
					}
					else
					{
						for (int i = 0; i < ColumnCount; i++)
						{
							var obj = r[i];
							string text = (obj == null) ? "" : obj.ToString();

							string pos = ExcelEx.ToPosition(j + 1, i);
							sheet.Cells[pos].Value = r[i];
						}
					}
				}

				package.Save();
			}
		}
	}
}
