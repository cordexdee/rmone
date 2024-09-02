﻿CREATE TABLE [dbo].[WikiMenuLeftNavigation] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [ItemOrder]      INT             NULL,
    [ColumnType]     NVARCHAR (MAX)  NULL,
    [Title]          VARCHAR (300)   NULL,
    [ImageUrl]       NVARCHAR (MAX)  NULL,
    [TenantID]       NVARCHAR (128)  NULL,
    [Created]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT             DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);


