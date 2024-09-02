
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using System.Linq;
using uGovernIT.Manager.Managers;
using DevExpress.ExpressApp;

namespace uGovernIT.Web
{
    public partial class UserRoleTypeNew : UserControl
    {
        private ModuleUserTypeManager objModuleUserTypeManager;
        private ModuleViewManager moduleViewManager;
        private GlobalRoleManager globalRoleManager;
        private TicketManager ticketManager;
        private DataTable ticketList;
        private ApplicationContext context;
        
        public UserRoleTypeNew()
        {
            context = HttpContext.Current.GetManagerContext();
            moduleViewManager = new ModuleViewManager(context);
            globalRoleManager = new GlobalRoleManager(context);
            ticketManager = new TicketManager(context);
        }

        protected override void OnInit(EventArgs e)
        {
            long ID = Convert.ToInt32(Request["ID"]);
            if (ID > 0)
            {
                Fill(ID);
            }
            objModuleUserTypeManager = new ModuleUserTypeManager(HttpContext.Current.GetManagerContext());
            BindModule();
            if (Request["module"] != null)
            {
                ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(Convert.ToString(Request["module"])));
            }
            else
            {
                ddlModule.SelectedIndex = 0;
            }
            FillFieldNames(ddlModule.SelectedValue);
            base.OnInit(e);
        }

        private void BindModule()
        {
            ddlModule.Items.Clear();
            List<UGITModule> lstModule = moduleViewManager.Load();
            if (lstModule != null && lstModule.Count > 0)
            {
                lstModule = lstModule.Where(x => x.EnableModule).OrderBy(x => x.ModuleName).ToList();
                ddlModule.DataSource = lstModule;
                ddlModule.DataValueField = DatabaseObjects.Columns.ModuleName;
                ddlModule.DataTextField = DatabaseObjects.Columns.Title;
                ddlModule.DataBind();                
            }

            BindUserTypes();
            FillFieldNames(lstModule.FirstOrDefault().ModuleName);
        }

        private void BindUserTypes()
        {
            IList<string> roleNames = new List<string>();

            IList<GlobalRole> savedRoles = globalRoleManager.Load(x => !x.Deleted && x.TenantID.EqualsIgnoreCase(context.TenantID));
            if (savedRoles != null && savedRoles.Count > 0)
            {
                IList<string> savedrolelist = savedRoles.Select(x => x.Name).Distinct().ToList();
                roleNames = savedrolelist;
            }
            IList<string> existingRoleTypes = objModuleUserTypeManager.Load().OrderBy(x => x.UserTypes).Select(x => x.UserTypes).Distinct().ToList();
            IList<string> combinedUserTypes = roleNames.Union(existingRoleTypes).ToList();

            combinedUserTypes.Add("Service Provider");

            ddlUserTypes.DataSource = combinedUserTypes.OrderBy(x => x);
            ddlUserTypes.DataBind();
        }
        private void Fill(long ID)
        {
            ModuleUserType objModuleUserType = objModuleUserTypeManager.LoadByID(ID);
            if (objModuleUserType == null)
                objModuleUserType = new ModuleUserType();
            ddlModule.SelectedItem.Text = objModuleUserType.ModuleNameLookup;
            cmbFieldName.SelectedItem.Value = objModuleUserType.ColumnName;
            ddlUserTypes.SelectedIndex = ddlUserTypes.Items.IndexOf(ddlUserTypes.Items.FindByText(objModuleUserType.UserTypes));
            ///Default User Value
            ppeDefaultUser.SetValues(objModuleUserType.DefaultUser);
            ///User Groups Value
            ppeUserGroups.SetValues(objModuleUserType.Groups);
            chkITOnly.Checked = objModuleUserType.ITOnly;
            chkManagerOnly.Checked = objModuleUserType.ManagerOnly;
            txtCustomProperties.Text = objModuleUserType.CustomProperties;
        }

        void FillFieldNames(string moduleName)
        {
            cmbFieldName.Items.Clear();

            List<string> roleUserFields = new List<string>();
            List<GlobalRole> savedRoles = globalRoleManager.Load(x => !x.Deleted && x.TenantID.EqualsIgnoreCase(context.TenantID));
            if (savedRoles != null && savedRoles.Count > 0)
            {
                List<string> savedRoleUserList = savedRoles.Select(x => x.FieldName).Distinct().ToList();
                roleUserFields = savedRoleUserList.ToList();
            }

            List<string> existingFields = new List<string>();
            UGITModule spListItem = moduleViewManager.Get(x => x.ModuleName == moduleName);

            if (spListItem != null)
                ticketList = ticketManager.GetColumnDetail(DatabaseObjects.Tables.InformationSchema, spListItem.ModuleTable);

            if (ticketList != null)
            {
                foreach (DataRow dataColumns in ticketList.Rows)
                {
                    if (dataColumns != null && !dataColumns["COLUMN_NAME"].Equals("ContentType") || !dataColumns["COLUMN_NAME"].Equals(DatabaseObjects.Columns.Title) || !dataColumns["COLUMN_NAME"].Equals(DatabaseObjects.Columns.Author) || !dataColumns["COLUMN_NAME"].Equals(DatabaseObjects.Columns.Modified) || !dataColumns["COLUMN_NAME"].Equals(DatabaseObjects.Columns.Created) || !dataColumns["COLUMN_NAME"].Equals(DatabaseObjects.Columns.CreatedByUser) || !dataColumns["COLUMN_NAME"].Equals(DatabaseObjects.Columns.ModifiedByUser))
                        if (!dataColumns["COLUMN_NAME"].Equals(DatabaseObjects.Columns.TenantID))
                            existingFields.Add(Convert.ToString(dataColumns["COLUMN_NAME"]));
                }

            }
            //BTS-24-001750: Add all Active Roles to User Role Mapping
            //Not adding all the userRoleFields from Roles table. It should be coming from Table Schema only
            roleUserFields.Clear();

            existingFields = existingFields.Where(x => x.EndsWith("user", StringComparison.OrdinalIgnoreCase)).Distinct().ToList();
            IList<string> combinedUserTypeFields = roleUserFields.Union(existingFields).ToList();
            
            cmbFieldName.DataSource = combinedUserTypeFields.OrderBy(x => x);
            cmbFieldName.DataBind();
        }
        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlModule.SelectedIndex != -1)
            {
                BindUserTypes();
                FillFieldNames(ddlModule.SelectedValue);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ModuleUserType objModuleUserType = new ModuleUserType();
            objModuleUserTypeManager = new ModuleUserTypeManager(HttpContext.Current.GetManagerContext());
            objModuleUserType.Title = ddlModule.SelectedValue + " - " + ddlUserTypes.Text;
            objModuleUserType.ModuleNameLookup = Convert.ToString(ddlModule.SelectedValue);
            objModuleUserType.ColumnName = cmbFieldName.Text;
            if (!string.IsNullOrWhiteSpace(txtUserType.Text))
            {
                objModuleUserType.UserTypes = txtUserType.Text.Trim();
            }
            else
            {
                objModuleUserType.UserTypes = ddlUserTypes.Text;
            }
            //objModuleUserType.UserTypes = ddlUserTypes.Text;
            objModuleUserType.ITOnly = chkITOnly.Checked;
            objModuleUserType.ManagerOnly = chkManagerOnly.Checked;
            objModuleUserType.CustomProperties = txtCustomProperties.Text;
            objModuleUserType.DefaultUser = ppeDefaultUser.GetValues();
            objModuleUserType.Groups = ppeUserGroups.GetValues();
            objModuleUserTypeManager.Insert(objModuleUserType);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
