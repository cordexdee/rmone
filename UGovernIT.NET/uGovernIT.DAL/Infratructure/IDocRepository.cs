using System.Collections.Generic;
using uGovernIT.Utility.Entities.DMSDB;

namespace uGovernIT.DAL
{
    public interface IDocRepository
    {
        List<DMSDirectory> GetChildDirectories(int id);
        List<DMSDirectory> GetUserRepoDirectories(string userGuid);
        List<DMSDirectory> GetChildDirectoriesByDirectName(string directoryName);
        DMSDirectory GetUserRepoDirectory(string userGuid, string folderName);

        List<int?> UserAccessedDirectoriesHierarchy(string userGuid);
        List<int?> UserAccessedDirectories(string userGuid);
        List<int?> UserFilesId(string userGuid);
        List<int?> FilesParentDirectoryId(List<int?> accessedDirectoryList);

        List<DMSDocument> GetUserRepoDocuments(string userGuid, List<int> directoryIds);
        List<DMSDocument> GetFilesWithName(int directoryId, string fileName, string fileVersion);
        List<DMSDocument> GetFileListByFileId(string fileID);
        List<DMSDocument> GetVersion(int Id);
        List<DMSDocument> GetMainFilesByDirId(int dirId);

        List<FilesDirectories> SelectedDirectoryFileData(int id, bool isFolder, string roleName, string userId);

        List<string> ListVersions(List<DMSDocument> currentDocument);
        List<string> DirectoryNamesList(int DirectoryId);
        //List<int?> BuildDirectoryHierarchy(int? id, List<int?> idList);
        //List<DMSDocument> UserDirectoryFilesList(int id, List<FilesDirectories> folderFileData, string userId);

        List<DMSDirectory> GetDuplicateDirectories(int directoryId, string folderName);

        ICollection<DMSDirectory> BuildRecursiveDirectories(ICollection<DMSDirectory> resultList);
        TreeViewModel PopulateCurrentUserDirectory(TreeViewModel treeViewModelCurrent, ICollection<DMSDirectory> resultList, ICollection<DMSDocument> resultFileList, List<int?> finalDirectoryIdList, List<int?> userFilesList);
        TreeViewModel PopulateCurrentDirectory(TreeViewModel treeViewModelCurrent, ICollection<DMSDirectory> resultList, ICollection<DMSDocument> resultFileList);

        DMSDocument GetFilesById(int Id);
        DMSDocument GetLatestCheckoutFileByUserIdAndId(string userId, int Id);
        DMSDocument GetDocumentVersions(List<DMSDocument> listFileDetails, string fileVersion);

        DMSDirectory GetPathByDirectoryId(int? directoryId);
        DMSDirectory GetDirectory(int dirId);

        void UpdateDirectory(DMSDirectory directory);
        void SaveDirectory(DMSDirectory directory);
        void DeleteDirectoryRecursive(int directoryId);

        void SaveDocument(DMSDocument doc);
        void ResetCheckout(List<DMSDocument> listFileDetails);

        void UpdateUserDirectoryFilePermissions(int ID, int ParentDirectoryId, string userId, string Identity);
        void SaveUserFileAuthorization(DMSUsersFilesAuthorization userAuhtorizedData);

        void DeleteDocuments(List<int> fileIds);
        bool DeleteDocumentByfileId(int fileId);
        int UpdateDocument(DMSDocument doc);
        int LogDocumentsDownloaded(int FileId, string userIdGuid);

        bool UserFilesId(string userGuid, int fileId);
        bool IsCheckOut(int fileId);
        bool HasCheckInAccess(string userId, int fileId);

        DMSTenantDocumentsDetails GetTenantDocumentsDetailByTenantId(string tenantId);
        List<DMSDocument> GetFilesByIds(List<int> Id);
    }
}
