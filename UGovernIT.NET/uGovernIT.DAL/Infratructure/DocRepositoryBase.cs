using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using uGovernIT.DAL.Infratructure;
using uGovernIT.Utility.Entities.DMSDB;
using Microsoft.EntityFrameworkCore;
using uGovernIT.Util.Log;

namespace uGovernIT.DAL
{
    public class DocRepositoryBase : IDocRepository
    {
        protected string tableName;
        protected CustomDbContext context;

        public DocRepositoryBase(CustomDbContext context, string tableName = "")
        {
            this.context = context;
            this.tableName = tableName;

            TableAttribute tAttr = Attribute.GetCustomAttribute(typeof(DocRepositoryBase), typeof(TableAttribute)) as TableAttribute;
            if (tAttr != null)
            {
                this.tableName = tAttr.Name;
            }

        }

        public virtual List<DMSDirectory> GetChildDirectories(int id)
        {
            List<DMSDirectory> directories = null;
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                directories = (from itemDirectory in ctx.DMSDirectories
                               join user in ctx.UserProfile on itemDirectory.AuthorId equals user.Id
                               where itemDirectory.DirectoryParentId == id && (context.TenantID == user.TenantID || context.TenantID == itemDirectory.TenantID) && itemDirectory.Deleted == false
                               select itemDirectory).ToList();
            }
            return directories;
        }

        //public virtual List<DMSDirectory> GetUserRepoDirectories(string userGuid)
        //{
        //    List<DMSDirectory> directories = null;
        //    using (DatabaseContext ctx = new DatabaseContext(context))
        //    {
        //        directories = (from direct in ctx.DMSDirectories
        //                       where direct.DirectoryParentId == 0
        //                       select direct).ToList();
        //    }
        //    return directories;
        //}

