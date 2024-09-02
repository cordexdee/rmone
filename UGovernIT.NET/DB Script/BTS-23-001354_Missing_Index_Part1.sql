/*
Alter table CRMProject 
ALTER COLUMN ContactLookup varchar(50);
GO
Alter table Lead 
ALTER COLUMN ContactLookup varchar(50);
GO
Alter table CRMServices 
ALTER COLUMN ContactLookup varchar(50);
GO
Alter table CRMCompany 
ALTER COLUMN ContactLookup varchar(50);
GO
Alter table CRMActivities 
ALTER COLUMN ContactLookup varchar(50);
GO
Alter table Opportunity 
ALTER COLUMN ContactLookup varchar(50);
GO
Alter table Opportunity 
ALTER COLUMN SalesforceLeadPriority varchar(50);
GO

*/


-- 35525396-e5fe-4692-9239-4df9305b915b
--CREATE NONCLUSTERED INDEX [IX_ResourceUsageMonthWise_Tenant_Month]
--ON [dbo].[ResourceUsageSummaryMonthWise] ([TenantID],[MonthStartDate])
--INCLUDE ([PctAllocation],[ResourceUser],[WorkItem],[WorkItemType])

GO
 
--CREATE NONCLUSTERED INDEX [IX_AspNetUsers_Role_Tenant]
--ON [dbo].[AspNetUsers] ([isRole],[TenantID])
--INCLUDE ([DepartmentLookup],[ManagerUser],[UGITStartDate],[UGITEndDate],[GlobalRoleID])
 

GO
--CREATE NONCLUSTERED INDEX [IX_ResourceUsage_Tenant_Month]
--ON [dbo].[ResourceUsageSummaryMonthWise] ([TenantID],[MonthStartDate])
--INCLUDE ([AllocationHour],[PctAllocation],[ResourceUser],[WorkItem],[WorkItemType])

GO

CREATE NONCLUSTERED INDEX [IX_CRMServices_TenantID]
ON [dbo].[CRMServices] ([TenantID])
INCLUDE (
    [EstimatedConstructionEnd],
    [EstimatedConstructionStart],
    [PreconStartDate],
    [Closed],
    [OnHold],
    [TicketId],
    [PreconEndDate],
    [CloseoutDate],
    [ERPJobIDNC],
    [CloseoutStartDate]
);

GO

CREATE NONCLUSTERED INDEX [IX_Opportunity_TenantID]
ON [dbo].[Opportunity] ([TenantID])
INCLUDE (
    [Closed],
    [EstimatedConstructionEnd],
    [EstimatedConstructionStart],
    [PreconStartDate],
    [TicketId],
    [OnHold],
    [PreconEndDate],
    [CloseoutDate],
    [ERPJobIDNC],
    [CloseoutStartDate]
);

GO

CREATE NONCLUSTERED INDEX [IX_CRMProject_TenantID]
ON [dbo].[CRMProject] ([TenantID])
INCLUDE (
    [EstimatedConstructionEnd],
    [EstimatedConstructionStart],
    [PreconStartDate],
    [Closed],
    [OnHold],
    [TicketId],
    [PreconEndDate],
    [ERPJobID],
    [CloseoutDate],
    [CloseoutStartDate]
);

GO

CREATE NONCLUSTERED INDEX [IX_ResourceAllocationMonthly_TenantID_MonthStartDate]
ON [dbo].[ResourceAllocationMonthly] ([TenantID], [MonthStartDate])
INCLUDE (
    [ResourceUser],
    [ResourceWorkItem]
);

GO

CREATE NONCLUSTERED INDEX [IX_ResourceAllocationMonthly_WorkItem_TenantID_MonthStartDate]
ON [dbo].[ResourceAllocationMonthly] ([ResourceWorkItem], [TenantID], [MonthStartDate])
INCLUDE (
    [ResourceUser]
);

GO

CREATE NONCLUSTERED INDEX [IX_Opportunity_CRMBusinessUnitChoice_TenantID]
ON [dbo].[Opportunity] ([CRMBusinessUnitChoice], [TenantID])
INCLUDE (
    [ApproxContractValue],
    [SectorChoice],
    [StudioChoice]
);

GO

