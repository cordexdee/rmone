CREATE TABLE [dbo].[ModuleMonthlyBudget] (
    [ID]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [AllocationStartDate]   DATETIME        NULL,
    [ActualCost]            FLOAT (53)      NULL,
    [BudgetAmount]          FLOAT (53)      NULL,
    [BudgetType]            NVARCHAR (250)  NULL,
    [TicketId]              NVARCHAR (25)   NULL,
    [ModuleNameLookup]      NVARCHAR (25)   NULL,
    [BudgetCategoryLookup]          BIGINT          NULL,
    [EstimatedCost]         FLOAT (53)      NULL,
    [NonProjectActualTotal] FLOAT (53)      NULL,
    [NonProjectPlanedTotal] FLOAT (53)      NULL,
    [ProjectActualTotal]    FLOAT (53)      NULL,
    [ProjectPlanedTotal]    FLOAT (53)      NULL,
    [ResourceCost]          FLOAT (53)      NULL,
    [Title]                 NVARCHAR (250)  NULL,
    [TenantID]              NVARCHAR (128)  NULL,
    [Created]               DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]              DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]         NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]        NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]               BIT             DEFAULT ((0)) NULL,
    [Attachments]           NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);










