using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using uGovernIT.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Web;
using DevExpress.Web.Rendering;
using uGovernIT.Core;
using System.Web.UI.HtmlControls;
using System.Collections.ObjectModel;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Manager.Helper;

namespace uGovernIT.Web
{
    public partial class ITGPortfolio : UserControl
    {
        List<string[]> ids;
        public bool IsReadOnly;
        public string FrameId;
        private DataTable budgetData;
        protected string ticketActionUrl;
        public string TicketActionUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/delegatecontrol.aspx?control=ticketaction");
        private DateTime yearStartDate = DateTime.Now;
        private DateTime yearEndDate = DateTime.Now;

        public string strTicketIDs = string.Empty;
        public string yearType = string.Empty;
        public string allocationType = string.Empty;
        public string year = string.Empty;
        public bool isPercentage = false;
        public bool isPendingApproval = false;
        public bool isApprovedProjectRequests = false;
        public string reporturl;
        ApplicationContext AppContext;
        ModuleViewManager ModuleManagerObj;
        FieldConfigurationManager FieldManagerObj;

        protected void Page_Init(object sender, EventArgs e)
        {
            AppContext = HttpContext.Current.GetManagerContext();
            ModuleManagerObj = new ModuleViewManager(AppContext);
            FieldManagerObj = new FieldConfigurationManager(AppContext);
            lblSelectedYear.Text = DateTime.Now.Date.Year.ToString();
            reporturl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
            //itgGrid.DataBind();
            //itgGrid.CollapseAll();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            constraintsContainer.Visible = false;
            measuresContainrer.Visible = false;
            scriptPanel.Visible = false;
            if(!IsPostBack)
                itgGrid.DataBind();
            FilterListView(FilterCheckBox_cp, new EventArgs());
            
            if (Request.Form["__CALLBACKPARAM"] != null)
            {
                string[] val = Request.Form["__CALLBACKPARAM"].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                if (Request.Form["__CALLBACKPARAM"].Contains("CUSTOMCALLBACK"))
                {

                    if (Request.Form["__CALLBACKPARAM"].ToString().Contains("DRAGROW"))
                    {

                        if (val.Length > 1)
                        {
                            ITG helper = new ITG(AppContext);
                            if (val[val.Length - 2].Replace(";", string.Empty) != "undefined" && val[val.Length - 1].Replace(";", string.Empty) != "undefined")
                            {
                                helper.UpdateGroup(val[val.Length - 2].Replace(";", string.Empty), val[val.Length - 1].Replace(";", string.Empty), ddlviewtype.SelectedValue);
                                itgGrid.DataBind();
                            }

                        }
                    }
                }
            }
            if (!IsPostBack)
            {
                ddlviewtype_SelectedIndexChanged(null, null);
                //itgGrid.CollapseAll();
            }
        }
        protected void BindITGList()
        {
            string filter = filterId.Value;
            List<string> filterIDs = UGITUtility.ConvertStringToList(filter, ",");
            FilterById(filterIDs);
        }
        protected void BindITGConstraints()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Budget", typeof(string));
            dt.Columns.Add("Actual", typeof(string));
            double budgetAmount = 0;
            double actualAmount = 0;
            foreach (string[] selectedTicketId in ids)
            {
                DataRow dr = budgetData.AsEnumerable().FirstOrDefault(x => x.Field<String>(DatabaseObjects.Columns.TicketId) == selectedTicketId[1]);
                if (dr != null)
                {
                    budgetAmount += Convert.ToDouble(dr[DatabaseObjects.Columns.BudgetAmount]);
                    actualAmount += Convert.ToDouble(dr["Actual"]);
                }

            }
            DataRow drCost = dt.NewRow();
            string strbudgetAmount = String.Format("{0:C}", budgetAmount);
            string stractualAmount = String.Format("{0:C}", actualAmount);
            drCost["Budget"] = strbudgetAmount;
            drCost["Actual"] = stractualAmount;
            dt.Rows.Add(drCost);
            lvITGConstraints.DataSource = dt;// ITG.Constraints.LoadById(ids, budgetData);
            lvITGConstraints.DataBind();
        }
        protected void BindITGMeasures()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Q1", typeof(string));
            dt.Columns.Add("Q2", typeof(string));
            dt.Columns.Add("Q3", typeof(string));
            dt.Columns.Add("Q4", typeof(string));
            double resourceQ1 = 0;
            double resourceQ2 = 0;
            double resourceQ3 = 0;
            double resourceQ4 = 0;
            foreach (string[] selectedTicketId in ids)
            {
                DataRow dr = budgetData.AsEnumerable().FirstOrDefault(x => x.Field<String>(DatabaseObjects.Columns.TicketId) == selectedTicketId[1]);
                if (dr != null)
                {
                    resourceQ1 += Convert.ToDouble(dr["Q1"]);
                    resourceQ2 += Convert.ToDouble(dr["Q2"]);
                    resourceQ3 += Convert.ToDouble(dr["Q3"]);
                    resourceQ4 += Convert.ToDouble(dr["Q4"]);
                }

            }
            DataRow drCost = dt.NewRow();
            if (rdbAllocationType.SelectedItem.Text == "Percentage")
            {
                drCost["Q1"] = string.Format("{0}%", Math.Round(resourceQ1, 1));
                drCost["Q2"] = string.Format("{0}%", Math.Round(resourceQ2, 1));
                drCost["Q3"] = string.Format("{0}%", Math.Round(resourceQ3, 1));
                drCost["Q4"] = string.Format("{0}%", Math.Round(resourceQ4, 1));
            }
            else
            {
                drCost["Q1"] = resourceQ1;
                drCost["Q2"] = resourceQ2;
                drCost["Q3"] = resourceQ3;
                drCost["Q4"] = resourceQ4;
            }

