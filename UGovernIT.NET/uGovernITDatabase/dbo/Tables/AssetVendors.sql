CREATE TABLE [dbo].[AssetVendors] (
    [ID]                        BIGINT          IDENTITY (1, 1) NOT NULL,
    [ContactName]               NVARCHAR (250)  NULL,
    [VendorAddress]             NVARCHAR (MAX)  NULL,
    [VendorEmail]               NVARCHAR (250)  NULL,
    [VendorLocation]            NVARCHAR (250)  NULL,
    [VendorName]                NVARCHAR (250)  NULL,
    [VendorPhone]               NVARCHAR (250)  NULL,
    [WebsiteUrl]                NVARCHAR (250)  NULL,
    [ExternalType]              NVARCHAR (250)  NULL,
    [VendorTypeLookup]          BIGINT          NULL,
    [ProductServiceDescription] VARCHAR (250)   NULL,
    [SupportHours]              VARCHAR (250)   NULL,
    [VendorTimeZoneChoice]      VARCHAR (250)   NULL,
    [SupportCredentials]        VARCHAR (250)   NULL,
    [AccountRepPhone]           NVARCHAR (250)  NULL,
    [AccountRepMobile]          NVARCHAR (250)  NULL,
    [AccountRepEmail]           NVARCHAR (250)  NULL,
    [AccountRepName]            VARCHAR (250)   NULL,
    [Title]                     VARCHAR (250)   NULL,
    [Vendortype]                INT             NULL,
    [TenantID]                  NVARCHAR (128)  NULL,
    [Created]                   DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                  DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]             NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]            NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                   BIT             DEFAULT ((0)) NULL,
    [Attachments]               NVARCHAR (2000) DEFAULT ('') NULL,
    [UGITDescription]           NVARCHAR (MAX)  NULL,
    [VendorAccountNum]          NVARCHAR (250)  NULL,
    [InternalContact]           NVARCHAR (250)  NULL,
    CONSTRAINT [PK_AssetVendors] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_AssetVendors_Deleted] ON [dbo].[AssetVendors] ([Deleted])

GO
CREATE NONCLUSTERED INDEX [IX_AssetVendors_VendorName] ON [dbo].[AssetVendors] ([VendorName])

GO
CREATE NONCLUSTERED INDEX [IX_AssetVendors_VendorTypeLookup] ON [dbo].[AssetVendors] ([VendorTypeLookup])



