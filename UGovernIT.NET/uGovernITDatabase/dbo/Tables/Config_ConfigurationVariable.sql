CREATE TABLE [dbo].[Config_ConfigurationVariable] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [CategoryName]   NVARCHAR (250)  NULL,
    [Description]    NVARCHAR (MAX)  NULL,
    [KeyName]        NVARCHAR (250)  NULL,
    [KeyValue]       NVARCHAR (MAX)  NULL,
    [Title]          NVARCHAR (250)  NULL,
    [Type]           NVARCHAR (100)  CONSTRAINT [DF_Config_ConfigurationVariable_Type] DEFAULT ('') NULL,
    [Internal]       BIT             CONSTRAINT [DF_Config_ConfigurationVariable_Internal] DEFAULT ((0)) NULL,
    [TenantID]       NVARCHAR (128)  NULL,
    [Created]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT             DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_Config_ConfigurationVariable] PRIMARY KEY CLUSTERED ([ID] ASC)
);










GO

GO
CREATE NONCLUSTERED INDEX [IX_Config_ConfigurationVariable_KeyName] ON [dbo].[Config_ConfigurationVariable] ([KeyName])