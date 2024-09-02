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
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class EmailsControl : UserControl
    {
        public string PublicTicketID { get; set; }
        public string PublicTicket { get; set; }
        public string ModuleName { get; set; }

        private EmailsManager _emailsManager = null;

        private TicketManager _ticketManager  = null;        

        protected EmailsManager EmailsManager
        {
            get
            {
                if (_emailsManager == null)
                {
                    _emailsManager = new EmailsManager(context);
                }
                return _emailsManager;

            }
        }

        protected TicketManager TicketManager
        {
            get
            {
                if (_ticketManager == null)
                {
                    _ticketManager = new TicketManager(context);
                }
                return _ticketManager;

            }
        }

        protected string viewUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=ticketemailview");

        public bool ReadOnly;

        DataRow[] dr;

        GridViewDataTextColumn colId = null;

        ApplicationContext context = HttpContext.Current.GetManagerContext();

        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            string expression = $"{DatabaseObjects.Columns.TicketId}='{PublicTicketID}'"; 

            var dt = TicketManager.GetAllTickets(ObjModuleViewManager.LoadByName(ModuleName));
            if (dt != null)
            {
                dr = dt.Select(expression);
                if (dr != null && dr.Length > 0)
                {
                    string ticketId = Convert.ToString(dr[0][DatabaseObjects.Columns.TicketId]);
                    var emails = EmailsManager.Load(x => x.TicketId == ticketId).ToList();

                    DataTable dtTicketEmails = UGITUtility.ToDataTable<Email>(emails);
                    if (dtTicketEmails != null)
                    {
                        dtTicketEmails.Columns.Add("TicketEmailType");
                        dtTicketEmails.Columns["TicketEmailType"].Expression = string.Format("IIF(IsIncomingMail = True,'Incoming','Outgoing')");
                    }

                    if (dtTicketEmails == null || dtTicketEmails.Rows.Count < 5)
                    {
                        //ASPxGridViewTicketEmails.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                        //ASPxGridViewTicketEmails.Settings.VerticalScrollableHeight = 150;
                    }
                    //ASPxGridViewTicketEmails.Theme = "CustomMaterial";
                    ASPxGridViewTicketEmails.Settings.GridLines = GridLines.Horizontal;
                    ASPxGridViewTicketEmails.Styles.Header.Border.BorderStyle = BorderStyle.None;

                    ASPxGridViewTicketEmails.DataSource = dtTicketEmails;
                    ASPxGridViewTicketEmails.DataBind();
                }
            }
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ASPxGridViewTicketEmails_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            DataRow currentRow = ASPxGridViewTicketEmails.GetDataRow(e.VisibleIndex);

            string func = string.Empty;
            string ticketEmailID = string.Empty;

            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id) && Convert.ToString(currentRow[DatabaseObjects.Columns.Id]) != string.Empty)
            {
                ticketEmailID = currentRow[DatabaseObjects.Columns.Id].ToString().Trim();
            }
            func = string.Format("openTicketDialog('{0}','{1}','{2}','{3}','{4}', 0)", viewUrl, string.Format("TicketEmailID={0}", ticketEmailID), string.Format("Email: {0}", PublicTicketID), 90, 90);
            e.Row.Attributes.Add("onClick", func);
        }

        protected void ASPxGridViewTicketEmails_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            colId = new GridViewDataTextColumn();

            if (e.Column.FieldName == "Created")
                e.DisplayText = ((DateTime)e.Value).ToString("MMM-dd-yyyy hh:mm tt");
            else if (e.Column.FieldName == "TicketEmailType")
            {
                string cellval = Convert.ToString(e.Value) == "Incoming" ? "/Content/images/uGovernIT/inbox.png" : "/Content/images/outbox.png";
                e.DisplayText = string.Format("<img src='{0}' width='16px' height='16px'></img>", cellval);
            }
            //else if (e.Column.FieldName == DatabaseObjects.Columns.MailSubject)
            //{
            //    colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            //    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            //    colId.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
            //}
            //else if (e.Column.FieldName == DatabaseObjects.Columns.EmailIDTo)
            //{
            //    colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            //    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            //    colId.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
            //}
            //else
            //{
            //    colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //}
        }
    }
}