using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DefaultConfig;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class ReportUploadControl : UserControl
    {
        public string localpath { get; set; }
        public string DocName { get; set; }
        public bool IsSelectFolder { get; set; }
        public string folderID { get; set; }
        public string selectedFolderGuid { get; set; }
        public string errorMessage { get; set; }
        public string relativepath { get; set; }

        string fileName = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            /*
            IsSelectFolder = true;
            //selectedFolderGuid = Request
            SelectFolder selectfoldercontrol = (SelectFolder)Page.LoadControl("~/ControlTemplates/uGovernIT/SelectFolder.ascx");
            selectfoldercontrol.Portal = DocName;
            selectfoldercontrol.SelectedFolder = selectedFolderGuid;
            selectfoldercontrol.DoneClick += selectfoldercontrol_DoneClick;
            pnlSelectFolder.Controls.Add(selectfoldercontrol);
            */

            linkFile.NavigateUrl = UGITUtility.GetAbsoluteURL(relativepath);
            fileName = localpath.Substring(localpath.LastIndexOf('\\') + 1, localpath.Length - (localpath.LastIndexOf('\\') + 1));
            linkFile.Text = "here";
            linkFile.Target = "_blank";
            base.OnInit(e);
        }

        void selectfoldercontrol_DoneClick(object sender, EventArgsDoneClickEventHandler e)
        {
            /*
            string retValue = e.Dict["DocId"];
            // string[] strvalues = uHelper.SplitString(retValue, Constants.Separator1);
            string[] strvalues = UGITUtility.SplitString(retValue, Constants.Separator5);

            hdnfolderId.Value = strvalues[0];
            lblSelectedFolder.Text = strvalues[1];
            hdnDocName.Value = strvalues[2];
            */
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (!File.Exists(localpath))
                return;

            var fileInfo = new System.IO.FileInfo(localpath);
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename=\"{0}\"", fileName));
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.WriteFile(localpath);
            Response.End();

            /*
            string fileName = localpath.Substring(localpath.LastIndexOf('\\') + 1, localpath.Length - (localpath.LastIndexOf('\\') + 1));
            FileStream stream = new FileStream(localpath, FileMode.Open);
            string strMessage = "";
            selectedFolderGuid = hdnfolderId.Value;
            //  bool returnval = UploadFile(ref strMessage, fileName, true, stream.Length, stream);
            bool returnval =true;
            if (returnval)
            {
                uHelper.ClosePopUpAndEndResponse(Context, false);
            }
            stream.Flush();
            stream.Close();
            stream = null;
            */
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        /// <summary>
        ///  This function upload a new file and update the proprty of existing file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private bool UploadFile(ref string message, string fileName, bool fileIsValid, long contentLength, Stream fileContent)
        //{
        //    // Upload a new file if documentId is not coming in request.// Application context
        //    SPWeb oWeb = SPContext.Current.Web;
        //    //string fileName = ufile.FileName;

        //    if (string.IsNullOrEmpty(DocName))
        //        DocName = hdnDocName.Value;

        //    folderID = selectedFolderGuid;


        //    if (folderID == string.Empty)
        //    {
        //        // XXX - NEED TO HANDLE!!
        //        //overwriteMsg.Text = "Please select a folder";
        //        message = "SELECTFOLDER";
        //        return false;
        //    }
        //    SPFolder oFolder = SPListHelper.GetSPFolder(Helper.StringToGuid(folderID), oWeb);

        //    SPListItem folderItem = oFolder.Item;

        //    SPFile file = null;

        //    string upLoadedfiles = string.Empty;
        //    //overwriteMsg.Text = string.Empty;
        //    int numUploadedFiles = 0;

        //    //New code

        //    if (fileIsValid)
        //    {

        //        #region Check  Folder Properties for Docuement
        //        //Check the Nof of file allowed in this folder.
        //        if (!CheckNoOfFileAllowedInFolder(oFolder))
        //        {
        //            string msgUploadedFiles = string.Empty;
        //            if (upLoadedfiles != string.Empty)
        //                msgUploadedFiles = "File(s) uploaded " + upLoadedfiles;
        //            //overwriteMsg.Text = "No of document allowed in folder is exceding. " + upLoadedfiles;
        //            message = "NUMOFDOC";
        //            return false;
        //        }

        //        //Check the size of folder
        //        if (!uHelper.checkSizeOfPortal(DocName, contentLength))                                    //uploadFileInFolder.PostedFile.ContentLength
        //        {
        //            string msgUploadedFiles = string.Empty;
        //            if (upLoadedfiles != string.Empty)
        //                msgUploadedFiles = "File(s) uploaded " + upLoadedfiles;
        //            overwriteMsg.Text = "Size of folder is exceeding then the allocated size. " + upLoadedfiles;
        //            message = "SIZEOFFOLDER";
        //            return false;
        //        }

        //        //  string filename = uploadedFilesList[i];
        //        string filename = fileName;
        //        //Handling the case of IE, in IE filename received with fullpath. here we extracting the only filname from that.
        //        if (filename.Contains("\\"))
        //            filename = UGITUtility.SplitString(filename, "\\")[UGITUtility.SplitString(filename, "\\").Length - 1];

        //        // Check if filename contains any of the known invalid characters 
        //        string invalidChars = @"~#%&*{}\<>?/+|""";
        //        if (uHelper.stringContainsAnyOf(filename, invalidChars))
        //        {
        //            // See http://blog.techgalaxy.net/archives/550
        //            overwriteMsg.Text = "Invalid character(s) in file name, cannot contain any of these: " + invalidChars;
        //            message = "INVALIDCHAR";
        //            return false;
        //        }

        //        string destinationUrl = oFolder.ServerRelativeUrl + "/" + filename;

        //        // get the file type.
        //        string extension = Path.GetExtension(destinationUrl);

        //        //Check the allowed file type. 
        //        if (!uHelper.checkFileType(extension, oFolder.Item))
        //        {
        //            string msgUploadedFiles = string.Empty;
        //            if (upLoadedfiles != string.Empty)
        //                msgUploadedFiles = "File(s) uploaded " + upLoadedfiles;

        //            overwriteMsg.Text = "The file type you are trying to upload is not allowed. " + upLoadedfiles;
        //            message = "NOTALLOWED";
        //            return false;
        //        }

        //        // get the file object of uploaded  file to check whether the file is exist or not.
        //        file = oWeb.GetFile(destinationUrl);

        //        SPList documentInfoList = oWeb.Lists[DatabaseObjects.Lists.DocumentInfoList];

        //        // Boolean fileAdded = false;

        //        if (file != null && file.Exists)
        //        {
        //            string documentId = file.UniqueId.ToString();

        //            // Check for deleted docs.
        //            SPQuery deletedDocQuery = new SPQuery();
        //            deletedDocQuery.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}' /><Value Type='Boolean'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.DocumentID, documentId, DatabaseObjects.Columns.IsDeleted, 1);
        //            SPListItemCollection deletedDocs = documentInfoList.GetItems(deletedDocQuery);

        //            // Check for non deleted docs.
        //            SPQuery nonDeletedDocQuery = new SPQuery();
        //            nonDeletedDocQuery.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq><Neq><FieldRef Name='{2}' /><Value Type='Boolean'>{3}</Value></Neq></And></Where>", DatabaseObjects.Columns.DocumentID, documentId, DatabaseObjects.Columns.IsDeleted, 1);
        //            SPListItemCollection nonDeletedDocs = documentInfoList.GetItems(nonDeletedDocQuery);

        //            // If one version of file is in deleted mode and another one is in non-deleted mode.(don't allow to delete the and overwrite the file.)
        //            if ((deletedDocs.Count > 0 && nonDeletedDocs.Count < 1) || (deletedDocs.Count == 0 && nonDeletedDocs.Count == 0))
        //            {
        //                //Delete the old file with all version
        //                while (deletedDocs.Count > 0)
        //                    deletedDocs[0].Delete();

        //                //Delete the file from sharepoint also.
        //                file.Delete();

        //                // Add the new file.
        //                file = oWeb.Files.Add(destinationUrl, fileContent, true);
        //            }
        //            else
        //            {
        //                overwriteMsg.Text = "File \"" + filename + "\" already exists in folder " + folderItem.Name;
        //                message = "ALREADYEXISTS";
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            try
        //            {
        //                oFolder.Files.Add(destinationUrl, fileContent);  //uploadFileInFolder.FileBytes
        //            }
        //            catch (Exception ex)
        //            {
        //                overwriteMsg.Text = "File Upload Error: " + ex.Message;
        //                message = "UPLOADERROR";
        //                return false;
        //            }
        //        }

        //        // Save document properties in list
        //        if (!SaveDocumentProperties(oWeb, file.UniqueId.ToString(), file.UIVersionLabel, file, false, true))
        //        {
        //            if (errorMessage == string.Empty && overwriteMsg.Text == string.Empty)
        //            {
        //                overwriteMsg.Text = "Missing or invalid entries";
        //                message = "INVALIDENTRIES";
        //            }
        //            return false;
        //        }

        //        //Save History
        //        SPQuery docQuery = new SPQuery();
        //        docQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.DocumentID, file.UniqueId);
        //        SPListItemCollection docInfoItems = documentInfoList.GetItems(docQuery);
        //        if (docInfoItems.Count > 0)
        //        {
        //            // Check in the file in doc library.
        //            if (file.CheckOutType != SPFile.SPCheckOutType.None)
        //                file.CheckIn(string.Empty);
        //        }

        //        if (upLoadedfiles == string.Empty)
        //            upLoadedfiles = file.Name;
        //        else
        //            upLoadedfiles = upLoadedfiles + ", " + file.Name;

        //        numUploadedFiles += 1;

        //        #endregion
        //    }
        //    else
        //    {
        //        overwriteMsg.Text = "Please browse a document to upload.";
        //        message = "NOTUPLOADED";
        //        return false;
        //    }
        //    //   }

        //    // send mail notification
        //    if (file != null)
        //    {
        //        if (numUploadedFiles > 1)
        //            SendFileUploadNotification(folderItem, upLoadedfiles, file, true);
        //        else if (numUploadedFiles == 1)
        //            SendFileUploadNotification(folderItem, upLoadedfiles, file, false);
        //    }

        //    return true;
        //}

        //private void SendFileUploadNotification(SPListItem folderItem, string files, SPFile file, Boolean isMultiDoc)
        //{
        //    SPWeb oWeb = SPContext.Current.Web;
        //    //Send Mail to Portal Owner, Portal Alt Owner, Folder Owner && CC user.  (UnComment the code if send to portal owner also)
        //    if (uHelper.StringToBoolean(folderItem[DatabaseObjects.Columns.NotifyOwnerOnDocumentUpload]))
        //    {
        //        string mailTo = string.Empty;
        //        string folderOwnersEmail = uHelper.GetUserEmailList(folderItem, DatabaseObjects.Columns.FolderOwner);
        //        if (!string.IsNullOrEmpty(folderOwnersEmail))
        //        {
        //            if (mailTo == string.Empty)
        //                mailTo = folderOwnersEmail;
        //            else
        //                mailTo = "," + folderOwnersEmail;
        //        }

        //        //Get CC user Email
        //        string CCUserEmail = uHelper.GetUserEmailList(folderItem, DatabaseObjects.Columns.CCUser);
        //        string subject;

        //        // Send Mail
        //        if (!string.IsNullOrEmpty(mailTo))
        //        {
        //            // main body text.
        //            StringBuilder htmlBody = new StringBuilder();

        //            // attach the document detail in mail footer when a single document is uploaded.
        //            if (isMultiDoc)
        //            {
        //                subject = "New Documents Uploaded";
        //                htmlBody.Append("New Documents <b>" + files + "</b> uploaded by " + oWeb.CurrentUser.Name + ".");
        //                string mailFooter = uHelper.getMailFooter(file, oWeb, true, files);
        //                htmlBody = htmlBody.Append(mailFooter);
        //            }
        //            else
        //            {
        //                subject = "New Document Uploaded: " + files;
        //                htmlBody.Append("New Document <b>" + files + "</b> uploaded by " + oWeb.CurrentUser.Name + ".");
        //                string mailFooter = uHelper.getMailFooter(file, oWeb, false, file.Name);
        //                htmlBody = htmlBody.Append(mailFooter);
        //            }

        //            //send the mail
        //            MailMessenger.SendMail(mailTo, subject, CCUserEmail, htmlBody.ToString(), true);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Func. : This function saves the document properties in list.
        ///// </summary>
        ///// <param name="oWeb"></param>
        ///// <param name="file"></param>
        //private bool SaveDocumentProperties(SPWeb oWeb, string documentId, string version, SPFile file, Boolean multiUpload, Boolean newFile)
        //{
        //    bool valid = true;
        //    try
        //    {
        //        // Load the document list.
        //        SPList documentInfoList = oWeb.Lists[DatabaseObjects.Lists.DocumentInfoList];
        //        SPListItem documentInfoItem = null;

        //        // Get the document from list.
        //        SPQuery query = new SPQuery();
        //        query.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}'/><Value Type='Text'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.DocumentID, documentId, DatabaseObjects.Columns.DocumentVersion, version);
        //        SPListItemCollection documentInfoColl = documentInfoList.GetItems(query);

        //        //Get the parent folder item.
        //        SPListItem parentFolder = file.ParentFolder.Item;

        //        // Check the file if already exist update it else add it in list.
        //        if (documentInfoColl.Count > 0)
        //            documentInfoItem = documentInfoColl[0];
        //        else
        //            documentInfoItem = documentInfoList.AddItem();

        //        if (documentInfoItem != null)
        //        {
        //            // Do not Update all the property when file is not a new file.
        //            if (newFile)
        //            {
        //                // Generate the DocumentControl id for document.
        //                documentInfoItem[DatabaseObjects.Columns.DocumentControlID] = uHelper.GetDocumentControlID();

        //                documentInfoItem[DatabaseObjects.Columns.Title] = file.Name;
        //                documentInfoItem[DatabaseObjects.Columns.PortalId] = file.DocumentLibrary.ID.ToString();
        //                documentInfoItem[DatabaseObjects.Columns.DocumentID] = file.UniqueId;
        //                documentInfoItem[DatabaseObjects.Columns.FileName] = file.Name;
        //                documentInfoItem[DatabaseObjects.Columns.FolderName] = file.ParentFolder.Url;
        //                documentInfoItem[DatabaseObjects.Columns.DocumentVersion] = file.UIVersionLabel.ToString();
        //                documentInfoItem[DatabaseObjects.Columns.Authors] = oWeb.CurrentUser;
        //                documentInfoItem[DatabaseObjects.Columns.FolderID] = file.ParentFolder.UniqueId.ToString();

        //                //Document Retention Days and Set The Expiration Date

        //                documentInfoItem[DatabaseObjects.Columns.KeepDocsAlive] = Convert.ToString(parentFolder[DatabaseObjects.Columns.KeepDocsAlive]);
        //                documentInfoItem[DatabaseObjects.Columns.ExpirationDate] = DateTime.Now.AddMonths(Convert.ToInt32(parentFolder[DatabaseObjects.Columns.KeepDocsAlive]));

        //                // Check for Review Required
        //                documentInfoItem[DatabaseObjects.Columns.ReviewRequired] = false;
        //                documentInfoItem[DatabaseObjects.Columns.NumOfReviewCycle] = 0;
        //                documentInfoItem[DatabaseObjects.Columns.ReviewStep] = 1;
        //                documentInfoItem[DatabaseObjects.Columns.DocumentStatus] = Constants.DocumentCompletedStatus;

        //                //documentInfoItem[DatabaseObjects.Columns.DocumentTypeLookup] = Request.Form[ddlDocType.UniqueID];//ddlDocType.SelectedValue;
        //                //documentInfoItem[DatabaseObjects.Columns.DocRevision] = Request.Form[txtDocumentRevision.UniqueID]; ;// txtDocumentRevision.Text;
        //            }

        //            //Update the properties that is available in new file upload and for update single & multiple file case also.
        //            //Replace all ";" and space " " seperator with ",". also attach a "," at last so that we can search for exact match for tag.
        //            //In case of multiple update keep the value same if the value is "<Keep>".
        //            if (Request.Form[txtTags.UniqueID].Trim().ToLower() != "<keep>")
        //            {
        //                // string[] inputTags = uHelper.SplitString(txtTags.Text.Trim(), new string[] { ",", ";", " " });
        //                string[] inputTags = uHelper.SplitString(Convert.ToString(Request.Form[txtTags.UniqueID]).Trim(), new string[] { ",", ";", " " });
        //                string formattedTags = string.Empty;

        //                for (int i = 0; i <= inputTags.Length - 1; i++)
        //                {
        //                    formattedTags = formattedTags + inputTags[i] + ",";
        //                }

        //                documentInfoItem[DatabaseObjects.Columns.Tags] = formattedTags;

        //                //Check all the entered tag is in TagTable if its not Add that tag in TagsTable.
        //                if (Request.Form[txtTags.UniqueID].Trim() != string.Empty)
        //                {
        //                    string[] tags = uHelper.SplitString(documentInfoItem[DatabaseObjects.Columns.Tags], new string[] { ",", ";", " " });
        //                    // Get the Tag tabel
        //                    SPList tagList = oWeb.Lists[DatabaseObjects.Lists.Tags];
        //                    foreach (string tag in tags)
        //                    {
        //                        SPQuery tagQuery = new SPQuery();
        //                        tagQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.TagName, tag);
        //                        SPListItemCollection tagsCollection = tagList.GetItems(tagQuery);
        //                        if (tagsCollection.Count == 0)
        //                        {
        //                            oWeb.AllowUnsafeUpdates = true;
        //                            SPListItem newTagItem = tagList.AddItem();
        //                            newTagItem[DatabaseObjects.Columns.TagName] = tag;
        //                            newTagItem.Update();
        //                            oWeb.AllowUnsafeUpdates = false;
        //                        }
        //                    }
        //                }
        //            }

        //            documentInfoItem[DatabaseObjects.Columns.Readers] = string.Empty;

        //            // Update the document info item.
        //            oWeb.AllowUnsafeUpdates = true;
        //            documentInfoItem.Update();
        //            oWeb.AllowUnsafeUpdates = false;

        //            //Update Cache Data
        //            DMCache.UpdateCacheData(documentInfoItem.ID, newFile, oWeb);
        //            DMCache.UpdateFolderCacheFileCount(DocName, Convert.ToString(file.ParentFolder.UniqueId));

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteException(ex);
        //        valid = false;
        //    }

        //    return valid;
        //}

        ///// <summary>
        ///// This function checks the No. of file allowed inn the folder.
        ///// </summary>
        ///// <param name="oFolder"></param>
        ///// <returns></returns>
        //private Boolean CheckNoOfFileAllowedInFolder(SPFolder oFolder)
        //{
        //    SPListItem folder = oFolder.Item;
        //    Boolean valid = true;
        //    int allowedNoOfFile = Convert.ToInt32(folder[DatabaseObjects.Columns.NumFiles]);

        //    SPWeb oWeb = SPContext.Current.Web;

        //    //Find the documents under the current folder.
        //    SPList documentInfoList = oWeb.Lists[DatabaseObjects.Lists.DocumentInfoList];

        //    SPQuery docQuery = new SPQuery();
        //    docQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.FolderName, oFolder.Url);
        //    SPListItemCollection docInfoItems = documentInfoList.GetItems(docQuery);

        //    DataTable docTable = docInfoItems.GetDataTable();
        //    int existNoOfFile = 0;
        //    // Distinct the document by DocumemtId so that all version of same doc will be count as one file.
        //    if (docTable != null && docTable.Rows.Count > 0)
        //    {
        //        var docCount = (from r in docTable.AsEnumerable()
        //                        select r[DatabaseObjects.Columns.DocumentID]).Distinct().ToList();
        //        existNoOfFile = docCount.Count;
        //    }

        //    if (allowedNoOfFile == 0)
        //        return true;
        //    else
        //    {
        //        if (existNoOfFile < allowedNoOfFile)
        //            valid = true;
        //        else
        //            valid = false;
        //    }
        //    return valid;
        //}
    }
}