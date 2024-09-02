using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using System.Data;
using System.Web;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using uGovernIT.Manager;
using uGovernIT.Manager.PMM;
using uGovernIT.Manager.Managers;
using DevExpress.Web;
using DevExpress.Web.Rendering;

namespace uGovernIT.Web
{
    public partial class MyTasks : UserControl
    {
        public bool IsPreview;
        public int PageSize;

        private bool isGetFilteredDataDone;
        private DataTable resultedTable;
        //private int rowCount;
        protected string ajaxPageURL;

        public bool ByProject;
        public bool ByDueDate;
        public bool byprogress;
        public bool bycompletedtasks;
        protected string editTaskFormUrl = string.Empty;
        protected string editExitCriteriaTaskFormUrl = string.Empty;
        int projectCount;
        public string Width { get; set; }
        public string Height { get; set; }
        public bool DisplayOnDashboard { get; set; }
        public int openTaskCount { get; set; }
        protected string sourceURL;
        protected string calendarURL=string.Empty;
        protected string keepActualHourMandatorys;
        ConfigurationVariableManager ConfigVariableMGR = null;
        ApplicationContext AppContext = null;
        ModuleViewManager ModuleManager = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Show my task view by project or by due date
        }

        protected override void OnInit(EventArgs e)
        {
            AppContext = HttpContext.Current.GetManagerContext();
            ConfigVariableMGR = new ConfigurationVariableManager(AppContext);
            ModuleManager = new ModuleViewManager(AppContext);
            keepActualHourMandatorys = string.Format("{{\"SVC\":\"{0}\",\"PMM\":\"{1}\",\"TSK\":\"{2}\",\"NPR\":\"{3}\"}}",
                ConfigVariableMGR.GetValueAsBool(string.Format("{0}Task_MakeActualHoursMandatory", "SVC")),
                ConfigVariableMGR.GetValueAsBool(string.Format("{0}Task_MakeActualHoursMandatory", "PMM")),
                ConfigVariableMGR.GetValueAsBool(string.Format("{0}Task_MakeActualHoursMandatory", "NPR")),
                ConfigVariableMGR.GetValueAsBool(string.Format("{0}Task_MakeActualHoursMandatory", "TSK")));


            sourceURL = Request["source"] != null ? Request["source"] : Server.UrlEncode(Request.Url.AbsolutePath);
            editExitCriteriaTaskFormUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=modulestagetask");
            calendarURL= UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=taskcalender&openTask=true&moduleName=MyTask");
            editTaskFormUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/delegatecontrol.aspx?control=taskedit");
            ajaxPageURL = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/AjaxHelper.aspx");

            if (IsPreview)
            {
                //ScriptLink4.Visible = false;

                if (this.DisplayOnDashboard && !string.IsNullOrEmpty(Width))
                {
                    rootDiv.Style.Add("width", Width);
                    rootDiv.Style.Add("height", Height);
                    rootDiv.Style.Add("overflow-y", "auto");
                    rootDiv.Style.Add("padding", "16px");
                }
            }

            GenerateColumns();
            
            base.OnInit(e);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
        }

        //void gvFilteredList_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    if (ViewState[Constants.FilterExpression] != null)
        //    {
        //        ticketDataSource.FilterExpression = (string)ViewState[Constants.FilterExpression];

        //    }
          
        //}

