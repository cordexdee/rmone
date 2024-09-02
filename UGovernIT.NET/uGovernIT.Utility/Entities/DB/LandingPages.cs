using Microsoft.AspNet.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.LandingPages)]
    public class LandingPages : DBBaseEntity
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public string Title { get; set; }
        public string LandingPage { get; set; }

        public LandingPages()
        {
            Id = Guid.NewGuid().ToString();
            Description = string.Empty;
            Title = string.Empty;
            Name = string.Empty;
            LandingPage = string.Empty;
        }
    }
}
