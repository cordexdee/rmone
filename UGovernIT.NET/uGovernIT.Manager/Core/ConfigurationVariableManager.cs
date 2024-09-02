using System;
using System.Linq;
using System.Xml;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;
using System.Collections.Generic;
using uGovernIT.Util.Cache;

namespace uGovernIT.Manager
{
    public interface IConfigurationVariableHelper : IManagerBase<ConfigurationVariable>
    {
    }

    public class ConfigurationVariableManager : ManagerBase<ConfigurationVariable>, IConfigurationVariableHelper
    {
        public ConfigurationVariableManager(ApplicationContext context) : base(context)
        {
            store = new ConfigurationVariableStore(this.dbContext);
        }

        public override bool Update(ConfigurationVariable item)
        {
            CacheHelper<ConfigurationVariable>.AddOrUpdate(item.KeyName, dbContext.TenantID, item);
            return base.Update(item);
        }

        public override long Insert(ConfigurationVariable item)
        {
            long id = base.Insert(item);
            CacheHelper<ConfigurationVariable>.AddOrUpdate(item.KeyName, dbContext.TenantID, item);
            return id;
        }

        public override bool Delete(ConfigurationVariable item)
        {
            CacheHelper<ConfigurationVariable>.Delete(item.KeyName, dbContext.TenantID);
            return base.Delete(item);
        }

        public ConfigurationVariable Save(string key, string value)
        {
            ConfigurationVariable cvariable = this.Get(x => x.KeyName == key);
            if (cvariable != null)
            {
                cvariable.KeyValue = value;
                store.Update(cvariable);
            }
            else
            {
                cvariable = new ConfigurationVariable();
                cvariable.KeyName = key;
                cvariable.KeyValue = value;

                Insert(cvariable);
            }
            return cvariable;
        }

        public ConfigurationVariable LoadVaribale(string Key)
        {
            ConfigurationVariable configVariable = new ConfigurationVariable();
            configVariable = GetValue(Key, true);
            return configVariable;
        }

        /// <summary>
        /// Get config variable value
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        /// 
        public string GetValue(string keyName)
        {
            ConfigurationVariable item = CacheHelper<ConfigurationVariable>.Get(keyName, this.dbContext.TenantID);
            if (item == null)
            {
                item = store.Get(x => x.KeyName == keyName);
                if (item != null)
                {
                    CacheHelper<ConfigurationVariable>.AddOrUpdate(keyName, this.dbContext.TenantID, item);
                }
                else
                {
                    ConfigurationVariable eItem = new ConfigurationVariable();
                    eItem.KeyValue = keyName;
                    eItem.KeyValue = string.Empty;
                    CacheHelper<ConfigurationVariable>.AddOrUpdate(keyName, this.dbContext.TenantID, eItem);
                }
            }

            if (item != null)
            {
                return item.KeyValue;
            }
            return string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public ConfigurationVariable GetValue(string keyName, bool flag)
        {
            ConfigurationVariable item = CacheHelper<ConfigurationVariable>.Get(keyName, this.dbContext.TenantID);
            if (item == null)
            {
                item = store.Get(x => x.KeyName == keyName);
                if (item != null)
                {
                    CacheHelper<ConfigurationVariable>.AddOrUpdate(keyName, this.dbContext.TenantID, item);
                }
                else
                {
                    ConfigurationVariable eItem = new ConfigurationVariable();
                    eItem.KeyValue = keyName;
                    eItem.KeyValue = string.Empty;
                    CacheHelper<ConfigurationVariable>.AddOrUpdate(keyName, this.dbContext.TenantID, eItem);
                }
            }

            if (item != null)
            {
                return item;
            }
            return null;
        }

        public object GetValueAsClassObj(string keyName, Type type)
        {
            ConfigurationVariable configVariable = new ConfigurationVariable();
            configVariable = GetValue(keyName, true);
            if (configVariable != null)
            {
                try
                {
                    XmlDocument xmlDocCtnt = new XmlDocument();
                    xmlDocCtnt.LoadXml(configVariable.KeyValue);
                    object obj = (SmtpConfiguration)uHelper.DeSerializeAnObject(xmlDocCtnt, type);
                    return obj;
                }
                catch (Exception ex)
                {
                    Util.Log.ULog.WriteException(ex);
                }
            }
            return string.Empty;
        }

        public bool GetValueAsBool(string keyName)
        {
            return UGITUtility.StringToBoolean(GetValue(keyName));
        }

        public string GetValue(string key, string defaultValue = "")
        {
            string value = GetValue(key);

            if (string.IsNullOrEmpty(value))
                return defaultValue;
            else
                return value;
        }

        public void RefreshCache()
        {
            List<ConfigurationVariable> lstConfigurationVariable = Load();
            if (lstConfigurationVariable == null || lstConfigurationVariable.Count == 0)
                return;

            foreach (ConfigurationVariable cVariable in lstConfigurationVariable)
            {
                if (string.IsNullOrEmpty(cVariable.KeyName))
                    continue;

                CacheHelper<ConfigurationVariable>.AddOrUpdate(cVariable.KeyName, this.dbContext.TenantID, cVariable);
            }
        }
    }
}
