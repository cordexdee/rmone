CREATE TABLE [dbo].[Report_ConfigData] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [ReportType]     NVARCHAR (MAX)  NULL,
    [FilterHeight]   VARCHAR (10)    NULL,
    [FilterWidth]    VARCHAR (10)    NULL,
    [FilterTitle]    NVARCHAR (500)  NULL,
    [ReportTitle]    NVARCHAR (MAX)  NULL,
    [TenantID]       NVARCHAR (128)  NULL,
    [Created]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT             DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (2000) DEFAULT ('') NULL
);


