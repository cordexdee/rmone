CREATE TABLE [dbo].[Config_ModuleLifeCycles] (
    [ID]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [Description]      NVARCHAR (MAX)  NULL,
    [ItemOrder]        INT             NULL,
    [Name]             NVARCHAR (250)  NULL,
    [ModuleNameLookup] NVARCHAR (250)  NULL,
    [TenantID]         NVARCHAR (128)  NULL,
    [Created]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]         DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]          BIT             DEFAULT ((0)) NULL,
    [Attachments]      NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_Config_ModuleLifeCycles] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Config_ModuleLifeCycles_ToTable] FOREIGN KEY ([ModuleNameLookup], [TenantID]) REFERENCES [dbo].[Config_Modules] ([ModuleName], [TenantID])
);







GO
CREATE NONCLUSTERED INDEX [IX_Config_ProjectLifeCycleStages_ModuleNameLookup] ON [dbo].[Config_ProjectLifeCycleStages] ([Deleted])

