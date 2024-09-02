using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using uGovernIT.DAL.Infratructure;
using uGovernIT.Utility.Entities.DMSDB;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using uGovernIT.Utility.Entities;

namespace uGovernIT.DAL
{
    public class DocRepoPermissionBase : IDocRepoPermission
    {

        protected string tableName;
        protected CustomDbContext context;


        public DocRepoPermissionBase(CustomDbContext context, string tableName = "")
        {
            this.context = context;
            this.tableName = tableName;

            TableAttribute tAttr = Attribute.GetCustomAttribute(typeof(DocRepositoryBase), typeof(TableAttribute)) as TableAttribute;
            if (tAttr != null)
            {
                this.tableName = tAttr.Name;
            }

        }

        public virtual List<UserProfile> searchUser(string Prefix)
        {
            List<UserProfile> userInfo = null;

            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                //userInfo = (from u in ctx.UserProfile
                //            where u.TenantID.Equals(context.TenantID.Trim())
                //            select u).ToList(); 

                userInfo = (from u in ctx.UserProfile 
                            where u.TenantID.Equals(context.TenantID.Trim()) && u.UserName.Equals(Prefix.Trim())
                            select u).ToList();
            }
            return userInfo;
        }

        public virtual int PerformUpdateAccessToHierarchy(List<DMSUsersFilesAuthorization> usersFilesAuthorization)
        {
            var result = 0;
            foreach (var item in usersFilesAuthorization)
            {
                using (DatabaseContext ctx = new DatabaseContext(context))
                {
                    SqlParameter sqlDirectoryId = new SqlParameter("@ID", item.DirectoryId);
                    SqlParameter sqlUserId = new SqlParameter("@UID", item.UserId);
                    SqlParameter sqlAccessId = new SqlParameter("@AccessID", item.AccessId);
                    SqlParameter sqlCustomerId = new SqlParameter("@CustomerID", item.CustomerId);
                    SqlParameter sqlCreatedBy = new SqlParameter("@CreatedByUser", item.CreatedByUser);
                    SqlParameter sqlUpdatedBy = new SqlParameter("@UpdatedBy", item.UpdatedBy);

                    result = ctx.Database.ExecuteSqlCommand("spAddDirectoryChildrenAccess @ID, @UID,@AccessID,@CustomerID,@CreatedByUser,@UpdatedBy",
                                             sqlDirectoryId, sqlUserId, sqlAccessId, sqlCustomerId, sqlCreatedBy, sqlUpdatedBy);
                }
            }
            return result;
        }

