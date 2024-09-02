using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager;

namespace uGovernIT.Web
{
    public partial class DashboardSideEditView : UserControl
    {
        List<Dashboard> listofDB = null;
        SideDashboardView sideView;
        List<DashboardSideProperty> lstSideLink = new List<DashboardSideProperty>();
        List<DashboardGroupProperty> groups = new List<DashboardGroupProperty>();
        DashboardViewProperties dbViewProp = new DashboardViewProperties();
        public DashboardPanelView View { get; set; }
        public String ViewType { get; set; }
        public string dashboardId;
        public string dashboardUrl;
        public string dashboardName;
        public string dashboardType;
        public string sideViewName = "Side Dashboards";
        private string absPath = string.Empty;
        DashboardManager dManager = new DashboardManager(HttpContext.Current.GetManagerContext());
        DashboardPanelViewManager objDashboardPanelViewManager = new DashboardPanelViewManager(HttpContext.Current.GetManagerContext());
        protected void Page_Load(object sender, EventArgs e)
        {
            dashboardUrl = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx");

            absPath = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.PickFromAsset);
            //lnkpickfromlibrary.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','900px','600px','','')", absPath, "Pick From Library"));
            
            absPath = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.PickFromAsset.Replace("pickSiteAsset","pickSiteAssetFortxtDBBackGround"));
            //lnkdbbackground.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','900px','600px','','')", absPath, "Pick From Library"));

