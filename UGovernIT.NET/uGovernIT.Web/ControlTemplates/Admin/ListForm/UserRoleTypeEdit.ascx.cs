
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Linq;
using uGovernIT.Manager.Managers;
using DevExpress.Charts.Native;

namespace uGovernIT.Web
{
    public partial class UserRoleTypeEdit : UserControl
    {
        public int Id { private get; set; }
        private DataTable ticketList;
        ModuleUserTypeManager objModuleUserTypeManager;
        ModuleUserType objModuleUserTypes;
        string ModuleName { get; set; }
        private ModuleViewManager moduleViewManager;
        private GlobalRoleManager globalRoleManager;
        private TicketManager ticketManager;

        private ApplicationContext context;
        public UserRoleTypeEdit()
        {
            context = HttpContext.Current.GetManagerContext();
            moduleViewManager = new ModuleViewManager(context);
            globalRoleManager = new GlobalRoleManager(context);
            ticketManager = new TicketManager(context);
        }

        protected override void OnInit(EventArgs e)
        {
            objModuleUserTypeManager = new ModuleUserTypeManager(HttpContext.Current.GetManagerContext());
            objModuleUserTypes = new ModuleUserType();
            objModuleUserTypes = objModuleUserTypeManager.LoadByID(Id);
            if (!IsPostBack)
            {
                ModuleName = objModuleUserTypes.ModuleNameLookup;
                BindColumnNames(ModuleName);
                BindUserTypes();
                BindModule();
                Fill();
            }
            base.OnInit(e);
        }
        private void BindColumnNames(string modulename)
        {
            /*
            ModuleFormLayoutManager formLayoutManager = new ModuleFormLayoutManager(HttpContext.Current.GetManagerContext());
            List<ModuleFormLayout> moduleFormLayout = formLayoutManager.Load(x=>x.ModuleNameLookup == modulename);
            cbColumnName.DataSource = moduleFormLayout.Where(x => !x.FieldName.StartsWith("#"));
            cbColumnName.TextField = DatabaseObjects.Columns.FieldName;
            cbColumnName.ValueField = DatabaseObjects.Columns.FieldName;
            cbColumnName.DataBind();
            */
            cbColumnName.Items.Clear();

            List<string> roleUserFields = new List<string>();

            List<GlobalRole> savedRoles = globalRoleManager.Load(x => !x.Deleted && x.TenantID.EqualsIgnoreCase(context.TenantID));
            if (savedRoles != null && savedRoles.Count > 0)
            {
                List<string> savedRoleUserList = savedRoles.Select(x => x.FieldName).Distinct().ToList();
                roleUserFields = savedRoleUserList.ToList();
            }

            List<string> existingFields = new List<string>();
            UGITModule spListItem = moduleViewManager.Get(x => x.ModuleName == modulename);

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

            existingFields = existingFields.Where(x=>x.EndsWith("user", StringComparison.OrdinalIgnoreCase)).Distinct().ToList();
            IList<string> combinedUserTypeFields = roleUserFields.Union(existingFields).ToList();

            cbColumnName.DataSource = combinedUserTypeFields.OrderBy(x => x);
            cbColumnName.DataBind();
        }
        private void BindUserTypes()
        {
            /*
            List<ModuleUserType> rows = objModuleUserTypeManager.Load(x => x.ModuleNameLookup == ModuleName);
            foreach (ModuleUserType dr in rows)
            {
                ddlUserTypes.Items.Add(new ListItem { Text = dr.UserTypes, Value = dr.UserTypes });
            }
            */

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
        }

        private void Fill()
        {
            ddlModule.Items.FindByValue(objModuleUserTypes.ModuleNameLookup).Selected = true;
            cbColumnName.SelectedIndex = cbColumnName.Items.IndexOf(cbColumnName.Items.FindByText(objModuleUserTypes.ColumnName));
            ddlUserTypes.SelectedIndex = ddlUserTypes.Items.IndexOf(ddlUserTypes.Items.FindByText(objModuleUserTypes.UserTypes));
            ppeDefaultUser.SetValues(objModuleUserTypes.DefaultUser);
            ppeUserGroups.SetValues(objModuleUserTypes.Groups);
            chkITOnly.Checked = objModuleUserTypes.ITOnly;
            chkManagerOnly.Checked = objModuleUserTypes.ManagerOnly;
            txtCustomProperties.Text = objModuleUserTypes.CustomProperties;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ModuleUserType objModuleUserType = objModuleUserTypeManager.LoadByID(Id); ;
            objModuleUserType.Title = ddlModule.SelectedValue + " - " + ddlUserTypes.Text;
            objModuleUserType.ModuleNameLookup = Convert.ToString(ddlModule.SelectedValue);
            objModuleUserType.ColumnName = cbColumnName.SelectedItem.Text;
            if (!string.IsNullOrWhiteSpace(txtSubCategory.Text))
            {
                objModuleUserType.UserTypes = txtSubCategory.Text.Trim();
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
            objModuleUserTypeManager.Update(objModuleUserType);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            if (objModuleUserTypes != null)
            {
                objModuleUserTypeManager.Delete(objModuleUserTypes);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlModule.SelectedIndex != -1)
            {
                BindUserTypes();
                ModuleName = ddlModule.SelectedValue;
            }
        }
    }
}
