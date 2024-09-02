
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Helpers;
using uGovernIT.Utility.Entities;
using DevExpress.Spreadsheet;
using Microsoft.AspNet.Identity;
using System.Collections;
using System.Text.RegularExpressions;
using uGovernIT.Utility;
using uGovernIT.Util.Log;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace uGovernIT.Manager
{
    public class ImportUserHelper
    {
        private static bool IsProcessActive;
        private static int totalRows;
        private static int processedRows;

        public bool ImportRunning()
        {
            return IsProcessActive;
        }

        public int PercentageComplete()
        {
            if (!IsProcessActive)
                return -1;

            if (processedRows > 0 && totalRows > 0)
            {
                if (processedRows == totalRows)
                    return 99; // final processing happening, so don't show 100%
                else
                    return processedRows * 100 / totalRows;
            }
            else
                return 0;
        }

        public static Tuple<string, int, int, int> ImportUsersElevated(ApplicationContext context, UserProfile user, string filePath, List<FieldAliasCollection> facColumns, string userID, int selectedUserType = 0, string FileName = "")
        {
            Tuple<string, int, int, int> message = new Tuple<string, int, int, int>(string.Empty, 0, 0, 0);

            try
            {
                IsProcessActive = true;
                
                            UserProfile currentUser = user;  
                            message = ImportUsers(filePath, facColumns, context, currentUser, selectedUserType);
                            List<string> error = new List<string>();
                            if (!string.IsNullOrWhiteSpace(message.Item1))
                                error = UGITUtility.ConvertStringToList(message.Item1, ",");

                context.UserManager.RefreshCache();
                //if (currentUser != null && !string.IsNullOrWhiteSpace(currentUser.Email))
                //{
                //    MailMessenger mail = new MailMessenger();
                //    StringBuilder body = new StringBuilder();
                //    string greeting = ConfigurationVariable.GetValue(spWeb, "Greeting");
                //    string signature = ConfigurationVariable.GetValue(spWeb, "Signature");
                //    body.AppendFormat("{0} {1},<br /><br />", greeting, currentUser.Name);
                //    body.AppendFormat("The User Import process has completed {0}: {1} new users imported, {2} users updated.",
                //                      (error == null || error.Count == 0 ? "successfully" : "unsuccessfully"), message.Item2, message.Item3);
                //    if (error != null && error.Count > 0)
                //        body.AppendFormat("<br /><br />ERRORS:<br />{0}", string.Join<string>("<br />", error));
                //    body.AppendFormat("<br /><br />{0}<br />", signature);
                //    mail.SendMail(currentUser.Email, "User Import Complete", string.Empty, body.ToString(), true, spWeb, false);
                //}

                // Mail sending feature written, below, so as to send mails, while importing users as Background Task (run in Background).
                MailMessenger mail = new MailMessenger(context);
                            if (currentUser != null && !string.IsNullOrWhiteSpace(currentUser.Email))
                            {
                                mail.SendMail(currentUser.Email, "Import Users Summary", "", message.Item1 +"</br> Imported Filename: "+FileName , true, new string[] { }, true);
                            }

                ULog.WriteUGITLog(context.CurrentUser.Id, message.Item1 + "</br> Imported Filename: " + FileName, Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), context.TenantID);

            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }
            finally
            {
                IsProcessActive = false;
            }

            return message;
        }

        public static Tuple<string, int, int, int> ImportUsers(string filePath, List<FieldAliasCollection> facColumns, ApplicationContext spWeb, UserProfile currentUser, int selectedUserType = 0)
        {
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(spWeb);
            UserProfileManager umanager = new  UserProfileManager(spWeb);
            umanager.UserValidator = new CustomUserValidator<UserProfile>(umanager);
            umanager.UserValidator = new Microsoft.AspNet.Identity.UserValidator<UserProfile>(umanager)
            {
                AllowOnlyAlphanumericUserNames = false,
            };
            List<string> errorMessage = new List<string>();

            Workbook workbook = new Workbook();
            workbook.LoadDocument(filePath);
            Worksheet worksheet = workbook.Worksheets[0];
            CellRange dataRange = worksheet.GetDataRange();
            DataTable data = worksheet.CreateDataTable(dataRange, true);
            List<string> lstColumn = null;
            if (data != null)
                lstColumn = data.Columns.Cast<DataColumn>().Select(x => x.ColumnName.Trim()).ToList();

            if (lstColumn == null || lstColumn.Count == 0 || dataRange.RowCount <= 1)
            {
                errorMessage.Add("ERROR: No data found in file!");
                return new Tuple<string, int, int, int>(errorMessage[0], 0, 0, 0);
            }

            totalRows = dataRange.RowCount - 1; // RowCount includes header row
            //Log.WriteLog(string.Format("Found {0} records in file", totalRows));

            RowCollection rows = worksheet.Rows;

            int newUsersCount = 0, updatedUsersCount = 0, skippedUserCount = 0, disabledUsersCount = 0;
            bool createUserInClaimsProvider = true;
            bool passwordChanged = false;
            Enums.UserType userType = Enums.UserType.NewADUser;
            StringBuilder sbUsrCreationFailure = new StringBuilder();
            StringBuilder sbFailedDepts = new StringBuilder();
            StringBuilder sbFailedFuncAreas = new StringBuilder();            
            StringBuilder sbFailedLocations = new StringBuilder();
            StringBuilder sbFailedMgrs = new StringBuilder();            
            StringBuilder sbFailedRoles = new StringBuilder();
            StringBuilder sbFailedGroups = new StringBuilder();
            StringBuilder sbFailedSkills = new StringBuilder();
            List<string> lstValues = new List<string>();
            Hashtable htUserManager = new Hashtable();
            //Condition to check if both AD and FBA users are configured
            if (configurationVariableManager.GetValueAsBool(ConfigConstants.AutoCreateNewUser) && (configurationVariableManager.GetValueAsBool(ConfigConstants.AutoCreateFBAUser)))
            {
                if (selectedUserType == 0)
                {
                    userType = Enums.UserType.NewADUser;
                }
                else if (selectedUserType == 1)
                {
                    userType = Enums.UserType.NewFBAUser;
                }
            }
            //Condition to check if only AD users are configured
            else if (configurationVariableManager.GetValueAsBool(ConfigConstants.AutoCreateNewUser))
            {
                userType = Enums.UserType.NewADUser;
            }
            //Condition to check if only AD users are configured
            else if (configurationVariableManager.GetValueAsBool(ConfigConstants.AutoCreateFBAUser))
            {
                userType = Enums.UserType.NewFBAUser;
            }
            else
            {
                createUserInClaimsProvider = false;
            }

            string memberGroupName = configurationVariableManager.GetValue(ConfigConstants.DefaultGroup);

            DepartmentManager departmentManager = new DepartmentManager(spWeb);
            CompanyDivisionManager companyDivisionManager = new CompanyDivisionManager(spWeb);
            DataTable dtDepartment = departmentManager.GetDataTable();
            FunctionalAreasManager functionalManager = new FunctionalAreasManager(spWeb);
            DataTable dtFunctionalArea = functionalManager.GetDataTable();
            LocationManager locationManager = new LocationManager(spWeb);
            DataTable dtLocation = locationManager.GetDataTable();
            UserSkillManager skillManager = new UserSkillManager(spWeb);
            DataTable dtSkills = skillManager.GetDataTable();
            //DataTable dtRoles = SPListHelper.GetDataTable(DatabaseObjects.Lists.UserRoles, spWeb);
            string defaultPassword = configurationVariableManager.GetValue(ConfigConstants.DefaultPassword);
            string defaultUserRole = configurationVariableManager.GetValue(ConfigConstants.DefaultUserRole);
            UserRoleManager userRole = new UserRoleManager(spWeb); // for User Groups
            //UserRolesManager userRoles = new UserRolesManager(spWeb); // for User Roles
            DataTable dtUserGroup = userRole.GetDataTable();  // User Groups
            //List<UserRoles> LstUserRole = userRoles.GetRoleList().Where(x => x.Deleted == false).ToList(); // User Roles like SMS User, PMO etc.,
            List<CompanyDivision> lstCompanyDivisions = companyDivisionManager.GetCompanyDivisionData();
            LandingPagesManager userRoles = new LandingPagesManager(spWeb); // UserRoles Table Renamed to LandingPages to avoid ambuiguity
            List<LandingPages> LstUserRole = userRoles.GetLandingPages().Where(x => x.Deleted == false).ToList(); // User Roles like SMS User, PMO etc.,
            string[] arrDept = null;
            long divisionID = 0;
            DataRow[] drs = null;
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            processedRows = 0;
            for (int i = dataRange.TopRowIndex+1; i <= dataRange.BottomRowIndex; i++)
            {
                processedRows++;
                arrDept = null;
                string rowErrorMsg = $"Skipped Line:{i + 1}:";
                List<string> missingvalues = new List<string>();
                Row rowData = rows[i];
                FieldAliasCollection facItemUserName = facColumns.Where(x => x.InternalName == "Username").FirstOrDefault();
                string loginName = GetValueByColumn(rowData, facItemUserName, lstColumn).ToLower();
                if (string.IsNullOrEmpty(loginName))
                {
                    missingvalues.Add($"login name");
                }

                FieldAliasCollection facItemName = facColumns.Where(x => x.InternalName == "Name").FirstOrDefault();
                string name = string.Empty;
                if (CheckColumnExistsInExcel(lstColumn, facItemName))
                {
                    name = GetValueByColumn(rowData, facItemName, lstColumn);
                    name = textInfo.ToTitleCase(name.ToLower());
                }
                if (string.IsNullOrEmpty(name))
                {
                    missingvalues.Add($"name");
                }

                FieldAliasCollection facItemPassword = facColumns.Where(x => x.InternalName == "Password").FirstOrDefault();
                string password = GetValueByColumn(rowData, facItemPassword, lstColumn);
                if (string.IsNullOrEmpty(password))
                    password = defaultPassword;

                FieldAliasCollection facItemEMail = facColumns.Where(x => x.InternalName == "EMail").FirstOrDefault();
                string email = GetValueByColumn(rowData, facItemEMail, lstColumn);
                if(string.IsNullOrEmpty(email))
                    missingvalues.Add($"email");

                FieldAliasCollection facItemJobTitle = facColumns.Where(x => x.InternalName == "JobTitle").FirstOrDefault();
                string jobProfile = string.Empty;
                long jobTitleLookup = 0;
                string jobRoleId = string.Empty;
                if (CheckColumnExistsInExcel(lstColumn, facItemJobTitle))
                {
                    jobProfile = GetValueByColumn(rowData, facItemJobTitle, lstColumn);
                        if (string.IsNullOrEmpty(jobProfile))
                        {
                            missingvalues.Add($"job profile");
                        }
                    else
                    {
                        JobTitleManager jobTitleMGR = new JobTitleManager(spWeb);
                        JobTitle jobtitleobj = jobTitleMGR.Load(x => x.Title == jobProfile).FirstOrDefault();
                        if (jobtitleobj != null)
                        {
                            jobTitleLookup = jobtitleobj.ID;
                            jobRoleId = jobtitleobj.RoleId;
                        }
                        else
                        {
                            missingvalues.Add($"Job Title '{jobProfile}'");
                            jobProfile = string.Empty;
                        }
                    }
                }

                FieldAliasCollection facItemDepartment = facColumns.Where(x => x.InternalName == "DepartmentLookup").FirstOrDefault();
                string userDepartment = string.Empty;
                divisionID = 0;
                drs = null;
                if (dtDepartment != null && dtDepartment.Rows.Count > 0 && CheckColumnExistsInExcel(lstColumn, facItemDepartment))
                {
                    string dept = GetValueByColumn(rowData, facItemDepartment, lstColumn);
                    if (!string.IsNullOrEmpty(dept))
                    {
                        dept = dept.Replace("'", "''"); // Escape single quotes, else LINQ query below will fail
                        //BTS-22-000975: Check if the value is in form Division > Department
                        if (dept.Contains(">"))
                        { 
                            arrDept = dept.Split('>');
                            if (!string.IsNullOrEmpty(arrDept[0]))
                            { //BTS-22-000975: Find out Division id based on the text
                                divisionID = lstCompanyDivisions.FirstOrDefault(x => x.Title == arrDept[0].Trim()).ID;
                            }
                            if (divisionID > 0) //Find out Department id based on the Divn and Dept text
                                drs = dtDepartment.Select(string.Format("{0} = '{1}' AND {2} = {3}", DatabaseObjects.Columns.Title, arrDept[1].Trim(), DatabaseObjects.Columns.DivisionIdLookup, divisionID));
                            else
                                drs = dtDepartment.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.Title, arrDept[1].Trim()));
                        }
                        else
                            drs = dtDepartment.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.Title, dept));

                        if (drs != null && drs.Length > 0)
                        {
                            userDepartment = Convert.ToString(drs[0][DatabaseObjects.Columns.Id]);
                        }
                        else
                        {
                            missingvalues.Add($"Department '{dept}'");
                        }
                    }
                    else
                    {
                        missingvalues.Add($"Department");
                    }
                }

                if(missingvalues.Count > 0)
                {
                    errorMessage.Add($"{rowErrorMsg} Invalid values {UGITUtility.ConvertListToString(missingvalues, uGovernIT.Utility.Constants.Separator6)}");
                    skippedUserCount++;
                    continue;
                }


                bool newUserCreated = false;
                if (createUserInClaimsProvider)
                    newUserCreated = spWeb.UserManager.SaveNewUser(spWeb, loginName, password, name, email, userType);
                
                UserProfile userCreated = new UserProfile() { UserName=@loginName.Trim(), Email=email, Enabled=true, Name= name, TenantID = spWeb.TenantID };
                userCreated.IsServiceAccount = false;
                IdentityResult result = umanager.Create(userCreated, password);

                if (result.Succeeded == false)
                {
                    //errorMessage.Add($"User creation failed: {name}");
                    //errorMessage.Add(result.Errors.FirstOrDefault());
                    skippedUserCount++;
                    //continue;
                }
                if (result.Succeeded == true)
                {
                    newUserCreated = true;

                    if (configurationVariableManager.GetValueAsBool(ConfigConstants.SendUserCredentialMail))
                    {
                        passwordChanged = true;
                        string successmail = $"Your Account have been created successfully! Please login with this credential.<br><br><br>Account Id: {spWeb.TenantAccountId}"
                            + $"<br>User Name: {userCreated.UserName}<br>Password: {password}";
                        MailMessenger mail = new MailMessenger(spWeb);
                        if (currentUser != null && !string.IsNullOrWhiteSpace(currentUser.Email))
                        {
                            mail.SendMail(currentUser.Email, "Import Users Summary", "", successmail, true, new string[] { }, true);
                        }
                    }
                    newUsersCount++;
                }

                userCreated = umanager.GetUserOnlyByUserName(loginName);

                if (userCreated == null || !userCreated.TenantID.Equals(spWeb.TenantID, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }


                FieldAliasCollection facItemPhone = facColumns.Where(x => x.InternalName == "Phone").FirstOrDefault();
                if (CheckColumnExistsInExcel(lstColumn, facItemPhone))
                {
                    userCreated.PhoneNumber = GetValueByColumn(rowData, facItemPhone, lstColumn);
                }


                #region userdependentcode
                DateTime minDate = DateTime.Today;
                DateTime maxDate = new DateTime(8900, 12, 31);
                DateTime updatedDate = DateTime.Today;

                if (newUserCreated)
                {
                    userCreated.UGITStartDate = minDate;
                    userCreated.UGITEndDate = maxDate;
                }

                // Start date.
                FieldAliasCollection facItemStartDate = facColumns.Where(x => x.InternalName == "UGITStartDate").FirstOrDefault();
                if (CheckColumnExistsInExcel(lstColumn, facItemStartDate))
                {
                    updatedDate = GetDateValueFromSerialNumber(minDate, GetValueByColumn(rowData, facItemStartDate, lstColumn));
                    if (updatedDate != minDate)
                        userCreated.UGITStartDate = updatedDate;
                }

                // End date.
                FieldAliasCollection facItemEndDate = facColumns.Where(x => x.InternalName == "UGITEndDate").FirstOrDefault();
                if (CheckColumnExistsInExcel(lstColumn, facItemEndDate))
                {
                    updatedDate = GetDateValueFromSerialNumber(maxDate, GetValueByColumn(rowData, facItemEndDate, lstColumn));
                    if (updatedDate <= maxDate)
                        userCreated.UGITEndDate = updatedDate;
                }

                if (CheckColumnExistsInExcel(lstColumn, facItemJobTitle))
                {
                    userCreated.JobProfile = jobProfile;
                    userCreated.JobTitleLookup = jobTitleLookup;
                    userCreated.GlobalRoleId = jobRoleId;
                }
				if (CheckColumnExistsInExcel(lstColumn, facItemDepartment))
					userCreated.Department = UGITUtility.ObjectToString(userDepartment);
                drs = null;
                FieldAliasCollection facItemFunctionalArea = facColumns.Where(x => x.InternalName == "FunctionalAreaLookup").FirstOrDefault();
                if (dtFunctionalArea != null && dtFunctionalArea.Rows.Count > 0 && CheckColumnExistsInExcel(lstColumn, facItemFunctionalArea))
                {
                    string funcArea = GetValueByColumn(rowData, facItemFunctionalArea, lstColumn);
                    if (!string.IsNullOrEmpty(funcArea))
                    {
                        funcArea = funcArea.Replace("'", "''"); // Escape single quotes, else LINQ query below will fail
                        drs = dtFunctionalArea.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.Title, funcArea));
                        if (drs != null && drs.Length > 0)
                        {
                            userCreated.FunctionalArea = UGITUtility.StringToInt(drs[0][DatabaseObjects.Columns.ID]);
                        }
                        else
                        {
                            missingvalues.Add($"{name} '{funcArea}'");                            
                        }
                    }
                }


                //LocationLookup
                FieldAliasCollection facItemLocation = facColumns.Where(x => x.InternalName == "LocationLookup").FirstOrDefault();
                if (dtLocation != null && dtLocation.Rows.Count > 0 && CheckColumnExistsInExcel(lstColumn, facItemLocation))
                {
                    string location = GetValueByColumn(rowData, facItemLocation, lstColumn);
                    if (!string.IsNullOrEmpty(location))
                    {
                        location = location.Replace("'", "''"); // Escape single quotes, else LINQ query below will fail
                        DataRow[] drLocations = dtLocation.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.Title, location));
                        if (drLocations != null && drLocations.Length > 0)
                        {
                            userCreated.Location = UGITUtility.ObjectToString(drLocations[0][DatabaseObjects.Columns.ID]);
                        }
                        else
                        {
                            missingvalues.Add($"{name} '{location}'");
                        }
                    }
                }
                               

                // NotificationEmail
                FieldAliasCollection facItemNotificationEmail = facColumns.Where(x => x.InternalName == "NotificationEmail").FirstOrDefault();
                if (CheckColumnExistsInExcel(lstColumn, facItemNotificationEmail))
                    userCreated.NotificationEmail = GetValueByColumn(rowData, facItemNotificationEmail, lstColumn);

                // DeskLocation
                FieldAliasCollection facItemDeskLocation = facColumns.Where(x => x.InternalName == "DeskLocation").FirstOrDefault();
                if (CheckColumnExistsInExcel(lstColumn, facItemDeskLocation))
                    userCreated.DeskLocation = GetValueByColumn(rowData, facItemDeskLocation, lstColumn);

                // Employee ID
                FieldAliasCollection facItemEmployeeID = facColumns.Where(x => x.InternalName == "EmployeeId").FirstOrDefault();
                if (CheckColumnExistsInExcel(lstColumn, facItemEmployeeID))
                    userCreated.EmployeeId = GetValueByColumn(rowData, facItemEmployeeID, lstColumn);

                if(!string.IsNullOrEmpty(email))
                    userCreated.Email= email;
                if (!string.IsNullOrEmpty(name))
                    userCreated.Name = name;
                // Resource Hourly Rate
                FieldAliasCollection facResourceHourlyRate = facColumns.Where(x => x.InternalName == "ResourceHourlyRate").FirstOrDefault();
                if (CheckColumnExistsInExcel(lstColumn, facResourceHourlyRate))
                    userCreated.HourlyRate = UGITUtility.StringToInt( GetValueByColumn(rowData, facResourceHourlyRate, lstColumn));

                FieldAliasCollection facItemIsConsultant = facColumns.Where(x => x.InternalName == "IsConsultant").FirstOrDefault();
                if (CheckColumnExistsInExcel(lstColumn, facItemIsConsultant))
                {
                    string consultant = GetValueByColumn(rowData, facItemIsConsultant, lstColumn);
                    if (!string.IsNullOrWhiteSpace(consultant)) // only set value if there is an actual value in the column
                        userCreated.IsConsultant = UGITUtility.StringToBoolean(consultant) ? true : false;
                }

                FieldAliasCollection facItemIT = facColumns.Where(x => x.InternalName == "IT").FirstOrDefault();
                if (CheckColumnExistsInExcel(lstColumn, facItemIT))
                {
                    string isIT = GetValueByColumn(rowData, facItemIT, lstColumn);
                    if (!string.IsNullOrWhiteSpace(isIT)) // only set value if there is an actual value in the column
                        userCreated.IsIT = UGITUtility.StringToBoolean(isIT) ? true : false;
                }

                FieldAliasCollection facItemIsManager = facColumns.Where(x => x.InternalName == "IsManager").FirstOrDefault();
                if (CheckColumnExistsInExcel(lstColumn, facItemIsManager))
                {
                    string isManager = GetValueByColumn(rowData, facItemIsManager, lstColumn);
                    if (!string.IsNullOrWhiteSpace(isManager)) // only set value if there is an actual value in the column
                        userCreated.IsManager = UGITUtility.StringToBoolean(isManager) ? true : false;
                }

                                
                FieldAliasCollection facItemManager = facColumns.Where(x => x.InternalName == "ManagerLookup").FirstOrDefault();
                if (CheckColumnExistsInExcel(lstColumn, facItemManager))
                {
                    string Usermanager = GetValueByColumn(rowData, facItemManager, lstColumn);
                  
                    if (!string.IsNullOrEmpty(Usermanager))
                        htUserManager.Add(userCreated.Id, Usermanager);
                }


                #endregion userdependentcode

                //// If Excel file contains Enabled column use it to enable/disable users, else assume all users in file are enabled!
                bool enabled = true;
                FieldAliasCollection facItemEnabled = facColumns.Where(x => x.InternalName == "Enabled").FirstOrDefault();
                if (CheckColumnExistsInExcel(lstColumn, facItemEnabled))
                {
                    string enabledValue = GetValueByColumn(rowData, facItemEnabled, lstColumn);
                    if (!string.IsNullOrWhiteSpace(enabledValue)) // only set value if there is an actual value in the column
                    {
                        enabled = UGITUtility.StringToBoolean(enabledValue);
                        userCreated.Enabled = enabled;
                        if (!enabled)
                            disabledUsersCount++;
                    }
                }
                else
                    userCreated.Enabled = true;

                //bool ForcePasswordChangeForNewUsers = configurationVariableManager.GetValueAsBool(ConfigConstants.ForcePasswordChangeForNewUsers);
                //if (newUserCreated)
                //{
                //    if (ForcePasswordChangeForNewUsers)
                //    {
                //        userItem[DatabaseObjects.Columns.EnablePasswordExpiration] = true;
                //        userItem[DatabaseObjects.Columns.PasswordExpiryDate] = DateTime.Now.AddDays(-1);
                //    }
                //    else
                //    {
                //        userItem[DatabaseObjects.Columns.EnablePasswordExpiration] = false;
                //        userItem[DatabaseObjects.Columns.PasswordExpiryDate] = DateTime.MaxValue;
                //    }
                //    newUsersCount++;
                //}
                //else
                //    updatedUsersCount++;

                //Skills
                FieldAliasCollection facItemSkills = facColumns.Where(x => x.InternalName == "Skills").FirstOrDefault();
                if (dtSkills != null && dtSkills.Rows.Count > 0 && CheckColumnExistsInExcel(lstColumn, facItemSkills))
                {
                    string Skills = GetValueByColumn(rowData, facItemSkills, lstColumn);
                    if (!string.IsNullOrEmpty(Skills))
                    {
                        Skills = Skills.Replace("'", "''"); // Escape single quotes, else LINQ query below will fail
                        string SkillName = string.Join(",", Skills.Split(';').Select(x => string.Format("'{0}'", x)).ToList());
                        lstValues.Clear();
                        lstValues = Skills.Split(';').ToList();
                        DataRow[] drSkills = dtSkills.Select(string.Format("{0} in ({1})", DatabaseObjects.Columns.Title, SkillName));
                        //DataRow[] drSkills = dtSkills.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.Title, Skills));
                        if (drSkills != null && drSkills.Count() > 0)
                        {
                            foreach (var item in drSkills)
                            {
                                userCreated.Skills = userCreated.Skills + UGITUtility.ObjectToString(item[DatabaseObjects.Columns.ID]) + ",";
                                lstValues.Remove(Convert.ToString(item[DatabaseObjects.Columns.Title]));
                            }
                            userCreated.Skills = userCreated.Skills.TrimEnd(',');
                        }

                        if (lstValues.Count > 0)
                        {
                            missingvalues.Add($"{name} '{String.Join(";", lstValues)}'");
                            //sbFailedSkills.AppendFormat("<br />{0} - {1}", name, String.Join(";", lstValues));
                        }
                    }
                }

                //User Roles
                FieldAliasCollection facItemUserRole = facColumns.Where(x => x.InternalName == "UserRoleLookup").FirstOrDefault();
                if (LstUserRole != null && LstUserRole.Count > 0 && CheckColumnExistsInExcel(lstColumn, facItemUserRole))
                {
                    string UsrRole = GetValueByColumn(rowData, facItemUserRole, lstColumn);
                    if (!string.IsNullOrEmpty(UsrRole))
                    {
                        userCreated.UserRoleId = LstUserRole.Where(x => x.Name.Equals(UsrRole, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Id).FirstOrDefault();

                        if (userCreated.UserRoleId == null)
                        {
                            missingvalues.Add($"{name} '{UsrRole}'");
                            //sbFailedRoles.AppendFormat("<br />{0} - {1}", name, UsrRole);
                        }
                    }
                }

                result = umanager.Update(userCreated);
                
                // User Groups                
                FieldAliasCollection facItemUserGroup = facColumns.Where(x => x.InternalName == "Groups").FirstOrDefault();
                if (dtUserGroup != null && dtUserGroup.Rows.Count > 0 && CheckColumnExistsInExcel(lstColumn, facItemUserGroup))
                {
                    string userRoleValues = GetValueByColumn(rowData, facItemUserGroup, lstColumn);
                    if (!string.IsNullOrEmpty(userRoleValues))
                    {
                        lstValues.Clear();
                        lstValues = userRoleValues.Split(';').ToList();
                        for (int idx = 0; idx < lstValues.Count; idx++)
                        {
                            lstValues[idx] = lstValues[idx].Trim();
                        }

                        string userRolesName = string.Join(",", userRoleValues.Split(';').Select(x => string.Format("'{0}'", x.Trim())).ToList());
                        DataRow[] drUserRoles = dtUserGroup.Select(string.Format("{0} in ({1})", DatabaseObjects.Columns.Title, userRolesName));
                        if (drUserRoles != null && drUserRoles.Count() > 0)
                        {
                            foreach (var item in drUserRoles)
                            {
                                umanager.AddUserRole(userCreated, Convert.ToString(item[DatabaseObjects.Columns.Name]));
                                lstValues.Remove(Convert.ToString(item[DatabaseObjects.Columns.Title]));
                            }
                        }

                        if (lstValues.Count > 0)
                        {
                            foreach (var item in lstValues)
                            {
                                if(userRole.GetRolesByName(item).Count <= 0)
                                    userRole.AddOrUpdate(new Role { Title = item, Name = Regex.Replace(item, @"\s", "") });

                                umanager.AddUserRole(userCreated, Regex.Replace(item, @"\s", ""));
                            }

                            //sbFailedGroups.AppendFormat("<br />{0} - {1}", name, String.Join(";", lstValues));
                        }
                    }
                }

                // Password
                facItemPassword = facColumns.Where(x => x.InternalName == "Password").FirstOrDefault();
                if (CheckColumnExistsInExcel(lstColumn, facItemPassword) && passwordChanged == false)
                {
                    password = GetValueByColumn(rowData, facItemPassword, lstColumn);
                    if (!string.IsNullOrEmpty(password))
                    {
                        string passwordToken = umanager.GeneratePasswordResetToken(userCreated.Id.Trim());
                        result = umanager.ResetPassword(userCreated.Id.Trim(), passwordToken, password.Trim());
                    }
                }
                passwordChanged = false;

                if (missingvalues.Count > 0)
                    errorMessage.Add($"{rowErrorMsg}{UGITUtility.ConvertListToString(missingvalues, uGovernIT.Utility.Constants.Separator6)}");
                //newUsersCount++;

                //string logMessage = string.Format("updated user: {0}{1}", spUser.Name, enabled ? "" : " (DISABLED)");
                //Log.WriteUGITLog(spWeb, logMessage, UGITLogSeverity.Information, UGITLogCategory.UserProfile, currentUser, string.Empty, string.Empty);
                //Log.WriteLog(logMessage);
            }
                        
            var LstUserProfile = umanager.GetUsersProfile().Where(x => x.TenantID.Equals(spWeb.TenantID, StringComparison.InvariantCultureIgnoreCase)).Select(x => new { x.Id, x.Name, x.UserName }).ToList();
            UserProfile user = new UserProfile();
            foreach (DictionaryEntry item in htUserManager)
            {                
                user = umanager.GetUserById(Convert.ToString(item.Key));
                if (user != null)
                {
                    //var managerId = UserManager.GetUserByUserName(Convert.ToString(item.Value));
                    var manager = LstUserProfile.FirstOrDefault(x => x.Name.Equals(Convert.ToString(item.Value), StringComparison.InvariantCultureIgnoreCase));
                    if (manager != null)
                    {
                        user.ManagerID = manager.Id;
                        umanager.Update(user);
                    }           
                    else
                    {
                        //missingvalues.Add($"{user.UserName} '{Convert.ToString(item.Value)}'");
                        //sbFailedMgrs.AppendFormat("<br />{0} - {1}", user.UserName, Convert.ToString(item.Value));
                    }
                }
            }
           
            processedRows = totalRows;

            //string msg = string.Format("The User Import process has completed successfully: {0} new users imported, {1} updated, {2} disabled.", newUsersCount, updatedUsersCount, disabledUsersCount);

            string msg = $"The User Import process has completed successfully:<br>  {newUsersCount} New user(s) imported, {updatedUsersCount} Updated, {disabledUsersCount} disabled." +
                $"<br><br>{UGITUtility.ConvertListToString(errorMessage, "<BR>")}";
                                       

            
            return new Tuple<string, int, int, int>(msg, newUsersCount, skippedUserCount, updatedUsersCount);
        }

        private static bool CheckColumnExistsInExcel(List<string> lstColumn, FieldAliasCollection facItemGroup)
        {
            if (lstColumn.Contains(facItemGroup.InternalName, StringComparer.OrdinalIgnoreCase))
                return true;

            foreach (var anItem in facItemGroup.AliasNames.Split(','))
            {
                if (lstColumn.Contains(anItem, StringComparer.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public static string GetValueByColumn(Row row, FieldAliasCollection fieldAliasName, List<string> lstColumn)
        {
            if (row == null || fieldAliasName == null || lstColumn == null)
            {
                //Log.WriteLog("ERROR: Null values passed into GetValueByColumn!");
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(fieldAliasName.InternalName))
            {
                int index = lstColumn.FindIndex(s => s.Equals(fieldAliasName.InternalName, StringComparison.OrdinalIgnoreCase)); // Do case-insensitive comparison
                if (index != -1)
                {
                    string value = row[index].DisplayText;
                    if (!string.IsNullOrEmpty(value))
                        value = value.Trim();
                    return value;
                }
            }

            if (!string.IsNullOrEmpty(fieldAliasName.AliasNames))
            {
                string[] aliasNames = fieldAliasName.AliasNames.Split(',');
                foreach (var tempName in aliasNames)
                {
                    int index = lstColumn.FindIndex(s => s.Equals(tempName, StringComparison.OrdinalIgnoreCase)); // Do case-insensitive comparison
                    if (index != -1)
                    {
                        string value = row[index].DisplayText;
                        if (!string.IsNullOrEmpty(value))
                            value = value.Trim();
                        return value;
                    }
                }
            }

            return string.Empty; // Not found!
        }

        /// <summary>
        /// Get date value from excel serial date number.
        /// </summary>
        /// <param name="dateValue"></param>
        /// <returns></returns>
        public static DateTime GetDateValueFromSerialNumber(DateTime defaultDate, string dateValue)
        {
            //DateTime dt = DateTime.MinValue;
            DateTime dt = defaultDate;
            dt = UGITUtility.StringToDateTime(dateValue);
            if (dt == DateTime.MinValue)
            {
                if (double.TryParse(dateValue, out double dateSerialNumber))
                {
                    dt = DateTime.FromOADate(dateSerialNumber);
                }
                else
                {
                    dt = defaultDate;
                }
            }
            
            return dt;
        }
    }
}
