using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.ProjectReleases)]
    public class ProjectReleases:DBBaseEntity
    {
        public long ID { get; set; }
        public int? ItemOrder { get; set; }
        public long PMMIdLookup { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string ReleaseID { get; set; }
        public string Title { get; set; }
        //public string TenantID { get; set; }
    }
}