CREATE NONCLUSTERED INDEX [IX_CRMProject_CRMBusinessUnitChoice_Closed_TenantID]
ON [dbo].[CRMProject] ([CRMBusinessUnitChoice], [Closed], [TenantID])
INCLUDE (
    [ApproxContractValue],
    [SectorChoice],
    [StudioChoice]
);

GO

CREATE NONCLUSTERED INDEX [IX_CRMProject_CRMBusinessUnitChoice_StageStep_TenantID]
ON [dbo].[CRMProject] ([CRMBusinessUnitChoice], [StageStep], [TenantID])
INCLUDE (
    [ApproxContractValue],
    [SectorChoice],
    [StudioChoice]
);

GO

CREATE NONCLUSTERED INDEX [IX_Opportunity_CRMBusinessUnitChoices_TenantID]
ON [dbo].[Opportunity] ([CRMBusinessUnitChoice], [TenantID])
INCLUDE (
    [TicketId],
    [SectorChoice],
    [StudioChoice]
);

GO
CREATE NONCLUSTERED INDEX [IX_ProjectEstimatedAllocation_TenantID_TicketId]
ON [dbo].[ProjectEstimatedAllocation] ([TenantID], [TicketId])
INCLUDE (
    [AssignedToUser]
);
GO
CREATE NONCLUSTERED INDEX [IX_ProjectEstimatedAllocation_TicketId_TenantID]
ON [dbo].[ProjectEstimatedAllocation] ([TicketId], [TenantID])
INCLUDE (
    [AssignedToUser]
);

GO

CREATE NONCLUSTERED INDEX [IX_ResourceUsageSummaryWeekWise_WeekStartDate_TenantID]
ON [dbo].[ResourceUsageSummaryWeekWise] ([WeekStartDate], [TenantID])
INCLUDE (
    [PctAllocation],
    [ResourceUser],
    [SubWorkItem],
    [WorkItem]
);


GO

CREATE NONCLUSTERED INDEX [IX_JobTitle_TenantID_JobType]
ON [dbo].[JobTitle] ([TenantID], [JobType])
INCLUDE (
    [ID],
    [Deleted]
);

GO

CREATE NONCLUSTERED INDEX [IX_ResourceUsageSummaryWeekWise_ResourceUser_TenantID_SoftAllocation_WeekStartDate]
ON [dbo].[ResourceUsageSummaryWeekWise] (
    [ResourceUser],
    [TenantID],
    [SoftAllocation],
    [WeekStartDate]
)
INCLUDE (
    [ActualHour],
    [AllocationHour],
    [FunctionalAreaTitleLookup],
    [ManagerName],
    [PctActual],
    [PctAllocation],
    [PctPlannedAllocation],
    [PlannedAllocationHour],
    [ResourceNameUser],
    [SubWorkItem],
    [WorkItem],
    [WorkItemID],
    [WorkItemType],
    [Title],
    [FunctionalArea],
    [ERPJobID]
);

GO
CREATE NONCLUSTERED INDEX [IX_ResourceUsageSummaryMonthWise_ResourceUser_TenantID_SoftAllocation_MonthStartDate]
ON [dbo].[ResourceUsageSummaryMonthWise] (
    [ResourceUser],
    [TenantID],
    [SoftAllocation],
    [MonthStartDate]
)
INCLUDE (
    [ActualHour],
    [AllocationHour],
    [FunctionalAreaTitleLookup],
    [ManagerName],
    [PctActual],
    [PctAllocation],
    [PctPlannedAllocation],
    [PlannedAllocationHour],
    [ResourceNameUser],
    [SubWorkItem],
    [WorkItem],
    [WorkItemID],
    [WorkItemType],
    [Title],
    [FunctionalArea],
    [ERPJobID]
);

GO

--CREATE NONCLUSTERED INDEX [IX_CRMProject_TenantID]
--ON [dbo].[CRMProject] ([TenantID])
--INCLUDE (
--    [CRMBusinessUnitChoice],
--    [TicketId],
--    [SectorChoice],
--    [StudioChoice]
--);

CREATE NONCLUSTERED INDEX [IX_CRMProject_TicketId_TenantID]
ON [dbo].[CRMProject] ([TicketId],[TenantID])

GO

