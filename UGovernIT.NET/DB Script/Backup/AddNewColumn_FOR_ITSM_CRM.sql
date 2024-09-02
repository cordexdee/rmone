alter table TSR
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table ACR
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table INC
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table DRQ
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table SVCRequests
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table INC
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table RCA
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table NPR
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table PMM
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table DashboardSummary
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table BTS
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table Contracts
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table Applications
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table Assets
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table ITG
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO

alter table TSR_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table ACR_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table INC_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table DRQ_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table SVCRequests_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table INC_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table RCA_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table NPR_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table PMM_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table BTS_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table Contracts_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table Applications_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table Assets_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table ITG_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table PRS_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table PRS
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO

-- Select distinct ModuleTable from Config_Modules
/*FOR CRM*/


alter table Lead
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table Opportunity
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table CRMCompany
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table CRMContact
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table CRMProject
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table CRMServices
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table Lead_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table Opportunity_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table CRMCompany_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table CRMContact_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table CRMProject_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
alter table CRMServices_Archive
add ApprovalDate datetime, ApprovedByUser varchar(128)
GO
 

// Added against BTS-23-001101: ITSM Migration


ALTER TABLE 	ACR	ADD 	BusinessManager2User	varchar(MAX)	;
ALTER TABLE 	ACR	ADD 	ClosedByUser	varchar(MAX)	;
ALTER TABLE 	ACR	ADD 	DepartmentManagerUser	varchar(MAX)	;
ALTER TABLE 	ACR	ADD 	DivisionManagerUser	varchar(MAX)	;
ALTER TABLE 	ACR	ADD 	AssignedByUser	varchar(MAX)	;
ALTER TABLE 	ACR	ADD 	SubLocationLookup	bigint	;
ALTER TABLE 	ACR	ADD 	OrganizationalImpactChoice	varchar(max)	;
ALTER TABLE 	ACR	ADD 	ReleaseID	varchar(500)	;
ALTER TABLE 	ACR	ADD 	RiskLevelChoice	varchar(500)	;
ALTER TABLE 	ACR	ADD 	ApproverUser	varchar(MAX)	;
ALTER TABLE 	ACR	ADD 	Approver2User	varchar(MAX)	;
ALTER TABLE 	ACR	ADD 	OnHoldStartDate	datetime	;
ALTER TABLE 	ACR_Archive	ADD 	Rejected	bit	;
ALTER TABLE 	ACR_Archive	ADD 	UserQuestionSummary	varchar(max)	;
ALTER TABLE 	ACR_Archive	ADD 	BusinessManager2User	varchar(max)	;
ALTER TABLE 	ACR_Archive	ADD 	DataEditor	varchar(500)	;
ALTER TABLE 	ACR_Archive	ADD 	ClosedByUser	varchar(MAX)	;
ALTER TABLE 	ACR_Archive	ADD 	ResolvedByUser	varchar(MAX)	;
ALTER TABLE 	ACR_Archive	ADD 	DepartmentManagerUser	varchar(max)	;
ALTER TABLE 	ACR_Archive	ADD 	DivisionManagerUser	varchar(max)	;
ALTER TABLE 	ACR_Archive	ADD 	AssignedByUser	varchar(MAX)	;
ALTER TABLE 	ACR_Archive	ADD 	SubLocationLookup	bigint	;
ALTER TABLE 	ACR_Archive	ADD 	OrganizationalImpactChoice	varchar(max)	;
ALTER TABLE 	ACR_Archive	ADD 	ReleaseID	varchar(500)	;
ALTER TABLE 	ACR_Archive	ADD 	RiskLevelChoice	varchar(500)	;
ALTER TABLE 	ACR_Archive	ADD 	ApproverUser	varchar(MAX)	;
ALTER TABLE 	ACR_Archive	ADD 	Approver2User	varchar(MAX)	;
ALTER TABLE 	ACR_Archive	ADD 	OnHoldStartDate	datetime	;
ALTER TABLE 	Applications	ADD 	AppLifeCycleChoice	varchar(500)	;
ALTER TABLE 	Applications	ADD 	RequestTypeCategory	varchar(500)	;
ALTER TABLE 	Applications	ADD 	RequestTypeSubCategory	varchar(500)	;
ALTER TABLE 	Applications	ADD 	SecurityDescription	varchar(MAX)	;
ALTER TABLE 	Applications	ADD 	AuthenticationChoice	varchar(500)	;
ALTER TABLE 	Applications	ADD 	NumUsers2	varchar(max)	;
 
