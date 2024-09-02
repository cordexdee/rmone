CREATE TABLE [dbo].[ProjectSimilarityConfig] (
    [Id]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [ColumnName]     NVARCHAR (250) NULL,
    [ColumnType]     NVARCHAR (250) NULL,
    [StageWeight]    INT            NULL,
    [Title]          NVARCHAR (250) NULL,
    [TenantID]       NVARCHAR (128) NULL,
    [Created]        DATETIME       DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT            DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (100) DEFAULT ('') NULL,
    [modulenamelookup] NVARCHAR(10) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


