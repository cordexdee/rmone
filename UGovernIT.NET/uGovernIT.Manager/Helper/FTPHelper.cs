using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using uGovernIT.Manager;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Web.Helpers
{
    public class FTPHelper
    {
        FTPConfiguration ftpConfig;
        ApplicationContext context;
        FTPFileType FileType;

        public FTPHelper(ApplicationContext spWeb, FTPFileType type)
        {
            this.context = spWeb;
            this.FileType = type;
            ftpConfig = GetFTPConfiguration(type);
        }

        public FTPHelper(ApplicationContext spWeb, FTPConfiguration config)
        {
            this.context = spWeb;
            ftpConfig = config;
        }

        public List<string> GetAllFiles()
        {
            List<string> listFiles = new List<string>();
            if (ftpConfig == null)
                return listFiles;
            FtpWebRequest reqFTP = null;
            FtpWebResponse response = null;
            Stream reader = null;

            try
            {
                if (!string.IsNullOrEmpty(ftpConfig.FtpBaseUrl) && !string.IsNullOrEmpty(ftpConfig.File) && !string.IsNullOrEmpty(ftpConfig.FtpCredential))
                {
                    string[] credentials = uGovernITCrypto.Decrypt(ftpConfig.FtpCredential, Constants.UGITAPass).Split(new string[] { "," }, StringSplitOptions.None);
                    Uri ftpUrl = new Uri(ftpConfig.FtpBaseUrl);
                    var netCredentials = new NetworkCredential(credentials[0], credentials[1]);
                    if (ftpUrl.Scheme.ToLower() == "sftp") // SFTP
                    {
                        string host = ftpUrl.Host;
                        string port = ftpUrl.Port.ToString();
                        string directory = ftpConfig.FtpFolderUrl;
                        try
                        {
                            string filepath = System.Web.Hosting.HostingEnvironment.MapPath("~/Resources/FTPkey.pem");
                            var privateKey = new PrivateKeyFile(filepath);
                            SftpClient sftpClient = null;
                            if (privateKey != null)
                                sftpClient = new SftpClient(host, UGITUtility.StringToInt(port), credentials[0], new[] { privateKey });
                            else
                                sftpClient = new SftpClient(host, UGITUtility.StringToInt(port), credentials[0], credentials[1]);

                            using (var sftp = sftpClient)
                            {
                                sftp.Connect();
                                string path = string.Format("{0}/", directory);
                                bool sftpFilesFound = sftp.Exists(path);
                                if (sftpFilesFound)
                                {
                                    List<SftpFile> allRemoteFile = sftp.ListDirectory(path).ToList().Where(x => x.Name.ToLower().Contains(ftpConfig.File.ToLower()) && !x.Name.ToLower().StartsWith("processed_")).ToList();
                                    foreach (SftpFile sfile in allRemoteFile)
                                    {
                                        try
                                        {
                                            DownloadFile(sftp, sfile, uHelper.GetTempFolderPathNew());
                                            listFiles.Add(Path.Combine(uHelper.GetTempFolderPathNew(), sfile.Name));
                                        }
                                        catch (Exception ex)
                                        {
                                            ULog.WriteException(ex, "ERROR Downloading FTP file");
                                        }
                                    }
                                }
                                else
                                {
                                    ULog.WriteLog(string.Format("ERROR: FTP File not found - {0}:{1} {2}", host, port, path));
                                }
                                sftp.Disconnect();
                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex, "ERROR Downloading file(s) via SFTP");
                        }

                    }
                    else // Plain FTP & FTPS
                    {
                        try
                        {
                            string url = string.Format("{0}//", ftpConfig.FtpBaseUrl);
                            reqFTP = FTPRequestObject(url, netCredentials, WebRequestMethods.Ftp.ListDirectory);
                            response = (FtpWebResponse)reqFTP.GetResponse();
                            reader = response.GetResponseStream();
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex, "ERROR Connecting to FTP/FTPS server");
                        }

                        if (reader != null)
                        {
                            StreamReader sreader = new StreamReader(reader);
                            var allfileExist = sreader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                            //Check archive folder, if not exist then create it
                            List<string> fileExist = allfileExist.Where(x => x.ToLower() == ftpConfig.FtpArchiveFolderUrl.ToLower()).ToList();
                            if (fileExist == null || fileExist.Count <= 0)
                            {
                                try
                                {
                                    string url = string.Format("{0}//{1}", ftpConfig.FtpBaseUrl, ftpConfig.FtpArchiveFolderUrl);
                                    reqFTP = FTPRequestObject(url, netCredentials, WebRequestMethods.Ftp.MakeDirectory);
                                    response = (FtpWebResponse)reqFTP.GetResponse();
                                    bool Success = response.StatusCode == FtpStatusCode.CommandOK || response.StatusCode == FtpStatusCode.FileActionOK;
                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex, "ERROR creating/checking Archive folder");
                                }
                            }

                            //Download All file based on ftpConfig.File
                            List<string> allFiles = allfileExist.Where(x => x.ToLower().Contains(ftpConfig.File.ToLower()) && !x.ToLower().StartsWith("processed_")).ToList();
                            if (allFiles.Count > 0)
                            {
                                string localPath = uHelper.GetTempFolderPath();
                                foreach (string fileName in allFiles)
                                {
                                    using (WebClient request = new WebClient())
                                    {
                                        request.Credentials = netCredentials;
                                        try
                                        {
                                            string localpath = localPath + @"\" + fileName;
                                            request.DownloadFile(string.Format("{0}//{1}", ftpConfig.FtpBaseUrl, fileName), localpath);
                                            listFiles.Add(localpath);
                                        }
                                        catch (Exception ex)
                                        {
                                            ULog.WriteException(ex, "ERROR Downloading FTP file");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    ULog.WriteLog("FTP Credentials properties are missing so kindly check it");
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "-- Error in Downloading File To Import FtpHelpers:GetAllFiles(string weburl) ");
            }

            return listFiles;
        }

        /// <summary>
        /// Moving all processed file to archive folder
        /// </summary>
        /// <param name="files"></param>
        public void MoveAllFiles(List<string> files)
        {
            if (ftpConfig == null || files == null || files.Count == 0)
                return;
            FtpWebRequest reqFTP = null;
            FtpWebResponse response = null;
            try
            {
                string ftpBaseUrl = ftpConfig.FtpBaseUrl;
                string userFile = ftpConfig.File;
                string ftpCredential = ftpConfig.FtpCredential;
                string ftpFolderUrl = ftpConfig.FtpFolderUrl;
                string ftpArchiveFolderUrl = ftpConfig.FtpArchiveFolderUrl;

                if (!string.IsNullOrEmpty(ftpBaseUrl) && !string.IsNullOrEmpty(userFile) && !string.IsNullOrEmpty(ftpCredential))
                {
                    ftpCredential = uGovernITCrypto.Decrypt(ftpCredential, Constants.UGITAPass);
                    string[] credentials = ftpCredential.Split(new string[] { "," }, StringSplitOptions.None);
                    Uri ftpUrl = new Uri(ftpBaseUrl);
                    if (ftpUrl.Scheme.ToLower() == "sftp") // SFTP
                    {
                        string host = ftpUrl.Host;
                        string port = ftpUrl.Port.ToString();
                        string directory = ftpFolderUrl;
                        string filepath = System.Web.Hosting.HostingEnvironment.MapPath("~/Resources/FTPkey.pem");
                        var privateKey = new PrivateKeyFile(filepath);
                        SftpClient sftpClient = null;
                        if (privateKey != null)
                            sftpClient = new SftpClient(host, UGITUtility.StringToInt(port), credentials[0], new[] { privateKey });
                        else
                            sftpClient = new SftpClient(host, UGITUtility.StringToInt(port), credentials[0], credentials[1]);
                        using (var sftp = sftpClient)
                        {
                            sftp.Connect();
                            string path = string.Format("{0}/", directory);
                            bool sftpFilesFound = sftp.Exists(path);
                            if (sftpFilesFound)
                            {
                                List<SftpFile> allRemoteFile = sftp.ListDirectory(path).ToList();
                                foreach (SftpFile sfile in allRemoteFile)
                                {
                                    if (files.Any(x => Path.GetFileName(x.ToLower()) == sfile.Name.ToLower()))
                                    {
                                        try
                                        {

                                            if (!string.IsNullOrEmpty(ftpArchiveFolderUrl))
                                                sfile.MoveTo(sfile.FullName.Replace(sfile.Name, ftpArchiveFolderUrl + "/" + sfile.Name));
                                            else
                                                sftp.RenameFile(sfile.FullName, "Processed_" + sfile.FullName);
                                        }
                                        catch (Exception ex)
                                        {
                                            ULog.WriteException(ex);
                                            sftp.Disconnect();
                                            sftp.Connect();
                                            sftp.RenameFile(sfile.FullName, sfile.FullName.Replace(sfile.Name, "Processed_" + sfile.Name));
                                        }
                                    }
                                }
                                sftp.Disconnect();
                            }
                            else
                            {
                                ULog.WriteLog(string.Format("ERROR: FTP File not found - {0}:{1} {2}", host, port, path));
                            }
                        }
                    }
                    else // Plain FTP && FTPS
                    {
                        var netCredentials = new NetworkCredential(credentials[0], credentials[1]);
                        foreach (string filename in files)
                        {
                            string fileName = Path.GetFileName(filename);
                            string renameurl = string.Format("{0}//{1}", ftpBaseUrl, fileName);

                            try
                            {
                                //Move file using Rename Command as well
                                reqFTP = FTPRequestObject(renameurl, netCredentials, WebRequestMethods.Ftp.Rename);
                                if (!string.IsNullOrEmpty(ftpArchiveFolderUrl))
                                    reqFTP.RenameTo = ftpArchiveFolderUrl + "\\" + fileName;
                                else
                                    reqFTP.RenameTo = "Processed_" + fileName;
                                response = (FtpWebResponse)reqFTP.GetResponse();
                                bool success = response.StatusCode == FtpStatusCode.CommandOK || response.StatusCode == FtpStatusCode.FileActionOK;
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex);
                                try
                                {
                                    reqFTP = FTPRequestObject(renameurl, netCredentials, WebRequestMethods.Ftp.Rename);
                                    reqFTP.RenameTo = "Processed_" + fileName;
                                    response = (FtpWebResponse)reqFTP.GetResponse();
                                    bool success = response.StatusCode == FtpStatusCode.CommandOK || response.StatusCode == FtpStatusCode.FileActionOK;
                                }
                                catch (Exception wex)
                                {
                                    ULog.WriteException(wex, "Unable to Rename File");
                                }
                            }
                        }
                    }
                }
                else
                {
                    ULog.WriteLog("FTP Credentials property are missing so kindly check it");
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "-- Error in Moving file on FTPHelpers:MoveAllFiles(string weburl) ");
            }
        }

        /// <summary>
        /// Create FTPWebRequest Object
        /// </summary>
        /// <param name="url">ftp ip or ftp address with eg. ftp://12.12.1.122 </param>
        /// <param name="netCredentials">User id, Password</param>
        /// <param name="method">WebRequestMethods.Ftp.Rename or WebRequestMethods.Ftp.ListDirectory, WebRequestMethods.Ftp.XXXXX</param>        
        /// <returns></returns>
        private static FtpWebRequest FTPRequestObject(string url, NetworkCredential netCredentials, string method)
        {
            Uri ftpUrl = new Uri(url);
            string link = url.Replace("ftps:", "ftp:");
            FtpWebRequest reqFTP = WebRequest.Create(new Uri(link)) as FtpWebRequest;
            reqFTP.Credentials = netCredentials;
            reqFTP.Method = method;
            if (ftpUrl.Scheme.ToLower() == "ftps")
            {
                reqFTP.UsePassive = true;
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                reqFTP.EnableSsl = true;
                ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;
            }
            return reqFTP;
        }

        /// <summary>
        /// Get FTP Configuration
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="filetype"></param>
        /// <returns></returns>
        private FTPConfiguration GetFTPConfiguration(FTPFileType filetype)
        {
            if (context == null)
                return null;

            FTPConfiguration ftpConfig = new FTPConfiguration();
            ConfigurationVariableManager configuration = new ConfigurationVariableManager(context);
            ftpConfig.FtpBaseUrl = configuration.GetValue("FTPBaseUrl");
            ftpConfig.File = string.Empty;

            if (filetype == FTPFileType.Assets)
                ftpConfig.File = configuration.GetValue("AssetFile");
            else if (filetype == FTPFileType.Departments)
                ftpConfig.File = configuration.GetValue("DepartmentFile");
            else if (filetype == FTPFileType.Users)
                ftpConfig.File = configuration.GetValue("UserFile");

            ftpConfig.FtpCredential = configuration.GetValue("FTPCredential");
            ftpConfig.FtpFolderUrl = configuration.GetValue("FTPFolderUrl");
            ftpConfig.FtpArchiveFolderUrl = configuration.GetValue("FTPArchiveFolderUrl");

            return ftpConfig;
        }

        private void DownloadFile(SftpClient client, SftpFile file, string directory)
        {
            using (Stream fileStream = File.OpenWrite(Path.Combine(directory, file.Name)))
            {
                client.DownloadFile(file.FullName, fileStream);
            }
        }

        public string UploadFiles(List<string> files)
        {
            if (ftpConfig == null || files == null || files.Count == 0)
                return "No FTP details";


            string errorMessage = string.Empty;

            FtpWebRequest reqFTP = null;
            try
            {
                string ftpBaseUrl = ftpConfig.FtpBaseUrl;
                string userFile = ftpConfig.File;
                string ftpCredential = ftpConfig.FtpCredential;
                string ftpFolderUrl = ftpConfig.FtpFolderUrl;
                string ftpArchiveFolderUrl = ftpConfig.FtpArchiveFolderUrl;

                if (!string.IsNullOrEmpty(ftpBaseUrl) && !string.IsNullOrEmpty(ftpCredential))
                {
                    ftpCredential = uGovernITCrypto.Decrypt(ftpCredential, Constants.UGITAPass);
                    string[] credentials = ftpCredential.Split(new string[] { "," }, StringSplitOptions.None);
                    Uri ftpUrl = new Uri(ftpBaseUrl);
                    if (ftpUrl.Scheme.ToLower() == "sftp") // SFTP
                    {
                        string host = ftpUrl.Host;
                        string port = ftpUrl.Port > 0 ? ftpUrl.Port.ToString() : "22"; // Use default port 22 if not specified
                        string directory = ftpFolderUrl;
                        string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Resources/FTPkey.pem");
                        //var privateKey = new PrivateKeyFile(@"D:\tta-newkey.pem");
                        var privateKey = new PrivateKeyFile(path);
                        SftpClient sftpClient = null;
                        if (privateKey!=null)
                            sftpClient  = new SftpClient(host, UGITUtility.StringToInt(port), credentials[0], new[] { privateKey });
                        else
                            sftpClient = new SftpClient(host, UGITUtility.StringToInt(port), credentials[0], credentials[1]);
                        
                        using (var sftp = sftpClient)
                        {
                            sftp.Connect();
                            foreach (string file in files)
                            {
                                try
                                {
                                    using (var fileStream = new FileStream(file, FileMode.Open))
                                    {
                                        sftp.BufferSize = 4 * 1024; // bypass Payload error large files
                                        sftp.UploadFile(fileStream, Path.GetFileName(file));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex);
                                    errorMessage = ex.Message;
                                }
                            }
                        }
                    }
                    else // Plain FTP && FTPS
                    {
                        var netCredentials = new NetworkCredential(credentials[0], credentials[1]);
                        foreach (string file in files)
                        {
                            string fileName = Path.GetFileName(file);
                            string renameurl = string.Format("{0}//{1}", ftpBaseUrl, fileName);

                            try
                            {
                                reqFTP = FTPRequestObject(renameurl, netCredentials, WebRequestMethods.Ftp.UploadFile);

                                FileStream fs = File.OpenRead(file);
                                byte[] buffer = new byte[fs.Length];
                                fs.Read(buffer, 0, buffer.Length);
                                fs.Close();

                                Stream ftpstream = reqFTP.GetRequestStream();
                                ftpstream.Write(buffer, 0, buffer.Length);
                                ftpstream.Close();
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex);
                                errorMessage = ex.Message;
                            }
                        }
                    }
                }
                else
                {
                    ULog.WriteLog("FTP Credentials property are missing so kindly check it");
                    errorMessage = "FTP Credentials property are missing so kindly check it";
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "-- Error in upload file on FTPHelpers:UploadFiles(string weburl) ");
                errorMessage = "-- Error in upload file on FTPHelpers:UploadFiles(string weburl)";
            }

            return errorMessage;
        }
    }
}
