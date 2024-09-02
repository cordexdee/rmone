CREATE TABLE [dbo].[BTS_Archive] (
    [ID]                      BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActualCompletionDate]    DATETIME        NULL,
    [ActualHours]             INT             NULL,
    [ActualStartDate]         DATETIME        NULL,
    [ApplicationModule]       NVARCHAR (250)  NULL,
    [ApplicationName]         NVARCHAR (250)  NULL,
    [CategoryChoice]          NVARCHAR (MAX)  NULL,
    [Closed]                  BIT             NULL,
    [CloseDate]               DATETIME        NULL,
    [Comment]                 NVARCHAR (MAX)  NULL,
    [CompanyTitleLookup]      BIGINT          NULL,
    [CreationDate]            DATETIME        NULL,
    [CurrentStageStartDate]   DATETIME        NULL,
    [CustomUGChoice01]        NVARCHAR (MAX)  NULL,
    [CustomUGChoice02]        NVARCHAR (MAX)  NULL,
    [CustomUGChoice03]        NVARCHAR (MAX)  NULL,
    [CustomUGChoice04]        NVARCHAR (MAX)  NULL,
    [CustomUGDate01]          DATETIME        NULL,
    [CustomUGDate02]          DATETIME        NULL,
    [CustomUGDate03]          DATETIME        NULL,
    [CustomUGDate04]          DATETIME        NULL,
    [CustomUGText01]          NVARCHAR (250)  NULL,
    [CustomUGText02]          NVARCHAR (250)  NULL,
    [CustomUGText03]          NVARCHAR (250)  NULL,
    [CustomUGText04]          NVARCHAR (250)  NULL,
    [CustomUGText05]          NVARCHAR (250)  NULL,
    [CustomUGText06]          NVARCHAR (250)  NULL,
    [CustomUGText07]          NVARCHAR (250)  NULL,
    [CustomUGText08]          NVARCHAR (250)  NULL,
    [CustomUGUser01]          NVARCHAR (MAX)  NULL,
    [CustomUGUser02]          NVARCHAR (MAX)  NULL,
    [CustomUGUser03]          NVARCHAR (MAX)  NULL,
    [CustomUGUser04]          NVARCHAR (MAX)  NULL,
    [CustomUGUserMulti01]     NVARCHAR (250)  NULL,
    [CustomUGUserMulti02]     NVARCHAR (250)  NULL,
    [CustomUGUserMulti03]     NVARCHAR (250)  NULL,
    [CustomUGUserMulti04]     NVARCHAR (250)  NULL,
    [DepartmentLookup]        BIGINT          NULL,
    [Description]             NVARCHAR (MAX)  NULL,
    [DesiredCompletionDate]   DATETIME        NULL,
    [DeskLocation]            NVARCHAR (250)  NULL,
    [DivisionLookup]          BIGINT          NULL,
    [DocumentLibraryName]     NVARCHAR (250)  NULL,
    [EstimatedHours]          INT             NULL,
    [FunctionalAreaLookup]    BIGINT          NULL,
    [History]                 NVARCHAR (MAX)  NULL,
    [ImpactLookup]            BIGINT          NULL,
    [InitiatorUser]           NVARCHAR (MAX)  NULL,
    [InitiatorResolvedChoice] NVARCHAR (MAX)  NULL,
    [LocationLookup]          BIGINT          NULL,
    [ModuleStepLookup]        NVARCHAR (250)  NULL,
    [NextSLATime]             DATETIME        NULL,
    [NextSLAType]             NVARCHAR (250)  NULL,
    [OnHold]                  INT             NULL,
    [OnHoldReasonChoice]      NVARCHAR (MAX)  NULL,
    [OnHoldTillDate]          DATETIME        NULL,
    [ORPUser]                 NVARCHAR (250)  NULL,
    [OwnerUser]               NVARCHAR (250)  NULL,
    [PriorityLookup]          BIGINT          NULL,
    [PRPUser]                 NVARCHAR (MAX)  NULL,
    [PRPGroupUser]            NVARCHAR (MAX)  NULL,
    [RelatedRecords]          NVARCHAR (250)  NULL,
    [ReopenCount]             INT             NULL,
    [RequestorUser]           NVARCHAR (250)  NULL,
    [RequestTypeCategory]     NVARCHAR (250)  NULL,
    [RequestTypeLookup]       BIGINT          NULL,
    [RequestTypeSubCategory]  NVARCHAR (250)  NULL,
    [RequestTypeWorkflow]     NVARCHAR (250)  NULL,
    [ResolutionComments]      NVARCHAR (MAX)  NULL,
    [ResolutionTypeChoice]    NVARCHAR (MAX)  NULL,
    [ServiceLookUp]           BIGINT          NULL,
    [SeverityLookup]          BIGINT          NULL,
    [StageActionUsersUser]    NVARCHAR (250)  NULL,
    [StageActionUserTypes]    NVARCHAR (250)  NULL,
    [StageStep]               INT             NULL,
    [Status]                  NVARCHAR (250)  NULL,
    [StatusChanged]           INT             NULL,
    [TargetCompletionDate]    DATETIME        NULL,
    [TargetStartDate]         DATETIME        NULL,
    [TesterUser]              NVARCHAR (250)  NULL,
    [TicketId]                NVARCHAR (250)  NULL,
    [TotalHoldDuration]       INT             NULL,
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
    [SLADisabled]             BIT             NULL,
	[Age]                             BIGINT          NULL,
    [IconBlob] VARBINARY(MAX) NULL, 
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
CREATE NONCLUSTERED INDEX [IX_BTS_Archive_ModuleStepLookup] ON [dbo].[BTS_Archive] ([ModuleStepLookup])

GO
CREATE NONCLUSTERED INDEX [IX_BTS_Archive_PriorityLookup] ON [dbo].[BTS_Archive] ([PriorityLookup])

GO
CREATE NONCLUSTERED INDEX [IX_BTS_Archive_RequestTypeLookup] ON [dbo].[BTS_Archive] ([RequestTypeLookup])

GO
CREATE NONCLUSTERED INDEX [IX_BTS_Archive_TicketId] ON [dbo].[BTS_Archive] ([TicketId])