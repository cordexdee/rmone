using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using System.Data.SqlClient;
using Utils;
using DevExpress.Web;
using System.Web;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class FunctionalAreaNew : UserControl
    {
        protected string departmentLabel;
        ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        CompanyManager companyManager = new CompanyManager(HttpContext.Current.GetManagerContext());
        FunctionalAreasManager objFunctionalAreasManager = new FunctionalAreasManager(HttpContext.Current.GetManagerContext());
        FunctionalArea objFunctionalAreas = new FunctionalArea();

        protected override void OnInit(EventArgs e)
        {
            var enableCompanies = companyManager.Load(x => x.Deleted == false).Select(x => x.ID).ToList();

            bool enableDivision = configurationVariableManager.GetValueAsBool(ConfigConstants.EnableDivision);
            if (enableDivision)
                ddlDepartment.FilterExpression = $"{DatabaseObjects.Columns.CompanyIdLookup} in ({string.Join(Constants.Separator6, enableCompanies)}) and {DatabaseObjects.Columns.Deleted} = 'False' and {DatabaseObjects.Columns.DivisionIdLookup} IS NOT NULL";
            else
                ddlDepartment.FilterExpression = $"{DatabaseObjects.Columns.CompanyIdLookup} in ({string.Join(Constants.Separator6, enableCompanies)}) and {DatabaseObjects.Columns.Deleted} = 'False' and {DatabaseObjects.Columns.DivisionIdLookup} IS NULL";

            //ppeOwner.UserTokenBoxAdd.ValidationSettings.RequiredField.IsRequired = true;
            //ppeOwner.UserTokenBoxAdd.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithText;
            //ppeOwner.UserTokenBoxAdd.ValidationSettings.ErrorText = "Value Cannot be Null";
            //ppeOwner.UserTokenBoxAdd.ValidationSettings.RequiredField.ErrorText = "Value Cannot be Null";
            //ppeOwner.UserTokenBoxAdd.ValidationSettings.ValidationGroup = "Save";
            departmentLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Department);
            base.OnInit(e);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            objFunctionalAreas.Title = txtTitle.Text.Trim();
            objFunctionalAreas.FunctionalAreaDescription = txtDescription.Text.Trim();
            objFunctionalAreas.Deleted = chkDeleted.Checked;
            if (ddlDepartment.GetValues() != null)
            {
                if (ddlDepartment.GetValues() == "" || ddlDepartment.GetValues() == "0")
                {
                    objFunctionalAreas.DepartmentLookup = null;
                }
                else
                {
                    objFunctionalAreas.DepartmentLookup = Convert.ToInt64(ddlDepartment.GetValues());
                }

            }
            UserValue useval = new UserValue();
            objFunctionalAreas.Owner = ppeOwner.GetValues();
            objFunctionalAreasManager.Insert(objFunctionalAreas);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added Funcational Area: {objFunctionalAreas.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}
