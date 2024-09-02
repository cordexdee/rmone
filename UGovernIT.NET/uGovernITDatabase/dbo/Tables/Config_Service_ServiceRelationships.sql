CREATE TABLE [dbo].[Config_Service_ServiceRelationships] (
    [ID]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [ApprovalStatus]      NVARCHAR (250)  NULL,
    [ApproverUser]        NVARCHAR (250)  NULL,
    [AssignedToUser]      NVARCHAR (250)  NULL,
    [AutoCreateUser]      BIT             NULL,
    [Body]                NVARCHAR (MAX)  NULL,
    [DueDate]             DATETIME        NULL,
    [Duration]            INT             NULL,
    [EnableApproval]      BIT             NULL,
    [ItemOrder]           INT             NULL,
    [Level]               INT             NULL,
    [ModuleNameLookup]    NVARCHAR (250)  NULL,
    [Name]                NVARCHAR (250)  NULL,
    [NewUserName]         NVARCHAR (250)  NULL,
    [ParentTask]          INT             NULL,
    [PercentComplete]     INT             NULL,
    [Predecessors]        NVARCHAR (250)  NULL,
    [Priority]            NVARCHAR (MAX)  NULL,
    [RequestTypeCategory] NVARCHAR (250)  NULL,
    [RequestTypeLookup]   BIGINT          NOT NULL,
    [RequestTypeWorkflow] NVARCHAR (250)  NULL,
    [ServiceLookUp]       BIGINT          NOT NULL,
    [StageWeight]         INT             NULL,
    [StartDate]           DATETIME        NULL,
    [Status]              NVARCHAR (MAX)  NULL,
    [SubTaskType]         NVARCHAR (MAX)  NULL,
    [TaskEstimatedHours]  INT             NULL,
    [TaskGroup]           NVARCHAR (MAX)  NULL,
    [Title]               VARCHAR (250)   NULL,
    [TenantID]            NVARCHAR (128)  NULL,
    [Created]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]             BIT             DEFAULT ((0)) NULL,
    [Attachments]         NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID]),
    FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID]),
    FOREIGN KEY ([ServiceLookUp]) REFERENCES [dbo].[Config_Services] ([ID])
);






GO

GO

GO
