
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using DevExpress.Web;
using System.Threading;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Web;
using uGovernIT.DAL;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity.Core.Common.CommandTrees;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class ModuleResourceAddEdit : UserControl
    {
        public int nprId { get; set; }
        public int resourceId { get; set; }
        public string TicketID { get; set; }
        public string ModuleName { get; set; }
        private long budgetId { get; set; }
        protected double WorkingHours { get; set; }
        protected double NoOfWorkingDays { get; set; }
        public UserProfileManager UserManager;
        NPRResourcesManager objNPRResourcesManager = new NPRResourcesManager(HttpContext.Current.GetManagerContext());
        BudgetCategoryViewManager budgetCategoryViewManager = new BudgetCategoryViewManager(HttpContext.Current.GetManagerContext());
        ModuleBudgetManager objModuleBudgetManager = new ModuleBudgetManager(HttpContext.Current.GetManagerContext());
       ApplicationContext context= HttpContext.Current.GetManagerContext();
        ConfigurationVariableManager ConfigVariableHelper;
        List<string> userIds = new List<string>();
        protected override void OnInit(EventArgs e)
        {
            UserManager = context.UserManager;
            ConfigVariableHelper = new ConfigurationVariableManager(context);
            int id = 0;
            int.TryParse(Request["NPRID"], out id);
            nprId = id;
            TicketID = Request["TicketID"];
            ModuleName = Request["ModuleName"];
            WorkingHours = uHelper.GetWorkingHoursInADay(context);
            id = 0;
            int.TryParse(Request["ID"], out id);
            resourceId = id;
            FillCategory();

            // peUsers.AfterCallbackClientScript = string.Format("FillOnUserFieldChange(\"{0}\")", peUsers.ID);
            if (!chkbxCreatebudget.Checked)
            {
                divDDLBudgetCategories.Style.Add("display", "block");;
            }
            else
            {
                divDDLBudgetCategories.Style.Add("display", "none");
            }
            base.OnInit(e);
        }
        DataTable GetActionUser()
        {
            DataTable dtGroups = new DataTable();
            dtGroups.Columns.Add("ID");
            dtGroups.Columns.Add("Name");
            dtGroups.Columns.Add("NameRole");
            dtGroups.Columns.Add("UserType");
            dtGroups.Columns.Add("Type");
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            EnumerableRowCollection<DataRow> rows = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserTypes, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").AsEnumerable().Where(x => !string.IsNullOrEmpty(x.Field<string>(DatabaseObjects.Columns.ColumnName)) && x.Field<string>(DatabaseObjects.Columns.ModuleNameLookup) == "NPR");
            if (rows != null && rows.Count() > 0)
            {
                DataTable dtUserTypes = rows.CopyToDataTable();
                foreach (DataRow drUserTypes in dtUserTypes.Rows)
                {
                    DataRow dr = dtGroups.NewRow();
                    dr["ID"] = drUserTypes[DatabaseObjects.Columns.Id];
                    dr["Name"] = drUserTypes[DatabaseObjects.Columns.ColumnName];
                    dr["UserType"] = drUserTypes[DatabaseObjects.Columns.UserTypes];
                    dr["NameRole"] = string.Format("{0} ({1})", drUserTypes[DatabaseObjects.Columns.UserTypes], drUserTypes[DatabaseObjects.Columns.ColumnName]);
                    dr["Type"] = "User";
                    dtGroups.Rows.Add(dr);
                }
            }
            return dtGroups;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            FillSkills();
            if (!IsPostBack)
            {
                //rfvSkill.ControlToValidate = cbSkill.devexListBox.ID;
                RequiredFieldValidator5.ControlToValidate = dtcStartDate.ID;
                CompareValidator1.ControlToValidate = dtcStartDate.ID;
                RequiredFieldValidator6.ControlToValidate = dtcEndDate.ID;
                CompareValidator2.ControlToValidate = dtcEndDate.ID;
              //  CustomValidator1.ControlToValidate = dtcEndDate.ID;
                RequiredFieldValidator1.ControlToValidate = cbCategory.devexListBox.ID;
                addNewResourceItem.Visible = true;
                if (resourceId > 0)
                {
                    addNewResourceItem.Visible = false;
                    DataTable resourceList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.NPRResources, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                    DataRow item = resourceList.Select(string.Format("{0}={1}", DatabaseObjects.Columns.Id, resourceId))[0];
                    //cbSkill.SetValues(Convert.ToString(item[DatabaseObjects.Columns.UserSkillLookup]));
                    cbSkill.Value = Convert.ToString(item[DatabaseObjects.Columns.UserSkillLookup]);
                    cbCategory.SetValues(Convert.ToString(item[DatabaseObjects.Columns.BudgetTypeChoice]));
                    txtDescription.Text = Convert.ToString(item[DatabaseObjects.Columns._ResourceType]);
                    txtFTEs.Text = Convert.ToString(item[DatabaseObjects.Columns.TicketNoOfFTEs]);
                    txtResourceHourlyRate.Text = Convert.ToString(item[DatabaseObjects.Columns.ResourceHourlyRate]);
                    SetTotalBudgetAmount();
                    txtNotes.Text = Convert.ToString(item[DatabaseObjects.Columns.BudgetDescription]);
                    if (item[DatabaseObjects.Columns.EstimatedHours] != null)
                        txtHrs.Text = Convert.ToString(item[DatabaseObjects.Columns.EstimatedHours]);
                    if (item[DatabaseObjects.Columns.RequestedResources] != null)
                        peUsers.SetValues(Convert.ToString(item[DatabaseObjects.Columns.RequestedResources]));
                    else
                        peUsers = null;
                    DateTime date;
                    if (item[DatabaseObjects.Columns.AllocationStartDate] != null)
                    {
                        try
                        {
                            date = Convert.ToDateTime(Convert.ToString(item[DatabaseObjects.Columns.AllocationStartDate]));
                        }
                        catch { date = DateTime.Today; }
                    }
                    else date = DateTime.Today;
                    dtcStartDate.Date = date;
                    ddlRoles.SetValues(Convert.ToString(item[DatabaseObjects.Columns.RoleNameChoice]));
                    if (item[DatabaseObjects.Columns.AllocationEndDate] != null)
                    {
                        try
                        {
                            date = Convert.ToDateTime(Convert.ToString(item[DatabaseObjects.Columns.AllocationEndDate]));
                        }
                        catch
                        {
                            date = DateTime.Today;
                        }
                    }
                    else
                    {
                        date = DateTime.Today;
                    }
                    dtcEndDate.Date = date;
                    GetNoOfWorkingDays();


                }
                else
                {
                    dtcEndDate.Date = DateTime.Today;//dtcEndDate.SelectedDate = DateTime.Today;
                    dtcStartDate.Date = DateTime.Today;

                }
                
                updateResourceItem.Visible = !addNewResourceItem.Visible;
                // lnkDelete.Visible = !addNewResourceItem.Visible;
            }
            GetNoOfWorkingDays();
            //GetFTESkillUserData();
            //if (userIds.Distinct().Count() > 0)
            //{
            //    //var selectedResource = gridAllocation.GetSelectedFieldValues("ResourceId");
            //    //if (selectedResource.Count > 0)
            //    //    lnkTotalSkillFTECount.Text = UGITUtility.ObjectToString(selectedResource.Count);
            //    //else
            //        lnkTotalSkillFTECount.Text = UGITUtility.ObjectToString(userIds.Distinct().Count());
            //}

        }

        public void FillSkills()
        {
            FieldConfigurationManager fieldManager = new FieldConfigurationManager(context);
            DataTable dt = fieldManager.GetFieldDataByFieldName(DatabaseObjects.Columns.UserSkillLookup, "", "", context.TenantID);
            cbSkill.DataSource = dt;
            cbSkill.TextField = "Title";
            cbSkill.ValueField = "ID";
            cbSkill.DataBind();
        }
        public class ObjUserSkill
        {
            public string SkillName { get; set; }
            public Int32 SkillID { get; set; }
        }
        private double GetResourceHourlyRate()
        {
            int totalHourlyRate = 0;
            if (!string.IsNullOrEmpty(peUsers.GetValues()))
            {
                string entity = UGITUtility.ConvertStringToList(peUsers.GetValues(), ",")[0];
                if (entity != null)
                {
                    UserProfile user = UserManager.GetUserById(entity);
                    if (user != null)
                        totalHourlyRate = user.HourlyRate;
                }

            }
            return totalHourlyRate;
        }
        private List<string> GetSelectedUsers()
        {
            List<string> selectedEnties = new List<string>();
            //    SPFieldUserValueCollection selectedEnties = new SPFieldUserValueCollection();
            //    try
            //    {
            //        foreach (string entity in peUsers.GetValues())
            //        {
            //            if (entity)
            //            {
            //                SPFieldUserValue userValue = new SPFieldUserValue(SPContext.Current.Web);
            //                userValue.LookupId = Convert.ToInt32(entity.EntityData["SPUserID"]);
            //                selectedEnties.Add(userValue);
            //            }
            //        }
            //    }
            //    catch (Exception)
            //    {
            //    }
            //    return selectedEnties;
            return selectedEnties;
        }

        protected void btnSkill_Click(object sender, ImageClickEventArgs e)
        {
            hdnFTESkill.Value = "";
            lblSkillErrorMessage.Visible = false;
            //var selectedUsers = GetSelectedUsers();
            //foreach (var user in selectedUsers)
            //    gridAllocation.Selection.SelectRowByKey(user.LookupId);

            grdSkill.ShowOnPageLoad = true;
            gridAllocation.DataBind();

            if (gridAllocation.VisibleRowCount > 0)
            {
                dxUpdateSkill.Visible = true;
            }
            else
            {
                dxUpdateSkill.Visible = false;
            }
        }

        public class Allocation
        {
            public string ResourceId { get; set; }
            public string Resource { get; set; }
            public string UserSkillLookup { get; set; }
            public double UserFullAllocation { get; set; }
            public string FullAllocation { get; set; }

            public double TotalFTE { get; set; }
        }

        private List<Allocation> GetSkillUserData()
        {
            ResourceAllocationManager allocManager = new ResourceAllocationManager(context);
            List<Allocation> listAllocation = new List<Allocation>();
            List<UserProfile> lstUserProfile = UserManager.GetUsersProfile();
            string selectedSkill = UGITUtility.ObjectToString(cbSkill.SelectedItem.Value); //cbSkill.GetValues() == null ? "-1" : cbSkill.GetValues();
            foreach (UserProfile user in lstUserProfile)
            {
                if (!string.IsNullOrEmpty(user.Skills))
                {
                    string[] skills = UGITUtility.SplitString(user.Skills, Constants.Separator6);

                    Allocation dataAllocation = new Allocation();
                    double totalPctAllocation = allocManager.AllocationPercentage(user.Id, 4);
                    dataAllocation.ResourceId = user.Id;
                    UserProfile profile = UserManager.LoadById(user.Id);
                    if (profile != null)
                        dataAllocation.Resource = profile.Name;
                    dataAllocation.UserSkillLookup = user.Skills;
                    dataAllocation.UserFullAllocation = totalPctAllocation;
                    dataAllocation.FullAllocation = CreateAllocationBar(totalPctAllocation);
                    listAllocation.Add(dataAllocation);
                }
            }
            
            return listAllocation.OrderBy(x => x.UserFullAllocation).ToList();
        }
        private string CreateAllocationBar(double pctAllocation)
        {
            StringBuilder bar = new StringBuilder();
            double percentage = 0;
            string progressBarClass = "progressbar";
            string empltyProgressBarClass = "emptyProgressBar";
            percentage = pctAllocation;
            if (percentage > 100)
            {
                progressBarClass = "progressbarhold";
                bar.AppendFormat("<div style='position:relative;'><strong style='position:absolute;font-size:9px;left:2px;width:95%;text-align:center;top:1px;'>{2}%</strong><div class='{0}' style='float:left; width:95%;'><div class='{1}' style='float:left; width:100%;'><b>&nbsp;</b></div></div></div>", empltyProgressBarClass, progressBarClass, percentage);
            }
            else
            {
                bar.AppendFormat("<div style='position:relative;'><strong style='position:absolute;font-size:9px;left:2px;width:95%;text-align:center;top:1px;'>{2}%</strong><div class='{0}' style='float:left; width:95%;'><div class='{1}' style='float:left; width:{2}%;'><b>&nbsp;</b></div></div></div>", empltyProgressBarClass, progressBarClass, percentage);
            }

            return bar.ToString();
        }

        protected void gridAllocation_DataBinding(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(cbSkill.SelectedItem.Value)))//if (!string.IsNullOrEmpty(cbSkill.GetValues()) && cbSkill.GetValues() != "0")
            {
                gridAllocation.DataSource = GetFTESkillUserData();
            }
            else
            {
                //if (gridAllocation.DataSource == null)
                //{
                gridAllocation.DataSource = GetSkillUserData();
                //}
            }
        }

        void BindAllocation()
        {
            gridAllocation.DataSource = GetSkillUserData();
        }

        protected void dxUpdateSkill_Click(object sender, EventArgs e)
        {
            var selectedResource = gridAllocation.GetSelectedFieldValues("ResourceId");
            List<string> users = new List<string>();
            foreach (var usrItm in selectedResource)
            {
                users.Add(UGITUtility.ObjectToString(usrItm));
            }
            if (selectedResource.Count != 0)
                peUsers.SetValues(UGITUtility.ConvertListToString(users, Constants.Separator6));
            else
                peUsers = null;
            grdSkill.ShowOnPageLoad = false;
            txtFTEs.Text = UGITUtility.ObjectToString(selectedResource.Count);
            lnkTotalSkillFTECount.Text = getFTECounts(UGITUtility.ObjectToString(cbSkill.SelectedItem.Value));
        }

        protected void addNewResourceItem_Click(object sender, EventArgs e)
        {
            insertUpdateResource();
        }

        protected void peUsers_OnLoad(object sender, EventArgs e)
        {
            peUsers.UserTokenBoxAdd.CssClass = "dxeButtonEditSys dxeButtonEdit_UGITNavyBlueDevEx";
        }

        protected void updateResourceItem_Click(object sender, EventArgs e)
        {
            insertUpdateResource();
        }
        private void SaveNPRBudget(long id, int resourceId)
        {
            ModuleBudget _nprBudget = new ModuleBudget();
            BudgetCategoryViewManager budgetCategoryViewManager = new BudgetCategoryViewManager(context);
            BudgetCategory budgetCategory= budgetCategoryViewManager.GetBudgetCategoryById(Convert.ToInt64(ddlBudgetCategories.SelectedValue));
            _nprBudget.TicketId = TicketID;
            _nprBudget.ModuleName = ModuleName;
            _nprBudget.BudgetCategoryLookup = Convert.ToInt64(ddlBudgetCategories.SelectedValue);
            _nprBudget.ResourceLookup = resourceId;
            double amount;
            if (lblTotalResourceBudget != null && double.TryParse(lblTotalResourceBudget.Text, out amount))
            {
                _nprBudget.BudgetAmount = amount;
            }
            if (txtNotes != null)
            {
                _nprBudget.BudgetDescription = txtNotes.Text;
            }
            if (txtDescription != null)
            {
               _nprBudget.Title = _nprBudget.BudgetItem = txtDescription.Text;
            }
            if (dtcStartDate.Date != null)
            {
                _nprBudget.AllocationStartDate = dtcStartDate.Date;
            }

            if (dtcEndDate.Date != null)
            {
                _nprBudget.AllocationEndDate = dtcEndDate.Date;
            }

            ModuleBudget budgetItem =  objModuleBudgetManager.InsertORUpdateData(_nprBudget);
            objModuleBudgetManager.UpdateProjectMonthlyDistribution(context,false, budgetItem, TicketID,ModuleName);
        

            uHelper.ClosePopUpAndEndResponse(Context, true, "control=modulebudget");
        }
        private void insertUpdateResource()
        {
            NPRResource _nprResources = new NPRResource();
            //Check Skill and Add
            int selectedSkillID = 0;
            if (UGITUtility.ObjectToString(cbSkill.SelectedItem.Value) == null)
            {
                List<ObjUserSkill> skills = new List<ObjUserSkill>();
                DataTable userSkillList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.UserSkills, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                foreach (DataRow itemSkill in userSkillList.Rows)
                {
                    skills.Add(new ObjUserSkill { SkillName = Convert.ToString(itemSkill[DatabaseObjects.Columns.Title]), SkillID = Convert.ToInt32(itemSkill[DatabaseObjects.Columns.ID]) });
                }
                var skill = skills.Where(x => x.SkillName.Equals(UGITUtility.ObjectToString(cbSkill.SelectedItem.Value)));
                if (skill.Count() == 0)
                {
                    DataRow spUserSkillItem = userSkillList.NewRow();
                    spUserSkillItem[DatabaseObjects.Columns.Title] = UGITUtility.ObjectToString(cbSkill.SelectedItem.Value);
                    //spUserSkillItem.Update();
                    //selectedSkillID = spUserSkillItem.ID;
                }
                else selectedSkillID = skill.FirstOrDefault().SkillID;
            }
            else
                selectedSkillID = Convert.ToInt32(UGITUtility.ObjectToString(cbSkill.SelectedItem.Value));
            DataRow nprTicket = Ticket.GetCurrentTicket(context,"NPR", TicketID);
            DataTable resourceList = objNPRResourcesManager.LoadNprResources();
            if (resourceId > 0)
            {
                _nprResources = objNPRResourcesManager.LoadByID(resourceId);
                if (_nprResources.UserSkillLookup == 0)
                    _nprResources.UserSkillLookup = 0;
            }
            _nprResources.UserSkillLookup = selectedSkillID;
            _nprResources.BudgetTypeChoice = cbCategory.GetValues();
            _nprResources.NoOfFTEs = UGITUtility.StringToDouble(txtFTEs.Text.Trim());
            _nprResources.AllocationStartDate = dtcStartDate.Date;
            _nprResources.AllocationEndDate = dtcEndDate.Date;
            _nprResources.BudgetDescription = txtNotes.Text.Trim();
            _nprResources.TicketId = TicketID;
            _nprResources.ModuleNameLookup = ModuleName;
            _nprResources.EstimatedHours = UGITUtility.StringToInt(txtHrs.Text.Trim());
            _nprResources._ResourceType=_nprResources.Title= string.IsNullOrEmpty(txtDescription.Text.Trim()) ? cbSkill.SelectedItem.Text : txtDescription.Text.Trim();

            if (ddlRoles.GetValues() != null)
            {
                string roleName = ddlRoles.GetValues();
                // item[DatabaseObjects.Columns.RoleName] = roleName;
                _nprResources.RoleNameChoice = roleName;
            }
            
            _nprResources.RequestedResourcesUser = peUsers.GetValues();
            _nprResources.HourlyRate = Convert.ToDecimal(txtResourceHourlyRate.Text);
            objNPRResourcesManager.InsertORUpdateData(_nprResources);

            objNPRResourcesManager.AddNPRMonthlyDistributionResource(_nprResources.TicketId, _nprResources.BudgetTypeChoice, _nprResources.ModuleNameLookup);

            #region npr planned allocation
            UGITTaskManager taskManager = new UGITTaskManager(context);
            List<UGITTask> ptasks = taskManager.LoadByProjectID("NPR", Convert.ToString(nprTicket[DatabaseObjects.Columns.TicketId]));

            if (ptasks == null || ptasks.Count == 0)
            {
                List<string> requestedUserList = UGITUtility.ConvertStringToList(UGITUtility.ObjectToString(_nprResources.RequestedResourcesUser), Constants.Separator6);
                
                bool autoCreateRMMProjectAllocation = ConfigVariableHelper.GetValueAsBool(ConfigConstants.AutoCreateRMMProjectAllocation);
                if (autoCreateRMMProjectAllocation)
                {
                    
                        List<UGITTask> nprTaskList = LoadNPRResourceList();
                        ResourceAllocationManager allocationManager = new ResourceAllocationManager(context);
                        ThreadStart threadStartMethodUpdateProjectPlannedAllocation = delegate () { allocationManager.UpdateProjectPlannedAllocation(string.Empty, nprTaskList, requestedUserList, "NPR", Convert.ToString(nprTicket[DatabaseObjects.Columns.TicketId]), false); };
                        Thread sThreadUpdateProjectPlannedAllocation = new Thread(threadStartMethodUpdateProjectPlannedAllocation);
                        sThreadUpdateProjectPlannedAllocation.IsBackground = true;
                        sThreadUpdateProjectPlannedAllocation.Start();
                    
                }
            }
            #endregion npr planned allocation

            UpdateProjectStartEndDate();
            if (chkbxCreatebudget.Checked)
                SaveNPRBudget(budgetId, Convert.ToInt32(_nprResources.ID));
            else
            {
                if (budgetId > 0)
                    SaveNPRBudget(budgetId, 0);
            }

            uHelper.ClosePopUpAndEndResponse(Context, true, "control=modulebudget");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        //protected void lnkDelete_Click(object sender, EventArgs e)
        //{
        //    SPList resourceList = SPListHelper.GetSPList(DatabaseObjects.Lists.NPRResources);
        //    SPQuery rQuery = new SPQuery();
        //    rQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Number'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.Id, resourceId);
        //    SPListItemCollection nprResources = resourceList.GetItems(rQuery);

        //    DataRow drnprresourceItem = nprResources.GetDataTable().Rows[0];
        //    SPListItem nprTicket = Ticket.getCurrentTicket("NPR", nprId.ToString());
        //    List<UGITTask> ptasks = UGITTaskHelper.LoadByProjectID(SPContext.Current.Web, "NPR", Convert.ToString(nprTicket[DatabaseObjects.Columns.TicketId]));

        //    // Only delete if task count is zero
        //    if (ptasks != null && ptasks.Count == 0)
        //    {
        //        UGITTask tsk = new UGITTask();
        //        tsk.ID = Convert.ToInt16(nprResources[0][DatabaseObjects.Columns.Id]);
        //        tsk.Title = uHelper.GetLookupValue(Convert.ToString(nprResources[0][DatabaseObjects.Columns.UserSkillLookup]));
        //        tsk.StartDate = Convert.ToDateTime(nprResources[0][DatabaseObjects.Columns.AllocationStartDate]);
        //        tsk.DueDate = Convert.ToDateTime(nprResources[0][DatabaseObjects.Columns.AllocationEndDate]);
        //        tsk.EstimatedHours = Convert.ToInt16(nprResources[0][DatabaseObjects.Columns.EstimatedHours]);
        //        tsk.PercentageComplete = Convert.ToDouble(nprResources[0][DatabaseObjects.Columns.TicketNoOfFTEs]);

        //        List<int> existingUsers = new List<int>();
        //        if (tsk.AssignedTo != null)
        //            existingUsers = tsk.AssignedTo.Select(x => x.LookupId).ToList();
        //        RMMSummaryHelper.DeleteAllocationsByTask(SPContext.Current.Web, ptasks, existingUsers, "NPR", Convert.ToString(nprTicket[DatabaseObjects.Columns.TicketId]));
        //    }
        //    nprResources[0].Delete();
        //    UpdateProjectStartEndDate();
        //    NPRBudget.UpdateNPRMonthlyDistributionResource(drnprresourceItem, null);

        //    uHelper.ClosePopUpAndEndResponse(Context, true);
        //}

        //private List<UGITTask> LoadNPRResourceList()
        //{
        //    List<UGITTask> nprTasks = new List<UGITTask>();
        //    SPList resourceList = SPListHelper.GetSPList(DatabaseObjects.Lists.NPRResources);
        //    SPQuery rQuery = new SPQuery();
        //    rQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.TicketNPRIdLookup, nprId);
        //    SPListItemCollection nprResources = resourceList.GetItems(rQuery);

        //    if (nprResources != null && nprResources.Count > 0)
        //    {
        //        foreach (SPListItem spItem in nprResources)
        //        {
        //            UGITTask tsk = new UGITTask();

        //            tsk.ID = Convert.ToInt16(nprResources[0][DatabaseObjects.Columns.Id]);
        //            tsk.Title = uHelper.GetLookupValue(Convert.ToString(nprResources[0][DatabaseObjects.Columns.UserSkillLookup]));
        //            tsk.StartDate = Convert.ToDateTime(spItem[DatabaseObjects.Columns.AllocationStartDate]);
        //            tsk.DueDate = Convert.ToDateTime(spItem[DatabaseObjects.Columns.AllocationEndDate]);
        //            tsk.EstimatedHours = Convert.ToInt16(spItem[DatabaseObjects.Columns.EstimatedHours]);
        //            tsk.PercentageComplete = Convert.ToDouble(spItem[DatabaseObjects.Columns.TicketNoOfFTEs]);

        //            SPFieldUserValueCollection userLookups = new SPFieldUserValueCollection(SPContext.Current.Web, Convert.ToString(spItem[DatabaseObjects.Columns.RequestedResources]));
        //            if (userLookups != null)
        //                tsk.AssignedTo = userLookups;

        //            nprTasks.Add(tsk);
        //        }
        //    }
        //    return nprTasks;
        //}



        public void UpdateProjectStartEndDate()
        {
            UGITTaskManager objUGITTaskManager = new UGITTaskManager(context);
            ModuleViewManager objmodule = new ModuleViewManager(context);
            TicketManager objTicketManager = new TicketManager(context);
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", context.TenantID);
            DataRow nprTicket = Ticket.GetCurrentTicket(context, "NPR",TicketID);
            List<UGITTask> nprTasks = objUGITTaskManager.LoadByProjectID("NPR", Convert.ToString(UGITUtility.GetSPItemValue(nprTicket, DatabaseObjects.Columns.TicketId)));
            if (nprTasks.Count == 0)
            {
                DataTable dtNprResource = GetTableDataManager.GetData(DatabaseObjects.Tables.NPRResources, values);
                if (dtNprResource != null && dtNprResource.Rows.Count > 0)
                {
                    DataRow[] nprResourcesColl = dtNprResource.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, TicketID));
                    if (nprResourcesColl != null)
                    {
                        dtNprResource = nprResourcesColl.CopyToDataTable();
                        DateTime minDate = dtNprResource.AsEnumerable().Min(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate));
                        DateTime maxDate = dtNprResource.AsEnumerable().Max(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate));
                        nprTicket[DatabaseObjects.Columns.TicketActualStartDate] = minDate;
                        nprTicket[DatabaseObjects.Columns.TicketActualCompletionDate] = maxDate;
                        int result = objTicketManager.Save(objmodule.LoadByName(ModuleNames.NPR), nprTicket);
                        if (result <= 0)
                            ULog.WriteException("NPR Budget Fail: actualstartdate and completetiondate not saved!");
                    }
                    else
                    {
                        nprTicket[DatabaseObjects.Columns.TicketActualStartDate] = null;
                        nprTicket[DatabaseObjects.Columns.TicketActualCompletionDate] = null;
                        int result = objTicketManager.Save(objmodule.LoadByName(ModuleNames.NPR), nprTicket);
                        if (result <= 0)
                            ULog.WriteException("NPR Budget Fail: actualstartdate and completetiondate not saved!");
                    }
                }

            }
        }
        private List<Allocation> GetFTESkillUserData()
        {
            UserSkillManager skillManagerObj = new UserSkillManager(context);
            List<Allocation> listAllocation = new List<Allocation>();
            
            List<UserProfile> lstUserProfile = UserManager.GetUsersProfile();
            string selectedSkill = UGITUtility.ObjectToString(cbSkill.SelectedItem.Value); //cbSkill.GetValues() == null ? "-1" : cbSkill.GetValues();
            UserSkills selectedskillObj = skillManagerObj.LoadByID(UGITUtility.StringToLong(selectedSkill));
            if (selectedskillObj != null)
            {
                foreach (UserProfile user in lstUserProfile)
                {
                    if (user.Skills != null)
                    {
                        string[] skills = UGITUtility.SplitString(user.Skills, Constants.columnSeprator); // user.Skills.Select(x => x.Value).ToArray();
                        foreach (string item in skills)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                UserSkills skill = skillManagerObj.LoadByID(UGITUtility.StringToLong(item));
                                if (skill != null && skill.Title == selectedskillObj.Title)
                                {
                                    userIds.Add(user.Id);
                                }
                            }
                        }
                    }
                }
            }
            userIds = userIds.Distinct().ToList();
            
            foreach (string userID in userIds)
            {
                Allocation dataAllocation = new Allocation();
                double totalPctAllocation = 8.0;//ResourceAllocation.AllocationPercentage(userID, 4);

                if (totalPctAllocation < 100)
                {
                    //dataAllocation.ResourceId = userID;
                    UserProfile profile = UserManager.LoadById(userID);
                    if (profile != null)
                    {
                        dataAllocation.ResourceId = profile.Id;
                        dataAllocation.Resource = profile.Name;
                        dataAllocation.UserSkillLookup = profile.Skills;
                        dataAllocation.UserFullAllocation = totalPctAllocation;
                        dataAllocation.FullAllocation = CreateAllocationBar(totalPctAllocation);
                        dataAllocation.TotalFTE = 100 - totalPctAllocation;
                        listAllocation.Add(dataAllocation);
                    }
                }
            }
            return listAllocation.OrderBy(x => x.UserFullAllocation).ToList();
        }

        protected void lnkTotalSkillFTECount_Click(object sender, EventArgs e)
        {
            grdSkill.ShowOnPageLoad = true;
            hdnFTESkill.Value = "Skill";
            gridAllocation.DataBind();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            double totalHourlyRate = GetResourceHourlyRate();
            txtResourceHourlyRate.Text = Convert.ToString(totalHourlyRate);
            SetTotalBudgetAmount();
        }

        private void SetTotalBudgetAmount()
        {
            if (!string.IsNullOrEmpty(txtFTEs.Text) && !string.IsNullOrEmpty(txtResourceHourlyRate.Text))
            {
                double fte = Convert.ToDouble(txtFTEs.Text);
                double resourceHourlyRate = Convert.ToDouble(txtResourceHourlyRate.Text);
                if (fte != 0 && resourceHourlyRate != 0 && WorkingHours != 0 && NoOfWorkingDays != 0)
                    lblTotalResourceBudget.Text = Convert.ToString(fte * resourceHourlyRate * WorkingHours * NoOfWorkingDays);
            }
        }

        private void FillCategory()
        {
            DataTable resultedTable = budgetCategoryViewManager.LoadCategories();// BudgetCategory.LoadCategories();
            List<ListItem> items = budgetCategoryViewManager.LoadCategoriesDropDownItems(resultedTable);//BudgetCategory.LoadCategoriesDropDownItems(resultedTable);
            foreach (ListItem item in items)
            {
                ddlBudgetCategories.Items.Add(item);
            }
            ListItem itemSelect = new ListItem("--Select--", "0");
            ddlBudgetCategories.Items.Insert(0, itemSelect);
        }

        private void GetNoOfWorkingDays()
        {
            DateTime endDate = dtcEndDate.Date;
            DateTime startDate = dtcStartDate.Date;
            // NoOfWorkingDays = 1;
            if (endDate != DateTime.MinValue && startDate != DateTime.MinValue)
            {
                NoOfWorkingDays = (endDate.Subtract(startDate).Days) + 1;
            }
            SetTotalBudgetAmount();
            if (chkbxCreatebudget.Checked)
                divDDLBudgetCategories.Style.Add("display", "block");
        }

        private List<UGITTask> LoadNPRResourceList()
        {
            List<UGITTask> nprTasks = new List<UGITTask>();
            //NPRResourcesManager 
            //SPList resourceList = SPListHelper.GetSPList(DatabaseObjects.Lists.NPRResources);
            //SPQuery rQuery = new SPQuery();
            //rQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.TicketNPRIdLookup, nprId);
            //SPListItemCollection nprResources = resourceList.GetItems(rQuery);
            List<NPRResource> nprResources = objNPRResourcesManager.Load(x => x.TicketId == TicketID);

            if (nprResources != null && nprResources.Count > 0)
            {
                foreach (NPRResource spItem in nprResources)
                {
                    UGITTask tsk = new UGITTask();

                    //tsk.ID = uHelper.StringToInt(nprResources[0][DatabaseObjects.Columns.Id]);
                    tsk.Title =  UGITUtility.ObjectToString(nprResources.First().UserSkillLookup);  // uHelper.GetLookupValue(Convert.ToString(nprResources[0][DatabaseObjects.Columns.UserSkillLookup]));
                    tsk.StartDate = UGITUtility.StringToDateTime(spItem.AllocationStartDate);
                    tsk.DueDate = UGITUtility.StringToDateTime(spItem.AllocationEndDate);
                    tsk.EstimatedHours = UGITUtility.StringToDouble(spItem.EstimatedHours);
                    tsk.PercentComplete = UGITUtility.StringToDouble(spItem.NoOfFTEs); 
                    string userLookups = Convert.ToString(spItem.RequestedResourcesUser);
                    if (userLookups != null)
                        tsk.AssignedTo = userLookups;
                    string userLookupAssignedPtc = string.Empty;
                    if (!string.IsNullOrEmpty(userLookups))
                    {
                        List<string> lstuserLookups = UGITUtility.SplitString(userLookups, Constants.Separator6).ToList();
                        foreach (string lookup in lstuserLookups)
                        {
                            userLookupAssignedPtc += lookup + Constants.Separator1 + "100" + Constants.Separator;
                        }
                    }
                    tsk.AssignToPct = userLookupAssignedPtc;
                    tsk.Duration = uHelper.GetTotalWorkingDaysBetween(context, tsk.StartDate, tsk.DueDate);
                    nprTasks.Add(tsk);
                }
            }
            return nprTasks;
        }

        protected void gridAllocation_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            FieldConfigurationManager fieldManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
            if (e.Column.FieldName.Contains("Lookup"))
            {
                string lookupid = Convert.ToString(e.Value);
                string values = fieldManager.GetFieldConfigurationData(e.Column.FieldName, Convert.ToString(e.Value));
                if (!string.IsNullOrEmpty(values))
                {
                    e.DisplayText = values;
                }
            }
            if (e.Column.FieldName.EndsWith("User"))
            {
                string userIDs = Convert.ToString(e.Value);
                if (!string.IsNullOrEmpty(userIDs))
                {
                    if (userIDs != null)
                    {
                        string separator = Constants.Separator6;
                        if (userIDs.Contains(Constants.Separator))
                            separator = Constants.Separator;
                        List<string> userlist = UGITUtility.ConvertStringToList(userIDs, separator);

                        string commanames = UserManager.CommaSeparatedNamesFrom(userlist, Constants.Separator6);
                        e.DisplayText = !string.IsNullOrEmpty(commanames) ? commanames : string.Empty;
                    }
                }
            }
        }

        

        protected void cbSkillcallback_Callback(object source, CallbackEventArgs e)
        {
            string selectedSkill = UGITUtility.ObjectToString(cbSkill.SelectedItem.Value);
            e.Result = getFTECounts(selectedSkill);
            //lnkTotalSkillFTECount.Text = UGITUtility.ObjectToString(userIds.Distinct().Count());
        }
        public string getFTECounts(string skillid)
        {
            UserSkillManager skillManagerObj = new UserSkillManager(context);
            List<UserProfile> lstUserProfile = UserManager.GetUsersProfile();
            
            UserSkills selectedskillObj = skillManagerObj.LoadByID(UGITUtility.StringToLong(skillid));
            if (selectedskillObj != null)
            {
                foreach (UserProfile user in lstUserProfile)
                {
                    if (user.Skills != null)
                    {
                        string[] skills = UGITUtility.SplitString(user.Skills, Constants.columnSeprator); // user.Skills.Select(x => x.Value).ToArray();
                        foreach (string item in skills)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                UserSkills skill = skillManagerObj.LoadByID(UGITUtility.StringToLong(item));
                                if (skill != null && skill.Title == selectedskillObj.Title)
                                {
                                    userIds.Add(user.Id);
                                }
                            }
                        }
                    }
                }
            }
            userIds = userIds.Distinct().ToList();
            if (userIds.Count > 0)
                return UGITUtility.ObjectToString(userIds.Distinct().Count());
            else
                return "0";
        }

    }
}
