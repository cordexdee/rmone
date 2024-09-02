using DevExpress.CodeParser;
using DevExpress.XtraEditors.Filtering.Templates;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.RMONE
{
    public partial class TimeOffCardView : System.Web.UI.UserControl
    {
        public DataTable TimeOffAllocationData { get; set; }
        public const string addMultiAllocationUrl = "/Layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&Type={2}&SelectedUsersList={3}&IncludeClosed={4}";

        public List<UserList> ListOfUsers
        {
            get
            {
                var data = TimeOffAllocationData != null ? (from timeoff in TimeOffAllocationData.AsEnumerable()
                                                        where timeoff.Field<string>(DatabaseObjects.Columns.WorkItemType) == "Time Off"
                                                        select new UserList
                                                        {
                                                            ResourceUser = timeoff.Field<string>("ResourceUser").Replace("'","`"),
                                                            ResourceId = timeoff.Field<string>("ResourceId"),
                                                            AddLink = GenerateAddLink(timeoff.Field<string>("ResourceId"), timeoff.Field<string>("ResourceUser").Replace("'", "`"))
                                                        }) : null;
                if (data != null)
                {
                    return data.DistinctBy(p => p.ResourceId).ToList();
                }

                return null;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        public string GenerateAddLink(string userId, string userName)
        {
            string AddNonProjectUrl = UGITUtility.GetAbsoluteURL(string.Format(addMultiAllocationUrl, "ptomultiallocationjs", "Add Non Project Allocations", "time off", userId, bool.FalseString));

            return $"window.parent.UgitOpenPopupDialog('{AddNonProjectUrl}', '', 'Add Non Project Allocation for {userName}', '680px', '90', 0, '');";

        }

        public List<string> GetWorkItems(string resourceId)
        {
            return TimeOffAllocationData != null ? (from timeoff in TimeOffAllocationData.AsEnumerable()
                                                    where timeoff.Field<string>("ResourceId") == resourceId
                                                    select
                                                    (
                                                    timeoff.Field<string>("WorkItemType")
                                                    )).Distinct().ToList() : null;
        }

        public List<ResourceItems> GetResourceItems(string resourceId, string workItemType)
        {
            return TimeOffAllocationData != null ? (from timeoff in TimeOffAllocationData.AsEnumerable()
                                                    where timeoff.Field<string>("ResourceId") == resourceId && timeoff.Field<string>("WorkItemType") == workItemType
                                                    select new ResourceItems
                                                    {
                                                        AllocationStartDate = timeoff.Field<DateTime>("AllocationStartDate"),
                                                        AllocationEndDate = timeoff.Field<DateTime>("AllocationEndDate"),
                                                        Color = GetColorValue(timeoff.Field<DateTime>("AllocationStartDate"), timeoff.Field<DateTime>("AllocationEndDate")),
                                                        WorkItem = timeoff.Field<string>(DatabaseObjects.Columns.WorkItem),
                                                        WorkItemID = timeoff.Field<long>(DatabaseObjects.Columns.Id).ToString(),
                                                    }).Distinct().ToList() : null;
        }

        public string GetColorValue(DateTime startDate, DateTime endDate)
        {
            DateTime currentDate = DateTime.Now;
            if (startDate < currentDate && endDate < currentDate)
            {
                return "darkgrey";
            }
            else if ((startDate >= currentDate && startDate <= currentDate.AddDays(7)) || (endDate >= currentDate && endDate <= currentDate.AddDays(7)))
            {
                return "green";
            }
            else
            {
                return "grey";
            }
        }

    }
    public class UserList
    {
        public string ResourceUser { get; set; }
        public string ResourceId { get; set; }
        public string AddLink { get; set; }
    }
    public class WorkItemList
    {
        public string WorkItemType { get; set; }
    }
    public class ResourceItems
    {
        public string WorkItemID { get; set; }
        public DateTime AllocationStartDate { get; set; }
        public DateTime AllocationEndDate { get; set; }
        public string WorkItem { get; set; }
        public string Color { get; set; }


    }
}