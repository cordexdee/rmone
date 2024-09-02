using System;
using System.Data;
using System.Web.UI;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class DashboardAndReports : UserControl
    {
          public string linkFactTable = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configdashboardfacttable");
          public string linkDashboardAndQueries = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configdashboard");

        protected void Page_Load(object sender, EventArgs e)
        {
            ImageSlider.DataSource = SelectFunction();
            ImageSlider.DataBind();

        }
        public DataTable SelectFunction()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("Link", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Text"] = "Fact Table";
            dr["Image"] = "/Content/Images/userNew.png";
            dr["Link"] = "";
            dt.Rows.Add(dr);

            DataRow dr1 = dt.NewRow();
            dr1["Text"] = "Dashboard and Queries";
            dr1["Image"] = "/Content/Images/userNew.png";
            dr1["Link"] = "";
            dt.Rows.Add(dr1);

            return dt;
        }
       
    }

}

    