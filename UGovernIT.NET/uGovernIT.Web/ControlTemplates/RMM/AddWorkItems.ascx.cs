using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using System.Linq;
using System.Threading;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Manager;
using static uGovernIT.Manager.AllocationTypeManager;
using uGovernIT.Manager.RMM.ViewModel;
using uGovernIT.Manager.Managers;
using System.Drawing;
using uGovernIT.Util.Log;
using static uGovernIT.Web.ModuleResourceAddEdit;
using uGovernIT.DAL.Store;

namespace uGovernIT.Web
{
    public partial class AddWorkItems : UserControl
    {
        public List<UserProfile> SelectedUsersList { get; set; }
        public string SelectedUsersListString { get; set; }
        public string Type { get; set; }
        public string WorkItemType { get; set; }
        public string WorkItem { get; set; }
        public bool IsInPopupCallback { get; set; }

        public string preconStart { get; set; }
        public string preconEnd { get; set; }
        public string constStart { get; set; }
        public string constEnd { get; set; }
        public string closeout { get; set; }

        public string SearchType { get; set; }

        public DateTime StartDate { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        UserProfileManager objUserManager = null;
        public string AllocationID = "";
        public bool RefreshPage = true;
        ResourceWorkItemsManager ObjWorkItemsManager = null;
        ResourceAllocationManager objAllocationManager = null;
        ProjectEstimatedAllocationManager estimatedAllocationManager = null;
        ProjectEstimatedAllocationManager ObjEstimatedAllocationManager = null;
        ModuleViewManager ModuleManager = null;
        ConfigurationVariableManager configVariableManager = null;
        DataTable resultedTableLevel2 = null;
        private bool fromModule = false;
        protected string ajaxHelper = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/ajaxhelper.aspx");
        bool enableProjStdWorkItems = false;
        bool allowPTOEdit = false;
        string level4Name = string.Empty;
        public bool useGanttDayFormat = true;
        protected override void OnInit(EventArgs e)
        {
            objUserManager = new UserProfileManager(context);
            ObjWorkItemsManager = new ResourceWorkItemsManager(context);
            objAllocationManager = new ResourceAllocationManager(context);
            ModuleManager = new ModuleViewManager(context);
            estimatedAllocationManager = new ProjectEstimatedAllocationManager(context);
            ObjEstimatedAllocationManager = new ProjectEstimatedAllocationManager(context);
            configVariableManager = new ConfigurationVariableManager(context);
            useGanttDayFormat = uHelper.GetGanttFormat(context) == GanttFormat.Days ? true : false;

            AllocationID = Request["allocId"];
            SearchType = Request["searchtype"];
            RefreshPage = Request["refreshpage"] != null && Request["refreshpage"] == "0" ? false : true;
            subSubitem.Visible = false;
            enableProjStdWorkItems = configVariableManager.GetValueAsBool(ConfigConstants.EnableProjStdWorkItems);
            allowPTOEdit = configVariableManager.GetValueAsBool(ConfigConstants.AllowPTOEdit);
            level4Name = configVariableManager.GetValue("RMMLevel4Name");

            RResourceAllocation rAllocation = null;
            if (!string.IsNullOrWhiteSpace(SearchType))
            {
                List<RResourceAllocation> data = objAllocationManager.Load();
                rAllocation = data.Where(o => o.ResourceWorkItemLookup == UGITUtility.StringToLong(AllocationID)).FirstOrDefault();
                AllocationID = rAllocation != null ? rAllocation.ID.ToString() : String.Empty;
            }

            if (Type == "ResourceAllocation")
            {
                if (!string.IsNullOrEmpty(AllocationID))
                {
                    FillAllocation();
                    ddlLevel1.Enabled = false;
                    cbLevel2.Enabled = false;
                    //cbLevel3.Enabled = false;
                    ddlLevel1.ForeColor = Color.LightGray;
                    cbLevel2.ForeColor = Color.LightGray;
                }
                trAllocation.Visible = true;
                trStartDate.Visible = true;
                trEndDate.Visible = true;

            }
            else if (Type == "Actuals")
            {
                if (!string.IsNullOrEmpty(AllocationID))
                {

                }
                trAllocation.Visible = false;
                trStartDate.Visible = false;
                trEndDate.Visible = false;
            }
            if (!String.IsNullOrEmpty(SelectedUsersListString))
            {
                SelectedUsersList = new List<UserProfile>();
                SelectedUsersList = objUserManager.GetUserInfosById(SelectedUsersListString);
            }
            if (WorkItemType == "Time Off" && !allowPTOEdit)
            {
                txtAllocation.Enabled = false;
                rdbSoftAllocation.Enabled = false;
                startDate.Enabled = false;
                endDate.Enabled = false;
                btnSave.Visible = false;
                btnCancel.Visible = false;
            }
            base.OnInit(e);
        }

        protected void FillDropDownLevel1(object sender, EventArgs e)
        {
            DropDownList level1 = (DropDownList)sender;
            if (level1.Items.Count <= 0)
            {
                DataTable resultedTable = AllocationTypeManager.LoadLevel1(context);
                if (resultedTable != null)
                {
                    level1.Items.Add(new ListItem("--Select--", "--Select--"));
                    foreach (DataRow row in resultedTable.Rows)
                    {
                        if (row["LevelTitle"] != null && row["LevelTitle"].ToString() != string.Empty)
                        {
                            level1.Items.Add(new ListItem(row["LevelTitle"].ToString(), row["LevelName"].ToString()));
                        }
                    }
                }
            }
            RResourceAllocation rAllocation = objAllocationManager.LoadByID(Convert.ToInt64(AllocationID));
            if (rAllocation != null)
            {
                rAllocation.ResourceWorkItems = rAllocation.ResourceWorkItems = ObjWorkItemsManager.LoadByID(rAllocation.ResourceWorkItemLookup);
                var item = ddlLevel1.Items.FindByText(rAllocation.ResourceWorkItems.WorkItemType);
                if (item != null)
                    item.Selected = true;
            }

            if (!string.IsNullOrEmpty(WorkItemType) && !IsPostBack)
            {
                var item = ddlLevel1.Items.FindByText(WorkItemType);
                if (item != null)
                {
                    ddlLevel1.SelectedIndex = ddlLevel1.Items.IndexOf(item);
                    //level1.Enabled = false;
                }
                else
                {
                    item = ddlLevel1.Items.FindByValue(WorkItemType);
                    if(item != null)
                    {
                        ddlLevel1.SelectedIndex = ddlLevel1.Items.IndexOf(item);
                        //level1.Enabled = false;
                    }
                }
            }
        }

        protected void cbLevel2_Load(object sender, EventArgs e)
        {
            if (ddlLevel1.SelectedItem != null && ddlLevel1.SelectedItem.Text != "--Select--")
            {
                DataRow[] drModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, ddlLevel1.SelectedItem.Text.Trim()));
                fromModule = drModules != null && drModules.Length > 0;
                if(fromModule)
                {
                    string moduleName = UGITUtility.ObjectToString(drModules[0][DatabaseObjects.Columns.ModuleName]);
                    if (moduleName == ModuleNames.OPM)
                        rdbSoftAllocation.Checked = true;
                    else
                        rdbSoftAllocation.Checked = false;
                }
                resultedTableLevel2 = AllocationTypeManager.LoadLevel2(context, ddlLevel1.SelectedItem.Text, fromModule);
                if (resultedTableLevel2 != null)
                {
                    bool ShowERPJobID = configVariableManager.GetValueAsBool(ConfigConstants.ShowERPJobID);
                    string ERPJobIDName = configVariableManager.GetValue(ConfigConstants.ERPJobIDName);
                    if (fromModule)
                    {
                        cbLevel2.Columns.Clear();
                        cbLevel2.Columns.Add("LevelTitle");
                        cbLevel2.Columns[0].Caption = "Item ID";
                        cbLevel2.Columns[0].Width = new Unit("95px");

                        if (ShowERPJobID)
                        {
                            cbLevel2.Columns.Add(DatabaseObjects.Columns.ERPJobID);
                            cbLevel2.Columns[1].Caption = ERPJobIDName;
                            cbLevel2.Columns.Add(DatabaseObjects.Columns.Title);
                            cbLevel2.Columns[2].Width = new Unit("306px");
                            cbLevel2.Columns.Add(DatabaseObjects.Columns.TicketStatus);
                            cbLevel2.Columns[3].Caption = "Status";
                            cbLevel2.DropDownWidth = Unit.Empty;
                            cbLevel2.TextFormatString = "{0} : {1} : {2}";
                        }
                        else
                        {
                            cbLevel2.Columns.Add(DatabaseObjects.Columns.Title);
                            cbLevel2.Columns[1].Width = new Unit("306px");
                            cbLevel2.Columns.Add(DatabaseObjects.Columns.TicketStatus);
                            cbLevel2.Columns[2].Caption = "Status";
                            cbLevel2.DropDownWidth = Unit.Empty;
                            cbLevel2.TextFormatString = "{0} : {1}";
                        }
                    }
                    else
                    {
                        cbLevel2.Columns.Clear();
                        cbLevel2.DropDownWidth = cbLevel2.Width;
                        cbLevel2.ValueField = "LevelTitle";
                        cbLevel2.ValueType = typeof(string);
                        cbLevel2.TextField = "LevelTitle";
                    }

                    cbLevel2.DataSource = resultedTableLevel2;                  
                    cbLevel2.DataBind();

                    if (cbLevel2.Items.Count > 1 && !fromModule)
                    {
                        // Else add the dummy item on top
                        cbLevel2.Items.Insert(0, new ListEditItem("--Select--", ""));
                    }
                }
                RResourceAllocation rAllocation = objAllocationManager.LoadByID(Convert.ToInt64(AllocationID));
                if (rAllocation != null)
                {
                    rAllocation.ResourceWorkItems = rAllocation.ResourceWorkItems = ObjWorkItemsManager.LoadByID(rAllocation.ResourceWorkItemLookup);
                    var item = cbLevel2.Items.FindByText(rAllocation.ResourceWorkItems.WorkItem);
                    if (item != null)
                    {
                        item.Selected = true;
                        FillDropDownLevel3(null, null);
                    }

                }

                if (!string.IsNullOrEmpty(WorkItem))
                {
                    var item = cbLevel2.Items.FindByValue(WorkItem);
                    if (item != null)
                    {
                        item.Selected = true;
                        cbLevel2.Enabled = false;
                        FillDropDownLevel3(null, null);
                    }
                }
                if (rAllocation != null && cbLevel3.Items.Count > 0 && cbLevel3.Visible == true && !string.IsNullOrWhiteSpace(rAllocation.ResourceWorkItems.SubWorkItem))
                {
                    cbLevel3.Text = rAllocation.ResourceWorkItems.SubWorkItem;
                    cbLevel3.Enabled = false;
                    cbLevel3.ForeColor = Color.LightGray;
                }

            }
        }
        protected void FillDropDownLevel2(object sender, EventArgs e)
        {
            cbLevel3.Items.Clear();
            cbLevel3.Visible = false;
            lblsubitem.Visible = false;
            lbl01.Visible = false;
            cbLevel2.Value = null;
            cbLevel2_Load(sender, e);
            if (cbLevel2.Items.Count == 1)
            {
                // If just one item, leave it pre-selected by default and get the level 3 items
                cbLevel2.SelectedIndex = 0;
                FillDropDownLevel3(cbLevel2, e);
            }
        }

