using System;
using System.Collections.Generic;
using System.Data;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;

namespace uGovernIT.Web
{
    public partial class UserService : System.Web.UI.UserControl
    {
        public string linkpath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagesview");
        public string delegateUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=service&category=services" + "&serviceid=");
        public string newServiceURL = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration?control=ServicesWizard&serviceID="));

        DataTable categories = new DataTable();
        protected override void OnInit(EventArgs e)
        {
            
            grid.DataSource =    BindCategory();
            grid.DataBind();
            base.OnInit(e);
        }



        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void grid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

            var CategoryId = e.KeyValue;
           ServicesManager serviceManager = new ServicesManager(HttpContext.Current.GetManagerContext());
           var Services = serviceManager.Load(x => x.CategoryId ==Convert.ToInt64( CategoryId) && x.IsActivated== true);
           DataTable dtervices = BindService(Services);
            ASPxImageSlider ASPxImageSliderGrid = e.Row.FindControlRecursive("ASPxImageSliderGrid") as ASPxImageSlider;
            
            ASPxImageSliderGrid.DataSource = dtervices;
            ASPxImageSliderGrid.DataBind();
        }

        protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            //ASPxImageSlider ASPxImageSliderGrid = grid.FindDetailRowTemplateControl(1, "ASPxImageSliderGrid") as ASPxImageSlider;
            ////ASPxImageSlider ASPxImageSliderGrid = grid.FindEditRowCellTemplateControl(grid.Columns["ImageSlider"] as GridViewDataColumn, "ASPxImageSliderGrid") as ASPxImageSlider;
            //ASPxImageSliderGrid.DataSource = Catagories();
            //ASPxImageSliderGrid.DataBind();

        }


        private List<ServiceCategory> BindCategory()
        {

            ServicesManager serviceManager = new ServicesManager(HttpContext.Current.GetManagerContext());
            List<ServiceCategory> servicescategory = serviceManager.ServiceCategories;
            List<ServiceCategory> finalcategory = new List<ServiceCategory>();
            foreach (ServiceCategory sc in servicescategory)
            {
                var lst = serviceManager.Load(x => x.CategoryId == sc.ID && x.IsActivated == true);
                if (serviceManager.Load(x => x.CategoryId == sc.ID) == null || serviceManager.Load(x => x.CategoryId == sc.ID).Count == 0)
                {

                    
                }
                else
                {
                    finalcategory.Add(sc);
                }



            }


            return finalcategory;



        }

        

        public DataTable BindService(List<Services> services)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("Link", typeof(string));
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("HrClass", typeof(string));
            if (services != null)
            {
                foreach (Services sev  in services)
                {

                    DataRow dr = dt.NewRow();
                    dr["ID"] = sev.ID;
                    dr["Text"] = sev.Title;
                    dr["Image"] = "/Content/Images/NewAdmin/applicationChangeRequest.png";
                    dr["HrClass"] = "";
                    if (sev == services.Last())
                    {
                        dr["HrClass"] = "lastHr";
                    }
                    
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
    }
}