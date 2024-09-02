CREATE TABLE [dbo].[DashboardSummary] (
    [ID]                      BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActualHours]             INT             NULL,
    [Age]                     BIGINT          NULL,
    [ALLSLAsMet]              NVARCHAR (MAX)  NULL,
    [AssignmentSLAMet]        NVARCHAR (MAX)  NULL,
    [Category]                NVARCHAR (250)  NULL,
    [Closed]                  BIT             NULL,
    [CloseSLAMet]             NVARCHAR (MAX)  NULL,
    [Country]                 NVARCHAR (250)  NULL,
    [CreationDate]            DATETIME        NULL,
    [Description]             NVARCHAR (MAX)  NULL,
    [FunctionalAreaLookup]    BIGINT          NULL,
    [GenericStatusLookup]     INT             NULL,
    [InitiatorUser]           NVARCHAR (MAX)  NULL,
    [InitiatorResolvedChoice] NVARCHAR (MAX)  NULL,
    [LocationLookup]          BIGINT          NULL,
    [ModuleNameLookup]        NVARCHAR (250)  NULL,
    [ModuleStepLookup]        NVARCHAR (250)  NULL,
    [OnHold]                  INT             NULL,
    [ORPUser]                 NVARCHAR (250)  NULL,
    [OtherSLAMet]             NVARCHAR (MAX)  NULL,
    [OwnerUser]               NVARCHAR (250)  NULL,
    [PriorityLookup]          BIGINT          NULL,
    [PRPUser]                 NVARCHAR (MAX)  NULL,
    [PRPGroupUser]            NVARCHAR (MAX)  NULL,
    [Region]                  NVARCHAR (250)  NULL,
    [ReopenCount]             INT             NULL,
    [RequestorUser]           NVARCHAR (250)  NULL,
    [RequestorCompany]        NVARCHAR (250)  NULL,
    [RequestorContactSLAMet]  NVARCHAR (MAX)  NULL,
    [RequestorDepartment]     NVARCHAR (250)  NULL,
    [RequestorDivision]       NVARCHAR (250)  NULL,
    [RequestSourceChoice]     NVARCHAR (MAX)  NULL,
    [RequestTypeLookup]       BIGINT          NULL,
    [ResolutionSLAMet]        NVARCHAR (MAX)  NULL,
    [ResolutionTypeChoice]    NVARCHAR (MAX)  NULL,
    [ServiceCategoryName]     NVARCHAR (250)  NULL,
    [ServiceName]             NVARCHAR (250)  NULL,
    [SLAMet]                  NVARCHAR (250)  NULL,
    [StageActionUsersUser]    NVARCHAR (250)  NULL,
    [State]                   NVARCHAR (250)  NULL,
    [Status]                  NVARCHAR (250)  NULL,
    [SubCategory]             NVARCHAR (250)  NULL,
    [TicketId]                NVARCHAR (250)  NULL,
    [WorkflowType]            NVARCHAR (250)  NULL,
    [Title]                   VARCHAR (250)   NULL,
    [TenantID]                NVARCHAR (128)  NULL,
    [OnHoldTillDate]          DATETIME        NULL,
    [TotalHoldDuration]       FLOAT (53)      NULL,
    [InitiatedDate]           DATETIME        NULL,
    [AssignedDate]            DATETIME        NULL,
    [ResolvedDate]            DATETIME        NULL,
    [TestedDate]              DATETIME        NULL,
    [ClosedDate]              DATETIME        NULL,
    [Rejected]                BIT             NULL,
    [ClosedByUser]            NVARCHAR (128)  NULL,
    [ResolvedByUser]          NVARCHAR (128)  NULL,
    [AssignedBy]              NVARCHAR (128)  NULL,
    [Created]                 DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]           NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                 BIT             DEFAULT ((0)) NULL,
    [Attachments]             NVARCHAR (2000) DEFAULT ('') NULL,
    [SLADisabled]             BIT             DEFAULT ((0)) NOT NULL,
    [StageStep] INT NULL , 
    [OnHoldStartDate] DATETIME2 NULL, 
    [TicketOnHold] BIT NULL, 
    [ApprovalDate] DATETIME NULL, 
    [ApprovedByUser] VARCHAR(128) NULL, 
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([FunctionalAreaLookup]) REFERENCES [dbo].[FunctionalAreas] ([ID]),
    FOREIGN KEY ([GenericStatusLookup]) REFERENCES [dbo].[GenericStatus] ([ID]) ON UPDATE CASCADE,
    FOREIGN KEY ([LocationLookup]) REFERENCES [dbo].[Location] ([ID]),
    FOREIGN KEY ([PriorityLookup]) REFERENCES [dbo].[Config_Module_Priority] ([ID]),
    FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID]),
    CONSTRAINT [FK_DashboardSummaryAssignedBy_ToUser] FOREIGN KEY ([AssignedBy]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_DashboardSummaryClosedBy_ToUser] FOREIGN KEY ([ClosedByUser]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_DashboardSummaryResolvedBy_ToUser] FOREIGN KEY ([ResolvedByUser]) REFERENCES [dbo].[AspNetUsers] ([Id])
);














GO

GO

GO

GO

GO
CREATE NONCLUSTERED INDEX [IX_DashboardSummary_ModuleNameLookup] ON [dbo].[DashboardSummary] ([ModuleNameLookup])

GO
CREATE NONCLUSTERED INDEX [IX_DashboardSummary_ModuleStepLookup] ON [dbo].[DashboardSummary] ([ModuleStepLookup])

GO
CREATE NONCLUSTERED INDEX [IX_DashboardSummary_PriorityLookup] ON [dbo].[DashboardSummary] ([PriorityLookup])

GO
CREATE NONCLUSTERED INDEX [IX_DashboardSummary_RequestTypeLookup] ON [dbo].[DashboardSummary] ([RequestTypeLookup])

GO
CREATE NONCLUSTERED INDEX [IX_DashboardSummary_Status] ON [dbo].[DashboardSummary] ([Status])

GO
CREATE NONCLUSTERED INDEX [IX_DashboardSummary_TicketId] ON [dbo].[DashboardSummary] ([TicketId])