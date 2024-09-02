using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.DashboardFactTables)]
    public class DashboardFactTables
    {
        public long ID { get; set; }
        public int CacheAfter { get; set; }
        public string Title { get; set; }
        public string CacheMode { get; set; }
        public bool CacheTable { get; set; }
        public int CacheThreshold { get; set; }
        public string DashboardPanelInfo { get; set; }
        public string Description { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string RefreshMode { get; set; }
        public string Status { get; set; }
        public string TenantID { get; set; }
        public DashboardFactTables()
        {
            LastUpdated = (DateTime)SqlDateTime.MinValue;
            ExpiryDate = (DateTime)SqlDateTime.MinValue;
        }

    }
}
