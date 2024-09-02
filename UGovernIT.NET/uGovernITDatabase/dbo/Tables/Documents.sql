CREATE TABLE [dbo].[Documents] (
    [Id]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [FileID]         NVARCHAR (128)  NOT NULL,
    [Name]           NVARCHAR (100)  NOT NULL,
    [Extension]      NVARCHAR (10)   NOT NULL,
    [ContentType]    NVARCHAR (100)  NOT NULL,
    [Size]           BIGINT          NOT NULL,
    [Blob]           VARBINARY (MAX) NOT NULL,
    [TenantID]       NVARCHAR (128)  NULL,
    [Created]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT             DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);




