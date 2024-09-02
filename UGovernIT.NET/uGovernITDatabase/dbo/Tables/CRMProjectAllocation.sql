CREATE TABLE [dbo].[CRMProjectAllocation] (
    [ID]                  BIGINT         IDENTITY (1, 1) NOT NULL,
    [AllocationEndDate]   DATETIME       NULL,
    [AllocationStartDate] DATETIME       NULL,
    [AssignedToUser]      NVARCHAR (MAX) NULL,
    [Created]             DATETIME       CONSTRAINT [DF_CRMProjectAllocation_Created] DEFAULT (getdate()) NOT NULL,
    [CRMDuration]         INT            NULL,
    [Deleted]             BIT            CONSTRAINT [DF_CRMProjectAllocation_Deleted] DEFAULT ((0)) NULL,
    [ItemOrder]           INT            CONSTRAINT [DF_CRMProjectAllocation_ItemOrder] DEFAULT ((0)) NULL,
    [Modified]            DATETIME       CONSTRAINT [DF_CRMProjectAllocation_Modified] DEFAULT (getdate()) NOT NULL,
    [PctAllocation]       FLOAT (53)     NULL,
    [Title]               NVARCHAR (128) NULL,
    [Type]                NVARCHAR (128) NULL,
    [CreatedByUser]       NVARCHAR (128) NULL,
    [TenantID]            NVARCHAR (128) NULL,
    [Attachments]         NVARCHAR (500) NULL,
    [TicketID]            NVARCHAR (128) NULL,
    [ModifiedByUser]      NVARCHAR (128) NULL,
    [DataMigrationID]     INT            NULL
);


