using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Web;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using uGovernIT.Util.Log;

namespace uGovernIT.Manager
{
    public  class ApplicationHelperManager
    {
        ApplicationContext context =null;
        ConfigurationVariableManager objConfigurationVariableManager = null;
        public ApplicationHelperManager(ApplicationContext _context)
        {
            context = _context;
            objConfigurationVariableManager = new ConfigurationVariableManager(_context);
        }
        public  void UpdateAppAccessedUser(DataRow item, string list)
        {
            if (item == null)
                return;
            
            string query =string.Format("{0}={1}", DatabaseObjects.Columns.APPTitleLookup, Convert.ToInt32(item["ID"]));
            //query.ViewFields = string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.ApplicationRoleAssign);

            //DataRow[] spListItemColl =GetTableDataManager.GetTableData(list,context.TenantID).Select(query);
            DataRow[] spListItemColl = GetTableDataManager.GetTableData(list, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select(query);
            if (spListItemColl != null && spListItemColl.Count() > 0)
            {
                DataTable dt = spListItemColl.CopyToDataTable();
                DataView view = dt.DefaultView;
                item[DatabaseObjects.Columns.NumUsers] = view.ToTable(true, DatabaseObjects.Columns.ApplicationRoleAssign).Rows.Count;
            }
            else
                item[DatabaseObjects.Columns.NumUsers] = 0;

            Ticket ticket = new Ticket(context, "APP");
            ticket.CommitChanges(item, string.Empty);

            // Send license exceeded notification if needed
            LicenseExceededNotification(item);
        }

        public  void LicenseExceededNotification(DataRow item)
        {          
            int totalLicenses = UGITUtility.StringToInt(item[DatabaseObjects.Columns.NumLicensesTotal]);
            int numUsers = UGITUtility.StringToInt(item[DatabaseObjects.Columns.NumUsers]);
            if (totalLicenses == 0 || numUsers == 0)
                return; // No licenses or no users

            string ownerEmails = uHelper.GetUserEmailList(UGITUtility.ConvertStringToList(Convert.ToString(item[DatabaseObjects.Columns.TicketOwner]),Constants.Separator6),context);
            if (string.IsNullOrEmpty(ownerEmails))
            {
                ULog.WriteLog("Can't send License Violation email, missing owner email address");
                return; // No one to send to
            }

            string appName = Convert.ToString(item[DatabaseObjects.Columns.Title]);
            double appLicenseThresholdPct = UGITUtility.StringToDouble(objConfigurationVariableManager.GetValue(ConfigConstants.AppLicenseThresholdPct));

            bool sendNotification = false;
            if (numUsers > totalLicenses)
                sendNotification = true; // exceeded available licenses
            else if (totalLicenses > numUsers && appLicenseThresholdPct > 0)
            {
                double appLicenseThreshold = totalLicenses - (Convert.ToDouble(totalLicenses) * appLicenseThresholdPct / 100.0);
                //Log.WriteLog(string.Format("License Threshold: {0}", appLicenseThreshold));
                if (numUsers >= appLicenseThreshold)
                    sendNotification = true; // exceeded license threshold
            }

            if (sendNotification)
            {
                MailMessenger mail = new MailMessenger(context);
                string greeting = objConfigurationVariableManager.GetValue( "Greeting");
                string signature = objConfigurationVariableManager.GetValue( "Signature");
                string subject = string.Format("License Threshold Warning for {0}", appName);

                StringBuilder body = new StringBuilder();
                body.AppendFormat("{0},<br /><br />", greeting);
                body.AppendFormat("The number of users of the application <b>{0}</b> has exceeded or is close to exceeding the number of available licenses. Please take appropriate action to purchase more licenses or reduce the number of users.<br /><br />", appName);
                body.AppendFormat("Total Licenses: {0}<br />", totalLicenses);
                body.AppendFormat("Licences Used: {0}<br />", numUsers);
                if (numUsers > totalLicenses)
                    body.AppendFormat("Excess Licences: {0}<br />", numUsers - totalLicenses);
                else
                    body.AppendFormat("Remaining license count ({0}) is less than configured threshold of {1}%<br />", totalLicenses - numUsers, appLicenseThresholdPct);

                body.AppendFormat("<br /><br />{0}<br />", signature);
                mail.SendMail(ownerEmails, subject, string.Empty, body.ToString(), true);
            }
        }

        public  void UpdateApplicationSpecificAccess(UGITTask applicationTask)
        {
            string userID = string.Empty;
            XmlDocument doc = new XmlDocument();
            string applicationAccessXml = HttpContext.Current.Server.HtmlDecode(applicationTask.ServiceApplicationAccessXml);
            doc.LoadXml(applicationAccessXml);

            ServiceMatrixData serviceMatrixData = new ServiceMatrixData();
            serviceMatrixData = (ServiceMatrixData)uHelper.DeSerializeAnObject(doc, serviceMatrixData);
            if (applicationTask.NewUserName != "" && string.IsNullOrEmpty(serviceMatrixData.RoleAssignee))
            {
                UserProfile user = context.UserManager.GetUserByUserName(applicationTask.NewUserName);

                if (user != null)
                    serviceMatrixData.RoleAssignee = Convert.ToString(user.Id);
            }
            if (serviceMatrixData != null && !string.IsNullOrEmpty(serviceMatrixData.RoleAssignee))
                userID = Convert.ToString(serviceMatrixData.RoleAssignee);

            UpdateApplicationSpecificAccess(userID, serviceMatrixData);
        }
        public  void UpdateApplicationSpecificAccess(string userID, ServiceMatrixData serviceMatrixData)
        {
            if (string.IsNullOrEmpty(userID))
                return;

            string userRoleAssignee =Convert.ToString(userID);

            Dictionary<string, string> dic = new Dictionary<string, string>();
            ServiceRequestBL requestBL = new ServiceRequestBL(context);

            string spQuery = string.Format("{0}={1} and {2}='{3}'", DatabaseObjects.Columns.APPTitleLookup, UGITUtility.StringToInt(serviceMatrixData.ID), DatabaseObjects.Columns.ApplicationRoleAssign, userID);

            //DataRow[] spListItemColl =GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplModuleRoleRelationship).Select(spQuery);
            ApplicationAccessManager appAccessManager = new Manager.ApplicationAccessManager(context);
            List<ApplicationAccess> lstAppAccess = appAccessManager.Load(x => x.APPTitleLookup == UGITUtility.StringToInt(serviceMatrixData.ID) && x.ApplicationRoleAssign == userID);
            if (lstAppAccess != null && lstAppAccess.Count() > 0)
            {
                DeleteMappings(ref dic, serviceMatrixData, userID,ref lstAppAccess);//remove access of particular application of which access is removed by the user
            }

            string query = string.Format("{0}={1}", DatabaseObjects.Columns.APPTitleLookup, serviceMatrixData.ID);

            DataRow[] splistAppRoles = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplicationRole, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select(query);
            ApplicationRoleManager appRoleManager = new ApplicationRoleManager(context);

            string queryModule = string.Format("{0}={1}", DatabaseObjects.Columns.APPTitleLookup, serviceMatrixData.ID);

            ApplicationModuleManager appModuleManager = new ApplicationModuleManager(context);
            List<ApplicationModule> splistAppModules = appModuleManager.Load(x => x.APPTitleLookup == UGITUtility.StringToLong(serviceMatrixData.ID));
            ApplicationAccess dr = null;
            foreach (ServiceData serviceData in serviceMatrixData.lstGridData)
            {
                if (lstAppAccess != null && lstAppAccess.Count() > 0)
                {
                    dr = lstAppAccess.FirstOrDefault(x => appRoleManager.LoadByID(Convert.ToInt64(x.ApplicationRoleLookup)).Title == serviceData.RowName && appModuleManager.LoadByID(Convert.ToInt64(x.ApplicationModulesLookup)).Title == serviceData.ColumnName &&  x.APPTitleLookup == Convert.ToInt64( serviceMatrixData.ID));
                }
                if (dr == null)//add item if it does not exist in db
                {
                    ApplicationAccess spListItem = new ApplicationAccess(); //GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplModuleRoleRelationship).NewRow();
                    spListItem.APPTitleLookup = Convert.ToInt64(serviceData.ID);
                    spListItem.Title = serviceData.RowName + " - " + serviceData.ColumnName;
                    if (serviceMatrixData.Name.Trim().ToLower() == serviceData.ColumnName.Trim().ToLower())
                    {
                        spListItem.ApplicationModulesLookup = null;
                        if (!dic.ContainsKey(string.Format("application {0} - role {1}", Convert.ToString(serviceMatrixData.Name), serviceData.RowName)))
                        {
                            dic.Add(string.Format("application {0} - role {1}", Convert.ToString(serviceMatrixData.Name), serviceData.RowName), "inserted");
                        }
                    }
                    else
                    {
                        if (splistAppModules != null && splistAppModules.Count() > 0)
                        {
                            ApplicationModule drModule = splistAppModules.FirstOrDefault(x => x.Title == serviceData.ColumnName);
                            if (drModule != null)
                                spListItem.ApplicationModulesLookup = Convert.ToInt64(drModule.ID);
                        }
                        if (!dic.ContainsKey(string.Format("{0} - {1}", serviceData.ColumnName, serviceData.RowName)))
                        {
                            dic.Add(string.Format("{0} - {1}", serviceData.ColumnName, serviceData.RowName), "inserted");
                        }
                    }
                    if (splistAppRoles != null && splistAppRoles.Count() > 0)
                    {
                        DataRow drRole = splistAppRoles.CopyToDataTable().AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.Title) == serviceData.RowName);
                        if (drRole != null)
                            spListItem.ApplicationRoleLookup = Convert.ToInt64(drRole[DatabaseObjects.Columns.Id]);
                        spListItem.Description = string.Empty;
                        spListItem.ApplicationRoleAssign = userRoleAssignee;
                        ApplicationAccessManager appAccessMGR = new ApplicationAccessManager(context);
                        if (spListItem.ID > 0)
                            appAccessMGR.Update(spListItem);
                        else
                            appAccessMGR.Insert(spListItem);
                       // spListItem.Update();
                    }
                }
            }

