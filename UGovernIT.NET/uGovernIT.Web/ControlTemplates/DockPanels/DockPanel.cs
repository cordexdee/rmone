using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
  
    public class DockPanel : ASPxDockPanel, IDockPanel
    {
        public bool NeedData { get; set; }
        public string PageID { get; set; }
        public bool Editable { get; set; }
        public virtual DockPanelSetting DockSetting { get; set; }

        public DockPanel()
        {
            DockSetting = new DockPanelSetting();          
            DockSetting.ControlID = this.PanelUID = Guid.NewGuid().ToString();
        }

        protected override void OnInit(EventArgs e)
        {          
            this.PanelUID = DockSetting.ControlID;           
            this.Width = Unit.Pixel(275);
            this.OwnerZoneUID = "LeftZone";
            this.AllowDragging = true;
            this.AllowedDockState = AllowedDockState.All;
            this.AllowResize = true;
            this.ID = PanelUID.Replace("-", "_");
          
            this.ClientInstanceName = PanelUID;
            if (!Editable)
            {
                this.AllowDragging = false;
                if (DockSetting != null)
                {
                    this.ShowHeader = DockSetting.ShowTitle;
                    this.Styles.Header.BackColor = Color.Transparent;
                    this.Styles.Header.Font.Bold = true;
                    this.Styles.Header.Paddings.PaddingLeft=Unit.Pixel(0);
                }
                else
                    this.ShowHeader = false;
                this.ShowCloseButton = false;
                this.Border.BorderStyle = BorderStyle.None;
                this.Border.BorderWidth = Unit.Pixel(0);
                this.Border.BorderColor = System.Drawing.Color.Transparent;
                this.Styles.Content.Border.BorderStyle = BorderStyle.None;
             
                this.Styles.Content.Paddings.Padding = Unit.Pixel(0);
                this.Styles.Content.Border.BorderWidth = Unit.Pixel(0);
                this.Styles.Content.Border.BorderColor = System.Drawing.Color.Transparent;
            }
            
            if(!NeedData)
            {
                this.ShowCloseButton = false;
                ASPxButton closeimage = new ASPxButton();
                closeimage.ID = "closeImage";
                closeimage.Attributes.Add("PanelUID", PanelUID);
                closeimage.Attributes.Add("PageID", PageID);
                closeimage.ImageUrl = "/Content/images/close-blue.png";
                closeimage.Image.Width = 16;
                closeimage.ToolTip = "Delete";
                closeimage.ClientSideEvents.Click = "function(){ConfirmDeletePanel();}";
                closeimage.Style.Add("top", "0px");
                closeimage.Style.Add("margin-right", "2px");
                closeimage.Style.Add("float", "right");
                closeimage.EnableTheming = false;
                closeimage.RenderMode = ButtonRenderMode.Link;
                closeimage.Click += Closeimage_Click;
                ASPxButton image = new ASPxButton();
                image.ID = "editImage";
                image.Attributes.Add("PanelUID", PanelUID);
                image.Attributes.Add("PageID", PageID);
                image.ImageUrl = "/Content/images/editNewIcon.png";
                image.ToolTip = "Edit";
                image.Style.Add("top", "0px");
                image.Style.Add("margin-right", "10px");
                image.Style.Add("float", "right");
                image.Image.Width = 16;
                image.EnableTheming = false;
                image.RenderMode = ButtonRenderMode.Link;
                image.AutoPostBack = false;
                image.ClientSideEvents.Click = "function(s,e){openPopup('"+ PanelUID + "','"+ PageID + "');}";
               // image.Click += Image_Click;
                this.Controls.Add(closeimage);
                this.Controls.Add(image);
                this.Controls.Add(closeimage);
                this.Controls.Add(image);
            }
          
            base.OnInit(e);
        }

        public virtual Control LoadControl(Page page) {

            return null;
        }

        //public void Image_Click(object sender, EventArgs e)
        //{
        //    ASPxButton button = sender as ASPxButton;
        //    if (button != null)
        //    {
        //        if (button != null)
        //        {
        //            string panelID = button.Attributes["PanelUID"];
        //            string pageID = button.Attributes["PageID"];
        //            if (!string.IsNullOrEmpty(pageID) && !string.IsNullOrEmpty(panelID))
        //            {
        //                PageConfigurationManager pageConfigurationManager = new PageConfigurationManager(HttpContext.Current.GetManagerContext());
        //                PageConfiguration pageConfiguration = pageConfigurationManager.Get(x => x.ID == Convert.ToInt64(pageID));
        //                List<DockPanelSetting> controlsInfo = new List<DockPanelSetting>();
        //                if (pageConfiguration != null && !string.IsNullOrEmpty(pageConfiguration.ControlInfo))
        //                {
        //                    XmlDocument document = new XmlDocument();
        //                    document.LoadXml(pageConfiguration.ControlInfo);
        //                    controlsInfo = uHelper.DeSerializeAnObject(document, controlsInfo) as List<DockPanelSetting>;
        //                }
        //                if (controlsInfo != null && controlsInfo.Count > 0)
        //                {
        //                    DockPanelSetting controlInfo = controlsInfo.FirstOrDefault(x => x.ControlID == panelID);
        //                    string url = "";
        //                    if (controlInfo != null)
        //                    {
        //                        if (controlInfo.AssemblyName == "uGovernIT.Web.ControlTemplates.DockPanels.DashboardDockPanel")
        //                        {
                                    
        //                        }
        //                    }                          
        //                }
        //            }
        //        }
        //    }
        //}
        public void Closeimage_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_delete_value"];
            if (confirmValue == "NO" || confirmValue == null)
                return;

            ASPxButton button = sender as ASPxButton;
            if (button != null)
            {
                string panelID = button.Attributes["PanelUID"];
                string pageID = button.Attributes["PageID"];
                if (!string.IsNullOrEmpty(pageID) && !string.IsNullOrEmpty(panelID))
                {
                    PageConfigurationManager pageConfigurationManager = new PageConfigurationManager(HttpContext.Current.GetManagerContext());
                    PageConfiguration pageConfiguration = pageConfigurationManager.Get(x => x.ID == Convert.ToInt64(pageID));
                    List<DockPanelSetting> controlsInfo = new List<DockPanelSetting>();
                    if (pageConfiguration != null && !string.IsNullOrEmpty(pageConfiguration.ControlInfo))
                    {
                        XmlDocument document = new XmlDocument();
                        document.LoadXml(pageConfiguration.ControlInfo);
                        controlsInfo = uHelper.DeSerializeAnObject(document, controlsInfo) as List<DockPanelSetting>;
                    }
                    if (controlsInfo != null && controlsInfo.Count>0)
                    {
                        DockPanelSetting controlInfo = controlsInfo.FirstOrDefault(x => x.ControlID == panelID);
                        if (controlInfo != null)
                        {
                            controlsInfo.Remove(controlInfo);
                        }
                        if (controlsInfo.Count > 0)
                        {
                            pageConfiguration.ControlInfo = uHelper.SerializeObject(controlsInfo).OuterXml;
                        }
                        else
                        {
                            pageConfiguration.ControlInfo = "";
                            pageConfiguration.LayoutInfo = "";
                        }
                        pageConfigurationManager.Update(pageConfiguration);
                        Response.Redirect(HttpContext.Current.Request.Url.OriginalString.ToString());
                    }

                }
            }
        }
    }
}