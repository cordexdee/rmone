CREATE TABLE [dbo].[Summary_ResourceProjectComplexity] (
    [ID]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [UserId]                 NVARCHAR (36)   NULL,
    [UserName]               NVARCHAR (100)  NULL,
    [DepartmentID]           BIGINT          NULL,
    [FunctionalAreaID]       BIGINT          NULL,
    [ManagerUser]            NVARCHAR (36)   NULL,
    [GroupID]                NVARCHAR (256)  NULL,
    [Complexity]             INT             DEFAULT ((0)) NULL,
    [HighProjectCapacity]    FLOAT (53)      DEFAULT ((0)) NULL,
    [AllHighProjectCapacity] FLOAT (53)      DEFAULT ((0)) NULL,
    [RequestTypes]           NVARCHAR (500)  NULL,
    [ModuleNameLookup]       NVARCHAR (10)   NULL,
    [Count]                  INT             NULL,
    [AllCount]               INT             DEFAULT ((0)) NULL,
    [TenantID]               NVARCHAR (128)  NULL,
    [Created]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]               DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]         NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                BIT             DEFAULT ((0)) NULL,
    [Attachments]            NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);




