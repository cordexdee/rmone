using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Helpers;
using uGovernIT.Web.Helpers;
using System.Data;
using uGovernIT.Web.ControlTemplates.Shared;
using uGovernIT.Web.ControlTemplates.uGovernIT.PMMProject;
using uGovernIT.Web.ControlTemplates.uGovernIT;
using uGovernIT.Web.ControlTemplates.RMM;

namespace uGovernIT.Web.ControlTemplates.PMM
{
    public partial class ProjectCompactView : System.Web.UI.UserControl
    {
        protected string fullViewUrl;
        protected string ticketID;
        protected string projectTitle;
        protected string moduleName = "PMM";
        protected string headerText;
        protected string ctrlName = string.Empty;
        DataRow ticketItem;
        Ticket projectTicket;
        protected void Page_Init(object sender, EventArgs e)
        {
            ticketID = Request["TicketId"];
            projectTicket = new Ticket(Context.GetManagerContext(), moduleName);
            fullViewUrl = string.Format("{0}?TicketID={1}&isudlg=1", UGITUtility.GetAbsoluteURL(projectTicket.Module.StaticModulePagePath), ticketID);

            ticketItem = Ticket.GetCurrentTicket(Context.GetManagerContext(), moduleName, ticketID);
            if (ticketItem == null)
                return;

            projectTitle = Convert.ToString(ticketItem[DatabaseObjects.Columns.Title]);
            if (string.IsNullOrWhiteSpace(Convert.ToString(Session["ProjectSummaryDefaultCtrl"])))
                Session["ProjectSummaryDefaultCtrl"] = "uGovernIT.NewProjectTask";

            ctrlName = Convert.ToString(Session["ProjectSummaryDefaultCtrl"]);
            loadControl(ctrlName);

            //TaskWorkflow.TicketID = ticketID;
           // TaskWorkflow.ModuleName = moduleName;
           
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cbpMainPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            contentView.Controls.Clear();
            loadControl(e.Parameter);
        }

        private void loadControl(string controlName)
        {
            Control ctrl = Page.loadUGITControl(controlName);
            if (ctrl != null)
            {
                ctrl.ID = controlName;
                Session["ProjectSummaryDefaultCtrl"] = controlName;
                if (ctrl is History)
                {
                    History sCtrl = ctrl as History;
                    sCtrl.ListName = projectTicket.Module.ModuleTable;
                    sCtrl.TicketId = UGITUtility.ObjectToString(ticketItem[DatabaseObjects.Columns.TicketId]);
                    headerText = "History";
                }
                else if (ctrl is PMMProjectStatus)
                {
                    PMMProjectStatus sCtrl = ctrl as PMMProjectStatus;
                    sCtrl.PMMID = UGITUtility.StringToInt(ticketItem[DatabaseObjects.Columns.ID]);
                    headerText = "Risk";
                }
                else if (ctrl is CustomTicketRelationShip)
                {
                    CustomTicketRelationShip sCtrl = ctrl as CustomTicketRelationShip;
                    sCtrl.LoadData = true;
                    sCtrl.TicketId = ticketID;
                    sCtrl.ParentDetailOnDemand = false;
                    sCtrl.ChildDetailOnDemand = false;
                    sCtrl.ModuleName = moduleName;
                    headerText = "Related Ticket";
                }
                //else if (ctrl is CRMProjectAllocationViewNew)
                //{
                //    CRMProjectAllocationViewNew sCtrl = ctrl as CRMProjectAllocationViewNew;
                //    sCtrl.ticketID = ticketID;
                //    sCtrl.ModuleName = moduleName;
                //    headerText = "Resource";
                //}
                else if (ctrl is CRMProjectAllocationViewNew)
                {
                    //ProjectResourceDetail sCtrl = Page.loadUGITControl(controlName) as ProjectResourceDetail;
                    ProjectResourceDetail sCtrl = (ProjectResourceDetail)this.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ProjectResourceDetail.ascx");
                    //ProjectResourceDetail sCtrl = ctrl as ProjectResourceDetail;
                    sCtrl.ProjectPublicID = ticketID;
                    sCtrl.Module= moduleName;
                    headerText = "Resource";
                    contentView.Controls.Add(sCtrl);
                    return;
                }
                else if (ctrl is NewProjectTask)
                {
                    NewProjectTask sCtrl = ctrl as NewProjectTask;
                    sCtrl.TicketID = ticketID;
                    sCtrl.ModuleName = moduleName;
                    sCtrl.IsCompactView = true;
                    headerText = "Task";
                }

                contentView.Controls.Add(ctrl);
            }
        }
    }
}