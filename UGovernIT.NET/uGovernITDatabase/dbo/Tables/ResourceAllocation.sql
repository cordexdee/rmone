﻿CREATE TABLE [dbo].[ResourceAllocation] (
    [ID]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [AllocationEndDate]      DATETIME        NULL,
    [AllocationStartDate]    DATETIME        NULL,
    [PctAllocation]          FLOAT (53)      NULL,
    [PctPlannedAllocation]   FLOAT (53)      NULL,
    [ResourceUser]           NVARCHAR (MAX)  NULL,
    [ResourceWorkItemLookup] BIGINT          NOT NULL,
    [Title]                  VARCHAR (250)   NULL,
    [PlannedStartDate]       DATETIME        NULL,
    [PlannedEndDate]         DATETIME        NULL,
    [PctEstimatedAllocation] FLOAT (53)      NULL,
    [EstStartDate]           DATETIME        NULL,
    [EstEndDate]             DATETIME        NULL,
    [TenantID]               NVARCHAR (128)  NULL,
    [Created]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]               DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]         NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                BIT             DEFAULT ((0)) NULL,
    [Attachments]            NVARCHAR (2000) DEFAULT ('') NULL,
    [ResolutionDate]         DATETIME        NULL,
    [RoleId] NVARCHAR(256) NULL, 
    [ProjectEstimatedAllocationId] NVARCHAR(100) NULL, 
    [TicketID] NVARCHAR(256) NULL,
	[NonChargeable] [bit] NULL DEFAULT ((0)),
    [SoftAllocation] [bit] NULL DEFAULT ((0)),
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ResourceWorkItemLookup]) REFERENCES [dbo].[ResourceWorkItems] ([ID])
);






GO
