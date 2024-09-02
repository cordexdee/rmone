using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.Report_ConfigData)]
    public class ReportConfigData : DBBaseEntity
    {
        public long ID { get; set; }
        public string ReportType { get; set; }
        public string FilterHeight { get; set; }
        public string FilterWidth { get; set; }
        public string FilterTitle { get; set; }
        public string ReportTitle { get; set; }
    }
}
