CREATE TABLE [dbo].[Config_Module_StatusMapping] (
    [ID]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [GenericStatusLookup] INT             NOT NULL,
    [ModuleNameLookup]    NVARCHAR (250)  NULL,
    [StageTitleLookup]    INT             NOT NULL,
    [Title]               VARCHAR (250)   NULL,
    [TenantID]            NVARCHAR (128)  NULL,
    [Created]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]             BIT             DEFAULT ((0)) NULL,
    [Attachments]         NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([GenericStatusLookup]) REFERENCES [dbo].[GenericStatus] ([ID]) ON UPDATE CASCADE,
    FOREIGN KEY ([ModuleNameLookup], [TenantID]) REFERENCES [dbo].[Config_Modules] ([ModuleName], [TenantID]) ON UPDATE CASCADE
);










GO

GO

GO
CREATE NONCLUSTERED INDEX [IX_Config_Module_StatusMapping_ModuleNameLookup] ON [dbo].[Config_Module_StatusMapping] ([ModuleNameLookup])