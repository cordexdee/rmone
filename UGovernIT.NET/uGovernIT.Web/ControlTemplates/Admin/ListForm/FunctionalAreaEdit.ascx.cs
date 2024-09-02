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
using Utils;
using System.Data.SqlClient;
using System.Web;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class FunctionalAreaEdit : UserControl
    {
        public int Id { private get; set; }
        protected string departmentLabel;
        FunctionalArea objFunctionalAreas = new FunctionalArea();
        FunctionalAreasManager objFunctionalAreasManager = new FunctionalAreasManager(HttpContext.Current.GetManagerContext());
        ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        CompanyManager companyManager = new CompanyManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            var enableCompanies = companyManager.Load(x => x.Deleted == false).Select(x => x.ID).ToList();
            bool enableDivision = configurationVariableManager.GetValueAsBool(ConfigConstants.EnableDivision);
            if (enableDivision)
                ddlDepartment.FilterExpression = $"{DatabaseObjects.Columns.CompanyIdLookup} in ({string.Join(Constants.Separator6, enableCompanies)}) and {DatabaseObjects.Columns.Deleted} = 'False' and {DatabaseObjects.Columns.DivisionIdLookup} IS NOT NULL";
            else
                ddlDepartment.FilterExpression = $"{DatabaseObjects.Columns.CompanyIdLookup} in ({string.Join(Constants.Separator6, enableCompanies)}) and {DatabaseObjects.Columns.Deleted} = 'False' and {DatabaseObjects.Columns.DivisionIdLookup} IS NULL";

            objFunctionalAreas = objFunctionalAreasManager.LoadByID(Convert.ToInt64(Id));
            departmentLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Department);
            if (!IsPostBack)
            {
                Fill();
            }
         
            base.OnInit(e);
        }
        private void Fill()
        {
            if (objFunctionalAreas != null)
            {
                txtTitle.Text = objFunctionalAreas.Title;
                txtDescription.Text = objFunctionalAreas.FunctionalAreaDescription;
                ppeOwner.SetValues(objFunctionalAreas.Owner);
                ddlDepartment.SetValues(Convert.ToString(objFunctionalAreas.DepartmentLookup));
                chkDeleted.Checked = objFunctionalAreas.Deleted;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            objFunctionalAreas.ID = Id;
            objFunctionalAreas.Title = txtTitle.Text.Trim();
            objFunctionalAreas.FunctionalAreaDescription = txtDescription.Text.Trim();
            objFunctionalAreas.Owner = ppeOwner.GetValues();
            //objFunctionalAreas.DepartmentLookup = Convert.ToInt32(ddlDepartment.GetValues());
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

            objFunctionalAreas.Deleted = chkDeleted.Checked;
            objFunctionalAreasManager.Update(objFunctionalAreas);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Updated Funcational Area: {objFunctionalAreas.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}
