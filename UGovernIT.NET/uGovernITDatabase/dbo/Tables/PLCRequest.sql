CREATE TABLE [dbo].[PLCRequest] (
    [ID]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [AccountManager]        NVARCHAR (MAX)  NULL,
    [ActualCompletionDate]  DATETIME        NULL,
    [ActualStartDate]       DATETIME        NULL,
    [BillingStartDate]      DATETIME        NULL,
    [BusinessManagerUser]   NVARCHAR (MAX)  NULL,
    [ChanceOfSuccess]       NVARCHAR (MAX)  NULL,
    [Closed]                BIT             NULL,
    [CloseDate]             DATETIME        NULL,
    [Comment]               NVARCHAR (MAX)  NULL,
    [ComponentsNeeded]      NVARCHAR (250)  NULL,
    [CreationDate]          DATETIME        NULL,
    [CRMLookup]             BIGINT          NOT NULL,
    [CurrentStageStartDate] DATETIME        NULL,
    [Description]           NVARCHAR (MAX)  NULL,
    [DesiredCompletionDate] DATETIME        NULL,
    [FollowUp]              NVARCHAR (250)  NULL,
    [History]               NVARCHAR (MAX)  NULL,
    [InitialCost]           NVARCHAR (250)  NULL,
    [InitiatorUser]         NVARCHAR (MAX)  NULL,
    [IsPrivate]             BIT             NULL,
    [ITStaffSize]           INT             NULL,
    [ModuleStepLookup]      NVARCHAR (250)  NULL,
    [NoOfLicenses]          INT             NULL,
    [OnHold]                INT             NULL,
    [OnHoldReasonChoice]    NVARCHAR (MAX)  NULL,
    [OnHoldStartDate]       DATETIME        NULL,
    [OnHoldTillDate]        DATETIME        NULL,
    [OwnerUser]             NVARCHAR (250)  NULL,
    [PctComplete]           INT             NULL,
    [PlanResources]         NVARCHAR (250)  NULL,
    [Price]                 NVARCHAR (250)  NULL,
    [PriorityLookup]        BIGINT          NOT NULL,
    [RequestorUser]         NVARCHAR (250)  NULL,
    [RequestSourceChoice]   NVARCHAR (MAX)  NULL,
    [RequestTypeCategory]   NVARCHAR (250)  NULL,
    [RequestTypeLookup]     BIGINT          NOT NULL,
    [RequestTypeWorkflow]   NVARCHAR (250)  NULL,
    [ResolutionComments]    NVARCHAR (MAX)  NULL,
    [ReSubmissionDate]      DATETIME        NULL,
    [ScopeOfServices]       NVARCHAR (MAX)  NULL,
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
    [TenantID]              NVARCHAR (128)  NULL,
    [DataEditors]           NVARCHAR (250)  DEFAULT ('') NULL,
    [Created]               DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]              DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]         NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]        NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]               BIT             DEFAULT ((0)) NULL,
    [Attachments]           NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([CRMLookup]) REFERENCES [dbo].[Customers] ([ID]),
    FOREIGN KEY ([PriorityLookup]) REFERENCES [dbo].[Config_Module_Priority] ([ID]),
    FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID])
);










GO

GO

GO
