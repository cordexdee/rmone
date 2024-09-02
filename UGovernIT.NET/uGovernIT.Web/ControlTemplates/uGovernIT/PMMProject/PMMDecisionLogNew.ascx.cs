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

namespace uGovernIT.Web
{
    public partial class PMMDecisionLogNew : UserControl
    {
        public string ProjectID { get; set; }
        string moduleName = "PMM";
        List<DecisionLog> decisionLogList;
        DecisionLogManager DecisionLogMGR = new DecisionLogManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            decisionLogList = DecisionLogMGR.Load();  //  SPListHelper.GetSPList(DatabaseObjects.Lists.DecisionLog);
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btSaveTask_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (decisionLogList != null)
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

                DecisionLog item = new DecisionLog();

                item.ModuleName = moduleName;
                item.TicketId = ProjectID;
                if(dtReleaseDate.Value == null)
                {
                    item.ReleaseDate = null;
                }
                else
                {
                    item.ReleaseDate = (DateTime)dtReleaseDate.Date;
                }
                if (dtDateAssigned.Value == null)
                {
                    item.DateAssigned = null;
                }
                else
                {
                    item.DateAssigned = (DateTime)dtDateAssigned.Date;
                }

                if (dtDecisionDate.Value == null)
                {
                    item.DecisionDate = null;
                }
                else
                {
                    item.DecisionDate = (DateTime)dtDecisionDate.Date;
                }

                if (dtDateIdentified.Value == null)
                {
                    item.DateIdentified = null;
                }
                else
                {
                    item.DateIdentified = (DateTime)dtDateIdentified.Date;
                }
               

                item.ItemOrder = UGITUtility.StringToInt(txtItemOrder.Text);
                item.ReleaseID = txtReleaseID.Text;
                item.Description = txtDescription.Text;
                item.Title = txtReleaseTitle.Text;
               
                item.DecisionStatus = ddlDecisionStatus.SelectedValue;
                item.DecisionSource = txtDecisionSource.Text;
                item.AssignedTo = peAssignedTo.GetValues();
                item.DecisionMaker = peDecisionMaker.GetValues();
               
                item.Decision = txtDecision.Text;
                item.AdditionalComments = txtAdditionalComments.Text;
                DecisionLogMGR.Insert(item);  // item.Update();
            }
            uHelper.ClosePopUpAndEndResponse(Context, true, "control=projectstatusdetail");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
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
            if (!string.IsNullOrEmpty(peDecisionMaker.GetValues()))
                args.IsValid = true;
            else
                args.IsValid = false;
        }
    }
}
