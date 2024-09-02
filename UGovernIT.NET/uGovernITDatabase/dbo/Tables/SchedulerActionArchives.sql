CREATE TABLE [dbo].[SchedulerActionArchives] (
    [ID]                BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActionType]        NVARCHAR (MAX)  NULL,
    [ActionTypeData]    NVARCHAR (MAX)  NULL,
    [AgentJobStatus]    NVARCHAR (MAX)  NULL,
    [AlertCondition]    NVARCHAR (250)  NULL,
    [AttachmentFormat]  NVARCHAR (MAX)  NULL,
    [CustomProperties]  NVARCHAR (MAX)  NULL,
    [EmailBody]         NVARCHAR (MAX)  NULL,
    [EmailIDCC]         NVARCHAR (MAX)  NULL,
    [EmailIDTo]         NVARCHAR (MAX)  NULL,
    [ListName]          NVARCHAR (250)  NULL,
    [Log]               NVARCHAR (MAX)  NULL,
    [MailSubject]       NVARCHAR (250)  NULL,
    [ModuleNameLookup]  NVARCHAR (250)  NULL,
    [Recurring]         BIT             NULL,
    [RecurringEndDate]  DATETIME        NULL,
    [RecurringInterval] INT             NULL,
    [StartTime]         DATETIME        NULL,
    [TicketId]          NVARCHAR (250)  NULL,
    [Title]             NVARCHAR (250)  NULL,
    [TenantID]          NVARCHAR (128)  NULL,
    [Created]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]           BIT             DEFAULT ((0)) NULL,
    [Attachments]       NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_SchedulerActionArchives] PRIMARY KEY CLUSTERED ([ID] ASC)
);





