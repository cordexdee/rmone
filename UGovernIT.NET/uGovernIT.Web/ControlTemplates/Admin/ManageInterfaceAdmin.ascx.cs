using System;
using System.Data;
using System.Web.UI;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class ManageInterfaceAdmin : UserControl
    {
        public string linkpath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagesview");
        protected void Page_Load(object sender, EventArgs e)
        {
            ImageSliderAdfs.DataSource = Adfs();
            ImageSliderAdfs.DataBind();

            ImageSliderQuickBook.DataSource = QuickBook();
            ImageSliderQuickBook.DataBind();

            ImageSliderMSDynamic.DataSource = MSDynamic();
            ImageSliderMSDynamic.DataBind();
        }

        public DataTable Adfs()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("Link", typeof(string));
            dt.Columns.Add("HrClass", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Text"] = "Credentials";
            dr["Image"] = "/Content/Images/NewAdmin/credenstial.png";
            dr["Link"] = "";
            dt.Rows.Add(dr);

            DataRow dr2 = dt.NewRow();
            dr2["Text"] = "Configure Utilities";
            dr2["Image"] = "/Content/Images/NewAdmin/configure-utilities.png";
            dr2["Link"] = "";
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr3["Text"] = "Map Interface";
            dr3["Image"] = "/Content/Images/NewAdmin/map-interface.png";
            dr3["Link"] = "";
            dt.Rows.Add(dr3);

            DataRow dr4 = dt.NewRow();
            dr4["Text"] = "Token Info";
            dr4["Image"] = "/Content/Images/NewAdmin/token-info.png";
            dr4["Link"] = "";
            dr4["HrClass"] = "lastHr";
            dt.Rows.Add(dr4);

            return dt;
        }

        public DataTable QuickBook()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("Link", typeof(string));
            dt.Columns.Add("HrClass", typeof(string));


            DataRow dr = dt.NewRow();
            dr["Text"] = "Credentials";
            dr["Image"] = "/Content/Images/NewAdmin/credenstial.png";
            dr["Link"] = "";
            dt.Rows.Add(dr);

            DataRow dr2 = dt.NewRow();
            dr2["Text"] = "Utilities";
            dr2["Image"] = "/Content/Images/NewAdmin/configure-utilities.png";
            dr2["Link"] = "";
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr3["Text"] = "Map Interface(Tokens)";
            dr3["Image"] = "/Content/Images/NewAdmin/map-interface.png";
            dr3["Link"] = "";
            dt.Rows.Add(dr3);

            DataRow dr4 = dt.NewRow();
            dr4["Text"] = "Token Info";
            dr4["Image"] = "/Content/Images/NewAdmin/token-info.png";
            dr4["Link"] = "";
            dr4["HrClass"] = "lastHr";

            dt.Rows.Add(dr4);
            return dt;
        }

        public DataTable MSDynamic()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("Link", typeof(string));
            dt.Columns.Add("HrClass", typeof(string));


            DataRow dr = dt.NewRow();
            dr["Text"] = "Credentials";
            dr["Image"] = "/Content/Images/NewAdmin/credenstial.png";
            dr["Link"] = "";
            dt.Rows.Add(dr);

            DataRow dr2 = dt.NewRow();
            dr2["Text"] = "Configure Interface S/W";
            dr2["Image"] = "/Content/Images/NewAdmin/configure-utilities.png";
            dr2["Link"] = "";
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr3["Text"] = "Map Interface";
            dr3["Image"] = "/Content/Images/NewAdmin/map-interface.png";
            dr3["Link"] = "";
            dr3["HrClass"] = "lastHr";

            dt.Rows.Add(dr3);
            return dt;
        }
    }
}