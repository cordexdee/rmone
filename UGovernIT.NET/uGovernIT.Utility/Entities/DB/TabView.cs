using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.TabView)]
    public class TabView : DBBaseEntity
    {
        [Key]
        public long ID { get; set; }
        public string TabName { get; set; }
        public string TabDisplayName { get; set; }
        public string ViewName { get; set; }
        public string ModuleNameLookup { get; set; }
        public int TabOrder { get; set; }
        public string ColumnViewName { get; set; }
        public bool ShowTab { get; set; }
        public string TablabelName { get; set; }
    }
}
