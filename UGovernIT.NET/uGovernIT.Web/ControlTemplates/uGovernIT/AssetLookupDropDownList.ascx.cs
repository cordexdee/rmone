using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Utils;
using DevExpress.Web;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Manager;
using System.Data;
using System.Web;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager.Managers;
namespace uGovernIT.Web
{
    public partial class AssetLookupDropDownList : UserControl
    {
        public string currentModuleName { get; set; }
        public ASPxDropDownEdit DropDown { get; set; }
        public bool DisableCustomFilter { get; set; }
        public string SetValueCheck { get; set; }
        public AssetTypeCustomProperties CustomProperties { get; set; }
        public ASPxGridView gridView;
        public bool IsMulti { get; set; }
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        //ASPxHiddenField hiddenField;

        public AssetLookupDropDownList()
        {
            CustomProperties = new AssetTypeCustomProperties();
            DropDown = new ASPxDropDownEdit();
            DropDown.ID = this.ID + "_AssetDropDown";
            DropDown.CssClass = "RequestorUser";

        }
        public class AssetLookupEditTemplate : ITemplate
        {
            public ASPxDropDownEdit dropDown;
            DataTable result = new DataTable();
            public AssetLookupEditTemplate(ASPxDropDownEdit DropDown)
            {
                dropDown = DropDown;
            }

            public void InstantiateIn(System.Web.UI.Control container)
            {
                ASPxRadioButtonList rdbAssetsFilter = CreateAssetFilterControl();
                ASPxGridView assetList = CreateAssetGridView();
                assetList.ID = "assetList";
                rdbAssetsFilter.CssClass = "assetList-radioBtn";
                container.Controls.Add(rdbAssetsFilter);
                container.Controls.Add(assetList);
            }

            private ASPxGridView CreateAssetGridView()
            {

                ASPxGridView assetList = new ASPxGridView();
                assetList.Settings.ShowFilterRow = true;
                assetList.Settings.ShowFilterRowMenu = true;
                assetList.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                assetList.SettingsBehavior.FilterRowMode = GridViewFilterRowMode.OnClick;
                assetList.EnableCallBacks = true;
                assetList.CustomCallback += GridAssets_CustomCallback;
                assetList.Init += AssetList_Init;
                assetList.ClientSideEvents.EndCallback = "function(s, e) { try{ onEndCallBack(s,e);}catch(ex){}}";
                assetList.ClientSideEvents.SelectionChanged = "function(s,e){onAssetSelectionChanged(s,e)}";
                assetList.Attributes.Add("class", DatabaseObjects.Columns.AssetLookup);
                assetList.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                List<ModuleColumn> lstModuleColumns;
                ModuleColumnManager objmodulecolumnmanager = new ModuleColumnManager(HttpContext.Current.GetManagerContext());
                lstModuleColumns = objmodulecolumnmanager.Load($"{DatabaseObjects.Columns.CategoryName}='" + Constants.MyAssets + "'").OrderBy(x => x.FieldSequence).ToList();
                if (lstModuleColumns != null && lstModuleColumns.Count > 0)
                    lstModuleColumns = lstModuleColumns.Where(x => x.IsDisplay == true).OrderBy(x => x.FieldSequence).ToList();
                else
                {
                    // Not configured, so add default columns
                    int sequence = 0;

                    ModuleColumn column = new ModuleColumn();
                    column.FieldName = DatabaseObjects.Columns.TicketId;
                    column.FieldDisplayName = "Asset ID";
                    column.FieldSequence = ++sequence;
                    lstModuleColumns.Add(column);

                    column = new ModuleColumn();
                    column.FieldName = DatabaseObjects.Columns.AssetTagNum;
                    column.FieldDisplayName = "Asset Tag";
                    column.FieldSequence = ++sequence;
                    lstModuleColumns.Add(column);

                    column = new ModuleColumn();
                    column.FieldName = DatabaseObjects.Columns.TicketRequestTypeLookup;
                    column.FieldDisplayName = "Asset Type";
                    column.ColumnType = "Lookup";
                    column.FieldSequence = ++sequence;
                    lstModuleColumns.Add(column);

                    column = new ModuleColumn();
                    column.FieldName = DatabaseObjects.Columns.AssetModelLookup;
                    column.FieldDisplayName = "Asset Model";
                    column.ColumnType = "Lookup";
                    column.FieldSequence = ++sequence;
                    lstModuleColumns.Add(column);

                    column = new ModuleColumn();
                    column.FieldName = DatabaseObjects.Columns.AssetOwner;
                    column.FieldDisplayName = "Owner";
                    column.ColumnType = "User";
                    column.FieldSequence = ++sequence;
                    lstModuleColumns.Add(column);

                    column = new ModuleColumn();
                    column.FieldName = DatabaseObjects.Columns.TicketStatus;
                    column.FieldDisplayName = "Status";
                    column.FieldSequence = ++sequence;
                    lstModuleColumns.Add(column);
                }

                GridViewCommandColumn select = new GridViewCommandColumn();
                select.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;
                select.ShowSelectCheckbox = true;
                select.Width = 30;

                assetList.Columns.Add(select);

                foreach (ModuleColumn moduleColumn in lstModuleColumns)
                {
                    string fieldColumn = Convert.ToString(moduleColumn.FieldName);
                    if (fieldColumn.EndsWith("lookup", StringComparison.OrdinalIgnoreCase))
                        fieldColumn = string.Format("{0}$", fieldColumn);
                    if(fieldColumn.EndsWith("user", StringComparison.OrdinalIgnoreCase))
                        fieldColumn = string.Format("{0}$", fieldColumn);

                    string fieldDisplayName = Convert.ToString(moduleColumn.FieldDisplayName);
                    if (fieldColumn != DatabaseObjects.Columns.Attachments)
                    {
                        GridViewDataTextColumn column = new GridViewDataTextColumn();
                        column.FieldName = fieldColumn;
                        column.Caption = fieldDisplayName;
                        column.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                        column.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                        column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                        if (fieldColumn == DatabaseObjects.Columns.TicketId || fieldColumn == DatabaseObjects.Columns.AssetTagNum)
                            column.Width = new Unit(120, UnitType.Pixel);
                        if (fieldColumn == DatabaseObjects.Columns.AssetModelLookup || fieldColumn == DatabaseObjects.Columns.DepartmentLookup)
                            column.Width = new Unit(200, UnitType.Pixel);
                        assetList.Columns.Add(column);
                    }
                }

                assetList.SettingsBehavior.EnableRowHotTrack = true;
                assetList.SettingsResizing.ColumnResizeMode = ColumnResizeMode.NextColumn;
                assetList.Settings.ShowHeaderFilterButton = true;
                assetList.ClientSideEvents.EndCallback = "function(s, e) { try{ onEndCallBack(s,e);}catch(ex){}}";
                assetList.EnableViewState = false;
                assetList.KeyFieldName = DatabaseObjects.Columns.ID;
                assetList.ClientInstanceName = "cbAssetList";
                assetList.Settings.VerticalScrollableHeight = 200;
                assetList.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                assetList.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                assetList.Width = Unit.Pixel(700);
                return assetList;
            }

