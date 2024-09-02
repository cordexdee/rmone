CREATE TABLE [dbo].[Config_Service_ServiceQuestions] (
    [ID]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [FieldMandatory]         BIT             NULL,
    [ItemOrder]              INT             NULL,
    [NavigationUrl]          NVARCHAR (MAX)  NULL,
    [QuestionTypeProperties] NVARCHAR (MAX)  NULL,
    [ServiceSectionID]       BIGINT          NULL,
    [ServiceID]              BIGINT          NOT NULL,
    [QuestionTitle]          NVARCHAR (250)  NULL,
    [QuestionType]           NVARCHAR (MAX)  NULL,
    [TokenName]              NVARCHAR (250)  NULL,
    [Helptext]               NVARCHAR (MAX)  NULL,
    [Title]                  VARCHAR (250)   NULL,
    [TenantID]               NVARCHAR (128)  NULL,
    [Created]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]               DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]         NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                BIT             DEFAULT ((0)) NULL,
    [Attachments]            NVARCHAR (2000) DEFAULT ('') NULL,
    [NavigationType]	     NVARCHAR (250)  NULL,
    [ContinueSameLine] BIT NULL DEFAULT 0, 
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Config_Services] ([ID]),
    FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Config_Services] ([ID]),
    FOREIGN KEY ([ServiceSectionID]) REFERENCES [dbo].[Config_Service_ServiceSections] ([ID])
);






GO

GO

GO
