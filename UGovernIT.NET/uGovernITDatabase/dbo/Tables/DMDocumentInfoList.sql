﻿CREATE TABLE [dbo].[DMDocumentInfoList] (
    [ID]                      BIGINT          IDENTITY (1, 1) NOT NULL,
    [ApprovedVersion]         NVARCHAR (250)  NULL,
    [Authors]                 NVARCHAR (250)  NULL,
    [CurrentApprover]         NVARCHAR (250)  NULL,
    [DepartmentNameLookup]    BIGINT          NOT NULL,
    [DMDocType1]              NVARCHAR (250)  NULL,
    [DMDocType2]              NVARCHAR (250)  NULL,
    [DMDocType3]              NVARCHAR (250)  NULL,
    [DMDocType4]              NVARCHAR (250)  NULL,
    [DMDocType5]              NVARCHAR (250)  NULL,
    [DMDocumentStatus]        NVARCHAR (250)  NULL,
    [DMVendorLookup]          BIGINT          NOT NULL,
    [DocumentComments]        NVARCHAR (MAX)  NULL,
    [DocumentControlID]       NVARCHAR (250)  NULL,
    [DocumentID]              NVARCHAR (250)  NULL,
    [DocumentTypeLookup]      BIGINT          NOT NULL,
    [DocVersion]              NVARCHAR (250)  NULL,
    [ExpirationDate]          DATETIME        NULL,
    [FileName]                NVARCHAR (250)  NULL,
    [FolderID]                NVARCHAR (250)  NULL,
    [FolderName]              NVARCHAR (250)  NULL,
    [KeepDocsAlive]           NVARCHAR (250)  NULL,
    [KeepNYear]               NVARCHAR (250)  NULL,
    [NotifyOnReviewComplete]  NVARCHAR (250)  NULL,
    [NotifyUserOnReviewStart] NVARCHAR (250)  NULL,
    [NumOfReviewCycle]        INT             NULL,
    [NumPings]                NVARCHAR (250)  NULL,
    [OverrideReaders]         BIT             NULL,
    [PortalId]                NVARCHAR (250)  NULL,
    [ProjectLookup]           BIGINT          NOT NULL,
    [Readers]                 NVARCHAR (250)  NULL,
    [ReviewCycle1]            NVARCHAR (250)  NULL,
    [ReviewCycle10]           NVARCHAR (250)  NULL,
    [ReviewCycle2]            NVARCHAR (250)  NULL,
    [ReviewCycle3]            NVARCHAR (250)  NULL,
    [ReviewCycle4]            NVARCHAR (250)  NULL,
    [ReviewCycle5]            NVARCHAR (250)  NULL,
    [ReviewCycle6]            NVARCHAR (250)  NULL,
    [ReviewCycle7]            NVARCHAR (250)  NULL,
    [ReviewCycle8]            NVARCHAR (250)  NULL,
    [ReviewCycle9]            NVARCHAR (250)  NULL,
    [ReviewRequired]          BIT             NULL,
    [ReviewStep]              INT             NULL,
    [Tags]                    NVARCHAR (250)  NULL,
    [UVersion]                NVARCHAR (250)  NULL,
    [Title]                   VARCHAR (250)   NULL,
    [TenantID]                NVARCHAR (128)  NULL,
    [Created]                 DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]           NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                 BIT             DEFAULT ((0)) NULL,
    [Attachments]             NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([DMVendorLookup]) REFERENCES [dbo].[Vendors] ([ID])
);






GO
