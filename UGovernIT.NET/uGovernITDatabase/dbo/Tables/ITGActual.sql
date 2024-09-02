CREATE TABLE [dbo].[ITGActual] (
    [ID]              BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActualCost]      NVARCHAR (250)  NULL,
    [BudgetEndDate]   DATETIME        NULL,
    [BudgetStartDate] DATETIME        NULL,
    [Description]     NVARCHAR (MAX)  NULL,
    [InvoiceNumber]   NVARCHAR (250)  NULL,
    [ITGBudgetLookup] BIGINT          NOT NULL,
    [VendorLookup]    BIGINT          NOT NULL,
    [Title]           VARCHAR (250)   NULL,
    [TenantID]        NVARCHAR (128)  NULL,
    [Created]         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]         BIT             DEFAULT ((0)) NULL,
    [Attachments]     NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ITGBudgetLookup]) REFERENCES [dbo].[ITGBudget] ([ID]),
    FOREIGN KEY ([VendorLookup]) REFERENCES [dbo].[Vendors] ([ID])
);






GO

GO
