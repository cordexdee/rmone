using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using uGovernIT.Manager;
using DevExpress.Web.ASPxPivotGrid;
using System.Linq.Expressions;
using uGovernIT.Utility.Helpers;
using uGovernIT.Utility.Entities;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraPivotGrid.Data;
using DevExpress.Web;
using uGovernIT.Manager.Managers;
using System.Data;

namespace uGovernIT.Web
{
    public partial class ResourceProjectComplexity : System.Web.UI.UserControl
    {
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        UserProfileManager profileManager = null;
        List<UserProfile> enabledUserProfiles = null;
        protected override void OnInit(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindAndAddFieldsOnPivotGrid();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectEstimatedAllocationManager cRMProjectAllocationManager = new ProjectEstimatedAllocationManager(HttpContext.Current.GetManagerContext());
            profileManager = new UserProfileManager(context);
            enabledUserProfiles = profileManager.GetEnabledUsers();
            //BTS-22-000904: Show the Refresh button only to Admin and Resource admin. 
            bool isResourceAdmin = profileManager.IsUGITSuperAdmin(HttpContext.Current.CurrentUser()) || profileManager.IsAdmin(HttpContext.Current.CurrentUser());
            if (isResourceAdmin)
                btnRefreshProjectComplexity.Visible = true;
            else
                btnRefreshProjectComplexity.Visible = false;
            if (!IsPostBack)
            {                
                //LoadDepartment();
                LoadFunctionalArea();
                //LoadManagers();
                UserProfile currentUserProfile = profileManager.GetUserInfoById(HttpContext.Current.CurrentUser().Id); //HttpContext.Current.CurrentUser();
                if (currentUserProfile != null && !string.IsNullOrEmpty(currentUserProfile.Department))
                {
                    UGITUtility.CreateCookie(Response, "filterdeptPC", currentUserProfile.Department);

                    ddlDepartment.SetValues(currentUserProfile.Department);
                    LoadGlobalRolesOnDepartment(currentUserProfile.Department);

                    if (currentUserProfile.IsManager)
                        LoadManagers(currentUserProfile.Department, currentUserProfile.Id);
                    else if (!String.IsNullOrEmpty(currentUserProfile.ManagerID))
                        LoadManagers(currentUserProfile.Department, currentUserProfile.ManagerID);
                    else
                        LoadManagers(currentUserProfile.Department);

                    //LoadManagers(currentUserProfile.Department, "");
                }
                else
                {
                    UGITUtility.CreateCookie(Response, "filterdeptPC", string.Empty);
                    LoadGlobalRolesOnDepartment("");
                    LoadManagers();
                }
            }
            if(cRMProjectAllocationManager.ProcessState())
            {
                
                btnRefreshProjectComplexity.Text = "Complexity Refresh In Process";
                btnRefreshProjectComplexity.ClientEnabled = false;
            }
            
            BindProjectComplexity();
            pvtGrdResourceComplexity.CellTemplate = new ValueFieldTemplate();
            pvtGrdResourceComplexity.OptionsCustomization.AllowExpand = false;
        }

