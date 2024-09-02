using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class DepartmentRoleMapping : System.Web.UI.UserControl
    {
        protected string Roleid = "";
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        List<GlobalRole> spGlobalRoleList;
        //GlobalRole spitem;
        public string RoleID { get; set; }
        public long Id { get; set; }
        public bool _checkdeleted = false;
        GlobalRoleManager globalrolesManager = null;
        UserRoleManager UserRoleMgr = null;
        CompanyManager companyManager = null;
        DepartmentManager departmentManager = null;
        ConfigurationVariableManager configurationVariableMgr = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            BindRoles();
            TextBox _txtRole = (TextBox)FindControl("txtRole");
            Roleid = _txtRole.Text;
            TextBox _chkdeleted = (TextBox)FindControl("txtDeleted");
            _checkdeleted = UGITUtility.StringToBoolean(_chkdeleted.Text);
        }
        protected override void OnInit(EventArgs e)
        {
            UserRoleMgr = new UserRoleManager(context);
            globalrolesManager = new GlobalRoleManager(context);
            companyManager = new CompanyManager(context);
            departmentManager = new DepartmentManager(context);
            configurationVariableMgr = new ConfigurationVariableManager(context);
            RoleID = Request["roleid"] != null ? Request["roleid"] : null;
            Id = Request["Id"] != null ? Convert.ToInt64(Request["Id"]) : 0;
            spGlobalRoleList = globalrolesManager.Load(x => x.Deleted == false);
        }
        private void BindRoles()
        {
            if (Convert.ToString(Request["mode"]) == "E")
            {
                departmentCtr.Value = Convert.ToString(Request["deptid"]);
                departmentCtr.SetValues(departmentCtr.Value);
                if (string.IsNullOrEmpty(txtBLR.Text))
                {
                    txtBLR.Text = Convert.ToString(Request["blr"]);
                }
                chkDeleted.Checked = UGITUtility.StringToBoolean(Request["deleted"]);
                Id = Convert.ToInt64(Request["Id"]);

            }
        }
        private Boolean ValidateSkill()
        {
            long? DeptId = departmentCtr.GetValues() != string.Empty ? Convert.ToInt64(departmentCtr.GetValues()) : (long?)null;
            if (DeptId == 0)
                DeptId = Convert.ToInt64(departmentCtr.Value);
            string Role = Roleid == "00000000-0000-0000-0000-000000000000" ? null : RoleID;

            Dictionary<string, object> arrParams = new Dictionary<string, object>();
            arrParams.Add("@TenantID", context.TenantID);
            arrParams.Add("@deptid", DeptId);
            arrParams.Add("@roleid", Role);
            arrParams.Add("@BillingRate", txtBLR.Text);
            arrParams.Add("@Deleted", _checkdeleted);
            arrParams.Add("@Id", Id);
            DataSet dtResultBillings = DAL.uGITDAL.ExecuteDataSet_WithParameters("usp_GetRole", arrParams);
            if (Id > 0)
            {
                if (dtResultBillings != null && dtResultBillings.Tables[0].Rows.Count > 0)
                {

                    DataRow rowFound = dtResultBillings.Tables[0].Select($"ID <> {Id}").FirstOrDefault();
                    if (rowFound != null)
                    {

                        lblErrorMessage.Text = "Department, Role combination already exists.";
                        return false;
                    }
                }
            }
            else
            {
                if (dtResultBillings != null && dtResultBillings.Tables[1].Rows.Count > 0)
                {

                    DataRow rowFound = dtResultBillings.Tables[1].Select($"RoleLookup='{Role}' AND DepartmentLookup={DeptId} AND Deleted=False").FirstOrDefault();
                    if (rowFound != null)
                    {
                        lblErrorMessage.Text = "Department, Role combination already exists.";
                        return false;
                    }
                }
            }
            
            
            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool flag = true;
            if (string.IsNullOrEmpty(UGITUtility.ObjectToString(departmentCtr.GetValues())) && string.IsNullOrEmpty(UGITUtility.ObjectToString(departmentCtr.Value)))
            {
                lbldept.Visible = true;
                flag = false;
            }
            if (string.IsNullOrEmpty(txtBLR.Text))
            {
                lblblr.Visible = true;
                flag = false;
            }
            if (!flag)
                return;
            if (ValidateSkill())
            {
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("@id", Id);
                arrParams.Add("@BillingRate", UGITUtility.StringToDouble(txtBLR.Text));
                arrParams.Add("@RoleId", Convert.ToString(Request["RoleID"]));
                arrParams.Add("@DepartmentId", departmentCtr.GetValues() == null ? Convert.ToInt64(departmentCtr.Value) : Convert.ToInt64(departmentCtr.GetValues()));
                arrParams.Add("@Deleted", _checkdeleted);
                arrParams.Add("@TenantID", context.TenantID);
                if (DAL.uGITDAL.ExecuteNonQueryWithParameters("usp_insertDepartmentRoleMapping", arrParams) > 0)
                {
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
                else
                {
                    lblErrorMessage.Text = "Something went wrong !!";
                }
            }
            else
                lblErrorMessage.Text = "Department, Role combination already exists.";
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            //spitem = globalrolesManager.LoadByID(RoleID);
            //spitem.Deleted = true;
            //globalrolesManager.Update(spitem);
            //uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}