            dt.Rows.Add(drCost);
            lvITGMeasures.DataSource = dt;
            lvITGMeasures.DataBind();
        }
        protected void FilterListView(object sender, EventArgs e)
        {
            List<string> filterIdPost = new List<string>();
            if (FilterCheckBox_cp.Checked)
            {
                filterIdPost.Add(Constants.ProjectType.CurrentProjects.ToString());
            }
            if (FilterCheckBox_apr.Checked)
            {
                filterIdPost.Add(Constants.ProjectType.ApprovedNPRs.ToString());
            }
            if (FilterCheckBox_pa.Checked)
            {
                filterIdPost.Add(Constants.ProjectType.PendingApprovalNPRs.ToString());
            }
            if (FilterCheckBox_rop.Checked)
            {
                filterIdPost.Add(Constants.ProjectType.OnHold.ToString());
            }
            if (FilterCheckBox_cpp.Checked)
            {
                filterIdPost.Add(Constants.ProjectType.CompletedProjects.ToString());
            }
            FilterById(filterIdPost);
            filterId.Value = string.Join(",", filterIdPost);
            constraintsContainer.Visible = false;
            measuresContainrer.Visible = false;
            itgGrid.DataBind();

            //hdnBtnSelectionChanged_Click(null, null);
            //ddlviewtype_SelectedIndexChanged(null, null);
        }
        protected void FilterById(List<string> filterIdPost)
        {
            List<Constants.ProjectType> filters = new List<Constants.ProjectType>();
            foreach (string filter in filterIdPost)
            {
                if (filter == Constants.ProjectType.CurrentProjects.ToString())
                {
                    filters.Add(Constants.ProjectType.CurrentProjects);
                    continue;
                }
                else if (filter == Constants.ProjectType.ApprovedNPRs.ToString())
                {
                    filters.Add(Constants.ProjectType.ApprovedNPRs);
                    continue;
                }
                else if (filter == Constants.ProjectType.PendingApprovalNPRs.ToString())
                {
                    filters.Add(Constants.ProjectType.PendingApprovalNPRs);
                    continue;
                }
                else if (filter == Constants.ProjectType.OnHold.ToString())
                {
                    filters.Add(Constants.ProjectType.OnHold);
                    continue;
                }
                else if (filter == Constants.ProjectType.CompletedProjects.ToString())
                {
                    filters.Add(Constants.ProjectType.CompletedProjects);
                    continue;
                }
            }
            if (filters.Count == 0)
            {
                filters.Add(Constants.ProjectType.All);
            }

            budgetData = ITG.Portfolio.LoadAll(AppContext, filters);
            filterId.Value = string.Join(",", filterIdPost);
            ITG.Portfolio.BindResourceAllocationData(AppContext, budgetData, ddlYear.SelectedItem.Text, rdbAllocationType.SelectedItem.Text, Convert.ToInt32(lblSelectedYear.Text), FilterCheckBox_apr.Checked, FilterCheckBox_pa.Checked);

            allocationType = rdbAllocationType.SelectedItem.Text;
            yearType = ddlYear.SelectedItem.Text;
            year = lblSelectedYear.Text;
            isPendingApproval = FilterCheckBox_pa.Checked;
            isPendingApproval = FilterCheckBox_apr.Checked;

            if (rdbAllocationType.SelectedValue == "1")
                isPercentage = true;

        }



        protected void itgGrid_DataBinding(object sender, EventArgs e)
        {
            if (budgetData == null)
            {
                BindITGList();
            }
            itgGrid.DataSource = budgetData;
        }

        protected void itgGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                DataRow dataRow = itgGrid.GetDataRow(e.VisibleIndex);
                string moduleName = Convert.ToString(dataRow[DatabaseObjects.Columns.ModuleName]);


                //Ajax button ITGPortfilio ::start                

                if (moduleName == "NPR")
                {
                    UGITModule nprModule = ModuleManagerObj.GetByName(moduleName); //  uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, moduleName);
                    LifeCycle lifeCycle = nprModule.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
                    LifeCycleStage itgReview = lifeCycle.Stages.FirstOrDefault(x => x.CustomProperties != null && x.CustomProperties.Contains("ITGReview"));
                    LifeCycleStage itgITSCReview = lifeCycle.Stages.FirstOrDefault(x => x.CustomProperties != null && x.CustomProperties.Contains("ITSCReview"));

                    int currentStageStep = UGITUtility.StringToInt(dataRow[DatabaseObjects.Columns.StageStep]);
                    if ((itgReview != null && currentStageStep == itgReview.StageStep) || (itgITSCReview != null && currentStageStep == itgITSCReview.StageStep))
                    {

                        string ticketTitle = UGITUtility.Truncate(Convert.ToString(dataRow[DatabaseObjects.Columns.Title]), 20);
                        string ticketPublicID = Convert.ToString(dataRow[DatabaseObjects.Columns.TicketId]);

                        string itemId = dataRow[DatabaseObjects.Columns.Id].ToString();

                        string imgHoldBtnHtml = string.Empty;
                        string imgUnHoldBtnHtml = string.Empty;
                        string imgReturnBtnHtml = string.Empty;


                        string imgApproveBtnHtml = string.Format("<img style='cursor: pointer; height:17px;width:17px;display:inline;' src='/Content/images/Approved.gif' title='Approve' onclick='event.cancelBubble= true; nprActionBtnsHandle(this,\"{0}\",\"approve\",\"{1}\",\"{2}\")' />", moduleName, ticketTitle, ticketPublicID);
                        string imgRejectBtnHtml = string.Format("<img style='cursor: pointer; height:17px;width:17px;display:inline;' src='/Content/images/Rejected.gif' title='Reject' onclick='event.cancelBubble= true; nprActionBtnsHandle(this,\"{0}\",\"reject\",\"{1}\",\"{2}\");'/>", moduleName, ticketTitle, ticketPublicID);
                        imgReturnBtnHtml = string.Format(" <img style='cursor: pointer; height:17px;width:17px;display:inline;' src='/Content/images/Return.png' title='Return' onclick='event.cancelBubble= true; nprActionBtnsHandle(this,\"{0}\",\"return\",\"{1}\",\"{2}\");' />", moduleName, ticketTitle, ticketPublicID);

                        if (Convert.ToString(dataRow[DatabaseObjects.Columns.TicketStatus]) == Constants.OnHoldStatus)
                        {
                            //show unhold button if ticket on hold
                            imgUnHoldBtnHtml = string.Format("<img style='cursor: pointer; height:17px;width:17px;display:inline;' src='/Content/images/unlock.png' title='Remove Hold' onclick='event.cancelBubble= true; nprActionBtnsHandle(this,\"{0}\",\"unhold\",\"{1}\",\"{2}\");' />", moduleName, ticketTitle, ticketPublicID);

                            //hide all other ajax button if ticket on hold
                            imgReturnBtnHtml = string.Empty;
                            imgApproveBtnHtml = string.Empty;
                            imgRejectBtnHtml = string.Empty;
                        }
                        else
                        {
                            imgHoldBtnHtml = string.Format("<img style='cursor: pointer; height:17px;width:17px;display:inline;' src='/Content/images/lock.png' title='Put on Hold' onclick='event.cancelBubble= true; nprActionBtnsHandle(this,\"{0}\",\"hold\",\"{1}\",\"{2}\");' />", moduleName, ticketTitle, ticketPublicID);
                        }


                        Panel panel = new Panel();
                        TableCell tCell = e.Row.Cells[itgGrid.Columns["Status"].VisibleIndex];
                        Label lbTaskAction = new Label();


                        panel.Attributes.Add("style", "position:relative;float:left;width:100%;");
                        lbTaskAction.CssClass = "action-container hide";

                        lbTaskAction.Text = string.Format("{0}{1}{2}{3}{4} ", imgApproveBtnHtml, imgRejectBtnHtml, imgReturnBtnHtml, imgHoldBtnHtml, imgUnHoldBtnHtml);
                        panel.Controls.Add(lbTaskAction);
                        tCell.Controls.Add(panel);

                        //Change background on mouse over
                        e.Row.Attributes.Add("onmouseover", "showTasksActions(this);");
                        e.Row.Attributes.Add("onmouseout", "hideTasksActions(this);");
                    }

                }
            }
        }

        protected void nextYear_Click(object sender, ImageClickEventArgs e)
        {
            lblSelectedYear.Text = Convert.ToString(Convert.ToInt32(lblSelectedYear.Text) + 1);
            itgGrid.DataBind();
            FilterListView(FilterCheckBox_cp, new EventArgs());

        }
        protected void previousYear_Click(object sender, ImageClickEventArgs e)
        {
            lblSelectedYear.Text = Convert.ToString(Convert.ToInt32(lblSelectedYear.Text) - 1);
            itgGrid.DataBind();
            FilterListView(FilterCheckBox_cp, new EventArgs());
        }



        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            itgGrid.DataBind();
            FilterListView(FilterCheckBox_cp, new EventArgs());
        }
        protected void rdbAllocationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            itgGrid.DataBind();
            FilterListView(FilterCheckBox_cp, new EventArgs());

        }

        protected void projectPorFolioReport_Click(object sender, ImageClickEventArgs e)
        {
            int startVisibleIndex = itgGrid.VisibleStartIndex;
            int selectedTicketCount = itgGrid.Selection.Count;
            List<string> mTicketIds = new List<string>();
            DataRow dataRow = null;
            if (selectedTicketCount > 0)
            {
                List<object> selectedIDs = itgGrid.GetSelectedFieldValues(DatabaseObjects.Columns.TicketId);
                if (selectedIDs != null && selectedIDs.Count > 0)
                    mTicketIds = selectedIDs.Select(x => Convert.ToString(x)).ToList();
            }
            else
            {
                for (int i = startVisibleIndex; i < itgGrid.VisibleRowCount; i++)
                {
                    dataRow = (DataRow)itgGrid.GetDataRow(i);
                    if (dataRow != null)
                    {
                        if (!mTicketIds.Contains(Convert.ToString(dataRow[DatabaseObjects.Columns.TicketId])))
                            mTicketIds.Add(Convert.ToString(dataRow[DatabaseObjects.Columns.TicketId]));
                    }
                }
            }
            strTicketIDs = string.Join(",", mTicketIds);
            scriptPanel.Visible = true;
        }

        protected void ddlviewtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlviewtype.SelectedIndex != -1 && itgGrid != null)
            {
                ReadOnlyCollection<GridViewDataColumn> groupCol = itgGrid.GetGroupedColumns();
                foreach (GridViewDataColumn col in groupCol)
                {
                    col.GroupIndex = -1;
                }
                if (ddlviewtype.SelectedItem.Value == "0")
                {
                    var obj = itgGrid.Columns[DatabaseObjects.Columns.TicketPriorityLookup];
                    if (obj != null)
                    {
                        GridViewColumn col = obj as GridViewColumn;
                        itgGrid.GroupBy(col, 0);
                    }
                    obj = itgGrid.Columns[DatabaseObjects.Columns.ProjectRank];
                    if (obj != null)
                    {
                        GridViewColumn col = obj as GridViewColumn;
                        itgGrid.GroupBy(col, 1);
                    }
                }
                else if (ddlviewtype.SelectedItem.Value == "1")
                {
                    var obj = itgGrid.Columns[DatabaseObjects.Columns.TicketRequestTypeCategory];
                    if (obj != null)
                    {
                        GridViewColumn col = obj as GridViewColumn;
                        itgGrid.GroupBy(col, 0);
                    }
                    obj = itgGrid.Columns[DatabaseObjects.Columns.TicketRequestTypeLookup];
                    if (obj != null)
                    {
                        GridViewColumn col = obj as GridViewColumn;
                        itgGrid.GroupBy(col, 1);
                    }
                    obj = itgGrid.Columns[DatabaseObjects.Columns.TicketRequestTypeSubCategory];

                    if (obj != null)
                    {
                        GridViewColumn col = obj as GridViewColumn;
                        itgGrid.GroupBy(col, 2);
                    }
                }
                else if (ddlviewtype.SelectedItem.Value == "2")
                {
                    var obj = itgGrid.Columns[DatabaseObjects.Columns.ProjectInitiativeLookup];
                    if (obj != null)
                    {
                        GridViewColumn col = obj as GridViewColumn;
                        itgGrid.GroupBy(col, 0);
                    }
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected void itgGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Group)
            {
                int level = itgGrid.GetRowLevel(e.VisibleIndex);
                var groupedCol = itgGrid.GetGroupedColumns()[level];
                var colValue = Convert.ToString(e.GetValue(groupedCol.FieldName));
                if (string.IsNullOrWhiteSpace(colValue))
                    e.Row.Visible = false;
            }
        }

        protected void itgGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (e.IsTotalSummary)
            {
                if (e.Item.FieldName == "Q1" || e.Item.FieldName == "Q2" || e.Item.FieldName == "Q3" || e.Item.FieldName == "Q4")
                {
                    if (rdbAllocationType.SelectedItem.Text == "Percentage")
                    {
                        e.Text = string.Format("{0:F1}%", e.Value);
                    }
                    else
                    {
                        e.Text = string.Format("{0:F1}", e.Value);
                    }
                }

            }
        }


        protected void itgGrid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName.Contains("Lookup"))
            {
                string lookupid = Convert.ToString(e.Value);
                string values = FieldManagerObj.GetFieldConfigurationData(e.Column.FieldName, Convert.ToString(e.Value));
                if (!string.IsNullOrEmpty(values))
                {
                    e.DisplayText = values;
                }
            }
            if (e.Column.FieldName.EndsWith("User"))
            {
                string userIDs = Convert.ToString(e.Value);
                if (!string.IsNullOrEmpty(userIDs))
                {
                    if (userIDs != null)
                    {
                        string separator = Constants.Separator6;
                        if (userIDs.Contains(Constants.Separator))
                            separator = Constants.Separator;
                        List<string> userlist = UGITUtility.ConvertStringToList(userIDs, separator);

                        string commanames = AppContext.UserManager.CommaSeparatedNamesFrom(userlist, Constants.Separator6);
                        e.DisplayText = !string.IsNullOrEmpty(commanames) ? commanames : string.Empty;
                    }
                }
            }
            if (e.Column.FieldName == "Q1" || e.Column.FieldName == "Q2" || e.Column.FieldName == "Q3" || e.Column.FieldName == "Q4")
            {
                if (rdbAllocationType.SelectedItem.Text == "Percentage")
                {
                    e.DisplayText = string.Format("{0:F1}%", e.Value);
                }
                else
                {
                    e.DisplayText = string.Format("{0:F1}", e.Value);
                }
            }
        }

        protected void itgGrid_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ASPxGridView gv = (ASPxGridView)sender;
            gv.JSProperties.Add("cpIsCustomCallback", null);
            gv.JSProperties["cpIsCustomCallback"] = e.CallbackName == "CUSTOMCALLBACK";
        }

        protected void itgGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Caption == "Resources")
            {
                DataRow dataRow = itgGrid.GetDataRow(e.VisibleIndex);
                string assignTo = string.Empty;
                DataTable dtfinal = new DataTable();
                string moduleName = Convert.ToString(dataRow[DatabaseObjects.Columns.ModuleName]);
                if (!string.IsNullOrEmpty(moduleName) && !string.IsNullOrEmpty(Convert.ToString(dataRow[DatabaseObjects.Columns.TicketId])) && !string.IsNullOrEmpty(Convert.ToString(dataRow[DatabaseObjects.Columns.TicketActualStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dataRow[DatabaseObjects.Columns.TicketActualCompletionDate])))
                {
                    DataTable dtResources = ITG.Portfolio.GetAllocationData(Convert.ToString(dataRow[DatabaseObjects.Columns.ModuleName]), Convert.ToString(dataRow[DatabaseObjects.Columns.TicketId]), Convert.ToDateTime(dataRow[DatabaseObjects.Columns.TicketActualStartDate]), Convert.ToDateTime(dataRow[DatabaseObjects.Columns.TicketActualCompletionDate]));
                    if (dtResources != null && dtResources.Rows.Count > 0)
                    {
                        DataView dv = dtResources.DefaultView;
                        dv.Sort = DatabaseObjects.Columns.PctPlannedAllocation + " DESC";
                        DataTable sortedResourceDT = dv.ToTable();
                        dtfinal = sortedResourceDT.AsEnumerable().Take(5).CopyToDataTable();
                    }

                    foreach (DataRow row in dtfinal.Rows)
                    {

                        if (!string.IsNullOrEmpty(assignTo))
                            assignTo = assignTo + "; ";
                        assignTo += row[DatabaseObjects.Columns.Resource];

                    }
                }
                e.Cell.Text = assignTo;

            }

        }

        protected void hdnBtnSelectionChanged_Click(object sender, EventArgs e)
        {
            List<object> selectedIDs = itgGrid.GetSelectedFieldValues(DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.Id);
            if (ids != null)
                ids.Clear();
            else
                ids = new List<string[]>();

            if (selectedIDs != null && selectedIDs.Count > 0)
            {
                foreach (object obj in selectedIDs)
                {
                    object[] vals = (object[])obj;
                    ids.Add(new string[] { Convert.ToString(vals[1]), Convert.ToString(vals[0]) });
                }
            }

            BindITGConstraints();
            BindITGMeasures();

            constraintsContainer.Visible = false;
            measuresContainrer.Visible = false;
            if (ids.Count > 0)
            {
                constraintsContainer.Visible = true;
                measuresContainrer.Visible = true;
            }
        }

        protected void itgGrid_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        {
            if (e.Column.FieldName == DatabaseObjects.Columns.ProjectInitiativeLookup)
            {
                string value1 = FieldManagerObj.GetFieldConfigurationData(DatabaseObjects.Columns.ProjectInitiativeLookup, UGITUtility.ObjectToString(e.Value1));
                string value2 = FieldManagerObj.GetFieldConfigurationData(DatabaseObjects.Columns.ProjectInitiativeLookup, UGITUtility.ObjectToString(e.Value2));
                e.Handled = true;
                e.Result = Comparer<string>.Default.Compare(value1, value2);
            }
            else if (e.Column.FieldName == DatabaseObjects.Columns.TicketPriority)
            {
                string value1 = FieldManagerObj.GetFieldConfigurationData(DatabaseObjects.Columns.TicketPriority, UGITUtility.ObjectToString(e.Value1));
                string value2 = FieldManagerObj.GetFieldConfigurationData(DatabaseObjects.Columns.TicketPriority, UGITUtility.ObjectToString(e.Value2));
                e.Handled = true;
                e.Result = Comparer<string>.Default.Compare(value1, value2);
            }
            else if (e.Column.FieldName == DatabaseObjects.Columns.RequestType)
            {
                string value1 = FieldManagerObj.GetFieldConfigurationData(DatabaseObjects.Columns.RequestType, UGITUtility.ObjectToString(e.Value1));
                string value2 = FieldManagerObj.GetFieldConfigurationData(DatabaseObjects.Columns.RequestType, UGITUtility.ObjectToString(e.Value2));
                e.Handled = true;
                e.Result = Comparer<string>.Default.Compare(value1, value2);
            }
            else
                return;
        }

        protected void itgGrid_BeforeGetCallbackResult(object sender, EventArgs e)
        {
           
        }
    }
}

