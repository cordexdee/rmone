using DevExpress.Web;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.ControlTemplates.uGovernIT;
using uGovernIT.Manager;
using uGovernIT.Manager.Helper;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.Controllers;
using uGovernIT.Web.ControlTemplates;
using uGovernIT.Web.ControlTemplates.Admin.ListForm;
using uGovernIT.Web.ControlTemplates.CoreUI;
using uGovernIT.Web.ControlTemplates.GlobalPage;
using uGovernIT.Web.ControlTemplates.PMM;
using uGovernIT.Web.ControlTemplates.RMM;
using uGovernIT.Web.ControlTemplates.RMONE;
using uGovernIT.Web.ControlTemplates.Shared;
using uGovernIT.Web.ControlTemplates.uGovernIT;
using uGovernIT.Web.ControlTemplates.uGovernIT.PMMProject;
using uGovernIT.Web.ControlTemplates.Wiki;

namespace uGovernIT.Web
{
    public partial class DelegateControl : UPage
    {
        public string TicketID { get; set; }
        public string listName { get; set; }
        public string commentType { get; set; }

        private ApplicationContext _context = null;
        private FieldConfigurationManager _fieldConfigurationManager = null;
        private DashboardManager _dashboardManager = null;

        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }

        }

        protected FieldConfigurationManager FieldConfigurationManager
        {
            get
            {
                if (_fieldConfigurationManager == null)
                {
                    _fieldConfigurationManager = new FieldConfigurationManager(ApplicationContext);
                }
                return _fieldConfigurationManager;
            }

        }

        protected DashboardManager DashboardManager
        {
            get
            {
                if (_dashboardManager == null)
                {
                    _dashboardManager = new DashboardManager(ApplicationContext);
                }
                return _dashboardManager;
            }

        }
        private ModuleViewManager _moduleViewManager = null;
        protected ModuleViewManager ModuleManager
        {
            get
            {
                if(_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(ApplicationContext);
                }
                return _moduleViewManager;
            }
        }

        private TicketManager _ticketManager;
        protected TicketManager TicketManager
        {
            get
            {
                if(_ticketManager == null)
                    _ticketManager = new TicketManager(ApplicationContext);
                return _ticketManager;
            }
        }

        protected string frameId = string.Empty;
        protected bool printEnable;



        //private int PMMId;
        // ApplicationContext context = HttpContext.Current.GetManagerContext();
        //FieldConfigurationManager configFieldManager = null;
        //DashboardManager dManager = null;



        protected override void OnInit(EventArgs e)
        {
            // configFieldManager = new FieldConfigurationManager(context);
            // dManager = new DashboardManager(context);
            frameId = Request["frameObjId"];
            string control = Request["control"];
            string ctrl = string.Empty;
            bool showBaseline = UGITUtility.StringToBoolean(Request["showBaseline"]);
            double baselineNum = Convert.ToDouble(Request["baselineNum"]);
            bool isReadOnly = false;
            UserProfileManager objUserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            if (!string.IsNullOrWhiteSpace(Request["ctrl"]))
                ctrl = Request["ctrl"].Trim();

            if (Request["enablePrint"] != null)
            {
                printEnable = true;
                isReadOnly = true;
            }

            if (!string.IsNullOrWhiteSpace(ctrl))
            {
                Type type = Type.GetType($"uGovernIT.Web.ControlTemplates.{ctrl}", false, true);
                if (type != null)
                {
                    string ctrlFullName = type.FullName.Replace($"{type.Assembly.GetName().Name}.", "").Replace(".", "/");
                    Control ctr = Page.LoadControl($"~/{ctrlFullName}.ascx");
                    managementControls.Controls.Add(ctr);
                    return;
                }
            }

            if (control != null && control.Trim() != string.Empty)
            {
                control = control.ToLower();
                switch (control)
                {
                    case "ugitmodulewebpart":
                        uGovernITModuleWebpartUserControl ugitwebpart = (uGovernITModuleWebpartUserControl)Page.LoadControl("/ControlTemplates/Shared/uGovernITModuleWebpartUserControl.ascx");
                        MainPanel.Controls.Add(ugitwebpart);
                        break;

                    case "addroles":
                        GlobalRolesAddEdit _rolesAddEdit = (GlobalRolesAddEdit)Page.LoadControl("/ControlTemplates/admin/listform/GlobalRolesAddEdit.ascx");
                        _rolesAddEdit.RoleID = UGITUtility.ObjectToString(Request["RoleID"]);
                        managementControls.Controls.Add(_rolesAddEdit);
                        break;
                    case "allocationtemplategrid":
                        AllocationTemplateGrid _allocationTemplateGrid = (AllocationTemplateGrid)Page.LoadControl("/ControlTemplates/RMM/AllocationTemplateGrid.ascx");
                        _allocationTemplateGrid.TemplateID = Convert.ToString(Request["templateID"]);
                        _allocationTemplateGrid.ProjectID = Convert.ToString(Request["projectID"]);
                        _allocationTemplateGrid.StartDate = Convert.ToString(Request["StartDate"]);
                        _allocationTemplateGrid.EndDate = Convert.ToString(Request["EndDate"]);
                        _allocationTemplateGrid.Option = Convert.ToString(Request["Option"]);
                        managementControls.Controls.Add(_allocationTemplateGrid);
                        break;
                    case "importallocationtemplate":
                        ImportAllocationTemplate _importAllocationTemplate = (ImportAllocationTemplate)Page.LoadControl("/ControlTemplates/RMM/ImportAllocationTemplate.ascx");
                        _importAllocationTemplate.ProjectID = Convert.ToString(Request["ProjectID"]);
                        managementControls.Controls.Add(_importAllocationTemplate);
                        break;
                    case "similarprojects":
                        SimilarProjects _SimilarProjects = (SimilarProjects)Page.LoadControl("/ControlTemplates/RMM/SimilarProjects.ascx");
                        _SimilarProjects.ProjectID = Convert.ToString(Request["ProjectID"]);
                        //_SimilarProjects.Sector = Convert.ToString(Request["Sector"]);
                        //_SimilarProjects.ProjectComplexity = Convert.ToString(Request["ProjectComplexity"]);
                        _SimilarProjects.SearchData = Convert.ToString(Request["SearchData"]);
                        managementControls.Controls.Add(_SimilarProjects);
                        break;
                    case "crmcomplexitytickets":
                        {
                            CustomFilteredTickets crmtickets = (CustomFilteredTickets)Page.LoadControl("~/ControlTemplates/Shared/CustomFilteredTickets.ascx");
                            string userValue = Convert.ToString(Request["UserId"]);
                            string complexity = Convert.ToString(Request["Complexity"]);
                            string moduleName = Convert.ToString(Request["ModuleName"]);
                            bool IncludeClosedProjects = Convert.ToBoolean(Request["IncludeClosedProjects"]);
                            ResourceProjectComplexityManager complexityManager = new ResourceProjectComplexityManager(ApplicationContext);
                            SummaryResourceProjectComplexity projectComplexity = null;
                            if (complexity == "Complexity Level" || string.IsNullOrEmpty(complexity))
                                projectComplexity = complexityManager.Get(x => x.UserName == userValue);
                            else
                                projectComplexity = complexityManager.Get(x => x.UserName == userValue && x.Complexity == Convert.ToInt32(complexity));

                            ProjectEstimatedAllocationManager projectAllocManager = new ProjectEstimatedAllocationManager(ApplicationContext);
                            List<ProjectEstimatedAllocation> lstAllocations = projectAllocManager.Load(x => x.AssignedTo == projectComplexity.UserId);
                            List<string> lstTicketIds = lstAllocations.Select(x => $"'{x.TicketId}'").Distinct().ToList();

                            ModuleViewManager moduleManager = new ModuleViewManager(ApplicationContext);
                            UGITModule cprmodule = moduleManager.LoadByName(moduleName);
                            TicketManager ticketManager = new TicketManager(ApplicationContext);
                            string complexityColumnName = DatabaseObjects.Columns.ProjectComplexity;

                            DataTable dtAlltickets = IncludeClosedProjects == true ? ticketManager.GetAllTickets(cprmodule) : ticketManager.GetOpenTickets(cprmodule);
                            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ProjectComplexity, dtAlltickets))
                                complexityColumnName = DatabaseObjects.Columns.ProjectComplexity;
                            else if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.CRMProjectComplexity, dtAlltickets))
                                complexityColumnName = DatabaseObjects.Columns.CRMProjectComplexity;

                            DataRow[] ticketrows = new DataRow[0];
                            if (complexity == "Complexity Level" || string.IsNullOrEmpty(complexity))
                                ticketrows = dtAlltickets.Select($"TicketId IN ({string.Join(Constants.Separator6, lstTicketIds)})");
                            else if (UGITUtility.StringToInt(complexity) == 1)
                                ticketrows = dtAlltickets.Select($"({complexityColumnName} is null or {complexityColumnName}='{complexity}') and TicketId IN ({string.Join(Constants.Separator6, lstTicketIds)})");
                            else
                                ticketrows = dtAlltickets.Select($"{complexityColumnName}='{complexity}' and TicketId IN ({string.Join(Constants.Separator6, lstTicketIds)})");

                            crmtickets.IsFilteredTableExist = true;
                            crmtickets.HideAllTicketTab = true;
                            crmtickets.HideModuleDesciption = true;
                            crmtickets.HideNewTicketButton = false;
                            crmtickets.HideModuleDetail = true;
                            crmtickets.ModuleName = cprmodule.ModuleName;
                            if (ticketrows.Length > 0)
                                crmtickets.FilteredTable = ticketrows.CopyToDataTable();
                            else
                                crmtickets.FilteredTable = null;
                            managementControls.Controls.Add(crmtickets);
                        }
                        break;
                    case "projectallstatussummary":
                        ProjectAllStatusSummary statusSummaries = (ProjectAllStatusSummary)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/PMMProject/ProjectAllStatusSummary.ascx");
                        statusSummaries.ProjectID = Request["ticketid"];
                        managementControls.Controls.Add(statusSummaries);
                        break;
                    case "projectstatusdetail":
                        PMMProjectStatus pStatus = (PMMProjectStatus)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/PMMProject/PMMProjectStatus.ascx");
                        pStatus.PMMID = Convert.ToInt32(Request["pmmID"]);
                        pStatus.FrameId = frameId;
                        pStatus.IsReadOnly = isReadOnly;
                        pStatus.IsShowBaseline = showBaseline;
                        pStatus.BaselineId = baselineNum;
                        pStatus.TicketId = Convert.ToString(Request["ticketId"]);
                        managementControls.Controls.Add(pStatus);
                        break;


                    case "cutomfilterticket":
                        CustomFilteredTickets _CustomFilterTicket = (CustomFilteredTickets)Page.LoadControl("~/ControlTemplates/Shared/CustomFilteredTickets.ascx");
                        _CustomFilterTicket.showExportIcons = true;
                        managementControls.Controls.Add(_CustomFilterTicket);
                        break;


                    case "editcomment":
                        TicketCommentsView _TicketCommentsView = (TicketCommentsView)Page.LoadControl("~/ControlTemplates/uGovernIT/TicketCommentsView.ascx");
                        _TicketCommentsView.TicketID = Request["ticketId"];
                        _TicketCommentsView.listName = Request["listName"];

                        _TicketCommentsView.commentType = Request["ctype"];
                        managementControls.Controls.Add(_TicketCommentsView);
                        break;
                    case "history":
                        History history = (History)Page.LoadControl("~/ControlTemplates/Shared/History.ascx");
                        history.FrameId = frameId;
                        history.ReadOnly = isReadOnly;
                        //int ticketId = 0;
                        //int.TryParse(Request["ticketId"], out ticketId);
                        string listName = Request["listName"];
                        history.TicketId = UGITUtility.ObjectToString(Request["ticketId"]);
                        history.ListName = listName;
                        managementControls.Controls.Add(history);
                        break;
                    case "ticketemails":
                        EmailsControl email = (EmailsControl)Page.LoadControl("~/ControlTemplates/uGovernIT/EmailsControl.ascx");

                        //TicketEmail ticketemail = (TicketEmail)Page.LoadControl("~/ControlTemplates/uGovernIT/TicketEmail.ascx");
                        //configurationVariableListEdit.clientAdminID = clientAdminID;
                        email.PublicTicketID = Request["PublicTicketID"];
                        if (Request["Module"] != null)
                        {
                            email.ModuleName = Request["Module"];
                        }
                        email.ReadOnly = isReadOnly;
                        managementControls.Controls.Add(email);
                        break;
                    case "ticketemailview":
                        TicketEmailView ticketemailviewControl = (TicketEmailView)Page.LoadControl("~/ControlTemplates/uGovernIT/TicketEmailView.ascx");
                        ticketemailviewControl.ticketEmailID = Request["TicketEmailID"];
                        managementControls.Controls.Add(ticketemailviewControl);
                        break;

                    case "scheduleactions":
                        ScheduleActionsView scheduleactions = (ScheduleActionsView)Page.LoadControl("~/ControlTemplates/uGovernIT/ScheduleActionsView.ascx");
                        if (Request["IsModuleWebpart"] != null)
                        {
                            scheduleactions.IsModuleWebpart = Convert.ToBoolean(Request["IsModuleWebpart"]);
                        }
                        if (Request["IsArchive"] != null)
                        {
                            scheduleactions.IsArchive = Convert.ToBoolean(Request["IsArchive"]);
                        }
                        if (Request["Module"] != null)
                        {
                            scheduleactions.ModuleName = Request["Module"];
                        }
                        if (Request["PublicTicketID"] != null)
                        {
                            scheduleactions.PublicTicket = Request["PublicTicketID"];
                        }
                        managementControls.Controls.Add(scheduleactions);
                        break;
                    case "approvaltab":
                        ModuleItemApprovals ctr3 = (ModuleItemApprovals)Page.LoadControl("~/ControlTemplates/uGovernIT/ModuleItemApprovals.ascx");
                        //int moduleId = 0;
                        //int.TryParse(Request["moduleName"], out moduleId);
                        string moduleItemPublicID = Request["ticketID"];
                        int currentWorkflowStep = 0;
                        int.TryParse(Request["CurrentWorkflowStep"], out currentWorkflowStep);
                        ctr3.ModuleName = Convert.ToString(Request["moduleName"]);
                        ctr3.CurrentWorkFlowStep = currentWorkflowStep;
                        ctr3.ModuleItemPublicID = moduleItemPublicID;
                        managementControls.Controls.Add(ctr3);
                        break;
                    case "scheduleactionslist":
                        ScheduleActionsView scheduleActionView = (ScheduleActionsView)Page.LoadControl("~/ControlTemplates/uGovernIT/ScheduleActionsView.ascx");
                        contentPanel.Controls.Add(scheduleActionView);
                        break;
                    case "customticketrelationship":
                        CustomTicketRelationShip crelation = (CustomTicketRelationShip)Page.LoadControl("~/ControlTemplates/Shared/CustomTicketRelationShip.ascx");
                        crelation.LoadData = true;
                        crelation.TicketId = Request["ticketId"];
                        crelation.ParentDetailOnDemand = false;
                        crelation.ChildDetailOnDemand = false;
                        crelation.ModuleName = Convert.ToString(Request["moduleName"]);
                        if (printEnable)
                        {

                            crelation.ShowDelete = false;
                            crelation.AddChild = false;
                        }
                        managementControls.Controls.Add(crelation);
                        break;
                    case "service":
                        NewServiceWizard servicesWizard = Page.LoadControl("/CONTROLTEMPLATES/uGovernIT/Services/NewServiceWizard.ascx") as NewServiceWizard;
                        int serviceId = 0;
                        int.TryParse(Request["serviceid"], out serviceId);
                        if (Request["category"] != null && Convert.ToString(Request["category"]) != string.Empty)
                            servicesWizard.Category = Convert.ToString(Request["category"]);
                        servicesWizard.ServiceId = serviceId;
                        managementControls.Controls.Add(servicesWizard);
                        break;
                    case "resourceavailability":
                        ResourceAvailability _ResourceAvailability = (ResourceAvailability)Page.LoadControl("~/controltemplates/RMM/ResourceAvailability.ascx");
                        _ResourceAvailability.TicketId = Convert.ToString(Request["projectPublicID"]);
                        _ResourceAvailability.ControlId = Convert.ToString(Request["ControlId"]);
                        _ResourceAvailability.FrameId = frameId;
                        _ResourceAvailability.ReadOnly = isReadOnly;
                        managementControls.Controls.Add(_ResourceAvailability);
                        break;
                    case "capcityreport":
                        CapacityReport _CapacityReport = (CapacityReport)Page.LoadControl("~/controltemplates/RMM/CapacityReport.ascx");
                        //_CapacityReport.TicketId = Convert.ToString(Request["projectPublicID"]);
                        //_CapacityReport.ControlId = Convert.ToString(Request["ControlId"]);
                        _CapacityReport.FrameId = frameId;
                        // _CapacityReport.ReadOnly = isReadOnly;
                        managementControls.Controls.Add(_CapacityReport);
                        break;
                    case "resourceprojectcomplexity":
                        ResourceProjectComplexity _ResourceProjectComplexity = (ResourceProjectComplexity)Page.LoadControl("~/controltemplates/RMM/ResourceProjectComplexity.ascx");
                        managementControls.Controls.Add(_ResourceProjectComplexity);
                        break;
                    case "resourceallocationgrid":
                        ResourceAllocationGrid _ResourceAllocationGrid = (ResourceAllocationGrid)Page.LoadControl("~/CONTROLTEMPLATES/RMM/ResourceAllocationGrid.ascx");
                        managementControls.Controls.Add(_ResourceAllocationGrid);
                        break;
                    case "resourceallocationgridnew":
                        ResourceAllocationGridNew _ResourceAllocationGridNew = (ResourceAllocationGridNew)Page.LoadControl("~/CONTROLTEMPLATES/RMONE/ResourceAllocationGridNew.ascx");
                        if (Request["SelectedResource"] != null && UGITUtility.ObjectToString(Request["SelectedResource"]) != string.Empty)
                        {
                           _ResourceAllocationGridNew.SelectedUser = UGITUtility.ObjectToString(Request["SelectedResource"]);
                        }
                        if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(Request["selectedYear"])))
                            _ResourceAllocationGridNew.SelectedYear = UGITUtility.ObjectToString(Request["selectedYear"]);
                        if (!string.IsNullOrEmpty( UGITUtility.ObjectToString(Request["SelectedUsers"]) ))
                            _ResourceAllocationGridNew.SelectedUser = UGITUtility.ObjectToString(Request["SelectedUsers"]);
                        if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(Request["RequestFromManagerScreen"])) 
                            && !string.IsNullOrEmpty(UGITUtility.ObjectToString(UGITUtility.GetCookieValue(Request, "SelectedUsersForGantt"))))
                        {
                            _ResourceAllocationGridNew.SelectedUser = UGITUtility.ObjectToString(UGITUtility.GetCookieValue(Request,"SelectedUsersForGantt")).Replace("%2C", ",");
                        }
                        if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(Request["RequestFromProjectAllocation"])))
                        {
                            _ResourceAllocationGridNew.RequestFromProjectAllocation = Convert.ToBoolean(Request["RequestFromProjectAllocation"]);
                        }
                        if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(Request["classNameSt"])))
                        {
                            _ResourceAllocationGridNew.classNameSt = UGITUtility.ObjectToString(Request["classNameSt"]);
                        }
                        if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(Request["classNameEd"])))
                        {
                            _ResourceAllocationGridNew.classNameEd = UGITUtility.ObjectToString(Request["classNameEd"]);
                        }
                        if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(Request["ganttProjSD"])))
                        {
                            _ResourceAllocationGridNew.ganttProjSD = UGITUtility.ObjectToString(Request["ganttProjSD"]);
                        }
                        if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(Request["ganttProjED"])))
                        {
                            _ResourceAllocationGridNew.ganttProjED = UGITUtility.ObjectToString(Request["ganttProjED"]);
                        }
                        if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(Request["ganttProjReqAloc"])))
                        {
                            _ResourceAllocationGridNew.ganttProjReqAloc = UGITUtility.ObjectToString(Request["ganttProjReqAloc"]);
                        }
                        managementControls.Controls.Add(_ResourceAllocationGridNew);
                        break;
                    case "managerprojectallocationview":
                        {
                            ManagerProjectAllocationView _managerProjectAllocationView = Page.LoadControl("~/CONTROLTEMPLATES/RMONE/ManagerProjectAllocationView.ascx") as ManagerProjectAllocationView;
                            managementControls.Controls.Add(_managerProjectAllocationView);
                        }
                        break;
                    case "customresourceallocation":
                        CustomResourceAllocation ctr = (CustomResourceAllocation)Page.LoadControl("~/CONTROLTEMPLATES/RMM/CustomResourceAllocation.ascx");
                        ctr.FrameId = frameId;
                        ctr.IncludeClosed = Request["IncludeClosed"] != null && Request["IncludeClosed"] != string.Empty ? Convert.ToBoolean(Request["IncludeClosed"]) : false;
                        ctr.managerFrom = Request["managerFrom"] != null && Request["managerFrom"] != string.Empty ? Request["managerFrom"] : null;
                        managementControls.Controls.Add(ctr);
                        break;
                    case "customresourcetimesheet":
                        CustomResourceTimeSheet ctr1 = (CustomResourceTimeSheet)Page.LoadControl("~/CONTROLTEMPLATES/RMM/CustomResourceTimeSheet.ascx");
                        ctr1.FrameId = frameId;
                        if (!string.IsNullOrEmpty(Request["ResourceId"]))
                            ctr1.ResourceId = Convert.ToString(Request["ResourceId"]);

                        if (!string.IsNullOrEmpty(Request["WeekStartDt"]))
                            ctr1.WeekStartDt = Convert.ToString(Request["WeekStartDt"]);

                        managementControls.Controls.Add(ctr1);
                        break;
                    case "resourcetimesheethistroy":
                        ResourceTimeSheetSignOffHistory signOffHistoryGrid = (ResourceTimeSheetSignOffHistory)Page.LoadControl("~/CONTROLTEMPLATES/RMM/ResourceTimeSheetSignOffHistory.ascx");
                        if (!string.IsNullOrEmpty(Request["signOffItemId"]))
                        {
                            signOffHistoryGrid.ListItemID = Request["signOffItemId"];
                            managementControls.Controls.Add(signOffHistoryGrid);
                        }
                        break;
                    case "timesheetpendingapprovals":
                        TimesheetPendingApprovals timesheetPendingApprovals = (TimesheetPendingApprovals)Page.LoadControl("~/CONTROLTEMPLATES/RMM/TimesheetPendingApprovals.ascx");
                        if (!string.IsNullOrEmpty(Request["Id"]))
                        {
                            timesheetPendingApprovals.ResourceId = Request["Id"];
                            managementControls.Controls.Add(timesheetPendingApprovals);
                        }
                        break;
                    case "customgroupsandusersinfo":
                        CustomGroupsAndUsersInfo ctr2 = (CustomGroupsAndUsersInfo)Page.LoadControl("~/CONTROLTEMPLATES/RMM/CustomGroupsAndUsersInfo.ascx");
                        ctr2.FrameId = frameId;
                        managementControls.Controls.Add(ctr2);
                        break;

                    case "buildprofile":
                        BuildProfile _BuildProfile = (BuildProfile)Page.LoadControl("~/controltemplates/RMM/BuildProfile.ascx");
                        _BuildProfile.ProjectID = Convert.ToString(Request["ProjectID"]);
                        managementControls.Controls.Add(_BuildProfile);
                        break;
                    case "compareuserprofile":
                        CompareUsersProfile _CompareUserProfile = (CompareUsersProfile)Page.LoadControl("~/controltemplates/RMM/CompareUsersProfile.ascx");
                        _CompareUserProfile.SelectedUsers = Convert.ToString(Request["SelectedUsers"]);
                        managementControls.Controls.Add(_CompareUserProfile);
                        break;
                    //case "filteredticketlist":
                    //    LoadCustomFilteredTickets(frameId);
                    //    break;

                    case "ticketholdunhold":
                        TicketHoldUnHold _TicketHoldUnHold = (TicketHoldUnHold)Page.LoadControl("~/controltemplates/uGovernIT/TicketHoldUnHold.ascx");
                        _TicketHoldUnHold.TicketId = Request["ids"].Trim();
                        _TicketHoldUnHold.TitleText = Request["titleText"] != null ? Request["titleText"].Trim() : string.Empty;
                        _TicketHoldUnHold.ModuleName = Convert.ToString(Request["moduleName"]);
                        _TicketHoldUnHold.Action = Request["Action"].Trim();
                        managementControls.Controls.Add(_TicketHoldUnHold);
                        break;
                    case "taskslist":
                        TaskList taskList = (TaskList)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Task/TaskList.ascx");
                        taskList.ModuleName = Convert.ToString(Request["module"]);
                        int id = 0;
                        int.TryParse(Request["ticketID"], out id);
                        taskList.TicketID = id;
                        taskList.TicketPublicId = Request["ticketID"];
                        isReadOnly = UGITUtility.StringToBoolean(Request["IsReadOnly"]);
                        taskList.FrameId = frameId;
                        taskList.ShowBaseline = showBaseline;
                        taskList.BaselineNum = baselineNum;
                        taskList.IsReadOnly = isReadOnly;
                        bool batchEdit = bool.TryParse(Request["BatchEdit"], out batchEdit);
                        bool markAsComplete = bool.TryParse(Request["MarkAsComplete"], out markAsComplete);
                        bool newRecuringTask = bool.TryParse(Request["NewRecuringTask"], out newRecuringTask);
                        taskList.DisableBatchEdit = batchEdit;
                        taskList.DisableMarkAsComplete = markAsComplete;
                        taskList.DisableNewRecuringTask = newRecuringTask;
                        managementControls.Controls.Add(taskList);
                        break;
                    case "taskedit":
                        EditTask editTask = (EditTask)Page.LoadControl("/ControlTemplates/uGovernIt/Task/edittask.ascx");
                        editTask.ModuleName = string.IsNullOrEmpty(Convert.ToString(Request["module"])) ? Convert.ToString(Request["moduleName"]) : Convert.ToString(Request["module"]);
                        if (Request["TaskId"] != null)
                            editTask.taskID = Convert.ToInt32(Request["TaskId"]);

                        editTask.FolderName = Convert.ToString(Request["folderName"]);


                        managementControls.Controls.Add(editTask);
                        break;
                    //case "projectresourcework":
                    //    ProjectResourceWorks projectWorks = (ProjectResourceWorks)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/ProjectResourceWorks.ascx");
                    //    int projectID = 0;
                    //    int.TryParse(Request["projectID"], out projectID);
                    //    projectWorks.projectID = projectID;
                    //    projectWorks.projectPublicID = Request["projectPublicID"];
                    //    int year = 0;
                    //    int.TryParse(Request["year"], out year);
                    //    projectWorks.CurrentYear = year;
                    //    managementControls.Controls.Add(projectWorks);
                    //    break;
                    case "importexcel":
                        ImportExcel importExcel = (ImportExcel)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ImportExcel.ascx");
                        managementControls.Controls.Add(importExcel);
                        break;
                    case "exportlist":
                        ExportList export = (ExportList)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ExportList.ascx");
                        managementControls.Controls.Add(export);
                        break;

                    //custom report for crm
                    case "coreservicereport":
                        CoreServiceReport _CoreServiceReport = (CoreServiceReport)Page.LoadControl("~/Controltemplates/uGovernIT/CoreServiceReport.ascx");
                        _CoreServiceReport.Module = Convert.ToString(Request["ModuleName"]);
                        _CoreServiceReport.Type = Convert.ToString(Request["Type"]);
                        managementControls.Controls.Add(_CoreServiceReport);
                        break;


                    // case "combinedlostjobreport":
                    //   CombinedLostJobReport _CombinedLostJobReport = (CombinedLostJobReport)Page.LoadControl("~/Controltemplates/uGovernIT/CombinedLostJobReport.ascx");
                    //   managementControls.Controls.Add(_CombinedLostJobReport);
                    //   break;

                    case "businessunitdistributionreport":
                        BusinessUnitDistributionReport _BusinessUnitDistributionReport = (BusinessUnitDistributionReport)Page.LoadControl("~/Controltemplates/uGovernIT/BusinessUnitDistributionReport.ascx");
                        managementControls.Controls.Add(_BusinessUnitDistributionReport);
                        break;


                    ////custom report for crm
                    //case "coreservicereport":
                    //    CoreServiceReport _CoreServiceReport = (CoreServiceReport)Page.LoadControl("~/Controltemplates/uGovernIT/CoreServiceReport.ascx");
                    //    _CoreServiceReport.Module = Convert.ToString(Request["ModuleName"]);
                    //    _CoreServiceReport.Type = Convert.ToString(Request["Type"]);
                    //    managementControls.Controls.Add(_CoreServiceReport);
                    //    break;


                    // case "combinedlostjobreport":
                    //   CombinedLostJobReport _CombinedLostJobReport = (CombinedLostJobReport)Page.LoadControl("~/Controltemplates/uGovernIT/CombinedLostJobReport.ascx");
                    //   managementControls.Controls.Add(_CombinedLostJobReport);
                    //   break;

                    //case "businessunitdistributionreport":
                    //    BusinessUnitDistributionReport _BusinessUnitDistributionReport = (BusinessUnitDistributionReport)Page.LoadControl("~/Controltemplates/uGovernIT/BusinessUnitDistributionReport.ascx");
                    //    managementControls.Controls.Add(_BusinessUnitDistributionReport);
                    //    break;

                    case "report":
                        DashboardQueryDisplay report = (DashboardQueryDisplay)Page.LoadControl("~/ControlTemplates/ugovernit/DashboardQueryDisplay.ascx");
                        managementControls.Controls.Add(report);
                        break;

                    case "reportresult":
                        QueryWizardPreview queryWizardPreview = (QueryWizardPreview)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/QueryWizardPreview.ascx");
                        int queryID = 0;
                        int.TryParse(Request["reportQueryID"], out queryID);
                        if (queryID <= 0)
                        {
                            string queryName = Request["reportQuery"];
                            if (!string.IsNullOrEmpty(queryName))
                            {
                                Dashboard dashboards = DashboardManager.LoadDashboardsByNames(new List<string>() { queryName }).FirstOrDefault();
                                if (dashboards != null)
                                {
                                    queryID = Convert.ToInt32(dashboards.ID);
                                }
                            }
                        }

                        //    //string whereFilter = Server.UrlDecode(Request["whereFilter"]);
                        queryWizardPreview.Id = queryID;
                        //    //queryWizardPreview.where = whereFilter;
                        managementControls.Controls.Add(queryWizardPreview);

                        //    //CustomFilteredTickets filteredListobj = (CustomFilteredTickets)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/CustomFilteredTickets.ascx");
                        //    //managementControls.Controls.Add(filteredListobj);

                        //    //int queryID = 0;
                        //    //int.TryParse(Request["reportQueryID"], out queryID);
                        //    //string whereFilter = Server.UrlDecode(Request["whereFilter"]);
                        //    //DataTable filteredData = new DataTable();
                        //    //DataTable totalsTable = new DataTable();
                        //    //if (whereFilter != null)
                        //    //    filteredData = GetQueryReportData(queryID, whereFilter, ref totalsTable);
                        //    //else
                        //    //    filteredData = GetQueryReportData(queryID, String.Empty, ref totalsTable);


                        //    //filteredListobj.FilteredTable = filteredData;
                        //    //filteredListobj.ModuleName = string.Empty;
                        //    //filteredListobj.IsFilteredTableExist = true;
                        //    //filteredListobj.HideGlobalSearch = true;
                        //    //filteredListobj.HideModuleDetail = false;
                        //    //filteredListobj.DisableStateManagment = false;
                        //    //filteredListobj.showExportIcons = true;
                        //    //filteredListobj.ColumnsAggregate = totalsTable;

                        break;
                    case "scheduleactionedit":
                        {
                            int Id = 0;
                            int.TryParse(Request["Id"], out Id);
                            ScheduleActionsEdit scheduleActionedit = (ScheduleActionsEdit)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ScheduleActionsEdit.ascx");
                            scheduleActionedit.requestId = Id;
                            managementControls.Controls.Add(scheduleActionedit);
                            break;
                        }
                    case "scheduleactionview":
                        {
                            int Id = 0;
                            int.TryParse(Request["Id"], out Id);
                            ScheduleActionView scheduleActionview = (ScheduleActionView)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ScheduleActionView.ascx");
                            scheduleActionview.requestId = Id;
                            managementControls.Controls.Add(scheduleActionview);
                            break;
                        }
                    case "resourcereport":
                        {
                            ResourceReport resourceReportCtrl = Page.LoadControl("~/CONTROLTEMPLATES/RMM/ResourceReport.ascx") as ResourceReport;
                            resourceReportCtrl.viewType = UGITUtility.ObjectToString(Request["ViewType"]);
                            resourceReportCtrl.reportType = UGITUtility.ObjectToString(Request["ReportType"]);
                            resourceReportCtrl.unitAllocAct = UGITUtility.ObjectToString(Request["unitAllocAct"]);
                            resourceReportCtrl.selectedCategory = UGITUtility.ObjectToString(Request["SelectedCategory"]);
                            resourceReportCtrl.dateFrom = UGITUtility.StringToDateTime(Request["DateFrom"]);
                            resourceReportCtrl.dateTo = UGITUtility.StringToDateTime(Request["DateTo"]);
                            resourceReportCtrl.startWith = UGITUtility.ObjectToString(Request["StartWith"]);
                            managementControls.Controls.Add(resourceReportCtrl);

                            //int PMMId = Convert.ToInt32(Request.QueryString["PMMId"]);
                            //ProjectResourceReport ProjectResourceReport = (ProjectResourceReport)this.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/ProjectResourceReport.ascx");
                            //ProjectResourceReport.PMMId = Convert.ToInt32(Request["PMMId"]);
                            //ProjectResourceReport.projectPublicID = Request["projectPublicID"];
                            //managementControls.Controls.Add(ProjectResourceReport);
                            //ResourceReport resourceReportCtrl = Page.LoadControl("~/CONTROLTEMPLATES/RMM/ResourceReport.ascx") as ResourceReport;
                            //resourceReportCtrl.viewType = UGITUtility.ObjectToString(Request["ViewType"]);
                            //resourceReportCtrl.reportType = UGITUtility.ObjectToString(Request["ReportType"]);
                            //resourceReportCtrl.unitAllocAct = UGITUtility.ObjectToString(Request["unitAllocAct"]);
                            //resourceReportCtrl.selectedCategory = UGITUtility.ObjectToString(Request["SelectedCategory"]);
                            //resourceReportCtrl.dateFrom = UGITUtility.StringToDateTime(Request["DateFrom"]);
                            //resourceReportCtrl.dateTo = UGITUtility.StringToDateTime(Request["DateTo"]);
                            //resourceReportCtrl.startWith = UGITUtility.ObjectToString(Request["StartWith"]);
                            //managementControls.Controls.Add(resourceReportCtrl);
                            break;
                        }
                    //case "scheduleaction":
                    //    int.TryParse(Request["Id"], out Id);
                    //    ScheduleActionControl scheduleAction = (ScheduleActionControl)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/ScheduleActionControl.ascx");
                    //    string actionType = Request["ActionType"];
                    //    if (actionType == "Report")
                    //    {
                    //        Dictionary<string, string> parameter = new Dictionary<string, string>();
                    //        foreach (var key in Request.QueryString.AllKeys)
                    //        {
                    //            if (key != null && key != "Id" && key != "control"
                    //                && key != "width" && key != "height")
                    //            {
                    //                parameter.Add(key, Request[key]);
                    //            }
                    //        }
                    //        scheduleAction.Parameter = parameter;
                    //    }
                    //    else
                    //    {
                    //        scheduleAction.DashboardPanelId = Id;
                    //    }
                    //    managementControls.Controls.Add(scheduleAction);
                    //    break;

                    case "servicequestioneditor":
                        ServiceQuestionEditor serviceQuestEditor = (ServiceQuestionEditor)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Services/ServiceQuestionEditor.ascx");
                        managementControls.Controls.Add(serviceQuestEditor);
                        break;
                    case "servicesectioneditor":
                        ServiceSectionEditor serviceSectionEditor = (ServiceSectionEditor)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Services/ServiceSectionEditor.ascx");
                        managementControls.Controls.Add(serviceSectionEditor);
                        break;

                    case "editservicetaskbranch":
                        ServiceTaskBranch serviceTaskBranch = (ServiceTaskBranch)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Services/ServiceTaskBranch.ascx");
                        managementControls.Controls.Add(serviceTaskBranch);
                        break;
                    case "editservicequestionbranch":
                        ServiceQuestionBranch serviceQuestionBranch = (ServiceQuestionBranch)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Services/ServiceQuestionBranch.ascx");
                        managementControls.Controls.Add(serviceQuestionBranch);
                        break;
                    
                    case "itgbudgetreport":
                        ITGBudgetReport itgBudgetReport = (ITGBudgetReport)this.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ITGBudgetReport.ascx");
                        managementControls.Controls.Add(itgBudgetReport);
                        break;
                    case "itgactualsreport":
                        ITGActualsReport itgActualReport = (ITGActualsReport)this.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ITGActualsReport.ascx");
                        managementControls.Controls.Add(itgActualReport);
                        break;
                    case "configvaribaleedit":
                        int clientAdminID;
                        int.TryParse(Request["ID"], out clientAdminID);
                        ConfigurationVariableListEdit configurationVariableListEdit = (ConfigurationVariableListEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ConfigurationVariableListEdit.ascx");
                        configurationVariableListEdit.clientAdminID = clientAdminID;
                        managementControls.Controls.Add(configurationVariableListEdit);
                        break;
                    case "requestedit":
                        int requestID;
                        string module;
                        int.TryParse(Request["ID"], out requestID);
                        module = Convert.ToString(Request["module"]);
                        RequestTypeListEdit requestTypeListEdit = (RequestTypeListEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/RequestTypeListEdit.ascx");
                        requestTypeListEdit.requestId = requestID;
                        requestTypeListEdit.module = module;
                        managementControls.Controls.Add(requestTypeListEdit);
                        break;
                    //case "prioritymapping":
                    //    int mappingID;
                    //    int.TryParse(Request["ID"], out mappingID);
                    //    PriorityMappingEdit PriorityMappingView = (PriorityMappingEdit)this.LoadControl("~/_CONTROLTEMPLATES/15/ListForm/PriorityMappingEdit.ascx");
                    //    managementControls.Controls.Add(PriorityMappingView);
                    //    break;
                    case "emailnotification":
                        int emailNotificationID;
                        int.TryParse(Request["ID"], out emailNotificationID);
                        EmailNotificationEdit emailNotificationEdit = (EmailNotificationEdit)this.LoadControl("~/CONTROLTEMPLATES/admin/ListForm/EmailNotificationEdit.ascx");
                        emailNotificationEdit.EmailNotificationID = emailNotificationID;
                        managementControls.Controls.Add(emailNotificationEdit);
                        break;
                    case "ticketimpact":
                        int ticketimpactID;
                        string mode = Request["mode"];
                        int.TryParse(Request["ItemID"], out ticketimpactID);
                        ImpactEdit ticketimpactEdit = (ImpactEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ImpactEdit.ascx");
                        ticketimpactEdit.TicketID = ticketimpactID;
                        ticketimpactEdit.Mode = (ViewMode)Enum.Parse(typeof(ViewMode), mode, true);
                        managementControls.Controls.Add(ticketimpactEdit);
                        break;
                    case "ticketimpactnew":
                        string mode1 = Request["mode"];
                        ImpactNew ticketimpactNew = (ImpactNew)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ImpactNew.ascx");
                        ticketimpactNew.Mode = (ViewMode)Enum.Parse(typeof(ViewMode), mode1, true);
                        managementControls.Controls.Add(ticketimpactNew);
                        break;
                    case "slaruleedit":
                        int _SLARuleID;
                        SLARulesEdit _SLARulesEdit = (SLARulesEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/SLARulesEdit.ascx");
                        int.TryParse(Request["ID"], out _SLARuleID);
                        _SLARulesEdit.SLARuleID = _SLARuleID;
                        managementControls.Controls.Add(_SLARulesEdit);
                        break;
                    case "slarulenew":
                        SLARulesNew _SLARulesNew = (SLARulesNew)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/SLARulesNew.ascx");
                        managementControls.Controls.Add(_SLARulesNew);
                        break;
                    case "escalationruledit":
                        int _SLAEscalationID;
                        SLAEscalationEdit _SLAEscalationEdit = (SLAEscalationEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/SLAEscalationEdit.ascx");
                        int.TryParse(Request["ID"], out _SLAEscalationID);
                        _SLAEscalationEdit.SLAEscalationID = _SLAEscalationID;
                        managementControls.Controls.Add(_SLAEscalationEdit);
                        break;
                    case "escalationrulenew":
                        SLAEscalationNew _SLAEscalationNew = (SLAEscalationNew)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/SLAEscalationNew.ascx");
                        managementControls.Controls.Add(_SLAEscalationNew);
                        break;
                    case "bgetcatnew":
                        BudgetCategoriesNew _BudgetCategoriesNew = (BudgetCategoriesNew)this.LoadControl("~/CONTROLTEMPLATES/admin/ListForm/BudgetCategoriesNew.ascx");
                        managementControls.Controls.Add(_BudgetCategoriesNew);
                        break;
                    case "bgetcatedit":
                        int _BudgetCategoryID;
                        BudgetCategoriesEdit _BudgetCategoriesEdit = (BudgetCategoriesEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/BudgetCategoriesEdit.ascx");
                        int.TryParse(Request["ID"], out _BudgetCategoryID);
                        _BudgetCategoriesEdit.BudgetCategoryID = _BudgetCategoryID;
                        managementControls.Controls.Add(_BudgetCategoriesEdit);
                        break;

                    case "projectclassnew":
                        ProjectClassNew _ProjectClassNew = (ProjectClassNew)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ProjectClassNew.ascx");
                        managementControls.Controls.Add(_ProjectClassNew);
                        break;
                    case "projectclassedit":
                        int _ProjectClassID;
                        ProjectClassEdit _ProjectClassEdit = (ProjectClassEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ProjectClassEdit.ascx");
                        int.TryParse(Request["ID"], out _ProjectClassID);
                        _ProjectClassEdit.Id = _ProjectClassID;
                        managementControls.Controls.Add(_ProjectClassEdit);
                        break;
                    case "projectinitnew":
                        ProjectInitNew _ProjectInitNew = (ProjectInitNew)this.LoadControl("~/CONTROLTEMPLATES/admin/ListForm/ProjectInitNew.ascx");
                        managementControls.Controls.Add(_ProjectInitNew);
                        break;
                    case "projectinitedit":
                        long _ProjectInitID;
                        //using same user control for both add and edit, in sharepoint have seperate controls
                        ProjectInitNew _ProjectInitEdit = (ProjectInitNew)this.LoadControl("~/CONTROLTEMPLATES/admin/ListForm/ProjectInitNew.ascx");
                        long.TryParse(Request["ID"], out _ProjectInitID);
                        _ProjectInitEdit.Id = _ProjectInitID;
                        managementControls.Controls.Add(_ProjectInitEdit);
                        break;
                    case "acrtypenew":
                        ACRTypeNew _ACRTypeNew = (ACRTypeNew)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ACRTypeNew.ascx");
                        managementControls.Controls.Add(_ACRTypeNew);
                        break;
                    case "acrtypeedit":
                        int _ACRTypeID;
                        ACRTypeEdit _ACRTypeEdit = (ACRTypeEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ACRTypeEdit.ascx");
                        int.TryParse(Request["ID"], out _ACRTypeID);
                        _ACRTypeEdit.Id = _ACRTypeID;
                        managementControls.Controls.Add(_ACRTypeEdit);
                        break;
                    case "drqsystemareasnew":
                        DRQSystemAreaNew _DRQSystemAreaNew = (DRQSystemAreaNew)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/DRQSystemAreaNew.ascx");
                        managementControls.Controls.Add(_DRQSystemAreaNew);
                        break;
                    case "drqsystemareasedit":
                        int _DRQSystemAreaID;
                        DRQSystemAreaEdit _DRQSystemAreaEdit = (DRQSystemAreaEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/DRQSystemAreaEdit.ascx");
                        int.TryParse(Request["ID"], out _DRQSystemAreaID);
                        _DRQSystemAreaEdit.Id = _DRQSystemAreaID;
                        managementControls.Controls.Add(_DRQSystemAreaEdit);
                        break;
                    case "userrolenew":
                        UserRoleTypeNew _UserRoleTypeNew = (UserRoleTypeNew)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/UserRoleTypeNew.ascx");
                        managementControls.Controls.Add(_UserRoleTypeNew);
                        break;
                    case "userroleedit":
                        int _UserRoleTypeID;
                        UserRoleTypeEdit _UserRoleTypeEdit = (UserRoleTypeEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/UserRoleTypeEdit.ascx");
                        int.TryParse(Request["ID"], out _UserRoleTypeID);
                        _UserRoleTypeEdit.Id = _UserRoleTypeID;
                        managementControls.Controls.Add(_UserRoleTypeEdit);
                        break;
                    case "drqrapidtypesnew":
                        DRQRapidTypesNew _DRQRapidTypesNew = (DRQRapidTypesNew)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/DRQRapidTypesNew.ascx");
                        managementControls.Controls.Add(_DRQRapidTypesNew);
                        break;
                    case "drqrapidtypesedit":
                        int _DRQRapidTypesID;
                        DRQRapidTypesEdit _DRQRapidTypesEdit = (DRQRapidTypesEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/DRQRapidTypesEdit.ascx");
                        int.TryParse(Request["ID"], out _DRQRapidTypesID);
                        _DRQRapidTypesEdit.Id = _DRQRapidTypesID;
                        managementControls.Controls.Add(_DRQRapidTypesEdit);
                        break;
                    case "messageboardedit":
                        int _MessageID;
                        MessageBoardEdit _MessageBoardEdit = (MessageBoardEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/MessageBoardEdit.ascx");
                        int.TryParse(Request["ID"], out _MessageID);
                        _MessageBoardEdit.Id = _MessageID;
                        managementControls.Controls.Add(_MessageBoardEdit);
                        break;

                    case "messageboardnew":
                        MessageBoardNew _MessageBoardNew = (MessageBoardNew)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/MessageBoardNew.ascx");
                        managementControls.Controls.Add(_MessageBoardNew);
                        break;
                    case "listpicker":
                        RelatedTicketPicker relatedTicketPicker = (RelatedTicketPicker)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/RelatedTicketPicker.ascx");
                        relatedTicketPicker.Module = Request["Module"] == null ? string.Empty : Convert.ToString(Request["Module"]);
                        relatedTicketPicker.Type = Request["Type"] == null ? string.Empty : Convert.ToString(Request["Type"]);
                        relatedTicketPicker.ParentTicketId = Request["TicketId"] == null ? string.Empty : Convert.ToString(Request["TicketId"]);
                        relatedTicketPicker.ParentModule = Request["ParentModule"] == null ? string.Empty : Convert.ToString(Request["ParentModule"]);
                        relatedTicketPicker.ControlId = Request["ControlId"] == null ? string.Empty : Convert.ToString(Request["ControlId"]);
                        relatedTicketPicker.Title = Request["Title"] == null ? string.Empty : Convert.ToString(Request["Title"]);
                        relatedTicketPicker.LookAhead = Request["lookahead"] == null ? false : true;
                        relatedTicketPicker.RequestType = Request["RequestType"] == null ? string.Empty : Convert.ToString(Request["RequestType"]);
                        relatedTicketPicker.CurrentModulePagePath = Request["CurrentModulePagePath"] == null ? string.Empty : Convert.ToString(Request["CurrentModulePagePath"]);
                        managementControls.Controls.Add(relatedTicketPicker);
                        break;

                    case "status":
                        {
                            Status statusCtl = (Status)Page.LoadControl("~/CONTROLTEMPLATES/Status.ascx");
                            statusCtl.tenantId = Request["tenantId"];
                            managementControls.Controls.Add(statusCtl);
                        }
                        break;

                    case "addworkitem":
                        AddWorkItems addWorkItems = (AddWorkItems)Page.LoadControl("~/CONTROLTEMPLATES/RMM/AddWorkItems.ascx");
                        addWorkItems.SelectedUsersListString = Request["SelectedUsersList"] == null ? string.Empty : Convert.ToString(Request["SelectedUsersList"]);
                        addWorkItems.Type = Request["Type"] == null ? string.Empty : Convert.ToString(Request["Type"]);
                        addWorkItems.StartDate = Request["StartDate"] == null ? DateTime.Now : Convert.ToDateTime(Request["StartDate"]);
                        addWorkItems.WorkItemType = Request["WorkItemType"];
                        addWorkItems.WorkItem = Request["WorkItem"];
                        managementControls.Controls.Add(addWorkItems);
                        break;
                    case "requesttypeloc":
                        RequestTypeByLocation _RequestTypeByLocation = (RequestTypeByLocation)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/RequestTypeByLocation.ascx");
                        _RequestTypeByLocation.Location = Request["Location"];
                        _RequestTypeByLocation.RequestTypeIDs = Request["RequestTypeID"];
                        _RequestTypeByLocation.ReqId = Convert.ToInt32(Request["ReqID"]);
                        _RequestTypeByLocation.Use24x7Calendar = Convert.ToBoolean(Request["Use24x7Calendar"]);
                        managementControls.Controls.Add(_RequestTypeByLocation);
                        break;
                    case "funcareanew":
                        FunctionalAreaNew _FunctionalAreaNew = (FunctionalAreaNew)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/FunctionalAreaNew.ascx");
                        managementControls.Controls.Add(_FunctionalAreaNew);
                        break;
                    case "funcareaedit":
                        int _FunctionalAreaID;
                        FunctionalAreaEdit _FunctionalAreaEdit = (FunctionalAreaEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/FunctionalAreaEdit.ascx");
                        int.TryParse(Request["ID"], out _FunctionalAreaID);
                        _FunctionalAreaEdit.Id = _FunctionalAreaID;
                        managementControls.Controls.Add(_FunctionalAreaEdit);
                        break;
                    
                    case "assetvendornew":
                        AssetVendorsNew _AssetVendorsNew = (AssetVendorsNew)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/AssetVendorsNew.ascx");
                        managementControls.Controls.Add(_AssetVendorsNew);
                        break;
                    case "assetvendoredit":
                        int _AssetVendorsID;
                        AssetVendorsEdit _AssetVendorsEdit = (AssetVendorsEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/AssetVendorsEdit.ascx");
                        int.TryParse(Request["ID"], out _AssetVendorsID);
                        _AssetVendorsEdit.Id = _AssetVendorsID;
                        managementControls.Controls.Add(_AssetVendorsEdit);
                        break;
                    case "assetmodelnew":
                        AssetModelsNew _AssetModelsNew = (AssetModelsNew)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/AssetModelsNew.ascx");
                        managementControls.Controls.Add(_AssetModelsNew);
                        break;
                    case "assetmodeledit":
                        int _AssetModelsID;
                        AssetModelsEdit _AssetModelsEdit = (AssetModelsEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/AssetModelsEdit.ascx");
                        int.TryParse(Request["ID"], out _AssetModelsID);
                        _AssetModelsEdit.Id = _AssetModelsID;
                        managementControls.Controls.Add(_AssetModelsEdit);
                        break;
                    case "modulenew":
                        ModuleNew _ModuleNew = (ModuleNew)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ModuleNew.ascx");
                        managementControls.Controls.Add(_ModuleNew);
                        break;
                    case "moduleedit":
                        int _ModuleID;
                        ModuleEdit _ModuleEdit = (ModuleEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ModuleEdit.ascx");
                        int.TryParse(Request["ID"], out _ModuleID);
                        _ModuleEdit.Id = _ModuleID;
                        managementControls.Controls.Add(_ModuleEdit);
                        break;
                    case "ganttreport":
                        string ganttType = Request.QueryString["GanttType"];
                        if (ganttType == "1")
                        {
                            //GanttView ganttReport = (GanttView)this.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/GanttView.ascx"); // NEW Report
                            ////GanttReport ganttReport = (GanttReport)this.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/GanttReport.ascx"); // OLD Report
                            //ganttReport.OpenProjectOnly = uHelper.StringToBoolean(Request.QueryString["OpenProjectOnly"]);
                            //ganttReport.GanttType = ganttType;
                            //ganttReport.Module = Request.QueryString["Module"];
                            //ganttReport.GroupBy = Request.QueryString["GroupBy"];
                            //managementControls.Controls.Add(ganttReport);
                        }
                        else if (ganttType == "2")
                        {
                            GanttReport ganttReport = (GanttReport)this.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Task/GanttReport.ascx");
                            ganttReport.TicketID = Request.QueryString["TicketID"];
                            ganttReport.Module = Request.QueryString["Module"];
                            ganttReport.GanttType = ganttType;
                            managementControls.Controls.Add(ganttReport);
                        }
                        break;
                    case "calendarview":
                        CustomCalendarView calendarView = (CustomCalendarView)this.LoadControl("~/CONTROLTEMPLATES/uGovernIT/CustomCalendarView.ascx");
                        calendarView.ModuleName = Request["ModuleName"];
                        managementControls.Controls.Add(calendarView);
                        break;
                    case "actuals":
                        CustomActualControl actualControl = (CustomActualControl)this.LoadControl("~/CONTROLTEMPLATES/uGovernIT/CustomActualControl.ascx");
                        int subcategoryId = 0;
                        int.TryParse(Request.QueryString["SubCategoryID"], out subcategoryId);
                        actualControl.IsProject = UGITUtility.StringToBoolean(Request.QueryString["IsProject"]);
                        actualControl.SubCategoryID = subcategoryId;
                        actualControl.StartDate = Convert.ToDateTime(Request.QueryString["StartDate"]);
                        actualControl.EndDate = Convert.ToDateTime(Request.QueryString["EndDate"]);
                        managementControls.Controls.Add(actualControl);
                        break;
                    case "budgets":
                        CustomBudgetControl budgetControl = (CustomBudgetControl)this.LoadControl("~/CONTROLTEMPLATES/uGovernIT/CustomBudgetControl.ascx");
                        int subcategoryIdOfBudget = 0;
                        int.TryParse(Request.QueryString["SubCategoryID"], out subcategoryIdOfBudget);
                        budgetControl.IsProject = UGITUtility.StringToBoolean(Request.QueryString["IsProject"]);
                        budgetControl.SubCategoryID = subcategoryIdOfBudget;
                        budgetControl.StartDate = Convert.ToDateTime(Request.QueryString["StartDate"]);
                        budgetControl.EndDate = Convert.ToDateTime(Request.QueryString["EndDate"]);
                        managementControls.Controls.Add(budgetControl);
                        break;
                    case "servicecategories":
                        ServiceCategories svcCategories = (ServiceCategories)this.LoadControl("~/CONTROLTEMPLATES/uGovernIT/services/ServiceCategories.ascx");
                        if (Request["categorytype"] != null && Convert.ToString(Request["categorytype"]) != string.Empty)
                            svcCategories.CategoryType = Convert.ToString(Request["categorytype"]);

                        managementControls.Controls.Add(svcCategories);
                        break;
                    case "servicecategoryeditor":
                        ServiceCategoryEditor sCategoryEditor = (ServiceCategoryEditor)this.LoadControl("~/CONTROLTEMPLATES/uGovernIT/services/ServiceCategoryEditor.ascx");
                        managementControls.Controls.Add(sCategoryEditor);
                        break;
                    //case "pickfromasset":
                    //    PickfromAsset sPickfromAsset = (PickfromAsset)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/PickfromAsset.ascx");
                    //    managementControls.Controls.Add(sPickfromAsset);
                    //    break;
                    //case "governance":
                    //    GovernanceCatalogue itgovernanceControl = (GovernanceCatalogue)this.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/GovernanceCatalogue.ascx");
                    //    managementControls.Controls.Add(itgovernanceControl);
                    //    break;
                    case "governanceconfigedit":
                        GovernanceConfiguratorEdit governanceConfigEdit = (GovernanceConfiguratorEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/GovernanceConfiguratorEdit.ascx");
                        governanceConfigEdit.itemID = Convert.ToString(Request["itemID"]).Trim();
                        governanceConfigEdit.categoryType = Request["categoryType"];
                        managementControls.Controls.Add(governanceConfigEdit);
                        break;
                    case "governanceconfigadd":
                        GovernanceConfiguratorAdd governanceConfigAdd = (GovernanceConfiguratorAdd)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/GovernanceConfiguratorAdd.ascx");
                        governanceConfigAdd.categoryType = Request["categoryType"];
                        managementControls.Controls.Add(governanceConfigAdd);
                        break;
                    case "governancecategoryedit":
                        GovernanceCategoryEdit governanceCategoryEdit = (GovernanceCategoryEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/GovernanceCategoryEdit.ascx");
                        governanceCategoryEdit.categoryID = Convert.ToString(Request["categoryID"]).Trim();
                        governanceCategoryEdit.categoryType = Request["categoryType"];
                        managementControls.Controls.Add(governanceCategoryEdit);
                        break;
                    case "governancecategoryadd":
                        GovernanceCategoryAdd governanceCategoryAdd = (GovernanceCategoryAdd)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/GovernanceCategoryAdd.ascx");
                        managementControls.Controls.Add(governanceCategoryAdd);
                        break;
                    //case "resourcereport":
                    //    PMMId = Convert.ToInt32(Request.QueryString["PMMId"]);
                    //    ProjectResourceReport ProjectResourceReport = (ProjectResourceReport)this.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/ProjectResourceReport.ascx");
                    //    ProjectResourceReport.PMMId = Convert.ToInt32(Request["PMMId"]);
                    //    ProjectResourceReport.projectPublicID = Request["projectPublicID"];
                    //    managementControls.Controls.Add(ProjectResourceReport);
                    //    break;
                    case "surveyratingquestioneditor":
                        SurveyRatingQuestionEditor rQuestionEditor = (SurveyRatingQuestionEditor)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Services/SurveyRatingQuestionEditor.ascx");
                        managementControls.Controls.Add(rQuestionEditor);
                        break;
                    case "serviceqmapvariableeditor":
                        ServiceQMapVariableEditor qMapEditor = (ServiceQMapVariableEditor)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Services/ServiceQMapVariableEditor.ascx");
                        managementControls.Controls.Add(qMapEditor);
                        break;
                    case "surveyfeedbackdetail":
                        SurveyFeedbackDetail surveyFeedbackDetail = (SurveyFeedbackDetail)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Services/SurveyFeedbackDetail.ascx");
                        managementControls.Controls.Add(surveyFeedbackDetail);
                        break;
                    case "applicationmodules":
                        ApplicationModulesCtrl applModulesCtrl = (ApplicationModulesCtrl)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ApplicationModulesCtrl.ascx");
                        int appId = 0;
                        int.TryParse(Request["ID"], out appId);
                        applModulesCtrl.ApplicationId = appId;
                        managementControls.Controls.Add(applModulesCtrl);
                        break;
                    //ApplicationModulesCtrl applModulesCtrl = (ApplicationModulesCtrl)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ApplicationModulesCtrl.ascx");
                    //int appId = 0;
                    //int.TryParse(Request["ID"], out appId);
                    //applModulesCtrl.ApplicationId = appId;
                    //managementControls.Controls.Add(applModulesCtrl);
                    //break;
                    case "modulesedit":
                        ApplicationModulesEdit applModulesEdit = (ApplicationModulesEdit)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ApplicationModulesEdit.ascx");
                        int applModules_id = 0;
                        int.TryParse(Request["ItemID"], out applModules_id);
                        int.TryParse(Request["AppId"], out appId);
                        applModulesEdit.id = applModules_id;
                        applModulesEdit.appid = appId;
                        managementControls.Controls.Add(applModulesEdit);
                        break;
                    //ApplicationModulesEdit applModulesEdit = (ApplicationModulesEdit)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ApplicationModulesEdit.ascx");
                    //int applModules_id = 0;
                    //int.TryParse(Request["ID"], out applModules_id);
                    //int.TryParse(Request["AppId"], out appId);
                    //applModulesEdit.id = applModules_id;
                    //applModulesEdit.appid = appId;
                    //managementControls.Controls.Add(applModulesEdit);
                    //break;
                    case "applicationrole":
                        ApplicationRoleCtrl applRoleCtrl = (ApplicationRoleCtrl)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ApplicationRoleCtrl.ascx");
                        int applId = 0;
                        int.TryParse(Request["ID"], out applId);
                        applRoleCtrl.ApplicationId = applId;
                        managementControls.Controls.Add(applRoleCtrl);
                        break;
                    case "approleedit":
                        ApplicationRoleEdit applRoleEdit = (ApplicationRoleEdit)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ApplicationRoleEdit.ascx");
                        int applRole_id = 0;
                        int.TryParse(Request["ItemID"], out applRole_id);
                        int.TryParse(Request["AppId"], out appId);
                        applRoleEdit.id = applRole_id;
                        applRoleEdit.appid = appId;
                        managementControls.Controls.Add(applRoleEdit);
                        break;
                    case "modulerolemapcontrol":
                        ApplModuleRoleMappingCtrl applModuleRoleMapCtrl = (ApplModuleRoleMappingCtrl)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ApplModuleRoleMappingCtrl.ascx");
                        int applMRId = 0;
                        int.TryParse(Request["ID"], out applMRId);
                        applModuleRoleMapCtrl.ApplicationId = applMRId;
                        if (!string.IsNullOrEmpty(Request["ticketID"]))
                        {
                            applModuleRoleMapCtrl.TicketId = Request["ticketID"];
                        }
                        managementControls.Controls.Add(applModuleRoleMapCtrl);
                        break;
                    case "modulerolemapedit":
                        ApplModuleRoleMapEdit applModuleRoleMapEdit = (ApplModuleRoleMapEdit)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ApplModuleRoleMapEdit.ascx");
                        int applModuleRoleMap_id = 0;
                        int.TryParse(Request["ID"], out applModuleRoleMap_id);
                        int applModuleRoleMap_appid = 0;
                        int.TryParse(Request["AppId"], out applModuleRoleMap_appid);
                        string applModuleRoleMap_userid = Convert.ToString(Request["UserId"]);
                        //string.TryParse(Request["UserId"], out applModuleRoleMap_userid);
                        applModuleRoleMapEdit.id = applModuleRoleMap_id;
                        applModuleRoleMapEdit.AppId = applModuleRoleMap_appid;
                        applModuleRoleMapEdit.userID = applModuleRoleMap_userid;
                        applModuleRoleMapEdit.AppMode = Request["Mode"];
                        managementControls.Controls.Add(applModuleRoleMapEdit);
                        break;
                    case "applicationserver":
                        ApplicationServerCtrl applServer = (ApplicationServerCtrl)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ApplicationServerCtrl.ascx");
                        int applServer_id = 0;
                        int.TryParse(Request["ID"], out applServer_id);
                        applServer.ApplicationId = applServer_id;
                        managementControls.Controls.Add(applServer);
                        break;
                    case "applicationpassword":
                        ApplicationPassword appPassword = (ApplicationPassword)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ApplicationPassword.ascx");
                        int AppId = 0;
                        int.TryParse(Request["ID"], out AppId);
                        appPassword.ApplicationId = AppId;
                        appPassword.TicketID = Request["ticketID"];
                        managementControls.Controls.Add(appPassword);
                        break;
                    case "creatapplicationpassword":
                        CreateApplicationPassword createPassword = (CreateApplicationPassword)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/CreateApplicationPassword.ascx");
                        int AppIdPswd = 0;
                        int.TryParse(Request["AppId"], out AppIdPswd);
                        int PasswordId = 0;
                        int.TryParse(Request["PasswordId"], out PasswordId);
                        createPassword.ApplicationId = AppIdPswd;
                        createPassword.Id = PasswordId;
                        createPassword.ticketID = Request["ticketID"];
                        managementControls.Controls.Add(createPassword);
                        break;

                    case "automateuserinfo":
                        AutomateUserInfo automateUserInfo = (AutomateUserInfo)Page.LoadControl("~/CONTROLTEMPLATES/RMM/AutomateUserInfo.ascx");
                        managementControls.Controls.Add(automateUserInfo);
                        break;
                    case "appserveredit":
                        ApplicationServerEdit applServerEdit = (ApplicationServerEdit)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ApplicationServerEdit.ascx");
                        int applEdit_id = 0;
                        int.TryParse(Request["ID"], out applEdit_id);
                        applServerEdit.id = applEdit_id;
                        int applEdit_appid = 0;
                        int.TryParse(Request["AppId"], out applEdit_appid);
                        applServerEdit.appid = applEdit_appid;
                        managementControls.Controls.Add(applServerEdit);
                        break;
                    case "environmentnew":
                        EnvironmentNew _EnvironmentNew = (EnvironmentNew)this.LoadControl("~/CONTROLTEMPLATES/uGovernIT/EnvironmentNew.ascx");
                        managementControls.Controls.Add(_EnvironmentNew);
                        break;
                    case "queryfilter":
                        QueryFilterAddEdit _QueryFilterAddEdit = (QueryFilterAddEdit)this.LoadControl("~/CONTROLTEMPLATES/uGovernIT/QueryFilterAddEdit.ascx");
                        int dashboardID;
                        int.TryParse(Request["dashboardID"], out dashboardID);
                        _QueryFilterAddEdit.dashboardID = dashboardID;

                        int filterID;
                        int.TryParse(Request["filterID"], out filterID);
                        _QueryFilterAddEdit.filterID = filterID;
                        managementControls.Controls.Add(_QueryFilterAddEdit);
                        break;
                    case "environmentedit":
                        long _EnvironmentID;
                        EnvironmentEdit _EnvironmentEdit = (EnvironmentEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/EnvironmentEdit.ascx");
                        long.TryParse(Request["ItemID"], out _EnvironmentID);
                        _EnvironmentEdit.Id = _EnvironmentID;
                        managementControls.Controls.Add(_EnvironmentEdit);
                        break;
                    case "wikilist":
                        WikiAdd wikiAdd = (WikiAdd)Page.LoadControl("~/CONTROLTEMPLATES/Wiki/WikiAdd.ascx");
                        wikiAdd.strAction = Request["action"];
                        wikiAdd.TicketId = Request["TicketId"];
                        if (!string.IsNullOrEmpty(Request["RelatedTicketId"]))
                        {
                            wikiAdd.RelatedTicketId = Request["RelatedTicketId"];
                        }
                        if (!string.IsNullOrEmpty(Request["ModuleId"]))
                        {
                            wikiAdd.ModuleId = Request["ModuleId"];
                        }
                        managementControls.Controls.Add(wikiAdd);
                        break;
                    case "wikilink":
                        WikiLink wikiLinks = (WikiLink)Page.LoadControl("~/CONTROLTEMPLATES/Wiki/WikiLink.ascx");
                        wikiLinks.TicketId = Request["TicketId"];
                        wikiLinks.width = Request["ControlWidth"];
                        managementControls.Controls.Add(wikiLinks);
                        break;
                    case "wikidetails":
                        WikiDetailCtrl wikiDetails = (WikiDetailCtrl)Page.LoadControl("~/CONTROLTEMPLATES/Wiki/WikiDetailCtrl.ascx");
                        managementControls.Controls.Add(wikiDetails);
                        managementControls.CssClass = "wikiArtical-popupWrap";
                        //wikiDetails.Controls.
                        wikiDetails.TicketId = Request["TicketId"];
                        //wikiDetails.isHelp = Request["isHelp"] == null ? false : Convert.ToBoolean(Request["isHelp"]);
                        break;

                    case "helpcarddisplay":
                        HelpCardDisplay helpCardDisplay = (HelpCardDisplay)Page.LoadControl("~/CONTROLTEMPLATES/HelpCard/HelpCardDisplay.ascx");
                        managementControls.Controls.Add(helpCardDisplay);
                        managementControls.CssClass = "wikiArtical-popupWrap";
                        helpCardDisplay.TicketId = Request["TicketId"];
                        break;

                    case "helpcard":
                        HelpCardAdd helpCardAdd = (HelpCardAdd)Page.LoadControl("~/CONTROLTEMPLATES/HelpCard/HelpCardAdd.ascx");
                        helpCardAdd.strAction = Request["action"];
                        helpCardAdd.TicketId = Request["TicketId"];
                        managementControls.Controls.Add(helpCardAdd);
                        break;

                    case "wikirelatedtickets":
                        WikiRelatedTickets wikiRelatedTickets = (WikiRelatedTickets)Page.LoadControl("~/ControlTemplates/uGovernIT/WikiRelatedTickets.ascx");
                        wikiRelatedTickets.CurrentTicketId = Request["ticketId"];
                        wikiRelatedTickets.ModuleName = Request["moduleName"];
                        managementControls.Controls.Add(wikiRelatedTickets);
                        break;
                    case "modulestagetask":
                        ModuleStageTask taskConstraint = (ModuleStageTask)Page.LoadControl("~/ControlTemplates/uGovernIT/ModuleStageTask.ascx");
                        taskConstraint.TicketPublicID = Request["ticketId"] != null ? Request["ticketId"].Trim() : string.Empty;
                        taskConstraint.ModuleName = Request["module"] != null ? Request["module"].Trim() : string.Empty;
                        taskConstraint.ModuleStageId = Request["moduleStage"] != null ? Request["moduleStage"].Trim() : string.Empty;
                        taskConstraint.ConstraintId = Request["taskID"] != null ? Convert.ToInt32(Request["taskID"].Trim()) : 0;
                        taskConstraint.Type = Request["type"] != null ? Request["type"].Trim() : string.Empty;
                        taskConstraint.isModuleConstraint = Request["isModuleConstraint"] != null ? Request["isModuleConstraint"].Trim() : "0";
                        //Added value to below variable to identify request coming from Summary view or DataAgent view // this is for ModuleStageTask
                        //if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(Request["IsReqFromSummary"])))
                        //{
                        //    taskConstraint.IsRequestFromSummaryOrTask = true;
                        //}
                        managementControls.Controls.Add(taskConstraint);
                        break;
                    case "preconditionlist":
                        ModuleConstraintsList preconditionsList = (ModuleConstraintsList)Page.LoadControl("~/ControlTemplates/uGovernIT/ModuleConstraintsList.ascx");
                        preconditionsList.TicketPublicID = Request["publicID"] != null ? Request["publicID"].Trim() : string.Empty;
                        preconditionsList.ModuleName = Request["ModuleName"] != null ? Request["ModuleName"].Trim() : string.Empty;
                        preconditionsList.ModuleStageId = Request["stageID"] != null ? Request["stageID"].Trim() : string.Empty;
                        managementControls.Controls.Add(preconditionsList);
                        break;
                    //case "moduletaskproposeadate":
                    //    ModuleConstraintProposeDate modulePDate = (ModuleConstraintProposeDate)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/ModuleConstraintProposeDate.ascx");
                    //    managementControls.Controls.Add(modulePDate);
                    //    break;

                    case "savetasktemplates":
                        SaveTaskTemplates taskTemplates = (SaveTaskTemplates)Page.LoadControl("~/ControlTemplates/uGovernIT/Task/SaveTaskTemplates.ascx");
                        managementControls.Controls.Add(taskTemplates);
                        break;
                    case "loadtasktemplate":
                        LoadTaskTemplate loadTaskTempate = (LoadTaskTemplate)Page.LoadControl("~/ControlTemplates/uGovernIT/Task/LoadTaskTemplate.ascx");
                        managementControls.Controls.Add(loadTaskTempate);
                        break;
                    case "pmmissuesedit":
                        PMMIssuesEdit issueEdit = (PMMIssuesEdit)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIt/PMMProject/PMMIssuesEdit.ascx");
                        issueEdit.ProjectID = Request["projectid"];
                        int taskID = UGITUtility.StringToInt(Request["taskID"]);
                        if (taskID > 0)
                            issueEdit.IssueID = taskID;
                        int viewType = UGITUtility.StringToInt(Request["viewtype"]);
                        if (viewType > 0)
                            issueEdit.ViewType = viewType;
                        managementControls.Controls.Add(issueEdit);
                        break;
                    case "pmmissuesnew":
                        PMMIssuesNew issueNew = (PMMIssuesNew)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIt/PMMProject/PMMIssuesNew.ascx");
                        issueNew.ProjectID = Request["projectid"];
                        managementControls.Controls.Add(issueNew);
                        break;
                    //case "pmmriskslist":
                    //    {
                    //        PMMRisksList vdnIssueList = (PMMRisksList)Page.LoadControl("~/_controltemplates/15/ListForm/PMMRisksList.ascx");
                    //        vdnIssueList.TicketID = Request["PublicTicketID"];
                    //        vdnIssueList.Module = Request["Module"];
                    //        vdnIssueList.ReadWriteID = Convert.ToInt32(Request["rwID"]);
                    //        managementControls.Controls.Add(vdnIssueList);

                    //    }
                    //    break;
                    case "pmmrisksedit":
                        {
                            PMMRisksEdit vdnRisk = (PMMRisksEdit)Page.LoadControl("~/controltemplates/ugovernit/PMMProject/PMMRisksEdit.ascx");
                            vdnRisk.TicketID = Request["projectID"];
                            vdnRisk.ItemID = Convert.ToInt32(Request["taskID"]);
                            managementControls.Controls.Add(vdnRisk);

                        }
                        break;
                    case "pmmrisksnew":
                        {
                            PMMRisksNew vdnRisk = (PMMRisksNew)Page.LoadControl("~/controltemplates/ugovernit/PMMProject/PMMRisksNew.ascx");
                            vdnRisk.TicketID = Request["projectID"];
                            managementControls.Controls.Add(vdnRisk);
                        }
                        break;
                    case "pmmdecisionlognew":
                        PMMDecisionLogNew decisionLogNew = (PMMDecisionLogNew)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIt/PMMProject/PMMDecisionLogNew.ascx");
                        decisionLogNew.ProjectID = Request["projectid"];
                        managementControls.Controls.Add(decisionLogNew);
                        break;
                    case "pmmdecisionlogedit":
                        PMMDecisionLogEdit decisionLogEdit = (PMMDecisionLogEdit)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIt/PMMProject/PMMDecisionLogEdit.ascx");
                        decisionLogEdit.ProjectID = Request["projectid"];
                        int decisionlogID = UGITUtility.StringToInt(Request["taskID"]);
                        if (decisionlogID > 0)
                            decisionLogEdit.decisionlogID = decisionlogID;
                        int decisionLogViewType = UGITUtility.StringToInt(Request["viewtype"]);
                        if (decisionLogViewType > 0)
                            decisionLogEdit.decisionLogViewType = decisionLogViewType;
                        managementControls.Controls.Add(decisionLogEdit);
                        break;
                    //case "pmmrisksreadview":
                    //    {
                    //        PMMRisksReadView vdnRisk = (PMMRisksReadView)Page.LoadControl("~/_controltemplates/15/ListForm/PMMRisksReadView.ascx");
                    //        vdnRisk.TicketID = Request["projectID"];
                    //        vdnRisk.ItemID = Convert.ToInt32(Request["taskID"]);
                    //        managementControls.Controls.Add(vdnRisk);

                    //    }
                    //    break;

                    case "edittask":
                        {
                            NameValueCollection queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                            string moduleName = Request["moduleName"];
                            Int32 parentTaskID = string.IsNullOrEmpty(Request["parentTaskID"]) ? 0 : Convert.ToInt32(Request["parentTaskID"]);
                            if (!string.IsNullOrEmpty(moduleName))
                            {
                                switch (moduleName)
                                {
                                    case Constants.PMMIssue:
                                        {
                                            if (parentTaskID > 0)
                                            {
                                                string listUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=pmmissuesnew"); ;
                                                queryCollection.Remove("control");
                                                listUrl = string.Format("{0}&{1}", listUrl, queryCollection.ToString());
                                                Context.Response.Redirect(listUrl);
                                            }
                                            else
                                            {
                                                string listUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=pmmissuesedit"); ;
                                                queryCollection.Remove("control");
                                                listUrl = string.Format("{0}&{1}", listUrl, queryCollection.ToString());
                                                Context.Response.Redirect(listUrl);
                                            }
                                        }
                                        break;
                                    case Constants.ExitCriteria:
                                        {
                                            string listUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=modulestagetask"); ;
                                            queryCollection.Remove("control");
                                            listUrl = string.Format("{0}&{1}", listUrl, queryCollection.ToString());
                                            Context.Response.Redirect(listUrl);
                                        }
                                        break;
                                    case Constants.VendorRisks:
                                        {
                                            string listUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=VendorRisksEdit"); ;
                                            queryCollection.Remove("control");
                                            listUrl = string.Format("{0}&{1}", listUrl, queryCollection.ToString());
                                            Context.Response.Redirect(listUrl);
                                        }
                                        break;
                                    case Constants.VendorIssue:
                                        {
                                            string listUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=vendorissueedit"); ;
                                            queryCollection.Remove("control");
                                            listUrl = string.Format("{0}&{1}", listUrl, queryCollection.ToString());
                                            Context.Response.Redirect(listUrl);
                                        }
                                        break;
                                    default:
                                        {
                                            string listUrl = UGITUtility.GetAbsoluteURL("/ControlTemplates/uGovernIt/Task/edittask.ascx"); ;
                                            queryCollection.Remove("control");
                                            listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
                                            Context.Response.Redirect(listUrl);
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    case "applicationaccessreportviewer":
                        ApplicationAccessReportViewer applicationAccessReportViewer = (ApplicationAccessReportViewer)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ApplicationAccessReportViewer.ascx");
                        int ApplicationId = 0;
                        int.TryParse(Request["ApplicationId"], out ApplicationId);
                        applicationAccessReportViewer.ApplicationId = ApplicationId;
                        if (!string.IsNullOrEmpty(Request["TicketId"]))
                        {
                            applicationAccessReportViewer.TicketId = Request["TicketId"];
                        }
                        managementControls.Controls.Add(applicationAccessReportViewer);
                        break;
                    //case "openticketreportviewer":
                    //    OpenTicketsReportViewer openTicketsReportViewer = (OpenTicketsReportViewer)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/OpenTicketsReportViewer.ascx");
                    //    if (!string.IsNullOrEmpty(Request["SelectedModule"]))
                    //    {
                    //        openTicketsReportViewer.SelectedModule = Request["SelectedModule"];
                    //    }
                    //    if (!string.IsNullOrEmpty(Request["SelectedType"]))
                    //    {
                    //        openTicketsReportViewer.SelectedType = Request["SelectedType"];
                    //    }
                    //    if (!string.IsNullOrEmpty(Request["SortType"]))
                    //    {
                    //        openTicketsReportViewer.SortType = Request["SortType"];
                    //    }
                    //    if (!string.IsNullOrEmpty(Request["IsModuleSort"]))
                    //    {
                    //        openTicketsReportViewer.IsModuleSort = Request["IsModuleSort"];
                    //    }

                    //    managementControls.Controls.Add(openTicketsReportViewer);
                    //    break;
                    case "showdashboardview":
                        {
                            ShowDashboardView dView = (ShowDashboardView)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/dashboard/ShowDashboardView.ascx");
                            int viewID = 0;
                            int.TryParse(Request["viewID"], out viewID);
                            string view = string.Empty;
                            if (viewID <= 0 && Request["view"] != null)
                                view = Request["view"].Trim();
                            dView.DashboardViewID = viewID;
                            dView.DashboardView = view;
                            managementControls.Controls.Add(dView);
                        }
                        break;

                    case "dashboardpreview":
                        {
                            DashboardPreview dView = (DashboardPreview)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/DashboardPreview.ascx");
                            int viewID = 0;
                            int.TryParse(Request["viewID"], out viewID);
                            string view = string.Empty;
                            if (viewID <= 0 && Request["view"] != null)
                                view = Request["view"].Trim();
                            dView.DashboardViewID = viewID;
                            dView.DashboardView = view;
                            managementControls.Controls.Add(dView);
                        }
                        break;
                    case "dashboarddesigner":
                        DashboardDesigner dashboardDesigner = (DashboardDesigner)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/DashboardDesigner.ascx");
                        managementControls.Controls.Add(dashboardDesigner);
                        break;
                    //case "ganttview":
                    //    GanttViewControl gvc = (GanttViewControl)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/GanttViewControl.ascx");
                    //    managementControls.Controls.Add(gvc);
                    //    break;
                    case "taskcalender":
                        CustomTaskCalendarControl calendarTaskView = (CustomTaskCalendarControl)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Task/CustomTaskCalendarControl.ascx");
                        calendarTaskView.ModuleName = Request["moduleName"];
                        calendarTaskView.ProjectPublicId = Request["projectPublicId"];
                        Session["CustomTaskListData"] = null;
                        //calendarTaskView.UserId = User.Id;
                        managementControls.Controls.Add(calendarTaskView);
                        break;

                    case "pmmstatusitem":
                        PMMAccomplishmentPlannedItem issueitem = (PMMAccomplishmentPlannedItem)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIt/PMMProject/PMMAccomplishmentPlannedItem.ascx");
                        issueitem.ProjectID = Request["projectid"];
                        issueitem.Itemtype = Request["itemType"];
                        issueitem.ItemId = Convert.ToInt32(Request["itemId"]);
                        issueitem.ViewType = Convert.ToInt16(Request["viewType"]);
                        managementControls.Controls.Add(issueitem);
                        break;

                    //case "resourceallocationsbyuser":
                    //    ResourceAllocationsByUser raByUser = (ResourceAllocationsByUser)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/ResourceAllocationsByUser.ascx");
                    //    managementControls.Controls.Add(raByUser);
                    //    break;

                    case "eventcategoryedit":
                        int _ID;
                        EventCategoryEdit _EventCategoryEdit = (EventCategoryEdit)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/EventCategoryEdit.ascx");
                        int.TryParse(Request["ID"], out _ID);
                        _EventCategoryEdit.Id = _ID;
                        managementControls.Controls.Add(_EventCategoryEdit);
                        break;
                    case "editspgroup":
                        EditSPGroup spGroupEdit = (EditSPGroup)this.LoadControl("~/CONTROLTEMPLATES/RMM/EditSPGroup.ascx");
                        managementControls.Controls.Add(spGroupEdit);
                        break;
                    case "projectcalendarview":
                        CalendarView _CalendarView = (CalendarView)this.LoadControl("~/CONTROLTEMPLATES/RMM/CalendarView.ascx");
                        _CalendarView.UserID = Request["selecteduser"];
                        managementControls.Controls.Add(_CalendarView);
                        break;
                    case "lifecyclestage":
                        ProjectLifeCycleStage _ProjectLifeCycleStage = (ProjectLifeCycleStage)this.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ProjectLifeCycleStage.ascx");
                        _ProjectLifeCycleStage.Id = Convert.ToInt32(Request["projectId"]);
                        _ProjectLifeCycleStage.ProjectPublicId = Request["publicTicketId"];
                        managementControls.Controls.Add(_ProjectLifeCycleStage);
                        break;
                    case "serviceimport":
                        ServiceImport serviceImport = (ServiceImport)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Services/ServiceImport.ascx");
                        managementControls.Controls.Add(serviceImport);
                        break;
                    case "documentcontrol":
                        DocumentControlView documentControl = (DocumentControlView)Page.LoadControl("~/controltemplates/uGovernIT/DocumentControlView.ascx");
                        //documentControl.docName = Request["docName"];
                        documentControl.TicketId = Request["ticketId"];
                        documentControl.ModuleName = Request["module"];
                        //documentControl.FolderId = Request["FolderId"];
                        managementControls.Controls.Add(documentControl);
                        break;



                    //case "pmmdocumentgrid":
                    //    PMMDocumentGrid documentGrid = (PMMDocumentGrid)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/PMMDocumentGrid.ascx");
                    //    documentGrid.docName = Request["docName"];
                    //    documentGrid.ModuleName = Request["ModuleName"];
                    //    managementControls.Controls.Add(documentGrid);
                    //    break;

                    //case "userorganizationchart":
                    //    UserOrganizationChart userOrgChart = (UserOrganizationChart)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/UserOrganizationChart.ascx");
                    //    managementControls.Controls.Add(userOrgChart);
                    //    break;

                    case "ticketemail":
                        AddTicketEmail ticketEmailControl = (AddTicketEmail)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/AddTicketEmail.ascx");
                        ticketEmailControl.publicTicketId = Request["currentTicketPublicID"];
                        ticketEmailControl.ModuleName = Request["ModuleName"];
                        ticketEmailControl.UsersId = Request["users"];
                        managementControls.Controls.Add(ticketEmailControl);
                        break;



                    case "wikinavigationview":
                        //int emailNotificationID;
                        //int.TryParse(Request["ID"], out emailNotificationID);
                        EditWikiNavigation _editwikinavigation = (EditWikiNavigation)this.LoadControl("~/ControlTemplates/uGovernIT/EditWikiNavigation.ascx");
                        _editwikinavigation.WikiID = Convert.ToInt32(Request["WikiId"]);
                        managementControls.Controls.Add(_editwikinavigation);
                        break;
                    case "wikipermission":
                        WikiPermissionControl wikipermissioncontrol = (WikiPermissionControl)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/WikiPermissionControl.ascx");
                        wikipermissioncontrol.TicketId = Request["currentTicketPublicID"];
                        managementControls.Controls.Add(wikipermissioncontrol);
                        break;
                    
                    case "reportuploadcontrol":
                        ReportUploadControl reportUploadControl = (ReportUploadControl)Page.LoadControl("~/controltemplates/uGovernIT/ReportUploadControl.ascx");
                        if (!string.IsNullOrEmpty(Request["DocName"]))
                        {
                            reportUploadControl.DocName = Request["DocName"];
                        }
                        if (!string.IsNullOrEmpty(Request["localpath"]))
                        {
                            reportUploadControl.localpath = Request["localpath"];
                        }
                        if (!string.IsNullOrEmpty(Request["relativepath"]))
                        {
                            reportUploadControl.relativepath = Request["relativepath"];
                        }
                        if (!string.IsNullOrEmpty(Request["folderid"]))
                        {
                            reportUploadControl.selectedFolderGuid = Request["folderid"];
                        }
                        managementControls.Controls.Add(reportUploadControl);
                        break;
                    case "resourceskillreport":
                        ResourceSkillReportControl resourceSkillReport = (ResourceSkillReportControl)Page.LoadControl("~/controltemplates/RMM/ResourceSkillReportControl.ascx");
                        resourceSkillReport.dateFrom = Convert.ToDateTime(Request["DateFrom"]);
                        resourceSkillReport.dateTo = Convert.ToDateTime(Request["DateTo"]);
                        resourceSkillReport.startWith = Request["StartWith"];
                        resourceSkillReport.viewType = Request["ViewType"];
                        resourceSkillReport.DrillDownTo = Request["DrillDownTo"];
                        resourceSkillReport.unitAllocAct = Request["unitAllocAct"];
                        managementControls.Controls.Add(resourceSkillReport);
                        break;

                    case "resourceerh":
                        ResourceERH resourceERH = (ResourceERH)Page.LoadControl("~/ControlTemplates/uGovernIT/ResourceERH.ascx");
                        managementControls.Controls.Add(resourceERH);
                        break;

                    case "tickettemplate":
                        Template ticketTemplate = (Template)Page.LoadControl("~/controltemplates/admin/listform/TicketTemplate.ascx");
                        ticketTemplate.Module = Convert.ToString(Request["ModuleName"]);
                        ticketTemplate.TicketId = Convert.ToString(Request["currentTicketPublicID"]);
                        int templateID = 0;
                        int.TryParse(Request["ItemID"], out templateID);
                        ticketTemplate.TemplateId = templateID;
                        managementControls.Controls.Add(ticketTemplate);
                        break;
                    case "tickettemplatelist":
                        TicketTemplateList ticketTemplatelist = (TicketTemplateList)Page.LoadControl("~/controltemplates/admin/listform/TicketTemplateList.ascx");
                        ticketTemplatelist.Module = Convert.ToString(Request["ModuleName"]);
                        //ticketTemplatelist.TicketId = Convert.ToString(Request["currentTicketPublicID"]);
                        managementControls.Controls.Add(ticketTemplatelist);
                        break;
                    //case "estimatedremaininghoursreport":
                    //    EstimatedRemainingHoursReport projectEstimatedRemainingHoursReport = (EstimatedRemainingHoursReport)this.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/EstimatedRemainingHoursReport.ascx");
                    //    projectEstimatedRemainingHoursReport.TicketId = Convert.ToInt32(Request["ProjectId"]);
                    //    projectEstimatedRemainingHoursReport.TicketPublicId = Request["TicketPublicId"];
                    //    managementControls.Controls.Add(projectEstimatedRemainingHoursReport);
                    //    break;
                    case "importtasktemplate":
                        ImportTaskTemplate importtask = (ImportTaskTemplate)Page.LoadControl("~/ControlTemplates/uGovernIT/Task/ImportTaskTemplate.ascx");
                        managementControls.Controls.Add(importtask);
                        break;
                    //case "investmentcontrol":
                    //    InvestmentControlView investmentcontrol = (InvestmentControlView)this.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/InvestmentControlView.ascx");
                    //    investmentcontrol.InvestorID = Convert.ToInt32(Request["InvestorID"]);
                    //    managementControls.Controls.Add(investmentcontrol);
                    //    break;
                    //case "distributioncontrol":
                    //    INVDistributionControl invDistributionControl = (INVDistributionControl)this.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/INVDistributionControl.ascx");
                    //    invDistributionControl.InvestorID = Convert.ToInt32(Request["InvestorID"]);

                    //    managementControls.Controls.Add(invDistributionControl);
                    //    break;
                    //case "vendorsowcontrol":
                    //    VendorSOWList sowview = (VendorSOWList)Page.LoadControl("~/_controltemplates/15/uGovernIT/VendorSOWList.ascx");
                    //    sowview.VNDID = Request["PublicTicketID"];
                    //    managementControls.Controls.Add(sowview);
                    //    break;
                    //case "vendorvcccontrol":
                    //    VendorVCCList vccView = (VendorVCCList)Page.LoadControl("~/_controltemplates/15/uGovernIT/VendorVCCList.ascx");
                    //    vccView.VNDVCCID = Request["PublicTicketID"];
                    //    managementControls.Controls.Add(vccView);
                    //    break;
                    //case "moduledocumentgridview":
                    //    ModuleDocumentGridview vccModuleDoc = (ModuleDocumentGridview)Page.LoadControl("~/_controltemplates/15/uGovernIT/ModuleDocumentGridview.ascx");
                    //    vccModuleDoc.ticketid = Request["PublicTicketID"];
                    //    vccModuleDoc.Module = Request["Module"];
                    //    vccModuleDoc.ReadWriteID = Convert.ToInt32(Request["rwID"]);
                    //    vccModuleDoc.HdnMandatoryCheck = hdnMandatoryCheck;
                    //    managementControls.Controls.Add(vccModuleDoc);
                    //    break;
                    //case "vendorfmcontrol":
                    //    {
                    //        VendorFMControl vfmControl = (VendorFMControl)Page.LoadControl("~/_controltemplates/15/uGovernIT/VendorFMControl.ascx");
                    //        vfmControl.TicketID = Request["PublicTicketID"];
                    //        vfmControl.Module = Request["Module"];
                    //        vfmControl.ReadWriteID = Convert.ToInt32(Request["rwID"]);
                    //        managementControls.Controls.Add(vfmControl);
                    //    }

                    //    break;
                    //case "vendorpmcontrol":
                    //    {
                    //        VendorPMControl vpmControl = (VendorPMControl)Page.LoadControl("~/_controltemplates/15/uGovernIT/VendorPMControl.ascx");
                    //        vpmControl.TicketID = Request["PublicTicketID"];
                    //        vpmControl.Module = Request["Module"];
                    //        vpmControl.ReadWriteID = Convert.ToInt32(Request["rwID"]);
                    //        managementControls.Controls.Add(vpmControl);
                    //    }
                    //    break;
                    //case "vendorsowfeelist":
                    //    {
                    //        VendorSOWFeeList sowFList = (VendorSOWFeeList)Page.LoadControl("~/_controltemplates/15/ListForm/VendorSOWFeeList.ascx");
                    //        sowFList.SOWTicketID = Request["PublicTicketID"];
                    //        sowFList.ReadWriteID = Convert.ToInt32(Request["rwID"]);
                    //        managementControls.Controls.Add(sowFList);

                    //    }
                    //    break;
                    //case "vendorsowfeeedit":
                    //    {
                    //        VendorSOWFeeEdit sowFEdit = (VendorSOWFeeEdit)Page.LoadControl("~/_controltemplates/15/ListForm/VendorSOWFeeEdit.ascx");
                    //        sowFEdit.SOWTicketID = Request["sowpublicid"];
                    //        sowFEdit.ItemID = Convert.ToInt32(Request["itemid"]);
                    //        managementControls.Controls.Add(sowFEdit);

                    //    }
                    //    break;
                    //case "vendorreportlist":
                    //    {
                    //        VendorReportList sowFList = (VendorReportList)Page.LoadControl("~/_controltemplates/15/ListForm/VendorReportList.ascx");
                    //        sowFList.TicketID = Request["PublicTicketID"];
                    //        sowFList.Module = Request["Module"];
                    //        sowFList.ReadWriteID = Convert.ToInt32(Request["rwID"]);
                    //        managementControls.Controls.Add(sowFList);

                    //    }
                    //    break;
                    //case "vendorreportedit":
                    //    {
                    //        VendorReportEdit sowFEdit = (VendorReportEdit)Page.LoadControl("~/_controltemplates/15/ListForm/VendorReportEdit.ascx");
                    //        sowFEdit.TicketID = Request["PublicTicketID"];
                    //        sowFEdit.Module = Request["Module"];
                    //        sowFEdit.ItemID = Convert.ToInt32(Request["itemid"]);
                    //        managementControls.Controls.Add(sowFEdit);

                    //    }
                    //    break;
                    //case "vendormeetinglist":
                    //    {
                    //        VendorMeetingList sowFList = (VendorMeetingList)Page.LoadControl("~/_controltemplates/15/ListForm/VendorMeetingList.ascx");
                    //        sowFList.TicketID = Request["PublicTicketID"];
                    //        sowFList.Module = Request["Module"];
                    //        sowFList.ReadWriteID = Convert.ToInt32(Request["rwID"]);
                    //        managementControls.Controls.Add(sowFList);

                    //    }
                    //    break;
                    //case "vendormeetingedit":
                    //    {
                    //        VendorMeetingEdit sowFEdit = (VendorMeetingEdit)Page.LoadControl("~/_controltemplates/15/ListForm/VendorMeetingEdit.ascx");
                    //        sowFEdit.TicketID = Request["PublicTicketID"];
                    //        sowFEdit.Module = Request["Module"];
                    //        sowFEdit.ItemID = Convert.ToInt32(Request["itemid"]);
                    //        managementControls.Controls.Add(sowFEdit);

                    //    }
                    //    break;
                    //case "vendorservicedurationlist":
                    //    {
                    //        VendorServiceDurationList sowFEdit = (VendorServiceDurationList)Page.LoadControl("~/_controltemplates/15/ListForm/VendorServiceDurationList.ascx");
                    //        sowFEdit.TicketID = Request["PublicTicketID"];
                    //        sowFEdit.Module = Request["Module"];
                    //        sowFEdit.ReadWriteID = Convert.ToInt32(Request["rwID"]);
                    //        managementControls.Controls.Add(sowFEdit);

                    //    }
                    //    break;
                    //case "vendorservicedurationedit":
                    //    {
                    //        VendorServiceDurationEdit sowFEdit = (VendorServiceDurationEdit)Page.LoadControl("~/_controltemplates/15/ListForm/VendorServiceDurationEdit.ascx");
                    //        sowFEdit.TicketID = Request["PublicTicketID"];
                    //        sowFEdit.Module = Request["Module"];
                    //        sowFEdit.ItemID = Convert.ToInt32(Request["itemid"]);
                    //        managementControls.Controls.Add(sowFEdit);

                    //    }
                    //    break;
                    //case "investmentedit":
                    //    InvestmentControlEdit investmentEdit = (InvestmentControlEdit)this.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/InvestmentControlEdit.ascx");
                    //    investmentEdit.InvestorId = Convert.ToInt32(Request["InvestorId"]);
                    //    investmentEdit.formMode = (FormMode)Enum.Parse(typeof(FormMode), Convert.ToString(Request["FormMode"]));
                    //    if (investmentEdit.formMode == FormMode.Edit)
                    //    {
                    //        investmentEdit.InvestmentId = Convert.ToInt32(Request["InvestmentId"]);
                    //    }
                    //    managementControls.Controls.Add(investmentEdit);

                    //    break;
                    //case "distributionedit":
                    //    INVDistributionControlEdit distributionEdit = (INVDistributionControlEdit)this.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/INVDistributionControlEdit.ascx");
                    //    distributionEdit.formMode = (FormMode)Enum.Parse(typeof(FormMode), Convert.ToString(Request["FormMode"]));
                    //    if (distributionEdit.formMode == FormMode.Edit)
                    //    {
                    //        distributionEdit.Id = Convert.ToInt32(Request["Id"]);
                    //    }
                    //    managementControls.Controls.Add(distributionEdit);
                    //    break;
                    //case "vendorsladashboard":
                    //    {
                    //        VendorSLADashboard slaDashboard = (VendorSLADashboard)Page.LoadControl("~/_controltemplates/15/uGovernIT/VendorSLADashboard.ascx");
                    //        slaDashboard.TicketID = Request["PublicTicketID"];
                    //        slaDashboard.Module = Request["Module"];
                    //        managementControls.Controls.Add(slaDashboard);

                    //    }
                    //    break;
                    //case "vendorcategoryedit":
                    //    {
                    //        VendorCategoryEdit vCategory = (VendorCategoryEdit)Page.LoadControl("~/_controltemplates/15/ListForm/VendorCategoryEdit.ascx");
                    //        vCategory.ItemID = Convert.ToInt32(Request["itemid"]);
                    //        managementControls.Controls.Add(vCategory);

                    //    }
                    //    break;
                    //case "vendorissuelist":
                    //    {
                    //        VendorIssueList vdnIssueList = (VendorIssueList)Page.LoadControl("~/_controltemplates/15/ListForm/VendorIssueList.ascx");
                    //        vdnIssueList.TicketID = Request["PublicTicketID"];
                    //        vdnIssueList.Module = Request["Module"];
                    //        vdnIssueList.ReadWriteID = Convert.ToInt32(Request["rwID"]);
                    //        managementControls.Controls.Add(vdnIssueList);

                    //    }
                    //    break;
                    //case "vendorissueedit":
                    //    {
                    //        VendorIssueEdit vdnIssueList = (VendorIssueEdit)Page.LoadControl("~/_controltemplates/15/ListForm/VendorIssueEdit.ascx");
                    //        vdnIssueList.TicketID = Request["projectID"];
                    //        vdnIssueList.IssueID = Convert.ToInt32(Request["taskID"]);
                    //        managementControls.Controls.Add(vdnIssueList);

                    //    }
                    //    break;
                    //case "vendorissuenew":
                    //    {
                    //        VendorIssueNew vdnIssueList = (VendorIssueNew)Page.LoadControl("~/_controltemplates/15/ListForm/VendorIssueNew.ascx");
                    //        vdnIssueList.TicketID = Request["projectID"];
                    //        managementControls.Controls.Add(vdnIssueList);

                    //    }
                    //    break;
                    //case "vendorriskslist":
                    //    {
                    //        VendorRisksList vdnIssueList = (VendorRisksList)Page.LoadControl("~/_controltemplates/15/ListForm/VendorRisksList.ascx");
                    //        vdnIssueList.TicketID = Request["PublicTicketID"];
                    //        vdnIssueList.Module = Request["Module"];
                    //        vdnIssueList.ReadWriteID = Convert.ToInt32(Request["rwID"]);
                    //        managementControls.Controls.Add(vdnIssueList);

                    //    }
                    //    break;
                    //case "vendorrisksedit":
                    //    {
                    //        VendorRisksEdit vdnRisk = (VendorRisksEdit)Page.LoadControl("~/_controltemplates/15/ListForm/VendorRisksEdit.ascx");
                    //        vdnRisk.TicketID = Request["projectID"];
                    //        vdnRisk.ItemID = uHelper.StringToInt(Request["taskID"]);
                    //        vdnRisk.ViewType = uHelper.StringToInt(Request["ViewType"]);
                    //        managementControls.Controls.Add(vdnRisk);

                    //    }
                    //    break;
                    //case "vendorrisksnew":
                    //    {
                    //        VendorRisksNew vdnRisk = (VendorRisksNew)Page.LoadControl("~/_controltemplates/15/ListForm/VendorRisksNew.ascx");
                    //        vdnRisk.TicketID = Request["projectID"];
                    //        managementControls.Controls.Add(vdnRisk);

                    //    }
                    //    break;
                    case "linkconfigadd":
                        LinkConfiguratorAdd _LinkConfiguratorAdd = (LinkConfiguratorAdd)this.LoadControl("~/ControlTemplates/Admin/ListForm/LinkConfiguratorAdd.ascx");
                        managementControls.Controls.Add(_LinkConfiguratorAdd);
                        break;
                    case "linkconfigedit":
                        LinkConfiguratorEdit _LinkConfiguratorEdit = (LinkConfiguratorEdit)this.LoadControl("~/ControlTemplates/Admin/ListForm/LinkConfiguratorEdit.ascx");
                        _LinkConfiguratorEdit.linkItemID = Convert.ToString(Request["linkItemID"]).Trim();
                        managementControls.Controls.Add(_LinkConfiguratorEdit);
                        break;
                    case "linkcategoryedit":
                        LinkCategoryEdit _LinkCategoryEdit = (LinkCategoryEdit)this.LoadControl("~/ControlTemplates/Admin/ListForm/LinkCategoryEdit.ascx");
                        _LinkCategoryEdit.linkCategoryID = Convert.ToString(Request["linkCategoryID"]).Trim();
                        managementControls.Controls.Add(_LinkCategoryEdit);
                        break;
                    //case "vendormeetingreadview":
                    //    {
                    //        VendorMeetingReadView sowFEdit = (VendorMeetingReadView)Page.LoadControl("~/_controltemplates/15/ListForm/VendorMeetingReadView.ascx");
                    //        sowFEdit.TicketID = Request["PublicTicketID"];
                    //        sowFEdit.Module = Request["Module"];
                    //        sowFEdit.ItemID = Convert.ToInt32(Request["itemid"]);
                    //        managementControls.Controls.Add(sowFEdit);
                    //    }
                    //    break;
                    //case "vendorissuereadview":
                    //    {
                    //        VendorIssueReadView vdnIssueList = (VendorIssueReadView)Page.LoadControl("~/_controltemplates/15/ListForm/VendorIssueReadView.ascx");
                    //        vdnIssueList.TicketID = Request["projectID"];
                    //        vdnIssueList.IssueID = Convert.ToInt32(Request["taskID"]);
                    //        managementControls.Controls.Add(vdnIssueList);

                    //    }
                    //    break;
                    //case "vendorservicedurationreadview":
                    //    {
                    //        VendorServiceDurationReadView sowFEdit = (VendorServiceDurationReadView)Page.LoadControl("~/_controltemplates/15/ListForm/VendorServiceDurationReadView.ascx");
                    //        sowFEdit.TicketID = Request["PublicTicketID"];
                    //        sowFEdit.Module = Request["Module"];
                    //        sowFEdit.ItemID = Convert.ToInt32(Request["itemid"]);
                    //        managementControls.Controls.Add(sowFEdit);

                    //    }
                    //    break;
                    //case "vendorrisksreadview":
                    //    {
                    //        VendorRisksReadView vdnRisk = (VendorRisksReadView)Page.LoadControl("~/_controltemplates/15/ListForm/VendorRisksReadView.ascx");
                    //        vdnRisk.TicketID = Request["projectID"];
                    //        vdnRisk.ItemID = Convert.ToInt32(Request["taskID"]);
                    //        managementControls.Controls.Add(vdnRisk);

                    //    }
                    //    break;
                    //case "vendorsowfeereadview":
                    //    {
                    //        VendorSOWFeeReadView sowFEdit = (VendorSOWFeeReadView)Page.LoadControl("~/_controltemplates/15/ListForm/VendorSOWFeeReadView.ascx");
                    //        sowFEdit.SOWTicketID = Request["sowpublicid"];
                    //        sowFEdit.ItemID = Convert.ToInt32(Request["itemid"]);
                    //        managementControls.Controls.Add(sowFEdit);

                    //    }
                    //    break;
                    //case "vendorreportreadview":
                    //    {
                    //        VendorReportReadView sowRead = (VendorReportReadView)Page.LoadControl("~/_controltemplates/15/ListForm/VendorReportReadView.ascx");
                    //        sowRead.TicketID = Request["PublicTicketID"];
                    //        sowRead.Module = Request["Module"];
                    //        sowRead.ItemID = Convert.ToInt32(Request["itemid"]);
                    //        managementControls.Controls.Add(sowRead);

                    //    }
                    //    break;
                    //case "vendorapprovedsubcontractorslist":
                    //    {
                    //        VendorApprovedSubContractorsList vdnContrt = (VendorApprovedSubContractorsList)Page.LoadControl("~/_controltemplates/15/ListForm/VendorApprovedSubContractorsList.ascx");
                    //        vdnContrt.TicketID = Request["PublicTicketID"];
                    //        vdnContrt.ReadWriteID = Convert.ToInt32(Request["rwID"]);
                    //        managementControls.Controls.Add(vdnContrt);

                    //    }
                    //    break;
                    //case "vendorapprovedsubcontractorsedit":
                    //    {
                    //        VendorApprovedSubContractorsEdit editControl = (VendorApprovedSubContractorsEdit)Page.LoadControl("~/_controltemplates/15/ListForm/VendorApprovedSubContractorsEdit.ascx");
                    //        editControl.TicketID = Request["projectID"];
                    //        editControl.ItemID = Convert.ToInt32(Request["taskID"]);
                    //        managementControls.Controls.Add(editControl);

                    //    }
                    //    break;
                    //case "vendorapprovedsubcontractorsnew":
                    //    {
                    //        VendorApprovedSubContractorsNew vdnContrt = (VendorApprovedSubContractorsNew)Page.LoadControl("~/_controltemplates/15/ListForm/VendorApprovedSubContractorsNew.ascx");
                    //        vdnContrt.TicketID = Request["projectID"];
                    //        managementControls.Controls.Add(vdnContrt);

                    //    }
                    //    break;
                    //case "vendorapprovedsubcontractorsreadview":
                    //    {
                    //        VendorApprovedSubContractorsReadView viewControl = (VendorApprovedSubContractorsReadView)Page.LoadControl("~/_controltemplates/15/ListForm/VendorApprovedSubContractorsReadView.ascx");
                    //        viewControl.TicketID = Request["projectID"];
                    //        viewControl.ItemID = Convert.ToInt32(Request["taskID"]);
                    //        managementControls.Controls.Add(viewControl);

                    //    }
                    //    break;
                    //case "vendorkeypersonnellist":
                    //    {
                    //        VendorKeyPersonnelList listControl = (VendorKeyPersonnelList)Page.LoadControl("~/_controltemplates/15/ListForm/VendorKeyPersonnelList.ascx");
                    //        listControl.TicketID = Request["PublicTicketID"];
                    //        listControl.Module = Request["Module"];
                    //        listControl.ReadWriteID = Convert.ToInt32(Request["rwID"]);
                    //        managementControls.Controls.Add(listControl);

                    //    }
                    //    break;
                    //case "vendorkeypersonneledit":
                    //    {
                    //        VendorKeyPersonnelEdit editControl = (VendorKeyPersonnelEdit)Page.LoadControl("~/_controltemplates/15/ListForm/VendorKeyPersonnelEdit.ascx");
                    //        editControl.TicketID = Request["projectID"];
                    //        editControl.ItemID = Convert.ToInt32(Request["taskID"]);
                    //        managementControls.Controls.Add(editControl);

                    //    }
                    //    break;
                    //case "vendorkeypersonnelnew":
                    //    {
                    //        VendorKeyPersonnelNew newControl = (VendorKeyPersonnelNew)Page.LoadControl("~/_controltemplates/15/ListForm/VendorKeyPersonnelNew.ascx");
                    //        newControl.TicketID = Request["projectID"];
                    //        managementControls.Controls.Add(newControl);

                    //    }
                    //    break;
                    //case "vendorkeypersonnelreadview":
                    //    {
                    //        VendorKeyPersonnelRead viewControl = (VendorKeyPersonnelRead)Page.LoadControl("~/_controltemplates/15/ListForm/VendorKeyPersonnelRead.ascx");
                    //        viewControl.TicketID = Request["projectID"];
                    //        viewControl.ItemID = Convert.ToInt32(Request["taskID"]);
                    //        managementControls.Controls.Add(viewControl);

                    //    }
                    //    break;
                    //case "vendorsowcontimprovementlist":
                    //    {
                    //        VendorSOWContImprovementList vdnContrt = (VendorSOWContImprovementList)Page.LoadControl("~/_controltemplates/15/ListForm/VendorSOWContImprovementList.ascx");
                    //        vdnContrt.TicketID = Request["PublicTicketID"];
                    //        vdnContrt.ReadWriteID = Convert.ToInt32(Request["rwID"]);
                    //        managementControls.Controls.Add(vdnContrt);

                    //    }
                    //    break;
                    //case "vendorsowcontimprovementedit":
                    //    {
                    //        VendorSOWContImprovementEdit editControl = (VendorSOWContImprovementEdit)Page.LoadControl("~/_controltemplates/15/ListForm/VendorSOWContImprovementEdit.ascx");
                    //        editControl.TicketID = Request["projectID"];
                    //        editControl.ItemID = Convert.ToInt32(Request["taskID"]);
                    //        managementControls.Controls.Add(editControl);

                    //    }
                    //    break;
                    //case "vendorsowcontimprovementnew":
                    //    {
                    //        VendorSOWContImprovementNew vdnContrt = (VendorSOWContImprovementNew)Page.LoadControl("~/_controltemplates/15/ListForm/VendorSOWContImprovementNew.ascx");
                    //        vdnContrt.TicketID = Request["projectID"];
                    //        managementControls.Controls.Add(vdnContrt);

                    //    }
                    //    break;
                    //case "vendorsowcontimprovementreadview":
                    //    {
                    //        VendorSOWContImprovementReadView viewControl = (VendorSOWContImprovementReadView)Page.LoadControl("~/_controltemplates/15/ListForm/VendorSOWContImprovementReadView.ascx");
                    //        viewControl.TicketID = Request["projectID"];
                    //        viewControl.ItemID = Convert.ToInt32(Request["taskID"]);
                    //        managementControls.Controls.Add(viewControl);

                    //    }
                    //    break;
                    case "batchcreate":
                        BatchCreateWizard _BatchCreateWizard = (BatchCreateWizard)this.LoadControl("~/ControlTemplates/uGovernIT/BatchCreateWizard.ascx");
                        // _BatchCreateWizard.linkCategoryID = Convert.ToString(Request["linkCategoryID"]).Trim();
                        _BatchCreateWizard.ModuleName = Request["moduleName"];
                        _BatchCreateWizard.ID = "batchCreate";
                        managementControls.Controls.Add(_BatchCreateWizard);
                        break;
                    //case "proposallistcontrol":
                    //    ProposalListControl _ProposalListControl = (ProposalListControl)this.LoadControl("~/_ControlTemplates/15/uGovernIT/ProposalListControl.ascx");
                    //    int custid;
                    //    int.TryParse(Request["CustomerId"], out custid);
                    //    _ProposalListControl.CustomerId = custid;
                    //    managementControls.Controls.Add(_ProposalListControl);
                    //    break;
                    case "skillreport":
                        //SkillReport _SkillReport = (SkillReport)this.LoadControl("~/ControlTemplates/RMM/SkillReport.ascx");
                        //managementControls.Controls.Add(_SkillReport);
                        break;

                    case "assetrelatedwithassets":

                        //AssetRelatedWithAssets assetRelatedWithAssets = (AssetRelatedWithAssets)Page.LoadControl("~/_ControlTemplates/15/uGovernIT/AssetRelatedWithAssets.ascx");
                        AssetRelatedWithAssets assetRelatedWithAssets = (AssetRelatedWithAssets)Page.LoadControl("~/ControlTemplates/uGovernIT/AssetRelatedWithAssets.ascx");
                        //assetRelatedWithAssets.LoadData = true;
                        managementControls.Controls.Add(assetRelatedWithAssets);
                        break;

                    case "assetrelatedwithtickets":
                        //AssetRelatedWithTickets assetRealtedWithTickets = (AssetRelatedWithTickets)Page.LoadControl("~/_ControlTemplates/15/uGovernIT/AssetRelatedWithTickets.ascx");
                        AssetRelatedWithTickets assetRealtedWithTickets = (AssetRelatedWithTickets)Page.LoadControl("~/ControlTemplates/uGovernIT/AssetRelatedWithTickets.ascx");
                        //assetRealtedWithTickets.LoadData = true;
                        managementControls.Controls.Add(assetRealtedWithTickets);
                        break;

                    //case "imageoptionusercontrol":
                    //    ImageOptionUserControl imageOptionUserControl = (ImageOptionUserControl)Page.LoadControl("~/_ControlTemplates/15/uGovernIT/ImageOptionUserControl.ascx");
                    //    managementControls.Controls.Add(imageOptionUserControl);
                    //    break;
                    //case "assetsstatususercontrol":
                    //    AssetsStatusUserControl assetStatusUserControl = (AssetsStatusUserControl)Page.LoadControl("~/_ControlTemplates/15/uGovernIT/AssetsStatusUserControl.ascx");
                    //    managementControls.Controls.Add(assetStatusUserControl);
                    //    break;
                    case "importexcelfile":
                        ImportExcelFile _ImportExcelFile = (ImportExcelFile)Page.LoadControl("~/ControlTemplates/Admin/ListForm/ImportExcelFile.ascx");
                        managementControls.Controls.Add(_ImportExcelFile);
                        break;
                    case "changepassword":
                        ChangePassword changePassword = (ChangePassword)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ChangePassword.ascx");
                        managementControls.Controls.Add(changePassword);
                        break;

                    case "image":
                        {
                            ImageControl uploadF = (ImageControl)Page.LoadControl("~/controltemplates/ugovernit/ImageControl.ascx");
                            uploadF.FileId = Request.QueryString["id"];
                            uploadF.FileName = "";
                            uploadF.Action = Request.QueryString["action"];
                            uploadF.Extension = Request.QueryString["extension"];
                            managementControls.Controls.Add(uploadF);
                        }
                        break;


                    case "moduledefaultsedit":
                        int _ModuleDefaultID = 0;
                        ModuleDefaultsEdit _ModuleDefaultsEdit = (ModuleDefaultsEdit)this.LoadControl("~/CONTROLTEMPLATES/admin/ListForm/ModuleDefaultsEdit.ascx");
                        int.TryParse(Request["ID"], out _ModuleDefaultID);
                        _ModuleDefaultsEdit.Id = _ModuleDefaultID;
                        _ModuleDefaultsEdit.ModuleName = Request["ModuleName"].Trim();
                        managementControls.Controls.Add(_ModuleDefaultsEdit);
                        break;
                    case "workflowbottleneck":
                        WorkflowBottleneck workflowBottleneck = (WorkflowBottleneck)Page.LoadControl("~/controltemplates/uGovernIT/WorkflowBottleneck.ascx");
                        workflowBottleneck.ModuleName = Request["moduleName"].Trim();
                        workflowBottleneck.ModuleTitle = Request["moduletitle"].Trim();
                        managementControls.Controls.Add(workflowBottleneck);
                        break;
                    case "moduleresourceaddedit":
                        ModuleResourceAddEdit _NPRResourceAddEdit = (ModuleResourceAddEdit)Page.LoadControl("~/ControlTemplates/uGovernIT/Budget/ModuleResourceAddEdit.ascx");
                        _NPRResourceAddEdit.TicketID = Request["TicketID"].Trim();
                        _NPRResourceAddEdit.ModuleName = Request["ModuleName"].Trim();
                        managementControls.Controls.Add(_NPRResourceAddEdit);
                        break;
                    case "ticketreassignment":
                        TicketReAssignment _TicketReAssignment = (TicketReAssignment)Page.LoadControl("~/ControlTemplates/uGovernIT/TicketReAssignment.ascx");
                        _TicketReAssignment.TicketId = Request["ids"].Trim();
                        managementControls.Controls.Add(_TicketReAssignment);
                        break;
                    case "manualescalation":
                        ManualEscalation _ManualEscalation = (ManualEscalation)Page.LoadControl("~/ControlTemplates/uGovernIT/ManualEscalation.ascx");
                        if (Request["ids"] != null)
                            _ManualEscalation.TicketId = Request["ids"].Trim();
                        _ManualEscalation.ModuleName = Request["ModuleName"].Trim();
                        managementControls.Controls.Add(_ManualEscalation);
                        break;

                    case "servicetaskworkflow":
                        ServiceTaskWorkFlow _servicetaskworkfLow = (ServiceTaskWorkFlow)Page.LoadControl("~/ControlTemplates/ServiceTaskWorkFlow.ascx");
                        if (Request["TicketId"] != null)
                            _servicetaskworkfLow.TicketId = Request["TicketId"].Trim();
                        _servicetaskworkfLow.ModuleName = Request["ModuleName"].Trim();
                        managementControls.Controls.Add(_servicetaskworkfLow);
                        break;

                    case "ticketcloseorreject":
                        TicketCloseOrReject _TicketCloseOrReject = (TicketCloseOrReject)Page.LoadControl("~/ControlTemplates/uGovernIT/TicketCloseOrReject.ascx");
                        _TicketCloseOrReject.TicketId = Request["ids"].Trim();
                        managementControls.Controls.Add(_TicketCloseOrReject);
                        break;
                    case "ticketreopen":
                        TicketReOpen _TicketReOpen = (TicketReOpen)Page.LoadControl("~/controltemplates/uGovernIT/TicketReOpen.ascx");
                        _TicketReOpen.TicketIds = Request["ids"].Trim();
                        managementControls.Controls.Add(_TicketReOpen);
                        break;
                    
                    case "ticketaction":
                        TicketAction _TicketAction = (TicketAction)Page.LoadControl("~/controltemplates/uGovernIT/TicketAction.ascx");
                        _TicketAction.ModuleName = Request["moduleName"].Trim();
                        _TicketAction.TicketCommand = Request["command"].Trim();
                        _TicketAction.TicketPublicID = Request["ticketPublicId"].Trim();
                        managementControls.Controls.Add(_TicketAction);
                        break;
                    

                    case "projectcard":
                        ProjectCard _ProjectCard = (ProjectCard)Page.LoadControl("~/controltemplates/uGovernIT/ProjectCard.ascx");
                        _ProjectCard.TicketIds = Convert.ToString(Request["CompanyTicketId"]);
                        managementControls.Controls.Add(_ProjectCard);
                        break;

                    case "queryexpression":
                        QueryExpression _QueryExpression = (QueryExpression)Page.LoadControl("~/controltemplates/uGovernIT/QueryExpression.ascx");
                        _QueryExpression.DashboardID = UGITUtility.StringToLong(Request["dashboardID"]);
                        managementControls.Controls.Add(_QueryExpression);
                        break;
                    //case "organizationaddedit":
                    //    int orgId = 0;
                    //    OrganizationAddEdit _OrganizationAddEdit = (OrganizationAddEdit)Page.LoadControl("~/_controltemplates/15/ListForm/OrganizationAddEdit.ascx");
                    //    int.TryParse(Request["ID"], out orgId);
                    //    _OrganizationAddEdit.Id = orgId;
                    //    managementControls.Controls.Add(_OrganizationAddEdit);
                    //    break;
                    //case "contactsaddedit":
                    //    int contactId = 0;
                    //    ContactAddEdit _ContactAddEdit = (ContactAddEdit)Page.LoadControl("~/_controltemplates/15/ListForm/ContactAddEdit.ascx");
                    //    int.TryParse(Request["ID"], out contactId);
                    //    _ContactAddEdit.Id = contactId;
                    //    managementControls.Controls.Add(_ContactAddEdit);
                    //    break;
                    //case "resourceaddedit":
                    //    ResourceAddEdit _ResourceAddEdit = (ResourceAddEdit)Page.LoadControl("~/_controltemplates/15/uGovernIT/ResourceAddEdit.ascx");
                    //    managementControls.Controls.Add(_ResourceAddEdit);
                    //    break;
                    //case "resourceplan":
                    //    ResourcePlan _ResourcePlan = (ResourcePlan)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/ResourcePlan.ascx");
                    //    _ResourcePlan.ticketID = Request["ticketId"];
                    //    _ResourcePlan.FrameId = frameId;
                    //    _ResourcePlan.ReadOnly = isReadOnly;
                    //    managementControls.Controls.Add(_ResourcePlan);
                    //    break;
                    //case "bid":
                    //    BidView _BidView = (BidView)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/BidView.ascx");
                    //    _BidView.ticketID = Request["ticketId"];
                    //    _BidView.FrameId = frameId;
                    //    _BidView.ReadOnly = isReadOnly;
                    //    managementControls.Controls.Add(_BidView);
                    //    break;
                    //case "bidaddedit":
                    //    int bidId = 0;
                    //    BidAddEdit _BidAddEdit = (BidAddEdit)Page.LoadControl("~/_controltemplates/15/uGovernIT/BidAddEdit.ascx");
                    //    int.TryParse(Request["ID"], out bidId);
                    //    _BidAddEdit.Id = bidId;
                    //    _BidAddEdit.ticketID = Request["ticketId"];
                    //    managementControls.Controls.Add(_BidAddEdit);
                    //    break;

                    case "crmprojectallocationaddedit":
                        int allocId = 0;
                        CRMProjectAllocationAddEdit _CRMProjectAllocationAddEdit = (CRMProjectAllocationAddEdit)Page.LoadControl("~/controltemplates/ugovernit/CRMProjectAllocationAddEdit.ascx");
                        int.TryParse(Request["ID"], out allocId);
                        _CRMProjectAllocationAddEdit.Id = allocId;
                        _CRMProjectAllocationAddEdit.ticketID = Request["ticketId"];
                        managementControls.Controls.Add(_CRMProjectAllocationAddEdit);
                        break;

                    case "viewsubcontractor":
                        ViewSubContractor _ViewSubContractor = (ViewSubContractor)Page.LoadControl("~/controltemplates/ugovernit/ViewSubContractor.ascx");
                        _ViewSubContractor.ModuleName = Convert.ToString(Request["module"]);
                        _ViewSubContractor.PublicTicketID = Request["ticketId"];
                        managementControls.Controls.Add(_ViewSubContractor);
                        break;


                    case "findresourceavailability":
                        FindResourceAvailability _FindResourceAvailability = (FindResourceAvailability)Page.LoadControl("~/controltemplates/ugovernit/FindResourceAvailability.ascx");

                        ////_FindResourceAvailability.TicketId = Convert.ToString(Request["projectPublicID"]);
                        ////_FindResourceAvailability.ControlId = Convert.ToString(Request["ControlId"]);
                        ////_FindResourceAvailability.FrameId = frameId;
                        ////_FindResourceAvailability.ReadOnly = isReadOnly;

                        managementControls.Controls.Add(_FindResourceAvailability);
                        break;



                    case "crmprojectallocation":
                        CRMProjectAllocationView _CRMProjectAllocationView = (CRMProjectAllocationView)Page.LoadControl("~/controltemplates/ugovernit/CRMProjectAllocationView.ascx");
                        _CRMProjectAllocationView.ticketID = Request["ticketId"];
                        _CRMProjectAllocationView.FrameId = frameId;
                        _CRMProjectAllocationView.ReadOnly = isReadOnly;
                        _CRMProjectAllocationView.ModuleName = Request["module"];
                        managementControls.Controls.Add(_CRMProjectAllocationView);
                        break;

                    case "crmprojectallocationnew":
                        CRMProjectAllocationViewNew _CRMProjectAllocationViewNew = (CRMProjectAllocationViewNew)Page.LoadControl("~/controltemplates/rmm/CRMProjectAllocationViewNew.ascx");
                        _CRMProjectAllocationViewNew.TicketID = Request["ticketId"];
                        _CRMProjectAllocationViewNew.ModuleName = Request["module"];
                        managementControls.Controls.Add(_CRMProjectAllocationViewNew);
                        break;
                    case "saveallocationastemplate":
                        SaveAllocationAsTemplate _SaveAllocationAsTemplate = (SaveAllocationAsTemplate)Page.LoadControl("~/ControlTemplates/RMM/SaveAllocationAsTemplate.ascx");
                        _SaveAllocationAsTemplate.ModuleName = Request["module"];
                        _SaveAllocationAsTemplate.ProjectID = Request["ticketId"];
                        managementControls.Controls.Add(_SaveAllocationAsTemplate);
                        break;
                    case "crmtemplateallocationview":
                        CRMTemplateAllocationView _CRMTemplateAllocationView = (CRMTemplateAllocationView)Page.LoadControl("~/ControlTemplates/RMM/CRMTemplateAllocationView.ascx");
                        _CRMTemplateAllocationView.TemplateId = Request["templateid"];
                        managementControls.Controls.Add(_CRMTemplateAllocationView);
                        break;
                    case "similarprojectpreviewallocations":
                        SimilarProjectPreviewAllocations _SimilarProjectPreviewAllocations = (SimilarProjectPreviewAllocations)Page.LoadControl("~/controltemplates/rmm/SimilarProjectPreviewAllocations.ascx");
                        //ActualProjectID
                        _SimilarProjectPreviewAllocations.ActualProjectID = Request["ActualProjectId"];
                        _SimilarProjectPreviewAllocations.ticketID = Request["ticketId"];
                        _SimilarProjectPreviewAllocations.ModuleName = Request["module"];
                        managementControls.Controls.Add(_SimilarProjectPreviewAllocations);
                        break;
                    case "contacts":
                        ContactsView _ContactsView = (ContactsView)Page.LoadControl("~/controltemplates/Admin/ListForm/ContactsView.ascx");
                        _ContactsView.ticketID = Request["ticketId"];
                        _ContactsView.FrameId = frameId;
                        _ContactsView.ReadOnly = isReadOnly;
                        //bool showSearchOption = false;
                        //Boolean.TryParse(Convert.ToString(Request["showSearchOption"]), out showSearchOption);
                        //_ContactsView.ShowSearchOption = showSearchOption;
                        //_ContactsView.ControlId = Convert.ToString(Request["ControlId"]);
                        managementControls.Controls.Add(_ContactsView);
                        break;

                    case "contactactivity":
                        ContactActivity _ContactActivity = (ContactActivity)Page.LoadControl("~/controltemplates/ugovernit/ContactActivity.ascx");
                        _ContactActivity.ticketID = Request["ticketId"];
                        _ContactActivity.FrameId = frameId;
                        _ContactActivity.ReadOnly = isReadOnly;
                        _ContactActivity.ControlId = Convert.ToString(Request["ControlId"]);
                        managementControls.Controls.Add(_ContactActivity);
                        break;

                    case "rankingcriteriaview":
                        RankingCriteriaView _RankingCriteriaView = (RankingCriteriaView)Page.LoadControl("~/controltemplates/ugovernit/RankingCriteriaView.ascx");
                        _RankingCriteriaView.ticketID = Request["ticketId"];
                        _RankingCriteriaView.FrameId = frameId;
                        _RankingCriteriaView.ReadOnly = isReadOnly;
                        _RankingCriteriaView.ControlId = Convert.ToString(Request["ControlId"]);
                        managementControls.Controls.Add(_RankingCriteriaView);
                        break;




                    case "activitiesaddedit":
                        int activityId = 0;
                        ActivitiesAddEdit _ActivitiesAddEdit = (ActivitiesAddEdit)Page.LoadControl("~/controltemplates/ugovernit/ActivitiesAddEdit.ascx");
                        int.TryParse(Request["ID"], out activityId);
                        _ActivitiesAddEdit.Id = activityId;
                        int cID = 0;
                        int.TryParse(Request["contactID"], out cID);
                        _ActivitiesAddEdit.contactID = cID;
                        _ActivitiesAddEdit.ticketID = Convert.ToString(Request["ticketID"]);
                        managementControls.Controls.Add(_ActivitiesAddEdit);
                        break;

                    case "stagetickets":
                        TicketsByStage _ticketsstage = (TicketsByStage)Page.LoadControl("~/controltemplates/ugovernit/ticketsbystage.ascx");
                        _ticketsstage.ModuleName = Request["modulename"];
                        _ticketsstage.StageStep = Convert.ToInt32(Request["stagestep"]);
                        _ticketsstage.StageTitle = Request["stagename"];

                        _ticketsstage.LifeCycle = Request["LifeCycle"];
                        //
                        managementControls.Controls.Add(_ticketsstage);
                        break;

                    case "bottlenecktrendchart":
                        BottleNeckTrendChart _BottleneckTrendChart = (BottleNeckTrendChart)Page.LoadControl("~/controltemplates/uGovernIT/BottleNeckTrendChart.ascx");
                        _BottleneckTrendChart.ModuleName = Request["ModuleName"];
                        _BottleneckTrendChart.StageStep = Convert.ToInt32(Request["StageStep"]);
                        _BottleneckTrendChart.StageTitle = Request["StageTitle"];
                        managementControls.Controls.Add(_BottleneckTrendChart);
                        break;

                    //case "editsoftwareimage":
                    //    {
                    //        EditSoftwareImage editSoftWareImage = (EditSoftwareImage)Page.LoadControl("~/_controltemplates/15/uGovernIT/EditSoftwareImage.ascx");
                    //        editSoftWareImage.ItemID = uHelper.StringToInt(Request["ItemID"]);
                    //        managementControls.Controls.Add(editSoftWareImage);
                    //    }
                    //    break;

                    //case "pmmbudgetaddedit":
                    //    PMMBudgetAddEdit pmmbudgetaddedit = (PMMBudgetAddEdit)Page.LoadControl("~/_controltemplates/15/uGovernIT/PMMBudgetAddEdit.ascx");
                    //    int.TryParse(Request["PMMId"], out PMMId);
                    //    pmmbudgetaddedit.PMMID = PMMId;
                    //    int budgetid;
                    //    int.TryParse(Request["id"], out id);
                    //    pmmbudgetaddedit.Id = id;
                    //    BudgetType budgettype = (BudgetType)Enum.Parse(typeof(BudgetType), Request["BudgetType"]);
                    //    pmmbudgetaddedit.Budgettype = budgettype;
                    //    pmmbudgetaddedit.pmmBudgetNeedsApproval = Convert.ToBoolean(Request["needBudgetApproval"]);
                    //    int.TryParse(Request["budgetid"], out budgetid);
                    //    pmmbudgetaddedit.BudgetId = budgetid;
                    //    managementControls.Controls.Add(pmmbudgetaddedit);
                    //    break;
                    case "modulebudgetaddedit":
                        ModuleBudgetAddEdit nprbudgetaddedit = (ModuleBudgetAddEdit)Page.LoadControl("~/ControlTemplates/uGovernIT/Budget/ModuleBudgetAddEdit.ascx");
                        int.TryParse(Request["ID"], out id);
                        nprbudgetaddedit.Id = id;
                        nprbudgetaddedit.TicketId = Request["TicketID"];
                        nprbudgetaddedit.ModuleName = Request["ModuleName"];
                        managementControls.Controls.Add(nprbudgetaddedit);
                        break;

                    case "newprojecttask":
                        NewProjectTask newprojecttask = (NewProjectTask)Page.LoadControl("~/ControlTemplates/uGovernIT/NewProjectTask.ascx");

                        newprojecttask.FrameId = frameId;
                        int.TryParse(Request["ID"], out id);

                        newprojecttask.Ids = id;

                        newprojecttask.TicketID = Request["TicketID"];

                        int baselineNumber = 0;
                        if (Request["baselineNum"] != null)
                            int.TryParse(Request["baselineNum"].Trim(), out baselineNumber);

                        newprojecttask.BaseLineID = baselineNumber;

                        newprojecttask.ModuleName = string.IsNullOrEmpty(Request["ModuleName"]) ? Request["Module"] : Request["ModuleName"];

                        // Need this to fix issue which occur when we edit n update a task and TaskList control is reloaded.
                        if (!string.IsNullOrEmpty(newprojecttask.TicketID))
                            newprojecttask.TicketID = newprojecttask.TicketID.ToUpper();
                        if (!string.IsNullOrEmpty(newprojecttask.ModuleName))
                            newprojecttask.ModuleName = newprojecttask.ModuleName.ToUpper();

                        managementControls.Controls.Add(newprojecttask);
                        break;
                    //case "tasklisttree":
                    //    TaskListTree tasklisttree = (TaskListTree)Page.LoadControl("~/ControlTemplates/PMM/TaskListTree.ascx");
                    //    int.TryParse(Request["ID"], out id);
                    //    tasklisttree.Ids = id;
                    //    tasklisttree.TicketID = Request["TicketID"];
                    //    tasklisttree.ModuleName = Request["ModuleName"];
                    //    managementControls.Controls.Add(tasklisttree);
                    //    break;



                    case "userprofilerelatedassets":
                        LoadAssetsForParticularUser();
                        break;
                    case "myclosedtickets":
                        LoadClosedForUser();
                        break;
                    case "myopentickets":
                        LoadMyRequests();
                        break;
                    case "waitingonme":
                        LoadWaitingonMeRequests();
                        break;
                    case "mytask":
                        LoadMyTask();
                        break;
                    case "showmytasks":
                        LoadAllMyTask();
                        break;
                    case "departmentticket":
                        LoadMyDepartment();
                        break;
                    case "myproject":
                        LoadMyProjects();
                        break;
                    case "documentpendingapprove":
                        LoadMyPendingDocuments();
                        break;
                    case "sendsurvey":
                        {
                            SendSurvey _SendSurvey = (SendSurvey)Page.LoadControl("~/controltemplates/admin/ListForm/SendSurvey.ascx");
                            managementControls.Controls.Add(_SendSurvey);
                        }
                        break;
                    case "groupscorecard":
                        GroupScorecard _GroupScorecard = (GroupScorecard)Page.LoadControl("~/controltemplates/uGovernIT/GroupScorecard.ascx");
                        managementControls.Controls.Add(_GroupScorecard);
                        break;

                    case "servicequestionsummary":
                        ServiceQuestionSummary summary = (ServiceQuestionSummary)Page.LoadControl("~/ControlTemplates/uGovernIT/ServiceQuestionSummary.ascx");
                        summary.FrameId = frameId;
                        summary.ReadOnly = isReadOnly;
                        int ticketiId = 0;
                        int.TryParse(Request["currentTicketId"], out ticketiId);
                        string listiName = Request["ListName"];
                        summary.ModuleName = Request["Module"];
                        summary.TicketId = ticketiId;
                        summary.ListName = listiName;
                        managementControls.Controls.Add(summary);
                        break;
                    ////case "printtabpagebreak":
                    ////    PrintTabPageBreak printtabpagebreakCtr = (PrintTabPageBreak)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/PrintTabPageBreak.ascx");
                    ////    printtabpagebreakCtr.sourceUrl = Convert.ToString(Request["source"]);
                    ////    managementControls.Controls.Add(printtabpagebreakCtr);
                    ////    break;


                    case "newdashboardchartui":
                        NewDashboardChartUI newdashboardchartui = (NewDashboardChartUI)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/NewDashboardChartUI.ascx");
                        managementControls.Controls.Add(newdashboardchartui);
                        break;

                    //case "customdashboardpanel":
                    //    CustomDashboardPanel customdashboardpanel = (CustomDashboardPanel)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/CustomDashboardPanel.ascx");
                    //    managementControls.Controls.Add(customdashboardpanel);
                    //    break;

                    //case "nprprojectsummaryreportcontrol":
                    //    NPRProjectSummaryReportControl _nprprojectSummaryReport = (NPRProjectSummaryReportControl)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/NPRProjectSummaryReportControl.ascx");
                    //    managementControls.Controls.Add(_nprprojectSummaryReport);
                    //    break;
                    //case "nprprojectsummaryreportviewer":
                    //    NPRProjectSummaryReportViewer _nprprojectsummaryreportViewer = (NPRProjectSummaryReportViewer)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/NPRProjectSummaryReportViewer.ascx");


                    //    int[] nprIds = null;
                    //    if (Request["NPRIDs"] != null)
                    //    {
                    //        if (!string.IsNullOrEmpty(Request["NPRIDs"]))
                    //        {
                    //            nprIds = Array.ConvertAll<string, int>(Request["NPRIDs"].Split(','), int.Parse);
                    //        }
                    //    }
                    //    List<string> displayFields = uHelper.ConvertStringToList(Request["Fields"], ",");
                    //    _nprprojectsummaryreportViewer.Fields = displayFields;

                    //    _nprprojectsummaryreportViewer.NPRIds = nprIds;
                    //    managementControls.Controls.Add(_nprprojectsummaryreportViewer);
                    //    break;
                    //case "vendorperformancemanagementdashboard":
                    //    VendorPerformanceManagementDashboard _vendorperformancemanagementdashboardcontrol = (VendorPerformanceManagementDashboard)Page.LoadControl("~/_controltemplates/15/uGovernIT/VendorPerformanceManagementDashboard.ascx");
                    //    managementControls.Controls.Add(_vendorperformancemanagementdashboardcontrol);
                    //    break;
                    //case "slaperformancebyvnd":
                    //    SLAPerformanceByVND _slaperformancebyvnd = (SLAPerformanceByVND)Page.LoadControl("~/_controltemplates/15/uGovernIT/SLAPerformanceByVND.ascx");
                    //    int month = 0;
                    //    int.TryParse(Request["Month"], out month);
                    //    _slaperformancebyvnd.Month = month;
                    //    int slayear = 0;
                    //    int.TryParse(Request["SLAYear"], out slayear);
                    //    _slaperformancebyvnd.Year = slayear;

                    //    _slaperformancebyvnd.PublicTicketId = Request["TicketPublicId"];

                    //    managementControls.Controls.Add(_slaperformancebyvnd);
                    //    break;
                    //case "showkpicpi":
                    //    VendorSLAsByVND _vendorslasbyvnd = (VendorSLAsByVND)Page.LoadControl("~/_controltemplates/15/uGovernIT/VendorSLAsByVND.ascx");
                    //    _vendorslasbyvnd.TicketID = Convert.ToString(Request["ticketid"]);
                    //    managementControls.Controls.Add(_vendorslasbyvnd);

                    //    break;
                    case "slaperformancedashboard":
                        SLAPerformanceDashboard _slaperformancedashboard = (SLAPerformanceDashboard)Page.LoadControl("~/controltemplates/uGovernIT/SLAPerformanceDashboard.ascx");
                        managementControls.Controls.Add(_slaperformancedashboard);

                        break;
                    case "slaperformancetabulardashboard":
                        SLAPerformanceTabularDashboard _slaperformancetabulardashboard = (SLAPerformanceTabularDashboard)Page.LoadControl("~/controltemplates/uGovernIT/SLAPerformanceTabularDashboard.ascx");
                        managementControls.Controls.Add(_slaperformancetabulardashboard);

                        break;
                    case "sladrildowndata":
                        string slaSummaryModule = Request["Module"];
                        string title = Uri.UnescapeDataString(Request["Title"]);
                        string dateFilter = Request["dateFilter"];
                        bool showOpen = UGITUtility.StringToBoolean(Request["IncludeOpen"]);
                        GetSLADrilledData(slaSummaryModule, title, dateFilter, includeOpen: showOpen);

                        break;
                    case "slametricsdrilldown":
                        string slaMetricsmodule = Request["Module"];
                        string slametricsCategory = Uri.UnescapeDataString(Request["CategoryChoice"]);
                        string slaMetricsdateFilter = Uri.UnescapeDataString(Request["filter"]);
                        string slaName = Request["SLAName"];
                        int ruleLookupId = UGITUtility.StringToInt(Request["SLARuleId"]);
                        int forSVCId = 0;
                        int.TryParse(Convert.ToString(Request["ForSVCMetrics"]), out forSVCId);
                        GetSLAMetricsData(slaMetricsmodule, slametricsCategory, slaMetricsdateFilter, slaName, ruleLookupId, forSVCId);
                        break;
                    case "surveyfeedbackreportviewer":
                        SurveyFeedbackReportViewer _surveyfeedbackreportviewer = (SurveyFeedbackReportViewer)Page.LoadControl("~/controltemplates/uGovernIT/Services/SurveyFeedbackReportViewer.ascx");
                        _surveyfeedbackreportviewer.Fromdate = Convert.ToString(Request["from"]);
                        _surveyfeedbackreportviewer.Todate = Convert.ToString(Request["to"]);
                        _surveyfeedbackreportviewer.Selectedsurvey = Convert.ToString(Request["selectedsurvey"]);
                        _surveyfeedbackreportviewer.Surveyoftype = Convert.ToString(Request["type"]);
                        _surveyfeedbackreportviewer.Survey = Convert.ToString(Request["survey"]);
                        _surveyfeedbackreportviewer.FilterExp = Convert.ToString(Request["FilterExp"]);
                        managementControls.Controls.Add(_surveyfeedbackreportviewer);
                        break;
                    case "problemreportdrildowndata":
                        string problemrequestmodule = Request["Module"];
                        string problemrequest = Uri.UnescapeDataString(Request["Title"]);
                        GetProblemRequestFilterData(problemrequestmodule, problemrequest);
                        break;
                    case "groupscorecarddrilldowndata":
                        LoadGroupScoreCard();
                        break;
                    case "predictedbacklogdrilldowndata":
                        LoadPredictedBacklog();
                        break;
                    case "projecttasklist":
                        ProjectsTaskList _ProjectsTaskList = (ProjectsTaskList)Page.LoadControl("~/controltemplates/uGovernIT/ProjectsTaskList.ascx");
                        managementControls.Controls.Add(_ProjectsTaskList);
                        break;

                    case "winandlossreport":
                        WinAndLossReport _WinAndLossReport = (WinAndLossReport)Page.LoadControl("~/controltemplates/uGovernIT/WinAndLossReport.ascx");
                        managementControls.Controls.Add(_WinAndLossReport);
                        break;

                    //case "dashboardcontroldrilldown":
                    //    {
                    //        string dashboardCtr = Request["dashboardControl"];
                    //        if (string.IsNullOrWhiteSpace(dashboardCtr))
                    //            break;

                    //        dashboardCtr = dashboardCtr.ToLower();
                    //        switch (dashboardCtr)
                    //        {
                    //            case "agentperformance":
                    //                {
                    //                    CustomFilteredTickets filteredListobj = (CustomFilteredTickets)Page.LoadControl("~/CONTROLTEMPLATES/Shared/CustomFilteredTickets.ascx");
                    //                    DataTable dt = GetAgentPerformanceData(Uri.UnescapeDataString(Request["PRPName"]));
                    //                    filteredListobj.FilteredTable = dt;
                    //                    filteredListobj.IsFilteredTableExist = true;
                    //                    managementControls.Controls.Add(filteredListobj);

                    //                }
                    //                break;
                    //        }
                    //    }
                    //    break;
                    case "weeklydrilldata":
                        string ticketAssignee = Request["assigneePRP"];
                        int weeks = UGITUtility.StringToInt(Request["weeks"]);
                        string type = Request["type"];
                        GetWeeklyDrilData(ticketAssignee, weeks, type);
                        break;
                    case "groupunsolvedticket":
                        string ticketgroup = Request["group"];
                        GetGroupUnsolvedData(ticketgroup);
                        break;
                    case "ticketcreatedweeklytrend":
                        int currentweeks = UGITUtility.StringToInt(Request["weeks"]);
                        GetTicketCreatedByWeeklyTrend(currentweeks);
                        break;

                    case "openandcloseforrequestor":

                        CustomFilteredTickets filtered = (CustomFilteredTickets)Page.LoadControl("~/ControlTemplates/Shared/CustomFilteredTickets.ascx");
                        UserProfile spUser = null;
                        if (!string.IsNullOrWhiteSpace(Request["CurrentRequestor"]) || !string.IsNullOrWhiteSpace(Request["userId"]))
                        {
                            string user = string.Empty;
                            if (Request["userId"] != null)
                            {
                                spUser = objUserManager.GetUserById(Request["userId"]);

                                if (spUser != null)
                                    user = spUser.Id; //user = spUser.Name;
                            }
                            else if (!string.IsNullOrWhiteSpace(Uri.UnescapeDataString(Request["CurrentRequestor"])))
                            {
                                user = Uri.UnescapeDataString(Request["CurrentRequestor"]);
                                user = user.Replace("'", "''");
                            }

                            DataTable dtRequestorTickets = DashboardCache.GetCachedDashboardData(ApplicationContext, DatabaseObjects.Tables.DashboardSummary);
                            filtered.IsFilteredTableExist = true;
                            filtered.IsDashboard = true;
                            string TicketRequestorColName = DatabaseObjects.Columns.TicketRequestor;
                            if (dtRequestorTickets != null)
                            {
                                string sort = string.Format("{0} DESC", DatabaseObjects.Columns.Created);
                                if (dtRequestorTickets.Columns.Contains(DatabaseObjects.Columns.TicketRequestor + "$Id"))
                                {
                                    TicketRequestorColName = DatabaseObjects.Columns.TicketRequestor + "$Id";
                                }
                                DataRow[] drColl = dtRequestorTickets.Select(string.Format("{0} = '{1}' and {2}='{3}'", TicketRequestorColName, user.Replace("'", "''"), DatabaseObjects.Columns.TenantID, spUser.TenantID), sort);
                                if (drColl != null && drColl.Length > 0)
                                {
                                    filtered.FilteredTable = drColl.CopyToDataTable();
                                }
                            }
                            managementControls.Controls.Add(filtered);
                        }
                        //LoadMyRequests();
                        break;
                    //case "businessstrategy":
                    //    BusinessStrategy _businessStrategy = (BusinessStrategy)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/BusinessStrategy.ascx");
                    //    managementControls.Controls.Add(_businessStrategy);
                    //    break;
                    case "businessstrategygroupeddata":
                        CustomFilteredTickets _groupedData = (CustomFilteredTickets)Page.LoadControl("~/ControlTemplates/Shared/CustomFilteredTickets.ascx");
                        string[] filter = null;
                        if (!string.IsNullOrEmpty(Convert.ToString(Request["filter"])))
                            filter = Convert.ToString(Request["filter"]).Split('_');

                        List<string> dataFilter = new List<string>();

                        if (!string.IsNullOrEmpty(Convert.ToString(Request["datafilter"])))
                            dataFilter = Convert.ToString(Request["datafilter"]).Split('_').ToList();

                        bool bsexist = false;
                        if (Request["BSExist"] != null)
                            bsexist = Convert.ToBoolean(Request["BSExist"]);

                        bool dayplan = false;
                        if (Request["dayplan"] != null)
                            dayplan = UGITUtility.StringToBoolean(Request["dayplan"]);

                        DateTime? previousDate = null;
                        if (Request["previousDate"] != null)
                            previousDate = Convert.ToDateTime(Request["previousDate"]);

                        DateTime? nextDate = null;
                        if (Request["nextDate"] != null)
                            nextDate = Convert.ToDateTime(Request["nextDate"]);

                        _groupedData.IsFilteredTableExist = true;
                        _groupedData.HideAllTicketTab = true;

                        _groupedData.MyHomeTab = "MyProjectTab";
                        ModuleColumnManager moduleColumnManager = new ModuleColumnManager(HttpContext.Current.GetManagerContext());
                        DataTable spList = moduleColumnManager.GetDataTable();
                        if (spList != null && spList.Rows.Count > 0)
                        {
                            // DataTable dtcoll = coll.GetDataTable();
                            //bool isexist = spList.AsEnumerable().Any(x => x.Field<string>(DatabaseObjects.Columns.CategoryName).ToLower() == "businessinitiatives");
                            bool isexist = spList.AsEnumerable().Any(x => x.Field<string>(DatabaseObjects.Columns.CategoryName).ToLower() == "myprojecttab");
                            if (isexist)
                                _groupedData.MyHomeTab = "MyProjectTab";

                        }
                        _groupedData.IsHomePage = true;
                        _groupedData.HideNewTicketButton = true;
                        _groupedData.HideModuleDetail = true;
                        _groupedData.CategoryName = "MyProjectTab";
                        BusinessStrategyDashboard obj = new BusinessStrategyDashboard(HttpContext.Current.GetManagerContext());
                        _groupedData.FilteredTable = obj.GetBusinessStrategyData(filter, dataFilter, bsexist, dayplan, previousDate, nextDate);

                        managementControls.Controls.Add(_groupedData);
                        break;
                    case "trackprojectstagehistory":
                        trackprojectstagehistory _TrackProjectStageHistory = (trackprojectstagehistory)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/TrackProjectStageHistory.ascx");
                        managementControls.Controls.Add(_TrackProjectStageHistory);
                        break;

                    //case "nprbudget":
                    //    NPRBudget nprBudget = (NPRBudget)Page.LoadControl("~/ControlTemplates/uGovernIT/NPRBudget.ascx");
                    //    nprBudget.TicketID = Request["ticketId"];
                    //    nprBudget.FrameId = frameId;
                    //    nprBudget.ModuleName = Request["module"];
                    //    nprBudget.ReadOnly = isReadOnly;
                    //    managementControls.Controls.Add(nprBudget);
                    //    break;
                    case "modulebudget":
                        ModuleBudgetList moduleBudget = (ModuleBudgetList)Page.LoadControl("~/ControlTemplates/uGovernIT/Budget/ModuleBudgetList.ascx");
                        moduleBudget.TicketID = Request["ticketId"];
                        moduleBudget.FrameId = frameId;
                        moduleBudget.ModuleName = Request["module"];
                        moduleBudget.ReadOnly = isReadOnly;
                        managementControls.Controls.Add(moduleBudget);
                        break;
                    case "budgetactualaddedit":
                        BudgetActualAddEdit actualBudget = (BudgetActualAddEdit)Page.LoadControl("~/ControlTemplates/uGovernIT/Budget/BudgetActualAddEdit.ascx");
                        int.TryParse(Request["ID"], out id);
                        int budgetid;
                        int.TryParse(Request["BudgetId"], out budgetid);
                        actualBudget.Id = id;
                        actualBudget.TicketId = Request["TicketID"];
                        actualBudget.ModuleName = Request["ModuleName"];
                        actualBudget.BudgetId = budgetid;
                        managementControls.Controls.Add(actualBudget);
                        break;
                    case "dashboardimport":
                        DashboardQueryImport dashboardImport = (DashboardQueryImport)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/DashboardQueryImport.ascx");
                        managementControls.Controls.Add(dashboardImport);
                        break;
                    case "modulecolumnsaddedit":
                        ModuleColumnsAddEdit _ModuleColumnsAddEdit = (ModuleColumnsAddEdit)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ModuleColumnsAddEdit.ascx");
                        int mcID;
                        int.TryParse(Request["ItemID"], out mcID);
                        _ModuleColumnsAddEdit.moduleColumnID = mcID;
                        _ModuleColumnsAddEdit.Module = Convert.ToString(Request["module"]);
                        int viewtype = 0;
                        int.TryParse(UGITUtility.ObjectToString(Request["moduleType"]), out viewtype);
                        _ModuleColumnsAddEdit.ViewType = viewtype;
                        contentPanel.Controls.Add(_ModuleColumnsAddEdit);
                        break;
                    case "accountchangeprocess":
                        AccountChangeProcess accountChangeProcess = (AccountChangeProcess)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/AccountChangeProcess.ascx");
                        accountChangeProcess.IsFilteredTableExist = true;
                        managementControls.Controls.Add(accountChangeProcess);
                        break;
                    case "globalsearchpage":
                        GlobalSearchPage globalSearchPage = (GlobalSearchPage)Page.LoadControl("~/ControlTemplates/Shared/GlobalSearchPage.ascx");
                        //globalSearchPage.IsFilteredTableExist = true;
                        managementControls.Controls.Add(globalSearchPage);
                        break;
                    case "summarybytechnician":
                        CustomFilteredTickets filterData = (CustomFilteredTickets)Page.LoadControl("~/ControlTemplates/Shared/CustomFilteredTickets.ascx");
                        string stage = string.Empty;
                        string technician = string.Empty;
                        DateTime startDates = new DateTime();
                        DateTime endDates = new DateTime();
                        string modulename = string.Empty;

                        bool includeTechnician = false;
                        string category = string.Empty;
                        if (!string.IsNullOrEmpty(Request["stage"]))
                        {
                            stage = Request["stage"];

                        }
                        if (!string.IsNullOrEmpty(Request["technician"]))
                        {
                            technician = Request["technician"];
                            if (technician.ToLower() == "total")
                                technician = "all";
                        }
                        if (!string.IsNullOrEmpty(Request["startDate"]))
                        {
                            startDates = UGITUtility.StringToDateTime(Request["startDate"]);
                        }
                        if (!string.IsNullOrEmpty(Request["endDate"]))
                        {
                            endDates = UGITUtility.StringToDateTime(Request["endDate"]);

                        }
                        if (!string.IsNullOrEmpty(Request["endDate"]))
                        {
                            endDates = UGITUtility.StringToDateTime(Request["endDate"]);

                        }
                        if (!string.IsNullOrEmpty(Request["includeTechnician"]))
                        {
                            includeTechnician = UGITUtility.StringToBoolean(Request["includeTechnician"]);
                        }
                        if (!string.IsNullOrEmpty(Request["modulename"]))
                        {
                            modulename = Request["modulename"];
                        }
                        if (!string.IsNullOrWhiteSpace(Request["category"]))
                        {
                            category = Request["category"];
                        }
                        ModuleStatistics moduleStatistics = new ModuleStatistics(HttpContext.Current.GetManagerContext());
                        DataTable dataTicketByPrp = moduleStatistics.GetSummaryByTechnicianDrillDownData(modulename, category, UGITUtility.ConvertStringToList(stage, ","), startDates, endDates, includeTechnician, technician);
                        filterData.FilteredTable = dataTicketByPrp;
                        filterData.IsFilteredTableExist = true;
                        filterData.IsDashboard = true;
                        filterData.HideNewTicketButton = true;
                        filterData.HideModuleDetail = true;
                        filterData.showExportIcons = true;
                        managementControls.Controls.Add(filterData);
                        break;
                    case "projectsimilaritycontrol":
                        ProjectSimilarityControl _ProjectSimilarityControl = (ProjectSimilarityControl)Page.LoadControl("~/ControlTemplates/uGovernIT/ProjectSimilarityControl.ascx");
                        _ProjectSimilarityControl.ProjectIds = Convert.ToString(Request["ids"]);
                        managementControls.Controls.Add(_ProjectSimilarityControl);
                        break;
                    case "taskdetailview":
                        TaskDetailView _TaskDetailView = (TaskDetailView)Page.LoadControl("~/Controltemplates/uGovernIT/TaskDetailView.ascx");
                        managementControls.Controls.Add(_TaskDetailView);
                        break;
                    case "projectplan":
                        ProjectPlanReport _ProjectPlanReport = (ProjectPlanReport)Page.LoadControl("~/Controltemplates/uGovernIT/ProjectPlanReport.ascx");
                        managementControls.Controls.Add(_ProjectPlanReport);
                        break;
                    case "similarityscoredetail":
                        SimilarityScoreDetail _SimilarityScoreDetail = (SimilarityScoreDetail)Page.LoadControl("~/ControlTemplates/uGovernIT/SimilarityScoreDetail.ascx");
                        _SimilarityScoreDetail.primaryProjectId = Request["primaryProjectId"];
                        _SimilarityScoreDetail.secondaryProjectId = Request["secondaryProjectId"];
                        _SimilarityScoreDetail.similarityScore = Request["Score"];
                        _SimilarityScoreDetail.MetricType = Request["MetricType"];

                        managementControls.Controls.Add(_SimilarityScoreDetail);
                        break;
                    case "addchecklist":
                        AddCheckList _AddCheckList = (AddCheckList)Page.LoadControl("~/ControlTemplates/Admin/ListForm/AddCheckList.ascx");
                        _AddCheckList.CheckListId = Request["CheckListId"];
                        managementControls.Controls.Add(_AddCheckList);
                        break;
                    case "addrankingcriteria":
                        RankingCriteriaAdd _RankingCriteriaAdd = (RankingCriteriaAdd)Page.LoadControl("~/ControlTemplates/Admin/ListForm/RankingCriteriaAdd.ascx");
                        _RankingCriteriaAdd.RankingCriteriaId = Request["RankingCriteriaId"];
                        managementControls.Controls.Add(_RankingCriteriaAdd);
                        break;

                    case "addchecklistrole":
                        AddCheckListRole _AddCheckListRole = (AddCheckListRole)Page.LoadControl("~/ControlTemplates/Admin/ListForm/AddCheckListRole.ascx");
                        _AddCheckListRole.CheckListId = Request["CheckListId"];
                        _AddCheckListRole.CheckListRoleId = Request["CheckListRoleId"];
                        managementControls.Controls.Add(_AddCheckListRole);
                        break;
                    case "addchecklisttask":
                        AddCheckListTask _AddCheckListTask = (AddCheckListTask)Page.LoadControl("~/ControlTemplates/Admin/ListForm/AddCheckListTask.ascx");
                        _AddCheckListTask.CheckListId = Request["CheckListId"];
                        _AddCheckListTask.CheckListTaskId = Request["CheckListTaskId"];
                        managementControls.Controls.Add(_AddCheckListTask);
                        break;
                    case "checklistprojectview":
                        CheckListProjectView _CheckListProjectView = (CheckListProjectView)Page.LoadControl("~/ControlTemplates/uGovernIT/CheckListProjectView.ascx");
                        _CheckListProjectView.ModuleName = Convert.ToString(Request["module"]);
                        _CheckListProjectView.PublicTicketID = Request["ticketId"];
                        managementControls.Controls.Add(_CheckListProjectView);
                        break;
                    case "importchecklisttemplate":
                        ImportCheckListTemplate _ImportCheckListTemplate = (ImportCheckListTemplate)Page.LoadControl("~/ControlTemplates/uGovernIT/ImportCheckListTemplate.ascx");
                        _ImportCheckListTemplate.ModuleName = Convert.ToString(Request["module"]);
                        _ImportCheckListTemplate.PublicTicketID = Request["ticketId"];
                        managementControls.Controls.Add(_ImportCheckListTemplate);
                        break;

                    case "importrankingcriteria":
                        ImportRankingCriteria _ImportRankingCriteria = (ImportRankingCriteria)Page.LoadControl("~/ControlTemplates/uGovernIT/ImportRankingCriteria.ascx");
                        _ImportRankingCriteria.ModuleName = Convert.ToString(Request["module"]);
                        _ImportRankingCriteria.TicketID = Request["ticketId"];
                        managementControls.Controls.Add(_ImportRankingCriteria);
                        break;

                    case "leadrankingaddedit":
                        LeadRankingAddEdit _LeadRankingAddEdit = (LeadRankingAddEdit)Page.LoadControl("~/ControlTemplates/uGovernIT/LeadRankingAddEdit.ascx");
                        _LeadRankingAddEdit.ModuleName = Convert.ToString(Request["module"]);
                        _LeadRankingAddEdit.TicketID = Request["ticketId"];
                        _LeadRankingAddEdit.LeadRankingId = Request["LeadRankingId"];
                        managementControls.Controls.Add(_LeadRankingAddEdit);
                        break;

                    case "leadcriteriaaddedit":
                        LeadCriteriaAddEdit leadCriteriaAddEdit = (LeadCriteriaAddEdit)Page.LoadControl("~/ControlTemplates/uGovernIT/LeadCriteriaAddEdit.ascx");
                        leadCriteriaAddEdit.CriteriaId = Request["CriteriaId"];
                        managementControls.Controls.Add(leadCriteriaAddEdit);
                        break;
                    case "projectcomplexityaddedit":
                        ProjectComplexityAddEdit projectComplexityAddEdit = (ProjectComplexityAddEdit)Page.LoadControl("~/ControlTemplates/uGovernIT/ProjectComplexityAddEdit.ascx");
                        projectComplexityAddEdit.CriteriaId = Request["CriteriaId"];
                        managementControls.Controls.Add(projectComplexityAddEdit);
                        break;

                    case "customemailnotification":
                        CustomEmailNotification _CustomEmailNotification = (CustomEmailNotification)Page.LoadControl("~/ControlTemplates/uGovernIT/CustomEmailNotification.ascx");
                        _CustomEmailNotification.CheckListRoleId = UGITUtility.StringToInt(Request["CheckListRoleId"]);
                        _CustomEmailNotification.CheckListId = UGITUtility.StringToInt(Request["CheckListId"]);
                        _CustomEmailNotification.ticketId = Request["ticketId"];
                        managementControls.Controls.Add(_CustomEmailNotification);
                        break;

                    case "relatedcompanies":
                        RelatedCompanies _RelatedCompanies = (RelatedCompanies)Page.LoadControl("~/ControlTemplates/uGovernIT/RelatedCompanies.ascx");
                        _RelatedCompanies.ticketID = Request["ticketId"];
                        managementControls.Controls.Add(_RelatedCompanies);
                        break;
                    case "relatedcompanyaddedit":
                        RelatedCompanyAddEdit _RelatedCompanyAddEdit = (RelatedCompanyAddEdit)Page.LoadControl("~/controltemplates/uGovernIT/RelatedCompanyAddEdit.ascx");
                        _RelatedCompanyAddEdit.ticketID = Request["ticketId"];

                        int rCompanyId = 0;
                        int.TryParse(Request["ID"], out rCompanyId);
                        _RelatedCompanyAddEdit.Id = rCompanyId;
                        managementControls.Controls.Add(_RelatedCompanyAddEdit);
                        break;
                    case "projectteam":
                        ProjectTeam _ProjectTeam = (ProjectTeam)Page.LoadControl("~/controltemplates/uGovernIT/ProjectTeam.ascx");
                        _ProjectTeam.ticketID = Request["ticketId"];

                        managementControls.Controls.Add(_ProjectTeam);
                        break;
                    case "cprprojecttitlecontrol":
                        CPRProjectTitleControl _CPRProjectTitleControl = (CPRProjectTitleControl)Page.LoadControl("~/controltemplates/uGovernIT/CPRProjectTitleControl.ascx");
                        _CPRProjectTitleControl.ticketID = Request["ticketId"];
                        managementControls.Controls.Add(_CPRProjectTitleControl);
                        break;
                    case "addprojectexperiencetags":
                        AddProjectExperienceTags _AddProjectExperienceTagsControl = (AddProjectExperienceTags)Page.LoadControl("~/controltemplates/RMONE/AddProjectExperienceTags.ascx");
                        _AddProjectExperienceTagsControl.TicketId = Request["ticketId"];
                        _AddProjectExperienceTagsControl.RequestFrom = Request["from"];
                        managementControls.Controls.Add(_AddProjectExperienceTagsControl);
                        break;
                    case "crmownercontractdetails":
                        CRMOwnerContractDetails _CRMOwnerContractDetails = (CRMOwnerContractDetails)Page.LoadControl("~/controltemplates/RMONE/CRMOwnerContractDetails.ascx");
                        _CRMOwnerContractDetails.TicketId = Request["ticketId"];
                        managementControls.Controls.Add(_CRMOwnerContractDetails);
                        break;
                    case "taskgraph":
                        TaskGraph _TaskGraph = (TaskGraph)Page.LoadControl("~/controltemplates/uGovernIT/TaskGraph.ascx");
                        _TaskGraph.ticketID = Request["ticketId"];
                        managementControls.Controls.Add(_TaskGraph);
                        break;
                    case "projectsummarycontrol":
                        ProjectSummaryControl _ProjectSummaryControl = (ProjectSummaryControl)Page.LoadControl("~/controltemplates/uGovernIT/ProjectSummaryControl.ascx");
                        _ProjectSummaryControl.ticketID = Request["ticketId"];
                        managementControls.Controls.Add(_ProjectSummaryControl);
                        break;
                    case "timelinecontrol":
                        TimeLineControl _TimeLineControl = (TimeLineControl)Page.LoadControl("~/controltemplates/uGovernIT/TimeLineControl.ascx");
                        _TimeLineControl.CPRId = Convert.ToInt32(Request["currentTicketId"]);
                        _TimeLineControl.Module = Convert.ToString(Request["Module"]);
                        _TimeLineControl.TicketId = Request["ticketId"];
                        managementControls.Controls.Add(_TimeLineControl);
                        break;
                    case "homedashboardtasks":
                        MyHomeTasks dashboardTasks = (MyHomeTasks)Page.LoadControl("~/ControlTemplates/uGovernIT/MyHomeTasks.ascx");
                        managementControls.Controls.Add(dashboardTasks);
                        break;
                    case "projectstandardworkitemnew":
                        ProjectStandardWorkItemNew _ProjectStandardWorkItemNew = (ProjectStandardWorkItemNew)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ProjectStandardWorkItemNew.ascx");
                        managementControls.Controls.Add(_ProjectStandardWorkItemNew);
                        break;
                    case "projectstandardworkitemedit":
                        ProjectStandardWorkItemEdit _ProjectStandardWorkItemEdit = (ProjectStandardWorkItemEdit)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/ProjectStandardWorkItemEdit.ascx");
                        _ProjectStandardWorkItemEdit.Id = UGITUtility.StringToInt(Request["ItemID"]);
                        managementControls.Controls.Add(_ProjectStandardWorkItemEdit);
                        break;
                    case "tickettask":
                        TaskUserControl ticketTask = (TaskUserControl)Page.LoadControl("~/ControlTemplates/uGovernIT/dashboard/TaskUserControl.ascx");
                        ticketTask.ID = "ticketTask";
                        managementControls.Controls.Add(ticketTask);
                        break;
                    case "homedashboardtickets":
                        CustomFilteredTickets dashboardTickets = (CustomFilteredTickets)Page.LoadControl("~/ControlTemplates/Shared/CustomFilteredTickets.ascx");

                        string ticketModuleName = Convert.ToString(Request["Module"]);
                        string hStatus = Convert.ToString(Request["Status"]);
                        //string UserType = Convert.ToString(Request["UserType"]);
                        //UserType = UserType.Replace(" ", string.Empty);
                        ApplicationContext context = HttpContext.Current.GetManagerContext();
                        TicketManager ticketMgr = new TicketManager(context);
                        ModuleViewManager moduleMgr = new ModuleViewManager(context);
                        UGITModule homeModule = moduleMgr.LoadByName(ticketModuleName);

                        DataTable dthometickets = new DataTable();
                        if (homeModule != null)
                        {
                            if (hStatus.Contains("close"))
                            {
                                dthometickets = ticketMgr.GetClosedTickets(homeModule);
                            }
                            else
                            {
                                dthometickets = ticketMgr.GetOpenTickets(homeModule);
                            }
                        }
                        if (hStatus == "all")
                        {
                            dashboardTickets.FilteredTable = dthometickets;
                        }
                        else
                        {
                            UserProfileManager umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                            string moduleTable = moduleMgr.GetModuleTableName(ticketModuleName);
                            string userid = context.CurrentUser.Id;
                            var role = umanager.GetUserRoles(userid).Select(x => x.Id).ToList();
                            ModuleStatistics modStatistics = new ModuleStatistics(context);

                            if (hStatus == "mystatus" || hStatus == "myprojectsclosein4weeks")
                            {

                            }
                            else if (hStatus == "myproject")
                            {
                                DataTable dtResult = new DataTable();

                                {
                                    List<UGITModule> uGITModules = new List<UGITModule>();
                                    List<DataTable> result = new List<DataTable>();
                                    List<ModuleStatisticResponse> resultList = new List<ModuleStatisticResponse>();
                                    uGITModules = moduleMgr.Load(x => x.ModuleType == ModuleType.Project);
                                    foreach (var mObj in uGITModules)
                                    {
                                        ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "myopentickets", ModuleName = mObj.ModuleName, Status = TicketStatus.Open, UserID = userid };
                                        ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                                        statistics = modStatistics.Load(statRequest, false);
                                        resultList.Add(statistics);
                                    }
                                    foreach (var data in resultList)
                                    {
                                        if (data.ResultedData != null)
                                            dtResult.Merge(data.ResultedData);
                                    }
                                }
                                dashboardTickets.FilteredTable = dtResult;
                            }
                            else if (hStatus == "waitingonme")
                            {
                                DataTable dtResult = new DataTable();

                                {
                                    List<UGITModule> uGITModules = new List<UGITModule>();
                                    List<DataTable> result = new List<DataTable>();
                                    List<ModuleStatisticResponse> resultList = new List<ModuleStatisticResponse>();
                                    uGITModules = moduleMgr.Load(x => x.ModuleType != ModuleType.Governance);
                                    foreach (var mObj in uGITModules)
                                    {
                                        ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "waitingonme", ModuleName = mObj.ModuleName, Status = TicketStatus.Open, UserID = userid };
                                        ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                                        statistics = modStatistics.Load(statRequest, false);
                                        resultList.Add(statistics);
                                    }
                                    foreach (var data in resultList)
                                    {
                                        if (data.ResultedData != null)
                                            dtResult.Merge(data.ResultedData);
                                    }
                                }
                                dashboardTickets.FilteredTable = dtResult;
                            }
                            else if (hStatus == "myactionitems")
                            {

                            }
                            else if (hStatus == "myopenopportunities")
                            {

                                ModuleStatistics mod = new ModuleStatistics(context);
                                List<ModuleStatisticResponse> resultList = new List<ModuleStatisticResponse>();
                                ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "myopentickets", ModuleName = ModuleNames.OPM, Status = TicketStatus.Open, UserID = userid };
                                ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                                statistics = mod.Load(statRequest, false);
                                dashboardTickets.FilteredTable = statistics.ResultedData;
                            }
                            else if (hStatus == "allopenopportunities")
                            {
                                DataTable dtOPMOpenTickets = ticketMgr.GetAllOpenTicketsBasedOnModuleName("OPM");
                                dashboardTickets.FilteredTable = dtOPMOpenTickets;
                            }
                            else if (hStatus == "allopenproject")
                            {
                                List<UGITModule> uGITModules = new List<UGITModule>();
                                DataTable dtAllOpenProject = new DataTable();
                                List<ModuleStatisticResponse> resultList = new List<ModuleStatisticResponse>();
                                uGITModules = moduleMgr.Load(x => x.ModuleType == ModuleType.Project);
                                foreach (var mObj in uGITModules)
                                {
                                    ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "allopenproject", ModuleName = mObj.ModuleName, Status = TicketStatus.Closed, UserID = userid };
                                    ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                                    statistics = modStatistics.Load(statRequest, false);
                                    resultList.Add(statistics);
                                }
                                foreach (var data in resultList)
                                {
                                    if (data.ResultedData != null)
                                        dtAllOpenProject.Merge(data.ResultedData);
                                }
                                dashboardTickets.FilteredTable = dtAllOpenProject;
                            }
                            else if (hStatus == "allcloseproject")
                            {
                                ModuleStatistics mod = new ModuleStatistics(context);
                                ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "allclosedtickets", ModuleName = "CPR", Status = TicketStatus.Closed, UserID = userid };
                                ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                                statistics = mod.Load(statRequest, false);
                                dashboardTickets.FilteredTable = statistics.ResultedData;
                            }
                            else if (hStatus == "futureopencpr")
                            {
                                ModuleStatistics mod = new ModuleStatistics(context);
                                ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "allopentickets", ModuleName = "CPR", Status = TicketStatus.Open, UserID = userid };
                                ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                                statistics = mod.Load(statRequest, false);
                                DataTable openTicketsTable = new DataTable();
                                if (statistics != null)
                                    openTicketsTable = statistics.ResultedData;
                                if (openTicketsTable != null)
                                {
                                    DataRow[] resolvedRows = openTicketsTable.Select(string.Format("{0} > '{1}'", DatabaseObjects.Columns.EstimatedConstructionStart, DateTime.Now.ToString("MM/dd/yyyy")));
                                    if (resolvedRows != null && resolvedRows.Count() > 0)
                                    {
                                        dashboardTickets.FilteredTable = resolvedRows.CopyToDataTable();
                                    }
                                }
                            }
                            else if (hStatus == "currentopencpr")
                            {
                                ModuleStatistics mod = new ModuleStatistics(context);
                                ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "allopentickets", ModuleName = "CPR", Status = TicketStatus.Open, UserID = userid };
                                ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                                statistics = mod.Load(statRequest, false);
                                DataTable openTicketsTable = new DataTable();
                                if (statistics != null)
                                    openTicketsTable = statistics.ResultedData;
                                if (openTicketsTable != null)
                                {
                                    DataRow[] resolvedRows = openTicketsTable.Select(string.Format("{0}  <=  '{1}'", DatabaseObjects.Columns.EstimatedConstructionStart, DateTime.Now.ToString("MM/dd/yyyy")));
                                    if (resolvedRows != null && resolvedRows.Count() > 0)
                                    {
                                        dashboardTickets.FilteredTable = resolvedRows.CopyToDataTable();
                                    }
                                }
                            }
                            else if (hStatus=="totalresource")
                            {
                                CustomResourceAllocation customResourceAllocation = (CustomResourceAllocation)Page.LoadControl("~/CONTROLTEMPLATES/RMM/CustomResourceAllocation.ascx");
                                customResourceAllocation.FrameId = frameId;
                                customResourceAllocation.IncludeClosed = Request["IncludeClosed"] != null && Request["IncludeClosed"] != string.Empty ? Convert.ToBoolean(Request["IncludeClosed"]) : false;
                                managementControls.Controls.Add(customResourceAllocation);
                                if (bool.TryParse(Request["ShowResourceAllocationBtn"], out bool ResourceAllocationBtn))
                                    dashboardTickets.IsShowResourceAllocationBtn = ResourceAllocationBtn;
                                break;
                            }
                            else if (hStatus == "allopenservices")
                            {
                                DataTable dtCNSOpenTickets = ticketMgr.GetAllOpenTicketsBasedOnModuleName("CNS");
                                dashboardTickets.FilteredTable = dtCNSOpenTickets;
                            }
                            else if (hStatus == "AllClosedTickets")
                            {
                                List<UGITModule> uGITModules = moduleMgr.Load(x => x.ModuleType == ModuleType.Project);
                                DataTable dtAllClosedProjects = new DataTable();
                                foreach (UGITModule mObj in uGITModules)
                                {
                                    DataTable dtclosedtickets = ticketMgr.GetClosedTickets(mObj);
                                    dtAllClosedProjects.Merge(dtclosedtickets);
                                }
                                dashboardTickets.FilteredTable = dtAllClosedProjects;
                            }
                            else if (hStatus == "ProjectPendingAllocation")
                            {
                                DataTable dtResult = GetTableDataManager.GetTableDataUsingQuery($"ProjectPendingAllocation @TenantId='{context.TenantID}'");
                                dashboardTickets.FilteredTable = dtResult;
                            }
                            else if (hStatus == "recentwonopportunity")
                            {
                                DataTable dtOPMOpenTickets = ticketMgr.GetAllOpenTicketsBasedOnModuleName("OPM");
                                DataRow[] dtWonTickets = dtOPMOpenTickets.Select($"{DatabaseObjects.Columns.CRMOpportunityStatus}='Awarded'");
                                if (dtWonTickets != null && dtWonTickets.Count() > 0)
                                    dashboardTickets.FilteredTable = dtWonTickets.CopyToDataTable();
                            }
                            else if (hStatus == "recentlostopportunity")
                            {
                                UGITModule opmModule = moduleMgr.LoadByName("OPM");
                                DataTable dtOPMOpenTickets = ticketMgr.GetOpenTickets(opmModule);
                                DataRow[] dtLostTickets = dtOPMOpenTickets.Select($"{DatabaseObjects.Columns.CRMOpportunityStatus}='Lost'");
                                if (dtLostTickets != null && dtLostTickets.Count() > 0)
                                    dashboardTickets.FilteredTable = dtLostTickets.CopyToDataTable();
                            }
                            else if (hStatus == "liveprojects")
                            {
                                dashboardTickets.FilteredTable = dthometickets.Select($"{DatabaseObjects.Columns.Closed} = 0 and {DatabaseObjects.Columns.StageStep} <= 8").CopyToDataTable();
                            }
                            else if (hStatus == "pipeline")
                            {
                                dashboardTickets.FilteredTable = dthometickets.Select($"{DatabaseObjects.Columns.Closed} = 0 and {DatabaseObjects.Columns.StageStep} < 8").CopyToDataTable();
                            }
                            else if (hStatus == "Projectsreadytostart")
                            {
                                dashboardTickets.FilteredTable = dthometickets.Select($"{DatabaseObjects.Columns.Closed} = 0 and {DatabaseObjects.Columns.StageStep} = 7").CopyToDataTable();
                            }
                            else if (hStatus == "closed")
                            {
                                dashboardTickets.FilteredTable = dthometickets.Select($"{DatabaseObjects.Columns.Closed} = 1").CopyToDataTable();
                            }
                            else if (hStatus == "closethisweek")
                            {
                                // Total Projects Due in this Week                             
                                DateTime dtMonday = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                                //DateTime dtSaturday = DateTime.Now.StartOfWeek(DayOfWeek.Saturday).AddDays(7);
                                //DataRow[] tickets = dthometickets.Select($"{DatabaseObjects.Columns.Closed} = 1 and {DatabaseObjects.Columns.CloseDate} is not null and {DatabaseObjects.Columns.CloseDate} <> '' and  {DatabaseObjects.Columns.CloseDate} >= #{dtSunday}# and {DatabaseObjects.Columns.CloseDate} <= #{dtSaturday}#");
                                DataRow[] tickets = dthometickets.Select($"{DatabaseObjects.Columns.Closed} = 1 and {DatabaseObjects.Columns.CloseDate} is not null and  {DatabaseObjects.Columns.CloseDate} >= #{dtMonday}#");

                                if (tickets.Count() > 0)
                                    dashboardTickets.FilteredTable = tickets.CopyToDataTable();
                            }
                            else if (hStatus == "closedthisyear")
                            {
                                // Total Projects Closed this year.                             
                                DateTime now = DateTime.Now;
                                var firstDay = new DateTime(now.Year, 1, 1);
                                var lastDay = firstDay.AddMonths(12).AddDays(-1);

                                DataRow[] tickets = dthometickets.Select($"{DatabaseObjects.Columns.Closed} = 1 and {DatabaseObjects.Columns.CloseDate} is not null and  {DatabaseObjects.Columns.CloseDate} >= #{firstDay}# and {DatabaseObjects.Columns.CloseDate} <= #{lastDay}#");

                                if (tickets.Count() > 0)
                                    dashboardTickets.FilteredTable = tickets.CopyToDataTable();
                            }
                            else if (hStatus == "closein4weeks")
                            {
                                // Total Projects Due in 4 Week
                                var dueDate = DateTime.Now.AddDays(28);
                                var currentDate = DateTime.Now;
                                //DataRow[] tickets = dthometickets.Select($"{DatabaseObjects.Columns.Closed} <> 1  AND {DatabaseObjects.Columns.EstimatedConstructionEnd} >= #{currentDate}# AND {DatabaseObjects.Columns.EstimatedConstructionEnd} <= #{dueDate}#");

                                //if (tickets.Count() > 0)
                                //    dashboardTickets.FilteredTable = tickets.CopyToDataTable();
                            }
                            else if (hStatus == "startedthismonth")
                            {
                                DateTime now = DateTime.Now;
                                var firstDay = new DateTime(now.Year, now.Month, 1);
                                var lastDay = firstDay.AddMonths(1).AddDays(-1);

                                //DataRow[] tickets = dthometickets.Select($"{DatabaseObjects.Columns.Closed} <> 1  AND {DatabaseObjects.Columns.EstimatedConstructionStart} >= #{firstDay}# AND {DatabaseObjects.Columns.EstimatedConstructionStart} <= #{lastDay}#");

                                //if (tickets.Count() > 0)
                                //    dashboardTickets.FilteredTable = tickets.CopyToDataTable();
                            }
                            else if (hStatus == "openticketstoday")
                            {
                                List<UGITModule> smsmodules = moduleMgr.Load(x => x.ModuleType == ModuleType.SMS);
                                DataTable dttickets = new DataTable();
                                foreach (UGITModule moduleitem in smsmodules)
                                {
                                    if (string.IsNullOrEmpty(moduleitem.ModuleTable))
                                        continue;
                                    DataTable dtmoduletickets = ticketMgr.GetTickets($"select * from {moduleitem.ModuleTable} where cast({DatabaseObjects.Columns.Created} as date) = '{DateTime.Now.ToShortDateString()}' and {DatabaseObjects.Columns.Closed} <> 1");
                                    if (dtmoduletickets != null && dtmoduletickets.Rows.Count > 0)
                                        dttickets.Merge(dtmoduletickets);
                                }
                                dashboardTickets.FilteredTable = dttickets;
                            }
                            else if (hStatus == "closeticketstoday")
                            {
                                List<UGITModule> smsmodules = moduleMgr.Load(x => x.ModuleType == ModuleType.SMS);
                                DataTable dttickets = new DataTable();
                                foreach (UGITModule moduleitem in smsmodules)
                                {
                                    if (string.IsNullOrEmpty(moduleitem.ModuleTable))
                                        continue;
                                    DataTable dtmoduletickets = ticketMgr.GetTickets($"select * from {moduleitem.ModuleTable} where cast({DatabaseObjects.Columns.CloseDate} as date) = '{DateTime.Now.ToShortDateString()}' and {DatabaseObjects.Columns.Closed} = 1");
                                    if (dtmoduletickets != null && dtmoduletickets.Rows.Count > 0)
                                        dttickets.Merge(dtmoduletickets);
                                }
                                dashboardTickets.FilteredTable = dttickets;
                            }
                            else if (hStatus == "resolvedtickets")
                            {
                                UGITModule nprmodule = moduleMgr.LoadByName(ModuleNames.TSR);

                                LifeCycle lifeCycle = nprmodule.List_LifeCycles.FirstOrDefault();
                                LifeCycleStage resolvedStage = null;
                                if (lifeCycle != null)
                                    resolvedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTitle == StageType.Resolved.ToString() || x.StageTypeChoice == StageType.Resolved.ToString());
                                // Added condition above, for CPR Under Construction/ Resolved tab
                                if (resolvedStage != null)
                                {
                                    ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "myopentickets", ModuleName = ModuleNames.TSR, Status = TicketStatus.Open, UserID = userid };
                                    ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                                    statistics = modStatistics.Load(statRequest, false);
                                    DataTable openTicketsTable = new DataTable();
                                    if (statistics != null)
                                        openTicketsTable = statistics.ResultedData;
                                    if (openTicketsTable != null)
                                    {
                                        DataRow[] resolvedRows = openTicketsTable.Select(string.Format("{0} >= {1}", DatabaseObjects.Columns.StageStep, resolvedStage.StageStep));
                                        if (resolvedRows != null)
                                        {
                                            dashboardTickets.FilteredTable = resolvedRows.CopyToDataTable();
                                        }
                                    }
                                }
                            }
                            else if (hStatus == "nprtickets")
                            {
                                ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "nprtickets", ModuleName = "NPR", Status = TicketStatus.Open, UserID = userid };
                                ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                                statistics = modStatistics.Load(statRequest, false);
                                dashboardTickets.FilteredTable = statistics.ResultedData;
                            }
                        }
                        dashboardTickets.IsFilteredTableExist = true;
                        dashboardTickets.HideAllTicketTab = true;
                        dashboardTickets.HideModuleDesciption = true;
                        dashboardTickets.HideNewTicketButton = false;
                        dashboardTickets.HideModuleDetail = true;
                        dashboardTickets.HideReport = true;
                        dashboardTickets.ModuleName = ticketModuleName;
                        dashboardTickets.IsHomedashboardCtrl = true;
                        if (bool.TryParse(Request["ShowResourceAllocationBtn"], out bool ShowResourceAllocationBtn))
                            dashboardTickets.IsShowResourceAllocationBtn = ShowResourceAllocationBtn;
                        managementControls.Controls.Add(dashboardTickets);
                        break;

                    case "filedetails":

                        FileDetails fileDetails = (FileDetails)Page.LoadControl("~/ControlTemplates/Documents/FileDetails.ascx");

                        int docId = 0;

                        int.TryParse(Request["fileID"], out docId);

                        fileDetails.fileID = docId;

                        managementControls.Controls.Add(fileDetails);

                        break;

                    case "runinbackgroundservicesview":
                        RunInBackgroundServicesView runInBackgroundServicesView = (RunInBackgroundServicesView)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/RunInBackgroundServicesView.ascx");
                        // wikipermissioncontrol.TicketId = Request["currentTicketPublicID"];
                        managementControls.Controls.Add(runInBackgroundServicesView);
                        break;
                    case "associatedgroups":
                        AssociatedGroups associatedGroups = (AssociatedGroups)Page.LoadControl("~/CONTROLTEMPLATES/RMM/AssociatedGroups.ascx");

                        managementControls.Controls.Add(associatedGroups);
                        break;

                    case "addnewproject":
                        AddNewProject ctrAddNewProject = (AddNewProject)Page.LoadControl("~/CONTROLTEMPLATES/PMM/AddNewProject.ascx");
                        managementControls.Controls.Add(ctrAddNewProject);
                        break;
                    case "tasktemplateview":
                        TaskTemplateView _TaskTemplateView = (TaskTemplateView)this.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/TaskTemplateView.ascx");
                        managementControls.Controls.Add(_TaskTemplateView);
                        break;
                    case "approvereject":
                        ApproveRejectControl approveRejectCtrl = Page.LoadControl("~/CONTROLTEMPLATES/ugovernit/ApproveRejectControl.ascx") as ApproveRejectControl;
                        managementControls.Controls.Add(approveRejectCtrl);
                        break;
                    case "billingandmarginsreport":
                        BillingAndMarginsReport billingReport = Page.LoadControl("~/CONTROLTEMPLATES/RMM/BillingAndMarginsReport.ascx") as BillingAndMarginsReport;
                        managementControls.Controls.Add(billingReport);
                        break;
                    case "executivekpi":
                        ExecutiveKPI executiveKpi = Page.LoadControl("~/CONTROLTEMPLATES/RMM/ExecutiveKPI.ascx") as ExecutiveKPI;
                        managementControls.Controls.Add(executiveKpi);
                        break;
                    case "resourceutilizationindex":
                        ResourceUtilizationIndex resourceUtilizationIndex = Page.LoadControl("~/CONTROLTEMPLATES/RMM/ResourceUtilizationIndex.ascx") as ResourceUtilizationIndex;
                        managementControls.Controls.Add(resourceUtilizationIndex);
                        break;
                    case "manageallocationtemplates":
                        ManageAllocationTemplates manageallocationtemplates = Page.LoadControl("~/CONTROLTEMPLATES/RMM/ManageAllocationTemplates.ascx") as ManageAllocationTemplates;
                        managementControls.Controls.Add(manageallocationtemplates);
                        break;
                    case "criticaltasklist":
                        CriticalTaskList ctrCriticalTaskList = Page.LoadControl("~/Controltemplates/CoreUI/CriticalTaskList.ascx") as CriticalTaskList;
                        managementControls.Controls.Add(ctrCriticalTaskList);
                        break;
                    case "applicationsrelatedassetsview":
                        ApplicationsRelatedAssetsView applicationsRelatedAssetsView = (ApplicationsRelatedAssetsView)Page.LoadControl("~/controltemplates/uGovernIT/ApplicationsRelatedAssetsView.ascx");
                        applicationsRelatedAssetsView.AssetID = UGITUtility.StringToInt(Request["Id"]);
                        managementControls.Controls.Add(applicationsRelatedAssetsView);
                        break;
                    case "applicationsrelatedassetsedit":
                        ApplicationsRelatedAssetsEdit appRelatedAssetsEdit = (ApplicationsRelatedAssetsEdit)Page.LoadControl("~/controltemplates/uGovernIT/ApplicationsRelatedAssetsEdit.ascx");
                        int itemId = 0;
                        int.TryParse(Request["ItemId"], out itemId);
                        appRelatedAssetsEdit.Id = itemId;
                        int assetId = 0;
                        int.TryParse(Request["AssetId"], out assetId);
                        appRelatedAssetsEdit.AssetId = assetId;
                        managementControls.Controls.Add(appRelatedAssetsEdit);
                        break;
                    case "projectlistdrilldown":
                        CustomFilteredTickets filteredProjectListobj = (CustomFilteredTickets)Page.LoadControl("~/CONTROLTEMPLATES/Shared/CustomFilteredTickets.ascx");
                        Dictionary<string, object> values = new Dictionary<string, object>();
                        values.Add("@tenantID", HttpContext.Current.GetManagerContext().TenantID);
                        values.Add("@filter", Request["Filter"]);
                        string startDate = UGITUtility.ObjectToString(Request["StartDate"]);
                        string endDate = UGITUtility.ObjectToString(Request["EndDate"]);
                        if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                        {
                            values.Add("@Startdate", UGITUtility.StringToDateTime(startDate));
                            values.Add("@Endate", UGITUtility.StringToDateTime(endDate));
                        }
                        else
                        {
                            values.Add("@Startdate", new DateTime(DateTime.Now.Year, 1, 1));
                            values.Add("@Endate", DateTime.Now.ToShortDateString());
                        }

                        List<string> selectionList = UGITUtility.ConvertStringToList(Request["ParentSelection"], Constants.Separator2);
                        string selectionType = selectionList.First();
                        if (selectionType == "Division")
                            values.Add("@division", selectionList.Last());
                        else if (selectionType == "Sector")
                            values.Add("@sector", selectionList.Last());
                        else if (selectionType == "Studio")
                            values.Add("@studio", selectionList.Last());

                        if (Request["Base"] == "Division")
                            values.Add("@division", Request["ChildSelection"]);
                        else if (Request["Base"] == "Sector")
                            values.Add("@sector", Request["ChildSelection"]);
                        else if (Request["Base"] == "Studio")
                            values.Add("@studio", Request["ChildSelection"]);

                        DataTable dt = GetTableDataManager.GetData("ProjectList", values);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            filteredProjectListobj.FilteredTable = dt;
                            filteredProjectListobj.IsFilteredTableExist = true;
                        }
                        
                        managementControls.Controls.Add(filteredProjectListobj);
                        break;
                    case "executiveviewdrilldown":
                        {
                            CustomFilteredTickets executiveProjectsList = (CustomFilteredTickets)Page.LoadControl("~/CONTROLTEMPLATES/Shared/CustomFilteredTickets.ascx");
                            //Dictionary<string, object> pValues = new Dictionary<string, object>();
                            //pValues.Add("@TenantID", HttpContext.Current.GetManagerContext().TenantID);
                            //pValues.Add("@Filter", UGITUtility.ObjectToString(Request["Filter"]));
                            //pValues.Add("@UserID", HttpContext.Current.GetManagerContext().CurrentUser.Id);

                            //DataTable dtResult = GetTableDataManager.GetData("ExecutiveViewDrillDown", pValues);

                            string filterType = UGITUtility.ObjectToString(Request["Filter"]);
                            if(filterType == "PendingAssignment")
                            {
                                UGITModule OPMModule = ModuleManager.LoadByName(ModuleNames.OPM);
                                DataTable opmTickets = TicketManager.GetAllTickets(OPMModule);
                                if (opmTickets != null && opmTickets.Rows.Count > 0)
                                {
                                    DataRow[] assignOPMTickets = opmTickets.Select($"StageStep = 2");
                                    executiveProjectsList.FilteredTable = assignOPMTickets.CopyToDataTable();
                                    executiveProjectsList.IsFilteredTableExist = true;
                                    executiveProjectsList.ManagerViewModule = ModuleNames.OPM;
                                }
                            }
                            else if(filterType == "ConstructionContract")
                            {
                                UGITModule CPRModule = ModuleManager.LoadByName(ModuleNames.CPR);
                                DataTable cprTickets = TicketManager.GetAllTickets(CPRModule);
                                if (cprTickets != null && cprTickets.Rows.Count > 0)
                                {
                                    DataRow[] constructionTickets = cprTickets.Select($"StageStep=6 and (OnHold is null or OnHold = 0)");
                                    executiveProjectsList.FilteredTable = constructionTickets.CopyToDataTable();
                                    executiveProjectsList.IsFilteredTableExist = true;
                                    executiveProjectsList.ManagerViewModule = ModuleNames.CPR;
                                }
                            }
                            else if(filterType == "Closeout")
                            {
                                UGITModule CPRModule = ModuleManager.LoadByName(ModuleNames.CPR);
                                DataTable cprTickets = TicketManager.GetAllTickets(CPRModule);
                                if (cprTickets != null && cprTickets.Rows.Count > 0)
                                {
                                    ConfigurationVariableManager _configVariableManager = new ConfigurationVariableManager(ApplicationContext);
                                    int optimalcloseout = UGITUtility.StringToInt(_configVariableManager.GetValue(ConfigConstants.CloseoutDueDays));

                                    DataRow[] constructionTickets = cprTickets.AsEnumerable().Where(row =>row.Field<int>("StageStep") == 8 &&
                                                                                                        (row.Field<DateTime?>("CloseoutDate")?.AddDays(-optimalcloseout) ?? DateTime.MinValue) <= DateTime.Now.Date
                                                                                                        && row.Field<int?>(DatabaseObjects.Columns.OnHold) != 1).ToArray();
                                    executiveProjectsList.FilteredTable = constructionTickets.CopyToDataTable();
                                    executiveProjectsList.IsFilteredTableExist = true;
                                    executiveProjectsList.ManagerViewModule = ModuleNames.CPR;
                                }
                            }
                            executiveProjectsList.IsManagerView = true;
                            managementControls.Controls.Add(executiveProjectsList);
                        }
                        break;
                    case "impactlistdrilldown":
                        {
                            CustomFilteredTickets executiveProjectsList = (CustomFilteredTickets)Page.LoadControl("~/CONTROLTEMPLATES/Shared/CustomFilteredTickets.ascx");
                            Dictionary<string, object> pValues = new Dictionary<string, object>();
                            DataTable Opentickets = new DataTable();
                            TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
                            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                            string mname = UGITUtility.ObjectToString(Request["ModuleName"]);
                            long impactID = UGITUtility.StringToLong(Request["ImpactID"]);
                            long severityID = UGITUtility.StringToLong(Request["SeverityID"]);
                            
                            Opentickets = ticketManager.GetOpenTickets(moduleViewManager.LoadByName(mname, true));
                            DataRow[] dtRows = Opentickets.AsEnumerable().Where(x => x.Field<Int64?>(DatabaseObjects.Columns.TicketImpactLookup) == impactID
                                                && x.Field<Int64?>(DatabaseObjects.Columns.TicketSeverityLookup) == severityID).ToArray();
                            
                            executiveProjectsList.FilteredTable = dtRows.CopyToDataTable();
                            executiveProjectsList.IsFilteredTableExist = true;


                            managementControls.Controls.Add(executiveProjectsList);
                        }
                        break;
                    case "moreurllink":
                        {
                            ExecutiveDrillDown executiveDrillDown = (ExecutiveDrillDown)Page.LoadControl("~/CONTROLTEMPLATES/CoreUI/ExecutiveDrillDown.ascx");
                            managementControls.Controls.Add(executiveDrillDown);
                        }
                        break;
                    case "financialdetails":
                        {
                            FinancialDetails _financialDetails = Page.LoadControl("~/CONTROLTEMPLATES/CoreUI/FinancialDetails.ascx") as FinancialDetails;
                            _financialDetails.Filter = UGITUtility.ObjectToString(Request["filter"]);
                            _financialDetails.DataType = UGITUtility.ObjectToString(Request["type"]);
                            managementControls.Controls.Add(_financialDetails);
                        }
                        break;
                    case "allocationgantt":
                        {
                            //AllocationGantt _allocationGantt = Page.LoadControl("~/ControlTemplates/RMM/AllocationGantt.ascx") as AllocationGantt;
                            //commented allocation gantt control for now, replacing this devexteme control with new devexpress control
                            ResourceAllocationGridNew _allocationGantt = Page.LoadControl("~/ControlTemplates/RMONE/ResourceAllocationGridNew.ascx") as ResourceAllocationGridNew;
                            //userall - selectedUsers - selectedYear - includeClosed
                            _allocationGantt.UserAll = UGITUtility.ObjectToString(Request["userall"]);
                            _allocationGantt.IncludeClosed = UGITUtility.ObjectToString(Request["includeClosed"]);
                            _allocationGantt.SelectedUser = UGITUtility.ObjectToString(Request["selectedUser"]);
                            _allocationGantt.SelectedUsers = UGITUtility.ObjectToString(Request["selectedUsers"]);
                            _allocationGantt.SelectedYear = UGITUtility.ObjectToString(Request["selectedYear"]);
                            //set ticketid for use in team agent
                            _allocationGantt.TicketID = UGITUtility.ObjectToString(Request["TicketID"]);
                            managementControls.Controls.Add(_allocationGantt);
                        }
                        break;
                    case "projectgantt":
                        {
                            ProjectGantt _projectGantt = Page.LoadControl("~/ControlTemplates/RMONE/ProjectGantt.ascx") as ProjectGantt;
                            //_projectGantt.TicketID = UGITUtility.ObjectToString(Request["TicketID"]);
                            managementControls.Controls.Add(_projectGantt);
                        }
                        break;
                    case "potentialallocations":
                        {
                            PotentialAllocationsList _potentialAllocationsList = Page.LoadControl("~/ControlTemplates/Bench/PotentialAllocationsList.ascx") as PotentialAllocationsList;
                            managementControls.Controls.Add(_potentialAllocationsList);
                        }
                        break;
                    case "customprojectteamgantt":
                        {
                            //AllocationGantt _allocationGantt = Page.LoadControl("~/ControlTemplates/RMM/AllocationGantt.ascx") as AllocationGantt;
                            //commented allocation gantt control for now, replacing this devexteme control with new devexpress control
                            CustomProjectTeamGantt _allocationGantt = Page.LoadControl("~/ControlTemplates/RMONE/CustomProjectTeamGantt.ascx") as CustomProjectTeamGantt;
                            //userall - selectedUsers - selectedYear - includeClosed
                            _allocationGantt.UserAll = UGITUtility.ObjectToString(Request["userall"]);
                            _allocationGantt.IncludeClosed = UGITUtility.ObjectToString(Request["includeClosed"]);
                            _allocationGantt.SelectedUser = UGITUtility.ObjectToString(Request["selectedUser"]);
                            _allocationGantt.SelectedUsers = UGITUtility.ObjectToString(Request["selectedUsers"]);
                            _allocationGantt.SelectedYear = UGITUtility.ObjectToString(Request["selectedYear"]);
                            //set ticketid for use in team agent
                            _allocationGantt.TicketID = UGITUtility.ObjectToString(Request["TicketID"]);
                            managementControls.Controls.Add(_allocationGantt);
                        }
                        break;
                    case "addmultiallocation":
                        {
                            AddMultiAllocation _addMultiAllocation = Page.LoadControl("~/ControlTemplates/RMM/addmultiallocation.ascx") as AddMultiAllocation;
                            _addMultiAllocation.UserID = UGITUtility.ObjectToString(Request["SelectedUsersList"]);
                            _addMultiAllocation.Type = UGITUtility.ObjectToString(Request["Type"]);
                            _addMultiAllocation.WorkItem = UGITUtility.ObjectToString(Request["WorkItem"]);
                            if (!string.IsNullOrEmpty(_addMultiAllocation.UserID))
                            {
                                UserProfile selectedUser = HttpContext.Current.GetManagerContext().UserManager.GetUserById(_addMultiAllocation.UserID);
                                if (selectedUser != null)
                                {
                                    _addMultiAllocation.RoleID = selectedUser.GlobalRoleId; ;
                                }
                            }
                            managementControls.Controls.Add(_addMultiAllocation);
                        }
                        break;
                    case "multiallocationjs":
                        {
                            MultiAllocationsJS _addMultiAllocation = Page.LoadControl("~/ControlTemplates/RMONE/MultiAllocationsJS.ascx") as MultiAllocationsJS;
                            _addMultiAllocation.UserID = UGITUtility.ObjectToString(Request["SelectedUsersList"]);
                            _addMultiAllocation.Type = UGITUtility.ObjectToString(Request["Type"]);
                            _addMultiAllocation.WorkItem = UGITUtility.ObjectToString(Request["WorkItem"]);
                            if (!string.IsNullOrEmpty(_addMultiAllocation.UserID))
                            {
                                UserProfile selectedUser = HttpContext.Current.GetManagerContext().UserManager.GetUserById(_addMultiAllocation.UserID);
                                if (selectedUser != null)
                                {
                                    _addMultiAllocation.RoleID = selectedUser.GlobalRoleId;
                                }
                            }
                            managementControls.Controls.Add(_addMultiAllocation);
                        }
                        break;
                    case "ptomultiallocationjs":
                        {
                            PTOMultiAllocationsJS _addPTOMultiAllocation = Page.LoadControl("~/ControlTemplates/RMONE/PTOMultiAllocationsJS.ascx") as PTOMultiAllocationsJS;
                            _addPTOMultiAllocation.UserID = UGITUtility.ObjectToString(Request["SelectedUsersList"]);
                            _addPTOMultiAllocation.Type = UGITUtility.ObjectToString(Request["Type"]);
                            _addPTOMultiAllocation.WorkItem = UGITUtility.ObjectToString(Request["WorkItem"]);
                            if (!string.IsNullOrEmpty(_addPTOMultiAllocation.UserID))
                            {
                                UserProfile selectedUser = HttpContext.Current.GetManagerContext().UserManager.GetUserById(_addPTOMultiAllocation.UserID);
                                if (selectedUser != null)
                                {
                                    _addPTOMultiAllocation.RoleID = selectedUser.GlobalRoleId;
                                }
                            }
                            managementControls.Controls.Add(_addPTOMultiAllocation);
                        }
                        break;
                    case "combinedmultiallocationjs":
                        {
                            CombinedMultiAllocationsJS _addCombinedMultiAllocation = Page.LoadControl("~/ControlTemplates/RMONE/CombinedMultiAllocationsJS.ascx") as CombinedMultiAllocationsJS;
                            _addCombinedMultiAllocation.UserID = UGITUtility.ObjectToString(Request["SelectedUsersList"]);
                            _addCombinedMultiAllocation.Type = UGITUtility.ObjectToString(Request["Type"]);
                            _addCombinedMultiAllocation.WorkItem = UGITUtility.ObjectToString(Request["WorkItem"]);
                            if (!string.IsNullOrEmpty(_addCombinedMultiAllocation.UserID))
                            {
                                UserProfile selectedUser = HttpContext.Current.GetManagerContext().UserManager.GetUserById(_addCombinedMultiAllocation.UserID);
                                if (selectedUser != null)
                                {
                                    _addCombinedMultiAllocation.RoleID = selectedUser.GlobalRoleId;
                                }
                            }
                            managementControls.Controls.Add(_addCombinedMultiAllocation);
                        }
                        break;
                    case "openuserresume":
                        {
                            UserResume userResume = (UserResume)Page.LoadControl("~/CONTROLTEMPLATES/RMONE/UserResume.ascx");
                            userResume.UserID = UGITUtility.ObjectToString(Request["SelectedUser"]);
                            managementControls.Controls.Add(userResume);
                        }
                        break;
                    case "resourceallocationreport":
                        {
                            ResourceAllocationReport _ResourceAllocationReport = (ResourceAllocationReport)Page.LoadControl("~/CONTROLTEMPLATES/RMM/ResourceAllocationReport.ascx");
                            managementControls.Controls.Add(_ResourceAllocationReport);
                        }
                        break;
                    case "projectsummary":
                        {
                            ProjectSummary _ProjectSummary = Page.LoadControl("~/CONTROLTEMPLATES/RMONE/ProjectSummary.ascx") as ProjectSummary;
                            _ProjectSummary.TicketId = UGITUtility.ObjectToString(Request["TicketId"]);
                            //Added value to below variable to identify request coming from Summary view or DataAgent view // this is for ProjectSummary
                            if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(Request["IsSummary"])))
                            {
                                _ProjectSummary.IsRequestFromSummary = true;
                            }
                            managementControls.Controls.Add(_ProjectSummary);
                        }
                        break;
                    case "addnewtask":
                        AddNewTask addnewtask = (AddNewTask)Page.LoadControl("~/CONTROLTEMPLATES/RMONE/AddNewTask.ascx");
                        managementControls.Controls.Add(addnewtask);
                        break;
                    case "newopmwizard":
                        NewOPMWizard _NewOPMWizard = Page.LoadControl("~/CONTROLTEMPLATES/RMONE/NewOPMWizard.ascx") as NewOPMWizard;
                        managementControls.Controls.Add(_NewOPMWizard);
                        break;
                    case "bench":
                        BenchTab _Bench = (BenchTab)Page.LoadControl("~/controltemplates/bench/BenchTab.ascx");
                        _Bench.TicketId = Convert.ToString(Request["projectPublicID"]);
                        _Bench.ControlId = Convert.ToString(Request["ControlId"]);
                        _Bench.FrameId = frameId;
                        _Bench.ReadOnly = isReadOnly;
                        _Bench.CalledFromDirectorView = true;
                        managementControls.Controls.Add(_Bench);
                        break;
                    case "functionroleaddedit":
                        FunctionRoleAddEdit _functionroleedit = Page.LoadControl("~/CONTROLTEMPLATES/ugovernit/FunctionRoleAddEdit.ascx") as FunctionRoleAddEdit;
                        _functionroleedit.FunctionId = UGITUtility.StringToLong(Request["Id"]);
                        _functionroleedit.Mode = UGITUtility.ObjectToString(Request["mode"]);
                        managementControls.Controls.Add(_functionroleedit);
                        break;
                    case "functionrolemappingaddedit":
                        FunctionRoleMappingAddEdit _functionrolemapping = Page.LoadControl("~/CONTROLTEMPLATES/ugovernit/FunctionRoleMappingAddEdit.ascx") as FunctionRoleMappingAddEdit;
                        managementControls.Controls.Add(_functionrolemapping);
                        break;
                    case "benchreport":
                        BenchReport _benchReport = Page.LoadControl("~/controltemplates/bench/BenchReport.ascx") as BenchReport;
                        managementControls.Controls.Add(_benchReport);
                        break;
                    case "createprojecttags":
                        CreateProjectTags _createProjectTags = Page.LoadControl("~/Controltemplates/uGovernIT/CreateProjectTags.ascx") as CreateProjectTags;
                        managementControls.Controls.Add(_createProjectTags);
                        break;
                    default:
                        break;
                }
            }
            base.OnInit(e);
        }

        private void LoadMyRequests()
        {
            MyOpenTickets fTickets = (MyOpenTickets)Page.LoadControl("~/ControlTemplates/Shared/MyOpenTickets.ascx");
            //page size set on pageload of myopentickets
            managementControls.Controls.Add(fTickets);
        }

        private void LoadWaitingonMeRequests()
        {
            CustomFilteredTickets myWaitingOnlistView = (CustomFilteredTickets)Page.LoadControl("~/ControlTemplates/Shared/CustomFilteredTickets.ascx");
            myWaitingOnlistView.ModuleName = string.Empty;
            myWaitingOnlistView.UserType = "my";
            myWaitingOnlistView.MTicketStatus = TicketStatus.WaitingOnMe;
            if (Request["NoOfPreviewTickets"] != null && Convert.ToString(Request["NoOfPreviewTickets"]) != string.Empty)
                myWaitingOnlistView.PageSize = UGITUtility.StringToInt(Request["NoOfPreviewTickets"]);

            if (myWaitingOnlistView.PageSize <= 0)
                myWaitingOnlistView.PageSize = 10;

            myWaitingOnlistView.IsPreview = true;
            myWaitingOnlistView.HideModuleDetail = true;
            DataTable waitingOnMeData = new DataTable();
            myWaitingOnlistView.MTicketStatus = TicketStatus.WaitingOnMe;
            myWaitingOnlistView.MyHomeTab = Constants.MyHomeTicketTab;
            myWaitingOnlistView.IsHomePage = true;
            myWaitingOnlistView.CategoryName = "MyHomeTab";
            string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={0}&control={1}&NoOfPreviewTickets={2}&source={3}&IsPreview={4}", frameId, "waitingonme", 10, Uri.EscapeDataString(Request.Url.AbsolutePath), "false"));
            myWaitingOnlistView.MoreUrl = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), "Ticket Details");
            myWaitingOnlistView.HideGlobalSearch = true;
            myWaitingOnlistView.HomeTabName = "waitingonme";
            managementControls.Controls.Add(myWaitingOnlistView);
        }

        private void LoadMyTask()
        {
            MyTasks myTasks = (MyTasks)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Task/MyTasks.ascx");
            myTasks.IsPreview = true;
            if (Request["NoOfPreviewTickets"] != null && Request["NoOfPreviewTickets"] != string.Empty)
                myTasks.PageSize = UGITUtility.StringToInt(Request["NoOfPreviewTickets"]);
            if (myTasks.PageSize <= 0)
                myTasks.PageSize = 10;
            managementControls.Controls.Add(myTasks);
        }

        private void LoadAllMyTask()
        {
            MyTasks myTasks = (MyTasks)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Task/MyTasks.ascx");
            myTasks.IsPreview = true;
            if (Request["NoOfPreviewTickets"] != null && Request["NoOfPreviewTickets"] != string.Empty)
                myTasks.PageSize = UGITUtility.StringToInt(Request["NoOfPreviewTickets"]);
            if (myTasks.PageSize <= 0)
                myTasks.PageSize = 10;
            managementControls.Controls.Add(myTasks);
        }

        private void LoadMyDepartment()
        {
            CustomFilteredTickets myDepartmentlistView = (CustomFilteredTickets)Page.LoadControl("~/Controltemplates/Shared/CustomFilteredTickets.ascx");
            myDepartmentlistView.ModuleName = string.Empty;
            myDepartmentlistView.UserType = "my";
            myDepartmentlistView.MTicketStatus = TicketStatus.Department;
            if (Request["NoOfPreviewTickets"] != null && Convert.ToString(Request["NoOfPreviewTickets"]) != string.Empty)
                myDepartmentlistView.PageSize = UGITUtility.StringToInt(Request["NoOfPreviewTickets"]);
            else
                myDepartmentlistView.PageSize = 10;

            string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={0}&control={1}&NoOfPreviewTickets={2}&source={3}&IsPreview={4}", frameId, "departmentticket", 10, Uri.EscapeDataString(Request.Url.AbsolutePath), "false"));
            myDepartmentlistView.MoreUrl = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), "Ticket Details");
            myDepartmentlistView.IsPreview = true;
            myDepartmentlistView.HideModuleDetail = true;
            myDepartmentlistView.HideGlobalSearch = true;
            myDepartmentlistView.HomeTabName = "mydepartment";
            myDepartmentlistView.CategoryName = "MyHomeTab";
            myDepartmentlistView.MyHomeTab = Constants.MyHomeTicketTab;
            myDepartmentlistView.IsHomePage = true;

            managementControls.Controls.Add(myDepartmentlistView);
        }


        private void LoadMyProjects()
        {
            CustomFilteredTickets myProjectlistView = (CustomFilteredTickets)Page.LoadControl("~/controltemplates/shared/CustomFilteredTickets.ascx");
            myProjectlistView.ModuleName = string.Empty;
            myProjectlistView.UserType = "my";
            myProjectlistView.MTicketStatus = TicketStatus.MyProject;
            if (Request["NoOfPreviewTickets"] != null && Convert.ToString(Request["NoOfPreviewTickets"]) != string.Empty)
                myProjectlistView.PageSize = UGITUtility.StringToInt(Request["NoOfPreviewTickets"]);
            else
                myProjectlistView.PageSize = 10;


            string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={0}&control={1}&NoOfPreviewTickets={2}&source={3}&IsPreview={4}", frameId, "myproject", 10, Uri.EscapeDataString(Request.Url.AbsolutePath), "false"));
            myProjectlistView.MoreUrl = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), "Ticket Details");
            myProjectlistView.HomeTabName = "myproject";
            myProjectlistView.IsPreview = true;
            myProjectlistView.HideModuleDetail = true;
            myProjectlistView.MyHomeTab = Constants.MyProjectTab;
            myProjectlistView.IsHomePage = true;
            myProjectlistView.CategoryName = "MyHomeTab";
            managementControls.Controls.Add(myProjectlistView);
        }

        private void LoadMyPendingDocuments()
        {
            //AllTask MyTaskPanel = (AllTask)Page.LoadControl("~/_controltemplates/15/DocumentLibraryControlEDM/AllTask.ascx");
            //MyTaskPanel.IsPMM = true;
            //MyTaskPanel.isHome = true;
            //MyTaskPanel.pendingApprovalsHome = uHelper.GetPendingApprovals();
            //MyTaskPanel.tabCount = MyTaskPanel.pendingApprovalsHome.Rows.Count;
            //MyTaskPanel.DMSPageUrl = DelegateControlsUrl.DMHomePagePopup;
            //managementControls.Controls.Add(MyTaskPanel);
        }

        private void LoadClosedForUser()
        {
            CustomFilteredTickets closedTickets = (CustomFilteredTickets)Page.LoadControl("~/controltemplates/shared/CustomFilteredTickets.ascx");

            closedTickets.MTicketStatus = TicketStatus.Closed;
            closedTickets.UserType = "my";
            closedTickets.IsPreview = true;
            closedTickets.ModuleName = string.Empty;
            closedTickets.HideModuleDetail = true;
            closedTickets.HideGlobalSearch = true;
            closedTickets.MyHomeTab = Constants.MyHomeTicketTab;
            closedTickets.IsHomePage = true;
            //closedTickets.ID = "closedTicketGrid";
            closedTickets.HomeTabName = "myclosedtickets";
            closedTickets.EnableNewButton = Convert.ToBoolean(Request["EnableNewButton"]);
            if (Request["NoOfPreviewTickets"] != null && Convert.ToString(Request["NoOfPreviewTickets"]) != string.Empty)
                closedTickets.PageSize = UGITUtility.StringToInt(Request["NoOfPreviewTickets"]);
            else
                closedTickets.PageSize = 10;

            string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={0}&control={1}&NoOfPreviewTickets={2}&source={3}&IsPreview={4}", frameId, "myclosedtickets", 10, Uri.EscapeDataString(Request.Url.AbsolutePath), "false"));
            closedTickets.MoreUrl = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), "Ticket Details");
            closedTickets.CategoryName = Constants.MyHomeTicketTab;
            managementControls.Controls.Add(closedTickets);

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        private void GetTicketCreatedByWeeklyTrend(int currentweeks)
        {
            DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");

            DataRow[] rowColl = null;
            rowColl = dtDashboardSummary.AsEnumerable().Where(x => x.Field<DateTime>(DatabaseObjects.Columns.TicketCreationDate) >= DateTime.Now.AddDays(-7 * currentweeks)).ToArray();
            DataTable dt = new DataTable();
            CustomFilteredTickets filteredListobj = (CustomFilteredTickets)Page.LoadControl("~/CONTROLTEMPLATES/Shared/CustomFilteredTickets.ascx");

            if (rowColl != null && rowColl.Length > 0)
            {
                dt = rowColl.CopyToDataTable();
            }
            filteredListobj.FilteredTable = dt;
            filteredListobj.IsFilteredTableExist = true;
            managementControls.Controls.Add(filteredListobj);
        }

        private void GetProblemRequestFilterData(string modulename, string title)
        {
            DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");
            string[] splitcategory = null;
            if (!string.IsNullOrEmpty(title))
            {
                splitcategory = title.Split('>');
            }
            string selectquery = string.Empty;

            if (splitcategory.Length == 2)
                selectquery = string.Format("{0}='{1}' OR {2}='{3}'", DatabaseObjects.Columns.Category, splitcategory[0].Trim(), DatabaseObjects.Columns.SubCategory, splitcategory[1].Trim());
            else
                selectquery = string.Format("{0}='{1}'", DatabaseObjects.Columns.Category, splitcategory[0].Trim());

            if (!string.IsNullOrEmpty(modulename))
                selectquery = string.Format("{0} And {1}='{2}'", selectquery, DatabaseObjects.Columns.ModuleNameLookup, modulename.Trim());

            DataRow[] problemrequestData = dtDashboardSummary.Select(selectquery);
            DataTable dt = new DataTable();
            CustomFilteredTickets filteredListobj = (CustomFilteredTickets)Page.LoadControl("~/CONTROLTEMPLATES/Shared/CustomFilteredTickets.ascx");
            if (problemrequestData != null && problemrequestData.Length > 0)
            {
                dt = problemrequestData.CopyToDataTable();
            }
            filteredListobj.FilteredTable = dt;
            filteredListobj.IsFilteredTableExist = true;
            managementControls.Controls.Add(filteredListobj);
        }

        private void LoadGroupScoreCard()
        {
            DateTime dtstart = new DateTime();
            DateTime dtend = new DateTime();
            string status = string.Empty;
            string functionalarea = string.Empty;
            long? functionalAreaLookup = 0;
            if (Request["startdate"] != null && Convert.ToString(Request["startdate"]) != string.Empty)
                dtstart = Convert.ToDateTime(Convert.ToString(Request["startdate"]));
            if (Request["enddate"] != null && Convert.ToString(Request["enddate"]) != string.Empty)
                dtend = Convert.ToDateTime(Convert.ToString(Request["enddate"]));
            if (Request["status"] != null && Convert.ToString(Request["status"]) != string.Empty)
                status = Convert.ToString(Convert.ToString(Request["status"]));
            if (Request["functionalarea"] != null && Convert.ToString(Request["functionalarea"]) != string.Empty)
                functionalarea = Convert.ToString(Convert.ToString(Request["functionalarea"]));

            if (!string.IsNullOrEmpty(functionalarea))
                functionalAreaLookup = Convert.ToInt64(FieldConfigurationManager.GetFieldConfigurationIdByName(DatabaseObjects.Columns.FunctionalAreaLookup, functionalarea));
            else
                functionalAreaLookup = null;

            DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");
            DataRow[] rowCol = null;
            if (status == "Closed")
                rowCol = dtDashboardSummary.AsEnumerable().Where(ds => (ds.Field<DateTime>(DatabaseObjects.Columns.Modified) >= dtstart &&
                                     ds.Field<DateTime>(DatabaseObjects.Columns.Modified) <= dtend) &&
                                     ds.Field<string>(DatabaseObjects.Columns.TicketStatus) == "Closed" &&
                                     ds.Field<long?>(DatabaseObjects.Columns.FunctionalAreaLookup) == functionalAreaLookup
                                     ).ToArray();
            else
            {
                rowCol = dtDashboardSummary.AsEnumerable().Where(ds => (ds.Field<DateTime>(DatabaseObjects.Columns.Modified) >= dtstart &&
                                        ds.Field<DateTime>(DatabaseObjects.Columns.Modified) <= dtend) &&
                                        ds.Field<string>(DatabaseObjects.Columns.TicketStatus) != "Closed" &&
                                        ds.Field<long?>(DatabaseObjects.Columns.FunctionalAreaLookup) == functionalAreaLookup
                                        ).ToArray();
            }
            DataTable dt = new DataTable();

            if (rowCol != null && rowCol.Length > 0)
                dt = rowCol.CopyToDataTable();
            CustomFilteredTickets grpScoreCardDrillDown = (CustomFilteredTickets)Page.LoadControl("~/controltemplates/Shared/CustomFilteredTickets.ascx");
            grpScoreCardDrillDown.FilteredTable = dt;
            managementControls.Controls.Add(grpScoreCardDrillDown);
        }

        private void LoadPredictedBacklog()
        {
            int weeks = 0;
            string status = string.Empty;
            string functionalarea = string.Empty;
            long? functionalAreaLookup = 0;
            if (Request["weeks"] != null && Convert.ToString(Request["weeks"]) != string.Empty)
                weeks = UGITUtility.StringToInt(Convert.ToString(Request["weeks"]));
            if (Request["status"] != null && Convert.ToString(Request["status"]) != string.Empty)
                status = Convert.ToString(Convert.ToString(Request["status"]));
            if (Request["functionalarea"] != null && Convert.ToString(Request["functionalarea"]) != string.Empty)
                functionalarea = Convert.ToString(Convert.ToString(Request["functionalarea"]));

            if (!string.IsNullOrEmpty(functionalarea))
                functionalAreaLookup = Convert.ToInt64(FieldConfigurationManager.GetFieldConfigurationIdByName(DatabaseObjects.Columns.FunctionalAreaLookup, functionalarea));
            else
                functionalAreaLookup = null;

            DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");
            DataRow[] rowCol = null;
            if (status == "Unsolved")
            {
                rowCol = dtDashboardSummary.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.TicketStatus) != "Closed" && x.Field<long?>(DatabaseObjects.Columns.FunctionalAreaLookup) == functionalAreaLookup).ToArray();
            }
            else
            {
                rowCol = dtDashboardSummary.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.TicketStatus) == "Closed" && x.Field<long?>(DatabaseObjects.Columns.FunctionalAreaLookup) == functionalAreaLookup && x.Field<DateTime>(DatabaseObjects.Columns.Modified) >= DateTime.Now.AddDays(-(weeks * 7))).ToArray();
            }
            DataTable dt = new DataTable();

            if (rowCol != null && rowCol.Length > 0)
                dt = rowCol.CopyToDataTable();
            CustomFilteredTickets grpPredictedBacklogDrillDown = (CustomFilteredTickets)Page.LoadControl("~/controltemplates/Shared/CustomFilteredTickets.ascx");
            grpPredictedBacklogDrillDown.FilteredTable = dt;
            managementControls.Controls.Add(grpPredictedBacklogDrillDown);
        }

        private void GetWeeklyDrilData(string assigneePRP, int weeks, string type)
        {
            DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");
            DataRow[] rowColl = null;
            if (type == "solve")
            {
                rowColl = dtDashboardSummary.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.TicketPRP) == assigneePRP && x.Field<string>(DatabaseObjects.Columns.TicketStatus) == "Closed" && x.Field<DateTime>(DatabaseObjects.Columns.Modified) >= DateTime.Now.AddDays(-7)).ToArray();
            }

            else if (type == "weekly")
            {
                rowColl = dtDashboardSummary.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.TicketPRP) == assigneePRP && x.Field<string>(DatabaseObjects.Columns.TicketStatus) == "Closed" && x.Field<DateTime>(DatabaseObjects.Columns.Modified) >= DateTime.Now.AddDays(-(weeks * 7))).ToArray();
            }

            DataTable dt = new DataTable();
            CustomFilteredTickets filteredListobj = (CustomFilteredTickets)Page.LoadControl("~/CONTROLTEMPLATES/Shared/CustomFilteredTickets.ascx");

            if (rowColl != null && rowColl.Length > 0)
                dt = rowColl.CopyToDataTable();

            filteredListobj.FilteredTable = dt;
            filteredListobj.IsFilteredTableExist = true;
            managementControls.Controls.Add(filteredListobj);
        }

        private void GetGroupUnsolvedData(string ticketgroup)
        {
            DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");
            List<string> queryExp = new List<string>();
            if (!string.IsNullOrEmpty(ticketgroup))
                queryExp.Add(string.Format(" {0} = {1}", DatabaseObjects.Columns.FunctionalAreaLookup, ticketgroup));
            else
                queryExp.Add(string.Format(" {0} is null", DatabaseObjects.Columns.FunctionalAreaLookup, ticketgroup));

            queryExp.Add(string.Format(" {0} <> 'Closed'", DatabaseObjects.Columns.TicketStatus));

            DataRow[] rowUnsolve = dtDashboardSummary.Select(string.Join(" And ", queryExp));
            DataTable dt = new DataTable();
            CustomFilteredTickets filteredListobj = (CustomFilteredTickets)Page.LoadControl("~/CONTROLTEMPLATES/Shared/CustomFilteredTickets.ascx");
            if (rowUnsolve != null && rowUnsolve.Length > 0)
                dt = rowUnsolve.CopyToDataTable();

            filteredListobj.FilteredTable = dt;
            filteredListobj.IsFilteredTableExist = true;
            managementControls.Controls.Add(filteredListobj);
        }

        private void GetSLADrilledData(string modulename, string title, string dateFilter, bool isSLAMetrics = false, int ruleLookupId = 0, bool includeOpen = false, int ForSVCId = 0)
        {
            DateTime cStartDate = DateTime.MinValue, cEndDate = DateTime.MinValue;
            DateTime comStartDate = DateTime.MinValue, comEndDate = DateTime.MaxValue;
            string stringText = string.Empty;
            string filterView = "last 30 days";
            DataTable slaTable = new DataTable();

            List<Tuple<string, DateTime, DateTime>> lstFilter = new List<Tuple<string, DateTime, DateTime>>();

            if (!string.IsNullOrWhiteSpace(dateFilter))
            {
                string[] filterArr = Regex.Split(dateFilter, @"\~#");
                if (!isSLAMetrics)
                {
                    if (filterArr.Length == 2)
                    {
                        if (Convert.ToString(filterArr[1]).Equals("Custom") && Request["CustomDateFilter"] != null)
                        {
                            string[] globalOne = UGITUtility.SplitString(Request["CustomDateFilter"], Constants.Separator4, StringSplitOptions.None);
                            string[] customArr = UGITUtility.SplitString(globalOne.Length == 2 ? globalOne[1] : globalOne[0], Constants.Separator9, StringSplitOptions.None);

                            comStartDate = UGITUtility.StringToDateTime(customArr[0]);
                            comEndDate = UGITUtility.StringToDateTime(customArr[1]);
                        }
                        else
                            uHelper.GetStartEndDateFromDateView(filterArr[1], ref comStartDate, ref comEndDate, ref stringText);

                        lstFilter.Add(new Tuple<string, DateTime, DateTime>("CompletedOn", comStartDate, comEndDate));
                    }

                    if (filterArr.Length > 0)
                    {
                        if (Convert.ToString(filterArr[0]).Equals("Custom") && Request["CustomDateFilter"] != null)
                        {
                            string[] globalOne = UGITUtility.SplitString(Request["CustomDateFilter"], Constants.Separator4, StringSplitOptions.None);
                            string[] customArr = UGITUtility.SplitString(globalOne[0], Constants.Separator9, StringSplitOptions.None);
                            cStartDate = UGITUtility.StringToDateTime(customArr[0]);
                            cEndDate = UGITUtility.StringToDateTime(customArr[1]);
                        }
                        else
                            uHelper.GetStartEndDateFromDateView(filterArr[0], ref cStartDate, ref cEndDate, ref stringText);

                        lstFilter.Add(new Tuple<string, DateTime, DateTime>("CreatedOn", cStartDate, cEndDate));
                    }
                    else
                    {
                        lstFilter.Add(new Tuple<string, DateTime, DateTime>("CreatedOn", cStartDate, cEndDate));
                        lstFilter.Add(new Tuple<string, DateTime, DateTime>("CompletedOn", comStartDate, comEndDate));
                    }

                    slaTable = uHelper.GetClosedTicketSLASummaryData(HttpContext.Current.GetManagerContext(), modulename, lstFilter, slaName: title, includeOpen: includeOpen, svcId: ForSVCId);
                }
                else
                {
                    if (Convert.ToString(filterArr[0]).Equals("Custom") && Request["CustomDateFilter"] != null)
                    {
                        string[] globalOne = UGITUtility.SplitString(Request["CustomDateFilter"], Constants.Separator4, StringSplitOptions.None);
                        string[] customArr = UGITUtility.SplitString(globalOne.Length == 2 ? globalOne[1] : globalOne[0], Constants.Separator9, StringSplitOptions.None);
                        cStartDate = UGITUtility.StringToDateTime(customArr[0]);
                        cEndDate = UGITUtility.StringToDateTime(customArr[1]);
                    }
                    else
                        uHelper.GetStartEndDateFromDateView(filterArr[0], ref cStartDate, ref cEndDate, ref stringText);
                    lstFilter.Add(new Tuple<string, DateTime, DateTime>("CreatedOn", cStartDate, cEndDate));
                    lstFilter.Add(new Tuple<string, DateTime, DateTime>("CompletedOn", comStartDate, comEndDate));
                    bool showOpenClosed = UGITUtility.StringToBoolean(filterArr[1]);
                    slaTable = uHelper.GetClosedTicketSLASummaryData(ApplicationContext, modulename, lstFilter, title, includeOpen: showOpenClosed, criteria: DatabaseObjects.Columns.RuleNameLookup, ruleLookupId: ruleLookupId, svcId: ForSVCId);//Get open tickets
                }

            }
            else
            {
                uHelper.GetStartEndDateFromDateView(filterView, ref cStartDate, ref cEndDate, ref stringText);
                lstFilter.Add(new Tuple<string, DateTime, DateTime>("CreatedOn", cEndDate, cEndDate));
                lstFilter.Add(new Tuple<string, DateTime, DateTime>("CompletedOn", cEndDate, cEndDate));
                slaTable = uHelper.GetClosedTicketSLASummaryData(ApplicationContext, modulename, lstFilter, slaName: title, includeOpen: includeOpen, svcId: ForSVCId);//Get closed tickets with default filter
            }

            CustomFilteredTickets filteredListobj = (CustomFilteredTickets)Page.LoadControl("~/CONTROLTEMPLATES/Shared/CustomFilteredTickets.ascx");
            int workingHoursInDays = uHelper.GetWorkingHoursInADay(ApplicationContext, true);
            if (workingHoursInDays <= 0)
                workingHoursInDays = 24;

            string postFix = "Days";
            if (Request["dUnit"] != null && Request["dUnit"].Equals("h"))
            {
                postFix = "Hrs";
                workingHoursInDays = 1;
            }

            if (slaTable != null)
            {
                if (!uHelper.IfColumnExists(DatabaseObjects.Columns.TicketTotalHoldDuration, slaTable))
                    slaTable.Columns.Add(DatabaseObjects.Columns.TicketTotalHoldDuration);

                slaTable.Columns[DatabaseObjects.Columns.TicketTotalHoldDuration].ColumnName = string.Format("Hold Time (mins)");

                foreach (DataRow row in slaTable.Rows)
                {
                    row[DatabaseObjects.Columns.TargetTime] = UGITUtility.StringToDouble(row[DatabaseObjects.Columns.TargetTime]) / (60 * workingHoursInDays);
                    row[DatabaseObjects.Columns.ActualTime] = UGITUtility.StringToDouble(row[DatabaseObjects.Columns.ActualTime]) / (60 * workingHoursInDays);
                }

                slaTable.Columns[DatabaseObjects.Columns.TargetTime].ColumnName = string.Format("Target {0}", postFix);
                slaTable.Columns[DatabaseObjects.Columns.ActualTime].ColumnName = string.Format("Actual {0}", postFix);

                //group by start date and end date
                slaTable.DefaultView.Sort = string.Format("{0} asc, {1} asc", DatabaseObjects.Columns.StageStartDate, DatabaseObjects.Columns.StageEndDate);

                slaTable = slaTable.DefaultView.ToTable(true, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.Title,
                    DatabaseObjects.Columns.StartStageName, DatabaseObjects.Columns.StageStartDate, DatabaseObjects.Columns.EndStageName,
                    DatabaseObjects.Columns.StageEndDate, string.Format("Target {0}", postFix), string.Format("Actual {0}", postFix), string.Format("Hold Time (mins)"));

                slaTable = slaTable.DefaultView.ToTable();
                filteredListobj.TimeUnit = postFix;
            }

            filteredListobj.FilteredTable = slaTable;
            filteredListobj.IsFilteredTableExist = true;
            filteredListobj.ShowTimeToo = true;
            filteredListobj.IsSLAMetricsDrilldown = true;
            filteredListobj.HideModuleDesciption = true;
            filteredListobj.HideModuleDetail = true;
            filteredListobj.HideNewTicketButton = true;
            filteredListobj.HideReport = true;
            managementControls.Controls.Add(filteredListobj);
        }

        private void GetSLAMetricsData(string slaMetricsmodule, string slametricsCategory, string slaMetricsdateFilter, string slaName, int ruleLookupId, int ForSVCId)
        {
            string title = string.Empty;
            if (slaMetricsmodule == ModuleNames.SVC)
                title = string.Format("{0}", slaName);
            else
                title = string.Format("{0} - {1}", slametricsCategory, slaName);

            GetSLADrilledData(slaMetricsmodule, title, slaMetricsdateFilter, true, ruleLookupId: ruleLookupId, ForSVCId: ForSVCId);
        }
        private void LoadAssetsForParticularUser()
        {
            if (Request["UserId"] != null)
            {
                DataTable dt = AssetHelper.GetUserAssets(HttpContext.Current.GetManagerContext(), Request["UserId"].ToString());
                CustomFilteredTickets filteredListobj = (CustomFilteredTickets)Page.LoadControl("~/ControlTemplates/Shared/CustomFilteredTickets.ascx");

                filteredListobj.ModuleName = "CMDB";
                filteredListobj.FilteredTable = dt;
                filteredListobj.IsFilteredTableExist = true;
                filteredListobj.HideNewTicketButton = true;
                filteredListobj.HideModuleDetail = true;
                filteredListobj.MyHomeTab = "MyAssets";
                filteredListobj.IsHomePage = true;
                managementControls.Controls.Add(filteredListobj);
            }
        }

    }
}
