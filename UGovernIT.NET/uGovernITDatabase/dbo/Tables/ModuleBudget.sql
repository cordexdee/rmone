CREATE TABLE [dbo].[ModuleBudget] (
    [ID]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [AllocationEndDate]   DATETIME        NULL,
    [AllocationStartDate] DATETIME        NULL,
    [IsAutoCalculated]    BIT             NULL,
    [BudgetAmount]        FLOAT (53)      NULL,
    [BudgetDescription]   NVARCHAR (250)  NULL,
    [BudgetItem]          NVARCHAR (250)  NULL,
    [BudgetCategoryLookup]        BIGINT          NULL,
    [BudgetStatus]        INT             NULL,
    [Comment]             NVARCHAR (MAX)  NULL,
    [DepartmentLookup]    BIGINT          NULL,
    [UnapprovedAmount]    FLOAT (53)      NULL,
    [GLCode]              NVARCHAR (250)  NULL,
    [ModuleNameLookup]    NVARCHAR (10)   NULL,
    [TicketId]            NVARCHAR (25)   NULL,
    [Title]               NVARCHAR (250)  NULL,
    [TenantID]            NVARCHAR (128)  NULL,
    [Created]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]             BIT             DEFAULT ((0)) NULL,
    [Attachments]         NVARCHAR (2000) DEFAULT ('') NULL,
    [ResourceLookup]      INT             DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([DepartmentLookup]) REFERENCES [dbo].[Department] ([ID])
);








GO

Go



