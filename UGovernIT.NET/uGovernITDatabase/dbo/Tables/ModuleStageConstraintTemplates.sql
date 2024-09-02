CREATE TABLE [dbo].[ModuleStageConstraintTemplates] (
    [ID]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [AssignedToUser]      NVARCHAR (250)  NULL,
    [Body]                NVARCHAR (MAX)  NULL,
    [Comment]             NVARCHAR (MAX)  NULL,
    [DocumentInfo]        NVARCHAR (250)  NULL,
    [DocumentLibraryName] NVARCHAR (250)  NULL,
    [FormulaValue]        NVARCHAR (MAX)  NULL,
    [ModuleAutoApprove]   BIT             NULL,
    [ModuleNameLookup]    NVARCHAR (250)  NULL,
    [ModuleStep]          INT             NULL,
    [PercentComplete]     FLOAT             NULL,
    [Predecessors]        NVARCHAR (250)  NULL,
    [Priority]            NVARCHAR (MAX)  NULL,
    [ProposedDate]        DATETIME        NULL,
    [ProposedStatus]      NVARCHAR (MAX)  NULL,
    [StartDate]           DATETIME2       NULL,
    [TaskActualHours]     FLOAT (53)      NULL,
    [TaskDueDate]         DATETIME2       NULL,
    [EstimatedHours]      FLOAT (53)      NULL,
    [TaskStatus]          NVARCHAR (MAX)  NULL,
    [TicketId]            NVARCHAR (250)  NULL,
    [Title]               NVARCHAR (250)  NULL,
    [TenantID]            NVARCHAR (128)  NULL,
    [DateExpression]      NVARCHAR (500)  NULL,
    [UserRoleType]        NVARCHAR (500)  NULL,
    [ItemOrder]           INT             NULL,
    [Created]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]             BIT             DEFAULT ((0)) NULL,
    [Attachments]         NVARCHAR (2000) DEFAULT ('') NULL
);





