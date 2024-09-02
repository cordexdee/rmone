CREATE TABLE [HangFire].[List] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Key]         NVARCHAR (100)  NOT NULL,
    [Value]       NVARCHAR (MAX)  NULL,
    [ExpireAt]    DATETIME        NULL,
    [Created]     DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]    DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedBy]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]     BIT             DEFAULT ((0)) NULL,
    [Attachments] NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_HangFire_List] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_HangFire_List_Key]
    ON [HangFire].[List]([Key] ASC)
    INCLUDE([ExpireAt], [Value]);


GO
CREATE NONCLUSTERED INDEX [IX_HangFire_List_ExpireAt]
    ON [HangFire].[List]([ExpireAt] ASC)
    INCLUDE([Id]);

