CREATE TABLE [dbo].[Config_Module_DefaultValues] (
    [ID]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [CustomProperties] NVARCHAR (MAX)  NULL,
    [KeyName]          NVARCHAR (250)  NULL,
    [KeyValue]         NVARCHAR (MAX)  NULL,
    [ModuleNameLookup] NVARCHAR (250)  NULL,
    [ModuleStepLookup] NVARCHAR (250)  NULL,
    [Title]            VARCHAR (250)   NULL,
    [TenantID]         NVARCHAR (128)  NULL,
    [Created]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]         DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]          BIT             DEFAULT ((0)) NULL,
    [Attachments]      NVARCHAR (2000) DEFAULT ('') NULL
);



GO
CREATE NONCLUSTERED INDEX [IX_Config_Module_DefaultValues_KeyName] ON [dbo].[Config_Module_DefaultValues] ([KeyName])

GO
CREATE NONCLUSTERED INDEX [IX_Config_Module_DefaultValues_ModuleNameLookup] ON [dbo].[Config_Module_DefaultValues] ([ModuleNameLookup])

GO
CREATE NONCLUSTERED INDEX [IX_Config_Module_DefaultValues_ModuleStepLookup] ON [dbo].[Config_Module_DefaultValues] ([ModuleStepLookup])


