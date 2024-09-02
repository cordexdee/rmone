using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Linq;
using Microsoft.AspNet.Identity.Owin;

namespace uGovernIT.Web
{
    public partial class ProjectTeam : UserControl
    {
        public string ticketID { get; set; }

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ProjectEstimatedAllocationManager crmProjectAllocationManager = null;
        //FieldConfigurationManager fieldManager = null;
        UserProfileManager userManager = null;
        //UserRoleManager roleManager = null;
        GlobalRoleManager roleManager = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            crmProjectAllocationManager = new ProjectEstimatedAllocationManager(context);
            //fieldManager = new FieldConfigurationManager(context);
            roleManager = new GlobalRoleManager(context);
            userManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

            BindGrid();
        }

        void BindGrid()
        {
            //var Result = crmProjectAllocationManager.Load(x => x.TicketId == ticketID).Select(x => new { x.ID, x.AssignedTo, x.Type }).ToList();

            var Result = crmProjectAllocationManager.Load(x => x.TicketId == ticketID).ToList();
            var rolesList = roleManager.Load()?.Select(x => new { x.Id, x.Name }).ToList();

            var userIds = Result.Select(x => x.AssignedTo).ToList();
            var userNames = userManager.GetUserNames(userIds, context.TenantID);
            var kvpDefault = default(KeyValuePair<string, string>);
            if (rolesList != null)
            {
                Result.ForEach(x =>
                {
                //var assignedTo = Convert.ToString(userManager.GetDisplayNameFromUserId(x.AssignedTo));
                var assignedTo = userNames.FirstOrDefault(u => u.Key.EqualsTo(x.AssignedTo));
                    var type = Convert.ToString(rolesList.Where(y => y.Id == x.Type).Select(y => y.Name).FirstOrDefault());

                    x.AssignedTo = assignedTo.Equals(kvpDefault) ? x.AssignedTo : assignedTo.Value;
                    if (x.AssignedTo == Guid.Empty.ToString())
                        x.AssignedTo = string.Empty;
                    x.Type = !string.IsNullOrEmpty(type) ? type : x.Type;
                });
            }

            grdAllocation.DataSource = Result;
            grdAllocation.DataBind();
            grdAllocation.ExpandAll();
        }
    }
}
