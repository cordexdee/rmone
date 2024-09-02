CREATE TABLE [dbo].[Analytic_ETSourceInfoes]
(
	[ETSchemaInfoID] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [AliasName] NVARCHAR(100) NULL, 
    [ETTableID] BIGINT NOT NULL, 
    [FieldName] NVARCHAR(50) NULL, 
    [ForeignKey] NVARCHAR(50) NULL, 
    [DataIntegrationID] BIGINT NOT NULL , 
    [PrimaryKey] NVARCHAR(50) NULL, 
    [FieldConstraint] NVARCHAR(50) NULL, 
    [DataType] NVARCHAR(50) NULL, 
    [AggregateFunction] NVARCHAR(100) NULL,
    [TenantID]                        NVARCHAR (128)  NOT NULL,
    [Created]                         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]                  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                         BIT             DEFAULT (0) NULL,
    [Attachments]                     NVARCHAR (2000) DEFAULT ('') NULL
)
