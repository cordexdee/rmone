
CREATE TABLE [dbo].[PmmProjectHistory] (
    [ID]                            BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActualCompletionDate]          DATETIME        NULL,
    [ActualHour]                    INT             NULL,
    [ActualStartDate]               DATETIME        NULL,
    [ApprovedRFE]                   NVARCHAR (250)  NULL,
    [ApprovedRFEAmount]             NVARCHAR (250)  NULL,
    [ApprovedRFEType]               NVARCHAR (MAX)  NULL,
    [APPTitleLookup]                BIGINT          NULL,
    [ArchitectureScore]             INT             NULL,
    [ArchitectureScoreNotes]        NVARCHAR (MAX)  NULL,
    [AutoAdjustAllocations]         BIT             NULL,
    [BeneficiariesLookup]           NVARCHAR (250)  NULL,
    [BenefitsExperienced]           NVARCHAR (MAX)  NULL,
    [BreakevenMonth]                NVARCHAR (250)  NULL,
    [BusinessManagerUser]           NVARCHAR (MAX)  NULL,
    [ClassificationChoice]          NVARCHAR (MAX)  NULL,
    [ClassificationImpact]          NVARCHAR (MAX)  NULL,
    [ClassificationTypeChoice]      NVARCHAR (MAX)  NULL,
    [Closed]                        BIT             NULL,
    [CloseDate]                     DATETIME        NULL,
    [Comment]                       NVARCHAR (MAX)  NULL,
    [CompanyMultiLookup]            BIGINT          NULL,
    [ConstraintNotes]               NVARCHAR (MAX)  NULL,
    [CreationDate]                  DATETIME        NULL,
    [CurrentStageStartDate]         DATETIME        NULL,
    [CustomUGChoice01]              NVARCHAR (MAX)  NULL,
    [CustomUGChoice02]              NVARCHAR (MAX)  NULL,
    [CustomUGChoice03]              NVARCHAR (MAX)  NULL,
    [CustomUGChoice04]              NVARCHAR (MAX)  NULL,
    [CustomUGDate01]                DATETIME        NULL,
    [CustomUGDate02]                DATETIME        NULL,
    [CustomUGDate03]                DATETIME        NULL,
    [CustomUGDate04]                DATETIME        NULL,
    [CustomUGText01]                NVARCHAR (250)  NULL,
    [CustomUGText02]                NVARCHAR (250)  NULL,
    [CustomUGText03]                NVARCHAR (250)  NULL,
    [CustomUGText04]                NVARCHAR (250)  NULL,
    [CustomUGText05]                NVARCHAR (250)  NULL,
    [CustomUGText06]                NVARCHAR (250)  NULL,
    [CustomUGText07]                NVARCHAR (250)  NULL,
    [CustomUGText08]                NVARCHAR (250)  NULL,
    [CustomUGUser01]                NVARCHAR (MAX)  NULL,
    [CustomUGUser02]                NVARCHAR (MAX)  NULL,
    [CustomUGUser03]                NVARCHAR (MAX)  NULL,
    [CustomUGUser04]                NVARCHAR (MAX)  NULL,
    [CustomUGUserMulti01]           NVARCHAR (250)  NULL,
    [CustomUGUserMulti02]           NVARCHAR (250)  NULL,
    [CustomUGUserMulti03]           NVARCHAR (250)  NULL,
    [CustomUGUserMulti04]           NVARCHAR (250)  NULL,
    [DaysToComplete]                INT             NULL,
    [Description]                   NVARCHAR (MAX)  NULL,
    [DesiredCompletionDate]         DATETIME        NULL,
    [DivisionMultiLookup]           BIGINT          NULL,
    [Duration]                      INT             NULL,
    [EliminatesStaffHeadCount]      NVARCHAR (250)  NULL,
    [EstimatedHours]                INT             NULL,
    [EstimatedRemainingHours]       INT             NULL,
    [EstProjectSpend]               NVARCHAR (250)  NULL,
    [EstProjectSpendComment]        NVARCHAR (MAX)  NULL,
    [FunctionalAreaLookup]          BIGINT          NULL,
    [History]                       NVARCHAR (MAX)  NULL,
    [ImpactAsImprovesRevenues]      NVARCHAR (250)  NULL,
    [ImpactAsOperationalEfficiency] NVARCHAR (250)  NULL,
    [ImpactAsSaveMoney]             NVARCHAR (250)  NULL,
    [ImpactLookup]                  BIGINT          NULL,
    [InitiatorUser]                 NVARCHAR (MAX)  NULL,
    [IsPrivate]                     BIT             NULL,
    [ITManager]                     NVARCHAR (MAX)  NULL,
    [LessonsLearned]                NVARCHAR (MAX)  NULL,
    [LocationMultLookup]            BIGINT          NULL,
    [ModuleNameLookup]              NVARCHAR (250)  NULL,
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
    [ProblemBeingSolved]            NVARCHAR (MAX)  NULL,
    [ProjectAssumptions]            NVARCHAR (MAX)  NULL,
    [ProjectBenefits]               NVARCHAR (MAX)  NULL,
    [ProjectClassLookup]            BIGINT          NULL,
    [ProjectCost]                   INT             NULL,
    [ProjectCostNote]               NVARCHAR (MAX)  NULL,
    [ProjectInitiativeLookup]       BIGINT          NULL,
    [ProjectIteration]              INT             NULL,
    [ProjectLifeCycleLookup]        BIGINT          NULL,
    [ProjectManagerUser]            NVARCHAR (250)  NULL,
    [ProjectPhasePctComplete]       INT             NULL,
    [ProjectRank]                   NVARCHAR (250)  NULL,
    [ProjectRank2]                  NVARCHAR (250)  NULL,
    [ProjectRank3]                  NVARCHAR (250)  NULL,
    [ProjectRiskNotes]              NVARCHAR (MAX)  NULL,
    [ProjectScheduleNote]           NVARCHAR (MAX)  NULL,
    [ProjectScope]                  NVARCHAR (MAX)  NULL,
    [ProjectScore]                  NVARCHAR (250)  NULL,
    [ProjectScoreNotes]             NVARCHAR (MAX)  NULL,
    [ProjectStatus]                 NVARCHAR (MAX)  NULL,
    [ProjectSummaryNote]            NVARCHAR (MAX)  NULL,
    [PRPGroupUser]                  NVARCHAR (MAX)  NULL,
    [RequestorUser]                 NVARCHAR (250)  NULL,
    [RequestTypeCategory]           NVARCHAR (250)  NULL,
    [RequestTypeLookup]             BIGINT          NULL,
    [RequestTypeSubCategory]        NVARCHAR (250)  NULL,
    [RiskScore]                     INT             NULL,
    [RiskScoreNotes]                NVARCHAR (MAX)  NULL,
    [ROI]                           NVARCHAR (250)  NULL,
    [ScrumLifeCycle]                BIT             NULL,
    [ServiceLookUp]                 BIGINT          NULL,
    [SeverityLookup]                BIGINT          NULL,
    [ShowProjectStatus]             BIT             NULL,
    [SponsorsUser]                  NVARCHAR (250)  NULL,
    [SprintDuration]                INT             NULL,
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
    [WorkflowSkipStages]            NVARCHAR (250)  NULL,
    [Title]                         VARCHAR (250)   NULL,
    [ApplicationMultiLookup]        BIGINT          NULL,
    [ClosedByUser]                  NVARCHAR (250)  NULL,
    [DataEditor]                    NVARCHAR (250)  NULL,
    [ResolvedByUser]                NVARCHAR (250)  NULL,
    [SharedServices]                NVARCHAR (MAX)  NULL,
    [TotalActualHours]              INT             NULL,
    [TenantID]                      NVARCHAR (128)  NULL,
    [DataEditors]                   NVARCHAR (250)  CONSTRAINT [DF__PmmProjectHistory__DataEditors__4B180DA3] DEFAULT ('') NULL,
    [Created]                       DATETIME        CONSTRAINT [DF__PmmProjectHistory__Created__4C0C31DC] DEFAULT (getdate()) NOT NULL,
    [Modified]                      DATETIME        CONSTRAINT [DF__PmmProjectHistory__Modified__4D005615] DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                 NVARCHAR (128)  CONSTRAINT [DF__PmmProjectHistory__CreatedBy__4DF47A4E] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]                NVARCHAR (128)  CONSTRAINT [DF__PmmProjectHistory__ModifiedBy__4EE89E87] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                       BIT             CONSTRAINT [DF__PmmProjectHistory__Deleted__4FDCC2C0] DEFAULT ((0)) NULL,
    [Attachments]                   NVARCHAR (2000) CONSTRAINT [DF__PmmProjectHistory__Attachments__50D0E6F9] DEFAULT ('') NULL,
    [ProjectComplexityChoice]       INT             NULL,
    [ProjectDuration]               INT             CONSTRAINT [DF__PmmProjectHistory__ProjectDura__306F045F] DEFAULT ((0)) NULL,
    [ApproxContractValue]           FLOAT (53)      NULL,
    [Baseline]                      INT             NULL,
    [BaselineDate]                  DATETIME        NULL,
    [pmmid]                         BIGINT          NULL,
    [NextMilestoneDate]             DATETIME        NULL,
    [ProjectCoordinators]           NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_PmmProjectHistory] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK__PmmProjectHistory__Application__75B852E5] FOREIGN KEY ([ApplicationMultiLookup]) REFERENCES [dbo].[Applications] ([ID]),
    CONSTRAINT [FK__PmmProjectHistory__APPTitleLoo__76AC771E] FOREIGN KEY ([APPTitleLookup]) REFERENCES [dbo].[Applications] ([ID]),
    CONSTRAINT [FK__PmmProjectHistory__FunctionalA__77A09B57] FOREIGN KEY ([FunctionalAreaLookup]) REFERENCES [dbo].[FunctionalAreas] ([ID]),
    CONSTRAINT [FK__PmmProjectHistory__ImpactLooku__7894BF90] FOREIGN KEY ([ImpactLookup]) REFERENCES [dbo].[Config_Module_Impact] ([ID]),
    CONSTRAINT [FK__PmmProjectHistory__NPRIdLookup__7988E3C9] FOREIGN KEY ([NPRIdLookup]) REFERENCES [dbo].[NPR] ([ID]),
    CONSTRAINT [FK__PmmProjectHistory__PriorityLoo__7A7D0802] FOREIGN KEY ([PriorityLookup]) REFERENCES [dbo].[Config_Module_Priority] ([ID]),
    CONSTRAINT [FK__PmmProjectHistory__ProjectClas__7B712C3B] FOREIGN KEY ([ProjectClassLookup]) REFERENCES [dbo].[Config_ProjectClass] ([ID]),
    CONSTRAINT [FK__PmmProjectHistory__ProjectInit__7C655074] FOREIGN KEY ([ProjectInitiativeLookup]) REFERENCES [dbo].[Config_ProjectInitiative] ([ID]),
    CONSTRAINT [FK__PmmProjectHistory__ProjectLife__7D5974AD] FOREIGN KEY ([ProjectLifeCycleLookup]) REFERENCES [dbo].[Config_ModuleLifeCycles] ([ID]),
    CONSTRAINT [FK__PmmProjectHistory__RequestType__7E4D98E6] FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID]),
    CONSTRAINT [FK__PmmProjectHistory__ServiceLook__7F41BD1F] FOREIGN KEY ([ServiceLookUp]) REFERENCES [dbo].[Config_Services] ([ID]),
    CONSTRAINT [FK__PmmProjectHistory__SeverityLoo__0035E158] FOREIGN KEY ([SeverityLookup]) REFERENCES [dbo].[Config_Module_Severity] ([ID])
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

GO

GO

GO

GO

GO

GO

GO

GO

GO

GO

GO

GO

GO

GO

GO

GO

GO

GO

GO

GO

GO

GO

GO

GO
