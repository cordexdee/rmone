using System.Collections.Generic;
using System.Web;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DMSDB;

namespace uGovernIT.Manager
{
    public interface IDMSManagerBase
    {
        DMSDirectory GetDirectory(int dirId);

        List<DMSDirectory> GetChildDirectories(int id);
        List<DMSDirectory> GetUserRepoDirectories(string userGuid);
        List<DMSDirectory> GetChildDirectoriesByDirectName(string directoryName);
        ICollection<DMSDirectory> BuildRecursiveDirectories(ICollection<DMSDirectory> resultList);

        List<int?> BuildRecursiveDirectoriesHierarchy(string userGuid);
        List<int?> UserFilesList(string userGuid);
        List<int?> FilesDirectoryId(List<int?> fileIdList);

        void SaveDirectory(DMSDirectory directory);
        void DeleteDirectoryRecursive(int directoryId);

        void SaveDocument(DMSDocument doc);
        int UpdateDocument(DMSDocument doc);
        void DeleteDocuments(List<int> fileIds);
        bool DeleteDocumentByfileId(int fileId);

        int UndoCheckout(int FileId, string userId);
        void ResetCheckout(List<DMSDocument> listFileDetails);

        List<DMSDocument> GetFilesWithName(int directoryId, string fileName, string fileVersion);
        List<DMSDocument> GetVersion(int Id);
        List<DMSDocument> GetMainFilesByDirId(int dirId);
        List<DMSDocument> GetUserRepoDocuments(string userGuid, List<int> directoryIds);
        List<DMSDocument> GetFileListByFileId(string fileId);

        DMSDocument GetDocumentVersions(List<DMSDocument> listFileDetails, string fileVersion);
        DMSDocument GetLatestCheckoutFileByUserIdAndId(string userId, int Id);
        List<DMSDocument> Upload(int DirectoryId, string fileVersion, string userId, HttpPostedFileBase[] files);

        DMSDirectory GetUserRepoDirectory(string userGuid, string folderName);
        DMSDirectory CreateNewFolder(string folderName, int DirectoryId, string userId, string Owners);
 

        TreeViewModel PopulateCurrentUserDirectory(TreeViewModel treeViewModelCurrent, ICollection<DMSDirectory> resultList, ICollection<DMSDocument> resultFileList, List<int?> finalDirectoryIdList, List<int?> userFilesList);
        TreeViewModel PopulateCurrentDirectory(TreeViewModel treeViewModelCurrent, ICollection<DMSDirectory> resultDirectoryList, ICollection<DMSDocument> resultFileList);
        string BuildPath(int? directoryId);

        List<FilesDirectories> SelectedDirectoryFileData(int id, bool isFolder, string roleName, string userId);

        StatusModel UploadCheckIn(int Id, string fileVersion, string userId, HttpPostedFileBase[] files);
        StatusModel Checkout(int FileId, string userIdGuid, string roleName);
        StatusModel DownloadLatestFile(int fileId, string userIdGuid);

        void DownloadFile(int fileId);
        bool IsCheckOut(int fileId);

        List<string> ListVersions(List<DMSDocument> currentDocument);
        List<string> GetDirectoryNamesList(int DirectoryId);

        bool hasUserWriteAccess(int fileId, string userId, string roleName);
        void UpdateUserPermissions(int ID, int ParentDirectoryId, string AdminId, string Identity);

        DMSTenantDocumentsDetails GetTenantDocumentsDetailByTenantId(string tenantId);
        void CopyDMSDirectory(string sourceTicketId, string targetTicketId);

        void UpdateHistory(UserProfile user, string TicketId, string HistoryDescription);
        List<DMSDocument> GetFilesByIds(List<int> ids);
        void CreatePortalForTicketElevated(ApplicationContext _applicationContext, string ticketId, string _userId, string ownerUser, string chekDefaultFolders);
    }
}
