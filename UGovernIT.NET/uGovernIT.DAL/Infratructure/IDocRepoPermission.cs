using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DMSDB;

namespace uGovernIT.DAL
{
   public interface IDocRepoPermission
    {
        List<UserProfile> searchUser(string Prefix);
        int PerformUpdateAccessToHierarchy(List<DMSUsersFilesAuthorization> usersFilesAuthorization);
        ICollection<FilesDirectories> CompleteFilesAndDirectoriesList(string userId);
        List<DMSUsersFilesAuthorization> GetExistingAccessDetails(string userID);
        List<DMSUsersFilesAuthorization> UpdateReadAccessDetails(string userID);
        void UpdateWriteAccessDetails(string userIdLogin, string userID, List<FilesDirectories> writeDetailsList,
            List<DMSUsersFilesAuthorization> userWriteAccessDetailsToUpdate, List<DMSUsersFilesAuthorization> existingUserAccessDetailsToDelete);

        void PerformUserAccessDetailUpdates(List<DMSUsersFilesAuthorization> userReadAccessDetailsToUpdate, List<DMSUsersFilesAuthorization> existingUserAccessDetailsToDelete, List<DMSUsersFilesAuthorization> userWriteAccessDetailsToUpdate);
        int UpdateCompleteAccessToHierarchy(string userId, List<DMSUsersFilesAuthorization> existingUserAccessDetailsToDelete, bool isDelete);
        List<UserAccessDetails> getUserAccess(string userID);
    }
}
