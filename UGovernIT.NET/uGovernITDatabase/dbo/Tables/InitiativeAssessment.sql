CREATE TABLE [dbo].[InitiativeAssessment]
(
	[ID] Bigint NOT NULL PRIMARY KEY,
	[BusinessInitiativeLookup] bigint null,
	[ImpactRevenueIncreaseChoice] nvarchar(max) null,
	[ImpactBusinessGrowthChoice] nvarchar(max) null,
	[ImpactReducesRiskChoice] nvarchar(max) null,
	[ImpactReducesExpensesChoice] nvarchar(max) null,
	[ImpactDecisionMakingChoice] nvarchar(max) null,
	[BreakEvenIn] Int,
	[EliminatesHeadcount] Int,
	[ROI] nvarchar(250),
	[OtherDescribe] nvarchar(max) null,
	[TenantID]                        NVARCHAR (128)  NOT NULL,
    [Created]                         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]                  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                         BIT             DEFAULT (0) NULL,
    [Attachments]                     NVARCHAR (2000) DEFAULT ('') NULL
)
