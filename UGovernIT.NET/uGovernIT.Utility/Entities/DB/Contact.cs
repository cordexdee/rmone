using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class Contact:DBBaseEntity
    {
        public long ID { get; set; }
        public string AddressedAs { get; set; }
        public string City{get;set;}
        public string Country { get; set; }
        public string EmailAddress { get; set; }
        public string Fax { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
      public string Mobile { get; set; }
      public string OrganizationLookup { get; set; }
      public string SecondaryEmail { get; set; }
      public string State { get; set; }
      public string StreetAddress1 { get; set; }
      public string StreetAddress2 { get; set; }
      public int Telephone { get; set; }
      public string Title { get; set; }
      public int Zip { get; set; }
    }
}
