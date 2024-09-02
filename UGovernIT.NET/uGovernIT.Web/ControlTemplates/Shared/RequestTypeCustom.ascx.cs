using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using uGovernIT.Helpers;
using System.Data;
using uGovernIT.Manager;
using DevExpress.Web;
using System.Linq.Expressions;
using DevExpress.XtraScheduler.iCalendar.Components;

namespace uGovernIT.Web
{
    public partial class RequestTypeCustom : System.Web.UI.UserControl
    {
        RequestTypeManager objRequestTypeManager = new RequestTypeManager(HttpContext.Current.GetManagerContext());
        List<ModuleRequestType> lstModuleRequestType;
        public string DisplayName { get; set; }

        public string ModuleName { get; set; }
        public string SetValueCheck { get; set; }
        public ASPxDropDownEdit DropDownEdit { get; set; }
        public RequestTypeCustomProperties CustomProperties { get; set; }
        public ModuleViewManager ModuleViewManagerObj;
        UGITModule ugitmodule;
        public RequestTypeCustom()
        {
            CustomProperties = new RequestTypeCustomProperties();
        }
        protected override void OnInit(EventArgs e)
        {
            if (CustomProperties == null)
                CustomProperties = new RequestTypeCustomProperties();

            ModuleViewManagerObj = new ModuleViewManager(HttpContext.Current.GetManagerContext());

            //requestTypeGrid.ClientInstanceName = GetUserControlSpecificId("_requestTypeGrid");
            //categoryDropDown.ClientInstanceName = GetUserControlSpecificId("_categoryDropDown");
            //subcategoryDropDown.ClientInstanceName = GetUserControlSpecificId("_subcategoryDropDown");
            //cbIssueType.ClientInstanceName = GetUserControlSpecificId("_cbIssueType");
            //btnOk.ClientInstanceName = GetUserControlSpecificId("_btnOk");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(DisplayName))
            {
                requestTypeGrid.Columns[DatabaseObjects.Columns.TicketRequestType].Caption = DisplayName;
            }
            string customParameters = string.Empty;
            requestTypeGrid.SettingsPager.Mode = GridViewPagerMode.EndlessPaging;
            requestTypeGrid.Settings.VerticalScrollableHeight = 200;

            Expression<Func<ModuleRequestType, bool>> where = x => x.ModuleNameLookup == ModuleName;
            if (CustomProperties != null && CustomProperties.RequestTypesList != null && CustomProperties.RequestTypesList.Count > 0)
            {
                customParameters = string.Join<string>(Constants.Separator6, CustomProperties.RequestTypesList);
                if(customParameters.ToLower() != "all")
                    where = x => x.ModuleNameLookup == ModuleName && customParameters.Contains(x.ID.ToString());
                else
                    where = x => x.ModuleNameLookup == ModuleName;
            }

            ugitmodule = ModuleViewManagerObj.LoadByName(ModuleName);
            if (ugitmodule != null)
            {
                if (CustomProperties != null && CustomProperties.RequestTypesList != null)
                    lstModuleRequestType = ugitmodule.List_RequestTypes?.Where(x => x.ModuleNameLookup == ModuleName && x.Deleted == false && customParameters.Contains(x.ID.ToString())).ToList();
                else
                    lstModuleRequestType = ugitmodule.List_RequestTypes?.Where(x => x.ModuleNameLookup == ModuleName  && x.Deleted == false).ToList();
            }
            else
            {
                lstModuleRequestType = objRequestTypeManager.Load(where);
            }
            if (CustomProperties != null)
            {
                if (CustomProperties.enableissuetypedropdown)
                {
                    requestTypeGrid.ClientSideEvents.SelectionChanged = string.Format("function(s, e){{ onGridSelectionChanged_IssueType(s, e); }}");
                    trIssueType.Visible = true;
                    BindIssueTypeDropdown();
                }
                else
                {
                    requestTypeGrid.ClientSideEvents.SelectionChanged = string.Format("function(s,e){{ onGridSelectionChanged_requesttype(s,e); }}");
                    trIssueType.Visible = false;
                }
            }
            if (lstModuleRequestType != null && lstModuleRequestType.Count() > 0)
            {
                List<string> listcategory = lstModuleRequestType.Select(x => x.Category).Distinct().OrderBy(x => x).ToList();
                List<string> listrequestCategory = lstModuleRequestType.Where(x => x.SubCategory != null).Select(x => x.SubCategory).Distinct().OrderBy(x => x).ToList();
                if (listcategory.Count == 0 && listrequestCategory.Count > 0)
                {
                    categoryDropDown.ClientVisible = false;
                    requestTypeGrid.Columns[DatabaseObjects.Columns.Category].Visible = false;
                    requestTypeGrid.Columns[DatabaseObjects.Columns.SubCategory].Visible = true;
                    requestTypeGrid.Columns[DatabaseObjects.Columns.SubCategory].Caption = "Category";
                    listrequestCategory.ForEach(x => subcategoryDropDown.Items.Add(x));
                }
                if (listrequestCategory.Count == 0)
                {
                    subcategoryDropDown.ClientVisible = false;
                    requestTypeGrid.Columns[DatabaseObjects.Columns.SubCategory].Visible = false;
                }
                if (listcategory.Count > 0)
                {
                    categoryDropDown.Items.Clear();
                    categoryDropDown.Items.Add("-- All Categories --", null);
                    listcategory.ForEach(x => categoryDropDown.Items.Add(x));
                    categoryDropDown.DataBind();
                    categoryDropDown.ClientVisible = true;
                    subcategoryDropDown.ClientVisible = false;
                    requestTypeGrid.Columns[DatabaseObjects.Columns.Category].Visible = true;
                    //Session["RequestType_Category"] = null;
                    categoryDropDown.SelectedIndex = 0;
                }

                var subCategories = lstModuleRequestType.Where(x => x.Category != null).Select(x => x.SubCategory).Distinct().OrderBy(x => x).ToList();
                if (subCategories != null && subCategories.Count > 0)
                {
                    subcategoryDropDown.Items.Clear();
                    subcategoryDropDown.Items.Add("-- All Sub-Categories --", null);
                    subCategories.ForEach(x => subcategoryDropDown.Items.Add(x));
                    subcategoryDropDown.DataBind();
                    subcategoryDropDown.SelectedIndex = 0;
                    //Session["RequestType_Subcategory"] = null;
                    //subcategoryDropDown.ClientVisible = true;
                    requestTypeGrid.Columns[DatabaseObjects.Columns.SubCategory].Visible = true;
                }
                else
                {
                    subcategoryDropDown.Items.Clear();
                    subcategoryDropDown.ClientVisible = false;
                    requestTypeGrid.Columns[DatabaseObjects.Columns.SubCategory].Visible = false;
                }

            }
            if (!IsPostBack)
            {
                Session["RequestType_Category"] = null;
                Session["RequestType_Subcategory"] = null;
                //requestTypeGrid.DataBind();
            }
                    
        }
        protected void subcategoryDropDown_Callback(object source, CallbackEventArgsBase e)
        {
            // Commented below code & fetching CATEGORY/SUBCATEGORY from parameter as RequestType dropdown is
            // not working for 'Report a Problem' Service 

            string[] param = e.Parameter.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            //FillSubCategory(e.Parameter);
            //Session["RequestType_Category"] = e.Parameter;
            FillSubCategory(param[1]);
            Session["RequestType_Category"] = param[1];
            requestTypeGrid.DataBind();
            
        }
        protected void FillSubCategory(string category)
        {
            if (string.IsNullOrEmpty(category))
                return;
            if (lstModuleRequestType != null && lstModuleRequestType.Count() > 0)
            {
                if (lstModuleRequestType.Where(x => x.Category == category).Count() > 0)
                {
                    subcategoryDropDown.Items.Clear();
                    List<string> listSubcategory = lstModuleRequestType.Where(x => x.Category == category && x.SubCategory != string.Empty).Select(x => x.SubCategory).Distinct().OrderBy(x=>x).ToList().Where(x=> x!=null).ToList();
                    if (listSubcategory.Count() > 0)
                    {
                        subcategoryDropDown.Items.Add("-- All Sub-Categories --", null);
                        listSubcategory.ForEach(x => subcategoryDropDown.Items.Add(x));
                        subcategoryDropDown.SelectedIndex = 0;
                        Session["RequestType_Subcategory"] = null;
                    }
                    else
                    {
                        subcategoryDropDown.Items.Clear();
                        subcategoryDropDown.Items.Add("-- All Sub-Categories --", null);
                        Session["RequestType_Subcategory"] = null;
                    }
                }
            }
        }

