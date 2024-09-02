using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace uGovernIT.Web
{
    public class ReadOnlyTemplate : ITemplate
    {
        public void InstantiateIn(Control _container)
        {
            GridViewEditItemTemplateContainer container = _container as GridViewEditItemTemplateContainer;

            ASPxLabel lbl = new ASPxLabel();
            lbl.ID = "lbl";
            lbl.EncodeHtml = false;
            container.Controls.Add(lbl);
            lbl.Text = container.Text;
        }
    }
}