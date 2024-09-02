CREATE TABLE [HangFire].[Set] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Key]         NVARCHAR (100)  NOT NULL,
    [Score]       FLOAT (53)      NOT NULL,
    [Value]       NVARCHAR (256)  NOT NULL,
    [ExpireAt]    DATETIME        NULL,
    [Created]     DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]    DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedBy]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]     BIT             DEFAULT ((0)) NULL,
    [Attachments] NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_HangFire_Set] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_HangFire_Set_Key]
    ON [HangFire].[Set]([Key] ASC)
    INCLUDE([ExpireAt], [Value]);


GO
CREATE NONCLUSTERED INDEX [IX_HangFire_Set_ExpireAt]
    ON [HangFire].[Set]([ExpireAt] ASC)
    INCLUDE([Id]);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_HangFire_Set_KeyAndValue]
    ON [HangFire].[Set]([Key] ASC, [Value] ASC);