        protected void requestTypeGrid_DataBinding(object sender, EventArgs e)
        {
            string categoryDropDownSession = string.Empty;
            string subcategoryDropDownSession = string.Empty;
            if (HttpContext.Current.Session["RequestType_Category"] != null && UGITUtility.ObjectToString(Session["RequestType_Category"]) != "null")
            {
                categoryDropDownSession = HttpContext.Current.Session["RequestType_Category"] as string;
            }
            if (HttpContext.Current.Session["RequestType_Subcategory"] != null && UGITUtility.ObjectToString(Session["RequestType_Subcategory"]) != "null")
            {
                subcategoryDropDownSession = HttpContext.Current.Session["RequestType_Subcategory"] as string;
            }
            if(ugitmodule == null && !string.IsNullOrEmpty(ModuleName))
                ugitmodule = ModuleViewManagerObj.LoadByName(ModuleName);
            
            if (requestTypeGrid.DataSource != null && string.IsNullOrWhiteSpace(categoryDropDownSession) && string.IsNullOrWhiteSpace(subcategoryDropDownSession))
                lstModuleRequestType = requestTypeGrid.DataSource as List<ModuleRequestType>;
            else
            {
                if (CustomProperties != null && CustomProperties.RequestTypesList != null && CustomProperties.RequestTypesList.Count > 0)
                {
                    string customParameters = string.Empty;
                    Expression<Func<ModuleRequestType, bool>> where = x => x.ModuleNameLookup == ModuleName;
                    customParameters = string.Join<string>(Constants.Separator6, CustomProperties.RequestTypesList);
                    if (ugitmodule != null)
                    {
                        if (customParameters.ToLower() != "all")
                            lstModuleRequestType = ugitmodule.List_RequestTypes.Where(x => x.ModuleNameLookup == ModuleName && x.Deleted == false && customParameters.Contains(x.ID.ToString())).ToList();
                        else
                            lstModuleRequestType = ugitmodule.List_RequestTypes.Where(x => x.ModuleNameLookup == ModuleName && x.Deleted == false).ToList();
                    }
                    else
                        lstModuleRequestType = objRequestTypeManager.Load(x => customParameters.Contains(x.ID.ToString()));

                }
                else
                {
                    if (ugitmodule != null)
                        //lstModuleRequestType.Clear();
                        lstModuleRequestType = ugitmodule.List_RequestTypes.Where(x => x.ModuleNameLookup == ModuleName && x.Deleted == false).ToList(); 
                }
            }
                
               // subcategoryDropDown.Visible = false;

            requestTypeGrid.DataSource = null;
            if (!string.IsNullOrEmpty(Convert.ToString(categoryDropDownSession)) && !string.IsNullOrEmpty(Convert.ToString(subcategoryDropDownSession)))
             {
                if (lstModuleRequestType != null && lstModuleRequestType.Count() > 0)
                {
                    requestTypeGrid.DataSource = lstModuleRequestType.Where(x => x.Category == Convert.ToString(categoryDropDownSession) && x.SubCategory == Convert.ToString(subcategoryDropDownSession)).ToList();
                    requestTypeGrid.Columns[DatabaseObjects.Columns.Category].Visible = false;
                    requestTypeGrid.Columns[DatabaseObjects.Columns.SubCategory].Visible = false;
                }
            }
            else if (!string.IsNullOrEmpty(Convert.ToString(subcategoryDropDownSession)))
            {

                if (lstModuleRequestType != null && lstModuleRequestType.Count() > 0)
                {
                    requestTypeGrid.DataSource = lstModuleRequestType.Where(x => x.SubCategory == Convert.ToString(subcategoryDropDownSession)).ToList();
                    requestTypeGrid.Columns[DatabaseObjects.Columns.SubCategory].Visible = false;
                }
            }
            else if(!string.IsNullOrEmpty(Convert.ToString(categoryDropDownSession)))
            {
                if (lstModuleRequestType != null && lstModuleRequestType.Count() > 0)
                {
                    requestTypeGrid.DataSource = lstModuleRequestType.Where(x => x.Category == Convert.ToString(categoryDropDownSession)).ToList();
                    requestTypeGrid.Columns[DatabaseObjects.Columns.Category].Visible = false;
                    List<string> subcategory = lstModuleRequestType.Where(x => x.Category == Convert.ToString(categoryDropDownSession) && !string.IsNullOrEmpty(x.SubCategory)).Select(x => x.SubCategory).Distinct().ToList();
                    if (subcategory.Count < 1)
                    {
                        requestTypeGrid.Columns[DatabaseObjects.Columns.SubCategory].Visible = false;
                        subcategoryDropDown.Items.Clear();
                    }
                    else
                        requestTypeGrid.Columns[DatabaseObjects.Columns.SubCategory].Visible = true;
                }
            }
            else
            {
                if (lstModuleRequestType != null && lstModuleRequestType.Count() > 0)
                {
                    requestTypeGrid.DataSource = lstModuleRequestType.ToList();
                    List<string> listcategory = lstModuleRequestType.Select(x => x.Category).Distinct().OrderBy(x => x).ToList();
                    List<string> listrequestCategory = lstModuleRequestType.Where(x => x.SubCategory != null).Select(x => x.SubCategory).Distinct().OrderBy(x => x).ToList();
                    if (listcategory.Count == 0 && listrequestCategory.Count > 0)
                    {
                        categoryDropDown.ClientVisible = false;
                        requestTypeGrid.Columns[DatabaseObjects.Columns.Category].Visible = false;
                        requestTypeGrid.Columns[DatabaseObjects.Columns.SubCategory].Visible = true;
                        requestTypeGrid.Columns[DatabaseObjects.Columns.SubCategory].Caption = "Category";
                        listrequestCategory.ForEach(x => subcategoryDropDown.Items.Add(x));
                    }
                    if (listrequestCategory.Count == 0)
                    {
                        subcategoryDropDown.ClientVisible = false;
                        requestTypeGrid.Columns[DatabaseObjects.Columns.SubCategory].Visible = false;
                    }
                    if (listcategory.Count > 0)
                    {
                        categoryDropDown.Items.Clear();
                        categoryDropDown.Items.Add("-- All Categories --", null);
                        listcategory.ForEach(x => categoryDropDown.Items.Add(x));
                        categoryDropDown.DataBind();
                        categoryDropDown.ClientVisible = true;
                        subcategoryDropDown.ClientVisible = false;
                        requestTypeGrid.Columns[DatabaseObjects.Columns.Category].Visible = true;
                        Session["RequestType_Category"] = null;
                        categoryDropDown.SelectedIndex = 0;
                    }

                    var subCategories = lstModuleRequestType.Where(x => x.Category != null).Select(x => x.SubCategory).Distinct().OrderBy(x => x).ToList();
                    if (subCategories != null && subCategories.Count > 0)
                    {
                        subcategoryDropDown.Items.Clear();
                        subcategoryDropDown.Items.Add("-- All Sub-Categories --", null);
                        subCategories.ForEach(x => subcategoryDropDown.Items.Add(x));
                        subcategoryDropDown.DataBind();
                        subcategoryDropDown.SelectedIndex = 0;
                        Session["RequestType_Subcategory"] = null;
                        //subcategoryDropDown.ClientVisible = true;
                        requestTypeGrid.Columns[DatabaseObjects.Columns.SubCategory].Visible = true;
                    }
                    else
                    {
                        subcategoryDropDown.Items.Clear();
                        subcategoryDropDown.ClientVisible = false;
                        requestTypeGrid.Columns[DatabaseObjects.Columns.SubCategory].Visible = false;
                    }                   
                }
            }
         
            //hide unhide sub category
            if(subcategoryDropDown.Items == null || subcategoryDropDown.Items.Count <= 1)
            {
                requestTypeGrid.Columns[DatabaseObjects.Columns.SubCategory].Visible = false;
                subcategoryDropDown.ClientVisible = false;
            }
            else
            {
                if (!string.IsNullOrEmpty(subcategoryDropDownSession))
                {
                    requestTypeGrid.Columns[DatabaseObjects.Columns.SubCategory].Visible = false;
                    requestTypeGrid.Columns[DatabaseObjects.Columns.Category].Visible = false;
                    subcategoryDropDown.ClientVisible = true;
                }
                else
                {
                    if (!string.IsNullOrEmpty(categoryDropDownSession))
                    {
                        requestTypeGrid.Columns[DatabaseObjects.Columns.SubCategory].Visible = true;
                        requestTypeGrid.Columns[DatabaseObjects.Columns.Category].Visible = false;
                        subcategoryDropDown.ClientVisible = false;
                    }
                    else
                    {
                        requestTypeGrid.Columns[DatabaseObjects.Columns.Category].Visible = true;
                    }
                }
            }
        }

