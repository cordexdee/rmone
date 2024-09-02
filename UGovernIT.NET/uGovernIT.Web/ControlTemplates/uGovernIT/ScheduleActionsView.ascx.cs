using DevExpress.Web;
using DevExpress.Web.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class ScheduleActionsView : UserControl
    {
        //private DataTable _DataTable;
        private string addNewItem = string.Empty;
        private string actionType = "0";

        public string ModuleName { get; set; }
        public string PublicTicket { get; set; }

        public bool IsArchive { get; set; }
        public bool IsModuleWebpart { get; set; }

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

        ScheduleActionsArchiveManager scheduleActionsArchiveManager;
        ScheduleActionsManager scheduleActionsManager;

        List<SchedulerActionArchive> scheduleActionsArchive;
        List<SchedulerAction> scheduleActions;

        DataRow[] dr;
        DataTable dtscheduleActionsArchive;
        DataTable dtscheduleActions;

        protected override void OnInit(EventArgs e)
        {
            scheduleActionsArchiveManager = new ScheduleActionsArchiveManager(ApplicationContext);
            scheduleActionsManager = new ScheduleActionsManager(ApplicationContext);
            InitializeGrid();
            hdnConfiguration.Set("NewUrl", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=scheduleactionedit&isudlg=1"));
            hdnConfiguration.Set("EditUrl", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=scheduleactionedit&isudlg=1"));
            hdnConfiguration.Set("ViewUrl", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=scheduleactionview&isudlg=1"));
            hdnConfiguration.Set("ScheduleView", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=scheduleactionslist&isudlg=1"));
            hdnConfiguration.Set("RequestUrl", Request.Url.AbsolutePath);

            FillActionType(actionType);
            if (IsModuleWebpart)
            {
                ddlActionType.Visible = false;
                div_header.Visible = false;
                aAddItem.Visible = false;
                aAddItem_Top.Visible = false;
            }
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["actiontype"] != null)
                {
                    actionType = Request["actiontype"];
                    ddlActionType.SelectedValue = actionType;
                }
            }

            base.OnLoad(e);
        }

        private void InitializeGrid()
        {
            aspxGridView.Settings.ShowFilterRowMenu = true;
            aspxGridView.Settings.ShowHeaderFilterButton = true;
            aspxGridView.Settings.ShowFooter = true;
            aspxGridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            aspxGridView.SettingsPopup.HeaderFilter.Height = 150;
            aspxGridView.Settings.ShowGroupFooter = DevExpress.Web.GridViewGroupFooterMode.VisibleAlways;
            aspxGridView.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            aspxGridView.Styles.AlternatingRow.BackColor = Color.FromArgb(234, 234, 234);
            //aspxGridView.Styles.Row.BackColor = Color.White;
            aspxGridView.Settings.GridLines = GridLines.Horizontal;
            aspxGridView.Styles.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");
            // aspxGridView.Styles.RowHotTrack.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");
            aspxGridView.SettingsBehavior.AllowSelectByRowClick = true;
            //aspxGridView.Theme = "CustomMaterial";

            #region Generate Columns
            if (!IsModuleWebpart)
            {
                aspxGridView.SettingsPager.PageSize = 20;
                aspxGridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                aspxGridView.SettingsPager.PageSizeItemSettings.Visible = true;
                aspxGridView.SettingsPager.PageSizeItemSettings.Position = DevExpress.Web.PagerPageSizePosition.Right;
                aspxGridView.SettingsPager.PageSizeItemSettings.Items = new string[] { "10", "15", "20", "25", "50", "75", "100" };
                aspxGridView.SettingsPager.Position = PagerPosition.Bottom;

                GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                col1.PropertiesEdit.EncodeHtml = false;
                aspxGridView.Columns.Add(col1);

                GridViewDataColumn colactiontype = new GridViewDataColumn();
                colactiontype.Caption = "Action Type";
                colactiontype.HeaderStyle.Font.Bold = true;
                colactiontype.FieldName = DatabaseObjects.Columns.ActionType;
                aspxGridView.Columns.Add(colactiontype);
            }
            GridViewDataTextColumn coltitle = new GridViewDataTextColumn();
            coltitle.Caption = "Title";
            coltitle.HeaderStyle.Font.Bold = true;
            coltitle.FieldName = DatabaseObjects.Columns.Title;

            GridViewDataTextColumn colrecurring = new GridViewDataTextColumn();
            colrecurring.Caption = "Recurring";
            colrecurring.HeaderStyle.Font.Bold = true;
            colrecurring.FieldName = DatabaseObjects.Columns.Recurring;

            GridViewDataTextColumn colstarttime = new GridViewDataTextColumn();
            colstarttime.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy HH:mm tt}";
            colstarttime.FieldName = DatabaseObjects.Columns.StartTime;
            if (IsModuleWebpart)
            {
                colstarttime.Caption = "Send Time";
            }
            else
            {
                colstarttime.Caption = "Start Time";
            }
            colstarttime.HeaderStyle.Font.Bold = true;

            GridViewDataTextColumn colemailto = new GridViewDataTextColumn();
            colemailto.Caption = "Email To";
            colemailto.HeaderStyle.Font.Bold = true;
            colemailto.FieldName = DatabaseObjects.Columns.EmailIDTo;

            GridViewDataTextColumn colsubject = new GridViewDataTextColumn();
            colsubject.Caption = "Email Subject";
            colsubject.HeaderStyle.Font.Bold = true;
            colsubject.FieldName = DatabaseObjects.Columns.MailSubject;
            if (IsModuleWebpart)
            {
                aspxGridView.Columns.AddRange(colstarttime, colrecurring, colemailto, colsubject);

            }
            else
            {
                aspxGridView.Columns.AddRange(coltitle, colrecurring, colstarttime, colemailto, colsubject);
            }
            if (IsArchive)
            {
                GridViewDataTextColumn colstatus = new GridViewDataTextColumn();
                colstatus.Caption = "Status";
                colstatus.HeaderStyle.Font.Bold = true;
                colstatus.FieldName = DatabaseObjects.Columns.AgentJobStatus;
                aspxGridView.Columns.Add(colstatus);
            }
            #endregion
        }

        void FillActionType(string actionType)
        {
            foreach (ScheduleActionType sactionType in Enum.GetValues(typeof(ScheduleActionType)))
            {
                ddlActionType.Items.Add(new ListItem(sactionType.ToString()));
            }
            ddlActionType.Items.Insert(0, new ListItem("All Action Types"));
        }

        protected void ddlActionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string url = string.Format("{0}&actiontype={1}", Convert.ToString(hdnConfiguration.Get("ScheduleView")), ddlActionType.SelectedValue);
            Response.Redirect(url);
        }

        protected void aspxGridView_DataBinding(object sender, EventArgs e)
        {
            // hdnConfiguration.Set("ViewUrl", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=scheduleactionview&IsModuleWebpart=true"));
            if (IsModuleWebpart)
            {
                TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
                ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                DataTable dt = ticketManager.GetAllTickets(moduleViewManager.GetByName(ModuleName));
                string expression = string.Format(DatabaseObjects.Columns.TicketId + "='{0}'", PublicTicket);
                dr = dt.Select(expression);
                string ticketId = dr[0][DatabaseObjects.Columns.TicketId].ToString();
                hdnConfiguration.Set("ViewUrl", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=scheduleactionview&IsModuleWebpart=true"));
                if (IsArchive)
                {
                    //dr = uGITCache.ModuleDataCache.GetAllTickets("TSR").Select("ID = " + PublicTicket);
                    hdnConfiguration.Set("ViewUrl", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=scheduleactionview&IsArchive=true&IsModuleWebpart=true"));
                    scheduleActionsArchive = scheduleActionsArchiveManager.Load(x => x.TicketId == ticketId);
                    dtscheduleActionsArchive = UGITUtility.ToDataTable(scheduleActionsArchive);
                }
                else
                {
                    scheduleActions = scheduleActionsManager.Load(x => x.TicketId == ticketId);
                    dtscheduleActions = UGITUtility.ToDataTable(scheduleActions);
                }
            }
            else if (ddlActionType.SelectedValue != "All Action Types")
            {
                string strquery = ddlActionType.SelectedValue;
                scheduleActions = scheduleActionsManager.Load(x => x.ActionType == strquery).OrderBy(x => x.StartTime).ToList();
                dtscheduleActions = UGITUtility.ToDataTable(scheduleActions);
            }
            else
            {
                scheduleActions = scheduleActionsManager.Load().OrderBy(x => x.StartTime).ToList();
                dtscheduleActions = UGITUtility.ToDataTable(scheduleActions);
            }

            //condtion for set the height of grid..
            if (scheduleActionsArchive != null)
            {
                if (scheduleActionsArchive.Count < 5)
                    aspxGridView.Settings.ShowHeaderFilterButton = false;

                //  aspxGridView.DataSource = scheduleActionsArchive;
                aspxGridView.DataSource = dtscheduleActionsArchive;
            }
            else if (scheduleActions != null)
            {
                if (scheduleActions.Count < 5)
                    aspxGridView.Settings.ShowHeaderFilterButton = false;

                //  aspxGridView.DataSource = scheduleActions;
                aspxGridView.DataSource = dtscheduleActions;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            aspxGridView.DataBind();
            base.OnPreRender(e);
        }

        protected void aspxGridView_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;
            DataRow currentRow = aspxGridView.GetDataRow(e.VisibleIndex);
            string func = string.Empty;
            string title = string.Empty;
            if (currentRow == null) return;
            string id = Convert.ToString(currentRow[DatabaseObjects.Columns.ID]);
            ScheduleActionType actionType = (ScheduleActionType)Enum.Parse(typeof(ScheduleActionType), Convert.ToString(currentRow[DatabaseObjects.Columns.ActionType]));
            title = string.Format("{0} ({1})",
                 (actionType != ScheduleActionType.Report && actionType != ScheduleActionType.Query && actionType != ScheduleActionType.Alert) ?
                 (Convert.ToString(currentRow[DatabaseObjects.Columns.TicketId]) == string.Empty) ?
                 Convert.ToString(currentRow[DatabaseObjects.Columns.Title]) :
                 Convert.ToString(currentRow[DatabaseObjects.Columns.TicketId]) :
                 Convert.ToString(currentRow[DatabaseObjects.Columns.Title]),
                 Convert.ToString(currentRow[DatabaseObjects.Columns.ActionType]));


            func = string.Format("javascript:EditWindowPopup('{0}','{1}')", id, title);

            if (actionType == ScheduleActionType.EscalationEmail || actionType == ScheduleActionType.AutoStageMove
                || actionType == ScheduleActionType.UnHoldTicket)
            {
                func = string.Format("javascript:ViewWindowPopup('{0}','{1}')", id, title);
                if (IsModuleWebpart)
                {
                    e.Row.Attributes.Add("onClick", func);
                }
            }

            foreach (object cell in e.Row.Cells)
            {
                if (cell is GridViewTableDataCell)
                {
                    GridViewTableDataCell editCell = (GridViewTableDataCell)cell;

                    int cellIndex = ((GridViewDataColumn)editCell.Column).Index;
                    if (!IsModuleWebpart)
                    {
                        if (cellIndex == 0)
                        {
                            e.Row.Cells[((GridViewDataColumn)editCell.Column).Index].Text =
                                string.Format("<a id=\"anchor\" runat=\"server\" onload=\"anchor_Load\" href=\"{0}\" ><img id=\"Imgedit\" width=\"16\" runat=\"server\" src=\"/Content/Images/editNewIcon.png\"/></a>", func);
                        }
                    }
                    if (((GridViewDataColumn)editCell.Column).Caption == DatabaseObjects.Columns.Title)
                    {
                        e.Row.Cells[((GridViewDataColumn)editCell.Column).Index].Text =
                            string.Format("<a id=\"anchor\" runat=\"server\" onload=\"anchor_Load\" href=\"{0}\" >{1}</a>", func, title);
                    }
                    if (((GridViewDataColumn)editCell.Column).Caption == DatabaseObjects.Columns.Recurring)
                    {
                        e.Row.Cells[((GridViewDataColumn)editCell.Column).Index].Text = (Convert.ToString(currentRow[DatabaseObjects.Columns.Recurring]) == "True" ? "Yes" : "No");
                    }
                }
            }
        }
    }
}