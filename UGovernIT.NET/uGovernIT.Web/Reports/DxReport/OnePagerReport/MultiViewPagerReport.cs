using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using uGovernIT.Manager.Entities;
using System.Data;

namespace uGovernIT.DxReport
{
    public partial class MultiViewPagerReport : DevExpress.XtraReports.UI.XtraReport
    {
        ProjectCompactReportEntity _prEntity;
        public MultiViewPagerReport(ProjectCompactReportEntity prEntity)
        {
            InitializeComponent();
            _prEntity = prEntity;
            xrlblTicketId.DataBindings.Add("Text", null, "TicketId");
            xrlblTicketId.Visible = false;
            Report.DataSource = prEntity.ProjectDetails;

        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (xrlblTicketId.Text == "TicketId")
            {
                return;
            }
            ProjectCompactReportEntity entity = new ProjectCompactReportEntity();
            string ticketId = xrlblTicketId.Text;
            DataTable dt = new DataTable();
            DataRow[] drs = null;

            drs = _prEntity.ProjectDetails.Select("TicketId='" + ticketId + "'");
            if (drs.Length > 0) entity.ProjectDetails = drs.CopyToDataTable();

            if (_prEntity.ProjectMonitorHealth != null && _prEntity.ProjectMonitorHealth.Rows.Count>0)
            {
                dt = _prEntity.ProjectMonitorHealth.Copy();
                drs = dt.Select("TicketId='" + ticketId + "'");
                if (drs.Length > 0) entity.ProjectMonitorHealth = drs.CopyToDataTable();
            }

            if (_prEntity.AccomPlanned != null && _prEntity.AccomPlanned.Rows.Count > 0)
            {
                dt = _prEntity.AccomPlanned.Copy();
                drs = dt.Select("TicketId='" + ticketId + "'");
                if (drs.Length > 0) entity.AccomPlanned = drs.CopyToDataTable();
            }

            if (_prEntity.ImediatePlanned != null && _prEntity.ImediatePlanned.Rows.Count>0)
            {
                dt = _prEntity.ImediatePlanned.Copy();
                drs = dt.Select("TicketId='" + ticketId + "'");
                if (drs.Length > 0) entity.ImediatePlanned = drs.CopyToDataTable();
            }

            if (_prEntity.PMMRisks != null && _prEntity.PMMRisks.Rows.Count > 0)
            {
                dt = _prEntity.PMMRisks.Copy();
                drs = dt.Select("TicketId='" + ticketId + "'");
                if (drs.Length > 0) entity.PMMRisks = drs.CopyToDataTable();
            }

            if (_prEntity.PMMIssues != null && _prEntity.PMMIssues.Rows.Count > 0)
            {
                dt = _prEntity.PMMIssues.Copy();
                drs = dt.Select("TicketId='" + ticketId + "'");
                if (drs.Length > 0) entity.PMMIssues = drs.CopyToDataTable();
            }

            if (_prEntity.MileStone != null && _prEntity.MileStone.Rows.Count>0)
            {
                dt = _prEntity.MileStone.Copy();

                drs = dt.Select("TicketId='" + ticketId + "'");
                if (drs.Length > 0) entity.MileStone = drs.CopyToDataTable();
            }

            ProjectCompactReport pr = new ProjectCompactReport(entity);
            xrSubreport1.ReportSource = pr;
        }

    }
}
