using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.DAL.Store;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace uGovernIT.Web
{
    public partial class AddUserRoles : UserControl
    {
        public string roleID { get; set; }
        List<LandingPages> spUserRoleList;
        LandingPages spitem;
        //UserRolesManager ObjUserRolesManager = new UserRolesManager(HttpContext.Current.GetManagerContext());
        LandingPagesManager ObjLandingPagesManager = new LandingPagesManager(HttpContext.Current.GetManagerContext());

        UserProfileManager umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
        Microsoft.AspNet.Identity.RoleManager<UserRoles> userRolesManager = new Microsoft.AspNet.Identity.RoleManager<UserRoles>(new UserRoleStore<UserRoles>(HttpContext.Current.GetManagerContext()));
        protected override void OnInit(EventArgs e)
        {
            bindSitePages();
            spUserRoleList = ObjLandingPagesManager.GetLandingPages().Where(x => x.Deleted == false).ToList();
            if (string.IsNullOrEmpty(roleID))
            {
                spitem = new LandingPages();
                txtUserRole.Text = "";
                txtDescription.Text = "";
                lnkDelete.Visible = false;
            }
            else
            {
                lnkDelete.Visible = true;
                spitem = spUserRoleList.Where(x => x.Id == roleID).FirstOrDefault();
                if (spitem != null)
                {
                    txtUserRole.Text = spitem.Name;
                    txtDescription.Text = Convert.ToString(spitem.Description);
                    //string strValue = string.Format("/{0}/", Utility.Constants.Pages.Replace(" ", ""));
                    string strValue = string.Empty;
                    if (spitem.LandingPage.Contains("/Admin/"))
                        strValue = Convert.ToString(spitem.LandingPage).Replace($"/{Utility.Constants.Admin}/", "").Replace(".aspx", ""); //string.Format("/{0}/", Utility.Constants.Admin.Replace(" ", ""));
                    else
                        strValue = Convert.ToString(spitem.LandingPage).Replace($"/{Utility.Constants.Pages}/", ""); //string.Format("/{0}/", Utility.Constants.Pages.Replace(" ", ""));

                    //ddlLandingPage.SelectedIndex = ddlLandingPage.Items.IndexOf(ddlLandingPage.Items.FindByValue(Convert.ToString(spitem.LandingPage).Replace(strValue, "")));
                    ddlLandingPage.SelectedIndex = ddlLandingPage.Items.IndexOf(ddlLandingPage.Items.FindByValue(strValue));
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateSkill())
            {
                spitem.Title = txtUserRole.Text;
                spitem.Name = txtUserRole.Text;
                spitem.Description = txtDescription.Text;
                if (ddlLandingPage.SelectedValue.Equals(Utility.Constants.Admin, StringComparison.InvariantCultureIgnoreCase))
                    spitem.LandingPage = string.Format("/{0}/{1}.aspx", Utility.Constants.Admin, ddlLandingPage.SelectedValue);
                else if (ddlLandingPage.SelectedValue.Equals(Utility.Constants.NewAdminUI, StringComparison.InvariantCultureIgnoreCase))
                    spitem.LandingPage = string.Format("/{0}/{1}.aspx", Utility.Constants.Admin, ddlLandingPage.SelectedValue);
                else if (ddlLandingPage.SelectedValue.Equals(Utility.Constants.NewLoginWizard, StringComparison.InvariantCultureIgnoreCase))
                    spitem.LandingPage = string.Format("/{0}/{1}.aspx", Utility.Constants.SitePages, ddlLandingPage.SelectedValue);
                else
                    spitem.LandingPage = string.Format("/{0}/{1}", Utility.Constants.Pages, ddlLandingPage.SelectedValue);
                //ObjLandingPagesManager.AddOrUpdate(spitem);
                if (string.IsNullOrEmpty(roleID))                
                    ObjLandingPagesManager.Insert(spitem);                
                else
                    ObjLandingPagesManager.Update(spitem);
                Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added/Updated User Roles: {spitem.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);

                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        private Boolean ValidateSkill()
        {
            // SPQuery query = new SPQuery();
           // query.Query = string.Format("<Where><And><Neq><FieldRef Name='{0}'/><Value Type='Number'>{1}</Value></Neq><Eq><FieldRef Name='{2}'/><Value Type='Text'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.Id, roleID, DatabaseObjects.Columns.Title, txtUserRole.Text);
            List<LandingPages> collection = spUserRoleList.Where(x=>x.Id!=roleID && x.Name == txtUserRole.Text).ToList();
            if (collection.Count > 0)
            {
                lblErrorMessage.Text = "Role is already in the list";
                return false;
            }
            else
            { return true; }
        }

        private void bindSitePages()
        {           
            PageConfigurationManager pageConfigurationManager = new PageConfigurationManager(HttpContext.Current.GetManagerContext());
            List<string> listPageConfiguration = pageConfigurationManager.Load().OrderBy(x => x.Name).Select(x=>x.Name).ToList();
            listPageConfiguration.Insert(0, "NewAdminUI");
            listPageConfiguration.Insert(0, "Admin");
            listPageConfiguration.Insert(0, "NewLoginWizard");
            ddlLandingPage.DataSource = listPageConfiguration.OrderBy(x => x).ToList();

            ddlLandingPage.DataBind();
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {            
            List<UserProfile> lstUser = new List<UserProfile>();
            IdentityResult result;
            spitem = spUserRoleList.Where(x => x.Id == roleID).FirstOrDefault();
            if (spitem != null)
            {
                lstUser = umanager.GetUsersProfile().Where(x => x.UserRoleId == spitem.Id).ToList();
                lstUser.ForEach(x => x.UserRoleId = null ); // Remove Users Roles before actually deleting Roles.
                foreach (UserProfile item in lstUser)
                {
                    result = umanager.Update(item);
                }

                Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Deleted User Roles: {spitem.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                ObjLandingPagesManager.Delete(spitem);            
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}
