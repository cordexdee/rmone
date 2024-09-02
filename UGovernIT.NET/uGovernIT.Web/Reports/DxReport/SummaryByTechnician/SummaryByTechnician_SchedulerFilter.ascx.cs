using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.DxReport
{
    public partial class SummaryByTechnician_SchedulerFilter : ReportScheduleFilterBase
    {
        public string PopID { get; set; }
        public string Type { get; set; }
        public Dictionary<string, object> ReportScheduleDict { get; set; }
        protected string ModuleName { get; set; }
        UserProfileManager userProfileManager;
        ModuleViewManager moduleViewManager;
        public string delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx");
        ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
        public long Id { get; set; }
        public SummaryByTechnician_SchedulerFilter()
        {
            PopID = "aspxPopupTicketSummaryByPRP";
            Type = "rrunreport";
            ModuleName = "TSR";
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request["ID"] != null)
            {
                Id = UGITUtility.StringToLong(Request["ID"]);
                scheduleID = Id;
            }
            moduleViewManager = new ModuleViewManager(applicationContext);
            userProfileManager = new UserProfileManager(applicationContext);
            dtcSummaryStartDate.Date = DateTime.Now.AddDays(-7);
            dtcSummaryEndDate.Date = DateTime.Now;
            txtValueFrom.Text = "-7";
            txtValueTo.Text = "0";

            FillModules();
            FillIncludeCounts();
            FillForm();
        }

        protected void glITManagers_Init(object sender, EventArgs e)
        {
            FillITManagersDDl();

        }

        private void FillModules()
        {
            //  ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            DataTable dtModules = moduleViewManager.GetDataTable();
            if (dtModules == null || dtModules.Rows.Count == 0)
                return;

            var modules = dtModules.AsEnumerable()
                                   .Where(x => !x.IsNull(DatabaseObjects.Columns.ShowTicketSummary)
                                             && x.Field<string>(DatabaseObjects.Columns.ShowTicketSummary).Equals("False")
                                             && !x.IsNull(DatabaseObjects.Columns.EnableModule)
                                             && x.Field<string>(DatabaseObjects.Columns.EnableModule).Equals("True"));
            if (modules != null && modules.Count() > 0)
            {
                cblSmryByTechModule.DataSource = modules.CopyToDataTable();
                cblSmryByTechModule.DataTextField = DatabaseObjects.Columns.Title;
                cblSmryByTechModule.DataValueField = DatabaseObjects.Columns.ModuleName;
                cblSmryByTechModule.DataBind();
            }
        }

        private void FillITManagersDDl()
        {
            userProfileManager = new UserProfileManager(applicationContext);
            //  UserProfileManager userProfileManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
            List<UserProfile> lstUserInfo = userProfileManager.FillITManagers();
            glITManagers.DataSource = lstUserInfo;
            glITManagers.DataBind();
            glITManagers.KeyFieldName = DatabaseObjects.Columns.Id;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnBuild.Visible = lnkSubmit.Visible = false;

            if (Type.ToLower() == "runreport")
                btnBuild.Visible = true;
            else
            {
                spanNoOfDays.Visible = true;
                lnkSubmit.Visible = true;
                spanDateRange.Visible = false;
            }

            if (!IsPostBack)
            {
                List<UserProfile> lstUserInfo = userProfileManager.FillITManagers(); //UserProfile.FillITManagers();
                glITManagers.GridView.Selection.SelectRowByKey(lstUserInfo.Select(x => x.Id).FirstOrDefault(y => y == HttpContext.Current.GetManagerContext().CurrentUser.Id));

                //if (ReportScheduleDict != null && ReportScheduleDict.Count != 0 && ReportScheduleDict.ContainsKey(ReportScheduleConstant.Report) && Convert.ToString(ReportScheduleDict[ReportScheduleConstant.Report]) == "SummaryByTechnician")
                //    FillForm();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {

        }

        private void FillIncludeCounts()
        {
            UGITModule module = moduleViewManager.LoadByName(ModuleName);
            //  UGITModule module = uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, ModuleName);
            LifeCycle lifeCycle = module.List_LifeCycles.FirstOrDefault();
            if (lifeCycle != null)
            {
                List<LifeCycleStage> stages = lifeCycle.Stages.Where(x => x.StageTypeChoice == StageType.Assigned.ToString() || x.StageTypeChoice == StageType.Closed.ToString()).ToList();
                cblIncludeColumns.DataSource = stages;
                cblIncludeColumns.TextField = "Name";
                cblIncludeColumns.ValueField = "Name";
                cblIncludeColumns.DataBind();
                cblIncludeColumns.Items.Add(new ListEditItem("OnHold", "OnHold"));
            }
            cblIncludeColumns.SelectAll();
        }

        private void FillForm()
        {
            if (Id > 0)
            {
                Dictionary<string, object> formdic = LoadFilter(Id);
                if (formdic == null || formdic.Count == 0)
                {
                    SummaryByTechnician_Scheduler scheduler = new SummaryByTechnician_Scheduler();
                    formdic = scheduler.GetDefaultData();
                }
                string selectedModule = Convert.ToString(formdic[ReportScheduleConstant.Module]);
                if (!string.IsNullOrEmpty(selectedModule))
                {
                    foreach (string module in selectedModule.Split(','))
                    {
                        if (cblSmryByTechModule.Items.FindByValue(module) != null)
                            cblSmryByTechModule.Items.FindByValue(module).Selected = true;
                    }

                }
                chkGroupByOption.Checked = UGITUtility.StringToBoolean(formdic[ReportScheduleConstant.GroupByCategory]);
                chkbxIncludeTechnician.Checked = UGITUtility.StringToBoolean(formdic[ReportScheduleConstant.IncludeORP]);
                chkSmryByTechSortByModule.Checked = UGITUtility.StringToBoolean(formdic[ReportScheduleConstant.SortByModule]);

                if (formdic.ContainsKey(ReportScheduleConstant.DateRange))
                {
                    string dateRange = Convert.ToString(formdic[ReportScheduleConstant.DateRange]);
                    string[] arrDateRange = dateRange.Split(new string[] { Constants.Separator }, StringSplitOptions.None);
                    if (arrDateRange.Length > 1)
                    {
                        ddlDateUnitsFrom.SelectedIndex = ddlDateUnitsFrom.Items.IndexOf(ddlDateUnitsFrom.Items.FindByTextWithTrim(arrDateRange[1]));
                        string[] arrDates = arrDateRange[0].Split(new string[] { Constants.Separator1 }, StringSplitOptions.None);
                        if (arrDates.Length > 1)
                        {
                            txtValueFrom.Text = arrDates[0];
                            txtValueTo.Text = arrDates[1];
                        }
                    }
                }

                string selectedManagers = string.Empty;
                
                if (formdic.ContainsKey(ReportScheduleConstant.ITManagers))
                    selectedManagers = Convert.ToString(formdic[ReportScheduleConstant.ITManagers]);

                if (!string.IsNullOrEmpty(selectedManagers))
                {
                    glITManagers.GridView.Selection.UnselectAll();
                    foreach (string item in selectedManagers.Split(','))
                        glITManagers.GridView.Selection.SetSelectionByKey(UGITUtility.StringToInt(item), true);
                }
                string selectedIncludeCounts = string.Empty;
                if (formdic.ContainsValue(ReportScheduleConstant.IncludeCounts))
                     selectedIncludeCounts = Convert.ToString(formdic[ReportScheduleConstant.IncludeCounts]);

                if (!string.IsNullOrEmpty(selectedIncludeCounts))
                {
                    cblIncludeColumns.UnselectAll();
                    foreach (string item in selectedIncludeCounts.Split(','))
                    {
                        if (cblIncludeColumns.Items.FindByValue(item) != null)
                            cblIncludeColumns.Items.FindByValue(item).Selected = true;
                    }

                }
            }
        }

        protected void cbpnlMain_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            List<string> selecteModules = cblSmryByTechModule.Items.Cast<ListItem>()
                                           .Where(li => li.Selected)
                                           .Select(li => li.Value)
                                           .ToList();
            if (selecteModules.Count == 0)
                return;

            Dictionary<string, object> formdic = new Dictionary<string, object>();
            formdic.Add(ReportScheduleConstant.Report, TypeOfReport.SummaryByTechnician);
            formdic.Add(ReportScheduleConstant.Module, string.Join(",", selecteModules));
            formdic.Add(ReportScheduleConstant.GroupByCategory, chkGroupByOption.Checked);
            formdic.Add(ReportScheduleConstant.IncludeORP, chkbxIncludeTechnician.Checked);
            if (cblIncludeColumns.SelectedItems.Count > 0)
                formdic.Add(ReportScheduleConstant.IncludeCounts, string.Join(",", cblIncludeColumns.SelectedItems.Cast<ListEditItem>().Select(x => x.Text)));

            formdic.Add(ReportScheduleConstant.DateRange, string.Format("{0}{1}{2}{3}{4}", txtValueFrom.Text, Constants.Separator1, txtValueTo.Text, Constants.Separator, ddlDateUnitsFrom.SelectedItem.Text));
            formdic.Add(ReportScheduleConstant.ITManagers, string.Join(",", glITManagers.GridView.GetSelectedFieldValues(DatabaseObjects.Columns.Id)));
            formdic.Add(ReportScheduleConstant.SortByModule, chkSmryByTechSortByModule.Checked);
            Filterproperties = formdic;
            SaveFilters();
            uHelper.ClosePopUpAndEndResponse(Context, false);
            //  uHelper.ReportScheduleDict = formdic;
            //   ScheduleActionsManager = new ScheduleActionsManager(HttpContext.Current.GetManagerContext());
            //  spListitem = ScheduleActionsManager.LoadByID(Id);
            //  if (spListitem != null)
            //{
            //    spListitem.ActionTypeData = UGITUtility.SerializeDicObject(formdic);
            //    ScheduleActionsManager.Update(spListitem);
            //}
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}