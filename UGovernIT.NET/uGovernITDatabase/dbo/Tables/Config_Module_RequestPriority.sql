CREATE TABLE [dbo].[Config_Module_RequestPriority] (
    [ID]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [ImpactLookup]     BIGINT          NULL,
    [ModuleNameLookup] NVARCHAR (250)  NULL,
    [PriorityLookup]   BIGINT          NULL,
    [SeverityLookup]   BIGINT          NULL,
    [Title]            VARCHAR (250)   NULL,
    [TenantID]         NVARCHAR (128)  NULL,
    [Created]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]         DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]          BIT             DEFAULT ((0)) NULL,
    [Attachments]      NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ImpactLookup]) REFERENCES [dbo].[Config_Module_Impact] ([ID]),
    FOREIGN KEY ([PriorityLookup]) REFERENCES [dbo].[Config_Module_Priority] ([ID]),
    FOREIGN KEY ([SeverityLookup]) REFERENCES [dbo].[Config_Module_Severity] ([ID]),
    FOREIGN KEY ([ModuleNameLookup], [TenantID]) REFERENCES [dbo].[Config_Modules] ([ModuleName], [TenantID]) ON UPDATE CASCADE
);










GO

GO

GO

GO
CREATE NONCLUSTERED INDEX [IX_Config_Module_RequestPriority_ModuleNameLookup] ON [dbo].[Config_Module_RequestPriority] ([ModuleNameLookup])