using System.Collections.Generic;

namespace CollectionUtils.Data
{
  internal class DataColumn
  {
    public DataColumn(string columnName)
    {
      ColumnName = columnName;
    }

    public string ColumnName { get; set; }
    public List<string?> Values { get; } = new();
  }
}
