using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using uGovernIT.DAL;
using uGovernIT.Manager.DMSOperations;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DMSDB;

namespace uGovernIT.Manager
{
    public class DMSManagerBase : IDMSManagerBase
    {
        protected ApplicationContext dbContext;
        protected IDocRepository docRepository;

        public DMSManagerBase(ApplicationContext context)
        {
            dbContext = context;
            docRepository = new DocRepositoryBase(dbContext);
        }

        public virtual List<DMSDirectory> GetChildDirectories(int id)
        {
            return docRepository.GetChildDirectories(id);
        }

        public virtual List<DMSDirectory> GetUserRepoDirectories(string userGuid)
        {
            return docRepository.GetUserRepoDirectories(userGuid);
        }

        public virtual List<DMSDirectory> GetChildDirectoriesByDirectName(string directoryName)
        {
            return docRepository.GetChildDirectoriesByDirectName(directoryName);
        }

        public virtual DMSDirectory GetUserRepoDirectory(string userGuid, string folderName)
        {
            return docRepository.GetUserRepoDirectory(userGuid, folderName);
        }

        public virtual List<int?> BuildRecursiveDirectoriesHierarchy(string userGuid)
        {
            return docRepository.UserAccessedDirectoriesHierarchy(userGuid);
        }

        public virtual List<int?> UserFilesList(string userGuid)
        {
            return docRepository.UserFilesId(userGuid);
        }

        public virtual List<int?> FilesDirectoryId(List<int?> fileIdList)
        {
            return docRepository.FilesParentDirectoryId(fileIdList);
        }

        public virtual void SaveDirectory(DMSDirectory directory)
        {
            docRepository.SaveDirectory(directory);
        }

        public virtual void UpdateDirectory(DMSDirectory directory)
        {
            docRepository.UpdateDirectory(directory);
        }

        public virtual void DeleteDirectoryRecursive(int directoryId)
        {
            docRepository.DeleteDirectoryRecursive(directoryId);
        }

        public virtual List<DMSDocument> GetUserRepoDocuments(string userGuid, List<int> directoryIds)
        {
            return docRepository.GetUserRepoDocuments(userGuid, directoryIds);
        }

        public virtual void SaveDocument(DMSDocument doc)
        {
            docRepository.SaveDocument(doc);
        }

        public virtual int UpdateDocument(DMSDocument doc)
        {
            return docRepository.UpdateDocument(doc);
        }

        public virtual void DeleteDocuments(List<int> fileIds)
        {
            docRepository.DeleteDocuments(fileIds);
        }

        public virtual bool DeleteDocumentByfileId(int fileId)
        {
            return(docRepository.DeleteDocumentByfileId(fileId));
        }
        

        public virtual List<DMSDocument> GetFilesWithName(int directoryId, string fileName, string fileVersion)
        {
            return docRepository.GetFilesWithName(directoryId, fileName, fileVersion);
        }

        public virtual int UndoCheckout(int FileId, string userId)
        {
            var documents = GetVersion(FileId);
            int updateDoc = 0;
            if (documents[0].CheckOutBy == null)
            {
                return updateDoc = -1;
            }
            if (documents != null && documents.Count > 0 && (documents[0].IsCheckedOut) && documents[0].CheckOutBy == userId)
            {
                var document = documents[0];
                // MembershipUser membershipUser = Membership.GetUser();
                document.IsCheckedOut = false;
                document.CheckOutBy = null;
                updateDoc = UpdateDocument(document);
            }
            return updateDoc;
        }

        public virtual DMSDocument GetLatestCheckoutFileByUserIdAndId(string userId, int Id)
        {
            return docRepository.GetLatestCheckoutFileByUserIdAndId(userId, Id);
        }

        public virtual List<DMSDocument> GetVersion(int Id)
        {
            return docRepository.GetVersion(Id);
        }

        public virtual void ResetCheckout(List<DMSDocument> listFileDetails)
        {
            docRepository.ResetCheckout(listFileDetails);
        }

