using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DataTransfer.Infratructure;
using JsonConfig;
using ClientOM = Microsoft.SharePoint.Client;
using uGovernIT.Manager;
using uGovernIT.DAL.Infratructure;
using uGovernIT.Utility;

using uGovernIT.Util.Log;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity;
using uGovernIT.DataTransfer.SharePointToDotNet;
using uGovernIT.DataTransfer.DotNetToDotNet;
using System.IO;
using Newtonsoft.Json.Linq;
using uGovernIT.Util.Cache;
using uGovernIT.DAL;

namespace uGovernIT.DataTransfer
{
    public class ImportManager
    {
        bool SelfRegisteredTenant = false;
        string configPath;
        ImportContext importContext;
        public ImportManager(string selectedConfig, string selectedModules, DMTenant sourceTenant, DMTenant targetTenant, DMTenant spSource)
        {
            if (!File.Exists(Path.Combine(uHelper.GetDataMigrationTemplate())))
                return;

            if (string.IsNullOrEmpty(selectedConfig) && string.IsNullOrEmpty(selectedModules))
                return;

            SelfRegisteredTenant = targetTenant.SelfRegisteredTenant.HasValue ? targetTenant.SelfRegisteredTenant.Value : false;
            configPath = Path.Combine(uHelper.GetDataMigrationTemplate());
            FileInfo configFileInfo = new FileInfo(configPath);
            string json = configFileInfo.OpenText().ReadToEnd();

            JObject jo = JObject.Parse(json);
            JToken jToken = JToken.Parse(json);
            var result = jToken["modules"].Select(x => x).ToList();

            List<string> lstModulesList = jToken["modules"].Select(x => x).Select(x => (string)x["name"]).Distinct().ToList();
            List<string> lstSelectedModules = selectedModules.Split(new string[] { Utility.Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (var module in lstModulesList.Where(x => !lstSelectedModules.Contains(x)))
            {
                foreach (var item in result)
                {
                    var name = item.SelectTokens("name").Values().ToList();
                    if (name.Where(x => x.ToString().Trim() == module).Any())
                    {
                        item.Remove();
                    }
                }
            }

            List<string> lstConfig = jToken["commonsettings"]["configurations"].Select(x => (string)x).ToList();
            List<string> lstSelectedConfig = selectedConfig.Split(new string[] { Utility.Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries).ToList();
            result = jToken["commonsettings"].Select(x => x).ToList();
            foreach (var config in lstConfig.Where(x => !lstSelectedConfig.Contains(x)))
            {
                foreach (var item in result)
                {
                    var name = item.SelectMany(x => x).ToList();
                    if (name.Where(x => x.ToString().Trim() == config).Any())
                    {
                        name.Where(x => x.ToString().Trim() == config).FirstOrDefault().Remove();
                    }
                }
            }

            jToken["permission"]["source"]["tenantid"] = sourceTenant.tenantid;
            jToken["permission"]["source"]["dbconnection"] = sourceTenant.dbconnection;
            jToken["permission"]["source"]["commondbconnection"] = sourceTenant.commondbconnection;

            jToken["permission"]["target"]["tenantname"] = targetTenant.tenantname;
            jToken["permission"]["target"]["tenantid"] = targetTenant.tenantid;
            jToken["permission"]["target"]["dbconnection"] = targetTenant.dbconnection;
            jToken["permission"]["target"]["commondbconnection"] = targetTenant.commondbconnection;

            jToken["permission"]["target"]["tenanturl"] = !string.IsNullOrEmpty(targetTenant.tenanturl) ? targetTenant.tenanturl : $"www.{targetTenant.tenantname}.com";
            jToken["permission"]["target"]["email"] = targetTenant.TenantEmail;
            jToken["permission"]["target"]["contact"] = targetTenant.contact;
            jToken["permission"]["target"]["title"] = targetTenant.title;

            jToken["permission"]["target"]["defaultuser"]["username"] = targetTenant.username;
            jToken["permission"]["target"]["defaultuser"]["password"] = targetTenant.password;
            jToken["permission"]["target"]["defaultuser"]["email"] = targetTenant.userEmail;

            jToken["permission"]["spsource"]["url"] = spSource.url;
            jToken["permission"]["spsource"]["username"] = spSource.username;
            jToken["permission"]["spsource"]["password"] = spSource.password;
            jToken["permission"]["spsource"]["domain"] = spSource.domain;

            json = jToken.ToString(Newtonsoft.Json.Formatting.Indented);

            JsonConfig.Config.SetDefaultConfig(JsonConfig.Config.ParseJson(json));


        }
        public ImportManager(string selectedConfig, string selectedModules, DMTenant sourceTenant, DMTenant targetTenant)
        {
            if (!File.Exists(Path.Combine(uHelper.GetDataMigrationTemplate())))
                return;

            if (string.IsNullOrEmpty(selectedConfig) && string.IsNullOrEmpty(selectedModules))
                return;

            SelfRegisteredTenant = targetTenant.SelfRegisteredTenant.HasValue ? targetTenant.SelfRegisteredTenant.Value : false;
            configPath = Path.Combine(uHelper.GetDataMigrationTemplate());
            FileInfo configFileInfo = new FileInfo(configPath);
            string json = configFileInfo.OpenText().ReadToEnd();

            JObject jo = JObject.Parse(json);
            JToken jToken = JToken.Parse(json);
            var result = jToken["modules"].Select(x => x).ToList();

            List<string> lstModulesList = jToken["modules"].Select(x => x).Select(x => (string)x["name"]).Distinct().ToList();
            List<string> lstSelectedModules = new List<string>();
            if (!string.IsNullOrEmpty(selectedModules))
                lstSelectedModules = selectedModules.Split(new string[] { Utility.Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (var module in lstModulesList.Where(x => !lstSelectedModules.Contains(x)))
            {
                foreach (var item in result)
                {
                    var name = item.SelectTokens("name").Values().ToList();
                    if (name.Where(x => x.ToString().Trim() == module).Any())
                    {
                        item.Remove();
                    }
                }
            }

            List<string> lstConfig = jToken["commonsettings"]["configurations"].Select(x => (string)x).ToList();
            List<string> lstSelectedConfig = new List<string>();
            if (!string.IsNullOrEmpty(selectedConfig))
                lstSelectedConfig = selectedConfig.Split(new string[] { Utility.Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries).ToList();

            result = jToken["commonsettings"].Select(x => x).ToList();
            foreach (var config in lstConfig.Where(x => !lstSelectedConfig.Contains(x)))
            {
                foreach (var item in result)
                {
                    var name = item.SelectMany(x => x).ToList();
                    if (name.Where(x => x.ToString().Trim() == config).Any())
                    {
                        name.Where(x => x.ToString().Trim() == config).FirstOrDefault().Remove();
                    }
                }
            }

            jToken["permission"]["source"]["tenantid"] = sourceTenant.tenantid;
            jToken["permission"]["source"]["dbconnection"] = sourceTenant.dbconnection;
            jToken["permission"]["source"]["commondbconnection"] = sourceTenant.commondbconnection;

            jToken["permission"]["target"]["tenantname"] = targetTenant.tenantname;
            jToken["permission"]["target"]["tenantid"] = targetTenant.tenantid;
            jToken["permission"]["target"]["dbconnection"] = targetTenant.dbconnection;
            jToken["permission"]["target"]["commondbconnection"] = targetTenant.commondbconnection;

            jToken["permission"]["target"]["tenanturl"] = !string.IsNullOrEmpty(targetTenant.url) ? targetTenant.url : $"www.{targetTenant.tenantname}.com";
            jToken["permission"]["target"]["email"] = targetTenant.TenantEmail;
            jToken["permission"]["target"]["contact"] = targetTenant.contact;
            jToken["permission"]["target"]["title"] = targetTenant.title;

            jToken["permission"]["target"]["defaultuser"]["username"] = targetTenant.username;
            jToken["permission"]["target"]["defaultuser"]["password"] = targetTenant.password;
            jToken["permission"]["target"]["defaultuser"]["email"] = targetTenant.userEmail;

            json = jToken.ToString(Newtonsoft.Json.Formatting.Indented);

            JsonConfig.Config.SetDefaultConfig(JsonConfig.Config.ParseJson(json));


        }

        public ImportManager(string configFile)
        {
            configPath = configFile;

            using (StreamReader sr = new StreamReader(configPath))
            {
                string json = sr.ReadToEnd();
                JsonConfig.Config.SetDefaultConfig(JsonConfig.Config.ParseJson(json));
            }
            //FileInfo configFileInfo = new FileInfo(configPath);
            //JsonConfig.Config.SetDefaultConfig(JsonConfig.Config.ParseJson(configFileInfo.OpenText().ReadToEnd()));
        }


        public void Excute(string cmdName, bool importdata = true)
        {

            ULog.WriteLog("Create SPImportManager instance and load configuration");
            if (cmdName == "dtd")
                importContext = new DNImportContext();
            else if (cmdName == "std")
                importContext = new SPImportContext();

            Initialize();
            //dtd stand for .Net to .Net
            if (cmdName == "dtd")
            {
                DNImportContext dnContext = (DNImportContext)importContext;
                dnContext.SourceAppContext = ApplicationContext.CreateContext(JsonConfig.Config.Global.permission.source.dbconnection, JsonConfig.Config.Global.permission.source.commondbconnection, JsonConfig.Config.Global.permission.source.tenantid);

                DNCommanImport spImport = new DNCommanImport(dnContext);
                spImport.Excecute();
                foreach (var module in JsonConfig.Config.Global.modules)
                {
                    if(!importdata)
                        module["importdata"] = importdata;
                    DNModuleImport moduleImport = new DNModuleImport(dnContext, module.name);
                    moduleImport.ImportConfiguration();
                    if (module.importdata)
                        moduleImport.Importdata();
                }


                try
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("@TenantID", dnContext.AppContext.TenantID);
                    values.Add("@AccountId", dnContext.AppContext.TenantAccountId);
                    values.Add("@ResetSequence",true);
                    uGITDAL.ExecuteDataSetWithParameters("usp_TenantDefaultConfiguration", values);
                }
                catch (Exception ex)
                {
                    ULog.WriteLog($"Exception in stored Procedure usp_TenantDefaultConfiguration: {ex.Message}");
                    ULog.WriteException(ex, $"Exception in stored Procedure usp_TenantDefaultConfiguration: {ex.Message}");
                }

                ConfigurationVariableManager cvManager = new ConfigurationVariableManager(dnContext.AppContext);
                cvManager.RefreshCache();
            }
            //std stand for Sharepoint to .Net
            else if (cmdName == "std")
            {

                SPImportContext spContext = (SPImportContext)importContext;

                SiteAuthentication spSite = new SiteAuthentication();
                spSite.SiteUrl = JsonConfig.Config.Global.permission.spsource.url;
                spSite.Domain = JsonConfig.Config.Global.permission.spsource.domain;
                spSite.UserName = JsonConfig.Config.Global.permission.spsource.username;
                spSite.Password = JsonConfig.Config.Global.permission.spsource.password;
                spContext.SPSite = spSite;

                SPCommanImport spImport = new SPCommanImport(spContext);
                spImport.Excecute();
                foreach (var module in JsonConfig.Config.Global.modules)
                {
                    SPModuleImport moduleImport = new SPModuleImport(spContext, module.name);
                    moduleImport.ImportConfiguration();
                    if (module.importdata)
                        moduleImport.Importdata();
                    Dictionary<string, object> _values = new Dictionary<string, object>();
                    _values.Add("@TenantID", spContext.AppContext.TenantID);
                    _values.Add("@AccountId", spContext.AppContext.TenantAccountId);
                    _values.Add("@Modulename", module.name);
                    uGITDAL.ExecuteDataSetWithParameters("Usp_setmoduledefaultcofiguration", _values);
                }
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@TenantID", spContext.AppContext.TenantID);
                values.Add("@AccountId", spContext.AppContext.TenantAccountId);
                uGITDAL.ExecuteDataSetWithParameters("Usp_setdefaultcofiguration", values);
                spImport.ExcecuteFinalMethods();
                spImport.Excecute_SPSpecificlMethods();
                ConfigurationVariableManager cvManager = new ConfigurationVariableManager(spContext.AppContext);
                cvManager.RefreshCache();
                ULog.WriteLog($"All data has been imported sucessfully!!");
            }
        }

        private void Initialize()
        {
            importContext.TenantName = JsonConfig.Config.Global.permission.target.tenantname;
            importContext.TenantAccountID = JsonConfig.Config.Global.permission.target.tenantid;

            CommonDatabaseContext commContext = new CommonDatabaseContext(new DAL.CustomDbContext(JsonConfig.Config.Global.permission.target.commondbconnection, JsonConfig.Config.Global.permission.target.commondbconnection));
            importContext.CommContext = commContext;

            importContext.AppContext = ApplicationContext.CreateContext(JsonConfig.Config.Global.permission.target.dbconnection, JsonConfig.Config.Global.permission.target.commondbconnection, JsonConfig.Config.Global.permission.target.tenantid);

            Tenant tenant = importContext.AppContext.ConfigManager.GetTenant(JsonConfig.Config.Global.permission.target.tenantid);
            if (tenant != null)
            {
                importContext.Tenant = tenant;
                ULog.WriteLog($"Tenant {importContext.TenantAccountID} is already found");
            }

            if (tenant == null)
            {
                ULog.WriteLog($"Existing tenant not found so create new tenant with tenant id {importContext.TenantAccountID}");
                tenant = CreateTenant();
                importContext.Tenant = tenant;
                importContext.AppContext = ApplicationContext.CreateContext(JsonConfig.Config.Global.permission.target.dbconnection, JsonConfig.Config.Global.permission.target.commondbconnection, JsonConfig.Config.Global.permission.target.tenantid);
            }

            importContext.TenantName = importContext.Tenant.TenantName;
            importContext.TenantAccountID = importContext.Tenant.AccountID;
            importContext.Tenant.TenantID = importContext.Tenant.TenantID;
        }

        private Tenant CreateTenant()
        {
            Guid _guid = Guid.NewGuid();
            string tenantName = JsonConfig.Config.Global.permission.target.tenantname;
            string tenantID = JsonConfig.Config.Global.permission.target.tenantid;

            var tenant = new Tenant()
            {
                TenantID = _guid,
                TenantName = !string.IsNullOrEmpty(tenantName) ? tenantName : tenantID,
                AccountID = tenantID,
                Country = "India",
                TenantUrl = JsonConfig.Config.Global.permission.target.tenanturl,
                DBName = string.Empty,
                DBServer = string.Empty,
                Deleted = false,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                CreatedByUser = _guid.ToString(),
                ModifiedByUser = _guid.ToString(),
                IsOffice365Subscription = false,
                Email = JsonConfig.Config.Global.permission.target.email,
                SelfRegisteredTenant = SelfRegisteredTenant,
                Name = tenantName,
                Contact = JsonConfig.Config.Global.permission.target.contact,
                Title = JsonConfig.Config.Global.permission.target.title
            };

            importContext.CommContext.Tenants.Add(tenant);
            importContext.CommContext.SaveChanges();

            List<Tenant> tenants = (List<Tenant>)Util.Cache.CacheHelper<object>.Get(string.Format("Available_Tenants"));
            tenants.Add(tenant);
            CacheHelper<object>.AddOrUpdate("Available_Tenants", tenants);

            return tenant;
        }
    }
}
