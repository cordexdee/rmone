using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uGovernIT.Web.Helpers
{
    public class FTPConfiguration
    {
        public string FtpBaseUrl { get; set; }
        public string File { get; set; }
        public string FtpCredential { get; set; }
        public string FtpFolderUrl { get; set; }
        public string FtpArchiveFolderUrl { get; set; }
    }
    public enum FTPFileType
    {
        Assets, Departments, Users
    }
}