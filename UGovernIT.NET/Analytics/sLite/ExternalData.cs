using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sLite
{
    public class ExternalData
    {
        public string TableName { get; set; }
        public int RowID { get; set; }
        public ExternalData()
        {
            
        }
        public ExternalData(string table,int rowId)
        {
            this.TableName = table;
            this.RowID = rowId;
        }
    }
}
