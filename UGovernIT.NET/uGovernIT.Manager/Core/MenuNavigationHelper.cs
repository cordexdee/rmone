using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Util.Cache;

namespace uGovernIT.Manager
{
    public class MenuNavigationManager:ManagerBase<MenuNavigation>, IMenuNavigationHelper
    {
        public MenuNavigationManager(ApplicationContext context):base(context)
        {
            store = new MenuNavigationStore(this.dbContext);
        }
        public  List<MenuNavigation> LoadMenuNavigation(string menuType)
        {
            string menu = string.Empty;
            if (string.IsNullOrEmpty(menuType))
                menu = "Default";
            else
                menu = menuType; //menutype is LeftMenuName

            List<MenuNavigation> items = new List<MenuNavigation>();
            items = (List<MenuNavigation>)CacheHelper<object>.Get($"MenuNavigation_{menu}{this.dbContext.TenantID}");
            
            if (items == null || items.Count == 0)
            {
                items = store.Load(x => x.MenuName == menu);
                CacheHelper<object>.AddOrUpdate($"MenuNavigation_{menuType}{this.dbContext.TenantID}", items);
            }

            if (items != null && items.Count > 0)
            {
                LoadRelational(ref items);
            }
            return items;

        }

        private  void LoadRelational(ref List<MenuNavigation> menu)
        {
            foreach (MenuNavigation menuItem in menu)
            {
                if (menuItem.MenuParentLookup > 0)
                {
                    MenuNavigation nv = menu.FirstOrDefault(x => x.ID == menuItem.MenuParentLookup);
                    if (nv != null)
                    {
                        menuItem.Parent = nv;
                    }
                }

                menuItem.Children = menu.Where(x => x.MenuParentLookup == menuItem.ID && x.IsDisabled == false).OrderBy(x => x.ItemOrder).ToList();
                if (menuItem.Children != null)
                {
                    menuItem.ChildCount = menuItem.Children.Count;
                }

            }
        }
    }
    public interface IMenuNavigationHelper : IManagerBase<MenuNavigation>
    {
        List<MenuNavigation> LoadMenuNavigation(string menuType);
    }
}
