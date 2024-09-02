CREATE TABLE [dbo].[Agents]
(
	[Id]  BIGINT  IDENTITY (1, 1) NOT NULL,
	[Title]          VARCHAR (250)   NULL,
	[Description] NVARCHAR (MAX)  NULL,  
	
    [TenantID]       NVARCHAR (128)  NULL,
	[Created]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT             DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (2000) DEFAULT ('') NULL,
	[Icon] [nvarchar](255) NULL,
	[Name] [nvarchar](250) NULL,
	[Url] [nvarchar](250) NULL,
	[ServiceLookUp] [bigint] Default (0) NULL,
	[Parameters] [nvarchar](max) NULL,
	[Control] [nvarchar](250) NULL,
	[Width] [nvarchar](250) NULL,
	[Height] [nvarchar](250) NULL,
	PRIMARY KEY CLUSTERED ([ID] ASC),
)
