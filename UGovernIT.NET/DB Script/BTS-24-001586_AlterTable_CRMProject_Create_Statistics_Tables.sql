ALTER TABLE [dbo].[CRMProject]
Add NumTags NVARCHAR(15) NULL
  , NumUniqueRoles NVARCHAR(15) NULL
  , NumAllocations NVARCHAR(15) NULL
  , NumContractAmountChanges NVARCHAR(15) NULL
  , PerContractAmountChange NVARCHAR(15) NULL
  , NumResourceAllocationChanges NVARCHAR(15) NULL
  , NumScheduleChanges NVARCHAR(15) NULL
  , PerScheduleChanges NVARCHAR(15) NULL
  , PerTimeTakenToFillUnfilledRoles NVARCHAR(15) NULL


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StatisticsConfiguration](
  [ID] [bigint] IDENTITY(1,1) NOT NULL,
  [ModuleName] [nvarchar](10) NOT NULL,
  [FieldName] [nvarchar](60) NOT NULL,
 CONSTRAINT [PK_StatisticsConfiguration] PRIMARY KEY CLUSTERED 
(
  [ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[StatisticsConfiguration] ON 
GO
INSERT [dbo].[StatisticsConfiguration] ([ID], [ModuleName], [FieldName]) VALUES (1, N'CPR', N'ApproxContractValue')
GO
INSERT [dbo].[StatisticsConfiguration] ([ID], [ModuleName], [FieldName]) VALUES (2, N'CPR', N'CRMDuration')
GO
SET IDENTITY_INSERT [dbo].[StatisticsConfiguration] OFF
GO


CREATE TABLE [dbo].[Statistics](
  [ID] [bigint] IDENTITY(1,1) NOT NULL,
  [TicketID] [nvarchar](50) NOT NULL,
  [FieldName] [nvarchar](60) NOT NULL,
  [Value] [nvarchar](500) NULL,
  [Date] [datetime] NOT NULL,
  [CreatedBy] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_Statistics] PRIMARY KEY CLUSTERED 
(
  [ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO  