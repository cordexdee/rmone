using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class ProjectMonitors : System.Web.UI.UserControl
    {
        public int PMMID { get; set; }
        public double BaselineId { get; set; }
        protected DataRow pmmItem = null;
        public bool IsReadOnly { get; set; }
        public bool IsShowBaseline{get;set;}

        protected string currentModulePagePath;

        Ticket ticketRequest = null;
        LifeCycle projectLifeCycle = null;
        //LifeCycleStage currentLifeCycleStage = null;
        //HtmlEditorControl htmlEditor = null;

        DataTable projectMonitorState;
        DataTable projectMonitorOptions;

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ProjectMonitorStateManager MonitorStateManager = new ProjectMonitorStateManager(HttpContext.Current.GetManagerContext());
        ModuleMonitorOptionManager MonitorOptionManager = new ModuleMonitorOptionManager(HttpContext.Current.GetManagerContext());

        UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        UserProfile User;
        UGITModule module;
        public string moduleName;
       
       // public string moduleName= "PMM";

        public string ticketId;

        protected override void OnInit(EventArgs e)
        {
            if (string.IsNullOrEmpty(moduleName))
                moduleName = "PMM";//Added monitor in the tab.
            User = HttpContext.Current.CurrentUser();

            module = ModuleManager.GetByName(moduleName);
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //string express = string.Format("{0}='{1}'", DatabaseObjects.Columns.TenantID,context.TenantID);
            projectMonitorState = MonitorStateManager.GetDataTable(); 

            projectMonitorOptions = MonitorOptionManager.GetDataTable(); 

            ticketRequest = new Ticket(HttpContext.Current.GetManagerContext(), moduleName);
            UGITModule module = ModuleManager.GetByName(moduleName);
            TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            if (!string.IsNullOrEmpty(ticketId))
            {
                pmmItem = ticketManager.GetByTicketID(module, ticketId);
                projectLifeCycle = ticketRequest.GetTicketLifeCycle(pmmItem);
                ticketId = UGITUtility.ObjectToString(pmmItem[DatabaseObjects.Columns.TicketId]);
                PMMID = UGITUtility.StringToInt(pmmItem[DatabaseObjects.Columns.ID]);
                LifeCycleStage closeStage = ticketRequest.GetTicketCloseStage(pmmItem);
            }

            // Bind Monitors
            projectMonitorState = MonitorStateManager.GetDataTable();
            projectMonitorOptions = MonitorOptionManager.GetDataTable();

            if (IsReadOnly)
            {
                monitorsContainerReadOnly.Visible = true;
                monitorsContainer.Visible = false;
                BindMonitors();
            }
            else
            {
                monitorsContainerReadOnly.Visible = false; 
                monitorsContainer.Visible = true;

                //Project Monitors
                UGITModule pmmModule = ModuleManager.GetByName(moduleName);
                if (pmmModule != null)
                    currentModulePagePath = UGITUtility.GetAbsoluteURL(Convert.ToString(pmmModule.ModuleRelativePagePath));

                projectMonitorState = MonitorStateManager.GetDataTable();
                projectMonitorOptions = MonitorOptionManager.GetDataTable();

                BindMonitors();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {

            
        }
        #region  Project Monitors
        /// <summary>
        /// Bind Project Monitors
        /// </summary>
        private void BindMonitors()
        {
          

            if (IsShowBaseline)
            {
                 projectMonitorState = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectMonitorStateHistory, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.BaselineId}={BaselineId}");
                scoreLabel.Visible = true;
               
            }
         
            
                var JoinedTable = (from p in projectMonitorState.AsEnumerable()
                                   join t in projectMonitorOptions.AsEnumerable()
                                   on Convert.ToString(p.Field<object>("ModuleMonitorOptionIdLookup")) equals Convert.ToString(t.Field<object>("ID"))

                                   select new
                                   {
                                       ID = Convert.ToInt64(p.Field<object>("ID")),
                                       OptionID = Convert.ToInt64(t.Field<object>("ID")),
                                       ModuleMonitorMultiplier = Convert.ToInt32(t.Field<object>("ModuleMonitorMultiplier")),
                                       ModuleMonitorName = GetMonitorName(Convert.ToInt64(p.Field<object>("ModuleMonitorNameLookup"))),
                                       ModuleMonitorOptionName = Convert.ToString(t.Field<object>("ModuleMonitorOptionName")),
                                       ModuleMonitorOptionLEDClass = Convert.ToString(t.Field<object>("ModuleMonitorOptionLEDClass")),
                                       Title = Convert.ToString(p.Field<object>("Title")),
                                       IsDefault = Convert.ToBoolean(t.Field<object>("IsDefault")),
                                       AutoCalculate = Convert.ToBoolean(p.Field<object>("AutoCalculate")),
                                       PMMIdLookup = UGITUtility.StringToLong(p.Field<object>("PMMIdLookup")),
                                       ModuleMonitorOptionIdLookup = Convert.ToInt64(p.Field<object>("ModuleMonitorOptionIdLookup")),
                                       ProjectMonitorNotes = Convert.ToString(p.Field<object>("ProjectMonitorNotes")),
                                       ProjectMonitorWeight = Convert.ToInt32(p.Field<object>("ProjectMonitorWeight")),
                                       TicketId = Convert.ToString(p.Field<object>("TicketId")),
                                       ModuleName = Convert.ToString(p.Field<object>("ModuleNameLookup")),
                                       Deleted = Convert.ToBoolean(p.Field<object>("Deleted"))
                                   }).ToList();

                JoinedTable = JoinedTable.Where(x => x.PMMIdLookup == PMMID).ToList();


                monitorsRepeater.DataSource = JoinedTable;
                monitorsRepeater.DataBind();

            if (!IsShowBaseline)
            {
                monitorDetailsRepeater.DataSource = JoinedTable;
                monitorDetailsRepeater.DataBind();

            }
            if (IsReadOnly)
            {
                rReadOnlyMonitors.DataSource = JoinedTable;
                rReadOnlyMonitors.DataBind();
            }

        }

        /// <summary>
        /// Fill Monitor options
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataView FillMonitorOptions(int id)
        {
            ProjectMonitorState monitorStateItem = MonitorStateManager.Load(x=>x.ModuleMonitorOptionIdLookup == id).FirstOrDefault(); 
            DataRow[] monitorOptions = projectMonitorOptions.Select(string.Format("{0}='{1}'",DatabaseObjects.Columns.ModuleMonitorNameLookup,monitorStateItem.ModuleMonitorNameLookup));
            DataView myView = new DataView(monitorOptions.CopyToDataTable());
            return myView;
        }

        protected string GetMonitorName(long monitorid)
        {
            string monitorname = "";
            ModuleMonitorManager moduleMonitorManager = new ModuleMonitorManager(HttpContext.Current.GetManagerContext());
            ModuleMonitor monitorOption = moduleMonitorManager.LoadByID(monitorid);
            if (monitorOption != null)
                monitorname = monitorOption.MonitorName;

            return monitorname;    

        }

        protected void FillSelectedChoice(object sender, EventArgs e)
        {
            DropDownList monitor = ((DropDownList)sender);
            monitor.SelectedIndex = monitor.Items.IndexOf(monitor.Items.FindByText(monitor.Attributes["selectedmonitorvalue"]));
        }

        protected string GetMonitorClass(string selectedMonitorValue)
        {
            return UGITUtility.SplitString(selectedMonitorValue, Constants.Separator, 1);
        }

        protected string GetMonitorScore(string selectedMonitorOptionId, string monitorOptionWeight)
        {
            float monitorOptionWeightFloat = 0;
            float.TryParse(monitorOptionWeight, out monitorOptionWeightFloat);

            int monitorOptionId = 0;
            int.TryParse(selectedMonitorOptionId, out monitorOptionId);

            ModuleMonitorOption monitorOptionItem = MonitorOptionManager.LoadByID(monitorOptionId);
            if (monitorOptionItem != null)
            {
                float multiplier = 0;
                float.TryParse(Convert.ToString(monitorOptionItem.ModuleMonitorMultiplier), out multiplier);
                return "Score : " + ((multiplier / 100) * monitorOptionWeightFloat).ToString();
            }
            else
                return "Score : 0";
        }

        /// <summary>
        /// Save Project Monitors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void monitorSave_Click(object sender, EventArgs e)
        {
            TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());

            float overallScore = 0;
            float totalWeight = 0;
            foreach (RepeaterItem monitorDD in monitorDetailsRepeater.Items)
            {
                DropDownList monitorOptionSelected = ((DropDownList)monitorDD.FindControl("monitorValue"));
                ProjectMonitorState monitor = MonitorStateManager.LoadByID(int.Parse(monitorOptionSelected.Attributes["MonitorId"]));  // SPListHelper.GetSPListItem(projectMonitors, int.Parse(monitorOptionSelected.Attributes["MonitorId"]));
                bool autoCalculate = ((CheckBox)monitorDD.FindControl("autocalculate")).Checked;
                if (autoCalculate)
                {
                    monitor.AutoCalculate = autoCalculate;
                    monitor.ProjectMonitorNotes = ((TextBox)monitorDD.FindControl("monitorNotes")).Text;
                    monitor.ProjectMonitorWeight = Convert.ToInt32( ((TextBox)monitorDD.FindControl("monitorWeight")).Text );
                    MonitorStateManager.Update(monitor);  
                }
                else
                {
                    //monitor.ModuleMonitorOptionNameLookup = Convert.ToInt64( monitorOptionSelected.SelectedValue);
                    monitor.ProjectMonitorWeight = Convert.ToInt32( ((TextBox)monitorDD.FindControl("monitorWeight")).Text);
                    monitor.ProjectMonitorNotes = ((TextBox)monitorDD.FindControl("monitorNotes")).Text;
                    //monitor.ModuleMonitorOptionLEDClassLookup = Convert.ToInt64(monitorOptionSelected.SelectedValue);
                    monitor.ModuleMonitorOptionIdLookup = Convert.ToInt64( monitorOptionSelected.SelectedValue);
                    monitor.AutoCalculate = false;
                    MonitorStateManager.Update(monitor); 
                }

                int selectedOption = 0;
                int.TryParse(monitorOptionSelected.SelectedValue, out selectedOption);
                ModuleMonitorOption selectedMonitorOption = MonitorOptionManager.LoadByID( selectedOption);
                if (selectedMonitorOption != null)
                {
                    float monitorWeightFloat = 0;
                    float.TryParse(((TextBox)monitorDD.FindControl("monitorWeight")).Text, out monitorWeightFloat);
                    totalWeight += monitorWeightFloat;

                    float multiplier = 0;
                    float.TryParse(Convert.ToString(selectedMonitorOption.ModuleMonitorMultiplier), out multiplier);

                    overallScore += monitorWeightFloat * (multiplier / 100);
                }
            }

            UGITModule module = ModuleManager.GetByName(ModuleNames.PMM);

            pmmItem[DatabaseObjects.Columns.TicketProjectScore] = Math.Round(overallScore * 100 / totalWeight, 0);
            monitorColor.Text = overallScore.ToString();

            Ticket pmmTicket = new Ticket(HttpContext.Current.GetManagerContext(), "PMM");
            pmmTicket.CommitChanges(pmmItem);

        }
        #endregion

        protected void monitorDetailsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            scoreLabel.Visible = true;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                dynamic dr = e.Item.DataItem;
                HtmlGenericControl autoCalculateActive = e.Item.FindControl("autoCalculateActive") as HtmlGenericControl;

                //adding attribute focusout(its called onblur in c#) to Notes Textbox
                TextBox tb = e.Item.FindControl("monitorNotes") as TextBox;
                tb.Attributes.Add("onblur", "UpdateScrore(this);");

                HtmlGenericControl defaultProcess = e.Item.FindControl("defaultProcess") as HtmlGenericControl;
                string monitorName =Convert.ToString(dr.ModuleMonitorName);
                bool autoCalculateEnabled = (monitorName == "On Budget" || monitorName == "On Time");
                if (!autoCalculateEnabled)
                {
                    CheckBox autoCalculateCheckbox = e.Item.FindControl("autoCalculate") as CheckBox;
                    autoCalculateCheckbox.Checked = false;
                    autoCalculateCheckbox.Visible = false;
                }
                else
                {
                    autoCalculateActive.Attributes["class"] = "hidden";
                }
                if (autoCalculateEnabled && !(dr.AutoCalculate is DBNull) &&
                    Convert.ToString(dr.AutoCalculate) == "1")
                {
                    defaultProcess.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
                else
                {
                    autoCalculateActive.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
            }
        }
    }
}