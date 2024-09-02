using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
using System.Web;
using uGovernIT.Utility.Entities;
using System.Data;
using DevExpress.Web;
using uGovernIT.Manager;

namespace uGovernIT.Web
{
    public partial class DashboardIndivisibleEditView : UserControl
    {
        DashboardPanelProperty dbPanPropObj;
        List<DashboardPanelProperty> lstDBPanProp;
        IndivisibleDashboardsView indViewObj = new IndivisibleDashboardsView();
        DashboardViewProperties dbViewProp = new DashboardViewProperties();
        public DashboardPanelView View { get; set; }
        public String ViewType { get; set; }
        public string dashboardname;
        public string indView = "Indivisible Dashboards";
        protected string dashboardId;
        protected string dashboardUrl;
        protected string dashboardType;
        List<Dashboard> listofDB = null;
        bool add = true;
        private string absPath = string.Empty;
        DashboardPanelViewManager objDashboardPanelViewManager = new DashboardPanelViewManager(HttpContext.Current.GetManagerContext());
        DashboardManager dboardManager = new DashboardManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            listofDB = dboardManager.LoadAll(false);
            listofDB = listofDB.Where(x => x.DashboardType == DashboardType.Chart || x.DashboardType == DashboardType.Panel).OrderBy(x => x.DashboardType).ThenBy(x => x.Title).ToList();
            if (!Page.IsPostBack)
            {
                if (View != null)
                {
                    ImgDeleteView.Visible = true;
                    indViewObj = (View.ViewProperty as IndivisibleDashboardsView);
                    lstDBPanProp = indViewObj.Dashboards;
                    txtviewname.Text = View.ViewName;
                    peAuthorizedToView.SetValues(View.AuthorizedToViewUsers);
                    SortAndBind(listofDB, lstDBPanProp);
                    BuildFilterTree();
                }
                else
                {
                    grddashboars.DataSource = listofDB;
                    grddashboars.DataBind();
                }
                ReloadDashboardTree();
            }
            else
            {
                if (View == null)
                {
                    int viewID;
                    Int32.TryParse(hdnviewid.Value, out viewID);
                    if (viewID > 0)
                    {
                        View = objDashboardPanelViewManager.LoadViewByID(viewID);
                        indViewObj = (View.ViewProperty as IndivisibleDashboardsView);
                        lstDBPanProp = indViewObj.Dashboards;
                        ImgDeleteView.Visible = true;
                        BuildFilterTree();
                        ReloadDashboardTree();
                    }
                }
                else
                {
                    indViewObj = (View.ViewProperty as IndivisibleDashboardsView);
                    lstDBPanProp = indViewObj.Dashboards;
                }
                SortAndBind(listofDB, lstDBPanProp);

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            dashboardUrl = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx");
            absPath = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.PickFromAsset);
            lnkbackground.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','900px','600px','','')", absPath, "Pick From Library"));
            /* Use To Load Dashboard List */

        }
        /// <summary>
        /// This is used to Sorting and Binding of Dashboards.
        /// </summary>
        /// <param name="listofDB"></param>
        /// <param name="lstIndViewProp"></param>
        public void SortAndBind(List<Dashboard> listofDB, List<DashboardPanelProperty> lstIndViewProp)
        {
            if (listofDB.Count > 0)
            {
                List<Dashboard> sortedList = new List<Dashboard>();
                if (lstIndViewProp != null)
                {
                    for (int k = 0; k < lstIndViewProp.Count; k++)
                    {
                        Dashboard udbprop = listofDB.FirstOrDefault(x => x.Title == lstIndViewProp[k].DashboardName);
                        if (udbprop != null)
                        {
                            udbprop.ItemOrder = lstIndViewProp[k].ItemOrder;
                            sortedList.Add(udbprop);
                        }
                    }
                    sortedList = sortedList.OrderBy(p => p.ItemOrder).ToList();
                    for (int t = 0; t < listofDB.Count; t++)
                    {
                        for (int l = 0; l < sortedList.Count; l++)
                        {
                            if (listofDB[t].ID == sortedList[l].ID)
                            {
                                add = false;
                                break;
                            }
                            else
                            {
                                add = true;

                            }
                        }
                        if (add)
                            sortedList.Add(listofDB[t]);
                    }
                    grddashboars.DataSource = sortedList;
                    grddashboars.DataBind();
                }
            }
        }
        /// <summary>
        ///To  Create New Indidvidual  View 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnAddView_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (View == null)
            {
                View = new DashboardPanelView();
                View.ViewName = txtviewname.Text.Trim();
                View.Title = txtviewname.Text.Trim();
            }
            else
            {
                View.ViewName = txtviewname.Text.Trim();
                View.Title = txtviewname.Text.Trim();
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


            if (lstDBPanProp == null)
            {
                lstDBPanProp = new List<DashboardPanelProperty>();
            }

            for (int k = 0; k < grddashboars.VisibleRowCount; k++)
            {
                CheckBox chkBx = grddashboars.FindRowCellTemplateControl(k, null, "ChkChecked") as CheckBox;
                string dashboardName = Convert.ToString(grddashboars.GetRowValues(k, grddashboars.KeyFieldName));
                DashboardPanelProperty panelProperty = lstDBPanProp.FirstOrDefault(x => x.DashboardName == dashboardName);
                if (panelProperty == null && chkBx.Checked)
                {
                    Dashboard uDashboard = listofDB.FirstOrDefault(x => x.Title == dashboardName);
                    if (uDashboard != null)
                    {
                        dbPanPropObj = new DashboardPanelProperty();
                        dbPanPropObj.Width = uDashboard.PanelWidth;
                        dbPanPropObj.Height = uDashboard.PanelHeight;
                        dbPanPropObj.Theme = uDashboard.ThemeColor;
                        if (lstDBPanProp.Count > 0)
                        {
                            dbPanPropObj.ItemOrder = lstDBPanProp.Max(x => x.ItemOrder) + 1;
                        }

                        dbPanPropObj.DashboardName = uDashboard.Title;
                        dbPanPropObj.DisplayName = uDashboard.panel.ContainerTitle;
                        dbPanPropObj.IconUrl = uDashboard.Icon;
                        lstDBPanProp.Add(dbPanPropObj);
                        View.ViewType = indView.Trim();
                    }
                }
                else if (panelProperty != null && !chkBx.Checked)
                {
                    lstDBPanProp.RemoveAt(lstDBPanProp.IndexOf(panelProperty));
                }
            }

            indViewObj.Dashboards = lstDBPanProp;
            View.ViewProperty = indViewObj;
            objDashboardPanelViewManager.Save(View);
            hdnviewid.Value = View.ID.ToString();

            ReloadDashboardTree();
        }

