CREATE TABLE [dbo].[LeadRanking] (
    [ID]              BIGINT          IDENTITY (1, 1) NOT NULL,
    [RankingCriteria] NVARCHAR (250)  NULL,
    [Description]     NVARCHAR (250)  NULL,
    [ItemOrder]       INT             NULL,
    [Ranking]         INT             DEFAULT ((0)) NULL,
    [Weight]          DECIMAL (4, 1)  NULL,
    [WeightedScore]   DECIMAL (10, 1) NULL,
    [TicketId]        NVARCHAR (256)  NULL,
    [Created]         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [TenantID]        NVARCHAR (128)  NULL,
    [Deleted]         BIT             DEFAULT ((0)) NULL,
    [Attachments]     NVARCHAR (256)  DEFAULT ('') NULL,
    CONSTRAINT [PK_LeadRanking] PRIMARY KEY CLUSTERED ([ID] ASC)
);


