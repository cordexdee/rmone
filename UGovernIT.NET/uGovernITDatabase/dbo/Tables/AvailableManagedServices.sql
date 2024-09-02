CREATE TABLE [dbo].[AvailableManagedServices] (
    [ID]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [ServiceChargeType]  NVARCHAR (MAX)  NULL,
    [ServiceDescription] NVARCHAR (MAX)  NULL,
    [ServiceFee]         NVARCHAR (250)  NULL,
    [ServiceFrequency]   NVARCHAR (MAX)  NULL,
    [ServiceName]        NVARCHAR (250)  NULL,
    [Title]              NVARCHAR (250)  NULL,
    [VendorSOWLookup]    BIGINT          NOT NULL,
    [TenantID]           NVARCHAR (128)  NULL,
    [Created]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]            BIT             DEFAULT ((0)) NULL,
    [Attachments]        NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([VendorSOWLookup]) REFERENCES [dbo].[VendorSOW] ([ID])
);






GO
