using System;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Web;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class EmployeeTypesView : UserControl
    {
        EmployeeTypeManager ObjEmployeeTypeManager = new EmployeeTypeManager(HttpContext.Current.GetManagerContext());
        protected string editUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=addemployeetypes");
        protected override void OnInit(EventArgs e)
        {
            List<EmployeeTypes> lstemptype = ObjEmployeeTypeManager.Load().Where(x=> x.Deleted == false).OrderBy(x => x.Title).ToList();
            if (lstemptype != null && lstemptype.Count > 0)
            {
                aspxGridEmployeeType.DataSource = lstemptype;
                aspxGridEmployeeType.DataBind();
            }
            LinkButton1.Attributes.Add("href", string.Format("javascript:NewEmpTypeDialog()"));
            lnkAddNewEmployeeType.Attributes.Add("href", string.Format("javascript:NewEmpTypeDialog()"));
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void aspxGridEmployeeType_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;
            string func = string.Format("openuEmpTypeDialog('{0}','{1}','{2}','{3}','{4}', 0)", editUrl, string.Format("ID={0}", e.KeyValue), "Edit Employee Type", "370px", "280px");
            e.Row.Attributes.Add("onClick", func);
        }
    }
}
