using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Linq;
using System.Web;
using DevExpress.Web;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity.Owin;

namespace uGovernIT.Web
{
    public partial class BatchCreateWizard : UserControl
    {
        // public string selectedUserIds { get; set; } 
        public string ModuleName;
        private DataRow moduleRow;
        protected string TicketURlBatchCreate = string.Empty;

        protected string sourceUrl = string.Empty;
        protected string popupSourceUrl = string.Empty;
        JsonUserList jUserList = new JsonUserList();
        private DataTable resultedTable;
        UserProfileManager UserManager;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        protected override void OnInit(EventArgs e)
        {
            UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            if (Request["sendsurvey"] == "true")
            {
                divbatchcreate.Attributes.Add("style", "display:none");
            }
            aspxGridviewUser.Columns["Department"].Caption = uHelper.GetDepartmentLabelName(DepartmentLevel.Department);
        }
        static string userIDs = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            moduleRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleName) == this.ModuleName);
            if (moduleRow != null)
            {
                TicketURlBatchCreate = UGITUtility.GetAbsoluteURL(Convert.ToString(moduleRow[DatabaseObjects.Columns.StaticModulePagePath]));
            }

            if (!IsPostBack)
            {
                hdnFileName.Value = "testUserImport";
            }

            trUserImport.Visible = false;
            aspxGridViewGroup.Visible = false;
            aspxGridviewUser.Visible = false;
            if (rbGroupList.Checked)
            {
                aspxGridViewGroup.Visible = true;
                GetAllSiteGroups();
            }
            else if (rbUserList.Checked)
            {
                aspxGridviewUser.Visible = true;
                GetAllSiteUsers();
            }
            else if (rbUserImport.Checked)
            {
                trUserImport.Visible = true;
                lblFileUpload.Visible = true;
                aspxGridviewUser.Visible = true;
            }
            else
            {
                aspxGridviewUser.DataSource = SetResultedData(null);
            }
        }

        private void GetAllSiteGroups()
        {
            //SPDelta 155(Start:-Survey complete functionality)
            List<string> groupList = UserManager.Users.Where(x => x.isRole == true && (x.TenantID == context.TenantID)).Select(x=>x.Name).ToList();   // SPContext.Current.Web.SiteGroups.Cast<SPGroup>().Select(x => x.Name).ToList();
            //SPDelta 155(End:-Survey complete functionality)
            DataTable data = new DataTable();
            data.Columns.Add("Group", typeof(string));
            foreach (string gr in groupList)
            {
                data.Rows.Add(gr);
            }
            aspxGridViewGroup.DataSource = data;

        }
        private void GetAllSiteUsers()
        {


             
            ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());

                   UserProfileManager profileManger = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                string memberGroupName = objConfigurationVariableHelper.GetValue(ConfigConstants.DefaultGroup);
                RoleType role; Enum.TryParse(memberGroupName, out role);
                var rolemanager = new UserRoleManager(context);
                List<Role> userGroup = rolemanager.GetUserRoleByGroup(role);
                List<UserProfile> userList = new List<UserProfile>();
               
                if (userGroup != null & userGroup.Count > 0)
                {
                userList = UserManager.GetEnabledUsers();
                //foreach (Role prole in userGroup)
                //{
                //    userList.AddRange(profileManger.GetFilteredUsers(prole.Id, null, null, null)); // uGITCache.UserProfileCache.GetAllUsers(SPContext.Current.Web);
                //}
            }
            else
                {
                    userList = UserManager.GetUsersProfile();
                }
                aspxGridviewUser.DataSource = SetResultedData(userList);
                hdnSellectAll.Value = "false";
            
        }
        private void GetImportUsers()
        {
            List<UserProfile> userList = new List<UserProfile>();
            List<string> tempUser = new List<string>();
            string filePath = Server.MapPath(string.Format(@"~\Content\csv\{0}", hdnFileName.Value));
            if (File.Exists(filePath))
            {
                StreamReader csvreader = new StreamReader(filePath);
                while (!csvreader.EndOfStream)
                {
                    var line = csvreader.ReadLine();
                    var values = line.Split(';');
                    foreach (var item in values)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            UserProfile user = UserManager.GetUserByUserName(item);
                            if (user != null)
                            {
                                //UserProfile userProfile = UserManager.LoadById(user.Id);
                                //if (userProfile != null)
                                    userList.Add(user);
                            }
                            else
                            {
                                tempUser.Add(item);
                            }
                        }
                    }
                }
                hdnSellectAll.Value = "true";
            }
            aspxGridviewUser.DataSource = SetResultedData(userList);
            lblMessage.Text = "";
            if (tempUser.Count > 0)
            {
                lblMessage.Text = string.Format("{0}, users are not available in site.", string.Join(", ", tempUser.ToArray()));
                lblMessage.Visible = true;
            }
        }
        protected void btnImportUserSubmit_Click(object sender, EventArgs e)
        {
            if (flpImportUser.HasFile)
            {
                string extension = Path.GetExtension(flpImportUser.PostedFile.FileName).ToLower();
                if (extension == ".csv")
                {
                    string uploadedPath = Server.MapPath("~/Content/csv");
                    string fileName = Guid.NewGuid() + flpImportUser.FileName;
                    string fullPath = Path.Combine(uploadedPath, fileName);
                    hdnFileName.Value = fileName;
                    while (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                    flpImportUser.SaveAs(fullPath);
                    GetImportUsers();
                }
                else
                {
                    lblFileUpload.Text = "only csv file.";
                }
            }
            else
            {
                lblFileUpload.Text = "File Required.";
            }
        }
        protected void btnBatchCreateFinish_Click(object sender, EventArgs e)
        {
            string userIds = GetSelectedUsers(); //userIDs;
            if (!string.IsNullOrEmpty(userIds))
            {
                Response.Redirect(string.Format("{0}?TicketId=0&userIds={1}&batchCreate=true&IsDlg=1&isudlg=1", TicketURlBatchCreate, userIds));
            }
            else
            {
                lblSelectUser.Visible = true;
                if (aspxGridviewUser.Visible)
                    lblSelectUser.Text = "Select at least one user ";
                else if (aspxGridViewGroup.Visible)
                    lblSelectUser.Text = "select at least one group or group has no users ";
                else
                    lblSelectUser.Text = "Select at least one user ";
                return;
            }
        }

        private DataTable SetResultedData(List<UserProfile> userList)
        {
            DataTable userTable = new DataTable();
            userTable.Columns.Add(DatabaseObjects.Columns.Id, typeof(string));
            userTable.Columns.Add("Manager");
            userTable.Columns.Add("Title");
            userTable.Columns.Add("Location");
            userTable.Columns.Add("Department");
            //userTable.Columns.Add("Groups");
            userTable.Columns.Add("IT");
            userTable.Columns.Add("Consultant");
            userTable.Columns.Add("IsManager");
            userTable.Columns.Add("Role");
            userTable.Columns.Add("Job");
            userTable.Columns.Add("TitleLink");
            userTable.Columns.Add("GroupsLink");
            userTable.Columns.Add("ManagerLink");
            userTable.Columns.Add("UserSkill");
            if (userList != null)
            {
                sourceUrl = Request.Url.PathAndQuery;
                popupSourceUrl = string.Format("{0}?frameObjId={1}", Request.Url.AbsolutePath, Request["frameObjId"]);
                foreach (UserProfile user in userList)
                {
                    JsonUser jUser = new JsonUser
                    {
                        id = user.Id,
                        parentId = user.ManagerID,
                        name = user.Name,
                        title = user.JobProfile,
                        email = user.Email,
                        phone = user.MobilePhone
                    };

                    jUserList.Users.Add(jUser);

                    DataRow row = userTable.NewRow();
                    row[DatabaseObjects.Columns.Id] = user.Id;
                    row["Title"] = user.Name;
                    if (!string.IsNullOrWhiteSpace(user.Location))
                        row["Location"] = "" + GetTableDataManager.GetTableData(DatabaseObjects.Tables.Location, DatabaseObjects.Columns.ID + "=" + user.Location).Rows[0][DatabaseObjects.Columns.Title];
                    else
                        row["Location"] = user.Location;
                    row["Department"] = user.Department;
                    row["IT"] = user.IsIT ? "Yes" : "No";
                    row["Consultant"] = user.IsConsultant ? "Yes" : "No";
                    row["IsManager"] = user.IsManager ? "Yes" : "No";
                    //row["Role"] = string.Join(",", user.Roles);
                    row["Job"] = user.JobProfile;
                    if (!string.IsNullOrWhiteSpace(user.ManagerID))
                        row["Manager"] = UserManager.GetUserById(user.ManagerID);
                    //SPDelta 155(Start:-Survey complete functionality)
                    string userLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/ControlTemplates/RMM/userinfo.aspx?uID={0}", user.Id));
                    row["TitleLink"] = string.Format("<a href='javascript:window.parent.UgitOpenPopupDialog(\"{0}\",\"\", \"User Details: {1}\", \"600px\",\"90\", false,\"{2}\")'>{3}</a>",
                                                    userLinkUrl, user.Name.Replace("'", string.Empty), Server.UrlEncode(sourceUrl), user.Name);
                    //SPDelta 155(End:-Survey complete functionality)
                    row["Title"] = user.Name;
                    if (!string.IsNullOrWhiteSpace(user.ManagerID))
                    {
                        //SPDelta 155(Start:-Survey complete functionality)
                        string managerLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/ControlTemplates/RMM/userinfo.aspx?uID={0}", user.ManagerID));
                        row["ManagerLink"] = string.Format("<a href='javascript:window.parent.UgitOpenPopupDialog(\"{0}\",\"\", \"User Details: {1}\", \"600px\",\"90\", false, \"{2}\")'>{3}</a>",
                                                    managerLinkUrl, user.Name.Replace("'", string.Empty), Server.UrlEncode(sourceUrl), user.Name);
                        //row["ManagerLink"] = string.Format("<a href='javascript:window.parent.UgitOpenPopupDialog(\"{0}\",\"\", \"User Details: {1}\", \"600px\",\"90\", false, \"{2}\")'>{3}</a>",
                        //                            managerLinkUrl, UserManager.GetUserById(user.ManagerID).Name.Replace("'", string.Empty), Server.UrlEncode(sourceUrl), UserManager.GetUserById(user.ManagerID).Name);
                        //SPDelta 155(End:-Survey complete functionality)
                    }
                    List<string> userSkilllst = new List<string>();

                    //    foreach (var userSkill in user.Skills)  //SPDelta 155(Commented:-Survey complete functionality)
                    //{
                    //    //userSkilllst.Add(userSkill.Value);
                    //}
                    //row["UserSkill"] = string.Join(";", userSkilllst.ToArray()); //SPDelta 155(Commented:-Survey complete functionality)
                    //SPDelta 155(Start:-Survey complete functionality)
                    if (!string.IsNullOrWhiteSpace(user.Skills))
                    {
                    string[] userSkills = user.Skills.Split(',');
                        foreach (var userSkill in userSkills)
                        {
                            userSkilllst.Add(userSkill);
                        }
                    }

                    //SPDelta 155(End:-Survey complete functionality)
                    row["UserSkill"] = string.Join(";", userSkilllst.ToArray());
                   
                    userTable.Rows.Add(row);
                }
            }
            resultedTable = userTable;
            return resultedTable;
        }
        protected void aspxGridviewUser_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column == aspxGridviewUser.Columns["Title"] || e.Column == aspxGridviewUser.Columns["Manager"])
            {
                string displaytext = Convert.ToString(e.GetFieldValue(e.VisibleIndex, e.Column == aspxGridviewUser.Columns["Title"] ? "TitleLink" : "ManagerLink"));
                e.DisplayText = displaytext;
            }
        }
        protected void aspxGridviewUser_DataBinding(object sender, EventArgs e)
        {
            if (rbUserList.Checked)
                GetAllSiteUsers();
            else if (rbUserImport.Checked)
                GetImportUsers();
            else
            {
                aspxGridviewUser.DataSource = SetResultedData(null);
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            if (rbGroupList.Checked)
            {
                aspxGridViewGroup.DataBind();
            }
            else
            {

                aspxGridviewUser.DataBind();
                if (hdnSellectAll.Value == "true")
                    aspxGridviewUser.Selection.SelectAll();
                else
                    aspxGridviewUser.Selection.UnselectAll();
            }

            base.OnPreRender(e);
        }
        protected void cbAll_Init(object sender, EventArgs e)
        {
            ASPxCheckBox chk = sender as ASPxCheckBox;
            DevExpress.Web.ASPxGridView grid = (chk.NamingContainer as GridViewHeaderTemplateContainer).Grid;
            chk.Checked = (grid.Selection.Count == grid.VisibleRowCount);
            var objList = grid.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.Id });
            List<string> idsList = new List<string>();
            foreach (string obj in objList)
            {
                    idsList.Add(Convert.ToString(obj));
            }
            if (idsList.Count() > 0)  
                userIDs = string.Join(",", idsList.ToArray());
            else 
            userIDs = "";
            
        }

        public string GetSelectedUsers()
        {
            string userIds = string.Empty;
            if (aspxGridViewGroup.Visible)
            {
                var groups = aspxGridViewGroup.GetSelectedFieldValues(new string[] { "Group" });
                List<string> allUserIdsList = new List<string>();
                foreach (object obj in groups)
                {
                    List<UserProfile> usersList = UserManager.GetUsersByGroupID(Convert.ToString(obj));
                    if (usersList != null && usersList.Count > 0)
                    {
                        List<string> usersIDs = usersList.Select(x => x.Id).ToList();
                        if (usersIDs != null && usersIDs.Count() > 0)
                            allUserIdsList.AddRange(usersIDs);
                    }
                }
                return string.Join(",", allUserIdsList);
            }
            if (aspxGridviewUser.Visible)
            {
                var objList = aspxGridviewUser.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.Id });
                if (objList != null && objList.Count() > 0)
                    userIds = string.Join(",", objList);            
            }
            return userIds;
        }
    }
}
