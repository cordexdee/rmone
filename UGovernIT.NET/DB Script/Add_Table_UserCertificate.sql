
CREATE TABLE [dbo].[UserCertificates](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[Title] [nvarchar](250) NULL,
	[TenantID] [nvarchar](128) NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[CreatedByUser] [nvarchar](128) NOT NULL,
	[ModifiedByUser] [nvarchar](128) NOT NULL,
	[Deleted] [bit] NULL,
	[Attachments] [nvarchar](2000) NULL,
 CONSTRAINT [PK_UserCertificates] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserCertificates] ADD  CONSTRAINT [DF__UserCertificate__Creat__5A254709]  DEFAULT (getdate()) FOR [Created]
GO

ALTER TABLE [dbo].[UserCertificates] ADD  CONSTRAINT [DF__UserCertificate__Modif__5B196B42]  DEFAULT (getdate()) FOR [Modified]
GO

ALTER TABLE [dbo].[UserCertificates] ADD  CONSTRAINT [DF__UserCertificate__Creat__5C0D8F7B]  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [CreatedByUser]
GO

ALTER TABLE [dbo].[UserCertificates] ADD  CONSTRAINT [DF__UserCertificate__Modif__5D01B3B4]  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [ModifiedByUser]
GO

ALTER TABLE [dbo].[UserCertificates] ADD  CONSTRAINT [DF__UserCertificate__Delet__5DF5D7ED]  DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [dbo].[UserCertificates] ADD  CONSTRAINT [DF__UserCertificate__Attac__5EE9FC26]  DEFAULT ('') FOR [Attachments]
GO


