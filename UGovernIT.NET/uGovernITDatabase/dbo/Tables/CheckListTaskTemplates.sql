CREATE TABLE [dbo].[CheckListTaskTemplates] (
    [ID]                      BIGINT         IDENTITY (1, 1) NOT NULL,
    [CheckListTemplateLookup] BIGINT         NULL,
    [Title]                   NVARCHAR (256) NULL,
    [Module]                  NVARCHAR (256) NULL,
    [Created]                 DATETIME       DEFAULT (getdate()) NOT NULL,
    [Modified]                DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]           NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]          NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [TenantID]                NVARCHAR (128) NULL,
    [Deleted]                 BIT            DEFAULT ((0)) NULL,
    [Attachments]             NVARCHAR (256) DEFAULT ('') NULL,
    [DataMigrationID]         INT            NULL,
    CONSTRAINT [PK_CheckListTaskTemplates] PRIMARY KEY CLUSTERED ([ID] ASC)
);

 