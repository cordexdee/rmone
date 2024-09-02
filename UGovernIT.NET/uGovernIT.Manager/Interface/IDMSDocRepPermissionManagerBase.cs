using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities.DMSDB;
using uGovernIT.Utility.Entities;
using System.Web;
using System.IO;

namespace uGovernIT.Manager
{
   public interface IDMSDocRepPermissionManagerBase
    {
        List<UserProfile> SearchUser(string Prefix);
        int PerformUpdateAccessToHierarchy(List<DMSUsersFilesAuthorization> usersFilesAuthorization);
        ICollection<FilesDirectories> CompleteFilesAndDirectoriesList(string userId);
        void SetUserAccessDetails(List<UserAccessDetails> userAccessDetails, ICollection<FilesDirectories> filesDirectoryDetails);
        List<DMSUsersFilesAuthorization> UpdateReadAccessDetails(string userIdLogin, string userID, List<FilesDirectories> readDetailsList, List<DMSUsersFilesAuthorization> userReadAccessDetailsToUpdate);
        void UpdateWriteAccessDetails(string userIdLogin, string userID, List<FilesDirectories> writeDetailsList,
            List<DMSUsersFilesAuthorization> userWriteAccessDetailsToUpdate, List<DMSUsersFilesAuthorization> existingUserAccessDetailsToDelete);
        void PerformUserAccessDetailUpdates(List<DMSUsersFilesAuthorization> userReadAccessDetailsToUpdate, List<DMSUsersFilesAuthorization> existingUserAccessDetailsToDelete, List<DMSUsersFilesAuthorization> userWriteAccessDetailsToUpdate);
        int UpdateCompleteAccessToHierarchy(string userId, List<DMSUsersFilesAuthorization> existingUserAccessDetailsToDelete, bool isDelete);
        List<DMSUsersFilesAuthorization> GetExistingAccessDetails(string userID);
    }
}
