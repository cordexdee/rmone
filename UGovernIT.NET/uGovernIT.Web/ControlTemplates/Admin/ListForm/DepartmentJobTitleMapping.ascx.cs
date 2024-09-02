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
    public partial class DepartmentJobTitleMapping : System.Web.UI.UserControl
    {
        protected string Roleid = "";
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        List<JobTitle> spJobTitleList;
        JobTitle spitem;
        public long JobTitleID { get; set; }
        public long Id { get; set; }
        public bool _checkdeleted = false;
        JobTitleManager JobTitleMGR = null;
        //UserRoleManager UserRoleMgr = null;
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
            JobTitleMGR = new JobTitleManager(context);
            //UserRoleMgr = new UserRoleManager(context);
            companyManager = new CompanyManager(context);
            departmentManager = new DepartmentManager(context);
            configurationVariableMgr = new ConfigurationVariableManager(context);
            JobTitleID = Request["JobTitleId"] != null ? Convert.ToInt64(Request["JobTitleId"]) : 0;
            Id = Request["Id"] != null ? Convert.ToInt64(Request["Id"]) : 0;
            spJobTitleList = JobTitleMGR.Load(x => x.Deleted != false);
        }
        private void BindRoles()
        {
            //DataTable Roles = GetTableDataManager.GetTableData("Roles", $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            //Roles.DefaultView.Sort = "Name ASC";
            //Roles = Roles.DefaultView.ToTable();

            //ddlRole.DataTextField = DatabaseObjects.Columns.Name;
            //ddlRole.DataValueField = DatabaseObjects.Columns.Id;
            //ddlRole.DataSource = Roles;
            //ddlRole.DataBind();

            //ddlRole.Items.Insert(0, new ListItem { Value = Guid.Empty.ToString(), Text = "--Select Role--" });
            if (Convert.ToString(Request["mode"]) == "E")
            {
                departmentCtr.Value = Convert.ToString(Request["deptid"]);
                departmentCtr.SetValues(departmentCtr.Value);
                ddlRole.SelectedValue = Convert.ToString(Request["roleid"]);
                if (string.IsNullOrEmpty(txtECR.Text))
                {
                    txtECR.Text = Convert.ToString(Request["ecr"]);
                }
                chkDeleted.Checked = UGITUtility.StringToBoolean(Request["deleted"]);

            }
        }
        private Boolean ValidateSkill()
        {
            long DeptId = Convert.ToInt64(departmentCtr.GetValues());
            if (DeptId == 0)
                DeptId = Convert.ToInt64(departmentCtr.Value);
            //string Role = Roleid == "00000000-0000-0000-0000-000000000000" ? null : Roleid;
            //if (Request["mode"] == "E")
            //{
            //    Role = Roleid == "" ? Convert.ToString(ddlRole.SelectedValue) : Roleid;
            //}

            Dictionary<string, object> arrParams = new Dictionary<string, object>();
            arrParams.Add("@TenantID", context.TenantID);
            arrParams.Add("@deptid", DeptId);
            //arrParams.Add("@roleid", Role);
            arrParams.Add("@JobTitleId", JobTitleID);
            arrParams.Add("@EmpCostRate", txtECR.Text);
            arrParams.Add("@Deleted", _checkdeleted);
            DataSet dtResultBillings = DAL.uGITDAL.ExecuteDataSet_WithParameters("usp_GetJobTitle", arrParams);
            if (Id > 0)
            {
                if (dtResultBillings != null && dtResultBillings.Tables[0].Rows.Count > 0)
                {
                    DataRow rowFound = dtResultBillings.Tables[0].Select($"ID <> {Id}").FirstOrDefault();
                    if (rowFound != null)
                    {

                        lblErrorMessage.Text = "Job Title, Department combination already exists.";
                        return false;
                    }
                }
            }
            else
            {
                if (dtResultBillings != null && dtResultBillings.Tables[0].Rows.Count > 0)
                {
                    lblErrorMessage.Text = "Job Title, Department combination already exists.";
                    return false;
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
            //if ((Roleid== "00000000-0000-0000-0000-000000000000" || Roleid=="" || Roleid== string.Empty) && ddlRole.SelectedValue == "00000000-0000-0000-0000-000000000000")
            //{
            //    lblrole.Visible = true;
            //    flag = false;
            //}
            if (string.IsNullOrEmpty(txtECR.Text))
            {
                lblecr.Visible = true;
                flag = false;
            }
            if (!flag)
                return;
            if (ValidateSkill())
            {
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("@id", Id);
                arrParams.Add("@JobTitleId", JobTitleID);
                arrParams.Add("@EmpCostRate", UGITUtility.StringToDouble(txtECR.Text));
                //arrParams.Add("@RoleId", Convert.ToString(Roleid) == "" ? Convert.ToString(ddlRole.SelectedValue) : Roleid);
                arrParams.Add("@DepartmentId", departmentCtr.GetValues() == null ? Convert.ToInt64(departmentCtr.Value) : Convert.ToInt64(departmentCtr.GetValues()));
                arrParams.Add("@Deleted", _checkdeleted);
                arrParams.Add("@TenantID", context.TenantID);
                if (DAL.uGITDAL.ExecuteNonQueryWithParameters("usp_insertDepartmentJobtitleMapping", arrParams) > 0)
                {
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
                else
                {
                    lblErrorMessage.Text = "Something went wrong !!";
                }
            }
            else
                lblErrorMessage.Text = "Job Title, Department combination already exists.";
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            spitem = JobTitleMGR.LoadByID(JobTitleID);
            spitem.Deleted = true;
            JobTitleMGR.Update(spitem);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}