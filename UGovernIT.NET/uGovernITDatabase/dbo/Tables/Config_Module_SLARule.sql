CREATE TABLE [dbo].[Config_Module_SLARule] (
    [ID]                       BIGINT          IDENTITY (1, 1) NOT NULL,
    [EndStageStep]             INT             NULL,
    [EndStageTitleLookup]      BIGINT          NOT NULL,
    [ModuleDescription]        NVARCHAR (MAX)  NULL,
    [ModuleNameLookup]         NVARCHAR (250)  NULL,
    [PriorityLookup]           BIGINT          NOT NULL,
    [SLACategoryChoice]        NVARCHAR (MAX)  NULL,
    [SLADaysRoundUpDownChoice] NVARCHAR (MAX)  NULL,
    [SLAHours]                 DECIMAL (18, 2) NULL,
    [SLATarget]                INT             NULL,
    [StageTitleLookup]         BIGINT          NOT NULL,
    [StagingId]                INT             NULL,
    [StartStageStep]           INT             NULL,
    [Title]                    VARCHAR (250)   NULL,
    [TenantID]                 NVARCHAR (128)  NULL,
    [Created]                  DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                 DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]            NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]           NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                  BIT             DEFAULT ((0)) NULL,
    [Attachments]              NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([PriorityLookup]) REFERENCES [dbo].[Config_Module_Priority] ([ID]),
    FOREIGN KEY ([ModuleNameLookup], [TenantID]) REFERENCES [dbo].[Config_Modules] ([ModuleName], [TenantID]) ON UPDATE CASCADE
);












GO

GO
CREATE NONCLUSTERED INDEX [IX_Config_Module_SLARule_ModuleNameLookup] ON [dbo].[Config_Module_SLARule] ([Deleted], [ModuleNameLookup], [PriorityLookup])