using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.RelatedCompanies)]
    public class RelatedCompany : DBBaseEntity
    {
        public long ID { get; set; }
        public string ContactLookup { get; set; }
        public long? CostCodeLookup { get; set; }
        //public long? CRMCompanyTitleLookup { get; set; }
        public string CustomProperties { get; set; }
        public int? ItemOrder { get; set; }
        public long? RelationshipTypeLookup { get; set; }
        public string TicketID { get; set; }
        public string Title { get; set; }
        public string CRMCompanyLookup { get; set; }
    }
}