        protected void DdlUserGroups_Init(object sender, EventArgs e)
        {
            string hdndept = UGITUtility.ObjectToString(Request[hdnaspDepartment.UniqueID]);
            if (string.IsNullOrEmpty(hdndept))
            {
                GlobalRoleManager gRoleMgr = new GlobalRoleManager(context);
                List<GlobalRole> roles = uHelper.GetGlobalRoles(context, false); //gRoleMgr.Load(x => !x.Deleted).OrderBy(x => x.Name).ToList();
                if (roles.Count > 0)
                {
                    ddlUserGroup.DataSource = roles.OrderBy(x => x.Name);
                    ddlUserGroup.DataTextField = "Name";
                    ddlUserGroup.DataValueField = "ID";
                    ddlUserGroup.DataBind();
                }
                ddlUserGroup.Items.Insert(0, new ListItem("All Roles", "0"));
            }
            else
            {
                LoadGlobalRolesOnDepartment(hdndept);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ProjectEstimatedAllocationManager cRMProjectAllocationManager = new ProjectEstimatedAllocationManager(HttpContext.Current.GetManagerContext());
            
        }
        
        public void BindAndAddFieldsOnPivotGrid()
        {
            PivotGridField fieldUserName = new PivotGridField();
            fieldUserName.ID = "fieldUserName";
            fieldUserName.FieldName = "UserName";
            fieldUserName.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            pvtGrdResourceComplexity.Fields.Add(fieldUserName);
            

            PivotGridField fieldComplexity = new PivotGridField();
            fieldComplexity.Caption = "Complexity Level";
            fieldComplexity.ID = "fieldComplexity";
            fieldComplexity.FieldName = "ModuleNameLookup";
            fieldComplexity.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            fieldComplexity.GroupIndex = 1;
            fieldComplexity.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            fieldComplexity.Options.AllowFilter = DevExpress.Utils.DefaultBoolean.False;
            fieldComplexity.Options.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            pvtGrdResourceComplexity.Fields.Add(fieldComplexity);


            PivotGridField fieldComplexity1 = new PivotGridField();
            fieldComplexity1.Caption = "Complexity Level1";
            fieldComplexity1.ID = "fieldComplexity1";
            fieldComplexity1.FieldName = "Complexity";
            fieldComplexity1.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            fieldComplexity1.GroupIndex = 2;
            fieldComplexity1.Options.AllowFilter = DevExpress.Utils.DefaultBoolean.False;
            pvtGrdResourceComplexity.Fields.Add(fieldComplexity1);

            PivotGridField fieldCount = new PivotGridField();
            fieldCount.ID = "fieldCount";

            if (chkIncludeClosed.Checked)
                fieldCount.FieldName = "AllCount";
            else
                fieldCount.FieldName = "Count";

            fieldCount.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            pvtGrdResourceComplexity.Fields.Add(fieldCount);

            //pvtGrdResourceComplexity.OptionsView.ShowColumnTotals = true;
            pvtGrdResourceComplexity.OptionsView.ShowColumnGrandTotals = false;
            pvtGrdResourceComplexity.OptionsView.ShowRowGrandTotals = false;
            
        }

        protected void BindProjectComplexity()
        {
            string dept = UGITUtility.GetCookieValue(Request, "filterdeptPC");

            if (dept.EqualsIgnoreCase("undefined"))
                dept = string.Empty;

            Expression<Func<SummaryResourceProjectComplexity, bool>> exp = (t) => true;

            if (!string.IsNullOrEmpty(dept))
            {
                //exp = exp.And(x => x.DepartmentID == Convert.ToInt64(dept));
                List<string> lstDepartments = UGITUtility.ConvertStringToList(dept, Constants.Separator6);
                exp = exp.And(x => lstDepartments.Contains(Convert.ToString(x.DepartmentID)));
            }

            if(ddlFunctionalArea.SelectedItem != null && !string.IsNullOrEmpty(ddlFunctionalArea.Text) && ddlFunctionalArea.SelectedValue != "0")
                exp = exp.And(x => x.FunctionalAreaID == Convert.ToInt64(ddlFunctionalArea.SelectedItem.Value));

            if(ddlResourceManager.SelectedItem != null && !string.IsNullOrEmpty(ddlResourceManager.Text) && ddlResourceManager.SelectedValue != "0")
                exp = exp.And(x => x.Manager == Convert.ToString(ddlResourceManager.SelectedItem.Value));

            if (ddlUserGroup.SelectedItem != null && !string.IsNullOrEmpty(ddlUserGroup.Text) && ddlUserGroup.SelectedValue != "0")
                exp = exp.And(x => x.GroupID.Contains(ddlUserGroup.SelectedItem.Value));
            ResourceProjectComplexityManager complexityManager = new ResourceProjectComplexityManager(context);

            //add manager as a user as well to show his complexity
            if (ddlResourceManager.SelectedItem != null && !string.IsNullOrEmpty(ddlResourceManager.Text) && ddlResourceManager.SelectedValue != "0")
                exp = exp.Or(x => x.UserId == Convert.ToString(ddlResourceManager.SelectedItem.Value));

            List<SummaryResourceProjectComplexity> summaryResourceProjectComplexities = complexityManager.Load(exp);
            var projectComplexities = (from s in summaryResourceProjectComplexities
                                      join usr in enabledUserProfiles on s.UserId equals usr.Id
                                      select s).ToList();

            pvtGrdResourceComplexity.DataSource = projectComplexities;
            pvtGrdResourceComplexity.DataBind();
			LoadManagers(dept, ddlResourceManager.SelectedItem.Value);

            if (!string.IsNullOrEmpty(dept))
                LoadGlobalRolesOnDepartment(dept, ddlUserGroup.SelectedItem.Value);
        }

        protected void ddlResourceManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            UGITUtility.GetCookieValue(Request, "filterResource");
            BindProjectComplexity();
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            UGITUtility.GetCookieValue(Request, "filterDepartment");
            LoadFunctionalArea();
            BindProjectComplexity();
        }

