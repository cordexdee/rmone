CREATE TABLE [dbo].[Config_ClientAdminConfigurationLists] (
    [ID]                        BIGINT          IDENTITY (1, 1) NOT NULL,
    [ClientAdminCategoryLookup] BIGINT          NOT NULL,
    [Description]               NVARCHAR (MAX)  NULL,
    [ListName]                  NVARCHAR (250)  NULL,
    [TabSequence]               INT             NULL,
    [Title]                     VARCHAR (250)   NULL,
    [TenantID]                  NVARCHAR (128)  NULL,
    [Created]                   DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                  DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]             NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]            NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                   BIT             DEFAULT ((0)) NULL,
    [Attachments]               NVARCHAR (2000) DEFAULT ('') NULL,
    [AuthorizedToViewUser] NVARCHAR(500) NULL, 
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ClientAdminCategoryLookup]) REFERENCES [dbo].[Config_ClientAdminCategory] ([ID]),
    FOREIGN KEY ([ClientAdminCategoryLookup]) REFERENCES [dbo].[Config_ClientAdminCategory] ([ID]) ON UPDATE CASCADE
);






GO

GO
