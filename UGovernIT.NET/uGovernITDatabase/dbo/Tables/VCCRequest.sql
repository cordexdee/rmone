CREATE TABLE [dbo].[VCCRequest] (
    [ID]                      BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActualCompletionDate]    DATETIME        NULL,
    [ActualHours]             INT             NULL,
    [ActualStartDate]         DATETIME        NULL,
    [AmendmentEffectiveDate]  DATETIME        NULL,
    [AmendmentId]             NVARCHAR (250)  NULL,
    [AmendmentReceived]       DATETIME        NULL,
    [APPTitleLookup]          BIGINT          NOT NULL,
    [AssetLookup]             BIGINT          NOT NULL,
    [BusinessManagerUser]     NVARCHAR (MAX)  NULL,
    [Closed]                  BIT             NULL,
    [CloseDate]               DATETIME        NULL,
    [Comment]                 NVARCHAR (MAX)  NULL,
    [CommercialMgtLead]       NVARCHAR (MAX)  NULL,
    [CompanyTitleLookup]      BIGINT          NOT NULL,
    [CreationDate]            DATETIME        NULL,
    [CRNumber]                NVARCHAR (250)  NULL,
    [CRSignedDate]            DATETIME        NULL,
    [CurrentStageStartDate]   DATETIME        NULL,
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
    [DeliverableImpact]       BIT             NULL,
    [DepartmentLookup]        BIGINT          NOT NULL,
    [Description]             NVARCHAR (MAX)  NULL,
    [DescriptionofService]    NVARCHAR (MAX)  NULL,
    [DesiredCompletionDate]   DATETIME        NULL,
    [DivisionLookup]          BIGINT          NOT NULL,
    [DocumentLibraryName]     NVARCHAR (250)  NULL,
    [EstimatedHours]          INT             NULL,
    [FinancialImpactAmount]   NVARCHAR (250)  NULL,
    [FolderUrl]               NVARCHAR (250)  NULL,
    [FunctionalAreaLookup]    BIGINT          NOT NULL,
    [History]                 NVARCHAR (MAX)  NULL,
    [ImpactLookup]            BIGINT          NOT NULL,
    [InitiatorUser]           NVARCHAR (MAX)  NULL,
    [InitiatorResolvedChoice] NVARCHAR (MAX)  NULL,
    [IsPrivate]               BIT             NULL,
    [LocationLookup]          BIGINT          NOT NULL,
    [ManagerApprovalNeeded]   BIT             NULL,
    [MGSSubmittedDate]        DATETIME        NULL,
    [ModuleStepLookup]        NVARCHAR (250)  NULL,
    [NextSLATime]             DATETIME        NULL,
    [NextSLAType]             NVARCHAR (250)  NULL,
    [OnHold]                  INT             NULL,
    [OnHoldReasonChoice]      NVARCHAR (MAX)  NULL,
    [OnHoldStartDate]         DATETIME        NULL,
    [OnHoldTillDate]          DATETIME        NULL,
    [ORPUser]                 NVARCHAR (MAX)  NULL,
    [OwnerUser]               NVARCHAR (250)  NULL,
    [PctComplete]             INT             NULL,
    [PriorityLookup]          BIGINT          NOT NULL,
    [ProjectName]             NVARCHAR (250)  NULL,
    [PRPUser]                 NVARCHAR (MAX)  NULL,
    [PRPGroupUser]            NVARCHAR (MAX)  NULL,
    [RequestorUser]           NVARCHAR (250)  NULL,
    [RequestorContacted]      BIT             NULL,
    [RequestorName]           NVARCHAR (250)  NULL,
    [RequestorOrganization]   NVARCHAR (250)  NULL,
    [RequestSourceChoice]     NVARCHAR (MAX)  NULL,
    [RequestTypeCategory]     NVARCHAR (250)  NULL,
    [RequestTypeLookup]       BIGINT          NOT NULL,
    [RequestTypeWorkflow]     NVARCHAR (250)  NULL,
    [ResolutionComments]      NVARCHAR (MAX)  NULL,
    [ResolutionTypeChoice]    NVARCHAR (MAX)  NULL,
    [ReSubmissionDate]        DATETIME        NULL,
    [RFSFormComplete]         BIT             NULL,
    [RFSSubmissionDate]       DATETIME        NULL,
    [RFSSubmitted]            BIT             NULL,
    [SCRBDate]                DATETIME        NULL,
    [ServiceLookUp]           BIGINT          NOT NULL,
    [SeverityLookup]          BIGINT          NOT NULL,
    [SLAImpact]               BIT             NULL,
    [StageActionUsersUser]    NVARCHAR (250)  NULL,
    [StageActionUserTypes]    NVARCHAR (250)  NULL,
    [StageStep]               INT             NULL,
    [Status]                  NVARCHAR (250)  NULL,
    [StatusChanged]           INT             NULL,
    [SubmissionDate]          DATETIME        NULL,
    [TargetCompletionDate]    DATETIME        NULL,
    [TargetStartDate]         DATETIME        NULL,
    [TesterUser]              NVARCHAR (250)  NULL,
    [TicketId]                NVARCHAR (250)  NULL,
    [TotalHoldDuration]       INT             NULL,
    [UserQuestionSummary]     NVARCHAR (MAX)  NULL,
    [VendorCRSignedDate]      DATETIME        NULL,
    [VendorMSALookup]         BIGINT          NOT NULL,
    [VendorMSANameLookup]     BIGINT          NOT NULL,
    [VendorSOWLookup]         BIGINT          NOT NULL,
    [VendorSOWNameLookup]     BIGINT          NOT NULL,
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
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([APPTitleLookup]) REFERENCES [dbo].[Applications] ([ID]),
    FOREIGN KEY ([AssetLookup]) REFERENCES [dbo].[Assets] ([ID]),
    FOREIGN KEY ([CompanyTitleLookup]) REFERENCES [dbo].[Company] ([ID]),
    FOREIGN KEY ([DepartmentLookup]) REFERENCES [dbo].[Department] ([ID]),
    FOREIGN KEY ([DivisionLookup]) REFERENCES [dbo].[CompanyDivisions] ([ID]),
    FOREIGN KEY ([FunctionalAreaLookup]) REFERENCES [dbo].[FunctionalAreas] ([ID]),
    FOREIGN KEY ([ImpactLookup]) REFERENCES [dbo].[Config_Module_Impact] ([ID]),
    FOREIGN KEY ([LocationLookup]) REFERENCES [dbo].[Location] ([ID]),
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

GO

GO

GO

GO

GO

GO

GO
CREATE NONCLUSTERED INDEX [IX_VCCRequest_PriorityLookup] ON [dbo].[VCCRequest] ([ModuleStepLookup], [PriorityLookup], [RequestTypeLookup], [Status], [TicketId], [VendorMSALookup], [VendorSOWLookup])