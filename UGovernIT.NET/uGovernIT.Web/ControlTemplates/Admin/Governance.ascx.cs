using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class Governance : System.Web.UI.UserControl
    {
        public string linkDashboardButtons = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=governanceconfig&param=dashboard");
        public string linkLinkViews = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=linkconfig&viewLink=0");
        public string linkADUserSync = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=adusers");
        public string configurance = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=governanceconfig&param=config");
        public string linkSurvey = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=services&ServiceType=SurveyFeedback");
        public string linkSurveyFeedback= UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=surveyfeedback&param=config");

        protected void Page_Load(object sender, EventArgs e)
        {
            ImageSlider.DataSource = DashBoardConfigLink();
            ImageSlider.DataBind();

            ASPxImageSlider1.DataSource = UserSurveyFeedback();
            ASPxImageSlider1.DataBind();

      

        }

        public DataTable DashBoardConfigLink()
        {
            var dt = new DataTable();

            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("HrClass", typeof(string));




            DataRow dr = dt.NewRow();
            dr["Text"] = "Dashboard Buttons";
            dr["Image"] = "/Content/Images/NewAdmin/dashboard-btn.png";
            dt.Rows.Add(dr);



            DataRow dr1 = dt.NewRow();
            dr1["Text"] = "Link Views";
            dr1["Image"] = "/Content/Images/NewAdmin/link-view.png";
            dt.Rows.Add(dr1);



            DataRow dr2 = dt.NewRow();
            dr2["Text"] = "AD User Sync";
            dr2["Image"] = "/Content/Images/NewAdmin/active-directory-user.png";
            dr2["HrClass"] = "lastHr";
            dt.Rows.Add(dr2);

            return dt;
        }

        public DataTable UserSurveyFeedback()
        {
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Text", typeof(string));
            dt1.Columns.Add("Image", typeof(string));
            dt1.Columns.Add("HrClass", typeof(string));


            DataRow dr = dt1.NewRow();
            dr["Text"] = "Configuration";
            dr["Image"] = "/Content/Images/NewAdmin/configuration";



            dt1.Rows.Add(dr);

            DataRow dr2 = dt1.NewRow();
            dr2["Text"] = "Survey";
            dr2["Image"] = "/Content/Images/NewAdmin/survey-feedback.png";

            dt1.Rows.Add(dr2);

            DataRow dr3 = dt1.NewRow();
            dr3["Text"] = "Survey feedback";
            dr3["Image"] = "/Content/Images/NewAdmin/survey-feedback.png";
            dr3["HrClass"] = "lastHr";

            dt1.Rows.Add(dr3);

            return dt1;
        }

    }
}