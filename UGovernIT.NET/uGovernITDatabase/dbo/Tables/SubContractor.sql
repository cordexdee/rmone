CREATE TABLE [dbo].[SubContractor] (
    [ID]                BIGINT         IDENTITY (1, 1) NOT NULL,
    [CommittmentID]     NVARCHAR (255) NULL,
    [CommittmentNumber] NVARCHAR (255) NULL,
    [CompanyName]       NVARCHAR (255) NULL,
    [Description]       NVARCHAR (MAX) NULL,
    [ExternalProjectId] NVARCHAR (255) NULL,
    [StartDate]         DATETIME       NULL,
    [Status]            NVARCHAR (255) NULL,
    [TicketID]          NVARCHAR (255) NULL,
    [Title]             NVARCHAR (255) NULL,
    [Created]           DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]     NVARCHAR (MAX) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Modified]          DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedByUser]    NVARCHAR (MAX) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Attachments]       NVARCHAR (255) NULL,
    [Deleted]           BIT            DEFAULT ((0)) NULL,
    [TenantID]          NVARCHAR (128) NULL,
    CONSTRAINT [PK_SubContractor] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO