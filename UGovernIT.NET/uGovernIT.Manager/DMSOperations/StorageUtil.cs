using System.Configuration;
using System.IO;
using System.Web;
using Amazon.S3;
using Amazon.S3.Model;

namespace uGovernIT.Manager.DMSOperations
{
    public class StorageUtil
    {
        public static bool Store(HttpPostedFileBase file, string localFolder, string path, string inputFileName)
        {
            bool returnAck = false;
            bool local = IsLocal();

            if (local)
            {
                string pathToSave = Path.Combine(localFolder, path).Replace("/", "\\");

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                pathToSave = Path.Combine(pathToSave, inputFileName);

                file.SaveAs(pathToSave);

                if (File.Exists(pathToSave))
                {
                    returnAck = true;
                }
            }
            else
            {
                string AWSProfileName = ConfigurationManager.AppSettings["Environment"];
                string accessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
                string secretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
                string bucketName = ConfigurationManager.AppSettings["AWSBucket"];

                using (var amazonS3Client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.USWest1))
                {
                    PutObjectRequest request = new PutObjectRequest()
                    {
                        BucketName = bucketName,
                        CannedACL = S3CannedACL.PublicRead,
                        Key = Path.Combine(AWSProfileName, path, inputFileName).Replace("\\", "/"),
                        InputStream = file.InputStream
                    };

                    PutObjectResponse response = amazonS3Client.PutObject(request);

                    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        returnAck = true;
                    }
                }
            }
            return returnAck;
        }

        public static void Download(string path, string fileName, string downloadFileName)
        {
            bool local = IsLocal();

            if (local)
            {
                var physicalPath = GetFileFullPath(path, fileName);
                var httpContext = HttpContext.Current;

                // if file doesn't exists then try with original file name path
                if (!File.Exists(physicalPath))
                {
                    physicalPath = GetFileFullPath(path, downloadFileName);
                }

                UtilityOperations.DownloadFile(httpContext, physicalPath, downloadFileName);
            }
            else
            {
                string dest = Path.Combine(HttpRuntime.CodegenDir, fileName);

                string AWSProfileName = ConfigurationManager.AppSettings["Environment"];
                string accessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
                string secretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
                string bucketName = ConfigurationManager.AppSettings["AWSBucket"];

                using (var amazonS3Client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.USWest1))
                {
                    GetObjectRequest request = new GetObjectRequest()
                    {
                        BucketName = bucketName,
                        Key = Path.Combine(AWSProfileName, path, fileName).Replace("\\", "/")
                    };

                    using (GetObjectResponse response = amazonS3Client.GetObject(request))
                    {
                        response.WriteResponseStreamToFile(dest, false);
                    }

                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + downloadFileName);
                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                    HttpContext.Current.Response.TransmitFile(dest);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();

                    // Clean up temporary file.
                    File.Delete(dest);
                }
            }
        }

        public static string GetDMSBaseFolderPath()
        {
            var uploadedFiles = ConfigurationManager.AppSettings["DMSBaseFolder"];
            var environment = ConfigurationManager.AppSettings["Environment"];

            var path = HttpContext.Current.Server.MapPath("~/" + uploadedFiles);

            path = Path.Combine(path, environment);

            return path;
        }

        public static string GetFileFullPath(string path, string fileName)
        {
            return Path.Combine(GetDMSBaseFolderPath(), path, fileName).Replace("/", "\\");
        }

        public static bool IsLocal()
        {
            bool.TryParse(ConfigurationManager.AppSettings["DMSUseAWSS3"], out bool result);

            return !result;
        }
    }
}
