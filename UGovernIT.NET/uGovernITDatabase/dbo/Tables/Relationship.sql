CREATE TABLE [dbo].[Relationship] (
    [ID]                          BIGINT          IDENTITY (1, 1) NOT NULL,
    [ApprovalStatus]              NVARCHAR (250)  NULL,
    [ApprovalTypeChoice]          NVARCHAR (MAX)  NULL,
    [ApprovedBy]                  NVARCHAR (250)  NULL,
    [ApproverUser]                NVARCHAR (250)  NULL,
    [AssignedToUser]              NVARCHAR (MAX)  NULL,
    [AutoCreateUser]              BIT             NULL,
    [Body]                        NVARCHAR (MAX)  NULL,
    [ChildId]                     NVARCHAR (250)  NULL,
    [Comment]                     NVARCHAR (MAX)  NULL,
    [CompletedBy]                 NVARCHAR (MAX)  NULL,
    [CompletionDate]              DATETIME        NULL,
    [DueDate]                     DATETIME        NULL,
    [EnableApproval]              BIT             NULL,
    [ErrorMsg]                    NVARCHAR (250)  NULL,
    [ItemOrder]                   INT             NULL,
    [Level]                       INT             NULL,
    [ModuleNameLookup]            NVARCHAR (250)  NULL,
    [NewUserName]                 NVARCHAR (250)  NULL,
    [ParentId]                    NVARCHAR (250)  NULL,
    [ParentTask]                  INT             NULL,
    [PercentComplete]             INT             NULL,
    [Predecessors]                NVARCHAR (250)  NULL,
    [Priority]                    NVARCHAR (MAX)  NULL,
    [ServiceApplicationAccessXml] NVARCHAR (MAX)  NULL,
    [StageWeight]                 INT             NULL,
    [StartDate]                   DATETIME        NULL,
    [Status]                      NVARCHAR (MAX)  NULL,
    [SubTaskType]                 NVARCHAR (MAX)  NULL,
    [TaskActionUser]              NVARCHAR (250)  NULL,
    [TaskActualHours]             INT             NULL,
    [TaskEstimatedHours]          INT             NULL,
    [TaskGroup]                   NVARCHAR (MAX)  NULL,
    [TaskType]                    NVARCHAR (MAX)  NULL,
    [Title]                       VARCHAR (250)   NULL,
    [TenantID]                    NVARCHAR (128)  NULL,
    [Created]                     DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                    DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]               NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]              NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                     BIT             DEFAULT ((0)) NULL,
    [Attachments]                 NVARCHAR (2000) DEFAULT ('') NULL
);







