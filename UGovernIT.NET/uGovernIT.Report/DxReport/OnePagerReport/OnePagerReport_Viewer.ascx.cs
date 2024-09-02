using System;
using System.Web.UI;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Report.Helpers;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Report.DxReport
{
    public partial class OnePagerReport_Viewer : UserControl
    {
        #region For Multiple Projects
        public int[] PMMIds { get; set; }
        public int projectYear { get; set; }
        ProjectCompactReportEntity entity;
        OnePagerReportHelper onePagerHelper;
        public string TicketId { get; set; }
        public string documnetPickerUrl { get; set; }
        public string DocName { get; set; }
        public string FolderGuid { get; set; }
        public string stageName { get; set; }
        public string SelectFolder { get; set; }
        public string PathValue { get; set; }
        public string Module { get; set; }
        #endregion
        ApplicationContext _context = System.Web.HttpContext.Current.GetManagerContext();
        public string delegateControl = "BuildReport.aspx";
        public string ReportFilterURl;
        protected override void OnInit(EventArgs e)
        {
            Module = Request["Module"];
            List<string> paramRequired = new List<string>();
            paramRequired.Add("PMMIds");
            paramRequired.Add("projectYear");
            if (Request["PMMIds"] != null)
            {
                if (!string.IsNullOrEmpty(Request["PMMIds"]))
                {
                    PMMIds = Array.ConvertAll<string, int>(Request["PMMIds"].Split(','), int.Parse);
                }
            }
            projectYear= Convert.ToInt32(Request["projectYear"]);
            if (ReportHelper.CheckFilterValues(Request.QueryString, paramRequired) == false)
            {
                ReportFilterURl = _context.SiteUrl + delegateControl + "?reportName=OnePagerReport&Filter=Filter&Module=" + Module + "&alltickets=" + UGITUtility.ObjectToString(Request["alltickets"]);
                Response.Redirect(ReportFilterURl);
            }
            hdnconfiguration.Set("RequestUrl", Request.Url.AbsolutePath);
            onePagerHelper = new OnePagerReportHelper(_context);
            onePagerHelper.PMMIds = PMMIds;
            entity = onePagerHelper.GetOnePagerReportEntity();
            if (entity.ProjectDetails.Rows.Count > 1)
            {
                ASPxSplitter1.Panes[0].Collapsed = false;
                MultiViewPagerReport proReport = new MultiViewPagerReport(entity);
                RptVwrProjectReport.Report = proReport;
            }
            else
            {
                ASPxSplitter1.Panes[0].Collapsed = true;
                ProjectCompactReport report = new ProjectCompactReport(entity);
                RptVwrProjectReport.Report = report;
            }
            pnlReport.Visible = true;

            ///Document Folder picker related code.
            //SPListItem spListItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.PMMProjects, PMMIds[0]);
            //if (spListItem != null)
            //{
            //    TicketId = Convert.ToString(spListItem[DatabaseObjects.Columns.TicketId]);
            //    DocName = TicketId.Replace("-", "_");
            //}

            //Ticket ticket = new Ticket(_context, "PMM");
            //var stage = ticket.GetTicketCurrentStage(spListItem);
            //stageName = stage.Name;
            //TreeViewEDM eDMTreeView = DMCache.LoadPortalTreeView(DocName);
            //if (eDMTreeView != null && eDMTreeView.FolderList != null && eDMTreeView.FolderList.Count > 0)
            //{
            //    FolderList folder = eDMTreeView.FolderList.FirstOrDefault(c => c.Name == stageName);
            //    if (folder != null)
            //    {
            //        FolderGuid = folder.folderGuid;
            //        CurrentUserSelectionInfo currentSelection = new CurrentUserSelectionInfo(DocName, FolderGuid, EDMViewType.ByFolder, false);
            //        if (currentSelection != null)
            //        {
            //            PathValue = currentSelection.pathValue;
            //            SelectFolder = currentSelection.selectedFolder;
            //        }
            //    }
            //}
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        

        private void UpdateAssignedTo(DataRow row, string columnName)
        {
            UserProfileManager userProfileManager = new UserProfileManager(_context);
            string[] userList = UGITUtility.SplitString(Convert.ToString(row[columnName]), ",", StringSplitOptions.RemoveEmptyEntries);
            if (userList.Length > 0)
            {
                List<string> users = new List<string>();
                foreach (string userName in userList)
                {
                    UserProfile user = userProfileManager.GetUserByUserName(userName);
                    if (user != null)
                        users.Add(user.Name);
                }

                if (users.Count > 0)
                    row[columnName] = String.Join("; ", users);
            }
        }

        //private SPListItem UpdateAssignedTo(SPListItem row, string columnName)
        //{
        //    string[] userList = uHelper.SplitString(Convert.ToString(row[columnName]), ",", StringSplitOptions.RemoveEmptyEntries);
        //    if (userList.Length > 0)
        //    {
        //        List<string> users = new List<string>();
        //        foreach (string userName in userList)
        //        {
        //            SPUser user = UserProfile.GetUserByName(userName, SPPrincipalType.All, SPContext.Current.Web);
        //            if (user != null)
        //                users.Add(user.Name);
        //        }

        //        if (users.Count > 0)
        //            row[columnName] = String.Join("; ", users);
        //    }
        //    return row;
        //}
        private void UpdateProjectDescription(DataRow row)
        {
            //row["ProjectDescription"] = item[DatabaseObjects.Columns.TicketDescription];
        }
        protected void cbMailsend_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            if (e.Parameter == "SendMail")
            {
                string fileName = string.Format("Project_Compact_Report_{0}", uHelper.GetCurrentTimestamp());
                string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content/images/ugovernit/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));
                //Export to pdf
                RptVwrProjectReport.Report.ExportToPdf(path);
                e.Result = UGITUtility.GetAbsoluteURL("layouts/ugovernit/DelegateControl.aspx?control=ticketemail&type=openTicketReport&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
            else if (e.Parameter == "SaveToDoc")
            {
                string fileName = string.Format("Project_Compact_Report_{0}", uHelper.GetCurrentTimestamp());
                string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content/images/ugovernit/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));

                //Export and save 
                RptVwrProjectReport.Report.ExportToPdf(path);
                e.Result = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.ReportUploadControlUrl + "&localpath=" + path + "&relativepath=" + uploadFileURL);
            }

        }
    }
}
