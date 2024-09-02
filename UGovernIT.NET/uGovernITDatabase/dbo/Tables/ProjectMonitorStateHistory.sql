CREATE TABLE [dbo].[ProjectMonitorStateHistory] (
    [ID]                          BIGINT          IDENTITY (1, 1) NOT NULL,
    [AutoCalculate]               BIT             NULL,
    [ModuleMonitorNameLookup]     BIGINT          NOT NULL,
    [ModuleMonitorOptionIdLookup] BIGINT          NOT NULL,
    [PMMIdLookup]                 BIGINT          NOT NULL,
    [ProjectMonitorNotes]         NVARCHAR (MAX)  NULL,
    [ProjectMonitorWeight]        INT             NULL,
    [Title]                       VARCHAR (250)   NULL,
    [TicketId]                    NVARCHAR (25)   NULL,
    [ModuleNameLookup]            NVARCHAR (25)   NULL,
    [TenantID]                    NVARCHAR (128)  NULL,
    [Created]                     DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                    DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]               NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]              NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                     BIT             DEFAULT ((0)) NULL,
    [Attachments]                 NVARCHAR (2000) DEFAULT ('') NULL,
    [BaselineId]                  INT             NULL,
    [BaselineDate]                DATETIME        NULL,
    CONSTRAINT [PK__ProjectMH__3214EC279C8414E0] PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ModuleMonitorNameLookup]) REFERENCES [dbo].[Config_ModuleMonitors] ([ID]),
    CONSTRAINT [FK__ProjectMoH__Modul__23F3538A] FOREIGN KEY ([ModuleMonitorOptionIdLookup]) REFERENCES [dbo].[Config_ModuleMonitorOptions] ([ID]),
    CONSTRAINT [FK__ProjectMoH__PMMId__3449B6E4] FOREIGN KEY ([PMMIdLookup]) REFERENCES [dbo].[PMM] ([ID])
);


GO
ALTER TABLE [dbo].[ProjectMonitorStateHistory] NOCHECK CONSTRAINT [FK__ProjectMoH__Modul__23F3538A];


GO
ALTER TABLE [dbo].[ProjectMonitorStateHistory] NOCHECK CONSTRAINT [FK__ProjectMoH__PMMId__3449B6E4];




GO
ALTER TABLE [dbo].[ProjectMonitorStateHistory] NOCHECK CONSTRAINT [FK__ProjectMoH__Modul__23F3538A];


GO
ALTER TABLE [dbo].[ProjectMonitorStateHistory] NOCHECK CONSTRAINT [FK__ProjectMoH__PMMId__3449B6E4];



GO
Alter TABLE [dbo].[ProjectMonitorStateHistory] NOCHECK CONSTRAINT [FK__ProjectMoH__Modul__23F3538A];


GO
ALTER TABLE [dbo].[ProjectMonitorStateHistory] NOCHECK CONSTRAINT [FK__ProjectMoH__PMMId__3449B6E4];




GO
ALTER TABLE [dbo].[ProjectMonitorStateHistory] NOCHECK CONSTRAINT [FK__ProjectMoH__Modul__23F3538A];


GO
ALTER TABLE [dbo].[ProjectMonitorStateHistory] NOCHECK CONSTRAINT [FK__ProjectMoH__PMMId__3449B6E4];