        protected void FillDropDownLevel3(object sender, EventArgs e)
        {
            bool workitemexists = Convert.ToBoolean(ViewState["workitemexists"]);

            if (!string.IsNullOrEmpty(WorkItem) && !string.IsNullOrEmpty(WorkItemType))
                if (!workitemexists)
                    if (!IsPostBack && !enableProjStdWorkItems)
                        return;

            ViewState["workitemexists"] = null;

            cbLevel3.Items.Clear();
            cbLevel3.Text = string.Empty;
            string secondLevelVal = string.Empty;
            secondLevelVal = Convert.ToString(cbLevel2.Value);
            if (ddlLevel1.SelectedItem.Text != "--Select--" && !string.IsNullOrEmpty(secondLevelVal))
            {
                bool isvalidTicket = UGITUtility.IsValidTicketID(secondLevelVal);
                string modulename = UGITUtility.ObjectToString(ddlLevel1.SelectedValue);
                if (isvalidTicket)
                    modulename = uHelper.getModuleNameByTicketId(secondLevelVal);
                bool ismodule = false;
                if (modulename == "CPR" || modulename == "OPM" || modulename == "CNS" || modulename == "PMM" || modulename == "NPR")
                {
                    ismodule = true;
                }
                string userGlobalRoleId = string.Empty;
                 
                if (ismodule)
                {
                    lblsubitem.Text = "Roles";
                    //lblsubitem.Visible = true;
                    //cbLevel3.Visible = true;
                }
                else
                {
                    lblsubitem.Text = "Sub WorkItem";
                    //lblsubitem.Visible = true;
                    //cbLevel3.Visible = true;
                }
                lblsubitem.Visible = true;
                cbLevel3.Visible = true;
                lbl01.Visible = true;

                LoadcbLevel3(secondLevelVal, modulename, ismodule, userGlobalRoleId);

                if (Type == "Actuals")
                    LoadcbLevel4(secondLevelVal, modulename, ismodule);
                
            }
        }
        //This method has been created to load Level 4 control when the it goes blank.
        protected void FillDropDownLevel4()
        {
            string secondLevelVal = Convert.ToString(cbLevel2.Value);
            if (ddlLevel1.SelectedItem.Text != "--Select--" && !string.IsNullOrEmpty(secondLevelVal))
            {
                bool isvalidTicket = UGITUtility.IsValidTicketID(secondLevelVal);
                string modulename = UGITUtility.ObjectToString(ddlLevel1.SelectedValue);
                if (isvalidTicket)
                    modulename = uHelper.getModuleNameByTicketId(secondLevelVal);
                bool ismodule = false;
                if (modulename == "CPR" || modulename == "OPM" || modulename == "CNS" || modulename == "PMM" || modulename == "NPR")
                {
                    ismodule = true;
                }

                if (Type == "Actuals")
                    LoadcbLevel4(secondLevelVal, modulename, ismodule);
            }
        }

