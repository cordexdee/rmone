CREATE TABLE [dbo].[Config_Module_ModuleStages] (
    [ID]                        BIGINT          IDENTITY (1, 1) NOT NULL,
    [Action]                    NVARCHAR (250)  NULL,
    [ActionUser]                NVARCHAR (250)  NULL,
    [AllowReassignFromList]     BIT             NULL,
    [ApproveActionDescription]  NVARCHAR (250)  NULL,
    [ApproveButtonTooltip]      NVARCHAR (250)  NULL,
    [ApproveIcon]               NVARCHAR (250)  NULL,
    [CustomProperties]          NVARCHAR (MAX)  NULL,
    [EnableCustomReturn]        BIT             NULL,
    [ModuleNameLookup]          NVARCHAR (250)  NULL,
    [Description]               NVARCHAR (250)  NULL,
    [DisableAutoApprove]        BIT             NULL,
    [StageStep]                 INT             NULL,
    [RejectActionDescription]   NVARCHAR (250)  NULL,
    [RejectButtonTooltip]       NVARCHAR (250)  NULL,
    [RejectIcon]                NVARCHAR (250)  NULL,
    [ReturnActionDescription]   NVARCHAR (250)  NULL,
    [ReturnButtonTooltip]       NVARCHAR (250)  NULL,
    [ReturnIcon]                NVARCHAR (250)  NULL,
    [SelectedTabNumber]         INT             NULL,
    [ShortStageTitle]           NVARCHAR (250)  NULL,
    [ShowBaselineButtons]       BIT             NULL,
    [SkipOnCondition]           NVARCHAR (MAX)  NULL,
    [StageAllApprovalsRequired] BIT             DEFAULT ((0)) NOT NULL,
    [StageApproveButtonName]    NVARCHAR (250)  NULL,
    [StageApprovedStatus]       INT             DEFAULT ((0)) NOT NULL,
    [StageCapacityMax]          INT             NULL,
    [StageCapacityNormal]       INT             NULL,
    [StageRejectedButtonName]   NVARCHAR (250)  NULL,
    [StageRejectedStatus]       INT             DEFAULT ((0)) NOT NULL,
    [StageReturnButtonName]     NVARCHAR (250)  NULL,
    [StageReturnStatus]         INT             DEFAULT ((0)) NOT NULL,
    [StageTitle]                NVARCHAR (250)  NULL,
    [StageTypeChoice]           NVARCHAR (250)  NULL,
    [StageWeight]               FLOAT (53)      NULL,
    [UserPrompt]                NVARCHAR (250)  NULL,
    [UserWorkflowStatus]        NVARCHAR (250)  NULL,
    [Title]                     NVARCHAR (250)  NULL,
    [Name]                      NVARCHAR (250)  NULL,
    [LifeCycleName]             BIGINT          NULL,
    [TenantID]                  NVARCHAR (128)  NULL,
    [DataEditors]               NVARCHAR (250)  DEFAULT ('') NOT NULL,
    [Created]                   DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                  DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]             NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]            NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                   BIT             DEFAULT ((0)) NULL,
    [Attachments]               NVARCHAR (2000) DEFAULT ('') NULL,
    [AutoApproveOnStageTasks]   BIT             DEFAULT ((0)) NOT NULL,
    [NavigationUrl]    		NVARCHAR (max)  NULL,
    [NavigationType]    	NVARCHAR (250)  NULL,
    [IconUrl] 			NVARCHAR(MAX) NULL, 
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ModuleNameLookup], [TenantID]) REFERENCES [dbo].[Config_Modules] ([ModuleName], [TenantID]) ON UPDATE CASCADE,
    CONSTRAINT [FK_Config_Module_ModuleStages_Lifecycles] FOREIGN KEY ([LifeCycleName]) REFERENCES [dbo].[Config_ModuleLifeCycles] ([ID])
);










GO

GO
CREATE NONCLUSTERED INDEX [IX_Config_Config_Module_ModuleStages_ModuleNameLookup] ON [dbo].[Config_Module_ModuleStages] ([ModuleNameLookup])

GO
CREATE NONCLUSTERED INDEX [IX_Config_Config_Module_ModuleStages_Deleted] ON [dbo].[Config_Module_ModuleStages] ([Deleted])