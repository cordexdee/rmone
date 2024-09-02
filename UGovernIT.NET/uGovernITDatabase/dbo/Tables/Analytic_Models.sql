CREATE TABLE [dbo].[Analytic_Models]
(
	[ModelID] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Description] NVARCHAR(500) NULL, 
    [ModelCategoryID] BIGINT NULL, 
    [Title] NVARCHAR(250) NULL, 
    [CurrentActiveVersionID] BIGINT NULL,
    [TenantID]                        NVARCHAR (128)  NOT NULL,
    [Created]                         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]                  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                         BIT             DEFAULT (0) NULL,
    [Attachments]                     NVARCHAR (2000) DEFAULT ('') NULL, 
    CONSTRAINT [FK_Analytic_Models_ModelCategories] FOREIGN KEY ([ModelCategoryID]) REFERENCES Analytic_ModelCategories([ModelCategoryID])
)
