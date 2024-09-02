using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class LinkConfiguratorView : UserControl
    {
        private string absoluteUrlEdit = "/layouts/ugovernit/DelegateControl.aspx?control={0}&linkItemID={1}";
        private string absoluteUrlNew = "/layouts/ugovernit/DelegateControl.aspx?control={0}&linkItemID={1}";

        private const string absoluteUrlView = "/layouts/ugovernit/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&IsDlg=1&isudlg=1&viewLink={2}";
        private const string formTitle = "Link Views";
        private const string viewParam = "linkconfig";

        private string newParam = "linkconfigadd";
        private string editParam = "linkconfigedit";
        List<LinkItems> splstLinkItems;
        List<LinkCategory> splstLinkCategory;
        List<LinkView> splstView;
        string addNewItem = string.Empty;
        // private string viewSelectedValue { get; set; }
        private string absoluteCategoryUrlEdit = "/layouts/ugovernit/DelegateControl.aspx?control={0}&linkCategoryID={1}";
        LinkItemManager ObjLinkItemManager = new LinkItemManager(HttpContext.Current.GetManagerContext());
        LinkCategoryManager ObjLinkCategoryManager = new LinkCategoryManager(HttpContext.Current.GetManagerContext());
        LinkViewManager ObjLinkViewManager = new LinkViewManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlNew, this.newParam, 0));
            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','New Item','600','400',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath)));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','New Item','600','400',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath)));
            splstLinkItems = ObjLinkItemManager.Load(x => !x.Deleted);
            splstLinkCategory = ObjLinkCategoryManager.Load(x => !x.Deleted);
            splstView = ObjLinkViewManager.Load(x => !x.Deleted);
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindViewDDL();
                BindCategoryDDL();

                ddlView.SelectedIndex = ddlView.Items.IndexOf(ddlView.Items.FindByValue(Request["viewLink"]));
                //BindGridView(ddlView.SelectedValue);
                if (ddlView.SelectedIndex > 0)
                    lnkDelete.Visible = true;
            }

            BindGridView(ddlView.SelectedValue);

            base.OnLoad(e);
        }

        // bind gridview on the base of ViewId.
        private void BindGridView(string viewID)
        {
            DataTable finalTable = CreateLinkTableSchema();

            List<LinkCategory> splstcolLinkCategory;
            if (splstLinkCategory != null)
            {
                splstcolLinkCategory = splstLinkCategory.Where(x => x.LinkViewLookup == Convert.ToInt64(viewID)).OrderBy(y => y.ItemOrder).ToList();

                foreach (LinkCategory item in splstcolLinkCategory)
                {
                    List<LinkItems> splstCollection = ObjLinkItemManager.Load(y => y.LinkCategoryLookup == item.ID && !y.Deleted).OrderBy(z => z.ItemOrder).ToList();

                    foreach (LinkItems lstItem in splstCollection)
                    {
                        DataRow row = finalTable.NewRow();

                        row[DatabaseObjects.Columns.Id] = lstItem.ID;
                        row[DatabaseObjects.Columns.Title] = lstItem.Title;
                        row[DatabaseObjects.Columns.UGITDescription] = lstItem.Description;
                        row[DatabaseObjects.Columns.ItemOrder] = lstItem.ItemOrder;
                        row[DatabaseObjects.Columns.TargetType] = lstItem.TargetType;

                        //SPFieldLookupValue splookup = new SPFieldLookupValue(Convert.ToString(lstItem[DatabaseObjects.Columns.LinkCategoryLookup]));
                        LinkCategory splookup = ObjLinkCategoryManager.LoadByID(lstItem.LinkCategoryLookup);
                        if (splookup != null)
                        {
                            row[DatabaseObjects.Columns.LinkCategoryLookup] = splookup.Title;
                            row["LinkCategoryId"] = splookup.ID;
                        }

                        row["CategoryItemOrder"] = item.ItemOrder;

                        finalTable.Rows.Add(row);

                    }
                }
            }

            ASPxGridViewLinkView.DataSource = finalTable;
            ASPxGridViewLinkView.DataBind();
        }

        protected void ddlView_SelectedIndexChanged(object sender, EventArgs e)
        {
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, viewParam, formTitle, ddlView.SelectedValue.Trim()));
            Response.Redirect(url);
        }

        private void BindViewDDL()
        {
            ddlView.Items.Clear();
            List<LinkView> dtView = ObjLinkViewManager.Load(x => !x.Deleted).OrderBy(x => x.CategoryName).ToList();

            if (dtView != null && dtView.Count > 0)
            {
                string prevCategoryName = string.Empty;
                foreach (LinkView itemView in dtView)
                {
                    string title = itemView.Title;
                    string categoryName = itemView.CategoryName;
                    string style = string.Empty;
                    ListItem item;

                    if (categoryName != prevCategoryName)
                    {
                        prevCategoryName = categoryName;
                        item = new ListItem(categoryName, "0");
                        item.Attributes.Add("style", string.Format("float:left;font-weight:bold;color:black"));
                        item.Attributes.Add("disabled", "disabled");
                        ddlView.Items.Add(item);
                    }
                    if (categoryName != string.Empty)
                    {
                        title = "— " + title;

                        style = "float:left;padding-left:10px;";
                    }
                    if (categoryName != "" || title != "— N/A")
                    {
                        item = new ListItem(title, itemView.ID.ToString());
                        item.Attributes.Add("style", style);
                        item.Attributes.Add("description", Convert.ToString(itemView.Description));
                        ddlView.Items.Add(item);
                    }
                }
            }

            ddlView.Items.Insert(0, new ListItem("--Please Select--", "0"));
        }

        protected void btnLinkViewSave_Click(object sender, EventArgs e)
        {
            if (!ValidateView())
                return;

            if (splstView != null)
            {
                LinkView item;
                if (hdnLinkViewAddEdit.Value == "1")
                    item = ObjLinkViewManager.LoadByID(Convert.ToInt32(ddlView.SelectedValue));
                else
                {
                    item = new LinkView();
                    item.Title = txtTitle.Text;
                    item.Description = txtDescription.Text;
                    if (hdnCategory.Value == "1")
                        item.CategoryName = txtCategoryName.Text;
                    else
                        item.CategoryName = ddlCategory.SelectedValue;
                }

                ObjLinkViewManager.Save(item);

                BindViewDDL();
                popupControlLinkView.ShowOnPageLoad = false;
                ddlView.SelectedIndex = ddlView.Items.IndexOf(ddlView.Items.FindByValue(Convert.ToString(item.ID)));
                BindGridView(ddlView.SelectedValue);
                BindCategoryDDL();
            }
        }

        private Boolean ValidateView()
        {
            if (hdnLinkViewAddEdit.Value == "1")
            {
                lblErrorMsgCategory.Text = string.Empty;
                if (hdnCategory.Value == "1")
                {
                    if (string.IsNullOrEmpty(txtCategoryName.Text))
                    {
                        lblErrorMsgCategory.Text = "field required.";
                        return false;
                    }
                    else
                    {
                        if (splstView != null && splstView.Count > 0)
                            
                                lblErrorMsgCategory.Text = "allready exist.";
                                return false;
                           
                        }
                    }
               
                else
                {
                    if (ddlCategory.SelectedValue == "0")
                    {
                        lblErrorMsgCategory.Text = "please select section.";
                        return false;
                    }
                    else
                    {
                        if (splstView != null && splstView.Count > 0)
                        {
                            //queryView.Query = string.Format("<Where><And><Neq><FieldRef Name='{4}' /><Value Type='Text'>{5}</Value></Neq><And><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}' /><Value Type='Text'>{3}</Value></Eq></And></And></Where>", DatabaseObjects.Columns.Title, txtTitle.Text.Trim(), DatabaseObjects.Columns.CategoryName, ddlCategory.SelectedValue, DatabaseObjects.Columns.Id, ddlView.SelectedValue);
                            List<LinkView> dtView = splstView.Where(x => x.ID != Convert.ToInt64(ddlView.SelectedValue) && x.Title.ToLower().Equals(txtTitle.Text.ToLower()) && x.CategoryName == ddlCategory.SelectedValue).ToList();
                            if (dtView != null && dtView.Count > 0)
                            {
                                lblErrorMsgCategory.Text = "allready exist.";
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            else
            {
                lblErrorMsgCategory.Text = string.Empty;
                if (hdnCategory.Value == "1")
                {
                    if (string.IsNullOrEmpty(txtCategoryName.Text))
                    {
                        lblErrorMsgCategory.Text = "field required.";
                        return false;
                    }
                    else
                    {
                        if (splstView != null && splstView.Count > 0)
                        {
                            //queryView.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}' /><Value Type='Text'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.Title, txtTitle.Text.Trim(), DatabaseObjects.Columns.CategoryName, txtCategoryName.Text.Trim());
                            
                                lblErrorMsgCategory.Text = "allready exist.";
                                return false;
                           
                        }
                    }
                }
                else
                {
                    if (ddlCategory.SelectedValue == "0")
                    {
                        lblErrorMsgCategory.Text = "please select section.";
                        return false;
                    }
                    else
                    {
                        if (splstView != null && splstView.Count > 0)
                        {

                            //queryView.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}' /><Value Type='Text'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.Title, txtTitle.Text.Trim(), DatabaseObjects.Columns.CategoryName, ddlCategory.SelectedValue);
                            List<LinkView> dtView = splstView.Where(x => x.Title.ToLower().Equals(txtTitle.Text.ToLower()) && x.CategoryName.ToLower().Equals(ddlCategory.SelectedValue)).ToList();
                            if (dtView != null && dtView.Count > 0)
                            {
                                lblErrorMsgCategory.Text = "allready exist.";
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

        private void BindCategoryDDL()
        {
            if (splstView != null && splstView.Count > 0)
            {
                // queryViewCategory.Query = string.Format("<OrderBy><FieldRef Name='{0}' /></OrderBy>", DatabaseObjects.Columns.CategoryName);
                List<LinkView> dtViewCategory = splstView.OrderBy(x => x.CategoryName).ToList();
                if (dtViewCategory != null && dtViewCategory.Count > 0)
                {
                    List<string> dtdistinctCategoryName = dtViewCategory.Select(x => x.CategoryName).Distinct().ToList();
                    ddlCategory.DataSource = dtdistinctCategoryName;
                    ddlCategory.DataBind();
                }
            }
            ddlCategory.Items.Insert(0, new ListItem("--Please Select--", "0"));
        }

        protected void ImgbtnEditViewLink_Click(object sender, ImageClickEventArgs e)
        {
            LinkView splistItemLinkView = ObjLinkViewManager.LoadByID(UGITUtility.StringToLong(ddlView.SelectedValue));
            if (splistItemLinkView != null)
            {
                txtTitle.Text = Convert.ToString(splistItemLinkView.Title);
                txtDescription.Text = Convert.ToString(splistItemLinkView.Description);
                ddlCategory.SelectedValue = Convert.ToString(splistItemLinkView.CategoryName);
            }

            BindViewDDL();
            popupControlLinkView.ShowOnPageLoad = true;
            ddlView.SelectedValue = Convert.ToString(splistItemLinkView.ID);
            BindGridView(ddlView.SelectedValue);
            BindCategoryDDL();
            hdnLinkViewAddEdit.Value = "1";
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            LinkView item;
            if (hdnLinkViewAddEdit.Value == "1")
            {
                item = ObjLinkViewManager.LoadByID(Convert.ToInt32(ddlView.SelectedValue));
                LinkCategory linkCategory = ObjLinkCategoryManager.Load(x=> x.LinkViewLookup==item.ID).FirstOrDefault();
                LinkItems linkItems = ObjLinkItemManager.Load(x=> x.LinkCategoryLookup==linkCategory.ID).FirstOrDefault();
                if (linkItems != null)
                    ObjLinkItemManager.Delete(linkItems);
                if (linkCategory != null)
                    ObjLinkCategoryManager.Delete(linkCategory);
                ObjLinkViewManager.Delete(item);
                BindViewDDL();
                ddlView.SelectedIndex = 0;
                popupControlLinkView.ShowOnPageLoad = false;
                uHelper.ClosePopUpAndEndResponse(Context,true);
            }
        }

        public static DataTable CreateLinkTableSchema()
        {
            DataTable myLinkData = new DataTable("LinkView");
            myLinkData.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
            //myLinkData.Columns.Add(DatabaseObjects.Columns.ModuleName);
            myLinkData.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            myLinkData.Columns.Add(DatabaseObjects.Columns.UGITDescription);
            myLinkData.Columns.Add(DatabaseObjects.Columns.ItemOrder, typeof(int));
            myLinkData.Columns.Add(DatabaseObjects.Columns.TargetType, typeof(string));
            myLinkData.Columns.Add(DatabaseObjects.Columns.LinkCategoryLookup, typeof(string));
            myLinkData.Columns.Add("LinkCategoryId", typeof(int));
            myLinkData.Columns.Add("CategoryItemOrder", typeof(int));

            return myLinkData;
        }

        protected void ASPxGridViewLinkView_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            //if ((e.RowType != GridViewRowType.Data && e.RowType != GridViewRowType.Group) || e.KeyValue == null)
            //    return;


            if (e.RowType == GridViewRowType.Data)
            {
                DataRow currentRow = ASPxGridViewLinkView.GetDataRow(e.VisibleIndex);
                string func = string.Empty;
                string adminModuleDefaultId = string.Empty;

                if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id) && Convert.ToString(currentRow[DatabaseObjects.Columns.Id]) != string.Empty)
                {
                    adminModuleDefaultId = currentRow[DatabaseObjects.Columns.Id].ToString().Trim();
                }
                func = string.Format("OpenPopupDialog('{0}','{1}','{2}','{3}','{4}', 0)", UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, adminModuleDefaultId)), "", currentRow[DatabaseObjects.Columns.Title].ToString().Trim(), "600px", "400px");

                HtmlImage img = (HtmlImage)ASPxGridViewLinkView.FindRowCellTemplateControl(e.VisibleIndex, null, "editLink");
                img.Attributes.Add("onClick", func);

                HtmlAnchor aTitle = ASPxGridViewLinkView.FindRowCellTemplateControl(e.VisibleIndex, null, "aTitle") as HtmlAnchor;
                aTitle.Attributes.Add("onClick", func);
            }

            if (e.RowType == GridViewRowType.Group)
            {
                DataRow row = ASPxGridViewLinkView.GetDataRow(e.VisibleIndex);
                ASPxLabel lb = (ASPxLabel)ASPxGridViewLinkView.FindGroupRowTemplateControl(e.VisibleIndex, "lblCategoryGroupHeader");
                lb.Text = "Section: " + row[DatabaseObjects.Columns.LinkCategoryLookup].ToString().Trim();

                HtmlImage img = (HtmlImage)ASPxGridViewLinkView.FindGroupRowTemplateControl(e.VisibleIndex, "imgEditCategorybutton");
                string func = string.Empty;
                func = string.Format("OpenPopupDialog('{0}','{1}','{2}','{3}','{4}', 0)", UGITUtility.GetAbsoluteURL(string.Format(absoluteCategoryUrlEdit, "linkcategoryedit", Convert.ToInt32(row["LinkCategoryId"]))), "", "Edit Category", "500px", "300px");
                img.Attributes.Add("onClick", func);
            }


        }

        protected void aTitle_Load(object sender, EventArgs e)
        {
            HtmlAnchor aHtml = (HtmlAnchor)sender;
            aHtml.InnerText = (aHtml.NamingContainer as GridViewDataItemTemplateContainer).Text.ToString();
        }

    }
}
