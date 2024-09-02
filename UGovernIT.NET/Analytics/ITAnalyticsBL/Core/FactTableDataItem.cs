using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
    public class FactTableDataItem
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }

        public FactTableDataItem()
        {
            this.ColumnName = string.Empty;
            this.DataType = string.Empty;
        }
        public FactTableDataItem(string columnName, string dType)
        {
            this.ColumnName = columnName;
            this.DataType = dType;
        }
    }
}
