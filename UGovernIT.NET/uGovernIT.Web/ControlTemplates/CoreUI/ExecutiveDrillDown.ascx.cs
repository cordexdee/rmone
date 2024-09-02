using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager.Managers;
using System.Data;
using DevExpress.Web;

namespace uGovernIT.Web.ControlTemplates.CoreUI
{
    public partial class ExecutiveDrillDown : System.Web.UI.UserControl
    {
        List<UserProfile> UserProfiles { get; set; }
        public string CPRPath { get; set; }
        public string OPMPath { get; set; }
        public string CNSPath { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();

            ModuleViewManager moduleMgr = new ModuleViewManager(context);
            UGITModule cprModule = moduleMgr.LoadByName("CPR", true);
            UGITModule opmModule = moduleMgr.LoadByName("OPM", true);
            UGITModule cnsModule = moduleMgr.LoadByName("CNS", true);

            CPRPath = cprModule.StaticModulePagePath;
            OPMPath = opmModule.StaticModulePagePath;
            CNSPath = cnsModule.StaticModulePagePath;

            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", context.TenantID);
            values.Add("@UserID", context.CurrentUser.Id);
            DataSet ds = GetTableDataManager.GetDataSet("ExecutiveMoreLinkDrillDown", values);
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    cardPipeline.DataSource = ds.Tables[0];
                    cardPipeline.DataBind();
                }
                else
                {
                    divPipeline.Visible = false;
                }

                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    cardClosed.DataSource = ds.Tables[1];
                    cardClosed.DataBind();
                }
                else
                {
                    divClosed.Visible = false;
                }

                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {
                    cardOnGoing.DataSource = ds.Tables[2];
                    cardOnGoing.DataBind();
                }
                else { divOnGoing.Visible = false; }

                if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                {
                    cardThingToDo.DataSource = ds.Tables[3];
                    cardThingToDo.DataBind();
                }
                else { divThingtodo.Visible = false; }
            }

            UserProfiles = context.UserManager.GetUsersProfile();
        }


        protected void cardPipeline_CardLayoutCreated1(object sender, DevExpress.Web.ASPxCardViewCardLayoutCreatedEventArgs e)
        {
            LayoutItem item = e.FindLayoutItemByColumn("ProjectManagerUser");
            if(UserProfiles != null && UserProfiles.Count > 0)
            {

            }
        }
    }
}