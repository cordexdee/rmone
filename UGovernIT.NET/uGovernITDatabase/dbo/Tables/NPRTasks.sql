CREATE TABLE [dbo].[NPRTasks] (
    [ID]                   BIGINT          IDENTITY (1, 1) NOT NULL,
    [AssignedToUser]       NVARCHAR (MAX)  NULL,
    [AssignToPct]          NVARCHAR (MAX)  NULL,
    [Body]                 NVARCHAR (MAX)  NULL,
    [ChildCount]           INT             NULL,
    [CompletionDate]       DATETIME        NULL,
    [Contribution]         INT             NULL,
    [DueDate]              DATETIME        NULL,
    [Duration]             INT             NULL,
    [IsCritical]           BIT             NULL,
    [ItemOrder]            INT             NULL,
    [Level]                INT             NULL,
    [NPRIdLookup]          BIGINT          NOT NULL,
    [ParentTask]           INT             NULL,
    [PercentComplete]      INT             NULL,
    [Predecessors]         NVARCHAR (250)  NULL,
    [Priority]             NVARCHAR (MAX)  NULL,
    [StartDate]            DATETIME        NULL,
    [Status]               NVARCHAR (MAX)  NULL,
    [TaskBehaviour]        NVARCHAR (MAX)  NULL,
    [TaskEstimatedHours]   INT             NULL,
    [TaskGroup]            NVARCHAR (MAX)  NULL,
    [UserSkillMultiLookup] BIGINT          NOT NULL,
    [Title]                VARCHAR (250)   NULL,
    [TenantID]             NVARCHAR (128)  NULL,
    [Created]              DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]        NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]              BIT             DEFAULT ((0)) NULL,
    [Attachments]          NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([NPRIdLookup]) REFERENCES [dbo].[NPR] ([ID])
);






GO
