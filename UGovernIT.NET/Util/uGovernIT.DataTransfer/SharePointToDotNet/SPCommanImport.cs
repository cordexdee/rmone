using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Owin;
using Microsoft.SharePoint.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.DataTransfer.Infratructure;
using uGovernIT.Manager;
using uGovernIT.Manager.Core;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.Common;
using uGovernIT.Utility.Entities.DB;
using uGovernIT.Utility.Entities.DB.uGovernIT.Utility;
using uGovernIT.Utility.Entities.DMSDB;
using ClientOM = Microsoft.SharePoint.Client;
using Constants = uGovernIT.Utility.Constants;
using DefaultData = uGovernIT.DefaultConfig;

namespace uGovernIT.DataTransfer.SharePointToDotNet
{
    public class SPCommanImport : CommanEntity
    {
        bool importWithUpdate;
        bool deleteBeforeImport;
        Hashtable htFunctionalArea
            = new Hashtable();
        //bool importAttachment;
        SPImportContext context;
        public SPCommanImport(SPImportContext context) : base(context)
        {
            this.context = context;
            importWithUpdate = false;
            if (JsonConfig.Config.Global.commonsettings.importwithupdate)
                importWithUpdate = true;

            deleteBeforeImport = false;
            if (JsonConfig.Config.Global.commonsettings.deletebeforeimport)
                deleteBeforeImport = true;

            //importAttachment = this.context.ImportAttachmentEnable();
        }
        public override void UpdateLocation()
        {
            base.UpdateLocation();

            bool import = context.IsImportEnable("Location");

            if (import)
                ULog.WriteLog($"Updating locations");
            else
                ULog.WriteLog($"Load Location Mapping");

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
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

                string listName = SPDatabaseObjects.Lists.Location;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;

                string region = string.Empty, country = string.Empty, state = string.Empty, title = string.Empty;
                Location targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        region = Convert.ToString(item[SPDatabaseObjects.Columns.UGITRegion]);
                        country = Convert.ToString(item[SPDatabaseObjects.Columns.UGITCountry]);
                        state = Convert.ToString(item[SPDatabaseObjects.Columns.UGITState]);
                        title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                        targetItem = null;
                        //targetItem = dbData.FirstOrDefault(x => x.Region == region && x.Country == country && x.State == state && x.Title == title);
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
                                targetItem.Title = title;
                                targetItem.Region = region;
                                targetItem.Country = country;
                                targetItem.State = state;
                                targetItem.LocationDescription = Convert.ToString(item[SPDatabaseObjects.Columns.LocationDescription]);
                                targetItem.Deleted = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                                ClientOM.FieldUserValue Createduser = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (Createduser != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(Createduser.LookupId));

                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));

                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                                mgr.AddOrUpdate(targetItem);
                            }
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} Locations added");
            }
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
            string listName = SPDatabaseObjects.Lists.Company;
            MappedItemList cmplist = context.GetMappedList(listName);

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
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

                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;
                string company = string.Empty;
                Company targetItem = null;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, string.Empty, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {

                        company = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                        targetItem = companydbData.FirstOrDefault(x => x.Title == company);
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
                                targetItem.Title = company;
                                targetItem.GLCode = Convert.ToString(item[SPDatabaseObjects.Columns.GLCode]);
                                targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                                targetItem.Deleted = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);


                        }

                        if (targetItem != null)
                            cmplist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} companies added");
            }

            if (import)
                ULog.WriteLog($"Updating divisions");
            else
                ULog.WriteLog($"Loading divisions");
            List<CompanyDivision> divisiondbData = new List<CompanyDivision>();
            listName = SPDatabaseObjects.Lists.CompanyDivisions;
            MappedItemList divisionlist = context.GetMappedList(listName);

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
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


                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;
                string division = string.Empty;
                CompanyDivision targetItem = null;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, string.Empty, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        division = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                        ClientOM.FieldLookupValue sCompanyLookup = item[SPDatabaseObjects.Columns.CompanyTitleLookup] as ClientOM.FieldLookupValue;

                        string targetCompanyID = cmplist.GetTargetID(sourceid: sCompanyLookup.LookupId.ToString());
                        if (string.IsNullOrWhiteSpace(targetCompanyID))
                            targetItem = divisiondbData.FirstOrDefault(x => x.Title == division && !x.CompanyIdLookup.HasValue);
                        else
                            targetItem = divisiondbData.FirstOrDefault(x => x.Title == division && x.CompanyIdLookup.GetValueOrDefault() == UGITUtility.StringToLong(targetCompanyID));

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
                                targetItem.GLCode = Convert.ToString(item[SPDatabaseObjects.Columns.GLCode]); ;
                                targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                                targetItem.Deleted = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                                targetItem.CompanyIdLookup = null;
                                if (!string.IsNullOrWhiteSpace(targetCompanyID))
                                {
                                    targetItem.CompanyIdLookup = UGITUtility.StringToLong(targetCompanyID);
                                }
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);




                        }
                        if (targetItem != null)
                            divisionlist.Add(new MappedItem(Convert.ToString(item["Id"]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} divisions added");
            }

            if (import)
                ULog.WriteLog($"Updating departments");
            else
                ULog.WriteLog($"Load departments mapping");
            List<Department> departmentdbData = new List<Department>();
            listName = "Department";
            MappedItemList departmentlist = context.GetMappedList(listName);

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
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

                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;
                string company = string.Empty;
                string division = string.Empty;
                string department = string.Empty;

                Department targetItem = null;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, string.Empty, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        department = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                        ClientOM.FieldLookupValue sCompanyLookup = item[SPDatabaseObjects.Columns.CompanyTitleLookup] as ClientOM.FieldLookupValue;
                        ClientOM.FieldLookupValue sDivisionLookup = item[SPDatabaseObjects.Columns.DivisionLookup] as ClientOM.FieldLookupValue;

                        string targetCompanyID = string.Empty;
                        if (sCompanyLookup != null && sCompanyLookup.LookupId > 0)
                            targetCompanyID = cmplist.GetTargetID(sCompanyLookup.LookupId.ToString());

                        string targetDivisionID = string.Empty;
                        if (sDivisionLookup != null && sDivisionLookup.LookupId > 0)
                            targetDivisionID = cmplist.GetTargetID(sDivisionLookup.LookupId.ToString());

                        if (string.IsNullOrWhiteSpace(targetCompanyID) && string.IsNullOrWhiteSpace(targetDivisionID))
                            targetItem = departmentdbData.FirstOrDefault(x => x.Title == department && !x.CompanyIdLookup.HasValue && !x.DivisionIdLookup.HasValue);
                        else if (!string.IsNullOrWhiteSpace(targetCompanyID) && !string.IsNullOrWhiteSpace(targetDivisionID))
                            targetItem = departmentdbData.FirstOrDefault(x => x.Title == department && x.CompanyIdLookup.HasValue && x.DivisionIdLookup.HasValue
                            && x.CompanyIdLookup.GetValueOrDefault() == UGITUtility.StringToLong(targetCompanyID) && x.DivisionIdLookup.GetValueOrDefault() == UGITUtility.StringToLong(targetDivisionID));
                        else if (!string.IsNullOrWhiteSpace(targetCompanyID))
                            targetItem = departmentdbData.FirstOrDefault(x => x.Title == department && x.CompanyIdLookup.HasValue && !x.DivisionIdLookup.HasValue && x.CompanyIdLookup.GetValueOrDefault() == UGITUtility.StringToLong(targetCompanyID));
                        else if (!string.IsNullOrWhiteSpace(targetDivisionID))
                            targetItem = departmentdbData.FirstOrDefault(x => x.Title == department && !x.CompanyIdLookup.HasValue && x.DivisionIdLookup.HasValue && x.DivisionIdLookup.GetValueOrDefault() == UGITUtility.StringToLong(targetDivisionID));

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
                                targetItem.Title = department;
                                targetItem.GLCode = Convert.ToString(item[SPDatabaseObjects.Columns.GLCode]); ;
                                targetItem.DepartmentDescription = Convert.ToString(item[SPDatabaseObjects.Columns.DepartmentDescription]);
                                targetItem.Deleted = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
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
                            departmentlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} departments added");
            }
        }
        public override void UpdateUsersAndRoles()
        {
            base.UpdateUsersAndRoles();

            bool import = context.IsImportEnable("UsersAndRoles");

            if (import)
                ULog.WriteLog($"Updating Users");
            else
                ULog.WriteLog($"Load users and roles mapping");
            try
            {
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    UserProfileManager mgr = new UserProfileManager(context.AppContext);
                    UserRoleManager roleMgr = new UserRoleManager(context.AppContext);
                    List<UserProfile> dbData = mgr.GetUsersProfileWithGroup();
                    ClientOM.List userList = spContext.Web.SiteUserInfoList;
                    MappedItemList userMappedlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    MappedItemList locationMappedList = context.GetMappedList(SPDatabaseObjects.Lists.Location);
                    MappedItemList UserSkillsMappedList = context.GetMappedList(SPDatabaseObjects.Lists.UserSkills);
                    MappedItemList FunctionAreaMappedList = context.GetMappedList(SPDatabaseObjects.Lists.FunctionalAreas);
                    MappedItemList DepartmentList = context.GetMappedList(SPDatabaseObjects.Lists.Department);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    int targetNewItemCount = 0;
                    int targetNewGroupCount = 0;
                    string userName = string.Empty;
                    string managername = string.Empty;
                    string functionalArea = string.Empty;
                    long DepartmentId = 0, targetLocationID = 0;
                    UserProfile targetItem = null;
                    Hashtable htUserManager = new Hashtable();

                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, userList, string.Empty, position);
                        ClientOM.ContentType personContentType = userList.ContentTypes.FirstOrDefault(x => x.Name == "Person");
                        string userContentTypeID = personContentType.StringId;
                        position = collection.ListItemCollectionPosition;
                        foreach (ClientOM.ListItem item in collection)
                        {
                            userName = UGITUtility.ConvertStringToList(Convert.ToString(item["Name"]), "\\").LastOrDefault();
                            if (Convert.ToString(item["ContentTypeId"]) == userContentTypeID)
                            {
                                targetItem = dbData.FirstOrDefault(x => x.UserName == userName && !x.isRole);
                                UserProfile IsduplicateProfile = mgr.GetUserByBothUserNameandDisplayName(userName);
                                if (IsduplicateProfile != null)
                                    userName = userName + targetNewItemCount;
                                if (import)
                                {
                                    if (targetItem == null)
                                    {
                                        targetItem = new UserProfile();
                                    }

                                    if (string.IsNullOrWhiteSpace(targetItem.Id) || importWithUpdate)
                                    {
                                        //userName = UGITUtility.ConvertStringToList(Convert.ToString(item["Name"]), "\\").LastOrDefault();
                                        targetItem.UserName = userName;
                                        targetItem.LoginName = userName;
                                        targetItem.Name = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                        targetItem.Email = Convert.ToString(item[SPDatabaseObjects.Columns.EMail]);
                                        targetItem.PhoneNumber = Convert.ToString(item[SPDatabaseObjects.Columns.MobilePhone]);
                                        DepartmentId = 0;
                                        if (DepartmentList != null && item[SPDatabaseObjects.Columns.DepartmentLookup] != null)
                                            DepartmentId = UGITUtility.StringToLong(DepartmentList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.DepartmentLookup]).LookupId.ToString()));
                                        if (DepartmentId > 0)
                                            targetItem.Department = Convert.ToString(DepartmentId);

                                        targetLocationID = 0;
                                        if (locationMappedList != null && item[SPDatabaseObjects.Columns.LocationLookup] != null)
                                            targetLocationID = UGITUtility.StringToLong(locationMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.LocationLookup]).LookupId.ToString()));
                                        if (targetLocationID > 0)
                                            targetItem.Location = Convert.ToString(targetLocationID);
                                        ClientOM.FieldLookupValue[] skills = item[SPDatabaseObjects.Columns.UserSkillMultiLookup] as ClientOM.FieldLookupValue[];
                                        if (skills != null && skills.Length > 0)
                                        {
                                            targetItem.Skills = UserSkillsMappedList.GetTargetIDs(skills.Select(x => x.LookupId.ToString()).ToList());
                                        }
                                        targetItem.IsManager = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsManager]);
                                        targetItem.IsConsultant = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsConsultant]);
                                        targetItem.IsIT = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IT]);
                                        targetItem.DisableWorkflowNotifications = Convert.ToBoolean(item[SPDatabaseObjects.Columns.DisableWorkflowNotifications]);
                                        targetItem.DeskLocation = Convert.ToString(item[SPDatabaseObjects.Columns.DeskLocation]);
                                        targetItem.JobProfile = Convert.ToString(item[SPDatabaseObjects.Columns.JobTitle]);
                                        targetItem.HourlyRate = Convert.ToInt32(item[SPDatabaseObjects.Columns.ResourceHourlyRate]);
                                        targetItem.NotificationEmail= Convert.ToString(item[SPDatabaseObjects.Columns.NotificationEmail]);
                                        ClientOM.FieldLookupValue EmployeeType = item[SPDatabaseObjects.Columns.EmployeeTypeLookup] as ClientOM.FieldLookupValue;
                                        if(EmployeeType != null)
                                               targetItem.EmployeeType = Convert.ToString(EmployeeType.LookupValue);
                                        targetItem.EmployeeId = Convert.ToString(item[SPDatabaseObjects.Columns.EmployeeID]);
                                        targetItem.EnableOutofOffice = Convert.ToBoolean(item[SPDatabaseObjects.Columns.EnableOutofOffice]);
                                        if (item[SPDatabaseObjects.Columns.WorkingHoursStart] != null)
                                            targetItem.WorkingHoursStart = Convert.ToDateTime(item[SPDatabaseObjects.Columns.WorkingHoursStart]);
                                        if (item[SPDatabaseObjects.Columns.WorkingHoursEnd] != null)
                                            targetItem.WorkingHoursStart = Convert.ToDateTime(item[SPDatabaseObjects.Columns.WorkingHoursEnd]);
                                        if (item[SPDatabaseObjects.Columns.WorkingHoursEnd] !=null)
                                            targetItem.UGITStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.UGITStartDate]);
                                        if (item[SPDatabaseObjects.Columns.UGITEndDate] != null)
                                            targetItem.UGITEndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.UGITEndDate]);
                                        targetItem.Enabled = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.Enabled]);
                                        targetItem.TenantID = context.Tenant.TenantID.ToString();
                                        targetItem.PasswordHash = mgr.GeneratePassword();
                                        if (context.AppContext.TenantID.ToString() != context.Tenant.TenantID.ToString())
                                            context.AppContext.TenantID = context.Tenant.TenantID.ToString();
                                        ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.ManagerLookup] as ClientOM.FieldUserValue;
                                        managername = string.Empty;
                                        if (users != null)
                                        {
                                            if (!string.IsNullOrEmpty(users.LookupValue))
                                                managername = users.LookupValue;
                                        }
                                        functionalArea = string.Empty;
                                        ClientOM.FieldLookupValue functArea = item[SPDatabaseObjects.Columns.FunctionalAreaLookup] as ClientOM.FieldLookupValue;
                                        if (functArea != null)
                                        {
                                            if (!string.IsNullOrEmpty(functArea.LookupValue))
                                                functionalArea = functArea.LookupValue;
                                        }
                                    }
                                    if (string.IsNullOrWhiteSpace(targetItem.Id))
                                    {
                                        IdentityResult result = mgr.Create(targetItem, "Password@123");
                                        if (result.Succeeded)
                                        {
                                            targetNewItemCount++;
                                            if (!string.IsNullOrEmpty(managername))
                                                htUserManager.Add(targetItem.Id, managername);
                                            if (!string.IsNullOrEmpty(functionalArea))
                                                htFunctionalArea.Add(targetItem.Id, functionalArea);
                                        }
                                        else
                                            ULog.WriteLog($"{targetItem.LoginName} not created, errors: {string.Join("; ", result.Errors)}");
                                    }
                                    else
                                        mgr.Update(targetItem);
                                }

                                if (targetItem != null)
                                    userMappedlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.Id));
                            }
                        }
                    } while (position != null);

                    var LstUserProfile = mgr.GetUsersProfile().Where(x => x.TenantID.Equals(context.AppContext.TenantID, StringComparison.InvariantCultureIgnoreCase)).Select(x => new { x.Id, x.Name, x.UserName }).ToList();
                    UserProfile user = new UserProfile();
                    foreach (DictionaryEntry item in htUserManager)
                    {
                        user = mgr.GetUserById(Convert.ToString(item.Key));
                        if (user != null)
                        {
                            var manager = LstUserProfile.FirstOrDefault(x => x.Name.Equals(Convert.ToString(item.Value), StringComparison.InvariantCultureIgnoreCase));
                            if (manager != null)
                            {
                                user.ManagerID = manager.Id;
                                mgr.Update(user);
                            }
                        }
                    }


                    if (import)
                    {
                        ULog.WriteLog($"{targetNewItemCount} users added");
                        ULog.WriteLog($"Update Roles and mapping");
                    }

                    spContext.Load(spContext.Web.SiteGroups);
                    spContext.ExecuteQuery();
                    // Below code added as Above code is resetting  context.AppContext.TenantID
                    if (context.AppContext.TenantID.ToString() != context.Tenant.TenantID.ToString())
                    {
                        context.AppContext = ApplicationContext.CreateContext(JsonConfig.Config.Global.permission.target.dbconnection, JsonConfig.Config.Global.permission.target.commondbconnection, JsonConfig.Config.Global.permission.target.tenantid); ;
                    }
                    List<Role> roleList = roleMgr.Load();
                    //ManagerBase<UserRole> userRoleMgr = new ManagerBase<UserRole>(context.AppContext);
                    StoreBase<UserRole> userRoleMgr = new StoreBase<UserRole>(context.AppContext);
                    foreach (ClientOM.Group spGroup in spContext.Web.SiteGroups)
                    {

                        if (spGroup.LoginName == "c:0(.s|true")
                            continue;

                        Role targetRole = roleMgr.Get(x => x.Title.ToLower() == spGroup.Title.ToLower());
                        if (import)
                        {
                            if (targetRole == null)
                            {
                                targetRole = new Role();
                                targetNewGroupCount++;
                            }

                            if (!string.IsNullOrWhiteSpace(targetRole.Id) || importWithUpdate)
                            {
                                string groupName = spGroup.LoginName.Replace(" ", string.Empty);
                                targetRole.Name = groupName;
                                targetRole.Title = spGroup.Title;
                                targetRole.TenantID = context.AppContext.TenantID;
                                targetRole.CreatedBy = context.Tenant.CreatedByUser;
                                roleMgr.AddOrUpdate(targetRole);
                            }
                        }

                        if (targetRole != null)
                            userMappedlist.Add(new MappedItem(spGroup.Id.ToString(), targetRole.Id));

                        if (import)
                        {
                            List<UserRole> userRoleList = userRoleMgr.Load(x => x.TenantID == context.AppContext.TenantID && x.RoleId == targetRole.Id);
                            spContext.Load(spGroup.Users);
                            spContext.ExecuteQuery();

                            string targetUserID = string.Empty;
                            foreach (ClientOM.User spUser in spGroup.Users)
                            {
                                targetUserID = userMappedlist.GetTargetID(spUser.Id.ToString());
                                if (string.IsNullOrEmpty(targetUserID))
                                {
                                    ULog.WriteLog($"{spUser.LoginName} user not found in target so not added in {spGroup.LoginName} group");
                                    continue;
                                }

                                try
                                {
                                    if (!userRoleList.Exists(x => x.UserId.ToLower() == targetUserID.ToLower()))
                                    {
                                        mgr.AddToRole(targetUserID, targetRole.Name);
                                    }
                                }
                                catch
                                {
                                    //crash happens when mapping is already exist
                                }
                            }
                        }
                    }

                    if (import)
                    {
                        ULog.WriteLog($"{targetNewGroupCount} groups added");
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex.ToString());
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

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                FunctionalAreasManager mgr = new FunctionalAreasManager(context.AppContext);
                UserProfileManager pmgr = new UserProfileManager(context.AppContext);
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

                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;
                string functoinalArea = string.Empty;
                FunctionalArea targetItem = null;
                ClientOM.FieldUserValue[] sOwner = null;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    MappedItemList departmentList = context.GetMappedList("Department");
                    MappedItemList userlst = context.GetMappedList("User Information List");

                    foreach (ClientOM.ListItem item in collection)
                    {
                        functoinalArea = Convert.ToString(item["Title"]);
                        ClientOM.FieldLookupValue sDepartmentLookup = item["DepartmentLookup"] as ClientOM.FieldLookupValue;
                        if (item["UGITOwner"] != null)
                            sOwner = item[SPDatabaseObjects.Columns.UGITOwner] as ClientOM.FieldUserValue[];
                        targetItem = targetdbData.FirstOrDefault(x => x.Title == functoinalArea);
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
                                targetItem.Title = functoinalArea;
                                targetItem.DepartmentLookup = null;
                                if (departmentList != null && sDepartmentLookup != null && sDepartmentLookup.LookupId > 0)
                                {
                                    string sourceID = departmentList.GetTargetID(sDepartmentLookup.LookupId.ToString());
                                    if (!string.IsNullOrWhiteSpace(sourceID) && UGITUtility.StringToLong(sourceID) > 0)
                                        targetItem.DepartmentLookup = UGITUtility.StringToLong(sourceID);
                                }
                                if (sOwner != null)
                                {
                                    string sourceID = userlst.GetTargetIDs(sOwner.Select(x => x.LookupId.ToString()).ToList());
                                    if (!string.IsNullOrWhiteSpace(sourceID))
                                        targetItem.Owner = sourceID;
                                }
                                targetItem.FunctionalAreaDescription = Convert.ToString(item["FunctionalAreaDescription"]);
                                targetItem.Deleted = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            maplist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                var LstUserProfile = pmgr.GetUsersProfile().Where(x => x.TenantID.Equals(context.AppContext.TenantID, StringComparison.InvariantCultureIgnoreCase)).Select(x => new { x.Id, x.Name, x.UserName }).ToList();
                UserProfile _user = new UserProfile();
                List<FunctionalArea> _targetdbData = new List<FunctionalArea>();
                FunctionalAreasManager _mgr = new FunctionalAreasManager(context.AppContext);
                _targetdbData = _mgr.Load();
                foreach (DictionaryEntry item in htFunctionalArea)
                {
                    _user = pmgr.GetUserById(Convert.ToString(item.Key));
                    if (_user != null)
                    {
                        var funcArea = targetdbData.FirstOrDefault(x => x.Title.Equals(Convert.ToString(item.Value), StringComparison.InvariantCultureIgnoreCase));
                        if (funcArea != null)
                        {
                            _user.FunctionalArea = Convert.ToInt32(funcArea.ID);
                            pmgr.Update(_user);
                        }
                    }
                }

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} functional areas added");
            }
        }
        public override void UpdateModules()
        {
            base.UpdateModules();
            bool import = context.IsImportEnable("Modules");

            if (import)
                ULog.WriteLog($"Updating modules");
            else
                ULog.WriteLog($"Load modules mapping");

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                ModuleViewManager mgr = new ModuleViewManager(context.AppContext);
                List<UGITModule> dbData = mgr.Load();
                string listName = SPDatabaseObjects.Lists.Modules;

                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();

                List<string> targetNewItems = new List<string>();
                List<string> targetUpdateItems = new List<string>();

                string modulename = string.Empty;
                UGITModule targetItem = null;
                MappedItemList userMappedList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, string.Empty, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        modulename = Convert.ToString(item[SPDatabaseObjects.Columns.ModuleName]);

                        targetItem = dbData.FirstOrDefault(x => x.ModuleName == modulename);
                        try
                        {
                            if (import)
                            {
                                if (targetItem == null)
                                {
                                    targetItem = new UGITModule();
                                }

                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    targetItem.ModuleName = modulename;
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    targetItem.ModuleDescription = Convert.ToString(item[SPDatabaseObjects.Columns.ModuleDescription]);
                                    targetItem.LastSequence = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.LastSequence]);
                                    targetItem.LastSequenceDate = UGITUtility.StringToDateTime(item[SPDatabaseObjects.Columns.LastSequenceDate]);
                                    targetItem.ModuleAutoApprove = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ModuleAutoApprove]);
                                    targetItem.HoldMaxStage = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.ModuleHoldMaxStage]);
                                    if (modulename == ModuleNames.SVC)
                                    {
                                        targetItem.ModuleTable = DatabaseObjects.Tables.SVCRequests;
                                    }
                                    else if (modulename == ModuleNames.APP)
                                    {
                                        targetItem.ModuleTable = DatabaseObjects.Tables.Applications;
                                    }
                                    else if (modulename == ModuleNames.CMDB)
                                    {
                                        targetItem.ModuleTable = DatabaseObjects.Tables.Assets;
                                    }
                                    else if (modulename == ModuleNames.DRQ)
                                    {
                                        targetItem.ModuleTable = ModuleNames.DRQ;
                                    }
                                    else if (modulename == ModuleNames.WIK)
                                    {
                                        targetItem.ModuleTable = DatabaseObjects.Tables.WikiArticles;
                                        targetItem.ModuleName = ModuleNames.WIKI;
                                    }
                                    else if (modulename == ModuleNames.CMT)
                                    {
                                        targetItem.ModuleTable = DatabaseObjects.Tables.Contracts;
                                    }
                                    else if (modulename == ModuleNames.ITG)
                                    {
                                        targetItem.ModuleTable = DatabaseObjects.Tables.ITGovernance;
                                    }
                                    else
                                    {
                                        targetItem.ModuleTable = modulename;
                                    }

                                    string url = Convert.ToString(item[SPDatabaseObjects.Columns.StaticModulePagePath]);
                                    if (!string.IsNullOrWhiteSpace(url))
                                    {
                                        url = $"/pages/{Path.GetFileNameWithoutExtension(url.Split('/').LastOrDefault())}";
                                        targetItem.ModuleRelativePagePath = url;
                                    }

                                    url = Convert.ToString(item[SPDatabaseObjects.Columns.ModuleRelativePagePath]);
                                    if (!string.IsNullOrWhiteSpace(url))
                                    {
                                        url = $"/pages/{Path.GetFileNameWithoutExtension(url.Split('/').LastOrDefault())}";
                                        targetItem.StaticModulePagePath = url;
                                    }

                                    ModuleType mType = ModuleType.All;
                                    if (Enum.TryParse(Convert.ToString(item[SPDatabaseObjects.Columns.ModuleType]), out mType))
                                        targetItem.ModuleType = mType;

                                    targetItem.AllowDraftMode = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.AllowDraftMode]);
                                    targetItem.EnableEventReceivers = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.EnableEventReceivers]);
                                    targetItem.ReloadCache = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ReloadCache]);
                                    targetItem.ItemOrder = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.ItemOrder]);
                                    targetItem.EnableModule = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.EnableModule]) == false ? true : UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.EnableModule]);
                                    targetItem.CustomProperties = Convert.ToString(item[SPDatabaseObjects.Columns.CustomProperties]);
                                    targetItem.ShortName = Convert.ToString(item[SPDatabaseObjects.Columns.UGITShortName]);
                                    targetItem.CategoryName = Convert.ToString(item[SPDatabaseObjects.Columns.CategoryName]);
                                    targetItem.ShowComment = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ShowComment]);
                                    targetItem.SyncAppsToRequestType = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.SyncAppsToRequestType]);
                                    targetItem.EnableCache = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.EnableCache]);
                                    targetItem.NavigationUrl = Convert.ToString(item[SPDatabaseObjects.Columns.NavigationUrl]);
                                    targetItem.EnableWorkflow = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.EnableWorkflow]);
                                    targetItem.EnableLayout = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.EnableLayout]);
                                    targetItem.StoreEmails = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.StoreTicketEmails]);
                                    targetItem.UseInGlobalSearch = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.UseInGlobalSearch]);
                                    targetItem.EnableQuick = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.EnableQuickTicket]);
                                    targetItem.AutoCreateDocumentLibrary = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.AutoCreateDocumentLibrary]);
                                    targetItem.DisableNewConfirmation = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.DisableNewTicketConfirmation]);
                                    targetItem.OwnerBindingChoice = Convert.ToString(item[SPDatabaseObjects.Columns.TicketOwnerBinding]);
                                    targetItem.AllowChangeType = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.AllowChangeTicketType]);
                                    targetItem.AllowBatchEditing = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.AllowBatchEditing]);
                                    targetItem.AllowBatchCreate = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.AllowBatchCreate]);
                                    targetItem.ShowSummary = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ShowTicketSummary]);
                                    targetItem.RequestorNotificationOnComment = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.RequestorNotificationOnComment]);
                                    targetItem.ActionUserNotificationOnComment = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ActionUserNotificationOnComment]);
                                    targetItem.InitiatorNotificationOnComment = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.InitiatorNotificationOnComment]);
                                    targetItem.RequestorNotificationOnCancel = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.RequestorNotificationOnCancel]);
                                    targetItem.ActionUserNotificationOnCancel = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ActionUserNotificationOnCancel]);
                                    targetItem.InitiatorNotificationOnCancel = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.InitiatorNotificationOnCancel]);
                                    targetItem.WaitingOnMeIncludesGroups = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.WaitingOnMeIncludesGroups]);
                                    targetItem.ShowBottleNeckChart = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ShowBottleNeckChart]);
                                    targetItem.AllowEscalationFromList = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.AllowEscalationFromList]);
                                    targetItem.AllowReassignFromList = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.AllowReassignFromList]);
                                    targetItem.OpenChart = Convert.ToString(item[SPDatabaseObjects.Columns.OpenTicketChart]);
                                    targetItem.CloseChart = Convert.ToString(item[SPDatabaseObjects.Columns.CloseTicketChart]);
                                    targetItem.AllowBatchClose = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.AllowBatchClose]);
                                    targetItem.AllowDelete = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.AllowTicketDelete]);
                                    targetItem.HideWorkFlow = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.HideWorkFlow]);
                                    targetItem.EnableRMMAllocation = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.EnableRMMAllocation]);
                                    targetItem.PreloadAllModuleTabs = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.PreloadAllModuleTabs]);
                                    targetItem.EnableModuleAgent = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.EnableModuleAgent]);
                                    targetItem.KeepItemOpen = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.KeepItemOpen]);
                                    targetItem.ReturnCommentOptional = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ReturnCommentOptional]);
                                    targetItem.EnableNewsOnHomePage = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.EnableNewTicketsOnHomePage]);
                                    targetItem.AutoRefreshListFrequency = Convert.ToInt32(item[SPDatabaseObjects.Columns.AutoRefreshListFrequency]);
                                    targetItem.ActualHoursByUser = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ActualHoursByUser]);
                                    targetItem.WaitingOnMeExcludesResolved = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.WaitingOnMeExcludesResolved]);
                                    targetItem.ThemeColor = Convert.ToString(item[SPDatabaseObjects.Columns.ThemeColor]);
                                    targetItem.EnableCloseOnHoldExpire = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.EnableCloseOnHoldExpire]);
                                    targetItem.EnableTicketImport = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.EnableTicketImport]);
                                    targetItem.ShowTasksInProjectTasks = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ShowTasksInProjectTasks]);
                                    targetItem.KeepTicketCounts = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.KeepTicketCounts]);
                                    targetItem.NavigationUrl = $"/content/images/menu/submenu/{targetItem.ModuleName}_32x32.svg";

                                    targetItem.AuthorizedToView = null;
                                    ClientOM.FieldUserValue[] users = item[SPDatabaseObjects.Columns.AuthorizedToView] as ClientOM.FieldUserValue[];
                                    if (users != null && users.Length > 0 && userMappedList != null)
                                        targetItem.AuthorizedToView = userMappedList.GetTargetIDs(users.Select(x => x.LookupId.ToString()).ToList());

                                    targetItem.AuthorizedToCreate = null;
                                    users = item[SPDatabaseObjects.Columns.AuthorizedToCreate] as ClientOM.FieldUserValue[];
                                    if (users != null && users.Length > 0 && userMappedList != null)
                                        targetItem.AuthorizedToCreate = userMappedList.GetTargetIDs(users.Select(x => x.LookupId.ToString()).ToList());


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
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                        }
                    }
                } while (position != null);

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
        }
        public override void UpdateBudgetsCategory()
        {
            base.UpdateBudgetsCategory();
            bool import = context.IsImportEnable("BudgetsCategory");

            if (import)
                ULog.WriteLog($"Updating budget categories");
            else
                ULog.WriteLog($"Load budget categories mapping");

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
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
                string listName = SPDatabaseObjects.Lists.BudgetCategories;
                MappedItemList targetMappedList = context.GetMappedList(listName);

                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;
                MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                string category = string.Empty, subCategory = string.Empty;
                BudgetCategory targetItem = null;
                MappedItemList userMappedList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, string.Empty, position);
                    position = collection.ListItemCollectionPosition;


                    foreach (ClientOM.ListItem item in collection)
                    {
                        category = Convert.ToString(item[SPDatabaseObjects.Columns.BudgetCategory]);
                        subCategory = Convert.ToString(item[SPDatabaseObjects.Columns.BudgetSubCategory]);
                        targetItem = null;
                        //targetItem = dbData.FirstOrDefault(x => x.BudgetCategoryName == category && x.BudgetSubCategory == subCategory);
                        if (import)
                        {
                            if (targetItem == null)
                            {
                                targetItem = new BudgetCategory();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            try
                            {
                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    targetItem.Title = category + "-" + subCategory;
                                    targetItem.BudgetCategoryName = category;
                                    targetItem.BudgetSubCategory = subCategory;
                                    targetItem.BudgetAcronym = Convert.ToString(item[SPDatabaseObjects.Columns.BudgetAcronym]);
                                    targetItem.BudgetCOA = Convert.ToString(item[SPDatabaseObjects.Columns.BudgetCOA]);
                                    targetItem.BudgetDescription = Convert.ToString(item[SPDatabaseObjects.Columns.BudgetDescription]);
                                    targetItem.IncludesStaffing = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IncludesStaffing]);
                                    targetItem.BudgetType = Convert.ToString(item[SPDatabaseObjects.Columns.BudgetType]);
                                    targetItem.BudgetTypeCOA = Convert.ToString(item[SPDatabaseObjects.Columns.BudgetTypeCOA]);
                                    targetItem.CapitalExpenditure = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.CapitalExpenditure]);
                                    targetItem.Deleted = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);

                                    targetItem.AuthorizedToView = null;
                                    ClientOM.FieldUserValue[] users = item[SPDatabaseObjects.Columns.AuthorizedToView] as ClientOM.FieldUserValue[];
                                    if (users != null && users.Length > 0 && userMappedList != null)
                                        targetItem.AuthorizedToView = userMappedList.GetTargetIDs(users.Select(x => x.LookupId.ToString()).ToList());

                                    targetItem.AuthorizedToEdit = null;
                                    users = item[SPDatabaseObjects.Columns.AuthorizedToEdit] as ClientOM.FieldUserValue[];
                                    if (users != null && users.Length > 0 && userMappedList != null)
                                        targetItem.AuthorizedToEdit = userMappedList.GetTargetIDs(users.Select(x => x.LookupId.ToString()).ToList());
                                    ClientOM.FieldUserValue Author = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                    if (Author != null && userlist != null)
                                        targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(Author.LookupId));
                                    ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                    if (Modifeduser != null && userlist != null)
                                        targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                    targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                    targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);



                                    if (targetItem.ID > 0)
                                        mgr.Update(targetItem);
                                    else
                                        mgr.Insert(targetItem);
                                }
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex);
                            }



                        }
                        if (targetItem != null)
                            targetMappedList.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} budgets categories added");
            }
        }
        public override void UpdateTenantDefaultUser()
        {
            base.UpdateTenantDefaultUser();

            ULog.WriteLog($"Create default admininstrator if not exist");
            List<Role> roles = new List<Role>();
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "SAdmin", Title = "Super Admin", IsSystem = true });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "Admin", Title = "Admin", IsSystem = true, LandingPage = "/Pages/UserHomePage" });
            var rolemanager = new UserRoleManager(context.AppContext);
            foreach (Role r in roles)
            {
                if (rolemanager.Get(x => x.Name == r.Name) == null)
                {
                    r.TenantID = context.AppContext.TenantID;
                    r.Created = DateTime.Now;
                    r.Modified = DateTime.Now;
                    r.CreatedBy = context.AppContext.TenantID;
                    r.ModifiedBy = context.AppContext.TenantID;
                    rolemanager.AddOrUpdate(r);
                }
            }

            MappedItemList userMappedlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
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
                targetItem.CreatedBy = context.Tenant.CreatedByUser;
                targetItem.ModifiedBy = context.Tenant.CreatedByUser;
                targetItem.Created = DateTime.Now;
                targetItem.Modified = DateTime.Now;
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
                    Role adminRole = rolemanager.Get(x => x.Name == RoleType.Admin.ToString());
                    ManagerBase<UserRole> userRoleMgr = new ManagerBase<UserRole>(context.AppContext);
                    UserRole userRole = userRoleMgr.Get(x => x.TenantID == context.AppContext.TenantID && x.RoleId == adminRole.Id);
                    if (userRole == null)
                    {
                        mgr.AddUserRole(targetItem, RoleType.Admin.ToString());
                    }
                }
                catch
                {
                    //comes with mapping is already exist
                }
                context.AppContext.SetCurrentUser(targetItem);
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

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                ConfigurationVariableManager mgr = new ConfigurationVariableManager(context.AppContext);
                List<ConfigurationVariable> dbData = mgr.Load();
                string listName = SPDatabaseObjects.Lists.ConfigurationVariable;
                MappedItemList locaionlist = context.GetMappedList(listName);

                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;
                MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                string keyName = string.Empty;
                ConfigurationVariable targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, string.Empty, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        keyName = Convert.ToString(item[SPDatabaseObjects.Columns.KeyName]);

                        targetItem = dbData.FirstOrDefault(x => x.KeyName == keyName);

                        if (targetItem == null)
                        {
                            targetItem = new ConfigurationVariable();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            targetItem.KeyName = keyName;
                            targetItem.KeyValue = Convert.ToString(item[SPDatabaseObjects.Columns.KeyValue]);
                            targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.KeyValue]);
                            targetItem.CategoryName = Convert.ToString(item[SPDatabaseObjects.Columns.CategoryName]);
                            targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                            ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                            if (users != null && userlist != null)
                                targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                            ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                            if (Modifeduser != null && userlist != null)
                                targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                            targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                            targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }



                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                ULog.WriteLog($"{targetNewItemCount} configuration variables added");
            }
        }
        public override void CreateDefaultEntries()
        {
            base.CreateDefaultEntries();

            bool import = context.IsImportEnable("DefaultEntries");
            if (!import)
                return;

            ULog.WriteLog($"Creating fieldconfigurations, tabs, admin menus");
            //DefaultData.Data.DefaultData.ConfigData defaultData = new DefaultData.Data.DefaultData.ConfigData(context.AppContext);

            TenantManager tenantManager = new TenantManager(ApplicationContext.Create());
            var masterTenant = tenantManager.GetTenant(ConfigHelper.DefaultTenant.ToLower());
            List<Tenant> tList = tenantManager.GetTenantList();
            Tenant tenant = null;
            ApplicationContext scontext = null;
            ConfigurationVariableManager ObjConfigVariable = new ConfigurationVariableManager(ApplicationContext.Create());
            if (masterTenant != null)
            {
                string ModelSite = ObjConfigVariable.GetValue(ConfigConstants.ModelSite);
                var name = uGITDAL.GetSingleValue(DatabaseObjects.Tables.ConfigurationVariable, $"{DatabaseObjects.Columns.TenantID}='{masterTenant.TenantID}'and {DatabaseObjects.Columns.KeyName}='{ConfigConstants.ModelSite}'", DatabaseObjects.Columns.KeyValue);
                if (!string.IsNullOrEmpty(Convert.ToString(name)))
                {
                    tenant = tList.FirstOrDefault(x => x.TenantName == UGITUtility.ObjectToString(name) || x.AccountID == UGITUtility.ObjectToString(name));
                    scontext = ApplicationContext.CreateContext(Convert.ToString(tenant.TenantID));
                }
            }

            TabViewManager tabViewManager = new TabViewManager(context.AppContext);
            List<TabView> commanTabView = tabViewManager.Load();
            if (commanTabView.Count == 0)
            {
                TabViewManager stabViewManager = new TabViewManager(scontext);
                List<TabView> scommanTabView = stabViewManager.Load();
                if (scommanTabView.Count > 0)
                {
                    scommanTabView.ForEach(x => { x.ID = 0; x.TenantID = context.AppContext.TenantID; x.ModifiedBy = string.Empty; x.CreatedBy = string.Empty; });
                    tabViewManager.InsertItems(scommanTabView);
                }
            }

            FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(context.AppContext);
            List<FieldConfiguration> fieldConfigurations = fieldConfigurationManager.Load();
            if (fieldConfigurations.Count == 0)
            {
                FieldConfigurationManager sfieldConfigurationManager = new FieldConfigurationManager(scontext);
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
                AdminCategoryManager sadminCategoryManager = new AdminCategoryManager(scontext);
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
                AdminConfigurationListManager sadminConfigurationListManager = new AdminConfigurationListManager(scontext);
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
                MenuNavigationManager smenuManager = new MenuNavigationManager(scontext);
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
                PageConfigurationManager spageMgr = new PageConfigurationManager(scontext);
                List<PageConfiguration> spages = spageMgr.Load();
                if (spages.Count > 0)
                {
                    spages.ForEach(x => { x.ID = 0; x.TenantID = context.AppContext.TenantID; x.ModifiedBy = string.Empty; x.CreatedBy = string.Empty; });
                    pageMgr.InsertItems(spages);
                }
            }

        }
        public string GetDefaultTenantName()
        {





            return null;
        }
        //Anurag Add below code on 20/10/21
        public override void UpdateWikiCategories()
        {
            base.UpdateWikiCategories();
            bool import = context.IsImportEnable("WikiCategories");
            if (!import)
                return;
            try
            {
                ULog.WriteLog($"Updating Wiki Categories");
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    WikiCategoryManager mgr = new WikiCategoryManager(context.AppContext);
                    List<WikiCategory> dbData = mgr.Load();
                    if (import && deleteBeforeImport)
                    {
                        try
                        {
                            mgr.Delete(dbData);
                            dbData = new List<WikiCategory>();
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteLog("Problem while deleting records");
                            ULog.WriteException(ex);
                        }
                    }

                    string listName = SPDatabaseObjects.Lists.WikiLeftNavigation;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    int targetNewItemCount = 0;
                    string region = string.Empty, country = string.Empty, state = string.Empty, title = string.Empty;
                    WikiCategory targetItem = null;

                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new WikiCategory();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            targetItem.ColumnType = Convert.ToString(item[SPDatabaseObjects.Columns.ColumnType]);
                            targetItem.ConditionalLogic = Convert.ToString(item[SPDatabaseObjects.Columns.ConditionalLogic]);
                            targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                            targetItem.ImageUrl = Convert.ToString(item[SPDatabaseObjects.Columns.UGITImageUrl]);
                            targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);



                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                            {
                                mgr.Insert(targetItem);
                                if (targetItem != null)
                                    locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                                ClientOM.FieldUserValue Createduser = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (Createduser != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(Createduser.LookupId));

                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));

                                mgr.Update(targetItem);
                            }
                        }
                    } while (position != null);

                    if (import)
                        ULog.WriteLog($"{targetNewItemCount} Wiki Categories added");
                }
            }
            catch (Exception e)
            {
                ULog.WriteException(e.ToString());
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

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {


                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;
                string keyName = string.Empty;
                string modulename = string.Empty;
                ModuleStageConstraintTemplates targetItem = null;
                ModuleStageConstraintTemplatesManager mgr = new ModuleStageConstraintTemplatesManager(context.AppContext);
                List<ModuleStageConstraintTemplates> dbData = mgr.Load();
                string listName = SPDatabaseObjects.Lists.ModuleStageConstraintTemplates;
                MappedItemList locaionlist = context.GetMappedList(listName);
                MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    MappedItemList users = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    MappedItemList userlst = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    foreach (ClientOM.ListItem item in collection)
                    {
                        keyName = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                        if (item[SPDatabaseObjects.Columns.ModuleNameLookup] != null)
                        {
                            modulename = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleNameLookup]).LookupValue);
                        }

                        targetItem = dbData.FirstOrDefault(x => x.Title == keyName && x.ModuleNameLookup == modulename);

                        if (targetItem == null)
                        {
                            targetItem = new ModuleStageConstraintTemplates();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            try
                            {
                                targetItem.Title = keyName;
                                targetItem.ModuleNameLookup = modulename;
                                targetItem.ProposedStatus = Convert.ToString(item[SPDatabaseObjects.Columns.UGITProposedStatus]);
                                if (item[SPDatabaseObjects.Columns.UGITProposedDate] != null)
                                    targetItem.ProposedDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.UGITProposedDate]);
                                targetItem.TicketId = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                                targetItem.ModuleAutoApprove = Convert.ToBoolean(item[SPDatabaseObjects.Columns.ModuleAutoApprove]);
                                targetItem.ModuleStep = Convert.ToInt32(item[SPDatabaseObjects.Columns.ModuleStep]);
                                targetItem.PercentComplete = item[SPDatabaseObjects.Columns.PercentComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.PercentComplete]) * 100;
                                targetItem.Comment = Convert.ToString(item[SPDatabaseObjects.Columns.TicketComment]);
                                targetItem.TaskActualHours = Convert.ToInt32(item[SPDatabaseObjects.Columns.TaskActualHours]);
                                targetItem.EstimatedHours = Convert.ToInt32(item[SPDatabaseObjects.Columns.TaskEstimatedHours]);
                                targetItem.TaskStatus = Convert.ToString(item[SPDatabaseObjects.Columns.TaskStatus]);
                                ClientOM.FieldUserValue[] _users = item[SPDatabaseObjects.Columns.AssignedTo] as ClientOM.FieldUserValue[];
                                if (_users != null && _users.Length > 0 && users != null)
                                    targetItem.AssignedTo = users.GetTargetIDs(_users.Select(x => x.LookupId.ToString()).ToList());
                                targetItem.Body = Convert.ToString(item[SPDatabaseObjects.Columns.Body]);
                                targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                                if (item[SPDatabaseObjects.Columns.TaskDueDate] != null)
                                    targetItem.TaskDueDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TaskDueDate]);
                                if (item[SPDatabaseObjects.Columns.StartDate] != null)
                                    targetItem.StartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.StartDate]);
                                targetItem.UserRoleType = Convert.ToString(item[SPDatabaseObjects.Columns.UserRoleType]);
                                targetItem.DateExpression = Convert.ToString(item[SPDatabaseObjects.Columns.DateExpression]);
                                targetItem.FormulaValue = Convert.ToString(item[SPDatabaseObjects.Columns.FormulaValue]);
                                targetItem.DocumentInfo = Convert.ToString(item["DocumentInfo"]);
                                targetItem.DocumentLibraryName = Convert.ToString(item[SPDatabaseObjects.Columns.DocumentLibraryName]);
                                ClientOM.FieldUserValue Createduser = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (Createduser != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(Createduser.LookupId));

                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex);
                            }

                        }
                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);


                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                ULog.WriteLog($"{targetNewItemCount} Stage Exit Criteria added");
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
            MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                DashboardManager mgr = new DashboardManager(context.AppContext);
                ModuleViewManager modulelist = new ModuleViewManager(context.AppContext);
                List<UGITModule> uGITModules = modulelist.Load();
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
                string listName = SPDatabaseObjects.Lists.DashboardPanels;
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();

                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                Dashboard targetItem = null;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, string.Empty, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        targetItem = null;
                        if (targetItem == null)
                        {
                            targetItem = new Dashboard();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                            targetItem.CategoryName = Convert.ToString(item[SPDatabaseObjects.Columns.CategoryName]);
                            targetItem.DashboardPanelInfo = Convert.ToString(item[SPDatabaseObjects.Columns.DashboardPanelInfo]);
                            targetItem.IsShowInSideBar = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsShowInSideBar]);
                            targetItem.DashboardPermission = Convert.ToString(item[SPDatabaseObjects.Columns.IsShowInSideBar]);
                            targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                            targetItem.DashboardDescription = Convert.ToString(item[SPDatabaseObjects.Columns.DashboardDescription]);
                            targetItem.DashboardType = (DashboardType)Enum.Parse(typeof(DashboardType), item[DatabaseObjects.Columns.DashboardType].ToString());
                            targetItem.IsHideTitle = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsHideTitle]);
                            targetItem.IsHideDescription = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsHideDescription]);
                            targetItem.PanelWidth = Convert.ToInt32(item[SPDatabaseObjects.Columns.PanelWidth]);
                            targetItem.PanelHeight = Convert.ToInt32(item[SPDatabaseObjects.Columns.PanelHeight]);
                            targetItem.ThemeColor = Convert.ToString(item[SPDatabaseObjects.Columns.ThemeColor]);
                            targetItem.SubCategory = Convert.ToString(item[SPDatabaseObjects.Columns.SubCategory]);
                            targetItem.IsActivated = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsActivated]);
                            ClientOM.FieldLookupValue[] DashboardModuleMultiLookup = item[SPDatabaseObjects.Columns.DashboardModuleMultiLookup] as ClientOM.FieldLookupValue[];
                            if (DashboardModuleMultiLookup != null && DashboardModuleMultiLookup.Count() > 0)
                            {
                                List<string> tmplist = new List<string>();
                                tmplist.Clear();
                                foreach (var modulename in DashboardModuleMultiLookup)
                                {
                                    string val = UGITUtility.ObjectToString(uGITModules.FirstOrDefault(x => x.ModuleName == modulename.LookupValue).ID);
                                    tmplist.Add(val);
                                }
                                targetItem.DashboardModuleMultiLookup = string.Join(",", tmplist.ToArray());
                            }

                            ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                            if (users != null && userlist != null)
                                targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                            ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                            if (Modifeduser != null && userlist != null)
                                targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                            targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                            targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                            if (!string.IsNullOrWhiteSpace(targetItem.AuthorizedToView))
                                targetItem.AuthorizedToView = context.GetTargetValue(spContext, list, item, targetItem.AuthorizedToView);
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                ULog.WriteLog($"{targetNewItemCount} DashboardPanels added");
            }
            targetNewItemCount = 0;
            ULog.WriteLog($"Updating DashboardPanelView");
            {
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
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
                    string listName = SPDatabaseObjects.Lists.DashboardPanelView;
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    DashboardPanelView targetItem = null;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, string.Empty, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new DashboardPanelView();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ViewType = Convert.ToString(item[SPDatabaseObjects.Columns.UGITViewType]);
                                targetItem.ViewName = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.DashboardPanelInfo = Convert.ToString(item[SPDatabaseObjects.Columns.DashboardPanelInfo]);
                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (users != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                                if (!string.IsNullOrWhiteSpace(targetItem.AuthorizedToViewUsers))
                                    targetItem.AuthorizedToViewUsers = context.GetTargetValue(spContext, list, item, targetItem.AuthorizedToViewUsers);
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }
                            if (targetItem != null)
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} DashboardPanelView added");
                }
            }
            ChartTemplates(import);
        }

        private void ChartTemplates(bool import)
        {
            int targetNewItemCount = 0;
            MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
            ULog.WriteLog($"Updating ChartTemplate");
            {
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
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
                    string listName = SPDatabaseObjects.Lists.ChartTemplates;
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    ChartTemplate targetItem = null;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, string.Empty, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new ChartTemplate();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ChartObject = Convert.ToString(item[SPDatabaseObjects.Columns.ChartObject]);
                                targetItem.TemplateType = Convert.ToString(item[SPDatabaseObjects.Columns.TemplateType]);
                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (users != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);

                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }

                            if (targetItem != null)
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} ChartTemplates added");
                }
                
            }
            ULog.WriteLog($"Updating ChartFilters");
            {
                System.Data.DataTable dbData = GetTableDataManager.GetTableStructure(DatabaseObjects.Tables.ChartFilters);
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ChartFilters;
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        System.Data.DataRow dr = null;
                        foreach (ClientOM.ListItem item in collection)
                        {
                            if (dbData != null || dbData.Rows.Count == 0)
                            {
                                dr = dbData.NewRow();
                                dr[DatabaseObjects.Columns.Title] = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                dr[DatabaseObjects.Columns.ColumnName] = item[SPDatabaseObjects.Columns.ColumnName];
                                dr[DatabaseObjects.Columns.ModuleType] = item[SPDatabaseObjects.Columns.ModuleType];
                                dr[DatabaseObjects.Columns.IsDefault] = item[DatabaseObjects.Columns.IsDefault];
                                dr[DatabaseObjects.Columns.ListName] = item[DatabaseObjects.Columns.ListName];
                                dr[DatabaseObjects.Columns.ValueAsId] = item[DatabaseObjects.Columns.ValueAsId];
                                ClientOM.FieldLookupValue ModuleNameLookup = item[SPDatabaseObjects.Columns.ModuleNameLookup] as ClientOM.FieldLookupValue;
                                if(ModuleNameLookup!=null)
                                    dr[DatabaseObjects.Columns.ModuleNameLookup] = ModuleNameLookup.LookupValue;
                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (users != null && userlist != null)
                                    dr[DatabaseObjects.Columns.CreatedByUser] = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    dr[DatabaseObjects.Columns.ModifiedByUser] = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                dr[DatabaseObjects.Columns.Created] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                dr[DatabaseObjects.Columns.Modified] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                                dr[DatabaseObjects.Columns.TenantID] = context.AppContext.TenantID;
                                dbData.Rows.Add(dr);
                            }
                        }
                        GetTableDataManager.bulkupload(dbData, DatabaseObjects.Tables.ChartFilters);
                        targetNewItemCount = dbData.Rows.Count;
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} ChartFilters added");
                }
            }
            ULog.WriteLog($"Updating ChartFormula");
            {
                System.Data.DataTable dbData = GetTableDataManager.GetTableStructure(DatabaseObjects.Tables.ChartFormula);
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ChartFormula;
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        System.Data.DataRow dr = null;
                        foreach (ClientOM.ListItem item in collection)
                        {
                            if (dbData != null || dbData.Rows.Count == 0)
                            {
                                dr = dbData.NewRow();
                                dr[DatabaseObjects.Columns.Title] = item[SPDatabaseObjects.Columns.Title];
                                dr[DatabaseObjects.Columns.FormulaValue] = Convert.ToString(item[SPDatabaseObjects.Columns.FormulaValue]);
                                dr[DatabaseObjects.Columns.Formula] = Convert.ToString(item[SPDatabaseObjects.Columns.Formula]);
                                dr[DatabaseObjects.Columns.ModuleType] = item[SPDatabaseObjects.Columns.ModuleType];
                                dr[DatabaseObjects.Columns.ChartTemplateIds] = item[SPDatabaseObjects.Columns.ChartTemplateIds];
                                dr[DatabaseObjects.Columns.IsDefault] = item[DatabaseObjects.Columns.IsDefault];
                                ClientOM.FieldLookupValue ModuleNameLookup = item[SPDatabaseObjects.Columns.ModuleNameLookup] as ClientOM.FieldLookupValue;
                                if (ModuleNameLookup != null)
                                    dr[DatabaseObjects.Columns.ModuleNameLookup] = ModuleNameLookup.LookupValue;
                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (users != null && userlist != null)
                                    dr[DatabaseObjects.Columns.CreatedByUser] = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    dr[DatabaseObjects.Columns.ModifiedByUser] = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                dr[DatabaseObjects.Columns.Created] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                dr[DatabaseObjects.Columns.Modified] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                                dr[DatabaseObjects.Columns.TenantID] = context.AppContext.TenantID;
                                dbData.Rows.Add(dr);
                            }
                        }
                        GetTableDataManager.bulkupload(dbData, DatabaseObjects.Tables.ChartFormula);
                        targetNewItemCount = dbData.Rows.Count;
                    } while (position != null);
                    ULog.WriteLog($"{targetNewItemCount} ChartFormula added");
                }
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

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                DocumentManager mgr = new DocumentManager(context.AppContext);
                List<Document> dbData = mgr.Load();
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<Document>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }
                string listName = SPDatabaseObjects.Lists.TSRTicket;
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                string keyName = string.Empty;
                Document targetItem = null;
                int targetNewItemCount = 0;
                byte[] imageArray = null;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        if (!Convert.ToBoolean(item[SPDatabaseObjects.Columns.Attachments]))
                        {
                            continue;
                        }
                        keyName = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                        try
                        {
                            ClientOM.AttachmentCollection oAttachments = item.AttachmentFiles;
                            spContext.Load(oAttachments);
                            spContext.ExecuteQuery();
                            foreach (ClientOM.Attachment attachment in oAttachments)
                            {
                                ClientOM.File file = item.ParentList.ParentWeb.GetFileByServerRelativeUrl(attachment.ServerRelativeUrl);
                                spContext.Load(file);
                                spContext.ExecuteQuery();
                                ClientOM.ClientResult<System.IO.Stream> data = file.OpenBinaryStream();
                                spContext.Load(file);
                                spContext.ExecuteQuery();
                                using (System.IO.MemoryStream mStream = new System.IO.MemoryStream())
                                {
                                    if (data != null)
                                    {
                                        data.Value.CopyTo(mStream);
                                        imageArray = mStream.ToArray();
                                        //string b64String = Convert.ToBase64String(imageArray);

                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteLog(ex.Message, $"Issue File Attachments conversion");
                            ULog.WriteException(ex);
                        }


                        //try
                        //{
                        //    //ClientOM.FileInformation fileInformation = ClientOM.File.OpenBinaryDirect(spContext, (string)item["FileRef"]);
                        //    //string url = item.Context.Url + "//Lists//" + listName + "//Attachments//" + item["FileName"];
                        //    //ClientOM.Folder folder = spContext.Web.GetFolderByServerRelativeUrl(url);
                        //    //spContext.Load(folder);
                        //    //spContext.ExecuteQuery();
                        //    //ClientOM.FileCollection oAttachments = folder.Files;
                        //    //ClientOM.AttachmentCollection oAttachments = item.AttachmentFiles;
                        //    //spContext.Load(oAttachments);
                        //    //spContext.ExecuteQuery();
                        //    //item.AttachmentFiles.ur
                        //    //string url = item.Context.Url + "/Lists//" + listName + "//Attachments//" +item["ID"]+"//"+ oAttachments[0].FileName;
                        //    //ClientOM.Folder folder = spContext.Web.GetFolderByServerRelativeUrl(url);
                        //    //spContext.Load(folder);
                        //    //spContext.ExecuteQuery();
                        //}
                        //catch (Exception ex)
                        //{


                        //}

                        //byte[] files = item.AttachmentFiles[0]
                        targetItem = dbData.FirstOrDefault(x => x.Name == keyName);

                        if (targetItem == null)
                        {
                            targetItem = new Document();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.Id == 0 || importWithUpdate)
                        {
                            targetItem.Name = keyName;
                            targetItem.FileID = Guid.NewGuid().ToString();
                            targetItem.Blob = imageArray;
                            if (targetItem.Id > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.Id.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"File Attachments added");
            }
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
            MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
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

                string listName = SPDatabaseObjects.Lists.TicketTemplates;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList Maplist = context.GetMappedList(listName);

                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();

                string ModuleNameLookup = string.Empty, TemplateType = string.Empty, title = string.Empty;
                TicketTemplate targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        ModuleNameLookup = item[SPDatabaseObjects.Columns.ModuleNameLookup] == null ? string.Empty : Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleNameLookup]).LookupValue);
                        targetItem = dbData.FirstOrDefault(x => x.Title == title && x.ModuleNameLookup == ModuleNameLookup && x.TemplateType == TemplateType);
                        targetItem = null;
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.TemplateType = Convert.ToString(item[SPDatabaseObjects.Columns.TemplateType]);
                                targetItem.ModuleNameLookup = ModuleNameLookup;
                                targetItem.Deleted = false;
                                targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.TicketDescription]);
                                targetItem.FieldValues = Convert.ToString(item[SPDatabaseObjects.Columns.FieldValues]);
                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (users != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);

                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }
                        }

                        if (targetItem != null)
                            Maplist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} QuickTickets added");
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
            MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
            MappedItemList locationMappedList = context.GetMappedList(SPDatabaseObjects.Lists.Location);
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
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

                string listName = SPDatabaseObjects.Lists.VendorType;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);

                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();

                string title = string.Empty;
                VendorType targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);

                        targetItem = dbData.FirstOrDefault(x => x.Title == title);
                        targetItem = null;
                        if (import)
                        {
                            if (targetItem == null)
                            {
                                targetItem = new VendorType();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                            targetItem.VTDescription = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                            ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                            if (users != null && userlist != null)
                                targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                            ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                            if (Modifeduser != null && userlist != null)
                                targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                            targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                            targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);

                            if (targetItem.id > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.id.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} VendorType added");
            }
            targetNewItemCount = 0;
            ULog.WriteLog($"Updating AssetVendors");
            {
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    AssetVendorViewManager mgr = new AssetVendorViewManager(context.AppContext);
                    List<AssetVendor> dbData = mgr.Load();
                    VendorTypeManager _mgr = new VendorTypeManager(context.AppContext);
                    List<VendorType> _dbData = _mgr.Load();
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

                    string listName = SPDatabaseObjects.Lists.AssetVendors;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);

                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();

                    string title = string.Empty;
                    AssetVendor targetItem = null;

                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        try
                        {
                            foreach (ClientOM.ListItem item in collection)
                            {
                                long VendorTypeId = 0;
                                ClientOM.FieldLookupValue sVendorLookup = item[SPDatabaseObjects.Columns.VendorTypeLookup] as ClientOM.FieldLookupValue;
                                if (sVendorLookup != null)
                                {
                                    if (!string.IsNullOrEmpty(sVendorLookup.LookupValue))
                                    {
                                        if (_dbData.Count > 0)
                                            VendorTypeId = _dbData.FirstOrDefault(x => x.Title == sVendorLookup.LookupValue).id;
                                    }
                                }
                                targetItem = null;
                                if (import)
                                {
                                    if (targetItem == null)
                                    {
                                        targetItem = new AssetVendor();
                                        dbData.Add(targetItem);
                                        targetNewItemCount++;
                                    }

                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    targetItem.VendorTypeLookup = VendorTypeId; //context.GetTargetValueAsOptionalLong(spContext, list, item, SPDatabaseObjects.Columns.VendorTypeLookup) ?? 0;
                                    targetItem.VendorName = Convert.ToString(item[SPDatabaseObjects.Columns.VendorName]);
                                    targetItem.ContactName = Convert.ToString(item[SPDatabaseObjects.Columns.ContactName]);
                                    targetItem.VendorLocation = Convert.ToString(item[SPDatabaseObjects.Columns.Location]);
                                    targetItem.VendorAddress = Convert.ToString(item[SPDatabaseObjects.Columns.VendorAddress]);
                                    targetItem.VendorEmail = Convert.ToString(item[SPDatabaseObjects.Columns.VendorEmail]);
                                    targetItem.VendorPhone = Convert.ToString(item[SPDatabaseObjects.Columns.VendorPhone]);
                                    targetItem.WebsiteUrl = Convert.ToString(item[SPDatabaseObjects.Columns.UGITWebsiteUrl]);
                                    targetItem.AccountRepEmail = Convert.ToString(item[SPDatabaseObjects.Columns.AccountRepEmail]);
                                    targetItem.AccountRepPhone = Convert.ToString(item[SPDatabaseObjects.Columns.AccountRepPhone]);
                                    targetItem.AccountRepMobile = Convert.ToString(item[SPDatabaseObjects.Columns.AccountRepMobile]);
                                    targetItem.AccountRepName = Convert.ToString(item[SPDatabaseObjects.Columns.AccountRepName]);
                                    targetItem.ExternalType = Convert.ToString(item[SPDatabaseObjects.Columns.ExternalType]);
                                    targetItem.ProductServiceDescription = Convert.ToString(item["ProductServiceDescription"]);
                                    ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                    if (users != null && userlist != null)
                                        targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                    ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                    if (Modifeduser != null && userlist != null)
                                        targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                    targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                    targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                                    targetItem.SupportHours = Convert.ToDouble(item[SPDatabaseObjects.Columns.SupportHours]);
                                    if (targetItem.ID > 0)
                                        mgr.Update(targetItem);
                                    else
                                        mgr.Insert(targetItem);
                                }

                                if (targetItem != null)
                                    locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                            }
                        }
                        catch (Exception ex)
                        {

                            ULog.WriteException(ex.ToString());
                        }

                    } while (position != null);

                    if (import)
                        ULog.WriteLog($"{targetNewItemCount} AssetVendors added");
                }
            }
        }

        public override void UpdateAssetModels()
        {
            base.UpdateAssetModels();

            bool import = context.IsImportEnable("AssetModels");

            if (import)
            {
                ULog.WriteLog($"Updating AssetModels");
                int targetNewItemCount = 0;
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    AssetModelViewManager mgr = new AssetModelViewManager(context.AppContext);
                    List<AssetModel> dbData = mgr.Load();
                    AssetVendorViewManager _mgr = new AssetVendorViewManager(context.AppContext);
                    List<AssetVendor> _dbData = _mgr.Load();
                    BudgetCategoryViewManager mgr_ = new BudgetCategoryViewManager(context.AppContext);
                    List<BudgetCategory> dbData_ = mgr_.Load();
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

                    string listName = SPDatabaseObjects.Lists.AssetModels;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string ModelName = string.Empty, title = string.Empty; long VendorLookup = 0;
                    AssetModel targetItem = null;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            ModelName = Convert.ToString(item[SPDatabaseObjects.Columns.ModelName]);
                            //VendorLookup = context.GetTargetValueAsOptionalLong(spContext, list, item, SPDatabaseObjects.Columns.VendorLookup) ?? 0;
                            title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                            ClientOM.FieldLookupValue sVendorLookup = item[SPDatabaseObjects.Columns.VendorLookup] as ClientOM.FieldLookupValue;
                            ClientOM.FieldLookupValue BudgetLookup = item[SPDatabaseObjects.Columns.BudgetLookup] as ClientOM.FieldLookupValue;
                            long BudgetLookupId = 0;
                            if (BudgetLookup != null)
                            {
                                if (!string.IsNullOrEmpty(BudgetLookup.LookupValue))
                                {
                                    BudgetLookupId = dbData_.FirstOrDefault(x => x.BudgetSubCategory == BudgetLookup.LookupValue).ID;
                                }
                            }
                            VendorLookup = 0;
                            if (sVendorLookup != null)
                            {
                                if (!string.IsNullOrEmpty(sVendorLookup.LookupValue))
                                {
                                    VendorLookup = _dbData.FirstOrDefault(x => x.Title == sVendorLookup.LookupValue).ID;
                                }
                            }
                            targetItem = dbData.FirstOrDefault(x => x.ModelName == ModelName);
                            targetItem = null;
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
                                    targetItem.Title = title;
                                    targetItem.ModelName = ModelName;
                                    targetItem.BudgetLookup = BudgetLookupId; //context.GetTargetValueAsOptionalLong(spContext, list, item, SPDatabaseObjects.Columns.BudgetLookup) ?? 0;
                                    targetItem.VendorLookup = VendorLookup;//context.GetTargetValueAsOptionalLong(spContext, list, item, SPDatabaseObjects.Columns.VendorLookup) ?? 0;
                                    targetItem.ModelDescription = Convert.ToString(item[SPDatabaseObjects.Columns.ModelDescription]);
                                    targetItem.ExternalType = Convert.ToString(item[SPDatabaseObjects.Columns.ExternalType]);

                                    ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                    if (users != null && userlist != null)
                                        targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                    ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                    if (Modifeduser != null && userlist != null)
                                        targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                    targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                    targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                                }
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }

                            if (targetItem != null)
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                    } while (position != null);

                    if (import)
                        ULog.WriteLog($"{targetNewItemCount} AssetModels added");
                }
            }
            else
                ULog.WriteLog($"Load AssetModels Mapping");


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

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
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

                string listName = SPDatabaseObjects.Lists.DashboardFactTables;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                DashboardFactTables targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.CacheMode = Convert.ToString(item[SPDatabaseObjects.Columns.CacheMode]);
                                targetItem.CacheAfter = Convert.ToInt32(item[SPDatabaseObjects.Columns.CacheAfter]);
                                targetItem.CacheTable = Convert.ToBoolean(item[SPDatabaseObjects.Columns.CacheTable]);
                                targetItem.CacheThreshold = Convert.ToInt32(item[SPDatabaseObjects.Columns.CacheThreshold]);
                                targetItem.Description = Convert.ToString(item["Description"]);
                                targetItem.DashboardPanelInfo = Convert.ToString(item[SPDatabaseObjects.Columns.DashboardPanelInfo]);
                                targetItem.ExpiryDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.ExpiryDate]);
                                targetItem.RefreshMode = Convert.ToString(item[SPDatabaseObjects.Columns.RefreshMode]);
                                targetItem.LastUpdated = DateTime.Now;
                                targetItem.Status = Convert.ToString(item[SPDatabaseObjects.Columns.Status]);
                                targetItem.TenantID = context.AppContext.TenantID;
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} FactTables added");
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

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
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

                string listName = SPDatabaseObjects.Lists.MessageBoard;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                MessageBoard targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                        targetItem = null;
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.Body = Convert.ToString(item[SPDatabaseObjects.Columns.Body]);
                                targetItem.Expires = item[SPDatabaseObjects.Columns.Expires] == null ? DateTime.Now.AddDays(10) : Convert.ToDateTime(item[SPDatabaseObjects.Columns.Expires]);
                                targetItem.MessageType = Convert.ToString(item[SPDatabaseObjects.Columns.MessageType]);
                                if (!string.IsNullOrWhiteSpace(targetItem.AuthorizedToView))
                                    targetItem.AuthorizedToView = context.GetTargetValue(spContext, list, item, targetItem.AuthorizedToView);

                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (users != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} MessageBoard added");
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
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
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

                string listName = SPDatabaseObjects.Lists.UserSkills;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                UserSkills targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        //targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]) && x.CategoryName == item[SPDatabaseObjects.Columns.CategoryName].ToString());
                        targetItem = null;
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.CategoryName = Convert.ToString(item[SPDatabaseObjects.Columns.CategoryName]);
                                targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} UserSkills added");
            }
        }

        public override void UpdateServiceCatalogAndAgents()
        {
            base.UpdateServiceCatalogAndAgents();

            bool import = context.IsImportEnable("ServiceCatalogAndAgents");
            if (!import)
                return;
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
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {

                    string listName = SPDatabaseObjects.Lists.ServiceCategories;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    ServiceCategory targetItem = null;

                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]) && x.CategoryName == item[SPDatabaseObjects.Columns.CategoryName].ToString());
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
                                    if (Convert.ToString(item[SPDatabaseObjects.Columns.Title]) != "~ModuleAgent~" && Convert.ToString(item[SPDatabaseObjects.Columns.Title]) != "~ModuleFeedback~")
                                    {
                                        targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    }
                                    targetItem.CategoryName = Convert.ToString(item[SPDatabaseObjects.Columns.CategoryName]);
                                    string url = $"/Content/Images/{Path.GetFileNameWithoutExtension(Convert.ToString(item[SPDatabaseObjects.Columns.UGITImageUrl]).Split('/').LastOrDefault())}";
                                    targetItem.ImageUrl = url + Path.GetExtension(Convert.ToString(item[SPDatabaseObjects.Columns.UGITImageUrl]));
                                    targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                                    targetItem.Deleted = false; //Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                                }
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }

                            if (targetItem != null)
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                    } while (position != null);

                    if (import)
                        ULog.WriteLog($"{targetNewItemCount} ServiceCategories added");
                }
            }
        }

        private void UpdateServices(bool import)
        {
            int targetNewItemCount = 0;

            ULog.WriteLog($"Updating Services");
            {
                ServicesManager mgr = new ServicesManager(context.AppContext);
                List<Services> dbData = mgr.Load();
                ServiceCategoryManager _mgr = new ServiceCategoryManager(context.AppContext);
                List<ServiceCategory> _dbData = _mgr.Load();
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
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.Services;
                    MappedItemList userMappedList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    Services targetItem = null;
                    long CategoryId = 0; string ServiceCategoryType = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            ClientOM.FieldLookupValue ServiceCategoryId = item[SPDatabaseObjects.Columns.ServiceCategoryNameLookup] as ClientOM.FieldLookupValue;
                            if (ServiceCategoryId != null)
                            {
                                if (!string.IsNullOrEmpty(ServiceCategoryId.LookupValue))
                                {
                                    CategoryId = _dbData.FirstOrDefault(x => x.CategoryName == ServiceCategoryId.LookupValue).ID;
                                    ServiceCategoryType = ServiceCategoryId.LookupValue;
                                }
                            }
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
                                    try
                                    {
                                        targetItem.AllowServiceTasksInBackground = Convert.ToBoolean(item[SPDatabaseObjects.Columns.AllowServiceTasksInBackground]);
                                        targetItem.AttachmentRequired = Convert.ToString(item[SPDatabaseObjects.Columns.AttachmentRequired]);
                                        targetItem.AttachmentsInChildTickets = Convert.ToBoolean(item[SPDatabaseObjects.Columns.AllowAttachmentsToChild]);
                                        targetItem.OwnerUser = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketOwner);
                                        targetItem.AuthorizedToView = null;
                                        ClientOM.FieldUserValue[] users = item[SPDatabaseObjects.Columns.AuthorizedToView] as ClientOM.FieldUserValue[];
                                        if (users != null && users.Length > 0 && userMappedList != null)
                                            targetItem.AuthorizedToView = userMappedList.GetTargetIDs(users.Select(x => x.LookupId.ToString()).ToList());

                                        targetItem.ConditionalLogic = Convert.ToString(item[SPDatabaseObjects.Columns.ConditionalLogic]);
                                        targetItem.QuestionMapVariables = Convert.ToString(item[SPDatabaseObjects.Columns.QuestionMapVariables]);
                                        targetItem.CreateParentServiceRequest = Convert.ToBoolean(item[SPDatabaseObjects.Columns.CreateParentServiceRequest]);
                                        targetItem.IncludeInDefaultData = true;
                                        targetItem.HideSummary = Convert.ToBoolean(item[SPDatabaseObjects.Columns.HideSummary]);
                                        targetItem.HideThankYouScreen = Convert.ToBoolean(item[SPDatabaseObjects.Columns.HideThankYouScreen]);
                                        string url = $"/Assets/{Path.GetFileNameWithoutExtension(Convert.ToString(item[SPDatabaseObjects.Columns.UGITImageUrl]).Split('/').LastOrDefault())}";
                                        targetItem.ImageUrl = url + Path.GetExtension(Convert.ToString(item[SPDatabaseObjects.Columns.UGITImageUrl]));
                                        targetItem.IsActivated = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsActivated]);
                                        targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                                        targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                        targetItem.LoadDefaultValue = Convert.ToBoolean(item[SPDatabaseObjects.Columns.LoadDefaultValue]);
                                        string val = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleNameLookup]).LookupValue);
                                        targetItem.ModuleNameLookup = val == null ? "SVCConfig" : val;
                                        ClientOM.FieldLookupValue[] ModuleStageMultiLookup = item[SPDatabaseObjects.Columns.ModuleStageMultiLookup] as ClientOM.FieldLookupValue[];

                                        if (ModuleStageMultiLookup != null && ModuleStageMultiLookup.Length > 0)
                                        {
                                            targetItem.ModuleStage = string.Join(";#", ModuleStageMultiLookup.Select(x => x.LookupId.ToString()));
                                            Dictionary<string, object> SPTicketrefernces = new Dictionary<string, object>();
                                            foreach (var _item in ModuleStageMultiLookup)
                                            {
                                                SPTicketrefernces.Add("Module", Convert.ToString(targetItem.ModuleNameLookup));
                                                SPTicketrefernces.Add("StageName", UGITUtility.ObjectToString(_item.LookupValue));
                                                SPTicketrefernces.Add("StageId", UGITUtility.ObjectToString(_item.LookupId));
                                                SPTicketrefernces.Add("Title", Convert.ToString(targetItem.Title));
                                                GetTableDataManager.AddItem<int>("MigrationModuleStage", SPTicketrefernces);
                                                SPTicketrefernces.Clear();
                                            }

                                        }


                                        string _url = Convert.ToString(item[SPDatabaseObjects.Columns.NavigationUrl]).Replace("_layouts/15", "layouts");
                                        targetItem.NavigationUrl = _url;
                                        targetItem.OwnerApprovalRequired = Convert.ToBoolean(item[SPDatabaseObjects.Columns.OwnerApprovalRequired]);
                                        targetItem.SectionConditionalLogic = Convert.ToString(item[SPDatabaseObjects.Columns.SectionConditionalLogic]);
                                        targetItem.CategoryId = CategoryId;
                                        if (ServiceCategoryType != "~ModuleAgent~" && ServiceCategoryType != "~ModuleFeedback~")
                                        {
                                            targetItem.ServiceCategoryType = ServiceCategoryType;
                                        }
                                        targetItem.ServiceDescription = Convert.ToString(item[SPDatabaseObjects.Columns.ServiceDescription]);
                                        targetItem.ShowStageTransitionButtons = Convert.ToBoolean(item[SPDatabaseObjects.Columns.ShowStageTransitionButtons]);

                                        if (ServiceCategoryType == "~ModuleAgent~" || ServiceCategoryType == "~ModuleFeedback~")
                                        {
                                            if (ServiceCategoryType == "~ModuleAgent~")
                                                targetItem.ServiceType = "ModuleAgent";
                                            if (ServiceCategoryType == "~ModuleFeedback~")
                                                targetItem.ServiceType = "ModuleFeedback";
                                        }
                                        else
                                        {
                                            targetItem.ServiceType = "Service"; //Convert.ToString(item[SPDatabaseObjects.Columns.ServiceCategoryType]);
                                        }
                                        targetItem.Deleted = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                                        targetItem.Use24x7Calendar = Convert.ToBoolean(item[SPDatabaseObjects.Columns.Use24x7Calendar]);
                                        targetItem.EnableTaskReminder = Convert.ToBoolean(item[SPDatabaseObjects.Columns.EnableTaskReminder]);
                                        targetItem.Reminders = Convert.ToString(item[SPDatabaseObjects.Columns.Reminders]);
                                        targetItem.SLAConfiguration = Convert.ToString(item[SPDatabaseObjects.Columns.SLAConfiguration]);
                                        targetItem.StartResolutionSLAFromAssigned = Convert.ToBoolean(item[SPDatabaseObjects.Columns.StartResolutionSLAFromAssigned]);
                                    }
                                    catch (Exception ex)
                                    {
                                        ULog.WriteException(ex);
                                    }


                                }
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }

                            if (targetItem != null)
                            {
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                                /*below code add to maintain reference of services*/
                                Dictionary<string, object> SPTicketrefernces = new Dictionary<string, object>();
                                SPTicketrefernces.Add("SPServiceId", (Convert.ToString(item[SPDatabaseObjects.Columns.Id])));
                                SPTicketrefernces.Add("DTServiceId", UGITUtility.ObjectToString(targetItem.ID));
                                GetTableDataManager.AddItem<int>("MigrationRecordUpdate", SPTicketrefernces);
                            }

                        }
                    } while (position != null);

                    if (import)
                        ULog.WriteLog($"{targetNewItemCount} Services added");



                }
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
                ServicesManager srvMgr = new ServicesManager(context.AppContext);
                List<Services> srvDbData = srvMgr.Load();
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ServiceSections;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    ServiceSection targetItem = null;
                    long ServiceId = 0;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        foreach (ClientOM.ListItem item in collection)
                        {
                            ClientOM.FieldLookupValue ServicedLookup = item[SPDatabaseObjects.Columns.ServiceTitleLookup] as ClientOM.FieldLookupValue;
                            if (ServicedLookup != null)
                            {
                                if (!string.IsNullOrEmpty(ServicedLookup.LookupValue))
                                {
                                    ServiceId = srvDbData.FirstOrDefault(x => x.Title == ServicedLookup.LookupValue).ID;
                                }
                            }
                            targetItem = dbData.FirstOrDefault(x => x.SectionName == Convert.ToString(item[SPDatabaseObjects.Columns.SectionName]) && x.ServiceID == ServiceId);

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
                                    try
                                    {
                                        targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                        targetItem.SectionName = Convert.ToString(item[SPDatabaseObjects.Columns.SectionName]);
                                        targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                                        targetItem.SectionSequence = Convert.ToInt32(item["SectionSequence"]);
                                        targetItem.ServiceID = ServiceId;
                                    }
                                    catch (Exception ex)
                                    {
                                        ULog.WriteException(ex);
                                    }
                                }
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }

                            if (targetItem != null)
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                    } while (position != null);

                    if (import)
                        ULog.WriteLog($"{targetNewItemCount} Service Sections added");
                }

            }

        }

        private void UpdateServiceQuestions(bool import)
        {
            int targetNewItemCount = 0;
            MappedItemList appmappedList = context.GetMappedList(SPDatabaseObjects.Lists.Applications);
            MappedItemList RequestTypeMappedList = context.GetMappedList(SPDatabaseObjects.Lists.RequestType);
            ULog.WriteLog($"Updating Service Questions");
            {
                ServiceQuestionManager mgr = new ServiceQuestionManager(context.AppContext);
                List<ServiceQuestion> dbData = mgr.Load();
                ServicesManager srvMgr = new ServicesManager(context.AppContext);
                List<Services> srvDbData = srvMgr.Load();
                ServiceSectionManager secMgr = new ServiceSectionManager(context.AppContext);
                List<ServiceSection> dbSecData = secMgr.Load();
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
                            x.ServiceSectionName = serviceSection.Title;
                    });
                }

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {

                    string listName = SPDatabaseObjects.Lists.ServiceQuestions;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    ServiceQuestion targetItem = null;
                    long ServiceId = 0; long ServiceSectionID = 0;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        foreach (ClientOM.ListItem item in collection)
                        {
                            try
                            {

                                ClientOM.FieldLookupValue ServicedLookup = item[SPDatabaseObjects.Columns.ServiceTitleLookup] as ClientOM.FieldLookupValue;
                                if (ServicedLookup != null)
                                {
                                    if (!string.IsNullOrEmpty(ServicedLookup.LookupValue))
                                    {
                                        ServiceId = srvDbData.FirstOrDefault(x => x.Title == ServicedLookup.LookupValue).ID;
                                    }
                                }
                                ClientOM.FieldLookupValue ServiceSection = item[SPDatabaseObjects.Columns.ServiceSectionsTitleLookup] as ClientOM.FieldLookupValue;
                                if (ServicedLookup != null)
                                {
                                    if (!string.IsNullOrEmpty(ServiceSection.LookupValue))
                                    {
                                        ServiceSectionID = dbSecData.FirstOrDefault(x => x.Title == ServiceSection.LookupValue).ID;
                                    }
                                }
                                targetItem = dbData.FirstOrDefault(x => x.QuestionTitle == Convert.ToString(item[SPDatabaseObjects.Columns.SWQuestion]) && x.ServiceID == ServiceId && x.ServiceSectionID == ServiceSectionID);
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
                                        targetItem.QuestionTitle = Convert.ToString(item[SPDatabaseObjects.Columns.SWQuestion]);
                                        targetItem.QuestionType = Convert.ToString(item[SPDatabaseObjects.Columns.SWQuestionType]);
                                        targetItem.TokenName = Convert.ToString(item[SPDatabaseObjects.Columns.TokenName]);
                                        targetItem.EnableZoomIn = Convert.ToBoolean(item[SPDatabaseObjects.Columns.EnableZoomIn]);
                                        targetItem.ServiceSectionID = ServiceSectionID;
                                        targetItem.ServiceID = ServiceId;
                                        targetItem.Helptext = Convert.ToString(item[SPDatabaseObjects.Columns.WebPartHelpText]);
                                        if (Convert.ToString(item[SPDatabaseObjects.Columns.TokenName]) == "ApplicationAccess" && Convert.ToString(item[SPDatabaseObjects.Columns.SWQuestionType]) == "ApplicationAccessRequest")
                                        {
                                            List<string> strval = new List<string>();
                                            strval = UGITUtility.SplitString((Convert.ToString(item[SPDatabaseObjects.Columns.QuestionTypeProperties])), ";#").ToList();
                                            string appstr = strval.FirstOrDefault(x => x.Contains("application"));
                                            List<string> appstrval = new List<string>();
                                            List<string> newstr = new List<string>();
                                            List<string> finalstr = new List<string>();
                                            appstrval = UGITUtility.SplitString(appstr, ";~").ToList();
                                            foreach (string val in appstrval)
                                            {
                                                newstr = UGITUtility.SplitString(val, "-").ToList();
                                                string finalval = appmappedList.GetTargetID(newstr[2]);
                                                finalstr.Add(val.Replace(newstr[2], finalval));
                                            }
                                            appstr = UGITUtility.ConvertListToString(finalstr, ";~");
                                            finalstr.Clear();
                                            foreach (string val in strval)
                                            {
                                                if (val.Contains("application"))
                                                {
                                                    finalstr.Add(appstr);
                                                }
                                                else
                                                {
                                                    finalstr.Add(val);
                                                }
                                            }
                                            targetItem.QuestionTypeProperties = UGITUtility.ConvertListToString(finalstr, ";#");

                                        }
                                        else if (Convert.ToString(item[SPDatabaseObjects.Columns.SWQuestionType]) == "RequestType")
                                        {
                                            List<string> strval = new List<string>();
                                            strval = UGITUtility.SplitString((Convert.ToString(item[SPDatabaseObjects.Columns.QuestionTypeProperties])), ";#").ToList();
                                            string appstr = strval.FirstOrDefault(x => x.Contains("requesttypes"));

                                            List<string> appstrval = new List<string>();
                                            List<string> newstr = new List<string>();
                                            List<string> finalstr = new List<string>();
                                            appstrval = UGITUtility.SplitString(appstr, "=").ToList();
                                            foreach (string val in appstrval)
                                            {
                                                if (val == "requesttypes")
                                                    continue;
                                                newstr = UGITUtility.SplitString(val, "~").ToList();
                                                foreach (var _item in newstr)
                                                {
                                                    string finalval = RequestTypeMappedList.GetTargetID(_item);
                                                    finalstr.Add(finalval == null ? "0" : finalval);
                                                }
                                            }
                                            if (finalstr.Count > 0)
                                                appstr = UGITUtility.ConvertListToString(finalstr, "~");
                                            finalstr.Clear();
                                            foreach (string val in strval)
                                            {
                                                if (val.Contains("requesttypes"))
                                                {
                                                    finalstr.Add("requesttypes=" + appstr);
                                                }
                                                else
                                                {
                                                    finalstr.Add(val);
                                                }
                                            }
                                            targetItem.QuestionTypeProperties = UGITUtility.ConvertListToString(finalstr, ";#");
                                        }
                                        else
                                            targetItem.QuestionTypeProperties = Convert.ToString(item[SPDatabaseObjects.Columns.QuestionTypeProperties]);
                                        targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                                        targetItem.FieldMandatory = Convert.ToBoolean(item[SPDatabaseObjects.Columns.FieldMandatory]);
                                        targetItem.ContinueSameLine = Convert.ToBoolean(item["ContinueSameLine"]);
                                    }
                                    if (targetItem.ID > 0)
                                        mgr.Update(targetItem);
                                    else
                                        mgr.Insert(targetItem);
                                }

                                if (targetItem != null)
                                    locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex);
                            }

                        }
                    } while (position != null);

                    if (import)
                        ULog.WriteLog($"{targetNewItemCount} Service Questions added");

                    /*Here we need to update newuser key*/
                    try
                    {
                        foreach (ServiceQuestion q in dbData)
                        {
                            List<string> strval = new List<string>();
                            strval = UGITUtility.SplitString(q.QuestionTypeProperties, ";#").ToList();
                            string appstr = strval.FirstOrDefault(x => x.Contains("newuser"));
                            if (string.IsNullOrEmpty(appstr))
                                continue;
                            List<string> appstrval = new List<string>();
                            List<string> newstr = new List<string>();
                            List<string> finalstr = new List<string>();
                            appstrval = UGITUtility.SplitString(appstr, "=").ToList();
                            foreach (string val in appstrval)
                            {
                                if (val == "newuser")
                                    continue;
                                newstr = UGITUtility.SplitString(val, ",").ToList();
                                foreach (var item in newstr)
                                {
                                    string finalval = locaionlist.GetTargetID(item);
                                    finalstr.Add(finalval == null ? "0" : finalval);
                                }
                            }
                            if (finalstr.Count > 0)
                                appstr = UGITUtility.ConvertListToString(finalstr, ",");
                            finalstr.Clear();
                            foreach (string val in strval)
                            {
                                if (val.Contains("newuser"))
                                {
                                    finalstr.Add("newuser=" + appstr);
                                }
                                else
                                {
                                    finalstr.Add(val);
                                }
                            }
                            q.QuestionTypeProperties = UGITUtility.ConvertListToString(finalstr, ";#");

                            mgr.Update(q);

                        }
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex);
                    }
                }
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

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ServiceTicketRelationships;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    MappedItemList servicelist = context.GetMappedList(SPDatabaseObjects.Lists.Services);
                    MappedItemList RequestTypeMappedList = context.GetMappedList(SPDatabaseObjects.Lists.RequestType);
                    UGITTask targetItem = null;
                    MappedItemList userlst = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]) && x.ModuleNameLookup == Convert.ToString(item[SPDatabaseObjects.Columns.ModuleNameLookup]));
                            targetItem = null;
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
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    string relatedmodule = item[SPDatabaseObjects.Columns.ModuleNameLookup] == null ? null : Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleNameLookup]).LookupValue);
                                    targetItem.RelatedModule = relatedmodule;
                                    if (!string.IsNullOrEmpty(relatedmodule))
                                        targetItem.Behaviour = "Ticket";
                                    else
                                        targetItem.Behaviour = "Task";

                                    targetItem.Status = Convert.ToString(item[SPDatabaseObjects.Columns.Status]);
                                    targetItem.ModuleNameLookup = "SVCConfig";
                                    ClientOM.FieldLookupValue ServicedLookup = item[SPDatabaseObjects.Columns.ServiceTitleLookup] as ClientOM.FieldLookupValue;
                                    if (ServicedLookup != null)
                                    {
                                        targetItem.TicketId = servicelist.GetTargetID(UGITUtility.ObjectToString(ServicedLookup.LookupId));
                                    }

                                    targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                                    targetItem.Priority = Convert.ToString(item[SPDatabaseObjects.Columns.TaskPriority]);
                                    targetItem.PercentComplete = item[SPDatabaseObjects.Columns.PercentComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.PercentComplete]) * 100;
                                    targetItem.body = Convert.ToString(item[SPDatabaseObjects.Columns.Body]);
                                    targetItem.ParentTaskID = Convert.ToInt32(item[SPDatabaseObjects.Columns.ParentTask]);
                                    targetItem.StartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.StartDate]);
                                    targetItem.DueDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.DueDate]);
                                    targetItem.EstimatedHours = Convert.ToDouble(item[SPDatabaseObjects.Columns.TaskEstimatedHours]);
                                    targetItem.Level = Convert.ToInt32(item[SPDatabaseObjects.Columns.UGITLevel]);
                                    targetItem.SubTaskType = Convert.ToString(item[SPDatabaseObjects.Columns.UGITSubTaskType]);
                                    targetItem.NewUserName = Convert.ToString(item[SPDatabaseObjects.Columns.UGITNewUserName]);
                                    targetItem.EnableApproval = Convert.ToBoolean(item[SPDatabaseObjects.Columns.EnableApproval]);
                                    targetItem.ApprovalStatus = Convert.ToString(item[SPDatabaseObjects.Columns.Approvalstatus]);
                                    targetItem.SLADisabled = Convert.ToBoolean(item[SPDatabaseObjects.Columns.SLADisabled]);
                                    targetItem.AutoCreateUser = Convert.ToBoolean(item[SPDatabaseObjects.Columns.AutoCreateUser]);
                                    targetItem.AutoFillRequestor = Convert.ToBoolean(item[SPDatabaseObjects.Columns.AutoFillRequestor]);
                                    targetItem.NotificationDisabled = Convert.ToBoolean(item[SPDatabaseObjects.Columns.NotificationDisabled]);
                                    targetItem.QuestionID = Convert.ToString(item[SPDatabaseObjects.Columns.QuestionID]);
                                    targetItem.QuestionProperties = Convert.ToString(item[SPDatabaseObjects.Columns.QuestionProperties]);
                                    ClientOM.FieldLookupValue[] Predecessors_ = item[SPDatabaseObjects.Columns.Predecessors] as ClientOM.FieldLookupValue[];
                                    List<string> pressors = new List<string>();
                                    foreach (ClientOM.FieldLookupValue pred in Predecessors_)
                                    {
                                        pressors.Add(Convert.ToString(pred.LookupValue));
                                    }
                                    targetItem.Predecessors = string.Join(", ", pressors.ToArray());

                                    //string[] arrPredecessors = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue[])item[DatabaseObjects.Columns.Predecessors])[0]).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                    //List<string> pressors = new List<string>();
                                    //if (arrPredecessors != null && arrPredecessors.Count() > 0)
                                    //{
                                    //    pressors.Clear();
                                    //    foreach (var pred in arrPredecessors)
                                    //    {
                                    //        if (pred == "Microsoft.SharePoint.Client.FieldLookupValue[]")
                                    //            continue;
                                    //        pressors.Add(Convert.ToString(pred));
                                    //    }
                                    //    targetItem.Predecessors = string.Join(", ", pressors.ToArray());
                                    //}
                                    //targetItem.ProposedStatus = (UGITTaskProposalStatus)Enum.Parse(typeof(UGITTaskProposalStatus), item[SPDatabaseObjects.Columns.UGITProposedStatus].ToString());
                                    //targetItem.Comment = Convert.ToString(item[SPDatabaseObjects.Columns.UGITComment]);
                                    ClientOM.FieldUserValue[] _users = item[SPDatabaseObjects.Columns.AssignedTo] as ClientOM.FieldUserValue[];
                                    if (_users != null && _users.Length > 0 && userlst != null)
                                        targetItem.AssignedTo = userlst.GetTargetIDs(_users.Select(x => x.LookupId.ToString()).ToList());

                                    //targetItem.PredecessorTasks
                                }
                                try
                                {
                                    if (targetItem.ID > 0)
                                        mgr.Update(targetItem);
                                    else
                                        mgr.Insert(targetItem);
                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex);
                                }

                            }

                            if (targetItem != null)
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                    } while (position != null);

                    if (import)
                        ULog.WriteLog($"{targetNewItemCount} Service Tasks and Tickets");
                }
            }

            UpdateServiceTaskPredecessors();
        }

        private void UpdateServiceTaskPredecessors()
        {
            ServicesManager srvMgr = new ServicesManager(context.AppContext);
            List<Services> srvDbData = srvMgr.Load();

            UGITTaskManager mgr = new UGITTaskManager(context.AppContext);
            List<UGITTask> dbData = mgr.Load(x => srvDbData.Any(y => x.TicketId.Equals(Convert.ToString(y.ID))));
            List<string> strval = new List<string>();
            List<string> strvalReplace = new List<string>();
            dbData.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.Predecessors))
                {
                    strvalReplace.Clear();
                    strval = UGITUtility.SplitString((x.Predecessors), ",").ToList();
                    foreach (string item in strval)
                    {
                        long val = dbData.FirstOrDefault(y => y.Title == item.Trim()).ID;
                        strvalReplace.Add(Convert.ToString(val));
                    }
                    x.Predecessors = string.Join(", ", strvalReplace.ToArray());
                }
                //x.Predecessors = Convert.ToString(context.GetTargetLookupValueOptionLong(Convert.ToString(x.Predecessors), DatabaseObjects.Columns.Predecessors, DatabaseObjects.Tables.ModuleTasks));
            });

            mgr.UpdateItems(dbData);
        }

        private void UpdateServiceMappings(bool import)
        {
            int targetNewItemCount = 0;
            Services services = null;
            ServiceQuestion serviceQuestion = null;


            FieldConfigurationManager configFieldManager = new FieldConfigurationManager(context.AppContext);
            UGITTaskManager taskmgr = new UGITTaskManager(context.AppContext);
            List<UGITTask> tasklist = taskmgr.Load();
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
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ServiceTicketDefaultValues;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList ModuleTaskslist = context.GetMappedList("ServiceTicketRelationships");
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    ServiceQuestionMapping targetItem = null;
                    long ServiceId = 0;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        foreach (ClientOM.ListItem item in collection)
                        {
                            long QuesId = 0;
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.ColumnName]));
                            targetItem = null;
                            ClientOM.FieldLookupValue ServicedLookup = item[SPDatabaseObjects.Columns.ServiceTitleLookup] as ClientOM.FieldLookupValue;
                            ClientOM.FieldLookupValue ServicedTaskLookup = item[SPDatabaseObjects.Columns.ServiceTicketTitleLookup] as ClientOM.FieldLookupValue;
                            ClientOM.FieldLookupValue ServiceQuesLookup = item[SPDatabaseObjects.Columns.ServiceQuestionTitleLookup] as ClientOM.FieldLookupValue;
                            if (ServicedLookup != null)
                            {
                                if (!string.IsNullOrEmpty(ServicedLookup.LookupValue))
                                {
                                    ServiceId = srvDbData.FirstOrDefault(x => x.Title == ServicedLookup.LookupValue).ID;
                                }
                            }

                            if (ServiceQuesLookup != null)
                            {
                                if (!string.IsNullOrEmpty(ServiceQuesLookup.LookupValue))
                                {
                                    QuesId = srvQDbData.FirstOrDefault(y => y.QuestionTitle == ServiceQuesLookup.LookupValue).ID;
                                }
                            }

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
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    if (Convert.ToString(item[SPDatabaseObjects.Columns.ColumnName]).EndsWith("Lookup") && Convert.ToString(item[SPDatabaseObjects.Columns.ColumnName]).StartsWith("Ticket"))
                                        targetItem.ColumnName = Convert.ToString(item[SPDatabaseObjects.Columns.ColumnName]).Replace("Ticket", "");
                                    else
                                        targetItem.ColumnName = Convert.ToString(item[SPDatabaseObjects.Columns.ColumnName]);
                                    targetItem.ColumnValue = Convert.ToString(item[SPDatabaseObjects.Columns.ColumnValue]);
                                    targetItem.PickValueFrom = Convert.ToString(item[SPDatabaseObjects.Columns.PickValueFrom]);
                                    targetItem.ServiceID = ServiceId;
                                    if (QuesId > 0)
                                        targetItem.ServiceQuestionID = QuesId;
                                    if (ServicedTaskLookup != null && ServicedTaskLookup.LookupId > 0)
                                    {

                                        targetItem.ServiceTaskID = UGITUtility.StringToLong(ModuleTaskslist.GetTargetID(Convert.ToString(ServicedTaskLookup.LookupId)));
                                        string moduleName = tasklist.FirstOrDefault(y => y.ID == targetItem.ServiceTaskID).RelatedModule;
                                        if (!string.IsNullOrEmpty(moduleName) && targetItem.ColumnName.EndsWith("Lookup"))
                                        {
                                            string val = configFieldManager.GetFieldConfigurationIdByName(targetItem.ColumnName, targetItem.ColumnValue, moduleName);
                                            if (!string.IsNullOrEmpty(val))
                                                targetItem.ColumnValue = val;
                                        }

                                    }
                                }
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }

                            if (targetItem != null)
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                    } while (position != null);

                    if (import)
                        ULog.WriteLog($"{targetNewItemCount} Service Mappings");
                }
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
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
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

                string listName = SPDatabaseObjects.Lists.ACRTypes;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                ACRType targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        //targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                        targetItem = null;
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.Deleted = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (users != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} ACRTypes added");
            }
        }

        public override void UpdateDRQRapidTypes()
        {
            base.UpdateDRQRapidTypes();

            bool import = context.IsImportEnable("DRQRapidTypes");

            if (import)
                ULog.WriteLog($"Updating DRQRapidTypes");
            else
                ULog.WriteLog($"Load DRQRapidTypes Mapping");
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
            int targetNewItemCount = 0;
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.DRQRapidTypes;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                DRQRapidType targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (users != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                                targetItem.Deleted = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} DRQRapidTypes added");
            }

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

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.DRQSystemAreas;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                DRQSystemArea targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.Deleted = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} DRQSystemAreas added");
            }

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
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.Environment;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                UGITEnvironment targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                                targetItem.Deleted = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} Environment added");
            }
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

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.SubLocation;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                MappedItemList locationMappedList = context.GetMappedList(SPDatabaseObjects.Lists.Location);

                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                SubLocation targetItem = null;
                long targetLocationID = 0;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        targetItem = dbSlData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                        if (locationMappedList != null)
                            if (item[SPDatabaseObjects.Columns.LocationLookup] != null)
                            {
                                targetLocationID = UGITUtility.StringToLong(locationMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.LocationLookup]).LookupId.ToString()));
                            }


                        if (targetLocationID <= 0)
                            continue;
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.LocationID = targetLocationID;
                                targetItem.LocationTag = Convert.ToString(item[SPDatabaseObjects.Columns.LocationTag]);
                                targetItem.Phone = Convert.ToString(item[SPDatabaseObjects.Columns.Phone]);
                                targetItem.Address1 = Convert.ToString(item[SPDatabaseObjects.Columns.Address1]);
                                targetItem.Address2 = Convert.ToString(item[SPDatabaseObjects.Columns.Address2]);
                                targetItem.Zip = Convert.ToString(item[SPDatabaseObjects.Columns.Zip]);
                                targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                            }
                            if (targetItem.ID > 0)
                                slMgr.Update(targetItem);
                            else
                                slMgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} SubLocation added");
            }

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
            List<BusinessStrategy> dbData = bsMgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    bsMgr.Delete(dbData);
                    dbData = new List<BusinessStrategy>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.BusinessStrategy;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                BusinessStrategy targetItem = null;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                        if (import)
                        {
                            if (targetItem == null)
                            {
                                targetItem = new BusinessStrategy();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.TicketDescription]);
                            }
                            if (targetItem.ID > 0)
                                bsMgr.Update(targetItem);
                            else
                                bsMgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} Business Strategy added");
            }
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

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.ProjectInitiative;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                ProjectInitiative targetItem = null;
                MappedItemList ProjectInitiativeList = context.GetMappedList(SPDatabaseObjects.Lists.BusinessStrategy);
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        targetItem = dbPiData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                        if (import)
                        {
                            if (targetItem == null)
                            {
                                targetItem = new ProjectInitiative();
                                dbPiData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ProjectNote = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectNote]);
                                if (context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.BusinessStrategyLookup) != null)
                                    targetItem.BusinessStrategyLookup = Convert.ToInt64(context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.BusinessStrategyLookup));
                            }
                            if (targetItem.ID > 0)
                                piMgr.Update(targetItem);
                            else
                                piMgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} Project Initiative added");
            }

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
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.ProjectClass;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                ProjectClass targetItem = null;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ProjectNote = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectNote]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} ProjectClass added");
            }
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
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.ProjectStandardWorkItems;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                MappedItemList MappBudgetList = context.GetMappedList(SPDatabaseObjects.Lists.BudgetCategories);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                ProjectStandardWorkItem targetItem = null;
                long budgetcatid = 0;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                        if (MappBudgetList != null)
                            if (item[SPDatabaseObjects.Columns.BudgetCategoryLookup] != null)
                            {
                                budgetcatid = UGITUtility.StringToLong(MappBudgetList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.BudgetCategoryLookup]).LookupId.ToString()));
                            }


                        if (budgetcatid <= 0)
                            continue;
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ItemOrder = Convert.ToString(item[SPDatabaseObjects.Columns.ItemOrder]);
                                targetItem.BudgetCategoryLookup = Convert.ToInt32(budgetcatid);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} Project Standards added");
            }
        }
        public override void UpdateGlobalRoles()
        {
            // Need to discuss
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
            TenantManager tenantManager = new TenantManager(ApplicationContext.Create());
            var masterTenant = tenantManager.GetTenant(ConfigHelper.DefaultTenant.ToLower());
            List<Tenant> tList = tenantManager.GetTenantList();
            Tenant tenant = null;
            ApplicationContext scontext = null;
            ConfigurationVariableManager ObjConfigVariable = new ConfigurationVariableManager(ApplicationContext.Create());
            if (masterTenant != null)
            {
                string ModelSite = ObjConfigVariable.GetValue(ConfigConstants.ModelSite);
                var name = uGITDAL.GetSingleValue(DatabaseObjects.Tables.ConfigurationVariable, $"{DatabaseObjects.Columns.TenantID}='{masterTenant.TenantID}'and {DatabaseObjects.Columns.KeyName}='{ConfigConstants.ModelSite}'", DatabaseObjects.Columns.KeyValue);
                if (!string.IsNullOrEmpty(Convert.ToString(name)))
                {
                    tenant = tList.FirstOrDefault(x => x.TenantName == UGITUtility.ObjectToString(name) || x.AccountID == UGITUtility.ObjectToString(name));
                    scontext = ApplicationContext.CreateContext(Convert.ToString(tenant.TenantID));
                }
            }

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

            LandingPagesManager sourceMgr = new LandingPagesManager(scontext);
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
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.EmployeeTypes;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                EmployeeTypes targetItem = null;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} EmployeeTypes added");
            }

        }
        public override void UpdateuGovernITLogs()
        {
            base.UpdateuGovernITLogs();

            bool import = context.IsImportEnable("uGovernITLogs");
            if (!import)
                return;
            if (import)
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
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.UGITLog;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                UGITLog targetItem = null;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        //targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                        targetItem = null;
                        if (import)
                        {
                            if (targetItem == null)
                            {
                                targetItem = new UGITLog();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ModuleNameLookup = Convert.ToString(item[SPDatabaseObjects.Columns.ModuleName]);
                                targetItem.CategoryName = Convert.ToString(item[SPDatabaseObjects.Columns.CategoryName]);
                                targetItem.Severity = Convert.ToString(item[SPDatabaseObjects.Columns.Severity]);
                                targetItem.ItemUser = Convert.ToString(item[SPDatabaseObjects.Columns.TicketUser]);
                                targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                                targetItem.TicketId = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} uGovernITLogs added");
            }

        }
        public override void UpdateGovernanceConfiguration()
        {
            base.UpdateGovernanceConfiguration();

            bool import = context.IsImportEnable("GovernanceConfiguration");

            if (import)
            {
                ULog.WriteLog($"Updating Governance Configuration");

                GovernanceLinkCategory(import);
                GovernanceLinkItems(import);
            }
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

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.GovernanceLinkCategory;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                GovernanceLinkCategory targetItem = null;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.CategoryName = Convert.ToString(item[SPDatabaseObjects.Columns.CategoryName]);
                                targetItem.ImageUrl = Convert.ToString(item[SPDatabaseObjects.Columns.UGITImageUrl]);
                                targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} Governance LinkCategory added");
            }
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
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                GovernanceLinkCategoryManager _mgr = new GovernanceLinkCategoryManager(context.AppContext);
                List<GovernanceLinkCategory> _dbData = _mgr.Load();

                string listName = SPDatabaseObjects.Lists.GovernanceLinkItems;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                GovernanceLinkItem targetItem = null;
                long GovernanceLinkCategoryLookupid = 0;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    try
                    {
                        foreach (ClientOM.ListItem item in collection)
                        {
                            string GovernanceLinkCategoryLookup = item[SPDatabaseObjects.Columns.GovernanceLinkCategoryLookup] == null ? string.Empty : Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.GovernanceLinkCategoryLookup]).LookupValue);
                            if (!string.IsNullOrEmpty(GovernanceLinkCategoryLookup))
                            {
                                GovernanceLinkCategoryLookupid = _dbData.FirstOrDefault(x => x.CategoryName == GovernanceLinkCategoryLookup).ID;
                            }
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
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
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    targetItem.ImageUrl = Convert.ToString(item[SPDatabaseObjects.Columns.UGITImageUrl]);
                                    targetItem.CustomProperties = Convert.ToString(item[SPDatabaseObjects.Columns.CustomProperties]);
                                    targetItem.TabSequence = Convert.ToInt32(item[SPDatabaseObjects.Columns.TabSequence]);
                                    targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                                    //targetItem.CategoryName = Convert.ToString(item[SPDatabaseObjects.Columns.CategoryName]);
                                    targetItem.TargetType = Convert.ToString(item[SPDatabaseObjects.Columns.TargetType]);
                                    targetItem.GovernanceLinkCategoryLookup = GovernanceLinkCategoryLookupid;
                                }
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }

                            if (targetItem != null)
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex);
                    }

                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} Governance Links added");
            }
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
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.ModuleMonitors;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                ModuleMonitor targetItem = null;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    try
                    {
                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
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
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    targetItem.ModuleNameLookup = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleNameLookup]).LookupValue);
                                    targetItem.MonitorName = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectMonitorName]);

                                    ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                    if (users != null && userlist != null)
                                        targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                    ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                    if (Modifeduser != null && userlist != null)
                                        targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                    targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                    targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);

                                }
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }

                            if (targetItem != null)
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex);
                    }

                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} ModuleMonitors added");
            }
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
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.ModuleMonitorOptions;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                MappedItemList MappedMonitorlist = context.GetMappedList(SPDatabaseObjects.Lists.ModuleMonitors);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                ModuleMonitorOption targetItem = null;
                long modulemonitorid = 0;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                        if (MappedMonitorlist != null)
                            if (item[SPDatabaseObjects.Columns.ModuleMonitorNameLookup] != null)
                            {
                                modulemonitorid = UGITUtility.StringToLong(MappedMonitorlist.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleMonitorNameLookup]).LookupId.ToString()));
                            }


                        if (modulemonitorid <= 0)
                            continue;
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ModuleMonitorOptionName = Convert.ToString(item[SPDatabaseObjects.Columns.ModuleMonitorOption]);
                                targetItem.ModuleMonitorOptionLEDClass = Convert.ToString(item["ModuleMonitorOptionLEDClass"]);
                                targetItem.ModuleMonitorMultiplier = Convert.ToInt32(item["ModuleMonitorMultiplier"]);
                                targetItem.IsDefault = true;
                                targetItem.ModuleMonitorNameLookup = modulemonitorid;

                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (users != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} ModuleMonitorOptions added");
            }

        }

        public override void UpdateProjectComplexity()
        {
            //base.UpdateProjectComplexity();

            //bool import = context.IsImportEnable("ProjectComplexity");

            //if (import)
            //    ULog.WriteLog($"Updating ProjectComplexity");

            //int targetNewItemCount = 0;

            //ProjectComplexityManager mgr = new ProjectComplexityManager(context.AppContext);
            //List<ProjectComplexity> dbData = mgr.Load();
            //if (import && deleteBeforeImport)
            //{
            //    try
            //    {
            //        mgr.Delete(dbData);
            //        dbData = new List<ProjectComplexity>();
            //    }
            //    catch (Exception ex)
            //    {
            //        ULog.WriteLog("Problem while deleting records");
            //        ULog.WriteException(ex);
            //    }
            //}
            //using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            //{
            //    string listName = SPDatabaseObjects.Lists.ProjectComplexity;
            //    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
            //    MappedItemList locaionlist = context.GetMappedList(listName);
            //    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
            //    ProjectComplexity targetItem = null;
            //    do
            //    {
            //        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
            //        position = collection.ListItemCollectionPosition;
            //        foreach (ClientOM.ListItem item in collection)
            //        {
            //            targetItem = dbData.FirstOrDefault(x => x.CRMProjectComplexity == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
            //            if (import)
            //            {
            //                if (targetItem == null)
            //                {
            //                    targetItem = new ProjectComplexity();
            //                    dbData.Add(targetItem);
            //                    targetNewItemCount++;
            //                }
            //                if (targetItem.ID == 0 || importWithUpdate)
            //                {
            //                    targetItem.CRMProjectComplexity = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
            //                }
            //                if (targetItem.ID > 0)
            //                    mgr.Update(targetItem);
            //                else
            //                    mgr.Insert(targetItem);
            //            }
            //            if (targetItem != null)
            //                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
            //        }
            //    } while (position != null);

            //    if (import)
            //        ULog.WriteLog($"{targetNewItemCount} v added");
            //}
            //ProjectComplexityManager sourceMgr = new ProjectComplexityManager(context.SourceAppContext);
            //List<ProjectComplexity> sourceDbData = sourceMgr.Load();
            //ProjectComplexity targetItem = null;
            //foreach (ProjectComplexity item in sourceDbData)
            //{
            //    targetItem = dbData.FirstOrDefault(x => x.CRMProjectComplexity == item.CRMProjectComplexity && x.MinValue == item.MinValue && x.MaxValue == item.MaxValue);
            //    if (import)
            //    {
            //        if (targetItem == null)
            //        {
            //            targetItem = new ProjectComplexity();
            //            dbData.Add(targetItem);
            //            targetNewItemCount++;
            //        }

            //        if (targetItem.ID == 0 || importWithUpdate)
            //        {
            //            item.ID = targetItem.ID;
            //            item.TenantID = targetItem.TenantID;
            //            item.ModifiedBy = targetItem.ModifiedBy;
            //            item.CreatedBy = targetItem.CreatedBy;
            //            targetItem = item;

            //            if (targetItem.ID > 0)
            //                mgr.Update(targetItem);
            //            else
            //                mgr.Insert(targetItem);
            //        }
            //    }

            //}
            //ULog.WriteLog($"{targetNewItemCount} ProjectComplexity added");
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
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.MailTokenColumnName;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                MailTokenColumnName targetItem = null;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        //targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                        targetItem = null;
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.KeyName = Convert.ToString(item[SPDatabaseObjects.Columns.KeyName]);
                                targetItem.KeyValue = Convert.ToString(item[SPDatabaseObjects.Columns.KeyValue]);
                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (users != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} MailTokenColumnName added");
            }

        }

        public override void UpdateGenericStatus()
        {
            base.UpdateGenericStatus();

            bool import = context.IsImportEnable("GenericStatus");

            if (import)
                ULog.WriteLog($"Updating GenericStatus");

            int targetNewItemCount = 0;

            if (import)
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

            //System.Data.DataTable dbData = GetTableDataManager.GetTableData(DatabaseObjects.Tables.GenericTicketStatus, $"{DatabaseObjects.Columns.TenantID} = '{context.AppContext.TenantID}'");
            System.Data.DataTable dbData = GetTableDataManager.GetTableStructure(DatabaseObjects.Tables.GenericTicketStatus);
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.GenericTicketStatus;
                MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    System.Data.DataRow dr = null;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        if (dbData != null || dbData.Rows.Count == 0)
                        {
                            dr = dbData.NewRow();
                            dr[DatabaseObjects.Columns.Title] = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                            dr[DatabaseObjects.Columns.GenericStatus] = Convert.ToString(item[SPDatabaseObjects.Columns.GenericStatus]);

                            ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                            if (users != null && userlist != null)
                                dr[DatabaseObjects.Columns.CreatedByUser] = userlist.GetTargetID(Convert.ToString(users.LookupId));
                            ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                            if (Modifeduser != null && userlist != null)
                                dr[DatabaseObjects.Columns.ModifiedByUser] = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                            dr[DatabaseObjects.Columns.Created] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                            dr[DatabaseObjects.Columns.Modified] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                            dr[DatabaseObjects.Columns.TenantID] = context.AppContext.TenantID;
                            dbData.Rows.Add(dr);
                        }
                    }
                    if (import)
                    {
                        GetTableDataManager.bulkupload(dbData, DatabaseObjects.Tables.GenericTicketStatus);
                        targetNewItemCount = dbData.Rows.Count;
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} GenericStatus added");
            }
        }

        public override void UpdateLinkViews()
        {
            base.UpdateLinkViews();

            bool import = context.IsImportEnable("LinkViews");

            if (import)
            {
                ULog.WriteLog($"Updating LinkViews");
                LinkView(import);
                LinkCategory(import);
                LinkItems(import);
            }

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
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.LinkView;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                LinkView targetItem = null;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.CategoryName = Convert.ToString(item[SPDatabaseObjects.Columns.CategoryName]);
                                targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} LinkView added");
            }

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
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.LinkCategory;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                LinkCategory targetItem = null;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    LinkViewManager lvMgr = new LinkViewManager(context.AppContext);
                    List<LinkView> dblvData = lvMgr.Load();
                    try
                    {
                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            string strlnkview = item[SPDatabaseObjects.Columns.LinkViewLookup] == null ? string.Empty : (((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.LinkViewLookup]).LookupValue);
                            long lnkviewid = dblvData.FirstOrDefault(x => x.Title == strlnkview).ID;
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
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                                    targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                                    targetItem.ImageUrl = Convert.ToString(item[SPDatabaseObjects.Columns.UGITImageUrl]);
                                    targetItem.LinkViewLookup = lnkviewid;
                                }
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }

                            if (targetItem != null)
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex);
                    }

                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} LinkCategory added");
            }

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

            LinkCategoryManager lcSourceMgr = new LinkCategoryManager(context.AppContext);
            List<LinkCategory> dbLcSourceData = lcSourceMgr.Load();

            LinkItemManager sourceMgr = new LinkItemManager(context.AppContext);
            List<LinkItems> sourceDbData = sourceMgr.Load();
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.LinkItems;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                LinkItems targetItem = null;
                MappedItemList userMappedList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    try
                    {
                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            string strcat = item[SPDatabaseObjects.Columns.LinkCategoryLookup] == null ? string.Empty : (((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.LinkCategoryLookup]).LookupValue);
                            long catid = dbLcSourceData.FirstOrDefault(x => x.Title == strcat).ID;
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
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                                    targetItem.CustomProperties = Convert.ToString(item[SPDatabaseObjects.Columns.CustomProperties]);
                                    targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                                    targetItem.TargetType = Convert.ToString(item[SPDatabaseObjects.Columns.TargetType]);
                                    targetItem.DisableDiscussion = Convert.ToBoolean(item[SPDatabaseObjects.Columns.DisableDiscussion]);
                                    targetItem.BottomPaneExpanded = Convert.ToBoolean(item[SPDatabaseObjects.Columns.BottomPaneExpanded]);
                                    targetItem.LeftPaneExpanded = Convert.ToBoolean(item[SPDatabaseObjects.Columns.LeftPaneExpanded]);
                                    targetItem.DisableRelatedItems = Convert.ToBoolean(item[SPDatabaseObjects.Columns.DisableRelatedItems]);
                                    targetItem.LinkCategoryLookup = catid;
                                    targetItem.AuthorizedToView = null;
                                    ClientOM.FieldUserValue[] users = item[SPDatabaseObjects.Columns.AuthorizedToView] as ClientOM.FieldUserValue[];
                                    if (users != null && users.Length > 0 && userMappedList != null)
                                        targetItem.AuthorizedToView = userMappedList.GetTargetIDs(users.Select(x => x.LookupId.ToString()).ToList());
                                }
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }

                            if (targetItem != null)
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex);
                    }

                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} LinkItems added");
            }
        }

        public override void UpdateTenantScheduler()
        {
            // This method is applicable for .Net only.
        }

        public override void UpdateSurveyFeedback()
        {
            base.UpdateSurveyFeedback();

            bool import = context.IsImportEnable("SurveyFeedback");
            if (!import)
                return;

            if (import)
                ULog.WriteLog($"Updating SurveyFeedback");

            int targetNewItemCount = 0;

            SurveyFeedbackManager mgr = new SurveyFeedbackManager(context.AppContext);
            List<SurveyFeedback> dbData = mgr.Load();
            ServicesManager srvMgr = new ServicesManager(context.AppContext);
            List<Services> srvDbData = srvMgr.Load();
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
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.SurveyFeedback;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                SurveyFeedback targetItem = null;
                long ServiceId = 0;
                do
                {
                    MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                        ClientOM.FieldLookupValue ServicedLookup = item[SPDatabaseObjects.Columns.ServiceTitleLookup] as ClientOM.FieldLookupValue;
                        if (ServicedLookup != null)
                        {
                            if (!string.IsNullOrEmpty(ServicedLookup.LookupValue))
                            {
                                ServiceId = srvDbData.FirstOrDefault(x => x.Title == ServicedLookup.LookupValue).ID;
                            }
                        }
                        if (import)
                        {
                            if (targetItem == null)
                            {
                                targetItem = new SurveyFeedback();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                try
                                {
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    targetItem.ServiceLookUp = ServiceId;
                                    targetItem.Rating1 = Convert.ToInt32(item[SPDatabaseObjects.Columns.Rating1]);
                                    targetItem.Rating2 = Convert.ToInt32(item[SPDatabaseObjects.Columns.Rating2]);
                                    targetItem.Rating3 = Convert.ToInt32(item[SPDatabaseObjects.Columns.Rating3]);
                                    targetItem.Rating4 = Convert.ToInt32(item[SPDatabaseObjects.Columns.Rating4]);
                                    targetItem.Rating5 = Convert.ToInt32(item[SPDatabaseObjects.Columns.Rating5]);
                                    targetItem.Rating6 = Convert.ToInt32(item[SPDatabaseObjects.Columns.Rating6]);
                                    targetItem.Rating7 = Convert.ToInt32(item[SPDatabaseObjects.Columns.Rating7]);
                                    targetItem.Rating8 = Convert.ToInt32(item[SPDatabaseObjects.Columns.Rating8]);
                                    targetItem.TotalRating = Convert.ToInt32(item[SPDatabaseObjects.Columns.TotalRating]);
                                    targetItem.UserDepartment = Convert.ToString(item[SPDatabaseObjects.Columns.UserDepartment]);
                                    targetItem.UserLocation = Convert.ToString(item[SPDatabaseObjects.Columns.UserLocation]);
                                    targetItem.CategoryName = Convert.ToString(item[SPDatabaseObjects.Columns.CategoryName]);
                                    targetItem.SubCategory = Convert.ToString(item[SPDatabaseObjects.Columns.SubCategory]);
                                    targetItem.Description = Convert.ToString(item[DatabaseObjects.Columns.Description]);
                                    targetItem.ModuleName = Convert.ToString(item[SPDatabaseObjects.Columns.ModuleName]);
                                    ClientOM.FieldUserValue usersPRP = item[SPDatabaseObjects.Columns.TicketPRP] as ClientOM.FieldUserValue;
                                    if (usersPRP != null && userList != null)
                                        targetItem.PRPUser = userList.GetTargetID(Convert.ToString(usersPRP.LookupId));

                                    ClientOM.FieldUserValue[] owneruser = item[SPDatabaseObjects.Columns.TicketOwner] as ClientOM.FieldUserValue[];
                                    if (owneruser != null && userList != null)
                                        targetItem.OwnerUser = userList.GetTargetIDs(owneruser.Select(x => x.LookupId.ToString()).ToList());

                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex);
                                }


                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                        if (targetItem != null)
                            locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} SurveyFeedback added");
            }
        }

        public override void UpdatePhrases()
        {
            // This method is applicable for .Net only.
        }

        public override void UpdateWidgets()
        {
            // This method is applicable for .Net only.
        }
        public override void UpdateDocuments()
        {
            base.UpdateDocuments();

            bool import = context.IsImportEnable("DocumentManagement");

            CopyDirectories();
            CopyDocuments();
        }
        private void CopyDirectories()
        {
            //MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.DMSDirectory);

            //List<DMSDirectory> directories = new List<DMSDirectory>();

            //using (DatabaseContext ctx = new DatabaseContext(context.SourceAppContext))
            //{
            //    directories = ctx.DMSDirectories.Where(x => x.TenantID.EqualsIgnoreCase(context.SourceAppContext.TenantID)).ToList();
            //    if (directories.Count > 0)
            //    {
            //        int oldDirId = 0;
            //        foreach (var x in directories)
            //        {
            //            DMSDirectory directory = new DMSDirectory();

            //            oldDirId = x.DirectoryId;

            //            directory.DirectoryName = x.DirectoryName;
            //            directory.DirectoryParentId = x.DirectoryParentId;
            //            directory.AuthorId = context.GetTargetUserValue(x.AuthorId);
            //            directory.CreatedByUser = context.GetTargetUserValue(x.CreatedByUser);
            //            directory.CreatedOn = x.CreatedOn;
            //            directory.UpdatedBy = context.GetTargetUserValue(x.UpdatedBy);
            //            directory.UpdatedOn = x.UpdatedOn;
            //            directory.TenantID = context.AppContext.TenantID;
            //            directory.Deleted = x.Deleted;

            //            ctx.DMSDirectories.Add(directory);
            //            ctx.SaveChanges();

            //            targetMappedList.Add(new MappedItem(Convert.ToString(oldDirId), Convert.ToString(directory.DirectoryId)));
            //        }

            //        directories.Clear();
            //        directories = ctx.DMSDirectories.Where(x => x.TenantID.EqualsIgnoreCase(context.AppContext.TenantID)).ToList();
            //        if (directories.Count > 0)
            //        {
            //            directories.ForEach(x => {
            //                if (x.DirectoryParentId != 0)
            //                    x.DirectoryParentId = Convert.ToInt32(context.GetTargetLookupValueOptionLong(Convert.ToString(x.DirectoryParentId), "DirectoryParentId", DatabaseObjects.Tables.DMSDirectory));
            //            });

            //            ctx.DMSDirectories.UpdateRange(directories);
            //            ctx.SaveChanges();
            //        }
            //    }

            //}
        }

        private void CopyDocuments()
        {
            List<DMSDocument> documents = new List<DMSDocument>();

            //using (DatabaseContext ctx = new DatabaseContext(context.SourceAppContext))
            //{
            //    documents = ctx.DMSDocument.Where(x => x.TenantID.EqualsIgnoreCase(context.SourceAppContext.TenantID)).ToList();
            //    if (documents.Count > 0)
            //    {
            //        foreach (var x in documents)
            //        {
            //            DMSDocument doc = new DMSDocument();
            //            doc.FileName = x.FileName;
            //            doc.FullPath = x.FullPath.Replace(context.SourceAppContext.TenantAccountId, context.AppContext.TenantAccountId);
            //            doc.TenantID = context.AppContext.TenantID;
            //            doc.AuthorId = context.GetTargetUserValue(x.AuthorId);
            //            doc.CreatedByUser = context.GetTargetUserValue(x.CreatedByUser);
            //            doc.CreatedOn = x.CreatedOn;
            //            doc.UpdatedOn = x.UpdatedOn;
            //            doc.UpdatedBy = context.GetTargetUserValue(x.UpdatedBy);
            //            doc.CheckOutBy = context.GetTargetUserValue(x.CheckOutBy);
            //            doc.DirectoryId = Convert.ToInt32(context.GetTargetLookupValueOptionLong(Convert.ToString(x.DirectoryId), "DirectoryId", DatabaseObjects.Tables.DMSDirectory));

            //            doc.CustomerId = x.CustomerId;

            //            doc.CustomerId = x.CustomerId;
            //            doc.FileParentId = x.FileParentId;
            //            doc.StoredFileName = x.StoredFileName;
            //            doc.DocumentControlID = x.DocumentControlID;
            //            doc.ReviewStep = x.ReviewStep;
            //            doc.Title = x.Title;
            //            doc.Size = x.Size;
            //            doc.Version = x.Version;
            //            doc.MainVersionFileId = x.MainVersionFileId;
            //            doc.Deleted = x.Deleted;


            //            ctx.DMSDocument.Add(doc);
            //            ctx.SaveChanges();
            //        }
            //    }
            //}

            //try
            //{
            //    string baseFolderPath = StorageUtil.GetDMSBaseFolderPath();

            //    if (string.IsNullOrEmpty(baseFolderPath))
            //        return;

            //    string sourcePath = Path.Combine(baseFolderPath, "ALL", context.SourceAppContext.TenantAccountId);
            //    string targetPath = Path.Combine(baseFolderPath, "ALL", context.AppContext.TenantAccountId);

            //    if (Directory.Exists(sourcePath))
            //    {
            //        Utility.UGITUtility.DirectoryCopy(sourcePath, targetPath, true);
            //    }
            //}
            //catch (Exception ex)
            //{
            //}

        }
        public override void ResourceWorkItems()
        {
            base.ResourceWorkItems();
            bool import = true;
            //bool import = context.IsImportEnable("ResourceWorkItems");
            // above importenable commented because its onetime entry and can't provide access on front end.
            if (!import)
                return;

            ULog.WriteLog($"Updating ResourceWorkItems");
            {
                ResourceWorkItemsManager mgr = new ResourceWorkItemsManager(context.AppContext);
                //List<ResourceWorkItems> dbData = mgr.Load(x => x.WorkItemType.Contains(moduleName) && x.WorkItem.StartsWith(moduleName));
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
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ResourceWorkItems;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        string username = string.Empty;
                        foreach (ClientOM.ListItem item in collection)
                        {
                            ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Resource] as ClientOM.FieldUserValue;
                            if (userlist != null && users != null)
                            {
                                username = userlist.GetTargetID(Convert.ToString(users.LookupId));
                            }
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]) && x.Resource == username && x.WorkItem == Convert.ToString(item[SPDatabaseObjects.Columns.WorkItem]) && x.WorkItemType == Convert.ToString(item[SPDatabaseObjects.Columns.WorkItemType]));
                            if (targetItem != null)
                                targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new ResourceWorkItems();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.SubWorkItem = Convert.ToString(item[SPDatabaseObjects.Columns.SubWorkItem]);
                                targetItem.WorkItem = Convert.ToString(item[SPDatabaseObjects.Columns.WorkItem]);
                                targetItem.WorkItemType = Convert.ToString(item[SPDatabaseObjects.Columns.WorkItemType]);
                                targetItem.Resource = username;

                                ClientOM.FieldUserValue _users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (_users != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(_users.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                            if (targetItem != null)
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                    } while (position != null);

                }
                ULog.WriteLog($"{targetNewItemCount} ResourceWorkItems added");
            }
        }
        public override void TicketHours()
        {
            base.TicketHours();
            bool import = true;
            //bool import = context.IsImportEnable("TicketHours");
            // above importenable commented because its onetime entry and can't provide access on front end.

            if (!import)
                return;

            ULog.WriteLog($"Updating TicketHours ");
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

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.TicketHours;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);

                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.TicketID == Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]));
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new ActualHour();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.TicketID = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                                targetItem.ModuleNameLookup = Convert.ToString(item[SPDatabaseObjects.Columns.ModuleName]);
                                targetItem.HoursTaken = Convert.ToInt32(item[SPDatabaseObjects.Columns.HoursTaken]);
                                if (item[SPDatabaseObjects.Columns.MonthStartDate] != null)
                                    targetItem.MonthStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.MonthStartDate]);
                                if (item[SPDatabaseObjects.Columns.WorkDate] != null)
                                    targetItem.WorkDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.WorkDate]);
                                if (item[SPDatabaseObjects.Columns.WeekStartDate] != null)
                                    targetItem.WeekStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.WeekStartDate]);
                                targetItem.StageStep = Convert.ToInt32(item[SPDatabaseObjects.Columns.StageStep]);
                                targetItem.StandardWorkItem = Convert.ToBoolean(item[SPDatabaseObjects.Columns.StandardWorkItem]);
                                targetItem.WorkItem = Convert.ToString(item[SPDatabaseObjects.Columns.WorkItem]);
                                targetItem.TaskID = Convert.ToInt32(item[SPDatabaseObjects.Columns.TaskID]);
                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Resource] as ClientOM.FieldUserValue;
                                if (userlist != null && users != null)
                                    targetItem.Resource = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                targetItem.Comment = Convert.ToString(item[SPDatabaseObjects.Columns.TicketComment]);
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                ClientOM.FieldUserValue user = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (user != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(user.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

                }

                ULog.WriteLog($"{targetNewItemCount} TicketHours added");
            }
        }
        public override void ResourceTimeSheet()
        {
            try
            {
                base.ResourceTimeSheet();
                bool import = true;
                //bool import = context.IsImportEnable("ResourceTimeSheet");
                // above importenable commented because its onetime entry and can't provide access on front end.
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

                    using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                    {
                        string listName = SPDatabaseObjects.Lists.ResourceTimeSheet;
                        ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                        MappedItemList locaionlist = context.GetMappedList(listName);
                        MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                        MappedItemList resourcelist = context.GetMappedList(SPDatabaseObjects.Lists.ResourceWorkItems);

                        ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                        string title = string.Empty;
                        do
                        {
                            ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                            position = collection.ListItemCollectionPosition;

                            foreach (ClientOM.ListItem item in collection)
                            {
                                targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                                targetItem = null;
                                if (targetItem == null)
                                {
                                    targetItem = new ResourceTimeSheet();
                                    dbData.Add(targetItem);
                                    targetNewItemCount++;
                                }
                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Resource] as ClientOM.FieldUserValue;
                                    if (userlist != null && users != null)
                                    {
                                        targetItem.Resource = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                    }
                                    long itemid = 0;
                                    ClientOM.FieldLookupValue resourceid = item[SPDatabaseObjects.Columns.ResourceWorkItemLookup] as ClientOM.FieldLookupValue;
                                    if (resourcelist != null)
                                        itemid = UGITUtility.StringToLong(resourcelist.GetTargetID(resourceid.LookupId.ToString()));
                                    if (itemid > 0)
                                        targetItem.ResourceWorkItemLookup = itemid;
                                    targetItem.HoursTaken = Convert.ToInt32(item[SPDatabaseObjects.Columns.HoursTaken]);
                                    if (item[SPDatabaseObjects.Columns.WorkDate] != null)
                                        targetItem.WorkDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.WorkDate]);
                                    targetItem.WorkDescription = Convert.ToString(item[SPDatabaseObjects.Columns.WorkDescription]);
                                    ClientOM.FieldUserValue user = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                    if (user != null && userlist != null)
                                        targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(user.LookupId));
                                    ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                    if (Modifeduser != null && userlist != null)
                                        targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                    targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                    targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);

                                }

                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);

                            }
                        } while (position != null);

                    }

                    ULog.WriteLog($"{targetNewItemCount} ResourceTimeSheet added");
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
        }
        public override void ResourceAllocation()
        {
            base.ResourceAllocation();
            bool import = true;
            //bool import = context.IsImportEnable("ResourceAllocation");
            // above importenable commented because its onetime entry and can't provide access on front end.

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
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ResourceAllocation;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList resourcelist = context.GetMappedList(SPDatabaseObjects.Lists.ResourceWorkItems);
                    MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty; string username = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Resource] as ClientOM.FieldUserValue;
                            if (userlist != null && users != null)
                            {
                                username = userlist.GetTargetID(Convert.ToString(users.LookupId));
                            }
                            long itemid = 0;
                            ClientOM.FieldLookupValue resourceid = item[SPDatabaseObjects.Columns.ResourceWorkItemLookup] as ClientOM.FieldLookupValue;
                            if (resourcelist != null)
                                itemid = UGITUtility.StringToLong(resourcelist.GetTargetID(resourceid.LookupId.ToString()));

                            targetItem = dbData.FirstOrDefault(x => x.Resource == username && x.ResourceWorkItemLookup == itemid);
                            if (targetItem != null)
                                targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new RResourceAllocation();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.PctAllocation = Convert.ToInt32(item[SPDatabaseObjects.Columns.PctAllocation]);

                                if (item[SPDatabaseObjects.Columns.AllocationStartDate] != null)
                                    targetItem.AllocationStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AllocationStartDate]);

                                if (item[SPDatabaseObjects.Columns.AllocationEndDate] != null)
                                    targetItem.AllocationEndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AllocationEndDate]);

                                if (item[SPDatabaseObjects.Columns.EstimatedEndDate] != null)
                                    targetItem.EstEndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.EstimatedEndDate]);

                                if (item[SPDatabaseObjects.Columns.EstimatedStartDate] != null)
                                    targetItem.EstStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.EstimatedStartDate]);

                                targetItem.PctPlannedAllocation = Convert.ToInt32(item[SPDatabaseObjects.Columns.PctPlannedAllocation]);

                                if (item[SPDatabaseObjects.Columns.PlannedStartDate] != null)
                                    targetItem.PlannedStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.PlannedStartDate]);
                                if (item[SPDatabaseObjects.Columns.PlannedEndDate] != null)
                                    targetItem.PlannedEndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.PlannedEndDate]);

                                targetItem.PctEstimatedAllocation = Convert.ToInt32(item[SPDatabaseObjects.Columns.PctEstimatedAllocation]);
                                targetItem.Deleted = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                                targetItem.Resource = username;
                                targetItem.ResourceWorkItemLookup = itemid;
                                ClientOM.FieldUserValue user = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (user != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(user.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

                }
                ULog.WriteLog($"{targetNewItemCount} ResourceAllocation added");
            }
        }
        public override void ResourceUsageSummaryMonthWise()
        {
            base.ResourceUsageSummaryMonthWise();
            bool import = true;
            //bool import = context.IsImportEnable("ResourceUsageSummaryMonthWise");
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
                List<FunctionalArea> functionalAreaslst = new List<FunctionalArea>();
                FunctionalAreasManager objfuncmgr = new FunctionalAreasManager(context.AppContext);
                functionalAreaslst = objfuncmgr.Load();
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ResourceUsageSummaryMonthWise;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        string username = string.Empty;
                        foreach (ClientOM.ListItem item in collection)
                        {
                            ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Resource] as ClientOM.FieldUserValue;
                            if (userlist != null && users != null)
                            {
                                username = userlist.GetTargetID(Convert.ToString(users.LookupId));
                            }
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]) && x.Resource == username && x.WorkItem == Convert.ToString(item[SPDatabaseObjects.Columns.WorkItem]) && x.WorkItemType == Convert.ToString(item[SPDatabaseObjects.Columns.WorkItemType]));
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new ResourceUsageSummaryMonthWise();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ActualHour = Convert.ToInt32(item[SPDatabaseObjects.Columns.ActualHour]);
                                targetItem.PctActual = Convert.ToDouble(item[SPDatabaseObjects.Columns.PctActual]);
                                targetItem.PctAllocation = Convert.ToDouble(item[SPDatabaseObjects.Columns.PctAllocation]);
                                if (item[SPDatabaseObjects.Columns.MonthStartDate] != null)
                                    targetItem.MonthStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.MonthStartDate]);
                                targetItem.PctPlannedAllocation = Convert.ToDouble(item[SPDatabaseObjects.Columns.PctPlannedAllocation]);
                                targetItem.AllocationHour = Convert.ToDouble(item[SPDatabaseObjects.Columns.AllocationHour]);
                                if (item[SPDatabaseObjects.Columns.FunctionalAreaTitle] != null)
                                {
                                    targetItem.FunctionalAreaTitleLookup = Convert.ToString(functionalAreaslst.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.FunctionalAreaTitle])).ID);
                                }
                                //Convert.ToString(item[SPDatabaseObjects.Columns.FunctionalAreaTitle]);
                                targetItem.SubWorkItem = Convert.ToString(item[SPDatabaseObjects.Columns.SubWorkItem]);
                                targetItem.WorkItemType = Convert.ToString(item[SPDatabaseObjects.Columns.WorkItemType]);
                                targetItem.WorkItem = Convert.ToString(item[SPDatabaseObjects.Columns.WorkItem]);
                                targetItem.WorkItemID = Convert.ToInt64(item[SPDatabaseObjects.Columns.WorkItemID]);
                                targetItem.IsConsultant = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsConsultant]);
                                targetItem.IsIT = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsIT]);
                                targetItem.IsManager = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsManager]);
                                targetItem.Resource = username;

                                ClientOM.FieldUserValue _users = item[SPDatabaseObjects.Columns.ManagerLookup] as ClientOM.FieldUserValue;
                                if (userlist != null && _users != null)
                                {
                                    targetItem.ManagerLookup = userlist.GetTargetID(Convert.ToString(_users.LookupId));
                                }
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

                }

                ULog.WriteLog($"{targetNewItemCount} ResourceUsageSummaryMonthWise added");
            }
        }
        public override void ResourceUsageSummaryWeekWise()
        {
            base.ResourceUsageSummaryWeekWise();
            bool import = true;
            //bool import = context.IsImportEnable("ResourceUsageSummaryWeekWise");
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
                List<FunctionalArea> functionalAreaslst = new List<FunctionalArea>();
                FunctionalAreasManager objfuncmgr = new FunctionalAreasManager(context.AppContext);
                functionalAreaslst = objfuncmgr.Load();

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ResourceUsageSummaryWeekWise;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);

                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        string username = string.Empty;
                        foreach (ClientOM.ListItem item in collection)
                        {
                            ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Resource] as ClientOM.FieldUserValue;
                            if (userlist != null && users != null)
                            {
                                username = userlist.GetTargetID(Convert.ToString(users.LookupId));
                            }
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]) && x.Resource == username && x.WorkItem == Convert.ToString(item[SPDatabaseObjects.Columns.WorkItem]) && x.WorkItemType == Convert.ToString(item[SPDatabaseObjects.Columns.WorkItemType]));
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new ResourceUsageSummaryWeekWise();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ActualHour = Convert.ToInt32(item[SPDatabaseObjects.Columns.ActualHour]);
                                targetItem.PctActual = Convert.ToDouble(item[SPDatabaseObjects.Columns.PctActual]);
                                targetItem.PctAllocation = Convert.ToDouble(item[SPDatabaseObjects.Columns.PctAllocation]);
                                if (item[SPDatabaseObjects.Columns.WeekStartDate] != null)
                                    targetItem.WeekStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.WeekStartDate]);
                                targetItem.PctPlannedAllocation = Convert.ToDouble(item[SPDatabaseObjects.Columns.PctPlannedAllocation]);
                                targetItem.AllocationHour = Convert.ToDouble(item[SPDatabaseObjects.Columns.AllocationHour]);
                                if (item[SPDatabaseObjects.Columns.FunctionalAreaTitle] != null)
                                {
                                    targetItem.FunctionalAreaTitleLookup = Convert.ToString(functionalAreaslst.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.FunctionalAreaTitle])).ID);
                                }
                                //targetItem.FunctionalAreaTitleLookup = Convert.ToString(item[SPDatabaseObjects.Columns.FunctionalAreaTitle]);
                                targetItem.SubWorkItem = Convert.ToString(item[SPDatabaseObjects.Columns.SubWorkItem]);
                                targetItem.WorkItemType = Convert.ToString(item[SPDatabaseObjects.Columns.WorkItemType]);
                                targetItem.WorkItem = Convert.ToString(item[SPDatabaseObjects.Columns.WorkItem]);
                                targetItem.WorkItemID = Convert.ToInt64(item[SPDatabaseObjects.Columns.WorkItemID]);
                                targetItem.IsConsultant = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsConsultant]);
                                targetItem.IsIT = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsIT]);
                                targetItem.IsManager = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsManager]);
                                targetItem.Resource = username;
                                ClientOM.FieldUserValue _users = item[SPDatabaseObjects.Columns.ManagerLookup] as ClientOM.FieldUserValue;
                                if (userlist != null && _users != null)
                                    targetItem.ManagerLookup = userlist.GetTargetID(Convert.ToString(_users.LookupId));

                                ClientOM.FieldUserValue user = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (user != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(user.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

                }
                ULog.WriteLog($"{targetNewItemCount} ResourceUsageSummaryWeekWise added");
            }
        }
        public override void ResourceAllocationMonthly()
        {
            try
            {
                base.ResourceAllocationMonthly();
                bool import = true;
                //bool import = context.IsImportEnable("ResourceAllocationMonthly");
                if (!import)
                    return;

                ULog.WriteLog($"Updating ResourceAllocationMonthly");
                {
                    int targetNewItemCount = 0;
                    ResourceAllocationMonthly targetItem = null;
                    ResourceAllocationMonthlyManager mgr = new ResourceAllocationMonthlyManager(context.AppContext);
                    List<ResourceAllocationMonthly> dbData = mgr.Load();
                    mgr.Delete(dbData);

                    using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                    {
                        string listName = SPDatabaseObjects.Lists.ResourceAllocationMonthly;
                        ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                        MappedItemList locaionlist = context.GetMappedList(listName);
                        MappedItemList resourcelist = context.GetMappedList(SPDatabaseObjects.Lists.ResourceWorkItems);
                        ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                        MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                        string title = string.Empty;
                        string username = string.Empty;
                        do
                        {
                            ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                            position = collection.ListItemCollectionPosition;

                            foreach (ClientOM.ListItem item in collection)
                            {
                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Resource] as ClientOM.FieldUserValue;
                                if (userlist != null && users != null)
                                    username = userlist.GetTargetID(Convert.ToString(users.LookupId));

                                ClientOM.FieldLookupValue resourceid = item[SPDatabaseObjects.Columns.ResourceWorkItemLookup] as ClientOM.FieldLookupValue;
                                long wid = 0;
                                if (resourcelist != null)
                                    wid = UGITUtility.StringToLong(resourcelist.GetTargetID(resourceid.LookupId.ToString()));

                                targetItem = dbData.FirstOrDefault(x => x.Resource == username && x.MonthStartDate == Convert.ToDateTime(item[SPDatabaseObjects.Columns.MonthStartDate]) && x.ResourceWorkItemLookup == wid);
                                if (targetItem != null)
                                    targetItem = null;
                                if (targetItem == null)
                                {
                                    targetItem = new ResourceAllocationMonthly();
                                    dbData.Add(targetItem);
                                    targetNewItemCount++;
                                }
                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    targetItem.PctAllocation = Convert.ToInt32(item[SPDatabaseObjects.Columns.PctAllocation]);

                                    targetItem.Resource = username;
                                    targetItem.ResourceWorkItemLookup = wid;
                                    string dt = Convert.ToDateTime(Convert.ToString(item[SPDatabaseObjects.Columns.MonthStartDate])).ToString("yyyy-MM-dd 00:00:00");
                                    targetItem.MonthStartDate = Convert.ToDateTime(dt);
                                    //Convert.ToDateTime(item[SPDatabaseObjects.Columns.MonthStartDate]).ToString("dd/MM/yyyy HH:mm:ss tt");
                                    targetItem.PctPlannedAllocation = Convert.ToInt32(item[SPDatabaseObjects.Columns.PctPlannedAllocation]);
                                    targetItem.ResourceSubWorkItem = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ResourceSubWorkItem]).LookupValue == null ? string.Empty : Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ResourceSubWorkItem]).LookupValue);
                                    targetItem.ResourceWorkItem = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ResourceWorkItem]).LookupValue == null ? string.Empty : Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ResourceWorkItem]).LookupValue);
                                    targetItem.ResourceWorkItemType = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ResourceWorkItemType]).LookupValue == null ? string.Empty : Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ResourceWorkItemType]).LookupValue);

                                    ClientOM.FieldUserValue user = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                    if (user != null && userlist != null)
                                        targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(user.LookupId));
                                    ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                    if (Modifeduser != null && userlist != null)
                                        targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                    targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                    targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                                }

                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);

                            }
                        } while (position != null);

                    }

                    ULog.WriteLog($"{targetNewItemCount} ResourceAllocationMonthly added");
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

        }

        public override void ProjectEstimatedAllocation()
        {
            // Need to discuss
        }

        public override void ProjectPlannedAllocation()
        {
            try
            {
                base.ProjectPlannedAllocation();
                bool import = true;
                //bool import = context.IsImportEnable("ProjectPlannedAllocation");
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
                    using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                    {
                        string listName = SPDatabaseObjects.Lists.ProjectAllocations;
                        ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                        MappedItemList locaionlist = context.GetMappedList(listName);
                        MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                        ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                        string title = string.Empty;
                        do
                        {
                            ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                            position = collection.ListItemCollectionPosition;
                            string username = string.Empty;
                            foreach (ClientOM.ListItem item in collection)
                            {
                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Resource] as ClientOM.FieldUserValue;
                                if (userlist != null && users != null)
                                {
                                    username = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                }
                                //if (moduleName == uHelper.getModuleNameByTicketId(Convert.ToString(item[SPDatabaseObjects.Columns.TicketId])))
                                //{
                                targetItem = dbData.FirstOrDefault(x => x.TicketID == Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]) && x.Resource == username && x.PctAllocation == Convert.ToDouble(item[SPDatabaseObjects.Columns.PctAllocation]) && x.AllocationStartDate == Convert.ToDateTime(item[SPDatabaseObjects.Columns.AllocationStartDate]));
                                //}
                                //else
                                //{
                                //    continue;
                                //}
                                if (targetItem != null)
                                    targetItem = null;
                                //targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]) && x.Resource == username && x.WorkItem == Convert.ToString(item[SPDatabaseObjects.Columns.WorkItem]) && x.WorkItemType == Convert.ToString(item[SPDatabaseObjects.Columns.WorkItemType]));
                                if (targetItem == null)
                                {
                                    targetItem = new ProjectAllocation();
                                    dbData.Add(targetItem);
                                    targetNewItemCount++;
                                }
                                try
                                {
                                    if (targetItem.ID == 0 || importWithUpdate)
                                    {
                                        targetItem.TicketID = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                                        targetItem.Resource = username;
                                        targetItem.PctAllocation = Convert.ToInt32(item[SPDatabaseObjects.Columns.PctAllocation]);
                                        if (item[SPDatabaseObjects.Columns.AllocationStartDate] != null)
                                            targetItem.AllocationStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AllocationStartDate]);
                                        if (item[SPDatabaseObjects.Columns.AllocationEndDate] != null)
                                            targetItem.AllocationEndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AllocationEndDate]);
                                    }

                                    if (targetItem.ID > 0)
                                        mgr.Update(targetItem);
                                    else
                                        mgr.Insert(targetItem);
                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex);
                                }

                            }
                        } while (position != null);
                    }

                    ULog.WriteLog($"{targetNewItemCount} ProjectPlannedAllocation added");
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

        }

        public override void UpdateMyModuleColumns()
        {
            base.UpdateMyModuleColumns();
            ULog.WriteLog($"Update MyModuleColumns");
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                ModuleColumnManager mgr = new ModuleColumnManager(context.AppContext);
                List<ModuleColumn> dbData = mgr.Load();
                string listName = SPDatabaseObjects.Lists.MyModuleColumns;
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;

                string fieldName = string.Empty;
                ModuleColumn targetItem = null;
                string newfieldname = string.Empty;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    try
                    {
                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new ModuleColumn();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.CategoryName = Convert.ToString(item[SPDatabaseObjects.Columns.CategoryName]);
                                targetItem.FieldSequence = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.FieldSequence]);
                                targetItem.FieldName = Convert.ToString(item[SPDatabaseObjects.Columns.FieldName]);
                                targetItem.FieldDisplayName = Convert.ToString(item[SPDatabaseObjects.Columns.FieldDisplayName]);
                                targetItem.SortOrder = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.SortOrder]);
                                targetItem.IsDisplay = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsDisplay]);
                                targetItem.ShowInMobile = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ShowInMobile]);
                                targetItem.DisplayForClosed = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.DisplayForClosed]);
                                targetItem.IsUseInWildCard = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsUseInWildCard]);
                                targetItem.DisplayForReport = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.DisplayForReport]);
                                targetItem.CustomProperties = Convert.ToString(item[SPDatabaseObjects.Columns.CustomProperties]);
                                targetItem.ColumnType = Convert.ToString(item[SPDatabaseObjects.Columns.ColumnType]);
                                targetItem.IsAscending = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsAscending]);
                                targetItem.TextAlignment = Convert.ToString(item[SPDatabaseObjects.Columns.TextAlignment]);
                                targetItem.TruncateTextTo = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.TruncateTextTo]);
                                //targetItem.SelectedTabs = Convert.ToString(item[SPDatabaseObjects.Columns.SelectedTabs]);
                                targetItem.ShowInCardView = false;
                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex);
                    }

                } while (position != null);

                ULog.WriteLog($"{targetNewItemCount} MyModuleColumns added");
            }
        }
        public override void UpdateTasktemplates()
        {
            base.UpdateTasktemplates();
            ULog.WriteLog($"Update Tasktemplates");
            int targetNewItemCount = 0;
            MappedItemList targetProjectlifecycleList = context.GetMappedList(SPDatabaseObjects.Lists.ProjectLifeCycles);
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                TaskTemplate templateItem = null;
                string listName = SPDatabaseObjects.Lists.UGITTaskTemplates;
                TaskTemplateManager taskTemplate = new TaskTemplateManager(context.AppContext);
                List<TaskTemplate> Data = taskTemplate.Load();
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList taskTemplatelist = context.GetMappedList(listName);
                MappedItemList _userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                string title = string.Empty; string ticketid = string.Empty;
                do
                {
                    ClientOM.ListItemCollection collection = null;
                    collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        templateItem = null;
                        if (templateItem == null)
                        {
                            templateItem = new TaskTemplate();
                            Data.Add(templateItem);
                            targetNewItemCount++;
                        }
                        try
                        {
                            if (templateItem.ID == 0)
                            {
                                templateItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                templateItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                                ClientOM.FieldLookupValue projectlifecycles = item[SPDatabaseObjects.Columns.ProjectLifeCycleLookup] as ClientOM.FieldLookupValue;
                                if (projectlifecycles != null && targetProjectlifecycleList != null)
                                    templateItem.ProjectLifeCycleLookup = UGITUtility.StringToLong(targetProjectlifecycleList.GetTargetID(Convert.ToString(projectlifecycles.LookupId)));

                                ClientOM.FieldUserValue Createduser = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (Createduser != null && _userList != null)
                                    templateItem.CreatedBy = _userList.GetTargetID(Convert.ToString(Createduser.LookupId));

                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && _userList != null)
                                    templateItem.ModifiedBy = _userList.GetTargetID(Convert.ToString(Modifeduser.LookupId));

                                templateItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                templateItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                            }
                            if (templateItem.ID > 0)
                                taskTemplate.Update(templateItem);
                            else
                                taskTemplate.Insert(templateItem);

                            if (templateItem != null)
                                taskTemplatelist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), templateItem.ID.ToString()));
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                        }
                    }
                } while (position != null);

                ULog.WriteLog($"{targetNewItemCount} TaskTemplates");
            }
        }
        public override void UpdateTaskTemplateItems()
        {
            DataTable dtRel = new DataTable();
            dtRel.Columns.Add("SPId");
            dtRel.Columns.Add("DId");
            dtRel.Columns.Add("TenantId");
            dtRel.Columns.Add("TicketId");
            dtRel.Columns.Add("ModuleId");

            base.UpdateTaskTemplateItems();
            ULog.WriteLog($"Update TaskTemplateItems");
            int targetNewItemCount = 0;
            UGITTask targetItem = null;
            MappedItemList tasktemplatelst = context.GetMappedList(SPDatabaseObjects.Lists.UGITTaskTemplates);
            MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
            UGITTaskManager mgr = new UGITTaskManager(context.AppContext);
            List<UGITTask> dbData = mgr.Load(x => x.ModuleNameLookup == "Template");
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.TaskTemplateItems;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                try
                {
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new UGITTask();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            if (targetItem.ID == 0)
                            {

                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.PercentComplete = item[SPDatabaseObjects.Columns.PercentComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.PercentComplete]);
                                targetItem.ActualHours = item[SPDatabaseObjects.Columns.TaskActualHours] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.TaskActualHours]);
                                targetItem.AssignToPct = UGITUtility.ObjectToString(item[SPDatabaseObjects.Columns.UGITAssignToPct]);
                                ClientOM.FieldUserValue[] AssignedTo = item[SPDatabaseObjects.Columns.AssignedTo] as ClientOM.FieldUserValue[];
                                if (AssignedTo != null && AssignedTo.Length > 0 && userlist != null)
                                    targetItem.AssignedTo = userlist.GetTargetIDs(AssignedTo.Select(x => x.LookupId.ToString()).ToList());
                                targetItem.ChildCount = Convert.ToInt32(item[SPDatabaseObjects.Columns.UGITChildCount]);
                                targetItem.Comment = Convert.ToString(item[SPDatabaseObjects.Columns.UGITComment]);
                                targetItem.body = Convert.ToString(item[SPDatabaseObjects.Columns.Body]);
                                targetItem.Contribution = UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.UGITContribution]);
                                if(item[SPDatabaseObjects.Columns.DueDate]!=null)
                                    targetItem.DueDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.DueDate]);
                                targetItem.Duration = item[SPDatabaseObjects.Columns.UGITDuration] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.UGITDuration]);
                                targetItem.EstimatedHours = item[SPDatabaseObjects.Columns.TaskEstimatedHours] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.TaskEstimatedHours]);
                                targetItem.IsMileStone = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsMilestone]);
                                targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                                targetItem.Level = Convert.ToInt32(item[SPDatabaseObjects.Columns.UGITLevel]);
                                targetItem.ParentTaskID = Convert.ToInt32(item[SPDatabaseObjects.Columns.ParentTask]);
                                ClientOM.FieldLookupValue[] Predecessors_ = item[SPDatabaseObjects.Columns.Predecessors] as ClientOM.FieldLookupValue[];
                                List<string> pressors = new List<string>();
                                foreach (ClientOM.FieldLookupValue pred in Predecessors_)
                                {
                                    pressors.Add(Convert.ToString(pred.LookupValue));
                                }
                                targetItem.Predecessors = string.Join(";#", pressors.ToArray());
                                targetItem.Priority = Convert.ToString(item[SPDatabaseObjects.Columns.TaskPriority]);
                                targetItem.StageStep = Convert.ToInt32(item[SPDatabaseObjects.Columns.StageStep]);
                                if(item[SPDatabaseObjects.Columns.StartDate]!=null)
                                targetItem.StartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.StartDate]);
                                targetItem.Status = Convert.ToString(item[SPDatabaseObjects.Columns.Status]);
                                targetItem.Behaviour = Convert.ToString(item[SPDatabaseObjects.Columns.TaskBehaviour]);
                                //targetItem. = Convert.ToString(item[SPDatabaseObjects.Columns.TaskGroup]);

                                ClientOM.FieldLookupValue _TaskTemplateLookup = item[SPDatabaseObjects.Columns.TaskTemplateLookup] as ClientOM.FieldLookupValue;
                                if (_TaskTemplateLookup != null && tasktemplatelst != null)
                                    targetItem.TicketId = tasktemplatelst.GetTargetID(Convert.ToString(_TaskTemplateLookup.LookupId));
                                targetItem.ModuleNameLookup = "Template";
                                targetItem.TenantID = context.AppContext.TenantID;
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                                targetItem.CreatedBy = context.Tenant.CreatedByUser;
                                targetItem.ModifiedBy = context.Tenant.ModifiedByUser;

                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);

                                // adding below process to make parent child relationship without predecessors
                                DataRow dr = dtRel.NewRow();
                                dr["SPId"] = item[SPDatabaseObjects.Columns.Id];
                                dr["DId"] = targetItem.ID;
                                dr["TenantId"] = context.AppContext.TenantID;
                                dr["TicketId"] = targetItem.TicketId;
                                dr["ModuleId"] = "Template";
                                dtRel.Rows.Add(dr);
                            }
                        }

                        GetTableDataManager.bulkupload(dtRel, GetTableDataManager.CreateTempTable());
                        Dictionary<string, object> values = new Dictionary<string, object>();
                        values.Add("@TenantID", context.AppContext.TenantID);
                        values.Add("@ModuleId", "Template");
                        DAL.uGITDAL.ExecuteDataSetWithParameters("usp_updateparenttask", values);
                        ULog.WriteLog($"{targetNewItemCount} Update parent task id of TaskTemplateItems");

                    } while (position != null);
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex.ToString());
                }

                ULog.WriteLog($"{targetNewItemCount} TaskTemplateItems added");

                List<UGITTask> tasklist = mgr.Load(x => x.ModuleNameLookup.StartsWith("Template"));
                tasklist.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x.Predecessors) && x.Predecessors.Contains(";#"))
                    {
                        List<string> collection = new List<string>();
                        List<string> pressors = new List<string>();
                        collection = UGITUtility.SplitString(x.Predecessors, Constants.Separator).ToList();
                        foreach (var item in collection)
                        {
                            pressors.Add(UGITUtility.ObjectToString(tasklist.Where(y => y.Title == item.Trim() && y.TicketId == x.TicketId).FirstOrDefault().ID));
                        }
                        x.Predecessors = string.Join(",", pressors.ToArray());
                    }
                    else if (!string.IsNullOrEmpty(x.Predecessors))
                        x.Predecessors = UGITUtility.ObjectToString(tasklist.Where(y => y.Title == x.Predecessors && y.TicketId == x.TicketId).FirstOrDefault().ID);
                });
                mgr.UpdateItems(tasklist);
                ULog.WriteLog($"{tasklist.Count} Predecessors updated of TaskTemplateItems");
            }
        }
        public override void UpdateRelatedTickets()
        {
            base.UpdateRelatedTickets();

            ULog.WriteLog($"Updating TicketRelation");
            {
                TicketRelationManager mgr = new TicketRelationManager(context.AppContext);
                List<TicketRelation> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<TicketRelation>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }
                int targetNewItemCount = 0;
                TicketRelation targetItem = null;
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.TicketRelationship;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        try
                        {
                            foreach (ClientOM.ListItem item in collection)
                            {
                                targetItem = null;
                                if (targetItem == null)
                                {
                                    targetItem = new TicketRelation();
                                    dbData.Add(targetItem);
                                    targetNewItemCount++;
                                }
                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    targetItem.ParentModuleName = uHelper.getModuleNameByTicketId(Convert.ToString(item[SPDatabaseObjects.Columns.ParentTicketId]));
                                    targetItem.ParentTicketID = Convert.ToString(item[SPDatabaseObjects.Columns.ParentTicketId]);
                                    targetItem.ChildTicketID = Convert.ToString(item[SPDatabaseObjects.Columns.ChildTicketId]);
                                    targetItem.ChildModuleName = uHelper.getModuleNameByTicketId(Convert.ToString(item[SPDatabaseObjects.Columns.ChildTicketId]));
                                }
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);

                        }

                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} TicketRelation added");
                }
            }
            ULog.WriteLog($"Updating AssetsRelation");
            {
                TicketRelationManager mgr = new TicketRelationManager(context.AppContext);
                List<TicketRelation> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<TicketRelation>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }
                int targetNewItemCount = 0;
                TicketRelation targetItem = null;
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.AssetRelations;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        try
                        {
                            foreach (ClientOM.ListItem item in collection)
                            {
                                targetItem = null;
                                if (targetItem == null)
                                {
                                    targetItem = new TicketRelation();
                                    dbData.Add(targetItem);
                                    targetNewItemCount++;
                                }
                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    targetItem.ParentModuleName = uHelper.getModuleNameByTicketId(Convert.ToString(item[SPDatabaseObjects.Columns.ParentTicketId]));
                                    targetItem.ParentTicketID = Convert.ToString(item[SPDatabaseObjects.Columns.ParentTicketId]);
                                    targetItem.ChildTicketID = Convert.ToString(item[SPDatabaseObjects.Columns.ChildTicketId]);
                                    targetItem.ChildModuleName = uHelper.getModuleNameByTicketId(Convert.ToString(item[SPDatabaseObjects.Columns.ChildTicketId]));
                                }
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);

                        }

                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} Assets Relation added");
                }
            }
        }

        public override void ProjectSimilarityConfig()
        {
            base.ProjectSimilarityConfig();
            ULog.WriteLog($"Update ProjectSimilarityConfig");
            int targetNewItemCount = 0;
            ProjectSimilarityConfig targetItem = null;
            MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
            ProjectSimilarityConfigManager mgr = new ProjectSimilarityConfigManager(context.AppContext);
            List<ProjectSimilarityConfig> dbData = mgr.Load();
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.ProjectSimilarityConfig;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                try
                {
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new ProjectSimilarityConfig();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ColumnName = Convert.ToString(item[SPDatabaseObjects.Columns.ColumnName]);
                                targetItem.ColumnType = Convert.ToString(item[SPDatabaseObjects.Columns.ColumnType]);
                                targetItem.StageWeight = Convert.ToInt32(item[SPDatabaseObjects.Columns.StageWeight]);
                                targetItem.TenantID = context.AppContext.TenantID;
                                ClientOM.FieldUserValue Createduser = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (Createduser != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(Createduser.LookupId));

                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));

                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                                
                            }
                        }
                        ULog.WriteLog($"{targetNewItemCount} Update ProjectSimilarityConfig");

                    } while (position != null);
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex.ToString());
                }
               
            }
        }
    }
}