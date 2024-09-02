CREATE TABLE [dbo].[Config_PageConfiguration] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [Title]          NVARCHAR (250)  NOT NULL,
    [Name]           NVARCHAR (250)  NULL,
    [LeftMenuName]   NVARCHAR (250)  NULL,
    [LeftMenuType]   NVARCHAR (250)  NULL,
    [HideLeftMenu]   BIT             NULL,
    [HideTopMenu]    BIT             NULL,
    [HideHeader]     BIT             DEFAULT ((0)) NOT NULL,
    [TopMenuName]    NVARCHAR (250)  NULL,
    [TopMenuType]    NVARCHAR (250)  NULL,
    [HideSearch]     BIT             NULL,
    [HideFooter]     BIT             NULL,
    [ControlInfo]    NVARCHAR (MAX)  NULL,
    [LayoutInfo]     NVARCHAR (MAX)  NULL,
    [TenantID]       NVARCHAR (128)  NULL,
    [RootFolder]     NVARCHAR (250)  NULL,
    [Created]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT             DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (2000) DEFAULT ('') NULL
);





