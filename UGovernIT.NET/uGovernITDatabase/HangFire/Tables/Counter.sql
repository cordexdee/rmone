CREATE TABLE [HangFire].[Counter] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Key]         NVARCHAR (100)  NOT NULL,
    [Value]       SMALLINT        NOT NULL,
    [ExpireAt]    DATETIME        NULL,
    [Created]     DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]    DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedBy]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]     BIT             DEFAULT ((0)) NULL,
    [Attachments] NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_HangFire_Counter] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_HangFire_Counter_Key]
    ON [HangFire].[Counter]([Key] ASC)
    INCLUDE([Value]);

