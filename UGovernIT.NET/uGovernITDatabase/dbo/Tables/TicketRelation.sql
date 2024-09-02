CREATE TABLE [dbo].[TicketRelation] (
    [Id]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [ParentModuleName] VARCHAR (5)     NOT NULL,
    [ParentTicketID]   NVARCHAR (20)   NOT NULL,
    [ChildModuleName]  VARCHAR (5)     NOT NULL,
    [ChildTicketID]    NVARCHAR (20)   NOT NULL,
    [TenantID]         NVARCHAR (128)  NULL,
    [Created]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]         DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]          BIT             DEFAULT ((0)) NULL,
    [Attachments]      NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);



GO
CREATE NONCLUSTERED INDEX [IX_TicketRelation_ChildTicketID] ON [dbo].[TicketRelation] ([ChildTicketID])

GO
CREATE NONCLUSTERED INDEX [IX_TicketRelation_Deleted] ON [dbo].[TicketRelation] ([Deleted])

GO
CREATE NONCLUSTERED INDEX [IX_TicketRelation_ParentModuleName] ON [dbo].[TicketRelation] ([ParentModuleName])

GO
CREATE NONCLUSTERED INDEX [IX_TicketRelation_ChildModuleName] ON [dbo].[TicketRelation] ([ChildModuleName])

GO
CREATE NONCLUSTERED INDEX [IX_TicketRelation_ParentTicketID] ON [dbo].[TicketRelation] ([ParentTicketID])


