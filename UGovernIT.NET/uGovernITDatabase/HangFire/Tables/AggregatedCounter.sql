CREATE TABLE [HangFire].[AggregatedCounter] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Key]         NVARCHAR (100)  NOT NULL,
    [Value]       BIGINT          NOT NULL,
    [ExpireAt]    DATETIME        NULL,
    [Created]     DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]    DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedBy]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]     BIT             DEFAULT ((0)) NULL,
    [Attachments] NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_HangFire_CounterAggregated] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_HangFire_CounterAggregated_Key]
    ON [HangFire].[AggregatedCounter]([Key] ASC)
    INCLUDE([Value]);

