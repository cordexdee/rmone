CREATE TABLE [dbo].[SubLocation] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [LocationID]     BIGINT          NULL,
    [Title]          NVARCHAR (250)  NOT NULL,
    [LocationTag]    NVARCHAR (250)  NULL,
    [Description]    NVARCHAR (250)  NULL,
    [Address1]       NVARCHAR (500)  NULL,
    [Address2]       NVARCHAR (500)  NULL,
    [Zip]            NVARCHAR (50)   NULL,
    [Phone]          NVARCHAR (20)   NULL,
    [Deleted]        BIT             NULL,
    [TenantID]       NVARCHAR (128)  NULL,
    [Modified]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [Created]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Attachments]    NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_SubLocation] PRIMARY KEY CLUSTERED ([ID] ASC)
);


