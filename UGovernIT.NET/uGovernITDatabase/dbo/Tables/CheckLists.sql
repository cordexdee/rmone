CREATE TABLE [dbo].[CheckLists] (
    [ID]                      BIGINT         IDENTITY (1, 1) NOT NULL,
    [AssignedToUser]          NVARCHAR (MAX) NULL,
    [CheckListName]           NVARCHAR (256) NULL,
    [CheckListTemplateLookup] BIGINT         NULL,
    [Module]                  NVARCHAR (256) NULL,
    [TicketId]                NVARCHAR (256) NULL,
    [Title]                   NVARCHAR (256) NULL,
    [Type]                    NVARCHAR (256) NULL,
    [Created]                 DATETIME       DEFAULT (getdate()) NOT NULL,
    [Modified]                DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]           NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]          NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [TenantID]                NVARCHAR (128) NULL,
    [Deleted]                 BIT            DEFAULT ((0)) NULL,
    [Attachments]             NVARCHAR (256) DEFAULT ('') NULL,
    [DataMigrationID]         INT            NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

