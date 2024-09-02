using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.Models;

namespace uGovernIT.Web.Helpers
{
    public class RequestTypeHelper
    {
       public static RequesetTypeDependentModel GetRequestType(ApplicationContext context, UGITModule module,  long requestTypeID, long locationID, string requestor)
        {
            ModuleViewManager moduleMgr = new ModuleViewManager(context);
            ModuleRequestType dr = module.List_RequestTypes.FirstOrDefault(x => x.ID == requestTypeID);
            ModuleRequestTypeLocation drLC = null;
            if (locationID > 0)
                drLC = module.List_RequestTypeByLocation.FirstOrDefault(x => x.RequestTypeLookup == requestTypeID && x.LocationLookup == locationID);
            else
            {
                UserProfile user = context.UserManager.GetUserById(requestor);
                if (user != null && user.LocationId > 0)
                {
                    drLC = module.List_RequestTypeByLocation.FirstOrDefault(x => x.RequestTypeLookup == requestTypeID && x.LocationLookup == user.LocationId);

                }
            }

            RequesetTypeDependentModel rtDep = new RequesetTypeDependentModel();
            if (dr != null)
            {
                rtDep.ID = dr.ID;
                rtDep.RequestType = dr.RequestType;
                rtDep.Category = dr.Category;
                rtDep.SubCategory = dr.SubCategory;
                rtDep.FunctionalAreaLookup = dr.FunctionalAreaLookup;
                if (rtDep.FunctionalArea != null)
                    rtDep.FunctionalArea = dr.FunctionalArea.Title;
                if(dr.EstimatedHours.HasValue)
                rtDep.EstimatedHours = dr.EstimatedHours.Value;
                rtDep.Workflowtype = dr.WorkflowType;
                if (drLC != null)
                {
                    rtDep.OwnerID = drLC.Owner;
                    if (!string.IsNullOrWhiteSpace(drLC.Owner))
                        rtDep.Owner = context.UserManager.ConcatenateValues(context.UserManager.uesrListByMultipleID(drLC.Owner, ";"), ";");
                    rtDep.PRPGroupID = drLC.PRPGroup;
                    rtDep.PRPGroup = context.UserManager.GetValue(context.UserManager.GetUserById(drLC.PRPGroup));
                    rtDep.ORPID = drLC.ORP;
                    rtDep.ORP = context.UserManager.ConcatenateValues(context.UserManager.uesrListByMultipleID(drLC.ORP, ";"), ";");
                    rtDep.LocationID = drLC.LocationLookup;
                }
                else
                {
                    rtDep.OwnerID = dr.Owner;
                    if (!string.IsNullOrWhiteSpace(dr.Owner))
                        rtDep.Owner = context.UserManager.ConcatenateValues(context.UserManager.uesrListByMultipleID(dr.Owner, ";"), ";");
                    if (string.IsNullOrEmpty(rtDep.Owner))
                    {
                        List<string> userlist = new List<string>();

                        userlist = UGITUtility.ConvertStringToList(dr.Owner, Constants.Separator);
                        rtDep.Owner = context.UserManager.CommaSeparatedGroupNamesFrom(userlist, Constants.UserInfoSeparator);
                    }

                    rtDep.PRPGroupID = dr.PRPGroup;
                    rtDep.PRPGroup = context.UserManager.GetValue(context.UserManager.GetUserById(dr.PRPGroup));
                    rtDep.ORPID = dr.ORP;
                    rtDep.ORP = context.UserManager.ConcatenateValues(context.UserManager.uesrListByMultipleID(dr.ORP, ";"), ";");
                }
            }
            return rtDep;
        }
    }
}