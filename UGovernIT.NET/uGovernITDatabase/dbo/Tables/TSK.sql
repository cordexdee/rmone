CREATE TABLE [dbo].[TSK] (
    [ID]                            BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActualCompletionDate]          DATETIME        NULL,
    [ActualHour]                    INT             NULL,
    [ActualStartDate]               DATETIME        NULL,
    [ArchitectureScore]             INT             NULL,
    [ArchitectureScoreNotes]        NVARCHAR (MAX)  NULL,
    [AutoAdjustAllocations]         BIT             NULL,
    [BeneficiariesLookup]           NVARCHAR (250)  NULL,
    [BenefitsExperienced]           NVARCHAR (MAX)  NULL,
    [BreakevenMonth]                NVARCHAR (250)  NULL,
    [Closed]                        BIT             NULL,
    [CloseDate]                     DATETIME        NULL,
    [Comment]                       NVARCHAR (MAX)  NULL,
    [CompanyMultiLookup]            BIGINT          NULL,
    [ConstraintNotes]               NVARCHAR (MAX)  NULL,
    [CreationDate]                  DATETIME        NULL,
    [CurrentStageStartDate]         DATETIME        NULL,
    [DaysToComplete]                INT             NULL,
    [Description]                   NVARCHAR (MAX)  NULL,
    [DesiredCompletionDate]         DATETIME        NULL,
    [DivisionMultiLookup]           BIGINT          NULL,
    [DocumentLibraryName]           NVARCHAR (250)  NULL,
    [Duration]                      INT             NULL,
    [EliminatesStaffHeadCount]      NVARCHAR (250)  NULL,
    [EstimatedHours]                INT             NULL,
    [EstimatedRemainingHours]       INT             NULL,
    [FunctionalAreaLookup]          BIGINT          NULL,
    [History]                       NVARCHAR (MAX)  NULL,
    [ImpactAsImprovesRevenues]      NVARCHAR (250)  NULL,
    [ImpactAsOperationalEfficiency] NVARCHAR (250)  NULL,
    [ImpactAsSaveMoney]             NVARCHAR (250)  NULL,
    [ImpactLookup]                  BIGINT          NULL,
    [InitiatorUser]                 NVARCHAR (MAX)  NULL,
    [ITManager]                     NVARCHAR (MAX)  NULL,
    [LessonsLearned]                NVARCHAR (MAX)  NULL,
    [ModuleStepLookup]              NVARCHAR (250)  NULL,
    [NextActivity]                  NVARCHAR (250)  NULL,
    [NextMilestone]                 NVARCHAR (250)  NULL,
    [NoOfConsultants]               INT             NULL,
    [NoOfConsultantsNotes]          NVARCHAR (MAX)  NULL,
    [NoOfFTEs]                      INT             NULL,
    [NoOfFTEsNotes]                 NVARCHAR (MAX)  NULL,
    [NPRIdLookup]                   BIGINT          NULL,
    [OnHold]                        INT             NULL,
    [OnHoldReasonChoice]            NVARCHAR (MAX)  NULL,
    [OnHoldTillDate]                DATETIME        NULL,
    [OwnerUser]                     NVARCHAR (250)  NULL,
    [PctComplete]                   INT             NULL,
    [PriorityLookup]                BIGINT          NULL,
    [ProjectClassLookup]            BIGINT          NULL,
    [ProjectCost]                   INT             NULL,
    [ProjectCostNote]               NVARCHAR (MAX)  NULL,
    [ProjectInitiativeLookup]       BIGINT          NULL,
    [ProjectManagerUser]            NVARCHAR (250)  NULL,
    [ProjectPhasePctComplete]       INT             NULL,
    [ProjectScheduleNote]           NVARCHAR (MAX)  NULL,
    [ProjectScore]                  NVARCHAR (250)  NULL,
    [ProjectScoreNotes]             NVARCHAR (MAX)  NULL,
    [ProjectSummaryNote]            NVARCHAR (MAX)  NULL,
    [RequestorUser]                 NVARCHAR (MAX)  NULL,
    [RequestTypeCategory]           NVARCHAR (250)  NULL,
    [RequestTypeLookup]             BIGINT          NULL,
    [RequestTypeSubCategory]        NVARCHAR (250)  NULL,
    [RiskScore]                     INT             NULL,
    [RiskScoreNotes]                NVARCHAR (MAX)  NULL,
    [ROI]                           NVARCHAR (250)  NULL,
    [ServiceLookUp]                 BIGINT          NULL,
    [SeverityLookup]                BIGINT          NULL,
    [ShowProjectStatus]             BIT             NULL,
    [SponsorsUser]                  NVARCHAR (250)  NULL,
    [StageActionUsersUser]          NVARCHAR (250)  NULL,
    [StageActionUserTypes]          NVARCHAR (250)  NULL,
    [StageStep]                     INT             NULL,
    [StakeHoldersUser]              NVARCHAR (250)  NULL,
    [Status]                        NVARCHAR (250)  NULL,
    [StatusChanged]                 INT             NULL,
    [TargetCompletionDate]          DATETIME        NULL,
    [TargetStartDate]               DATETIME        NULL,
    [TicketId]                      NVARCHAR (250)  NULL,
    [TotalConsultantHeadcount]      INT             NULL,
    [TotalConsultantHeadcountNotes] NVARCHAR (MAX)  NULL,
    [TotalCost]                     INT             NULL,
    [TotalCostsNotes]               NVARCHAR (MAX)  NULL,
    [TotalStaffHeadcount]           INT             NULL,
    [TotalStaffHeadcountNotes]      NVARCHAR (MAX)  NULL,
    [UserQuestionSummary]           NVARCHAR (MAX)  NULL,
    [WorkflowSkipStages]            NVARCHAR (250)  NULL,
    [Title]                         VARCHAR (250)   NULL,
    [TenantID]                      NVARCHAR (128)  NULL,
    [DataEditors]                   NVARCHAR (250)  DEFAULT ('') NULL,
    [Created]                       DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                      DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                 NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]                NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                       BIT             DEFAULT ((0)) NULL,
    [Attachments]                   NVARCHAR (2000) DEFAULT ('') NULL,
    [NextMilestoneDate]             DATETIME2 (7)   NULL,
    [ResolutionDate]                DATETIME        NULL,
	[Age]                             BIGINT          NULL,
    CONSTRAINT [PK_TSK] PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([FunctionalAreaLookup]) REFERENCES [dbo].[FunctionalAreas] ([ID]),
    FOREIGN KEY ([ImpactLookup]) REFERENCES [dbo].[Config_Module_Impact] ([ID]),
    FOREIGN KEY ([NPRIdLookup]) REFERENCES [dbo].[NPR] ([ID]),
    FOREIGN KEY ([PriorityLookup]) REFERENCES [dbo].[Config_Module_Priority] ([ID]),
    FOREIGN KEY ([ProjectClassLookup]) REFERENCES [dbo].[Config_ProjectClass] ([ID]),
    FOREIGN KEY ([ProjectInitiativeLookup]) REFERENCES [dbo].[Config_ProjectInitiative] ([ID]),
    FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID]),
    FOREIGN KEY ([ServiceLookUp]) REFERENCES [dbo].[Config_Services] ([ID]),
    FOREIGN KEY ([SeverityLookup]) REFERENCES [dbo].[Config_Module_Severity] ([ID])
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
CREATE NONCLUSTERED INDEX [IX_TSK_PriorityLookup] ON [dbo].[TSK] ([AutoAdjustAllocations], [ModuleStepLookup], [NPRIdLookup], [PriorityLookup], [RequestTypeLookup], [Status], [TicketId])