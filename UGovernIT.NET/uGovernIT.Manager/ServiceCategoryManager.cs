using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Utility;
using System.Data;
using System.Collections.Specialized;

namespace uGovernIT.Manager
{
    public class ServiceCategoryManager : ManagerBase<ServiceCategory>, IServiceCategoryManager
    {
        public ServiceCategoryManager(ApplicationContext context) : base(context)
        {
        }

        public bool Save(ServiceCategory objServiceCategory)
        {
            bool result = false;
            if(objServiceCategory.ID <= 0)
            {
                long i = Insert(objServiceCategory);
                if(i > 0) { result = true;  }
                else { result = false; }
            }
            else
            {
                result = Update(objServiceCategory);
            }
            return result;
        }
        public List<ServiceCategory> LoadAllCategories()
        {
            List<ServiceCategory> categories = new List<ServiceCategory>();
            categories = Load(); // SPListHelper.GetSPList(DatabaseObjects.Lists.ServiceCategories);
            return categories;

        }

        public List<ServiceCategory> LoadCategoryByType(string categoryName)
        {
            List<ServiceCategory> categories = new List<ServiceCategory>();
            categories = Load();
            if (categories != null)
            {
                categories = categories.Where(x=> x.CategoryName==categoryName).OrderBy(x=> x.ItemOrder).ThenBy(x=> x.CategoryName).ToList();
                categories.ForEach(x=> x.ImageUrl=UGITUtility.GetAbsoluteURL(x.ImageUrl));
            }
            return categories;

        }
        public List<ServiceCategory> GetServiceType(string serviceType)
        {
            List<ServiceCategory> category = new List<ServiceCategory>();
            category = Load();
            if(category != null)
            {
                category = category.Where(x=> x.CategoryName.Replace("~","").ToString()==serviceType).ToList();

            }
            return category;
        }
        public ServiceCategory GetByID(int? categoryID)
        {
            ServiceCategory category = null;
            if (categoryID == null || categoryID == 0)
                category = new ServiceCategory();
            else
                category = LoadByID(Convert.ToInt64(categoryID));            
            return category;
        }
        public void SaveCategoryOrder(OrderedDictionary keys, OrderedDictionary newValues)
        {
            var id = Convert.ToInt32(keys["ID"]);
            ServiceCategory serviceCategory = LoadByID(id);
            if (serviceCategory != null)
            {
                serviceCategory.CategoryName = Convert.ToString(newValues["CategoryName"]);
                serviceCategory.ItemOrder = UGITUtility.StringToInt(newValues["ItemOrder"]);
                Update(serviceCategory);
            }
        }
        //ServiceCategory LoadItem(ServiceCategory item)
        //{
        //    ServiceCategory category = new ServiceCategory();
        //    category.ID = item.ID;
        //    category.CategoryName = Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.CategoryName));
        //    int itemOrder = 0;
        //    int.TryParse(Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.ItemOrder)), out itemOrder);
        //    category.ItemOrder = itemOrder;
        //    category.IconUrl = uHelper.GetAbsoluteURL(Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.UGITImageUrl)));
        //    return category;
        //}

        ServiceCategory IServiceCategoryManager.LoadItem(DataRow item)
        {
            throw new NotImplementedException();
        }
    }
    interface IServiceCategoryManager : IManagerBase<ServiceCategory>
    {
        List<ServiceCategory> LoadAllCategories();
        List<ServiceCategory> LoadCategoryByType(string categoryName);
        ServiceCategory LoadItem(DataRow item);
    }
}