        public virtual List<DMSDocument> GetMainFilesByDirId(int dirId)
        {
            return docRepository.GetMainFilesByDirId(dirId);
        }

        public virtual DMSDirectory GetDirectory(int dirId)
        {
            return docRepository.GetDirectory(dirId);
        }

        public virtual ICollection<DMSDirectory> BuildRecursiveDirectories(ICollection<DMSDirectory> resultList)
        {
            return docRepository.BuildRecursiveDirectories(resultList);
        }

        public virtual TreeViewModel PopulateCurrentUserDirectory(TreeViewModel treeViewModelCurrent, ICollection<DMSDirectory> resultList, ICollection<DMSDocument> resultFileList, List<int?> finalDirectoryIdList, List<int?> userFilesList)
        {
            return docRepository.PopulateCurrentUserDirectory(treeViewModelCurrent, resultList, resultFileList, finalDirectoryIdList, userFilesList);
        }

        public virtual TreeViewModel PopulateCurrentDirectory(TreeViewModel treeViewModelCurrent, ICollection<DMSDirectory> resultDirectoryList, ICollection<DMSDocument> resultFileList)
        {
            return docRepository.PopulateCurrentDirectory(treeViewModelCurrent, resultDirectoryList, resultFileList);
        }

        public virtual string BuildPath(int? directoryId)
        {
            var title = "";
            while (directoryId != 0)
            {
                var text = docRepository.GetPathByDirectoryId(directoryId);
                title = text.DirectoryName + "/" + title;
                directoryId = text.DirectoryParentId;
            }
            var relativePath = "ALL/" + dbContext.TenantAccountId + "/" + title;
            return relativePath;
        }

        public virtual DMSDocument GetDocumentVersions(List<DMSDocument> listFileDetails, string fileVersion)
        {
            return docRepository.GetDocumentVersions(listFileDetails, fileVersion);
        }

        public virtual List<FilesDirectories> SelectedDirectoryFileData(int id, bool isFolder, string roleName, string userId)
        {
            return docRepository.SelectedDirectoryFileData(id, isFolder, roleName, userId);
        }

        public virtual List<DMSDocument> Upload(int DirectoryId, string fileVersion, string userId, HttpPostedFileBase[] files)
        {
            var documentList = new List<DMSDocument>();
            var document = new DMSDocument();

            DateTime localDate = DateTime.Now;

            var path = BuildPath(DirectoryId);

            foreach (HttpPostedFileBase file in files)
            {
                if (file.ContentLength > 0)
                {
                    // check if same file name is alrady present
                    var InputFileName = Path.GetFileName(file.FileName);

                    var duplicateDocuments = GetFilesWithName(DirectoryId, InputFileName, fileVersion);
                    var duplicateDocumentIds = duplicateDocuments.Select(x => x.FileId).ToList();

                    if (duplicateDocumentIds.Any())
                    {
                        DeleteDocuments(duplicateDocumentIds);
                    }

                    //if (duplicateDocuments.Count <= 0)
                    //{
                    document = new DMSDocument
                    {
                        FileId = 0,
                        FileName = InputFileName,
                        FullPath = path,
                        StoredFileName = Guid.NewGuid().ToString(),
                        Size = file.ContentLength,
                        MainVersionFileId = 1,
                        Version = fileVersion,
                        CustomerId = 1,
                        IsCheckedOut = false,
                        FileParentId = 0,
                        DirectoryId = DirectoryId,
                        AuthorId = userId,
                        CreatedByUser = userId,
                        UpdatedBy = userId, //vinod_check
                        CreatedOn = localDate,
                        UpdatedOn = localDate,
                        Deleted = false,
                        CheckOutBy = null,
                        Title = InputFileName,
                        ReviewRequired = false,
                        ReviewStep = 1,
                        DocumentControlID = null
                    };

                    if (StorageUtil.Store(file, StorageUtil.GetDMSBaseFolderPath(), path, document.StoredFileName))
                    {
                        SaveDocument(document);

                        UpdateUserPermissions(document.FileId, DirectoryId, userId, "NewFile");
                        documentList.Add(document);
                    }

                    //}
                }
            }

            if (documentList.Count > 0)
                return documentList;
            else
                return null;
        }

