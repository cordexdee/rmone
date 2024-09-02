CREATE TABLE [dbo].[ITGovernance] (
    [ID]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [Closed]                 BIT             NULL,
    [Comment]                NVARCHAR (MAX)  NULL,
    [CreationDate]           DATETIME        NULL,
    [CurrentStageStartDate]  DATETIME        NULL,
    [DesiredCompletionDate]  DATETIME        NULL,
    [History]                NVARCHAR (MAX)  NULL,
    [ImpactLookup]           BIGINT          NOT NULL,
    [InitiatorUser]          NVARCHAR (MAX)  NULL,
    [ModuleStepLookup]       NVARCHAR (250)  NULL,
    [OnHold]                 INT             NULL,
    [OnHoldReasonChoice]     NVARCHAR (MAX)  NULL,
    [OnHoldTillDate]         DATETIME        NULL,
    [OwnerUser]              NVARCHAR (250)  NULL,
    [RequestorUser]          NVARCHAR (250)  NULL,
    [RequestTypeCategory]    NVARCHAR (250)  NULL,
    [RequestTypeLookup]      BIGINT          NOT NULL,
    [RequestTypeSubCategory] NVARCHAR (250)  NULL,
    [ServiceLookUp]          BIGINT          NOT NULL,
    [SeverityLookup]         BIGINT          NOT NULL,
    [StageActionUsersUser]   NVARCHAR (250)  NULL,
    [StageStep]              INT             NULL,
    [Status]                 NVARCHAR (250)  NULL,
    [StatusChanged]          INT             NULL,
    [TargetCompletionDate]   DATETIME        NULL,
    [TargetStartDate]        DATETIME        NULL,
    [TicketId]               NVARCHAR (250)  NULL,
    [WorkflowSkipStages]     NVARCHAR (250)  NULL,
    [Title]                  VARCHAR (250)   NULL,
    [TenantID]               NVARCHAR (128)  NULL,
    [DataEditors]            NVARCHAR (250)  DEFAULT ('') NULL,
    [Created]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]               DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]         NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                BIT             DEFAULT ((0)) NULL,
    [Attachments]            NVARCHAR (2000) DEFAULT ('') NULL,
    [ResolutionDate]         DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ImpactLookup]) REFERENCES [dbo].[Config_Module_Impact] ([ID]),
    FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID]),
    FOREIGN KEY ([ServiceLookUp]) REFERENCES [dbo].[Config_Services] ([ID]),
    FOREIGN KEY ([SeverityLookup]) REFERENCES [dbo].[Config_Module_Severity] ([ID])
);










GO

GO

GO

GO
