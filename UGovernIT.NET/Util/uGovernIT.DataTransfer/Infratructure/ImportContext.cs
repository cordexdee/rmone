using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Infratructure;
using uGovernIT.DataTransfer.Infratructure;
using uGovernIT.DataTransfer.SharePointToDotNet;
using uGovernIT.Manager;
using uGovernIT.Utility;
using ClientOM = Microsoft.SharePoint.Client;

namespace uGovernIT.DataTransfer.Infratructure
{
    public class ImportContext
    {
        public string ConfigPath { get; set; }
        public CommonDatabaseContext CommContext { get; set; }
        public string TenantName { get; set; }
        public string TenantAccountID { get; set; }
        public Tenant Tenant { get; set; }
        public ApplicationContext AppContext { get; set; }
        private Dictionary<string, MappedItemList> supportedLists;

        public ImportContext()
        {
            supportedLists = new Dictionary<string, MappedItemList>();
        }

        public MappedItemList GetMappedList(string name)
        {
            if (supportedLists == null || !supportedLists.ContainsKey(name))
            {
                if (supportedLists == null)
                    supportedLists = new Dictionary<string, MappedItemList>();

                MappedItemList mappedlist = new MappedItemList(name);
                supportedLists.Add(name, mappedlist);

                return mappedlist;
            }

            return supportedLists[name];
        }

        public bool IsImportEnable(string importName, string moduleName = null)
        {
            bool importEnable = false;

            if (string.IsNullOrWhiteSpace(moduleName))
            {
                if (JsonConfig.Config.Global.commonsettings.importconfiguration)
                {
                    if (JsonConfig.Config.Global.commonsettings.configurations == null)
                    {
                        importEnable = false;
                    }
                    else
                    {
                        foreach (var item in JsonConfig.Config.Global.commonsettings.configurations)
                        {
                            if (item.ToLower() == importName.ToLower())
                            {
                                importEnable = true;
                                break;
                            }
                        }
                    }
                }

            }
            else
            {
                if (JsonConfig.Config.Global.modules != null)
                {
                    foreach (var module in JsonConfig.Config.Global.modules)
                    {
                        if (module.name.ToLower() == moduleName.ToLower())
                        {
                            if (module.importconfiguration)
                            {
                                if (module.configurations == null)
                                {
                                    importEnable = false;
                                }
                                else
                                {
                                    foreach (var item in module.configurations)
                                    {
                                        if (item.ToLower() == importName.ToLower())
                                        {
                                            importEnable = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }

            return importEnable;
        }
      
    }
}
