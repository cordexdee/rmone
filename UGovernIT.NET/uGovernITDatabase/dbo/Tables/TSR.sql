CREATE TABLE [dbo].[TSR] (
    [ID]                        BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActualCompletionDate]      DATETIME       NULL,
    [ActualHours]               INT            NULL,
    [ActualStartDate]           DATETIME       NULL,
    [APPTitleLookup]            BIGINT         NULL,
    [AssetLookup]               NVARCHAR(MAX)  NULL,
    [Attachments]               NVARCHAR (MAX) NULL,
    [BusinessManagerUser]       NVARCHAR (MAX) NULL,
    [Closed]                    BIT            NULL,
    [CloseDate]                 DATETIME       NULL,
    [Comment]                   NVARCHAR (MAX) NULL,
    [CompanyTitleLookup]        BIGINT         NULL,
    [CreationDate]              DATETIME       NULL,
    [CurrentStageStartDate]     DATETIME       NULL,
    [CustomUGChoice01]          NVARCHAR (MAX) NULL,
    [CustomUGChoice02]          NVARCHAR (MAX) NULL,
    [CustomUGChoice03]          NVARCHAR (MAX) NULL,
    [CustomUGChoice04]          NVARCHAR (MAX) NULL,
    [CustomUGDate01]            DATETIME       NULL,
    [CustomUGDate02]            DATETIME       NULL,
    [CustomUGDate03]            DATETIME       NULL,
    [CustomUGDate04]            DATETIME       NULL,
    [CustomUGText01]            NVARCHAR (250) NULL,
    [CustomUGText02]            NVARCHAR (250) NULL,
    [CustomUGText03]            NVARCHAR (250) NULL,
    [CustomUGText04]            NVARCHAR (250) NULL,
    [CustomUGText05]            NVARCHAR (250) NULL,
    [CustomUGText06]            NVARCHAR (250) NULL,
    [CustomUGText07]            NVARCHAR (250) NULL,
    [CustomUGText08]            NVARCHAR (250) NULL,
    [CustomUGUser01]            NVARCHAR (MAX) NULL,
    [CustomUGUser02]            NVARCHAR (MAX) NULL,
    [CustomUGUser03]            NVARCHAR (MAX) NULL,
    [CustomUGUser04]            NVARCHAR (MAX) NULL,
    [CustomUGUserMulti01]       NVARCHAR (250) NULL,
    [CustomUGUserMulti02]       NVARCHAR (250) NULL,
    [CustomUGUserMulti03]       NVARCHAR (250) NULL,
    [CustomUGUserMulti04]       NVARCHAR (250) NULL,
    [DepartmentLookup]          BIGINT         NULL,
    [Description]               NVARCHAR (MAX) NULL,
    [DesiredCompletionDate]     DATETIME       NULL,
    [DeskLocation]              NVARCHAR (250) NULL,
    [DivisionLookup]            BIGINT         NULL,
    [DocumentLibraryName]       NVARCHAR (250) NULL,
    [EstimatedHours]            INT            NULL,
    [ExternalID]                NVARCHAR (250) NULL,
    [FunctionalAreaLookup]      BIGINT         NULL,
    [GLCode]                    NVARCHAR (250) NULL,
    [History]                   NVARCHAR (MAX) NULL,
    [ImpactLookup]              BIGINT         NULL,
    [InfrastructureManagerUser] NVARCHAR (MAX) NULL,
    [InitiatorUser]             NVARCHAR (MAX) NULL,
    [InitiatorResolvedChoice]   NVARCHAR (MAX) NULL,
    [isAttachment]              BIT            DEFAULT ((0)) NULL,
    [IsPrivate]                 BIT            NULL,
    [IssueTypeChoice]           NVARCHAR (250) NULL,
    [LocationLookup]            BIGINT         NULL,
    [ManagerApprovalNeeded]     BIT            NULL,
    [ModuleStepLookup]          NVARCHAR (250) NULL,
    [NextSLATime]               DATETIME       NULL,
    [NextSLAType]               NVARCHAR (250) NULL,
    [OnHold]                    INT            NULL,
    [OnHoldReasonChoice]        NVARCHAR (MAX) NULL,
    [OnHoldStartDate]           DATETIME       NULL,
    [OnHoldTillDate]            DATETIME       NULL,
    [ORPUser]                   NVARCHAR (MAX) NULL,
    [OwnerUser]                 NVARCHAR (250) NULL,
    [PctComplete]               INT            NULL,
    [PriorityLookup]            BIGINT         NULL,
    [PRPUser]                   NVARCHAR (MAX) NULL,
    [PRPGroupUser]              NVARCHAR (MAX) NULL,
    [RequestorUser]             NVARCHAR (250) NULL,
    [RequestorContacted]        BIT            NULL,
    [RequestSourceChoice]       NVARCHAR (MAX) NULL,
    [RequestTypeCategory]       NVARCHAR (250) NULL,
    [RequestTypeLookup]         BIGINT         NULL,
    [RequestTypeSubCategory]    NVARCHAR (250) NULL,
    [RequestTypeWorkflow]       NVARCHAR (250) NULL,
    [ResolutionComments]        NVARCHAR (MAX) NULL,
    [ResolutionTypeChoice]      NVARCHAR (MAX) NULL,
    [ReSubmissionDate]          DATETIME       NULL,
    [SecurityManagerUser]       NVARCHAR (250) NULL,
    [ServiceLookUp]             BIGINT         NULL,
    [SeverityLookup]            BIGINT         NULL,
    [StageActionUsersUser]      NVARCHAR (250) NULL,
    [StageActionUserTypes]      NVARCHAR (250) NULL,
    [StageStep]                 INT            NULL,
    [Status]                    NVARCHAR (250) NULL,
    [StatusChanged]             INT            NULL,
    [TargetCompletionDate]      DATETIME       NULL,
    [TargetStartDate]           DATETIME       NULL,
    [TesterUser]                NVARCHAR (250) NULL,
    [TicketId]                  NVARCHAR (250) NULL,
    [TotalHoldDuration]         INT            NULL,
    [WorkflowSkipStages]        NVARCHAR (250) NULL,
    [Title]                     VARCHAR (250)  NULL,
    [TenantID]                  NVARCHAR (128) NULL,
    [DataEditors]               NVARCHAR (250) DEFAULT ('') NULL,
    [Created]                   DATETIME       DEFAULT (getdate()) NOT NULL,
    [Modified]                  DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]             NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]            NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                   BIT            DEFAULT ((0)) NULL,
    [SLADisabled]               BIT            DEFAULT ((0)) NOT NULL,
    [AgentSummary]              NVARCHAR (MAX) NULL,
    [ElevatedPriority]          BIT            DEFAULT ((0)) NULL,
    [reopencount]               INT            NULL,
    [ResolutionDate]            DATETIME       NULL,
    [ResolvedByUser] NVARCHAR(128) NULL, 	
	[Age]                             BIGINT          NULL,
    [AssetConditionChoice] NVARCHAR(250) NULL, 
    [IconBlob] [varbinary](max) NULL,
    CONSTRAINT [PK_TSR] PRIMARY KEY CLUSTERED ([ID] ASC)
);


















GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_TSR_TicketId] ON [dbo].[TSR] ([TicketId],[TenantID])

GO
CREATE NONCLUSTERED INDEX [IX_TSR_ModuleStepLookup] ON [dbo].[TSR] ([ModuleStepLookup])

GO
CREATE NONCLUSTERED INDEX [IX_TSR_PriorityLookup] ON [dbo].[TSR] ([PriorityLookup])

GO
CREATE NONCLUSTERED INDEX [IX_TSR_RequestTypeLookup] ON [dbo].[TSR] ([RequestTypeLookup])

GO
CREATE NONCLUSTERED INDEX [IX_TSR_Status] ON [dbo].[TSR] ([Status])

GO
CREATE NONCLUSTERED INDEX [IX_TSR_TicketId_NC] ON [dbo].[TSR] ([TicketId])
