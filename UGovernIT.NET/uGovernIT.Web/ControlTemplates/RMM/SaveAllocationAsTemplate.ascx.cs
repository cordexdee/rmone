using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.RMM
{
    public partial class SaveAllocationAsTemplate : System.Web.UI.UserControl
    {
        public string ProjectID { get; set; }
        public string ModuleName { get; set; }
        public string PopupID { get; set; }
        public DateTime PreconStartDate { get; set; }
        public DateTime PreconEndDate { get; set; }
        public DateTime ConstStartDate { get; set; }
        public DateTime ConstEndDate { get; set; }
        public DateTime CloseOutStartDate { get; set; }
        public DateTime CloseOutEndDate { get; set; }

        protected string ajaxHelper = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/ajaxhelper.aspx");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Request["ProjectID"]))
                ProjectID = Request["ProjectID"].Trim();

            if (!string.IsNullOrWhiteSpace(Request["ModuleName"]))
                ModuleName = Request["ModuleName"].Trim();

            if (!string.IsNullOrWhiteSpace(Request["PopupID"]))
                PopupID = Request["PopupID"].Trim();

            DataRow ticketRow = Ticket.GetCurrentTicket(HttpContext.Current.GetManagerContext(), ModuleName, ProjectID);
            if (ticketRow != null)
            {
                // Start date logic.
                if (ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.PreconStartDate))
                    dteStartDate.Date = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.PreconStartDate]);
                
                if (dteStartDate.Date == DateTime.MinValue && ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.EstimatedConstructionStart))
                    dteStartDate.Date = UGITUtility.GetObjetToDateTime(ticketRow[DatabaseObjects.Columns.EstimatedConstructionStart]);
                
                if (dteStartDate.Date == DateTime.MinValue && ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketTargetStartDate))
                    dteStartDate.Date = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.TicketTargetStartDate]);
                
                if (dteStartDate.Date == DateTime.MinValue && ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                    dteStartDate.Date = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.TicketCreationDate]);

                // End Date Logic.
                if (ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.CloseoutDate) && ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.CloseoutStartDate))
                {
                    DateTime closeOutEndDate = UGITUtility.GetObjetToDateTime(ticketRow[DatabaseObjects.Columns.CloseoutDate]);
                    DateTime closeOutStartDate = UGITUtility.GetObjetToDateTime(ticketRow[DatabaseObjects.Columns.CloseoutStartDate]);
                    if (closeOutEndDate != DateTime.MinValue)
                    {
                        dteEndDate.Date = closeOutEndDate;
                    }
                    else if (closeOutStartDate != DateTime.MinValue)
                    {
                        dteEndDate.Date = closeOutStartDate.AddWorkingDays(uHelper.getCloseoutperiod(HttpContext.Current.GetManagerContext()));
                    }
                }

                if (dteEndDate.Date == DateTime.MinValue && ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.EstimatedConstructionEnd))
                    dteEndDate.Date = UGITUtility.GetObjetToDateTime(ticketRow[DatabaseObjects.Columns.EstimatedConstructionEnd]);

                if(dteEndDate.Date == DateTime.MinValue && ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketTargetCompletionDate))
                    dteEndDate.Date = UGITUtility.GetObjetToDateTime(ticketRow[DatabaseObjects.Columns.TicketTargetCompletionDate]);

                int projectduration = 0;
                if (ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.ProjectDuration))
                    projectduration = UGITUtility.StringToInt(ticketRow[DatabaseObjects.Columns.ProjectDuration]);
                if (ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.CRMDuration))
                    projectduration = UGITUtility.StringToInt(ticketRow[DatabaseObjects.Columns.CRMDuration]);
                txtDuration.Text = projectduration.ToString();

                if (dteEndDate.Date == DateTime.MinValue)
                {
                    if (projectduration <= 0)
                        dteEndDate.Date = dteStartDate.Date;
                    else
                        dteEndDate.Date = dteStartDate.Date.AddDays(projectduration * 7);
                }
                if (ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.PreconStartDate))
                    PreconStartDate = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.PreconStartDate]);
                if (ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.PreconEndDate))
                    PreconEndDate = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.PreconEndDate]);
                if (ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.EstimatedConstructionStart))
                    ConstStartDate = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.EstimatedConstructionStart]);
                if (ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.EstimatedConstructionEnd))
                    ConstEndDate = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                if (ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.CloseoutStartDate))
                    CloseOutStartDate = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.CloseoutStartDate]);
                if (ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.CloseoutDate))
                    CloseOutEndDate = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.CloseoutDate]);
            }
        }
    }
}