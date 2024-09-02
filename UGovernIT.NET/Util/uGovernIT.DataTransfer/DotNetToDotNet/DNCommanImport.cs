using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using uGovernIT.DAL.Infratructure;
using uGovernIT.DataTransfer.Infratructure;
using uGovernIT.Manager;
using uGovernIT.Manager.Core;
using uGovernIT.Manager.DMSOperations;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.Common;
using uGovernIT.Utility.Entities.DB;
using uGovernIT.Utility.Entities.DB.uGovernIT.Utility;
using uGovernIT.Utility.Entities.DMSDB;
using Constants = uGovernIT.Utility.Constants;
using DefaultData = uGovernIT.DefaultConfig;

namespace uGovernIT.DataTransfer.DotNetToDotNet
{
    public class DNCommanImport : CommanEntity
    {
        bool importWithUpdate;
        bool deleteBeforeImport;
        DNImportContext context;
        public DNCommanImport(DNImportContext context) : base(context)
        {
            this.context = context;
            importWithUpdate = false;
            if (JsonConfig.Config.Global.commonsettings.importwithupdate)
                importWithUpdate = true;

            deleteBeforeImport = false;
            if (JsonConfig.Config.Global.commonsettings.deletebeforeimport)
                deleteBeforeImport = true;
        }

        public override void UpdateLocation()
        {
            base.UpdateLocation();

            bool import = context.IsImportEnable("Location");

            if (import)
                ULog.WriteLog($"Updating locations");
            else
                ULog.WriteLog($"Load Location Mapping");

            LocationManager mgr = new LocationManager(context.AppContext);
            List<Location> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<Location>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            string listName = DatabaseObjects.Tables.Location;
            MappedItemList locationlist = context.GetMappedList(listName);
            LocationManager sourceMgr = new LocationManager(context.SourceAppContext);
            List<Location> sourceDbData = sourceMgr.Load();

            int targetNewItemCount = 0;

            Location targetItem = null;

            string sourceItemID = string.Empty;
            foreach (Location item in sourceDbData)
            {
                sourceItemID = item.ID.ToString();
                targetItem = dbData.FirstOrDefault(x => x.Region == item.Region && x.Country == item.Country && x.State == item.State && x.Title == item.Title);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new Location();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        targetItem = item;
                        mgr.AddOrUpdate(targetItem);
                    }
                }

                if (targetItem != null)
                    locationlist.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
            }

