CREATE TABLE [dbo].[Config_Module_FormLayout] (
    [ID]                BIGINT          IDENTITY (1, 1) NOT NULL,
    [ColumnType]        NVARCHAR (250)  NULL,
    [CustomProperties]  NVARCHAR (MAX)  NULL,
    [FieldDisplayName]  NVARCHAR (250)  NULL,
    [FieldDisplayWidth] INT             NULL,
    [FieldName]         NVARCHAR (250)  NULL,
    [FieldSequence]     INT             NULL,
    [FieldType]         NVARCHAR (250)  NULL,
    [HideInTemplate]    BIT             DEFAULT ((0)) NOT NULL,
    [ModuleNameLookup]  NVARCHAR (250)  NULL,
    [ShowInMobile]      BIT             DEFAULT ((0)) NOT NULL,
    [SkipOnCondition]   NVARCHAR (MAX)  NULL,
    [TabId]             INT             NULL,
    [TargetType]        NVARCHAR (250)  NULL,
    [TargetURL]         NVARCHAR (250)  NULL,
    [ToolTip]           NVARCHAR (250)  NULL,
    [TrimContentAfter]  INT             DEFAULT ((0)) NOT NULL,
    [Title]             VARCHAR (250)   NULL,
    [TenantID]          NVARCHAR (128)  NULL,
    [Created]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]           BIT             DEFAULT ((0)) NULL,
    [Attachments]       NVARCHAR (2000) DEFAULT ('') NULL,
    FOREIGN KEY ([ModuleNameLookup], [TenantID]) REFERENCES [dbo].[Config_Modules] ([ModuleName], [TenantID]) ON UPDATE CASCADE
);










GO
CREATE NONCLUSTERED INDEX [IX_Config_Module_FormLayout_ModuleNameLookup] ON [dbo].[Config_Module_FormLayout] ([ModuleNameLookup])