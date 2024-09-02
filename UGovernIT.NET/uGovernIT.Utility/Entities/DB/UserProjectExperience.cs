using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Serializable]
    [Table(DatabaseObjects.Tables.UserProjectExperience)]
    public class UserProjectExperience : DBBaseEntity
    {
        public long ID { get; set; }
        public string ProjectID { get; set; }
        public string UserId { get; set; }
        public string ResourceUser { get; set; }
        public int TagLookup { get; set; }
        [NotMapped]
        public string Title { get; set; }
    }
}