        protected sealed override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            if (Context.Request.Form["__EVENTARGUMENT"] != null &&
                 Context.Request.Form["__EVENTARGUMENT"].EndsWith("__ClearFilter__"))
            {
                // Clear FilterExpression
                ViewState.Remove(Constants.FilterExpression);
            }
        }

        void ticketDataSource_Filtering(object sender, ObjectDataSourceFilteringEventArgs e)
        {
            ViewState[Constants.FilterExpression] = ((ObjectDataSourceView)sender).FilterExpression;
        }

        private void TicketDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = this;
        }
        public DataTable getDataSource()
        {
            if (isGetFilteredDataDone)
            {
                return resultedTable;
            }
            else
            {
                return GetConfigTables();
            }
        }

        protected void GvFilteredList_Init(object sender, EventArgs e)
        {
            gvFilteredList.SettingsPager.Visible = false;

            moreOptionPanel.Visible = false;
            if (IsPreview && PageSize <= 0)
            {
                gvFilteredList.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            }

            string myTaskViewType = UGITUtility.GetCookieValue(Request, "mytaskviewtype");
            if (Request["myTaskViewType"] != null)
            {
                myTaskViewType = Request["myTaskViewType"];
            }
            if (myTaskViewType.ToLower() == "byduedate")
            {

                ByProject = false;
                ByDueDate = true;
                byprogress = false;
                bycompletedtasks = false;
            }
            else if (myTaskViewType.EqualsIgnoreCase("byproject"))
            {
                ByProject = true;
                ByDueDate = false;
                byprogress = false;
                bycompletedtasks = false;
            }
            else if (myTaskViewType.EqualsIgnoreCase("byprogress"))
            {
                ByProject = false;
                ByDueDate = false;
                byprogress = true;
                bycompletedtasks = false;
            }
            else if (myTaskViewType.EqualsIgnoreCase("bycompletedtasks"))
            {
                ByProject = false;
                ByDueDate = false;
                byprogress = false;
                bycompletedtasks = true;
            }
            else
            {
                ByProject = true;
                ByDueDate = false;
                byprogress = false;
                bycompletedtasks = false;
            }

            gvFilteredList.DataBind();
                          
        }
        void GvFilteredList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFilteredList.PageIndex = e.NewPageIndex;
        }
        protected void GvFilteredList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {

        }
        
        protected void GvFilteredList_PreRender(object sender, EventArgs e)
        {

        }

        protected override void OnPreRender(EventArgs e)
        {
            //// get rowcount and  bind gridview to show data
            //ViewState[Constants.FilterExpression] = ticketDataSource.FilterExpression;
            //DataView view = (System.Data.DataView)ticketDataSource.Select();
            //rowCount = (view == null ? 0 : view.Count);
            gvFilteredList.DataBind();
            string myTaskViewType = UGITUtility.GetCookieValue(Request, "mytaskviewtype");
            if (Request["myTaskViewType"] != null)
            {
                myTaskViewType = Request["myTaskViewType"];
            }

            if (myTaskViewType.ToLower() == "byduedate")
            {
                myTaskByDueDate.Attributes.Add("class", "ucontentdiv ugitsellinkbg clickedTab");
                myTaskByProject.Attributes.Add("class", "ucontentdiv ugitlinkbg");
                myTaskByProgress.Attributes.Add("class", "ucontentdiv ugitlinkbg");
                myTaskByCompleted.Attributes.Add("class", "ucontentdiv ugitlinkbg");
            }
            else if (myTaskViewType.ToLower() == "byproject")
            {
                myTaskByProject.Attributes.Add("class", "ucontentdiv ugitsellinkbg clickedTab");
                myTaskByDueDate.Attributes.Add("class", "ucontentdiv ugitlinkbg");
                myTaskByProgress.Attributes.Add("class", "ucontentdiv ugitlinkbg");
                myTaskByCompleted.Attributes.Add("class", "ucontentdiv ugitlinkbg");
            }
            else if (myTaskViewType.ToLower() == "byprogress")
            {
                myTaskByProgress.Attributes.Add("class", "ucontentdiv ugitsellinkbg clickedTab");
                myTaskByProject.Attributes.Add("class", "ucontentdiv ugitlinkbg");
                myTaskByDueDate.Attributes.Add("class", "ucontentdiv ugitlinkbg");
                myTaskByCompleted.Attributes.Add("class", "ucontentdiv ugitlinkbg");
            }
            else if (myTaskViewType.ToLower() == "bycompletedtasks")
            {
                myTaskByCompleted.Attributes.Add("class", "ucontentdiv ugitsellinkbg clickedTab");
                myTaskByDueDate.Attributes.Add("class", "ucontentdiv ugitlinkbg");
                myTaskByProject.Attributes.Add("class", "ucontentdiv ugitlinkbg");
                myTaskByProgress.Attributes.Add("class", "ucontentdiv ugitlinkbg");
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Generates resulted table and bind table column with gridview
        /// </summary>
        /// <returns></returns>
        private DataTable GetConfigTables()
        {
            isGetFilteredDataDone = true;
            UGITTaskManager taskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
            UserProfile currentProfile = HttpContext.Current.GetManagerContext().CurrentUser;
            DataTable table = taskManager.GetOpenedTasksByUser(currentProfile.Id, true, new string[] { "NPR", "SVCConfig", "Template" }, bycompletedtasks);
            if (table != null)
            {
                DataView view = table.DefaultView;
                if (ByProject)
                    view.Sort = string.Format("{0} asc, {1} asc", DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.DueDate);
                else if (ByDueDate || byprogress) // ByDueDate
                    view.Sort = string.Format("{0} asc, {1} asc", DatabaseObjects.Columns.DueDate, DatabaseObjects.Columns.TicketId);
                else if (bycompletedtasks)
                    view.Sort = string.Format("{0} asc, {1} desc", DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.CompletionDate);

                        table = view.ToTable();
                if (IsPreview && table.Rows.Count > PageSize && PageSize !=0)
                {
                    moreOptionPanel.Visible = true;
                    btMoreOption.NavigateUrl = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','{1}','{2}','90','90', 0, '{3}')", UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=ShowMyTasks"), "", "My Tasks", sourceURL);
                }

                var projects = table.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)).Distinct();
                projectCount = projects.Count();
                openTaskCount = table.Rows.Count;

                if (projectCount == 1 && table.Rows.Count > 0)
                {

                    //DataRow row = table.NewRow();
                    //table.Rows.InsertAt(row, 0);

                }

                if (IsPreview && table.Rows.Count >= PageSize && PageSize != 0)
                {
                    resultedTable = table.AsEnumerable().Take(PageSize).ToArray().CopyToDataTable();
                }
                else
                {
                    resultedTable = table;
                }

                
            }
            else
            {
                resultedTable = table;
            }

            return table;
        }

        /// <summary>
        /// Bind table column with gridview
        /// </summary>
        private void GenerateColumns()
        {
            if (gvFilteredList.Columns.Count <= 0)
            {
                ModuleColumnManager moduleColumnManager = new ModuleColumnManager(HttpContext.Current.GetManagerContext());
                DataRow[] selectedColumns = moduleColumnManager.GetModuleColumnDataTable($"{DatabaseObjects.Columns.CategoryName} = '{Constants.MyTaskTab}'").Select();  //uGITCache.GetDataTable(DatabaseObjects.Lists.MyModuleColumns, DatabaseObjects.Columns.CategoryName, Constants.MyTaskTab);
                if (selectedColumns.Length <= 0)
                {
                    return;
                }

                StringBuilder filterDataFields = new StringBuilder();
                GridViewDataTextColumn colTitle = null;

                //Shows project ID column if view type is by due date
                if (projectCount > 0)
                {
                    colTitle = new GridViewDataTextColumn();
                    colTitle.FieldName = "TicketId";
                    colTitle.Caption = "Project ID";
                    colTitle.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                    colTitle.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                    if (ByProject)
                    {
                        gvFilteredList.ExpandAll();
                        colTitle.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.True;
                        colTitle.GroupBy();
                        colTitle.GroupIndex = 0;
                    }
                    else
                    {
                        colTitle.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                        colTitle.UnGroup();
                        colTitle.GroupIndex = -1;
                    }
                    gvFilteredList.Columns.Add(colTitle);
                    filterDataFields.AppendFormat("{0},", "ProjectID");
                }

                Dictionary<string, string> customProperties = new Dictionary<string, string>();

                selectedColumns = selectedColumns.Where(x => x.Field<string>(DatabaseObjects.Columns.IsDisplay) == "True").OrderBy(x => x.Field<int>(DatabaseObjects.Columns.FieldSequence)).ToArray();
                foreach (DataRow moduleColumn in selectedColumns)
                {
                    //1. check for column exist is resultedtable or not
                    if (resultedTable != null && resultedTable.Columns.Contains(moduleColumn["FieldName"].ToString()))
                    {
                        if (ByProject && Convert.ToString(moduleColumn["FieldName"]) == DatabaseObjects.Columns.ProjectTitle)
                            continue;
                        GridViewDataTextColumn colId = new GridViewDataTextColumn();
                        colId.FieldName = moduleColumn["FieldName"].ToString();
                        colId.Caption = moduleColumn["FieldDisplayName"].ToString(); 
                        //colId.SortExpression = moduleColumn["FieldName"].ToString();
                        colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                        if (colId.FieldName == DatabaseObjects.Columns.PercentComplete)
                        {
                            colId.PropertiesEdit.DisplayFormatString = "{0}%";
                        }

                        if (Convert.ToString(moduleColumn["FieldName"]) == "ID")
                        {
                            colId.Visible = false;
                        }

                        if (resultedTable.Columns[moduleColumn["FieldName"].ToString()].DataType == typeof(DateTime))
                        {
                            colId.PropertiesEdit.DisplayFormatString = "{0: MMM-d-yyyy}";
                            colId.HeaderStyle.CssClass = "ms-vh2";
                        }

                        //resultedTable.Columns[moduleColumn["FieldName"].ToString()].DataType == typeof(DateTime) 
                        if (Convert.ToString(moduleColumn["FieldName"]) == DatabaseObjects.Columns.TicketId)
                        {
                            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        }
                        
                        gvFilteredList.Columns.Add(colId);

                        if (resultedTable.Columns[moduleColumn["FieldName"].ToString()].DataType != typeof(DateTime))
                        {
                            filterDataFields.AppendFormat("{0},", moduleColumn["FieldName"]);
                        }
                        else
                        {
                            filterDataFields.Append(",");
                        }
                        
                    }
                }

                gvFilteredList.SettingsBehavior.AllowSort = true;
                gvFilteredList.SettingsBehavior.AllowGroup = true;

                //Desables filtering, sorting in privew mode
                if (!IsPreview)
                {
                    gvFilteredList.FilterEnabled = true;
                    //gvFilteredList.FilterDataFields = filterDataFields.ToString();
                    //gvFilteredList.FilteredDataSourcePropertyName = Constants.FilterExpression;
                    //gvFilteredList.FilteredDataSourcePropertyFormat = "{1} = '{0}'";
                    gvFilteredList.SettingsBehavior.AllowSort = true;
                }
            }

            ////Disable grouping when view type id byduedate
            if (ByProject)
            {
                gvFilteredList.ExpandAll();
                gvFilteredList.GroupBy(gvFilteredList.Columns[DatabaseObjects.Columns.TicketId]);
                gvFilteredList.SettingsBehavior.AllowGroup = true;
                //gvFilteredList.GroupField = "ProjectID";
                //gvFilteredList.DisplayGroupFieldName = false;
                //gvFilteredList.GroupDescriptionField = "ProjectTitle";
                //gvFilteredList.GroupFieldDisplayName = "";
                //gvFilteredList.GroupMenu.ShowHeader = false;

            }
            else
            {
                gvFilteredList.UnGroup(gvFilteredList.Columns[DatabaseObjects.Columns.TicketId]);
                gvFilteredList.SettingsBehavior.AllowGroup = false;
            }
        }

        protected void btMyTaskByProject_Click(object sender, EventArgs e)
        {
            gvFilteredList.ExpandAll();
            gvFilteredList.GroupBy(gvFilteredList.Columns[DatabaseObjects.Columns.TicketId]);
            gvFilteredList.SettingsBehavior.AllowGroup = true;
        }

        protected void btMyTaskByDueDate_Click(object sender, EventArgs e)
        {
            gvFilteredList.UnGroup(gvFilteredList.Columns[DatabaseObjects.Columns.TicketId]);
            gvFilteredList.SettingsBehavior.AllowGroup = false;
        }

        protected void gvFilteredList_DataBinding(object sender, EventArgs e)
        {
            gvFilteredList.DataSource = getDataSource();
        }

        protected void gvFilteredList_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {

            if (e.RowType == GridViewRowType.Data)
            {
                string showTab = string.Empty;
                DataRow currentRow = gvFilteredList.GetDataRow(e.VisibleIndex);
                DataRow row = currentRow;
                string moduleName = Convert.ToString(row[DatabaseObjects.Columns.ModuleNameLookup]);

                UGITModule moduleRow = ModuleManager.LoadByName(moduleName);
                if (moduleName.Equals(Constants.ExitCriteria))
                {
                    //Exit criteria is name given in place of actual modulename,  to group  all the moduleStage open tasks from any module. 
                    //We can derive the actual module name from ticketpublicid/Projectid.
                    if (row["TicketId"] != null)
                    {
                        string[] actualModuleNameData = Convert.ToString(row["TicketId"]).Split('-');
                        if (actualModuleNameData != null && actualModuleNameData.Length > 0)
                        {
                            string actualModule = actualModuleNameData.First().Trim();
                            moduleRow = ModuleManager.LoadByName(actualModule);

                            if (actualModule.Equals("NPR"))
                                showTab = "&showTab=5";
                            else if (actualModule.Equals("PRS"))
                                showTab = "&showTab=3";
                            else
                                showTab = "&showTab=1";
                        }
                    }
                }

                if (moduleRow != null)
                {
                    //Set click event on each row
                    string title = UGITUtility.TruncateWithEllipsis(row[DatabaseObjects.Columns.Title].ToString(), 100, string.Empty);
                    title = string.Format("{0}: {1}", Convert.ToString(row[DatabaseObjects.Columns.TicketId]), UGITUtility.ReplaceInvalidCharsInURL(title)); 

                    if (moduleName == "PMM")
                    {
                        showTab = "&showTab=2";
                    }
                    if (moduleName.Equals(Constants.ExitCriteria))
                    {
                        // string projectUrl =  Ticket.GenerateTicketURL(moduleName, Convert.ToString(rowView["ProjectID"]));

                        e.Row.Cells[gvFilteredList.Columns.IndexOf(gvFilteredList.Columns["DatabaseObjects.Columns.Title"])].Attributes.Add("OnClick", string.Format("$(this).hasClass('editrow') ? '' : window.parent.UgitOpenPopupDialog('{0}','{1}','{2}','90','90', 0, '{3}');", UGITUtility.GetAbsoluteURL(Convert.ToString(moduleRow.StaticModulePagePath)), string.Format("TicketId={0}{1}", Convert.ToString(row["ProjectID"]), showTab), title, sourceURL));
                        //gvRow.Attributes.Add("OnClick", string.Format("$(this).hasClass('editrow') ? '' : window.parent.UgitOpenPopupDialog('{0}','{1}','{2}','90','90', 0, '{3}');", UGITUtility.GetAbsoluteURL(Convert.ToString(moduleRow.StaticModulePagePath)), string.Format("TicketId={0}{1}", Convert.ToString(rowView["ProjectID"]), showTab), title, sourceURL));
                    }
                    else
                    {
                        e.Row.Cells[gvFilteredList.Columns.IndexOf(gvFilteredList.Columns["Title"])].Attributes.Add("OnClick", string.Format("$(this).hasClass('editrow') ? '' : window.parent.UgitOpenPopupDialog('{0}','{1}','{2}','90','90', 0, '{3}');", UGITUtility.GetAbsoluteURL(Convert.ToString(moduleRow.StaticModulePagePath)), string.Format("TicketId={0}{1}", Convert.ToString(row["TicketId"]), showTab), title, sourceURL));
                    }
                }

                //Change background on mouse over
                e.Row.Attributes.Add("onmouseover", "showTasksActions(this);");
                e.Row.Attributes.Add("onmouseout", "hideTasksActions(this);");

                //Put description icon in first column of each row if exist
                if (gvFilteredList.IsGroupRow(e.VisibleIndex))
                {
                    e.Row.Cells[0].Controls.Clear();
                    e.Row.Cells[0].Style.Remove(HtmlTextWriterStyle.Width);
                    if (Convert.ToString(UGITUtility.GetSPItemValue(row, DatabaseObjects.Columns.Description)) != string.Empty)
                    {
                        e.Row.Cells[0].Controls.Add(new LiteralControl("<span  class='action-description' style='float:left;padding-left:2px;visibility:hidden'><img src='/Content/buttonimages/comments.png' style='cursor:help;'/></span>"));
                    }
                }
                else
                {
                    if (Convert.ToString(UGITUtility.GetSPItemValue(row, DatabaseObjects.Columns.Description)) != string.Empty)
                    {
                        e.Row.Cells[0].Text = string.Format("<span>{0}</span><span  class='action-description' style='float:right;padding-left:2px;visibility:hidden'><img src='/Content/buttonimages/comments.png' style='cursor:help;'/></span>", e.Row.Cells[0].Text);
                    }
                }

                //Set some task information as attribute of row
                e.Row.Attributes.Add("taskDesc", Server.UrlEncode(Convert.ToString(row[DatabaseObjects.Columns.Description])));
                e.Row.Attributes.Add("moduleName", moduleName);
                e.Row.Attributes.Add("projectID", Convert.ToString(row[DatabaseObjects.Columns.TicketId]));
                e.Row.Attributes.Add("taskID", Convert.ToString(row["ID"]));
                e.Row.Attributes.Add("taskTitle", Server.UrlEncode(Convert.ToString(row["Title"])));

                if (!(row[DatabaseObjects.Columns.IsCritical] is DBNull) && Convert.ToBoolean(row[DatabaseObjects.Columns.IsCritical]))
                {
                    e.Row.Cells[0].Text = string.Format("<span>{0}</span><span style='float:right;padding-left:2px;'><img src='/Content/buttonimages/critical.png' alt='critical' title='critical'/></span>", e.Row.Cells[0].Text);
                }

                GridViewTableDataCell editCell = null;
                GridViewDataColumn cellColumn = null;
                foreach (object cell in e.Row.Cells)
                {
                    if (cell is GridViewTableDataCell)
                    {
                        editCell = (GridViewTableDataCell)cell;
                        cellColumn = editCell.Column as GridViewDataColumn;
                        if (editCell != null)
                        {
                            if (cellColumn.FieldName == DatabaseObjects.Columns.Status)
                            {
                                if (UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(row, DatabaseObjects.Columns.TicketOnHold)))
                                    editCell.CssClass = "taskactions statuscolumn clsChangeBackgroundOnHold";
                                else
                                    editCell.CssClass = "taskactions statuscolumn";

                                editCell.Attributes.Add("status", Convert.ToString(UGITUtility.GetSPItemValue(row, DatabaseObjects.Columns.Status)));
                            }
                            else if (cellColumn.FieldName == DatabaseObjects.Columns.PercentComplete)
                            {
                                editCell.CssClass = "pctcompletecolumn";
                                editCell.Attributes.Add("pctComplete", Convert.ToString(UGITUtility.GetSPItemValue(row, DatabaseObjects.Columns.PercentComplete)));
                            }
                            else if (cellColumn.FieldName == DatabaseObjects.Columns.Title)
                            {
                                //TableCell tCell = gvRow.Cells[gvFilteredList.Columns.IndexOf(field)];
                                Panel panel = new Panel();
                                editCell.Text = string.Empty;
                                panel.Attributes.Add("style", "position:relative;float:left;width:100%;");
                                Label lbItemInfo = new Label();
                                string itemInfoJson = string.Format("{0}\"itemid\":{2},\"status\":\"{3}\",\"pctcomplete\":{4},\"title\":\"{5}\",\"projectid\":\"{6}\",\"modulename\":\"{7}\",\"itemtype\":\"{8}\"{1}",
                                   "{", "}", row[DatabaseObjects.Columns.ID], row[DatabaseObjects.Columns.Status], row[DatabaseObjects.Columns.PercentComplete], Server.UrlEncode(Convert.ToString(row["Title"])), row["TicketId"], row[DatabaseObjects.Columns.ModuleNameLookup], row["Behaviour"]);
                                lbItemInfo.Text = itemInfoJson;
                                lbItemInfo.CssClass = "hide budgetiteminfo";
                                panel.Controls.Add(lbItemInfo);

                                Label lbTaskTitle = new Label();
                                lbTaskTitle.Text = Convert.ToString(row[DatabaseObjects.Columns.Title]);
                                panel.Controls.Add(lbTaskTitle);

                                Label lbTaskAction = new Label();
                                lbTaskAction.CssClass = "action-container hide";
                                UGITTaskProposalStatus proposalStatus = UGITTaskProposalStatus.Not_Proposed;
                                if (Convert.ToString(row[DatabaseObjects.Columns.UGITProposedStatus]) != string.Empty)
                                {
                                    proposalStatus = (UGITTaskProposalStatus)Enum.Parse(typeof(UGITTaskProposalStatus),
                                                                                    Convert.ToString(row[DatabaseObjects.Columns.UGITProposedStatus]));
                                }

                                if (proposalStatus == UGITTaskProposalStatus.Pending_AssignTo || (Convert.ToString(row[DatabaseObjects.Columns.Approvalstatus]).ToLower() == TaskApprovalStatus.Pending) || UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.TicketOnHold]) || Convert.ToString(row[DatabaseObjects.Columns.TaskBehaviour]) == "Account Task")
                                {
                                    lbTaskAction.Text = string.Format(@"<span><img style='cursor: pointer;'  src='/Content/images/edit-icon.png' title='Edit Status' onclick='event.cancelBubble=true;doTaskEdit(this)'/></span>");
                                }
                                else
                                {
                                    if (Convert.ToString(UGITUtility.GetSPItemValue(row, DatabaseObjects.Columns.PercentComplete)) == "100")
                                        lbTaskAction.Text = string.Format("<span><img style='cursor: pointer;'  src='/Content/images/edit-icon.png' title='Edit Status' onclick='event.cancelBubble=true;doTaskEdit(this)'/></span>", row[DatabaseObjects.Columns.TaskActualHours]);
                                    else
                                        lbTaskAction.Text = string.Format("<span><img style='cursor: pointer;' src='/Content/images/tick-icon.png' title='Mark as Complete' style='cursor:pointer;' onclick='event.cancelBubble=true;doTaskComplete(this,\"{0}\")'  /><img style='cursor: pointer;'  src='/Content/images/edit-icon.png' title='Edit Status' onclick='event.cancelBubble=true;doTaskEdit(this)'/></span>", row[DatabaseObjects.Columns.TaskActualHours]);
                                }
                                if (UGITUtility.StringToBoolean(ConfigVariableMGR.GetValue(ConfigConstants.AllowAddFromMyTasks)))
                                {
                                    if (moduleName == "TSK" || moduleName == "PMM")
                                    {
                                        lbTaskAction.Text += string.Format(@"&nbsp;<img style='cursor: pointer;'  src='/Content/images/task-new.png' title='Add new Task' onclick='event.cancelBubble=true;doTaskAdd(this)'/>");
                                    }
                                }
                                panel.Controls.Add(lbTaskAction);
                                editCell.Controls.Add(panel);
                            }
                        }
                    }
                }
            }
        }

        protected void gvFilteredList_CustomGroupDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == DatabaseObjects.Columns.TicketId)
            {
                DataRow row = gvFilteredList.GetDataRow(e.VisibleIndex);
                if (row != null)
                    e.DisplayText = $"{UGITUtility.ObjectToString(e.Value)}: {UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Title])}";
            }
        }

        protected void btMyTaskByProgress_Click(object sender, EventArgs e)
        {
            gvFilteredList.UnGroup(gvFilteredList.Columns[DatabaseObjects.Columns.TicketId]);
            gvFilteredList.FilterExpression = "Status='In Progress'";
            gvFilteredList.SettingsBehavior.AllowGroup = false;
        }

        protected void btMyTaskByCompleted_Click(object sender, EventArgs e)
        {
            gvFilteredList.UnGroup(gvFilteredList.Columns[DatabaseObjects.Columns.TicketId]);
            gvFilteredList.FilterExpression = "Status='Completed'";
            gvFilteredList.SettingsBehavior.AllowGroup = false;
        }
    }
}
