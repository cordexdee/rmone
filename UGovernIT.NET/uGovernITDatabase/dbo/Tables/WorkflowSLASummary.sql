CREATE TABLE [dbo].[WorkflowSLASummary] (
    [ID]                BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActualTime]        INT             NULL,
    [Closed]            BIT             NULL,
    [DueDate]           DATETIME        NULL,
    [EndStageName]      NVARCHAR (250)  NULL,
    [EndStageStep]      INT             NULL,
    [ModuleNameLookup]  NVARCHAR (250)  NULL,
    [OnHold]            INT             NULL,
    [RuleNameLookup]    BIGINT          NULL,
    [ServiceLookup]     BIGINT          NULL,
    [SLACategoryChoice] NVARCHAR (MAX)  NULL,
    [SLARuleName]       NVARCHAR (250)  NULL,
    [StageEndDate]      DATETIME        NULL,
    [StageStartDate]    DATETIME        NULL,
    [StartStageName]    NVARCHAR (250)  NULL,
    [StartStageStep]    INT             NULL,
    [TargetTime]        INT             NULL,
    [TicketId]          NVARCHAR (250)  NULL,
    [Title]             VARCHAR (250)   NULL,
    [TotalHoldDuration] FLOAT             NULL,
    [Use24x7Calendar]   BIT             DEFAULT ((0)) NOT NULL,
    [TenantID]          NVARCHAR (128)  NULL,
    [Created]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]           BIT             DEFAULT ((0)) NULL,
    [Attachments]       NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([RuleNameLookup]) REFERENCES [dbo].[Config_Module_SLARule] ([ID])
);








GO
CREATE NONCLUSTERED INDEX [IX_WorkflowSLASummary_TicketId] ON [dbo].[WorkflowSLASummary] ([SLARuleName],[TicketId])