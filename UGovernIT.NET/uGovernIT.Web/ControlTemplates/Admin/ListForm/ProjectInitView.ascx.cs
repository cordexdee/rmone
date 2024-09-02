using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Linq;
using System.Web;
using System.Collections.Generic;

namespace uGovernIT.Web
{
    public partial class ProjectInitView : UserControl
    {
        //private SPList _SPList;
        private DataTable _DataTable;
        private string addNewItem = string.Empty;

        private string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}";
        //private string absoluteUrlView = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&showdelete={2}";
        private string formTitle = "Project Initiatives";
        //private string viewParam = "projectinit";
        private string newParam = "projectinitnew";
        private string editParam = "projectinitedit";
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        DataTable bsTable;
        public string BStitle;
        public string iniTitle;
        List<BusinessStrategy> BSList;
        ConfigurationVariableManager ConfigVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        BusinessStrategyManager BSManager = new BusinessStrategyManager(HttpContext.Current.GetManagerContext());
        ProjectInitiativeViewManager PIManager = new ProjectInitiativeViewManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
          
            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, "0"));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','700','350',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            BStitle = string.IsNullOrEmpty(ConfigVariableManager.GetValue(ConfigConstants.InitiativeLevel1Name)) ? "Business Initiative" : ConfigVariableManager.GetValue(ConfigConstants.InitiativeLevel1Name);
            iniTitle = string.IsNullOrEmpty(ConfigVariableManager.GetValue(ConfigConstants.InitiativeLevel2Name)) ? "Program" : ConfigVariableManager.GetValue(ConfigConstants.InitiativeLevel2Name);

            //Popup Header Text
            addnewstrategy.HeaderText = string.Format("Add New {0}", BStitle);
            ccpEditBS.HeaderText = string.Format("Edit {0}", BStitle);

            formTitle = iniTitle;

            BStitle = string.Format("{0}(s)", BStitle);
            iniTitle = string.Format("{0}(s)", iniTitle);

            if (!IsPostBack)
            {
                if (Request["showdelete"] != null)
                {
                    chkShowDeleted.Checked = Convert.ToString(Request["showdelete"]) == "0" ? false : true;
                }
            }
            BindBSGrid();
            //BindGriview();
            BindEditPopupData();
            _gridView.DataBind();
            base.OnLoad(e);
        }

        private void BindBSGrid()
        {
            int focusindex = 0;
            BSList = BSManager.Load();  // SPListHelper.GetSPList(DatabaseObjects.Lists.BusinessStrategy, spWeb);
            if (BSList == null)
                return;
            bsTable = BSManager.GetDataTable();

            if (bsTable != null)
            {
                focusindex = grdBS.FocusedRowIndex;
                foreach (DataRow row in bsTable.Rows)
                {
                    row[DatabaseObjects.Columns.TicketDescription] = UGITUtility.TruncateWithEllipsis(Convert.ToString(row[DatabaseObjects.Columns.TicketDescription]), 100);
                }
                DataView dtsortview = bsTable.DefaultView;
                dtsortview.Sort = string.Format("{0} asc", DatabaseObjects.Columns.Title);
                DataTable dtsort = dtsortview.ToTable(true);
                bsTable = dtsort;
            }

            grdBS.DataSource = bsTable;
            grdBS.DataBind();
            if (focusindex > 0)
                grdBS.FocusedRowIndex = focusindex;

        }

        private void BindGriview()
        {
            //_SPList = SPListHelper.GetSPList(DatabaseObjects.Lists.ProjectInitiative);
            _DataTable = PIManager.GetDataTable();  // _SPList.Items.GetDataTable();

            if (_DataTable != null && !chkShowDeleted.Checked)
            {
                DataRow[] dataRows = _DataTable.Select(string.Format("{0}={1} or {0} IS NULL", DatabaseObjects.Columns.Deleted, "False"));
                if (dataRows.Length > 0)
                {
                    _DataTable = null;
                    _DataTable = dataRows.CopyToDataTable();
                }
            }

            if (_DataTable != null && _DataTable.Rows.Count > 0)
            {
                // Only show first 100 characters of description
                foreach (DataRow row in _DataTable.Rows)
                {
                    row[DatabaseObjects.Columns.ProjectNote] = UGITUtility.TruncateWithEllipsis(Convert.ToString(row[DatabaseObjects.Columns.ProjectNote]), 100);
                }

                // Show any initiatives not assigned to a strategy under the <Unassigned> strategy
                if (bsTable != null && bsTable.Rows.Count > 0)
                {
                    bool unassignedInitiativesExist = _DataTable.AsEnumerable().Any(x => string.IsNullOrEmpty(Convert.ToString(x[DatabaseObjects.Columns.BusinessStrategyLookup])));
                    bool exist = bsTable.AsEnumerable().Any(x => Convert.ToString(x[DatabaseObjects.Columns.Title]).ToLower() == "<unassigned>");

                    if (unassignedInitiativesExist)
                    {
                        if (!exist)
                        {
                            DataRow drow = bsTable.NewRow();
                            drow[DatabaseObjects.Columns.Title] = "<Unassigned>";
                            drow[DatabaseObjects.Columns.TicketDescription] = "Initiatives not assigned to a strategy";
                            bsTable.Rows.InsertAt(drow, 0);
                        }
                    }
                    else if (exist)
                    {
                        DataRow row = bsTable.AsEnumerable().FirstOrDefault(x => Convert.ToString(x[DatabaseObjects.Columns.Title]).ToLower() == "<unassigned>");
                        if (row != null)
                            bsTable.Rows.Remove(row);
                    }
                }
            }

            grdBS.DataSource = bsTable;
            grdBS.DataBind();
            _gridView.DataSource = _DataTable;

        }

        protected void chkShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            _gridView.DataBind();
        }

        protected void btnSaveBS_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBSTitle.Text))
                return;
            
            BusinessStrategy item = new BusinessStrategy();
            item.Title = txtBSTitle.Text;
            item.Description = txtBSDescription.Text;
                
            BSManager.Insert(item);

            BindGriview();
            BindBSGrid();
        }

        protected void lnkupdateBS_Click(object sender, EventArgs e)
        {
            if (hdnBSKey != null && hdnBSKey.Contains("BSKey"))
            {
                int key = 0;
                int.TryParse(Convert.ToString(hdnBSKey.Get("BSKey")), out key);
                if (key > 0)
                {
                    BusinessStrategy item = BSList.FirstOrDefault(x => x.ID == key);  // bsList.GetItemById(key);
                    if (item != null)
                    {
                        item.Title = hdnBSKey.Contains("BSEditTitle") ? Convert.ToString(hdnBSKey.Get("BSEditTitle")) : string.Empty;
                        item.Description = hdnBSKey.Contains("BSEditDecription") ? Convert.ToString(hdnBSKey.Get("BSEditDecription")) : string.Empty;

                        BSManager.Update(item);
                        BindBSGrid();
                    }
                }
            }
        }

        private void BindEditPopupData()
        {
            if (hdnBSKey != null && hdnBSKey.Contains("BSKey"))
            {
                int key = 0;
                int.TryParse(Convert.ToString(hdnBSKey.Get("BSKey")), out key);
                if (key > 0)
                {
                    BusinessStrategy item = BSList.FirstOrDefault(x => x.ID == key);  // bsList.GetItemById(key);
                    if (item != null)
                    {
                        txtEditTitle.Text = Convert.ToString(item.Title);
                        txtEditDescription.Text = Convert.ToString(item.Description);
                    }
                }
            }
        }

        protected void _gridView_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                ProjectInitiative projectInit = PIManager.LoadByID(Convert.ToInt64(e.KeyValue));
                bool IsDeleted = Convert.ToBoolean(projectInit.Deleted);
                if (IsDeleted)
                {
                    e.Row.BackColor = System.Drawing.Color.FromArgb(165, 52, 33);
                    foreach (TableCell item in e.Row.Cells)
                    {
                        item.Style.Add("color", "#FFF");
                    }
                }

                ASPxGridView grdview = sender as ASPxGridView;
                HtmlAnchor anchorEdit = (HtmlAnchor)grdview.FindRowCellTemplateControl(e.VisibleIndex, null, "aEdit");
                if (anchorEdit != null)
                {
                    string editItem;
                    editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, Convert.ToString(e.KeyValue)));
                    string jsFunc = string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','700','350',0,'{2}','true')", editItem, Convert.ToString(e.GetValue(DatabaseObjects.Columns.Title)), Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
                    anchorEdit.Attributes.Add("href", jsFunc);
                }
            }
        }

        protected void _gridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            _gridView.PageIndex = 0;
            
        }

        protected void _gridView_DataBinding(object sender, EventArgs e)
        {
            object bskey = null;
            //grdBS.GetRowValues(grdBS.FocusedRowIndex, DatabaseObjects.Columns.Title);
            int key = 0;
            if (hdnbskeyvalue != null && hdnbskeyvalue.Contains("keyvalue"))
            {
                
                int.TryParse(Convert.ToString(hdnbskeyvalue.Get("keyvalue")), out key);
                if (key > 0)
                {
                    BusinessStrategy item = BSList.FirstOrDefault(x => x.ID == key);  // bsList.GetItemById(key);
                    if (item != null)
                    {
                        bskey = item.Title;
                    }
                }
                else
                    bskey = "<Unassigned>";
            }
            if (bskey != null)
            {
                DataRow[] result = null;
                string title = Convert.ToString(bskey);
                if (title.ToLower() == "<unassigned>")
                    title = string.Empty;

                _DataTable = PIManager.GetDataTable();

                if (chkShowDeleted.Checked)
                    result = _DataTable.AsEnumerable().Where(x => Convert.ToString(x[DatabaseObjects.Columns.BusinessStrategyLookup]) == Convert.ToString(key) && UGITUtility.StringToBoolean(x[DatabaseObjects.Columns.Deleted])).ToArray();
                else
                    result = _DataTable.AsEnumerable().Where(x => Convert.ToString(x[DatabaseObjects.Columns.BusinessStrategyLookup]) == Convert.ToString(key) && !UGITUtility.StringToBoolean(x[DatabaseObjects.Columns.Deleted])).ToArray();

                if (result != null && result.Length > 0)
                    _gridView.DataSource = result.CopyToDataTable();
                else
                    _gridView.DataSource = result;
            }
        }

        protected void grdBS_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                if (string.IsNullOrEmpty(Convert.ToString(e.KeyValue)))
                {
                    HtmlImage img = (HtmlImage)grdBS.FindRowCellTemplateControl(e.VisibleIndex, null, "imgBSEdit");
                    if (img != null)
                    {
                        img.Attributes.Add("style", "display:none");
                    }
                }
            }
        }

        protected void btnApplyChanges_Click(object sender, EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            string cacheName = "Lookup_" + DatabaseObjects.Tables.BusinessStrategy + "_" + context.TenantID;
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.BusinessStrategy, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            Util.Cache.CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);

           
            cacheName = "Lookup_" + DatabaseObjects.Tables.ProjectInitiative + "_" + context.TenantID;
            dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectInitiative, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            Util.Cache.CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);
        }
    }
}
