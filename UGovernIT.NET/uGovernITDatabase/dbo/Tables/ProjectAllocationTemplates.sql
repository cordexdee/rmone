CREATE TABLE [dbo].[ProjectAllocationTemplates] (
    [ID]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]         NVARCHAR (128)  NOT NULL,
    [Created]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]         DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]          BIT             DEFAULT ((0)) NULL,
    [Attachments]      NVARCHAR (2000) DEFAULT ('') NULL,
    [Name]             NVARCHAR (255)  DEFAULT ('') NOT NULL,
    [ModuleNameLookup] NVARCHAR (50)   DEFAULT ('') NOT NULL,
    [TicketID]         NVARCHAR (50)   DEFAULT ('') NOT NULL,
    [Template]         NVARCHAR (MAX)  DEFAULT ('') NOT NULL,
    [TicketStartDate]  DATETIME        NULL,
    [TicketEndDate]    DATETIME        NULL,
    [Duration]         INT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ProjectAllocationTemplates] PRIMARY KEY CLUSTERED ([ID] ASC, [TenantID] ASC)
);





