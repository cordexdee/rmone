CREATE TABLE [dbo].[DRQ_Archive] (
    [ID]                              BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActualCompletionDate]            DATETIME        NULL,
    [ActualHours]                     INT             NULL,
    [ActualStartDate]                 DATETIME        NULL,
    [ApplicationManagerUser]          NVARCHAR (MAX)  NULL,
    [APPTitleLookup]                  BIGINT          NULL,
    [AutoSend]                        BIT             NULL,
    [BeneficiariesLookup]             NVARCHAR (250)  NULL,
    [BusinessManagerUser]             NVARCHAR (MAX)  NULL,
    [ChangePurpose]                   NVARCHAR (MAX)  NULL,
    [Closed]                          BIT             NULL,
    [CloseDate]                       DATETIME        NULL,
    [Comment]                         NVARCHAR (MAX)  NULL,
    [CompanyTitleLookup]              BIGINT          NULL,
    [CreationDate]                    DATETIME        NULL,
    [CurrentStageStartDate]           DATETIME        NULL,
    [CustomUGChoice01]                NVARCHAR (MAX)  NULL,
    [CustomUGChoice02]                NVARCHAR (MAX)  NULL,
    [CustomUGChoice03]                NVARCHAR (MAX)  NULL,
    [CustomUGChoice04]                NVARCHAR (MAX)  NULL,
    [CustomUGDate01]                  DATETIME        NULL,
    [CustomUGDate02]                  DATETIME        NULL,
    [CustomUGDate03]                  DATETIME        NULL,
    [CustomUGDate04]                  DATETIME        NULL,
    [CustomUGText01]                  NVARCHAR (250)  NULL,
    [CustomUGText02]                  NVARCHAR (250)  NULL,
    [CustomUGText03]                  NVARCHAR (250)  NULL,
    [CustomUGText04]                  NVARCHAR (250)  NULL,
    [CustomUGText05]                  NVARCHAR (250)  NULL,
    [CustomUGText06]                  NVARCHAR (250)  NULL,
    [CustomUGText07]                  NVARCHAR (250)  NULL,
    [CustomUGText08]                  NVARCHAR (250)  NULL,
    [CustomUGUser01]                  NVARCHAR (MAX)  NULL,
    [CustomUGUser02]                  NVARCHAR (MAX)  NULL,
    [CustomUGUser03]                  NVARCHAR (MAX)  NULL,
    [CustomUGUser04]                  NVARCHAR (MAX)  NULL,
    [CustomUGUserMulti01]             NVARCHAR (250)  NULL,
    [CustomUGUserMulti02]             NVARCHAR (250)  NULL,
    [CustomUGUserMulti03]             NVARCHAR (250)  NULL,
    [CustomUGUserMulti04]             NVARCHAR (250)  NULL,
    [DepartmentLookup]                BIGINT          NULL,
    [DeploymentPlan]                  NVARCHAR (MAX)  NULL,
    [DeploymentResponsibleUser]       NVARCHAR (MAX)  NULL,
    [Description]                     NVARCHAR (MAX)  NULL,
    [DesiredCompletionDate]           DATETIME        NULL,
    [DeskLocation]                    NVARCHAR (250)  NULL,
    [DivisionLookup]                  BIGINT          NULL,
    [DocumentLibraryName]             NVARCHAR (250)  NULL,
    [DRBRDescription]                 NVARCHAR (MAX)  NULL,
    [DRBRImpactChoice]                NVARCHAR (MAX)  NULL,
    [DRBRManagerUser]                 NVARCHAR (MAX)  NULL,
    [DRQChangeTypeChoice]             NVARCHAR (MAX)  NULL,
    [DRQRapidTypeLookup]              BIGINT          NULL,
    [DRQSystemsLookup]                BIGINT          NULL,
    [DRReplicationChangeChoice]       NVARCHAR (MAX)  NULL,
    [Duration]                        INT             NULL,
    [EstimatedHours]                  INT             NULL,
    [FunctionalAreaLookup]            BIGINT          NULL,
    [History]                         NVARCHAR (MAX)  NULL,
    [ImpactLookup]                    BIGINT          NULL,
    [ImpactsOrganization]             BIT             NULL,
    [InfrastructureManagerUser]       NVARCHAR (MAX)  NULL,
    [InitiatorUser]                   NVARCHAR (MAX)  NULL,
    [InitiatorResolvedChoice]         NVARCHAR (MAX)  NULL,
    [IsUserNotificationRequired]      BIT             NULL,
    [LocationMultLookup]              NVARCHAR(MAX)          NULL,
    [ModuleStepLookup]                NVARCHAR (250)  NULL,
    [NextSLATime]                     DATETIME        NULL,
    [NextSLAType]                     NVARCHAR (250)  NULL,
    [NotificationText]                NVARCHAR (MAX)  NULL,
    [OnHold]                          INT             NULL,
    [OnHoldReasonChoice]              NVARCHAR (MAX)  NULL,
    [OnHoldTillDate]                  DATETIME        NULL,
    [ORPUser]                         NVARCHAR (250)  NULL,
    [Outage]                          NVARCHAR (MAX)  NULL,
    [OwnerUser]                       NVARCHAR (250)  NULL,
    [PriorityLookup]                  BIGINT          NULL,
    [ProductionVerificationPlan]      NVARCHAR (MAX)  NULL,
    [ProductionVerifyResponsibleUser] NVARCHAR (MAX)  NULL,
    [PRPUser]                         NVARCHAR (MAX)  NULL,
    [RapidRequest]                    NVARCHAR (MAX)  NULL,
    [RecoveryPlan]                    NVARCHAR (MAX)  NULL,
    [RelatedRequestID]                NVARCHAR (250)  NULL,
    [RelatedRequestType]              NVARCHAR (250)  NULL,
    [ReopenCount]                     INT             NULL,
    [RequestorUser]                   NVARCHAR (250)  NULL,
    [RequestTypeCategory]             NVARCHAR (250)  NULL,
    [RequestTypeLookup]               BIGINT          NULL,
    [RequestTypeSubCategory]          NVARCHAR (250)  NULL,
    [RequestTypeWorkflow]             NVARCHAR (250)  NULL,
    [ResolutionComments]              NVARCHAR (MAX)  NULL,
    [RiskChoice]                      NVARCHAR (MAX)  NULL,
    [RollbackResponsibleUser]         NVARCHAR (MAX)  NULL,
    [ScheduledEndDateTime]            DATETIME        NULL,
    [ScheduledStartDateTime]          DATETIME        NULL,
    [SecurityManagerUser]             NVARCHAR (250)  NULL,
    [ServiceLookUp]                   BIGINT          NULL,
    [Severity]                        NVARCHAR (MAX)  NULL,
    [SeverityLookup]                  BIGINT          NULL,
    [StageActionUsersUser]            NVARCHAR (250)  NULL,
    [StageActionUserTypes]            NVARCHAR (250)  NULL,
    [StageStep]                       INT             NULL,
    [Status]                          NVARCHAR (250)  NULL,
    [StatusChanged]                   INT             NULL,
    [TargetCompletionDate]            DATETIME        NULL,
    [TargetStartDate]                 DATETIME        NULL,
    [TesterUser]                      NVARCHAR (250)  NULL,
    [TestingDoneChoice]               NVARCHAR (MAX)  NULL,
    [TestPlan]                        NVARCHAR (MAX)  NULL,
    [TicketId]                        NVARCHAR (250)  NULL,
    [ToBeSentByDate]                  DATETIME        NULL,
    [TotalHoldDuration]               INT             NULL,
    [UserImpactDetails]               NVARCHAR (MAX)  NULL,
    [UserQuestionSummary]             NVARCHAR (MAX)  NULL,
    [UsersAffectedUser]               NVARCHAR (250)  NULL,
    [WorkflowSkipStages]              NVARCHAR (250)  NULL,
    [Title]                           VARCHAR (250)   NULL,
    [TenantID]                        NVARCHAR (128)  NULL,
    [DataEditors]                     NVARCHAR (250)  DEFAULT ('') NULL,
    [Created]                         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]                  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                         BIT             DEFAULT ((0)) NULL,
    [Attachments]                     NVARCHAR (2000) DEFAULT ('') NULL,
	[SLADisabled]                     BIT             DEFAULT ((0)) NOT NULL,
    [ResolutionDate]                  DATETIME        NULL,
    [Age]                             BIGINT          NULL,
    [ServiceDeliveryManager]          NVARCHAR (250)  NULL,
    [EnterpriseArchitect]             NVARCHAR (250)  NULL,
    [OnHoldStartDate]                 DATETIME        NULL,
    [AssetLookup]                     NVARCHAR(MAX)          NULL,
    [ImpactsSecurity]                 NVARCHAR (128)  NULL,
    [ApplicationMultiLookup]          BIGINT          NULL,
    [AssignedBy]                      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [OutageHours]                     BIGINT          NULL,
    [DivisionManagerUser]             NVARCHAR (250)  NULL,
    [DepartmentManagerUser]           NVARCHAR (250)  NULL,
    [ResolvedByUser]                  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ClosedByUser]                    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [DataEditor]                      NVARCHAR (250)  NULL,
    [Rejected]                        BIT             DEFAULT ((0)) NOT NULL,
    [ApprovalDate] DATETIME NULL, 
    [ApprovedByUser] VARCHAR(128) NULL, 
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([APPTitleLookup]) REFERENCES [dbo].[Applications] ([ID]),
    FOREIGN KEY ([CompanyTitleLookup]) REFERENCES [dbo].[Company] ([ID]),
    FOREIGN KEY ([DepartmentLookup]) REFERENCES [dbo].[Department] ([ID]),
    FOREIGN KEY ([DivisionLookup]) REFERENCES [dbo].[CompanyDivisions] ([ID]),
    FOREIGN KEY ([DRQRapidTypeLookup]) REFERENCES [dbo].[DRQRapidTypes] ([ID]),
    FOREIGN KEY ([DRQSystemsLookup]) REFERENCES [dbo].[DRQSystemAreas] ([ID]),
    FOREIGN KEY ([FunctionalAreaLookup]) REFERENCES [dbo].[FunctionalAreas] ([ID]),
    FOREIGN KEY ([ImpactLookup]) REFERENCES [dbo].[Config_Module_Impact] ([ID]),
    FOREIGN KEY ([PriorityLookup]) REFERENCES [dbo].[Config_Module_Priority] ([ID]),
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

GO

GO

GO
CREATE NONCLUSTERED INDEX [IX_DRQ_Archive_PriorityLookup] ON [dbo].[DRQ_Archive] ([ModuleStepLookup], [PriorityLookup], [RequestTypeLookup], [Status], [TicketId])