CREATE NONCLUSTERED INDEX IX_ResourceUsage_TenantID ON ResourceUsageSummaryMonthWise (TenantID);
GO
-- If FunctionalAreaTitleLookup is frequently used in joins or the WHERE clause:
CREATE NONCLUSTERED INDEX IX_ResourceUsage_FunctionalAreaTitleLookup ON ResourceUsageSummaryMonthWise (FunctionalAreaTitleLookup);
GO
-- If CreatedByUser and ModifiedByUser are frequently used in joins or the WHERE clause:
CREATE NONCLUSTERED INDEX IX_ResourceUsage_CreatedByUser ON ResourceUsageSummaryMonthWise (CreatedByUser);
CREATE NONCLUSTERED INDEX IX_ResourceUsage_ModifiedByUser ON ResourceUsageSummaryMonthWise (ModifiedByUser);
GO
CREATE NONCLUSTERED INDEX IX_AspNetUsers_TenantID_Id ON AspNetUsers (TenantID, Id);
GO
CREATE NONCLUSTERED INDEX IX_AspNetUserRoles_TenantID
ON [dbo].[AspNetUserRoles] ([TenantID]);

GO

CREATE NONCLUSTERED INDEX IX_CRMProject_ProjectId_Title_TenantID
ON [dbo].[CRMProject] ([TenantID])
INCLUDE ([ProjectId],[Title],[TicketId]);

GO

CREATE NONCLUSTERED INDEX IX_Opportunity_Title_ProjectId_TenantID
ON [dbo].[Opportunity] ([TenantID])
INCLUDE ([TicketId],[Title],[ProjectId]);
GO
CREATE NONCLUSTERED INDEX IX_CRMServices_Title_ProjectId__TenantID
ON [dbo].[CRMServices] ([TenantID])
INCLUDE ([ProjectId],[Title],[TicketId]);
GO
CREATE NONCLUSTERED INDEX IX_ResourceAllocation_TenantDate
ON [dbo].[ResourceAllocation] ([TenantID], [AllocationStartDate])
INCLUDE ([TicketId]);
GO
CREATE NONCLUSTERED INDEX IX_ResourceAllocation_TenantDates
ON [dbo].[ResourceAllocation] ([TenantID], [AllocationEndDate], [AllocationStartDate])
INCLUDE ([PctAllocation], [ResourceUser], [RoleId], [TicketId]);
GO

CREATE NONCLUSTERED INDEX IX_CRMServices_Tenant
ON [dbo].[CRMServices] ([TenantID])
INCLUDE ([StageStep], [TicketId]);

GO

CREATE NONCLUSTERED INDEX IX_CRMServices_TicketTenant
ON [dbo].[CRMServices] ([TicketId], [TenantID])
INCLUDE ([StageStep]);

GO

CREATE NONCLUSTERED INDEX IX_JobTitle_DeletedJobType
ON [dbo].[JobTitle] ([Deleted], [JobType])
INCLUDE ([ID], [BillingLaborRate], [EmployeeCostRate]);
GO

CREATE NONCLUSTERED INDEX IX_CRMProject_TenantClosed
ON [dbo].[CRMProject] ([TenantID], [Closed])
INCLUDE ([ApproxContractValue], [StudioLookup]);
GO

CREATE NONCLUSTERED INDEX IX_CRMProject_EnableStdWorkItems_TenantClosed
ON [dbo].[CRMProject] ([TenantID], [Closed])
INCLUDE ([Status], [Title], [TicketId], [EnableStdWorkItems]);

GO
CREATE NONCLUSTERED INDEX IX_CRMServices_TenantStep
ON [dbo].[CRMServices] ([TenantID], [StageStep])
INCLUDE ([Status], [Closed], [Title], [TicketId]);

GO

CREATE NONCLUSTERED INDEX IX_CRMProject_TenantStep
ON [dbo].[CRMProject] ([TenantID], [StageStep])
INCLUDE ([Status], [Closed], [Title], [TicketId]);

GO

CREATE NONCLUSTERED INDEX [IX_CRMProject_Studio_Division_TenantID]
ON [dbo].[CRMProject] ([TenantID])
INCLUDE ([TicketId],[SectorChoice],[StudioLookup],[DivisionLookup]);

GO

CREATE NONCLUSTERED INDEX [IX_AspNetUsers_DepartmentLookup_JobTitleLookup]
ON [dbo].[AspNetUsers] ([Enabled],[TenantID],[JobProfile],[GlobalRoleID])
INCLUDE ([DepartmentLookup],[JobTitleLookup]);

