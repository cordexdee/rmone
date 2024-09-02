CREATE TABLE [dbo].[PMMIssues] (
    [ID]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [AssignedToUser]     NVARCHAR (250)  NULL,
    [Body]               NVARCHAR (MAX)  NULL,
    [ChildCount]         INT             NULL,
    [Comment]            NVARCHAR (MAX)  NULL,
    [Contribution]       FLOAT           NULL,
    [DueDate]            DATETIME2        NULL,
    [Duration]           FLOAT             NULL,
    [IssueImpact]        NVARCHAR (MAX)  NULL,
    [ItemOrder]          INT             NULL,
    [Level]              INT             NULL,
    [ParentTask]         INT             NULL,
    [PercentComplete]    FLOAT             NULL,
    [PMMIdLookup]        BIGINT          NOT NULL,
    [Predecessors]       NVARCHAR (250)  NULL,
    [Priority]           NVARCHAR (MAX)  NULL,
    [ProposedDate]       DATETIME        NULL,
    [ProposedStatus]     NVARCHAR (MAX)  NULL,
    [Resolution]         NVARCHAR (MAX)  NULL,
    [ResolutionDate]     DATETIME        NULL,
    [StartDate]          DATETIME2        NULL,
    [Status]             NVARCHAR (MAX)  NULL,
    [TaskActualHours]    FLOAT             NULL,
    [TaskEstimatedHours] FLOAT             NULL,
    [TaskGroup]          NVARCHAR (MAX)  NULL,
    [Title]              VARCHAR (250)   NULL,
    [TenantID]           NVARCHAR (128)  NULL,
    [Created]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]            BIT             DEFAULT ((0)) NULL,
    [Attachments]        NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([PMMIdLookup]) REFERENCES [dbo].[PMM] ([ID])
);






GO