        public virtual StatusModel UploadCheckIn(int Id, string fileVersion, string userId, HttpPostedFileBase[] files)
        {
            StatusModel status = new StatusModel();
            status.IsSuccess = false;
            status.Message = "Failed to upload, try after sometime.";
            DMSDocument documentFirst = new DMSDocument();
            DMSDocument documentLast = new DMSDocument();
            DateTime localDate = System.DateTime.Now;
            HttpPostedFileBase file = files[0];
            var listFileDetails = GetVersion(Id);

            // file is already present , get files details
            // check version
            // if version is not present then use last version
            if (listFileDetails.Count > 0)
            {
                documentLast = listFileDetails[0];
                documentFirst = listFileDetails[listFileDetails.Count - 1];
            }
            DMSDocument document = new DMSDocument();
            document.FileName = file.FileName;
            document.IsCheckedOut = false;
            document.CheckOutBy = null;
            document.FullPath = documentLast.FullPath;
            document.DirectoryId = documentLast.DirectoryId;
            var documentVersion = GetDocumentVersions(listFileDetails, fileVersion);
            if (string.IsNullOrEmpty(fileVersion) || documentVersion != null)
            { // overwrite same file
              // overwrite last version file
                if (string.IsNullOrEmpty(fileVersion))
                    document = documentLast;
                else
                    document = documentVersion;
                document.UpdatedBy = userId;
                if (document.StoredFileName == null)
                {
                    document.StoredFileName = Guid.NewGuid().ToString();
                    UpdateDocument(document);
                }
                if (StorageUtil.Store(file, StorageUtil.GetDMSBaseFolderPath(), document.FullPath, document.StoredFileName))
                {
                    ResetCheckout(listFileDetails);
                    status.IsSuccess = true;
                    return status;
                }
            }
            else
            {
                var size = file.ContentLength;
                document.StoredFileName = Guid.NewGuid().ToString();
                document.FileId = 0;
                document.Size = size;
                document.MainVersionFileId = 1;
                document.Version = fileVersion;
                document.CustomerId = 1;
                document.IsCheckedOut = false;
                document.FileParentId = documentFirst.FileId;
                document.AuthorId = userId;
                document.CreatedByUser = userId;
                document.UpdatedBy = userId; //vinod_check
                document.CreatedOn = localDate;
                document.UpdatedOn = localDate;
                document.Deleted = false;
                //string pathToSave = Path.Combine(Server.MapPath("~/UploadedFiles"), InputFileName);

                if (StorageUtil.Store(file, StorageUtil.GetDMSBaseFolderPath(), document.FullPath, document.StoredFileName))
                {
                    ResetCheckout(listFileDetails);
                    SaveDocument(document);
                    status.IsSuccess = true;
                    status.Message = "File Uploaded successfully.";
                    return status;
                }
            }
            return status;
        }

        public virtual StatusModel Checkout(int FileId, string userIdGuid, string roleName)
        {
            StatusModel status = new StatusModel();
            status.IsSuccess = false;
            status.Message = "";
            var isFileAccessible = docRepository.UserFilesId(userIdGuid, FileId);
            if (isFileAccessible || roleName == "Admin" || roleName == "SuperAdmin")
            {
                var documents = docRepository.GetVersion(FileId);

                if (documents != null && documents.Count > 0 && !(documents[0].IsCheckedOut))
                {
                    var document = documents[0];
                    document.IsCheckedOut = true;
                    document.CheckOutBy = userIdGuid;
                    if (docRepository.UpdateDocument(document) == 1)
                    {
                        if (docRepository.LogDocumentsDownloaded(FileId, userIdGuid) == 1)
                        {
                            status.IsSuccess = true;
                        }
                    }
                }
                else
                {
                    status.IsSuccess = false;
                    status.Message = "File is already checkedout";
                }
            }
            return status;
        }

