CREATE TABLE [dbo].[Config_ProjectComplexity] (
    [ID]                   BIGINT         IDENTITY (1, 1) NOT NULL,
    [CRMProjectComplexityChoice] NVARCHAR (3)   NULL,
    [MinValue]             FLOAT (53)     NULL,
    [MaxValue]             FLOAT (53)     NULL,
    [Created]              DATETIME       DEFAULT (getdate()) NOT NULL,
    [Modified]             DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]        NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]       NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [TenantID]             NVARCHAR (128) NULL,
    [Deleted]              BIT            DEFAULT ((0)) NULL,
    [Attachments]          NVARCHAR (100) DEFAULT ('') NULL,
    CONSTRAINT [PK_ProjectComplexity] PRIMARY KEY CLUSTERED ([ID] ASC)
);

