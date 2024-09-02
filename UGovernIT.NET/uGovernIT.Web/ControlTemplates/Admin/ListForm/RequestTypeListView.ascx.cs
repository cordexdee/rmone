using DevExpress.Spreadsheet;
using DevExpress.Web.ASPxTreeList;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
namespace uGovernIT.Web
{
    public partial class RequestTypeListView : UserControl
    {

        //SPList spList;
        string moduleName = string.Empty;
        private string subCategory = string.Empty;
        private string category = string.Empty;
        private string FormTitle = "Request Type";
        private string ViewParam = "requesttype";
        private string NewParam = "requestedit";
        private string EditParam = "requestedit";
        private string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}&module={2}";
        private string absoluteUrlView = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&module={2}&showdelete={3}";
        private string absoluteUrlImport = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&listName={1}";

        private ApplicationContext _context = null;
        private ConfigurationVariableManager _configurationVariableManager = null;
        private FieldConfigurationManager _fieldConfigurationManager = null;
        private RequestTypeManager _requestTypeManager = null;
        private RequestTypeByLocationManager _requestTypeByLocationManager = null;

        protected string selectedIDs = string.Empty;
        protected string importUrl;
        protected string moveToProductionUrl = "";
        protected string addNewItem, editItem;

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
                if (_configurationVariableManager == null)
                {
                    _configurationVariableManager = new ConfigurationVariableManager(ApplicationContext);
                }
                return _configurationVariableManager;
            }
        }

        protected FieldConfigurationManager FieldConfigurationManager
        {
            get
            {
                if (_fieldConfigurationManager == null)
                {
                    _fieldConfigurationManager = new FieldConfigurationManager(ApplicationContext);
                }
                return _fieldConfigurationManager;
            }
        }

        protected RequestTypeManager RequestTypeManager
        {
            get
            {
                if (_requestTypeManager == null)
                {
                    _requestTypeManager = new RequestTypeManager(ApplicationContext);
                }
                return _requestTypeManager;
            }
        }

        protected RequestTypeByLocationManager RequestTypeByLocationManager
        {
            get
            {
                if (_requestTypeByLocationManager == null)
                {
                    _requestTypeByLocationManager = new RequestTypeByLocationManager(ApplicationContext);
                }
                return _requestTypeByLocationManager;
            }
        }

        List<ModuleRequestType> requestTypeData = new List<ModuleRequestType>();
        List<ModuleRequestType> requestTypeList = new List<ModuleRequestType>();
        List<ModuleRequestTypeLocation> requestTypeLocation;
        ModuleRequestType requestType;
        object dataTable = new DataTable(typeof(ModuleRequestType).Name);


        /// <summary>
        /// initialize module and grid when collapse and Expand
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {

            //objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            if (ConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableAdminImport))
                btnImport.Visible = true;
            BindModule();
            moduleName = ddlModule.SelectedValue;

            importUrl = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlImport, "importexcelfile", "RequestType"));
            if (Request["module"] != null)
            {
                ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(Request["module"]));
            }
            if (Request["showdelete"] != null)
            {
                chkShowDeleted.Checked = Convert.ToString(Request["showdelete"]) == "0" ? false : true;
            }

            BindData();
            EnableMigrate();
            base.OnInit(e);
        }

        private void EnableMigrate()
        {
            if (ConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableMigrate))
            {
                btnMigrateRequestType.Visible = true;
            }
        }

        /// <summary>
        /// Bind Module dropdown
        /// </summary>
        private void BindModule()
        {
            List<string> requestTypeModuleList = new List<string>();
            if (!string.IsNullOrWhiteSpace(Request[hdnModuleList.UniqueID]))
            {
                requestTypeModuleList = UGITUtility.ConvertStringToList(Request[hdnModuleList.UniqueID], ",");
            }
            else
            {
                List<ModuleRequestType> requestTypeModules = RequestTypeManager.Load();

                if (requestTypeModules != null && requestTypeModules.Count > 0)
                    requestTypeModuleList = requestTypeModules.Where(x => x.ModuleNameLookup != null)
                        .Select(x => x.ModuleNameLookup).Distinct().ToList();

                hdnModuleList.Value = string.Join(",", requestTypeModuleList);
            }
            ddlModule.Items.Clear();

            ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            List<UGITModule> ModuleList = ObjModuleViewManager.Load(x => x.EnableModule).OrderBy(x => x.ModuleName).ToList();
            if (ModuleList != null && requestTypeModuleList.Count > 0)
            {
                if (ModuleList.Count > 0)
                {
                    string selectRequestModuleList = string.Join(",", requestTypeModuleList.Select(x => "'" + x + "'"));
                    ModuleList = ModuleList.Where(x => selectRequestModuleList.Contains(x.ModuleName.ToString())).ToList();

                    ddlModule.DataSource = ModuleList;
                    ddlModule.DataTextField = DatabaseObjects.Columns.Title;
                    ddlModule.DataValueField = DatabaseObjects.Columns.ModuleName;
                    ddlModule.DataBind();
                    ddlModule.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// load event for data which are initialized once and Export Excel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // condition for download excel...
            //if (Request["exportType"] == "excel")
            //{
            //    List<int> selectedRequestTypes = aspxtreelist.GetSelectedNodes().Select(x => Convert.ToInt32(x.Key)).ToList();
            //    DataTable list = RequestTypeManager.GetDataTable();
            //    //DataRow[] coll = null;
            //    //coll = list.Select(string.Format("{0} in ({1})", DatabaseObjects.Columns.ID, string.Join(",", selectedRequestTypes.Select(x => "'" + x + "'"))));
            //    //DataTable dt = new DataTable();
            //    //dt = coll.CopyToDataTable();
            //    DataTable table = ExportListToExcel.GetDataTableFromList(list, ApplicationContext);
            //    var worksheet = ASPxSpreadsheet1.Document.Worksheets.ActiveWorksheet;
            //    worksheet.Import(table, true, 0, 0);
            //    MemoryStream st = new MemoryStream();
            //    ASPxSpreadsheet1.Document.SaveDocument(st, DocumentFormat.OpenXml);
            //    Response.Clear();
            //    Response.ContentType = "application/force-download";
            //    Response.AddHeader("content-disposition", "attachment; filename=RequestType.xlsx");
            //    Response.BinaryWrite(st.ToArray());
            //    Response.End();
            //}
            //else if (Request["exportType"] == "xml")
            //{
            //    DataTable list = RequestTypeManager.GetDataTable();
            //    DataTable table = ExportListToExcel.GetDataTableFromList(list, ApplicationContext);
            //    DataSet dS = new DataSet();
            //    dS.DataSetName = "RecordSet";
            //    dS.Tables.Add(table);
            //    StringWriter sw = new StringWriter();
            //    dS.WriteXml(sw, XmlWriteMode.IgnoreSchema);
            //    string s = sw.ToString();
            //    string attachment = "attachment; filename=RequestType.xml";
            //    Response.ClearContent();
            //    Response.ContentType = "application/xml";
            //    Response.AddHeader("content-disposition", attachment);
            //    Response.Write(s);
            //    Response.End();
            //}
            //else
            //{
            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.NewParam, "0", ddlModule.SelectedValue));
            string jsFunc = string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','653','900',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), this.FormTitle);
            AddItem.ClientSideEvents.Click = "function(s, e) { " + jsFunc + " }";
            AddItem_Top.ClientSideEvents.Click = "function(s, e) { " + jsFunc + " }";
            AddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','653','900',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), this.FormTitle));
            // }

            moveToProductionUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=movestagetoproduction&module={0}&list={1}", ddlModule.SelectedValue, DatabaseObjects.Tables.RequestType));

        }


        /// <summary>
        /// pre-render to bind treeview List 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            if (chkShowDeleted.Checked)
            {
                lnkUnDeleteItem_Top.Visible = true;
                lnkUnDeleteItem_Bottom.Visible = true;
            }
        }

        /// <summary>
        /// bind grid function to get filtered data from Requesttype List according to selected module. 
        /// </summary>
        /// <param name="SelectedItem">Selected module name</param>
        private void GenerateData(string SelectedItem)
        {

            List<ModuleRequestType> requestTypeModules = RequestTypeManager.Load(x => x.ModuleNameLookup.Equals(SelectedItem));
            if (requestTypeModules != null && requestTypeModules.Count > 0)
            {

                if (!chkShowDeleted.Checked)
                {
                    requestTypeModules = requestTypeModules.Where(x => x.Deleted == false).ToList();
                }
                if (requestTypeModules != null && requestTypeModules.Count > 0)
                {
                    List<ModuleRequestType> tempRequestTypes = requestTypeModules;
                    tempRequestTypes = tempRequestTypes.OrderBy(x => x.Category).ThenBy(y => y.SubCategory).ThenBy(z => z.RequestType).ToList();



                    foreach (var requestType in requestTypeModules)
                    {
                        requestType.ItemID = string.Format("{0}", requestType.ID);
                        requestType.ParentID = string.Format("{0}_{1}", requestType.Category, requestType.SubCategory);
                        if (string.IsNullOrWhiteSpace(Convert.ToString(requestType.SubCategory)))
                        {
                            requestType.ParentID = string.Format("{0}", requestType.Category);
                        }
                    }


                    var categoryList = tempRequestTypes.Select(x => x.Category).Distinct().ToList();

                    if (categoryList != null)
                    {
                        ModuleRequestType requestType = null;
                        foreach (var category in categoryList)
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(category)))
                            {

                                requestType = new ModuleRequestType();
                                requestType.ItemID = string.Format("{0}", category);
                                requestType.RequestType = string.Format("Category: {0}", category);
                                requestType.ParentID = "";
                                requestType.Category = category;
                                tempRequestTypes.Add(requestType);
                                requestType = null;
                            }
                        }
                    }
                    var subCategoryList = tempRequestTypes.Select(x => new { x.Category, x.SubCategory }).Distinct().ToList();

                    if (subCategoryList != null)
                    {
                        ModuleRequestType requestType = null;
                        foreach (var subCategory in subCategoryList)
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(subCategory.SubCategory)))
                            {

                                requestType = new ModuleRequestType();
                                requestType.ItemID = string.Format("{0}_{1}", subCategory.Category, subCategory.SubCategory);
                                requestType.RequestType = string.Format("Sub Category: {0}", subCategory.SubCategory);
                                requestType.ParentID = string.Format("{0}", subCategory.Category);
                                requestType.SubCategory = subCategory.SubCategory;
                                requestType.Category = subCategory.Category;
                                tempRequestTypes.Add(requestType);
                                requestType = null;
                            }
                        }
                    }

                    tempRequestTypes.OrderBy(x => x.Category).ThenBy(y => y.SubCategory).ThenBy(z => z.RequestType).ToList();
                    requestTypeData = tempRequestTypes;
                }
            }
            AddItem.Visible = true;
            AddItem_Top.Visible = true;
        }

        /// <summary>
        /// Module dropdown selected index Changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadView();
        }

        /// <summary>
        /// Show deleted Record Check box checked change event to Show/Hide Deleted Record.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            ReloadView();
        }

        /// <summary>
        /// HTML Row Prepared Event to Set Title column as link and Add Edit link on edit icon in every row.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void aspxtreelist_HtmlRowPrepared(object sender, TreeListHtmlRowEventArgs e)
        {
            if (UGITUtility.StringToInt(e.NodeKey) > 0)
            {
                string lsDataKeyValue = e.NodeKey;
                // Remove user-ids from multi-user fields    
                string requestType = string.Empty;
                if (e.GetValue(DatabaseObjects.Columns.TicketRequestType) != DBNull.Value)
                {
                    requestType = (string)(e.GetValue(DatabaseObjects.Columns.TicketRequestType));
                }
                string editItem;
                editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.EditParam, lsDataKeyValue, ddlModule.SelectedValue));
                HtmlAnchor anchorRequestType = (HtmlAnchor)aspxtreelist.FindDataCellTemplateControl(lsDataKeyValue, (TreeListDataColumn)aspxtreelist.Columns["Request Type"], "aRequestType1");
                string jsFunc = string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - Edit Item','653','900',0,'{1}','true')", editItem, Server.UrlEncode(Request.Url.AbsolutePath), this.FormTitle);
                anchorRequestType.Attributes.Add("href", jsFunc);
                 
                anchorRequestType.InnerText = requestType;

                if (chkShowDeleted.Checked)
                {
                    bool IsDeleted = UGITUtility.StringToBoolean(e.GetValue(DatabaseObjects.Columns.Deleted));
                    if (IsDeleted)
                    {
                        e.Row.BackColor = System.Drawing.Color.FromArgb(165, 52, 33);
                        foreach (TableCell item in e.Row.Cells)
                        {
                            item.Style.Add("color", "#FFF");
                        }
                        anchorRequestType.Style.Add("color", "#FFF");
                    }
                }

            }
            else if (e.Level == 1)
            {

                string category = string.Empty;
                if (e.GetValue(DatabaseObjects.Columns.Category) != DBNull.Value)
                    category = (string)e.GetValue(DatabaseObjects.Columns.Category);

                string requestType = string.Empty;
                if (e.GetValue(DatabaseObjects.Columns.TicketRequestType) != DBNull.Value)
                {
                    requestType = (string)e.GetValue(DatabaseObjects.Columns.TicketRequestType);

                }

                string editItem;
                string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&category={1}&module={2}";
                editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.EditParam, Uri.EscapeDataString(category), ddlModule.SelectedValue));
                HtmlAnchor anchorRequestType = (HtmlAnchor)aspxtreelist.FindDataCellTemplateControl(e.NodeKey, (TreeListDataColumn)aspxtreelist.Columns["Request Type"], "aRequestType1");
                string jsFunc = string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - Edit Item','653','900',0,'{1}','true')", editItem, Server.UrlEncode(Request.Url.AbsolutePath), this.FormTitle);
                anchorRequestType.Attributes.Add("href", "javascript:");
                anchorRequestType.Attributes.Add("onclick", jsFunc);
                anchorRequestType.InnerText = requestType;
                anchorRequestType.Attributes.Add("style", string.Format("font-weight:bold"));

                if (chkShowDeleted.Checked)
                {
                    bool IsDeleted = UGITUtility.StringToBoolean(e.GetValue(DatabaseObjects.Columns.Deleted));
                    if (IsDeleted)
                    {
                        e.Row.BackColor = System.Drawing.Color.FromArgb(165, 52, 33);
                        foreach (TableCell item in e.Row.Cells)
                        {
                            item.Style.Add("color", "#FFF");
                        }
                        anchorRequestType.Style.Add("color", "#FFF");
                    }
                }
            }
            else if (e.Level == 2)
            {
                aspxtreelist.FindNodeByKeyValue(e.NodeKey).AllowSelect = false;
                string category = string.Empty;
                string subcategory = string.Empty;
                string requestType = string.Empty;

                if (e.GetValue(DatabaseObjects.Columns.Category) != DBNull.Value)
                    category = (string)e.GetValue(DatabaseObjects.Columns.Category);

                if (e.GetValue(DatabaseObjects.Columns.SubCategory) != DBNull.Value)
                    subcategory = (string)e.GetValue(DatabaseObjects.Columns.SubCategory);
                if (e.GetValue(DatabaseObjects.Columns.TicketRequestType) != DBNull.Value)
                {
                    requestType = (string)e.GetValue(DatabaseObjects.Columns.TicketRequestType);

                }
                string editItem;
                string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&category={1}&module={2}&subcategory={3}";
                editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.EditParam, Uri.EscapeDataString(category), ddlModule.SelectedValue, Uri.EscapeDataString(subcategory)));
                HtmlAnchor anchorRequestType = (HtmlAnchor)aspxtreelist.FindDataCellTemplateControl(e.NodeKey, (TreeListDataColumn)aspxtreelist.Columns["Request Type"], "aRequestType1");
                string jsFunc = string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - Edit Item','653','900',0,'{1}','true')", editItem, Server.UrlEncode(Request.Url.AbsolutePath), this.FormTitle);
                anchorRequestType.Attributes.Add("href", "javascript:");
                anchorRequestType.Attributes.Add("onclick", jsFunc);
                anchorRequestType.InnerText = requestType;
                anchorRequestType.Attributes.Add("style", string.Format("font-weight:bold"));

            }
        }

        /// <summary>
        /// Multiple Edit button Event for Batch Editing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void aEditItem_Top_Click(object sender, EventArgs e)
        {
            List<string> ids = new List<string>();
            foreach (TreeListNode node in aspxtreelist.GetSelectedNodes())
            {
                if (Convert.ToInt32(node.Key) > 0)
                {
                    ids.Add(node.Key);
                }
            }

            if (ids.Count > 0)
            {
                selectedIDs = string.Join(",", ids.ToArray());
            }
            else { ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowMessage();", true); }
        }

        /// <summary>
        /// Multiple Edit button Event for Batch Editing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btDuplicateItem_Click(object sender, EventArgs e)
        {
            string moduleName = ddlModule.SelectedValue;
            List<string> selectedRequestTypes = aspxtreelist.GetSelectedNodes().Select(x => x.Key).ToList();
            int CopyCount = 0;
            if (selectedRequestTypes.Count > 0)
            {
                foreach (var item in selectedRequestTypes)
                {
                    requestType = null;
                    requestType = RequestTypeManager.LoadByID(Convert.ToInt64(item));
                    requestType.ID = 0;
                    requestType.RequestType = $"{requestType.RequestType}-Copy";
                    requestType.Title = $"{requestType.Title}-Copy";
                    CopyCount = RequestTypeManager.Load().Where(x => x.RequestType != null && x.RequestType.StartsWith(requestType.RequestType) && requestType.ModuleNameLookup == moduleName).Count();
                    if (CopyCount > 0)
                    {
                        requestType.RequestType = requestType.RequestType + (CopyCount);
                        requestType.Title = requestType.Title + (CopyCount);
                    }
                    RequestTypeManager.Insert(requestType);

                    CopyLocation(Convert.ToInt64(item), requestType.ID);
                }
            }
            else
            {
                #region[Copy Category And SubCategory]
                category = aspxtreelist.FocusedNode.Key;

                if (category.Contains('_'))
                {
                    string[] subCatArr = category.Split('_');
                    subCategory = subCatArr[subCatArr.Length - 1];
                    CopyRequestTypeInSubCatg(subCatArr[subCatArr.Length - 2], subCatArr[subCatArr.Length - 1], moduleName);
                }
                else
                {
                    CopyAllRequestTypeInCategory(category, moduleName);
                }
                #endregion
            }

            ReloadView();
        }

        private void CopyRequestTypeInSubCatg(string category, string subCategory, string moduleName)
        {
            int CopyCount = 0;
            long OldId = 0;
            requestTypeList = RequestTypeManager.Load().Where(x => x.Category != null && x.SubCategory != null && x.Category == category && x.SubCategory == subCategory && x.ModuleNameLookup == moduleName).ToList();
            CopyCount = RequestTypeManager.Load().Where(x => x.Category != null && x.SubCategory != null && x.Category == category && x.SubCategory.StartsWith(subCategory + "-Copy") && x.ModuleNameLookup == moduleName).Select(x => x.SubCategory).Distinct().Count();
            if (requestTypeList != null && requestTypeList.Count > 0)
            {
                foreach (var item in requestTypeList)
                {
                    requestType = requestTypeList.Where(x => x.ID == item.ID).FirstOrDefault();
                    OldId = requestType.ID;
                    requestType.ID = 0;
                    requestType.SubCategory = $"{requestType.SubCategory}-Copy";
                    requestType.Title = $"{requestType.Category} > {requestType.SubCategory} {(String.IsNullOrEmpty(requestType.SubCategory) ? "" : ">")} {requestType.RequestType}";
                    if (CopyCount > 0)
                    {
                        requestType.SubCategory = requestType.SubCategory + (CopyCount);
                        requestType.Title = $"{requestType.Category} > {requestType.SubCategory} {(String.IsNullOrEmpty(requestType.SubCategory) ? "" : ">")} {requestType.RequestType}";
                    }
                    RequestTypeManager.Insert(requestType);
                    CopyLocation(OldId, requestType.ID);
                }
            }
        }

        private void CopyAllRequestTypeInCategory(string category, string moduleName)
        {
            int CopyCount = 0;
            long OldId = 0;
            requestTypeList = RequestTypeManager.Load().Where(x => x.Category != null && x.Category == category && x.ModuleNameLookup == moduleName).ToList();
            CopyCount = RequestTypeManager.Load().Where(x => x.Category != null && x.Category.StartsWith(category + "-Copy") && x.ModuleNameLookup == moduleName).Select(x => x.Category).Distinct().Count();
            if (requestTypeList != null && requestTypeList.Count > 0)
            {
                foreach (var item in requestTypeList)
                {
                    requestType = requestTypeList.Where(x => x.ID == item.ID).FirstOrDefault();
                    OldId = requestType.ID;
                    requestType.ID = 0;
                    requestType.Category = $"{requestType.Category}-Copy";
                    requestType.Title = $"{requestType.Category} {(String.IsNullOrEmpty(requestType.Category) ? "" : ">")} {requestType.SubCategory} {(String.IsNullOrEmpty(requestType.SubCategory) ? "" : ">")} {requestType.RequestType}";
                    if (CopyCount > 0)
                    {
                        requestType.Category = requestType.Category + (CopyCount);
                        requestType.Title = $"{requestType.Category} {(String.IsNullOrEmpty(requestType.Category) ? "" : ">")} {requestType.SubCategory} {(String.IsNullOrEmpty(requestType.SubCategory) ? "" : ">")} {requestType.RequestType}";
                    }
                    RequestTypeManager.Insert(requestType);
                    CopyLocation(OldId, requestType.ID);
                }
            }
        }

        private void CopyLocation(long oldId, long newId)
        {
            requestTypeLocation = RequestTypeByLocationManager.Load().Where(x => x.RequestTypeLookup == oldId).ToList();

            if (requestTypeLocation != null && requestTypeLocation.Count > 0)
            {
                requestTypeLocation.ForEach(x =>
                {
                    x.ID = 0;
                    x.RequestTypeLookup = newId;
                });
                RequestTypeByLocationManager.InsertItems(requestTypeLocation);
            }
        }

        /// <summary>
        /// HTML Data Cell Prepared to set Owner and RequestType Escalation manager in 
        /// specific format(Remove IDs from multi Lookup User field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void aspxtreelist_HtmlDataCellPrepared(object sender, TreeListHtmlDataCellEventArgs e)
        {
            if (UGITUtility.StringToInt(e.NodeKey) > 0)
            {
                string values = Convert.ToString(e.GetValue(e.Column.FieldName));

                if (string.IsNullOrEmpty(values))
                    return;
                string value = FieldConfigurationManager.GetFieldConfigurationData(e.Column.FieldName, values);
                if (!string.IsNullOrEmpty(value))
                {
                    e.Cell.Text = value;
                }
                else if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(values))
                {
                    e.Cell.Text = "";
                }
            }
        }

        /// <summary>
        /// Data Bound to set Node Selection Settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void aspxtreelist_DataBound(object sender, EventArgs e)
        {
            SetNodeSelectionSettings();
        }

        /// <summary>
        /// To set Node Selection Settings.
        /// </summary>
        private void SetNodeSelectionSettings()
        {
            TreeListNodeIterator iterator = aspxtreelist.CreateNodeIterator();
            TreeListNode node;
            while (true)
            {
                node = iterator.GetNext();
                if (node == null) break;
                node.AllowSelect = !node.HasChildren;
            }
        }

        protected void btDeleteItem_Top_Click(object sender, EventArgs e)
        {
            List<string> selectedRequestTypes = aspxtreelist.GetSelectedNodes().Select(x => x.Key).ToList();
            if (selectedRequestTypes.Count > 0)
            {
                List<ModuleRequestType> requestTypeList = RequestTypeManager.Load();
                if (requestTypeData != null && requestTypeData.Count > 0)
                {

                    string selectRequestType = string.Join(",", selectedRequestTypes.Select(y => "'" + y + "'"));
                    var requestTypes = requestTypeList.Where(x => selectRequestType.Contains(x.ID.ToString())).ToList();
                    if (requestTypes != null && requestTypes.Count > 0)
                        requestTypeList = requestTypes;
                    var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(requestTypeList);
                    var itemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModuleRequestType>>(jsonData);
                    foreach (var item in itemList)
                    {

                        item.Deleted = true;
                        RequestTypeManager.Update(item);
                    }
                }
                ReloadView();
            }
        }

        protected void btUnDeleteItem_Top_Click(object sender, EventArgs e)
        {
            List<string> selectedRequestTypes = aspxtreelist.GetSelectedNodes().Select(x => x.Key).ToList();
            if (selectedRequestTypes.Count > 0)
            {

                List<ModuleRequestType> requestTypeList = RequestTypeManager.Load();
                if (requestTypeData != null && requestTypeData.Count > 0)
                {
                    string selectRequestType = string.Join(",", selectedRequestTypes.Select(y => "'" + y + "'"));
                    var requestTypes = requestTypeList.Where(x => selectRequestType.Contains(x.ID.ToString())).ToList();
                    if (requestTypes != null && requestTypes.Count > 0)
                        requestTypeList = requestTypes;
                    foreach (var item in requestTypeList)
                    {
                        item.Deleted = false;
                        RequestTypeManager.Update(item);

                    }
                }
                ReloadView();
            }
        }

        private void BindData()
        {
            GenerateData(ddlModule.SelectedValue);
            aspxtreelist.DataSource = UGITUtility.ToDataTable(requestTypeData);
            aspxtreelist.DataBind();
            if (aspxtreelist.TotalNodeCount > 30)
                aspxtreelist.CollapseAll();
            else
                aspxtreelist.ExpandAll();
        }

        protected void aspxtreelist_CustomColumnDisplayText(object sender, TreeListColumnDisplayTextEventArgs e)
        {

        }

        protected void btnApplyChanges_Click(object sender, EventArgs e)
        {
            string moduleName = ddlModule.SelectedValue;
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            UGITModule module = moduleManager.LoadByName(ddlModule.SelectedValue, false);
            if (module != null)
            {
                string cacheName = "Lookup_" + DatabaseObjects.Tables.RequestType + "_" + context.TenantID;
                Util.Cache.CacheHelper<UGITModule>.AddOrUpdate(module.ModuleName, context.TenantID, module);
                DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);
            }
        }

        private void ReloadView()
        {
            // Re-use existing parameters to ensure current view is maintained
            string moduleName = ddlModule.SelectedValue;
            string showdelete = chkShowDeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, //"_layouts/15/ugovernit/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&module={2}&showdelete={3}",
                                                      this.ViewParam, this.FormTitle, moduleName, showdelete));
            Response.Redirect(url);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            List<int> selectedRequestTypes = aspxtreelist.GetSelectedNodes().Select(x => Convert.ToInt32(x.Key)).ToList();
            DataTable list = RequestTypeManager.GetDataTable();
            DataTable dt = new DataTable();
            DataTable table = new DataTable();
            if (selectedRequestTypes.Count > 0)
            {
                DataRow[] coll = null;
                coll = list.Select(string.Format("{0} in ({1})", DatabaseObjects.Columns.ID, string.Join(",", selectedRequestTypes.Select(x => "'" + x + "'"))));

                dt = coll.CopyToDataTable();
                table = ExportListToExcel.GetDataTableFromList(dt, ApplicationContext);
            }
            else
            {
                table = ExportListToExcel.GetDataTableFromList(list, ApplicationContext);
            }
            try
            {
                var worksheet = ASPxSpreadsheet1.Document.Worksheets.ActiveWorksheet;
                worksheet.Import(table, true, 0, 0);
                MemoryStream st = new MemoryStream();
                ASPxSpreadsheet1.Document.SaveDocument(st, DocumentFormat.OpenXml);
                Response.Clear();
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment; filename=RequestType.xlsx");
                Response.BinaryWrite(st.ToArray());
                Response.End();
            }catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
        }
        protected void btnExportXML_Click(object sender, EventArgs e)
        {
            List<int> selectedRequestTypes = aspxtreelist.GetSelectedNodes().Select(x => Convert.ToInt32(x.Key)).ToList();
            DataTable list = RequestTypeManager.GetDataTable();
            DataTable dt = new DataTable();
            DataTable table = new DataTable();
            if (selectedRequestTypes.Count > 0)
            {
                DataRow[] coll = null;
                coll = list.Select(string.Format("{0} in ({1})", DatabaseObjects.Columns.ID, string.Join(",", selectedRequestTypes.Select(x => "'" + x + "'"))));

                dt = coll.CopyToDataTable();
                table = ExportListToExcel.GetDataTableFromList(dt, ApplicationContext);
            }
            else
            {
                table = ExportListToExcel.GetDataTableFromList(list, ApplicationContext);
            }
            DataSet dS = new DataSet();
            dS.DataSetName = "RecordSet";
            dS.Tables.Add(table);
            StringWriter sw = new StringWriter();
            dS.WriteXml(sw, XmlWriteMode.IgnoreSchema);
            string s = sw.ToString();
            string attachment = "attachment; filename=RequestType.xml";
            Response.ClearContent();
            Response.ContentType = "application/xml";
            Response.AddHeader("content-disposition", attachment);
            Response.Write(s);
            Response.End();
            var worksheet = ASPxSpreadsheet1.Document.Worksheets.ActiveWorksheet;
            worksheet.Import(table, true, 0, 0);
            MemoryStream st = new MemoryStream();
            ASPxSpreadsheet1.Document.SaveDocument(st, DocumentFormat.OpenXml);
            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment; filename=RequestType.xlsx");
            Response.BinaryWrite(st.ToArray());
            Response.End();
        }
    }
}