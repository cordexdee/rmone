using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.Common
{
    [Table(DatabaseObjects.Tables.ApplicationPassword)]
    public class ApplicationPasswordEntity
    {
        public long ID { get; set; }
        public string APPPasswordTitle { get; set; }
        public long APPTitleLookup { get; set; }
        public string APPUserName { get; set; }
        public string Description { get; set; }
        public string EncryptedPassword { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }
        public string TenantID { get; set; }
    }

}
