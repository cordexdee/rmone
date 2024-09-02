using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ClientAdminConfigurationLists)]
    public class ClientAdminConfigurationList:DBBaseEntity
    {
        public long ID { get; set; }
        public long ClientAdminCategoryLookup { get; set; }
        public string Description { get; set; }
        public string ListName { get; set; }
        public int TabSequence { get; set; }
        public string Title { get; set; }
        public string AuthorizedToViewUser { get; set; }
    }
}
