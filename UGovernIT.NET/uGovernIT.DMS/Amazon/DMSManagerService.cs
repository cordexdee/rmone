using System.Collections.Generic;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DMSDB;

namespace uGovernIT.DMS.Amazon
{
    public class DMSManagerService
    {
        private ApplicationContext _applicationContext;
        private IDMSManagerBase _dmsManagerBase;

        public DMSManagerService(ApplicationContext context)
        {
            _applicationContext = context;
            _dmsManagerBase = new DMSManagerBase(_applicationContext);
        }

        public List<DMSDirectory> GetChildDirectories(int id)
        {
            return _dmsManagerBase.GetChildDirectories(id);
        }

        public List<DMSDirectory> GetUserRepoDirectories(string userGuid)
        {
            return _dmsManagerBase.GetUserRepoDirectories(userGuid);
        }

        public DMSDirectory GetUserRepoDirectory(string userGuid, string folderName)
        {
            return _dmsManagerBase.GetUserRepoDirectory(userGuid, folderName);
        }

        public List<DMSDirectory> GetChildDirectoriesByDirectName(string directoryName)
        {
            return _dmsManagerBase.GetChildDirectoriesByDirectName(directoryName);
        }

        public List<int?> BuildRecursiveDirectoriesHierarchy(string userGuid)
        {
            return _dmsManagerBase.BuildRecursiveDirectoriesHierarchy(userGuid);
        }

        public List<int?> UserFilesList(string userGuid)
        {
            return _dmsManagerBase.UserFilesList(userGuid);
        }

        public List<int?> FilesDirectoryId(List<int?> fileIdList)
        {
            return _dmsManagerBase.FilesDirectoryId(fileIdList);
        }

        public void SaveDirectory(DMSDirectory directory)
        {
            _dmsManagerBase.SaveDirectory(directory);
        }

        public List<DMSDocument> GetUserRepoDocuments(string userGuid, List<int> directoryIds)
        {
            return _dmsManagerBase.GetUserRepoDocuments(userGuid, directoryIds);
        }

        public void SaveDocument(DMSDocument doc)
        {
            _dmsManagerBase.SaveDocument(doc);
        }

        public int UpdateDocument(DMSDocument doc)
        {
            return _dmsManagerBase.UpdateDocument(doc);
        }

        public List<DMSDocument> GetFilesWithName(int directoryId, string fileName, string fileVersion)
        {
            return _dmsManagerBase.GetFilesWithName(directoryId, fileName, fileVersion);
        }

        public int UndoCheckout(int FileId, string userId)
        {
            return _dmsManagerBase.UndoCheckout(FileId, userId);
        }

        public DMSDocument GetLatestCheckoutFileByUserIdAndId(string userId, int Id)
        {
            return _dmsManagerBase.GetLatestCheckoutFileByUserIdAndId(userId, Id);
        }

        public List<DMSDocument> GetVersion(int Id)
        {
            return _dmsManagerBase.GetVersion(Id);
        }

        public void ResetCheckout(List<DMSDocument> listFileDetails)
        {
            _dmsManagerBase.ResetCheckout(listFileDetails);
        }

        public List<DMSDocument> GetMainFilesByDirId(int dirId)
        {
            return _dmsManagerBase.GetMainFilesByDirId(dirId);
        }

        public DMSDirectory GetDirectory(int dirId)
        {
            return _dmsManagerBase.GetDirectory(dirId);
        }

        public ICollection<DMSDirectory> BuildRecursiveDirectories(ICollection<DMSDirectory> resultList)
        {
            return _dmsManagerBase.BuildRecursiveDirectories(resultList);
        }

        public TreeViewModel PopulateCurrentUserDirectory(TreeViewModel treeViewModelCurrent, ICollection<DMSDirectory> resultList, ICollection<DMSDocument> resultFileList, List<int?> finalDirectoryIdList, List<int?> userFilesList)
        {
            return _dmsManagerBase.PopulateCurrentUserDirectory(treeViewModelCurrent, resultList, resultFileList, finalDirectoryIdList, userFilesList);
        }

        public TreeViewModel PopulateCurrentDirectory(TreeViewModel treeViewModelCurrent, ICollection<DMSDirectory> resultDirectoryList, ICollection<DMSDocument> resultFileList)
        {
            return _dmsManagerBase.PopulateCurrentDirectory(treeViewModelCurrent, resultDirectoryList, resultFileList);
        }

