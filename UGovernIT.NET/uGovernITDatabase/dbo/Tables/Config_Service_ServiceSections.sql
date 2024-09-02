CREATE TABLE [dbo].[Config_Service_ServiceSections] (
    [ID]              BIGINT          IDENTITY (1, 1) NOT NULL,
    [Description]     NVARCHAR (MAX)  NULL,
    [ItemOrder]       INT             NULL,
    [SectionName]     NVARCHAR (250)  NULL,
    [SectionSequence] INT             NULL,
    [ServiceID]       BIGINT          NOT NULL,
    [Title]           VARCHAR (250)   NULL,
    [TenantID]        NVARCHAR (128)  NULL,
    [Created]         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]         BIT             DEFAULT ((0)) NULL,
    [Attachments]     NVARCHAR (2000) DEFAULT ('') NULL,
	[IconUrl]		  NVARCHAR (500)  NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Config_Services] ([ID]),
    FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Config_Services] ([ID]),
    FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Config_Services] ([ID])
);






GO

GO

GO
