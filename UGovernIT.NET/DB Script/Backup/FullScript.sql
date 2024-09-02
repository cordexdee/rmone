USE [master]
GO
/****** Object:  Database [uGovernITFinal]    Script Date: 22-02-2017 17:01:11 ******/
CREATE DATABASE [uGovernITFinal]
 GO
ALTER DATABASE [uGovernITFinal] SET COMPATIBILITY_LEVEL = 110
GO

ALTER DATABASE [uGovernITFinal] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [uGovernITFinal] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [uGovernITFinal] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [uGovernITFinal] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [uGovernITFinal] SET ARITHABORT OFF 
GO
ALTER DATABASE [uGovernITFinal] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [uGovernITFinal] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [uGovernITFinal] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [uGovernITFinal] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [uGovernITFinal] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [uGovernITFinal] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [uGovernITFinal] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [uGovernITFinal] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [uGovernITFinal] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [uGovernITFinal] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [uGovernITFinal] SET  DISABLE_BROKER 
GO
ALTER DATABASE [uGovernITFinal] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [uGovernITFinal] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [uGovernITFinal] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [uGovernITFinal] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [uGovernITFinal] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [uGovernITFinal] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [uGovernITFinal] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [uGovernITFinal] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [uGovernITFinal] SET  MULTI_USER 
GO
ALTER DATABASE [uGovernITFinal] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [uGovernITFinal] SET DB_CHAINING OFF 
GO
ALTER DATABASE [uGovernITFinal] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [uGovernITFinal] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [uGovernITFinal]
GO
/****** Object:  StoredProcedure [dbo].[AddIDColumnUtility]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[AddIDColumnUtility]
as
  Declare @tableName varchar(250);
  Declare tableCursor Cursor for
      select TABLE_NAME from INFORMATION_SCHEMA.TABLES;
Begin
    Open tableCursor
	 Fetch NEXT from tableCursor into @tableName
	  While @@FETCH_STATUS=0
	   Begin
	     if not exists(select 1 from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME=@tableName and COLUMN_NAME='Title')
		 begin
		     Exec ('Alter Table ' + @tableName + ' Add Title varchar(250)');
		 End;
		 Fetch NEXT from tableCursor into @tableName;
	   End;
	   Close tableCursor
	   Deallocate tableCursor
End





GO
/****** Object:  StoredProcedure [dbo].[RenameColumn]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create Procedure [dbo].[RenameColumn]
as
  Declare @newTableName varchar(250);
  Declare @tableName varchar(250);
  Declare tableCursor Cursor for
      select TABLE_NAME from INFORMATION_SCHEMA.TABLES;
Begin
    Open tableCursor
	 Fetch NEXT from tableCursor into @tableName
	  While @@FETCH_STATUS=0
	   Begin
	     if exists(SELECT 1 FROM information_schema.tables tables join INFORMATION_SCHEMA.COLUMNS col  on tables.TABLE_NAME = @tableName where col.COLUMN_NAME='ServiceTitleLookUp')
		 begin
		     set @newTableName = @tableName + '.ServiceTitleLookUp';
		     exec sp_rename @newTableName,'ServiceLookUp','COLUMN';
		 End;
		 Fetch NEXT from tableCursor into @tableName;
	   End;
	   Close tableCursor
	   Deallocate tableCursor
End






GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ACR]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ACR](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ACRTypeTitleLookup] [bigint] NOT NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualHours] [int] NULL,
	[ActualStartDate] [datetime] NULL,
	[APPTitleLookup] [bigint] NOT NULL,
	[BAAnalysisHours] [int] NULL,
	[BATestingHours] [int] NULL,
	[BATotalHours] [int] NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CompanyTitleLookup] [bigint] NOT NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[CustomUGChoice01] [nvarchar](max) NULL,
	[CustomUGChoice02] [nvarchar](max) NULL,
	[CustomUGChoice03] [nvarchar](max) NULL,
	[CustomUGChoice04] [nvarchar](max) NULL,
	[CustomUGDate01] [datetime] NULL,
	[CustomUGDate02] [datetime] NULL,
	[CustomUGDate03] [datetime] NULL,
	[CustomUGDate04] [datetime] NULL,
	[CustomUGText01] [nvarchar](250) NULL,
	[CustomUGText02] [nvarchar](250) NULL,
	[CustomUGText03] [nvarchar](250) NULL,
	[CustomUGText04] [nvarchar](250) NULL,
	[CustomUGText05] [nvarchar](250) NULL,
	[CustomUGText06] [nvarchar](250) NULL,
	[CustomUGText07] [nvarchar](250) NULL,
	[CustomUGText08] [nvarchar](250) NULL,
	[CustomUGUser01] [nvarchar](max) NULL,
	[CustomUGUser02] [nvarchar](max) NULL,
	[CustomUGUser03] [nvarchar](max) NULL,
	[CustomUGUser04] [nvarchar](max) NULL,
	[CustomUGUserMulti01] [nvarchar](250) NULL,
	[CustomUGUserMulti02] [nvarchar](250) NULL,
	[CustomUGUserMulti03] [nvarchar](250) NULL,
	[CustomUGUserMulti04] [nvarchar](250) NULL,
	[DepartmentLookup] [bigint] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[DeskLocation] [nvarchar](250) NULL,
	[DeveloperCodingHours] [int] NULL,
	[DeveloperSupportHours] [int] NULL,
	[DeveloperTotalHours] [int] NULL,
	[DivisionLookup] [bigint] NOT NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[Duration] [int] NULL,
	[EstimatedHours] [int] NULL,
	[ExternalID] [nvarchar](250) NULL,
	[FunctionalAreaLookup] [bigint] NOT NULL,
	[History] [nvarchar](max) NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[Initiator] [nvarchar](max) NULL,
	[InitiatorResolved] [nvarchar](max) NULL,
	[IsBusinessImpactDocAttached] [bit] NULL,
	[IsPerformanceTestingDone] [bit] NULL,
	[IsPrivate] [bit] NULL,
	[IssueType] [nvarchar](250) NULL,
	[LocationLookup] [bigint] NOT NULL,
	[ManagerApprovalNeeded] [bit] NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NextSLATime] [datetime] NULL,
	[NextSLAType] [nvarchar](250) NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldTillDate] [datetime] NULL,
	[ORP] [nvarchar](250) NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[PRP] [nvarchar](max) NULL,
	[PRPGroup] [nvarchar](max) NULL,
	[ReopenCount] [int] NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestorContacted] [bit] NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeSubCategory] [nvarchar](250) NULL,
	[RequestTypeWorkflow] [nvarchar](250) NULL,
	[ResolutionComments] [nvarchar](max) NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[SeverityLookup] [bigint] NOT NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[Tester] [nvarchar](250) NULL,
	[TestingTotalHours] [int] NULL,
	[TicketId] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
	[TotalHours] [int] NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ACRTypes]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ACRTypes](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_ACRTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ADUserMapping]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ADUserMapping](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ADProperty] [nvarchar](250) NULL,
	[UserProperty] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AnalyticDashboards]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnalyticDashboards](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AnalyticID] [int] NULL,
	[AnalyticName] [nvarchar](250) NULL,
	[AnalyticVID] [int] NULL,
	[DashboardID] [int] NULL,
	[Title] [nvarchar](250) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ApplicationModules]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApplicationModules](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[APPTitleLookup] [bigint] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Owner] [nvarchar](250) NULL,
	[SupportedBy] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ApplicationPassword]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApplicationPassword](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[APPPasswordTitle] [nvarchar](250) NULL,
	[APPTitleLookup] [bigint] NOT NULL,
	[APPUserName] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[EncryptedPassword] [nvarchar](250) NULL,
	[Password] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ApplicationRole]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApplicationRole](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ApplicationRoleModuleLookup] [bigint] NOT NULL,
	[APPTitleLookup] [bigint] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Applications]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Applications](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AccessAdmin] [nvarchar](250) NULL,
	[AccessManageLevel] [nvarchar](max) NULL,
	[AllocatedSeats] [int] NULL,
	[ApprovalType] [nvarchar](max) NULL,
	[Approver] [nvarchar](250) NULL,
	[BuildNumber] [nvarchar](250) NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[CategoryName] [nvarchar](250) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[DepartmentLookup] [bigint] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[FunctionalAreaLookup] [bigint] NOT NULL,
	[History] [nvarchar](max) NULL,
	[Initiator] [nvarchar](max) NULL,
	[IssueTypeOptions] [nvarchar](max) NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NumberOfSeats] [int] NULL,
	[Owner] [nvarchar](250) NULL,
	[SoftwareKey] [nvarchar](250) NULL,
	[SoftwareMajorVersion] [nvarchar](250) NULL,
	[SoftwareMinorVersion] [nvarchar](250) NULL,
	[SoftwarePatchRevision] [nvarchar](250) NULL,
	[SoftwareVersion] [nvarchar](250) NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[SubCategory] [nvarchar](250) NULL,
	[SupportedBy] [nvarchar](250) NULL,
	[SyncAtModuleLevel] [bit] NULL,
	[SyncToRequestType] [bit] NULL,
	[TicketId] [nvarchar](250) NULL,
	[VendorName] [nvarchar](250) NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ApplicationServers]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApplicationServers](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[APPTitleLookup] [bigint] NOT NULL,
	[AssetsTitleLookup] [bigint] NOT NULL,
	[EnvironmentLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ApplModuleRoleRelationship]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApplModuleRoleRelationship](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ApplicationModulesLookup] [bigint] NOT NULL,
	[ApplicationRoleAssign] [nvarchar](max) NULL,
	[ApplicationRoleLookup] [bigint] NOT NULL,
	[APPTitleLookup] [bigint] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Title] [varchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[Discriminator] [nvarchar](max) NULL,
	[IsSystem] [bit] NOT NULL DEFAULT ((0)),
	[RoleType] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[Name] [varchar](100) NULL,
	[Department] [varchar](100) NULL,
	[HourlyRate] [int] NULL,
	[Location] [varchar](100) NULL,
	[Manager] [varchar](100) NULL,
	[MobilePhone] [varchar](100) NULL,
	[JobProfile] [varchar](100) NULL,
	[IsIT] [bit] NULL,
	[IsConsultant] [bit] NULL,
	[IsManager] [bit] NULL,
	[LocationId] [int] NULL,
	[DepartmentId] [int] NULL,
	[Enabled] [bit] NOT NULL DEFAULT ((1)),
	[FunctionalArea] [int] NULL,
	[BudgetCategory] [int] NULL,
	[DeskLocation] [varchar](100) NULL,
	[UGITStartDate] [datetime] NULL,
	[UGITEndDate] [datetime] NULL,
	[EnablePasswordExpiration] [bit] NULL,
	[PasswordExpiryDate] [datetime] NULL,
	[DisableWorkflowNotifications] [bit] NULL,
	[Picture] [varchar](500) NULL,
	[NotificationEmail] [varchar](256) NULL,
	[ApproveLevelAmount] [float] NULL,
	[LeaveFromDate] [datetime] NULL,
	[LeaveToDate] [datetime] NULL,
	[EnableOutofOffice] [bit] NULL,
	[ManagerID] [nvarchar](100) NULL,
	[DelegateUserOnLeave] [varchar](500) NULL,
	[DelegateUserFor] [varchar](500) NULL,
	[isRole] [bit] NOT NULL DEFAULT ((1)),
	[Skills] [nvarchar](max) NULL,
	[WorkingHoursStart] [datetime] NULL,
	[WorkingHoursEnd] [datetime] NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AssetIncidentRelations]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssetIncidentRelations](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssetTagNumLookup] [bigint] NOT NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AssetModels]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssetModels](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BudgetLookup] [bigint] NULL,
	[IsDeleted] [bit] NULL,
	[ModelDescription] [nvarchar](250) NULL,
	[ModelName] [nvarchar](250) NULL,
	[VendorLookup] [bigint] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AssetReferences]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssetReferences](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssetLookup] [bigint] NOT NULL,
	[AssetReferenceNum] [nvarchar](250) NULL,
	[AssetReferenceType] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AssetRelations]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssetRelations](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Asset1] [nvarchar](250) NULL,
	[Asset2] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Assets]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Assets](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AcquisitionDate] [datetime] NULL,
	[ActualReplacementDate] [datetime] NULL,
	[AdditionalKey] [nvarchar](250) NULL,
	[ApplicationMultiLookup] [bigint] NOT NULL,
	[AssetDescription] [nvarchar](max) NULL,
	[AssetModelLookup] [bigint] NOT NULL,
	[AssetName] [nvarchar](250) NULL,
	[AssetOwner] [nvarchar](max) NULL,
	[AssetsStatusLookup] [bigint] NOT NULL,
	[AssetTagNum] [nvarchar](250) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[Cost] [nvarchar](250) NULL,
	[CPU] [nvarchar](250) NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[DataRetention] [int] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletionDate] [datetime] NULL,
	[DepartmentLookup] [bigint] NOT NULL,
	[EndDate] [datetime] NULL,
	[HardDrive1] [int] NULL,
	[HardDrive2] [int] NULL,
	[History] [nvarchar](max) NULL,
	[HostName] [nvarchar](250) NULL,
	[ImageInstallDate] [datetime] NULL,
	[ImageOptionLookup] [bigint] NOT NULL,
	[InstalledBy] [nvarchar](max) NULL,
	[InstalledDate] [datetime] NULL,
	[IPAddress] [nvarchar](250) NULL,
	[IsDeleted] [bit] NULL,
	[LicenseKey] [nvarchar](250) NULL,
	[LicenseType] [nvarchar](250) NULL,
	[LocationLookup] [bigint] NOT NULL,
	[Manufacturer] [nvarchar](max) NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NICAddress] [nvarchar](250) NULL,
	[OS] [nvarchar](250) NULL,
	[OSKey] [nvarchar](250) NULL,
	[PO] [int] NULL,
	[PreAcquired] [bit] NULL,
	[PreviousOwner1] [nvarchar](max) NULL,
	[PreviousOwner2] [nvarchar](max) NULL,
	[PreviousOwner3] [nvarchar](max) NULL,
	[PreviousUser] [nvarchar](250) NULL,
	[PurchasedBy] [nvarchar](max) NULL,
	[RAM] [int] NULL,
	[RegisteredBy] [nvarchar](max) NULL,
	[RegistrationDate] [datetime] NULL,
	[RenewalDate] [datetime] NULL,
	[ReplacementAsset_SNLookup] [bigint] NOT NULL,
	[ReplacementDate] [datetime] NULL,
	[ReplacementDeliveryDate] [datetime] NULL,
	[ReplacementOrderedDate] [datetime] NULL,
	[ReplacementType] [nvarchar](max) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeSubCategory] [nvarchar](250) NULL,
	[ResaleValue] [int] NULL,
	[ResoldFor] [nvarchar](250) NULL,
	[ResoldTo] [nvarchar](250) NULL,
	[RetiredDate] [datetime] NULL,
	[SaleDate] [datetime] NULL,
	[ScheduleStatus] [nvarchar](max) NULL,
	[SerialAssetDetail] [nvarchar](250) NULL,
	[SerialNum1] [nvarchar](250) NULL,
	[SerialNum1Description] [nvarchar](250) NULL,
	[SerialNum2] [nvarchar](250) NULL,
	[SerialNum2Description] [nvarchar](250) NULL,
	[SerialNum3] [nvarchar](250) NULL,
	[SerialNum3Description] [nvarchar](250) NULL,
	[SetupCompletedBy] [nvarchar](max) NULL,
	[SetupCompletedDate] [datetime] NULL,
	[SoftwareLookup] [bigint] NOT NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[StartDate] [datetime] NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChangeDate] [datetime] NULL,
	[Supplier] [nvarchar](max) NULL,
	[SupportNumber] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[TransferDate] [datetime] NULL,
	[TSRIdLookup] [bigint] NOT NULL,
	[UninstallDate] [datetime] NULL,
	[Upgrade] [nvarchar](max) NULL,
	[VendorLookup] [bigint] NOT NULL,
	[WarrantyExpirationDate] [datetime] NULL,
	[WarrantyType] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AssetsStatus]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssetsStatus](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_AssetsStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AssetVendors]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssetVendors](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactName] [nvarchar](250) NULL,
	[IsDeleted] [bit] NULL,
	[VendorAddress] [nvarchar](max) NULL,
	[VendorEmail] [nvarchar](250) NULL,
	[VendorLocation] [nvarchar](250) NULL,
	[VendorName] [nvarchar](250) NULL,
	[VendorPhone] [nvarchar](250) NULL,
	[WebsiteUrl] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
	[Vendortype] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
SET ANSI_PADDING OFF
ALTER TABLE [dbo].[AssetVendors] ADD [ProductServiceDesc] [varchar](500) NULL
ALTER TABLE [dbo].[AssetVendors] ADD [VendorTimeZone] [varchar](3) NULL
ALTER TABLE [dbo].[AssetVendors] ADD [VendorSupportHours] [varchar](100) NULL
ALTER TABLE [dbo].[AssetVendors] ADD [VendorSupportCredentials] [varchar](100) NULL
ALTER TABLE [dbo].[AssetVendors] ADD [VendorAccountRepPhone] [varchar](100) NULL
ALTER TABLE [dbo].[AssetVendors] ADD [VendorAccountRepEmail] [varchar](100) NULL
ALTER TABLE [dbo].[AssetVendors] ADD [VendorAccountRepMobile] [varchar](100) NULL

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AvailableManagedServices]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AvailableManagedServices](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ServiceChargeType] [nvarchar](max) NULL,
	[ServiceDescription] [nvarchar](max) NULL,
	[ServiceFee] [nvarchar](250) NULL,
	[ServiceFrequency] [nvarchar](max) NULL,
	[ServiceName] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
	[VendorSOWLookup] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Bid]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bid](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BidAmount] [nvarchar](250) NULL,
	[BidArea] [nvarchar](max) NULL,
	[BidDate] [datetime] NULL,
	[BidSequence] [int] NULL,
	[ContactLookup] [bigint] NOT NULL,
	[OrganizationLookup] [bigint] NOT NULL,
	[TargetAmount] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BTS]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BTS](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualHours] [int] NULL,
	[ActualStartDate] [datetime] NULL,
	[ApplicationModule] [nvarchar](250) NULL,
	[ApplicationName] [nvarchar](250) NULL,
	[Category] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CompanyTitleLookup] [bigint] NOT NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[CustomUGChoice01] [nvarchar](max) NULL,
	[CustomUGChoice02] [nvarchar](max) NULL,
	[CustomUGChoice03] [nvarchar](max) NULL,
	[CustomUGChoice04] [nvarchar](max) NULL,
	[CustomUGDate01] [datetime] NULL,
	[CustomUGDate02] [datetime] NULL,
	[CustomUGDate03] [datetime] NULL,
	[CustomUGDate04] [datetime] NULL,
	[CustomUGText01] [nvarchar](250) NULL,
	[CustomUGText02] [nvarchar](250) NULL,
	[CustomUGText03] [nvarchar](250) NULL,
	[CustomUGText04] [nvarchar](250) NULL,
	[CustomUGText05] [nvarchar](250) NULL,
	[CustomUGText06] [nvarchar](250) NULL,
	[CustomUGText07] [nvarchar](250) NULL,
	[CustomUGText08] [nvarchar](250) NULL,
	[CustomUGUser01] [nvarchar](max) NULL,
	[CustomUGUser02] [nvarchar](max) NULL,
	[CustomUGUser03] [nvarchar](max) NULL,
	[CustomUGUser04] [nvarchar](max) NULL,
	[CustomUGUserMulti01] [nvarchar](250) NULL,
	[CustomUGUserMulti02] [nvarchar](250) NULL,
	[CustomUGUserMulti03] [nvarchar](250) NULL,
	[CustomUGUserMulti04] [nvarchar](250) NULL,
	[DepartmentLookup] [bigint] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[DeskLocation] [nvarchar](250) NULL,
	[DivisionLookup] [bigint] NOT NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[EstimatedHours] [int] NULL,
	[FunctionalAreaLookup] [bigint] NOT NULL,
	[History] [nvarchar](max) NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[Initiator] [nvarchar](max) NULL,
	[InitiatorResolved] [nvarchar](max) NULL,
	[LocationLookup] [bigint] NOT NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NextSLATime] [datetime] NULL,
	[NextSLAType] [nvarchar](250) NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldTillDate] [datetime] NULL,
	[ORP] [nvarchar](250) NULL,
	[Owner] [nvarchar](250) NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[PRP] [nvarchar](max) NULL,
	[PRPGroup] [nvarchar](max) NULL,
	[RelatedRecords] [nvarchar](250) NULL,
	[ReopenCount] [int] NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeSubCategory] [nvarchar](250) NULL,
	[RequestTypeWorkflow] [nvarchar](250) NULL,
	[ResolutionComments] [nvarchar](max) NULL,
	[ResolutionType] [nvarchar](max) NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[SeverityLookup] [bigint] NOT NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[Tester] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
	[UserQuestionSummary] [nvarchar](max) NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ChartFilters]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChartFilters](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ColumnName] [nvarchar](250) NULL,
	[IsDefault] [bit] NULL,
	[ListName] [nvarchar](250) NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[ModuleType] [nvarchar](max) NULL,
	[ValueAsId] [bit] NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ChartFormula]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChartFormula](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ChartTemplateIds] [nvarchar](250) NULL,
	[Formula] [nvarchar](250) NULL,
	[FormulaValue] [nvarchar](250) NULL,
	[IsDefault] [bit] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[ModuleType] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ChartTemplates]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChartTemplates](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ChartObject] [nvarchar](250) NULL,
	[TemplateType] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Company]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Company](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[GLCode] [nvarchar](250) NULL,
	[IsDeleted] [bit] NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CompanyDivisions]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyDivisions](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyIdLookup] [bigint] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[GLCode] [nvarchar](250) NULL,
	[IsDeleted] [bit] NULL,
	[Title] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Config_BudgetCategories]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_BudgetCategories](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AuthorizedToEdit] [nvarchar](250) NULL,
	[AuthorizedToView] [nvarchar](250) NULL,
	[BudgetAcronym] [nvarchar](250) NULL,
	[BudgetCategory] [nvarchar](250) NULL,
	[BudgetCOA] [nvarchar](250) NULL,
	[BudgetDescription] [nvarchar](max) NULL,
	[BudgetSubCategory] [nvarchar](250) NULL,
	[BudgetType] [nvarchar](250) NULL,
	[BudgetTypeCOA] [nvarchar](250) NULL,
	[CapitalExpenditure] [bit] NULL,
	[IncludesStaffing] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_ClientAdminCategory]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_ClientAdminCategory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](250) NULL,
	[ImageUrl] [nvarchar](250) NULL,
	[ItemOrder] [int] NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_Config_ClientAdminCategory_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_ClientAdminConfigurationLists]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_ClientAdminConfigurationLists](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ClientAdminCategoryLookup] [bigint] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ListName] [nvarchar](250) NULL,
	[TabSequence] [int] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_ConfigurationVariable]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_ConfigurationVariable](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[KeyName] [nvarchar](250) NULL,
	[KeyValue] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Dashboard_DashboardFactTables]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Config_Dashboard_DashboardFactTables](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CacheAfter] [int] NULL,
	[CacheMode] [nvarchar](max) NULL,
	[CacheTable] [bit] NULL,
	[CacheThreshold] [int] NULL,
	[DashboardPanelInfo] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[ExpiryDate] [datetime] NULL,
	[LastUpdated] [datetime] NULL,
	[RefreshMode] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[Title] [nvarchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Config_Dashboard_DashboardPanels]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Dashboard_DashboardPanels](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AuthorizedToView] [nvarchar](250) NULL,
	[CategoryName] [nvarchar](250) NULL,
	[DashboardDescription] [nvarchar](max) NULL,
	[DashboardModuleMultiLookup] [bigint] NOT NULL,
	[DashboardPanelInfo] [nvarchar](max) NULL,
	[DashboardPermission] [nvarchar](max) NULL,
	[DashboardType] [nvarchar](max) NULL,
	[FontStyle] [nvarchar](250) NULL,
	[HeaderFontStyle] [nvarchar](250) NULL,
	[IsActivated] [bit] NULL,
	[IsHideDescription] [bit] NULL,
	[IsHideTitle] [bit] NULL,
	[IsShowInSideBar] [bit] NULL,
	[ItemOrder] [int] NULL,
	[PanelHeight] [int] NULL,
	[PanelWidth] [int] NULL,
	[SubCategory] [nvarchar](250) NULL,
	[ThemeColor] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Dashboard_DashboardPanelView]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Dashboard_DashboardPanelView](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DashboardPanelInfo] [nvarchar](max) NULL,
	[ViewType] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_EventCategories]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Config_EventCategories](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[ItemColor] [nvarchar](250) NULL,
	[ItemOrder] [int] NULL,
	[Title] [nvarchar](250) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Config_MailTokenColumnName]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_MailTokenColumnName](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[KeyName] [nvarchar](250) NULL,
	[KeyValue] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_MenuNavigation]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_MenuNavigation](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AuthorizedToView] [nvarchar](250) NULL,
	[CustomizeFormat] [bit] NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[MenuHeight] [int] NULL,
	[IconUrl] [nvarchar](250) NULL,
	[IsDisabled] [bit] NULL,
	[ItemOrder] [int] NULL,
	[MenuBackground] [nvarchar](250) NULL,
	[MenuDisplayType] [nvarchar](max) NULL,
	[MenuFontColor] [nvarchar](250) NULL,
	[MenuItemSeparation] [nvarchar](250) NULL,
	[MenuName] [nvarchar](250) NULL,
	[MenuParentLookup] [bigint] NOT NULL,
	[NavigationType] [nvarchar](max) NULL,
	[NavigationUrl] [nvarchar](max) NULL,
	[SubMenuItemAlignment] [nvarchar](250) NULL,
	[SubMenuItemPerRow] [int] NULL,
	[SubMenuStyle] [nvarchar](max) NULL,
	[TextMenuAlignment] [nvarchar](max) NULL,
	[MenuWidth] [int] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_MenuNavigation_bkp]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_MenuNavigation_bkp](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AuthorizedToView] [nvarchar](250) NULL,
	[CustomizeFormat] [bit] NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[MenuHeight] [int] NULL,
	[IconUrl] [nvarchar](250) NULL,
	[IsDisabled] [bit] NULL,
	[ItemOrder] [int] NULL,
	[MenuBackground] [nvarchar](250) NULL,
	[MenuDisplayType] [nvarchar](max) NULL,
	[MenuFontColor] [nvarchar](250) NULL,
	[MenuItemSeparation] [nvarchar](250) NULL,
	[MenuName] [nvarchar](250) NULL,
	[MenuParentLookup] [bigint] NOT NULL,
	[NavigationType] [nvarchar](max) NULL,
	[NavigationUrl] [nvarchar](max) NULL,
	[SubMenuItemAlignment] [nvarchar](250) NULL,
	[SubMenuItemPerRow] [int] NULL,
	[SubMenuStyle] [nvarchar](max) NULL,
	[TextMenuAlignment] [nvarchar](max) NULL,
	[MenuWidth] [int] NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_MenuNavigation_bkp1]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_MenuNavigation_bkp1](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AuthorizedToView] [nvarchar](250) NULL,
	[CustomizeFormat] [bit] NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[MenuHeight] [int] NULL,
	[IconUrl] [nvarchar](250) NULL,
	[IsDisabled] [bit] NULL,
	[ItemOrder] [int] NULL,
	[MenuBackground] [nvarchar](250) NULL,
	[MenuDisplayType] [nvarchar](max) NULL,
	[MenuFontColor] [nvarchar](250) NULL,
	[MenuItemSeparation] [nvarchar](250) NULL,
	[MenuName] [nvarchar](250) NULL,
	[MenuParentLookup] [bigint] NOT NULL,
	[NavigationType] [nvarchar](max) NULL,
	[NavigationUrl] [nvarchar](max) NULL,
	[SubMenuItemAlignment] [nvarchar](250) NULL,
	[SubMenuItemPerRow] [int] NULL,
	[SubMenuStyle] [nvarchar](max) NULL,
	[MenuTextAlignment] [nvarchar](max) NULL,
	[MenuWidth] [int] NULL,
	[Title] [varchar](250) NULL,
	[MenuFontSize] [int] NULL,
	[MenuFontFontFamily] [varchar](250) NULL,
	[MenuIconAlignment] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[config_menuNavigation_temp]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[config_menuNavigation_temp](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AuthorizedToView] [nvarchar](250) NULL,
	[CustomizeFormat] [bit] NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[MenuHeight] [int] NULL,
	[IconUrl] [nvarchar](250) NULL,
	[IsDisabled] [bit] NULL,
	[ItemOrder] [int] NULL,
	[MenuBackground] [nvarchar](250) NULL,
	[MenuDisplayType] [nvarchar](max) NULL,
	[MenuFontColor] [nvarchar](250) NULL,
	[MenuItemSeparation] [nvarchar](250) NULL,
	[MenuName] [nvarchar](250) NULL,
	[MenuParentLookup] [bigint] NOT NULL,
	[NavigationType] [nvarchar](max) NULL,
	[NavigationUrl] [nvarchar](max) NULL,
	[SubMenuItemAlignment] [nvarchar](250) NULL,
	[SubMenuItemPerRow] [int] NULL,
	[SubMenuStyle] [nvarchar](max) NULL,
	[TextMenuAlignment] [nvarchar](max) NULL,
	[MenuWidth] [int] NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_EscalationRule]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_EscalationRule](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EscalationDescription] [nvarchar](max) NULL,
	[EscalationEmailBody] [nvarchar](max) NULL,
	[EscalationFrequency] [int] NULL,
	[EscalationMailSubject] [nvarchar](250) NULL,
	[EscalationMinutes] [int] NULL,
	[EscalationToEmails] [nvarchar](250) NULL,
	[EscalationToRoles] [nvarchar](250) NULL,
	[IncludeActionUsers] [bit] NULL,
	[SLARuleIdLookup] [bigint] NOT NULL,
	[UseDesiredCompletionDate] [bit] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_FormLayout]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_FormLayout](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ColumnType] [nvarchar](250) NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[FieldDisplayName] [nvarchar](250) NULL,
	[FieldDisplayWidth] [int] NULL,
	[FieldName] [nvarchar](250) NULL,
	[FieldSequence] [int] NULL,
	[FieldType] [nvarchar](250) NULL,
	[HideInTemplate] [bit] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[ShowInMobile] [bit] NULL,
	[SkipOnCondition] [nvarchar](max) NULL,
	[TabId] [int] NULL,
	[TargetType] [nvarchar](250) NULL,
	[TargetURL] [nvarchar](250) NULL,
	[ToolTip] [nvarchar](250) NULL,
	[TrimContentAfter] [int] NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_Impact]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_Impact](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Impact] [nvarchar](250) NULL,
	[IsDeleted] [bit] NULL,
	[ItemOrder] [int] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_Config_Module_Impact] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_ModuleColumns]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_ModuleColumns](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ColumnType] [nvarchar](250) NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[DisplayForClosed] [bit] NULL,
	[DisplayForReport] [bit] NULL,
	[FieldDisplayName] [nvarchar](250) NULL,
	[FieldName] [nvarchar](250) NULL,
	[FieldSequence] [int] NULL,
	[IsAscending] [bit] NULL,
	[IsDisplay] [bit] NULL,
	[IsUseInWildCard] [bit] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[ShowInMobile] [bit] NULL,
	[SortOrder] [int] NULL,
	[TextAlignment] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_ModuleColumns_bkp]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_ModuleColumns_bkp](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ColumnType] [nvarchar](250) NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[DisplayForClosed] [bit] NULL,
	[DisplayForReport] [bit] NULL,
	[FieldDisplayName] [nvarchar](250) NULL,
	[FieldName] [nvarchar](250) NULL,
	[FieldSequence] [int] NULL,
	[IsAscending] [bit] NULL,
	[IsDisplay] [bit] NULL,
	[IsUseInWildCard] [bit] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[ShowInMobile] [bit] NULL,
	[SortOrder] [int] NULL,
	[TextAlignment] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_ModuleFormTab]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_ModuleFormTab](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AuthorizedToEdit] [nvarchar](250) NULL,
	[AuthorizedToView] [nvarchar](250) NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[TabId] [int] NULL,
	[TabName] [nvarchar](250) NULL,
	[TabSequence] [int] NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_ModuleStages]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_ModuleStages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Action] [nvarchar](250) NULL,
	[ActionUser] [nvarchar](250) NULL,
	[AllowReassignFromList] [bit] NULL,
	[ApproveActionDescription] [nvarchar](250) NULL,
	[ApproveButtonTooltip] [nvarchar](250) NULL,
	[ApproveIcon] [nvarchar](250) NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[EnableCustomReturn] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[RejectActionDescription] [nvarchar](250) NULL,
	[RejectButtonTooltip] [nvarchar](250) NULL,
	[RejectIcon] [nvarchar](250) NULL,
	[ReturnActionDescription] [nvarchar](250) NULL,
	[ReturnButtonTooltip] [nvarchar](250) NULL,
	[ReturnIcon] [nvarchar](250) NULL,
	[SelectedTabNumber] [int] NULL,
	[ShortStageTitle] [nvarchar](250) NULL,
	[ShowBaselineButtons] [bit] NULL,
	[SkipOnCondition] [nvarchar](max) NULL,
	[StageAllApprovalsRequired] [int] NULL,
	[StageApproveButtonName] [nvarchar](250) NULL,
	[StageApprovedStatus] [nvarchar](250) NULL,
	[StageCapacityMax] [int] NULL,
	[StageCapacityNormal] [int] NULL,
	[StageRejectedButtonName] [nvarchar](250) NULL,
	[StageRejectedStatus] [nvarchar](250) NULL,
	[StageReturnButtonName] [nvarchar](250) NULL,
	[StageReturnStatus] [nvarchar](250) NULL,
	[StageTitle] [nvarchar](250) NULL,
	[StageTypeLookup] [int] NOT NULL,
	[StageWeight] [int] NULL,
	[UserPrompt] [nvarchar](250) NULL,
	[UserWorkflowStatus] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
	[Name] [varchar](250) NULL,
	[LifeCycleName] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_ModuleStages_bkp]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_ModuleStages_bkp](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Action] [nvarchar](250) NULL,
	[ActionUser] [nvarchar](250) NULL,
	[AllowReassignFromList] [bit] NULL,
	[ApproveActionDescription] [nvarchar](250) NULL,
	[ApproveButtonTooltip] [nvarchar](250) NULL,
	[ApproveIcon] [nvarchar](250) NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[EnableCustomReturn] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[RejectActionDescription] [nvarchar](250) NULL,
	[RejectButtonTooltip] [nvarchar](250) NULL,
	[RejectIcon] [nvarchar](250) NULL,
	[ReturnActionDescription] [nvarchar](250) NULL,
	[ReturnButtonTooltip] [nvarchar](250) NULL,
	[ReturnIcon] [nvarchar](250) NULL,
	[SelectedTabNumber] [int] NULL,
	[ShortStageTitle] [nvarchar](250) NULL,
	[ShowBaselineButtons] [bit] NULL,
	[SkipOnCondition] [nvarchar](max) NULL,
	[StageAllApprovalsRequired] [int] NULL,
	[StageApproveButtonName] [nvarchar](250) NULL,
	[StageApprovedStatus] [nvarchar](250) NULL,
	[StageCapacityMax] [int] NULL,
	[StageCapacityNormal] [int] NULL,
	[StageRejectedButtonName] [nvarchar](250) NULL,
	[StageRejectedStatus] [nvarchar](250) NULL,
	[StageReturnButtonName] [nvarchar](250) NULL,
	[StageReturnStatus] [nvarchar](250) NULL,
	[StageTitle] [nvarchar](250) NULL,
	[StageTypeLookup] [int] NOT NULL,
	[StageWeight] [int] NULL,
	[UserPrompt] [nvarchar](250) NULL,
	[UserWorkflowStatus] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
	[Name] [varchar](250) NULL,
	[LifeCycleName] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_ModuleUserTypes]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_ModuleUserTypes](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ColumnName] [nvarchar](250) NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[DefaultUser] [nvarchar](max) NULL,
	[Groups] [nvarchar](250) NULL,
	[ITOnly] [bit] NULL,
	[ManagerOnly] [bit] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[UserTypes] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_Priority]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_Priority](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EmailIDTo] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
	[IsVIP] [bit] NULL,
	[ItemOrder] [int] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[uPriority] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_Config_Module_Priority] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_RequestPriority]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_RequestPriority](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[SeverityLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_RequestRoleWriteAccess]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_RequestRoleWriteAccess](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActionUser] [nvarchar](250) NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[FieldMandatory] [bit] NULL,
	[FieldName] [nvarchar](250) NULL,
	[HideInServiceMapping] [bit] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[ShowEditButton] [bit] NULL,
	[ShowWithCheckBox] [bit] NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_RequestType]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_RequestType](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ApplicationModulesLookup] [bigint] NULL,
	[AppReferenceInfo] [nvarchar](250) NULL,
	[APPTitleLookup] [bigint] NULL,
	[AssignmentSLA] [int] NULL,
	[AutoAssignPRP] [bit] NULL,
	[BudgetIdLookup] [bigint] NULL,
	[Category] [nvarchar](250) NULL,
	[CloseSLA] [int] NULL,
	[EstimatedHours] [int] NULL,
	[FunctionalAreaLookup] [bigint] NULL,
	[IsDeleted] [bit] NULL,
	[IssueTypeOptions] [nvarchar](max) NULL,
	[KeyWords] [nvarchar](max) NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[ORP] [nvarchar](max) NULL,
	[OutOfOffice] [bit] NULL,
	[PRPGroup] [nvarchar](max) NULL,
	[RequestCategory] [nvarchar](max) NULL,
	[RequestorContactSLA] [int] NULL,
	[RequestType] [nvarchar](250) NULL,
	[RequestTypeBackupEscalationManager] [nvarchar](max) NULL,
	[RequestTypeDescription] [nvarchar](max) NULL,
	[RequestTypeEscalationManager] [nvarchar](max) NULL,
	[RequestTypeOwner] [nvarchar](250) NULL,
	[RequestTypeSubCategory] [nvarchar](250) NULL,
	[ResolutionSLA] [int] NULL,
	[ResolutionTypes] [nvarchar](max) NULL,
	[ServiceWizardOnly] [bit] NULL,
	[SortToBottom] [bit] NULL,
	[StagingId] [int] NULL,
	[TaskTemplateLookup] [bigint] NULL,
	[WorkflowType] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_RequestType_bkp]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_RequestType_bkp](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ApplicationModulesLookup] [bigint] NULL,
	[AppReferenceInfo] [nvarchar](250) NULL,
	[APPTitleLookup] [bigint] NULL,
	[AssignmentSLA] [int] NULL,
	[AutoAssignPRP] [bit] NULL,
	[BudgetIdLookup] [bigint] NULL,
	[Category] [nvarchar](250) NULL,
	[CloseSLA] [int] NULL,
	[EstimatedHours] [int] NULL,
	[FunctionalAreaLookup] [bigint] NULL,
	[IsDeleted] [bit] NULL,
	[IssueTypeOptions] [nvarchar](max) NULL,
	[KeyWords] [nvarchar](max) NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[ORP] [nvarchar](max) NULL,
	[OutOfOffice] [bit] NULL,
	[PRPGroup] [nvarchar](max) NULL,
	[RequestCategory] [nvarchar](max) NULL,
	[RequestorContactSLA] [int] NULL,
	[RequestType] [nvarchar](250) NULL,
	[RequestTypeBackupEscalationManager] [nvarchar](max) NULL,
	[RequestTypeDescription] [nvarchar](max) NULL,
	[RequestTypeEscalationManager] [nvarchar](max) NULL,
	[RequestTypeOwner] [nvarchar](250) NULL,
	[RequestTypeSubCategory] [nvarchar](250) NULL,
	[ResolutionSLA] [int] NULL,
	[ResolutionTypes] [nvarchar](max) NULL,
	[ServiceWizardOnly] [bit] NULL,
	[SortToBottom] [bit] NULL,
	[StagingId] [int] NULL,
	[TaskTemplateLookup] [bigint] NULL,
	[WorkflowType] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_Severity]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_Severity](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[ItemOrder] [int] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[Severity] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_Config_Module_Severity] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_SLARule]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_SLARule](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EndStageStep] [int] NULL,
	[EndStageTitleLookup] [bigint] NOT NULL,
	[IsDeleted] [bit] NULL,
	[ModuleDescription] [nvarchar](max) NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[SLACategory] [nvarchar](max) NULL,
	[SLADaysRoundUpDown] [nvarchar](max) NULL,
	[SLAHours] [int] NULL,
	[SLATarget] [int] NULL,
	[StageTitleLookup] [bigint] NOT NULL,
	[StagingId] [int] NULL,
	[StartStageStep] [int] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_StageType]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_StageType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleStageType] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_StatusMapping]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_StatusMapping](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[GenericStatusLookup] [int] NOT NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[StageTitleLookup] [int] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Module_TaskEmails]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Module_TaskEmails](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[EmailBody] [nvarchar](max) NULL,
	[EmailIDCC] [nvarchar](max) NULL,
	[EmailTitle] [nvarchar](250) NULL,
	[EmailUserTypes] [nvarchar](250) NULL,
	[HideFooter] [bit] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[SendEvenIfStageSkipped] [bit] NULL,
	[Status] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_ModuleMonitorOptions]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_ModuleMonitorOptions](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ModuleMonitorMultiplier] [int] NULL,
	[ModuleMonitorNameLookup] [nvarchar](250) NOT NULL,
	[ModuleMonitorOption] [nvarchar](250) NULL,
	[ModuleMonitorOptionLEDClass] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_ModuleMonitors]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_ModuleMonitors](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDefault] [bit] NULL,
	[ModuleMonitorOptionNameLookup] [bigint] NOT NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[MonitorName] [nvarchar](250) NOT NULL,
	[ShortName] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_Config_ModuleMonitors_MonitorName] PRIMARY KEY CLUSTERED 
(
	[MonitorName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Modules]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Modules](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActionUserNotificationOnCancel] [bit] NULL,
	[ActionUserNotificationOnComment] [bit] NULL,
	[AllowBatchClose] [bit] NULL,
	[AllowBatchCreate] [bit] NULL,
	[AllowBatchEditing] [bit] NULL,
	[AllowChangeType] [bit] NULL,
	[AllowDelete] [bit] NULL,
	[AllowDraftMode] [bit] NULL,
	[AllowEscalationFromList] [bit] NULL,
	[AllowReassignFromList] [bit] NULL,
	[AuthorizedToCreate] [nvarchar](250) NULL,
	[AuthorizedToView] [nvarchar](250) NULL,
	[AutoCreateDocumentLibrary] [bit] NULL,
	[CategoryName] [nvarchar](250) NULL,
	[CloseChart] [nvarchar](250) NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[DisableNewConfirmation] [bit] NULL,
	[EnableCache] [bit] NULL,
	[EnableEventReceivers] [bit] NULL,
	[EnableLayout] [bit] NULL,
	[EnableModule] [bit] NULL,
	[EnableModuleAgent] [bit] NULL,
	[EnableNewsOnHomePage] [bit] NULL,
	[EnableQuick] [bit] NULL,
	[EnableRMMAllocation] [bit] NULL,
	[EnableWorkflow] [bit] NULL,
	[HideWorkFlow] [bit] NULL,
	[InitiatorNotificationOnCancel] [bit] NULL,
	[InitiatorNotificationOnComment] [bit] NULL,
	[ItemOrder] [int] NULL,
	[KeepItemOpen] [bit] NULL,
	[LastSequence] [int] NULL,
	[LastSequenceDate] [datetime] NULL,
	[ModuleAutoApprove] [bit] NULL,
	[ModuleDescription] [nvarchar](max) NULL,
	[ModuleHoldMaxStage] [int] NULL,
	[ModuleId] [int] NULL,
	[ModuleName] [nvarchar](250) NOT NULL,
	[ModuleRelativePagePath] [nvarchar](250) NULL,
	[ModuleTable] [nvarchar](250) NULL,
	[ModuleType] [nvarchar](max) NULL,
	[NavigationUrl] [nvarchar](max) NULL,
	[OpenChart] [nvarchar](250) NULL,
	[OwnerBinding] [nvarchar](max) NULL,
	[PreloadAllModuleTabs] [bit] NULL,
	[ReloadCache] [bit] NULL,
	[RequestorNotificationOnCancel] [bit] NULL,
	[RequestorNotificationOnComment] [bit] NULL,
	[ReturnCommentOptional] [bit] NULL,
	[ShortName] [nvarchar](250) NULL,
	[ShowBottleNeckChart] [bit] NULL,
	[ShowComment] [bit] NULL,
	[ShowNextSLA] [bit] NULL,
	[ShowSummary] [bit] NULL,
	[StaticModulePagePath] [nvarchar](250) NULL,
	[StoreEmails] [bit] NULL,
	[SyncAppsToRequestType] [bit] NULL,
	[ThemeColor] [nvarchar](max) NULL,
	[UseInGlobalSearch] [bit] NULL,
	[WaitingOnMeIncludesGroups] [bit] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ModuleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[config_modules_bkp]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[config_modules_bkp](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActionUserNotificationOnCancel] [bit] NULL,
	[ActionUserNotificationOnComment] [bit] NULL,
	[AllowBatchClose] [bit] NULL,
	[AllowBatchCreate] [bit] NULL,
	[AllowBatchEditing] [bit] NULL,
	[AllowChangeType] [bit] NULL,
	[AllowDelete] [bit] NULL,
	[AllowDraftMode] [bit] NULL,
	[AllowEscalationFromList] [bit] NULL,
	[AllowReassignFromList] [bit] NULL,
	[AuthorizedToCreate] [nvarchar](250) NULL,
	[AuthorizedToView] [nvarchar](250) NULL,
	[AutoCreateDocumentLibrary] [bit] NULL,
	[CategoryName] [nvarchar](250) NULL,
	[CloseChart] [nvarchar](250) NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[DisableNewConfirmation] [bit] NULL,
	[EnableCache] [bit] NULL,
	[EnableEventReceivers] [bit] NULL,
	[EnableLayout] [bit] NULL,
	[EnableModule] [bit] NULL,
	[EnableModuleAgent] [bit] NULL,
	[EnableNewsOnHomePage] [bit] NULL,
	[EnableQuick] [bit] NULL,
	[EnableRMMAllocation] [bit] NULL,
	[EnableWorkflow] [bit] NULL,
	[HideWorkFlow] [bit] NULL,
	[InitiatorNotificationOnCancel] [bit] NULL,
	[InitiatorNotificationOnComment] [bit] NULL,
	[ItemOrder] [int] NULL,
	[KeepItemOpen] [bit] NULL,
	[LastSequence] [int] NULL,
	[LastSequenceDate] [datetime] NULL,
	[ModuleAutoApprove] [bit] NULL,
	[ModuleDescription] [nvarchar](max) NULL,
	[ModuleHoldMaxStage] [int] NULL,
	[ModuleId] [int] NULL,
	[ModuleName] [nvarchar](250) NOT NULL,
	[ModuleRelativePagePath] [nvarchar](250) NULL,
	[ModuleTable] [nvarchar](250) NULL,
	[ModuleType] [nvarchar](max) NULL,
	[NavigationUrl] [nvarchar](max) NULL,
	[OpenChart] [nvarchar](250) NULL,
	[OwnerBinding] [nvarchar](max) NULL,
	[PreloadAllModuleTabs] [bit] NULL,
	[ReloadCache] [bit] NULL,
	[RequestorNotificationOnCancel] [bit] NULL,
	[RequestorNotificationOnComment] [bit] NULL,
	[ReturnCommentOptional] [bit] NULL,
	[ShortName] [nvarchar](250) NULL,
	[ShowBottleNeckChart] [bit] NULL,
	[ShowComment] [bit] NULL,
	[ShowNextSLA] [bit] NULL,
	[ShowSummary] [bit] NULL,
	[StaticModulePagePath] [nvarchar](250) NULL,
	[StoreEmails] [bit] NULL,
	[SyncAppsToRequestType] [bit] NULL,
	[ThemeColor] [nvarchar](max) NULL,
	[UseInGlobalSearch] [bit] NULL,
	[WaitingOnMeIncludesGroups] [bit] NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_MyModuleColumns]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_MyModuleColumns](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](250) NULL,
	[ColumnType] [nvarchar](250) NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[DisplayForClosed] [bit] NULL,
	[DisplayForReport] [bit] NULL,
	[FieldDisplayName] [nvarchar](250) NULL,
	[FieldName] [nvarchar](250) NULL,
	[FieldSequence] [int] NULL,
	[IsDisplay] [bit] NULL,
	[IsUseInWildCard] [bit] NULL,
	[ShowInMobile] [bit] NULL,
	[TextAlignment] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_PageConfiguration]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Config_PageConfiguration](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](250) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[LeftMenuName] [nvarchar](250) NULL,
	[LeftMenuType] [nvarchar](250) NULL,
	[HideLeftMenu] [bit] NULL,
	[HideTopMenu] [bit] NULL,
	[TopMenuName] [nvarchar](250) NULL,
	[TopMenuType] [nvarchar](250) NULL,
	[HideSearch] [bit] NULL,
	[HideFooter] [bit] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Config_ProjectClass]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_ProjectClass](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[ProjectNote] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_Config_ProjectClass] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_ProjectInitiative]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_ProjectInitiative](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[ProjectNote] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_Config_ProjectInitiative] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_ProjectLifeCycles]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Config_ProjectLifeCycles](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
	[ItemOrder] [int] NULL,
	[Title] [nvarchar](250) NULL,
 CONSTRAINT [PK_Config_ProjectLifeCycles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Config_ProjectLifeCycleStages]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Config_ProjectLifeCycleStages](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ModuleStep] [int] NULL,
	[ProjectLifeCycleLookup] [bigint] NOT NULL,
	[StageWeight] [int] NULL,
	[Title] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Config_Service_ServiceCategories]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Service_ServiceCategories](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](250) NULL,
	[ImageUrl] [nvarchar](250) NULL,
	[ItemOrder] [int] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Service_ServiceColumns]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Service_ServiceColumns](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ModuleIdLookup] [nvarchar](250) NOT NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Service_ServiceDefaultValues]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Service_ServiceDefaultValues](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ColumnName] [nvarchar](250) NULL,
	[ColumnValue] [nvarchar](max) NULL,
	[PickValueFrom] [nvarchar](250) NULL,
	[ServiceLookup] [bigint] NOT NULL,
	[ServiceQuestionTitleLookup] [bigint] NOT NULL,
	[ServiceTaskLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Service_ServiceQuestions]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Service_ServiceQuestions](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FieldMandatory] [bit] NULL,
	[ItemOrder] [int] NULL,
	[QuestionTypeProperties] [nvarchar](max) NULL,
	[ServiceSectionsTitleLookup] [bigint] NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[SWQuestion] [nvarchar](250) NULL,
	[SWQuestionType] [nvarchar](max) NULL,
	[TokenName] [nvarchar](250) NULL,
	[WebPartHelpText] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Service_ServiceRelationships]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Service_ServiceRelationships](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ApprovalStatus] [nvarchar](250) NULL,
	[Approver] [nvarchar](250) NULL,
	[AssignedTo] [nvarchar](250) NULL,
	[AutoCreateUser] [bit] NULL,
	[Body] [nvarchar](max) NULL,
	[DueDate] [datetime] NULL,
	[Duration] [int] NULL,
	[EnableApproval] [bit] NULL,
	[ItemOrder] [int] NULL,
	[Level] [int] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[Name] [nvarchar](250) NULL,
	[NewUserName] [nvarchar](250) NULL,
	[ParentTask] [int] NULL,
	[PercentComplete] [int] NULL,
	[Predecessors] [nvarchar](250) NULL,
	[Priority] [nvarchar](max) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeWorkflow] [nvarchar](250) NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[StageWeight] [int] NULL,
	[StartDate] [datetime] NULL,
	[Status] [nvarchar](max) NULL,
	[SubTaskType] [nvarchar](max) NULL,
	[TaskEstimatedHours] [int] NULL,
	[TaskGroup] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Service_ServiceSections]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Service_ServiceSections](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ItemOrder] [int] NULL,
	[SectionName] [nvarchar](250) NULL,
	[SectionSequence] [int] NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_Services]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_Services](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AllowServiceTasksInBackground] [bit] NULL,
	[AttachmentRequired] [nvarchar](max) NULL,
	[AuthorizedToView] [nvarchar](250) NULL,
	[ConditionalLogic] [nvarchar](max) NULL,
	[CreateParentServiceRequest] [bit] NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[HideSummary] [bit] NULL,
	[HideThankYouScreen] [bit] NULL,
	[ImageUrl] [nvarchar](250) NULL,
	[IsActivated] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[ItemOrder] [int] NULL,
	[LoadDefaultValue] [bit] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[ModuleStageMultiLookup] [bigint] NOT NULL,
	[NavigationUrl] [nvarchar](max) NULL,
	[Owner] [nvarchar](250) NULL,
	[OwnerApprovalRequired] [bit] NULL,
	[QuestionMapVariables] [nvarchar](max) NULL,
	[SectionConditionalLogic] [nvarchar](max) NULL,
	[ServiceCategoryNameLookup] [bigint] NULL,
	[ServiceDescription] [nvarchar](max) NULL,
	[ShowStageTransitionButtons] [bit] NULL,
	[StagingId] [int] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_UserRoles]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_UserRoles](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[LandingPage] [nvarchar](250) NULL,
	[UserRole] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_VendorResourceCategory]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Config_VendorResourceCategory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CategoryCode] [nvarchar](250) NULL,
	[CategoryName] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[SubCategory] [nvarchar](250) NULL,
	[SubCategoryCode] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_Config_VendorResourceCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Config_WikiLeftNavigation]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Config_WikiLeftNavigation](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ColumnType] [nvarchar](250) NULL,
	[ConditionalLogic] [nvarchar](max) NULL,
	[ImageUrl] [nvarchar](250) NULL,
	[ItemOrder] [int] NULL,
	[Title] [nvarchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contacts](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AddressedAs] [nvarchar](250) NULL,
	[City] [nvarchar](250) NULL,
	[Country] [nvarchar](250) NULL,
	[EmailAddress] [nvarchar](250) NULL,
	[Fax] [nvarchar](250) NULL,
	[FirstName] [nvarchar](250) NULL,
	[LastName] [nvarchar](250) NULL,
	[MiddleName] [nvarchar](250) NULL,
	[Mobile] [nvarchar](250) NULL,
	[OrganizationLookup] [bigint] NOT NULL,
	[SecondaryEmail] [nvarchar](250) NULL,
	[State] [nvarchar](250) NULL,
	[StreetAddress1] [nvarchar](250) NULL,
	[StreetAddress2] [nvarchar](250) NULL,
	[Telephone] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
	[Zip] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Contracts]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contracts](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualStartDate] [datetime] NULL,
	[AnnualMaintenanceCost] [nvarchar](250) NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[ContractExpirationDate] [datetime] NULL,
	[ContractStartDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[EstimatedHours] [int] NULL,
	[FinanceManager] [nvarchar](max) NULL,
	[FunctionalAreaLookup] [bigint] NOT NULL,
	[History] [nvarchar](max) NULL,
	[InitialCost] [nvarchar](250) NULL,
	[Initiator] [nvarchar](max) NULL,
	[IsPrivate] [bit] NULL,
	[Legal] [nvarchar](max) NULL,
	[LicenseCount] [int] NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NeedReview] [nvarchar](max) NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldStartDate] [datetime] NULL,
	[OnHoldTillDate] [datetime] NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PONumber] [nvarchar](250) NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[PurchaseInstructions] [nvarchar](max) NULL,
	[Purchasing] [nvarchar](max) NULL,
	[Quantity] [int] NULL,
	[ReminderBody] [nvarchar](max) NULL,
	[ReminderDate] [datetime] NULL,
	[ReminderDays] [int] NULL,
	[ReminderTo] [nvarchar](250) NULL,
	[RenewalCancelNoticeDays] [int] NULL,
	[RepeatInterval] [nvarchar](max) NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestSource] [nvarchar](max) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeWorkflow] [nvarchar](250) NULL,
	[ResolutionComments] [nvarchar](max) NULL,
	[ReSubmissionDate] [datetime] NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[TermType] [nvarchar](max) NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
	[UserQuestionSummary] [nvarchar](max) NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Customers]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AccountAddress] [nvarchar](max) NULL,
	[AccountManager] [nvarchar](max) NULL,
	[AccountName] [nvarchar](250) NULL,
	[AcquiredBy] [nvarchar](max) NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualStartDate] [datetime] NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[CampaignInfo] [nvarchar](max) NULL,
	[CampaignStrategy] [nvarchar](max) NULL,
	[CampaignUsed] [nvarchar](max) NULL,
	[ChanceOfSuccess] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[Company] [nvarchar](250) NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[CustomerAccountSize] [int] NULL,
	[CustomerAccountType] [nvarchar](max) NULL,
	[CustomerAccountVertical] [nvarchar](max) NULL,
	[CustomerSource] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[Duration] [int] NULL,
	[EmailAddress] [nvarchar](250) NULL,
	[FacebookPage] [nvarchar](250) NULL,
	[FirstName] [nvarchar](250) NULL,
	[History] [nvarchar](max) NULL,
	[IdentifiedBy] [nvarchar](max) NULL,
	[InitialCost] [nvarchar](250) NULL,
	[Initiator] [nvarchar](max) NULL,
	[IsPrivate] [bit] NULL,
	[JobTitle] [nvarchar](250) NULL,
	[LastName] [nvarchar](250) NULL,
	[LeadStatus] [nvarchar](max) NULL,
	[LinkedInPage] [nvarchar](250) NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NoOfEmployee] [int] NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldStartDate] [datetime] NULL,
	[OnHoldTillDate] [datetime] NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[Quality] [nvarchar](max) NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestSource] [nvarchar](max) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeWorkflow] [nvarchar](250) NULL,
	[ResolutionComments] [nvarchar](max) NULL,
	[ReSubmissionDate] [datetime] NULL,
	[Revenues] [nvarchar](250) NULL,
	[SICCode] [nvarchar](250) NULL,
	[SourceDetails] [nvarchar](max) NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[TaskActualCompletionDate] [datetime] NULL,
	[TaskActualStartDate] [datetime] NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
	[TwitterPage] [nvarchar](250) NULL,
	[URL] [nvarchar](250) NULL,
	[VendorPhone] [nvarchar](250) NULL,
	[WorkAddress] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DashboardSummary]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DashboardSummary](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualHours] [int] NULL,
	[ALLSLAsMet] [nvarchar](max) NULL,
	[AssignmentSLAMet] [nvarchar](max) NULL,
	[Category] [nvarchar](250) NULL,
	[Closed] [bit] NULL,
	[CloseSLAMet] [nvarchar](max) NULL,
	[Country] [nvarchar](250) NULL,
	[CreationDate] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[FunctionalAreaLookup] [bigint] NOT NULL,
	[GenericStatusLookup] [int] NOT NULL,
	[Initiator] [nvarchar](max) NULL,
	[InitiatorResolved] [nvarchar](max) NULL,
	[LocationLookup] [bigint] NOT NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[OnHold] [int] NULL,
	[ORP] [nvarchar](250) NULL,
	[OtherSLAMet] [nvarchar](max) NULL,
	[Owner] [nvarchar](250) NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[PRP] [nvarchar](max) NULL,
	[PRPGroup] [nvarchar](max) NULL,
	[Region] [nvarchar](250) NULL,
	[ReopenCount] [int] NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestorCompany] [nvarchar](250) NULL,
	[RequestorContactSLAMet] [nvarchar](max) NULL,
	[RequestorDepartment] [nvarchar](250) NULL,
	[RequestorDivision] [nvarchar](250) NULL,
	[RequestSource] [nvarchar](max) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[ResolutionSLAMet] [nvarchar](max) NULL,
	[ResolutionType] [nvarchar](max) NULL,
	[ServiceCategoryName] [nvarchar](250) NULL,
	[ServiceName] [nvarchar](250) NULL,
	[SLAMet] [nvarchar](250) NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[State] [nvarchar](250) NULL,
	[Status] [nvarchar](250) NULL,
	[SubCategory] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[WorkflowType] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Department]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Department](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyIdLookup] [bigint] NULL,
	[DepartmentDescription] [nvarchar](max) NULL,
	[DivisionIdLookup] [bigint] NULL,
	[GLCode] [nvarchar](250) NULL,
	[IsDeleted] [bit] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DMConfigList]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DMConfigList](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DMKeyValue] [nvarchar](250) NULL,
	[KeyName] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DMDepartmentList]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DMDepartmentList](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DMSDescription] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DMDocInfoType]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DMDocInfoType](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ClassApplied] [nvarchar](max) NULL,
	[Extensions] [nvarchar](max) NULL,
	[Type] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DMDocumentHistoryList]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DMDocumentHistoryList](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Action] [nvarchar](250) NULL,
	[Authors] [nvarchar](max) NULL,
	[DocumentControlID] [nvarchar](250) NULL,
	[DocumentID] [nvarchar](250) NULL,
	[DocVersion] [nvarchar](250) NULL,
	[FileName] [nvarchar](250) NULL,
	[FolderID] [nvarchar](250) NULL,
	[History] [nvarchar](max) NULL,
	[HistoryDate] [datetime] NULL,
	[PortalId] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DMDocumentInfoList]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DMDocumentInfoList](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ApprovedVersion] [nvarchar](250) NULL,
	[Authors] [nvarchar](250) NULL,
	[CurrentApprover] [nvarchar](250) NULL,
	[DepartmentNameLookup] [bigint] NOT NULL,
	[DMDocType1] [nvarchar](250) NULL,
	[DMDocType2] [nvarchar](250) NULL,
	[DMDocType3] [nvarchar](250) NULL,
	[DMDocType4] [nvarchar](250) NULL,
	[DMDocType5] [nvarchar](250) NULL,
	[DMDocumentStatus] [nvarchar](250) NULL,
	[DMVendorLookup] [bigint] NOT NULL,
	[DocumentComments] [nvarchar](max) NULL,
	[DocumentControlID] [nvarchar](250) NULL,
	[DocumentID] [nvarchar](250) NULL,
	[DocumentTypeLookup] [bigint] NOT NULL,
	[DocVersion] [nvarchar](250) NULL,
	[ExpirationDate] [datetime] NULL,
	[FileName] [nvarchar](250) NULL,
	[FolderID] [nvarchar](250) NULL,
	[FolderName] [nvarchar](250) NULL,
	[IsDeleted] [bit] NULL,
	[KeepDocsAlive] [nvarchar](250) NULL,
	[KeepNYear] [nvarchar](250) NULL,
	[NotifyOnReviewComplete] [nvarchar](250) NULL,
	[NotifyUserOnReviewStart] [nvarchar](250) NULL,
	[NumOfReviewCycle] [int] NULL,
	[NumPings] [nvarchar](250) NULL,
	[OverrideReaders] [bit] NULL,
	[PortalId] [nvarchar](250) NULL,
	[ProjectLookup] [bigint] NOT NULL,
	[Readers] [nvarchar](250) NULL,
	[ReviewCycle1] [nvarchar](250) NULL,
	[ReviewCycle10] [nvarchar](250) NULL,
	[ReviewCycle2] [nvarchar](250) NULL,
	[ReviewCycle3] [nvarchar](250) NULL,
	[ReviewCycle4] [nvarchar](250) NULL,
	[ReviewCycle5] [nvarchar](250) NULL,
	[ReviewCycle6] [nvarchar](250) NULL,
	[ReviewCycle7] [nvarchar](250) NULL,
	[ReviewCycle8] [nvarchar](250) NULL,
	[ReviewCycle9] [nvarchar](250) NULL,
	[ReviewRequired] [bit] NULL,
	[ReviewStep] [int] NULL,
	[Tags] [nvarchar](250) NULL,
	[UVersion] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DMDocumentLinkList]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DMDocumentLinkList](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DocumentID] [nvarchar](250) NULL,
	[FolderID] [nvarchar](250) NULL,
	[FolderUrl] [nvarchar](250) NULL,
	[LinkText] [nvarchar](250) NULL,
	[PortalId] [nvarchar](250) NULL,
	[SourceFolderID] [nvarchar](250) NULL,
	[SourcePortalID] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DMDocumentTypeList]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DMDocumentTypeList](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Acronym] [nvarchar](250) NULL,
	[DMSDescription] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DMDocumentWorkflowHistory]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DMDocumentWorkflowHistory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DocumentID] [nvarchar](250) NULL,
	[DocVersion] [nvarchar](250) NULL,
	[DocWorkflowHistory] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DMPingInformation]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DMPingInformation](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[NumPings] [nvarchar](250) NULL,
	[PingComments] [nvarchar](max) NULL,
	[PingUser] [nvarchar](max) NULL,
	[TaskID] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DMPortalInfo]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DMPortalInfo](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AllowAllTypes] [bit] NULL,
	[AlternateOwner] [nvarchar](max) NULL,
	[AlwaysNotifyOwnerOnReviewComplete] [bit] NULL,
	[AlwaysNotifyOwnerOnReviewStart] [bit] NULL,
	[AlwaysReviewRequired] [bit] NULL,
	[Authors] [nvarchar](250) NULL,
	[CCUser] [nvarchar](250) NULL,
	[DCLastSequence] [nvarchar](250) NULL,
	[Green] [nvarchar](250) NULL,
	[IsDocumentID] [bit] NULL,
	[IsFolderProperties] [bit] NULL,
	[IsPortalProjectType] [bit] NULL,
	[IsSizeUnlimited] [bit] NULL,
	[KeepDocsAlive] [nvarchar](250) NULL,
	[KeepNYear] [nvarchar](250) NULL,
	[ModuleName] [nvarchar](250) NULL,
	[NotifyAuthor] [bit] NULL,
	[NotifyOnReviewComplete] [nvarchar](250) NULL,
	[NotifyOwnerBeforeDeletion] [bit] NULL,
	[NotifyOwnerOnDocUpload] [bit] NULL,
	[NotifyReader] [bit] NULL,
	[NotifyUserOnReviewStart] [nvarchar](250) NULL,
	[NumFiles] [nvarchar](250) NULL,
	[NumOfReviewCycle] [int] NULL,
	[NumVersions] [nvarchar](250) NULL,
	[PortalDescription] [nvarchar](max) NULL,
	[PortalId] [nvarchar](250) NULL,
	[PortalName] [nvarchar](250) NULL,
	[PortalOwner] [nvarchar](250) NULL,
	[PrefixAcronym] [bit] NULL,
	[Readers] [nvarchar](250) NULL,
	[Red] [nvarchar](250) NULL,
	[ReviewCycle1] [nvarchar](250) NULL,
	[ReviewCycle10] [nvarchar](250) NULL,
	[ReviewCycle2] [nvarchar](250) NULL,
	[ReviewCycle3] [nvarchar](250) NULL,
	[ReviewCycle4] [nvarchar](250) NULL,
	[ReviewCycle5] [nvarchar](250) NULL,
	[ReviewCycle6] [nvarchar](250) NULL,
	[ReviewCycle7] [nvarchar](250) NULL,
	[ReviewCycle8] [nvarchar](250) NULL,
	[ReviewCycle9] [nvarchar](250) NULL,
	[ReviewRequired] [bit] NULL,
	[SizeOfFolder] [nvarchar](250) NULL,
	[UseDefaultCategory] [bit] NULL,
	[Yellow] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DMProjectList]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DMProjectList](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DMSDescription] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DMTagList]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DMTagList](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[TagName] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DMVendorList]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DMVendorList](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DMSDescription] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DMWorkflowTask]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DMWorkflowTask](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssignedTo] [nvarchar](max) NULL,
	[Body] [nvarchar](max) NULL,
	[DocID] [nvarchar](250) NULL,
	[DueDate] [datetime] NULL,
	[PercentComplete] [int] NULL,
	[PingComments] [nvarchar](max) NULL,
	[Predecessors] [nvarchar](250) NULL,
	[Priority] [nvarchar](max) NULL,
	[Requested_By] [nvarchar](max) NULL,
	[StartDate] [datetime] NULL,
	[Status] [nvarchar](max) NULL,
	[TaskGroup] [nvarchar](max) NULL,
	[WorkflowTaskStatus] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DRQ]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DRQ](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualHours] [int] NULL,
	[ActualStartDate] [datetime] NULL,
	[ApplicationManager] [nvarchar](max) NULL,
	[APPTitleLookup] [bigint] NOT NULL,
	[AutoSend] [bit] NULL,
	[Beneficiaries] [nvarchar](250) NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[ChangePurpose] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CompanyTitleLookup] [bigint] NOT NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[CustomUGChoice01] [nvarchar](max) NULL,
	[CustomUGChoice02] [nvarchar](max) NULL,
	[CustomUGChoice03] [nvarchar](max) NULL,
	[CustomUGChoice04] [nvarchar](max) NULL,
	[CustomUGDate01] [datetime] NULL,
	[CustomUGDate02] [datetime] NULL,
	[CustomUGDate03] [datetime] NULL,
	[CustomUGDate04] [datetime] NULL,
	[CustomUGText01] [nvarchar](250) NULL,
	[CustomUGText02] [nvarchar](250) NULL,
	[CustomUGText03] [nvarchar](250) NULL,
	[CustomUGText04] [nvarchar](250) NULL,
	[CustomUGText05] [nvarchar](250) NULL,
	[CustomUGText06] [nvarchar](250) NULL,
	[CustomUGText07] [nvarchar](250) NULL,
	[CustomUGText08] [nvarchar](250) NULL,
	[CustomUGUser01] [nvarchar](max) NULL,
	[CustomUGUser02] [nvarchar](max) NULL,
	[CustomUGUser03] [nvarchar](max) NULL,
	[CustomUGUser04] [nvarchar](max) NULL,
	[CustomUGUserMulti01] [nvarchar](250) NULL,
	[CustomUGUserMulti02] [nvarchar](250) NULL,
	[CustomUGUserMulti03] [nvarchar](250) NULL,
	[CustomUGUserMulti04] [nvarchar](250) NULL,
	[DepartmentLookup] [bigint] NOT NULL,
	[DeploymentPlan] [nvarchar](max) NULL,
	[DeploymentResponsible] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[DeskLocation] [nvarchar](250) NULL,
	[DivisionLookup] [bigint] NOT NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[DRBRDescription] [nvarchar](max) NULL,
	[DRBRImpact] [nvarchar](max) NULL,
	[DRBRManager] [nvarchar](max) NULL,
	[DRQChangeType] [nvarchar](max) NULL,
	[DRQRapidTypeLookup] [bigint] NOT NULL,
	[DRQSystemsLookup] [bigint] NOT NULL,
	[DRReplicationChange] [nvarchar](max) NULL,
	[Duration] [int] NULL,
	[EstimatedHours] [int] NULL,
	[FunctionalAreaLookup] [bigint] NOT NULL,
	[History] [nvarchar](max) NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[ImpactsOrganization] [bit] NULL,
	[InfrastructureManager] [nvarchar](max) NULL,
	[Initiator] [nvarchar](max) NULL,
	[InitiatorResolved] [nvarchar](max) NULL,
	[IsUserNotificationRequired] [bit] NULL,
	[LocationMultLookup] [bigint] NOT NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NextSLATime] [datetime] NULL,
	[NextSLAType] [nvarchar](250) NULL,
	[NotificationText] [nvarchar](max) NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldTillDate] [datetime] NULL,
	[ORP] [nvarchar](250) NULL,
	[Outage] [nvarchar](max) NULL,
	[Owner] [nvarchar](250) NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[ProductionVerificationPlan] [nvarchar](max) NULL,
	[ProductionVerifyResponsible] [nvarchar](max) NULL,
	[PRP] [nvarchar](max) NULL,
	[RapidRequest] [nvarchar](max) NULL,
	[RecoveryPlan] [nvarchar](max) NULL,
	[RelatedRequestID] [nvarchar](250) NULL,
	[RelatedRequestType] [nvarchar](250) NULL,
	[ReopenCount] [int] NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeSubCategory] [nvarchar](250) NULL,
	[RequestTypeWorkflow] [nvarchar](250) NULL,
	[ResolutionComments] [nvarchar](max) NULL,
	[Risk] [nvarchar](max) NULL,
	[RollbackResponsible] [nvarchar](max) NULL,
	[ScheduledEndDateTime] [datetime] NULL,
	[ScheduledStartDateTime] [datetime] NULL,
	[SecurityManager] [nvarchar](250) NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[Severity] [nvarchar](max) NULL,
	[SeverityLookup] [bigint] NOT NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[Tester] [nvarchar](250) NULL,
	[TestingDone] [nvarchar](max) NULL,
	[TestPlan] [nvarchar](max) NULL,
	[TicketId] [nvarchar](250) NULL,
	[ToBeSentByDate] [datetime] NULL,
	[TotalHoldDuration] [int] NULL,
	[UserImpactDetails] [nvarchar](max) NULL,
	[UserQuestionSummary] [nvarchar](max) NULL,
	[UsersAffected] [nvarchar](250) NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DRQRapidTypes]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DRQRapidTypes](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_DRQRapidTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DRQSystemAreas]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DRQSystemAreas](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_DRQSystemAreas] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmailFooter]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailFooter](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FieldDisplayName] [nvarchar](250) NULL,
	[FieldName] [nvarchar](250) NULL,
	[ItemOrder] [int] NULL,
	[ModuleName] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EmailQueue]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailQueue](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EmailBody] [nvarchar](max) NULL,
	[EmailOnDate] [datetime] NULL,
	[MailSubject] [nvarchar](250) NULL,
	[MailTo] [nvarchar](max) NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Emails]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Emails](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EmailIDCC] [nvarchar](max) NULL,
	[EmailIDFrom] [nvarchar](max) NULL,
	[EmailIDTo] [nvarchar](max) NULL,
	[EmailReplyTo] [nvarchar](max) NULL,
	[EscalationEmailBody] [nvarchar](max) NULL,
	[IsIncomingMail] [bit] NULL,
	[MailSubject] [nvarchar](250) NULL,
	[MessageId] [nvarchar](250) NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Environment]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Environment](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_Environment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EscalationLog]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EscalationLog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EscalationSentTime] [datetime] NULL,
	[EscalationSentTo] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EscalationQueue]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EscalationQueue](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EscalationRuleIDLookup] [bigint] NOT NULL,
	[NextEscalationTime] [datetime] NULL,
	[SLARuleIdLookup] [bigint] NOT NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FieldConfiguration]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FieldConfiguration](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FieldName] [nvarchar](250) NULL,
	[ParentTableName] [varchar](25) NULL,
	[ParentFieldName] [varchar](25) NULL,
	[DataType] [varchar](25) NULL,
	[Data] [nvarchar](250) NULL,
	[DisplayChoicesControl] [nvarchar](250) NULL,
	[Multi] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_FieldConfiguration] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FieldConfiguration_bkp]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FieldConfiguration_bkp](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FieldName] [nvarchar](250) NULL,
	[ParentTableName] [varchar](25) NULL,
	[ParentFieldName] [varchar](25) NULL,
	[DataType] [varchar](25) NULL,
	[Data] [nvarchar](250) NULL,
	[DisplayChoicesControl] [nvarchar](250) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FunctionalAreas]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FunctionalAreas](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DepartmentLookup] [bigint] NOT NULL,
	[FunctionalAreaDescription] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
	[Owner] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GenericStatus]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GenericStatus](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GenericStatus] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GovernanceLinkCategory]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GovernanceLinkCategory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](250) NULL,
	[ImageUrl] [nvarchar](250) NULL,
	[ItemOrder] [int] NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_GovernanceLinkCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GovernanceLinkItems]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GovernanceLinkItems](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[GovernanceLinkCategoryLookup] [bigint] NOT NULL,
	[ImageUrl] [nvarchar](250) NULL,
	[TabSequence] [int] NULL,
	[TargetType] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HolidaysAndWorkDaysCalendar]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HolidaysAndWorkDaysCalendar](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Category] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Duration] [nvarchar](250) NULL,
	[EndDate] [datetime] NULL,
	[EventCanceled] [bit] NULL,
	[EventDate] [datetime] NULL,
	[EventType] [nvarchar](250) NULL,
	[Facilities] [nvarchar](250) NULL,
	[fAllDayEvent] [nvarchar](250) NULL,
	[fRecurrence] [nvarchar](250) NULL,
	[FreeBusy] [nvarchar](250) NULL,
	[IsDeleted] [bit] NULL,
	[Location] [nvarchar](250) NULL,
	[MasterSeriesItemID] [nvarchar](250) NULL,
	[Overbook] [nvarchar](250) NULL,
	[Participants] [nvarchar](max) NULL,
	[ParticipantsPicker] [nvarchar](250) NULL,
	[RecurrenceData] [nvarchar](max) NULL,
	[RecurrenceID] [datetime] NULL,
	[RecurrenceInfo] [nvarchar](250) NULL,
	[Status] [nvarchar](250) NULL,
	[TimeZone] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
	[UID] [nvarchar](250) NULL,
	[Workspace] [nvarchar](250) NULL,
	[WorkspaceLink] [nvarchar](250) NULL,
	[XMLTZone] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ImageSoftware]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ImageSoftware](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[OS] [nvarchar](250) NULL,
	[SoftwareKey] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_ImageSoftware] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ImageSoftwareMap]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ImageSoftwareMap](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[APPTitleLookup] [bigint] NOT NULL,
	[ImageOptionLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[INC]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[INC](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualHours] [int] NULL,
	[AffectedUsers] [nvarchar](250) NULL,
	[AssetLookup] [bigint] NOT NULL,
	[AssetMultiLookup] [bigint] NOT NULL,
	[Beneficiaries] [nvarchar](250) NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CompanyTitleLookup] [bigint] NOT NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[CustomUGChoice01] [nvarchar](max) NULL,
	[CustomUGChoice02] [nvarchar](max) NULL,
	[CustomUGChoice03] [nvarchar](max) NULL,
	[CustomUGChoice04] [nvarchar](max) NULL,
	[CustomUGDate01] [datetime] NULL,
	[CustomUGDate02] [datetime] NULL,
	[CustomUGDate03] [datetime] NULL,
	[CustomUGDate04] [datetime] NULL,
	[CustomUGText01] [nvarchar](250) NULL,
	[CustomUGText02] [nvarchar](250) NULL,
	[CustomUGText03] [nvarchar](250) NULL,
	[CustomUGText04] [nvarchar](250) NULL,
	[CustomUGText05] [nvarchar](250) NULL,
	[CustomUGText06] [nvarchar](250) NULL,
	[CustomUGText07] [nvarchar](250) NULL,
	[CustomUGText08] [nvarchar](250) NULL,
	[CustomUGUser01] [nvarchar](max) NULL,
	[CustomUGUser02] [nvarchar](max) NULL,
	[CustomUGUser03] [nvarchar](max) NULL,
	[CustomUGUser04] [nvarchar](max) NULL,
	[CustomUGUserMulti01] [nvarchar](250) NULL,
	[CustomUGUserMulti02] [nvarchar](250) NULL,
	[CustomUGUserMulti03] [nvarchar](250) NULL,
	[CustomUGUserMulti04] [nvarchar](250) NULL,
	[DepartmentLookup] [bigint] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DeskLocation] [nvarchar](250) NULL,
	[DetectionDate] [datetime] NULL,
	[DivisionLookup] [bigint] NOT NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[FunctionalAreaLookup] [bigint] NOT NULL,
	[History] [nvarchar](max) NULL,
	[ImpactsOrganization] [bit] NULL,
	[Initiator] [nvarchar](max) NULL,
	[LocationLookup] [bigint] NOT NULL,
	[LocationMultLookup] [bigint] NOT NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NextSLATime] [datetime] NULL,
	[NextSLAType] [nvarchar](250) NULL,
	[OccurrenceDate] [datetime] NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldTillDate] [datetime] NULL,
	[ORP] [nvarchar](250) NULL,
	[Owner] [nvarchar](250) NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[PRP] [nvarchar](max) NULL,
	[PRSLookup] [bigint] NOT NULL,
	[ReopenCount] [int] NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestorContacted] [bit] NULL,
	[RequestSource] [nvarchar](max) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeSubCategory] [nvarchar](250) NULL,
	[RequestTypeWorkflow] [nvarchar](250) NULL,
	[ResolutionComments] [nvarchar](max) NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[SeverityLookup] [bigint] NOT NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[TicketId] [nvarchar](250) NULL,
	[UserQuestionSummary] [nvarchar](max) NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InvDistribution]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvDistribution](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DistributionAmount] [nvarchar](250) NULL,
	[DistributionDate] [datetime] NULL,
	[DistributionQuarter] [nvarchar](max) NULL,
	[DistributionType] [nvarchar](250) NULL,
	[InvestmentIDLookup] [bigint] NOT NULL,
	[InvestorIDLookup] [bigint] NOT NULL,
	[Title] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Investments]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Investments](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AcquireDate] [datetime] NULL,
	[EmailAddress] [nvarchar](250) NULL,
	[ExpectedExit] [datetime] NULL,
	[Investment] [nvarchar](250) NULL,
	[InvestmentManagers] [nvarchar](250) NULL,
	[InvestorShortNameLookup] [bigint] NOT NULL,
	[INVType] [nvarchar](250) NULL,
	[ReturnYield] [nvarchar](250) NULL,
	[State] [nvarchar](250) NULL,
	[StreetAddress] [nvarchar](max) NULL,
	[Title] [nvarchar](250) NULL,
	[VendorPhone] [nvarchar](250) NULL,
	[WorkCity] [nvarchar](250) NULL,
	[WorkZip] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Investors]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Investors](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AccountType] [nvarchar](max) NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualStartDate] [datetime] NULL,
	[AddedDate] [datetime] NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[Contact] [nvarchar](250) NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[Custodian] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[EmailAddress] [nvarchar](250) NULL,
	[EmployerIdentificationNumber] [int] NULL,
	[FinanceManager] [nvarchar](max) NULL,
	[FirstName] [nvarchar](250) NULL,
	[History] [nvarchar](max) NULL,
	[Initiator] [nvarchar](max) NULL,
	[InvestorID] [int] NULL,
	[InvestorName] [nvarchar](250) NULL,
	[InvestorShortName] [nvarchar](250) NULL,
	[InvestorStatus] [nvarchar](max) NULL,
	[IsPrivate] [bit] NULL,
	[LastName] [nvarchar](250) NULL,
	[Legal] [nvarchar](max) NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[OnHold] [int] NULL,
	[OnHoldStartDate] [datetime] NULL,
	[OtherAddress] [nvarchar](max) NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[Purchasing] [nvarchar](max) NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestSource] [nvarchar](max) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[ResolutionComments] [nvarchar](max) NULL,
	[Responsible] [nvarchar](max) NULL,
	[ReSubmissionDate] [datetime] NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[State] [nvarchar](250) NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[StreetAddress] [nvarchar](max) NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
	[UpdateDate] [datetime] NULL,
	[WorkCity] [nvarchar](250) NULL,
	[WorkZip] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ITGActual]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ITGActual](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCost] [nvarchar](250) NULL,
	[BudgetEndDate] [datetime] NULL,
	[BudgetStartDate] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[InvoiceNumber] [nvarchar](250) NULL,
	[ITGBudgetLookup] [bigint] NOT NULL,
	[VendorLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ITGBudget]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ITGBudget](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BudgetAmount] [nvarchar](250) NULL,
	[BudgetEndDate] [datetime] NULL,
	[BudgetLookup] [bigint] NOT NULL,
	[BudgetStartDate] [datetime] NULL,
	[DepartmentLookup] [bigint] NOT NULL,
	[GLCode] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ITGMonthlyBudget]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ITGMonthlyBudget](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCost] [nvarchar](250) NULL,
	[AllocationStartDate] [datetime] NULL,
	[BudgetAmount] [nvarchar](250) NULL,
	[BudgetLookup] [bigint] NOT NULL,
	[BudgetType] [nvarchar](250) NULL,
	[EstimatedCost] [nvarchar](250) NULL,
	[NonProjectActualTotal] [nvarchar](250) NULL,
	[NonProjectPlanedTotal] [nvarchar](250) NULL,
	[ProjectActualTotal] [nvarchar](250) NULL,
	[ProjectPlanedTotal] [nvarchar](250) NULL,
	[ResourceCost] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ITGovernance]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ITGovernance](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Closed] [bit] NULL,
	[Comment] [nvarchar](max) NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[History] [nvarchar](max) NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[Initiator] [nvarchar](max) NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldTillDate] [datetime] NULL,
	[Owner] [nvarchar](250) NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeSubCategory] [nvarchar](250) NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[SeverityLookup] [bigint] NOT NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[TicketId] [nvarchar](250) NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LinkCategory]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LinkCategory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ImageUrl] [nvarchar](250) NULL,
	[ItemOrder] [int] NULL,
	[LinkViewLookup] [bigint] NOT NULL,
	[Title] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LinkItems]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LinkItems](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AuthorizedToView] [nvarchar](250) NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[ItemOrder] [int] NULL,
	[LinkCategoryLookup] [bigint] NOT NULL,
	[TargetType] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LinkView]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LinkView](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[Title] [nvarchar](250) NULL,
 CONSTRAINT [PK_LinkView] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Location]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Location](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Country] [nvarchar](250) NULL,
	[IsDeleted] [bit] NULL,
	[LocationDescription] [nvarchar](max) NULL,
	[Region] [nvarchar](250) NULL,
	[State] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Log]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[ItemUser] [nvarchar](250) NULL,
	[ModuleName] [nvarchar](250) NULL,
	[Severity] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MessageBoard]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MessageBoard](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AuthorizedToView] [nvarchar](250) NULL,
	[Body] [nvarchar](max) NULL,
	[Expires] [datetime] NULL,
	[MessageType] [nvarchar](max) NULL,
	[NavigationUrl] [nvarchar](max) NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ModuleDefaultValues]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ModuleDefaultValues](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[KeyName] [nvarchar](250) NULL,
	[KeyValue] [nvarchar](max) NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ModuleStageConstraints]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModuleStageConstraints](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssignedTo] [nvarchar](max) NULL,
	[Body] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[CompletionDate] [datetime] NULL,
	[DocumentInfo] [nvarchar](250) NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[FormulaValue] [nvarchar](max) NULL,
	[ModuleAutoApprove] [bit] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[ModuleStep] [int] NULL,
	[PercentComplete] [int] NULL,
	[Predecessors] [nvarchar](250) NULL,
	[Priority] [nvarchar](max) NULL,
	[ProposedDate] [datetime] NULL,
	[ProposedStatus] [nvarchar](max) NULL,
	[StartDate] [datetime] NULL,
	[TaskActualHours] [int] NULL,
	[TaskDueDate] [datetime] NULL,
	[TaskEstimatedHours] [int] NULL,
	[TaskStatus] [nvarchar](max) NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ModuleStageConstraintTemplates]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModuleStageConstraintTemplates](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssignedTo] [nvarchar](250) NULL,
	[Body] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[DocumentInfo] [nvarchar](250) NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[FormulaValue] [nvarchar](max) NULL,
	[ModuleAutoApprove] [bit] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[ModuleStep] [int] NULL,
	[PercentComplete] [int] NULL,
	[Predecessors] [nvarchar](250) NULL,
	[Priority] [nvarchar](max) NULL,
	[ProposedDate] [datetime] NULL,
	[ProposedStatus] [nvarchar](max) NULL,
	[StartDate] [datetime] NULL,
	[TaskActualHours] [int] NULL,
	[TaskDueDate] [datetime] NULL,
	[TaskEstimatedHours] [int] NULL,
	[TaskStatus] [nvarchar](max) NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ModuleTasks]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ModuleTasks](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssignedTo] [nvarchar](max) NULL,
	[AssignToPct] [nvarchar](max) NULL,
	[Body] [nvarchar](max) NULL,
	[ChildCount] [int] NULL,
	[Comment] [nvarchar](max) NULL,
	[Contribution] [int] NULL,
	[DueDate] [datetime] NULL,
	[Duration] [int] NULL,
	[EstimatedRemainingHours] [int] NULL,
	[IsCritical] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[ItemOrder] [int] NULL,
	[Level] [int] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[ParentTask] [int] NULL,
	[PercentComplete] [int] NULL,
	[Predecessors] [nvarchar](250) NULL,
	[Priority] [nvarchar](max) NULL,
	[ProposedDate] [datetime] NULL,
	[ProposedStatus] [nvarchar](max) NULL,
	[StartDate] [datetime] NULL,
	[Status] [nvarchar](max) NULL,
	[TaskActualHours] [int] NULL,
	[TaskBehaviour] [nvarchar](max) NULL,
	[TaskEstimatedHours] [int] NULL,
	[TaskGroup] [nvarchar](max) NULL,
	[TaskReminderDays] [int] NULL,
	[TaskReminderEnabled] [bit] NULL,
	[TicketId] [nvarchar](250) NULL,
	[UserSkillMultiLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ModuleUserStatistics]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ModuleUserStatistics](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[IsActionUser] [bit] NULL,
	[ModuleId] [int] NULL,
	[TicketId] [nvarchar](250) NULL,
	[UserRole] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ModuleWorkflowHistory]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ModuleWorkflowHistory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActionUserType] [nvarchar](250) NULL,
	[Duration] [int] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[OnHoldDuration] [int] NULL,
	[SLAMet] [bit] NULL,
	[StageClosedBy] [nvarchar](max) NULL,
	[StageClosedByName] [nvarchar](250) NULL,
	[StageEndDate] [datetime] NULL,
	[StageStartDate] [datetime] NULL,
	[StageStep] [int] NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MonthlyBudget]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MonthlyBudget](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AllocationStartDate] [datetime] NULL,
	[BudgetAmount] [nvarchar](250) NULL,
	[BudgetType] [nvarchar](250) NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NPRBudget]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NPRBudget](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AllocationEndDate] [datetime] NULL,
	[AllocationStartDate] [datetime] NULL,
	[BudgetAmount] [nvarchar](250) NULL,
	[BudgetDescription] [nvarchar](max) NULL,
	[BudgetItem] [nvarchar](250) NULL,
	[BudgetLookup] [bigint] NOT NULL,
	[IsAutoCalculated] [bit] NULL,
	[NPRIdLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NPRMonthlyBudget]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NPRMonthlyBudget](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AllocationStartDate] [datetime] NULL,
	[BudgetAmount] [nvarchar](250) NULL,
	[BudgetType] [nvarchar](250) NULL,
	[NPRIdLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NPRRequest]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NPRRequest](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualStartDate] [datetime] NULL,
	[AdoptionRisk] [nvarchar](max) NULL,
	[AnalyticsArchitecture] [int] NULL,
	[AnalyticsCost] [int] NULL,
	[AnalyticsRisk] [int] NULL,
	[AnalyticsROI] [int] NULL,
	[AnalyticsSchedule] [int] NULL,
	[ApprovedRFE] [nvarchar](250) NULL,
	[ApprovedRFEAmount] [nvarchar](250) NULL,
	[ApprovedRFEType] [nvarchar](max) NULL,
	[APPTitleLookup] [bigint] NOT NULL,
	[ArchitectureScore] [int] NULL,
	[ArchitectureScoreNotes] [nvarchar](max) NULL,
	[AssignedAnalyst] [nvarchar](max) NULL,
	[AutoAdjustAllocations] [bit] NULL,
	[Beneficiaries] [nvarchar](250) NULL,
	[BreakEvenIn] [int] NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[CannotStartBefore] [datetime] NULL,
	[CannotStartBeforeNotes] [nvarchar](max) NULL,
	[CapitalExpenditure] [bit] NULL,
	[CapitalExpenditureNotes] [nvarchar](250) NULL,
	[CapitalExpense] [nvarchar](250) NULL,
	[Classification] [nvarchar](max) NULL,
	[ClassificationImpact] [nvarchar](max) NULL,
	[ClassificationNotes] [nvarchar](max) NULL,
	[ClassificationScope] [nvarchar](max) NULL,
	[ClassificationSize] [nvarchar](max) NULL,
	[ClassificationType] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CompanyMultiLookup] [bigint] NOT NULL,
	[Complexity] [nvarchar](max) NULL,
	[ComplexityNotes] [nvarchar](max) NULL,
	[ConstraintNotes] [nvarchar](max) NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[CustomUGChoice01] [nvarchar](max) NULL,
	[CustomUGChoice02] [nvarchar](max) NULL,
	[CustomUGChoice03] [nvarchar](max) NULL,
	[CustomUGChoice04] [nvarchar](max) NULL,
	[CustomUGDate01] [datetime] NULL,
	[CustomUGDate02] [datetime] NULL,
	[CustomUGDate03] [datetime] NULL,
	[CustomUGDate04] [datetime] NULL,
	[CustomUGText01] [nvarchar](250) NULL,
	[CustomUGText02] [nvarchar](250) NULL,
	[CustomUGText03] [nvarchar](250) NULL,
	[CustomUGText04] [nvarchar](250) NULL,
	[CustomUGText05] [nvarchar](250) NULL,
	[CustomUGText06] [nvarchar](250) NULL,
	[CustomUGText07] [nvarchar](250) NULL,
	[CustomUGText08] [nvarchar](250) NULL,
	[CustomUGUser01] [nvarchar](max) NULL,
	[CustomUGUser02] [nvarchar](max) NULL,
	[CustomUGUser03] [nvarchar](max) NULL,
	[CustomUGUser04] [nvarchar](max) NULL,
	[CustomUGUserMulti01] [nvarchar](250) NULL,
	[CustomUGUserMulti02] [nvarchar](250) NULL,
	[CustomUGUserMulti03] [nvarchar](250) NULL,
	[CustomUGUserMulti04] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[DesiredCompletionDateNotes] [nvarchar](max) NULL,
	[DivisionMultiLookup] [bigint] NOT NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[Duration] [int] NULL,
	[EliminatesHeadcount] [int] NULL,
	[EstimatedHours] [int] NULL,
	[FunctionalAreaLookup] [bigint] NOT NULL,
	[History] [nvarchar](max) NULL,
	[ImpactBusinessGrowth] [nvarchar](max) NULL,
	[ImpactDecisionMaking] [nvarchar](max) NULL,
	[ImpactIncreasesProductivity] [nvarchar](max) NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[ImpactReducesExpenses] [nvarchar](max) NULL,
	[ImpactReducesRisk] [nvarchar](max) NULL,
	[ImpactRevenueIncrease] [nvarchar](max) NULL,
	[ImprovesOperationalEfficiency] [nvarchar](max) NULL,
	[ImprovesRevenues] [nvarchar](max) NULL,
	[Initiator] [nvarchar](max) NULL,
	[InternalCapability] [nvarchar](max) NULL,
	[IsITGApprovalRequired] [bit] NULL,
	[IsPrivate] [bit] NULL,
	[IsProjectBudgeted] [bit] NULL,
	[IsSteeringApprovalRequired] [bit] NULL,
	[ITGReviewApproval] [nvarchar](max) NULL,
	[ITManager] [nvarchar](max) NULL,
	[ITSteeringCommitteeApproval] [nvarchar](max) NULL,
	[LocationMultLookup] [bigint] NOT NULL,
	[ManagerApprovalNeeded] [bit] NULL,
	[MetricsNotes] [nvarchar](max) NULL,
	[ModuleName] [nvarchar](250) NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NextSLATime] [datetime] NULL,
	[NextSLAType] [nvarchar](250) NULL,
	[NoAlternative] [nvarchar](250) NULL,
	[NoAlternativeOtherDescribe] [nvarchar](max) NULL,
	[NoOfConsultants] [int] NULL,
	[NoOfConsultantsNotes] [nvarchar](max) NULL,
	[NoOfFTEs] [int] NULL,
	[NoOfFTEsNotes] [nvarchar](max) NULL,
	[NoOfReports] [int] NULL,
	[NoOfReportsNotes] [nvarchar](max) NULL,
	[NoOfScreens] [int] NULL,
	[NoOfScreensNotes] [nvarchar](max) NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldStartDate] [datetime] NULL,
	[OnHoldTillDate] [datetime] NULL,
	[OrganizationalImpact] [nvarchar](max) NULL,
	[OtherDescribe] [nvarchar](max) NULL,
	[Owner] [nvarchar](250) NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[ProblemBeingSolved] [nvarchar](max) NULL,
	[ProjectAssumptions] [nvarchar](max) NULL,
	[ProjectAssumptionsDescription] [nvarchar](max) NULL,
	[ProjectBenefits] [nvarchar](max) NULL,
	[ProjectBenefitsDescription] [nvarchar](max) NULL,
	[ProjectClassLookup] [bigint] NOT NULL,
	[ProjectComplexity] [nvarchar](max) NULL,
	[ProjectCost] [nvarchar](250) NULL,
	[ProjectDirector] [nvarchar](max) NULL,
	[ProjectEstDurationMaxDays] [int] NULL,
	[ProjectEstDurationMinDays] [int] NULL,
	[ProjectEstSizeMaxHrs] [int] NULL,
	[ProjectEstSizeMinHrs] [int] NULL,
	[ProjectInitiativeLookup] [bigint] NOT NULL,
	[ProjectManager] [nvarchar](250) NULL,
	[ProjectRank] [nvarchar](250) NULL,
	[ProjectRank2] [nvarchar](250) NULL,
	[ProjectRank3] [nvarchar](250) NULL,
	[ProjectRisk] [nvarchar](max) NULL,
	[ProjectRiskNotes] [nvarchar](max) NULL,
	[ProjectScope] [nvarchar](max) NULL,
	[ProjectScopeDescription] [nvarchar](max) NULL,
	[ProjectScore] [nvarchar](250) NULL,
	[ProjectScoreNotes] [nvarchar](max) NULL,
	[PRPGroup] [nvarchar](max) NULL,
	[RapidRequest] [nvarchar](max) NULL,
	[ReducesCost] [nvarchar](max) NULL,
	[RegulatoryCompliance] [nvarchar](max) NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeSubCategory] [nvarchar](250) NULL,
	[RequestTypeWorkflow] [nvarchar](250) NULL,
	[RiskScore] [int] NULL,
	[RiskScoreNotes] [nvarchar](max) NULL,
	[ROI] [int] NULL,
	[ScheduleComplexity] [nvarchar](max) NULL,
	[SecurityManager] [nvarchar](250) NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[SeverityLookup] [bigint] NOT NULL,
	[Sponsors] [nvarchar](250) NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[StakeHolders] [nvarchar](250) NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[StrategicInitiative] [nvarchar](max) NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[Technology] [nvarchar](max) NULL,
	[TechnologyAvailability] [nvarchar](max) NULL,
	[TechnologyImpact] [nvarchar](max) NULL,
	[TechnologyIntegration] [nvarchar](max) NULL,
	[TechnologyNotes1] [nvarchar](max) NULL,
	[TechnologyReliability] [nvarchar](max) NULL,
	[TechnologyRisk] [nvarchar](max) NULL,
	[TechnologySecurity] [nvarchar](max) NULL,
	[TechnologyUsability] [nvarchar](max) NULL,
	[TicketId] [nvarchar](250) NULL,
	[TotalConsultantHeadcount] [int] NULL,
	[TotalConsultantHeadcountNotes] [nvarchar](max) NULL,
	[TotalCost] [nvarchar](250) NULL,
	[TotalCostsNotes] [nvarchar](max) NULL,
	[TotalHoldDuration] [int] NULL,
	[TotalOffSiteConsultantHeadcount] [int] NULL,
	[TotalOffSiteConsultantHeadcountNotes] [nvarchar](max) NULL,
	[TotalOnSiteConsultantHeadcount] [int] NULL,
	[TotalOnSiteConsultantHeadcountNotes] [nvarchar](max) NULL,
	[TotalStaffHeadcount] [int] NULL,
	[TotalStaffHeadcountNotes] [nvarchar](max) NULL,
	[UserQuestionSummary] [nvarchar](max) NULL,
	[VendorLookup] [bigint] NOT NULL,
	[VendorName] [nvarchar](250) NULL,
	[VendorSupport] [nvarchar](max) NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NPRResources]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NPRResources](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[_ResourceType] [nvarchar](250) NULL,
	[AllocationEndDate] [datetime] NULL,
	[AllocationStartDate] [datetime] NULL,
	[BudgetDescription] [nvarchar](max) NULL,
	[BudgetType] [nvarchar](250) NULL,
	[EstimatedHours] [int] NULL,
	[NoOfFTEs] [int] NULL,
	[NPRIdLookup] [bigint] NOT NULL,
	[RequestedResources] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
	[UserSkillLookup] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NPRTasks]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NPRTasks](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssignedTo] [nvarchar](max) NULL,
	[AssignToPct] [nvarchar](max) NULL,
	[Body] [nvarchar](max) NULL,
	[ChildCount] [int] NULL,
	[CompletionDate] [datetime] NULL,
	[Contribution] [int] NULL,
	[DueDate] [datetime] NULL,
	[Duration] [int] NULL,
	[IsCritical] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[ItemOrder] [int] NULL,
	[Level] [int] NULL,
	[NPRIdLookup] [bigint] NOT NULL,
	[ParentTask] [int] NULL,
	[PercentComplete] [int] NULL,
	[Predecessors] [nvarchar](250) NULL,
	[Priority] [nvarchar](max) NULL,
	[StartDate] [datetime] NULL,
	[Status] [nvarchar](max) NULL,
	[TaskBehaviour] [nvarchar](max) NULL,
	[TaskEstimatedHours] [int] NULL,
	[TaskGroup] [nvarchar](max) NULL,
	[UserSkillMultiLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Opportunity]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Opportunity](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AcceptedDate] [datetime] NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualStartDate] [datetime] NULL,
	[Address] [nvarchar](max) NULL,
	[AnalysisDetails] [nvarchar](max) NULL,
	[AssistantProjectManager] [nvarchar](max) NULL,
	[AutoAssign] [bit] NULL,
	[BidAmount] [nvarchar](250) NULL,
	[ChanceOfSuccess] [nvarchar](max) NULL,
	[Clarifications] [nvarchar](max) NULL,
	[ClientAcceptance] [bit] NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CompanyTitleLookup] [bigint] NOT NULL,
	[Competition] [nvarchar](max) NULL,
	[ContactLookup] [bigint] NOT NULL,
	[ContractedAmount] [nvarchar](250) NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[DepartmentLookup] [bigint] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Description1] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[DivisionLookup] [bigint] NOT NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[EmailAddress] [nvarchar](250) NULL,
	[FunctionalAreaLookup] [bigint] NOT NULL,
	[GoNoGo] [bit] NULL,
	[History] [nvarchar](max) NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[Initiator] [nvarchar](max) NULL,
	[IsPrivate] [bit] NULL,
	[ManagerApprovalNeeded] [bit] NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NextSLATime] [datetime] NULL,
	[NextSLAType] [nvarchar](250) NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldStartDate] [datetime] NULL,
	[OnHoldTillDate] [datetime] NULL,
	[OpportunityName] [nvarchar](250) NULL,
	[OpportunitySize] [nvarchar](250) NULL,
	[OrganizationLookup] [bigint] NOT NULL,
	[ORP] [nvarchar](max) NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[ProjectManager] [nvarchar](250) NULL,
	[ProjectName] [nvarchar](250) NULL,
	[ProjectTeamLookup] [bigint] NOT NULL,
	[PRP] [nvarchar](max) NULL,
	[PRPGroup] [nvarchar](max) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[ResolutionType] [nvarchar](max) NULL,
	[ReSubmissionDate] [datetime] NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[SquareFootage] [nvarchar](250) NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[Superintendent] [nvarchar](max) NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[Telephone] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Organization]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organization](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BusinessType] [nvarchar](250) NULL,
	[Certifications] [nvarchar](max) NULL,
	[City] [nvarchar](250) NULL,
	[CompanyName] [nvarchar](250) NULL,
	[ContractorLicense] [nvarchar](250) NULL,
	[Country] [nvarchar](250) NULL,
	[CRMStatus] [nvarchar](max) NULL,
	[Division] [nvarchar](max) NULL,
	[EmailAddress] [nvarchar](250) NULL,
	[Fax] [nvarchar](250) NULL,
	[FederalID] [nvarchar](250) NULL,
	[LegalName] [nvarchar](250) NULL,
	[MasterAgreement] [nvarchar](max) NULL,
	[OrganizationNote] [nvarchar](250) NULL,
	[OrganizationStatus] [nvarchar](max) NULL,
	[OrganizationType] [nvarchar](max) NULL,
	[ShortName] [nvarchar](250) NULL,
	[State] [nvarchar](250) NULL,
	[StreetAddress1] [nvarchar](250) NULL,
	[StreetAddress2] [nvarchar](250) NULL,
	[Telephone] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
	[Trade] [nvarchar](max) NULL,
	[WebsiteUrl] [nvarchar](250) NULL,
	[WorkRegion] [nvarchar](250) NULL,
	[WorkType] [nvarchar](250) NULL,
	[Zip] [nvarchar](250) NULL,
 CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PLCRequest]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PLCRequest](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AccountManager] [nvarchar](max) NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualStartDate] [datetime] NULL,
	[BillingStartDate] [datetime] NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[ChanceOfSuccess] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[ComponentsNeeded] [nvarchar](250) NULL,
	[CreationDate] [datetime] NULL,
	[CRMLookup] [bigint] NOT NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[FollowUp] [nvarchar](250) NULL,
	[History] [nvarchar](max) NULL,
	[InitialCost] [nvarchar](250) NULL,
	[Initiator] [nvarchar](max) NULL,
	[IsPrivate] [bit] NULL,
	[ITStaffSize] [int] NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NoOfLicenses] [int] NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldStartDate] [datetime] NULL,
	[OnHoldTillDate] [datetime] NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PlanResources] [nvarchar](250) NULL,
	[Price] [nvarchar](250) NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestSource] [nvarchar](max) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeWorkflow] [nvarchar](250) NULL,
	[ResolutionComments] [nvarchar](max) NULL,
	[ReSubmissionDate] [datetime] NULL,
	[ScopeOfServices] [nvarchar](max) NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PMMBaselineDetail]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PMMBaselineDetail](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BaselineComment] [nvarchar](max) NULL,
	[BaselineDate] [datetime] NULL,
	[BaselineNum] [int] NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMMBudget]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PMMBudget](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AllocationEndDate] [datetime] NULL,
	[AllocationStartDate] [datetime] NULL,
	[BudgetAmount] [nvarchar](250) NULL,
	[BudgetDescription] [nvarchar](max) NULL,
	[BudgetItem] [nvarchar](250) NULL,
	[BudgetLookup] [bigint] NOT NULL,
	[BudgetStatus] [int] NULL,
	[Comment] [nvarchar](max) NULL,
	[IsAutoCalculated] [bit] NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[UnapprovedAmount] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMMBudgetActuals]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PMMBudgetActuals](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AllocationEndDate] [datetime] NULL,
	[AllocationStartDate] [datetime] NULL,
	[BudgetAmount] [nvarchar](250) NULL,
	[BudgetDescription] [nvarchar](max) NULL,
	[PMMBudgetLookup] [bigint] NOT NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMMBudgetHistory]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PMMBudgetHistory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AllocationEndDate] [datetime] NULL,
	[AllocationStartDate] [datetime] NULL,
	[BaselineDate] [datetime] NULL,
	[BaselineNum] [int] NULL,
	[BudgetAmount] [nvarchar](250) NULL,
	[BudgetDescription] [nvarchar](max) NULL,
	[BudgetItem] [nvarchar](250) NULL,
	[BudgetLookup] [bigint] NOT NULL,
	[PMMBudgetLookup] [bigint] NOT NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMMComments]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PMMComments](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AccomplishmentDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[ProjectNote] [nvarchar](max) NULL,
	[ProjectNoteType] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMMCommentsHistory]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PMMCommentsHistory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AccomplishmentDate] [datetime] NULL,
	[BaselineDate] [datetime] NULL,
	[BaselineNum] [int] NULL,
	[EndDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[ProjectNote] [nvarchar](max) NULL,
	[ProjectNoteType] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMMEvents]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PMMEvents](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Category] [nvarchar](max) NULL,
	[Comments] [nvarchar](max) NULL,
	[Duration] [nvarchar](250) NULL,
	[EndDate] [datetime] NULL,
	[EventCanceled] [bit] NULL,
	[EventType] [nvarchar](250) NULL,
	[fAllDayEvent] [nvarchar](250) NULL,
	[fRecurrence] [nvarchar](250) NULL,
	[IsDeleted] [bit] NULL,
	[Location] [nvarchar](250) NULL,
	[MasterSeriesItemID] [nvarchar](250) NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[RecurrenceData] [nvarchar](max) NULL,
	[RecurrenceID] [datetime] NULL,
	[RecurrenceInfo] [nvarchar](max) NULL,
	[StartDate] [datetime] NULL,
	[Status] [nvarchar](250) NULL,
	[TimeZone] [nvarchar](250) NULL,
	[UID] [nvarchar](250) NULL,
	[Workspace] [nvarchar](250) NULL,
	[WorkspaceLink] [nvarchar](250) NULL,
	[XMLTZone] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMMIssues]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PMMIssues](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssignedTo] [nvarchar](250) NULL,
	[Body] [nvarchar](max) NULL,
	[ChildCount] [int] NULL,
	[Comment] [nvarchar](max) NULL,
	[Contribution] [int] NULL,
	[DueDate] [datetime] NULL,
	[Duration] [int] NULL,
	[IsDeleted] [bit] NULL,
	[IssueImpact] [nvarchar](max) NULL,
	[ItemOrder] [int] NULL,
	[Level] [int] NULL,
	[ParentTask] [int] NULL,
	[PercentComplete] [int] NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[Predecessors] [nvarchar](250) NULL,
	[Priority] [nvarchar](max) NULL,
	[ProposedDate] [datetime] NULL,
	[ProposedStatus] [nvarchar](max) NULL,
	[Resolution] [nvarchar](max) NULL,
	[ResolutionDate] [datetime] NULL,
	[StartDate] [datetime] NULL,
	[Status] [nvarchar](max) NULL,
	[TaskActualHours] [int] NULL,
	[TaskEstimatedHours] [int] NULL,
	[TaskGroup] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMMIssuesHistory]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PMMIssuesHistory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssignedTo] [nvarchar](250) NULL,
	[BaselineDate] [datetime] NULL,
	[BaselineNum] [int] NULL,
	[Body] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[DueDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[IssueImpact] [nvarchar](max) NULL,
	[ItemOrder] [int] NULL,
	[ParentTask] [int] NULL,
	[PercentComplete] [int] NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[Predecessors] [nvarchar](250) NULL,
	[Priority] [nvarchar](max) NULL,
	[Resolution] [nvarchar](max) NULL,
	[ResolutionDate] [datetime] NULL,
	[StartDate] [datetime] NULL,
	[Status] [nvarchar](max) NULL,
	[TaskActualHours] [int] NULL,
	[TaskEstimatedHours] [int] NULL,
	[TaskGroup] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMMMonthlyBudget]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PMMMonthlyBudget](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCost] [nvarchar](250) NULL,
	[AllocationStartDate] [datetime] NULL,
	[BudgetAmount] [nvarchar](250) NULL,
	[BudgetType] [nvarchar](250) NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMMMonthlyBudgetHistory]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PMMMonthlyBudgetHistory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCost] [nvarchar](250) NULL,
	[AllocationStartDate] [datetime] NULL,
	[BaselineDate] [datetime] NULL,
	[BaselineNum] [int] NULL,
	[BudgetAmount] [nvarchar](250) NULL,
	[BudgetType] [nvarchar](250) NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMMProjects]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PMMProjects](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualHour] [int] NULL,
	[ActualStartDate] [datetime] NULL,
	[ApprovedRFE] [nvarchar](250) NULL,
	[ApprovedRFEAmount] [nvarchar](250) NULL,
	[ApprovedRFEType] [nvarchar](max) NULL,
	[APPTitleLookup] [bigint] NOT NULL,
	[ArchitectureScore] [int] NULL,
	[ArchitectureScoreNotes] [nvarchar](max) NULL,
	[AutoAdjustAllocations] [bit] NULL,
	[Beneficiaries] [nvarchar](250) NULL,
	[BenefitsExperienced] [nvarchar](max) NULL,
	[BreakevenMonth] [nvarchar](250) NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[Classification] [nvarchar](max) NULL,
	[ClassificationImpact] [nvarchar](max) NULL,
	[ClassificationType] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CompanyMultiLookup] [bigint] NOT NULL,
	[ConstraintNotes] [nvarchar](max) NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[CustomUGChoice01] [nvarchar](max) NULL,
	[CustomUGChoice02] [nvarchar](max) NULL,
	[CustomUGChoice03] [nvarchar](max) NULL,
	[CustomUGChoice04] [nvarchar](max) NULL,
	[CustomUGDate01] [datetime] NULL,
	[CustomUGDate02] [datetime] NULL,
	[CustomUGDate03] [datetime] NULL,
	[CustomUGDate04] [datetime] NULL,
	[CustomUGText01] [nvarchar](250) NULL,
	[CustomUGText02] [nvarchar](250) NULL,
	[CustomUGText03] [nvarchar](250) NULL,
	[CustomUGText04] [nvarchar](250) NULL,
	[CustomUGText05] [nvarchar](250) NULL,
	[CustomUGText06] [nvarchar](250) NULL,
	[CustomUGText07] [nvarchar](250) NULL,
	[CustomUGText08] [nvarchar](250) NULL,
	[CustomUGUser01] [nvarchar](max) NULL,
	[CustomUGUser02] [nvarchar](max) NULL,
	[CustomUGUser03] [nvarchar](max) NULL,
	[CustomUGUser04] [nvarchar](max) NULL,
	[CustomUGUserMulti01] [nvarchar](250) NULL,
	[CustomUGUserMulti02] [nvarchar](250) NULL,
	[CustomUGUserMulti03] [nvarchar](250) NULL,
	[CustomUGUserMulti04] [nvarchar](250) NULL,
	[DaysToComplete] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[DivisionMultiLookup] [bigint] NOT NULL,
	[Duration] [int] NULL,
	[EliminatesStaffHeadCount] [nvarchar](250) NULL,
	[EstimatedHours] [int] NULL,
	[EstimatedRemainingHours] [int] NULL,
	[EstProjectSpend] [nvarchar](250) NULL,
	[EstProjectSpendComment] [nvarchar](max) NULL,
	[FunctionalAreaLookup] [bigint] NOT NULL,
	[History] [nvarchar](max) NULL,
	[ImpactAsImprovesRevenues] [nvarchar](250) NULL,
	[ImpactAsOperationalEfficiency] [nvarchar](250) NULL,
	[ImpactAsSaveMoney] [nvarchar](250) NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[Initiator] [nvarchar](max) NULL,
	[IsPrivate] [bit] NULL,
	[ITManager] [nvarchar](max) NULL,
	[LessonsLearned] [nvarchar](max) NULL,
	[LocationMultLookup] [bigint] NOT NULL,
	[ModuleName] [nvarchar](250) NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NextActivity] [nvarchar](250) NULL,
	[NextMilestone] [nvarchar](250) NULL,
	[NoOfConsultants] [int] NULL,
	[NoOfConsultantsNotes] [nvarchar](max) NULL,
	[NoOfFTEs] [int] NULL,
	[NoOfFTEsNotes] [nvarchar](max) NULL,
	[NPRIdLookup] [bigint] NOT NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldTillDate] [datetime] NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[ProblemBeingSolved] [nvarchar](max) NULL,
	[ProjectAssumptions] [nvarchar](max) NULL,
	[ProjectBenefits] [nvarchar](max) NULL,
	[ProjectClassLookup] [bigint] NOT NULL,
	[ProjectCost] [nvarchar](250) NULL,
	[ProjectCostNote] [nvarchar](max) NULL,
	[ProjectInitiativeLookup] [bigint] NOT NULL,
	[ProjectIteration] [int] NULL,
	[ProjectLifeCycleLookup] [bigint] NOT NULL,
	[ProjectManager] [nvarchar](250) NULL,
	[ProjectPhasePctComplete] [int] NULL,
	[ProjectRank] [nvarchar](250) NULL,
	[ProjectRank2] [nvarchar](250) NULL,
	[ProjectRank3] [nvarchar](250) NULL,
	[ProjectRiskNotes] [nvarchar](max) NULL,
	[ProjectScheduleNote] [nvarchar](max) NULL,
	[ProjectScope] [nvarchar](max) NULL,
	[ProjectScore] [nvarchar](250) NULL,
	[ProjectScoreNotes] [nvarchar](max) NULL,
	[ProjectStatus] [nvarchar](max) NULL,
	[ProjectSummaryNote] [nvarchar](max) NULL,
	[PRPGroup] [nvarchar](max) NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeSubCategory] [nvarchar](250) NULL,
	[RiskScore] [int] NULL,
	[RiskScoreNotes] [nvarchar](max) NULL,
	[ROI] [nvarchar](250) NULL,
	[ScrumLifeCycle] [bit] NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[SeverityLookup] [bigint] NOT NULL,
	[ShowProjectStatus] [bit] NULL,
	[Sponsors] [nvarchar](250) NULL,
	[SprintDuration] [int] NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[StakeHolders] [nvarchar](250) NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[TicketId] [nvarchar](250) NULL,
	[TotalConsultantHeadcount] [int] NULL,
	[TotalConsultantHeadcountNotes] [nvarchar](max) NULL,
	[TotalCost] [nvarchar](250) NULL,
	[TotalCostsNotes] [nvarchar](max) NULL,
	[TotalStaffHeadcount] [int] NULL,
	[TotalStaffHeadcountNotes] [nvarchar](max) NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_PMMProjects] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMMRisks]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PMMRisks](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssignedTo] [nvarchar](250) NULL,
	[ContingencyPlan] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
	[IssueImpact] [nvarchar](max) NULL,
	[MitigationPlan] [nvarchar](max) NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[RiskProbability] [int] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMMTasks]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PMMTasks](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssignedTo] [nvarchar](max) NULL,
	[AssignToPct] [nvarchar](max) NULL,
	[Body] [nvarchar](max) NULL,
	[ChildCount] [int] NULL,
	[Comment] [nvarchar](max) NULL,
	[CompletedBy] [nvarchar](max) NULL,
	[CompletionDate] [datetime] NULL,
	[Contribution] [int] NULL,
	[DueDate] [datetime] NULL,
	[Duration] [int] NULL,
	[EstimatedRemainingHours] [int] NULL,
	[IsCritical] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[IsMilestone] [bit] NULL,
	[ItemOrder] [int] NULL,
	[Level] [int] NULL,
	[LinkedDocuments] [nvarchar](250) NULL,
	[ParentTask] [int] NULL,
	[PercentComplete] [int] NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[Predecessors] [nvarchar](250) NULL,
	[Priority] [nvarchar](max) NULL,
	[ProposedDate] [datetime] NULL,
	[ProposedStatus] [nvarchar](max) NULL,
	[ShowOnProjectCalendar] [bit] NULL,
	[SprintLookup] [bigint] NOT NULL,
	[StageStep] [int] NULL,
	[StartDate] [datetime] NULL,
	[Status] [nvarchar](max) NULL,
	[TaskActualHours] [int] NULL,
	[TaskBehaviour] [nvarchar](max) NULL,
	[TaskEstimatedHours] [int] NULL,
	[TaskGroup] [nvarchar](max) NULL,
	[TaskReminderDays] [int] NULL,
	[TaskReminderEnabled] [bit] NULL,
	[TaskRepeatInterval] [int] NULL,
	[TicketId] [nvarchar](250) NULL,
	[UserSkillMultiLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[PMMIdLookup] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMMTasksHistory]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PMMTasksHistory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualHour] [int] NULL,
	[AssignedTo] [nvarchar](250) NULL,
	[AssignToPct] [nvarchar](max) NULL,
	[BaselineDate] [datetime] NULL,
	[BaselineNum] [int] NULL,
	[Body] [nvarchar](max) NULL,
	[CompletedBy] [nvarchar](max) NULL,
	[CompletionDate] [datetime] NULL,
	[DueDate] [datetime] NULL,
	[EstimatedRemainingHours] [int] NULL,
	[IsCritical] [bit] NULL,
	[IsMilestone] [bit] NULL,
	[ItemOrder] [int] NULL,
	[ParentTask] [int] NULL,
	[PercentComplete] [int] NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[Predecessors] [nvarchar](250) NULL,
	[Priority] [nvarchar](max) NULL,
	[ProposedStatus] [nvarchar](max) NULL,
	[SprintLookup] [bigint] NOT NULL,
	[StageStep] [int] NULL,
	[StartDate] [datetime] NULL,
	[Status] [nvarchar](max) NULL,
	[TaskActualHours] [int] NULL,
	[TaskBehaviour] [nvarchar](max) NULL,
	[TaskEstimatedHours] [int] NULL,
	[TaskGroup] [nvarchar](max) NULL,
	[TaskReminderDays] [int] NULL,
	[TaskReminderEnabled] [bit] NULL,
	[UserSkillMultiLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProjectMonitorState]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProjectMonitorState](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AutoCalculate] [bit] NULL,
	[ModuleMonitorNameLookup] [nvarchar](250) NOT NULL,
	[ModuleMonitorOptionIdLookup] [bigint] NOT NULL,
	[ModuleMonitorOptionLEDClassLookup] [bigint] NOT NULL,
	[ModuleMonitorOptionNameLookup] [bigint] NOT NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[ProjectMonitorNotes] [nvarchar](max) NULL,
	[ProjectMonitorWeight] [int] NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK__ProjectM__3214EC279C8414E0] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProjectMonitorStateHistory]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProjectMonitorStateHistory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BaselineDate] [datetime] NULL,
	[BaselineNum] [int] NULL,
	[ModuleMonitorNameLookup] [nvarchar](250) NOT NULL,
	[ModuleMonitorOptionIdLookup] [bigint] NOT NULL,
	[ModuleMonitorOptionLEDClassLookup] [bigint] NOT NULL,
	[ModuleMonitorOptionNameLookup] [bigint] NOT NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[ProjectMonitorNotes] [nvarchar](max) NULL,
	[ProjectMonitorWeight] [int] NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK__ProjectM__3214EC27D2F1ADA6] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProjectReleases]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProjectReleases](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ItemOrder] [int] NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[ReleaseDate] [datetime] NULL,
	[ReleaseID] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProjectSummary]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProjectSummary](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualStartDate] [datetime] NULL,
	[AssignedTo] [nvarchar](max) NULL,
	[Beneficiaries] [nvarchar](250) NULL,
	[Body] [nvarchar](max) NULL,
	[CategoryName] [nvarchar](250) NULL,
	[DueDate] [datetime] NULL,
	[ParentTask] [int] NULL,
	[PercentComplete] [int] NULL,
	[Predecessors] [nvarchar](250) NULL,
	[Priority] [nvarchar](max) NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[ProjectClassLookup] [bigint] NOT NULL,
	[ProjectCost] [nvarchar](250) NULL,
	[ProjectInitiativeLookup] [bigint] NOT NULL,
	[ProjectManager] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[ShowProjectStatus] [bit] NULL,
	[StartDate] [datetime] NULL,
	[Status] [nvarchar](max) NULL,
	[TaskGroup] [nvarchar](max) NULL,
	[TicketId] [nvarchar](250) NULL,
	[TotalCost] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PRS]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PRS](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualHours] [int] NULL,
	[ActualStartDate] [datetime] NULL,
	[APPTitleLookup] [bigint] NOT NULL,
	[AssetLookup] [bigint] NOT NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CompanyTitleLookup] [bigint] NOT NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[CustomUGChoice01] [nvarchar](max) NULL,
	[CustomUGChoice02] [nvarchar](max) NULL,
	[CustomUGChoice03] [nvarchar](max) NULL,
	[CustomUGChoice04] [nvarchar](max) NULL,
	[CustomUGDate01] [datetime] NULL,
	[CustomUGDate02] [datetime] NULL,
	[CustomUGDate03] [datetime] NULL,
	[CustomUGDate04] [datetime] NULL,
	[CustomUGText01] [nvarchar](250) NULL,
	[CustomUGText02] [nvarchar](250) NULL,
	[CustomUGText03] [nvarchar](250) NULL,
	[CustomUGText04] [nvarchar](250) NULL,
	[CustomUGText05] [nvarchar](250) NULL,
	[CustomUGText06] [nvarchar](250) NULL,
	[CustomUGText07] [nvarchar](250) NULL,
	[CustomUGText08] [nvarchar](250) NULL,
	[CustomUGUser01] [nvarchar](max) NULL,
	[CustomUGUser02] [nvarchar](max) NULL,
	[CustomUGUser03] [nvarchar](max) NULL,
	[CustomUGUser04] [nvarchar](max) NULL,
	[CustomUGUserMulti01] [nvarchar](250) NULL,
	[CustomUGUserMulti02] [nvarchar](250) NULL,
	[CustomUGUserMulti03] [nvarchar](250) NULL,
	[CustomUGUserMulti04] [nvarchar](250) NULL,
	[DepartmentLookup] [bigint] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[DivisionLookup] [bigint] NOT NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[EstimatedHours] [int] NULL,
	[FunctionalAreaLookup] [bigint] NOT NULL,
	[History] [nvarchar](max) NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[Initiator] [nvarchar](max) NULL,
	[InitiatorResolved] [nvarchar](max) NULL,
	[IsPrivate] [bit] NULL,
	[LocationLookup] [bigint] NOT NULL,
	[ManagerApprovalNeeded] [bit] NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NextSLATime] [datetime] NULL,
	[NextSLAType] [nvarchar](250) NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldStartDate] [datetime] NULL,
	[OnHoldTillDate] [datetime] NULL,
	[ORP] [nvarchar](250) NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[ProjectName] [nvarchar](250) NULL,
	[PRP] [nvarchar](max) NULL,
	[PRPGroup] [nvarchar](max) NULL,
	[ReopenCount] [int] NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestorContacted] [bit] NULL,
	[RequestSource] [nvarchar](max) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeSubCategory] [nvarchar](250) NULL,
	[RequestTypeWorkflow] [nvarchar](250) NULL,
	[ResolutionComments] [nvarchar](max) NULL,
	[ResolutionType] [nvarchar](max) NULL,
	[ReSubmissionDate] [datetime] NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[SeverityLookup] [bigint] NOT NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[Tester] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RCA]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RCA](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualStartDate] [datetime] NULL,
	[AnalysisDetails] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CompanyTitleLookup] [bigint] NOT NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[DepartmentLookup] [bigint] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[DeskLocation] [nvarchar](250) NULL,
	[DivisionLookup] [bigint] NOT NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[FunctionalAreaLookup] [bigint] NOT NULL,
	[History] [nvarchar](max) NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[Initiator] [nvarchar](max) NULL,
	[IsPrivate] [bit] NULL,
	[LocationLookup] [bigint] NOT NULL,
	[ManagerApprovalNeeded] [bit] NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NextSLATime] [datetime] NULL,
	[NextSLAType] [nvarchar](250) NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldStartDate] [datetime] NULL,
	[OnHoldTillDate] [datetime] NULL,
	[ORP] [nvarchar](max) NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[PRP] [nvarchar](max) NULL,
	[PRPGroup] [nvarchar](max) NULL,
	[RCAType] [nvarchar](max) NULL,
	[ReopenCount] [int] NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeWorkflow] [nvarchar](250) NULL,
	[ResolutionType] [nvarchar](max) NULL,
	[ReSubmissionDate] [datetime] NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
	[UserQuestionSummary] [nvarchar](max) NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Relationship]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Relationship](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ApprovalStatus] [nvarchar](250) NULL,
	[ApprovalType] [nvarchar](max) NULL,
	[ApprovedBy] [nvarchar](250) NULL,
	[Approver] [nvarchar](250) NULL,
	[AssignedTo] [nvarchar](max) NULL,
	[AutoCreateUser] [bit] NULL,
	[Body] [nvarchar](max) NULL,
	[ChildId] [nvarchar](250) NULL,
	[Comment] [nvarchar](max) NULL,
	[CompletedBy] [nvarchar](max) NULL,
	[CompletionDate] [datetime] NULL,
	[DueDate] [datetime] NULL,
	[EnableApproval] [bit] NULL,
	[ErrorMsg] [nvarchar](250) NULL,
	[IsDeleted] [bit] NULL,
	[ItemOrder] [int] NULL,
	[Level] [int] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[NewUserName] [nvarchar](250) NULL,
	[ParentId] [nvarchar](250) NULL,
	[ParentTask] [int] NULL,
	[PercentComplete] [int] NULL,
	[Predecessors] [nvarchar](250) NULL,
	[Priority] [nvarchar](max) NULL,
	[ServiceApplicationAccessXml] [nvarchar](max) NULL,
	[StageWeight] [int] NULL,
	[StartDate] [datetime] NULL,
	[Status] [nvarchar](max) NULL,
	[SubTaskType] [nvarchar](max) NULL,
	[TaskActionUser] [nvarchar](250) NULL,
	[TaskActualHours] [int] NULL,
	[TaskEstimatedHours] [int] NULL,
	[TaskGroup] [nvarchar](max) NULL,
	[TaskType] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ReportComponents]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReportComponents](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AskUser] [bit] NULL,
	[ComponentType] [nvarchar](max) NULL,
	[DashBoardPanelId] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[Height] [int] NULL,
	[ItemOrder] [int] NULL,
	[ReportDefinitionLookup] [bigint] NOT NULL,
	[RequestParams] [nvarchar](max) NULL,
	[ShowInNextLine] [bit] NULL,
	[Width] [int] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ReportDefinition]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReportDefinition](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_ReportDefinition] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RequestTypeByLocation]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RequestTypeByLocation](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssignmentSLA] [int] NULL,
	[CloseSLA] [int] NULL,
	[LocationLookup] [bigint] NOT NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[ORP] [nvarchar](max) NULL,
	[PRPGroup] [nvarchar](max) NULL,
	[RequestorContactSLA] [int] NULL,
	[RequestTypeBackupEscalationManager] [nvarchar](max) NULL,
	[RequestTypeEscalationManager] [nvarchar](max) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeOwner] [nvarchar](250) NULL,
	[ResolutionSLA] [int] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ResourceAllocation]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ResourceAllocation](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AllocationEndDate] [datetime] NULL,
	[AllocationStartDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[PctAllocation] [int] NULL,
	[PctPlannedAllocation] [int] NULL,
	[Resource] [nvarchar](max) NULL,
	[ResourceWorkItemLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ResourceAllocationMonthly]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceAllocationMonthly](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MonthStartDate] [datetime] NULL,
	[PctAllocation] [int] NULL,
	[PctPlannedAllocation] [int] NULL,
	[Resource] [nvarchar](max) NULL,
	[ResourceSubWorkItem] [nvarchar](250) NULL,
	[ResourceWorkItem] [nvarchar](250) NULL,
	[ResourceWorkItemLookup] [bigint] NOT NULL,
	[ResourceWorkItemType] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Resources]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Resources](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[_ResourceType] [nvarchar](250) NULL,
	[AllocationEndDate] [datetime] NULL,
	[AllocationStartDate] [datetime] NULL,
	[BudgetDescription] [nvarchar](max) NULL,
	[BudgetType] [nvarchar](250) NULL,
	[EstimatedHours] [int] NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[NoOfFTEs] [int] NULL,
	[RequestedResources] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
	[UserSkillLookup] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ResourceTimeSheet]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ResourceTimeSheet](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[HoursTaken] [int] NULL,
	[Resource] [nvarchar](max) NULL,
	[ResourceWorkItemLookup] [bigint] NOT NULL,
	[WorkDate] [datetime] NULL,
	[WorkDescription] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ResourceUsageSummaryMonthWise]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ResourceUsageSummaryMonthWise](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualHour] [int] NULL,
	[AllocationHour] [int] NULL,
	[FunctionalAreaTitle] [nvarchar](250) NULL,
	[IsConsultant] [bit] NULL,
	[IsIT] [bit] NULL,
	[IsManager] [bit] NULL,
	[ManagerLookup] [bigint] NOT NULL,
	[ManagerName] [nvarchar](250) NULL,
	[MonthStartDate] [datetime] NULL,
	[PctActual] [int] NULL,
	[PctAllocation] [int] NULL,
	[PctPlannedAllocation] [int] NULL,
	[PlannedAllocationHour] [int] NULL,
	[Resource] [nvarchar](max) NULL,
	[ResourceName] [nvarchar](250) NULL,
	[SubWorkItem] [nvarchar](250) NULL,
	[WorkItem] [nvarchar](250) NULL,
	[WorkItemID] [int] NULL,
	[WorkItemType] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ResourceUsageSummaryWeekWise]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ResourceUsageSummaryWeekWise](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualHour] [int] NULL,
	[AllocationHour] [int] NULL,
	[FunctionalAreaTitle] [nvarchar](250) NULL,
	[IsConsultant] [bit] NULL,
	[IsIT] [bit] NULL,
	[IsManager] [bit] NULL,
	[ManagerLookup] [bigint] NOT NULL,
	[ManagerName] [nvarchar](250) NULL,
	[PctActual] [int] NULL,
	[PctAllocation] [int] NULL,
	[PctPlannedAllocation] [int] NULL,
	[PlannedAllocationHour] [int] NULL,
	[Resource] [nvarchar](max) NULL,
	[ResourceName] [nvarchar](250) NULL,
	[SubWorkItem] [nvarchar](250) NULL,
	[WeekStartDate] [datetime] NULL,
	[WorkItem] [nvarchar](250) NULL,
	[WorkItemID] [int] NULL,
	[WorkItemType] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ResourceWorkItems]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ResourceWorkItems](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[Resource] [nvarchar](max) NULL,
	[SubWorkItem] [nvarchar](250) NULL,
	[WorkItem] [nvarchar](250) NULL,
	[WorkItemType] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
 CONSTRAINT [PK_ResourceWorkItems] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ScheduleActions]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduleActions](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActionType] [nvarchar](max) NULL,
	[ActionTypeData] [nvarchar](max) NULL,
	[AlertCondition] [nvarchar](250) NULL,
	[AttachmentFormat] [nvarchar](max) NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[EmailBody] [nvarchar](max) NULL,
	[EmailIDCC] [nvarchar](max) NULL,
	[EmailIDTo] [nvarchar](max) NULL,
	[ListName] [nvarchar](250) NULL,
	[MailSubject] [nvarchar](250) NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[Recurring] [bit] NULL,
	[RecurringEndDate] [datetime] NULL,
	[RecurringInterval] [int] NULL,
	[StartTime] [datetime] NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ScheduleActionsArchive]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduleActionsArchive](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActionType] [nvarchar](max) NULL,
	[ActionTypeData] [nvarchar](max) NULL,
	[AgentJobStatus] [nvarchar](max) NULL,
	[AlertCondition] [nvarchar](250) NULL,
	[AttachmentFormat] [nvarchar](max) NULL,
	[CustomProperties] [nvarchar](max) NULL,
	[EmailBody] [nvarchar](max) NULL,
	[EmailIDCC] [nvarchar](max) NULL,
	[EmailIDTo] [nvarchar](max) NULL,
	[ListName] [nvarchar](250) NULL,
	[Log] [nvarchar](max) NULL,
	[MailSubject] [nvarchar](250) NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[Recurring] [bit] NULL,
	[RecurringEndDate] [datetime] NULL,
	[RecurringInterval] [int] NULL,
	[StartTime] [datetime] NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sprint]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sprint](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[EndDate] [datetime] NULL,
	[ItemOrder] [int] NULL,
	[PercentComplete] [int] NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[RemainingHours] [int] NULL,
	[StartDate] [datetime] NULL,
	[TaskEstimatedHours] [int] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SprintSummary]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SprintSummary](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[EndDate] [datetime] NULL,
	[ItemOrder] [int] NULL,
	[PercentComplete] [int] NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[RemainingHours] [int] NULL,
	[SprintLookup] [bigint] NOT NULL,
	[StartDate] [datetime] NULL,
	[TaskEstimatedHours] [int] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SprintTasks]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SprintTasks](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssignedTo] [nvarchar](250) NULL,
	[Body] [nvarchar](max) NULL,
	[ChildCount] [int] NULL,
	[Comment] [nvarchar](max) NULL,
	[Contribution] [int] NULL,
	[Duration] [int] NULL,
	[IsDeleted] [bit] NULL,
	[IsMilestone] [bit] NULL,
	[ItemOrder] [int] NULL,
	[Level] [int] NULL,
	[ParentTask] [int] NULL,
	[PercentComplete] [int] NULL,
	[PMMIdLookup] [bigint] NOT NULL,
	[Priority] [nvarchar](max) NULL,
	[ProposedDate] [datetime] NULL,
	[ProposedStatus] [nvarchar](max) NULL,
	[ReleaseLookup] [bigint] NOT NULL,
	[ShowOnProjectCalendar] [bit] NULL,
	[SprintLookup] [bigint] NOT NULL,
	[SprintOrder] [int] NULL,
	[StageStep] [int] NULL,
	[StartDate] [datetime] NULL,
	[TaskActualHours] [int] NULL,
	[TaskBehaviour] [nvarchar](max) NULL,
	[TaskDueDate] [datetime] NULL,
	[TaskEstimatedHours] [int] NULL,
	[TaskGroup] [nvarchar](max) NULL,
	[TaskStatus] [nvarchar](max) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SurveyFeedback]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SurveyFeedback](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ModuleName] [nvarchar](250) NULL,
	[Rating1] [int] NULL,
	[Rating2] [int] NULL,
	[Rating3] [int] NULL,
	[Rating4] [int] NULL,
	[Rating5] [int] NULL,
	[Rating6] [int] NULL,
	[Rating7] [int] NULL,
	[Rating8] [int] NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
	[TotalRating] [int] NULL,
	[UserDepartment] [nvarchar](250) NULL,
	[UserLocation] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SVCRequests]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SVCRequests](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualHours] [int] NULL,
	[ActualStartDate] [datetime] NULL,
	[Approver] [nvarchar](250) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[EstimatedHours] [int] NULL,
	[History] [nvarchar](max) NULL,
	[Initiator] [nvarchar](max) NULL,
	[IsAllTaskComplete] [bit] NULL,
	[IsPrivate] [bit] NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldTillDate] [datetime] NULL,
	[Owner] [nvarchar](250) NULL,
	[OwnerApprovalRequired] [bit] NULL,
	[PctComplete] [int] NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[ProjectClassLookup] [bigint] NOT NULL,
	[ProjectInitiativeLookup] [bigint] NOT NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeSubCategory] [nvarchar](250) NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[ShowProjectStatus] [bit] NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[TicketId] [nvarchar](250) NULL,
	[UserQuestionSummary] [nvarchar](max) NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TaskTemplateItems]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskTemplateItems](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssignedTo] [nvarchar](max) NULL,
	[AssignToPct] [nvarchar](max) NULL,
	[Body] [nvarchar](max) NULL,
	[ChildCount] [int] NULL,
	[Comment] [nvarchar](max) NULL,
	[Contribution] [int] NULL,
	[DueDate] [datetime] NULL,
	[Duration] [int] NULL,
	[IsMilestone] [bit] NULL,
	[ItemOrder] [int] NULL,
	[Level] [int] NULL,
	[ParentTask] [int] NULL,
	[PercentComplete] [int] NULL,
	[Predecessors] [nvarchar](250) NULL,
	[Priority] [nvarchar](max) NULL,
	[StageStep] [int] NULL,
	[StartDate] [datetime] NULL,
	[Status] [nvarchar](max) NULL,
	[TaskActualHours] [int] NULL,
	[TaskBehaviour] [nvarchar](max) NULL,
	[TaskEstimatedHours] [int] NULL,
	[TaskGroup] [nvarchar](max) NULL,
	[TaskTemplateLookup] [bigint] NOT NULL,
	[Title] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TaskTemplates]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskTemplates](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ProjectLifeCycleLookup] [bigint] NOT NULL,
	[Title] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Templates]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Templates](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[FieldValues] [nvarchar](max) NULL,
	[ModuleNameLookup] [nvarchar](250) NULL,
	[TemplateType] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TSKProjects]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TSKProjects](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualHour] [int] NULL,
	[ActualStartDate] [datetime] NULL,
	[ArchitectureScore] [int] NULL,
	[ArchitectureScoreNotes] [nvarchar](max) NULL,
	[AutoAdjustAllocations] [bit] NULL,
	[Beneficiaries] [nvarchar](250) NULL,
	[BenefitsExperienced] [nvarchar](max) NULL,
	[BreakevenMonth] [nvarchar](250) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CompanyMultiLookup] [bigint] NOT NULL,
	[ConstraintNotes] [nvarchar](max) NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[DaysToComplete] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[DivisionMultiLookup] [bigint] NOT NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[Duration] [int] NULL,
	[EliminatesStaffHeadCount] [nvarchar](250) NULL,
	[EstimatedHours] [int] NULL,
	[EstimatedRemainingHours] [int] NULL,
	[FunctionalAreaLookup] [bigint] NOT NULL,
	[History] [nvarchar](max) NULL,
	[ImpactAsImprovesRevenues] [nvarchar](250) NULL,
	[ImpactAsOperationalEfficiency] [nvarchar](250) NULL,
	[ImpactAsSaveMoney] [nvarchar](250) NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[Initiator] [nvarchar](max) NULL,
	[ITManager] [nvarchar](max) NULL,
	[LessonsLearned] [nvarchar](max) NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NextActivity] [nvarchar](250) NULL,
	[NextMilestone] [nvarchar](250) NULL,
	[NoOfConsultants] [int] NULL,
	[NoOfConsultantsNotes] [nvarchar](max) NULL,
	[NoOfFTEs] [int] NULL,
	[NoOfFTEsNotes] [nvarchar](max) NULL,
	[NPRIdLookup] [bigint] NOT NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldTillDate] [datetime] NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[ProjectClassLookup] [bigint] NOT NULL,
	[ProjectCost] [nvarchar](250) NULL,
	[ProjectCostNote] [nvarchar](max) NULL,
	[ProjectInitiativeLookup] [bigint] NOT NULL,
	[ProjectManager] [nvarchar](250) NULL,
	[ProjectPhasePctComplete] [int] NULL,
	[ProjectScheduleNote] [nvarchar](max) NULL,
	[ProjectScore] [nvarchar](250) NULL,
	[ProjectScoreNotes] [nvarchar](max) NULL,
	[ProjectSummaryNote] [nvarchar](max) NULL,
	[Requestor] [nvarchar](max) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeSubCategory] [nvarchar](250) NULL,
	[RiskScore] [int] NULL,
	[RiskScoreNotes] [nvarchar](max) NULL,
	[ROI] [nvarchar](250) NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[SeverityLookup] [bigint] NOT NULL,
	[ShowProjectStatus] [bit] NULL,
	[Sponsors] [nvarchar](250) NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[StakeHolders] [nvarchar](250) NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[TicketId] [nvarchar](250) NULL,
	[TotalConsultantHeadcount] [int] NULL,
	[TotalConsultantHeadcountNotes] [nvarchar](max) NULL,
	[TotalCost] [nvarchar](250) NULL,
	[TotalCostsNotes] [nvarchar](max) NULL,
	[TotalStaffHeadcount] [int] NULL,
	[TotalStaffHeadcountNotes] [nvarchar](max) NULL,
	[UserQuestionSummary] [nvarchar](max) NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TSKTasks]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TSKTasks](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssignedTo] [nvarchar](max) NULL,
	[AssignToPct] [nvarchar](max) NULL,
	[Body] [nvarchar](max) NULL,
	[ChildCount] [int] NULL,
	[Comment] [nvarchar](max) NULL,
	[CompletedBy] [nvarchar](max) NULL,
	[CompletionDate] [datetime] NULL,
	[Contribution] [int] NULL,
	[DueDate] [datetime] NULL,
	[Duration] [int] NULL,
	[EstimatedRemainingHours] [int] NULL,
	[IsCritical] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[ItemOrder] [int] NULL,
	[Level] [int] NULL,
	[ParentTask] [int] NULL,
	[PercentComplete] [int] NULL,
	[Predecessors] [nvarchar](250) NULL,
	[Priority] [nvarchar](max) NULL,
	[ProposedDate] [datetime] NULL,
	[ProposedStatus] [nvarchar](max) NULL,
	[StartDate] [datetime] NULL,
	[Status] [nvarchar](max) NULL,
	[TaskActualHours] [int] NULL,
	[TaskBehaviour] [nvarchar](max) NULL,
	[TaskEstimatedHours] [int] NULL,
	[TaskGroup] [nvarchar](max) NULL,
	[TaskReminderDays] [int] NULL,
	[TaskReminderEnabled] [bit] NULL,
	[TaskRepeatInterval] [int] NULL,
	[TSKIDLookup] [bigint] NOT NULL,
	[UserSkillMultiLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TSR]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TSR](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualHours] [int] NULL,
	[ActualStartDate] [datetime] NULL,
	[APPTitleLookup] [bigint] NULL,
	[AssetLookup] [bigint] NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CompanyTitleLookup] [bigint] NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[CustomUGChoice01] [nvarchar](max) NULL,
	[CustomUGChoice02] [nvarchar](max) NULL,
	[CustomUGChoice03] [nvarchar](max) NULL,
	[CustomUGChoice04] [nvarchar](max) NULL,
	[CustomUGDate01] [datetime] NULL,
	[CustomUGDate02] [datetime] NULL,
	[CustomUGDate03] [datetime] NULL,
	[CustomUGDate04] [datetime] NULL,
	[CustomUGText01] [nvarchar](250) NULL,
	[CustomUGText02] [nvarchar](250) NULL,
	[CustomUGText03] [nvarchar](250) NULL,
	[CustomUGText04] [nvarchar](250) NULL,
	[CustomUGText05] [nvarchar](250) NULL,
	[CustomUGText06] [nvarchar](250) NULL,
	[CustomUGText07] [nvarchar](250) NULL,
	[CustomUGText08] [nvarchar](250) NULL,
	[CustomUGUser01] [nvarchar](max) NULL,
	[CustomUGUser02] [nvarchar](max) NULL,
	[CustomUGUser03] [nvarchar](max) NULL,
	[CustomUGUser04] [nvarchar](max) NULL,
	[CustomUGUserMulti01] [nvarchar](250) NULL,
	[CustomUGUserMulti02] [nvarchar](250) NULL,
	[CustomUGUserMulti03] [nvarchar](250) NULL,
	[CustomUGUserMulti04] [nvarchar](250) NULL,
	[DepartmentLookup] [bigint] NULL,
	[Description] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[DeskLocation] [nvarchar](250) NULL,
	[DivisionLookup] [bigint] NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[EstimatedHours] [int] NULL,
	[ExternalID] [nvarchar](250) NULL,
	[FunctionalAreaLookup] [bigint] NULL,
	[GLCode] [nvarchar](250) NULL,
	[History] [nvarchar](max) NULL,
	[ImpactLookup] [bigint] NULL,
	[InfrastructureManager] [nvarchar](max) NULL,
	[Initiator] [nvarchar](max) NULL,
	[InitiatorResolved] [nvarchar](max) NULL,
	[IsPrivate] [bit] NULL,
	[IssueType] [nvarchar](250) NULL,
	[LocationLookup] [bigint] NULL,
	[ManagerApprovalNeeded] [bit] NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NextSLATime] [datetime] NULL,
	[NextSLAType] [nvarchar](250) NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldStartDate] [datetime] NULL,
	[OnHoldTillDate] [datetime] NULL,
	[ORP] [nvarchar](max) NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PriorityLookup] [bigint] NULL,
	[PRP] [nvarchar](max) NULL,
	[PRPGroup] [nvarchar](max) NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestorContacted] [bit] NULL,
	[RequestSource] [nvarchar](max) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NULL,
	[RequestTypeSubCategory] [nvarchar](250) NULL,
	[RequestTypeWorkflow] [nvarchar](250) NULL,
	[ResolutionComments] [nvarchar](max) NULL,
	[ResolutionType] [nvarchar](max) NULL,
	[ReSubmissionDate] [datetime] NULL,
	[SecurityManager] [nvarchar](250) NULL,
	[ServiceLookUp] [bigint] NULL,
	[SeverityLookup] [bigint] NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[Tester] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TSR_bkp]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TSR_bkp](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualHours] [int] NULL,
	[ActualStartDate] [datetime] NULL,
	[APPTitleLookup] [bigint] NULL,
	[AssetLookup] [bigint] NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CompanyTitleLookup] [bigint] NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[CustomUGChoice01] [nvarchar](max) NULL,
	[CustomUGChoice02] [nvarchar](max) NULL,
	[CustomUGChoice03] [nvarchar](max) NULL,
	[CustomUGChoice04] [nvarchar](max) NULL,
	[CustomUGDate01] [datetime] NULL,
	[CustomUGDate02] [datetime] NULL,
	[CustomUGDate03] [datetime] NULL,
	[CustomUGDate04] [datetime] NULL,
	[CustomUGText01] [nvarchar](250) NULL,
	[CustomUGText02] [nvarchar](250) NULL,
	[CustomUGText03] [nvarchar](250) NULL,
	[CustomUGText04] [nvarchar](250) NULL,
	[CustomUGText05] [nvarchar](250) NULL,
	[CustomUGText06] [nvarchar](250) NULL,
	[CustomUGText07] [nvarchar](250) NULL,
	[CustomUGText08] [nvarchar](250) NULL,
	[CustomUGUser01] [nvarchar](max) NULL,
	[CustomUGUser02] [nvarchar](max) NULL,
	[CustomUGUser03] [nvarchar](max) NULL,
	[CustomUGUser04] [nvarchar](max) NULL,
	[CustomUGUserMulti01] [nvarchar](250) NULL,
	[CustomUGUserMulti02] [nvarchar](250) NULL,
	[CustomUGUserMulti03] [nvarchar](250) NULL,
	[CustomUGUserMulti04] [nvarchar](250) NULL,
	[DepartmentLookup] [bigint] NULL,
	[Description] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[DeskLocation] [nvarchar](250) NULL,
	[DivisionLookup] [bigint] NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[EstimatedHours] [int] NULL,
	[ExternalID] [nvarchar](250) NULL,
	[FunctionalAreaLookup] [bigint] NULL,
	[GLCode] [nvarchar](250) NULL,
	[History] [nvarchar](max) NULL,
	[ImpactLookup] [bigint] NULL,
	[InfrastructureManager] [nvarchar](max) NULL,
	[Initiator] [nvarchar](max) NULL,
	[InitiatorResolved] [nvarchar](max) NULL,
	[IsPrivate] [bit] NULL,
	[IssueType] [nvarchar](250) NULL,
	[LocationLookup] [bigint] NULL,
	[ManagerApprovalNeeded] [bit] NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NextSLATime] [datetime] NULL,
	[NextSLAType] [nvarchar](250) NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldStartDate] [datetime] NULL,
	[OnHoldTillDate] [datetime] NULL,
	[ORP] [nvarchar](max) NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PriorityLookup] [bigint] NULL,
	[PRP] [nvarchar](max) NULL,
	[PRPGroup] [nvarchar](max) NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestorContacted] [bit] NULL,
	[RequestSource] [nvarchar](max) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NULL,
	[RequestTypeSubCategory] [nvarchar](250) NULL,
	[RequestTypeWorkflow] [nvarchar](250) NULL,
	[ResolutionComments] [nvarchar](max) NULL,
	[ResolutionType] [nvarchar](max) NULL,
	[ReSubmissionDate] [datetime] NULL,
	[SecurityManager] [nvarchar](250) NULL,
	[ServiceLookUp] [bigint] NULL,
	[SeverityLookup] [bigint] NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[Tester] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UPR]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UPR](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualHours] [int] NULL,
	[ActualStartDate] [datetime] NULL,
	[AssetLookup] [bigint] NOT NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[DepartmentLookup] [bigint] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[EstimatedHours] [int] NULL,
	[FunctionalAreaLookup] [bigint] NOT NULL,
	[GLCode] [nvarchar](250) NULL,
	[History] [nvarchar](max) NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[Initiator] [nvarchar](max) NULL,
	[InitiatorResolved] [nvarchar](max) NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[OnHold] [int] NULL,
	[OnHoldStartDate] [datetime] NULL,
	[ORP] [nvarchar](250) NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[PRP] [nvarchar](max) NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestSource] [nvarchar](max) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeWorkflow] [nvarchar](250) NULL,
	[ResoltuonComments] [nvarchar](max) NULL,
	[ReSubmissionDate] [datetime] NULL,
	[SecurityManager] [nvarchar](250) NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[SeverityLookup] [bigint] NOT NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[Tester] [nvarchar](max) NULL,
	[TicketId] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserProfile](
	[ID] [varchar](128) NULL,
	[LoginName] [varchar](100) NULL,
	[Name] [varchar](100) NULL,
	[Email] [varchar](256) NULL,
	[Department] [varchar](100) NULL,
	[HourlyRate] [int] NULL,
	[Location] [varchar](100) NULL,
	[Manager] [varchar](100) NULL,
	[MobilePhone] [varchar](100) NULL,
	[JobProfile] [varchar](100) NULL,
	[IsIT] [bit] NULL,
	[IsConsultant] [bit] NULL,
	[IsManager] [bit] NULL,
	[LocationId] [int] NULL,
	[DepartmentId] [int] NULL,
	[RoleId] [int] NULL,
	[RoleName] [varchar](100) NULL,
	[Enabled] [bit] NULL,
	[FunctionalArea] [int] NULL,
	[BudgetCategory] [int] NULL,
	[DeskLocation] [varchar](100) NULL,
	[UGITStartDate] [datetime] NULL,
	[UGITEndDate] [datetime] NULL,
	[EnablePasswordExpiration] [bit] NULL,
	[PasswordExpiryDate] [datetime] NULL,
	[DisableWorkflowNotifications] [bit] NULL,
	[Picture] [varchar](500) NULL,
	[NotificationEmail] [varchar](256) NULL,
	[ApproveLevelAmount] [float] NULL,
	[LeaveFromDate] [datetime] NULL,
	[LeaveToDate] [datetime] NULL,
	[EnableOutofOffice] [bit] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserSkills]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSkills](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[Title] [nvarchar](250) NULL,
 CONSTRAINT [PK_UserSkills] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VCCRequest]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VCCRequest](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualHours] [int] NULL,
	[ActualStartDate] [datetime] NULL,
	[AmendmentEffectiveDate] [datetime] NULL,
	[AmendmentId] [nvarchar](250) NULL,
	[AmendmentReceived] [datetime] NULL,
	[APPTitleLookup] [bigint] NOT NULL,
	[AssetLookup] [bigint] NOT NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CommercialMgtLead] [nvarchar](max) NULL,
	[CompanyTitleLookup] [bigint] NOT NULL,
	[CreationDate] [datetime] NULL,
	[CRNumber] [nvarchar](250) NULL,
	[CRSignedDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[CustomUGDate01] [datetime] NULL,
	[CustomUGDate02] [datetime] NULL,
	[CustomUGDate03] [datetime] NULL,
	[CustomUGDate04] [datetime] NULL,
	[CustomUGText01] [nvarchar](250) NULL,
	[CustomUGText02] [nvarchar](250) NULL,
	[CustomUGText03] [nvarchar](250) NULL,
	[CustomUGText04] [nvarchar](250) NULL,
	[CustomUGText05] [nvarchar](250) NULL,
	[CustomUGText06] [nvarchar](250) NULL,
	[CustomUGText07] [nvarchar](250) NULL,
	[CustomUGText08] [nvarchar](250) NULL,
	[CustomUGUser01] [nvarchar](max) NULL,
	[CustomUGUser02] [nvarchar](max) NULL,
	[CustomUGUser03] [nvarchar](max) NULL,
	[CustomUGUser04] [nvarchar](max) NULL,
	[CustomUGUserMulti01] [nvarchar](250) NULL,
	[CustomUGUserMulti02] [nvarchar](250) NULL,
	[CustomUGUserMulti03] [nvarchar](250) NULL,
	[CustomUGUserMulti04] [nvarchar](250) NULL,
	[DeliverableImpact] [bit] NULL,
	[DepartmentLookup] [bigint] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DescriptionofService] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[DivisionLookup] [bigint] NOT NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[EstimatedHours] [int] NULL,
	[FinancialImpactAmount] [nvarchar](250) NULL,
	[FolderUrl] [nvarchar](250) NULL,
	[FunctionalAreaLookup] [bigint] NOT NULL,
	[History] [nvarchar](max) NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[Initiator] [nvarchar](max) NULL,
	[InitiatorResolved] [nvarchar](max) NULL,
	[IsPrivate] [bit] NULL,
	[LocationLookup] [bigint] NOT NULL,
	[ManagerApprovalNeeded] [bit] NULL,
	[MGSSubmittedDate] [datetime] NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NextSLATime] [datetime] NULL,
	[NextSLAType] [nvarchar](250) NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldStartDate] [datetime] NULL,
	[OnHoldTillDate] [datetime] NULL,
	[ORP] [nvarchar](max) NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[ProjectName] [nvarchar](250) NULL,
	[PRP] [nvarchar](max) NULL,
	[PRPGroup] [nvarchar](max) NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestorContacted] [bit] NULL,
	[RequestorName] [nvarchar](250) NULL,
	[RequestorOrganization] [nvarchar](250) NULL,
	[RequestSource] [nvarchar](max) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeWorkflow] [nvarchar](250) NULL,
	[ResolutionComments] [nvarchar](max) NULL,
	[ResolutionType] [nvarchar](max) NULL,
	[ReSubmissionDate] [datetime] NULL,
	[RFSFormComplete] [bit] NULL,
	[RFSSubmissionDate] [datetime] NULL,
	[RFSSubmitted] [bit] NULL,
	[SCRBDate] [datetime] NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[SeverityLookup] [bigint] NOT NULL,
	[SLAImpact] [bit] NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[SubmissionDate] [datetime] NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[Tester] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
	[UserQuestionSummary] [nvarchar](max) NULL,
	[VendorCRSignedDate] [datetime] NULL,
	[VendorMSALookup] [bigint] NOT NULL,
	[VendorMSANameLookup] [bigint] NOT NULL,
	[VendorSOWLookup] [bigint] NOT NULL,
	[VendorSOWNameLookup] [bigint] NOT NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorApprovedSubcontractors]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VendorApprovedSubcontractors](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ApprovedSubContractorName] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[SubContractorService] [nvarchar](250) NULL,
	[VendorMSALookup] [bigint] NOT NULL,
	[VendorMSANameLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorIssues]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VendorIssues](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssignedTo] [nvarchar](250) NULL,
	[Body] [nvarchar](max) NULL,
	[ChildCount] [int] NULL,
	[Comment] [nvarchar](max) NULL,
	[Contribution] [int] NULL,
	[DueDate] [datetime] NULL,
	[Duration] [int] NULL,
	[IsDeleted] [bit] NULL,
	[ItemOrder] [int] NULL,
	[Level] [int] NULL,
	[ParentTask] [int] NULL,
	[PercentComplete] [int] NULL,
	[Predecessors] [nvarchar](250) NULL,
	[Priority] [nvarchar](max) NULL,
	[ProposedDate] [datetime] NULL,
	[ProposedStatus] [nvarchar](max) NULL,
	[Resolution] [nvarchar](max) NULL,
	[ResolutionDate] [datetime] NULL,
	[StartDate] [datetime] NULL,
	[Status] [nvarchar](max) NULL,
	[TaskGroup] [nvarchar](max) NULL,
	[Title] [nvarchar](250) NULL,
	[VendorIssueImpact] [nvarchar](max) NULL,
	[VendorMSALookup] [bigint] NOT NULL,
	[VendorMSANameLookup] [bigint] NOT NULL,
	[VendorSOWLookup] [bigint] NOT NULL,
	[VendorSOWNameLookup] [bigint] NOT NULL,
	[VNDActionType] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/**************************object:TabView**************************/
