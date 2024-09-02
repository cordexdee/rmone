using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class ImageControl : System.Web.UI.UserControl
    {
        public string FileName { get; set; }
        public string FileId { get; set; }
        public string Action { get; set; }
        public string Extension { get; set; }
        protected void OnInit(object sender, EventArgs e)
        {            
            base.OnInit(e);
        }
        public void GetImage()
        {
         
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DocumentManager documentManager = new DocumentManager(HttpContext.Current.GetManagerContext());
            Utility.Entities.Document documents = documentManager.Load(x => x.FileID == FileId).FirstOrDefault();
            if (Action.Equals("view") && documents != null)
            {
                byte[] bytes = documents.Blob;
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = documents.ContentType;
                Response.AddHeader("content-disposition", "attachment;filename="
                + documents.Name + "." + documents.Extension);
                Response.BinaryWrite(bytes);
                //Response.Flush();
                Response.End();
            }
        }
    }
}