        protected void ddlFunctionalArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            UGITUtility.GetCookieValue(Request, "filterFunctionArea");
            BindProjectComplexity();
        }

        private void LoadDepartment()
        {
            CompanyManager companyManager = new CompanyManager(HttpContext.Current.GetManagerContext());
            List<Company> companies = new List<Company>();
            companies = companyManager.Load();  // uGITCache.LoadCompanies(SPContext.Current.Web);
            DepartmentManager departmentManager = new DepartmentManager(HttpContext.Current.GetManagerContext());
            //List<Department> activeDepartments = departmentManager.Load();   // companies.First().Departments.Where(x => !x.IsDeleted).ToList();
            //ddlDepartment.DataValueField = DatabaseObjects.Columns.ID;
            //ddlDepartment.DataTextField = DatabaseObjects.Columns.Title;
            //ddlDepartment.DataSource = activeDepartments;
            //ddlDepartment.DataBind();
            //ddlDepartment.Items.Insert(0, new ListItem(Constants.AllDepartments, "0"));

        }

        private void LoadFunctionalArea()
        {
            FunctionalAreasManager functionalAreasManager = new FunctionalAreasManager(context);
            List<FunctionalArea> funcationalArealst = functionalAreasManager.LoadFunctionalAreas();     /// uGITCache.LoadFunctionalAreas(SPContext.Current.Web);

            List<FunctionalArea> filterFuncationalArealst = new List<FunctionalArea>();
            if (ddlDepartment.GetValues() != "0")
                filterFuncationalArealst = funcationalArealst.Where(x => !x.Deleted && x.DepartmentLookup != null && x.DepartmentLookup.Value == UGITUtility.StringToInt(ddlDepartment.GetValues())).ToList();
            else
                filterFuncationalArealst = funcationalArealst.Where(x => !x.Deleted).ToList();

            ddlFunctionalArea.DataValueField = DatabaseObjects.Columns.ID;
            ddlFunctionalArea.DataTextField = DatabaseObjects.Columns.Title;

            ddlFunctionalArea.DataSource = filterFuncationalArealst;
            ddlFunctionalArea.DataBind();
            ddlFunctionalArea.Items.Insert(0, new ListItem("None", "0"));
        }

        private void LoadManagers(string values = "", string selectedMgr = "")
        {
            UserProfileManager userManager = new UserProfileManager(context);
            //List<UserProfile> userManagersList = userManager.Load(x => x.IsManager == true).OrderBy(x => x.Name).ToList();  // commented to filter disabled Managers
            //List<UserProfile> userManagersList = userManager.Load(x => x.IsManager == true && x.Enabled == true).OrderBy(x => x.Name).ToList();
            List<UserProfile> userManagersList = new List<UserProfile>();
            List<string> lstDepartments = UGITUtility.ConvertStringToList(values, Constants.Separator6);

            if (values == "undefined" || string.IsNullOrEmpty(values))
                userManagersList = userManager.Load(x => x.IsManager == true && x.Enabled == true).OrderBy(x => x.Name).ToList();
            else
                userManagersList = userManager.Load(x => x.IsManager == true && x.Enabled == true && lstDepartments.Contains(x.Department)).OrderBy(x => x.Name).ToList();

            ddlResourceManager.Items.Clear();

            if (userManagersList != null && userManagersList.Count > 0)
            {
                ddlResourceManager.DataSource = userManagersList;
                ddlResourceManager.DataValueField = DatabaseObjects.Columns.Id;
                ddlResourceManager.DataTextField = DatabaseObjects.Columns.Name;
                ddlResourceManager.DataBind();
                //ddlResourceManager.Items.Insert(0, new ListItem(Constants.AllUsers, "0"));
            }
            ddlResourceManager.Items.Insert(0, new ListItem(Constants.AllUsers, "0"));

            if (!string.IsNullOrEmpty(selectedMgr) && ddlResourceManager.Items.FindByValue(selectedMgr) != null)
            {
                ddlResourceManager.Items.FindByValue(selectedMgr).Selected = true;
                //UserProfile selectedMgrProfile =  userManager.GetUserById(selectedMgr);
                //if (selectedMgrProfile != null && !string.IsNullOrEmpty(selectedMgrProfile.Department))
                //    ddlDepartment.SetValues(selectedMgrProfile.Department);
            }
        }

