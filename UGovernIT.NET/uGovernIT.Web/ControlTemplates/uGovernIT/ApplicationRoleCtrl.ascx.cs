using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class ApplicationRoleCtrl : UserControl
    {
        public int ApplicationId { get; set; }
        List<ApplicationRole> lstAppRole = new List<ApplicationRole>();
        private string absoluteUrlEdit = "/layouts/ugovernit/DelegateControl.aspx?control={0}&ItemID={1}&AppId={2}&ItemOrder={3}";
        private string addNewItem = string.Empty;
        public bool isUserExists { get; set; }
        DataTable dt = new DataTable();
        ApplicationRoleManager appRoleManager = new ApplicationRoleManager(HttpContext.Current.GetManagerContext());
        uGovernIT.Utility.Entities.UserProfile User = HttpContext.Current.CurrentUser();
        protected override void OnInit(EventArgs e)
        {
            DataRow spItem = GetTableDataManager.GetDataRow(DatabaseObjects.Tables.Applications, DatabaseObjects.Columns.ID, ApplicationId)[0]; // SPListHelper.GetSPListItem(DatabaseObjects.Lists.Applications, ApplicationId);
            isUserExists = Ticket.IsActionUser(HttpContext.Current.GetManagerContext(), spItem, User);

            BindGrid();

            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, "approleedit", "0", ApplicationId, "ADD"));
            string url = string.Format("window.parent.UgitOpenPopupDialog('{0}','','{2} - New Item','600','250',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), "Application Role");
            aAddNew.ClientSideEvents.Click = "function(){ " + url + " }";
            aAddNew.Visible = isUserExists;
            base.OnInit(e);
        }

        private void BindGrid()
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", HttpContext.Current.GetManagerContext().TenantID);
            values.Add("@ApplicationId", Convert.ToString(ApplicationId));
            dt = GetTableDataManager.GetData(DatabaseObjects.Tables.ApplicationRole, values);
            if (dt != null && dt.Rows.Count > 0)
            {
                grdApplRoles.DataSource = dt;
                grdApplRoles.DataBind();
            }
            //lstAppRole = appRoleManager.Load(); // SPListHelper.GetSPList(DatabaseObjects.Lists.ApplicationRole);
            //lstAppRole = lstAppRole.Where(x => x.APPTitleLookup == ApplicationId).ToList(); // SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ApplicationRole, spQuery);

            //if (lstAppRole != null)
            //{
            //    int i = 1;
            //    foreach (ApplicationRole role in lstAppRole)
            //    {
            //        if ( role.ItemOrder == 0)
            //        {
            //            role.ItemOrder = i;
            //        }
            //        i++;
            //        if (!string.IsNullOrEmpty(Convert.ToString(role.ApplicationRoleModuleLookup)) && Convert.ToString(role.ApplicationRoleModuleLookup) != "0")
            //            role.Modules = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(role.ApplicationRoleModuleLookup));
            //        else
            //        {
            //            role.Modules = "All";
            //            role.ApplicationRoleModuleLookup = "All";
            //        }
            //    }
            //}

            //grdApplRoles.DataSource = lstAppRole;
            //grdApplRoles.DataBind();
        }

        protected void grdApplRoles_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                string lsDataKeyValue = Convert.ToString(e.KeyValue);
                string editItem;
                string itemOrder = Convert.ToString(e.GetValue(DatabaseObjects.Columns.ItemOrder));
                editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, "approleedit", lsDataKeyValue, ApplicationId, itemOrder));
                HtmlAnchor anchorEdit = grdApplRoles.FindRowCellTemplateControl(e.VisibleIndex, null, "aEdit") as HtmlAnchor;
                string Title = Convert.ToString(e.GetValue("Title"));
                string jsFunc = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2} - Edit Item','600','290',0,'{1}','true')", editItem, Server.UrlEncode(Request.Url.AbsolutePath), Title);
                if (anchorEdit != null)
                {
                    anchorEdit.Attributes.Add("href", jsFunc);
                    anchorEdit.Visible = isUserExists;
                }
                ASPxButton lnkDelete = grdApplRoles.FindRowCellTemplateControl(e.VisibleIndex, null, "lnkDelete") as ASPxButton;
                if (lnkDelete != null)
                {
                    lnkDelete.CommandArgument = lsDataKeyValue;
                    lnkDelete.Visible = isUserExists;
                }
            }
        }
        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            ASPxButton lnkDelete = sender as ASPxButton;
            int dataKey = Convert.ToInt32(lnkDelete.CommandArgument);
            ApplicationAccessManager appAccessMGR = new ApplicationAccessManager(HttpContext.Current.GetManagerContext());
            List<ApplicationAccess> lstAccess = appAccessMGR.Load(x => x.ApplicationRoleLookup == UGITUtility.StringToLong(dataKey));
            foreach (ApplicationAccess item in lstAccess)
            {
                appAccessMGR.Delete(item);
            }
            lstAppRole = appRoleManager.Load().Where(x => x.APPTitleLookup == ApplicationId && x.ID == dataKey).ToList();
            ApplicationRole role = lstAppRole.FirstOrDefault(x => x.ID == dataKey);
            appRoleManager.Delete(role);
            //SPQuery _query = new SPQuery();
            //_query.Query = string.Format("<Where>" +
            //                                "<And>" +
            //                                    "<Eq>" +
            //                                        "<FieldRef Name='{0}' LookupId='True'/>" +
            //                                        "<Value Type='Lookup'>{1}</Value>" +
            //                                    "</Eq>" +
            //                                    "<Eq>" +
            //                                        "<FieldRef Name='{2}' LookupId='True'/>" +
            //                                        "<Value Type='Lookup'>{3}</Value>" +
            //                                    "</Eq>" +
            //                                "</And>" +
            //                            "</Where>", DatabaseObjects.Columns.APPTitleLookup, ApplicationId,
            //                            DatabaseObjects.Columns.ApplicationRoleLookup, dataKey);

            //var itemColl = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ApplModuleRoleRelationship, _query);
            //SPListItem item = null;
            //while (itemColl.Count > 0)
            //{
            //    item = itemColl[0];
            //    item.Delete();
            //}

            //spListItemColl.DeleteItemById(Convert.ToInt32(dataKey));
            //lstAppRole = appRoleManager.Load(); // SPListHelper.GetSPList(DatabaseObjects.Lists.ApplicationRole);
            //lstAppRole = lstAppRole.Where(x => x.APPTitleLookup == ApplicationId).ToList();

            BindGrid();
        }


    }
}
