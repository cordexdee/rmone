using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevExpress.DataAccess.Native.Data;
using uGovernIT.DAL;
using uGovernIT.Utility.Entities.DMSDB;
using uGovernIT.Utility.Entities;
using System.Web;
using System.IO;
using uGovernIT.Manager.DMSOperations;

namespace uGovernIT.Manager
{
    public  class DMSDocRepPermissionManagerBase : IDMSDocRepPermissionManagerBase
    {
        protected ApplicationContext dbContext;

        protected IDocRepoPermission docRepoPermission;

        public DMSDocRepPermissionManagerBase(ApplicationContext context)
        {
            dbContext = context;
            docRepoPermission = new DocRepoPermissionBase(dbContext);

        }
        public virtual List<UserProfile> SearchUser(string Prefix)
        {
            return docRepoPermission.searchUser(Prefix);
        }

        public virtual int PerformUpdateAccessToHierarchy(List<DMSUsersFilesAuthorization> usersFilesAuthorization)
        {
            return docRepoPermission.PerformUpdateAccessToHierarchy(usersFilesAuthorization);
        }

        public virtual ICollection<FilesDirectories> CompleteFilesAndDirectoriesList(string userId)
        {
            ICollection<FilesDirectories> ret = docRepoPermission.CompleteFilesAndDirectoriesList(userId);
            //Guid userID = new Guid();
            //var isGuid = Guid.TryParse(userId, out userID);
            if (!string.IsNullOrEmpty(userId))
            {
                List<UserAccessDetails> userDetailsListForFilesAndDirectories = docRepoPermission.getUserAccess(userId);
                SetUserAccessDetails(userDetailsListForFilesAndDirectories, ret);
            }
            return ret;
        }

        public virtual void SetUserAccessDetails(List<UserAccessDetails> userAccessDetails, ICollection<FilesDirectories> filesDirectoryDetails)
        {
            foreach (var accessDetailItem in userAccessDetails)
            {
                foreach (var item in filesDirectoryDetails)
                {
                    if (item.Id == accessDetailItem.directoryId && item.IsFolder == true && accessDetailItem.fileId == null)
                    {
                        if (!item.readAccess)
                        {
                            item.readAccess = accessDetailItem.readAccess == 1 ? true : false;
                        }
                        if (!item.writeAccess)
                        {
                            item.writeAccess = accessDetailItem.writeAccess == 2 ? true : false;
                        }
                    }
                    else if (item.Id == accessDetailItem.fileId)
                    {
                        if (!item.readAccess)
                        {
                            item.readAccess = accessDetailItem.readAccess == 1 ? true : false;
                        }
                        if (!item.writeAccess)
                        {
                            item.writeAccess = accessDetailItem.writeAccess == 2 ? true : false;
                        }

                    }
                }
            }
        }

        public virtual List<DMSUsersFilesAuthorization> UpdateReadAccessDetails(string userIdLogin, string userID, List<FilesDirectories> readDetailsList, List<DMSUsersFilesAuthorization> userReadAccessDetailsToUpdate)
        {
            DateTime localDate = System.DateTime.Now;

            List<DMSUsersFilesAuthorization> existingUserAccessDetails = docRepoPermission.UpdateReadAccessDetails(userID);

            foreach (var readItem in readDetailsList)
            {
                var usersAccessDetails = new DMSUsersFilesAuthorization
                {
                    AccessId = 1,
                    CustomerId = 1,
                    UserId = userID,
                    CreatedByUser = userID,
                    UpdatedBy = userID,
                    CreatedOn = localDate,
                    UpdatedOn = localDate,
                };

                if (readItem.IsFile)
                {
                    usersAccessDetails.FileId = readItem.Id;
                    userReadAccessDetailsToUpdate.Add(usersAccessDetails);
                }
                else
                {
                    usersAccessDetails.DirectoryId = readItem.Id;
                    userReadAccessDetailsToUpdate.Add(usersAccessDetails);
                }
            }

            var existingUserAccessDetailsToDelete = existingUserAccessDetails.Except(userReadAccessDetailsToUpdate).ToList();

            return existingUserAccessDetailsToDelete;
        }

        public virtual void UpdateWriteAccessDetails(string userIdLogin, string userID, List<FilesDirectories> writeDetailsList,
            List<DMSUsersFilesAuthorization> userWriteAccessDetailsToUpdate, List<DMSUsersFilesAuthorization> existingUserAccessDetailsToDelete)
        {
            docRepoPermission.UpdateWriteAccessDetails(userIdLogin, userID, writeDetailsList,
            userWriteAccessDetailsToUpdate, existingUserAccessDetailsToDelete);
        }

        public virtual void PerformUserAccessDetailUpdates(List<DMSUsersFilesAuthorization> userReadAccessDetailsToUpdate, List<DMSUsersFilesAuthorization> existingUserAccessDetailsToDelete, List<DMSUsersFilesAuthorization> userWriteAccessDetailsToUpdate)
        {
            docRepoPermission.PerformUserAccessDetailUpdates(userReadAccessDetailsToUpdate, existingUserAccessDetailsToDelete, userWriteAccessDetailsToUpdate);
        }

        public virtual int UpdateCompleteAccessToHierarchy(string userId, List<DMSUsersFilesAuthorization> existingUserAccessDetailsToDelete, bool isDelete)
        {
            return docRepoPermission.UpdateCompleteAccessToHierarchy(userId, existingUserAccessDetailsToDelete, isDelete);
        }

        public List<DMSUsersFilesAuthorization> GetExistingAccessDetails(string userID)
        {
            return docRepoPermission.GetExistingAccessDetails(userID);
        }
    }
}
