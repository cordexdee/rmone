using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Manager;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.UI.HtmlControls;
using uGovernIT.Utility;
using System.Web;
using DevExpress.Web;
using static uGovernIT.Utility.Constants;

namespace uGovernIT.Web
{
    public partial class ServiceCatalog : UserControl
    {
        List<ServiceCategory> categories;
        List<Services> services;
        public string newServiceURL;
        int numServicesPerCategory;
        public bool ShowServiceIcons { get; set; }
        public string ServiceCatalogViewMode { get; set; }
        public int IconSize { get; set; }
        public Unit Height { get; set; }
        public Unit Width { get; set; }
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        ConfigurationVariableManager objConfigurationVariableHelper = null;
        ServiceCategoryManager serviceCategoryManager = null;
        ServicesManager serviceManager = null;
        UserProfileManager userManager = null;
        protected override void OnInit(EventArgs e)
        {
            //Url to open server wizard
            objConfigurationVariableHelper = _context.ConfigManager;
            newServiceURL = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration?control=ServicesWizard&serviceID=")); //UGITUtility.GetAbsoluteURL("/ControlTemplates/uGovernIT/Services/ServiceWizard.ascx?serviceID=");

            //Load config variable which tells how many service which we need to show in each category
            int.TryParse(objConfigurationVariableHelper.GetValue(ConfigConstants.NumServicesPerCategory), out numServicesPerCategory);
            if (numServicesPerCategory <= 0)
                numServicesPerCategory = 5;

            //Hide Service catelog when service module is desable
            ModuleViewManager ObjModuleViewManager = new ModuleViewManager(_context);
            UGITModule serviceRow = ObjModuleViewManager.LoadByName("SVC", true);
            if (serviceRow == null)
                this.Visible = false;
            else
            {
                bool isEnable = UGITUtility.StringToBoolean(serviceRow.EnableModule);
                if (!isEnable)
                    this.Visible = false;
            }

            //Load all services with their categoires
            serviceManager = new ServicesManager(_context);
            categories = serviceManager.ServiceCategories;
            services = serviceManager.LoadCurrentUserServices();


            //Only activated services
            if (services != null && services.Count > 0)
            {
                services = services.Where(x => x.IsActivated && !x.Deleted).ToList();
            }

            //Remove empty categories and order category by itemorder
            if (categories != null && categories.Count > 0 && services != null)
            {
                categories = categories.Where(x => services.Where(y => y.CategoryId == x.ID).Count() > 0).OrderBy(x => x.ItemOrder).ToList();
            }

            #region new Created user
            // if user is have no role minimun authorization

            //var user = HttpContext.Current.CurrentUser();
            //var userRoleList = userManager.GetUserRoles(user.Id);//b

            //if (userRoleList.Count == 0)
            //    categories = categories.Where(x => x.CategoryName == "I Have an Issue").ToList();
            #endregion
            rptSubCategory.DataSource = categories;
            rptSubCategory.DataBind();
            rptlargeserviceicon.DataSource = categories;
            rptlargeserviceicon.DataBind();

            //All Service block
            categories = categories.OrderBy(x => x.CategoryName).ToList();



            foreach (ServiceCategory catry in categories)
            {
                if (!ShowServiceIcons)
                    cmbCategories.Items.Add(new ListEditItem(catry.CategoryName, catry.ID.ToString()));
                else
                    cmbCategories.Items.Add(new ListEditItem(catry.CategoryName, catry.ID.ToString(), UGITUtility.GetAbsoluteURL(Convert.ToString(catry.ImageUrl))));
            }
            cmbCategories.Items.Insert(0, new ListEditItem("--All Services--", ""));

            if (!IsPostBack)
            {
                cmbCategories.SelectedIndex = 0;
                DDLCategories_SelectedIndexChanged(cmbServices, new EventArgs());
            }

            if (services == null || services.Count <= 0)
            {
                pServiceCatelogMain.Visible = false;
            }

            if (ServiceCatalogViewMode == ServiceViewType.ButtonView)
                pServiceCatelogMain.Attributes.Add("class", "servciecatalog-main buttonview-container");

            if (ServiceCatalogViewMode == ServiceViewType.IconView)
            {
                divLargeServiceIcon.Visible = true;
                ServiceCatalog1.Visible = false;
            }
            else
            {
                divLargeServiceIcon.Visible = false;
            }

            populate_services.Visible = ServiceCatalogViewMode == ServiceViewType.DropdownView;
            pServiceCatelogMain.Style.Add("height", Convert.ToString(Height) == string.Empty ? "100%" : Convert.ToString(Height));
            pServiceCatelogMain.Style.Add("width", Convert.ToString(Width) == string.Empty ? "100%" : Convert.ToString(Width));
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            serviceCategoryManager = new ServiceCategoryManager(_context);
            serviceManager = new ServicesManager(_context);
            userManager = new UserProfileManager(_context);
            objConfigurationVariableHelper = _context.ConfigManager;
        }

        protected void DDLCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbServices.Items.Clear();
            List<Services> categoryServices = services;
            if (cmbCategories.Value != null && cmbCategories.SelectedIndex != 0)
            {
                categoryServices = services.Where(x => x.IsActivated && !x.Deleted && x.CategoryId == Convert.ToInt32(cmbCategories.Value)).ToList();
            }

