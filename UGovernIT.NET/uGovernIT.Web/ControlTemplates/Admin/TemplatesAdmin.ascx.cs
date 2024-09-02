using System;
using System.Data;
using System.Web.UI;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class TemplatesAdmin :UserControl
    {
        public string tasktemplateedit = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=UGITTaskTemplates");

        protected void Page_Load(object sender, EventArgs e)
        {
            ImageSlider.DataSource = Checklists();
            ImageSlider.DataBind();

            ASPxImageSlider1.DataSource = Resources();
            ASPxImageSlider1.DataBind();

            ASPxImageSlider2.DataSource = Tasks();
            ASPxImageSlider2.DataBind();
        }

        public DataTable Checklists()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("Link", typeof(string));
            dt.Columns.Add("HrClass", typeof(string));


            DataRow dr = dt.NewRow();
            dr["Text"] = "Add";
            dr["Image"] = "/Content/Images/plus-blue.png";
            dr["Link"] = "";
            dt.Rows.Add(dr);



            DataRow dr2 = dt.NewRow();
            dr2["Text"] = "Edit";
            dr2["Image"] = "/Content/Images/editNewIcon.png";
            dr2["Link"] = "";
            dt.Rows.Add(dr2);



            DataRow dr3 = dt.NewRow();
            dr3["Text"] = "Links";
            dr3["Image"] = "/Content/Images/linkBlue.png";
            dr3["Link"] = "";
            dr3["HrClass"] = "lastHr";

            dt.Rows.Add(dr3);
            return dt;
        }

        public DataTable Resources()
        {
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Text", typeof(string));
            dt1.Columns.Add("Image", typeof(string));
            dt1.Columns.Add("Link", typeof(string));
            dt1.Columns.Add("HrClass",typeof(string));



            DataRow dr = dt1.NewRow();
            dr["Text"] = "Add";
            dr["Image"] = "/Content/Images/plus-blue.png";
            dr["Link"] = "";
            dt1.Rows.Add(dr);



            DataRow dr2 = dt1.NewRow();
            dr2["Text"] = "Edit";
            dr2["Image"] = "/Content/Images/editNewIcon.png";
            dr2["Link"] = "";
            dt1.Rows.Add(dr2);



            DataRow dr3 = dt1.NewRow();
            dr3["Text"] = "Links";
            dr3["Image"] = "/Content/Images/linkBlue.png";
            dr3["Link"] = "";
            dr3["HrClass"] = "lastHr";

            dt1.Rows.Add(dr3);
            return dt1;
        }

        public DataTable Tasks()
        {
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Text", typeof(string));
            dt2.Columns.Add("Image", typeof(string));
            dt2.Columns.Add("Link", typeof(string));
            dt2.Columns.Add("HrClass", typeof(string));



            DataRow dr = dt2.NewRow();
            dr["Text"] = "Add";
            dr["Image"] = "/Content/Images/plus-blue.png";
            dr["Link"] = "";
            dt2.Rows.Add(dr);



            DataRow dr2 = dt2.NewRow();
            dr2["Text"] = "Edit";
            dr2["Image"] = "/Content/Images/editNewIcon.png";
            dr2["Link"] = "";
            dt2.Rows.Add(dr2);



            DataRow dr3 = dt2.NewRow();
            dr3["Text"] = "Links";
            dr3["Image"] = "/Content/Images/linkBlue.png";
            dr3["Link"] = "";
            dr3["HrClass"] = "lastHr";
            dt2.Rows.Add(dr3);
            return dt2;
        }
    }
}
