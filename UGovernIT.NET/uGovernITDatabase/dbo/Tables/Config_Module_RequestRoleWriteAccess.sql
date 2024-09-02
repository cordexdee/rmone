CREATE TABLE [dbo].[Config_Module_RequestRoleWriteAccess] (
    [ID]                   BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActionUser]           NVARCHAR (250)  NULL,
    [CustomProperties]     NVARCHAR (MAX)  NULL,
    [FieldMandatory]       BIT             NULL,
    [FieldName]            NVARCHAR (250)  NULL,
    [HideInServiceMapping] BIT             DEFAULT ((0)) NULL,
    [ModuleNameLookup]     NVARCHAR (250)  NULL,
    [StageStep]            INT             NULL,
    [ShowEditButton]       BIT             NULL,
    [ShowWithCheckBox]     BIT             NULL,
    [Title]                VARCHAR (250)   NULL,
    [TenantID]             NVARCHAR (128)  NULL,
    [Created]              DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]        NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]              BIT             DEFAULT ((0)) NULL,
    [Attachments]          NVARCHAR (2000) DEFAULT ('') NULL,
    FOREIGN KEY ([ModuleNameLookup], [TenantID]) REFERENCES [dbo].[Config_Modules] ([ModuleName], [TenantID]) ON UPDATE CASCADE
);










GO
CREATE NONCLUSTERED INDEX [IX_Config_Module_RequestRoleWriteAccess_ModuleNameLookup] ON [dbo].[Config_Module_RequestRoleWriteAccess] ([ModuleNameLookup])