using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager.Managers;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
//using System.Web;
using DevExpress.Data.Async.Helpers;
using static uGovernIT.Web.ModuleResourceAddEdit;
using System.Configuration;
using uGovernIT.Web.Controllers;
using DevExpress.ExpressApp;
using DevExpress.XtraRichEdit.Model;
using System.Threading;
using uGovernIT.Helpers;
using System.Windows.Forms;
using uGovernIT.DAL;
using System.Web.Services;
using DevExpress.XtraCharts.Native;
using ApplicationContext = uGovernIT.Manager.ApplicationContext;
using DevExpress.ExpressApp.Utils;
using System.Text;
using DevExpress.Utils.Drawing.Helpers;
using System.Net.Mail;
using System.Net;
using Microsoft.Graph;
using DevExpress.Utils.Extensions;
using DevExpress.Web.Rendering;

namespace uGovernIT.Web.ControlTemplates.Admin.ListForm
{
    public partial class ExperiencedTags : System.Web.UI.UserControl
    {
        ExperiencedTagManager experiencedTagManager = new ExperiencedTagManager(HttpContext.Current.GetManagerContext());
        protected string editUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=addexperiencedtags");
        protected override void OnInit(EventArgs e)
        {
            List<ExperiencedTag> lstExperiencedTags = experiencedTagManager.Load();

            if (lstExperiencedTags != null && lstExperiencedTags.Count > 0)
            {
                aspxGridProjectTags.DataSource = lstExperiencedTags.OrderBy(x => x.Title);
                aspxGridProjectTags.DataBind();
            }
            //  addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, "0"));
            LinkButton1.Attributes.Add("href", string.Format("javascript:NewProjectTagDialog()"));
            lnkAddNewExperiencedTags.Attributes.Add("href", string.Format("javascript:NewProjectTagDialog()"));
            // LinkButton1.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','600',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void aspxGridProjectTags_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            GridViewTableDataCell editCell = null;
            foreach (object cell in e.Row.Cells)
            {
                if (cell is GridViewTableDataCell)
                {
                    editCell = (GridViewTableDataCell)cell;
                    if (editCell.DataColumn.FieldName == "CreatedBy")
                    {
                        editCell.Text = uHelper.GetUserNameBasedOnId(UGITUtility.ObjectToString(e.GetValue("CreatedBy")));
                    }
                }
            }

            string func = string.Empty;
            func = string.Format("openProjectTagDialog('{0}','{1}','{2}','{3}','{4}', 0)", editUrl, string.Format("ProjectTagId={0}", e.KeyValue), "Edit Experience Tag", "450px", "330px");
            e.Row.Attributes.Add("onClick", func);
        }
    }
}