        public virtual StatusModel DownloadLatestFile(int fileId, string userIdGuid)
        {
            StatusModel status = new StatusModel();
            status.IsSuccess = false;

            var document = docRepository.GetLatestCheckoutFileByUserIdAndId(userIdGuid, fileId);
            if (document != null)
            {
                if (document.StoredFileName != null)
                {
                    StorageUtil.Download(document.FullPath, document.StoredFileName, document.FileName);
                }
                else
                {
                    StorageUtil.Download(document.FullPath, document.FileName, document.FileName);
                }
                status.IsSuccess = true;
                status.Message = "File Downloaded.";
            }
            else
            {
                status.IsSuccess = false;
                status.ErrorCode = -999;
            }
            return status;
        }

        public virtual DMSDirectory CreateNewFolder(string folderName, int DirectoryId, string userId, string Owners)
        {
            var localDate = DateTime.Now;
            var directoryNames = GetDuplicateDirectories(DirectoryId, folderName);

            if (directoryNames.Any(x => x.DirectoryName.Contains(folderName) && x.Deleted))
            {
                var directory = directoryNames.FirstOrDefault(x => x.DirectoryName.Contains(folderName) && x.Deleted);

                directory.Deleted = false;
                directory.UpdatedBy = userId;
                directory.UpdatedOn = localDate;

                // update directory
                UpdateDirectory(directory);

                return directory;
            }

            if (!directoryNames.Any(x => x.DirectoryName.Contains(folderName)))
            {
                DMSDirectory directory = new DMSDirectory
                {
                    DirectoryName = folderName,
                    DirectoryParentId = DirectoryId,
                    AuthorId = userId,
                    UpdatedBy = userId,
                    CreatedByUser = userId,
                    CreatedOn = localDate,
                    UpdatedOn = localDate,
                    TenantID = dbContext.TenantID,
                    OwnerUser = Owners
                };

                SaveDirectory(directory);

                UpdateUserPermissions(directory.DirectoryId, DirectoryId, userId, "NewFolder");

                return directory;
            }
            return null;
        }

        public virtual List<string> GetDirectoryNamesList(int DirectoryId)
        {
            return docRepository.DirectoryNamesList(DirectoryId);
        }

        public virtual List<DMSDirectory> GetDuplicateDirectories(int directoryId, string folderName)
        {
            return docRepository.GetDuplicateDirectories(directoryId, folderName);
        }

        public virtual void DownloadFile(int fileId)
        {
            var document = docRepository.GetFilesById(fileId);

            if (document != null)
            {
                if (document.StoredFileName != null)
                {
                    StorageUtil.Download(document.FullPath, document.StoredFileName, document.FileName);
                }
                else
                {
                    StorageUtil.Download(document.FullPath, document.FileName, document.FileName);
                }
            }
        }

        public virtual bool IsCheckOut(int fileId)
        {
            return docRepository.IsCheckOut(fileId);
        }

        public virtual List<string> ListVersions(List<DMSDocument> currentDocument)
        {
            return docRepository.ListVersions(currentDocument);
        }

        public virtual bool hasUserWriteAccess(int fileId, string userId, string roleName)
        {
            var hasCheckOutAccess = docRepository.HasCheckInAccess(userId, fileId);
            return (!hasCheckOutAccess && (!(roleName == "Admin" || roleName == "SuperAdmin"))) ? false : true;
        }

        public virtual void UpdateUserPermissions(int ID, int ParentDirectoryId, string AdminId, string Identity)
        {
            docRepository.UpdateUserDirectoryFilePermissions(ID, ParentDirectoryId, AdminId, Identity);
        }

