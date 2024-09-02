using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class FunctionRoleMapping: DBBaseEntity
    {
        public long ID { get; set; }
        public long FunctionId { get; set; }
        public string RoleId { get; set; }
        [NotMapped]
        public string RoleName { get; set; }
        [NotMapped]
        public string FunctionName { get; set; }
    }
}
