using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Web
{
    public class WikiArticleHelper
    {
        public static bool IsWikiOwner(UserProfile user)
        {
            bool isWikiOwner = false;
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(context);
            ConfigurationVariable wikiOwners = configurationVariableManager.GetValue(ConfigConstants.WikiOwners, false);
            UserProfileManager userManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

            if (wikiOwners != null)
            {
                if (wikiOwners.Type == "Text" && wikiOwners.KeyValue != null)
                {
                    isWikiOwner = CheckUserIsInGroup(context, wikiOwners.KeyValue, context.CurrentUser);
                }
                if (wikiOwners.Type == "User" && wikiOwners.KeyValue != null)
                {
                    // KeyValue is userId
                    if (wikiOwners.KeyValue == context.CurrentUser.Id)
                    {
                        isWikiOwner = true;

                    }
                    //KeyValue is usergroup
                    else
                    {
                        isWikiOwner = userManager.CheckUserInGroup(context.CurrentUser.Id, wikiOwners.KeyValue);

                    }

                }
            }

            return isWikiOwner;
        }


        public static bool CheckUserIsInGroup(ApplicationContext context, string groupNames, UserProfile user)
        {
            UserProfileManager userProfileManager = new UserProfileManager(context);

            if (string.IsNullOrEmpty(groupNames) || user == null)
                return false;

            string[] groups = UGITUtility.SplitString(groupNames, Constants.Separator6);
            if (groups != null && groups.Count() > 0)
                return userProfileManager.IsUserinGroups(groupNames, user.UserName);

            return false;
        }
        public static bool GetPermissions(ApplicationContext context, string action, string WikiId)
        {
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(context);
            // If current user is super admin, just return true
            if (context.UserManager.IsUGITSuperAdmin(context.CurrentUser))
                return true;

            if (!string.IsNullOrEmpty(action))
            {
                if (IsWikiOwner(context.CurrentUser))
                    return true;

                if (action == "add")
                {
                    string wikiCreators = configurationVariableManager.GetValue(ConfigConstants.WikiCreators);
                    bool isCreator = context.UserManager.CheckUserIsInGroup(wikiCreators, context.CurrentUser);
                    if (isCreator == true)
                        return true;
                }
                else // action == "edit-delete"
                {
                    WikiArticlesManager wikiArticlesManager = new WikiArticlesManager(context);
                    WikiArticles spColl = wikiArticlesManager.Load(x=>x.TicketId== WikiId).FirstOrDefault();
                    string WikiUserId = string.Empty;
                    if (spColl != null)
                        WikiUserId = spColl.CreatedBy;

                    if (WikiUserId == context.CurrentUser.Id)
                        return true;
                }
            }

            // Didn't find right permissions
            return true;
        }
    }
}