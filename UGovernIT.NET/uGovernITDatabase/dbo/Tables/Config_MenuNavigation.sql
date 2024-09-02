CREATE TABLE [dbo].[Config_MenuNavigation] (
    [ID]                   BIGINT          IDENTITY (1, 1) NOT NULL,
    [AuthorizedToView]     NVARCHAR (250)  NULL,
    [CustomizeFormat]      BIT             NULL,
    [CustomProperties]     NVARCHAR (MAX)  NULL,
    [MenuHeight]           INT             NULL,
    [IconUrl]              NVARCHAR (250)  NULL,
    [IsDisabled]           BIT             NULL,
    [ItemOrder]            INT             NULL,
    [MenuBackground]       NVARCHAR (250)  NULL,
    [MenuDisplayType]      NVARCHAR (MAX)  NULL,
    [MenuFontColor]        NVARCHAR (250)  NULL,
    [MenuItemSeparation]   NVARCHAR (250)  NULL,
    [MenuName]             NVARCHAR (250)  NULL,
    [MenuParentLookup]     BIGINT          NOT NULL,
    [NavigationType]       NVARCHAR (MAX)  NULL,
    [NavigationUrl]        NVARCHAR (MAX)  NULL,
    [SubMenuItemAlignment] NVARCHAR (250)  NULL,
    [SubMenuItemPerRow]    INT             NULL,
    [SubMenuStyle]         NVARCHAR (MAX)  NULL,
    [TextMenuAlignment]    NVARCHAR (MAX)  NULL,
    [MenuWidth]            INT             NULL,
    [Title]                VARCHAR (250)   NULL,
    [TenantID]             NVARCHAR (128)  NULL,
    [Created]              DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]        NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]              BIT             DEFAULT ((0)) NULL,
    [Attachments]          NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);





