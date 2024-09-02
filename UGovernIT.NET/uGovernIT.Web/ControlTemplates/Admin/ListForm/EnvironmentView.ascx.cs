
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Collections.Generic;
using DevExpress.Web;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class EnvironmentView : UserControl
    {
        private List<UGITEnvironment> _SPList;
        private string addNewItem = string.Empty;
        private string absoluteUrlEdit = "/Layouts/ugovernit/DelegateControl.aspx?control={0}&ItemID={1}";
        private string absoluteUrlView = "/Layouts/ugovernit/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&showdelete={2}";
        private string formTitle = "Environment";
        private string viewParam = "environmentview";
        private string newParam = "environmentnew";
        private string editParam = "environmentedit";

        EnvironmentManager environMGR = new EnvironmentManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, "0"));
            string addNewItemURL = string.Format("UgitOpenPopupDialog('{0}','','{2} - New Item','600','330',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
           // aAddItem.ClientSideEvents.Click = "function(){ " + addNewItemURL + " }";
          //  aAddItem_Top.ClientSideEvents.Click = "function(){ " + addNewItemURL + " }";
            addItem.Attributes.Add("onclick", string.Format("UgitOpenPopupDialog('{0}','','{2} - New Item','600','330',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            addItemTop.Attributes.Add("onclick", string.Format("UgitOpenPopupDialog('{0}','','{2} - New Item','600','330',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            //aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','250',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            //aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','250',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["showdelete"] != null)
                {
                    string showdelete = Convert.ToString(Request["showdelete"]);
                    chkShowDeleted.Checked = showdelete == "0" ? false : true;
                }
            }
            BindGriview();
            base.OnLoad(e);
        }

        private void BindGriview()
        {
            _SPList = environMGR.Load().OrderBy(x => x.Title).ToList();  // SPListHelper.GetSPList(DatabaseObjects.Lists.Environment);
            if (_SPList != null && !chkShowDeleted.Checked)
            {
                _SPList = _SPList.Where(x => !x.Deleted).ToList();
            }

            //foreach (DataRow row in _DataTable.Rows)
            //{

            //    row[DatabaseObjects.Columns.UGITDescription] = Convert.ToString(row[DatabaseObjects.Columns.UGITDescription]).Length >= 50 ?
            //                                               Convert.ToString(row[DatabaseObjects.Columns.UGITDescription]).Substring(0, 50) + "..." :
            //                                               Convert.ToString(row[DatabaseObjects.Columns.UGITDescription]);
            //}
            // _DataTable.DefaultView.Sort = "Title ASC";
            _gridView.DataSource = _SPList.OrderBy(x=>x.Title);//_DataTable.DefaultView;
            _gridView.DataBind();
        }
        protected void chkShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            string showdelete = chkShowDeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, viewParam, formTitle, showdelete));
            Response.Redirect(url);
        }

        protected void _gridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data) return;

            ASPxButton aEditButton = _gridView.FindRowCellTemplateControl(e.VisibleIndex, null, "aEdit") as ASPxButton;
            aEditButton.AutoPostBack = false;
            string aTitle = e.GetValue("Title") as string;
            ASPxButton aEditLinkButton = _gridView.FindRowCellTemplateControl(e.VisibleIndex, null, "lblTitle") as ASPxButton;
            aEditLinkButton.Text = aTitle;
            aEditLinkButton.AutoPostBack = false;
            string editItem;
            editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, e.KeyValue));
            string jsFunc = string.Format("UgitOpenPopupDialog('{0}','','{3} - {1}','600','330',0,'{2}','true')", editItem, aTitle, Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
            aEditLinkButton.ClientSideEvents.Click = "function(){ " + jsFunc + " }";
            aEditButton.ClientSideEvents.Click = "function(){ " + jsFunc + " }";
        }
    }
}
