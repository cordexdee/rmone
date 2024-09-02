CREATE TABLE [dbo].[Config_Service_ServiceDefaultValues] (
    [ID]                BIGINT          IDENTITY (1, 1) NOT NULL,
    [ColumnName]        NVARCHAR (250)  NULL,
    [ColumnValue]       NVARCHAR (MAX)  NULL,
    [PickValueFrom]     NVARCHAR (250)  NULL,
    [ServiceID]         BIGINT          NULL,
    [ServiceQuestionID] BIGINT          NULL,
    [ServiceTaskID]     BIGINT          NULL,
    [Title]             VARCHAR (250)   NULL,
    [TenantID]          NVARCHAR (128)  NULL,
    [Created]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]           BIT             DEFAULT ((0)) NULL,
    [Attachments]       NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Config_Services] ([ID]),
    FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Config_Services] ([ID]) ON UPDATE CASCADE,
    FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Config_Services] ([ID]),
    FOREIGN KEY ([ServiceQuestionID]) REFERENCES [dbo].[Config_Service_ServiceQuestions] ([ID]) ON UPDATE CASCADE,
    FOREIGN KEY ([ServiceQuestionID]) REFERENCES [dbo].[Config_Service_ServiceQuestions] ([ID]),
    FOREIGN KEY ([ServiceTaskID]) REFERENCES [dbo].[ModuleTasks] ([ID]) ON UPDATE CASCADE,
    FOREIGN KEY ([ServiceTaskID]) REFERENCES [dbo].[ModuleTasks] ([ID])
);






GO

GO

GO

GO

GO

GO

GO
