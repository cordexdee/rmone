using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace uGovernIT.Utility
{
    
    [Table(DatabaseObjects.Tables.FormLayout)]
    [ProtoContract(ImplicitFields =ImplicitFields.AllFields)]
    public class ModuleFormLayout: DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string FieldDisplayName { get; set; }
        //public int TabNumber { get; set; }
        public int FieldSequence { get; set; }
        public string FieldName { get; set; }
        public int FieldDisplayWidth { get; set; }
        public bool ShowInMobile { get; set; }
        //public string ModuleName { get; set; }

        //Custom properties
        public string CustomProperties { get; set; }
        //public bool? Prop_TreeView { get; set; }
        //public bool? Prop_IsInFrame { get; set; }
        //public bool? Prop_IsReadOnly { get; set; }

        public string SkipOnCondition { get; set; }
        public string TargetType { get; set; }
        public string TargetURL { get; set; }
        public string Tooltip { get; set; }

        public int TrimContentAfter { get; set; }
        //To set column type
        public string ColumnType { get; set; }
        public string ModuleNameLookup { get; set; }
        public int TabId { get; set; }
        public bool HideInTemplate { get; set; }

    }
}
