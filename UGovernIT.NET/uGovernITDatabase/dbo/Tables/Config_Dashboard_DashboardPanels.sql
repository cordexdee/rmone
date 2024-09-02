CREATE TABLE [dbo].[Config_Dashboard_DashboardPanels] (
    [ID]                         BIGINT         IDENTITY (1, 1) NOT NULL,
    [AuthorizedToView]           NVARCHAR (250) NULL,
    [CategoryName]               NVARCHAR (250) NULL,
    [DashboardDescription]       NVARCHAR (MAX) NULL,
    [DashboardModuleMultiLookup] NVARCHAR (MAX) NULL,
    [DashboardPanelInfo]         NVARCHAR (MAX) NULL,
    [DashboardPermissionUser]    NVARCHAR (MAX) NULL,
    [DashboardType]              INT            NULL,
    [FontStyle]                  NVARCHAR (250) NULL,
    [HeaderFontStyle]            NVARCHAR (250) NULL,
    [IsActivated]                BIT            NULL,
    [IsHideDescription]          BIT            NULL,
    [IsHideTitle]                BIT            NULL,
    [IsShowInSideBar]            BIT            NULL,
    [ItemOrder]                  INT            NULL,
    [PanelHeight]                INT            NULL,
    [PanelWidth]                 INT            NULL,
    [SubCategory]                NVARCHAR (250) NULL,
    [ThemeColor]                 NVARCHAR (MAX) NULL,
    [Title]                      VARCHAR (250)  NULL,
    [Attachments]                NVARCHAR (MAX) NULL,
    [TenantID]                   NVARCHAR (128) NULL,
    [Icon]                       NVARCHAR (500) NULL,
    [Created]                    DATETIME       DEFAULT (getdate()) NOT NULL,
    [Modified]                   DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]              NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]             NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                    BIT            DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);









