CREATE TABLE [dbo].[AspNetRoles] (
    [Id]             NVARCHAR (128)  NOT NULL,
    [Name]           NVARCHAR (256)  NOT NULL,
    [Title]          VARCHAR (250)   NULL,
    [Description]    NVARCHAR (MAX)  NULL,
    [Discriminator]  NVARCHAR (MAX)  NULL,
    [IsSystem]       BIT             DEFAULT ((0)) NOT NULL,
    [RoleType]       INT             DEFAULT ((0)) NOT NULL,
    [LandingPage]    NVARCHAR (250)  NULL,
    [TenantID]       NVARCHAR (128)  NULL,
    [Created]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT             DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);






GO

GO

GO
/****** Object:  Index [RoleNameIndex]    Script Date: 3/13/2017 5:14:58 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[Name] ASC,
	[TenantID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]