using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.TenantRegistration)]
    public class TenantRegistration : DBBaseEntity
    {
        public long Id { get; set; }
        public string TenantRegistrationData { get; set; }
    }

    public class TenantRegistrationData
    {
        public string Company { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Contact { get; set; }
        public bool TenantCreationStarted { get; set; }
    }
}
