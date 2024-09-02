using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DAL;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.Common;

namespace uGovernIT.Web
{
	public partial class Status : System.Web.UI.UserControl
	{
        public string tenantId = null;
        protected ApplicationContext context = null;
        public StatusSummary statusSummary = null;
        ConfigurationVariableManager ObjConfigurationVariableManager = null;
        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (context == null)
                {
                    context = HttpContext.Current.GetManagerContext();
                }
                return context;
            }

        }


        protected void Page_Load(object sender, EventArgs e)
		{
            ObjConfigurationVariableManager = new ConfigurationVariableManager(ApplicationContext);
            statusSummary = new StatusSummary();
            TenantStatusHelper tenantStatusHelper = new TenantStatusHelper(ApplicationContext);
            statusSummary = tenantStatusHelper.GetProfileStatusSummary(tenantId);
            tenantStatusHelper.GetProfileCompletePercent(statusSummary);
            setCount(statusSummary);
            setProgressStatus(statusSummary);
            showHideControl();

        }
        public void showHideControl()
        {
            var clientType = ObjConfigurationVariableManager.GetValue("ClientType");
            if (clientType.ToLower() == "gc")
            {
                
                lstDepartment.Visible = true;
                lstTitle.Visible = true;
                lstRole.Visible = true;
                lstProject.Visible = true;
                lstAllocation.Visible = true;
                lstRegistration.Visible = true;
                lstAddUser.Visible = true;
                lstUserCreated.Visible = false;
                lstUserService.Visible = false;
                lstManageIncidents.Visible = false;
            }
            else
            {
                lstDepartment.Visible = false;
                lstTitle.Visible = false;
                lstRole.Visible = false;
                lstProject.Visible = false;
                lstAllocation.Visible = false;
                lstUserService.Visible = true;
                lstManageIncidents.Visible = true;
            }
        }
        public void setCount(StatusSummary  statusSummary)
        {
            dvmanageIncident.InnerHtml = statusSummary.ticketCount.ToString();
            dvAddedUser.InnerHtml = statusSummary.Usercount.ToString();
            dvusercreatedincident.InnerHtml = statusSummary.userTicket.ToString();
            dvUsedSvc.InnerHtml = statusSummary.svcTicketcount.ToString();

            dvDepartment.InnerHtml= statusSummary.departmentCount.ToString();
            dvRole.InnerHtml = statusSummary.roleCount.ToString();
            dvTitle.InnerHtml = statusSummary.titleCount.ToString();
            dvProject.InnerHtml = statusSummary.projectCount.ToString();
            dvAllocation.InnerHtml = statusSummary.allocationCount.ToString();

        }

        public void setProgressStatus(StatusSummary statusSummary)
        {

            if (statusSummary.IsRegistrationCom)
            {
                registration.Attributes.Add("class", "status-complete statusDot");
            }
            else
            {

            }

            if (statusSummary.IsticketCom)
            {
                manageIncident.Attributes.Add("class", "status-complete statusDot status-dot1");
            }
            else
            {

            }
            if (statusSummary.IsUserCom)
            {
                AddedUser.Attributes.Add("class", "status-complete statusDot ml166");
            }
            else
            {

            }
            if (statusSummary.IsUserTicketCom)
            {
                usercreatedincident.Attributes.Add("class", "status-complete statusDot status-dot3");
            }
            else
            {

            }
            if (statusSummary.IsSvcTicketCom)
            {
                UsedSvc.Attributes.Add("class", "status-complete status-lastDot");
            }
            else
            {

            }
            if (statusSummary.IsDepartmentCom)
            {
                Department.Attributes.Add("class", "status-complete statusDot ml166");
            }
            else
            {

            }
            if (statusSummary.IsRoleCom)
            {
                Role.Attributes.Add("class", "status-complete statusDot ml166");
            }
            else
            {

            }
            if (statusSummary.IsTitleCom)
            {
                Title.Attributes.Add("class", "status-complete statusDot ml166");
            }
            else
            {

            }
            if (statusSummary.IsProjectCom)
            {
                Project.Attributes.Add("class", "status-complete statusDot ml166");
            }
            else
            {

            }
            if (statusSummary.IsAllocationCom)
            {
                Allocation.Attributes.Add("class", "status-complete statusDot ml166");
            }
            else
            {

            }

        }

        
	}
}