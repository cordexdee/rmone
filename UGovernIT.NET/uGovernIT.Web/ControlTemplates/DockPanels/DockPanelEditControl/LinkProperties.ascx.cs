using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web
{
    public partial class LinkProperties : UserControl
    {
        public ApplicationContext context = HttpContext.Current.GetManagerContext();
        LinkViewManager linkViewManager = null;

        public LinkPanelSetting linkPanelSetting { get; set; }        
        public string LinkView { get; set; }
        public string ControlWidth { get; set; }
        public bool HideControlBorder { get; set; }

        public bool ShowTitle { get; set; }
        public string ContentTitle { get; set; }

        protected override void OnInit(EventArgs e)
        {
            linkViewManager = new LinkViewManager(context);

            if (linkPanelSetting != null)
            {
                
                txtTitle.Text = linkPanelSetting.Title;
                chkTitle.Checked = linkPanelSetting.ShowTitle;
                chkborder.Checked = linkPanelSetting.HideControlBorder;
                txtWidth.Text = linkPanelSetting.ControlWidth;

                if (linkPanelSetting.LinkView != null)
                {
                    ddlLinkView.ClearSelection();
                    ddlLinkView.Items.FindByValue(linkPanelSetting.LinkView).Selected = true;
                }
            }
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ShowTitle = chkTitle.Checked;

            if (ShowTitle)
                ContentTitle = txtTitle.Text.Trim();
            else
                ContentTitle = string.Empty;

            HideControlBorder = chkborder.Checked;
            ControlWidth = txtWidth.Text.Trim();

            if (ddlLinkView.SelectedItem != null)
                LinkView = ddlLinkView.SelectedItem.Value;
        }

        public void CopyFromWebpart(LinkPanelSetting webpart)
        {
            webpart.ShowTitle = chkTitle.Checked;
            webpart.Title = txtTitle.Text;

            linkPanelSetting = webpart;
        }

        public void CopyFromControl(LinkPanelSetting webpart)
        {
            if (webpart != null)
            {
                webpart.ShowTitle = chkTitle.Checked;
                webpart.Title = txtTitle.Text;
                webpart.HideControlBorder = chkborder.Checked;
                webpart.ControlWidth = txtWidth.Text;
                webpart.LinkView = LinkView;
                linkPanelSetting = webpart;
            }            
        }

        protected void ddlLinkView_Init(object sender, EventArgs e)
        {
            linkViewManager = new LinkViewManager(context);
            List<LinkView> dtView = linkViewManager.Load().OrderBy(x => x.CategoryName).ToList();            

            if (dtView != null && dtView.Count > 0)
            {
                string prevCategoryName = string.Empty;
                foreach (var itemView in dtView)
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
                        ddlLinkView.Items.Add(item);
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
                        ddlLinkView.Items.Add(item);
                    }
                }
            }

            ddlLinkView.Items.Insert(0, new ListItem("Please Select", "0"));

            if (!string.IsNullOrEmpty(LinkView))
            {
                ddlLinkView.SelectedIndex = ddlLinkView.Items.IndexOf(ddlLinkView.Items.FindByValue(LinkView.ToString()));
            }            
        }
    }
}