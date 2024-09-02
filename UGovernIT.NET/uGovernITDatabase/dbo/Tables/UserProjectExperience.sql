CREATE TABLE [dbo].[UserProjectExperience]
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](200) NULL,
	[UserId] [nvarchar](max) NULL,
	[ResourceUser] [varchar](200) NULL,
	[TagLookup] [int] NULL,
	[TenantID] [nvarchar](128) NULL,
	[Created] [datetime] NULL,
	[Modified] [datetime] NULL,
	[CreatedByUser] [nvarchar](128) NULL,
	[ModifiedByUser] [nvarchar](128) NULL
)
