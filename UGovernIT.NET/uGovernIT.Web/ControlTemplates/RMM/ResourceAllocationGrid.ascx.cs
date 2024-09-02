using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using System.Linq;
using DevExpress.Web;
using DevExPrinting = DevExpress.XtraPrinting;
using uGovernIT.Manager.Reports;
//using ReportEntity = uGovernIT.Manager.Report.Entities;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.Shape;
using System.Drawing;
using DevExpress.Web.Rendering;
using System.Globalization;
using System.Collections;
using System.Text;
using uGovernIT.Manager;
using System.Web.UI.HtmlControls;
using System.Threading;
using DevExpress.XtraGrid;
using DevExpress.Data;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager.Managers;
using System.Data.SqlClient;
using uGovernIT.DAL;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class ResourceAllocationGrid : UserControl
    {
        protected ZoomLevel zoomLevel;
        DataTable ResultData = new DataTable();
        protected bool IsCustomCallBack = false;
        public bool HideTopBar { get; set; }
        public Unit Height { get; set; }
        public Unit Width { get; set; }

        public bool ShowCurrentUserDetailsOnly { get; set; }
        public bool HideAllocationType { get; set; }

        private DateTime dateFrom;
        private DateTime dateTo;
        public bool btnexport;

        private string selectedCategory = string.Empty;
        private string selectedManager = string.Empty;
        public string SelectedUser = string.Empty;
        private string selecteddepartment = string.Empty;
        private long selectedfunctionalare = -1;
        List<string> selectedTypes = new List<string>();
        private string ControlName;
        public string absoluteUrlView = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}";
        private DataTable dataTable = null;
        ConfigurationVariableManager ConfigVariableMGR = null;
        ConfigurationVariable cvAllocationTimeLineColor;   // = ConfigurationVariable.Load("AllocationTimeLineColor");
        List<string> lstEstimateColors = null;
        List<string> lstEstimateColorsAndFontColors = null;
        List<string> lstAssignColors = null;
        List<string> lstAssignColorsAndFontColors = null;

        protected bool isResourceAdmin = false;
        //private bool allowAllocationForSelf;
        private string allowAllocationForSelf;
        private bool allowAllocationViewAll;
        private bool viewself = false;
        DataTable dtFilterTypeData;
        private string companyLogo;  // = ConfigurationVariable.GetValue(SPContext.Current.Web, ConfigConstants.CompanyLogo);

        protected List<UserProfile> userEditPermisionList = null;
        protected List<UserProfile> userProfiles = null;
        private List<int> states = null;
        DataTable allocationData = null;
        bool stopToRegerateColumns = false;
        private bool DisablePlannedAllocation;
        protected bool enableDivision;

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        private UserProfileManager ObjUserProfileManager = null;
        private UserProfile CurrentUser;
        private UserProfile user;
        private ResourceAllocationManager ResourceAllocationManager = null;
        private ResourceWorkItemsManager ResourceWorkItemsManager = null;
        ResourceProjectComplexityManager cpxManager = null;
        FieldConfigurationManager fieldConfigMgr = null;
        FieldConfiguration fieldConfig = null;
        public string AllocationGanttURL = "/layouts/ugovernit/DelegateControl.aspx?control=allocationgantt";
        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            ConfigVariableMGR = new ConfigurationVariableManager(context);
            ObjUserProfileManager = new UserProfileManager(context);
            ResourceAllocationManager = new ResourceAllocationManager(context);
            ResourceWorkItemsManager = new ResourceWorkItemsManager(context);
            cpxManager = new ResourceProjectComplexityManager(context);
            fieldConfigMgr = new FieldConfigurationManager(context);
            cvAllocationTimeLineColor = ConfigVariableMGR.LoadVaribale("AllocationTimeLineColor");
            companyLogo = ConfigVariableMGR.GetValue(ConfigConstants.ReportLogo);
            DisablePlannedAllocation = ConfigVariableMGR.GetValueAsBool(ConfigConstants.DisablePlannedAllocation);
            //CurrentUser = HttpContext.Current.CurrentUser();
            userProfiles = ObjUserProfileManager.GetUsersProfile();
            CurrentUser = userProfiles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(HttpContext.Current.CurrentUser().Id)); //HttpContext.Current.CurrentUser();
            enableDivision = ConfigVariableMGR.GetValueAsBool(ConfigConstants.EnableDivision);
            if (string.IsNullOrEmpty(Request["DateTo"]) && string.IsNullOrEmpty(Request["DateFrom"]))
            {
                string syear = Request[hndYear.UniqueID];
                if (string.IsNullOrEmpty(syear))
                {
                    hndYear.Value = DateTime.Now.Year.ToString();
                    syear = hndYear.Value;
                }
                lblSelectedYear.Text = syear;
                dateFrom = new DateTime(UGITUtility.StringToInt(syear), 1, 1);
                dateTo = dateFrom.AddMonths(12);
            }

            gvPreview.EnableRowsCache = false;
            gvPreview.SettingsBehavior.AutoExpandAllGroups = true;
            //set porperty for header,footer and company logo etc.
            if (!string.IsNullOrEmpty(companyLogo))
            {
                if (!companyLogo.Contains(HttpContext.Current.GetManagerContext().SiteUrl /* SPContext.Current.Web.Url*/))
                {
                    companyLogo = "~/" + companyLogo;
                }
                else
                {
                    companyLogo.Replace(HttpContext.Current.GetManagerContext().SiteUrl, "~/");
                }
            }

            EditPermisionList();


            if (!IsPostBack)
            {

                // set default header and footer;
                txtHeader.Text = "Allocation Timeline Report";
                txtFooter.Text = "© uGovernIT";
                chkShowCompanyLogo.Checked = true;
                chkShowDateInFooter.Checked = true;
                txtAdditionalFooterInfo.Text = "";
                txtAdditionalInfo.Text = "Report";

                LoadFunctionalArea();
                
            }

            FillDropDownTypeAndColorGrid();

            if (ObjUserProfileManager.IsUGITSuperAdmin(CurrentUser) || ObjUserProfileManager.IsResourceAdmin(CurrentUser))
            {
                btnSetTypeColor.Visible = true;
            }
            else
            {
                btnSetTypeColor.Visible = false;
            }
            if (cvAllocationTimeLineColor != null && !string.IsNullOrEmpty(cvAllocationTimeLineColor.KeyValue))
            {
                string[] color = cvAllocationTimeLineColor.KeyValue.Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries);
                if (color.Length > 1)
                {
                    lstEstimateColors = color[0].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    lstAssignColors = color[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                else
                    lstEstimateColors = color[0].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            lstEstimateColorsAndFontColors = RMMSummaryHelper.SetFontColors(lstEstimateColors);
            lstAssignColorsAndFontColors = RMMSummaryHelper.SetFontColors(lstAssignColors);
            if (ConfigVariableMGR.GetValue(ConfigConstants.AllowAllocationForSelf).EqualsIgnoreCase("View"))
                viewself = true;

            base.OnInit(e);

        }

        private void EditPermisionList()
        {
            isResourceAdmin = ObjUserProfileManager.IsUGITSuperAdmin(HttpContext.Current.CurrentUser()) || ObjUserProfileManager.IsResourceAdmin(HttpContext.Current.CurrentUser());
            //allowAllocationForSelf = ConfigVariableMGR.GetValueAsBool(ConfigConstants.AllowAllocationForSelf);
            allowAllocationForSelf = ConfigVariableMGR.GetValue(ConfigConstants.AllowAllocationForSelf);
            allowAllocationViewAll = ConfigVariableMGR.GetValueAsBool(ConfigConstants.AllowAllocationViewAll);

            if (!isResourceAdmin)
                userEditPermisionList = ObjUserProfileManager.LoadAuthorizedUsers(allowAllocationForSelf);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (glType.DataSource == null)
            {
                glType.DataSource = dtFilterTypeData;
                glType.DataBind();
            }
            
            trFilter.Visible = string.IsNullOrEmpty(Request["Type"]) || Request["Type"] != "Report";
            if (HideTopBar)
            {
                trFilter.Visible = false;
            }
            
            ControlName = uHelper.GetPostBackControlId(this.Page);
            absoluteUrlView = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, "addworkitem", "Add Allocation", "ResourceAllocation"));

            if (!string.IsNullOrEmpty(Request["SelectedCategory"]))
                selectedCategory = UGITUtility.StripHTML(Request["SelectedCategory"]);

            #region cookies

            DataTable dataTable = (DataTable)glType.DataSource;
            if (ControlName == "ddlDepartment")
            {
                UGITUtility.CreateCookie(Response, "filterDepartment", string.Format("department${0}", ddlDepartment.GetValues()));
                UGITUtility.CreateCookie(Response, "filterFunctionArea", string.Format("functionarea${0}", "0"));
            }
            //else if (ControlName == "ddlUserGroup")
            //{
            //    uHelper.CreateCookie(Response, "filter", string.Format("group${0}~#type${1}", ddlUserGroup.SelectedValue, selectedCategory));
            //}
            if (ControlName == "ddlFunctionalArea")
            {
                UGITUtility.CreateCookie(Response, "filterFunctionArea", string.Format("functionarea${0}", ddlFunctionalArea.SelectedValue));
            }
            if (ControlName == "ddlResourceManager")
            {
                UGITUtility.CreateCookie(Response, "filterResource", string.Format("user${0}", ddlResourceManager.SelectedValue));
            }

            //Get the selected categories from request.
            if (!string.IsNullOrEmpty(glType.Text))
            {
                if (dataTable == null)
                    dataTable = (DataTable)glType.DataSource;

                selectedTypes = glType.Text.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                List<string> selectModules = new List<string>();
                foreach (string sType in selectedTypes)
                {
                    DataRow row = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("LevelTitle") == sType);
                    if (row != null)
                    {
                        selectModules.Add(Convert.ToString(row[DatabaseObjects.Columns.ModuleName]));
                    }
                    else
                    {
                        selectModules.Add(sType);
                    }
                }

                selectedCategory = string.Join("#", selectModules.ToArray());

            }

            if (ControlName == "gv")
            {
                UGITUtility.CreateCookie(Response, "filter", string.Format("type${0}", selectedCategory));
            }


            if (!IsPostBack)
            {
                string filterBarPercentageFTE = UGITUtility.GetCookieValue(Request, "filterbarpercentagefte");

                if (filterBarPercentageFTE == "fte")
                    rbtnFTE.Checked = true;
                else //if (filterBarPercentageFTE == "percentage")
                    rbtnPercentage.Checked = true;
                //else
                //    rbtnBar.Checked = true;

                string closedfilter = UGITUtility.GetCookieValue(Request, "IncludeClosedAT");
                if (closedfilter == "true")
                {
                    chkIncludeClosed.Checked = true;
                }
                else
                {
                    chkIncludeClosed.Checked = false;
                }
                

                if (ControlName != "ddlDepartment" && ControlName != "ddlFunctionalArea" && ControlName != "ddlResourceManager" && ControlName != "gv")
                {
                    //UserProfile currentUserProfile = ObjUserProfileManager.LoadById(CurrentUser.Id);
                    UserProfile currentUserProfile = userProfiles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(CurrentUser.Id));

                    string afilter = UGITUtility.GetCookieValue(Request, "filter");
                    string afilterResource = UGITUtility.GetCookieValue(Request, "filterResource");
                    string afilterFunctionalArea = UGITUtility.GetCookieValue(Request, "filterFunctionArea");
                    string afilterDepartment = UGITUtility.GetCookieValue(Request, "filterDepartment");


                    if (!string.IsNullOrEmpty(afilter))
                    {
                        string[] TypeVals = afilter.Split(new string[] { "$" }, StringSplitOptions.RemoveEmptyEntries);

                        selectedTypes.Clear();
                        //List<string> sActionUrs = new List<string>();
                        if (TypeVals.Length > 1)
                        {
                            List<string> cookieselectedTypes = TypeVals[1].Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (string sau in cookieselectedTypes)
                            {
                                DataRow row = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleName) == sau);
                                if (row != null)
                                {
                                    selectedTypes.Add(Convert.ToString(row["LevelTitle"]));
                                }
                                else
                                {
                                    selectedTypes.Add(sau);
                                }
                            }
                        }

                        glType.Text = string.Join("; ", selectedTypes.ToArray());
                    }

                    if (!string.IsNullOrEmpty(afilterResource))
                    {
                        string[] Vals = afilterResource.Split(new string[] { "$" }, StringSplitOptions.RemoveEmptyEntries);
                        if (Vals.Count() > 0)
                        {
                            ddlResourceManager.SelectedIndex = ddlResourceManager.Items.IndexOf(ddlResourceManager.Items.FindByValue(Vals[1]));
                        }
                    }


                    if (!string.IsNullOrEmpty(afilterDepartment))
                    {
                        string[] Vals = afilterDepartment.Split(new string[] { "$" }, StringSplitOptions.RemoveEmptyEntries);
                        if (Vals.Count() > 0)
                        {
                            ddlDepartment.SetValues(Vals[1]);
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(ddlDepartment.GetValues()) && UGITUtility.ObjectToString(Request["ViewName"]) != "FindAvailability")
                        {
                            if (currentUserProfile != null && UGITUtility.StringToLong(currentUserProfile.Department) > 0)
                            {
                                ddlDepartment.SetValues(Convert.ToString(currentUserProfile.Department));
                                hdnaspDepartment.Value = Convert.ToString(currentUserProfile.Department);
                                
                                LoadDdlResourceManager(currentUserProfile.Department);
                                //UGITUtility.CreateCookie(Response, "filterdeptRA", Convert.ToString(currentUserProfile.Department));
                            }
                            else
                            {
                                ddlDepartment.SetText("All");
                            }
                        }
                    }

                    LoadFunctionalArea();
                    if (!string.IsNullOrEmpty(afilterFunctionalArea))
                    {
                        string[] Vals = afilterFunctionalArea.Split(new string[] { "$" }, StringSplitOptions.RemoveEmptyEntries);
                        if (Vals.Count() > 0)
                        {
                            ddlFunctionalArea.SelectedIndex = ddlFunctionalArea.Items.IndexOf(ddlFunctionalArea.Items.FindByValue(Vals[1]));

                        }
                    }

                }

            }


            gvPreview.Templates.GroupRowContent = new GridGroupRowContentTemplate(userEditPermisionList, isResourceAdmin, rbtnPercentage.Checked, chkIncludeClosed.Checked, selectedCategory);
            #endregion

            gvPreview.SettingsPager.AlwaysShowPager = false;
            gvPreview.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;

            //monthly view grid setting.
            //if (chkPercentage.Checked && hdndisplayMode.Value == "Monthly")
            if (rbtnPercentage.Checked && hdndisplayMode.Value == "Monthly")
            {
                gvPreview.SettingsDataSecurity.AllowEdit = true;
                gvPreview.SettingsEditing.Mode = GridViewEditingMode.Batch;
                gvPreview.SettingsEditing.BatchEditSettings.EditMode = GridViewBatchEditMode.Row;
                gvPreview.SettingsEditing.BatchEditSettings.StartEditAction = GridViewBatchStartEditAction.Click;
                gvPreview.SettingsEditing.BatchEditSettings.ShowConfirmOnLosingChanges = false;
            }
            else
            {
                gvPreview.SettingsDataSecurity.AllowEdit = false;
            }

            if (hdndisplayMode.Value == "Weekly")
            {
                dateFrom = new DateTime(dateFrom.Year, DateTime.Today.Month, dateFrom.Day);
                dateTo = new DateTime(dateFrom.Year, DateTime.Today.Month, DateTime.DaysInMonth(dateFrom.Year, DateTime.Today.Month));
            }

            if (!IsPostBack)
            {
                //gvPreview.DataSource = GetAllocationData();
                //allocationData = null;
                //gvPreview.ExpandAll();
                //gvPreview.DataBind();
            }

            if (Request["SelectedResource"] != null)
            {
                divFilter.Visible = false;
            }

            hdnfilterTypeLoader.Value = glType.Text;

            states = new List<int>();
            for (Int32 i = 0; i < gvPreview.VisibleRowCount; i++)
            {
                if (gvPreview.IsGroupRow(i) && gvPreview.IsRowExpanded(i))
                    states.Add(i);
            }
            UGITUtility.CreateCookie(Response, "hdnSelectedIndex", string.Join(",", states));

            if (uHelper.IsCPRModuleEnabled(context))
            {
                HideAllocationType = true;
                btnSetTypeColor.Visible = false;
                //imgNewGanttAllocation.Visible = false;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!IsPostBack)
            {
                gvPreview.DataBind();
            }

            gvPreview.ExpandAll();
            base.OnPreRender(e);
        }

        #endregion

        #region Control Events
        protected void previousYear_Click(object sender, ImageClickEventArgs e)
        {
            int sYear = DateTime.Now.Year;
            if (!string.IsNullOrEmpty(hndYear.Value))
            {
                sYear = UGITUtility.StringToInt(hndYear.Value);
                sYear = sYear - 1;
                hndYear.Value = sYear.ToString();
            }

            lblSelectedYear.Text = hndYear.Value;
            dateFrom = new DateTime(sYear, 1, 1);
            dateTo = dateFrom.AddMonths(12);
            allocationData = null;
            gvPreview.DataBind();
        }

        protected void nextYear_Click(object sender, ImageClickEventArgs e)
        {
            int sYear = DateTime.Now.Year;
            if (!string.IsNullOrEmpty(hndYear.Value))
            {
                sYear = UGITUtility.StringToInt(hndYear.Value);
                sYear = sYear + 1;
                hndYear.Value = sYear.ToString();
            }

            lblSelectedYear.Text = hndYear.Value;
            dateFrom = new DateTime(sYear, 1, 1);
            dateTo = dateFrom.AddMonths(12);

            allocationData = null;
            gvPreview.DataBind();
        }

        protected void ddlUserGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            //PrepareAllocationGrid();
            //  dtFinal = null;
            //  gvPreview.DataBind();
        }

        protected void ddlResourceManager_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFunctionalArea();
        }

        protected void ddlFunctionalArea_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void gvPreview_DataBinding(object sender, EventArgs e)
        {
            string strSectionCookie = UGITUtility.GetCookieValue(Request, "CalledFromDivisionDropdown");
            if (!(IsPostBack && !string.IsNullOrWhiteSpace(strSectionCookie) && strSectionCookie.Equals("true")))
            {
                string dept = hdnaspDepartment.Value; // UGITUtility.GetCookieValue(Request, "filterdeptRA");
                if (dept.EqualsIgnoreCase("undefined"))
                {
                    dept = string.Empty;
                }

                LoadDdlResourceManager(dept, ddlResourceManager.SelectedItem?.Value);

                if (allocationData == null)
                {
                    allocationData = GetAllocationData();

                    if (!stopToRegerateColumns)
                        PrepareAllocationGrid();
                }

                gvPreview.DataSource = allocationData;
            }
            //gvPreview.ExpandAll();
            //List<string> collapsedRows = uHelper.GetCookieValue(Request, "collapsedRows").Split(',').ToList(); ;
            //List<string> expandedRows =  uHelper.GetCookieValue(Request, "expandedRows").Split(',').ToList();

            //if (collapsedRows == null && expandedRows == null) return;

            //for (int i = 0; i < gvPreview.VisibleRowCount; i++)
            //{
            //    if (gvPreview.IsGroupRow(i))
            //    {
            //        string value = Convert.ToString(gvPreview.GetRowValues(i, DatabaseObjects.Columns.Resource));
            //        if (gvPreview.IsRowExpanded(i) && collapsedRows != null && collapsedRows.Contains(value))
            //        {
            //            gvPreview.CollapseRow(i);
            //        }
            //        else if (!gvPreview.IsRowExpanded(i) && expandedRows != null && expandedRows.Contains(value))
            //        {
            //            gvPreview.ExpandRow(i);
            //        }
            //    }
            //}
        }

        protected void gvPreview_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

            if (e.RowType == GridViewRowType.Data)
            {
                DataRow row = gvPreview.GetDataRow(e.VisibleIndex);
                string absoluteUrlEdit = string.Empty;
                string userid = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Id]);
                //int workID = UGITUtility.StringToInt( UGITUtility.SplitString(workitemid, Constants.Separator)[0]);
                if (!string.IsNullOrEmpty(Convert.ToString(row[DatabaseObjects.Columns.AllocationStartDate])) && !string.IsNullOrEmpty(Convert.ToString(row[DatabaseObjects.Columns.AllocationEndDate])))
                    absoluteUrlEdit = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/DelegateControl.aspx?control={0}&ID={1}&startDate={2}&endDate={3}&ticketId={4}&tabname=allocationtimeline&isRedirectFromCardView=true",
                        "CustomResourceAllocation", userid, UGITUtility.StringToDateTime(Convert.ToString(row[DatabaseObjects.Columns.AllocationStartDate])).ToShortDateString(), UGITUtility.StringToDateTime(Convert.ToString(row[DatabaseObjects.Columns.AllocationEndDate])).ToShortDateString(), Convert.ToString(row[DatabaseObjects.Columns.TicketId])));
                else
                    absoluteUrlEdit = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/DelegateControl.aspx?control={0}&ID={1}&startDate={2}&endDate={3}&ticketId={4}&workItemType={5}&subWorkItem={6}&monthlyAllocationEdit=false&tabname=allocationtimeline&isRedirectFromCardView=true",
                        "CustomResourceAllocation", userid, UGITUtility.StringToDateTime(Convert.ToString(row[DatabaseObjects.Columns.PlannedStartDate])).ToShortDateString(), UGITUtility.StringToDateTime(Convert.ToString(row[DatabaseObjects.Columns.PlannedEndDate])).ToShortDateString(), Convert.ToString(row[DatabaseObjects.Columns.TicketId]), Convert.ToString(row[DatabaseObjects.Columns.ModuleName]).Trim(), Convert.ToString(row[DatabaseObjects.Columns.SubWorkItem]).Trim()));
                string func = string.Format("openResourceAllocationDialog('{0}','{1}','{2}')", absoluteUrlEdit, "Resource Utilization", Server.UrlEncode(Request.Url.AbsolutePath));

                if (isResourceAdmin)
                {
                    e.Row.Cells[1].Text = string.Format("<div onmouseover='ShowEditImage(this)' onmouseout='HideEditImage(this)' >{0} {1}</div>", row["ProjectNameLink"], "<image style=\"padding-right:5px;visibility:hidden; width:22px;\" src=\"/content/images/editNewIcon.png\" onclick=\"javascript:" + func + "  \"  />");
                }
                else
                {
                    string userId = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Id]);
                    if (userEditPermisionList != null && userEditPermisionList.Count > 0 && userEditPermisionList.Exists(x => x.Id == userId))
                        e.Row.Cells[1].Text = string.Format("<div onmouseover='ShowEditImage(this)' onmouseout='HideEditImage(this)' >{0} {1}</div>", row["ProjectNameLink"], "<image style=\"padding-right:5px;visibility:hidden; width:22px;\" src=\"/content/images/editNewIcon.png\" onclick=\"javascript:  " + func + "  \"  />");
                    else if (viewself == true)
                        e.Row.Cells[1].Text = $"{row[DatabaseObjects.Columns.Project]}";
                }
            }
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            btnexport = true;
            allocationData = GetAllocationData();
            gvPreview.DataSource = allocationData;
            //gvPreview.DataBind();
            DevExpress.XtraPrinting.XlsExportOptionsEx options = new DevExpress.XtraPrinting.XlsExportOptionsEx();
            options.ExportType = DevExpress.Export.ExportType.WYSIWYG;
            options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;
            //gvPreview.ExportRenderBrick += GvPreview_ExportRenderBrick;

            gvPreview.Columns["AllocationType"].Visible = false;
            gridExporter.WriteXlsToResponse("Resource Allocation", options);
            //ReportEntity.ReportGenerationHelper reportHelper = new ReportEntity.ReportGenerationHelper();
            //reportHelper.companyLogo = companyLogo;
            //reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
            //reportHelper.CustomizeColumn += reportHelper_CustomizeColumn;
            //FillGantViewdata();

            //ReportQueryFormat qryFormat = new ReportQueryFormat();
            //qryFormat.AdditionalInfo = txtAdditionalInfo.Text;
            //qryFormat.Header = txtHeader.Text;
            //qryFormat.Footer = txtFooter.Text;
            //qryFormat.ShowCompanyLogo = chkShowCompanyLogo.Checked;
            //qryFormat.AdditionalFooterInfo = txtAdditionalFooterInfo.Text;
            //qryFormat.ShowDateInFooter = chkShowDateInFooter.Checked;

            //XtraReport report = reportHelper.GenerateReport(gvPreview, (DataTable)gvPreview.DataSource, "Resource Allocation", 8F, "xls", null, qryFormat);
            //// XtraReport report = reportHelper.GenerateReport(gvPreview, (DataTable)gvPreview.DataSource, "Resource Allocation", 8F, "xls");
            //reportHelper.WriteXlsToResponse(Response, "Allocation Timeline" + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
        }

        private void GvPreview_ExportRenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;
            if ((e.Column as GridViewDataColumn).FieldName == "UnitPrice" && e.RowType != GridViewRowType.Header)
            {
                if (Convert.ToInt32(e.TextValue) > 15)
                    e.BrickStyle.BackColor = Color.Yellow;
                else
                    e.BrickStyle.BackColor = Color.Green;
            }
        }

        protected void btnPdfExport_Click(object sender, EventArgs e)
        {
            string pageHeaderInfo;
            string deptDetailInfo;
            string deptShortInfo;

            if (ddlDepartment.dropBox.Text.Contains(",") || ddlDepartment.dropBox.Text == "<Various>")
            {
                deptShortInfo = "Various Departments";
                deptDetailInfo = RMMSummaryHelper.GetSelectedDepartmentsInfo(context, selecteddepartment, enableDivision);
            }
            else if (ddlDepartment.dropBox.Text == "All")
            {
                deptShortInfo = "All Departments";
                deptDetailInfo = ddlDepartment.dropBox.Text;
            }
            else
            {
                deptShortInfo = ddlDepartment.dropBox.Text;
                deptDetailInfo = ddlDepartment.dropBox.Text;
            }
            pageHeaderInfo = string.Format("Resource Allocation: {0}; {1} to {2}", deptShortInfo, uHelper.GetDateStringInFormat(context, dateFrom, false), uHelper.GetDateStringInFormat(context, dateTo, false));
            string reportFooterInfo = string.Format("\nSelection Criteria\n   {0}: {1}\n   {2}: {3} to {4}", "Departments", deptDetailInfo, "Date Range", uHelper.GetDateStringInFormat(context, dateFrom, false), uHelper.GetDateStringInFormat(context, dateTo, false));

            gvPreview.DataSource = allocationData;

            gvPreview.Columns["AllocationType"].Visible = false;

            gridExporter.Landscape = true;
            gridExporter.PaperKind = System.Drawing.Printing.PaperKind.A4;

            gridExporter.LeftMargin = 1;
            gridExporter.RightMargin = 1;
            gridExporter.TopMargin = -1;
            gridExporter.BottomMargin = -1;

            gridExporter.PageHeader.Font.Size = 11;
            gridExporter.PageHeader.Font.Name = "Arial";
            gridExporter.PageHeader.Center = pageHeaderInfo;

            gridExporter.PageFooter.Center = "Page [Page # of Pages #]";
            gridExporter.PageFooter.Left = "[Date Printed]";
            gridExporter.ReportFooter = reportFooterInfo;

            gridExporter.WritePdfToResponse("Resource Allocation");
        }




        protected void imgPrint_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            gvPreview.Columns["AllocationType"].Visible = false;

            switch (btn.AlternateText)
            {
                case "Excel":
                    gridExporter.WriteXlsToResponse("Resource Allocation");
                    break;
                case "Pdf":
                    gridExporter.WritePdfToResponse("Resource Allocation");
                    break;
                case "Print":
                    gridExporter.Landscape = true;
                    gridExporter.PaperKind = System.Drawing.Printing.PaperKind.A4;

                    gridExporter.LeftMargin = 1;
                    gridExporter.RightMargin = 1;
                    gridExporter.TopMargin = -1;
                    gridExporter.BottomMargin = -1;

                    gridExporter.PageHeader.Font.Size = 14;
                    gridExporter.PageHeader.Font.Name = "Arial";
                    gridExporter.PageHeader.Center = "Resource Allocation";

                    gridExporter.PageFooter.Center = "Page [Page # of Pages #]";
                    gridExporter.PageFooter.Left = "[Date Printed]";

                    DevExPrinting.PdfExportOptions option = new DevExPrinting.PdfExportOptions();
                    option.ShowPrintDialogOnOpen = true;
                    option.DocumentOptions.Title = "Resource Allocation";
                    gridExporter.WritePdfToResponse("Resource Allocation", false, option);
                    break;
                default:
                    break;
            }
        }

        //void reportHelper_CustomizeColumn(object source, ReportEntity.ControlCustomizationEventArgs e)
        //{

        //}

        void control_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        //void reportHelper_CustomizeColumnsCollection(object source, ReportEntity.ColumnsCreationEventArgs e)
        //{
        //    if (e.ColumnsInfo.Exists(c => c.ColumnCaption == "Project"))
        //    {
        //        e.ColumnsInfo.Find(c => c.ColumnCaption == "Project").ColumnWidth *= 2;
        //    }
        //}

        protected void rbtnPercentage_CheckedChanged(object sender, EventArgs e)
        {
            UGITUtility.CreateCookie(Response, "filterbarpercentagefte", "percentage");
        }

        protected void rbtnFTE_CheckedChanged(object sender, EventArgs e)
        {
            UGITUtility.CreateCookie(Response, "filterbarpercentagefte", "fte");
        }

        //protected void rbtnBar_CheckedChanged(object sender, EventArgs e)
        //{
        //    UGITUtility.CreateCookie(Response, "filterbarpercentagefte", "bar");
        //}

        #endregion

        #region Custom Methods
        protected void LoadDdlResourceManager(string values = "", string selectedMgr = "")
        {
            
            if (Request.Form?.AllKeys?.ToList()?.Any(x => x.EndsWith("ddlResourceManager")) ?? false)
            {
                selectedMgr = Request[Request.Form.AllKeys.ToList().Where(x => x.EndsWith("ddlResourceManager")).First()];
            }

            UserProfileManager userManager = new UserProfileManager(context);

            List<UserProfile> userManagersList = new List<UserProfile>();
            List<string> lstdepartments = UGITUtility.ConvertStringToList(values, Constants.Separator6);
            if (values == "undefined" || values == "0" || string.IsNullOrEmpty(values))
                userManagersList = userManager.Load(x => x.IsManager == true && x.Enabled == true).OrderBy(x => x.Name).ToList();
            else
                userManagersList = userManager.Load(x => x.IsManager == true && x.Enabled == true && lstdepartments.Contains(x.Department)).OrderBy(x => x.Name).ToList();

            ddlResourceManager.SelectedIndex = -1;
            ddlResourceManager.Items.Clear();
            //ddlResourceManager.ClearSelection();
            if (userManagersList != null && userManagersList.Count > 0)
            {
                ddlResourceManager.DataSource = userManagersList;
                ddlResourceManager.DataValueField = DatabaseObjects.Columns.Id;
                ddlResourceManager.DataTextField = DatabaseObjects.Columns.Name;
                ddlResourceManager.DataBind();
                //ddlResourceManager.Items.Insert(0, new ListItem(Constants.AllUsers, "0"));
            }
            
            ddlResourceManager.Items.Insert(0, new ListItem("All Users", "0"));
            UserProfile currentUserProfile = HttpContext.Current.CurrentUser();
            if (currentUserProfile != null && currentUserProfile.IsManager)
            {
                ddlResourceManager.SelectedIndex = ddlResourceManager.Items.IndexOf(ddlResourceManager.Items.FindByValue(currentUserProfile.Id));
                hdnaspDepartment.Value = "";
                ddlDepartment.SetValues("0");
            }
            else
            {
                ddlResourceManager.Items.FindByValue("0").Selected = true;
                ddlDepartment.SetValues(currentUserProfile.Department);
            }
            if (!string.IsNullOrEmpty(selectedMgr) && ddlResourceManager.Items.FindByValue(selectedMgr) != null)
            {
                ddlResourceManager.ClearSelection();
                ddlResourceManager.Items.FindByValue(selectedMgr).Selected = true;
                //UserProfile selectedMgrProfile = ObjUserProfileManager.GetUserById(selectedMgr);
                //if(selectedMgrProfile != null && !string.IsNullOrEmpty(selectedMgrProfile.Department))
                //    ddlDepartment.SetValues(selectedMgrProfile.Department);

            }
        }

        private void LoadDepartment()
        {
            //code commented because loading data for no use
            //CompanyManager companyManager = new CompanyManager(context);
            //List<Company> companies = new List<Company>();
            //companies = companyManager.Load();  // uGITCache.LoadCompanies(SPContext.Current.Web);
            //DepartmentManager departmentManager = new DepartmentManager(context);
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
            List<FunctionalArea> funcationalArealst = functionalAreasManager.LoadFunctionalAreas();  // uGITCache.LoadFunctionalAreas(SPContext.Current.Web);

            List<FunctionalArea> filterFuncationalArealst = new List<FunctionalArea>();
            if (ddlDepartment.GetValues() != "0")
                filterFuncationalArealst = funcationalArealst.Where(x => !x.Deleted && x.DepartmentLookup != null && x.DepartmentLookup == UGITUtility.StringToInt(ddlDepartment.GetValues())).ToList();
            else
                filterFuncationalArealst = funcationalArealst.Where(x => !x.Deleted).ToList();

            ddlFunctionalArea.DataValueField = DatabaseObjects.Columns.ID;
            ddlFunctionalArea.DataTextField = DatabaseObjects.Columns.Title;

            ddlFunctionalArea.DataSource = filterFuncationalArealst;
            ddlFunctionalArea.DataBind();
            ddlFunctionalArea.Items.Insert(0, new ListItem("None", "0"));
        }

        protected void FillDropDownTypeAndColorGrid()
        {
            DataTable dtTypeData = AllocationTypeManager.LoadLevel1(context);

            //lstColors.Add(string.Format("{0};#{1}", ID, ceColor.Value));

            //ConfigurationVariable cV = ConfigVariableMGR.LoadVaribale("AllocationTimeLineColor");

            //List<string> lstEstimateColors = null;
            //List<string> lstAssignColors = null;

            if (cvAllocationTimeLineColor != null && !string.IsNullOrEmpty(cvAllocationTimeLineColor.KeyValue))
            {
                string[] color = cvAllocationTimeLineColor.KeyValue.Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries);
                if (color.Length > 1)
                {
                    lstEstimateColors = color[0].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    lstAssignColors = color[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                else
                    lstEstimateColors = color[0].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            lstEstimateColorsAndFontColors = RMMSummaryHelper.SetFontColors(lstEstimateColors);
            if (dtTypeData != null)
            {
                for (int i = dtTypeData.Rows.Count - 1; i >= 0; i--)
                {
                    if (dtTypeData.Rows[i]["LevelTitle"] == DBNull.Value)
                    {
                        dtTypeData.Rows[i].Delete();
                    }
                }
                dtTypeData.AcceptChanges();

                if (!dtTypeData.Columns.Contains(DatabaseObjects.Columns.ModuleName))
                    dtTypeData.Columns.Add(DatabaseObjects.Columns.ModuleName, typeof(string));

                if (!dtTypeData.Columns.Contains("EstimatedColor"))
                    dtTypeData.Columns.Add("EstimatedColor", typeof(string));

                if (!dtTypeData.Columns.Contains("AssignedColor"))
                    dtTypeData.Columns.Add("AssignedColor", typeof(string));

                if (!dtTypeData.Columns.Contains("ColumnType"))
                    dtTypeData.Columns.Add("ColumnType", typeof(string));
                ModuleViewManager moduleManager = new ModuleViewManager(context);
                foreach (DataRow rowitem in dtTypeData.Rows)
                {
                    //DataRow[] drModules = uGITCache.GetDataTable(DatabaseObjects.Lists.Modules, DatabaseObjects.Columns.Title, rowitem["LevelTitle"]);

                    //DataRow[] drModules = moduleManager.GetDataTable().Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, Convert.ToString(rowitem["LevelTitle"])));
                    //DataRow[] drModules = moduleManager.GetDataTable(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, Convert.ToString(rowitem["LevelTitle"]))).Select();
                    DataRow[] drModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, string.Format("{0}='{1}' and {2}='{3}' ", DatabaseObjects.Columns.Title, Convert.ToString(rowitem["LevelTitle"]), DatabaseObjects.Columns.TenantID, context.TenantID)).Select();
                    if (drModules != null && drModules.Length > 0)
                    {
                        rowitem[DatabaseObjects.Columns.ModuleName] = Convert.ToString(drModules[0][DatabaseObjects.Columns.ModuleName]);
                        rowitem["ColumnType"] = "Module";
                        //if (Convert.ToString(drModules[0][DatabaseObjects.Columns.ModuleName]) == "CPR")
                        //{
                        //    defaultValue = Convert.ToString(rowitem["LevelTitle"]);
                        //}
                    }
                    else
                    {
                        rowitem[DatabaseObjects.Columns.ModuleName] = rowitem["LevelTitle"];
                        rowitem["ColumnType"] = "NonModule";
                    }

                    if (lstEstimateColors != null && lstEstimateColors.Count > 0)
                    {
                        string value = Convert.ToString(lstEstimateColors.FirstOrDefault(x => x.Contains(Convert.ToString(rowitem[DatabaseObjects.Columns.ModuleName]))));
                        rowitem["EstimatedColor"] = UGITUtility.SplitString(value, ";#", 1);
                    }

                    if (lstAssignColors != null && lstAssignColors.Count > 0)
                    {
                        string value = Convert.ToString(lstAssignColors.FirstOrDefault(x => x.Contains(Convert.ToString(rowitem[DatabaseObjects.Columns.ModuleName]))));
                        rowitem["AssignedColor"] = UGITUtility.SplitString(value, ";#", 1);
                    }
                }

                //DataRow dtrow = dtTypeDate.NewRow();
                //dtrow["LevelTitle"] = "ALL";
                //dtrow[DatabaseObjects.Columns.ModuleName] = "ALL";
                //dtTypeDate.Rows.InsertAt(dtrow, 0);

                grdColor.DataSource = dtTypeData;
                grdColor.DataBind();

                dtFilterTypeData = dtTypeData;
            }
        }


        private int GetDaysForDisplayMode(string dMode, DateTime dt)
        {
            int days = 30;
            switch (dMode)
            {
                case "Daily":
                    days = 1;
                    break;
                case "Weekly":
                    {
                        if (dt.ToString("ddd") == "Mon")
                            days = 7;
                        else if (dt.ToString("ddd") == "Tue")
                            days = 6;
                        else if (dt.ToString("ddd") == "Wed")
                            days = 5;
                        else if (dt.ToString("ddd") == "Thu")
                            days = 4;
                        else if (dt.ToString("ddd") == "Fri")
                            days = 3;
                        else if (dt.ToString("ddd") == "Sat")
                            days = 2;
                        else if (dt.ToString("ddd") == "Sun")
                            days = 1;

                        break;
                    }
                case "Monthly":
                    days = (uHelper.FirstDayOfMonth(dt.AddMonths(1)) - dt).Days;
                    break;
                case "Quarterly":
                    days = (uHelper.FirstDayOfMonth(dt.AddMonths(3)) - dt).Days;
                    break;
                case "HalfYearly":
                    days = (uHelper.FirstDayOfMonth(dt.AddMonths(6)) - dt).Days;
                    break;
                case "Yearly":
                    days = 365;
                    break;
                default:
                    break;
            }
            return days;
        }

        #endregion

        protected void btnSaveColor_Click(object sender, EventArgs e)
        {
            List<string> lstEstimateColors = new List<string>();
            List<string> lstAssignColors = new List<string>();
            for (int i = 0; i < grdColor.VisibleRowCount; i++)
            {
                string ID = grdColor.GetRowValues(i, grdColor.KeyFieldName) is DBNull ? string.Empty : Convert.ToString(grdColor.GetRowValues(i, grdColor.KeyFieldName));
                ASPxColorEdit ceEstimateColor = (ASPxColorEdit)grdColor.FindRowCellTemplateControl(i, grdColor.Columns["EstimatedColor"] as GridViewDataColumn, "WebEstimatedColorEditor");
                lstEstimateColors.Add(string.Format("{0};#{1}", ID, ceEstimateColor.Text));

                ASPxColorEdit ceAssignColor = (ASPxColorEdit)grdColor.FindRowCellTemplateControl(i, grdColor.Columns["AssignedColor"] as GridViewDataColumn, "WebAssignedColorEditor");
                lstAssignColors.Add(string.Format("{0};#{1}", ID, ceAssignColor.Text));
            }

            string strEstimateColor = string.Join(",", lstEstimateColors.ToArray());
            string strAssignColor = string.Join(",", lstAssignColors.ToArray());

            string strColor = strEstimateColor + Constants.Separator1 + strAssignColor;
            ConfigurationVariable cV = ConfigVariableMGR.LoadVaribale("AllocationTimeLineColor");
            if (cV != null)
            {
                cV.KeyValue = strColor;
                ConfigVariableMGR.Update(cV);
            }
            pcSetColor.ShowOnPageLoad = false;

        }

        protected string GetColor(object container)
        {
            return Convert.ToString(container);
        }

        protected void gvPreview_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.Title)
            {
                string Title = UGITUtility.ObjectToString(e.GetValue(DatabaseObjects.Columns.Title));
                bool closedTicket = UGITUtility.StringToBoolean(e.GetValue(DatabaseObjects.Columns.Closed));
                if (closedTicket)
                    e.Cell.Text = string.Format("<b style='color:#9A2A2A;'>(Closed)</b> {0}", Title);
            }
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.AllocationStartDate || e.DataColumn.FieldName == DatabaseObjects.Columns.AllocationEndDate)
            {
                if (e.DataColumn.FieldName == DatabaseObjects.Columns.AllocationStartDate)
                {
                    DateTime plndD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.PlannedStartDate));
                    DateTime estD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.AllocationStartDate));
                    string plnd = "";
                    string est = "";
                    if (plndD != DateTime.MinValue)
                    {
                        plnd = UGITUtility.GetDateStringInFormat(plndD, false);
                    }
                    if (estD != DateTime.MinValue && estD != Convert.ToDateTime("01-01-1753 00:00:00"))
                    {
                        est = UGITUtility.GetDateStringInFormat(estD, false);
                    }
                    string txt = string.Empty;
                    txt = string.Format("<div>{0}</div><div>{1}</div>", est, plnd);
                    e.Cell.Text = string.Format("<div>{0}</div>", txt);


                }
                else if (e.DataColumn.FieldName == DatabaseObjects.Columns.AllocationEndDate)
                {
                    DateTime plndD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.PlannedEndDate));
                    DateTime estD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.AllocationEndDate));
                    string plnd = "";
                    string est = "";
                    if (plndD != DateTime.MinValue)
                    {
                        plnd = UGITUtility.GetDateStringInFormat(plndD, false);
                    }
                    if (estD != DateTime.MinValue && estD != Convert.ToDateTime("01-01-1753 00:00:00"))
                    {
                        est = UGITUtility.GetDateStringInFormat(estD, false);
                    }
                    string txt = string.Empty;
                    txt = string.Format("<div>{0}</div><div>{1}</div>", est, plnd);
                    e.Cell.Text = string.Format("<div>{0}</div>", txt);
                }
            }

            if (e.DataColumn.FieldName == "AllocationType")
            {
                DateTime plndD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.PlannedStartDate));
                DateTime estD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.AllocationStartDate));
                string txt = string.Empty;
                if (plndD != DateTime.MinValue && estD != DateTime.MinValue)
                {
                    txt = string.Format("<div>Allocation </div><div>Assignment</div>");
                }
                else if (plndD != DateTime.MinValue)
                {
                    txt = string.Format("<div>Assignment </div>");
                }
                else if (estD != DateTime.MinValue)
                {
                    txt = string.Format("<div>Allocation </div>");
                }

                e.Cell.Text = string.Format("<div>{0}</div>", txt);
            }
            string foreColor = "#000000";
            string Estimatecolor = "#24b6fe";
            string Assigncolor = "#24b6fe";
            string moduleName = Convert.ToString(e.GetValue(DatabaseObjects.Columns.ModuleName));
            if (lstEstimateColorsAndFontColors != null && lstEstimateColorsAndFontColors.Count > 0)
            {
                string value = Convert.ToString(lstEstimateColorsAndFontColors.FirstOrDefault(x => x.Contains(moduleName)));
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Estimatecolor = UGITUtility.SplitString(value, Constants.Separator, 1);
                    foreColor = UGITUtility.SplitString(value, Constants.Separator, 2);
                }
            }

            if (lstAssignColorsAndFontColors != null && lstAssignColorsAndFontColors.Count > 0)
            {
                string value = Convert.ToString(lstAssignColorsAndFontColors.FirstOrDefault(x => x.Contains(moduleName)));
                Assigncolor = UGITUtility.SplitString(value, Constants.Separator, 1);
                //foreColor = UGITUtility.SplitString(value, Constants.Separator, 2);
            }
            if (e.DataColumn.FieldName != DatabaseObjects.Columns.Resource && e.DataColumn.FieldName != "AllocationType" && e.DataColumn.FieldName != DatabaseObjects.Columns.AllocationStartDate && e.DataColumn.FieldName != DatabaseObjects.Columns.AllocationEndDate && e.DataColumn.FieldName != DatabaseObjects.Columns.Title)
            {
                int defaultBarH = 12;
                if (DisablePlannedAllocation)
                    defaultBarH = 24;
                string html;
                DateTime plndD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.PlannedStartDate));
                DateTime estD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.AllocationStartDate));

                for (DateTime dt = Convert.ToDateTime(dateFrom); Convert.ToDateTime(dateTo) > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
                {
                    html = "";
                    //if (!chkPercentage.Checked)
                    if (rbtnFTE.Checked)
                    {

                        if (e.DataColumn.FieldName == dt.ToString("MMM-dd-yy") + "E")
                        {
                            string estAlloc = Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "E"));
                            string AssignAlloc;
                            try
                            {
                                AssignAlloc = Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "A") ?? string.Empty);
                            }
                            catch (Exception ex)
                            {
                                AssignAlloc = string.Empty;
                                ULog.WriteException($"An Exception Occurred in gvPreview_HtmlDataCellPrepared: " + ex);
                            }

                            if (!string.IsNullOrEmpty(estAlloc) && estAlloc != "0")
                            {
                                // html = estAlloc + "% <br>";
                                estAlloc = string.Format("{0:0.00}", Math.Round(UGITUtility.StringToDouble(estAlloc) / 100, 2)) + "<br>";
                                html = $"<div style='float:left;width:100%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;'>{estAlloc}</div>";
                            }
                            else
                            {
                                html = $" <div style='float:left;width:100%;padding-left:{defaultBarH}px;height: {defaultBarH}px;'></div>";
                            }
                            if (!DisablePlannedAllocation)
                            {
                                html = string.Empty;
                                if (!string.IsNullOrEmpty(AssignAlloc) & plndD != DateTime.MinValue)
                                {
                                    // html += AssignAlloc + "%";
                                    AssignAlloc = estAlloc + string.Format("{0:0.00}", Math.Round(UGITUtility.StringToDouble(AssignAlloc) / 100, 2));
                                }
                                if (string.IsNullOrEmpty(Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "A"))) || Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "A")) == "0")
                                {
                                    html = $"<br> <div style='float:left;width:100%;padding-left:12px;height: 12px;'>{AssignAlloc}</div>";
                                }
                                else
                                {
                                    html = $"<br> <div style='float:left;width:100%;color:{foreColor};background-color: {Assigncolor};height: 12px;'>{AssignAlloc}</div>";
                                }

                            }


                            e.Cell.Text = html;

                        }
                    }
                    else
                    {

                        if (e.DataColumn.FieldName == dt.ToString("MMM-dd-yy") + "E")
                        {
                            string estAlloc = Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "E"));
                            string AssignAlloc;
                            try
                            {
                                AssignAlloc = Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "A") ?? string.Empty);
                            }
                            catch (Exception ex)
                            {
                                AssignAlloc = string.Empty;
                                ULog.WriteException($"An Exception Occurred in gvPreview_HtmlDataCellPrepared: " + ex);
                            }

                            if (!string.IsNullOrEmpty(estAlloc) && estAlloc != "0")
                            {
                                estAlloc = estAlloc + "% <br>";
                                html = $"<div style='float:left;width:100%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;'>{estAlloc}</div>";
                            }
                            else
                            {
                                html = $" <div style='float:left;width:100%;padding-left:{defaultBarH}px;height: {defaultBarH}px;'></div>";
                            }
                            if (!DisablePlannedAllocation)
                            {
                                html = string.Empty;
                                if (!string.IsNullOrEmpty(AssignAlloc) & plndD != DateTime.MinValue)
                                {
                                    AssignAlloc += estAlloc + "%";
                                }
                                if (string.IsNullOrEmpty(Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "A"))) || Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "A")) == "0")
                                {
                                    html = $"<br> <div style='float:left;width:100%;padding-left:12px;height: 12px;'>{AssignAlloc}</div>";
                                }
                                else
                                {
                                    html = $"<br> <div style='float:left;width:100%;color:{foreColor};background-color: {Assigncolor};padding-left:12px;height: 12px;'>{AssignAlloc}</div>";
                                }

                            }
                            e.Cell.Text = html;

                        }
                    }

                }
            }

        }

        protected void grdColor_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "AssignedColor")
            {
                if (Convert.ToString(e.GetValue("ColumnType")) == "NonModule")
                {
                    e.Cell.Text = "";
                }
            }
        }

        private DataTable LoadAllocationMonthlyView()
        {
            try
            {
                string ModuleNames = "CPR,OPM,CNS,PMM,NPR";
                DataTable dtAllocationMonthWise = null;
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@TenantID", context.TenantID);
                values.Add("@ModuleNames", ModuleNames);
                values.Add("@Fromdate", Convert.ToDateTime(dateFrom));
                values.Add("@Todate", Convert.ToDateTime(dateTo));
                values.Add("@Isclosed", chkIncludeClosed.Checked);
                dtAllocationMonthWise = uGITDAL.ExecuteDataSetWithParameters("usp_GetAllocationdata", values);

                //Below code commented due to performance
                //ResourceAllocationMonthlyManager allocationMonthlyManager = new ResourceAllocationMonthlyManager(context);
                //List<ResourceAllocationMonthly> allocMonthly = new List<ResourceAllocationMonthly>();
                //if (chkIncludeClosed.Checked == true)
                //    allocMonthly = allocationMonthlyManager.Load(x => x.MonthStartDate.Value.Date >= Convert.ToDateTime(dateFrom).Date && x.MonthStartDate.Value.Date <= Convert.ToDateTime(dateTo));
                //else
                //    allocMonthly = allocationMonthlyManager.LoadOpenItems(dateFrom, dateTo);
                //dtAllocationMonthWise = UGITUtility.ToDataTable<ResourceAllocationMonthly>(allocMonthly);
                return dtAllocationMonthWise;
            }
            catch (Exception)
            { }
            return null;
        }

        private DataTable LoadAllocationWeeklySummaryView()
        {
            try
            {
                DateTime dtFrom = dateFrom;
                DateTime dtTo = dateTo;

                dtFrom = new DateTime(dateFrom.Year, DateTime.Today.Month, dateFrom.Day);
                dtTo = new DateTime(dateFrom.Year, DateTime.Today.Month, DateTime.DaysInMonth(dateFrom.Year, DateTime.Today.Month));

                string commQuery = string.Empty;
                ResourceUsageSummaryWeekWiseManager allocationWeekWiseManager = new ResourceUsageSummaryWeekWiseManager(context);
                commQuery = string.Format("{0} >= '{1}' AND {0} <= '{2}'", DatabaseObjects.Columns.WeekStartDate, Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtTo));

                DataTable dtAllocationWeekWise = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryWeekWise, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'" + "AND " + commQuery);
                
                return dtAllocationWeekWise;
            }
            catch (Exception)
            { }
            return null;
        }
        private DataTable GetAllocationData()
        {
            bool containsModules = false;

            glType.DataSource = dtFilterTypeData;
            glType.DataBind();

            //Get the selected categories from request.
            if (!string.IsNullOrEmpty(Request["SelectedCategory"]))
                selectedCategory = UGITUtility.StripHTML(Request["SelectedCategory"]);
            else if (!string.IsNullOrEmpty(glType.Text))
            {
                if (dataTable == null)
                    dataTable = (DataTable)glType.DataSource;

                selectedTypes = glType.Text.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                List<string> selectModules = new List<string>();

                foreach (string sType in selectedTypes)
                {
                    DataRow row = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("LevelTitle") == sType);
                    if (row != null)
                    {
                        selectModules.Add(Convert.ToString(row[DatabaseObjects.Columns.ModuleName]));
                        if (Convert.ToString(row["ColumnType"]) == "Module")
                        {
                            if (!containsModules)
                                containsModules = true;
                        }
                    }
                    else
                    {
                        selectModules.Add(sType);
                    }
                }

                selectedCategory = string.Join("#", selectModules.ToArray());
            }
            string dept = hdnaspDepartment.Value;
            if (dept.EqualsIgnoreCase("undefined"))
                dept = string.Empty;
            if (Request["SelectedResource"] != null && UGITUtility.ObjectToString(Request["SelectedResource"]) != string.Empty)
            {
                SelectedUser = UGITUtility.ObjectToString(Request["SelectedResource"]);
            }
            if (ShowCurrentUserDetailsOnly)
            {
                SelectedUser = CurrentUser.Id;
            }

            if (Request["selectedManager"] != null && Request["selectedManager"] != "0")
            {
                selectedManager = Convert.ToString(Request["selectedManager"]);
            }
            else if (ddlResourceManager.SelectedIndex != -1)
            {
                selectedManager = Convert.ToString(ddlResourceManager.SelectedValue);
            }
            if (!string.IsNullOrEmpty(ddlDepartment.GetValues()))
            {
                selecteddepartment = ddlDepartment.GetValues();
            }
            else
            {
                selecteddepartment = UGITUtility.ObjectToString(dept);
            }

            if (Request["selectedfunctionalarea"] != null && Request["selectedfunctionalarea"] != "0")
            {
                selectedfunctionalare = Convert.ToInt32(Request["selectedfunctionalarea"]);
            }
            else if (ddlFunctionalArea.SelectedIndex != -1)
            {
                selectedfunctionalare = Convert.ToInt32(ddlFunctionalArea.SelectedValue);
            }
            if (dateFrom == DateTime.MinValue || dateTo == DateTime.MinValue)
            {
                if (string.IsNullOrEmpty(hndYear.Value))
                {
                    lblSelectedYear.Text = DateTime.Now.Year.ToString();
                    hndYear.Value = lblSelectedYear.Text;
                }
                dateFrom = new DateTime(Convert.ToInt16(hndYear.Value), 1, 1);
                dateTo = dateFrom.AddMonths(12);
            }
            DataTable data = null;
            data = RMMSummaryHelper.GetAllocationData(context, dateFrom, dateTo, hdndisplayMode.Value, selectedCategory, selecteddepartment, SelectedUser, selectedManager,
                selectedfunctionalare, viewself, hndYear.Value, CurrentUser, chkIncludeClosed.Checked, btnexport);
            return data;
        }
        private DataTable GetAllocationData_old()
        {
            DataTable data = new DataTable();
            data.Columns.Add(DatabaseObjects.Columns.Id, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ItemOrder, typeof(int));
            data.Columns.Add(DatabaseObjects.Columns.Resource, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Name, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Project, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.AllocationStartDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.AllocationEndDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.PlannedStartDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.PlannedEndDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.PctEstimatedAllocation, typeof(int));
            data.Columns.Add(DatabaseObjects.Columns.PctPlannedAllocation, typeof(int));
            data.Columns.Add("AllocationType", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.WorkItemID, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.AllocationID, typeof(int));
            data.Columns.Add("ProjectNameLink", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Closed, typeof(bool));
            data.Columns.Add(DatabaseObjects.Columns.ModuleName, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ModuleRelativePagePath, typeof(string));
            data.Columns.Add("ExtendedDate", typeof(string));
            data.Columns.Add("ExtendedDateAssign", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.SubWorkItem, typeof(string));
            DataTable dtlist = new DataTable();
            dtlist = ResourceAllocationManager.GetDatelist(hdndisplayMode.Value, dateFrom, dateTo);
            data.Merge(dtlist);
            data.Clear();
            glType.DataSource = dtFilterTypeData;
            glType.DataBind();

            bool containsModules = false;
            //Get the selected categories from request.
            if (!string.IsNullOrEmpty(Request["SelectedCategory"]))
                selectedCategory = UGITUtility.StripHTML(Request["SelectedCategory"]);
            else if (!string.IsNullOrEmpty(glType.Text))
            {
                if (dataTable == null)
                    dataTable = (DataTable)glType.DataSource;

                selectedTypes = glType.Text.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                List<string> selectModules = new List<string>();

                foreach (string sType in selectedTypes)
                {
                    DataRow row = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("LevelTitle") == sType);
                    if (row != null)
                    {
                        selectModules.Add(Convert.ToString(row[DatabaseObjects.Columns.ModuleName]));
                        if (Convert.ToString(row["ColumnType"]) == "Module")
                        {
                            if (!containsModules)
                                containsModules = true;
                        }
                    }
                    else
                    {
                        selectModules.Add(sType);
                    }
                }

                selectedCategory = string.Join("#", selectModules.ToArray());
            }

            string dept = hdnaspDepartment.Value;

            if (dept.EqualsIgnoreCase("undefined"))
                dept = string.Empty;
            if (Request["SelectedResource"] != null && UGITUtility.ObjectToString(Request["SelectedResource"]) != string.Empty)
            {
                SelectedUser = UGITUtility.ObjectToString(Request["SelectedResource"]);
            }

            if (ShowCurrentUserDetailsOnly)
            {
                SelectedUser = CurrentUser.Id;
            }

            if (Request["selectedManager"] != null && Request["selectedManager"] != "0")
            {
                selectedManager = Convert.ToString(Request["selectedManager"]);
            }
            else if (ddlResourceManager.SelectedIndex != -1)
            {
                selectedManager = Convert.ToString(ddlResourceManager.SelectedValue);
            }

            //if (Request["selecteddepartment"] != null && Request["selecteddepartment"] != "0")
            //{
            //    selecteddepartment = UGITUtility.StringToLong(Request["selecteddepartment"]);
            //}
            if (!string.IsNullOrEmpty(ddlDepartment.GetValues()))
            {
                selecteddepartment = ddlDepartment.GetValues();
            }
            else
            {
                selecteddepartment = UGITUtility.ObjectToString(dept);
            }

            if (Request["selectedfunctionalarea"] != null && Request["selectedfunctionalarea"] != "0")
            {
                selectedfunctionalare = Convert.ToInt32(Request["selectedfunctionalarea"]);
            }
            else if (ddlFunctionalArea.SelectedIndex != -1)
            {
                selectedfunctionalare = Convert.ToInt32(ddlFunctionalArea.SelectedValue);
            }


            if (dateFrom == DateTime.MinValue || dateTo == DateTime.MinValue)
            {
                if (string.IsNullOrEmpty(hndYear.Value))
                {
                    lblSelectedYear.Text = DateTime.Now.Year.ToString();
                    hndYear.Value = lblSelectedYear.Text;
                }
                dateFrom = new DateTime(Convert.ToInt16(hndYear.Value), 1, 1);
                dateTo = dateFrom.AddMonths(12);
            }
            List<UserProfile> lstUProfile = new List<UserProfile>();

            bool limitedUsers = false;
            if (!string.IsNullOrEmpty(selectedManager) && selectedManager != "0")
            {
                lstUProfile = ObjUserProfileManager.GetUserByManager(selectedManager);

                //UserProfile newlstitem = ObjUserProfileManager.LoadById(selectedManager);
                UserProfile newlstitem = userProfiles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(selectedManager));
                lstUProfile.Add(newlstitem);
                limitedUsers = true;
            }
            else
            {
                lstUProfile = ObjUserProfileManager.GetUsersProfile();
            }

            if (!string.IsNullOrEmpty(SelectedUser))
            {
                lstUProfile.Clear();
                UserProfile user = Context.GetUserManager().GetUserById(SelectedUser);
                if (user != null)
                    lstUProfile.Add(user);
                limitedUsers = true;
            }

            DataTable dtResult = null;
            DataTable workitems = null;
            List<string> userIds = new List<string>();

            if (!isResourceAdmin && !allowAllocationViewAll)
            {

                if (userEditPermisionList != null && userEditPermisionList.Count > 0)
                {
                    userIds.AddRange(userEditPermisionList.Select(x => x.Id));

                    if (!userIds.Contains(CurrentUser.Id))
                        userIds.Add(CurrentUser.Id);

                    userIds = userIds.Distinct().ToList();
                }

                if (viewself)
                {
                    if (!userIds.Contains(CurrentUser.Id))
                        userIds.Add(CurrentUser.Id);
                }

                if (userIds != null && userIds.Count > 0)
                {
                    dtResult = ResourceAllocationManager.LoadRawTableByResource(userIds, 4, dateFrom, dateTo);
                    workitems = ResourceWorkItemsManager.LoadRawTableByResource(userIds, 1);
                }
            }
            else
            {
                //// temp code
                //limitedUsers = true;
                //lstUProfile = lstUProfile.Where(x => x.Id == "f66c66d3-55d4-47dc-8094-33804dfad0a8").ToList();
                if (limitedUsers)
                {
                    userIds = lstUProfile.Select(x => x.Id).ToList();

                    dtResult = ResourceAllocationManager.LoadRawTableByResource(userIds, 4, dateFrom, dateTo);
                    workitems = ResourceWorkItemsManager.LoadRawTableByResource(userIds, 4);

                }
                else
                {
                    dtResult = ResourceAllocationManager.LoadRawTableByResource(null, 4, dateFrom, dateTo);
                    workitems = RMMSummaryHelper.GetOpenworkitems(context, chkIncludeClosed.Checked);
                }
            }

            //filter data based on closed check
            if (!chkIncludeClosed.Checked)
            {
                // Filter by Open Tickets.
                List<string> LstClosedTicketIds = new List<string>();
                //get closed ticket instead of open ticket and then filter all except closed ticket
                DataTable dtClosedTickets = RMMSummaryHelper.GetClosedTicketIds(context);
                if (dtClosedTickets != null && dtClosedTickets.Rows.Count > 0)
                {
                    LstClosedTicketIds.AddRange(dtClosedTickets.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)));
                }
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    DataRow[] dr = dtResult.AsEnumerable().Where(x => !LstClosedTicketIds.Contains(x.Field<string>(DatabaseObjects.Columns.TicketId), StringComparer.OrdinalIgnoreCase)).ToArray();
                    if (dr != null && dr.Length > 0)
                        dtResult = dr.CopyToDataTable();
                    else
                        dtResult.Rows.Clear();
                }
            }

            DataTable dtAllocationMonthly = null;
            DataTable dtAllocationWeekly = null;

            if (hdndisplayMode.Value == "Weekly")
            {
                dtAllocationWeekly = LoadAllocationWeeklySummaryView();
            }
            else
                dtAllocationMonthly = LoadAllocationMonthlyView();

            if (dtResult == null)
                return data;

            ILookup<object, DataRow> dtAllocLookups = null;
            ILookup<object, DataRow> dtWeeklyLookups = null;
            //Grouping on AllocationMonthly datatable based on ResourceWorkItemLookup
            if (dtAllocationMonthly != null && dtAllocationMonthly.Rows.Count > 0)
                dtAllocLookups = dtAllocationMonthly.AsEnumerable().ToLookup(x => x[DatabaseObjects.Columns.ResourceWorkItemLookup]);
            //Grouping on AllocationWeekly datatable based on WorkItemID
            if (dtAllocationWeekly != null && dtAllocationWeekly.Rows.Count > 0)
                dtWeeklyLookups = dtAllocationWeekly.AsEnumerable().ToLookup(x => x[DatabaseObjects.Columns.WorkItemID]);

            Dictionary<string, DataRow> tempTicketCollection = new Dictionary<string, DataRow>();
            #region data creating
            UserProfile userDetails = null;

            TicketManager ticketManager = new TicketManager(context);
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            UGITModule module = null;
            //GetUsersProfile()

            List<string> lstSelectedDepartment = UGITUtility.ConvertStringToList(selecteddepartment, Constants.Separator6);
            List<string> departmentTempUserIds = lstUProfile.Where(a => lstSelectedDepartment.Exists(d => d == a.Department)).Select(x => x.Id).ToList();
            List<string> functionalTempUserIds = lstUProfile.Where(a => a.FunctionalArea != null && a.FunctionalArea == selectedfunctionalare).Select(x => x.Id).ToList();
            List<string> managerTempUserIds = lstUProfile.Select(x => x.Id).ToList();
            foreach (DataRow dr in dtResult.Rows)
            {
                string userid = Convert.ToString(dr[DatabaseObjects.Columns.ResourceId]);
                if (string.IsNullOrEmpty(userid))
                    continue;

                userDetails = userProfiles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(userid));

                if (userDetails == null || !userDetails.Enabled)
                    continue;

                //filter...
                if (!string.IsNullOrEmpty(selecteddepartment))
                {
                    if (departmentTempUserIds.IndexOf(userid) == -1)
                        continue;
                }

                if (selectedfunctionalare > 0)
                {
                    if (functionalTempUserIds.IndexOf(userid) == -1)
                        continue;
                }

                if (!string.IsNullOrEmpty(selectedManager))
                {
                    if (managerTempUserIds.IndexOf(userid) == -1)
                        continue;
                }

                if (string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.ResourceWorkItemLookup])))
                    continue;

                DataRow newRow = data.NewRow();
                newRow[DatabaseObjects.Columns.Id] = userid;
                newRow[DatabaseObjects.Columns.ItemOrder] = 1;
                if (btnexport)
                {
                    newRow[DatabaseObjects.Columns.Resource] = userDetails.Name;
                }
                else
                {
                    newRow[DatabaseObjects.Columns.Resource] = Convert.ToString(dr[DatabaseObjects.Columns.Resource]);
                }
                newRow[DatabaseObjects.Columns.Name] = userDetails.Name;

                DataRow drWorkItem = null;
                if (workitems != null && workitems.Rows.Count > 0)
                {
                    string workitemid = Convert.ToString(dr[DatabaseObjects.Columns.ResourceWorkItemLookup]);
                    DataRow[] filterworkitemrow = workitems.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.Id, UGITUtility.GetLookupID(Convert.ToString(dr[DatabaseObjects.Columns.ResourceWorkItemLookup]))));
                    if (filterworkitemrow != null && filterworkitemrow.Length > 0)
                        drWorkItem = filterworkitemrow[0];
                }

                if (drWorkItem != null && drWorkItem[DatabaseObjects.Columns.WorkItem] != null)
                {
                    string workItem = Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItem]);
                    string[] arrayModule = selectedCategory.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                    string moduleName = string.Empty;
                    if (UGITUtility.IsValidTicketID(workItem))
                        moduleName = uHelper.getModuleNameByTicketId(workItem);

                    if (!string.IsNullOrEmpty(moduleName))
                    {
                        if (arrayModule.Contains(moduleName) || arrayModule.Length == 0)
                        {

                            module = moduleManager.GetByName(moduleName);
                            if (module == null)
                                continue;

                            //check for active modules.
                            if (!UGITUtility.StringToBoolean(module.EnableRMMAllocation))
                                continue;
                            DataRow dataRow = null;
                            if (tempTicketCollection.ContainsKey(workItem))
                                dataRow = tempTicketCollection[workItem];
                            else
                            {
                                dataRow = ticketManager.GetByTicketID(module, workItem, viewFields: new List<string>() { DatabaseObjects.Columns.Title, DatabaseObjects.Columns.Closed });
                                if (dataRow != null)
                                {
                                    tempTicketCollection.Add(workItem, dataRow);
                                }
                            }


                            if (dataRow != null)
                            {
                                string ticketID = workItem;
                                string title = UGITUtility.TruncateWithEllipsis(Convert.ToString(dataRow[DatabaseObjects.Columns.Title]), 50);
                                if (!string.IsNullOrEmpty(ticketID))
                                {
                                    newRow[DatabaseObjects.Columns.Title] = string.Format("{0}: {1}", ticketID, title);
                                    newRow[DatabaseObjects.Columns.Project] = string.Format("{0}: {1}", ticketID, title);
                                    if (UGITUtility.StringToBoolean(dataRow["Closed"]))
                                    {
                                        newRow[DatabaseObjects.Columns.Closed] = true;
                                    }
                                }
                                else
                                {
                                    newRow[DatabaseObjects.Columns.Title] = title;// title;
                                    newRow[DatabaseObjects.Columns.Project] = title;
                                }
                                newRow[DatabaseObjects.Columns.TicketId] = workItem;
                                newRow[DatabaseObjects.Columns.WorkItemID] = UGITUtility.StringToLong(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(dr[DatabaseObjects.Columns.Id]);

                                //condition for add new column for breakup gantt chart...
                                if (data != null && data.Rows.Count > 0)
                                {
                                    string expression = $"{DatabaseObjects.Columns.TicketId}='{workItem}' AND {DatabaseObjects.Columns.Id}='{userid}'";
                                    DataRow[] row = data.Select(expression);

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.PlannedStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.PlannedStartDate])))
                                    {
                                        newRow[DatabaseObjects.Columns.PlannedStartDate] = dr[DatabaseObjects.Columns.PlannedStartDate];
                                        newRow[DatabaseObjects.Columns.PlannedEndDate] = dr[DatabaseObjects.Columns.PlannedEndDate];
                                        newRow["ExtendedDateAssign"] = dr[DatabaseObjects.Columns.PlannedStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.PlannedEndDate];

                                        newRow[DatabaseObjects.Columns.PctPlannedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctPlannedAllocation]);
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])) && Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate]) != "01-01-1753 00:00:00")
                                    {
                                        newRow[DatabaseObjects.Columns.AllocationStartDate] = dr[DatabaseObjects.Columns.EstStartDate];
                                        newRow[DatabaseObjects.Columns.AllocationEndDate] = dr[DatabaseObjects.Columns.EstEndDate];
                                        newRow["ExtendedDate"] = dr[DatabaseObjects.Columns.EstStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.EstEndDate];

                                        newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctEstimatedAllocation]);
                                    }

                                    newRow["ProjectNameLink"] = string.Format("<a href='{0}'>{1}: {2}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                                           workItem, true, title).NavigateUrl, ticketID, UGITUtility.TruncateWithEllipsis(title, 40));

                                    newRow[DatabaseObjects.Columns.ModuleName] = moduleName;
                                    newRow[DatabaseObjects.Columns.ModuleRelativePagePath] = module.DetailPageUrl;


                                    if (hdndisplayMode.Value == "Weekly" && dtWeeklyLookups != null)
                                    {
                                        DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();

                                        if (dttemp != null && dttemp.Length > 0)
                                            ViewTypeAllocation(data, newRow, dttemp);
                                    }
                                    else
                                    {
                                        if (dtAllocLookups != null)
                                        {
                                            DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                            if (dttemp != null && dttemp.Length > 0)
                                                ViewTypeAllocation(data, newRow, dttemp);
                                        }
                                    }

                                    data.Rows.Add(newRow);
                                    //}
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.PlannedStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.PlannedEndDate])))
                                    {
                                        newRow[DatabaseObjects.Columns.PlannedStartDate] = dr[DatabaseObjects.Columns.PlannedStartDate];
                                        newRow[DatabaseObjects.Columns.PlannedEndDate] = dr[DatabaseObjects.Columns.PlannedEndDate];
                                        newRow["ExtendedDateAssign"] = dr[DatabaseObjects.Columns.PlannedStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.PlannedEndDate];
                                        newRow[DatabaseObjects.Columns.PctPlannedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctPlannedAllocation]);
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])))
                                    {
                                        newRow[DatabaseObjects.Columns.AllocationStartDate] = dr[DatabaseObjects.Columns.EstStartDate];
                                        newRow[DatabaseObjects.Columns.AllocationEndDate] = dr[DatabaseObjects.Columns.EstEndDate];
                                        newRow["ExtendedDate"] = dr[DatabaseObjects.Columns.EstStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.EstEndDate];
                                        newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctEstimatedAllocation]);
                                    }

                                    newRow["ProjectNameLink"] = string.Format("<a href='{0}'>{1}: {2}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                                           workItem, true, title).NavigateUrl, ticketID, UGITUtility.TruncateWithEllipsis(title, 40));

                                    newRow[DatabaseObjects.Columns.ModuleName] = moduleName;
                                    newRow[DatabaseObjects.Columns.ModuleRelativePagePath] = module.DetailPageUrl;

                                    if (hdndisplayMode.Value == "Weekly" && dtWeeklyLookups != null)
                                    {
                                        DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                        if (dttemp != null && dttemp.Length > 0)
                                            ViewTypeAllocation(data, newRow, dttemp);
                                    }
                                    else
                                    {
                                        if (dtAllocLookups != null)
                                        {
                                            DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                            if (dttemp != null && dttemp.Length > 0)
                                            {
                                                ViewTypeAllocation(data, newRow, dttemp);
                                            }
                                        }
                                    }
                                    data.Rows.Add(newRow);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (arrayModule.Length > 0 && !arrayModule.Contains(Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType])))
                            continue;

                        if (data != null && data.Rows.Count > 0)
                        {
                            string expression = string.Format("{0}= '{1}' AND {2}='{3}' AND {4}='{5}'", DatabaseObjects.Columns.TicketId, workItem, DatabaseObjects.Columns.Id, userid, DatabaseObjects.Columns.SubWorkItem, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));
                            DataRow[] row = data.Select(expression);

                            if (row != null && row.Count() > 0)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])))
                                {
                                    row[0]["ExtendedDate"] = Convert.ToString(row[0]["ExtendedDate"]) + Constants.Separator + dr[DatabaseObjects.Columns.EstStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.EstEndDate];

                                    if (!string.IsNullOrEmpty(Convert.ToString(row[0][DatabaseObjects.Columns.AllocationStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && (Convert.ToDateTime(row[0][DatabaseObjects.Columns.AllocationStartDate]) > Convert.ToDateTime(dr[DatabaseObjects.Columns.EstStartDate])))
                                        row[0][DatabaseObjects.Columns.AllocationStartDate] = dr[DatabaseObjects.Columns.EstStartDate];

                                    if (!string.IsNullOrEmpty(Convert.ToString(row[0][DatabaseObjects.Columns.AllocationEndDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])) && (Convert.ToDateTime(row[0][DatabaseObjects.Columns.AllocationEndDate]) < Convert.ToDateTime(dr[DatabaseObjects.Columns.EstEndDate])))
                                        row[0][DatabaseObjects.Columns.AllocationEndDate] = dr[DatabaseObjects.Columns.EstEndDate];
                                }

                                if (hdndisplayMode.Value == "Weekly" && dtWeeklyLookups != null)
                                {
                                    DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                    if (dttemp != null && dttemp.Length > 0)
                                        ViewTypeAllocation(data, row[0], dttemp);
                                }
                                else
                                {
                                    if (dtAllocLookups != null)
                                    {
                                        DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();

                                        if (dttemp != null && dttemp.Length > 0)
                                        {
                                            ViewTypeAllocation(data, row[0], dttemp, false);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string title = UGITUtility.TruncateWithEllipsis(workItem, 50);
                                if (string.IsNullOrEmpty(Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem])))
                                {
                                    newRow[DatabaseObjects.Columns.Title] = string.Format("{0} > {1}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title);// title;
                                    newRow[DatabaseObjects.Columns.Project] = string.Format("{0} > {1}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title);
                                }
                                else
                                {
                                    newRow[DatabaseObjects.Columns.Title] = string.Format("{0} > {1} > {2}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));// title;
                                    newRow[DatabaseObjects.Columns.Project] = string.Format("{0} > {1} > {2}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));
                                }

                                newRow[DatabaseObjects.Columns.TicketId] = workItem;
                                newRow["ProjectNameLink"] = workItem;
                                newRow[DatabaseObjects.Columns.ModuleName] = Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]);
                                newRow[DatabaseObjects.Columns.SubWorkItem] = Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]);
                                newRow[DatabaseObjects.Columns.WorkItemID] = UGITUtility.StringToLong(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(dr[DatabaseObjects.Columns.Id]);

                                if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])))
                                {
                                    newRow[DatabaseObjects.Columns.AllocationStartDate] = dr[DatabaseObjects.Columns.EstStartDate];
                                    newRow[DatabaseObjects.Columns.AllocationEndDate] = dr[DatabaseObjects.Columns.EstEndDate];

                                    newRow["ExtendedDate"] = dr[DatabaseObjects.Columns.EstStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.EstEndDate];
                                    newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctEstimatedAllocation]);
                                }

                                if (hdndisplayMode.Value == "Weekly" && dtWeeklyLookups != null)
                                {
                                    DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                    if (dttemp != null && dttemp.Length > 0)
                                        ViewTypeAllocation(data, newRow, dttemp);
                                }
                                else
                                {
                                    if (dtAllocLookups != null)
                                    {
                                        DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();

                                        if (dttemp != null && dttemp.Length > 0)
                                        {
                                            ViewTypeAllocation(data, newRow, dttemp, false);
                                        }
                                    }
                                }
                                data.Rows.Add(newRow);
                            }
                        }
                        else
                        {
                            string title = UGITUtility.TruncateWithEllipsis(workItem, 50);
                            if (string.IsNullOrEmpty(Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem])))
                            {
                                newRow[DatabaseObjects.Columns.Title] = string.Format("{0} > {1}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title);// title;
                                newRow[DatabaseObjects.Columns.Project] = string.Format("{0} > {1}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title);
                            }
                            else
                            {
                                newRow[DatabaseObjects.Columns.Title] = string.Format("{0} > {1} > {2}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));// title;
                                newRow[DatabaseObjects.Columns.Project] = string.Format("{0} > {1} > {2}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));
                            }

                            newRow[DatabaseObjects.Columns.TicketId] = workItem;
                            newRow[DatabaseObjects.Columns.ModuleName] = Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]);
                            newRow[DatabaseObjects.Columns.SubWorkItem] = Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]);
                            newRow[DatabaseObjects.Columns.WorkItemID] = UGITUtility.StringToInt(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(dr[DatabaseObjects.Columns.Id]);
                            newRow["ProjectNameLink"] = workItem;

                            if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])))
                            {
                                newRow[DatabaseObjects.Columns.AllocationStartDate] = dr[DatabaseObjects.Columns.EstStartDate];
                                newRow[DatabaseObjects.Columns.AllocationEndDate] = dr[DatabaseObjects.Columns.EstEndDate];

                                newRow["ExtendedDate"] = dr[DatabaseObjects.Columns.EstStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.EstEndDate];
                                newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctEstimatedAllocation]);
                            }

                            if (hdndisplayMode.Value == "Weekly" && dtWeeklyLookups != null)
                            {
                                DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                if (dttemp != null && dttemp.Length > 0)
                                    ViewTypeAllocation(data, newRow, dttemp);
                            }
                            else
                            {
                                if (dtAllocLookups != null)
                                {
                                    DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                    if (dttemp != null && dttemp.Length > 0)
                                    {
                                        ViewTypeAllocation(data, newRow, dttemp, false);

                                    }
                                }
                            }

                            data.Rows.Add(newRow);
                        }
                    }
                }

            }
            #endregion
            //data.DefaultView.Sort = string.Format("{0} ASC ,{1} ASC", DatabaseObjects.Columns.Name, DatabaseObjects.Columns.Project);
            //data = data.DefaultView.ToTable();
            data.DefaultView.Sort = string.Format("{0} ASC ,{1} ASC, {2} ASC", DatabaseObjects.Columns.Name, DatabaseObjects.Columns.Closed, DatabaseObjects.Columns.Project);
            return data;
        }

        private void ViewTypeAllocation(DataTable data, DataRow newRow, DataRow[] dttemp, bool Assigned = true)
        {
            double yearquaAllocE = 0;
            double yearquaAllocA = 0;

            double halfyearquaAllocE1 = 0;
            double halfyearquaAllocE2 = 0;
            double halfyearquaAllocA1 = 0;
            double halfyearquaAllocA2 = 0;

            double quaterquaAllocE1 = 0;
            double quaterquaAllocE2 = 0;
            double quaterquaAllocE3 = 0;
            double quaterquaAllocE4 = 0;
            double quaterquaAllocA1 = 0;
            double quaterquaAllocA2 = 0;
            double quaterquaAllocA3 = 0;
            double quaterquaAllocA4 = 0;

            foreach (DataRow rowitem in dttemp)
            {
                if (hdndisplayMode.Value == "Yearly")
                {
                    if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Year == UGITUtility.StringToInt(hndYear.Value))
                    {
                        yearquaAllocE += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 12;
                        yearquaAllocA += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 12;
                    }

                    DateTime yearColumn = new DateTime(UGITUtility.StringToInt(hndYear.Value), 1, 1);
                    if (data.Columns.Contains((yearColumn).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(yearColumn).ToString("MMM-dd-yy") + "E"] = Math.Round(yearquaAllocE, 2);
                    }

                    if (Assigned)
                    {
                        if (data.Columns.Contains((yearColumn).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(yearColumn).ToString("MMM-dd-yy") + "A"] = Math.Round(yearquaAllocA, 2);
                        }
                    }
                }
                else if (hdndisplayMode.Value == "HalfYearly")
                {
                    if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Year == UGITUtility.StringToInt(hndYear.Value))
                    {
                        if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month < 7)
                        {
                            halfyearquaAllocE1 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 6;
                            halfyearquaAllocA1 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 6;
                        }

                        if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month > 6)
                        {
                            halfyearquaAllocE2 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 6;
                            halfyearquaAllocA2 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 6;
                        }
                    }

                    DateTime halfyearColumn1 = new DateTime(UGITUtility.StringToInt(hndYear.Value), 1, 1);
                    DateTime halfyearColumn2 = new DateTime(UGITUtility.StringToInt(hndYear.Value), 7, 1);
                    if (data.Columns.Contains((halfyearColumn1).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(halfyearColumn1).ToString("MMM-dd-yy") + "E"] = Math.Round(halfyearquaAllocE1, 2);
                    }

                    if (data.Columns.Contains((halfyearColumn2).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(halfyearColumn2).ToString("MMM-dd-yy") + "E"] = Math.Round(halfyearquaAllocE2, 2);
                    }

                    if (Assigned)
                    {
                        if (data.Columns.Contains((halfyearColumn1).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(halfyearColumn1).ToString("MMM-dd-yy") + "A"] = Math.Round(halfyearquaAllocA1, 2);
                        }

                        if (data.Columns.Contains((halfyearColumn2).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(halfyearColumn2).ToString("MMM-dd-yy") + "A"] = Math.Round(halfyearquaAllocA2, 2);
                        }
                    }

                }
                else if (hdndisplayMode.Value == "Quarterly")
                {
                    if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Year == UGITUtility.StringToInt(hndYear.Value))
                    {
                        if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month < 4)
                        {
                            quaterquaAllocE1 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 3;
                            quaterquaAllocA1 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 3;
                        }
                        else if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month > 3 && Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month < 7)
                        {
                            quaterquaAllocE2 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 3;
                            quaterquaAllocA2 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 3;
                        }
                        else if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month > 6 && Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month < 10)
                        {
                            quaterquaAllocE3 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 3;
                            quaterquaAllocA3 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 3;
                        }
                        else
                        {
                            quaterquaAllocE4 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 3;
                            quaterquaAllocA4 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 3;
                        }
                    }

                    DateTime quaterColumn1 = new DateTime(UGITUtility.StringToInt(hndYear.Value), 1, 1);
                    DateTime quaterColumn2 = new DateTime(UGITUtility.StringToInt(hndYear.Value), 4, 1);
                    DateTime quaterColumn3 = new DateTime(UGITUtility.StringToInt(hndYear.Value), 7, 1);
                    DateTime quaterColumn4 = new DateTime(UGITUtility.StringToInt(hndYear.Value), 10, 1);

                    if (data.Columns.Contains((quaterColumn1).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(quaterColumn1).ToString("MMM-dd-yy") + "E"] = Math.Round(quaterquaAllocE1, 2);
                    }

                    if (data.Columns.Contains((quaterColumn2).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(quaterColumn2).ToString("MMM-dd-yy") + "E"] = Math.Round(quaterquaAllocE2, 2);
                    }

                    if (data.Columns.Contains((quaterColumn3).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(quaterColumn3).ToString("MMM-dd-yy") + "E"] = Math.Round(quaterquaAllocE3, 2);
                    }

                    if (data.Columns.Contains((quaterColumn4).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(quaterColumn4).ToString("MMM-dd-yy") + "E"] = Math.Round(quaterquaAllocE4, 2);
                    }

                    if (Assigned)
                    {
                        if (data.Columns.Contains((quaterColumn1).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(quaterColumn1).ToString("MMM-dd-yy") + "A"] = Math.Round(quaterquaAllocA1, 2);
                        }

                        if (data.Columns.Contains((quaterColumn2).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(quaterColumn2).ToString("MMM-dd-yy") + "A"] = Math.Round(quaterquaAllocA2, 2);
                        }

                        if (data.Columns.Contains((quaterColumn3).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(quaterColumn3).ToString("MMM-dd-yy") + "A"] = Math.Round(quaterquaAllocA3, 2);
                        }

                        if (data.Columns.Contains((quaterColumn4).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(quaterColumn4).ToString("MMM-dd-yy") + "A"] = Math.Round(quaterquaAllocA4, 2);
                        }
                    }

                }
                else if (hdndisplayMode.Value == "Weekly")
                {
                    if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"] = Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]), 2);
                    }

                    if (Assigned)
                    {
                        if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "A"] = Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]), 2);
                        }
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        DateTime AllocationStartDate = UGITUtility.StringToDateTime(newRow[DatabaseObjects.Columns.AllocationStartDate]);
                        DateTime AllocationMonthStartDate = new DateTime(AllocationStartDate.Year, AllocationStartDate.Month, 1);
                        DateTime allocationEndDate = UGITUtility.StringToDateTime(newRow[DatabaseObjects.Columns.AllocationEndDate]);
                        if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]) >= AllocationMonthStartDate && Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]) <= allocationEndDate)
                        {
                            if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"))
                            {
                                newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"] = Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]), 2);
                            }
                        }
                    }
                    if (Assigned)
                    {
                        if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "A"] = Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]), 2);
                        }
                    }

                }
            }

        }

        private void PrepareAllocationGrid()
        {
            gvPreview.Columns.Clear();
            gvPreview.GroupSummary.Clear();
            gvPreview.TotalSummary.Clear();

            GridViewDataTextColumn colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Resource;
            colId.Caption = DatabaseObjects.Columns.Resource;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.Width = new Unit("200px");
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.HeaderStyle.Font.Bold = true;
            colId.GroupIndex = 0;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
            colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.True;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
            gvPreview.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Name;
            colId.Caption = DatabaseObjects.Columns.Name;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.True;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.Visible = false;
            //colId.SortOrder = ColumnSortOrder.Ascending;
            colId.Settings.SortMode = ColumnSortMode.Custom;
            gvPreview.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Id;
            colId.Caption = DatabaseObjects.Columns.Id;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.Width = new Unit("30px");
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.HeaderStyle.Font.Bold = true;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.True;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
            colId.Visible = false;
            gvPreview.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Title;
            //colId.Caption = DatabaseObjects.Columns.Title;
            colId.Caption = "Work Item";
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.Width = new Unit("250px");
            colId.ExportWidth = 400;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.HeaderStyle.Font.Bold = true;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
            gvPreview.Columns.Add(colId);

            gvPreview.TotalSummary.Add(SummaryItemType.Custom, DatabaseObjects.Columns.Title);

            var IsShowTotalCapicityFTE = ConfigVariableMGR.GetValueAsBool(ConfigConstants.ShowTotalCapicityFTE);
            if (IsShowTotalCapicityFTE)
            {
                ASPxSummaryItem item = new ASPxSummaryItem(DatabaseObjects.Columns.Title, SummaryItemType.Custom);
                item.Tag = "ResourceItem";

                gvPreview.TotalSummary.Add(item);
            }

            if (!HideAllocationType)
            {
                colId = new GridViewDataTextColumn();
                colId.FieldName = "AllocationType";
                colId.Caption = "Allocation Type";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
                colId.Width = new Unit("150px");
                gvPreview.Columns.Add(colId);
            }

            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.AllocationStartDate;
            colId.Caption = "Start Date";
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
            colId.Width = new Unit("100px");
            colId.ExportWidth = 90;
            colId.GroupFooterCellStyle.Font.Bold = true;
            gvPreview.Columns.Add(colId);

            CreateGridSummaryColumn(gvPreview, DatabaseObjects.Columns.AllocationStartDate);

            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.AllocationEndDate;
            colId.Caption = "End Date";
            colId.UnboundType = DevExpress.Data.UnboundColumnType.String;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
            colId.GroupFooterCellStyle.Font.Bold = true;
            colId.Width = new Unit("100px");
            colId.ExportWidth = 90;
            gvPreview.Columns.Add(colId);

            CreateGridSummaryColumn(gvPreview, DatabaseObjects.Columns.AllocationEndDate);

            GridViewBandColumn bdCol = new GridViewBandColumn();
            string currentDate = string.Empty;

            if (hdndisplayMode.Value == "Weekly")
            {
                dateFrom = new DateTime(dateFrom.Year, DateTime.Today.Month, dateFrom.Day);
                dateTo = new DateTime(dateFrom.Year, DateTime.Today.Month, DateTime.DaysInMonth(dateFrom.Year, DateTime.Today.Month));
            }

            for (DateTime dt = dateFrom; dateTo > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
            {
                if (dt.ToString("yyyy") != currentDate && !string.IsNullOrEmpty(currentDate))
                {
                    gvPreview.Columns.Add(bdCol);
                    bdCol = new GridViewBandColumn();
                }

                if (dt.ToString("yyyy") != currentDate)
                {
                    bdCol.Caption = dt.ToString("yyyy");
                    bdCol.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    bdCol.HeaderStyle.Font.Bold = true;
                    currentDate = dt.ToString("yyyy");
                }

                GridViewDataSpinEditColumn ColIdData = new GridViewDataSpinEditColumn();
                if (hdndisplayMode.Value == "Weekly")
                {
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("dd-MMM");
                }
                else if (hdndisplayMode.Value == "Quarterly")
                {
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("MMM");
                }
                else if (hdndisplayMode.Value == "HalfYearly")
                {
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("MMM");
                }
                else if (hdndisplayMode.Value == "Yearly")
                {
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("MMM");
                }
                else
                {
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("MMM");
                }
                ColIdData.CellStyle.CssClass = "timeline-td";
                ColIdData.UnboundType = DevExpress.Data.UnboundColumnType.String;
                ColIdData.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                ColIdData.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                ColIdData.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                ColIdData.HeaderStyle.Font.Bold = true;

                ColIdData.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                ColIdData.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                ColIdData.PropertiesSpinEdit.NumberType = SpinEditNumberType.Float;
                ColIdData.PropertiesSpinEdit.MinValue = 0;
                ColIdData.PropertiesSpinEdit.MaxValue = 999;
                ColIdData.PropertiesSpinEdit.SpinButtons.ShowIncrementButtons = false;

                if (hdndisplayMode.Value == "Monthly")
                {
                    ColIdData.Width = new Unit("60px");
                    ColIdData.ExportWidth = 38;
                }

                if (hdndisplayMode.Value == "Weekly")
                {
                    ColIdData.Width = new Unit("90px");
                    ColIdData.ExportWidth = 60;
                }

                CreateGridSummaryColumn(gvPreview, dt.ToString("MMM-dd-yy") + "E");

                bdCol.Columns.Add(ColIdData);


                ASPxSummaryItem itemCFTE = new ASPxSummaryItem(dt.ToString("MMM-dd-yy") + "E", SummaryItemType.Custom);
                itemCFTE.DisplayFormat = "N2";
                gvPreview.TotalSummary.Add(itemCFTE);

                if (IsShowTotalCapicityFTE)
                {
                    ASPxSummaryItem itemTFTE = new ASPxSummaryItem(dt.ToString("MMM-dd-yy") + "E", SummaryItemType.Custom);
                    itemTFTE.Tag = "TFTE";
                    itemTFTE.DisplayFormat = "N2";
                    gvPreview.TotalSummary.Add(itemTFTE);
                }



            }

            gvPreview.Columns.Add(bdCol);

        }

        private void CreateGridSummaryColumn(DevExpress.Web.ASPxGridView gvPreview, string column)
        {
            ASPxSummaryItem summary = new ASPxSummaryItem(column, DevExpress.Data.SummaryItemType.Sum);
            summary.ShowInGroupFooterColumn = column;
            summary.DisplayFormat = "{0}"; // "{0:n0}";
            gvPreview.GroupSummary.Add(summary);

            if (column == DatabaseObjects.Columns.AllocationStartDate)
            {
                ASPxSummaryItem summaryStartDate = new ASPxSummaryItem(column, DevExpress.Data.SummaryItemType.Custom);
                summaryStartDate.ShowInGroupFooterColumn = column;
                summaryStartDate.DisplayFormat = "{0:MMM-dd-yyyy}";
                gvPreview.GroupSummary.Add(summaryStartDate);
            }
            if (column == DatabaseObjects.Columns.AllocationEndDate)
            {
                ASPxSummaryItem summaryEndDate = new ASPxSummaryItem(column, DevExpress.Data.SummaryItemType.Custom);
                summaryEndDate.ShowInGroupFooterColumn = column;
                summaryEndDate.DisplayFormat = "{0:MMM-dd-yyyy}";
                gvPreview.GroupSummary.Add(summaryEndDate);
            }

        }

        protected void gvPreview_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            ResourceAllocationMonthlyManager allocationMonthlyManager = new ResourceAllocationMonthlyManager(HttpContext.Current.GetManagerContext());
            ResourceAllocationManager allocationManager = new ResourceAllocationManager(HttpContext.Current.GetManagerContext());
            ResourceWorkItemsManager workItemsManager = new ResourceWorkItemsManager(HttpContext.Current.GetManagerContext());
            ProjectEstimatedAllocationManager CRMProjectAllocationManager = new ProjectEstimatedAllocationManager(HttpContext.Current.GetManagerContext());
            ProjectEstimatedAllocation cRMProjectAllocation = new ProjectEstimatedAllocation();

            List<long> workItemsId = new List<long>();
            int workindHrsInDay = uHelper.GetWorkingHoursInADay(context);
            RResourceAllocation resAllocation = null;
            long workItemID = 0;
            long allocationID = 0;
            string[] ids = new string[0];

            foreach (var args in e.UpdateValues)
            {
                ids = (Convert.ToString(args.Keys[0])).Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                if (ids.Length != 2)
                {
                    //Log.WriteLog("gvPreview_BatchUpdate: gridview is not correctly updated");
                    continue;
                }
                workItemID = UGITUtility.StringToLong(ids[0]);
                allocationID = UGITUtility.StringToLong(ids[1]);

                //don't need go further
                if (workItemID == 0 || allocationID == 0)
                    continue;

                resAllocation = allocationManager.Get(allocationID);

                if (resAllocation.ResourceWorkItemLookup > 0)
                {
                    resAllocation.ResourceWorkItems = workItemsManager.Get(resAllocation.ResourceWorkItemLookup);
                }

                #region CPR Integration
                if (resAllocation.ResourceWorkItems != null)
                {
                    if (resAllocation.ResourceWorkItems.WorkItemType == "CPR" || resAllocation.ResourceWorkItems.WorkItemType == "OPM" || resAllocation.ResourceWorkItems.WorkItemType == "CNS")
                    {
                        string query = string.Format(" where TicketId = '{0}' AND AssignedToUser = '{1}' AND AllocationStartDate = '{2}' AND AllocationEndDate = '{3}'", resAllocation.ResourceWorkItems.WorkItem, resAllocation.Resource, resAllocation.AllocationStartDate, resAllocation.AllocationEndDate);

                        cRMProjectAllocation = CRMProjectAllocationManager.Get(query);

                    }
                }
                #endregion
                double totalWorkingHrs = 0;
                double allocatedHrs = 0;

                ////fetch monthly allocations
                List<ResourceAllocationMonthly> monthAllocDBData = allocationMonthlyManager.Load(x => x.ResourceWorkItemLookup == workItemID).ToList();

                //update or create monthly allocations
                double pctAllocation = 0;
                for (DateTime dt = dateFrom; dateTo >= dt;)
                {
                    pctAllocation = UGITUtility.StringToDouble(Convert.ToString(args.NewValues[dt.ToString("MMM-dd-yy") + "E"]));
                    ResourceAllocationMonthly dRow = null;
                    if (monthAllocDBData != null && monthAllocDBData.Count > 0)
                    {
                        dRow = monthAllocDBData.FirstOrDefault(x => x.MonthStartDate == dt.Date);  // monthAllocDBData.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate).Date == dt.Date);

                        //If allocation is save then don't need to update it
                        if (dRow != null && pctAllocation == UGITUtility.StringToDouble(dRow.PctAllocation))
                        {
                            dt = dt.AddMonths(1);
                            continue;
                        }
                    }

                    if (dRow == null)
                    {
                        //don't add new entry if pct allocation is zero against it.
                        if (pctAllocation == 0)
                        {
                            dt = dt.AddMonths(1);
                            continue;
                        }

                        dRow = new ResourceAllocationMonthly();
                        dRow.MonthStartDate = dt;
                        dRow.ResourceWorkItemLookup = workItemID;
                        dRow.Resource = resAllocation.Resource;    //.ResourceId.ToString();
                        dRow.PctAllocation = pctAllocation;
                        allocationMonthlyManager.Save(dRow);
                        monthAllocDBData.Add(dRow);
                    }
                    else
                    {
                        dRow.PctAllocation = pctAllocation;
                        allocationMonthlyManager.Save(dRow);
                        dt = dt.AddMonths(1);
                    }
                }

                #region Update Allocation based on monthly entries
                //Calculate total working hours and actual hours allocation based on month wise data
                pctAllocation = 0;
                foreach (ResourceAllocationMonthly sRow in monthAllocDBData)
                {
                    pctAllocation = UGITUtility.StringToDouble(sRow.PctAllocation);
                    if (pctAllocation == 0)
                        continue;

                    DateTime sDate = UGITUtility.StringToDateTime(sRow.MonthStartDate);
                    DateTime eDate = new DateTime(sDate.Year, sDate.Month, DateTime.DaysInMonth(sDate.Year, sDate.Month));
                    int workingDaysInMonth = uHelper.GetTotalWorkingDaysBetween(context, sDate.Date, eDate.Date);
                    int totalHrsInMonth = workindHrsInDay * workingDaysInMonth;
                    totalWorkingHrs += totalHrsInMonth;
                    allocatedHrs += (pctAllocation * totalHrsInMonth) / 100;
                }


                List<ResourceAllocationMonthly> dRows = monthAllocDBData.Where(x => x.PctAllocation != null && x.PctAllocation > 0).ToList();
                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MinValue;
                if (dRows.Count() > 0)
                {
                    startDate = dRows.Min(x => x.MonthStartDate).GetValueOrDefault();
                    endDate = dRows.Max(x => x.MonthStartDate).GetValueOrDefault();
                }

                //keep enddate and startdate equal to year start date if allocation don't have any value in months.
                if (startDate == DateTime.MinValue && endDate == DateTime.MinValue)
                {
                    startDate = dateFrom; endDate = dateFrom;
                }
                endDate = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month));


                if (resAllocation.AllocationStartDate.Value.Year == startDate.Year && resAllocation.AllocationStartDate.Value.Month == startDate.Month)
                    startDate = resAllocation.AllocationStartDate.Value;
                if (resAllocation.AllocationEndDate.Value.Year == endDate.Year && resAllocation.AllocationEndDate.Value.Month == endDate.Month)
                    endDate = resAllocation.AllocationEndDate.Value;

                resAllocation.AllocationStartDate = startDate;
                resAllocation.AllocationEndDate = endDate;
                resAllocation.PctEstimatedAllocation = resAllocation.PctAllocation = 0;
                if (totalWorkingHrs > 0)
                {
                    resAllocation.PctEstimatedAllocation = resAllocation.PctAllocation = Convert.ToInt32((allocatedHrs * 100) / totalWorkingHrs);
                }

                allocationManager.Save(resAllocation, false, true);

                #endregion
                #region CPR Integration
                if (resAllocation.ResourceWorkItems != null && cRMProjectAllocation != null)
                {
                    if (resAllocation.ResourceWorkItems.WorkItemType == "CPR" || resAllocation.ResourceWorkItems.WorkItemType == "OPM" || resAllocation.ResourceWorkItems.WorkItemType == "CNS")
                    {
                        // cRMProjectAllocation.TicketId = Convert.ToString(cbLevel2.Value);
                        cRMProjectAllocation.AllocationStartDate = resAllocation.AllocationStartDate;
                        cRMProjectAllocation.AllocationEndDate = resAllocation.AllocationEndDate;
                        cRMProjectAllocation.AssignedTo = resAllocation.Resource;
                        cRMProjectAllocation.PctAllocation = (double)resAllocation.PctAllocation;
                        //cRMProjectAllocation.Type = Convert.ToString(ddlUserType.Value);
                        //cRMProjectAllocation.Title = txtSubProject.Text;
                        CRMProjectAllocationManager.Update(cRMProjectAllocation);
                    };
                }
                #endregion


                //Delete useless entries
                if (monthAllocDBData != null)
                {
                    //List<ResourceAllocationMonthly> wasteRows = monthAllocDBData.Where(x => (x.PctPlannedAllocation.HasValue || x.PctPlannedAllocation == 0) && (x.PctAllocation.HasValue || x.PctAllocation == 0)).ToList();        //Select("ID > 0 and (PctPlannedAllocation is null or PctPlannedAllocation=0) and (PctAllocation is null or PctAllocation=0)");
                    List<ResourceAllocationMonthly> wasteRows = monthAllocDBData.Where(x => (x.PctPlannedAllocation == null || x.PctPlannedAllocation == 0) && (x.PctAllocation == null || x.PctAllocation == 0)).ToList();//Select("ID > 0 and (PctPlannedAllocation is null or PctPlannedAllocation=0) and (PctAllocation is null or PctAllocation=0)");

                    foreach (ResourceAllocationMonthly dRow in wasteRows)
                    {
                        //spItem = SPListHelper.GetItemByID(monthlyAllocCollection, uHelper.StringToInt(dRow[DatabaseObjects.Columns.Id]));
                        if (dRow != null)
                        {
                            allocationMonthlyManager.Delete(dRow);
                        }
                    }
                }

                //only update resource summary weekly and monthly if pct allocation is non zero.
                //in zero case, saveallocation firing update summary for estimation
                if (resAllocation.ResourceWorkItemLookup > 0 && resAllocation.PctAllocation > 0)
                {
                    workItemsId.Add(resAllocation.ResourceWorkItemLookup);
                }
            }

            if (workItemsId.Count > 0)
            {
                //Start Thread to update rmm summary list w.r.t current workitem
                ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
                ULog.WriteException("Method UpdateRMMAllocationSummary Called Inside Thread In Event gvPreview_BatchUpdate on Page ResourceAllocationGrid.ascx");

                ThreadStart threadStartMethod = delegate () { RMMSummaryHelper.UpdateRMMAllocationSummary(applicationContext, workItemsId); };
                Thread sThread = new Thread(threadStartMethod);
                sThread.IsBackground = true;
                sThread.Start();
            }

            allocationData = null;
            stopToRegerateColumns = true;
            gvPreview.DataBind();

            e.Handled = true;
            gvPreview.CancelEdit();
        }

        protected void chkPercentage_CheckedChanged(object sender, EventArgs e)
        {

            allocationData = null;
            gvPreview.DataBind();
        }

        protected void gvPreview_Init(object sender, EventArgs e)
        {
            // gvPreview.Templates.GroupRowContent = new GridGroupRowContentTemplate(userEditPermisionList, isResourceAdmin,rbtnPercentage.Checked);
        }

        protected void btnZoomIn_Click(object sender, EventArgs e)
        {

            allocationData = null;
            gvPreview.DataBind();
        }

        protected void btnZoomOut_Click(object sender, EventArgs e)
        {

            allocationData = null;
            gvPreview.DataBind();
        }

        protected void gvPreview_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                DataRow currentRow = gvPreview.GetDataRow(e.VisibleIndex);

                if (!isResourceAdmin)
                {
                    string userId = UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.Id]);
                    if (userEditPermisionList != null && userEditPermisionList.Count > 0 && !userEditPermisionList.Exists(x => x.Id == userId))
                        e.Row.Attributes.Add("onclick", "event.cancelBubble = true");
                }
            }
        }

        protected void gvPreview_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            ////if (string.IsNullOrWhiteSpace(Convert.ToString(e.NewValues["Title"])))
            ////    AddError(e.Errors, gridTaskList.Columns["Title"], "Title cannot be blank");

            //string[] ids = (Convert.ToString(e.Keys[0])).Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
            //if (ids.Length > 1)
            //{
            //    int workItemID = uHelper.StringToInt(ids[0]);
            //    int allocationID = uHelper.StringToInt(ids[1]);



            //    SPList rAllocation = SPListHelper.GetSPList(DatabaseObjects.Lists.ResourceAllocation);
            //    // Check to make sure the new (or edited) allocation doesn't have a overlapping date range with any other entry
            //    List<string> requiredQuery = new List<string>();
            //    requiredQuery.Add(string.Format("<Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq>", DatabaseObjects.Columns.ResourceWorkItemLookup, workItemID));
            //    SPQuery rQuery = new SPQuery();
            //    rQuery.Query = string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(requiredQuery, requiredQuery.Count - 1, true));
            //    SPListItemCollection resourceAllocations = rAllocation.GetItems(rQuery);

            //    if (resourceAllocations.Count > 0)
            //    {
            //        foreach (SPListItem resourceAllocation in resourceAllocations)
            //        {
            //            // Skip if its the same allocation entry :-)
            //            if (allocationID == int.Parse(Convert.ToString(resourceAllocation[DatabaseObjects.Columns.Id])))
            //                continue;

            //            DateTime allocationStartDate = (DateTime)resourceAllocation[DatabaseObjects.Columns.AllocationStartDate];
            //            DateTime allocationEndDate = (DateTime)resourceAllocation[DatabaseObjects.Columns.AllocationEndDate];

            //            //Overlaps between 2 AllocationRanges
            //            //(StartA <= EndB) And (EndA >= StartB)
            //            DateTime StartDate = new DateTime(Convert.ToInt32(hndYear.Value), 1, 1);
            //            DateTime EndDate = new DateTime(Convert.ToInt32(hndYear.Value), 12, 31);

            //            if (StartDate <= allocationEndDate && EndDate >= allocationStartDate)
            //            {
            //                //Check if the previous duplicate entry was deleted, if yes un-delete it so that user can edit.
            //                if (!uHelper.StringToBoolean(resourceAllocation[DatabaseObjects.Columns.IsDeleted]))
            //                {
            //                    AddError(e.Errors, gvPreview.Columns[5], Constants.ErrorMsgRMMOverlappingDates);
            //                    // return string.Empty;
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}
        }

        void AddError(Dictionary<GridViewColumn, string> errors, GridViewColumn column, string errorText)
        {
            if (errors.ContainsKey(column)) return;
            errors[column] = errorText;
        }

        // Variables that store summary values.  
        DateTime dtstartDate;
        DateTime dtEndDate;
        double ResourceFTE;
        double ResourceTotalFTE;
        protected void gvPreview_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            #region group summary
            if (e.IsGroupSummary)
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    dtstartDate = DateTime.MinValue;
                    dtEndDate = DateTime.MinValue;

                }

                // Calculation. 
                if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DevExpress.Web.ASPxSummaryItem item = ((DevExpress.Web.ASPxSummaryItem)e.Item);

                    if (item.FieldName == "AllocationStartDate" && !string.IsNullOrEmpty(Convert.ToString(e.GetValue("AllocationStartDate"))))
                    {
                        if (dtstartDate == DateTime.MinValue)
                            dtstartDate = Convert.ToDateTime(e.FieldValue);
                        else if (Convert.ToDateTime(e.GetValue("AllocationStartDate")) < dtstartDate)
                        {
                            dtstartDate = Convert.ToDateTime(e.FieldValue);
                        }
                    }

                    if (item.FieldName == "AllocationStartDate" && !string.IsNullOrEmpty(Convert.ToString(e.GetValue("PlannedStartDate"))))
                    {
                        if (dtstartDate == DateTime.MinValue)
                            dtstartDate = Convert.ToDateTime(e.GetValue("PlannedStartDate"));
                        else if (Convert.ToDateTime(e.GetValue("PlannedStartDate")) < dtstartDate)
                        {
                            dtstartDate = Convert.ToDateTime(e.GetValue("PlannedStartDate"));
                        }
                    }


                    if (item.FieldName == "AllocationEndDate" && !string.IsNullOrEmpty(Convert.ToString(e.GetValue("AllocationEndDate"))))
                    {
                        if (dtEndDate == DateTime.MinValue)
                            dtEndDate = Convert.ToDateTime(e.FieldValue);
                        else if (Convert.ToDateTime(e.GetValue("AllocationEndDate")) > dtEndDate)
                        {
                            dtEndDate = Convert.ToDateTime(e.FieldValue);
                        }
                    }

                    if (item.FieldName == "AllocationEndDate" && !string.IsNullOrEmpty(Convert.ToString(e.GetValue("PlannedEndDate"))))
                    {
                        if (dtEndDate == DateTime.MinValue)
                            dtEndDate = Convert.ToDateTime(e.GetValue("PlannedEndDate"));
                        else if (Convert.ToDateTime(e.GetValue("PlannedEndDate")) > dtEndDate)
                        {
                            dtEndDate = Convert.ToDateTime(e.GetValue("PlannedEndDate"));
                        }
                    }
                }
                // Finalization.  
                if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {

                    DevExpress.Web.ASPxSummaryItem item = ((DevExpress.Web.ASPxSummaryItem)e.Item);

                    if (item.FieldName == "AllocationStartDate")
                    {
                        if (dtstartDate != DateTime.MinValue)
                            e.TotalValue = dtstartDate.ToString("MMM-dd-yyyy");
                        else
                            e.TotalValue = "";

                    }
                    if (item.FieldName == "AllocationEndDate")
                    {
                        if (dtEndDate != DateTime.MinValue)
                            e.TotalValue = dtEndDate.ToString("MMM-dd-yyyy");
                        else
                            e.TotalValue = "";
                    }
                }
            }
            #endregion

            #region total summary
            if (e.IsTotalSummary)
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    ResourceFTE = 0.0;
                    ResourceTotalFTE = 0.0;
                }

                // Calculation. 
                if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DevExpress.Web.ASPxSummaryItem item = ((DevExpress.Web.ASPxSummaryItem)e.Item);

                    if (item.FieldName != DatabaseObjects.Columns.Resource && item.FieldName != DatabaseObjects.Columns.Id && item.FieldName != DatabaseObjects.Columns.Title && item.FieldName != "AllocationType" && item.FieldName != DatabaseObjects.Columns.AllocationStartDate && item.FieldName != DatabaseObjects.Columns.AllocationEndDate)
                    {
                        if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "")
                            ResourceFTE += UGITUtility.StringToDouble(Convert.ToString(e.FieldValue));
                    }

                }
                // Finalization.  
                if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    DevExpress.Web.ASPxSummaryItem item = ((DevExpress.Web.ASPxSummaryItem)e.Item);

                    if (item.FieldName == DatabaseObjects.Columns.Title)
                    {
                        if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "ResourceItem")
                            e.TotalValue = "Total Capacity";
                        else
                            e.TotalValue = "Allocated Demand";
                    }

                    if (item.FieldName != DatabaseObjects.Columns.Resource && item.FieldName != DatabaseObjects.Columns.Id && item.FieldName != DatabaseObjects.Columns.Title && item.FieldName != "AllocationType" && item.FieldName != DatabaseObjects.Columns.AllocationStartDate && item.FieldName != DatabaseObjects.Columns.AllocationEndDate)
                    {

                        List<UserProfile> lstUProfile;

                        if (ddlResourceManager.SelectedValue != "0" && ddlResourceManager.SelectedValue != "")
                        {
                            lstUProfile = ObjUserProfileManager.GetUserByManager(Convert.ToString(ddlResourceManager.SelectedValue));  // .LoadUsersByManagerId(Convert.ToInt32(ddlResourceManager.SelectedValue));

                            //UserProfile newlstitem = ObjUserProfileManager.LoadById(Convert.ToString(ddlResourceManager.SelectedValue));  // uGITCache.UserProfileCache.LoadByID(Convert.ToInt32(ddlResourceManager.SelectedValue));
                            UserProfile newlstitem = userProfiles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(Convert.ToString(ddlResourceManager.SelectedValue)));
                            lstUProfile.Add(newlstitem);
                        }
                        else
                        {
                            lstUProfile = ObjUserProfileManager.GetUsersProfile();  /// uGITCache.UserProfileCache.GetAllUsers(SPContext.Current.Web);
                        }


                        foreach (UserProfile userProfile in lstUProfile)
                        {
                            if (!userProfile.Enabled)
                                continue;

                            //filter code.. for dropdowns.
                            if (divFilter.Visible)
                            {
                                if (!string.IsNullOrEmpty(UGITUtility.GetCookieValue(Request, "filterdeptRA")))
                                {
                                    if (userProfile.Department != Convert.ToString(UGITUtility.GetCookieValue(Request, "filterdeptRA")))
                                        continue;
                                }

                                if (ddlFunctionalArea.SelectedValue != "0")
                                {
                                    if (userProfile.FunctionalArea == null)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if (userProfile.FunctionalArea != Convert.ToInt32(ddlFunctionalArea.SelectedValue))
                                            continue;
                                    }

                                }
                            }

                            if (userProfile.UGITStartDate < UGITUtility.StringToDateTime(item.FieldName.Remove(item.FieldName.Length - 1)) && userProfile.UGITEndDate > UGITUtility.StringToDateTime(item.FieldName.Remove(item.FieldName.Length - 1)))
                                ResourceTotalFTE++;

                        }

                        if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "TFTE")
                        {
                            e.TotalValue = Math.Round(ResourceTotalFTE, 2);
                        }
                        else
                        {
                            e.TotalValue = Math.Round(ResourceFTE / 100, 2);
                        }
                    }
                }
            }
            #endregion
        }

        protected void gvPreview_ClientLayout(object sender, ASPxClientLayoutArgs e)
        {
            if (e.LayoutMode == DevExpress.Web.ClientLayoutMode.Saving)
            {
                UGITUtility.CreateCookie(Response, "AccountGrid", e.LayoutData);
            }
            if (e.LayoutMode == DevExpress.Web.ClientLayoutMode.Loading)
            {
                e.LayoutData = UGITUtility.GetCookieValue(Request, "AccountGrid");
            }
        }

        protected void gvPreview_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadFunctionalArea();
            //gvPreview.DataBind();
        }

        protected void cbpManagers_Callback(object sender, CallbackEventArgsBase e)
        {
            string parameters = UGITUtility.ObjectToString(e.Parameter);
            string[] values = UGITUtility.SplitString(parameters, Constants.Separator2);
            if (values.Count() >= 1)
            {
                //UGITUtility.CreateCookie(Response, "filterdeptRA", values[0]);
                hdnaspDepartment.Value = values[0];
                LoadDdlResourceManager(values[0]);
            }
            else
            {
                //UGITUtility.DeleteCookie(Request, Response, "filterdeptRA");
                hdnaspDepartment.Value = "";
                LoadDdlResourceManager();
            }
        }
        protected void gvPreview_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        {
            if (e.Column.FieldName == DatabaseObjects.Columns.Resource)
            {
                //string val1 = uHelper.GetUserNameBasedOnId(HttpContext.Current.GetManagerContext(), UGITUtility.ObjectToString(e.Value1));
                //string val2 = uHelper.GetUserNameBasedOnId(HttpContext.Current.GetManagerContext(), UGITUtility.ObjectToString(e.Value2));
                object name1 = e.GetRow1Value(DatabaseObjects.Columns.Name);
                object name2 = e.GetRow2Value(DatabaseObjects.Columns.Name);
                e.Handled = true;
                e.Result = System.Collections.Comparer.Default.Compare(name1, name2);
            }
        }

        protected void gridExporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.Font = new Font("Calibri", 11f);
            if (e.RowType == GridViewRowType.Header)
                return;
            GridViewDataColumn dataColumn = e.Column as GridViewDataColumn;
            fieldConfig = fieldConfigMgr.GetFieldByFieldName("Resource");
            string Estimatecolor = "#24b6fe";
            string moduleName = "";
            //string Assigncolor = "#24b6fe";
            //string[] color = null;
            List<SummaryResourceProjectComplexity> rComplexity = new List<SummaryResourceProjectComplexity>();
            //BTS-22-000974: Error in pdf export fixed by using the list lstEstimateColors instead of color. Below code is not required.
            //ConfigurationVariable cV = ConfigVariableMGR.LoadVaribale("AllocationTimeLineColor");
            //if (cV != null && !string.IsNullOrEmpty(cV.KeyValue))
            //{
            //    color = cV.KeyValue.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries);
            //}
            string WhiteSpaces;

            if (e.RowType == GridViewRowType.Group)
            {
                user = ObjUserProfileManager.GetUserInfoByIdOrName(e.Text);
                if (e.Text.Length < 35)
                    WhiteSpaces = new string(' ', 35 - e.Text.Length);
                else
                    WhiteSpaces = "  ";

                if (string.IsNullOrEmpty(selectedCategory))
                    rComplexity = cpxManager.Load(x => x.UserId.EqualsIgnoreCase(user.Id));
                else
                    rComplexity = cpxManager.Load(x => x.UserId.EqualsIgnoreCase(user.Id) && selectedCategory.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries).ToList().Any(y => x.ModuleNameLookup.Contains(y)));

                //e.Text = fieldConfigMgr.GetFieldConfigurationData(fieldConfig, e.Text);

                if (rComplexity.Count() > 0)
                {
                    if (chkIncludeClosed.Checked == false) //(chkIncludeClosed.Value == false)
                        e.Text = string.Format("{2}{3}# Active: {0}/$ Active: {1}", rComplexity.Sum(x => x.Count), UGITUtility.FormatNumber(rComplexity.Sum(x => x.HighProjectCapacity), "currency"), e.Text, WhiteSpaces);
                    else
                        e.Text = string.Format("{2}{3}# Lifetime: {0}/$ Lifetime: {1}", rComplexity.Sum(x => x.AllCount), UGITUtility.FormatNumber(rComplexity.Sum(x => x.AllHighProjectCapacity), "currency"), e.Text, WhiteSpaces);
                }

                e.BrickStyle.Font = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);
                e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#4A90E2");
                e.BrickStyle.Sides = DevExPrinting.BorderSide.Bottom;
                e.BrickStyle.BorderColor = System.Drawing.ColorTranslator.FromHtml("#d9dae0");//Grey border lines
                e.BrickStyle.BackColor = System.Drawing.Color.White;
            }

            if (e.RowType == GridViewRowType.Data && dataColumn != null)
            {
                e.BrickStyle.Sides = DevExPrinting.BorderSide.Bottom | DevExPrinting.BorderSide.Top;
                e.BrickStyle.BorderColor = System.Drawing.Color.White;
                e.BrickStyle.BorderWidth = 0;
                e.BrickStyle.BorderStyle = DevExpress.XtraPrinting.BrickBorderStyle.Inset;
                string foreColor = "#000000";
                moduleName = Convert.ToString(e.GetValue(DatabaseObjects.Columns.ModuleName));
                if (lstEstimateColorsAndFontColors != null)
                {
                    string value = Convert.ToString(lstEstimateColorsAndFontColors.FirstOrDefault(x => x.Contains(moduleName)));
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        Estimatecolor = UGITUtility.SplitString(value, Constants.Separator, 1);
                        foreColor = UGITUtility.SplitString(value, Constants.Separator, 2);
                    }
                }

                if (dataColumn.FieldName == "AllocationStartDate" || dataColumn.FieldName == "AllocationEndDate")
                {
                    if (string.IsNullOrEmpty(e.Text.ToString()))
                        e.Text = "01/01/1900";
                    e.Text = string.Format("{0: MMM dd, yyyy}", Convert.ToDateTime(e.Text));
                }
                if (dataColumn.FieldName != DatabaseObjects.Columns.Resource && dataColumn.FieldName != "AllocationType" && dataColumn.FieldName != DatabaseObjects.Columns.AllocationStartDate && dataColumn.FieldName != DatabaseObjects.Columns.AllocationEndDate && dataColumn.FieldName != DatabaseObjects.Columns.Title)
                {
                    DateTime plndD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.PlannedStartDate));
                    DateTime estD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.AllocationStartDate));
                    for (DateTime dt = Convert.ToDateTime(dateFrom); Convert.ToDateTime(dateTo) > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
                    {
                        string estAlloc;

                        if (dataColumn.FieldName == dt.ToString("MMM-dd-yy") + "E")
                        {
                            object estAllocValue = e.GetValue(dt.ToString("MMM-dd-yy") + "E");
                            if (estAllocValue != System.DBNull.Value)
                                estAlloc = Math.Ceiling(Convert.ToDecimal(e.GetValue(dt.ToString("MMM-dd-yy") + "E"))).ToString();
                            else
                                estAlloc = Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "E"));
                            string AssignAlloc = Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "A"));

                            string html = string.Empty;
                            if (!string.IsNullOrEmpty(estAlloc) && !estAlloc.Equals("0"))
                            {
                                html = $"{estAlloc}%";
                            }

                            if (!string.IsNullOrEmpty(AssignAlloc) & plndD != DateTime.MinValue)
                            {
                                html += $"{AssignAlloc}%";
                            }

                            e.Text = html;
                            //create bars with module specific colours

                            //if (moduleName == "Time Off" || moduleName == "CNS")
                            e.BrickStyle.ForeColor = ColorTranslator.FromHtml(foreColor);
                            if (!string.IsNullOrEmpty(estAlloc) && !estAlloc.Equals("0"))
                            {
                                e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(Estimatecolor);
                                e.BrickStyle.BorderColor = System.Drawing.Color.White;
                                e.BrickStyle.BorderWidth = 6;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(e.Text) && !e.Text.Contains("%"))
                            {
                                e.Text = $"{e.Text}%";
                            }
                        }

                    }
                }
            }
        }

        protected void chkIncludeClosed_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIncludeClosed.Checked)
                UGITUtility.CreateCookie(Response, "IncludeClosedAT", "true");
            else
                UGITUtility.CreateCookie(Response, "IncludeClosedAT", "");

            allocationData = GetAllocationData();
            gvPreview.DataBind();
        }

        protected void gvPreview_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            ASPxSpinEdit ctr = e.Editor as ASPxSpinEdit;
            if (ctr != null)
            {
                ctr.Text = UGITUtility.ObjectToString(e.Value);
            }
        }


        protected void bZoomIn_Click(object sender, EventArgs e)
        {
            allocationData = null;
            gvPreview.DataBind();
        }
        protected void bZoomOut_Click(object sender, EventArgs e)
        {
            allocationData = null;
            gvPreview.DataBind();
        }

        protected void cp_Callback(object sender, CallbackEventArgsBase e)
        {
            if (rbtnPercentage.Checked)
            {
                UGITUtility.CreateCookie(Response, "filterbarpercentagefte", "percentage");
            }
            if (rbtnFTE.Checked)
            {
                UGITUtility.CreateCookie(Response, "filterbarpercentagefte", "fte");
            }
        }

        protected void mnuExportOptions_ItemClick(object source, MenuItemEventArgs e)
        {
            string strCommand = e.Item.ToString();

            btnexport = true;
            allocationData = GetAllocationData();
            gvPreview.DataSource = allocationData;

            if (strCommand == "CSV")
            {
                DevExpress.XtraPrinting.CsvExportOptionsEx optionsEx = new DevExpress.XtraPrinting.CsvExportOptionsEx();
                optionsEx.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                optionsEx.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;

                gvPreview.Columns["AllocationType"].Visible = false;
                gridExporter.WriteCsvToResponse("Administrator View", true, optionsEx);
            }
            else
            {
                DevExpress.XtraPrinting.XlsExportOptionsEx options = new DevExpress.XtraPrinting.XlsExportOptionsEx();
                options.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;

                gvPreview.Columns["AllocationType"].Visible = false;
                gridExporter.WriteXlsToResponse("Administrator View", options);
            }
        }
    }

    public class GridGroupRowContentTemplate : ITemplate
    {
        const string
            MainTableCssClassName = "summaryTable rmmSummary-table",
            VisibleColumnCssClassName = "gridVisibleColumn",
            SummaryTextContainerCssClassName = "summaryTextContainer",
            SummaryCellCssClassNameFormat = "summaryCell_{0}",
            GroupTextFormat = "{0}: {1}";

        List<UserProfile> permisionlist = null;
        bool isAdminResource = false;
        bool isPercentageMode = false;
        bool IncludeClosedProjects = false;
        List<string> lstselectedCategory = null;

        public GridGroupRowContentTemplate(List<UserProfile> userEditPermisionList, bool isResourceAdmin, bool rbtnPercentage, bool IncludeClosedProjects, string selectedCategory)
        {
            permisionlist = userEditPermisionList;
            isAdminResource = isResourceAdmin;
            isPercentageMode = rbtnPercentage;
            this.IncludeClosedProjects = IncludeClosedProjects;
            this.lstselectedCategory = selectedCategory.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        protected GridViewGroupRowTemplateContainer Container { get; set; }
        protected DevExpress.Web.ASPxGridView Grid { get { return Container.Grid; } }

        protected Table MainTable { get; set; }
        protected TableRow GroupTextRow { get; set; }
        protected TableRow SummaryTextRow { get; set; }

        protected int IndentCount { get { return Grid.GroupCount - GroupLevel - 1; } }
        protected int GroupLevel { get { return Grid.DataBoundProxy.GetRowLevel(Container.VisibleIndex); } }
        protected UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
        protected ConfigurationVariableManager ConfigVarManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        ResourceProjectComplexityManager cpxManager = new ResourceProjectComplexityManager(HttpContext.Current.GetManagerContext());
        List<SummaryResourceProjectComplexity> rComplexity = new List<SummaryResourceProjectComplexity>();
        UserProfile user = null;
        protected List<GridViewColumn> VisibleColumns
        {
            get
            {
                List<GridViewColumn> lstCols = new List<GridViewColumn>();
                foreach (GridViewColumn item in Grid.AllColumns)
                {
                    if (item.Visible && (item as GridViewBandColumn) == null)
                    {
                        lstCols.Add(item);
                    }
                }

                return lstCols.Except(Grid.GetGroupedColumns()).ToList();

                // return Grid.VisibleColumns.Except(Grid.GetGroupedColumns()).ToList(); 
            }
        }

        public void InstantiateIn(Control container)
        {

            Container = (GridViewGroupRowTemplateContainer)container;
            CreateGroupRowTable();
            Container.Controls.Add(MainTable);

            ApplyStyles();
        }

        protected void CreateGroupRowTable()
        {
            MainTable = new Table();

            GroupTextRow = CreateRow("Group");
            SummaryTextRow = CreateRow("Summary");

            CreateGroupTextCell();
            CreateIndentCells();
            foreach (var column in VisibleColumns)
                CreateSummaryTextCell(column);
        }

        protected void CreateGroupTextCell()
        {
            var cell = CreateCell(GroupTextRow);
            cell.Text = "";
            cell.ColumnSpan = VisibleColumns.Count + IndentCount;
        }

        protected void CreateSummaryTextCell(GridViewColumn column)
        {
            var cell = CreateCell(SummaryTextRow);

            //if (column.Caption == "Title")
            if (column.Caption == "Work Item")
            {
                string strCell = string.Empty;
                //UserProfile user = UserManager.GetUserInfoById(Container.GroupText);
                //user = UserManager.GetUserInfoById(Container.GroupText);
                user = UserManager.GetUserInfoByIdOrName(Container.GroupText);

                isAdminResource = UserManager.IsUGITSuperAdmin(HttpContext.Current.CurrentUser()) || UserManager.IsResourceAdmin(HttpContext.Current.CurrentUser());
                //bool allowAllocationForSelf = ConfigVarManager.GetValueAsBool(ConfigConstants.AllowAllocationForSelf);
                string allowAllocationForSelf = ConfigVarManager.GetValue(ConfigConstants.AllowAllocationForSelf);

                if (!isAdminResource)
                {
                    permisionlist = UserManager.LoadAuthorizedUsers(allowAllocationForSelf);
                }

                string appendIcons = "";

                if (isAdminResource)
                {

                    if (user != null)
                    {
                        appendIcons = string.Format("<image style=\"padding-right:7px; width: 20px ;cursor:pointer;\" src=\"/content/images/plus-blue.png\" title='New Allocation' onclick=\"javascript:event.cancelBubble=true; OpenAddAllocationPopup('" + user.Id + "', '" + user.Name + "')\"  />");
                    }
                }
                else
                {
                    if (user != null)
                    {
                        if (permisionlist != null && permisionlist.Exists(x => x.Id == user.Id))
                            appendIcons = string.Format("<image style=\"padding-right:7px;width: 20px ;cursor:pointer;\" src=\"/content/images/plus-blue.png\" title='New Allocation' onclick=\"javascript:event.cancelBubble=true; OpenAddAllocationPopup('" + user.Id + "', '" + user.Name + "')\"  />");
                    }
                }

                strCell = string.Format("{0} {1}", user.Name, appendIcons);

                cell.Text = strCell;
                return;
            }
            else if (column.Caption == "Allocation Type")
            {
                string strCell = string.Empty;
                UserProfile user = UserManager.GetUserInfoByIdOrName(Container.GroupText);
                if (user != null)
                {
                    //ResourceProjectComplexityManager cpxManager = new ResourceProjectComplexityManager(HttpContext.Current.GetManagerContext());
                    //List<SummaryResourceProjectComplexity> rComplexity = cpxManager.Load(x => x.UserId == user.Id);

                    if (lstselectedCategory.Count > 0)
                        rComplexity = cpxManager.Load(x => x.UserId == user.Id && lstselectedCategory.Any(y => x.ModuleNameLookup.Contains(y)));
                    else
                        rComplexity = cpxManager.Load(x => x.UserId == user.Id);

                    if (rComplexity.Count() > 0)
                    {
                        if (IncludeClosedProjects == false)
                            strCell = string.Format("# Active: {0}/$ Active: {1}", rComplexity.Sum(x => x.Count), UGITUtility.FormatNumber(rComplexity.Sum(x => x.HighProjectCapacity), "currency"));
                        else
                            strCell = string.Format("# Lifetime: {0}/$ Lifetime: {1}", rComplexity.Sum(x => x.AllCount), UGITUtility.FormatNumber(rComplexity.Sum(x => x.AllHighProjectCapacity), "currency"));
                    }
                    cell.Text = strCell;
                }
                return;
            }

            var dataColumn = column as GridViewDataColumn;
            if (dataColumn == null)
                return;


            var summaryItems = FindSummaryItems(dataColumn);
            if (summaryItems.Count == 0)
                return;

            var div = new WebControl(HtmlTextWriterTag.Div) { CssClass = SummaryTextContainerCssClassName };
            cell.Controls.Add(div);

            var text = string.Empty;
            if (dataColumn.FieldName != DatabaseObjects.Columns.Title && dataColumn.FieldName != DatabaseObjects.Columns.AllocationStartDate && dataColumn.FieldName != DatabaseObjects.Columns.AllocationEndDate)
            {
                var summarytext = GetGroupSummaryText(summaryItems, column);
                if (!string.IsNullOrEmpty(summarytext))
                {

                    if (isPercentageMode)
                        text = summarytext + "%";
                    else
                    {
                        text = string.Format("{0:0.00}", Math.Round(UGITUtility.StringToDouble(summarytext) / 100, 2));
                    }
                }
            }
            else
            {
                text = GetGroupSummaryText(summaryItems, column);//.Replace("<br />", "");
            }
            div.Controls.Add(new LiteralControl(text));
        }

        protected string GetGroupSummaryText(List<ASPxSummaryItem> items, GridViewColumn column)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var summaryValue = Grid.GetGroupSummaryValue(Container.VisibleIndex, item);



                sb.Append(item.GetGroupRowDisplayText(column, summaryValue));
            }
            return sb.ToString();
        }

        protected void ApplyStyles()
        {
            MainTable.CssClass = MainTableCssClassName;
            VisibleColumns[0].HeaderStyle.CssClass = VisibleColumnCssClassName;

            var startIndex = GroupLevel + 1;
            for (var i = 0; i < SummaryTextRow.Cells.Count; i++)
                SummaryTextRow.Cells[i].CssClass = string.Format(SummaryCellCssClassNameFormat, i + startIndex);
        }

        protected void CreateIndentCells()
        {
            for (var i = 0; i < IndentCount; i++)
                CreateCell(SummaryTextRow);
        }
        protected List<ASPxSummaryItem> FindSummaryItems(GridViewDataColumn column)
        {
            return Grid.GroupSummary.Where(i => i.FieldName == column.FieldName).ToList();
        }
        protected TableRow CreateRow(string rowtype)
        {
            var row = new TableRow();
            if (rowtype == "Summary")
                row.CssClass = "SummaryHeaderAdjustment";
            MainTable.Rows.Add(row);
            return row;
        }
        protected TableCell CreateCell(TableRow row)
        {
            var cell = new TableCell();
            row.Cells.Add(cell);
            return cell;
        }
    }

}