            absPath = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.PickFromAsset.Replace("pickSiteAsset", "pickSiteAssetFortxtfuicon"));
           // lnkfuicon.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','900px','600px','','')", absPath, "Pick From Library"));

            listofDB = dManager.LoadAll(false);
            listofDB = listofDB.Where(x => x.DashboardType == DashboardType.Chart || x.DashboardType == DashboardType.Panel).OrderBy(x => x.DashboardType).ThenBy(x => x.Title).ToList();
            if (View == null)
            {
                BtnDeleteLink.Visible = false;
            }
            else
            {
                BtnDeleteLink.Visible = true;
            }

            if (!Page.IsPostBack)
            {
                if (View != null)
                {
                    btnDeleteView.Visible = true;
                    sideView = View.ViewProperty as SideDashboardView;
                    txtViewname.Text = View.ViewName;
                    txtViewWidth.Text = sideView.Width.ToString();
                    //SPFieldUserValueCollection collection = new SPFieldUserValueCollection();
                    //if (View.AuthorizedToView != null)
                    //{
                    //    View.AuthorizedToView.ForEach(x => { collection.Add(new SPFieldUserValue(SPContext.Current.Web, x.ID, x.Name)); });
                    //    peAuthorizedToView.UpdateEntities(uHelper.getUsersListFromCollection(collection));
                    //}
                    peAuthorizedToView.SetValues(View.AuthorizedToViewUsers);
                    //divMessage.Visible = false;
                    trMainPanel.Visible = true;
                    trMessage.Visible = false;
                    BindTreeview();
                    DefaultLinkBind();
                    
                   
                }
                else
                {
                    divMessage.InnerText = "Please create a view to add items.";
                    //divMessage.Visible = true;
                    trMainPanel.Visible = false;
                    trMessage.Visible = true;
                }
               
            }
            else
            {
                //dvDashboard.Visible = true;
                //dvView.Visible = false;
                if (View != null)
                {
                    sideView = View.ViewProperty as SideDashboardView;
                    btnDeleteView.Visible = true;
                }
                if (View == null)
                {
                    int viewID;
                    Int32.TryParse(hdnviewid.Value, out viewID);
                    if (viewID > 0)
                    {
                        View = objDashboardPanelViewManager.LoadViewByID(viewID);
                        if (View != null)
                            sideView = View.ViewProperty as SideDashboardView;
                    }
                }
            }

        }

        public void DefaultLinkBind()
        {
            List<DashboardSideProperty> lstSideProperty = (View.ViewProperty as SideDashboardView).DashboardSideList;
            //pnlsideView.Visible = true;
            //pnlgroup.Visible = false;
            //pnlDashboard.Visible = false;
            //pnlsideView.GroupingText = "Edit Link ";
            var obj = treeViewGroup.Nodes[0].ChildNodes.Count > 0 ? treeViewGroup.Nodes[0].ChildNodes[0].Text : treeViewGroup.Nodes[0].Text;
            treeViewGroup.Nodes[0].Selected = true; //ChildNodes[0].
            var lstLinkproperty = lstSideProperty.FirstOrDefault(x => x.Title.ToLower() == obj.ToLower());
            if (lstLinkproperty != null)
            {
                txtTitle.Text = lstLinkproperty.Title;
                txtDescription.Text = lstLinkproperty.Description;
                txtLinkHeight.Text = lstLinkproperty.Height.ToString();
                txtLinkOrder.Text = lstLinkproperty.ItemOrder.ToString();
                txtUrl.Text = lstLinkproperty.LinkUrl;                
                chkHideTitle.Checked = lstLinkproperty.IsHideTitle;
                txtfileupload.SetImageUrl(lstLinkproperty.Imapgepath);
            }

        }

        protected void BtnSaveLink_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (View == null)
            {
                View = new DashboardPanelView();
                View.Title = txtViewname.Text.Trim();
                View.ViewName = txtViewname.Text.Trim();
            }
          
            if (sideView != null)
            {
                groups = sideView.DashboardGroups;
                lstSideLink = sideView.DashboardSideList;
            }
            else
            {
                sideView = new SideDashboardView();
            }
            View.ViewProperty = sideView;
            string listName = txtTitle.Text;

            if (treeViewGroup.SelectedNode != null)
                if (treeViewGroup.SelectedNode.ChildNodes.Count == 0 && treeViewGroup.SelectedNode.Parent != null)
                {
                    listName = treeViewGroup.SelectedNode.Text;
                }
                else
                {
                    listName = txtTitle.Text;
                }

            var currentList = lstSideLink.FirstOrDefault(x => x.Title == listName);
            if (currentList == null)
            {
                currentList = new DashboardSideProperty();

            }

            //string uploaedFileURL = string.Empty;
            //if (fileupload.HasFile)
            //{
            //    string fileName = string.Format("{0}_{1}{2}", System.IO.Path.GetFileNameWithoutExtension(fileupload.PostedFile.FileName), Guid.NewGuid(), System.IO.Path.GetExtension(fileupload.PostedFile.FileName));
            //    uploaedFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}", fileName);
            //    string path = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName);
            //    fileupload.PostedFile.SaveAs(path);
            //}

            View.ViewName = txtViewname.Text.Trim();
            double width = 0;
            double.TryParse(txtViewWidth.Text.Trim(), out width);
            sideView.Width = width;

            currentList.NavigationType = UGITUtility.StringToShort(rbLinkNavigationType.SelectedValue);
            currentList.Title = txtTitle.Text.Trim();
            currentList.Description = txtDescription.Text.Trim();
            currentList.Height = Convert.ToInt32(txtLinkHeight.Text.Trim());
            //if (uploaedFileURL != string.Empty)
            //{
            //    currentList.Imapgepath = uploaedFileURL;
            //    txtfileupload.Text = currentList.Imapgepath;
            //}
            //else
            //{
                currentList.Imapgepath = txtfileupload.GetImageUrl();
            //}

            // default value
            currentList.IsFlat = true; // chkLayout.Checked;

            int itemOrder;
            int.TryParse(txtLinkOrder.Text.Trim(), out itemOrder);
            currentList.ItemOrder = itemOrder;
            currentList.LinkUrl = txtUrl.Text.Trim();
            currentList.IsHideTitle = chkHideTitle.Checked;
            if (!lstSideLink.Contains(currentList))
                lstSideLink.Add(currentList);
            sideView.DashboardGroups = groups;
            sideView.DashboardSideList = lstSideLink;
            View.ViewType = sideViewName;
            objDashboardPanelViewManager.Save(View);
            hdnviewid.Value = View.ID.ToString();
            BindTreeview();
            ClearLinkDetails();
            pnlsideView.Visible = false;
        }

        private void BindTreeview()
        {
            DataTable table = CreateTable();
            treeViewGroup.Nodes.Clear();
            TreeNode root = null;
            TreeNode childNode;
            TreeNode childRoot;
            txtViewname.Text = View.ViewName;

            root = new TreeNode();
            root.Text = View.ViewName;

            if (sideView != null && sideView.DashboardSideList != null && sideView.DashboardSideList.Count > 0)
            {
                var sideLinkList = sideView.DashboardSideList.OrderBy(x => x.ItemOrder).ToList();

                for (int i = 0; sideLinkList.Count > i; i++)
                {
                    DataRow dr = table.NewRow();
                    dr[0] = sideLinkList[i].ItemOrder;
                    dr[1] = sideLinkList[i].Title;
                    dr[2] = "link";
                    table.Rows.Add(dr);
                }
            }

            if (sideView != null && sideView.Dashboards != null && sideView.Dashboards.Count > 0)
            {
                var sideLinkList = sideView.Dashboards.OrderBy(x => x.ItemOrder).ToList();

                for (int i = 0; sideLinkList.Count > i; i++)
                {
                    DataRow dr = table.NewRow();
                    dr[0] = sideLinkList[i].ItemOrder;
                    dr[1] = sideLinkList[i].DashboardName;                    
                    dr[2] = "dashboard";                     
                    table.Rows.Add(dr);
                }
            }

            var dbGroupList = sideView.DashboardGroups;
            if (sideView != null && sideView.DashboardGroups != null && sideView.DashboardGroups.Count > 0)
            {
            dbGroupList = sideView.DashboardGroups.OrderBy(x => x.ItemOrder).ToList();
            for (int j = 0; dbGroupList.Count > j; j++)
            {
                DataRow dr = table.NewRow();
                dr[0] = dbGroupList[j].ItemOrder;
                dr[1] = dbGroupList[j].DashboardGroup;
                dr[2] = "group";
                table.Rows.Add(dr);
            }
            }
            var view = table.DefaultView;
            view.Sort = "ID asc";
            table = view.ToTable();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i]["Type"].ToString() == "link")
                {
                    childRoot = new TreeNode();
                    childRoot.Text = Convert.ToString(table.Rows[i]["Title"]);
                    childRoot.Value = string.Format("{0}_{1}", table.Rows[i]["Type"], table.Rows[i]["Title"]);
                    root.ChildNodes.Add(childRoot);
                }
                else if (table.Rows[i]["Type"].ToString() == "group")
                {
                    childRoot = new TreeNode();
                    childRoot.Text = Convert.ToString(table.Rows[i]["Title"]);
                    childRoot.Value = string.Format("{0}_{1}", table.Rows[i]["Type"], table.Rows[i]["Title"]);
                    root.ChildNodes.Add(childRoot);
                    var dbGroupProp = dbGroupList.First(x => x.DashboardGroup == table.Rows[i]["Title"].ToString());
                    dbGroupProp.Dashboards = dbGroupProp.Dashboards.OrderBy(x => x.ItemOrder).ToList();
                    for (int j = 0; j < dbGroupProp.Dashboards.Count; j++)
                    {
                        Dashboard dashboard = listofDB.FirstOrDefault(x => x.Title == dbGroupProp.Dashboards[j].DashboardName);
                        if (dashboard != null)
                        {
                            childNode = new TreeNode();
                            childNode.Value = dbGroupProp.Dashboards[j].DashboardName;                           
                            childNode.Text = dashboard.Title;
                            childNode.Value = "dashboard_" + dashboard.Title ;
                            childRoot.ChildNodes.Add(childNode);
                        }
                    }

                }
                else if (table.Rows[i]["Type"].ToString() == "dashboard")
                {
                    childRoot = new TreeNode();
                    childRoot.Text = Convert.ToString(table.Rows[i]["Title"]);
                    childRoot.Value = string.Format("{0}_{1}", table.Rows[i]["Type"], table.Rows[i]["Title"]);
                    root.ChildNodes.Add(childRoot);
                }
            }

            //divMessage.Visible = true;
            //if (root != null && root.ChildNodes.Count == 0)
            //{
            //    divMessage.InnerText = string.Format("No items added on {0}.",root.Text);
               
            //}
            //else
            //{
            //    if (pnlDashboard.Visible == false && pnlgroup.Visible == false && pnlsideView.Visible == false)
            //    {
            //        divMessage.InnerText = "Please either add or edit items by using respective actions.";
            //    }
            //    else
            //    {
            //        divMessage.Visible = false;
            //    }
            //}
            treeViewGroup.Nodes.Add(root);
            treeViewGroup.DataBind();


        }

        protected void TreeViewGroup_SelectedNodeChanged(object sender, EventArgs e)
        {
            List<DashboardSideProperty> lstSideProperty = (View.ViewProperty as SideDashboardView).DashboardSideList;
            string seletedNode = treeViewGroup.SelectedNode.Text;
            if (treeViewGroup.SelectedNode == null)
            {
                //pnlsideView.GroupingText = "New Link";
                //pnlgroup.Visible = false;
                //pnlDashboard.Visible = false;
                //pnlsideView.Visible = true;
                //BtnDeleteLink.Visible = false;
                //ClearLinkDetails();

                return;
            }


            if (treeViewGroup.SelectedNode.Value.StartsWith("group_"))
            {
                BindDBList();
                pnlgroup.Visible = true;
                pnlDashboard.Visible = false;
                pnlsideView.Visible = false;
                BtnDeleteGroup.Visible = true;
                pnlgroup.GroupingText = "Edit Group View";
                treeViewGroup.SelectedNode.Selected = true;
                var dbGroupList = (View.ViewProperty as SideDashboardView).DashboardGroups.FirstOrDefault(x => x.DashboardGroup.ToLower() == seletedNode.ToLower());
                txtGroupname.Text = dbGroupList.DashboardGroup;
                txtItemorder.Text = dbGroupList.ItemOrder.ToString();
              
                foreach (ListItem item in chkdashboard.Items)
                {
                    if ((dbGroupList.Dashboards.Exists(x => x.DashboardName == item.Value)))
                        item.Selected = true;
                    else
                        item.Selected = false;
                }
                BtnDeleteGroup.Visible = true;
            }
            else if (treeViewGroup.SelectedNode.Value.StartsWith("link_"))
            {
                pnlgroup.Visible = false;
                pnlDashboard.Visible = false;
                pnlsideView.Visible = true;
                BtnDeleteLink.Visible = true;
                ClearLinkDetails();

                pnlsideView.GroupingText = "Edit Link";
                var lstLinkproperty = lstSideProperty.FirstOrDefault(x => x.Title == treeViewGroup.SelectedNode.Text);
                txtTitle.Text = lstLinkproperty.Title;
                txtDescription.Text = lstLinkproperty.Description;
                txtLinkHeight.Text = lstLinkproperty.Height.ToString();
                txtLinkOrder.Text = lstLinkproperty.ItemOrder.ToString();
                txtUrl.Text = lstLinkproperty.LinkUrl;
                txtfileupload.SetImageUrl(lstLinkproperty.Imapgepath);
                chkHideTitle.Checked = lstLinkproperty.IsHideTitle;
                rbLinkNavigationType.SelectedIndex = rbLinkNavigationType.Items.IndexOf(rbLinkNavigationType.Items.FindByValue(lstLinkproperty.NavigationType.ToString()));

            }
            else if (treeViewGroup.SelectedNode.Value.StartsWith("dashboard_"))
            {
                BindRadioButtonDBList();
                pnlgroup.Visible = false;
                pnlsideView.Visible = false;
                pnlDashboard.Visible = true;
                btnSaveDashBoard.Visible = false;

                DashboardGroupProperty groupProperty = (View.ViewProperty as SideDashboardView).DashboardGroups.FirstOrDefault(x => x.DashboardGroup.ToLower() == treeViewGroup.SelectedNode.Parent.Text.ToLower() && treeViewGroup.SelectedNode.Parent.Value.StartsWith("group_"));
                if (groupProperty != null)
                {
                    var lstDBGroupProp = groupProperty.Dashboards.FirstOrDefault(m => m.DashboardName == treeViewGroup.SelectedNode.Text);
                    Dashboard dashboard = listofDB.FirstOrDefault(x => x.Title == lstDBGroupProp.DashboardName);


                    txtheight.Text = lstDBGroupProp.Height.ToString();
                    txtorder.Text = lstDBGroupProp.ItemOrder.ToString();
                    txtDisplayName.Text = lstDBGroupProp.DisplayName;
                    txtFuIcon.SetImageUrl(lstDBGroupProp.IconUrl);
                    txtDBBackGround.SetImageUrl(lstDBGroupProp.BackGroundUrl);
                    ddlDashBoardList.SelectedValue = treeViewGroup.SelectedNode.Text;
                    txtDashboardUrl.Text = lstDBGroupProp.DashboardUrl;
                    rbNavigationList.SelectedIndex = rbNavigationList.Items.IndexOf(rbNavigationList.Items.FindByValue(lstDBGroupProp.NavigationType.ToString()));
                    trDBBackGround.Visible = false;
                    trDBBackgroundImage.Visible = false;
                    trDBIcon.Visible = false;
                    trSelectDashBoard.Visible = false;

                }
                else
                {
                    DashboardPanelProperty dbPanelProperty = (View.ViewProperty as SideDashboardView).Dashboards.FirstOrDefault(x => x.DashboardName.ToLower() == treeViewGroup.SelectedNode.Text.ToLower());

                    if (dbPanelProperty != null)
                    {

                        ddlDBBackGround.Items[ddlDBBackGround.SelectedIndex].Selected = false;
                        ddlDBBackGround.SelectedIndex = ddlDBBackGround.Items.IndexOf(ddlDBBackGround.Items.FindByText(dbPanelProperty.Theme));
                        ddlDBBackGround_SelectedIndexChanged(ddlDBBackGround, e);
                        trDBBackGround.Visible = true;
                        trSelectDashBoard.Visible = true;
                        trDBIcon.Visible = true;
                        ddlDashBoardList.SelectedValue = treeViewGroup.SelectedNode.Text; // MUST be before ddlDashBoardList_SelectedIndexChanged call
                        ddlDashBoardList_SelectedIndexChanged(ddlDashBoardList, e);
                        txtheight.Text = dbPanelProperty.Height.ToString();
                        txtorder.Text = dbPanelProperty.ItemOrder.ToString();
                        txtDisplayName.Text = dbPanelProperty.DisplayName;
                        txtFuIcon.SetImageUrl(dbPanelProperty.IconUrl);
                        txtDBBackGround.SetImageUrl(dbPanelProperty.BackGroundUrl);
                        txtDashboardUrl.Text = dbPanelProperty.DashboardUrl;
                        rbNavigationList.SelectedIndex = rbNavigationList.Items.IndexOf(rbNavigationList.Items.FindByValue(dbPanelProperty.NavigationType.ToString()));
                    }
                }

                BtnEditProperty.Visible = true;
                btnSaveDashBoard.Visible = false;
                BtnDelelteDB.Visible = true;

            }
            else if (treeViewGroup.SelectedNode.Parent == null)
            {
                pnlgroup.Visible = false;
                pnlDashboard.Visible = false;
                pnlsideView.Visible = false;
                BtnDeleteLink.Visible = false;
            }
           
        }

        public void ClearLinkDetails()
        {
            txtTitle.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtUrl.Text = string.Empty;
            txtLinkHeight.Text = string.Empty;
            txtLinkOrder.Text = string.Empty;            
            txtfileupload.SetImageUrl(string.Empty);
            chkHideTitle.Checked = false;
            
        }

        protected void BtnNewLink_Click(object sender, EventArgs e)
        {
            if (View == null)
            {
                return;
            }
            BtnDeleteLink.Visible = false;
            pnlsideView.Visible = true;
            pnlgroup.Visible = false;
            pnlDashboard.Visible = false;
            //divMessage.Visible = false;
            pnlsideView.GroupingText = "New Link";
            ClearLinkDetails();
            if (treeViewGroup.SelectedNode != null)
                treeViewGroup.SelectedNode.Selected = false;
        }

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
        }

        protected void CustValtxtTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (treeViewGroup.SelectedNode != null)
            {
                if (treeViewGroup.SelectedNode.Text.Trim().ToLower() != txtTitle.Text.Trim().ToLower())
                {
                    if (View != null)
                    {
                        sideView = View.ViewProperty as SideDashboardView;
                        if (sideView.DashboardSideList.Exists(t => t.Title.ToLower() == txtTitle.Text.ToLower()))
                            args.IsValid = false;
                    }
                }
            }
            else
            {
                if (View != null)
                {
                    sideView = View.ViewProperty as SideDashboardView;
                    if (sideView.DashboardSideList.Exists(t => t.Title.ToLower() == txtTitle.Text.ToLower()))
                        args.IsValid = false;

                }
            }
        }

        protected void BtnAddnewGroup_Click(object sender, EventArgs e)
        {
            if (View == null)
                return;
            ClearViewGroup();
            BtnDeleteGroup.Visible = false;
            pnlgroup.Visible = true;
            pnlsideView.Visible = false;
            pnlDashboard.Visible = false;
            //divMessage.Visible = false;
            BindDBList();
        }
       
        protected void BtnSaveView_Click(object sender, EventArgs e)
        {
            if (View == null)
            {
                View = new DashboardPanelView();
                View.Title = txtViewname.Text.Trim();
                View.ViewName = txtViewname.Text.Trim();
            }
            else
            {
                View.ViewName = txtViewname.Text.Trim();
                View.Title = txtViewname.Text.Trim();
                View.ViewName = txtViewname.Text.Trim();
            }

          
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
            if (sideView != null)
            {
                groups = sideView.DashboardGroups;
                lstSideLink = sideView.DashboardSideList;
            }
            else
            {
                sideView = new SideDashboardView();
            }

            double width = 0;
            double.TryParse(txtViewWidth.Text.Trim(), out width);
            sideView.Width = width;

            View.ViewProperty = sideView;
            View.ViewType = this.ViewType;
         

            objDashboardPanelViewManager.Save(View);
            hdnviewid.Value = View.ID.ToString();
            trMainPanel.Visible = true;
            trMessage.Visible = false;
            BindTreeview();
          

        }

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
                if (sideView != null)
                {
                    groups = sideView.DashboardGroups;
                }
                else
                {
                    sideView = new SideDashboardView();
                }

                View.ViewProperty = sideView;
                string name = string.Empty;
                if (treeViewGroup.SelectedNode != null)
                    if (treeViewGroup.SelectedNode.ChildNodes.Count > 0 && treeViewGroup.SelectedNode.Parent != null)
                    {
                        name = treeViewGroup.SelectedNode.Text;
                    }
                    else
                    {
                        name = txtGroupname.Text;
                    }
                DashboardGroupProperty currentGroup = groups.FirstOrDefault(x => x.DashboardGroup.ToLower() == name.ToLower());
                if (currentGroup == null)
                {
                    currentGroup = new DashboardGroupProperty();
                    groups.Add(currentGroup);
                }
                List<DashboardPanelProperty> lstDBPanlProp = new List<DashboardPanelProperty>();
                View.ViewName = txtViewname.Text.Trim();
                double width = 0;
                double.TryParse(txtViewWidth.Text.Trim(), out width);
                sideView.Width = width;
                currentGroup.Width = Convert.ToInt32(width);
                int itemOrder;
                int.TryParse(txtItemorder.Text, out itemOrder);

                currentGroup.ItemOrder = itemOrder;
                currentGroup.DashboardGroup = txtGroupname.Text;              
                string uploaedFileURL = string.Empty;
                string uploadedBackgroundIcon = string.Empty;

                if (currentGroup.Dashboards == null)
                    currentGroup.Dashboards = new List<DashboardPanelProperty>();
             
                DashboardPanelProperty dashboard = null;
                foreach (ListItem lstItem in chkdashboard.Items)
                {
                    if (lstItem.Selected)
                    {
                        Dashboard uDashboard = listofDB.FirstOrDefault(x => x.Title == lstItem.Value);
                        DashboardPanelProperty panelProp = currentGroup.Dashboards.FirstOrDefault(x => x.DashboardName.ToLower() == lstItem.Value.ToLower());
                        if (panelProp != null)
                        {
                            lstDBPanlProp.Add(panelProp);
                        }
                        else if (uDashboard != null)
                        {
                            dashboard = new DashboardPanelProperty();
                          
                            dashboard.DashboardName = uDashboard.Title;
                            dashboard.DisplayName = uDashboard.panel.ContainerTitle;
                            dashboard.Height = uDashboard.PanelHeight;
                            dashboard.ItemOrder = uDashboard.ItemOrder;
                            dashboard.Theme = uDashboard.ThemeColor;

                            lstDBPanlProp.Add(dashboard);
                            dashboard = null;
                        }
                    }
                }

                currentGroup.Dashboards= lstDBPanlProp.OrderBy(m=>m.ItemOrder).ToList();
                sideView.DashboardGroups = groups;
                View.ViewType = sideViewName;
                objDashboardPanelViewManager.Save(View);
                hdnviewid.Value = View.ID.ToString();
                BindTreeview();
                ClearViewGroup();
                pnlgroup.Visible = false;

            }
            else
            {
                lblchklstmsg.Text = "Please select at least one dashboard.";
            }


        }

        public void BindDBList()
        {
            chkdashboard.DataSource = listofDB.OrderBy(x => x.Title).ToList();
            chkdashboard.DataTextField = "Title";
            chkdashboard.DataValueField = "Title";
            chkdashboard.DataBind();
        }

        public void BindRadioButtonDBList()
        {

            ddlDashBoardList.DataSource = listofDB.OrderBy(x => x.Title).ToList();
            ddlDashBoardList.DataTextField = "Title";
            ddlDashBoardList.DataValueField = "Title";
            ddlDashBoardList.DataBind();
        }       

        protected void btnDashBoard_Click(object sender, EventArgs e)
        {
            if (View == null)
            {
                return;
            }
            ClearViewGroup();
            ClearDashboard();
            BindRadioButtonDBList();           
            BtnEditProperty.Visible = false;
            btnSaveDashBoard.Visible = true;
            pnlDashboard.Visible = true;
            pnlgroup.Visible = false;
            pnlsideView.Visible = false;
            trDBIcon.Visible = true;
            BtnDelelteDB.Visible = false;
            trSelectDashBoard.Visible = true;
            ddlDashBoardList_SelectedIndexChanged(ddlDashBoardList, e);
         
        }        
       
        private void ClearViewGroup()
        {
            lblchklstmsg.Text = string.Empty;
            txtGroupname.Text = string.Empty;
            txtItemorder.Text = string.Empty;
           
            if (treeViewGroup.SelectedNode != null)
            {
                treeViewGroup.SelectedNode.Selected = false;
            }
            foreach (ListItem item in chkdashboard.Items)
            {
                item.Selected = false;
            }
        }

        protected void CustValGroupname_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (treeViewGroup.SelectedNode != null)
            {
                if (treeViewGroup.SelectedNode.Text.Trim().ToLower() != txtGroupname.Text.Trim().ToLower())
                {
                    if (View != null)
                    {
                        sideView = View.ViewProperty as SideDashboardView;
                        if (sideView.DashboardGroups.Exists(t => t.DashboardGroup.ToLower() == txtGroupname.Text.ToLower()))
                            args.IsValid = false;

                    }
                }
            }
            else
            {
                if (View != null)
                {
                    sideView = View.ViewProperty as SideDashboardView;
                    if (sideView.DashboardGroups.Exists(t => t.DashboardGroup.ToLower() == txtGroupname.Text.ToLower()))
                        args.IsValid = false;

                }
            }
        }

        protected void btnSaveDashBoard_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            SaveNewDashBoard();
            ClearDashboard();
            pnlDashboard.Visible = false;
            BindTreeview();
            
        }

        private void SaveNewDashBoard()
        {
            List<DashboardPanelProperty> dashboards = new List<DashboardPanelProperty>();
            if (sideView != null)
            {
                dashboards = sideView.Dashboards;
            }
            else
            {
                sideView = new SideDashboardView();
            }

            DashboardPanelProperty dashboard = new DashboardPanelProperty();
            dashboard.DashboardName = ddlDashBoardList.SelectedValue;// treeViewGroup.SelectedNode.Text;
            dashboard.Height = Convert.ToInt32(txtheight.Text.Trim());
            dashboard.Width =  Convert.ToInt32(sideView.Width);           
            dashboard.DisableInheritDefault = true;
            int itemOrder;
            int.TryParse(txtorder.Text.Trim(), out itemOrder);
            dashboard.ItemOrder = itemOrder;
            dashboard.DisplayName = txtDisplayName.Text.Trim();
            dashboard.Theme = ddlDBBackGround.SelectedItem.Text.Trim();
            dashboard.DashboardUrl = txtDashboardUrl.Text.Trim();

            dashboard.NavigationType = UGITUtility.StringToShort(rbNavigationList.SelectedValue);
            switch (ddlDBBackGround.SelectedValue)
            {
                case "0":

                    dashboard.IconUrl = txtFuIcon.GetImageUrl();
                    txtDBBackGround.SetImageUrl(string.Empty);
                    break;

                case "1":
                    dashboard.IconUrl = txtFuIcon.GetImageUrl();
                    dashboard.BackGroundUrl = txtDBBackGround.GetImageUrl();
                    break;


                default:
                    dashboard.BackGroundUrl = string.Empty;
                    dashboard.IconUrl = string.Empty;
                    break;
            }

            string uploaedFileURL = string.Empty;
            if (fuDBBackGround.HasFile)
            {
                string fileName = string.Format("{0}_{1}{2}", System.IO.Path.GetFileNameWithoutExtension(fuDBBackGround.PostedFile.FileName), Guid.NewGuid(), System.IO.Path.GetExtension(fuDBBackGround.PostedFile.FileName));
                uploaedFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}", fileName);
                string path = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName);
                fuDBBackGround.PostedFile.SaveAs(path);
                dashboard.BackGroundUrl = uploaedFileURL;
                txtDBBackGround.SetImageUrl(dashboard.BackGroundUrl);
            }

           // string uploaedFileURL = string.Empty;
            if (fuIcon.HasFile)
            {
                string fileName = string.Format("{0}_{1}{2}", System.IO.Path.GetFileNameWithoutExtension(fuIcon.PostedFile.FileName), Guid.NewGuid(), System.IO.Path.GetExtension(fuIcon.PostedFile.FileName));
                uploaedFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}", fileName);
                string path = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName);
                fuIcon.PostedFile.SaveAs(path);
                dashboard.IconUrl = uploaedFileURL;
                txtFuIcon.SetImageUrl(dashboard.IconUrl);
            }

            sideView.Dashboards.Add(dashboard);
            View.ViewType = sideViewName;
            objDashboardPanelViewManager.Save(View);
            hdnviewid.Value = View.ID.ToString();
        }

        protected void BtnEditProperty_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            List<DashboardGroupProperty> groups = new List<DashboardGroupProperty>();
            List<DashboardPanelProperty> dashboards;
            if (sideView != null)
            {
                groups = sideView.DashboardGroups;
            }
            else
            {
                sideView = new SideDashboardView();
            }

            DashboardGroupProperty currentGroup = null;

            if (treeViewGroup.SelectedNode != null)
            {
                currentGroup = (View.ViewProperty as SideDashboardView).DashboardGroups.FirstOrDefault(x => x.DashboardGroup.ToLower() == treeViewGroup.SelectedNode.Parent.Text.ToLower() && treeViewGroup.SelectedNode.Parent.Value.StartsWith("group_"));
            }


            if (currentGroup != null)
            {
                var dashboard = currentGroup.Dashboards.FirstOrDefault(m => m.DashboardName == treeViewGroup.SelectedNode.Text);
                dashboards = currentGroup.Dashboards;
                dashboards.Remove(dashboard);
                dashboard.DashboardName = treeViewGroup.SelectedNode.Text;
                dashboard.Height = Convert.ToInt32(txtheight.Text.Trim());
                dashboard.Width = currentGroup.Width;
                dashboard.DisableInheritDefault = true;
                int itemOrder;
                int.TryParse(txtorder.Text.Trim(), out itemOrder);
                dashboard.ItemOrder = itemOrder;
                dashboard.DisplayName = txtDisplayName.Text.Trim();
                dashboard.DashboardUrl = txtDashboardUrl.Text.Trim();
                dashboard.NavigationType = UGITUtility.StringToShort(rbNavigationList.SelectedValue);
                string uploaedFileURL = string.Empty;
                if (fuIcon.HasFile)
                {
                    string fileName = string.Format("{0}_{1}{2}", System.IO.Path.GetFileNameWithoutExtension(fuIcon.PostedFile.FileName), Guid.NewGuid(), System.IO.Path.GetExtension(fuIcon.PostedFile.FileName));
                    uploaedFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}", fileName);
                    string path = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName);
                    fuIcon.PostedFile.SaveAs(path);
                }

                if (uploaedFileURL != string.Empty)
                {
                    dashboard.IconUrl = uploaedFileURL;
                    txtFuIcon.SetImageUrl(dashboard.IconUrl);
                }
                else
                {
                    dashboard.IconUrl = txtFuIcon.GetImageUrl();
                }
                dashboards.Add(dashboard);
                currentGroup.Dashboards = dashboards;
                sideView.DashboardGroups = groups;
            }
            else
            {
                DashboardPanelProperty dashboard = (View.ViewProperty as SideDashboardView).Dashboards.FirstOrDefault(x => x.DashboardName.ToLower() == treeViewGroup.SelectedNode.Text.ToLower());

                dashboards = (View.ViewProperty as SideDashboardView).Dashboards;
                dashboards.Remove(dashboard);
                dashboard.DashboardName = treeViewGroup.SelectedNode.Text;
                dashboard.Height = Convert.ToInt32(txtheight.Text.Trim());
                dashboard.Width = Convert.ToInt32(sideView.Width);
                dashboard.DisableInheritDefault = true;
                int itemOrder;
                int.TryParse(txtorder.Text.Trim(), out itemOrder);
                dashboard.ItemOrder = itemOrder;
                dashboard.DisplayName = txtDisplayName.Text.Trim();
                dashboard.Theme = ddlDBBackGround.SelectedItem.Text.Trim();
                dashboard.DashboardUrl = txtDashboardUrl.Text.Trim();
                dashboard.NavigationType = UGITUtility.StringToShort(rbNavigationList.SelectedValue);
                switch (ddlDBBackGround.SelectedValue)
                {
                    case "0":
                      
                        dashboard.IconUrl = txtFuIcon.GetImageUrl();
                        //txtDBBackGround.Text = string.Empty;
                        break;

                    case "1":
                        dashboard.IconUrl = txtFuIcon.GetImageUrl();                        
                        dashboard.BackGroundUrl = txtDBBackGround.GetImageUrl();
                        break;
                 

                    default:
                        dashboard.BackGroundUrl = string.Empty;
                        dashboard.IconUrl = string.Empty;
                        break;
                }


                //string uploaedFileURL = string.Empty;
                
                //if (fuDBBackGround.HasFile)
                //{
                //    string fileName = string.Format("{0}_{1}{2}", System.IO.Path.GetFileNameWithoutExtension(fuDBBackGround.PostedFile.FileName), Guid.NewGuid(), System.IO.Path.GetExtension(fuDBBackGround.PostedFile.FileName));
                //    uploaedFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}", fileName);
                //    string path = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName);
                //    fuDBBackGround.PostedFile.SaveAs(path);
                //    dashboard.BackGroundUrl = uploaedFileURL;
                //    txtDBBackGround.Text = dashboard.BackGroundUrl;
                //}

                //if (fuIcon.HasFile)
                //{
                //    string fileName = string.Format("{0}_{1}{2}", System.IO.Path.GetFileNameWithoutExtension(fuIcon.PostedFile.FileName), Guid.NewGuid(), System.IO.Path.GetExtension(fuIcon.PostedFile.FileName));
                //    uploaedFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}", fileName);
                //    string path = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName);
                //    fuIcon.PostedFile.SaveAs(path);
                //    dashboard.IconUrl = uploaedFileURL;
                //    txtFuIcon.Text = dashboard.IconUrl;
                //}

                dashboards.Add(dashboard);
                sideView.Dashboards = dashboards;
            }

           
           
            View.ViewType = sideViewName;
            objDashboardPanelViewManager.Save(View);
            hdnviewid.Value = View.ID.ToString();
            ClearDashboard();
            pnlDashboard.Visible = false;
            pnlgroup.Visible = false;
           
            BindTreeview();
        }

        public DataTable CreateTable()
        {
            DataTable table = new DataTable();
            DataColumn col1 = new DataColumn(DatabaseObjects.Columns.Id, typeof(Int32));
            DataColumn col2 = new DataColumn(DatabaseObjects.Columns.Title, typeof(String));
            DataColumn col3 = new DataColumn("Type", typeof(String));
            table.Columns.Add(col1);
            table.Columns.Add(col2);
            table.Columns.Add(col3);
            return table;
        }

        protected void BtnDeleteLink_Click(object sender, EventArgs e)
        {

            if (sideView != null)
            {
                lstSideLink = sideView.DashboardSideList;
                groups = sideView.DashboardGroups;
            }
            var sideLinkprop = lstSideLink.First(x => x.Title == txtTitle.Text);
            lstSideLink.Remove(sideLinkprop);
            sideView.DashboardGroups = groups;
            sideView.DashboardSideList = lstSideLink;
            View.ViewType = sideViewName;
            objDashboardPanelViewManager.Save(View);
            hdnviewid.Value = View.ID.ToString();
            pnlsideView.Visible = false;
            BindTreeview();           
            ClearLinkDetails();          
        }

        protected void BtnDelelteDB_Click(object sender, EventArgs e)
        {

            if (sideView != null)
            {
                lstSideLink = sideView.DashboardSideList;
                groups = sideView.DashboardGroups;
            }
            var sideGroupProp = groups.FirstOrDefault(x => x.DashboardGroup.Trim().ToLower() == treeViewGroup.SelectedNode.Parent.Text.Trim().ToLower());

            if (sideGroupProp != null)
            {
                var dbGroupProp = sideGroupProp.Dashboards.Find(x => x.DashboardName == treeViewGroup.SelectedNode.Text);

                sideGroupProp.Dashboards.Remove(dbGroupProp);
                sideView.DashboardGroups = groups;
                sideView.DashboardSideList = lstSideLink;
            }
            else
            {
                DashboardPanelProperty dashboard = (View.ViewProperty as SideDashboardView).Dashboards.FirstOrDefault(x => x.DashboardName.ToLower() == treeViewGroup.SelectedNode.Text.ToLower());
                if (dashboard != null)
                {
                    sideView.Dashboards.Remove(dashboard);
                }
            }
            
            View.ViewType = sideViewName;
            objDashboardPanelViewManager.Save(View);
            hdnviewid.Value = View.ID.ToString();
            pnlDashboard.Visible = false;
            BindTreeview();           
            ClearDashboard();


        }

        /// <summary>
        /// Delete Group View 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnDeleteGroup_Click(object sender, EventArgs e)
        {
            if (sideView != null)
            {
                lstSideLink = sideView.DashboardSideList;
                groups = sideView.DashboardGroups;
               
            }
            var sideGroupProp = groups.First(x => x.DashboardGroup.Trim().ToLower() == txtGroupname.Text.Trim().ToLower());
            groups.Remove(sideGroupProp);
            sideView.DashboardGroups = groups;
            sideView.DashboardSideList = lstSideLink;
            View.ViewType = sideViewName;
            objDashboardPanelViewManager.Save(View);
            hdnviewid.Value = View.ID.ToString();
            pnlgroup.Visible = false;
            BindTreeview();
            //ClearViewGroup();
         
         
        }

        protected void btnDeleteView_Click(object sender, EventArgs e)
        {
            if (View != null)
            {
                objDashboardPanelViewManager.Delete(View);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        //protected void ValidateFileUpload_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    if (fileupload.HasFile)
        //    {
        //        CheckfileValidation(fileupload, args, CustValFileUpload);
        //    }
           
        //}
        
        protected void ddlDBBackGround_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlDBBackGround.SelectedItem.Value)
            {
                case "0":

                    trDBBackgroundImage.Visible = false;
                   // trDBIcon.Visible = false;
                    break;

                case "1":
                    trDBBackgroundImage.Visible = true;
                    //trDBIcon.Visible = false;
                    break;

                //case "2":
                //    trDBBackgroundImage.Visible = false;
                //    trDBIcon.Visible = true;
                //    break;

                //case "3":
                //    trDBBackgroundImage.Visible = true;
                //    trDBIcon.Visible = true;
                //    break;
                    
                default:
                    break;
            }
        }

        private void CheckfileValidation(FileUpload fileUploadCntrl, ServerValidateEventArgs args, CustomValidator CustomValidator)
        {
            if (!fileUploadCntrl.PostedFile.ContentType.StartsWith("image", StringComparison.CurrentCultureIgnoreCase))
            {
                args.IsValid = false;
                CustomValidator.ErrorMessage = "Please upload image file(png, jpg, gif) only.";
            }
            else if (fileUploadCntrl.PostedFile.ContentLength > 2097152)
            {
                args.IsValid = false;
                CustomValidator.ErrorMessage = "File size must be below 2MB.";
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        private void ClearDashboard()
        {
            txtheight.Text = string.Empty;
            txtorder.Text = string.Empty;            
            txtFuIcon.SetImageUrl(string.Empty);
            txtDisplayName.Text = string.Empty;
            txtDashboardUrl.Text = string.Empty;
            txtDBBackGround.SetImageUrl(string.Empty);
            rbNavigationList.SelectedIndex = 0;
            
           
        }

        protected void ddlDashBoardList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dashboard dashBoard = listofDB.Where(x => x.Title == ddlDashBoardList.SelectedValue).FirstOrDefault();
            if (dashBoard != null)
            {
                dashboardId=Convert.ToString(dashBoard.ID);
                dashboardName = dashBoard.Title;
                dashboardType =Convert.ToString(dashBoard.DashboardType);
                txtDisplayName.Text = dashBoard.Title;
                txtorder.Text = Convert.ToString(dashBoard.ItemOrder);
                txtheight.Text = Convert.ToString(dashBoard.PanelHeight);
                
               
                if (dashBoard.DashboardType == DashboardType.Chart)
                {
                    txtFuIcon.SetImageUrl(dashBoard.Icon);
                    trDashBoardUrl.Visible = false;
                    trDBBackGround.Visible = false;
                    ddlDBBackGround.SelectedIndex = 0;
                }
                else
                {
                    PanelSetting pSetting = (PanelSetting)dashBoard.panel;
                    txtFuIcon.SetImageUrl(pSetting.IconUrl);
                    trDashBoardUrl.Visible = true;
                    trDBBackGround.Visible = true;
                }
            }
        }

        protected void CustomValidatorDashBoard_ServerValidate(object source, ServerValidateEventArgs args)
        {

            if (treeViewGroup.SelectedNode != null)
            {
                if (treeViewGroup.SelectedNode.Text.Trim().ToLower() != ddlDashBoardList.SelectedValue.ToLower())
                {
                    if (View != null)
                    {
                        sideView = View.ViewProperty as SideDashboardView;
                        if (sideView.Dashboards.Exists(t => t.DashboardName.ToLower() == ddlDashBoardList.SelectedValue.ToLower()))
                            args.IsValid = false;
                    }
                }
            }
            else
            {
                if (View != null)
                {
                    sideView = View.ViewProperty as SideDashboardView;
                    if (sideView.Dashboards.Exists(t => t.DashboardName.ToLower() == ddlDashBoardList.SelectedValue.ToLower()))
                        args.IsValid = false;

                }
            }

        }

        //protected void CustomValidatorfuDBBackGround_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    if (fuDBBackGround.HasFile)
        //    {
        //        CheckfileValidation(fuDBBackGround, args, CustomValidatorfuDBBackGround);
        //    }
        //}

        //protected void CustomValidatorfuIcon_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    if (fuIcon.HasFile)
        //    {
        //        CheckfileValidation(fuIcon,args, CustomValidatorfuIcon);
        //    }
        //}
    }
}
