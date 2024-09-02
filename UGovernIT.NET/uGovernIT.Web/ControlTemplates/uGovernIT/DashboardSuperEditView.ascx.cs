using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using System.Web;
using uGovernIT.Utility.Entities;
using System.Data;
using uGovernIT.Manager.Helper;

namespace uGovernIT.Web
{
    public partial class DashboardSuperEditView : UserControl
    {

        List<Dashboard> listofDB = null;
        public DashboardPanelView View { get; set; }
        public String ViewType { get; set; }
        private SuperDashboardsView superView;
        public String superViewName = "Super Dashboards";
        List<DashboardGroupProperty> groups = new List<DashboardGroupProperty>();
        protected string dashboardId;
        protected string dashboardUrl;
        protected string dashboardname;
        protected string dashboardType;
        private string absPath = string.Empty;
        DashboardManager dManager = new DashboardManager(HttpContext.Current.GetManagerContext());
        DashboardPanelViewManager objDashboardPanelViewManager = new DashboardPanelViewManager(HttpContext.Current.GetManagerContext());
        DataTable defaultValueData = null;
        protected override void OnInit(EventArgs e)
        {
            glDefaultValue.GridView.CustomCallback += GridView_CustomCallback;
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            dashboardUrl = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx");

            absPath = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.PickFromAsset);
            //lnkbtnPickAssets.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','900px','600px','','')", absPath, "Pick From Library"));

            listofDB = dManager.LoadAll(false);
            listofDB = listofDB.Where(x => x.DashboardType == DashboardType.Chart || x.DashboardType == DashboardType.Panel).OrderBy(x => x.DashboardType).ThenBy(x => x.Title).ToList();

            if (View != null)
            {
                BtnDeleteGroup.Visible = true;
                superView = View.ViewProperty as SuperDashboardsView;
            }
            else
            {
                BtnDeleteGroup.Visible = false;
            }

