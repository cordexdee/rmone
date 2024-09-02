using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Serializable]
    [Table(DatabaseObjects.Tables.Assets)]
    public class Assests : DBBaseEntity
    {
        public long ID { get; set; }
        public DateTime? AcquisitionDate { get; set; }
        public DateTime? ActualReplacementDate { get; set; }
        public string AdditionalKey { get; set; }
        public long? ApplicationMultiLookup { get; set; }
        public string AssetDescription { get; set; }
        public long? AssetModelLookup { get; set; }
        public string AssetName { get; set; }
        [Column(DatabaseObjects.Columns.Owner)]
        public string Owner { get; set; }
        public long? AssetsStatusLookup { get; set; }
        public string AssetTagNum { get; set; }
        public bool? Closed { get; set; }
        public DateTime? CloseDate { get; set; }
        public string Comment { get; set; }
        public string Cost { get; set; }
        public string CPU { get; set; }
        public DateTime? CurrentStageStartDate { get; set; }
        public int? DataRetention { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletionDate { get; set; }
        public long? DepartmentLookup { get; set; }
        public DateTime? EndDate { get; set; }
        public int? HardDrive1 { get; set; }
        public int? HardDrive2 { get; set; }
        public string History { get; set; }
        public string HostName { get; set; }
        public DateTime? ImageInstallDate { get; set; }
        public long? ImageOptionLookup { get; set; }
        [Column(DatabaseObjects.Columns.InstalledBy)]
        public string InstalledBy { get; set; }
        public DateTime? InstalledDate { get; set; }
        public string IPAddress { get; set; }
        
        public string LicenseKey { get; set; }
        public string LicenseType { get; set; }
        public long? LocationLookup { get; set; }
        public string ManufacturerChoice { get; set; }
        public string ModuleStepLookup { get; set; }
        public string NICAddress { get; set; }
        public string OS { get; set; }
        public string OSKey { get; set; }
        public int? PO { get; set; }
        public bool? PreAcquired { get; set; }
        [Column(DatabaseObjects.Columns.PreviousOwner1)]
        public string PreviousOwner1 { get; set; }
        [Column(DatabaseObjects.Columns.PreviousOwner2)]
        public string PreviousOwner2 { get; set; }
        [Column(DatabaseObjects.Columns.PreviousOwner3)]
        public string PreviousOwner3 { get; set; }
        public string PreviousUser { get; set; }
        public string PurchasedBy { get; set; }
        public int? RAM { get; set; }
        [Column(DatabaseObjects.Columns.RegisteredBy)]
        public string RegisteredBy { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? RenewalDate { get; set; }
        public long? ReplacementAsset_SNLookup { get; set; }
        public DateTime? ReplacementDate { get; set; }
        public DateTime? ReplacementDeliveryDate { get; set; }
        public DateTime? ReplacementOrderedDate { get; set; }
        [Column(DatabaseObjects.Columns.ReplacementType)]
        public string ReplacementType { get; set; }
        public string RequestTypeCategory { get; set; }
        public long? RequestTypeLookup { get; set; }
        public string RequestTypeSubCategory { get; set; }
        public int? ResaleValue { get; set; }
        public string ResoldFor { get; set; }
        public string ResoldTo { get; set; }
        public DateTime? RetiredDate { get; set; }
        public DateTime? SaleDate { get; set; }
        public string ScheduleStatusChoice { get; set; }
        public string SerialAssetDetail { get; set; }
        public string SerialNum1 { get; set; }
        public string SerialNum1Description { get; set; }
        public string SerialNum2 { get; set; }
        public string SerialNum2Description { get; set; }
        public string SerialNum3 { get; set; }
        public string SerialNum3Description { get; set; }
        [Column(DatabaseObjects.Columns.SetupCompletedBy)]
        public string SetupCompletedBy { get; set; }
        public DateTime? SetupCompletedDate { get; set; }
        public long? SoftwareLookup { get; set; }
        [Column(DatabaseObjects.Columns.TicketStageActionUsers)]
        public string StageActionUsers { get; set; }
        public string StageActionUserTypes { get; set; }
        public int? StageStep { get; set; }
        public DateTime? StartDate { get; set; }
        public string Status { get; set; }
        public DateTime? StatusChangeDate { get; set; }
        public string SupplierChoice { get; set; }
        public string SupportNumber { get; set; }
        public string TicketId { get; set; }
        public DateTime? TransferDate { get; set; }
        public long? TSRIdLookup { get; set; }
        public DateTime? UninstallDate { get; set; }
        public string UpgradeChoice { get; set; }
        public long? VendorLookup { get; set; }
        public DateTime? WarrantyExpirationDate { get; set; }
        public string WarrantyType { get; set; }
        public string Title { get; set; }
        public string ServiceTag { get; set; }
        public string ServerStorageType { get; set; }
        public string ServerDomain { get; set; }
        [Column(DatabaseObjects.Columns.TicketResolvedBy)]
        public string ResolvedBy { get; set; }
        public string PONumber2 { get; set; }
        public string PONumber { get; set; }
        public string PlanRefresh { get; set; }
        public DateTime? PatchDate { get; set; }
        public string PartNumber { get; set; }
        public int? NoOfProcessorCores { get; set; }
        public string NICName { get; set; }
        public string NetworkLocation { get; set; }
        public string LeaseVendor { get; set; }
        public DateTime? LeaseToDate { get; set; }
        public DateTime? LeaseFromDate { get; set; }
        public DateTime? LastSeenDate { get; set; }
        public string ExternalType { get; set; }
        public string ExternaID { get; set; }
        public long? DivisionLookup { get; set; }
        public string DeskLocation { get; set; }
        public string DataEditor { get; set; }
        public string CurrentUser { get; set; }
        [Column(DatabaseObjects.Columns.TicketClosedBy)]
        public string ClosedBy { get; set; }
        public string BackupType { get; set; }
        public string BackupPolicy { get; set; }
        public string AssetDispositionChoice { get; set; }
        public long? EnvironmentLookup { get; set; }
        public string CellPhoneNumber { get; set; }
        public string LocalAdminUser { get; set; }
    }
}
