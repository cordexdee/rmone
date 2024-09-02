CREATE TABLE [dbo].[Analytic_ModelOutputMappers]
(
	[ID] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [ModelVersionID] BIGINT NULL, 
    [Yaxis] NVARCHAR(100) NULL, 
    [Xaxis] NVARCHAR(100) NULL, 
    [Title] NVARCHAR(250) NULL, 
    [ConfigString] NVARCHAR(MAX) NULL, 
    [Rows] INT NOT NULL, 
    [Columns] INT NOT NULL, 
    [Activated] INT NOT NULL, 
    [MapperType] NVARCHAR(50) NULL,
    [TenantID]                        NVARCHAR (128)  NOT NULL,
    [Created]                         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]                  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                         BIT             DEFAULT (0) NULL,
    [Attachments]                     NVARCHAR (2000) DEFAULT ('') NULL
)
