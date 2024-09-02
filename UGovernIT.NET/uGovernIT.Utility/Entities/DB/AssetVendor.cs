using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Serializable]
    [Table(DatabaseObjects.Tables.AssetVendors)]
    public class AssetVendor: DBBaseEntity
    {
        public long ID { get; set; }
        public string VendorName { get; set; }
        public string VendorLocation { get; set; }
        public string VendorPhone { get; set; }
        public string VendorEmail { get; set; }
        public string VendorAddress { get; set; }
        public string ContactName { get; set; }
        //public bool IsDeleted { get; set; }
        public string WebsiteUrl { get; set; }
        public string ExternalType { get; set; }
        public long VendorTypeLookup { get; set; }
        public string ProductServiceDescription { get; set; }
        public double SupportHours { get; set; }
        public string VendorTimeZoneChoice { get; set; }
        public string SupportCredentials { get; set; }
        public string AccountRepPhone { get; set; }
        public string AccountRepMobile { get; set; }
        public string AccountRepEmail { get; set; }
        public string AccountRepName { get; set; }
        public string Title { get; set; }
        [NotMapped]
        public string VendorType { get; set; }
    }
}
