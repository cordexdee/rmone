
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using System.Web;
using DevExpress.Web;
using uGovernIT.Utility;
using DevExpress.Spreadsheet;
using System.IO;
using DevExpress.Web.ASPxTreeList;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web
{
    public partial class DepartmentView : UserControl
    {
        // private SPList _SPList;
        private string addNewItem = string.Empty;

        #region constant
        private bool enableDivision;
        // CompanyHelper cHelper;
        List<Company> companies = new List<Company>();
        List<CompanyDivision> divisions = new List<CompanyDivision>();
        List<Department> departments = new List<Department>();

        public string companyLabel;
        public string divisionLevel;
        public string departmentLevel;
        DataView dvDept;
        #endregion


        private string absoluteUrlImport = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&listName={1}";
        public string importUrl;
        public bool HideMigrate;
        public bool HideApplyChanges;
        public string moveToProductionUrl = "";
        public string Level2EditFunction = "";
        ApplicationContext context = HttpContext.Current.GetManagerContext();

        CompanyManager objCompanyManager;
        DepartmentManager objDepartmentManager;
        CompanyDivisionManager objCompanyDivisionManager;
        protected override void OnInit(EventArgs e)
        {
            objCompanyDivisionManager = new CompanyDivisionManager(context);
            companyLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Company);
            divisionLevel = uHelper.GetDepartmentLabelName(DepartmentLevel.Division);
            departmentLevel = uHelper.GetDepartmentLabelName(DepartmentLevel.Department);

            if (context.ConfigManager.GetValueAsBool(ConfigConstants.EnableAdminImport))
                btnImport.Visible = false;

            hdnConfiguration.Set("NewCompanyURL", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=companynew"));
            hdnConfiguration.Set("NewDivisionURL", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=divisionnew"));
            hdnConfiguration.Set("NewDepartmentURL", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=departmentnew"));
            hdnConfiguration.Set("EditCompanyURL", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=companyedit"));
            hdnConfiguration.Set("EditDivisionURL", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=divisionedit"));
            hdnConfiguration.Set("EditDepartmentURL", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=departmentedit"));
            hdnConfiguration.Set("RequestUrl", Request.Url.AbsoluteUri);
            enableDivision = context.ConfigManager.GetValueAsBool(ConfigConstants.EnableDivision);
            hdnConfiguration.Set("enableDivision", enableDivision);

            importUrl = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlImport, "importexcelfile", "Department"));
            //Migrate Url
            moveToProductionUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=movestagetoproduction&list={0}", DatabaseObjects.Tables.Department));

            //contenSplitter.Panes["Division"].Visible = false;
            if (enableDivision)
            {
                Level2EditFunction = "editDivision";
            }
            else
            {
                Level2EditFunction = "editDepartment";
            }

            objCompanyManager = new CompanyManager(context); //new CompanyHelper(SPContext.Current.Web);
            objDepartmentManager = new DepartmentManager(context); //new DepartmentHelper(SPContext.Current.Web);
            //if (!Page.IsPostBack)
                BindTree();
            EnableMigrate();
            if (!HideApplyChanges)
                btnApplyChanges.Visible = true;
            //base.OnInit(e);
        }

        private void BindTree()
        {
            DataTable dtDepartments = objDepartmentManager.GetDepartmentInfo(enableDivision, chkShowDeleted.Checked);
            
            if (!enableDivision)
            {
                dtDepartments = dtDepartments?.AsEnumerable()
                                .GroupBy(x => x.Field<string>("Title")).Select(y => y.First()).CopyToDataTable();
            }

            dvDept = new DataView(dtDepartments);
            if (!chkShowDeleted.Checked)
            {
                dvDept.RowFilter = "DELETED = False"; //Filter Records which are not deleted
            }
            dvDept.Sort = "Title ASC";
            TreeListDepartments.DataSource = dvDept;
            TreeListDepartments.DataBind();
            dvDept = new DataView(dtDepartments);
            dvDept.RowFilter = string.Empty;
        }

        private void EnableMigrate()
        {
            if (context.ConfigManager.GetValueAsBool(ConfigConstants.EnableMigrate) && !HideMigrate)
            {
                btnMigrateCompany.Visible = true;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            // condition for download excel...
            if (Request["exportType"] == "excel")
            {
                companies.Clear();
                companies = objCompanyManager.GetCompanyData().Where(x => !x.Deleted).ToList();

                departments.Clear();
                departments = objDepartmentManager.GetDepartmentData().Where(x => !x.Deleted).ToList();

                var DeptInfo = (from x in companies
                                join y in departments on x.ID equals y.CompanyIdLookup
                                select new { y.Title, y.DepartmentDescription, y.Deleted, CompanyTitleLookup = x.Title, y.GLCode, y.Manager, y.Attachments }).ToList();

                DataTable table = UGITUtility.ToDataTable(DeptInfo);

                var worksheet = ASPxSpreadsheet1.Document.Worksheets.ActiveWorksheet;
                // var worksheet = ASPxSpreadsheet1.Document.Worksheets.Add();
                worksheet.Import(table, true, 0, 0);
                MemoryStream st = new MemoryStream();
                ASPxSpreadsheet1.Document.SaveDocument(st, DocumentFormat.OpenXml);
                Response.Clear();
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment; filename=Department.xlsx");
                Response.BinaryWrite(st.ToArray());
                Response.End();
            }
            if (Request["__EVENTTARGET"] != null)
            {
                if (Convert.ToString(Request["__EVENTTARGET"]).Contains("DeleteItem"))
                {
                    string Custom_ID = Request["__EVENTARGUMENT"];
                    if (Custom_ID != "")
                    {
                        DeleteItem(Custom_ID);
                        BindTree();
                    }
                }
            }

        }


        protected void chkShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            BindTree();
            if (chkShowDeleted.Checked)
            {
                TreeListDepartments.ExpandAll();
            }
        }

        protected void gvCompany_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;
            bool archived = UGITUtility.StringToBoolean(e.GetValue(DatabaseObjects.Columns.Deleted));
            if (archived)
            {
                e.Row.CssClass += " archived-row";
            }
        }

        protected void gvDivision_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;
            bool archived = UGITUtility.StringToBoolean(e.GetValue(DatabaseObjects.Columns.Deleted));
            if (archived)
            {
                e.Row.CssClass += " archived-row";
            }
        }

        protected void gvDepartment_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;
            bool archived = UGITUtility.StringToBoolean(e.GetValue(DatabaseObjects.Columns.Deleted));
            if (archived)
            {
                e.Row.CssClass += " archived-row";
            }
        }


        protected void btnApplyChanges_Click(object sender, EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            string cacheName = "Lookup_" + DatabaseObjects.Tables.Department + "_" + context.TenantID;
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Department, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            Util.Cache.CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);

            cacheName = "Lookup_" + DatabaseObjects.Tables.Company + "_" + context.TenantID;
            dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Company, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            Util.Cache.CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);

            cacheName = "Lookup_" + DatabaseObjects.Tables.CompanyDivisions + "_" + context.TenantID;
            dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CompanyDivisions, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            Util.Cache.CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);

            TicketManager ticketManager = new TicketManager(context);
            ticketManager.ApplyChangesToTicketCache(DatabaseObjects.Columns.DivisionLookup);
        }
        protected void btExpandAllAllocation_Click(object sender, ImageClickEventArgs e)
        {
            TreeListDepartments.ExpandAll();
        }

        protected void btCollapseAllAllocation_Click(object sender, ImageClickEventArgs e)
        {
            TreeListDepartments.CollapseAll();
        }
        protected void TreeListDepartments_HtmlRowPrepared(object sender, DevExpress.Web.ASPxTreeList.TreeListHtmlRowEventArgs e)
        {
            string func = "";
            if (e.NodeKey.StartsWith("NN")) //Edit icon not required for <None> row
                return;
            if (Object.Equals(e.GetValue("DELETED"), true))
                e.Row.CssClass += " archived-row";
            if(e.Level == 1)
            {
                func = string.Format("javascript:editCompnay({0});", e.NodeKey.Substring(2));
                e.Row.Cells[1].Attributes.Add("onClick", func);
                e.Row.Cells[2].Attributes.Add("onClick", func);
                e.Row.Cells[3].Attributes.Add("onClick", func);
                e.Row.Attributes.Add("style", "cursor:pointer");
                if (Object.Equals(e.GetValue("DELETED"), true))
                    e.Row.Cells[4].Text = "";
                else
                    e.Row.Cells[4].Text = "<a href='javascript:confirmDelete(\"" + e.NodeKey + "\");'><img src='/content/images/redNew_delete.png' alt='Delete' class='btnEditCss' width=16px; /></a>";
                e.Row.Cells[4].Text += "<a href='javascript:editCompnay(" + e.NodeKey.Substring(2) + ");'><img src='/content/images/editNewIcon.png' alt='Edit' class='deleteBtn-report' width=16px; /></a>";
            }
            else if(e.Level == 2)
            {
                func = string.Format("javascript:{1}({0});", e.NodeKey.Substring(2), Level2EditFunction);
                e.Row.Cells[2].Attributes.Add("onClick", func);
                e.Row.Cells[3].Attributes.Add("onClick", func);
                e.Row.Cells[4].Attributes.Add("onClick", func);
                e.Row.Attributes.Add("style", "cursor:pointer");
                if (Object.Equals(e.GetValue("DELETED"), true))
                    e.Row.Cells[5].Text = "";
                else
                    e.Row.Cells[5].Text = "<a href='javascript:confirmDelete(\"" + e.NodeKey + "\");'><img src='/content/images/redNew_delete.png' alt='Delete' class='btnEditCss' width=16px; title='Delete'/></a>";
                e.Row.Cells[5].Text += "<a href='javascript:" + Level2EditFunction + "(" + e.NodeKey.Substring(2) + ");'><img src='/content/images/editNewIcon.png' alt='Edit' class='deleteBtn-report' width=16px; title='Edit'/></a>";
            }
            else if(e.Level == 3)
            {
                func = string.Format("javascript:editDepartment({0});", e.NodeKey.Substring(2));
                e.Row.Cells[3].Attributes.Add("onClick", func);
                e.Row.Cells[4].Attributes.Add("onClick", func);
                e.Row.Cells[5].Attributes.Add("onClick", func);
                e.Row.Attributes.Add("style", "cursor:pointer");
                if (Object.Equals(e.GetValue("DELETED"), true))
                    e.Row.Cells[6].Text = "";
                else
                    e.Row.Cells[6].Text = "<a href='javascript:confirmDelete(\"" + e.NodeKey + "\");'><img src='/content/images/redNew_delete.png' alt='Delete' class='btnEditCss' width=16px; title='Delete'/></a>";
                e.Row.Cells[6].Text += "<a href='javascript:editDepartment(" + e.NodeKey.Substring(2) + ");'><img src='/content/images/editNewIcon.png' alt='Edit' class='deleteBtn-report' width=16px; title='Edit'/></a>";
            }
        }

        protected void TreeListDepartments_NodeDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        { 
            Department objDepartment = new Department();            
            objDepartment = objDepartmentManager.LoadByID(Convert.ToInt32(e.Keys[0].ToString().Substring(2)));
            objDepartment.Deleted = false;
            objDepartmentManager.Update(objDepartment);
            Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Deleted Department: {objDepartment.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);

        }
        private void DeleteItem(string custom_Id)
        {
            if (custom_Id.StartsWith("CM")) {
                Company objCompany = new Company();
                objCompany = objCompanyManager.LoadByID(Convert.ToInt32(custom_Id.Substring(2)));
                objCompany.Deleted = true;
                objCompanyManager.Update(objCompany);
                Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Deleted Company: {objCompany.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
            }
            else if (custom_Id.StartsWith("DV")) {
                CompanyDivision objDivision = new CompanyDivision();
                objDivision = objCompanyDivisionManager.LoadByID(Convert.ToInt32(custom_Id.Substring(2)));
                objDivision.Deleted = true;
                objCompanyDivisionManager.Update(objDivision);
                Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Deleted Division: {objDivision.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
            }
            else if (custom_Id.StartsWith("DP"))
            {
                Department objDepartment = new Department();
                objDepartment = objDepartmentManager.LoadByID(Convert.ToInt32(custom_Id.Substring(2)));
                objDepartment.Deleted = true;
                objDepartmentManager.Update(objDepartment);
                Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Deleted Department: {objDepartment.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
            }
        }

        protected void TreeListDepartments_CustomCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomCallbackEventArgs e)
        {
            bool hasChild = TreeListDepartments.FocusedNode.HasChildren;
            string nodeKey = TreeListDepartments.FocusedNode.Key;
            if (hasChild)
            {
                TreeListNodeCollection childNodes = TreeListDepartments.FocusedNode.ChildNodes;
                string childKey = "";
                for (int i = 0; i < childNodes.Count; i++)
                {
                    childKey = childNodes[i].Key;
                    for (int j = 0; j < childNodes[i].ChildNodes.Count; j++)
                    {
                        childKey = childNodes[i].ChildNodes[j].Key;
                        DeleteItem(childKey);
                    }
                    childKey = childNodes[i].Key;
                    DeleteItem(childNodes[i].Key);
                }
            }
            DeleteItem(nodeKey);
        }
    }
}
