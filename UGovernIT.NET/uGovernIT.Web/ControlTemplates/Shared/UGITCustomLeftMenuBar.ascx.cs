using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class UGITCustomLeftMenuBar : System.Web.UI.UserControl
    {
        public string MenuType { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        MenuNavigationManager objMenuNavigationHelper;
        protected void Page_Init(object sender, EventArgs e)
        {
            objMenuNavigationHelper = new MenuNavigationManager(context);
            MenuType = "MenuBar";

            List<MenuNavigation> list = objMenuNavigationHelper.LoadMenuNavigation(MenuType);

            for (int i = 0; i < list.Count; i++)
            {
                List<MenuNavigation> childItems = list[i].Children;
                if (list[i].MenuParentLookup == 0)
                {
                    DevExpress.Web.MenuItem item = new DevExpress.Web.MenuItem();
                    item.Text = list[i].MenuName;
                    item.Name = list[i].MenuName;
                    menuLeftBar.Items.Add(item);

                    if (list[i].ChildCount > 1)
                    {
                        for (int j = 0; j < childItems.Count; j++)
                        {
                            DevExpress.Web.MenuItem child = new DevExpress.Web.MenuItem();
                            child.Text = childItems[j].MenuName;
                            child.Name = childItems[j].MenuName;
                            item.Items.Add(child);
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}