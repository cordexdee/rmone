CREATE TABLE [dbo].[VendorSOWInvoices] (
    [ID]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [AmountDisputed]         NVARCHAR (250)  NULL,
    [AmountPaid]             NVARCHAR (250)  NULL,
    [BudgetAmount]           NVARCHAR (250)  NULL,
    [BusinessManagerUser]    NVARCHAR (MAX)  NULL,
    [Closed]                 BIT             NULL,
    [CloseDate]              DATETIME        NULL,
    [Comment]                NVARCHAR (MAX)  NULL,
    [CreationDate]           DATETIME        NULL,
    [CurrentStageStartDate]  DATETIME        NULL,
    [DatePaid]               DATETIME        NULL,
    [Description]            NVARCHAR (MAX)  NULL,
    [DocumentLibraryName]    NVARCHAR (250)  NULL,
    [DueDate]                DATETIME        NULL,
    [FinancialManager]       NVARCHAR (MAX)  NULL,
    [FolderUrl]              NVARCHAR (250)  NULL,
    [FunctionalManager]      NVARCHAR (MAX)  NULL,
    [History]                NVARCHAR (MAX)  NULL,
    [ImpactLookup]           BIGINT          NOT NULL,
    [InitiatorUser]          NVARCHAR (MAX)  NULL,
    [InvoiceNumber]          NVARCHAR (250)  NULL,
    [IsPrivate]              BIT             NULL,
    [ModuleStepLookup]       NVARCHAR (250)  NULL,
    [NextSLATime]            DATETIME        NULL,
    [NextSLAType]            NVARCHAR (250)  NULL,
    [OnHold]                 INT             NULL,
    [OnHoldReasonChoice]     NVARCHAR (MAX)  NULL,
    [OnHoldStartDate]        DATETIME        NULL,
    [OnHoldTillDate]         DATETIME        NULL,
    [OwnerUser]              NVARCHAR (250)  NULL,
    [PctComplete]            INT             NULL,
    [PerformanceManager]     NVARCHAR (MAX)  NULL,
    [PONumber]               NVARCHAR (250)  NULL,
    [PriorityLookup]         BIGINT          NOT NULL,
    [ReceivedOn]             DATETIME        NULL,
    [RequestorUser]          NVARCHAR (250)  NULL,
    [RequestTypeCategory]    NVARCHAR (250)  NULL,
    [RequestTypeLookup]      BIGINT          NOT NULL,
    [ResolutionTypeChoice]   NVARCHAR (MAX)  NULL,
    [ReSubmissionDate]       DATETIME        NULL,
    [ServiceDeliveryManager] NVARCHAR (MAX)  NULL,
    [ServiceLookUp]          BIGINT          NOT NULL,
    [SeverityLookup]         BIGINT          NOT NULL,
    [SOWInvoiceActualAmount] NVARCHAR (250)  NULL,
    [SOWInvoiceAmount]       NVARCHAR (250)  NULL,
    [SOWInvoiceDate]         DATETIME        NULL,
    [StageActionUsersUser]   NVARCHAR (250)  NULL,
    [StageActionUserTypes]   NVARCHAR (250)  NULL,
    [StageStep]              INT             NULL,
    [Status]                 NVARCHAR (250)  NULL,
    [TicketId]               NVARCHAR (250)  NULL,
    [TotalHoldDuration]      INT             NULL,
    [UserQuestionSummary]    NVARCHAR (MAX)  NULL,
    [VendorMSALookup]        BIGINT          NOT NULL,
    [VendorMSANameLookup]    BIGINT          NOT NULL,
    [VendorSOWLookup]        BIGINT          NOT NULL,
    [VendorSOWNameLookup]    BIGINT          NOT NULL,
    [VMOApprover]            NVARCHAR (250)  NULL,
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
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ImpactLookup]) REFERENCES [dbo].[Config_Module_Impact] ([ID]),
    FOREIGN KEY ([PriorityLookup]) REFERENCES [dbo].[Config_Module_Priority] ([ID]),
    FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID]),
    FOREIGN KEY ([ServiceLookUp]) REFERENCES [dbo].[Config_Services] ([ID]),
    FOREIGN KEY ([SeverityLookup]) REFERENCES [dbo].[Config_Module_Severity] ([ID]),
    FOREIGN KEY ([VendorMSALookup]) REFERENCES [dbo].[VendorMSA] ([ID]),
    FOREIGN KEY ([VendorMSANameLookup]) REFERENCES [dbo].[VendorMSA] ([ID]),
    FOREIGN KEY ([VendorSOWLookup]) REFERENCES [dbo].[VendorSOW] ([ID]),
    FOREIGN KEY ([VendorSOWNameLookup]) REFERENCES [dbo].[VendorSOW] ([ID])
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
