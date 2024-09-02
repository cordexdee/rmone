CREATE TABLE [dbo].[Contracts] (
    [ID]                      BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActualCompletionDate]    DATETIME        NULL,
    [ActualStartDate]         DATETIME        NULL,
    [AnnualMaintenanceCost]   NVARCHAR (250)  NULL,
    [BusinessManagerUser]     NVARCHAR (MAX)  NULL,
    [Closed]                  BIT             NULL,
    [CloseDate]               DATETIME        NULL,
    [Comment]                 NVARCHAR (MAX)  NULL,
    [ContractExpirationDate]  DATETIME        NULL,
    [ContractStartDate]       DATETIME        NULL,
    [CurrentStageStartDate]   DATETIME        NULL,
    [Description]             NVARCHAR (MAX)  NULL,
    [DesiredCompletionDate]   DATETIME        NULL,
    [EstimatedHours]          INT             NULL,
    [FinanceManagerUser]      NVARCHAR (MAX)  NULL,
    [FunctionalAreaLookup]    BIGINT          NULL,
    [History]                 NVARCHAR (MAX)  NULL,
    [InitialCost]             NVARCHAR (250)  NULL,
    [InitiatorUser]           NVARCHAR (MAX)  NULL,
    [IsPrivate]               BIT             NULL,
    [LegalUser]               NVARCHAR (MAX)  NULL,
    [LicenseCount]            INT             NULL,
    [ModuleStepLookup]        NVARCHAR (250)  NULL,
    [NeedReviewChoice]        NVARCHAR (MAX)  NULL,
    [OnHold]                  INT             NULL,
    [OnHoldReasonChoice]      NVARCHAR (MAX)  NULL,
    [OnHoldStartDate]         DATETIME        NULL,
    [OnHoldTillDate]          DATETIME        NULL,
    [OwnerUser]               NVARCHAR (250)  NULL,
    [PctComplete]             INT             NULL,
    [PONumber]                NVARCHAR (250)  NULL,
    [PriorityLookup]          BIGINT          NULL,
    [PurchaseInstructions]    NVARCHAR (MAX)  NULL,
    [PurchasingUser]          NVARCHAR (MAX)  NULL,
    [Quantity]                INT             NULL,
    [ReminderBody]            NVARCHAR (MAX)  NULL,
    [ReminderDate]            DATETIME        NULL,
    [ReminderDays]            INT             NULL,
    [ReminderToUser]          NVARCHAR (250)  NULL,
    [RenewalCancelNoticeDays] INT             NULL,
    [RepeatIntervalChoice]    NVARCHAR (MAX)  NULL,
    [RequestorUser]           NVARCHAR (250)  NULL,
    [RequestSourceChoice]     NVARCHAR (MAX)  NULL,
    [RequestTypeCategory]     NVARCHAR (250)  NULL,
    [RequestTypeLookup]       BIGINT          NULL,
    [RequestTypeWorkflow]     NVARCHAR (250)  NULL,
    [ResolutionComments]      NVARCHAR (MAX)  NULL,
    [ReSubmissionDate]        DATETIME        NULL,
    [ServiceLookUp]           BIGINT          NULL,
    [StageActionUsersUser]    NVARCHAR (250)  NULL,
    [StageActionUserTypes]    NVARCHAR (250)  NULL,
    [StageStep]               INT             NULL,
    [Status]                  NVARCHAR (250)  NULL,
    [StatusChanged]           INT             NULL,
    [TargetCompletionDate]    DATETIME        NULL,
    [TargetStartDate]         DATETIME        NULL,
    [TermTypeChoice]          NVARCHAR (MAX)  NULL,
    [TicketId]                NVARCHAR (250)  NULL,
    [Title]                   NVARCHAR (250)  NULL,
    [TotalHoldDuration]       INT             NULL,
    [UserQuestionSummary]     NVARCHAR (MAX)  NULL,
    [WorkflowSkipStages]      NVARCHAR (250)  NULL,
    [TenantID]                NVARCHAR (128)  NULL,
    [DataEditors]             NVARCHAR (250)  DEFAULT ('') NULL,
    [Created]                 DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]           NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                 BIT             DEFAULT ((0)) NULL,
    [Attachments]             NVARCHAR (2000) DEFAULT ('') NULL,
    [ReopenCount] INT NULL, 
    [ApprovalDate] DATETIME NULL, 
    [ApprovedByUser] VARCHAR(128) NULL, 
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([FunctionalAreaLookup]) REFERENCES [dbo].[FunctionalAreas] ([ID]),
    FOREIGN KEY ([PriorityLookup]) REFERENCES [dbo].[Config_Module_Priority] ([ID]),
    FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID]),
    FOREIGN KEY ([ServiceLookUp]) REFERENCES [dbo].[Config_Services] ([ID])
);










GO

GO

GO

GO
