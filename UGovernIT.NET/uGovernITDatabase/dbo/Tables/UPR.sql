CREATE TABLE [dbo].[UPR] (
    [ID]                      BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActualCompletionDate]    DATETIME        NULL,
    [ActualHours]             INT             NULL,
    [ActualStartDate]         DATETIME        NULL,
    [AssetLookup]             NVARCHAR(MAX)   NULL,
    [BusinessManagerUser]     NVARCHAR (MAX)  NULL,
    [CloseDate]               DATETIME        NULL,
    [Comment]                 NVARCHAR (MAX)  NULL,
    [CurrentStageStartDate]   DATETIME        NULL,
    [DepartmentLookup]        BIGINT          NOT NULL,
    [Description]             NVARCHAR (MAX)  NULL,
    [DesiredCompletionDate]   DATETIME        NULL,
    [EstimatedHours]          INT             NULL,
    [FunctionalAreaLookup]    BIGINT          NOT NULL,
    [GLCode]                  NVARCHAR (250)  NULL,
    [History]                 NVARCHAR (MAX)  NULL,
    [ImpactLookup]            BIGINT          NOT NULL,
    [InitiatorUser]           NVARCHAR (MAX)  NULL,
    [InitiatorResolvedChoice] NVARCHAR (MAX)  NULL,
    [ModuleStepLookup]        NVARCHAR (250)  NULL,
    [OnHold]                  INT             NULL,
    [OnHoldStartDate]         DATETIME        NULL,
    [ORPUser]                 NVARCHAR (250)  NULL,
    [OwnerUser]               NVARCHAR (250)  NULL,
    [PctComplete]             INT             NULL,
    [PriorityLookup]          BIGINT          NOT NULL,
    [PRPUser]                 NVARCHAR (MAX)  NULL,
    [RequestorUser]           NVARCHAR (250)  NULL,
    [RequestSourceChoice]     NVARCHAR (MAX)  NULL,
    [RequestTypeCategory]     NVARCHAR (250)  NULL,
    [RequestTypeLookup]       BIGINT          NOT NULL,
    [RequestTypeWorkflow]     NVARCHAR (250)  NULL,
    [ResoltuonComments]       NVARCHAR (MAX)  NULL,
    [ReSubmissionDate]        DATETIME        NULL,
    [SecurityManagerUser]     NVARCHAR (250)  NULL,
    [ServiceLookUp]           BIGINT          NOT NULL,
    [SeverityLookup]          BIGINT          NOT NULL,
    [StageActionUsersUser]    NVARCHAR (250)  NULL,
    [StageActionUserTypes]    NVARCHAR (250)  NULL,
    [Status]                  NVARCHAR (250)  NULL,
    [StatusChanged]           INT             NULL,
    [TargetCompletionDate]    DATETIME        NULL,
    [TargetStartDate]         DATETIME        NULL,
    [TesterUser]              NVARCHAR (MAX)  NULL,
    [TicketId]                NVARCHAR (250)  NULL,
    [TotalHoldDuration]       INT             NULL,
    [Title]                   VARCHAR (250)   NULL,
    [TenantID]                NVARCHAR (128)  NULL,
    [Created]                 DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]           NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                 BIT             DEFAULT ((0)) NULL,
    [Attachments]             NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([DepartmentLookup]) REFERENCES [dbo].[Department] ([ID]),
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
