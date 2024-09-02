using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ApplicationModules)]
    public class ApplicationModule   : DBBaseEntity
    {
        public long ID { get; set; }
        public long APPTitleLookup { get; set; }
        public string Description { get; set; }
        [Column(DatabaseObjects.Columns.Owner)]
        public string Owner { get; set; }
        [Column(DatabaseObjects.Columns.SupportedBy)]
        public string SupportedBy { get; set; }
        public string Title { get; set; }
        [Column(DatabaseObjects.Columns.AccessAdmin)]
        public string AccessAdmin { get; set; }
        [Column(DatabaseObjects.Columns.Approver)]
        public string Approver { get; set; }
        public string ApprovalTypeChoice { get; set; }
        public long? ItemOrder { get; set; }

    }
}
