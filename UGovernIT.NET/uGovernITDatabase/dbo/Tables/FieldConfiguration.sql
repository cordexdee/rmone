CREATE TABLE [dbo].[FieldConfiguration] (
    [ID]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [FieldName]             NVARCHAR (250)  NULL,
    [ParentTableName]       VARCHAR (250)   NULL,
    [ParentFieldName]       VARCHAR (250)   NULL,
    [DataType]              VARCHAR (250)   NULL,
    [Data]                  NVARCHAR (450)  NULL,
    [DisplayChoicesControl] NVARCHAR (250)  NULL,
    [Multi]                 BIT             DEFAULT ((0)) NOT NULL,
    [SelectionSet]          NVARCHAR (500)  NULL,
    [Notation]              VARCHAR (500)   NULL,
    [TenantID]              NVARCHAR (128)  NULL,
    [TemplateType]          VARCHAR (500)   NULL,
    [Created]               DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]              DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]         NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]        NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]               BIT             DEFAULT ((0)) NULL,
    [Attachments]           NVARCHAR (2000) DEFAULT ('') NULL,
    [Width]                 NVARCHAR (10)   NULL,
    [TableName]             NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_FieldConfiguration] PRIMARY KEY CLUSTERED ([ID] ASC)
);








GO


CREATE INDEX [IX_FieldConfiguration_FieldName] ON [dbo].[FieldConfiguration] ([FieldName])
