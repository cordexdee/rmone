CREATE TABLE [dbo].[ITGMonthlyBudget] (
    [ID]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActualCost]            NVARCHAR (250)  NULL,
    [AllocationStartDate]   DATETIME        NULL,
    [BudgetAmount]          NVARCHAR (250)  NULL,
    [BudgetLookup]          BIGINT          NOT NULL,
    [BudgetType]            NVARCHAR (250)  NULL,
    [EstimatedCost]         NVARCHAR (250)  NULL,
    [NonProjectActualTotal] NVARCHAR (250)  NULL,
    [NonProjectPlanedTotal] NVARCHAR (250)  NULL,
    [ProjectActualTotal]    NVARCHAR (250)  NULL,
    [ProjectPlanedTotal]    NVARCHAR (250)  NULL,
    [ResourceCost]          NVARCHAR (250)  NULL,
    [Title]                 VARCHAR (250)   NULL,
    [TenantID]              NVARCHAR (128)  NULL,
    [Created]               DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]              DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]         NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]        NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]               BIT             DEFAULT ((0)) NULL,
    [Attachments]           NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);









