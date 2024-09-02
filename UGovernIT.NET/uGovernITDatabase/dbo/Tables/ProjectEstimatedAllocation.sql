CREATE TABLE [dbo].[ProjectEstimatedAllocation] (
    [ID]                  BIGINT         IDENTITY (1, 1) NOT NULL,
    [AllocationEndDate]   DATETIME       NULL,
    [AllocationStartDate] DATETIME       NULL,
    [AssignedToUser]      NVARCHAR (MAX) NULL,
    [Created]             DATETIME       DEFAULT (getdate()) NOT NULL,
    [Duration]            INT            NULL,
    [Deleted]             BIT            DEFAULT ((0)) NULL,
    [ItemOrder]           INT            DEFAULT ((0)) NULL,
    [Modified]            DATETIME       DEFAULT (getdate()) NOT NULL,
    [PctAllocation]       FLOAT (53)     NULL,
    [Title]               NVARCHAR (128) NULL,
    [Type]                NVARCHAR (128) NULL,
    [CreatedByUser]       NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NULL,
    [TenantID]            NVARCHAR (128) NULL,
    [Attachments]         NVARCHAR (500) NULL,
    [TicketID]            NVARCHAR (128) NULL,
    [ModifiedByUser]      NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
	[NonChargeable] [bit] NULL DEFAULT ((0)),
    [SoftAllocation] [bit] NULL DEFAULT ((0)),
);



