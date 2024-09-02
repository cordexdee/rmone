CREATE TABLE [dbo].[Config_Module_TaskEmails] (
    [ID]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [CustomProperties]       NVARCHAR (MAX)  NULL,
    [EmailBody]              NVARCHAR (MAX)  NULL,
    [EmailIDCC]              NVARCHAR (MAX)  NULL,
    [EmailTitle]             NVARCHAR (250)  NULL,
    [EmailUserTypes]         NVARCHAR (250)  NULL,
    [HideFooter]             BIT             NULL,
    [ModuleNameLookup]       NVARCHAR (250)  NULL,
    [StageStep]              INT             NULL,
    [SendEvenIfStageSkipped] BIT             NULL,
    [Status]                 NVARCHAR (250)  NULL,
    [Title]                  VARCHAR (250)   NULL,
    [TenantID]               NVARCHAR (128)  NULL,
    [Created]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]               DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]         NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                BIT             DEFAULT ((0)) NULL,
    [Attachments]            NVARCHAR (2000) DEFAULT ('') NULL,
    [TicketPriorityLookup]   BIGINT          NULL,
    [NotifyInPlainText]      BIT             DEFAULT ((0)) NOT NULL,
    [EmailEventType]         NVARCHAR (200)  NULL,
    FOREIGN KEY ([ModuleNameLookup], [TenantID]) REFERENCES [dbo].[Config_Modules] ([ModuleName], [TenantID]) ON UPDATE CASCADE
);










GO
CREATE NONCLUSTERED INDEX [IX_Config_Module_TaskEmails_ModuleNameLookup] ON [dbo].[Config_Module_TaskEmails] ([ModuleNameLookup], [Status])