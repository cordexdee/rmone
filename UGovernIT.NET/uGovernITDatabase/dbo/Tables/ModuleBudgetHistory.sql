CREATE TABLE [dbo].[ModuleBudgetHistory] (
    [ID]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [AllocationEndDate]   DATETIME        NULL,
    [AllocationStartDate] DATETIME        NULL,
    [BaselineDate]        DATETIME        NULL,
    [BaselineId]          INT             NULL,
    [BudgetAmount]        FLOAT (53)      NULL,
    [BudgetDescription]   NVARCHAR (MAX)  NULL,
    [BudgetItem]          NVARCHAR (250)  NULL,
    [BudgetLookup]        BIGINT          NULL,
    [PMMIdLookup]         BIGINT          NULL,
    [Title]               VARCHAR (250)   NULL,
    [BudgetStatus]        INT             NULL,
    [Comment]             NVARCHAR (MAX)  NULL,
    [IsAutoCalculated]    BIT             NULL,
    [UnapprovedAmount]    FLOAT (53)      NULL,
    [GLCode]              NVARCHAR (250)  NULL,
    [ModuleNameLookup]    NVARCHAR (10)   NULL,
    [ResourceLookup]      INT             NOT NULL,
    [TicketId]            NVARCHAR (25)   NULL,
    [DepartmentLookup]    BIGINT          NULL,
    [TenantID]            NVARCHAR (128)  NULL,
    [Created]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]             BIT             DEFAULT ((0)) NULL,
    [Attachments]         NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([DepartmentLookup]) REFERENCES [dbo].[Department] ([ID])
);








GO

GO
