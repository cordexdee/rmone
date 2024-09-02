CREATE TABLE [dbo].[Resources] (
    [ID]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [_ResourceType]          NVARCHAR (250)  NULL,
    [AllocationEndDate]      DATETIME        NULL,
    [AllocationStartDate]    DATETIME        NULL,
    [BudgetDescription]      NVARCHAR (MAX)  NULL,
    [BudgetType]             NVARCHAR (250)  NULL,
    [EstimatedHours]         INT             NULL,
    [ModuleNameLookup]       NVARCHAR (250)  NULL,
    [NoOfFTEs]               INT             NULL,
    [RequestedResourcesUser] NVARCHAR (250)  NULL,
    [TicketId]               NVARCHAR (250)  NULL,
    [Title]                  NVARCHAR (250)  NULL,
    [UserSkillLookup]        BIGINT          NOT NULL,
    [TenantID]               NVARCHAR (128)  NULL,
    [Created]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]               DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]         NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                BIT             DEFAULT ((0)) NULL,
    [Attachments]            NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([UserSkillLookup]) REFERENCES [dbo].[UserSkills] ([ID])
);










GO
