using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Manager;

namespace uGovernIT.Web
{
    public partial class AddEmployeeTypes : UserControl
    {
        public long EmpTypeID { get; set; }
        EmployeeTypes spitem;
        EmployeeTypeManager objEmployeeTypeManager = new EmployeeTypeManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            if (Convert.ToInt64(Request.QueryString["ID"]) > 0 && Request.QueryString["ID"] != null)
            {
                EmpTypeID = Convert.ToInt64(Request.QueryString["ID"]);
                spitem = objEmployeeTypeManager.LoadByID(EmpTypeID);
                if (spitem != null)
                {
                    txtEmployeeType.Text = spitem.Title;
                    txtDescription.Text = spitem.Description;
                    lnkDelete.Visible = true;
                }
            }
            else
            {
                spitem = new EmployeeTypes();
                txtEmployeeType.Text = "";
                txtDescription.Text = "";
                lnkDelete.Visible = false;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            if (Validate())
            {
                spitem.Title = txtEmployeeType.Text;
                spitem.Description = txtDescription.Text;
                objEmployeeTypeManager.Save(spitem);
                Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added/Updated Employee types: {spitem.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }
        private Boolean Validate()
        {
            List<EmployeeTypes> collection = objEmployeeTypeManager.Load(x => x.ID != EmpTypeID && x.Title == txtEmployeeType.Text);
            if (collection.Count > 0)
            {
                lblErrorMessage.Text = "Employee Type is already in the list";
                return false;
            }
            else
            { return true; }
        }
        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            spitem = objEmployeeTypeManager.LoadByID(EmpTypeID);
            spitem.Deleted = true;
            objEmployeeTypeManager.Update(spitem);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Deleted Employee types: {spitem.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
