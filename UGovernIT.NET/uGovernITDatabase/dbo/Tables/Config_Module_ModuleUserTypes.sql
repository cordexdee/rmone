CREATE TABLE [dbo].[Config_Module_ModuleUserTypes] (
    [ID]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [ColumnName]       NVARCHAR (250)  NULL,
    [CustomProperties] NVARCHAR (MAX)  NULL,
    [DefaultUser]      NVARCHAR (MAX)  NULL,
    [GroupsUser]       NVARCHAR (250)  NULL,
    [ITOnly]           BIT             NULL,
    [ManagerOnly]      BIT             NULL,
    [ModuleNameLookup] NVARCHAR (250)  NULL,
    [UserTypes]        NVARCHAR (MAX)  NULL,
    [Title]            VARCHAR (250)   NULL,
    [TenantID]         NVARCHAR (128)  NULL,
    [Created]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]         DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]          BIT             DEFAULT ((0)) NULL,
    [Attachments]      NVARCHAR (2000) DEFAULT ('') NULL,
    FOREIGN KEY ([ModuleNameLookup], [TenantID]) REFERENCES [dbo].[Config_Modules] ([ModuleName], [TenantID]) ON UPDATE CASCADE
);










GO
CREATE NONCLUSTERED INDEX [IX_Config_Config_Module_ModuleUserTypes_ModuleNameLookup] ON [dbo].[Config_Module_ModuleUserTypes] ([ModuleNameLookup])