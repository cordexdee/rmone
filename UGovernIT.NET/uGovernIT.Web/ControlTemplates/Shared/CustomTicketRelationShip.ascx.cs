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

namespace uGovernIT.Web.ControlTemplates.Shared
{
    public partial class CustomTicketRelationShip : UserControl
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

        public bool HideExpendCollapse { get; set; }
        public bool HideRootNode { get; set; }
        private const string absoluteUrlView = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&TicketId={2}&Type={3}&Module={4}&ticketrelation=1&TabId={5}";

        private string newParam = "listpicker";
        private string formTitle = "Picker List";
        private string TabId = "1";
        public bool enablePrint;

        ModulePrioirty priority = null;

        TicketRelationshipHelper objTicketRelationshipHelper;
        ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        private PrioirtyViewManager prioirtyMGR = new PrioirtyViewManager(HttpContext.Current.GetManagerContext());

        public CustomTicketRelationShip()
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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Request["TabId"]) != null)
                            TabId = Convert.ToString(Request["TabId"]);

            if (uHelper.getModuleNameByTicketId(TicketId) == "CON")
                aAddNewSubTicket.Visible = false;

            if (Request["enablePrint"] != null && Convert.ToBoolean(Request["enablePrint"]))
            {
                aAddItem.Visible = false;
                aAddNewSubTicket.Visible = false;
                enablePrint = Convert.ToBoolean(Request["enablePrint"]);
            }
            if (Request["Type"] != null && Convert.ToString(Request["Type"]) == "SubTicket")
            {
                dvModule.Visible = true;
                pageDetailPanel.Visible = false;
                if (!IsPostBack)
                    DdlModuleDetail_Load();
            }
            else
            {
                dvModule.Visible = false;
                pageDetailPanel.Visible = true;
                LoadRelationalTree();
                string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, newParam, formTitle, TicketId, "TicketRelation", ModuleName, TabId));
                string parentctrl = UGITUtility.ObjectToString(Request["ctrl"]);
                if(parentctrl == "PMM.ProjectCompactView")
                    aAddItem.Attributes.Add("href", string.Format("javascript:(window.parent) ? UgitOpenPopupDialog('{0}','','{2}','95','95',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle)+":"+string.Format("UgitOpenPopupDialog('{0}', '', '{2}', '95', '95', 0, '{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
                else
                    aAddItem.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','95','95',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

                url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, "customticketrelationship", "Add New Sub Item", TicketId, "SubTicket", ModuleName, TabId));
                aAddNewSubTicket.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','300px','200px',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), "Add New Sub Item"));
            }
        }

        public void LoadRelationalTree()
        {
            DataRow currentTicket = Ticket.GetCurrentTicket(HttpContext.Current.GetManagerContext(), uHelper.getModuleNameByTicketId(TicketId), TicketId);
            if (currentTicket == null)
                return;
            bool isActionUser = Ticket.IsActionUser(HttpContext.Current.GetManagerContext(),currentTicket, HttpContext.Current.GetManagerContext().CurrentUser);
            bool isDataEditor = Ticket.IsDataEditor(currentTicket, HttpContext.Current.GetManagerContext());
            pageDetailPanel.Visible = LoadData;
            if (!string.IsNullOrEmpty(TicketId))
            {
                DataTable dt = objTicketRelationshipHelper.LoadTickets(TicketId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    treeListRT.DataSource = dt;
                    treeListRT.DataBind();
                    treeListRT.ExpandAll();
                }
                else
                {
                    treeListRT.Visible = false;
                }
            }

            if (isActionUser || isDataEditor || HttpContext.Current.GetManagerContext().UserManager.IsUGITSuperAdmin(HttpContext.Current.GetManagerContext().CurrentUser))
            {
                dvAddNewRelation.Visible = true;
                if (treeListRT.Visible)
                    treeListRT.Columns["Delete"].Visible = true;
            }
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
            ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
            ddlModuleDetail.Items.Clear();
            DataTable modules = moduleViewManager.LoadAllModules();
            //DataTable moduleCollection = uHelper.GetModuleList(ModuleType.SMS);
            DataTable modulesColumns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}'");   //uGITCache.ModuleConfigCache.LoadModuleListByName("",DatabaseObjects.Tables.ModuleColumns);   //GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns);    //uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleColumns);
            if (modules != null && modulesColumns != null && modulesColumns.Rows.Count > 0)
            {
                DataView dtView = new DataView(modulesColumns);
                List<string> distinctDt = dtView.ToTable(true, DatabaseObjects.Columns.CategoryName).AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.CategoryName)).ToList();
                distinctDt.Remove(ModuleNames.EDM);
                distinctDt.Remove(ModuleNames.CMDB);
                distinctDt.Remove(ModuleNames.WIK);
                //distinctDt.Remove(ModuleNames.PMM);
                distinctDt.Remove(ModuleNames.APP);
                distinctDt.Remove(ModuleNames.CMT);
                distinctDt.Remove(ModuleNames.SVC);
                distinctDt.Remove(ModuleNames.TSK);
                distinctDt.Remove(ModuleNames.WIKI);
                distinctDt.Sort();
                bool enableModule = true;
                DataRow[] moduleRows = modules.Select(DatabaseObjects.Columns.EnableModule + "=" + enableModule);
                foreach (string moduleName in distinctDt)
                {
                    DataRow row = moduleRows.Where(x => x.Field<string>(DatabaseObjects.Columns.ModuleName) == moduleName).FirstOrDefault();
                    if (row != null)
                    {
                        ddlModuleDetail.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.Title]), Convert.ToString(row[DatabaseObjects.Columns.ModuleName])));
                    }
                }
            }
            ////List<UGITModule> ugitModule = moduleViewManager.Load($"{DatabaseObjects.Columns.ModuleType}='{((int)ModuleType.SMS)}'").ToList();
            //////DataTable moduleCollection = UGITUtility.ToDataTable(UgitModule);

            ////if (ugitModule == null)
            ////    return;

            ////foreach (UGITModule module in ugitModule)
            ////{
            ////    // Condition for excluding SVC
            ////    if (Convert.ToString(module.ModuleName) != "SVC" && UGITUtility.StringToBoolean(module.EnableModule))
            ////    {
            ////        ListItem li = new ListItem(module.Title.ToString(), module.ModuleName.ToString());
            ////        string source = string.Format("&source={0}", Guid.NewGuid().ToString());
            ////        string url = UGITUtility.GetAbsoluteURL(Convert.ToString(module.StaticModulePagePath));

            ////        li.Attributes.Add("Url", url);
            ////        ddlModuleDetail.Items.Add(li);
            ////    }
            ////}

            ////int nprIndex = ddlModuleDetail.Items.IndexOf(ddlModuleDetail.Items.FindByValue("NPR"));
            ////if (nprIndex == -1)
            ////{
            ////    //ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            ////    UGITModule nprModule = moduleViewManager.LoadByName("NPR");
            ////    if (nprModule != null && UGITUtility.StringToBoolean(nprModule.EnableModule))
            ////    {
            ////        ListItem li = new ListItem(nprModule.Title, nprModule.ModuleName);
            ////        string source = string.Format("&source={0}", Guid.NewGuid().ToString());
            ////        string url = UGITUtility.GetAbsoluteURL(nprModule.ModuleRelativePagePath);
            ////        li.Attributes.Add("Url", url);
            ////        ddlModuleDetail.Items.Add(li);
            ////    }
            ////}
            ////ddlModuleDetail.Items.Insert(0, new ListItem("Select Module", "0"));
        }
        protected void BtTicketDelete_Click(object sender, EventArgs e)
        {
            string childTicketId = hfTicketDelete.Value.Trim();
            bool isSibling = false;
           ASPxButton btn = sender as ASPxButton;
            if (string.IsNullOrWhiteSpace(childTicketId))
            {
                childTicketId = btn.CommandArgument;
                isSibling = btn.CommandName == "2";
            }
            if (!string.IsNullOrEmpty(childTicketId.Trim()) && 
                (objTicketRelationshipHelper.IsRelationExist(this.TicketId, childTicketId) ||
                isSibling))
            {
                int rowEffected = objTicketRelationshipHelper.DeleteRelation(this.TicketId, childTicketId);
                if (rowEffected > 0)
                {
                    LoadRelationalTree();
                    ddlModuleDetail.ClearSelection();
                }
            }
        }
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            //old server side popup open code not in use
            if (ddlModuleDetail.SelectedValue != null && ddlModuleDetail.SelectedValue != string.Empty)
            {
                //string url = ddlModuleDetail.Attributes["Url"];// " / sitepages/Request/?module=" + ddlModuleDetail.SelectedValue ;// UGITUtility.GetAbsoluteURL(Convert.ToString(spListItem["ModuleRelativePagePath"]));
                UGITModule module = moduleViewManager.LoadByName(ddlModuleDetail.SelectedValue);
                string url = module.StaticModulePagePath;
                url = string.Format("{1}?TicketId=0&ParentId={0}&NewSubticket=true&SourceTicketId={0}", TicketId, url);
                uHelper.ClosePopUpAndEndResponse(Context, true);
                UGITUtility.OpenPopup(Context, url, string.Format("New Sub-Item for {0}", ddlModuleDetail.SelectedValue), false);
            }
        }

        protected void treeListRT_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxTreeList.TreeListHtmlDataCellEventArgs e)
        {
            if (e.Column.FieldName == DatabaseObjects.Columns.TicketCreationDate)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketCreationDate))))
                    e.Cell.Text = DateTime.Parse((string)e.GetValue(DatabaseObjects.Columns.TicketCreationDate)).ToString("MMM-dd-yyyy");
            }
            if (e.Column.FieldName == DatabaseObjects.Columns.ModuleStepLookup)
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
            if (e.Column.FieldName == DatabaseObjects.Columns.TicketPriorityLookup)
            {
                priority = prioirtyMGR.LoadByID(UGITUtility.StringToLong(e.GetValue(DatabaseObjects.Columns.TicketPriorityLookup)));
                if (priority != null)
                    e.Cell.Text = priority.Title;
                else
                    e.Cell.Text = string.Empty;
            }
        }
    }
}