CREATE TABLE [HangFire].[Server] (
    [Id]            NVARCHAR (100)  NOT NULL,
    [Data]          NVARCHAR (MAX)  NULL,
    [LastHeartbeat] DATETIME        NOT NULL,
    [Created]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]      DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedBy]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]       BIT             DEFAULT ((0)) NULL,
    [Attachments]   NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_HangFire_Server] PRIMARY KEY CLUSTERED ([Id] ASC)
);



