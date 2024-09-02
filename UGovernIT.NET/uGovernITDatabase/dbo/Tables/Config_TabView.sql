CREATE TABLE [dbo].[Config_TabView] (
    [ID]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [TabName]          VARCHAR (50)    NULL,
    [TabDisplayName]   VARCHAR (100)   NULL,
    [ViewName]         VARCHAR (100)   NULL,
    [ModuleNameLookup] VARCHAR (10)    NULL,
    [TabOrder]         INT             NULL,
    [ColumnViewName]   VARCHAR (20)    NULL,
    [ShowTab]          BIT             DEFAULT ((1)) NOT NULL,
    [TenantID]         NVARCHAR (128)  NULL,
    [Created]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]         DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]          BIT             DEFAULT ((0)) NULL,
    [Attachments]      NVARCHAR (2000) DEFAULT ('') NULL,
    [TablabelName] VARCHAR(100) NULL, 
    CONSTRAINT [PK_Config_TabView] PRIMARY KEY CLUSTERED ([ID] ASC)
);







