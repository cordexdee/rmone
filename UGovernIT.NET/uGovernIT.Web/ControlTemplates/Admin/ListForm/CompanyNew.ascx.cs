using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
namespace uGovernIT.Web
{
    public partial class CompanyNew : UserControl
    {
        public string companyLabel;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        CompanyManager objCompanyManager;
        protected override void OnInit(EventArgs e)
        {
            objCompanyManager = new CompanyManager(context);
            companyLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Company);
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            Company cmpny = new Company();
            cmpny.Title = txtTitle.Text.Trim();
            cmpny.ShortName=txtshortName.Text.Trim();
            cmpny.Description = txtDescription.Text.Trim();
            cmpny.GLCode = txtGLCode.Text.Trim();
            objCompanyManager.Insert(cmpny);
            Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Added Company: {cmpny.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void cvtxtTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {
            List<Company> lstCompny = objCompanyManager.Load(x=> x.Title== txtTitle.Text.Trim());
            if (lstCompny.Count > 0)
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