GO

--CREATE NONCLUSTERED INDEX IX_AspNetUsers_UserProperties
--ON [dbo].[AspNetUsers] ([Enabled], [TenantID], [JobTitleLookup], [JobProfile], [GlobalRoleID])
--INCLUDE ([DepartmentLookup]);

--GO

CREATE NONCLUSTERED INDEX IX_AspNetUsers_UserProperties1
ON [dbo].[AspNetUsers] ([Enabled], [TenantID], [JobTitleLookup], [JobProfile], [GlobalRoleID])
INCLUDE ([DepartmentLookup]);
GO
CREATE NONCLUSTERED INDEX IX_AspNetUsers_UserProperties2
ON [dbo].[AspNetUsers] ([Enabled], [TenantID])
INCLUDE ([Name], [DepartmentLookup], [JobProfile], [JobTitleLookup], [GlobalRoleID]);
GO

CREATE NONCLUSTERED INDEX [IX_CNS_CRMCompanyLookup_Tenant2]
ON [dbo].[CRMServices] ([TenantID])
INCLUDE (
    [EstimatedConstructionEnd],
    [EstimatedConstructionStart],
    [PreconStartDate],
    [Closed],
    [Title],
    [TicketId],
    [CRMCompanyLookup],
    [ERPJobID],
    [PreconEndDate],
    [CloseoutDate],
    [ERPJobIDNC]
);


GO
CREATE NONCLUSTERED INDEX IX_Opportunity_Tenant2
ON [dbo].[Opportunity] ([TenantID])
INCLUDE (
    [Closed],
    [EstimatedConstructionEnd],
    [EstimatedConstructionStart],
    [PreconStartDate],
    [TicketId],
    [Title],
    [CRMCompanyLookup],
    [PreconEndDate],
    [ERPJobID],
    [CloseoutDate],
    [ERPJobIDNC]
);
GO
CREATE NONCLUSTERED INDEX IX_CRMProject_Tenant2
ON [dbo].[CRMProject] ([TenantID])
INCLUDE (
    [EstimatedConstructionEnd],
    [EstimatedConstructionStart],
    [PreconStartDate],
    [Closed],
    [Title],
    [TicketId],
    [CRMCompanyLookup],
    [PreconEndDate],
    [ERPJobID],
    [CloseoutDate],
    [ERPJobIDNC]
);

--GO



DECLARE @TableName NVARCHAR(255)
DECLARE @ColumnName NVARCHAR(255)
 
DECLARE table_cursor CURSOR FOR
SELECT t.name AS TableName, c.name AS ColumnName
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id 
where type='U' and c.name not in ('Attachments','IconBlob','SectionConditionalLogic','QuestionMapVariables','ConditionalLogic','AttachmentRequired','BaselineComment','AssignedToUser','Description','ModuleDescription',
'Comment','Remarks','BusinessManagerUser','CommercialMgtLead','Body','FormulaValue','Priority','ModuleStageConstraints','PRPUSer','ORPUser','Status',
'AdditionalComments','DecisionStatus','ProposedStatus','TaskStatus','CompletedBy','Content','Decision','AssignToPct','Behaviour','GroupUser','IssueImpact',
'ContingencyPlan','MitigationPlan','Resolution','ServiceApplicationAccessXml','definition','TaskGroup','EmailBody',
'VendorIssueImpact','VNDActionType','CustomProperties','EmailIDCC','FinancialManager','History','FunctionalManager',
'StageClosedBy','AdditionalCommentsByVendor','InitiatorUser','UserQuestionSummary','ProjectAssumptions','LessonsLearned','ProblemBeingSolved',
'ClassificationImpact','BenefitsExperienced','Job','JobId','ApprovedRFEType','Name','ITManager','Reason','CreatedAt','Data','CreatedBy','Modifiedby',
'FunctionalArea','LayoutInfo','ControlInfo','Schema','ThemeColor','VendorMeetingAgenda','MeetingMaterial','MeetingFrequencyUnit',
'ResourceUser','VendorAddress','TerminationChargesByVendor','LegalPublicity','LegalWarranties','ServiceDeliveryManager',
'OtherPaymentTerms','PaymentDelayInterest','ProjectStatus','AcceptanceCriteria','PerformanceManager','PRPGroupUser','ProjectScope','RequiredNotifications','ProjectBenefits',
'SharedServices','TaskBehaviour','ColumnValue','AssignedByUser','DepartmentManagerUser','DivisionManagerUser','ClosedByUser','BusinessManager2User','Reminders','Kyename','KeyValue','QuestionTypeProperties','QuestionType','Helptext','Counter','AggregatedCounter'
) 

