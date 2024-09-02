using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class SelectFolder : UserControl
    {
        public string Portal { get; set; }
        public string PortalType { get; set; }
        public string SelectedFolder { get; set; }
        List<int> validPermissions = new List<int>();
        DataTable collectionTable = new DataTable();
        string portal = string.Empty;
        const string DirImageUrl = "~/_layouts/15/images/uGovernIT/DocumentLibraryManagement/directory.png";
        string reportType = string.Empty;
        public string Type
        {
            get;
            set;
        }
        public delegate void DoneClickEventHandler(object sender, EventArgsDoneClickEventHandler e);
        public event DoneClickEventHandler DoneClick = delegate { };


        //protected override void OnInit(EventArgs e)
        //{
        //    collectionTable = uHelper.GetAuthorizedPortals();

        //    SPUser currentUser = SPContext.Current.Web.CurrentUser;
        //    validPermissions = currentUser.Groups.Cast<SPGroup>().Select(x => x.ID).ToList();
        //    validPermissions.Add(currentUser.ID);
        //    if (!string.IsNullOrEmpty(Request["type"]))
        //        reportType = Request["type"];
        //    else if (!string.IsNullOrEmpty(Type))
        //        reportType = Type;
        //    if (!string.IsNullOrEmpty(reportType))
        //    {
        //        //reportType = Request["type"];
        //        if (reportType == "projectreport")
        //        {
        //            divSelectPortal.Visible = false;
        //            FillTreeViewPortal(Portal);
        //        }
        //        else if (reportType == "MoveDocument")
        //        {
        //            FillTreeViewPortal(Portal);
        //        }
        //        else
        //        {
        //            divSelectPortal.Visible = true;

        //            if (!string.IsNullOrEmpty(Request[ddltype.UniqueID]))
        //            {
        //                FillPortalDropDown(Request[ddltype.UniqueID]);
        //                if (!string.IsNullOrEmpty(Request[hdnportalName.UniqueID]))
        //                {
        //                    Portal = Request[hdnportalName.UniqueID];
        //                    FillTreeViewPortal(Portal);
        //                }
        //            }
        //            else
        //            {
        //                FillPortalDropDown(ddltype.SelectedValue);
        //            }
        //        }
        //    }

        //    base.OnInit(e);
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    if (string.IsNullOrEmpty(Portal))
            //    {
            //        if (ddlPortal.SelectedIndex > 0)
            //        {
            //            portal = ddlPortal.SelectedItem.Text;
            //            FillTreeViewPortal(portal);
            //        }
            //        else
            //        {
            //            portal = ddltype.SelectedValue;
            //            FillTreeViewPortal(ddltype.SelectedValue);
            //        }
            //    }
            //}
        }

        //protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string type = ddltype.SelectedValue;

        //    //FillTreeViewPortal(ddlPortal.SelectedItem.Text);
        //}

        //private void FillPortalDropDown(string type)
        //{
        //    if (collectionTable == null || collectionTable.Rows.Count == 0)
        //    {
        //        collectionTable = uHelper.GetAuthorizedPortals();
        //    }
        //    if (collectionTable != null && collectionTable.Rows.Count > 0)
        //    {
        //        DataTable dtSelectedPortals = null;
        //        DataRow[] dr = null;
        //        switch (type)
        //        {
        //            case "project":
        //                dr = collectionTable.Select(string.Format("{0}={1}", DatabaseObjects.Columns.IsPortalProjectType, true));
        //                if (dr.Length > 0)
        //                    dtSelectedPortals = dr.CopyToDataTable();
        //                else
        //                    dtSelectedPortals = null;

        //                break;
        //            case "nonproject":
        //                dr = collectionTable.Select(string.Format("{0}={1}", DatabaseObjects.Columns.IsPortalProjectType, false));
        //                if (dr.Length > 0)
        //                    dtSelectedPortals = dr.CopyToDataTable();
        //                else
        //                    dtSelectedPortals = null;
        //                break;
        //            case "allportals":
        //                dtSelectedPortals = collectionTable;
        //                break;
        //            default:
        //                break;
        //        }

        //        if (dtSelectedPortals != null && dtSelectedPortals.Rows.Count > 0)
        //        {
        //            DataView defaultView = dtSelectedPortals.DefaultView;
        //            defaultView.Sort = DatabaseObjects.Columns.PortalName;

        //            ddlPortal.Items.Clear();

        //            // Bind the table with the portal Drop Down.
        //            DataRow newRow = dtSelectedPortals.NewRow();
        //            ddlPortal.DataSource = defaultView;
        //            ddlPortal.DataTextField = DatabaseObjects.Columns.PortalName;
        //            ddlPortal.DataValueField = DatabaseObjects.Columns.Id;
        //            ddlPortal.DataBind();

        //            // Insert an item "All Portals" only if it has the portal count >= 2.
        //            if (dtSelectedPortals.Rows.Count >= 2)
        //            {
        //                ListItem item = new ListItem("Select Portal", "0");
        //                ddlPortal.Items.Insert(0, item);
        //            }
        //        }
        //        else
        //        {
        //            ddlPortal.Items.Clear();
        //            ListItem item = new ListItem("Select Portal", "0");
        //            ddlPortal.Items.Insert(0, item);
        //        }
        //        if (!string.IsNullOrEmpty(Portal))
        //        {
        //            ddlPortal.Items.FindByText(Portal).Selected = true;
        //        }
        //    }
        //}

        //protected void ddlPortal_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    FillTreeViewPortal(ddlPortal.SelectedItem.Text);
        //}

        //private void FillTreeViewPortal(string portalName)
        //{
        //    if (validPermissions == null || validPermissions.Count == 0)
        //    {
        //        SPUser currentUser = SPContext.Current.Web.CurrentUser;
        //        validPermissions = currentUser.Groups.Cast<SPGroup>().Select(x => x.ID).ToList();
        //        validPermissions.Add(currentUser.ID);
        //    }

        //    aspxtvPortal.Nodes.Clear();
        //    SPWeb oWeb = SPContext.Current.Web;
        //    TreeViewNode node = null;
        //    string baseURL = Convert.ToString(oWeb.Url);

        //    node = new TreeViewNode();
        //    node.Name = node.Text = portalName;

        //    string viewType = EDMViewType.ByFolder.ToString();

        //    if (!string.IsNullOrEmpty(portalName) && portalName.ToLower() == "all portals")
        //    {
        //        // In "All Portals" case, we only have alternate views (not by folder)
        //        node = GetFolderNode(node, viewType, true);
        //    }
        //    else if (!string.IsNullOrEmpty(portalName))
        //    {

        //        TreeViewEDM eDMTreeView = DMCache.LoadPortalTreeView(portalName);
        //        // Case: When user uploaded multiple files.
        //        AjaxHelper ajaxHelper = new AjaxHelper();

        //        Dictionary<string, string> fileMetaInfo = ajaxHelper.GetSessionVarable("FileMetaInfo");
        //        // Check the user if the user is same who uploaded the multiple files then sync it.
        //        if (fileMetaInfo != null)
        //        {
        //            // Get the selected fodler while multiple files upload.
        //            string folderId = Convert.ToString(fileMetaInfo[DatabaseObjects.Columns.FolderID]);
        //            SPFolder oFolder = SPListHelper.GetSPFolder(uHelper.StringToGuid(folderId), oWeb);

        //            if (oFolder != null)
        //            {
        //                // Check all the folder is in sync.
        //                CheckFolderIsInSync(oFolder, fileMetaInfo);
        //                SPFolderCollection subFoldersColl = oFolder.SubFolders;
        //                if (subFoldersColl != null && subFoldersColl.Count > 0)
        //                {
        //                    foreach (SPFolder subFolder in subFoldersColl)
        //                    {
        //                        bool isExists = false;
        //                        var dtitem = subFolder.Item["Created"];
        //                        if (dtitem != null)
        //                        {
        //                            DateTime dtCreated = Convert.ToDateTime(dtitem);
        //                            DateTime dtCurrentdate = DateTime.Now;
        //                            if ((dtCurrentdate - dtCreated).Minutes <= 10)
        //                            {
        //                                List<FolderList> lstFolders = eDMTreeView.FolderList;
        //                                foreach (var item in lstFolders)
        //                                {
        //                                    FolderList selectedFolder = FindNode(item, Convert.ToString(subFolder.UniqueId));
        //                                    if (selectedFolder != null)
        //                                    {
        //                                        isExists = true;
        //                                        break;
        //                                    }
        //                                }
        //                                if (isExists == false)
        //                                {
        //                                    DMCache.InsertItemInCache(portalName, Convert.ToString(subFolder.UniqueId));
        //                                }
        //                            }
        //                        }

        //                    }
        //                }
        //            }

        //            // Remove session for multiple upload file.
        //            ajaxHelper.RemoveSessionVarable("FileMetaInfo");
        //        }

        //        //Build the tree from cache object.
        //        if (eDMTreeView != null)
        //        {
        //            SPUser currentUser = SPContext.Current.Web.CurrentUser;
        //            node = GetFolderNode(node, eDMTreeView.FolderList, eDMTreeView.PortalInfo.Name, currentUser);
        //            if (eDMTreeView.FolderList != null && eDMTreeView.FolderList.Count > 0)
        //            {
        //                imgCollapse.Visible = true;
        //                imgExpand.Visible = true;
        //            }
        //            else
        //            {
        //                imgCollapse.Visible = false;
        //                imgExpand.Visible = false;
        //            }
        //        }

        //    }

        //    node.Image.Url = DirImageUrl;
        //    aspxtvPortal.Nodes.Add(node);
        //    aspxtvPortal.ExpandAll();
        //    //Default setting
        //    portal = portalName;
        //}

        //private TreeViewNode GetFolderNode(TreeViewNode node, List<FolderList> folders, string portal, SPUser currentUser)
        //{
        //    // Note: Disable size calculation to speed up
        //    if (folders != null && folders.Count > 0)
        //    {
        //        TreeViewNode folderNode;
        //        for (int j = 0; j <= folders.Count - 1; j++)
        //        {
        //            if (uHelper.CheckPermission(folders[j].FolderPermission, validPermissions))
        //            {
        //                folderNode = new TreeViewNode();
        //                folderNode.Text = folders[j].Name;
        //                folderNode.ToolTip = folders[j].Name + ":\n" + folders[j].NoOfFiles + " file(s)";
        //                if (folders[j].NoOfLinks > 0)
        //                    folderNode.ToolTip += ", " + folders[j].NoOfLinks + " link(s)";
        //                TreeViewNode treeNode = GetFolderNode(folderNode, folders[j].SubFolderList, portal, currentUser);
        //                if (treeNode != null && treeNode.Nodes != null && treeNode.Nodes.Count > 0 && folderNode != treeNode)
        //                {
        //                    folderNode.Nodes.Add(treeNode);
        //                }
        //                folderNode.Expanded = false;
        //                folderNode.Name = folders[j].folderGuid;
        //                folderNode.Image.Url = DirImageUrl;
        //                node.Nodes.Add(folderNode);
        //            }
        //        }
        //    }
        //    return node;
        //}

        //public static FolderList FindNode(FolderList node, string folderGuid)
        //{

        //    if (node == null)
        //        return null;

        //    if (node.folderGuid == folderGuid)
        //        return node;
        //    if (node.SubFolderList != null)
        //    {
        //        foreach (var child in node.SubFolderList)
        //        {
        //            var found = FindNode(child, folderGuid);
        //            if (found != null)
        //                return found;
        //        }
        //    }
        //    return null;
        //}

        //private void CheckFolderIsInSync(SPFolder folder, Dictionary<string, string> fileMetaInfo)
        //{
        //    if (folder.Name != "Forms")
        //    {
        //        if (folder.Name != folder.DocumentLibrary.RootFolder.Name)
        //        {
        //            if (string.IsNullOrEmpty(Convert.ToString(folder.Item[DatabaseObjects.Columns.FolderName])))
        //                SPListHelper.UpdateFolderDefaultValue(folder);

        //            if (SPListHelper.GetNumberOfFilesInFolder(folder, false) > 0)
        //                SPListHelper.SyncFolderWithApplication(folder, fileMetaInfo);
        //        }
        //        foreach (SPFolder folder1 in folder.SubFolders)
        //        {
        //            CheckFolderIsInSync(folder1, fileMetaInfo);
        //        }
        //    }
        //    return;
        //}

        /// <summary>
        /// Func. : This function builds the Tree view on the base of View Type selected (for non-folder views)
        /// </summary>
        /// <param name="portalNode"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        //private TreeViewNode GetFolderNode(TreeViewNode portalNode, string viewType, bool useCache)
        //{
        //    // If cache is on.
        //    if (useCache)
        //    {
        //        return GetFolderNode(portalNode, viewType);
        //    }

        //    // if cache is off.
        //    SPList oList = SPListHelper.GetSPList(portal);
        //    SPList treeList = null;
        //    string colName = string.Empty;


        //    UGITModule moduleObj = uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, "EDM");
        //    List<ModuleColumn> moduleColumnsList = moduleObj.List_ModuleColumns;
        //    if (moduleColumnsList != null && moduleColumnsList.Count > 0)
        //    {
        //        ModuleColumn moduleColumn = moduleColumnsList.FirstOrDefault(c => c.FieldDisplayName.ToLower() == viewType.ToLower());
        //        if (moduleColumn != null)
        //        {
        //            colName = moduleColumn.FieldName;
        //            string listName = ConfigurationVariable.GetValue(Convert.ToString(moduleColumn.FieldName));
        //            if (!string.IsNullOrEmpty(listName))
        //            {
        //                treeList = SPContext.Current.Web.Lists[listName];
        //            }
        //        }
        //    }

        //    // Add new Node "-N/A-" if there are documents with blank value or "-N/A-".
        //    DataTable tblDocumentInfo = SPListHelper.GetDataTable(DatabaseObjects.Lists.DocumentInfoList);
        //    if (tblDocumentInfo != null && tblDocumentInfo.Rows.Count > 0)
        //    {

        //        if (colName != string.Empty)
        //        {
        //            if (portal != Constants.AllPortals)
        //            {
        //                var documents = from row in tblDocumentInfo.AsEnumerable()
        //                                where (Convert.ToString(row[DatabaseObjects.Columns.PortalId]) == oList.ID.ToString() && (Convert.ToString(row[colName]) == string.Empty || Convert.ToString(row[colName]) == Constants.NA))
        //                                select row;

        //                if (documents.Count() > 0)
        //                {
        //                    TreeViewNode treeNode = new TreeViewNode();
        //                    treeNode.Text = Constants.NA;
        //                    treeNode.Name = Constants.NA;
        //                    treeNode.ToolTip = Constants.NA;
        //                    treeNode.Expanded = false;
        //                    treeNode.Image.Url = DirImageUrl;
        //                    portalNode.Nodes.Add(treeNode);
        //                }
        //            }
        //            else
        //            {
        //                var documents = from row in tblDocumentInfo.AsEnumerable()
        //                                where (Convert.ToString(row[colName]) == string.Empty || Convert.ToString(row[colName]) == Constants.NA)
        //                                select row;
        //                if (documents.Count() > 0)
        //                {
        //                    TreeViewNode treeNode = new TreeViewNode();
        //                    treeNode.Text = Constants.NA;
        //                    treeNode.Name = Constants.NA;
        //                    treeNode.ToolTip = Constants.NA;
        //                    treeNode.Expanded = false;
        //                    treeNode.Image.Url = DirImageUrl;
        //                    portalNode.Nodes.Add(treeNode);
        //                }
        //            }
        //        }
        //    }

        //    // Add remaining nodes based on values from actual items
        //    if (treeList != null && treeList.ItemCount > 0)
        //    {
        //        TreeViewNode folderNode;
        //        SPQuery queryOrderBy = new SPQuery();
        //        queryOrderBy.Query = "<OrderBy><FieldRef Name='Title' /></OrderBy>";
        //        SPListItemCollection listItemCollection = treeList.GetItems(queryOrderBy);

        //        for (int j = 0; j <= listItemCollection.Count - 1; j++)
        //        {
        //            string nodeTitle = Convert.ToString(listItemCollection[j][DatabaseObjects.Columns.Title]).Trim();
        //            if (!string.IsNullOrEmpty(nodeTitle))
        //            {
        //                SPWeb oWeb = SPContext.Current.Web;
        //                string baseURL = Convert.ToString(oWeb.Url);

        //                bool isContainsFiles = false;

        //                if (portal != Constants.AllPortals)
        //                    isContainsFiles = SPListHelper.CheckFilesInPortal(oList, colName, nodeTitle, string.Empty);
        //                else
        //                    isContainsFiles = SPListHelper.CheckFilesInPortal(string.Empty, colName, nodeTitle, string.Empty);

        //                // If the file exist for the particular View Type's value, then add the View Type's value as node.
        //                // Exp: View Type: Department  Value= "Sale", "Purchase" so if the files exist for sale department add in tree as node otherwise don't add it.
        //                if (isContainsFiles)
        //                {
        //                    folderNode = new TreeViewNode();
        //                    folderNode.Text = nodeTitle;
        //                    folderNode.Name = nodeTitle;
        //                    folderNode.ToolTip = nodeTitle;
        //                    folderNode.Image.Url = DirImageUrl;
        //                    folderNode.Expanded = false;
        //                    portalNode.Nodes.Add(folderNode);
        //                }
        //            }
        //        }
        //    }

        //    return portalNode;
        //}
        //private TreeViewNode GetFolderNode(TreeViewNode portalNode, string viewType)
        //{
        //    DataTable treeList = null;
        //    string colName = string.Empty;
        //    SPList oList = SPListHelper.GetSPList(portal);

        //    //if (viewType.Trim() == EDMViewType.ByDepartment)
        //    //{
        //    //    treeList = DMCache.GetDataTable(DatabaseObjects.Lists.DMDepartment);
        //    //    colName = DatabaseObjects.Columns.DepartmentNameLookup;
        //    //}
        //    //else if (viewType.Trim() == EDMViewType.ByDocType)
        //    //{
        //    //    treeList = DMCache.GetDataTable(DatabaseObjects.Lists.DocumentType);
        //    //    colName = DatabaseObjects.Columns.DocumentTypeLookup;
        //    //}
        //    //else if (viewType.Trim() == EDMViewType.ByVendor)
        //    //{
        //    //    treeList = DMCache.GetDataTable(DatabaseObjects.Lists.Vendor);
        //    //    colName = DatabaseObjects.Columns.DMVendorLookup;
        //    //}
        //    //else if (viewType.Trim() == EDMViewType.ByProject)
        //    //{
        //    //    treeList = DMCache.GetDataTable(DatabaseObjects.Lists.Projects);
        //    //    colName = DatabaseObjects.Columns.ProjectLookup;
        //    //}
        //    if (viewType.Trim() == EDMViewType.ByDocType)
        //    {
        //        treeList = DMCache.GetDataTable(DatabaseObjects.Lists.DocumentType);
        //        colName = DatabaseObjects.Columns.DocumentTypeLookup;
        //    }
        //    else
        //    {
        //        UGITModule moduleObj = uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, "EDM");
        //        List<ModuleColumn> moduleColumnsList = moduleObj.List_ModuleColumns;
        //        if (moduleColumnsList != null && moduleColumnsList.Count > 0)
        //        {
        //            ModuleColumn moduleColumn = moduleColumnsList.FirstOrDefault(c => c.FieldDisplayName.ToLower() == viewType.ToLower());
        //            if (moduleColumn != null)
        //            {
        //                colName = moduleColumn.FieldName;
        //                string listName = ConfigurationVariable.GetValue(Convert.ToString(moduleColumn.FieldName));
        //                if (!string.IsNullOrEmpty(listName))
        //                {
        //                    //SPList list = SPContext.Current.Web.Lists[listName];
        //                    //if (list != null && list.ItemCount > 0)
        //                    //{
        //                    //    treeList = list.Items.GetDataTable();
        //                    //}
        //                    treeList = DMCache.GetDataTable(listName);
        //                }
        //            }
        //        }
        //    }
        //    // Add new Node "-N/A-" if there are documents with blank value or "-N/A-".       
        //    DataTable tblDocumentInfo = DMCache.GetDataTable(DatabaseObjects.Lists.DocumentInfoList);
        //    if (colName != string.Empty && tblDocumentInfo != null)
        //    {
        //        if (portal != Constants.AllPortals)
        //        {
        //            var documents = from row in tblDocumentInfo.AsEnumerable()
        //                            where (Convert.ToString(row[DatabaseObjects.Columns.PortalId]) == oList.ID.ToString() && (Convert.ToString(row[colName]) == string.Empty || Convert.ToString(row[colName]) == Constants.NA))
        //                            select row;

        //            if (documents.Count() > 0)
        //            {
        //                TreeViewNode treeNode = new TreeViewNode();
        //                treeNode.Text = Constants.NA;
        //                treeNode.Name = Constants.NA;
        //                treeNode.ToolTip = Constants.NA;
        //                treeNode.Expanded = false;
        //                treeNode.Image.Url = DirImageUrl;
        //                portalNode.Nodes.Add(treeNode);
        //            }
        //        }
        //        else
        //        {
        //            var documents = from row in tblDocumentInfo.AsEnumerable()
        //                            where (Convert.ToString(row[colName]) == string.Empty || Convert.ToString(row[colName]) == Constants.NA)
        //                            select row;
        //            if (documents.Count() > 0)
        //            {
        //                TreeViewNode treeNode = new TreeViewNode();
        //                treeNode.Text = Constants.NA;
        //                treeNode.Name = Constants.NA;
        //                treeNode.ToolTip = Constants.NA;
        //                treeNode.Image.Url = DirImageUrl;
        //                treeNode.Expanded = false;
        //                portalNode.Nodes.Add(treeNode);
        //            }
        //        }
        //    }

        //    // Add remaining nodes based on values from actual items
        //    if (treeList != null && treeList.Rows.Count > 0)
        //    {
        //        TreeViewNode folderNode;
        //        treeList.DefaultView.Sort = DatabaseObjects.Columns.Title + " ASC";
        //        treeList = treeList.DefaultView.ToTable();
        //        foreach (DataRow item in treeList.Rows)
        //        {
        //            string nodeTitle = Convert.ToString(item[DatabaseObjects.Columns.Title]).Trim();
        //            if (!string.IsNullOrEmpty(nodeTitle))
        //            {
        //                SPWeb oWeb = SPContext.Current.Web;
        //                string baseURL = Convert.ToString(oWeb.Url);
        //                bool isContainsFiles = false;

        //                if (portal != Constants.AllPortals)
        //                    isContainsFiles = SPListHelper.CheckFilesInPortal(oList, colName, nodeTitle, string.Empty);
        //                else
        //                    isContainsFiles = SPListHelper.CheckFilesInPortal(string.Empty, colName, nodeTitle, string.Empty);

        //                // If the file exist for the particular View Type's value, then add the View Type's value as node.
        //                // Exp: View Type: Department  Value= "Sale", "Purchase" so if the files exist for sale department add in tree as node otherwise don't add it.
        //                if (isContainsFiles)
        //                {
        //                    folderNode = new TreeViewNode();
        //                    folderNode.Text = nodeTitle;
        //                    folderNode.Name = nodeTitle;
        //                    folderNode.ToolTip = nodeTitle;
        //                    folderNode.Expanded = false;
        //                    folderNode.Image.Url = DirImageUrl;
        //                    portalNode.Nodes.Add(folderNode);
        //                }
        //                // ------New Code ends here.
        //            }
        //        }
        //    }

        //    return portalNode;
        //}

        protected void btnDone_Click(object sender, EventArgs e)
        {
            if (aspxtvPortal.SelectedNode == null)
                return;
            string retValue = aspxtvPortal.SelectedNode.Name + Constants.Separator1 + aspxtvPortal.SelectedNode.Text + Constants.Separator1 + hdnportalName.Value;
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict.Add("DocId", retValue);
            dict.Add("IsUpload", "true");
            // var vals = uHelper.GetJsonForDictionary(dict);
            EventArgsDoneClickEventHandler evnt = new EventArgsDoneClickEventHandler();
            evnt.Dict = dict;
            DoneClick(this, evnt);
            popupSelectFolder.ShowOnPageLoad = false;
        }
    }
}