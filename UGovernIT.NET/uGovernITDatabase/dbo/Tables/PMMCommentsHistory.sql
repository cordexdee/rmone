CREATE TABLE [dbo].[PMMCommentsHistory] (
    [ID]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [AccomplishmentDate] DATETIME        NULL,
    [BaselineDate]       DATETIME        NULL,
    [BaselineId]         INT             NULL,
    [EndDate]            DATETIME        NULL,
    [PMMIdLookup]        BIGINT          NOT NULL,
    [ProjectNote]        NVARCHAR (MAX)  NULL,
    [ProjectNoteType]    NVARCHAR (MAX)  NULL,
    [Title]              VARCHAR (250)   NULL,
    [TicketID]           VARCHAR (30)    NULL,
    [TenantID]           NVARCHAR (128)  NULL,
    [Created]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]            BIT             DEFAULT ((0)) NULL,
    [Attachments]        NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([PMMIdLookup]) REFERENCES [dbo].[PMM] ([ID])
);






GO
