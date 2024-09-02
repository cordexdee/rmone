using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.DAL;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class ContactActivity : System.Web.UI.UserControl
    {
        public string ModuleName { get; set; }
        ModuleViewManager moduleViewManager = null;
        UGITModule moduleDetail = new UGITModule();
        DataRow spContact;
        public string ticketID { get; set; }
        public string FrameId;
        public bool ReadOnly;
        public string ControlId { get; set; }
        public bool IsPreview { get; set; }
        // private string absoluteUrlEdit = "/_layouts/15/ugovernit/DelegateControl.aspx?control={0}&ID={1}&ticketID={2}";
        protected string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control=activitiesaddedit";
        string addNewActivity;
        //FieldConfigurationManager fmanger = null;
        UserProfileManager userprofile = new UserProfileManager(HttpContext.Current.GetManagerContext());

        public bool ByContact;
        private ApplicationContext _context = null;

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

        protected override void OnInit(EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            IsPreview = UGITUtility.StringToBoolean(Request["IsPreview"]);
            if (IsPreview)
            {
                aAddItem.Visible = false;
                if (grdActivities.Columns.Count > 0)
                {
                    grdActivities.Columns[0].Visible = false;
                }
                grdActivities.Settings.ShowHeaderFilterButton = false;
                tbTabs.Visible = true;

                string myActivityViewType = UGITUtility.GetCookieValue(Request, "myActivityViewType");
                if (Request["myActivityViewType"] != null)
                {
                    myActivityViewType = Request["myActivityViewType"];
                }

                if (myActivityViewType.ToLower() == "byduedate")
                {
                    ByContact = false;
                    foreach (GridViewDataColumn item in grdActivities.GetGroupedColumns())
                        item.UnGroup();
                }
                else
                {
                    ByContact = true;
                    grdActivities.GroupBy(grdActivities.Columns[DatabaseObjects.Columns.ContactLookup]);
                }
            }
            else
            {
                tbTabs.Visible = false;
                List<string> viewFields = new List<string>() { DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title };
                spContact = Ticket.GetCurrentTicket(ApplicationContext, uHelper.getModuleNameByTicketId(ticketID), ticketID);
                aAddItem.Visible = true;
                grdActivities.Settings.ShowHeaderFilterButton = true;
            }

            FillData();

            grdActivities.Settings.GroupFormat = "{1} {2}";
            grdActivities.Settings.ShowGroupButtons = false;
            grdActivities.ExpandAll();
        }

        protected override void OnPreRender(EventArgs e)
        {
            string myActivityViewType = UGITUtility.GetCookieValue(Request, "myActivityViewType");
            if (Request["myActivityViewType"] != null)
            {
                myActivityViewType = Request["myActivityViewType"];
            }

            if (myActivityViewType.ToLower() == "byduedate")
            {
                myActivityByDueDate.Attributes.Add("class", "ucontentdiv ugitsellinkbg clickedTab");
                myActivityByContact.Attributes.Add("class", "ucontentdiv ugitlinkbg");
            }
            else
            {
                myActivityByContact.Attributes.Add("class", "ucontentdiv ugitsellinkbg clickedTab");
                myActivityByDueDate.Attributes.Add("class", "ucontentdiv ugitlinkbg");
            }
            base.OnPreRender(e);
        }
        protected void grdActivities_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {

            string func = string.Empty;
            string contactId = "0";

            if (e.RowType == GridViewRowType.Data)
            {
                DataRow row = grdActivities.GetDataRow(e.VisibleIndex);

                row[DatabaseObjects.Columns.AssignedTo] = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(row[DatabaseObjects.Columns.AssignedTo]));

                //if (spContact != null)
                //{
                //    ////contactId = Convert.ToString(spContact[DatabaseObjects.Columns.Id]);
                //}

                string contactID = Convert.ToString(row["ContactId"]);
                if (!string.IsNullOrEmpty(contactID))
                {
                    moduleViewManager = new ModuleViewManager(ApplicationContext);
                    moduleDetail = moduleViewManager.GetByName("CON");
                    //DataRow dr = uGITCache.GetModuleDetails(uHelper.getModuleIdByTicketID(ApplicationContext, contactID));
                    string spQuery = string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, row[DatabaseObjects.Columns.ContactLookup]);
                    DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMContact, $"{spQuery.ToString()} and TenantID='{ApplicationContext.TenantID}'");

                    if (dt.Rows.Count > 0)
                    {
                        //func = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", UGITUtility.GetAbsoluteURL(Convert.ToString(dr[DatabaseObjects.Columns.ModuleRelativePagePath])), string.Format("TicketId={0}", contactID), Convert.ToString(row[DatabaseObjects.Columns.ContactLookup]), Server.UrlEncode(Request.Url.AbsolutePath), "90", "90");
                        func = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", UGITUtility.GetAbsoluteURL(moduleDetail.StaticModulePagePath), string.Format("TicketId={0}", contactID), Convert.ToString(dt.Rows[0]["TicketId"] + ": " + dt.Rows[0]["FirstName"].ToString() + " " + dt.Rows[0]["LastName"].ToString()), Server.UrlEncode(Request.Url.AbsolutePath), "90", "90");
                        HtmlAnchor aContact = grdActivities.FindRowCellTemplateControl(e.VisibleIndex, null, "aContact") as HtmlAnchor;
                        if (aContact != null)
                        {
                            aContact.Attributes.Add("onClick", func);
                            aContact.InnerText = Convert.ToString(row[DatabaseObjects.Columns.ContactLookup]);
                        } 
                    }
                }

                string leadID = Convert.ToString(row[DatabaseObjects.Columns.TicketId]);
                if (!string.IsNullOrEmpty(leadID))
                {
                    //DataRow dr = uGITCache.GetModuleDetails(uHelper.getModuleIdByTicketID(ApplicationContext,leadID));
                    //func = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", UGITUtility.GetAbsoluteURL(Convert.ToString(dr[DatabaseObjects.Columns.ModuleRelativePagePath])), string.Format("TicketId={0}", leadID), leadID, Server.UrlEncode(Request.Url.AbsolutePath), "90", "90");
                    moduleViewManager = new ModuleViewManager(ApplicationContext);
                    moduleDetail = moduleViewManager.GetByName("LEM");
                    func = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", UGITUtility.GetAbsoluteURL(moduleDetail.StaticModulePagePath), string.Format("TicketId={0}", leadID), leadID, Server.UrlEncode(Request.Url.AbsolutePath), "90", "90");

                    HtmlAnchor aLead = grdActivities.FindRowCellTemplateControl(e.VisibleIndex, null, "aLead") as HtmlAnchor;
                    if (aLead != null)
                    {
                        aLead.Attributes.Add("onClick", func);
                        aLead.InnerText = leadID;
                    }
                }

                func = string.Format("openActivityDialog('{0}','{1}','{2}')", UGITUtility.GetAbsoluteURL(absoluteUrlEdit), string.Format("&ID={0}&contactID={1}&ticketID={2}", e.KeyValue, contactId, ticketID), "Activities - Edit Item");
                HtmlAnchor aTitle = grdActivities.FindRowCellTemplateControl(e.VisibleIndex, null, "aTitle") as HtmlAnchor;

                if (aTitle != null)
                    aTitle.Attributes.Add("onClick", func);

                e.Row.Attributes.Add("onmouseover", string.Format("showContactActivityActions(this,{0})", e.KeyValue));
                e.Row.Attributes.Add("onmouseout", string.Format("hideContactActivityActions(this,{0})", e.KeyValue));
            }

            if (e.RowType == GridViewRowType.Group)
            {
                DataRow row = grdActivities.GetDataRow(e.VisibleIndex);
                HtmlAnchor aGroupContact = grdActivities.FindGroupRowTemplateControl(e.VisibleIndex, "aGroupContact") as HtmlAnchor;

                string title = string.Empty;
                string contactID = Convert.ToString(row["ContactId"]);
                if (!string.IsNullOrEmpty(contactID))
                {
                    if (row != null)
                    {
                        aGroupContact.InnerText = Convert.ToString(row[DatabaseObjects.Columns.ContactLookup]);
                        title = string.Format("{0}:{1}", contactID, Convert.ToString(row[DatabaseObjects.Columns.ContactLookup]));
                    }

                    //DataRow moduleDetail = uGITCache.GetModuleDetails("CON");
                    //string editContactUrl = string.Format("{0}?TicketId={1}&isudlg=1", UGITUtility.GetAbsoluteURL(moduleDetail[DatabaseObjects.Columns.ModuleRelativePagePath].ToString()), contactID);

                    moduleViewManager = new ModuleViewManager(ApplicationContext);
                    moduleDetail = moduleViewManager.GetByName(ModuleName);
                    string editContactUrl = string.Format("{0}?TicketId={1}&isudlg=1", UGITUtility.GetAbsoluteURL(moduleDetail.ModuleRelativePagePath.ToString()), contactID);
                    func = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','1100px','550px',0,'{1}')", editContactUrl, Server.UrlEncode(Request.Url.AbsolutePath), title);
                }
                aGroupContact.Attributes.Add("onClick", func);
            }
        }


        protected void grdActivities_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.AssignedTo)
            {
                string values = Convert.ToString(e.GetValue(e.DataColumn.FieldName));
                if (!string.IsNullOrEmpty(values))
                {
                    var user = userprofile.GetUserById(values);
                    Label lblAssign = (Label)e.Cell.FindControlRecursive("lblAssignTo");
                    lblAssign.Text = user.Name;
                }

                //fmanger = new FieldConfigurationManager(context);
                //if (fmanger.GetFieldByFieldName(e.DataColumn.FieldName) != null)
                //{
                //string value = fmanger.GetFieldConfigurationData(e.DataColumn.FieldName, values);
                //e.Cell.Text = value;
                //}
            }

            ////DataTable resultCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMContact, $"{spQuery.ToString()} and TenantID='{ApplicationContext.TenantID}'"); 
            //DataTable resultCollection = null;
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.ContactLookup)
            {
                string values = Convert.ToString(e.GetValue(e.DataColumn.FieldName));
                string spQuery = string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, values);

                if (values != null && values != "0")
                {
                    DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMContact, $"{spQuery.ToString()} and TenantID='{ApplicationContext.TenantID}'");
                    HtmlAnchor lblAssign = (HtmlAnchor)e.Cell.FindControlRecursive("aContact");
                    lblAssign.InnerText = dt.Rows[0][DatabaseObjects.Columns.TicketId].ToString();
                }
            }
        }

        private void FillData()
        {
            //    string viewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/><FieldRef Name='{4}'/><FieldRef Name='{5}'/><FieldRef Name='{6}'/> <FieldRef Name='{7}'/><FieldRef Name='{8}'/>",
            //DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.UGITDescription, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.UGITAssignedTo,
            //DatabaseObjects.Columns.UGITDueDate, DatabaseObjects.Columns.Id, DatabaseObjects.Columns.ActivityStatus, DatabaseObjects.Columns.Type, DatabaseObjects.Columns.ContactLookup);
            // SPQuery spQuery = new SPQuery();

            String spQuery = "";
            if (IsPreview)
            {
                //spQuery = string.Format("<Where><And><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq> <Neq><FieldRef Name='{2}' /><Value Type='Choice'>{3}</Value></Neq> </And></Where>",
                //                                                       DatabaseObjects.Columns.UGITAssignedTo, HttpContext.Current.CurrentUser().Id, DatabaseObjects.Columns.ActivityStatus, "Completed");

                spQuery = string.Format("{0} = '{1}' and {2} = '{3}' and Deleted = 0", DatabaseObjects.Columns.UGITAssignedTo, HttpContext.Current.CurrentUser().Id, DatabaseObjects.Columns.ActivityStatus, "Completed");

            }
            else
            {
                if (spContact != null)
                {
                    //addNewActivity = uHelper.GetAbsoluteURL(string.Format(absoluteUrlEdit, "activitiesaddedit", "0", Convert.ToString(spContact[DatabaseObjects.Columns.Id])));
                    addNewActivity = UGITUtility.GetAbsoluteURL(absoluteUrlEdit);
                    aAddItem.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','{3}','{2} - New Item','600','500',0,'{1}','true')", addNewActivity, Server.UrlEncode(Request.Url.AbsolutePath), "Activities", string.Format("&ID=0&contactID={0}&ticketID={1}", Convert.ToString(spContact[DatabaseObjects.Columns.Id]), ticketID)));

                    if (uHelper.getModuleNameByTicketId(ticketID) == "CON")  // For CON module record are fetched by Contactlookup field
                    {
                        //spQuery = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>",
                        //                                   DatabaseObjects.Columns.ContactLookup, Convert.ToInt32((spContact.[DatabaseObjects.Columns.Id])));
                        //grdActivities.Columns[DatabaseObjects.Columns.ContactLookup].Visible = false;
                        // spQuery = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>",
                        // DatabaseObjects.Columns.ContactLookup, Convert.ToInt32((DatabaseObjects.Columns.Id)));
                        spQuery = string.Format("{0} = '{1}' and Deleted = 0 ", DatabaseObjects.Columns.ContactLookup, Convert.ToString((spContact[DatabaseObjects.Columns.TicketId])));

                        grdActivities.Columns[DatabaseObjects.Columns.ContactLookup].Visible = false;
                    }
                    else // For Other module records are fetched by TicketID field and HIDE grdEmailActivity
                    {
                        spQuery = string.Format("{0} = '{1}' and Deleted = 0", DatabaseObjects.Columns.TicketId, ticketID);
                        ////spQuery = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>",
                        ////                                  DatabaseObjects.Columns.TicketId, ticketID);
                        grdEmailActivity.Visible = false;
                        grdActivities.Columns[DatabaseObjects.Columns.TicketId].Visible = false;
                    }
                }
            }
            // spQuery.ViewFields = viewFields;
            // spQuery.ViewFieldsOnly = true;
            //SPListItemCollection collection = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.CRMActivities, spQuery);

            DataTable collection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMActivities, $"{spQuery.ToString()} and TenantID='{ApplicationContext.TenantID}'"); //spList.GetItems(spQuery);

            DataTable dtResult = null;
            if (collection != null && collection.Rows.Count > 0)
            {
                List<int> contactIDs = new List<int>();
                foreach (DataRow spListItem in collection.Rows)
                {
                    if (spListItem[DatabaseObjects.Columns.ContactLookup] != null)
                    {
                        //int id = UGITUtility.GetLookupID(spListItem, DatabaseObjects.Columns.ContactLookup);
                        int id = UGITUtility.GetLookupID(spListItem[DatabaseObjects.Columns.ContactLookup].ToString());
                        if (!contactIDs.Contains(id))
                        {
                            contactIDs.Add(id);
                        }
                    }
                }

                dtResult = collection;

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.TicketId))
                {
                    dtResult.Columns.Add(DatabaseObjects.Columns.TicketId);
                }

                if (!dtResult.Columns.Contains("Company"))
                {
                    dtResult.Columns.Add("Company");
                }

                if (!dtResult.Columns.Contains("ContactId"))
                {
                    dtResult.Columns.Add("ContactId");
                }

                // SPList lstContacts = SPListHelper.GetSPList(DatabaseObjects.Tables.CRMContact);
                DataTable lstContacts = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMContact, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'");

                //SPQuery rQuery = new SPQuery();
                string rQuery = "";
                StringBuilder fields = new StringBuilder();

                fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Id);
                fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title);
                fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.TicketId);
                fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.CRMCompanyTitleLookup);
                //rQuery.ViewFields = fields.ToString();
                rQuery = fields.ToString();

                List<string> requiredQuery = new List<string>();
                List<string> conExprs = new List<string>();
                foreach (int ct in contactIDs)
                {
                    conExprs.Add(string.Format("<Eq><FieldRef Name='{0}'/><Value Type='Number'>{1}</Value></Eq>", DatabaseObjects.Columns.Id, ct));
                }
                ////  requiredQuery.Add(uHelper.GenerateWhereQueryWithAndOr(conExprs, conExprs.Count - 1, false));

                // rQuery.Query = string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(requiredQuery, requiredQuery.Count - 1, true));
                //// rQuery  = string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(requiredQuery, requiredQuery.Count - 1, true));

                string modulename = uHelper.getModuleNameByTicketId(DatabaseObjects.Columns.Id);
                // SPListItemCollection resultCollection = lstContacts.GetItems(rQuery);

                //spQuery = string.Format("{0} = {1} and Deleted = 0 ",
                //                                           DatabaseObjects.Columns.ID, Convert.ToInt32((collection.Rows[0][DatabaseObjects.Columns.ContactLookup])));

                ////DataTable resultCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMContact, $"{spQuery.ToString()} and TenantID='{ApplicationContext.TenantID}'"); 
                //// DataTable resultCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMContact, $"{spQuery.ToString()} and TenantID='{ApplicationContext.TenantID}'");
                DataTable resultCollection = null;
                if (resultCollection != null)
                {
                    foreach (DataRow item in resultCollection.Rows)
                    {
                        string title = Convert.ToString(item[DatabaseObjects.Columns.Title]);
                        if (!string.IsNullOrEmpty(title))
                        {
                            title = title.Replace("'", "''");
                        }

                        DataRow[] drs = dtResult.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ContactLookup, title)).ToArray();
                        if (drs != null && drs.Length > 0)
                        {
                            foreach (DataRow dr in drs)
                            {
                                if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.CRMCompanyLookup))
                                    //// dr["Company"] = uHelper.GetLookupValue(item, DatabaseObjects.Columns.CRMCompanyTitleLookup);
                                    if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.TicketId))
                                        dr["ContactId"] = Convert.ToString(item[DatabaseObjects.Columns.TicketId]);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i <= collection.Rows.Count - 1; i++)
            {
                dtResult.Rows[i]["ContactId"] = collection.Rows[i][DatabaseObjects.Columns.ContactLookup];
            }

            if (dtResult != null)
            {
                DataView dataView = dtResult.AsDataView();

                if (ByContact)
                {
                    dataView.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.ContactLookup);
                }
                else
                {
                    dataView.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.UGITDueDate);
                }

                dtResult = dataView.ToTable();
                //DataRow[] drActivities = dtResult.Select(string.Format("{0} = 'Email'", DatabaseObjects.Columns.Type));
                //DataRow[] drEmailActivities = dtResult.Select(string.Format("{0} is null OR {0} <> 'Email'", DatabaseObjects.Columns.Type));
                //grdEmailActivity.DataSource = drActivities != null && drActivities.Length > 0 ? drActivities.CopyToDataTable() : null;
                //grdEmailActivity.DataBind();

                //grdActivities.DataSource = drEmailActivities != null && drEmailActivities.Length > 0 ? drEmailActivities.CopyToDataTable() : null;
                grdActivities.DataSource = dataView.ToTable();
                grdActivities.DataBind();
            }
            else
            {
                grdEmailActivity.DataSource = null;
                grdEmailActivity.DataBind();

                grdActivities.DataSource = null;
                grdActivities.DataBind();
            }
        }

        protected void aTitle_Load(object sender, EventArgs e)
        {
            HtmlAnchor aHtml = (HtmlAnchor)sender;
            aHtml.InnerText = UGITUtility.StripHTML(Convert.ToString((aHtml.NamingContainer as GridViewDataItemTemplateContainer).Text));
        }

        protected void grdActivities_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            //ASPxGridView grid = (ASPxGridView)sender;
            // DevExpress grid = (DevExpress.Web)sender;

            CRMActivitiesManager CRMActivitiesManager = new CRMActivitiesManager(HttpContext.Current.GetManagerContext());
            CRMActivities CRMActivities = new CRMActivities();
            object id = e.KeyValue;
            if (e.CommandArgs.CommandName == "MarkAsComplete")
            {
                int activityId = int.Parse(e.CommandArgs.CommandArgument.ToString());
                if (activityId > 0)
                {
                    CRMActivities = CRMActivitiesManager.LoadByID(activityId);
                    //string viewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.ActivityStatus);
                    ////SPListItem spListItemActivity = SPListHelper.GetSPListItem(DatabaseObjects.Lists.CRMActivities, activityId, SPContext.Current.Web, viewFields);

                    //  DataTable spListItemActivity = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMActivities, $"{viewFields.ToString()} and TenantID='{ApplicationContext.TenantID}'");

                    //spListItemActivity.Rows[0][DatabaseObjects.Columns.ActivityStatus] = "Completed";

                    //spListItemActivity.Update();                    
                    if (CRMActivities != null)
                    {
                        CRMActivities.ActivityStatus = "Completed";
                        CRMActivitiesManager.Update(CRMActivities);
                    }

                    FillData();
                }
            }

            if (e.CommandArgs.CommandName == "DeleteActivity")
            {
                int activityId = int.Parse(e.CommandArgs.CommandArgument.ToString());
                if (activityId > 0)
                {
                    string viewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.ActivityStatus);
                    ////SPListItem spListItemActivity = SPListHelper.GetSPListItem(DatabaseObjects.Lists.CRMActivities, activityId, SPContext.Current.Web, viewFields);

                    ////spListItemActivity.Delete();
                    ////uGITDAL.DeleteData("CRMActivities", "Id", DatabaseObjects.Columns.Id);

                    CRMActivities = CRMActivitiesManager.LoadByID(activityId);

                    if (CRMActivities != null)
                    {
                        CRMActivities.Deleted = true;
                        //spListItemActivity.Delete();
                        CRMActivitiesManager.Update(CRMActivities);
                    }

                    FillData();
                }
            }
        }

        protected void grdEmailActivity_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;

            if (e.CommandArgs.CommandName == "MarkAsComplete")
            {
                int activityId = int.Parse(e.CommandArgs.CommandArgument.ToString());
                if (activityId > 0)
                {
                    string viewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.ActivityStatus);
                    //SPListItem spListItemActivity = SPListHelper.GetSPListItem(DatabaseObjects.Lists.CRMActivities, activityId, SPContext.Current.Web, viewFields);
                    // spListItemActivity[DatabaseObjects.Columns.ActivityStatus] = "Completed";
                    // spListItemActivity.Update();
                    DataTable spListItemActivity = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMActivities, $"{viewFields.ToString()} and TenantID='{ApplicationContext.TenantID}'");
                    spListItemActivity.Rows[0][DatabaseObjects.Columns.ActivityStatus] = "Completed";
                    Ticket tc = new Ticket(ApplicationContext, uHelper.getModuleIdByTicketID(ApplicationContext, DatabaseObjects.Columns.Id));
                    tc.CommitChanges(spListItemActivity.Rows[0]);
                    FillData();
                }
            }

            if (e.CommandArgs.CommandName == "DeleteActivity")
            {
                int activityId = int.Parse(e.CommandArgs.CommandArgument.ToString());
                if (activityId > 0)
                {
                    //0string viewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.ActivityStatus);
                    // SPListItem spListItemActivity = SPListHelper.GetSPListItem(DatabaseObjects.Lists.CRMActivities, activityId, SPContext.Current.Web, viewFields);

                    // spListItemActivity.Delete();
                    uGITDAL.DeleteData("CRMActivities", "Id", DatabaseObjects.Columns.Id);
                    FillData();
                }
            }
        }
    }
}