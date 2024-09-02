using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Xml;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using uGovernIT.Util.Cache;
using DevExpress.DocumentServices.ServiceModel.DataContracts;
using System.Collections;
using System.IO;
using DevExpress.Spreadsheet;

namespace uGovernIT.Manager
{
    public class AssetHelper
    {
        DataTable assetList;
        List<string> moduleTickets = new List<string>();
        ConfigurationVariableManager configManager = null;
        ApplicationContext context = null;
        public AssetHelper(ApplicationContext spWeb)
        {
            context = spWeb;
            assetList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Assets, $"TenantID='{context.TenantID}'");
            configManager = new ConfigurationVariableManager(spWeb);
        }

        public void UpdateSolarwindAssets()
        {
            AssetIntegrationConfiguration iniobj = new AssetIntegrationConfiguration();
            string configurations = configManager.GetValue(ConfigConstants.AssetIntegrationConfigurations);

            if (!string.IsNullOrWhiteSpace(configurations))
            {
                try
                {
                    XmlDocument xmlDocCtnt = new XmlDocument();
                    xmlDocCtnt.LoadXml(configurations);
                    iniobj = (AssetIntegrationConfiguration)uHelper.DeSerializeAnObject(xmlDocCtnt, iniobj);

                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
            if (iniobj != null)
            {
                if (iniobj.EnableAssetIntegration)
                {
                    SolarwindConfig config = new SolarwindConfig(iniobj.APIURL, iniobj.APIKey);
                    Solarwind solarWind = new Solarwind(config);

                    //fetch assets page wise and update into system assets list
                    int page = 1;
                    int currentAssets = 0;
                    do
                    {

                        List<SolarwindAssetModel> sAssets = solarWind.GetAssets(page);
                        currentAssets = sAssets.Count;
                        page += 1;

                        Ticket assetTicket = new Ticket(context, "CMDB");

                        foreach (SolarwindAssetModel asset in sAssets)
                        {
                            UpdateAsset(assetTicket, asset);
                        }
                        //SPListHelper.ReloadTicketsInCache(moduleTickets, spWeb);
                    } while (currentAssets == config.Limit);
                }
            }


        }

        private DataRow GetAssetItem(string externalID)
        {
            string query = "";
            //query.ViewFields = string.Format("<FieldRef Name='{0}/>", DatabaseObjects.Columns.Id);
            //query.ViewFieldsOnly = true;
            query = string.Format("{0}={1} and {2}={3}", DatabaseObjects.Columns.ExternalTicketID, externalID, DatabaseObjects.Columns.ExternalType, "Solarwind");

            DataRow[] itemColl = assetList.Select(query);

            if (itemColl.Count() == 0)
                return null;

            return itemColl[0];
        }

        private void UpdateAsset(Ticket assetTicket, SolarwindAssetModel asset)
        {
            DataRow item = GetAssetItem(asset.id.ToString());

            if (item == null)
            {
                //Create asset if not persent in current list
                item = assetList.NewRow();
                item[DatabaseObjects.Columns.ExternalTicketID] = asset.id.ToString();
                item[DatabaseObjects.Columns.ExternalType] = "Solarwind";
                assetTicket.Create(item, context.CurrentUser);
                assetTicket.CommitChanges(item, "", donotUpdateEscalations: true);
            }
            else
            {
                //fetch full asset detail to update fields
                item = assetList.Select(string.Format("{0}={1}", DatabaseObjects.Columns.ID, item["ID"]))[0];
            }
            moduleTickets.Add(Convert.ToString(item[DatabaseObjects.Columns.TicketId]));
            //Create column value object againt changed value
            List<TicketColumnValue> columnValues = new List<TicketColumnValue>();
            TicketColumnValue column = null;

            column = new TicketColumnValue();
            column.InternalFieldName = DatabaseObjects.Columns.Title;
            column.Value = asset.id + " Solarwinds";
            columnValues.Add(column);

            column = new TicketColumnValue();
            column.InternalFieldName = DatabaseObjects.Columns.NICAddress;
            column.Value = asset.macAddress;
            columnValues.Add(column);

            column = new TicketColumnValue();
            column.InternalFieldName = DatabaseObjects.Columns.IPAddress;
            column.Value = asset.networkAddress;
            columnValues.Add(column);

            column = new TicketColumnValue();
            column.InternalFieldName = DatabaseObjects.Columns.AssetDescription;
            column.Value = asset.Notes;
            columnValues.Add(column);

            column = new TicketColumnValue();
            column.InternalFieldName = DatabaseObjects.Columns.AcquisitionDate;
            column.Value = asset.purchaseDate;
            columnValues.Add(column);

            column = new TicketColumnValue();
            column.InternalFieldName = DatabaseObjects.Columns.HostName;
            column.Value = asset.networkName;
            columnValues.Add(column);

            column = new TicketColumnValue();
            column.InternalFieldName = DatabaseObjects.Columns.SerialAssetDetail;
            column.Value = asset.serialNumber;
            columnValues.Add(column);

            if (asset.warrantType != null)
            {
                column = new TicketColumnValue();
                column.InternalFieldName = DatabaseObjects.Columns.WarrantyType;
                column.Value = asset.warrantType.type;
                columnValues.Add(column);
            }

            if (asset.warrantyExpirationDate.HasValue)
            {
                column = new TicketColumnValue();
                column.InternalFieldName = DatabaseObjects.Columns.WarrantyExpirationDate;
                column.Value = asset.warrantyExpirationDate.Value;
                columnValues.Add(column);
            }
            if (asset.location != null)
            {
                column = new TicketColumnValue();
                column.InternalFieldName = DatabaseObjects.Columns.LocationLookup;
                column.Value = setLocation(asset.location.locationName);
                if (Convert.ToInt32(column.Value) > 0)
                    columnValues.Add(column);
            }
            if (asset.model != null && asset.model.manufacturer != null)
            {
                column = new TicketColumnValue();
                column.InternalFieldName = DatabaseObjects.Columns.VendorLookup;
                column.Value = setAssetVendors(asset.model.manufacturer);
                if (Convert.ToInt32(column.Value) > 0)
                    columnValues.Add(column);
            }

            if (asset.model != null)
            {
                column = new TicketColumnValue();
                column.InternalFieldName = DatabaseObjects.Columns.AssetModelLookup;
                column.Value = setAssetModels(asset.model);
                if (Convert.ToInt32(column.Value) > 0)
                    columnValues.Add(column);
            }

            assetTicket.SetItemValues(item, columnValues, true, true, "");
            assetTicket.CommitChanges(item, "");
        }

        private int setLocation(string location)
        {
            int locationLookupID = 0;
            DataTable locationList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Location, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            if (locationList == null)
                return 0;

            //SPQuery query = new SPQuery();
            //query.ViewFields = string.Format("<FieldRef Name='{0}/>", DatabaseObjects.Columns.Id);
            //query.ViewFieldsOnly = true;
            string query = string.Format("{0}='{1}'",
                DatabaseObjects.Columns.Title, location);

            DataRow[] itemColl = locationList.Select(query);
            if (itemColl.Count() == 0)
                return 0;

            locationLookupID = Convert.ToInt32(itemColl[0]["ID"]);
            return locationLookupID;
        }

        private int setAssetVendors(SolarwindAssetModel.Manufacturer vendor)
        {
            int vendorLookupID = 0;
            DataTable vendorsList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AssetVendors, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            if (vendorsList == null)
                return 0;
            // SPQuery query = new SPQuery();
            //query.ViewFields = string.Format("<FieldRef Name='{0}/>", DatabaseObjects.Columns.Id);
            //query.ViewFieldsOnly = true;
            string query = string.Format("{0}='{1}' and {2}='{3}'",
                DatabaseObjects.Columns.Title, vendor.name, DatabaseObjects.Columns.ExternalType, "Solarwind");

            DataRow[] itemColl = vendorsList.Select(query);
            if (itemColl.Count() == 0)
            {
                DataRow item = vendorsList.NewRow();//if vendor not found then add with externaltype as solarwind
                item[DatabaseObjects.Columns.ExternalType] = "Solarwind";
                item[DatabaseObjects.Columns.Title] = vendor.name;
                item[DatabaseObjects.Columns.VendorName] = vendor.name;
                item[DatabaseObjects.Columns.Location] = vendor.city + "," + " " + vendor.state + "," + " " + vendor.country;
                item[DatabaseObjects.Columns.VendorPhone] = vendor.phone;
                item[DatabaseObjects.Columns.VendorAddress] = vendor.address;
                item[DatabaseObjects.Columns.Deleted] = false;
                item.AcceptChanges();
                vendorLookupID = Convert.ToInt32(item["ID"]);
            }
            else
            {
                vendorLookupID = Convert.ToInt32(itemColl[0]["ID"]);
            }
            return vendorLookupID;
        }

        private int setAssetModels(SolarwindAssetModel.Model assetModel)
        {
            int AssetModelID = 0;
            DataTable assetModelList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AssetModels, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            if (assetModelList == null)
                return 0;
            //SPQuery query = new SPQuery();
            //query.ViewFields = string.Format("<FieldRef Name='{0}/>", DatabaseObjects.Columns.Id);
            ////  query.ViewFieldsOnly = true;
            string query = string.Format("{0}='{1}' and {2}='{3}'",
                DatabaseObjects.Columns.Title, assetModel.manufacturer.name + "-" + assetModel.modelName, DatabaseObjects.Columns.ExternalType, "Solarwind");

            DataRow[] itemColl = assetModelList.Select(query);
            if (itemColl.Count() == 0)
            {
                DataRow _SPListItem = assetModelList.NewRow();//if vendor not found then add with externaltype as solarwind
                _SPListItem[DatabaseObjects.Columns.ExternalType] = "Solarwind";
                _SPListItem[DatabaseObjects.Columns.Title] = assetModel.manufacturer.name + "-" + assetModel.modelName;
                _SPListItem[DatabaseObjects.Columns.ModelName] = assetModel.modelName;
                _SPListItem[DatabaseObjects.Columns.VendorLookup] = setAssetVendors(assetModel.manufacturer);
                _SPListItem[DatabaseObjects.Columns.Deleted] = false;
                // _SPListItem.Update();
                AssetModelID = Convert.ToInt32(_SPListItem["ID"]);
            }
            else
            {
                AssetModelID = Convert.ToInt32(itemColl[0]["ID"]);
            }
            return AssetModelID;
        }

        /// <summary>
        /// Return Assets owned by or assigned to specified user. Returns ALL assets if no user passed in.
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static DataTable GetUserAssets(ApplicationContext _context, string userID)
        {
            return GetUserAssets(_context, _context.UserManager.GetUserById(userID));
        }

        /// <summary>
        /// Return Assets owned by or assigned to specified user. Returns ALL assets if no user passed in.
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static DataTable GetUserAssets(ApplicationContext context, UserProfile user)
        {
            DataTable userAssets = null;
            if (context != null)
            {
                ModuleViewManager moduleViewManager = new ModuleViewManager(context);
                UGITModule cmdbModule = moduleViewManager.LoadByName("CMDB");
                TicketManager ticketManager = new TicketManager(context);
                DataTable allAssets = ticketManager.GetOpenTickets(cmdbModule);
                if (allAssets != null && allAssets.Rows.Count > 0)
                {
                    if (user != null)
                    {
                        //Filter all the assets where current user is assetower or current user
                        var xbase = allAssets.AsEnumerable().Where(x => (!x.IsNull(DatabaseObjects.Columns.Owner) && x.Field<string>(DatabaseObjects.Columns.Owner) == user.Id) ||
                                                                        (!x.IsNull(DatabaseObjects.Columns.CurrentUser) && x.Field<string>(DatabaseObjects.Columns.CurrentUser) == user.Id));
                        if (xbase.Count() > 0)
                            userAssets = xbase.CopyToDataTable();
                        else
                            userAssets = allAssets.Clone();
                    }
                    else
                        userAssets = allAssets; // if user not passed in, return ALL assets
                }
            }
            else
            {
                ModuleColumnHelper moduleColumnHelper = new ModuleColumnHelper(context);
                List<ModuleColumn> moduleColumns = moduleColumnHelper.LoadByModule("CMDB");
                string query = "";
                if (user != null)
                {
                    query = string.Format(@"{0}='{2}' ||{1}='{2}'",
                                                DatabaseObjects.Columns.AssetOwner, DatabaseObjects.Columns.CurrentUser, user.Id);
                }

                DataTable dt = (DataTable)CacheHelper<object>.Get($"OpenTicket_{ModuleNames.CMDB}", context.TenantID);
                if (dt == null)
                {
                    Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                    keyValuePairs.Add(DatabaseObjects.Columns.TenantID, context.TenantID);
                    dt = GetTableDataManager.GetData(DatabaseObjects.Tables.Assets, keyValuePairs);

                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] rowColl = dt.Select(query);
                    if (rowColl == null || rowColl.Length == 0)
                        dt = null;
                    else
                        dt = rowColl.CopyToDataTable();

                }

                userAssets = dt;
            }

            return userAssets;
        }

        public static DataTable GetAssetsByDepartment(ApplicationContext context, int departmentID)
        {
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            UGITModule cmdbModuleConfig = moduleViewManager.LoadByName("CMDB");
            string query = string.Format("{0}='{1}'", DatabaseObjects.Columns.DepartmentLookup, departmentID);
            DataTable dt= (DataTable)CacheHelper<object>.Get($"OpenTicket_{ModuleNames.CMDB}", context.TenantID);
            if (dt == null)
            {
                Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                keyValuePairs.Add(DatabaseObjects.Columns.TenantID, context.TenantID);
                dt = GetTableDataManager.GetData(DatabaseObjects.Tables.Assets, keyValuePairs);
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] rowColl = dt.Select(query);
                if (rowColl == null || rowColl.Length == 0)
                    dt = null;
                else
                    dt = rowColl.CopyToDataTable();

            }

            return dt;
        }

