using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Xml;
using System.Text;
using DevExpress.Web;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using DevExpress.Web.ASPxTreeList;

namespace uGovernIT.Web
{
    public partial class AssetRelatedWithTickets : System.Web.UI.UserControl
    {
        public bool LoadData { get; set; }
        public bool ShowChildren { get; set; }
        public bool AddChild { get; set; }
        public bool ChildDetailOnDemand { get; set; }
        public string AssetTagNum { get; set; }
        public int AssetTagId { get; set; }
        public bool ShowDelete { get; set; }

        string currentAssetTagNum;
        int assetId;
        private string newParam = "listpicker";
        private string formTitle = "Picker List";
        private const string absoluteUrlView = "layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&TicketId={2}&Type={3}&AssetId={4}";
        TicketRelationshipHelper objTicketRelationshipHelper;

        public AssetRelatedWithTickets()
        {
            LoadData = true;
            ShowChildren = true;
            AddChild = true;
            ChildDetailOnDemand = false;
            AssetTagNum = string.Empty;
            ShowDelete = true;
            objTicketRelationshipHelper = new TicketRelationshipHelper(HttpContext.Current.GetManagerContext());
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            AssetTagNum = Convert.ToString(Request.QueryString["ticketId"]);
            AssetTagId = UGITUtility.StringToInt(Request.QueryString["Id"]);
            currentAssetTagNum = AssetTagNum;
            assetId = AssetTagId;
            LoadRelationalTree();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, newParam, formTitle, AssetTagNum, "AssetRelatedWithTickets", assetId));
            aAddItem.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','95','95',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            //GenerateColumns();
        }
        protected void LoadRelationalTree()
        {
            DataTable table = objTicketRelationshipHelper.GetRelatedTicketsDetails(assetId, HttpContext.Current.GetManagerContext());
            grdAssetRelatedTicket.DataSource = table;
            grdAssetRelatedTicket.DataBind();
        }
        private void GenerateColumns()
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();

            DataTable columns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");

            DataRow[] selectedColumns = columns.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.CategoryName) == "CMDB").ToArray();



            ModuleColumnManager moduleColumnManager = new ModuleColumnManager(HttpContext.Current.GetManagerContext());
            DataRow[] selectedColumns1 = moduleColumnManager.GetDataTable().Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CategoryName, "CMDB"));
            if (selectedColumns.Length <= 0)
            {
                return;
            }

            DataTable resultedTable = grdAssetRelatedTicket.DataSource as DataTable;
            selectedColumns = selectedColumns.Where(x => x.Field<bool>(DatabaseObjects.Columns.IsDisplay) == true).OrderBy(x => x.Field<Int32>(DatabaseObjects.Columns.FieldSequence)).ToArray();

            #region Generate Columns for tree list
            grdAssetRelatedTicket.Columns.Clear();
            if (grdAssetRelatedTicket.Columns.Count == 0)
            {
                #region "Generate Columns"

                foreach (DataRow moduleColumn in selectedColumns)
                {
                    bool textAlignmentValueExists = false;
                    HorizontalAlign alignment = HorizontalAlign.Center;
                    if (moduleColumn.Table.Columns.Contains(DatabaseObjects.Columns.TextAlignment) &&
                        !string.IsNullOrEmpty(Convert.ToString(moduleColumn[DatabaseObjects.Columns.TextAlignment])))
                    {
                        textAlignmentValueExists = true;
                        alignment = (HorizontalAlign)Enum.Parse(typeof(HorizontalAlign), Convert.ToString(moduleColumn[DatabaseObjects.Columns.TextAlignment]));
                    }

                    string fieldName = Convert.ToString(moduleColumn["FieldName"]);
                    if (fieldName == DatabaseObjects.Columns.Attachments)
                        continue;
                    if (resultedTable != null && resultedTable.Columns.Contains(fieldName))
                    {
                        Type dataType = typeof(string);
                        if (resultedTable.Columns.Contains(fieldName))
                            dataType = resultedTable.Columns[fieldName].DataType;

                        TreeListDataColumn colId = null;
                        TreeListDateTimeColumn dateTimeColumn = null;

                        if (fieldName.ToLower() == DatabaseObjects.Columns.TicketStatus.ToLower())
                        {
                            colId = new TreeListDataColumn();

                            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.FieldName = fieldName;
                            colId.Caption = Convert.ToString(moduleColumn["FieldDisplayName"]);
                            colId.HeaderStyle.Font.Bold = true;
                            colId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketStatus);
                            grdAssetRelatedTicket.Columns.Add(colId);
                        }
                        else if (fieldName.ToLower() == DatabaseObjects.Columns.TicketAge.ToLower())
                        {
                            colId = new TreeListDataColumn();
                            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.FieldName = fieldName;
                            colId.Caption = Convert.ToString(moduleColumn["FieldDisplayName"]);
                            colId.HeaderStyle.Font.Bold = true;
                            colId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketAge);
                            grdAssetRelatedTicket.Columns.Add(colId);
                        }
                        else if (fieldName.ToLower() == DatabaseObjects.Columns.TicketId.ToLower())
                        {
                            colId = new TreeListDataColumn();

                            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.PropertiesEdit.EncodeHtml = false;
                            colId.FieldName = fieldName;
                            colId.Caption = Convert.ToString(moduleColumn["FieldDisplayName"]);
                            colId.HeaderStyle.Font.Bold = true;
                            colId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketAge);
                            grdAssetRelatedTicket.Columns.Add(colId);
                        }
                        else if (dataType == typeof(DateTime))
                        {
                            dateTimeColumn = new TreeListDateTimeColumn();
                            dateTimeColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                            dateTimeColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

                            dateTimeColumn.FieldName = fieldName;
                            dateTimeColumn.Caption = Convert.ToString(moduleColumn["FieldDisplayName"]);
                            dateTimeColumn.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
                            dateTimeColumn.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                            dateTimeColumn.HeaderStyle.Font.Bold = true;
                            dateTimeColumn.Width = GetColumnWidth("DateTime");
                            grdAssetRelatedTicket.Columns.Add(dateTimeColumn);
                        }
                        else
                        {
                            colId = new TreeListDataColumn();
                            colId.FieldName = fieldName;
                            colId.Caption = Convert.ToString(moduleColumn["FieldDisplayName"]);

                            if (fieldName.ToLower() == DatabaseObjects.Columns.Title.ToLower())
                            {
                                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                            }
                            else if (fieldName.ToLower() == DatabaseObjects.Columns.TicketId.ToLower())
                            {
                                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                colId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketId);
                                colId.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                            }
                            else
                            {
                                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                if (fieldName.ToLower() == DatabaseObjects.Columns.TicketPriority.ToLower() ||
                                    fieldName.ToLower() == DatabaseObjects.Columns.TicketPriorityLookup.ToLower())
                                {
                                    colId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketPriorityLookup);
                                }
                            }



                            if (dataType == typeof(double) || dataType == typeof(float) || dataType == typeof(decimal))
                                colId.PropertiesEdit.DisplayFormatString = "#.##";

                            colId.HeaderStyle.Font.Bold = true;

                            if (textAlignmentValueExists)
                            {
                                colId.HeaderStyle.HorizontalAlign = alignment;
                                colId.CellStyle.HorizontalAlign = alignment;
                            }

                            grdAssetRelatedTicket.Columns.Add(colId);
                        }
                    }
                }
                #endregion




            }
            #endregion

        }
        private Unit GetColumnWidth(string columnName)
        {
            Unit width = new Unit("30px");

            if (columnName == DatabaseObjects.Columns.TicketPriority || columnName == DatabaseObjects.Columns.TicketPriorityLookup)
                width = new Unit("60px");
            else if (columnName == DatabaseObjects.Columns.TicketId)
                width = new Unit("105px");
            else if (columnName == "DateTime")
                width = new Unit("105px");
            else if (columnName == DatabaseObjects.Columns.TicketStatus)
                width = new Unit("125px");
            else if (columnName == DatabaseObjects.Columns.TicketAge)
                width = new Unit("50px");
            else if (columnName == DatabaseObjects.Columns.ProjectHealth || columnName == DatabaseObjects.Columns.ProjectRank)
                width = new Unit("30px");
            else if (columnName == DatabaseObjects.Columns.SelfAssign)
                width = new Unit("20px");
            else if (columnName == DatabaseObjects.Columns.Attachments)
                width = new Unit("8px");
            else if (columnName == "CheckBox")
                width = new Unit("20px");
            // else use default width from above

            return width;
        }
        //Delete related ticket
        protected void grdAssetRelatedTicket_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            if (e.Keys.Count <= 0)
                return;
            string TicketId = Convert.ToString(e.Keys[DatabaseObjects.Columns.TicketId]);
            //if (TicketId != string.Empty && objTicketRelationshipHelper.IsRelationExist(assetId, TicketId))
            //{
            //    //int rowEffected = objTicketRelationshipHelper.DeteleRelation(this.AssetTagNum, TicketId);
            //    //if (rowEffected > 0)
            //    //{
            //    //    //Asset History
            //    //    objTicketRelationshipHelper.DeletedHistory(this.AssetTagNum, TicketId);
            //    //    LoadRelationalTree();
            //    //}
            //}
            e.Cancel = true;
        }

        //Here we can display data as per need row wise.
        protected void grdAssetRelatedTicket_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.TicketCreationDate)
            {
                DateTime creationDate = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.TicketCreationDate));
                //e.Cell.Text = uHelper.GetDateStringInFormat(context, creationDate, false, true);
            }
            else if (e.DataColumn.FieldName == DatabaseObjects.Columns.TicketId)
            {
                //string navigationUrl = string.Empty;
                //string url = Ticket.GenerateTicketURL(Convert.ToString(e.CellValue), context);
                //navigationUrl = string.Format("javascript:(window.parent) ? window.parent.UgitOpenPopupDialog(\"{0}\",\"\",\" {1}\",\"auto\",\"auto\") : UgitOpenPopupDialog(\"{0}\",\"\",\" {1}\",\"auto\",\"auto\")", url, Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketId)));
                //e.Cell.Text = string.Format("<a href='{0}'>{1}</a>", navigationUrl, Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketId)));
            }
        }
    }
}