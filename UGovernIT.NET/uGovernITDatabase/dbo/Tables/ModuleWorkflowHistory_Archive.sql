CREATE TABLE [dbo].[ModuleWorkflowHistory_Archive] (
    [ID]                BIGINT          NOT NULL,
    [ActionUserType]    NVARCHAR (250)  NULL,
    [Duration]          INT             NULL,
    [ModuleNameLookup]  NVARCHAR (250)  NULL,
    [OnHoldDuration]    INT             NULL,
    [SLAMet]            BIT             NULL,
    [StageClosedBy]     NVARCHAR (MAX)  NULL,
    [StageClosedByName] NVARCHAR (250)  NULL,
    [StageEndDate]      DATETIME        NULL,
    [StageStartDate]    DATETIME        NULL,
    [StageStep]         INT             NULL,
    [TicketId]          NVARCHAR (250)  NULL,
    [Title]             VARCHAR (250)   NULL,
    [TenantID]          NVARCHAR (128)  NULL,
    [Created]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]           BIT             DEFAULT ((0)) NULL,
    [Attachments]       NVARCHAR (2000) DEFAULT ('') NULL
);