        public virtual ICollection<FilesDirectories> CompleteFilesAndDirectoriesList(string userId)
        {


            var listOfFilesFolders = new List<FilesDirectories>();

            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                var fileList = from file in ctx.DMSDocument
                               join user in ctx.UserProfile on file.AuthorId equals user.Id
                               where file.FileParentId == 0 && user.TenantID == context.TenantID
                               select new FilesDirectories
                               {
                                   Id = file.FileId,
                                   Name = file.FileName,
                                   FullPath = file.FullPath,
                                   ParentID = file.DirectoryId,
                                   Version = file.Version,
                                   CreatedOn = file.CreatedOn,
                                   UpdatdedOn = file.UpdatedOn,
                                   IsFile = true,
                                   IsFolder = false,
                               };

                listOfFilesFolders.AddRange(fileList);

                var directories = (from directory in ctx.DMSDirectories
                                   join user in ctx.UserProfile on directory.AuthorId equals user.Id
                                   where  user.TenantID == context.TenantID
                                   select directory).ToList();

                foreach (var i in directories)
                {
                    var directory = new FilesDirectories
                    {
                        Id = i.DirectoryId,
                        Name = i.DirectoryName,
                        FullPath = BuildPath(i.DirectoryId, i),
                        CreatedOn = i.CreatedOn,
                        UpdatdedOn = i.UpdatedOn,
                        IsFile = false,
                        IsFolder = true,
                    };

                    listOfFilesFolders.Add(directory);
                }
            }
            return listOfFilesFolders;
        }

        public virtual List<DMSUsersFilesAuthorization> GetExistingAccessDetails(string userID)
        {
            List<DMSUsersFilesAuthorization> usersFilesAuthorization = null;
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                {
                    usersFilesAuthorization = (from item in ctx.DMSUsersFilesAuthorization
                                               where item.UserId == userID && item.FileId == null
                                               select item).ToList();
                }
            }
            return usersFilesAuthorization;
        }

        public virtual List<DMSUsersFilesAuthorization> UpdateReadAccessDetails(string userID)
        {
            List<DMSUsersFilesAuthorization> usersFilesAuthorization = null;
            using (DatabaseContext ctx = new DatabaseContext(context))
            {

                usersFilesAuthorization = (from item in ctx.DMSUsersFilesAuthorization
                                           where item.UserId == userID && item.AccessId == 1
                                           select item).ToList();
            }
            return usersFilesAuthorization;
        }

        public virtual void UpdateWriteAccessDetails(string userIdLogin, string userID, List<FilesDirectories> writeDetailsList,
            List<DMSUsersFilesAuthorization> userWriteAccessDetailsToUpdate, List<DMSUsersFilesAuthorization> existingUserAccessDetailsToDelete)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {

                var localDate = System.DateTime.Now;
                var existingUserAccessDetails = (from item in ctx.DMSUsersFilesAuthorization
                                                 where item.UserId == userID && item.AccessId == 2
                                                 select item).ToList();

                var fileList = from item in writeDetailsList
                               where item.IsFile
                               select new DMSUsersFilesAuthorization
                               {
                                   AccessId = 2,
                                   CustomerId = 1,
                                   UserId = userID,
                                   CreatedByUser = userIdLogin,
                                   UpdatedBy = userIdLogin,
                                   CreatedOn = localDate,
                                   UpdatedOn = localDate,
                                   FileId = item.Id
                               };
                var directoryList = from item in writeDetailsList
                                    where item.IsFile == false
                                    select new DMSUsersFilesAuthorization
                                    {
                                        AccessId = 2,
                                        CustomerId = 1,
                                        UserId = userID,
                                        CreatedByUser = userIdLogin,
                                        UpdatedBy = userIdLogin,
                                        CreatedOn = localDate,
                                        UpdatedOn = localDate,
                                        DirectoryId = item.Id
                                    };

                userWriteAccessDetailsToUpdate.AddRange(fileList);
                userWriteAccessDetailsToUpdate.AddRange(directoryList);

                //Records to delete
                var deleteFileRecords = existingUserAccessDetails.Except(userWriteAccessDetailsToUpdate).ToList();
                existingUserAccessDetailsToDelete.AddRange(deleteFileRecords);
            }
        }

        public virtual void PerformUserAccessDetailUpdates(List<DMSUsersFilesAuthorization> userReadAccessDetailsToUpdate, List<DMSUsersFilesAuthorization> existingUserAccessDetailsToDelete, List<DMSUsersFilesAuthorization> userWriteAccessDetailsToUpdate)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                // Deletion
                existingUserAccessDetailsToDelete.ForEach(x => ctx.Entry(x).State = (Microsoft.EntityFrameworkCore.EntityState)Microsoft.EntityFrameworkCore.EntityState.Deleted);
                ctx.SaveChanges();

                foreach (var item in userReadAccessDetailsToUpdate)
                {
                    var read = ctx.DMSUsersFilesAuthorization.Any(t => t.UserId == item.UserId && t.AccessId == 1
                                                    && (t.FileId == null && t.DirectoryId == item.DirectoryId || t.FileId == item.FileId));
                    if (!read)
                    {
                        ctx.DMSUsersFilesAuthorization.Add(item);
                    }
                }
                foreach (var item in userWriteAccessDetailsToUpdate)
                {
                    var write = ctx.DMSUsersFilesAuthorization.Any(t => t.UserId == item.UserId && t.AccessId == 2
                                                    && (t.FileId == null && t.DirectoryId == item.DirectoryId || t.FileId == item.FileId));
                    if (!write)
                    {
                        ctx.DMSUsersFilesAuthorization.Add(item);
                    }
                }
                ctx.SaveChanges();
            }
        }

        public virtual int UpdateCompleteAccessToHierarchy(string userId, List<DMSUsersFilesAuthorization> existingUserAccessDetailsToDelete, bool isDelete)
        {

            var result = 0;
            //var UserId = Guid.Parse(userId);
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                if (isDelete)
                {
                    result = DeleteDirectoryPermissionsHierarchy(userId, existingUserAccessDetailsToDelete);
                }
                else
                {
                    var detailsList = (from userAccessTable in ctx.DMSUsersFilesAuthorization
                                       where userAccessTable.UserId == userId && userAccessTable.FileId == null
                                       select userAccessTable).ToList();

                    result = PerformUpdateAccessToHierarchy(detailsList);
                }
            }
            return result;
        }

        public virtual  List<UserAccessDetails> getUserAccess(string userID)
        {
            List<UserAccessDetails> userFileAuthorizationList = null;
            using (DatabaseContext ctx = new DatabaseContext(context))
            {

                userFileAuthorizationList = (from access in ctx.DMSUsersFilesAuthorization
                                           join user in ctx.UserProfile on access.UserId equals user.Id
                                             where access.UserId == userID //&& user.TenantID == context.TenantID
                                             select new UserAccessDetails
                                             {
                                                 fileId = access.FileId != null ? access.FileId : null,
                                                 directoryId = access.DirectoryId,
                                                 readAccess = access.AccessId == 1 ? 1 : 0,
                                                 writeAccess = access.AccessId == 2 ? 2 : 0,
                                             }).ToList();
            }
            return userFileAuthorizationList;
        }

        private int DeleteDirectoryPermissionsHierarchy(string userID, List<DMSUsersFilesAuthorization> userPermissionList)
        {

            // var userID = Guid.Parse(UID);
            var result = 0;

            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                var latestDetailsList = (from latestDetails in ctx.DMSUsersFilesAuthorization
                                         where latestDetails.UserId == userID && latestDetails.FileId == null
                                         select latestDetails).ToList();

                var existingUserAccessDetailsToDelete = userPermissionList.Where(item => !latestDetailsList.Any(item2 => item2.DirectoryId == item.DirectoryId) || !latestDetailsList.Any(item2 => item2.DirectoryId == item.DirectoryId && item2.AccessId == item.AccessId)).ToList();
                foreach (var item in existingUserAccessDetailsToDelete)
                {

                    SqlParameter sqlDirectoryId = new SqlParameter("@ID", item.DirectoryId);
                    SqlParameter sqlUserId = new SqlParameter("@UID", item.UserId);
                    SqlParameter sqlAccessId = new SqlParameter("@AccessID", item.AccessId);
                    SqlParameter sqlCustomerId = new SqlParameter("@CustomerID", item.CustomerId);
                    SqlParameter sqlCreatedBy = new SqlParameter("@CreatedByUser", item.CreatedByUser);
                    SqlParameter sqlUpdatedBy = new SqlParameter("@UpdatedBy", item.UpdatedBy);

                    result = ctx.Database.ExecuteSqlCommand("spDeleteDirectoryChildrenAccess @ID, @UID,@AccessID,@CustomerID,@CreatedByUser,@UpdatedBy",
                                             sqlDirectoryId, sqlUserId, sqlAccessId, sqlCustomerId, sqlCreatedBy, sqlUpdatedBy);
                }
            }
            return result;
        }

        private string BuildPath(int? DirectoryId, DMSDirectory directories)
        {
            var title = "";
         //   while (DirectoryId != 0)
            {
                var path = getPath(DirectoryId, directories);
                //if (path == null) break;
                title = path.DirectoryName + "/" + title;
                DirectoryId = path.DirectoryParentId;
            }
            var relativePath = "ALL/"+ context.TenantAccountId + "/" + title;
            return relativePath;
        }

        private DMSDirectory getPath(int? directroryid, DMSDirectory directories)
        {
            return directories;
            //return (from doc in directories where doc.DirectoryId == directroryid select doc).FirstOrDefault();
        }
    }
}
