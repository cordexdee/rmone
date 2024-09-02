using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Data;
using DevExpress.Web;
using System.Text;
using System.Web.UI.HtmlControls;
using DevExpress.Web.Rendering;
using System.Collections.Specialized;
using System.Threading;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Manager.Managers;
using System.Drawing;

namespace uGovernIT.Web
{
    public partial class TaskList : UserControl
    {
        //Public Properties
        public int TicketID { get; set; }
        public int baselineTaskThreshold;

        protected bool isReadOnly;
        public string ModuleName { get; set; }
        public string FrameId { get; set; }
        public string TicketPublicId { get; set; }
        public string editTaskFormUrl = string.Empty;
        public string saveAsTemplatePageUrl;
        public string ajaxHelperURL;
        UGITTask moduleInstDepny;
        public bool IsReadOnly { get; set; }
        public bool ShowBaseline { get; set; }
        public bool EnableImportExport { get; set; }
        public bool DisableBatchEdit { get; set; }
        public bool DisableMarkAsComplete { get; set; }
        public bool DisableNewRecuringTask { get; set; }

        public double BaselineNum { get; set; }

        string openChildrenCookie = "openchildren";
        //private bool isBindTaskDone;
        private const string absoluteUrlView = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&TicketId={2}&Type={3}";
        public string ServiceTaskWorkFlow = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/delegatecontrol.aspx?control=servicetaskworkfLow");

        protected string ganttReportUrl = string.Empty;
        protected string calendarURL = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/delegatecontrol.aspx");

        protected bool enableEdit;
        protected bool keepActualHourMandatory = false;
        protected bool ShowCriticalPath { get; set; }

        protected int milestoneTaskcount = 0;

        ImportExportTasks importControl;

        DataTable moduleTicketList = null;
        DataTable moduleTaskList = null;
        private DataTable taskTable = null;
        DataRow ticketItem = null;

        UGITModule moduleObj;
        UserProfileManager UserManager;
        UserProfile User;

        public List<UGITTask> CriticalPathTasks { get; set; }

        List<OpenTaskToKeepState> taskExpandStates;
        List<ModuleColumn> moduleColumns;

