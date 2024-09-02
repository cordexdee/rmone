CREATE TABLE [dbo].[ResourceUsageSummaryWeekWise] (
    [ID]                        BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActualHour]                FLOAT (53)      NULL,
    [AllocationHour]            FLOAT (53)      NULL,
    [FunctionalAreaTitleLookup] NVARCHAR (250)  NULL,
    [IsConsultant]              BIT             NULL,
    [IsIT]                      BIT             NULL,
    [IsManager]                 BIT             NULL,
    [ManagerLookup]             NVARCHAR (250)  NULL,
    [ManagerName]               NVARCHAR (250)  NULL,
    [PctActual]                 FLOAT (53)      NULL,
    [PctAllocation]             FLOAT (53)      NULL,
    [PctPlannedAllocation]      FLOAT (53)      NULL,
    [PlannedAllocationHour]     FLOAT (53)      NULL,
    [ResourceUser]              NVARCHAR (MAX)  NULL,
    [ResourceNameUser]          NVARCHAR (250)  NULL,
    [SubWorkItem]               NVARCHAR (250)  NULL,
    [WeekStartDate]             DATETIME        NULL,
    [WorkItem]                  NVARCHAR (250)  NULL,
    [WorkItemID]                BIGINT          NULL,
    [WorkItemType]              NVARCHAR (250)  NULL,
    [Title]                     VARCHAR (250)   NULL,
    [GlobalRoleID]         nvarchar(128) NULL,
    [TenantID]                  NVARCHAR (128)  NULL,
    [Created]                   DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                  DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]             NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]            NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                   BIT             DEFAULT ((0)) NULL,
    [Attachments]               NVARCHAR (2000) DEFAULT ('') NULL,
    [SoftAllocation]            BIT             DEFAULT ((0)) NULL,
    [FunctionalArea]            NVARCHAR (2000) NULL,
    [ERPJobID]                  NVARCHAR (500) NULL,
    [ActualStartDate] Datetime null,
    [ActualEndDate] Datetime null,
    [ActualPctAllocation] float null,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);







