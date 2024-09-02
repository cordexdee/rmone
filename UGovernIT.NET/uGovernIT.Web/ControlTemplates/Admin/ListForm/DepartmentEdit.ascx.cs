
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using uGovernIT.Utility;
using uGovernIT.Manager;

namespace uGovernIT.Web
{
    public partial class DepartmentEdit : UserControl
    {
        public int DepartmentID { get; set; }
        //private SPListItem _SPListItem;
        bool enableDivision;
        public string companyLabel;
        public string divisionLevel;
        public string departmentLevel;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        CompanyManager objCompanyManager;
        CompanyDivisionManager objCompanyDivisionManager;
        DepartmentManager objDepartmentManager;
        Department objDepartment;
        protected override void OnInit(EventArgs e)
        {
            objDepartmentManager = new DepartmentManager(context);
            objCompanyDivisionManager = new CompanyDivisionManager(context);
            objCompanyManager = new CompanyManager(context);
            companyLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Company);
            divisionLevel = uHelper.GetDepartmentLabelName( DepartmentLevel.Division);
            departmentLevel = uHelper.GetDepartmentLabelName(DepartmentLevel.Department);

            //if (DepartmentID <= 0)
            //    SPUtility.TransferToErrorPage(Constants.MalformedURLMsg);

            objDepartment = new Department();
            objDepartment = objDepartmentManager.LoadByID(DepartmentID);
            //_SPListItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.Department, DepartmentID);
            enableDivision = context.ConfigManager.GetValueAsBool(ConfigConstants.EnableDivision);

            //if (_SPListItem == null)
            //    SPUtility.TransferToErrorPage(Constants.MalformedURLMsg);

            if (enableDivision)
            {
                companyTr.Visible = true;
                divisionTr.Visible = true;
                ddlCompany.AutoPostBack = true;
                ddlCompany.SelectedIndexChanged += ddlCompany_SelectedIndexChanged;
                BindCompany();
            }
            else
            {
                companyTr.Visible = true;
                divisionTr.Visible = false;
                BindCompany();
            }


            Fill();
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
            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(Request["companyid"]));

        }

        private void BindDivisioin()
        {
            ddlDivision.ClearSelection();
            ddlDivision.Items.Clear();
            List<CompanyDivision> dtDivision = objCompanyDivisionManager.Load(x => x.CompanyIdLookup == Convert.ToInt64(ddlCompany.SelectedValue)).OrderBy(x => x.Title).ToList();
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
            ddlDivision.SelectedIndex = ddlDivision.Items.IndexOf(ddlDivision.Items.FindByValue(Request["divisionid"]));
        }

        private void Fill()
        {
            if (objDepartment != null)
            {

                txtTitle.Text = objDepartment.Title;
                txtshortName.Text=objDepartment.ShortName;
                txtDescription.Text = objDepartment.DepartmentDescription;
                txtGLCode.Text = objDepartment.GLCode;
                chkDeleted.Checked = objDepartment.Deleted;
                // SPFieldLookupValue cmpSPFieldValue = new SPFieldLookupValue(Convert.ToString(_SPListItem[DatabaseObjects.Columns.CompanyTitleLookup]));
                ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(Convert.ToString(objDepartment.CompanyIdLookup)));

                if (enableDivision)
                {
                    BindDivisioin();
                    // SPFieldLookupValue divSPFieldValue = new SPFieldLookupValue(Convert.ToString(_SPListItem[DatabaseObjects.Columns.DivisionLookup]));
                    ddlDivision.SelectedIndex = ddlDivision.Items.IndexOf(ddlDivision.Items.FindByValue(Convert.ToString(objDepartment.DivisionIdLookup)));
                }
                ppeManager.SetValues(objDepartment.Manager);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            objDepartment.Title = txtTitle.Text.Trim();
            objDepartment.ShortName=txtshortName.Text.Trim();
            objDepartment.DepartmentDescription = txtDescription.Text.Trim();
            objDepartment.GLCode = txtGLCode.Text.Trim();
            objDepartment.Deleted = chkDeleted.Checked;

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
            objDepartmentManager.Update(objDepartment);
            Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Updated Department: {objDepartment.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void cvtxtTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {

            if (enableDivision)
            {
                List<Department> lstDepartment = objDepartmentManager.Load(x => x.ID!= DepartmentID && x.Title.ToLower() == txtTitle.Text.Trim().ToLower() && x.CompanyIdLookup == Convert.ToInt64(ddlCompany.SelectedValue) && x.DivisionIdLookup == Convert.ToInt64(ddlDivision.SelectedValue));
                if (lstDepartment.Count > 0)
                {
                    args.IsValid = false;
                }
            }
            else
            {
                List<Department> lstDepartment = objDepartmentManager.Load(x => x.ID != DepartmentID && x.Title.ToLower() == txtTitle.Text.Trim().ToLower() && x.CompanyIdLookup == Convert.ToInt64(ddlCompany.SelectedValue));
                if (lstDepartment.Count > 0)
                {
                    args.IsValid = false;
                }
            }
        }
    }
}
