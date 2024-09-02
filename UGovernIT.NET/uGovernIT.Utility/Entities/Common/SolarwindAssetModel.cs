using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class SolarwindAssetModel
    {
        public int id { get; set; }
        public string type { get; set; }
        public DateTime? contractExpiration { get; set; }
        public string macAddress { get; set; }
        public string networkAddress { get; set; }
        public string networkName { get; set; }
        public string Notes { get; set; }
        public DateTime? purchaseDate { get; set; }
        public string serialNumber { get; set; }
        public string version { get; set; }
        public AssetStatus assetstatus { get; set; }
        public double? billingRate { get; set; }
        public Location location { get; set; }
        public Model model { get; set; }
        public Room room { get; set; }
        public WarrantyType warrantType { get; set; }
        public List<Client> clients { get; set; }
        public bool isSynchronizationDisabled { get; set; }
        public bool isReservable { get; set; }
        public DateTime? leaseExpirationDate { get; set; }
        public DateTime? warrantyExpirationDate { get; set; }
        public bool isNotesVisibleToClients { get; set; }
        public bool isDeleted { get; set; }
        public List<AssetCustomField> assetCustomFields { get; set; }
        public Manufacturer manufacturer { get; set; }

        public class AssetStatus
        {
            public int id { get; set; }
            public string type { get; set; }
        }

        public class Location
        {
            public int id { get; set; }
            public string type { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public string locationName { get; set; }
            public string postalCode { get; set; }
            public string state { get; set; }
            public string defaultPriorityTypeId { get; set; }
        }

        public class Model
        {
            public int id { get; set; }
            public string type { get; set; }
            public int assetTypeId { get; set; }
            public int manufacturerId { get; set; }
            public int modelId { get; set; }
            public string modelName { get; set; }
            public string warrentyTypeId { get; set; }
            public AssetType assetType { get; set; }
            public Manufacturer manufacturer { get; set; }
        }

        public class AssetType
        {
            public int id { get; set; }
            public string type { get; set; }
            public string assetType { get; set; }
        }

        public class Manufacturer
        {
            public int id { get; set; }
            public string type { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public string country { get; set; }
            public string fax { get; set; }
            public string fullName { get; set; }
            public int manufacturerId { get; set; }
            public string name { get; set; }
            public string phone { get; set; }
            public string postalCode { get; set; }
            public string state { get; set; }
            public string url { get; set; }
        }

        public class Room
        {
            public int id { get; set; }
            public string type { get; set; }
            public string roomName { get; set; }
        }
        public class WarrantyType
        {
            public int id { get; set; }
            public string type { get; set; }
        }

        public class Client
        {
            public int id { get; set; }
            public string type { get; set; }
        }

        public class AssetCustomField
        {
            public string id { get; set; }
            public string type { get; set; }
            public int definitionId { get; set; }
            public string restValue { get; set; }
        }
    }
}
