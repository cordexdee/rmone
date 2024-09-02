using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Linq;
using System.Data;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace uGovernIT.Web
{
    public partial class AddJobTitle : UserControl
    {
        public long JobTitleID { get; set; }
        List<JobTitle> spJobTitleList;
        JobTitle spitem;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        JobTitleManager JobTitleMGR = null;
        UserRoleManager UserRoleMgr = null;
        CompanyManager companyManager = null;
        DepartmentManager departmentManager = null;
        ConfigurationVariableManager configurationVariableMgr = null;
        UserProfileManager userProfileManager = null;

        protected override void OnInit(EventArgs e)
        {
            //departmentCtr.dropBox.Style.Add("min-width", "350px");
            //departmentCtr.dropBox.Style.Add("margin-bottom", "0px");

            JobTitleMGR = new JobTitleManager(context);
            UserRoleMgr = new UserRoleManager(context);
            companyManager = new CompanyManager(context);
            departmentManager = new DepartmentManager(context);
            configurationVariableMgr = new ConfigurationVariableManager(context);
            userProfileManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

            JobTitleID = Request["JobTitleId"] != null ? Convert.ToInt64(Request["JobTitleId"]) : 0;
            spJobTitleList = JobTitleMGR.Load(x => x.Deleted != false);
            BindJobType();
            if (!Page.IsPostBack)
            {
                //BindCompanies();
                BindRoles();
            }
            if (JobTitleID == 0)
            {
                spitem = new JobTitle();
                txtJobTitle.Text = "";
                txtLowProjectCapacity.Text = "";
                txtHighProjectCapacity.Text = "";
                txtLowRevenueCapacity.Text = "";
                txtHighRevenueCapacity.Text = "";
                //txtJobType.Text = "";
                //lnkDelete.Visible = false;
            }
            else
            {
                //lnkDelete.Visible = true;
                spitem = JobTitleMGR.LoadByID(JobTitleID);
                txtJobTitle.Text = spitem.Title;
                txtshortName.Text = spitem.ShortName;
                if (spitem.DepartmentId.HasValue)
                {
                    //long CompanyId = departmentManager.LoadByID(spitem.DepartmentId.Value).CompanyIdLookup.Value;

                    //ddlCompany.SelectedValue = Convert.ToString(CompanyId);

                    //BindDepartments();
                    //ddlDepartment.SelectedValue = Convert.ToString(spitem.DepartmentId.Value);
                    //departmentCtr.Value = Convert.ToString(spitem.DepartmentId);
                    //departmentCtr.SetValues(departmentCtr.Value);
                }

                ddlRole.SelectedValue = spitem.RoleId;
                txtLowProjectCapacity.Text = Convert.ToString(spitem.LowProjectCapacity);
                txtHighProjectCapacity.Text = Convert.ToString(spitem.HighProjectCapacity);
                txtLowRevenueCapacity.Text = Convert.ToString(spitem.LowRevenueCapacity);
                txtHighRevenueCapacity.Text = Convert.ToString(spitem.HighRevenueCapacity);
                //txtBLR.Text = UGITUtility.ObjectToString(spitem.BillingLaborRate);
                //txtECR.Text = UGITUtility.ObjectToString(spitem.EmployeeCostRate);
                txtRLT.Text = UGITUtility.ObjectToString(spitem.ResourceLevelTolerance);
                //txtJobType.Text = Convert.ToString(spitem.JobType); 
                if (spitem.JobType != null && cmbJobType.Items.FindByText(spitem.JobType) != null)
                    cmbJobType.SelectedIndex = cmbJobType.Items.FindByText(spitem.JobType).Index;
                chkDeleted.Checked = spitem.Deleted;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void BindJobType()
        {
            var items = configurationVariableMgr.GetValue(DatabaseObjects.Columns.JobType);

            if (!string.IsNullOrEmpty(items))
            {
                string[] priority = UGITUtility.SplitString(items, uGovernIT.Utility.Constants.Separator);
                cmbJobType.DataSource = priority;
            }
            else
            {
                cmbJobType.DataSource = null;
            }

            cmbJobType.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateSkill())
            {
                string existingRoleId = string.Empty;
                bool titleChanged = false;
                JobTitle jobTitle = JobTitleMGR.LoadByID(JobTitleID);
                if (jobTitle != null)
                    existingRoleId = jobTitle.RoleId;

                if (jobTitle != null && jobTitle.Title != txtJobTitle.Text)
                    titleChanged = true;
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("@Id", JobTitleID);
                arrParams.Add("@Title", txtJobTitle.Text);
                arrParams.Add("@Shortname", txtshortName.Text);
                arrParams.Add("@JobType", Convert.ToString(cmbJobType.SelectedItem)==null? "Overhead": Convert.ToString(cmbJobType.SelectedItem?.Value));
                arrParams.Add("@LowProjectCapacity", UGITUtility.StringToLong(txtLowProjectCapacity.Text));
                arrParams.Add("@HighProjectCapacity", UGITUtility.StringToLong(txtHighProjectCapacity.Text)); 
                arrParams.Add("@LowRevenueCapacity", UGITUtility.StringToLong(txtLowRevenueCapacity.Text));
                arrParams.Add("@HighRevenueCapacity", UGITUtility.StringToLong(txtHighRevenueCapacity.Text));
                //arrParams.Add("@EmpCostRate", Convert.ToInt32(txtECR.Text));
                arrParams.Add("@ResourceLevelTolerance", UGITUtility.StringToLong(txtRLT.Text));
                arrParams.Add("@RoleId", Convert.ToString(ddlRole.SelectedValue));
                //arrParams.Add("@DepartmentId", departmentCtr.GetValues()==null ?spitem.DepartmentId: Convert.ToInt64(departmentCtr.GetValues()));
                arrParams.Add("@Deleted", chkDeleted.Checked);
                arrParams.Add("@TenantID", context.TenantID);
                if (DAL.uGITDAL.ExecuteNonQueryWithParameters("usp_insertJobtitle", arrParams) > 0)
                {
                    if (titleChanged)
                        UpdateJobTitleInSummaryTables();
                    if (!existingRoleId.EqualsIgnoreCase(Convert.ToString(ddlRole.SelectedValue)))
                    {
                        UpdateUserProfileCache(JobTitleID, Convert.ToString(ddlRole.SelectedValue));
                    }

                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
                else
                {
                    lblErrorMessage.Text = "Something went wrong !!";
                }
            }
        }
        private void UpdateJobTitleInSummaryTables()
        {
            //Code commented out as the required DB changes are in DEV2

            //Dictionary<string, object> arrParams = new Dictionary<string, object>();
            //arrParams.Add("TenantId", context.CurrentUser.TenantID);
            //arrParams.Add("ID", JobTitleID);
            //arrParams.Add("IDType", "job");
            //DAL.uGITDAL.ExecuteNonQueryWithParameters("usp_UpdateTitleInSummaryTables", arrParams);
        }

        private void UpdateUserProfileCache(long jobTitleID, string RoleId)
        {
            List<UserProfile> users = userProfileManager.Load(x => x.JobTitleLookup == jobTitleID && x.Enabled == true);
            foreach (UserProfile usr in users)
            {
                userProfileManager.UpdateIntoCache(usr);
            }
        }

        //protected void btnSave_Click1(object sender, EventArgs e)
        //{
        //    if (ValidateSkill())
        //    {
        //        spitem.Title = txtJobTitle.Text;
        //        //spitem.JobType = txtJobType.Text;

        //        if (cmbJobType.SelectedItem != null)
        //            spitem.JobType = Convert.ToString(cmbJobType.SelectedItem.Value);

        //        int lowprojectcapacity = 0;
        //        int.TryParse(txtLowProjectCapacity.Text, out lowprojectcapacity);
        //        spitem.LowProjectCapacity = lowprojectcapacity;

        //        int highprojectcapacity = 0;
        //        int.TryParse(txtHighProjectCapacity.Text, out highprojectcapacity);
        //        spitem.HighProjectCapacity = highprojectcapacity;

        //        double lowrevenuecapacity = 0;
        //        double.TryParse(txtLowRevenueCapacity.Text, out lowrevenuecapacity);
        //        spitem.LowRevenueCapacity = lowrevenuecapacity;

        //        double blr = 0;
        //        // double.TryParse(txtBLR.Text, out blr);
        //        spitem.BillingLaborRate = blr;

        //        double ecr = 0;
        //        double.TryParse(txtECR.Text, out ecr);
        //        spitem.EmployeeCostRate = ecr;

        //        int resourceLevelTolerance = 0;
        //        int.TryParse(txtRLT.Text, out resourceLevelTolerance);
        //        spitem.ResourceLevelTolerance = resourceLevelTolerance;

        //        string oldRoleID = spitem.RoleId;
        //        if (ddlRole.SelectedValue != Guid.Empty.ToString())
        //        {
        //            spitem.RoleId = ddlRole.SelectedValue;
        //        }
        //        else
        //        {
        //            spitem.RoleId = null;
        //        }

        //        //if (ddlDepartment.SelectedValue != "")
        //        //{
        //        //    spitem.DepartmentId = Convert.ToInt64(ddlDepartment.SelectedItem.Value);
        //        //}

        //        if (!string.IsNullOrEmpty(departmentCtr.GetValues()))
        //        {
        //            spitem.DepartmentId = Convert.ToInt64(departmentCtr.GetValues());
        //        }
        //        else if (departmentCtr.GetValues() == string.Empty)
        //        {
        //            spitem.DepartmentId = null;
        //        }

        //        double highrevenuecapacity = 0;
        //        double.TryParse(txtHighRevenueCapacity.Text, out highrevenuecapacity);
        //        spitem.HighRevenueCapacity = highrevenuecapacity;
        //        spitem.Deleted = chkDeleted.Checked;

        //        if (spitem.ID <= 0)
        //            JobTitleMGR.Insert(spitem);
        //        else
        //            JobTitleMGR.Update(spitem);

        //        UpdateGlobleRoleID(spitem.ID, oldRoleID, spitem.RoleId);
        //        uHelper.ClosePopUpAndEndResponse(Context, true);
        //    }
        //}

        //protected void UpdateGlobleRoleID(long jobtitleID, string oldRoleID, string newRoleID)
        //{
        //    spitem.RoleId = oldRoleID;
        //    ddlRole.SelectedValue = newRoleID;
        //    UserProfileManager userProfileManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

        //    if (newRoleID != oldRoleID)
        //    {
        //        List<UserProfile> users = userProfileManager.Load(x => x.JobTitleLookup == jobtitleID);
        //        foreach (UserProfile usr in users)
        //        {
        //            usr.GlobalRoleId = newRoleID;
        //            userProfileManager.Update(usr);
        //        }

        //    }
        //}        //protected void btnSave_Click1(object sender, EventArgs e)
        //{
        //    if (ValidateSkill())
        //    {
        //        spitem.Title = txtJobTitle.Text;
        //        //spitem.JobType = txtJobType.Text;

        //        if (cmbJobType.SelectedItem != null)
        //            spitem.JobType = Convert.ToString(cmbJobType.SelectedItem.Value);

        //        int lowprojectcapacity = 0;
        //        int.TryParse(txtLowProjectCapacity.Text, out lowprojectcapacity);
        //        spitem.LowProjectCapacity = lowprojectcapacity;

        //        int highprojectcapacity = 0;
        //        int.TryParse(txtHighProjectCapacity.Text, out highprojectcapacity);
        //        spitem.HighProjectCapacity = highprojectcapacity;

        //        double lowrevenuecapacity = 0;
        //        double.TryParse(txtLowRevenueCapacity.Text, out lowrevenuecapacity);
        //        spitem.LowRevenueCapacity = lowrevenuecapacity;

        //        double blr = 0;
        //        // double.TryParse(txtBLR.Text, out blr);
        //        spitem.BillingLaborRate = blr;

        //        double ecr = 0;
        //        double.TryParse(txtECR.Text, out ecr);
        //        spitem.EmployeeCostRate = ecr;

        //        int resourceLevelTolerance = 0;
        //        int.TryParse(txtRLT.Text, out resourceLevelTolerance);
        //        spitem.ResourceLevelTolerance = resourceLevelTolerance;

        //        string oldRoleID = spitem.RoleId;
        //        if (ddlRole.SelectedValue != Guid.Empty.ToString())
        //        {
        //            spitem.RoleId = ddlRole.SelectedValue;
        //        }
        //        else
        //        {
        //            spitem.RoleId = null;
        //        }

        //        //if (ddlDepartment.SelectedValue != "")
        //        //{
        //        //    spitem.DepartmentId = Convert.ToInt64(ddlDepartment.SelectedItem.Value);
        //        //}

        //        if (!string.IsNullOrEmpty(departmentCtr.GetValues()))
        //        {
        //            spitem.DepartmentId = Convert.ToInt64(departmentCtr.GetValues());
        //        }
        //        else if (departmentCtr.GetValues() == string.Empty)
        //        {
        //            spitem.DepartmentId = null;
        //        }

        //        double highrevenuecapacity = 0;
        //        double.TryParse(txtHighRevenueCapacity.Text, out highrevenuecapacity);
        //        spitem.HighRevenueCapacity = highrevenuecapacity;
        //        spitem.Deleted = chkDeleted.Checked;

        //        if (spitem.ID <= 0)
        //            JobTitleMGR.Insert(spitem);
        //        else
        //            JobTitleMGR.Update(spitem);

        //        UpdateGlobleRoleID(spitem.ID, oldRoleID, spitem.RoleId);
        //        uHelper.ClosePopUpAndEndResponse(Context, true);
        //    }
        //}

        //protected void UpdateGlobleRoleID(long jobtitleID, string oldRoleID, string newRoleID)
        //{
        //    spitem.RoleId = oldRoleID;
        //    ddlRole.SelectedValue = newRoleID;
        //    UserProfileManager userProfileManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

        //    if (newRoleID != oldRoleID)
        //    {
        //        List<UserProfile> users = userProfileManager.Load(x => x.JobTitleLookup == jobtitleID);
        //        foreach (UserProfile usr in users)
        //        {
        //            usr.GlobalRoleId = newRoleID;
        //            userProfileManager.Update(usr);
        //        }

        //    }
        //}

        private Boolean ValidateSkill()
        {
            //JobTitle collection = spJobTitleList.FirstOrDefault(x => x.ID == JobTitleID && x.Title == txtJobTitle.Text && x.Deleted == chkDeleted.Checked);
            //long? DeptId = departmentCtr.GetValues() != string.Empty ? Convert.ToInt64(departmentCtr.GetValues()) : (long?)null;
            //if (DeptId == 0)
            //    DeptId = Convert.ToInt64(departmentCtr.Value);

            JobTitle collection = JobTitleMGR.Load(x => !string.IsNullOrWhiteSpace(x.Title) && x.Title.Trim().ToLower() == txtJobTitle.Text.ToLower() && x.Deleted == chkDeleted.Checked).FirstOrDefault();
            if (collection != null && JobTitleID != collection.ID)
            {
                lblErrorMessage.Text = "Job Title already exists.";
                return false;
            }
            else
            {
                return true;
            }
        }

        /*
        private void BindCompanies()
        {
            List<Company> companies = null;
            if (JobTitleID == 0)
            {
                companies = companyManager.Load(x => x.IsDeleted == false);
            }
            else
            {
                companies = companyManager.Load();
            }

            companies = companyManager.Load(x => x.IsDeleted == false);
            ddlCompany.DataTextField = DatabaseObjects.Columns.Title;
            ddlCompany.DataValueField = DatabaseObjects.Columns.ID;
            ddlCompany.DataSource = companies;
            ddlCompany.DataBind();

            ddlCompany.Items.Insert(0, new ListItem { Value = "0", Text = "--Select Company--" });
        }
        */

        private void BindRoles()
        {
            ddlRole.DataSource = uHelper.GetGlobalRoles(context, false).OrderBy(x => x.Name);
            ddlRole.DataTextField = DatabaseObjects.Columns.Name;
            ddlRole.DataValueField = DatabaseObjects.Columns.Id;
     
            ddlRole.DataBind();

            ddlRole.Items.Insert(0, new ListItem { Value = Guid.Empty.ToString(), Text = "--Select Role--" });
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
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        /*
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDepartments();            
        }

        private void BindDepartments()
        {
            List<Department> departments = null;
            if (JobTitleID == 0)
            {
                departments = departmentManager.Load(x => x.CompanyIdLookup == Convert.ToInt64(ddlCompany.SelectedItem.Value) && x.IsDeleted == false);
            }
            else
            {
                departments = departmentManager.Load(x => x.CompanyIdLookup == Convert.ToInt64(ddlCompany.SelectedItem.Value));
            }

            ddlDepartment.DataTextField = DatabaseObjects.Columns.Title;
            ddlDepartment.DataValueField = DatabaseObjects.Columns.ID;
            ddlDepartment.DataSource = departments;
            ddlDepartment.DataBind();            
        }        
        */
    }
}
