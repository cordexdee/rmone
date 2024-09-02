using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using System.IO;
using uGovernIT.Manager.Helper;
using uGovernIT.Util.Log;

namespace uGovernIT.Manager.Managers
{
    public class UploadManager
    {
        ApplicationContext context = null;
        ConfigurationVariableManager configManager = null;

        public UploadManager(ApplicationContext web)
        {
            context = web;
            configManager = new ConfigurationVariableManager(web);
        }

        /// <summary>
        /// This method is used to upload the file into temp folder
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public string UploadFile(FileStream fileStream)
        {
            string uploadedFileName = string.Empty;

            try
            {
                string uploadedFilePath = uHelper.GetTempFolderPath();
                string fileType = Path.GetExtension(fileStream.Name);
                string fileName = Path.GetFileName(fileStream.Name);
                uploadedFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + fileName;
                int size = Convert.ToInt32(fileStream.Length);
                byte[] fileBytes = new byte[size];
                fileStream.Read(fileBytes, 0, size);
                fileStream.Close();

                uploadedFilePath = uploadedFilePath + uploadedFileName;

                File.WriteAllBytes(uploadedFilePath, fileBytes);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return uploadedFileName;
        }
    }
}
