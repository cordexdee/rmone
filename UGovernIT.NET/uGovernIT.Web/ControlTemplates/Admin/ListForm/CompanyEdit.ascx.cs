using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Linq;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using System.Web;

namespace uGovernIT.Web
{
    public partial class CompanyEdit : UserControl
    {
       // SPList companyList;
        public int CompanyID{get;set;}
       // SPListItem item;
        public string companyLabel;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        CompanyManager objCompanyManager;
        Company companyList;
        protected override void OnInit(EventArgs e)
        {
            objCompanyManager = new CompanyManager(context);
            companyList = objCompanyManager.LoadByID(CompanyID);
            companyLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Company);
            if (Request["__EVENTTARGET"] != null)
            {
                if (Convert.ToString(Request["__EVENTTARGET"]).Contains("DeleteChildItem"))
                {
                    string deleteChildAction = Request["__EVENTARGUMENT"];
                    DeleteChildItem(deleteChildAction); //deletes child divisions and departments
                }
            }

            Fill();
            if (companyList.Deleted)
            {
                btUnDelete.Visible = true;
                btDelete.Visible = false;
            }
            else
            {
                btUnDelete.Visible = false;
                btDelete.Visible = true;
            }
            base.OnInit(e);
        }
        //This method deletes the Divisions and Departments mapped to the company being edited.
        private void DeleteChildItem(string deleteChildAction)
        {
            //deletes the Divisions and Departments mapped to the company as per user input.
            if (deleteChildAction == "1")
            {
                DepartmentManager objDepartmentManager = new DepartmentManager(context);
                CompanyDivisionManager objCompanyDivisionManager = new CompanyDivisionManager(context);

                //List<Company> allCompanies = objCompanyManager.LoadAllHierarchy().Where(x => x.ID == CompanyID).ToList();
                List<CompanyDivision> divisions = objCompanyDivisionManager.GetCompanyDivisionData().Where(x => x.CompanyIdLookup == CompanyID).ToList();//allCompanies[0].CompanyDivisions.Where(x => x.CompanyIdLookup == CompanyID).ToList();
                List<Department> departments;

                departments = objDepartmentManager.GetDepartmentData().Where(x => x.CompanyIdLookup == CompanyID).ToList();
                foreach (CompanyDivision divn in divisions)
                {
                    divn.Deleted = true;
                    Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Updated Division: {divn.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
                    objCompanyDivisionManager.Update(divn);
                }
                foreach (Department dept in departments)
                {
                    dept.Deleted = true;
                    Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Updated Department: {dept.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
                    objDepartmentManager.Update(dept);
                }
            }
            //delete the company 
            companyList.Deleted = !companyList.Deleted;
            Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Updated Company: {companyList.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
            objCompanyManager.Update(companyList);

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
         
        }
        private void Fill()
        {
            if (companyList != null)
            {
                txtTitle.Text = companyList.Title;
                txtshortName.Text = companyList.ShortName;
                txtDescription.Text = companyList.Description;
                txtGLCode.Text = companyList.GLCode;
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (companyList != null)
            {
                companyList.Title = txtTitle.Text.Trim();
                companyList.ShortName=txtshortName.Text.Trim();
                companyList.Description = txtDescription.Text.Trim();
                companyList.GLCode = txtGLCode.Text.Trim();
                Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Updated Company: {companyList.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
                objCompanyManager.Update(companyList);
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void cvtxtTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {
            List<Company> lstCompny = objCompanyManager.Load(x => x.ID!= CompanyID &&  x.Title == txtTitle.Text.Trim());
            if (lstCompny.Count > 0)
            {
                args.IsValid = false;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btUnDelete_Click(object sender, EventArgs e)
        {
            companyList.Deleted = false;
            Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Updated Company: {companyList.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
            objCompanyManager.Update(companyList);

            uHelper.ClosePopUpAndEndResponse(Context, true);

        }
    }
}