create table Config_TabView(TabId int, TabName varchar(50), TabDisplayName varchar(100), ViewName varchar(100), ModuleName varchar(10), TabOrder int, ColumnViewName varchar(20))


/****** Object:  Table [dbo].[VendorKeyPersonnel]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[VendorKeyPersonnel](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[EmailAddress] [nvarchar](250) NULL,
	[EndDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[StartDate] [datetime] NULL,
	[VendorMSALookup] [bigint] NOT NULL,
	[VendorMSANameLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorMSA]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VendorMSA](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AdditionalComments] [nvarchar](max) NULL,
	[AdditionalCommentsByVendor] [nvarchar](max) NULL,
	[AdditionalInformation] [nvarchar](max) NULL,
	[AgreementNumber] [nvarchar](250) NULL,
	[AnnualRevenue] [nvarchar](250) NULL,
	[AttritionInAccount] [int] NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[ClientTerminationTriggers] [nvarchar](250) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[ComplianceBreachesLastYear] [int] NULL,
	[ContactName] [nvarchar](250) NULL,
	[ContractSigningDate] [datetime] NULL,
	[ContractValue] [nvarchar](250) NULL,
	[ContractValueDetails] [nvarchar](max) NULL,
	[CreationDate] [datetime] NULL,
	[CSATScore] [int] NULL,
	[Currency] [nvarchar](250) NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[DeliveryMisses] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[DisentanglementClausePresent] [bit] NULL,
	[DisentanglementTerm] [int] NULL,
	[DisentanglementTransitionPlan] [int] NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[EffectiveEndDate] [datetime] NULL,
	[EffectiveStartDate] [datetime] NULL,
	[FinancialManager] [nvarchar](max) NULL,
	[FirmAttrition] [int] NULL,
	[FolderUrl] [nvarchar](250) NULL,
	[FunctionalManager] [nvarchar](max) NULL,
	[GrowthOverLastYear] [int] NULL,
	[History] [nvarchar](max) NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[InitialContractTerm] [int] NULL,
	[Initiator] [nvarchar](max) NULL,
	[IsPrivate] [bit] NULL,
	[IssueResolutionProcessExists] [bit] NULL,
	[IssueResolutionProcessSummary] [nvarchar](max) NULL,
	[KeyRefUniqueID] [nvarchar](250) NULL,
	[LeadershipChurn] [bit] NULL,
	[LegalApprovedSubcontractors] [nvarchar](250) NULL,
	[LegalCompliances] [nvarchar](250) NULL,
	[LegalJurisdiction] [nvarchar](250) NULL,
	[LegalPublicity] [nvarchar](max) NULL,
	[LegalWarranties] [nvarchar](max) NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NoOfEmployee] [int] NULL,
	[NumberOfClient] [int] NULL,
	[NumberofMisses] [int] NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldStartDate] [datetime] NULL,
	[OnHoldTillDate] [datetime] NULL,
	[OtherClientsBreachesReported] [int] NULL,
	[OtherPaymentTerms] [nvarchar](max) NULL,
	[Owner] [nvarchar](250) NULL,
	[PaymentDelayInterest] [nvarchar](max) NULL,
	[PaymentDueTerm] [int] NULL,
	[PctComplete] [int] NULL,
	[PerformanceManager] [nvarchar](max) NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[Profitability] [int] NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequiredNotifications] [nvarchar](max) NULL,
	[ResolutionType] [nvarchar](max) NULL,
	[ReSubmissionDate] [datetime] NULL,
	[Risk] [nvarchar](max) NULL,
	[RiskScore] [int] NULL,
	[ServiceDeliveryManager] [nvarchar](max) NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[SeverityLookup] [bigint] NOT NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[TerminationByClient] [bit] NULL,
	[TerminationbyClientForceMajeureEvent] [bit] NULL,
	[TerminationByVendor] [bit] NULL,
	[TerminationChargesByVendor] [nvarchar](max) NULL,
	[TerminationForConvenienceAllowed] [bit] NULL,
	[TerminationForIncurredLiability] [bit] NULL,
	[TerminationNoticePeriod] [nvarchar](250) NULL,
	[TerminationNoticePeriodByVendor] [nvarchar](250) NULL,
	[TerminationTriggerByVendor] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
	[UserQuestionSummary] [nvarchar](max) NULL,
	[VendorAddress] [nvarchar](max) NULL,
	[VendorDetailLookup] [bigint] NOT NULL,
	[VendorEmail] [nvarchar](250) NULL,
	[VendorLocation] [nvarchar](250) NULL,
	[VendorName] [nvarchar](250) NULL,
	[VendorPhone] [nvarchar](250) NULL,
	[VMOApprover] [nvarchar](250) NULL,
	[WebsiteUrl] [nvarchar](250) NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[YearInBusiness] [int] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorMSAMeeting]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VendorMSAMeeting](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MeetingFrequency] [int] NULL,
	[MeetingFrequencyUnit] [nvarchar](max) NULL,
	[MeetingMaterial] [nvarchar](max) NULL,
	[MeetingParticipants] [nvarchar](250) NULL,
	[VendorMeetingAgenda] [nvarchar](max) NULL,
	[VendorMeetingType] [nvarchar](250) NULL,
	[VendorMSALookup] [bigint] NOT NULL,
	[VendorMSANameLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorPO]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VendorPO](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BilledAmount] [nvarchar](250) NULL,
	[BudgetAmount] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[EndDate] [datetime] NULL,
	[PONumber] [nvarchar](250) NULL,
	[PrespentAmount] [nvarchar](250) NULL,
	[StartDate] [datetime] NULL,
	[VendorMSALookup] [bigint] NOT NULL,
	[VendorMSANameLookup] [bigint] NOT NULL,
	[VendorSOWLookup] [bigint] NOT NULL,
	[VendorSOWNameLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorPOLineItems]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VendorPOLineItems](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BilledAmount] [nvarchar](250) NULL,
	[BudgetAmount] [nvarchar](250) NULL,
	[Cost] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[EndDate] [datetime] NULL,
	[LineItemNumber] [nvarchar](250) NULL,
	[PrespentAmount] [nvarchar](250) NULL,
	[StartDate] [datetime] NULL,
	[VendorMSALookup] [bigint] NOT NULL,
	[VendorMSANameLookup] [bigint] NOT NULL,
	[VendorPOLookup] [bigint] NOT NULL,
	[VendorSOWLookup] [bigint] NOT NULL,
	[VendorSOWNameLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorReport]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VendorReport](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AcceptanceCriteria] [nvarchar](max) NULL,
	[ClientObligations] [nvarchar](max) NULL,
	[DeliverableAttributes] [nvarchar](max) NULL,
	[DeliverableMode] [nvarchar](max) NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[FolderUrl] [nvarchar](250) NULL,
	[IsDeleted] [bit] NULL,
	[NoDocumentNeeded] [bit] NULL,
	[ReminderDays] [int] NULL,
	[ReminderType] [nvarchar](max) NULL,
	[ReportFrequencyType] [nvarchar](max) NULL,
	[ReportingFrequency] [int] NULL,
	[ReportingFrequencyUnit] [nvarchar](max) NULL,
	[ReportingRecepients] [nvarchar](250) NULL,
	[ReportingSLA] [int] NULL,
	[ReportingStartDate] [datetime] NULL,
	[ReportMonthFrequencyType] [nvarchar](max) NULL,
	[Responsible] [nvarchar](250) NULL,
	[SLAMissedPenalty] [int] NULL,
	[VendorMSALookup] [bigint] NOT NULL,
	[VendorMSANameLookup] [bigint] NOT NULL,
	[VendorReportingType] [nvarchar](250) NULL,
	[VendorSOWLookup] [bigint] NOT NULL,
	[VendorSOWNameLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorReportInstance]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VendorReportInstance](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AcceptedOn] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[DueDate] [datetime] NULL,
	[FolderUrl] [nvarchar](250) NULL,
	[NoDocumentNeeded] [bit] NULL,
	[ReceivedOn] [datetime] NULL,
	[ReportInstanceStatus] [nvarchar](max) NULL,
	[ReportReceived] [bit] NULL,
	[VendorMSALookup] [bigint] NOT NULL,
	[VendorMSANameLookup] [bigint] NOT NULL,
	[VendorReportLookup] [bigint] NOT NULL,
	[VendorSOWLookup] [bigint] NOT NULL,
	[VendorSOWNameLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorRisks]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VendorRisks](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AssignedTo] [nvarchar](250) NULL,
	[ContingencyPlan] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[DueDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[MitigationPlan] [nvarchar](max) NULL,
	[RiskProbability] [int] NULL,
	[Status] [nvarchar](max) NULL,
	[VendorMSALookup] [bigint] NOT NULL,
	[VendorMSANameLookup] [bigint] NOT NULL,
	[VendorRiskImpact] [nvarchar](max) NULL,
	[VendorSOWLookup] [bigint] NOT NULL,
	[VendorSOWNameLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Vendors]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vendors](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactName] [nvarchar](250) NULL,
	[IsDeleted] [bit] NULL,
	[Title] [nvarchar](250) NULL,
	[VendorAddress] [nvarchar](max) NULL,
	[VendorEmail] [nvarchar](250) NULL,
	[VendorLocation] [nvarchar](250) NULL,
	[VendorName] [nvarchar](250) NULL,
	[VendorPhone] [nvarchar](250) NULL,
	[WebsiteUrl] [nvarchar](250) NULL,
 CONSTRAINT [PK_Vendors] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VendorServiceDuration]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VendorServiceDuration](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ServiceDurationName] [nvarchar](250) NULL,
	[ServiceEndDate] [datetime] NULL,
	[ServiceStartDate] [datetime] NULL,
	[Title] [nvarchar](250) NULL,
	[VendorMSALookup] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VendorSLA]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VendorSLA](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](250) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[HigherIsBetter] [bit] NULL,
	[History] [nvarchar](max) NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[Initiator] [nvarchar](max) NULL,
	[IsPrivate] [bit] NULL,
	[MaxThreshold] [int] NULL,
	[MeasurementFrequency] [nvarchar](max) NULL,
	[MinThreshold] [int] NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldStartDate] [datetime] NULL,
	[OnHoldTillDate] [datetime] NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[Penalty] [int] NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[ResolutionType] [nvarchar](max) NULL,
	[ReSubmissionDate] [datetime] NULL,
	[Reward] [int] NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[SeverityLookup] [bigint] NOT NULL,
	[SLAName] [nvarchar](250) NULL,
	[SLANumber] [nvarchar](250) NULL,
	[SLATarget] [int] NULL,
	[SLAUnit] [nvarchar](max) NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
	[VendorMSALookup] [bigint] NOT NULL,
	[VendorMSANameLookup] [bigint] NOT NULL,
	[VendorResourceCategoryLookup] [bigint] NOT NULL,
	[VendorResourceSubCategoryLookup] [bigint] NOT NULL,
	[VendorSOWLookup] [bigint] NOT NULL,
	[VendorSOWNameLookup] [bigint] NOT NULL,
	[Weightage] [int] NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorSLAPerformance]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VendorSLAPerformance](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[HigherIsBetter] [bit] NULL,
	[ITMSReported] [int] NULL,
	[MaxThreshold] [int] NULL,
	[MinThreshold] [int] NULL,
	[Penalty] [int] NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[RequestTypeWorkflow] [nvarchar](250) NULL,
	[Reward] [int] NULL,
	[SLANumber] [nvarchar](250) NULL,
	[SLATarget] [int] NULL,
	[SLAUnit] [nvarchar](max) NULL,
	[SPReported] [int] NULL,
	[VendorMSALookup] [bigint] NOT NULL,
	[VendorMSANameLookup] [bigint] NOT NULL,
	[VendorPerformanceWaiver] [nvarchar](250) NULL,
	[VendorSLALookup] [bigint] NOT NULL,
	[VendorSLAMet] [nvarchar](max) NULL,
	[VendorSLANameLookup] [bigint] NOT NULL,
	[VendorSLAPerformanceNumber] [int] NULL,
	[VendorSLAReportingDate] [datetime] NULL,
	[VendorVPMLookup] [bigint] NOT NULL,
	[Weightage] [int] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorSOW]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VendorSOW](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AdditionalInformation] [nvarchar](max) NULL,
	[AgreementNumber] [nvarchar](250) NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[ContractSigningDate] [datetime] NULL,
	[ContractValue] [nvarchar](250) NULL,
	[CreationDate] [datetime] NULL,
	[Currency] [nvarchar](250) NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[EffectiveEndDate] [datetime] NULL,
	[EffectiveStartDate] [datetime] NULL,
	[FinancialManager] [nvarchar](max) NULL,
	[FolderUrl] [nvarchar](250) NULL,
	[FunctionalManager] [nvarchar](max) NULL,
	[History] [nvarchar](max) NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[InitialContractTerm] [int] NULL,
	[Initiator] [nvarchar](max) NULL,
	[IsPartialTermination] [bit] NULL,
	[IsPrivate] [bit] NULL,
	[KeyRefUniqueID] [nvarchar](250) NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldStartDate] [datetime] NULL,
	[OnHoldTillDate] [datetime] NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PerformanceManager] [nvarchar](max) NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[ResolutionType] [nvarchar](max) NULL,
	[ReSubmissionDate] [datetime] NULL,
	[ServiceDeliveryManager] [nvarchar](max) NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[SeverityLookup] [bigint] NOT NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
	[VendorMSALookup] [bigint] NOT NULL,
	[VendorMSANameLookup] [bigint] NOT NULL,
	[VMOApprover] [nvarchar](250) NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorSOWContImprovement]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VendorSOWContImprovement](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SOWContinuousImprovementPct] [int] NULL,
	[SOWContinuousImprovementPeriod] [int] NULL,
	[SOWContinuousImprovementPeriodUnit] [nvarchar](max) NULL,
	[VendorMSALookup] [bigint] NOT NULL,
	[VendorMSANameLookup] [bigint] NOT NULL,
	[VendorSOWLookup] [bigint] NOT NULL,
	[VendorSOWNameLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorSOWFees]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VendorSOWFees](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BudgetAmount] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[EndDate] [datetime] NULL,
	[FixedFees] [nvarchar](250) NULL,
	[SOWAdditionalUnitRate] [nvarchar](250) NULL,
	[SOWAnnualChangePct] [int] NULL,
	[SOWDeadBandPct] [int] NULL,
	[SOWFeeUnit] [nvarchar](max) NULL,
	[SOWFeeUnit2] [nvarchar](max) NULL,
	[SOWNoOfUnit] [int] NULL,
	[SOWReducedUnitRate] [nvarchar](250) NULL,
	[SOWUnitRate] [nvarchar](250) NULL,
	[StartDate] [datetime] NULL,
	[VendorPOLineItemLookup] [bigint] NOT NULL,
	[VendorPOLookup] [bigint] NOT NULL,
	[VendorResourceCategoryLookup] [bigint] NOT NULL,
	[VendorResourceSubCategoryLookup] [bigint] NOT NULL,
	[VendorSOWLookup] [bigint] NOT NULL,
	[VendorSOWNameLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorSOWInvoiceDetail]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VendorSOWInvoiceDetail](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BudgetAmount] [nvarchar](250) NULL,
	[FixedFees] [nvarchar](250) NULL,
	[InvoiceItemAmount] [nvarchar](250) NULL,
	[InvoiceNumber] [nvarchar](250) NULL,
	[PONumber] [nvarchar](250) NULL,
	[ResourceQuantity] [int] NULL,
	[SOWAdditionalUnitRate] [nvarchar](250) NULL,
	[SOWAnnualChangePct] [int] NULL,
	[SOWDeadBandPct] [int] NULL,
	[SOWFeeUnit] [nvarchar](max) NULL,
	[SOWFeeUnit2] [nvarchar](max) NULL,
	[SOWInvoiceDate] [datetime] NULL,
	[SOWInvoiceLookup] [bigint] NOT NULL,
	[SOWNoOfUnit] [int] NULL,
	[SOWReducedUnitRate] [nvarchar](250) NULL,
	[SOWUnitRate] [nvarchar](250) NULL,
	[VariableAmount] [int] NULL,
	[VendorMSALookup] [bigint] NOT NULL,
	[VendorMSANameLookup] [bigint] NOT NULL,
	[VendorPOLineItemLookup] [bigint] NOT NULL,
	[VendorPOLookup] [bigint] NOT NULL,
	[VendorResourceCategoryLookup] [bigint] NOT NULL,
	[VendorResourceSubCategoryLookup] [bigint] NOT NULL,
	[VendorSOWFeeLookup] [bigint] NOT NULL,
	[VendorSOWLookup] [bigint] NOT NULL,
	[VendorSOWNameLookup] [bigint] NOT NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorSOWInvoices]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VendorSOWInvoices](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AmountDisputed] [nvarchar](250) NULL,
	[AmountPaid] [nvarchar](250) NULL,
	[BudgetAmount] [nvarchar](250) NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[DatePaid] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[DueDate] [datetime] NULL,
	[FinancialManager] [nvarchar](max) NULL,
	[FolderUrl] [nvarchar](250) NULL,
	[FunctionalManager] [nvarchar](max) NULL,
	[History] [nvarchar](max) NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[Initiator] [nvarchar](max) NULL,
	[InvoiceNumber] [nvarchar](250) NULL,
	[IsPrivate] [bit] NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NextSLATime] [datetime] NULL,
	[NextSLAType] [nvarchar](250) NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldStartDate] [datetime] NULL,
	[OnHoldTillDate] [datetime] NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PerformanceManager] [nvarchar](max) NULL,
	[PONumber] [nvarchar](250) NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[ReceivedOn] [datetime] NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[ResolutionType] [nvarchar](max) NULL,
	[ReSubmissionDate] [datetime] NULL,
	[ServiceDeliveryManager] [nvarchar](max) NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[SeverityLookup] [bigint] NOT NULL,
	[SOWInvoiceActualAmount] [nvarchar](250) NULL,
	[SOWInvoiceAmount] [nvarchar](250) NULL,
	[SOWInvoiceDate] [datetime] NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
	[UserQuestionSummary] [nvarchar](max) NULL,
	[VendorMSALookup] [bigint] NOT NULL,
	[VendorMSANameLookup] [bigint] NOT NULL,
	[VendorSOWLookup] [bigint] NOT NULL,
	[VendorSOWNameLookup] [bigint] NOT NULL,
	[VMOApprover] [nvarchar](250) NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorType]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[VendorType](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[VTDescription] [nvarchar](500) NULL,
	[isdeleted] [bit] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorVPM]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VendorVPM](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BusinessManager] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[Completeness] [nvarchar](max) NULL,
	[ContractChange] [nvarchar](max) NULL,
	[ContractChangeComment] [nvarchar](max) NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[ExclusionException] [nvarchar](max) NULL,
	[ExclusionExceptionComment] [nvarchar](max) NULL,
	[FinancialManager] [nvarchar](max) NULL,
	[FolderUrl] [nvarchar](250) NULL,
	[FunctionalManager] [nvarchar](max) NULL,
	[History] [nvarchar](max) NULL,
	[ImpactLookup] [bigint] NOT NULL,
	[Initiator] [nvarchar](max) NULL,
	[IsPrivate] [bit] NULL,
	[MissedSLAsComment] [nvarchar](max) NULL,
	[MissedVPMSLAs] [int] NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NextSLATime] [datetime] NULL,
	[NextSLAType] [nvarchar](250) NULL,
	[NotDueSLAs] [int] NULL,
	[NotDueSLAsComment] [nvarchar](max) NULL,
	[OnHold] [int] NULL,
	[OnHoldReason] [nvarchar](max) NULL,
	[OnHoldStartDate] [datetime] NULL,
	[OnHoldTillDate] [datetime] NULL,
	[OtherSLAs] [int] NULL,
	[OtherSLAsComment] [nvarchar](max) NULL,
	[Owner] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PerformanceManager] [nvarchar](max) NULL,
	[PriorityLookup] [bigint] NOT NULL,
	[ReceivedOn] [datetime] NULL,
	[Requestor] [nvarchar](250) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[ResolutionType] [nvarchar](max) NULL,
	[ReSubmissionDate] [datetime] NULL,
	[RootCauseAnalysis] [nvarchar](max) NULL,
	[RootCauseAnalysisComment] [nvarchar](max) NULL,
	[RootCauseAnalysisNeeded] [bit] NULL,
	[ServiceDeliveryManager] [nvarchar](max) NULL,
	[ServiceLookUp] [bigint] NOT NULL,
	[SeverityLookup] [bigint] NOT NULL,
	[SLAComments] [nvarchar](max) NULL,
	[SLAsMetComment] [nvarchar](max) NULL,
	[SLAsMissed] [int] NULL,
	[SLCreditsDue] [nvarchar](max) NULL,
	[SLCreditsDueComment] [nvarchar](max) NULL,
	[SLDefaults] [nvarchar](max) NULL,
	[SLDefaultsComment] [nvarchar](max) NULL,
	[StageActionUsers] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[Timeliness] [nvarchar](max) NULL,
	[TotalHoldDuration] [int] NULL,
	[TotalSLAs] [int] NULL,
	[TotalSLAsMet] [int] NULL,
	[UserQuestionSummary] [nvarchar](max) NULL,
	[VendorAcceptsFailure] [bit] NULL,
	[VendorMSALookup] [bigint] NOT NULL,
	[VendorMSANameLookup] [bigint] NOT NULL,
	[VendorSLAReportingDate] [datetime] NULL,
	[VendorSLAReportingStart] [datetime] NULL,
	[VendorSOWLookup] [bigint] NOT NULL,
	[VendorSOWNameLookup] [bigint] NOT NULL,
	[VMOApprover] [nvarchar](250) NULL,
	[Waiver] [nvarchar](max) NULL,
	[WaiverComment] [nvarchar](max) NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WikiArticles]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WikiArticles](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AuthorizedToView] [nvarchar](250) NULL,
	[IsDeleted] [bit] NULL,
	[ModuleName] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NOT NULL,
	[TicketId] [nvarchar](250) NULL,
	[WikiAverageScore] [int] NULL,
	[WikiDescription] [nvarchar](250) NULL,
	[WikiDiscussionCount] [int] NULL,
	[WikiDislikedBy] [nvarchar](250) NULL,
	[WikiDislikesCount] [int] NULL,
	[WikiFavorites] [bit] NULL,
	[WikiHistory] [nvarchar](max) NULL,
	[WikiLikedBy] [nvarchar](250) NULL,
	[WikiLikesCount] [int] NULL,
	[WikiLinksCount] [int] NULL,
	[WikiServiceRequestCount] [int] NULL,
	[WikiViews] [int] NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WikiDiscussion]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WikiDiscussion](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[_UIVersionString] [nvarchar](250) NULL,
	[Body] [nvarchar](max) NULL,
	[BodyAndMore] [nvarchar](250) NULL,
	[BodyWasExpanded] [nvarchar](250) NULL,
	[CorrectBodyToShow] [nvarchar](250) NULL,
	[DiscussionLastUpdated] [datetime] NULL,
	[DiscussionTitle] [nvarchar](250) NULL,
	[EmailReferences] [nvarchar](max) NULL,
	[EmailSender] [nvarchar](max) NULL,
	[FileLeafRef] [nvarchar](250) NULL,
	[FolderChildCount] [nvarchar](250) NULL,
	[FullBody] [nvarchar](250) NULL,
	[Indentation] [nvarchar](250) NULL,
	[IndentLevel] [nvarchar](250) NULL,
	[IsRootPost] [nvarchar](250) NULL,
	[ItemChildCount] [nvarchar](250) NULL,
	[LessLink] [nvarchar](250) NULL,
	[LimitedBody] [nvarchar](250) NULL,
	[LinkDiscussionTitle] [nvarchar](250) NULL,
	[LinkDiscussionTitleNoMenu] [nvarchar](250) NULL,
	[LinkTitle] [nvarchar](250) NULL,
	[LinkTitleNoMenu] [nvarchar](250) NULL,
	[MessageId] [nvarchar](250) NULL,
	[MoreLink] [nvarchar](250) NULL,
	[MyEditor] [nvarchar](max) NULL,
	[ParentFolderId] [nvarchar](250) NULL,
	[PersonImage] [nvarchar](250) NULL,
	[PersonViewMinimal] [nvarchar](250) NULL,
	[QuotedTextWasExpanded] [nvarchar](250) NULL,
	[RelevantMessages] [nvarchar](max) NULL,
	[ReplyNoGif] [nvarchar](250) NULL,
	[ShortestThreadIndex] [nvarchar](max) NULL,
	[ShortestThreadIndexId] [nvarchar](250) NULL,
	[StatusBar] [nvarchar](250) NULL,
	[ThreadIndex] [nvarchar](250) NULL,
	[Threading] [nvarchar](250) NULL,
	[ThreadingControls] [nvarchar](250) NULL,
	[ThreadTopic] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[ToggleQuotedText] [nvarchar](250) NULL,
	[TrimmedBody] [nvarchar](max) NULL,
	[WikiID] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WikiLinks]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WikiLinks](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Comments] [nvarchar](max) NULL,
	[DocIcon] [nvarchar](250) NULL,
	[Edit] [nvarchar](250) NULL,
	[LinkTitle] [nvarchar](250) NULL,
	[LinkTitleNoMenu] [nvarchar](250) NULL,
	[TicketId] [nvarchar](250) NULL,
	[URL] [nvarchar](250) NULL,
	[URLNoMenu] [nvarchar](250) NULL,
	[URLwMenu] [nvarchar](250) NULL,
	[URLwMenu2] [nvarchar](250) NULL,
	[WikiID] [nvarchar](250) NULL,
	[WikiLinkTitle] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WikiReview]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WikiReview](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[TicketId] [nvarchar](250) NULL,
	[WikiRatingDetails] [nvarchar](250) NULL,
	[WikiScore] [int] NULL,
	[WikiUserType] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WorkflowSLASummary]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WorkflowSLASummary](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualTime] [int] NULL,
	[Closed] [bit] NULL,
	[DueDate] [datetime] NULL,
	[EndStageName] [nvarchar](250) NULL,
	[EndStageStep] [int] NULL,
	[ModuleName] [nvarchar](250) NULL,
	[RuleNameLookup] [bigint] NOT NULL,
	[SLACategory] [nvarchar](max) NULL,
	[SLARuleName] [nvarchar](250) NULL,
	[StageEndDate] [datetime] NULL,
	[StageStartDate] [datetime] NULL,
	[StartStageName] [nvarchar](250) NULL,
	[StartStageStep] [int] NULL,
	[TargetTime] [int] NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[vw_PRS]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE View [dbo].[vw_PRS] as
select prs.TicketId, app.Description as Application, a.AssetName, c.Description as Company, d.DepartmentDescription as Department,
      cd.Description as CompanyDivision, fa.FunctionalAreaDescription as FunctionalArea, cimpact.Title as Impact,
	  loc.LocationDescription as Location, cpriority.Title as Priority, crequestpri.Title as RequestPriority,
	  cservices.Title as Services, cseverity.Severity as Severity, prs.ID, prs.Status, prs.Owner, prs.ActualHours, 
       prs.BusinessManager, prs.CloseDate, prs.CreationDate, prs.PRP, prs.PRPGroup, prs.Initiator, prs.StageStep, prs.TargetStartDate
	    from PRS prs left join Applications app on prs.APPTitleLookup = app.ID
                      left join Assets a on prs.AssetLookup = a.ID
					  left join Company c on prs.CompanyTitleLookup = c.ID
					  left join Department d on prs.DepartmentLookup = d.ID
					  left join CompanyDivisions cd on prs.DivisionLookup = cd.ID
					  left join FunctionalAreas fa on prs.FunctionalAreaLookup = fa.ID
					  left join Config_Module_Impact cimpact on prs.ImpactLookup = cimpact.ID
					  left join Location loc on prs.LocationLookup = loc.ID
					  left join Config_Module_Priority cpriority on prs.PriorityLookup = cpriority.ID
					  left join Config_Module_RequestPriority crequestpri on prs.RequestTypeLookup = crequestpri.ID
					  left join Config_Services cservices on prs.ServiceLookUp = cservices.ID
					  left join Config_Module_Severity cseverity on prs.SeverityLookup = cseverity.ID





GO
/****** Object:  View [dbo].[vw_TSR]    Script Date: 22-02-2017 17:01:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE View [dbo].[vw_TSR]
as
select t.TicketId, a.AssetName, c.Title as CompanyTitle, d.DepartmentDescription , cd.Title as CompanyDivision, fa.Title as FunctionalArea,
       cimpact.Title as Impact, l.LocationDescription as Location,
       cseverity.Title as Severity, t.ID, t.Status, t.Owner, t.ActualHours, 
       t.BusinessManager, t.CloseDate, t.CreationDate, t.PRP, t.PRPGroup, t.Initiator, t.StageStep, t.TargetStartDate
	    from TSR t left join Assets a on t.AssetLookup = a.ID
                             left join Company c on t.CompanyTitleLookup = c.ID
							left join Department d on t.DepartmentLookup = d.ID
							left join CompanyDivisions cd on t.DivisionLookup = cd.ID
							left join FunctionalAreas fa on t.FunctionalAreaLookup = fa.ID
							left join Config_Module_Impact cimpact on t.ImpactLookup = cimpact.ID
							left join Location l on t.LocationLookup = l.ID
							left join Config_Module_Severity cseverity on t.SeverityLookup = cseverity.ID





GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [RoleNameIndex]    Script Date: 22-02-2017 17:01:12 ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 22-02-2017 17:01:12 ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 22-02-2017 17:01:12 ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_RoleId]    Script Date: 22-02-2017 17:01:12 ******/
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 22-02-2017 17:01:12 ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UserNameIndex]    Script Date: 22-02-2017 17:01:12 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[VendorType] ADD  DEFAULT ((1)) FOR [isdeleted]
GO
ALTER TABLE [dbo].[ACR]  WITH NOCHECK ADD FOREIGN KEY([ACRTypeTitleLookup])
REFERENCES [dbo].[ACRTypes] ([ID])
GO
ALTER TABLE [dbo].[ACR]  WITH NOCHECK ADD FOREIGN KEY([APPTitleLookup])
REFERENCES [dbo].[Applications] ([ID])
GO
ALTER TABLE [dbo].[ACR]  WITH NOCHECK ADD FOREIGN KEY([CompanyTitleLookup])
REFERENCES [dbo].[Company] ([ID])
GO
ALTER TABLE [dbo].[ACR]  WITH NOCHECK ADD FOREIGN KEY([DepartmentLookup])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[ACR]  WITH NOCHECK ADD FOREIGN KEY([DivisionLookup])
REFERENCES [dbo].[CompanyDivisions] ([ID])
GO
ALTER TABLE [dbo].[ACR]  WITH NOCHECK ADD FOREIGN KEY([FunctionalAreaLookup])
REFERENCES [dbo].[FunctionalAreas] ([ID])
GO
ALTER TABLE [dbo].[ACR]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[ACR]  WITH NOCHECK ADD FOREIGN KEY([LocationLookup])
REFERENCES [dbo].[Location] ([ID])
GO
ALTER TABLE [dbo].[ACR]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[ACR]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[ACR]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[ACR]  WITH NOCHECK ADD FOREIGN KEY([SeverityLookup])
REFERENCES [dbo].[Config_Module_Severity] ([ID])
GO
ALTER TABLE [dbo].[ApplicationModules]  WITH NOCHECK ADD FOREIGN KEY([APPTitleLookup])
REFERENCES [dbo].[Applications] ([ID])
GO
ALTER TABLE [dbo].[ApplicationPassword]  WITH NOCHECK ADD FOREIGN KEY([APPTitleLookup])
REFERENCES [dbo].[Applications] ([ID])
GO
ALTER TABLE [dbo].[ApplicationPassword]  WITH NOCHECK ADD FOREIGN KEY([APPTitleLookup])
REFERENCES [dbo].[Applications] ([ID])
GO
ALTER TABLE [dbo].[ApplicationRole]  WITH NOCHECK ADD FOREIGN KEY([ApplicationRoleModuleLookup])
REFERENCES [dbo].[ApplicationModules] ([ID])
GO
ALTER TABLE [dbo].[ApplicationRole]  WITH NOCHECK ADD FOREIGN KEY([ApplicationRoleModuleLookup])
REFERENCES [dbo].[ApplicationModules] ([ID])
GO
ALTER TABLE [dbo].[ApplicationRole]  WITH NOCHECK ADD FOREIGN KEY([APPTitleLookup])
REFERENCES [dbo].[Applications] ([ID])
GO
ALTER TABLE [dbo].[Applications]  WITH NOCHECK ADD FOREIGN KEY([DepartmentLookup])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[Applications]  WITH NOCHECK ADD FOREIGN KEY([FunctionalAreaLookup])
REFERENCES [dbo].[FunctionalAreas] ([ID])
GO
ALTER TABLE [dbo].[ApplicationServers]  WITH NOCHECK ADD FOREIGN KEY([APPTitleLookup])
REFERENCES [dbo].[Applications] ([ID])
GO
ALTER TABLE [dbo].[ApplicationServers]  WITH NOCHECK ADD FOREIGN KEY([AssetsTitleLookup])
REFERENCES [dbo].[Assets] ([ID])
GO
ALTER TABLE [dbo].[ApplicationServers]  WITH NOCHECK ADD FOREIGN KEY([EnvironmentLookup])
REFERENCES [dbo].[Environment] ([ID])
GO
ALTER TABLE [dbo].[ApplModuleRoleRelationship]  WITH NOCHECK ADD FOREIGN KEY([ApplicationModulesLookup])
REFERENCES [dbo].[ApplicationModules] ([ID])
GO
ALTER TABLE [dbo].[ApplModuleRoleRelationship]  WITH NOCHECK ADD FOREIGN KEY([ApplicationRoleLookup])
REFERENCES [dbo].[ApplicationRole] ([ID])
GO
ALTER TABLE [dbo].[ApplModuleRoleRelationship]  WITH NOCHECK ADD FOREIGN KEY([APPTitleLookup])
REFERENCES [dbo].[Applications] ([ID])
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AssetIncidentRelations]  WITH NOCHECK ADD FOREIGN KEY([AssetTagNumLookup])
REFERENCES [dbo].[Assets] ([ID])
GO
ALTER TABLE [dbo].[AssetReferences]  WITH NOCHECK ADD FOREIGN KEY([AssetLookup])
REFERENCES [dbo].[Assets] ([ID])
GO
ALTER TABLE [dbo].[Assets]  WITH NOCHECK ADD FOREIGN KEY([AssetModelLookup])
REFERENCES [dbo].[AssetModels] ([ID])
GO
ALTER TABLE [dbo].[Assets]  WITH NOCHECK ADD FOREIGN KEY([AssetsStatusLookup])
REFERENCES [dbo].[AssetsStatus] ([ID])
GO
ALTER TABLE [dbo].[Assets]  WITH NOCHECK ADD FOREIGN KEY([DepartmentLookup])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[Assets]  WITH NOCHECK ADD FOREIGN KEY([ImageOptionLookup])
REFERENCES [dbo].[ImageSoftware] ([ID])
GO
ALTER TABLE [dbo].[Assets]  WITH NOCHECK ADD FOREIGN KEY([LocationLookup])
REFERENCES [dbo].[Location] ([ID])
GO
ALTER TABLE [dbo].[Assets]  WITH NOCHECK ADD FOREIGN KEY([ReplacementAsset_SNLookup])
REFERENCES [dbo].[Assets] ([ID])
GO
ALTER TABLE [dbo].[Assets]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[Assets]  WITH NOCHECK ADD FOREIGN KEY([SoftwareLookup])
REFERENCES [dbo].[ImageSoftware] ([ID])
GO
ALTER TABLE [dbo].[AvailableManagedServices]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[Bid]  WITH NOCHECK ADD FOREIGN KEY([ContactLookup])
REFERENCES [dbo].[Contacts] ([ID])
GO
ALTER TABLE [dbo].[Bid]  WITH NOCHECK ADD FOREIGN KEY([OrganizationLookup])
REFERENCES [dbo].[Organization] ([ID])
GO
ALTER TABLE [dbo].[BTS]  WITH NOCHECK ADD FOREIGN KEY([CompanyTitleLookup])
REFERENCES [dbo].[Company] ([ID])
GO
ALTER TABLE [dbo].[BTS]  WITH NOCHECK ADD FOREIGN KEY([DepartmentLookup])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[BTS]  WITH NOCHECK ADD FOREIGN KEY([DivisionLookup])
REFERENCES [dbo].[CompanyDivisions] ([ID])
GO
ALTER TABLE [dbo].[BTS]  WITH NOCHECK ADD FOREIGN KEY([FunctionalAreaLookup])
REFERENCES [dbo].[FunctionalAreas] ([ID])
GO
ALTER TABLE [dbo].[BTS]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[BTS]  WITH NOCHECK ADD FOREIGN KEY([LocationLookup])
REFERENCES [dbo].[Location] ([ID])
GO
ALTER TABLE [dbo].[BTS]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[BTS]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[BTS]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[BTS]  WITH NOCHECK ADD FOREIGN KEY([SeverityLookup])
REFERENCES [dbo].[Config_Module_Severity] ([ID])
GO
ALTER TABLE [dbo].[CompanyDivisions]  WITH NOCHECK ADD FOREIGN KEY([CompanyIdLookup])
REFERENCES [dbo].[Company] ([ID])
GO
ALTER TABLE [dbo].[Config_ClientAdminConfigurationLists]  WITH NOCHECK ADD FOREIGN KEY([ClientAdminCategoryLookup])
REFERENCES [dbo].[Config_ClientAdminCategory] ([ID])
GO
ALTER TABLE [dbo].[Config_ClientAdminConfigurationLists]  WITH NOCHECK ADD FOREIGN KEY([ClientAdminCategoryLookup])
REFERENCES [dbo].[Config_ClientAdminCategory] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Module_EscalationRule]  WITH NOCHECK ADD FOREIGN KEY([SLARuleIdLookup])
REFERENCES [dbo].[Config_Module_SLARule] ([ID])
GO
ALTER TABLE [dbo].[Config_Module_FormLayout]  WITH NOCHECK ADD FOREIGN KEY([ModuleNameLookup])
REFERENCES [dbo].[Config_Modules] ([ModuleName])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Module_Impact]  WITH NOCHECK ADD FOREIGN KEY([ModuleNameLookup])
REFERENCES [dbo].[Config_Modules] ([ModuleName])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Module_ModuleColumns]  WITH NOCHECK ADD FOREIGN KEY([ModuleNameLookup])
REFERENCES [dbo].[Config_Modules] ([ModuleName])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Module_ModuleFormTab]  WITH NOCHECK ADD FOREIGN KEY([ModuleNameLookup])
REFERENCES [dbo].[Config_Modules] ([ModuleName])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Module_ModuleStages]  WITH NOCHECK ADD FOREIGN KEY([ModuleNameLookup])
REFERENCES [dbo].[Config_Modules] ([ModuleName])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Module_ModuleStages]  WITH NOCHECK ADD FOREIGN KEY([StageTypeLookup])
REFERENCES [dbo].[Config_Module_StageType] ([ID])
GO
ALTER TABLE [dbo].[Config_Module_ModuleUserTypes]  WITH NOCHECK ADD FOREIGN KEY([ModuleNameLookup])
REFERENCES [dbo].[Config_Modules] ([ModuleName])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Module_Priority]  WITH NOCHECK ADD FOREIGN KEY([ModuleNameLookup])
REFERENCES [dbo].[Config_Modules] ([ModuleName])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Module_RequestPriority]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[Config_Module_RequestPriority]  WITH NOCHECK ADD FOREIGN KEY([ModuleNameLookup])
REFERENCES [dbo].[Config_Modules] ([ModuleName])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Module_RequestPriority]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[Config_Module_RequestPriority]  WITH NOCHECK ADD FOREIGN KEY([SeverityLookup])
REFERENCES [dbo].[Config_Module_Severity] ([ID])
GO
ALTER TABLE [dbo].[Config_Module_RequestRoleWriteAccess]  WITH NOCHECK ADD FOREIGN KEY([ModuleNameLookup])
REFERENCES [dbo].[Config_Modules] ([ModuleName])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Module_RequestType]  WITH NOCHECK ADD FOREIGN KEY([ApplicationModulesLookup])
REFERENCES [dbo].[ApplicationModules] ([ID])
GO
ALTER TABLE [dbo].[Config_Module_RequestType]  WITH NOCHECK ADD FOREIGN KEY([APPTitleLookup])
REFERENCES [dbo].[Applications] ([ID])
GO
ALTER TABLE [dbo].[Config_Module_RequestType]  WITH NOCHECK ADD FOREIGN KEY([FunctionalAreaLookup])
REFERENCES [dbo].[FunctionalAreas] ([ID])
GO
ALTER TABLE [dbo].[Config_Module_RequestType]  WITH NOCHECK ADD FOREIGN KEY([ModuleNameLookup])
REFERENCES [dbo].[Config_Modules] ([ModuleName])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Module_RequestType]  WITH NOCHECK ADD FOREIGN KEY([TaskTemplateLookup])
REFERENCES [dbo].[TaskTemplates] ([ID])
GO
ALTER TABLE [dbo].[Config_Module_Severity]  WITH NOCHECK ADD FOREIGN KEY([ModuleNameLookup])
REFERENCES [dbo].[Config_Modules] ([ModuleName])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Module_SLARule]  WITH NOCHECK ADD FOREIGN KEY([ModuleNameLookup])
REFERENCES [dbo].[Config_Modules] ([ModuleName])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Module_SLARule]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[Config_Module_StatusMapping]  WITH NOCHECK ADD FOREIGN KEY([GenericStatusLookup])
REFERENCES [dbo].[GenericStatus] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Module_StatusMapping]  WITH NOCHECK ADD FOREIGN KEY([ModuleNameLookup])
REFERENCES [dbo].[Config_Modules] ([ModuleName])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Module_StatusMapping]  WITH NOCHECK ADD FOREIGN KEY([StageTitleLookup])
REFERENCES [dbo].[Config_Module_ModuleStages] ([ID])
GO
ALTER TABLE [dbo].[Config_Module_TaskEmails]  WITH NOCHECK ADD FOREIGN KEY([ModuleNameLookup])
REFERENCES [dbo].[Config_Modules] ([ModuleName])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_ModuleMonitorOptions]  WITH NOCHECK ADD FOREIGN KEY([ModuleMonitorNameLookup])
REFERENCES [dbo].[Config_ModuleMonitors] ([MonitorName])
GO
ALTER TABLE [dbo].[Config_ProjectLifeCycleStages]  WITH CHECK ADD FOREIGN KEY([ProjectLifeCycleLookup])
REFERENCES [dbo].[Config_ProjectLifeCycles] ([ID])
GO
ALTER TABLE [dbo].[Config_Service_ServiceColumns]  WITH NOCHECK ADD FOREIGN KEY([ModuleIdLookup])
REFERENCES [dbo].[Config_Modules] ([ModuleName])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Service_ServiceColumns]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[Config_Service_ServiceColumns]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[Config_Service_ServiceDefaultValues]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookup])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[Config_Service_ServiceDefaultValues]  WITH NOCHECK ADD FOREIGN KEY([ServiceTaskLookup])
REFERENCES [dbo].[Config_Service_ServiceRelationships] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Service_ServiceDefaultValues]  WITH NOCHECK ADD FOREIGN KEY([ServiceQuestionTitleLookup])
REFERENCES [dbo].[Config_Service_ServiceQuestions] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Service_ServiceDefaultValues]  WITH NOCHECK ADD FOREIGN KEY([ServiceQuestionTitleLookup])
REFERENCES [dbo].[Config_Service_ServiceQuestions] ([ID])
GO
ALTER TABLE [dbo].[Config_Service_ServiceDefaultValues]  WITH NOCHECK ADD FOREIGN KEY([ServiceTaskLookup])
REFERENCES [dbo].[Config_Service_ServiceRelationships] ([ID])
GO
ALTER TABLE [dbo].[Config_Service_ServiceDefaultValues]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookup])
REFERENCES [dbo].[Config_Services] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Config_Service_ServiceDefaultValues]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookup])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[Config_Service_ServiceQuestions]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[Config_Service_ServiceQuestions]  WITH NOCHECK ADD FOREIGN KEY([ServiceSectionsTitleLookup])
REFERENCES [dbo].[Config_Service_ServiceSections] ([ID])
GO
ALTER TABLE [dbo].[Config_Service_ServiceQuestions]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[Config_Service_ServiceRelationships]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[Config_Service_ServiceRelationships]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[Config_Service_ServiceRelationships]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[Config_Service_ServiceSections]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[Config_Service_ServiceSections]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[Config_Service_ServiceSections]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[Config_Services]  WITH NOCHECK ADD FOREIGN KEY([ServiceCategoryNameLookup])
REFERENCES [dbo].[Config_Service_ServiceCategories] ([ID])
GO
ALTER TABLE [dbo].[Contacts]  WITH NOCHECK ADD FOREIGN KEY([OrganizationLookup])
REFERENCES [dbo].[Organization] ([ID])
GO
ALTER TABLE [dbo].[Contracts]  WITH NOCHECK ADD FOREIGN KEY([FunctionalAreaLookup])
REFERENCES [dbo].[FunctionalAreas] ([ID])
GO
ALTER TABLE [dbo].[Contracts]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[Contracts]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[Contracts]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[Customers]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[Customers]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[DashboardSummary]  WITH NOCHECK ADD FOREIGN KEY([FunctionalAreaLookup])
REFERENCES [dbo].[FunctionalAreas] ([ID])
GO
ALTER TABLE [dbo].[DashboardSummary]  WITH NOCHECK ADD FOREIGN KEY([GenericStatusLookup])
REFERENCES [dbo].[GenericStatus] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[DashboardSummary]  WITH NOCHECK ADD FOREIGN KEY([LocationLookup])
REFERENCES [dbo].[Location] ([ID])
GO
ALTER TABLE [dbo].[DashboardSummary]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[DashboardSummary]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[Department]  WITH NOCHECK ADD FOREIGN KEY([CompanyIdLookup])
REFERENCES [dbo].[Company] ([ID])
GO
ALTER TABLE [dbo].[Department]  WITH NOCHECK ADD FOREIGN KEY([DivisionIdLookup])
REFERENCES [dbo].[CompanyDivisions] ([ID])
GO
ALTER TABLE [dbo].[DMDocumentInfoList]  WITH NOCHECK ADD FOREIGN KEY([DMVendorLookup])
REFERENCES [dbo].[Vendors] ([ID])
GO
ALTER TABLE [dbo].[DRQ]  WITH NOCHECK ADD FOREIGN KEY([APPTitleLookup])
REFERENCES [dbo].[Applications] ([ID])
GO
ALTER TABLE [dbo].[DRQ]  WITH NOCHECK ADD FOREIGN KEY([CompanyTitleLookup])
REFERENCES [dbo].[Company] ([ID])
GO
ALTER TABLE [dbo].[DRQ]  WITH NOCHECK ADD FOREIGN KEY([DepartmentLookup])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[DRQ]  WITH NOCHECK ADD FOREIGN KEY([DivisionLookup])
REFERENCES [dbo].[CompanyDivisions] ([ID])
GO
ALTER TABLE [dbo].[DRQ]  WITH NOCHECK ADD FOREIGN KEY([DRQRapidTypeLookup])
REFERENCES [dbo].[DRQRapidTypes] ([ID])
GO
ALTER TABLE [dbo].[DRQ]  WITH NOCHECK ADD FOREIGN KEY([DRQSystemsLookup])
REFERENCES [dbo].[DRQSystemAreas] ([ID])
GO
ALTER TABLE [dbo].[DRQ]  WITH NOCHECK ADD FOREIGN KEY([FunctionalAreaLookup])
REFERENCES [dbo].[FunctionalAreas] ([ID])
GO
ALTER TABLE [dbo].[DRQ]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[DRQ]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[DRQ]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[DRQ]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[DRQ]  WITH NOCHECK ADD FOREIGN KEY([SeverityLookup])
REFERENCES [dbo].[Config_Module_Severity] ([ID])
GO
ALTER TABLE [dbo].[FunctionalAreas]  WITH NOCHECK ADD FOREIGN KEY([DepartmentLookup])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[GovernanceLinkItems]  WITH NOCHECK ADD FOREIGN KEY([GovernanceLinkCategoryLookup])
REFERENCES [dbo].[GovernanceLinkCategory] ([ID])
GO
ALTER TABLE [dbo].[GovernanceLinkItems]  WITH NOCHECK ADD FOREIGN KEY([GovernanceLinkCategoryLookup])
REFERENCES [dbo].[GovernanceLinkCategory] ([ID])
GO
ALTER TABLE [dbo].[ImageSoftwareMap]  WITH NOCHECK ADD FOREIGN KEY([APPTitleLookup])
REFERENCES [dbo].[Applications] ([ID])
GO
ALTER TABLE [dbo].[ImageSoftwareMap]  WITH NOCHECK ADD FOREIGN KEY([ImageOptionLookup])
REFERENCES [dbo].[ImageSoftware] ([ID])
GO
ALTER TABLE [dbo].[INC]  WITH NOCHECK ADD FOREIGN KEY([AssetLookup])
REFERENCES [dbo].[Assets] ([ID])
GO
ALTER TABLE [dbo].[INC]  WITH NOCHECK ADD FOREIGN KEY([CompanyTitleLookup])
REFERENCES [dbo].[Company] ([ID])
GO
ALTER TABLE [dbo].[INC]  WITH NOCHECK ADD FOREIGN KEY([DepartmentLookup])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[INC]  WITH NOCHECK ADD FOREIGN KEY([DivisionLookup])
REFERENCES [dbo].[CompanyDivisions] ([ID])
GO
ALTER TABLE [dbo].[INC]  WITH NOCHECK ADD FOREIGN KEY([FunctionalAreaLookup])
REFERENCES [dbo].[FunctionalAreas] ([ID])
GO
ALTER TABLE [dbo].[INC]  WITH NOCHECK ADD FOREIGN KEY([LocationLookup])
REFERENCES [dbo].[Location] ([ID])
GO
ALTER TABLE [dbo].[INC]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[INC]  WITH NOCHECK ADD FOREIGN KEY([PRSLookup])
REFERENCES [dbo].[PRS] ([ID])
GO
ALTER TABLE [dbo].[INC]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[INC]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[INC]  WITH NOCHECK ADD FOREIGN KEY([SeverityLookup])
REFERENCES [dbo].[Config_Module_Severity] ([ID])
GO
ALTER TABLE [dbo].[InvDistribution]  WITH NOCHECK ADD FOREIGN KEY([InvestmentIDLookup])
REFERENCES [dbo].[Investments] ([ID])
GO
ALTER TABLE [dbo].[InvDistribution]  WITH NOCHECK ADD FOREIGN KEY([InvestorIDLookup])
REFERENCES [dbo].[Investments] ([ID])
GO
ALTER TABLE [dbo].[Investments]  WITH NOCHECK ADD FOREIGN KEY([InvestorShortNameLookup])
REFERENCES [dbo].[Investors] ([ID])
GO
ALTER TABLE [dbo].[Investors]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[Investors]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[ITGActual]  WITH NOCHECK ADD FOREIGN KEY([ITGBudgetLookup])
REFERENCES [dbo].[ITGBudget] ([ID])
GO
ALTER TABLE [dbo].[ITGActual]  WITH NOCHECK ADD FOREIGN KEY([VendorLookup])
REFERENCES [dbo].[Vendors] ([ID])
GO
ALTER TABLE [dbo].[ITGBudget]  WITH NOCHECK ADD FOREIGN KEY([DepartmentLookup])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[ITGovernance]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[ITGovernance]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[ITGovernance]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[ITGovernance]  WITH NOCHECK ADD FOREIGN KEY([SeverityLookup])
REFERENCES [dbo].[Config_Module_Severity] ([ID])
GO
ALTER TABLE [dbo].[LinkCategory]  WITH NOCHECK ADD FOREIGN KEY([LinkViewLookup])
REFERENCES [dbo].[LinkView] ([ID])
GO
ALTER TABLE [dbo].[LinkItems]  WITH NOCHECK ADD FOREIGN KEY([LinkCategoryLookup])
REFERENCES [dbo].[LinkCategory] ([ID])
GO
ALTER TABLE [dbo].[NPRBudget]  WITH NOCHECK ADD FOREIGN KEY([NPRIdLookup])
REFERENCES [dbo].[NPRRequest] ([ID])
GO
ALTER TABLE [dbo].[NPRMonthlyBudget]  WITH NOCHECK ADD FOREIGN KEY([NPRIdLookup])
REFERENCES [dbo].[NPRRequest] ([ID])
GO
ALTER TABLE [dbo].[NPRRequest]  WITH NOCHECK ADD FOREIGN KEY([APPTitleLookup])
REFERENCES [dbo].[Applications] ([ID])
GO
ALTER TABLE [dbo].[NPRRequest]  WITH NOCHECK ADD FOREIGN KEY([FunctionalAreaLookup])
REFERENCES [dbo].[FunctionalAreas] ([ID])
GO
ALTER TABLE [dbo].[NPRRequest]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[NPRRequest]  WITH NOCHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[NPRRequest]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[NPRRequest]  WITH NOCHECK ADD FOREIGN KEY([ProjectClassLookup])
REFERENCES [dbo].[Config_ProjectClass] ([ID])
GO
ALTER TABLE [dbo].[NPRRequest]  WITH NOCHECK ADD FOREIGN KEY([ProjectInitiativeLookup])
REFERENCES [dbo].[Config_ProjectInitiative] ([ID])
GO
ALTER TABLE [dbo].[NPRRequest]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[NPRRequest]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[NPRRequest]  WITH NOCHECK ADD FOREIGN KEY([SeverityLookup])
REFERENCES [dbo].[Config_Module_Severity] ([ID])
GO
ALTER TABLE [dbo].[NPRRequest]  WITH NOCHECK ADD FOREIGN KEY([VendorLookup])
REFERENCES [dbo].[Vendors] ([ID])
GO
ALTER TABLE [dbo].[NPRResources]  WITH NOCHECK ADD FOREIGN KEY([NPRIdLookup])
REFERENCES [dbo].[NPRRequest] ([ID])
GO
ALTER TABLE [dbo].[NPRResources]  WITH NOCHECK ADD FOREIGN KEY([UserSkillLookup])
REFERENCES [dbo].[UserSkills] ([ID])
GO
ALTER TABLE [dbo].[NPRTasks]  WITH NOCHECK ADD FOREIGN KEY([NPRIdLookup])
REFERENCES [dbo].[NPRRequest] ([ID])
GO
ALTER TABLE [dbo].[Opportunity]  WITH NOCHECK ADD FOREIGN KEY([CompanyTitleLookup])
REFERENCES [dbo].[Company] ([ID])
GO
ALTER TABLE [dbo].[Opportunity]  WITH NOCHECK ADD FOREIGN KEY([ContactLookup])
REFERENCES [dbo].[Contacts] ([ID])
GO
ALTER TABLE [dbo].[Opportunity]  WITH NOCHECK ADD FOREIGN KEY([DepartmentLookup])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[Opportunity]  WITH NOCHECK ADD FOREIGN KEY([DivisionLookup])
REFERENCES [dbo].[CompanyDivisions] ([ID])
GO
ALTER TABLE [dbo].[Opportunity]  WITH NOCHECK ADD FOREIGN KEY([FunctionalAreaLookup])
REFERENCES [dbo].[FunctionalAreas] ([ID])
GO
ALTER TABLE [dbo].[Opportunity]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[Opportunity]  WITH NOCHECK ADD FOREIGN KEY([OrganizationLookup])
REFERENCES [dbo].[Organization] ([ID])
GO
ALTER TABLE [dbo].[Opportunity]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[Opportunity]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[Opportunity]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[PLCRequest]  WITH NOCHECK ADD FOREIGN KEY([CRMLookup])
REFERENCES [dbo].[Customers] ([ID])
GO
ALTER TABLE [dbo].[PLCRequest]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[PLCRequest]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[PMMBaselineDetail]  WITH NOCHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[PMMBudgetActuals]  WITH NOCHECK ADD FOREIGN KEY([PMMBudgetLookup])
REFERENCES [dbo].[PMMBudget] ([ID])
GO
ALTER TABLE [dbo].[PMMBudgetActuals]  WITH NOCHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[PMMBudgetHistory]  WITH NOCHECK ADD FOREIGN KEY([PMMBudgetLookup])
REFERENCES [dbo].[PMMBudget] ([ID])
GO
ALTER TABLE [dbo].[PMMBudgetHistory]  WITH NOCHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[PMMComments]  WITH NOCHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[PMMCommentsHistory]  WITH NOCHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[PMMEvents]  WITH NOCHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[PMMIssues]  WITH NOCHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[PMMIssuesHistory]  WITH NOCHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[PMMMonthlyBudget]  WITH NOCHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[PMMMonthlyBudgetHistory]  WITH NOCHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[PMMProjects]  WITH NOCHECK ADD FOREIGN KEY([APPTitleLookup])
REFERENCES [dbo].[Applications] ([ID])
GO
ALTER TABLE [dbo].[PMMProjects]  WITH NOCHECK ADD FOREIGN KEY([FunctionalAreaLookup])
REFERENCES [dbo].[FunctionalAreas] ([ID])
GO
ALTER TABLE [dbo].[PMMProjects]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[PMMProjects]  WITH NOCHECK ADD FOREIGN KEY([NPRIdLookup])
REFERENCES [dbo].[NPRRequest] ([ID])
GO
ALTER TABLE [dbo].[PMMProjects]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[PMMProjects]  WITH NOCHECK ADD FOREIGN KEY([ProjectClassLookup])
REFERENCES [dbo].[Config_ProjectClass] ([ID])
GO
ALTER TABLE [dbo].[PMMProjects]  WITH NOCHECK ADD FOREIGN KEY([ProjectInitiativeLookup])
REFERENCES [dbo].[Config_ProjectInitiative] ([ID])
GO
ALTER TABLE [dbo].[PMMProjects]  WITH NOCHECK ADD FOREIGN KEY([ProjectLifeCycleLookup])
REFERENCES [dbo].[Config_ProjectLifeCycles] ([ID])
GO
ALTER TABLE [dbo].[PMMProjects]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[PMMProjects]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[PMMProjects]  WITH NOCHECK ADD FOREIGN KEY([SeverityLookup])
REFERENCES [dbo].[Config_Module_Severity] ([ID])
GO
ALTER TABLE [dbo].[PMMRisks]  WITH NOCHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[PMMTasks]  WITH CHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[PMMTasks]  WITH CHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[PMMTasks]  WITH CHECK ADD FOREIGN KEY([SprintLookup])
REFERENCES [dbo].[Sprint] ([ID])
GO
ALTER TABLE [dbo].[PMMTasks]  WITH CHECK ADD FOREIGN KEY([UserSkillMultiLookup])
REFERENCES [dbo].[UserSkills] ([ID])
GO
ALTER TABLE [dbo].[PMMTasksHistory]  WITH CHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[PMMTasksHistory]  WITH CHECK ADD FOREIGN KEY([SprintLookup])
REFERENCES [dbo].[Sprint] ([ID])
GO
ALTER TABLE [dbo].[PMMTasksHistory]  WITH CHECK ADD FOREIGN KEY([UserSkillMultiLookup])
REFERENCES [dbo].[UserSkills] ([ID])
GO
ALTER TABLE [dbo].[ProjectMonitorState]  WITH NOCHECK ADD  CONSTRAINT [FK__ProjectMo__Modul__23F3538A] FOREIGN KEY([ModuleMonitorOptionIdLookup])
REFERENCES [dbo].[Config_ModuleMonitorOptions] ([ID])
GO
ALTER TABLE [dbo].[ProjectMonitorState] NOCHECK CONSTRAINT [FK__ProjectMo__Modul__23F3538A]
GO
ALTER TABLE [dbo].[ProjectMonitorState]  WITH NOCHECK ADD  CONSTRAINT [FK__ProjectMo__Modul__24E777C3] FOREIGN KEY([ModuleMonitorOptionLEDClassLookup])
REFERENCES [dbo].[Config_ModuleMonitorOptions] ([ID])
GO
ALTER TABLE [dbo].[ProjectMonitorState] NOCHECK CONSTRAINT [FK__ProjectMo__Modul__24E777C3]
GO
ALTER TABLE [dbo].[ProjectMonitorState]  WITH NOCHECK ADD  CONSTRAINT [FK__ProjectMo__Modul__25DB9BFC] FOREIGN KEY([ModuleMonitorOptionNameLookup])
REFERENCES [dbo].[Config_ModuleMonitorOptions] ([ID])
GO
ALTER TABLE [dbo].[ProjectMonitorState] NOCHECK CONSTRAINT [FK__ProjectMo__Modul__25DB9BFC]
GO
ALTER TABLE [dbo].[ProjectMonitorState]  WITH NOCHECK ADD FOREIGN KEY([ModuleMonitorNameLookup])
REFERENCES [dbo].[Config_ModuleMonitors] ([MonitorName])
GO
ALTER TABLE [dbo].[ProjectMonitorState]  WITH NOCHECK ADD  CONSTRAINT [FK__ProjectMo__PMMId__3449B6E4] FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[ProjectMonitorState] NOCHECK CONSTRAINT [FK__ProjectMo__PMMId__3449B6E4]
GO
ALTER TABLE [dbo].[ProjectMonitorStateHistory]  WITH NOCHECK ADD  CONSTRAINT [FK__ProjectMo__Modul__29AC2CE0] FOREIGN KEY([ModuleMonitorOptionIdLookup])
REFERENCES [dbo].[Config_ModuleMonitorOptions] ([ID])
GO
ALTER TABLE [dbo].[ProjectMonitorStateHistory] NOCHECK CONSTRAINT [FK__ProjectMo__Modul__29AC2CE0]
GO
ALTER TABLE [dbo].[ProjectMonitorStateHistory]  WITH NOCHECK ADD  CONSTRAINT [FK__ProjectMo__Modul__2B947552] FOREIGN KEY([ModuleMonitorOptionNameLookup])
REFERENCES [dbo].[Config_ModuleMonitorOptions] ([ID])
GO
ALTER TABLE [dbo].[ProjectMonitorStateHistory] NOCHECK CONSTRAINT [FK__ProjectMo__Modul__2B947552]
GO
ALTER TABLE [dbo].[ProjectMonitorStateHistory]  WITH NOCHECK ADD  CONSTRAINT [FK__ProjectMo__Modul__3726238F] FOREIGN KEY([ModuleMonitorOptionLEDClassLookup])
REFERENCES [dbo].[Config_ModuleMonitorOptions] ([ID])
GO
ALTER TABLE [dbo].[ProjectMonitorStateHistory] NOCHECK CONSTRAINT [FK__ProjectMo__Modul__3726238F]
GO
ALTER TABLE [dbo].[ProjectMonitorStateHistory]  WITH NOCHECK ADD FOREIGN KEY([ModuleMonitorNameLookup])
REFERENCES [dbo].[Config_ModuleMonitors] ([MonitorName])
GO
ALTER TABLE [dbo].[ProjectMonitorStateHistory]  WITH NOCHECK ADD  CONSTRAINT [FK__ProjectMo__PMMId__0B27A5C0] FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[ProjectMonitorStateHistory] NOCHECK CONSTRAINT [FK__ProjectMo__PMMId__0B27A5C0]
GO
ALTER TABLE [dbo].[ProjectReleases]  WITH CHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[ProjectSummary]  WITH CHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[ProjectSummary]  WITH CHECK ADD FOREIGN KEY([ProjectClassLookup])
REFERENCES [dbo].[Config_ProjectClass] ([ID])
GO
ALTER TABLE [dbo].[ProjectSummary]  WITH CHECK ADD FOREIGN KEY([ProjectInitiativeLookup])
REFERENCES [dbo].[Config_ProjectInitiative] ([ID])
GO
ALTER TABLE [dbo].[ProjectSummary]  WITH CHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[PRS]  WITH NOCHECK ADD FOREIGN KEY([APPTitleLookup])
REFERENCES [dbo].[Applications] ([ID])
GO
ALTER TABLE [dbo].[PRS]  WITH NOCHECK ADD FOREIGN KEY([AssetLookup])
REFERENCES [dbo].[Assets] ([ID])
GO
ALTER TABLE [dbo].[PRS]  WITH NOCHECK ADD FOREIGN KEY([CompanyTitleLookup])
REFERENCES [dbo].[Company] ([ID])
GO
ALTER TABLE [dbo].[PRS]  WITH NOCHECK ADD FOREIGN KEY([DepartmentLookup])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[PRS]  WITH NOCHECK ADD FOREIGN KEY([DivisionLookup])
REFERENCES [dbo].[CompanyDivisions] ([ID])
GO
ALTER TABLE [dbo].[PRS]  WITH NOCHECK ADD FOREIGN KEY([FunctionalAreaLookup])
REFERENCES [dbo].[FunctionalAreas] ([ID])
GO
ALTER TABLE [dbo].[PRS]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[PRS]  WITH NOCHECK ADD FOREIGN KEY([LocationLookup])
REFERENCES [dbo].[Location] ([ID])
GO
ALTER TABLE [dbo].[PRS]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[PRS]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[PRS]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[PRS]  WITH NOCHECK ADD FOREIGN KEY([SeverityLookup])
REFERENCES [dbo].[Config_Module_Severity] ([ID])
GO
ALTER TABLE [dbo].[RCA]  WITH NOCHECK ADD FOREIGN KEY([CompanyTitleLookup])
REFERENCES [dbo].[Company] ([ID])
GO
ALTER TABLE [dbo].[RCA]  WITH NOCHECK ADD FOREIGN KEY([DepartmentLookup])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[RCA]  WITH NOCHECK ADD FOREIGN KEY([DivisionLookup])
REFERENCES [dbo].[CompanyDivisions] ([ID])
GO
ALTER TABLE [dbo].[RCA]  WITH NOCHECK ADD FOREIGN KEY([FunctionalAreaLookup])
REFERENCES [dbo].[FunctionalAreas] ([ID])
GO
ALTER TABLE [dbo].[RCA]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[RCA]  WITH NOCHECK ADD FOREIGN KEY([LocationLookup])
REFERENCES [dbo].[Location] ([ID])
GO
ALTER TABLE [dbo].[RCA]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[RCA]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[RCA]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[ReportComponents]  WITH NOCHECK ADD FOREIGN KEY([ReportDefinitionLookup])
REFERENCES [dbo].[ReportDefinition] ([ID])
GO
ALTER TABLE [dbo].[RequestTypeByLocation]  WITH NOCHECK ADD FOREIGN KEY([LocationLookup])
REFERENCES [dbo].[Location] ([ID])
GO
ALTER TABLE [dbo].[RequestTypeByLocation]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[ResourceAllocation]  WITH NOCHECK ADD FOREIGN KEY([ResourceWorkItemLookup])
REFERENCES [dbo].[ResourceWorkItems] ([ID])
GO
ALTER TABLE [dbo].[ResourceAllocationMonthly]  WITH NOCHECK ADD FOREIGN KEY([ResourceWorkItemLookup])
REFERENCES [dbo].[ResourceWorkItems] ([ID])
GO
ALTER TABLE [dbo].[Resources]  WITH NOCHECK ADD FOREIGN KEY([UserSkillLookup])
REFERENCES [dbo].[UserSkills] ([ID])
GO
ALTER TABLE [dbo].[ResourceTimeSheet]  WITH NOCHECK ADD FOREIGN KEY([ResourceWorkItemLookup])
REFERENCES [dbo].[ResourceWorkItems] ([ID])
GO
ALTER TABLE [dbo].[Sprint]  WITH NOCHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[SprintSummary]  WITH NOCHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[SprintSummary]  WITH NOCHECK ADD FOREIGN KEY([SprintLookup])
REFERENCES [dbo].[Sprint] ([ID])
GO
ALTER TABLE [dbo].[SprintTasks]  WITH NOCHECK ADD FOREIGN KEY([PMMIdLookup])
REFERENCES [dbo].[PMMProjects] ([ID])
GO
ALTER TABLE [dbo].[SprintTasks]  WITH NOCHECK ADD FOREIGN KEY([ReleaseLookup])
REFERENCES [dbo].[ProjectReleases] ([ID])
GO
ALTER TABLE [dbo].[SprintTasks]  WITH NOCHECK ADD FOREIGN KEY([SprintLookup])
REFERENCES [dbo].[Sprint] ([ID])
GO
ALTER TABLE [dbo].[SurveyFeedback]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[SVCRequests]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[SVCRequests]  WITH NOCHECK ADD FOREIGN KEY([ProjectClassLookup])
REFERENCES [dbo].[Config_ProjectClass] ([ID])
GO
ALTER TABLE [dbo].[SVCRequests]  WITH NOCHECK ADD FOREIGN KEY([ProjectClassLookup])
REFERENCES [dbo].[Config_ProjectClass] ([ID])
GO
ALTER TABLE [dbo].[SVCRequests]  WITH NOCHECK ADD FOREIGN KEY([ProjectInitiativeLookup])
REFERENCES [dbo].[Config_ProjectInitiative] ([ID])
GO
ALTER TABLE [dbo].[SVCRequests]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[SVCRequests]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[TaskTemplateItems]  WITH NOCHECK ADD FOREIGN KEY([TaskTemplateLookup])
REFERENCES [dbo].[TaskTemplates] ([ID])
GO
ALTER TABLE [dbo].[TaskTemplates]  WITH NOCHECK ADD FOREIGN KEY([ProjectLifeCycleLookup])
REFERENCES [dbo].[Config_ProjectLifeCycles] ([ID])
GO
ALTER TABLE [dbo].[TSKProjects]  WITH NOCHECK ADD FOREIGN KEY([FunctionalAreaLookup])
REFERENCES [dbo].[FunctionalAreas] ([ID])
GO
ALTER TABLE [dbo].[TSKProjects]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[TSKProjects]  WITH NOCHECK ADD FOREIGN KEY([NPRIdLookup])
REFERENCES [dbo].[NPRRequest] ([ID])
GO
ALTER TABLE [dbo].[TSKProjects]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[TSKProjects]  WITH NOCHECK ADD FOREIGN KEY([ProjectClassLookup])
REFERENCES [dbo].[Config_ProjectClass] ([ID])
GO
ALTER TABLE [dbo].[TSKProjects]  WITH NOCHECK ADD FOREIGN KEY([ProjectInitiativeLookup])
REFERENCES [dbo].[Config_ProjectInitiative] ([ID])
GO
ALTER TABLE [dbo].[TSKProjects]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[TSKProjects]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[TSKProjects]  WITH NOCHECK ADD FOREIGN KEY([SeverityLookup])
REFERENCES [dbo].[Config_Module_Severity] ([ID])
GO
ALTER TABLE [dbo].[TSKTasks]  WITH NOCHECK ADD FOREIGN KEY([TSKIDLookup])
REFERENCES [dbo].[TSKProjects] ([ID])
GO
ALTER TABLE [dbo].[UPR]  WITH NOCHECK ADD FOREIGN KEY([AssetLookup])
REFERENCES [dbo].[Assets] ([ID])
GO
ALTER TABLE [dbo].[UPR]  WITH NOCHECK ADD FOREIGN KEY([DepartmentLookup])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[UPR]  WITH NOCHECK ADD FOREIGN KEY([FunctionalAreaLookup])
REFERENCES [dbo].[FunctionalAreas] ([ID])
GO
ALTER TABLE [dbo].[UPR]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[UPR]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[UPR]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[UPR]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[UPR]  WITH NOCHECK ADD FOREIGN KEY([SeverityLookup])
REFERENCES [dbo].[Config_Module_Severity] ([ID])
GO
ALTER TABLE [dbo].[VCCRequest]  WITH NOCHECK ADD FOREIGN KEY([APPTitleLookup])
REFERENCES [dbo].[Applications] ([ID])
GO
ALTER TABLE [dbo].[VCCRequest]  WITH NOCHECK ADD FOREIGN KEY([AssetLookup])
REFERENCES [dbo].[Assets] ([ID])
GO
ALTER TABLE [dbo].[VCCRequest]  WITH NOCHECK ADD FOREIGN KEY([CompanyTitleLookup])
REFERENCES [dbo].[Company] ([ID])
GO
ALTER TABLE [dbo].[VCCRequest]  WITH NOCHECK ADD FOREIGN KEY([DepartmentLookup])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[VCCRequest]  WITH NOCHECK ADD FOREIGN KEY([DivisionLookup])
REFERENCES [dbo].[CompanyDivisions] ([ID])
GO
ALTER TABLE [dbo].[VCCRequest]  WITH NOCHECK ADD FOREIGN KEY([FunctionalAreaLookup])
REFERENCES [dbo].[FunctionalAreas] ([ID])
GO
ALTER TABLE [dbo].[VCCRequest]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[VCCRequest]  WITH NOCHECK ADD FOREIGN KEY([LocationLookup])
REFERENCES [dbo].[Location] ([ID])
GO
ALTER TABLE [dbo].[VCCRequest]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[VCCRequest]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[VCCRequest]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[VCCRequest]  WITH NOCHECK ADD FOREIGN KEY([SeverityLookup])
REFERENCES [dbo].[Config_Module_Severity] ([ID])
GO
ALTER TABLE [dbo].[VCCRequest]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VCCRequest]  WITH NOCHECK ADD FOREIGN KEY([VendorMSANameLookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VCCRequest]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VCCRequest]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWNameLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorApprovedSubcontractors]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorApprovedSubcontractors]  WITH NOCHECK ADD FOREIGN KEY([VendorMSANameLookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorIssues]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorIssues]  WITH NOCHECK ADD FOREIGN KEY([VendorMSANameLookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorIssues]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorIssues]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWNameLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorKeyPersonnel]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorKeyPersonnel]  WITH NOCHECK ADD FOREIGN KEY([VendorMSANameLookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorMSA]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[VendorMSA]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[VendorMSA]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[VendorMSA]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[VendorMSA]  WITH NOCHECK ADD FOREIGN KEY([SeverityLookup])
REFERENCES [dbo].[Config_Module_Severity] ([ID])
GO
ALTER TABLE [dbo].[VendorMSA]  WITH NOCHECK ADD FOREIGN KEY([VendorDetailLookup])
REFERENCES [dbo].[Vendors] ([ID])
GO
ALTER TABLE [dbo].[VendorMSAMeeting]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorMSAMeeting]  WITH NOCHECK ADD FOREIGN KEY([VendorMSANameLookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorPO]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorPO]  WITH NOCHECK ADD FOREIGN KEY([VendorMSANameLookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorPO]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorPO]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWNameLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorPOLineItems]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorPOLineItems]  WITH NOCHECK ADD FOREIGN KEY([VendorMSANameLookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorPOLineItems]  WITH NOCHECK ADD FOREIGN KEY([VendorPOLookup])
REFERENCES [dbo].[VendorPO] ([ID])
GO
ALTER TABLE [dbo].[VendorPOLineItems]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorPOLineItems]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWNameLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorReport]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorReport]  WITH NOCHECK ADD FOREIGN KEY([VendorMSANameLookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorReport]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorReport]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWNameLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorReportInstance]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorReportInstance]  WITH NOCHECK ADD FOREIGN KEY([VendorMSANameLookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorReportInstance]  WITH NOCHECK ADD FOREIGN KEY([VendorReportLookup])
REFERENCES [dbo].[VendorReport] ([ID])
GO
ALTER TABLE [dbo].[VendorReportInstance]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorReportInstance]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWNameLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorRisks]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorRisks]  WITH NOCHECK ADD FOREIGN KEY([VendorMSANameLookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorRisks]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorRisks]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWNameLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorServiceDuration]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorSLA]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[VendorSLA]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[VendorSLA]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[VendorSLA]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[VendorSLA]  WITH NOCHECK ADD FOREIGN KEY([SeverityLookup])
REFERENCES [dbo].[Config_Module_Severity] ([ID])
GO
ALTER TABLE [dbo].[VendorSLA]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorSLA]  WITH NOCHECK ADD FOREIGN KEY([VendorMSANameLookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorSLA]  WITH NOCHECK ADD FOREIGN KEY([VendorResourceCategoryLookup])
REFERENCES [dbo].[Config_VendorResourceCategory] ([ID])
GO
ALTER TABLE [dbo].[VendorSLA]  WITH NOCHECK ADD FOREIGN KEY([VendorResourceCategoryLookup])
REFERENCES [dbo].[Config_VendorResourceCategory] ([ID])
GO
ALTER TABLE [dbo].[VendorSLA]  WITH NOCHECK ADD FOREIGN KEY([VendorResourceSubCategoryLookup])
REFERENCES [dbo].[Config_VendorResourceCategory] ([ID])
GO
ALTER TABLE [dbo].[VendorSLA]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorSLA]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWNameLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorSLAPerformance]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[VendorSLAPerformance]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorSLAPerformance]  WITH NOCHECK ADD FOREIGN KEY([VendorMSANameLookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorSLAPerformance]  WITH NOCHECK ADD FOREIGN KEY([VendorSLALookup])
REFERENCES [dbo].[VendorSLA] ([ID])
GO
ALTER TABLE [dbo].[VendorSLAPerformance]  WITH NOCHECK ADD FOREIGN KEY([VendorSLANameLookup])
REFERENCES [dbo].[VendorSLA] ([ID])
GO
ALTER TABLE [dbo].[VendorSLAPerformance]  WITH NOCHECK ADD FOREIGN KEY([VendorVPMLookup])
REFERENCES [dbo].[VendorVPM] ([ID])
GO
ALTER TABLE [dbo].[VendorSOW]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[VendorSOW]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[VendorSOW]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[VendorSOW]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[VendorSOW]  WITH NOCHECK ADD FOREIGN KEY([SeverityLookup])
REFERENCES [dbo].[Config_Module_Severity] ([ID])
GO
ALTER TABLE [dbo].[VendorSOW]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorSOW]  WITH NOCHECK ADD FOREIGN KEY([VendorMSANameLookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWContImprovement]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWContImprovement]  WITH NOCHECK ADD FOREIGN KEY([VendorMSANameLookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWContImprovement]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWContImprovement]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWNameLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWFees]  WITH NOCHECK ADD FOREIGN KEY([VendorPOLineItemLookup])
REFERENCES [dbo].[VendorPOLineItems] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWFees]  WITH NOCHECK ADD FOREIGN KEY([VendorPOLookup])
REFERENCES [dbo].[VendorPO] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWFees]  WITH NOCHECK ADD FOREIGN KEY([VendorResourceCategoryLookup])
REFERENCES [dbo].[Config_VendorResourceCategory] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWFees]  WITH NOCHECK ADD FOREIGN KEY([VendorResourceSubCategoryLookup])
REFERENCES [dbo].[Config_VendorResourceCategory] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWFees]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWFees]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWNameLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoiceDetail]  WITH NOCHECK ADD FOREIGN KEY([SOWInvoiceLookup])
REFERENCES [dbo].[VendorSOWInvoices] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoiceDetail]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoiceDetail]  WITH NOCHECK ADD FOREIGN KEY([VendorMSANameLookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoiceDetail]  WITH NOCHECK ADD FOREIGN KEY([VendorPOLineItemLookup])
REFERENCES [dbo].[VendorPOLineItems] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoiceDetail]  WITH NOCHECK ADD FOREIGN KEY([VendorPOLookup])
REFERENCES [dbo].[VendorPO] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoiceDetail]  WITH NOCHECK ADD FOREIGN KEY([VendorResourceCategoryLookup])
REFERENCES [dbo].[Config_VendorResourceCategory] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoiceDetail]  WITH NOCHECK ADD FOREIGN KEY([VendorResourceSubCategoryLookup])
REFERENCES [dbo].[Config_VendorResourceCategory] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoiceDetail]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWFeeLookup])
REFERENCES [dbo].[VendorSOWFees] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoiceDetail]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoiceDetail]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWNameLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoices]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoices]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoices]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoices]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoices]  WITH NOCHECK ADD FOREIGN KEY([SeverityLookup])
REFERENCES [dbo].[Config_Module_Severity] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoices]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoices]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWNameLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoices]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorSOWInvoices]  WITH NOCHECK ADD FOREIGN KEY([VendorMSANameLookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorVPM]  WITH NOCHECK ADD FOREIGN KEY([ImpactLookup])
REFERENCES [dbo].[Config_Module_Impact] ([ID])
GO
ALTER TABLE [dbo].[VendorVPM]  WITH NOCHECK ADD FOREIGN KEY([PriorityLookup])
REFERENCES [dbo].[Config_Module_Priority] ([ID])
GO
ALTER TABLE [dbo].[VendorVPM]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[VendorVPM]  WITH NOCHECK ADD FOREIGN KEY([ServiceLookUp])
REFERENCES [dbo].[Config_Services] ([ID])
GO
ALTER TABLE [dbo].[VendorVPM]  WITH NOCHECK ADD FOREIGN KEY([SeverityLookup])
REFERENCES [dbo].[Config_Module_Severity] ([ID])
GO
ALTER TABLE [dbo].[VendorVPM]  WITH NOCHECK ADD FOREIGN KEY([VendorMSALookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorVPM]  WITH NOCHECK ADD FOREIGN KEY([VendorMSANameLookup])
REFERENCES [dbo].[VendorMSA] ([ID])
GO
ALTER TABLE [dbo].[VendorVPM]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[VendorVPM]  WITH NOCHECK ADD FOREIGN KEY([VendorSOWNameLookup])
REFERENCES [dbo].[VendorSOW] ([ID])
GO
ALTER TABLE [dbo].[WikiArticles]  WITH NOCHECK ADD FOREIGN KEY([RequestTypeLookup])
REFERENCES [dbo].[Config_Module_RequestType] ([ID])
GO
ALTER TABLE [dbo].[WorkflowSLASummary]  WITH NOCHECK ADD FOREIGN KEY([RuleNameLookup])
REFERENCES [dbo].[Config_Module_SLARule] ([ID])
GO
USE [master]
GO
ALTER DATABASE [uGovernITFinal] SET  READ_WRITE 
GO
