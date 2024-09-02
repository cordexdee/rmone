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

namespace uGovernIT.ControlTemplates.uGovernIT
{
    public partial class SummaryByTechnician_Filter : UserControl
    {
        public string PopID { get; set; }
        public string Type { get; set; }
        public string ModuleName { get; set; }
        public string delegateControl;

        public Dictionary<string, object> ReportScheduleDict { get; set; }

        UserProfileManager userProfileManager;
        ModuleViewManager moduleViewManager;

        ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();

        public SummaryByTechnician_Filter()
        {
            delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
            ModuleName = "TSR";
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            moduleViewManager = new ModuleViewManager(applicationContext);
            userProfileManager = new UserProfileManager(applicationContext);
            dtcSummaryStartDate.Date = DateTime.Now.AddDays(-7);
            dtcSummaryEndDate.Date = DateTime.Now;
            txtValueFrom.Text = "-7";
            txtValueTo.Text = "0";

            if (!IsPostBack)
            {
                ModuleName = Request["Module"];
                FillModules();
            }
            // FillModules();
            FillIncludeCounts();
        }

        protected void glITManagers_Init(object sender, EventArgs e)
        {
            FillITManagersDDl();

        }

        private void FillModules()
        {

            string prms = string.Format("{0}='{1}' AND {2}='{3}'", DatabaseObjects.Columns.ShowTicketSummary, 1, DatabaseObjects.Columns.EnableModule, 1);
            DataTable dtModules = moduleViewManager.GetDataTable(prms);
            if (dtModules != null && dtModules.Rows.Count > 0)
            {
                cblSmryByTechModule.DataSource = dtModules;
                cblSmryByTechModule.DataTextField = DatabaseObjects.Columns.Title;
                cblSmryByTechModule.DataValueField = DatabaseObjects.Columns.ModuleName;
                cblSmryByTechModule.DataBind();
            }
            //if (dtModules == null || dtModules.Rows.Count == 0)
            //    return;

            //var modules = dtModules.AsEnumerable()
            //                       .Where(x => !x.IsNull(DatabaseObjects.Columns.ShowTicketSummary)
            //                                 && x.Field<bool>(DatabaseObjects.Columns.ShowTicketSummary).Equals(true)
            //                                 && !x.IsNull(DatabaseObjects.Columns.EnableModule)
            //                                 && x.Field<bool>(DatabaseObjects.Columns.EnableModule).Equals(true));
            //if (modules != null && modules.Count() > 0)
            //{
            //    cblSmryByTechModule.DataSource = modules.CopyToDataTable();
            //    cblSmryByTechModule.DataTextField = DatabaseObjects.Columns.Title;
            //    cblSmryByTechModule.DataValueField = DatabaseObjects.Columns.ModuleName;
            //    cblSmryByTechModule.DataBind();
            //}
        }

        private void FillITManagersDDl()
        {
            userProfileManager = new UserProfileManager(applicationContext);
            List<UserProfile> lstUserInfo = userProfileManager.FillITManagers();
            glITManagers.DataSource = lstUserInfo;
            glITManagers.DataBind();
            glITManagers.KeyFieldName = DatabaseObjects.Columns.Id;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //LiBuildReport.Visible = true;
            spanNoOfDays.Visible = false;
            spanDateRange.Visible = true;

        }

        protected override void OnPreRender(EventArgs e)
        {

        }

        private void FillIncludeCounts()
        {
            UGITModule module = moduleViewManager.LoadByName(ModuleName, false); // useCache is set false
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
    }
}
