CREATE TABLE [dbo].[Analytic_DataIntegrations]
(
	[DataIntegrationID] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(100) NULL, 
    [Description] NVARCHAR(500) NULL, 
    [ConnectionString] NVARCHAR(100) NULL, 
    [ListName] NVARCHAR(100) NULL, 
    [FieldName] NVARCHAR(100) NULL, 
    [SourceType] SMALLINT NOT NULL, 
    [PublicKey] NVARCHAR(100) NULL, 
    [Active] BIT NOT NULL,
    [TenantID]                        NVARCHAR (128)  NOT NULL,
    [Created]                         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]                  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                         BIT             DEFAULT (0) NULL,
    [Attachments]                     NVARCHAR (2000) DEFAULT ('') NULL
)
