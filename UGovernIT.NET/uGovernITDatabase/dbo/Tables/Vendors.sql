CREATE TABLE [dbo].[Vendors] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [ContactName]    NVARCHAR (250)  NULL,
    [Title]          NVARCHAR (250)  NULL,
    [VendorAddress]  NVARCHAR (MAX)  NULL,
    [VendorEmail]    NVARCHAR (250)  NULL,
    [VendorLocation] NVARCHAR (250)  NULL,
    [VendorName]     NVARCHAR (250)  NULL,
    [VendorPhone]    NVARCHAR (250)  NULL,
    [WebsiteUrl]     NVARCHAR (250)  NULL,
    [TenantID]       NVARCHAR (128)  NULL,
    [Created]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT             DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_Vendors] PRIMARY KEY CLUSTERED ([ID] ASC)
);





