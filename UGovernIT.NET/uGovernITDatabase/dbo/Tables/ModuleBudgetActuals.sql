CREATE TABLE [dbo].[ModuleBudgetActuals] (
    [ID]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [AllocationEndDate]   DATETIME        NULL,
    [AllocationStartDate] DATETIME        NULL,
    [BudgetAmount]        FLOAT (53)      NULL,
    [BudgetDescription]   NVARCHAR (MAX)  NULL,
    [ModuleBudgetLookup]  BIGINT          NULL,
    [Title]               VARCHAR (250)   NULL,
    [TicketId]            NVARCHAR (25)   NULL,
    [ModuleNameLookup]    NVARCHAR (20)   NULL,
    [VendorName]          NVARCHAR (250)  NULL,
    [InvoiceNumber]       NVARCHAR (250)  NULL,
    [VendorLookup]        BIGINT          NULL,
    [TenantID]            NVARCHAR (128)  NULL,
    [Created]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]             BIT             DEFAULT ((0)) NULL,
    [Attachments]         NVARCHAR (2000) DEFAULT ('') NULL,
	[DepartmentLookup]    BIGINT          NULL,
	[BudgetItem]          NVARCHAR (250)  NULL,
    [ActualCost] FLOAT NULL, 
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ModuleBudgetLookup]) REFERENCES [dbo].[ModuleBudget] ([ID]),
    FOREIGN KEY ([VendorLookup]) REFERENCES [dbo].[AssetVendors] ([ID]),
	FOREIGN KEY ([DepartmentLookup]) REFERENCES [dbo].[Department] ([ID])
);








GO

GO

GO