ALTER TABLE 	Applications	ADD 	SupportedBrowsersChoice	varchar(500)	;
ALTER TABLE 	AssetModels	ADD 	ExternalType	varchar(500)	;
ALTER TABLE 	AssetRelations	ADD 	ChildTicketId	varchar(128)	;
ALTER TABLE 	AssetRelations	ADD 	ParentTicketId	varchar(128)	;
ALTER TABLE 	Assets	ADD 	AssignedByUser	varchar(MAX)	;
ALTER TABLE 	Assets	ADD 	SubLocationLookup	bigint	;
ALTER TABLE 	Assets	ADD 	ProductionCritical	bit	;
ALTER TABLE 	Assets	ADD 	Firmware	varchar(500)	;
ALTER TABLE 	Assets	ADD 	VersionNumber	varchar(500)	;
ALTER TABLE 	Assets	ADD 	Unmanaged	bit	;
ALTER TABLE 	Assets	ADD 	SSLCertName	varchar(500)	;
ALTER TABLE 	Assets	ADD 	SSLCertExpiration	datetime	;
ALTER TABLE 	Assets	ADD 	ContractLookup	varchar(128)	;
ALTER TABLE 	Assets	ADD 	ProductReleaseDate	datetime	;
ALTER TABLE 	Assets	ADD 	EndOfSaleDate	datetime	;
ALTER TABLE 	Assets	ADD 	EndOfSoftwareMaintenanceDate	datetime	;
ALTER TABLE 	Assets	ADD 	EndOfSecurityUpdatesDate	datetime	;
ALTER TABLE 	Assets	ADD 	EndOfSupportDate	datetime	;
ALTER TABLE 	Assets	ADD 	EndOfExtendedSupportDate	datetime	;
ALTER TABLE 	Assets	ADD 	EndOfLifeDate	datetime	;
ALTER TABLE 	Assets	ADD 	StandardRefreshPeriod	varchar(500)	;
ALTER TABLE 	Assets	ADD 	StandardChoice	varchar(500)	;
ALTER TABLE 	Assets	ADD 	StandardReviewDate	datetime	;
ALTER TABLE 	Assets	ADD 	NextStandardReviewDate	datetime	;
ALTER TABLE 	Assets	ADD 	OnHold	int	;
ALTER TABLE 	Assets	ADD 	OnHoldStartDate	datetime	;
ALTER TABLE 	Assets	ADD 	TotalHoldDuration	int	;
ALTER TABLE 	Assets	ADD 	OnHoldTillDate	datetime	;
ALTER TABLE 	Assets	ADD 	OnHoldReasonChoice	varchar(500)	;
ALTER TABLE 	Assets	ADD 	BackedUpComponentsChoice	varchar(500)	;
ALTER TABLE 	Assets	ADD 	OrderNum	varchar(500)	;
ALTER TABLE 	Assets	ADD 	NonStandardConfiguration	bit	;
ALTER TABLE 	Assets_Archive	ADD 	ExternalID	varchar(500)	;
ALTER TABLE 	Assets_Archive	ADD 	AssignedByUser	varchar(MAX)	;
ALTER TABLE 	Assets_Archive	ADD 	SubLocationLookup	bigint	;
ALTER TABLE 	Assets_Archive	ADD 	ProductionCritical	bit	;
ALTER TABLE 	Assets_Archive	ADD 	Firmware	varchar(500)	;
ALTER TABLE 	Assets_Archive	ADD 	VersionNumber	varchar(500)	;
ALTER TABLE 	Assets_Archive	ADD 	Unmanaged	bit	;
ALTER TABLE 	Assets_Archive	ADD 	SSLCertName	varchar(500)	;
ALTER TABLE 	Assets_Archive	ADD 	SSLCertExpiration	datetime	;
ALTER TABLE 	Assets_Archive	ADD 	SupplierLookup	bigint	;
ALTER TABLE 	Assets_Archive	ADD 	ContractLookup	varchar(128)	;
ALTER TABLE 	Assets_Archive	ADD 	ContractTitleLookup	bigint	;
ALTER TABLE 	Assets_Archive	ADD 	ProductReleaseDate	datetime	;
ALTER TABLE 	Assets_Archive	ADD 	EndOfSaleDate	datetime	;
ALTER TABLE 	Assets_Archive	ADD 	EndOfSoftwareMaintenanceDate	datetime	;
ALTER TABLE 	Assets_Archive	ADD 	EndOfSecurityUpdatesDate	datetime	;
ALTER TABLE 	Assets_Archive	ADD 	EndOfSupportDate	datetime	;
ALTER TABLE 	Assets_Archive	ADD 	EndOfExtendedSupportDate	datetime	;
ALTER TABLE 	Assets_Archive	ADD 	EndOfLifeDate	datetime	;
ALTER TABLE 	Assets_Archive	ADD 	StandardRefreshPeriod	varchar(500)	;
ALTER TABLE 	Assets_Archive	ADD 	StandardChoice	varchar(500)	;
ALTER TABLE 	Assets_Archive	ADD 	StandardReviewDate	datetime	;
ALTER TABLE 	Assets_Archive	ADD 	NextStandardReviewDate	datetime	;
ALTER TABLE 	Assets_Archive	ADD 	OnHold	int	;
ALTER TABLE 	Assets_Archive	ADD 	OnHoldStartDate	datetime	;
ALTER TABLE 	Assets_Archive	ADD 	TotalHoldDuration	int	;
ALTER TABLE 	Assets_Archive	ADD 	OnHoldTillDate	datetime	;
ALTER TABLE 	Assets_Archive	ADD 	OnHoldReason	varchar(500)	;
ALTER TABLE 	Assets_Archive	ADD 	BackedUpComponentsChoice	varchar(500)	;
ALTER TABLE 	Assets_Archive	ADD 	OrderNum	varchar(500)	;
ALTER TABLE 	Assets_Archive	ADD 	CellPhoneNumber	varchar(500)	;
ALTER TABLE 	Assets_Archive	ADD 	LocalAdmins	varchar(500)	;
ALTER TABLE 	Assets_Archive	ADD 	NonStandardConfiguration	bit	;
ALTER TABLE 	BTS	ADD 	Rejected	bit	;
ALTER TABLE 	BTS	ADD 	SubLocationLookup	bigint	;
ALTER TABLE 	BTS	ADD 	ClientLookup	bigint	;
ALTER TABLE 	BTS	ADD 	ReleaseID	varchar(500)	;
ALTER TABLE 	BTS	ADD 	ReleaseDate	datetime	;
ALTER TABLE 	BTS_Archive	ADD 	Rejected	bit	;
ALTER TABLE 	BTS_Archive	ADD 	ClosedByuser	varchar(max)	;
ALTER TABLE 	BTS_Archive	ADD 	ResolvedByUser	varchar(MAX)	;
ALTER TABLE 	BTS_Archive	ADD 	ElevatedPriority	bit	;
ALTER TABLE 	BTS_Archive	ADD 	AssignedByUser	varchar(MAX)	;
ALTER TABLE 	BTS_Archive	ADD 	SubLocationLookup	bigint	;
ALTER TABLE 	BTS_Archive	ADD 	ClientLookup	bigint	;
ALTER TABLE 	BTS_Archive	ADD 	ReleaseID	varchar(500)	;
ALTER TABLE 	BTS_Archive	ADD 	ReleaseDate	datetime	;
ALTER TABLE 	BTS_Archive	ADD 	ResolutionDate	datetime	;
ALTER TABLE 	Contracts	ADD 	AssetLookup	bigint	;
ALTER TABLE 	Contracts	ADD 	Rejected	bit	;
ALTER TABLE 	Contracts	ADD 	ActualHours	int	;
ALTER TABLE 	Contracts	ADD 	ClosedByUser	varchar(MAX)	;
ALTER TABLE 	Contracts	ADD 	ResolvedByUser	varchar(MAX)	;
ALTER TABLE 	Contracts	ADD 	VendorLookup	bigint	;
ALTER TABLE 	Contracts	ADD 	AssignedByUser	varchar(MAX)	;
ALTER TABLE 	Contracts	ADD 	Archived	bit	;
ALTER TABLE 	Contracts	ADD 	Extensions	varchar(MAX)	;
ALTER TABLE 	Contracts	ADD 	FriendlyName	varchar(500)	;
ALTER TABLE 	Contracts	ADD 	Handle	varchar(500)	;
ALTER TABLE 	Contracts	ADD 	HasPrivateKey	bit	;
ALTER TABLE 	Contracts	ADD 	IssuerName	varchar(500)	;
ALTER TABLE 	Contracts	ADD 	NotAfter	datetime	;
ALTER TABLE 	Contracts	ADD 	NotBefore	datetime	;
ALTER TABLE 	Contracts	ADD 	PublicKey	varchar(500)	;
ALTER TABLE 	Contracts	ADD 	RawData	varchar(500)	;
ALTER TABLE 	Contracts	ADD 	SignatureAlgorithm	varchar(500)	;
ALTER TABLE 	Contracts	ADD 	Subject	varchar(500)	;
ALTER TABLE 	Contracts	ADD 	SubjectName	varchar(500)	;
ALTER TABLE 	Contracts	ADD 	Thumbprint	varchar(500)	;
ALTER TABLE 	Contracts	ADD 	DnsNameList	varchar(MAX)	;
ALTER TABLE 	Contracts	ADD 	EnhancedKeyUsageList	varchar(max)	;
ALTER TABLE 	Contracts	ADD 	SendAsTrustedIssuer	bit	;
ALTER TABLE 	Contracts	ADD 	APPTitleLookup	bigint	;
ALTER TABLE 	Contracts	ADD 	PaymentTerms	varchar(max)	;
ALTER TABLE 	Contracts	ADD 	ManufacturingContact	varchar(500)	;
ALTER TABLE 	Contracts	ADD 	VendorContact	varchar(500)	;
ALTER TABLE 	Contracts	ADD 	SalesRepName	varchar(500)	;
ALTER TABLE 	Contracts	ADD 	SalesRepContact	varchar(500)	;
ALTER TABLE 	Contracts	ADD 	CurrentFundingProject	varchar(500)	;
ALTER TABLE 	Contracts	ADD 	CurrentGWOFields	varchar(max)	;
ALTER TABLE 	Contracts	ADD 	LocationMultLookup	varchar(max)	;
ALTER TABLE 	DashboardSummary	ADD 	FCRCategorization	varchar(500)	;
ALTER TABLE 	DashboardSummary	ADD 	BulkRequestCount	int	;
ALTER TABLE 	DashboardSummary	ADD 	BreakFix	varchar(500)	;
ALTER TABLE 	INC	ADD 	Rejected	bit	;
ALTER TABLE 	INC	ADD 	IssueTypeChoice	varchar(500)	;
ALTER TABLE 	INC	ADD 	ClosedByUser	varchar(MAX)	;
ALTER TABLE 	INC	ADD 	ResolvedByUser	varchar(MAX)	;
ALTER TABLE 	INC	ADD 	TotalHoldDuration	int	;
ALTER TABLE 	INC_Archive	ADD 	Rejected	bit	;
ALTER TABLE 	INC_Archive	ADD 	DataEditor	varchar(500)	;
ALTER TABLE 	INC_Archive	ADD 	IssueTypeChoice	varchar(500)	;
ALTER TABLE 	INC_Archive	ADD 	ClosedByUser	varchar(MAX)	;
ALTER TABLE 	INC_Archive	ADD 	ResolvedByUser	varchar(MAX)	;
ALTER TABLE 	INC_Archive	ADD 	TotalHoldDuration	int	;
ALTER TABLE 	ITGovernance	ADD 	DataEditor	varchar(500)	;
ALTER TABLE 	ITGovernance	ADD 	ResolvedByUser	varchar(MAX)	;
ALTER TABLE 	LinkItems	ADD 	DisableDiscussion	bit	;
ALTER TABLE 	LinkItems	ADD 	DisableRelatedItems	bit	;
ALTER TABLE 	LinkItems	ADD 	LeftPaneExpanded	bit	;
ALTER TABLE 	LinkItems	ADD 	BottomPaneExpanded	bit	;
ALTER TABLE 	ModuleStageConstraints	ADD 	RelatedItems	varchar(500)	;
ALTER TABLE 	ModuleStageConstraintTemplates	ADD 	RelatedItems	varchar(500)	;
ALTER TABLE 	NPR	ADD 	Rejected	bit	;
ALTER TABLE 	NPR	ADD 	ActualHours	int	;
ALTER TABLE 	NPR	ADD 	CustomerProgram	varchar(500)	;
ALTER TABLE 	NPR	ADD 	ClientLookup	bigint	;
ALTER TABLE 	NPR	ADD 	ProjectContractDecision	varchar(500)	;
ALTER TABLE 	NPR	ADD 	ProjectDataDecision	varchar(500)	;
ALTER TABLE 	NPR	ADD 	ProjectApplicationDecision	varchar(500)	;
ALTER TABLE 	NPR	ADD 	OwnerUser2User	varchar(max)	;
ALTER TABLE 	NPR	ADD 	BusinessManager2User	varchar(max)	;
ALTER TABLE 	NPR	ADD 	ITManager2User	varchar(MAX)	;
ALTER TABLE 	NPR	ADD 	CostSavings	money	;
ALTER TABLE 	NPR	ADD 	HighLevelRequirements	varchar(max)	;
ALTER TABLE 	NPR	ADD 	SpecificInclusions	varchar(MAX)	;
ALTER TABLE 	NPR	ADD 	SpecificExclusions	varchar(MAX)	;
ALTER TABLE 	NPR	ADD 	ProjectApplicationStatusChoice	varchar(500);	
ALTER TABLE 	NPR_Archive	ADD 	ActualHours	int	;
ALTER TABLE 	NPR_Archive	ADD 	ClientLookup	bigint	;
ALTER TABLE 	NPR_Archive	ADD 	ProjectContractDecision	varchar(500)	;
ALTER TABLE 	NPR_Archive	ADD 	ProjectDataDecision	varchar(500)	;
ALTER TABLE 	NPR_Archive	ADD 	ProjectApplicationDecision	varchar(500)	;
ALTER TABLE 	NPR_Archive	ADD 	ContractStatusChoice	varchar(500)	;
ALTER TABLE 	NPR_Archive	ADD 	ProjectDataStatusChoice	varchar(500)	;
ALTER TABLE 	NPR_Archive	ADD 	ProjectApplicationStatusChoice	varchar(500)	;
ALTER TABLE 	NPR_Archive	ADD 	OwnerUser2User	varchar(max)	;
ALTER TABLE 	NPR_Archive	ADD 	BusinessManager2User	varchar(max)	;
ALTER TABLE 	NPR_Archive	ADD 	ITManager2User	varchar(MAX)	;
ALTER TABLE 	NPR_Archive	ADD 	CostSavings	money	;
ALTER TABLE 	NPR_Archive	ADD 	HighLevelRequirements	varchar(max)	;
ALTER TABLE 	NPR_Archive	ADD 	SpecificInclusions	varchar(MAX)	;
ALTER TABLE 	NPR_Archive	ADD 	SpecificExclusions	varchar(MAX)	;
ALTER TABLE 	NPR_Archive	ADD 	ProjectDeliverables	varchar(max)	;
ALTER TABLE 	PMM	ADD 	Rejected	bit	;
--ALTER TABLE 	PMM	ADD 	ImpactReducesRiskChoice	varchar(MAX)	;
--ALTER TABLE 	PMM	ADD 	ImpactIncreasesProductivityChoice	varchar(MAX)	;

