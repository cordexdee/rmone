using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Data;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.Util.Log;

namespace uGovernIT.Manager
{
    public class UGITLicense
    {
        public string SiteCollectionID { get; private set; }
        public List<ModuleDetail> Modules { get; private set; }
        public string LicenseDetail { get; private set; }

        /// <summary>
        /// Create Constructor, Through exception if sitecollectionid is null or empty. 
        /// </summary>
        /// <param name="siteCollectionID"></param>
        public UGITLicense(string siteCollectionID)
        {
            if (!string.IsNullOrEmpty(siteCollectionID) && siteCollectionID.Trim() != string.Empty)
            {
                SiteCollectionID = siteCollectionID;
                Modules = new List<ModuleDetail>();
                LicenseDetail = string.Empty;
            }
            else
            {
                new Exception("Site collection id is mandatory.");
            }
        }

        /// <summary>
        /// Performs decryption on licensed modules
        /// </summary>
        /// <param name="licenseKey"></param>
        public void DecryptLicense(string license)
        {
            if (license != null)
            {
                LicenseDetail = license;
                string decryptedLicense = uGovernITCrypto.Decrypt(license, SiteCollectionID);
                Modules = GetModulesFromString(decryptedLicense);
            }
        }

        private List<ModuleDetail> GetModulesFromString(string decryptedLicense)
        {
            List<ModuleDetail> modules =new List<ModuleDetail>();
            string[] licenseKeyVals = decryptedLicense.Split(new string[] { "#;#" }, StringSplitOptions.RemoveEmptyEntries);

            if (licenseKeyVals.Length > 1 && licenseKeyVals[0] == SiteCollectionID)
            {
                string[] modulesDetail = licenseKeyVals[1].Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string moduleDetail in modulesDetail)
                {
                    string[] moduleContraints = moduleDetail.Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries);
                    ModuleDetail module = new ModuleDetail();
                    if (moduleContraints.Length > 0)
                    {
                        module.ModuleName = moduleContraints[0];
                        modules.Add(module);
                    }
                    if (moduleContraints.Length > 1)
                    {
                        module.UserCount = int.Parse(moduleContraints[1]);
                    }
                    if (moduleContraints.Length > 2)
                    {
                        module.ExpiryDate = Convert.ToDateTime(moduleContraints[2]);
                    }
                }
            }

            return modules;
        }

        /// <summary>
        /// Convert Module list to datatable having columns
        /// Module, UserCount, ExpiryDate
        /// </summary>
        /// <returns></returns>
        public DataTable ConvertModulesToDatatable()
        {
            DataTable licensedModules = new DataTable();
            licensedModules.Columns.Add("Module");
            licensedModules.Columns.Add("UserCount");
            licensedModules.Columns.Add("ExpiryDate");
            foreach (ModuleDetail module in Modules)
            {
                DataRow row = licensedModules.NewRow();
                row["Module"] = module.ModuleName;
                row["UserCount"] = module.UnlimitedUser ? "Unlimited" : module.UserCount.ToString();
                row["ExpiryDate"] = module.NonExpiry ? "Non-Expiring" : module.ExpiryDate.ToString("MMM dd, yyyy");
                licensedModules.Rows.Add(row);
            }
            return licensedModules;
        }
    }

    public class ModuleDetail
    {
        public string ModuleName { get; set; }
        public int UserCount { get; set; }
        public bool UnlimitedUser { get { return UserCount > 0 ? false : true; } }
        public DateTime ExpiryDate { get; set; }
        public bool NonExpiry { get { return ExpiryDate.Date.CompareTo(new DateTime(2011, 1, 1)) <= 0 ? true : false; } }
    }


    /// <summary>
    /// Used for encrpytion and decrpytion license based on salt and encrpytionpassword
    /// Encryption password must be sitecollectionid of customer
    /// Salt is alway same and is hardcoded in the class
    /// </summary>
    public class uGovernITCrypto
    {
        private static readonly byte[] salt = Encoding.ASCII.GetBytes("81D52423-EF18-4A63-A8CA-48A7ED659ADA-9414E4B8-3DDE-4B79-AA43-226B78E4FAAB");

        /// <summary>
        /// Encypt give string
        /// </summary>
        /// <param name="textToEncrypt"></param>
        /// <param name="encryptionPassword"></param>
        /// <returns></returns>
        public static string Encrypt(string textToEncrypt, string encryptionPassword)
        {
            // Get RijndaelManaged variable having key and vector
            var algorithm = GetAlgorithm(encryptionPassword);

            byte[] encryptedBytes;
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV))
            {
                byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);
                encryptedBytes = InMemoryCrypt(bytesToEncrypt, encryptor);
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// Performs an Decryption operation of encrypted string based on encryption password.
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <param name="encryptionPassword"></param>
        /// <returns></returns>
        public static string Decrypt(string encryptedText, string encryptionPassword)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
                return string.Empty;

            var algorithm = GetAlgorithm(encryptionPassword);

            byte[] decryptedBytes;
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV))
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                decryptedBytes = InMemoryCrypt(encryptedBytes, decryptor);
            }

            return Encoding.UTF8.GetString(decryptedBytes);
        }

        /// <summary>
        /// Performs an in-memory encrypt/decrypt transformation on a byte array.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        private static byte[] InMemoryCrypt(byte[] data, ICryptoTransform transform)
        {
            MemoryStream memory = new MemoryStream();
            try
            {
                using (Stream stream = new CryptoStream(memory, transform, CryptoStreamMode.Write))
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "InMemoryCrypt crashed");
            }
            return memory.ToArray();
        }

        /// <summary>
        ///  Defines a RijndaelManaged algorithm and sets its key and Initialization Vector (IV) 
        ///  values based on the encryptionPassword received.
        /// </summary>
        /// <param name="encryptionPassword"></param>
        /// <returns></returns>
        private static RijndaelManaged GetAlgorithm(string encryptionPassword)
        {
            // Create an encryption key from the encryptionPassword and salt.
            var key = new Rfc2898DeriveBytes(encryptionPassword, salt);

            // we are going to use the Rijndael algorithm with the key that we've just got.
            var algorithm = new RijndaelManaged();

            int bytesForKey = algorithm.KeySize / 8;
            int bytesForIV = algorithm.BlockSize / 8;
            algorithm.Key = key.GetBytes(bytesForKey);
            algorithm.IV = key.GetBytes(bytesForIV);

            return algorithm;

        }
    }
}
