using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity.Owin;

namespace uGovernIT.Web
{
    public partial class BudgetCategoriesView : UserControl
    {
        string addNewItem;
        List<BudgetCategory> listBudgetCategory;
        //SPList spListBGetCat;
        UserProfile user;
        UserProfileManager userManager;
        private string formTitle = "Budget Category";
        private string viewParam = "bgetcatview";
        private string newParam = "bgetcatnew";
        private string editParam = "bgetcatedit";
        protected string importUrl;
        private string absoluteUrlImport = "/layouts/ugovernit/DelegateControl.aspx?control={0}&listName={1}";
        private string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}";
        private string absoluteUrlView = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&budgetype={2}&showdelete={3}";

        ApplicationContext context = HttpContext.Current.GetManagerContext();

        protected override void OnInit(EventArgs e)
        {
            user = HttpContext.Current.CurrentUser();
            userManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

            bool isAdmin = userManager.IsAdmin(user) || userManager.IsUGITSuperAdmin(user);
            ConfigurationVariableManager configVariableManager = new ConfigurationVariableManager(context);
            importUrl = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlImport, "importexcelfile", DatabaseObjects.Tables.BudgetCategories));
            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, "0"));
            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','600',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','600',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            BindBudgetType();
            bool isImportshow = configVariableManager.GetValueAsBool(ConfigConstants.ShowBudgetImport);
            if (isImportshow && isAdmin)
                btnImport.Visible = true;
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["budgetype"] != null)
                {
                    dxddlBudgetType.Value = Convert.ToString(Request["budgetype"]);
                }
                else
                {
                    dxddlBudgetType.SelectedIndex = 0;
                }

                if (Request["showdelete"] != null)
                {
                    dxShowDeleted.Checked = Convert.ToString(Request["showdelete"]) == "0" ? false : true;
                }

            }
            BindGridView();
            base.OnLoad(e);
        }

        private void BindBudgetType()
        {

            if (Request["showdelete"] != null)
            {
                dxShowDeleted.Checked = Convert.ToString(Request["showdelete"]) == "0" ? false : true;
            }

            dxheader.Visible = false;

            BudgetCategoryViewManager budgetCataegoryManager = new BudgetCategoryViewManager(context);
            listBudgetCategory = budgetCataegoryManager.Load(x => !string.IsNullOrEmpty(x.BudgetType));
            if (dxShowDeleted.Checked)
            {
                if (listBudgetCategory != null && listBudgetCategory.Count > 0)
                {
                    dxddlBudgetType.Items.Clear();
                    dxheader.Visible = true;

                    List<BudgetCategory> dtBudgetType = listBudgetCategory.OrderBy(x => x.BudgetType).ToList();

                    if (dtBudgetType != null && dtBudgetType.Count > 0)
                    {
                        foreach (BudgetCategory row in dtBudgetType)
                        {
                            if (Convert.ToString(row.Deleted) == "True")
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(row.BudgetType)))
                                {
                                    if (dxddlBudgetType.Items.FindByTextWithTrim(Convert.ToString(row.BudgetType)) == null)
                                        dxddlBudgetType.Items.Add(Convert.ToString(row.BudgetType));
                                }
                                else
                                {
                                    if (dxddlBudgetType.Items.FindByTextWithTrim("None") == null)
                                        dxddlBudgetType.Items.Add("None");
                                }

                            }
                            else
                            {
                                if (Convert.ToString(row.Deleted) == "False")
                                {
                                    if (!String.IsNullOrEmpty(Convert.ToString(row.BudgetType)))
                                    {
                                        if (dxddlBudgetType.Items.FindByTextWithTrim(Convert.ToString(row.BudgetType)) == null)
                                            dxddlBudgetType.Items.Add(Convert.ToString(row.BudgetType));
                                    }
                                    else
                                    {
                                        if (dxddlBudgetType.Items.FindByTextWithTrim("None") == null)
                                            dxddlBudgetType.Items.Add("None");
                                    }


                                }

                            }
                        }
                    }
                }
            }

            else if (!dxShowDeleted.Checked)
            {
                if (listBudgetCategory.Count > 0)
                {
                    dxheader.Visible = true;

                    List<BudgetCategory> dtBudgetType = listBudgetCategory.OrderBy(x => x.BudgetType).ToList();
                    if (dtBudgetType != null && dtBudgetType.Count > 0)
                    {
                        foreach (BudgetCategory row in dtBudgetType)
                        {
                            if (Convert.ToString(row.Deleted) == "False")
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(row.BudgetType)))
                                {
                                    if (dxddlBudgetType.Items.FindByTextWithTrim(Convert.ToString(row.BudgetType)) == null)
                                        dxddlBudgetType.Items.Add(Convert.ToString(row.BudgetType));
                                }
                                else
                                {
                                    if (dxddlBudgetType.Items.FindByTextWithTrim("None") == null)
                                        dxddlBudgetType.Items.Add("None");
                                }

                            }
                            else
                            {
                                if (Convert.ToString(row.Deleted) == "True")
                                {
                                    if (!String.IsNullOrEmpty(Convert.ToString(row.BudgetType)))
                                    {
                                        if (dxddlBudgetType.Items.FindByTextWithTrim(Convert.ToString(row.BudgetType)) == null)
                                            dxddlBudgetType.Items.Add(Convert.ToString(row.BudgetType));
                                    }

                                    else
                                    {
                                        if (dxddlBudgetType.Items.FindByTextWithTrim("None") == null)
                                            dxddlBudgetType.Items.Add("None");
                                    }

                                }
                            }
                        }
                    }
                }
            }

            // }
        }

        private void BindGridView()
        {
            BudgetCategoryViewManager budgetCataegoryManager = new BudgetCategoryViewManager(context);
            List<BudgetCategory> _dataTable = budgetCataegoryManager.Load();
            string dxddlModuleValue = Convert.ToString(dxddlBudgetType.Value);
            //if (!dxheader.Visible || (dxheader.Visible && dxddlModuleValue == "None"))
            //{
            //    if (!dxShowDeleted.Checked)
            //    {

            //        _dataTable = _dataTable.Where(x => !x.IsDeleted).ToList();


            //    }


            //}
            if (dxheader.Visible && !string.IsNullOrEmpty(dxddlModuleValue))
            {
                if (dxShowDeleted.Checked)
                {
                    _dataTable = _dataTable.Where(x => x.BudgetType == dxddlModuleValue).ToList();

                }
                else if (!dxShowDeleted.Checked)
                {
                    _dataTable = _dataTable.Where(x => x.BudgetType == dxddlModuleValue && !x.Deleted).ToList();

                }
            }
            dxgridview.DataSource = _dataTable;
            dxgridview.DataBind();
            dxgridview.ExpandAll();

        }
        protected void ddlBudgetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // GetRequest();
        }
        private void GetRequest()
        {
            string budgettype = Convert.ToString(dxddlBudgetType.Value);
            string showdelete = dxShowDeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, viewParam, formTitle, budgettype, showdelete));
            Response.Redirect(url);
        }

        protected void dxddlBudgetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetRequest();
        }

        protected void dxShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            GetRequest();
        }

        protected void dxgridview_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == "BudgetSubCategory")
            {
                string Title = Convert.ToString(e.GetValue(DatabaseObjects.Columns.BudgetSubCategory));
                int Index = e.VisibleIndex;
                string datakeyvalue = Convert.ToString(e.KeyValue);
                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, datakeyvalue));
                string Url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {2}','600','600',0,'{1}','true')", editItem, Server.UrlEncode(Request.Url.AbsolutePath), Title, formTitle);
                HtmlAnchor aHtml = (HtmlAnchor)dxgridview.FindRowCellTemplateControl(Index, e.DataColumn, "editlink");
                aHtml.Attributes.Add("href", Url);
                if (e.DataColumn.FieldName == "BudgetSubCategory")
                {
                    aHtml.InnerText = e.CellValue.ToString();
                }
            }
            if (e.DataColumn.FieldName == "CapitalExpenditure" || e.DataColumn.FieldName == "IncludesStaffing")
            {
                string val = e.CellValue.ToString();
                e.Cell.Text = "No";
                if (UGITUtility.StringToBoolean(val))
                {
                    e.Cell.Text = "Yes";
                }
            }
        }
    }
}