            if (import)
                ULog.WriteLog($"{targetNewItemCount} Locations added");

        }
        public override void UpdateDepartments()
        {
            base.UpdateDepartments();
            bool import = context.IsImportEnable("Departments");

            if (import)
                ULog.WriteLog($"Updating companies");
            else
                ULog.WriteLog($"Load companies mapping");

            List<Company> companydbData = new List<Company>();
            string listName = DatabaseObjects.Tables.Company;
            MappedItemList cmplist = context.GetMappedList(listName);

            {
                CompanyManager mgr = new CompanyManager(context.AppContext);
                companydbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(companydbData);
                        companydbData = new List<Company>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                Company targetItem = null;
                CompanyManager sourceMgr = new CompanyManager(context.SourceAppContext);
                List<Company> sourceCompanydbData = sourceMgr.Load();

                string sourceItemID = string.Empty;
                foreach (Company item in sourceCompanydbData)
                {
                    sourceItemID = item.ID.ToString();
                    targetItem = companydbData.FirstOrDefault(x => x.Title == item.Title);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new Company();
                            companydbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }



                    }

                    if (targetItem != null)
                        cmplist.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }


                if (import)
                    ULog.WriteLog($"{targetNewItemCount} companies added");
            }

            if (import)
                ULog.WriteLog($"Updating divisions");
            else
                ULog.WriteLog($"Loading divisions");
            List<CompanyDivision> divisiondbData = new List<CompanyDivision>();
            listName = DatabaseObjects.Tables.CompanyDivisions;
            MappedItemList divisionlist = context.GetMappedList(listName);

            {
                CompanyDivisionManager mgr = new CompanyDivisionManager(context.AppContext);
                divisiondbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(divisiondbData);
                        divisiondbData = new List<CompanyDivision>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }



                int targetNewItemCount = 0;
                CompanyDivision targetItem = null;
                CompanyDivisionManager sourceMgr = new CompanyDivisionManager(context.SourceAppContext);
                List<CompanyDivision> sdivisiondbData = sourceMgr.Load();


                string sourceItemID = string.Empty;
                foreach (CompanyDivision item in sdivisiondbData)
                {
                    sourceItemID = item.ID.ToString();
                    long? sCompanyLookup = item.CompanyIdLookup;
                    string targetCompanyID = string.Empty;

                    if (item.CompanyIdLookup.HasValue)
                        targetCompanyID = cmplist.GetTargetID(sourceid: item.CompanyIdLookup.Value.ToString());

                    if (string.IsNullOrWhiteSpace(targetCompanyID))
                        targetItem = divisiondbData.FirstOrDefault(x => x.Title == item.Title && !x.CompanyIdLookup.HasValue);
                    else
                        targetItem = divisiondbData.FirstOrDefault(x => x.Title == item.Title && x.CompanyIdLookup.GetValueOrDefault() == UGITUtility.StringToLong(targetCompanyID));

                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new CompanyDivision();
                            divisiondbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            targetItem.CompanyIdLookup = null;
                            if (!string.IsNullOrWhiteSpace(targetCompanyID))
                            {
                                targetItem.CompanyIdLookup = UGITUtility.StringToLong(targetCompanyID);
                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                    }
                    if (targetItem != null)
                        divisionlist.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} divisions added");
            }

            if (import)
                ULog.WriteLog($"Updating departments");
            else
                ULog.WriteLog($"Load departments mapping");
            List<Department> departmentdbData = new List<Department>();
            listName = "Department";
            MappedItemList departmentlist = context.GetMappedList(DatabaseObjects.Tables.Department);

            {
                DepartmentManager mgr = new DepartmentManager(context.AppContext);
                departmentdbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(departmentdbData);
                        departmentdbData = new List<Department>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }


                int targetNewItemCount = 0;
                Department targetItem = null;

                DepartmentManager sourceMgr = new DepartmentManager(context.SourceAppContext);
                List<Department> sdepartmentdbData = sourceMgr.Load();
                string sourceItemID = string.Empty;
                foreach (Department item in sdepartmentdbData)
                {
                    sourceItemID = item.ID.ToString();
                    long? sCompanyLookup = item.CompanyIdLookup;
                    long? sDivisionLookup = item.DivisionIdLookup;

                    string targetCompanyID = string.Empty;
                    if (sCompanyLookup.GetValueOrDefault(0) > 0)
                        targetCompanyID = cmplist.GetTargetID(sCompanyLookup.Value.ToString());

                    string targetDivisionID = string.Empty;
                    if (sDivisionLookup.GetValueOrDefault(0) > 0)
                        targetDivisionID = cmplist.GetTargetID(sDivisionLookup.Value.ToString());

                    if (string.IsNullOrWhiteSpace(targetCompanyID) && string.IsNullOrWhiteSpace(targetDivisionID))
                        targetItem = departmentdbData.FirstOrDefault(x => x.Title == item.Title && !x.CompanyIdLookup.HasValue && !x.DivisionIdLookup.HasValue);
                    else if (!string.IsNullOrWhiteSpace(targetCompanyID) && !string.IsNullOrWhiteSpace(targetDivisionID))
                        targetItem = departmentdbData.FirstOrDefault(x => x.Title == item.Title && x.CompanyIdLookup.HasValue && x.DivisionIdLookup.HasValue
                        && x.CompanyIdLookup.GetValueOrDefault() == UGITUtility.StringToLong(targetCompanyID) && x.DivisionIdLookup.GetValueOrDefault() == UGITUtility.StringToLong(targetDivisionID));
                    else if (!string.IsNullOrWhiteSpace(targetCompanyID))
                        targetItem = departmentdbData.FirstOrDefault(x => x.Title == item.Title && x.CompanyIdLookup.HasValue && !x.DivisionIdLookup.HasValue && x.CompanyIdLookup.GetValueOrDefault() == UGITUtility.StringToLong(targetCompanyID));
                    else if (!string.IsNullOrWhiteSpace(targetDivisionID))
                        targetItem = departmentdbData.FirstOrDefault(x => x.Title == item.Title && !x.CompanyIdLookup.HasValue && x.DivisionIdLookup.HasValue && x.DivisionIdLookup.GetValueOrDefault() == UGITUtility.StringToLong(targetDivisionID));

                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new Department();
                            departmentdbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {

                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            targetItem.CompanyIdLookup = null;
                            if (!string.IsNullOrWhiteSpace(targetCompanyID) && UGITUtility.StringToLong(targetCompanyID) > 0)
                            {
                                targetItem.CompanyIdLookup = UGITUtility.StringToLong(targetCompanyID);
                            }

                            targetItem.DivisionIdLookup = null;
                            if (!string.IsNullOrWhiteSpace(targetDivisionID) && UGITUtility.StringToLong(targetDivisionID) > 0)
                            {
                                targetItem.DivisionIdLookup = UGITUtility.StringToLong(targetDivisionID);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }


                    }

                    if (targetItem != null)
                        departmentlist.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} departments added");
            }
        }
        public override void UpdateFunctionalAreas()
        {
            base.UpdateFunctionalAreas();
            bool import = context.IsImportEnable("FunctionalAreas");

            if (import)
                ULog.WriteLog($"Updating Functional Areas");
            else
                ULog.WriteLog($"Load functional areas mapping");

            List<FunctionalArea> targetdbData = new List<FunctionalArea>();
            string listName = "FunctionalAreas";
            MappedItemList maplist = context.GetMappedList(listName);
            MappedItemList userMappedList = context.GetMappedList(DatabaseObjects.Tables.AspNetUsers);
            {
                FunctionalAreasManager mgr = new FunctionalAreasManager(context.AppContext);
                targetdbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(targetdbData);
                        targetdbData = new List<FunctionalArea>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                FunctionalArea targetItem = null;

                FunctionalAreasManager sourceMgr = new FunctionalAreasManager(context.SourceAppContext);
                List<FunctionalArea> sourcedbData = sourceMgr.Load();
                MappedItemList departmentList = context.GetMappedList(DatabaseObjects.Tables.Department);
                string sourceItemID = string.Empty;
                foreach (FunctionalArea item in sourcedbData)
                {
                    sourceItemID = item.ID.ToString();
                    long sDepartmentLookup = item.DepartmentLookup.GetValueOrDefault(0);


                    targetItem = targetdbData.FirstOrDefault(x => x.Title == item.Title);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new FunctionalArea();
                            targetdbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            targetItem.DepartmentLookup = null;
                            if (sDepartmentLookup > 0)
                            {
                                string sourceID = departmentList.GetTargetID(sDepartmentLookup.ToString());
                                if (!string.IsNullOrWhiteSpace(sourceID) && UGITUtility.StringToLong(sourceID) > 0)
                                    targetItem.DepartmentLookup = UGITUtility.StringToLong(sourceID);
                            }

                            if (!string.IsNullOrWhiteSpace(targetItem.Owner) && userMappedList != null)
                                targetItem.Owner = userMappedList.GetTargetIDs(UGITUtility.ConvertStringToList(targetItem.Owner, Constants.Separator));

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                    }


                    if (targetItem != null)
                        maplist.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} functional areas added");
            }
        }
        public override void UpdateUsersAndRoles()
        {
            base.UpdateUsersAndRoles();

            bool import = context.IsImportEnable("Users");

            if (import)
                ULog.WriteLog($"Updating Users");
            else
                ULog.WriteLog($"Load users and roles mapping");

            UserProfileManager mgr = context.AppContext.UserManager;
            List<UserProfile> dbData = mgr.GetUsersProfileWithGroup();
            MappedItemList userMappedlist = context.GetMappedList(DatabaseObjects.Tables.AspNetUsers);

            int targetNewItemCount = 0;
            UserProfile targetItem = null;
            UserProfileManager sourceMgr = context.SourceAppContext.UserManager;
            List<UserProfile> sourceDbData = sourceMgr.GetUsersProfile();
            string sourceItemID = string.Empty;
            Hashtable htUserManager = new Hashtable(); //to set new User's Managers.
            foreach (UserProfile item in sourceDbData)
            {
                if (!item.isRole)
                {
                    sourceItemID = item.Id;
                    targetItem = dbData.FirstOrDefault(x => x.UserName == item.UserName && !x.isRole);

                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new UserProfile();
                        }

                        if (string.IsNullOrWhiteSpace(targetItem.Id) || importWithUpdate)
                        {
                            targetItem.UserName = item.UserName;
                            targetItem.Name = item.Name;
                            targetItem.Email = item.Email;
                            targetItem.Enabled = item.Enabled;
                            targetItem.CreatedBy = item.CreatedBy;
                            targetItem.ModifiedBy = item.ModifiedBy;
                            targetItem.TenantID = context.AppContext.TenantID;
                            targetItem.ManagerID = item.ManagerID;
                        }

                        if (string.IsNullOrWhiteSpace(targetItem.Id))
                        {
                            IdentityResult result = mgr.Create(targetItem, mgr.GeneratePassword());
                            if (result.Succeeded)
                            {
                                targetNewItemCount++;
                            }
                            else
                            {
                                ULog.WriteLog($"{targetItem.LoginName} not created, errors: {string.Join("; ", result.Errors)}");
                            }
                        }
                        else
                        {
                            mgr.Update(targetItem);
                        }
                    }

                    if (targetItem != null)
                        userMappedlist.Add(new MappedItem(sourceItemID, targetItem.Id));

                    if (targetItem != null && !string.IsNullOrEmpty(targetItem.Id) && !string.IsNullOrEmpty(targetItem.ManagerID))
                        htUserManager.Add(targetItem.Id, targetItem.ManagerID);
                }
            }

            var LstUserProfile = mgr.GetUsersProfile().Where(x => x.TenantID.Equals(context.AppContext.TenantID, StringComparison.InvariantCultureIgnoreCase)).Select(x => new { x.Id, x.Name, x.UserName }).ToList();
            UserProfile user = new UserProfile();
            foreach (DictionaryEntry item in htUserManager)
            {
                user = mgr.GetUserById(Convert.ToString(item.Key));
                if (user != null)
                {
                    //var manager = LstUserProfile.FirstOrDefault(x => x.Id.Equals(Convert.ToString(item.Value), StringComparison.InvariantCultureIgnoreCase));
                    var manager = context.GetTargetUserValue($"{item.Value}");
                    if (!string.IsNullOrEmpty(manager))
                    {
                        user.ManagerID = manager;
                        mgr.Update(user);
                    }                    
                }
            }

            if (import)
            {
                ULog.WriteLog($"{targetNewItemCount} users added");
                ULog.WriteLog($"Update Roles and mapping");
            }

            // Below code added as Above code is resetting  context.AppContext.TenantID
            if (context.AppContext.TenantID == context.SourceAppContext.TenantID)
            {
                context.AppContext = ApplicationContext.CreateContext(JsonConfig.Config.Global.permission.target.dbconnection, JsonConfig.Config.Global.permission.target.commondbconnection, JsonConfig.Config.Global.permission.target.tenantid); ;
            }

            if (context.IsImportEnable("Roles"))
            {
                ULog.WriteLog($"Updating Roles");
                UserRoleManager roleMgr = new UserRoleManager(context.AppContext);
                List<Role> roleList = roleMgr.Load();
                ManagerBase<UserRole> userRoleMgr = new ManagerBase<UserRole>(context.AppContext);
                int targetGroupCount = 0;
                ManagerBase<UserRole> sourceUserRoleMgr = new ManagerBase<UserRole>(context.SourceAppContext);
                UserRoleManager sourceRoleMgr = new UserRoleManager(context.SourceAppContext);
                List<Role> sourceRoleList = sourceRoleMgr.Load();
                foreach (Role rItem in sourceRoleList)
                {
                    Role targetRole = roleList.FirstOrDefault(x => x.Title.ToLower() == rItem.Title.ToLower());
                    if (targetRole == null)
                    {
                        targetRole = new Role();
                        targetGroupCount++;
                    }
                    if (!string.IsNullOrWhiteSpace(targetRole.Id) || importWithUpdate)
                    {
                        targetRole.Name = rItem.Name;
                        targetRole.Title = rItem.Title;
                        targetRole.LandingPage = rItem.LandingPage;
                        targetRole.Description = rItem.Description;
                        targetRole.CreatedBy = rItem.CreatedBy;
                        targetRole.ModifiedBy = rItem.ModifiedBy;
                        targetRole.TenantID = context.AppContext.TenantID;
                        roleMgr.AddOrUpdate(targetRole);
                    }
                    if (import)
                    {
                        if (targetRole != null)
                        {
                            sourceItemID = rItem.Id;
                            userMappedlist.Add(new MappedItem(sourceItemID, targetRole.Id));
                        }
                        List<UserRole> userRoleList = userRoleMgr.Load(x => x.TenantID == context.AppContext.TenantID && x.RoleId == targetRole.Id);
                        string targetUserID = string.Empty;
                        List<UserRole> roleUsers = sourceUserRoleMgr.Load(x => x.RoleId == rItem.Id);
                        foreach (UserRole spUser in roleUsers)
                        {
                            targetUserID = userMappedlist.GetTargetID(spUser.UserId.ToString());
                            if (string.IsNullOrEmpty(targetUserID))
                            {
                                ULog.WriteLog($"user not found in target so not added in {rItem.Title} group");
                                continue;
                            }
                            try
                            {
                                if (!userRoleList.Exists(x => x.UserId.ToLower() == targetUserID.ToLower()))
                                    mgr.AddToRole(targetUserID, targetRole.Name);
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteLog($"Exception in UpdateUsersAndRoles(): {ex}");
                            }
                        }
                    }
                }
                ULog.WriteLog($"{targetGroupCount} groups added");
            }
            else
                return;
        }

        public override void UpdateModules()
        {
            base.UpdateModules();
            bool import = context.IsImportEnable("Modules");

            if (import)
                ULog.WriteLog($"Updating modules");
            else
                ULog.WriteLog($"Load modules mapping");


            ModuleViewManager mgr = new ModuleViewManager(context.AppContext);
            List<UGITModule> dbData = mgr.Load();



            List<string> targetNewItems = new List<string>();
            List<string> targetUpdateItems = new List<string>();

            UGITModule targetItem = null;
            MappedItemList userMappedList = context.GetMappedList(DatabaseObjects.Tables.AspNetUsers);


            ModuleViewManager sourceMgr = new ModuleViewManager(context.SourceAppContext);
            List<UGITModule> sourceDbData = sourceMgr.Load();
            string sourceItemID = string.Empty;
            foreach (UGITModule item in sourceDbData)
            {
                sourceItemID = item.ID.ToString();
                targetItem = dbData.FirstOrDefault(x => x.ModuleName == item.ModuleName);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new UGITModule();
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        targetItem = item;

                        if (!string.IsNullOrWhiteSpace(targetItem.AuthorizedToView) && userMappedList != null)
                            targetItem.AuthorizedToView = userMappedList.GetTargetIDs(UGITUtility.ConvertStringToList(targetItem.AuthorizedToView, Constants.Separator));

                        if (!string.IsNullOrWhiteSpace(targetItem.AuthorizedToCreate) && userMappedList != null)
                            targetItem.AuthorizedToCreate = userMappedList.GetTargetIDs(UGITUtility.ConvertStringToList(targetItem.AuthorizedToCreate, Constants.Separator));


                        if (targetItem.ID > 0)
                        {
                            mgr.Update(targetItem);
                            targetUpdateItems.Add(targetItem.ModuleName);
                        }
                        else
                        {
                            mgr.Insert(targetItem);
                            dbData.Add(targetItem);
                            targetNewItems.Add(targetItem.ModuleName);
                        }
                    }

                }
            }

            if (import)
            {
                if (targetNewItems.Count > 0)
                    ULog.WriteLog($"{string.Join(", ", targetNewItems)} modules added");
                else
                    ULog.WriteLog($"no new modules added");

                if (targetUpdateItems.Count > 0)
                    ULog.WriteLog($"{string.Join(", ", targetUpdateItems)} modules updated");
            }
        }
        public override void UpdateTenantDefaultUser()
        {
            base.UpdateTenantDefaultUser();

            ULog.WriteLog($"Create default admininstrator if not exist");
            List<Role> roles = new List<Role>();
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "SAdmin", Title = "Super Admin", IsSystem = true });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "Admin", Title = "Admin", IsSystem = true, LandingPage = "/Admin/Admin" });
            var rolemanager = new UserRoleManager(context.AppContext);
            foreach (Role r in roles)
            {
                if (rolemanager.Get(x => x.Name == r.Name) == null)
                {
                    r.TenantID = context.AppContext.TenantID;
                    rolemanager.AddOrUpdate(r);
                }
            }

            MappedItemList userMappedlist = context.GetMappedList(DatabaseObjects.Tables.AspNetUsers);
            string userName = JsonConfig.Config.Global.permission.target.defaultuser.username;
            string password = JsonConfig.Config.Global.permission.target.defaultuser.password;
            string email = JsonConfig.Config.Global.permission.target.defaultuser.email;
            UserProfileManager mgr = context.AppContext.UserManager;
            UserProfile targetItem = mgr.GetUserByUserName(userName);
            if (targetItem == null)
            {
                targetItem = new UserProfile();
                targetItem.UserName = userName;
                targetItem.LoginName = userName;
                targetItem.Name = userName;
                targetItem.Enabled = true;
                targetItem.Email = email;


                IdentityResult result = mgr.Create(targetItem, password);
                if (result.Succeeded)
                {
                    userMappedlist.Add(new MappedItem("0", targetItem.Id));
                }
                else
                {
                    ULog.WriteLog($"{targetItem.LoginName} not created, errors: {string.Join("; ", result.Errors)}");
                }

                try
                {
                    //Create Super Admin User
                    UserProfile sadmin = new UserProfile();
                    sadmin.UserName = "SuperAdmin";
                    sadmin.Name = "Super Admin";
                    sadmin.Enabled = true;
                    sadmin.Email = "support@serviceprime.com";
                    IdentityResult sadminResult = mgr.Create(sadmin, password);

                    Role adminRole = rolemanager.Get(x => x.Name == RoleType.Admin.ToString());
                    ManagerBase<UserRole> userRoleMgr = new ManagerBase<UserRole>(context.AppContext);
                    UserRole userRole = userRoleMgr.Get(x => x.TenantID == context.AppContext.TenantID && x.RoleId == adminRole.Id);
                    if (userRole == null)
                    {
                        mgr.AddUserRole(targetItem, RoleType.Admin.ToString());
                        mgr.AddUserRole(sadmin, RoleType.Admin.ToString());
                    }
                }
                catch
                {
                    //comes with mapping is already exist
                }
            }
            else
            {
                targetItem.Enabled = true;
                mgr.Update(targetItem);

                userMappedlist.Add(new MappedItem("0", targetItem.Id));
                try
                {
                    mgr.AddUserRole(targetItem, RoleType.Admin.ToString());
                }
                catch { }
            }
        }
        public override void UpdateConfigurationVariables()
        {
            base.UpdateConfigurationVariables();
            bool import = context.IsImportEnable("ConfigurationVariables");
            if (!import)
                return;

            ULog.WriteLog($"Updating configuration variables");

            {
                ConfigurationVariableManager mgr = new ConfigurationVariableManager(context.AppContext);
                List<ConfigurationVariable> dbData = mgr.Load();

                int targetNewItemCount = 0;

                string keyName = string.Empty;
                ConfigurationVariable targetItem = null;


                ConfigurationVariableManager sourceMgr = new ConfigurationVariableManager(context.SourceAppContext);
                List<ConfigurationVariable> sourceDbData = sourceMgr.Load();
                foreach (ConfigurationVariable item in sourceDbData)
                {
                    if (item.KeyName.EqualsIgnoreCase(ConfigConstants.SmtpCredentials))
                        continue;

                    keyName = item.KeyName;

                    targetItem = dbData.FirstOrDefault(x => x.KeyName == keyName);

                    if (targetItem == null)
                    {
                        targetItem = new ConfigurationVariable();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }

                }

                ConfigurationVariable config = mgr.LoadVaribale(ConfigConstants.AccountName);
                if (config != null)
                {
                    config.KeyValue = context.AppContext.TenantAccountId;
                    mgr.Update(config);
                }

                ULog.WriteLog($"{targetNewItemCount} configuration variables added");
            }
        }
        public override void UpdateBudgetsCategory()
        {
            base.UpdateBudgetsCategory();
            bool import = context.IsImportEnable("BudgetsCategory");

            if (import)
                ULog.WriteLog($"Updating budget categories");
            else
                ULog.WriteLog($"Load budget categories mapping");

            {
                BudgetCategoryViewManager mgr = new BudgetCategoryViewManager(context.AppContext);
                List<BudgetCategory> dbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<BudgetCategory>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }
                string listName = DatabaseObjects.Tables.BudgetCategories;
                MappedItemList targetMappedList = context.GetMappedList(listName);

                int targetNewItemCount = 0;

                string category = string.Empty, subCategory = string.Empty;
                BudgetCategory targetItem = null;
                MappedItemList userMappedList = context.GetMappedList(DatabaseObjects.Tables.AspNetUsers);

                BudgetCategoryViewManager sourceMgr = new BudgetCategoryViewManager(context.SourceAppContext);
                List<BudgetCategory> sourceDbData = sourceMgr.Load();
                string sourceItemID = string.Empty;
                foreach (BudgetCategory item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();
                    category = item.BudgetCategoryName;
                    subCategory = item.BudgetSubCategory;

                    targetItem = dbData.FirstOrDefault(x => x.BudgetCategoryName == category && x.BudgetSubCategory == subCategory);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new BudgetCategory();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            if (!string.IsNullOrWhiteSpace(targetItem.AuthorizedToView) && userMappedList != null)
                                targetItem.AuthorizedToView = userMappedList.GetTargetIDs(UGITUtility.ConvertStringToList(targetItem.AuthorizedToView, Constants.Separator));

                            if (!string.IsNullOrWhiteSpace(targetItem.AuthorizedToEdit) && userMappedList != null)
                                targetItem.AuthorizedToEdit = userMappedList.GetTargetIDs(UGITUtility.ConvertStringToList(targetItem.AuthorizedToEdit, Constants.Separator));


                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                    }
                    if (targetItem != null)
                        targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} budgets categories added");
            }
        }

        public override void CreateDefaultEntries()
        {
            base.CreateDefaultEntries();

            bool import = context.IsImportEnable("DefaultEntries");
            if (!import)
                return;

            ULog.WriteLog($"Creating fieldconfigurations, tabs, admin menus");
            DefaultData.Data.DefaultData.ConfigData defaultData = new DefaultData.Data.DefaultData.ConfigData(context.AppContext);

            TabViewManager tabViewManager = new TabViewManager(context.AppContext);
            List<TabView> commanTabView = tabViewManager.Load();
            if (commanTabView.Count == 0)
            {
                TabViewManager stabViewManager = new TabViewManager(context.SourceAppContext);
                List<TabView> scommanTabView = stabViewManager.Load();
                if (scommanTabView.Count > 0)
                {
                    scommanTabView.ForEach(x => { x.ID = 0; x.TenantID = context.AppContext.TenantID; x.ModifiedBy = string.Empty; x.CreatedBy = string.Empty; });
                    tabViewManager.InsertItems(scommanTabView);
                }
            }

            // Import Non Modules Request List.
            ModuleViewManager sourceMgr = new ModuleViewManager(context.AppContext);
            List<UGITModule> sourceDbData = sourceMgr.Load();
            ModuleColumnManager moduleColumnManager = new ModuleColumnManager(context.AppContext);
            List<ModuleColumn> moduleColumns = moduleColumnManager.Load(x => !sourceDbData.Any(y => x.CategoryName.Equals(y.ModuleName)));
            moduleColumnManager.Delete(moduleColumns);
            moduleColumns = new List<ModuleColumn>();
            if (moduleColumns.Count == 0)
            {
                ModuleColumnManager sModuleColumnManager = new ModuleColumnManager(context.SourceAppContext);
                List<ModuleColumn> sModuleColumns = sModuleColumnManager.Load(x => !sourceDbData.Any(y => x.CategoryName.Equals(y.ModuleName)));
                if (sModuleColumns.Count > 0)
                {
                    sModuleColumns.ForEach(x => { x.ID = 0; x.TenantID = context.AppContext.TenantID; x.ModifiedBy = string.Empty; x.CreatedBy = string.Empty; });
                    moduleColumnManager.InsertItems(sModuleColumns);
                }
            }


            FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(context.AppContext);
            List<FieldConfiguration> fieldConfigurations = fieldConfigurationManager.Load();
            if (fieldConfigurations.Count == 0)
            {
                FieldConfigurationManager sfieldConfigurationManager = new FieldConfigurationManager(context.SourceAppContext);
                List<FieldConfiguration> sfieldConfigurations = sfieldConfigurationManager.Load();
                if (sfieldConfigurations.Count > 0)
                {
                    sfieldConfigurations.ForEach(x => { x.ID = 0; x.TenantID = context.AppContext.TenantID; x.ModifiedBy = string.Empty; x.CreatedBy = string.Empty; });
                    fieldConfigurationManager.InsertItems(sfieldConfigurations);
                }
            }


            AdminCategoryManager adminCategoryManager = new AdminCategoryManager(context.AppContext);
            List<ClientAdminCategory> adminCategories = adminCategoryManager.Load();
            MappedItemList cAdminCategoriesMappedList = context.GetMappedList(DatabaseObjects.Tables.ClientAdminCategory);
            if (adminCategories.Count == 0)
            {
                AdminCategoryManager sadminCategoryManager = new AdminCategoryManager(context.SourceAppContext);
                List<ClientAdminCategory> sadminCategories = sadminCategoryManager.Load();
                foreach (ClientAdminCategory cts in sadminCategories)
                {
                    string sourceItemId = cts.ID.ToString();
                    cts.ID = 0;
                    cts.TenantID = context.AppContext.TenantID;
                    cts.ModifiedBy = string.Empty;
                    cts.CreatedBy = string.Empty;
                    adminCategoryManager.Insert(cts);
                    cAdminCategoriesMappedList.Add(new MappedItem(sourceItemId, cts.ID.ToString()));
                }
            }

            AdminConfigurationListManager adminConfigurationListManager = new AdminConfigurationListManager(context.AppContext);
            List<ClientAdminConfigurationList> adminList = adminConfigurationListManager.Load();
            if (adminList.Count == 0)
            {
                AdminConfigurationListManager sadminConfigurationListManager = new AdminConfigurationListManager(context.SourceAppContext);
                List<ClientAdminConfigurationList> sadminList = sadminConfigurationListManager.Load();
                foreach (ClientAdminConfigurationList cts in sadminList)
                {
                    string sourceItemId = cts.ID.ToString();
                    cts.ID = 0;
                    cts.TenantID = context.AppContext.TenantID;
                    cts.ModifiedBy = string.Empty;
                    cts.CreatedBy = string.Empty;
                    cts.ClientAdminCategoryLookup = UGITUtility.StringToLong(cAdminCategoriesMappedList.GetTargetID(cts.ClientAdminCategoryLookup.ToString()));
                }

                adminConfigurationListManager.InsertItems(sadminList);
            }

            MenuNavigationManager menuManager = new MenuNavigationManager(context.AppContext);
            List<MenuNavigation> menu = menuManager.Load();
            menuManager.Delete(menu);
            menu = new List<MenuNavigation>();
            if (menu.Count == 0)
            {
                MenuNavigationManager smenuManager = new MenuNavigationManager(context.SourceAppContext);
                List<MenuNavigation> smenu = smenuManager.Load();

                List<MenuNavigation> rootItems = smenu.Where(x => x.MenuParentLookup == 0).ToList();
                foreach (MenuNavigation mItem in rootItems)
                {
                    long rootItemID = mItem.ID;

                    mItem.ID = 0;
                    mItem.TenantID = context.AppContext.TenantID;
                    mItem.CreatedBy = string.Empty;
                    mItem.ModifiedBy = string.Empty;
                    menuManager.Insert(mItem);
                    List<MenuNavigation> subItems = smenu.Where(x => x.MenuParentLookup == rootItemID).ToList();
                    if (subItems.Count > 0)
                    {
                        foreach (MenuNavigation smItem in subItems)
                        {
                            smItem.ID = 0;
                            smItem.TenantID = context.AppContext.TenantID;
                            smItem.CreatedBy = string.Empty;
                            smItem.ModifiedBy = string.Empty;
                            smItem.MenuParentLookup = mItem.ID;
                            menuManager.Insert(smItem);
                        }
                    }
                }
            }


            PageConfigurationManager pageMgr = new PageConfigurationManager(context.AppContext);
            List<PageConfiguration> pages = pageMgr.Load();
            pageMgr.Delete(pages);
            pages = new List<PageConfiguration>();
            if (pages.Count == 0)
            {
                PageConfigurationManager spageMgr = new PageConfigurationManager(context.SourceAppContext);
                List<PageConfiguration> spages = spageMgr.Load();
                if (spages.Count > 0)
                {
                    spages.ForEach(x => { x.ID = 0; x.TenantID = context.AppContext.TenantID; x.ModifiedBy = string.Empty; x.CreatedBy = string.Empty; });
                    pageMgr.InsertItems(spages);
                }
            }
        }

        public override void UpdateWikiCategories()
        {
            base.UpdateWikiCategories();

            bool import = context.IsImportEnable("WikiCategories");

            if (import)
                ULog.WriteLog($"Updating Wiki Categories");
            else
                ULog.WriteLog($"Load Wiki Categories Mapping");

            WikiMenuLeftNavigationManager wikiMenuLeftNavigationManager = new WikiMenuLeftNavigationManager(context.AppContext);
            List<WikiCategory> wikiCategory = wikiMenuLeftNavigationManager.Load();
            wikiMenuLeftNavigationManager.Delete(wikiCategory);
            if (wikiCategory.Count == 0)
            {
                WikiMenuLeftNavigationManager sWikiMenuLeftNavigationManager = new WikiMenuLeftNavigationManager(context.SourceAppContext);
                List<WikiCategory> sWikiCategory = sWikiMenuLeftNavigationManager.Load();
                if (sWikiCategory.Count > 0)
                {
                    sWikiCategory.ForEach(x => { x.ID = 0; x.TenantID = context.AppContext.TenantID; x.ModifiedBy = string.Empty; x.CreatedBy = string.Empty; });
                    wikiMenuLeftNavigationManager.InsertItems(sWikiCategory);
                }
            }
        }

        public override void UpdateStageExitCriteriaTemplates()
        {
            base.UpdateStageExitCriteriaTemplates();

            bool import = context.IsImportEnable("UpdateStageExitCriteriaTemplates");

            if (import)
                ULog.WriteLog($"Updating Stage Exit Criteria");
            else
                ULog.WriteLog($"Load Stage Exit Criteria Mapping");

            ModuleStageConstraintTemplatesManager constraintManager = new ModuleStageConstraintTemplatesManager(context.AppContext);
            List<ModuleStageConstraintTemplates> constraint = constraintManager.Load();
            constraintManager.Delete(constraint);
            if (constraint.Count == 0)
            {
                ModuleStageConstraintTemplatesManager sConstraintManager = new ModuleStageConstraintTemplatesManager(context.SourceAppContext);
                List<ModuleStageConstraintTemplates> sConstraint = sConstraintManager.Load();
                if (sConstraint.Count > 0)
                {
                    sConstraint.ForEach(x => { x.ID = 0; x.AssignedTo = context.GetTargetUserValue(x.AssignedTo); x.TenantID = context.AppContext.TenantID; x.ModifiedBy = string.Empty; x.CreatedBy = string.Empty; });
                    constraintManager.InsertItems(sConstraint);
                }
            }
        }

        public override void UpdateDashboardAndQueries()
        {
            base.UpdateDashboardAndQueries();

            bool import = context.IsImportEnable("DashboardAndQueries");

            if (import)
                ULog.WriteLog($"Updating DashboardAndQueries");
            else
                ULog.WriteLog($"Load DashboardAndQueries Mapping");

            int targetNewItemCount = 0;

            ULog.WriteLog($"Updating DashboardPanels");
            {
                DashboardManager mgr = new DashboardManager(context.AppContext);
                List<Dashboard> dbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<Dashboard>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                DashboardManager sourceMgr = new DashboardManager(context.SourceAppContext);
                List<Dashboard> sourceDbData = sourceMgr.Load();
                //string sourceItemID = string.Empty;
                Dashboard targetItem = null;
                foreach (Dashboard item in sourceDbData)
                {
                    //sourceItemID = item.ID.ToString();

                    targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new Dashboard();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            if (!string.IsNullOrWhiteSpace(targetItem.AuthorizedToView))
                                targetItem.AuthorizedToView = context.GetTargetUserValue(targetItem.AuthorizedToView);


                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                    }
                }
                ULog.WriteLog($"{targetNewItemCount} DashboardPanels added");
            }

            targetNewItemCount = 0;

            ULog.WriteLog($"Updating DashboardPanelView");
            {
                DashboardPanelViewManager mgr = new DashboardPanelViewManager(context.AppContext);
                List<DashboardPanelView> dbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<DashboardPanelView>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                DashboardPanelViewManager sourceMgr = new DashboardPanelViewManager(context.SourceAppContext);
                List<DashboardPanelView> sourceDbData = sourceMgr.Load();
                string sourceItemID = string.Empty;
                DashboardPanelView targetItem = null;
                foreach (DashboardPanelView item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();

                    targetItem = dbData.FirstOrDefault(x => x.ViewName == item.ViewName);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new DashboardPanelView();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            if (!string.IsNullOrWhiteSpace(targetItem.AuthorizedToViewUsers))
                                targetItem.AuthorizedToViewUsers = context.GetTargetUserValue(targetItem.AuthorizedToViewUsers);


                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                    }
                }
                ULog.WriteLog($"{targetNewItemCount} DashboardPanelView added");
            }

            ChartTemplates(import);
        }

        private void ChartTemplates(bool import)
        {
            int targetNewItemCount = 0;

            ULog.WriteLog($"Updating ChartTemplate");
            {
                ChartTemplatesManager mgr = new ChartTemplatesManager(context.AppContext);
                List<ChartTemplate> dbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ChartTemplate>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                ChartTemplatesManager sourceMgr = new ChartTemplatesManager(context.SourceAppContext);
                List<ChartTemplate> sourceDbData = sourceMgr.Load();
                string sourceItemID = string.Empty;
                ChartTemplate targetItem = null;
                foreach (ChartTemplate item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();

                    targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new ChartTemplate();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                    }
                }
                ULog.WriteLog($"{targetNewItemCount} ChartTemplate added");
            }
        }

        public override void UpdateFileAttachments()
        {
            base.UpdateFileAttachments();

            bool import = context.IsImportEnable("Attachments");

            if (import)
                ULog.WriteLog($"Updating Attachments");
            else
                ULog.WriteLog($"Load Attachments Mapping");

            string listName = DatabaseObjects.Tables.Documents;
            MappedItemList targetMappedList = context.GetMappedList(listName);

            DocumentManager documentManager = new DocumentManager(context.AppContext);

            DocumentManager sDocumentManager = new DocumentManager(context.SourceAppContext);
            List<Document> sDocument = sDocumentManager.Load();
            if (sDocument.Count > 0)
            {
                string newId;
                sDocument.ForEach(x =>
                {
                    newId = Guid.NewGuid().ToString();

                    targetMappedList.Add(new MappedItem(x.FileID, newId));

                    x.Id = 0;
                    x.FileID = newId;
                    x.TenantID = context.AppContext.TenantID;
                    x.ModifiedBy = string.Empty;
                    x.CreatedBy = string.Empty;
                });

                documentManager.InsertItems(sDocument);
            }

            if (import)
                ULog.WriteLog($"File Attachments added");

        }

        public override void UpdateQuickTickets()
        {
            base.UpdateQuickTickets();

            bool import = context.IsImportEnable("QuickTickets");

            if (import)
                ULog.WriteLog($"Updating QuickTickets");
            else
                ULog.WriteLog($"Load QuickTickets Mapping");

            int targetNewItemCount = 0;


            TicketTemplateManager mgr = new TicketTemplateManager(context.AppContext);
            List<TicketTemplate> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<TicketTemplate>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            TicketTemplateManager sourceMgr = new TicketTemplateManager(context.SourceAppContext);
            List<TicketTemplate> sourceDbData = sourceMgr.Load();
            TicketTemplate targetItem = null;
            foreach (TicketTemplate item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.ModuleNameLookup == item.ModuleNameLookup && x.TemplateType == item.TemplateType);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new TicketTemplate();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }

                    ULog.WriteLog($"{targetNewItemCount} QuickTickets added");
                }
            }
        }

        public override void UpdateAssetVendors()
        {
            base.UpdateAssetVendors();

            bool import = context.IsImportEnable("AssetVendors");

            if (import)
                ULog.WriteLog($"Updating AssetVendors");
            else
                ULog.WriteLog($"Load AssetVendors Mapping");

            int targetNewItemCount = 0;

            ULog.WriteLog($"Updating VendorType");
            {
                VendorTypeManager mgr = new VendorTypeManager(context.AppContext);
                List<VendorType> dbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<VendorType>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.VendorType);

                VendorTypeManager sourceMgr = new VendorTypeManager(context.SourceAppContext);
                List<VendorType> sourceDbData = sourceMgr.Load();
                string sourceItemID = string.Empty;
                VendorType targetItem = null;
                foreach (VendorType item in sourceDbData)
                {
                    sourceItemID = item.id.ToString();

                    targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new VendorType();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.id == 0 || importWithUpdate)
                        {
                            item.id = targetItem.id;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            if (targetItem.id > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                    }
                    if (targetItem != null)
                        targetMappedList.Add(new MappedItem(sourceItemID, targetItem.id.ToString()));
                }
                ULog.WriteLog($"{targetNewItemCount} VendorType added");
            }

            targetNewItemCount = 0;

            ULog.WriteLog($"Updating AssetVendors");
            {
                AssetVendorViewManager mgr = new AssetVendorViewManager(context.AppContext);
                List<AssetVendor> dbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<AssetVendor>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.AssetVendors);

                AssetVendorViewManager sourceMgr = new AssetVendorViewManager(context.SourceAppContext);
                List<AssetVendor> sourceDbData = sourceMgr.Load();
                string sourceItemID = string.Empty;
                AssetVendor targetItem = null;
                foreach (AssetVendor item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();

                    targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new AssetVendor();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            targetItem.VendorTypeLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.VendorTypeLookup), "VendorTypeLookup", DatabaseObjects.Tables.VendorType) ?? 0;

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                    }
                    if (targetItem != null)
                        targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }
                ULog.WriteLog($"{targetNewItemCount} AssetVendors added");
            }
        }

        public override void UpdateAssetModels()
        {
            base.UpdateAssetModels();

            bool import = context.IsImportEnable("AssetModels");

            if (import)
                ULog.WriteLog($"Updating AssetModels");
            else
                ULog.WriteLog($"Load AssetModels Mapping");

            int targetNewItemCount = 0;

            ULog.WriteLog($"Updating AssetModels");
            {
                AssetModelViewManager mgr = new AssetModelViewManager(context.AppContext);
                List<AssetModel> dbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<AssetModel>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.AssetModels);

                AssetModelViewManager sourceMgr = new AssetModelViewManager(context.SourceAppContext);
                List<AssetModel> sourceDbData = sourceMgr.Load();
                string sourceItemID = string.Empty;
                AssetModel targetItem = null;
                foreach (AssetModel item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();

                    targetItem = dbData.FirstOrDefault(x => x.ModelName == item.ModelName && x.VendorLookup == item.VendorLookup);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new AssetModel();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            targetItem.BudgetLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.BudgetLookup), "BudgetLookup", DatabaseObjects.Tables.BudgetCategories) ?? 0;
                            targetItem.VendorLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.VendorLookup), "VendorLookup", DatabaseObjects.Tables.AssetVendors) ?? 0;

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                    }
                    if (targetItem != null)
                        targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }
                ULog.WriteLog($"{targetNewItemCount} AssetModels added");
            }
        }

        public override void UpdateFactTables()
        {
            base.UpdateFactTables();

            bool import = context.IsImportEnable("FactTables");

            if (import)
                ULog.WriteLog($"Updating FactTables");
            else
                ULog.WriteLog($"Load FactTables Mapping");

            int targetNewItemCount = 0;


            DashboardFactTableManager mgr = new DashboardFactTableManager(context.AppContext);
            List<DashboardFactTables> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<DashboardFactTables>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            DashboardFactTableManager sourceMgr = new DashboardFactTableManager(context.SourceAppContext);
            List<DashboardFactTables> sourceDbData = sourceMgr.Load();
            DashboardFactTables targetItem = null;
            foreach (DashboardFactTables item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new DashboardFactTables();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = context.AppContext.TenantID;
                        //item.ModifiedBy = targetItem.ModifiedBy;
                        //item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }

                    ULog.WriteLog($"{targetNewItemCount} FactTables added");
                }
            }
        }

        public override void UpdateMessageBoard()
        {
            base.UpdateMessageBoard();

            bool import = context.IsImportEnable("MessageBoard");

            if (import)
                ULog.WriteLog($"Updating MessageBoard");
            else
                ULog.WriteLog($"Load MessageBoard Mapping");

            int targetNewItemCount = 0;


            MessageBoardManager mgr = new MessageBoardManager(context.AppContext);
            List<MessageBoard> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<MessageBoard>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            MessageBoardManager sourceMgr = new MessageBoardManager(context.SourceAppContext);
            List<MessageBoard> sourceDbData = sourceMgr.Load();
            MessageBoard targetItem = null;
            foreach (MessageBoard item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new MessageBoard();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (!string.IsNullOrWhiteSpace(targetItem.AuthorizedToView))
                            targetItem.AuthorizedToView = context.GetTargetUserValue(targetItem.AuthorizedToView);

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }

                    ULog.WriteLog($"{targetNewItemCount} MessageBoard added");
                }
            }
        }

        public override void UpdateUserSkills()
        {
            base.UpdateUserSkills();

            bool import = context.IsImportEnable("UserSkills");

            if (import)
                ULog.WriteLog($"Updating UserSkills");
            else
                ULog.WriteLog($"Load UserSkills Mapping");

            int targetNewItemCount = 0;


            UserSkillManager mgr = new UserSkillManager(context.AppContext);
            List<UserSkills> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<UserSkills>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            MappedItemList list = context.GetMappedList(DatabaseObjects.Tables.UserSkills);

            UserSkillManager sourceMgr = new UserSkillManager(context.SourceAppContext);
            List<UserSkills> sourceDbData = sourceMgr.Load();
            UserSkills targetItem = null;
            string sourceItemID = string.Empty;
            foreach (UserSkills item in sourceDbData)
            {
                sourceItemID = item.ID.ToString();
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.CategoryName == item.CategoryName);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new UserSkills();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }
                if (targetItem != null)
                    list.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
            }
            ULog.WriteLog($"{targetNewItemCount} UserSkills added");
        }

        public override void UpdateServiceCatalogAndAgents()
        {
            base.UpdateServiceCatalogAndAgents();

            bool import = context.IsImportEnable("ServiceCatalogAndAgents");

            if (import)
                ULog.WriteLog($"Updating ServiceCatalogAndAgents");
            else
                ULog.WriteLog($"Load ServiceCatalogAndAgents Mapping");

            UpdateServiceCategories(import);
            UpdateServices(import);
            UpdateServiceSections(import);
            UpdateServiceQuestions(import);
            UpdateServiceTasksAndTickets(import);
            UpdateServiceMappings(import);
        }

        private void UpdateServiceCategories(bool import)
        {
            int targetNewItemCount = 0;

            ULog.WriteLog($"Updating ServiceCategories");
            {
                ServiceCategoryManager mgr = new ServiceCategoryManager(context.AppContext);
                List<ServiceCategory> dbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ServiceCategory>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.ServiceCategories);

                ServiceCategoryManager sourceMgr = new ServiceCategoryManager(context.SourceAppContext);
                List<ServiceCategory> sourceDbData = sourceMgr.Load();
                string sourceItemID = string.Empty;
                ServiceCategory targetItem = null;
                foreach (ServiceCategory item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();

                    targetItem = dbData.FirstOrDefault(x => x.CategoryName == item.CategoryName);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new ServiceCategory();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                    }
                    if (targetItem != null)
                        targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }
                ULog.WriteLog($"{targetNewItemCount} ServiceCategories added");
            }
        }

        private void UpdateServices(bool import)
        {
            int targetNewItemCount = 0;

            ULog.WriteLog($"Updating Services");
            {
                ServicesManager mgr = new ServicesManager(context.AppContext);
                List<Services> dbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<Services>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.Services);

                ServicesManager sourceMgr = new ServicesManager(context.SourceAppContext);
                List<Services> sourceDbData = sourceMgr.Load();
                string sourceItemID = string.Empty;
                Services targetItem = null;
                foreach (Services item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();

                    targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.ServiceCategoryType == item.ServiceCategoryType);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new Services();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            targetItem.CategoryId = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.CategoryId), "CategoryId", DatabaseObjects.Tables.ServiceCategories) ?? 0;

                            if (!string.IsNullOrWhiteSpace(targetItem.AuthorizedToView))
                                targetItem.AuthorizedToView = context.GetTargetUserValue(targetItem.AuthorizedToView);

                            if (!string.IsNullOrWhiteSpace(targetItem.OwnerUser))
                                targetItem.OwnerUser = context.GetTargetUserValue(targetItem.OwnerUser);

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                    }
                    if (targetItem != null)
                        targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }
                ULog.WriteLog($"{targetNewItemCount} Services added");
            }
        }

        private void UpdateServiceSections(bool import)
        {
            int targetNewItemCount = 0;

            ULog.WriteLog($"Updating Service Sections");
            {
                ServiceSectionManager mgr = new ServiceSectionManager(context.AppContext);
                List<ServiceSection> dbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ServiceSection>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }
                else
                {
                    ServicesManager srvMgr = new ServicesManager(context.AppContext);
                    List<Services> srvDbData = srvMgr.Load();

                    if (srvDbData != null && srvDbData.Count > 0)
                    {
                        Services services = null;
                        dbData.ForEach(x =>
                            {
                                services = srvDbData.FirstOrDefault(y => y.ID == x.ServiceID);
                                if (services != null)
                                    x.ServiceName = services.Title;
                            });
                    }
                }

                MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.ServiceSections);

                ServicesManager srvSourceMgr = new ServicesManager(context.SourceAppContext);
                List<Services> srvSourceDbData = srvSourceMgr.Load();

                ServiceSectionManager sourceMgr = new ServiceSectionManager(context.SourceAppContext);
                List<ServiceSection> sourceDbData = sourceMgr.Load();
                if (sourceDbData != null && sourceDbData.Count > 0)
                {
                    Services services = null;
                    sourceDbData.ForEach(x =>
                    {
                        services = srvSourceDbData.FirstOrDefault(y => y.ID == x.ServiceID);
                        if (services != null)
                            x.ServiceName = services.Title;
                    });
                }

                string sourceItemID = string.Empty;
                ServiceSection targetItem = null;
                foreach (ServiceSection item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();
                    targetItem = dbData.FirstOrDefault(x => x.SectionName == item.SectionName && x.ServiceName == item.ServiceName);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new ServiceSection();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            targetItem.ServiceID = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.ServiceID), "ServiceID", DatabaseObjects.Tables.Services) ?? 0;


                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                    }
                    if (targetItem != null)
                        targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }
                ULog.WriteLog($"{targetNewItemCount} Service Sections added");
            }

        }

        private void UpdateServiceQuestions(bool import)
        {
            int targetNewItemCount = 0;

            ULog.WriteLog($"Updating Service Questions");
            {
                ServiceQuestionManager mgr = new ServiceQuestionManager(context.AppContext);
                List<ServiceQuestion> dbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ServiceQuestion>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }
                else
                {
                    ServicesManager srvMgr = new ServicesManager(context.AppContext);
                    List<Services> srvDbData = srvMgr.Load();

                    ServiceSectionManager secMgr = new ServiceSectionManager(context.AppContext);
                    List<ServiceSection> dbSecData = secMgr.Load();

                    if (srvDbData != null && srvDbData.Count > 0)
                    {
                        Services services = null;
                        ServiceSection serviceSection = null;
                        dbData.ForEach(x =>
                        {
                            services = srvDbData.FirstOrDefault(y => y.ID == x.ServiceID);
                            if (services != null)
                                x.ServiceName = services.Title;

                            serviceSection = dbSecData.FirstOrDefault(y => y.ID == x.ServiceSectionID);
                            if (serviceSection != null)
                                x.ServiceName = serviceSection.Title;
                        });
                    }
                }

                MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.ServiceQuestions);

                ServicesManager srvSourceMgr = new ServicesManager(context.SourceAppContext);
                List<Services> srvSourceDbData = srvSourceMgr.Load();

                ServiceQuestionManager sourceMgr = new ServiceQuestionManager(context.SourceAppContext);
                List<ServiceQuestion> sourceDbData = sourceMgr.Load();

                ServiceSectionManager secSourceMgr = new ServiceSectionManager(context.SourceAppContext);
                List<ServiceSection> dbSourceSecData = secSourceMgr.Load();

                if (sourceDbData != null && sourceDbData.Count > 0)
                {
                    Services services = null;
                    ServiceSection serviceSection = null;
                    sourceDbData.ForEach(x =>
                    {
                        services = srvSourceDbData.FirstOrDefault(y => y.ID == x.ServiceID);
                        if (services != null)
                            x.ServiceName = services.Title;

                        serviceSection = dbSourceSecData.FirstOrDefault(y => y.ID == x.ServiceSectionID);
                        if (serviceSection != null)
                            x.ServiceSectionName = serviceSection.Title;
                    });
                }

                string sourceItemID = string.Empty;
                ServiceQuestion targetItem = null;
                foreach (ServiceQuestion item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();
                    targetItem = dbData.FirstOrDefault(x => x.QuestionTitle == item.QuestionTitle && x.ServiceSectionName == item.ServiceSectionName && x.ServiceName == item.ServiceName);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new ServiceQuestion();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            targetItem.ServiceSectionID = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.ServiceSectionID), "ServiceSectionID", DatabaseObjects.Tables.ServiceSections) ?? 0;
                            targetItem.ServiceID = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.ServiceID), "ServiceID", DatabaseObjects.Tables.Services) ?? 0;


                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                    }
                    if (targetItem != null)
                        targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }
                ULog.WriteLog($"{targetNewItemCount} Service Questions added");
            }
        }

        private void UpdateServiceTasksAndTickets(bool import)
        {
            int targetNewItemCount = 0;

            ULog.WriteLog($"Updating Service Tasks and Tickets");
            {
                ServicesManager srvMgr = new ServicesManager(context.AppContext);
                List<Services> srvDbData = srvMgr.Load();

                UGITTaskManager mgr = new UGITTaskManager(context.AppContext);
                List<string> srvIds = srvDbData.Select(x => Convert.ToString(x.ID)).Distinct().ToList();
                List<UGITTask> dbData = mgr.Load(x => srvIds.Any(y => x.TicketId.Equals(y)));
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<UGITTask>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }
                else
                {
                    if (srvDbData != null && srvDbData.Count > 0)
                    {
                        Services services = null;
                        dbData.ForEach(x =>
                        {
                            services = srvDbData.FirstOrDefault(y => Convert.ToString(y.ID) == x.TicketId);
                            if (services != null)
                                x.ParentInstance = services.Title; // Temporarily using ParentInstance variable to store Service Title
                        });
                    }
                }

                MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.ModuleTasks);

                ServicesManager srvSourceMgr = new ServicesManager(context.SourceAppContext);
                List<Services> srvSourceDbData = srvSourceMgr.Load();
                List<string> srvSourceIds = srvSourceDbData.Select(x => Convert.ToString(x.ID)).Distinct().ToList();
                UGITTaskManager sourceMgr = new UGITTaskManager(context.SourceAppContext);
                List<UGITTask> sourceDbData = sourceMgr.Load(x => srvSourceIds.Any(y => x.TicketId.Equals(y)));
                if (sourceDbData != null && sourceDbData.Count > 0)
                {
                    Services services = null;
                    sourceDbData.ForEach(x =>
                    {
                        services = srvSourceDbData.FirstOrDefault(y => Convert.ToString(y.ID) == x.TicketId);
                        if (services != null)
                            x.ParentInstance = services.Title;
                    });
                }

                string sourceItemID = string.Empty;
                UGITTask targetItem = null;
                foreach (UGITTask item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();
                    // Temporarily using ParentInstance variable to store Service Title
                    targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.ParentInstance == item.ParentInstance);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new UGITTask();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            targetItem.TicketId = Convert.ToString(context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.TicketId), "ServiceID", DatabaseObjects.Tables.Services));
                            targetItem.RequestTypeCategory = Convert.ToString(context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.RequestTypeCategory), "RequestTypeCategory", DatabaseObjects.Tables.RequestType));

                            if (!string.IsNullOrWhiteSpace(targetItem.Approver))
                                targetItem.Approver = context.GetTargetUserValue(targetItem.Approver);

                            if (!string.IsNullOrWhiteSpace(targetItem.AssignedTo))
                                targetItem.AssignedTo = context.GetTargetUserValue(targetItem.AssignedTo);

                            if (!string.IsNullOrWhiteSpace(targetItem.CompletedBy))
                                targetItem.CompletedBy = context.GetTargetUserValue(targetItem.CompletedBy);

                            if (!string.IsNullOrWhiteSpace(targetItem.Attachments))
                                context.GetTargetValue(targetItem.Attachments, DatabaseObjects.Columns.Attachments, DatabaseObjects.Tables.Documents, "Attachment");

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                    }
                    if (targetItem != null)
                        targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }
                ULog.WriteLog($"{targetNewItemCount} Service Tasks and Tickets");
            }

            UpdateServiceTaskPredecessors();
        }

        private void UpdateServiceTaskPredecessors()
        {
            ServicesManager srvMgr = new ServicesManager(context.AppContext);
            List<Services> srvDbData = srvMgr.Load();

            UGITTaskManager mgr = new UGITTaskManager(context.AppContext);
            List<UGITTask> dbData = mgr.Load(x => srvDbData.Any(y => x.TicketId.Equals(y.ID)));

            dbData.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.Predecessors))
                    x.Predecessors = Convert.ToString(context.GetTargetLookupValueOptionLong(Convert.ToString(x.Predecessors), DatabaseObjects.Columns.Predecessors, DatabaseObjects.Tables.ModuleTasks));
            });

            mgr.UpdateItems(dbData);
        }

        private void UpdateServiceMappings(bool import)
        {
            int targetNewItemCount = 0;
            Services services = null;
            ServiceQuestion serviceQuestion = null;

            ULog.WriteLog($"Updating Service Mappings");
            {
                ServicesManager srvMgr = new ServicesManager(context.AppContext);
                List<Services> srvDbData = srvMgr.Load();

                ServiceQuestionManager srvQMgr = new ServiceQuestionManager(context.AppContext);
                List<ServiceQuestion> srvQDbData = srvQMgr.Load();

                ServiceQuestionMappingManager mgr = new ServiceQuestionMappingManager(context.AppContext);
                List<ServiceQuestionMapping> dbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ServiceQuestionMapping>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }
                else
                {
                    if (srvDbData != null && srvDbData.Count > 0)
                    {

                        dbData.ForEach(x =>
                        {
                            services = srvDbData.FirstOrDefault(y => y.ID == x.ServiceID);
                            if (services != null)
                                x.ServiceName = services.Title;

                            serviceQuestion = srvQDbData.FirstOrDefault(y => y.ID == x.ServiceQuestionID);
                            if (serviceQuestion != null)
                                x.ServiceQuestionName = serviceQuestion.QuestionTitle;
                        });
                    }
                }

                ServicesManager srvSourceMgr = new ServicesManager(context.SourceAppContext);
                List<Services> srvSourceDbData = srvSourceMgr.Load();

                ServiceQuestionManager srvQSourceMgr = new ServiceQuestionManager(context.SourceAppContext);
                List<ServiceQuestion> srvQSourceDbData = srvQSourceMgr.Load();

                ServiceQuestionMappingManager sourceMgr = new ServiceQuestionMappingManager(context.SourceAppContext);
                List<ServiceQuestionMapping> sourceDbData = sourceMgr.Load();
                if (sourceDbData != null && sourceDbData.Count > 0)
                {
                    sourceDbData.ForEach(x =>
                    {
                        services = srvSourceDbData.FirstOrDefault(y => y.ID == x.ServiceID);
                        if (services != null)
                            x.ServiceName = services.Title;

                        serviceQuestion = srvQSourceDbData.FirstOrDefault(y => y.ID == x.ServiceQuestionID);
                        if (serviceQuestion != null)
                            x.ServiceQuestionName = serviceQuestion.QuestionTitle;
                    });
                }

                string sourceItemID = string.Empty;
                ServiceQuestionMapping targetItem = null;
                foreach (ServiceQuestionMapping item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();
                    targetItem = dbData.FirstOrDefault(x => x.ColumnName == item.ColumnName && x.ColumnValue == item.ColumnValue && x.ServiceName == item.ServiceName && x.ServiceQuestionName == item.ServiceQuestionName);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new ServiceQuestionMapping();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            targetItem.ServiceID = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.ServiceID), "ServiceID", DatabaseObjects.Tables.Services) ?? 0;
                            targetItem.ServiceQuestionID = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.ServiceQuestionID), "ServiceQuestionID", DatabaseObjects.Tables.ServiceQuestions) ?? null;
                            targetItem.ServiceTaskID = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.ServiceTaskID), "ServiceTaskID", DatabaseObjects.Tables.ModuleTasks) ?? null;


                            if (!string.IsNullOrWhiteSpace(targetItem.Attachments))
                                context.GetTargetValue(targetItem.Attachments, DatabaseObjects.Columns.Attachments, DatabaseObjects.Tables.Documents, "Attachment");

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                    }
                }
                ULog.WriteLog($"{targetNewItemCount} Service Mappings");
            }
        }

        public override void UpdateACRTypes()
        {
            base.UpdateACRTypes();

            bool import = context.IsImportEnable("ACRTypes");

            if (import)
                ULog.WriteLog($"Updating ACRTypes");
            else
                ULog.WriteLog($"Load ACRTypes Mapping");

            int targetNewItemCount = 0;

            ACRTypeManager mgr = new ACRTypeManager(context.AppContext);
            List<ACRType> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<ACRType>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            ACRTypeManager sourceMgr = new ACRTypeManager(context.SourceAppContext);
            List<ACRType> sourceDbData = sourceMgr.Load();
            ACRType targetItem = null;
            foreach (ACRType item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new ACRType();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} ACRTypes added");
        }

        public override void UpdateDRQRapidTypes()
        {
            base.UpdateDRQRapidTypes();

            bool import = context.IsImportEnable("DRQRapidTypes");

            if (import)
                ULog.WriteLog($"Updating DRQRapidTypes");
            else
                ULog.WriteLog($"Load DRQRapidTypes Mapping");

            int targetNewItemCount = 0;

            DrqRapidTypesManager mgr = new DrqRapidTypesManager(context.AppContext);
            List<DRQRapidType> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<DRQRapidType>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            DrqRapidTypesManager sourceMgr = new DrqRapidTypesManager(context.SourceAppContext);
            List<DRQRapidType> sourceDbData = sourceMgr.Load();
            DRQRapidType targetItem = null;
            foreach (DRQRapidType item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new DRQRapidType();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} DRQRapidTypes added");
        }

        public override void UpdateDRQSystemAreas()
        {
            base.UpdateDRQSystemAreas();

            bool import = context.IsImportEnable("DRQSystemAreas");

            if (import)
                ULog.WriteLog($"Updating DRQSystemAreas");
            else
                ULog.WriteLog($"Load DRQSystemAreas Mapping");

            int targetNewItemCount = 0;

            DrqSyetemAreaViewManager mgr = new DrqSyetemAreaViewManager(context.AppContext);
            List<DRQSystemArea> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<DRQSystemArea>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            DrqSyetemAreaViewManager sourceMgr = new DrqSyetemAreaViewManager(context.SourceAppContext);
            List<DRQSystemArea> sourceDbData = sourceMgr.Load();
            DRQSystemArea targetItem = null;
            foreach (DRQSystemArea item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new DRQSystemArea();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} DRQSystemAreas added");
        }

        public override void UpdateEnvironment()
        {
            base.UpdateEnvironment();

            bool import = context.IsImportEnable("Environment");

            if (import)
                ULog.WriteLog($"Updating Environment");
            else
                ULog.WriteLog($"Load Environment Mapping");

            int targetNewItemCount = 0;

            EnvironmentManager mgr = new EnvironmentManager(context.AppContext);
            List<UGITEnvironment> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<UGITEnvironment>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            EnvironmentManager sourceMgr = new EnvironmentManager(context.SourceAppContext);
            List<UGITEnvironment> sourceDbData = sourceMgr.Load();
            UGITEnvironment targetItem = null;
            foreach (UGITEnvironment item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new UGITEnvironment();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} Environment added");
        }

        public override void UpdateSubLocation()
        {
            base.UpdateSubLocation();

            bool import = context.IsImportEnable("SubLocation");

            if (import)
                ULog.WriteLog($"Updating SubLocation");
            else
                ULog.WriteLog($"Load SubLocation Mapping");

            int targetNewItemCount = 0;

            Location location = null;
            SubLocationManager slMgr = new SubLocationManager(context.AppContext);
            List<SubLocation> dbSlData = slMgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    slMgr.Delete(dbSlData);
                    dbSlData = new List<SubLocation>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }
            else
            {
                LocationManager locMgr = new LocationManager(context.AppContext);
                dbSlData.ForEach(x =>
                {
                    location = locMgr.LoadByID(x.LocationID);
                    if (location != null)
                        x.LocationDetails = $"{location.Country};#{location.State};#{location.Title}";
                });
            }

            LocationManager sourceLocMgr = new LocationManager(context.SourceAppContext);
            SubLocationManager sourceSlMgr = new SubLocationManager(context.SourceAppContext);
            List<SubLocation> sourceDbSlData = sourceSlMgr.Load();
            SubLocation targetItem = null;

            sourceDbSlData.ForEach(x =>
            {
                location = sourceLocMgr.LoadByID(x.LocationID);
                if (location != null)
                    x.LocationDetails = $"{location.Country};#{location.State};#{location.Title}";
            });

            foreach (SubLocation item in sourceDbSlData)
            {
                targetItem = dbSlData.FirstOrDefault(x => x.Title == item.Title && x.LocationDetails == item.LocationDetails);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new SubLocation();
                        dbSlData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        targetItem.LocationID = Convert.ToInt64(context.GetTargetValue(targetItem.LocationID, "LocationID", DatabaseObjects.Tables.Location, "Lookup") ?? 0);

                        if (targetItem.ID > 0)
                            slMgr.Update(targetItem);
                        else
                            slMgr.Insert(targetItem);
                    }
                }
            }

            ULog.WriteLog($"{targetNewItemCount} SubLocation added");
        }



        public override void UpdateProjectInitiative()
        {
            base.UpdateProjectInitiative();

            bool import = context.IsImportEnable("ProjectInitiative");

            if (import)
                ULog.WriteLog($"Updating Project Initiative");
            else
                ULog.WriteLog($"Load Project Initiative Mapping");

            int targetNewItemCount = 0;

            ULog.WriteLog($"Updating Business Strategy");

            BusinessStrategy businessStrategy = null;
            BusinessStrategyManager bsMgr = new BusinessStrategyManager(context.AppContext);
            List<BusinessStrategy> dbBsData = bsMgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    bsMgr.Delete(dbBsData);
                    dbBsData = new List<BusinessStrategy>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.BusinessStrategy);

            BusinessStrategyManager sourceBsMgr = new BusinessStrategyManager(context.SourceAppContext);
            List<BusinessStrategy> sourceDbBsData = sourceBsMgr.Load();
            BusinessStrategy targetItem = null;
            string sourceItemID = string.Empty;
            foreach (BusinessStrategy item in sourceDbBsData)
            {
                sourceItemID = item.ID.ToString();
                targetItem = dbBsData.FirstOrDefault(x => x.Title == item.Title);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new BusinessStrategy();
                        dbBsData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            bsMgr.Update(targetItem);
                        else
                            bsMgr.Insert(targetItem);
                    }
                }
                if (targetItem != null)
                    targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
            }

            ULog.WriteLog($"{targetNewItemCount} Business Strategy added");


            targetNewItemCount = 0;
            ULog.WriteLog($"Updating Project Initiative");

            ProjectInitiativeViewManager piMgr = new ProjectInitiativeViewManager(context.AppContext);
            List<ProjectInitiative> dbPiData = piMgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    piMgr.Delete(dbPiData);
                    dbPiData = new List<ProjectInitiative>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }
            else
            {
                dbPiData.ForEach(x =>
                {
                    businessStrategy = bsMgr.LoadByID(x.BusinessStrategyLookup);
                    if (businessStrategy != null)
                        x.BusinessStrategy = businessStrategy.Title;
                });
            }

            ProjectInitiativeViewManager sourcePiMgr = new ProjectInitiativeViewManager(context.SourceAppContext);
            List<ProjectInitiative> sourceDbPiData = sourcePiMgr.Load();
            ProjectInitiative targetItemPi = null;

            sourceDbPiData.ForEach(x =>
            {
                businessStrategy = sourceBsMgr.LoadByID(x.BusinessStrategyLookup);
                if (businessStrategy != null)
                    x.BusinessStrategy = businessStrategy.Title;
            });

            foreach (ProjectInitiative item in sourceDbPiData)
            {
                targetItemPi = dbPiData.FirstOrDefault(x => x.Title == item.Title && x.BusinessStrategy == item.BusinessStrategy);
                if (import)
                {
                    if (targetItemPi == null)
                    {
                        targetItemPi = new ProjectInitiative();
                        dbPiData.Add(targetItemPi);
                        targetNewItemCount++;
                    }

                    if (targetItemPi.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItemPi.ID;
                        item.TenantID = targetItemPi.TenantID;
                        item.ModifiedBy = targetItemPi.ModifiedBy;
                        item.CreatedBy = targetItemPi.CreatedBy;
                        targetItemPi = item;

                        targetItemPi.BusinessStrategyLookup = Convert.ToInt64(context.GetTargetValue(targetItemPi.BusinessStrategyLookup, DatabaseObjects.Columns.BusinessStrategyLookup, DatabaseObjects.Tables.BusinessStrategy, "Lookup") ?? 0);

                        if (targetItemPi.ID > 0)
                            piMgr.Update(targetItemPi);
                        else
                            piMgr.Insert(targetItemPi);
                    }
                }
            }

            ULog.WriteLog($"{targetNewItemCount} Project Initiative added");
        }

        public override void UpdateProjectClass()
        {
            base.UpdateProjectClass();

            bool import = context.IsImportEnable("ProjectClass");

            if (import)
                ULog.WriteLog($"Updating ProjectClass");
            else
                ULog.WriteLog($"Load ProjectClass Mapping");

            int targetNewItemCount = 0;

            ProjectClassViewManager mgr = new ProjectClassViewManager(context.AppContext);
            List<ProjectClass> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<ProjectClass>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            ProjectClassViewManager sourceMgr = new ProjectClassViewManager(context.SourceAppContext);
            List<ProjectClass> sourceDbData = sourceMgr.Load();
            ProjectClass targetItem = null;
            foreach (ProjectClass item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new ProjectClass();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} ProjectClass added");
        }

        public override void UpdateProjectStandards()
        {
            base.UpdateProjectStandards();

            bool import = context.IsImportEnable("ProjectStandards");

            if (import)
                ULog.WriteLog($"Updating Project Standards");
            else
                ULog.WriteLog($"Load Project Standards Mapping");

            int targetNewItemCount = 0;

            ProjectStandardWorkItemManager mgr = new ProjectStandardWorkItemManager(context.AppContext);
            List<ProjectStandardWorkItem> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<ProjectStandardWorkItem>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            ProjectStandardWorkItemManager sourceMgr = new ProjectStandardWorkItemManager(context.SourceAppContext);
            List<ProjectStandardWorkItem> sourceDbData = sourceMgr.Load();
            ProjectStandardWorkItem targetItem = null;
            foreach (ProjectStandardWorkItem item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new ProjectStandardWorkItem();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        targetItem.BudgetCategoryLookup = Convert.ToInt32(context.GetTargetValue(Convert.ToString(targetItem.BudgetCategoryLookup), "BudgetCategoryLookup", DatabaseObjects.Tables.BudgetCategories, "Lookup") ?? 0);

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} Project Standards added");
        }

        public override void UpdateGlobalRoles()
        {
            base.UpdateGlobalRoles();

            bool import = context.IsImportEnable("GlobalRoles");

            if (import)
                ULog.WriteLog($"Updating GlobalRoles");
            else
                ULog.WriteLog($"Load GlobalRoles Mapping");

            int targetNewItemCount = 0;

            GlobalRoleManager mgr = new GlobalRoleManager(context.AppContext);
            List<GlobalRole> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<GlobalRole>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.GlobalRole);

            GlobalRoleManager sourceMgr = new GlobalRoleManager(context.SourceAppContext);
            List<GlobalRole> sourceDbData = sourceMgr.Load();
            if (sourceDbData == null || sourceDbData.Count == 0)
                return;

            GlobalRole targetItem = null;
            string sourceItemID = string.Empty;
            foreach (GlobalRole item in sourceDbData)
            {
                sourceItemID = item.Id.ToString();

                targetItem = dbData.FirstOrDefault(x => x.Name == item.Name && x.FieldName == item.FieldName);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new GlobalRole();
                        targetItem.Id = Guid.Empty.ToString();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.Id == Guid.Empty.ToString() || importWithUpdate)
                    {
                        item.Id = targetItem.Id;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.Id != Guid.Empty.ToString())
                            mgr.Update(targetItem);
                        else
                        {
                            targetItem.Id = Guid.NewGuid().ToString();
                            mgr.Insert(targetItem);
                        }
                    }

                }
                if (targetItem != null)
                    targetMappedList.Add(new MappedItem(sourceItemID, targetItem.Id.ToString()));
            }
            ULog.WriteLog($"{targetNewItemCount} GlobalRoles added");
        }

        public override void UpdateLandingPages()
        {
            base.UpdateLandingPages();

            bool import = context.IsImportEnable("LandingPages");

            if (import)
                ULog.WriteLog($"Updating LandingPages");
            else
                ULog.WriteLog($"Load LandingPages Mapping");

            int targetNewItemCount = 0;

            LandingPagesManager mgr = new LandingPagesManager(context.AppContext);
            List<LandingPages> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<LandingPages>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            LandingPagesManager sourceMgr = new LandingPagesManager(context.SourceAppContext);
            List<LandingPages> sourceDbData = sourceMgr.Load();
            LandingPages targetItem = null;
            foreach (LandingPages item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new LandingPages();
                        targetItem.Id = Guid.Empty.ToString();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.Id == Guid.Empty.ToString() || importWithUpdate)
                    {
                        item.Id = targetItem.Id;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.Id != Guid.Empty.ToString())
                            mgr.Update(targetItem);
                        else
                        {
                            targetItem.Id = Guid.NewGuid().ToString();
                            mgr.Insert(targetItem);
                        }
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} LandingPages added");
        }

        public override void UpdateJobTitle()
        {
            base.UpdateJobTitle();

            bool import = context.IsImportEnable("JobTitle");

            if (import)
                ULog.WriteLog($"Updating JobTitle");
            else
                ULog.WriteLog($"Load JobTitle Mapping");

            int targetNewItemCount = 0;

            JobTitleManager mgr = new JobTitleManager(context.AppContext);
            List<JobTitle> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<JobTitle>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            JobTitleManager sourceMgr = new JobTitleManager(context.SourceAppContext);
            List<JobTitle> sourceDbData = sourceMgr.Load();
            JobTitle targetItem = null;
            foreach (JobTitle item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new JobTitle();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        targetItem.DepartmentId = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.DepartmentId), "DepartmentId", DatabaseObjects.Tables.Department) ?? 0;
                        targetItem.RoleId = Convert.ToString(context.GetTargetValue(targetItem.RoleId, DatabaseObjects.Columns.RoleId, DatabaseObjects.Tables.GlobalRole, "Lookup"));

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} JobTitle added");
        }

        public override void UpdateEmployeeTypes()
        {
            base.UpdateEmployeeTypes();

            bool import = context.IsImportEnable("EmployeeTypes");

            if (import)
                ULog.WriteLog($"Updating EmployeeTypes");
            else
                ULog.WriteLog($"Load EmployeeTypes Mapping");

            int targetNewItemCount = 0;

            EmployeeTypeManager mgr = new EmployeeTypeManager(context.AppContext);
            List<EmployeeTypes> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<EmployeeTypes>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            EmployeeTypeManager sourceMgr = new EmployeeTypeManager(context.SourceAppContext);
            List<EmployeeTypes> sourceDbData = sourceMgr.Load();
            EmployeeTypes targetItem = null;
            foreach (EmployeeTypes item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new EmployeeTypes();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} EmployeeTypes added");
        }

        public override void UpdateuGovernITLogs()
        {
            base.UpdateuGovernITLogs();

            bool import = context.IsImportEnable("uGovernITLogs");

            if (import)
            {
                ULog.WriteLog($"Updating uGovernITLogs");

                int targetNewItemCount = 0;

                UGITLogManager mgr = new UGITLogManager(context.AppContext);
                List<UGITLog> dbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<UGITLog>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                UGITLogManager sourceMgr = new UGITLogManager(context.SourceAppContext);
                List<UGITLog> sourceDbData = sourceMgr.Load();
                if (sourceDbData != null && sourceDbData.Count > 0)
                {
                    sourceDbData.ForEach(x => { x.ID = 0; x.TenantID = context.AppContext.TenantID; x.ModifiedBy = string.Empty; x.CreatedBy = string.Empty; });
                    //mgr.InsertItems(sourceDbData);
                    // Added below code, as CreatedDate is updated to DateTime.Now, which should not happen in this case.
                    DataTable dtLogs = UGITUtility.ToDataTable<UGITLog>(sourceDbData);
                    GetTableDataManager.bulkupload(dtLogs, DatabaseObjects.Tables.UGITLog);
                    targetNewItemCount = sourceDbData.Count;
                    ULog.WriteLog($"{targetNewItemCount} uGovernITLogs added");
                }
            }
        }

        public override void UpdateGovernanceConfiguration()
        {
            base.UpdateGovernanceConfiguration();

            bool import = context.IsImportEnable("GovernanceConfiguration");

            if (import)
                ULog.WriteLog($"Updating Governance Configuration");

            GovernanceLinkCategory(import);
            GovernanceLinkItems(import);
        }

        private void GovernanceLinkCategory(bool import)
        {
            int targetNewItemCount = 0;

            GovernanceLinkCategoryManager mgr = new GovernanceLinkCategoryManager(context.AppContext);
            List<GovernanceLinkCategory> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<GovernanceLinkCategory>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            ULog.WriteLog($"Updating Governance LinkCategory");
            MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.GovernanceLinkCategory);

            GovernanceLinkCategoryManager sourceMgr = new GovernanceLinkCategoryManager(context.SourceAppContext);
            List<GovernanceLinkCategory> sourceDbData = sourceMgr.Load();
            GovernanceLinkCategory targetItem = null;
            string sourceItemID = string.Empty;

            foreach (GovernanceLinkCategory item in sourceDbData)
            {
                sourceItemID = item.ID.ToString();

                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new GovernanceLinkCategory();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else

                            mgr.Insert(targetItem);
                    }
                }
                if (targetItem != null)
                    targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
            }
            ULog.WriteLog($"{targetNewItemCount} Governance LinkCategory added");
        }

        private void GovernanceLinkItems(bool import)
        {
            int targetNewItemCount = 0;

            GovernanceLinkItemManager mgr = new GovernanceLinkItemManager(context.AppContext);
            List<GovernanceLinkItem> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<GovernanceLinkItem>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            ULog.WriteLog($"Updating Governance Links");

            GovernanceLinkItemManager sourceMgr = new GovernanceLinkItemManager(context.SourceAppContext);
            List<GovernanceLinkItem> sourceDbData = sourceMgr.Load();
            GovernanceLinkItem targetItem = null;

            foreach (GovernanceLinkItem item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.TargetType == item.TargetType);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new GovernanceLinkItem();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        targetItem.GovernanceLinkCategoryLookup = Convert.ToInt64(context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.GovernanceLinkCategoryLookup), DatabaseObjects.Columns.GovernanceLinkCategoryLookup, DatabaseObjects.Tables.GovernanceLinkCategory) ?? 0);

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else

                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} Governance Links added");
        }

        public override void UpdateModuleMonitors()
        {
            base.UpdateModuleMonitors();

            bool import = context.IsImportEnable("ModuleMonitors");

            if (import)
                ULog.WriteLog($"Updating ModuleMonitors");

            int targetNewItemCount = 0;

            ModuleMonitorManager mgr = new ModuleMonitorManager(context.AppContext);
            List<ModuleMonitor> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<ModuleMonitor>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.ModuleMonitors);
            ModuleMonitorManager sourceMgr = new ModuleMonitorManager(context.SourceAppContext);
            List<ModuleMonitor> sourceDbData = sourceMgr.Load();
            ModuleMonitor targetItem = null;
            string sourceItemID = string.Empty;
            foreach (ModuleMonitor item in sourceDbData)
            {
                sourceItemID = item.ID.ToString();
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new ModuleMonitor();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }
                if (targetItem != null)
                    targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
            }
            ULog.WriteLog($"{targetNewItemCount} ModuleMonitors added");
        }

        public override void UpdateModuleMonitorOptions()
        {
            base.UpdateModuleMonitorOptions();

            bool import = context.IsImportEnable("ModuleMonitorOptions");

            if (import)
                ULog.WriteLog($"Updating ModuleMonitorOptions");

            int targetNewItemCount = 0;

            ModuleMonitorOptionManager mgr = new ModuleMonitorOptionManager(context.AppContext);
            List<ModuleMonitorOption> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<ModuleMonitorOption>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.ModuleMonitorOptions);

            ModuleMonitorOptionManager sourceMgr = new ModuleMonitorOptionManager(context.SourceAppContext);
            List<ModuleMonitorOption> sourceDbData = sourceMgr.Load();
            ModuleMonitorOption targetItem = null;
            string sourceItemID = string.Empty;
            foreach (ModuleMonitorOption item in sourceDbData)
            {
                sourceItemID = item.ID.ToString();
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new ModuleMonitorOption();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        targetItem.ModuleMonitorNameLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.ModuleMonitorNameLookup), DatabaseObjects.Columns.ModuleMonitorNameLookup, DatabaseObjects.Tables.ModuleMonitors) ?? 0;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }
                if (targetItem != null)
                    targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
            }
            ULog.WriteLog($"{targetNewItemCount} ModuleMonitorOptions added");
        }

        public override void UpdateProjectComplexity()
        {
            base.UpdateProjectComplexity();

            bool import = context.IsImportEnable("ProjectComplexity");

            if (import)
                ULog.WriteLog($"Updating ProjectComplexity");

            int targetNewItemCount = 0;

            ProjectComplexityManager mgr = new ProjectComplexityManager(context.AppContext);
            List<ProjectComplexity> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<ProjectComplexity>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            ProjectComplexityManager sourceMgr = new ProjectComplexityManager(context.SourceAppContext);
            List<ProjectComplexity> sourceDbData = sourceMgr.Load();
            ProjectComplexity targetItem = null;
            foreach (ProjectComplexity item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.CRMProjectComplexity == item.CRMProjectComplexity && x.MinValue == item.MinValue && x.MaxValue == item.MaxValue);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new ProjectComplexity();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }

            }
            ULog.WriteLog($"{targetNewItemCount} ProjectComplexity added");
        }

        public override void UpdateMailTokenColumnName()
        {
            base.UpdateMailTokenColumnName();

            bool import = context.IsImportEnable("MailTokenColumnName");

            if (import)
                ULog.WriteLog($"Updating MailTokenColumnName");

            int targetNewItemCount = 0;

            MailTokenColumnNameManager mgr = new MailTokenColumnNameManager(context.AppContext);
            List<MailTokenColumnName> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<MailTokenColumnName>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            MailTokenColumnNameManager sourceMgr = new MailTokenColumnNameManager(context.SourceAppContext);
            List<MailTokenColumnName> sourceDbData = sourceMgr.Load();
            MailTokenColumnName targetItem = null;
            foreach (MailTokenColumnName item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.KeyName == item.KeyName && x.KeyValue == item.KeyValue);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new MailTokenColumnName();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }

            }
            ULog.WriteLog($"{targetNewItemCount} MailTokenColumnName added");
        }

        public override void UpdateGenericStatus()
        {
            base.UpdateGenericStatus();

            bool import = context.IsImportEnable("GenericStatus");

            if (import)
                ULog.WriteLog($"Updating GenericStatus");

            int targetNewItemCount = 0;

            if (import && deleteBeforeImport)
            {
                try
                {
                    GetTableDataManager.ExecuteQuery($"Delete from {DatabaseObjects.Tables.GenericTicketStatus} where {DatabaseObjects.Columns.TenantID} = '{context.AppContext.TenantID}'");
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            DataTable sourceDbData = GetTableDataManager.GetTableData(DatabaseObjects.Tables.GenericTicketStatus, $"{DatabaseObjects.Columns.TenantID} = '{context.SourceAppContext.TenantID}'");

            foreach (DataRow item in sourceDbData.Rows)
            {
                item[DatabaseObjects.Columns.ID] = 0;
                item[DatabaseObjects.Columns.CreatedByUser] = Guid.Empty.ToString();
                item[DatabaseObjects.Columns.ModifiedByUser] = Guid.Empty.ToString();
                item[DatabaseObjects.Columns.TenantID] = context.AppContext.TenantID;
            }

            GetTableDataManager.bulkupload(sourceDbData, DatabaseObjects.Tables.GenericTicketStatus);
            targetNewItemCount = sourceDbData.Rows.Count;

            ULog.WriteLog($"{targetNewItemCount} GenericStatus added");
        }

        public override void UpdateLinkViews()
        {
            base.UpdateLinkViews();

            bool import = context.IsImportEnable("LinkViews");

            if (import)
                ULog.WriteLog($"Updating LinkViews");

            LinkView(import);
            LinkCategory(import);
            LinkItems(import);
        }

        private void LinkView(bool import)
        {
            int targetNewItemCount = 0;

            LinkViewManager mgr = new LinkViewManager(context.AppContext);
            List<LinkView> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<LinkView>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            ULog.WriteLog($"Updating LinkView");
            MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.LinkView);

            LinkViewManager sourceMgr = new LinkViewManager(context.SourceAppContext);
            List<LinkView> sourceDbData = sourceMgr.Load();
            LinkView targetItem = null;
            string sourceItemID = string.Empty;

            foreach (LinkView item in sourceDbData)
            {
                sourceItemID = item.ID.ToString();

                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.CategoryName == item.CategoryName);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new LinkView();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else

                            mgr.Insert(targetItem);
                    }
                }
                if (targetItem != null)
                    targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
            }
            ULog.WriteLog($"{targetNewItemCount} LinkView added");
        }

        private void LinkCategory(bool import)
        {
            int targetNewItemCount = 0;
            LinkView linkView = null;
            LinkCategoryManager mgr = new LinkCategoryManager(context.AppContext);
            List<LinkCategory> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<LinkCategory>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }
            else
            {
                LinkViewManager lvMgr = new LinkViewManager(context.AppContext);
                List<LinkView> dblvData = lvMgr.Load();

                if (dbData.Count > 0)
                {
                    dbData.ForEach(x =>
                    {
                        linkView = dblvData.FirstOrDefault(y => y.ID == x.LinkViewLookup);
                        if (linkView != null)
                        {
                            x.LinkViewTitle = $"{linkView.CategoryName};#{linkView.Title}";
                        }
                    });
                }
            }

            ULog.WriteLog($"Updating LinkCategory");
            MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.LinkCategory);

            LinkViewManager lvSourceMgr = new LinkViewManager(context.SourceAppContext);
            List<LinkView> dblvSourceData = lvSourceMgr.Load();

            LinkCategoryManager sourceMgr = new LinkCategoryManager(context.SourceAppContext);
            List<LinkCategory> sourceDbData = sourceMgr.Load();
            LinkCategory targetItem = null;
            string sourceItemID = string.Empty;

            if (sourceDbData.Count > 0)
            {
                sourceDbData.ForEach(x =>
                {
                    linkView = dblvSourceData.FirstOrDefault(y => y.ID == x.LinkViewLookup);
                    if (linkView != null)
                    {
                        x.LinkViewTitle = $"{linkView.CategoryName};#{linkView.Title}";
                    }
                });
            }

            foreach (LinkCategory item in sourceDbData)
            {
                sourceItemID = item.ID.ToString();

                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.LinkViewTitle == item.LinkViewTitle);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new LinkCategory();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        targetItem.LinkViewLookup = context.GetTargetLookupValueLong(targetItem.LinkViewLookup.ToString(), DatabaseObjects.Columns.LinkViewLookup, DatabaseObjects.Tables.LinkView);

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else

                            mgr.Insert(targetItem);
                    }
                }
                if (targetItem != null)
                    targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
            }
            ULog.WriteLog($"{targetNewItemCount} LinkCategory added");
        }

        private void LinkItems(bool import)
        {
            int targetNewItemCount = 0;
            LinkCategory linkCategory = null;
            LinkItemManager mgr = new LinkItemManager(context.AppContext);
            List<LinkItems> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<LinkItems>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }
            else
            {
                LinkCategoryManager lcMgr = new LinkCategoryManager(context.AppContext);
                List<LinkCategory> dbLcData = lcMgr.Load();

                if (dbData.Count > 0)
                {
                    dbData.ForEach(x =>
                    {
                        linkCategory = dbLcData.FirstOrDefault(y => y.ID == x.LinkCategoryLookup);
                        if (linkCategory != null)
                            x.LinkCategory = linkCategory.Title;
                    });
                }
            }

            ULog.WriteLog($"Updating LinkItems");

            LinkCategoryManager lcSourceMgr = new LinkCategoryManager(context.SourceAppContext);
            List<LinkCategory> dbLcSourceData = lcSourceMgr.Load();

            LinkItemManager sourceMgr = new LinkItemManager(context.SourceAppContext);
            List<LinkItems> sourceDbData = sourceMgr.Load();

            if (sourceDbData.Count > 0)
            {
                sourceDbData.ForEach(x =>
                {
                    linkCategory = dbLcSourceData.FirstOrDefault(y => y.ID == x.LinkCategoryLookup);
                    if (linkCategory != null)
                        x.LinkCategory = linkCategory.Title;
                });
            }

            LinkItems targetItem = null;
            foreach (LinkItems item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.LinkCategory == item.LinkCategory);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new LinkItems();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        targetItem.LinkCategoryLookup = context.GetTargetLookupValueLong(targetItem.LinkCategoryLookup.ToString(), DatabaseObjects.Columns.LinkCategoryLookup, DatabaseObjects.Tables.LinkCategory);

                        if (!string.IsNullOrWhiteSpace(targetItem.AuthorizedToView))
                            targetItem.AuthorizedToView = context.GetTargetUserValue(targetItem.AuthorizedToView);

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else

                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} LinkItems added");
        }

        public override void UpdateTenantScheduler()
        {
            base.UpdateTenantScheduler();

            bool import = context.IsImportEnable("TenantScheduler");

            if (import)
                ULog.WriteLog($"Updating TenantScheduler");

            int targetNewItemCount = 0;

            TenantSchedulerManager mgr = new TenantSchedulerManager(context.AppContext);
            List<TenantScheduler> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<TenantScheduler>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            ULog.WriteLog($"Updating TenantScheduler");

            TenantSchedulerManager sourceMgr = new TenantSchedulerManager(context.SourceAppContext);
            List<TenantScheduler> sourceDbData = sourceMgr.Load();
            TenantScheduler targetItem = null;

            foreach (TenantScheduler item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.JobType == item.JobType && x.CronExpression == item.CronExpression);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new TenantScheduler();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.Id == 0 || importWithUpdate)
                    {
                        item.Id = targetItem.Id;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.Id > 0)
                            mgr.Update(targetItem);
                        else

                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} TenantScheduler added");
        }

        public override void UpdateSurveyFeedback()
        {
            base.UpdateSurveyFeedback();

            bool import = context.IsImportEnable("SurveyFeedback");

            if (import)
                ULog.WriteLog($"Updating SurveyFeedback");

            int targetNewItemCount = 0;

            SurveyFeedbackManager mgr = new SurveyFeedbackManager(context.AppContext);
            List<SurveyFeedback> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<SurveyFeedback>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            ULog.WriteLog($"Inserting SurveyFeedback");

            SurveyFeedbackManager sourceMgr = new SurveyFeedbackManager(context.SourceAppContext);
            List<SurveyFeedback> sourceDbData = sourceMgr.Load();

            sourceDbData.ForEach(x =>
            {
                x.ID = 0;
                x.CreatedBy = Guid.Empty.ToString();
                x.ModifiedBy = Guid.Empty.ToString();
                x.TenantID = context.AppContext.TenantID;
            });

            mgr.InsertItems(sourceDbData);
            targetNewItemCount = sourceDbData.Count;

            ULog.WriteLog($"{targetNewItemCount} SurveyFeedback added");
        }

        public override void UpdatePhrases()
        {
            base.UpdatePhrases();

            bool import = context.IsImportEnable("Phrases");

            if (import)
                ULog.WriteLog($"Updating Phrases");

            int targetNewItemCount = 0;

            PhraseManager mgr = new PhraseManager(context.AppContext);
            List<Phrases> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<Phrases>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            ULog.WriteLog($"Updating Widgets");

            PhraseManager sourceMgr = new PhraseManager(context.SourceAppContext);
            List<Phrases> sourceDbData = sourceMgr.Load();
            Phrases targetItem = null;

            foreach (Phrases item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Phrase == item.Phrase && x.AgentType == item.AgentType);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new Phrases();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.PhraseId == 0 || importWithUpdate)
                    {
                        item.PhraseId = targetItem.PhraseId;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        targetItem.TicketType = Convert.ToString(context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.TicketType), "TicketType", DatabaseObjects.Tables.Modules));
                        targetItem.RequestType = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.RequestType), "RequestType", DatabaseObjects.Tables.RequestType);
                        targetItem.Services = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.Services), "Services", DatabaseObjects.Tables.Services);

                        if (targetItem.PhraseId > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} Phrases added");
        }

        public override void UpdateWidgets()
        {
            base.UpdateWidgets();

            bool import = context.IsImportEnable("Widgets");

            if (import)
                ULog.WriteLog($"Updating Widgets");

            int targetNewItemCount = 0;

            AgentsManager mgr = new AgentsManager(context.AppContext);
            List<Agents> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<Agents>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            ULog.WriteLog($"Updating Widgets");

            AgentsManager sourceMgr = new AgentsManager(context.SourceAppContext);
            List<Agents> sourceDbData = sourceMgr.Load();
            Agents targetItem = null;

            foreach (Agents item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.Name == item.Name && x.Control == item.Control);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new Agents();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.Id == 0 || importWithUpdate)
                    {
                        item.Id = targetItem.Id;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.Id > 0)
                            mgr.Update(targetItem);
                        else

                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} Widgets added");
        }

        public override void UpdateDocuments()
        {
            base.UpdateDocuments();

            bool import = context.IsImportEnable("DocumentManagement");
            if (import)
            {
                CopyDirectories();
                CopyDocuments();
            }
        }

        private void CopyDirectories()
        {
            MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.DMSDirectory);

            List<DMSDirectory> directories = new List<DMSDirectory>();

            using (DatabaseContext ctx = new DatabaseContext(context.SourceAppContext))
            {
                directories = ctx.DMSDirectories.Where(x => x.TenantID.EqualsIgnoreCase(context.SourceAppContext.TenantID)).ToList();
                if (directories.Count > 0)
                {
                    int oldDirId = 0;
                    foreach (var x in directories)
                    {
                        DMSDirectory directory = new DMSDirectory();

                        oldDirId = x.DirectoryId;

                        directory.DirectoryName = x.DirectoryName;
                        directory.DirectoryParentId = x.DirectoryParentId;
                        directory.AuthorId = context.GetTargetUserValue(x.AuthorId);
                        directory.CreatedByUser = context.GetTargetUserValue(x.CreatedByUser);
                        directory.CreatedOn = x.CreatedOn;
                        directory.UpdatedBy = context.GetTargetUserValue(x.UpdatedBy);
                        directory.UpdatedOn = x.UpdatedOn;
                        directory.TenantID = context.AppContext.TenantID;
                        directory.Deleted = x.Deleted;

                        ctx.DMSDirectories.Add(directory);
                        ctx.SaveChanges();

                        targetMappedList.Add(new MappedItem(Convert.ToString(oldDirId), Convert.ToString(directory.DirectoryId)));
                    }

                    directories.Clear();
                    directories = ctx.DMSDirectories.Where(x => x.TenantID.EqualsIgnoreCase(context.AppContext.TenantID)).ToList();
                    if (directories.Count > 0)
                    {
                        directories.ForEach(x =>
                        {
                            if (x.DirectoryParentId != 0)
                                x.DirectoryParentId = Convert.ToInt32(context.GetTargetLookupValueOptionLong(Convert.ToString(x.DirectoryParentId), "DirectoryParentId", DatabaseObjects.Tables.DMSDirectory));
                        });

                        ctx.DMSDirectories.UpdateRange(directories);
                        ctx.SaveChanges();
                    }
                }

            }
        }

        private void CopyDocuments()
        {
            List<DMSDocument> documents = new List<DMSDocument>();

            using (DatabaseContext ctx = new DatabaseContext(context.SourceAppContext))
            {
                documents = ctx.DMSDocument.Where(x => x.TenantID.EqualsIgnoreCase(context.SourceAppContext.TenantID)).ToList();
                if (documents.Count > 0)
                {
                    foreach (var x in documents)
                    {
                        DMSDocument doc = new DMSDocument();
                        doc.FileName = x.FileName;
                        doc.FullPath = x.FullPath.Replace(context.SourceAppContext.TenantAccountId, context.AppContext.TenantAccountId);
                        doc.TenantID = context.AppContext.TenantID;
                        doc.AuthorId = context.GetTargetUserValue(x.AuthorId);
                        doc.CreatedByUser = context.GetTargetUserValue(x.CreatedByUser);
                        doc.CreatedOn = x.CreatedOn;
                        doc.UpdatedOn = x.UpdatedOn;
                        doc.UpdatedBy = context.GetTargetUserValue(x.UpdatedBy);
                        doc.CheckOutBy = context.GetTargetUserValue(x.CheckOutBy);
                        doc.DirectoryId = Convert.ToInt32(context.GetTargetLookupValueOptionLong(Convert.ToString(x.DirectoryId), "DirectoryId", DatabaseObjects.Tables.DMSDirectory));

                        doc.CustomerId = x.CustomerId;

                        doc.CustomerId = x.CustomerId;
                        doc.FileParentId = x.FileParentId;
                        doc.StoredFileName = x.StoredFileName;
                        doc.DocumentControlID = x.DocumentControlID;
                        doc.ReviewStep = x.ReviewStep;
                        doc.Title = x.Title;
                        doc.Size = x.Size;
                        doc.Version = x.Version;
                        doc.MainVersionFileId = x.MainVersionFileId;
                        doc.Deleted = x.Deleted;


                        ctx.DMSDocument.Add(doc);
                        ctx.SaveChanges();
                    }
                }
            }

            try
            {
                string baseFolderPath = StorageUtil.GetDMSBaseFolderPath();

                if (string.IsNullOrEmpty(baseFolderPath))
                    return;

                string sourcePath = Path.Combine(baseFolderPath, "ALL", context.SourceAppContext.TenantAccountId);
                string targetPath = Path.Combine(baseFolderPath, "ALL", context.AppContext.TenantAccountId);

                if (Directory.Exists(sourcePath))
                {
                    Utility.UGITUtility.DirectoryCopy(sourcePath, targetPath, true);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

        }

        public override void RankingCriteria()
        {
            base.RankingCriteria();

            bool import = context.IsImportEnable("RankingCriteria");

            if (import)
                ULog.WriteLog($"Updating RankingCriteria");
            else
                ULog.WriteLog($"Load RankingCriteria Mapping");

            int targetNewItemCount = 0;

            RankingCriteriaManager mgr = new RankingCriteriaManager(context.AppContext);
            List<RankingCriterias> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<RankingCriterias>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            RankingCriteriaManager sourceMgr = new RankingCriteriaManager(context.SourceAppContext);
            List<RankingCriterias> sourceDbData = sourceMgr.Load();
            RankingCriterias targetItem = null;
            foreach (RankingCriterias item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.RankingCriteria == item.RankingCriteria);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new RankingCriterias();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} RankingCriteria added");
        }

        public override void LeadCriteria()
        {
            base.LeadCriteria();

            bool import = context.IsImportEnable("LeadCriteria");

            if (import)
                ULog.WriteLog($"Updating LeadCriteria");
            else
                ULog.WriteLog($"Load LeadCriteria Mapping");

            int targetNewItemCount = 0;

            LeadCriteriaManager mgr = new LeadCriteriaManager(context.AppContext);
            List<LeadCriteria> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<LeadCriteria>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            LeadCriteriaManager sourceMgr = new LeadCriteriaManager(context.SourceAppContext);
            List<LeadCriteria> sourceDbData = sourceMgr.Load();
            LeadCriteria targetItem = null;
            foreach (LeadCriteria item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Priority == item.Priority);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new LeadCriteria();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} LeadCriteria added");
        }
        public override void ChecklistTemplates()
        {
            base.ChecklistTemplates();

            bool import = context.IsImportEnable("ChecklistTemplates");

            CopyCheckListTemplates(import);
            CopyCheckListRoleTemplates(import);
            CopyCheckListTaskTemplates(import);
        }

        private void CopyCheckListTemplates(bool import)
        {
            int targetNewItemCount = 0;

            CheckListTemplatesManager mgr = new CheckListTemplatesManager(context.AppContext);
            List<CheckListTemplates> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<CheckListTemplates>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            ULog.WriteLog($"Updating CheckListTemplates");
            MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.CheckListTemplates);

            CheckListTemplatesManager sourceMgr = new CheckListTemplatesManager(context.SourceAppContext);
            List<CheckListTemplates> sourceDbData = sourceMgr.Load();
            CheckListTemplates targetItem = null;
            string sourceItemID = string.Empty;

            foreach (CheckListTemplates item in sourceDbData)
            {
                sourceItemID = item.ID.ToString();

                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.Module == item.Module);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new CheckListTemplates();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else

                            mgr.Insert(targetItem);
                    }
                }
                if (targetItem != null)
                    targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
            }
            ULog.WriteLog($"{targetNewItemCount} CheckListTemplates added");
        }

        private void CopyCheckListTaskTemplates(bool import)
        {
            int targetNewItemCount = 0;

            CheckListTaskTemplatesManager mgr = new CheckListTaskTemplatesManager(context.AppContext);
            List<CheckListTaskTemplates> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<CheckListTaskTemplates>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            ULog.WriteLog($"Updating CheckListTaskTemplates");

            CheckListTaskTemplatesManager sourceMgr = new CheckListTaskTemplatesManager(context.SourceAppContext);
            List<CheckListTaskTemplates> sourceDbData = sourceMgr.Load();
            CheckListTaskTemplates targetItem = null;
            string sourceItemID = string.Empty;

            foreach (CheckListTaskTemplates item in sourceDbData)
            {
                sourceItemID = item.ID.ToString();

                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.Module == item.Module);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new CheckListTaskTemplates();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        targetItem.CheckListTemplateLookup = context.GetTargetLookupValueLong(sourceItemID, DatabaseObjects.Columns.ID, DatabaseObjects.Tables.CheckListTemplates);

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else

                            mgr.Insert(targetItem);
                    }
                }

            }
            ULog.WriteLog($"{targetNewItemCount} CheckListTaskTemplates added");
        }

        private void CopyCheckListRoleTemplates(bool import)
        {
            int targetNewItemCount = 0;

            CheckListRoleTemplatesManager mgr = new CheckListRoleTemplatesManager(context.AppContext);
            List<CheckListRoleTemplates> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<CheckListRoleTemplates>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            ULog.WriteLog($"Updating CheckListRoleTemplates");

            CheckListRoleTemplatesManager sourceMgr = new CheckListRoleTemplatesManager(context.SourceAppContext);
            List<CheckListRoleTemplates> sourceDbData = sourceMgr.Load();
            CheckListRoleTemplates targetItem = null;
            string sourceItemID = string.Empty;

            foreach (CheckListRoleTemplates item in sourceDbData)
            {
                sourceItemID = item.ID.ToString();

                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.Module == item.Module);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new CheckListRoleTemplates();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        targetItem.CheckListTemplateLookup = context.GetTargetLookupValueLong(sourceItemID, DatabaseObjects.Columns.ID, DatabaseObjects.Tables.CheckListTemplates);

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else

                            mgr.Insert(targetItem);
                    }
                }

            }
            ULog.WriteLog($"{targetNewItemCount} CheckListRoleTemplates added");
        }

        public override void Studio()
        {
            base.Studio();

            bool import = context.IsImportEnable("Studio");

            if (import)
                ULog.WriteLog($"Updating Studio");
            else
                ULog.WriteLog($"Load Studio Mapping");

            int targetNewItemCount = 0;

            StudioManager mgr = new StudioManager(context.AppContext);
            List<Studio> dbData = mgr.Load();

            CompanyDivision division = null;
            if (dbData.Count > 0)
            {
                CompanyDivisionManager sSourceMgr = new CompanyDivisionManager(context.AppContext);
                List<CompanyDivision> sdivisiondbData = sSourceMgr.Load();

                dbData.ForEach(x =>
                {
                    division = sdivisiondbData.FirstOrDefault(y => y.ID == x.DivisionLookup);
                    if (division != null)
                        x.Division = division.Title;
                });
            }

            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<Studio>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            StudioManager sourceMgr = new StudioManager(context.SourceAppContext);
            List<Studio> sourceDbData = sourceMgr.Load();

            if (sourceDbData.Count > 0)
            {
                CompanyDivisionManager tSourceMgr = new CompanyDivisionManager(context.AppContext);
                List<CompanyDivision> tdivisiondbData = tSourceMgr.Load();

                sourceDbData.ForEach(x =>
                {
                    division = tdivisiondbData.FirstOrDefault(y => y.ID == x.DivisionLookup);
                    if (division != null)
                        x.Division = division.Title;
                });
            }

            Studio targetItem = null;
            string sourceItemID = string.Empty;

            foreach (Studio item in sourceDbData)
            {
                sourceItemID = item.ID.ToString();
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.Division == item.Division);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new Studio();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        targetItem.DivisionLookup = context.GetTargetLookupValueLong(sourceItemID, DatabaseObjects.Columns.ID, DatabaseObjects.Tables.CompanyDivisions);

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} Studio added");
        }

        public override void CRMRelationshipTypeLookup()
        {
            base.CRMRelationshipTypeLookup();

            bool import = context.IsImportEnable("CRMRelationshipTypeLookup");
            if (import)
                ULog.WriteLog($"Updating CRMRelationshipTypeLookup");

            int targetNewItemCount = 0;

            CRMRelationshipTypeManager mgr = new CRMRelationshipTypeManager(context.AppContext);
            List<CRMRelationshipType> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<CRMRelationshipType>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            CRMRelationshipTypeManager sourceMgr = new CRMRelationshipTypeManager(context.SourceAppContext);
            List<CRMRelationshipType> sourceDbData = sourceMgr.Load();
            CRMRelationshipType targetItem = null;
            foreach (CRMRelationshipType item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new CRMRelationshipType();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} CRMRelationshipType added");
        }

        public override void State()
        {
            base.State();

            bool import = context.IsImportEnable("State");

            if (import)
                ULog.WriteLog($"Updating State");
            else
                ULog.WriteLog($"Load State Mapping");

            int targetNewItemCount = 0;

            StateManager mgr = new StateManager(context.AppContext);
            List<State> dbData = mgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<State>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            StateManager sourceMgr = new StateManager(context.SourceAppContext);
            List<State> sourceDbData = sourceMgr.Load();
            State targetItem = null;
            foreach (State item in sourceDbData)
            {
                targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.StateCode == item.StateCode);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new State();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }
                }
            }
            ULog.WriteLog($"{targetNewItemCount} State added");
        }
        public override void ResourceWorkItems()
        {
            base.ResourceWorkItems();

            bool import = context.IsImportEnable("ResourceWorkItems");
            if (!import)
                return;

            ULog.WriteLog($"Updating ResourceWorkItems");
            {
                ResourceWorkItemsManager mgr = new ResourceWorkItemsManager(context.AppContext);
                List<ResourceWorkItems> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ResourceWorkItems>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ResourceWorkItems targetItem = null;

                MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.ResourceWorkItems);
                ResourceWorkItemsManager sourceMgr = new ResourceWorkItemsManager(context.SourceAppContext);
                List<ResourceWorkItems> sourceDbData = sourceMgr.Load();
                string sourceItemID = string.Empty;
                foreach (ResourceWorkItems item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();
                    targetItem = dbData.FirstOrDefault(x => x.WorkItem == item.WorkItem && x.WorkItemType == item.WorkItemType && x.SubWorkItem == item.SubWorkItem);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                    {
                        targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                        continue;
                    }
                    if (targetItem == null)
                    {
                        targetItem = new ResourceWorkItems();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;

                        if (!string.IsNullOrWhiteSpace(targetItem.Resource))
                            targetItem.Resource = context.GetTargetUserValue(targetItem.Resource);

                        if (!string.IsNullOrWhiteSpace(targetItem.Attachments))
                            targetItem.Attachments = Convert.ToString(context.GetTargetValue(targetItem.Attachments, DatabaseObjects.Columns.Attachments, DatabaseObjects.Tables.Documents, "Attachment"));
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);


                    if (targetItem != null)
                        targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }

                ULog.WriteLog($"{targetNewItemCount} ResourceWorkItems added");
            }
        }
        public override void TicketHours()
        {
            base.TicketHours();

            bool import = context.IsImportEnable("TicketHours");
            if (!import)
                return;

            ULog.WriteLog($"Updating TicketHours");
            {
                TicketHoursManager mgr = new TicketHoursManager(context.AppContext);
                List<ActualHour> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ActualHour>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ActualHour targetItem = null;

                TicketHoursManager sourceMgr = new TicketHoursManager(context.SourceAppContext);
                List<ActualHour> sourceDbData = sourceMgr.Load();
                foreach (ActualHour item in sourceDbData)
                {
                    targetItem = dbData.FirstOrDefault(x => x.TicketID == item.TicketID);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new ActualHour();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;

                        if (!string.IsNullOrWhiteSpace(targetItem.Resource))
                            targetItem.Resource = context.GetTargetUserValue(targetItem.Resource);


                        //targetItem.StandardWorkItem = 0; // Need to check later
                        //targetItem.TaskID = 0; // Need to check later
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} TicketHours added");
            }
        }
        public override void ResourceTimeSheet()
        {
            base.ResourceTimeSheet();

            bool import = context.IsImportEnable("ResourceTimeSheet");
            if (!import)
                return;

            ULog.WriteLog($"Updating ResourceTimeSheet");
            {
                ResourceWorkItems resourceWorkItems = null;
                ResourceWorkItemsManager rwiMgr = new ResourceWorkItemsManager(context.AppContext);

                ResourceTimeSheetManager mgr = new ResourceTimeSheetManager(context.AppContext);
                List<long> raIds = rwiMgr.Load().Select(x => x.ID).Distinct().ToList();
                List<ResourceTimeSheet> dbData = mgr.Load(x => raIds.Any(y => x.ResourceWorkItemLookup == y));
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ResourceTimeSheet>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }
                else
                {

                    if (dbData.Count > 0)
                    {
                        dbData.ForEach(x =>
                        {
                            resourceWorkItems = rwiMgr.LoadByID(x.ResourceWorkItemLookup);
                            if (resourceWorkItems != null)
                                //x.Title = $"{resourceWorkItems.WorkItem};#{resourceWorkItems.WorkItemType};#{resourceWorkItems.Resource}";
                                x.ResourceWorkItem = resourceWorkItems;
                        });
                    }

                }

                int targetNewItemCount = 0;
                ResourceTimeSheet targetItem = null;

                ResourceWorkItemsManager rwiMgrSource = new ResourceWorkItemsManager(context.SourceAppContext);
                List<long> raIdsSource = rwiMgrSource.Load().Select(x => x.ID).Distinct().ToList();

                ResourceTimeSheetManager sourceMgr = new ResourceTimeSheetManager(context.SourceAppContext);
                List<ResourceTimeSheet> sourceDbData = sourceMgr.Load(x => raIdsSource.Any(y => x.ResourceWorkItemLookup == y));
                if (sourceDbData.Count > 0)
                {
                    sourceDbData.ForEach(x =>
                    {
                        resourceWorkItems = rwiMgrSource.LoadByID(x.ResourceWorkItemLookup);
                        if (resourceWorkItems != null)
                            //x.Title = $"{resourceWorkItems.WorkItem};#{resourceWorkItems.WorkItemType};#{resourceWorkItems.Resource}";
                            x.ResourceWorkItem = resourceWorkItems;
                    });
                }

                foreach (ResourceTimeSheet item in sourceDbData)
                {
                    targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.WorkDate == item.WorkDate && x.ResourceWorkItem.WorkItem == item.ResourceWorkItem.WorkItem && x.ResourceWorkItem.WorkItemType == item.ResourceWorkItem.WorkItemType);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new ResourceTimeSheet();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;
                        targetItem = item;

                        targetItem.ResourceWorkItemLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.ResourceWorkItemLookup), DatabaseObjects.Columns.ResourceWorkItemLookup, DatabaseObjects.Tables.ResourceWorkItems) ?? 0;

                        if (!string.IsNullOrWhiteSpace(targetItem.Resource))
                            targetItem.Resource = context.GetTargetUserValue(targetItem.Resource);

                        if (!string.IsNullOrWhiteSpace(targetItem.Attachments))
                            targetItem.Attachments = Convert.ToString(context.GetTargetValue(targetItem.Attachments, DatabaseObjects.Columns.Attachments, DatabaseObjects.Tables.Documents, "Attachment"));
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} ResourceAllocation added");
            }

        }
        public override void ResourceAllocation()
        {
            base.ResourceAllocation();

            bool import = context.IsImportEnable("ResourceAllocation");
            if (!import)
                return;

            ULog.WriteLog($"Updating ResourceAllocation");
            {
                ResourceWorkItems resourceWorkItems = null;
                ResourceWorkItemsManager rwiMgr = new ResourceWorkItemsManager(context.AppContext);

                ResourceAllocationManager mgr = new ResourceAllocationManager(context.AppContext);
                List<long> raIds = rwiMgr.Load().Select(x => x.ID).Distinct().ToList();
                List<RResourceAllocation> dbData = mgr.Load(x => raIds.Any(y => x.ResourceWorkItemLookup == y));
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<RResourceAllocation>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }
                else
                {

                    if (dbData.Count > 0)
                    {
                        dbData.ForEach(x =>
                        {
                            resourceWorkItems = rwiMgr.LoadByID(x.ResourceWorkItemLookup);
                            if (resourceWorkItems != null)
                                x.Title = $"{resourceWorkItems.WorkItem};#{resourceWorkItems.WorkItemType}";
                        });
                    }

                }

                int targetNewItemCount = 0;
                RResourceAllocation targetItem = null;

                //MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.ResourceAllocation);
                ResourceWorkItemsManager rwiMgrSource = new ResourceWorkItemsManager(context.SourceAppContext);
                List<long> raIdsSource = rwiMgrSource.Load().Select(x => x.ID).Distinct().ToList();

                ResourceAllocationManager sourceMgr = new ResourceAllocationManager(context.SourceAppContext);
                //List<RResourceAllocation> sourceDbData = sourceMgr.Load(x => raIdsSource.Any(y => x.ResourceWorkItemLookup == y));
                List<RResourceAllocation> sourceDbData = sourceMgr.Load();
                if (sourceDbData.Count > 0)
                {
                    sourceDbData.ForEach(x =>
                    {
                        resourceWorkItems = rwiMgrSource.LoadByID(x.ResourceWorkItemLookup);
                        if (resourceWorkItems != null)
                            x.Title = $"{resourceWorkItems.WorkItem};#{resourceWorkItems.WorkItemType}";
                    });
                }

                // sourceItemID = string.Empty;
                foreach (RResourceAllocation item in sourceDbData)
                {
                    //sourceItemID = item.ID.ToString();
                    //targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.AllocationStartDate == item.AllocationStartDate && x.AllocationEndDate == item.AllocationEndDate && x.EstStartDate == item.EstStartDate && x.EstEndDate == item.EstEndDate);
                    targetItem = dbData.FirstOrDefault(x => x.TicketID == item.TicketID && x.AllocationStartDate == item.AllocationStartDate && x.AllocationEndDate == item.AllocationEndDate && x.EstStartDate == item.EstStartDate && x.EstEndDate == item.EstEndDate);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new RResourceAllocation();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;
                        item.Title = null; //clear Title, in this table, aswe are temporarily using Title field.
                        targetItem = item;

                        targetItem.ResourceWorkItemLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.ResourceWorkItemLookup), DatabaseObjects.Columns.ResourceWorkItemLookup, DatabaseObjects.Tables.ResourceWorkItems) ?? 0;

                        if (!string.IsNullOrWhiteSpace(targetItem.Resource))
                            targetItem.Resource = context.GetTargetUserValue(targetItem.Resource);

                        if (!string.IsNullOrWhiteSpace(targetItem.Attachments))
                            targetItem.Attachments = Convert.ToString(context.GetTargetValue(targetItem.Attachments, DatabaseObjects.Columns.Attachments, DatabaseObjects.Tables.Documents, "Attachment"));
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                    //if (targetItem != null)
                    //    targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }

                ULog.WriteLog($"{targetNewItemCount} ResourceAllocation added");
            }
        }
        public override void ResourceUsageSummaryMonthWise()
        {
            base.ResourceUsageSummaryMonthWise();

            bool import = context.IsImportEnable("ResourceUsageSummaryMonthWise");
            if (!import)
                return;

            ULog.WriteLog($"Updating ResourceUsageSummaryMonthWise");
            {
                ResourceUsageSummaryMonthWiseManager mgr = new ResourceUsageSummaryMonthWiseManager(context.AppContext);
                List<ResourceUsageSummaryMonthWise> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ResourceUsageSummaryMonthWise>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ResourceUsageSummaryMonthWise targetItem = null;
                ResourceUsageSummaryMonthWiseManager sourceMgr = new ResourceUsageSummaryMonthWiseManager(context.SourceAppContext);
                List<ResourceUsageSummaryMonthWise> sourceDbData = sourceMgr.Load();
                foreach (ResourceUsageSummaryMonthWise item in sourceDbData)
                {
                    targetItem = dbData.FirstOrDefault(x => x.WorkItem == item.WorkItem && x.Title == item.Title && x.MonthStartDate == item.MonthStartDate && x.WorkItemType == item.WorkItemType);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        targetItem = null;

                    if (targetItem == null)
                    {
                        targetItem = new ResourceUsageSummaryMonthWise();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;

                        if (!string.IsNullOrWhiteSpace(targetItem.ManagerLookup))
                            targetItem.ManagerLookup = context.GetTargetUserValue(targetItem.ManagerLookup);

                        if (!string.IsNullOrWhiteSpace(targetItem.Resource))
                            targetItem.Resource = context.GetTargetUserValue(targetItem.Resource);

                        targetItem.FunctionalAreaTitleLookup = Convert.ToString(context.GetTargetLookupValueLong(item.FunctionalAreaTitleLookup, DatabaseObjects.Columns.FunctionalAreaTitleLookup, DatabaseObjects.Tables.FunctionalAreas));
                        targetItem.WorkItemID = context.GetTargetLookupValueLong(item.WorkItemID, DatabaseObjects.Columns.WorkItemID, DatabaseObjects.Tables.ResourceWorkItems);
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} ResourceUsageSummaryMonthWise added");
            }
        }
        public override void ResourceUsageSummaryWeekWise()
        {
            base.ResourceUsageSummaryWeekWise();

            bool import = context.IsImportEnable("ResourceUsageSummaryWeekWise");
            if (!import)
                return;

            ULog.WriteLog($"Updating ResourceUsageSummaryWeekWise");
            {
                ResourceUsageSummaryWeekWiseManager mgr = new ResourceUsageSummaryWeekWiseManager(context.AppContext);
                List<ResourceUsageSummaryWeekWise> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ResourceUsageSummaryWeekWise>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ResourceUsageSummaryWeekWise targetItem = null;

                ResourceUsageSummaryWeekWiseManager sourceMgr = new ResourceUsageSummaryWeekWiseManager(context.SourceAppContext);
                List<ResourceUsageSummaryWeekWise> sourceDbData = sourceMgr.Load();
                foreach (ResourceUsageSummaryWeekWise item in sourceDbData)
                {
                    targetItem = dbData.FirstOrDefault(x => x.WorkItem == item.WorkItem && x.Title == item.Title && x.WeekStartDate == item.WeekStartDate && x.WorkItemType == item.WorkItemType);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        targetItem = null;

                    if (targetItem == null)
                    {
                        targetItem = new ResourceUsageSummaryWeekWise();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;

                        if (!string.IsNullOrWhiteSpace(targetItem.ManagerLookup))
                            targetItem.ManagerLookup = context.GetTargetUserValue(targetItem.ManagerLookup);

                        if (!string.IsNullOrWhiteSpace(targetItem.Resource))
                            targetItem.Resource = context.GetTargetUserValue(targetItem.Resource);

                        targetItem.FunctionalAreaTitleLookup = Convert.ToString(context.GetTargetLookupValueLong(item.FunctionalAreaTitleLookup, DatabaseObjects.Columns.FunctionalAreaTitleLookup, DatabaseObjects.Tables.FunctionalAreas));
                        targetItem.WorkItemID = context.GetTargetLookupValueLong(item.WorkItemID, DatabaseObjects.Columns.WorkItemID, DatabaseObjects.Tables.ResourceWorkItems);
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} ResourceUsageSummaryWeekWise added");
            }
        }
        public override void ResourceAllocationMonthly()
        {
            base.ResourceAllocationMonthly();

            bool import = context.IsImportEnable("ResourceAllocationMonthly");
            if (!import)
                return;

            ULog.WriteLog($"Updating ResourceAllocationMonthly");
            {
                int targetNewItemCount = 0;

                ResourceAllocationMonthlyManager mgr = new ResourceAllocationMonthlyManager(context.AppContext);
                List<ResourceAllocationMonthly> dbData = mgr.Load();
                mgr.Delete(dbData);

                ResourceAllocationMonthlyManager sourceMgr = new ResourceAllocationMonthlyManager(context.SourceAppContext);
                List<ResourceAllocationMonthly> sourceDbData = sourceMgr.Load();

                sourceDbData.ForEach(x => {
                    x.ID = 0;
                    x.CreatedBy = Guid.Empty.ToString();
                    x.ModifiedBy = Guid.Empty.ToString();
                    x.TenantID = context.AppContext.TenantID;

                    x.Resource = context.GetTargetUserValue(x.Resource);
                    x.ResourceWorkItemLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(x.ResourceWorkItemLookup), DatabaseObjects.Columns.ResourceWorkItemLookup, DatabaseObjects.Tables.ResourceWorkItems) ?? 0;
                });

                mgr.InsertItems(sourceDbData);

                ULog.WriteLog($"{targetNewItemCount} ResourceAllocationMonthly added");
            }
        }

        public override void ProjectEstimatedAllocation()
        {
            base.ProjectEstimatedAllocation();

            bool import = context.IsImportEnable("ProjectEstimatedAllocation");
            if (!import)
                return;

            ULog.WriteLog($"Updating ProjectEstimatedAllocation");
            {
                ProjectEstimatedAllocationManager mgr = new ProjectEstimatedAllocationManager(context.AppContext);
                List<ProjectEstimatedAllocation> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ProjectEstimatedAllocation>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ProjectEstimatedAllocation targetItem = null;

                //MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.ProjectEstimatedAllocation);
                ProjectEstimatedAllocationManager sourceMgr = new ProjectEstimatedAllocationManager(context.SourceAppContext);
                List<ProjectEstimatedAllocation> sourceDbData = sourceMgr.Load();
                //string sourceItemID = string.Empty;
                foreach (ProjectEstimatedAllocation item in sourceDbData)
                {
                    //sourceItemID = item.ID.ToString();
                    targetItem = dbData.FirstOrDefault(x => x.TicketId == item.TicketId && x.Type == item.Type && x.AllocationStartDate == item.AllocationStartDate && x.AllocationEndDate == item.AllocationEndDate);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new ProjectEstimatedAllocation();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;

                        if (!string.IsNullOrWhiteSpace(targetItem.AssignedTo))
                            targetItem.AssignedTo = context.GetTargetUserValue(targetItem.AssignedTo);

                        if (!string.IsNullOrWhiteSpace(targetItem.Type))
                            targetItem.Type = context.GetTargetLookupValue(item.Type, "Type", DatabaseObjects.Tables.GlobalRole);

                        if (!string.IsNullOrWhiteSpace(targetItem.Attachments))
                            targetItem.Attachments = Convert.ToString(context.GetTargetValue(targetItem.Attachments, DatabaseObjects.Columns.Attachments, DatabaseObjects.Tables.Documents, "Attachment"));

                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                    //if (targetItem != null)
                    //    targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }

                ULog.WriteLog($"{targetNewItemCount} ProjectEstimatedAllocation added");
            }
        }

        public override void ProjectPlannedAllocation()
        {
            base.ProjectPlannedAllocation();

            bool import = context.IsImportEnable("ProjectPlannedAllocation");
            if (!import)
                return;

            ULog.WriteLog($"Updating ProjectPlannedAllocation");
            {
                ProjectAllocationManager mgr = new ProjectAllocationManager(context.AppContext);
                List<ProjectAllocation> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ProjectAllocation>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ProjectAllocation targetItem = null;

                ProjectAllocationManager sourceMgr = new ProjectAllocationManager(context.SourceAppContext);
                List<ProjectAllocation> sourceDbData = sourceMgr.Load();
                foreach (ProjectAllocation item in sourceDbData)
                {
                    targetItem = dbData.FirstOrDefault(x => x.TicketID == item.TicketID && x.AllocationStartDate == item.AllocationStartDate && x.AllocationEndDate == item.AllocationEndDate);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new ProjectAllocation();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;

                        if (!string.IsNullOrWhiteSpace(targetItem.Resource))
                            targetItem.Resource = context.GetTargetUserValue(targetItem.Resource);

                        if (!string.IsNullOrWhiteSpace(targetItem.Attachments))
                            targetItem.Attachments = Convert.ToString(context.GetTargetValue(targetItem.Attachments, DatabaseObjects.Columns.Attachments, DatabaseObjects.Tables.Documents, "Attachment"));

                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} ProjectPlannedAllocation added");
            }
        }
    }
}

