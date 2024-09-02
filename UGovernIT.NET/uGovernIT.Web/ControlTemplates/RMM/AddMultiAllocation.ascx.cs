using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager.Managers;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
//using System.Web;
using DevExpress.Data.Async.Helpers;
using static uGovernIT.Web.ModuleResourceAddEdit;
using System.Configuration;
using uGovernIT.Web.Controllers;
using DevExpress.ExpressApp;
using DevExpress.XtraRichEdit.Model;
using System.Threading;
using uGovernIT.Helpers;
using uGovernIT.Util.Log;

namespace uGovernIT.Web.ControlTemplates.RMM
{

    public partial class AddMultiAllocation : System.Web.UI.UserControl
    {
        public string UserID { get; set; }
        public string RoleID { get; set; }
        public string Type { get; set; }
        public string WorkItem { get; set; }
        public List<MultiAllocations> MultiAllocationList {
            get {
                if (Session["Data"] == null)
                    Session["Data"] = new List<MultiAllocations>();
                return (List<MultiAllocations>)(Session["Data"]);
            }
        }
        public string preconStart { 
            get {
                return UGITUtility.ObjectToString(Session["PreconStart"]);
            }
            set
            {
                Session["PreconStart"] = value;
            }
        }
        public string preconEnd {
            get {
                return UGITUtility.ObjectToString(Session["PreconEnd"]);
            }
            set
            {
                Session["PreconEnd"] = value;
            }
        }
        public string constStart {
            get
            {
                return UGITUtility.ObjectToString(Session["ConstStart"]);
            }
            set
            {
                Session["ConstStart"] = value;
            }
        }
        public string constEnd {
            get
            {
                return UGITUtility.ObjectToString(Session["ConstEnd"]);
            }
            set
            {
                Session["ConstEnd"] = value;
            }
        }
        public string closeout {
            get
            {
                return UGITUtility.ObjectToString(Session["Closeout"]);
            }
            set
            {
                Session["Closeout"] = value;
            }
        }
        public string ModuleName { get; set; }
        public DateTime StartDate {
            get
            {
                return UGITUtility.StringToDateTime(Session["StartDate"]);
            }
            set
            {
                Session["StartDate"] = value;
            }
        }
        public int ItemCounter
        {
            get
            {
                if (Session["Counter"] == null)
                    Session["Counter"] = 999;
                return UGITUtility.StringToInt(Session["Counter"]);
            }
            set
            {
                Session["Counter"] = value;
            }
        }
        public DateTime EndDate {
            get
            {
                return UGITUtility.StringToDateTime(Session["EndDate"]);
            }
            set
            {
                Session["EndDate"] = value;
            }
        }
        //private UserProfile SelectedUser;
        public ApplicationContext context = HttpContext.Current.GetManagerContext();
        public ConfigurationVariableManager configVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        public ResourceAllocationManager objAllocationManager = new ResourceAllocationManager(HttpContext.Current.GetManagerContext());
        ResourceWorkItemsManager ObjWorkItemsManager = new ResourceWorkItemsManager(HttpContext.Current.GetManagerContext());
        ProjectEstimatedAllocationManager ObjEstimatedAllocationManager = new ProjectEstimatedAllocationManager(HttpContext.Current.GetManagerContext());
        protected void Page_Load(object sender, EventArgs e)
        {
            //UserID = "08c74834-0a9f-429b-85c1-af5ccae0c487";
            if (!string.IsNullOrEmpty(WorkItem))
            {
                workitemrow.Attributes.Add("style", "display:none;");
                userrow.Attributes.Add("style", "display:block;");
                ModuleName = Type;
            }
            else
            {
                workitemrow.Attributes.Add("style", "display:block;");
                userrow.Attributes.Add("style", "display:none;");
            }
            if (!Page.IsPostBack)
            {
                Session["Data"] = null;
                if (MultiAllocationList != null && MultiAllocationList.Count <= 0)
                {
                    int id = GetNextId();
                    MultiAllocationList.Add(new MultiAllocations() { Id = id, StartDate = DateTime.MinValue, EndDate = DateTime.MinValue, PctAllocation = null, Role = RoleID, SoftAllocation = false, WorkItem = string.Empty, WorkItemType = ModuleName });
                }

            }
            multiAllocationPanel.DataSource = MultiAllocationList.OrderBy(x=>x.Id).ToList();
            multiAllocationPanel.DataBind();
            if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(ddlLevel1.SelectedItem?.Text)))
                ModuleName = UGITUtility.ObjectToString(ddlLevel1.SelectedItem?.Value);
            SetDateLabels();

        }

        private int GetNextId()
        {
            int id = ItemCounter;
            Session["Counter"] = --id;
            return id;
        }

        private void SetDateLabels()
        {
            if (!string.IsNullOrEmpty(ModuleName))
            {
                TicketManager objTicketManager = new TicketManager(context);
                Ticket ticketRequest = new Ticket(context, ModuleName);
                string ticketTableName = ticketRequest.Module.ModuleTable;
                string ticketID = UGITUtility.ObjectToString(cbLevel2.SelectedItem?.Value);
                if (string.IsNullOrEmpty(ticketID))
                    ticketID = WorkItem;
                if (!string.IsNullOrEmpty(ticketID) && UGITUtility.IsValidTicketID(ticketID))
                {
                    DataRow currentTicket = objTicketManager.GetTicketTableBasedOnTicketId(ModuleName, ticketID).Rows[0];
                    if (currentTicket != null)
                    {
                        preconStart = UGITUtility.GetDateStringInFormat(Convert.ToString(currentTicket[DatabaseObjects.Columns.PreconStartDate]), false);
                        preconEnd = UGITUtility.GetDateStringInFormat(Convert.ToString(currentTicket[DatabaseObjects.Columns.PreconEndDate]), false);
                        constStart = UGITUtility.GetDateStringInFormat(Convert.ToString(currentTicket[DatabaseObjects.Columns.EstimatedConstructionStart]), false);
                        constEnd = UGITUtility.GetDateStringInFormat(Convert.ToString(currentTicket[DatabaseObjects.Columns.EstimatedConstructionEnd]), false);
                        closeout = UGITUtility.GetDateStringInFormat(Convert.ToString(currentTicket["CloseOutDate"]), false);
                        DateTime closeoutstartdate = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                        if (closeoutstartdate != DateTime.MinValue)
                            closeoutstartdate = closeoutstartdate.AddDays(1);
                        string closeoutStart = UGITUtility.GetDateStringInFormat(Convert.ToString(closeoutstartdate), false);

                        if (!string.IsNullOrEmpty(preconStart) || !string.IsNullOrEmpty(preconEnd))
                            lblPrecon.Text = $"<b> {preconStart} to {preconEnd}</b>";
                        else
                            lblPrecon.Text = "<p>&nbsp;</p>";

                        if(!string.IsNullOrEmpty(constStart) || !string.IsNullOrEmpty(constEnd))
                            lblConst.Text = $"<b> {constStart} to {constEnd}</b>";
                        else
                            lblConst.Text = "<p>&nbsp;</p>";

                        if (!string.IsNullOrEmpty(closeoutStart) || !string.IsNullOrEmpty(closeout))
                            lblCloseout.Text = $"<b> {closeoutStart} to {closeout}</b>";
                        else
                            lblCloseout.Text = "<p>&nbsp;</p>";
                    }
                }
            }
        }

        protected void ASPxDataView1_CustomCallback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            List<string> values = UGITUtility.ConvertStringToList(e.Parameter, Constants.Separator2);
            string callbackType = UGITUtility.ObjectToString(values[0]);
            switch (callbackType)
            {
                case "Add":
                    {
                        MultiAllocationList.Add(new MultiAllocations() { Id = GetNextId(), StartDate = DateTime.MinValue, EndDate = DateTime.MinValue, PctAllocation = null, Role = RoleID, WorkItem = "", WorkItemType = "" });
                        multiAllocationPanel.DataSource = MultiAllocationList.OrderBy(x=>x.Id);
                        multiAllocationPanel.DataBind();
                    }
                    break;
                case "PreconDate":
                    {
                        if (values.Count > 1 && !string.IsNullOrEmpty(UGITUtility.ObjectToString(values[1])))
                            SetDatesOnDataItem(UGITUtility.StringToDateTime(preconStart), UGITUtility.StringToDateTime(preconEnd), values[1]);
                        else
                            SetStartEndDatesToMinValue();
                    }
                    break;
                case "ConstDate":
                    {
                        if (values.Count > 1 && !string.IsNullOrEmpty(UGITUtility.ObjectToString(values[1])))
                            SetDatesOnDataItem(UGITUtility.StringToDateTime(constStart), UGITUtility.StringToDateTime(constEnd), values[1]);
                        else
                            SetStartEndDatesToMinValue();
                    }
                    break;
                case "CloseDate":
                    {
                        if (values.Count > 1 && !string.IsNullOrEmpty(UGITUtility.ObjectToString(values[1])))
                            SetDatesOnDataItem(UGITUtility.StringToDateTime(constEnd), UGITUtility.StringToDateTime(closeout), values[1]);
                        else
                            SetStartEndDatesToMinValue();
                    }
                    break;
                case "StartDate":
                    {
                        if (values.Count > 2)
                        {
                            MultiAllocations item = MultiAllocationList.FirstOrDefault(x => x.Id == UGITUtility.StringToInt(values[1]));
                            if (item != null)
                                item.StartDate = UGITUtility.StringToDateTime(values[2]);
                        }
                    }
                    break;
                case "EndDate":
                    {
                        if (values.Count > 2)
                        {
                            MultiAllocations item = MultiAllocationList.FirstOrDefault(x => x.Id == UGITUtility.StringToInt(values[1]));
                            if (item != null)
                                item.EndDate = UGITUtility.StringToDateTime(values[2]);
                        }
                    }
                    break;
                case "Role":
                    {
                        if(values.Count > 2)
                        {
                            MultiAllocations item = MultiAllocationList.FirstOrDefault(x => x.Id == UGITUtility.StringToInt(values[1]));
                            if (item != null)
                                item.Role = UGITUtility.ObjectToString(values[2]);
                        }
                    }
                    break;
                case "PctAllocation":
                    {
                        if (values.Count > 2)
                        {
                            MultiAllocations item = MultiAllocationList.FirstOrDefault(x => x.Id == UGITUtility.StringToInt(values[1]));
                            if (item != null)
                                item.PctAllocation = UGITUtility.StringToDouble(values[2]);
                        }
                    }
                    break;
                case "Delete":
                    {
                        if (values.Count > 1)
                        {
                            MultiAllocations item = MultiAllocationList.FirstOrDefault(x => x.Id == UGITUtility.StringToInt(values[1]));
                            if (item != null)
                                MultiAllocationList.Remove(item);
                            multiAllocationPanel.DataSource = MultiAllocationList.OrderBy(x=>x.Id);
                            multiAllocationPanel.DataBind();
                        }
                    }
                    break;
                default:
                    break;
            }
 
        }

        private void SetStartEndDatesToMinValue()
        {
            StartDate = DateTime.MinValue; EndDate = DateTime.MinValue;
        }

        private void SetDatesOnDataItem(DateTime start, DateTime end)
        {
            StartDate = UGITUtility.StringToDateTime(start);
            EndDate = UGITUtility.StringToDateTime(end);
            if (EndDate == DateTime.MinValue)
                EndDate = StartDate;
            MultiAllocations item = MultiAllocationList.OrderBy(x=>x.Id).FirstOrDefault();
            if (item != null)
            {
                item.StartDate = StartDate;
                item.EndDate = EndDate;
            }

            Session["Data"] = MultiAllocationList.OrderBy(x => x.Id).ToList();
            multiAllocationPanel.DataSource = MultiAllocationList.OrderBy(x => x.Id);
            multiAllocationPanel.DataBind();
        }

        private void SetDatesOnDataItem(DateTime start, DateTime end, string selectedIds)
        {
            List<string> lstSelectedIds = UGITUtility.ConvertStringToList(selectedIds, Constants.Separator6);
            for (int i = 0; i < multiAllocationPanel.VisibleItems.Count; i++)
            {
                DataViewItem dataItem = multiAllocationPanel.Items[i];
                ASPxDateEdit dteStartDate = multiAllocationPanel.FindItemControl("dteStartDate", dataItem) as ASPxDateEdit;
                ASPxDateEdit dteEndDate = multiAllocationPanel.FindItemControl("dteEndDate", dataItem) as ASPxDateEdit;

                StartDate = UGITUtility.StringToDateTime(start);
                EndDate = UGITUtility.StringToDateTime(end);

                MultiAllocations item = dataItem.DataItem as MultiAllocations;
                if (item != null)
                {
                    bool itemExits = lstSelectedIds.Contains(UGITUtility.ObjectToString(item.Id));
                    if (itemExits)
                    {
                        item.StartDate = StartDate;
                        item.EndDate = EndDate;
                    }                   
                }
            }

            Session["Data"] = MultiAllocationList.OrderBy(x => x.Id).ToList();
            multiAllocationPanel.DataSource = MultiAllocationList.OrderBy(x => x.Id);
            multiAllocationPanel.DataBind();
        }

        protected void cbLevel2_Load(object sender, EventArgs e)
        {
            if (ddlLevel1.SelectedItem != null && ddlLevel1.SelectedItem.Text != "--Select--")
            {
                DataRow[] drModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, ddlLevel1.SelectedItem.Text.Trim()));
                bool fromModule = drModules != null && drModules.Length > 0;
                if (fromModule)
                {
                    string moduleName = UGITUtility.ObjectToString(drModules[0][DatabaseObjects.Columns.ModuleName]);
                }
                DataTable resultedTable = AllocationTypeManager.LoadLevel2(context, ddlLevel1.SelectedItem.Text, fromModule);
                if (resultedTable != null)
                {
                    bool ShowERPJobID = configVariableManager.GetValueAsBool(ConfigConstants.ShowERPJobID);
                    string ERPJobIDName = configVariableManager.GetValue(ConfigConstants.ERPJobIDName);
                    if (fromModule)
                    {
                        cbLevel2.Columns.Clear();
                        cbLevel2.Columns.Add("LevelTitle");
                        cbLevel2.Columns[0].Caption = "ID";
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

                    cbLevel2.DataSource = resultedTable;
                    cbLevel2.DataBind();

                    if (cbLevel2.Items.Count > 1 && !fromModule)
                    {
                        // Else add the dummy item on top
                        cbLevel2.Items.Insert(0, new ListEditItem("--Select--", ""));
                    }
                }
                               
            }
        }

        protected void ddlLevel1_Load(object sender, EventArgs e)
        {
            //DropDownList level1 = (DropDownList)sender;
            if (ddlLevel1.Items.Count <= 0)
            {
                DataTable resultedTable = AllocationTypeManager.LoadLevel1(context);
                if (resultedTable != null)
                {
                    ddlLevel1.Items.Add(new ListEditItem("--Select--", "--Select--"));
                    foreach (DataRow row in resultedTable.Rows)
                    {
                        if (row["LevelTitle"] != null && row["LevelTitle"].ToString() != string.Empty)
                        {
                            if (row["LevelName"].ToString() == "CPR" || row["LevelName"].ToString() == "CNS" || row["LevelName"].ToString() == "OPM")
                                ddlLevel1.Items.Add(new ListEditItem(row["LevelTitle"].ToString(), row["LevelName"].ToString()));
                        }
                    }
                }
            }
            
        }

        protected void ddlLevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLevel1.SelectedItem != null && ddlLevel1.SelectedItem.Text != "--Select--")
            {
                DataRow[] drModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, ddlLevel1.SelectedItem.Text.Trim()));
                bool fromModule = drModules != null && drModules.Length > 0;
                
                DataTable resultedTable = AllocationTypeManager.LoadLevel2(context, ddlLevel1.SelectedItem.Text, fromModule);
                if (resultedTable != null)
                {
                    bool ShowERPJobID = configVariableManager.GetValueAsBool(ConfigConstants.ShowERPJobID);
                    string ERPJobIDName = configVariableManager.GetValue(ConfigConstants.ERPJobIDName);
                    if (fromModule)
                    {
                        cbLevel2.Columns.Clear();
                        cbLevel2.Columns.Add("LevelTitle");
                        cbLevel2.Columns[0].Caption = "ID";
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

                    cbLevel2.DataSource = resultedTable;
                    cbLevel2.DataBind();

                    if (cbLevel2.Items.Count > 1 && !fromModule)
                    {
                        // Else add the dummy item on top
                        cbLevel2.Items.Insert(0, new ListEditItem("--Select--", ""));
                    }
                }

            }
        }

        protected void cbLevel2_Callback(object sender, CallbackEventArgsBase e)
        {
            if (ddlLevel1.SelectedItem != null && ddlLevel1.SelectedItem.Text != "--Select--")
            {
                DataRow[] drModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, ddlLevel1.SelectedItem.Text.Trim()));
                bool fromModule = drModules != null && drModules.Length > 0;
                
                DataTable resultedTable = AllocationTypeManager.LoadLevel2(context, ddlLevel1.SelectedItem.Text, fromModule);
                if (resultedTable != null)
                {
                    bool ShowERPJobID = configVariableManager.GetValueAsBool(ConfigConstants.ShowERPJobID);
                    string ERPJobIDName = configVariableManager.GetValue(ConfigConstants.ERPJobIDName);
                    if (fromModule)
                    {
                        cbLevel2.Columns.Clear();
                        cbLevel2.Columns.Add("LevelTitle");
                        cbLevel2.Columns[0].Caption = "ID";
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

                    cbLevel2.DataSource = resultedTable;
                    cbLevel2.DataBind();

                    if (cbLevel2.Items.Count > 1 && !fromModule)
                    {
                        // Else add the dummy item on top
                        cbLevel2.Items.Insert(0, new ListEditItem("--Select--", ""));
                    }
                }

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string workitemType = string.Empty;
            string workitem = string.Empty;
            string userid = string.Empty;

            if (!string.IsNullOrEmpty(WorkItem))
            {
                workitemType = ModuleName;
                workitem = WorkItem;
            }
            else
            {
                workitemType = UGITUtility.ObjectToString(ddlLevel1.SelectedItem?.Value);
                workitem = UGITUtility.ObjectToString(cbLevel2.SelectedItem?.Value);
            }
            if (string.IsNullOrEmpty(UserID))
                userid = UGITUtility.ObjectToString(cmbUsers.SelectedItem?.Value);
            else
                userid = UserID;
            List<MultiAllocations> lstmultiAllocations = new List<MultiAllocations>();
            for (int i = 0; i < multiAllocationPanel.VisibleItems.Count; i++)
            {
                DataViewItem item = multiAllocationPanel.Items[i];
                ASPxCheckBox chkItemSelect = multiAllocationPanel.FindItemControl("chkItemSelect", item) as ASPxCheckBox;
                ASPxDateEdit dteStartDate = multiAllocationPanel.FindItemControl("dteStartDate", item) as ASPxDateEdit;
                ASPxDateEdit dteEndDate = multiAllocationPanel.FindItemControl("dteEndDate", item) as ASPxDateEdit;
                ASPxTextBox txtpctAllocation = multiAllocationPanel.FindItemControl("txtPctAllocation", item) as ASPxTextBox;
                ASPxComboBox cmbRoles = multiAllocationPanel.FindItemControl("cmbRoles", item) as ASPxComboBox;
                //ASPxRadioButtonList softAllocation = multiAllocationPanel.FindItemControl("rdbSoftAllocation", item) as ASPxRadioButtonList;

                double pctAllocation = UGITUtility.StringToDouble(txtpctAllocation.Text);
                string role = UGITUtility.ObjectToString(cmbRoles.SelectedItem?.Value);
                string roleName = UGITUtility.ObjectToString(cmbRoles.SelectedItem?.Text);
                string ticketid = UGITUtility.ObjectToString(cbLevel2.Value);
                int id = UGITUtility.StringToInt(chkItemSelect.Attributes["ItemId"]);
                MultiAllocations newAllocObj = new MultiAllocations() { Id = id,
                    StartDate = dteStartDate.Date, EndDate = dteEndDate.Date, PctAllocation = UGITUtility.StringToDouble(txtpctAllocation.Text), 
                    Role = role, RoleName= roleName, SoftAllocation = false, WorkItem = workitem, WorkItemType = workitemType };
                lstmultiAllocations.Add(newAllocObj);
            }

            foreach(MultiAllocations allocation in lstmultiAllocations)
            {

                List<RResourceAllocation> existingAllocation = objAllocationManager.Load(x => ((allocation.StartDate <= x.AllocationEndDate
                        && allocation.StartDate >= x.AllocationStartDate) || (allocation.EndDate <= x.AllocationEndDate && allocation.EndDate >= x.AllocationStartDate))
                        && allocation.Role == x.RoleId && x.Resource == userid && allocation.WorkItem == x.TicketID);
                if (existingAllocation != null && existingAllocation.Count > 0)
                {
                    allocation.IsDuplicate = "1";
                    pnlError.ClientVisible = true;
                    multiAllocationPanel.DataSource = lstmultiAllocations;
                    multiAllocationPanel.DataBind();
                    return;
                }
                RResourceAllocation rAllocation = new RResourceAllocation();
                rAllocation.ResourceWorkItems = new ResourceWorkItems(userid);
                rAllocation.ResourceWorkItems.WorkItemType = ModuleName;
                rAllocation.ResourceWorkItems.WorkItem = Convert.ToString(workitem);
                rAllocation.TicketID = UGITUtility.ObjectToString(workitem);
                rAllocation.ResourceWorkItems.SubWorkItem = allocation.RoleName;
                rAllocation.RoleId = allocation.Role;

                rAllocation.ResourceWorkItems.StartDate = allocation.StartDate;
                rAllocation.ResourceWorkItems.EndDate = allocation.EndDate;
                ObjWorkItemsManager.Insert(rAllocation.ResourceWorkItems);
                rAllocation.ResourceWorkItemLookup = rAllocation.ResourceWorkItems.ID;
                rAllocation.PctAllocation = allocation.PctAllocation;
                rAllocation.PctEstimatedAllocation = allocation.PctAllocation;
                rAllocation.SoftAllocation = false;

                rAllocation.AllocationStartDate = allocation.StartDate;

                rAllocation.AllocationEndDate = allocation.EndDate;
                
                rAllocation.Resource = userid;

                string allocationsavemsg = objAllocationManager.Save(rAllocation);

                //Save Same Entry in Project Estimated Allocation if allocation is at project level
                string projectid = rAllocation.ResourceWorkItems.WorkItem;
                string workitemtype = null;
                if (UGITUtility.IsValidTicketID(projectid))
                    workitemtype = uHelper.getModuleNameByTicketId(projectid);
                if (workitemtype == "PMM" || workitemtype == "NPR" || workitemtype == "CPR" ||
                    workitemtype == "CNS" || workitemtype == "OPM" || workitemtype == "TSK" || workitemtype == "Current Projects (PMM)" || workitemtype == "Project Management Module (PMM)")
                {
                    ProjectEstimatedAllocation estimatedAllocation = ObjEstimatedAllocationManager.MapIntoEstAllocationFromResourceObject(rAllocation);
                    ObjEstimatedAllocationManager.Insert(estimatedAllocation);
                    if (estimatedAllocation.ID > 0)
                    {
                        rAllocation.ProjectEstimatedAllocationId = UGITUtility.ObjectToString(estimatedAllocation.ID);
                        objAllocationManager.Update(rAllocation);

                        string userName = context.UserManager.GetUserNameById(rAllocation.Resource);
                        string historyDesc = string.Format("Created new allocation for user: {0} - {1} {2}% {3}-{4}", userName, rAllocation.ResourceWorkItems.SubWorkItem, rAllocation.PctAllocation, String.Format("{0:MM/dd/yyyy}", rAllocation.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", rAllocation.AllocationEndDate));
                        ResourceAllocationManager.UpdateHistory(context, historyDesc, rAllocation.TicketID);
                        ULog.WriteLog(historyDesc);
                    }
                }

                if (rAllocation.ResourceWorkItemLookup > 0)
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


            Session["Data"] = null;
            Session["Counter"] = null;
            uHelper.ClosePopUpAndEndResponse(HttpContext.Current, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session["Data"] = null;
            Session["Counter"] = null;
            uHelper.ClosePopUpAndEndResponse(HttpContext.Current, true);
        }

        protected void cbLevel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDateLabels();
            ClearCheckboxes();
        }

        protected void cmbRoles_Load(object sender, EventArgs e)
        {
            ASPxComboBox cbLevel3 = sender as ASPxComboBox;
            if (cbLevel3 != null)
            {
                DataTable resultedTable = AllocationTypeManager.LoadLevel3(context, ModuleNames.CPR, string.Empty, string.Empty, true, false);
                if (resultedTable != null || resultedTable.Rows.Count > 0)
                {
                    cbLevel3.ValueField = "LevelId";
                    cbLevel3.ValueType = typeof(string);
                    cbLevel3.TextField = "LevelName";

                    // Add selection placeholder
                    cbLevel3.Items.Insert(0, new DevExpress.Web.ListEditItem("--Select--", ""));

                    cbLevel3.DataSource = resultedTable;
                    cbLevel3.DataBind();                    
                }
            }
        }

        protected void ddlLevel1_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (ddlLevel1.SelectedItem != null && ddlLevel1.SelectedItem.Text != "--Select--")
            {
                DataRow[] drModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, ddlLevel1.SelectedItem.Text.Trim()));
                bool fromModule = drModules != null && drModules.Length > 0;
                if (fromModule)
                {
                    string moduleName = UGITUtility.ObjectToString(drModules[0][DatabaseObjects.Columns.ModuleName]);
                    //if (moduleName == ModuleNames.OPM)
                    //    rdbSoftAllocation.SelectedIndex = 1;
                    //else
                    //    rdbSoftAllocation.SelectedIndex = 0;
                }
                DataTable resultedTable = AllocationTypeManager.LoadLevel2(context, ddlLevel1.SelectedItem.Text, fromModule);
                if (resultedTable != null)
                {
                    bool ShowERPJobID = configVariableManager.GetValueAsBool(ConfigConstants.ShowERPJobID);
                    string ERPJobIDName = configVariableManager.GetValue(ConfigConstants.ERPJobIDName);
                    if (fromModule)
                    {
                        cbLevel2.Columns.Clear();
                        cbLevel2.Columns.Add("LevelTitle");
                        cbLevel2.Columns[0].Caption = "ID";
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

                    cbLevel2.DataSource = resultedTable;
                    cbLevel2.DataBind();

                    if (cbLevel2.Items.Count > 1 && !fromModule)
                    {
                        // Else add the dummy item on top
                        cbLevel2.Items.Insert(0, new ListEditItem("--Select--", ""));
                    }
                }

            }

            ClearCheckboxes();
        }

        private void ClearCheckboxes()
        {
            for (int i = 0; i < multiAllocationPanel.VisibleItems.Count; i++)
            {
                DataViewItem item = multiAllocationPanel.Items[i];
                ASPxCheckBox chkItemSelect = multiAllocationPanel.FindItemControl("chkItemSelect", item) as ASPxCheckBox;
                chkItemSelect.Checked = false;
            }
        }

        protected void cmbRoles_Init(object sender, EventArgs e)
        {
            ASPxComboBox cb = (ASPxComboBox)sender;
            DataViewItemTemplateContainer container = cb.NamingContainer as DataViewItemTemplateContainer;
            MultiAllocations currentItem = container.DataItem as MultiAllocations;
            cb.Attributes.Add("ItemId", DataBinder.Eval(container.DataItem, "Id").ToString());
            
            cb.Value = UGITUtility.ObjectToString(DataBinder.Eval(container.DataItem, "Role"));
            if(cb.Items?.Count > 0)
                cb.SelectedIndex = cb.Items.IndexOf(cb.Items.FindByValue(currentItem.Role));
        }

        protected void cmbUsers_Load(object sender, EventArgs e)
        {
            List<UserProfile> lstUserProfiles = context.UserManager.GetUsersProfile();
            if(lstUserProfiles != null && lstUserProfiles.Count > 0)
            {
                cmbUsers.ValueField = "Id";
                cmbUsers.ValueType = typeof(string);
                cmbUsers.TextField = "Name";

                // Add selection placeholder
                cmbUsers.Items.Insert(0, new DevExpress.Web.ListEditItem("--Select--", ""));

                cmbUsers.DataSource = lstUserProfiles.OrderBy(x=>x.Name);
                cmbUsers.DataBind();
            }
        }
    }

    public class MultiAllocations
    {
        public int Id { get; set; }
        public string WorkItemType { get; set; }
        public string WorkItem { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Role { get; set; }
        public string RoleName { get; set; }
        public double? PctAllocation { get; set; }
        public bool SoftAllocation { get; set; }
        public string IsDuplicate { get; set; } = "0";
    }
}