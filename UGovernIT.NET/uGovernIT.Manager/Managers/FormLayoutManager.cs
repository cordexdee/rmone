using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class FormLayoutManager:ManagerBase<ModuleFormTab>, IFormLayoutManager
    {
        private ModuleFormLayoutStore layoutStore;
        private ModuleFormTabStore tabStore;
        private ModuleRoleWriteAccessStore accessStore;
        private DAL.Store.ModuleColumnsStore columnStore;

        public FormLayoutManager(ApplicationContext context):base(context)
        {
            layoutStore = new ModuleFormLayoutStore(this.dbContext);
            tabStore = new ModuleFormTabStore(this.dbContext);
            accessStore = new ModuleRoleWriteAccessStore(this.dbContext);
            columnStore = new DAL.Store.ModuleColumnsStore(this.dbContext);
            store = new FormLayoutStore(this.dbContext);

        }
        public List<ModuleFormLayout> LoadLayout()
        {
            return layoutStore.Load();
        }
        public List<ModuleFormTab> GetConfigClientAdminCategoryData()
        {
            List<ModuleFormTab> muduleFormTabList = store.Load();
            return muduleFormTabList;
        }
        public void AddTab(ModuleFormTab tab)
        {
            tab.TabId = tab.TabSequence;
            tabStore.Insert(tab);
        }
        public void RenameTab(long tabID, string text)
        {

        }
        public void UpdateTab(ModuleFormTab tab)
        {
            tabStore.Update(tab);
        }
        public void UpdateSequence(ModuleFormLayout layout)
        {
            layoutStore.Update(layout);
        }
        public void UpdateSequence(Utility.ModuleColumn columns)
        {
            columnStore.Update(columns);

        }
        public void DeleteTab(ModuleFormTab tab)
        {
            tabStore.Delete(tab);
        }
        public void DeleteFieldFromTab(List<ModuleFormLayout> Layout)
        {
            layoutStore.Delete(Layout);
        }
        public void DeleteRequestWriteAccess(List<ModuleRoleWriteAccess> Request)
        {
            accessStore.Delete(Request);
        }
        public void OrderTab(List<ModuleFormTab> tabs, long firstID, long sencondID)
        {

        }
        public void OrderLayout(List<ModuleFormLayout> layoutItems, long firstID, long secondID)
        {

        }
    }
    public interface IFormLayoutManager:IManagerBase<ModuleFormTab>
    {
        List<ModuleFormTab> GetConfigClientAdminCategoryData();
    }
}
