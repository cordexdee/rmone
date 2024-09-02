
CREATE PROCEDURE [dbo].[DeleteTenantData]
(
	@TenantID nvarchar(128)
)
AS
BEGIN

	SET NOCOUNT ON;

	IF(@TenantID = 'C345E784-AA08-420F-B11F-2753BBEBFDD5' OR @TenantID = '35525396-E5FE-4692-9239-4DF9305B915B')
	RETURN

	BEGIN TRY

	DECLARE @Success BIT = 0;

	BEGIN TRANSACTION; 

	DELETE FROM ACR WHERE TenantID = @TenantID;
	DELETE FROM DRQ WHERE TenantID = @TenantID;	
	DELETE FROM NPR WHERE TenantID = @TenantID;
	DELETE FROM Assets WHERE TenantID = @TenantID;

	DELETE FROM WikiArticles WHERE TenantID = @TenantID;

	DELETE FROM DashboardSummary WHERE RequestTypeLookup IN (SELECT ID FROM Config_Module_RequestType WHERE TenantID = @TenantID);
	DELETE FROM Config_Module_RequestType WHERE TenantID = @TenantID;
	DELETE FROM DashboardSummary WHERE TenantID = @TenantID;	

	DELETE FROM ChartTemplates WHERE TenantID = @TenantID;
	DELETE FROM __MigrationHistory WHERE TenantID = @TenantID;
	DELETE FROM DMSDocument WHERE TenantID = @TenantID;
	DELETE FROM DMSDirectory WHERE TenantID = @TenantID;
	DELETE FROM ACR WHERE TenantID = @TenantID;
	DELETE FROM ACR_Archive WHERE TenantID = @TenantID;
	DELETE FROM ACRTypes WHERE TenantID = @TenantID;
	DELETE FROM ADUserMapping WHERE TenantID = @TenantID;
	DELETE FROM AnalyticDashboards WHERE TenantID = @TenantID;
	DELETE FROM ApplicationAccess WHERE TenantID = @TenantID;
	DELETE FROM ApplicationModules WHERE TenantID = @TenantID;
	DELETE FROM ApplicationPassword WHERE TenantID = @TenantID;
	DELETE FROM ApplicationRole WHERE TenantID = @TenantID;
	DELETE FROM Applications WHERE TenantID = @TenantID;
	DELETE FROM ApplicationServers WHERE TenantID = @TenantID;
	DELETE FROM Appointment WHERE TenantID = @TenantID;
	DELETE FROM AspNetRoles WHERE TenantID = @TenantID;
	DELETE FROM AspNetUserClaims WHERE TenantID = @TenantID;
	DELETE FROM AspNetUserLogins WHERE TenantID = @TenantID;
	DELETE FROM AspNetUserRoles WHERE TenantID = @TenantID;
	DELETE FROM AspNetUsers WHERE TenantID = @TenantID;
	DELETE FROM AssetIncidentRelations WHERE TenantID = @TenantID;
	DELETE FROM AssetModels WHERE TenantID = @TenantID;
	DELETE FROM AssetReferences WHERE TenantID = @TenantID;
	DELETE FROM AssetRelations WHERE TenantID = @TenantID;
	-- DELETE FROM Assets WHERE TenantID = @TenantID;
	DELETE FROM Assets_Archive WHERE TenantID = @TenantID;
	DELETE FROM AssetsStatus WHERE TenantID = @TenantID;
	DELETE FROM AssetVendors WHERE TenantID = @TenantID;
	DELETE FROM AvailableManagedServices WHERE TenantID = @TenantID;
	DELETE FROM Bid WHERE TenantID = @TenantID;
	DELETE FROM BTS WHERE TenantID = @TenantID;
	DELETE FROM BTS_Archive WHERE TenantID = @TenantID;
	DELETE FROM BusinessStrategy WHERE TenantID = @TenantID;
	DELETE FROM ChartFilters WHERE TenantID = @TenantID;
	DELETE FROM ChartFormula WHERE TenantID = @TenantID;
	DELETE FROM Config_ClientAdminConfigurationLists WHERE TenantID = @TenantID;
	DELETE FROM Config_BudgetCategories WHERE TenantID = @TenantID;
	DELETE FROM Config_ClientAdminCategory WHERE TenantID = @TenantID;
	DELETE FROM Config_ConfigurationVariable WHERE TenantID = @TenantID;
	DELETE FROM Config_Dashboard_DashboardFactTables WHERE TenantID = @TenantID;
	DELETE FROM Config_Dashboard_DashboardPanels WHERE TenantID = @TenantID;
	DELETE FROM Config_Dashboard_DashboardPanels_Temp WHERE TenantID = @TenantID;
	DELETE FROM Config_Dashboard_DashboardPanelView WHERE TenantID = @TenantID;
	DELETE FROM Config_Dashboard_DashboardPanelView_Temp WHERE TenantID = @TenantID;
	DELETE FROM Config_EventCategories WHERE TenantID = @TenantID;
	DELETE FROM Config_MailTokenColumnName WHERE TenantID = @TenantID;
	DELETE FROM Config_MenuNavigation WHERE TenantID = @TenantID;
	DELETE FROM Config_Module_DefaultValues WHERE TenantID = @TenantID;
	DELETE FROM Config_Module_RequestPriority WHERE TenantID = @TenantID;
	DELETE FROM Config_Module_EscalationRule WHERE TenantID = @TenantID;
	DELETE FROM Config_Module_FormLayout WHERE TenantID = @TenantID;
	DELETE FROM Config_Module_ModuleColumns WHERE TenantID = @TenantID;
	DELETE FROM Config_Module_ModuleFormTab WHERE TenantID = @TenantID;
	DELETE FROM Config_Module_ModuleStages WHERE TenantID = @TenantID;
	DELETE FROM Config_Module_ModuleUserTypes WHERE TenantID = @TenantID;
	DELETE FROM Config_Module_Priority WHERE TenantID = @TenantID;
	DELETE FROM Config_Module_RequestRoleWriteAccess WHERE TenantID = @TenantID;
	DELETE FROM Config_Module_Severity WHERE TenantID = @TenantID;
	DELETE FROM Config_Module_SLARule WHERE TenantID = @TenantID;
	DELETE FROM Config_Module_StageType WHERE TenantID = @TenantID;
	DELETE FROM Config_Module_StatusMapping WHERE TenantID = @TenantID;
	DELETE FROM Config_Module_TaskEmails WHERE TenantID = @TenantID;
	DELETE FROM Config_Module_Impact WHERE TenantID = @TenantID;
	DELETE FROM Config_ModuleLifeCycles WHERE TenantID = @TenantID;
	DELETE FROM Config_ModuleMonitorOptions WHERE TenantID = @TenantID;
	DELETE FROM Config_ModuleMonitors WHERE TenantID = @TenantID;
	DELETE FROM Config_Modules WHERE TenantID = @TenantID;
	DELETE FROM Config_PageConfiguration WHERE TenantID = @TenantID;
	DELETE FROM Config_ProjectClass WHERE TenantID = @TenantID;
	DELETE FROM Config_ProjectInitiative WHERE TenantID = @TenantID;
	DELETE FROM SVCRequests WHERE TenantID = @TenantID;

	DELETE FROM Config_ProjectLifeCycleStages WHERE TenantID = @TenantID;
	
	DELETE FROM Config_Service_ServiceDefaultValues WHERE TenantID = @TenantID;
	DELETE FROM Config_Service_ServiceQuestions WHERE TenantID = @TenantID;
	DELETE FROM Config_Service_ServiceSections WHERE TenantID = @TenantID;
	-- DELETE FROM ModuleTasks WHERE TenantID = @TenantID;
		
	--DELETE FROM Config_Service_ServiceDefaultValues WHERE TenantID = @TenantID;
	DELETE FROM ModuleTasks WHERE TenantID = @TenantID;

	DELETE FROM Config_Services WHERE TenantID = @TenantID;
	DELETE FROM Config_Service_ServiceCategories WHERE TenantID = @TenantID;

	-- DELETE FROM Config_Service_ServiceQuestions WHERE TenantID = @TenantID;
	-- DELETE FROM Config_Service_ServiceDefaultValues WHERE TenantID = @TenantID;

	DELETE FROM Config_Service_ServiceColumns WHERE TenantID = @TenantID;
	DELETE FROM Config_Service_ServiceRelationships WHERE TenantID = @TenantID;

	DELETE FROM Config_TabView WHERE TenantID = @TenantID;
	DELETE FROM Config_UserRoles WHERE TenantID = @TenantID;
	DELETE FROM Config_VendorResourceCategory WHERE TenantID = @TenantID;
	DELETE FROM Config_WikiLeftNavigation WHERE TenantID = @TenantID;
	DELETE FROM Contacts WHERE TenantID = @TenantID;
	DELETE FROM Contracts WHERE TenantID = @TenantID;
	DELETE FROM Customers WHERE TenantID = @TenantID;
	-- DELETE FROM DashboardSummary WHERE TenantID = @TenantID;
	DELETE FROM DecisionLog WHERE TenantID = @TenantID;
	DELETE FROM DMConfigList WHERE TenantID = @TenantID;
	DELETE FROM DMDepartmentList WHERE TenantID = @TenantID;
	DELETE FROM DMDocInfoType WHERE TenantID = @TenantID;
	DELETE FROM DMDocumentHistoryList WHERE TenantID = @TenantID;
	DELETE FROM DMDocumentInfoList WHERE TenantID = @TenantID;
	DELETE FROM DMDocumentLinkList WHERE TenantID = @TenantID;
	DELETE FROM DMDocumentTypeList WHERE TenantID = @TenantID;
	DELETE FROM DMDocumentWorkflowHistory WHERE TenantID = @TenantID;
	DELETE FROM DMPingInformation WHERE TenantID = @TenantID;
	DELETE FROM DMPortalInfo WHERE TenantID = @TenantID;
	DELETE FROM DMProjectList WHERE TenantID = @TenantID;
	DELETE FROM DMSTenant_Documents_Details WHERE TenantID = @TenantID;
	DELETE FROM DMTagList WHERE TenantID = @TenantID;
	DELETE FROM DMVendorList WHERE TenantID = @TenantID;
	DELETE FROM DMWorkflowTask WHERE TenantID = @TenantID;
	DELETE FROM Documents WHERE TenantID = @TenantID;
	-- DELETE FROM DRQ WHERE TenantID = @TenantID;
	DELETE FROM DRQ_Archive WHERE TenantID = @TenantID;
	DELETE FROM DRQRapidTypes WHERE TenantID = @TenantID;
	DELETE FROM DRQSystemAreas WHERE TenantID = @TenantID;
	DELETE FROM EmailFooter WHERE TenantID = @TenantID;
	DELETE FROM EmailQueue WHERE TenantID = @TenantID;
	DELETE FROM Emails WHERE TenantID = @TenantID;
	DELETE FROM Environment WHERE TenantID = @TenantID;
	DELETE FROM EscalationLog WHERE TenantID = @TenantID;
	DELETE FROM EscalationQueue WHERE TenantID = @TenantID;
	DELETE FROM FieldConfiguration WHERE TenantID = @TenantID;
	DELETE FROM FunctionalAreas WHERE TenantID = @TenantID;
	DELETE FROM Department WHERE TenantID = @TenantID;
	DELETE FROM Company WHERE TenantID = @TenantID;
	DELETE FROM CompanyDivisions WHERE TenantID = @TenantID;

	DELETE FROM GenericStatus WHERE TenantID = @TenantID;
	DELETE FROM GovernanceLinkItems WHERE TenantID = @TenantID;
	DELETE FROM GovernanceLinkCategory WHERE TenantID = @TenantID;

	DELETE FROM HolidaysAndWorkDaysCalendar WHERE TenantID = @TenantID;
	DELETE FROM ImageSoftware WHERE TenantID = @TenantID;
	DELETE FROM ImageSoftwareMap WHERE TenantID = @TenantID;
	DELETE FROM INC WHERE TenantID = @TenantID;
	DELETE FROM INC_Archive WHERE TenantID = @TenantID;
	DELETE FROM InvDistribution WHERE TenantID = @TenantID;
	DELETE FROM Investments WHERE TenantID = @TenantID;
	DELETE FROM Investors WHERE TenantID = @TenantID;
	DELETE FROM ITGActual WHERE TenantID = @TenantID;
	DELETE FROM ITGBudget WHERE TenantID = @TenantID;
	DELETE FROM ITGMonthlyBudget WHERE TenantID = @TenantID;
	DELETE FROM ITGovernance WHERE TenantID = @TenantID;
	DELETE FROM JobTitle WHERE TenantID = @TenantID;
	DELETE FROM LandingPages WHERE TenantID = @TenantID;
	DELETE FROM LinkItems WHERE TenantID = @TenantID;
	DELETE FROM LinkCategory WHERE TenantID = @TenantID;
	DELETE FROM LinkView WHERE TenantID = @TenantID;
	DELETE FROM Location WHERE TenantID = @TenantID;
	DELETE FROM Log WHERE TenantID = @TenantID;
	DELETE FROM MessageBoard WHERE TenantID = @TenantID;
	DELETE FROM ModuleBudget WHERE TenantID = @TenantID;
	DELETE FROM ModuleBudgetActuals WHERE TenantID = @TenantID;
	DELETE FROM ModuleBudgetHistory WHERE TenantID = @TenantID;
	DELETE FROM ModuleMonthlyBudget WHERE TenantID = @TenantID;
	DELETE FROM ModuleStageConstraints WHERE TenantID = @TenantID;
	DELETE FROM ModuleStageConstraintTemplates WHERE TenantID = @TenantID;

	DELETE FROM ModuleUserStatistics WHERE TenantID = @TenantID;
	DELETE FROM ModuleWorkflowHistory WHERE TenantID = @TenantID;
	DELETE FROM ModuleWorkflowHistory_Archive WHERE TenantID = @TenantID;
	DELETE FROM MonthlyBudget WHERE TenantID = @TenantID;
	-- DELETE FROM NPR WHERE TenantID = @TenantID;
	DELETE FROM NPR_Archive WHERE TenantID = @TenantID;
	DELETE FROM NPRBudget WHERE TenantID = @TenantID;
	DELETE FROM NPRMonthlyBudget WHERE TenantID = @TenantID;
	DELETE FROM NPRResources WHERE TenantID = @TenantID;
	DELETE FROM NPRTasks WHERE TenantID = @TenantID;
	DELETE FROM Opportunity WHERE TenantID = @TenantID;
	DELETE FROM Organization WHERE TenantID = @TenantID;
	DELETE FROM Phrase WHERE TenantID = @TenantID;
	DELETE FROM PLCRequest WHERE TenantID = @TenantID;
	DELETE FROM PMM WHERE TenantID = @TenantID;
	DELETE FROM PMM_Archive WHERE TenantID = @TenantID;
	DELETE FROM PMMBaselineDetail WHERE TenantID = @TenantID;
	DELETE FROM PMMBudget WHERE TenantID = @TenantID;
	DELETE FROM PMMComments WHERE TenantID = @TenantID;
	DELETE FROM PMMCommentsHistory WHERE TenantID = @TenantID;
	DELETE FROM PMMEvents WHERE TenantID = @TenantID;
	DELETE FROM PMMIssues WHERE TenantID = @TenantID;
	DELETE FROM PMMIssuesHistory WHERE TenantID = @TenantID;
	DELETE FROM PMMMonthlyBudget WHERE TenantID = @TenantID;
	DELETE FROM PMMMonthlyBudgetHistory WHERE TenantID = @TenantID;
	DELETE FROM PMMRisks WHERE TenantID = @TenantID;
	DELETE FROM PMMTasks WHERE TenantID = @TenantID;
	DELETE FROM PMMTasksHistory WHERE TenantID = @TenantID;
	DELETE FROM ProjectMonitorState WHERE TenantID = @TenantID;
	DELETE FROM ProjectMonitorStateHistory WHERE TenantID = @TenantID;
	DELETE FROM ProjectReleases WHERE TenantID = @TenantID;
	DELETE FROM ProjectSimilarityConfig WHERE TenantID = @TenantID;
	DELETE FROM ProjectStageHistory WHERE TenantID = @TenantID;
	DELETE FROM ProjectSummary WHERE TenantID = @TenantID;
	DELETE FROM PRS WHERE TenantID = @TenantID;
	DELETE FROM PRS_Archive WHERE TenantID = @TenantID;
	DELETE FROM RCA WHERE TenantID = @TenantID;
	DELETE FROM RCA_Archive WHERE TenantID = @TenantID;
	DELETE FROM Relationship WHERE TenantID = @TenantID;
	DELETE FROM ReportComponents WHERE TenantID = @TenantID;
	DELETE FROM ReportDefinition WHERE TenantID = @TenantID;
	DELETE FROM RequestTypeByLocation WHERE TenantID = @TenantID;
	DELETE FROM ResourceAllocation WHERE TenantID = @TenantID;
	DELETE FROM ResourceAllocationMonthly WHERE TenantID = @TenantID;
	DELETE FROM Resources WHERE TenantID = @TenantID;
	DELETE FROM Resources_1 WHERE TenantID = @TenantID;
	DELETE FROM ResourceTimeSheet WHERE TenantID = @TenantID;
	DELETE FROM ResourceUsageSummaryMonthWise WHERE TenantID = @TenantID;
	DELETE FROM ResourceUsageSummaryWeekWise WHERE TenantID = @TenantID;
	DELETE FROM ResourceWorkItems WHERE TenantID = @TenantID;
	DELETE FROM Roles WHERE TenantID = @TenantID;
	DELETE FROM SchedulerActionArchives WHERE TenantID = @TenantID;
	DELETE FROM SchedulerActions WHERE TenantID = @TenantID;
	DELETE FROM Sprint WHERE TenantID = @TenantID;
	DELETE FROM SprintSummary WHERE TenantID = @TenantID;
	DELETE FROM SprintTasks WHERE TenantID = @TenantID;
	DELETE FROM SubLocation WHERE TenantID = @TenantID;
	DELETE FROM SurveyFeedback WHERE TenantID = @TenantID;

	DELETE FROM TaskTemplateItems WHERE TenantID = @TenantID;
	DELETE FROM TaskTemplates WHERE TenantID = @TenantID;
	DELETE FROM Templates WHERE TenantID = @TenantID;
	DELETE FROM TenantScheduler WHERE TenantID = @TenantID;
	DELETE FROM TicketHours WHERE TenantID = @TenantID;
	DELETE FROM TicketRelation WHERE TenantID = @TenantID;
	DELETE FROM TSK WHERE TenantID = @TenantID;
	DELETE FROM TSK_Archive WHERE TenantID = @TenantID;
	DELETE FROM TSKTasks WHERE TenantID = @TenantID;
	DELETE FROM TSR WHERE TenantID = @TenantID;
	DELETE FROM TSR_Archive WHERE TenantID = @TenantID;
	DELETE FROM UPR WHERE TenantID = @TenantID;
	DELETE FROM UPR_Archive WHERE TenantID = @TenantID;
	DELETE FROM UserProfile WHERE TenantID = @TenantID;
	DELETE FROM UserSkills WHERE TenantID = @TenantID;
	DELETE FROM VCCRequest WHERE TenantID = @TenantID;
	DELETE FROM VendorApprovedSubcontractors WHERE TenantID = @TenantID;
	DELETE FROM VendorIssues WHERE TenantID = @TenantID;
	DELETE FROM VendorKeyPersonnel WHERE TenantID = @TenantID;
	DELETE FROM VendorMSA WHERE TenantID = @TenantID;
	DELETE FROM VendorMSAMeeting WHERE TenantID = @TenantID;
	DELETE FROM VendorPO WHERE TenantID = @TenantID;
	DELETE FROM VendorPOLineItems WHERE TenantID = @TenantID;
	DELETE FROM VendorReport WHERE TenantID = @TenantID;
	DELETE FROM VendorReportInstance WHERE TenantID = @TenantID;
	DELETE FROM VendorRisks WHERE TenantID = @TenantID;
	DELETE FROM Vendors WHERE TenantID = @TenantID;
	DELETE FROM VendorServiceDuration WHERE TenantID = @TenantID;
	DELETE FROM VendorSLA WHERE TenantID = @TenantID;
	DELETE FROM VendorSLAPerformance WHERE TenantID = @TenantID;
	DELETE FROM VendorSOW WHERE TenantID = @TenantID;
	DELETE FROM VendorSOWContImprovement WHERE TenantID = @TenantID;
	DELETE FROM VendorSOWFees WHERE TenantID = @TenantID;
	DELETE FROM VendorSOWInvoiceDetail WHERE TenantID = @TenantID;
	DELETE FROM VendorSOWInvoices WHERE TenantID = @TenantID;
	DELETE FROM VendorType WHERE TenantID = @TenantID;
	DELETE FROM VendorVPM WHERE TenantID = @TenantID;
	-- DELETE FROM WikiArticles WHERE TenantID = @TenantID;
	DELETE FROM WikiContents WHERE TenantID = @TenantID;
	DELETE FROM WikiDiscussion WHERE TenantID = @TenantID;
	DELETE FROM WikiLinks WHERE TenantID = @TenantID;
	DELETE FROM WikiMenuLeftNavigation WHERE TenantID = @TenantID;
	DELETE FROM WikiReview WHERE TenantID = @TenantID;
	DELETE FROM WikiReviews WHERE TenantID = @TenantID;
	DELETE FROM WorkflowSLASummary WHERE TenantID = @TenantID;	

	DELETE FROM Agents WHERE TenantID = @TenantID;
	DELETE FROM Config_ProjectComplexity WHERE TenantID = @TenantID;
	DELETE FROM EmployeeTypes WHERE TenantID = @TenantID;
	DELETE FROM HelpCard WHERE TenantID = @TenantID;
	DELETE FROM HelpCardContent WHERE TenantID = @TenantID;
	DELETE FROM ProjectStandardWorkItems WHERE TenantID = @TenantID;

	DELETE FROM [State] WHERE TenantID = @TenantID;

	DELETE FROM [CheckListRoleTemplates] WHERE TenantID = @TenantID;
	DELETE FROM [CheckListTaskTemplates] WHERE TenantID = @TenantID;
	DELETE FROM [CheckListTemplates] WHERE TenantID = @TenantID;
	DELETE FROM [Config_LeadCriteria] WHERE TenantID = @TenantID;
	DELETE FROM [Config_Master_RankingCriteria] WHERE TenantID = @TenantID;

	-- PRINT 'Data deleted for tenant: ' + @TenantID

	COMMIT TRANSACTION

	SET @Success = 1;

	END TRY
	BEGIN CATCH
		
		ROLLBACK TRANSACTION
		PRINT 'ERROR: data deletion for tenant: ' + @TenantID
		PRINT ERROR_MESSAGE()
	END CATCH

END
