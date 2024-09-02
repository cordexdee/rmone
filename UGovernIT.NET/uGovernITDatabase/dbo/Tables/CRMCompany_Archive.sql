﻿CREATE TABLE [dbo].[CRMCompany_Archive] (
    [ID]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [PctComplete]            INT             NULL,
    [StageActionUserTypes]   NVARCHAR (250)  NULL,
    [StageActionUsersUser]   NVARCHAR (250)  NULL,
    [AdditionalInfo]         NVARCHAR (250)  NULL,
    [Address]                NVARCHAR (MAX)  NULL,
    [AnnualRevenues]         NVARCHAR (250)  NULL,
    [CategoryLookup]         BIGINT          NULL,
    [CertificationsChoice]   NVARCHAR (250)  NULL,
    [City]                   NVARCHAR (250)  NULL,
    [CloseDate]              DATETIME        NULL,
    [Comment]                NVARCHAR (MAX)  NULL,
    [CompanyLookup]          BIGINT          NULL,
    [ContactLookup]          NVARCHAR (1000) NULL,
    [ContractorLicense]      NVARCHAR (250)  NULL,
    [Country]                NVARCHAR (250)  NULL,
    [CurrentStageStartDate]  DATETIME        NULL,
    [DataEditors]            NVARCHAR (250)  NULL,
    [DepartmentLookup]       BIGINT          NULL,
    [Description]            NVARCHAR (MAX)  NULL,
    [DesiredCompletionDate]  DATETIME        NULL,
    [DivisionChoice]         NVARCHAR (250)  NULL,
    [DocumentLibraryName]    NVARCHAR (250)  NULL,
    [DueDate]                DATETIME        NULL,
    [EmailAddress]           NVARCHAR (250)  NULL,
    [Fax]                    NVARCHAR (250)  NULL,
    [FederalID]              NVARCHAR (250)  NULL,
    [FunctionalAreaLookup]   BIGINT          NULL,
    [History]                NVARCHAR (MAX)  NULL,
    [ImpactLookup]           BIGINT          NULL,
    [InitiatorUser]          NVARCHAR (MAX)  NULL,
    [IsPrivate]              BIT             NULL,
    [LegalName]              NVARCHAR (250)  NULL,
    [ManagerApprovalNeeded]  BIT             NULL,
    [ModuleStepLookup]       BIGINT          NULL,
    [OnHoldReasonChoice]     NVARCHAR (250)  NULL,
    [OnHoldStartDate]        DATETIME        NULL,
    [OnHoldTillDate]         DATETIME        NULL,
    [ORPUser]                NVARCHAR (MAX)  NULL,
    [OwnerUser]              NVARCHAR (MAX)  NULL,
    [OwnershipTypeChoice]    NVARCHAR (250)  NULL,
    [PriorityLookup]         BIGINT          NULL,
    [ProcoreId]              NVARCHAR (250)  NULL,
    [PRPUser]                NVARCHAR (MAX)  NULL,
    [PRPGroupUser]           NVARCHAR (MAX)  NULL,
    [RegionsofWorkChoice]    NVARCHAR (250)  NULL,
    [RelationshipType]       NVARCHAR (250)  NULL,
    [RelationshipTypeLookup] NVARCHAR (250)  NULL,
    [RequestTypeLookup]      BIGINT          NULL,
    [ResolutionTypeChoice]   NVARCHAR (250)  NULL,
    [ReSubmissionDate]       DATETIME        NULL,
    [ServiceLookup]          BIGINT          NULL,
    [ShortName]              NVARCHAR (250)  NULL,
    [SICCode]                NVARCHAR (250)  NULL,
    [StageStep]              INT             NULL,
    [StartDate]              DATETIME        NULL,
    [State]                  NVARCHAR (250)  NULL,
    [StateLookup]            BIGINT          NULL,
    [Status]                 NVARCHAR (250)  NULL,
    [StatusChanged]          INT             NULL,
    [StreetAddress1]         NVARCHAR (250)  NULL,
    [StreetAddress2]         NVARCHAR (250)  NULL,
    [TargetCompletionDate]   DATETIME        NULL,
    [TargetStartDate]        DATETIME        NULL,
    [Telephone]              NVARCHAR (250)  NULL,
    [Closed]                 BIT             NULL,
    [Created]                DATETIME        DEFAULT (getdate()) NULL,
    [TicketId]               NVARCHAR (250)  NULL,
    [OnHold]                 INT             NULL,
    [Title]                  NVARCHAR (250)  NULL,
    [TotalHoldDuration]      INT             NULL,
    [TypesofWorkChoice]      NVARCHAR (250)  NULL,
    [UnionAffiliation]       BIT             NULL,
    [WebsiteUrl]             NVARCHAR (500)  NULL,
    [WorkflowSkipStages]     NVARCHAR (250)  NULL,
    [Zip]                    NVARCHAR (250)  NULL,
    [CreatedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NULL,
    [Modified]               DATETIME        DEFAULT (getdate()) NULL,
    [ModifiedByUser]         NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NULL,
    [TenantID]               NVARCHAR (128)  NULL,
    [Deleted]                BIT             DEFAULT ((0)) NULL,
    [Attachments]            NVARCHAR (MAX)  NULL,
    [CreationDate]           DATETIME        NULL,
    [MasterAgreementChoice]  BIT             DEFAULT ((0)) NULL,
	[ExternalID]			 NVARCHAR (MAX)  NULL,
    [ReopenCount] INT NULL, 
    [ApprovalDate] DATETIME NULL, 
    [ApprovedByUser] VARCHAR(128) NULL, 
    CONSTRAINT [PK_CRMCompany_Archive] PRIMARY KEY CLUSTERED ([ID] ASC)
);





