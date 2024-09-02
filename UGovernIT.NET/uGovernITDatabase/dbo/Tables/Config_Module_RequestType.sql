CREATE TABLE [dbo].[Config_Module_RequestType] (
    [ID]                          BIGINT          IDENTITY (1, 1) NOT NULL,
    [ApplicationModulesLookup]    NVARCHAR (50)   NULL,
    [AppReferenceInfo]            NVARCHAR (250)  NULL,
    [APPTitleLookup]              BIGINT          NULL,
    [AssignmentSLA]               INT             NULL,
    [AutoAssignPRP]               BIT             NULL,
    [BudgetIdLookup]              BIGINT          NULL,
    [Category]                    NVARCHAR (250)  NULL,
    [CloseSLA]                    INT             NULL,
    [EstimatedHours]              FLOAT (53)      NULL,
    [FunctionalAreaLookup]        BIGINT          NULL,
    [IssueTypeOptions]            NVARCHAR (MAX)  NULL,
    [KeyWords]                    NVARCHAR (MAX)  NULL,
    [ModuleNameLookup]            NVARCHAR (250)  NULL,
    [ORPUser]                     NVARCHAR (MAX)  NULL,
    [OutOfOffice]                 BIT             DEFAULT ((0)) NULL,
    [PRPGroupUser]                NVARCHAR (MAX)  NULL,
    [RequestCategory]             NVARCHAR (MAX)  NULL,
    [RequestorContactSLA]         INT             NULL,
    [RequestType]                 NVARCHAR (250)  NULL,
    [BackupEscalationManagerUser] NVARCHAR (MAX)  NULL,
    [Description]                 NVARCHAR (MAX)  NULL,
    [EscalationManagerUser]       NVARCHAR (MAX)  NULL,
    [OwnerUser]                   NVARCHAR (250)  NULL,
    [SubCategory]                 NVARCHAR (250)  NULL,
    [ResolutionSLA]               INT             NULL,
    [ResolutionTypes]             NVARCHAR (MAX)  NULL,
    [ServiceWizardOnly]           BIT             NULL,
    [SortToBottom]                BIT             NULL,
    [StagingId]                   INT             NULL,
    [TaskTemplateLookup]          BIGINT          NULL,
    [WorkflowType]                NVARCHAR (250)  NULL,
    [Title]                       VARCHAR (250)   NULL,
    [EmailToTicketSender]         NVARCHAR (MAX)  NULL,
    [TenantID]                    NVARCHAR (128)  NULL,
    [Created]                     DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                    DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]               NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]              NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                     BIT             DEFAULT ((0)) NULL,
    [Attachments]                 NVARCHAR (2000) DEFAULT ('') NULL,
    [SLADisabled]                 BIT             DEFAULT ((0)) NOT NULL,
    [Use24x7Calendar]             BIT             DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([APPTitleLookup]) REFERENCES [dbo].[Applications] ([ID]),
    FOREIGN KEY ([FunctionalAreaLookup]) REFERENCES [dbo].[FunctionalAreas] ([ID]),
    FOREIGN KEY ([TaskTemplateLookup]) REFERENCES [dbo].[TaskTemplates] ([ID]),
    FOREIGN KEY ([ModuleNameLookup], [TenantID]) REFERENCES [dbo].[Config_Modules] ([ModuleName], [TenantID]) ON UPDATE CASCADE
);












GO

GO

GO

GO

GO
CREATE NONCLUSTERED INDEX [IX_Config_Module_RequestType_ModuleNameLookup] ON [dbo].[Config_Module_RequestType] ([ModuleNameLookup], [Deleted], [OutOfOffice], [RequestType])