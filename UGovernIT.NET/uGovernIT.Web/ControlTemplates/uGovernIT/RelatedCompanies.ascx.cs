using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class RelatedCompanies : UserControl
    {
        public string ticketID { get; set; }
        public string FrameId;
        public bool ReadOnly;
        public string ControlId { get; set; }
        protected string absoluteUrlEdit = "/layouts/ugovernit/DelegateControl.aspx?control=relatedcompanyaddedit";
        string addCompany;
        public string ExtProjectId { get; set; }
        //DataTable dtCompanies, dtContact, dtRelatedCompanies;
        //List<ProjectUser> lstProjectUser = new List<ProjectUser>();

        private ApplicationContext _context = null;
        private TicketManager _ticketManager = null;
        private ModuleViewManager _moduleViewManager = null;
        private RelatedCompanyManager _relatedCompanyManager = null;
        private CRMRelationshipTypeManager _relationshipTypeManager = null;

        protected TicketManager TicketManager
        {
            get
            {
                if (_ticketManager == null)
                {
                    _ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
                }
                return _ticketManager;
            }
        }

        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                }
                return _moduleViewManager;
            }
        }

        protected RelatedCompanyManager RelatedCompanyManager
        {
            get
            {
                if (_relatedCompanyManager == null)
                {
                    _relatedCompanyManager = new RelatedCompanyManager(HttpContext.Current.GetManagerContext());
                }
                return _relatedCompanyManager;
            }
        }

        protected CRMRelationshipTypeManager RelationshipTypeManager
        {
            get
            {
                if (_relationshipTypeManager == null)
                {
                    _relationshipTypeManager = new CRMRelationshipTypeManager(HttpContext.Current.GetManagerContext());
                }
                return _relationshipTypeManager;
            }
        }

        public RelatedCompanies()
        {
            _context = HttpContext.Current.GetManagerContext();
        }

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            addCompany = UGITUtility.GetAbsoluteURL(absoluteUrlEdit);
            aAddItem.Attributes.Add("href", string.Format("javascript: window.parent.UgitOpenPopupDialog('{0}','{3}','{2} - New Item','600','400',0,'{1}','true')", addCompany, Server.UrlEncode(Request.Url.AbsolutePath), "Add Related Company", string.Format("&ID=0&ticketID={0}", ticketID)));

            // SPListItem spProjectItem = Ticket.getCurrentTicket(uHelper.getModuleNameByTicketId(ticketID), ticketID);
            //if (spProjectItem != null && !string.IsNullOrEmpty(Convert.ToString(spProjectItem[DatabaseObjects.Columns.ExternalProjectId])))
            //{
            //    ExtProjectId = Convert.ToString(spProjectItem[DatabaseObjects.Columns.ExternalProjectId]);
            //}
            //else
            //{
            //    ExtProjectId = string.Empty;
            //}
            //try
            //{

            //    string ProcoreBaseUrl = ConfigurationVariable.GetValue(ConfigConstants.ProcoreBaseUrl);
            //    string ProcoreCompanyId = ConfigurationVariable.GetValue(ConfigConstants.ProcoreCompanyId);
            //    string ProcoreToken = ConfigurationVariable.GetValue(ConfigConstants.ProcoreToken);


            //    if (!string.IsNullOrEmpty(ProcoreBaseUrl)) //&& !string.IsNullOrEmpty(ProcoreCompanyId)
            //    {
            //        string ProcoreAPIToken = string.Empty;
            //        if (!string.IsNullOrEmpty(ProcoreToken))
            //        {
            //            ProcoreAPIToken = uHelper.ValidateProcoreToken();
            //        }
            //        else
            //        {
            //            Log.WriteLog("Procore Token Required.", "Related Companies - ProcoreAPIToken");
            //            return;
            //        }


            //        dtCompanies = SPListHelper.GetDataTable(DatabaseObjects.Lists.CRMCompany);
            //        dtContact = SPListHelper.GetDataTable(DatabaseObjects.Lists.CRMContact);
            //        dtRelatedCompanies = SPListHelper.GetDataTable(DatabaseObjects.Lists.RelatedCompanies);
            //        var apiUrl = string.Format("{1}vapid/projects/{2}/vendors?access_token={0}", ProcoreAPIToken.Trim(), ProcoreBaseUrl.Trim(), ExtProjectId);
            //        var syncClient = new WebClient();
            //        var content = syncClient.DownloadString(apiUrl);


            //        var apiUsersUrl = string.Format("{1}vapid/projects/{2}/users?access_token={0}", ProcoreAPIToken.Trim(), ProcoreBaseUrl.Trim(), ExtProjectId);
            //        var syncUsers = new WebClient();
            //        var contentUsers = syncUsers.DownloadString(apiUsersUrl);

            //        lstProjectUser = JsonConvert.DeserializeObject<List<ProjectUser>>(contentUsers);
            //        lstCompanies = JsonConvert.DeserializeObject<List<CompanyDirectory>>(content);

            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log.WriteException(ex,"PageLoad() in RelatedCompanies");
            //}

            FillData();

            grdRelatedCompanies.Settings.GroupFormat = "{1} {2}";
            grdRelatedCompanies.Settings.ShowGroupButtons = false;
            grdRelatedCompanies.ExpandAll();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        #endregion

        #region Custom Methods

        private void FillData()
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", _context.TenantID);
            values.Add("@TicketId", ticketID);
            DataTable dtRelatedCompanies = GetTableDataManager.GetData(DatabaseObjects.Tables.RelatedCompanies, values);
             

            //string query = $"{DatabaseObjects.Columns.TicketId} = '{ticketID}' and {DatabaseObjects.Columns.TenantID}='{_context.TenantID}'";
            //DataTable dtRelatedCompanies = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RelatedCompanies, query);

            //if (dtRelatedCompanies != null && dtRelatedCompanies.Rows.Count > 0)
            //{
            //    if (!dtRelatedCompanies.Columns.Contains(DatabaseObjects.Columns.Address))
            //        dtRelatedCompanies.Columns.Add(DatabaseObjects.Columns.Address, typeof(string));

            //    if (!dtRelatedCompanies.Columns.Contains(DatabaseObjects.Columns.CompanyName))
            //        dtRelatedCompanies.Columns.Add(DatabaseObjects.Columns.CompanyName, typeof(string));

            //    if (!dtRelatedCompanies.Columns.Contains(DatabaseObjects.Columns.RelationshipType))
            //        dtRelatedCompanies.Columns.Add(DatabaseObjects.Columns.RelationshipType, typeof(string));

            //    if (!dtRelatedCompanies.Columns.Contains(DatabaseObjects.Columns.Contact))
            //        dtRelatedCompanies.Columns.Add(DatabaseObjects.Columns.Contact, typeof(string));

            //    var moduleData = ModuleViewManager.GetByName("COM");
            //    var dtCompanies = TicketManager.GetOpenTickets(moduleData);

            //    var moduleDataContact = ModuleViewManager.GetByName("CON");
            //    var dtContacts = TicketManager.GetOpenTickets(moduleDataContact);

            //    var relationshipTypes = RelationshipTypeManager.Load();                

            //    foreach (DataRow drRelatedCompany in dtRelatedCompanies.Rows)
            //    {
            //        if (UGITUtility.IsSPItemExist(drRelatedCompany, DatabaseObjects.Columns.CRMCompanyLookup))
            //        {
            //            var companyLookup = Convert.ToString(drRelatedCompany[DatabaseObjects.Columns.CRMCompanyLookup]);

            //            //DataRow drRelatedCompany = dtRelatedCompanies.AsEnumerable().FirstOrDefault(x => x.Field<long>(DatabaseObjects.Columns.CRMCompanyTitleLookup) == companyTitleLookup);
            //            DataRow drCompany = dtCompanies.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.TicketId) == companyLookup);                        

            //            if (drCompany != null)
            //            {
            //                List<string> pAddress = new List<string>();

            //                if (!string.IsNullOrEmpty(Convert.ToString(drCompany[DatabaseObjects.Columns.StreetAddress1])))
            //                    pAddress.Add(Convert.ToString(drCompany[DatabaseObjects.Columns.StreetAddress1]));

            //                if (!string.IsNullOrEmpty(Convert.ToString(drCompany[DatabaseObjects.Columns.City])))
            //                    pAddress.Add(Convert.ToString(drCompany[DatabaseObjects.Columns.City]));

            //                if (!string.IsNullOrEmpty(Convert.ToString(drCompany[DatabaseObjects.Columns.StateLookup])))
            //                {
            //                    //pAddress.Add(uHelper.GetLookupValue(Convert.ToString(drCompany[DatabaseObjects.Columns.UGITStateLookup])));
            //                    var state = Convert.ToString(GetTableDataManager.GetSingleValueByID(DatabaseObjects.Tables.State, DatabaseObjects.Columns.Title, Convert.ToString(drCompany[DatabaseObjects.Columns.StateLookup]), _context.TenantID));
            //                    pAddress.Add(state);
            //                }

            //                if (!string.IsNullOrEmpty(Convert.ToString(drCompany[DatabaseObjects.Columns.Zip])))
            //                    pAddress.Add(Convert.ToString(drCompany[DatabaseObjects.Columns.Zip]));

            //                drRelatedCompany[DatabaseObjects.Columns.Address] = string.Join(Constants.BRTag, pAddress.ToArray());

            //                drRelatedCompany[DatabaseObjects.Columns.CompanyName] = Convert.ToString(drCompany[DatabaseObjects.Columns.Title]);                            
            //            }                        
            //        }

            //        if (UGITUtility.IsSPItemExist(drRelatedCompany, DatabaseObjects.Columns.RelationshipTypeLookup) && relationshipTypes != null)
            //        {
            //            var relationshipType = relationshipTypes.FirstOrDefault(x => x.ID.Equals(drRelatedCompany[DatabaseObjects.Columns.RelationshipTypeLookup]));

            //            if (relationshipType != null)
            //                drRelatedCompany[DatabaseObjects.Columns.RelationshipType] = relationshipType.Title;
            //        }

            //        if (UGITUtility.IsSPItemExist(drRelatedCompany, DatabaseObjects.Columns.ContactLookup) && dtContacts != null)
            //        {
            //            var contactIds = Convert.ToString(drRelatedCompany[DatabaseObjects.Columns.ContactLookup]).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //            var drContacts = dtContacts.AsEnumerable().Where(x => contactIds.Contains(x.Field<string>(DatabaseObjects.Columns.TicketId).ToString())).Select(x => x.Field<string>(DatabaseObjects.Columns.Title)).ToList();

            //            drRelatedCompany[DatabaseObjects.Columns.ContactLookup] = string.Join(",", drContacts.ToArray());
            //        }
            //    }
            //}

            if (dtRelatedCompanies != null && dtRelatedCompanies.Rows.Count>0)
            {
                DataView dataView = dtRelatedCompanies.AsDataView();
                dataView.Sort = DatabaseObjects.Columns.ItemOrder;
                dtRelatedCompanies = dataView.ToTable();

                grdRelatedCompanies.DataSource = dtRelatedCompanies;
                grdRelatedCompanies.DataBind();
            }
            else
            {
                grdRelatedCompanies.DataSource = null;
                grdRelatedCompanies.DataBind();
            }

        }

        #endregion

        #region Control Events

        protected void grdRelatedCompanies_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            string func = string.Empty;
            if (e.RowType == GridViewRowType.Data)
            {
                var ID = 0L;
                DataRow dr = grdRelatedCompanies.GetDataRow(e.VisibleIndex);

                if (dr != null)
                {
                    ID = Convert.ToInt64(dr[DatabaseObjects.Columns.ID]);
                }

                func = string.Format("openDialog('{0}','{1}','{2}')", UGITUtility.GetAbsoluteURL(absoluteUrlEdit), string.Format("&ID={0}&ticketID={1}", ID, ticketID), "Related Companies - Edit Item");

                HtmlAnchor aTitle = grdRelatedCompanies.FindRowCellTemplateControl(e.VisibleIndex, null, "aTitle") as HtmlAnchor;
                if (aTitle != null)
                    aTitle.Attributes.Add("onClick", func);
            }
        }

        protected void aTitle_Load(object sender, EventArgs e)
        {
            HtmlAnchor aHtml = (HtmlAnchor)sender;

            aHtml.InnerText = UGITUtility.StripHTML(Convert.ToString((aHtml.NamingContainer as GridViewDataItemTemplateContainer).Text));
        }

        protected void grdRelatedCompanies_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.ContactLookup || e.DataColumn.FieldName == DatabaseObjects.Columns.CostCodeLookup)
            {
                List<string> lstValues = uHelper.GetMultiLookupText(Convert.ToString(e.CellValue), ",").ToList();
                if (lstValues != null && lstValues.Count > 0)
                {
                    e.Cell.Text = string.Join(Constants.BRTag, lstValues);
                }
            }
            else if (e.DataColumn.FieldName == DatabaseObjects.Columns.Address)
            {
                // resigning value get html formatting
                e.Cell.Text = Convert.ToString(e.CellValue);
            }
        }

        //protected void lnkSyncProjectDirectory_Click(object sender, EventArgs e)
        //{

        //    if (lstCompanies != null && dtCompanies != null)
        //    {
        //      List<CompanyDirectory> lstMissingCompanies  = lstCompanies.Where(x => dtCompanies.AsEnumerable().Count(a => a.Field<string>(DatabaseObjects.Columns.Title).Equals(x.name, StringComparison.CurrentCultureIgnoreCase)) == 0).ToList();
        //      if (lstMissingCompanies != null && lstMissingCompanies.Count > 0)
        //      {
        //          ProcoreAPIHelper.CreateNewCompaniesFromProcore(lstMissingCompanies, ConfigurationVariable.GetValue(ConfigConstants.DefaultSetIdForCompaniesImport, string.Empty));
        //          dtCompanies = SPListHelper.GetDataTable(DatabaseObjects.Lists.CRMCompany);
        //      }
        //    }

        //    if (lstProjectUser != null && dtContact != null)
        //    {
        //        List<ProjectUser> lstMissingContacts = lstProjectUser.Where(x => dtContact.AsEnumerable().Count(a => (!a.IsNull(DatabaseObjects.Columns.EmailAddress) && a.Field<string>(DatabaseObjects.Columns.EmailAddress).Equals(x.email_address, StringComparison.CurrentCultureIgnoreCase)) || ProcoreAPIHelper.CheckForDefaultCompanyVendorContacts(x)) == 0).ToList();
        //        if (lstMissingContacts != null && lstMissingContacts.Count > 0)
        //        {
        //            ProcoreAPIHelper.SyncContactsWithProcore(Constants.FromProcore, ConfigurationVariable.GetValue(ConfigConstants.DefaultSetIdForCOMContactsImport, string.Empty), null, 0, lstMissingContacts);
        //            dtContact = SPListHelper.GetDataTable(DatabaseObjects.Lists.CRMContact);
        //        }
        //    }

        //    #region Company/Contact Sync

        //        try
        //        {                   
        //            if (!string.IsNullOrEmpty(ExtProjectId))
        //            {

        //                string errorMessage = string.Empty;

        //                if (lstCompanies != null)
        //                {
        //                    SPList spRelatedCompany = SPListHelper.GetSPList(DatabaseObjects.Lists.RelatedCompanies);

        //                    foreach (CompanyDirectory itemCmpDirectory in lstCompanies)
        //                    {
        //                        if (!string.IsNullOrEmpty(itemCmpDirectory.name))
        //                        {
        //                            DataRow[] drs =  dtCompanies.AsEnumerable().Where( a=> a.Field<string>(DatabaseObjects.Columns.Title).Equals(itemCmpDirectory.name,StringComparison.CurrentCultureIgnoreCase)).ToArray();
        //                            if (drs != null && drs.Length > 0)
        //                            {
        //                                SPListItem lstItem = null;
        //                                DataRow[] drsRCompany = dtRelatedCompanies != null ? dtRelatedCompanies.AsEnumerable().Where( a=> a.Field<string>(DatabaseObjects.Columns.CRMCompanyTitleLookup).Equals(itemCmpDirectory.name,StringComparison.CurrentCultureIgnoreCase) && a.Field<string>(DatabaseObjects.Columns.TicketId).Equals(ticketID,StringComparison.CurrentCultureIgnoreCase)).ToArray() : null;
        //                                if (drsRCompany != null && drsRCompany.Length > 0)
        //                                {
        //                                    lstItem = spRelatedCompany.GetItems(new SPQuery() { Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' LookupId='FALSE'/><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></And></Where>", DatabaseObjects.Columns.CRMCompanyTitleLookup, itemCmpDirectory.name.Replace("'", "''"), DatabaseObjects.Columns.TicketId, ticketID) })[0];
        //                                }
        //                                else
        //                                {
        //                                    lstItem = spRelatedCompany.AddItem();
        //                                }

        //                                List<LookupValue> lookups = new List<LookupValue>();
        //                                List<ProjectUser> lstProjectUserForCompany = lstProjectUser.Where(x => x.vendor != null && x.vendor.name == itemCmpDirectory.name).ToList();

        //                                foreach (ProjectUser pUser in lstProjectUserForCompany)
        //                                {
        //                                    if (!ProcoreAPIHelper.CheckForDefaultCompanyVendorContacts(pUser))
        //                                    {
        //                                        DataRow[] drContacts = dtContact.AsEnumerable().Where(a => a.Field<string>(DatabaseObjects.Columns.Title).Equals(pUser.name, StringComparison.CurrentCultureIgnoreCase)).ToArray();
        //                                        if (drContacts != null && drContacts.Length > 0)
        //                                            lookups.Add(new LookupValue(Convert.ToInt32(drContacts[0][DatabaseObjects.Columns.Id]), Convert.ToString(drContacts[0][DatabaseObjects.Columns.Title])));
        //                                    }
        //                                }

        //                                lstItem[DatabaseObjects.Columns.ContactLookup] = string.Join(";#", lookups.Select(x => string.Format("{0};#{1}", x.ID.ToString(), x.Value)).ToArray());

        //                                lstItem[DatabaseObjects.Columns.TicketId] = ticketID;
        //                                lstItem[DatabaseObjects.Columns.CRMCompanyTitleLookup] = drs[0][DatabaseObjects.Columns.Id];
        //                                //lstItem[DatabaseObjects.Columns.CustomProperties] = itemCmpDirectory.notes;
        //                                lstItem.Update();

        //                            }
        //                        }
        //                    }
        //                    uGITCache.ModuleDataCache.RefreshTicketsCache();
        //                }
        //            }

        //            FillData();
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.WriteException(ex, "ProcoreUtility- Project Directory Sync");
        //        }

        //        #endregion

        //}
        #endregion

        protected void cbPanel_Callback(object sender, CallbackEventArgsBase e)
        {
        }

        protected void cbType_Callback(object sender, CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
            {
                ASPxComboBox cbType = sender as ASPxComboBox;
                GridViewDataItemTemplateContainer container = cbType.NamingContainer as GridViewDataItemTemplateContainer;
                DataRow dr = grdRelatedCompanies.GetDataRow(container.VisibleIndex);
                int id = 0;

                if (int.TryParse(Convert.ToString(dr[DatabaseObjects.Columns.Id]), out id))
                {
                    // change code vsp
                    //SPListItem spItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.RelatedCompanies, id);
                    //if (spItem != null)
                    //{
                    //    spItem[DatabaseObjects.Columns.CustomProperties] = e.Parameter;
                    //    spItem.Update();
                    //}

                    var relatedCompany = RelatedCompanyManager.LoadByID(id);
                    if (relatedCompany != null)
                    {
                        relatedCompany.CustomProperties = e.Parameter;
                        RelatedCompanyManager.Update(relatedCompany);
                    }
                }
            }
        }
    }
}
