CREATE TABLE [dbo].[CompanyDivisions] (
    [ID]              BIGINT          IDENTITY (1, 1) NOT NULL,
    [CompanyIdLookup] BIGINT          NOT NULL,
    [Description]     NVARCHAR (MAX)  NULL,
    [GLCode]          NVARCHAR (250)  NULL,
    [Title]           NVARCHAR (250)  NULL,
    [TenantID]        NVARCHAR (128)  NULL,
    [CompanyID]       BIGINT          NULL,
    [ManagerUser]     NVARCHAR (250)  NULL,
    [Created]         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]         BIT             DEFAULT ((0)) NULL,
    [Attachments]     NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([CompanyIdLookup]) REFERENCES [dbo].[Company] ([ID])
);






GO
