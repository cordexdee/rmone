CREATE TABLE [dbo].[TicketCountTrends] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [ModuleName]     NVARCHAR (250)  NULL,
    [EndOfDay]       DATETIME        NULL,
    [NumCreated]     INT             DEFAULT ((0)) NOT NULL,
    [NumResolved]    INT             DEFAULT ((0)) NOT NULL,
    [NumClosed]      INT             DEFAULT ((0)) NOT NULL,
    [TotalActive]    INT             DEFAULT ((0)) NOT NULL,
    [TotalOnHold]    INT             DEFAULT ((0)) NOT NULL,
    [TotalResolved]  INT             DEFAULT ((0)) NOT NULL,
    [TotalClosed]    INT             DEFAULT ((0)) NOT NULL,
    [TenantID]       NVARCHAR (128)  NULL,
    [Created]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT             DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);


