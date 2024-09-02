
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using DevExpress.Web;
using System.IO;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using System.Linq;
using DevExpress.Spreadsheet;
using uGovernIT.Web.Helpers;
using System.Collections.Specialized;

namespace uGovernIT.Web
{
    public partial class LocationView : UserControl
    {
        private List<Location> _SPList;
        private string addNewItem = string.Empty;
        private string absoluteUrlEdit = "_layouts/15/ugovernit/DelegateControl.aspx?control={0}";
        private string editParam = "locationedit";
        List<Location> dataTable = null;
        string selectedRegion = string.Empty;
        string selectedCountry = string.Empty;
        string selectedState = string.Empty;

        protected string importUrl;
        //private string absoluteUrlImport = "_layouts/15/ugovernit/DelegateControl.aspx?control={0}&listName={1}";
        private string absoluteUrlImport = "/layouts/ugovernit/DelegateControl.aspx?control={0}&listName={1}";
        protected string moveToProductionUrl = "";
        ConfigurationVariableManager ObjConfigurationVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        LocationManager ObjLocationManager = new LocationManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            if (ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableAdminImport))
                btnImport.Visible = true;
            importUrl = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlImport, "importexcelfile", "Location"));
            List<Location> locations = GetLocationData();
            LoadRegions(locations);
            LoadCountry(locations);
            LoadState(locations);
            EnableMigrate();
            base.OnInit(e);
        }
        private void EnableMigrate()
        {
            if (ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableMigrate))
            {
                btnMigrateLocation.Visible = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Migrate Url
            moveToProductionUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/uGovernITConfiguration.aspx?control=movestagetoproduction"));
            // condition for download excel...
            if (Request["exportType"] == "excel")
            {
                //DataTable table = UGITUtility.ObjectToData(list); // original code

                var list = ObjLocationManager.Load().Select(x => new { x.Title, x.LocationDescription, x.Deleted, UGITState = x.State, UGITCountry = x.Country, UGITRegion = x.Region, x.Attachments }).OrderBy(x => x.Title).ToList();

                DataTable table = UGITUtility.ToDataTable(list);
                var worksheet = SpreadSheetDev.Document.Worksheets.Add();
                worksheet.Import(table, true, 0, 0);
                MemoryStream st = new MemoryStream();
                SpreadSheetDev.Document.SaveDocument(st, DocumentFormat.OpenXml);
                Response.Clear();
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment; filename=Location.xlsx");
                Response.BinaryWrite(st.ToArray());
                Response.End();
            }
            else
            {
                if (!IsPostBack)
                {
                    chkShowDeleted.Checked = UGITUtility.StringToBoolean(Request["showarchive"]);
                }

                dataTable = GetLocationData();
                selectedRegion = Convert.ToString(gvRegion.GetRowValues(gvRegion.FocusedRowIndex, DatabaseObjects.Columns.UGITRegion));
                gvRegion.DataSource = GetRegions();
                gvRegion.DataBind();
                gvRegion.FocusedRowIndex = gvRegion.FindVisibleIndexByKeyValue(selectedRegion);

                if (!IsPostBack)
                    gvRegion.FocusedRowIndex = 0;

                gvCountry.DataBind();
                if (!IsPostBack)
                    gvCountry.FocusedRowIndex = 0;
                gvState.DataBind();
                if (!IsPostBack)
                    gvState.FocusedRowIndex = 0;

                gvLocation.DataBind();

                //  BindGriview();
                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam));
                hdnInformation.Set("EditUrl", editItem);
                hdnInformation.Set("RequestUrl", Server.UrlEncode(Request.Url.AbsolutePath));
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
        protected void chkShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            string listUrl = UGITUtility.GetAbsoluteURL(Request.Path);
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            queryCollection.Set("showarchive", chkShowDeleted.Checked.ToString());
            listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
            Context.Response.Redirect(listUrl);
            //dataTable = GetLocationData();
            //gvRegion.DataSource = GetRegions();
            //gvRegion.DataBind();
            //gvRegion.FocusedRowIndex = gvRegion.FindVisibleIndexByKeyValue(selectedRegion);

            //gvCountry.DataBind();
            //gvState.DataBind();
            //gvLocation.DataBind();

            //string showdelete = chkShowDeleted.Checked ? "1" : "0";
            //string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, viewParam, formTitle, showdelete));
            //Response.Redirect(url);
        }


        private List<Location> GetLocationData()
        {
            _SPList = ObjLocationManager.Load();
            if (_SPList.Count > 0)
            {
                List<Location> list = new List<Location>();
                if (!chkShowDeleted.Checked)
                {
                    list = _SPList.Where(x => !x.Deleted).OrderBy(x => x.Title).ToList();
                }
                else if (chkShowDeleted.Checked)
                {
                    list = _SPList.OrderBy(x => x.Title).ToList();
                }
                return list;
            }
            return null;
        }

        private DataTable GetRegions()
        {
            if (dataTable == null)
                return null;
            DataTable table = UGITUtility.ToDataTable(dataTable).DefaultView.ToTable(true, DatabaseObjects.Columns.UGITRegion, DatabaseObjects.Columns.Deleted);
            table.DefaultView.Sort = string.Format("{0} asc", DatabaseObjects.Columns.UGITRegion);
            return table.DefaultView.ToTable();
        }

        private DataTable GetCountries()
        {
            if (dataTable == null)
                return null;

            string query = string.Format("{0} = '{1}'", DatabaseObjects.Columns.UGITRegion, selectedRegion);
            if (string.IsNullOrEmpty(selectedRegion))
            {
                query = string.Format("{0} = '{1}' or {0} is null", DatabaseObjects.Columns.UGITRegion, selectedRegion);
            }
            DataRow[] rows = UGITUtility.ToDataTable(dataTable).Select(query);
            DataTable table = null;
            if (rows.Length > 0)
            {
                table = rows.CopyToDataTable().DefaultView.ToTable(true, DatabaseObjects.Columns.UGITCountry, DatabaseObjects.Columns.Deleted);
                table.DefaultView.Sort = string.Format("{0} asc", DatabaseObjects.Columns.UGITCountry);
                table = table.DefaultView.ToTable();
            }
            return table;
        }
        private DataTable GetStates()
        {
            if (dataTable == null)
                return null;

            List<string> expts = new List<string>();
            if (string.IsNullOrEmpty(selectedRegion))
                expts.Add(string.Format(" ({0} = '' or {0} is null) ", DatabaseObjects.Columns.UGITRegion));
            else
                expts.Add(string.Format("{0} = '{1}'", DatabaseObjects.Columns.UGITRegion, selectedRegion));

            if (string.IsNullOrEmpty(selectedCountry))
                expts.Add(string.Format(" ({0} = '' or {0} is null) ", DatabaseObjects.Columns.UGITCountry));
            else
                expts.Add(string.Format("{0} = '{1}'", DatabaseObjects.Columns.UGITCountry, selectedCountry));

            string query = string.Join(" And ", expts.ToArray());
            DataRow[] rows = UGITUtility.ToDataTable(dataTable).Select(query);
            DataTable table = null;
            if (rows.Length > 0)
            {
                table = rows.CopyToDataTable().DefaultView.ToTable(true, DatabaseObjects.Columns.UGITState, DatabaseObjects.Columns.Deleted);
                table.DefaultView.Sort = string.Format("{0} asc", DatabaseObjects.Columns.UGITState);
                table = table.DefaultView.ToTable();
            }

            return table;
        }
        private DataTable GetLocations()
        {
            if (dataTable == null)
                return null;

            List<string> expts = new List<string>();
            if (string.IsNullOrEmpty(selectedRegion))
                expts.Add(string.Format(" ({0} = '' or {0} is null) ", DatabaseObjects.Columns.UGITRegion));
            else
                expts.Add(string.Format("{0} = '{1}'", DatabaseObjects.Columns.UGITRegion, selectedRegion));

            if (string.IsNullOrEmpty(selectedCountry))
                expts.Add(string.Format(" ({0} = '' or {0} is null) ", DatabaseObjects.Columns.UGITCountry));
            else
                expts.Add(string.Format("{0} = '{1}'", DatabaseObjects.Columns.UGITCountry, selectedCountry));

            if (string.IsNullOrEmpty(selectedState))
                expts.Add(string.Format(" ({0} = '' or {0} is null) ", DatabaseObjects.Columns.UGITState));
            else
                expts.Add(string.Format("{0} = '{1}'", DatabaseObjects.Columns.UGITState, selectedState));

            string query = string.Join(" And ", expts.ToArray());

            DataRow[] rows = UGITUtility.ToDataTable(dataTable).Select(query);
            DataTable table = null;
            if (rows.Length > 0)
            {
                table = rows.CopyToDataTable();
                table.DefaultView.Sort = string.Format("{0} asc", DatabaseObjects.Columns.Title);
                table = table.DefaultView.ToTable();
            }

            return table;
        }

        protected void gvRegion_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

        }

        protected void gvCountry_DataBinding(object sender, EventArgs e)
        {
            selectedRegion = Convert.ToString(gvRegion.GetRowValues(gvRegion.FocusedRowIndex, DatabaseObjects.Columns.UGITRegion));
            gvCountry.DataSource = GetCountries();
        }

        protected void gvCountry_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

        }

        protected void gvCountry_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            gvCountry.PageIndex = 0;
        }

        protected void gvState_DataBinding(object sender, EventArgs e)
        {
            selectedRegion = Convert.ToString(gvRegion.GetRowValues(gvRegion.FocusedRowIndex, DatabaseObjects.Columns.UGITRegion));
            selectedCountry = Convert.ToString(gvCountry.GetRowValues(gvCountry.FocusedRowIndex, DatabaseObjects.Columns.UGITCountry));
            gvState.DataSource = GetStates();
        }

        protected void gvState_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

        }

        protected void gvState_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            gvState.PageIndex = 0;
        }

        protected void gvLocation_DataBinding(object sender, EventArgs e)
        {
            selectedRegion = Convert.ToString(gvRegion.GetRowValues(gvRegion.FocusedRowIndex, DatabaseObjects.Columns.UGITRegion));
            selectedCountry = Convert.ToString(gvCountry.GetRowValues(gvCountry.FocusedRowIndex, DatabaseObjects.Columns.UGITCountry));
            selectedState = Convert.ToString(gvState.GetRowValues(gvState.FocusedRowIndex, DatabaseObjects.Columns.UGITState));
            gvLocation.DataSource = GetLocations();
        }

        protected void gvLocation_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;
            bool archived = UGITUtility.StringToBoolean(e.GetValue(DatabaseObjects.Columns.Deleted));
            if (archived)
            {
                e.Row.CssClass += " archived-row";
            }
        }

        protected void gvLocation_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            gvLocation.PageIndex = 0;
        }

        protected void lnkSaveEdit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (hdnActionType.Value == "region")
            {
                string oldTitle = hdnTitle.Value;
                string newTitle = txtTitle.Text.Trim();
                if (oldTitle != newTitle)
                {
                    List<Location> collection = new List<Location>();
                    string query = string.Empty;
                    if (string.IsNullOrEmpty(oldTitle))
                        collection = _SPList.Where(x => x.Region == null || x.Region == string.Empty).ToList();
                    else
                        collection = _SPList.Where(x => x.Region == oldTitle).ToList();

                    foreach (Location item in collection)
                    {
                        item.Region = newTitle;
                        ObjLocationManager.AddOrUpdate(item);
                    }
                    Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Update region details", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                }
            }
            else if (hdnActionType.Value == "country")
            {
                selectedRegion = Convert.ToString(gvRegion.GetRowValues(gvRegion.FocusedRowIndex, DatabaseObjects.Columns.UGITRegion));


                string oldTitle = hdnTitle.Value;
                string newTitle = txtTitle.Text.Trim();
                if (oldTitle != newTitle)
                {
                    List<Location> collection = new List<Location>();
                    List<string> expressions = new List<string>();
                    if (string.IsNullOrEmpty(selectedRegion))
                        collection = _SPList.Where(x => x.Region == null || x.Region == string.Empty).ToList();
                    else
                        collection = _SPList.Where(x => x.Region == selectedRegion).ToList();

                    if (string.IsNullOrEmpty(oldTitle))
                        collection = collection.Where(x => x.Country == null || x.Country == string.Empty).ToList();
                    else
                        collection = collection.Where(x => x.Country == oldTitle).ToList();

                    foreach (Location item in collection)
                    {
                        item.Country = newTitle;
                        ObjLocationManager.AddOrUpdate(item);
                    }
                    Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Update country details", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                }
            }
            else if (hdnActionType.Value == "state")
            {
                selectedRegion = Convert.ToString(gvRegion.GetRowValues(gvRegion.FocusedRowIndex, DatabaseObjects.Columns.UGITRegion));
                selectedCountry = Convert.ToString(gvCountry.GetRowValues(gvCountry.FocusedRowIndex, DatabaseObjects.Columns.UGITCountry));

                string oldTitle = hdnTitle.Value;
                string newTitle = txtTitle.Text.Trim();
                if (oldTitle != newTitle)
                {
                    List<Location> collection = new List<Location>();
                    List<string> expressions = new List<string>();
                    if (string.IsNullOrEmpty(selectedRegion))
                        collection = _SPList.Where(x => x.Region == null || x.Region == string.Empty).ToList();// expressions.Add(string.Format("<Or><Eq><FieldRef Name='{0}'/><Value Type='Text'></Value></Eq><IsNull><FieldRef Name='{0}'/></IsNull></Or>", DatabaseObjects.Columns.UGITRegion));
                    else
                        collection = _SPList.Where(x => x.Region == selectedRegion).ToList();

                    if (string.IsNullOrEmpty(selectedCountry))
                        collection = collection.Where(x => x.Country == null || x.Country == string.Empty).ToList();//expressions.Add(string.Format("<Or><Eq><FieldRef Name='{0}'/><Value Type='Text'></Value></Eq><IsNull><FieldRef Name='{0}'/></IsNull></Or>", DatabaseObjects.Columns.UGITCountry));
                    else
                        collection = collection.Where(x => x.Country == selectedCountry).ToList();//expressions.Add(string.Format("<Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq>", DatabaseObjects.Columns.UGITCountry, selectedCountry));

                    if (string.IsNullOrEmpty(oldTitle))
                        collection = collection.Where(x => x.State == null || x.State == string.Empty).ToList();// expressions.Add(string.Format("<Or><Eq><FieldRef Name='{0}'/><Value Type='Text'></Value></Eq><IsNull><FieldRef Name='{0}'/></IsNull></Or>", DatabaseObjects.Columns.UGITState));
                    else
                        collection = collection.Where(x => x.State == oldTitle).ToList();//expressions.Add(string.Format("<Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq>", DatabaseObjects.Columns.UGITState, oldTitle));

                    foreach (Location item in collection)
                    {
                        item.State = newTitle;
                        ObjLocationManager.AddOrUpdate(item);
                    }
                    Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Update state details", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                }
            }


            editBox.ShowOnPageLoad = false;
            dataTable = GetLocationData();
            gvRegion.DataSource = GetRegions();
            gvRegion.DataBind();
            gvCountry.DataBind();
            gvState.DataBind();
            gvLocation.DataBind();

            LoadRegions(dataTable);
            LoadCountry(dataTable);
            LoadState(dataTable);

        }

        protected void btnSaveLocation_Click(object sender, EventArgs e)
        {
            // Commented as upgrading to DevExpress 19.1.3, below condition is failing
            //if (!Page.IsValid)
            //    return;

            int id = 0;
            int.TryParse(hdnSelectedLocation.Value, out id);
            Location _SPListItem = ObjLocationManager.LoadByID(id);
            if (_SPListItem == null)
                _SPListItem = new Location();


            _SPListItem.Title = txtLocationTitle.Text.Trim();
            _SPListItem.LocationDescription = txtDescription.Text.Trim();
            _SPListItem.Deleted = chkDeleted.Checked;

            string region = ddlRegion.SelectedValue;
            if (txtRegion.Text.Trim() != string.Empty)
                region = txtRegion.Text.Trim();
            if (!string.IsNullOrEmpty(region))
                _SPListItem.Region = region;


            string country = ddlCountry.SelectedValue;
            if (txtCountry.Text.Trim() != string.Empty)
                country = txtCountry.Text.Trim();
            if (!string.IsNullOrEmpty(country))
                _SPListItem.Country = country;

            string state = ddlState.SelectedValue;
            if (txtState.Text.Trim() != string.Empty)
                state = txtState.Text.Trim();
            if (!string.IsNullOrEmpty(state))
                _SPListItem.State = state;

            ObjLocationManager.AddOrUpdate(_SPListItem);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added/Updated location: {_SPListItem.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);

            hdnSelectedLocation.Value = string.Empty;
            txtLocationTitle.Text = string.Empty;
            txtDescription.Text = string.Empty;
            chkDeleted.Checked = false;
            ddlRegion.ClearSelection();
            txtRegion.Text = string.Empty;
            ddlCountry.ClearSelection();
            txtCountry.Text = string.Empty;
            ddlState.ClearSelection();
            txtState.Text = string.Empty;
            editLocationBox.ShowOnPageLoad = false;
            dataTable = GetLocationData();
            gvRegion.DataSource = GetRegions();
            gvRegion.DataBind();
            gvCountry.DataBind();
            gvState.DataBind();
            gvLocation.DataBind();
            LoadRegions(dataTable);
            LoadCountry(dataTable);
            LoadState(dataTable);
        }

        private void LoadRegions(List<Location> locationTable)
        {
            if (locationTable != null && locationTable.Count > 0)
            {
                ddlRegion.DataSource = locationTable.DistinctBy(x => x.Region).ToList();
                ddlRegion.DataTextField = DatabaseObjects.Columns.UGITRegion;
                ddlRegion.DataValueField = DatabaseObjects.Columns.UGITRegion;
                ddlRegion.DataBind();

                if (!string.IsNullOrEmpty(Convert.ToString(locationTable[0].Region)))
                {
                    ddlRegion.Items.Insert(0, new ListItem(string.Empty, string.Empty));
                }
            }

        }
        private void LoadCountry(List<Location> locationTable)
        {
            if (locationTable != null && locationTable.Count > 0)
            {
                ddlCountry.DataSource = locationTable.DistinctBy(x => x.Country).ToList();
                ddlCountry.DataValueField = DatabaseObjects.Columns.UGITCountry;
                ddlCountry.DataTextField = DatabaseObjects.Columns.UGITCountry;
                ddlCountry.DataBind();

                if (!string.IsNullOrEmpty(Convert.ToString(locationTable[0].Country)))
                {
                    ddlCountry.Items.Insert(0, new ListItem(string.Empty, string.Empty));
                }
            }
        }
        private void LoadState(List<Location> locationTable)
        {
            if (locationTable != null && locationTable.Count > 0)
            {
                ddlState.DataSource = locationTable.DistinctBy(x => x.State).ToList();
                ddlState.DataValueField = DatabaseObjects.Columns.UGITState;
                ddlState.DataTextField = DatabaseObjects.Columns.UGITState;
                ddlState.DataBind();
                if (!string.IsNullOrEmpty(Convert.ToString(locationTable[0].State)))
                {
                    ddlState.Items.Insert(0, new ListItem(string.Empty, string.Empty));
                }
            }
        }
        private void FillWithSelectedLocation(Location _SPListItem)
        {
            hdnSelectedLocation.Value = _SPListItem.ID.ToString();
            txtLocationTitle.Text = _SPListItem.Title;
            txtDescription.Text = _SPListItem.LocationDescription;
            chkDeleted.Checked = _SPListItem.Deleted;

            ddlRegion.SelectedIndex = ddlRegion.Items.IndexOf(ddlRegion.Items.FindByValue(Convert.ToString(_SPListItem.Region)));
            ddlCountry.SelectedIndex = ddlCountry.Items.IndexOf(ddlCountry.Items.FindByValue(Convert.ToString(_SPListItem.Country)));
            ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(Convert.ToString(_SPListItem.State)));
        }
        protected void btLocationEdit_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btEditLocation = (ImageButton)sender;
            int id = Convert.ToInt32(btEditLocation.CommandArgument);
            Location item = ObjLocationManager.LoadByID(id);
            if (item != null)
            {
                FillWithSelectedLocation(item);
                editLocationBox.ShowOnPageLoad = true;
            }
        }
        protected void lnkLocationEdit_Click(object sender, EventArgs e)
        {
            LinkButton lnkEditLocation = (LinkButton)sender;
            int id = Convert.ToInt32(lnkEditLocation.CommandArgument);
            Location item = ObjLocationManager.LoadByID(id);
            if (item != null)
            {
                FillWithSelectedLocation(item);
                editLocationBox.ShowOnPageLoad = true;
            }


        }

        protected void btnApplyChanges_Click(object sender, EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            string cacheName = "Lookup_" + DatabaseObjects.Tables.Location + "_" + context.TenantID;
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Location, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            Util.Cache.CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);

            cacheName = "Lookup_" + DatabaseObjects.Tables.SubLocation + "_" + context.TenantID;
            dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SubLocation, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            Util.Cache.CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
