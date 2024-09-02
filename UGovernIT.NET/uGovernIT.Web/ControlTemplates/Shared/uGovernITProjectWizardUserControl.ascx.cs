using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Web;
using uGovernIT.DAL;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity.Owin;
namespace uGovernIT.Web
{
    public partial class uGovernITProjectWizardUserControl : UserControl
    {
        protected bool refreshPage = false;
        protected string errorMessage = "";
        string[] stepHeadings = { "Select NPR ticket to import", "Basic Project Details", "Project Lifecycle & Schedule", "Summary" };
        // SPListItem nprItem = null;
        DataRow[] nprItem = null;
        bool isFromNPR;
        public string TicketId { get; set; }
        public static List<bool> lstCheckbox = new List<bool>();
        protected string ajaxHelperURL = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ajaxhelper.aspx");
        public string ticketStage;
        public int ticketStageId;
        List<TicketColumnError> errors = new List<TicketColumnError>();
        protected Ticket TicketRequest;
        public static string pmmsponser = string.Empty;
        public static string pmmmanager = string.Empty;
        public static string projectName = string.Empty;
        public static int ddlvalue = 0;
        public string error = string.Empty;
        UserProfile User;
        UserProfileManager UserManager;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ProjectMonitorStateManager MonitorStateManagerObj = new ProjectMonitorStateManager(HttpContext.Current.GetManagerContext());
        ModuleMonitorOptionManager MonitorOptionManagerObj = new ModuleMonitorOptionManager(HttpContext.Current.GetManagerContext());
        ConfigurationVariableManager objConfigurationVariableHelper = null;
        ModuleViewManager ObjModuleViewManager = null;
        protected override void OnInit(EventArgs e)
        {
            objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            ObjModuleViewManager = new ModuleViewManager(context);

            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonitors, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            if (dt != null && dt.Rows.Count > 0)
            {
                monitorsRepeater.DataSource = dt;
                monitorsRepeater.DataBind();
                //  pMonitors.Visible = true;
            }
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            User = HttpContext.Current.CurrentUser();
            UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            TicketRequest = new Ticket(context, "PMM", User);
            nprItem = GetNPRTicket();
            pmmWizard.StepPreviousButtonStyle.CssClass = string.Empty;
            pmmWizard.StepNextButtonStyle.CssClass = string.Empty;
            UGITModule mDetails = ObjModuleViewManager.LoadByName("NPR");
            if (mDetails.List_LifeCycles.Count > 1)
            {
                ticketStage = mDetails.List_LifeCycles.FirstOrDefault(x => x.ID == 0).Stages.FirstOrDefault(x => x.Prop_ReadyToImport == true).Name;
            }
            else if (mDetails.List_LifeCycles.Count == 1)
            {
                ticketStage = mDetails.List_LifeCycles[0].Stages.FirstOrDefault(x => x.Prop_ReadyToImport == true).Name;
            }

            if (isFromNPR)
            {
                if (pmmWizard.ActiveStepIndex == 0)
                {
                    pmmWizard.HeaderText = stepHeadings[0];
                    ActivateStep(2);
                    pmmWizard.ActiveStepIndex = 1;
                    LoadBasicDetails(nprItem);
                    pmmWizard.StepPreviousButtonStyle.CssClass = "hideblock";

                }
            }
            else
            {
                // Get filtered table (need to remove NPRs from which PMMs has already been created)
                DataTable pmmTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.NPRRequest, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                DataTable filteredTable = null;
                if (pmmTable != null && pmmTable.Columns.Contains(DatabaseObjects.Columns.TicketPMMIdLookup))
                {
                    var filteredRows = pmmTable.Select(string.Format("{1}='{3}' And ({0} is null Or {0}= 0) And ({2} is null Or {2} = 0)", DatabaseObjects.Columns.TicketPMMIdLookup, DatabaseObjects.Columns.Status, DatabaseObjects.Columns.TicketOnHold, ticketStage));
                    if (filteredRows.Length > 0)
                    {
                        filteredTable = filteredRows.CopyToDataTable();
                    }
                    else
                    {
                        filteredTable = null;
                    }
                }

                if (filteredTable != null && !filteredTable.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
                {
                    filteredTable.Columns.Add(DatabaseObjects.Columns.ModuleNameLookup, typeof(string));
                    filteredTable.Columns[DatabaseObjects.Columns.ModuleNameLookup].Expression = "";//string.Format("'{0}'", ModuleName);
                }

                //Exclude existing project from npr list
                DataTable finalFilteredTable = null;
                try
                {
                    if (filteredTable != null && filteredTable.Rows.Count > 0)
                    {
                        DataTable projectTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                        if (projectTable != null && projectTable.Rows.Count > 0)
                        {
                            DataTable dataview = projectTable.DefaultView.ToTable(true, DatabaseObjects.Columns.Id, DatabaseObjects.Columns.TicketNPRIdLookup);
                            projectTable = dataview;
                        }
                        if (projectTable == null && projectTable.Rows.Count < 0)
                        {
                            List<string> projectEixsts = (from m in projectTable.AsEnumerable()
                                                          select m.Field<string>(DatabaseObjects.Columns.TicketNPRIdLookup)).ToList();
                            finalFilteredTable = (from p in filteredTable.AsEnumerable()
                                                  where !(from o in projectEixsts select o).Contains(p.Field<string>(DatabaseObjects.Columns.TicketId))
                                                  select p).CopyToDataTable();
                        }
                        else
                        {
                            finalFilteredTable = filteredTable;
                        }
                    }
                }
                catch { }

                if (finalFilteredTable == null && filteredTable != null)
                {
                    finalFilteredTable = filteredTable.Clone();
                }
                ListPicker customListPicker = ((ListPicker)lstPicker);
                customListPicker.Module = "NPR";
                customListPicker.FilteredTable = finalFilteredTable;
                customListPicker.IsMultiSelect = true;

                if (nprItem != null)
                {
                    if (customListPicker.selectedTicketIds == null)
                    {
                        customListPicker.selectedTicketIds = new List<string>();
                    }
                    foreach (DataRow item in nprItem)
                    {
                        customListPicker.selectedTicketIds.Add(Convert.ToString(item[DatabaseObjects.Columns.TicketId]));
                    }
                }

                pmmWizard.HeaderText = stepHeadings[0];
                ActivateStep(1);
            }

            #region Checking permission for import NPR

            ///PMMCreateGroup Group and PMOGroup can only able to Import PMM and Else will not able to see the new button and remove onclick attribute of module Icon.
            bool allowImport = false;

            string varPMMCreateGroup = objConfigurationVariableHelper.GetValue(ConfigConstants.PMMCreateGroup);
            if (string.IsNullOrEmpty(varPMMCreateGroup))
                varPMMCreateGroup = objConfigurationVariableHelper.GetValue(ConfigConstants.PMOGroup);
            if (!string.IsNullOrEmpty(varPMMCreateGroup))
            {
                allowImport = UserManager.CheckUserIsInGroup(varPMMCreateGroup, User);

            }

            trError.Visible = !allowImport;
            pmmWizard.Enabled = allowImport;

            #endregion

            try
            {
                //uGovernITProjectWizard wepPartObj = (uGovernITProjectWizard)this.Parent;
                //uHelper.DisplayHelpTextLink(wepPartObj, helpTextContainer);
            }
            catch {; }
        }
        protected void PMMWizard_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {

            pmmWizard.StepPreviousButtonStyle.CssClass = string.Empty;
            pmmWizard.StepNextButtonStyle.CssClass = string.Empty;
            ddlvalue = Convert.ToInt32(ddlLifeCycleModel.Value);
            switch (pmmWizard.ActiveStepIndex)
            {
                case 0:
                    List<string> selectedIds = ((ListPicker)lstPicker).selectedTicketIds;
                    if (selectedIds == null || selectedIds.Count == 0)
                    {
                        errorMessage = "Please select a NPR ticket";
                        e.Cancel = true;
                    }
                    else
                    {
                        //nprID.Value = ((ListPicker)lstPicker).selectedTicketIds[0];
                        nprID.Value = UGITUtility.ConvertListToString(((ListPicker)lstPicker).selectedTicketIds, Constants.Separator6);
                        nprItem = GetNPRTicket();
                        LoadBasicDetails(nprItem);
                    }

                    break;
                case 1:
                    pmmSponsorHidden.Value = pmmSponsor.GetValues();
                    pmmProjectManagerHidden.Value = pmmProjectManager.GetValues();
                    pmmsponser = pmmSponsor.GetValues();
                    pmmmanager = pmmProjectManager.GetValues();
                    projectName = pmmProjectName.Text.Trim();
                    lstCheckbox.Clear();
                    foreach (RepeaterItem monitor in monitorsRepeater.Items)
                    {
                        CheckBox checkbox = (CheckBox)monitor.FindControl("checkbox");
                        //if (checkbox != null && checkbox.Checked)
                        //{
                        lstCheckbox.Add(checkbox.Checked);

                        //   atleastneMonitorSelected = true;
                        //}
                    }
                    break;
                case 2:

                    break;
                case 3:
                    bool atleastneMonitorSelected = false;
                    foreach (RepeaterItem monitor in monitorsRepeater.Items)
                    {
                        CheckBox checkbox = (CheckBox)monitor.FindControl("checkbox");
                        if (checkbox != null && checkbox.Checked)
                        {

                            atleastneMonitorSelected = true;
                        }
                    }
                    if (!atleastneMonitorSelected)
                    {
                        errorMessage = "Please select at least one monitor";
                        e.Cancel = true;
                    }
                    break;
                case 4:

                    break;
            }
        }
        public void PMMWizard_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
        {
            pmmWizard.StepPreviousButtonStyle.CssClass = string.Empty;
            //pMonitors.Visible = true;
            //if (!string.IsNullOrEmpty("" + ddlvalue))
            //{
            //    ddlLifeCycleModel.Items.FindByValue(Convert.ToString(ddlvalue)).Selected = true;
            //}
            if (isFromNPR && e.NextStepIndex == 1)
            {
                pmmWizard.StepPreviousButtonStyle.CssClass = "hideblock";
            }
            else if (isFromNPR && e.NextStepIndex == 0)
            {
                e.Cancel = true;
                pmmWizard.StepPreviousButtonStyle.CssClass = "hideblock";
            }
            else if (lstCheckbox != null && lstCheckbox.Count > 0)
            {
                int i = 0;
                foreach (RepeaterItem monitor in monitorsRepeater.Items)
                {
                    CheckBox checkbox = (CheckBox)monitor.FindControl("checkbox");
                    checkbox.Checked = lstCheckbox[i];
                    i++;
                }
            }

        }
        protected void PMMWizard_CancelButtonClick(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
        protected void PMMWizard_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            int pmmId = 0;

            DateTime baselineDate;
            //SPWeb thisWeb = SPContext.Current.Web;
            // thisWeb.AllowUnsafeUpdates = true;
            DataTable pmmTickets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");  //thisWeb.Lists[DatabaseObjects.Lists.PMMProjects];
                                                                                                                                                                      //SPListItem pmmNewTicket = pmmTickets.Items.Add();
                                                                                                                                                                      //DataRow pmmNewTicket = pmmTickets.NewRow(); 
                                                                                                                                                                      //SPListItem pmmModule = SPListHelper.LoadModuleListItem("PMM");
            DataTable dtpmmModule = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");  //uGITCache.ModuleConfigCache.LoadAllModules();
            DataRow[] pmmModuleColl = dtpmmModule.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleName, "PMM"));
            DataRow pmmModule = null;