--and t.name not in ('Hash','AggregatedCounter','SubContractor','UserProjectExperience','Config_Service_ServiceRelationships','Config_Services','FieldConfigurationChanges','UserProfile','Counter' ,'Organization','PLCRequest','List','Report_ConfigData','ServiceUpdates_Master','HelpCardContent','','') 
and c.name not like '%AuthorizedTo%' and c.name not like '%Choice%' and c.name not like '%Resolution%' and c.name not like '%Note%' and c.name not like '%Comment%' and c.name not like '%Custom%' and c.name not like '%url%' and c.name not like '%Description%' and c.name not like '%Details%' and c.name not like '%Information%'
--and t.name not like '%_Archive%' and t.name not like '%Analytic_%' and t.name not like 'VCC%' and t.name not like'%Job%' and t.name not like'%JobParameter%' and t.name not like'%History%'
--and t.name	not like '%ProjectSimilarityMetrics%' and t.name not like '%Config_ProjectClass%' and t.name not like'%JobQueue%'  and t.name not like '%Server%'  
--and t.name not like '%backup%' and t.name not like '%_bak%' and t.name not like '%Vendor%'

and t.name in ('ACR',
'ACRTypes',
'Agents',
'AggregatedCounter',
'ApplicationAccess',
'ApplicationModules',
'ApplicationPassword',
'ApplicationRole',
'Applications',
'ApplicationServers',
'Appointment',
'AspNetRoles',
'AspNetUserClaims',
'AspNetUserLogins',
'AspNetUserRoles',
'AspNetUsers',
'AssetIncidentRelations',
'AssetModels',
'AssetReferences',
'AssetRelations',
'Assets',
'AssetsStatus',
'AssetVendors',
'AvailableManagedServices',
'BaselineDetails',
'BTS',
'BusinessStrategy',
'BusinessUnits',
'ChartFilters',
'CheckListRoleTemplates',
'CheckLists',
'CheckListTasks',
'CheckListTaskStatus',
'CheckListTaskTemplates',
'CheckListTemplates',
'Company',
'CompanyDivisions',
'Config_BudgetCategories',
'Config_ClientAdminCategory',
'Config_ClientAdminConfigurationLists',
'Config_ConfigurationVariable',
'Config_Dashboard_DashboardFactTables',
'Config_Dashboard_DashboardPanels',
'Config_Dashboard_DashboardPanelView',
'Config_LeadCriteria',
'Config_MailTokenColumnName',
'Config_Master_RankingCriteria',
'Config_MenuNavigation',
'Config_Module_DefaultValues',
'Config_Module_EscalationRule',
'Config_Module_FormLayout',
'Config_Module_Impact',
'Config_Module_ModuleColumns',
'Config_Module_ModuleFormTab',
'Config_Module_ModuleStages',
'Config_Module_ModuleUserTypes',
'Config_Module_Priority',
'Config_Module_RequestPriority',
'Config_Module_RequestRoleWriteAccess',
'Config_Module_RequestType',
'Config_Module_Severity',
'Config_Module_SLARule',
'Config_Module_StageType',
'Config_Module_StatusMapping',
'Config_Module_TaskEmails',
'Config_ModuleLifeCycles',
'Config_ModuleMonitorOptions',
'Config_ModuleMonitors',
'Config_Modules',
'Config_PageConfiguration',
'Config_ProjectClass',
'Config_ProjectComplexity',
'Config_ProjectInitiative',
'Config_ProjectLifeCycleStages',
'Config_Service_ServiceCategories',
'Config_Service_ServiceColumns',
'Config_Service_ServiceDefaultValues',
'Config_Service_ServiceQuestions',
'Config_Service_ServiceRelationships',
'Config_Service_ServiceSections',
'Config_Services',
'Config_TabView',
'Config_UserRoles',
'Config_VendorResourceCategory',
'Config_WikiLeftNavigation',
'Contacts',
'CRMActivities',
'CRMCompany',
'CRMContact',
'CRMEstimate',
'CRMProject',
'CRMProjectAllocation',
'CRMRelationshipType',
'CRMServices',
'DashboardSummary',
'Department',
'DepartmentRoleMapping',
--'DMConfigList',
--'DMDepartmentList',
--'DMDocInfoType',
--'DMDocumentHistoryList',
--'DMDocumentInfoList',
--'DMDocumentLinkList',
'DRQ',
'DRQRapidTypes',
'DRQSystemAreas',
'EmployeeTypes',
'Environment',
'EscalationLog',
'EscalationQueue',
'ExperiencedTags',
'FieldConfiguration',
'FieldConfigurationChanges',
'FunctionalAreas',
'GenericStatus',
'GovernanceLinkCategory',
'GovernanceLinkItems',
'INC',
'ITGActual',
'ITGBudget',
'ITGMonthlyBudget',
'ITGovernance',
'JobTitle',
'LandingPages',
'Lead',
'LeadRanking',
'LinkCategory',
'LinkItems',
'LinkView',
--'List',
'Location',
'ModuleBudget',
'ModuleBudgetActuals',
'ModuleBudgetActualsHistory',
'ModuleBudgetHistory',
'ModuleMonitorOptionsHistory',
'ModuleMonthlyBudget',
'ModuleMonthlyBudgetHistory',
'ModuleStageConstraints',
'ModuleStageConstraintTemplates',
'ModuleTasks',
'ModuleUserStatistics',
'ModuleWorkflowHistory',
'MonthlyBudget',
'NPR',
'NPRBudget',
'NPRMonthlyBudget',
'NPRResources',
'NPRTasks',
'Opportunity',
--'Organization',
'PMM',
'PMMBudget',
'PMMComments',
'PMMCommentsHistory',
'PMMEvents',
--'PMMHistory',
'PMMIssues',
'PMMIssuesHistory',
'PMMMonthlyBudget',
--'PMMMonthlyBudgetHistory',
--'PmmProjectHistory',
'PMMRisks',
'PMMTasks',
'PMMTasksHistory',
'ProcoreFieldsMapping',
'ProjectAllocationTemplates',
'ProjectEstimatedAllocation',
'ProjectMonitorState',
'ProjectMonitorStateHistory',
'ProjectPlannedAllocation',
'ProjectReleases',
'ProjectSimilarityConfig',
--'ProjectSimilarityMetrics',
--'ProjectStageHistory',
'ProjectStandardWorkItems',
'ProjectSummary',
'PRS',
'RCA',
'RelatedCompanies',
'Relationship',
--'Report_ConfigData',
'RequestTypeByLocation',
'ResourceAllocation',
'ResourceAllocationMonthly',
'ResourceTimeSheet',
'ResourceTimeSheetSignOff',
'ResourceUsageSummaryMonthWise',
'ResourceUsageSummaryWeekWise',
'ResourceUtilizationSummary',
'ResourceUtilizationSummaryFooter',
'ResourceWorkItems',
'RoleBillingRateByDept',
'Roles',
'Sprint',
'SprintSummary',
'SprintTasks',
'State',
'Studio',
'SVCRequests',
'TaskTemplateItems',
'TaskTemplates',
'Templates',
'TicketCountTrends',
'TicketEvents',
'TicketHours',
'TicketRelation',
'TSK',
'TSKTasks',
'TSR',
'UserCertificates',
'UserProjectExperience',
'UserSkills'--,
--'VendorSOWFees',
--'VendorSOWInvoiceDetail',
--'VendorSOWInvoices'
)
OPEN table_cursor
FETCH NEXT FROM table_cursor INTO @TableName, @ColumnName
 
WHILE @@FETCH_STATUS = 0
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    SET @SQL = 'CREATE INDEX IX_' + @TableName + '_' + @ColumnName + ' ON ' + @TableName + ' (' + @ColumnName + ')'
	IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(@TableName) AND name = 'IX_' + @TableName + '_' + @ColumnName)
	BEGIN
    EXEC('DROP INDEX IX_' + @TableName + '_' + @ColumnName+' ON ' + @TableName)
	END
    EXEC sp_executesql @SQL
  
    FETCH NEXT FROM table_cursor INTO @TableName, @ColumnName
END
 
 
CLOSE table_cursor
DEALLOCATE table_cursor


