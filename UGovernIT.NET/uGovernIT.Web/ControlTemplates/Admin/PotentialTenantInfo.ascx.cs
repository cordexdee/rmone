using DevExpress.Web;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class PotentialTenantInfo : UserControl
    {
        UserProfile currentUser;
        UserProfileManager userManager;
        private TenantRegistrationManager _tenantRegistrationManager = null;
        private ApplicationContext _context = null;

        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }

        }

        protected TenantRegistrationManager TenantRegistrationManager
        {
            get
            {
                if (_tenantRegistrationManager == null)
                {
                    _tenantRegistrationManager = new TenantRegistrationManager(HttpContext.Current.GetManagerContext());
                }
                return _tenantRegistrationManager;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            currentUser = HttpContext.Current.CurrentUser();
            userManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            var isSuperAdmin = userManager.IsUGITSuperAdmin(currentUser);
            if (isSuperAdmin)
            {
                BindPotentialTenantInfo();
            }
            else
            {
                Response.Redirect("~/SuperAdmin/PotentialTenantInfo.aspx", false);
            }
        }

        private void BindPotentialTenantInfo()
        {
            List<TenantRegistration> tenantRegistrations = TenantRegistrationManager.Load(x => x.Deleted == false).OrderBy(x => x.Created).ToList();
            var dt = new DataTable();
            if (tenantRegistrations != null && tenantRegistrations.Count > 0)
            {
                dt.Columns.Add("Id", typeof(string));
                dt.Columns.Add("Company", typeof(string));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Email", typeof(string));
                dt.Columns.Add("Title", typeof(string));
                dt.Columns.Add("Url", typeof(string));
                dt.Columns.Add("Contact", typeof(string));
                dt.Columns.Add("Created", typeof(string));

                TenantRegistrationData tenantRegistrationData = new TenantRegistrationData();
                foreach (var item in tenantRegistrations)
                {
                    DataRow dr = dt.NewRow();
                    tenantRegistrationData = Newtonsoft.Json.JsonConvert.DeserializeObject<TenantRegistrationData>(item.TenantRegistrationData);

                    dr["Id"] = item.Id;
                    dr["Company"] = tenantRegistrationData.Company;
                    dr["Name"] = tenantRegistrationData.Name;
                    dr["Email"] = tenantRegistrationData.Email;
                    dr["Title"] = tenantRegistrationData.Title;
                    dr["Url"] = tenantRegistrationData.Url;
                    dr["Contact"] = tenantRegistrationData.Contact;
                    dr["Created"] = item.Created.ToShortDateString();
                    dt.Rows.Add(dr);
                }
            }
            gvPotentialTenants.DataSource = dt;
            gvPotentialTenants.Settings.ShowFilterBar = GridViewStatusBarMode.Visible;
            gvPotentialTenants.SettingsFilterControl.ViewMode = FilterControlViewMode.VisualAndText;
            gvPotentialTenants.SettingsFilterControl.AllowHierarchicalColumns = false;
            gvPotentialTenants.SettingsFilterControl.ShowAllDataSourceColumns = true;
            gvPotentialTenants.SettingsFilterControl.HierarchicalColumnPopupHeight = 200;

            gvPotentialTenants.DataBind();            
        }

        protected void DeleteTenant_Click(object sender, ImageClickEventArgs e)
        {
            string Id = ((ImageButton)sender).CommandArgument;
            TenantRegistration tenant = TenantRegistrationManager.LoadByID(UGITUtility.StringToLong(Id));
            if (tenant != null)
            {
                tenant.Deleted = true;
                TenantRegistrationManager.Update(tenant);
                BindPotentialTenantInfo();
            }
        }
    }
}