        public virtual DMSTenantDocumentsDetails GetTenantDocumentsDetailByTenantId(string tenantId)
        {
            return docRepository.GetTenantDocumentsDetailByTenantId(tenantId);
        }


        /// <summary>
        /// Recursive copy for Directory, Sub Directories, and files with in Directories (eg., copy Directories from OPM to CPR).
        /// </summary>        
        public void CopyDMSDirectory(string SourceTicketId, string TargetTicketId)
        {
            var userId = dbContext.CurrentUser.Id;
            List<string> userList = new List<string>();
            string ModuleName = uHelper.getModuleNameByTicketId(TargetTicketId);
            string ownerUser = string.Empty;
            DataRow currentTicket = Ticket.GetCurrentTicket(System.Web.HttpContext.Current.GetManagerContext(), ModuleName, TargetTicketId);
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
                ownerUser = userId;

            if (!string.IsNullOrEmpty(SourceTicketId))
                SourceTicketId = SourceTicketId.Replace("-", "_");

            if (!string.IsNullOrEmpty(TargetTicketId))
                TargetTicketId = TargetTicketId.Replace("-", "_");

            var sourceDirectory = GetUserRepoDirectory(userId, SourceTicketId);
            if (sourceDirectory != null)
            {
                try
                {
                    var TargetDirectory = GetUserRepoDirectory(userId, TargetTicketId);

                    if (TargetDirectory == null)
                    {
                        var resultDirectoryList = new List<DMSDirectory>();

                        CreateNewFolder(TargetTicketId, 0, userId, ownerUser);
                        TargetDirectory = GetUserRepoDirectory(userId, TargetTicketId);

                        resultDirectoryList.Add(TargetDirectory);
                        //BuildRecursiveDirectories(resultDirectoryList);

                        // source directure structure.
                        var resultSourceDirectoryList = new List<DMSDirectory>();
                        resultSourceDirectoryList.Add(sourceDirectory);
                        BuildRecursiveDirectories(resultSourceDirectoryList);

                        CreatedNestedDirectoryinDB(sourceDirectory, TargetDirectory, userId, ownerUser);

                        string sourcePath = Path.Combine(StorageUtil.GetDMSBaseFolderPath(), "ALL", dbContext.TenantAccountId , SourceTicketId);
                        string targetPath = Path.Combine(StorageUtil.GetDMSBaseFolderPath(), "ALL", dbContext.TenantAccountId, TargetTicketId);

                        if (Directory.Exists(sourcePath))
                        {
                            Utility.UGITUtility.DirectoryCopy(sourcePath, targetPath, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Util.Log.ULog.WriteException(ex, "Error in Copying Documents from Module to Module.");
                }
            }
        }

        private void CreatedNestedDirectoryinDB(DMSDirectory sourceDirectory, DMSDirectory targetDirectory, string userId, string ownerUser)
        {
            var files = GetMainFilesByDirId(sourceDirectory.DirectoryId);
            if (files != null && files.Count > 0)
            {
                foreach (var items in files)
                {
                    CreateFile(items, targetDirectory.DirectoryId);
                }
            }

            foreach (var item in sourceDirectory.DirectoryList)
            {
                var dir = CreateNewFolder(item.DirectoryName, targetDirectory.DirectoryId, userId, ownerUser);
                files = GetMainFilesByDirId(dir.DirectoryId);
                if (files != null && files.Count > 0)
                {
                    foreach (var items in files)
                    {
                        CreateFile(items, dir.DirectoryId);
                    }
                }

                CreatedNestedDirectoryinDB(item, dir, userId, ownerUser);
            }
        }

        private void CreateFile(DMSDocument items, int directoryId)
        {
            var newDoc = new DMSDocument();
            newDoc = items;
            newDoc.FileId = 0;
            newDoc.FullPath = BuildPath(directoryId);
            newDoc.DirectoryId = directoryId;
            SaveDocument(newDoc);
        }

        public virtual List<DMSDocument> GetFileListByFileId(string fileId)
        {
            return docRepository.GetFileListByFileId(fileId);
        }

        public void UpdateHistory(UserProfile user, string TicketId, string HistoryDescription)
        {
            string moduleName = uHelper.getModuleNameByTicketId(TicketId);
            Ticket ticket = new Ticket(dbContext, moduleName);
            DataRow currentTicket = Ticket.GetCurrentTicket(dbContext, moduleName, TicketId);
            uHelper.CreateHistory(user, HistoryDescription, currentTicket, true, dbContext);
            ticket.CommitChanges(currentTicket);
        }

        public virtual List<DMSDocument> GetFilesByIds(List<int> Ids)
        {
            return docRepository.GetFilesByIds(Ids);
        }

        public void CreatePortalForTicketElevated(ApplicationContext _applicationContext, string ticketId, string _userId, string ownerUser, string chekDefaultFolders)
        {
            string ticketIdOrginal = ticketId;
            LifeCycle lifeCycle;
            LifeCycleManager lcHelper = new LifeCycleManager(_applicationContext);
            string ModuleName = uHelper.getModuleNameByTicketId(ticketIdOrginal);
            DataRow currentTicket = Ticket.GetCurrentTicket(_applicationContext, ModuleName, ticketIdOrginal);
            if (!string.IsNullOrEmpty(ticketId))
            {
                ticketId = ticketId.Replace("-", "_");
            }
            var directory = GetUserRepoDirectory(_userId, ticketId);
            if (directory == null)
                directory = CreateNewFolder(ticketId, 0, _userId, ownerUser);
            if (!string.IsNullOrEmpty(chekDefaultFolders) && "true" == chekDefaultFolders.ToLower())
            { 
                if ((ModuleName).Equals("PMM"))
                {
                    lifeCycle = lcHelper.LoadProjectLifeCycles().FirstOrDefault(x => x.ID == Convert.ToInt32(currentTicket[DatabaseObjects.Columns.ProjectLifeCycleLookup])); // TicketRequest.Module.List_LifeCycles.FirstOrDefault(x => x.ID ==  Convert.ToInt32(currentTicket[DatabaseObjects.Columns.ProjectLifeCycleLookup]));
                    if (lifeCycle != null)
                    {
                        List<LifeCycleStage> stages = lifeCycle.Stages;
                        List<string> folders = new List<string>();
                        foreach (LifeCycleStage stage in stages)
                        {
                            string folderName = string.Format("{0:D2}-{1}", stage.StageStep, stage.Name);
                            folderName = UGITUtility.Truncate(UGITUtility.ReplaceInvalidCharsInFolderName(folderName), 50);
                            folders.Add(folderName);
                            CreateNewFolder(folderName, directory.DirectoryId, _userId, ownerUser);
                        }

                        UpdateHistory(_applicationContext.CurrentUser, ticketIdOrginal, $"{string.Join($"{Constants.Separator6} ", folders)} Folders added by {_applicationContext.CurrentUser.Name}");
                    }
                }
            else
            {
                CreateNewFolder(ticketId, 0, _userId, ownerUser);
                ConfigurationVariableManager objConfigVariable = new ConfigurationVariableManager(_applicationContext);
                string DefaultFolders = objConfigVariable.GetValue("DefaultFolders");
                if (!string.IsNullOrEmpty(DefaultFolders))
                {
                    foreach (var item in DefaultFolders.Split(new string[] { uGovernIT.Utility.Constants.Separator }, StringSplitOptions.RemoveEmptyEntries))
                        CreateNewFolder(item, directory.DirectoryId, _userId, ownerUser);

                    UpdateHistory(_applicationContext.CurrentUser, ticketIdOrginal, $"{DefaultFolders.Replace(Constants.Separator, $", ")} Folders added by {_applicationContext.CurrentUser.Name}");
                }
            }
            }
        }

    }
}
