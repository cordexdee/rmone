CREATE TABLE [dbo].[ReportComponents] (
    [ID]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [AskUser]                BIT             NULL,
    [ComponentType]          NVARCHAR (MAX)  NULL,
    [DashBoardPanelId]       NVARCHAR (250)  NULL,
    [Description]            NVARCHAR (MAX)  NULL,
    [Height]                 INT             NULL,
    [ItemOrder]              INT             NULL,
    [ReportDefinitionLookup] BIGINT          NOT NULL,
    [RequestParams]          NVARCHAR (MAX)  NULL,
    [ShowInNextLine]         BIT             NULL,
    [Width]                  INT             NULL,
    [Title]                  VARCHAR (250)   NULL,
    [TenantID]               NVARCHAR (128)  NULL,
    [Created]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]               DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]         NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                BIT             DEFAULT ((0)) NULL,
    [Attachments]            NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ReportDefinitionLookup]) REFERENCES [dbo].[ReportDefinition] ([ID])
);






GO
