CREATE TABLE [dbo].[Analytic_ModelVersions]
(
	[ModelVersionID] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [ModelXml] NVARCHAR(MAX) NULL, 
    [HelpXml] NVARCHAR(MAX) NULL, 
    [ScoreSegmentXml] NVARCHAR(MAX) NULL, 
    [Comment] NVARCHAR(500) NULL, 
    [Status] TINYINT NULL, 
    [ModelID] BIGINT NULL, 
    [ParentID] BIGINT NULL, 
    [RevisionID] UNIQUEIDENTIFIER NULL, 
    [VersionNumber] INT NULL, 
    [ScoreType] TINYINT NULL,
    [TenantID]                        NVARCHAR (128)  NOT NULL,
    [Created]                         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]                  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                         BIT             DEFAULT (0) NULL,
    [Attachments]                     NVARCHAR (2000) DEFAULT ('') NULL
)
