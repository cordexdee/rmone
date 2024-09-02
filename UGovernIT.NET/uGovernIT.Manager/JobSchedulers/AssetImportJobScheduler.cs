using System;
using System.Collections.Generic;
using uGovernIT.Utility;
using uGovernIT.Util.Log;
using System.Threading.Tasks;
using DevExpress.CodeParser.Diagnostics;
using DevExpress.DocumentServices.ServiceModel.DataContracts;
using DevExpress.UnitConversion;
using uGovernIT.Web.Helpers;

namespace uGovernIT.Manager.JobSchedulers
{
    public class AssetImportJobScheduler : IJobScheduler
    {
        public string Duration { get; set; }
        public async Task Execute(string TenantID)
        {
            await Task.FromResult(0);
            ApplicationContext _context = ApplicationContext.CreateContext(TenantID);
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(_context);
            ModuleViewManager moduleViewManager = new ModuleViewManager(_context);
            if (!configurationVariableManager.GetValueAsBool(ConfigConstants.EnableAssetSyncTimerJob))
                return;
            ULog.WriteLog("*** Starting Asset Import job ...");

            UGITModule module = moduleViewManager.LoadByName(ModuleNames.CMDB);
            ImportStatus status = new ImportStatus(module.ModuleName);
            FTPHelper ftpHelp = new FTPHelper(_context, FTPFileType.Assets);
            List<string> allDownloadedFiles = ftpHelp.GetAllFiles();
            List<string> processedFiles = new List<string>();
            if (allDownloadedFiles.Count > 0)
            {
                foreach (string fileName in allDownloadedFiles)
                {
                    try
                    {
                        AssetHelper.AssetImport(_context, fileName, status, module.ModuleName);
                        processedFiles.Add(fileName);
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex, "ERROR Importing Asset file: " + fileName);
                        status.succeeded = false;
                        status.errorMessages.Add(string.Format("ERROR Importing Asset file {0}: {1}", fileName, ex.Message));
                    }
                }

                if (processedFiles.Count > 0)
                    ftpHelp.MoveAllFiles(processedFiles);

                ULog.WriteLog(string.Format("*** Asset Import job done, processed {0} file(s)", processedFiles.Count));

               if (status.succeeded)
                    uHelper.SendJobStatusEmail(_context, "Asset Import Job", status);
            }
            else
                ULog.WriteLog("*** No new asset files to process!");
        }
         
    }
}
