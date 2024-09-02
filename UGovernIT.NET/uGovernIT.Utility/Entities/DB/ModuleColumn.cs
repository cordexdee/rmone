using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace uGovernIT.Utility
{
    
    [Table(DatabaseObjects.Tables.ModuleColumns)]
    [ProtoContract(ImplicitFields =ImplicitFields.AllFields)]
    public class ModuleColumn:DBBaseEntity
    {
        public long ID { get; set; }
        public string FieldName { get; set; }
        public string FieldDisplayName { get; set; }
        public bool IsDisplay { get; set; }
        public int FieldSequence { get; set; }
        public string CategoryName { get; set; }
        public bool IsUseInWildCard { get; set; }
        public bool ShowInMobile { get; set; }
        public string CustomProperties { get; set; }
        public bool? DisplayForClosed { get; set; }
        public bool? DisplayForReport { get; set; }
        public string ColumnType { get; set; }
        //public bool? Prop_UseforGlobalDateFilter { get; set; }
        //public bool Prop_MiniView { get; set; }
        public string Title { get; set; }
        //public string AssetLookup { get; set; }
        public int? SortOrder { get; set; }
        public bool? IsAscending { get; set; }
        public int TruncateTextTo { get; set; }
        public string SelectedTabs { get; set; }
        
        [Column(DatabaseObjects.Columns.TextAlignment)]
        public string TextAlignment { get; set; }
		public bool? ShowInCardView { get; set; }
    }
}
