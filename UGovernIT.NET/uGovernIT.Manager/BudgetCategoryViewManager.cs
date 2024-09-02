using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public class BudgetCategoryViewManager:ManagerBase<BudgetCategory>, IBudgetCategoryViewManager
    {
        public BudgetCategoryViewManager(ApplicationContext context):base(context)
        {
            store = new BudgetCategoryViewStore(this.dbContext);
        }
        public List<BudgetCategory> GetConfigBudgetCategoryData()
        {           
            List<BudgetCategory> configBudgetCategoryList = store.Load();
            return configBudgetCategoryList;
        }
        public  DataTable LoadCategories()
        {
            List<BudgetCategory> configBudgetCategoryList = store.Load();
            return UGITUtility.ToDataTable<BudgetCategory>(configBudgetCategoryList);
        }

        /// <summary>
        /// Load categories by userid and authorizetype
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="authorizeType">1=authorize to view and 2=authorize to edit</param>
        /// <returns></returns>
        public List<BudgetCategory> LoadCategories(string userid, int authorizeType)
        {
            List<BudgetCategory> result = null;
            string authorizeView = DatabaseObjects.Columns.AuthorizedToView;
            if (authorizeType == 2)
            {
                authorizeView = DatabaseObjects.Columns.AuthorizedToEdit;
            }
            result = Load($"{authorizeView} like '%{userid}%' OR {authorizeView} is null");
            return result;
        }
        public List<ListItem> LoadCategoriesDropDownItems(DataTable categoriesTable)
        {
            List<ListItem> items = new List<ListItem>();
            if (categoriesTable == null || categoriesTable.Rows.Count == 0)
                return items;

            bool enableBudgetType = true;//UGITUtility.StringToBoolean(ConfigurationVariable.GetValue(ConfigurationVariableHelper.EnableBudgetCategoryType));
            ListItem ddlItem = null;
            string gap = string.Empty;
            if (enableBudgetType)
            {
                var budgetTypeLookup = categoriesTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.BudgetType)).OrderBy(x => x.Key);
                foreach (var budgetType in budgetTypeLookup)
                {
                    DataRow[] categories = budgetType.ToArray();

                    string budgetTypeVal = budgetType.Key;
                    if (budgetTypeVal == null || budgetTypeVal.Trim() == string.Empty)
                        budgetTypeVal = "(None)";
                    string budgetTypeGLCode = string.Format("{0}", UGITUtility.GetSPItemValue(categories[0], DatabaseObjects.Columns.BudgetTypeCOA));
                    ddlItem = new ListItem(string.Format("{0} {1}", budgetTypeGLCode, budgetTypeVal), budgetType.Key);
                    ddlItem.Attributes.Add("disabled", "disabled");
                    ddlItem.Attributes.Add("glcode", budgetTypeGLCode);
                    ddlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.Color, "Black");
                    ddlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.FontWeight, "bold");
                    ddlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.FontStyle, "italic");
                    items.Add(ddlItem);

                    var categoryLookup = categories.ToLookup(x => x.Field<string>(DatabaseObjects.Columns.BudgetCategoryName)).OrderBy(x => x.Key);
                    foreach (var category in categoryLookup)
                    {
                        gap = string.Empty;
                        if (enableBudgetType)
                            gap = string.Empty;//SPEncode.HtmlDecode("&nbsp;&nbsp;");

                        DataRow[] subCategories = category.ToArray();
                        string categoryGLCode = string.Format(" {0}", UGITUtility.GetSPItemValue(subCategories[0], DatabaseObjects.Columns.BudgetAcronym));
                        ddlItem = new ListItem(string.Format("{0}{1} {2}", gap, categoryGLCode, category.Key), category.Key);
                        ddlItem.Attributes.Add("disabled", "disabled");
                        ddlItem.Attributes.Add("glcode", categoryGLCode);
                        ddlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.Color, "Black");
                        ddlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.FontWeight, "bold");
                        ddlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.FontStyle, "italic");
                        ddlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.PaddingLeft, "5px");
                        items.Add(ddlItem);

                        subCategories = subCategories.OrderBy(x => x.Field<string>(DatabaseObjects.Columns.BudgetSubCategory)).ToArray();
                        foreach (DataRow subCategory in subCategories)
                        {
                            if (enableBudgetType)
                                gap = string.Empty;//SPEncode.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                            else
                                gap = string.Empty;//SPEncode.HtmlDecode("&nbsp;&nbsp;");

                            string subCategoryGLCode = string.Format("{0}", UGITUtility.GetSPItemValue(subCategory, DatabaseObjects.Columns.BudgetCOA));
                            ListItem subCDdlItem = new ListItem(string.Format("{2}{1} {0}", subCategory[DatabaseObjects.Columns.BudgetSubCategory], subCategoryGLCode, gap), Convert.ToString(subCategory[DatabaseObjects.Columns.Id]));
                            subCDdlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.PaddingLeft, "10px");
                            subCDdlItem.Attributes.Add("glcode", subCategoryGLCode);
                            items.Add(subCDdlItem);
                        }
                    }
                }
            }
            else
            {
                var categoryLookup = categoriesTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.BudgetCategory)).OrderBy(x => x.Key);
                foreach (var category in categoryLookup)
                {
                    gap = string.Empty;
                    if (enableBudgetType)
                        gap = string.Empty;//SPEncode.HtmlDecode("&nbsp;&nbsp;");

                    DataRow[] subCategories = category.ToArray();
                    string categoryGLCode = string.Format("{0}", UGITUtility.GetSPItemValue(subCategories[0], DatabaseObjects.Columns.BudgetAcronym));
                    ddlItem = new ListItem(string.Format("{0}{1} {2}", gap, categoryGLCode, category.Key), category.Key);
                    ddlItem.Attributes.Add("disabled", "disabled");
                    ddlItem.Attributes.Add("glcode", categoryGLCode);
                    ddlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.Color, "Black");
                    ddlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.FontWeight, "bold");
                    ddlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.FontStyle, "italic");
                    ddlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.PaddingLeft, "5px");
                    items.Add(ddlItem);

                    subCategories = subCategories.OrderBy(x => x.Field<string>(DatabaseObjects.Columns.BudgetSubCategory)).ToArray();
                    foreach (DataRow subCategory in subCategories)
                    {
                        if (enableBudgetType)
                            gap = string.Empty; //SPEncode.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                        else
                            gap = string.Empty; //SPEncode.HtmlDecode("&nbsp;&nbsp;");

                        string subCategoryGLCode = string.Format("{0}", UGITUtility.GetSPItemValue(subCategory, DatabaseObjects.Columns.BudgetCOA));
                        ListItem subCDdlItem = new ListItem(string.Format("{2}{1} {0}", subCategory[DatabaseObjects.Columns.BudgetSubCategory], subCategoryGLCode, gap), Convert.ToString(subCategory[DatabaseObjects.Columns.Id]));
                        subCDdlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.PaddingLeft, "10px");
                        subCDdlItem.Attributes.Add("glcode", subCategoryGLCode);
                        items.Add(subCDdlItem);
                    }
                }
            }
            return items;
        }
        public  BudgetCategory GetBudgetCategoryById(long id)
        {
             return store.LoadByID(id);
            //BudgetCategories budgetCategory = null;
            //try
            //{
            //    DataTable categoryMappingList = LoadCategories();
            //    DataRow category = categoryMappingList.Select(string.Format("{0}='{1}'",DatabaseObjects.Columns.ID,id))[0];
            //    //SPList categoryMappingList = SPListHelper.GetSPList(DatabaseObjects.Lists.BudgetCategories);
            //    //SPListItem category = SPListHelper.GetSPListItem(categoryMappingList, id);
            //    if (category != null)
            //    {
            //        budgetCategory = new BudgetCategories();
            //        budgetCategory.ID = Convert.ToInt32(UGITUtility.GetSPItemValue(category, DatabaseObjects.Columns.Id));
            //        budgetCategory.BudgetType = Convert.ToString(UGITUtility.GetSPItemValue(category, DatabaseObjects.Columns.BudgetType));
            //        budgetCategory.BudgetCategory = Convert.ToString(UGITUtility.GetSPItemValue(category, DatabaseObjects.Columns.BudgetCategory));
            //        budgetCategory.BudgetSubCategory = Convert.ToString(UGITUtility.GetSPItemValue(category, DatabaseObjects.Columns.BudgetSubCategory));
            //        budgetCategory.BudgetCOA = Convert.ToString(UGITUtility.GetSPItemValue(category, DatabaseObjects.Columns.BudgetCOA));
            //       // budgetCategory.AuthorizedToEdit = UGITUtility.IsSPItemExist(category, DatabaseObjects.Columns.AuthorizedToEdit) ? (SPFieldUserValueCollection)uHelper.GetSPItemValue(category, DatabaseObjects.Columns.AuthorizedToEdit) : null;
            //        //budgetCategory.AuthorizedToView = UGITUtility.IsSPItemExist(category, DatabaseObjects.Columns.AuthorizedToView) ? (SPFieldUserValueCollection)uHelper.GetSPItemValue(category, DatabaseObjects.Columns.AuthorizedToView) : null;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //Log.WriteException(ex, "Error getting budget categories");
            //}
            //return budgetCategory;
        }
    }
    public interface IBudgetCategoryViewManager : IManagerBase<BudgetCategory>
    {
        BudgetCategory GetBudgetCategoryById(long id);
        DataTable LoadCategories();
        List<ListItem> LoadCategoriesDropDownItems(DataTable categoriesTable);
    }
}
