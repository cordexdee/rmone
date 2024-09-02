using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.IO;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Text.RegularExpressions;
using DevExpress.Web;
using DevExpress.Web.Rendering;
using uGovernIT.Manager;
using System.Collections.Specialized;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
using uGovernIT.Web.ControlTemplates.GlobalPage;
using uGovernIT.Web.ControlTemplates.Admin.ListForm;
using uGovernIT.Web.ControlTemplates.PMM;

namespace uGovernIT.Web
{
    public partial class uGovernITConfiguration : UPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
        }
        protected override void OnInit(EventArgs e)
        {

            string control = Request["control"];
            if (!string.IsNullOrEmpty(control) && control.Trim() != string.Empty)
            {
                switch (control.ToLower())
                {
                    case "rolesview":
                        GlobalRolesView _rolesView = Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/GlobalRolesView.ascx") as GlobalRolesView;
                        contentPanel.Controls.Add(_rolesView);
                        break;
                    case "configvar":
                        ConfigurationVariableListView viewconfig = (ConfigurationVariableListView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ConfigurationVariableListView.ascx");
                        contentPanel.Controls.Add(viewconfig);
                        break;
                    //case "configlicense":
                    //ConfigLicense licenseInfo = (ConfigLicense)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/ConfigLicense.ascx");
                    // contentPanel.Controls.Add(licenseInfo);
                    //  break;
                    case "configcache":
                        ConfigCache cacheConfig = (ConfigCache)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ConfigCache.ascx");
                        contentPanel.Controls.Add(cacheConfig);
                        break;
                    //            case "configlists":
                    //                ConfigLists listsConfig = (ConfigLists)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/ConfigLists.ascx");
                    //                contentPanel.Controls.Add(listsConfig);
                    //                break;
                    case "configdashboard":
                        ConfigDashboard dashboardConfig1 = (ConfigDashboard)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ConfigDashboard.ascx");
                        contentPanel.Controls.Add(dashboardConfig1);
                        break;
                    //            case "configdashboardfilter":
                    //                DashboardFilter filterConfig1 = (DashboardFilter)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/DashboardFilter.ascx");
                    //                contentPanel.Controls.Add(filterConfig1);
                    //                break;
                    case "configdashboardpanel":
                        DashboardKPI panelConfig1 = (DashboardKPI)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/DashboardKPI.ascx");
                        contentPanel.Controls.Add(panelConfig1);
                        break;
                    case "configdashboardchart":
                        NewDashboardChartUI newdashboardchartui = (NewDashboardChartUI)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/NewDashboardChartUI.ascx");
                        contentPanel.Controls.Add(newdashboardchartui);
                        break;
                    case "configdashboardfacttable":
                        DashboardFactTablesView dashboardFactTable = (DashboardFactTablesView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/DashboardFactTablesView.ascx");
                        contentPanel.Controls.Add(dashboardFactTable);
                        break;
                    case "configadddashboardfacttable":
                        AddDashboardFactTable addDashboardFactTable = (AddDashboardFactTable)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/AddDashboardFactTable.ascx");
                        contentPanel.Controls.Add(addDashboardFactTable);
                        break;
                    case "configeditdashboardfacttable":
                        EditViewDashboardFactTable editDashboardFactTable = (EditViewDashboardFactTable)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/EditViewDashboardFactTable.ascx");
                        contentPanel.Controls.Add(editDashboardFactTable);
                        break;
                    case "services":
                        ServicesList services = (ServicesList)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Services/ServicesList.ascx");
                        contentPanel.Controls.Add(services);
                        break;
                    case "configdashboardquery":
                        QueryWizard dashboardQuery = (QueryWizard)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/QueryWizard.ascx");
                        contentPanel.Controls.Add(dashboardQuery);
                        break;
                    case "querywizardpreview":
                        int queryId = 0;
                        int.TryParse(Request["ItemId"], out queryId);
                        QueryWizardPreview queryWizardPreview = (QueryWizardPreview)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/QueryWizardPreview.ascx");
                        queryWizardPreview.Id = queryId;
                        contentPanel.Controls.Add(queryWizardPreview);
                        break;
                    case "serviceswizard":
                        ServicesWizard servicesWizard = (ServicesWizard)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Services/ServicesWizard.ascx");
                        contentPanel.Controls.Add(servicesWizard);
                        contentPanel.CssClass = "isSvcPopup_container";
                        break;
                    case "newprojectwizard":
                        AddNewProject newprojectwizard = (AddNewProject)Page.LoadControl("~/CONTROLTEMPLATES/PMM/AddNewProject.ascx");
                        contentPanel.Controls.Add(newprojectwizard);
                        break;

                    case "scheduleactionslist":
                        ScheduleActionsView scheduleActionView = (ScheduleActionsView)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ScheduleActionsView.ascx");
                        contentPanel.Controls.Add(scheduleActionView);
                        break;
                    case "scheduleaction":
                        int Id = 0;
                        int.TryParse(Request["Id"], out Id);
                        ScheduleActionControl scheduleAction = (ScheduleActionControl)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ScheduleActionControl.ascx");
                        string actionType = Request["ActionType"];
                        if (actionType == "Report")
                        {
                            Dictionary<string, string> parameter = new Dictionary<string, string>();
                            foreach (var key in Request.QueryString.AllKeys)
                            {
                                if (key != "Id" && key != "control")
                                {
                                    parameter.Add(key, Request[key]);
                                }
                            }
                            scheduleAction.Parameter = parameter;
                        }
                        else
                        {
                            scheduleAction.DashboardPanelID = Id;
                        }
                        contentPanel.Controls.Add(scheduleAction);
                        break;

                    //            case "scheduleaction":
                    //                int Id = 0;
                    //                int.TryParse(Request["Id"], out Id);
                    //                ScheduleActionControl scheduleAction = (ScheduleActionControl)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/ScheduleActionControl.ascx");
                    //                string actionType = Request["ActionType"];
                    //                if (actionType == "Report")
                    //                {
                    //                    Dictionary<string, string> parameter = new Dictionary<string, string>();
                    //                    foreach (var key in Request.QueryString.AllKeys)
                    //                    {
                    //                        if (key != "Id" && key != "control")
                    //                        {
                    //                            parameter.Add(key, Request[key]);
                    //                        }
                    //                    }
                    //                    scheduleAction.Parameter = parameter;
                    //                }
                    //                else
                    //                {
                    //                    scheduleAction.DashboardPanelId = Id;
                    //                }
                    //                contentPanel.Controls.Add(scheduleAction);
                    //                break;
                    //            case "configdashboardquerydispaly":
                    //                DashboardQueryDisplay dashboardQueryDisplay = (DashboardQueryDisplay)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/DashboardQueryDisplay.ascx");
                    //                contentPanel.Controls.Add(dashboardQueryDisplay);
                    //                break;
                    case "configdashboardeditviews":
                        int viewID = 0;
                        int.TryParse(Request["viewID"], out viewID);
                        string viewType = Request["viewType"];
                        DashboardPanelView view = null;
                        DashboardPanelViewManager objDashboardPanelViewManager = new DashboardPanelViewManager(HttpContext.Current.GetManagerContext());
                        if (viewID > 0)
                        {
                            view = objDashboardPanelViewManager.LoadViewByID(viewID);
                            if (view != null)
                            {
                                viewType = view.ViewType;
                            }
                        }
                        if (viewType != null)
                        {
                            switch (viewType.ToLower())
                            {
                                case "indivisible dashboards":
                                    DashboardIndivisibleEditView dashboardEditView1 = (DashboardIndivisibleEditView)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/DashboardIndivisibleEditView.ascx");
                                    dashboardEditView1.ViewType = viewType;
                                    dashboardEditView1.View = view;
                                    contentPanel.Controls.Add(dashboardEditView1);
                                    break;
                                case "super dashboards":
                                    DashboardSuperEditView dashboardEditView2 = (DashboardSuperEditView)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/DashboardSuperEditView.ascx");
                                    dashboardEditView2.ViewType = viewType;
                                    dashboardEditView2.View = view;
                                    contentPanel.Controls.Add(dashboardEditView2);
                                    break;
                                case "side dashboards":
                                    DashboardSideEditView dashboardEditView3 = (DashboardSideEditView)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/DashboardSideEditView.ascx");
                                    dashboardEditView3.ViewType = viewType;
                                    dashboardEditView3.View = view;
                                    contentPanel.Controls.Add(dashboardEditView3);
                                    break;
                            }
                        }

                        break;
                    case "configdashboardviews":
                        DashboardViews dashboardView = (DashboardViews)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/DashboardViews.ascx");
                        contentPanel.Controls.Add(dashboardView);
                        break;
                    case "requesttype":
                        RequestTypeListView requestType = (RequestTypeListView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/RequestTypeListView.ascx");
                        contentPanel.Controls.Add(requestType);
                        break;
                    case "priortymapping":
                        PriorityMappingView priortyview = (PriorityMappingView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/PriorityMappingView.ascx");
                        contentPanel.Controls.Add(priortyview);
                        break;
                    case "slarule":
                        SLARulesView slaRulesView = (SLARulesView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/SLARulesView.ascx");
                        contentPanel.Controls.Add(slaRulesView);
                        break;
                    case "escalationrule":
                        SLAEscalationView _SLAEscalationView = (SLAEscalationView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/SLAEscalationView.ascx");
                        contentPanel.Controls.Add(_SLAEscalationView);
                        break;
                    case "emailnotification":
                        EmailNotificationView emailNotificationView = (EmailNotificationView)Page.LoadControl("~/CONTROLTEMPLATES/admin/ListForm/EmailNotificationView.ascx");
                        contentPanel.Controls.Add(emailNotificationView);
                        break;
                    case "ticketimpact":
                        string mode = Request["mode"];
                        ImpactView impactView = (ImpactView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ImpactView.ascx");
                        impactView.Mode = (ViewMode)Enum.Parse(typeof(ViewMode), mode, true);
                        contentPanel.Controls.Add(impactView);
                        break;
                    case "bgetcatview":
                        BudgetCategoriesView _BudgetCategoriesView = (BudgetCategoriesView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/BudgetCategoriesView.ascx");
                        contentPanel.Controls.Add(_BudgetCategoriesView);
                        break;
                    case "projectclass":
                        ProjectClassView _ProjectClassView = (ProjectClassView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ProjectClassView.ascx");
                        contentPanel.Controls.Add(_ProjectClassView);
                        break;
                    case "projectinit":
                        ProjectInitView _ProjectInitView = (ProjectInitView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ProjectInitView.ascx");
                        contentPanel.Controls.Add(_ProjectInitView);
                        break;
                    case "acrtypes":
                        ACRTypeView _ACRTypeView = (ACRTypeView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ACRTypeView.ascx");
                        contentPanel.Controls.Add(_ACRTypeView);
                        break;
                    case "drqsystemarea":
                        DRQSystemAreaView _DRQSystemAreaView = (DRQSystemAreaView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/DRQSystemAreaView.ascx");
                        contentPanel.Controls.Add(_DRQSystemAreaView);
                        break;
                    case "drqrapidtypes":
                        DRQRapidTypesView _drqrapidtypesview = (DRQRapidTypesView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/DRQRapidTypesView.ascx");
                        contentPanel.Controls.Add(_drqrapidtypesview);
                        break;
                    case "userroletypes":
                        UserRoleTypeView _UserRoleTypeView = (UserRoleTypeView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/UserRoleTypeView.ascx");
                        contentPanel.Controls.Add(_UserRoleTypeView);
                        break;
                    case "deptview":
                        DepartmentView _DepartmentView = (DepartmentView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/DepartmentView.ascx");
                        contentPanel.Controls.Add(_DepartmentView);
                        break;
                    case "funcareaview":
                        FunctionalAreaView _FunctionalAreaView = (FunctionalAreaView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/FunctionalAreaView.ascx");
                        contentPanel.Controls.Add(_FunctionalAreaView);
                        break;
                    case "locationview":
                        LocationView _LocationView = (LocationView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/LocationView.ascx");
                        contentPanel.Controls.Add(_LocationView);
                        break;
                    case "sublocationview":
                        SubLocationView sublocationview = (SubLocationView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/SubLocationView.ascx");
                        contentPanel.Controls.Add(sublocationview);
                        break;
                    case "assetvendorview":
                        AssetVendorsView _AssetVendorsView = (AssetVendorsView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/AssetVendorsView.ascx");
                        contentPanel.Controls.Add(_AssetVendorsView);
                        break;

                    case "assetmodelview":
                        AssetModelsView _AssetModelsView = (AssetModelsView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/AssetModelsView.ascx");
                        contentPanel.Controls.Add(_AssetModelsView);
                        break;
                    case "moduleview":
                        ModuleView _ModuleView = (ModuleView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ModuleView.ascx");
                        contentPanel.Controls.Add(_ModuleView);
                        break;
                    case "modulestagesview":
                        ModuleStagesView _ModuleStagesView = (ModuleStagesView)Page.LoadControl("/ControlTemplates/Admin/ListForm/ModuleStagesView.ascx");
                        // bool testFalg = true;
                        _ModuleStagesView.fromAdmin = true;





                        contentPanel.Controls.Add(_ModuleStagesView);
                        break;

                    case "modulecolumnsview":
                        ModuleColumnsView _ModuleColumnsView = (ModuleColumnsView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ModuleColumnsView.ascx");
                        contentPanel.Controls.Add(_ModuleColumnsView);
                        break;
                    case "formlayoutandpermission":
                        FormLayoutAndPermission _FormLayoutAndPermission = (FormLayoutAndPermission)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/FormLayoutAndPermission.ascx");
                        contentPanel.Controls.Add(_FormLayoutAndPermission);
                        break;
                    case "requestrolewriteaccess":
                        RequestRoleWriteAccess _RequestRoleWriteAccess = (RequestRoleWriteAccess)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/RequestRoleWriteAccess.ascx");
                        int rwaID;
                        int.TryParse(Request["ID"], out rwaID);
                        _RequestRoleWriteAccess.ItemID = rwaID;
                        _RequestRoleWriteAccess.Module = Convert.ToString(Request["module"]);
                        contentPanel.Controls.Add(_RequestRoleWriteAccess);
                        break;
                    case "modulestageaddedit":
                        ModuleStageEdit _ModuleStageEdit = (ModuleStageEdit)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ModuleStageEdit.ascx");
                        contentPanel.Controls.Add(_ModuleStageEdit);
                        break;



                    case "formlayoutandpermissionaddedit":
                        FormLayoutAndPermissionAddEdit _FormLayoutAndPermissionAddEdit = (FormLayoutAndPermissionAddEdit)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/FormLayoutAndPermissionAddEdit.ascx");
                        int flID;
                        int.TryParse(Request["ID"], out flID);
                        _FormLayoutAndPermissionAddEdit.ItemID = flID;
                        _FormLayoutAndPermissionAddEdit.Module = Convert.ToString(Request["module"]);
                        _FormLayoutAndPermissionAddEdit.CurrentTabIndex = Request["currentTabIndex"] != null ? Convert.ToInt16(Request["currentTabIndex"]) : -1;
                        contentPanel.Controls.Add(_FormLayoutAndPermissionAddEdit);

                        break;

                    case "environmentview":
                        EnvironmentView _EnvironmentView = (EnvironmentView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/EnvironmentView.ascx");
                        contentPanel.Controls.Add(_EnvironmentView);
                        break;
                    case "governanceconfig":
                        GovernanceConfiguratorView govConfig = (GovernanceConfiguratorView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/GovernanceConfiguratorView.ascx");
                        govConfig.categoryTypeParam = Request["param"];
                        contentPanel.Controls.Add(govConfig);
                        break;
                    //            case "governancecategoriesview":
                    //                GovernanceCategoryView govCategoriesView = (GovernanceCategoryView)Page.LoadControl("~/_CONTROLTEMPLATES/15/ListForm/GovernanceCategoryView.ascx");
                    //                govCategoriesView.categoryTypeParam = Request["param"];
                    //                contentPanel.Controls.Add(govCategoriesView);
                    //                break;
                    //            case "newsurveywizard":
                    //                NewSurveyWizard newSurveyWizard = (NewSurveyWizard)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/NewSurveyWizard.ascx");
                    //                int serviceId;
                    //                int.TryParse(Request["serviceid"], out serviceId);
                    //                newSurveyWizard.ServiceId = serviceId;
                    //                contentPanel.Controls.Add(newSurveyWizard);
                    //                break;
                    //            case "surveyformslink":
                    //                SurveyFormList surveyList = (SurveyFormList)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/SurveyFormList.ascx");
                    //                contentPanel.Controls.Add(surveyList);
                    //                break;
                    case "surveyfeedback":
                        SurveyFeedbackList sFeedback = (SurveyFeedbackList)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Services/SurveyFeedbackList.ascx");
                        contentPanel.Controls.Add(sFeedback);
                        break;
                    //            case "analyticauth":
                    //                AnalyticAuth aAuth = (AnalyticAuth)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/AnalyticAuth.ascx");
                    //                contentPanel.Controls.Add(aAuth);
                    //                break;
                    case "projectlifecycles":
                        LifeCycleView lcView = (LifeCycleView)Page.LoadControl("~/CONTROLTEMPLATES/admin/ListForm/LifeCycleView.ascx");
                        contentPanel.Controls.Add(lcView);
                        break;
                    case "lifecycleedit":
                        LifeCycleEdit lcViewEdit = (LifeCycleEdit)Page.LoadControl("~/CONTROLTEMPLATES/admin/ListForm/LifeCycleEdit.ascx");
                        contentPanel.Controls.Add(lcViewEdit);
                        break;
                    case "lifecyclenew":
                        LifeCycleNew lcViewNew = (LifeCycleNew)Page.LoadControl("~/CONTROLTEMPLATES/admin/ListForm/LifeCycleNew.ascx");
                        contentPanel.Controls.Add(lcViewNew);
                        break;
                    case "lifecyclestageedit":
                        LifeCycleStageEdit lcStageViewEdit = (LifeCycleStageEdit)Page.LoadControl("~/CONTROLTEMPLATES/admin/ListForm/LifeCycleStageEdit.ascx");
                        contentPanel.Controls.Add(lcStageViewEdit);
                        break;
                    case "lifecyclestagenew":
                        LifeCycleStageNew lcStageViewNew = (LifeCycleStageNew)Page.LoadControl("~/CONTROLTEMPLATES/admin/ListForm/LifeCycleStageNew.ascx");
                        contentPanel.Controls.Add(lcStageViewNew);
                        break;

                    case "messageboard":
                        MessageBoardView msgBoardView = (MessageBoardView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/MessageBoardView.ascx");
                        contentPanel.Controls.Add(msgBoardView);
                        break;
                    case "modulestagetemplates":
                        ModuleStageTaskTemplateList templateList = (ModuleStageTaskTemplateList)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ModuleStageTaskTemplateList.ascx");
                        contentPanel.Controls.Add(templateList);
                        break;
                    case "modulestagetasktemplate":
                        ModuleStageTaskTemplateEdit taskTemplate = (ModuleStageTaskTemplateEdit)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ModuleStageTaskTemplateEdit.ascx");
                        taskTemplate.TemplateId = Request["ItemID"] != null ? Convert.ToInt32(Request["ItemID"].Trim()) : 0;
                        contentPanel.Controls.Add(taskTemplate);
                        break;
                    case "modulestagerule":
                        ModuleStagesRule moduleRule = (ModuleStagesRule)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ModuleStagesRule.ascx");
                        moduleRule.StageId = (Request["stageId"] == null ? string.Empty : Convert.ToString(Request["stageId"]));
                        moduleRule.ModuleName = (Request["moduleName"] == null ? string.Empty : Convert.ToString(Request["moduleName"]));
                        moduleRule.SkipCondition = (Request["SkipCondition"] == null ? string.Empty : Convert.ToString(Request["SkipCondition"]));
                        moduleRule.ControlId = (Request["controlId"] == null ? string.Empty : Convert.ToString(Request["controlId"]));
                        contentPanel.Controls.Add(moduleRule);
                        break;
                    case "companynew":
                        CompanyNew cmpNew = (CompanyNew)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/CompanyNew.ascx");
                        contentPanel.Controls.Add(cmpNew);
                        break;
                    case "companyedit":
                        {
                            CompanyEdit cmpEdit = (CompanyEdit)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/CompanyEdit.ascx");
                            int id = 0;
                            int.TryParse(Request["ItemID"], out id);
                            cmpEdit.CompanyID = id;
                            contentPanel.Controls.Add(cmpEdit);
                        }
                        break;
                    case "divisionedit":
                        {
                            DivisionEdit divEdit = (DivisionEdit)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/DivisionEdit.ascx");
                            int id = 0;
                            int.TryParse(Request["ItemID"], out id);
                            divEdit.DivisionID = id;
                            contentPanel.Controls.Add(divEdit);
                        }
                        break;
                    case "divisionnew":
                        DivisionNew divNew = (DivisionNew)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/DivisionNew.ascx");
                        contentPanel.Controls.Add(divNew);
                        break;
                    case "departmentnew":
                        DepartmentNew dptNew = (DepartmentNew)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/DepartmentNew.ascx");
                        contentPanel.Controls.Add(dptNew);
                        break;
                    case "departmentedit":
                        {
                            DepartmentEdit dptEdit = (DepartmentEdit)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/DepartmentEdit.ascx");
                            int id = 0;
                            int.TryParse(Request["ItemID"], out id);
                            dptEdit.DepartmentID = id;
                            contentPanel.Controls.Add(dptEdit);
                        }
                        break;
                    case "ugittasktemplates":
                        ProjectTemplateView ptView = (ProjectTemplateView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ProjectTemplateView.ascx");
                        contentPanel.Controls.Add(ptView);
                        break;
                    case "projecttemplateedit":
                        ProjectTemplateEdit pteView = (ProjectTemplateEdit)Page.LoadControl("~/CONTROLTEMPLATES/admin/ListForm/ProjectTemplateEdit.ascx");
                        int itemID = 0;
                        int.TryParse(Request["ID"], out itemID);
                        pteView.ItemID = itemID;
                        contentPanel.Controls.Add(pteView);
                        break;

                    case "eventcategory":
                        EventCategoriesView _EventCategoriesView = (EventCategoriesView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/EventCategoriesView.ascx");
                        contentPanel.Controls.Add(_EventCategoriesView);
                        break;
                    case "deletetickets":
                        DeleteTickets deleteTickets = (DeleteTickets)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/DeleteTickets.ascx");
                        deleteTickets.Module = Request["Module"] == null ? string.Empty : Convert.ToString(Request["Module"]);
                        contentPanel.Controls.Add(deleteTickets);
                        break;
                    case "holidaycalendarevent":
                        HolidayCalendar _HolidayCalendar = (HolidayCalendar)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/HolidayCalendar.ascx");
                        contentPanel.Controls.Add(_HolidayCalendar);
                        break;
                    case "pmmeventscalendar":
                        PMMEventsCalendar _HolidayCalendar1 = (PMMEventsCalendar)Page.LoadControl("~/CONTROLTEMPLATES/uGovernit/PMMProject/PMMEventsCalendar.ascx");
                        _HolidayCalendar1.ProjectPublicID = Convert.ToString(Request["projectPublicID"]);
                        contentPanel.Controls.Add(_HolidayCalendar1);
                        break;

                    //case "adusers":
                    //    ADUser _ADUser = (ADUser)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ADUser.ascx");
                    //    contentPanel.Controls.Add(_ADUser);
                    //    break;
                    case "ugitlog":
                        UGITLogViewer ugitLogViewer = (UGITLogViewer)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/UGITLogViewer.ascx");
                        contentPanel.Controls.Add(ugitLogViewer);
                        break;

                    case "wikileftnavigation":
                        WikiNavigationView wikiNavigationView = (WikiNavigationView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/WikiNavigationView.ascx");
                        contentPanel.Controls.Add(wikiNavigationView);
                        break;

                    case "menunavigationview":
                        MenuNavigationView _MenuNavigationView = (MenuNavigationView)Page.LoadControl("~/controltemplates/admin/ListForm/MenuNavigationView.ascx");
                        contentPanel.Controls.Add(_MenuNavigationView);
                        break;

                    case "editmenunavigation":
                        MenuNavigationEdit _MenuNavigationEdit = (MenuNavigationEdit)Page.LoadControl("~/controltemplates/admin/ListForm/MenuNavigationEdit.ascx");
                        _MenuNavigationEdit.Id = Request["ItemId"];
                        _MenuNavigationEdit.MenuType = Request["type"];
                        contentPanel.Controls.Add(_MenuNavigationEdit);
                        break;
                    case "pageeditorview":
                        PageEditor _PageEditor = (PageEditor)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/PageDesigner/PageEditor.ascx");
                        contentPanel.Controls.Add(_PageEditor);
                        break;

                    //            case "admincatalog":
                    //                AdminCatalog admincatalog = (AdminCatalog)Page.LoadControl("~/_controltemplates/15/uGovernIT/AdminCatalog.ascx");
                    //                contentPanel.Controls.Add(admincatalog);
                    //                break;
                    //            case "updatethemecolor":
                    //                UpdateThemeColor uTheme = (UpdateThemeColor)Page.LoadControl("~/_controltemplates/15/ListForm/UpdateThemeColor.ascx");
                    //                contentPanel.Controls.Add(uTheme);
                    //                break;
                    case "userskillsview":
                        UserSkillsView _UserSkillsView = (UserSkillsView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/UserSkillsView.ascx");
                        contentPanel.Controls.Add(_UserSkillsView);
                        break;
                    case "usercertificatesview":
                        UserCertificatesView _UserCertificatesView = (UserCertificatesView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/UserCertificatesView.ascx");
                        contentPanel.Controls.Add(_UserCertificatesView);
                        break;
                    case "userrolesview":
                        UserRolesView _UserRolesView = (UserRolesView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/UserRolesView.ascx");
                        contentPanel.Controls.Add(_UserRolesView);
                        break;
                    case "adduserskills":
                        AddUserSkills _AddUserSkills = (AddUserSkills)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/AddUserSkills.ascx");
                        _AddUserSkills.skillID = Convert.ToInt32(Request["SkillID"]);
                        contentPanel.Controls.Add(_AddUserSkills);
                        break;
                    case "addusercertificates":
                        AddUserCertificates _AddUserCertificates = (AddUserCertificates)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/AddUserCertificates.ascx");
                        _AddUserCertificates.certificateID = Convert.ToInt32(Request["CertificateID"]);
                        contentPanel.Controls.Add(_AddUserCertificates);
                        break;
                    case "addexperiencedtags":
                        AddExperiencedTags _AddExperiencedTags = (AddExperiencedTags)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/AddExperiencedTags.ascx");
                        _AddExperiencedTags.ExperiencedTagID = Convert.ToInt32(Request["ProjectTagID"]);
                        contentPanel.Controls.Add(_AddExperiencedTags);
                        break;
                    case "adduserroles":
                        AddUserRoles _AddUserRoles = (AddUserRoles)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/AddUserRoles.ascx");
                        _AddUserRoles.roleID = Convert.ToString(Request["RoleID"]);
                        contentPanel.Controls.Add(_AddUserRoles);
                        break;
                    //            case "adminauth":
                    //                AdminAuth adminAuth = (AdminAuth)Page.LoadControl("~/_controltemplates/15/uGovernIT/AdminAuth.ascx");
                    //                contentPanel.Controls.Add(adminAuth);
                    //                break;
                    case "linkconfig":
                        LinkConfiguratorView _LinkConfig = (LinkConfiguratorView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/LinkConfiguratorView.ascx");
                        contentPanel.Controls.Add(_LinkConfig);
                        break;
                    case "moduledefaultvalues":
                        ModuleDefaultsView _ModuleDefaultsView = (ModuleDefaultsView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ModuleDefaultsView.ascx");
                        contentPanel.Controls.Add(_ModuleDefaultsView);
                        break;
                    //            case "organizationview":
                    //                OrganizationView _OrganizationView = (OrganizationView)Page.LoadControl("~/_controltemplates/15/ListForm/OrganizationView.ascx");
                    //                contentPanel.Controls.Add(_OrganizationView);
                    //                break;
                    //            case "contactsview":
                    //                ContactsView _ContactsView = (ContactsView)Page.LoadControl("~/_controltemplates/15/ListForm/ContactsView.ascx");
                    //                contentPanel.Controls.Add(_ContactsView);
                    //                break;
                    case "emailtoticket":
                        EmailToTicket _tickettoemail = (EmailToTicket)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/EmailToTicket.ascx");
                        contentPanel.Controls.Add(_tickettoemail);
                        break;
                    case "smtpmailsetting":
                        SmtpSetting _smtpsetting = (SmtpSetting)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/SmtpSetting.ascx");
                        contentPanel.Controls.Add(_smtpsetting);
                        break;
                    case "resourcemanagement":
                        CustomGroupsAndUsersInfo _Resourcemanagement = (CustomGroupsAndUsersInfo)Page.LoadControl("~/CONTROLTEMPLATES/RMM/CustomGroupsAndUsersInfo.ascx");
                        contentPanel.Controls.Add(_Resourcemanagement);
                        break;
                    case "ticketpriority":
                        {
                            PriorityView priorityCtr = (PriorityView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/PriorityView.ascx");
                            contentPanel.Controls.Add(priorityCtr);
                        }
                        break;
                    case "ticketprioritynew":
                        {
                            PriorityNew ticketPriorityNew = (PriorityNew)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/PriorityNew.ascx");
                            contentPanel.Controls.Add(ticketPriorityNew);
                        }
                        break;
                    case "ticketpriorityedit":
                        int ticketPriorityID;
                        int.TryParse(Request["ID"], out ticketPriorityID);
                        PriorityEdit ticketpriorityEdit = (PriorityEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/PriorityEdit.ascx");
                        ticketpriorityEdit.ItemID = ticketPriorityID;
                        contentPanel.Controls.Add(ticketpriorityEdit);
                        break;
                    case "movestagetoproduction":
                        MoveStageToProduction _movestagetoproduction = (MoveStageToProduction)Page.LoadControl("~/controltemplates/admin/listform/MoveStageToProduction.ascx");
                        _movestagetoproduction.ModuleName = string.Empty;
                        _movestagetoproduction.ServiceId = string.Empty;

                        if (Request["module"] != null)
                            _movestagetoproduction.ModuleName = Convert.ToString(Request["module"]);
                        if (Request["serviceId"] != null)
                            _movestagetoproduction.ServiceId = Convert.ToString(Request["serviceId"]);
                        _movestagetoproduction.ListName = Convert.ToString(Request["list"]);
                        contentPanel.Controls.Add(_movestagetoproduction);
                        break;
                    //            case "enablemigrate":
                    //                Migrate _migrate = (Migrate)Page.LoadControl("~/_controltemplates/15/ListForm/Migrate.ascx");
                    //                contentPanel.Controls.Add(_migrate);
                    //                break;
                    case "setugittheme":
                        SetUgitTheme setUgitTheme = (SetUgitTheme)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/SetUgitTheme.ascx");
                        contentPanel.Controls.Add(setUgitTheme);
                        break;
                    //            case "assetintegrationconfiguration":
                    //                AssetIntegrationConfigurations asset = (AssetIntegrationConfigurations)Page.LoadControl("~/_controltemplates/15/ListForm/AssetIntegrationConfigurations.ascx");
                    //                contentPanel.Controls.Add(asset);
                    //                break;
                    //            case "choicefieldedit":
                    //                ChoiceFieldEdit _choiceFieldEdit = (ChoiceFieldEdit)Page.LoadControl("~/_controltemplates/15/ListForm/ChoiceFieldEdit.ascx");
                    //                contentPanel.Controls.Add(_choiceFieldEdit);
                    //                break;
                    //            case "vendortypeview":
                    //                VendorType vendortype = (VendorType)Page.LoadControl("~/_controltemplates/15/uGovernIT/VendorType.ascx");
                    //                contentPanel.Controls.Add(vendortype);
                    //                break;
                    //            default:
                    //                break;
                    //        }
                    case "checklisttemplates":
                        CheckListTemplatesView _CheckListTemplatesView = (CheckListTemplatesView)this.LoadControl("~/controltemplates/admin/listform/CheckListTemplatesView.ascx");
                        contentPanel.Controls.Add(_CheckListTemplatesView);
                        break;
                    case "rankingcriteria":
                        RankingCriteriaViewAdmin _RankingCriteriaViewAdmin = (RankingCriteriaViewAdmin)this.LoadControl("~/controltemplates/admin/listform/RankingCriteriaViewAdmin.ascx");
                        contentPanel.Controls.Add(_RankingCriteriaViewAdmin);
                        break;

                    case "jobtitleview":
                        JobTitleView _jobTitleView = (JobTitleView)this.LoadControl("~/controltemplates/admin/listform/JobTitleView.ascx");
                        contentPanel.Controls.Add(_jobTitleView);
                        break;
                    case "addjobtitle":
                        AddJobTitle _addJobTitle = (AddJobTitle)this.LoadControl("~/controltemplates/admin/listform/AddJobTitle.ascx");
                        contentPanel.Controls.Add(_addJobTitle);
                        break;
                    case "leadcriteria":
                        LeadCriteriaView leadCriteriaView = (LeadCriteriaView)this.LoadControl("~/controltemplates/admin/listform/LeadCriteriaView.ascx");
                        contentPanel.Controls.Add(leadCriteriaView);
                        break;
                    case "projectcomplexity":
                        ProjectComplexityView projectComplexityView = (ProjectComplexityView)this.LoadControl("~/controltemplates/admin/listform/ProjectComplexityView.ascx");
                        contentPanel.Controls.Add(projectComplexityView);
                        break;
                    case "experiencedtags":
                        ExperiencedTags experiencedTags = (ExperiencedTags)this.LoadControl("~/controltemplates/admin/listform/ExperiencedTags.ascx");
                        contentPanel.Controls.Add(experiencedTags);
                        break;
                    case "userprojectexperiences":
                        UserProjectExperiences userProjectExperiences = (UserProjectExperiences)this.LoadControl("~/controltemplates/admin/listform/UserProjectExperiences.ascx");
                        contentPanel.Controls.Add(userProjectExperiences);
                        break;
                    case "datarefresh":
                        DataRefresh dataRefresh = (DataRefresh)this.LoadControl("~/controltemplates/admin/listform/DataRefresh.ascx");
                        contentPanel.Controls.Add(dataRefresh);
                        break;
                    case "applicationhealth":
                        ApplicationHealth applicationhealth = (ApplicationHealth)this.LoadControl("~/controltemplates/admin/listform/ApplicationHealth.ascx");
                        contentPanel.Controls.Add(applicationhealth);
                        break;
                    case "documenttypeview":
                        DocumentTypeView documentViewType = (DocumentTypeView)Page.LoadControl("~/controltemplates/admin/ListForm/DocumentTypeView.ascx");
                        contentPanel.Controls.Add(documentViewType);
                        break;

                    case "phrasesview":
                        PhrasesView _phraseView = (PhrasesView)Page.LoadControl("~/controltemplates/admin/ListForm/PhrasesView.ascx");
                        contentPanel.Controls.Add(_phraseView);
                        break;

                    case "widgetsview":
                        WidgetsView _widgetsView = (WidgetsView)Page.LoadControl("~/controltemplates/admin/ListForm/WidgetsView.ascx");
                        contentPanel.Controls.Add(_widgetsView);
                        break;


                    case "widgetaddedit":
                        WidgetAddEdit _widgetAddEdit = (WidgetAddEdit)Page.LoadControl("~/controltemplates/admin/ListForm/WidgetAddEdit.ascx");
                        long.TryParse(Request["ID"], out _widgetAddEdit.ID);
                        contentPanel.Controls.Add(_widgetAddEdit);
                        break;

                    case "phrasesaddnew":
                        PhrasesAddNew _phrasesAddNew = (PhrasesAddNew)Page.LoadControl("~/controltemplates/admin/ListForm/PhraseAddNew.ascx");
                        contentPanel.Controls.Add(_phrasesAddNew);
                        break;

                    case "phraseedit":
                        int _phraseID;
                        PhraseEdit _phraseEdit = (PhraseEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/PhraseEdit.ascx");
                        int.TryParse(Request["ID"], out _phraseID);
                        _phraseEdit.ID = _phraseID;
                        contentPanel.Controls.Add(_phraseEdit);
                        break;
                    case "projectstandardworkitemview":
                        ProjectStandardWorkItemView projectStandardWorkItemView = (ProjectStandardWorkItemView)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ProjectStandardWorkItemView.ascx");
                        contentPanel.Controls.Add(projectStandardWorkItemView);
                        break;

                    case "addchoices":
                        AddChoiceFieldOptions addChoiceFieldOptionsCtr = this.LoadControl("~/ControlTemplates/Admin/ListForm/AddChoiceFieldOptions.ascx") as AddChoiceFieldOptions;
                        contentPanel.Controls.Add(addChoiceFieldOptionsCtr);
                        break;
                    case "employeetypeview":
                        EmployeeTypesView EmployeeTypeView = this.LoadControl("~/ControlTemplates/Admin/ListForm/EmployeeTypesView.ascx") as EmployeeTypesView;
                        contentPanel.Controls.Add(EmployeeTypeView);
                        break;
                    case "addemployeetypes":
                        AddEmployeeTypes addEmployeeType = this.LoadControl("~/ControlTemplates/Admin/ListForm/AddEmployeeTypes.ascx") as AddEmployeeTypes;
                        contentPanel.Controls.Add(addEmployeeType);
                        break;
                    case "projectsimilarityconfigview":
                        ProjectSimilarityConfigView _ProjectSimilarityConfigView = (ProjectSimilarityConfigView)this.LoadControl("~/controltemplates/admin/listform/ProjectSimilarityConfigView.ascx");
                        contentPanel.Controls.Add(_ProjectSimilarityConfigView);
                        break;
                    case "addprojectsimilarityconfig":
                        AddProjectSimilarityConfig _AddProjectSimilarityConfigView = (AddProjectSimilarityConfig)this.LoadControl("~/controltemplates/admin/listform/AddProjectSimilarityConfig.ascx");
                        _AddProjectSimilarityConfigView.moduleName = Request["moduleName"];
                        _AddProjectSimilarityConfigView.selectedMetricType = Request["selectedMetricType"]; 
                        contentPanel.Controls.Add(_AddProjectSimilarityConfigView);
                        break;
                    case "studios":
                        StudioView  _StudioView = (StudioView)this.LoadControl("~/controltemplates/admin/listform/StudioView.ascx");
                        contentPanel.Controls.Add(_StudioView);
                        break;
                    case "departmentjobtitlemapping":
                        DepartmentJobTitleMapping _departmentjobtitlemapping = (DepartmentJobTitleMapping)this.LoadControl("~/controltemplates/admin/listform/DepartmentJobTitleMapping.ascx");
                        contentPanel.Controls.Add(_departmentjobtitlemapping);
                        break;
                    case "departmentrolemapping":
                        DepartmentRoleMapping _departmentrolemapping = (DepartmentRoleMapping)this.LoadControl("~/controltemplates/admin/listform/DepartmentRoleMapping.ascx");
                        contentPanel.Controls.Add(_departmentrolemapping);
                        break;
                    case "functionrolemapping":
                        FunctionRoleMappingView _functionrolemappingview = this.LoadControl("~/controltemplates/admin/listform/FunctionRoleMappingView.ascx") as FunctionRoleMappingView;
                        contentPanel.Controls.Add(_functionrolemappingview); 
                        break;
                }
                base.OnInit(e);
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

    }
}
