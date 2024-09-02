using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uGovernIT.Utility
{
    public class TicketColumnValue
    {
        public string InternalFieldName { get; set; }
        public string DisplayName { get; set; }
        public object Value { get; set; }
        public string ErrorDisplayName { get; set; }
        /// <summary>
        /// In which tab, field is being shown
        /// </summary>
        public int TabNumber { get; set; }
    }
}
