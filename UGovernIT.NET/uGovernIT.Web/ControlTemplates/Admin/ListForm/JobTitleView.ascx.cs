using DevExpress.Web;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using uGovernIT.Utility.Entities;
using System.Collections.Specialized;
using DevExpress.Spreadsheet;
using System.IO;
using uGovernIT.DAL;

namespace uGovernIT.Web
{
    public partial class JobTitleView : UserControl
    {
        protected string editUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=addjobtitle");
        protected string departmentJobtitleMappingUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=departmentjobtitlemapping");
        protected string ImportJobTitleUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=importexcelfile");
    
        
        protected override void OnInit(EventArgs e)
        {
             
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
 
        protected void btnExport_Click(object sender, EventArgs e)
        {
            var table = new DataTable();
            table.Columns.Add("Job Title");
            table.Columns.Add("Short Name");
            table.Columns.Add("Job Type");
            table.Columns.Add("User Role");
            table.Columns.Add("Low Revenue Capacity");
            table.Columns.Add("High Revenue Capacity");
            table.Columns.Add("Low Project Capacity");
            table.Columns.Add("High Project Capacity");
            table.Columns.Add("Resource Level Tolerance");
            table.Columns.Add("Division");
            table.Columns.Add("Department");
            table.Columns.Add("Cost Rate");
        
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            Dictionary<string, object> arrParams = new Dictionary<string, object>();
            arrParams.Add("@TenantID", context.TenantID);
            arrParams.Add("@deptid", null);
            arrParams.Add("@roleid", null);
            DataSet dtResultBillings = uGITDAL.ExecuteDataSet_WithParameters("USP_GetJobTitle", arrParams);
            if (dtResultBillings != null && dtResultBillings.Tables.Count > 0)
            {
                DataTable jobTitles = dtResultBillings.Tables[0];
                DataTable department = dtResultBillings.Tables[1];
                foreach (DataRow roleRow in jobTitles.Rows)
                {
                    if (!Convert.ToBoolean(roleRow["Deleted"]))
                    {
                        DataRow[] foundDeptRows = department.Select($"JobTitleLookup={Convert.ToString(roleRow["ID"])} AND Deleted=0");
                        if (foundDeptRows.Length > 0)
                        {
                            foreach (DataRow deptRow in foundDeptRows)
                            {
                                DataRow dr = table.NewRow();
                                dr["Job Title"] = Convert.ToString(roleRow["Title"]);
                                dr["Short Name"] = Convert.ToString(roleRow["ShortName"]);
                                dr["Job Type"] = Convert.ToString(roleRow["JobType"]);
                                dr["User Role"] = Convert.ToString(roleRow["RoleName"]);
                                string lowRevenueCapacity = Convert.ToString(roleRow["LowRevenueCapacity"]);
                                if (string.IsNullOrWhiteSpace(lowRevenueCapacity) || lowRevenueCapacity.Trim() == "0")
                                    lowRevenueCapacity = "";
                                dr["Low Revenue Capacity"] = lowRevenueCapacity;
                                string highRevenueCapacity = Convert.ToString(roleRow["HighRevenueCapacity"]);
                                if (string.IsNullOrWhiteSpace(highRevenueCapacity) || highRevenueCapacity.Trim() == "0")
                                    highRevenueCapacity = "";
                                dr["High Revenue Capacity"] = highRevenueCapacity;
                                string lowProjectCapacity = Convert.ToString(roleRow["LowProjectCapacity"]);
                                if (string.IsNullOrWhiteSpace(lowProjectCapacity) || lowProjectCapacity.Trim() == "0")
                                    lowProjectCapacity = "";
                                dr["Low Project Capacity"] = lowProjectCapacity;
                                string highProjectCapacity = Convert.ToString(roleRow["HighProjectCapacity"]);
                                if (string.IsNullOrWhiteSpace(highProjectCapacity) || highProjectCapacity.Trim() == "0")
                                    highProjectCapacity = "";
                                dr["High Project Capacity"] = highProjectCapacity;
                                string resourceLevelTolerance = Convert.ToString(roleRow["ResourceLevelTolerance"]);
                                if (string.IsNullOrWhiteSpace(resourceLevelTolerance) || resourceLevelTolerance.Trim() == "0")
                                    resourceLevelTolerance = "";
                                dr["Resource Level Tolerance"] = resourceLevelTolerance;
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
                                string costRate = Convert.ToString(deptRow["EmpCostRate"]);
                                if (string.IsNullOrWhiteSpace(costRate) || costRate.Trim() == "0")
                                    costRate = "";
                                dr["Cost Rate"] = costRate.Trim();
                                table.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = table.NewRow();
                            dr["Job Title"] = Convert.ToString(roleRow["Title"]);
                            dr["Short Name"] = Convert.ToString(roleRow["ShortName"]);
                            dr["Job Type"] = Convert.ToString(roleRow["JobType"]);
                            dr["User Role"] = Convert.ToString(roleRow["RoleName"]);
                            dr["Low Revenue Capacity"] = Convert.ToString(roleRow["LowRevenueCapacity"]);
                            dr["High Revenue Capacity"] = Convert.ToString(roleRow["HighRevenueCapacity"]);
                            dr["Low Project Capacity"] = Convert.ToString(roleRow["LowProjectCapacity"]);
                            dr["High Project Capacity"] = Convert.ToString(roleRow["HighProjectCapacity"]);
                            dr["Resource Level Tolerance"] = Convert.ToString(roleRow["ResourceLevelTolerance"]);
                            dr["Division"] = "";
                            dr["Department"] = "";
                            dr["Cost Rate"] = "";
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
            Response.AddHeader("content-disposition", "attachment; filename=JobTitles.xlsx");
            Response.BinaryWrite(st.ToArray());
            Response.End();
        }
    }
}
