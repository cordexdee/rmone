CREATE TABLE [dbo].[Phrase] (
    [PhraseId]       BIGINT          IDENTITY (1, 1) NOT NULL,
    [Phrase]         NVARCHAR (256)  NOT NULL,
    [TenantId]       NVARCHAR (128)  NOT NULL,
    [Deleted]        BIT             CONSTRAINT [DF_Phrase_Deleted] DEFAULT ((0)) NOT NULL,
    [Created]        DATETIME        CONSTRAINT [DF_Phrase_Created] DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME        CONSTRAINT [DF_Phrase_Modified] DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  CONSTRAINT [DF_Phrase_CreatedBy] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  CONSTRAINT [DF_Phrase_ModifiedBy] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Attachments]    NVARCHAR (2000) NULL,
    [AgentType]      INT             DEFAULT ((0)) NULL,
    [TicketType]     NVARCHAR (256)  NULL,
    [RequestType]    BIGINT          DEFAULT ((0)) NULL,
    [Services]       BIGINT          NULL,
	[WikiLookUp]     NVARCHAR (MAX)  NULL,
	[HelpCardLookUp]     NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_Phrase_PhraseId] PRIMARY KEY CLUSTERED ([PhraseId] ASC)
);


GO
