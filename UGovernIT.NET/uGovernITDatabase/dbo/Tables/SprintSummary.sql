﻿CREATE TABLE [dbo].[SprintSummary] (
    [ID]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [Description]        NVARCHAR (MAX)  NULL,
    [EndDate]            DATETIME        NULL,
    [ItemOrder]          INT             NULL,
    [PercentComplete]    INT             NULL,
    [PMMIdLookup]        BIGINT          NOT NULL,
    [RemainingHours]     INT             NULL,
    [SprintLookup]       BIGINT          NOT NULL,
    [StartDate]          DATETIME        NULL,
    [TaskEstimatedHours] INT             NULL,
    [Title]              VARCHAR (250)   NULL,
    [TenantID]           NVARCHAR (128)  NULL,
    [Created]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]            BIT             DEFAULT ((0)) NULL,
    [Attachments]        NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([PMMIdLookup]) REFERENCES [dbo].[PMM] ([ID]),
    FOREIGN KEY ([SprintLookup]) REFERENCES [dbo].[Sprint] ([ID])
);






GO

GO