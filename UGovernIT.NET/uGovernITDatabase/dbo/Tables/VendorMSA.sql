﻿CREATE TABLE [dbo].[VendorMSA] (
    [ID]                                   BIGINT          IDENTITY (1, 1) NOT NULL,
    [AdditionalComments]                   NVARCHAR (MAX)  NULL,
    [AdditionalCommentsByVendor]           NVARCHAR (MAX)  NULL,
    [AdditionalInformation]                NVARCHAR (MAX)  NULL,
    [AgreementNumber]                      NVARCHAR (250)  NULL,
    [AnnualRevenue]                        NVARCHAR (250)  NULL,
    [AttritionInAccount]                   INT             NULL,
    [BusinessManagerUser]                  NVARCHAR (MAX)  NULL,
    [ClientTerminationTriggers]            NVARCHAR (250)  NULL,
    [Closed]                               BIT             NULL,
    [CloseDate]                            DATETIME        NULL,
    [Comment]                              NVARCHAR (MAX)  NULL,
    [ComplianceBreachesLastYear]           INT             NULL,
    [ContactName]                          NVARCHAR (250)  NULL,
    [ContractSigningDate]                  DATETIME        NULL,
    [ContractValue]                        NVARCHAR (250)  NULL,
    [ContractValueDetails]                 NVARCHAR (MAX)  NULL,
    [CreationDate]                         DATETIME        NULL,
    [CSATScore]                            INT             NULL,
    [Currency]                             NVARCHAR (250)  NULL,
    [CurrentStageStartDate]                DATETIME        NULL,
    [DeliveryMisses]                       INT             NULL,
    [Description]                          NVARCHAR (MAX)  NULL,
    [DisentanglementClausePresent]         BIT             NULL,
    [DisentanglementTerm]                  INT             NULL,
    [DisentanglementTransitionPlan]        INT             NULL,
    [DocumentLibraryName]                  NVARCHAR (250)  NULL,
    [EffectiveEndDate]                     DATETIME        NULL,
    [EffectiveStartDate]                   DATETIME        NULL,
    [FinancialManager]                     NVARCHAR (MAX)  NULL,
    [FirmAttrition]                        INT             NULL,
    [FolderUrl]                            NVARCHAR (250)  NULL,
    [FunctionalManager]                    NVARCHAR (MAX)  NULL,
    [GrowthOverLastYear]                   INT             NULL,
    [History]                              NVARCHAR (MAX)  NULL,
    [ImpactLookup]                         BIGINT          NOT NULL,
    [InitialContractTerm]                  INT             NULL,
    [InitiatorUser]                        NVARCHAR (MAX)  NULL,
    [IsPrivate]                            BIT             NULL,
    [IssueResolutionProcessExists]         BIT             NULL,
    [IssueResolutionProcessSummary]        NVARCHAR (MAX)  NULL,
    [KeyRefUniqueID]                       NVARCHAR (250)  NULL,
    [LeadershipChurn]                      BIT             NULL,
    [LegalApprovedSubcontractors]          NVARCHAR (250)  NULL,
    [LegalCompliances]                     NVARCHAR (250)  NULL,
    [LegalJurisdiction]                    NVARCHAR (250)  NULL,
    [LegalPublicity]                       NVARCHAR (MAX)  NULL,
    [LegalWarranties]                      NVARCHAR (MAX)  NULL,
    [ModuleStepLookup]                     NVARCHAR (250)  NULL,
    [NoOfEmployee]                         INT             NULL,
    [NumberOfClient]                       INT             NULL,
    [NumberofMisses]                       INT             NULL,
    [OnHold]                               INT             NULL,
    [OnHoldReasonChoice]                   NVARCHAR (MAX)  NULL,
    [OnHoldStartDate]                      DATETIME        NULL,
    [OnHoldTillDate]                       DATETIME        NULL,
    [OtherClientsBreachesReported]         INT             NULL,
    [OtherPaymentTerms]                    NVARCHAR (MAX)  NULL,
    [OwnerUser]                            NVARCHAR (250)  NULL,
    [PaymentDelayInterest]                 NVARCHAR (MAX)  NULL,
    [PaymentDueTerm]                       INT             NULL,
    [PctComplete]                          INT             NULL,
    [PerformanceManager]                   NVARCHAR (MAX)  NULL,
    [PriorityLookup]                       BIGINT          NOT NULL,
    [Profitability]                        INT             NULL,
    [RequestorUser]                        NVARCHAR (250)  NULL,
    [RequestTypeCategory]                  NVARCHAR (250)  NULL,
    [RequestTypeLookup]                    BIGINT          NOT NULL,
    [RequiredNotifications]                NVARCHAR (MAX)  NULL,
    [ResolutionTypeChoice]                 NVARCHAR (MAX)  NULL,
    [ReSubmissionDate]                     DATETIME        NULL,
    [RiskChoice]                           NVARCHAR (MAX)  NULL,
    [RiskScore]                            INT             NULL,
    [ServiceDeliveryManager]               NVARCHAR (MAX)  NULL,
    [ServiceLookUp]                        BIGINT          NOT NULL,
    [SeverityLookup]                       BIGINT          NOT NULL,
    [StageActionUsersUser]                 NVARCHAR (250)  NULL,
    [StageActionUserTypes]                 NVARCHAR (250)  NULL,
    [StageStep]                            INT             NULL,
    [Status]                               NVARCHAR (250)  NULL,
    [TerminationByClient]                  BIT             NULL,
    [TerminationbyClientForceMajeureEvent] BIT             NULL,
    [TerminationByVendor]                  BIT             NULL,
    [TerminationChargesByVendor]           NVARCHAR (MAX)  NULL,
    [TerminationForConvenienceAllowed]     BIT             NULL,
    [TerminationForIncurredLiability]      BIT             NULL,
    [TerminationNoticePeriod]              NVARCHAR (250)  NULL,
    [TerminationNoticePeriodByVendor]      NVARCHAR (250)  NULL,
    [TerminationTriggerByVendor]           NVARCHAR (250)  NULL,
    [TicketId]                             NVARCHAR (250)  NULL,
    [TotalHoldDuration]                    INT             NULL,
    [UserQuestionSummary]                  NVARCHAR (MAX)  NULL,
    [VendorAddress]                        NVARCHAR (MAX)  NULL,
    [VendorDetailLookup]                   BIGINT          NOT NULL,
    [VendorEmail]                          NVARCHAR (250)  NULL,
    [VendorLocation]                       NVARCHAR (250)  NULL,
    [VendorName]                           NVARCHAR (250)  NULL,
    [VendorPhone]                          NVARCHAR (250)  NULL,
    [VMOApprover]                          NVARCHAR (250)  NULL,
    [WebsiteUrl]                           NVARCHAR (250)  NULL,
    [WorkflowSkipStages]                   NVARCHAR (250)  NULL,
    [YearInBusiness]                       INT             NULL,
    [Title]                                VARCHAR (250)   NULL,
    [TenantID]                             NVARCHAR (128)  NULL,
    [DataEditors]                          NVARCHAR (250)  DEFAULT ('') NULL,
    [Created]                              DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                             DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                        NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]                       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                              BIT             DEFAULT ((0)) NULL,
    [Attachments]                          NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ImpactLookup]) REFERENCES [dbo].[Config_Module_Impact] ([ID]),
    FOREIGN KEY ([PriorityLookup]) REFERENCES [dbo].[Config_Module_Priority] ([ID]),
    FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID]),
    FOREIGN KEY ([ServiceLookUp]) REFERENCES [dbo].[Config_Services] ([ID]),
    FOREIGN KEY ([SeverityLookup]) REFERENCES [dbo].[Config_Module_Severity] ([ID]),
    FOREIGN KEY ([VendorDetailLookup]) REFERENCES [dbo].[Vendors] ([ID])
);










GO

GO

GO

GO

GO

GO
CREATE NONCLUSTERED INDEX [IX_VendorMSA_PriorityLookup] ON [dbo].[VendorMSA] ([AgreementNumber], [ContactName], [KeyRefUniqueID], [ModuleStepLookup], [PriorityLookup], [RequestTypeLookup], [Status], [TicketId], [VendorName])