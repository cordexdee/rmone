using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using System.Data;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class RelatedCompanyAddEdit : UserControl
    {
        public int Id { private get; set; }
        public string ticketID { get; set; }
        //SPListItem spListItemRCompany;
        private RelatedCompany relatedCompany = null;

        private ApplicationContext _context = null;
        private TicketManager _ticketManager = null;
        private ModuleViewManager _moduleViewManager = null;
        private RelatedCompanyManager _relatedCompanyManager = null;
        private CRMRelationshipTypeManager _relationshipTypeManager = null;
        public UserProfile user;
        //public UserProfileManager UserManager;

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

        public RelatedCompanyAddEdit()
        {
            _context = HttpContext.Current.GetManagerContext();
        }



        //string viewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/><FieldRef Name='{4}'/><FieldRef Name='{5}'/><FieldRef Name='{6}'/><FieldRef Name='{7}'/>",
        //           DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.ContactLookup, DatabaseObjects.Columns.CRMCompanyTitleLookup, DatabaseObjects.Columns.CustomProperties,
        //         DatabaseObjects.Columns.Id, DatabaseObjects.Columns.RelationshipTypeLookup, DatabaseObjects.Columns.CostCodeLookup, DatabaseObjects.Columns.ItemOrder);

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            if (Page.User.Identity.IsAuthenticated)
            {
                user = HttpContext.Current.CurrentUser();
                //UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            }

            if (Id > 0)
            {
                //spListItemRCompany = SPListHelper.GetSPListItem(DatabaseObjects.Lists.RelatedCompanies, Id, SPContext.Current.Web, viewFields);
                relatedCompany = RelatedCompanyManager.LoadByID(Id);

                //if (uHelper.IsSPItemExist(spListItemRCompany, DatabaseObjects.Columns.CRMCompanyTitleLookup))
                if (relatedCompany != null && !string.IsNullOrEmpty(relatedCompany.CRMCompanyLookup))
                {
                    //int cmpId = uHelper.GetLookupID(Convert.ToString(spListItemRCompany[DatabaseObjects.Columns.CRMCompanyTitleLookup]));
                    string cmpId = relatedCompany.CRMCompanyLookup;
                    BindType(cmpId);
                }
            }
            else
            {
                //spListItemRCompany = SPListHelper.GetSPList(DatabaseObjects.Lists.RelatedCompanies).AddItem();
                relatedCompany = new RelatedCompany();
            }

            BindCompanies();


            if (!IsPostBack)
                FillData();

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region Custom Methods

        private void BindCompanies()
        {
            //DataTable dtCompanies = uGITCache.ModuleDataCache.GetOpenTickets(uHelper.getModuleIdByModuleName("COM"));
            UGITModule moduleData = ModuleViewManager.GetByName("COM");
            DataTable dtCompanies = TicketManager.GetOpenTickets(moduleData);

            cmdCompany.DataSource = dtCompanies;
            cmdCompany.TextField = DatabaseObjects.Columns.Title;
            cmdCompany.ValueField = DatabaseObjects.Columns.TicketId;
            cmdCompany.DataBind();
        }

        private DataTable LoadContactData()
        {
            //DataTable dtContacts = uGITCache.ModuleDataCache.GetOpenTickets(uHelper.getModuleIdByModuleName("CON"));
            UGITModule moduleData = ModuleViewManager.GetByName("CON");
            DataTable dtContacts = TicketManager.GetOpenTickets(moduleData);

            if (!string.IsNullOrEmpty(Convert.ToString(cmdCompany.Value)))
            {
                DataRow[] drs = dtContacts.Select($"{DatabaseObjects.Columns.CRMCompanyLookup} = '{cmdCompany.Value}'");
                if (drs != null && drs.Length > 0)
                {
                    DataView dview = drs.CopyToDataTable().AsDataView();
                    dview.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.Title);
                    return dview.ToTable();
                }
            }
            return null;
        }

        private void BindType(string cmpId)
        {
            // vsp-check

            //SPListItem spCompany = SPListHelper.GetSPListItem(DatabaseObjects.Lists.CRMCompany, cmpId);
            string query = $"{DatabaseObjects.Columns.TicketId}='{cmpId}' and {DatabaseObjects.Columns.TenantID}='{_context.TenantID}'";
            DataTable dtCompany = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMCompany, query);

            //if (spCompany != null && uHelper.IsSPItemExist(spCompany, DatabaseObjects.Columns.RelationshipTypeLookup))
            //{
            //    string[] texts = uHelper.GetMultiLookupText(Convert.ToString(spCompany[DatabaseObjects.Columns.RelationshipTypeLookup]));
            //    string[] values = uHelper.GetMultiLookupValue(Convert.ToString(spCompany[DatabaseObjects.Columns.RelationshipTypeLookup]));
            //    for (int i = 0; i < texts.Length; i++)
            //    {
            //        cmbType.Items.Add(new ListEditItem(texts[i], values[i]));
            //    }
            //}
            
            if (dtCompany.Rows.Count > 0)
            {
                var relationshipTypeLookups = Convert.ToString(dtCompany.Rows[0][DatabaseObjects.Columns.RelationshipTypeLookup]);

                if (string.IsNullOrEmpty(relationshipTypeLookups)) return;

                query = $"{DatabaseObjects.Columns.ID} in ({relationshipTypeLookups})";
                var relationshipTypes = RelationshipTypeManager.Load(query);

                cmbType.TextField = DatabaseObjects.Columns.Title;
                cmbType.ValueField = DatabaseObjects.Columns.ID;
                cmbType.DataSource = relationshipTypes;
                cmbType.DataBind();
            }
        }

        //private DataTable GetCostCodes(int cmpId)
        //{
        //    SPListItem spCompany = SPListHelper.GetSPListItem(DatabaseObjects.Lists.CRMCompany, cmpId);
        //    DataTable dtResult = new DataTable();
        //    if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Id))
        //    {
        //        dtResult.Columns.Add(DatabaseObjects.Columns.Id);
        //    }

        //    if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Title))
        //    {
        //        dtResult.Columns.Add(DatabaseObjects.Columns.Title);
        //    }

        //    if (spCompany != null && uHelper.IsSPItemExist(spCompany, DatabaseObjects.Columns.CostCodeLookup))
        //    {
        //        string[] texts = uHelper.GetMultiLookupText(Convert.ToString(spCompany[DatabaseObjects.Columns.CostCodeLookup]));
        //        string[] values = uHelper.GetMultiLookupValue(Convert.ToString(spCompany[DatabaseObjects.Columns.CostCodeLookup]));
        //        for (int i = 0; i < texts.Length; i++)
        //        {
        //            dtResult.Rows.Add(new object[] { values[i], texts[i] });
        //        }
        //    }

        //    return dtResult;
        //}

        private void FillData()
        {
            if (Id > 0)
            {
                //string value = Convert.ToString(uHelper.GetLookupID(Convert.ToString(spListItemRCompany[DatabaseObjects.Columns.CRMCompanyTitleLookup])));

                if (!string.IsNullOrEmpty(relatedCompany.CRMCompanyLookup))
                {
                    string value = relatedCompany.CRMCompanyLookup;
                    cmdCompany.SelectedIndex = cmdCompany.Items.IndexOf(cmdCompany.Items.FindByValue(value));
                }

                GridLookup.DataBind();

                if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(relatedCompany.ContactLookup)))
                {
                    List<string> selectedKeys = uHelper.GetMultiLookupText(Convert.ToString(relatedCompany.ContactLookup), Constants.Separator6);
                    foreach (string val in selectedKeys)
                    {
                        GridLookup.GridView.Selection.SelectRowByKey(val);
                    }
                }

                //string rsType = Convert.ToString(uHelper.GetLookupID(Convert.ToString(spListItemRCompany[DatabaseObjects.Columns.RelationshipTypeLookup])));

                if (relatedCompany.RelationshipTypeLookup.HasValue)
                {
                    string rsType = Convert.ToString(relatedCompany.RelationshipTypeLookup);
                    cmbType.SelectedIndex = cmbType.Items.IndexOf(cmbType.Items.FindByValue(rsType));
                }

                //gridLookupCostCodes.DataBind();
                //selectedKeys = uHelper.GetMultiLookupValue(Convert.ToString(spListItemRCompany[DatabaseObjects.Columns.CostCodeLookup]));
                //foreach (string val in selectedKeys)
                //{
                //    gridLookupCostCodes.GridView.Selection.SelectRowByKey(val);
                //}
                //txtItemOrder.Text = Convert.ToString(spListItemRCompany[DatabaseObjects.Columns.ItemOrder]);
                //txtItemOrder.Text = Convert.ToString(relatedCompany.ItemOrder);
            }
        }

        #endregion

        #region Control Events

        protected void GridLookup_DataBinding(object sender, EventArgs e)
        {
            GridLookup.DataSource = LoadContactData();
        }

        protected void ASPxCallbackPanel1_Callback(object sender, CallbackEventArgsBase e)
        {
            GridLookup.DataBind();
            //gridLookupCostCodes.DataBind();
            string cmpId = Convert.ToString(e.Parameter);
            //gridLookupCostCodes.DataSource = GetCostCodes(cmpId);
            BindType(cmpId);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //long? companyTitleLookup = null;
            string companyLookup = string.Empty;
            long? relationshipTypeLookup = null;
            string contactLookup = null;

            List<string> lookups = new List<string>();
            List<object> multiKeys = GridLookup.GridView.GetSelectedFieldValues(DatabaseObjects.Columns.TicketId);

            foreach (object obj in multiKeys)
            {
                var contactId = Convert.ToString(obj);
                if (string.IsNullOrEmpty(contactId)) continue;

                lookups.Add(contactId);
            }

            //spListItemRCompany[DatabaseObjects.Columns.ContactLookup] = string.Join(";#", lookups.Select(x => string.Format("{0};#{1}", x.ID.ToString(), x.Value)).ToArray());
            //spListItemRCompany[DatabaseObjects.Columns.TicketId] = ticketID;
            //spListItemRCompany[DatabaseObjects.Columns.CRMCompanyTitleLookup] = cmdCompany.Value;
            //spListItemRCompany[DatabaseObjects.Columns.RelationshipTypeLookup] = cmbType.Value;
            //spListItemRCompany[DatabaseObjects.Columns.ItemOrder] = txtItemOrder.Text;

            //if (!int.TryParse(txtItemOrder.Text, out int itemOrder))
            //{       
            //    itemOrder = 1;
            //}            
            
            int itemOrder = 1;
            
            if (cmdCompany.Value != null)
            {
                //companyTitleLookup = Convert.ToInt64(cmdCompany.Value);
                companyLookup = Convert.ToString(cmdCompany.Value);
            }            
            if (cmbType.Value != null)
            {
                relationshipTypeLookup = Convert.ToInt64(cmbType.Value);
            }
            if (lookups.Any())
            {
                contactLookup = string.Join(",", lookups.ToArray());
            }

            relatedCompany.ContactLookup = contactLookup;
            relatedCompany.TicketID  = ticketID;
            //relatedCompany.CRMCompanyTitleLookup = companyTitleLookup;
            relatedCompany.CRMCompanyLookup = companyLookup;
            relatedCompany.RelationshipTypeLookup = relationshipTypeLookup;
            relatedCompany.ItemOrder = itemOrder;

            //lookups.Clear();
            //multiKeys = gridLookupCostCodes.GridView.GetSelectedFieldValues(DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title);
            //foreach (object obj in multiKeys)
            //{
            //    lookups.Add(new LookupValue(Convert.ToInt32(((object[])obj)[0]), Convert.ToString(((object[])obj)[1])));
            //}
            //spListItemRCompany[DatabaseObjects.Columns.CostCodeLookup] = string.Join(";#", lookups.Select(x => string.Format("{0};#{1}", x.ID.ToString(), x.Value)).ToArray());


            if (Id > 0)
            {
                //spListItemRCompany.Update();
                RelatedCompanyManager.Update(relatedCompany);
            }
            else
            {
                RelatedCompanyManager.Insert(relatedCompany);
            }

            if (!string.IsNullOrEmpty(cmdCompany.Text))
            {
                //string module = uHelper.getModuleNameByTicketId(ticketID);

                //UGITModule moduleObj = uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, module);
                //ModuleFormLayout formLayout = moduleObj.List_FormLayout.FirstOrDefault(x => x.FieldDisplayName == "RelatedCompanies");
                //ModuleFormTab formTab = moduleObj.List_FormTab.FirstOrDefault(x => x.TabNumber == formLayout.TabNumber);


                //string historyTxt = string.Format("Company updated on tab {1}:<br/> [{0}]", cmdCompany.Text, formTab.Name);
                //if (Id == 0)
                //    historyTxt = string.Format("Company added on tab {1}:<br/> [{0}]", cmdCompany.Text, formTab.Name);

                //uHelper.CreateHistory(SPContext.Current.Web.CurrentUser, historyTxt, Ticket.getCurrentTicket(module, ticketID), true);
                //uHelper.CreateHistory(user, historyTxt, copyTicket[DatabaseObjects.Columns.TicketId]), newTicket, false);
            }

            uHelper.ClosePopUpAndEndResponse(Context, true);

        }

        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            if (relatedCompany != null)
            {
                RelatedCompanyManager.Delete(relatedCompany);

                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        #endregion


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        //protected void gridLookupCostCodes_DataBinding(object sender, EventArgs e)
        //{
        //    int cmpId = Convert.ToInt16(cmdCompany.Value);
        //    if (cmpId > 0)
        //    {
        //        gridLookupCostCodes.DataSource = GetCostCodes(cmpId);
        //    }
        //}
    }
}
