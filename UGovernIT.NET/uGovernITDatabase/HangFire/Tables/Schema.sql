CREATE TABLE [HangFire].[Schema] (
    [Version]     INT             NOT NULL,
    [Created]     DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]    DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedBy]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]     BIT             DEFAULT ((0)) NULL,
    [Attachments] NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_HangFire_Schema] PRIMARY KEY CLUSTERED ([Version] ASC)
);



