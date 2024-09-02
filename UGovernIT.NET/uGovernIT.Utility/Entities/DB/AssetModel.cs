using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Serializable]
    [Table(DatabaseObjects.Tables.AssetModels)]
    public class AssetModel: DBBaseEntity
    {
        public long ID { get; set; }
        public long BudgetLookup { get; set; }
        //public bool IsDeleted { get; set; }
        public string ModelDescription { get; set; }
        public string ModelName { get; set; }
        public long VendorLookup { get; set; }
        [NotMapped]
        public string VendorName { get; set; }
        public string Title { get; set; }
        [NotMapped]
        public string BudgetItem { get; set; }
        public string ExternalType { get; set; }
        
    }
}
