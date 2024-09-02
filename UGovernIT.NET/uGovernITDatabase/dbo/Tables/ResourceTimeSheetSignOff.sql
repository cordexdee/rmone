CREATE TABLE [dbo].[ResourceTimeSheetSignOff] (
    [ID]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [EndDate]        DATETIME       NULL,
    [StartDate]      DATETIME       NULL,
    [SignOffStatus]  NVARCHAR (128) NULL,
    [ResourceUser]   NVARCHAR (MAX) NULL,
    [Title]          VARCHAR (250)  NULL,
    [History]        VARCHAR (MAX)  NULL,
    [TenantID]       NVARCHAR (128) NULL,
    [Created]        DATETIME       DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT            DEFAULT ((0)) NULL, 
    [Attachments] NVARCHAR(500) NULL
);


