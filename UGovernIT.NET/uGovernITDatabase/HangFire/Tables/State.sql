CREATE TABLE [HangFire].[State] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [JobId]       INT             NOT NULL,
    [Name]        NVARCHAR (20)   NOT NULL,
    [Reason]      NVARCHAR (100)  NULL,
    [CreatedAt]   DATETIME        NOT NULL,
    [Data]        NVARCHAR (MAX)  NULL,
    [Created]     DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]    DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedBy]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]     BIT             DEFAULT ((0)) NULL,
    [Attachments] NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_HangFire_State] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_HangFire_State_Job] FOREIGN KEY ([JobId]) REFERENCES [HangFire].[Job] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_HangFire_State_JobId]
    ON [HangFire].[State]([JobId] ASC);

