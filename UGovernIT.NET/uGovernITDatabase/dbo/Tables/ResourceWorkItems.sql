CREATE TABLE [dbo].[ResourceWorkItems] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [ResourceUser]   NVARCHAR (MAX)  NULL,
    [SubWorkItem]    NVARCHAR (250)  NULL,
    [WorkItem]       NVARCHAR (250)  NULL,
    [WorkItemType]   NVARCHAR (250)  NULL,
    [Title]          VARCHAR (250)   NULL,
    [StartDate]      DATETIME        NULL,
    [EndDate]        DATETIME        NULL,
    [TenantID]       NVARCHAR (128)  NULL,
    [Created]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT             DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (2000) DEFAULT ('') NULL,
    [SubSubWorkItem] NVARCHAR (250)  NULL,
    CONSTRAINT [PK_ResourceWorkItems] PRIMARY KEY CLUSTERED ([ID] ASC)
);





