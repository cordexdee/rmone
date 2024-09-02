using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections.Generic;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class LinkCategoryEdit : UserControl
    {
        public string linkCategoryID;
        List<LinkCategory> splstLinkCategory;
        List<LinkView> splstLinkView;
        LinkCategory itemLinkCategory;
        LinkCategoryManager ObjLinkCategoryManager = new LinkCategoryManager(HttpContext.Current.GetManagerContext());
        LinkViewManager ObjLinkViewManager = new LinkViewManager(HttpContext.Current.GetManagerContext());
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnInit(EventArgs e)
        {
            splstLinkCategory = ObjLinkCategoryManager.Load();
            splstLinkView = ObjLinkViewManager.Load();

            if (!IsPostBack)
            {
                BindViewDDL();
                BindCategoryDDL();
            }


            if (Convert.ToInt32(linkCategoryID) > 0)
            {
                lnkDelete.Visible = true;
                itemLinkCategory = ObjLinkCategoryManager.LoadByID(Convert.ToInt32(linkCategoryID));
                if (itemLinkCategory != null)
                {
                    txtCategoryName.Text = itemLinkCategory.Title;
                    txtOrder.Text =UGITUtility.ObjectToString(itemLinkCategory.ItemOrder);
                    txtFile.Text = itemLinkCategory.ImageUrl;
                    txtDescription.Text =itemLinkCategory.Description;

                
                    if (itemLinkCategory.LinkViewLookup>0 )
                        ddlView.SelectedIndex = ddlView.Items.IndexOf(ddlView.Items.FindByValue(Convert.ToString(itemLinkCategory.LinkViewLookup)));
                }
            }
            else
            {
                itemLinkCategory = new LinkCategory();
            }
            base.OnInit(e);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (itemLinkCategory != null)
                {
                    itemLinkCategory.Title = txtCategoryName.Text;
                    itemLinkCategory.ItemOrder =UGITUtility.StringToInt(txtOrder.Text);
                    itemLinkCategory.Description = txtDescription.Text;
                    itemLinkCategory.LinkViewLookup =UGITUtility.StringToLong(ddlView.SelectedValue);

                    itemLinkCategory.ImageUrl = txtFile.Text;
                    if (fileUpload.HasFile)
                    {
                        string uploadedPath = uHelper.GetUploadFolderPath();
                        // becasue if we upload more then one file with same name its problem.
                        //string fileName = fileUpload.FileName;
                        string fileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), fileUpload.FileName);
                        string fullPath = Path.Combine(uploadedPath, fileName);
                        while (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }
                        fileUpload.SaveAs(fullPath);

                        string relativePath = string.Format("/_layouts/15/images/uGovernIT/UploadedFiles/{0}", fileName);
                        itemLinkCategory.ImageUrl = relativePath;
                    }
                    ObjLinkCategoryManager.Save(itemLinkCategory);
                }
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        private void BindViewDDL()
        {
            ddlView.Items.Clear();
            List<LinkView> dtLinkView = ObjLinkViewManager.Load().OrderBy(x=>x.CategoryName).ToList();

            if (dtLinkView != null && dtLinkView.Count > 0)
            {
                string prevCategoryName = string.Empty;
                foreach (LinkView itemView in dtLinkView)
                {
                    string title = Convert.ToString(itemView.Title);
                    string categoryName = Convert.ToString(itemView.CategoryName);
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

            ddlView.Items.Insert(0, new ListItem("--Please Select--", ""));
        }

        protected void btnLinkViewSave_Click(object sender, EventArgs e)
        {
            if (!ValidateView())
                return;

            if (splstLinkView != null)
            {
                LinkView item = new LinkView();
                item.Title = txtTitle.Text;
                item.Description = txtDescription.Text;
                if (hdnCategory.Value == "1")
                    item.CategoryName = txtLinkViewCategory.Text;
                else
                    item.CategoryName = ddlCategory.SelectedValue;

                ObjLinkViewManager.Save(item);

                BindViewDDL();
                popupControlLinkView.ShowOnPageLoad = false;
                ddlView.SelectedValue = Convert.ToString(item.ID);
                BindCategoryDDL();
            }
        }

        private Boolean ValidateView()
        {
            lblErrorMsgCategory.Text = string.Empty;
            if (hdnCategory.Value == "1")
            {
                if (string.IsNullOrEmpty(txtLinkViewCategory.Text))
                {
                    lblErrorMsgCategory.Text = "field required.";
                    return false;
                }
                else
                {
                    if (splstLinkView != null && splstLinkView.Count > 0)
                    {
                        List<LinkView> dtView = splstLinkView.Where(x => x.Title.ToLower().Equals(txtTitle.Text.Trim().ToLower()) && x.CategoryName.ToLower().Equals(txtCategoryName.Text.ToLower().Trim())).ToList();
                        if (dtView != null && dtView.Count > 0)
                        {
                            lblErrorMsgCategory.Text = "allready exist.";
                            return false;
                        }
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
                    if (splstLinkView != null && splstLinkView.Count > 0)
                    {
                        var dtView = splstLinkView.Where(x => x.Title.ToLower().Equals(txtTitle.Text.Trim().ToLower()) && x.CategoryName.ToLower().Equals(ddlCategory.SelectedValue.ToLower().Trim())).ToList();
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

        private void BindCategoryDDL()
        {
            if (splstLinkView != null && splstLinkView.Count > 0)
            {
                var dtViewCategory = UGITUtility.ToDataTable(splstLinkView.OrderBy(x=>x.CategoryName).ToList());
                if (dtViewCategory != null && dtViewCategory.Rows.Count > 0)
                {
                    DataView dtviewCategory = new DataView(dtViewCategory);
                    DataTable dtdistinctCategoryName = dtviewCategory.ToTable(true, DatabaseObjects.Columns.CategoryName);
                    ddlCategory.DataSource = dtdistinctCategoryName;
                    ddlCategory.DataTextField = DatabaseObjects.Columns.CategoryName;
                    ddlCategory.DataValueField = DatabaseObjects.Columns.CategoryName;
                    ddlCategory.DataBind();
                }
            }
            ddlCategory.Items.Insert(0, new ListItem("--Please Select--", "0"));
        }

        protected void cvfileUpload_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (fileUpload.HasFile)
            {
                string extension = Path.GetExtension(fileUpload.PostedFile.FileName).ToLower();
                if (extension != ".png" && extension != ".jpeg" && extension != ".gif" && extension != ".jpg")
                {
                    args.IsValid = false;
                    cvfileUpload.ErrorMessage = "Please upload image file.(like png, jpeg, gif, jpg)";
                }
            }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(linkCategoryID) > 0)
            {
                itemLinkCategory = ObjLinkCategoryManager.LoadByID(Convert.ToInt32(linkCategoryID));
                if (itemLinkCategory != null)
                {
                    itemLinkCategory.Deleted = true;
                    ObjLinkCategoryManager.Save(itemLinkCategory);
                }
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
