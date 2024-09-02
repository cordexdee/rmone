CREATE TABLE [dbo].[VendorSOW] (
    [ID]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [AdditionalInformation]  NVARCHAR (MAX)  NULL,
    [AgreementNumber]        NVARCHAR (250)  NULL,
    [BusinessManagerUser]    NVARCHAR (MAX)  NULL,
    [Closed]                 BIT             NULL,
    [CloseDate]              DATETIME        NULL,
    [Comment]                NVARCHAR (MAX)  NULL,
    [ContractSigningDate]    DATETIME        NULL,
    [ContractValue]          NVARCHAR (250)  NULL,
    [CreationDate]           DATETIME        NULL,
    [Currency]               NVARCHAR (250)  NULL,
    [CurrentStageStartDate]  DATETIME        NULL,
    [Description]            NVARCHAR (MAX)  NULL,
    [DocumentLibraryName]    NVARCHAR (250)  NULL,
    [EffectiveEndDate]       DATETIME        NULL,
    [EffectiveStartDate]     DATETIME        NULL,
    [FinancialManager]       NVARCHAR (MAX)  NULL,
    [FolderUrl]              NVARCHAR (250)  NULL,
    [FunctionalManager]      NVARCHAR (MAX)  NULL,
    [History]                NVARCHAR (MAX)  NULL,
    [ImpactLookup]           BIGINT          NOT NULL,
    [InitialContractTerm]    INT             NULL,
    [InitiatorUser]          NVARCHAR (MAX)  NULL,
    [IsPartialTermination]   BIT             NULL,
    [IsPrivate]              BIT             NULL,
    [KeyRefUniqueID]         NVARCHAR (250)  NULL,
    [ModuleStepLookup]       NVARCHAR (250)  NULL,
    [OnHold]                 INT             NULL,
    [OnHoldReasonChoice]     NVARCHAR (MAX)  NULL,
    [OnHoldStartDate]        DATETIME        NULL,
    [OnHoldTillDate]         DATETIME        NULL,
    [OwnerUser]              NVARCHAR (250)  NULL,
    [PctComplete]            INT             NULL,
    [PerformanceManager]     NVARCHAR (MAX)  NULL,
    [PriorityLookup]         BIGINT          NOT NULL,
    [RequestorUser]          NVARCHAR (250)  NULL,
    [RequestTypeCategory]    NVARCHAR (250)  NULL,
    [RequestTypeLookup]      BIGINT          NOT NULL,
    [ResolutionTypeChoice]   NVARCHAR (MAX)  NULL,
    [ReSubmissionDate]       DATETIME        NULL,
    [ServiceDeliveryManager] NVARCHAR (MAX)  NULL,
    [ServiceLookUp]          BIGINT          NOT NULL,
    [SeverityLookup]         BIGINT          NOT NULL,
    [StageActionUsersUser]   NVARCHAR (250)  NULL,
    [StageActionUserTypes]   NVARCHAR (250)  NULL,
    [StageStep]              INT             NULL,
    [Status]                 NVARCHAR (250)  NULL,
    [TicketId]               NVARCHAR (250)  NULL,
    [TotalHoldDuration]      INT             NULL,
    [VendorMSALookup]        BIGINT          NOT NULL,
    [VendorMSANameLookup]    BIGINT          NOT NULL,
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
    FOREIGN KEY ([VendorMSANameLookup]) REFERENCES [dbo].[VendorMSA] ([ID])
);










GO

GO

GO

GO

GO

GO

GO
