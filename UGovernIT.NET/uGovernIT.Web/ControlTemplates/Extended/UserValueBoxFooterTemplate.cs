using DevExpress.Web;
using System;
using System.Web.UI;

namespace uGovernIT.Web
{
    internal class UserValueBoxFooterTemplate : ITemplate
    {
        private string clientID;

        public UserValueBoxFooterTemplate(string clientID)
        {
            this.clientID = clientID;
        }
        public void InstantiateIn(Control container)
        {
            GridViewStatusBarTemplateContainer Container = (GridViewStatusBarTemplateContainer)container;
            ASPxButton btn = new ASPxButton();
            btn.ID = this.clientID+"CloseBtn";
            btn.AutoPostBack = false;
            btn.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Justify;
            btn.Text = "Close";
            btn.ClientSideEvents.Click = "function(s,e){"+this.clientID+".ConfirmCurrentSelection(); "+this.clientID+".HideDropDown();}";
            Container.Controls.Add(btn);

        }
    }
}