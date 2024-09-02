using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
 
using System.IO;
using DevExpress.Spreadsheet;

namespace uGovernIT.Web
{
    public partial class ExportList : UserControl
    {

        protected bool startDownload;
        protected String pageName;
        protected String listName;
        protected string ModuleName;
        protected const String ITG_BUDGET_PAGE = "ITGBudgetManagement";
        protected const String PMM_BUDGET_PAGE = "PMMBudget";
        protected void Page_Load(object sender, EventArgs e)
        {
            pageName = Convert.ToString(Request.QueryString["sourcePage"]);
            if (!Page.IsPostBack)
            {
                operationsDropDown.DataSource = GetAllListName();
                operationsDropDown.DataBind();
            }
            if (Request["startDownload"] != null)
            {
                if (Request["listName"] != null)
                    listName = Convert.ToString(Request["listName"]);

                switch (pageName)
                {
                    case ITG_BUDGET_PAGE:
                        listName = DatabaseObjects.Tables.ModuleMonthlyBudget;
                        ModuleName = ModuleNames.ITG;
                        break;
                    case PMM_BUDGET_PAGE:
                        listName = DatabaseObjects.Tables.ModuleMonthlyBudget;
                        ModuleName = ModuleNames.PMM;
                        break;
                    default:
                        break;
                }
                CreateDownloadData(listName, ModuleName);

            }
            startDownload = false;
        }

        protected List<string> GetAllListName()
        {
            List<string> listNames = new List<string>();
            //try
            //{
            //    SPweb currentWeb = HttpContext.Current.GetManagerContext();

            //    foreach ( list in currentWeb.Lists)
            //    {
            //        listNames.Add(list.Title);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Util.Log.ULog.WriteException(ex, "GetAllListName");
            //}
            return listNames;
        }
        protected void btDownload_Click(object sender, EventArgs e)
        {
            startDownload = true;
        }

        public void CreateDownloadData(String listTitle, string modulename)
        {
            //SPWeb web = SPContext.Current.Web;
            DataTable list = new DataTable();
            DataTable table = new DataTable();
            if (modulename != "ITG")
            {
                list = ExportListToExcel.GetDataTableFromList(listTitle);
                if (list != null)
                {
                    //Guid listGuid = list.ID;
                    //Guid listViewGuid = list.Views[0].ID;
                    //Response.Redirect(string.Format("{0}/_vti_bin/owssvr.dll?CS=109&Using=_layouts/query.iqy&List={1}&View={2}&CacheControl=1", web.Site.Url, listGuid, listViewGuid));
                    table = ExportListToExcel.GetDataTableFromList(list.ToString());
                    ExportListToExcel.ExportToSpreadsheet(table, listTitle);
                }
            }
            else
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@TenantID", HttpContext.Current.GetManagerContext().TenantID);
                values.Add("@ModuleNamelookup", modulename);
                table = GetTableDataManager.GetData("RPTModulebudgetData", values);
                var worksheet = ASPxSpreadsheet1.Document.Worksheets.ActiveWorksheet;
                 
                worksheet.Import(table, true, 0, 0);
                MemoryStream st = new MemoryStream();
                ASPxSpreadsheet1.Document.SaveDocument(st, DocumentFormat.OpenXml);
                Response.Clear();
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment; filename=BudgetData.xlsx");
                Response.BinaryWrite(st.ToArray());
                Response.End();
            }
            

            uHelper.ClosePopUpAndEndResponse(Context, false);

        }

        protected void btClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}