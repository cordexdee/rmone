using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
    public class ChartIntegration
    {
        public long ModelVersionId { get; set; }
        public string NodeName { get; set; }
        public Guid RefId { get; set; }
        public string IntegrationId { get; set; }
        public string TableName { get; set; }
        public int TableId { get; set; }
        public string ColumnName { get; set; }
        public int DataSource { get; set; }
        public string RefType { get; set; }
        public string DataType { get; set; }
        public ChartIntegration()
        {
            ModelVersionId = 0;
            IntegrationId = String.Empty;
            TableName = String.Empty;
            ColumnName = String.Empty;
            this.DataSource =(int)ITAnalyticsBL.ChartIntegrationSource.Analytic; 
            RefId = Guid.Empty;
            RefType = string.Empty;
            DataType = string.Empty;
            NodeName = string.Empty;
        }
    }
}
