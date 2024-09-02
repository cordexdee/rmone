CREATE TABLE [dbo].[TaskTemplateItems] (
    [ID]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [AssignedToUser]     NVARCHAR (MAX)  NULL,
    [AssignToPct]        NVARCHAR (MAX)  NULL,
    [Body]               NVARCHAR (MAX)  NULL,
    [ChildCount]         INT             NULL,
    [Comment]            NVARCHAR (MAX)  NULL,
    [Contribution]       INT             NULL,
    [DueDate]            DATETIME        NULL,
    [Duration]           INT             NULL,
    [IsMilestone]        BIT             NULL,
    [ItemOrder]          INT             NULL,
    [Level]              INT             NULL,
    [ParentTask]         INT             NULL,
    [PercentComplete]    INT             NULL,
    [Predecessors]       NVARCHAR (250)  NULL,
    [Priority]           NVARCHAR (MAX)  NULL,
    [StageStep]          INT             NULL,
    [StartDate]          DATETIME        NULL,
    [Status]             NVARCHAR (MAX)  NULL,
    [TaskActualHours]    INT             NULL,
    [TaskBehaviour]      NVARCHAR (MAX)  NULL,
    [TaskEstimatedHours] INT             NULL,
    [TaskGroup]          NVARCHAR (MAX)  NULL,
    [TaskTemplateLookup] BIGINT          NOT NULL,
    [Title]              NVARCHAR (250)  NULL,
    [TenantID]           NVARCHAR (128)  NULL,
    [Created]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]            BIT             DEFAULT ((0)) NULL,
    [Attachments]        NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([TaskTemplateLookup]) REFERENCES [dbo].[TaskTemplates] ([ID])
);






GO
