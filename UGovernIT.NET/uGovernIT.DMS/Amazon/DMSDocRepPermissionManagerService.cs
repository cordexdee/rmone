using System.Collections.Generic;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DMSDB;

namespace uGovernIT.DMS.Amazon
{
    public class DMSDocRepPermissionManagerService
    {
        private ApplicationContext _applicationContext;
        private IDMSDocRepPermissionManagerBase _DMSDocRepPermissionManagerBase;

        public DMSDocRepPermissionManagerService(ApplicationContext context)
        {
            _applicationContext = context;
            _DMSDocRepPermissionManagerBase = new DMSDocRepPermissionManagerBase(_applicationContext);
        }

        public List<UserProfile> SearchUser(string Prefix)
        {
            return _DMSDocRepPermissionManagerBase.SearchUser(Prefix);
        }

        public int PerformUpdateAccessToHierarchy(List<DMSUsersFilesAuthorization> usersFilesAuthorization)
        {
            return _DMSDocRepPermissionManagerBase.PerformUpdateAccessToHierarchy(usersFilesAuthorization);
        }

        public ICollection<FilesDirectories> CompleteFilesAndDirectoriesList(string userId)
        {
            return _DMSDocRepPermissionManagerBase.CompleteFilesAndDirectoriesList(userId);
        }

        public void SetUserAccessDetails(List<UserAccessDetails> userAccessDetails, ICollection<FilesDirectories> filesDirectoryDetails)
        {
            _DMSDocRepPermissionManagerBase.SetUserAccessDetails(userAccessDetails, filesDirectoryDetails);
        }

        public List<DMSUsersFilesAuthorization> UpdateReadAccessDetails(string userIdLogin, string userID, List<FilesDirectories> readDetailsList, List<DMSUsersFilesAuthorization> userReadAccessDetailsToUpdate)
        {
            return _DMSDocRepPermissionManagerBase.UpdateReadAccessDetails(userIdLogin, userID, readDetailsList, userReadAccessDetailsToUpdate);
        }

        public void UpdateWriteAccessDetails(string userIdLogin, string userID, List<FilesDirectories> writeDetailsList,
            List<DMSUsersFilesAuthorization> userWriteAccessDetailsToUpdate, List<DMSUsersFilesAuthorization> existingUserAccessDetailsToDelete)
        {
            _DMSDocRepPermissionManagerBase.UpdateWriteAccessDetails(userIdLogin, userID, writeDetailsList,
            userWriteAccessDetailsToUpdate, existingUserAccessDetailsToDelete);
        }

        public void PerformUserAccessDetailUpdates(List<DMSUsersFilesAuthorization> userReadAccessDetailsToUpdate, List<DMSUsersFilesAuthorization> existingUserAccessDetailsToDelete, List<DMSUsersFilesAuthorization> userWriteAccessDetailsToUpdate)
        {
            _DMSDocRepPermissionManagerBase.PerformUserAccessDetailUpdates(userReadAccessDetailsToUpdate, existingUserAccessDetailsToDelete, userWriteAccessDetailsToUpdate);
        }

        public int UpdateCompleteAccessToHierarchy(string userId, List<DMSUsersFilesAuthorization> existingUserAccessDetailsToDelete, bool isDelete)
        {
            return _DMSDocRepPermissionManagerBase.UpdateCompleteAccessToHierarchy(userId, existingUserAccessDetailsToDelete, isDelete);
        }

        public List<DMSUsersFilesAuthorization> GetExistingAccessDetails(string userID)
        {
            return _DMSDocRepPermissionManagerBase.GetExistingAccessDetails(userID);
        }
    }
}
