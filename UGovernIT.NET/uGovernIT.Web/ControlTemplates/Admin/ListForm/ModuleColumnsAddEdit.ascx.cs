
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
using uGovernIT.Util.Cache;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;


namespace uGovernIT.Web
{
    public partial class ModuleColumnsAddEdit : UserControl
    {
        public long moduleColumnID { private get; set; }
        IModuleColumnManager moduleColumnManager;
        ModuleViewManager ObjModuleManager;
        ModuleColumn moduleColumn;
        public string Module { get; set; }
        //private DataTable ticketList;
        //ILookup<string, DataRow> existingColumnsDataLookup;
        public int ViewType { get; set; }
        private Dictionary<string, string> dicNonModuleMapping;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
        List<UserProfile> userList = new List<UserProfile>();
        UserProfileManager UserP = HttpContext.Current.GetUserManager();
        List<ModuleColumn> moduleColumnsList = new List<ModuleColumn>();
        ConfigurationVariableManager ConfigManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        public ModuleColumnsAddEdit()
        {
            moduleColumnManager = new ModuleColumnManager(Context.GetManagerContext());
            ObjModuleManager = new ModuleViewManager(context);

        }
        protected override void OnInit(EventArgs e)
        {
            dicNonModuleMapping = new Dictionary<string, string>
                                    {{ Constants.SVCTask,DatabaseObjects.Tables.PMMProjects} ,{Constants.BusinessInitiatives,DatabaseObjects.Tables.PMMProjects}, { Constants.MyAssets, DatabaseObjects.Tables.Assets }, { Constants.MyDashboardIssues,DatabaseObjects.Tables.DashboardSummary }, { Constants.MyDashboardTicket, DatabaseObjects.Tables.DashboardSummary }, { Constants.MyHomeTicketTab, "" },
                                    { Constants.MyProjectTab,DatabaseObjects.Tables.PMMProjects },{ Constants.MyTaskTab,"" },{ Constants.ResourceTab,"SiteUserInfoList"},{ Constants.PMMWorkItemDropdown,DatabaseObjects.Tables.PMMProjects}
                                    };

            if (moduleColumnID > 0)
                moduleColumn = moduleColumnManager.LoadByID(moduleColumnID);

            moduleColumnsList = moduleColumnManager.Load($"{DatabaseObjects.Columns.CategoryName}='{Module}'").OrderBy(x => x.FieldSequence).ToList();
            BindSeqSortDropDownList();
            BindModule();
            FillFieldNames();
            FillColumnType();
            //SpDelta 40(Request List Config:  Define columns to show by tab-->Adding Show in tabs dropdown,adding and updating data to modulecolumns inside request List.)  
            if (ViewType == 0)
            {
                chkDisplay.Visible = false;
                BindShowInTabsDropDown();
            }
            //
            if (!IsPostBack)
            {
                Fill();
            }
            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            HideFields();
            if (ViewType > 0 && !IsPostBack)
            {
                ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(Module));
            }
            //base.OnLoad(e);
        }

        void HideFields()
        {
            ddlSortOrder.Visible = true;
            ddlSortDirection.Visible = true;
            chkIsUseInWildCard.Visible = true;
            chkDisplayForReport.Visible = true;
            tr6.Visible = true;
            tr8.Visible = true;
            if (ViewType > 0)
            {
                chkIsUseInWildCard.Visible = false;
                chkDisplayForReport.Visible = false;
                tr6.Visible = false;
                tr8.Visible = false;
                Div1.Visible = false;
            }
            else
            {
                //tr3.Attributes.Add("class", "hide");
                tr7.Attributes.Add("class", "hide");
            }
        }
        void BindColumnType()
        {
            //SPField field = SPListHelper.GetSPField(SPTickets, cmbFieldName.Text);
            //if (field != null)
            //{
            //    if (cmbColumnType.Items.FindByTextWithTrim(Convert.ToString(field.Type)) != null)
            //    {
            //        cmbColumnType.Text = Convert.ToString(field.Type);
            //    }
            //    else
            //    {
            //        cmbColumnType.Text = "String";
            //    }
            //}

        }


