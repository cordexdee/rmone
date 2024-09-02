﻿CREATE TABLE [dbo].[VendorKeyPersonnel] (
    [ID]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [Description]         NVARCHAR (MAX)  NULL,
    [EmailAddress]        NVARCHAR (250)  NULL,
    [EndDate]             DATETIME        NULL,
    [StartDate]           DATETIME        NULL,
    [VendorMSALookup]     BIGINT          NOT NULL,
    [VendorMSANameLookup] BIGINT          NOT NULL,
    [Title]               VARCHAR (250)   NULL,
    [TenantID]            NVARCHAR (128)  NULL,
    [Created]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]             BIT             DEFAULT ((0)) NULL,
    [Attachments]         NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([VendorMSALookup]) REFERENCES [dbo].[VendorMSA] ([ID]),
    FOREIGN KEY ([VendorMSANameLookup]) REFERENCES [dbo].[VendorMSA] ([ID])
);






GO

GO