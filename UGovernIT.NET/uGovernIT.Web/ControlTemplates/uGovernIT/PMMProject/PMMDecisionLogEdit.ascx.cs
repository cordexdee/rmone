using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using System.Web;
using System.IO;
using DevExpress.Web;
using System.Data;
using System.Collections;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DMSDB;
using uGovernIT.DMS.Amazon;

namespace uGovernIT.Web
{
    public partial class PMMDecisionLogEdit : UserControl
    {
        public string ProjectID { get; set; }
        public int decisionlogID { get; set; }
        public int decisionLogViewType { get; set; }
        string moduleName = "PMM";

        DecisionLog decisionLogItem;
        private DMSManagerService _dmsManagerService = null;
        private ApplicationContext _context = null;
        DecisionLogManager DecisionLogMGR = new DecisionLogManager(HttpContext.Current.GetManagerContext());
        UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
        StringBuilder linkFile = new StringBuilder();

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

        protected DMSManagerService DMSManagerService
        {
            get
            {
                if (_dmsManagerService == null)
                {
                    _dmsManagerService = new DMSManagerService(ApplicationContext);
                }
                return _dmsManagerService;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnInit(EventArgs e)
        {

            List<string> peopleNames = new List<string>();
            List<string> peopleDisplayName = new List<string>();
            decisionLogItem = DecisionLogMGR.LoadByID(decisionlogID);   // SPListHelper.GetSPListItem(DatabaseObjects.Lists.DecisionLog, decisionlogID);
            if (decisionLogItem != null)
            {
                bool archiveStatus = UGITUtility.StringToBoolean(decisionLogItem.Deleted);
                if (archiveStatus)
                {
                    btDelete.Visible = true;
                    btUnArchiveDecisionLog.Visible = true;
                }
                else
                {
                    btArchiveDecisionLog.Visible = true;
                }

                dtReleaseDate.Date = UGITUtility.StringToDateTime(decisionLogItem.ReleaseDate);
                txtItemOrder.Text = Convert.ToString(decisionLogItem.ItemOrder);
                txtReleaseID.Text = Convert.ToString(decisionLogItem.ReleaseID);
                txtReleaseTitle.Text = Convert.ToString(decisionLogItem.Title);
                txtDescription.Text = Convert.ToString(decisionLogItem.Description);
                dtDateIdentified.Date = UGITUtility.StringToDateTime(decisionLogItem.DateIdentified);
                ddlDecisionStatus.SelectedValue = Convert.ToString(decisionLogItem.DecisionStatus);
                txtDecisionSource.Text = Convert.ToString(decisionLogItem.DecisionSource);

                //SPFieldUserValueCollection userValues = new SPFieldUserValueCollection(SPContext.Current.Web, Convert.ToString(uHelper.GetSPItemValue(decisionLogItem, DatabaseObjects.Columns.UGITAssignedTo)));
                //if (userValues != null && userValues.Count > 0)
                //peAssignedTo.UpdateEntities(uHelper.getUsersListFromCollection(userValues));

                //SPFieldUserValueCollection decisionMaker = new SPFieldUserValueCollection(SPContext.Current.Web, Convert.ToString(uHelper.GetSPItemValue(decisionLogItem, DatabaseObjects.Columns.DecisionMaker)));
                //if (decisionMaker != null && decisionMaker.Count > 0)
                //    peDecisionMaker.UpdateEntities(uHelper.getUsersListFromCollection(decisionMaker));

                // if (decisionLogItem.AssignedTo != null && UserManager.GetUserInfosById(decisionLogItem.AssignedTo).Count > 0)
                // {
                foreach (UserProfile userV in UserManager.GetUserInfosById(decisionLogItem.AssignedTo))
                {
                    if (userV != null)
                    {
                        peopleDisplayName.Add(userV.UserName);
                        peopleNames.Add(userV.UserName);
                    }
                }
                if (!string.IsNullOrEmpty(peAssignedTo.GetValues()))
                {
                    decisionLogItem.AssignedTo = peAssignedTo.GetValues();
                }
                peAssignedTo.SetValues(decisionLogItem.AssignedTo);
                // }

                foreach (UserProfile userV in UserManager.GetUserInfosById(decisionLogItem.DecisionMaker))
                {
                    if (userV != null)
                    {
                        peopleDisplayName.Add(userV.UserName);
                        peopleNames.Add(userV.UserName);
                    }
                }
                if (!string.IsNullOrEmpty(peDecisionMaker.GetValues()))
                {
                    decisionLogItem.DecisionMaker = peDecisionMaker.GetValues();
                }
                peDecisionMaker.SetValues(decisionLogItem.DecisionMaker);

                dtDateAssigned.Date = UGITUtility.StringToDateTime(decisionLogItem.DateAssigned);
                txtDecision.Text = Convert.ToString(decisionLogItem.Decision);
                dtDecisionDate.Date = UGITUtility.StringToDateTime(decisionLogItem.DecisionDate);
                txtAdditionalComments.Text = Convert.ToString(decisionLogItem.AdditionalComments);

                if (!string.IsNullOrEmpty(decisionLogItem.Attachments))
                {
                    List<DMSDocument> attachedFileList = DMSManagerService.GetFileListByFileId(decisionLogItem.Attachments);

                    foreach (var file in attachedFileList)
                    {
                        linkFile = linkFile.Append($"<a id='file_{file.FileId}' style='cursor: pointer;' onclick='window.downloadSelectedFile({file.FileId})'>{file.FileName}</a><img src='/content/images/close-red.png' id='img_{file.FileId}' class='cancelUploadedFiles' onclick='window.deleteLinkedDocument(\"" + file.FileId + "\")'></img><br/>");
                    }

                    bindMultipleLink.InnerHtml = Convert.ToString(linkFile);
                }



            }
            base.OnInit(e);
        }

        protected void btSaveDecision_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (decisionLogItem != null)
            {
                //string[] UsersSeperated = peAssignedTo.CommaSeparatedAccounts.Split(',');
                //SPFieldUserValueCollection userValueCollection = new SPFieldUserValueCollection();
                //foreach (PickerEntity resolvedEntity in peAssignedTo.ResolvedEntities)
                //{
                //    SPPrincipalInfo principalInfo = SPUtility.ResolvePrincipal(web, resolvedEntity.Key, SPPrincipalType.All, SPPrincipalSource.UserInfoList, web.SiteUsers, false);
                //    if (principalInfo != null)
                //    {
                //        SPFieldUserValue userFValue = new SPFieldUserValue(web, principalInfo.PrincipalId, principalInfo.DisplayName);
                //        userValueCollection.Add(userFValue);
                //    }
                //}

                //string[] DecisionMaker = peDecisionMaker.CommaSeparatedAccounts.Split(',');
                //SPFieldUserValueCollection decisionMakerCollection = new SPFieldUserValueCollection();
                //foreach (PickerEntity resolvedEntity in peDecisionMaker.ResolvedEntities)
                //{
                //    SPPrincipalInfo principalInfo = SPUtility.ResolvePrincipal(web, resolvedEntity.Key, SPPrincipalType.All, SPPrincipalSource.UserInfoList, web.SiteUsers, false);
                //    if (principalInfo != null)
                //    {
                //        SPFieldUserValue userFValue = new SPFieldUserValue(web, principalInfo.PrincipalId, principalInfo.DisplayName);
                //        decisionMakerCollection.Add(userFValue);
                //    }
                //}

                if (dtReleaseDate.Value == null)
                {
                    decisionLogItem.ReleaseDate = null;
                }
                else
                {
                    decisionLogItem.ReleaseDate = (DateTime)dtReleaseDate.Date;
                }
                if (dtDateAssigned.Value == null)
                {
                    decisionLogItem.DateAssigned = null;
                }
                else
                {
                    decisionLogItem.DateAssigned = (DateTime)dtDateAssigned.Date;
                }

                if (dtDecisionDate.Value == null)
                {
                    decisionLogItem.DecisionDate = null;
                }
                else
                {
                    decisionLogItem.DecisionDate = (DateTime)dtDecisionDate.Date;
                }

                if (dtDateIdentified.Value == null)
                {
                    decisionLogItem.DateIdentified = null;
                }
                else
                {
                    decisionLogItem.DateIdentified = (DateTime)dtDateIdentified.Date;
                }




                decisionLogItem.ModuleName = moduleName;
                decisionLogItem.TicketId = ProjectID;
                // decisionLogItem.ReleaseDate = (DateTime)dtReleaseDate.Date;
                decisionLogItem.ItemOrder = Convert.ToInt32(txtItemOrder.Text);
                decisionLogItem.ReleaseID = txtReleaseID.Text;
                decisionLogItem.Description = txtDescription.Text;
                decisionLogItem.Title = txtReleaseTitle.Text;
                // decisionLogItem.DateIdentified = (DateTime)dtDateIdentified.Date;
                decisionLogItem.DecisionStatus = ddlDecisionStatus.SelectedValue;
                decisionLogItem.DecisionSource = txtDecisionSource.Text;
                decisionLogItem.AssignedTo = peAssignedTo.GetValues();
                decisionLogItem.DecisionMaker = peDecisionMaker.GetValues();
                // decisionLogItem.DateAssigned = (DateTime)dtDateAssigned.Date;
                // decisionLogItem.DecisionDate = (DateTime)dtDecisionDate.Date;
                decisionLogItem.Decision = txtDecision.Text;
                decisionLogItem.AdditionalComments = txtAdditionalComments.Text;

                var currentLinkedDocument = Convert.ToString(((HiddenField)UploadAndLinkDocuments.FindControl("fileAttchmentId")).Value);

                if (!string.IsNullOrEmpty(decisionLogItem.Attachments) && !string.IsNullOrEmpty(currentLinkedDocument))
                {
                    var attachment = decisionLogItem.Attachments + "," + string.Join(",", currentLinkedDocument);//check comma

                    decisionLogItem.Attachments = attachment;
                }
                else
                    decisionLogItem.Attachments = currentLinkedDocument;

                DecisionLogMGR.Update(decisionLogItem); 
            }
            uHelper.ClosePopUpAndEndResponse(Context, true, "control=projectstatusdetail");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void btArchiveDecisionLog_Click(object sender, EventArgs e)
        {
            if (decisionLogItem != null)
            {
                decisionLogItem.Deleted = true;
                DecisionLogMGR.Update(decisionLogItem);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        protected void btUnArchiveDecisionLog_Click(object sender, EventArgs e)
        {
            if (decisionLogItem != null)
            {
                decisionLogItem.Deleted = false;
                DecisionLogMGR.Update(decisionLogItem);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            if (decisionLogItem != null)
            {
                DecisionLogMGR.Delete(decisionLogItem);  //decisionLogItem.Delete();
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        protected void cvAssignedTo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(peAssignedTo.GetValues()))
                args.IsValid = true;
            else
                args.IsValid = false;
        }

        protected void cvDecisionMaker_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(peAssignedTo.GetValues()))
                args.IsValid = true;
            else
                args.IsValid = false;
        }
    }
}
