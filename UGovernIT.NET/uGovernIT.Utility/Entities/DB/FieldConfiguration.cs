using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.FieldConfiguration)]
    public class FieldConfiguration:DBBaseEntity
    {
        public long ID { get; set; }
        public string FieldName { get; set; }
        public string ParentTableName { get; set; }
        public string ParentFieldName { get; set; }
        public string Datatype { get; set; }
        public string Data { get; set; }
        public string DisplayChoicesControl { get; set; }
        public bool Multi { get; set; }
        public string SelectionSet { get; set; }
        public string Notation { get; set; }
        public string TemplateType { get; set; }
        public string Width { get; set; }
        public string TableName { get; set; }

    }
}
