using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using uGovernIT.Web.ControlTemplates.GlobalPage;
using uGovernIT.Web.ControlTemplates.Shared;
using uGovernIT.Web.ControlTemplates.uGovernIT;
using uGovernIT.Web.ControlTemplates.uGovernIT.PMMProject;

namespace uGovernIT.Web
{
    public partial class ProjectManagement : UPage
    {
        protected string frameId = string.Empty;
        protected bool printEnable;
        public string AssemblyVersion = string.Empty;
        protected override void OnInit(EventArgs e)
        {
            AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            int pmmID = 0;
            int.TryParse(Request["PMMID"], out pmmID);
            frameId = Request["frameObjId"];
            int nprId = 0;
            int.TryParse(Request["NPRID"], out nprId);
            frameId = Request["frameObjId"];
            string control = Request["control"];
            bool isReadOnly = true;
            bool.TryParse(Request["IsReadOnly"], out isReadOnly);
            bool showBaseline = UGITUtility.StringToBoolean(Request["showBaseline"]);
            double baselineNum = Convert.ToDouble(Request["baselineNum"]);

            // In case of null it will become false;
            bool onlyPendingBudget = UGITUtility.StringToBoolean(Request["OnlyPendingBudget"]);

            if (Request["enablePrint"] != null)
            {
                printEnable = true;
                isReadOnly = true;
            }

            if (control != null && control.Trim() != string.Empty)
            {
                control = control.ToLower();
                switch (control)
                {
                    case "nprbudget":
                        NPRBudget nprBudget = (NPRBudget)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/NPRBudget.ascx");
                        nprBudget.nprId = int.Parse(nprId.ToString());
                        nprBudget.FrameId = frameId;
                        nprBudget.ReadOnly = isReadOnly;
                        managementControls.Controls.Add(nprBudget);
                        break;
                    case "pmmtaskslist":
                        //ControlTemplates.uGovernIT.ProjectTasks taskCtr = (ControlTemplates.uGovernIT.ProjectTasks)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ProjectTasks.ascx");
                        //string pmmPublicID = Request["ticketid"];
                        //taskCtr.PMMID = pmmPublicID;
                        //taskCtr.IsReadOnly = isReadOnly;
                        //taskCtr.FrameId = frameId;
                        //taskCtr.ShowBaseline = showBaseline;
                        //taskCtr.BaselineNum = baselineNum;
                        //managementControls.Controls.Add(taskCtr);

                        break;
                    case "projectstatusdetail":
                        PMMProjectStatus pStatus = (PMMProjectStatus)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/PMMProject/PMMProjectStatus.ascx");
                        pStatus.PMMID = pmmID;
                        pStatus.FrameId = frameId;
                        pStatus.IsReadOnly = isReadOnly;
                        pStatus.IsShowBaseline = showBaseline;
                        pStatus.BaselineId = baselineNum;
                        managementControls.Controls.Add(pStatus);
                        break;
                    case "projectbudgetdetail":
                        //PMMBudgetList pBudget = (PMMBudgetList)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/PMMBudgetList.ascx");
                        //pBudget.PMMID = pmmID;
                        //pBudget.FrameId = frameId;
                        //pBudget.IsReadOnly = isReadOnly;
                        //pBudget.ShowBaseline = showBaseline;
                        //pBudget.BaselineNum = baselineNum;
                        //managementControls.Controls.Add(pBudget);
                        break;
                    case "projectvariancereport":
                        PMMVarianceReport pVariances = (PMMVarianceReport)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/PMMVarianceReport.ascx");
                        pVariances.PMMID = pmmID;
                        pVariances.FrameId = frameId;
                        pVariances.IsReadOnly = isReadOnly;
                        managementControls.Controls.Add(pVariances);
                        break;
                    case "projectpendingbudgetdetail":
                        //PMMBudgetList pmmPendingBudget = (PMMBudgetList)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/PMMBudgetList.ascx");
                        //pmmPendingBudget.PMMID = pmmID;
                        //pmmPendingBudget.FrameId = frameId;
                        //pmmPendingBudget.IsReadOnly = isReadOnly;
                        //pmmPendingBudget.ShowBaseline = showBaseline;
                        //pmmPendingBudget.BaselineNum = baselineNum;
                        //pmmPendingBudget.OnlyPendingBudget = onlyPendingBudget;
                        //managementControls.Controls.Add(pmmPendingBudget);
                        break;
                    case "projectallstatussummary":
                        ProjectAllStatusSummary statusSummaries = (ProjectAllStatusSummary)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/PMMProject/ProjectAllStatusSummary.ascx");
                        statusSummaries.ProjectID = Request["ticketid"];
                        managementControls.Controls.Add(statusSummaries);
                        break;

                    case "itgbudgetmanagement":
                        ITGBudgetManagement pBudgetManagement = (ITGBudgetManagement)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Budget/ITGBudgetManagement.ascx");
                        pBudgetManagement.FrameId = frameId;
                        pBudgetManagement.IsReadOnly = isReadOnly;
                        managementControls.Controls.Add(pBudgetManagement);
                        break;
                    case "governancereview":
                        GovernanceReview pGovernanceReview = (GovernanceReview)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Budget/GovernanceReview.ascx");
                        pGovernanceReview.FrameId = frameId;
                        pGovernanceReview.IsReadOnly = isReadOnly;
                        managementControls.Controls.Add(pGovernanceReview);
                        break;
                    case "itgcommitteereview":
                        //ITGCommitteeReview pITGCommitteeReview = (ITGCommitteeReview)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/ITGCommitteeReview.ascx");
                        //pITGCommitteeReview.FrameId = frameId;
                        //pITGCommitteeReview.IsReadOnly = isReadOnly;
                        //managementControls.Controls.Add(pITGCommitteeReview);
                        break;
                    case "itgovernancereview":
                        //ITGovernanceReview pITGovernanceReview = (ITGovernanceReview)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/ITGovernanceReview.ascx");
                        //pITGovernanceReview.FrameId = frameId;
                        //pITGovernanceReview.IsReadOnly = isReadOnly;
                        //managementControls.Controls.Add(pITGovernanceReview);
                        break;
                    case "itgportfolio":
                        ITGPortfolio pITGPortfolio = (ITGPortfolio)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Budget/ITGPortfolio.ascx");
                        pITGPortfolio.FrameId = frameId;
                        pITGPortfolio.IsReadOnly = isReadOnly;
                        managementControls.Controls.Add(pITGPortfolio);
                        break;
                    case "history":
                        History history = (History)Page.LoadControl("~/CONTROLTEMPLATES/Shared/History.ascx");
                        history.FrameId = frameId;
                        history.ReadOnly = isReadOnly;
                        //int ticketId = 0; // history var changed to string to hold ticketid column not id
                        //int.TryParse(Request["ticketId"], out ticketId);
                        string listName = Request["listName"];
                        history.TicketId = UGITUtility.ObjectToString(Request["ticketId"]);
                        history.ListName = listName;
                        managementControls.Controls.Add(history);
                        break;
                    case "itgbudgeteditor":
                        ITGBudgetEditor itgBudgetEditor = (ITGBudgetEditor)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Budget/ITGBudgetEditor.ascx");
                        itgBudgetEditor.FrameId = frameId;
                        itgBudgetEditor.IsReadOnly = isReadOnly;
                        managementControls.Controls.Add(itgBudgetEditor);
                        break;
                    case "projectresourcedetail":
                        ProjectResourceDetail resourceDetail = (ProjectResourceDetail)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ProjectResourceDetail.ascx");
                        resourceDetail.ProjectPublicID = Request["projectPublicID"];
                        resourceDetail.Module = Request["Module"];
                        resourceDetail.ReadOnly = isReadOnly;
                        managementControls.Controls.Add(resourceDetail);
                        break;
                    case "projectresourceupdate":
                        ProjectResourceUpdate resourceUpdate = (ProjectResourceUpdate)Page.LoadControl("~/CONTROLTEMPLATES/PMM/ProjectResourceUpdate.ascx");
                        resourceUpdate.PublicID = Request["projectID"];
                        resourceUpdate.Module = Request["Module"];
                        resourceUpdate.ReadOnly = isReadOnly;
                        int resourceID = 0;
                        int.TryParse(Request["ItemID"], out resourceID);
                        resourceUpdate.ItemID = resourceID;
                        managementControls.Controls.Add(resourceUpdate);
                        break;

                    case "pmmeventscalendar":
                        //PMMEventsCalendar pmmEventsCalendar = (PMMEventsCalendar)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/PMMProject/PMMEventsCalendar.ascx");
                        ////configurationVariableListEdit.clientAdminID = clientAdminID;
                        //pmmEventsCalendar.ProjectPublicID = Request["projectPublicID"];
                        //pmmEventsCalendar.ReadOnly = isReadOnly;
                        //managementControls.Controls.Add(pmmEventsCalendar);
                        break;

                    case "ticketemails":
                        //TicketEmails ticketemails = (TicketEmails)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/TicketEmails.ascx");
                        ////configurationVariableListEdit.clientAdminID = clientAdminID;
                        //ticketemails.PublicTicketID = Request["PublicTicketID"];
                        //ticketemails.ReadOnly = isReadOnly;
                        //managementControls.Controls.Add(ticketemails);
                        break;
                    case "pmmsprints":
                        PMMSprints pmmSprints = (PMMSprints)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/PMMSprints.ascx");
                        pmmSprints.ticketId = Request["PublicTicketID"];
                        pmmSprints.ReadOnly = isReadOnly;
                        pmmSprints.ModuleName = "PMM";
                        pmmSprints.Iframe = frameId;

                        managementControls.Controls.Add(pmmSprints);
                        break;
                   
                    case "pmmaddsprinttask":
                        PMMSprintAdd pmmaddsprinttask = (PMMSprintAdd)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/PMMSprintAdd.ascx");
                        pmmaddsprinttask.ticketId = Request["PublicTicketID"];
                        pmmaddsprinttask.IsNew = UGITUtility.StringToBoolean(Request["IsNew"]);
                        pmmaddsprinttask.TaskId = (!string.IsNullOrEmpty(Request["TaskId"])) ? UGITUtility.StringToInt(Request["TaskId"]) : 0;
                        pmmaddsprinttask.ReadOnly = isReadOnly;
                        pmmaddsprinttask.Iframe = Request["ParentIframeId"];
                        managementControls.Controls.Add(pmmaddsprinttask);
                        break;

                    case "pmmaddeditsprint":

                        PmmAddEditSprint pmmaddeditsprint = (PmmAddEditSprint)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/PmmAddEditSprint.ascx");
                        pmmaddeditsprint.ticketId = Request["PublicTicketID"];
                        pmmaddeditsprint.IsNew = UGITUtility.StringToBoolean(Request["IsNew"]);
                        pmmaddeditsprint.SprintID = (!string.IsNullOrEmpty(Request["sprintID"])) ? UGITUtility.StringToInt(Request["sprintID"]) : 0;
                        pmmaddeditsprint.ReadOnly = isReadOnly;
                        pmmaddeditsprint.Iframe = Request["ParentIframeId"];
                        pmmaddeditsprint.SprintTitle = Request["sprintTitle"];

                        managementControls.Controls.Add(pmmaddeditsprint);
                        break;
                    case "pmmaddeditrelease":

                        PMMAddEditRelease pmmAddeditRelease = (PMMAddEditRelease)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/PMMAddEditRelease.ascx");
                        pmmAddeditRelease.ticketId = Request["PublicTicketID"];
                        pmmAddeditRelease.IsNew = UGITUtility.StringToBoolean(Request["IsNew"]);
                        pmmAddeditRelease.ReleaseID = (!string.IsNullOrEmpty(Request["releaseID"])) ? UGITUtility.StringToInt(Request["releaseID"]) : 0;
                        pmmAddeditRelease.ReadOnly = isReadOnly;
                        pmmAddeditRelease.Iframe = Request["ParentIframeId"];

                        managementControls.Controls.Add(pmmAddeditRelease);
                        break;

                    default:
                        break;
                }
            }

            Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext()?.TenantAccountId}|{HttpContext.Current.CurrentUser()?.Name}: Visited Page: ProjectManagement.aspx");

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}