        protected void pvtGrdResourceComplexity_DataBinding(object sender, EventArgs e)
        {
            //BindProjectComplexity();
        }

        protected void pvtGrdResourceComplexity_CustomCellStyle(object sender, PivotCustomCellStyleEventArgs e)
        {
            if (e.ColumnValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal && e.RowValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal)
                e.CellStyle.Font.Bold = true;
            if (e.ColumnValueType == DevExpress.XtraPivotGrid.PivotGridValueType.Value && e.RowValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal)
                e.CellStyle.Font.Bold = true;
            if (e.ColumnValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal && e.RowValueType == DevExpress.XtraPivotGrid.PivotGridValueType.Value)
                e.CellStyle.Font.Bold = true;
        }

        protected void pvtGrdResourceComplexity_FieldValueDisplayText(object sender, PivotFieldDisplayTextEventArgs e)
        {
            if (e.ValueType == PivotGridValueType.Total || e.ValueType ==PivotGridValueType.GrandTotal)
            {
                if (e.IsColumn)
                {
                    e.DisplayText = "# of " + e.Value + " Projects";
                }
                else
                    e.DisplayText = "Total";
            }
            if(e.ValueType == PivotGridValueType.GrandTotal && !e.IsColumn)
            {
                e.DisplayText = string.Empty;
            }
        }

        protected void pvtGrdResourceComplexity_CustomCallback(object sender, PivotGridCustomCallbackEventArgs e)
        {
            BindProjectComplexity();
            pvtGrdResourceComplexity.ReloadData();
        }

        protected void cbpManagers_Callback(object sender, CallbackEventArgsBase e)
        {
            string parameters = UGITUtility.ObjectToString(e.Parameter);
            string[] values = UGITUtility.SplitString(parameters, Constants.Separator2);
            if (values.Count() >= 1)
            {
                LoadManagers(values[0]);
                UGITUtility.CreateCookie(Response, "filterdeptPC", values[0]);
            }
            else
            {
                UGITUtility.CreateCookie(Response, "filterdeptPC", string.Empty);
                LoadManagers();
            }
        }

        protected void cbpComplexity_Callback(object sender, CallbackEventArgsBase e)
        {
            string parameters = UGITUtility.ObjectToString(e.Parameter);
            string[] arrParams = UGITUtility.SplitString(parameters, Constants.CommentSeparator);
            if(arrParams.Count() > 0)
            {
                string[] values = UGITUtility.SplitString(arrParams[0], Constants.Separator2);
                if(values.Count() >= 1)
                {
                    LoadGlobalRolesOnDepartment(values[1]);
                    hdnaspDepartment.Value = values[1];
					UGITUtility.CreateCookie(Response, "filterdeptPC", values[1]); 
                }
            }
        }

