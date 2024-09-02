using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Manager.Core;
using uGovernIT.Manager.Managers;
using DevExpress.Web;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;
using uGovernIT.Util.Cache;

namespace uGovernIT.Web
{
    public partial class GlobalRolesAddEdit : System.Web.UI.UserControl
    {
        private ModuleViewManager ModuleManagerObj;
        private GlobalRoleManager GlobalRoleManagerObj;
        private ModuleUserTypeManager objModuleUserTypeManager;
        private ApplicationContext context;

        public string RoleID { get; set; }
        public GlobalRole RoleObj;
        public GlobalRolesAddEdit()
        {
            context = HttpContext.Current.GetManagerContext();
            ModuleManagerObj = new ModuleViewManager(context);
            GlobalRoleManagerObj = new GlobalRoleManager(context);
            objModuleUserTypeManager = new ModuleUserTypeManager(context);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            loadFieldNames();
            //RoleID = UGITUtility.ObjectToString(Request["RoleID"]);
            
            if (!string.IsNullOrEmpty(RoleID))
            {
                //btnDelete.Visible = true;
                RoleObj = GlobalRoleManagerObj.LoadById(RoleID);
            }
            if (!IsPostBack)
            {
                if (RoleObj != null)
                {
                    txtName.Text = RoleObj.Name;
                    txtshortName.Text= RoleObj.ShortName;
                    cmbFieldName.Items.Add(new ListEditItem() { Text = RoleObj.FieldName, Value = RoleObj.FieldName });
                    cmbFieldName.SelectedIndex = cmbFieldName.Items.IndexOf(cmbFieldName.Items.FindByText(RoleObj.FieldName));
                    memoDescription.Text = RoleObj.Description;
                    chkDeleted.Checked = RoleObj.Deleted;
                }
            }
        }

        private void loadFieldNames()
        {
            FieldConfigurationManager fieldManagerObj = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
            List<string> lstModuleNames = new List<string> { "Opportunity", "CRMServices", "CRMProject","PMM", "NPR" };
            List<string> fieldNames = new List<string>();
            
            foreach (string mItem in lstModuleNames)
            {
                List<FieldConfiguration> fields = fieldManagerObj.Load(x => x.Datatype == "UserField" && x.ParentTableName == mItem);
                if (fields != null)
                {
                    List<string> mitemFields = fields.Select(x => x.FieldName).Distinct().ToList();
                    fieldNames = fieldNames.Union(mitemFields).ToList();
                }
            }
            fieldNames.Insert(0, "<-- select -->");

            List<GlobalRole> savedRoles = GlobalRoleManagerObj.Load();
            if(savedRoles != null && savedRoles.Count > 0)
            {
                List<string> savedrolelist = savedRoles.Select(x => x.FieldName).ToList();
                fieldNames = fieldNames.Except(savedrolelist).ToList();
            }
            cmbFieldName.DataSource = fieldNames.OrderBy(x=>x);
            cmbFieldName.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";
            if (!ASPxEdit.ValidateEditorsInContainer(Page))
                return;
            if (CheckDuplicateRole(txtName.Text))
                return;

            if (!string.IsNullOrEmpty(RoleID) && RoleObj != null)
            {
                RoleObj.Name = txtName.Text.Trim();
                RoleObj.ShortName=txtshortName.Text.Trim();
                if(cmbFieldName.SelectedItem != null)
                {
                    if (cmbFieldName.SelectedItem.Text != "<-- select -->")
                        RoleObj.FieldName = cmbFieldName.SelectedItem.Text;
                    else
                        RoleObj.FieldName = null;
                }
                //if (hdnNewFieldFlag.Contains("IsNewField") && UGITUtility.StringToBoolean(hdnNewFieldFlag.Get("IsNewField")) && !string.IsNullOrEmpty(txtName.Text))
                {
                    string fieldname = Regex.Replace(txtName.Text, @"[^\w\d]", "");
                    if (!string.IsNullOrEmpty(fieldname))
                        GlobalRoleManagerObj.addNewUserField(fieldname + "User");
                    RoleObj.FieldName = fieldname + "User";
                }
                RoleObj.Description = memoDescription.Text;
                RoleObj.Deleted = chkDeleted.Checked;
                GlobalRoleManagerObj.Update(RoleObj);
                GlobalRoleManagerObj.MapUserRoles(RoleObj.Id, RoleObj.Deleted);
            }
            else
            {
                GlobalRole globalRole = new GlobalRole();
                globalRole.Name = txtName.Text;
                globalRole.ShortName = txtshortName.Text.Trim();
                if (cmbFieldName.SelectedItem != null)
                {
                    if(cmbFieldName.SelectedItem.Text != "<-- select -->")
                        globalRole.FieldName = cmbFieldName.SelectedItem.Text;
                    else
                        globalRole.FieldName = null;
                }
                if(hdnNewFieldFlag.Contains("IsNewField") && UGITUtility.StringToBoolean(hdnNewFieldFlag.Get("IsNewField")) && !string.IsNullOrEmpty(txtName.Text))
                {
                    string fieldname = Regex.Replace(txtName.Text, @"[^\w\d]", "");
                    if(!string.IsNullOrEmpty(fieldname))
                        GlobalRoleManagerObj.addNewUserField(fieldname + "User");
                    globalRole.FieldName = fieldname + "User";
                }
                globalRole.Description = memoDescription.Text;
                globalRole.Id = UGITUtility.ObjectToString(Guid.NewGuid());
                GlobalRoleManagerObj.Insert(globalRole);
                GlobalRoleManagerObj.MapUserRoles(globalRole.Id);
            }
            uHelper.ClosePopUpAndEndResponse(HttpContext.Current, true);
        }

        private bool CheckDuplicateRole(string roleName)
        {
            bool result = false;
            List<GlobalRole> role = GlobalRoleManagerObj.Load(x => x.Name.ToLower() == roleName.Trim().ToLower() && x.Id != RoleID && !x.Deleted);
            if (role != null && role.Count > 0)
            {
                lblErrorMessage.Text = "Role Name Already Exits.";
                return true;
            }

            return result;
        }

        protected void txtName_Validation(object sender, DevExpress.Web.ValidationEventArgs e)
        {
            ASPxTextBox txtName = sender as ASPxTextBox;

            string roleName = txtName.Text;
            if (CheckDuplicateRole(roleName))
            {
                e.IsValid = false;
                e.ErrorText = "Role Name Already Exits.";
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RoleID))
            {
                UserProfileManager userManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
                //update users
                List<UserProfile> usersWithRoleid = userManager.GetUsersByGlobalRoleID(RoleID);
                foreach(UserProfile user in usersWithRoleid)
                {
                    user.GlobalRoleId = string.Empty;
                    //user.JobTitleLookup = 0;
                    IdentityResult result = userManager.Update(user);
                }

                //update JobTitles
                JobTitleManager jobTitleManager = new JobTitleManager(HttpContext.Current.GetManagerContext());
                List<JobTitle> jobtitleWithRole = jobTitleManager.Load(x => x.RoleId == RoleID);
                foreach(JobTitle jobtitle in jobtitleWithRole)
                {
                    jobtitle.RoleId = string.Empty;
                    jobTitleManager.Update(jobtitle);
                }
                RoleObj.Deleted = true;
                GlobalRoleManagerObj.Update(RoleObj);
                GlobalRoleManagerObj.MapUserRoles(RoleObj.Id, RoleObj.Deleted);
            }
            uHelper.ClosePopUpAndEndResponse(HttpContext.Current, true);
        }
        

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}