        private void EnableDateSelectionCheckboxes()
        {
            chkPreconDates.ClientVisible = true;

        }

        private void LoadcbLevel4(string secondLevelVal, string modulename, bool ismodule)
        {
            DataTable resultedTable = AllocationTypeManager.LoadLevel4(context, modulename, secondLevelVal, ismodule, enableProjStdWorkItems);
            if (resultedTable != null)
            {
                subSubitem.Visible = true;
                lblsubsubitem.Visible = true;
                cbLevel4.Visible = true;
                lblsubsubitem.Text = level4Name;

                cbLevel4.Columns.Clear();
                cbLevel4.Columns.Add("LevelId");
                cbLevel4.Columns[0].Caption = "ID";
                //cbLevel4.Columns[0].Width = new Unit("95px");
                cbLevel4.Columns[0].Visible = false;
                cbLevel4.Columns.Add("LevelName");
                cbLevel4.Columns[1].Width = new Unit("200px");
                cbLevel4.Columns[1].Caption = "Std Work Item";
                cbLevel4.Columns.Add("LevelCode");
                cbLevel4.Columns[2].Width = new Unit("95px");
                cbLevel4.Columns[2].Caption = "Job code";
                cbLevel4.Columns.Add("LevelDescription");
                cbLevel4.Columns[3].Width = new Unit("150px");
                cbLevel4.Columns[3].Caption = "Description";
                cbLevel4.DropDownWidth = Unit.Empty;
                cbLevel4.TextFormatString = "{0} : {1}";
                cbLevel4.ValueType = typeof(string);
                cbLevel4.ValueField = "LevelItem";
                cbLevel4.TextField = "LevelItem";

                cbLevel4.DataSource = resultedTable;
                cbLevel4.DataBind();
            }
            else
            {
                subSubitem.Visible = false;
            }
        }

