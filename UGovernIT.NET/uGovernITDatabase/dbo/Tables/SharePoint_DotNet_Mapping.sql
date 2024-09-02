CREATE TABLE [dbo].[SharePoint_DotNet_Mapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SPColumn] [varchar](100) NULL,
	[DTColumn] [varchar](100) NULL,
 CONSTRAINT [PK_SharePoint_DotNet_Mapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SharePoint_DotNet_Mapping] ON 
GO
INSERT [dbo].[SharePoint_DotNet_Mapping] ([Id], [SPColumn], [DTColumn]) VALUES (1, N'TicketCreationDate', N'CreationDate')
GO
INSERT [dbo].[SharePoint_DotNet_Mapping] ([Id], [SPColumn], [DTColumn]) VALUES (2, N'TicketPriorityLookup', N'PriorityLookup')
GO
INSERT [dbo].[SharePoint_DotNet_Mapping] ([Id], [SPColumn], [DTColumn]) VALUES (3, N'TicketRequestTypeLookup', N'RequestTypeLookup')
GO
INSERT [dbo].[SharePoint_DotNet_Mapping] ([Id], [SPColumn], [DTColumn]) VALUES (4, N'TicketRequestSource', N'RequestSource')
GO
INSERT [dbo].[SharePoint_DotNet_Mapping] ([Id], [SPColumn], [DTColumn]) VALUES (5, N'GenericStatusLookup', N'Status')
GO
INSERT [dbo].[SharePoint_DotNet_Mapping] ([Id], [SPColumn], [DTColumn]) VALUES (6, N'TicketOwner', N'OwnerUser')
GO
INSERT [dbo].[SharePoint_DotNet_Mapping] ([Id], [SPColumn], [DTColumn]) VALUES (7, N'TicketPRP', N'PRPUser')
GO
SET IDENTITY_INSERT [dbo].[SharePoint_DotNet_Mapping] OFF
GO
