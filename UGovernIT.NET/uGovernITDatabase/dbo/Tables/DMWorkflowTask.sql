CREATE TABLE [dbo].[DMWorkflowTask] (
    [ID]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [AssignedToUser]     NVARCHAR (MAX)  NULL,
    [Body]               NVARCHAR (MAX)  NULL,
    [DocID]              NVARCHAR (250)  NULL,
    [DueDate]            DATETIME        NULL,
    [PercentComplete]    INT             NULL,
    [PingComments]       NVARCHAR (MAX)  NULL,
    [Predecessors]       NVARCHAR (250)  NULL,
    [Priority]           NVARCHAR (MAX)  NULL,
    [Requested_By]       NVARCHAR (MAX)  NULL,
    [StartDate]          DATETIME        NULL,
    [Status]             NVARCHAR (MAX)  NULL,
    [TaskGroup]          NVARCHAR (MAX)  NULL,
    [WorkflowTaskStatus] NVARCHAR (MAX)  NULL,
    [Title]              VARCHAR (250)   NULL,
    [TenantID]           NVARCHAR (128)  NULL,
    [Created]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]            BIT             DEFAULT ((0)) NULL,
    [Attachments]        NVARCHAR (2000) DEFAULT ('') NULL
);





