CREATE TABLE [dbo].[RCA_Archive] (
    [ID]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActualCompletionDate]  DATETIME        NULL,
    [ActualStartDate]       DATETIME        NULL,
    [AnalysisDetails]       NVARCHAR (MAX)  NULL,
    [Closed]                BIT             NULL,
    [CloseDate]             DATETIME        NULL,
    [Comment]               NVARCHAR (MAX)  NULL,
    [CompanyTitleLookup]    BIGINT          NULL,
    [CreationDate]          DATETIME        NULL,
    [CurrentStageStartDate] DATETIME        NULL,
    [DepartmentLookup]      BIGINT          NULL,
    [Description]           NVARCHAR (MAX)  NULL,
    [DesiredCompletionDate] DATETIME        NULL,
    [DeskLocation]          NVARCHAR (250)  NULL,
    [DivisionLookup]        BIGINT          NULL,
    [DocumentLibraryName]   NVARCHAR (250)  NULL,
    [FunctionalAreaLookup]  BIGINT          NULL,
    [History]               NVARCHAR (MAX)  NULL,
    [ImpactLookup]          BIGINT          NULL,
    [InitiatorUser]         NVARCHAR (MAX)  NULL,
    [IsPrivate]             BIT             NULL,
    [LocationLookup]        BIGINT          NULL,
    [ManagerApprovalNeeded] BIT             NULL,
    [ModuleStepLookup]      NVARCHAR (250)  NULL,
    [NextSLATime]           DATETIME        NULL,
    [NextSLAType]           NVARCHAR (250)  NULL,
    [OnHold]                INT             NULL,
    [OnHoldReasonChoice]    NVARCHAR (MAX)  NULL,
    [OnHoldStartDate]       DATETIME        NULL,
    [OnHoldTillDate]        DATETIME        NULL,
    [ORPUser]               NVARCHAR (MAX)  NULL,
    [OwnerUser]             NVARCHAR (250)  NULL,
    [PctComplete]           INT             NULL,
    [PriorityLookup]        BIGINT          NULL,
    [PRPUser]               NVARCHAR (MAX)  NULL,
    [PRPGroupUser]          NVARCHAR (MAX)  NULL,
    [RCATypeChoice]         NVARCHAR (MAX)  NULL,
    [ReopenCount]           INT             NULL,
    [RequestTypeCategory]   NVARCHAR (250)  NULL,
    [RequestTypeLookup]     BIGINT          NULL,
    [RequestTypeWorkflow]   NVARCHAR (250)  NULL,
    [ResolutionTypeChoice]  NVARCHAR (MAX)  NULL,
    [ReSubmissionDate]      DATETIME        NULL,
    [ServiceLookUp]         BIGINT          NULL,
    [StageActionUsersUser]  NVARCHAR (250)  NULL,
    [StageActionUserTypes]  NVARCHAR (250)  NULL,
    [StageStep]             INT             NULL,
    [Status]                NVARCHAR (250)  NULL,
    [StatusChanged]         INT             NULL,
    [TargetCompletionDate]  DATETIME        NULL,
    [TargetStartDate]       DATETIME        NULL,
    [TicketId]              NVARCHAR (250)  NULL,
    [Title]                 NVARCHAR (250)  NULL,
    [TotalHoldDuration]     INT             NULL,
    [UserQuestionSummary]   NVARCHAR (MAX)  NULL,
    [WorkflowSkipStages]    NVARCHAR (250)  NULL,
    [TenantID]              NVARCHAR (128)  NULL,
    [DataEditors]           NVARCHAR (250)  DEFAULT ('') NULL,
    [Created]               DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]              DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]         NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]        NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]               BIT             DEFAULT ((0)) NULL,
    [Attachments]           NVARCHAR (2000) DEFAULT ('') NULL,
	[ResolutionDate]           DATETIME        NULL,
    [UGITIssueType]            NVARCHAR (128)  NULL,
    [Age]                      BIGINT          NULL,
    [Justification]            NVARCHAR (MAX)  NULL,
    [CorrectiveActions]        NVARCHAR (MAX)  NULL,
    [MonitoringToolNotifiable] BIT             DEFAULT ((0)) NOT NULL,
    [ContributingCauses]       NVARCHAR (MAX)  NULL,
    [RootCause]                NVARCHAR (MAX)  NULL,
    [WhyFinalVerify]           BIT             DEFAULT ((0)) NOT NULL,
    [WhyFinal]                 NVARCHAR (MAX)  NULL,
    [TemporaryCountermeasure]  NVARCHAR (MAX)  NULL,
    [Why1]                     NVARCHAR (MAX)  NULL,
    [Why2]                     NVARCHAR (MAX)  NULL,
    [Why3]                     NVARCHAR (MAX)  NULL,
    [Why4]                     NVARCHAR (MAX)  NULL,
    [Why5]                     NVARCHAR (MAX)  NULL,
    [OccurrenceDetails]        NVARCHAR (MAX)  NULL,
    [CustomUGChoice01]         NVARCHAR (MAX)  NULL,
    [CustomUGChoice02]         NVARCHAR (MAX)  NULL,
    [CustomUGChoice03]         NVARCHAR (MAX)  NULL,
    [CustomUGChoice04]         NVARCHAR (MAX)  NULL,
    [CustomUGDate01]           DATETIME        NULL,
    [CustomUGDate02]           DATETIME        NULL,
    [CustomUGDate03]           DATETIME        NULL,
    [CustomUGDate04]           DATETIME        NULL,
    [CustomUGText01]           NVARCHAR (250)  NULL,
    [CustomUGText02]           NVARCHAR (250)  NULL,
    [CustomUGText03]           NVARCHAR (250)  NULL,
    [CustomUGText04]           NVARCHAR (250)  NULL,
    [CustomUGText05]           NVARCHAR (250)  NULL,
    [CustomUGText06]           NVARCHAR (250)  NULL,
    [CustomUGText07]           NVARCHAR (250)  NULL,
    [CustomUGText08]           NVARCHAR (250)  NULL,
    [CustomUGUser01]           NVARCHAR (MAX)  NULL,
    [CustomUGUser02]           NVARCHAR (MAX)  NULL,
    [CustomUGUser03]           NVARCHAR (MAX)  NULL,
    [CustomUGUser04]           NVARCHAR (MAX)  NULL,
    [CustomUGUserMulti01]      NVARCHAR (250)  NULL,
    [CustomUGUserMulti02]      NVARCHAR (250)  NULL,
    [CustomUGUserMulti03]      NVARCHAR (250)  NULL,
    [CustomUGUserMulti04]      NVARCHAR (250)  NULL,
    [AssignedBy]               NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ResolutionComments]       NVARCHAR (MAX)  NULL,
    [ResolvedByUser]           NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ClosedByUser]             NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [DataEditor]               NVARCHAR (250)  NULL,
    [Rejected]                 BIT             DEFAULT ((0)) NOT NULL,
    [ApprovalDate] DATETIME NULL, 
    [ApprovedByUser] VARCHAR(128) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([CompanyTitleLookup]) REFERENCES [dbo].[Company] ([ID]),
    FOREIGN KEY ([DepartmentLookup]) REFERENCES [dbo].[Department] ([ID]),
    FOREIGN KEY ([DivisionLookup]) REFERENCES [dbo].[CompanyDivisions] ([ID]),
    FOREIGN KEY ([FunctionalAreaLookup]) REFERENCES [dbo].[FunctionalAreas] ([ID]),
    FOREIGN KEY ([ImpactLookup]) REFERENCES [dbo].[Config_Module_Impact] ([ID]),
    FOREIGN KEY ([LocationLookup]) REFERENCES [dbo].[Location] ([ID]),
    FOREIGN KEY ([PriorityLookup]) REFERENCES [dbo].[Config_Module_Priority] ([ID]),
    FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID]),
    FOREIGN KEY ([ServiceLookUp]) REFERENCES [dbo].[Config_Services] ([ID])
);










GO

GO

GO

GO

GO

GO

GO

GO

GO
CREATE NONCLUSTERED INDEX [IX_RCA_Archive_PriorityLookup] ON [dbo].[RCA_Archive] ([ModuleStepLookup], [PriorityLookup], [RequestTypeLookup], [Status], [TicketId])