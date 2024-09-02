using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Collections.Generic;
using DevExpress.Data.Filtering;
using DevExpress.Web.Rendering;
using System.Collections;
using System.Web;
using System.Linq;
using Microsoft.AspNet.Identity.Owin;

namespace uGovernIT.Web
{
    public partial class FunctionalAreaView : UserControl
    {
        private string addNewItem = string.Empty;
        public DataRow[] moduleColumns;

        #region constant
        private const string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}";
        private const string absoluteUrlView = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&showdelete={2}";
        private const string formTitle = "Functional Area";
        private const string viewParam = "funcareaview";
        private const string newParam = "funcareanew";
        private const string editParam = "funcareaedit";
        // protected string departmentLabel;
        #endregion
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        private FieldConfigurationManager _fmanger = null;
        protected FieldConfigurationManager FieldConfigurationManager
        {
            get
            {
                if (_fmanger == null)
                {
                    _fmanger = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                }
                return _fmanger;
            }
        }
        public UserProfileManager UserManager;
        protected override void OnInit(EventArgs e)
        {
            UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, "0"));
            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','650','400',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','650','400',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            base.OnInit(e);

        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["showdelete"] != null)
                {
                    chkShowDeleted10.Checked = Convert.ToString(Request["showdelete"]) == "0" ? false : true;
                }
            }
            BindGrid();
            base.OnLoad(e);
        }

        void BindGrid()
        {
            FunctionalAreasManager functionAreaManager = new FunctionalAreasManager(context);

            List<FunctionalArea> lstFunctionalArea = functionAreaManager.Load();

            if (lstFunctionalArea != null && !chkShowDeleted10.Checked)
            {
                lstFunctionalArea = lstFunctionalArea.Where(x => !x.Deleted).ToList();
            }

            if (lstFunctionalArea != null)
            {
                dxgridview.DataSource =UGITUtility.ToDataTable(lstFunctionalArea.OrderBy(x => x.Title).ToList());
                dxgridview.DataBind();
            }
        }

        protected void chkShowDeleted_CheckedChanged1(object sender, EventArgs e)
        {
            string showdelete = chkShowDeleted10.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, viewParam, formTitle, showdelete));
            Response.Redirect(url);
        }

        protected void dxgridview_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == "Title")
            {
                int index = e.VisibleIndex;
                string dataKeyValue = Convert.ToString(e.KeyValue);
                string title = Convert.ToString(e.GetValue(DatabaseObjects.Columns.Title));
                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, dataKeyValue));
                string url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','650','400',0,'{2}','true')", editItem, title, Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
                HtmlAnchor aHtml = (HtmlAnchor)dxgridview.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
                aHtml.Attributes.Add("href", url);
                if (e.DataColumn.FieldName == DatabaseObjects.Columns.Title)
                {
                    aHtml.InnerText = e.CellValue.ToString();
                }
            }

        }
        protected void dxgridview_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {
            List<KeyValuePair<string, string>> nameCollection = new List<KeyValuePair<string, string>>();
            foreach (FilterValue fValue in e.Values)
            {
                if (fValue.ToString() != "(All)" && fValue.ToString() != "(Blanks)" && fValue.ToString() != "(Non blanks)")
                {
                    string values = Convert.ToString(fValue);
                    LookUpValueCollection lookUPValueCollection = new LookUpValueCollection(context, e.Column.FieldName, values, true);
                    if (lookUPValueCollection != null)
                    {
                        foreach (LookupValue lookUpValue in lookUPValueCollection)
                        {
                            nameCollection.Add(new KeyValuePair<string, string>(lookUpValue.Value, lookUpValue.ID));

                        }
                        if (lookUPValueCollection.Count == 0)
                        {
                            nameCollection.Add(new KeyValuePair<string, string>(fValue.ToString(), fValue.ToString()));
                        }
                    }
                }
                else
                {
                    nameCollection.Add(new KeyValuePair<string, string>(fValue.ToString(), fValue.ToString()));

                }
            }
            e.Values.Clear();
            foreach (KeyValuePair<string, string> s in nameCollection)
            {
                FilterValue v = new FilterValue(s.Key, s.Value);
                e.Values.Add(v);
            }
        }

        protected void btnApplyChanges_Click(object sender, EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            string cacheName = "Lookup_" + DatabaseObjects.Tables.FunctionalAreas + "_" + context.TenantID;
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.FunctionalAreas, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            Util.Cache.CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);
        }

        protected void dxgridview_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName.Contains("Lookup"))
            {
                string lookupid = Convert.ToString(e.Value);
                string values = FieldConfigurationManager.GetFieldConfigurationData(e.Column.FieldName, Convert.ToString(e.Value));
                if (!string.IsNullOrEmpty(values))
                {
                    e.DisplayText = values;
                }
            }
            if (e.Column.FieldName.EndsWith("User"))
            {
                string userIDs = Convert.ToString(e.Value);
                if (!string.IsNullOrEmpty(userIDs))
                {
                    if (userIDs != null)
                    {
                        string separator = Constants.Separator6;
                        if (userIDs.Contains(Constants.Separator))
                            separator = Constants.Separator;
                        List<string> userlist = UGITUtility.ConvertStringToList(userIDs, separator);

                        string commanames = UserManager.CommaSeparatedNamesFrom(userlist, Constants.Separator6);
                        e.DisplayText = !string.IsNullOrEmpty(commanames) ? commanames : string.Empty;
                    }
                }
            }
        }
    }
}
