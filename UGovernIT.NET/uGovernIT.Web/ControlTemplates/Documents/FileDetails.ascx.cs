using Microsoft.AspNet.Identity.Owin;
using System;
using System.Web;
using System.Web.UI;
using uGovernIT.DAL;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DMSDB;

namespace uGovernIT.Web
{
    public partial class FileDetails : UserControl
    {
        private ApplicationContext _context = null;
        private DocRepositoryBase _docRepositoryBase = null;
        UserProfileManager umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
        UserProfile userInfo;

        public int fileID { get; set; }

        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }

        protected DocRepositoryBase DocRepositoryBase
        {
            get
            {
                if (_docRepositoryBase == null)
                {
                    _docRepositoryBase = new DocRepositoryBase(ApplicationContext);
                }
                return _docRepositoryBase;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindDocumentDetails();
        }

        private void BindDocumentDetails()
        {
            if (fileID > 0)
            {
                DMSDocument dmsDocument = DocRepositoryBase.GetFilesById(fileID);

                if (dmsDocument != null)
                {
                    ltlDocID.Text = dmsDocument.DocumentControlID;

                    ltlDocumentName.Text = dmsDocument.FileName;

                    ltlVersion.Text = dmsDocument.Version;

                    ltlPath.Text = dmsDocument.FullPath;

                    ltlCreatedOn.Text = Convert.ToString(dmsDocument.CreatedOn);

                    ltlModifiedOn.Text = Convert.ToString(dmsDocument.UpdatedOn);

                    ltlSizeOfFile.Text = UGITUtility.BytesToString(dmsDocument.Size);

                    if (!string.IsNullOrEmpty(dmsDocument.AuthorId))
                    {
                        userInfo = umanager.LoadById(dmsDocument.AuthorId);

                    }

                    ltlAuthor.Text = userInfo != null ? userInfo.Name : "";

                    ltlTags.Text = dmsDocument.Tags;

                    ltlReviewRequired.Text =Convert.ToString(dmsDocument.ReviewRequired);


                }

            }
        }
    }
}