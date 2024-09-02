CREATE TABLE [dbo].[ApplicationPassword] (
    [ID]                BIGINT          IDENTITY (1, 1) NOT NULL,
    [APPPasswordTitle]  NVARCHAR (250)  NULL,
    [APPTitleLookup]    BIGINT          NOT NULL,
    [APPUserName]       NVARCHAR (250)  NULL,
    [Description]       NVARCHAR (MAX)  NULL,
    [EncryptedPassword] NVARCHAR (250)  NULL,
    [Password]          NVARCHAR (250)  NULL,
    [Title]             VARCHAR (250)   NULL,
    [TenantID]          NVARCHAR (128)  NULL,
    [Created]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]           BIT             DEFAULT ((0)) NULL,
    [Attachments]       NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([APPTitleLookup]) REFERENCES [dbo].[Applications] ([ID]),
    FOREIGN KEY ([APPTitleLookup]) REFERENCES [dbo].[Applications] ([ID])
);






GO

GO
CREATE NONCLUSTERED INDEX [IX_ApplicationPassword_EncryptedPassword] ON [dbo].[ApplicationPassword] ([EncryptedPassword])

GO
CREATE NONCLUSTERED INDEX [IX_ApplicationPassword_Password] ON [dbo].[ApplicationPassword] ([Password])