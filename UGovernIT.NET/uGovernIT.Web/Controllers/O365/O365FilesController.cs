using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using uGovernIT.DMS;
//using uGovernIT.DMS.O365;
using uGovernIT.Utility.Entities.DMSDB;
using Microsoft.Graph;

using uGovernIT.Utility;
using Owin;
using Microsoft.Owin;
using uGovernIT.Web.O365;
//[assembly: OwinStartup(typeof(uGovernIT.DMS.Helpers.Startup))]
namespace uGovernIT.Web
{

    [Authorize]
    public class O365FilesController : Controller
    {

        //protected IFilesService filesService = null;

        protected FilesService filesService = null;

        public O365FilesController()
        {
            filesService = new FilesService();
            //uGovernIT.DMS.Helpers.Startup objStartup = new DMS.Helpers.Startup();
            //uGovernIT.Web.O365Startup objStartup = new O365Startup();

            //var dmsApp = HoldStartup.app;
            //objStartup.Configuration((IAppBuilder)dmsApp);
        }


        public ActionResult Index()
        {
            ViewBag.activeTab = "O365-Files";
            return View("Files");
        }

        // Get the drive items in the root directory of the current user's default drive.
        [Obsolete]
        public async Task<ActionResult> GetMyFilesAndFolders()
        {


            ResultsViewModel results = new ResultsViewModel();
            try
            {

                // Initialize the GraphServiceClient.
                GraphServiceClient graphClient = SDKHelper.GetAuthenticatedClient();

                // Get the files and folders in the current user's drive.
                results.Items = await filesService.GetMyFilesAndFolders(graphClient);
                results.Message = "My Files And Folders";
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resources.Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = string.Format(Resources.Resource.Error_Message, Request.RawUrl, se.Error.Code, se.Error.Message) });
            }
            return View("Files", results);
        }

        // Get the items that are shared with the current user.
        [Obsolete]
        public async Task<ActionResult> GetSharedWithMe()
        {
            ResultsViewModel results = new ResultsViewModel(false);
            try
            {

                // Initialize the GraphServiceClient.
                GraphServiceClient graphClient = SDKHelper.GetAuthenticatedClient();

                // Get the shared items.
                results.Items = await filesService.GetSharedWithMe(graphClient);
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resources.Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = string.Format(Resources.Resource.Error_Message, Request.RawUrl, se.Error.Code, se.Error.Message) });

            }
            return View("Files", results);
        }

        // Get the current user's default drive.
        [Obsolete]
        public async Task<ActionResult> GetMyDrive()
        {
            ResultsViewModel results = new ResultsViewModel(false);
            try
            {

                // Initialize the GraphServiceClient.
                GraphServiceClient graphClient = SDKHelper.GetAuthenticatedClient();

                // Get the current user's default drive.
                results.Items = await filesService.GetMyDrive(graphClient);
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resources.Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = string.Format(Resources.Resource.Error_Message, Request.RawUrl, se.Error.Code, se.Error.Message) });
            }
            return View("Files", results);
        }

        // Create a text file in the current user's root directory.
        [Obsolete]
        public async Task<ActionResult> CreateFile()
        {
            ResultsViewModel results = new ResultsViewModel();

            try
            {

                // Initialize the GraphServiceClient.
                GraphServiceClient graphClient = SDKHelper.GetAuthenticatedClient();

                // Add the file.
                results.Items = await filesService.CreateFile(graphClient);
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resources.Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = string.Format(Resources.Resource.Error_Message, Request.RawUrl, se.Error.Code, se.Error.Message) });
            }
            return View("Files", results);
        }

        // Create a folder in the current user's root directory. 
        [Obsolete]
        public async Task<ActionResult> CreateFolder()
        {
            ResultsViewModel results = new ResultsViewModel();
            try
            {

                // Initialize the GraphServiceClient.
                GraphServiceClient graphClient = SDKHelper.GetAuthenticatedClient();

                // Add the folder.
                results.Items = await filesService.CreateFolder(graphClient);
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resources.Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = string.Format(Resources.Resource.Error_Message, Request.RawUrl, se.Error.Code, se.Error.Message) });
            }
            return View("Files", results);
        }

        // Upload a BMP file to the current user's root directory.
        [Obsolete]
        public async Task<ActionResult> UploadLargeFile()
        {
            ResultsViewModel results = new ResultsViewModel();

            try
            {

                // Initialize the GraphServiceClient.
                GraphServiceClient graphClient = SDKHelper.GetAuthenticatedClient();

                // Add the file.
                results.Items = await filesService.UploadLargeFile(graphClient);
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resources.Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = string.Format(Resources.Resource.Error_Message, Request.RawUrl, se.Error.Code, se.Error.Message) });
            }
            return View("Files", results);
        }

        // Get a file or folder (metadata) in the current user's drive.
        [Obsolete]
        public async Task<ActionResult> GetFileOrFolderMetadata(string id)
        {
            ResultsViewModel results = new ResultsViewModel();
            try
            {

                // Initialize the GraphServiceClient.
                GraphServiceClient graphClient = SDKHelper.GetAuthenticatedClient();

                // Get the file or folder object.
                results.Items = await filesService.GetFileOrFolderMetadata(graphClient, id);
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resources.Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = string.Format(Resources.Resource.Error_Message, Request.RawUrl, se.Error.Code, se.Error.Message) });
            }
            return View("Files", results);
        }

        // Download the content of an existing file.
        // This snippet returns the length of the file stream and some file metadata properties.
        [Obsolete]
        public async Task<ActionResult> DownloadFile(string id)
        {
            ResultsViewModel results = new ResultsViewModel();
            try
            {

                // Initialize the GraphServiceClient.
                GraphServiceClient graphClient = SDKHelper.GetAuthenticatedClient();

                // Download the file.
                results.Items = await filesService.DownloadFile(graphClient, id);

                // Handle selected item is not supported.
                foreach (var item in results.Items)
                {
                    if (item.Properties.ContainsKey(Resources.Resource.File_ChooseFile)) results.Selectable = false;
                }
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resources.Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = string.Format(Resources.Resource.Error_Message, Request.RawUrl, se.Error.Code, se.Error.Message) });
            }
            return View("Files", results);
        }

        // Update the metadata of a file or folder. 
        // This snippet updates the item's name. 
        // To move an item, point the ParentReference.Id or ParentReference.Path property to the target destination.
        [Obsolete]
        public async Task<ActionResult> UpdateFileOrFolderMetadata(string id, string name)
        {
            ResultsViewModel results = new ResultsViewModel();
            try
            {

                // Initialize the GraphServiceClient.
                GraphServiceClient graphClient = SDKHelper.GetAuthenticatedClient();

                // Update the item.
                results.Items = await filesService.UpdateFileOrFolderMetadata(graphClient, id, name);
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resources.Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = string.Format(Resources.Resource.Error_Message, Request.RawUrl, se.Error.Code, se.Error.Message) });
            }
            return View("Files", results);
        }

        // Update the content of a file in the user's root directory. 
        // This snippet replaces the text content of a .txt file.
        [Obsolete]
        public async Task<ActionResult> UpdateFileContent(string id)
        {
            ResultsViewModel results = new ResultsViewModel();
            try
            {

                // Initialize the GraphServiceClient.
                GraphServiceClient graphClient = SDKHelper.GetAuthenticatedClient();

                // Get the file. Make sure it's a .txt file (for the purposes of this snippet).
                results.Items = await filesService.UpdateFileContent(graphClient, id);

                // Handle selected item is not supported.
                foreach (var item in results.Items)
                {
                    if (item.Properties.ContainsKey(Resources.Resource.File_ChooseTextFile)) results.Selectable = false;
                }
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resources.Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = string.Format(Resources.Resource.Error_Message, Request.RawUrl, se.Error.Code, se.Error.Message) });
            }
            return View("Files", results);
        }

        // Delete a file in the user's root directory.
        [Obsolete]
        public async Task<ActionResult> DeleteFileOrFolder(string id)
        {
            ResultsViewModel results = new ResultsViewModel(false);
            try
            {

                // Initialize the GraphServiceClient.
                GraphServiceClient graphClient = SDKHelper.GetAuthenticatedClient();

                // Delete the item.
                results.Items = await filesService.DeleteFileOrFolder(graphClient, id);
                results.Message = "Deleted File Or Folder";
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resources.Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = string.Format(Resources.Resource.Error_Message, Request.RawUrl, se.Error.Code, se.Error.Message) });
            }
            return View("Files", results);
        }

        // Get a sharing link for a file in the current user's drive.
        [Obsolete]
        public async Task<ActionResult> GetSharingLink(string id)
        {
            ResultsViewModel results = new ResultsViewModel(false);
            try
            {

                // Initialize the GraphServiceClient.
                GraphServiceClient graphClient = SDKHelper.GetAuthenticatedClient();

                results.Items = await filesService.GetSharingLink(graphClient, id);
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resources.Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = string.Format(Resources.Resource.Error_Message, Request.RawUrl, se.Error.Code, se.Error.Message) });
            }
            return View("Files", results);
        }
    }

    public class AppBuilderProvider : IDisposable
    {
        private IAppBuilder _app;
        public AppBuilderProvider(IAppBuilder app)
        {
            _app = app;
        }
        public IAppBuilder Get() { return _app; }
        public void Dispose() { }
    }

}