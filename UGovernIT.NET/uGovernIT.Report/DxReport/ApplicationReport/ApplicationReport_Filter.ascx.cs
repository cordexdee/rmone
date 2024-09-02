using DevExpress.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Report.DxReport.ApplicationReport
{
    public partial class ApplicationReport_Filter : UserControl
    {
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        DataTable appDataTable = null;
        public string delegateControl = "BuildReport.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tblApplicationFilters.Height = "342px";
                trApplications.Visible = true;
                trUsers.Visible = false;
                lnkSubmit.Visible = false;
                BindApplications();
                BindCategory();
            }
        }

        protected void rbSelectionType_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbSelection = (RadioButton)sender;
            string rbText = rbSelection.Attributes["RbText"];
            if (!string.IsNullOrEmpty(rbText) && rbText.ToLower() == "application")
                ClearAppCategoryControls();
            else
                ClearUserControls();
        }

        private void BindApplications()
        {
            lstApplicationsLHS.Items.Clear();

            appDataTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"TenantID='{_context.TenantID}'");
            DataTable dtApplications = appDataTable;
            if (appDataTable != null && appDataTable.Rows.Count > 0)
            {
                DataView view = appDataTable.DefaultView;
                view.Sort = "Title asc";
                dtApplications = view.ToTable();
                lstApplicationsLHS.TextField = DatabaseObjects.Columns.Title;
                lstApplicationsLHS.ValueField = DatabaseObjects.Columns.ID;
            }
            lstApplicationsLHS.DataSource = dtApplications;
            lstApplicationsLHS.DataBind();
        }

        protected void BindUsers()
        {
            UserProfileManager userProfileManager = new UserProfileManager(_context);
            lstUsersLHS.Items.Clear();
            DataTable applUsers = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplModuleRoleRelationship, $"TenantID='{_context.TenantID}'");
            if (applUsers != null && applUsers.Rows.Count > 0)
            {
                IEnumerable distinctUsersList = applUsers.AsEnumerable().Cast<DataRow>().Select(itm => itm[DatabaseObjects.Columns.ApplicationRoleAssign]).Distinct();
                if (distinctUsersList != null)
                {
                    IEnumerator enumerator = distinctUsersList.GetEnumerator();
                    UserProfile spUser = null;
                    while (enumerator.MoveNext())
                    {
                        spUser = userProfileManager.GetUserById(Convert.ToString(enumerator.Current));
                        if (spUser != null)
                        {
                            lstUsersLHS.Items.Add(Convert.ToString(spUser.Name), Convert.ToString(spUser.Id));
                        }
                    }
                }
            }
        }

        private void BindCategory()
        {
            DataTable dataTable = new DataTable();
            if(appDataTable==null)
                appDataTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"TenantID='{_context.TenantID}'");

            lstCategoriesLHS.Items.Clear();
            if (appDataTable != null && appDataTable.Rows.Count > 0)
            {
                dataTable = appDataTable;
                DataTable dtCategories = dataTable.DefaultView.ToTable(true, DatabaseObjects.Columns.CategoryNameChoice);
                for (int i = dtCategories.Rows.Count - 1; i >= 0; i--)
                {
                    if (dtCategories.Rows[i][DatabaseObjects.Columns.CategoryNameChoice] == DBNull.Value)
                        dtCategories.Rows[i].Delete();
                }

                dtCategories.AcceptChanges();
                lstCategoriesLHS.TextField = DatabaseObjects.Columns.CategoryNameChoice;
                lstCategoriesLHS.ValueField = DatabaseObjects.Columns.CategoryNameChoice;
                lstCategoriesLHS.DataSource = dtCategories;
                lstCategoriesLHS.DataBind();
            }
        }

        protected void btnMoveSelectedItemsToRightApplication_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstApplicationsLHS, lstApplicationRHS);
            DisableEnableButtonControl(false, true, false, true, "applications");
            if (lstApplicationRHS.Items.Count > 0)
                lnkSubmit.Visible = true;
        }

        protected void btnMoveAllItemsToRightApplication_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstApplicationsLHS, lstApplicationRHS);
            DisableEnableButtonControl(false, false, false, true, "applications");
            if (lstApplicationRHS.Items.Count > 0)
                lnkSubmit.Visible = true;
        }

        protected void btnMoveSelectedItemsToLeftApplication_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstApplicationRHS, lstApplicationsLHS);
            DisableEnableButtonControl(false, true, false, true, "applications");
            if (lstApplicationRHS.Items.Count > 0)
                lnkSubmit.Visible = true;
        }

        protected void btnMoveAllItemsToLeftApplication_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstApplicationRHS, lstApplicationsLHS);
            DisableEnableButtonControl(false, false, false, true, "applications");
            lnkSubmit.Visible =  false;
        }

        protected void btnMoveSelectedItemsToRightUsers_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstUsersLHS, lstUsersRHS);
            DisableEnableButtonControl(false, true, false, true, "users");
            BindUsers();
            if (lstUsersRHS.Items.Count > 0)
                lnkSubmit.Visible =  true;
        }

        protected void btnMoveAllItemsToRightUsers_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstUsersLHS, lstUsersRHS);
            DisableEnableButtonControl(false, false, false, true, "users");
            BindUsers();
            if (lstUsersRHS.Items.Count > 0)
                lnkSubmit.Visible = true;
        }

        protected void btnMoveSelectedItemsToLeftUsers_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstUsersRHS, lstUsersLHS);
            DisableEnableButtonControl(false, true, false, true, "users");
            BindUsers();
            if (lstUsersRHS.Items.Count > 0)
                lnkSubmit.Visible = true;
        }

        protected void btnMoveAllItemsToLeftUsers_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstUsersRHS, lstUsersLHS);
            DisableEnableButtonControl(false, false, false, true, "users");
            BindUsers();
            lnkSubmit.Visible = false;
        }
        protected void btnMoveSelectedItemsToRightCategories_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstCategoriesLHS, lstCategoriesRHS);
            DisableEnableButtonControl(false, true, false, true, "categories");
            BindCategory();
            SetSelectedApplications();
        }

        private void SetSelectedApplications()
        {
            List<string> lstCategory = new List<string>();
            DataTable dtApplications = new DataTable();
            if(appDataTable==null)
                appDataTable= GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"TenantID='{_context.TenantID}'");
            dtApplications = appDataTable;
            if (dtApplications != null && dtApplications.Rows.Count > 0)
            {
                DataTable dtAppCopy = dtApplications.Clone();
                foreach (ListEditItem item in lstCategoriesRHS.Items)
                {
                    DataRow[] drApps = dtApplications.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.CategoryNameChoice, item.Value));
                    foreach (DataRow dr in drApps)
                    {
                        dtAppCopy.ImportRow(dr);
                    }
                }
                if (dtAppCopy != null && dtAppCopy.Rows.Count > 0)
                {
                    lstApplicationsLHS.Items.Clear();
                    foreach (DataRow dr in dtAppCopy.Rows)
                    {
                        lstApplicationsLHS.Items.Add(Convert.ToString(dr[DatabaseObjects.Columns.Title]), Convert.ToString(dr[DatabaseObjects.Columns.ID]));
                    }
                }
            }
        }

        protected void btnMoveAllItemsToRightCategories_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstCategoriesLHS, lstCategoriesRHS);
            DisableEnableButtonControl(false, false, false, true, "categories");
            BindCategory();
            SetSelectedApplications();
        }

        protected void btnMoveSelectedItemsToLeftCategories_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstCategoriesRHS, lstCategoriesLHS);
            DisableEnableButtonControl(false, true, false, true, "categories");
            BindCategory();
            SetSelectedApplications();
        }

        protected void btnMoveAllItemsToLeftCategories_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstCategoriesRHS, lstCategoriesLHS);
            DisableEnableButtonControl(false, false, false, true, "categories");
            BindCategory();
            BindApplications();
        }

        protected void lnkBuild_Click(object sender, EventArgs e)
        {
            List<string> lstUsers = new List<string>();
            List<string> lstApplications = new List<string>();
            List<string> lstCategory = new List<string>();
            foreach (ListEditItem item in lstUsersRHS.Items)
            {
                lstUsers.Add(Convert.ToString(item.Value));
            }
            foreach (ListEditItem item in lstApplicationRHS.Items)
            {
                lstApplications.Add(Convert.ToString(item.Value));
            }
            foreach (ListEditItem item in lstCategoriesRHS.Items)
            {
                lstCategory.Add(Convert.ToString(item.Text));
            }
            string UserIds = (lstUsers != null && lstUsers.Count > 0) ? string.Join(",", lstUsers.ToArray()) : "0";
            string ApplicationIds = (lstApplications != null && lstApplications.Count > 0) ? string.Join(",", lstApplications.ToArray()) : "0";
            string CategoryIds = (lstCategory != null && lstCategory.Count > 0) ? string.Join(",", lstCategory.ToArray()) : "0";
            bool isUserType = rbUser.Checked;
            string url = _context.SiteUrl + delegateControl + "?reportName=ApplicationReport&isudlg=1";
            url = url + string.Format("&UserIds={0}", UserIds);
            url = url + string.Format("&ApplicationIds={0}", ApplicationIds);
            url = url + string.Format("&Categories={0}", CategoryIds);
            url = url + string.Format("&isUserType={0}", isUserType);
            url = url + string.Format("&isDetails={0}", chkShowDetail.Checked);
            Response.Redirect(url);
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        private void DisableEnableButtonControl(bool addButton, bool addAllButton, bool removeButton, bool removeAllButton, string controltype)
        {
            switch (controltype.ToLower())
            {
                case "categories":
                    btnMoveAllItemsToLeftCategories.ClientEnabled = removeAllButton;
                    btnMoveSelectedItemsToLeftCategories.ClientEnabled = removeButton;
                    btnMoveAllItemsToRightCategories.ClientEnabled = addAllButton;
                    btnMoveSelectedItemsToLeftCategories.ClientEnabled = addButton;
                    break;
                case "applications":
                    btnMoveAllItemsToLeftApplication.ClientEnabled = removeAllButton;
                    btnMoveSelectedItemsToLeftApplication.ClientEnabled = removeButton;
                    btnMoveAllItemsToRightApplication.ClientEnabled = addAllButton;
                    btnMoveSelectedItemsToLeftApplication.ClientEnabled = addButton;
                    break;
                case "users":
                    btnMoveAllItemsToLeftUsers.ClientEnabled = removeAllButton;
                    btnMoveSelectedItemsToLeftUsers.ClientEnabled = removeButton;
                    btnMoveAllItemsToRightUsers.ClientEnabled = addAllButton;
                    btnMoveSelectedItemsToLeftUsers.ClientEnabled = addButton;
                    break;
                default:
                    break;
            }
        }

        private void MoveSelectedItem(ASPxListBox lstFrom, ASPxListBox lstTo)
        {
            if (lstFrom.SelectedItems.Count > 0)
            {
                while (lstFrom.SelectedItems.Count > 0)
                {
                    ListEditItem item = lstFrom.SelectedItems[0];
                    lstTo.Items.Add(item.Text, item.Value);
                    lstFrom.Items.RemoveAt(item.Index);
                }
            }
        }

        private void MoveAllItem(ASPxListBox lstFrom, ASPxListBox lstTo)
        {
            if (lstFrom.Items.Count > 0)
            {
                while (lstFrom.Items.Count > 0)
                {
                    ListEditItem item = lstFrom.Items[0];
                    lstTo.Items.Add(item.Text, item.Value);
                    lstFrom.Items.RemoveAt(item.Index);
                }
            }
        }

        private void ClearAppCategoryControls()
        {
            //lstApplicationRHS.Items.Clear();
            //lstCategoriesRHS.Items.Clear();
            tblApplicationFilters.Height = "342px";
            trApplications.Visible = true;
            trUsers.Visible = false;
            trCategory.Visible = true;
            BindApplications();
            BindCategory();

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "UpdateAppCategoryButtonStatus", "<script>UpdateButtonStateCategories(); UpdateButtonStateApplications();</script>", false);
        }

        private void ClearUserControls()
        {
            //lstUsersRHS.Items.Clear();
            tblApplicationFilters.Height = "200px";
            trApplications.Visible = false;
            trUsers.Visible = true;
            trCategory.Visible = false;
            BindUsers();

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "UpdateUsersButtonStatus", "<script>UpdateButtonStateUsers(); </script>", false);
        }
    }
}