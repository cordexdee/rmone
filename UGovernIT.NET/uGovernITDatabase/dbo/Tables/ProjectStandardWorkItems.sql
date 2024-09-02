﻿CREATE TABLE [dbo].[ProjectStandardWorkItems] (
    [ID]                   INT             IDENTITY (1, 1) NOT NULL,
    [ItemOrder]            NVARCHAR (50)   NOT NULL,
    [BudgetCategoryLookup] INT             NULL,
    [Title]                NVARCHAR (250)  NULL,
    [TenantID]             NVARCHAR (128)  NULL,
    [Created]              DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]        NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]              BIT             DEFAULT ((0)) NULL,
    [Attachments]          NVARCHAR (2000) DEFAULT ('') NULL,
    [Code]                 NVARCHAR(100) NULL, 
    [Description] NVARCHAR(200) NULL, 
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

