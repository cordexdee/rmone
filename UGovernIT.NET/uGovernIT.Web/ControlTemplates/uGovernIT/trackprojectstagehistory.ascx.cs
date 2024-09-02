using DevExpress.Web;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DefaultConfig;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class trackprojectstagehistory : UserControl
    {
        DataTable dtresult;
        UserProfile user = null;
        UserProfileManager userManager;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        TicketManager ticketManager = null;
        FieldConfigurationManager fieldManager = null; 

        protected override void OnInit(EventArgs e)
        {
            user = HttpContext.Current.CurrentUser();
            userManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            ticketManager = new TicketManager(context);
            fieldManager = new FieldConfigurationManager(context);

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack && Request["showSingleCheckBox"] != null)
            {
                showSingleCheckBox.Checked = UGITUtility.StringToBoolean(Request["showSingleCheckBox"]);
            }

            gvTrackProjectStageReport.SettingsPopup.HeaderFilter.MinWidth = Unit.Pixel(250);
            gvTrackProjectStageReport.SettingsPopup.HeaderFilter.ResizingMode = ResizingMode.Live;
            GenerateColumns();

            try
            {
                gvTrackProjectStageReport.DataSource = GenerateData();
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
                gvTrackProjectStageReport.DataSource = null;
            }
            finally
            {                
                gvTrackProjectStageReport.DataBind();
            }
        }

        private DataTable GenerateData()
        {

            DataTable dttemp = new DataTable();
            dttemp.Columns.Add(DatabaseObjects.Columns.TaskID, typeof(string));
            dttemp.Columns.Add("ProjectTitle", typeof(string));
            dttemp.Columns.Add(DatabaseObjects.Columns.StageStep, typeof(string));
            dttemp.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            dttemp.Columns.Add(DatabaseObjects.Columns.UGITStartDate, typeof(DateTime));
            dttemp.Columns.Add(DatabaseObjects.Columns.UGITDueDate, typeof(DateTime));
            dttemp.Columns.Add(DatabaseObjects.Columns.Created, typeof(DateTime));
            dttemp.Columns.Add(DatabaseObjects.Columns.Author, typeof(string));

            string query = $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'";

            DataTable projectStageHistory = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectStageHistory, query);

            if (!string.IsNullOrWhiteSpace((Request["publicTicketId"])))
            {
                query = query + $" and {DatabaseObjects.Columns.TicketId} = '{Request["publicTicketId"]}'";
            }
            else if (Request["AllProject"] == "true")
            { }
            else
            {
                dtresult = dttemp;
                return dtresult;
            }

            DataRow[] resultCollection = projectStageHistory.Select(query);
            DataTable listdt = projectStageHistory.Copy();

            if (listdt != null && listdt.Rows.Count > 0)
            {
                //Exclude pmm marked as private
                //if (!UserProfile.IsSuperAdmin(user))
                if(!userManager.IsUGITSuperAdmin(user))
                {
                    DataTable pmmdt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");

                    if (pmmdt != null && pmmdt.Rows.Count > 0)
                    {
                        var commonResultSet = (from ph in listdt.AsEnumerable()
                                               join pmm in pmmdt.AsEnumerable() on ph.Field<string>(DatabaseObjects.Columns.TicketId) equals pmm.Field<string>(DatabaseObjects.Columns.TicketId)
                                               where pmm[DatabaseObjects.Columns.IsPrivate]==DBNull.Value ||  Convert.ToInt32(pmm[DatabaseObjects.Columns.IsPrivate]) != 1
                                               select ph);
                        if (commonResultSet != null && commonResultSet.Count() > 0)
                            listdt = commonResultSet.CopyToDataTable();
                    }
                }

                if (listdt == null || listdt.Rows.Count == 0)
                {
                    dtresult = dttemp;
                    return dtresult;
                }
                DataRow ticket = null;
                foreach (DataRow item in listdt.Rows)
                {
                    DataRow newitem = item;
                    if (showSingleCheckBox.Checked)
                    {
                        DataRow[] drCollection = listdt.Select().Where(x => x.Field<DateTime>(DatabaseObjects.Columns.Created).Date == UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.Created]).Date && x.Field<string>(DatabaseObjects.Columns.TaskID) == Convert.ToString(item[DatabaseObjects.Columns.TaskID])).OrderByDescending(x => x.Field<DateTime>(DatabaseObjects.Columns.Created)).ToArray();
                        bool isMulti = drCollection.Count() > 1;
                        bool isExist = false;
                        if (isMulti)
                            isExist = dttemp.Select().Where(x => x.Field<DateTime>(DatabaseObjects.Columns.Created).Date == UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.Created]).Date && x.Field<string>(DatabaseObjects.Columns.TaskID) == Convert.ToString(item[DatabaseObjects.Columns.TaskID])).ToArray().Count() > 0;
                        if (isExist)
                            continue;
                        else
                        {
                            // Get latest entry, but use start date of earliest entry
                            newitem = drCollection[0];
                            newitem[DatabaseObjects.Columns.UGITStartDate] = drCollection[drCollection.Length - 1][DatabaseObjects.Columns.UGITStartDate];
                        }
                    }
                    DataRow row = dttemp.Rows.Add();
                    row[DatabaseObjects.Columns.TaskID] = item[DatabaseObjects.Columns.TaskID];
                    row[DatabaseObjects.Columns.Created] = UGITUtility.GetDateStringInFormat(UGITUtility.StringToDateTime(newitem[DatabaseObjects.Columns.Created]), true);
                    row[DatabaseObjects.Columns.StageStep] = newitem[DatabaseObjects.Columns.StageStep];
                    row[DatabaseObjects.Columns.Title] = newitem[DatabaseObjects.Columns.Title];
                    row[DatabaseObjects.Columns.UGITStartDate] = UGITUtility.GetDateStringInFormat(UGITUtility.StringToDateTime(newitem[DatabaseObjects.Columns.UGITStartDate]), false);
                    row[DatabaseObjects.Columns.UGITDueDate] = UGITUtility.GetDateStringInFormat(UGITUtility.StringToDateTime(newitem[DatabaseObjects.Columns.UGITEndDate]), false);

                    //row[DatabaseObjects.Columns.Author] = newitem[DatabaseObjects.Columns.ModifiedByUser];
                    row[DatabaseObjects.Columns.Author] = fieldManager.GetFieldConfigurationData(DatabaseObjects.Columns.ModifiedByUser, Convert.ToString(newitem[DatabaseObjects.Columns.ModifiedByUser]));
                    ticket = null;
                    ticket = Ticket.GetCurrentTicket(context, uHelper.getModuleNameByTicketId(newitem[DatabaseObjects.Columns.TicketId].ToString()), newitem[DatabaseObjects.Columns.TicketId].ToString());
                    if (ticket != null)
                        row["ProjectTitle"] = Convert.ToString(newitem[DatabaseObjects.Columns.TicketId]) + ": " + Convert.ToString(ticket[DatabaseObjects.Columns.Title]);
                }
            }

            dtresult = dttemp;
            return dtresult;
        }

        private void GenerateColumns()
        {
            if (gvTrackProjectStageReport.Columns.Count <= 0)
            {
                if (Request["AllProject"] == "true")
                {
                    GridViewDataTextColumn colProjectId = new GridViewDataTextColumn();

                    colProjectId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    colProjectId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                    colProjectId.PropertiesTextEdit.EncodeHtml = true;
                    colProjectId.FieldName = "ProjectTitle";
                    colProjectId.Caption = "Project";
                    colProjectId.HeaderStyle.Font.Bold = true;
                    colProjectId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                    colProjectId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                    colProjectId.Width = new Unit(200);
                    gvTrackProjectStageReport.Columns.Add(colProjectId);
                }

                GridViewDataTextColumn colStageStep = new GridViewDataTextColumn();
                colStageStep.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colStageStep.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colStageStep.PropertiesTextEdit.EncodeHtml = true;
                colStageStep.FieldName = DatabaseObjects.Columns.StageStep;
                colStageStep.Caption = "Stage";
                colStageStep.HeaderStyle.Font.Bold = true;
                colStageStep.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colStageStep.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colStageStep.Width = new Unit(80);
                gvTrackProjectStageReport.Columns.Add(colStageStep);

                GridViewDataTextColumn colTitle = new GridViewDataTextColumn();
                colTitle.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colTitle.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                colTitle.PropertiesTextEdit.EncodeHtml = true;
                colTitle.FieldName = DatabaseObjects.Columns.Title;
                colTitle.Caption = "Task";
                colTitle.HeaderStyle.Font.Bold = true;
                colTitle.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colTitle.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colTitle.Width = new Unit(140);
                gvTrackProjectStageReport.Columns.Add(colTitle);

                GridViewDataDateColumn colStartDate = new GridViewDataDateColumn();
                colStartDate.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colStartDate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colStartDate.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
                colStartDate.FieldName = DatabaseObjects.Columns.UGITStartDate;
                colStartDate.Caption = "Prev Due Date";
                colStartDate.HeaderStyle.Font.Bold = true;
                colStartDate.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colStartDate.SettingsHeaderFilter.Mode = GridHeaderFilterMode.DateRangePicker;
                colStartDate.Width = new Unit(130);
                gvTrackProjectStageReport.Columns.Add(colStartDate);

                GridViewDataDateColumn colEndDate = new GridViewDataDateColumn();
                colEndDate.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colEndDate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colEndDate.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
                colEndDate.FieldName = DatabaseObjects.Columns.UGITDueDate;
                colEndDate.Caption = "Revised Due Date";
                colEndDate.HeaderStyle.Font.Bold = true;
                colEndDate.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colEndDate.SettingsHeaderFilter.Mode = GridHeaderFilterMode.DateRangePicker;
                colEndDate.Width = new Unit(140);
                gvTrackProjectStageReport.Columns.Add(colEndDate);

                GridViewDataDateColumn colCreateDate = new GridViewDataDateColumn();
                colCreateDate.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colCreateDate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colCreateDate.FieldName = DatabaseObjects.Columns.Created;
                colCreateDate.Caption = "Modified On";
                colCreateDate.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy HH:mm:ss}";
                colCreateDate.HeaderStyle.Font.Bold = true;
                colCreateDate.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colCreateDate.SettingsHeaderFilter.Mode = GridHeaderFilterMode.DateRangePicker;
                colCreateDate.Width = new Unit(150);
                gvTrackProjectStageReport.Columns.Add(colCreateDate);

                GridViewDataTextColumn colAuthor = new GridViewDataTextColumn();
                colAuthor.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colAuthor.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colAuthor.PropertiesTextEdit.EncodeHtml = true;
                colAuthor.FieldName = DatabaseObjects.Columns.Author;
                colAuthor.Caption = "Modified By";
                colAuthor.HeaderStyle.Font.Bold = true;
                colAuthor.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colAuthor.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colAuthor.Width = new Unit(180);
                gvTrackProjectStageReport.Columns.Add(colAuthor);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            StringBuilder filterDetailString = new StringBuilder();
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(UGITUtility.GetAbsoluteURL(Request.Url.PathAndQuery + "&showSingleCheckBox=" + showSingleCheckBox.Checked));
            exportURL.Value = urlBuilder.ToString();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (Request["initiateExport"] == "true")
            {
                DataTable filteredDataTable = new DataTable();
                if (dtresult != null && dtresult.Rows.Count > 0 && gvTrackProjectStageReport != null && !string.IsNullOrEmpty(gvTrackProjectStageReport.FilterExpression))
                {
                    try
                    {
                        filteredDataTable = dtresult.Clone();
                        int startVisibleIndex = gvTrackProjectStageReport.VisibleStartIndex;
                        for (int i = startVisibleIndex; i < gvTrackProjectStageReport.VisibleRowCount; i++)
                        {
                            DataRow dataRow = filteredDataTable.NewRow();
                            dataRow = (DataRow)gvTrackProjectStageReport.GetDataRow(i);
                            if (dataRow != null)
                                filteredDataTable.Rows.Add(dataRow.ItemArray);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex, "Error exporting filtered ticket list");
                        filteredDataTable = dtresult.Clone();
                    }
                }
                else if (dtresult != null && gvTrackProjectStageReport != null && gvTrackProjectStageReport.FilterExpression == string.Empty && gvTrackProjectStageReport.VisibleRowCount == 0)
                    filteredDataTable = dtresult.Clone();
                else if (dtresult != null)
                    filteredDataTable = dtresult;

                if (filteredDataTable != null && gvTrackProjectStageReport.SortCount > 0)
                {
                    ReadOnlyCollection<GridViewDataColumn> sortedColumn = gvTrackProjectStageReport.GetSortedColumns();

                    string fieldName = string.Empty;
                    string orderdirection = string.Empty;
                    foreach (GridViewDataColumn item in sortedColumn)
                    {
                        fieldName = item.FieldName;
                        if (item.SortOrder == DevExpress.Data.ColumnSortOrder.Descending)
                            orderdirection = "Desc";
                        else
                            orderdirection = "Asc";
                    }

                    DataView view = filteredDataTable.DefaultView;

                    view.Sort = string.Format("{0} {1}", fieldName, orderdirection);
                    filteredDataTable = view.ToTable();
                }

                if (filteredDataTable != null && filteredDataTable.Rows.Count > 0)
                {
                    filteredDataTable.Columns["ProjectTitle"].ColumnName = "Project";
                    filteredDataTable.Columns[DatabaseObjects.Columns.StageStep].ColumnName = "Stage";
                    filteredDataTable.Columns[DatabaseObjects.Columns.Title].ColumnName = "Task";
                    filteredDataTable.Columns[DatabaseObjects.Columns.UGITStartDate].ColumnName = "Prev Due Date";
                    filteredDataTable.Columns[DatabaseObjects.Columns.UGITDueDate].ColumnName = "Revised Due Date";
                    filteredDataTable.Columns[DatabaseObjects.Columns.Created].ColumnName = "Modified On";
                    filteredDataTable.Columns[DatabaseObjects.Columns.Author].ColumnName = "Modified By";
                }

                string csvData = UGITUtility.ConvertTableToCSV(filteredDataTable);
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

            base.Render(writer);
        }

        protected void showSingleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}

