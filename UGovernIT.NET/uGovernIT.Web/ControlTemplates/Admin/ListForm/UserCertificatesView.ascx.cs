using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class UserCertificatesView : UserControl
    {
        UserCertificateManager userCertificateManager = new UserCertificateManager(HttpContext.Current.GetManagerContext());
        protected string editUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=addusercertificates");
        protected override void OnInit(EventArgs e)
        {
            List<UserCertificates> lstUserCertificates = userCertificateManager.Load().OrderBy(x => x.Title).ToList();

            if (lstUserCertificates != null && lstUserCertificates.Count > 0)
            {
                aspxGridUserCertificates.DataSource = lstUserCertificates;
                aspxGridUserCertificates.DataBind();
            }
            LinkButton1.Attributes.Add("href", string.Format("javascript:NewUserCertificateDialog()"));
            lnkAddNewUserCertificate.Attributes.Add("href", string.Format("javascript:NewUserCertificateDialog()"));
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void aspxGridUserCertificates_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;
            string func = string.Empty;
            func = string.Format("openuserCertificateDialog('{0}','{1}','{2}','{3}','{4}', 0)", editUrl, string.Format("CertificateId={0}", e.KeyValue), "Edit User Certificates", "450px", "330px");
            e.Row.Attributes.Add("onClick", func);
        }
    }
}