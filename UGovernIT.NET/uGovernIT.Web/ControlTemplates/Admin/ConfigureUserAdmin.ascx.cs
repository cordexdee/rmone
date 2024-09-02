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
    public partial class ConfigureUserAdmin : System.Web.UI.UserControl
    {
        public string linkpath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagesview");
        public string userskills = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=userskillsview");
        public string userroles = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=userrolesview");
        public string rolesview = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=rolesview");
        public string usermanagement = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=resourcemanagement");
        protected string editGroupUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?control=editspgroup"));

        protected void Page_Load(object sender, EventArgs e)
        {
            ImageSliderManageUser.DataSource = ManageUser();
            ImageSliderManageUser.DataBind();


            ImageSliderListUserGroup.DataSource = ListUserGroup();
            ImageSliderListUserGroup.DataBind();
        }

        public DataTable ManageUser()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("Link", typeof(string));
            dt.Columns.Add("HrClass", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Text"] = "User Skills";
            dr["Image"] = "/Content/Images/NewAdmin/user-skill.png";
            dr["Link"] = "";
            dt.Rows.Add(dr);

            DataRow dr2 = dt.NewRow();
            dr2["Text"] = "Landing Pages";
            dr2["Image"] = "/Content/Images/NewAdmin/user-roles.png";
            dr2["Link"] = "";
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr3["Text"] = "Access(ADFS or Okta)";
            dr3["Image"] = "/Content/Images/NewAdmin/access-adfs.png";
            dr3["Link"] = "";
            dt.Rows.Add(dr3);

            DataRow dr4 = dt.NewRow();
            dr4["Text"] = "User Management";
            dr4["Image"] = "/Content/Images/NewAdmin/manage-user.png";
            dr4["Link"] = "";
            dt.Rows.Add(dr4);

            DataRow dr5 = dt.NewRow();
            dr5["Text"] = "Job Title";
            dr5["Image"] = "/Content/Images/NewAdmin/job-title.png";
            dr5["Link"] = "";
            dr5["HrClass"] = "lastHr";
            dt.Rows.Add(dr5);
            /*
            DataRow dr6 = dt.NewRow();
            dr5["Text"] = "User Roles";
            dr5["Image"] = "/Content/Images/NewAdmin/user-roles.png";
            dr5["Link"] = "";
            //dr5["HrClass"] = "lastHr";
            dt.Rows.Add(dr6);
            */


            return dt;
        }


        public DataTable ListUserGroup()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("Link", typeof(string));
            dt.Columns.Add("HrClass", typeof(string));


            DataRow dr = dt.NewRow();
            dr["Text"] = "Edit";
            dr["Image"] = "/Content/Images/editNewIcon.png";
            dr["Link"] = "";
            dt.Rows.Add(dr);

            DataRow dr2 = dt.NewRow();
            dr2["Text"] = "New";
            dr2["Image"] = "/Content/Images/newTask.png";
            dr2["Link"] = "";
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr3["Text"] = "Advanced RMM";
            dr3["Image"] = "/Content/Images/NewAdmin/advance-rmm.png";
            dr3["Link"] = "";
            dr3["HrClass"] = "lastHr";

            dt.Rows.Add(dr3);
            return dt;
        }
    }
}