            private ASPxRadioButtonList CreateAssetFilterControl()
            {
                ASPxRadioButtonList rdbAssetsFilter = new ASPxRadioButtonList();
                rdbAssetsFilter.RepeatDirection = RepeatDirection.Horizontal;
                rdbAssetsFilter.Border.BorderStyle = BorderStyle.None;
                ListEditItem myAssetItem = new ListEditItem("Requestor's Assets", 0);
                myAssetItem.Selected = true;
                rdbAssetsFilter.Items.Add(myAssetItem);
                rdbAssetsFilter.Items.Add("All Assets", 1);

                rdbAssetsFilter.ID = "rdbAssetsFilter";
                rdbAssetsFilter.EnableClientSideAPI = true;
                rdbAssetsFilter.ClientInstanceName = "rdbAssetsFilterCIN";
                rdbAssetsFilter.ClientSideEvents.SelectedIndexChanged = "function(s, e) { try{ showAllAssets(s,e);}catch(ex){}}";
                return rdbAssetsFilter;
            }

            private void AssetList_Init(object sender, EventArgs e)
            {
                ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                UserProfileManager userProfileManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                ASPxGridView cbx = sender as ASPxGridView;


                string userName = context.CurrentUser.Name;
                if (cbx.JSProperties.ContainsKey("cpDependentFieldValue"))
                    userName = Convert.ToString(cbx.JSProperties["cpDependentFieldValue"]);
                UserProfile user = userProfileManager.GetUserByUserName(userName);
                UGITModule cmdbModule = moduleViewManager.LoadByName("CMDB");//uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, "CMDB");
                TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
                result = ticketManager.GetOpenTickets(cmdbModule); //uGITCache.ModuleDataCache.GetOpenTickets(cmdbModule.ID);
                string strUserName = UGITUtility.GetCookieValue(HttpContext.Current.Request, "DependentRequestor");
                List<HistoryEntry> datalist;
                foreach (DataRow dr in result.Rows)
                {
                    if (result != null && result.Rows.Count > 0)
                    {

                        foreach (DataColumn col in result.Columns)
                        {
                            if (dr.Table.Columns.Contains(col.ColumnName))
                            {
                                if (col.ColumnName == DatabaseObjects.Columns.Comment)
                                {
                                    datalist = uHelper.GetHistorywithusername(dr, DatabaseObjects.Columns.TicketComment, true);
                                    dr[col.ColumnName] = uHelper.GetComments(datalist);
                                }
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(strUserName))
                {
                    if (strUserName != "all")
                    {
                        cbx.Attributes.Add("userName", strUserName);
                        if (result != null && result.Rows.Count > 0 && result.Columns.Contains(DatabaseObjects.Columns.Owner))
                        {
                            //Filter all the assets where current user is assetower or current user
                            var xbase = result.Select(string.Format("({0} in ('{1}')) or ({2} in ('{1}'))", DatabaseObjects.Columns.Owner + "$Id", strUserName.Replace("'", "''"), DatabaseObjects.Columns.CurrentUser));
                            if (xbase.Count() > 0)
                                result = xbase.CopyToDataTable();
                            else
                                result = null;
                        }

                        if (result == null && cbx.Selection.Count > 0)
                            cbx.Selection.UnselectAll();
                    }
                    cbx.DataSource = result;
                    cbx.DataBind();
                }

                if (!string.IsNullOrEmpty(strUserName))
                {
                    List<object> seledtedIds = cbx.GetSelectedFieldValues(DatabaseObjects.Columns.ID);
                    DataTable dt = cbx.DataSource as DataTable;
                    if (dt != null)
                    {
                        if (!dt.Columns.Contains("Selected"))
                        {
                            DataColumn selectedColmn = new DataColumn("Selected", typeof(Int16));
                            selectedColmn.AllowDBNull = true;
                            selectedColmn.DefaultValue = 0;
                            dt.Columns.Add(selectedColmn);
                        }
                        foreach (object item in seledtedIds)
                        {
                            DataRow dr = dt.AsEnumerable().FirstOrDefault(x => x.Field<Int64>(DatabaseObjects.Columns.ID) == Convert.ToInt64(item));
                            if (dr != null)
                            {
                                dr["Selected"] = 1;
                            }
                        }
                        dt = dt.AsEnumerable().OrderByDescending(x => x.Field<Int16>("Selected")).CopyToDataTable();
                    }
                    cbx.DataSource = dt;
                    cbx.DataBind();
                }
            }

            private void GridAssets_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
            {
                ASPxGridView cbx = (ASPxGridView)sender;
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                UserProfileManager userProfileManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
                string parameters = Uri.UnescapeDataString(e.Parameters);
                string userName = string.Empty;
                string strSelectAll = string.Empty;
                string strSelectedValue = string.Empty;
                if (parameters.Contains(Constants.Separator2))
                {
                    userName = parameters.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[0];
                    strSelectedValue = parameters.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[1];
                }
                else
                    userName = parameters;
                if (userName.Contains(Constants.Separator))
                {
                    strSelectAll = userName.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries)[1];
                    userName = userName.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries)[0];

                }

                List<string> lstUserNames = new List<string>();
                string[] userNames = userName.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in userNames)
                {
                    //if (item != "all")
                    //{
                    UserProfile user = userProfileManager.GetUserById(item);
                    if (user != null)
                        lstUserNames.Add(user.Id);
                    //}
                }

                string strUserName = string.Join("','", lstUserNames);
                if (string.IsNullOrEmpty(strUserName) && userName == "all")
                    strUserName = userName;
                UGITUtility.CreateCookie(HttpContext.Current.Response, "DependentRequestor", strUserName);
                UGITUtility.CreateCookie(HttpContext.Current.Response, "View", "allassets");
                UGITModule cmdbModule = moduleViewManager.LoadByName("CMDB");
                TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
                DataTable result = ticketManager.GetOpenTickets(cmdbModule); 
                if (result == null)
                    return;

                if (!result.Columns.Contains("Selected"))
                {
                    DataColumn selectedColmn = new DataColumn("Selected", typeof(Int16));
                    selectedColmn.AllowDBNull = true;
                    selectedColmn.DefaultValue = 0;
                    result.Columns.Add(selectedColmn);
                }

                DataTable filteredTable = result.Copy();

                if (!string.IsNullOrEmpty(strUserName) && strUserName != "all")
                {
                    cbx.Attributes.Add("userName", strUserName);
                    if (result != null && result.Rows.Count > 0 && result.Columns.Contains(DatabaseObjects.Columns.Owner))
                    {

                        //Filter all the assets where current user is assetower or current user
                        var xbase = result.Select(string.Format("({0} in ('{1}')) or ({2} in ('{1}'))", DatabaseObjects.Columns.Owner+"$Id", strUserName.Replace("'", "''"), DatabaseObjects.Columns.CurrentUser));
                        if (xbase.Count() > 0)
                            filteredTable = xbase.CopyToDataTable();
                        else
                            filteredTable = null;
                    }
                    if (filteredTable == null && cbx.Selection.Count > 0)
                        cbx.Selection.UnselectAll();
                }
                else if (string.IsNullOrEmpty(strUserName))
                {
                    filteredTable = null;
                }

                UGITUtility.DeleteCookie(HttpContext.Current.Request, HttpContext.Current.Response, "View");
                if (!string.IsNullOrEmpty(strUserName) && strUserName.Equals("all", StringComparison.OrdinalIgnoreCase))
                    UGITUtility.CreateCookie(HttpContext.Current.Response, "View", "allassets");

                List<string> selectedAssets = null;

                if (!string.IsNullOrEmpty(strSelectedValue) && strSelectedValue != "undefined")
                {
                    selectedAssets = UGITUtility.SplitString(strSelectedValue, Constants.Separator6).ToList(); //new List<string>(strSelectedValue);
                    if (selectedAssets != null)
                    {
                        bool showAll = false;
                        foreach (string item in selectedAssets)
                        {
                            if (filteredTable != null && !filteredTable.AsEnumerable().Any(x => x.Field<Int64>(DatabaseObjects.Columns.ID) == Convert.ToInt64(item)))
                            {
                                showAll = true;
                                break;
                            }

                        }
                        if (showAll)
                        {
                            UGITUtility.CreateCookie(HttpContext.Current.Response, "View", "allassets");
                            UGITUtility.CreateCookie(HttpContext.Current.Response, "DependentRequestor", "all");
                            string selectedAsset = string.Join(Constants.Separator6, selectedAssets);
                            if (!string.IsNullOrEmpty(selectedAsset))
                            {
                                filteredTable = result.Copy();
                                ASPxDropDownEdit drop = cbx.NamingContainer.NamingContainer.NamingContainer as ASPxDropDownEdit;
                                if (drop != null)
                                    //if (!cbx.JSProperties.ContainsKey("cpSelectedValues"))
                                    //    cbx.JSProperties.Add("cpSelectedValues", string.Join(";", selectedAssets.Select(x => x.LookupValue)));

                                    if (!cbx.JSProperties.ContainsKey("cpSelectedValues"))
                                    {
                                        cbx.JSProperties.Add("cpSelectedValues", "");
                                        cbx.JSProperties["cpSelectedValue"] = selectedAsset;
                                    }
                            }
                        }

                    }
                }
                cbx.DataSource = filteredTable;
                cbx.DataBind();
                if (selectedAssets != null)
                {
                    foreach (string item in selectedAssets)
                    {
                        cbx.Selection.SetSelectionByKey(item, true);

                    }
                }
                if (!string.IsNullOrEmpty(strUserName))
                {
                    List<object> seledtedIds = cbx.GetSelectedFieldValues(DatabaseObjects.Columns.ID);
                    DataTable dt = cbx.DataSource as DataTable;
                    if (dt != null)
                    {
                        if (!dt.Columns.Contains("Selected"))
                        {
                            DataColumn selectedColmn = new DataColumn("Selected", typeof(Int16));
                            selectedColmn.AllowDBNull = true;
                            selectedColmn.DefaultValue = 0;
                            dt.Columns.Add(selectedColmn);
                        }
                        foreach (object item in seledtedIds)
                        {
                            DataRow dr = dt.AsEnumerable().FirstOrDefault(x => x.Field<Int64>(DatabaseObjects.Columns.ID) == Convert.ToInt64(item));
                            if (dr != null)
                            {
                                dr["Selected"] = 1;
                            }
                        }
                        dt = dt.AsEnumerable().OrderByDescending(x => x.Field<Int16>("Selected")).CopyToDataTable();
                    }
                    cbx.DataSource = dt;
                    cbx.DataBind();
                }


                GridViewCommandColumn cbSelectAllCol = cbx.Columns[0] as GridViewCommandColumn;
                ASPxCheckBox cbSelectAll = cbx.FindHeaderTemplateControl(cbSelectAllCol, "cbSelectAll") as ASPxCheckBox;

                if (cbSelectAll != null)
                {
                    if (strSelectAll.ToLower() == "selectall")
                    {
                        cbSelectAll.Checked = true;
                        cbx.Selection.SelectAll();
                    }
                    else if (strSelectAll.ToLower() == "unselectall")
                    {
                        cbSelectAll.Checked = false;
                        cbx.Selection.UnselectAll();
                    }
                }
            }

        }
        protected override void OnInit(EventArgs e)
        {
            DropDown.ClientInstanceName = "relatedAssetDropDown";
            DropDown.DropDownWindowStyle.CssClass = "relatedAssetDropDown";
            DropDown.DropDownWindowWidth = new Unit(100, UnitType.Percentage);
            DropDown.DropDownWindowTemplate = new AssetLookupEditTemplate(DropDown);

            ASPxRadioButtonList rdnlist = (ASPxRadioButtonList)DropDown.FindControl("rdbAssetsFilter");
            if (rdnlist != null && DisableCustomFilter)
            {
                rdnlist.SelectedIndex = 1;
                rdnlist.ClientVisible = false;
            }

            if (!DropDown.JSProperties.ContainsKey("cpCurrentModuleName"))
                DropDown.JSProperties.Add("cpCurrentModuleName", "");
            DropDown.JSProperties["cpCurrentModuleName"] = "TSR";// currentModuleName;
            ASPxGridView grid = DropDown.FindControl("assetList") as ASPxGridView;

            if (grid != null)
            {
                if (!grid.JSProperties.ContainsKey("cpDependentFieldValue"))
                    grid.JSProperties.Add("cpDependentFieldValue", "");
                if (DropDown.JSProperties.ContainsKey("cpDependentFieldValue"))
                    grid.JSProperties["cpDependentFieldValue"] = DropDown.JSProperties["cpDependentFieldValue"];
            }
            DropDown.ClientSideEvents.GotFocus = "function(s,e){BindRelatedAssetsOnLoad(s, e);}";
            this.Controls.Add(this.DropDown);
            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                UGITUtility.DeleteCookie(HttpContext.Current.Request, HttpContext.Current.Response, "DependentRequestor");
                UGITUtility.DeleteCookie(HttpContext.Current.Request, HttpContext.Current.Response, "View");

            }

        }
        protected override void OnPreRender(EventArgs e)
        {
            SetValues(SetValueCheck);
            base.OnPreRender(e);
        }
        public void SetValues(string values)
        {
            if (string.IsNullOrWhiteSpace(values))
                return;
            // SPFieldLookupValueCollection assets = new SPFieldLookupValueCollection(values);
            DropDown.KeyValue = values;

            // DropDown.KeyValue = values;
            List<string> list = new List<string>();
            List<string> listMultiValue = UGITUtility.SplitString(values, Constants.Separator6).ToList();
            if (listMultiValue.Count > 0)
            {
                listMultiValue.ForEach(x =>
                {
                    DataRow dataRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Assets, "ID =" + x).Select().First();
                    if (dataRow != null)
                    {
                        if (UGITUtility.IsSPItemExist(dataRow, DatabaseObjects.Columns.AssetTagNum))
                        {
                            list.Add(Convert.ToString(dataRow[DatabaseObjects.Columns.AssetTagNum]));
                            DropDown.Text = string.Join(Constants.Separator5, list);
                        }
                        else
                        {
                            list.Add(Convert.ToString(dataRow[DatabaseObjects.Columns.TicketId]));
                            DropDown.Text = string.Join(Constants.Separator5, list);

                        }
                    }

                });
            }
            // DropDown.Text = string.Join(";", assets.Select(x => x.LookupValue));
            if (!DropDown.JSProperties.ContainsKey("cpSelectedValue"))
            {
                DropDown.JSProperties.Add("cpSelectedValue", "");
                DropDown.JSProperties["cpSelectedValue"] = values;
            }


        }
        public string GetValues()
        {
            string value = "";
            AssetLookupEditTemplate template = this.DropDown.DropDownWindowTemplate as AssetLookupEditTemplate;
            if (template != null && this.DropDown.Value != null)
            {
                value = UGITUtility.ObjectToString(this.DropDown.KeyValue);
            }
            return value;
        }
        public void SetDependentFieldValue(string value)
        {
            DropDown.Attributes.Add("DependentField", DatabaseObjects.Columns.TicketRequestor);
            if (!DropDown.JSProperties.ContainsKey("cpDependentFieldValue"))
                DropDown.JSProperties.Add("cpDependentFieldValue", "");
            DropDown.JSProperties["cpDependentFieldValue"] = value;

        }
    }
}