            Ticket nprTicket = new Ticket(context, "NPR");
            Ticket pmmTicketRequest = new Ticket(context, "PMM");

            foreach (var item in nprItem)
            {
                DataRow pmmNewTicket = pmmTickets.NewRow();

                if (pmmModuleColl.Length > 0)
                {
                    pmmModule = pmmModuleColl[0];
                }


                // Created PMM ID
                if (pmmModule != null)
                {
                    if ((DateTime.Parse(pmmModule[DatabaseObjects.Columns.LastSequenceDate].ToString())).Year != DateTime.Today.Year)
                    {
                        pmmModule[DatabaseObjects.Columns.LastSequence] = 0;
                        pmmModule[DatabaseObjects.Columns.LastSequenceDate] = DateTime.Now.ToString();
                    }

                    pmmNewTicket[DatabaseObjects.Columns.TicketId] = Convert.ToString(item[DatabaseObjects.Columns.TicketId]).Replace("NPR", "PMM");
                    string[] ticketSplit = Convert.ToString(item[DatabaseObjects.Columns.TicketId]).Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                    if (ticketSplit.Length > 2)
                    {
                        pmmModule[DatabaseObjects.Columns.LastSequence] = Convert.ToInt32(ticketSplit[2]);
                    }

                    //pmmModule.Update();
                    DataTable dt = TicketDal.SaveTickettemp(pmmModule, DatabaseObjects.Tables.Modules, false);
                    //if (!string.IsNullOrEmpty(error))
                    //{
                    //    errors.Add(TicketColumnError.AddError(error));
                    //    // valid = false;
                    //}
                }

                // Copied basic info
                pmmNewTicket[DatabaseObjects.Columns.TicketNPRIdLookup] = item[DatabaseObjects.Columns.Id];
                if (pmmProjectName.Text.Trim() != string.Empty)
                {
                    pmmNewTicket[DatabaseObjects.Columns.Title] = pmmProjectName.Text;
                }
                else
                {
                    pmmNewTicket[DatabaseObjects.Columns.Title] = item[DatabaseObjects.Columns.Title];
                }

                LifeCycle lifeCycle = null;
                if (ddlLifeCycleModel.SelectedItem != null)
                {
                    if (ddlvalue == 0)
                        pmmNewTicket[DatabaseObjects.Columns.ProjectLifeCycleLookup] = DBNull.Value;
                    else
                        pmmNewTicket[DatabaseObjects.Columns.ProjectLifeCycleLookup] = ddlvalue;

                    //ddlLifeCycleModel.SelectedValue;
                    List<LifeCycle> objLifeCycle = new List<LifeCycle>();
                    LifeCycleManager lifeCycleHelper = new LifeCycleManager(context);
                    objLifeCycle = lifeCycleHelper.LoadLifeCycleByModule("PMM");
                    lifeCycle = objLifeCycle.FirstOrDefault(x => x.ID == Convert.ToInt16(ddlLifeCycleModel.SelectedItem.Value));
                    //LifeCycleHelper lifeCycleHelper = new LifeCycleHelper(SPContext.Current.Web);
                    //lifeCycle = lifeCycleHelper.LoadLifeCycleByModule(ddlLifeCycleModel.SelectedItem.Text);
                    if (lifeCycle.Stages.Count > 0)
                    {
                        pmmNewTicket[DatabaseObjects.Columns.ModuleStepLookup] = null;
                        pmmNewTicket[DatabaseObjects.Columns.TicketStatus] = lifeCycle.Stages[0].Name;
                        pmmNewTicket[DatabaseObjects.Columns.StageStep] = lifeCycle.Stages[0].StageStep;
                        pmmNewTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = string.Format("{0}{1}{2}", DatabaseObjects.Columns.TicketProjectManager, Constants.Separator, objConfigurationVariableHelper.GetValue(ConfigConstants.PMOGroup), Constants.Separator, DatabaseObjects.Columns.TicketProjectCoordinators);
                    }
                }
                else
                {
                    //SPListItemCollection stageCollection =(pmmModule.ID);
                    //if (stageCollection.Count > 0)
                    //{
                    //    pmmNewTicket[DatabaseObjects.Columns.ProjectLifeCycleLookup] = null;
                    //    pmmNewTicket[DatabaseObjects.Columns.ModuleStepLookup] = stageCollection[0][DatabaseObjects.Columns.Id];
                    //    pmmNewTicket[DatabaseObjects.Columns.ProjectPhasePctComplete] = 0;
                    //    pmmNewTicket[DatabaseObjects.Columns.TicketStatus] = Convert.ToString(stageCollection[0][DatabaseObjects.Columns.StageTitle]);
                    //    pmmNewTicket[DatabaseObjects.Columns.StageStep] = stageCollection[0][DatabaseObjects.Columns.ModuleStep];
                    //    pmmNewTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = uHelper.GetSPItemValue(stageCollection[0], DatabaseObjects.Columns.ActionUser);
                    //}
                }

                pmmNewTicket[DatabaseObjects.Columns.ScrumLifeCycle] = chkIsScrum.Checked;

                try
                {
                    if (pmmProjectManagerHidden.Value != string.Empty)
                    {
                        List<string> userCollection = new List<string>();
                        // string[] users = pmmProjectManagerHidden.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        string[] users = pmmProjectManagerHidden.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string userName in users)
                        {
                            UserProfile user = UserManager.GetUserInfoById(userName);
                            if (user != null)
                            {
                                userCollection.Add(user.Id);
                            }
                        }
                        pmmNewTicket[DatabaseObjects.Columns.TicketProjectManager] = userCollection;
                    }
                    else
                    {
                        pmmNewTicket[DatabaseObjects.Columns.TicketProjectManager] = item[DatabaseObjects.Columns.TicketProjectManager];
                    }
                }
                catch
                {
                    pmmNewTicket[DatabaseObjects.Columns.TicketProjectManager] = item[DatabaseObjects.Columns.TicketProjectManager];
                }

                try
                {
                    if (pmmSponsorHidden.Value != string.Empty)
                    {
                        List<string> userCollection = new List<string>();
                        string[] sponsors = pmmSponsorHidden.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string spns in sponsors)
                        {
                            UserProfile user = UserManager.GetUserById(spns);
                            if (user != null)
                            {
                                userCollection.Add(user.Id);
                            }
                        }
                        pmmNewTicket[DatabaseObjects.Columns.TicketSponsors] = userCollection;
                    }
                    else
                    {
                        pmmNewTicket[DatabaseObjects.Columns.TicketSponsors] = item[DatabaseObjects.Columns.TicketSponsors];
                    }
                }
                catch
                {
                    pmmNewTicket[DatabaseObjects.Columns.TicketSponsors] = item[DatabaseObjects.Columns.TicketSponsors];
                }

