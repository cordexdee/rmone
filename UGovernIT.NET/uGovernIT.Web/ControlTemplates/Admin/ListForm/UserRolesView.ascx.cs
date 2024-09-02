using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class UserRolesView : UserControl
    {
        private ApplicationContext _context = null;
        private LandingPagesManager _landingPagesManager = null;

        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }

        protected LandingPagesManager LandingPagesManager
        {
            get
            {
                if (_landingPagesManager == null)
                {
                    _landingPagesManager = new LandingPagesManager(ApplicationContext);
                }
                return _landingPagesManager;
            }
        }

        protected string editUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=adduserroles");

        protected override void OnInit(EventArgs e)
        {
            //List<Role> listUserRole = userRoleManager.GetRoleList();
            var listUserRoles = LandingPagesManager.GetLandingPages().OrderBy(x => x.Name).ToList();
            //UserProfile user = HttpContext.Current.GetManagerContext().CurrentUser;
            //bool isSuperAdmin = HttpContext.Current.GetManagerContext().UserManager.IsSuperAdmin(user);
            //if (!isSuperAdmin)
            //{
            //    listUserRoles = listUserRoles.Where(x => x.Name != "UGITSuperAdmin").ToList();
            //}

            if (listUserRoles != null && listUserRoles.Count > 0)
            {
                aspxGridUserRoles.DataSource = listUserRoles;
                aspxGridUserRoles.DataBind();
            }
            lnkNewuserRole.Attributes.Add("href", string.Format("javascript:NewUserRoleDialog()"));
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
                
        protected void aspxGridUserRoles_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            DataRow currentRow = aspxGridUserRoles.GetDataRow(e.VisibleIndex);
            string func = string.Empty;
            string roleId = string.Empty;

            //if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id) && Convert.ToString(currentRow[DatabaseObjects.Columns.Id]) != string.Empty)
            //{
            //    roleId = currentRow[DatabaseObjects.Columns.Id].ToString().Trim();
            //}
            func = string.Format("openuserRoleDialog('{0}','{1}','{2}','{3}','{4}', 0)", editUrl, string.Format("RoleId={0}", e.KeyValue), "Edit Landing Page", "400px", "350px");
            e.Row.Attributes.Add("onClick", func);
            e.Row.Attributes.Add("style", "cursor:pointer");
        }        

        /*
        protected void aspxGridUserRoles_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit")
            {
                int index = e.VisibleIndex;
                string dataKeyValue = Convert.ToString(e.KeyValue);
                string editItem = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=adduserroles&ID={0} ", dataKeyValue));
                string Url = string.Format("javascript:openuserRoleDialog('{0}','{1}','{2}','{3}','{4}', 0)", editUrl, string.Format("RoleId={0}", dataKeyValue), "Edit Landing Page", "400px", "250px");
                HtmlAnchor aHtml = (HtmlAnchor)aspxGridUserRoles.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
                aHtml.Attributes.Add("href", Url);
                if (e.DataColumn.FieldName == "Name")
                {
                    aHtml.InnerText = e.CellValue.ToString();
                }
            }
        }
        */
        protected void aspxGridUserRoles_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {
            e.Values.RemoveAll(x => string.IsNullOrEmpty(x.Value));
        }
    }
}
