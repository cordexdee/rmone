CREATE TABLE [dbo].[ExperiencedTags]
(
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](500) NULL,
	[TenantID] [nvarchar](128) NULL,
	[InsertedBy] [nvarchar](100) NULL,
	[Created] [datetime] NULL,
	[Modified] [datetime] NULL,
	[CreatedByUser] [nvarchar](128) NULL,
	[ModifiedByUser] [nvarchar](128) NULL,
	[Deleted] [bit] NULL,
	[Attachments] [nvarchar](2000) NULL
)