        ApplicationContext _context = null;
        FieldConfiguration field = null;
        FieldConfigurationManager _fmanger = null;
        TicketManager _ticketManager = null;
        ResourceAllocationManager _resourceAllocationManager = null;
        ModuleViewManager _moduleViewManager = null;
        ConfigurationVariableManager _configVariableHelper = null;
        UGITTaskManager _taskManager = null;
        LifeCycleManager lifeCycleHelper = new LifeCycleManager(HttpContext.Current.GetManagerContext());


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
                if (_fmanger == null)
                {
                    _fmanger = new FieldConfigurationManager(ApplicationContext);
                }
                return _fmanger;
            }
        }

        protected TicketManager TicketManager
        {
            get
            {
                if (_ticketManager == null)
                {
                    _ticketManager = new TicketManager(ApplicationContext);
                }
                return _ticketManager;
            }
        }

        protected ResourceAllocationManager ResourceAllocationManager
        {
            get
            {
                if (_resourceAllocationManager == null)
                {
                    _resourceAllocationManager = new ResourceAllocationManager(ApplicationContext);
                }
                return _resourceAllocationManager;
            }
        }

        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(ApplicationContext);
                }
                return _moduleViewManager;
            }
        }

        protected ConfigurationVariableManager ConfigurationVariableManager
        {
            get
            {
                if (_configVariableHelper == null)
                {
                    _configVariableHelper = new ConfigurationVariableManager(ApplicationContext);
                }
                return _configVariableHelper;
            }
        }

        protected UGITTaskManager UGITTaskManager
        {
            get
            {
                if (_taskManager == null)
                {
                    _taskManager = new UGITTaskManager(ApplicationContext);
                }
                return _taskManager;
            }
        }

        private class GlobalTicket
        {
            public static string ticketid;
        }

        protected override void OnInit(EventArgs e)
        {
            ajaxHelperURL = UGITUtility.GetAbsoluteURL("/api/Account/");

            UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

            User = HttpContext.Current.CurrentUser();

            if (!string.IsNullOrEmpty(ModuleName))
            {
                string PredIds = string.Empty;
                if (ModuleName == "SVCConfig")
                {
                    moduleTaskList = UGITTaskManager.LoadTaskTable(ModuleName, TicketPublicId);
                    //UGITUtility.ToDataTable<UGITTask>(UGITTaskManager.LoadByProjectID(ModuleName, TicketPublicId));
                    //Set type like account, access or module task
                    if (moduleTaskList != null && moduleTaskList.Rows.Count > 0)
                    {
                        if (!moduleTaskList.Columns.Contains("ModuleType"))
                            moduleTaskList.Columns.Add("ModuleType");

                        GetTaskType(moduleTaskList);
                    }

                    GlobalTicket.ticketid = TicketPublicId;
                    ganttDiv.Visible = false;
                    btncriticalpath.Visible = false;
                    rightToolDiv.Visible = false;

                }
                else
                {
                    moduleObj = ModuleViewManager.LoadByName(ModuleName);
                    GlobalTicket.ticketid = TicketPublicId;
                    moduleTicketList = TicketManager.GetAllTickets(moduleObj);
                    ticketItem = moduleTicketList.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, TicketPublicId))[0];
                    moduleTaskList = UGITTaskManager.LoadTaskTable(ModuleName, TicketPublicId);
                    //UGITTaskManager.LoadTaskTable(ModuleName, TicketPublicId);
                    //UGITUtility.ToDataTable<UGITTask>(UGITTaskManager.LoadByProjectID(ModuleName, TicketPublicId));
                }

                for (int rowIndex = 0; rowIndex < moduleTaskList.Rows.Count; rowIndex++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(moduleTaskList.Rows[rowIndex][DatabaseObjects.Columns.Predecessors])))
                    {
                        PredIds = Convert.ToString(moduleTaskList.Rows[rowIndex][DatabaseObjects.Columns.Predecessors]);
                        moduleTaskList.Rows[rowIndex][DatabaseObjects.Columns.Predecessors] = UGITUtility.GetPredecessors(moduleTaskList, PredIds);
                    }
                }
            }


            if (ModuleName != "SVCConfig")
            {
                if (Ticket.IsActionUser(ApplicationContext, ticketItem, User) || UserManager.IsTicketAdmin(User)
                    || UserManager.IsDataEditor(ticketItem, User))
                {
                    enableEdit = true;
                    if (ModuleName == "SVC")                                 //subtask,new ticket,existing ticket link appears for svc only
                    {
                        if (!ConfigurationVariableManager.GetValueAsBool(ConfigConstants.DisableSVCNewSubTasks))
                        {
                            addTasksContainer.Visible = true;
                            btNewTask.Attributes.Add("style", "display:block;float:right;padding-left:10px;");
                            //btNewTask.Attributes.Add("class", "gridLinkbutton");
                        }
                        if (!ConfigurationVariableManager.GetValueAsBool(ConfigConstants.DisableSVCNewSubTickets))
                        {
                            addTasksContainer.Visible = true;
                            aAddItem.Attributes.Add("style", "display:block;float:right;padding-left:10px;");
                            aAddNewSubTicket.Attributes.Add("style", "display:block;float:right;padding-left:10px;");
                        }
                    }
                }
            }
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, "listpicker", "Picker List", TicketPublicId, "ServiceSubTicket"));
            aAddItem.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','95','95',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), "Picker List"));

            gridTaskList.BatchUpdate += gridTaskList_BatchUpdate;
            gridTaskList.HtmlRowPrepared += gridTaskList_HtmlRowPrepared;
            gridTaskList.HtmlRowCreated += gridTaskList_HtmlRowCreated;
            gridTaskList.SummaryDisplayText += gridTaskList_SummaryDisplayText;
            gridTaskList.CellEditorInitialize += gridTaskList_CellEditorInitialize;
            gridTaskList.Settings.GridLines = GridLines.Horizontal;
            gridTaskList.RowCommand += gridTaskList_RowCommand;
            //gridTaskList.Styles.Header.Border.BorderStyle = BorderStyle.None;
            //gridTaskList.Theme = "CustomMaterial";
            //if (IsReadOnly)
            //{
            //    tasktoolbar.Visible = false;
            //}
            //else
            //{
            gridTaskList.SettingsEditing.Mode = GridViewEditingMode.Batch;
            gridTaskList.SettingsEditing.NewItemRowPosition = GridViewNewItemRowPosition.Bottom;
            gridTaskList.SettingsEditing.BatchEditSettings.StartEditAction = GridViewBatchStartEditAction.Click;
            gridTaskList.SettingsEditing.BatchEditSettings.EditMode = GridViewBatchEditMode.Cell;
            gridTaskList.SettingsEditing.BatchEditSettings.ShowConfirmOnLosingChanges = false;
            //}

            if (ModuleName != ModuleNames.NPR && ModuleName != ModuleNames.PMM && ModuleName != ModuleNames.TSK)
                tasktoolbar.Attributes.CssStyle.Add(HtmlTextWriterStyle.Display, "none");

            LoadModuleColumns();
            GenerateColumns();
            if (DisableBatchEdit)
                gridTaskList.SettingsEditing.Mode = GridViewEditingMode.Inline;
            if (DisableMarkAsComplete)
                btMarkComplete.Visible = false;
            gridTaskList.DataSource = moduleTaskList;
            gridTaskList.DataBind();
            openChildrenCookie += TicketPublicId;

            // Enable/ Disable import export
            //remove this condition from below if uHelper.StringToBoolean(ConfigurationVariable.GetValue(DatabaseObjects.Columns.MSProjectImportExportEnabled)) && 
            if (!EnableImportExport)
            {
                importControl = (ImportExportTasks)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Task/ImportExportTasks.ascx");
                exportImportPanel.Controls.Add(importControl);
            }

            EnableImportExport = true;

            saveAsTemplatePageUrl = string.Format("window.parent.UgitOpenPopupDialog('{0}', 'control=savetasktemplates&projectid={1}&moduleName={2}', 'Save as Template', '400px', '300px', 0)", UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/delegatecontrol.aspx"), TicketPublicId, ModuleName);
            btSaveAsTemplate.Attributes.Add("onclick", saveAsTemplatePageUrl);
            string loadTemplatePageUrl = string.Format("window.parent.UgitOpenPopupDialog('{0}', 'control=loadtasktemplate&projectid={1}&moduleName={2}', 'Load Task Template', '500px', '80', 0, '{3}')", UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/delegatecontrol.aspx"), TicketPublicId, ModuleName, Uri.EscapeUriString(Request.Url.AbsolutePath));
            btLoadTempate.Attributes.Add("onclick", loadTemplatePageUrl);

            string importTemplatePageUrl = string.Format("window.parent.UgitOpenPopupDialog('{0}', 'control=importtasktemplate&projectid={1}&moduleName={2}', 'Import Task Template', '700px', '550px', 0, '{3}')", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/delegatecontrol.aspx"), TicketPublicId, ModuleName, Uri.EscapeUriString(Request.Url.AbsolutePath));
            btImportTaskTemplate.Attributes.Add("onclick", importTemplatePageUrl);

            string oVal = Uri.UnescapeDataString(UGITUtility.GetCookieValue(Request, openChildrenCookie));
            taskExpandStates = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OpenTaskToKeepState>>(oVal);

            keepActualHourMandatory = ConfigurationVariableManager.GetValueAsBool(string.Format("{0}Task_MakeActualHoursMandatory", ModuleName));

            int.TryParse(ConfigurationVariableManager.GetValue("CreateBaselineTaskThreshold"), out baselineTaskThreshold);

            //SPDelta 31 (Disable adding new tasks/sub-tickets and open existing tasks in read-only mode if ticket status is On-Hold/Closed/Canceled)
            if (ModuleName != "SVCConfig")
            {
                ticketItem = TicketManager.GetTicketTableBasedOnTicketId(ModuleName, TicketPublicId).Select()[0];
                moduleInstDepny = UGITTaskManager.LoadByID(ticketItem.Field<long>("ID"));

                string svcTicketStatus = UGITUtility.ObjectToString(ticketItem[DatabaseObjects.Columns.Status]);
                // Hide action buttons if below condition is true
                if ((svcTicketStatus.ToLower() == "closed") || (svcTicketStatus.ToLower() == "on hold") || (svcTicketStatus.ToLower() == "cancelled"))
                {
                    addTasksContainer.Visible = false;
                    btNewTask.Attributes.Add("style", "display:none;padding-left:10px;");
                    aAddItem.Attributes.Add("style", "display:none;padding-left:10px;");
                    aAddNewSubTicket.Attributes.Add("style", "display:none;padding-left:10px;");
                    isReadOnly = true;
                }
                else if (UserManager.IsActionUser(ticketItem, User) || UserManager.IsTicketAdmin(User))
                {
                    addTasksContainer.Visible = true;
                    btNewTask.Attributes.Add("style", "display:block;padding-left:10px;");
                    aAddItem.Attributes.Add("style", "display:block;padding-left:10px;");
                    aAddNewSubTicket.Attributes.Add("style", "display:block;padding-left:10px;");
                    isReadOnly = false;
                }
                else
                {
                    addTasksContainer.Visible = false;
                    btNewTask.Attributes.Add("style", "display:none;padding-left:10px;");
                    aAddItem.Attributes.Add("style", "display:none;padding-left:10px;");
                    aAddNewSubTicket.Attributes.Add("style", "display:none;padding-left:10px;");
                    isReadOnly = true;
                }


            }
        }
        //SPDelta 31 (Disable adding new tasks/sub-tickets and open existing tasks in read-only mode if ticket status is On-Hold/Closed/Canceled)
        private bool IsCurrentUserAssignedToTask(UGITTask moduleInstDepny)
        {
            if (moduleInstDepny == null || string.IsNullOrEmpty(moduleInstDepny.AssignedTo) || UserManager.GetUserInfosById(moduleInstDepny.AssignedTo).Count <= 0)
                return false;

            List<string> lstAssignedUsers = moduleInstDepny.AssignedTo.Split(',').ToList();

            // Check if current user is in the list
            bool userExists = lstAssignedUsers.Any(x => x == User.Id);

            // check if lstAssignedUsers contains any group
            // if yes then check if current user exists in that group
            if (!userExists)
            {

                foreach (string assignedUserId in lstAssignedUsers)
                {
                    if (UserManager.CheckUserIsGroup(assignedUserId))
                    {
                        userExists = UserManager.CheckUserInGroup(User.Id, assignedUserId);
                    }
                }
            }

            return userExists;
        }
        //
        protected void Page_Load(object sender, EventArgs e)
        {
            editTaskFormUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx");
            ganttReportUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/delegatecontrol.aspx?control=ganttreport");
            if (importControl != null && !IsReadOnly && !ShowBaseline && ticketItem != null)
            {
                importControl.TicketId = TicketPublicId;
                importControl.moduleName = ModuleName;
                importControl.PublicTicketID = Convert.ToString(UGITUtility.GetSPItemValue(ticketItem, DatabaseObjects.Columns.TicketId));
            }

            if (Request["__EVENTTARGET"] != null)
            {
                if (Convert.ToString(Request["__EVENTTARGET"]).Contains("btMarkComplete"))
                {
                    long TaskID = 0;
                    UGITUtility.IsNumber(Request["__EVENTARGUMENT"], out TaskID);
                    if (TaskID != 0)
                        MarkTaskAsComplete(Convert.ToInt32(TaskID));
                }
            }

            if (Request.Form["__CALLBACKPARAM"] != null)
            {
                string[] val = Request.Form["__CALLBACKPARAM"].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                if (Request.Form["__CALLBACKPARAM"].Contains("CUSTOMCALLBACK"))
                {

                    if (Request.Form["__CALLBACKPARAM"].ToString().Contains("DRAGROW"))
                    {
                        if (val.Length > 1)
                            UpdateTaskListItemOrder(val[val.Length - 2].Replace(";", string.Empty), val[val.Length - 1].Replace(";", string.Empty));
                    }

                    if (Request.Form["__CALLBACKPARAM"].ToString().Contains("MOVEDOWN"))
                    {
                        if (val.Length > 1)
                            UpdateTaskListItemOrder(val[val.Length - 2].Replace(";", string.Empty), val[val.Length - 1].Replace(";", string.Empty), "MOVEDOWN");
                    }

                    if (Request.Form["__CALLBACKPARAM"].ToString().Contains("MOVEUP"))
                    {
                        if (val.Length > 1)
                            UpdateTaskListItemOrder(val[val.Length - 2].Replace(";", string.Empty), val[val.Length - 1].Replace(";", string.Empty), "MOVEUP");
                    }
                }
            }

            if (ModuleName == ModuleNames.SVC)
            {
                //BTS-22-000826 : Graphic view should be visible for all
                addTasksContainer.Visible = true;
                btnGraphicView.Visible = true;
                btnGraphicView.Attributes.Add("onclick", "gotoTaskWorkFlow()");
            }
        }

        private void UpdateTaskListItemOrder(string dragRow, string targetRow)
        {
            UpdateTaskListItemOrder(dragRow, targetRow, "DRAGROW");
        }

        private void UpdateTaskListItemOrder(string dragRow, string targetRow, string callbackparam)
        {
            List<UGITTask> tasks = UGITTaskManager.LoadByProjectID(ModuleName, TicketPublicId);
            tasks = UGITTaskManager.MapRelationalObjects(tasks);

            int dragrowId = Convert.ToInt32(dragRow);
            int targetRowId = Convert.ToInt32(targetRow);
            UGITTask dragTasks = tasks.Where(x => x.ID == dragrowId).FirstOrDefault();
            UGITTask targetTasks = tasks.Where(x => x.ID == targetRowId).FirstOrDefault();

            if (targetTasks.ParentTaskID > 0)
            {
                UGITTaskManager.AddChildTask(targetTasks.ParentTaskID, dragTasks.ID, ref tasks);
            }

            List<UGITTask> temptaskList = null;

            if (targetTasks.ParentTaskID > 0)
            {
                var childIndex = targetTasks.ParentTask.ChildTasks.IndexOf(dragTasks);
                int childtargetIndex;
                if (callbackparam == "MOVEDOWN")
                {
                    childtargetIndex = targetTasks.ParentTask.ChildTasks.IndexOf(targetTasks) + 1;
                }
                else
                {
                    childtargetIndex = targetTasks.ParentTask.ChildTasks.IndexOf(targetTasks);
                }
                targetTasks.ParentTask.ChildTasks.RemoveAt(childIndex);
                targetTasks.ParentTask.ChildTasks.Insert(childtargetIndex, dragTasks);
                temptaskList = targetTasks.ParentTask.ChildTasks;

            }
            else
            {
                temptaskList = tasks.Where(x => x.ParentTaskID == 0).ToList();
                temptaskList.Remove(dragTasks);

                if (callbackparam == "MOVEDOWN")
                {
                    temptaskList.Insert(temptaskList.IndexOf(targetTasks) + 1, dragTasks);
                }
                if (callbackparam == "MOVEUP")
                {
                    temptaskList.Insert(temptaskList.IndexOf(targetTasks), dragTasks);
                }
                if (callbackparam == "DRAGROW")
                {
                    temptaskList.Insert(temptaskList.IndexOf(targetTasks), dragTasks);
                }

            }

            for (int i = 0; i < temptaskList.Count; i++)
            {
                temptaskList[i].ItemOrder = i + 1;
            }

            UGITTaskManager.ReOrderTasks(ref tasks);
            UGITTaskManager.SaveTasks(ref tasks, ModuleName, TicketPublicId);

            //if (ModuleName != "NPR")
            //TaskCache.ReloadProjectTasks(ModuleName, TicketPublicId);

            BindProjectTasks();
        }

        void LoadModuleColumns()
        {
            ModuleColumnManager moduleColumnManager = new ModuleColumnManager(ApplicationContext);
            if (ModuleName == ModuleNames.SVC)
            {
                moduleColumns = moduleColumnManager.GetModuleColumns().Where(x => x.CategoryName == "SVCTask" && x.IsDisplay == true).OrderBy(x => x.FieldSequence).ToList();
            }
            else
            {
                moduleColumns = moduleColumnManager.GetModuleColumns().Where(x => x.CategoryName == "TSKTask" && x.IsDisplay == true).OrderBy(x => x.FieldSequence).ToList();
            }
        }

        public void GenerateColumns()
        {

            foreach (ModuleColumn moduleColumn in moduleColumns)
            {
                string fieldColumn = Convert.ToString(moduleColumn.FieldName);
                Boolean isDisplay = Convert.ToBoolean(moduleColumn.IsDisplay);
                GridViewDataTextColumn colId = null;
                GridViewDataColumn grdViewDataColumn = null;
                //GridViewDataTextColumn grdViewDataTextColumn = null;
                GridViewDataSpinEditColumn grdViewDataSpinEditColumn = null;
                GridViewDataComboBoxColumn grdViewDataComboBoxColumn = null;
                GridViewDataDateColumn grdViewDataDateColumn = null;
                HorizontalAlign alignment = HorizontalAlign.Center;
                if (!string.IsNullOrWhiteSpace(moduleColumn.TextAlignment))
                    alignment = (HorizontalAlign)Enum.Parse(typeof(HorizontalAlign), moduleColumn.TextAlignment);
                if (moduleTaskList != null && (moduleTaskList.Columns.Contains(fieldColumn)))
                {
                    Type dataType = typeof(string);
                    if (moduleTaskList.Columns.Contains(fieldColumn))
                        dataType = moduleTaskList.Columns[fieldColumn].DataType;

                    if (fieldColumn.ToLower() == DatabaseObjects.Columns.IsMilestone.ToLower())
                    {
                        grdViewDataColumn = new GridViewDataColumn();
                        grdViewDataColumn.Caption = fieldColumn;
                        grdViewDataColumn.FieldName = fieldColumn;
                        grdViewDataColumn.HeaderStyle.Font.Bold = true;
                        if (!isDisplay)
                            grdViewDataColumn.Visible = false;
                        gridTaskList.Columns.Add(grdViewDataColumn);

                    }
                    else if (fieldColumn.ToLower() == DatabaseObjects.Columns.DueDate.ToLower() || fieldColumn.ToLower() == DatabaseObjects.Columns.StartDate.ToLower()
                        || fieldColumn.ToLower() == DatabaseObjects.Columns.CompletionDate.ToLower())
                    {
                        grdViewDataDateColumn = new GridViewDataDateColumn();
                        grdViewDataDateColumn.CellStyle.HorizontalAlign = alignment;
                        grdViewDataDateColumn.FieldName = Convert.ToString(moduleColumn.FieldName);
                        grdViewDataDateColumn.Caption = Convert.ToString(moduleColumn.FieldDisplayName);
                        grdViewDataDateColumn.PropertiesDateEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
                        grdViewDataDateColumn.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                        grdViewDataDateColumn.HeaderStyle.Font.Bold = true;
                        grdViewDataDateColumn.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                        //grdViewDataDateColumn.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
                        if (!isDisplay)
                            grdViewDataColumn.Visible = false;
                        gridTaskList.Columns.Add(grdViewDataDateColumn);
                    }
                    else if (fieldColumn.ToLower() == DatabaseObjects.Columns.EstimatedRemainingHours.ToLower() || fieldColumn.ToLower() == DatabaseObjects.Columns.TaskActualHours.ToLower()
                        || fieldColumn.ToLower() == DatabaseObjects.Columns.TaskEstimatedHours.ToLower() || fieldColumn.ToLower() == DatabaseObjects.Columns.PercentComplete.ToLower())
                    {
                        grdViewDataSpinEditColumn = new GridViewDataSpinEditColumn();
                        grdViewDataSpinEditColumn.FieldName = Convert.ToString(moduleColumn.FieldName);
                        grdViewDataSpinEditColumn.Caption = Convert.ToString(moduleColumn.FieldDisplayName);
                        grdViewDataSpinEditColumn.CellStyle.HorizontalAlign = alignment;
                        grdViewDataSpinEditColumn.HeaderStyle.Font.Bold = true;
                        grdViewDataSpinEditColumn.PropertiesSpinEdit.NumberType = SpinEditNumberType.Integer;
                        grdViewDataSpinEditColumn.PropertiesSpinEdit.MinValue = 0;
                        grdViewDataSpinEditColumn.PropertiesSpinEdit.MaxValue = 100;
                        if (fieldColumn == DatabaseObjects.Columns.PercentComplete)
                            grdViewDataSpinEditColumn.PropertiesSpinEdit.DisplayFormatString = "{0:N0}%";
                        if (!isDisplay)
                            grdViewDataColumn.Visible = false;
                        gridTaskList.Columns.Add(grdViewDataSpinEditColumn);
                    }
                    else if (fieldColumn.ToLower() == DatabaseObjects.Columns.Status.ToLower())
                    {
                        grdViewDataComboBoxColumn = new GridViewDataComboBoxColumn();
                        grdViewDataComboBoxColumn.Caption = Convert.ToString(moduleColumn.FieldDisplayName);
                        grdViewDataComboBoxColumn.FieldName = Convert.ToString(moduleColumn.FieldName);
                        grdViewDataComboBoxColumn.Width = new Unit("100px");
                        grdViewDataComboBoxColumn.CellStyle.CssClass = "editforminputwidth";
                        grdViewDataComboBoxColumn.HeaderStyle.Font.Bold = true;
                        grdViewDataComboBoxColumn.CellStyle.HorizontalAlign = alignment;
                        if (!isDisplay)
                            grdViewDataColumn.Visible = false;
                        gridTaskList.Columns.Add(grdViewDataComboBoxColumn);
                    }
                    else if (fieldColumn.ToLower() == DatabaseObjects.Columns.Title.ToLower())
                    {

                        colId = new GridViewDataTextColumn();
                        colId.PropertiesEdit.ClientInstanceName = Convert.ToString(moduleColumn.FieldName);
                        colId.FieldName = Convert.ToString(moduleColumn.FieldName);
                        colId.Caption = Convert.ToString(moduleColumn.FieldDisplayName);
                        colId.PropertiesTextEdit.EncodeHtml = false;
                        colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                        if (!IsReadOnly)
                        {
                            TitleDataItemTemplate obj = new TitleDataItemTemplate();
                            colId.DataItemTemplate = obj;
                            colId.EditItemTemplate = obj;
                            obj.CurrentUser = User;
                            obj.ProfileManager = UserManager;
                            obj.CurrentItem = ticketItem;
                            obj.DisableMarkAsCompleteTemplate = DisableMarkAsComplete;

                        }
                        colId.Width = new Unit(450, UnitType.Pixel);
                        if (!isDisplay)
                            grdViewDataColumn.Visible = false;
                        gridTaskList.Columns.Add(colId);

                    }
                    else if (fieldColumn.ToLower() == DatabaseObjects.Columns.RelatedTicketID.ToLower())
                    {
                        colId = new GridViewDataTextColumn();
                        colId.PropertiesEdit.ClientInstanceName = Convert.ToString(moduleColumn.FieldName);
                        colId.FieldName = Convert.ToString(moduleColumn.FieldName);
                        colId.Caption = Convert.ToString(moduleColumn.FieldDisplayName);
                        colId.PropertiesTextEdit.EncodeHtml = false;
                        TicketIDDataItemTemplate objTicketIDTemplate = new TicketIDDataItemTemplate();
                        colId.DataItemTemplate = objTicketIDTemplate;
                        colId.EditItemTemplate = objTicketIDTemplate;
                        gridTaskList.Columns.Add(colId);
                    }
                    else
                    {
                        colId = new GridViewDataTextColumn();
                        if(Convert.ToString(moduleColumn.FieldName)==DatabaseObjects.Columns.ItemOrder)
                            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                        else
                            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        colId.PropertiesTextEdit.EncodeHtml = false;
                        colId.PropertiesEdit.ClientInstanceName = Convert.ToString(moduleColumn.FieldName);
                        colId.FieldName = Convert.ToString(moduleColumn.FieldName);
                        if (ModuleName == "SVCConfig" && moduleColumn.FieldName == DatabaseObjects.Columns.ModuleNameLookup)
                            colId.FieldName = "ModuleType";

                        string dollorCol = (fieldColumn != DatabaseObjects.Columns.ModuleNameLookup && (fieldColumn.ToLower().EndsWith("lookup") || fieldColumn.ToLower().EndsWith("user"))) ? string.Format("{0}$", fieldColumn) : fieldColumn;
                        if (dollorCol.EndsWith("$") && moduleTaskList.Rows.Count > 0 && UGITUtility.IfColumnExists(moduleTaskList.Rows[0], dollorCol))
                            colId.FieldName = dollorCol;

                        colId.Caption = Convert.ToString(moduleColumn.FieldDisplayName);
                        colId.HeaderStyle.Font.Bold = true;
                        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                        //colId.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
                        colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                        if (fieldColumn == DatabaseObjects.Columns.ID || fieldColumn == DatabaseObjects.Columns.Duration || fieldColumn == DatabaseObjects.Columns.ItemOrder)
                            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;

                        if (fieldColumn == DatabaseObjects.Columns.ItemOrder)
                        {
                            colId.CellStyle.CssClass = "taskSNo";
                            colId.DataItemTemplate = new IDDataItemTemplate();
                            colId.Width = Unit.Percentage(5);
                            if (!DisableBatchEdit)
                            {
                                colId.FooterTemplate = new TitleFooter();
                            }
                        }
                        if (!isDisplay)
                            grdViewDataColumn.Visible = false;
                        gridTaskList.Columns.Add(colId);
                    }
                }
            }
        }

        #region "helpermethods"
        protected static string GetTitleHtmlData(UGITTaskManager taskManager, string title, string ID, string moduleName, bool withLink, bool includeExpandCollapseIcon = false, bool isTicketLink = false, bool linksForIcon = false)
        {
            StringBuilder data = new StringBuilder();
            string editTaskUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx");
            if (!string.IsNullOrEmpty(ID))
            {
                DataTable taskList = UGITUtility.ToDataTable<UGITTask>(taskManager.LoadByProjectID(moduleName, GlobalTicket.ticketid));
                DataRow taskRow = taskList.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.ID, ID))[0];
                int level = Convert.ToInt32(taskRow[DatabaseObjects.Columns.UGITLevel]);
                int childCount = Convert.ToInt16(taskRow[DatabaseObjects.Columns.UGITChildCount]);
                bool isMileStone = UGITUtility.StringToBoolean(taskRow[DatabaseObjects.Columns.IsMilestone]);
                string taskBehaviour = Convert.ToString(taskRow[DatabaseObjects.Columns.TaskBehaviour]);
                string taskTicketId = Convert.ToString(taskRow[DatabaseObjects.Columns.TicketId]);
                string taskReleatedModule = Convert.ToString(taskRow[DatabaseObjects.Columns.RelatedModule]);
                string relatedTicketId = Convert.ToString(taskRow[DatabaseObjects.Columns.RelatedTicketID]);
                string relatedTitle = Convert.ToString(taskRow[DatabaseObjects.Columns.Title]);
                ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                if (string.IsNullOrEmpty(taskBehaviour))
                    taskBehaviour = "Task";

                string cssChanges = string.Empty;
                if (moduleName == "PMM" || moduleName == "TSK" || taskManager.IsModuleTasks(moduleName))
                {
                    bool isCritical = UGITUtility.StringToBoolean(taskRow[DatabaseObjects.Columns.IsCritical]);
                    if (isCritical)
                        cssChanges = "Color:#CF0D0D;font-weight: bold;";
                }
                else
                    cssChanges = "color:#000;";

                if (includeExpandCollapseIcon)
                {
                    if (childCount > 0)
                    {
                        data.AppendFormat("<img style='float:left;padding-right:2px;' src='/Content/Images/minimise.gif' colexpand='true' onclick=\"event.cancelBubble=true; CloseChildren('{0}' , '{1}', this)\" />", taskRow[DatabaseObjects.Columns.ItemOrder], taskRow["ID"]);
                    }
                }
                //if(UGITTaskStatus==)

                if (taskBehaviour == Constants.TaskType.Deliverable)
                {
                    data.AppendFormat("<img style='float:left;padding-right:2px;' src='/Content/Images/document_down.png' />");
                }
                else if (taskBehaviour == Constants.TaskType.Receivable)
                {
                    data.AppendFormat("<img style='float:left;padding-right:2px;' src='/Content/Images/document_up.png' />");
                }
                else if (taskBehaviour == Constants.TaskType.Milestone)
                {
                    data.AppendFormat("<img style='float:left;padding-right:1px;' src='/Content/Images/milestone_icon.png' />");
                    cssChanges = "color:blue;font-weight:bold;";
                }
                else if (taskBehaviour == Constants.TaskType.Ticket || !string.IsNullOrWhiteSpace(taskReleatedModule))
                {
                    taskBehaviour = !string.IsNullOrWhiteSpace(taskReleatedModule) ? Constants.TaskType.Ticket : taskBehaviour;
                    if (isTicketLink)
                        relatedTitle = relatedTicketId;

                    if (!linksForIcon)
                    {
                        if (!string.IsNullOrEmpty(relatedTicketId))
                        {
                            string strtitle = string.Empty;
                            if (string.IsNullOrEmpty(relatedTitle))
                                strtitle = relatedTicketId;
                            else
                                strtitle = string.Format("{0}: {1}", relatedTicketId, relatedTitle);
                            data.AppendFormat("<a href='javascript:void(0);' onclick='window.parent.UgitOpenPopupDialog(\"{0}\", \"{1}\",\"{2}\", \"{3}\", \"{4}\",\"{5}\")'><span style='padding-top:1px;{1}'>{6}</span></a>", Ticket.GenerateTicketURL(HttpContext.Current.GetManagerContext(), relatedTicketId), string.Empty, strtitle, "90", "90", "0", relatedTitle);
                        }
                        else
                        {
                            // fix for Service Cat. & Agents, link not working for Task Type=Ticket
                            string urlParam = "ID=" + ID + "&ItemOrder=" + moduleName + "&control=taskedit&moduleName=" + moduleName + "&ticketId=" + GlobalTicket.ticketid + "&taskID=" + ID + "&taskType=" + taskBehaviour;
                            data.AppendFormat("<a href='javascript:void(0);' onclick='javascript:openTaskEditDialog(\"{0}\", \"{1}\",\"{6}\", \"{3}\", \"{4}\",\"{5}\")'><span style='padding-top:1px;{1}'>{2}</span></a>", editTaskUrl, urlParam, relatedTitle, "90", "90", "0", "Edit Ticket");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(relatedTicketId))
                        {
                            data.AppendFormat("javascript:window.parent.UgitOpenPopupDialog(\"{0}\", \"{1}\",\"{2}\", \"{3}\", \"{4}\",\"{5}\")", Ticket.GenerateTicketURL(HttpContext.Current.GetManagerContext(), relatedTicketId), string.Empty, (relatedTicketId + " : " + relatedTitle), "90", "90", "0");
                        }
                        else
                        {
                            // fix for Service Cat. & Agents, link not working for Task Type=Ticket
                            string urlParam = "ID=" + ID + "&ItemOrder=" + moduleName + "&control=taskedit&moduleName=" + moduleName + "&ticketId=" + GlobalTicket.ticketid + "&taskID=" + ID + "&taskType=" + taskBehaviour;
                            data.AppendFormat("javascript:openTaskEditDialog(\"{0}\", \"{1}\",\"{2}\", \"{3}\", \"{4}\",\"{5}\")", editTaskUrl, urlParam, (relatedTicketId + " : " + relatedTitle), "90", "90", "0");
                        }
                    }

                    return data.ToString();
                }

                if (withLink)
                {
                    if (title != null)
                    {
                        string[] arr = Convert.ToString(title).Replace('(', ' ').Replace(')', ' ').Replace("\r\n", " ").Split(new string[] { "Sprint:" }, StringSplitOptions.RemoveEmptyEntries);
                        string urlParam = "ID=" + ID + "&ItemOrder=" + moduleName + "&control=taskedit&moduleName=" + moduleName + "&ticketId=" + GlobalTicket.ticketid + "&taskID=" + ID + "&taskType=" + taskBehaviour;
                        if (arr.Length > 0)
                        {
                            string sourceURL = "/Layouts/uGovernIT/DelegateControl" + "?control=TasksList";

                            if (!linksForIcon)
                                data.AppendFormat("<a href='javascript:void(0);' onclick='openTaskEditDialog(\"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\",\" {7}\", \"{8}\")'><span style='padding-top:1px;{1}'>{0}</span></a>", arr[0].Trim(), cssChanges, editTaskUrl, urlParam, "Edit Task: " + title.Replace("\r\n", " "), "90", "90", false, sourceURL);
                            else
                                data.AppendFormat("javascript:openTaskEditDialog(\"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\",\" {7}\", \"{8}\")", arr[0].Trim(), cssChanges, editTaskUrl, urlParam, "Edit Task: " + title.Replace("\r\n", " "), "90", "90", false, sourceURL);
                        }

                        if (arr.Length > 1)
                        {
                            data.AppendFormat("<a href='javascript:void(0);' onclick='openSprintTask(\"{0}\")'><span style='padding-top:1px;{1}'>(Sprint:{0})</span></a>", arr[1].Trim(), cssChanges);
                        }
                    }

                    // data.AppendFormat("<a href='javascript:void(0);' onclick='editTask({1},{3})'><span style='padding-top:1px;{2}'>{0}</span></a>", Eval("Title"), Eval("ID"), cssChanges, Eval("ItemOrder"));
                }
                else
                    data.AppendFormat("<span style='padding-top:1px;{1}'>{0}</span>", title, cssChanges);
            }
            return data.ToString();
        }

        protected string GetMilestoneStage()
        {
            StringBuilder data = new StringBuilder();
            data.Append("this is dummy text");
            return data.ToString();
        }

        protected string GetDueDateSummaryText(DataRow currentRow)
        {
            DateTime projectEndDate = DateTime.MinValue;
            if (UGITUtility.GetSPItemValue(currentRow, DatabaseObjects.Columns.DueDate) is DateTime)
                projectEndDate = (DateTime)UGITUtility.GetSPItemValue(currentRow, DatabaseObjects.Columns.DueDate);
            if (currentRow != null)
            {
                return projectEndDate.ToString("MMM-d-yyyy");
            }
            else
            {
                return string.Empty;
            }

        }

        protected string GetStartDateSummaryText(DateTime date)
        {
            DateTime projectStartDate = DateTime.MinValue;
            if (date != null)
                projectStartDate = date;
            if (date != null)
            {
                return projectStartDate.ToString("MMM-d-yyyy");
            }
            else
            {
                return string.Empty;
            }
        }

        protected string GetPercentSummaryText(string currentTicket)
        {
            double pctComplete = UGITUtility.StringToDouble(currentTicket);
            //pctComplete *= 100;

            if (currentTicket != null)
            {
                if (pctComplete > 99.9 && pctComplete < 100)
                {
                    pctComplete = 99.9; // Don't show 100% unless all the way done!
                    return string.Format("{0} {1}% complete", ModuleName == "PMM" ? "Project is" : "Tasks are", pctComplete);
                }
                else
                {
                    pctComplete = Math.Round(pctComplete, 1, MidpointRounding.AwayFromZero); // Round to nearest 0.1
                    //return Convert.ToString(pctComplete + "%");
                    return string.Format("{0} {1}% complete", ModuleName == "PMM" ? "Project is" : "Tasks are", pctComplete);
                }
            }
            else
            {
                return string.Empty;
            }
        }

        protected string GetEstimatedHoursSummaryText(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                double hrs = UGITUtility.StringToDouble(value);
                //return hrs.ToString("#,0.##");
                return string.Format("Total: {0} hrs", hrs.ToString("#,0.##"));
            }
            else
                return string.Empty;
        }

        protected string GetActualHoursSummaryText(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                double hrs = UGITUtility.StringToDouble(value);
                return hrs.ToString("#,0.##");
            }
            else
                return string.Empty;
        }

        protected string GetEstimatedRemainingHoursSummaryText(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                double hrs = UGITUtility.StringToDouble(value);
                return hrs.ToString("#,0.##");
            }
            else
                return string.Empty;
        }

        protected string GetTaskDueDateBGColor(DataRow taskrow, string defaultColor)
        {
            string bgColor = defaultColor;

            DateTime dueDate = DateTime.MinValue;
            DateTime.TryParse(Convert.ToString(taskrow[(DatabaseObjects.Columns.DueDate)]), out dueDate);
            string status = Convert.ToString(taskrow[(DatabaseObjects.Columns.Status)]);

            UGITTaskProposalStatus proposedDateStatus = UGITTaskProposalStatus.Not_Proposed;
            if (taskrow[(DatabaseObjects.Columns.UGITProposedStatus)] != null && taskrow[(DatabaseObjects.Columns.UGITProposedStatus)] != DBNull.Value)
            {
                proposedDateStatus = (UGITTaskProposalStatus)Enum.Parse(typeof(UGITTaskProposalStatus), Convert.ToString(taskrow[(DatabaseObjects.Columns.UGITProposedStatus)]));
            }

            if (proposedDateStatus == UGITTaskProposalStatus.Pending_Date)
            {
                bgColor = "#9999FF";
            }
            else if (dueDate.Date < DateTime.Today.Date && status.ToLower() != Constants.Completed.ToLower())
            {
                bgColor = "#FF0000";
            }

            return bgColor;
        }

        protected string GetBgColor(int TaskID)
        {
            string color = "#FFFFFFFF";
            if (ShowCriticalPath)
            {
                if (CriticalPathTasks.Exists(x => x.ID == TaskID))
                {
                    color = "#FAEAED";
                }
            }

            return color;
        }
        #endregion "helpermethods"

        #region "Grid Events"

        protected void gridTaskList_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == DevExpress.Web.GridViewRowType.Data)
            {
                e.Row.Height = Unit.Pixel(35);
                int parent = Convert.ToInt32(e.GetValue("ParentTaskID"));
                if (parent > 0)
                    e.Row.CssClass += " hideElement";
            }
            if (e.RowType == DevExpress.Web.GridViewRowType.Footer)
            {
                e.Row.Cells[0].CssClass = "nprScheduleFooter_wrap";
                e.Row.Cells.RemoveAt(1);
                e.Row.Cells.RemoveAt(2);
                e.Row.Cells[0].ColumnSpan = 3;
            }
        }

        protected void gridTaskList_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            //if (e.Item.FieldName == DatabaseObjects.Columns.DueDate)
            //{
            //    if(e.Value != null)
            //       e.Text = Convert.ToDateTime(e.Value).ToString("MMM-d-yyyy");
            //}
            //if (e.Item.FieldName == DatabaseObjects.Columns.StartDate)
            //{
            //    e.Text = GetStartDateSummaryText(Convert.ToDateTime(e.Value));
            //}
            if (e.Item.FieldName == DatabaseObjects.Columns.PercentComplete)
            {
                e.Text = GetPercentSummaryText(Convert.ToString(e.Value));
            }
            if (e.Item.FieldName == DatabaseObjects.Columns.TaskActualHours)
            {
                e.Text = GetActualHoursSummaryText(Convert.ToString(e.Value));
            }
            if (e.Item.FieldName == DatabaseObjects.Columns.EstimatedHours)
            {
                e.Text = GetEstimatedHoursSummaryText(Convert.ToString(e.Value));
            }
            if (e.Item.FieldName == DatabaseObjects.Columns.EstimatedRemainingHours)
            {
                e.Text = GetEstimatedRemainingHoursSummaryText(Convert.ToString(e.Value));
            }
            e.EncodeHtml = false;
            e.Text = "<span style='font-weight: bold;'>" + e.Text + "</span>";
        }

        protected void gridTaskList_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            //bool enableTicketReopenByRequestor = true; 
            //uGITCache.GetConfigVariableValueAsBool(ConfigConstants.EnableTicketReopenByRequestor);

            // code imported from sp project
            if (e.RowType == DevExpress.Web.GridViewRowType.Data)
            {
                //DataRow currentRow = gridTaskList.GetDataRow(e.VisibleIndex); // already defined at top
                string categoryName = string.Empty;

                DataRow currentRow = gridTaskList.GetDataRow(e.VisibleIndex);
                if (currentRow == null)
                    return; // No rows in table, nothing to do!

                if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.CategoryName))
                {
                    categoryName = Convert.ToString(currentRow[DatabaseObjects.Columns.CategoryName]);
                }
                if (string.IsNullOrEmpty(categoryName))
                {
                    string ticketId = Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketId));
                    if (ticketId.Length > 0 && ticketId.IndexOf("-") > 0)
                        categoryName = ticketId.Substring(0, ticketId.IndexOf("-"));
                }

                int titleIndex = gridTaskList.Columns["Title"].Index;
                //int dueDateIndex = gridTaskList.Columns["DueDate"].Index;

                if (milestoneTaskcount == 0)
                {
                    titleIndex = titleIndex - 1;
                    //dueDateIndex -= 1;
                }


                var objRow = gridTaskList.GetRow(e.VisibleIndex);
                int level = Convert.ToInt32(gridTaskList.GetRowValues(e.VisibleIndex, DatabaseObjects.Columns.UGITLevel));
                if (level != 0 && e.Row.Cells.Count > 1)
                {
                    if (e.Row.Cells.Count > 11)
                    {
                        e.Row.Cells[titleIndex].Style.Add("padding-left", string.Format("{0}px", 25 * level));
                    }
                }

                if (e.Row.Cells.Count > 11 && currentRow != null)
                {
                    //if (e.Row.Cells.Count > 12)
                    //    dueDateIndex = gridTaskList.Columns["DueDate"].Index - 1;
                    //else if (e.Row.Cells.Count > 11)
                    //    dueDateIndex = gridTaskList.Columns["DueDate"].Index - 2;

                    e.Row.Cells[titleIndex].CssClass = "CellProperty";
                    string bgcolor = GetTaskDueDateBGColor(currentRow, "");
                    //e.Row.Cells[dueDateIndex].Style.Add("color", bgcolor);
                    //e.Row.Cells[dueDateIndex].Style.Add("font-weight", "bold");
                }

                if (currentRow != null && e.Row.Cells.Count > 1)
                {

                    e.Row.Attributes.Add("level", Convert.ToString(currentRow[DatabaseObjects.Columns.UGITLevel]));
                    e.Row.Attributes.Add("itemorder", Convert.ToString(currentRow[DatabaseObjects.Columns.ItemOrder]));
                    e.Row.Attributes.Add("childcount", Convert.ToString(currentRow[DatabaseObjects.Columns.UGITChildCount]));
                    e.Row.Attributes.Add("parenttask", Convert.ToString(currentRow[DatabaseObjects.Columns.ParentTaskID]));
                    e.Row.Attributes.Add("parenttaskID", Convert.ToString(currentRow[DatabaseObjects.Columns.ParentTaskID]));
                    e.Row.Attributes.Add("task", Convert.ToString(currentRow[DatabaseObjects.Columns.ID]));
                    e.Row.Attributes.Add("behaviour", Convert.ToString(currentRow[DatabaseObjects.Columns.TaskBehaviour]));
                    e.Row.Attributes.Add("onmouseover", string.Format("showTaskActions(this,{0})", Convert.ToString(currentRow[DatabaseObjects.Columns.ID])));
                    e.Row.Attributes.Add("onmouseout", string.Format("hideTaskActions(this,{0})", Convert.ToString(currentRow[DatabaseObjects.Columns.ID])));
                    e.Row.Attributes.Add("onclick", string.Format("showTaskActionButtons(this)"));
                    e.Row.Attributes.Add("status", Convert.ToString(currentRow[DatabaseObjects.Columns.Status]));

                    e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, GetBgColor(Convert.ToInt32(currentRow[DatabaseObjects.Columns.ID])));
                    int parentTaskID = UGITUtility.StringToInt(currentRow[DatabaseObjects.Columns.ParentTaskID]);
                    int childCount = UGITUtility.StringToInt(currentRow[DatabaseObjects.Columns.UGITChildCount]);
                    int taskID = UGITUtility.StringToInt(currentRow[DatabaseObjects.Columns.ID]);

                }

                GridViewTableDataCell editCell = null;
                GridViewDataColumn cellColumn = null;
                int cellIndex = 0;
                foreach (object cell in e.Row.Cells)
                {
                    if (cell is GridViewTableDataCell)
                    {
                        if (UGITTaskManager.IsModuleTasks(ModuleName))
                        {
                            editCell = (GridViewTableDataCell)cell;
                            cellColumn = editCell.Column as GridViewDataColumn;
                            cellIndex = cellColumn.VisibleIndex;

                            string fieldName = cellColumn.FieldName;
                            if (fieldName == DatabaseObjects.Columns.DueDate)
                            {
                                e.Row.Cells[cellIndex].Text = GetDueDateSummaryText(currentRow);
                            }
                            if (fieldName.ToLower() == DatabaseObjects.Columns.IsMilestone.ToLower())
                            {
                                e.Row.Cells[cellIndex].Text = GetMilestoneStage();
                            }
                            if (fieldName == DatabaseObjects.Columns.StartDate)
                            {
                                //e.Row.Cells[cellIndex].Text = GetStartDateSummaryText();
                            }
                            if (fieldName == DatabaseObjects.Columns.Duration)
                            {
                                string duration = Convert.ToString(currentRow[DatabaseObjects.Columns.Duration]);
                                string contribution = Convert.ToString(currentRow[DatabaseObjects.Columns.UGITContribution]);
                                if (!string.IsNullOrEmpty(duration))
                                {
                                    e.Row.Cells[cellIndex].Text = (Convert.ToDouble(duration) == 1 ? duration + " Day" : duration + " Days") + " (" + contribution + "%)";
                                }
                            }
                        }
                    }

                }


            }

        }

        protected void gridTaskList_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            List<UGITTask> tasksList = UGITTaskManager.LoadByProjectID(ModuleName, TicketPublicId);

            tasksList = UGITTaskManager.MapRelationalObjects(tasksList);

            foreach (var args in e.UpdateValues)
            {
                UpdateItem(args.Keys, args.NewValues, tasksList);
            }

            if (e.InsertValues.Count > 0)
            {
                string batchAddItemsData = Convert.ToString(gridExtraData.Get("batchAddItemsData"));
                List<BatchGridExtraData> extraData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BatchGridExtraData>>(batchAddItemsData);
                for (int i = 0; i < e.InsertValues.Count; i++)
                {
                    var item = e.InsertValues[i];
                    BatchGridExtraData extraDataItem = null;
                    if (extraData.Count > i)
                        extraDataItem = extraData[i];
                    UpdateItem(null, item.NewValues, tasksList, extraDataItem);
                }
            }

            if (TicketPublicId != string.Empty)
                ticketItem = TicketManager.GetTicketTableBasedOnTicketId(ModuleName, TicketPublicId).Select()[0]; //SPListHelper.GetSPListItem(moduleTicketList, TicketID);
            //Calculates project's startdate, enddate, pctcomplete, duration,  DaysToComplete, nextactivity and nextmilestone.
            UGITTaskManager.CalculateProjectStartEndDate(ModuleName, tasksList, ticketItem);
            Ticket ticketRequest = new Ticket(ApplicationContext, ModuleName);
            if (ticketRequest != null)
                ticketRequest.CommitChanges(ticketItem, string.Empty);
            UGITTaskManager.ReOrderTasks(ref tasksList);
            UGITTaskManager.SaveTasks(ref tasksList, ModuleName, TicketPublicId);
            //TaskCache.ReloadProjectTasks(ModuleName, TicketPublicId);  // cache is not implemented so not using right now
            BindProjectTasks();

            e.Handled = true;
            gridTaskList.CancelEdit();
        }

        protected void gridTaskList_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (DisableBatchEdit)
            {
                e.Editor.ReadOnly = true;
            }
            e.Column.Width = Unit.Percentage(100);
            if (e.Column.FieldName == DatabaseObjects.Columns.DueDate || e.Column.FieldName == DatabaseObjects.Columns.StartDate)
            {

                ASPxDateEdit dateEdit = (ASPxDateEdit)e.Editor;
                if (e.Editor.Value == null)
                {
                    e.Editor.Value = new DateTime(1900, 1, 1);
                }
            }
        }
        //Change cell color as per status.
        protected void gridTaskList_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

            if (e.DataColumn.FieldName == DatabaseObjects.Columns.Status)
            {
                var statusVal = Convert.ToString(e.CellValue);

                if (statusVal.Equals("Waiting") || statusVal.Contains("Initiated"))
                    e.Cell.ForeColor = Color.Gray;

                else if (statusVal.Equals("Completed") || statusVal.Equals("Close"))
                    e.Cell.ForeColor = Color.Green;

                else
                    e.Cell.ForeColor = Color.Orange;
            }

            if (e.DataColumn.FieldName == DatabaseObjects.Columns.StageStep)
            {
                var stageStep = Convert.ToString(e.CellValue);
                int stagestepint = 0;
                int.TryParse(stageStep, out stagestepint);
                List<LifeCycle> spListModuleStep = lifeCycleHelper.LoadLifeCycleByModule("SVC");
                List<LifeCycleStage> rows = spListModuleStep[0].Stages.Where(x => x.ModuleNameLookup.Equals("SVC", StringComparison.CurrentCultureIgnoreCase))
                    .OrderBy(x => x.StageStep).ToList();
                var lifeCycleStage = rows.Where(x => x.StageStep == Convert.ToInt32(stagestepint)).ToList().SingleOrDefault();
                if (lifeCycleStage != null)
                {
                    if (lifeCycleStage.StageStep > 0)
                    {
                        e.Cell.Text = lifeCycleStage.StageTitle;
                    }
                    else
                    {
                        e.Cell.Text = "None";
                    }


                }
                else
                {
                    e.Cell.Text = "None";
                }



            }


        }


        #endregion "Grid Events"

        protected void UpdateItem(OrderedDictionary keys, OrderedDictionary newValues, List<UGITTask> tasksList, BatchGridExtraData extraData = null)
        {
            int taskId = 0;
            UGITTask task = null;
            bool isNew = false;
            if (keys == null)
            {
                isNew = true;
                task = new UGITTask(ModuleName);
                //SPFieldLookupValue projectLookup = new SPFieldLookupValue();
                //projectLookup.LookupId = TicketID;
                //task.ProjectLookup = projectLookup;
                //task.ParentTask = null;
                task.TicketId = TicketPublicId;
                task.ParentTask = null;
            }
            else
            {
                taskId = Convert.ToInt32(keys["ID"]);
                task = tasksList.FirstOrDefault(x => x.ID == taskId);
            }

            task.Changes = true;
            List<string> userMultiLookup = new List<string>();
            string assignToPct = Convert.ToString(newValues[DatabaseObjects.Columns.AssignedTo]);
            string strAssignToPct = string.Empty;

            List<UserProfile> profiles = UserManager.GetUsersProfile(); // uGITCache.UserProfileCache.LoadCacheUsers(SPContext.Current.Web);
            string[] assignedUsers = UGITUtility.SplitString(assignToPct, ";", StringSplitOptions.RemoveEmptyEntries);
            if (profiles != null && assignedUsers.Length > 0)
            {
                foreach (string user in assignedUsers)
                {
                    string assignedUser = user.Trim();
                    string assignedPcntg = string.Empty;
                    if (user.Contains("["))
                    {
                        string[] userComponents = UGITUtility.SplitString(assignedUser, "[", StringSplitOptions.RemoveEmptyEntries);
                        assignedUser = userComponents[0].Trim();
                        if (userComponents.Length > 1)
                            assignedPcntg = UGITUtility.SplitString(userComponents[1].Trim(), "%", StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    }

                    UserProfile userAssigned = profiles.FirstOrDefault(x => (x.UserName.ToLower().EndsWith("\\" + assignedUser.ToLower())) || (x.UserName.ToLower().EndsWith("|" + assignedUser.ToLower())) || x.Email.ToLower() == assignedUser.ToLower() || x.Name.ToLower() == assignedUser.ToLower());
                    if (userAssigned != null)
                    {
                        //SPFieldUserValue userLookup = new SPFieldUserValue();
                        //userLookup.LookupId = userAssigned.ID;
                        //userMultiLookup.Add(userLookup);
                        string strPercentage = assignedPcntg;
                        if (string.IsNullOrEmpty(strPercentage))
                            strPercentage = "100";
                        if (!string.IsNullOrEmpty(strAssignToPct))
                            strAssignToPct += Constants.Separator;
                        strAssignToPct += userAssigned.UserName + Constants.Separator1 + strPercentage;
                    }
                }
            }

            //List<string> existingUsers = new List<string>();
            string[] existingUsers = null;
            if (task.AssignedTo != null)
                existingUsers = UGITUtility.SplitString(task.AssignedTo, Constants.Separator6); //new List<string>(UGITUtility.SplitString(task.AssignedTo, Constants.Separator6));


            task.AssignedTo = userMultiLookup.Serialize();
            task.AssignToPct = strAssignToPct;

            task.Title = Convert.ToString(newValues["Title"]);
            task.PercentComplete = Convert.ToDouble(newValues["PercentComplete"]);
            task.Status = Convert.ToString(newValues["Status"]);
            task.EstimatedHours = Convert.ToDouble(newValues["TaskEstimatedHours"]);
            task.ActualHours = Convert.ToDouble(newValues["TaskActualHours"]);
            task.StartDate = Convert.ToDateTime(newValues["StartDate"]);
            task.DueDate = Convert.ToDateTime(newValues["DueDate"]);
            task.EstimatedRemainingHours = Convert.ToDouble(newValues["EstimatedRemainingHours"]);
            task.Predecessors = Convert.ToString(newValues["PredecessorsByOrder"]); //new SPFieldLookupValueCollection();
            //string predecessor = Convert.ToString(newValues["PredecessorsByOrder"]);
            //string[] predArray = predecessor.Split(',', ';');
            //predArray.ToList().ForEach(x =>
            //{
            //    if (!string.IsNullOrEmpty(x))
            //    {
            //        int predItemOrder = Convert.ToInt32(x);
            //        var tk = tasksList.FirstOrDefault(t => t.ItemOrder == predItemOrder);
            //        task.Predecessors.Add(new SPFieldLookupValue { LookupId = tk.ID });
            //    }
            //});

            if (task.Status == "Completed")
            {
                //completed on..
                task.CompletionDate = DateTime.Now;


            }

            if (isNew)
            {
                UGITTask previousTask = null;
                if (extraData.PreviousTaskID > 0)
                    previousTask = tasksList.FirstOrDefault(x => x.ID == extraData.PreviousTaskID);
                UGITTask nextTask = null;
                if (extraData.NextTaskID > 0)
                    nextTask = tasksList.FirstOrDefault(x => x.ID == extraData.NextTaskID);

                UGITTask refTask = null;
                if (previousTask != null)
                    refTask = previousTask;
                if (nextTask != null)
                    refTask = nextTask;
                int order = 0;
                if (refTask != null)
                {
                    task.ParentTaskID = refTask.ParentTaskID;
                    order = refTask.ItemOrder;
                    if (order > 0)
                        task.ItemOrder = order;
                }
                else
                    task.ItemOrder = tasksList.Count;

                UGITTaskManager.SaveTask(ref task, ModuleName, TicketPublicId);
                if (order > 0)
                {
                    int index = 0;
                    if (nextTask != null)
                        index = tasksList.IndexOf(nextTask);
                    else if (previousTask != null)
                        index = tasksList.IndexOf(previousTask) + 1;

                    tasksList.Insert(index, task);
                }
                else
                {
                    tasksList.Add(task);
                }

                tasksList = UGITTaskManager.MapRelationalObjects(tasksList);
            }

            UGITTaskManager.PropagateTaskEffect(ref tasksList, task);

            // Update resource utilization pct of resource only if Task had assignees before OR has assignees now
            if ((existingUsers.Count() > 0 || userMultiLookup.Count > 0) && (ModuleName == "PMM" || ModuleName == "TSK" || ModuleName == "NPR" || UGITTaskManager.IsModuleTasks(ModuleName)))
            {
                bool autoCreateRMMProjectAllocation = ConfigurationVariableManager.GetValueAsBool(ConfigConstants.AutoCreateRMMProjectAllocation);
                if (autoCreateRMMProjectAllocation)
                {
                    string webUrl = HttpContext.Current.Request.Url.ToString();
                    List<string> users = userMultiLookup;//.Select(x => x.LookupId).ToList();
                    users.AddRange(existingUsers.ToArray<string>());

                    ThreadStart threadStartMethodUpdateProjectPlannedAllocation = delegate () { ResourceAllocationManager.UpdateProjectPlannedAllocation(webUrl, tasksList, users, ModuleName, TicketPublicId, true); };
                    Thread sThreadUpdateProjectPlannedAllocation = new Thread(threadStartMethodUpdateProjectPlannedAllocation);
                    sThreadUpdateProjectPlannedAllocation.Start();
                }
            }
        }

        private void BindProjectTasks()
        {
            DateTime projectStartDate = DateTime.MinValue;
            DateTime projectEndDate = DateTime.MinValue;
            if (UGITUtility.GetSPItemValue(ticketItem, DatabaseObjects.Columns.TicketActualStartDate) is DateTime)
                projectStartDate = (DateTime)UGITUtility.GetSPItemValue(ticketItem, DatabaseObjects.Columns.TicketActualStartDate);

            if (UGITUtility.GetSPItemValue(ticketItem, DatabaseObjects.Columns.TicketActualCompletionDate) is DateTime)
                projectEndDate = (DateTime)UGITUtility.GetSPItemValue(ticketItem, DatabaseObjects.Columns.TicketActualCompletionDate);

            double projectDuration = UGITUtility.StringToDouble(UGITUtility.GetSPItemValue(ticketItem, DatabaseObjects.Columns.TicketDuration));
            double pctComplete = 100 * UGITUtility.StringToDouble(UGITUtility.GetSPItemValue(ticketItem, DatabaseObjects.Columns.TicketPctComplete));

            taskTable = UGITTaskManager.LoadTasksTable(ModuleName, true, TicketPublicId);

            //int milestoneTaskcount = 0;
            if (taskTable != null && taskTable.Rows.Count > 0 && taskTable.Columns.Contains(DatabaseObjects.Columns.IsMilestone))
            {
                milestoneTaskcount = (int)taskTable.Compute(string.Format("count({0})", DatabaseObjects.Columns.IsMilestone), string.Format("{0} = true", DatabaseObjects.Columns.IsMilestone));
            }

            if (milestoneTaskcount == 0)
            {
                //gridTaskList.Columns[0].Visible = false;
            }

            //grid is crashing when something is added/deleted/moved in grid.
            //Didn't fine perfect solution for it now
            if (IsPostBack && taskTable != null && taskTable.Rows.Count > 0)
            {
                gridTaskList.DataSource = taskTable.Clone();
                gridTaskList.DataBind();
            }
            //new lines for grid...
            gridTaskList.DataSource = taskTable;
            gridTaskList.DataBind();

            //isBindTaskDone = true;
        }

        protected void btncriticalpath_Click(object sender, ImageClickEventArgs e)
        {
            if (btncriticalpath.CommandArgument == "1")
            {
                var tasks = UGITTaskManager.LoadByProjectID(ModuleName, TicketPublicId);
                tasks = UGITTaskManager.MapRelationalObjects(tasks);
                CriticalPathTasks = GetCriticalPathTask(tasks);

                ShowCriticalPath = true;
                btncriticalpath.ImageUrl = "/Content/Images/critical.png";
                btncriticalpath.CommandArgument = "0";
                btncriticalpath.ToolTip = "Hide Critical Path";
                iscritical.Value = "1";
            }
            else if (btncriticalpath.CommandArgument == "0")
            {
                ShowCriticalPath = false;
                btncriticalpath.ImageUrl = "/Content/Images/critical_inactive.png";
                btncriticalpath.CommandArgument = "1";
                btncriticalpath.ToolTip = "Show Critical Path";
                iscritical.Value = "0";
            }
        }

        public List<UGITTask> GetCriticalPathTask(List<UGITTask> tasks)
        {
            #region Old Calculation
            //List<UGITTask> criticalPathTasks = new List<UGITTask>();
            //var firsttask = tasks.FirstOrDefault();
            //criticalPathTasks.Add(firsttask);
            //var lasttask = tasks.LastOrDefault();
            //criticalPathTasks.AddRange(tasks.Where(x => x.ID != lasttask.ID && x.ID != firsttask.ID).ToList());
            //criticalPathTasks.Add(lasttask);

            //var notvalidCriticalTask = new List<UGITTask>();
            //foreach (var ctask in tasks)
            //{
            //    if (ctask.ParentTaskID == 0)
            //    {
            //        if (ctask.ChildCount > 0)
            //        {
            //            notvalidCriticalTask.Add(ctask);
            //            continue;
            //        }
            //    }

            //    if (ctask.PredecessorTasks != null)
            //    {
            //        var predTasks = ctask.PredecessorTasks;
            //        foreach (var predtask in predTasks)
            //        {
            //            string nextworkingDatenTime = uHelper.GetNextWorkingDateAndTime(ctask.DueDate, SPContext.Current.Web);
            //            string[] nextworkingStartNEndTime = uHelper.SplitString(nextworkingDatenTime, Constants.Separator);
            //            DateTime nextStartTime = Convert.ToDateTime(nextworkingStartNEndTime[0]);

            //            TimeSpan tsdiff = predtask.StartDate.Subtract(nextStartTime);
            //            if (tsdiff.Days > 0)
            //            {
            //                notvalidCriticalTask.Add(ctask);
            //            }
            //        }
            //    }
            //}

            //criticalPathTasks = criticalPathTasks.Except(notvalidCriticalTask).ToList();

            //var tasknopredecessors = tasks.Where(x => (x.PredecessorTasks == null || x.PredecessorTasks.Count == 0) &&
            //                                   (x.SuccessorTasks == null || x.SuccessorTasks.Count == 0) &&
            //                                   x.ID != firsttask.ID && x.ID != lasttask.ID).ToList();

            //criticalPathTasks = criticalPathTasks.Except(tasknopredecessors).ToList();
            #endregion

            #region Critical Path Task calculation based on estimated hours of each task

            List<UGITTask> roottasks = tasks.OrderBy(x => x.StartDate).Where(x => (x.PredecessorTasks == null || x.PredecessorTasks.Count == 0) && (x.ParentTaskID == 0)).ToList();
            List<Tuple<List<UGITTask>>> allPath = new List<Tuple<List<UGITTask>>>();
            foreach (UGITTask roottask in roottasks)
            {
                Tuple<List<UGITTask>> path = new Tuple<List<UGITTask>>(new List<UGITTask>());
                path.Item1.Add(roottask);
                allPath.Add(path);
                if (roottask.SuccessorTasks != null && roottask.SuccessorTasks.Count > 0)
                {
                    CalculateCriticalPath(allPath, path, roottask.SuccessorTasks);
                }
            }

            var data = tasks.Sum(x => x.EstimatedHours) > 0 ? (from a in allPath select new { tasks = a.Item1, sum = a.Item1.Sum(x => x.EstimatedHours) }) : (from a in allPath select new { tasks = a.Item1, sum = a.Item1.Sum(x => x.Duration) });
            List<UGITTask> criticalPathTasks = new List<UGITTask>();
            if (data != null)
            {
                var criticalPath = data.OrderByDescending(x => x.sum).FirstOrDefault();
                if (criticalPath != null)
                    criticalPathTasks = criticalPath.tasks;
            }
            #endregion

            return criticalPathTasks;
        }

        void CalculateCriticalPath(List<Tuple<List<UGITTask>>> allPath, Tuple<List<UGITTask>> path, List<UGITTask> successorTasks)
        {
            if (successorTasks != null)
            {
                List<UGITTask> pathTasks = new List<UGITTask>();
                pathTasks.AddRange(path.Item1);
                for (int i = 0; i < successorTasks.Count; i++)
                {
                    UGITTask task = successorTasks[i];
                    Tuple<List<UGITTask>> sPath = null;
                    if (i == 0)
                    {
                        sPath = path;
                        sPath.Item1.Add(task);
                    }
                    else
                    {
                        sPath = new Tuple<List<UGITTask>>(new List<UGITTask>());
                        sPath.Item1.AddRange(pathTasks);
                        sPath.Item1.Add(task);
                        allPath.Add(sPath);
                    }
                    CalculateCriticalPath(allPath, sPath, task.SuccessorTasks);
                }
            }
        }

        protected void btMarkComplete_Click(object sender, ImageClickEventArgs e)
        {
            var ids = gridTaskList.GetSelectedFieldValues("ID");
            if (ids.Count > 0)
            {
                var taskID = Convert.ToInt32(ids[0]);
                MarkTaskAsComplete(taskID);
            }
            BindProjectTasks();
        }

        protected void lnkDecIndent_Click(object sender, ImageClickEventArgs e)
        {
            List<object> fieldValues = gridTaskList.GetSelectedFieldValues(DatabaseObjects.Columns.ID);
            if (fieldValues == null || fieldValues.Count == 0)
                return;
            int taskID = Convert.ToInt32(fieldValues[0]);
            if (taskID > 0)
            {
                DecIndent(taskID);
            }
            BindProjectTasks();
        }

        protected void lnkIncIndent_Click(object sender, ImageClickEventArgs e)
        {
            List<object> fieldValues = gridTaskList.GetSelectedFieldValues(DatabaseObjects.Columns.ID);
            if (fieldValues == null || fieldValues.Count == 0)
                return;
            int taskID = Convert.ToInt32(fieldValues[0]);
            if (taskID > 0)
            {
                IncIndent(taskID);
            }
            BindProjectTasks();
        }

        private void IncIndent(int tskTaskID)
        {
            List<UGITTask> tasks = UGITTaskManager.LoadByProjectID(ModuleName, TicketPublicId);
            tasks = UGITTaskManager.MapRelationalObjects(tasks);

            UGITTask childTask = tasks.FirstOrDefault(x => x.ID == tskTaskID);


            //remove predecessors of of task which trigger indent
            childTask.Predecessors = null;

            int childIndex = tasks.IndexOf(childTask);
            UGITTask parentTask = null;
            for (int i = childIndex - 1; i >= 0; i--)
            {
                // return because not child task exits and parent  id exits task cannot be further indented
                if (childTask.ParentTaskID == tasks[i].ID)
                    return;
                if (tasks[i].Level == childTask.Level)
                {
                    parentTask = tasks[i];
                    break;
                }
                else if (tasks[i].Level < childTask.Level)
                {
                    parentTask = null;
                    break;
                }
            }

            long parentTaskID = 0;
            if (parentTask != null)
            {
                parentTaskID = parentTask.ID;

                //if (parentTask.SprintLookup != null)
                //{
                //    string strscript = string.Format("<script>openTaskDialog(\"{0}\");</script>", parentTask.Title);

                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Task", strscript);
                //    return;
                //}
            }

            UGITTaskManager.AddChildTask(parentTaskID, tskTaskID, ref tasks);
            UGITTaskManager.ReOrderTasks(ref tasks);
            UGITTaskManager.PropagateTaskEffect(ref tasks, childTask);

            childTask = tasks.FirstOrDefault(x => x.ID == tskTaskID);
            //Not task is move from root then dettach it from milestone.
            if (childTask.ParentTaskID > 0)
            {
                childTask.IsMileStone = false;
                childTask.StageStep = 0;
            }
            UGITTaskManager.SaveTasks(ref tasks, ModuleName, TicketPublicId);

            //Calculate project related values
            UGITTaskManager.CalculateProjectStartEndDate(ModuleName, tasks, ticketItem);
            //ticketItem.UpdateOverwriteVersion();

            //if (ModuleName != "NPR")
            //TaskCache.ReloadProjectTasks(ModuleName, TicketPublicId);
        }

        private void DecIndent(int tskTaskID)
        {
            List<UGITTask> tasks = UGITTaskManager.LoadByProjectID(ModuleName, TicketPublicId);
            tasks = UGITTaskManager.MapRelationalObjects(tasks);

            UGITTask childTask = tasks.FirstOrDefault(x => x.ID == tskTaskID);


            if (childTask.Level > 0)
            {
                //remove predecessors of of task which trigger indent
                childTask.Predecessors = null;

                int childIndex = tasks.IndexOf(childTask);
                UGITTask parentTask = null;
                for (int i = childIndex - 1; i >= 0; i--)
                {
                    if (tasks[i].Level == childTask.Level - 1)
                    {
                        parentTask = tasks[i];
                        break;
                    }
                }

                if (parentTask != null)
                {
                    UGITTaskManager.RemoveChildTask(parentTask.ID, tskTaskID, ref tasks);
                    UGITTaskManager.ReOrderTasks(ref tasks);
                    UGITTaskManager.PropagateTaskEffect(ref tasks, childTask);

                    childTask = tasks.FirstOrDefault(x => x.ID == tskTaskID);
                    //Not task is move from root then dettach it from milestone.
                    if (childTask.ParentTaskID > 0)
                    {
                        childTask.IsMileStone = false;
                        childTask.StageStep = 0;
                    }

                    UGITTaskManager.SaveTasks(ref tasks, ModuleName, TicketPublicId);

                    //Calculate project related values
                    UGITTaskManager.CalculateProjectStartEndDate(ModuleName, tasks, ticketItem);
                    //ticketItem.UpdateOverwriteVersion();

                    //TaskCache.ReloadProjectTasks(ModuleName, TicketPublicId);
                }
            }
        }

        protected void lnkDelete_Click(object sender, ImageClickEventArgs e)
        {
            List<object> fieldValues = gridTaskList.GetSelectedFieldValues(DatabaseObjects.Columns.ID);
            if (fieldValues == null || fieldValues.Count == 0)
                return;
            int taskID = Convert.ToInt32(fieldValues[0]);
            if (taskID > 0)
            {
                List<UGITTask> tasks = UGITTaskManager.LoadByProjectID(ModuleName, TicketPublicId);
                tasks = UGITTaskManager.MapRelationalObjects(tasks);

                UGITTask task = tasks.FirstOrDefault(x => x.ID == taskID);
                if (task != null)
                {
                    UGITTaskManager.DeleteTask(ModuleName, ref tasks, task);
                    //Log.AuditTrail(string.Format("User {0} DELETED task [{1}] for project {2}", SPContext.Current.Web.CurrentUser.Name, task.Title, TicketPublicId));

                    List<string> existingUsers = new List<string>();
                    if (task.AssignedTo != null)
                        existingUsers = new List<string>(UGITUtility.SplitString(task.AssignedTo, Constants.Separator6)); //task.AssignedTo.Select(x => x.LookupId).ToList();
                    //RMMSummaryHelper.DeleteAllocationsByTask(SPContext.Current.Web, tasks, existingUsers, ModuleName, TicketPublicId);

                    //UGITTaskHelper.ReloadProjectTasks(ModuleName, TicketPublicId);
                    UGITTaskManager.CalculateProjectStartEndDate(ModuleName, tasks, ticketItem);
                    //ticketItem.UpdateOverwriteVersion();
                }
                BindProjectTasks();
            }
        }

        protected void btnDuplicate_Click(object sender, EventArgs e)
        {
            List<object> selectedValues = new List<object>();

            bool copychild = false;

            object[] a = (object[])hdntask.Get("taskid");
            if (a.Count() > 0)
            {
                int taskid = Convert.ToInt32(a[0]);
                CopyTask(taskid, TicketPublicId, copychild, ModuleName);
            }
        }

        protected void btnAutoAdjustSchedules_Click(object sender, ImageClickEventArgs e)
        {
            List<UGITTask> tasks = UGITTaskManager.LoadByProjectID(ModuleName, TicketPublicId);
            tasks = UGITTaskManager.MapRelationalObjects(tasks);
            UGITTaskManager.AutoAdjustSchedules(ref tasks, ModuleName, TicketPublicId);

            DataRow project = Ticket.GetCurrentTicket(HttpContext.Current.GetManagerContext(), ModuleName, TicketPublicId);
            UGITTaskManager.CalculateProjectStartEndDate(ModuleName, tasks, project);
            Ticket ticket = new Ticket(HttpContext.Current.GetManagerContext(), ModuleName);
            ticket.CommitChanges(project, string.Empty);
            //TaskCache.ReloadProjectTasks(ModuleName, TicketPublicId);
        }

        protected void btnTaskDelete_Click(object sender, ImageClickEventArgs e)
        {
            List<UGITTask> projectTasks = UGITTaskManager.LoadByProjectID(ModuleName, TicketPublicId);
            if (projectTasks != null && projectTasks.Count > 0)
            {
                UGITTaskManager.DeleteTasks(ModuleName, projectTasks);
                //Log.AuditTrail(string.Format("User {0} DELETED ALL tasks for project {1}", SPContext.Current.Web.CurrentUser.Name, TicketPublicId));

                projectTasks = UGITTaskManager.LoadByProjectID(ModuleName, TicketID);
                UGITTaskManager.CalculateProjectStartEndDate(ModuleName, projectTasks, ticketItem);
                //TaskCache.ReloadProjectTasks(ModuleName, TicketPublicId);
            }
        }

        protected bool isRequestTaskCompleted(UGITTask task)
        {
            bool isRequestTaskCompleted = true;
            string ticketId = task.TicketId;
            List<UGITTask> allTaskOnSameStage = UGITTaskManager.LoadByProjectID(task.TicketId).Where(x => x.StageStep == task.StageStep).ToList();
            if (allTaskOnSameStage.Count > 0)
            {
                List<UGITTask> incompleteTaskOnSameStage = allTaskOnSameStage.Where(x => x.Status != "Completed" && x.ID != task.ID).ToList();
                if (incompleteTaskOnSameStage.Count > 0)
                {
                    isRequestTaskCompleted = false;
                }
                else
                {

                }
            }

            return isRequestTaskCompleted;

        }


        private void MarkTaskAsComplete(int taskID)
        {
            bool RunApproveService = false;
            //bool taskOnStagedifferentfromTicket = false;
            List<UGITTask> tasks = UGITTaskManager.LoadByProjectID(ModuleName, TicketPublicId);
            tasks = UGITTaskManager.MapRelationalObjects(tasks);

            UGITTask task = tasks.FirstOrDefault(x => x.ID == taskID);
            string oldStatus = task.Status;
            double oldPctComplete = task.PercentComplete;

            if (oldStatus != Constants.Completed || oldPctComplete != 100)
            {

                if (ModuleName == "SVC")
                {
                    ////if (ddlStatus.SelectedValue == "Completed")
                    ////{
                    //    RunApproveService = true;
                    //    RunApproveService = isRequestTaskCompleted(task);
                    //    DataRow dr = Ticket.GetCurrentTicket(_context, ModuleName, TicketPublicId);
                    //    int ticketStage = (int)dr[DatabaseObjects.Columns.StageStep];
                    //    if (ticketStage != task.StageStep)
                    //    {
                    //        taskOnStagedifferentfromTicket = true;
                    //    }

                    ////}


                    //}

                    //if (!taskOnStagedifferentfromTicket)
                    //{

                    task.Status = Constants.Completed;
                    task.PercentComplete = 100;
                    //completed on..
                    task.CompletionDate = DateTime.Now;
                    task.Changes = true;
                    if (keepActualHourMandatory)
                    {
                        //task.ActualHours = UGITUtility.StringToDouble(txtActualHours.Text.Trim());
                    }

                    UGITTaskManager.PropagateTaskStatusEffect(ref tasks, task);
                    UGITTaskManager.SaveTasks(ref tasks, ModuleName, TicketPublicId);

                    //Calculate project related values
                    UGITTaskManager.CalculateProjectStartEndDate(ModuleName, tasks, ticketItem);
                    //ticketItem.UpdateOverwriteVersion();

                    #region Update History
                    string historyDesc = string.Empty;
                    if (oldPctComplete != task.PercentComplete)
                    {
                        historyDesc = string.Format("Task [{0}]:", task.Title);
                        historyDesc += string.Format(" {0}% => {1}%", oldPctComplete, task.PercentComplete);
                    }
                    if (oldStatus != task.Status)
                    {
                        if (historyDesc == string.Empty)
                            historyDesc += string.Format("Task [{0}]:", task.Title);
                        else
                            historyDesc += ",";
                        historyDesc += string.Format(" {0} => {1}", oldStatus, task.Status);
                    }

                    if (historyDesc != string.Empty)
                    {
                        //uHelper.CreateHistory(SPContext.Current.Web.CurrentUser, historyDesc, ticketItem, false);
                        //ticketItem.UpdateOverwriteVersion();
                    }
                    #endregion
                    ///update dependent tasks

                    UGITTaskManager.StartTasks(TicketPublicId, task.ID.ToString(), task.Behaviour == "Task");
                    UGITTaskManager.MoveSVCTicket(TicketPublicId);

                    int updatedTasks = tasks.Where(x => x.Changes).Count();
                    //if (updatedTasks > 2 && ModuleName == "PMM" && hdnActionType.Value == "BaselineAndSave" && baselineTaskThreshold > 0)
                    //{
                    //    hdnActionType.Value = string.Empty;
                    //    SPListItem project = Ticket.getCurrentTicket(ModuleName, TicketPublicId);
                    //    PMMBaseline baseline = new PMMBaseline(TicketID, DateTime.Now);
                    //    baseline.BaselineComment = string.Format("Changed status of {0} tasks as Completed", updatedTasks);
                    //    baseline.CreateBaseline(project);


                }


            }

            //AgentJobHelper agentHelper = new AgentJobHelper(SPContext.Current.Web);
            //agentHelper.DeleteTaskReminder(taskID.ToString());

            if (RunApproveService)
            {
                Session["isRequestTaskCompleted"] = "true";
            }
            uHelper.ClosePopUpAndEndResponse(Context, true, "/refreshpage/");
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            //int taskid = 0;
            //bool copychild = false;
            //object[] a = (object[])hdntask.Get("taskid");
            //taskid = Convert.ToInt32(a[0]);

            //UGITTask.CopyTask(taskid, TicketPublicId, copychild, ModuleName);
            pcDuplicate.ShowOnPageLoad = false;
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            //int taskid = 0;
            //bool copychild = true;
            //object[] a = (object[])hdntask.Get("taskid");
            //taskid = Convert.ToInt32(a[0]);

            //UGITTask.CopyTask(taskid, TicketPublicId, copychild, ModuleName);
            pcDuplicate.ShowOnPageLoad = false;
        }

        /// <summary>
        /// To Copy Task and Copy Child Task it is True.
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="ticketPublicId"></param>
        /// <param name="copyChild"></param>
        /// <param name="moduleName"></param>
        public void CopyTask(int taskid, string ticketPublicId, bool copyChild, string moduleName)
        {
            var pTasks = UGITTaskManager.LoadByProjectID(moduleName, ticketPublicId);
            pTasks = UGITTaskManager.MapRelationalObjects(pTasks);

            UGITTask ptask = pTasks.FirstOrDefault(x => x.ID == taskid);

            CopyTasks(ptask, pTasks, ptask.ParentTaskID, copyChild, moduleName, true, ticketPublicId);

            pTasks = UGITTaskManager.MapRelationalObjects(pTasks);
            UGITTaskManager.ReOrderTasks(ref pTasks);

            //UGITTaskHelper.ReloadProjectTasks(moduleName, ticketPublicId);
        }

        public void CopyTasks(UGITTask ctask, List<UGITTask> pTasks, long pid, bool copyChild, string moduleName, bool isfirst, string ticketPublicId)
        {
            UGITTask t = UGITTaskManager.CopyTo(ctask, pTasks, pid, ticketPublicId);
            pTasks.Add(t);
            if (isfirst)
            {
                UGITTaskManager.PlacedTaskAt(t, ctask, ref pTasks);
            }
            else
            {
                UGITTask ptask = pTasks.FirstOrDefault(x => x.ID == pid);
                UGITTaskManager.PlacedTaskAt(t, ptask, ref pTasks);
            }
            pTasks = UGITTaskManager.MapRelationalObjects(pTasks);
            UGITTaskManager.ReOrderTasks(ref pTasks);
            UGITTaskManager.SaveTasks(ref pTasks, moduleName, ticketPublicId);
            if (copyChild)
            {
                if (ctask.ChildCount > 0 && ctask.ChildTasks != null)
                {
                    foreach (var childtask in ctask.ChildTasks.Reverse<UGITTask>())
                    {
                        CopyTasks(childtask, pTasks, t.ID, copyChild, moduleName, !isfirst, ticketPublicId);
                    }
                }
            }
        }

        class IDDataItemTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                UGITTaskManager taskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
                GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;
                UGITTask currentTask = taskManager.GetTaskById(Convert.ToString(gridContainer.KeyValue));
                if (currentTask != null)
                {
                    string ID = Convert.ToString(currentTask.ID);
                    HtmlGenericControl input1 = new HtmlGenericControl("INPUT");
                    input1.Attributes.Add("type", "hidden");
                    input1.Attributes.Add("value", ID);
                    HtmlGenericControl input2 = new HtmlGenericControl("INPUT");
                    input2.Attributes.Add("id", "hdnchildcount" + ID);
                    input2.Attributes.Add("type", "hidden");
                    input2.Attributes.Add("value", Convert.ToString(currentTask.ChildCount));
                    HtmlGenericControl input3 = new HtmlGenericControl("DIV");
                    input3.InnerHtml = Convert.ToString(currentTask.ItemOrder);
                    container.Controls.Add(input3);
                    container.Controls.Add(input1);
                    container.Controls.Add(input2);

                }
            }
        }

        class TitleDataItemTemplate : ITemplate
        {
            public bool DisableMarkAsCompleteTemplate { get; set; }
            public UserProfile CurrentUser { get; set; }
            public UserProfileManager ProfileManager { get; set; }
            public DataRow CurrentItem { get; set; }
            public void InstantiateIn(Control container)
            {
                UGITTaskManager taskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
                GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;
                UGITTask currentTask = taskManager.GetTaskById(Convert.ToString(gridContainer.KeyValue));
                DisableMarkAsCompleteTemplate = false;
                ASPxImage image1 = new ASPxImage();
                image1.ImageUrl = "/Content/ButtonImages/comments.png";
                image1.CssClass = "action-description";

                ASPxImage image2 = new ASPxImage();
                image2.CssClass = "action-add";
                image2.ImageUrl = "/Content/Images/add_icon.png";
                image2.ClientSideEvents.Click = "function() { addNewItem_Click(" + gridContainer.VisibleIndex + "); }";


                ASPxImage imageButton = new ASPxImage();
                imageButton.CssClass = "markascomplete-action markAsCompleteBtn";
                imageButton.ToolTip = "Mark as Complete";
                //imageButton.CommandName = "MarkAsComplete";
                imageButton.ID = "btMarkComplete";
                imageButton.ImageUrl = "/Content/images/accept-symbol.png";
                imageButton.Attributes.Add("Style", "padding-bottom: 5px;");
                //imageButton.ClientSideEvents.Click = "MarkAsComplete";
                imageButton.ClientSideEvents.Click = "function(event){return MarkAsComplete(event); }";

                if (currentTask != null)
                {
                    ASPxImage imgEditTask = new ASPxImage();
                    imgEditTask.CssClass = "markascomplete-action editTaskBtn";
                    imgEditTask.ToolTip = "Edit";
                    imgEditTask.ID = "btEditTask";
                    imgEditTask.ImageUrl = "/Content/images/editNewIcon.png";
                    imgEditTask.Attributes.Add("Style", "padding-left: 5px;padding-bottom: 5px; width:20px;");
                    imgEditTask.ClientSideEvents.Click = "function() { " + GetTitleHtmlData(taskManager, gridContainer.Text, Convert.ToString(gridContainer.KeyValue), currentTask.ModuleNameLookup, true, false, false, true) + " }";

                    HtmlGenericControl contentTitle = new HtmlGenericControl("DIV");
                    contentTitle.Attributes.Add("id", "contentTitle");
                    contentTitle.Attributes.Add("class", "task-title");
                    contentTitle.Attributes.Add("style", "float: left;");
                    contentTitle.Attributes.Add("runat", "server");


                    contentTitle.InnerHtml = GetTitleHtmlData(taskManager, gridContainer.Text, Convert.ToString(gridContainer.KeyValue), currentTask.ModuleNameLookup, true);

                    TaskList tasklistContainer = new TaskList();
                    string oVal = Uri.UnescapeDataString(UGITUtility.GetCookieValue(System.Web.HttpContext.Current.Request, tasklistContainer.openChildrenCookie));
                    tasklistContainer.taskExpandStates = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OpenTaskToKeepState>>(oVal);
                    if (currentTask.ChildCount > 0)
                    {
                        string expandIcon = string.Format("<img style='float:left;padding-right:2px;' src='/Content/images/maximise.gif' colexpand='true' onclick=\"event.cancelBubble=true; OpenChildren('{0}', '{1}', this)\">", currentTask.ItemOrder, currentTask.ID);
                        if (tasklistContainer.taskExpandStates != null && tasklistContainer.taskExpandStates.Exists(x => x.id == currentTask.ID.ToString()))
                            expandIcon = string.Format("<img style='float:left;padding-right:2px;' src='/Content/images/minimise.gif' colexpand='true' onclick=\"event.cancelBubble=true; CloseChildren('{0}' , '{1}', this)\" />", currentTask.ItemOrder, currentTask.ID);

                        contentTitle.InnerHtml = string.Format("{0}{1}", expandIcon, contentTitle.InnerHtml);
                        contentTitle.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                    }
                    //if ((tasklistContainer.taskExpandStates == null || !tasklistContainer.taskExpandStates.Exists(x => x.id == currentTask.ParentTaskID.ToString())) && currentTask.ParentTaskID != 0)
                    //{
                    //    contentTitle.Style.Add("class", " hideElement");
                    //}

                    if (currentTask.Level > 0)
                        contentTitle.Style.Add(HtmlTextWriterStyle.PaddingLeft, string.Format("{0}px", currentTask.Level * 25));


                    HtmlGenericControl child2Div = new HtmlGenericControl("DIV");
                    child2Div.Attributes.Add("id", "actionButtons" + Convert.ToString(gridContainer.KeyValue));
                    child2Div.Attributes.Add("style", "display: none; float: right; top: -3px; right: -180px; padding: 1px 5px 0px; opacity: 1;  position:absolute;");
                    child2Div.Attributes.Add("class", "grid-task-btn markascomplete_actionWrap");

                    // // Allow mark as complete if admin OR ticket Owner OR task assignee
                    // if (ProfileManager.IsTicketAdmin(CurrentUser) ||
                    //ProfileManager.IsUserPresentInField(CurrentUser, CurrentItem, DatabaseObjects.Columns.TicketOwner) || ProfileManager.IsUserPresentInUserCollection(CurrentUser,currentTask.AssignedTo))

                    // {
                    //     DisableMarkAsCompleteTemplate = true;
                    // }

                    // // Disable mark as complete if in Waiting, Completed, Cancelled status OR Access Task Pending Approval OR Account Task
                    // if (currentTask.OnHold || currentTask.Status.ToLower() == Constants.Waiting.ToLower() || currentTask.Status.ToLower() == Constants.Completed.ToLower() || currentTask.Status.ToLower() == Constants.Cancelled.ToLower() ||
                    //     (currentTask.SubTaskType.ToLower() == ServiceSubTaskType.AccessTask.ToLower() && !string.IsNullOrEmpty(currentTask.ApprovalType) && currentTask.ApprovalStatus.ToLower() == "pending") ||
                    //     currentTask.SubTaskType.ToLower() == ServiceSubTaskType.AccountTask.ToLower())
                    // {
                    //     DisableMarkAsCompleteTemplate = false;
                    // }
                    if (currentTask.StartDate == DateTime.MinValue && currentTask.DueDate == DateTime.MinValue)
                        DisableMarkAsCompleteTemplate = true;

                    if (!DisableMarkAsCompleteTemplate)
                    {
                        //child2Div.Controls.Add(image1);
                        //child2Div.Controls.Add(image2);
                        if (currentTask.Behaviour == "Task" && (currentTask.Status != "Completed" && currentTask.Status != "Waiting"))
                            child2Div.Controls.Add(imageButton);

                        child2Div.Controls.Add(imgEditTask);
                    }


                    HtmlGenericControl parentDiv = new HtmlGenericControl("DIV");
                    parentDiv.Attributes.Add("id", "div_title_" + Convert.ToString(gridContainer.KeyValue));
                    parentDiv.Attributes.Add("style", "float: left; width: 250px; position:relative;");
                    parentDiv.Controls.Add(contentTitle);
                    parentDiv.Controls.Add(child2Div);
                    container.Controls.Add(parentDiv);

                }
            }
        }

        class TicketIDDataItemTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                UGITTaskManager taskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
                GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;
                UGITTask currentTask = taskManager.GetTaskById(Convert.ToString(gridContainer.KeyValue));

                HtmlGenericControl contentTitle = new HtmlGenericControl("DIV");
                contentTitle.Attributes.Add("id", "contentTitle");
                contentTitle.Attributes.Add("class", "task-title");
                contentTitle.Attributes.Add("style", "float: left;");
                contentTitle.Attributes.Add("runat", "server");
                if (currentTask != null)
                {
                    contentTitle.InnerHtml = GetTitleHtmlData(taskManager, currentTask.RelatedTicketID, Convert.ToString(gridContainer.KeyValue), currentTask.ModuleNameLookup, true, false, true);
                }

                HtmlGenericControl parentDiv = new HtmlGenericControl("DIV");
                parentDiv.Controls.Add(contentTitle);
                container.Controls.Add(parentDiv);
            }
        }

        class MyPropertiesSpinEdit : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;
            }
        }

        class MyImage : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                ASPxImage image = new ASPxImage();
                GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;
                image.ImageUrl = "/Content/ButtonImages/comments.png";
                image.CssClass = "action-description";
                gridContainer.Controls.Add(image);
            }
        }

        class TitleFooter : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                GridViewFooterCellTemplateContainer gridFooter = (GridViewFooterCellTemplateContainer)container;
                HtmlGenericControl DIV1 = new HtmlGenericControl("div");
                HtmlGenericControl DIV2 = new HtmlGenericControl("div");
                HtmlGenericControl DIV3 = new HtmlGenericControl("div");

                DIV2.Attributes.Add("style", "");
                DIV3.Attributes.Add("style", "");

                ASPxButton saveButton = new ASPxButton();
                saveButton.ID = "updateTask";
                saveButton.Text = "Save Changes";
                saveButton.CssClass = "updateTask nprschedule_saveBtn";
                saveButton.ImageUrl = "/Content/Images/saveFile_icon.png";
                saveButton.Image.Width = 16;
                saveButton.ClientSideEvents.Click = "function(){ gridTaskList_BatchUpdate() }";
                saveButton.AutoPostBack = true;
                DIV2.Controls.Add(saveButton);

                ASPxButton cancelButton = new ASPxButton();
                cancelButton.ID = "cancelButton";
                cancelButton.Text = "Cancel Changes";
                cancelButton.CssClass = "nprschedule_cancelBtn";
                //cancelButton.ImageUrl = "/Content/ButtonImages/cancelwhite.png";
                cancelButton.ClientSideEvents.Click = "function(){ gridTaskList_CancelBatchUpdate() }";
                cancelButton.Attributes.Add("style", "margin-left:6px;"); // = Unit.Pixel(5);
                DIV3.Controls.Add(cancelButton);
                DIV1.Controls.Add(DIV2);
                DIV1.Controls.Add(DIV3);
                //button.Click += gridTaskList_BatchUpdate;
                //container.Controls.Add(saveButton);
                container.Controls.Add(DIV1);

            }
        }

        public class OpenTaskToKeepState
        {
            public string id { get; set; }
        }

        public class BatchGridExtraData
        {
            public int PreviousTaskID { get; set; }
            public int NextTaskID { get; set; }
            public int Index { get; set; }
        }

        protected void gridTaskList_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == DatabaseObjects.Columns.DueDate || e.Column.FieldName == DatabaseObjects.Columns.StartDate)
            {
                DateTime textValue = Convert.ToDateTime(e.Value);

                if (textValue == new DateTime(1800, 1, 1) || textValue == DateTime.MinValue)
                {
                    e.DisplayText = string.Empty;
                }

            }
            else if (e.Column.FieldName == DatabaseObjects.Columns.TicketRequestTypeCategory)
            {
                field = FieldConfigurationManager.GetFieldByFieldName(DatabaseObjects.Columns.TicketRequestTypeLookup);

                if (field != null)
                {
                    string value = FieldConfigurationManager.GetFieldConfigurationData(field, Convert.ToString(e.Value));//Set only Request type.

                    value = value.Split().LastOrDefault();

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        e.DisplayText = value;
                    }
                    else
                    {
                        e.DisplayText = "";
                    }
                }
            }
            //if (e.Column.FieldName == DatabaseObjects.Columns.Status)
            //{

            //    string statusVal = Convert.ToString(e.Value);
            //    if (statusVal == "Waiting")
            //        // e.Column.CellStyle.ForeColor = System.Drawing.Color.Blue;
            //        e.Column.CellStyle.BackColor = ColorTranslator.FromHtml("gray");
            //    else if (statusVal.Contains("Completed"))
            //    {
            //        e.Column.CellStyle.ForeColor = Color.Green;
            //    }

            //}


        }
        protected void gridTaskList_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;

            if (e.CommandArgs.CommandName == "MarkAsComplete")
            {
                int taskID = int.Parse(e.CommandArgs.CommandArgument.ToString());
                if (taskID > 0)
                {

                    MarkTaskAsComplete(taskID);
                }
            }
        }

        protected void DdlModuleDetail_Load(object sender, EventArgs e)
        {
            ddlModuleDetail.Items.Clear();

            List<UGITModule> modules = ModuleViewManager.Load(x => x.ModuleType == ModuleType.SMS);   // uGITCache.GetModuleList(ModuleType.SMS, SPContext.Current.Web, true);
            foreach (UGITModule module in modules)
            {
                // Condition for excluding SVC
                if (Convert.ToString(module.ModuleName) != "SVC")
                {
                    ListItem li = new ListItem(Convert.ToString(module.Title), Convert.ToString(module.ModuleName));
                    string source = string.Format("&source={0}", Guid.NewGuid().ToString());
                    string url = UGITUtility.GetAbsoluteURL(Convert.ToString(module.StaticModulePagePath));
                    li.Attributes.Add("Url", url);
                    ddlModuleDetail.Items.Add(li);
                }
            }

            //DataRow nprModule = uGITCache.GetModuleDetails("NPR");
            //if (nprModule != null)
            //{
            //    ListItem li = new ListItem(Convert.ToString(nprModule[DatabaseObjects.Columns.Title]), Convert.ToString(nprModule[DatabaseObjects.Columns.ModuleName]));
            //    string source = string.Format("&source={0}", Guid.NewGuid().ToString());
            //    string url = UGITUtility.GetAbsoluteURL(Convert.ToString(nprModule["ModuleRelativePagePath"]));
            //    li.Attributes.Add("Url", url);
            //    ddlModuleDetail.Items.Add(li);
            //}
        }
        /// <summary>
        /// SVC Config Tickets/Tasks
        /// </summary>
        /// <param name="dtTask"></param>
        /// <returns></returns>
        protected void GetTaskType(DataTable dtTask)
        {
            string type = string.Empty;
            UGITModule uGITModule = null;
            foreach (DataRow dr in dtTask.Rows)
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(dr[DatabaseObjects.Columns.RelatedModule])))
                {
                    type = Convert.ToString(dr[DatabaseObjects.Columns.RelatedModule]);

                    if (uGITModule == null || uGITModule.ModuleName.ToLower() != type.ToLower())
                        uGITModule = ModuleViewManager.GetByName(type);
                    if (uGITModule != null)
                        type = uGITModule.Title;
                }
                else if (!string.IsNullOrWhiteSpace(Convert.ToString(dr[DatabaseObjects.Columns.UGITSubTaskType])))
                {
                    type = Convert.ToString(dr[DatabaseObjects.Columns.UGITSubTaskType]);
                }
                else { type = "Task"; }

                dr["ModuleType"] = type;
            }
        }
    }
}