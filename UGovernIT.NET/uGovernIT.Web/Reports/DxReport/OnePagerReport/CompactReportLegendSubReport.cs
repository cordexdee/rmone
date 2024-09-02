using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using uGovernIT.Manager.Entities;
using uGovernIT.Utility;
namespace uGovernIT.DxReport
{
    public partial class CompactReportLegendSubReport : DevExpress.XtraReports.UI.XtraReport
    {
        public CompactReportLegendSubReport(ProjectCompactReportEntity entity)
        {
            InitializeComponent();
            if (entity.ProjectMonitorHealth != null && entity.ProjectMonitorHealth.Rows.Count > 0)
            {
                xrMonitorOpt.DataBindings.Add("Text", null,DatabaseObjects.Columns.ModuleMonitorName);
                this.DataSource = entity.ProjectMonitorHealth;
                xrdrawColor.BeforePrint += xrTableCell3_BeforePrint;
            }
        }

        void xrTableCell3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell currentOne = sender as XRTableCell;
            currentOne.Text = string.Empty;
            string colorClass = Convert.ToString(GetCurrentColumnValue(DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup));
            if (colorClass.EndsWith("LED_Red.png"))
                colorClass = "RedLED";
            else if (colorClass.EndsWith("LED_Yellow.png"))
                colorClass = "YellowLED";
            else if (colorClass.EndsWith("LED_Green.png"))
                colorClass = "GreenLED";

            switch (colorClass) 
            {
                case "RedLED":
                    currentOne.BackColor = Color.Red;
                    break;
                case "YellowLED":
                    currentOne.BackColor = Color.Yellow;
                    break;
                case "GreenLED":
                    currentOne.BackColor = Color.Green;
                    break;
                default:
                    break;
            }
        }
    }
}
