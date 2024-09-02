
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Manager;


namespace uGovernIT.Web
{
    public partial class DepartmentNew : UserControl
    {
        //  private SPListItem _SPListItem;
        private bool enableDivision;
        public string companyLabel;
        public string divisionLevel;
        public string departmentLevel;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        CompanyManager objCompanyManager;
        CompanyDivisionManager objCompanyDivisionManager;
        DepartmentManager objDepartmentManager;
        protected override void OnInit(EventArgs e)
        {
            objDepartmentManager = new DepartmentManager(context);
            objCompanyDivisionManager = new CompanyDivisionManager(context);
            objCompanyManager = new CompanyManager(context);
            companyLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Company);
            divisionLevel = uHelper.GetDepartmentLabelName(DepartmentLevel.Division);
            departmentLevel = uHelper.GetDepartmentLabelName(DepartmentLevel.Department);

            enableDivision = context.ConfigManager.GetValueAsBool(ConfigConstants.EnableDivision);

            if (enableDivision)
            {
                companyTr.Visible = true;
                divisionTr.Visible = true;
                ddlCompany.AutoPostBack = true;
                ddlCompany.SelectedIndexChanged += ddlCompany_SelectedIndexChanged;
                BindCompany();
                BindDivisioin();
            }
            else
            {
                companyTr.Visible = true;
                divisionTr.Visible = false;
                BindCompany();
            }


            base.OnInit(e);
        }

        void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDivisioin();
        }

        private void BindCompany()
        {
            List<Company> cmpny = objCompanyManager.Load().Where(x => x.Deleted == false).ToList();
            foreach (Company row in cmpny)
            {
                ddlCompany.Items.Add(new ListItem(row.Title, Convert.ToString(row.ID)));
            }
            ddlCompany.Items.Insert(0, new ListItem("None", "0"));
            // ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(Request["companyid"]));

        }

        private void BindDivisioin()
        {
            ddlDivision.ClearSelection();
            ddlDivision.Items.Clear();
            List<CompanyDivision> dtDivision = objCompanyDivisionManager.Load(x => x.CompanyIdLookup == Convert.ToInt64(ddlCompany.SelectedValue) && x.Deleted == false).OrderBy(x => x.Title).ToList();
            if (dtDivision == null || dtDivision.Count <= 0 || ddlCompany.SelectedItem == null)
            {
                ddlDivision.Items.Insert(0, new ListItem("None", "0"));
                return;
            }
            else
            {

                foreach (CompanyDivision row in dtDivision)
                {
                    ddlDivision.Items.Add(new ListItem(row.Title, Convert.ToString(row.ID)));
                }
            }

            ddlDivision.Items.Insert(0, new ListItem("None", "0"));
            // ddlDivision.SelectedIndex = ddlDivision.Items.IndexOf(ddlDivision.Items.FindByValue(Request["divisionid"]));
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            Department objDepartment = new Department();
            objDepartment.Title = txtTitle.Text.Trim();
            objDepartment.ShortName = txtshortName.Text.Trim();
            objDepartment.DepartmentDescription = txtDescription.Text.Trim();
            objDepartment.GLCode = txtGLCode.Text.Trim();
            objDepartment.CompanyIdLookup = Convert.ToInt64(ddlCompany.SelectedValue.Trim());
            if (enableDivision)
            {
                objDepartment.DivisionIdLookup = Convert.ToInt64(ddlDivision.SelectedValue.Trim());
                if (Convert.ToInt64(ddlDivision.SelectedValue) == 0)
                {
                    objDepartment.DivisionIdLookup = null;

                }
            }
            objDepartment.Manager = Convert.ToString(ppeManager.GetValues());
            long _divisionIdLookup = 0;

           long.TryParse(objDepartment.DivisionIdLookup.ToString(),out _divisionIdLookup);
           if (!CheckExistingDepartment(_divisionIdLookup, objDepartment.Title))
            {
                objDepartmentManager.Insert(objDepartment);
                UGITUtility.CreateCookie(Response, "companycooke", ddlCompany.SelectedValue);
                UGITUtility.CreateCookie(Response, "divisioncooke", ddlDivision.SelectedValue);
                Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Added Department: {objDepartment.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);

            }
            else
            {   
                Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Duplicate Department: {objDepartment.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected bool CheckExistingDepartment(long divisionId, string departmentName)
        {
            var data = objDepartmentManager.Load(x => x.Title.Trim().ToLower() == departmentName.Trim().ToLower() && (x.DivisionIdLookup==divisionId|| x.DivisionIdLookup==null));
            if (data != null && data.Count>0)
                return true;
            return false;
        }
        protected void cvtxtTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {

            if (enableDivision)
            {
                List<Department> lstDepartment = objDepartmentManager.Load(x => x.Title.ToLower() == txtTitle.Text.Trim().ToLower() && x.CompanyIdLookup == Convert.ToInt64(ddlCompany.SelectedValue) && x.DivisionIdLookup == Convert.ToInt64(ddlDivision.SelectedValue));
                if (lstDepartment.Count > 0)
                {
                    args.IsValid = false;
                }
            }
            else
            {
                List<Department> lstDepartment = objDepartmentManager.Load(x => x.Title.ToLower() == txtTitle.Text.Trim().ToLower() && x.CompanyIdLookup == Convert.ToInt64(ddlCompany.SelectedValue));
                if (lstDepartment.Count > 0)
                {
                    args.IsValid = false;
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
