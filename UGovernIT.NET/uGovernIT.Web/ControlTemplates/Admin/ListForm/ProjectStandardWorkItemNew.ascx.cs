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
    public partial class ProjectStandardWorkItemNew : UserControl
    {
        //private DataTable _SPListItem;
        private DataTable _SPList;
        private DataRow[] _DataTable;
        protected ApplicationContext dbContext = HttpContext.Current.GetManagerContext();

        ProjectStandardWorkItemManager ObjProjectStandardWorkItemsManager = new ProjectStandardWorkItemManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            // _SPList = SPListHelper.GetSPList(DatabaseObjects.Lists.ProjectStandardWorkItems);
            _SPList = ObjProjectStandardWorkItemsManager.GetDataTable();
            // _SPListItem = SPListHelper.GetSPList(DatabaseObjects.Lists.ProjectStandardWorkItems).AddItem();
            // ObjProjectStandardWorkItemsManager.Insert();
            BindBudgetCategory();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (!IsStdWorkItemExist())
            {
                var _SPListItem = new ProjectStandardWorkItem();
                //_SPListItem[DatabaseObjects.Columns.Title] = txtProjectWorkItem.Text.Trim();
                //_SPListItem[DatabaseObjects.Columns.BudgetCategoryLookup] = ddlSubBudgetCategory.SelectedValue.Trim();
                //_SPListItem[DatabaseObjects.Columns.ItemOrder] = uHelper.StringToDouble(txtItemOrder.Text.Trim());
                //_SPListItem.Update();

                _SPListItem.Title = txtProjectWorkItem.Text.Trim();
                _SPListItem.BudgetCategoryLookup = UGITUtility.StringToInt(ddlSubBudgetCategory.SelectedValue.Trim());
                _SPListItem.ItemOrder = txtItemOrder.Text.Trim();
                _SPListItem.Code = txtCode.Text.Trim();
                _SPListItem.Description = txtDescription.Text;
                ObjProjectStandardWorkItemsManager.Insert(_SPListItem);

                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void rfcProjectWorkItem_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (_SPList != null)
            {
                // SPQuery query = new SPQuery();
                // query.ViewFields = string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title);
                // query.ViewFieldsOnly = true;
                string query = string.Empty;
                //  query.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.Title, txtProjectWorkItem.Text.Trim());
                query = string.Format("{0}= '{1}'", DatabaseObjects.Columns.Title, txtProjectWorkItem.Text.Trim());
                  _DataTable = _SPList.Select(query);
                if (_DataTable != null && _DataTable.Count() > 0)
                {
                    args.IsValid = false;
                }
            }
        }

        private bool IsStdWorkItemExist()
        {
            //SPQuery query = new SPQuery();
            string query = string.Empty;
            //query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/>",
            //                                 DatabaseObjects.Columns.Title, DatabaseObjects.Columns.Id, DatabaseObjects.Columns.IsDeleted);
            // query.ViewFieldsOnly = true;
            query = string.Format(" {0} ='{1}' and Code='{2}'", DatabaseObjects.Columns.Title, txtProjectWorkItem.Text.Trim(), txtCode.Text.Trim());
            DataTable items = ObjProjectStandardWorkItemsManager.GetDataTable(query);
              //   items.Select(query);
            if (items != null && items.Rows.Count > 0)
            {
                pcMessage.ShowOnPageLoad = true;
                if (Convert.ToString(items.Rows[0][DatabaseObjects.Columns.Deleted]) == "True")
                {
                    btnRestore.Visible = true;
                    btnRestore.CommandArgument = Convert.ToString(items.Rows[0][DatabaseObjects.Columns.Id]);
                    lblMessage.Text = "Item is already in archive. Do you want to restore it?";
                    lblMessage.Font.Bold = true;
                }
                else
                {
                    lblMessage.Text = "Item is already exist. Please create with different title.";
                    btnRestore.Visible = false;
                    lblMessage.Font.Bold = true;
                }
                return true;
            }
            return false;
        }

        //protected void btnRestore_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    int Id = Convert.ToInt32(btn.CommandArgument);
        //    _SPListItem = _SPList.Select("{0}={1}",DatabaseObjects.Columns.ID, Id);
        //    if (_SPListItem != null)
        //    {
        //        _SPListItem[DatabaseObjects.Columns.IsDeleted] = false;
        //        _SPListItem[DatabaseObjects.Columns.BudgetCategoryLookup] = ddlSubBudgetCategory.SelectedValue.Trim();
        //        _SPListItem[DatabaseObjects.Columns.ItemOrder] = UGityUtility.StringToDouble(txtItemOrder.Text.Trim());
        //        _SPListItem.Update();
        //    }
        //    UGityUtility.ClosePopUpAndEndResponse(Context, true);
        //}

    }
}