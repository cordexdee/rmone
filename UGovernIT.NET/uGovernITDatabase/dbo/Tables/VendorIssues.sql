CREATE TABLE [dbo].[VendorIssues] (
    [ID]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [AssignedToUser]      NVARCHAR (250)  NULL,
    [Body]                NVARCHAR (MAX)  NULL,
    [ChildCount]          INT             NULL,
    [Comment]             NVARCHAR (MAX)  NULL,
    [Contribution]        INT             NULL,
    [DueDate]             DATETIME        NULL,
    [Duration]            INT             NULL,
    [ItemOrder]           INT             NULL,
    [Level]               INT             NULL,
    [ParentTask]          INT             NULL,
    [PercentComplete]     INT             NULL,
    [Predecessors]        NVARCHAR (250)  NULL,
    [Priority]            NVARCHAR (MAX)  NULL,
    [ProposedDate]        DATETIME        NULL,
    [ProposedStatus]      NVARCHAR (MAX)  NULL,
    [Resolution]          NVARCHAR (MAX)  NULL,
    [ResolutionDate]      DATETIME        NULL,
    [StartDate]           DATETIME        NULL,
    [Status]              NVARCHAR (MAX)  NULL,
    [TaskGroup]           NVARCHAR (MAX)  NULL,
    [Title]               NVARCHAR (250)  NULL,
    [VendorIssueImpact]   NVARCHAR (MAX)  NULL,
    [VendorMSALookup]     BIGINT          NOT NULL,
    [VendorMSANameLookup] BIGINT          NOT NULL,
    [VendorSOWLookup]     BIGINT          NOT NULL,
    [VendorSOWNameLookup] BIGINT          NOT NULL,
    [VNDActionType]       NVARCHAR (MAX)  NULL,
    [TenantID]            NVARCHAR (128)  NULL,
    [Created]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]             BIT             DEFAULT ((0)) NULL,
    [Attachments]         NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([VendorMSALookup]) REFERENCES [dbo].[VendorMSA] ([ID]),
    FOREIGN KEY ([VendorMSANameLookup]) REFERENCES [dbo].[VendorMSA] ([ID]),
    FOREIGN KEY ([VendorSOWLookup]) REFERENCES [dbo].[VendorSOW] ([ID]),
    FOREIGN KEY ([VendorSOWNameLookup]) REFERENCES [dbo].[VendorSOW] ([ID])
);






GO

GO

GO

GO
