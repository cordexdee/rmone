CREATE TABLE [dbo].[Analytic_Interpretations]
(
	[InterpretationId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [ModelVersionID] BIGINT NOT NULL, 
    [Title] NVARCHAR(250) NULL, 
    [Description] NVARCHAR(500) NULL, 
    [InterpretationText] NVARCHAR(100) NULL, 
    [Expression] NVARCHAR(100) NULL, 
    [Activated] INT NOT NULL DEFAULT 0, 
    [Scope] NVARCHAR(50) NULL, 
    [ScopeId] NVARCHAR(50) NULL, 
    [ExpressionXml] NVARCHAR(MAX) NULL,
    [TenantID]                        NVARCHAR (128)  NOT NULL,
    [Created]                         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]                  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                         BIT             DEFAULT (0) NULL,
    [Attachments]                     NVARCHAR (2000) DEFAULT ('') NULL
)
