using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class ActivitiesAddEdit : System.Web.UI.UserControl
    {

        private ApplicationContext _context = null;
        //FieldConfigurationManager fmanger = null;
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

        CRMActivitiesManager CRMActivitiesManager = null;
        CRMActivities CRMActivities = new CRMActivities();

        ModuleViewManager moduleViewManager = null;
        public int Id { private get; set; }
        public int contactID { get; set; }
        DataTable spListItemActivity;
        public string ticketID { get; set; }
        string module = string.Empty;
        string viewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/><FieldRef Name='{4}'/><FieldRef Name='{5}'/><FieldRef Name='{6}'/><FieldRef Name='{7}'/>",
                   DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.ContactLookup, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.UGITAssignedTo,
                 DatabaseObjects.Columns.UGITDueDate, DatabaseObjects.Columns.Id, DatabaseObjects.Columns.ActivityStatus, DatabaseObjects.Columns.UGITDescription);

        protected override void OnInit(EventArgs e)
        {
            CRMActivitiesManager = new CRMActivitiesManager(ApplicationContext);
            moduleViewManager = new ModuleViewManager(ApplicationContext);

            if (Id > 0)
            {
                viewFields = string.Format("{0} = {1} ", "Id", Id);
                // spListItemActivity = SPListHelper.GetSPListItem(DatabaseObjects.Lists.CRMActivities, Id, SPContext.Current.Web, viewFields);
                spListItemActivity = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMActivities, $"{viewFields.ToString()} and TenantID='{ApplicationContext.TenantID}'"); //spList.GetItems(spQuery);
            }
            else
            {
                viewFields = string.Format("{0} = {1} ", "Id", 0);
                spListItemActivity = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMActivities, $"{viewFields.ToString()} and TenantID='{ApplicationContext.TenantID}'"); //spList.GetItems(spQuery);
                ////spListItemActivity = SPListHelper.GetSPList(DatabaseObjects.Lists.CRMActivities).AddItem();
                spListItemActivity = spListItemActivity.Clone();
                DataRow dr = spListItemActivity.NewRow();
                spListItemActivity.Rows.InsertAt(dr, 0);

            }
            module = uHelper.getModuleNameByTicketId(ticketID);

            if (module == "CON")
            {
                trContact.Visible = false;
                trActivity.Attributes.Add("class", "col-md-6 col-sm-6 col-xs-6 noPadding");
                trStatus.Attributes.Add("class", "col-md-6 col-sm-6 col-xs-6 noPadding");
                trAssignee.Attributes.Add("class", "col-md-6 col-sm-6 col-xs-6 noPadding");
                trWhendue.Attributes.Add("class", "col-md-6 col-sm-6 col-xs-6 noPadding fieldWrap");
            }
            else
            {
                trContact.Visible = true;
                BindContactDropdown();
                trActivity.Attributes.Add("class", "col-md-6 col-sm-6 col-xs-6 noPadding");
                trStatus.Attributes.Add("class", "col-md-6 col-sm-6 col-xs-6 noPadding");
                trAssignee.Attributes.Add("class", "col-md-6 col-sm-6 col-xs-6 noPadding");
                trWhendue.Attributes.Add("class", "col-md-6 col-sm-6 col-xs-6 noPadding fieldWrap");
            }

            

            //SPListItemCollection spCRMEstimateCollection = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.CRMEstimate, new SPQuery() { ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/>", DatabaseObjects.Columns.ActivityStatus, DatabaseObjects.Columns.Id) });
           // string spQuery = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/>", DatabaseObjects.Columns.ActivityStatus, DatabaseObjects.Columns.Id);
            string spQuery = string.Format("{0} = {1}", DatabaseObjects.Columns.ActivityStatus, DatabaseObjects.Columns.Id);

            //DataTable spCRMEstimateCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMEstimate, $"{spQuery.ToString()} and TenantID='{ApplicationContext.TenantID}'"); //spList.GetItems(spQuery);
            DataTable spCRMEstimateCollection =  new DataTable();

            BindDDL(spCRMEstimateCollection, ddlStatus, DatabaseObjects.Columns.ActivityStatus, true);
            FillData();

            if (string.IsNullOrEmpty(peAssignee.GetValues()))
            {
                peAssignee.SetValues(ApplicationContext.CurrentUser.Id);
            }

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //peAssignee.UserTokenBoxAdd.GridViewStyles.FilterRow.CssClass = "userValueBox-filterRow";
            //peAssignee.UserTokenBoxAdd.GridViewStyles.StatusBar.CssClass = "userValueBox-footerCloseBtn";
        }

        private void BindContactDropdown()
        {
            if (!string.IsNullOrEmpty(ticketID))
            {
                //DataTable dtResults = uGITCache.ModuleDataCache.GetOpenTickets(uHelper.getModuleIdByModuleName("CON"));
                 UGITModule CONModule = moduleViewManager.LoadByName("CON");
                TicketManager ticketManager = new TicketManager(ApplicationContext);
                DataTable dtResults = ticketManager.GetOpenTickets(CONModule); 
                // SPListItem spListItem = Ticket.GetCurrentTicket(uHelper.getModuleNameByTicketId(ticketID), ticketID);
                 DataRow spListItem = Ticket.GetCurrentTicket(_context, uHelper.getModuleNameByTicketId(ticketID), ticketID);
                if (spListItem != null)
                {
                    DataTable dtContacts = null;
                    string company = "";
                     //company = uHelper.GetLookupValue(Convert.ToString(spListItem[DatabaseObjects.Columns.CRMCompanyTitleLookup]));

                    //if (fmanger.GetFieldByFieldName(DatabaseObjects.Columns.CRMCompanyTitleLookup) != null)
                    //{
                        //company = fmanger.GetFieldConfigurationData(DatabaseObjects.Columns.CRMCompanyTitleLookup, Convert.ToString(spListItem[DatabaseObjects.Columns.CRMCompanyTitleLookup]));
                        
                    //}

                    //if (!string.IsNullOrEmpty(company))
                    //{
                        company = company.Replace("'", "''");
                    //    DataRow[] drs = dtResults.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CRMCompanyTitleLookup, company.Trim())).ToArray();

                    if (spListItem[DatabaseObjects.Columns.CRMCompanyLookup] != DBNull.Value)
                    {
                        DataRow[] drs = dtResults.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CRMCompanyLookup, spListItem[DatabaseObjects.Columns.CRMCompanyLookup])).ToArray();

                        if (drs != null && drs.Length > 0)
                        {
                            dtContacts = drs.CopyToDataTable();
                        }
                        //}

                        ddlContact.DataSource = dtContacts;
                        ddlContact.ValueField = DatabaseObjects.Columns.TicketId;
                        ddlContact.TextField = DatabaseObjects.Columns.Title;
                        ddlContact.DataBind();
                    }

                    if (spListItemActivity != null && spListItemActivity.Rows[0][DatabaseObjects.Columns.ContactLookup] != null)
                    {
                        ddlContact.SelectedIndex = ddlContact.Items.IndexOf(ddlContact.Items.FindByValue(UGITUtility.GetLookupID(Convert.ToString(spListItemActivity.Rows[0][DatabaseObjects.Columns.ContactLookup])).ToString()));
                    }

                }

            }

        }

        private void BindDDL(DataTable spListItemCollection, ASPxComboBox drpList, string fieldName, bool fromChoice)
        {
            try
            {

                //SPList listName = SPListHelper.GetSPList(DatabaseObjects.Tables.CRMActivities);
                //SPField field = (SPField)listName.Fields.GetFieldByInternalName(fieldName);
                List<string> listItem = new List<string>();
                //SPListItemCollection items = spList.Items;

                //if (field.Type == SPFieldType.Choice)
                //{
                //foreach (string str in ((SPFieldChoice)field).Choices)
                //{
                //    listItem.Add(str);
                //}

                //if (!fromChoice)
                //{
                //    foreach (SPListItem item in spListItemCollection)
                //    {
                //        listItem.Add(item[fieldName].ToString());
                //    }
                //}
                //}

                listItem.Add("Completed");
                listItem.Add("In Progress");
                listItem.Add("Pending Start");
                listItem.Sort();
                drpList.DataSource = listItem.Distinct();
                drpList.DataBind();
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
        }

        private void FillData()
        {

            if (Id > 0)
            {
                txtTitle.Text = Convert.ToString(spListItemActivity.Rows[0][DatabaseObjects.Columns.Title]);
                txtDesc.Text = Convert.ToString(spListItemActivity.Rows[0][DatabaseObjects.Columns.UGITDescription]);
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(Convert.ToString(spListItemActivity.Rows[0][DatabaseObjects.Columns.ActivityStatus])));

                DateTime date;
                if (!DateTime.TryParse(Convert.ToString(spListItemActivity.Rows[0][DatabaseObjects.Columns.UGITDueDate]), out date))
                {
                    date = DateTime.Today;
                }
                dtcRecurrEndDate.Date = date;

               // if (spListItemActivity.Rows[0][DatabaseObjects.Columns.UGITAssignedTo] != null && spListItemActivity.Rows[0][DatabaseObjects.Columns.UGITAssignedTo].ToString().Contains(Constants.Separator))
                    if (spListItemActivity.Rows[0][DatabaseObjects.Columns.UGITAssignedTo] != null)
                        // peAssignee = uHelper.GetCommaSeparatedLookupValue(spListItemActivity[DatabaseObjects.Columns.UGITAssignedTo].ToString());
                        peAssignee.SetValues(spListItemActivity.Rows[0][DatabaseObjects.Columns.UGITAssignedTo].ToString());
                else
                    peAssignee.SetValues(" ");
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if(Id > 0)
            {
                CRMActivities = CRMActivitiesManager.LoadByID(Id);
            }
            spListItemActivity.Rows[0][DatabaseObjects.Columns.Title] = txtTitle.Text.Trim();
            CRMActivities.ID = Id;
            CRMActivities.Title = txtTitle.Text.Trim();
            //spListItemActivity.Rows[0][DatabaseObjects.Columns.ActivityStatus] = ddlStatus.Text;
            
            CRMActivities.ActivityStatus = ddlStatus.Text;

            if (module == "CON")
            {
                if (contactID > 0)
                {                    
                    CRMActivities.ContactLookup = ticketID;
                    //CRMActivities.TicketId = ticketID;
                }
            }
            else
            {
                ////if (UGITUtility.StringToInt(ddlContact.Value) > 0)
                {
                    // spListItemActivity.Rows[0][DatabaseObjects.Columns.ContactLookup] = ddlContact.Value; // ContactId;
                    //dr[DatabaseObjects.Columns.ContactLookup] = ddlContact.Value;
                    CRMActivities.ContactLookup = Convert.ToString(ddlContact.Value); 
                }
                CRMActivities.TicketId = ticketID;
            }
           // spListItemActivity.Rows[0][DatabaseObjects.Columns.UGITDescription] = txtDesc.Text;
            // dr[DatabaseObjects.Columns.UGITDescription] = txtDesc.Text;
            CRMActivities.Description = txtDesc.Text;

            spListItemActivity.Rows[0][DatabaseObjects.Columns.UGITDueDate] = dtcRecurrEndDate.Value==null ? DateTime.Now.ToString("MM/dd/yyyy").ToDateTime() : dtcRecurrEndDate.Value;
            CRMActivities.DueDate = (DateTime)(dtcRecurrEndDate.Value == null ? DateTime.Now.ToString("MM/dd/yyyy").ToDateTime() : dtcRecurrEndDate.Value);

            //SPFieldUserValueCollection usrVals = UGITUtility.GetFieldUserValueCollection(peAssignee.ResolvedEntities, SPContext.Current.Web);
            string usrVals = peAssignee.GetValues();
            spListItemActivity.Rows[0][DatabaseObjects.Columns.UGITAssignedTo] = usrVals;
            CRMActivities.AssignedTo = usrVals;

            if (CRMActivities != null && CRMActivities.ID > 0)
            {
                if (ddlStatus.SelectedItem != null && CRMActivities.EndDate == null && ddlStatus.SelectedItem.Text.EqualsIgnoreCase("Completed"))
                {
                    CRMActivities.EndDate = DateTime.Now.ToString("MM/dd/yyyy").ToDateTime();
                    if (CRMActivities.StartDate == null)
                        CRMActivities.StartDate = DateTime.Now.ToString("MM/dd/yyyy").ToDateTime();
                }

                if (ddlStatus.SelectedItem != null && CRMActivities.StartDate == null && ddlStatus.SelectedItem.Text.EqualsIgnoreCase("In Progress"))
                {
                    CRMActivities.StartDate = DateTime.Now.ToString("MM/dd/yyyy").ToDateTime();
                    CRMActivities.EndDate = null;
                }

                if (ddlStatus.SelectedItem != null && !ddlStatus.SelectedItem.Text.EqualsIgnoreCase("In Progress") && !ddlStatus.SelectedItem.Text.EqualsIgnoreCase("Completed"))
                {
                    CRMActivities.StartDate = null;
                    CRMActivities.EndDate = null;
                }

                if (ddlStatus.SelectedItem == null)
                {
                    CRMActivities.StartDate = null;
                    CRMActivities.EndDate = null;
                }
            }
            else
            {
                if (ddlStatus.SelectedItem == null)
                {
                    CRMActivities.StartDate = null;
                    CRMActivities.EndDate = null;
                }
                else if (ddlStatus.SelectedItem.Text.EqualsIgnoreCase("In Progress"))
                {
                    CRMActivities.StartDate = DateTime.Now.ToString("MM/dd/yyyy").ToDateTime();
                }
                else if (ddlStatus.SelectedItem.Text.EqualsIgnoreCase("Completed"))
                {
                    CRMActivities.EndDate = DateTime.Now.ToString("MM/dd/yyyy").ToDateTime();
                    CRMActivities.StartDate = DateTime.Now.ToString("MM/dd/yyyy").ToDateTime();
                }
            }

            CRMActivitiesManager.Update(CRMActivities);
            // spListItemActivity.Update();
            //Ticket tc = new Ticket(ApplicationContext, uHelper.getModuleIdByTicketID(ApplicationContext, DatabaseObjects.Columns.Id));
            //tc.CommitChanges(spListItemActivity.Rows[0]);

           //  _context.Cache.Add(string.Format("EditActivityInfo-{0}", SPContext.Current.Web.CurrentUser.ID), string.Format("contactId={0}", ContactId), null, DateTime.Now.AddSeconds(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);

            Context.Cache.Add(string.Format("EditActivityInfo-{0}", ApplicationContext.TenantID), string.Format("contactId={0}", contactID), null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
            uHelper.ClosePopUpAndEndResponse(Context, true);

        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }




        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            if (Id > 0)
            {
                CRMActivities = CRMActivitiesManager.LoadByID(Id);
            }
            if (CRMActivities != null)
            {
                CRMActivities.Deleted = true;
                //spListItemActivity.Delete();
                CRMActivitiesManager.Update(CRMActivities);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }
    }
}