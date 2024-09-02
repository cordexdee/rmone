CREATE TABLE [dbo].[DMDocumentHistoryList] (
    [ID]                BIGINT          IDENTITY (1, 1) NOT NULL,
    [Action]            NVARCHAR (250)  NULL,
    [Authors]           NVARCHAR (MAX)  NULL,
    [DocumentControlID] NVARCHAR (250)  NULL,
    [DocumentID]        NVARCHAR (250)  NULL,
    [DocVersion]        NVARCHAR (250)  NULL,
    [FileName]          NVARCHAR (250)  NULL,
    [FolderID]          NVARCHAR (250)  NULL,
    [History]           NVARCHAR (MAX)  NULL,
    [HistoryDate]       DATETIME        NULL,
    [PortalId]          NVARCHAR (250)  NULL,
    [Title]             VARCHAR (250)   NULL,
    [TenantID]          NVARCHAR (128)  NULL,
    [Created]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]           BIT             DEFAULT ((0)) NULL,
    [Attachments]       NVARCHAR (2000) DEFAULT ('') NULL
);