        protected void requestTypeGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            // Commented below code & fetching CATEGORY/SUBCATEGORY from parameter as RequestType dropdown is
            // not working for 'Report a Problem' Service 

            if (subcategoryDropDown.Value != null)
                Session["RequestType_Subcategory"] = subcategoryDropDown.Value;
            else if (categoryDropDown.Value == null)
            {
                //Session["RequestType_Category"] = null;
                Session["RequestType_Subcategory"] = null;
            }

            string[] param = e.Parameters.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (!string.IsNullOrEmpty(e.Parameters))
            {
                if (param.Length > 1)
                {
                    if (param[0].EqualsIgnoreCase("CATEGORY"))
                    {
                        Session["RequestType_Category"] = param[1];
                    }
                    if (param[0].EqualsIgnoreCase("SUBCATEGORY"))
                    {
                        Session["RequestType_Subcategory"] = param[1];
                    }
                }
                else
                {
                    Session["RequestType_Category"] = param[0];
                }
            }
            requestTypeGrid.DataBind(); 
        }
        public string GetValues()
        {
            string value =  string.Join(",", this.requestTypeGrid.GetSelectedFieldValues(requestTypeGrid.KeyFieldName).ToList());
            return value;
        }
        public void SetValues(string value)
        {
            //RequestTypeManager requestTypeManager = new RequestTypeManager(HttpContext.Current.GetManagerContext());
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            UGITModule uGITModule = moduleViewManager.LoadByName(ModuleName);
            List<string> inputValues;
            if (requestTypeGrid.DataSource == null)
                requestTypeGrid.DataBind();           
            if (!string.IsNullOrEmpty(value))
            {
                List<string> issueTypes = new List<string>();
                inputValues = UGITUtility.ConvertStringToList(value, Constants.Separator);
                //requestTypeGrid.Selection.SetSelectionByKey(inputValues[0], true);
                ModuleRequestType moduleRequestType = uGITModule.List_RequestTypes.Where(x => x.ID == Convert.ToInt32(inputValues[0])).FirstOrDefault();//requestTypeManager.LoadByID(Convert.ToInt32(inputValues[0]));
                if (moduleRequestType == null)
                    return;

                DropDownEdit.Value = inputValues[0];
                DropDownEdit.KeyValue = inputValues[0];
                string title = string.Empty;
                DropDownEdit.Text = moduleRequestType.Title;
                if (inputValues.Count > 1)
                {
                    DropDownEdit.Text = title + " > " + inputValues[1];
                    if (!string.IsNullOrEmpty(moduleRequestType.IssueTypeOptions))
                        issueTypes = UGITUtility.ConvertStringToList(moduleRequestType.IssueTypeOptions, Constants.Separator);
                    else
                    {
                        
                        //string moduleTable = moduleViewManager.GetModuleTableName(ModuleName);
                        FieldConfigurationManager fieldManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                        FieldConfiguration field = fieldManager.GetFieldByFieldName(DatabaseObjects.Columns.IssueTypeChoice, uGITModule.ModuleTable);
                        if (field != null)
                        {
                            issueTypes = UGITUtility.ConvertStringToList(field.Data, Constants.Separator);
                        }
                    }
                    cbIssueType.DataSource = issueTypes;
                    cbIssueType.DataBind();
                    cbIssueType.SelectedIndex = cbIssueType.Items.IndexOfText(inputValues[1]);
                }

                ListEditItem categoryItem = categoryDropDown.Items.FindByValue(moduleRequestType.Category);
                if (categoryItem != null)
                {
                    categoryDropDown.SelectedIndex = categoryDropDown.Items.IndexOf(categoryItem);
                    Session["RequestType_Category"] = categoryItem.Text;
                    //List<ModuleRequestType> lstSubCategory = objRequestTypeManager.Load(x => x.Category == categoryItem.Text && x.ModuleNameLookup == ModuleName && !string.IsNullOrEmpty(x.SubCategory))?.OrderBy(x => x.SubCategory).ToList();
                    List<ModuleRequestType> lstSubCategory = uGITModule.List_RequestTypes.Where(x => x.Category == categoryItem.Text && x.ModuleNameLookup == ModuleName && !string.IsNullOrEmpty(x.SubCategory))?.OrderBy(x => x.SubCategory).ToList();
                    if (lstSubCategory.Count > 0)
                    {
                        subcategoryDropDown.Items.Clear();
                        subcategoryDropDown.DataSource = lstSubCategory.GroupBy(x => x.SubCategory, (key, group) => group.First());
                        subcategoryDropDown.DataBind();
                        subcategoryDropDown.Items.Insert(0, new ListEditItem("-- All Sub-Categories --", null));
                        subcategoryDropDown.SelectedIndex = subcategoryDropDown.Items.IndexOf(subcategoryDropDown.Items.FindByValue(moduleRequestType.SubCategory));
                        Session["RequestType_Subcategory"] = moduleRequestType.SubCategory;
                    }
                    else
                    {
                        subcategoryDropDown.Items.Clear();
                        Session["RequestType_Subcategory"] = moduleRequestType.SubCategory;
                    }
                }
                requestTypeGrid.FocusedRowIndex = requestTypeGrid.FindVisibleIndexByKeyValue(Convert.ToInt32(inputValues[0]));
                requestTypeGrid.DataBind();
            }

        }

