CREATE TABLE [dbo].[Analytic_ModelInputs]
(
	[ModelInputID] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [InputXml] NVARCHAR(MAX) NULL, 
    [Title] NVARCHAR(250) NULL, 
    [Description] NVARCHAR(1000) NULL, 
    [ModelVersionID] BIGINT NULL,
    [TenantID]                        NVARCHAR (128)  NOT NULL,
    [Created]                         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]                  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                         BIT             DEFAULT (0) NULL,
    [Attachments]                     NVARCHAR (2000) DEFAULT ('') NULL
)
