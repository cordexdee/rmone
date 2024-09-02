using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using System.Data;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using uGovernIT.Utility;


namespace uGovernIT.Web
{
    public partial class DivisionNew : UserControl
    {
        //SPList divisionList;
        public string companyLabel;
        CompanyManager objCompanyManager;
        CompanyDivisionManager objCompanyDivisionManager;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        //CompanyDivision divisionList;
        protected override void OnInit(EventArgs e)
        {
            objCompanyManager = new CompanyManager(context);
            objCompanyDivisionManager = new CompanyDivisionManager(context);
            companyLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Company);
            BindCompany();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
                
        }

        private void BindCompany()
        {
            List<Company> cmpny = objCompanyManager.Load(x => x.Deleted == false);
            foreach (Company row in cmpny)
            {
                ddlCompany.Items.Add(new ListItem(row.Title, Convert.ToString(row.ID)));
            }
            ddlCompany.Items.Insert(0, new ListItem("None", "0"));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            CompanyDivision cmpny = new CompanyDivision();
            cmpny.Title = txtTitle.Text.Trim();
            cmpny.ShortName=txtshortName.Text.Trim();
            cmpny.Description = txtDescription.Text.Trim();
            cmpny.GLCode = txtGLCode.Text.Trim();
            cmpny.CompanyIdLookup = Convert.ToInt64(ddlCompany.SelectedValue);
            cmpny.Manager = ppeManager.GetValues();
            objCompanyDivisionManager.Insert(cmpny);
            UGITUtility.CreateCookie(Response, "companycooke", ddlCompany.SelectedValue);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        protected void cvtxtTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {
            List<CompanyDivision> data = objCompanyDivisionManager.Load(x => x.Title.ToLower() == txtTitle.Text.Trim().ToLower() && x.CompanyIdLookup == Convert.ToInt64(ddlCompany.SelectedValue));
            if ( data.Count > 0)
            {
                    args.IsValid = false;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