        public virtual List<DMSDirectory> GetUserRepoDirectories(string userGuid)
        {
            List<DMSDirectory> directories = null;

            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                directories = (from direct in ctx.DMSDirectories
                               join user in ctx.UserProfile on direct.AuthorId equals user.Id
                               where direct.DirectoryParentId == 0 && user.TenantID == context.TenantID
                               select direct).ToList();
            }
            return directories;
        }

        public virtual List<DMSDirectory> GetChildDirectoriesByDirectName(string directoryName)
        {
            List<DMSDirectory> directories = null;

            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                directories = (from parent in ctx.DMSDirectories
                               join child in ctx.DMSDirectories on parent.DirectoryId equals child.DirectoryParentId
                               where parent.DirectoryName.Equals(directoryName, StringComparison.InvariantCultureIgnoreCase)
                               select child).ToList();
            }
            return directories;
        }

        public virtual DMSDirectory GetUserRepoDirectory(string userGuid, string folderName)
        {
            DMSDirectory directory = null;

            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                directory = (from direct in ctx.DMSDirectories
                             join user in ctx.UserProfile on direct.AuthorId equals user.Id
                             where direct.DirectoryParentId == 0 && (user.TenantID == context.TenantID  || direct.TenantID == context.TenantID)
                                  && direct.DirectoryName.Equals(folderName, StringComparison.InvariantCultureIgnoreCase)
                                  && direct.Deleted == false
                             select direct).FirstOrDefault();
            }
            return directory;
        }

        public virtual List<int?> UserAccessedDirectoriesHierarchy(string userGuid)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                var accessedDirectoryList = UserAccessedDirectories(userGuid);
                var idList = new List<int?>();
                var hierarchyDirectoryList = accessedDirectoryList.SelectMany(x => BuildDirectoryHierarchy(x.Value, idList)).Distinct().ToList();
                return hierarchyDirectoryList;
            }
        }

        public virtual List<int?> UserAccessedDirectories(string userGuid)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                return (from d in ctx.DMSUsersFilesAuthorization
                        where d.UserId.ToString() == userGuid && d.FileId == null
                        select d.DirectoryId).Distinct().ToList();
            }
        }

        private List<int?> BuildDirectoryHierarchy(int? id, List<int?> idList)
        {
            idList.Add(id);
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                var data = ctx.DMSDirectories.Where(x => x.DirectoryId == id).ToList();
                if (data.Count > 0 && data[0].DirectoryParentId != 0)
                {
                    BuildDirectoryHierarchy(data[0].DirectoryParentId, idList);
                }
            }
            return idList.Distinct().ToList();
        }

        public virtual List<int?> UserFilesId(string userGuid)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                return (from d in ctx.DMSUsersFilesAuthorization
                        where d.UserId.ToString() == userGuid && d.FileId != null
                        select d.FileId).Distinct().ToList();
            }
        }

        public virtual List<int?> FilesParentDirectoryId(List<int?> accessedDirectoryList)
        {
            List<int?> directoryIdList = new List<int?>();
            using (DatabaseContext ctx = new DatabaseContext(context))
            {

                foreach (var item in accessedDirectoryList)
                {
                    var fileDirectoryIdList = (from d in ctx.DMSDocument
                                               where d.FileId == item
                                               select d.DirectoryId
                                               ).ToList();
                    directoryIdList.Add(fileDirectoryIdList[0]);
                }
            }
            var distinctFileDirectoryId = directoryIdList.Distinct().ToList();
            List<int?> idList = new List<int?>();
            var hierarchyFileDirectoryList = distinctFileDirectoryId.SelectMany(x => BuildDirectoryHierarchy(x.Value, idList)).ToList();
            return hierarchyFileDirectoryList;
        }

        public virtual void SaveDirectory(DMSDirectory directory)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                ctx.DMSDirectories.Add(directory);
                ctx.SaveChanges();
            }
        }

        public virtual void UpdateDirectory(DMSDirectory directory)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                ctx.DMSDirectories.Update(directory);
                ctx.SaveChanges();
            }
        }

        public virtual void DeleteDirectoryRecursive(int directoryId)
        {
            var directory = GetDirectory(directoryId);

            if (directory == null) return;

            // delete child directories
            var directories = BuildRecursiveDirectories(new List<DMSDirectory>() { directory }).ToList();

            if (directories.Any())
            {
                var directoryIds = new List<int>();
                BuildRecursiveDirectory(directoryIds, directories);

                var fileIds = GetDocumentIds(directoryIds);

                // delete all files
                DeleteDocuments(fileIds);

                foreach (var id in directoryIds)
                {
                    // delete child directories
                    DeleteDirectory(id);
                }

                // delete root directory
                DeleteDirectory(directoryId);
            }
        }

        public virtual void BuildRecursiveDirectory(List<int> resultList, ICollection<DMSDirectory> directoryList)
        {
            if (resultList == null)
                return;

            foreach (var item in directoryList)
            {
                resultList.Add(item.DirectoryId);

                BuildRecursiveDirectory(resultList, item.DirectoryList);
            }
        }

        public virtual List<int> GetDocumentIds(List<int> directoryIds)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                return ctx.DMSDocument
                    .Where(x => x.FileParentId == 0 && directoryIds.Contains(x.DirectoryId))
                    .Select(x => x.FileId)
                    .ToList();
            }
        }

        public virtual void DeleteDirectory(int directoryId)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                var directory = ctx.DMSDirectories.FirstOrDefault(x => x.DirectoryId == directoryId);

                if (directory == null) return;

                directory.Deleted = true;
                directory.UpdatedOn = DateTime.Now;
                directory.UpdatedBy = context.CurrentUser.Id;

                ctx.SaveChanges();
            }
        }

        public virtual List<DMSDocument> GetUserRepoDocuments(string userGuid, List<int> directoryIds)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                return (from file in ctx.DMSDocument
                        join user in ctx.UserProfile on file.AuthorId equals user.Id
                        where file.FileParentId == 0 && user.TenantID == context.TenantID
                        select file).ToList();
            }
        }

        public virtual void SaveDocument(DMSDocument doc)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                using (var dbContextTransaction = ctx.Database.BeginTransaction())
                {
                    if (string.IsNullOrEmpty(doc.DocumentControlID))
                    {
                        var lastSequence = "";
                        var lastRecord = ctx.DMSDocument.Where(x => !string.IsNullOrEmpty(x.DocumentControlID)).OrderByDescending(p => p.FileId).FirstOrDefault();

                        if (lastRecord != null)
                            lastSequence = lastRecord.DocumentControlID;

                        var documentControlId = GetNextDocumentControlID(lastSequence);
                        doc.DocumentControlID = documentControlId;
                        doc.TenantID = context.TenantID;
                    }

                    ctx.DMSDocument.Add(doc);

                    ctx.SaveChanges();

                    dbContextTransaction.Commit();
                }
            }
        }

        public string GetNextDocumentControlID(string lastSequence)
        {
            int seqNum = 0;
            string ticketId = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(lastSequence))
                {
                    var charIndex = lastSequence.IndexOf("-");

                    if (charIndex > 0)
                        lastSequence = lastSequence.Substring(charIndex + 1);

                    // Get next sequence number
                    seqNum = int.Parse(lastSequence) + 1;
                }
            }
            catch (Exception)
            {
            }

            ticketId = DateTime.Now.ToString("yy") + "-" + seqNum.ToString(new string('0', 6));

            return ticketId;
        }

        public virtual int UpdateDocument(DMSDocument doc)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {

                ctx.DMSDocument.Add(doc);
                ctx.Entry(doc).State = (Microsoft.EntityFrameworkCore.EntityState)Microsoft.EntityFrameworkCore.EntityState.Modified;
                // existingUserAccessDetailsToDelete.ForEach(x => ctx.Entry(x).State = (Microsoft.EntityFrameworkCore.EntityState)Microsoft.EntityFrameworkCore.EntityState.Deleted);
                return ctx.SaveChanges();
            }
        }

        public virtual void DeleteDocuments(List<int> fileIds)
        {
            if (!fileIds.Any()) return;

            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                foreach (var fileId in fileIds)
                {
                    var document = ctx.DMSDocument.FirstOrDefault(x => x.FileId == fileId);

                    if (document == null) continue;

                    document.Deleted = true;
                    document.UpdatedOn = DateTime.Now;
                    document.UpdatedBy = context.CurrentUser.Id;

                    ctx.SaveChanges();
                }
            }
        }

        public virtual bool DeleteDocumentByfileId(int fileId)
        {
            //check Id present or not
            //if () return;
            try
            {
                using (DatabaseContext ctx = new DatabaseContext(context))
                {
                    var document = ctx.DMSDocument.FirstOrDefault(x => x.FileId == fileId);

                    document.Deleted = true;

                    document.UpdatedOn = DateTime.Now;

                    document.UpdatedBy = context.CurrentUser.Id;

                    ctx.SaveChanges();
                    return true;
                }

            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return false;
            }

        }


        public virtual List<DMSDocument> GetFilesWithName(int directoryId, string fileName, string fileVersion)
        {
            List<DMSDocument> documents = null;

            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                documents = (from itemFile in ctx.DMSDocument
                             where itemFile.DirectoryId == directoryId && itemFile.FileName.ToLower() == fileName.ToLower() && itemFile.Version.ToLower() == fileVersion.ToLower()
                             select itemFile).ToList();
            }

            return documents;
        }

        public DMSDocument GetFilesById(int Id)
        {
            DMSDocument objDocument = null;
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                objDocument = (from itemFile in ctx.DMSDocument
                               where itemFile.FileId == Id
                               orderby itemFile.UpdatedOn descending
                               select itemFile).FirstOrDefault();
            }
            return objDocument;
        }

        public virtual DMSDocument GetLatestCheckoutFileByUserIdAndId(string userId, int Id) // 
        {
            var documents = GetVersion(Id);
            return documents.Where(x => x.CheckOutBy == userId).FirstOrDefault();
        }

        public virtual List<DMSDocument> GetVersion(int Id)
        {
            List<DMSDocument> docVersion = null;
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                docVersion = (from itemFile in ctx.DMSDocument
                              where itemFile.FileId == Id || itemFile.FileParentId == Id
                              orderby itemFile.UpdatedOn descending
                              select itemFile).ToList();
            }
            return docVersion;
        }

        public virtual List<DMSDocument> GetMainFilesByDirId(int dirId)
        {
            List<DMSDocument> docMainFilesByDir = null;
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                docMainFilesByDir = (from itemFile in ctx.DMSDocument
                                     where itemFile.DirectoryId == dirId && itemFile.FileParentId == 0 && itemFile.Deleted == false
                                     select itemFile).ToList();
            }
            return docMainFilesByDir;
        }

        public virtual DMSDirectory GetDirectory(int dirId)
        {
            DMSDirectory directory = new DMSDirectory();

            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                directory = (from dir in ctx.DMSDirectories
                             where dir.DirectoryId == dirId
                             select dir)
                             .FirstOrDefault();
            }
            return directory;
        }

        public virtual List<string> ListVersions(List<DMSDocument> currentDocument)
        {
            return (from doc in currentDocument select doc.Version).ToList();
        }

        public virtual void ResetCheckout(List<DMSDocument> listFileDetails)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                foreach (var item in listFileDetails)
                {
                    item.IsCheckedOut = false;
                    item.CheckOutBy = null;
                }
                ctx.SaveChanges();
            }
        }

        public virtual DMSDirectory GetPathByDirectoryId(int? directoryId)
        {
            DMSDirectory docPathByDirectory = null;
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                docPathByDirectory = ctx.DMSDirectories.Where(x => x.DirectoryId == directoryId).FirstOrDefault();
            }
            return docPathByDirectory;
        }

        public virtual ICollection<DMSDirectory> BuildRecursiveDirectories(ICollection<DMSDirectory> resultList)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                foreach (var item in resultList)
                {
                    item.DirectoryList = (from direct in ctx.DMSDirectories
                                          join user in ctx.UserProfile on direct.AuthorId equals user.Id
                                          where item.DirectoryId == direct.DirectoryParentId && user.TenantID == context.TenantID && direct.Deleted == false
                                          select direct).ToList();

                    BuildRecursiveDirectories(item.DirectoryList);
                }
            }
            return resultList;
        }

        public virtual TreeViewModel PopulateCurrentUserDirectory(TreeViewModel treeViewModelCurrent, ICollection<DMSDirectory> resultList, ICollection<DMSDocument> resultFileList, List<int?> finalDirectoryIdList, List<int?> userFilesList)
        {
            List<TreeViewModel> treeViewModelSubs = new List<TreeViewModel>();
            var filteredItemsFiles = resultFileList.Where(item =>
                                                  item.DirectoryId == treeViewModelCurrent.ID).ToList();

            foreach (var itemUserFile in userFilesList)
            {
                var model = (from f in filteredItemsFiles
                             where f.FileId == itemUserFile
                             select new TreeViewModel
                             {
                                 ID = f.FileId,
                                 key = "F" + f.FileId.ToString(),
                                 ParentID = f.DirectoryId,
                                 title = f.FileName,
                                 isFolder = false,
                                 isSubFolder = false,
                                 activate = false,
                             }).ToList();

                treeViewModelSubs.AddRange(model);
            }
            foreach (var itemD in finalDirectoryIdList)
            {
                foreach (var item in resultList)
                {
                    if (itemD == item.DirectoryId)
                    {
                        TreeViewModel dyanaTreeViewItem1 = new TreeViewModel();
                        dyanaTreeViewItem1.ID = item.DirectoryId;
                        dyanaTreeViewItem1.key = "D" + item.DirectoryId.ToString();
                        dyanaTreeViewItem1.ParentID = item.DirectoryParentId;
                        dyanaTreeViewItem1.title = item.DirectoryName;
                        dyanaTreeViewItem1.isFolder = true;
                        dyanaTreeViewItem1.isSubFolder = true;
                        treeViewModelSubs.Add(dyanaTreeViewItem1);

                        PopulateCurrentUserDirectory(dyanaTreeViewItem1, item.DirectoryList, resultFileList, finalDirectoryIdList, userFilesList);
                    }
                }
            }
            treeViewModelCurrent.children = treeViewModelSubs;
            return treeViewModelCurrent;
        }

        public virtual TreeViewModel PopulateCurrentDirectory(TreeViewModel currentTreeViewModel, ICollection<DMSDirectory> directoryList, ICollection<DMSDocument> fileList)
        {
            var treeViewModelChildNodes = new List<TreeViewModel>();

            //// Files under directory to show in tree view
            //var currentDirectoryFiles = fileList.Where(item =>
            //                                      item.DirectoryId == currentTreeViewModel.ID).ToList();

            //treeViewModelChildNodes.AddRange((from item in currentDirectoryFiles
            //                            select new TreeViewModel()
            //                            {
            //                                ID = item.FileId,
            //                                key = "F" + item.FileId.ToString(),
            //                                ParentID = item.FileParentId,
            //                                title = item.FileName,
            //                                isFolder = false,
            //                                isSubFolder = false,
            //                                activate = false
            //                            }));

            foreach (var item in directoryList)
            {
                var dynaTreeViewDir = new TreeViewModel
                {
                    ID = item.DirectoryId,
                    key = "D" + item.DirectoryId.ToString(),
                    ParentID = item.DirectoryParentId,
                    title = item.DirectoryName,
                    isFolder = true,
                    isSubFolder = true,
                    isTabSelected = (item.DirectoryName == currentTreeViewModel.folderName) ? currentTreeViewModel.isTabSelected : false,
                    activate = (item.DirectoryName == currentTreeViewModel.folderName) == true ? true : false
                };
                treeViewModelChildNodes.Add(dynaTreeViewDir);

                PopulateCurrentDirectory(dynaTreeViewDir, item.DirectoryList, fileList);
            }

            // add child nodes to parent node
            currentTreeViewModel.children = treeViewModelChildNodes;

            return currentTreeViewModel;
        }

        public virtual DMSDocument GetDocumentVersions(List<DMSDocument> listFileDetails, string fileVersion)
        {
            return listFileDetails.Where(x => x.Version == fileVersion).FirstOrDefault();
        }

        public virtual bool UserFilesId(string userGuid, int fileId)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                var filesCount = ctx.DMSUsersFilesAuthorization.Any(i => i.UserId == userGuid && i.FileId == fileId);
                return filesCount;
            }
        }

        public virtual List<FilesDirectories> SelectedDirectoryFileData(int id, bool isFolder, string roleName, string userId)
        {
            List<FilesDirectories> folderFileData = new List<FilesDirectories>();
            List<DMSDocument> listFileDetails = new List<DMSDocument>();

            if (isFolder)
            {
                //// get folder details
                //if (roleName != "Admin" && roleName != "SuperAdmin")
                //{
                //    listFileDetails = UserDirectoryFilesList(id, folderFileData, userId);
                //}
                //else
                //{
                //    var ListDirectoryDetails = GetChildDirectories(id);
                //    folderFileData.AddRange((from item in ListDirectoryDetails
                //                             select new FilesDirectories()
                //                             {
                //                                 Id = item.DirectoryId,
                //                                 Name = item.DirectoryName,
                //                                 ParentID = item.DirectoryParentId,
                //                                 AuthorId = item.AuthorId,
                //                                 IsFolder = true
                //                             }));
                //    listFileDetails = GetMainFilesByDirId(id);
                //}

                var ListDirectoryDetails = GetChildDirectories(id);

                folderFileData.AddRange((from item in ListDirectoryDetails
                                         select new FilesDirectories()
                                         {
                                             Id = item.DirectoryId,
                                             Name = item.DirectoryName,
                                             ParentID = item.DirectoryParentId,
                                             AuthorId = item.AuthorId,
                                             IsFolder = true
                                         }));

                listFileDetails = GetMainFilesByDirId(id);
            }
            else
            { // get file details
                listFileDetails = GetVersion(id);
            }

            var authors = listFileDetails.Select(x => x.AuthorId).Distinct().ToList();
            var users = GetUserNames(authors);

            folderFileData.AddRange((from item in listFileDetails
                                     select new FilesDirectories()
                                     {
                                         Id = item.FileId,
                                         Name = item.FileName,
                                         ParentID = item.DirectoryId,
                                         FullPath = item.FullPath,
                                         Size = item.Size,
                                         IsCheckedOut = item.IsCheckedOut,
                                         AuthorId = item.AuthorId,
                                         Version = item.Version,
                                         IsFile = true,
                                         AutherName = GetUserName(users, item.AuthorId),
                                         DocumentControlID = item.DocumentControlID
                                     }));
            return folderFileData;
        }

        private string GetUserName(List<KeyValuePair<string, string>> users, string authorId)
        {
            var author = users.FirstOrDefault(x => x.Key.Equals(authorId, StringComparison.InvariantCultureIgnoreCase));

            if (author.Key != null)
                return author.Value;

            return "";
        }

        private List<DMSDocument> UserDirectoryFilesList(int id, List<FilesDirectories> folderFileData, string userId)
        {
            var listFileDetails = new List<DMSDocument>();
            var accessedDirectories = UserAccessedDirectories(userId);
            var directoryHierarchy = UserAccessedDirectoriesHierarchy(userId);
            var userFileId = UserFilesId(userId);
            var filesDirectoryIds = FilesParentDirectoryId(userFileId);
            var finalDirectoryIdList = directoryHierarchy.Union(filesDirectoryIds).ToList();
            var ListDirectoryDetails = GetChildDirectories(id);
            var userDirectoryList = new List<DMSDirectory>();

            if (ListDirectoryDetails.Count != 0)
            {
                foreach (var item in finalDirectoryIdList)
                {
                    foreach (var itemDetails in ListDirectoryDetails)
                    {
                        if (itemDetails.DirectoryId == item)
                        {
                            userDirectoryList.Add(itemDetails);
                        }
                    }
                }
                folderFileData.AddRange((from item in userDirectoryList
                                         select new FilesDirectories()
                                         {
                                             Id = item.DirectoryId,
                                             Name = item.DirectoryName,
                                             ParentID = item.DirectoryParentId,
                                             AuthorId = item.AuthorId,
                                             IsFolder = true
                                         }));

                foreach (var accessedDirectoryId in accessedDirectories)
                {
                    if (accessedDirectoryId == id)
                    {
                        listFileDetails = GetMainFilesByDirId(id);
                    }
                }
            }
            if (userDirectoryList.Count == 0)
            {
                var fileByDirectoryIdList = GetMainFilesByDirId(id);
                foreach (var item in fileByDirectoryIdList)
                {
                    foreach (var userFile in userFileId)
                    {
                        if (userFile == item.FileId)
                        {
                            listFileDetails.Add(item);
                        }
                    }
                }
            }
            return listFileDetails;
        }

        public virtual int LogDocumentsDownloaded(int FileId, string userIdGuid)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                ctx.DMSFileAuditLog.Add(new DMSFileAuditLog
                {
                    FileId = FileId,
                    UserId = userIdGuid,
                    CheckOutDate = DateTime.Now,
                    CheckInDate = DateTime.Now,
                });
                return ctx.SaveChanges();
            }
        }

        public virtual bool IsCheckOut(int fileId)
        {
            var documents = GetVersion(fileId);
            var isCheckOut = documents[0].IsCheckedOut;
            return isCheckOut;
        }

        public virtual List<string> DirectoryNamesList(int DirectoryId)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                return (from d in ctx.DMSDirectories
                        join user in ctx.UserProfile on d.AuthorId equals user.Id
                        where d.DirectoryParentId == DirectoryId && user.TenantID == context.TenantID
                        select d.DirectoryName).ToList();
            }
        }

        public virtual List<DMSDirectory> GetDuplicateDirectories(int directoryId, string folderName)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                return (from d in ctx.DMSDirectories
                        join user in ctx.UserProfile on d.AuthorId equals user.Id
                        where d.DirectoryParentId == directoryId && user.TenantID == context.TenantID
                            && d.DirectoryName.Equals(folderName, StringComparison.InvariantCultureIgnoreCase)
                        select d).ToList();
            }
        }

        public virtual bool HasCheckInAccess(string userId, int fileId)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                var files = (from d in ctx.DMSUsersFilesAuthorization
                             where d.UserId == userId && (d.FileId == fileId
                             || d.DirectoryId == fileId)
                             select d.AccessId
                    ).ToList();
                var Writeaccess = ctx.DMSAccess.Where(x => x.AccessType == "Write").Select(x => x.AccessId).FirstOrDefault();
                return (files.Contains(Writeaccess)) ? true : false;
            }
        }

        public void UpdateUserDirectoryFilePermissions(int ID, int ParentDirectoryId, string userId, string Identity)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                var userDirectoryList = (from d in ctx.DMSUsersFilesAuthorization
                                         join user in ctx.UserProfile on d.UserId equals user.Id
                                         where d.DirectoryId == ParentDirectoryId && context.TenantID == user.TenantID
                                         select new { d.UserId, d.AccessId }).ToList();

                foreach (var accessDetails in userDirectoryList)
                {
                    var userAuhtorizedData = new DMSUsersFilesAuthorization
                    {
                        AccessId = accessDetails.AccessId,
                        CustomerId = 1,
                        UserId = accessDetails.UserId,
                        CreatedByUser = userId,
                        UpdatedBy = userId,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        TenantID = context.TenantID
                    };

                    if (Identity == "NewFolder")
                    {
                        userAuhtorizedData.FileId = null;
                        userAuhtorizedData.DirectoryId = ID;
                    }
                    else
                    {
                        userAuhtorizedData.FileId = ID;
                        userAuhtorizedData.DirectoryId = null;
                    }

                    SaveUserFileAuthorization(userAuhtorizedData);
                }
            }
        }

        public void SaveUserFileAuthorization(DMSUsersFilesAuthorization userAuhtorizedData)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                ctx.DMSUsersFilesAuthorization.Add(userAuhtorizedData);
                ctx.SaveChanges();
            }
        }

        public virtual DMSTenantDocumentsDetails GetTenantDocumentsDetailByTenantId(string tenantId)
        {
            DMSTenantDocumentsDetails objDocument = null;
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                objDocument = (from item in ctx.DMSTenantDocumentsDetails
                               where item.TenantID == tenantId
                               select item).FirstOrDefault();
            }
            return objDocument;
        }

        public virtual List<KeyValuePair<string, string>> GetUserNames(List<string> authorIds)
        {
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                var users = ctx.UserProfile
                    .Where(x => authorIds.Contains(x.Id) && x.TenantID == context.TenantID)
                    .Select(x => new KeyValuePair<string, string>(x.Id, x.Name))
                    .ToList();

                return users;
            }
        }

        public virtual List<DMSDocument> GetFileListByFileId(string fileID)
        {
            List<DMSDocument> fileList = new List<DMSDocument>();

            foreach (var file in fileID.Split(','))
            {
                if (Guid.TryParse(file, out var newGuid))
                {
                    continue;
                    //Get document which is comming from ducument tabbles
                }
                else
                {
                    int fileId = (Convert.ToInt32(file));
                    using (DatabaseContext ctx = new DatabaseContext(context))
                    {

                        var GetFileData = (from itemFile in ctx.DMSDocument
                                           where itemFile.FileId == fileId
                                           orderby itemFile.UpdatedOn descending
                                           select itemFile).ToList();

                        fileList.Add(GetFileData[0]);

                    }
                }

            }
            return fileList;
        }

        public List<DMSDocument> GetFilesByIds(List<int> Ids)
        {
            List<DMSDocument> objDocument = new List<DMSDocument>();
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                objDocument = (from itemFile in ctx.DMSDocument
                               where Ids.Contains(itemFile.FileId)
                               orderby itemFile.UpdatedOn descending
                               select itemFile).ToList();
            }
            return objDocument;
        }
    }
}
