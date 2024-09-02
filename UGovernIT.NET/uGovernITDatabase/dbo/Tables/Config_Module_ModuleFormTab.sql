CREATE TABLE [dbo].[Config_Module_ModuleFormTab] (
    [ID]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [AuthorizedToEdit] NVARCHAR (250)  NULL,
    [AuthorizedToView] NVARCHAR (250)  NULL,
    [CustomProperties] NVARCHAR (MAX)  NULL,
    [ModuleNameLookup] NVARCHAR (250)  NULL,
    [TabId]            INT             DEFAULT ((0)) NULL,
    [TabName]          NVARCHAR (250)  NULL,
    [TabSequence]      INT             NULL,
    [Title]            VARCHAR (250)   NULL,
    [TenantID]         NVARCHAR (128)  NULL,
    [Created]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]         DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]          BIT             DEFAULT ((0)) NULL,
    [Attachments]      NVARCHAR (2000) DEFAULT ('') NULL,
    [ShowInMobile] BIT NULL DEFAULT ((1)), 
    FOREIGN KEY ([ModuleNameLookup], [TenantID]) REFERENCES [dbo].[Config_Modules] ([ModuleName], [TenantID]) ON UPDATE CASCADE
);










GO
CREATE NONCLUSTERED INDEX [IX_Config_Module_ModuleFormTab_ModuleNameLookup] ON [dbo].[Config_Module_ModuleFormTab] ([ModuleNameLookup])