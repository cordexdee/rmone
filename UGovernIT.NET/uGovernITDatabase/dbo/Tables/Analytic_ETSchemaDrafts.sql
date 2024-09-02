CREATE TABLE [dbo].[Analytic_ETSchemaDrafts]
(
	[ETSchemaDraftID] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [TableName] NVARCHAR(50) NULL, 
    [UserName] NVARCHAR(50) NULL, 
    [SelectedTables] NVARCHAR(100) NULL, 
    [SelectedSchema] NVARCHAR(100) NULL,
    [TenantID]                        NVARCHAR (128)  NOT NULL,
    [Created]                         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]                  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                         BIT             DEFAULT (0) NULL,
    [Attachments]                     NVARCHAR (2000) DEFAULT ('') NULL
)
