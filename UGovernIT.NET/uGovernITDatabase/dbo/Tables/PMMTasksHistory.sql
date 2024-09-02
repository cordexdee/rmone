CREATE TABLE [dbo].[PMMTasksHistory] (
    [ID]                      BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActualHour]              INT             NULL,
    [AssignedToUser]          NVARCHAR (250)  NULL,
    [AssignToPct]             NVARCHAR (MAX)  NULL,
    [BaselineDate]            DATETIME        NULL,
    [BaselineNum]             INT             NULL,
    [Body]                    NVARCHAR (MAX)  NULL,
    [CompletedBy]             NVARCHAR (MAX)  NULL,
    [CompletionDate]          DATETIME        NULL,
    [DueDate]                 DATETIME        NULL,
    [EstimatedRemainingHours] INT             NULL,
    [IsCritical]              BIT             NULL,
    [IsMilestone]             BIT             NULL,
    [ItemOrder]               INT             NULL,
    [ParentTask]              INT             NULL,
    [PercentComplete]         INT             NULL,
    [PMMIdLookup]             BIGINT          NOT NULL,
    [Predecessors]            NVARCHAR (250)  NULL,
    [Priority]                NVARCHAR (MAX)  NULL,
    [ProposedStatus]          NVARCHAR (MAX)  NULL,
    [SprintLookup]            BIGINT          NOT NULL,
    [StageStep]               INT             NULL,
    [StartDate]               DATETIME        NULL,
    [Status]                  NVARCHAR (MAX)  NULL,
    [TaskActualHours]         INT             NULL,
    [TaskBehaviour]           NVARCHAR (MAX)  NULL,
    [TaskEstimatedHours]      INT             NULL,
    [TaskGroup]               NVARCHAR (MAX)  NULL,
    [TaskReminderDays]        INT             NULL,
    [TaskReminderEnabled]     BIT             NULL,
    [UserSkillMultiLookup]    BIGINT          NOT NULL,
    [Title]                   VARCHAR (250)   NULL,
    [TenantID]                NVARCHAR (128)  NULL,
    [Created]                 DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]           NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                 BIT             DEFAULT ((0)) NULL,
    [Attachments]             NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([PMMIdLookup]) REFERENCES [dbo].[PMM] ([ID]),
    FOREIGN KEY ([SprintLookup]) REFERENCES [dbo].[Sprint] ([ID]),
    FOREIGN KEY ([UserSkillMultiLookup]) REFERENCES [dbo].[UserSkills] ([ID])
);






GO

GO

GO
