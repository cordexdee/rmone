using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using System.Data;
using System.Web;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using uGovernIT.Manager;
using DevExpress.Web;
using uGovernIT.Core;
using System.Xml;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class SurveyFeedbackList : UserControl
    {
        //string selectedSurvey = string.Empty;
        string surveyDetailUrl = string.Empty;
        string detailsUrl = string.Empty;
        string sourceUrl = string.Empty;
        private DataTable resultedTable;
        public string strfilterids;
        public string fromdatefilter;
        public string todatefilter;
        public string selectedsurvey;
        public string type;
        public string survey;
        public string reportviewrUrl;
        Services service;
        protected bool isDataBind;
        ServicesManager ServicesManager = new ServicesManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        protected string filterExpression;
        protected override void OnInit(EventArgs e)
        {
            reportviewrUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
            surveyDetailUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=surveyfeedbackdetail");
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            scriptPanel.Visible = false;
            if (!IsPostBack)
                LoadFeedbackSurvey(ddlType.SelectedValue);

            LoadFeedbackData();
        }

        /// <summary>
        /// Loads feedback table from selected module
        /// </summary>
        /// <returns></returns>
        /// private DataTable LoadFeedbackData(string surveyId)
        private DataTable LoadFeedbackData()
        {
            if (string.IsNullOrEmpty(ddlSurvey.SelectedValue))
                return null;
            if (ddlSurvey.Items.Count == 2)
                selectedsurvey = ddlSurvey.Items[1].Value;
            else
                selectedsurvey = ddlSurvey.SelectedValue;

            service = ServicesManager.LoadSurveybySurvey(Convert.ToInt32(selectedsurvey));
            if (service == null)
                return null;
            if (service.ModuleId > 0)
            {
                survey = service.Title;
                type = "Module";
            }
            if (service.ModuleId == 0)
            {
                type = "Generic";
                survey = service.Title;
            }
            
            SurveyFeedbackManager surveyFeedbackManager = new SurveyFeedbackManager(HttpContext.Current.GetManagerContext());
            //DataTable table = surveyFeedbackManager.GetDataTable().Select(string.Format("{0} = {1}", DatabaseObjects.Columns.ServiceTitleLookup, selectedsurvey)).CopyToDataTable(); // SPListHelper.GetDataTable(DatabaseObjects.Lists.SurveyFeedback, sQuery);
            DataTable table = UGITUtility.ToDataTable<SurveyFeedback>(surveyFeedbackManager.Load(x => x.ServiceLookUp ==  Convert.ToInt64(selectedsurvey)));
            //return null if there is no feedback
            if (table == null)
            {
                return null;
            }
            
            //Changes column name of created, userdepartment, userlocation
            if (table.Columns.Contains(DatabaseObjects.Columns.Created))
                table.Columns[DatabaseObjects.Columns.Created].ColumnName = "Submit Date";

            if (table.Columns.Contains(DatabaseObjects.Columns.Author))
                table.Columns[DatabaseObjects.Columns.Author].ColumnName = "Submitted By";

            if (table.Columns.Contains(DatabaseObjects.Columns.TotalRating))
                table.Columns[DatabaseObjects.Columns.TotalRating].ColumnName = "Average Rating";

            DataTable calculatedData = new DataTable();

            List<ServiceQuestion> rQuestions = service.Questions.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.Rating).ToList();
            ServiceQuestion question = null;
            List<string> deleteColumns = new List<string>();
            foreach (DataColumn column in table.Columns)
            {
                question = rQuestions.FirstOrDefault(x => x.TokenName == column.ColumnName);
                if (question != null)
                {

                    column.Caption = question.QuestionTitle;
                    if (!table.Columns.Contains(question.QuestionTitle))
                        column.ColumnName = question.QuestionTitle;
                }
                else if (column.ColumnName.Contains("Rating") && !column.ColumnName.Contains("Average Rating")
                    && (!column.ColumnName.Contains(DatabaseObjects.Columns.ModuleName) || !column.ColumnName.Contains(DatabaseObjects.Columns.ModuleNameLookup)) && !column.ColumnName.Contains(DatabaseObjects.Columns.ID))
                {
                    deleteColumns.Add(column.ColumnName);
                }
            }

            //Remove unwanted columns
            foreach (string column in deleteColumns)
            {
                table.Columns.Remove(table.Columns[column]);
            }

            if (!table.Columns.Contains(DatabaseObjects.Columns.Title))
                table.Columns.Add(DatabaseObjects.Columns.Title);
            if (!table.Columns.Contains(DatabaseObjects.Columns.TicketPRP))
                table.Columns.Add(DatabaseObjects.Columns.TicketPRP);
            if (!table.Columns.Contains(DatabaseObjects.Columns.TicketOwner))
                table.Columns.Add(DatabaseObjects.Columns.TicketOwner);
            if (!table.Columns.Contains(DatabaseObjects.Columns.CategoryName))
                table.Columns.Add(DatabaseObjects.Columns.CategoryName);

            if (!table.Columns.Contains(DatabaseObjects.Columns.UserDepartment))
                table.Columns.Add(DatabaseObjects.Columns.UserDepartment);


            if (!table.Columns.Contains(DatabaseObjects.Columns.UserLocation))
                table.Columns.Add(DatabaseObjects.Columns.UserLocation);

            if (table.Columns.Contains(DatabaseObjects.Columns.Title) && table.Columns.Contains(DatabaseObjects.Columns.TicketId))
            {
                table.Columns[DatabaseObjects.Columns.TicketId].SetOrdinal(0);
                table.Columns[DatabaseObjects.Columns.Title].SetOrdinal(1);
            }

            if (table.Columns.Contains(DatabaseObjects.Columns.TicketId))
                table.Columns[DatabaseObjects.Columns.TicketId].ColumnName = "Ticket ID";

            if (table.Columns.Contains(DatabaseObjects.Columns.TicketPRP))
                table.Columns[DatabaseObjects.Columns.TicketPRP].ColumnName = "PRP";

            if (table.Columns.Contains(DatabaseObjects.Columns.TicketOwner))
                table.Columns[DatabaseObjects.Columns.TicketOwner].ColumnName = "Owner";

            if (table.Columns.Contains(DatabaseObjects.Columns.CategoryName))
                table.Columns[DatabaseObjects.Columns.CategoryName].ColumnName = "Category";

            if (table.Columns.Contains(DatabaseObjects.Columns.UserDepartment))
                table.Columns[DatabaseObjects.Columns.UserDepartment].ColumnName = "Department";

            if (table.Columns.Contains(DatabaseObjects.Columns.UserLocation))
                table.Columns[DatabaseObjects.Columns.UserLocation].ColumnName = "Location";

            if (table.Columns.Contains(DatabaseObjects.Columns.TenantID))
                table.Columns.Remove(DatabaseObjects.Columns.TenantID);

            //if (table.Columns.Contains(DatabaseObjects.Columns.ID))
            //    table.Columns.Remove(DatabaseObjects.Columns.ID);

            if (table.Columns.Contains(DatabaseObjects.Columns.UserDepartment))
                table.Columns.Remove(DatabaseObjects.Columns.UserDepartment);

            if (table.Columns.Contains(DatabaseObjects.Columns.Location))
                table.Columns.Remove(DatabaseObjects.Columns.Location);

            if (table.Columns.Contains(DatabaseObjects.Columns.SLADisabled))
                table.Columns.Remove(DatabaseObjects.Columns.SLADisabled);

            if (table.Columns.Contains(DatabaseObjects.Columns.ModifiedByUser))
                table.Columns.Remove(DatabaseObjects.Columns.ModifiedByUser);

            if (table.Columns.Contains(DatabaseObjects.Columns.Attachments))
                table.Columns.Remove(DatabaseObjects.Columns.Attachments);

            if (table.Columns.Contains(DatabaseObjects.Columns.Deleted))
                table.Columns.Remove(DatabaseObjects.Columns.Deleted);

            resultedTable = table;

            return table;
        }

        protected void ddlSurvey_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedsurvey = ddlSurvey.SelectedIndex > 0 ? ddlSurvey.SelectedValue : string.Empty;
            grid.DataSource = LoadFeedbackData();
            grid.Columns.Clear();
            grid.TotalSummary.Clear();

            GenerateGridColumns();
            grid.DataBind();
            //GetFiletredIds();
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlType.SelectedItem.Text != "All")
                type = ddlType.SelectedItem.Text;

            ClearDateFilter();
            grid.Columns.Clear();
            grid.TotalSummary.Clear();
            resultedTable = null;
            grid.DataBind();
            LoadFeedbackSurvey(ddlType.SelectedValue);

        }
        
        private void LoadFeedbackSurvey(string selectedType)
        {
            List<Services> surveydt = ServicesManager.LoadAllSurveys();
            if (surveydt != null && surveydt.Count > 0)
            {
                if (selectedType == "Generic")
                    surveydt = surveydt.Where(x => x.ModuleNameLookup == "" || x.ModuleNameLookup == null).ToList();
                else if (selectedType == "Module")
                    surveydt = surveydt.Where(x => x.ModuleNameLookup != "" && x.ModuleNameLookup != null).ToList();

                ddlSurvey.Items.Clear();

                if (surveydt != null && surveydt.Count > 0)
                {
                    ddlSurvey.DataSource = surveydt;
                    ddlSurvey.DataTextField = DatabaseObjects.Columns.Title;
                    ddlSurvey.DataValueField = DatabaseObjects.Columns.ID;
                    ddlSurvey.DataBind();
                }

                ddlSurvey.Items.Insert(0, new ListItem("--Select--", "0"));

                if (ddlSurvey.Items.Count == 2 && !isDataBind)
                {
                    isDataBind = true;
                    ddlSurvey.SelectedIndex = ddlSurvey.Items.IndexOf(ddlSurvey.Items.FindByValue(ddlSurvey.Items[1].Value));
                    ddlSurvey_SelectedIndexChanged(null, null);
                }
            }
        }
        
        
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            grid.DataSource = resultedTable;
        }

        protected void grid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if ((e.RowType != GridViewRowType.Data && e.RowType != GridViewRowType.Group) || e.KeyValue == null)
                return;

            if (e.RowType == GridViewRowType.Data)
            {
                DataRow currentRow = grid.GetDataRow(e.VisibleIndex);
                string func = string.Empty;

                string moduleName = string.Empty;
                string ticketID = string.Empty;
                string ID = string.Empty;
                string title = string.Empty;
                if (currentRow == null)
                    return;

                if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup) && Convert.ToString(currentRow[DatabaseObjects.Columns.ModuleNameLookup]) != string.Empty)
                {
                    moduleName = currentRow[DatabaseObjects.Columns.ModuleNameLookup].ToString().Trim();
                }

                if (currentRow.Table.Columns.Contains("Ticket ID") && Convert.ToString(currentRow["Ticket ID"]) != string.Empty)
                {
                    ticketID = currentRow["Ticket ID"].ToString().Trim();
                    if (ticketID.Contains('-'))
                    {
                        string modulename = uHelper.getModuleNameByTicketId(ticketID);
                        UGITModule module = ObjModuleViewManager.GetByName(modulename);  // uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, modulename);
                        detailsUrl = UGITUtility.GetAbsoluteURL(module.DetailPageUrl);
                        sourceUrl = Server.UrlEncode(Request.Url.AbsolutePath);
                        string fun = string.Empty;
                        title = currentRow[DatabaseObjects.Columns.Title].ToString().Trim();
                        string headercaption = string.Format("{0}: {1}", ticketID, title);
                        if (!string.IsNullOrEmpty(detailsUrl))
                        {
                            string width = "90";
                            string height = "90";
                            string urlParams = string.Format("TicketId={0}", ticketID);
                            func = string.Format("event.stopPropagation();window.parent.UgitOpenPopupDialog(\"{0}\",\"{1}\",\"{2}\",\"{4}\",\"{5}\", 0, \"{3}\");", detailsUrl, urlParams, headercaption, sourceUrl, width, height);
                        }

                        GridViewColumn column = grid.Columns["Ticket ID"];
                        GridViewDataTextColumn cln = column as GridViewDataTextColumn;
                        if (column != null && cln != null && cln.GroupIndex == -1)
                        {
                            ((GridViewDataTextColumn)column).PropertiesTextEdit.EncodeHtml = false;

                            e.Row.Cells[grid.GroupCount + column.VisibleIndex].Text = string.Format("<a href='javascript:void(0);' onclick='{1}'>{0}</a>", ticketID, func);
                        }
                    }
                }

                if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Title) && Convert.ToString(currentRow[DatabaseObjects.Columns.Title]) != string.Empty)
                {
                    ticketID = currentRow["Ticket ID"].ToString().Trim();
                    if (ticketID.Contains('-'))
                    {
                        string modulename = uHelper.getModuleNameByTicketId(ticketID);
                        UGITModule module = ObjModuleViewManager.GetByName(modulename); //uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, modulename);
                        detailsUrl = UGITUtility.GetAbsoluteURL(module.DetailPageUrl);
                        sourceUrl = Server.UrlEncode(Request.Url.AbsolutePath);
                        string fun = string.Empty;
                        title = currentRow[DatabaseObjects.Columns.Title].ToString().Trim();
                        string headertitle = string.Format("{0}: {1}", ticketID, title);
                        if (!string.IsNullOrEmpty(detailsUrl))
                        {
                            string width = "90";
                            string height = "90";
                            string urlParams = string.Format("TicketId={0}", ticketID);
                            func = string.Format("event.stopPropagation();window.parent.UgitOpenPopupDialog(\"{0}\",\"{1}\",\"{2}\",\"{4}\",\"{5}\", 0, \"{3}\");", detailsUrl, urlParams, headertitle, sourceUrl, width, height);
                        }

                        GridViewColumn column = grid.Columns[DatabaseObjects.Columns.Title];
                        GridViewDataTextColumn cln = column as GridViewDataTextColumn;
                        if (column != null && cln != null && cln.GroupIndex == -1)
                        {
                            ((GridViewDataTextColumn)column).PropertiesTextEdit.EncodeHtml = false;

                            e.Row.Cells[grid.GroupCount + column.VisibleIndex].Text = string.Format("<a href='javascript:void(0);' onclick='{1}'>{0}</a>", title, func);
                        }
                    }
                }

                if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id) && Convert.ToString(currentRow[DatabaseObjects.Columns.Id]) != string.Empty)
                {
                    ID = currentRow[DatabaseObjects.Columns.Id].ToString().Trim();
                }

                string detailUrl = string.Format("{0}&feedbackid={1}", surveyDetailUrl, ID);
                e.Row.Attributes.Add("onClick", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{1}','70','80')", detailUrl, "Feedback Detail"));
            }
        }

        private void GenerateGridColumns()
        {
            if (grid.Columns.Count == 0 && resultedTable != null && resultedTable.Rows.Count > 0)
            {
                #region "Generate Columns"

                foreach (DataColumn moduleColumn in resultedTable.Columns)
                {
                    if (moduleColumn.ColumnName == DatabaseObjects.Columns.ModuleNameLookup || moduleColumn.ColumnName == DatabaseObjects.Columns.Id || moduleColumn.ColumnName.ToLower() == "description")
                    {
                        continue;
                    }

                    GridViewDataTextColumn colId = null;



                    colId = new GridViewDataTextColumn();
                    colId.FieldName = Convert.ToString(moduleColumn.ColumnName);
                    colId.Caption = Convert.ToString(moduleColumn.ColumnName);
                    colId.HeaderStyle.Font.Bold = true;
                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    colId.PropertiesTextEdit.EncodeHtml = false;
                    colId.FooterCellStyle.Font.Bold = true;
                    if (moduleColumn.ColumnName == "Submit Date" || moduleColumn.ColumnName == "Modified")
                    {
                        colId.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
                    }
                    if (moduleColumn.ColumnName == "Ticket ID")
                    {
                        colId.Width = new Unit(100, UnitType.Pixel);
                        colId.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                    }

                    grid.Columns.Add(colId);
                }

                //Generate grid summary. 
                foreach (DataColumn moduleColumn in resultedTable.Columns)
                {
                    if (moduleColumn.ColumnName == DatabaseObjects.Columns.ModuleNameLookup || moduleColumn.ColumnName == DatabaseObjects.Columns.Id
                        || moduleColumn.ColumnName == "Submit Date" || moduleColumn.ColumnName == "Submitted By" || moduleColumn.ColumnName == "Modified")
                    {
                        continue;
                    }
                    else if (moduleColumn.ColumnName == "Ticket ID")
                    {
                        ASPxSummaryItem sumItem = new ASPxSummaryItem(moduleColumn.ColumnName, DevExpress.Data.SummaryItemType.Average);
                        sumItem.ShowInColumn = moduleColumn.ColumnName;
                        sumItem.FieldName = "Ticket ID";
                        sumItem.DisplayFormat = "Average {0}";
                        grid.TotalSummary.Add(sumItem);
                    }
                    else
                    {
                        ASPxSummaryItem sumItem = new ASPxSummaryItem(moduleColumn.ColumnName, DevExpress.Data.SummaryItemType.Average);
                        sumItem.ShowInColumn = moduleColumn.ColumnName;
                        sumItem.DisplayFormat = "{0:F2}";

                        grid.TotalSummary.Add(sumItem);
                    }
                }
                #endregion
            }
        }

        protected void lnkfilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(dtFrom.Text) && resultedTable != null && resultedTable.Rows.Count > 0 && UGITUtility.IfColumnExists("Submit Date", resultedTable))
            {
                DataRow[] rowColl = resultedTable.Select(string.Format("[{0}]>=#{1}# AND [{0}]<=#{2}#", "Submit Date", dtFrom.Date.ToString("MM/dd/yyyy"), dtTo.Date.AddDays(1).ToString("MM/dd/yyyy")));
                if (rowColl != null)
                {
                    if (rowColl.Length == 0)
                        resultedTable = null;
                    else
                        resultedTable = rowColl.CopyToDataTable();
                    grid.DataBind();
                }
            }

        }

        protected void lnkClear_Click(object sender, EventArgs e)
        {
            ClearDateFilter();
            grid.DataBind();
        }
        protected void ClearDateFilter()
        {
            dtFrom.Text = string.Empty;
            dtTo.Text = string.Empty;
            grid.DataBind();
        }

        protected void rptSurveyFeedback_Click(object sender, ImageClickEventArgs e)
        {
            int startindex = grid.VisibleStartIndex;
            //Applied Filter
            filterExpression = grid.FilterExpression;
            selectedsurvey = ddlSurvey.SelectedValue == "0" ? string.Empty : ddlSurvey.SelectedValue;
            scriptPanel.Visible = true;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected void btnInvokeClick_Click(object sender, EventArgs e)
        {
            grid.DataBind();
            GetFeedBackData();
        }

        private void GetFeedBackData()
        {
            DataTable feedbackdata = new DataTable();
            if (resultedTable != null && resultedTable.Rows.Count > 0 && grid != null && !string.IsNullOrEmpty(grid.FilterExpression))
            {
                try
                {

                    feedbackdata = resultedTable.Clone();
                    int startVisibleIndex = grid.VisibleStartIndex;
                    for (int i = startVisibleIndex; i < grid.VisibleRowCount; i++)
                    {
                        DataRow dataRow = feedbackdata.NewRow();
                        dataRow = (DataRow)grid.GetDataRow(i);
                        if (dataRow != null)
                            feedbackdata.Rows.Add(dataRow.ItemArray);
                    }
                }
                catch (Exception ex)
                {
                    //Log.WriteException(ex, "Error exporting filtered ticket list");
                    feedbackdata = resultedTable.Clone();
                    ULog.WriteException(ex);
                }
            }
            else if (resultedTable != null && grid != null && grid.FilterExpression == string.Empty && grid.VisibleRowCount == 0)
                feedbackdata = resultedTable.Clone();
            else if (resultedTable != null)
                feedbackdata = resultedTable;

            if (feedbackdata == null && feedbackdata.Rows.Count == 0)
                return;

            DataTable newTable = null;
            if (feedbackdata != null && feedbackdata.Rows.Count > 0)
            {
                newTable = GetTableSchemaWithData(feedbackdata);

            }

            if (newTable == null)
                return;
            string csvData = UGITUtility.ConvertTableToCSV(newTable);
            string attachment = string.Format("attachment; filename={0}.csv", "Export");
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "text/csv";
            Response.Write(csvData.ToString());
            Response.Flush();
            Response.End();


        }

        protected void grid_DataBound(object sender, EventArgs e)
        {
            ASPxGridView gridctr = (ASPxGridView)sender;
            foreach (GridViewDataColumn c in gridctr.Columns)
            {
                if (c.FieldName.ToLower() == "location" || c.FieldName.ToLower() == "department" || c.FieldName == DatabaseObjects.Columns.ServiceTitleLookup || c.FieldName.ToLower() == "description")
                {
                    c.Visible = false;
                }
            }
        }

        protected void grid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (e.Item.FieldName.ToLower() == "ticket id")
                e.Text = "Average";
            //string.Format("Sum = {0}", Convert.ToDouble(e.Value) * Convert.ToDouble(ASPxSpinEdit1.Value));
        }

        private DataTable GetTableSchemaWithData(DataTable dt)
        {
            if (service == null)
                return null;

            DataTable returnTable = new DataTable();

            returnTable.Columns.Add("Ticket ID");
            returnTable.Columns.Add(DatabaseObjects.Columns.Title);
            //returnTable.Columns.Add(DatabaseObjects.Columns.Id);

            List<ServiceQuestion> allquestion = service.Questions.Where(x => x.ServiceID == Convert.ToInt32(selectedsurvey)).ToList();


            string detail = Convert.ToString(dt.Rows[0]["Description"]);
            List<ServiceQuestionAnswer> questAnsList = ReturnDeserializeData(detail);
            List<Tuple<string, string>> updatedone = new List<Tuple<string, string>>();

            foreach (ServiceQuestion ans in allquestion)
            {
                int j = 0;
                string question = ans.QuestionTitle;
                for (int i = j; i == j; i++)
                {
                    if (returnTable.Columns.Contains(question))
                    {
                        question = string.Format("{0}_{1}", question, i);
                        j += 1;
                    }
                    else
                    {
                        updatedone.Add(new Tuple<string, string>(question, ans.TokenName));
                        returnTable.Columns.Add(question);
                        break;
                    }
                }
            }

            returnTable.Columns.Add("Submitted By");
            returnTable.Columns.Add("Submit Date");
            returnTable.Columns.Add(DatabaseObjects.Columns.ModuleNameLookup);
            returnTable.Columns.Add("Department");
            returnTable.Columns.Add("Location");
            returnTable.Columns.Add("PRP");
            returnTable.Columns.Add(DatabaseObjects.Columns.Owner);
            returnTable.Columns.Add(DatabaseObjects.Columns.Category);

            foreach (DataRow row in dt.Rows)
            {
                detail = Convert.ToString(row["Description"]);
                questAnsList = ReturnDeserializeData(detail);
                DataRow newrow = returnTable.NewRow();
                foreach (DataColumn column in dt.Columns)
                {
                    if (returnTable.Columns.Contains(column.ColumnName) && column.ColumnName.ToLower() == "submit date")
                    {
                        string submitdate = Convert.ToDateTime(row[column.ColumnName]).Date.ToString("MM/dd/yyyy");
                        newrow[column.ColumnName] = submitdate;
                    }
                    else if (returnTable.Columns.Contains(column.ColumnName))
                    {
                        newrow[column.ColumnName] = row[column.ColumnName];
                    }
                }

                foreach (Tuple<string, string> tupl in updatedone)
                {
                    ServiceQuestionAnswer quesAns = questAnsList.FirstOrDefault(x => x.Token == tupl.Item2);
                    if (quesAns != null)
                        newrow[tupl.Item1] = quesAns.Answer;
                }

                returnTable.Rows.Add(newrow);
            }




            return returnTable;
        }

        private List<ServiceQuestionAnswer> ReturnDeserializeData(string detail)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(detail);
            List<ServiceQuestionAnswer> questAnsList = new List<ServiceQuestionAnswer>();
            questAnsList = (List<ServiceQuestionAnswer>)uHelper.DeSerializeAnObject(xmlDoc, questAnsList);
            return questAnsList;
        }
    }
}

