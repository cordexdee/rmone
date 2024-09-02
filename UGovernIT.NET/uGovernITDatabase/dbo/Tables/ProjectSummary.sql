CREATE TABLE [dbo].[ProjectSummary] (
    [ID]                      BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActualCompletionDate]    DATETIME        NULL,
    [ActualStartDate]         DATETIME        NULL,
    [AssignedToUser]          NVARCHAR (MAX)  NULL,
    [BeneficiariesLookup]     NVARCHAR (250)  NULL,
    [Body]                    NVARCHAR (MAX)  NULL,
    [CategoryName]            NVARCHAR (250)  NULL,
    [DueDate]                 DATETIME        NULL,
    [ParentTask]              INT             NULL,
    [PercentComplete]         INT             NULL,
    [Predecessors]            NVARCHAR (250)  NULL,
    [Priority]                NVARCHAR (MAX)  NULL,
    [PriorityLookup]          BIGINT          NOT NULL,
    [ProjectClassLookup]      BIGINT          NOT NULL,
    [ProjectCost]             INT             NULL,
    [ProjectInitiativeLookup] BIGINT          NOT NULL,
    [ProjectManagerUser]      NVARCHAR (250)  NULL,
    [RequestTypeLookup]       BIGINT          NOT NULL,
    [ShowProjectStatus]       BIT             NULL,
    [StartDate]               DATETIME        NULL,
    [Status]                  NVARCHAR (MAX)  NULL,
    [TaskGroup]               NVARCHAR (MAX)  NULL,
    [TicketId]                NVARCHAR (250)  NULL,
    [TotalCost]               INT             NULL,
    [Title]                   VARCHAR (250)   NULL,
    [TenantID]                NVARCHAR (128)  NULL,
    [Created]                 DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]           NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                 BIT             DEFAULT ((0)) NULL,
    [Attachments]             NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([PriorityLookup]) REFERENCES [dbo].[Config_Module_Priority] ([ID]),
    FOREIGN KEY ([ProjectClassLookup]) REFERENCES [dbo].[Config_ProjectClass] ([ID]),
    FOREIGN KEY ([ProjectInitiativeLookup]) REFERENCES [dbo].[Config_ProjectInitiative] ([ID]),
    FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID])
);










GO

GO
CREATE NONCLUSTERED INDEX [IX_ProjectSummary_PriorityLookup] ON [dbo].[ProjectSummary] ([PriorityLookup])

GO
CREATE NONCLUSTERED INDEX [IX_ProjectSummary_RequestTypeLookup] ON [dbo].[ProjectSummary] ([RequestTypeLookup])

GO
CREATE NONCLUSTERED INDEX [IX_ProjectSummary_TicketId] ON [dbo].[ProjectSummary] ([TicketId])