        protected void BtnEditDBProp_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || View == null)
                return;

            var panView = View;
            lstDBPanProp = (View.ViewProperty as IndivisibleDashboardsView).Dashboards;//.Where(x=>!string.IsNullOrEmpty(x.DashboardName)).ToList();
            indViewObj = (View.ViewProperty as IndivisibleDashboardsView);

            DashboardPanelProperty panelProperty = lstDBPanProp.FirstOrDefault(x => x.DashboardName == tvSelectedDashboard.SelectedValue);
            if (panelProperty != null)
            {
                panelProperty = lstDBPanProp[lstDBPanProp.IndexOf(panelProperty)];
                panelProperty.DisplayName = txtDisplayName.Text.Trim();
                panelProperty.Height = Convert.ToInt32(txtheight.Text);
                panelProperty.Width = Convert.ToInt32(txtwidth.Text);
                panelProperty.Theme = ddlBorderType.SelectedValue;
                panelProperty.ItemOrder = Convert.ToInt32(txtitemorder.Text);
                panelProperty.DisableInheritDefault = !chkinherit.Checked;
                panelProperty.StartFromNewLine = cbStartFromNewLine.Checked;
                lstDBPanProp.ForEach(o => { o.WidthUnitType = UnitType.Pixel; o.LeftUnitType = UnitType.Pixel; });
                indViewObj.Dashboards = lstDBPanProp;

                panView.ViewProperty = indViewObj;

                //string uploaedFileURL = string.Empty;
                //if (fuIcon.HasFile)
                //{
                //    string fileName = string.Format("{0}_{1}{2}", System.IO.Path.GetFileNameWithoutExtension(fuIcon.PostedFile.FileName), Guid.NewGuid(), System.IO.Path.GetExtension(fuIcon.PostedFile.FileName));
                //    uploaedFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}", fileName);
                //    string path = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName);
                //    fuIcon.PostedFile.SaveAs(path);
                //}

                //if (uploaedFileURL != string.Empty)
                //{
                //    panelProperty.IconUrl = uploaedFileURL;
                //    txtfileupload.Text = panelProperty.IconUrl;
                //}
                //else
                //{
                panelProperty.IconUrl = txtfileupload.Text;
                //}
            }
            objDashboardPanelViewManager.Save(panView);
            //panView.Save();
            ReloadDashboardTree();
        }

        //protected void FuIcon_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    if (fuIcon.HasFile)
        //    {
        //        if (!fuIcon.PostedFile.ContentType.StartsWith("image", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            args.IsValid = false;
        //            CustValFileUpload.ErrorMessage = "Please upload image file(png, jpg, gif) only.";
        //        }
        //        else if (fuIcon.PostedFile.ContentLength > 2097152)
        //        {
        //            args.IsValid = false;
        //            CustValFileUpload.ErrorMessage = "File size must be below 2MB.";
        //        }
        //    }
        //}

        private void ReloadDashboardTree()
        {
            if (View == null)
                return;

            tvSelectedDashboard.Nodes.Clear();
            List<DashboardPanelProperty> panels = (View.ViewProperty as IndivisibleDashboardsView).Dashboards;
            foreach (DashboardPanelProperty panel in panels.OrderBy(m => m.ItemOrder).ToList())
            {
                Dashboard uDashboard = listofDB.FirstOrDefault(x => x.Title == panel.DashboardName);
                if (uDashboard != null)
                {

                    tvSelectedDashboard.Nodes.Add(new TreeNode(string.Format("{0} - {1} - {2} - {3}", panel.ItemOrder, uDashboard.Title, uDashboard.ID, uDashboard.DashboardType), panel.DashboardName));
                }
            }

            if (tvSelectedDashboard.Nodes.Count > 0 && tvSelectedDashboard.SelectedNode == null)
            {
                tvSelectedDashboard.Nodes[0].Selected = true;
                TVSelectedDashboard_SelectedNodeChanged(tvSelectedDashboard, new EventArgs());
            }
        }

        protected void TVSelectedDashboard_SelectedNodeChanged(object sender, EventArgs e)
        {
            clear();
            if (View != null)
            {
                lstDBPanProp = (View.ViewProperty as IndivisibleDashboardsView).Dashboards;
                DashboardPanelProperty panelProperty = lstDBPanProp.FirstOrDefault(x => x.DashboardName == tvSelectedDashboard.SelectedValue);
                var sboardname = tvSelectedDashboard.SelectedNode.Text.Split(new char[] { '-' });
                dashboardname = tvSelectedDashboard.SelectedValue;
                dashboardId = sboardname[2];
                dashboardType = sboardname[3];

                if (panelProperty != null)
                {

                    chkinherit.Checked = !panelProperty.DisableInheritDefault;
                    txtheight.Text = panelProperty.Height.ToString();
                    txtwidth.Text = panelProperty.Width.ToString();

                    txtitemorder.Text = panelProperty.ItemOrder.ToString();
                    ViewState["Prevvalue"] = txtheight.Text.Trim() + "," + txtwidth.Text.Trim() + "," + txtitemorder.Text.Trim();
                    txtDisplayName.Text = panelProperty.DisplayName;
                    txtfileupload.Text = panelProperty.IconUrl;
                    cbStartFromNewLine.Checked = panelProperty.StartFromNewLine;
                    ddlBorderType.SelectedIndex = ddlBorderType.Items.IndexOf(ddlBorderType.Items.FindByValue(Convert.ToString(panelProperty.Theme)));
                }
            }
        }

        /// <summary>
        /// To Clear the value of control
        /// </summary>
        private void clear()
        {
            txtheight.Text = "";
            txtwidth.Text = "";
            txtitemorder.Text = "";

            chkinherit.Checked = false;
        }

        protected void DdlView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grddashboars_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (lstDBPanProp != null)
            //    if (e.Row.RowType == DataControlRowType.DataRow)
            //    {
            //        string dashboardName = Convert.ToString(grddashboars.DataKeys[e.Row.RowIndex].Value);
            //        if (lstDBPanProp.Exists(x => x.DashboardName == dashboardName))
            //        {
            //            CheckBox chkBx = (CheckBox)e.Row.FindControl("ChkChecked");
            //            chkBx.Checked = true;
            //        }
            //    }
        }

        //protected void chkinherit_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkinherit.Checked)
        //    {
        //        reqvalheight.Enabled = true;
        //        ReqValwidth.Enabled = true;
        //        ReqValorder.Enabled = true;
        //        txtheight.ReadOnly = false;
        //        Txtwidth.ReadOnly = false;
        //        Txtitemorder.ReadOnly = false;
        //    }
        //    else
        //    {
        //        reqvalheight.Enabled = false;
        //        ReqValorder.Enabled = false; ;
        //        ReqValwidth.Enabled = false;
        //        ReqValorder.EnableClientScript = true;
        //        txtheight.ReadOnly = true ;
        //        Txtwidth.ReadOnly = true;
        //        Txtitemorder.ReadOnly = true;
        //        if (ViewState["Prevvalue"] != null)
        //        {
        //            var str=ViewState["Prevvalue"].ToString().Split(new char[] {','});
        //            txtheight.Text = str[0].ToString();
        //            Txtwidth.Text = str[1].ToString();
        //            Txtitemorder.Text = str[2].ToString();
        //        }
        //    }
        //    //reqvalheight.Enabled = true;
        //    //ReqValwidth.Enabled = true;

        //}

        protected void ImgDeleteView_Click(object sender, ImageClickEventArgs e)
        {
            if (View != null)
            {
                objDashboardPanelViewManager.Delete(View);
                uGovernIT.Manager.uHelper.ClosePopUpAndEndResponse(Context, true, "");
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
                if (factTableFields != null && factTableFields.Count > 0 && factTable.Trim() != DatabaseObjects.Tables.DashboardSummary)
                {
                    factTableFields.RemoveAll(x => x.FieldName.EndsWith("User$Id"));
                }
                foreach (FactTableField field in factTableFields.OrderBy(x => x.FieldName))
                {
                    ListItem item = new ListItem(string.Format("{0}({1})", field.FieldName, field.DataType.Replace("System.", string.Empty)), field.FieldName);
                    item.Attributes.Add("datatype", field.DataType.Replace("System.", string.Empty));
                    list.Items.Add(item);
                }
            }
        }

        protected void TrDashboardFilter_SelectedNodeChanged(object sender, EventArgs e)
        {
            IndivisibleDashboardsView indView = View.ViewProperty as IndivisibleDashboardsView;
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
                chkHidden.Checked = filter.Hidden;
            }
            else
            {
                ClearFilterForm();
            }
        }

        protected void BtSaveFilter_Click(object sender, EventArgs e)
        {
            IndivisibleDashboardsView indView = View.ViewProperty as IndivisibleDashboardsView;

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
            indView.GlobalFilers = filters;
            objDashboardPanelViewManager.Save(View);
            BuildFilterTree();
            ClearFilterForm();
        }

        protected void BtDeleteFilter_Click(object sender, EventArgs e)
        {
            IndivisibleDashboardsView indView = View.ViewProperty as IndivisibleDashboardsView;
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
            //View.Save(SPContext.Current.Web);
            BuildFilterTree();
            ClearFilterForm();
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
            chkHidden.Checked = false;
            //glDefaultValue.Text = string.Empty;
            //glDefaultValue.DataBind();
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void BtCancelFilter_Click(object sender, EventArgs e)
        {
            ClearFilterForm();
            btAddNewFiler.Visible = false;
            btDeleteFilter.Visible = false;
        }

        protected void BuildFilterTree()
        {
            IndivisibleDashboardsView indView = View.ViewProperty as IndivisibleDashboardsView;

            trDashboardFilter.Nodes.Clear();
            trDashboardFilter.ShowLines = true;

            TreeNode root = null;
            TreeNode childRoot;
            root = new TreeNode();
            root.Text = "Global Filters";
            List<DashboardFilterProperty> filters = indView.GlobalFilers.OrderBy(x => x.ItemOrder).ToList();
            for (int i = 0; i < filters.Count; i++)
            {
                childRoot = new TreeNode();
                childRoot.Text = string.Format("{0} - {1}", filters[i].ItemOrder, filters[i].Title);
                childRoot.Value = filters[i].ID.ToString();
                //childroot.ShowCheckBox = true;
                root.ChildNodes.Add(childRoot);
            }
            trDashboardFilter.Nodes.Add(root);
            trDashboardFilter.DataBind();
        }
        protected void grddashboars_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

            if (e.RowType != DevExpress.Web.GridViewRowType.Data)
                return;
            if (lstDBPanProp != null)
            {
                string dashboardName = Convert.ToString(e.KeyValue);
                if (lstDBPanProp.Exists(x => x.DashboardName == dashboardName))
                {
                    CheckBox chk = grddashboars.FindRowCellTemplateControl(e.VisibleIndex, null, "ChkChecked") as CheckBox;
                    chk.Checked = true;
                }
            }

        }

        #endregion

    }
}