        protected void cbIssueType_Callback(object sender, CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
            {
                ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                List<string> issueTypes = new List<string>();
                UGITModule uGITModule = moduleViewManager.LoadByName(ModuleName);
                //ModuleRequestType requestType = objRequestTypeManager.LoadByID(Convert.ToInt32(e.Parameter));
                ModuleRequestType requestType= uGITModule.List_RequestTypes?.Where(x => x.ID == Convert.ToInt32(e.Parameter) && x.Deleted == false).FirstOrDefault();
                if (!string.IsNullOrEmpty(requestType.IssueTypeOptions))
                    issueTypes = UGITUtility.ConvertStringToList(requestType.IssueTypeOptions, Constants.Separator);
                else
                {
                    FieldConfigurationManager fieldManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                    FieldConfiguration field = fieldManager.GetFieldByFieldName(DatabaseObjects.Columns.IssueTypeChoice, uGITModule.ModuleTable);
                    if(field != null)
                    {
                        issueTypes = UGITUtility.ConvertStringToList(field.Data, Constants.Separator);
                    }
                }
                cbIssueType.DataSource = issueTypes;
                cbIssueType.DataBind();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            
        }

        protected override void OnPreRender(EventArgs e)
        {
            SetValues(SetValueCheck);
            base.OnPreRender(e);
        }

        protected string GetUserControlSpecificId(string elementId)
        {
            return this.ClientID + elementId;
        }
        
        private void BindIssueTypeDropdown()
        {
            List<string> issueTypes = new List<string>();
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            UGITModule uGITModule = moduleViewManager.LoadByName(ModuleName);
            FieldConfigurationManager fieldManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
            FieldConfiguration field = fieldManager.GetFieldByFieldName(DatabaseObjects.Columns.IssueTypeChoice, uGITModule.ModuleTable);
            if (field != null)
            {
                issueTypes = UGITUtility.ConvertStringToList(field.Data, Constants.Separator);
            }
            cbIssueType.DataSource = issueTypes;
            cbIssueType.DataBind();
        }
    }
}