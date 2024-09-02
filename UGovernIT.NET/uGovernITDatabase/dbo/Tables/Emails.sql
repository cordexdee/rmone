CREATE TABLE [dbo].[Emails] (
    [ID]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [EmailIDCC]           NVARCHAR (MAX)  NULL,
    [EmailIDFrom]         NVARCHAR (MAX)  NULL,
    [EmailIDTo]           NVARCHAR (MAX)  NULL,
    [EmailReplyTo]        NVARCHAR (MAX)  NULL,
    [EscalationEmailBody] NVARCHAR (MAX)  NULL,
    [IsIncomingMail]      BIT             NULL,
    [MailSubject]         NVARCHAR (250)  NULL,
    [MessageId]           NVARCHAR (250)  NULL,
    [ModuleNameLookup]    NVARCHAR (250)  NULL,
    [TicketId]            NVARCHAR (250)  NULL,
    [Title]               VARCHAR (250)   NULL,
    [EmailStatus]         NVARCHAR (50)   NULL,
    [TenantID]            NVARCHAR (128)  NULL,
    [Created]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]             BIT             DEFAULT ((0)) NULL,
    [Attachments]         NVARCHAR (2000) DEFAULT ('') NULL, 
    [EmailError] NVARCHAR(MAX) NULL
);




GO
CREATE NONCLUSTERED INDEX [IX_Emails_IsIncomingMail] ON [dbo].[Emails] ([IsIncomingMail])


GO
CREATE NONCLUSTERED INDEX [IX_Emails_ModuleNameLookup] ON [dbo].[Emails] ([ModuleNameLookup])


GO
CREATE NONCLUSTERED INDEX [IX_Emails_TicketId] ON [dbo].[Emails] ([TicketId])