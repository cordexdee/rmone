using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class ProjectResourceDetail : UserControl
    {
        public string ProjectPublicID { get; set; }
        public string Module { get; set; }
        private string userResourceUrl;
        private bool isAuthorized;
        public bool ReadOnly;
        DateTime pStartDate;
        DateTime pEndDate;
        private bool individualAllocation=false;
        Ticket ticket = null;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        UserProfile currentUser = HttpContext.Current.CurrentUser();
        public string ViewMode = "PMM";

        protected override void OnInit(EventArgs e)
        {
            ticket = new Ticket(context, Module);
            userResourceUrl = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/DelegateControl.aspx?Control=CustomResourceAllocation");

            
            DataRow projectItem = Ticket.GetCurrentTicket(context, Module, ProjectPublicID);
            if ((Ticket.IsActionUser(context, projectItem, context.CurrentUser) || Ticket.IsDataEditor(projectItem, context)))
                isAuthorized = true;

            pStartDate = DateTime.MinValue;
            if (UGITUtility.IsSPItemExist(projectItem, DatabaseObjects.Columns.TicketActualStartDate) && projectItem[DatabaseObjects.Columns.TicketActualStartDate] != DBNull.Value)
                pStartDate = Convert.ToDateTime(projectItem[DatabaseObjects.Columns.TicketActualStartDate]);

            pEndDate = DateTime.MinValue;
            if (UGITUtility.IsSPItemExist(projectItem, DatabaseObjects.Columns.TicketActualCompletionDate) && projectItem[DatabaseObjects.Columns.TicketActualCompletionDate] != DBNull.Value)
                pEndDate = Convert.ToDateTime(projectItem[DatabaseObjects.Columns.TicketActualCompletionDate]);

            if (ReadOnly)
            {
                lnkSync.Visible = false;
            }
            PrepareAllocationGrid();
            BindAllocation();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["view"] != null && UGITUtility.ObjectToString(Request["view"]).Trim().ToLower() == "rmm")
            {
                ViewMode = "RMM";
            }

            CRMProjectAllocationViewNew.TicketID = ProjectPublicID;
            CRMProjectAllocationViewNew.ModuleName = Module;
            //if (allocationViewOptions.SelectedItem != null)
            //    individualAllocation = Convert.ToString(allocationViewOptions.SelectedItem.Value) == "1";
            //if (allocationViewOptions.SelectedItem.Text == "Project Allocation")
            //{
            //    gridAllocation.Visible = false;
            //    lnkSync.Visible = false;
            //    divProjectTeam.Visible = true;
            //}
            //BindAllocation();
            hdnInformation.Set("Module", Module);
            hdnInformation.Set("PublicID", ProjectPublicID);
            hdnInformation.Set("UpdateUrl", UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ProjectManagement.aspx?control=projectresourceupdate"));
            hdnInformation.Set("RequestUrl", Request.Url.AbsolutePath);

        }

        private DataTable GetAllocationData()
        {
            DataTable data = new DataTable();
            data.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
            data.Columns.Add(DatabaseObjects.Columns.ResourceId, typeof(int));
            data.Columns.Add(DatabaseObjects.Columns.Resource, typeof(string));
            data.Columns.Add("ResourceLink", typeof(string));
            data.Columns.Add("UserDetail", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.UserSkillMultiLookup, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.UserRole, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.StartDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.EndDate, typeof(DateTime));
            data.Columns.Add("ProjectAllocation", typeof(string));
            data.Columns.Add("FullAllocation", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.PctPlannedAllocation, typeof(string));

            ResourceAllocationManager objAllocationManager = new ResourceAllocationManager(context);
            ProjectAllocationManager pManager = new ProjectAllocationManager(context);
            UserProfileManager userProfileManager = new UserProfileManager(context);

            int workingHrs = uHelper.GetWorkingHoursInADay(context);
            string level1Type = Constants.RMMLevel1PMMProjectsType;

            if (Module == "TSK")
            {
                level1Type = Constants.RMMLevel1TSKProjectsType;
            }
            else if (Module == "NPR")
            {
                level1Type = Constants.RMMLevel1NPRProjectsType;
            }
            
            List<RResourceAllocation> resAlloctions = objAllocationManager.LoadByWorkItem(level1Type, ProjectPublicID, null, 4, false, true);

            if (resAlloctions == null || resAlloctions.Count <= 0)
                return data;

            if (individualAllocation)
            {
                UserProfile userProfile = null;
                string userid = Request["userid"];

                var collection = pManager.GetItems(ProjectPublicID, context.TenantID, userid);
                foreach (var item in collection)
                {
                    string userId = Request["userid"];
                    List<UsersEmail> userInfoList = new List<UsersEmail>();

                    DateTime startDate = UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.AllocationStartDate]);
                    DateTime endDate = UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.AllocationEndDate]);

                    //double callcWorkingDays = uHelper.GetTotalWorkingDaysBetween(startDate, endDate);
                    
                    var userLookup = Convert.ToString(item[DatabaseObjects.Columns.Resource]);
                    var userInfo = userProfileManager.GetUserInfoById(userId);
                                                                               //need convertion to .net
                                                                               // userInfoList = userInfoList.AddRange(userInfo);
                                                                               // userProfileManager.AddUsersFromFieldUserValue(ref userInfoList, userLookup,true, false);
                                                                               //UserProfile.AddUsersFromFieldUserValue(ref userInfo, userLookup, SPContext.Current.Web, true, false);
                                                                               // userProfileManager.GetUserInfo();
                    DataRow newRow = data.NewRow();
                    newRow[DatabaseObjects.Columns.Id] = item[DatabaseObjects.Columns.Id];

                    if (userLookup != null)
                    {
                        newRow[DatabaseObjects.Columns.Resource] = userLookup;
                        userProfile = userProfileManager.LoadById(userLookup);
                    }

                    //Get user infomations from user profile
                    if (userProfile != null)
                    {
                        // newRow[DatabaseObjects.Columns.UserRole] = userProfile.RoleName;
                        newRow[DatabaseObjects.Columns.UserRole] = userProfile.UserRoleId;
                        //  newRow[DatabaseObjects.Columns.UserSkillMultiLookup] = string.Join("; ", userProfile.Skills.Select(x => x.Value).ToArray());
                        ////newRow[DatabaseObjects.Columns.UserSkillMultiLookup] = string.Join("; ", userProfile.Skills.ToArray()); commented
                        newRow[DatabaseObjects.Columns.UserSkillMultiLookup] = userProfile.Skills;
                    }

                    string userLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/ControlTemplates/RMM/userinfo.aspx?uID={0}", userLookup));
                    string userName = Convert.ToString(newRow[DatabaseObjects.Columns.Resource]);

                    if (userLookup == null)
                        newRow["ResourceLink"] = Server.HtmlEncode("<Unassigned>");
                    else
                        newRow["ResourceLink"] = string.Format("<a title='{3}' class='jqtooltip' href='javascript:window.parent.UgitOpenPopupDialog(\"{0}\",\"\", \"User Details: {1}\", \"615px\",\"90\", false,\"{2}\")'>{4}</a>",
                                                                userLinkUrl, userProfile.Name.Replace("'", string.Empty), Server.UrlEncode(Request.Url.ToString()), userProfile.Name.Replace("'", string.Empty), userProfile.Name);

                    // newRow["ProjectAllocation"] = uHelper.CreateAllocationBar(uHelper.StringToDouble(item[DatabaseObjects.Columns.PctAllocation]));
                    newRow["ProjectAllocation"] = CreateAllocationBar(UGITUtility.StringToDouble(item[DatabaseObjects.Columns.PctAllocation]));

                    if (userLookup != null)
                    {
                        string userID = userLookup;
                        double totalPctAllocation = objAllocationManager.AllocationPercentage(userLookup, startDate.Date, endDate.Date);
                        // Current user is authorized to view fullallocation in following cases
                        // 1. row user is loggedin user, he can see his own full allocation.
                        // 2. row user manager is loggedin, he can see his subordinate full allocation
                        // 3. Logged in user is RMM Admin or Super Admin
                        if (userID == context.CurrentUser.Id || (userProfile != null && userProfile.Manager1 != null && userProfile.ManagerID == context.CurrentUser.Id) || userProfileManager.IsUGITSuperAdmin(userProfile) || userProfileManager.IsResourceAdmin(userProfile))
                        {
                            newRow["FullAllocation"] = string.Format("<a title='Click to see full allocation' href='javascript:window.parent.UgitOpenPopupDialog(\"{0}\",\"userid={3}&module={4}&projectid={5}\", \"Total Allocation: {1}\", \"70\",\"80\", true)'>{2}</a>", userResourceUrl, userLookup, CreateAllocationBar(totalPctAllocation), userID, Module, ProjectPublicID);
                            
                        }
                        
                        else
                        {
                            newRow["FullAllocation"] = CreateAllocationBar(totalPctAllocation);
                        }
                    }

                    newRow[DatabaseObjects.Columns.PctPlannedAllocation] = 0;
                    newRow[DatabaseObjects.Columns.StartDate] = UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.AllocationStartDate]);
                    newRow[DatabaseObjects.Columns.EndDate] = UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.AllocationEndDate]);
                    data.Rows.Add(newRow);
                }
            }
            else
            {
                var rLookup = resAlloctions.ToLookup(x => x.ID); 
                List<RResourceAllocation> allocations = new List<RResourceAllocation>();

                foreach (var r in rLookup)
                {
                    //double userWorkingHours = 0;
                    //foreach (var s in r)
                    //{
                    //    userWorkingHours += (uHelper.GetTotalWorkingDaysBetween(s.StartDate, s.EndDate) * workingHrs * Convert.ToDouble(s.PctAllocation)) / 100;
                    //}

                    RResourceAllocation ra = r.First();
                    ra.AllocationStartDate = r.Min(x => x.AllocationStartDate);
                    ra.AllocationEndDate = r.Max(x => x.AllocationEndDate);
                    //double totalWorkingHours = uHelper.GetTotalWorkingDaysBetween(ra.StartDate, ra.EndDate) * workingHrs;
                    //ra.PctAllocation = 0;
                    //if (totalWorkingHours > 0)
                    //    ra.PctAllocation = uHelper.StringToInt(Math.Round((userWorkingHours / totalWorkingHours * 100), 0));

                    ra.PctPlannedAllocation = r.Sum(x => x.PctPlannedAllocation); // PlannedPctAllocation=PctPlannedAllocation

                    allocations.Add(ra);
                }
                resAlloctions = allocations;

                UserProfile userProfile = null;
                DataTable spModuleList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                foreach (RResourceAllocation rAllocation in resAlloctions)
                {
                    string userId = rAllocation.Resource;
                    double callcWorkingDays = 0;
                    if (rAllocation.AllocationStartDate!=null && rAllocation.AllocationEndDate!= null)
                    {
                        callcWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, (DateTime)rAllocation.AllocationStartDate, (DateTime)rAllocation.AllocationEndDate);
                    }
                    
                    // need convertion to .net
                    //UserProfile.UsersInfo userInfo = new UserProfile.UsersInfo();
                    //SPFieldUserValue userLookup = new SPFieldUserValue(SPContext.Current.Web, string.Format("{0}{1}{2}", rAllocation.ResourceId, Constants.Separator, rAllocation.ResourceName));
                    //UserProfile.AddUsersFromFieldUserValue(ref userInfo, userLookup, SPContext.Current.Web, true, false);

                    var userInfo = userProfileManager.GetUserInfoById(userId);
                    DataRow newRow = data.NewRow();
                    newRow[DatabaseObjects.Columns.Id] = rAllocation.ID; // AllocationId=?
                    newRow[DatabaseObjects.Columns.ResourceId] = UGITUtility.StringToInt(rAllocation.Resource); //ResourceId

                    newRow[DatabaseObjects.Columns.Resource] = rAllocation.Resource;//ResourceName=?

                    //Get user infomations from user profile
                    userProfile = userProfileManager.LoadById(Convert.ToString(rAllocation.Resource));//ResourceId
                    if (userProfile != null)
                    {
                        newRow[DatabaseObjects.Columns.UserRole] = userProfile.UserRoleId; 
                        newRow[DatabaseObjects.Columns.UserSkillMultiLookup] = userProfile.Skills;
                    }

                    string userLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/ControlTemplates/RMM/userinfo.aspx?uID={0}", rAllocation.Resource));//ResourceId
                    string userName = rAllocation.Resource;//ResourceName

                    if (rAllocation.ID == 0 || userId == "00000000-0000-0000-0000-000000000000") //ResourceId
                        newRow["ResourceLink"] = Server.HtmlEncode("<Unassigned>");
                    // need equal conversion
                    else
                        newRow["ResourceLink"] = string.Format("<a title='{3}' class='jqtooltip' href='javascript:window.parent.UgitOpenPopupDialog(\"{0}\",\"\", \"User Details: {1}\", \"615px\",\"90\", false,\"{2}\")'>{4}</a>",
                                                                userLinkUrl, userProfile.Name.Replace("'", string.Empty), Server.UrlEncode(Request.Url.ToString()), userProfile.Name.Replace("'", string.Empty), userProfile.Name);

                    newRow["ProjectAllocation"] = CreateAllocationBar(rAllocation.PctPlannedAllocation.Value);
                    
                    string userID = rAllocation.Resource;//ResourceId
                    double totalPctAllocation = 0;
                    if (rAllocation.AllocationStartDate != null && rAllocation.AllocationEndDate != null)
                    {
                       totalPctAllocation = objAllocationManager.AllocationPercentage(Convert.ToString(userID), (DateTime)rAllocation.AllocationStartDate, (DateTime)rAllocation.AllocationEndDate);
                    }
                        

                    // Current user is authorized to view fullallocation in following cases
                    // 1. row user is loggedin user, he can see his own full allocation.
                    // 2. row user manager is loggedin, he can see his subordinate full allocation
                    // 3. Logged in user is RMM Admin or Super Admin

                    if (userID == context.CurrentUser.Id || (userProfile != null && userProfile.Manager1 != null && userProfile.ManagerID == context.CurrentUser.Id) || userProfileManager.IsUGITSuperAdmin(userProfile) || userProfileManager.IsResourceAdmin(userProfile))
                    {

                        //newRow["FullAllocation"] = string.Format("<a style='width:100%' title='Click to see full allocation' href='javascript:window.parent.UgitOpenPopupDialog(\"{0}\",\"userid={3}&module={4}&projectid={5}\", \"Total Allocation: {1}\", \"70\",\"80\", true)'>{2}</a>", userResourceUrl, rAllocation.Resource, CreateAllocationBar(totalPctAllocation), userID, Module, ProjectPublicID);
                        newRow["FullAllocation"] = string.Format("<a style='width:100%' title='Click to see full allocation'  href='javascript:window.parent.UgitOpenPopupDialog(\"{0}\",\"ID={3}&module={4}&projectid={5}&singleselection=true&isRedirectFromCardView=true\", \"Total Allocation: {1}\", \"70\",\"80\", false)'>{2}</a>",
                                                               userResourceUrl, userProfile.Name, CreateAllocationBar(totalPctAllocation), userID, Module, ProjectPublicID);
                    }
                    else
                    {
                        newRow["FullAllocation"] = CreateAllocationBar(totalPctAllocation);
                    }

                    string workItemType = string.Empty;
                    
                    DataRow[] drModules = spModuleList.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, rAllocation.ResourceWorkItems.WorkItem));

                    if (drModules != null && drModules.Length > 0)
                    {
                        workItemType = Convert.ToString(drModules[0][DatabaseObjects.Columns.ModuleName]);
                    }

                    string userShowLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/delegatecontrol.aspx?control=resourceutilisationdetail&uID={0}&workItemType={1}&workItem={2}", rAllocation.ID, workItemType, rAllocation.ResourceWorkItems.WorkItem));
                    newRow[DatabaseObjects.Columns.PctPlannedAllocation] = string.Format("<a  href='javascript:window.parent.UgitOpenPopupDialog(\"{0}\",\"\", \"{2} Tasks Utilization: {1}%\", \"80\",\"80\", false)'>{1}%</a>",
                                                                                         userShowLinkUrl, Math.Round((double)rAllocation.PctPlannedAllocation, 1), userName.Replace("'", string.Empty));
                    // PlannedPctAllocation=
                    if (rAllocation.AllocationStartDate != null && rAllocation.AllocationEndDate != null)
                    {
                        newRow[DatabaseObjects.Columns.StartDate] = rAllocation.AllocationStartDate;
                        newRow[DatabaseObjects.Columns.EndDate] = rAllocation.AllocationEndDate;
                    }
                    data.Rows.Add(newRow);
                }
            }

            data.DefaultView.Sort = "ResourceLink ASC, StartDate ASC, EndDate ASC";
            data = data.DefaultView.ToTable();
            return data;
        }

        protected override void OnPreRender(EventArgs e)
        {
            

            gridAllocation.DataBind();
            base.OnPreRender(e);
        }

        private void PrepareAllocationGrid()
        {
            if (gridAllocation.Columns.Count <= 0)
            {

                GridViewDataTextColumn colId = new GridViewDataTextColumn();
                colId.Caption = "#";
                colId.FieldName = "UserRole";
                colId.Width = 50;
                gridAllocation.Columns.Add(colId);
                //gridAllocation.Border.BorderWidth = Unit.Point(0.5);
                //gridAllocation.Styles.Cell.Border.BorderWidth = Unit.Point();

                colId = new GridViewDataTextColumn();
                colId.FieldName = "ResourceLink";
                colId.Caption = "Resource";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.PropertiesTextEdit.EncodeHtml = false;
                gridAllocation.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.UserRole;
                colId.Caption = "Role";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                
                colId.EditItemTemplate = new ReadOnlyTemplate();
                colId.Visible = false;
                gridAllocation.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.UserSkillMultiLookup;
                colId.Caption = "Skills";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.EditItemTemplate = new ReadOnlyTemplate();
                gridAllocation.Columns.Add(colId);

                GridViewDataDateColumn colIdDate = new GridViewDataDateColumn();
                colIdDate.FieldName = DatabaseObjects.Columns.StartDate;
                colIdDate.Caption = "Planned Start Date";
                colIdDate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colIdDate.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colIdDate.PropertiesEdit.DisplayFormatString = "MMM-dd-yyy";
                colId.Width = 100;
                gridAllocation.Columns.Add(colIdDate);

                colIdDate = new GridViewDataDateColumn();
                colIdDate.FieldName = DatabaseObjects.Columns.EndDate;
                colIdDate.Caption = "Planned End Date";
                colIdDate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colIdDate.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colIdDate.PropertiesEdit.DisplayFormatString = "MMM-dd-yyy";
                colId.Width = 100;
                gridAllocation.Columns.Add(colIdDate);

                colId = new GridViewDataTextColumn();
                colId.FieldName = "ProjectAllocation";
                colId.Caption = "Task Assignment";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.PropertiesTextEdit.EncodeHtml = false;
                gridAllocation.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = "FullAllocation";
                colId.Caption = "Total Allocation";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.PropertiesTextEdit.EncodeHtml = false;
                gridAllocation.Columns.Add(colId);

                if (isAuthorized)
                {
                    //if (ReadOnly) return;
                    //GridViewCommandColumn colAction = new GridViewCommandColumn();
                    //colAction.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    //colAction.ButtonRenderMode = GridCommandButtonRenderMode.Image;
                    //colAction.Caption = " ";
                    //GridViewCommandColumnCustomButton customButton = new GridViewCommandColumnCustomButton();
                    //customButton.Image.Url = "/Content/Images/editNewIcon.png";
                    //customButton.Image.AlternateText = "Edit";
                    //customButton.Image.ToolTip = "Edit Estimated Allocation";
                    //customButton.ID = "editButton";
                    //customButton.Image.Width = 16; 
                    //colAction.CustomButtons.Add(customButton);
                    //gridAllocation.Columns.Add(colAction);
                }

            }
            //if (gridAllocation.Columns.Count <= 0)
            //{

            //    GridViewDataTextColumn colId = new GridViewDataTextColumn();
            //    colId.Caption = "#";
            //    colId.FieldName = "UserRole";
            //    colId.Width = 50;
            //    gridAllocation.Columns.Add(colId);


            //    colId = new GridViewDataTextColumn();
            //    colId.FieldName = DatabaseObjects.Columns.Resource;
            //    colId.Caption = "Resource";
            //    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //    colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colId.PropertiesTextEdit.EncodeHtml = false;
            //    gridAllocation.Columns.Add(colId);

            //    colId = new GridViewDataTextColumn();
            //    colId.FieldName = DatabaseObjects.Columns.UserRole;
            //    colId.Caption = "Role";
            //    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //    colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colId.EditItemTemplate = new ReadOnlyTemplate();
            //    colId.Visible = false;
            //    gridAllocation.Columns.Add(colId);

            //    colId = new GridViewDataTextColumn();
            //    colId.FieldName = DatabaseObjects.Columns.UserSkillMultiLookup;
            //    colId.Caption = "Skills";
            //    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //    colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colId.EditItemTemplate = new ReadOnlyTemplate();
            //    gridAllocation.Columns.Add(colId);

            //    GridViewDataDateColumn colIdDate = new GridViewDataDateColumn();
            //    colIdDate.FieldName = DatabaseObjects.Columns.StartDate;
            //    colIdDate.Caption = "Planned Start Date";
            //    colIdDate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colIdDate.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colIdDate.PropertiesEdit.DisplayFormatString = "MMM-dd-yyy";
            //    colId.Width = 100;
            //    gridAllocation.Columns.Add(colIdDate);

            //    colIdDate = new GridViewDataDateColumn();
            //    colIdDate.FieldName = DatabaseObjects.Columns.EndDate;
            //    colIdDate.Caption = "Planned End Date";
            //    colIdDate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colIdDate.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colIdDate.PropertiesEdit.DisplayFormatString = "MMM-dd-yyy";
            //    colId.Width = 100;
            //    gridAllocation.Columns.Add(colIdDate);

            //    //colId = new GridViewDataTextColumn();
            //    //colId.FieldName = DatabaseObjects.Columns.PctPlannedAllocation;
            //    //colId.Caption = "Tasks Utilization";  //Project
            //    //colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //    //colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //    //colId.PropertiesTextEdit.EncodeHtml = false;
            //    //gridAllocation.Columns.Add(colId);

            //    colId = new GridViewDataTextColumn();
            //    colId.FieldName = "ProjectAllocation";
            //    colId.Caption = "Task Assignment";
            //    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colId.PropertiesTextEdit.EncodeHtml = false;
            //    gridAllocation.Columns.Add(colId);

            //    colId = new GridViewDataTextColumn();
            //    colId.FieldName = "FullAllocation";
            //    colId.Caption = "Project Allocation";
            //    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colId.PropertiesTextEdit.EncodeHtml = false;
            //    gridAllocation.Columns.Add(colId);
            //    //Commented not to display edit button
            //    ////if (isAuthorized)
            //    ////{
            //    ////    if (ReadOnly) return;
            //    ////    GridViewCommandColumn colAction = new GridViewCommandColumn();
            //    ////    colAction.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //    ////    colAction.ButtonRenderMode = GridCommandButtonRenderMode.Image;
            //    ////    colAction.Caption = " ";
            //    ////    GridViewCommandColumnCustomButton customButton = new GridViewCommandColumnCustomButton();
            //    ////    customButton.Image.Url = "/_layouts/15/images/ugovernit/edit-icon.png";
            //    ////    customButton.Image.AlternateText = "Edit";
            //    ////    customButton.ID = "editButton";
            //    ////    colAction.CustomButtons.Add(customButton);
            //    ////    gridAllocation.Columns.Add(colAction);
            //    ////}

            //}
        }

        void BindAllocation()
        {
            gridAllocation.DataSource = GetAllocationData();
            gridAllocation.DataBind();
        }

        protected void gridAllocation_DataBinding(object sender, EventArgs e)
        {
            if (gridAllocation.DataSource == null)
            {
                gridAllocation.DataSource = GetAllocationData();
            }
        }

        protected void gridAllocation_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {

        }

        protected void gridAllocation_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {

        }

        protected void gridAllocation_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {

        }

        protected void gridAllocation_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {

        }

        protected void gridAllocation_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                e.Row.Cells[0].Text = (e.VisibleIndex + 1).ToString();
            }
        }

        protected void gridAllocation_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {

        }

        protected void lnkSync_Click(object sender, EventArgs e)
        {
            // List<UGITTask> tasks = UGITTaskHelper.LoadByProjectID(SPContext.Current.Web, Module, ProjectPublicID);

            UGITTaskManager objUGITTaskManager = new UGITTaskManager(context);
            ResourceAllocationManager objAllocationManager = new ResourceAllocationManager(context);

            List<UGITTask> tasks = objUGITTaskManager.LoadByProjectID(ProjectPublicID);
            if (tasks != null && tasks.Count > 0)
            {
                objAllocationManager.UpdateProjectPlannedAllocationByUser(tasks, Module, ProjectPublicID, true);
                objAllocationManager.DeleteProjectPlannedAllocationByUnAssignedUsers(tasks, Module, ProjectPublicID, true);
            }
            else
            {
                if (Module == "NPR")
                {
                    tasks = objAllocationManager.LoadNPRResourceList(ProjectPublicID);
                    objAllocationManager.UpdateProjectPlannedAllocationByUser(tasks, Module, ProjectPublicID, false);
                    objAllocationManager.DeleteProjectPlannedAllocationByUnAssignedUsers(tasks, Module, ProjectPublicID, true);
                }
            }
        }

        private string CreateAllocationBar(double pctAllocation)
        {
            StringBuilder bar = new StringBuilder();
            double percentage = 0;
            string progressBarClass = "progressbar";
            string empltyProgressBarClass = "emptyProgressBar";
            percentage = pctAllocation;
            if (percentage > 100)
            {
                progressBarClass = "progressbarhold";
                bar.AppendFormat("<div style='position:relative;'><strong style='position:absolute;font-size:12px;color:#FFF;left:2px;width:95%;text-align:center;top:1px; margin-top:3px;'>{2}%</strong><div class='{0}' style='float:left; width:95%;'><div class='{1}' style='float:left; width:100%;'><b>&nbsp;</b></div></div></div>", empltyProgressBarClass, progressBarClass, percentage);
            }
            else
            {
                bar.AppendFormat("<div style='position:relative;'><strong style='position:absolute;font-size:12px;left:2px;width:95%;text-align:center;top:1px; margin-top:3px;'>{2}%</strong><div class='{0}' style='float:left; width:95%;'><div class='{1}' style='float:left; width:{2}%;'><b>&nbsp;</b></div></div></div>", empltyProgressBarClass, progressBarClass, percentage);
            }

            return bar.ToString();
        }


    }
}