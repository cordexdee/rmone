CREATE TABLE [dbo].[ModuleUserStatistics] (
    [ID]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [IsActionUser]     BIT             CONSTRAINT [DF_ModuleUserStatistics1_IsActionUser] DEFAULT ((0)) NULL,
    [ModuleNameLookup] NVARCHAR (10)   CONSTRAINT [DF_ModuleUserStatistics1_ModuleName] DEFAULT ('') NULL,
    [TicketId]         NVARCHAR (50)   CONSTRAINT [DF_ModuleUserStatistics1_TicketId] DEFAULT ('') NULL,
    [UserRole]         NVARCHAR (100)  CONSTRAINT [DF_ModuleUserStatistics1_UserRole] DEFAULT ('') NULL,
    [Title]            NVARCHAR (250)  CONSTRAINT [DF_ModuleUserStatistics1_Title] DEFAULT ('') NULL,
    [UserName]         NVARCHAR (250)  NULL,
    [TenantID]         NVARCHAR (128)  NULL,
    [Created]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]         DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]          BIT             DEFAULT ((0)) NULL,
    [Attachments]      NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_ModuleUserStatistics1] PRIMARY KEY CLUSTERED ([ID] ASC)
);








GO

GO

GO

GO

GO
CREATE NONCLUSTERED INDEX [IX_ModuleUserStatistics_TicketId] ON [dbo].[ModuleUserStatistics] ([TicketId])