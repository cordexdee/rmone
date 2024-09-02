CREATE TABLE [dbo].[Assets] (
    [ID]                        BIGINT          IDENTITY (1, 1) NOT NULL,
    [AcquisitionDate]           DATETIME        NULL,
    [ActualReplacementDate]     DATETIME        NULL,
    [AdditionalKey]             NVARCHAR (250)  NULL,
    [ApplicationMultiLookup]    BIGINT          NULL,
    [AssetDescription]          NVARCHAR (MAX)  NULL,
    [AssetModelLookup]          BIGINT          NULL,
    [AssetName]                 NVARCHAR (250)  NULL,
    [OwnerUser]                 NVARCHAR (MAX)  NULL,
    [AssetsStatusLookup]        BIGINT          NULL,
    [AssetTagNum]               NVARCHAR (250)  NULL,
    [Closed]                    BIT             NULL,
    [CloseDate]                 DATETIME        NULL,
    [Comment]                   NVARCHAR (MAX)  NULL,
    [Cost]                      NVARCHAR (250)  NULL,
    [CPU]                       NVARCHAR (250)  NULL,
    [CurrentStageStartDate]     DATETIME        NULL,
    [DataRetention]             INT             NULL,
    [DeletedBy]                 NVARCHAR (MAX)  NULL,
    [DeletionDate]              DATETIME        NULL,
    [DepartmentLookup]          BIGINT          NULL,
    [EndDate]                   DATETIME        NULL,
    [HardDrive1]                INT             NULL,
    [HardDrive2]                INT             NULL,
    [History]                   NVARCHAR (MAX)  NULL,
    [HostName]                  NVARCHAR (250)  NULL,
    [ImageInstallDate]          DATETIME        NULL,
    [ImageOptionLookup]         BIGINT          NULL,
    [InstalledByUser]           NVARCHAR (MAX)  NULL,
    [InstalledDate]             DATETIME        NULL,
    [IPAddress]                 NVARCHAR (250)  NULL,
    [LicenseKey]                NVARCHAR (250)  NULL,
    [LicenseType]               NVARCHAR (250)  NULL,
    [LocationLookup]            BIGINT          NULL,
    [ManufacturerChoice]        NVARCHAR (MAX)  NULL,
    [ModuleStepLookup]          NVARCHAR (250)  NULL,
    [NICAddress]                NVARCHAR (250)  NULL,
    [OS]                        NVARCHAR (250)  NULL,
    [OSKey]                     NVARCHAR (250)  NULL,
    [PO]                        INT             NULL,
    [PreAcquired]               BIT             NULL,
    [PreviousOwner1User]        NVARCHAR (MAX)  NULL,
    [PreviousOwner2User]        NVARCHAR (MAX)  NULL,
    [PreviousOwner3User]        NVARCHAR (MAX)  NULL,
    [PreviousUser]              NVARCHAR (250)  NULL,
    [PurchasedBy]               NVARCHAR (MAX)  NULL,
    [RAM]                       INT             NULL,
    [RegisteredByUser]          NVARCHAR (MAX)  NULL,
    [RegistrationDate]          DATETIME        NULL,
    [RenewalDate]               DATETIME        NULL,
    [ReplacementAsset_SNLookup] BIGINT          NULL,
    [ReplacementDate]           DATETIME        NULL,
    [ReplacementDeliveryDate]   DATETIME        NULL,
    [ReplacementOrderedDate]    DATETIME        NULL,
    [ReplacementTypeChoice]     NVARCHAR (MAX)  NULL,
    [RequestTypeCategory]       NVARCHAR (250)  NULL,
    [RequestTypeLookup]         BIGINT          NULL,
    [RequestTypeSubCategory]    NVARCHAR (250)  NULL,
    [ResaleValue]               INT             NULL,
    [ResoldFor]                 NVARCHAR (250)  NULL,
    [ResoldTo]                  NVARCHAR (250)  NULL,
    [RetiredDate]               DATETIME        NULL,
    [SaleDate]                  DATETIME        NULL,
    [ScheduleStatusChoice]      NVARCHAR (MAX)  NULL,
    [SerialAssetDetail]         NVARCHAR (250)  NULL,
    [SerialNum1]                NVARCHAR (250)  NULL,
    [SerialNum1Description]     NVARCHAR (250)  NULL,
    [SerialNum2]                NVARCHAR (250)  NULL,
    [SerialNum2Description]     NVARCHAR (250)  NULL,
    [SerialNum3]                NVARCHAR (250)  NULL,
    [SerialNum3Description]     NVARCHAR (250)  NULL,
    [SetupCompletedByUser]      NVARCHAR (MAX)  NULL,
    [SetupCompletedDate]        DATETIME        NULL,
    [SoftwareLookup]            BIGINT          NULL,
    [StageActionUsersUser]      NVARCHAR (250)  NULL,
    [StageActionUserTypes]      NVARCHAR (250)  NULL,
    [StageStep]                 INT             NULL,
    [StartDate]                 DATETIME        NULL,
    [Status]                    NVARCHAR (250)  NULL,
    [StatusChangeDate]          DATETIME        NULL,
    [SupplierChoice]            NVARCHAR (MAX)  NULL,
    [SupportNumber]             NVARCHAR (250)  NULL,
    [TicketId]                  NVARCHAR (250)  NULL,
    [TransferDate]              DATETIME        NULL,
    [TSRIdLookup]               BIGINT          NULL,
    [UninstallDate]             DATETIME        NULL,
    [UpgradeChoice]             NVARCHAR (MAX)  NULL,
    [VendorLookup]              BIGINT          NULL,
    [WarrantyExpirationDate]    DATETIME        NULL,
    [WarrantyType]              NVARCHAR (250)  NULL,
    [Title]                     VARCHAR (250)   NULL,
    [ServiceTag]                NVARCHAR (250)  NULL,
    [ServerStorageType]         NVARCHAR (MAX)  NULL,
    [ServerDomain]              NVARCHAR (MAX)  NULL,
    [ResolvedByUser]            NVARCHAR (250)  NULL,
    [PONumber2]                 NVARCHAR (250)  NULL,
    [PONumber]                  NVARCHAR (250)  NULL,
    [PlanRefresh]               NVARCHAR (250)  NULL,
    [PatchDate]                 DATETIME        NULL,
    [PartNumber]                NVARCHAR (250)  NULL,
    [NoOfProcessorCores]        INT             NULL,
    [NICName]                   NVARCHAR (250)  NULL,
    [NetworkLocation]           NVARCHAR (250)  NULL,
    [LeaseVendor]               NVARCHAR (250)  NULL,
    [LeaseToDate]               DATETIME        NULL,
    [LeaseFromDate]             DATETIME        NULL,
    [LastSeenDate]              DATETIME        NULL,
    [ExternalType]              NVARCHAR (250)  NULL,
    [ExternaID]                 NVARCHAR (250)  NULL,
    [DivisionLookup]            BIGINT          NULL,
    [DeskLocation]              NVARCHAR (250)  NULL,
    [DataEditor]                NVARCHAR (250)  NULL,
    [CurrentUser]               NVARCHAR (250)  NULL,
    [ClosedByUser]              NVARCHAR (250)  NULL,
    [BackupType]                NVARCHAR (MAX)  NULL,
    [BackupPolicy]              NVARCHAR (250)  NULL,
    [AssetDispositionChoice]    NVARCHAR (MAX)  NULL,
    [EnvironmentLookup]         BIGINT          NULL,
    [TenantID]                  NVARCHAR (128)  NULL,
    [DataEditors]               NVARCHAR (250)  DEFAULT ('') NULL,
    [Created]                   DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                  DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]             NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]            NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                   BIT             DEFAULT ((0)) NULL,
    [Attachments]               NVARCHAR (2000) DEFAULT ('') NULL,
    [ResolutionDate]            DATETIME        NULL,
    [CellPhoneNumber] NVARCHAR(20) NULL, 
    [LocalAdminUser] NVARCHAR(128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL, 
    [ApprovalDate] DATETIME NULL, 
    [ApprovedByUser] VARCHAR(128) NULL, 
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([AssetModelLookup]) REFERENCES [dbo].[AssetModels] ([ID]),
    FOREIGN KEY ([AssetsStatusLookup]) REFERENCES [dbo].[AssetsStatus] ([ID]),
    FOREIGN KEY ([DepartmentLookup]) REFERENCES [dbo].[Department] ([ID]),
    FOREIGN KEY ([DivisionLookup]) REFERENCES [dbo].[CompanyDivisions] ([ID]),
    FOREIGN KEY ([EnvironmentLookup]) REFERENCES [dbo].[Environment] ([ID]),
    FOREIGN KEY ([ImageOptionLookup]) REFERENCES [dbo].[ImageSoftware] ([ID]),
    FOREIGN KEY ([LocationLookup]) REFERENCES [dbo].[Location] ([ID]),
    FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID]),
    FOREIGN KEY ([SoftwareLookup]) REFERENCES [dbo].[ImageSoftware] ([ID])
);












GO

GO

GO

GO

GO

GO

GO

GO

GO

GO
CREATE NONCLUSTERED INDEX [IX_Assets_AssetTagNum] ON [dbo].[Assets] ([AssetTagNum])

GO
CREATE NONCLUSTERED INDEX [IX_Assets_Deleted] ON [dbo].[Assets] ([Deleted])

GO
CREATE NONCLUSTERED INDEX [IX_Assets_ModuleStepLookup] ON [dbo].[Assets] ([ModuleStepLookup])

GO
CREATE NONCLUSTERED INDEX [IX_Assets_RequestTypeLookup] ON [dbo].[Assets] ([RequestTypeLookup])

GO
CREATE NONCLUSTERED INDEX [IX_Assets_Status] ON [dbo].[Assets] ([Status])

GO
CREATE NONCLUSTERED INDEX [IX_Assets_TicketId] ON [dbo].[Assets] ([TicketId])

GO
CREATE NONCLUSTERED INDEX [IX_Assets_TSRIdLookup] ON [dbo].[Assets] ([TSRIdLookup])