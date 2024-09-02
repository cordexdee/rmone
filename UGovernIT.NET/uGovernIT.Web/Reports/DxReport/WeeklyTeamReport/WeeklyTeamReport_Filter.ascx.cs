
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Serialization;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
using System.Linq;

namespace uGovernIT.DxReport
{
    public partial class WeeklyTeamReport_Filter : ReportScheduleFilterBase
    {
        public string PopID { get; set; }
        public string Type { get; set; }
        public long Id { get; set; }
        ModuleViewManager moduleViewManager;
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        public Dictionary<string,object> ReportScheduleDict { get; set; }
        public string delegateControl;
        public string reportUrl = string.Empty;
        public string ModuleName { get; set; }
        public WeeklyTeamReport_Filter()
        {
            ModuleName = "TSR";
            delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request["ID"] != null)
            {
                Id = UGITUtility.StringToLong(Request["ID"]);
                scheduleID = Id;
            }
            dtFromtDate.Date = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            dtToDate.Date = DateTime.Now;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        protected void glCategories_Init(object sender, EventArgs e)
        {
            FillCategories();
            glCategories.DataBind();
        }
       
        private void FillCategories()
        {
            moduleViewManager = new ModuleViewManager(_context);
            UGITModule module = moduleViewManager.LoadByName(ModuleName);
            if (module == null)
                return;
            List<ModuleRequestType> requestType = module.List_RequestTypes;
            if (requestType != null && requestType.Count > 0)
            {
                string[] requestTypeCategories = requestType.Where(x => !x.Deleted).Select(x => x.Category).OrderBy(x => x).Distinct().ToArray();
                if (requestTypeCategories.Length > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add(DatabaseObjects.Columns.Category);
                    foreach (string item in requestTypeCategories)
                    {
                        DataRow dr = dt.NewRow();
                        dr[DatabaseObjects.Columns.Category] = item;
                        dt.Rows.Add(dr);
                    }
                    glCategories.DataSource = dt;
                }
            }

          


        }
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}
