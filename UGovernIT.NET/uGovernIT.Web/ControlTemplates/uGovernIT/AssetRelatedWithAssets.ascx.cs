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
    public partial class AssetRelatedWithAssets : System.Web.UI.UserControl
    {
        public bool LoadData { get; set; }
        public bool ShowParent { get; set; }
        public bool ShowChildren { get; set; }
        public bool AddChild { get; set; }
        public int ParentLevel { get; set; }
        public int ChildrenLevel { get; set; }
        public bool ParentDetailOnDemand { get; set; }
        public bool ChildDetailOnDemand { get; set; }
        public string TicketId { get; set; }
        public bool ShowDelete { get; set; }
        public string ModuleName { get; set; }
        public int assetId;
        public string moduleUrl;
        public string AssetTagNum { get; set; }
        public bool HideExpendCollapse { get; set; }
        public bool HideRootNode { get; set; }
        UGITModule ugitModule = null;
        List<ModuleColumn> lstModuleColumns;
        private const string absoluteUrlView = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&TicketId={2}&Type={3}&Module={4}&ticketrelation=1";

        private string newParam = "listpicker";
        private string formTitle = "Picker List";
        public bool enablePrint;
        TicketRelationshipHelper objTicketRelationshipHelper;
        ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        //private DataTable openTickets;
        public AssetRelatedWithAssets()
        {
            objTicketRelationshipHelper = new TicketRelationshipHelper(HttpContext.Current.GetManagerContext());
            LoadData = true;
            ShowParent = true;
            ShowChildren = true;
            AddChild = true;
            ParentLevel = -1;
            ChildrenLevel = -1;
            ParentDetailOnDemand = false;
            ChildDetailOnDemand = false;
            TicketId = string.Empty;
            ShowDelete = true;

            ugitModule = moduleViewManager.LoadByName("CMDB");
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            AssetTagNum = Convert.ToString(Request.QueryString["ticketId"]);
            assetId = UGITUtility.StringToInt(Convert.ToString(Request.QueryString["Id"]));
            LoadRelationalTree();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            moduleUrl = UGITUtility.GetAbsoluteURL(ugitModule.DetailPageUrl);
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, newParam, formTitle, AssetTagNum, "AssetRelatedWithAssets", "CMDB"));
            aAddItem.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','95','95',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            //GenerateColumns();
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




            DataTable resultedTable = treeList.DataSource as DataTable;
            selectedColumns = selectedColumns.Where(x => x.Field<bool>(DatabaseObjects.Columns.IsDisplay) == true).OrderBy(x => x.Field<Int32>(DatabaseObjects.Columns.FieldSequence)).ToArray();

            #region Generate Columns for tree list
            treeList.Columns.Clear();
            if (treeList.Columns.Count == 0)
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
                            treeList.Columns.Add(colId);
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
                            treeList.Columns.Add(colId);
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
                            treeList.Columns.Add(colId);
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
                            treeList.Columns.Add(dateTimeColumn);
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

                            treeList.Columns.Add(colId);
                        }
                    }
                }
                #endregion




            }
            #endregion

        }

        public void LoadRelationalTree()
        {
            treeList.GroupBy(treeList.Columns[0], 0);
            pageDetailPanel.Visible = LoadData;
            if (Convert.ToString(Request.QueryString["ticketId"]).Trim() != string.Empty)
            {
                //Added by mudassir 18 march 2020
                DataTable dt = LoadTickets(Convert.ToString(Request.QueryString["ticketId"]).Trim());
                if (dt != null && dt.Rows.Count > 0)
                {
                    ModuleColumnManager objmodulecolumnmanager = new ModuleColumnManager(HttpContext.Current.GetManagerContext());
                    lstModuleColumns = objmodulecolumnmanager.Load($"{DatabaseObjects.Columns.CategoryName}='MyAssets'").Where(x => x.IsDisplay != false).OrderBy(x => x.FieldSequence).ToList();
                    foreach (var col in lstModuleColumns)
                    {
                        GridViewDataColumn gridcolumn = new GridViewDataColumn();
                        gridcolumn.Caption = col.FieldDisplayName;
                        gridcolumn.FieldName = col.FieldName;
                        gridcolumn.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
                        treeList.Columns.Add(gridcolumn);
                    }
                    treeList.DataSource = dt;
                    treeList.DataBind();
                    //treeList.BeginUpdate();
                    //try
                    //{
                    //    treeList.ClearSort();
                    //    treeList.GroupBy(treeList.Columns["ParentId"]);
                    //    treeList.GroupBy(treeList.Columns["TicketType"]);

                    //}
                    //finally
                    //{
                    //    treeList.EndUpdate();
                    //}
                    treeList.ExpandAll();
                }
                else
                {
                    treeList.Visible = false;
                }
            }
        }


        public DataTable LoadTickets(string currentTicketID)
        {
            DataTable dtTickets = new DataTable();
            dtTickets.Columns.Add("ID", typeof(int));
            dtTickets.Columns.Add("ParentId", typeof(int));
            dtTickets.Columns.Add("TicketID", typeof(string));
            dtTickets.Columns.Add("NavigateURL", typeof(string));
            dtTickets.Columns.Add("TicketType", typeof(string));
            dtTickets.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            dtTickets.Columns.Add(DatabaseObjects.Columns.TicketCreationDate, typeof(string));
            dtTickets.Columns.Add(DatabaseObjects.Columns.TicketPriorityLookup, typeof(string));
            dtTickets.Columns.Add(DatabaseObjects.Columns.ModuleStepLookup, typeof(string));
            ModuleColumnManager objmodulecolumnmanager = new ModuleColumnManager(HttpContext.Current.GetManagerContext());
            lstModuleColumns = objmodulecolumnmanager.Load($"{DatabaseObjects.Columns.CategoryName}='MyAssets'").OrderBy(x => x.FieldSequence).ToList();
            foreach (var col in lstModuleColumns)
            {
                dtTickets.Columns.Add(col.FieldName, typeof(string));
            }
            DataTable relationshipTicketList_dt = objTicketRelationshipHelper.GetTicketRelationshipByID(currentTicketID);
            if (relationshipTicketList_dt != null)
            {
                //Added by mudassir 13 march 2020
                //bool hasParentItem = false, hasChildItems = false, hasSiblingItems = false;
                var tickets = from p in relationshipTicketList_dt.AsEnumerable()
                              where ((!p.IsNull(DatabaseObjects.Columns.ParentTicketId) && p.Field<string>(DatabaseObjects.Columns.ParentTicketId).Equals(currentTicketID)) ||
                                     (!p.IsNull(DatabaseObjects.Columns.ChildTicketId) && p.Field<string>(DatabaseObjects.Columns.ChildTicketId) == currentTicketID))
                              select new
                              {
                                  TicketType = p.Field<string>(DatabaseObjects.Columns.ParentTicketId).Equals(currentTicketID) ? "C" : "P",
                                  TicketID = p.Field<string>(DatabaseObjects.Columns.ParentTicketId).Equals(currentTicketID) ?
                                             p.Field<string>(DatabaseObjects.Columns.ChildTicketId) : p.Field<string>(DatabaseObjects.Columns.ParentTicketId)
                              };

                int id = 3;
                tickets = tickets.OrderByDescending(x => x.TicketType);
                foreach (var t in tickets.Distinct())
                {
                    if (t.TicketType.Equals("P"))
                    {
                        dtTickets.Rows.Add(id++, 1, t.TicketID, objTicketRelationshipHelper.CreateNavigateURL(t.TicketID), "Parent Item(s)");
                        //hasParentItem = true;
                    }
                    else if (t.TicketType.Equals("C"))
                    {
                        dtTickets.Rows.Add(id++, 3, t.TicketID, objTicketRelationshipHelper.CreateNavigateURL(t.TicketID), "Child Item(s)");
                        //hasChildItems = true;
                    }

                }
                if (dtTickets != null && dtTickets.Rows.Count > 0)
                {
                    var groupByParentTicketId = dtTickets.AsEnumerable().Where(x => !x.IsNull("TicketType") && x.Field<string>("TicketType") == "Parent Item(s)").GroupBy(x => x.Field<string>("TicketID"));
                    foreach (var parent in groupByParentTicketId)
                    {
                        DataRow[] siblings = relationshipTicketList_dt.AsEnumerable().Where(x => !string.IsNullOrEmpty(Convert.ToString(x[DatabaseObjects.Columns.ParentTicketId])) && Convert.ToString(x[DatabaseObjects.Columns.ParentTicketId]) == parent.Key &&
                                                                                               !string.IsNullOrEmpty(Convert.ToString(x[DatabaseObjects.Columns.ChildTicketId])) && Convert.ToString(x[DatabaseObjects.Columns.ChildTicketId]) != currentTicketID &&
                                                                                               !string.IsNullOrEmpty(Convert.ToString(x[DatabaseObjects.Columns.ParentModuleName])) && Convert.ToString(x[DatabaseObjects.Columns.ParentModuleName]) != ModuleNames.WIKI).ToArray();
                        if (siblings == null || siblings.Length == 0)
                            continue;
                        foreach (DataRow dr in siblings)
                        {
                            string ticketId = Convert.ToString(dr[DatabaseObjects.Columns.ChildTicketId]);
                            dtTickets.Rows.Add(id++, 2, ticketId, objTicketRelationshipHelper.CreateNavigateURL(ticketId), "sChild Item(s)", true);
                            //dtTickets.Rows.Add(id++, 2, ticketId, objTicketRelationshipHelper.CreateNavigateURL(ticketId), "sChild Item(s)", hasSiblingItems = true);
                            //hasSiblingItems = true;
                        }
                    }
                }
                //
                foreach (DataRow dr in dtTickets.Rows)
                {
                    DataRow table = objTicketRelationshipHelper.GetFilteredAssetDetail(Convert.ToString(dr["TicketID"])).Select("TicketID='" + Convert.ToString(dr["TicketID"]) + "'")[0];
                    if (table != null && (table.Table).Rows.Count > 0)
                    {
                        List<HistoryEntry> datalist;
                        foreach (DataColumn col in table.Table.Columns)
                        {
                            if (dr.Table.Columns.Contains(col.ColumnName))
                            {
                                if (col.ColumnName == DatabaseObjects.Columns.Comment)
                                {
                                    datalist = uHelper.GetHistorywithusername(table, DatabaseObjects.Columns.TicketComment, true);
                                    dr[col.ColumnName] = uHelper.GetComments(datalist);
                                }
                                else
                                {
                                    dr[col.ColumnName] = table[col.ColumnName];
                                }

                            }
                            else
                            {




                            }
                        }
                        //dr[DatabaseObjects.Columns.Title] = table[DatabaseObjects.Columns.AssetName];
                        //var moduleName = Convert.ToString(dr["TicketID"]).Split('-')[0];
                        //if (!(moduleName.ToUpper() == ModuleNames.CMDB))
                        //{
                        //    dr[DatabaseObjects.Columns.TicketCreationDate] = table[DatabaseObjects.Columns.TicketCreationDate];
                        //    if ((table.Table).Columns.Contains(DatabaseObjects.Columns.TicketPriorityLookup))
                        //        dr[DatabaseObjects.Columns.TicketPriorityLookup] = table[DatabaseObjects.Columns.TicketPriorityLookup];

                        //}
                        // dr[DatabaseObjects.Columns.ModuleStepLookup] = table[DatabaseObjects.Columns.ModuleStepLookup];
                    }
                }
                if (tickets.Count() == 0)
                {
                    dtTickets.Clear();
                }
            }
            return dtTickets;
        }

        private string LoadNodeDetail(XmlElement element)
        {
            StringBuilder details = new StringBuilder();

            if (element.Attributes[DatabaseObjects.Columns.Title] != null)
            {
                details.Append("&nbsp;(");
                string value = element.GetAttribute(DatabaseObjects.Columns.Title);
                value = value.Length > 50 ? value.Remove(50) + "..." : value;
                details.Append(value.Trim());
                details.Append(")&nbsp;");
            }


            if (element.Attributes[DatabaseObjects.Columns.TicketCreationDate] != null)
            {
                try
                {
                    string value = element.GetAttribute(DatabaseObjects.Columns.TicketCreationDate);
                    DateTime creationDate = DateTime.Parse(value.Trim());
                    details.Append("created <strong>");
                    details.Append(creationDate.ToString("MMM-dd-yyyy"));
                    details.Append(" </strong>");
                }
                catch (Exception)
                {
                }
            }

            if (element.Attributes[DatabaseObjects.Columns.TicketPriorityLookup] != null)
            {
                details.Append("has priority <strong>");
                string value = element.GetAttribute(DatabaseObjects.Columns.TicketPriorityLookup);
                details.Append(value);
                details.Append(" </strong>");
            }

            if (element.Attributes[DatabaseObjects.Columns.ModuleStepLookup] != null)
            {
                if (element.Attributes[DatabaseObjects.Columns.TicketPriorityLookup] == null)
                {
                    details.Append("has ");
                }
                else
                {
                    details.Append("and ");
                }

                details.Append("status <Strong>");
                string value = element.GetAttribute(DatabaseObjects.Columns.ModuleStepLookup);
                details.Append(value);
                details.Append("&nbsp;</strong>");

            }
            return details.ToString();
        }
        private string GetDeleteButton(string childTicketId)
        {
            string deleteHtml = string.Format("<span class='' onmousedown='OverOnDeleteButton(this)' onmouseout='OverOutOnDeleteButton(this)'><input type='image'  class='deleteimg'  alt='*' src='/Content/images/uGovernIT/TrashIcon.png' class='' onclick='event.cancelBubble = true;ComfrimDelete(this,\"{0}\")' /></span>", childTicketId);
            return deleteHtml;
        }

        private string CreateNavigateURLForNode(XmlElement xmlElement, string ticketId)
        {
            string navigationUrl = "javascript:";
            if (ticketId != null & ticketId.Trim() != string.Empty)
            {
                string url = string.Empty;
                if (xmlElement.Attributes[DatabaseObjects.Columns.ModuleRelativePagePath] != null)
                {
                    url = xmlElement.GetAttribute(DatabaseObjects.Columns.ModuleRelativePagePath);
                }
                url = UGITUtility.GetAbsoluteURL(url);
                navigationUrl = string.Format("javascript:(window.parent) ? window.parent.UgitOpenPopupDialog(\"{0}\",\"TicketId={1}\",\"{2} Ticket:{1}\",\"auto\",\"auto\") : UgitOpenPopupDialog(\"{0}\",\"TicketId={1}\",\"{2} Ticket: {1}\",\"auto\",\"auto\")", url, ticketId, xmlElement.GetAttribute(DatabaseObjects.Columns.ModuleName));
            }
            return navigationUrl;
        }
        protected void DdlModuleDetail_Load()
        {
            ddlModuleDetail.Items.Clear();

            List<UGITModule> ugitModule = moduleViewManager.Load($"{DatabaseObjects.Columns.ModuleType}='{((int)ModuleType.SMS)}'").ToList();


            if (ugitModule == null)
                return;

            foreach (UGITModule module in ugitModule)
            {
                // Condition for excluding SVC
                if (Convert.ToString(module.ModuleName) != "SVC" && UGITUtility.StringToBoolean(module.EnableModule))
                {
                    ListItem li = new ListItem(module.Title.ToString(), module.ModuleName.ToString());
                    string source = string.Format("&source={0}", Guid.NewGuid().ToString());
                    string url = UGITUtility.GetAbsoluteURL(Convert.ToString(module.StaticModulePagePath));

                    li.Attributes.Add("Url", url);
                    ddlModuleDetail.Items.Add(li);
                }
            }

            int nprIndex = ddlModuleDetail.Items.IndexOf(ddlModuleDetail.Items.FindByValue("NPR"));
            if (nprIndex == -1)
            {

                UGITModule nprModule = moduleViewManager.LoadByName("NPR");
                if (nprModule != null && UGITUtility.StringToBoolean(nprModule.EnableModule))
                {
                    ListItem li = new ListItem(nprModule.Title, nprModule.ModuleName);
                    string source = string.Format("&source={0}", Guid.NewGuid().ToString());
                    string url = UGITUtility.GetAbsoluteURL(nprModule.ModuleRelativePagePath);
                    li.Attributes.Add("Url", url);
                    ddlModuleDetail.Items.Add(li);
                }
            }
            ddlModuleDetail.Items.Insert(0, new ListItem("Select Module", "0"));
        }
        protected void BtTicketDelete_Click(object sender, EventArgs e)
        {
            string childTicketId = hfTicketDelete.Value.Trim();

            ASPxButton btn = sender as ASPxButton;
            if (string.IsNullOrWhiteSpace(childTicketId))
            {
                childTicketId = btn.CommandArgument;
            }

            if (!string.IsNullOrEmpty(childTicketId.Trim()) && objTicketRelationshipHelper.IsRelationExist(this.AssetTagNum, childTicketId))
            {
                int rowEffected = objTicketRelationshipHelper.DeleteRelation(this.AssetTagNum, childTicketId);
                if (rowEffected > 0)
                {
                    LoadRelationalTree();
                    ddlModuleDetail.ClearSelection();
                }
            }
        }
        protected void treeList_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.TicketCreationDate)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketCreationDate))))
                    e.Cell.Text = DateTime.Parse((string)e.GetValue(DatabaseObjects.Columns.TicketCreationDate)).ToString("MMM-dd-yyyy");
            }
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.ModuleStepLookup)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(e.GetValue(DatabaseObjects.Columns.ModuleStepLookup))))
                {
                    DataRow[] rowItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, DatabaseObjects.Columns.ID + "=" + e.GetValue(DatabaseObjects.Columns.ModuleStepLookup)).Select();
                    if (rowItem != null && rowItem.Count() > 0)
                    {
                        string status = Convert.ToString(rowItem[0][DatabaseObjects.Columns.Name]);
                        e.Cell.Text = status;
                    }
                }
            }
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.TicketRequestTypeLookup)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketRequestTypeLookup))))
                {
                    DataRow[] rowItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, DatabaseObjects.Columns.ID + "=" + e.GetValue(DatabaseObjects.Columns.TicketRequestTypeLookup)).Select();
                    if (rowItem != null && rowItem.Count() > 0)
                    {
                        string status = Convert.ToString(rowItem[0][DatabaseObjects.Columns.Title]);
                        e.Cell.Text = status;
                    }
                }
            }
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.AssetModelLookup)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(e.GetValue(DatabaseObjects.Columns.AssetModelLookup))))
                {
                    DataRow[] rowItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AssetModels, DatabaseObjects.Columns.ID + "=" + e.GetValue(DatabaseObjects.Columns.AssetModelLookup)).Select();
                    if (rowItem != null && rowItem.Count() > 0)
                    {
                        string status = Convert.ToString(rowItem[0][DatabaseObjects.Columns.Title]);
                        e.Cell.Text = status;
                    }
                }
            }
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.Owner)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(e.GetValue(DatabaseObjects.Columns.Owner))))
                {
                    string id = Convert.ToString(e.GetValue(DatabaseObjects.Columns.Owner));
                    DataRow[] rowItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AspNetUsers, DatabaseObjects.Columns.ID + "=" + "'" + id + "'").Select();
                    if (rowItem != null && rowItem.Count() > 0)
                    {
                        string status = Convert.ToString(rowItem[0][DatabaseObjects.Columns.Name]);
                        e.Cell.Text = status;
                    }
                }
            }

        }

        protected void UpdateButton_Click(object sender, EventArgs e)
        {

            if (ddlModuleDetail.SelectedValue != null && ddlModuleDetail.SelectedValue != string.Empty)
            {
                UGITModule module = moduleViewManager.LoadByName(ddlModuleDetail.SelectedValue);
                string url = module.StaticModulePagePath;
                url = string.Format("{1}?TicketId=0&ParentId={0}&NewSubticket=true&SourceTicketId={0}", TicketId, url);
                uHelper.ClosePopUpAndEndResponse(Context, true);
                UGITUtility.OpenPopup(Context, url, string.Format("New {0} Ticket", ddlModuleDetail.SelectedValue), false);
            }
        }
    }
}