CREATE TABLE [dbo].[SprintTasks] (
    [ID]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [AssignedToUser]        NVARCHAR (250)  NULL,
    [Body]                  NVARCHAR (MAX)  NULL,
    [ChildCount]            INT             NULL,
    [Comment]               NVARCHAR (MAX)  NULL,
    [Contribution]          INT             NULL,
    [Duration]              INT             NULL,
    [IsMilestone]           BIT             NULL,
    [ItemOrder]             INT             NULL,
    [Level]                 INT             NULL,
    [ParentTask]            INT             NULL,
    [PercentComplete]       INT             NULL,
    [PMMIdLookup]           BIGINT          NOT NULL,
    [Priority]              NVARCHAR (MAX)  NULL,
    [ProposedDate]          DATETIME        NULL,
    [ProposedStatus]        NVARCHAR (MAX)  NULL,
    [ReleaseLookup]         BIGINT          NULL,
    [ShowOnProjectCalendar] BIT             NULL,
    [SprintLookup]          BIGINT          NULL,
    [SprintOrder]           INT             NULL,
    [StageStep]             INT             NULL,
    [StartDate]             DATETIME        NULL,
    [TaskActualHours]       INT             NULL,
    [TaskBehaviour]         NVARCHAR (MAX)  NULL,
    [TaskDueDate]           DATETIME        NULL,
    [TaskEstimatedHours]    INT             NULL,
    [TaskGroup]             NVARCHAR (MAX)  NULL,
    [TaskStatus]            NVARCHAR (MAX)  NULL,
    [Title]                 VARCHAR (250)   NULL,
    [TenantID]              NVARCHAR (128)  NULL,
    [Created]               DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]              DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]         NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]        NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]               BIT             DEFAULT ((0)) NULL,
    [Attachments]           NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([PMMIdLookup]) REFERENCES [dbo].[PMM] ([ID]),
    FOREIGN KEY ([ReleaseLookup]) REFERENCES [dbo].[ProjectReleases] ([ID]),
    FOREIGN KEY ([SprintLookup]) REFERENCES [dbo].[Sprint] ([ID])
);






GO

GO

GO
