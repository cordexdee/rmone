using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Integration
{
    public class FieldDetail
    {
        public string DisplayName { get; set; }
        public string InternalName { get; set; }
        public string DataType { get; set; }
        public string DisplayNameWithType { get; set; }
        public string InternalNameWithType { get; set; }
        public string RefDisplayName { get; set; }
        public string RefInternalName { get; set; }

        public FieldDetail()
        {
            DisplayName = string.Empty;
            InternalName = string.Empty;
            DataType = string.Empty;
            DisplayNameWithType = string.Empty;
            InternalNameWithType = string.Empty;
            RefDisplayName = string.Empty;
            RefInternalName = string.Empty;
        }
    }
}
