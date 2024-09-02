CREATE TABLE [dbo].[ProjectStageHistory] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [Title]          VARCHAR (250)   NULL,
    [TaskID]         NVARCHAR (250)  NULL,
    [TicketId]       NVARCHAR (250)  NULL,
    [StageStep]      INT             NULL,
    [StartDate]      DATETIME        NULL,
    [EndDate]        DATETIME        NULL,
    [TenantID]       NVARCHAR (128)  NULL,
    [Created]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT             DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);