        private void LoadGlobalRolesOnDepartment(string values, string selectedRole = "")
        {
            JobTitleManager jobTitleManager = new JobTitleManager(context);
            List<JobTitle> jobTitles = new List<JobTitle>();
            List<string> lstDepartments = UGITUtility.ConvertStringToList(values, Constants.Separator6);
            if (string.IsNullOrEmpty(values) || values == "undefined")
                jobTitles = jobTitleManager.Load();
            else
                jobTitles = jobTitleManager.Load(x => lstDepartments.Contains(UGITUtility.ObjectToString(x.DepartmentId)));
            //jobTitles = jobTitleManager.Load(x => x.DepartmentId == UGITUtility.StringToInt(values));

            List<string> jobtitleids = jobTitles.Select(x => x.RoleId).ToList();

            GlobalRoleManager roleManager = new GlobalRoleManager(context);
            List<GlobalRole> globalRoles = new List<GlobalRole>();
            globalRoles = uHelper.GetGlobalRoles(context, false).Where(x => jobtitleids.Contains(x.Id)).OrderBy(y => y.Name).ToList(); // roleManager.Load(x => jobtitleids.Contains(x.Id)).OrderBy(y => y.Name).ToList();
            if (globalRoles != null)
            {
                ddlUserGroup.DataSource = globalRoles;
                ddlUserGroup.DataTextField = "Name";
                ddlUserGroup.DataValueField = "ID";
                ddlUserGroup.DataBind();

            }
            ddlUserGroup.Items.Insert(0, new ListItem("All Roles", "0"));

            if(!string.IsNullOrEmpty(selectedRole) && ddlUserGroup.Items.FindByValue(selectedRole) != null)
                ddlUserGroup.Items.FindByValue(selectedRole).Selected = true;
        }

        protected void chkIncludeClosed_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIncludeClosed.Checked)
                UGITUtility.CreateCookie(Response, "IncludeClosedPC", "true");
            else
                UGITUtility.CreateCookie(Response, "IncludeClosedPC", "");
            BindAndAddFieldsOnPivotGrid();
            pvtGrdResourceComplexity.DataSource = null;
            pvtGrdResourceComplexity.DataBind();
        }
    }

    class ValueFieldTemplate : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            PivotGridCellTemplateContainer c = container as PivotGridCellTemplateContainer;
            if (c == null)
                return;
            
            
            if (c.Item.RowValueType == PivotGridValueType.GrandTotal || c.Item.ColumnValueType == PivotGridValueType.GrandTotal)
            {
                ASPxLabel lblgrandtotal = new ASPxLabel();
                lblgrandtotal.Text = c.Text;
                lblgrandtotal.ID = Convert.ToString(Guid.NewGuid());
                c.Controls.Add(lblgrandtotal);
            }
            else
            {
                DevExpress.Web.ASPxPivotGrid.ASPxPivotGrid pivot = (c.NamingContainer as DevExpress.Web.ASPxPivotGrid.ASPxPivotGrid);
                string userValue = Convert.ToString(c.GetFieldValue(c.RowField));
                string complexity = Convert.ToString(c.GetFieldValue(pivot.Fields["fieldComplexity1"]));
                string moduleName = Convert.ToString(c.GetFieldValue(pivot.Fields["fieldComplexity"]));
                bool IncludeClosedProjects = false;

                string closedfilter = UGITUtility.GetCookieValue(HttpContext.Current.Request, "IncludeClosedPC");
                if (closedfilter == "true")
                {
                    IncludeClosedProjects = true;
                }
                else
                {
                    IncludeClosedProjects = false;
                }

                string cellLink = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?control=CRMComplexityTickets&UserId={0}&Complexity={1}&moduleName={2}&IncludeClosedProjects={3}", userValue, complexity, moduleName, IncludeClosedProjects));

                if (!string.IsNullOrEmpty(c.Text) && !c.Text.Equals("0"))
                {
                    ASPxButton btnLink = new ASPxButton();
                    btnLink.RenderMode = ButtonRenderMode.Link;
                    btnLink.Text = c.Text;
                    btnLink.ID = Convert.ToString(Guid.NewGuid());
                    string windowTitle = $"Projects with Complexity Level {complexity}";
                    if(string.IsNullOrEmpty(complexity))
                        windowTitle = $"Projects with all Complexity Levels";
                    btnLink.ClientSideEvents.Click = "function(s, e){ window.parent.UgitOpenPopupDialog(\"" + cellLink + "\", '' , \"" + windowTitle + "\", 90, 90, 0,'') }";
                    c.Controls.Add(btnLink);
                }
            }
        }
    }

    public class MyLink : HyperLink
    {
        public MyLink(string userid, string ticketId, string count)
            : base()
        {
            Text = count;
            Style.Add("text-decoration", "underline");
            string valuesString = ticketId;
            Attributes.Add("Href", "javascript:alert('successful!')");
        }
    }
}
