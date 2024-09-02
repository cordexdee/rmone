using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.ControlTemplates.GlobalPage;

namespace uGovernIT.Web
{
    public partial class DocumentControl : UPage
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
            FileId = Request.QueryString["id"];
            FileName = "";
            Action = Request.QueryString["action"];
            Extension = Request.QueryString["extension"];
            if (!string.IsNullOrEmpty(FileId))
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
                    Response.AddHeader("content-disposition", "attachment;filename=" + documents.Name);
                    Response.BinaryWrite(bytes);
                    //Response.Flush();
                    Response.End();
                }
            }
            Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext()?.TenantAccountId}|{HttpContext.Current.CurrentUser()?.Name}: Visited Page: DocumentControl.aspx");
        }
    }
}