        public string BuildPath(int? directoryId)
        {

            return _dmsManagerBase.BuildPath(directoryId);
        }

        public DMSDocument GetDocumentVersions(List<DMSDocument> listFileDetails, string fileVersion)
        {
            return _dmsManagerBase.GetDocumentVersions(listFileDetails, fileVersion);
        }

        public List<FilesDirectories> SelectedDirectoryFileData(int id, bool isFolder, string roleName, string userId)
        {
            return _dmsManagerBase.SelectedDirectoryFileData(id, isFolder, roleName, userId);
        }

        public List<DMSDocument> Upload(int DirectoryId, string fileVersion, string userId, HttpPostedFileBase[] files)
        {
            return _dmsManagerBase.Upload(DirectoryId, fileVersion, userId, files);
        }

        public StatusModel UploadCheckIn(int Id, string fileVersion, string userId, HttpPostedFileBase[] files)
        {
            return _dmsManagerBase.UploadCheckIn(Id, fileVersion, userId, files);
        }

        public StatusModel Checkout(int FileId, string userIdGuid, string roleName)
        {
            return _dmsManagerBase.Checkout(FileId, userIdGuid, roleName);
        }

        public StatusModel DownloadLatestFile(int fileId, string userIdGuid)
        {
            return _dmsManagerBase.DownloadLatestFile(fileId, userIdGuid);
        }

        public DMSDirectory CreateNewFolder(string folderName, int DirectoryId, string userId, string Owners)
        {
            return _dmsManagerBase.CreateNewFolder(folderName, DirectoryId, userId, Owners);
        }
        public void CreatePortalForTicketElevated(ApplicationContext _applicationContext, string ticketId, string _userId, string ownerUser, string chekDefaultFolders)
        {
            _dmsManagerBase.CreatePortalForTicketElevated(_applicationContext, ticketId, _userId, ownerUser, chekDefaultFolders);
        }
        public List<string> GetDirectoryNamesList(int DirectoryId)
        {
            return _dmsManagerBase.GetDirectoryNamesList(DirectoryId);
        }

        public void DownloadFile(int fileId)
        {
            _dmsManagerBase.DownloadFile(fileId);
        }

        public bool IsCheckOut(int fileId)
        {
            return _dmsManagerBase.IsCheckOut(fileId);
        }

        public List<string> ListVersions(List<DMSDocument> currentDocument)
        {
            return _dmsManagerBase.ListVersions(currentDocument);
        }

        public bool hasUserWriteAccess(int fileId, string userId, string roleName)
        {
            return _dmsManagerBase.hasUserWriteAccess(fileId, userId, roleName);
        }

        public void UpdateUserPermissions(int ID, int ParentDirectoryId, string AdminId, string Identity)
        {
            _dmsManagerBase.UpdateUserPermissions(ID, ParentDirectoryId, AdminId, Identity);
        }

        public DMSTenantDocumentsDetails GetTenantDocumentsDetailByTenantId(string tenantId)
        {
            return _dmsManagerBase.GetTenantDocumentsDetailByTenantId(tenantId);
        }

        public void DeleteDocuments(List<int> fileIds)
        {
            _dmsManagerBase.DeleteDocuments(fileIds);
        }

        public bool DeleteDocumentByfileId(int fileId)
        {
           return( _dmsManagerBase.DeleteDocumentByfileId(fileId));
        }

        public void DeleteDirectoryRecursive(int directoryId)
        {
            _dmsManagerBase.DeleteDirectoryRecursive(directoryId);
        }

        /// <summary>
        /// Recursive copy for Directory, Sub Directories, and files with in Directories (eg., copy Directories from OPM to CPR).
        /// </summary>        
        public void CopyDMSDirectory(string SourceTicketId, string TargetTicketId)
        {
            _dmsManagerBase.CopyDMSDirectory(SourceTicketId, TargetTicketId);
        }

        public List<DMSDocument> GetFileListByFileId(string fileId)
        {
            return _dmsManagerBase.GetFileListByFileId(fileId);
        }

        public void UpdateHistory(UserProfile user, string TicketId, string HistoryDescription)
        {
            _dmsManagerBase.UpdateHistory(user, TicketId, HistoryDescription);
        }

        public List<DMSDocument> GetFilesByIds(List<int> Ids)
        {
            return _dmsManagerBase.GetFilesByIds(Ids);
        }
    }
}
