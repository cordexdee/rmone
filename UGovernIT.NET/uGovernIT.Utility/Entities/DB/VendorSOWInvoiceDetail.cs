using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.VendorSOWInvoiceDetail)]

    public class VendorSOWInvoiceDetail
    {
        public long ID { get; set; }
        public string BudgetAmount { get; set; }
        public string FixedFees { get; set; }
        public string InvoiceItemAmount { get; set; }
        public string InvoiceNumber { get; set; }
        public string PONumber { get; set; }
        public int? ResourceQuantity { get; set; }
        public string SOWAdditionalUnitRate { get; set; }
        public int? SOWAnnualChangePct { get; set; }
        public int? SOWDeadBandPct { get; set; }
        public string SOWFeeUnit { get; set; }
        public string SOWFeeUnit2 { get; set; }
        public DateTime? SOWInvoiceDate { get; set; }
        public long SOWInvoiceLookup { get; set; }
        public int? SOWNoOfUnit { get; set; }
        public string SOWReducedUnitRate { get; set; }
        public string SOWUnitRate { get; set; }
        public int? VariableAmount { get; set; }
        public long VendorMSALookup { get; set; }
        public long VendorMSANameLookup { get; set; }
        public long VendorPOLineItemLookup { get; set; }
        public long VendorPOLookup { get; set; }
        public long VendorResourceCategoryLookup { get; set; }
        public long VendorResourceSubCategoryLookup { get; set; }
        public long VendorSOWFeeLookup { get; set; }
        public long VendorSOWLookup { get; set; }
        public long VendorSOWNameLookup { get; set; }
        public string Title { get; set; }
        public string TenantID { get; set; }
    }
}
