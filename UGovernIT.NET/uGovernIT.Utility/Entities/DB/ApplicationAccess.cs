using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ApplModuleRoleRelationship)]
    public class ApplicationAccess  : DBBaseEntity
    {
        public long ID { get; set; }        
        public long? ApplicationModulesLookup { get; set; }
        [Column(DatabaseObjects.Columns.ApplicationRoleAssign)]
        public string ApplicationRoleAssign { get; set; }
        public long? ApplicationRoleLookup { get; set; }
        public long? APPTitleLookup { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
    }
}
