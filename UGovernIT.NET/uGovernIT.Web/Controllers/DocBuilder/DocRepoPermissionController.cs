using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using uGovernIT.DMS.Amazon;
using uGovernIT.Utility.Entities.DMSDB;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity.Owin;

namespace uGovernIT.Web
{
    public class DocRepoPermissionController : Controller
    {

        ApplicationContext context = null;
        string userId = string.Empty;
        DMSDocRepPermissionManagerService permissionService = null;
        UserProfileManager manager;
        UserProfile user = null;
        public DocRepoPermissionController()
        {

            context = System.Web.HttpContext.Current.GetManagerContext();
            userId = context.CurrentUser.Id;
            permissionService = new DMSDocRepPermissionManagerService(context);
            user = System.Web.HttpContext.Current.CurrentUser();
        }
       

        [HttpGet]
        public ActionResult UserAdminRepository()
        {
            manager = System.Web.HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            var isAdmin = manager.IsRole(RoleType.Admin, user.UserName);
            ViewBag.activeTab = "permission";

            var moduleViewManager = new ModuleUserTypeManager(context);
            var tenant = moduleViewManager.GetTenantById(user.TenantID);
            ViewBag.isOffice365 = tenant.IsOffice365Subscription;

            StatusModel status = new StatusModel();
            Session["logInUserId"] = userId;
            Session["RoleName"] = "Admin";
            ViewBag.role = isAdmin;
            try
            {
                if (Session["logInUserId"] != null)
                {
                    var filesDirectoryDetails = permissionService.CompleteFilesAndDirectoriesList(userId);
                    return View(filesDirectoryDetails);
                }
                else
                {
                    status.IsSuccess = false;
                    status.ErrorCode = -999;
                }
            }
            catch (Exception ex)
            {
                status.IsSuccess = false;
                status.Message = ex.ToString();
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UserAdminRepository(string Prefix)
        {
            StatusModel status = new StatusModel();

            try
            {
                if (Session["logInUserId"] != null)
                {
                    List<UserProfile> users = permissionService.SearchUser(Prefix);
                    var UserList = (from u in users
                                    select new { u.UserName, u.Id }).ToList();

                    return Json(UserList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    status.IsSuccess = false;
                    status.ErrorCode = -999;
                }
            }
            catch (Exception ex)
            {
                status.IsSuccess = false;
                status.Message = ex.ToString();
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LoadUserAccessedData(string userId)
        {
            StatusModel status = new StatusModel();

            try
            {
                if (Session["logInUserId"] != null)
                {
                    var completeFilesAndDirectoriesList = permissionService.CompleteFilesAndDirectoriesList(userId);
                    return PartialView("UserAccessedData", completeFilesAndDirectoriesList);
                }
                else
                {
                    status.IsSuccess = false;
                    status.ErrorCode = -999;
                }
            }
            catch (Exception ex)
            {
                status.IsSuccess = false;
                status.Message = ex.ToString();
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateUserAccessToFilesAndDirectories(string userId, string ReadAccess, string WriteAccess)
        {
            StatusModel status = new StatusModel();

            try
            {
                if (Session["logInUserId"] != null)
                {
                    //Guid userID = new Guid();
                    //var isGuid = Guid.TryParse(userId, out userID);

                    if (!string.IsNullOrEmpty(userId))
                    {
                        var existedUserAccessDetailsList = permissionService.GetExistingAccessDetails(userId);

                        List<DMSUsersFilesAuthorization> userReadAccessDetailsToUpdate = new List<DMSUsersFilesAuthorization>();
                        List<DMSUsersFilesAuthorization> userWriteAccessDetailsToUpdate = new List<DMSUsersFilesAuthorization>();

                        //MembershipUser membershipUser = Membership.GetUser();
                        string userIdLogin = context.CurrentUser.Id;
                        DateTime localDate = System.DateTime.Now;

                        List<FilesDirectories> readDetailsList = new JavaScriptSerializer().Deserialize<List<FilesDirectories>>(ReadAccess);
                        List<FilesDirectories> writeDetailsList = new JavaScriptSerializer().Deserialize<List<FilesDirectories>>(WriteAccess);

                        var existingUserAccessDetailsToDelete = permissionService.UpdateReadAccessDetails(userIdLogin, userId, readDetailsList, userReadAccessDetailsToUpdate);

                        //Set Write AccessDetails
                        permissionService.UpdateWriteAccessDetails(userIdLogin, userId, writeDetailsList, userWriteAccessDetailsToUpdate, existingUserAccessDetailsToDelete);
                        permissionService.PerformUserAccessDetailUpdates(userReadAccessDetailsToUpdate, existingUserAccessDetailsToDelete, userWriteAccessDetailsToUpdate);

                        var result = permissionService.UpdateCompleteAccessToHierarchy(userId, existedUserAccessDetailsList, true);
                        result = permissionService.UpdateCompleteAccessToHierarchy(userId, null, false);
                        status.IsSuccess = true;
                        status.Message = "Permission set successfully.";
                    }
                }
                else
                {
                    status.IsSuccess = false;
                    status.ErrorCode = -999;
                }
            }
            catch (Exception ex)
            {
                status.IsSuccess = false;
                status.Message = ex.ToString();
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void  RedirectToHome()
        {
            //Response.Redirect("~/Default.aspx");
            Response.Redirect("~/pages/Home");


        }

    }
}