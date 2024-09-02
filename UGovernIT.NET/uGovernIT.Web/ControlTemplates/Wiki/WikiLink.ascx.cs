using DevExpress.XtraRichEdit.Drawing;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Web.ControlTemplates.Wiki
{
    public partial class WikiLink : System.Web.UI.UserControl
    {
        public string TicketId { get; set; }
        public string width { get; set; }
        public bool IsHeader { get; set; }
        protected string ajaxHelper = UGITUtility.GetAbsoluteURL("~/uGovernIT/_vti_bin/uGovernIT");
        ApplicationContext context;
        private const string absoluteUrlView = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Module={2}&TicketId={3}&Type={4}&&ControlId={5}";

        private string newParam = "listpicker";
        private string formTitle = "Picker List";

        protected override void OnInit(EventArgs e)
        {
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, newParam, formTitle, "WIKI", string.Empty, "WikiHelp", txtLinkUrl.ClientID));
            aShowWiki.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            context= HttpContext.Current.GetManagerContext();
            if (!IsPostBack)
            {
                divLinks.Style.Add("width", width);
                bool isExists = WikiArticleHelper.GetPermissions(context,"edit-delete", TicketId);
                ViewState["isExists"] = isExists;
                if (isExists == false)
                {
                    imgnew.Visible = false;
                    imgEdit.Visible = false;
                }

                BindLinks();

                //if (!string.IsNullOrEmpty(Request["control"]) && Request["control"].ToLower() == "wikilinks")
                //    UpdateWikiViewsCount();

                if (!IsHeader)
                {
                    divWikiLinkHeader.Style.Add("display", "none");
                    divLinks.Style.Add("border", "1px solid #C0C0C0");
                }
                else
                {
                    divWikiLinkHeader.Style.Add("display", "block");
                    divLinks.Style.Add("border", "none");
                }

            }
        }

        private void BindLinks()
        {
            WikiLinks wikiLinks = new WikiLinks();
            wikiLinks.TicketId = TicketId;
            WikiLinksManager wikiLinksManager = new WikiLinksManager(context);
            List<WikiLinks> spListItemCollection = wikiLinksManager.GetWikiLinks(wikiLinks);
            if (spListItemCollection != null && spListItemCollection.Count > 0)
            {
                // URL is stored internally as "<url>,<title>", get just the url part
                foreach (WikiLinks spListItem in spListItemCollection)
                {
                    string Url = UGITUtility.SplitString(spListItem.URL, ",", 0);
                    //if (spListItem != null )
                    //{
                    //    Url = spListItem.URL + spListItem.Attachments;
                    //}
                    if (!string.IsNullOrEmpty(Url))
                    {
                        if (!Url.StartsWith("http://") && !Url.StartsWith("https://"))
                            Url = UGITUtility.GetAbsoluteURL(Url);
                        spListItem.URL = Url;
                    }
                }


                //System.Data.DataTable links = UGITUtility.ToDataTable<WikiLinks>(spListItemCollection);

                // URL is stored internally as "<url>,<title>", get just the url part
                //for (int i = 0; i < links.Rows.Count; i++)
                //{
                //    string Url = UGITUtility.SplitString(links.Rows[i][DatabaseObjects.Columns.URL], ",", 0);

                //    DataRow oItem = GetTableDataManager.get(spListItemCollection.List, UGITUtility.StringToInt(links.Rows[i][DatabaseObjects.Columns.Id]));

                //    if (oItem != null && oItem.Attachments.Count > 0)
                //    {
                //        Url = oItem.Attachments.UrlPrefix + oItem.Attachments[0];
                //    }

                //    if (!Url.StartsWith("http://") && !Url.StartsWith("https://"))
                //        Url = UGITUtility.GetAbsoluteURL(Url);

                //    links.Rows[i][DatabaseObjects.Columns.URL] = Url;
                //}

                // Sort links by description/name
                System.Data.DataTable links = UGITUtility.ToDataTable<WikiLinks>(spListItemCollection);
                gvLinkList.DataSource = links;
                gvLinkList.DataBind();
            }
            else
            {
                gvLinkList.DataSource = null;
                gvLinkList.DataBind();
            }
        }

        protected void btnAddLink_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            WikiLinks wikiLinks = new WikiLinks();
            wikiLinks.TicketId = TicketId;
            byte[] fileData = null;

            if (fileupload.HasFile)
            {
                txtLinkUrl.Text = fileupload.FileName;
                fileData = fileupload.FileBytes;
                string path = "/Content/Images/ugovernit/upload";
                if (!Directory.Exists(Server.MapPath(path)))
                    Directory.CreateDirectory(Server.MapPath(Path.GetDirectoryName(path)));

                path = $"{path}{'/'}{fileupload.FileName}";
                fileupload.SaveAs(Server.MapPath(path));
                wikiLinks.URL = path;
            }
            else
                wikiLinks.URL = txtLinkUrl.Text;

            if (string.IsNullOrEmpty(txtLinkUrl.Text))
            {
                if (!string.IsNullOrEmpty(txtLinkTitle.Text))
                    wikiLinks.URL = txtLinkTitle.Text;
            }

            if (string.IsNullOrEmpty(txtLinkTitle.Text))
                wikiLinks.Title = wikiLinks.URL;
            else
                wikiLinks.Title = txtLinkTitle.Text;

            //if (!wikiLinks.Url.StartsWith("http://") && !wikiLinks.Url.StartsWith("https://"))
            //    wikiLinks.Url = "http://" + wikiLinks.Url;

            wikiLinks.Comments = wikiLinks.Title;

            WikiLinksManager wikiLinksManager = new WikiLinksManager(context);
            bool IsSuccess = wikiLinksManager.SaveLink(wikiLinks, fileData);
            txtLinkUrl.Text = string.Empty;
            txtLinkTitle.Text = string.Empty;
            if (IsSuccess == true)
                BindLinks();
            
        }

        //private void UpdateWikiViewsCount()
        //{
        //    WikiArticleHelper wikiArticleHelper = new WikiArticleHelper();
        //    bool isUpdated = wikiArticleHelper.UpdateWikiViewsCount(TicketId);
        //}

        protected void DeleteLink_Click(object sender, EventArgs e)
        {
            WikiLinksManager wikiLinksManager = new WikiLinksManager(context);
            WikiLinks wikiLinks = new WikiLinks();
            ImageButton btnDelete = (ImageButton)sender;
            wikiLinks.ID = UGITUtility.StringToInt(btnDelete.Attributes["LinkId"]);
            wikiLinks.TicketId = TicketId;
            bool isSuccess = wikiLinksManager.DeleteLink(wikiLinks);
            if (isSuccess)
            {
                BindLinks();
            }
        }

        protected void gvLinkList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            bool isExists = (bool)ViewState["isExists"];
            if (e.Row.RowType == DataControlRowType.DataRow && isExists)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                HyperLink lnkUrl = (HyperLink)e.Row.FindControl("lnkUrl");
                // e.Row.Attributes.Add("onmouseover", "ShowDeleteButton(this)");
                //e.Row.Attributes.Add("onmouseout", "HideDeleteButton(this)");
                //lnkUrl.Attributes.Add("onmouseover", "ShowDeleteButton(this)");
                //lnkUrl.Attributes.Add("onmouseout", "HideDeleteButton(this)");
            }
        }
    }
}