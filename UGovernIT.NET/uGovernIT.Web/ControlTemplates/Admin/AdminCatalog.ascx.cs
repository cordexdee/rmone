using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class AdminCatalog : UserControl
    {
        DataTable dataTable;
        DataTable dataTableLists;

        private DataRow serviceRow;
        private string absoluteUrlImport = "/layouts/ugovernit/DelegateControl.aspx?control={0}&listName={1}";
        private ApplicationContext _context = null;
        private ConfigurationVariableManager _configurationVariableHelper = null;
        private AdminConfigurationListManager _adminConfigurationList = null;
        private AdminCategoryManager _adminCategory = null;

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

        protected ConfigurationVariableManager ConfigurationVariableManager
        {
            get
            {
                if (_configurationVariableHelper == null)
                {
                    _configurationVariableHelper = new ConfigurationVariableManager(ApplicationContext);
                }
                return _configurationVariableHelper;

            }
        }

        protected AdminConfigurationListManager AdminConfigurationListManager
        {
            get
            {
                if (_adminConfigurationList == null)
                {
                    _adminConfigurationList = new AdminConfigurationListManager(ApplicationContext);
                }
                return _adminConfigurationList;
            }
        }

        protected AdminCategoryManager AdminCategoryManager
        {
            get
            {
                if (_adminCategory == null)
                {
                    _adminCategory = new AdminCategoryManager(ApplicationContext);
                }
                return _adminCategory;
            }
        }

        protected string importUrl;

        protected void Page_Load(object sender, EventArgs e)
        {

            importUrl = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlImport, "importexcelfile", "MasterImport"));


            if (ConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableAdminImport))
                btnMasterImport.Visible = true;

            string licenseUrl = string.Empty;
            string serviceurl = string.Empty;
            string facttablUrl = string.Empty;
            string cacheurl = string.Empty;
            string dashboardurl = string.Empty;
            string tenantUrl = string.Empty;

            bool isSuperAdmin = ApplicationContext.UserManager.IsUGITSuperAdmin(ApplicationContext.CurrentUser);

            //AdminConfigurationListManager adminConfigurationListManager = new AdminConfigurationListManager(ApplicationContext);
            // AdminCategoryManager adminCategoryManager = new AdminCategoryManager(ApplicationContext);
            DataTable lstAdminCat = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ClientAdminCategory, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
            DataTable lstAdmingConfig = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ClientAdminConfigurationLists, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");

            licenseUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configlicense");
            Alicencse.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Licenses','70','90')", licenseUrl));

            serviceurl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=services");
            Aservice.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Services & Agents','90','90')", serviceurl));

            cacheurl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configcache");
            Achache.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Cache','90','90')", cacheurl));

            dashboardurl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configdashboard");
            Adashbord.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Dashboards & Queries','90','90')", dashboardurl));

            facttablUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configdashboardfacttable");
            Afacttable.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Fact Tables','90','75')", facttablUrl));

            if (isSuperAdmin)
            {
                tenantUrl = UGITUtility.GetAbsoluteURL("/Tenant/Index");
                ATenantManagement.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Tenant Management','90','70')", tenantUrl));
            }
            else
            {
                ATenantManagement.Visible = false;
            }

            // dataTableLists = SPListHelper.GetDataTable(lstAdmingConfig);
            //AdminConfigurationListManager AdminConfigurationListManager = new AdminConfigurationListManager(ApplicationContext);
            var menulist = AdminConfigurationListManager.Load();
            var itemCollection = menulist.Where(x => null != x.AuthorizedToViewUser && x.AuthorizedToViewUser.Split(';').Contains(Context.CurrentUser().Id)).ToList();
            if (itemCollection.Count > 0 && itemCollection != null)
            {
                dataTableLists = UGITUtility.ToDataTable<ClientAdminConfigurationList>(itemCollection);
            }
            else
            {
                dataTableLists = UGITUtility.ToDataTable<ClientAdminConfigurationList>(menulist);
            }
            
            //if (lstAdminCat != null)
            // {
            // dataTable = SPListHelper.GetDataTable(lstAdminCat);
            // AdminCategoryManager AdminCategoryManager = new AdminCategoryManager(ApplicationContext);
            dataTable = UGITUtility.ToDataTable<ClientAdminCategory>(AdminCategoryManager.Load());
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                dataTable.DefaultView.Sort = DatabaseObjects.Columns.ItemOrder;
                RptCategory.DataSource = dataTable.DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.Title });
                RptCategory.DataBind();
            }



            // }

            //Hide Service catelog when service config is desable
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            UGITModule module = moduleViewManager.LoadByName("SVC",true);
            DataTable dtmodule = UGITUtility.ObjectToData(module);// uGITCache.ModuleConfigCache.LoadModuleDtByName("SVC");
            //RefreshFactable();

            //DataRow moduleDetail = serviceRow;
            if (dtmodule.Rows.Count > 0)
            {
                serviceRow = dtmodule.Rows[0];
                if (serviceRow == null)
                    tdServiceBlock.Visible = false;
                else
                {
                    bool isEnable = UGITUtility.StringToBoolean(serviceRow[DatabaseObjects.Columns.EnableModule]);
                    if (!isEnable)
                        tdServiceBlock.Visible = false;
                }
            }

        }

        protected void RptCategory_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblCategory = (Label)e.Item.FindControl("LblCategroy");
                Repeater rptrSubCategories = (Repeater)e.Item.FindControl("RptSubCategory");
                DataRow[] dr = dataTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, lblCategory.Text));
                if (dr.Length > 0)
                {
                    rptrSubCategories.DataSource = dr.CopyToDataTable();
                    rptrSubCategories.DataBind();
                }
            }
        }

        protected void RptSubCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblSubCategory = (Label)e.Item.FindControl("LblSubCategroy");

                DataRow[] drparent = dataTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.CategoryName, lblSubCategory.Text));

                string parentid = drparent[0][DatabaseObjects.Columns.Id].ToString();

                Repeater rptListItem = (Repeater)e.Item.FindControl("RptItemList");
                //Creates new UL after 3 LI block
                Literal lULTop = (Literal)e.Item.FindControl("lULTop");
                Literal lULBottom = (Literal)e.Item.FindControl("lULBottom");
                if (e.Item.ItemIndex % 3 == 0)
                {
                    lULTop.Visible = true;
                    lULBottom.Visible = false;
                }

                DataRowView view = (DataRowView)e.Item.DataItem;
                if (string.IsNullOrEmpty(Convert.ToString(view[DatabaseObjects.Columns.ImageUrl])))
                {
                    HtmlImage imgCategory = (HtmlImage)e.Item.FindControl("imgCategory");
                    imgCategory.Visible = false;
                   
                }

                if (dataTableLists != null && dataTableLists.Rows.Count > 0)
                {
                    DataRow[] dr = dataTableLists.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ClientAdminCategoryLookup, parentid));
                    if (dr.Length > 0)
                    {
                        DataTable categoryItems = dr.CopyToDataTable();
                        categoryItems.DefaultView.Sort = "TabSequence ASC";
                        rptListItem.DataSource = categoryItems.DefaultView.ToTable();
                        rptListItem.DataBind();
                    }
                }
            }
        }

        protected void RptItemList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string absPath = string.Empty;
            string width = "80";
            string height = "80";
            string isPixel = "false";

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RepeaterItem gvRow = e.Item;
                DataRowView rowView = (DataRowView)e.Item.DataItem;
                string listName = Convert.ToString(rowView[DatabaseObjects.Columns.ListName]).Trim();
                listName = listName.ToLower();
                LinkButton link = (LinkButton)e.Item.FindControl("LnkListName");
                //HtmlAnchor htmlAnchor = (HtmlAnchor)e.Item.FindControl("anchorElement");


                // Code block to disable link whose Deleted column is set to true, in DB
                if (Convert.ToBoolean(rowView[DatabaseObjects.Columns.Deleted]) == true)
                {
                    link.Attributes.Remove("href");
                    link.Attributes["OnClick"] = "return false";
                    link.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "gray";
                    link.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
                    return;
                }

                if (listName == DatabaseObjects.Tables.ConfigurationVariable.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configvar");
                }
                else if (listName == DatabaseObjects.Tables.RequestType.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=requesttype");
                    width = "90";
                    height = "90";
                }
                else if (listName == DatabaseObjects.Tables.RequestPriority.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=priortymapping");
                    // Passing 'true' in last parameter makes width & height in pixels instead of %
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','700','500','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.TaskEmails.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=emailnotification");
                    width = "80";
                    height = "87";
                }
                else if (listName == DatabaseObjects.Tables.TicketImpact.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=ticketimpact&mode=Impact");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','700','500','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.TicketSeverity.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=ticketimpact&mode=Severity");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','700','500','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.TicketPriority.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=ticketpriority&mode=Priority");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','700','500','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.SLARule.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=slarule");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','1000px','700px','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.EscalationRule.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=escalationrule");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','800px','600px','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.BudgetCategories.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=bgetcatview");
                }
                else if (listName == DatabaseObjects.Tables.ProjectClass.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=projectclass");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','800','450','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.ProjectSimilarityConfig.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=projectsimilarityconfigview");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','1000','590','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.ProjectInitiative.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=projectinit");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','1000','550','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.ACRTypes.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=acrtypes");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','650','510','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.DRQSystemAreas.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=drqsystemarea");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','650','510','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.DRQRapidTypes.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=drqrapidtypes");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','650','500','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.ModuleUserTypes.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=userroletypes");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.Department.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=deptview");
                    height = "80";
                    width = "55";
                }
                else if (listName == DatabaseObjects.Tables.FunctionalAreas.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=funcareaview");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','70','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.Location.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=locationview");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','90','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.AssetVendors.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=assetvendorview");
                }
                else if (listName == DatabaseObjects.Tables.AssetModels.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=assetmodelview");
                }
                else if (listName == DatabaseObjects.Tables.Modules.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=moduleview");
                }
                else if (listName.ToLower() == DatabaseObjects.Tables.ModuleStages.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagesview");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','90','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.ModuleColumns.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulecolumnsview");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','90','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.FormLayout.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=formlayoutandpermission");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','90','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.GovernanceLinkCategory.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=governanceconfig&param=config");
                }
                else if (listName == DatabaseObjects.Tables.GovernanceLinkItems.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=governanceconfig&param=dashboard");
                }
                else if (listName == "governancecategories")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=governancecategoriesview&param=config");
                }
                else if (listName == "surveyformslink")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=surveyformslink&param=config");
                }
                else if (listName == "surveyfeedbacklink")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=surveyfeedback&param=config");
                    width = "90";
                    height = "90";
                }
                else if (listName == "survey")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=services&ServiceType=SurveyFeedback");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Services & Agents','90','90')", absPath));
                }
                else if (listName == "analyticauth")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=analyticauth&param=config");
                    width = "500";
                    height = "200";
                    isPixel = "true";
                }
                else if (listName == "projectlifecycles")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=projectlifecycles&param=config");
                }
                else if (listName == "environment")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=environmentview&param=config");
                }
                else if (listName == DatabaseObjects.Tables.SubLocation.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=sublocationview");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','90','','')", absPath, link.Text));

                    //string url1 = string.Format("window.parent.UgitOpenPopupDialog('{0}','','Sub Location','600','250',0,'{1}','true')", absPath, Server.UrlEncode(Request.Url.AbsolutePath));
                }
                else if (listName == "messageboard")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=messageboard&mode=config");
                    width = "900";
                    height = "700";
                    isPixel = "true";
                }
                else if (listName == "dmdepartment" || listName == "doctypeinfo" || listName == "documenttype" || listName == "dmprojects" || listName == "dmvendor")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=todo");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','1000','500','','','true')", absPath, link.Text));
                    return;

                }
                else if (listName == "modulestagetemplates")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagetemplates");
                }
                else if (listName == "modulestagerules")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagerulelist");
                }
                else if (listName == DatabaseObjects.Tables.SchedulerActions.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=scheduleactionslist");
                    width = "90";
                    height = "90";
                }
                else if (listName == DatabaseObjects.Tables.UGITTaskTemplates.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=UGITTaskTemplates");
                }
                else if (listName == DatabaseObjects.Tables.ProjectStandardWorkItems.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=ProjectStandardWorkItemView");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','1000','500','','','true')", absPath, link.Text));
                    return;
                }
                // added new condition.
                else if (listName == DatabaseObjects.Tables.EventCategories.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=eventcategory");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','600','400','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName.ToLower() == "deletetickets")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=deletetickets");
                }
                else if (listName == DatabaseObjects.Tables.HolidaysAndWorkDaysCalendar.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=holidaycalendarevent");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','90','','')", absPath, link.Text));
                    return;
                }
                else if (listName == "adusermapping")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=adusers");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','620px','600px','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.UGITLog.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=UGITLog");
                    width = "85";
                    height = "66";
                }
                else if (listName == DatabaseObjects.Tables.WikiLeftNavigation.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=wikileftnavigation");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','70','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.MenuNavigation.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=menunavigationview");
                    height = "90";
                    width = "90";
                }
                else if (listName == DatabaseObjects.Tables.PageEditor.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=pageeditorview");
                    height = "90";
                    width = "90";
                }
                else if (listName == "updatethemecolor")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=updatethemecolor");
                    height = "90";
                    width = "90";
                }
                else if (listName == DatabaseObjects.Tables.UserSkills.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=userskillsview");
                    height = "530px";
                    width = "650px";
                }
                else if (listName == DatabaseObjects.Tables.UserCertificates.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=usercertificatesview");
                    height = "530px";
                    width = "650px";
                }
                else if (listName == DatabaseObjects.Tables.UserRoles.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=userrolesview");
                    height = "430px";
                    width = "560px";
                }
                else if (listName == DatabaseObjects.Tables.adminauth.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=adminauth");
                    height = "200px";
                    width = "500px";
                }
                else if (listName == DatabaseObjects.Tables.setugittheme.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=setugittheme");
                    height = "90";
                    width = "90";

                    string source = Uri.EscapeDataString(Request.Url.AbsolutePath);
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','{2}','{3}', false, '{4}')", absPath, link.Text, width, height, source));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.TicketTemplates.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=TicketTemplatelist");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','80','80','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.LinkView.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=linkconfig&viewLink=0");
                }
                else if (listName == DatabaseObjects.Tables.ModuleDefaultValues.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=moduledefaultvalues");
                }
                else if (listName == DatabaseObjects.Tables.Organization.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=organizationview");
                }
                else if (listName == DatabaseObjects.Tables.Contacts.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=contactsview");
                }
                else if (listName == DatabaseObjects.Tables.Emails.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=emailtoticket");
                    height = "450px";
                    width = "500px";
                }
                else if (listName == "smtpmail")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=smtpmailsetting");
                    height = "370px";
                    width = "500px";
                }
                else if (listName == DatabaseObjects.Tables.ResourceManagement.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=resourcemanagement").ToLower();
                    height = "80";
                    width = "90";
                }
                else if (listName == DatabaseObjects.Tables.EnableMigrate.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=enablemigrate");
                    height = "200px";
                    width = "500px";
                }
                else if (listName == DatabaseObjects.Tables.AssetIntegrationConfiguration.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=assetintegrationconfiguration");
                    height = "200px";
                    width = "500px";
                }
                else if (listName == "editchoicefield")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=choicefieldedit");
                    height = "200px";
                    width = "500px";
                }
                else if (listName == DatabaseObjects.Tables.VendorType.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=vendortypeview");
                }
                else if (listName == DatabaseObjects.Tables.DocumentType.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=DocumentTypeView");
                }
                else if (listName == DatabaseObjects.Tables.ModuleStageConstraintTemplates.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagetemplates");
                }
                else if (listName == DatabaseObjects.Tables.LinkItems.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=governanceconfig&param=dashboard");
                }
                else if (listName == DatabaseObjects.Tables.LinkCategory.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=governanceconfig&param=config");

                }
                else if (listName == DatabaseObjects.Tables.AspNetRoles.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=userrolesview");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','45','55','','')", absPath, link.Text));
                    return;
                }
                else if (listName == Convert.ToString("Config_ProjectLifeCycles").ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=projectlifecycles&param=config");
                }
				else if (listName.Equals("LandingPages", StringComparison.InvariantCultureIgnoreCase))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=userrolesview");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','45','70','','')", absPath, link.Text));
                    return;
                }
				else if (listName.Equals("JobTitle", StringComparison.InvariantCultureIgnoreCase))
                {
                    //absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=jobtitleview");
                    //link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','80','','')", absPath, link.Text));

                    //absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=jobtitleview&mode=config");
                    //width = "850";
                    //height = "450";
                    //isPixel = "true";
                    //return;
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=jobtitleview&mode=config");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','75','75','','')", absPath, link.Text));
                    return;
                }
				else if(listName.Equals("Roles", StringComparison.InvariantCultureIgnoreCase))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=RolesView");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','75','75','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("addchoices"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=addchoices");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','40','50','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("HelpCard"))
                {
                    link.Attributes.Add("href", string.Format("/pages/HelpCard"));
                    link.Attributes.Add("target", "_blank");
                    return;
                }
                else if (listName.EqualsIgnoreCase("CheckListTemplates"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=CheckListTemplates");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','90','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("RankingCriteria"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=RankingCriteria");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("ProcoreUtility"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=ProcoreUtility");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("ProcoreMapping"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=ProcoreMapping");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("ProcoreCredentials"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=RestAPICredentials");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("TokenInfo"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/RedirectionHandler.aspx");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("LeadCriteria"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=leadcriteria");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','60','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("ProjectComplexity"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=projectcomplexity");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','60','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("ExperiencedTags"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=ExperiencedTags");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','60','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("UserProjectExperiences"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=UserProjectExperiences");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','60','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("DataRefresh"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=DataRefresh");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','60','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName== DatabaseObjects.Tables.EmployeeTypes.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=employeetypeview");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','60','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("phrasesview"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=phrasesview");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','80','80','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("widgets"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=widgetsview");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','80','80','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("SchedulerJob"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/scheduler-jobs");
                    link.Attributes.Add("href", absPath);
                    link.Attributes.Add("target", "_blank");
                    return;
                }
                else if (listName.EqualsIgnoreCase("Studios"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=Studios");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','80','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("ApplicationHealth"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=ApplicationHealth");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','90','','')", absPath, link.Text));
                    return;
                }
                else
                {
                    absPath = GetDefaultAbsPath(listName);
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','80','','')", absPath, link.Text));
                    return;
                }

                link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','{2}','{3}', false, '', {4})", absPath, link.Text, width, height, isPixel));
                //htmlAnchor.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','{2}','{3}', false, '', {4})", absPath, link.Text, width, height, isPixel));
            }
        }

        protected string GetDefaultAbsPath(string listName)
        {
            return UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=" + listName);
        }

        protected void RptItemList1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string absPath = string.Empty;
            string width = "80";
            string height = "80";
            string isPixel = "false";

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RepeaterItem gvRow = e.Item;
                DataRowView rowView = (DataRowView)e.Item.DataItem;
                string listName = Convert.ToString(rowView[DatabaseObjects.Columns.ListName]).Trim();
                listName = listName.ToLower();
                LinkButton link = (LinkButton)e.Item.FindControl("LnkListName");
                //HtmlAnchor htmlAnchor = (HtmlAnchor)e.Item.FindControl("anchorElement");


                // Code block to disable link whose Deleted column is set to true, in DB
                if (Convert.ToBoolean(rowView[DatabaseObjects.Columns.Deleted]) == true)
                {
                    link.Attributes.Remove("href");
                    link.Attributes["OnClick"] = "return false";
                    link.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "gray";
                    link.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
                    return;
                }

                if (listName == DatabaseObjects.Tables.ConfigurationVariable.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configvar");
                }
                else if (listName == DatabaseObjects.Tables.RequestType.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=requesttype");
                    width = "90";
                    height = "90";
                }
                else if (listName == DatabaseObjects.Tables.RequestPriority.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=priortymapping");
                    // Passing 'true' in last parameter makes width & height in pixels instead of %
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','700','400','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.TaskEmails.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=emailnotification");
                }
                else if (listName == DatabaseObjects.Tables.TicketImpact.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=ticketimpact&mode=Impact");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','700','500','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.TicketSeverity.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=ticketimpact&mode=Severity");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','700','500','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.TicketPriority.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=ticketpriority&mode=Priority");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','700','500','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.SLARule.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=slarule");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','1000px','700px','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.EscalationRule.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=escalationrule");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','800px','600px','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.BudgetCategories.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=bgetcatview");
                }
                else if (listName == DatabaseObjects.Tables.ProjectClass.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=projectclass");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','90','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.ProjectInitiative.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=projectinit");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','1000','550','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.ACRTypes.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=acrtypes");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','600','300','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.DRQSystemAreas.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=drqsystemarea");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','600','300','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.DRQRapidTypes.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=drqrapidtypes");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','600','500','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.ModuleUserTypes.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=userroletypes");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','200','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.Department.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=deptview");
                    height = "90";
                    width = "1000px";
                }
                else if (listName == DatabaseObjects.Tables.FunctionalAreas.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=funcareaview");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','70','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.Location.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=locationview");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','90','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.AssetVendors.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=assetvendorview");
                }
                else if (listName == DatabaseObjects.Tables.AssetModels.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=assetmodelview");
                }
                else if (listName == DatabaseObjects.Tables.Modules.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=moduleview");
                }
                else if (listName.ToLower() == DatabaseObjects.Tables.ModuleStages.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagesview&title=workflows");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','800px','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.ModuleColumns.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulecolumnsview");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','90','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.FormLayout.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=formlayoutandpermission");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','90','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.GovernanceLinkCategory.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=governanceconfig&param=config");
                }
                else if (listName == DatabaseObjects.Tables.GovernanceLinkItems.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=governanceconfig&param=dashboard");
                }
                else if (listName == "governancecategories")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=governancecategoriesview&param=config");
                }
                else if (listName == "surveyformslink")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=surveyformslink&param=config");
                }
                else if (listName == "surveyfeedbacklink")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=surveyfeedback&param=config");
                    width = "90";
                    height = "90";
                }
                else if (listName == "survey")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=services&ServiceType=SurveyFeedback");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Services & Agents','90','90')", absPath));
                }
                else if (listName == "analyticauth")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=analyticauth&param=config");
                    width = "500";
                    height = "200";
                    isPixel = "true";
                }
                else if (listName == "projectlifecycles")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=projectlifecycles&param=config");
                }
                else if (listName == "environment")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=environmentview&param=config");
                }
                else if (listName == DatabaseObjects.Tables.SubLocation.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=sublocationview");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','90','','')", absPath, link.Text));

                    //string url1 = string.Format("window.parent.UgitOpenPopupDialog('{0}','','Sub Location','600','250',0,'{1}','true')", absPath, Server.UrlEncode(Request.Url.AbsolutePath));
                }
                else if (listName == "messageboard")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=messageboard&mode=config");
                    width = "800";
                    height = "500";
                    isPixel = "true";
                }
                else if (listName == "dmdepartment" || listName == "doctypeinfo" || listName == "documenttype" || listName == "dmprojects" || listName == "dmvendor")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=todo");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','1000','500','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName == "modulestagetemplates")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagetemplates");
                }
                else if (listName == "modulestagerules")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagerulelist");
                }
                else if (listName == DatabaseObjects.Tables.SchedulerActions.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=scheduleactionslist");
                    width = "90";
                    height = "90";
                }
                else if (listName == DatabaseObjects.Tables.UGITTaskTemplates.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=UGITTaskTemplates");
                }
                // added new condition.
                else if (listName == DatabaseObjects.Tables.EventCategories.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=eventcategory");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','600','400','','','true')", absPath, link.Text));
                    return;
                }
                else if (listName.ToLower() == "deletetickets")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=deletetickets");
                }
                else if (listName == DatabaseObjects.Tables.HolidaysAndWorkDaysCalendar.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=holidaycalendarevent");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','90','','')", absPath, link.Text));
                    return;
                }
                else if (listName == "adusermapping")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=adusers");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','620px','600px','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.UGITLog.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=UGITLog");
                }
                else if (listName == DatabaseObjects.Tables.WikiLeftNavigation.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=wikileftnavigation");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','45','55','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.MenuNavigation.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=menunavigationview");
                    height = "90";
                    width = "90";
                }
                else if (listName == DatabaseObjects.Tables.PageEditor.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=pageeditorview");
                    height = "90";
                    width = "90";
                }
                else if (listName == "updatethemecolor")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=updatethemecolor");
                    height = "90";
                    width = "90";
                }
                else if (listName == DatabaseObjects.Tables.UserSkills.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=userskillsview");
                    height = "430px";
                    width = "650px";
                }
                else if (listName == DatabaseObjects.Tables.UserRoles.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=RolesView");
                    height = "430px";
                    width = "560px";
                }
                else if (listName == DatabaseObjects.Tables.adminauth.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=adminauth");
                    height = "200px";
                    width = "500px";
                }
                else if (listName == DatabaseObjects.Tables.setugittheme.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=setugittheme");
                    height = "90";
                    width = "90";

                    string source = Uri.EscapeDataString(Request.Url.AbsolutePath);
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','{2}','{3}', false, '{4}')", absPath, link.Text, width, height, source));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.TicketTemplates.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=TicketTemplatelist");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','600px','600px','','')", absPath, link.Text));
                    return;
                }
                else if (listName == DatabaseObjects.Tables.LinkView.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=linkconfig&viewLink=0");
                }
                else if (listName == DatabaseObjects.Tables.ModuleDefaultValues.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=moduledefaultvalues");
                }
                else if (listName == DatabaseObjects.Tables.Organization.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=organizationview");
                }
                else if (listName == DatabaseObjects.Tables.Contacts.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=contactsview");
                }
                else if (listName == DatabaseObjects.Tables.Emails.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=emailtoticket");
                    height = "225px";
                    width = "500px";
                }
                else if (listName == "smtpmail")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=smtpmailsetting");
                    height = "280px";
                    width = "500px";
                }
                else if (listName == DatabaseObjects.Tables.ResourceManagement.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=resourcemanagement");
                    height = "80";
                    width = "90";
                }
                else if (listName == DatabaseObjects.Tables.EnableMigrate.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=enablemigrate");
                    height = "200px";
                    width = "500px";
                }
                else if (listName == DatabaseObjects.Tables.AssetIntegrationConfiguration.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=assetintegrationconfiguration");
                    height = "200px";
                    width = "500px";
                }
                else if (listName == "editchoicefield")
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=choicefieldedit");
                    height = "200px";
                    width = "500px";
                }
                else if (listName == DatabaseObjects.Tables.VendorType.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=vendortypeview");
                }
                else if (listName == DatabaseObjects.Tables.ModuleStageConstraintTemplates.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagetemplates");
                }
                else if (listName == DatabaseObjects.Tables.LinkItems.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=governanceconfig&param=dashboard");
                }
                else if (listName == DatabaseObjects.Tables.LinkCategory.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=governanceconfig&param=config");

                }
                else if (listName == DatabaseObjects.Tables.AspNetRoles.ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=userrolesview");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','45','55','','')", absPath, link.Text));
                    return;
                }
                else if (listName == Convert.ToString("Config_ProjectLifeCycles").ToLower())
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=projectlifecycles&param=config");
                }
                else if (listName.EqualsIgnoreCase("HelpCard"))
                {
                    link.Attributes.Add("href", string.Format("/pages/HelpCard"));
                    link.Attributes.Add("target", "_blank");
                    return;
                }
                else if(listName.Equals("CheckListTemplates", StringComparison.InvariantCultureIgnoreCase))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=CheckListTemplates");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','90','','')", absPath, link.Text));
                    return;
                }
                else if (listName.Equals("RankingCriteria", StringComparison.InvariantCultureIgnoreCase))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=RankingCriteria");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','60','','')", absPath, link.Text));
                    return;
                }


                else if (listName.Equals("ProcoreUtility", StringComparison.InvariantCultureIgnoreCase))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=ProcoreUtility");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.Equals("ProcoreMapping", StringComparison.InvariantCultureIgnoreCase))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=ProcoreMapping");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.Equals("ProcoreCredentials", StringComparison.InvariantCultureIgnoreCase))                
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=RestAPICredentials");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.Equals("TokenInfo", StringComparison.InvariantCultureIgnoreCase))                
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/RedirectionHandler.aspx");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.Equals("ADAdminAuth", StringComparison.InvariantCultureIgnoreCase))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=adminauth");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.Equals("JobTitle", StringComparison.InvariantCultureIgnoreCase))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=jobtitleview");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','70','','')", absPath, link.Text));
                    return;
                }
                else if (listName.Equals("LeadCriteria", StringComparison.InvariantCultureIgnoreCase))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=leadcriteria");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','60','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.Equals("ProjectComplexity", StringComparison.InvariantCultureIgnoreCase))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=projectcomplexity");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','60','60','','')", absPath, link.Text));
                    return;
                }
                else if(listName.Equals("Roles", StringComparison.InvariantCultureIgnoreCase))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=RolesView");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','60','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("ExperiencedTags"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=ExperiencedTags");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','60','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("UserProjectExperiences"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=UserProjectExperiences");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','60','60','','')", absPath, link.Text));
                    return;
                }
                else if (listName.EqualsIgnoreCase("DataRefresh"))
                {
                    absPath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=DataRefresh");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','60','60','','')", absPath, link.Text));
                    return;
                }
                else
                {
                    //absPath = UGITUtility.GetAbsoluteURL(string.Format("/Lists/{0}/AllItems.aspx?isdlg=1&isudlg=1", listName));
                    //link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','50','','')", absPath, link.Text));
                    //return;
                    absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=todo");
                    link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','70','50','','')", absPath, link.Text));
                    return;
                }

                link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','{2}','{3}', false, '', {4})", absPath, link.Text, width, height, isPixel));
                //htmlAnchor.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','{2}','{3}', false, '', {4})", absPath, link.Text, width, height, isPixel));
            }
        }

        //protected void imgCategory_OnClick(object sender, EventArgs e)
        //{
        //    var btn = (ImageButton)sender;
        //    repeaterForCatagory.Visible = false;
        //    importData.Visible = false;
        //    WorkFlowAdmin work = (WorkFlowAdmin)Page.LoadControl("~/ControlTemplates/Admin/WorkFlowAdmin.ascx");
        //    workFlow.Controls.Add(work);
        //}

        public void RefreshFactable()
        {
            DashboardFactTableManager  ObjDashboard = new DashboardFactTableManager(this._context);
            List<DashboardFactTables> tables = ObjDashboard.Load();
            Task.Run(async () =>
            {
                await Task.FromResult(0);
                foreach (var row in tables)
                {
                    if(row.CacheTable)
                        DashboardCache.RefreshDashboardCache(row.Title, this._context);
                }
            });
        }
    }
}
