﻿CREATE TABLE [dbo].[Investors] (
    [ID]                           BIGINT          IDENTITY (1, 1) NOT NULL,
    [AccountType]                  NVARCHAR (MAX)  NULL,
    [ActualCompletionDate]         DATETIME        NULL,
    [ActualStartDate]              DATETIME        NULL,
    [AddedDate]                    DATETIME        NULL,
    [BusinessManagerUser]          NVARCHAR (MAX)  NULL,
    [Closed]                       BIT             NULL,
    [CloseDate]                    DATETIME        NULL,
    [Comment]                      NVARCHAR (MAX)  NULL,
    [Contact]                      NVARCHAR (250)  NULL,
    [CreationDate]                 DATETIME        NULL,
    [CurrentStageStartDate]        DATETIME        NULL,
    [Custodian]                    NVARCHAR (250)  NULL,
    [Description]                  NVARCHAR (MAX)  NULL,
    [DesiredCompletionDate]        DATETIME        NULL,
    [EmailAddress]                 NVARCHAR (250)  NULL,
    [EmployerIdentificationNumber] INT             NULL,
    [FinanceManagerUser]           NVARCHAR (MAX)  NULL,
    [FirstName]                    NVARCHAR (250)  NULL,
    [History]                      NVARCHAR (MAX)  NULL,
    [InitiatorUser]                NVARCHAR (MAX)  NULL,
    [InvestorID]                   INT             NULL,
    [InvestorName]                 NVARCHAR (250)  NULL,
    [InvestorShortName]            NVARCHAR (250)  NULL,
    [InvestorStatus]               NVARCHAR (MAX)  NULL,
    [IsPrivate]                    BIT             NULL,
    [LastName]                     NVARCHAR (250)  NULL,
    [LegalUser]                    NVARCHAR (MAX)  NULL,
    [ModuleStepLookup]             NVARCHAR (250)  NULL,
    [OnHold]                       INT             NULL,
    [OnHoldStartDate]              DATETIME        NULL,
    [OtherAddress]                 NVARCHAR (MAX)  NULL,
    [OwnerUser]                    NVARCHAR (250)  NULL,
    [PctComplete]                  INT             NULL,
    [PriorityLookup]               BIGINT          NOT NULL,
    [PurchasingUser]               NVARCHAR (MAX)  NULL,
    [RequestorUser]                NVARCHAR (250)  NULL,
    [RequestSourceChoice]          NVARCHAR (MAX)  NULL,
    [RequestTypeCategory]          NVARCHAR (250)  NULL,
    [RequestTypeLookup]            BIGINT          NOT NULL,
    [ResolutionComments]           NVARCHAR (MAX)  NULL,
    [Responsible]                  NVARCHAR (MAX)  NULL,
    [ReSubmissionDate]             DATETIME        NULL,
    [StageActionUsersUser]         NVARCHAR (250)  NULL,
    [StageActionUserTypes]         NVARCHAR (250)  NULL,
    [StageStep]                    INT             NULL,
    [State]                        NVARCHAR (250)  NULL,
    [Status]                       NVARCHAR (250)  NULL,
    [StatusChanged]                INT             NULL,
    [StreetAddress]                NVARCHAR (MAX)  NULL,
    [TargetCompletionDate]         DATETIME        NULL,
    [TargetStartDate]              DATETIME        NULL,
    [TicketId]                     NVARCHAR (250)  NULL,
    [Title]                        NVARCHAR (250)  NULL,
    [TotalHoldDuration]            INT             NULL,
    [UpdateDate]                   DATETIME        NULL,
    [WorkCity]                     NVARCHAR (250)  NULL,
    [WorkZip]                      NVARCHAR (250)  NULL,
    [TenantID]                     NVARCHAR (128)  NULL,
    [Created]                      DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                     DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]               NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                      BIT             DEFAULT ((0)) NULL,
    [Attachments]                  NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([PriorityLookup]) REFERENCES [dbo].[Config_Module_Priority] ([ID]),
    FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID])
);








GO

GO