            foreach (Services ser in categoryServices)
            {
                if (!ShowServiceIcons)
                    cmbServices.Items.Add(new ListEditItem(ser.Title, ser.ID.ToString()));
                else
                    cmbServices.Items.Add(new ListEditItem(ser.Title, ser.ID.ToString(), UGITUtility.GetAbsoluteURL(Convert.ToString(ser.ImageUrl))));
            }

            if (ShowServiceIcons)
            {
                cmbServices.ShowImageInEditBox = true;
                cmbServices.ImageUrlField = DatabaseObjects.Columns.ImageUrl;
                cmbServices.ItemImage.Height = 17;
                cmbServices.ItemImage.Width = 17;
                cmbServices.ItemStyle.Height = 24;
            }
            else
            {
                cmbServices.ItemStyle.Height = 18;
            }

            cmbServices.SelectedIndex = 0;
        }

        protected void RptSubCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Panel pCategoryContainer = (Panel)e.Item.FindControl("pCategoryContainer");
                HtmlAnchor anchor = (HtmlAnchor)e.Item.FindControl("moreServiceDisplayLink");
                //hide the ancho tag on UI
                anchor.Visible = false;
                //pCategoryContainer.Style.Add("min-height", string.Format("{0}px", numServicesPerCategory * 30));
                ServiceCategory category = (ServiceCategory)e.Item.DataItem;
                List<Services> serviceList = services.Where(x => x.CategoryId == category.ID).OrderBy(x => x.ItemOrder).ToList();
                if (serviceList.Count > numServicesPerCategory)
                {
                    //show the anchor tag if categeory contain greater value than set in configuration variable
                    anchor.Visible = true;
                }

                HtmlImage imgCategory = (HtmlImage)e.Item.FindControl("imgCategory");
                if (string.IsNullOrEmpty(category.ImageUrl))
                {
                    imgCategory.Visible = false;
                }

                if (ServiceCatalogViewMode == ServiceViewType.ButtonView)
                {
                    pCategoryContainer.CssClass += " categorylist-container";

                    HtmlGenericControl tdCategoryContainer = (HtmlGenericControl)pCategoryContainer.FindControl("tdCategoryContainer");
                    tdCategoryContainer.Attributes.Add("onclick", "javascript: ShowHideListMenu(this,event);");
                }

                Repeater rptItemList = (Repeater)e.Item.FindControl("rptItemList");
                if (rptItemList != null)
                {
                    rptItemList.DataSource = serviceList;
                    rptItemList.DataBind();
                }
            }
        }

        protected void RptItemList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Services svc = (Services)e.Item.DataItem;
                string title = uHelper.ReplaceInvalidCharsInURL(svc.Title);
                LinkButton lnkListName = (LinkButton)e.Item.FindControl("lnkListName");


                if (ServiceCatalogViewMode == ServiceViewType.ButtonView)
                    lnkListName.OnClientClick = "HideServiceMenu();";
                else
                    //lnkListName.ToolTip = Uri.EscapeDataString(svc.ServiceDescription);commented by sayali ...remove encrited text from tooltip
                    lnkListName.ToolTip = svc.ServiceDescription;


                lnkListName.CssClass = "jqtooltip";
                lnkListName.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog(\"{0}{2}\", \"\", \"{1}\", 90, 90, false,\"{3}\")", newServiceURL, title, svc.ID, Uri.EscapeUriString(Request.Url.AbsolutePath)));

                HtmlGenericControl trServiceItem = (HtmlGenericControl)e.Item.FindControl("trServiceItem");
                // HtmlTableRow trServiceItem = (HtmlTableRow)e.Item.FindControl("trServiceItem");
                if (e.Item.ItemIndex >= numServicesPerCategory && ServiceCatalogViewMode == ServiceViewType.ListView)
                {
                    trServiceItem.Style.Add(HtmlTextWriterStyle.Display, "none");
                    trServiceItem.Attributes.Add("class", "expandableservice");
                }
            }
        }

        protected void rptlargeserviceicon_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ServiceCategory category = (ServiceCategory)e.Item.DataItem;
                List<Services> serviceList = services.Where(x => x.CategoryId == category.ID).OrderBy(x => x.ItemOrder).ToList();

                Repeater rptItemList = (Repeater)e.Item.FindControl("rptServiceIcon");
                if (rptItemList != null)
                {
                    rptItemList.DataSource = serviceList;
                    rptItemList.DataBind();
                }
            }
        }

        protected void rptServiceIcon_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Services svc = (Services)e.Item.DataItem;
                string title = uHelper.ReplaceInvalidCharsInURL(svc.Title);

                HtmlImage imgServiceIcon = e.Item.FindControl("imgServiceIcon") as HtmlImage;
                imgServiceIcon.Attributes.Add("src", UGITUtility.GetAbsoluteURL(svc.ImageUrl));
                imgServiceIcon.Attributes.Add("style", "max-height:" + IconSize + "px; max-width:" + IconSize + "px;");
                Panel pServiceIconContainer = e.Item.FindControl("pServiceIconContainer") as Panel;
                pServiceIconContainer.Attributes.Add("style", "cursor: pointer");
                pServiceIconContainer.Attributes.Add("onclick", string.Format("javascript:UgitOpenPopupDialog(\"{0}{2}\", \"\", \"{1}\", 90, 90, false,\"{3}\");return false;", newServiceURL, title, svc.ID, Uri.EscapeUriString(Request.Url.AbsolutePath)));
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {

        }
    }
}