        void FillFieldNames()
        {
            cmbFieldName.Items.Clear();
            List<string> fields = new List<string>();
            UGITModule ugitModule = ObjModuleManager.GetByName(Module);
            FieldConfigurationManager fieldColl = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
            string currentCategory = Module;
            string selectedValue = Convert.ToString(Request.QueryString["moduleType"]);
            if (selectedValue != "1")
            {
                string moduleTblName = ugitModule.ModuleTable;
                TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
                DataTable dt = ticketManager.GetTableSchemaDetail(DatabaseObjects.Tables.InformationSchema, moduleTblName);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToString(dr[DatabaseObjects.Columns.ColumnNameSchema]) != "ContentType" || Convert.ToString(dr[DatabaseObjects.Columns.ColumnNameSchema]) != "Title")
                        {
                            fields.Add(Convert.ToString(dr[DatabaseObjects.Columns.ColumnNameSchema]));
                        }
                    }
                }
                if (!fields.Contains(DatabaseObjects.Columns.SelfAssign) && fields.Contains(DatabaseObjects.Columns.PRPGroup))
                    fields.Add(DatabaseObjects.Columns.SelfAssign);
            }


            else
            {
                if (!string.IsNullOrEmpty(currentCategory))
                {

                    List<string> dtSchema = null;

                    if (currentCategory.ToLower() == Constants.ResourceTab.ToLower())
                    {
                        //dtProfileSchema = UserProfile.GetCustomizeTable(uProfile, string.Empty);
                        //dtSchema = GetCustomizeTable();

                        //if (dtSchema != null && dtSchema.Count > 0)
                        //    fields = dtSchema;

                        //Code added by Inderjeet Kaur on 22-09-2022 to pick column names from DB schema in case table name is AspNetUsers.
                        DataTable dt = ticketManager.GetTableSchemaDetail(DatabaseObjects.Tables.InformationSchema, DatabaseObjects.Tables.AspNetUsers);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (Convert.ToString(dr[DatabaseObjects.Columns.ColumnNameSchema]) != "ContentType" || Convert.ToString(dr[DatabaseObjects.Columns.ColumnNameSchema]) != "Title")
                                {
                                    fields.Add(Convert.ToString(dr[DatabaseObjects.Columns.ColumnNameSchema]));
                                }
                            }
                        }
                    }
                    else if (currentCategory.ToLower() == Constants.MyTaskTab.ToLower())
                    {
                        dtSchema = TaskColumns();
                        if (dtSchema != null && dtSchema.Count > 0)
                            fields = dtSchema;
                    }
                    else if (currentCategory.ToLower() == Constants.SVCTask.ToLower())
                    {
                        dtSchema = TaskColumns();
                        if (dtSchema != null && dtSchema.Count > 0)
                            fields = dtSchema;
                    }
                    else if (currentCategory.ToLower() == Constants.MyHomeTicketTab.ToLower())
                    {
                        dtSchema = MyHomeTask();
                        if (dtSchema != null && dtSchema.Count > 0)
                            fields = dtSchema;
                    }
                    else if (currentCategory.ToLower() == Constants.MyAssets.ToLower())
                    {
                        DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.InformationSchema, $"Table_Name='" + DatabaseObjects.Tables.Assets + "'");
                        dtSchema = dt.AsEnumerable()
                           .Select(r => r.Field<string>("Column_Name"))
                           .ToList();
                        //dtSchema = MyAssestes();
                        if (dtSchema != null && dtSchema.Count > 0)
                            fields = dtSchema;
                    }
                    else if (currentCategory.ToLower() == Constants.MyDashboardTicket.ToLower())
                    {
                        dtSchema = MyDashboardTicket();
                        if (dtSchema != null && dtSchema.Count > 0)
                            fields = dtSchema;
                    }
                    else if (currentCategory.ToLower() == Constants.TSKTask.ToLower())
                    {
                        dtSchema = TaskColumns();
                        if (dtSchema != null && dtSchema.Count > 0)
                            fields = dtSchema;
                    }
                    else if (currentCategory.ToLower() == Constants.MyDashboardIssues.ToLower())
                    {
                        dtSchema = MyDashboardIssues();
                        if (dtSchema != null && dtSchema.Count > 0)
                            fields = dtSchema;
                    }
                    else if (currentCategory.ToLower() == Constants.NPRResource.ToLower())
                    {
                        DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.InformationSchema, $"Table_Name='" + DatabaseObjects.Tables.NPRResources + "'");
                        dtSchema = dt.AsEnumerable()
                           .Select(r => r.Field<string>("Column_Name"))
                           .ToList();
                        //dtSchema = MyAssestes();
                        if (dtSchema != null && dtSchema.Count > 0)
                            fields = dtSchema;
                    }
                    else if (currentCategory.ToLower() == Constants.NPRBudget.ToLower() || currentCategory == "PMMBudget")
                    {
                        dtSchema = NPRBudget();
                        if (dtSchema != null && dtSchema.Count > 0)
                            fields = dtSchema;
                    }
                    else if (currentCategory.ToLower() == Constants.PMMActuals.ToLower())
                    {
                        dtSchema = PMMActuals();
                        if (dtSchema != null && dtSchema.Count > 0)
                            fields = dtSchema;
                    }
                    else if (currentCategory.ToLower() == Constants.MyProjectTab.ToLower())
                    {
                        dtSchema = MyProjectTab();
                        if (dtSchema != null && dtSchema.Count > 0)
                            fields = dtSchema;
                    }
                    if (!fields.Contains(DatabaseObjects.Columns.SelfAssign))
                        fields.Add(DatabaseObjects.Columns.SelfAssign);
                }
            }
            //removing already added column fields from field dropdown
            if (moduleColumnsList != null && moduleColumnsList.Count != 0)
            {
                foreach (ModuleColumn moduleColumn in moduleColumnsList)
                {
                    if (fields.Contains(moduleColumn.FieldName))
                    {
                        fields.RemoveAll(x => x == moduleColumn.FieldName);
                    }
                }
            }

            if ((Module == ModuleNames.CPR || Module == ModuleNames.OPM || Module == ModuleNames.CNS) && (!fields.Contains(Constants.CRMSummary)))
            {
                fields.Add(Constants.CRMSummary);
            }
            if ((Module == ModuleNames.CPR || Module == ModuleNames.OPM || Module == ModuleNames.CNS) && (!fields.Contains(Constants.CRMAllocationCount)))
            {
                fields.Add(Constants.CRMAllocationCount);
            }
            //fields.Sort();
            cmbFieldName.DataSource = fields.OrderBy(q => q).ToList();
            cmbFieldName.DataBind();
        }

        private List<string> MyProjectTab()
        {
            List<string> myTaskData = new List<string>();
            myTaskData.Add(DatabaseObjects.Columns.ProjectMonitorName);
            myTaskData.Add(DatabaseObjects.Columns.TicketBeneficiaries);
            myTaskData.Add(DatabaseObjects.Columns.ProjectInitiativeLookup);
            myTaskData.Add(DatabaseObjects.Columns.TicketProjectScore);
            myTaskData.Add(DatabaseObjects.Columns.TicketDesiredCompletionDate);
            myTaskData.Add(DatabaseObjects.Columns.TicketPctComplete);
            myTaskData.Add(DatabaseObjects.Columns.TicketProjectManager);
            myTaskData.Add(DatabaseObjects.Columns.Status);
            myTaskData.Add(DatabaseObjects.Columns.TicketPriorityLookup);
            myTaskData.Add(DatabaseObjects.Columns.TicketRequestTypeLookup);
            myTaskData.Add(DatabaseObjects.Columns.Title);
            myTaskData.Add(DatabaseObjects.Columns.TicketId);
            myTaskData.Add(DatabaseObjects.Columns.Attachments);

            return myTaskData.OrderBy(x => x).ToList();
        }

        private List<string> PMMActuals()
        {
            List<string> myTaskData = new List<string>();
            myTaskData.Add(DatabaseObjects.Columns.ModuleBudgetLookup);
            myTaskData.Add(DatabaseObjects.Columns.BudgetDescription);
            myTaskData.Add(DatabaseObjects.Columns.VendorLookup);
            myTaskData.Add(DatabaseObjects.Columns.InvoiceNumber);
            myTaskData.Add(DatabaseObjects.Columns.BudgetAmount);
            myTaskData.Add(DatabaseObjects.Columns.AllocationEndDate);
            myTaskData.Add(DatabaseObjects.Columns.AllocationStartDate);
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.InformationSchema, $"Table_Name='" + DatabaseObjects.Tables.ModuleBudgetActuals + "'");
            List<string> dtfields = dt.AsEnumerable()
               .Select(r => r.Field<string>("Column_Name"))
               .ToList();

            if (dtfields != null)
                myTaskData.AddRange(dtfields);

            return myTaskData.Distinct()?.OrderBy(x => x).ToList();
        }

        private List<string> NPRBudget()
        {
            List<string> myTaskData = new List<string>();
            myTaskData.Add(DatabaseObjects.Columns.BudgetDescription);
            myTaskData.Add(DatabaseObjects.Columns.AllocationEndDate);
            myTaskData.Add(DatabaseObjects.Columns.AllocationStartDate);
            myTaskData.Add(DatabaseObjects.Columns.BudgetAmount);
            myTaskData.Add(DatabaseObjects.Columns.BudgetItem);
            myTaskData.Add(DatabaseObjects.Columns.BudgetSubCategory);
            myTaskData.Add(DatabaseObjects.Columns.BudgetCategory);
            myTaskData.Add(DatabaseObjects.Columns.Title);
            myTaskData.Add(DatabaseObjects.Columns.IsAutoCalculated);

            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.InformationSchema, $"Table_Name='" + DatabaseObjects.Tables.ModuleBudget + "'");
            List<string> dtcolumns = dt.AsEnumerable()
               .Select(r => r.Field<string>("Column_Name"))
               .ToList();

            if (dtcolumns != null)
                myTaskData.AddRange(dtcolumns);

            return myTaskData.Distinct()?.OrderBy(x => x).ToList();
        }

        private List<string> MyDashboardTicket()
        {
            List<string> myTaskData = new List<string>();
            myTaskData.Add(DatabaseObjects.Columns.TicketCreationDate);
            myTaskData.Add(DatabaseObjects.Columns.TicketAge);
            myTaskData.Add(DatabaseObjects.Columns.ModuleStageMultiLookup);
            myTaskData.Add(DatabaseObjects.Columns.Status);
            myTaskData.Add(DatabaseObjects.Columns.TicketPriorityLookup);
            myTaskData.Add(DatabaseObjects.Columns.Title);
            myTaskData.Add(DatabaseObjects.Columns.TicketId);
            myTaskData.Add(DatabaseObjects.Columns.TicketDesiredCompletionDate);
            myTaskData.Add(DatabaseObjects.Columns.TicketRequestor);
            myTaskData.Add(DatabaseObjects.Columns.PRPGroup);

            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.InformationSchema, $"Table_Name='" + DatabaseObjects.Tables.DashboardSummary + "'");
            List<string> dtFields = dt.AsEnumerable()
               .Select(r => r.Field<string>("Column_Name"))
               .ToList();

            if (dtFields != null)
                myTaskData.AddRange(dtFields);

            return myTaskData.Distinct()?.OrderBy(x => x).ToList();
        }

        private List<string> MyDashboardIssues()
        {
            List<string> myTaskData = new List<string>();
            myTaskData.Add(DatabaseObjects.Columns.AssignedTo);
            myTaskData.Add(DatabaseObjects.Columns.Created);
            myTaskData.Add(DatabaseObjects.Columns.PercentComplete);
            myTaskData.Add(DatabaseObjects.Columns.Status);
            myTaskData.Add(DatabaseObjects.Columns.IssueImpact);
            myTaskData.Add(DatabaseObjects.Columns.StartDate);
            myTaskData.Add(DatabaseObjects.Columns.Priority);
            myTaskData.Add(DatabaseObjects.Columns.Title);

            return myTaskData.OrderBy(x => x).ToList();
        }

        private List<string> TSKTask()
        {
            List<string> myTaskData = new List<string>();
            myTaskData.Add(DatabaseObjects.Columns.CompletionDate);
            myTaskData.Add(DatabaseObjects.Columns.StageStep);
            myTaskData.Add(DatabaseObjects.Columns.IsMilestone);
            myTaskData.Add(DatabaseObjects.Columns.Duration);
            myTaskData.Add(DatabaseObjects.Columns.DueDate);
            myTaskData.Add(DatabaseObjects.Columns.StartDate);
            myTaskData.Add(DatabaseObjects.Columns.EstimatedRemainingHours);
            myTaskData.Add(DatabaseObjects.Columns.TaskActualHours);
            myTaskData.Add(DatabaseObjects.Columns.TaskEstimatedHours);
            myTaskData.Add(DatabaseObjects.Columns.Predecessors);
            myTaskData.Add(DatabaseObjects.Columns.AssignedTo);
            myTaskData.Add(DatabaseObjects.Columns.Status);
            myTaskData.Add(DatabaseObjects.Columns.PercentComplete);
            myTaskData.Add(DatabaseObjects.Columns.TaskBehaviour);
            myTaskData.Add(DatabaseObjects.Columns.Title);
            myTaskData.Add(DatabaseObjects.Columns.ID);
            myTaskData.Add(DatabaseObjects.Columns.ItemOrder);
            myTaskData.Add(DatabaseObjects.Columns.ActualHours);
            //myTaskData.Add(DatabaseObjects.Columns.EstimatedHours);
            myTaskData.Add(DatabaseObjects.Columns.ModuleName);
            myTaskData.Add(DatabaseObjects.Columns.RequestCategoryType);

            return myTaskData.OrderBy(x => x).ToList();
        }

        private List<string> MyAssestes()
        {
            List<string> myTaskData = new List<string>();
            myTaskData.Add(DatabaseObjects.Columns.Status);
            myTaskData.Add(DatabaseObjects.Columns.HostName);
            myTaskData.Add(DatabaseObjects.Columns.AssetModelLookup);
            myTaskData.Add(DatabaseObjects.Columns.AssetName);
            myTaskData.Add(DatabaseObjects.Columns.AssetTagNum);
            myTaskData.Add(DatabaseObjects.Columns.AcquisitionDate);
            //myTaskData.Add(DatabaseObjects.Columns.);
            //myTaskData.Add(DatabaseObjects.Columns.AssetTagNum);
            //myTaskData.Add(DatabaseObjects.Columns.AssetTagNum);
            //myTaskData.Add(DatabaseObjects.Columns.AssetTagNum);
            //myTaskData.Add(DatabaseObjects.Columns.AssetTagNum);
            //myTaskData.Add(DatabaseObjects.Columns.AssetTagNum);
            //myTaskData.Add(DatabaseObjects.Columns.AssetTagNum);

            return myTaskData.OrderBy(x => x).ToList();
        }

        private List<string> MyHomeTask()
        {
            List<string> myTaskData = new List<string>();
            myTaskData.Add(DatabaseObjects.Columns.TicketId);
            myTaskData.Add(DatabaseObjects.Columns.Title);
            myTaskData.Add(DatabaseObjects.Columns.TicketPriorityLookup);
            myTaskData.Add(DatabaseObjects.Columns.Status);
            myTaskData.Add(DatabaseObjects.Columns.ModuleStageMultiLookup);
            myTaskData.Add(DatabaseObjects.Columns.TicketAge);
            myTaskData.Add(DatabaseObjects.Columns.CloseDate);
            myTaskData.Add(DatabaseObjects.Columns.TicketDesiredCompletionDate);
            myTaskData.Add(DatabaseObjects.Columns.TicketRequestor);
            myTaskData.Add(DatabaseObjects.Columns.LocationLookup);
            myTaskData.Add(DatabaseObjects.Columns.PRPGroup);
            myTaskData.Add(DatabaseObjects.Columns.TicketStageActionUsers);
            myTaskData.Add(DatabaseObjects.Columns.PRP);
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            List<UGITModule> lstmodule = moduleManager.Load(x => x.EnableModule);

            if (lstmodule != null)
            {
                foreach (UGITModule module in lstmodule)
                {
                    TicketManager ticketManager = new TicketManager(context);
                    DataTable tickets = ticketManager.GetOpenTickets(module);
                    if (tickets != null && tickets.Columns.Count > 0)
                    {
                        string[] columnNames = tickets.Columns.Cast<DataColumn>()
                                     .Select(x => x.ColumnName)
                                     .ToArray();
                        myTaskData.AddRange(columnNames);
                    }
                }
            }
            return myTaskData.Distinct()?.OrderBy(x => x).ToList();
        }

        public static List<string> GetCustomizeTable()
        {
            List<string> myTaskData = new List<string>();
            myTaskData.Add(DatabaseObjects.Columns.ManagerLink);
            myTaskData.Add(DatabaseObjects.Columns.TitleLink);
            myTaskData.Add(DatabaseObjects.Columns.ID);
            myTaskData.Add(DatabaseObjects.Columns.Skills);
            myTaskData.Add("RoleName");
            myTaskData.Add(DatabaseObjects.Columns.IsManager);
            myTaskData.Add(DatabaseObjects.Columns.IsConsultant);
            myTaskData.Add(DatabaseObjects.Columns.IsIT);
            myTaskData.Add(DatabaseObjects.Columns.ManagerID);
            myTaskData.Add(DatabaseObjects.Columns.Location);
            myTaskData.Add(DatabaseObjects.Columns.FunctionalArea);
            myTaskData.Add(DatabaseObjects.Columns.PhoneNumber);

            return myTaskData.OrderBy(x => x).ToList();
        }

        public static List<string> CreateSchema()
        {
            List<string> myTaskData = new List<string>();
            myTaskData.Add(DatabaseObjects.Columns.ActionType);
            myTaskData.Add(DatabaseObjects.Columns.AssignedTo);
            myTaskData.Add(DatabaseObjects.Columns.DueDate);
            myTaskData.Add(DatabaseObjects.Columns.PercentComplete);
            myTaskData.Add(DatabaseObjects.Columns.Status);
            myTaskData.Add(DatabaseObjects.Columns.Title);

            myTaskData.Add(DatabaseObjects.Columns.Id);
            myTaskData.Add(DatabaseObjects.Columns.ProjectID);
            myTaskData.Add("ProjectTitle");
            myTaskData.Add(DatabaseObjects.Columns.ModuleName);
            myTaskData.Add(DatabaseObjects.Columns.Description);
            myTaskData.Add(DatabaseObjects.Columns.StartDate);
            myTaskData.Add(DatabaseObjects.Columns.Predecessors);
            myTaskData.Add(DatabaseObjects.Columns.PredecessorsID);
            myTaskData.Add(DatabaseObjects.Columns.PredecessorsByOrder);
            myTaskData.Add(DatabaseObjects.Columns.AssignedToID);
            myTaskData.Add(DatabaseObjects.Columns.ParentTask);
            myTaskData.Add(DatabaseObjects.Columns.Priority);
            myTaskData.Add(DatabaseObjects.Columns.ItemOrder);
            myTaskData.Add(DatabaseObjects.Columns.UGITDuration);
            myTaskData.Add(DatabaseObjects.Columns.UGITContribution);
            myTaskData.Add(DatabaseObjects.Columns.TaskActualHours);
            myTaskData.Add(DatabaseObjects.Columns.TaskEstimatedHours);
            myTaskData.Add(DatabaseObjects.Columns.Modified);
            myTaskData.Add(DatabaseObjects.Columns.UGITLevel);
            myTaskData.Add(DatabaseObjects.Columns.UGITChildCount);
            myTaskData.Add(DatabaseObjects.Columns.UGITProposedDate);
            myTaskData.Add(DatabaseObjects.Columns.UGITProposedStatus);
            myTaskData.Add(DatabaseObjects.Columns.IsMilestone);
            myTaskData.Add(DatabaseObjects.Columns.StageStep);
            myTaskData.Add(DatabaseObjects.Columns.TaskBehaviour);

            myTaskData.Add(DatabaseObjects.Columns.UGITAssignToPct);
            myTaskData.Add(DatabaseObjects.Columns.EstimatedRemainingHours);
            myTaskData.Add(DatabaseObjects.Columns.UGITComment);
            myTaskData.Add(DatabaseObjects.Columns.TaskReminderDays);
            myTaskData.Add(DatabaseObjects.Columns.TaskReminderEnabled);
            myTaskData.Add(DatabaseObjects.Columns.IsCritical);
            myTaskData.Add(DatabaseObjects.Columns.SprintLookup);

            myTaskData.Add(DatabaseObjects.Columns.TaskSkill);
            myTaskData.Add(DatabaseObjects.Columns.TaskSkillId);

            myTaskData.Add(DatabaseObjects.Columns.CompletionDate);
            myTaskData.Add(DatabaseObjects.Columns.CompletedBy);
            myTaskData.Add(DatabaseObjects.Columns.ShowOnProjectCalendar);
            myTaskData.Add(DatabaseObjects.Columns.TaskRepeatInterval);
            return myTaskData.OrderBy(x => x).ToList();
        }

        public static List<string> TaskColumns()
        {
            List<string> myTaskData = new List<string>();
            myTaskData.Add(DatabaseObjects.Columns.ItemOrder);
            myTaskData.Add(DatabaseObjects.Columns.ID);
            myTaskData.Add(DatabaseObjects.Columns.Title);
            myTaskData.Add(DatabaseObjects.Columns.TaskBehaviour);
            myTaskData.Add(DatabaseObjects.Columns.PercentComplete);
            myTaskData.Add(DatabaseObjects.Columns.Status);
            myTaskData.Add(DatabaseObjects.Columns.AssignedTo);
            myTaskData.Add(DatabaseObjects.Columns.Predecessors);
            myTaskData.Add(DatabaseObjects.Columns.DueDate);
            myTaskData.Add(DatabaseObjects.Columns.RelatedTicketID);
            myTaskData.Add(DatabaseObjects.Columns.EstimatedHours);

            UGITTaskManager taskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
            DataTable dtTask = taskManager.GetDataTable();
            if (dtTask != null)
            {
                string[] fields = dtTask.Columns.Cast<DataColumn>()
                                     .Select(x => x.ColumnName)
                                     .ToArray();
                myTaskData.AddRange(fields);
            }

            return myTaskData.Distinct()?.OrderBy(x => x).ToList();
        }

        void FillColumnType()
        {
            List<string> list = new List<string>() {"Boolean", "String", "SLADate", "SLADateTime", "User", "Currency", "DateTime", "Date",
                                                     "IndicatorLight", "Lookup", "MultiUser", "MultiLookup", "Percentage", "ProgressBar" };
            list.Sort();
            cmbColumnType.DataSource = list;
            cmbColumnType.DataBind();
        }
        private void BindModule()
        {
            ddlModule.Items.Clear();
            List<UGITModule> lstModule = ObjModuleManager.Load();
            string selectedValue = Convert.ToString(Request.QueryString["moduleType"]);
            if (selectedValue != "1")
            {
                ModuleLabel.InnerText = "Module";
                if (lstModule != null && lstModule.Count > 0)
                {
                    lstModule = lstModule.Where(x => x.EnableModule).OrderBy(x => x.ModuleName).ToList();

                    ddlModule.DataSource = lstModule;
                    ddlModule.DataValueField = DatabaseObjects.Columns.ModuleName;
                    ddlModule.DataTextField = DatabaseObjects.Columns.Title;

                }
            }
            else
            {
                ModuleLabel.InnerText = "Category";
                var spNonModuleList = moduleColumnManager.Load().OrderBy(x => x.CategoryName).Select(x => x.CategoryName);
                List<string> listModule = lstModule.Select(x => x.ModuleName).ToList();
                List<string> listAllModule = spNonModuleList.Select(x => Convert.ToString(x)).Distinct().ToList();
                List<string> listNonModule = listAllModule.Except(listModule).ToList();
                listNonModule.ForEach(x =>
                {
                    ddlModule.Items.Add(new ListItem { Text = x, Value = x });
                });
            }

            ddlModule.DataBind();
        }
        void Fill()
        {
            if (moduleColumn != null)
            {
                cmbFieldName.Text = moduleColumn.FieldName;
                txtDisplayName.Text = moduleColumn.FieldDisplayName;
                ddlModule.SelectedValue = moduleColumn.CategoryName;
                cmbColumnType.Text = moduleColumn.ColumnType;
                bool isTruncated = UGITUtility.IfColumnExists(DatabaseObjects.Columns.TruncateTextTo, UGITUtility.ObjectToData(moduleColumn)) && UGITUtility.StringToInt(moduleColumn.TruncateTextTo) > 0;
                if (cmbColumnType.Text.ToLower() == "string")
                {
                    chkTruncate.Checked = isTruncated;
                    if (chkTruncate.Checked)
                    {
                        divNoOfChars.Visible = true;
                        divTruncateText.Visible = true;
                        spnNoOfChars.Value = moduleColumn.TruncateTextTo;
                    }
                }
                txtCustomProperties.Text = moduleColumn.CustomProperties;
                chkDisplay.Checked = moduleColumn.IsDisplay;
                chkShowInMobile.Checked = moduleColumn.ShowInMobile;
                chkIsUseInWildCard.Checked = moduleColumn.IsUseInWildCard;
                chkDisplayForClosed.Checked = UGITUtility.StringToBoolean(moduleColumn.DisplayForClosed);
                chkDisplayForReport.Checked = UGITUtility.StringToBoolean(moduleColumn.DisplayForReport);
                chkShowInCardView.Checked = UGITUtility.StringToBoolean(moduleColumn.ShowInCardView);
                ddlFieldSequence.SelectedValue = Convert.ToString(moduleColumn.FieldSequence);
                ddlSortOrder.SelectedValue = Convert.ToString(moduleColumn.SortOrder);
                ddlSortDirection.SelectedValue = moduleColumn.IsAscending.HasValue && moduleColumn.IsAscending.Value == true ? "Ascending" : "Descending";
                //if (ddlTextAlignment.GetValues() == string.Empty)
                //    moduleColumn.TextAlignment = "Center";
                if (moduleColumn.TextAlignment == string.Empty || moduleColumn.TextAlignment == null)
                    moduleColumn.TextAlignment = "Center";
                ddlTextAlignment.SetValues(Convert.ToString(moduleColumn.TextAlignment));
                ////SpDelta 40(Request List Config:  Define columns to show by tab-->Adding Show in tabs dropdown,adding and updating data to modulecolumns inside request List.)
                //ddlTextAlignment.SelectedIndex = ddlTextAlignment.Items.IndexOf(ddlTextAlignment.Items.FindByText(Convert.ToString(moduleColumn[DatabaseObjects.Columns.TextAlignment])));
                if (ViewType == 0)
                {
                    if (!string.IsNullOrEmpty(moduleColumn.SelectedTabs))
                    {
                        // Bind Selected Tabs dropdown
                        ASPxListBox aspxListBox = ddlSelectedTabs.FindControl("listBox") as ASPxListBox;
                        string[] selectedtabs = Convert.ToString(moduleColumn.SelectedTabs).Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);

                        string selectedListItems = string.Empty;

                        // If all tabs are selected for a field then there will be only one value i.e. "all" in selectedtabs array, so we need to skip if value is all
                        if (selectedtabs != null && selectedtabs.Length > 0 && Array.IndexOf(selectedtabs, "all") == -1)
                        {
                            foreach (string tab in selectedtabs)
                            {
                                ListEditItem item = aspxListBox.Items.FindByValue(tab);
                                if (item != null)
                                {
                                    item.Selected = true;

                                    if (!string.IsNullOrEmpty(selectedListItems))
                                        selectedListItems += Constants.Separator5;

                                    selectedListItems += item.Text;
                                }
                            }
                            ddlSelectedTabs.Text = selectedListItems;
                        }

                        #region Select tabs on the basis of Display and Diplay In Closed checkboxes for old fields

                        // Select tabs on the basis of "Display" and "Diplay In Closed" checkboxes for old fields only when any tab name isn't selected in the dropdown for this field
                        bool selectAll = (selectedtabs != null && Array.IndexOf(selectedtabs, "all") != -1)
                            || ((selectedtabs == null || selectedtabs.Length == 0) && chkDisplay.Checked);

                        bool unselectAll = string.IsNullOrEmpty(selectedListItems) && !chkDisplay.Checked && !chkDisplayForClosed.Checked;
                        bool diplayForClosed = string.IsNullOrEmpty(selectedListItems) && chkDisplayForClosed.Checked;

                        if (selectAll)
                        {
                            aspxListBox.SelectAll();
                            ddlSelectedTabs.Text = "All";
                        }
                        else if (unselectAll)
                        {
                            aspxListBox.UnselectAll();
                            ddlSelectedTabs.Text = string.Empty;
                        }
                        else if (diplayForClosed)
                        {
                            ListEditItem item = aspxListBox.Items.FindByValue("allclosedtickets");

                            if (item != null && !item.Selected)
                            {
                                item.Selected = true;
                                ddlSelectedTabs.Text = item.Text;
                            }
                        }
                        #endregion Select tabs on the basis of Display and Diplay In Closed checkboxes for old fields
                    }
                }
            }
            else
            {
                LnkbtnDelete.Visible = false;
                if (!string.IsNullOrEmpty(Module))
                    ddlModule.SelectedValue = Module;


            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<ModuleColumn> lstOfmoduleColumns = new List<ModuleColumn>();
            long Id = 0;
            ModuleColumn moduleColumn = null;
            if (moduleColumnID > 0)
                moduleColumn = moduleColumnManager.LoadByID(moduleColumnID);
            else
                moduleColumn = new ModuleColumn();

            #region reset the sortorder if the current sortorder is used already. 
            // reset the sortorder if the current sortorder is used already.
            int sortOrder = UGITUtility.StringToInt(ddlSortOrder.SelectedValue);
            lstOfmoduleColumns = moduleColumnManager.Load();

            if (!string.IsNullOrEmpty(txtDisplayName.Text.Trim()))
            {
                int DuplicateCount = 0;
                if (lstOfmoduleColumns.Where(x => x.CategoryName == Module && x.FieldDisplayName != null && x.FieldDisplayName.ToLower() == txtDisplayName.Text.Trim().ToLower()) != null)
                    DuplicateCount = lstOfmoduleColumns.Where(x => x.CategoryName == Module && x.FieldDisplayName != null && x.FieldDisplayName.ToLower() == txtDisplayName.Text.Trim().ToLower()).Count();
                ModuleColumn _moduleColumn = lstOfmoduleColumns.Where(x => x.CategoryName == Module && x.FieldDisplayName != null && x.FieldDisplayName.ToLower() == txtDisplayName.Text.Trim().ToLower()).FirstOrDefault();
                if (DuplicateCount == 1 && moduleColumnID > 0 && moduleColumnID == _moduleColumn.ID)
                    moduleColumn.FieldDisplayName = string.IsNullOrEmpty(txtDisplayName.Text.Trim()) ? "" : txtDisplayName.Text.Trim();
                else if (DuplicateCount == 0)
                    moduleColumn.FieldDisplayName = string.IsNullOrEmpty(txtDisplayName.Text.Trim()) ? "" : txtDisplayName.Text.Trim();
                else
                {
                    lblerrormsg.Text = "Display Name already exist";
                    return;
                }
            }
            if (sortOrder > 0)
            {
                lstOfmoduleColumns = lstOfmoduleColumns.Where(x => x.CategoryName == Module && x.SortOrder == sortOrder).ToList();
                foreach (ModuleColumn mColumn in lstOfmoduleColumns)
                {
                    if (mColumn.SortOrder.HasValue && mColumn.SortOrder == sortOrder)
                    {
                        if (mColumn.ID != moduleColumn.ID)
                        {
                            mColumn.SortOrder = null;
                            mColumn.IsAscending = null;
                            moduleColumnManager.Update(mColumn);
                        }
                    }
                }
            }
            #endregion

            moduleColumn.FieldName = cmbFieldName.Text.Trim();

            moduleColumn.CategoryName = ddlModule.SelectedValue;
            moduleColumn.ColumnType = cmbColumnType.Text.Trim();
            bool isTruncate = !string.IsNullOrEmpty(Convert.ToString(cmbColumnType.Value)) & cmbColumnType.Text.ToLower() == "string" & chkTruncate.Checked;

            if (isTruncate)
                moduleColumn.TruncateTextTo = UGITUtility.StringToInt(spnNoOfChars.Value);
            else
                moduleColumn.TruncateTextTo = 0;

            moduleColumn.CustomProperties = txtCustomProperties.Text.Trim();
            moduleColumn.ShowInMobile = chkShowInMobile.Checked;
            moduleColumn.IsUseInWildCard = chkIsUseInWildCard.Checked;
            moduleColumn.DisplayForReport = chkDisplayForReport.Checked;
            moduleColumn.ShowInCardView = chkShowInCardView.Checked;
            if (ddlTextAlignment.GetValues() == string.Empty)
                moduleColumn.TextAlignment = "Center";
            else
                moduleColumn.TextAlignment = ddlTextAlignment.GetValues();

            if (ddlFieldSequence.SelectedValue != string.Empty)
                moduleColumn.FieldSequence = Convert.ToInt32(ddlFieldSequence.SelectedValue);
            else
                moduleColumn.FieldSequence = 0;
            if (ddlSortOrder.SelectedValue != string.Empty)
            {
                moduleColumn.SortOrder = Convert.ToInt32(ddlSortOrder.SelectedValue);
                moduleColumn.IsAscending = ddlSortDirection.SelectedValue == "Ascending" ? true : false;
            }
            else
            {
                moduleColumn.SortOrder = null;
                moduleColumn.IsAscending = null; //ddlSortDirection.SelectedValue == "Ascending" ? true : false;
            }

            moduleColumn.Title = ddlModule.SelectedValue + "-" + cmbFieldName.Text.Trim();
            Id = moduleColumn.ID;
            ////SpDelta 40(Request List Config:  Define columns to show by tab-->Adding Show in tabs dropdown,adding and updating data to modulecolumns inside request List.)
            if (ViewType > 0)
                moduleColumn.Title = string.Format("{0} - {1}", ddlModule.SelectedValue.Trim(), txtDisplayName.Text.Trim());

            else
                moduleColumn.Title = string.Format("{0} - {1}", ddlModule.SelectedValue.Trim().ToString(), txtDisplayName.Text.Trim());

            if (ViewType == 0)
            {
                ASPxListBox aspxListBox = ddlSelectedTabs.FindControl("listBox") as ASPxListBox;
                string selectedTabs = string.Empty;
                if (aspxListBox.Items.Count != aspxListBox.SelectedItems.Count)
                {
                    foreach (ListEditItem selectedItem in aspxListBox.SelectedItems)
                    {
                        if (Convert.ToString(selectedItem.Value) != "All")
                        {
                            if (!string.IsNullOrEmpty(selectedTabs))
                                selectedTabs += Constants.Separator;

                            selectedTabs += Convert.ToString(selectedItem.Value);
                        }
                    }
                }
                else
                {
                    selectedTabs = "all";
                }
                moduleColumn.SelectedTabs = selectedTabs;
                moduleColumn.IsDisplay = false;// We have to use hide/show  column on basis of selectedtabs values and remove conflicts isdisplay
                //if (string.IsNullOrWhiteSpace(selectedTabs))
                if (!string.IsNullOrWhiteSpace(selectedTabs)) // changed condition as updating any field is hiding field from Grid.
                {
                    moduleColumn.IsDisplay = false;
                    moduleColumn.DisplayForClosed = false;
                }
            }
            else
            {
                moduleColumn.IsDisplay = chkDisplay.Checked;
                moduleColumn.DisplayForClosed = chkDisplayForClosed.Checked;
            }

            bool Updated = false;
            if (moduleColumnID > 0)
                Updated = moduleColumnManager.Update(moduleColumn);
            else
                Updated = moduleColumnManager.Insert(moduleColumn) > 0 ? true : false;

            string logMessage = string.Format("Add/Update request list items : {0}", moduleColumn.Title);
            Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, logMessage, Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
            #region Copy code from Sharepoint

            lstOfmoduleColumns = moduleColumnManager.Load();
            if (lstOfmoduleColumns != null && lstOfmoduleColumns.Count > 0)
            {
                lstOfmoduleColumns = lstOfmoduleColumns.OrderBy(x => x.FieldSequence).ToList();
                lstOfmoduleColumns = lstOfmoduleColumns.Where(x => x.CategoryName.Equals(Module)).ToList();
                int sequence = 1;
                foreach (ModuleColumn mCol in lstOfmoduleColumns)
                {
                    mCol.FieldSequence = sequence;
                    moduleColumnManager.Update(mCol);
                    sequence++;
                }
            }


            #endregion



            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        //SpDelta 40(Request List Config:  Define columns to show by tab-->Adding Show in tabs dropdown,adding and updating data to modulecolumns inside request List.)
        #region Method to bind Show in Tabs dropdown
        /// <summary>
        /// This method is used to bind Show in Tabs dropdown
        /// </summary>
        protected void BindShowInTabsDropDown()
        {
            // Get 'Waiting On Me' Tab name from configuration variable
            //string waitingOnMeTabName = ConfigManager.GetValue(ConfigConstants.WaitingOnMeTabName);

            //if (string.IsNullOrEmpty(waitingOnMeTabName))
            //    waitingOnMeTabName = "Waiting on Me";

            /* Moved to UGITUtility in GetTabNames()
            // Creating a dictionary for tab names and key values
            Dictionary<string, string> dictTabs = new Dictionary<string, string>();
            dictTabs.Add("allopentickets", "Open");
            dictTabs.Add("allclosedtickets", "Closed");
            dictTabs.Add("waitingonme", waitingOnMeTabName);
            dictTabs.Add("myopentickets", "My Tickets");
            dictTabs.Add("unassigned", "Unassigned");
            dictTabs.Add("allresolvedtickets", "Resolved");
            dictTabs.Add("onhold", "On-Hold");
            dictTabs.Add("departmentticket", "My Department");
            dictTabs.Add("mygrouptickets", "My Group");
            */

            //Dictionary<string, string> dictTabs = UGITUtility.GetTabNames();
            //dictTabs["waitingonme"] = waitingOnMeTabName;

            //// Bind Show In Tabs dropdown
            ASPxListBox aspxListBox = ddlSelectedTabs.FindControl("listBox") as ASPxListBox;
            ////ASPxListBox aspxListBox = (ASPxListBox)ddlSelectedTabs.FindControl("listBox");

            //foreach (var item in dictTabs)
            //{
            //    aspxListBox.Items.Add(new ListEditItem(item.Value, item.Key));
            //}
            TabViewManager tabViewManager = new TabViewManager(HttpContext.Current.GetManagerContext());
            List<TabView> tabView = tabViewManager.Load(z => z.ModuleNameLookup == ddlModule.SelectedItem.Value);
            foreach (var item in tabView)
            {
                aspxListBox.Items.Add(new ListEditItem(item.TabDisplayName, item.TabName));
            }
            aspxListBox.Items.Insert(0, new ListEditItem("Select All", "all"));
        }
        #endregion Method to bind Show in Tabs dropdown
        //
        private void UpdateSequence(string firstID, string secondID, string CategoryName, bool IsEdit = false)
        {
            List<ModuleColumn> drModuleColumn = moduleColumnManager.Load().Where(x => x.CategoryName == CategoryName).OrderBy(x => x.FieldSequence).ToList();
            ModuleColumn drFirstItem = drModuleColumn.FirstOrDefault(m => m.ID == Convert.ToInt64(firstID.Trim()));
            ModuleColumn drSecondItem = drModuleColumn.Where(m => m.ID == Convert.ToInt64(secondID.Trim())).FirstOrDefault();
            if (drFirstItem != null && drSecondItem != null)
            {
                UpdateSequence(drModuleColumn, drFirstItem, drSecondItem, CategoryName, IsEdit);
            }
        }

        private void UpdateSequence(List<ModuleColumn> drModuleColumn, ModuleColumn drFirstItem, ModuleColumn drSecondItem, string categoryName, bool isEdit = false)
        {
            int firstSeq = Convert.ToInt16(drFirstItem.FieldSequence);
            int secondSeq = Convert.ToInt16(drSecondItem.FieldSequence);
            string firstID = Convert.ToString(drFirstItem.ID);
            //int groupEnd = firstSeq;

            ModuleColumn moduleColumn = new ModuleColumn();

            drModuleColumn = drModuleColumn.Where(x => x.FieldSequence >= secondSeq && x.ID != drFirstItem.ID).OrderBy(x => x.FieldSequence).ToList();

            foreach (ModuleColumn dr in drModuleColumn)
            {
                if (firstSeq >= secondSeq)
                {
                    if (firstSeq == secondSeq && Convert.ToString(dr.ID) != firstID)
                    {
                        dr.FieldSequence = Convert.ToInt16(dr.FieldSequence) + 1;
                    }
                    else
                    {
                        dr.FieldSequence = ++secondSeq;
                    }
                }
            }

            foreach (ModuleColumn dr in drModuleColumn)
            {
                moduleColumn.ID = Convert.ToInt64(dr.ID);
                moduleColumn.Title = Convert.ToString(dr.Title);
                moduleColumn.CategoryName = dr.CategoryName;
                moduleColumn.FieldName = dr.FieldName;
                moduleColumn.FieldDisplayName = dr.FieldDisplayName;
                moduleColumn.IsDisplay = dr.IsDisplay;
                moduleColumn.FieldSequence = dr.FieldSequence;
                moduleColumn.IsUseInWildCard = dr.IsUseInWildCard;
                moduleColumn.ShowInMobile = dr.ShowInMobile;
                moduleColumn.CustomProperties = dr.CustomProperties;
                moduleColumn.DisplayForClosed = dr.DisplayForClosed;
                moduleColumn.DisplayForReport = dr.DisplayForReport;
                moduleColumn.ColumnType = dr.ColumnType;
                moduleColumn.SortOrder = dr.SortOrder;
                moduleColumn.IsAscending = dr.IsAscending;
                //moduleColumn.AllowInlineEdit = dr.AllowInlineEdit;
                moduleColumn.ShowInCardView = dr.ShowInCardView;

                moduleColumnManager.Update(moduleColumn);
            }

            if (isEdit)
            {
                drModuleColumn = moduleColumnManager.Load(x => x.CategoryName == categoryName).OrderBy(x => x.FieldSequence).ToList();
                int startSequence = 1;

                foreach (ModuleColumn dr in drModuleColumn)
                {
                    moduleColumn.ID = Convert.ToInt64(dr.ID);
                    moduleColumn.Title = Convert.ToString(dr.Title);
                    moduleColumn.CategoryName = dr.CategoryName;
                    moduleColumn.FieldName = dr.FieldName;
                    moduleColumn.FieldDisplayName = dr.FieldDisplayName;
                    moduleColumn.IsDisplay = dr.IsDisplay;
                    moduleColumn.FieldSequence = startSequence++;
                    moduleColumn.IsUseInWildCard = dr.IsUseInWildCard;
                    moduleColumn.ShowInMobile = dr.ShowInMobile;
                    moduleColumn.CustomProperties = dr.CustomProperties;
                    moduleColumn.DisplayForClosed = dr.DisplayForClosed;
                    moduleColumn.DisplayForReport = dr.DisplayForReport;
                    moduleColumn.ColumnType = dr.ColumnType;
                    moduleColumn.SortOrder = dr.SortOrder;
                    moduleColumn.IsAscending = dr.IsAscending;
                    //moduleColumn.AllowInlineEdit = dr.AllowInlineEdit;
                    moduleColumn.ShowInCardView = dr.ShowInCardView;
                    moduleColumnManager.Update(moduleColumn);
                }
            }

        }

        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            string category = string.Empty;
            int fieldSequence = 0;
            if (moduleColumn != null)
            {
                category = moduleColumn.CategoryName;
                fieldSequence = moduleColumn.FieldSequence;
                Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Delete request list item : {moduleColumn.FieldName} {category}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
                moduleColumnManager.Delete(moduleColumn);
                UpdateSequenceAfterDelete(category, fieldSequence);

                UGITModule module = ObjModuleManager.GetByName(ddlModule.SelectedValue);
                if (module != null)
                {
                    module.List_ModuleColumns = moduleColumnManager.Load().Where(x => x.CategoryName == ddlModule.SelectedValue).OrderBy(x => x.FieldSequence).ToList();
                    CacheHelper<UGITModule>.AddOrUpdate(module.ModuleName, context.TenantID, module);
                }

            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        private void UpdateSequenceAfterDelete(string category, int fieldSequence)
        {
            List<ModuleColumn> drModuleColumn = moduleColumnManager.Load(x => x.CategoryName == category && x.FieldSequence > fieldSequence).OrderBy(x => x.FieldSequence).ToList();
            int Sequence = fieldSequence;
            foreach (ModuleColumn dr in drModuleColumn)
            {
                dr.FieldSequence = Sequence++;
                moduleColumnManager.Update(dr);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        void BindSeqSortDropDownList()
        {
            List<ModuleColumn> lstModuleColumns = moduleColumnManager.Load().Where(x => x.CategoryName == Module).ToList();

            //bind sortorder show only 5 entry and one Blank entry.
            for (int i = 1; i < 6; i++)
            {
                ddlSortOrder.Items.Add(Convert.ToString(i));
            }
            ddlSortOrder.Items.Insert(0, "");

            //bind Seqnumber and one blank entry.
            for (int i = 1; i <= lstModuleColumns.Count + 1; i++)
            {
                ddlFieldSequence.Items.Add(Convert.ToString(i));
            }
            ddlFieldSequence.Items.Insert(0, "");

            // bind the sortdirection
            ddlSortDirection.Items.Add("Ascending");
            ddlSortDirection.Items.Add("Descending");
        }

        protected void cmbFieldName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindColumnType();
        }
    }
}
