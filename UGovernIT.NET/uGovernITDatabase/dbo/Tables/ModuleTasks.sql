CREATE TABLE [dbo].[ModuleTasks] (
    [ID]                          BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActualHours]                 FLOAT (53)      NULL,
    [ApprovalStatus]              NVARCHAR (250)  NULL,
    [ApproverUser]                NVARCHAR (250)  NULL,
    [AssignedToUser]              NVARCHAR (MAX)  NULL,
    [AssignToPct]                 NVARCHAR (MAX)  NULL,
    [Body]                        NVARCHAR (MAX)  NULL,
    [ChildCount]                  INT             NULL,
    [Description]                 NVARCHAR (500)  NULL,
    [Comment]                     NVARCHAR (MAX)  NULL,
    [CompletedBy]                 NVARCHAR (MAX)  NULL,
    [CompletionDate]              DATETIME2 (7)   NULL,
    [Contribution]                FLOAT (53)      NULL,
    [DueDate]                     DATETIME2 (7)   NULL,
    [Duration]                    FLOAT (53)      NULL,
    [EnableApproval]              BIT             NULL,
    [EstimatedRemainingHours]     FLOAT (53)      NULL,
    [IsCritical]                  BIT             NULL,
    [IsMilestone]                 BIT             NULL,
    [ItemOrder]                   INT             NULL,
    [Level]                       INT             NULL,
    [LinkedDocuments]             NVARCHAR (250)  NULL,
    [ModuleNameLookup]            NVARCHAR (250)  NULL,
    [NewUserName]                 NVARCHAR (250)  NULL,
    [ParentTaskID]                BIGINT          NULL,
    [PercentComplete]             FLOAT (53)      NULL,
    [Predecessors]                NVARCHAR (250)  NULL,
    [Priority]                    NVARCHAR (MAX)  NULL,
    [ProposedDate]                DATETIME        NULL,
    [ProposedStatus]              NVARCHAR (MAX)  NULL,
    [ShowOnProjectCalendar]       BIT             NULL,
    [SprintLookup]                BIGINT          NULL,
    [StageStep]                   INT             NULL,
    [StartDate]                   DATETIME2 (7)   NULL,
    [Status]                      NVARCHAR (MAX)  NULL,
    [Behaviour]                   NVARCHAR (MAX)  NULL,
    [EstimatedHours]              FLOAT (53)      NULL,
    [GroupUser]                   NVARCHAR (MAX)  NULL,
    [RepeatInterval]              INT             NULL,
    [ReminderDays]                INT             NULL,
    [ReminderEnabled]             BIT             NULL,
    [RequestTypeCategory]         NVARCHAR (250)  NULL,
    [RequestTypeLookup]           BIGINT          NULL,
    [RequestTypeWorkflow]         NVARCHAR (250)  NULL,
    [RelatedModule]               NVARCHAR (250)  NULL,
    [RelatedTicketID]             NVARCHAR (250)  NULL,
    [SubTaskType]                 NVARCHAR (250)  NULL,
    [TicketId]                    NVARCHAR (250)  NULL,
    [Title]                       VARCHAR (250)   NULL,
    [UserSkillMultiLookup]        VARCHAR (250)   NULL,
    [Weight]                      INT             NULL,
    [TenantID]                    NVARCHAR (128)  NULL,
    [Created]                     DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                    DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]               NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]              NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                     BIT             DEFAULT ((0)) NULL,
    [Attachments]                 NVARCHAR (2000) DEFAULT ('') NULL,
    [TotalHoldDuration]           FLOAT (53)      NULL,
    [OnHoldStartDate]             DATETIME2 (7)   NULL,
    [OnHold]                      BIT             DEFAULT ((0)) NOT NULL,
    [OnHoldTillDate]              DATETIME2 (7)   NULL,
    [OnHoldReasonChoice]          NVARCHAR (500)  NULL,
    [TaskActualStartDate]         DATETIME2 (7)   NULL,
    [SLADisabled]                 BIT             DEFAULT ((0)) NOT NULL,
    [IssueImpact]                 NVARCHAR (MAX)  NULL,
    [ContingencyPlan]             NVARCHAR (MAX)  NULL,
    [MitigationPlan]              NVARCHAR (MAX)  NULL,
    [RiskProbability]             INT             NULL,
    [Resolution]                  NVARCHAR (MAX)  NULL,
    [ResolutionDate]              DATETIME        NULL,
    [NotificationDisabled]        BIT             DEFAULT ((0)) NOT NULL,
    [ServiceApplicationAccessXml] VARCHAR (MAX)   NULL,
	[QuestionID] nvarchar(250) null,
	[QuestionProperties] nvarchar(500) null,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);







GO
CREATE NONCLUSTERED INDEX [IX_ModuleTasks_Deleted] ON [dbo].[ModuleTasks] ([Deleted])

GO
CREATE NONCLUSTERED INDEX [IX_ModuleTasks_ModuleName]
    ON [dbo].[ModuleTasks]([ModuleNameLookup] ASC);



GO
CREATE NONCLUSTERED INDEX [IX_ModuleTasks_TicketId] ON [dbo].[ModuleTasks] ([TicketId])

