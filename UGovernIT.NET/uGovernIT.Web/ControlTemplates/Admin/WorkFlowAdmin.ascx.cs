using DevExpress.Web;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;


namespace uGovernIT.Web
{
    public partial class WorkFlowAdmin : System.Web.UI.UserControl
    {
        public string linkpath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagesview");
        public string stageexitcriteria = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagetemplates");
        public string environment = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=environmentview&param=config");
        public string eventcategories = "";
        public string messageboard =  UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=messageboard&mode=config");
        public string emailtoworkflow = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=emailtoticket");

        public string emailnotification =  UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=emailnotification");
        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        protected void Page_Load(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("Link", typeof(string));
            dt.Columns.Add("ModuleName", typeof(string));
            dt.Columns.Add("HrClass", typeof(string));
            DataTable moduleList = null;
            DataTable dtModule = ObjModuleViewManager.LoadAllModules();
            dtModule.DefaultView.Sort = DatabaseObjects.Columns.ModuleName + " ASC";
            dtModule = dtModule.DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.ModuleName, DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title,
                                                                             DatabaseObjects.Columns.EnableModule, DatabaseObjects.Columns.EnableWorkflow });
            DataRow[] moduleRows = dtModule.Select(string.Format("{0}='True' AND {1}='True'", DatabaseObjects.Columns.EnableModule, DatabaseObjects.Columns.EnableWorkflow));

            moduleList = (moduleRows != null && moduleRows.Length > 0 ? moduleRows.CopyToDataTable() : null);
            if (moduleList != null)
            {
                int rowCount = moduleList.Rows.Count;
                int i = 0;
                foreach (DataRow row in moduleList.Rows)
                {
                    DataRow dr = dt.NewRow();
                    String module = row["ModuleName"].ToString();
                    dr["Text"] = row["Title"].ToString();
                    switch (module)
                    {
                        case ModuleNames.ACR:
                            dr["Image"] = "/Content/Images/NewAdmin/applicationChangeRequest.png";
                            break;
                        case ModuleNames.APP:
                            dr["Image"] = "/Content/Images/NewAdmin/application-mang.png";
                            break;
                        case ModuleNames.BTS:
                            dr["Image"] = "/Content/Images/NewAdmin/bug-tracking-system.png";
                            break;
                        case ModuleNames.CMDB:
                            dr["Image"] = "/Content/Images/NewAdmin/asset-management.png";
                            break;
                        case ModuleNames.CMT:
                            dr["Image"] = "/Content/Images/NewAdmin/contract-management.png";
                            break;
                        case ModuleNames.NPR:
                            dr["Image"] = "/Content/Images/NewAdmin/New-project-Request.png";
                            break;
                        case ModuleNames.PMM:
                            dr["Image"] = "/Content/Images/NewAdmin/current_project.png";
                            break;
                        case ModuleNames.TSK:
                            dr["Image"] = "/Content/Images/NewAdmin/task-list.png";
                            break;
                        case ModuleNames.RCA:
                            dr["Image"] = "/Content/Images/NewAdmin/root-cause-analysis.png";
                            break;

                        case ModuleNames.TSR:
                            dr["Image"] = "/Content/Images/NewAdmin/technical-services-request.png";
                            //dr["HrClass"] = "lastHr";
                            break;

                        case ModuleNames.INC:
                            dr["Image"] = "/Content/Images/NewAdmin/outage-incidents.png";
                            break;

                        case ModuleNames.PRS:
                            dr["Image"] = "/Content/Images/NewAdmin/problemResolution.png";
                            break;

                        case ModuleNames.SVC:
                            dr["Image"] = "/Content/Images/NewAdmin/services.png";
                            break;

                        case ModuleNames.DRQ:
                            dr["Image"] = "/Content/Images/NewAdmin/changeManagementsystem.png";
                            break;

                        case ModuleNames.WIKI:
                            dr["Image"] = "/Content/Images/NewAdmin/quickbook.png";
                            //dr["HrClass"] = "lastHr";
                            break;

                        default:
                            dr["Image"] = "/Content/Images/leads.png";

                            break;

                    }

                    i++;
                    if (i == rowCount)
                        dr["HrClass"] = "lastHr";

                    // dr["Image"] = "/Content/Images/leads.png";
                    // dr["Link"] = $"javascript: UgitOpenPopupDialog({ absPath },'','WorkFLow','90','90','','')";
                    dr["Link"] = "";
                    dr["ModuleName"] = row["ModuleName"].ToString();
                    dt.Rows.Add(dr);
                }
            }
            ImageSlider.DataSource = dt;
            ImageSlider.DataBind();

            ImageSliderWorkflowSupport.DataSource = BindWorkfLowSupport();
            ImageSliderWorkflowSupport.DataBind();


            ImageSliderCollaboration.DataSource = BindCollaboration();
            ImageSliderCollaboration.DataBind();
        }



        public DataTable BindWorkfLowSupport()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("Link", typeof(string));
            dt.Columns.Add("HrClass", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Text"] = "Stage Exit Criteria";
            dr["Image"] = "/Content/Images/exit-criteria.png";
            dr["Link"] = "";
            dt.Rows.Add(dr);

            DataRow dr2 = dt.NewRow();
            dr2["Text"] = "Environment";
            dr2["Image"] = "/Content/Images/enviornment.png";
            dr2["Link"] = "";
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr3["Text"] = "Event Categories";
            dr3["Image"] = "/Content/Images/event.png";
            dr3["HrClass"] = "lastHr";
            dt.Rows.Add(dr3);
            return dt;
        }


        public DataTable BindCollaboration()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("Link", typeof(string));
            dt.Columns.Add("HrClass", typeof(string));
            dt.Columns.Add("moduleNameClass", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Text"] = "Email to Workflow";
            dr["Image"] = "/Content/Images/email-workflow.png";
            dr["Link"] = "";
            dr["moduleNameClass"] = "module-name";
            dt.Rows.Add(dr);

            DataRow dr2 = dt.NewRow();
            dr2["Text"] = "Email Notifications";
            dr2["Image"] = "/Content/Images/email-notification.png";
            dr2["Link"] = "";
            dr2["moduleNameClass"] = "module-name";
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr3["Text"] = "Message Board";
            dr3["Image"] = "/Content/Images/message-board.png";
            dr3["HrClass"] = "lastHr";
            dr3["moduleNameClass"] = "lastModuleName module-name";
            dt.Rows.Add(dr3);
            return dt;
        }
    }
}