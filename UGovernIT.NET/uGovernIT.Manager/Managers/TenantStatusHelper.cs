using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Cache;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.Common;

namespace uGovernIT.Manager
{
    public class TenantStatusHelper
    {
        public ApplicationContext context = null;
        //public DefaultConfigManager defaultConfigManager = null;
        public StatusSummary statusSummary = null;
        public UGITModule module = null;
        public ModuleViewManager moduleViewManager = null;
        public TenantStatusHelper(ApplicationContext context)
        {
            this.context = context;
            statusSummary = new StatusSummary();
            moduleViewManager = new ModuleViewManager(context);
        }


        public StatusSummary GetProfileStatusSummary(string tenantId)
        {
            DataTable clientProfileStatus = (DataTable)CacheHelper<object>.Get($"clientProfileStatus_{context.TenantID}", context.TenantID);
            if (clientProfileStatus == null)
            {
                clientProfileStatus = GetTableDataManager.GetTableDataUsingQuery($"ClientProfileStatus @TenantId='{tenantId}'");
                CacheHelper<object>.AddOrUpdate($"clientProfileStatus_{context.TenantID}", context.TenantID, clientProfileStatus);
            }
            if (clientProfileStatus != null && clientProfileStatus.Rows.Count != 0)
            {
                if (Convert.ToInt32(clientProfileStatus.Rows[0]["Percentage"]) > 0)
                {
                    statusSummary.IsRegistrationCom = true;
                }
                if (Convert.ToInt32(clientProfileStatus.Rows[1]["Percentage"]) > 0)
                {
                    statusSummary.IsticketCom = true;
                }
                if (Convert.ToInt32(clientProfileStatus.Rows[2]["Percentage"]) > 0)
                {
                    statusSummary.IsUserCom = true;
                }

                if (Convert.ToInt32(clientProfileStatus.Rows[3]["Percentage"]) > 0)
                {
                    statusSummary.IsUserTicketCom = true;
                }
                if (Convert.ToInt32(clientProfileStatus.Rows[4]["Percentage"]) > 0)
                {
                    statusSummary.IsSvcTicketCom = true;
                }

                if (Convert.ToInt32(clientProfileStatus.Rows[5]["Percentage"]) > 0)
                {
                    statusSummary.IsDepartmentCom = true;
                }
                if (Convert.ToInt32(clientProfileStatus.Rows[6]["Percentage"]) > 0)
                {
                    statusSummary.IsRoleCom = true;
                }
                if (Convert.ToInt32(clientProfileStatus.Rows[7]["Percentage"]) > 0)
                {
                    statusSummary.IsTitleCom = true;
                }
                if (Convert.ToInt32(clientProfileStatus.Rows[8]["Percentage"]) > 0)
                {
                    statusSummary.IsProjectCom = true;
                }
                if (Convert.ToInt32(clientProfileStatus.Rows[9]["Percentage"]) > 0)
                {
                    statusSummary.IsAllocationCom = true;
                }


                // statusSummary.RegistrationStatus = clientProfileStatus.Rows[0]["TotalCount"].ToString();
                if (clientProfileStatus.Rows[1]["TotalCount"] != null)
                {
                    statusSummary.ticketCount = (Convert.ToInt32(clientProfileStatus.Rows[1]["TotalCount"]));

                }
                if (clientProfileStatus.Rows[2]["TotalCount"] != null)
                {
                    statusSummary.Usercount = (Convert.ToInt32(clientProfileStatus.Rows[2]["TotalCount"]));


                }
                if (clientProfileStatus.Rows[3]["TotalCount"] != null)
                {
                    statusSummary.userTicket = (Convert.ToInt32(clientProfileStatus.Rows[3]["TotalCount"]));

                }
                if (clientProfileStatus.Rows[4]["TotalCount"] != null)
                {
                    statusSummary.svcTicketcount = (Convert.ToInt32(clientProfileStatus.Rows[4]["TotalCount"]));
                }
                if (clientProfileStatus.Rows[5]["TotalCount"] != null)
                {
                    statusSummary.departmentCount = (Convert.ToInt32(clientProfileStatus.Rows[5]["TotalCount"]));
                }
                if (clientProfileStatus.Rows[6]["TotalCount"] != null)
                {
                    statusSummary.roleCount = (Convert.ToInt32(clientProfileStatus.Rows[6]["TotalCount"]));
                }
                if (clientProfileStatus.Rows[7]["TotalCount"] != null)
                {
                    statusSummary.titleCount = (Convert.ToInt32(clientProfileStatus.Rows[7]["TotalCount"]));
                }
                if (clientProfileStatus.Rows[8]["TotalCount"] != null)
                {
                    statusSummary.projectCount = (Convert.ToInt32(clientProfileStatus.Rows[8]["TotalCount"]));
                }
                if (clientProfileStatus.Rows[9]["TotalCount"] != null)
                {
                    statusSummary.allocationCount = (Convert.ToInt32(clientProfileStatus.Rows[9]["TotalCount"]));
                }


            }
            return statusSummary;
        }

        public int GetProfileCompletePercent(StatusSummary statusSummary)
        {
            int count = 0;
            if (statusSummary.IsRegistrationCom)
            {
                count = count + 20;
            }
            if (statusSummary.IsticketCom)
            {
                count = count + 20;
            }
            if (statusSummary.IsUserCom)
            {
                count = count + 20;
            }
            if (statusSummary.IsUserTicketCom)
            {
                count = count + 20;
            }
            if (statusSummary.IsSvcTicketCom)
            {
                count = count + 20;
            }

            return count;
        }



    }
}
