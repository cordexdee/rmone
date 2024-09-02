CREATE TABLE [dbo].[BusinessUnits] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [Title]          NVARCHAR (250)  NULL,
    [Description]    NVARCHAR (MAX)  NULL,
    [TenantID]       NVARCHAR (128)  NULL,
    [Created]        DATETIME        NOT NULL,
    [Modified]       DATETIME        NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  NOT NULL,
    [Deleted]        BIT             NULL,
    [Attachments]    NVARCHAR (2000) NULL,
    CONSTRAINT [PK_BusinessUnits] PRIMARY KEY CLUSTERED ([ID] ASC)
);


