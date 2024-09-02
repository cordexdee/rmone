using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using System.Data;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using System.Web;
using DevExpress.Web;

namespace uGovernIT.Web
{
    public partial class GovernanceReview : System.Web.UI.UserControl
    {
        public bool IsReadOnly;
        public string FrameId;
        DataTable budgetData;
        public string TicketActionUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=ticketaction");
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
                GenerateColumns();

            BindITGReviewList();
        }
        protected void BindITGReviewList()
        {
            
            ModuleViewManager objmgr = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            string nprAbsoluteURL = UGITUtility.GetAbsoluteURL(Convert.ToString(objmgr.LoadByName(ModuleNames.NPR).StaticModulePagePath));
            string pmmAbsoluteURL = UGITUtility.GetAbsoluteURL(Convert.ToString(objmgr.LoadByName(ModuleNames.PMM).StaticModulePagePath));
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", HttpContext.Current.GetManagerContext().TenantID);
            values.Add("@url", UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ProjectManagement.aspx"));
            values.Add("@nprAbsoluteURL", nprAbsoluteURL);
            values.Add("@pmmAbsoluteURL", pmmAbsoluteURL);
            values.Add("@userid", HttpContext.Current.CurrentUser().Id);
            budgetData = GetTableDataManager.GetData("Governencedtl", values);
            itgGrid.DataSource = budgetData;
            itgGrid.DataBind();

        }
        private void GenerateColumns()
        {
            if (itgGrid.Columns.Count != 0)
                return;

            GridViewDataTextColumn colId = null;

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.TicketId;
            colId.Caption = "Project ID";
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            colId.Width = new Unit("100px");
            itgGrid.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.TitleLink;
            colId.Caption = "Project Name";
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            itgGrid.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

            colId.FieldName = DatabaseObjects.Columns.TicketSponsors;
            colId.Caption = "Sponsors";
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            itgGrid.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = DatabaseObjects.Columns.TicketPriorityLookup;
            colId.Caption = "Priority";
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            itgGrid.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = DatabaseObjects.Columns.TicketStatus;
            colId.Caption = "Status";
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            itgGrid.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Right;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.BudgetAmountWithLink;
            colId.Caption = "Budget";
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            itgGrid.Columns.Add(colId);


            GridViewDataDateColumn dateTimeColumn = null;

            dateTimeColumn = new GridViewDataDateColumn();
            dateTimeColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            dateTimeColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            dateTimeColumn.FieldName = DatabaseObjects.Columns.TicketActualStartDate;
            dateTimeColumn.Caption = "Start Date";
            dateTimeColumn.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
            dateTimeColumn.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
            dateTimeColumn.HeaderStyle.Font.Bold = true;
            dateTimeColumn.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            dateTimeColumn.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            itgGrid.Columns.Add(dateTimeColumn);

            dateTimeColumn = new GridViewDataDateColumn();
            dateTimeColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            dateTimeColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            dateTimeColumn.FieldName = DatabaseObjects.Columns.TicketActualCompletionDate;
            dateTimeColumn.Caption = "End Date";
            dateTimeColumn.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
            dateTimeColumn.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
            dateTimeColumn.HeaderStyle.Font.Bold = true;
            dateTimeColumn.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            dateTimeColumn.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            itgGrid.Columns.Add(dateTimeColumn);
        }
        protected void itgGrid_DataBinding(object sender, EventArgs e)
        {
            if (budgetData == null)
            {
                BindITGReviewList();
            }
            itgGrid.DataSource = budgetData;
        }
        protected void itgGrid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                DataRow dataRow = itgGrid.GetDataRow(e.VisibleIndex);
                string moduleName = Convert.ToString(dataRow[DatabaseObjects.Columns.ModuleName]);
                ModuleViewManager objmgr = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                //Ajax button ITG review tab ::start                

                if (moduleName == "NPR")
                {
                    UGITModule nprModule = objmgr.LoadByName(moduleName);
                    LifeCycle lifeCycle = nprModule.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
                    LifeCycleStage itgReview = lifeCycle.Stages.FirstOrDefault(x => x.CustomProperties != null && x.CustomProperties.Contains("ITGReview"));
                    LifeCycleStage itgITSCReview = lifeCycle.Stages.FirstOrDefault(x => x.CustomProperties != null && x.CustomProperties.Contains("ITSCReview"));

                    int currentStageStep = UGITUtility.StringToInt(dataRow[DatabaseObjects.Columns.StageStep]);
                    if ((itgReview != null && currentStageStep == itgReview.StageStep) || (itgITSCReview != null && currentStageStep == itgITSCReview.StageStep))
                    {
                        string ticketTitle = UGITUtility.Truncate(Convert.ToString(dataRow[DatabaseObjects.Columns.Title]), 20);
                        string ticketPublicID = Convert.ToString(dataRow[DatabaseObjects.Columns.TicketId]);

                        string imgHoldBtnHtml = string.Empty;
                        string imgUnHoldBtnHtml = string.Empty;
                        string imgReturnBtnHtml = string.Empty;

                        string imgApproveBtnHtml = string.Format("<img style='cursor: pointer; height:17px;width:17px;display:inline;' src='/content/images/Approved.gif' title='Approve' onclick='event.cancelBubble= true; actionBtnHandle(this,\"{0}\",\"approve\",\"{1}\",\"{2}\")' />", moduleName, ticketTitle, ticketPublicID);
                        string imgRejectBtnHtml = string.Format("<img style='cursor: pointer; height:17px;width:17px;display:inline;' src='/content/images/Rejected.gif' title='Reject' onclick='event.cancelBubble= true; actionBtnHandle(this,\"{0}\",\"reject\",\"{1}\",\"{2}\");'/>", moduleName, ticketTitle, ticketPublicID);
                        imgReturnBtnHtml = string.Format(" <img style='cursor: pointer; height:17px;width:17px;display:inline;' src='/content/images/Return.png' title='Return' onclick='event.cancelBubble= true; actionBtnHandle(this,\"{0}\",\"return\",\"{1}\",\"{2}\");' />", moduleName, ticketTitle, ticketPublicID);

                        if (dataRow[DatabaseObjects.Columns.TicketStatus].ToString() == Constants.OnHoldStatus)
                        {
                            //show unhold button if ticket on hold
                            imgUnHoldBtnHtml = string.Format("<img style='cursor: pointer; height:17px;width:17px;display:inline;' src='/content/images/unlock.png' title='Remove Hold' onclick='event.cancelBubble= true; actionBtnHandle(this,\"{0}\",\"unhold\",\"{1}\",\"{2}\");' />", moduleName, ticketTitle, ticketPublicID);
                            //hide all other ajax button if ticket on hold
                            imgReturnBtnHtml = string.Empty;
                            imgApproveBtnHtml = string.Empty;
                            imgRejectBtnHtml = string.Empty;
                        }
                        else
                        {
                            imgHoldBtnHtml = string.Format("<img style='cursor: pointer; height:17px;width:17px;display:inline;' src='/content/images/lock.png' title='Put on Hold' onclick='event.cancelBubble= true; actionBtnHandle(this,\"{0}\",\"hold\",\"{1}\",\"{2}\");' />", moduleName, ticketTitle, ticketPublicID);
                        }


                        Panel panel = new Panel();
                        TableCell tCell = e.Row.Cells[itgGrid.Columns["Status"].VisibleIndex];
                        Label lbTaskAction = new Label();


                        panel.Attributes.Add("style", "position:relative;float:left;width:100%;");
                        lbTaskAction.CssClass = "action-container hidenew";

                        lbTaskAction.Text = string.Format("{0}{1}{2}{3}{4} ", imgApproveBtnHtml, imgRejectBtnHtml, imgReturnBtnHtml, imgHoldBtnHtml, imgUnHoldBtnHtml);
                        panel.Controls.Add(lbTaskAction);
                        tCell.Controls.Add(panel);

                        //Change background on mouse over
                        e.Row.Attributes.Add("onmouseover", "showTasksActions(this);");
                        e.Row.Attributes.Add("onmouseout", "hideTasksActions(this);");
                    }

                }
                else if (moduleName == "PMM")
                {
                    string ticketTitle = UGITUtility.Truncate(Convert.ToString(dataRow[DatabaseObjects.Columns.Title]), 20);
                    string ticketPublicID = Convert.ToString(dataRow[DatabaseObjects.Columns.TicketId]);

                    string imgApproveBtnHtml = string.Format("<img style='cursor: pointer; height:17px;width:17px;display:inline;' src='/content/images/Approved.gif' title='Approve' onclick='event.cancelBubble= true; actionBtnHandle(this,\"{0}\",\"approvebudget\",\"{1}\",\"{2}\")' />", moduleName, ticketTitle, ticketPublicID);
                    string imgRejectBtnHtml = string.Format("<img style='cursor: pointer; height:17px;width:17px;display:inline;' src='/content/images/Rejected.gif' title='Reject' onclick='event.cancelBubble= true; actionBtnHandle(this,\"{0}\",\"rejectbudget\",\"{1}\",\"{2}\");'/>", moduleName, ticketTitle, ticketPublicID);



                    Panel panel = new Panel();
                    TableCell tCell = e.Row.Cells[itgGrid.Columns["Status"].VisibleIndex];
                    Label lbTaskAction = new Label();


                    panel.Attributes.Add("style", "position:relative;float:left;width:100%;");
                    lbTaskAction.CssClass = "action-container hidenew";

                    lbTaskAction.Text = string.Format("{0}{1}", imgApproveBtnHtml, imgRejectBtnHtml);
                    panel.Controls.Add(lbTaskAction);
                    tCell.Controls.Add(panel);

                    //Change background on mouse over
                    e.Row.Attributes.Add("onmouseover", "showTasksActions(this);");
                    e.Row.Attributes.Add("onmouseout", "hideTasksActions(this);");


                }
                //Ajax button ITG Pending review tab::end
            }
        }
    }
}