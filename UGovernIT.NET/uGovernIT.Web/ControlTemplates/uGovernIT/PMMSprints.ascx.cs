using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class PMMSprints : UserControl
    {
        #region variables
        DataTable dtProductBacklog = new DataTable();
        DataTable dtSprintTasks = new DataTable();
        DataTable dtSprints = new DataTable();
        DataTable dtReleases = new DataTable();
        public string ticketId { get; set; }
        public string ModuleName { get; set; }
        public string Iframe { get; set; }
        protected string RequestUrl { get; set; }  
        protected string SprintRequestUrl { get; set; }
        protected string ReleaseRequestUrl { get; set; }
        protected int noOfWorkingDays { get; set; }
        protected string ajaxHelperURL = string.Empty;
        public bool ReadOnly;

        public new long ID = 0;

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ConfigurationVariableManager configurationVariable = null;
        Ticket tkt = null;
        SprintManager sprintManager = null;
        SprintTaskManager sprintTaskManager = null;
        PMMReleaseManager releaseManager = null;
        UserProfileManager UserManager = null;
        TicketManager ticketManager = null;

        #endregion

        #region PageEvent
        protected override void OnInit(EventArgs e)
        {
            configurationVariable = new ConfigurationVariableManager(context);
            tkt = new Ticket(context, "PMM");
            sprintManager = new SprintManager(context);
            sprintTaskManager = new SprintTaskManager(context);
            releaseManager = new PMMReleaseManager(context);
            UserManager = HttpContext.Current.GetUserManager();
            ticketManager = new TicketManager(context);

            gvProductBacklog.SettingsPager.Visible = false;
            gvSprintTasks.SettingsPager.Visible = false;
            ajaxHelperURL = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/ajaxhelper.aspx");

            //new line of code of show sprint form task list.
            if (Request.QueryString["viewType"] == "1" && Request.QueryString["viewType"] != null)
            {
                contenSplitter.GetPaneByName("ProductBackLog").Visible = false;
                contenSplitter.GetPaneByName("PaneReleaseSprint").Visible = false;
                contenSplitter.Height = new Unit(Convert.ToInt32(Request["Height"]) - 50, UnitType.Pixel);
                // contenSplitter.Height = "400";
                // if(!IsPostBack)

            }

            ID = Convert.ToInt64(ticketManager.GetSingleValueByTicketID("PMM", DatabaseObjects.Columns.ID, ticketId));
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //set cookie for radio button...
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(UGITUtility.GetCookieValue(Request, "ProductBacklogType")) && UGITUtility.GetCookieValue(Request, "ProductBacklogType") == "Unallocated")
                {
                    rdProductBackLog.SelectedValue = "unallocated";
                }
            }

            bool isUpdate = false;
            if (Request.Form["__CALLBACKPARAM"] != null)
            {
                string callBackValues = Request.Form["__CALLBACKPARAM"];
                string[] val = Request.Form["__CALLBACKPARAM"].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                if (Request.Form["__CALLBACKPARAM"].Contains("SprintDrop"))
                {
                    UpdateSprinTask(val[1], val[2]);
                    isUpdate = true;
                }
                else if (Request.Form["__CALLBACKPARAM"].Contains("FirstSprintTaskDrop"))
                {
                    object sprintID = gvSprints.GetRowValues(gvSprints.FocusedRowIndex, "ID");
                    if (sprintID != null)
                    {
                        UpdateSprinTask(val[1], Convert.ToString(sprintID));
                        isUpdate = true;
                    }
                }
                else if (Request.Form["__CALLBACKPARAM"].Contains("SprintTaskDrop"))
                {
                    //object sprintID = gvSprintTasks.GetRowValuesByKeyValue(val[2], "SprintId");
                    object sprintID = gvSprints.GetRowValuesByKeyValue(val[2], "ID");
                    if (sprintID != null)
                    {
                        UpdateSprinTask(val[1], Convert.ToString(sprintID));
                        isUpdate = true;
                    }
                }
                else if (Request.Form["__CALLBACKPARAM"].Contains("RemoveSprintTask"))
                {
                    if (!string.IsNullOrEmpty(val[1]))
                    {
                        UpdateSprinTask(val[1], string.Empty);
                        isUpdate = true;
                    }
                }
                else if (Request.Form["__CALLBACKPARAM"].Contains("ProjectReleaseDrop"))
                {
                    UpdateReleaseId(val[1], val[2]);
                    isUpdate = true;
                }
                else if (Request.Form["__CALLBACKPARAM"].Contains("RowReOrder"))
                {
                    if (Request.Form["__CALLBACKPARAM"].Contains("gvProductBacklog"))
                    {
                        if (val[2].Contains(","))
                        {
                            string[] taskIds = val[2].Split(',');
                            for (int i = 0; i < taskIds.Length; i++)
                            {
                                ReOrderGridRows(gvProductBacklog, DatabaseObjects.Tables.SprintTasks, taskIds[i], val[3], DatabaseObjects.Columns.ItemOrder);
                            }
                        }
                        else
                        {
                            ReOrderGridRows(gvProductBacklog, DatabaseObjects.Tables.SprintTasks, val[2], val[3], DatabaseObjects.Columns.ItemOrder);
                        }
                        CreateProductBackLogTable();
                        CreateSprintTable();
                        gvSprints.DataBind();
                        gvProductBacklog.DataBind();
                        gvSprintTasks.DataBind();
                        isUpdate = true;
                    }
                    else if (Request.Form["__CALLBACKPARAM"].Contains("gvSprintTasks"))
                    {
                        ReOrderGridRows(gvSprintTasks, DatabaseObjects.Tables.SprintTasks, val[2], val[3], DatabaseObjects.Columns.SprintOrder);
                        CreateProductBackLogTable();
                        CreateSprintTable();
                        gvSprints.DataBind();
                        gvProductBacklog.DataBind();
                        gvSprintTasks.DataBind();
                        isUpdate = true;
                    }
                    else if (Request.Form["__CALLBACKPARAM"].Contains("gvSprints"))
                    {
                        object selectedData = gvSprints.GetRowValues(gvSprints.FocusedRowIndex, "ID");
                        ReOrderGridRows(gvSprints, DatabaseObjects.Tables.Sprint, val[2], val[3], DatabaseObjects.Columns.ItemOrder);
                        CreateSprintTable();
                        gvSprints.DataBind();
                        gvSprints.FocusedRowIndex = -1;
                        if (!string.IsNullOrEmpty(Convert.ToString(selectedData)))
                        {
                            int index = gvSprints.FindVisibleIndexByKeyValue(selectedData);
                            if (index != -1)
                            {
                                gvSprints.FocusedRowIndex = index;
                            }
                        }
                        isUpdate = true;
                    }
                    else if (Request.Form["__CALLBACKPARAM"].Contains("gvReleases"))
                    {
                        object selectedData = gvReleases.GetRowValues(gvReleases.FocusedRowIndex, "ID");
                        ReOrderGridRows(gvReleases, DatabaseObjects.Tables.ProjectReleases, val[2], val[3], DatabaseObjects.Columns.ItemOrder);
                        CreateProductBackLogTable();
                        CreateReleaseTable();
                        gvProductBacklog.DataBind();
                        gvSprintTasks.DataBind();
                        gvReleases.DataBind();
                        isUpdate = true;
                    }
                }
                else
                {
                    if (dtSprintTasks != null && dtSprintTasks.Rows.Count > 0)
                    {
                        DataRow[] dr = dtSprintTasks.Select(string.Format("{0}={1}", DatabaseObjects.Columns.Id, val[2]));
                        if (dr.Length > 0)
                        {
                            UpdateSprinTask(val[1], Convert.ToString(dr[0]["SprintId"]));
                            isUpdate = true;
                        }
                    }
                }
            }

            RequestUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/ProjectManagement.aspx?control=pmmaddsprinttask");

            RequestUrl = $"{RequestUrl}&PublicTicketID={ticketId}&ModuleName={ModuleName}&ParentIframeId={Iframe}";

            string addNetItem =$"{RequestUrl}&IsNew=true&folderName=Sprints&isTabActive=true";

            lnkAddTask.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Add Task - New Item','500','350',0,'{1}', 'true')", addNetItem, Server.UrlEncode(Request.Url.AbsolutePath)));


            SprintRequestUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/ProjectManagement.aspx?control=pmmaddeditsprint");

            SprintRequestUrl = $"{SprintRequestUrl}&PublicTicketID={ticketId}&ModuleName={ModuleName}&ParentIframeId={Iframe}";

           string  addNetSprint = $"{SprintRequestUrl}&IsNew=true&folderName=Sprints&isTabActive=true";

            lnkAddNewSprint.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Add Task - New Item','500','350',0,'{1}', 'true')", addNetSprint, Server.UrlEncode(Request.Url.AbsolutePath)));


            ReleaseRequestUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/ProjectManagement.aspx?control=pmmaddeditrelease");

            ReleaseRequestUrl = $"{ReleaseRequestUrl}&PublicTicketID={ticketId}&ModuleName={ModuleName}&ParentIframeId={Iframe}";

             addNetItem = $"{ReleaseRequestUrl}&IsNew=true&folderName=Sprints&isTabActive=true";

            lnkAddNewRelease.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Add Task - New Item','500','350',0,'{1}', 'true')", addNetItem, Server.UrlEncode(Request.Url.AbsolutePath)));

            if (!isUpdate)
            {
                CreateProductBackLogTable();
                CreateSprintTable();
                CreateReleaseTable();
            }
            if (!IsPostBack)
            {
                DataRow item = Ticket.GetCurrentTicket(context, "PMM", ticketId);
                SetImageDefaultDuration(item);
                //if (Request.Url.OriginalString.Contains("endtimespan"))
                //{
                if (!string.IsNullOrEmpty(UGITUtility.GetCookieValue(Request, "IsPaneExpanded")))
                {
                    bool IsPaneExpanded = UGITUtility.StringToBoolean(UGITUtility.GetCookieValue(Request, "IsPaneExpanded"));
                    contenSplitter.GetPaneByName("ProductBackLog").Collapsed = !IsPaneExpanded;

                }
                else
                {
                    contenSplitter.GetPaneByName("ProductBackLog").Collapsed = true;
                    contenSplitter.GetPaneByName("ProductBackLog").ShowCollapseBackwardButton = DevExpress.Utils.DefaultBoolean.True;
                }


                if (Request["projectTaskView"] == "1")
                {
                    contenSplitter.GetPaneByName("ProductBackLog").Collapsed = false;
                    contenSplitter.GetPaneByName("ProductBackLog").ShowCollapseBackwardButton = DevExpress.Utils.DefaultBoolean.True;

                    UGITUtility.DeleteCookie(Request, Response, "IsPaneExpanded");
                    UGITUtility.CreateCookie(Response, "IsPaneExpanded", "true");
                }                
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            gvProductBacklog.DataBind();
            gvSprints.DataBind();
            gvSprintTasks.DataBind();
            gvReleases.DataBind();
            if (gvProductBacklog.VisibleRowCount > 0)
            {
                lnkDeleteTask.Enabled = true;
                lnkDeleteTask.CssClass = "pmmScrum_deleteBtn";
            }
            else
            {
                lnkDeleteTask.Enabled = false;
                //lnkDeleteTask.CssClass = "btnDelete disableButton";
                lnkDeleteTask.CssClass = "pmmScrum_deleteBtn";
            }
            if (gvSprints.VisibleRowCount > 0)
            {
                lnkDeleteSprint.Enabled = true;
                lnkDeleteSprint.CssClass = "pmmScrum_deleteBtn";
            }
            else
            {
                lnkDeleteSprint.Enabled = false;
                //lnkDeleteSprint.CssClass = "btnDelete disableButton";
                lnkDeleteSprint.CssClass = "pmmScrum_deleteBtn";
            }
        }
        #endregion

        #region Productbacklog
        protected void gvProductBacklog_DataBinding(object sender, EventArgs e)
        {
            if (dtProductBacklog != null && dtProductBacklog.Rows.Count > 0)
            {
                dtProductBacklog.DefaultView.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.ItemOrder);
                gvProductBacklog.DataSource = dtProductBacklog.DefaultView;               
            }
        }

        private void CreateProductBackLogTable()
        {
            List<ProjectReleases> lstProjectReleases = releaseManager.Load();
            List<Sprint> lstSprint = sprintManager.Load();            
            string query = $"{DatabaseObjects.Columns.TicketPMMIdLookup} = {ID} and {DatabaseObjects.Columns.SprintLookup} is not null";

            DataTable coll = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SprintTasks, $"{query} and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'");

            if (coll != null && coll.Rows.Count > 0)
            {
                if (dtProductBacklog.Columns == null || dtProductBacklog.Columns.Count == 0)
                {
                    dtProductBacklog.Columns.Add(new DataColumn(DatabaseObjects.Columns.Id));
                    dtProductBacklog.Columns.Add(new DataColumn(DatabaseObjects.Columns.Title));
                    dtProductBacklog.Columns.Add(new DataColumn(DatabaseObjects.Columns.ItemOrder, typeof(Int32)));
                    dtProductBacklog.Columns.Add(new DataColumn("SprintTitle"));
                    dtProductBacklog.Columns.Add(new DataColumn("ReleaseTitle"));
                    dtProductBacklog.Columns.Add(new DataColumn(DatabaseObjects.Columns.TaskEstimatedHours, typeof(float)));
                }
                if (dtSprintTasks.Columns == null || dtSprintTasks.Columns.Count == 0)
                {
                    dtSprintTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.Id));
                    dtSprintTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.Title));
                    dtSprintTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.SprintOrder, typeof(Int32)));
                    dtSprintTasks.Columns.Add(new DataColumn("SprintId", typeof(int)));
                    dtSprintTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.PercentComplete));
                    dtSprintTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.Status));
                    dtSprintTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.AssignedTo));
                    dtSprintTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.TaskEstimatedHours, typeof(float)));
                    dtSprintTasks.Columns.Add(new DataColumn("TaskRemainingHours", typeof(float)));
                    dtSprintTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.RemainingHours, typeof(string)));
                    dtSprintTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.ItemOrder, typeof(Int32)));
                }
                foreach (DataRow item in coll.Rows)
                {
                    DataRow dr;
                    string releaseLookup = Convert.ToString(item[DatabaseObjects.Columns.ReleaseLookup]);
                    if (item[DatabaseObjects.Columns.SprintLookup] != null)
                    {
                        string sprintLookup = Convert.ToString(item[DatabaseObjects.Columns.SprintLookup]);

                        CreateSprintTaskTable(item);
                        if (rdProductBackLog.SelectedValue.ToLower() == "all")
                        {
                            dr = dtProductBacklog.NewRow();
                            dr[DatabaseObjects.Columns.Id] = item[DatabaseObjects.Columns.Id];
                            dr[DatabaseObjects.Columns.Title] = item[DatabaseObjects.Columns.Title];
                            dr[DatabaseObjects.Columns.ItemOrder] = item[DatabaseObjects.Columns.ItemOrder];
                            dr[DatabaseObjects.Columns.TaskEstimatedHours] = item[DatabaseObjects.Columns.TaskEstimatedHours1];

                            if (lstSprint != null && lstSprint.Count() > 0)
                                dr["SprintTitle"] = lstSprint.Where(x => x.ID == UGITUtility.StringToLong(sprintLookup)).Select(x => x.Title).FirstOrDefault(); //sprintLookup;
                            else
                                dr["SprintTitle"] = string.Empty;

                            if (lstProjectReleases != null && lstProjectReleases.Count() > 0)
                                dr["ReleaseTitle"] = lstProjectReleases.Where(x => x.ID == UGITUtility.StringToLong(releaseLookup)).Select(x => x.Title).FirstOrDefault();//releaseLookup;
                            else
                                dr["ReleaseTitle"] = string.Empty;

                            dtProductBacklog.Rows.Add(dr);
                            dtProductBacklog.AcceptChanges();
                        }
                    }
                    else
                    {
                        dr = dtProductBacklog.NewRow();
                        dr[DatabaseObjects.Columns.Id] = item[DatabaseObjects.Columns.Id];
                        dr[DatabaseObjects.Columns.Title] = item[DatabaseObjects.Columns.Title];
                        dr[DatabaseObjects.Columns.ItemOrder] = item[DatabaseObjects.Columns.ItemOrder];
                        dr[DatabaseObjects.Columns.TaskEstimatedHours] = item[DatabaseObjects.Columns.TaskEstimatedHours1];

                        if (lstProjectReleases != null && lstProjectReleases.Count() > 0)
                            dr["ReleaseTitle"] = lstProjectReleases.Where(x => x.ID == UGITUtility.StringToLong(releaseLookup)).Select(x => x.Title).FirstOrDefault();//releaseLookup;
                        else
                            dr["ReleaseTitle"] = string.Empty;

                        dtProductBacklog.Rows.Add(dr);
                        dtProductBacklog.AcceptChanges();
                    }
                }
            }
        }
        protected void rdProductBackLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdProductBackLog.SelectedItem.Value.ToLower() == "all")
            {
                UGITUtility.CreateCookie(Response, "ProductBacklogType", "All");
                gvProductBacklog.Columns["SprintTitle"].Visible = true;
            }
            else
            {
                gvProductBacklog.Columns["SprintTitle"].Visible = false;
                UGITUtility.CreateCookie(Response, "ProductBacklogType", "Unallocated");
            }
        }

        protected void btDone_Click(object sender, EventArgs e)
        {
            if (chkSprints != null && chkSprints.SelectedItem != null)
            {
                string sprintID = chkSprints.SelectedItem.Value;
                List<object> lstSelectedTasks = gvProductBacklog.GetSelectedFieldValues(DatabaseObjects.Columns.Id);
                if (lstSelectedTasks != null && lstSelectedTasks.Count > 0 && !string.IsNullOrEmpty(sprintID))
                {
                    foreach (var item in lstSelectedTasks)
                    {
                        UpdateSprinTask(Convert.ToString(item), sprintID);
                    }
                }
            }
        }

        protected void lnkDeleteTask_Click(object sender, EventArgs e)
        {
            List<object> selectedIds = gvProductBacklog.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.Id });
            bool isDelete = false;
            if (selectedIds != null && selectedIds.Count > 0)
            {
                foreach (var item in selectedIds)
                {                    
                    SprintTasks backLogTask = sprintTaskManager.Load(x => x.ID == UGITUtility.StringToInt(item)).FirstOrDefault();
                    if (backLogTask != null)
                    {                 
                        sprintTaskManager.Delete(backLogTask);
                        isDelete = true;
                    }
                }
            }
            if (isDelete)
            {
                ReOrderSprintTask(DatabaseObjects.Columns.ItemOrder);
                ClearTables();
                CreateProductBackLogTable();
                CreateSprintTable();
                gvProductBacklog.DataBind();
                gvSprints.DataBind();
                gvSprintTasks.DataBind();
            }
        }
        #endregion

        #region Sprint
        protected void gvSprints_DataBinding(object sender, EventArgs e)
        {
            if (dtSprints == null || dtSprints.Rows.Count == 0)
            {
                CreateSprintTable();
            }
            if (dtSprints != null && dtSprints.Rows.Count > 0)
            {
                dtSprints.DefaultView.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.ItemOrder);
                gvSprints.DataSource = dtSprints.DefaultView;
            }
        }

        private void CreateSprintTable()
        {            
            List<Sprint> pmmSprintList = sprintManager.Load(x => x.PMMIdLookup == ID);
            if (pmmSprintList != null && pmmSprintList.Count > 0)
            {
                dtSprints = UGITUtility.ToDataTable<Sprint>(pmmSprintList);
                if (!IsPostBack)
                {
                    chkSprints.Items.Clear();
                    chkSprints.DataSource = pmmSprintList;
                    chkSprints.DataTextField = DatabaseObjects.Columns.Title;
                    chkSprints.DataValueField = DatabaseObjects.Columns.Id;
                    chkSprints.DataBind();
                }
            }
        }

        protected void gvSprints_FocusedRowChanged(object sender, EventArgs e)
        {
            int focussedRowKeyValue = UGITUtility.StringToInt(gvSprints.GetRowValues(gvSprints.FocusedRowIndex, DatabaseObjects.Columns.Id));
         
            DataTable coll = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SprintTasks, $"{DatabaseObjects.Columns.TicketPMMIdLookup} = '{ticketId}' and {DatabaseObjects.Columns.SprintLookup} = {focussedRowKeyValue} and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'");  //SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.SprintTasks, spQuery);

            if (coll != null && coll.Rows.Count > 0)
            {
                if (dtSprintTasks.Columns == null || dtSprintTasks.Columns.Count == 0)
                {
                    dtSprintTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.Id));
                    dtSprintTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.Title));
                    dtSprintTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.SprintOrder));
                    dtSprintTasks.Columns.Add(new DataColumn("SprintId"));
                    dtSprintTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.PercentComplete));
                    dtSprintTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.Status));
                    dtSprintTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.AssignedTo));
                    dtSprintTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.TaskEstimatedHours));
                    dtSprintTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.ItemOrder));
                }
                else
                {
                    dtSprintTasks.Rows.Clear();
                }
                foreach (DataRow item in coll.Rows)
                {
                    CreateSprintTaskTable(item);
                }
            }

        }

        protected void lnkAddSprint_Click(object sender, EventArgs e)
        {
            List<Sprint> lstSprints = new List<Sprint>();
            bool isError = false;
            if (dtcStartDate.Date == null)
            {
                lbStartDate.Text = "Please enter start date";
                isError = true;
                lbStartDate.Style.Add("display", "");
            }
            else if (dtcEndDate.Date == null)
            {
                dtcDateError.Text = "Please enter end date";
                isError = true;
                dtcDateError.Style.Add("display", "");
            }
            else if (dtcStartDate.Date > dtcEndDate.Date)
            {
                isError = true;
                dtcDateError.Text = "End Date cannot be before Start Date";
                dtcDateError.Style.Add("display", "");
            }
            else if (string.IsNullOrEmpty(hdnSprintId.Value.Trim()))
            {
                lstSprints = sprintManager.Load(); //GetTableDataManager.GetTableData(DatabaseObjects.Tables.Sprint, $"{DatabaseObjects.Columns.TenantID}= '{context.TenantID}'");  //SPListHelper.GetSPList(DatabaseObjects.Lists.Sprint);
         
                Sprint coll = lstSprints.Where(x => x.PMMIdLookup == ID && x.Title.Equals(txtTitle.Text.Trim(), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (coll != null)
                {
                    lblTitleError.Text = "Sprint with same name already exists.";
                    lblTitleError.Style.Add("display", "");
                    isError = true;
                }
            }
            if (isError)
            {
                return;
            }

            Sprint item = null;
            if (string.IsNullOrEmpty(hdnSprintId.Value.Trim()) && lstSprints != null)
            {   
                item = new Sprint();                             
                int collCount = lstSprints.Where(x => x.PMMIdLookup == ID).Count();
                if (collCount > 0)
                {                    
                    item.ItemOrder = collCount + 1;
                }
                else
                {                    
                    item.ItemOrder = 1;
                }

                DataRow projectItem = Ticket.GetCurrentTicket(context, "PMM", ticketId);
                if (projectItem != null)
                {                 
                    item.PMMIdLookup = Convert.ToInt64(projectItem[DatabaseObjects.Columns.ID]);
                }
                if (collCount == 0)
                {
                    gvSprints.FocusedRowIndex = 0;
                }
            }
            else
            {               
                item = sprintManager.LoadByID(Convert.ToInt64(hdnSprintId.Value.Trim()));
            }
            if (item != null)
            {
                item.Title = txtTitle.Text.Trim();
                item.StartDate = dtcStartDate.Date;
                item.EndDate= dtcEndDate.Date;
                if (item.RemainingHours == null)
                    item.RemainingHours = 0;
                if (item.TaskEstimatedHours == null)
                    item.TaskEstimatedHours = 0;
                if (item.PercentComplete == null)
                    item.PercentComplete = 0;

                if (item.ID > 0)
                    sprintManager.Update(item);
                else
                    sprintManager.Insert(item);

                PopupControl.ShowOnPageLoad = false;
                ClearTables();
                CreateProductBackLogTable();
                CreateSprintTable();
                gvProductBacklog.DataBind();
                gvSprints.DataBind();
                gvSprintTasks.DataBind();
                txtTitle.Text = string.Empty;
                dtcStartDate.Date = DateTime.MinValue;
                dtcEndDate.Date = DateTime.MinValue;
                //////new UGITTaskManager(context).UpdateProjectTask(item);
            }
        }
        protected void lnkDeleteSprint_Click(object sender, EventArgs e)
        {
            List<object> selectedIds = gvSprints.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.ID });
            bool isDelete = false;
            if ((selectedIds == null || selectedIds.Count == 0) && gvSprints.FocusedRowIndex != -1)
            {
                object selectedId = gvSprints.GetRowValues(gvSprints.FocusedRowIndex, new string[] { DatabaseObjects.Columns.Id });
                selectedIds.Add(selectedId);
            }
            if (selectedIds != null && selectedIds.Count > 0)
            {
                foreach (var item in selectedIds)
                {
                    List<SprintTasks> coll = sprintTaskManager.Load(x => x.PMMIdLookup == Convert.ToInt64(ID) && x.SprintLookup == Convert.ToInt64(item));

                    if (coll != null && coll.Count > 0)
                    {
                        foreach (SprintTasks sprintTask in coll)
                        {
                            sprintTask.SprintLookup = null;
                            sprintTask.SprintOrder = 0;
                         
                            sprintTaskManager.Update(sprintTask);
                        }
                    }
                    
                    Sprint backLogTask = sprintManager.LoadByID(UGITUtility.StringToInt(item));

                    if (backLogTask != null)
                    {
                        int itemOrder = UGITUtility.StringToInt(backLogTask.ItemOrder);
                    
                        sprintManager.Delete(backLogTask);

                        isDelete = true;

                        List<Sprint> collSprints = sprintManager.Load($"{DatabaseObjects.Columns.TicketPMMIdLookup} = '{ID}'");

                        foreach (Sprint sprintItem in collSprints)
                        {
                            if (UGITUtility.StringToInt(sprintItem.ItemOrder) > itemOrder)
                            {
                                sprintItem.ItemOrder = UGITUtility.StringToInt(sprintItem.ItemOrder) - 1;                        
                                sprintManager.Update(sprintItem);
                            }
                        }
                    }
                }
            }
            if (isDelete)
            {
                ClearTables();
                CreateProductBackLogTable();
                CreateSprintTable();
                gvProductBacklog.DataBind();
                gvSprints.DataBind();
                gvSprintTasks.DataBind();
                lblSprintTask.Text = string.Empty;
                divSprintTask.Style["padding-bottom"] = "13px";
            }
        }
        protected void imgBtnDuration_Click(object sender, ImageClickEventArgs e)
        {
            SetSprintDurationValues();
            popupSprintDuration.ShowOnPageLoad = true;
        }

        protected void lnkOpenSprintPopUp_Click(object sender, EventArgs e)
        {
            PopupControl.HeaderText = "Add New Sprint";
            txtTitle.Text = string.Empty;
            //dtcEndDate.ClearSelection();
            //dtcStartDate.ClearSelection();
            dtcEndDate.Date = DateTime.MinValue;
            dtcStartDate.Date = DateTime.MinValue;
            hdnSprintId.Value = string.Empty;
            hdnSprintTitle.Value = string.Empty;
            DataRow item = Ticket.GetCurrentTicket(context, "PMM", ticketId);
            if (item != null)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.SprintDuration])) && Convert.ToInt32(item[DatabaseObjects.Columns.SprintDuration]) > 0)
                {
                    noOfWorkingDays = Convert.ToInt32(item[DatabaseObjects.Columns.SprintDuration]);
                }
                else
                {
                    string defaultSprintDuration = configurationVariable.GetValue(ConfigConstants.DefaultSprintDuration);
                    if (!string.IsNullOrEmpty(defaultSprintDuration))
                    {
                        noOfWorkingDays = Convert.ToInt32(configurationVariable.GetValue(ConfigConstants.DefaultSprintDuration));
                    }
                    else
                    {
                        noOfWorkingDays = 10;
                    }
                }
           
                Sprint col1 = sprintManager.Load(x => x.PMMIdLookup == ID).FirstOrDefault();
                DateTime maxDate = sprintManager.Load(x => x.PMMIdLookup == ID).Max(x => x.EndDate) ?? DateTime.MinValue;

                if (col1 != null)
                {           
                    DateTime maxEndDate = Convert.ToDateTime(maxDate);
                    string nextWorkingDate = uHelper.GetNextWorkingDateAndTime(context, maxEndDate);
                    if (!string.IsNullOrEmpty(nextWorkingDate))
                    {
                        string[] nextWorkingDaysDateTime = nextWorkingDate.Split(new string[] { Constants.Separator }, StringSplitOptions.None);
                        if (nextWorkingDaysDateTime != null && nextWorkingDaysDateTime.Length > 1)
                        {
                            dtcStartDate.Date = Convert.ToDateTime(nextWorkingDaysDateTime[1]);
                        }
                    }
                }
                else
                {
                    dtcStartDate.Date = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                }
                if (dtcStartDate.Date != null)
                {
                    DateTime[] calculatedDates = null;
                    calculatedDates = uHelper.GetEndDateByWorkingDays(context, dtcStartDate.Date, noOfWorkingDays);
                    if (calculatedDates != null && calculatedDates.Length > 0)
                    {
                        dtcEndDate.Date = calculatedDates[1];
                        int week = uHelper.GetWeeksFromDays(context, noOfWorkingDays);
                        if (week > 0)
                        {
                            lblSprintDuration.Text = string.Format("{0} week(s)", week);
                        }
                        else
                        {
                            lblSprintDuration.Text = string.Format("{0} day(s)", noOfWorkingDays);
                        }
                    }
                }
            }
            PopupControl.ShowOnPageLoad = true;
            if (contenSplitter.GetPaneByName("ProductBackLog").Collapsed == true)
            {
                contenSplitter.GetPaneByName("ProductBackLog").ShowCollapseBackwardButton = DevExpress.Utils.DefaultBoolean.True;
            }
            else
            {
                contenSplitter.GetPaneByName("ProductBackLog").ShowCollapseForwardButton = DevExpress.Utils.DefaultBoolean.True;
            }
        }

        protected void lnkSaveDuration_Click(object sender, EventArgs e)
        {
            DataRow item = Ticket.GetCurrentTicket(context, "PMM", ticketId);
            if (item != null)
            {
                SaveDuration(item);
            }
        }
        #endregion

        #region Release
        protected void gvReleases_DataBinding(object sender, EventArgs e)
        {
            if (dtReleases == null || dtReleases.Rows.Count == 0)
            {
                CreateReleaseTable();
            }
            if (dtReleases != null && dtReleases.Rows.Count > 0)
            {
                dtReleases.DefaultView.Sort = string.Format("{0} ASC, {1} ASC", DatabaseObjects.Columns.ReleaseDate, DatabaseObjects.Columns.ReleaseID);
                gvReleases.DataSource = dtReleases.DefaultView;
            }
        }

        protected void lnkDeleteReleasePopup_Click(object sender, EventArgs e)
        {

        }

        protected void lnkOpenReleasePopUp_Click(object sender, EventArgs e)
        {
            popUpControlRelease.HeaderText = "Add New Release";
            txtRelease.Text = txtReleaseID.Text = string.Empty;
            //dtcReleaseDate.ClearSelection();
            dtcReleaseDate.Date = DateTime.MinValue;
            hdnReleaseId.Value = string.Empty;
            popUpControlRelease.ShowOnPageLoad = true;
           
        }

        protected void lnkSaveRelease_Click(object sender, EventArgs e)
        {
            List<ProjectReleases> lstReleases = new List<ProjectReleases>();
            bool isError = false;
            if (txtRelease.Text.Trim() == string.Empty)
            {
                lblRelease.Text = "Please enter release title.";
                isError = true;
                lblRelease.Style.Add("display", "");
            }

            if (dtcReleaseDate.Date == null)
            {
                lblReleaseDate.Text = "Please enter release date.";
                isError = true;
                lblReleaseDate.Style.Add("display", "");
            }
            if (txtReleaseID.Text.Trim() == string.Empty)
            {
                lblReleaseID.Text = "Please enter release id.";
                isError = true;
                lblReleaseID.Style.Add("display", "block");
            }
            else if (string.IsNullOrEmpty(hdnReleaseId.Value.Trim()))
            {                
                lstReleases = releaseManager.Load();             
                ProjectReleases coll = lstReleases.Where(x => x.PMMIdLookup == ID && x.ReleaseID.Equals(txtReleaseID.Text.Trim(), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (coll != null)
                {
                    lblReleaseID.Text = "Release with same Id already exists.";
                    lblReleaseID.Style.Add("display", "block");
                    isError = true;
                }
            }
            if (isError)
            {
                return;
            }

            ProjectReleases item = null;
            if (string.IsNullOrEmpty(hdnReleaseId.Value.Trim()) && lstReleases != null)
            {                
                item = new ProjectReleases();             
                int collCount = lstReleases.Where(x => x.PMMIdLookup == ID).Count();

                if (collCount > 0)
                {
                    item.ItemOrder = collCount + 1;
                }
                else
                {
                    item.ItemOrder = 1;
                }

                DataRow projectItem = Ticket.GetCurrentTicket(context, "PMM", ticketId);
                if (projectItem != null)
                {
                    item.PMMIdLookup = Convert.ToInt64(projectItem[DatabaseObjects.Columns.ID]); //new SPFieldLookupValue(projectItem.ID, ticketId);
                }
                if (collCount  == 0)
                {
                    gvReleases.FocusedRowIndex = 0;
                }
            }
            else
            {                
                item = releaseManager.LoadByID(Convert.ToInt64(hdnReleaseId.Value.Trim())); //GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectReleases, $"{DatabaseObjects.Columns.ID} = {Convert.ToInt32(hdnReleaseId.Value.Trim())} and {DatabaseObjects.Columns.TenantID}= '{context.TenantID}'").Select()[0];
            }
            if (item != null)
            {
                item.Title = txtRelease.Text.Trim();
                item.ReleaseID = txtReleaseID.Text.Trim();
                item.ReleaseDate = dtcReleaseDate.Date;
                
                if (item.ID > 0)
                    releaseManager.Update(item);
                else
                    releaseManager.Insert(item);

                popUpControlRelease.ShowOnPageLoad = false;
                ClearTables();
                CreateProductBackLogTable();
                CreateReleaseTable();
                gvReleases.DataBind();
                txtRelease.Text = string.Empty;
                txtReleaseID.Text = string.Empty;
                //dtcReleaseDate.ClearSelection();
                dtcReleaseDate.Date = DateTime.MinValue;
            }

        }

        private void CreateReleaseTable()
        {            
            DataTable collReleases = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectReleases, $"{DatabaseObjects.Columns.TicketPMMIdLookup} = '{ID}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'");

            if (collReleases != null && collReleases.Rows.Count > 0)
            {
                dtReleases = collReleases.Copy();
            }
        }

        protected void lnkDeleteReleases_Click(object sender, EventArgs e)
        {
            bool isDelete = false;
            List<object> selectedIds = gvReleases.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.ID });
            if ((selectedIds == null || selectedIds.Count == 0) && gvReleases.FocusedRowIndex != -1)
            {
                object selectedId = gvReleases.GetRowValues(gvReleases.FocusedRowIndex, new string[] { DatabaseObjects.Columns.ID });
                selectedIds.Add(selectedId);
            }
            if (selectedIds != null && selectedIds.Count > 0)
            {
                foreach (var item in selectedIds)
                {         
                    List<SprintTasks> coll = sprintTaskManager.Load(x => x.PMMIdLookup == UGITUtility.StringToLong(ID) && x.ReleaseLookup == UGITUtility.StringToLong(item));
                    coll.ForEach(x => { x.ReleaseLookup = 0;  });
                    sprintTaskManager.UpdateItems(coll);

                    ProjectReleases spItem = releaseManager.Load(x => x.ID == UGITUtility.StringToLong(item)).FirstOrDefault();

                    if (spItem != null)
                    {
                        int itemOrder = UGITUtility.StringToInt(spItem.ItemOrder);
                        releaseManager.Delete(spItem);

                        isDelete = true;

                        List<ProjectReleases> collRelease = releaseManager.Load(x => x.PMMIdLookup == UGITUtility.StringToLong(ticketId));
                        collRelease.ForEach(x => {
                                                if(UGITUtility.StringToInt(x.ItemOrder) > itemOrder)
                                                {
                                                    x.ItemOrder = x.ItemOrder - 1;
                                                }
                                            });
                        releaseManager.UpdateItems(collRelease);
                    }
                }
            }
            if (isDelete)
            {
                dtReleases.Rows.Clear();
                dtProductBacklog.Rows.Clear();
                dtSprintTasks.Rows.Clear();
                CreateProductBackLogTable();
                gvProductBacklog.DataBind();
                gvSprintTasks.DataBind();
                gvReleases.DataBind();
            }
        }

        private void EditRelease(int id, string releaseTitle)
        {
            if (id > 0)
            {                
                DataRow spItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectReleases, $"{DatabaseObjects.Columns.ID} = {id} and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select()[0];

                if (spItem != null)
                {
                    txtRelease.Text = Convert.ToString(spItem[DatabaseObjects.Columns.Title]);
                    dtcReleaseDate.Date = (Convert.ToDateTime(spItem[DatabaseObjects.Columns.ReleaseDate]));
                    txtReleaseID.Text = (Convert.ToString(spItem[DatabaseObjects.Columns.ReleaseID]));
                }
            }
            popUpControlRelease.HeaderText = "Edit Release- " + releaseTitle;
            popUpControlRelease.ShowOnPageLoad = true;
        }
        #endregion

        #region Sprinttask
        protected void gvSprintTasks_DataBinding(object sender, EventArgs e)
        {
            string sprintChartView = configurationVariable.GetValue(ConfigConstants.SprintChartView); //uGITCache.GetConfigVariableValue(ConfigConstants.SprintChartView);

            List<string> viewParams = new List<string>();
            object selectedData = 0;
            object selectedtitle = "";
            if (Request["viewType"] == "1" && Request["sprintTitle"] != null)
            {
                selectedtitle = Request["sprintTitle"].Trim();
                
                Sprint sprintItemCol = sprintManager.Load($"{DatabaseObjects.Columns.TicketPMMIdLookup} = '{ticketId}' and {DatabaseObjects.Columns.Title} = '{selectedtitle}'").FirstOrDefault();
                if (sprintItemCol != null)
                    selectedData = sprintItemCol.ID;
            }
            else
            {
                int sprintID = 0; // defaults to 0 if cookie not set
                int.TryParse(UGITUtility.GetCookieValue(Request, "SelectedVisibleIndexSprintId"), out sprintID);
                selectedData = gvSprints.GetRowValues(sprintID, "ID");
                selectedtitle = gvSprints.GetRowValues(sprintID, "Title");
            }

            if (!string.IsNullOrEmpty(Convert.ToString(selectedData)))
            {
                int sprintId = 0;
                int.TryParse(Convert.ToString(selectedData), out sprintId);
                viewParams.Add(string.Format("view={0}", sprintChartView));
                viewParams.Add(string.Format("externalfilter={0}", Uri.EscapeDataString(string.Format("SprintLookupId='{0}'", sprintId))));
                sprintChart.Attributes.Add("onClick", string.Format("window.parent.UgitOpenPopupDialog(\"{0}\", \"{1}\", 'Sprint Burndown Chart', '{2}', '{3}', 0)",
                                                                    UGITUtility.GetAbsoluteURL(DelegateControlsUrl.ShowDashboardView), string.Join("&", viewParams.ToArray()), 45, 90));

                if (dtSprintTasks != null && dtSprintTasks.Rows.Count > 0)
                {
                    DataRow[] dr = dtSprintTasks.Select(string.Format("SprintId={0}", sprintId));
                    if (dr.Length > 0)
                    {
                        dtSprintTasks = dr.CopyToDataTable();
                        dtSprintTasks.DefaultView.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.SprintOrder);
                    }
                    else
                    {
                        dtSprintTasks.Rows.Clear();
                    }
                }
                if (dtSprints != null && dtSprints.Rows.Count > 0)
                {
                    DataRow[] drSprint = dtSprints.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Id, sprintId));
                    if (!string.IsNullOrEmpty(Convert.ToString(drSprint[0][DatabaseObjects.Columns.RemainingHours])) && !string.IsNullOrEmpty(Convert.ToString(drSprint[0][DatabaseObjects.Columns.TaskEstimatedHours1])) && !string.IsNullOrEmpty(Convert.ToString(drSprint[0][DatabaseObjects.Columns.PercentComplete])))
                    {
                        ShowSprintHours(Convert.ToDouble(drSprint[0][DatabaseObjects.Columns.RemainingHours]), Convert.ToDouble(drSprint[0][DatabaseObjects.Columns.TaskEstimatedHours1]), Convert.ToDouble(drSprint[0][DatabaseObjects.Columns.PercentComplete]), sprintId);
                    }
                    else
                    {
                        ShowSprintHours(0, 0, 0, sprintId);
                    }
                }
                else
                {
                    ShowSprintHours(0, 0, 0, sprintId);
                }
                gvSprintTasks.DataSource = dtSprintTasks.DefaultView;

            }
        }

        private void CreateSprintTaskTable(DataRow item)
        {            
            if (item[DatabaseObjects.Columns.SprintLookup] == DBNull.Value)
                return;
            
            List<string> assignees = new List<string>();

            assignees.Clear();
            assignees = UGITUtility.ConvertStringToList(Convert.ToString(item[DatabaseObjects.Columns.AssignedTo]), ",");

            string assignedTo = UserManager.GetUserOrGroupName(assignees);
            int sprintLookup = Convert.ToInt32(item[DatabaseObjects.Columns.SprintLookup]);

            DataRow dr = dtSprintTasks.NewRow();
            dr[DatabaseObjects.Columns.PercentComplete] = string.Format("{0}%", Convert.ToDouble(item[DatabaseObjects.Columns.PercentComplete]));
            dr[DatabaseObjects.Columns.Status] = item[DatabaseObjects.Columns.TaskStatus];
            dr[DatabaseObjects.Columns.AssignedTo] = string.IsNullOrEmpty(assignedTo) ? assignedTo : string.Empty; //(assignedTo != null && assignedTo.Count > 0) ? string.Join("; ", assignedTo.Select(x => x.LookupValue).ToArray()) : string.Empty;
            dr[DatabaseObjects.Columns.TaskEstimatedHours] = item[DatabaseObjects.Columns.TaskEstimatedHours1];
            dr[DatabaseObjects.Columns.Id] = item[DatabaseObjects.Columns.Id];
            dr[DatabaseObjects.Columns.Title] = item[DatabaseObjects.Columns.Title];
            dr[DatabaseObjects.Columns.SprintOrder] = item[DatabaseObjects.Columns.SprintOrder];
            dr[DatabaseObjects.Columns.ItemOrder] = UGITUtility.StringToInt(item[DatabaseObjects.Columns.ItemOrder]);
            dr["SprintId"] = sprintLookup > 0 ? sprintLookup : 0; //(sprintLookup != null && sprintLookup.LookupId > 0) ? sprintLookup.LookupId : 0;
            if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.PercentComplete])) && !string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.TaskEstimatedHours1])))
            {
                Double remaininghrs = (1 - (Convert.ToDouble(item[DatabaseObjects.Columns.PercentComplete]) / 100)) * (Convert.ToDouble(item[DatabaseObjects.Columns.TaskEstimatedHours1]));
                remaininghrs = Math.Round(remaininghrs, 2);
                dr["TaskRemainingHours"] = remaininghrs;
                dr[DatabaseObjects.Columns.RemainingHours] = string.Format("{0} / {1}", remaininghrs, item[DatabaseObjects.Columns.TaskEstimatedHours1]);
            }
            dtSprintTasks.Rows.Add(dr);
            dtSprintTasks.AcceptChanges();
            dtSprintTasks.DefaultView.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.SprintOrder);
        }

        protected void lnkMoveToBackLog_Click(object sender, EventArgs e)
        {
            List<object> selectedIds = gvSprintTasks.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.Id });

            bool isDelete = false;

            DataTable dtnew = new DataTable();
            DataRow sprintTask;

            if (selectedIds != null && selectedIds.Count > 0)
            {
                foreach (var item in selectedIds)
                {         
                    sprintTask = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SprintTasks, $"{DatabaseObjects.Columns.ID} = {Convert.ToInt32(item)} and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select()[0];
                    int sprintLookup = Convert.ToInt32(sprintTask[DatabaseObjects.Columns.SprintLookup]);
                    sprintTask[DatabaseObjects.Columns.SprintLookup] = null;
                    sprintTask[DatabaseObjects.Columns.SprintOrder] = 0;
                    sprintTask.AcceptChanges();                    
                 
                    isDelete = true;

                    DataRow[] newResult = dtSprintTasks.Select(string.Format("Id <> '{0}'", item));
                    if (newResult != null && newResult.Length > 0)
                        dtnew = newResult.CopyToDataTable();

                    UpdateRemainingHoursSprint(dtnew, sprintLookup);
                }
            }
            if (isDelete)
            {
                ClearTables();
                CreateProductBackLogTable();
                CreateSprintTable();
                gvProductBacklog.DataBind();
                gvSprints.DataBind();
                gvSprintTasks.DataBind();
                lnkMoveToBackLog.Attributes.Add("disabled", "disabled");
                lnkMoveToBackLog.CssClass = "disableButton";
            }
            gvSprintTasks.Selection.UnselectAll();
        }

        protected void gvSprintTasks_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {
            if (e.Column.FieldName == DatabaseObjects.Columns.AssignedTo)
            {
                List<FilterValue> temp = new List<FilterValue>(); // List of filter value objects
                List<string> values = new List<string>(); // List in string format used to keep out duplicates
                foreach (FilterValue fValue in e.Values)
                {
                    if (fValue.Value.Contains(";"))
                    {
                        // Found multiple semi-colon separated values
                        string[] vals = fValue.Value.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string val in vals)
                        {
                            // Add to filter list only if not already in it
                            string trimmedVal = val.Trim();
                            if (!string.IsNullOrEmpty(trimmedVal) && !values.Contains(trimmedVal))
                            {
                                temp.Add(new FilterValue(trimmedVal, trimmedVal, string.Format("[{0}] LIKE '%{1}%'", e.Column.FieldName, trimmedVal)));
                                values.Add(trimmedVal);
                            }
                        }
                    }
                    else
                    {
                        // Single value, add to list if not already in it
                        string trimmedVal = fValue.Value.Trim();
                        if (!string.IsNullOrEmpty(trimmedVal) && !values.Contains(trimmedVal))
                        {
                            temp.Add(new FilterValue(trimmedVal, trimmedVal, string.Format("[{0}] LIKE '%{1}%'", e.Column.FieldName, trimmedVal)));
                            values.Add(trimmedVal);
                        }
                    }
                }

                // Add to filter list in order
                temp = temp.OrderBy(o => o.Value).ToList();
                if (e.Values.Count > 3)
                {
                    //  e.Values.RemoveRange(3, e.Values.Count - 3);
                }
                foreach (FilterValue fVal in temp)
                {
                    e.Values.Add(fVal);
                }
            }
        }
        #endregion

        #region Funtions

        private void UpdateReleaseId(string taskId, string releaseId)
        {            
            SprintTasks coll = sprintTaskManager.Load(x => x.ID == Convert.ToInt64(taskId)).FirstOrDefault();

            if (coll != null)
            {         
                if (!string.IsNullOrEmpty(releaseId))
                {             
                    coll.ReleaseLookup = UGITUtility.StringToLong(releaseId);
                }
                else
                {                    
                }
                                
                sprintTaskManager.Update(coll);

                ClearTables();
                CreateProductBackLogTable();
                CreateReleaseTable();
                gvProductBacklog.DataBind();
                gvSprintTasks.DataBind();
                gvReleases.DataBind();
            }
        }

        private void EditSprint(int id, string sprintTitle)
        {
            if (id > 0)
            {                
                Sprint spItem = sprintManager.Load(x => x.ID == UGITUtility.StringToLong(id)).FirstOrDefault();

                if (spItem != null)
                {
                    txtTitle.Text = Convert.ToString(spItem.Title);
                    dtcStartDate.Date = (Convert.ToDateTime(spItem.StartDate));
                    dtcEndDate.Date = (Convert.ToDateTime(spItem.EndDate));
                    DataRow item = Ticket.GetCurrentTicket(context, "PMM", ticketId);
                    if (item != null)
                    {

                        noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, dtcStartDate.Date, dtcEndDate.Date);
                        int week = uHelper.GetWeeksFromDays(context, noOfWorkingDays);
                        if (week > 0)
                        {
                            lblSprintDuration.Text = string.Format("{0} week(s)", week);
                        }
                        else
                        {
                            lblSprintDuration.Text = string.Format("{0} day(s)", noOfWorkingDays);
                        }
                    }
                }
            }
            PopupControl.HeaderText = "Edit Sprint- " + sprintTitle;
            PopupControl.ShowOnPageLoad = true;

        }


        private void UpdateSprinTask(string taskId, string sprintID)
        {            
            SprintTasks item = sprintTaskManager.Load(x => x.ID == Convert.ToInt64(taskId)).FirstOrDefault();

            if (item != null)
            {         
                if (!string.IsNullOrEmpty(sprintID))
                {
                    item.SprintLookup = UGITUtility.StringToLong(sprintID);
             
                    int collSprintsCount = sprintTaskManager.Load(x => x.PMMIdLookup == UGITUtility.StringToLong(ticketId) && x.SprintLookup == UGITUtility.StringToLong(sprintID)).Count();
                    if (collSprintsCount > 0)
                    {
                        item.SprintOrder = collSprintsCount + 1;
                    }
                    else
                    {
                        item.SprintOrder = 1;
                    }
                }
                else
                {
                    long lookup = item.SprintLookup ?? 0;
                    if (lookup > 0)
                    {
                        sprintID = Convert.ToString(lookup);                        
                        List<SprintTasks> collSprints = sprintTaskManager.Load(x => x.PMMIdLookup == UGITUtility.StringToLong(ticketId) && x.SprintLookup == lookup);

                        if (collSprints != null && collSprints.Count > 0)
                        {
                            foreach (SprintTasks itemTask in collSprints)
                            {
                                if (UGITUtility.StringToInt(itemTask.SprintOrder) > UGITUtility.StringToInt(item.SprintOrder))
                                {
                                    itemTask.SprintOrder = UGITUtility.StringToInt(itemTask.SprintOrder) - 1;                                    
                                    sprintTaskManager.Update(itemTask);
                                }
                            }
                        }
                    }
                    item.SprintOrder = 0;
                    item.SprintLookup = null;
                }
                                
                sprintTaskManager.Update(item);
                ClearTables();
                CreateProductBackLogTable();
                UpdateRemainingHoursSprint(dtSprintTasks, Convert.ToInt32(sprintID));
                CreateSprintTable();
                gvProductBacklog.DataBind();
                gvSprints.DataBind();
                gvSprintTasks.DataBind();

            }
        }

        private void ClearTables()
        {
            dtProductBacklog.Rows.Clear();
            dtSprints.Rows.Clear();
            dtSprintTasks.Rows.Clear();
            dtReleases.Rows.Clear();
        }

        protected void btnHidden_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnSprintId.Value.Trim()) && !string.IsNullOrEmpty(hdnSprintTitle.Value.Trim()) && !string.IsNullOrEmpty(hdnSource.Value.Trim()) && hdnSource.Value.Trim().ToLower() == "sprint")
            {
                EditSprint(Convert.ToInt32(hdnSprintId.Value.Trim()), hdnSprintTitle.Value.Trim());
            }
            else
            {
                EditRelease(Convert.ToInt32(hdnReleaseId.Value.Trim()), hdnReleaseTitle.Value.Trim());
            }
        }

        private void SetSprintDurationValues()
        {
            DataRow item = Ticket.GetCurrentTicket(context, "PMM", ticketId);
            if (item != null)
            {
                int weeks = 0;
                if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.SprintDuration])) && Convert.ToInt32(item[DatabaseObjects.Columns.SprintDuration]) > 0)
                {
                    weeks = uHelper.GetWeeksFromDays(context, Convert.ToInt32(item[DatabaseObjects.Columns.SprintDuration]));
                    txtduration.Text = Convert.ToString(item[DatabaseObjects.Columns.SprintDuration]);
                }
                else
                {
                    string defaultSprintDuration = configurationVariable.GetValue(ConfigConstants.DefaultSprintDuration);
                    if (string.IsNullOrEmpty(defaultSprintDuration))
                    {
                        defaultSprintDuration = "10";
                    }
                    weeks = uHelper.GetWeeksFromDays(context, Convert.ToInt32(defaultSprintDuration));
                    txtduration.Text = defaultSprintDuration;
                }
                if (weeks != 0)
                {
                    txtduration.Text = Convert.ToString(weeks);
                    ddlDuration.SelectedIndex = ddlDuration.Items.IndexOf(ddlDuration.Items.FindByValue("weeks"));
                }
            }

        }

        private void SaveDuration(DataRow item)
        {
            if (!string.IsNullOrEmpty(txtduration.Text))
            {
                int noOfDays = Convert.ToInt32(txtduration.Text.Trim());
                if (ddlDuration.SelectedValue == "weeks")
                {
                    noOfDays = uHelper.GetWorkingDaysInWeeks(context, Convert.ToInt32(txtduration.Text.Trim()));
                }
                item[DatabaseObjects.Columns.SprintDuration] = noOfDays;
            }
            else
            {
                item[DatabaseObjects.Columns.SprintDuration] = string.Empty;
            }
            //item.Update();
            tkt.CommitChanges(item);
            SetImageDefaultDuration(item);
            popupSprintDuration.ShowOnPageLoad = false;

        }

        private void SetImageDefaultDuration(DataRow item)
        {
            int daysCount = 0;
            if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.SprintDuration])) && Convert.ToInt32(item[DatabaseObjects.Columns.SprintDuration]) > 0)
            {
                daysCount = Convert.ToInt32(item[DatabaseObjects.Columns.SprintDuration]);
            }
            else
            {
                string defaultSprintDuration = configurationVariable.GetValue(ConfigConstants.DefaultSprintDuration);
                if (!string.IsNullOrEmpty(defaultSprintDuration))
                {
                    daysCount = Convert.ToInt32(configurationVariable.GetValue(ConfigConstants.DefaultSprintDuration));
                }
                else
                {
                    // hardCode duration value when config value is not set and Sprint Duration is not set for PMM.
                    daysCount = 10;
                }
            }
            int weeks = 0;
            if (daysCount > 0)
            {
                weeks = uHelper.GetWeeksFromDays(context, Convert.ToInt32(daysCount));
            }
            if (weeks > 0)
            {
                imgBtnDuration.ToolTip = string.Format("{0} week(s)", weeks);
            }
            else
            {
                imgBtnDuration.ToolTip = string.Format("{0} days(s)", daysCount);
            }
        }

        private void UpdateRemainingHoursSprint(DataTable dtSprintTasks, int sprintId)
        {
            DataTable dtSelectedTasks = new DataTable();
            if (dtSprintTasks != null && dtSprintTasks.Rows.Count > 0)
            {
                DataRow[] dr = dtSprintTasks.Select(string.Format("SprintId={0}", sprintId));
                if (dr.Length > 0)
                {
                    dtSelectedTasks = dr.CopyToDataTable();
                }
            }            
            Sprint selectedSprint = sprintManager.LoadByID(Convert.ToInt32(sprintId));

            double estimatedHours = 0;
            double remainingHours = 0;
            double pctComplete = 0;
            if (dtSelectedTasks != null && dtSelectedTasks.Rows.Count > 0)
            {
                double sprintRemainingHours = Convert.ToDouble(dtSelectedTasks.Compute(string.Format("SUM({0})", "TaskRemainingHours"), string.Empty));
                double sprintEstimatedHours = Convert.ToDouble(dtSelectedTasks.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.TaskEstimatedHours), string.Empty));
                remainingHours = Math.Round(sprintRemainingHours, 2);
                estimatedHours = Math.Round(sprintEstimatedHours, 2);
                if (estimatedHours > 0)
                    pctComplete = Math.Round((((estimatedHours - remainingHours) / estimatedHours) * 100), 2);
                else
                    pctComplete = 0;
            }
            selectedSprint.RemainingHours = Convert.ToInt32(remainingHours);
            selectedSprint.TaskEstimatedHours = Convert.ToInt32(estimatedHours);
            if (pctComplete > 0)
                selectedSprint.PercentComplete = Convert.ToInt32(pctComplete / 100);
            else
                selectedSprint.PercentComplete = 0;
                        
            sprintManager.Update(selectedSprint);
                        
            new UGITTaskManager(context).UpdateProjectTask(UGITUtility.ObjectToData(selectedSprint).Select()[0]);
        }

        private void ShowSprintHours(Double remainingHours, Double estimatedHours, Double pctComplete, int sprintId)
        {            
            Sprint selectedSprint = sprintManager.LoadByID(Convert.ToInt32(sprintId));

            if (selectedSprint != null)
            {
                string sprintTaskTitle = string.Format("{0}: {1} to {2}", Convert.ToString(selectedSprint.Title),
                                                      Convert.ToDateTime(selectedSprint.StartDate).ToString("MMM-dd-yyyy"),
                                                      Convert.ToDateTime(selectedSprint.EndDate).ToString("MMM-dd-yyyy"));

                lblSprintTask.Text = sprintTaskTitle;
                divSprintTask.Style["padding-bottom"] = "0px";
                if (dtSprintTasks != null && dtSprintTasks.Rows.Count > 0)
                {
                    // Don't show rounded up to 100% unless complete!
                    if (pctComplete > 99.9 && pctComplete < 100)
                        pctComplete = 99.9;

                    lblTotalHours.Text = string.Format(" ({0:0.0}% Completed, Remaining: {1} / {2} hrs)", pctComplete, remainingHours, estimatedHours);
                    int workingHrsInDay = uHelper.GetWorkingHoursInADay(context);
                    int workingDays = uHelper.GetTotalWorkingDaysBetween(context, Convert.ToDateTime(Convert.ToString(selectedSprint.StartDate)), Convert.ToDateTime(Convert.ToString(selectedSprint.EndDate)));
                    int totalHours = workingHrsInDay * workingDays;
                    if (estimatedHours > totalHours)
                    {
                        lblTotalHours.Style.Add("color", "red");
                    }
                    else
                    {
                        lblTotalHours.Style.Add("color", "green");
                    }
                }
                else
                {
                    lblTotalHours.Text = "";
                    lblTotalHours.Style.Add("color", "black");
                }
            }
        }
        #endregion

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        private void ReOrderGridRows(DevExpress.Web.ASPxGridView grid, string listName, string sourceRowId, string targetRowId, string colName)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            string query = string.Empty;

            if (colName == DatabaseObjects.Columns.SprintOrder && listName == DatabaseObjects.Tables.SprintTasks)
            {
                object selectedSprintId = 0;
                selectedSprintId = gvSprints.GetRowValues(gvSprints.FocusedRowIndex, "ID");                
                query = $"{DatabaseObjects.Columns.SprintLookup} = {selectedSprintId}";
            }
            else
            {                
                query = $"{DatabaseObjects.Columns.TicketPMMIdLookup} = '{ID}'";
            }

            DataTable splstCol = GetTableDataManager.GetTableData(listName, $"{query} and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'");

            object sourceData = grid.GetRowValuesByKeyValue(sourceRowId, colName);
            object targetData = grid.GetRowValuesByKeyValue(targetRowId, colName);

            if (sourceData != null && splstCol != null && splstCol.Rows.Count > 0 && targetData != null)
            {
                if (UGITUtility.StringToInt(sourceData) < UGITUtility.StringToInt(targetData))
                {
                    foreach (DataRow item in splstCol.Rows)
                    {
                        values.Clear();
                        if (UGITUtility.StringToInt(item[DatabaseObjects.Columns.Id]) == UGITUtility.StringToInt(sourceRowId))
                        {
                            item[colName] = targetData;
                        }
                        //else if(UGITUtility.StringToInt(item[DatabaseObjects.Columns.Id]) == UGITUtility.StringToInt(targetRowId))
                        //{
                        //    item[colName] = sourceData;
                        //}
                        else if (UGITUtility.StringToInt(item[colName]) <= UGITUtility.StringToInt(targetData) && UGITUtility.StringToInt(item[colName]) >= UGITUtility.StringToInt(sourceData))
                        {
                            item[colName] = UGITUtility.StringToInt(item[colName]) - 1;
                        }

                        values.Add(colName, item[colName]);
                        //tkt.CommitChanges(item);
                        GetTableDataManager.UpdateItem<int>(listName, UGITUtility.StringToLong(item[DatabaseObjects.Columns.ID]), values);
                    }
                }
                else if (UGITUtility.StringToInt(sourceData) > UGITUtility.StringToInt(targetData))
                {
                    foreach (DataRow item in splstCol.Rows)
                    {
                        values.Clear();
                        if (UGITUtility.StringToInt(item[DatabaseObjects.Columns.Id]) == UGITUtility.StringToInt(sourceRowId))
                        {
                            item[colName] = targetData;
                        }
                        else if (UGITUtility.StringToInt(item[colName]) <= UGITUtility.StringToInt(sourceData) && UGITUtility.StringToInt(item[colName]) >= UGITUtility.StringToInt(targetData))
                        {
                            item[colName] = UGITUtility.StringToInt(item[colName]) + 1;
                        }

                        values.Add(colName, item[colName]);
                        //tkt.CommitChanges(item);
                        GetTableDataManager.UpdateItem<int>(listName, UGITUtility.StringToLong(item[DatabaseObjects.Columns.ID]), values);
                    }
                }
            }
                        
            //DataTable newsplstCol = GetTableDataManager.GetTableData(listName, $"{query} and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            //int count = 0;
            //foreach (DataRow item in newsplstCol.Rows)
            //{
            //    values.Clear();
            //    item[colName] = ++count;
            //    values.Add(colName, item[colName]);
            //    GetTableDataManager.UpdateItem<int>(listName, UGITUtility.StringToLong(item[DatabaseObjects.Columns.ID]), values);
            //}

        }

        //new method for reorder while add and update sprint task...
        private void ReOrderSprintTask(string colName)
        {            
            List<SprintTasks> newsplstCol = new List<SprintTasks>();
            newsplstCol = sprintTaskManager.Load($"{DatabaseObjects.Columns.TicketPMMIdLookup} = {ID}").ToList();

            int count = 0;
            foreach (SprintTasks item in newsplstCol)
            {
                if(colName == DatabaseObjects.Columns.ItemOrder)
                    item.ItemOrder = ++count;
                else if(colName == DatabaseObjects.Columns.SprintOrder)
                    item.SprintOrder = ++count;

                sprintTaskManager.Update(item);
            }            
        }

        protected void lnkSaveDuration_Click1(object sender, EventArgs e)
        {
            DataRow item = Ticket.GetCurrentTicket(context, "PMM", ticketId);
            if (item != null)
            {
                SaveDuration(item);
            }
        }
    }
}