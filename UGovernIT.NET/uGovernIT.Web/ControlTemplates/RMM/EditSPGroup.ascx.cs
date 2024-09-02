using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Text;
using uGovernIT.Manager;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using uGovernIT.Utility.Entities;
using uGovernIT.DAL;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class EditSPGroup : UserControl
    {
        public string GroupId { get; set; }
        //SPGroup group = null;
        Role group = null;
        string username ="";
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        UserRoleManager rolemanager = new UserRoleManager(HttpContext.Current.GetManagerContext());
        UserProfileManager manager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
        Microsoft.AspNet.Identity.RoleManager<Role> UserRoleManager = new Microsoft.AspNet.Identity.RoleManager<Role>(new RoleStore<Role>(HttpContext.Current.GetManagerContext()));

        protected override void OnInit(EventArgs e)
        {
            
            if(Request["groupid"] !=null)
            {
                GroupId=Request["groupid"].ToString();
                
            }
            username = Page.User.Identity.Name.ToString();
            bindSitePages();
            if (!IsPostBack)
            {
                fill();
            }
            
           
            base.OnInit(e);
        }
        //private void FillRoles()
        //{
        //    Array roles = Enum.GetValues(typeof(RoleType));
        //    ddlroleType.Items.Add(new ListItem("Select Role Type", "0").ToString());
        //    foreach (RoleType role in roles)
        //    {
        //        ddlroleType.Items.Add(new ListItem(role.ToString(), ((int)role).ToString()));
        //    }
        //}
        private void fill()
        {
            if (!string.IsNullOrEmpty(GroupId))
            {
                txtNameCustomValidator.Visible = false;
                UserRoleManager u = new UserRoleManager(context);
                if (u != null)
                {
                    group = u.GetRoleList().SingleOrDefault(x => x.Id == GroupId);
                    if (group != null)
                    {
                        bool isResourceAdmin = manager.IsRole("Admin", username) || manager.IsRole("ResourceAdmin", username);
                        if (isResourceAdmin)
                        {
                            btnDelete.Visible = true;
                        }
                        if (group.IsSystem)
                        {
                            btnDelete.Visible = false;
                            //btSave.Visible = false;
                        }
                        //txtName.Text = group.Name;
                        txtName.Text = group.Title;
                        //ListItem item = ddlroleType.Items.FindByValue(group.Name);
                        //if (item != null)
                        //    item.Selected = true;
                        txtDesc.Text = group.Description;
                        //string strValue = string.Format("/{0}/", Utility.Constants.Pages.Replace(" ", ""));
                        string strValue = string.Empty;
                        if (group.LandingPage.Contains("/Admin/"))
                            strValue = Convert.ToString(group.LandingPage).Replace($"/{Utility.Constants.Admin}/", "").Replace(".aspx", "");
                        else
                            strValue = Convert.ToString(group.LandingPage).Replace($"/{Utility.Constants.Pages}/", "");

                        if (group.LandingPage != null)
                        {
                            var landingPage = strValue; //Convert.ToString(group.LandingPage).Replace(strValue, "");

                            foreach (ListItem item in ddlLandingPage.Items)
                            {
                                if (item.Value.Equals(landingPage, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    ddlLandingPage.SelectedIndex = ddlLandingPage.Items.IndexOf(item);
                                    break;
                                }
                            }
                            //ddlLandingPage.SelectedIndex = ddlLandingPage.Items.IndexOf(ddlLandingPage.Items.FindByValue(Convert.ToString(group.LandingPage).Replace(strValue, "")));
                        }

                        usercount.Value = "0";//Convert.ToString(group.Users.Count);
                        grpLiteral.Visible = false;
                    }
                    else {
                        grpLiteral.Visible = true;
                        msgLiteral.Text = "There is no such kind of group";
                        groupTable.Visible = false;
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
            
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            bool isRoleexist = rolemanager.GetRoleList().SingleOrDefault(x => x.Id == GroupId) != null;
            try
            {
                string LandingPage = "";
                if (ddlLandingPage.SelectedValue.Equals(Utility.Constants.Admin, StringComparison.InvariantCultureIgnoreCase))
                    LandingPage = string.Format("/{0}/{1}.aspx", Utility.Constants.Admin, ddlLandingPage.SelectedValue);
                else if (ddlLandingPage.SelectedValue.Equals(Utility.Constants.NewAdminUI, StringComparison.InvariantCultureIgnoreCase))
                    LandingPage = string.Format("/{0}/{1}.aspx", Utility.Constants.Admin, ddlLandingPage.SelectedValue);
                else
                    LandingPage = string.Format("/{0}/{1}", Utility.Constants.Pages, ddlLandingPage.SelectedValue);
                if (!isRoleexist)
                {
                    Role uRole = new Role()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = txtName.Text.Trim().Replace(" ", string.Empty),
                        IsSystem = false,
                        Description = txtDesc.Text,
                        Discriminator = txtName.Text,
                        Title = txtName.Text.Trim(),
                        LandingPage = LandingPage
                    };

                    rolemanager.CreateUserRole(uRole);
                    Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Added Group: {uRole.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
                }

                if (isRoleexist)
                {
                    Role uRoleUpdate = new Role()
                    {
                        Id = GroupId,
                        Name = txtName.Text.Trim().Replace(" ", string.Empty).ToString(),
                        Title = txtName.Text.Trim().ToString(),
                        IsSystem = false,
                        Description = txtDesc.Text,
                        Discriminator = txtName.Text,
                        LandingPage = LandingPage
                    };

                    rolemanager.UpdateUserRole(uRoleUpdate);
                    Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Updated Group: {uRoleUpdate.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            uHelper.ClosePopUpAndEndResponse(Context, true, "/sitePages/RMM/");
        }

        protected void txtNameCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            UserRoleManager roleManager = new UserRoleManager(context);
                List<Role> sRoles = roleManager.GetRoleList().Where(x => x.Title.Replace(" ", "").ToLower() == txtName.Text.Trim().Replace(" ", "").ToLower()).ToList();
                // SPGroup group = SPContext.Current.Web.SiteGroups.Cast<SPGroup>().FirstOrDefault(x => x.Name.ToLower() == txtName.Text.Trim().ToLower());
                if ((sRoles != null && sRoles.Count >0) ||(txtName.Text.Trim().Equals("AllGroups",StringComparison.InvariantCultureIgnoreCase)) ||( txtName.Text.Trim().Equals("DisabledUsers",StringComparison.InvariantCultureIgnoreCase)))
                    args.IsValid = false;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Role u = rolemanager.Get(x=>x.Id==GroupId);
            if (u != null)
            {
                if (!u.IsSystem)
                {
                    u.Deleted = true;
                    UserRoleManager.DeleteAsync(u);
                    rolemanager.UpdateRolesInCache();
                }
            }
            uHelper.ClosePopUpAndEndResponse(Context, true, "/sitePages/RMM/");
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        private void bindSitePages()
        {
            PageConfigurationManager pageConfigurationManager = new PageConfigurationManager(HttpContext.Current.GetManagerContext());
            List<string> listPageConfiguration = pageConfigurationManager.Load().OrderBy(x => x.Name).Select(x => x.Name).ToList();
            listPageConfiguration.Insert(0, "NewAdminUI");
            listPageConfiguration.Insert(0, "Admin");
            ddlLandingPage.DataSource = listPageConfiguration.OrderBy(x => x).ToList();

            ddlLandingPage.DataBind();
        }
    }
}