                pmmNewTicket[DatabaseObjects.Columns.TicketStakeHolders] = item[DatabaseObjects.Columns.TicketStakeHolders];

                pmmNewTicket[DatabaseObjects.Columns.TicketBeneficiaries] = item[DatabaseObjects.Columns.TicketBeneficiaries];
                pmmNewTicket[DatabaseObjects.Columns.CompanyMultiLookup] = item[DatabaseObjects.Columns.CompanyMultiLookup];
                pmmNewTicket[DatabaseObjects.Columns.DivisionMultiLookup] = item[DatabaseObjects.Columns.DivisionMultiLookup];
                pmmNewTicket[DatabaseObjects.Columns.TicketDescription] = item[DatabaseObjects.Columns.TicketDescription];

                //Copied Project Measures and contraints
                pmmNewTicket[DatabaseObjects.Columns.TicketTotalCost] = item[DatabaseObjects.Columns.TicketTotalCost];
                pmmNewTicket[DatabaseObjects.Columns.ProjectCost] = 0;
                pmmNewTicket[DatabaseObjects.Columns.TicketTotalStaffHeadcount] = item[DatabaseObjects.Columns.TicketTotalStaffHeadcount];
                pmmNewTicket[DatabaseObjects.Columns.TicketTotalStaffHeadcountNotes] = item[DatabaseObjects.Columns.TicketTotalStaffHeadcountNotes];
                pmmNewTicket[DatabaseObjects.Columns.TicketTotalConsultantHeadcount] = item[DatabaseObjects.Columns.TicketTotalConsultantHeadcount];
                pmmNewTicket[DatabaseObjects.Columns.TicketTotalConsultantHeadcountNotes] = item[DatabaseObjects.Columns.TicketTotalConsultantHeadcountNotes];
                pmmNewTicket[DatabaseObjects.Columns.TicketTotalConsultantHeadcount] = item[DatabaseObjects.Columns.TicketTotalConsultantHeadcount];
                pmmNewTicket[DatabaseObjects.Columns.TicketTotalConsultantHeadcountNotes] = item[DatabaseObjects.Columns.TicketTotalConsultantHeadcountNotes];
                pmmNewTicket[DatabaseObjects.Columns.TicketRiskScore] = item[DatabaseObjects.Columns.TicketRiskScore];
                pmmNewTicket[DatabaseObjects.Columns.TicketRiskScoreNotes] = item[DatabaseObjects.Columns.TicketRiskScoreNotes];
                pmmNewTicket[DatabaseObjects.Columns.TicketArchitectureScore] = item[DatabaseObjects.Columns.TicketArchitectureScore];
                pmmNewTicket[DatabaseObjects.Columns.TicketArchitectureScoreNotes] = item[DatabaseObjects.Columns.TicketArchitectureScoreNotes];
                pmmNewTicket[DatabaseObjects.Columns.TicketProjectScoreNotes] = item[DatabaseObjects.Columns.TicketProjectScoreNotes];
                pmmNewTicket[DatabaseObjects.Columns.TicketNoOfFTEs] = item[DatabaseObjects.Columns.TicketNoOfFTEs];
                pmmNewTicket[DatabaseObjects.Columns.TicketNoOfConsultants] = item[DatabaseObjects.Columns.TicketNoOfConsultants];
                pmmNewTicket[DatabaseObjects.Columns.TicketNoOfConsultantsNotes] = item[DatabaseObjects.Columns.TicketNoOfConsultantsNotes];
                pmmNewTicket[DatabaseObjects.Columns.TicketNoOfFTEsNotes] = item[DatabaseObjects.Columns.TicketNoOfFTEsNotes];
                pmmNewTicket[DatabaseObjects.Columns.TicketDesiredCompletionDate] = item[DatabaseObjects.Columns.TicketDesiredCompletionDate];

                pmmNewTicket[DatabaseObjects.Columns.UGITDaysToComplete] = pmmNewTicket[DatabaseObjects.Columns.TicketDuration];
                pmmNewTicket[DatabaseObjects.Columns.TicketPctComplete] = 0;
                pmmNewTicket[DatabaseObjects.Columns.IsPrivate] = item[DatabaseObjects.Columns.IsPrivate];
                pmmNewTicket[DatabaseObjects.Columns.ProjectRank] = item[DatabaseObjects.Columns.ProjectRank];
                pmmNewTicket[DatabaseObjects.Columns.ProjectRank2] = item[DatabaseObjects.Columns.ProjectRank2];
                pmmNewTicket[DatabaseObjects.Columns.ProjectRank3] = item[DatabaseObjects.Columns.ProjectRank3];
                pmmNewTicket[DatabaseObjects.Columns.TicketCreationDate] = DateTime.Now;

