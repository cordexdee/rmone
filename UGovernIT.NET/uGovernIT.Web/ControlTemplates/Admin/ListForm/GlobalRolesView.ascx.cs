using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using uGovernIT.Manager;
using DevExpress.Web;
using System.Data;
using System.IO;
using DevExpress.Spreadsheet;
using uGovernIT.DAL;

namespace uGovernIT.Web
{
    public partial class GlobalRolesView : System.Web.UI.UserControl
    {
        public string EditUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=addRoles");
        public string ImportUserRolesUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=importexcelfile");
        protected string editUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=addRoles");
        protected string departmentJobtitleMappingUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=departmentrolemapping");
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            var table = new DataTable();
            table.Columns.Add("User Role");
            table.Columns.Add("Short Name");
            table.Columns.Add("Description");
            table.Columns.Add("Division");
            table.Columns.Add("Department");
            table.Columns.Add("Billing Rate");
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            Dictionary<string, object> arrParams = new Dictionary<string, object>();
            arrParams.Add("@TenantID", context.TenantID);
            arrParams.Add("@deptid", null);
            arrParams.Add("@roleid", null);
            DataSet dtResultBillings = uGITDAL.ExecuteDataSet_WithParameters("usp_GetRole", arrParams);
            if (dtResultBillings != null && dtResultBillings.Tables.Count > 1)
            {
                DataTable roles = dtResultBillings.Tables[0];
                DataTable department = dtResultBillings.Tables[1];
                foreach (DataRow roleRow in roles.Rows)
                {
                    if (!Convert.ToBoolean(roleRow["Deleted"]))
                    {
                        DataRow[] foundDeptRows = department.Select($"RoleLookup='{Convert.ToString(roleRow["Id"])}' AND Deleted=0");
                        if (foundDeptRows.Length > 0)
                        {
                            foreach (DataRow deptRow in foundDeptRows)
                            {
                                DataRow dr = table.NewRow();
                                dr["User Role"] = Convert.ToString(roleRow["Name"]);
                                dr["Short Name"] = Convert.ToString(roleRow["ShortName"]);
                                dr["Description"] = Convert.ToString(roleRow["Description"]);
                                string deptName = Convert.ToString(deptRow["DepartmentName"]);
                                if (!string.IsNullOrWhiteSpace(deptName))
                                {
                                    string[] divDept = deptName.Split('>');
                                    dr["Division"] = divDept[0].Trim();
                                    dr["Department"] = divDept[1].Trim();
        }
                                else
                                {
                                    dr["Division"] = "";
                                    dr["Department"] = "";
                                }
                                string billingRate = Convert.ToString(deptRow["BillingRate"]);
                                if (string.IsNullOrWhiteSpace(billingRate) || billingRate.Trim() == "0")
                                    billingRate = "";
                                dr["Billing Rate"] = billingRate.Trim();
                                table.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = table.NewRow();
                            dr["User Role"] = Convert.ToString(roleRow["Name"]);
                            dr["Short Name"] = Convert.ToString(roleRow["ShortName"]);
                            dr["Description"] = Convert.ToString(roleRow["Description"]);
                            dr["Division"] = "";
                            dr["Department"] = "";
                            dr["Billing Rate"] = "";
                            table.Rows.Add(dr);
                        }
                    }
                }
            }
        
            var worksheet = ASPxSpreadsheet1.Document.Worksheets.ActiveWorksheet;
            worksheet.Import(table, true, 0, 0);
            MemoryStream st = new MemoryStream();
            ASPxSpreadsheet1.Document.SaveDocument(st, DocumentFormat.OpenXml);
            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment; filename=UserRoles.xlsx");
            Response.BinaryWrite(st.ToArray());
            Response.End();
        }
        //protected void aspxGridJobTitles_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        //{
        //    GlobalRoleManager rolesManager = new GlobalRoleManager(HttpContext.Current.GetManagerContext());
        //    if (e.RowType != GridViewRowType.Data)
        //        return;

        //    GlobalRole currentRow = grdRoles.GetRow(e.VisibleIndex) as GlobalRole;
        //    string func = string.Empty;
        //    string roleId = string.Empty;
        //    if (currentRow != null)
        //    {
        //        if (Convert.ToString(currentRow.Id) != string.Empty)
        //        {
        //            roleId = currentRow.Id.ToString().Trim();
        //        }
        //        func = string.Format("openGlobalRoleDialog('{0}','{1}','{2}','{3}','{4}', 0)", EditUrl, string.Format("RoleID={0}", roleId), "Edit Role", "600px", "400px");
        //        e.Row.Attributes.Add("onClick", func);
        //        e.Row.Attributes.Add("style", "cursor:pointer");

        //    }
        //}
    }
}