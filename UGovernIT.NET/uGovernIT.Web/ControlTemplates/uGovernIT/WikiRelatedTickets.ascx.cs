using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class WikiRelatedTickets : UserControl
    {
        public string CurrentTicketId { get; set; }
        public string ModuleName { get; set; }

        private string newParam = "listpicker";
        private string formTitle = "Picker List";
        private DataTable finalResult;
        protected string detailsUrl = string.Empty;

        private const string absoluteUrlView = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Module={2}&TicketId={3}&Type={4}&ParentModule={5}";

        private ApplicationContext _applicationContext = null;

        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_applicationContext == null)
                {
                    _applicationContext = HttpContext.Current.GetManagerContext();
                }
                return _applicationContext;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //grid.Theme = "CustomMaterial";
            detailsUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=wikiDetails");
            _applicationContext = HttpContext.Current.GetManagerContext();
            if (!IsPostBack)
            {
                BindWikiTickets();
                string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, newParam, formTitle, "WIKI", CurrentTicketId, "TicketWiki", new Ticket(ApplicationContext, ModuleName)));
                aAddItem.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

                string delegateUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/DelegateControl.aspx?control=wikiList&action=saveRelatedWiki&RelatedTicketId={0}&ModuleName={1}", CurrentTicketId, ModuleName));
                aAddNew.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}' , '', 'New Wiki Article', '900px', '90', 0, '{1}')", delegateUrl, Server.UrlEncode(Request.Url.AbsolutePath)));


            }
            DataRow currentTicket = Ticket.GetCurrentTicket(ApplicationContext,uHelper.getModuleNameByTicketId(CurrentTicketId), CurrentTicketId);
            if (currentTicket != null)
            {
                if (Ticket.IsActionUser(_applicationContext, currentTicket, ApplicationContext.CurrentUser) || Ticket.IsDataEditor(currentTicket, ApplicationContext) || ApplicationContext.UserManager.IsUGITSuperAdmin(ApplicationContext.CurrentUser)
                    || ApplicationContext.UserManager.IsAdmin(ApplicationContext.CurrentUser))
                    divAddNewRelatedWiki.Visible = true;
            }
        }

        private void BindWikiTickets()
        {
            UGITTaskManager uGITTaskManager = new UGITTaskManager(_applicationContext);
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", _applicationContext.TenantID);
            DataTable dtWikiTickets = GetTableDataManager.GetData(DatabaseObjects.Tables.WikiArticles, values);
            if (dtWikiTickets != null && dtWikiTickets.Rows.Count > 0)
            {
                DataTable dt = UGITUtility.ToDataTable<UGITTask>(uGITTaskManager.LoadByProjectID(CurrentTicketId));
                DataTable dtUnSelectedTickets = dtWikiTickets;
                DataTable dtSelectedTickets = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] dr = dt.Select(string.Format("RelatedModule= '{0}'", "WIKI"));
                    if (dr != null && dr.Length > 0)
                    {
                        dt = dr.CopyToDataTable();

                        string ticketIds = string.Empty;
                        foreach (DataRow row in dt.Rows)
                        {
                            ticketIds = ticketIds + "'" + Convert.ToString(row[DatabaseObjects.Columns.RelatedTicketID]) + "',";
                        }

                        if (!string.IsNullOrEmpty(ticketIds))
                            ticketIds = ticketIds.Substring(0, ticketIds.Length - 1);

                        DataRow[] drSelectedTickets = dtWikiTickets.Select(string.Format("TicketId IN ({0})", ticketIds));
                        if (drSelectedTickets != null && drSelectedTickets.Length > 0)
                            dtSelectedTickets = drSelectedTickets.CopyToDataTable();
                    }
                }

                if (dtSelectedTickets != null && dtSelectedTickets.Rows.Count > 0)
                {
                    finalResult = dtSelectedTickets;
                    grid.DataSource = finalResult;
                    grid.DataBind();
                }
            }
            else
            {
                grid.Visible = false;
            }
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (finalResult == null)
                BindWikiTickets();

            grid.DataSource = finalResult;
        }
    }
}