ALTER TABLE 	PMM	ADD 	ProjectConstraints	varchar(max)	;
ALTER TABLE 	PMM	ADD 	AssignedAnalyst	varchar(max)	;
ALTER TABLE 	PMM	ADD 	AssignedByUser	varchar(MAX)	;
ALTER TABLE 	PMM	ADD 	ProjectState	varchar(500)	;
ALTER TABLE 	PMM	ADD 	TechnologyImpactChoice	varchar(max)	;
ALTER TABLE 	PMM	ADD 	VendorSupport	varchar(500)	;
ALTER TABLE 	PMM	ADD 	PctROI	int	;
ALTER TABLE 	PMM	ADD 	StrategicInitiativeChoice	varchar(max)	;
ALTER TABLE 	PMM	ADD 	ProjectDirector	varchar(max)	;
ALTER TABLE 	PMM	ADD 	ContributionToStrategy	varchar(max)	;
ALTER TABLE 	PMM	ADD 	PaybackCostSavings	varchar(max)	;
ALTER TABLE 	PMM	ADD 	CustomerBenefit	varchar(max)	;
ALTER TABLE 	PMM	ADD 	Regulatory	varchar(max)	;
ALTER TABLE 	PMM	ADD 	CustomerProgram	varchar(max)	;
ALTER TABLE 	PMM	ADD 	ProductCode	varchar(max)	;
ALTER TABLE 	PMM	ADD 	ProductName	varchar(500)	;
ALTER TABLE 	PMM	ADD 	ClientLookup	bigint	;
ALTER TABLE 	PMM	ADD 	ContractStatusChoice	varchar(500)	;
ALTER TABLE 	PMM	ADD 	ProjectContractDecision	varchar(500)	;
ALTER TABLE 	PMM	ADD 	ProjectDataStatusChoice	varchar(500)	;
ALTER TABLE 	PMM	ADD 	ProjectDataDecision	varchar(500)	;
ALTER TABLE 	PMM	ADD 	ProjectApplicationStatusChoice	varchar(500)	;
ALTER TABLE 	PMM	ADD 	ProjectApplicationDecision	varchar(500)	;
ALTER TABLE 	PMM	ADD 	OwnerUser2User	varchar(max)	;
ALTER TABLE 	PMM	ADD 	BusinessManager2User	varchar(max)	;
ALTER TABLE 	PMM	ADD 	ITManager2User	varchar(MAX)	;
ALTER TABLE 	PMM	ADD 	CostSavings	money	;
ALTER TABLE 	PMM	ADD 	HighLevelRequirements	varchar(max)	;
ALTER TABLE 	PMM	ADD 	SpecificInclusions	varchar(MAX)	;
ALTER TABLE 	PMM	ADD 	SpecificExclusions	varchar(MAX)	;
ALTER TABLE 	PMM	ADD 	ProjectDeliverables	varchar(max)	;
--ALTER TABLE 	PMM	ADD 	ImpactReducesExpensesChoice	varchar(max)	;
--ALTER TABLE 	PMM	ADD 	ImpactDecisionMakingChoice	varchar(max)	;
--ALTER TABLE 	PMM	ADD 	ImpactRevenueIncreaseChoice	varchar(max)	;
--ALTER TABLE 	PMM	ADD 	ImpactBusinessGrowthChoice	varchar(MAX)	;

