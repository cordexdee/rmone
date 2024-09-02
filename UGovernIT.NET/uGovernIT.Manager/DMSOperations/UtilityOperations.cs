using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace uGovernIT.Manager.DMSOperations
{
    public class UtilityOperations
    {
        public static void DownloadFile(HttpContext httpContext, string filePath, string fileName)
        {            
            var fileAttributes = File.GetAttributes(filePath);

            if ((fileAttributes & FileAttributes.Directory) != FileAttributes.Directory)
            {
                httpContext.Response.Buffer = true;
                httpContext.Response.Clear();
                httpContext.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                httpContext.Response.ContentType = "application/octet-stream";

                httpContext.Response.TransmitFile(filePath);
                httpContext.Response.Flush();
                httpContext.Response.End();
            }
        }

        // Need to check this method
        public static void DownloadFiles(List<string> filePaths, HttpContext httpContext)
        {
            if (filePaths.Count == 0)
                return;

            var file = filePaths[0];
            var attr1 = File.GetAttributes(file);

            if (filePaths.Count == 1 && ((attr1 & FileAttributes.Directory) != FileAttributes.Directory))
            {
                string filename = Path.GetFileName(file);
                httpContext.Response.Buffer = true;
                httpContext.Response.Clear();
                httpContext.Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                httpContext.Response.ContentType = "application/octet-stream";
                httpContext.Response.WriteFile(file);
            }
            else
            {
                string zipName = string.Format("archive-{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                httpContext.Response.Buffer = true;
                httpContext.Response.Clear();
                httpContext.Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                httpContext.Response.ContentType = "application/zip";

                //using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                //{
                //    foreach (string path in archives)
                //    {
                //        try
                //        {
                //            FileAttributes attr = System.IO.File.GetAttributes(path);

                //            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                //                zip.AddDirectory(path, Path.GetFileNameWithoutExtension(path));
                //            else
                //                zip.AddFile(path, "");
                //        }
                //        catch (Exception)
                //        {
                //        }
                //    }
                //    zip.Save(httpContext.Response.OutputStream);
            }
        }

        // Need to check this method
        public static string GetVirtualPath(string physicalPath)
        {
            string applicationPath = System.Web.Hosting.HostingEnvironment.MapPath("~/");
            string url = physicalPath.Substring(applicationPath.Length).Replace('\\', '/').Insert(0, "~/");
            return (url);
        }
    }
}
