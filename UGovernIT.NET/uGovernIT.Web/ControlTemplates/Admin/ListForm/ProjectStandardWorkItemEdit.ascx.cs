using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Web
{
    public partial class ProjectStandardWorkItemEdit : UserControl
    {
        public int Id { get; set; }
        ProjectStandardWorkItem _SPListItem = new ProjectStandardWorkItem();
        //private DataTable _SPList;
        List<ProjectStandardWorkItem> _SPList = new List<ProjectStandardWorkItem>();
        //private DataTable _DataTable;

        protected ApplicationContext dbContext = HttpContext.Current.GetManagerContext();
        ProjectStandardWorkItemManager ObjProjectStandardWorkItemsManager = null;
        BudgetCategoryViewManager budgetCategoryManager = null;
        protected override void OnInit(EventArgs e)
        {
            budgetCategoryManager = new BudgetCategoryViewManager(dbContext);
            ObjProjectStandardWorkItemsManager = new ProjectStandardWorkItemManager(dbContext);

            _SPList = ObjProjectStandardWorkItemsManager.Load();
            if (Id > 0)
            {
                _SPListItem = _SPList.Where(x=> x.ID == Id).FirstOrDefault();

            }
            BindBudgetCategory();
            Fill();
            base.OnInit(e);
        }

        protected void BindBudgetCategory()
        {
            if (ddlBudgetCategory.Items.Count <= 0)
            {
                DataTable budgets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.BudgetCategories, $"{DatabaseObjects.Columns.TenantID}='{dbContext.TenantID}'");
                if (budgets != null && budgets.Rows.Count > 0)
                {
                    DataTable budgetCategories = budgets.AsDataView().ToTable(true, DatabaseObjects.Columns.BudgetCategoryName);
                    foreach (DataRow row in budgetCategories.Rows)
                    {
                        ddlBudgetCategory.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.BudgetCategoryName]), Convert.ToString(row[DatabaseObjects.Columns.BudgetCategoryName])));
                    }
                    ddlBudgetCategory.Items.Insert(0, new ListItem("(None)", string.Empty));
                }
            }
        }

        protected void DDLBudgetCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSubBudgetCategory.Items.Clear();
            DataTable budgets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.BudgetCategories, $"{DatabaseObjects.Columns.TenantID}='{dbContext.TenantID}'");
            if (budgets != null && budgets.Rows.Count > 0)
            {
                DataRow[] budgetCategories = budgets.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.BudgetCategoryName, ddlBudgetCategory.SelectedValue));
                foreach (DataRow row in budgetCategories)
                {
                    ddlSubBudgetCategory.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.BudgetSubCategory]), Convert.ToString(row[DatabaseObjects.Columns.Id])));
                }
                ddlSubBudgetCategory.Items.Insert(0, new ListItem("(None)", string.Empty));
            }
        }

        private void Fill()
        {
            if (!IsPostBack)
            {
                ///set budget item value
                //DataTable budgets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.BudgetCategories, $"{DatabaseObjects.Columns.TenantID}='{dbContext.TenantID}'");
                //string[] values = Convert.ToString(_SPListItem[DatabaseObjects.Columns.BudgetIdLookup]).Split(';');
                
               // int? value = _SPListItem.BudgetCategoryLookup;

                //if (budgets != null && budgets.Rows.Count > 0)
                //{
                //    DataRow[] budgetSubCategories = budgets.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.BudgetCategory, dt.Rows[0]["BudgetCategory"]).ToString());
                //    foreach (DataRow row in budgetSubCategories)
                //    {
                //        ddlSubBudgetCategory.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.BudgetSubCategory]), Convert.ToString(row[DatabaseObjects.Columns.Id])));
                //    }
                //    ddlSubBudgetCategory.Items.Insert(0, new ListItem("(None)", string.Empty));
                //    ddlSubBudgetCategory.Items.FindByValue(Convert.ToString(userInfo.BudgetCategory)).Selected = true;
                //    DataTable budgetCategories = budgets.AsDataView().ToTable(true, DatabaseObjects.Columns.BudgetCategory);
                //    foreach (DataRow row in budgetCategories.Rows)
                //    {
                //        ddlBudgetCategory.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.BudgetCategory]), Convert.ToString(row[DatabaseObjects.Columns.BudgetCategory])));
                //    }
                //    ddlBudgetCategory.Items.Insert(0, new ListItem("(None)", string.Empty));
                //    ddlBudgetCategory.Items.FindByText(dt.Rows[0]["BudgetCategory"].ToString()).Selected = true;
                //}

                // budgetList.AsEnumerable().Where(x=>x.id)
                //SPListItem item = SPListHelper.GetSPListItem(budgetList, uHelper.StringToInt(values[0]));
                // ddlBudgetCategory.SelectedIndex = ddlBudgetCategory.Items.IndexOf(ddlBudgetCategory.Items.FindByText(Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.BudgetCategory))));
                // DDLBudgetCategory_SelectedIndexChanged(ddlSubBudgetCategory, null);
                //ddlSubBudgetCategory.SelectedIndex = ddlSubBudgetCategory.Items.IndexOf(ddlSubBudgetCategory.Items.FindByValue(Convert.ToString(values[0])));
                /////Set Project Standard Work Item title

                List<BudgetCategory> listBudgetCategory = budgetCategoryManager.GetConfigBudgetCategoryData().ToList();

                if (listBudgetCategory != null && listBudgetCategory.Count() > 0)
                {
                    ddlSubBudgetCategory.Items.Clear();
                    BudgetCategory objBudgetCategory = listBudgetCategory.Where(x => x.ID == _SPListItem.BudgetCategoryLookup).FirstOrDefault();
                    if (objBudgetCategory != null)
                    {
                        ddlSubBudgetCategory.DataSource = (from x in listBudgetCategory
                                                           where x.BudgetCategoryName == objBudgetCategory.BudgetCategoryName
                                                           orderby x.BudgetSubCategory
                                                           select new { x.ID, x.BudgetSubCategory }).ToList();
                        ddlSubBudgetCategory.DataValueField = DatabaseObjects.Columns.Id;
                        ddlSubBudgetCategory.DataTextField = DatabaseObjects.Columns.BudgetSubCategory;
                        ddlSubBudgetCategory.DataBind();
                        ddlSubBudgetCategory.Items.FindByValue(Convert.ToString(_SPListItem.BudgetCategoryLookup)).Selected = true;
                        ddlBudgetCategory.Items.FindByText(objBudgetCategory.BudgetCategoryName).Selected = true;
                    }
                    ddlSubBudgetCategory.Items.Insert(0, new ListItem("(None)", string.Empty));
                }

                txtProjectWorkItem.Text = _SPListItem.Title;
                txtItemOrder.Text = _SPListItem.ItemOrder;
                txtCode.Text = _SPListItem.Code;
                txtDescription.Text = _SPListItem.Description;
                if (_SPListItem.Deleted)
                    chkDelete.Checked = true;

                //lnkDelete.Visible = true;
                //if (Convert.ToBoolean(_SPListItem[DatabaseObjects.Columns.IsDeleted]))
                //{
                //    deletePanel.Visible = true;
                //    chkDelete.Checked = true;
                //    lnkDelete.Visible = false;
                //}
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            _SPListItem.Title = txtProjectWorkItem.Text.Trim();
            // _SPListItem[DatabaseObjects.Columns.BudgetCategoryLookup] = ddlSubBudgetCategory.SelectedValue.Trim();
            _SPListItem.BudgetCategoryLookup = UGITUtility.StringToInt(ddlSubBudgetCategory.SelectedValue);
            _SPListItem.ItemOrder = txtItemOrder.Text.Trim();
            _SPListItem.Code = txtCode.Text.Trim();
            _SPListItem.Description = txtDescription.Text;
            _SPListItem.Deleted = chkDelete.Checked;            
            ObjProjectStandardWorkItemsManager.Update(_SPListItem);

            uHelper.ClosePopUpAndEndResponse(Context, true);            
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            _SPListItem.Title = txtProjectWorkItem.Text.Trim();
             
            _SPListItem.ItemOrder = txtItemOrder.Text.Trim();

            _SPListItem.Deleted = chkDelete.Checked;

            ObjProjectStandardWorkItemsManager.Update(_SPListItem);


            //SPListItem item = _SPList.GetItemById(Id);
            //if (item != null)
            //{
            //    SPQuery sQuery = new SPQuery();
            //    sQuery.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}'/><Value Type='Boolean'>{1}</Value></Eq><Eq><FieldRef Name='{2}'/><Value Type='Text'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.StandardWorkItem, 1, DatabaseObjects.Columns.TaskID, item.ID);
            //    SPListItemCollection items = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.TicketHours, sQuery);
            //    //Check if standard work item has entry in ticket hours then just make it soft delete.
            //    if (items.Count > 0)
            //    {
            //        item[DatabaseObjects.Columns.IsDeleted] = true;
            //        item.Update();
            //    }
            //    else
            //        item.Recycle();
            //}
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void rfcProjectWorkItem_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //if (_SPList != null)
            //{
            //    SPQuery query = new SPQuery();
            //    query.ViewFields = string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title);
            //    query.ViewFieldsOnly = true;
            //    query.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq><Neq><FieldRef Name='{2}'/><Value Type='Counter'>{3}</Value></Neq></And></Where>",
            //        DatabaseObjects.Columns.Title, txtProjectWorkItem.Text.Trim(), DatabaseObjects.Columns.Id, Id);
            //    _DataTable = SPListHelper.GetDataTable(_SPList, query);
            //    if (_DataTable != null && _DataTable.Rows.Count > 0)
            //    {
            //        args.IsValid = false;
            //    }
            //}
        }

    }
}