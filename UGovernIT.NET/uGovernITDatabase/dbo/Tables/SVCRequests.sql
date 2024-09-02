CREATE TABLE [dbo].[SVCRequests] (
    [ID]                      BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActualCompletionDate]    DATETIME        NULL,
    [ActualHours]             INT             NULL,
    [ActualStartDate]         DATETIME        NULL,
    [ApproverUser]            NVARCHAR (250)  NULL,
    [Closed]                  BIT             NULL,
    [CloseDate]               DATETIME        NULL,
    [Comment]                 NVARCHAR (MAX)  NULL,
    [CreationDate]            DATETIME        NULL,
    [CurrentStageStartDate]   DATETIME        NULL,
    [Description]             NVARCHAR (MAX)  NULL,
    [DesiredCompletionDate]   DATETIME        NULL,
    [EstimatedHours]          INT             NULL,
    [History]                 NVARCHAR (MAX)  NULL,
    [InitiatorUser]           NVARCHAR (MAX)  NULL,
    [IsAllTaskComplete]       BIT             NULL,
    [IsPrivate]               BIT             NULL,
    [ModuleStepLookup]        NVARCHAR (250)  NULL,
    [OnHold]                  INT             NULL,
    [OnHoldReasonChoice]      NVARCHAR (MAX)  NULL,
    [OnHoldTillDate]          DATETIME        NULL,
    [OwnerUser]               NVARCHAR (250)  NULL,
    [OwnerApprovalRequired]   BIT             NULL,
    [PctComplete]             INT             NULL,
    [PriorityLookup]          BIGINT          NULL,
    [ProjectClassLookup]      BIGINT          NULL,
    [ProjectInitiativeLookup] BIGINT          NULL,
    [RequestorUser]           NVARCHAR (250)  NULL,
    [RequestTypeCategory]     NVARCHAR (250)  NULL,
    [RequestTypeLookup]       BIGINT          NULL,
    [RequestTypeSubCategory]  NVARCHAR (250)  NULL,
    [ServiceLookUp]           BIGINT          NULL,
    [ShowProjectStatus]       BIT             NULL,
    [StageActionUsersUser]    NVARCHAR (250)  NULL,
    [StageActionUserTypes]    NVARCHAR (250)  NULL,
    [StageStep]               INT             NULL,
    [Status]                  NVARCHAR (250)  NULL,
    [StatusChanged]           INT             NULL,
    [TargetCompletionDate]    DATETIME        NULL,
    [TargetStartDate]         DATETIME        NULL,
    [TicketId]                NVARCHAR (250)  NULL,
    [UserQuestionSummary]     NVARCHAR (MAX)  NULL,
    [WorkflowSkipStages]      NVARCHAR (250)  NULL,
    [Title]                   VARCHAR (250)   NULL,
    [TenantID]                NVARCHAR (128)  NULL,
    [DataEditors]             NVARCHAR (250)  DEFAULT ('') NULL,
    [Created]                 DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]           NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                 BIT             DEFAULT ((0)) NULL,
    [Attachments]             NVARCHAR (2000) DEFAULT ('') NULL,
    [SLADisabled]             BIT             DEFAULT ((0)) NOT NULL,
    [OnHoldStartDate]         DATETIME2 (7)   NULL,
    [TotalHoldDuration]       INT             NULL,
    [isAttachment]            BIT             DEFAULT ((0)) NULL,
    [PRPUser]                 NVARCHAR (MAX)  NULL,
    [PONumber]                NVARCHAR (MAX)  NULL,
    [Quantity]                INT             NULL,
    [Quantity2]               INT             NULL,
    [Enable]                  NCHAR (10)      NULL,
    [EnableTaskReminder]      BIT             DEFAULT ((0)) NOT NULL,
    [ResolutionDate]          DATETIME        NULL,
    [ReopenCount]             INT             NULL,
    [ResolvedByUser] NVARCHAR(128) NULL, 
    [ORPUser] NVARCHAR (MAX) NULL, 
	[Age]                             BIGINT          NULL,
    [ApprovalDate] DATETIME NULL, 
    [ApprovedByUser] VARCHAR(128) NULL,
    [LocationMultLookup] NVARCHAR(MAX) NULL, 
    [DepartmentLookup ] BIGINT NULL, 
    CONSTRAINT [PK_SVCRequests] PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([PriorityLookup]) REFERENCES [dbo].[Config_Module_Priority] ([ID]),
    FOREIGN KEY ([ProjectClassLookup]) REFERENCES [dbo].[Config_ProjectClass] ([ID]),
    FOREIGN KEY ([ProjectClassLookup]) REFERENCES [dbo].[Config_ProjectClass] ([ID]),
    FOREIGN KEY ([ProjectInitiativeLookup]) REFERENCES [dbo].[Config_ProjectInitiative] ([ID]),
    FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID]),
    FOREIGN KEY ([ServiceLookUp]) REFERENCES [dbo].[Config_Services] ([ID])
);





GO
CREATE NONCLUSTERED INDEX [IX_SVCRequests_ModuleStepLookup] ON [dbo].[SVCRequests] ([ModuleStepLookup])

GO
CREATE NONCLUSTERED INDEX [IX_SVCRequests_PriorityLookup] ON [dbo].[SVCRequests] ([PriorityLookup])

GO
CREATE NONCLUSTERED INDEX [IX_SVCRequests_RequestTypeLookup] ON [dbo].[SVCRequests] ([RequestTypeLookup])

GO
CREATE NONCLUSTERED INDEX [IX_SVCRequests_Status] ON [dbo].[SVCRequests] ([Status])

GO
CREATE NONCLUSTERED INDEX [IX_SVCRequests_TicketId] ON [dbo].[SVCRequests] ([TicketId])



GO

GO

GO

GO

GO

GO