            if (!Page.IsPostBack)
            {
                BindDBList();
                if (View != null)
                {
                    ImgDeleteView.Visible = true;
                    BindTreeview();
                    DefaultGroupBind();
                    BuildFilterTree();
                }
            }
            else if (View == null)
            {

                int viewID;
                Int32.TryParse(hdnviewid.Value, out viewID);
                if (viewID > 0)
                {
                    View = objDashboardPanelViewManager.LoadByID(viewID);
                    if (View != null)
                        superView = View.ViewProperty as SuperDashboardsView;
                    ImgDeleteView.Visible = true;

                    BuildFilterTree();
                }
            }
        }
        /// <summary>
        /// To Select First Node of Treeview
        /// </summary>
        public void DefaultGroupBind()
        {
            pnlgroup.Visible = true;
            pnlDashboard.Visible = false;
            pnlgroup.GroupingText = "Edit Group View";
            var obj = treeViewGroup.Nodes[0].ChildNodes[0].Text;
            treeViewGroup.Nodes[0].ChildNodes[0].Selected = true;
            var lst = (View.ViewProperty as SuperDashboardsView).DashboardGroups.FirstOrDefault(x => x.DashboardGroup.ToLower() == obj.ToLower());
            txtname.Text = lst.DashboardGroup;
            txtwidth.Text = lst.Width.ToString();
            txtItemorder.Text = lst.ItemOrder.ToString();

            foreach (ListItem item in chkdashboard.Items)
            {
                if ((lst.Dashboards.Exists(x => x.DashboardName == item.Value)))
                    item.Selected = true;
                else
                    item.Selected = false;
            }

        }
        /// <summary>
        ///Bind the treeView with Super View,Group,and dashboard 
        /// </summary>
        private void BindTreeview()
        {
            treeViewGroup.Nodes.Clear();
            TreeNode root = null;
            TreeNode childNode;
            TreeNode childRoot;
            txtViewname.Text = View.ViewName;
            //SPFieldUserValueCollection collection = new SPFieldUserValueCollection();
            //if (View.AuthorizedToView != null)
            //{
            //    View.AuthorizedToView.ForEach(x => { collection.Add(new SPFieldUserValue(SPContext.Current.Web, x.ID, x.Name)); });
            //    peAuthorizedToView.UpdateEntities(uHelper.getUsersListFromCollection(collection));
            //}
            peAuthorizedToView.SetValues(View.AuthorizedToViewUsers);
            root = new TreeNode();
            root.Text = View.ViewName;
            var Dashboards = superView.DashboardGroups.OrderBy(x => x.ItemOrder).ToList(); ;
            for (int i = 0; i < Dashboards.Count; i++)
            {
                childRoot = new TreeNode();
                childRoot.Value = "group_" + Dashboards[i].DashboardGroup;
                childRoot.Text = Dashboards[i].DashboardGroup;
                root.ChildNodes.Add(childRoot);
                Dashboards[i].Dashboards = Dashboards[i].Dashboards.OrderBy(x => x.ItemOrder).ToList();
                for (int j = 0; j < Dashboards[i].Dashboards.Count; j++)
                {
                    Dashboard dashboard = listofDB.FirstOrDefault(x => x.Title == Dashboards[i].Dashboards[j].DashboardName);
                    if (dashboard != null)
                    {
                        childNode = new TreeNode();
                        childNode.Value = "dashboard_" + dashboard.Title + "_" + dashboard.ID + "_" + dashboard.DashboardType;
                        childNode.Text = dashboard.Title;
                        childRoot.ChildNodes.Add(childNode);
                    }
                }

            }
            treeViewGroup.Nodes.Add(root);
            treeViewGroup.DataBind();
        }
        /// <summary>
        ///To Bind Checkbox List with  Existing Dashboard  
        /// </summary>
        public void BindDBList()
        {
            foreach (Dashboard dashobard in listofDB)
            {
                ListItem item = new ListItem(dashobard.Title, dashobard.Title);
                chkdashboard.Items.Add(item);
            }
        }
        /// <summary>
        ///To Add new group in View  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnAddGroup_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (chkdashboard.SelectedIndex != -1)
            {
                if (View == null)
                {

                    View = new DashboardPanelView();
                    View.ViewName = txtViewname.Text.Trim();
                    View.Title = txtViewname.Text.Trim();
                }

                List<DashboardGroupProperty> groups = new List<DashboardGroupProperty>();
                if (superView != null)
                {
                    groups = superView.DashboardGroups;
                }
                else
                {
                    superView = new SuperDashboardsView();
                }
                View.ViewProperty = superView;
                string name = string.Empty;
                if (treeViewGroup.SelectedNode != null)
                    if (treeViewGroup.SelectedNode.ChildNodes.Count > 0 && treeViewGroup.SelectedNode.Parent != null)
                    {
                        name = treeViewGroup.SelectedNode.Text;
                    }
                    else
                    {
                        name = txtname.Text;
                    }
                DashboardGroupProperty currentGroup = groups.FirstOrDefault(x => x.DashboardGroup.ToLower() == name.ToLower());
                if (currentGroup == null)
                {
                    currentGroup = new DashboardGroupProperty();
                    groups.Add(currentGroup);
                }
                List<DashboardPanelProperty> lstDBPanlProp = currentGroup.Dashboards;
                if (lstDBPanlProp == null)
                {
                    lstDBPanlProp = new List<DashboardPanelProperty>();
                }

                View.ViewName = txtViewname.Text.Trim();
                int width = 0;
                int.TryParse(txtwidth.Text.Trim(), out width);
                currentGroup.Width = width;

                int itemOrder = 0;
                int.TryParse(txtItemorder.Text.Trim(), out itemOrder);
                currentGroup.ItemOrder = itemOrder;
                currentGroup.DashboardGroup = txtname.Text;
                DashboardPanelProperty dashboard = null;

                string dashboardName = string.Empty;
                foreach (ListItem lstItem in chkdashboard.Items)
                {
                    dashboardName = lstItem.Value;
                    dashboard = lstDBPanlProp.FirstOrDefault(x => x.DashboardName == dashboardName);

                    if (lstItem.Selected)
                    {
                        Dashboard uDashboard = listofDB.FirstOrDefault(x => x.Title == dashboardName);
                        if (dashboard == null && uDashboard != null)
                        {
                            dashboard = new DashboardPanelProperty();
                            dashboard.DashboardName = dashboardName;
                            dashboard.DisplayName = uDashboard.panel.ContainerTitle;
                            dashboard.Height = uDashboard.PanelHeight;
                            dashboard.ItemOrder = lstDBPanlProp.Count;
                            dashboard.Theme = uDashboard.ThemeColor;
                            lstDBPanlProp.Add(dashboard);
                        }
                        dashboard.Width = Convert.ToInt32(txtwidth.Text.Trim());
                        dashboard = null;
                    }
                    else
                    {
                        if (dashboard != null)
                        {
                            lstDBPanlProp.Remove(dashboard);
                        }
                    }
                }

                currentGroup.Dashboards = lstDBPanlProp;
                superView.DashboardGroups = groups;
                View.ViewType = superViewName;
                //SPFieldUserValueCollection collection = uHelper.GetFieldUserValueCollection(peAuthorizedToView.ResolvedEntities, SPContext.Current.Web);
                View.AuthorizedToView = new List<UserProfile>();
                //if (collection != null)
                //{
                //    collection.ForEach(x =>
                //    {
                //        View.AuthorizedToView.Add(new Core.UserInfo(x.LookupId, x.LookupValue, x.User == null ? true : false));
                //    });
                //}
                View.AuthorizedToViewUsers = peAuthorizedToView.GetValues();
                objDashboardPanelViewManager.Save(View);
                hdnviewid.Value = View.ID.ToString();
                BindTreeview();
                ClearViewGroup();

            }
            else
            {
                lblchklstmsg.Text = "Please select at least one dashboard.";
            }
            BtnDeleteGroup.Visible = false;

        }
        /// <summary>
        ///To Bind the property of Group and Dashbord with control 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreeViewGroup_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (treeViewGroup.SelectedNode.Value.StartsWith("group_"))
            {
                ClearViewGroup();
                BtnDeleteGroup.Visible = true;
                pnlgroup.Visible = true;
                pnlDashboard.Visible = false;
                pnlgroup.GroupingText = "Edit Group View";
                var obj = treeViewGroup.SelectedNode.Text;
                treeViewGroup.SelectedNode.Selected = true;
                var lst = (View.ViewProperty as SuperDashboardsView).DashboardGroups.FirstOrDefault(x => x.DashboardGroup.ToLower() == obj.ToLower());
                txtname.Text = lst.DashboardGroup;
                txtwidth.Text = lst.Width.ToString();
                txtItemorder.Text = lst.ItemOrder.ToString();

                foreach (ListItem item in chkdashboard.Items)
                {
                    if ((lst.Dashboards.Exists(x => x.DashboardName == item.Value)))
                        item.Selected = true;
                    else
                        item.Selected = false;
                }

            }
            else if (treeViewGroup.SelectedNode.Value.StartsWith("dashboard_"))
            {
                ClearDashboard();
                BtnDeleteGroup.Visible = false;
                pnlgroup.Visible = false;
                pnlDashboard.Visible = true;

                dashboardId = treeViewGroup.SelectedNode.Value;
                var sboardname = treeViewGroup.SelectedNode.Value.Split(new char[] { '_' });
                dashboardname = sboardname[1];
                dashboardId = sboardname[2];
                dashboardType = sboardname[3];
                var lstDBGroupProp = (View.ViewProperty as SuperDashboardsView).DashboardGroups.FirstOrDefault(x => x.DashboardGroup.ToLower() == treeViewGroup.SelectedNode.Parent.Text.ToLower()).Dashboards.FirstOrDefault(m => m.DashboardName == treeViewGroup.SelectedNode.Text);
                if (lstDBGroupProp != null)
                {
                    txtheight.Text = lstDBGroupProp.Height.ToString();
                    txtorder.Text = lstDBGroupProp.ItemOrder.ToString();
                    chkinherit.Checked = !lstDBGroupProp.DisableInheritDefault;
                    txtDisplayName.Text = lstDBGroupProp.DisplayName;
                    txtFuIcon.SetImageUrl(lstDBGroupProp.IconUrl);
                }


            }
            else if (treeViewGroup.SelectedNode.Parent == null)
            {
                BtnDeleteGroup.Visible = false;
                pnlgroup.Visible = true;
                pnlDashboard.Visible = false;
                ClearViewGroup();
            }

        }
        /// <summary>
        ///To Edit the Dashboard property 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnEditProperty_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            List<DashboardGroupProperty> groups = new List<DashboardGroupProperty>();
            List<DashboardPanelProperty> dashboards;
            if (superView != null)
            {
                groups = superView.DashboardGroups;
            }
            else
            {
                superView = new SuperDashboardsView();
            }
            DashboardGroupProperty currentGroup = (View.ViewProperty as SuperDashboardsView).DashboardGroups.FirstOrDefault(x => x.DashboardGroup.ToLower() == treeViewGroup.SelectedNode.Parent.Text.ToLower());
            var dashboard = currentGroup.Dashboards.FirstOrDefault(m => m.DashboardName == treeViewGroup.SelectedNode.Text);
            dashboards = currentGroup.Dashboards;
            dashboards.Remove(dashboard);
            dashboard.DashboardName = treeViewGroup.SelectedNode.Text;

            int height = 0;
            int.TryParse(txtheight.Text.Trim(), out height);
            dashboard.Height = height;

            dashboard.Width = currentGroup.Width;
            dashboard.DisableInheritDefault = !chkinherit.Checked;
            int itemOrder = 0;
            int.TryParse(txtorder.Text.Trim(), out itemOrder);
            dashboard.ItemOrder = itemOrder;
            dashboard.DisplayName = txtDisplayName.Text.Trim();
            string uploaedFileURL = string.Empty;
            //if (fuIcon.HasFile)
            //{
            //    string fileName = string.Format("{0}_{1}{2}", System.IO.Path.GetFileNameWithoutExtension(fuIcon.PostedFile.FileName), Guid.NewGuid(), System.IO.Path.GetExtension(fuIcon.PostedFile.FileName));
            //    uploaedFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}", fileName);
            //    string path = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName);
            //    fuIcon.PostedFile.SaveAs(path);
            //}

            //if (uploaedFileURL != string.Empty)
            //{
            //    dashboard.IconUrl = uploaedFileURL;
            //    txtFuIcon.Text = dashboard.IconUrl;
            //}
            //else
            //{
            dashboard.IconUrl = txtFuIcon.GetImageUrl();
            //}



            dashboards.Add(dashboard);
            currentGroup.Dashboards = dashboards;
            superView.DashboardGroups = groups;
            View.ViewType = superViewName;


            objDashboardPanelViewManager.Save(View);
            hdnviewid.Value = View.ID.ToString();
        }
        /// <summary>
        ///To Clear control after saving or selecting treeView Node 
        /// </summary>
        private void ClearViewGroup()
        {
            lblchklstmsg.Text = "";
            txtname.Text = "";
            txtwidth.Text = "";
            txtItemorder.Text = "";
            foreach (ListItem item in chkdashboard.Items)
            {
                item.Selected = false;
            }
        }
        private void ClearDashboard()
        {
            txtheight.Text = string.Empty;
            txtorder.Text = string.Empty;
            chkinherit.Checked = false;
            txtDisplayName.Text = string.Empty;
            txtFuIcon.SetImageUrl(string.Empty);
        }
        /// <summary>
        ///To Edit Property of Dashboards  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnNewGroup_Click(object sender, EventArgs e)
        {
            BtnDeleteGroup.Visible = false;
            BtnDeleteGroup.Visible = false;
            pnlDashboard.Visible = false;
            pnlgroup.Visible = true;
            pnlgroup.GroupingText = "Add Group View";
            ClearViewGroup();
            //   TVgroup.SelectedNodeStyle.BackColor = System.Drawing.Color.Black;
        }
        /// <summary>
        ///Custom validator To check Existing  Group name 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void CustValGroupname_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (treeViewGroup.SelectedNode != null)
            {
                if (treeViewGroup.SelectedNode.Text.Trim().ToLower() != txtname.Text.Trim().ToLower())
                {
                    if (View != null)
                    {
                        superView = View.ViewProperty as SuperDashboardsView;
                        if (superView.DashboardGroups.Exists(t => t.DashboardGroup.ToLower() == txtname.Text.ToLower()))
                            args.IsValid = false;

                    }
                }
            }
            else
            {
                if (View != null)
                {
                    superView = View.ViewProperty as SuperDashboardsView;
                    if (superView.DashboardGroups.Exists(t => t.DashboardGroup.ToLower() == txtname.Text.ToLower()))
                        args.IsValid = false;

                }
            }

        }
        /// <summary>
        ///Custom validator To check Existing  View name 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void CustSuperver_ServerValidate(object source, ServerValidateEventArgs args)
        {

            if (View == null || View.ViewName.Trim().ToLower() != txtViewname.Text.Trim().ToLower())
            {
                var view = objDashboardPanelViewManager.LoadViewByName(txtViewname.Text.Trim());

                if (view != null)
                {
                    args.IsValid = false;
                }
            }


            //DashboardPanelView.Load(
        }
        protected void ImgDeleteView_Click(object sender, ImageClickEventArgs e)
        {
            if (View != null)
            {
                objDashboardPanelViewManager.Delete(View);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        protected void BtnDelelteDB_Click(object sender, EventArgs e)
        {
            if (treeViewGroup.SelectedNode == null)
            {
                return;
            }

            if (superView != null)
            {
                groups = superView.DashboardGroups;
            }

            string dashboardName = treeViewGroup.SelectedNode.Value.Replace("dashboard_", string.Empty);
            var sideGroupProp = groups.First(x => x.DashboardGroup.Trim().ToLower() == treeViewGroup.SelectedNode.Parent.Text.Trim().ToLower());
            var dbGroupProp = sideGroupProp.Dashboards.Find(x => x.DashboardName == dashboardName);
            sideGroupProp.Dashboards.Remove(dbGroupProp);
            //groups.Add(sideGroupProp);
            superView.DashboardGroups = groups;
            View.ViewType = superViewName;
            objDashboardPanelViewManager.Save(View);
            hdnviewid.Value = View.ID.ToString();
            BindTreeview();
            ClearDashboard();

            BtnNewGroup_Click(BtnNewGroup, new EventArgs());
        }

        protected void BtnDeleteGroup_Click(object sender, EventArgs e)
        {

            if (superView != null)
            {

                groups = superView.DashboardGroups;
            }
            var sideGroupProp = groups.First(x => x.DashboardGroup.Trim().ToLower() == txtname.Text.Trim().ToLower());
            groups.Remove(sideGroupProp);
            superView.DashboardGroups = groups;
            View.ViewType = superViewName;
            objDashboardPanelViewManager.Save(View);
            hdnviewid.Value = View.ID.ToString();
            BindTreeview();
            //ClearViewGroup();
            ClearViewGroup();
        }

        protected void FuIcon_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (fuIcon.HasFile)
            {
                if (!fuIcon.PostedFile.ContentType.StartsWith("image", StringComparison.CurrentCultureIgnoreCase))
                {
                    args.IsValid = false;
                    CustomValidator1.ErrorMessage = "Please upload image file(png, jpg, gif) only.";
                }
                else if (fuIcon.PostedFile.ContentLength > 2097152)
                {
                    args.IsValid = false;
                    CustomValidator1.ErrorMessage = "File size must be below 2MB.";
                }
            }
        }

        #region Dashboard Filter

        protected void DdlFilterFactTable_Load(object sender, EventArgs e)
        {
            if (ddlFilterFactTable.Items.Count <= 0)
            {
                List<string> factTables = DashboardCache.DashboardFactTables(HttpContext.Current.GetManagerContext());
                foreach (string fTable in factTables)
                {
                    ddlFilterFactTable.Items.Add(new ListItem(fTable, fTable));
                }
                FillFactTableFields(ddlFactTableFieldsForFilter, ddlFilterFactTable.SelectedValue, null);
            }
        }

        protected void DdlFilterFactTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillFactTableFields(ddlFactTableFieldsForFilter, ddlFilterFactTable.SelectedValue, null);
        }

        private void FillFactTableFields(object sender, string factTable, string typeFilter)
        {
            DropDownList list = (DropDownList)sender;
            list.Items.Clear();

            if (factTable != null && factTable.Trim() != string.Empty)
            {
                List<FactTableField> factTableFields = DashboardCache.GetFactTableFields(HttpContext.Current.GetManagerContext(), factTable.Trim());
                if (!string.IsNullOrEmpty(typeFilter))
                {
                    factTableFields = factTableFields.Where(x => x.DataType.ToLower() == "system.datetime").ToList();
                }

                foreach (FactTableField field in factTableFields)
                {
                    ListItem item = new ListItem(string.Format("{0}({1})", field.FieldName, field.DataType.Replace("System.", string.Empty)), field.FieldName);
                    item.Attributes.Add("datatype", field.DataType.Replace("System.", string.Empty));
                    list.Items.Add(item);
                }
                
                factTableFields.RemoveAll(x => x.FieldName.EndsWith("User$Id"));
            }
        }



        protected void TrDashboardFilter_SelectedNodeChanged(object sender, EventArgs e)
        {
            SuperDashboardsView indView = View.ViewProperty as SuperDashboardsView;
            Guid filterID = Guid.Empty;
            try
            {
                filterID = new Guid(trDashboardFilter.SelectedValue);
            }
            catch
            {
                filterID = Guid.Empty;
            }

            DashboardFilterProperty filter = indView.GlobalFilers.FirstOrDefault(x => x.ID == filterID);
            if (filter != null)
            {
                txtFilterTitle.Text = filter.Title;
                txtFilterItemOrder.Text = filter.ItemOrder.ToString();
                ddlFilterFactTable.SelectedIndex = ddlFilterFactTable.Items.IndexOf(ddlFilterFactTable.Items.FindByValue(filter.ListName));
                DdlFilterFactTable_SelectedIndexChanged(ddlFilterFactTable, new EventArgs());
                ddlFactTableFieldsForFilter.SelectedIndex = ddlFactTableFieldsForFilter.Items.IndexOf(ddlFactTableFieldsForFilter.Items.FindByValue(filter.ColumnName));
                hfEditFilter.Value = filter.ID.ToString();
                btAddNewFiler.Visible = true;
                btDeleteFilter.Visible = true;

                Session["filterListName"] = ddlFilterFactTable.SelectedValue;
                Session["filterListFieldName"] = ddlFactTableFieldsForFilter.SelectedValue;
                defaultValueData = null;
                glDefaultValue.DataBind();

                if (filter.DefaultValues != null)
                {
                    foreach (string v in filter.DefaultValues)
                    {
                        glDefaultValue.GridView.Selection.SelectRowByKey(v);
                    }
                }
            }

            else
            {
                ClearFilterForm();
            }
        }

        protected void BtSaveFilter_Click(object sender, EventArgs e)
        {
            SuperDashboardsView indView = View.ViewProperty as SuperDashboardsView;

            Guid filterID = Guid.Empty;
            try
            {
                filterID = new Guid(hfEditFilter.Value.Trim());
            }
            catch
            {
                filterID = Guid.Empty;
            }

            List<DashboardFilterProperty> filters = indView.GlobalFilers;
            if (filters == null)
            {
                filters = new List<DashboardFilterProperty>();
            }

            DashboardFilterProperty filter = filters.FirstOrDefault(x => x.ID == filterID);
            if (filter == null)
            {
                filter = new DashboardFilterProperty();
                filters.Add(filter);
            }

            filter.Title = txtFilterTitle.Text.Trim();
            int itemOrder = 0;
            int.TryParse(txtFilterItemOrder.Text.Trim(), out itemOrder);
            filter.ItemOrder = itemOrder;
            filter.ListName = ddlFilterFactTable.SelectedValue;
            filter.ColumnName = ddlFactTableFieldsForFilter.SelectedValue;
            filter.Hidden = chkHidden.Checked;
            filter.DefaultValues = new List<string>();
            string vals = Convert.ToString(glDefaultValue.Text);
            if (!string.IsNullOrWhiteSpace(vals))
                filter.DefaultValues = UGITUtility.ConvertStringToList(vals, ";");

            indView.GlobalFilers = filters;
            objDashboardPanelViewManager.Save(View);
            BuildFilterTree();
            ClearFilterForm();

        }

        protected void BtDeleteFilter_Click(object sender, EventArgs e)
        {
            SuperDashboardsView indView = View.ViewProperty as SuperDashboardsView;
            Guid filterID = Guid.Empty;
            try
            {
                filterID = new Guid(hfEditFilter.Value.Trim());
            }
            catch
            {
                filterID = Guid.Empty;
            }

            DashboardFilterProperty filter = indView.GlobalFilers.FirstOrDefault(x => x.ID == filterID);
            indView.GlobalFilers.Remove(filter);
            objDashboardPanelViewManager.Save(View);

        }

        protected void BtAddNewFiler_Click(object sender, EventArgs e)
        {
            ClearFilterForm();
            btAddNewFiler.Visible = false;
        }

        private void ClearFilterForm()
        {
            txtFilterTitle.Text = string.Empty;
            txtFilterItemOrder.Text = string.Empty;
            ddlFilterFactTable.ClearSelection();
            DdlFilterFactTable_SelectedIndexChanged(ddlFilterFactTable, new EventArgs());
            hfEditFilter.Value = string.Empty;
        }

        protected void BtCancelFilter_Click(object sender, EventArgs e)
        {
            ClearFilterForm();
            btAddNewFiler.Visible = false;
            btDeleteFilter.Visible = false;
        }

        protected void BuildFilterTree()
        {
            SuperDashboardsView indView = View.ViewProperty as SuperDashboardsView;

            trDashboardFilter.Nodes.Clear();
            trDashboardFilter.ShowLines = true;

            TreeNode root = null;
            TreeNode childRoot;
            root = new TreeNode();
            root.Text = "Global Filters";
            if (indView != null)
            {
                List<DashboardFilterProperty> filters = indView.GlobalFilers.OrderBy(x => x.ItemOrder).ToList();
                for (int i = 0; i < filters.Count; i++)
                {
                    childRoot = new TreeNode();
                    childRoot.Text = filters[i].Title;
                    childRoot.Value = filters[i].ID.ToString();
                    //childroot.ShowCheckBox = true;
                    root.ChildNodes.Add(childRoot);

                }
            }
            trDashboardFilter.Nodes.Add(root);
            trDashboardFilter.DataBind();
        }
        private void GridView_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Parameters))
                return;

            List<string> sParams = UGITUtility.ConvertStringToList(e.Parameters, "~");
            if (sParams.Count != 2)
                return;

            Session["filterListName"] = sParams[0];
            Session["filterListFieldName"] = sParams[1];
            glDefaultValue.Text = string.Empty;
            defaultValueData = null;
            glDefaultValue.DataBind();
        }

        protected void glDefaultValue_DataBinding(object sender, EventArgs e)
        {
            if (defaultValueData == null)
            {
                defaultValueData = LoadFilterDefaultValueData();
            }
            glDefaultValue.DataSource = defaultValueData;
        }

        DataTable LoadFilterDefaultValueData()
        {
            string filterListName = Convert.ToString(Session["filterListName"]);
            if (filterListName.Contains("-"))
            {
                filterListName = filterListName.Split('-')[0];
            }
            string filterListFieldName = Convert.ToString(Session["filterListFieldName"]);
            if (string.IsNullOrWhiteSpace(filterListName) || string.IsNullOrWhiteSpace(filterListFieldName))
                return null;

            DashboardFilterProperty filter = new DashboardFilterProperty();
            filter.ListName = filterListName;
            filter.ColumnName = filterListFieldName;
            string filterDataType = "String";
            DataTable filterValues = DevxChartHelper.GetDatatableForGlobalFilter(HttpContext.Current.GetManagerContext(), filter, ref filterDataType);

            if (DashboardHelper.IsFilterFieldRelatedToUser(HttpContext.Current.GetManagerContext(), filterListFieldName, filterListName))
            {
                DataRow row = filterValues.NewRow();
                row["Value"] = "[Me]";
                filterValues.Rows.InsertAt(row, 0);
            }

            return filterValues;
        }
        #endregion
    }
}