ALTER TABLE 	PMM_Archive	ADD 	Rejected	bit	;
ALTER TABLE 	PMM_Archive	ADD 	ClassificationSizeChoice	varchar(max)	;
ALTER TABLE 	PMM_Archive	ADD 	ProjectConstraints	varchar(max)	;
ALTER TABLE 	PMM_Archive	ADD 	AssignedAnalyst	varchar(max)	;
ALTER TABLE 	PMM_Archive	ADD 	ProjectState	varchar(500)	;
ALTER TABLE 	PMM_Archive	ADD 	AssignedByUser	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	IsITGApprovalRequired	bit	;
ALTER TABLE 	PMM_Archive	ADD 	IsSteeringApprovalRequired	bit	;
ALTER TABLE 	PMM_Archive	ADD 	ClassificationNotes	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	ClassificationScope	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	ProjectComplexity	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	OrganizationalImpactChoice	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	TechnologyUsabilityChoice	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	TechnologyReliabilityChoice	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	TechnologySecurityChoice	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	TechnologyImpactChoice	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	InternalCapabilityChoice	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	VendorSupport	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	AdoptionRiskChoice	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	ProjectEstSizeMinHrs	int	;
ALTER TABLE 	PMM_Archive	ADD 	ProjectEstSizeMaxHrs	int	;
ALTER TABLE 	PMM_Archive	ADD 	ProjectEstDurationMinDays	int	;
ALTER TABLE 	PMM_Archive	ADD 	ProjectEstDurationMaxDays	int	;
ALTER TABLE 	PMM_Archive	ADD 	ImpactRevenueIncrease	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	ImpactBusinessGrowthChoice	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	ImpactReducesRisk	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	ImpactIncreasesProductivity	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	ImpactReducesExpenses	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	ImpactDecisionMaking	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	BreakEvenIn	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	EliminatesHeadcount	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	OtherDescribe	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	PctROI	int	;
ALTER TABLE 	PMM_Archive	ADD 	StrategicInitiativeChoice	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	ProjectDirector	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	ContributionToStrategy	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	PaybackCostSavings	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	CustomerBenefit	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	RegulatoryChoice	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	ITLifecycleRefreshChoice	varchar(max)	;
ALTER TABLE 	PMM_Archive	ADD 	CustomerProgram	varchar(max)	;
ALTER TABLE 	PMM_Archive	ADD 	ProductCode	varchar(max)	;
ALTER TABLE 	PMM_Archive	ADD 	ProductName	varchar(500)	;
ALTER TABLE 	PMM_Archive	ADD 	ClientLookup	bigint	;
ALTER TABLE 	PMM_Archive	ADD 	ProjectCoordinators	varchar(max)	;
ALTER TABLE 	PMM_Archive	ADD 	NextMilestoneDate	datetime	;
ALTER TABLE 	PMM_Archive	ADD 	ContractStatusChoice	varchar(500)	;
ALTER TABLE 	PMM_Archive	ADD 	ProjectContractDecision	varchar(500)	;
ALTER TABLE 	PMM_Archive	ADD 	ProjectDataStatusChoice	varchar(500)	;
ALTER TABLE 	PMM_Archive	ADD 	ProjectDataDecision	varchar(500)	;
ALTER TABLE 	PMM_Archive	ADD 	ProjectApplicationStatusChoice	varchar(500)	;
ALTER TABLE 	PMM_Archive	ADD 	ProjectApplicationDecision	varchar(500)	;
ALTER TABLE 	PMM_Archive	ADD 	OwnerUser2User	varchar(max)	;
ALTER TABLE 	PMM_Archive	ADD 	BusinessManager2User	varchar(max)	;
ALTER TABLE 	PMM_Archive	ADD 	ITManager2User	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	CostSavings	money	;
ALTER TABLE 	PMM_Archive	ADD 	HighLevelRequirements	varchar(max)	;
ALTER TABLE 	PMM_Archive	ADD 	SpecificInclusions	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	SpecificExclusions	varchar(MAX)	;
ALTER TABLE 	PMM_Archive	ADD 	ProjectDeliverables	varchar(max)	;
ALTER TABLE 	PMMHistory	ADD 	Rejected	bit	;
ALTER TABLE 	PMMHistory	ADD 	ClassificationSizeChoice	varchar(max)	;
ALTER TABLE 	PMMHistory	ADD 	ProjectConstraints	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	AssignedAnalyst	varchar(500)	;
ALTER TABLE 	PMMHistory	ADD 	ProjectState	varchar(500)	;
ALTER TABLE 	PMMHistory	ADD 	AssignedByUser	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	IsITGApprovalRequired	bit	;
ALTER TABLE 	PMMHistory	ADD 	IsSteeringApprovalRequired	bit	;
ALTER TABLE 	PMMHistory	ADD 	ClassificationNotes	varchar(max)	;
ALTER TABLE 	PMMHistory	ADD 	ClassificationScope	varchar(max)	;
ALTER TABLE 	PMMHistory	ADD 	ProjectComplexity	varchar(max)	;
ALTER TABLE 	PMMHistory	ADD 	TechnologyReliabilityChoice	varchar(max)	;
ALTER TABLE 	PMMHistory	ADD 	TechnologySecurityChoice	varchar(max)	;
ALTER TABLE 	PMMHistory	ADD 	TechnologyImpactChoice	Varchar(max)	;
ALTER TABLE 	PMMHistory	ADD 	InternalCapabilityChoice	varchar(max)	;
ALTER TABLE 	PMMHistory	ADD 	VendorSupport	varchar(500)	;
ALTER TABLE 	PMMHistory	ADD 	ImpactRevenueIncreaseChoice	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	ImpactBusinessGrowthChoice	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	ImpactReducesRiskChoice	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	ImpactIncreasesProductivityChoice	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	ImpactReducesExpensesChoice	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	ImpactDecisionMakingChoice	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	BreakEvenIn	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	EliminatesHeadcount	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	OtherDescribe	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	PctROI	int	;
ALTER TABLE 	PMMHistory	ADD 	PMMIdLookup	bigint	;
ALTER TABLE 	PMMHistory	ADD 	BaselineNum	int	;
ALTER TABLE 	PMMHistory	ADD 	StrategicInitiativeChoice	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	ProjectDirector	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	ContributionToStrategy	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	PaybackCostSavings	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	CustomerBenefitChoice	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	RegulatoryChoice	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	ITLifecycleRefreshChoice	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	CustomerProgram	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	ProductCode	varchar(MAX)	;
ALTER TABLE 	PMMHistory	ADD 	ProductName	varchar(500)	;
ALTER TABLE 	PMMHistory	ADD 	ClientLookup	bigint	;
ALTER TABLE 	PMMHistory	ADD 	CostSavings	money	;
ALTER TABLE 	ProjectSummary	ADD 	RelatedItems	varchar(500)	;
ALTER TABLE 	PRS	ADD 	Rejected	bit	;
ALTER TABLE 	PRS	ADD 	IssueTypeChoice	varchar(500)	;
ALTER TABLE 	PRS	ADD 	ClosedByUser	varchar(MAX)	;
ALTER TABLE 	PRS	ADD 	ResolvedByUser	varchar(MAX)	;
ALTER TABLE 	PRS	ADD 	AssignedByUser	varchar(MAX)	;
ALTER TABLE 	PRS	ADD 	DataEditor	varchar(500)	;
ALTER TABLE 	PRS	ADD 	DepartmentManagerUser	varchar(MAX)	;
ALTER TABLE 	PRS	ADD 	DivisionManagerUser	varchar(MAX)	;
ALTER TABLE 	PRS	ADD 	ApproverUser	varchar(MAX)	;
ALTER TABLE 	PRS	ADD 	Approver2User	varchar(MAX)	;
ALTER TABLE 	PRS	ADD 	UserQuestionSummary	varchar(max)	;
ALTER TABLE 	PRS_Archive	ADD 	Rejected	bit	;
ALTER TABLE 	PRS_Archive	ADD 	ClosedByUser	varchar(MAX)	;
ALTER TABLE 	PRS_Archive	ADD 	ResolvedByUser	varchar(MAX)	;
ALTER TABLE 	PRS_Archive	ADD 	AssignedByUser	varchar(MAX)	;
ALTER TABLE 	PRS_Archive	ADD 	SubLocationLookup	bigint	;
ALTER TABLE 	PRS_Archive	ADD 	SLADisabled	bit	;
ALTER TABLE 	PRS_Archive	ADD 	DataEditor	varchar(500)	;
ALTER TABLE 	PRS_Archive	ADD 	DepartmentManagerUser	varchar(MAX)	;
ALTER TABLE 	PRS_Archive	ADD 	DivisionManagerUser	varchar(MAX)	;
ALTER TABLE 	PRS_Archive	ADD 	ApproverUser	varchar(MAX)	;
ALTER TABLE 	PRS_Archive	ADD 	Approver2User	varchar(MAX)	;
ALTER TABLE 	PRS_Archive	ADD 	ResolutionDate	datetime	;
ALTER TABLE 	PRS_Archive	ADD 	UserQuestionSummary	varchar(max)	;
ALTER TABLE 	RCA	ADD 	ActualHours	int	;
ALTER TABLE 	RCA	ADD 	FinalCountermeasure	varchar(max)	;
ALTER TABLE 	RCA_Archive	ADD 	ActualHours	int	;
ALTER TABLE 	RCA_Archive	ADD 	FinalCountermeasure	varchar(max)	;
ALTER TABLE 	Config_Module_RequestType	ADD 	MatchAllKeywords	bit	;
ALTER TABLE 	Config_Module_RequestType	ADD 	BreakFix	varchar(500);
ALTER TABLE 	SchedulerActionArchives	ADD 	FileLocation	varchar(max);
ALTER TABLE 	Config_Service_ServiceQuestions	ADD 	EnableZoomIn bit;
ALTER TABLE 	Config_Services	ADD 	DataEditor	varchar(500);
ALTER TABLE 	Config_Service_ServiceColumns	ADD 	SVCRequestLookup	bigint	;
ALTER TABLE 	Config_Service_ServiceColumns	ADD 	ParentId	bigint	;
ALTER TABLE 	Config_Service_ServiceColumns	ADD 	IsDefault	bit	;
ALTER TABLE 	Config_Service_ServiceColumns	ADD 	Predecessor	varchar(500)	;
ALTER TABLE 	Config_Service_ServiceColumns	ADD 	RequestTypeLookup	bigint	;
ALTER TABLE 	Config_Service_ServiceColumns	ADD 	Name	varchar(500)	;
ALTER TABLE 	Config_Service_ServiceDefaultValues	ADD 	ServiceQuestionTitleLookup	bigint	;
ALTER TABLE 	Config_Service_ServiceDefaultValues	ADD 	ServiceLookup	bigint	;
ALTER TABLE 	Config_Service_ServiceRelationships	ADD 	AutoFillRequestor	bit	;
ALTER TABLE 	Config_Service_ServiceRelationships	ADD 	QuestionProperties	varchar(500)	;
ALTER TABLE 	Config_Service_ServiceRelationships	ADD 	QuestionID	varchar(500)	;
ALTER TABLE 	Config_Service_ServiceRelationships	ADD 	SLADisabled	bit	;
ALTER TABLE 	Config_Service_ServiceRelationships	ADD 	NotificationDisabled	bit	;
ALTER TABLE 	SurveyFeedback	ADD 	PRPUser	varchar(max)	;
ALTER TABLE 	SurveyFeedback	ADD 	OwnerUser	varchar(max)	;
ALTER TABLE 	SVCRequests	ADD 	Rejected	bit	;
ALTER TABLE 	SVCRequests	ADD 	ClosedByUser	varchar(MAX)	;
ALTER TABLE 	SVCRequests	ADD 	AssignedByUser	varchar(MAX)	;
ALTER TABLE 	SVCRequests	ADD 	BulkRequestCount	int	;
ALTER TABLE 	SVCRequests	ADD 	Approver2User	varchar(MAX)	;
ALTER TABLE 	SVCRequests	ADD 	NextSLAType	varchar(500)	;
ALTER TABLE 	SVCRequests	ADD 	NextSLATime	datetime	;
ALTER TABLE 	SVCRequests	ADD 	LocationLookup	bigint	;
ALTER TABLE 	SVCRequests	ADD 	CompanyTitleLookup	bigint	;
ALTER TABLE 	SVCRequests	ADD 	DivisionLookup	bigint	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGText01	varchar(500)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGText02	varchar(500)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGText03	varchar(500)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGText04	varchar(500)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGText05	varchar(500)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGText06	varchar(500)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGText07	varchar(500)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGText08	varchar(500)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGDate01	datetime	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGDate02	datetime	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGDate03	datetime	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGDate04	datetime	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGUser01	varchar(max)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGUser02	varchar(max)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGUser03	varchar(max)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGUser04	varchar(max)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGUserMulti01	varchar(max)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGUserMulti02	varchar(max)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGUserMulti03	varchar(max)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGUserMulti04	varchar(max)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGChoice01	varchar(500)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGChoice02	varchar(500)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGChoice03	varchar(500)	;
ALTER TABLE 	SVCRequests	ADD 	CustomUGChoice04	varchar(500)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	Rejected	bit	;
ALTER TABLE 	SVCRequests_Archive	ADD 	ORPUser	varchar(MAX)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	ClosedByUser	varchar(MAX)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	ResolvedByUser	varchar(MAX)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	AssignedByUser	varchar(MAX)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	BulkRequestCount	int	;
ALTER TABLE 	SVCRequests_Archive	ADD 	DataEditor	varchar(500)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	ReopenCount	int	;
ALTER TABLE 	SVCRequests_Archive	ADD 	Approver2User	varchar(MAX)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	NextSLAType	varchar(500)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	NextSLATime	datetime	;
ALTER TABLE 	SVCRequests_Archive	ADD 	LocationLookup	bigint	;
ALTER TABLE 	SVCRequests_Archive	ADD 	DepartmentLookup	bigint	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CompanyTitleLookup	bigint	;
ALTER TABLE 	SVCRequests_Archive	ADD 	DivisionLookup	bigint	;
ALTER TABLE 	SVCRequests_Archive	ADD 	EnableTaskReminder	bit	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGText01	varchar(500)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGText02	varchar(500)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGText03	varchar(500)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGText04	varchar(500)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGText05	varchar(500)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGText06	varchar(500)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGText07	varchar(500)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGText08	varchar(500)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGDate01	datetime	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGDate02	datetime	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGDate03	datetime	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGDate04	datetime	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGUser01	varchar(max)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGUser02	varchar(max)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGUser03	varchar(max)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGUser04	varchar(max)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGUserMulti01	varchar(max)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGUserMulti02	varchar(max)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGUserMulti03	varchar(max)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGUserMulti04	varchar(max)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGChoice01	varchar(500)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGChoice02	varchar(500)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGChoice03	varchar(500)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	CustomUGChoice04	varchar(500)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	ResolutionDate	datetime	;
ALTER TABLE 	SVCRequests_Archive	ADD 	Age	int	;
ALTER TABLE 	SVCRequests_Archive	ADD 	PONumber	varchar(500)	;
ALTER TABLE 	SVCRequests_Archive	ADD 	Quantity	int	;
ALTER TABLE 	SVCRequests_Archive	ADD 	Quantity2	int	;
ALTER TABLE 	TicketCountTrends	ADD 	Title	varchar(500)	;
ALTER TABLE 	TicketEvents	ADD 	Attachments	varchar(max)	;
ALTER TABLE 	TicketHours	ADD 	Title	varchar(500)	;
ALTER TABLE 	Relationship	ADD 	AutoFillRequestor	bit	;
ALTER TABLE 	Relationship	ADD 	UserFieldXML	varchar(max)	;
ALTER TABLE 	Relationship	ADD 	TotalHoldDuration	int	;
ALTER TABLE 	Relationship	ADD 	OnHoldStartDate	datetime	;
ALTER TABLE 	Relationship	ADD 	OnHold	int	;
ALTER TABLE 	Relationship	ADD 	OnHoldTillDate	datetime	;
ALTER TABLE 	Relationship	ADD 	OnHoldReasonChoice	varchar(500)	;
ALTER TABLE 	Relationship	ADD 	TaskActualStartDate	datetime	;
ALTER TABLE 	Relationship	ADD 	SLADisabled	bit	;
ALTER TABLE 	Relationship	ADD 	UGITNewUserDisplayName	varchar(max)	;
ALTER TABLE 	Relationship	ADD 	NotificationDisabled	bit	;
ALTER TABLE 	WorkflowSLASummary	ADD 	CreationDate	datetime	;
ALTER TABLE 	WorkflowSLASummary	ADD 	CloseDate	datetime	;
ALTER TABLE 	WorkflowSLASummary	ADD 	Status	varchar(500)	;
ALTER TABLE 	WorkflowSLASummary	ADD 	OnHoldStartDate	datetime	;
ALTER TABLE 	WorkflowSLASummary	ADD 	OnHoldTillDate	datetime	;
ALTER TABLE 	TSK	ADD 	Rejected	bit	;
ALTER TABLE 	TSK	ADD 	ActualHours	int	;
ALTER TABLE 	TSK	ADD 	IsPrivate	bit	;
ALTER TABLE 	TSK	ADD 	ClosedByUser	varchar(MAX)	;
ALTER TABLE 	TSK	ADD 	ResolvedByUser	varchar(MAX)	;
ALTER TABLE 	TSK	ADD 	AssignedByUser	varchar(MAX)	;
ALTER TABLE 	TSK_Archive	ADD 	Rejected	bit	;
ALTER TABLE 	TSK_Archive	ADD 	ActualHours	int	;
ALTER TABLE 	TSK_Archive	ADD 	IsPrivate	bit	;
ALTER TABLE 	TSK_Archive	ADD 	ClosedByUser	varchar(MAX)	;
ALTER TABLE 	TSK_Archive	ADD 	ResolvedByUser	varchar(MAX)	;
ALTER TABLE 	TSK_Archive	ADD 	AssignedByUser	varchar(MAX)	;
ALTER TABLE 	TSR	ADD 	Rejected	bit	;
ALTER TABLE 	TSR	ADD 	BusinessManager2User	varchar(max)	;
ALTER TABLE 	TSR	ADD 	ClosedByUser	varchar(MAX)	;
ALTER TABLE 	TSR	ADD 	DepartmentManagerUser	varchar(MAX)	;
ALTER TABLE 	TSR	ADD 	DivisionManagerUSer	varchar(MAX)	;
ALTER TABLE 	TSR	ADD 	ProjectCode	varchar(500)	;
ALTER TABLE 	TSR	ADD 	PONumber	varchar(500)	;
ALTER TABLE 	TSR	ADD 	PackingListNumber	varchar(500)	;
ALTER TABLE 	TSR	ADD 	AssetCondition	varchar(500)	;
ALTER TABLE 	TSR	ADD 	VendorLookup	bigint	;
ALTER TABLE 	TSR	ADD 	PODate	datetime	;
ALTER TABLE 	TSR	ADD 	ActualCost	int	;
ALTER TABLE 	TSR	ADD 	ChargeBackAmount	money	;
ALTER TABLE 	TSR	ADD 	ReceivedOn	datetime	;
ALTER TABLE 	TSR	ADD 	WarrantyExpirationDate	datetime	;
ALTER TABLE 	TSR	ADD 	QuoteAmount	money	;
ALTER TABLE 	TSR	ADD 	FCRCategorization	varchar(500)	;
ALTER TABLE 	TSR	ADD 	AfterHours	int	;
ALTER TABLE 	TSR	ADD 	AssignedByUser	varchar(MAX)	;
ALTER TABLE 	TSR	ADD 	ReviewRequired	bit	;
ALTER TABLE 	TSR	ADD 	SubLocationLookup	bigint	;
ALTER TABLE 	TSR	ADD 	Quantity	int	;
ALTER TABLE 	TSR	ADD 	BulkRequestCount	int	;
ALTER TABLE 	TSR	ADD 	BreakFix	varchar(500)	;
ALTER TABLE 	TSR	ADD 	Quantity2	int	;
ALTER TABLE 	TSR	ADD 	RCADisabled	bit	;
ALTER TABLE 	TSR	ADD 	RCARequested	bit	;
ALTER TABLE 	TSR	ADD 	ApproverUser	varchar(MAX)	;
ALTER TABLE 	TSR	ADD 	Approver2User	varchar(MAX)	;
ALTER TABLE 	TSR_Archive	ADD 	Rejected	bit	;
ALTER TABLE 	TSR_Archive	ADD 	BusinessManager2User	varchar(max)	;
ALTER TABLE 	TSR_Archive	ADD 	ClosedByUser	varchar(MAX)	;
ALTER TABLE 	TSR_Archive	ADD 	DepartmentManagerUser	varchar(MAX)	;
ALTER TABLE 	TSR_Archive	ADD 	DivisionManagerUSer	varchar(MAX)	;
ALTER TABLE 	TSR_Archive	ADD 	ProjectCode	varchar(500)	;
ALTER TABLE 	TSR_Archive	ADD 	PONumber	varchar(500)	;
ALTER TABLE 	TSR_Archive	ADD 	PackingListNumber	varchar(500)	;
ALTER TABLE 	TSR_Archive	ADD 	AssetCondition	varchar(500)	;
ALTER TABLE 	TSR_Archive	ADD 	InStock	varchar(500)	;
ALTER TABLE 	TSR_Archive	ADD 	VendorLookup	bigint	;
ALTER TABLE 	TSR_Archive	ADD 	PODate	datetime	;
ALTER TABLE 	TSR_Archive	ADD 	ActualCost	int	;
ALTER TABLE 	TSR_Archive	ADD 	ChargeBackAmount	money	;
ALTER TABLE 	TSR_Archive	ADD 	ReceivedOn	datetime	;
ALTER TABLE 	TSR_Archive	ADD 	WarrantyExpirationDate	datetime	;
ALTER TABLE 	TSR_Archive	ADD 	QuoteAmount	money	;
ALTER TABLE 	TSR_Archive	ADD 	FCRCategorization	varchar(500)	;
ALTER TABLE 	TSR_Archive	ADD 	AfterHours	int	;
ALTER TABLE 	TSR_Archive	ADD 	AssignedByUser	varchar(MAX)	;
ALTER TABLE 	TSR_Archive	ADD 	ReviewRequired	bit	;
ALTER TABLE 	TSR_Archive	ADD 	SubLocationLookup	bigint	;
ALTER TABLE 	TSR_Archive	ADD 	Quantity	int	;
ALTER TABLE 	TSR_Archive	ADD 	BulkRequestCount	int	;
ALTER TABLE 	TSR_Archive	ADD 	BreakFix	varchar(500)	;
ALTER TABLE 	TSR_Archive	ADD 	Quantity2	int	;
ALTER TABLE 	TSR_Archive	ADD 	RCADisabled	bit	;
ALTER TABLE 	TSR_Archive	ADD 	RCARequested	bit	;
ALTER TABLE 	TSR_Archive	ADD 	ApproverUser	varchar(MAX)	;
ALTER TABLE 	TSR_Archive	ADD 	Approver2User	varchar(MAX)	;
ALTER TABLE 	AspNetUsers	ADD 	Title	varchar(500)	;
ALTER TABLE 	AspNetUsers	ADD 	Notes	varchar(max)	;
ALTER TABLE 	AspNetUsers	ADD 	SipAddress	varchar(max)	;
ALTER TABLE 	AspNetUsers	ADD 	Locale	varchar(500)	;
ALTER TABLE 	AspNetUsers	ADD 	CalendarType	varchar(500)	;
ALTER TABLE 	AspNetUsers	ADD 	AdjustHijriDays	int	;
ALTER TABLE 	AspNetUsers	ADD 	TimeZone	varchar(500)	;
ALTER TABLE 	AspNetUsers	ADD 	Time24	varchar(500)	;
ALTER TABLE 	AspNetUsers	ADD 	AltCalendarType	varchar(500)	;
ALTER TABLE 	AspNetUsers	ADD 	CalendarViewOptions	varchar(500)	;
ALTER TABLE 	AspNetUsers	ADD 	WorkDays	int	;
ALTER TABLE 	AspNetUsers	ADD 	WorkDayStartHour	int	;
ALTER TABLE 	AspNetUsers	ADD 	WorkDayEndHour	int	;
ALTER TABLE 	AspNetUsers	ADD 	MUILanguages	varchar(500)	;
ALTER TABLE 	AspNetUsers	ADD 	ContentLanguages	varchar(500)	;
ALTER TABLE 	AspNetUsers	ADD 	IsSiteAdmin	bit	;
ALTER TABLE 	AspNetUsers	ADD 	UserInfoHidden	varchar(max)	;
ALTER TABLE 	AspNetUsers	ADD 	IsActive	bit	;
ALTER TABLE 	AspNetUsers	ADD 	BudgetLookup	bigint	;
ALTER TABLE 	AspNetUsers	ADD 	GroupLink	varchar(max)	;
ALTER TABLE 	AspNetUsers	ADD 	GroupEdit	varchar(max)	;
ALTER TABLE 	AspNetUsers	ADD 	ImnName	varchar(500)	;
ALTER TABLE 	AspNetUsers	ADD 	PictureDisp	varchar(max)	;
ALTER TABLE 	AspNetUsers	ADD 	NameWithPicture	varchar(500)	;
ALTER TABLE 	AspNetUsers	ADD 	NameWithPictureAndDetails	varchar(500)	;
ALTER TABLE 	AspNetUsers	ADD 	EditUser	varchar(max)	;
ALTER TABLE 	AspNetUsers	ADD 	UserSelection	varchar(max)	;
ALTER TABLE 	AspNetUsers	ADD 	ContentTypeDisp	varchar(max)	;
ALTER TABLE 	WikiArticles	ADD 	WikiLikedByUser	varchar(MAX)	;
ALTER TABLE 	WikiArticles	ADD 	WikiDislikedByUser	varchar(MAX)	;
ALTER TABLE 	WikiDiscussion	ADD 	Title	varchar(500)	;
ALTER TABLE 	WikiDiscussion	ADD 	DiscussionTitleLookup	bigint	;
ALTER TABLE 	WikiDiscussion	ADD 	Body	varchar(max)	;
ALTER TABLE 	WikiDiscussion	ADD 	DiscussionLastUpdated	datetime	;
ALTER TABLE 	WikiDiscussion	ADD 	ShortestThreadIndexIdLookup	bigint	;
ALTER TABLE 	WikiDiscussion	ADD 	WikiID	varchar(500)	;
ALTER TABLE 	WikiDiscussion	ADD 	LastReplyByUser	varchar(MAX)	;
ALTER TABLE 	WikiDiscussion	ADD 	IsQuestion	bit	;
ALTER TABLE 	WikiDiscussion	ADD 	MyEditor	varchar(max)	;
