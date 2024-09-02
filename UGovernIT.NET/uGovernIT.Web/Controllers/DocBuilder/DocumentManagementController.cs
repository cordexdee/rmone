using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uGovernIT.DMS.Amazon;
using uGovernIT.Utility.Entities.DMSDB;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Utility;
using System.Data;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public class DocumentManagementController : Controller
    {
        private ApplicationContext _applicationContext = null;
        private UserProfile _user = null;
        private UserProfileManager _userProfileManager;
        private string _userId = string.Empty;

        public string FolderName { get; set; }

        public bool IsTabSelected { get; set; }

        public DocumentManagementController()
        {
            _applicationContext = System.Web.HttpContext.Current.GetManagerContext();
            _user = System.Web.HttpContext.Current.CurrentUser();
            _userId = _applicationContext.CurrentUser.Id;
        }

        #region "Actions"

        // GET: Repository
        public ActionResult Index()
        {
            //manager = System.Web.HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            //var isAdmin = manager.IsRole(RoleType.Admin, user.UserName);

            //ViewBag.role = isAdmin;
            //var moduleViewManager = new ModuleUserTypeManager(context);
            //var tenant = moduleViewManager.GetTenantById(user.TenantID);
            //ViewBag.isOffice365 = tenant.IsOffice365Subscription;

            //return RedirectToAction("Index", "O365Files");

            //if (tenant != null && !tenant.IsOffice365Subscription)
            //{
            //    return RedirectToAction("Repository");
            //}

            var ticketId = Request.QueryString["ticketid"];
            Session["TicketId"] = ticketId;

            var repositoryService = new DMSManagerService(_applicationContext);

            if (!string.IsNullOrEmpty(ticketId))
            {
                ticketId = ticketId.Replace("-", "_");
            }

            var directory = repositoryService.GetUserRepoDirectory(_userId, ticketId);

            if (directory != null)
            {
                return RedirectToAction("Repository");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(string chkDefaultFolder, string ticketID = "", string typeOfFolder = "", bool isUpload = false, bool isTabSelected = false)
        {
            try
            {

                var ticketId = string.Empty;

                var ticketIdOrginal = string.Empty;

                if (!string.IsNullOrEmpty(typeOfFolder))
                {
                    Session["Folder"] = typeOfFolder;
                    Session["isUpload"] = isUpload;
                    Session["isActiveTab"] = isTabSelected;
                }
                var repositoryService = new DMSManagerService(_applicationContext);

                if (!string.IsNullOrEmpty(ticketID))
                {
                    ticketId = ticketID;
                    ticketIdOrginal = ticketId;

                }
                else
                {

                    ticketId = Convert.ToString(Session["TicketId"]);

                    //ticketIdOrginal = Convert.ToString(Session["TicketId"]);
                    ticketIdOrginal = ticketId;
                }

                if (!string.IsNullOrEmpty(ticketId))
                {
                    ticketId = ticketId.Replace("-", "_");
                }
                string ownerUser = GetOwnerUser(ticketIdOrginal, _userId);
                var directory = repositoryService.GetUserRepoDirectory(_userId, ticketId);

                if (directory == null || typeOfFolder == string.Empty)
                {
                    repositoryService.CreatePortalForTicketElevated(_applicationContext, ticketIdOrginal,_userId, ownerUser, chkDefaultFolder);
                    //repositoryService.CreateNewFolder(ticketId, 0, _userId, ownerUser);

                    //directory = repositoryService.GetUserRepoDirectory(_userId, ticketId);

                    //if (!string.IsNullOrEmpty(chkDefaultFolder) && "true" == chkDefaultFolder.ToLower())
                    //{
                    //    ConfigurationVariableManager objConfigVariable = new ConfigurationVariableManager(_applicationContext);
                    //    string DefaultFolders = objConfigVariable.GetValue("DefaultFolders");
                    //    LifeCycle lifeCycle;
                    //    LifeCycleManager lcHelper = new LifeCycleManager(_applicationContext);
                    //    string ModuleName = uHelper.getModuleNameByTicketId(ticketIdOrginal);
                    //    DataRow currentTicket = Ticket.GetCurrentTicket(_applicationContext, ModuleName, ticketIdOrginal);
                    //    if ((ModuleName).Equals("PMM"))
                    //    {
                    //        lifeCycle = lcHelper.LoadProjectLifeCycles().FirstOrDefault(x => x.ID == Convert.ToInt32(currentTicket[DatabaseObjects.Columns.ProjectLifeCycleLookup])); // TicketRequest.Module.List_LifeCycles.FirstOrDefault(x => x.ID ==  Convert.ToInt32(currentTicket[DatabaseObjects.Columns.ProjectLifeCycleLookup]));
                    //        if (lifeCycle != null)
                    //        {
                    //            List<LifeCycleStage> stages = lifeCycle.Stages;
                    //            List<string> folders = new List<string>();
                    //            foreach (LifeCycleStage stage in stages)
                    //            {
                    //                string folderName = string.Format("{0:D2}-{1}", stage.StageStep, stage.Name);
                    //                folderName = UGITUtility.Truncate(UGITUtility.ReplaceInvalidCharsInFolderName(folderName), 50);
                    //                folders.Add(folderName);
                    //                repositoryService.CreateNewFolder(folderName, directory.DirectoryId, _userId, ownerUser);
                    //            }

                    //            repositoryService.UpdateHistory(_applicationContext.CurrentUser, ticketIdOrginal, $"{string.Join($"{Constants.Separator6} ", folders)} Folders added by {_applicationContext.CurrentUser.Name}");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (!string.IsNullOrEmpty(DefaultFolders))
                    //        {
                    //            foreach (var item in DefaultFolders.Split(new string[] { uGovernIT.Utility.Constants.Separator }, StringSplitOptions.RemoveEmptyEntries))
                    //                repositoryService.CreateNewFolder(item, directory.DirectoryId, _userId, ownerUser);

                    //            repositoryService.UpdateHistory(_applicationContext.CurrentUser, ticketIdOrginal, $"{DefaultFolders.Replace(Constants.Separator, $", ")} Folders added by {_applicationContext.CurrentUser.Name}");
                    //        }
                    //    }


                    //}
                }
                else if (!string.IsNullOrEmpty(typeOfFolder) && typeOfFolder == Convert.ToString(Session["Folder"]))//check 
                {
                    string folderName = string.Format("{0}", typeOfFolder);
                    if (directory == null)
                        repositoryService.CreateNewFolder(ticketId, 0, _userId, ownerUser);

                    var directory1 = repositoryService.GetUserRepoDirectory(_userId, ticketId);
                    folderName = UGITUtility.Truncate(UGITUtility.ReplaceInvalidCharsInFolderName(folderName), 50);
                    if (directory1 != null)
                        repositoryService.CreateNewFolder(folderName, directory1.DirectoryId, _userId, ownerUser);
                }
                
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, MessageCategory.DocumentManagement);
            }

            return RedirectToAction("Repository");
        }

        public ActionResult Repository()
        {
            _userProfileManager = System.Web.HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            var isAdmin = _userProfileManager.IsRole(RoleType.Admin, _user.UserName);
            //var ticketId = Request.QueryString["ticketid"];

            ViewBag.activeTab = "Repository";
            ViewBag.role = isAdmin;

            var moduleViewManager = new ModuleUserTypeManager(_applicationContext);
            var tenant = moduleViewManager.GetTenantById(_user.TenantID);
            var isOffice365 = tenant.IsOffice365Subscription;

            ViewBag.isOffice365 = isOffice365;

            Session["logInUserId"] = _userId;
            Session["RoleName"] = "User";
            //Session["TicketId"] = ticketId;

            if (isAdmin)
            {
                Session["RoleName"] = "Admin";
            }
            //if (isOffice365)
            //{
            //    return RedirectToAction("Index", "O365Files");
            //}

            return View();
        }

        [HttpGet]
        public ActionResult BuildTree()
        {
            _applicationContext = System.Web.HttpContext.Current.GetManagerContext();
            var repositoryService = new DMSManagerService(_applicationContext);
            _userId = _applicationContext.CurrentUser.Id;

            TreeViewModel rootTreeViewModel = new TreeViewModel
            {
                ID = 0,
                key = "D0",
                ParentID = 0,
                title = "ALL",
                isFolder = true,
                activate = true,
                folderName = Convert.ToString(Session["Folder"]),
                isUpload = Convert.ToBoolean(Session["isUpload"]),
                isTabSelected = Convert.ToBoolean(Session["isActiveTab"])


            };

            try
            {
                if (Session["logInUserId"] != null)
                {
                    string ownerUser = string.Empty;

                    var loggedInUserId = Session["logInUserId"].ToString();
                    var roleName = Session["RoleName"].ToString();
                    var ticketId = Convert.ToString(Session["TicketId"]);
                    string originalTicketID = ticketId;
                    if (!string.IsNullOrEmpty(ticketId))
                    {
                        ticketId = ticketId.Replace("-", "_");
                    }

                    var resultFileList = new List<DMSDocument>();
                    var resultDirectoryList = new List<DMSDirectory>();

                    if (!string.IsNullOrEmpty(ticketId))
                    {
                        var directory = repositoryService.GetUserRepoDirectory(_userId, ticketId);

                        if (directory == null)
                        {
                            ownerUser = GetOwnerUser(ticketId, _userId);
                            repositoryService.CreateNewFolder(ticketId, 0, _userId, ownerUser);

                            directory = repositoryService.GetUserRepoDirectory(_userId, ticketId);
                        }

                        if (directory != null)
                        {
                            rootTreeViewModel = new TreeViewModel
                            {
                                ID = directory.DirectoryId,
                                key = $"D{directory.DirectoryId}",
                                ParentID = 0,
                                title = directory.DirectoryName,
                                isFolder = true,
                                activate = true,
                                folderName = Convert.ToString(Session["Folder"]),
                                isUpload = Convert.ToBoolean(Session["isUpload"]),
                                isTabSelected = Convert.ToBoolean(Session["isActiveTab"])
                            };

                            resultDirectoryList.Add(directory);
                            repositoryService.BuildRecursiveDirectories(resultDirectoryList);

                            resultDirectoryList = resultDirectoryList.First(x => x.DirectoryId == directory.DirectoryId).DirectoryList.ToList();
                        }
                    }
                    else
                    {
                        resultDirectoryList = repositoryService.GetUserRepoDirectories(_userId);
                        repositoryService.BuildRecursiveDirectories(resultDirectoryList);
                    }

                    //var directoryIds = resultDirectoryList.SelectMany(x => x.DirectoryList).Select(x => x.DirectoryId).Distinct().ToList();
                    //List<DMSDocument> resultFileList = repositoryService.GetUserRepoDocuments(_userId, directoryIds);

                    //// temp fix for CRM
                    //if (roleName != "Admin" && roleName != "SuperAdmin")
                    //{
                    //    var userDirectoryHierarchyList = repositoryService.BuildRecursiveDirectoriesHierarchy(loggedInUserId);
                    //    var userFilesList = repositoryService.UserFilesList(loggedInUserId);
                    //    var userFilesDirectoryIdList = repositoryService.FilesDirectoryId(userFilesList);
                    //    var finalDirectoryIdList = userDirectoryHierarchyList.Union(userFilesDirectoryIdList).ToList();
                    //    var sortedFinalDirectoryIdList = finalDirectoryIdList.OrderBy(id => id).ToList();
                    //    repositoryService.PopulateCurrentUserDirectory(treeViewModel, resultDirectoryList, resultFileList, sortedFinalDirectoryIdList, userFilesList);
                    //}
                    //else
                    {
                        repositoryService.PopulateCurrentDirectory(rootTreeViewModel, resultDirectoryList, resultFileList);
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(rootTreeViewModel), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Upload(int directoryId, FormCollection collection, HttpPostedFileBase[] files)
        {
            StatusModel status = new StatusModel();
            try
            {
                if (Session["logInUserId"] != null)
                {
                    var grantAccess = true;
                    var loggedInUser = Session["logInUserId"];
                    var roleName = Session["RoleName"].ToString();

                    //MembershipUser membershipUser = Membership.GetUser(loggedInUser);
                    // var userId = Guid.Parse(membershipUser.ProviderUserKey.ToString());

                    DMSManagerService repositoryService = new DMSManagerService(_applicationContext);
                    List<string> fileNames = new List<string>();

                    if (repositoryService.hasUserWriteAccess(directoryId, _userId, roleName) || grantAccess)
                    {
                        string fileVersion = collection["versionId"];
                        TreeViewModel dyanaTreeViewItemFile = new TreeViewModel();

                        foreach (HttpPostedFileBase file in files)
                        {
                            if (file.ContentLength <= 0)
                            {
                                status.Message = "File is empty.";
                            }
                            else
                            {
                                var documentList = repositoryService.Upload(directoryId, fileVersion, _userId, files);

                                //// do not add file as child node in tree for CRM
                                //if (document != null)
                                //{
                                //    dyanaTreeViewItemFile.ID = document.FileId;
                                //    dyanaTreeViewItemFile.key = "F" + document.FileId.ToString();
                                //    dyanaTreeViewItemFile.ParentID = document.DirectoryId;
                                //    dyanaTreeViewItemFile.title = document.FileName;
                                //    dyanaTreeViewItemFile.isFolder = false;
                                //    dyanaTreeViewItemFile.isSubFolder = false;
                                //    dyanaTreeViewItemFile.activate = false;

                                //    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(dyanaTreeViewItemFile);
                                //    return Json(jsonString, JsonRequestBehavior.AllowGet);
                                //}

                                if (documentList.Count > 0)
                                {
                                    fileNames.Add(file.FileName);
                                    status.Message = "File uploaded successfully.";
                                    status.IsSuccess = true;
                                    status.ErrorCode = 0;
                                }
                                else
                                {
                                    status.Message = "Uploaded file is already present.";
                                }
                            }
                        }

                        if (fileNames != null && fileNames.Count > 0)
                        {
                            string ticketId = Convert.ToString(Session["TicketId"]);
                            repositoryService.UpdateHistory(_applicationContext.CurrentUser, ticketId, $"{string.Join($"{Constants.Separator6} ", fileNames)} version {fileVersion} File(s) added by {_applicationContext.CurrentUser.Name}");
                        }
                       
                    }
                    else
                    {
                        status.Message = "You don't have access to upload.";
                        status.IsSuccess = false;
                        status.ErrorCode = -111;
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
                status.Message = "Failed to upload, try after sometime.";
                ULog.WriteException(ex);
            }

            return new JsonResult() { Data = status };
        }

        [HttpPost]
        public ActionResult UploadCheckIn(int id, FormCollection collection, HttpPostedFileBase[] files)
        {
            StatusModel status = new StatusModel();
            try
            {
                if (Session["logInUserId"] != null)
                {
                    var loggedInUser = Session["logInUserId"];
                    var roleName = Session["RoleName"].ToString();
                    //MembershipUser membershipUser = Membership.GetUser(loggedInUser);
                    DMSManagerService repositoryService = new DMSManagerService(_applicationContext);

                    // var userId = Guid.Parse(membershipUser.ProviderUserKey.ToString());

                    //bool hasWriteAccess = repositoryService.hasUserWriteAccess(id, userId, roleName);
                    if (repositoryService.hasUserWriteAccess(id, _userId, roleName))
                    {
                        string fileVersion = collection["versionId"];
                        status = repositoryService.UploadCheckIn(id, fileVersion, _userId, files);
                    }
                    else
                    {
                        status.Message = "you don't have access to check in.";
                        status.IsSuccess = false;
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
                status.Message = "Failed to upload, try after sometime.";
                ULog.WriteException(ex);
            }
            return new JsonResult() { Data = status };
        }

        [HttpPost]
        public JsonResult UndoCheckout(int fileId)
        {
            StatusModel status = new StatusModel();
            try
            {
                if (Session["logInUserId"] != null)
                {
                    var loggedInUser = Session["logInUserId"];
                    var roleName = Session["RoleName"].ToString();
                    // MembershipUser membershipUser = Membership.GetUser(loggedInUser);

                    //var userId = Guid.Parse(membershipUser.ProviderUserKey.ToString());

                    DMSManagerService repositoryService = new DMSManagerService(_applicationContext);

                    //bool hasWriteAccess = repositoryService.hasUserWriteAccess(fileId, userId, roleName);
                    if (repositoryService.hasUserWriteAccess(fileId, _userId, roleName))
                    {
                        status.IsSuccess = false;
                        status.Message = "Undo checkout failed.";

                        var updateDoc = repositoryService.UndoCheckout(fileId, _userId);
                        if (updateDoc == 1)
                        {
                            status.IsSuccess = true;
                            status.Message = "Undo checkout successful.";
                            return new JsonResult() { Data = status };
                        }
                        else if (updateDoc == -1)
                        {
                            status.IsSuccess = false;
                            status.Message = "Please Checkedout file first";
                        }
                        else
                        {
                            status.IsSuccess = false;
                            status.Message = "This file is check out by other user.";
                        }
                    }
                    else
                    {
                        status.Message = "You don't have undo check out access.";
                        status.IsSuccess = false;
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
                ULog.WriteException(ex);
            }
            return new JsonResult() { Data = status };
        }

        [HttpPost]
        public JsonResult Checkout(int fileId)
        {
            StatusModel status = new StatusModel();
            try
            {
                if (Session["logInUserId"] != null)
                {
                    var loggedInUser = Session["logInUserId"];
                    var roleName = Session["RoleName"].ToString();
                    DMSManagerService repositoryService = new DMSManagerService(_applicationContext);
                    //MembershipUser membershipUser = Membership.GetUser(loggedInUser);
                    //var userIdGuid = Guid.Parse(membershipUser.ProviderUserKey.ToString());

                    //string userIdGuid = null;


                    if (repositoryService.hasUserWriteAccess(fileId, _userId, roleName))
                    {
                        status = repositoryService.Checkout(fileId, _userId, roleName);
                    }
                    else
                    {
                        status.Message = "You don't have check out access.";
                        status.IsSuccess = false;
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
                status.Message = "Fail to check Out, try after sometime.";
                ULog.WriteException(ex);
            }
            return new JsonResult() { Data = status };
        }

        [HttpGet]
        public JsonResult DownloadLatestFile(int fileId)
        {
            StatusModel status = new StatusModel();
            try
            {
                if (Session["logInUserId"] != null)
                {
                    var loggedInUser = Session["logInUserId"];
                    //MembershipUser membershipUser = Membership.GetUser(loggedInUser);

                    //var userIdGuid = Guid.Parse(membershipUser.ProviderUserKey.ToString());
                    // string userIdGuid = "91d3d9f8-4bbc-490d-97bc-4964e6fafeae";

                    DMSManagerService repositoryService = new DMSManagerService(_applicationContext);

                    status = repositoryService.DownloadLatestFile(fileId, _userId);
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
                status.Message = "Download failed. " + ex.Message;
            }
            return new JsonResult() { Data = status };
        }

        [HttpGet]
        public ActionResult DownloadFile(int fileId)
        {
            var status = new StatusModel();

            try
            {
                if (Session["logInUserId"] != null)
                {
                    var repositoryService = new DMSManagerService(_applicationContext);
                    repositoryService.DownloadFile(fileId);

                    status.IsSuccess = true;
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
                status.Message = "Download failed";
                ULog.WriteException(ex);
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult IsCheckOutByMe(int fileId)
        {
            StatusModel status = new StatusModel();

            try
            {
                if (Session["logInUserId"] != null)
                {
                    status.IsSuccess = false;
                    status.Message = "File is not checked out by you.";

                    var loggedInUser = Session["logInUserId"];
                    var roleName = Session["RoleName"].ToString();

                    //MembershipUser membershipUser = Membership.GetUser(loggedInUser);

                    //var userId = Guid.Parse(membershipUser.ProviderUserKey.ToString());

                    DMSManagerService repositoryService = new DMSManagerService(_applicationContext);


                    if (!(repositoryService.hasUserWriteAccess(fileId, _userId, roleName)))
                    {
                        status.Message = "You don't have check in access.";
                        status.IsSuccess = false;
                    }
                    else if (!repositoryService.IsCheckOut(fileId))
                    {
                        status.Message = "Please Checkout file first.";
                    }
                    else
                    {
                        var documentDetails = repositoryService.GetLatestCheckoutFileByUserIdAndId(_userId, fileId);
                        if (documentDetails != null && documentDetails.IsCheckedOut)
                        {
                            status.IsSuccess = true;
                            List<DMSDocument> currentDocument = repositoryService.GetVersion(fileId);
                            var versions = repositoryService.ListVersions(currentDocument);
                            return Json(new { Data = status, version = string.Join("<br>", versions) }, JsonRequestBehavior.AllowGet);
                        }
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
                status.Message = ex.Message;
                ULog.WriteException(ex);
            }
            return Json(new { Data = status }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult SelectFile(int id, bool isFolder)
        {
            return GetDirectoryDetail(id, isFolder);
        }

        [HttpPost]
        public JsonResult CreateNewFolder(string folderName, int directoryId)
        {
            StatusModel status = new StatusModel();
            try
            {
                if (Session["logInUserId"] != null)
                {
                    var grantAccess = true;
                    var loggedInUser = Session["logInUserId"];
                    var roleName = Session["RoleName"].ToString();

                    //MembershipUser membershipUser = Membership.GetUser(loggedInUser);
                    //var userId = Guid.Parse(membershipUser.ProviderUserKey.ToString());

                    DMSManagerService repositoryService = new DMSManagerService(_applicationContext);

                    if (repositoryService.hasUserWriteAccess(directoryId, _userId, roleName) || grantAccess)
                    {
                        var directory = repositoryService.CreateNewFolder(folderName, directoryId, _userId, _userId);
                        if (directory != null)
                        {
                            TreeViewModel dyanaTreeViewItem = new TreeViewModel();
                            dyanaTreeViewItem.ID = directory.DirectoryId;
                            dyanaTreeViewItem.key = "D" + directory.DirectoryId.ToString();
                            dyanaTreeViewItem.ParentID = directory.DirectoryParentId;
                            dyanaTreeViewItem.title = directory.DirectoryName;
                            dyanaTreeViewItem.isFolder = true;
                            dyanaTreeViewItem.isSubFolder = true;

                            string ticketId = Convert.ToString(Session["TicketId"]);
                            repositoryService.UpdateHistory(_applicationContext.CurrentUser, ticketId, $"Folder '{folderName}' created by {_applicationContext.CurrentUser.Name}");

                            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(dyanaTreeViewItem);
                            return Json(jsonString, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            status.IsSuccess = false;
                            status.Message = "Failed to create folder " + folderName;
                        }
                    }
                    else
                    {
                        status.Message = "You don't have access to create folder.";
                        status.IsSuccess = false;
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
                status.Message = "Failed to create folder " + folderName;
                ULog.WriteException(ex);
            }
            return new JsonResult() { Data = status };
        }

        [HttpPost]
        public PartialViewResult DeleteFiles(int directoryId, int[] fileIds)
        {
            var repositoryService = new DMSManagerService(_applicationContext);

            try
            {
                if (fileIds != null && fileIds.Any())
                {
                    var documents = repositoryService.GetFilesByIds(fileIds.ToList());
                    repositoryService.DeleteDocuments(fileIds.ToList());

                    if (documents != null && documents.Count > 0)
                    {
                        string ticketId = Convert.ToString(Session["TicketId"]);
                        repositoryService.UpdateHistory(_applicationContext.CurrentUser, ticketId, $"{string.Join($"{Constants.Separator6} ", documents.Select(x => x.FileName).ToList())} File(s) deleted by {_applicationContext.CurrentUser.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return GetDirectoryDetail(directoryId, true);
        }
        [HttpPost]
        public ActionResult LinkFile(int directoryId, int[] fileIds)
        {
            //FileUploadControl fileUploadControl = new FileUploadControl();

            //uHelper.ClosePopUpAndEndResponse(System.Web.HttpContext.Current, false);
            //EditTask editTask = new EditTask();
            //editTask.TestAjax();
            ////var repositoryService = new DMSManagerService(_applicationContext);

            //try
            //{
            //    //if (fileIds != null && fileIds.Any())
            //    //{
            //    //    repositoryService.DeleteDocuments(fileIds.ToList());
            //    //}
            //}
            //catch (Exception ex)
            //{
            //}

            return View();
        }

        [HttpPost]
        public PartialViewResult DeleteFolder(int directoryId)
        {
            var repositoryService = new DMSManagerService(_applicationContext);
            var directory = repositoryService.GetDirectory(directoryId);
            var directoryParentId = 0;
            string ticketId = Convert.ToString(Session["TicketId"]);

            try
            {
                if (directory != null && directory.DirectoryParentId != 0)
                {
                    directoryParentId = directory.DirectoryParentId.Value;
                    repositoryService.DeleteDirectoryRecursive(directoryId);
                    repositoryService.UpdateHistory(_applicationContext.CurrentUser, ticketId, $"Folder '{directory.DirectoryName}' deleted by {_applicationContext.CurrentUser.Name}");
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return GetDirectoryDetail(directoryParentId, true);
        }

        public PartialViewResult GetDirectoryDetail(int id, bool isFolder)
        {
            var directoryDetail = new DirectoryDetail();
            _userProfileManager = System.Web.HttpContext.Current.GetOwinContext().Get<UserProfileManager>();


            try
            {
                var repositoryService = new DMSManagerService(_applicationContext);

                var folderFileData = repositoryService.SelectedDirectoryFileData(id, isFolder, Session["RoleName"].ToString(), Session["logInUserId"].ToString());

                if (isFolder)
                {
                    var totalSize = folderFileData.Sum(x => x.Size);
                    var mbSize = 0d;

                    if (totalSize > 0)
                    {
                        mbSize = Math.Round((double)totalSize / (1024 * 1024), 2);
                    }

                    var directory = repositoryService.GetDirectory(id);

                    directoryDetail.Name = directory.DirectoryName;
                    directoryDetail.DirectorySize = $"{mbSize} MB";
                    directoryDetail.FileCount = folderFileData.Count(x => x.IsFile);
                    directoryDetail.Modified = directory.UpdatedOn?.ToString("MMM-dd-yyyy");
                    directoryDetail.Owners = _userProfileManager.GetUserNamesById(directory.OwnerUser);
                    directoryDetail.DirectoryParentId = directory.DirectoryParentId;
                }

                directoryDetail.Files = folderFileData;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return PartialView("_FileDetailsList", directoryDetail);
        }
        public string GetOwnerUser(string ticketID, string defaultOwner ="")
        {
            string ownerUser = string.Empty;
            try
            {
                List<string> userList = new List<string>();
                string ModuleName = uHelper.getModuleNameByTicketId(ticketID);
                DataRow currentTicket = Ticket.GetCurrentTicket(_applicationContext, ModuleName, ticketID);
                if (currentTicket != null)
                {
                    if (currentTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketStageActionUsers) && !string.IsNullOrEmpty(currentTicket[DatabaseObjects.Columns.TicketStageActionUsers].ToString()))
                    {
                        userList = $"{currentTicket[DatabaseObjects.Columns.TicketStageActionUsers]}".Split(new string[] { ";#" }, StringSplitOptions.None).ToList();
                    }
                    if (currentTicket.Table.Columns.Contains(DatabaseObjects.Columns.Owner) && !string.IsNullOrEmpty(currentTicket[DatabaseObjects.Columns.Owner].ToString()))
                    {
                        userList.Add(currentTicket[DatabaseObjects.Columns.Owner].ToString());
                    }
                    ownerUser = string.Join(",", userList.Select(x => x).Distinct());
                }
                if (!string.IsNullOrEmpty(ownerUser))
                    ownerUser = defaultOwner;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, MessageCategory.DocumentManagement);
                ownerUser = defaultOwner;
            }
            return ownerUser;
        }
        #endregion

    }
}
