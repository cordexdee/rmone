CREATE TABLE [dbo].[Config_ClientAdminCategory] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [CategoryName]   NVARCHAR (250)  NULL,
    [ImageUrl]       NVARCHAR (250)  NULL,
    [ItemOrder]      INT             NULL,
    [Title]          VARCHAR (250)   NULL,
    [TenantID]       NVARCHAR (128)  NULL,
    [Created]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT             DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_Config_ClientAdminCategory_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);









