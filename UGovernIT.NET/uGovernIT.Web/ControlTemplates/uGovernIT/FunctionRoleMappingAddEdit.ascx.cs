using DevExpress.Web;
using DevExpress.XtraPivotGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class FunctionRoleMappingAddEdit : System.Web.UI.UserControl
    {
        FunctionRoleManager _functionRoleManager = new FunctionRoleManager(HttpContext.Current.GetManagerContext());    
        FunctionRoleMappingManager _functionRoleMappingManager = new FunctionRoleMappingManager(HttpContext.Current.GetManagerContext());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadFunctions();
                loadRoles();
            }
        }

        private void loadRoles()
        {
            GlobalRoleManager _globalRoleManager = new GlobalRoleManager(HttpContext.Current.GetManagerContext());
            List<GlobalRole> _roles = _globalRoleManager.Load();
            lstRoles.DataSource = _roles;
            lstRoles.DataBind();
        }

        private void loadFunctions()
        {
            List<FunctionRole> functionRoles = _functionRoleManager.Load();
            cmbFunction.ValueField = DatabaseObjects.Columns.ID;
            cmbFunction.TextField = DatabaseObjects.Columns.Title;
            cmbFunction.DataSource = functionRoles;
            cmbFunction.DataBind();
        }

        protected void btnSaveMapping_Click(object sender, EventArgs e)
        {
            FunctionRoleMapping functionRoleMappingObj = null;
            FunctionRoleMappingManager functionRoleMappingManager = new FunctionRoleMappingManager(HttpContext.Current.GetManagerContext());
            FunctionRoleMapping existingObj = null;

            if (lstRoles.SelectedItems == null || lstRoles.SelectedItems.Count  <= 0)
                return;
            if (cmbFunction.SelectedIndex < 0)
                return;

            List<FunctionRoleMapping> lstFunctionRoleMapping = _functionRoleMappingManager.LoadFunctionRoleMappingById(UGITUtility.StringToLong(cmbFunction.SelectedItem?.Value));
            foreach (ListEditItem item in lstRoles.SelectedItems)
            {
                functionRoleMappingObj = new FunctionRoleMapping();
                existingObj = lstFunctionRoleMapping.FirstOrDefault(x => x.FunctionId == UGITUtility.StringToLong(cmbFunction.SelectedItem?.Value) 
                && x.RoleId == UGITUtility.ObjectToString(item.Value)); 
                if(existingObj == null)
                { 
                    functionRoleMappingObj.FunctionId = UGITUtility.StringToLong(cmbFunction.SelectedItem?.Value);
                    functionRoleMappingObj.RoleId = UGITUtility.ObjectToString(item.Value);
                    _functionRoleMappingManager.Insert(functionRoleMappingObj);
                }
            }
            
            uHelper.ClosePopUpAndEndResponse(HttpContext.Current, true);
        }
    }
}