                //Get priority in NPR and set in pmm project
                string nprPriority = UGITUtility.SplitString(item[DatabaseObjects.Columns.TicketPriorityLookup], Constants.Separator, 1);
                if (!string.IsNullOrEmpty(nprPriority))
                {
                    //SPQuery pmmPrioriyQuery = new SPQuery();
                    //pmmPrioriyQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ModuleNameLookup, "PMM");
                    DataTable pmmPriorityTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketPriority, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                    if (pmmPriorityTable != null && pmmPriorityTable.Rows.Count > 0)
                    {
                        DataRow[] pmmPriorityTableColl = pmmPriorityTable.Select(string.Format("{0}={1}", DatabaseObjects.Columns.ModuleNameLookup, "PMM"));
                        // DataRow[] pmmPriorityTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketPriority).Select(string.Format("{0}={1}", DatabaseObjects.Columns.ModuleNameLookup, "PMM"));  //SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.TicketPriority, pmmPrioriyQuery).GetDataTable();
                        if (pmmPriorityTableColl.Length > 0)
                        {
                            pmmPriorityTable = pmmPriorityTableColl.CopyToDataTable();
                            DataRow pmmPriorityRow = pmmPriorityTable.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.UPriority) == nprPriority);
                            if (pmmPriorityRow != null)
                            {
                                pmmNewTicket[DatabaseObjects.Columns.TicketPriorityLookup] = pmmPriorityRow[DatabaseObjects.Columns.Id];
                            }
                        }
                    }
                }

                pmmNewTicket[DatabaseObjects.Columns.FunctionalAreaLookup] = item[DatabaseObjects.Columns.FunctionalAreaLookup];
                pmmNewTicket[DatabaseObjects.Columns.ProjectInitiativeLookup] = item[DatabaseObjects.Columns.ProjectInitiativeLookup];

                //Import NPR Request Type equivalent request type from PMM
                // SPFieldLookupValue nprRequestTypeLookup = new SPFieldLookupValue(Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.TicketRequestTypeLookup)));
                int nprRequestTypeLookup = Convert.ToInt32(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.TicketRequestTypeLookup));
                if (nprRequestTypeLookup > 0)
                {
                    ModuleRequestType nprRequestType = nprTicket.Module.List_RequestTypes.FirstOrDefault(x => x.ID == nprRequestTypeLookup);
                    if (nprRequestType != null)
                    {
                        ModuleRequestType pmmRequestType = pmmTicketRequest.Module.List_RequestTypes.FirstOrDefault(x => x.Category == nprRequestType.Category); //&& x.ID == nprRequestTypeLookup need discusion for check
                        if (pmmRequestType != null)
                        {
                            pmmNewTicket[DatabaseObjects.Columns.TicketRequestTypeLookup] = pmmRequestType.ID;
                        }
                    }
                }
                pmmNewTicket[DatabaseObjects.Columns.TicketOwner] = item[DatabaseObjects.Columns.TicketOwner];

                pmmNewTicket[DatabaseObjects.Columns.TicketApprovedRFE] = item[DatabaseObjects.Columns.TicketApprovedRFE];
                pmmNewTicket[DatabaseObjects.Columns.TicketApprovedRFEAmount] = item[DatabaseObjects.Columns.TicketApprovedRFEAmount];
                pmmNewTicket[DatabaseObjects.Columns.TicketApprovedRFEType] = item[DatabaseObjects.Columns.TicketApprovedRFEType];

                pmmNewTicket[DatabaseObjects.Columns.TicketRequestor] = item[DatabaseObjects.Columns.TicketRequestor];
                pmmNewTicket[DatabaseObjects.Columns.PRPGroup] = item[DatabaseObjects.Columns.PRPGroup];
                pmmNewTicket[DatabaseObjects.Columns.LocationMultLookup] = item[DatabaseObjects.Columns.LocationMultLookup];
                pmmNewTicket[DatabaseObjects.Columns.APPTitleLookup] = item[DatabaseObjects.Columns.APPTitleLookup];
                pmmNewTicket[DatabaseObjects.Columns.ModuleNameLookup] = item[DatabaseObjects.Columns.ModuleNameLookup];
                pmmNewTicket[DatabaseObjects.Columns.TicketProjectScope] = item[DatabaseObjects.Columns.TicketProjectScope];
                pmmNewTicket[DatabaseObjects.Columns.TicketProjectAssumptions] = item[DatabaseObjects.Columns.TicketProjectAssumptions];
                pmmNewTicket[DatabaseObjects.Columns.TicketProjectBenefits] = item[DatabaseObjects.Columns.TicketProjectBenefits];
                pmmNewTicket[DatabaseObjects.Columns.ProblemBeingSolved] = item[DatabaseObjects.Columns.ProblemBeingSolved];
                pmmNewTicket[DatabaseObjects.Columns.ProjectRiskNotes] = item[DatabaseObjects.Columns.ProjectRiskNotes];
                // pmmNewTicket[DatabaseObjects.Columns.ProjectClassLookup] = item[DatabaseObjects.Columns.ProjectClassLookup];
                pmmNewTicket[DatabaseObjects.Columns.TicketBusinessManager] = item[DatabaseObjects.Columns.TicketBusinessManager];

                pmmNewTicket[DatabaseObjects.Columns.TicketClassification] = item[DatabaseObjects.Columns.TicketClassification];
                pmmNewTicket[DatabaseObjects.Columns.TicketClassificationType] = item[DatabaseObjects.Columns.TicketClassificationType];
                pmmNewTicket[DatabaseObjects.Columns.TicketClassificationImpact] = item[DatabaseObjects.Columns.TicketClassificationImpact];

                // Copy Custom fields
                int numFields = 10;
                for (int i = 1; i <= numFields; i++)
                {
                    string textFieldName = string.Format("CustomUGText{0}", i.ToString("D2"));
                    string dateFieldName = string.Format("CustomUGDate{0}", i.ToString("D2"));
                    string userFieldName = string.Format("CustomUGUser{0}", i.ToString("D2"));
                    string multiuserFieldName = string.Format("CustomUGUserMulti{0}", i.ToString("D2"));

                    if (UGITUtility.IfColumnExists(textFieldName, pmmTickets) && UGITUtility.IsSPItemExist(item, textFieldName))
                        pmmNewTicket[textFieldName] = item[textFieldName];
                    if (UGITUtility.IfColumnExists(dateFieldName, pmmTickets) && UGITUtility.IsSPItemExist(item, dateFieldName))
                        pmmNewTicket[dateFieldName] = item[dateFieldName];
                    if (UGITUtility.IfColumnExists(userFieldName, pmmTickets) && UGITUtility.IsSPItemExist(item, userFieldName))
                        pmmNewTicket[userFieldName] = item[userFieldName];
                    if (UGITUtility.IfColumnExists(multiuserFieldName, pmmTickets) && UGITUtility.IsSPItemExist(item, multiuserFieldName))
                        pmmNewTicket[multiuserFieldName] = item[multiuserFieldName];
                }
                pmmNewTicket[DatabaseObjects.Columns.TicketDescription] = item[DatabaseObjects.Columns.TicketDescription];
                error = TicketRequest.CommitChanges(pmmNewTicket, "pmmWizard", Request.Url);
                if (!string.IsNullOrEmpty(error))
                {
                    errors.Add(TicketColumnError.AddError(error));
                    // valid = false;
                }
                //  pmmNewTicket[DatabaseObjects.Columns.TicketDescription] = item[DatabaseObjects.Columns.TicketDescription];

                //pmmNewTicket.UpdateOverwriteVersion();

                //Store PMM ref in NPR so that NPR knowd that PMM is created.
                item[DatabaseObjects.Columns.TicketPMMIdLookup] = pmmNewTicket[DatabaseObjects.Columns.ID];

                //Close NPR ticket if checkbox is checked
                if (cbCloseNPR.Checked)
                {
                    item.AcceptChanges();
                    item.SetModified();
                    nprTicket.Approve(User, item, true);
                    // Send Email notification - NOT NEEDED
                    // nprTicket.SendEmailToActionUsers(Convert.ToString(item[DatabaseObjects.Columns.ModuleStepLookup]), item, nprTicket.Module.ID);
                    // Update Cache
                    //  uGITCache.ModuleDataCache.UpdateOpenTicketsCache(nprTicket.Module.ID, item);
                }
                error = nprTicket.CommitChanges(item, "pmmWizard", Request.Url);
                if (!string.IsNullOrEmpty(error))
                {
                    errors.Add(TicketColumnError.AddError(error));
                    // valid = false;
                }

                //Added to update PMMIdLookup into NPR table
                GetTableDataManager.UpdateItem<int>(DatabaseObjects.Tables.NPRRequest, Convert.ToInt64(item[DatabaseObjects.Columns.ID]), new Dictionary<string, object>() { { DatabaseObjects.Columns.TicketPMMIdLookup, item[DatabaseObjects.Columns.TicketPMMIdLookup] } });

                pmmId = Convert.ToInt32(pmmNewTicket[DatabaseObjects.Columns.ID]);
                baselineDate = DateTime.Now;
                // Created selected monitors on project
                // SPList pmmMonitorOptions = thisWeb.Lists[DatabaseObjects.Lists.ModuleMonitorOptions];
                // SPList projectMonitors = thisWeb.Lists[DatabaseObjects.Lists.ProjectMonitorState];
                //DataTable pmmMonitorOptions = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonitorOptions, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                List<ModuleMonitorOption> pmmMonitorOptions = MonitorOptionManagerObj.Load();
                //DataTable projectMonitors = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectMonitorState, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                List<ProjectMonitorState> projectMonitors = MonitorStateManagerObj.Load();

                int totalMonitorsSelected = 0;
                foreach (RepeaterItem monitor in monitorsRepeater.Items)
                {
                    CheckBox checkbox = (CheckBox)monitor.FindControl("checkbox");
                    if (checkbox != null && checkbox.Checked)
                    {
                        totalMonitorsSelected++;
                    }
                }

                double projectScore = 0;
                ProjectMonitorStateManager monitorStateManager = null;
                ProjectMonitorState newPMMMonitor = null;
                foreach (RepeaterItem monitor in monitorsRepeater.Items)
                {
                    CheckBox checkbox = (CheckBox)monitor.FindControl("checkbox");
                    HiddenField label = (HiddenField)monitor.FindControl("monitorLabel");
                    //HiddenField monitorDefault = (HiddenField)monitor.FindControl("monitorDefaultLabel");
                    CheckBox autocalc = (CheckBox)monitor.FindControl("chkautocalc");

                    if (pmmMonitorOptions != null && pmmMonitorOptions.Count > 0)
                    {
                        //DataRow[] coll = pmmMonitorOptions.Select(string.Format("{0}={1}", DatabaseObjects.Columns.ModuleMonitorNameLookup, label.Value)); 
                        //ModuleMonitorOption row = pmmMonitorOptions.FirstOrDefault(x => x.IsDefault == false);
                        List<ModuleMonitorOption> lstOfModuleMonitor = pmmMonitorOptions.Where(x => x.ModuleMonitorNameLookup == UGITUtility.StringToLong(label.Value)).ToList();
                        //DataRow row = pmmMonitorOptions.AsEnumerable().FirstOrDefault(x => x.Field<Boolean>(DatabaseObjects.Columns.IsDefault) == false);
                        if (lstOfModuleMonitor != null && lstOfModuleMonitor.Count > 0)
                        {
                            ModuleMonitorOption moduleMonitorOption = lstOfModuleMonitor.FirstOrDefault(x => x.IsDefault);
                            if (checkbox.Checked && moduleMonitorOption != null)
                            {
                                monitorStateManager = new ProjectMonitorStateManager(context);
                                newPMMMonitor = new ProjectMonitorState();
                                newPMMMonitor.Title = string.Format("{0} - {1}", Convert.ToString(pmmNewTicket[DatabaseObjects.Columns.TicketId]), checkbox.Text);
                                newPMMMonitor.PMMIdLookup = Convert.ToInt64(pmmNewTicket[DatabaseObjects.Columns.ID]);
                                newPMMMonitor.TicketId = Convert.ToString(pmmNewTicket[DatabaseObjects.Columns.TicketId]);
                                newPMMMonitor.ModuleMonitorNameLookup = Convert.ToInt64(label.Value);
                                newPMMMonitor.ModuleNameLookup = uHelper.getModuleNameByTicketId(newPMMMonitor.TicketId);
                                newPMMMonitor.ModuleMonitorOptionName = moduleMonitorOption.ModuleMonitorOptionName;
                                newPMMMonitor.ProjectMonitorWeight = (100 / totalMonitorsSelected);
                                newPMMMonitor.ModuleMonitorOptionLEDClass = moduleMonitorOption.ID;
                                newPMMMonitor.ModuleMonitorOptionLEDName = moduleMonitorOption.ModuleMonitorOptionLEDClass;
                                newPMMMonitor.ModuleMonitorOptionIdLookup = Convert.ToInt64(moduleMonitorOption.ID);
                                projectScore += (100 / totalMonitorsSelected) * (float.Parse(moduleMonitorOption.ModuleMonitorMultiplier.ToString()) / 100);
                                if ((checkbox.Text == "On Budget" || checkbox.Text == "On Time") && autocalc != null && autocalc.Checked)
                                {
                                    newPMMMonitor.AutoCalculate = true;
                                }
                                string monitorname = Convert.ToString(moduleMonitorOption.ModuleMonitorNameLookup);
                                if (!string.IsNullOrEmpty(monitorname) && monitorname == "Risk Level")
                                {
                                    float weightage = 0;
                                    float.TryParse(Convert.ToString(newPMMMonitor.ProjectMonitorWeight), out weightage);
                                    float multiplier = 0;
                                    float.TryParse(Convert.ToString(moduleMonitorOption.ModuleMonitorMultiplier), out multiplier);
                                    float score = weightage * (multiplier / 100);
                                    pmmNewTicket[DatabaseObjects.Columns.TicketRiskScore] = Math.Round(100 - score);
                                }

                                monitorStateManager.Insert(newPMMMonitor);
                                error = TicketRequest.CommitChanges(pmmNewTicket, "pmmWizard", Request.Url);
                                if (!string.IsNullOrEmpty(error))
                                {
                                    errors.Add(TicketColumnError.AddError(error));
                                    // valid = false;
                                }
                            }

                        }
                    }
                }

                // update prject socre in PMM list

                pmmNewTicket[DatabaseObjects.Columns.TicketProjectScore] = projectScore;
                List<UGITTask> importTasks = new List<UGITTask>();
                List<UGITTask> preTasks = new List<UGITTask>();

                if (btImportTempleSchedule.Checked && lifeCycle != null)
                {
                    //TaskTemplateHelper taskTemplateHelper = new TaskTemplateHelper(SPContext.Current.Web);
                    //if (ddlTaskTemplates.SelectedValue == string.Empty)
                    //{
                    //    importTasks = taskTemplateHelper.GenerateDefaultTasks(lifeCycle, dtcStartDate.SelectedDate);
                    //}
                    //else
                    //{
                    //    TaskTemplate taskTemplate = taskTemplateHelper.LoadTemplateByID(Convert.ToInt16(ddlTaskTemplates.SelectedValue));
                    //    importTasks = taskTemplateHelper.GenerateTasksFromTaskTemplate(lifeCycle, dtcStartDate.SelectedDate, taskTemplate.Tasks);
                    //}

                    //foreach (UGITTask tk in importTasks)
                    //{
                    //    tk.ModuleName = "PMM";
                    //    tk.ProjectLookup = new SPFieldLookupValue(pmmNewTicket.ID, Convert.ToString(pmmNewTicket[DatabaseObjects.Columns.TicketId]));
                    //    tk.Predecessors = null;
                    //    tk.ParentTaskID = 0;
                    //}
                }
                else
                {
                    //importTasks = UGITTaskHelper.LoadByProjectID(SPContext.Current.Web, "NPR", item.ID);
                    //UGITTask.ReManageTasks(ref importTasks, true);

                    //foreach (UGITTask task in importTasks)
                    //{
                    //    task.ModuleName = "PMM";
                    //    task.ProjectLookup = new SPFieldLookupValue(pmmNewTicket.ID, Convert.ToString(pmmNewTicket[DatabaseObjects.Columns.TicketId]));
                    //}
                }

                if (importTasks != null && importTasks.Count > 0)
                {
                    ////Imports tasks against project
                    //importTasks = UGITTaskHelper.ImportTasks(SPContext.Current.Web, "PMM", importTasks, false);

                    ////set fields like start and enddate and other schedule related field in project
                    //UGITTaskHelper.CalculateProjectStartEndDate("PMM", importTasks, pmmNewTicket);
                    //pmmNewTicket.UpdateOverwriteVersion();

                    ////Update task cache for imported project.
                    //TaskCache.ReloadProjectTasks("PMM", Convert.ToString(uHelper.GetSPItemValue(pmmNewTicket, DatabaseObjects.Columns.TicketId)));

                    ////User Allocation in RMM in TaskImport...
                    //ResourceAllocation.UpdateProjectPlannedAllocationByUser(SPContext.Current.Web, importTasks, "PMM", Convert.ToString(uHelper.GetSPItemValue(pmmNewTicket, DatabaseObjects.Columns.TicketId)), true);

                }


                //Copied NPR budget into pmm budget
                //double budgetedCost = 0;
                // SPList pmmBudgets = thisWeb.Lists[DatabaseObjects.Lists.PMMBudget];
                DataTable nprBudgets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.NPRBudget, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");

                DataRow[] nprBudgetscoll = null;
                if (nprBudgets != null && nprBudgets.Rows.Count > 0)
                {
                    nprBudgetscoll = nprBudgets.Select(string.Format("{0}={1}", DatabaseObjects.Columns.TicketNPRIdLookup, item[DatabaseObjects.Columns.ID]));
                    if (nprBudgetscoll.Length > 0)
                    {
                        DataTable pmmBudgets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMBudget, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                        //pmmBudgets = nprBudgetscoll.CopyToDataTable();
                        //if (pmmBudgets != null && pmmBudgets.Rows.Count > 0)
                        //{
                        foreach (DataRow nprBudget in nprBudgetscoll)
                        {
                            DataRow pmmBudget = pmmBudgets.NewRow();
                            pmmBudget[DatabaseObjects.Columns.TicketPMMIdLookup] = pmmNewTicket[DatabaseObjects.Columns.ID];
                            pmmBudget[DatabaseObjects.Columns.BudgetItem] = nprBudget[DatabaseObjects.Columns.BudgetItem];
                            pmmBudget[DatabaseObjects.Columns.AllocationStartDate] = nprBudget[DatabaseObjects.Columns.AllocationStartDate];
                            pmmBudget[DatabaseObjects.Columns.AllocationEndDate] = nprBudget[DatabaseObjects.Columns.AllocationEndDate];
                            pmmBudget[DatabaseObjects.Columns.BudgetDescription] = nprBudget[DatabaseObjects.Columns.BudgetDescription];
                            pmmBudget[DatabaseObjects.Columns.BudgetCategoryLookup] = nprBudget[DatabaseObjects.Columns.BudgetCategoryLookup];
                            pmmBudget[DatabaseObjects.Columns.BudgetAmount] = nprBudget[DatabaseObjects.Columns.BudgetAmount];
                            pmmBudget[DatabaseObjects.Columns.IsAutoCalculated] = false;
                            pmmBudget[DatabaseObjects.Columns.BudgetStatus] = (int)Enums.BudgetStatus.Approve;

                            //  budgetedCost += Convert.ToDouble(nprBudget[DatabaseObjects.Columns.BudgetAmount]);
                            // pmmBudget.Update();
                            // error = TicketRequest.CommitChanges(pmmBudget, "pmmWizard", Request.Url);
                            //if (!string.IsNullOrEmpty(error))
                            //{
                            //    errors.Add(TicketColumnError.AddError(error));
                            //    // valid = false;
                            //}

                        }
                        //pmmNewTicket[DatabaseObjects.Columns.TicketTotalCost] = budgetedCost;
                        // pmmNewTicket.UpdateOverwriteVersion();

                        // }
                    }
                    //SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.NPRBudget, nprBudgetQuery);


                }
                // SPQuery nprBudgetQuery = new SPQuery();
                // nprBudgetQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.TicketNPRIdLookup, item[DatabaseObjects.Columns.Id]);
                // SPListItemCollection nprBudgets = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.NPRBudget, nprBudgetQuery);
                //foreach (DataRow nprBudget in nprBudgets)
                //{
                //    DataRow pmmBudget = pmmBudgets.NewRow();
                //    pmmBudget[DatabaseObjects.Columns.TicketPMMIdLookup] = pmmNewTicket.ID;
                //    pmmBudget[DatabaseObjects.Columns.BudgetItem] = nprBudget[DatabaseObjects.Columns.BudgetItem];
                //    pmmBudget[DatabaseObjects.Columns.AllocationStartDate] = nprBudget[DatabaseObjects.Columns.AllocationStartDate];
                //    pmmBudget[DatabaseObjects.Columns.AllocationEndDate] = nprBudget[DatabaseObjects.Columns.AllocationEndDate];
                //    pmmBudget[DatabaseObjects.Columns.BudgetDescription] = nprBudget[DatabaseObjects.Columns.BudgetDescription];
                //    pmmBudget[DatabaseObjects.Columns.BudgetLookup] = nprBudget[DatabaseObjects.Columns.BudgetLookup];
                //    pmmBudget[DatabaseObjects.Columns.BudgetAmount] = nprBudget[DatabaseObjects.Columns.BudgetAmount];
                //    pmmBudget[DatabaseObjects.Columns.IsAutoCalculated] = "0";
                //    pmmBudget[DatabaseObjects.Columns.BudgetStatus] = (int)Helpers.Enums.BudgetStatus.Approve;

                //    budgetedCost += Convert.ToDouble(nprBudget[DatabaseObjects.Columns.BudgetAmount]);
                //    pmmBudget.Update();

                //}
                //Update TicketTotalCost of pmm in PMM list
                //pmmNewTicket[DatabaseObjects.Columns.TicketTotalCost] = budgetedCost;
                //pmmNewTicket.UpdateOverwriteVersion();

                //Copied Monthly Budgets
                //List<PMMBudget> budgets = PMMBudget.LoadListByID(pmmNewTicket.ID);
                //foreach (PMMBudget bg in budgets)
                //{
                //    // Update Monthly distribution in Itg Monthly budget list.
                //    PMMBudget.UpdateNonProjectMonthlyDistributionBudget(bg.budgetCategory.Id);

                //    // Revoke budget distribution of budget item and update new data
                //    PMMBudget.UpdateProjectMonthlyDistributionBudget(null, bg);
                //}

                //List<UGITTask> tasks = UGITTaskHelper.LoadByProjectID(SPContext.Current.Web, "PMM", pmmNewTicket.ID);
                //if (tasks != null && tasks.Count > 0)
                //{
                //    tasks = UGITTask.MapRelationalObjects(tasks);
                //    UGITTask.CalculateDuration(ref tasks);
                //    UGITTask.ReOrderTasks(ref tasks);
                //    tasks = tasks.OrderBy(x => x.ItemOrder).ToList();
                //    UGITTaskHelper.SaveTasks(SPContext.Current.Web, ref tasks, "PMM");
                //    TaskCache.ReloadProjectTasks("PMM", Convert.ToString(pmmNewTicket[DatabaseObjects.Columns.TicketId]));
                //}

                //Refresh Cache
                //uGITCache.ModuleDataCache.UpdateOpenTicketsCache(pmmTicketRequest.Module.ID, pmmNewTicket);

                //  thisWeb.AllowUnsafeUpdates = false;

                // Notify project manager if configured
                if (objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.NotifyPMMManagerOnStart))
                {
                    string emailTo = string.Empty;
                    string[] users = pmmProjectManagerHidden.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string userName in users)
                    {
                        UserProfile user = UserManager.GetUserInfoById(userName);
                        if (user != null && !string.IsNullOrEmpty(user.Email))
                        {
                            if (emailTo != string.Empty)
                                emailTo += ";";
                            emailTo += user.Email;
                        }
                    }

                    //StringBuilder emailBodyTemp = new StringBuilder();
                    //string signature = ConfigurationVariableHelper.GetConfigVariableValue("Signature");
                    //string emailTitle = string.Format("New Project Started - {0}: {1}", pmmNewTicket[DatabaseObjects.Columns.TicketId], pmmNewTicket[DatabaseObjects.Columns.Title]);

                    //string url = string.Format("{0}?TicketId={1}&ModuleName=PMM",  UGITUtility.GetAbsoluteURL(Constants.HomePage), Convert.ToString(pmmNewTicket[DatabaseObjects.Columns.TicketId]));
                    //emailBodyTemp.AppendFormat("Project <a href='{0}'>{1}: {2} </a> has been started and you have been assigned as the project manager.", url, pmmNewTicket[DatabaseObjects.Columns.TicketId], pmmNewTicket[DatabaseObjects.Columns.Title]);
                    //emailBodyTemp.AppendFormat("<br /><br />" + signature + "<br />");
                    //emailBodyTemp.AppendFormat("{0}", HttpUtility.HtmlDecode(uHelper.GetTicketDetailsForEmailFooter(pmmNewTicket, "PMM", true)));

                    //MailMessenger mail = new MailMessenger();
                    //mail.SendMail(emailTo, emailTitle, string.Empty, Convert.ToString(emailBodyTemp), true, SPContext.Current.Web, new string[] { }, true);
                }

                refreshPage = false;

                if (pmmId > 0)
                {
                   // PMMBaseline baseline = new PMMBaseline(Convert.ToInt64(pmmId), baselineDate,context);//check impact@Chetan
                    Baseline baseline = new Baseline("", baselineDate,context);
                    baseline.BaselineComment = "Initial Baseline";
                    // Create baseline: Passing true in 2nd parm prevents generating new version
                    //baseline.CreateBaseline(pmmNewTicket, true);
                    TicketId = Convert.ToString(pmmNewTicket[DatabaseObjects.Columns.TicketId]);
                    if (chkPortal.Checked)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CreatePortal", "<script>CreatePortal();</script>");
                    }
                    else
                    {
                        uHelper.ClosePopUpAndEndResponse(Context, true);
                    }
                }
                else
                {
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
            }
        }
        protected void ActivateStep(int stepNumber)
        {
            Label stepheading = (Label)pmmWizard.FindControl("HeaderContainer").FindControl("stepHeading");
            for (int i = 1; i <= 4; i++)
            {
                HtmlTableCell step1div = (HtmlTableCell)pmmWizard.FindControl("HeaderContainer").FindControl("step" + i.ToString() + "Div");

                step1div.Attributes.Clear();
                if (i == stepNumber)
                {
                    step1div.Attributes.Add("class", "arrow_active");
                    stepheading.Text = stepHeadings[i - 1];
                }
                else
                {
                    step1div.Attributes.Add("class", "arrow");
                }
            }

            // pmmWizard.ActiveStep = pmmWizard.WizardSteps[stepNumber];
        }
        protected void Step2_Load(object sender, EventArgs e)
        {
            ActivateStep(2);
            pmmWizard.HeaderText = stepHeadings[1];
            pmmProjectName.Text = projectName;
            if (pmmmanager != string.Empty)
            {
                pmmProjectManager.SetValues(pmmmanager);
            }


            if (pmmsponser != string.Empty)
            {
                pmmSponsor.SetValues(pmmsponser);
            }

            if (monitorsRepeater.Items.Count > 0)
            {
                // pMonitors.Visible = true;
            }
        }
        protected void Step3_Load(object sender, EventArgs e)
        {
            ActivateStep(3);
            HtmlTableCell step1div = (HtmlTableCell)pmmWizard.FindControl("HeaderContainer").FindControl("step2Div");
            step1div.Attributes.Clear();
            step1div.Attributes.Add("class", "arrow");
            HtmlTableCell step2div = (HtmlTableCell)pmmWizard.FindControl("HeaderContainer").FindControl("step3Div");
            step2div.Attributes.Clear();
            step2div.Attributes.Add("class", "arrow_active");
            pmmWizard.HeaderText = stepHeadings[2];
            setStage3Data();

        }
        protected void Step4_Load(object sender, EventArgs e)
        {
            ActivateStep(4);

            HtmlTableCell step1div = (HtmlTableCell)pmmWizard.FindControl("HeaderContainer").FindControl("step3Div");
            step1div.Attributes.Clear();
            step1div.Attributes.Add("class", "arrow");
            HtmlTableCell step2div = (HtmlTableCell)pmmWizard.FindControl("HeaderContainer").FindControl("step4Div");
            step2div.Attributes.Clear();
            step2div.Attributes.Add("class", "arrow_active");
            pmmWizard.HeaderText = stepHeadings[3];


            Table details = new Table();
            details.CssClass = "tableProject";
            //project name
            if (pmmProjectName.Text.Trim() != null)
            {
                pmmProjectName.Text = projectName;
                TableRow row = new TableRow();
                TableCell c1 = new TableCell();
                c1.Text = string.Format("<b style='float:right; padding-right:2px;'>{0}:</b>", "Project Name");
                TableCell c2 = new TableCell();
                c2.Text = pmmProjectName.Text;
                row.Cells.Add(c1);
                row.Cells.Add(c2);
                details.Rows.Add(row);
            }

            // sponsors 
            string sponsors = string.Empty;
            List<string> sponsorsVals = new List<string>();
            if (pmmsponser != string.Empty)
            {
                sponsorsVals = UGITUtility.ConvertStringToList(pmmsponser, new string[] { "," });
            }
            else
            {
                if (nprItem[0][DatabaseObjects.Columns.TicketSponsors] != null)
                {
                    //SPFieldUserValueCollection tsponsors = (SPFieldUserValueCollection)nprItem[DatabaseObjects.Columns.TicketSponsors];
                    //List<string> tsponsors = new List<string>();
                    //tsponsors=  //nprItem[DatabaseObjects.Columns.TicketSponsors];
                    //if (sponsors != null)
                    //{
                    //    foreach (string usrVal in tsponsors)
                    //    {
                    //        sponsorsVals.Add(usrVal);
                    //    }
                    //}
                }
            }

            try
            {
                List<UserProfile> users = UserManager.GetUserInfosById(sponsorsVals.ToArray());
                foreach (UserProfile usr in users)
                {
                    if (sponsors != string.Empty)
                        sponsors += ",";
                    sponsors += usr.Name;
                }
            }
            catch { }

            {
                TableRow row = new TableRow();
                TableCell c1 = new TableCell();
                c1.Text = string.Format("<b style='float:right; padding-right:2px;'>{0}:</b>", "Sponsors");
                TableCell c2 = new TableCell();
                c2.Text = sponsors;
                row.Cells.Add(c1);
                row.Cells.Add(c2);
                details.Rows.Add(row);
            }


            // Project Manager(s)
            string projectManagers = string.Empty;
            List<string> projectManagerVals = new List<string>();
            if (pmmmanager != string.Empty)
            {
                //projectManagerVals = uHelper.ConvertStringToList(pmmProjectManagerHidden.Value, new string[] { "," });
                projectManagerVals = UGITUtility.ConvertStringToList(pmmmanager, new string[] { ";" });
            }
            else
            {
                if (nprItem[0][DatabaseObjects.Columns.TicketProjectManager] != null)
                {
                    // SPFieldUserValueCollection tprojManagers = (SPFieldUserValueCollection)nprItem[DatabaseObjects.Columns.TicketProjectManager];
                    List<string> tprojManagers = new List<string>();
                    tprojManagers = UGITUtility.ConvertStringToList(Convert.ToString(nprItem[0][DatabaseObjects.Columns.TicketSponsors]), Constants.Separator);
                    if (projectManagerVals != null)
                    {
                        foreach (string usrVal in tprojManagers)
                        {
                            projectManagerVals.Add(usrVal);
                        }
                    }
                }
            }

            try
            {
                List<UserProfile> users = UserManager.GetUserInfosById(projectManagerVals.ToArray());
                foreach (UserProfile usr in users)
                {
                    if (projectManagers != string.Empty)
                        projectManagers += ",";
                    projectManagers += usr.Name;
                }
            }
            catch { }

            {
                TableRow row = new TableRow();
                TableCell c1 = new TableCell();
                c1.Text = string.Format("<b style='float:right; padding-right:2px;'>{0}:</b>", "Project Manager(s)");
                TableCell c2 = new TableCell();
                c2.Text = projectManagers;
                row.Cells.Add(c1);
                row.Cells.Add(c2);
                details.Rows.Add(row);
            }

            //LifeCycle
            {
                TableRow row = new TableRow();
                TableCell c1 = new TableCell();
                c1.Text = string.Format("<b style='float:right; padding-right:2px;'>{0}:</b>", "Project Lifecycle");
                TableCell c2 = new TableCell();
                c2.Text = ddlLifeCycleModel.Text;
                row.Cells.Add(c1);
                row.Cells.Add(c2);
                details.Rows.Add(row);
            }

            //Is Scrum
            {
                TableRow row = new TableRow();
                TableCell c1 = new TableCell();
                c1.Text = string.Format("<b style='float:right; padding-right:2px;'>{0}:</b>", "Enable Scrum");
                TableCell c2 = new TableCell();
                c2.Text = Convert.ToString(chkIsScrum.Checked);
                row.Cells.Add(c1);
                row.Cells.Add(c2);
                details.Rows.Add(row);
            }

            //Task Template
            {
                TableRow row = new TableRow();
                TableCell c1 = new TableCell();
                c1.Text = string.Format("<b style='float:right; padding-right:2px;'>{0}:</b>", "Task Template");
                TableCell c2 = new TableCell();
                // c2.Text = ddlTaskTemplates.SelectedItem.Text;
                row.Cells.Add(c1);
                row.Cells.Add(c2);
                details.Rows.Add(row);
            }

            //Monitors
            Table detailsMonitor = new Table();
            detailsMonitor.CssClass = "tableProject";
            bool includeAllProjectMonitors = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.IncludeAllProjectMonitors);
            if (!includeAllProjectMonitors)
            {
                {
                    TableRow row = new TableRow();
                    TableCell c1 = new TableCell();
                    c1.Text = "<b>Selected Monitors:</b>";
                    c1.Style.Add(HtmlTextWriterStyle.PaddingTop, "5px");
                    c1.Style.Add(HtmlTextWriterStyle.PaddingBottom, "5px");
                    c1.ColumnSpan = 2;
                    row.Cells.Add(c1);
                    detailsMonitor.Rows.Add(row);
                }
                int i = 0;
                foreach (RepeaterItem monitor in monitorsRepeater.Items)
                {
                    if (lstCheckbox != null)
                    {
                        CheckBox checkbox = (CheckBox)monitor.FindControl("checkbox");
                        checkbox.Checked = lstCheckbox[i];
                        HiddenField label = (HiddenField)monitor.FindControl("monitorLabel");

                        TableRow row = new TableRow();
                        TableCell c1 = new TableCell();
                        c1.Text = string.Format("<span style='float:right; padding-right:2px;'>{0}:</span>", checkbox.Text);
                        TableCell c2 = new TableCell();
                        c2.Text = "<b style='color:red;font-weight:normal;'>Not Required</b>";
                        row.Cells.Add(c1);
                        row.Cells.Add(c2);
                        detailsMonitor.Rows.Add(row);

                        if (checkbox != null && checkbox.Checked)
                        {
                            c2.Text = "<b style='color:green;font-weight:normal;'>Required</b>";
                        }
                    }
                    i++;
                }
            }

            wizardStep4Panel.Controls.Add(details);
            wizardStep4Panel.Controls.Add(detailsMonitor);
        }
        public bool GetChecked(string checkedBit)
        {
            if (checkedBit == "True")
            {
                return true;
            }
            else return false;

        }
        private DataRow[] GetNPRTicket()
        {
            string nprRequest = string.Empty;
            if (!string.IsNullOrEmpty(Request["NPRTicketId"]) && Request["NPRTicketId"].Trim() != string.Empty && Request["NPRTicketId"].Trim() != "0")
            {
                nprRequest = Request["NPRTicketId"].Trim();
                isFromNPR = true;
            }
            else
            {
                nprRequest = nprID.Value.Trim();
            }

            DataTable lists = GetTableDataManager.GetTableData("NPR", $"{DatabaseObjects.Columns.ID} = '0'");
            DataRow listRow;
            if (!string.IsNullOrEmpty(nprRequest))
            {
                string[] nprIds = UGITUtility.SplitString(nprRequest, Constants.Separator6);
                foreach (string nprIdvalue in nprIds)
                {
                    listRow = lists.NewRow();
                    DataRow row = Ticket.GetCurrentTicket(context, "NPR", nprIdvalue);
                    for (int i = 0; i < row.ItemArray.Length; i++)
                    {
                        listRow[i] = row[i];
                    }

                    lists.Rows.Add(listRow);
                }
                //lists.Add(Ticket.GetCurrentTicket(context, "NPR", nprIdvalue));
            }
            //return Ticket.GetCurrentTicket(context, "NPR", nprRequest);
            return lists.Select();
        }
        private void LoadBasicDetails(DataRow[] items)
        {
            if (items != null)
            {
                pmmProjectName.Text = string.Empty;
                if (pmmProjectName.Text == string.Empty)
                {
                    List<string> listTitles = items.AsQueryable().Select(x => Convert.ToString(x[DatabaseObjects.Columns.Title]).Trim()).Distinct().ToList();

                    if (listTitles.Count > 1)
                    {
                        lblPmmProjectName.Text = "&lt;Value Varies&gt;";
                        lblPmmProjectName.Visible = true;
                        pmmProjectName.Visible = false;
                    }
                    else
                    {
                        if (items.Count() > 1)
                        {
                            lblPmmProjectName.Text = items[0][DatabaseObjects.Columns.Title].ToString(); ;
                            lblPmmProjectName.Visible = true;
                            pmmProjectName.Visible = false;
                        }
                        else
                        {
                            pmmProjectName.Text = items[0][DatabaseObjects.Columns.Title].ToString();
                            projectName = items[0][DatabaseObjects.Columns.Title].ToString();
                        }
                    }
                    //pmmProjectName.Text = items[DatabaseObjects.Columns.Title].ToString();
                    //projectName = items[DatabaseObjects.Columns.Title].ToString();
                }

                //pmmSponsor.SetValues(Convert.ToString(items[0][DatabaseObjects.Columns.TicketSponsors]));

                List<string> listSponsors = items.AsQueryable().Select(x => Convert.ToString(x[DatabaseObjects.Columns.TicketSponsors]).Trim()).Distinct().ToList();
                if (listSponsors.Count > 1)
                {
                    //SponsorsView.Visible = true;
                    lblSponsors.Text = "&lt;Value Varies&gt;";
                    //SponsorsWrapper.Style.Add("display", "none");
                    pmmSponsor.Visible = false;
                }
                else
                {
                    if (items[0][DatabaseObjects.Columns.TicketSponsors] != null)
                        pmmSponsor.SetValues(Convert.ToString(items[0][DatabaseObjects.Columns.TicketSponsors]));
                }

                //if (pmmSponsor.CommaSeparatedAccounts == string.Empty && item[DatabaseObjects.Columns.TicketSponsors] != null)
                //{
                //    pmmSponsor.UpdateEntities(uHelper.getUsersListFromField(item, DatabaseObjects.Columns.TicketSponsors));
                //}

                dtcStartDate.NullText = null;
                if (UGITUtility.IsSPItemExist(items[0], DatabaseObjects.Columns.TicketActualStartDate))
                    dtcStartDate.Date = DateTime.Parse(Convert.ToString(items[0][DatabaseObjects.Columns.TicketActualStartDate]));


                //pmmProjectManager.SetValues(Convert.ToString(items[0][DatabaseObjects.Columns.TicketProjectManager]));
                List<string> listPMs = items.AsQueryable().Select(x => Convert.ToString(x[DatabaseObjects.Columns.TicketProjectManager]).Trim()).Distinct().ToList();

                if (listPMs.Count > 1)
                {
                    //ProjectManagerView.Visible = true;
                    lblProjectManager.Text = "&lt;Value Varies&gt;";
                    //ProjectManagerWrapper.Style.Add("display", "none");
                    pmmProjectManager.Visible = false;
                }
                else
                {
                    if (items[0][DatabaseObjects.Columns.TicketProjectManager] != null)
                        pmmProjectManager.SetValues(Convert.ToString(items[0][DatabaseObjects.Columns.TicketProjectManager]));
                }


                //pmmProjectManager.CommaSeparatedAccounts = string.Empty;
                //if (pmmProjectManager.CommaSeparatedAccounts == string.Empty && item[DatabaseObjects.Columns.TicketProjectManager] != null)
                //{
                //    pmmProjectManager.UpdateEntities(uHelper.getUsersListFromField(item, DatabaseObjects.Columns.TicketProjectManager));
                //}

                bool includeAllProjectMonitors = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.IncludeAllProjectMonitors);
                if (!includeAllProjectMonitors)
                {
                    pMonitors.Visible = true;
                }

                pImportTaskOptions.Visible = false;
                btImportTempleSchedule.Checked = true;
                //List<UGITTask> nprTasks = UGITTaskHelper.LoadByProjectID(SPContext.Current.Web, "NPR", item.ID);
                //if (nprTasks.Count > 0)
                //{
                //    pImportTaskOptions.Visible = true;
                //    btImportNPRSchedule.Checked = true;
                //    btImportTempleSchedule.Checked = false;
                //}
            }

        }
        protected void ddlLifeCycleModel_Init(object sender, EventArgs e)
        {
            ObjModuleViewManager = new ModuleViewManager(context);
            UGITModule moduleObj = ObjModuleViewManager.LoadByName("PMM");
            if (moduleObj != null && moduleObj.List_LifeCycles != null && moduleObj.List_LifeCycles.Count > 0)
            {
                foreach (LifeCycle cycle in moduleObj.List_LifeCycles)
                {
                    ddlLifeCycleModel.Items.Add(new DevExpress.Web.ListEditItem(cycle.Name, cycle.ID));
                }
            }
        }
        protected void cbAllTemplates_CheckedChanged(object sender, EventArgs e)
        {
            ddlLifeCycleModel_SelectedIndexChanged(ddlLifeCycleModel, new EventArgs());
        }

        //protected void ddlTaskTemplates_Init(object sender, EventArgs e)
        //{
        //    TaskTemplateHelper templateHelper = new TaskTemplateHelper(SPContext.Current.Web);
        //    DataTable templates = templateHelper.LoadTemplates();
        //    ddlTaskTemplates.ClearSelection();
        //    ddlTaskTemplates.Items.Clear();

        //    if (templates != null)
        //    {
        //        foreach (DataRow row in templates.Rows)
        //        {
        //            ddlTaskTemplates.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.Title]), Convert.ToString(row[DatabaseObjects.Columns.Id])));
        //        }
        //    }

        //    ddlTaskTemplates.Items.Insert(0, new ListItem("Default", string.Empty));
        //}

        protected void InportSchedule_CheckedChanged(object sender, EventArgs e)
        {
            // ActivateStep(3);
            //  setStage3Data();
        }

        protected void ddlLifeCycleModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActivateStep(3);
            setStage3Data();
        }

        /// <summary>
        /// set and reset Lifecycle Stage GUI an
        /// </summary>
        private void setStage3Data()
        {
            pImportFromTempalte.Visible = false;
            if (btImportTempleSchedule.Checked)
            {
                pImportFromTempalte.Visible = true;
            }

            LifeCycleManager lcHelper = new LifeCycleManager(context);
            List<LifeCycle> lifeCycles = lcHelper.LoadLifeCycleByModule("PMM");
            LifeCycle selectedLifeCycle = lifeCycles.FirstOrDefault(x => x.ID == Convert.ToInt16(ddlLifeCycleModel.SelectedItem.Value));
            if (selectedLifeCycle != null)
            {
                LifeCycleGUI ctr2 = (LifeCycleGUI)Page.LoadControl("~/CONTROLTEMPLATES/Shared/LifeCycleGUI.ascx");
                ctr2.ModuleLifeCycle = selectedLifeCycle;
                pLifeCycleStageGUI.Controls.Add(ctr2);
                // lcsgraphics.Controls.Add(ctr2);
            }
        }

        protected void monitorsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CheckBox checkbox = (CheckBox)e.Item.FindControl("checkbox");
            if (checkbox != null && (checkbox.Text == "On Budget" || checkbox.Text == "On Time") && checkbox.Checked)
            {
                CheckBox autocalc = (CheckBox)e.Item.FindControl("chkautocalc");
                if (autocalc != null)
                {
                    autocalc.Visible = true;
                    autocalc.Checked = true;
                }
            }
        }
    }
}
