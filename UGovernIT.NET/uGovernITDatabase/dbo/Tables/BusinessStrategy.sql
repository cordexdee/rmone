CREATE TABLE [dbo].[BusinessStrategy] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [Title]          NVARCHAR (250)  NULL,
	[Owner]  nvarchar(250) null,
	[StakeHolder] nvarchar(250) null,
    [Description]    NVARCHAR (500)  NULL,
    [TenantID]       NVARCHAR (128)  NULL,
    [Created]        DATETIME        NULL,
    [Modified]       DATETIME        NULL,
    [CreatedByUser]  NVARCHAR (128)  NULL,
    [ModifiedByUser] NVARCHAR (128)  NULL,
    [Deleted]        BIT             NULL,
    [Attachments]    NVARCHAR (2000) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);


