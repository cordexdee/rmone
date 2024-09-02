CREATE TABLE [dbo].[VendorSOWFees] (
    [ID]                              BIGINT          IDENTITY (1, 1) NOT NULL,
    [BudgetAmount]                    NVARCHAR (250)  NULL,
    [Description]                     NVARCHAR (MAX)  NULL,
    [EndDate]                         DATETIME        NULL,
    [FixedFees]                       NVARCHAR (250)  NULL,
    [SOWAdditionalUnitRate]           NVARCHAR (250)  NULL,
    [SOWAnnualChangePct]              INT             NULL,
    [SOWDeadBandPct]                  INT             NULL,
    [SOWFeeUnit]                      NVARCHAR (MAX)  NULL,
    [SOWFeeUnit2]                     NVARCHAR (MAX)  NULL,
    [SOWNoOfUnit]                     INT             NULL,
    [SOWReducedUnitRate]              NVARCHAR (250)  NULL,
    [SOWUnitRate]                     NVARCHAR (250)  NULL,
    [StartDate]                       DATETIME        NULL,
    [VendorPOLineItemLookup]          BIGINT          NOT NULL,
    [VendorPOLookup]                  BIGINT          NOT NULL,
    [VendorResourceCategoryLookup]    BIGINT          NOT NULL,
    [VendorResourceSubCategoryLookup] BIGINT          NOT NULL,
    [VendorSOWLookup]                 BIGINT          NOT NULL,
    [VendorSOWNameLookup]             BIGINT          NOT NULL,
    [Title]                           VARCHAR (250)   NULL,
    [TenantID]                        NVARCHAR (128)  NULL,
    [Created]                         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]                  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                         BIT             DEFAULT ((0)) NULL,
    [Attachments]                     NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([VendorPOLineItemLookup]) REFERENCES [dbo].[VendorPOLineItems] ([ID]),
    FOREIGN KEY ([VendorPOLookup]) REFERENCES [dbo].[VendorPO] ([ID]),
    FOREIGN KEY ([VendorResourceCategoryLookup]) REFERENCES [dbo].[Config_VendorResourceCategory] ([ID]),
    FOREIGN KEY ([VendorResourceSubCategoryLookup]) REFERENCES [dbo].[Config_VendorResourceCategory] ([ID]),
    FOREIGN KEY ([VendorSOWLookup]) REFERENCES [dbo].[VendorSOW] ([ID]),
    FOREIGN KEY ([VendorSOWNameLookup]) REFERENCES [dbo].[VendorSOW] ([ID])
);






GO

GO

GO

GO

GO

GO
