using System;
using System.Web.UI;

namespace uGovernIT.Web
{
    public partial class CustomPanelBox : UserControl
    {
        public string BoxTitle { get; set; }       
        public string BoxHeight{ get; set; }
        public string BoxWidth{ get; set; }
        public string BoxCss{ get; set; }
        public bool IsFilterRequired
        {
            get;
            set;
        }
        public Control ContentControl { get; set; }
        public string pageId;
        protected string removeBorder = string.Empty;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            SetDefaultSettings();
        }

        protected override void OnPreRender(EventArgs e)
        {
            SetDefaultSettings();
            if (ContentControl != null)
            {
                ContentPlaceHolder.Controls.Add(ContentControl);
            }
            base.OnPreRender(e);
        }
        private void SetDefaultSettings()
        {
            if (BoxTitle == null)
                BoxTitle = string.Empty;
           
            if (BoxHeight == null || BoxHeight.Trim() == string.Empty)
            { 
                BoxHeight = "100%"; 
            }
            else
            {
                int height = 0;
                if (int.TryParse(BoxHeight, out height))
                    BoxHeight = height.ToString() + "px";
            }
            if (BoxWidth == null || BoxWidth.Trim() == string.Empty)
            { 
                BoxWidth = "100%"; 
            }
            else
            {
                int width = 0;
                if (int.TryParse(BoxWidth, out width))
                    BoxWidth = width.ToString() + "px";
            }
            pageId = this.ID;
            BoxHeaderTitle.Text = BoxTitle;
            BoxHeaderTitle.ToolTip = BoxTitle;
        }
    }
}
