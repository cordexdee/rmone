CREATE TABLE [dbo].[Config_Dashboard_DashboardFactTables] (
    [ID]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [CacheAfter]         INT             NULL,
    [CacheMode]          NVARCHAR (MAX)  NULL,
    [CacheTable]         BIT             NULL,
    [CacheThreshold]     INT             NULL,
    [DashboardPanelInfo] NVARCHAR (MAX)  NULL,
    [Description]        NVARCHAR (MAX)  NULL,
    [ExpiryDate]         DATETIME        NULL,
    [LastUpdated]        DATETIME        NULL,
    [RefreshMode]        NVARCHAR (MAX)  NULL,
    [Status]             NVARCHAR (MAX)  NULL,
    [Title]              NVARCHAR (250)  NULL,
    [TenantID]           NVARCHAR (128)  NULL,
    [Created]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]            BIT             DEFAULT ((0)) NULL,
    [Attachments]        NVARCHAR (2000) DEFAULT ('') NULL
);





