CREATE TABLE [HangFire].[Job] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [StateId]        INT             NULL,
    [StateName]      NVARCHAR (20)   NULL,
    [InvocationData] NVARCHAR (MAX)  NOT NULL,
    [Arguments]      NVARCHAR (MAX)  NOT NULL,
    [CreatedAt]      DATETIME        NOT NULL,
    [ExpireAt]       DATETIME        NULL,
    [Created]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedBy]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT             DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_HangFire_Job] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_HangFire_Job_ExpireAt]
    ON [HangFire].[Job]([ExpireAt] ASC)
    INCLUDE([Id]);


GO
CREATE NONCLUSTERED INDEX [IX_HangFire_Job_StateName]
    ON [HangFire].[Job]([StateName] ASC);

