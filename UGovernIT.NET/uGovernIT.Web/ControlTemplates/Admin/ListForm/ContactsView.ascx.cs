using DevExpress.Web;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.HtmlControls;
using uGovernIT.DAL;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.Admin.ListForm
{
    public partial class ContactsView : System.Web.UI.UserControl
    {
        public string ticketID { get; set; }
        public string FrameId;
        public bool ReadOnly;
        public bool ShowSearchOption { get; set; }
        public string ControlId { get; set; }
        ////SPListItem spItem = null;
        DataRow spItem = null;
        ////DataTable dtActivities = SPListHelper.GetDataTable(DatabaseObjects.Lists.CRMActivities);
        //DataTable dtActivities = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMActivities);
        string addNewItem;
        //private string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}&ticketID={2}";
        //private string formTitle = "Contacts";
        //private string activityUrl = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}&ticketID={2}";
        private string ContactActivityUrl = "/Layouts/uGovernIT/DelegateControl.aspx?control=ContactActivity";
        ModuleViewManager moduleViewManager = null;
        UGITModule moduleDetail = new UGITModule();
        private ApplicationContext _context = null;
        FieldConfigurationManager fmanger = null;

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

            ////spItem = Ticket.getCurrentTicket("COM", ticketID);
            _context = HttpContext.Current.GetManagerContext();
            spItem = Ticket.GetCurrentTicket(_context, "COM", ticketID);
            grdContacts.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
            fmanger = new FieldConfigurationManager(_context);

            // .
            if (ShowSearchOption)
            {
                aAddItem_Top.Visible = false;
                trSearch.Visible = true;
                grdContacts.AllColumns["Edit"].Visible = false;
            }
            else
            {
                string parameterComp = string.Empty;
                if (spItem != null)
                {
                    int compId = UGITUtility.StringToInt(spItem[DatabaseObjects.Columns.Id]);



                    if (compId > 0)
                    {
                        parameterComp = string.Format("&CompanyId={0}", compId);
                    }

                    ////DataRow moduleDetail = uGITCache.GetModuleDetails("CON");
                    moduleViewManager = new ModuleViewManager(ApplicationContext);
                    moduleDetail = moduleViewManager.GetByName("CON");
                    addNewItem = string.Format("{0}?TicketId=0&isudlg=1&hpac=true{1}", UGITUtility.GetAbsoluteURL(moduleDetail.StaticModulePagePath.ToString()), parameterComp);
                    //aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2}','100','500px',0,'{1}')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), UGITUtility.newTicketTitle("CON")));
                    aAddItem_Top.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','100','500px',0,'{1}')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), "New Contact"));


                }
                grdContacts.AllColumns["Edit"].Visible = true;
                trSearch.Visible = false;
            }

            addNewItem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=acrtypenew");
           

            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            BindGrid();
            if (!IsPostBack)
            {
                object cacheVal = Context.Cache.Get(string.Format("EditActivityInfo-{0}", HttpContext.Current.CurrentUser().Id));

                if (cacheVal != null)
                {
                    Context.Cache.Remove(string.Format("EditActivityInfo-{0}", HttpContext.Current.CurrentUser().Id));

                    Dictionary<string, string> cacheParams = UGITUtility.GetCustomProperties(Convert.ToString(cacheVal), "&");
                    if (cacheParams != null && cacheParams.Count > 0 && cacheParams.ContainsKey("contactid"))
                    {
                        grdContacts.Selection.SelectRowByKey(cacheParams["contactid"]);
                    }
                }
            }
            base.OnLoad(e);
        }


        void BindGrid()
        {
            TicketManager ticketMGR = new TicketManager(ApplicationContext);
            DataTable collection =ticketMGR.GetAllTickets(moduleViewManager.LoadByName(ModuleNames.CON));
            DataTable dtResult = null;
            if (collection != null && collection.Rows.Count > 0)
            {
                // dtResult = collection.GetDataTable();
                dtResult = collection;
                dtResult.Columns.Add("TicketStatus",typeof(string));
                
                
                foreach (DataRow dr in dtResult.Rows)
                {
                    if (Convert.ToString(dr[DatabaseObjects.Columns.TicketClosed]) == "False")
                    {
                        dr["TicketStatus"] = "Active";
                        //dr[DatabaseObjects.Columns.TicketClosed] = "Active";
                    }
                    else
                    {

                        dr["TicketStatus"] = "Inactive";
                        //dr[DatabaseObjects.Columns.TicketClosed] = false;
                    }
                }
                // DataTable versionTable = GetVersionData();
                dtResult.DefaultView.Sort = DatabaseObjects.Columns.Title + " ASC";
            }

            grdContacts.DataSource = dtResult;
            grdContacts.DataBind();
            grdContacts.ExpandAll();
        }

        protected void grdContacts_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            ////Ticket ContactTicketRequest = new Ticket(SPContext.Current.Web, "CON");
            //Ticket ContactTicketRequest = new Ticket(_context,"CON");

            string func = string.Empty;
            //func = string.Format("openContactDialog('{0}','{1}','{2}')", uHelper.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, e.KeyValue, ticketID)), "", formTitle + " - Edit Item");
            DataRow dr = grdContacts.GetDataRow(e.VisibleIndex);
            string title = string.Empty;
            if (dr != null)
            {
                ////SPListItem currentListItem = UGITUtility.GetTicket(Convert.ToString(dr[DatabaseObjects.Columns.TicketId]));
                if (Convert.ToString(dr[DatabaseObjects.Columns.TicketClosed]) == "False")
                {
                    //dr[DatabaseObjects.Columns.TicketClosed] = "Active";
                    dr["TicketStatus"] = "Active";
                }
                else
                {
                    //dr[DatabaseObjects.Columns.TicketClosed] = "De-Active";
                    dr["TicketStatus"] = "De-Active";
                    
                }

               //// LifeCycleStage currentLifeCycleStage = ContactTicketRequest.GetTicketCurrentStage(currentListItem);

                title = string.Format("{0}:{1}", Convert.ToString(dr[DatabaseObjects.Columns.TicketId]), uHelper.ReplaceInvalidCharsInURL(Convert.ToString(dr[DatabaseObjects.Columns.Title])));
            }

            //DataRow moduleDetail = uGITCache.GetModuleDetails("CON");
            moduleDetail = moduleViewManager.GetByName("CON");
            string editContactUrl = string.Format("{0}?TicketId={1}&isudlg=1", UGITUtility.GetAbsoluteURL(moduleDetail.StaticModulePagePath.ToString()), Convert.ToString(dr[DatabaseObjects.Columns.TicketId]));
            func = string.Format("javascript:event.cancelBubble=true;javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','100','650px',0,'{1}')", editContactUrl, Server.UrlEncode(Request.Url.AbsolutePath), title);
            HtmlImage img = (HtmlImage)grdContacts.FindRowCellTemplateControl(e.VisibleIndex, null, "editLink");
            if (img != null)
                img.Attributes.Add("onClick", func);
            e.Row.Attributes.Add("onclick", string.Format("ContactRowClick('{0}','{1}')", ContactActivityUrl, Convert.ToString(dr[DatabaseObjects.Columns.TicketId])));
        }

        protected void grdContacts_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            //if (e.DataColumn.FieldName == DatabaseObjects.Columns.CRMCompanyLookup)
            //{
            //    string values = Convert.ToString(e.GetValue(e.DataColumn.FieldName));
            //    //fmanger = new FieldConfigurationManager(context);
            //    if (fmanger.GetFieldByFieldName(e.DataColumn.FieldName) != null)
            //    {
            //        string value = fmanger.GetFieldConfigurationData(e.DataColumn.FieldName, values);
            //        e.Cell.Text = value;
            //    }
            //}
        }


        protected void CallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            if (ShowSearchOption)
            {
                var objSelected = grdContacts.GetSelectedFieldValues(DatabaseObjects.Columns.Id);
                if (objSelected != null && objSelected.Count > 0)
                {
                    string sourceUrl = string.Empty;
                    if (Context.Request["source"] != null && Context.Request["source"].Trim() != string.Empty)
                    {
                        sourceUrl = Context.Request["source"].Trim();
                    }
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("ControlId", this.ControlId);
                    dict.Add("LookupId", Convert.ToString(objSelected[0]));
                    dict.Add("frameUrl", sourceUrl);
                     var vals = UGITUtility.GetJsonForDictionary(dict);
                       uHelper.ClosePopUpAndEndResponse(Context, false, vals);
                }
            }
        }
    }
}