        private void LoadcbLevel3(string secondLevelVal, string modulename, bool ismodule, string userGlobalRoleId)
        {
            //bool configEnableStd = configVariableManager.GetValueAsBool(ConfigConstants.EnableProjStdWorkItems);

            //DataTable resultedTable = AllocationTypeManager.LoadLevel3(context, modulename, secondLevelVal, userGlobalRoleId, ismodule, configEnableStd);
            DataTable resultedTable = AllocationTypeManager.LoadLevel3(context, modulename, secondLevelVal, userGlobalRoleId, ismodule, false);
            if (resultedTable != null)
            {
                //DataRow[] drModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleName, modulename));
                // Hide level 3 if module is selected
                if (resultedTable != null || resultedTable.Rows.Count > 0)
                {
                    //if (configEnableStd && ismodule)
                    //{
                    //    cbLevel3.Columns.Clear();
                    //    cbLevel3.Columns.Add("LevelId");
                    //    cbLevel3.Columns[0].Caption = "ID";
                    //    //cbLevel3.Columns[0].Width = new Unit("95px");
                    //    cbLevel3.Columns[0].Visible = false;
                    //    cbLevel3.Columns.Add("LevelName");
                    //    cbLevel3.Columns[1].Width = new Unit("200px");
                    //    cbLevel3.Columns[1].Caption = "Std Work Item";
                    //    cbLevel3.Columns.Add("LevelCode");
                    //    cbLevel3.Columns[2].Width = new Unit("95px");
                    //    cbLevel3.Columns[2].Caption = "Job code";
                    //    cbLevel3.Columns.Add("LevelDescription");
                    //    cbLevel3.Columns[3].Width = new Unit("150px");
                    //    cbLevel3.Columns[3].Caption = "Description";
                    //    cbLevel3.DropDownWidth = Unit.Empty;
                    //    cbLevel3.TextFormatString = "{0} : {1}";
                    //    cbLevel3.ValueType = typeof(string);
                    //    cbLevel3.ValueField = "LevelItem";
                    //    cbLevel3.TextField = "LevelItem";
                    //}
                    //else
                    //{
                        // For non-PMM, always load 3rd level
                        //cbLevel3.DataSource = resultedTable;
                        cbLevel3.ValueField = "LevelId";
                        cbLevel3.ValueType = typeof(string);
                        cbLevel3.TextField = "LevelName";

                        // Add selection placeholder
                        cbLevel3.Items.Insert(0, new DevExpress.Web.ListEditItem("--Select--", ""));
                    //}

                    cbLevel3.DataSource = resultedTable;
                    cbLevel3.DataBind();
                }

                

                // Hide level 3 if we have no items (except --Select-- place-holder) 
                // OR if we have single level 3 item with same name as level 2 (indicating place-holder)

                if (cbLevel3.Items.Count == 1 || (cbLevel3.Items.Count == 2 && secondLevelVal == Convert.ToString(cbLevel3.Items[1].Value)))
                {
                    cbLevel3.Visible = false;
                    lblsubitem.Visible = false;
                    lbl01.Visible = false;
                }
            }
            else
            {
                // No data
                cbLevel3.Visible = false;
                lblsubitem.Visible = false;
                lbl01.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            lbMessage.Text = string.Empty;

            if (Type == "ResourceAllocation")
            {
                if (!string.IsNullOrEmpty(AllocationID))
                    UpdateAllocation();
                else
                    NewAllocation();
            }
            else if (Type == "Actuals")
            {
                NewWorkItem();
            }

            if (!string.IsNullOrEmpty(lbMessage.Text))
            {
                return;
            }

            if(SelectedUsersList != null)
                UGITUtility.CreateCookie(Response, "SelectedUsers", string.Join(",", SelectedUsersList.Select(x => x.Id.ToString()).ToArray()));

            if (IsInPopupCallback)
                Response.Redirect(HttpContext.Current.Request.RawUrl.ToString());
            else
            {
                if (!RefreshPage)
                {
                    Context.Response.Write("<script type='text/javascript'>javascript:window.parent.CloseAndUpdatePage();</script>");
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    uHelper.ClosePopUpAndEndResponse(Context, RefreshPage);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (IsInPopupCallback)
                Response.Redirect(HttpContext.Current.Request.RawUrl.ToString());
            else
                uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void cusCustom_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //if (txtBody.Text.Trim() == string.Empty)
            //    args.IsValid = false;
            //else
            //    args.IsValid = true;
        }

        private void NewAllocation()
        {
            RResourceAllocation rAllocation = null;
            string errorMessage;
            
            if (ValidateAllocation(out errorMessage))
            {
                if (SelectedUsersList != null)
                {
                    foreach (UserProfile user in SelectedUsersList)
                    {
                        //Validate Overlapping allocation does not exits
                        List<RResourceAllocation> existingAllocation = objAllocationManager.Load(x => ((startDate.Date <= x.AllocationEndDate
                        && startDate.Date >= x.AllocationStartDate) || (endDate.Date <= x.AllocationEndDate && endDate.Date >= x.AllocationStartDate)) 
                        && UGITUtility.ObjectToString(cbLevel3.Value) == x.RoleId && x.Resource == user.Id 
                        && UGITUtility.ObjectToString(cbLevel2.Value) == x.TicketID);
                        if (existingAllocation != null && existingAllocation.Count > 0)
                        {
                            lbMessage.Text = Constants.ErrorMsgRMMDuplicateAllocation;
                            return;
                        }

                        rAllocation = new RResourceAllocation();

                        rAllocation.ResourceWorkItems = new ResourceWorkItems(user.Id);
                        string workitemtype = ddlLevel1.SelectedItem.Text.Replace("--Select--", string.Empty);
                        if (workitemtype == "Current Projects (PMM)" || workitemtype == "Project Management Module (PMM)" || workitemtype == uHelper.GetModuleTitle("PMM"))
                            workitemtype = "PMM";
                        if (workitemtype == "Project Management (CPR)" || workitemtype ==  uHelper.GetModuleTitle("CPR") || workitemtype.Contains("(CPR)"))
                            workitemtype = "CPR";
                        if (workitemtype == "Opportunity Management (OPM)" || workitemtype == uHelper.GetModuleTitle("OPM") || workitemtype.Contains("(OPM)"))
                            workitemtype = ModuleNames.OPM;
                        if (workitemtype == "Service Projects (CNS)" || workitemtype == uHelper.GetModuleTitle("CNS") || workitemtype.Contains("(CNS)"))
                            workitemtype = ModuleNames.CNS;
                        rAllocation.ResourceWorkItems.WorkItemType = workitemtype;

                        if (cbLevel2.Value != null)
                        {
                            rAllocation.ResourceWorkItems.WorkItem = Convert.ToString(cbLevel2.Value);
                            rAllocation.TicketID = UGITUtility.ObjectToString(cbLevel2.Value);
                            //Pick workitem title from Datatable using the cbLevel2.Value
                            DataRow[] drSelectedItem = resultedTableLevel2.Select(string.Format("LevelTitle='{0}'", cbLevel2.Value));
                            if (drSelectedItem.Length > 0 && fromModule)
                                rAllocation.Title = Convert.ToString(drSelectedItem[0][DatabaseObjects.Columns.Title]);
                            else
                            {
                                //If non-module item, title to be constructed using all the available levels
                                string title = ddlLevel1.Text + " > " + cbLevel2.Text;
                                if(cbLevel3.Visible == true && cbLevel3.SelectedItem.Text != "--Select--")
                                    title = title + " > " + cbLevel3.Text;
                                rAllocation.Title = title;
                            }
                            rAllocation.ResourceWorkItems.Title = rAllocation.Title;
                        }

                        if (cbLevel3.SelectedItem != null && cbLevel3.SelectedItem.Text != "--Select--")
                        {
                            rAllocation.ResourceWorkItems.SubWorkItem = Convert.ToString(cbLevel3.Text);
                            rAllocation.RoleId = UGITUtility.ObjectToString(cbLevel3.Value);
                        }
                        rAllocation.ResourceWorkItems.StartDate = startDate.Date;
                        rAllocation.ResourceWorkItems.EndDate = endDate.Date;
                        
                        rAllocation.ResourceWorkItemLookup = rAllocation.ResourceWorkItems.ID;
                        rAllocation.PctAllocation = int.Parse(txtAllocation.Text);
                        rAllocation.PctEstimatedAllocation = int.Parse(txtAllocation.Text);
                        rAllocation.SoftAllocation = UGITUtility.StringToBoolean(rdbSoftAllocation.Value);
                        rAllocation.NonChargeable = UGITUtility.StringToBoolean(rdbNonChargeable.Value);

                        if (startDate.Date != null && startDate.Date.TimeOfDay.ToString() == "00:00:00")
                        {
                            rAllocation.AllocationStartDate = startDate.Date;
                        }

                        if (endDate.Date != null && endDate.Date.TimeOfDay.ToString() == "00:00:00")
                        {
                            rAllocation.AllocationEndDate = endDate.Date;
                        }
                        rAllocation.Resource = user.Id;
                        //load global role id from subworkitem name

                        GlobalRoleManager roleManager = new GlobalRoleManager(context);
                        List<GlobalRole> lstGlobalRole = roleManager.Load(x => x.Name == rAllocation.ResourceWorkItems.SubWorkItem);
                        if (lstGlobalRole != null && lstGlobalRole.Count > 0)
                            rAllocation.RoleId = lstGlobalRole.FirstOrDefault().Id;

                        //Save Same Entry in Project Estimated Allocation if allocation is at project level
                        string projectid = rAllocation.ResourceWorkItems.WorkItem;
                        if (UGITUtility.IsValidTicketID(projectid))
                            workitemtype = uHelper.getModuleNameByTicketId(projectid);
                        uHelper.UpdateEstimatedAllocationTable(objAllocationManager, ObjEstimatedAllocationManager, rAllocation, workitemtype);

                        lbMessage.Text = objAllocationManager.Save(rAllocation);

                        
                        
                        string userName = context.UserManager.GetUserNameById(rAllocation.Resource);
                        string historyDesc = string.Format("Created new allocation for user: {0} - {1} {2}% {3}-{4}", userName, rAllocation.ResourceWorkItems.SubWorkItem, rAllocation.PctAllocation, String.Format("{0:MM/dd/yyyy}", rAllocation.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", rAllocation.AllocationEndDate));
                        ResourceAllocationManager.UpdateHistory(context, historyDesc, rAllocation.TicketID);
                        ULog.WriteLog(historyDesc);
                        if (lbMessage.Text == string.Empty && rAllocation.ResourceWorkItemLookup > 0)
                        {
                            string webUrl = HttpContext.Current.Request.Url.ToString();
                            //ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
                            long workItemID = rAllocation.ResourceWorkItemLookup;
                            //Start Thread to update rmm summary list and resourceallocation monthly w.r.t current workitem
                            ThreadStart threadStartMethod = delegate () { RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(context, workItemID); };
                            Thread sThread = new Thread(threadStartMethod);
                            sThread.IsBackground = true;
                            sThread.Start();
                        }
                    }
                    // Add entry in Project Complexity table
                    ThreadStart threadStart = delegate () { 
                        estimatedAllocationManager.UpdateProjectGroups(uHelper.getModuleNameByTicketId(rAllocation.TicketID), rAllocation.TicketID); 
                        //code commented bcz subbu will give new algo for complexity calculation
                        //estimatedAllocationManager.RefreshProjectComplexity(uHelper.getModuleNameByTicketId(rAllocation.TicketID), SelectedUsersList.Select(x => x.Id).Distinct().ToList()); 
                    };
                    Thread thread = new Thread(threadStart);
                    thread.IsBackground = true;
                    thread.Start();
                }

                

                uHelper.ClosePopUpAndEndResponse(HttpContext.Current, true);
            }
            else
            {
                lbMessage.Text = errorMessage;
                lbMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void AddNewAllocation(ProjectEstimatedAllocation crmAllocation)
        {
            ProjectEstimatedAllocationManager CRMProjAllocManager = new ProjectEstimatedAllocationManager(context);
            GlobalRoleManager roleManager = new GlobalRoleManager(context);
            UserProfileManager objUserProfileManager = new UserProfileManager(context);
            List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();
            List<string> historyDesc = new List<string>();
            crmAllocation.ID = 0;
            CRMProjAllocManager.Insert(crmAllocation);
            string roleName = string.Empty;
            GlobalRole uRole = roleManager.Get(x => x.Id == crmAllocation.Type);
            if (uRole != null)
                roleName = uRole.Name;

            lstUserWithPercetage.Add(
                new UserWithPercentage()
                {
                    EndDate = crmAllocation.AllocationEndDate ?? DateTime.MinValue,
                    StartDate = crmAllocation.AllocationStartDate ?? DateTime.MinValue,
                    Percentage = crmAllocation.PctAllocation,
                    UserId = crmAllocation.AssignedTo,
                    RoleTitle = roleName,
                    ProjectEstiAllocId = UGITUtility.ObjectToString(crmAllocation.ID),
                    RoleId = crmAllocation.Type,
                    SoftAllocation = crmAllocation.SoftAllocation,
                    NonChargeable = crmAllocation.NonChargeable,
                });

            historyDesc.Add(string.Format("Created new allocation for user: {0} - {1} {2}% {3}-{4}", context.UserManager.GetUserNameById(crmAllocation.AssignedTo), roleName, crmAllocation.PctAllocation, String.Format("{0:MM/dd/yyyy}", crmAllocation.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", crmAllocation.AllocationEndDate)));
            ThreadStart threadStartMethodUpdateCPRProjectAllocation = delegate ()
            {
                ResourceAllocationManager.CPRResourceAllocation(context, uHelper.getModuleNameByTicketId(crmAllocation.TicketId), crmAllocation.TicketId, lstUserWithPercetage);
                historyDesc.ForEach(o =>
                {
                    ULog.WriteLog("MV >> " + context.CurrentUser.Name + o);
                });
                ResourceAllocationManager.UpdateHistory(context, historyDesc, crmAllocation.TicketId);
            };
            Thread sThreadStartMethodUpdateCPRProjectAllocation = new Thread(threadStartMethodUpdateCPRProjectAllocation);
            sThreadStartMethodUpdateCPRProjectAllocation.IsBackground = true;
            sThreadStartMethodUpdateCPRProjectAllocation.Start();
        }
        private void UpdateAllocation()
        {
            try
            {
                uHelper._semaphore.WaitAsync();
                {
                    RResourceAllocation rAllocation = objAllocationManager.LoadByID(Convert.ToInt64(AllocationID));

                    foreach (UserProfile user in SelectedUsersList)
                    {
                        if (startDate.Date > endDate.Date)
                        {
                            lbMessage.Text = Constants.ErrorMsgRMMStartAndEndDate;
                            return;
                        }
                        //Validate Overlapping allocation does not exits

                        List<RResourceAllocation> existingAllocation = objAllocationManager.Load(x =>
                                (startDate.Date <= x.AllocationStartDate && endDate.Date >= x.AllocationStartDate
                                || startDate.Date <= x.AllocationEndDate && endDate.Date >= x.AllocationEndDate
                                || startDate.Date <= x.AllocationStartDate && endDate.Date >= x.AllocationEndDate
                                || startDate.Date >= x.AllocationStartDate && endDate.Date <= x.AllocationEndDate)
                                && UGITUtility.ObjectToString(cbLevel3.Value) == x.RoleId && x.Resource == user.Id
                                && UGITUtility.ObjectToString(cbLevel2.Value) == x.TicketID && UGITUtility.StringToLong(AllocationID) != x.ID);

                        if (existingAllocation != null && existingAllocation.Count > 0)
                        {
                            lbMessage.Text = Constants.ErrorMsgRMMDuplicateAllocation;
                            return;
                        }

                        DateTime oldAllocationStartDate = DateTime.MinValue;// varirable used to get project estimated entries
                        DateTime oldAllocationEndDate = DateTime.MinValue;
                        rAllocation.ResourceWorkItems = ObjWorkItemsManager.LoadByID(rAllocation.ResourceWorkItemLookup);

                        string oldUser = context.UserManager.GetUserNameById(rAllocation.Resource);
                        string historyDesc = string.Format("Updated allocation from user: {0} - {1} {2}% {3}-{4}", oldUser, rAllocation.ResourceWorkItems.SubWorkItem, rAllocation.PctAllocation, String.Format("{0:MM/dd/yyyy}", rAllocation.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", rAllocation.AllocationEndDate));

                        rAllocation.ResourceWorkItems.StartDate = startDate.Date;
                        rAllocation.ResourceWorkItems.EndDate = endDate.Date;
                        if (Type == "ResourceAllocation")
                        {
                            string title = "";
                            if (cbLevel2.Value != null)
                            {
                                rAllocation.ResourceWorkItems.WorkItem = Convert.ToString(cbLevel2.Value);
                                rAllocation.TicketID = UGITUtility.ObjectToString(cbLevel2.Value);
                                //Pick workitem title from Datatable using the cbLevel2.Value
                                DataRow[] drSelectedItem = resultedTableLevel2?.Select(string.Format("LevelTitle='{0}'", cbLevel2.Value));
                                if (drSelectedItem != null && drSelectedItem.Length > 0 && fromModule)
                                    title = Convert.ToString(drSelectedItem[0][DatabaseObjects.Columns.Title]);
                                else
                                {
                                    //If non-module item, title to be constructed using all the available levels
                                    title = ddlLevel1.Text + " > " + cbLevel2.Text;
                                }

                            }

                            if (cbLevel3.SelectedItem != null && cbLevel3.SelectedItem.Text != "--Select--")
                            {
                                rAllocation.ResourceWorkItems.SubWorkItem = Convert.ToString(cbLevel3.Text);
                                rAllocation.RoleId = UGITUtility.ObjectToString(cbLevel3.Value);
                                title = title + " > " + cbLevel3.Text;
                            }
                            rAllocation.Title = title;
                        }
                        ObjWorkItemsManager.Save(rAllocation.ResourceWorkItems);
                        double tempPctAlloc = rAllocation.PctAllocation.Value;
                        rAllocation.ResourceWorkItemLookup = rAllocation.ResourceWorkItems.ID;
                        rAllocation.PctAllocation = UGITUtility.StringToDouble(txtAllocation.Text);
                        rAllocation.PctEstimatedAllocation = UGITUtility.StringToDouble(txtAllocation.Text);
                        if (startDate.Date != null)
                        {
                            oldAllocationStartDate = UGITUtility.StringToDateTime(rAllocation.AllocationStartDate);
                            rAllocation.AllocationStartDate = startDate.Date;
                        }

                        if (endDate.Date != null)
                        {
                            oldAllocationEndDate = UGITUtility.StringToDateTime(rAllocation.AllocationEndDate);
                            rAllocation.AllocationEndDate = endDate.Date;
                        }
                        rAllocation.Resource = user.Id;
                        rAllocation.SoftAllocation = UGITUtility.StringToBoolean(rdbSoftAllocation.Value);
                        rAllocation.NonChargeable = UGITUtility.StringToBoolean(rdbNonChargeable.Value);
                        bool addNewAlloc = false;
                        if (tempPctAlloc != UGITUtility.StringToDouble(txtAllocation.Text) && startDate.Date.Date < DateTime.Now.Date && endDate.Date.Date >= DateTime.Now.Date)
                        {
                            rAllocation.AllocationEndDate = DateTime.Now.AddDays(-1);
                            rAllocation.PctAllocation = tempPctAlloc;
                            addNewAlloc = true;
                        }
                        lbMessage.Text = objAllocationManager.Save(rAllocation);

                        ProjectEstimatedAllocation updatedEstimatedAllocation = estimatedAllocationManager.LoadByID(UGITUtility.StringToLong(rAllocation.ProjectEstimatedAllocationId));
                        if (updatedEstimatedAllocation != null)
                        {
                            updatedEstimatedAllocation.AllocationStartDate = rAllocation.AllocationStartDate;
                            updatedEstimatedAllocation.AllocationEndDate = rAllocation.AllocationEndDate;
                            updatedEstimatedAllocation.PctAllocation = UGITUtility.StringToDouble(rAllocation.PctAllocation);
                            updatedEstimatedAllocation.Type = rAllocation.RoleId;
                            estimatedAllocationManager.Update(updatedEstimatedAllocation);
                            
                            if (addNewAlloc)
                            {
                                ProjectEstimatedAllocation projectEstimatedAllocationNew = ProjectEstimatedAllocationManager.DeepCopy(updatedEstimatedAllocation);
                                projectEstimatedAllocationNew.AllocationStartDate = DateTime.Now;
                                projectEstimatedAllocationNew.AllocationEndDate = endDate.Date;
                                projectEstimatedAllocationNew.PctAllocation = UGITUtility.StringToDouble(txtAllocation.Text);
                                int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, projectEstimatedAllocationNew.AllocationStartDate.Value, projectEstimatedAllocationNew.AllocationEndDate.Value);
                                int noOfWeeks = uHelper.GetWeeksFromDays(context, noOfWorkingDays);
                                projectEstimatedAllocationNew.ID = 0;
                                projectEstimatedAllocationNew.Duration = noOfWeeks;
                                AddNewAllocation(projectEstimatedAllocationNew);
                            }
                        }

                        string newUser = context.UserManager.GetUserNameById(rAllocation.Resource);
                        historyDesc += string.Format(" to {0} - {1} {2}% {3}-{4}", oldUser, rAllocation.ResourceWorkItems.SubWorkItem, rAllocation.PctAllocation, String.Format("{0:MM/dd/yyyy}", rAllocation.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", rAllocation.AllocationEndDate));

                        ////update project estimated table based on old dates
                        //List<ProjectEstimatedAllocation> lstOldEstimatedAllocation = estimatedAllocationManager.Load(x => x.AllocationStartDate == oldAllocationStartDate
                        //        && x.AllocationEndDate == oldAllocationEndDate &&
                        //     x.AssignedTo == rAllocation.Resource && x.TicketId == rAllocation.ResourceWorkItems.WorkItem && x.Type == rAllocation.RoleId);

                        //if (lstOldEstimatedAllocation != null && lstOldEstimatedAllocation.Count > 0)
                        //{
                        //    ProjectEstimatedAllocation updatedEstimatedAllocation = lstOldEstimatedAllocation.FirstOrDefault();
                        //    updatedEstimatedAllocation.AllocationStartDate = rAllocation.AllocationStartDate;
                        //    updatedEstimatedAllocation.AllocationEndDate = rAllocation.AllocationEndDate;
                        //    updatedEstimatedAllocation.PctAllocation = UGITUtility.StringToDouble(rAllocation.PctAllocation);
                        //    estimatedAllocationManager.Update(updatedEstimatedAllocation);
                        //}
                        //else
                        //{

                        //}

                        if (UGITUtility.IsValidTicketID(rAllocation.TicketID))
                        {
                            ResourceAllocationManager.UpdateHistory(context, historyDesc, rAllocation.TicketID);
                            ULog.WriteLog("MV >> " + HttpContext.Current.GetManagerContext().CurrentUser.Name + historyDesc);
                        }
                        if (lbMessage.Text == string.Empty && rAllocation.ResourceWorkItemLookup > 0)
                        {
                            string webUrl = HttpContext.Current.Request.Url.ToString();
                            //ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
                            long workItemID = rAllocation.ResourceWorkItemLookup;

                            //Start Thread to update rmm summary list and resourceallocation monthly w.r.t current workitem
                            ThreadStart threadStartMethod = delegate () { RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(context, workItemID); };
                            Thread sThread = new Thread(threadStartMethod);
                            sThread.IsBackground = true;
                            sThread.Start();
                        }
                    }

                    // Update entry in Project Complexity table
                    if (SelectedUsersList != null && SelectedUsersList.Count > 0)
                    {
                        ThreadStart threadStart = delegate () { 
                            estimatedAllocationManager.UpdateProjectGroups(uHelper.getModuleNameByTicketId(rAllocation.TicketID), rAllocation.TicketID); 
                            //commented bcz subbu will give new algo for complexity calculation
                            //estimatedAllocationManager.RefreshProjectComplexity(uHelper.getModuleNameByTicketId(rAllocation.TicketID), SelectedUsersList.Select(x => x.Id).Distinct().ToList()); 
                        };
                        Thread thread = new Thread(threadStart);
                        thread.IsBackground = true;
                        thread.Start();
                    }
                    if (RefreshPage)
                    {
                        uHelper.ClosePopUpAndEndResponse(HttpContext.Current, RefreshPage);
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException("UpdateAllocations_semaphore Lock:" + ex.ToString());
            }
            finally
            {
                uHelper._semaphore.Release();
            }
        }
        private void FillAllocation()
        {
            RResourceAllocation rAllocation = objAllocationManager.LoadByID(Convert.ToInt64(AllocationID));
            if (rAllocation != null)
            {
                SelectedUsersList = new List<UserProfile>();
                SelectedUsersList = objUserManager.GetUserInfosById(rAllocation.Resource);
                if (rAllocation.ResourceWorkItemLookup > 0)
                    rAllocation.ResourceWorkItems = ObjWorkItemsManager.LoadByID(rAllocation.ResourceWorkItemLookup);
                else
                {
                    rAllocation.ResourceWorkItems = new ResourceWorkItems(HttpContext.Current.CurrentUser().Id);
                    if (ddlLevel1.SelectedIndex > 0)
                    {
                        rAllocation.ResourceWorkItems.WorkItemType = ddlLevel1.SelectedItem.Text;
                    }
                    if (cbLevel2.Value != null)
                    {
                        rAllocation.ResourceWorkItems.WorkItem = Convert.ToString(cbLevel2.Value);
                    }
                    if (cbLevel3.SelectedItem != null && cbLevel3.SelectedItem.Text != "--Select--")
                    {
                        rAllocation.ResourceWorkItems.SubWorkItem = Convert.ToString(cbLevel3.Value);
                    }
                    rAllocation.ResourceWorkItems.StartDate = startDate.Date;
                    rAllocation.ResourceWorkItems.EndDate = endDate.Date;
                    ObjWorkItemsManager.Insert(rAllocation.ResourceWorkItems);
                    rAllocation.ResourceWorkItemLookup = rAllocation.ResourceWorkItems.ID;
                    ObjWorkItemsManager.Insert(rAllocation.ResourceWorkItems);
                }
                var item = ddlLevel1.Items.FindByText(rAllocation.ResourceWorkItems.WorkItemType);
                if (item != null)
                    item.Selected = true;
                else
                {
                    ddlLevel1.Items.Insert(0, new ListItem(rAllocation.ResourceWorkItems.WorkItemType, rAllocation.ResourceWorkItems.WorkItemType));
                 }
                if (!string.IsNullOrWhiteSpace(rAllocation.ResourceWorkItems.WorkItem))
                {
                    cbLevel2.Text = rAllocation.ResourceWorkItems.WorkItem;
                    cbLevel3.Visible = true;
                    lblsubitem.Visible = true;
                    lbl01.Visible = true;
                    string modulename = string.Empty;
                    if (UGITUtility.IsValidTicketID(rAllocation.ResourceWorkItems.WorkItem))
                    {
                        modulename = uHelper.getModuleNameByTicketId(rAllocation.ResourceWorkItems.WorkItem);
                        lblsubitem.Text = "Roles";
                        LoadcbLevel3(rAllocation.ResourceWorkItems.WorkItem, modulename, true, string.Empty);
                    }
                    else
                    { 
                        lblsubitem.Text = "Sub Item";
                    }
                }
                else
                    workitem.Visible = false;

                if (!string.IsNullOrWhiteSpace(rAllocation.ResourceWorkItems.SubWorkItem))
                {
                    cbLevel3.Text = rAllocation.ResourceWorkItems.SubWorkItem;
                    
                }
                else
                    subitem.Visible = false;

                txtAllocation.Text = UGITUtility.ObjectToString(rAllocation.PctAllocation);
                rdbSoftAllocation.Value = rAllocation.SoftAllocation;
                rdbNonChargeable.Value = rAllocation.NonChargeable;
                startDate.Date = UGITUtility.StringToDateTime(rAllocation.AllocationStartDate);
                endDate.Date = UGITUtility.StringToDateTime(rAllocation.AllocationEndDate);
            }
        }
        protected void NewWorkItem()
        {
            string errorMessage;
            if (ValidateAllocation(out errorMessage))
            {
                if (SelectedUsersList != null && SelectedUsersList.Count == 1)
                {
                    ResourceWorkItems workItem = new ResourceWorkItems(SelectedUsersList[0].Id);
                    if (ddlLevel1.SelectedItem != null && ddlLevel1.SelectedItem.Text.Trim() != string.Empty && ddlLevel1.SelectedItem.Text.Trim() != "--Select--")
                    {
                        string workitemtype = ddlLevel1.SelectedItem.Value.Replace("--Select--", string.Empty);
                        if (workitemtype == "Current Projects (PMM)" || workitemtype == "Project Management Module (PMM)" || workitemtype.Contains("(PMM)"))
                            workitemtype = "PMM";

                        if (workitemtype == "Opportunity Management (OPM)" || workitemtype.Contains("(OPM)"))
                            workitemtype = ModuleNames.OPM;

                        if (workitemtype == "Project Management (CPR)" || workitemtype.Contains("(CPR)"))
                            workitemtype = ModuleNames.CPR;

                        if (workitemtype == "Service Projects (CNS)" || workitemtype.Contains("(CNS)"))
                            workitemtype = ModuleNames.CNS;

                        workItem.WorkItemType = workitemtype;
                        if (cbLevel2.Value != null)
                        {
                            workItem.WorkItem = Convert.ToString(cbLevel2.Value);
                        }

                        if (cbLevel3.SelectedItem != null && cbLevel3.SelectedItem.Text != string.Empty && cbLevel3.SelectedItem.Text != "--Select--")
                        {
                            workItem.SubWorkItem = cbLevel3.SelectedItem.Text;
                        }
                        else if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(cbLevel3.Value)))
                        {
                            workItem.SubWorkItem = UGITUtility.ObjectToString(cbLevel3.Value);
                        }

                        if (cbLevel3.Visible == true)
                        {
                            if (string.IsNullOrEmpty(workItem.SubWorkItem))
                            {
                                lbMessage.Text = $"{lblsubitem.Text} required.";
                                lbMessage.ForeColor = System.Drawing.Color.Red;

                                ViewState["workitemexists"] = true;
                                FillDropDownLevel3(null, null);
                                return;
                            }
                        }

                        if (cbLevel4.SelectedItem != null && cbLevel4.SelectedItem.Text != string.Empty && cbLevel4.SelectedItem.Text != "--Select--")
                        {
                            workItem.SubSubWorkItem = cbLevel4.SelectedItem.Text;
                        }
                        else if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(cbLevel4.Value)))
                        {
                            workItem.SubSubWorkItem = UGITUtility.ObjectToString(cbLevel4.Value);
                        }

                        ResourceWorkItemsManager ObjWorkItemManager = new ResourceWorkItemsManager(context);

                        workItem.StartDate = Convert.ToDateTime(StartDate.ToShortDateString()); //StartDate;
                        workItem.EndDate = Convert.ToDateTime(StartDate.AddDays(6).ToShortDateString());
                        long messageType = ObjWorkItemManager.SaveResourceWorkItems(workItem);

                        switch (messageType)
                        {                          
                            case 2:
                                if (workItem.Deleted)
                                    lbMessage.Text = "Work Item deleted!";
                                else
                                {
                                    string moduleName = uHelper.getModuleNameByTicketId(Convert.ToString(cbLevel2.Value)); 
                                    UGITModule module = ModuleManager.LoadByName(moduleName);
                                    if (module != null && (module.ModuleName == "NPR" || module.ModuleName == "PMM" || module.ModuleName == "TSK" || module.ModuleName == "OPM" || module.ModuleName == "CPR" || module.ModuleName == "CNS") && module.ActualHoursByUser)
                                    {
                                        lbMessage.Text = "Actuals are only enabled at the task level for projects, please enter your hours on the task(s) or ask the project manager to assign you to the tasks";
                                    }
                                    else
                                    {
                                        lbMessage.Text = "Work Item already present!";

                                        if (cbLevel3.Visible == true)
                                        {
                                            ViewState["workitemexists"] = true;
                                            FillDropDownLevel3(null, null);
                                        }
                                    }
                                    lbMessage.ForeColor = System.Drawing.Color.Red;
                                }
                                break;
                            default:
                                lbMessage.Text = string.Empty;
                                break;
                        }
                    }
                }
            }
            else
            {
                lbMessage.Text = "Error inserting allocation";
                if (!string.IsNullOrEmpty(errorMessage))
                    lbMessage.Text = errorMessage;
                lbMessage.ForeColor = System.Drawing.Color.Red;
            }
        }


        private bool ValidateAllocation(out string errorMessage)
        {
            bool noErrorExist = true;
            errorMessage = string.Empty;

            if (Type == "ResourceAllocation")
            {
                if (txtAllocation.Text.Trim() == string.Empty)
                {
                    noErrorExist = false;
                    errorMessage = Constants.ErrorMsgRMMInvalidAllocationPct;
                }
                else
                {
                    try
                    {
                        int allocation = int.Parse(txtAllocation.Text.Trim());
                        if (allocation < 1 || allocation > 100)
                        {
                            noErrorExist = false;
                            errorMessage = Constants.ErrorMsgRMMInvalidAllocationPct;
                        }
                    }
                    catch
                    {
                        noErrorExist = false;
                        errorMessage = Constants.ErrorMsgRMMInvalidAllocationPct;
                    }
                }


                //if (cmbLevel2.Value == null || level2.SelectedItem.Text.Trim() == string.Empty || level2.SelectedItem.Text.Trim() == "--Select--")
                if (cbLevel2.Value == null)
                {
                    noErrorExist = false;
                    errorMessage = Constants.ErrorMsgRMMInvalidWorkItem;
                }

                else if (cbLevel3.Visible == true && cbLevel3.Value == null)
                {
                    noErrorExist = false;
                    errorMessage = $"{lblsubitem.Text} is mandatory";
                }

                else if (ddlLevel1.SelectedItem == null || ddlLevel1.SelectedItem.Text.Trim() == string.Empty || ddlLevel1.SelectedItem.Text.Trim() == "--Select--")
                {
                    noErrorExist = false;
                    errorMessage = Constants.ErrorMsgRMMInvalidWorkItem;
                }

                else if (startDate.Date == DateTime.MinValue)
                {
                    noErrorExist = false;
                    errorMessage = "Start Date is mandatory";
                }
                else if (endDate.Date == DateTime.MinValue)
                {
                    noErrorExist = false;
                    errorMessage = "End Date is mandatory";
                }
                else
                {
                    // The date controls are set to Date-only. So use the fact that a blank control will return current time (instead of zero time) to decide whether user entered a value
                    // If not, default startdate to now, and enddate to end of the year
                    DateTime start = startDate.Date.TimeOfDay.ToString() == "00:00:00" ? startDate.Date : DateTime.Now.Date;
                    DateTime end = endDate.Date.TimeOfDay.ToString() == "00:00:00" ? endDate.Date : DateTime.Now.Date;
                    if (start > end)
                    {
                        noErrorExist = false;
                        errorMessage = Constants.ErrorMsgRMMInvalidDates;
                    }
                }
            }
            else if (Type == "Actuals")
            {
                noErrorExist = true;
                if (ddlLevel1.SelectedItem == null || ddlLevel1.SelectedItem.Text.Trim() == string.Empty || ddlLevel1.SelectedItem.Text.Trim() == "--Select--")
                {
                    noErrorExist = false;
                }
                if (cbLevel2.Value == null)
                {
                    noErrorExist = false;
                }
                if (enableProjStdWorkItems && cbLevel4.Visible && cbLevel4.Value == null) { 
                    errorMessage = $"Please select {level4Name}";
                    noErrorExist = false;
                    FillDropDownLevel4();
                }
            }

            return noErrorExist;
        }
    }
}
