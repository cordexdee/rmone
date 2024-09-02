CREATE TABLE [dbo].[Contracts_Archive](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActualCompletionDate] [datetime] NULL,
	[ActualStartDate] [datetime] NULL,
	[AnnualMaintenanceCost] [nvarchar](250) NULL,
	[BusinessManagerUser] [nvarchar](max) NULL,
	[Closed] [bit] NULL,
	[CloseDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
	[ContractExpirationDate] [datetime] NULL,
	[ContractStartDate] [datetime] NULL,
	[CurrentStageStartDate] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[DesiredCompletionDate] [datetime] NULL,
	[EstimatedHours] [int] NULL,
	[FinanceManagerUser] [nvarchar](max) NULL,
	[FunctionalAreaLookup] [bigint] NULL,
	[History] [nvarchar](max) NULL,
	[InitialCost] [nvarchar](250) NULL,
	[InitiatorUser] [nvarchar](max) NULL,
	[IsPrivate] [bit] NULL,
	[LegalUser] [nvarchar](max) NULL,
	[LicenseCount] [int] NULL,
	[ModuleStepLookup] [nvarchar](250) NULL,
	[NeedReviewChoice] [nvarchar](max) NULL,
	[OnHold] [int] NULL,
	[OnHoldReasonChoice] [nvarchar](max) NULL,
	[OnHoldStartDate] [datetime] NULL,
	[OnHoldTillDate] [datetime] NULL,
	[OwnerUser] [nvarchar](250) NULL,
	[PctComplete] [int] NULL,
	[PONumber] [nvarchar](250) NULL,
	[PriorityLookup] [bigint] NULL,
	[PurchaseInstructions] [nvarchar](max) NULL,
	[PurchasingUser] [nvarchar](max) NULL,
	[Quantity] [int] NULL,
	[ReminderBody] [nvarchar](max) NULL,
	[ReminderDate] [datetime] NULL,
	[ReminderDays] [int] NULL,
	[ReminderToUser] [nvarchar](250) NULL,
	[RenewalCancelNoticeDays] [int] NULL,
	[RepeatIntervalChoice] [nvarchar](max) NULL,
	[RequestorUser] [nvarchar](250) NULL,
	[RequestSourceChoice] [nvarchar](max) NULL,
	[RequestTypeCategory] [nvarchar](250) NULL,
	[RequestTypeLookup] [bigint] NULL,
	[RequestTypeWorkflow] [nvarchar](250) NULL,
	[ResolutionComments] [nvarchar](max) NULL,
	[ReSubmissionDate] [datetime] NULL,
	[ServiceLookUp] [bigint] NULL,
	[StageActionUsersUser] [nvarchar](250) NULL,
	[StageActionUserTypes] [nvarchar](250) NULL,
	[StageStep] [int] NULL,
	[Status] [nvarchar](250) NULL,
	[StatusChanged] [int] NULL,
	[TargetCompletionDate] [datetime] NULL,
	[TargetStartDate] [datetime] NULL,
	[TermTypeChoice] [nvarchar](max) NULL,
	[TicketId] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
	[TotalHoldDuration] [int] NULL,
	[UserQuestionSummary] [nvarchar](max) NULL,
	[WorkflowSkipStages] [nvarchar](250) NULL,
	[TenantID] [nvarchar](128) NULL,
	[DataEditors] [nvarchar](250) NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[CreatedByUser] [nvarchar](128) NOT NULL,
	[ModifiedByUser] [nvarchar](128) NOT NULL,
	[Deleted] [bit] NULL,
	[Attachments] [nvarchar](2000) NULL,
	[ReopenCount] INT NULL, 
	[ApprovalDate] DATETIME NULL, 
    [ApprovedByUser] VARCHAR(128) NULL, 
    PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Contracts_Archive] ADD  DEFAULT ('') FOR [DataEditors]
GO

ALTER TABLE [dbo].[Contracts_Archive] ADD  DEFAULT (getdate()) FOR [Created]
GO

ALTER TABLE [dbo].[Contracts_Archive] ADD  DEFAULT (getdate()) FOR [Modified]
GO

ALTER TABLE [dbo].[Contracts_Archive] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [CreatedByUser]
GO

ALTER TABLE [dbo].[Contracts_Archive] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [ModifiedByUser]
GO

ALTER TABLE [dbo].[Contracts_Archive] ADD  DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [dbo].[Contracts_Archive] ADD  DEFAULT ('') FOR [Attachments]
GO

