CREATE TABLE [dbo].[Applications_Archive](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AccessAdminUser] [nvarchar](250) NULL,
	[AccessManageLevel] [nvarchar](max) NULL,
	[AllocatedSeats] [int] NULL,
	[ApprovalTypeChoice] [nvarchar](max) NULL,
	[ApproverUser] [nvarchar](250) NULL,
	[BuildNumber] [nvarchar](250) NULL,
	[BusinessManagerUser] [nvarchar](max) NULL,
	[CategoryNameChoice] [nvarchar](250) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[CreationDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[DepartmentLookup] [bigint] NULL,
	[Description] [nvarchar](max) NULL,
	[DocumentLibraryName] [nvarchar](250) NULL,
	[FunctionalAreaLookup] [bigint] NULL,
	[History] [nvarchar](max) NULL,
	[InitiatorUser] [nvarchar](max) NULL,
	[IssueTypeOptions] [nvarchar](max) NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NumberOfSeats] [int] NULL,
	[OnHold] [int] NULL,
	[OwnerUser] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NULL,
	[SoftwareKey] [nvarchar](250) NULL,
	[SoftwareMajorVersion] [nvarchar](250) NULL,
	[SoftwareMinorVersion] [nvarchar](250) NULL,
	[SoftwarePatchRevision] [nvarchar](250) NULL,
	[SoftwareVersion] [nvarchar](250) NULL,
	[StageActionUsersUser] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[SubCategory] [nvarchar](250) NULL,
	[SupportedByUser] [nvarchar](250) NULL,
	[SyncAtModuleLevel] [bit] NULL,
	[SyncToRequestType] [bit] NULL,
	[TicketId] [nvarchar](250) NULL,
	[VendorName] [nvarchar](250) NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[Title] [varchar](250) NULL,
	[LicenseBasisChoice] [nvarchar](250) NULL,
	[NumLicensesTotal] [int] NULL,
	[NumLicensesTotalNotes] [nvarchar](250) NULL,
	[NumUsers] [int] NULL,
	[HostingTypeChoice] [nvarchar](250) NULL,
	[VendorLookup] [bigint] NULL,
	[InProductionSince] [nvarchar](250) NULL,
	[FrequencyOfUpgradesChoice] [nvarchar](250) NULL,
	[FrequencyOfTypicalUse] [nvarchar](250) NULL,
	[FrequencyOfUpgradesNotes] [nvarchar](250) NULL,
	[NextPlannedMajorUpgrade] [nvarchar](500) NULL,
	[VersionInstalled] [nvarchar](250) NULL,
	[VersionLatestRelease] [nvarchar](250) NULL,
	[TenantID] [nvarchar](128) NULL,
	[DataEditors] [nvarchar](250) NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[CreatedByUser] [nvarchar](128) NOT NULL,
	[ModifiedByUser] [nvarchar](128) NOT NULL,
	[Deleted] [bit] NULL,
	[Attachments] [nvarchar](2000) NULL,
	[IsDeleted] [bit] NULL,
	[ResolutionDate] [datetime] NULL,
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
	[MaintenanceWindowStart] [nvarchar](250) NULL,
	[MaintenanceWindowEnd] [nvarchar](250) NULL,
	[SLADisabled] [bit] NULL,
	[AssignedBy] [nvarchar](128) NULL,
	[ApplicationTier] [nvarchar](250) NULL,
	[ApplicationType] [nvarchar](250) NULL,
	[IsPDSA] [bit] NULL,
	[IsProductApp] [bit] NULL,
	[InteractsWithMSOffice] [bit] NULL,
	[ContainsPI] [bit] NULL,
	[OnSB272List] [bit] NULL,
	[OnICD10List] [bit] NULL,
	[SystemPurpose] [nvarchar](max) NULL,
	[SponsorsUser] [nvarchar](128) NULL,
	[BusinessFunction] [nvarchar](250) NULL,
	[ResolvedByUser] [nvarchar](128) NULL,
	[ClosedByUser] [nvarchar](128) NULL,
	[UsesADAuthentication] [bit] NULL,
	[Complexity] [nvarchar](250) NULL,
	[PriorityLookup] [bigint] NULL,
	[Platform] [nvarchar](250) NULL,
	[ClientUIPlatform] [nvarchar](250) NULL,
	[DBType] [nvarchar](250) NULL,
	[InstallLocation] [nvarchar](250) NULL,
	[Availability] [nvarchar](250) NULL,
	[NumUserNotes] [nvarchar](250) NULL,
	[UserType] [nvarchar](250) NULL,
	[ComplianceNotes] [nvarchar](max) NULL,
	[ProgrammingLanguage] [nvarchar](250) NULL,
	[InterfacesOutgoing] [nvarchar](max) NULL,
	[InterfacesIncoming] [nvarchar](max) NULL,
	[PlannedActivities] [nvarchar](max) NULL,
	[EOLTarget] [nvarchar](250) NULL,
	[BusinessUsage] [nvarchar](250) NULL,
	[ExternalAppName] [nvarchar](250) NULL,
	[VersionInstalledDate] [datetime] NULL,
	[ExternalBusinessPartners] [nvarchar](max) NULL,
	[ExternalUsers] [bit] NULL,
	[ExternalUsersLoginRequired] [bit] NULL,
	[BusinessImpact] [nvarchar](250) NULL,
	[ContractStatus] [nvarchar](250) NULL,
	[FrontEndTechnology] [nvarchar](250) NULL,
	[BackEndTechnology] [nvarchar](250) NULL,
	[MiddlewareTechnology] [nvarchar](250) NULL,
	[FileSystem] [nvarchar](250) NULL,
	[ConfigurationDataLocation] [nvarchar](250) NULL,
	[ClientSoftware] [nvarchar](250) NULL,
	[CreatesCookies] [nvarchar](250) NULL,
	[UsesHTMLStorage] [nvarchar](250) NULL,
	[AdminAccount] [nvarchar](250) NULL,
	[BackEndAccess] [nvarchar](250) NULL,
	[FrontEndAccess] [nvarchar](250) NULL,
	[OtherAccess] [nvarchar](250) NULL,
	[LicenseExpiratonDate] [datetime] NULL,
	[InternalProcurementContact] [nvarchar](250) NULL,
	[UserRange] [nvarchar](250) NULL,
	[AccessType] [nvarchar](250) NULL,
	[TouchPointsType] [nvarchar](250) NULL,
	[TechnologyComplexity] [nvarchar](250) NULL,
	[TechnologyMaturity] [nvarchar](250) NULL,
	[ScheduleRisk] [nvarchar](50) NULL,
	[FinancialImpact] [nvarchar](250) NULL,
	[BusinessAdoptionRisk] [nvarchar](250) NULL,
	[ImpactsRevenue] [varchar](250) NULL,
	[ImpactOnUsers] [nvarchar](250) NULL,
	[SecurityAndCompliance] [nvarchar](250) NULL,
	[AcceptableLeadTimeToFix] [nvarchar](250) NULL,
	[ComplexityScore] [int] NULL,
	[RiskScore] [int] NULL,
	[BusinessImpactScore] [int] NULL,
	[ComplexityClassification] [nvarchar](250) NULL,
	[RiskBusinessImpactClass] [nvarchar](250) NULL,
	[ApplicationRedundancy] [nvarchar](250) NULL,
	[ApplicationImportance] [nvarchar](250) NULL,
	[ApplicationPrivacy] [nvarchar](250) NULL,
	[InProductionDate] [datetime] NULL,
	[MaintenanceContractStartDate] [datetime] NULL,
	[MaintenanceContractEndDate] [datetime] NULL,
	[NextUpgradeDate] [datetime] NULL,
	[InterfacesIncomingApps] [nvarchar](250) NULL,
	[InterfacesOutgoingApps] [nvarchar](250) NULL,
	[TicketBusinessManager2User] [nvarchar](128) NULL,
	[TicketOwner2] [nvarchar](128) NULL,
	[SupportedBy2] [nvarchar](128) NULL,
	[AppTypeRating] [nvarchar](250) NULL,
	[LatestVersionReleaseDate] [datetime] NULL,
	[ExtendedSupportEndDate] [datetime] NULL,
	[EndOfLifeDate] [datetime] NULL,
	[Integrity] [nvarchar](250) NULL,
	[RecoveryTimeObjective] [nvarchar](250) NULL,
	[RecoveryPointObjective] [nvarchar](250) NULL,
	[SupportedBusinessProcesses] [nvarchar](max) NULL,
	[Standard] [nvarchar](128) NULL,
	[StandardReviewDate] [datetime] NULL,
	[NextStandardReviewDate] [datetime] NULL,
	[AuthenticationTypes] [nvarchar](50) NULL,
	[AuthenticationMechanisms] [nvarchar](50) NULL,
	[UserAuthInterfaces] [nvarchar](50) NULL,
	[SSOTypes] [nvarchar](50) NULL,
	[ApprovalDate] DATETIME NULL, 
    [ApprovedByUser] VARCHAR(128) NULL, 
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ('') FOR [DataEditors]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT (getdate()) FOR [Created]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT (getdate()) FOR [Modified]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [CreatedByUser]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [ModifiedByUser]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ('') FOR [Attachments]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ((0)) FOR [SLADisabled]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [AssignedBy]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ((0)) FOR [IsPDSA]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ((0)) FOR [IsProductApp]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ((0)) FOR [InteractsWithMSOffice]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ((0)) FOR [ContainsPI]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ((0)) FOR [OnSB272List]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ((0)) FOR [OnICD10List]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [SponsorsUser]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [ResolvedByUser]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [ClosedByUser]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ((0)) FOR [UsesADAuthentication]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ((0)) FOR [ExternalUsers]
GO

ALTER TABLE [dbo].[Applications_Archive] ADD  DEFAULT ((0)) FOR [ExternalUsersLoginRequired]
GO
