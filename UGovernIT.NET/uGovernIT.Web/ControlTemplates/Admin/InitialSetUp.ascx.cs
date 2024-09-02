using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace uGovernIT.Web
{
    public partial class InitialSetUp : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ImageSlider.DataSource = InitialSetup();
            ImageSlider.DataBind();
        }

        public DataTable InitialSetup()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("Link", typeof(string));
            dt.Columns.Add("Desc", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Text"] = "Configuration Variable";
            dr["Image"] = "/Content/Images/configurationVariable.png";
            //dr["Link"] = "url servicePrime-url url1";
            dr["Desc"] = "These control the operation of Core.";
            dt.Rows.Add(dr);

            DataRow dr2 = dt.NewRow();
            dr2["Text"] = "Modules";
            dr2["Image"] = "/Content/Images/modules.png";
            //dr2["Link"] = "url servicePrime-url url2";
            dr2["Desc"] = "Set up modules and configure them for the specific tenant.";
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr3["Text"] = "Form Layout";
            dr3["Image"] = "/Content/Images/formLayout.png";
            //dr3["Link"] = "url servicePrime-url url3";
            dr3["Desc"] = "Design the forms for the tenant from the pre-bulid out of the box forms.";
            dt.Rows.Add(dr3);

            DataRow dr4 = dt.NewRow();
            dr4["Text"] = "Request List";
            dr4["Image"] = "/Content/Images/requestList.png";
            //dr4["Link"] = "url servicePrime-url url4";
            dr4["Desc"] = "Define the specific request list which display summary forms on the landing pages.";
            dt.Rows.Add(dr4);

            DataRow dr5 = dt.NewRow();
            dr5["Text"] = "Request Types";
            dr5["Image"] = "/Content/Images/requesttype.png";
            //dr5["Link"] = "url servicePrime-url url5";
            dr5["Desc"] = "Configure applicable request types for the modules using pre-build request types.";
            dt.Rows.Add(dr5);

            DataRow dr6 = dt.NewRow();
            dr6["Text"] = "Organization";
            dr6["Image"] = "/Content/Images/organization.png";
            //dr6["Link"] = "url servicePrime-url url6";
            dr6["Desc"] = "Set up the tenant Departments.";
            dt.Rows.Add(dr6);

            DataRow dr7 = dt.NewRow();
            dr7["Text"] = "Functional Areas";
            dr7["Image"] = "/Content/Images/functionalArea.png";
            //dr7["Link"] = "url servicePrime-url url7";
            dr7["Desc"] = "Set up the functional areas for the tenant.";
            dt.Rows.Add(dr7);

            DataRow dr8 = dt.NewRow();
            dr8["Text"] = "Locations";
            dr8["Image"] = "/Content/Images/locations.png";
            //dr8["Link"] = "url servicePrime-url url8";
            dr8["Desc"] = "Set up the geographic locations.";
            dt.Rows.Add(dr8);

            DataRow dr9 = dt.NewRow();
            dr9["Text"] = "Module Defaults";
            dr9["Image"] = "/Content/Images/moduleDefault.png";
            //dr9["Link"] = "url servicePrime-url url9";
            dr9["Desc"] = "Type your text hear.";
            dt.Rows.Add(dr9);

            DataRow dr10 = dt.NewRow();
            dr10["Text"] = "Phrases";
            dr10["Image"] = "/Content/Images/phrases.png";
            //dr9["Link"] = "url servicePrime-url url9";
            dr10["Desc"] = "Add Phrases";
            dt.Rows.Add(dr10);


            return dt;



        }

    }
}