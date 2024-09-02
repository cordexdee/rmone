CREATE TABLE [dbo].[BackgroundProcessStatus] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [ServiceName]    NVARCHAR (256)  NULL,
    [StartDate]      DATETIME        NULL,
    [EndDate]        DATETIME        NULL,
    [Status]         NVARCHAR (256)  NULL,
    [UserName]       NVARCHAR (256)  NULL,
    [TenantID]       NVARCHAR (128)  NULL,
    [Created]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT             DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (2000) DEFAULT ('') NULL
);