        public static Control GetAssetLink(string value, DataColumn field, string tabId, string viewUrl, string sourceURL)
        {
            Panel mlfPanel = new Panel();
            if (string.IsNullOrEmpty(value))
                return mlfPanel;
            DataTable dtAssets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Assets, $"{DatabaseObjects.Columns.ID} in ({value})");

            if (dtAssets == null || dtAssets.Rows.Count == 0)
                return mlfPanel;

            DataRow asset;
            int count = 0;
            foreach (var lookup in value.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries))
            {
                count++;
                asset = dtAssets.NewRow();
                asset = dtAssets.Select($"{DatabaseObjects.Columns.ID} = {lookup}")[0];
                if (asset != null)
                {
                    string title = string.Format("{0}: {1}", Convert.ToString(asset[DatabaseObjects.Columns.TicketId]), Convert.ToString(asset[DatabaseObjects.Columns.AssetTagNum]));
                    HtmlAnchor anchor = new HtmlAnchor();
                    anchor.ID = GenerateID(field, tabId).Replace("-", "") + "_view" + "_" + count;
                    anchor.InnerText = Convert.ToString(asset[DatabaseObjects.Columns.AssetTagNum]);
                    anchor.Style.Add("cursor", "pointer");
                    anchor.Style.Add("text-decoration", "none");
                    anchor.Attributes.Add("onclick", string.Format("javascript:openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", asset[DatabaseObjects.Columns.TicketId]), title, sourceURL, 90, 90));
                    anchor.Attributes.Add("onmouseover", string.Format("item_mousehover(this, \'{0}\',\'{1}\')", lookup, "Assets"));
                    anchor.Attributes.Add("onmouseout", string.Format("item_mouseout(this, \'{0}\',\'{1}\')", lookup, "Assets"));
                    mlfPanel.Controls.Add(anchor);
                    if (count < value.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries).Length)
                        mlfPanel.Controls.Add(new LiteralControl("<span>, &nbsp;</span>"));
                }
            }
            return mlfPanel;
        }

        private static string GenerateID(DataColumn field, string tabId)
        {
            return field.ColumnName + '_' + tabId;
        }

        public static Panel GetAssetModelLink(string value, DataColumn field, string tabId, string sourceURL)
        {
            Panel mlfPanel = new Panel();
            DataTable dtVendors = new DataTable();
            DataTable dtAssetModels = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AssetModels, $"{DatabaseObjects.Columns.ID} in ({value})");
            if (dtAssetModels != null && dtAssetModels.Rows.Count > 0)
            {
                string vendorIds = string.Join(Constants.Separator6, dtAssetModels.AsEnumerable().Select(x => x.Field<Int64>(DatabaseObjects.Columns.VendorLookup)));
                dtVendors = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AssetVendors, $"{DatabaseObjects.Columns.ID} in ({vendorIds})", "ID,VendorName,Title", null);
            }
            if (dtAssetModels == null || dtAssetModels.Rows.Count == 0)
                return mlfPanel;

            DataRow assetModelRowColl;
            DataRow assetVendorRowColl;
            int count = 0;

            string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}";
            string editParam = "assetmodeledit";
            string vendorName = string.Empty;
            foreach (var lookup in value.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries))
            {
                count++;
                assetModelRowColl = dtAssetModels.NewRow();
                assetModelRowColl = dtAssetModels.Select($"{DatabaseObjects.Columns.ID} = {lookup}")[0];
                assetVendorRowColl = dtVendors.NewRow();
                if (Convert.ToInt32(assetModelRowColl[DatabaseObjects.Columns.VendorLookup]) > 0)
                {
                    assetVendorRowColl = dtVendors.Select($"{DatabaseObjects.Columns.ID} = {assetModelRowColl[DatabaseObjects.Columns.VendorLookup]}")[0];
                    vendorName = Convert.ToString(assetVendorRowColl[DatabaseObjects.Columns.VendorName]);
                }
                else
                    vendorName = "";
                if (assetModelRowColl != null)
                {
                    string title = string.Format("{0}: {1}", vendorName, Convert.ToString(assetModelRowColl[DatabaseObjects.Columns.ModelName]));
                    HtmlAnchor anchor = new HtmlAnchor();
                    anchor.ID = GenerateID(field, tabId).Replace("-", "") + "_view" + "_" + count;
                    anchor.InnerText = Convert.ToString(assetModelRowColl[DatabaseObjects.Columns.ModelName]);
                    anchor.Style.Add("cursor", "pointer");
                    anchor.Style.Add("text-decoration", "none");
                    string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, Convert.ToString(assetModelRowColl[DatabaseObjects.Columns.Id])));
                    string jsFunc = string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','600px','320px',0,'')", editItem, title);
                    anchor.Attributes.Add("href", jsFunc);
                    anchor.Attributes.Add("onmouseover", string.Format("item_mousehover(this, \'{0}\',\'{1}\')", lookup, "AssetModels"));
                    anchor.Attributes.Add("onmouseout", string.Format("item_mouseout(this, \'{0}\',\'{1}\')", lookup, "AssetModels"));
                    mlfPanel.Controls.Add(anchor);
                    if (count < value.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries).Length)
                        mlfPanel.Controls.Add(new LiteralControl("<span>, &nbsp;</span>"));
                }
            }
            return mlfPanel;
        }
        public static void AssetImport (ApplicationContext _context, string filename, ImportStatus status, string Modulename)
        {
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(_context);
            string errorMessage = string.Empty;
            Workbook workbook = new Workbook();
            workbook.LoadDocument(filename);
            CellRange range = workbook.Worksheets[0].GetDataRange();
            DataTable data = workbook.Worksheets[0].CreateDataTable(range, true);
            if (data == null)
            {
                errorMessage = "No data found in file!";
                status.succeeded = false;
                status.errorMessages.Add(errorMessage);
                status.recordsProcessed = 0;
                return;
            }

            string workSheetName = workbook.Worksheets[0].Name;
            List<FieldAliasCollection> templstColumn = FieldAliasCollection.FillFieldAliasCollection();
            string listName = DatabaseObjects.Tables.Assets;
            string uniqueFieldName = configurationVariableManager.GetValue(ConfigConstants.AssetUniqueTag); // Internal name of unique field
            if (string.IsNullOrWhiteSpace(uniqueFieldName))
                uniqueFieldName = DatabaseObjects.Columns.AssetTagNum;
            List<FieldAliasCollection> listFields = templstColumn.Where(r => r.ListName == listName).ToList();
            DataTable spList = GetTableDataManager.GetTableData(listName, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
            List<string> lstColumn = data.Columns.Cast<DataColumn>().Select(x => x.ColumnName.Trim()).ToList();
            Ticket ticket = new Ticket(_context, Modulename);

            // Mandatory checks
            string mandatoryColumnName = string.Empty; // Excel column name of unique mandatory field
            List<FieldAliasCollection> mandatoryFields = new List<FieldAliasCollection>();
            mandatoryFields = listFields.Where(r => r.InternalName == uniqueFieldName).ToList();
            bool checkMandatory = false;
            if (mandatoryFields.Count > 0)
            {
                string[] aliasName = mandatoryFields[0].AliasNames.Split(',');
                foreach (string alias in aliasName)
                {
                    if (lstColumn.Contains(alias))
                    {
                        checkMandatory = true;
                        mandatoryColumnName = alias;
                        break;
                    }
                }
            }

            if (!checkMandatory)
            {
                //Log.WriteLog(string.Format("{0} is mandatory in excel for {1}", uniqueFieldName, listName));
                errorMessage = string.Format("{0} is mandatory in excel", UGITUtility.AddSpaceBeforeWord(uniqueFieldName));
                status.succeeded = false;
                status.errorMessages.Add(errorMessage);
                status.recordsProcessed = 0;
                return;
            }
            List<string> error = new List<string>();
            List<string> alreadyexist = new List<string>();
            int lastIndex = workbook.Worksheets[0].Rows.LastUsedIndex;

            status.totalRecords = lastIndex;

            DataColumn assetTagfield = null;

            if (spList.Columns.Contains(uniqueFieldName))
                assetTagfield = spList.Columns[uniqueFieldName];

            int assetTagIndex = -1;
            if (assetTagfield != null)
                assetTagIndex = lstColumn.FindIndex(s => s.Equals(mandatoryColumnName, StringComparison.OrdinalIgnoreCase));

            Hashtable existingAssets = new Hashtable();
            if (assetTagIndex != -1)
            {
                DataRow[] assetCollection = spList.Select();
                if (assetCollection != null && assetCollection.Count() > 0)
                {
                    foreach (DataRow item in assetCollection)
                    {
                        string assetTag = Convert.ToString(item[assetTagfield.ColumnName]);
                        if (!existingAssets.ContainsKey(assetTag))
                        {
                            existingAssets.Add(assetTag, Convert.ToString(item[DatabaseObjects.Columns.Id]));
                        }
                        else
                            ULog.WriteLog("ERROR: Found duplicate asset tag in existing assets - " + assetTag);
                    }
                    ULog.WriteLog(string.Format("Loaded list of {0} existing assets", existingAssets.Count));
                }
            }
            else
            {
                // SHOULD NOT HAPPEN since we are already checking for mandatory column further up!
                //Log.WriteLog(string.Format("{0} is mandatory in excel for {1}", uniqueFieldName, listName));
                errorMessage = string.Format("{0} is mandatory in excel", UGITUtility.AddSpaceBeforeWord(uniqueFieldName));
                status.succeeded = false;
                status.errorMessages.Add(errorMessage);
                status.recordsProcessed = 0;
                return;
            }

            List<string> updatedTickets = new List<string>();
            Dictionary<string, DataTable> lookupLists = GetLookupLists(_context, ModuleNames.CMDB);
            Hashtable importedAssets = new Hashtable();
            for (int i = 1; i <= lastIndex; i++)
            {
                status.recordsProcessed++;

                DataRow listItem = null;
                Row row = workbook.Worksheets[0].Rows[i];
                string assetTag = row[assetTagIndex].DisplayText;
                if (string.IsNullOrWhiteSpace(assetTag))
                {
                    errorMessage = string.Format("Line # {0}: Ignoring record with missing {1}", i + 1, assetTagfield.ColumnName);
                    //Log.WriteLog(errorMessage);
                    status.errorMessages.Add(errorMessage);
                    status.recordsSkipped++;
                    continue;
                }

                bool isExistingAsset = false;
                if (importedAssets.ContainsKey(assetTag))
                {
                    errorMessage = string.Format("Line # {0}: Skipping duplicate asset tag {1}", i + 1, assetTag);
                    //Log.WriteLog(errorMessage);
                    status.errorMessages.Add(errorMessage);
                    status.recordsSkipped++;
                    continue;
                }
                else if (existingAssets.ContainsKey(assetTag))
                {
                    int assetID = UGITUtility.StringToInt(existingAssets[assetTag]);
                    listItem = spList.Select("ID=" + assetID)[0];
                    if (listItem == null)
                    {
                        errorMessage = string.Format("ERROR retrieving existing asset with tag # {0}, ID {1}", assetTag, assetID);
                        //Log.WriteLog(errorMessage);
                        status.errorMessages.Add(errorMessage);
                        status.recordsSkipped++;
                        continue;
                    }
                    //Log.WriteLog(string.Format("Updating asset with tag # {0}, ID {1}", assetTag, assetID));
                    isExistingAsset = true;
                }
                else
                {
                    listItem = spList.NewRow();
                    //Log.WriteLog("Importing new asset with tag # " + assetTag);
                }

                List<TicketColumnValue> formValues = new List<TicketColumnValue>();

                uHelper.SetFilledValues(_context, lstColumn, row, listItem, listFields, formValues, lookupLists, moduleName: ModuleNames.CMDB);

                //ticket.SetItemValues(listItem, formValues, true, false, User.Id);
                 
                if (UGITUtility.StringToInt(listItem["ID"]) == 0)
                    ticket.Create(listItem, _context.CurrentUser);

                LifeCycle defaultLifeCycle = ticket.Module.List_LifeCycles.FirstOrDefault();


                if (defaultLifeCycle != null)
                {
                    LifeCycleStage currentStage = null;
                    TicketColumnValue assetStage = null;
                    if (formValues.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.TicketStatus))
                    {
                        assetStage = formValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketStatus);
                        if (assetStage != null)
                        {
                            currentStage = defaultLifeCycle.Stages.FirstOrDefault(x => x.Name == Convert.ToString(assetStage.Value));
                            listItem[assetStage.InternalFieldName] = assetStage.Value;
                        }
                    }
                    else if (formValues.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.StageStep))
                    {
                        assetStage = formValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.StageStep);
                        if (assetStage != null)
                        {
                            currentStage = defaultLifeCycle.Stages.FirstOrDefault(x => x.StageStep == UGITUtility.StringToInt(assetStage.Value));
                            listItem[assetStage.InternalFieldName] = assetStage.Value;
                        }
                    }

                    if (currentStage != null)
                    {
                        if (UGITUtility.IfColumnExists(listItem, DatabaseObjects.Columns.StageStep))
                        {
                            listItem[DatabaseObjects.Columns.StageStep] = currentStage.StageStep;
                        }
                    }

                    if (currentStage != null)
                        Ticket.SetStageSpecificFields(_context,listItem, currentStage);
                    else if (!isExistingAsset)
                        ticket.QuickClose(uHelper.getModuleIdByModuleName(_context, Modulename), listItem, string.Empty);
                }

                string innererror = ticket.CommitChanges(listItem, donotUpdateEscalations: true, stopUpdateDependencies: true);
                if (!string.IsNullOrEmpty(innererror))
                {
                    error.Add(innererror);
                    status.recordsSkipped++;
                }
                else if (isExistingAsset)
                    status.recordsUpdated++;
                else
                    status.recordsAdded++;

                importedAssets.Add(assetTag, listItem["ID"]);
                string ticketID = Convert.ToString(listItem[DatabaseObjects.Columns.TicketId]);
                if (!string.IsNullOrEmpty(ticketID))
                    updatedTickets.Add(Convert.ToString(listItem[DatabaseObjects.Columns.TicketId]));
            }

            string message = string.Format("Asset Import from {0} successful: {1} new assets imported, {2} updated, {3} skipped, {4} processed",
                                            Path.GetFileName(filename), status.recordsAdded, status.recordsUpdated, status.recordsSkipped, status.recordsProcessed); ;
            ULog.WriteUGITLog(_context.CurrentUser.Id, message, Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), _context.TenantID);

            // Probably not needed - anyway causes issue when thousands of tickets loaded at once!
            //if (SPContext.Current == null && updatedTickets.Count > 0)
            //    SPListHelper.ReloadTicketsInCache(updatedTickets, spWeb);

            if (status.recordsAdded > 0 || status.recordsUpdated > 0)
                status.succeeded = true;
            else
                status.succeeded = false;

            return;
        }
        private static Dictionary<string, DataTable> GetLookupLists(ApplicationContext spWeb, string moduleName)
        {
            Dictionary<string, DataTable> data = new Dictionary<string, DataTable>();
            List<string> lookupLists = new List<string>() { DatabaseObjects.Tables.AssetVendors, DatabaseObjects.Tables.AssetModels, DatabaseObjects.Tables.Department,
                                                            DatabaseObjects.Tables.CompanyDivisions, DatabaseObjects.Tables.Location, DatabaseObjects.Tables.RequestType,DatabaseObjects.Tables.TicketPriority,
                                                            DatabaseObjects.Tables.ModuleStages,DatabaseObjects.Tables.TicketImpact,DatabaseObjects.Tables.TicketSeverity,DatabaseObjects.Tables.ACRTypes,
                                                            DatabaseObjects.Tables.FunctionalAreas,DatabaseObjects.Tables.Services,DatabaseObjects.Tables.Applications,DatabaseObjects.Tables.SubLocation,
                                                            DatabaseObjects.Tables.Assets};
            List<string> viewFields = new List<string>();

            foreach (string lookupListName in lookupLists)
            {
                //string query = "";
                // Add all required fields depending on lookup field
                viewFields.Add(string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Id));
                viewFields.Add(string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title));

                if (lookupListName == DatabaseObjects.Tables.AssetVendors)
                    viewFields.Add(string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.VendorName));
                else if (lookupListName == DatabaseObjects.Tables.AssetModels)
                    viewFields.Add(string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.ModelName));
                else if (lookupListName == DatabaseObjects.Tables.RequestType)
                    viewFields.Add(string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.TicketRequestType));

                //query.ViewFields = string.Join("", viewFields);

                //if (lookupListName == DatabaseObjects.Tables.RequestType)
                //    query.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ModuleNameLookup, moduleName);
                //else
                //    query.Query = string.Format("<Where></Where>");

                //DataTable dataTable = SPListHelper.GetDataTable(lookupListName, query, spWeb);
                //data.Add(lookupListName, dataTable);
            }
            return data;
        }
    }
    public class ImportStatus
    {
        public string moduleName;

        public bool succeeded; // true = import succeeded, else failed
        public List<string> errorMessages = new List<string>();

        public int numErrors;
        public int recordsProcessed;
        public int recordsSkipped;
        public int recordsAdded;
        public int recordsUpdated;
        public int totalRecords;
        DataTable table = new DataTable();
        public ImportStatus(string moduleName)
        {
            this.moduleName = moduleName;
        }
    }
}

