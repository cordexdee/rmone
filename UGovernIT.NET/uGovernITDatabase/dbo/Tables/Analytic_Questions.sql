CREATE TABLE [dbo].[Analytic_Questions]
(
	[QuestionId] BIGINT NOT NULL PRIMARY KEY, 
    [SurveyId] BIGINT NOT NULL, 
    [QuestionType] NVARCHAR(50) NULL, 
    [QuestionDesc] NVARCHAR(500) NULL, 
    [Section] NVARCHAR(50) NULL, 
    [ItemOrder] INT NOT NULL, 
    [Token] NVARCHAR(50) NULL, 
    [QuestionTypeProperties] NVARCHAR(500) NULL, 
    [Mandatory] BIT NOT NULL, 
    [HelpText] NVARCHAR(500) NULL,
    [TenantID]                        NVARCHAR (128)  NOT NULL,
    [Created]                         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]                  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                         BIT             DEFAULT (0) NULL,
    [Attachments]                     NVARCHAR (2000) DEFAULT ('') NULL
)