            if (dic != null && dic.Count > 0)
            {
                string history = string.Empty;
                string action = string.Empty;
                string historyHeader = string.Empty;
                foreach (KeyValuePair<string, string> entry in dic)
                {
                    if (action != entry.Value)
                    {
                        action = entry.Value;
                        if (history != string.Empty)
                            history += "<br/>";
                        history += string.Format("Access {0} for <b>User:</b> {1} on <br/>", 
                                                 action,context.UserManager.GetUserById(userRoleAssignee) != null ? context.UserManager.GetUserById(userRoleAssignee).Name : userID.ToString());
                    }
                    history += string.Format("{0};", entry.Key);
                }

                history = history.Substring(0, history.Length - 1);
                DataRow spListItemApp = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select(string.Format("{0}={1}",DatabaseObjects.Columns.Id, Convert.ToString(serviceMatrixData.ID)))[0];

                //Update Number of user on updating access of user at application
                UpdateAppAccessedUser(spListItemApp, DatabaseObjects.Tables.ApplModuleRoleRelationship);

                uHelper.CreateHistory(context.CurrentUser, history, spListItemApp,context);
            }
        }

        private  void DeleteMappings(ref Dictionary<string, string> dic, ServiceMatrixData ServiceMatrixData, string userID,ref List<ApplicationAccess> colExistingExcess)
        {

            ApplicationAccessManager appAccessMGR = new ApplicationAccessManager(context);
            if (colExistingExcess != null && colExistingExcess.Count() > 0)
            {
                List<int> lstItemsToBeDeleted = new List<int>();
                for (int i = 0; i < colExistingExcess.Count(); i++)
                {
                    string spFieldLookupValueAppl = Convert.ToString(colExistingExcess[i].APPTitleLookup);
                    string spFieldLookupValueRoles =Convert.ToString(colExistingExcess[i].ApplicationRoleLookup);
                    if (string.IsNullOrEmpty(Convert.ToString(colExistingExcess[i].ApplicationModulesLookup)))
                    {
                        ServiceData item = ServiceMatrixData.lstGridData.Where(c => c.RowName == Convert.ToString(spFieldLookupValueAppl) && c.ColumnName == Convert.ToString(spFieldLookupValueRoles)).Select(c => c).FirstOrDefault();

                        if (item == null)
                        {

                            dic.Add(string.Format("application {0} - role {1}", Convert.ToString(spFieldLookupValueAppl), Convert.ToString(spFieldLookupValueRoles)), "deleted");
                            lstItemsToBeDeleted.Add(UGITUtility.StringToInt(colExistingExcess[i].ID));
                        }
                    }
                    else
                    {
                        string spFieldLookupValueModules = Convert.ToString(colExistingExcess[i].ApplicationModulesLookup);
                        ServiceData item = ServiceMatrixData.lstGridData.Where(c => c.RowName == Convert.ToString(spFieldLookupValueRoles) & c.ColumnName == Convert.ToString(spFieldLookupValueModules)).Select(c => c).FirstOrDefault();
                        if (item == null)
                        {
                            dic.Add(string.Format("{0} - {1}", Convert.ToString(spFieldLookupValueModules), Convert.ToString(spFieldLookupValueRoles)), "deleted");
                            lstItemsToBeDeleted.Add(UGITUtility.StringToInt(colExistingExcess[i].ID));

                        }
                    }
                }
                foreach (int id in lstItemsToBeDeleted)
                {
                    ApplicationAccess access = appAccessMGR.LoadByID(id);;
                    appAccessMGR.Delete(access);
                    //access.ID = lstItemsToBeDeleted[0];

                    colExistingExcess.RemoveAll(x => x.ID == id);
                    //colExistingExcess.DeleteItemById(id);
                }
                

            }
        }

