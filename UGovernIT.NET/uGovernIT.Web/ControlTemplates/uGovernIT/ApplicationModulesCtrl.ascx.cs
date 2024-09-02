using DevExpress.Web;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class ApplicationModulesCtrl : UserControl
    {
        public int ApplicationId { get; set; }
        //DataRow[] spListItemColl = null;
        List<ApplicationModule> dt = new List<ApplicationModule>();
        string absoluteUrlEdit = "/layouts//ugovernit/DelegateControl.aspx?control={0}&ItemID={1}&AppId={2}&ItemOrder={3}";
        string addNewItem = string.Empty;
        public bool isUserExists { get; set; }
        uGovernIT.Utility.Entities.UserProfile User = HttpContext.Current.CurrentUser();
        ConfigurationVariableManager configManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        //bool enableAppModuleRoles;
        ApplicationModuleManager applicationModuleManager = new ApplicationModuleManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            DataRow spItem = GetTableDataManager.GetDataRow(DatabaseObjects.Tables.Applications, DatabaseObjects.Columns.ID, Convert.ToInt32(ApplicationId))[0]; // SPListHelper.GetSPListItem(DatabaseObjects.Tables.Applications, ApplicationId);
            isUserExists = Ticket.IsActionUser(HttpContext.Current.GetManagerContext(), spItem, User);
            
            List<ApplicationModule> spList = applicationModuleManager.Load();
            dt = spList.Where(x => x.APPTitleLookup == ApplicationId).OrderBy(x => x.Title).ToList();
            
            BindGrid();

            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, "modulesedit", "0", ApplicationId, "ADD"));
            string url = string.Format("window.parent.UgitOpenPopupDialog('{0}','','{2} - New Item','700','400',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), "Application Module");
            aAddNew.ClientSideEvents.Click = "function(){ " + url  + " }";
            aAddNew.Visible = isUserExists;

            base.OnInit(e);
        }

        private void BindGrid()
        {
            if (dt != null && dt.Count > 0)
            {
                int i = 1;
                dt.OrderBy(x => x.Title);
                foreach (var item in dt)
                {
                    if(i<= dt.Count && (item.ItemOrder==0 || item.ItemOrder==null))
                    {
                        item.ItemOrder = i;
                    }
                    i++;
                }
               
                grdApplModules.DataSource = dt;
            }
            else
                grdApplModules.DataSource = dt;

            grdApplModules.DataBind();
        }
       
       
        protected void grdApplModules_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                string lsDataKeyValue = Convert.ToString(e.KeyValue);
                string editItem;
                string itemOrder = Convert.ToString(e.GetValue(DatabaseObjects.Columns.ItemOrder));
                editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, "modulesedit", lsDataKeyValue, ApplicationId,itemOrder));
                HtmlAnchor anchorEdit = grdApplModules.FindRowCellTemplateControl(e.VisibleIndex, null, "aEdit") as HtmlAnchor;
                string Title = Convert.ToString(e.GetValue("Title"));
                string jsFunc = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2} - Edit Item','700','400',0,'{1}','true')", editItem, Server.UrlEncode(Request.Url.AbsolutePath), Title);
                if (anchorEdit != null)
                {
                    anchorEdit.Attributes.Add("href", jsFunc);
                    anchorEdit.Visible = isUserExists;
                }
                LinkButton lnkDelete = grdApplModules.FindRowCellTemplateControl(e.VisibleIndex, null, "lnkDelete") as LinkButton;
                if(lnkDelete!=null)
                {
                    lnkDelete.CommandArgument = lsDataKeyValue;
                    lnkDelete.Visible = isUserExists;
                 }
            }
        }
        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            LinkButton lnkDelete = sender as LinkButton;
            int dataKey = Convert.ToInt32(lnkDelete.CommandArgument);

            ApplicationRoleManager appRoleMGR = new ApplicationRoleManager(HttpContext.Current.GetManagerContext());
            ApplicationAccessManager appAccessMGR = new ApplicationAccessManager(HttpContext.Current.GetManagerContext());
            List<ApplicationRole> appRoleList = appRoleMGR.Load(x => x.APPTitleLookup == ApplicationId).Where(x => UGITUtility.ConvertStringToList(x.ApplicationRoleModuleLookup, Constants.Separator6).Contains(Convert.ToString(dataKey))).ToList();
            foreach (ApplicationRole item in appRoleList)
            {
                List<ApplicationAccess> lstAccess = appAccessMGR.Load(x => x.ApplicationRoleLookup == item.ID);
                foreach (ApplicationAccess access in lstAccess)
                {
                    appAccessMGR.Delete(access);
                }
                appRoleMGR.Delete(item);
            }
            List<ApplicationAccess> lstAccessWithoutModule = appAccessMGR.Load(x => x.ApplicationModulesLookup == UGITUtility.StringToLong(dataKey));
            foreach (ApplicationAccess item in lstAccessWithoutModule)
            {
                appAccessMGR.Delete(item);
            }
            ApplicationModule roledeleted = dt.FirstOrDefault(x => x.ID == dataKey);
            applicationModuleManager.Delete(roledeleted);

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
            //                            DatabaseObjects.Columns.ApplicationModulesLookup, dataKey);
            //_query.ViewFields = string.Concat(
            //            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.Id),
            //            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.Title),
            //            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.ApplicationRoleLookup),
            //            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.APPTitleLookup),
            //            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.ApplicationModulesLookup),
            //            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.ApplicationRoleAssign)
            //            );
            //SPListItemCollection itemColl = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ApplModuleRoleRelationship, _query);
            //SPListItem item = null;
            //while (itemColl != null && itemColl.Count > 0)
            //{
            //    item = itemColl[0];
            //    item.Delete();
            //}

            //spListItemColl.DeleteItemById(Convert.ToInt32(dataKey));
            //dt = spListItemColl.GetDataTable();

            List<ApplicationModule> spList = applicationModuleManager.Load();
            dt = spList.Where(x => x.APPTitleLookup == ApplicationId).ToList();

            BindGrid();
        }

        protected void grdApplModules_Load(object sender, EventArgs e)
        {
            bool enableAppModuleRoles = configManager.GetValueAsBool(ConfigConstants.EnableAppModuleRoles);
            if (!enableAppModuleRoles)
            {
                ASPxGridView grid = (ASPxGridView)sender;
                grid.Columns["Owner"].Visible = false;
                grid.Columns["SupportedBy"].Visible = false;
                grid.Columns["AccessAdmin"].Visible = false;
                grid.Columns["Approver"].Visible = false;
                grid.Columns["ApprovalTypeChoice"].Visible = false;
            }
        }
    }
}
