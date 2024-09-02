CREATE TABLE [dbo].[Config_Module_Priority] (
    [ID]                BIGINT          IDENTITY (1, 1) NOT NULL,
    [EmailIDTo]         NVARCHAR (MAX)  NULL,
    [IsVIP]             BIT             NULL,
    [ItemOrder]         INT             NULL,
    [ModuleNameLookup]  NVARCHAR (250)  NULL,
    [uPriority]         NVARCHAR (250)  NULL,
    [Title]             VARCHAR (250)   NULL,
    [NotifyInPlainText] BIT             DEFAULT ((0)) NOT NULL,
    [Color]             NVARCHAR (50)   NULL,
    [TenantID]          NVARCHAR (128)  NULL,
    [Created]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]           BIT             DEFAULT ((0)) NULL,
    [Attachments]       NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_Config_Module_Priority] PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ModuleNameLookup], [TenantID]) REFERENCES [dbo].[Config_Modules] ([ModuleName], [TenantID]) ON UPDATE CASCADE
);










GO
CREATE NONCLUSTERED INDEX [IX_Config_Module_Priority_ModuleNameLookup] ON [dbo].[Config_Module_Priority] ([ModuleNameLookup])