CREATE TABLE [dbo].[CRMEstimate] (
    [Id]               BIGINT         NOT NULL,
    [Created]          DATETIME       NOT NULL,
    [Description]      NVARCHAR (MAX) NULL,
    [EstimateCategory] NVARCHAR (255) NULL,
    [EstimatedAmount]  NVARCHAR (MAX) NULL,
    [ItemOrder]        INT            NULL,
    [Modified]         DATETIME       NOT NULL,
    [TicketId]         NVARCHAR (128) NULL,
    [ModifiedByUser]   NVARCHAR (128) NULL,
    [Title]            NVARCHAR (128) NULL,
    [EstimateType]     NVARCHAR (255) NULL,
    [CreatedByUser]    DATETIME       NULL,
    [IsDeleted]        BIT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


