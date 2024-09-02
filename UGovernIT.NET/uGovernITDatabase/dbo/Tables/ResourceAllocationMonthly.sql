CREATE TABLE [dbo].[ResourceAllocationMonthly] (
    [ID]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [MonthStartDate]         DATETIME        NULL,
    [PctAllocation]          FLOAT (53)      NULL,
    [PctPlannedAllocation]   FLOAT (53)      NULL,
    [ResourceUser]           NVARCHAR (MAX)  NULL,
    [ResourceSubWorkItem]    NVARCHAR (250)  NULL,
    [ResourceWorkItem]       NVARCHAR (250)  NULL,
    [ResourceWorkItemLookup] BIGINT          NOT NULL,
    [ResourceWorkItemType]   NVARCHAR (250)  NULL,
    [Title]                  NVARCHAR (250)  NULL,
    [TenantID]               NVARCHAR (128)  NULL,
    [Created]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]               DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]         NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                BIT             DEFAULT ((0)) NULL,
    [Attachments]            NVARCHAR (2000) DEFAULT ('') NULL,
    [ActualStartDate] Datetime null,
    [ActualEndDate] Datetime null,
    [ActualPctAllocation] float null,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ResourceWorkItemLookup]) REFERENCES [dbo].[ResourceWorkItems] ([ID])
);






GO