        public  DataTable InitializeDataByUser(string userID)
        {
            DataTable userAccessData = new DataTable();
            string dateQuery = string.Format("{0}='{1}'", DatabaseObjects.Columns.ApplicationRoleAssign, userID);
            //ApplicationAccessManager appAccessManager = new ApplicationAccessManager(context);
            //ApplicationModuleManager appModuleManager = new ApplicationModuleManager(context);

            //List<ApplicationAccess> lstApplicationAccess = appAccessManager.Load(x => x.ApplicationRoleAssign == userID);

            DataTable wfhList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplModuleRoleRelationship,dateQuery);
           

            return userAccessData= wfhList;
        }
        public  List<ServiceData> GetExistingAccessOfUser(DataRow app, DataTable userAccessData)
        {
            ApplicationModuleManager appModuleManager = new ApplicationModuleManager(context);
            ApplicationRoleManager appRoleManager = new ApplicationRoleManager(context);
            List<ServiceData> lstServiceData = new List<ServiceData>();
            if (userAccessData == null || userAccessData.Rows.Count == 0)
                return lstServiceData;

            DataRow[] accessDataRows = new DataRow[0];
            if (app == null)
                accessDataRows = userAccessData.Select();
            else
                accessDataRows = userAccessData.Select(string.Format("{0} = {1}", DatabaseObjects.Columns.APPTitleLookup, app[DatabaseObjects.Columns.Id]));
            //accessDataRows = userAccessData.Select(string.Format("{0} like '{1};#%'", DatabaseObjects.Columns.APPTitleLookup, app[DatabaseObjects.Columns.Id]));

            if (accessDataRows.Length == 0)
                return lstServiceData;

            foreach (DataRow spItem in accessDataRows)
            {
                ServiceData serviceData = new ServiceData();
                serviceData.ID = Convert.ToString(spItem[DatabaseObjects.Columns.Id]);
                string spFieldLookupValueRoles = Convert.ToString(spItem[DatabaseObjects.Columns.ApplicationRoleLookup]);
                if (!string.IsNullOrEmpty(spFieldLookupValueRoles))
                    serviceData.RowName = appRoleManager.LoadByID(UGITUtility.StringToLong(spFieldLookupValueRoles)).Title;
                string spFieldLookupValueApp = Convert.ToString(spItem[DatabaseObjects.Columns.APPTitleLookup]);
                //serviceData.RowName = spFieldLookupValueRoles;
                if (string.IsNullOrEmpty(Convert.ToString(spItem[DatabaseObjects.Columns.ApplicationModulesLookup])))
                    serviceData.ColumnName = serviceData.RowName;
                else
                {
                    string spFieldLookupValueModules =Convert.ToString(spItem[DatabaseObjects.Columns.ApplicationModulesLookup]);
                    serviceData.ColumnName = appModuleManager.LoadByID(UGITUtility.StringToLong(spFieldLookupValueModules)).Title;
                }
                serviceData.State = 0;
                lstServiceData.Add(serviceData);
            }

            return lstServiceData;
        }
        public  List<ServiceData> GetExistingAccessOfUser( DataRow app, string userID)
        {
            List<ServiceData> lstServiceData = new List<ServiceData>();
           
            DataTable userAccessData = InitializeDataByUser(userID);
            if (userAccessData == null || userAccessData.Rows.Count == 0)
                return lstServiceData;
            lstServiceData = GetExistingAccessOfUser(app, userAccessData);
            return lstServiceData;
        }
        public int GetApplicationModuleLookup (int appTitleLookup, string Title)
        {
          DataTable dt= GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplicationModules,string.Format("{0}={1} and {2}='{3}'",DatabaseObjects.Columns.APPTitleLookup,appTitleLookup,DatabaseObjects.Columns.Title,Title));
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0][DatabaseObjects.Columns.ID]);
            }
            else
            {
                return 0;
            }
        }

    }
}
