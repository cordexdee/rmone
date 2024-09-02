CREATE TABLE [dbo].[VendorServiceDuration] (
    [ID]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [Description]         NVARCHAR (MAX)  NULL,
    [ServiceDurationName] NVARCHAR (250)  NULL,
    [ServiceEndDate]      DATETIME        NULL,
    [ServiceStartDate]    DATETIME        NULL,
    [Title]               NVARCHAR (250)  NULL,
    [VendorMSALookup]     BIGINT          NOT NULL,
    [TenantID]            NVARCHAR (128)  NULL,
    [Created]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]             BIT             DEFAULT ((0)) NULL,
    [Attachments]         NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([VendorMSALookup]) REFERENCES [dbo].[VendorMSA] ([ID])
);






GO
