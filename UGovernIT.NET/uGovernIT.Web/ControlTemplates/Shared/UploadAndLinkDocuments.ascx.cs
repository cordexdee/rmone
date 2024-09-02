using System;
using System.Web;
using uGovernIT.DMS.Amazon;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.Shared
{
    public partial class UploadAndLinkDocuments : System.Web.UI.UserControl
    {
        public string projectPublicID { get; set; }

        public string DocumentManagementUrl { get; set; }

        protected string projectID = string.Empty;

        public string ModuleName { get; set; }
        
        public string FolderName { get; set; }

        public string ParentIframeId { get; set; }

        public bool IsTabActive { get; set; }

        protected string sourceURL;


        private DocumentManagementController _documentManagement = null;

        protected DocumentManagementController DocumentManagementController
        {
            get
            {
                if (_documentManagement == null)
                {
                    _documentManagement = new DocumentManagementController();
                }
                return _documentManagement;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (string.IsNullOrEmpty(ModuleName))
                ModuleName = string.IsNullOrEmpty(Request["ModuleName"]) ? Request["Module"] : Request["ModuleName"];
            ApplicationContext context = HttpContext.Current.GetManagerContext();

            projectID = projectPublicID = string.IsNullOrEmpty(Request["PublicTicketID"]) ? Request["ticketId"] : Request["PublicTicketID"];
            if(!string.IsNullOrEmpty(Request["folderName"]))
                FolderName = Convert.ToString(Request["folderName"]);

            if (!string.IsNullOrEmpty(Request["isTabActive"]))
                IsTabActive = Convert.ToBoolean(Request["isTabActive"]);

            sourceURL = Server.UrlEncode(Request.Url.AbsolutePath);
            //sourceURL = Request["source"] != null ? Request["source"] : Server.UrlEncode(Request.Url.AbsolutePath);

            ParentIframeId = Convert.ToString(Request["ParentIframeId"]);
            string docName = projectPublicID.Replace("-", "_");
            if (!string.IsNullOrEmpty(docName.Trim()))
            { 
                var repositoryService = new DMSManagerService(context);
                var directory = repositoryService.GetUserRepoDirectory(context.CurrentUser.Id, docName);
                if (directory == null) //document portal not created already
                {
                    pnlCreateDocPortal.Style.Add("display", "block"); //display "create document portal" button if doc portal not created already.
                    divLinkToDoc.Visible = false;
                }
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Guid newFrameId = Guid.NewGuid();
            DocumentManagementUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=DocumentControl&ticketId={0}&module={2}",
                                                       projectID, newFrameId, ModuleName));
        }

        protected void btnLinkDocument_Click(object sender, EventArgs e)
        {
            //Upload upload = new Upload();
            //upload.LinkDocument();
            //Try to pass dynamic value and if documentment created or not

            DocumentManagementController.Index("", projectPublicID, Convert.ToString(Request["folderName"]));
        }
    }
}