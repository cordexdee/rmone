using DevExpress.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.SqlServer.Server;
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

namespace uGovernIT.Web.ControlTemplates.Admin.ListForm
{
    public partial class DocumentTypeView : UserControl
    {

       // private SPList _SPList;
        //private DataTable _DataTable;
        //SPWeb spweb;
        private string addNewItem = string.Empty;

        DMDocumentTypeListManager dMDocumentTypeListManager = new DMDocumentTypeListManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            gridview.SettingsPager.ShowDisabledButtons = true;
            gridview.SettingsPager.ShowNumericButtons = true;
            gridview.SettingsPager.ShowSeparators = true;
            gridview.SettingsPager.ShowDefaultImages = true;
            gridview.SettingsPager.PageSizeItemSettings.Visible = true;
            gridview.SettingsPager.PageSizeItemSettings.Position = DevExpress.Web.PagerPageSizePosition.Right;

            //addNewItem = uHelper.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, "0"));
            //aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','700','630',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            //aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','700','600',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            //spweb = SPContext.Current.Web;
            //_SPList = data.GetSPList(DatabaseObjects.Lists.DocumentType);

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //var ObjUserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            UserProfileManager ObjUserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
             bool isSuperAdmin = ObjUserManager.IsUGITSuperAdmin(HttpContext.Current.CurrentUser());
            //isSuperAdmin = true;
            if (isSuperAdmin)
                btnrecycle.Visible = true;
            
            //if (UserProfile.IsSuperAdmin(SqlContext.Current.Web.CurrentUser))
            //    btnrecycle.Visible = true;
            BindGrid();
        }
        void BindGrid()
        {
            List<DMDocumentTypeList> dMDocumentTypeList = new List<DMDocumentTypeList>();
            dMDocumentTypeList = dMDocumentTypeListManager.Load();
            dMDocumentTypeList=dMDocumentTypeList.Where(x => x.Deleted == false).ToList();
            if (dMDocumentTypeList.Count> 0)
            {
                  gridview.DataSource = dMDocumentTypeList;
                  gridview.DataBind();
            }
            //if (_SPList != null && _SPList.ItemCount > 0)
            //    _DataTable = _SPList.Items.GetDataTable();
            //else
            //    _DataTable = null;

            //if (_DataTable != null)
            //{
            //    string sort = string.Format("{0} ASC", DatabaseObjects.Columns.Title);
            //    _DataTable.DefaultView.Sort = sort;
            //    gridview.DataSource = _DataTable;
            //    gridview.DataBind();
            //}
        }
        protected void callback_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {

            string mode = string.Empty;
            if (!string.IsNullOrEmpty(Convert.ToString(e.Parameter)))
                mode = Convert.ToString(e.Parameter);
            else
                mode = hdnKeepkeyValue.Contains("mode") ? Convert.ToString(hdnKeepkeyValue.Get("mode")) : string.Empty;

            if (!string.IsNullOrEmpty(mode) && mode == "new")
            {
                EditOrUpdate(mode);
                popupnew.JSProperties.Add("cpCloseNew", true);
            }
            else if (mode == "edit" || mode == "update")
            {
                string key = hdnKeepkeyValue.Contains("KeyValue") ? Convert.ToString(hdnKeepkeyValue.Get("KeyValue")) : string.Empty;
                if (!string.IsNullOrEmpty(key))
                    EditOrUpdate(mode, key);
                if (mode == "update")
                    popupedit.JSProperties.Add("cpCloseEdit", true);
            }
        }
        protected void EditOrUpdate(string mode, string key = "")
        {
            //DataRow item = null;
            DMDocumentTypeList dMDocumentType = new DMDocumentTypeList();
            if (mode == "new")
            {
                if (string.IsNullOrEmpty(txttitle.Text.Trim()))
                    return;
                dMDocumentType.Title = txttitle.Text.Trim();
                dMDocumentType.DMSDescription = txtdexcription.Text.Trim();
                dMDocumentTypeListManager.Insert(dMDocumentType);
                //item = _SPList.AddItem();
                // bool allow = item.Web.AllowUnsafeUpdates;
                //// item.Web.AllowUnsafeUpdates = true;
                //item[DatabaseObjects.Columns.Title] = txttitle.Text.Trim();
                //item[DatabaseObjects.Columns.DMSDescription] = txtdexcription.Text.Trim();
                //item.Update();
                //item.Web.AllowUnsafeUpdates = allow;
            }
            else if (mode == "edit")
            {
                var obj = dMDocumentTypeListManager.LoadByID(Convert.ToInt64(key));
               // item = SPListHelper.GetSPListItem(_SPList, uHelper.StringToInt(key));
                if (obj != null)
                {
                    txttitleedit.Text = obj.Title;
                    txtdesedit.Text = obj.DMSDescription;
                }
            }
            else if (mode == "update")
            {
                if (string.IsNullOrEmpty(txttitleedit.Text.Trim()))
                    return;
                var obj = dMDocumentTypeListManager.LoadByID(Convert.ToInt64(key));

                // item = SPListHelper.GetSPListItem(_SPList, uHelper.StringToInt(key));
                if (obj != null)
                {
                    obj.Title = txttitleedit.Text.Trim();
                    obj.DMSDescription = txtdesedit.Text.Trim();
                    dMDocumentTypeListManager.Update(obj);
                   // bool allow = item.Web.AllowUnsafeUpdates;
                   // item.Web.AllowUnsafeUpdates = true;
                   //item[DatabaseObjects.Columns.Title] = txttitleedit.Text.Trim();
                   //item[DatabaseObjects.Columns.DMSDescription] = txtdesedit.Text.Trim();
                   //item.Update();
                   // item.Web.AllowUnsafeUpdates = allow;
                }
            }
        }
        protected void ASPxCallbackPanel1_Callback(object sender, CallbackEventArgsBase e)
        {
            string mode = string.Empty;
            if (!string.IsNullOrEmpty(Convert.ToString(e.Parameter)))
                mode = Convert.ToString(e.Parameter);
            else
                mode = hdnKeepkeyValue.Contains("mode") ? Convert.ToString(hdnKeepkeyValue.Get("mode")) : string.Empty;

            if (!string.IsNullOrEmpty(mode) && mode == "new" && !string.IsNullOrEmpty(txttitle.Text.Trim()))
            {
                EditOrUpdate(mode);
                popupnew.JSProperties.Add("cpCloseNew", true);
            }
            else if (mode == "edit" || mode == "update")
            {
                string key = hdnKeepkeyValue.Contains("KeyValue") ? Convert.ToString(hdnKeepkeyValue.Get("KeyValue")) : string.Empty;
                if (!string.IsNullOrEmpty(key))
                    EditOrUpdate(mode, key);
                if (mode == "update")
                    popupedit.JSProperties.Add("cpCloseEdit", true);
            }
        }

        protected void popupdelete_Click(object sender, EventArgs e)
        {
            //DataRow item = null;
            string key = hdnKeepkeyValue.Contains("KeyValue") ? Convert.ToString(hdnKeepkeyValue.Get("KeyValue")) : string.Empty;

            if (!string.IsNullOrEmpty(key) && UGITUtility.StringToInt(key) > 0)
            {
                var item = dMDocumentTypeListManager.LoadByID(UGITUtility.StringToInt(key));
                if (item != null)
                {
                    item.Deleted = true;
                    dMDocumentTypeListManager.Update(item);// soft delete
                    //item.Recycle();
                }
            }
            BindGrid();
        }


    }
}