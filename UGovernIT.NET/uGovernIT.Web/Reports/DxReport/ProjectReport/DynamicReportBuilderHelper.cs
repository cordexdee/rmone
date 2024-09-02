using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Shape;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.DxReport
{

    public class DynamicReportBuilderHelper
    {
        const int ReportOffset = 10;
        List<DataSourceDefinition> dsd;
        TSKProjectReportEntity prEntity;
        NPRProjectReportEntity nprprEntity;

        public void GenerateReport(XtraReport r, TSKProjectReportEntity prEntity)
        {
            this.prEntity = prEntity;
            r.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            r.Margins = new System.Drawing.Printing.Margins(3, 6, 4, 33);
            r.Landscape = true;
            r.PaperKind = System.Drawing.Printing.PaperKind.A3;
            r.Version = "12.2";
            r.DataSource = prEntity.Projects;
            dsd = GenerateDataSourceDefinition(prEntity);
            InitBands(r);
            InitDetailsBasedOnXRTableCell(r, dsd);
        }

        public void NPRGenerateReport(XtraReport r, NPRProjectReportEntity prEntity)
        {
            this.nprprEntity = prEntity;
            r.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            r.Margins = new System.Drawing.Printing.Margins(3, 6, 4, 33);
            r.Landscape = true;
            r.PaperKind = System.Drawing.Printing.PaperKind.A3;
            r.Version = "12.2";
            r.DataSource = prEntity.Projects;
            dsd = GenerateDataSourceDefinition(prEntity);
            InitBands(r);
            InitDetailsBasedOnXRTableCell(r, dsd);
        }
        void cell2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string ticketId = ((XRLabel)sender).Text;
            if (string.IsNullOrEmpty(ticketId))
            {
                return;
            }

            foreach (XRTableCell xrTC in ((XRTableRow)((XRLabel)sender).Parent.Parent).Cells)
            {
                if (xrTC.Name == "ShowAccomplishment")
                {
                    //Accomplishment Data
                    if (prEntity.Accomplishment != null && prEntity.Accomplishment.Rows.Count > 0)
                    {
                        var rows = prEntity.Accomplishment.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, ticketId), DatabaseObjects.Columns.AccomplishmentDate + " DESC");
                        if (rows != null && rows.Count() > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            DataTable dtAccomplishment = rows.CopyToDataTable();
                            xrTC.Multiline = true;
                            sb.AppendLine("<span style='font-family: Verdana,Arial,sans-serif;font-size: 8pt;'><br/>");
                            foreach (DataRow row in rows)
                            {
                                DateTime dtAccompDate;

                                if (DateTime.TryParse(Convert.ToString(row[DatabaseObjects.Columns.AccomplishmentDate]), out dtAccompDate))
                                {
                                    sb.AppendLine(string.Format("<b>{0}:</b> {1}", string.Format("{0:MMM-dd-yyyy}", Convert.ToDateTime(row[DatabaseObjects.Columns.AccomplishmentDate])),
                                                                        Convert.ToString(row[DatabaseObjects.Columns.Title])));
                                }
                                else
                                {
                                    sb.AppendLine(string.Format("{0}", Convert.ToString(row[DatabaseObjects.Columns.Title])));
                                }

                                if (prEntity.ShowAccomplishmentDesc)
                                {
                                    sb.AppendLine(string.Format("<br/>{0}", Convert.ToString(row[DatabaseObjects.Columns.ProjectNote])));
                                }
                                sb.AppendLine("<br/><br/>");
                            }
                            sb.AppendLine("</span>");
                            ((XRRichText)xrTC.Controls[0]).Html = sb.ToString();
                        }
                        else
                        {
                            ((XRRichText)xrTC.Controls[0]).Html = string.Empty;
                        }
                        //((XRRichText)xrTC.Controls[0]).SizeF = xrTC.SizeF;
                    }

                }
                else if (xrTC.Name == "ShowPlan")
                {
                    ///Show Immediate Planned Data
                    if (prEntity.ImmediatePlans != null && prEntity.ImmediatePlans.Rows.Count > 0)
                    {
                        xrTC.Multiline = true;
                        var rows = prEntity.ImmediatePlans.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, ticketId), DatabaseObjects.Columns.UGITEndDate);
                        if (rows != null && rows.Count() > 0)
                        {

                            DataTable dtPlannedItem = rows.CopyToDataTable();
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("<span style='font-family: Verdana,Arial,sans-serif;font-size: 8pt;'><br/>");
                            foreach (DataRow row in rows)
                            {
                                DateTime dtEndDate;

                                if (DateTime.TryParse(Convert.ToString(row[DatabaseObjects.Columns.UGITEndDate]), out dtEndDate))
                                {
                                    sb.AppendLine(string.Format("<b>{0}:</b> {1}", string.Format("{0:MMM-dd-yyyy}", dtEndDate),
                                                    Convert.ToString(row[DatabaseObjects.Columns.Title])));
                                }
                                else
                                {
                                    sb.AppendLine(string.Format("{0}", Convert.ToString(row[DatabaseObjects.Columns.Title])));
                                }
                                if (prEntity.ShowPlanDesc)
                                {
                                    sb.AppendLine(string.Format("<br/>{0}", Convert.ToString(row[DatabaseObjects.Columns.ProjectNote])));
                                }
                                sb.AppendLine("<br/><br/>");
                            }
                            sb.AppendLine("</span>");
                            ((XRRichText)xrTC.Controls[0]).Html = sb.ToString();
                        }
                        else
                        {
                            ((XRRichText)xrTC.Controls[0]).Html = string.Empty;
                        }
                    }
                }
                else if (xrTC.Name == "ShowIssues")
                {
                    xrTC.Multiline = true;
                    ///Show Issues
                    if (prEntity.Issues != null && prEntity.Issues.Rows.Count > 0)
                    {

                        var rows = prEntity.Issues.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, ticketId), DatabaseObjects.Columns.ItemOrder);
                        if (rows != null && rows.Count() > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            int i = 0;
                            sb.AppendLine("<span style='font-family: Verdana,Arial,sans-serif;font-size: 8pt;'><br/>");
                            foreach (DataRow row in rows)
                            {
                                sb.AppendLine(string.Format("<b>{0}.</b> {1}", ++i, Convert.ToString(row[DatabaseObjects.Columns.Title])));
                                if (prEntity.ShowIssuesDesc)
                                {
                                    sb.AppendLine(string.Format("<br/>{0}", Convert.ToString(row[DatabaseObjects.Columns.Body])));
                                }
                                sb.AppendLine("<br/><br/>");
                            }
                            sb.AppendLine("</span>");
                            ((XRRichText)xrTC.Controls[0]).Html = sb.ToString();
                        }
                        else
                        {
                            ((XRRichText)xrTC.Controls[0]).Html = string.Empty;
                        }
                    }
                }
                else if (xrTC.Name == "ShowRisk")
                {
                    xrTC.Multiline = true;
                    ///Show Issues
                    if (prEntity.Risks != null && prEntity.Risks.Rows.Count > 0)
                    {

                        var rows = prEntity.Risks.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, ticketId));
                        if (rows != null && rows.Count() > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            int i = 0;
                            sb.AppendLine("<span style='font-family: Verdana,Arial,sans-serif;font-size: 8pt;'><br/>");
                            foreach (DataRow row in rows)
                            {
                                sb.AppendLine(string.Format("<b>{0}.</b> {1}", ++i, Convert.ToString(row[DatabaseObjects.Columns.Title])));
                                if (prEntity.ShowRiskDesc)
                                {
                                    sb.AppendLine(string.Format("<br/>{0}", Convert.ToString(row[DatabaseObjects.Columns.UGITDescription])));
                                }
                                sb.AppendLine("<br/><br/>");
                            }
                            sb.AppendLine("</span>");
                            ((XRRichText)xrTC.Controls[0]).Html = sb.ToString();
                        }
                        else
                        {
                            ((XRRichText)xrTC.Controls[0]).Html = string.Empty;
                        }
                    }
                }
                else if (xrTC.Name == "ShowProStatus")
                {
                    if (xrTC.Controls.Count > 0)
                    {
                        if (prEntity.ShowLatestOnly)
                        {
                            DataRow dr = prEntity.Projects.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.TicketId) == ticketId);
                            if(UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.ProjectSummaryNote))
                                ((XRRichText)xrTC.Controls[0]).Html = Convert.ToString(dr[DatabaseObjects.Columns.ProjectSummaryNote]);
                        }
                        else
                        {
                            var rows = prEntity.ExecutiveHistory.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, ticketId));
                            StringBuilder sb = new StringBuilder();
                            if (rows != null && rows.Count() > 0)
                            {
                                int i = 0;
                                foreach (DataRow row in rows)
                                {
                                    sb.AppendLine(string.Format(" {0}. {1}", ++i, Convert.ToString(row["Data"])));
                                    sb.AppendLine();
                                }
                            }
                                ((XRRichText)xrTC.Controls[0]).Html = sb.ToString();
                        }
                    }
                    else
                    {
                        if (prEntity.ShowLatestOnly)
                        {
                            DataRow dr = prEntity.Projects.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.TicketId) == ticketId);
                            if(UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.ProjectSummaryNote))
                                xrTC.Text = GetPlainTextFromHtml(Convert.ToString(dr[DatabaseObjects.Columns.ProjectSummaryNote]));
                        }
                        else
                        {
                            xrTC.Multiline = true;
                            var rows = prEntity.ExecutiveHistory.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, ticketId));
                            StringBuilder sb = new StringBuilder();
                            if (rows != null && rows.Count() > 0)
                            {
                                int i = 0;
                                foreach (DataRow row in rows)
                                {
                                    sb.AppendLine(string.Format("{0}. {1}", ++i, Convert.ToString(row["Data"])));
                                    sb.AppendLine();
                                }
                            }
                            xrTC.Text = GetPlainTextFromHtml(sb.ToString());
                        }
                    }
                }
                else if (xrTC.Name == "Iss")
                {
                    ///Show Monitors
                    if (prEntity.MonitorState != null && prEntity.MonitorState.Rows.Count > 0)
                    {
                        var rows = prEntity.MonitorState.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, ticketId));

                        if (rows != null && rows.Count() > 0)
                        {


                            XRTableCell ctrTC = xrTC;
                            while (ctrTC != null && Convert.ToString(ctrTC.Tag) == "Monitors")
                            {
                                if (ctrTC.Name == "Iss")
                                {
                                    var criticalIssue = rows.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleMonitorName) == "Critical Issues");
                                    //string colorname = Convert.ToString(criticalIssue[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).Replace("LED", "");
                                    
                                    if (criticalIssue != null)
                                    {
                                        if (prEntity.ShowTrafficlight)
                                            ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = UGITUtility.GetImageUrlForReport(uHelper.GetHealthIndicatorImageUrl(UGITUtility.ObjectToString(criticalIssue[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup])));
                                        else
                                        {
                                            if (Convert.ToString(criticalIssue[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("GreenLED"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "G";
                                            if (Convert.ToString(criticalIssue[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("YellowLED"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "Y";
                                            if (Convert.ToString(criticalIssue[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("RedLED"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "R";
                                        }
                                    }
                                    else
                                    {
                                        // ctrTC.Controls[0].Visible = false;
                                        ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = UGITUtility.GetImageUrlForReport(Convert.ToString("/Content/Images/LED_Green.png"));
                                    }
                                }
                                else if (ctrTC.Name == "Sco")
                                {
                                    var scope = rows.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleMonitorName) == "Within Scope");
                                    if (scope != null)
                                    {
                                        ((XRPictureBox)ctrTC.Controls[0]).Sizing = ImageSizeMode.Normal;
                                        if (prEntity.ShowTrafficlight)
                                            ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = UGITUtility.GetImageUrlForReport(uHelper.GetHealthIndicatorImageUrl( Convert.ToString(scope[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup])));
                                        else
                                        {
                                            if (Convert.ToString(scope[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("GreenLED"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "G";
                                            if (Convert.ToString(scope[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("YellowLED"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "Y";
                                            if (Convert.ToString(scope[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("RedLED"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "R";
                                        }
                                    }
                                    else
                                    {
                                        ((XRPictureBox)ctrTC.Controls[0]).Sizing = ImageSizeMode.Normal;
                                        ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = UGITUtility.GetImageUrlForReport(Convert.ToString("/Content/Images/LED_Green.png"));
                                    }
                                }
                                else if (ctrTC.Name == "$$$")
                                {
                                    var budget = rows.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleMonitorName) == "On Budget");
                                    if (budget != null)
                                    {
                                        ((XRPictureBox)ctrTC.Controls[0]).Sizing = ImageSizeMode.Normal;
                                        if (prEntity.ShowTrafficlight)
                                            ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = UGITUtility.GetImageUrlForReport(uHelper.GetHealthIndicatorImageUrl(Convert.ToString(budget[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup])));
                                        else
                                        {
                                            if (Convert.ToString(budget[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("GreenLED"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "G";
                                            if (Convert.ToString(budget[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("YellowLED"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "Y";
                                            if (Convert.ToString(budget[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("RedLED"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "R";
                                        }
                                    }
                                    else
                                    {
                                        // ctrTC.Controls[0].Visible = false;
                                        ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = UGITUtility.GetImageUrlForReport(Convert.ToString("/Content/Images/LED_Green.png"));
                                    }
                                }
                                else if (ctrTC.Name == "Tme")
                                {
                                    var time = rows.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleMonitorName) == "On Time");
                                    if (time != null)
                                    {
                                        ((XRPictureBox)ctrTC.Controls[0]).Sizing = ImageSizeMode.Normal;
                                        if (prEntity.ShowTrafficlight)
                                            ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = UGITUtility.GetImageUrlForReport(uHelper.GetHealthIndicatorImageUrl(Convert.ToString(time[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup])));
                                        else
                                        {
                                            if (Convert.ToString(time[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("GreenLED"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "G";
                                            if (Convert.ToString(time[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("YellowLED"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "Y";
                                            if (Convert.ToString(time[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("RedLED"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "R";
                                        }
                                    }
                                    else
                                    {
                                        //ctrTC.Controls[0].Visible = false;
                                        ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = UGITUtility.GetImageUrlForReport(Convert.ToString("/Content/Images/LED_Green.png"));
                                    }
                                }
                                else if (ctrTC.Name == "Rsk")
                                {
                                    var risk = rows.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleMonitorName) == "Risk Level");
                                    if (risk != null)
                                    {
                                        ((XRPictureBox)ctrTC.Controls[0]).Sizing = ImageSizeMode.Normal;
                                        if (prEntity.ShowTrafficlight)
                                            ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = UGITUtility.GetImageUrlForReport(uHelper.GetHealthIndicatorImageUrl(Convert.ToString(risk[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup])));
                                        else
                                        {
                                            if (Convert.ToString(risk[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("GreenLED"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "G";
                                            if (Convert.ToString(risk[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("YellowLED"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "Y";
                                            if (Convert.ToString(risk[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("RedLED"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "R";
                                        }
                                    }
                                    else
                                    {
                                        ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = UGITUtility.GetImageUrlForReport(Convert.ToString("/Content/Images/LED_Green.png"));
                                    }
                                }

                                ctrTC = ctrTC.NextCell;
                            }
                        }
                    }
                }
                else if (xrTC.Name == "ShowPercentComplete")
                {
                    DataRow dr = prEntity.Projects.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.TicketId) == ticketId);
                    if (dr != null && UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.TicketPctComplete))
                    {
                        double pct = 0;
                        double.TryParse(Convert.ToString(dr[DatabaseObjects.Columns.TicketPctComplete]), out pct);
                        pct = Math.Round(pct, 1, MidpointRounding.AwayFromZero); // Round to nearest 0.1
                        xrTC.Text = string.Format("{0}%", pct);
                    }
                }
                else if(xrTC.Name == "ShowTargetDate")
                {
                    DataRow dr = prEntity.Projects.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.TicketId) == ticketId);
                    if (dr != null)
                    {
                        DateTime targetDate;
                        DateTime.TryParse(Convert.ToString(dr[DatabaseObjects.Columns.TicketDesiredCompletionDate]), out targetDate);
                        if (targetDate != DateTime.MinValue)
                            xrTC.Text = string.Format("{0:MMM-dd-yyyy}", UGITUtility.StringToDateTime(targetDate));
                        else
                            xrTC.Text = string.Empty;
                    }
                }
            }
        }

        private string GetPlainTextFromHtml(string htmlString)
        {
            string htmlTagPattern = "<.*?>";
            var regexCss = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            htmlString = regexCss.Replace(htmlString, string.Empty);
            htmlString = Regex.Replace(htmlString, htmlTagPattern, string.Empty);
            htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            htmlString = htmlString.Replace("&nbsp;", string.Empty);

            return htmlString;
        }

        private void InitDetailsBasedOnXRTableCell(XtraReport rep, List<DataSourceDefinition> dsd)
        {
            int colCount = dsd.Count;
            int totalf = 0;
            for (int i = 0; i < dsd.Count; i++)
                totalf += dsd[i].Factor;
            int fWidth = (rep.PageWidth - ReportOffset - (rep.Margins.Left + rep.Margins.Right)) / totalf;
            int incShift = 0;
            List<XRTableCell> headers = new List<XRTableCell>();
            List<XRTableCell> details = new List<XRTableCell>();
            for (int i = 0; i < colCount; i++)
            {
                if (dsd[i].Fieldname == "ShowMonitorState")
                {
                    int width = (fWidth * dsd[i].Factor) / 5;
                    float padding = (width - 20) / 2;

                    XRTableCell xrHeaderTableCellIss = new XRTableCell();
                    xrHeaderTableCellIss.Text = "Iss";
                    xrHeaderTableCellIss.TextAlignment = dsd[i].TextAlignment;
                    xrHeaderTableCellIss.SizeF = new SizeF(width, 20);

                    XRTableCell xrHeaderTableCellSco = new XRTableCell();
                    xrHeaderTableCellSco.Text = "Sco";
                    xrHeaderTableCellSco.TextAlignment = dsd[i].TextAlignment;
                    xrHeaderTableCellSco.SizeF = new SizeF(width, 20);

                    XRTableCell xrHeaderTableCellBug = new XRTableCell();
                    xrHeaderTableCellBug.Text = "$$$";
                    xrHeaderTableCellBug.TextAlignment = dsd[i].TextAlignment;
                    xrHeaderTableCellBug.SizeF = new SizeF(width, 20);

                    XRTableCell xrHeaderTableCellTme = new XRTableCell();
                    xrHeaderTableCellTme.Text = "Tme";
                    xrHeaderTableCellTme.TextAlignment = dsd[i].TextAlignment;
                    xrHeaderTableCellTme.SizeF = new SizeF(width, 20);

                    XRTableCell xrHeaderTableCellRsk = new XRTableCell();
                    xrHeaderTableCellRsk.Text = "Rsk";
                    xrHeaderTableCellRsk.TextAlignment = dsd[i].TextAlignment;
                    xrHeaderTableCellRsk.SizeF = new SizeF(width, 20);
                    headers.AddRange(new XRTableCell[] { xrHeaderTableCellIss, xrHeaderTableCellSco, xrHeaderTableCellBug, xrHeaderTableCellTme, xrHeaderTableCellRsk });

                    
                        XRTableCell xrTableCellIss = new XRTableCell();
                        xrTableCellIss.Name = "Iss";
                        xrTableCellIss.TextAlignment = dsd[i].TextAlignment;
                        xrTableCellIss.SizeF = new SizeF(width, 22);
                        xrTableCellIss.Padding = new PaddingInfo(0, 0, 0, 0);

                        XRPictureBox xrPB1 = new XRPictureBox();
                        xrPB1.SizeF = new SizeF(20, 20);
                        xrPB1.Borders = DevExpress.XtraPrinting.BorderSide.None;
                        //xrPB1.ImageUrl = UGITUtility.GetImageUrlForReport(Convert.ToString("/Report/Content/Images/LED_Green.png"));
                        xrPB1.StylePriority.UseBorders = false;
                        //xrPB1.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                        XRLabel xrLabeliis = new XRLabel();
                        xrLabeliis.Borders = BorderSide.None;
                        xrLabeliis.TextAlignment = TextAlignment.MiddleCenter;
                        //xrLabeliis.Font = new Font(new FontFamily("verdana"), 8.23f, FontStyle.Bold);
                        xrLabeliis.SizeF = new SizeF(20, 20);
                        //xrLabeliis.LocationFloat = new DevExpress.Utils.PointFloat(padding, 1F);

                        xrTableCellIss.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrPB1, xrLabeliis });
                        xrTableCellIss.Tag = "Monitors";
                    

                    
                        XRTableCell xrTableCellSco = new XRTableCell();
                        xrTableCellSco.Name = "Sco";
                        xrTableCellSco.TextAlignment = dsd[i].TextAlignment;
                        xrTableCellSco.SizeF = new SizeF(width, 20);
                        XRPictureBox xrPB2 = new XRPictureBox();
                        xrPB2.SizeF = new SizeF(20, 20);
                        xrPB2.Borders = DevExpress.XtraPrinting.BorderSide.None;
                        //xrPB2.Borders. = dsd[i].TextAlignment;
                        xrPB2.StylePriority.UseBorders = false;
                        //xrPB2.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                        XRLabel xrLabelSco = new XRLabel();
                        xrLabelSco.Borders = BorderSide.None;
                        xrLabelSco.TextAlignment = TextAlignment.MiddleCenter;
                        //xrLabelSco.Font = new Font(new FontFamily("verdana"), 8.23f, FontStyle.Bold);
                        xrLabelSco.SizeF = new SizeF(20, 20);
                        //xrLabelSco.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                        xrTableCellSco.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrPB2, xrLabelSco });
                        xrTableCellSco.Padding = new PaddingInfo(0, 0, 0, 0);
                        xrTableCellSco.Tag = "Monitors";
                    

                    
                        XRTableCell xrTableCellBug = new XRTableCell();
                        xrTableCellBug.Name = "$$$";
                        xrTableCellBug.TextAlignment = dsd[i].TextAlignment;
                        xrTableCellBug.SizeF = new SizeF(width, 20);
                        XRPictureBox xrPB3 = new XRPictureBox();
                        xrPB3.SizeF = new SizeF(20, 20);
                        xrPB3.Borders = DevExpress.XtraPrinting.BorderSide.None;
                        //xrPB3.TextAlignment = dsd[i].TextAlignment;
                        xrPB3.StylePriority.UseBorders = false;
                        //xrPB3.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                        XRLabel xrLabelDolar = new XRLabel();
                        xrLabelDolar.Borders = BorderSide.None;
                        xrLabelDolar.TextAlignment = TextAlignment.MiddleCenter;
                        //xrLabelDolar.Font = new Font(new FontFamily("verdana"), 8.23f, FontStyle.Bold);
                        xrLabelDolar.SizeF = new SizeF(20, 20);
                        //xrLabelDolar.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                        xrTableCellBug.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrPB3, xrLabelDolar });
                        xrTableCellBug.Padding = new PaddingInfo(0, 0, 0, 0);
                        xrTableCellBug.Tag = "Monitors";
                    

                    
                        XRTableCell xrTableCellTme = new XRTableCell();
                        xrTableCellTme.Name = "Tme";
                        xrTableCellTme.TextAlignment = dsd[i].TextAlignment;
                        xrTableCellTme.SizeF = new SizeF(width, 20);
                        XRPictureBox xrPB4 = new XRPictureBox();
                        xrPB4.SizeF = new SizeF(20, 20);
                        xrPB4.Borders = DevExpress.XtraPrinting.BorderSide.None;
                        //xrPB4.TextAlignment = dsd[i].TextAlignment;
                        xrPB4.StylePriority.UseBorders = false;
                        //xrPB4.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                        XRLabel xrLabelTme = new XRLabel();
                        xrLabelTme.Borders = BorderSide.None;
                        xrLabelTme.TextAlignment = TextAlignment.MiddleCenter;
                        //xrLabelTme.Font = new Font(new FontFamily("verdana"), 8.23f, FontStyle.Bold);
                        xrLabelTme.SizeF = new SizeF(20, 20);
                        //xrLabelTme.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                        xrTableCellTme.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrPB4, xrLabelTme });
                        xrTableCellTme.Padding = new PaddingInfo(0, 0, 0, 0);
                        xrTableCellTme.Tag = "Monitors";
                    

                    
                        XRTableCell xrTableCellRsk = new XRTableCell();
                        xrTableCellRsk.Name = "Rsk";
                        xrTableCellRsk.TextAlignment = dsd[i].TextAlignment;
                        xrTableCellRsk.SizeF = new SizeF(width, 20);
                        xrTableCellRsk.Tag = "Monitors";
                        XRPictureBox xrPB5 = new XRPictureBox();
                        xrPB5.SizeF = new SizeF(20, 20);
                        xrPB5.Borders = DevExpress.XtraPrinting.BorderSide.None;

                        //xrPB5.TextAlignment = dsd[i].TextAlignment;
                        xrPB5.StylePriority.UseBorders = false;
                        //xrPB5.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                        XRLabel xrLabelRsk = new XRLabel();
                        xrLabelRsk.Borders = BorderSide.None;
                        xrLabelRsk.TextAlignment = TextAlignment.MiddleCenter;
                        //xrLabelRsk.Font = new Font(new FontFamily("verdana"), 8.23f, FontStyle.Bold);
                        xrLabelRsk.SizeF = new SizeF(20, 20);
                        //xrLabelRsk.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                        xrTableCellRsk.Padding = new PaddingInfo(0,0,0,0);
                        xrTableCellRsk.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrPB5, xrLabelRsk });
                    
                    details.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] { xrTableCellIss, xrTableCellSco, xrTableCellBug, xrTableCellTme, xrTableCellRsk });
                }
                else
                {
                    XRTableCell xrHeaderTableCell = CreateHeaderTableCell(dsd[i], fWidth, incShift);
                    XRTableCell xrDetailTableCell = CreateDetailTableCell(dsd[i], fWidth, incShift);
                    incShift += fWidth * dsd[i].Factor;
                    headers.Add(xrHeaderTableCell);
                    details.Add(xrDetailTableCell);
                }


            }
            ((XRTableRow)rep.Bands[BandKind.PageHeader].Controls[0].Controls[0]).Cells.AddRange(headers.ToArray());
            ((XRTableRow)rep.Bands[BandKind.Detail].Controls[0].Controls[0]).Cells.AddRange(details.ToArray());

        }

        private static XRLabel CreateLabel(DataSourceDefinition dsd, int fWidth, int incShift)
        {
            XRLabel labeld = new XRLabel();
            labeld.Location = new Point(incShift, 0);
            labeld.Size = new Size(fWidth * dsd.Factor, 20);
            return labeld;
        }

        private XRTableCell CreateHeaderTableCell(DataSourceDefinition dsd, int fWidth, int incShift)
        {
            XRTableCell xrTableCell = new XRTableCell();
            xrTableCell.Name = string.Format("xrTC{0}", dsd.Fieldname);
            xrTableCell.Text = dsd.CaptionName;

            if (dsd.Fieldname == "TicketId")
            {
                xrTableCell.Visible = false;
            }
            xrTableCell.TextAlignment = dsd.TextAlignment;
            xrTableCell.SizeF = new SizeF(fWidth * dsd.Factor, 20);
            return xrTableCell;

        }

        private XRTableCell CreateDetailTableCell(DataSourceDefinition dsd, int fWidth, int incShift)
        {
            XRTableCell xrTableCell = new XRTableCell();
            xrTableCell.Name = dsd.Fieldname;

            if (dsd.Fieldname == "ShowProStatus")
            {
                if (prEntity.ShowPlainText)
                {
                    //xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.ProjectSummaryNote);
                }
                else
                {
                    XRRichText richText = new XRRichText();
                    richText.StylePriority.UseBorders = false;
                    richText.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    richText.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                    //richText.Padding = new PaddingInfo(2, 2, 5, 0);
                    richText.DataBindings.Add("Html", null, DatabaseObjects.Columns.ProjectSummaryNote);
                    xrTableCell.Name = dsd.Fieldname;
                    richText.SizeF = new SizeF(fWidth * dsd.Factor, 20);
                    xrTableCell.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { richText });
                    xrTableCell.Tag = "status";
                }
            }
            else if (dsd.Fieldname == "ShowProjectName" || dsd.Fieldname == DatabaseObjects.Columns.Title)
            {
                XRRichText richText = new XRRichText();
                richText.StylePriority.UseBorders = false;
                richText.Borders = DevExpress.XtraPrinting.BorderSide.Left;
                richText.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                richText.SizeF = new SizeF(fWidth * dsd.Factor, 20);
                richText.Html = string.Format("<span style='font-family: Verdana,Arial,sans-serif;font-size: 8pt;'><br/><b>[{0}]:</b> [{1}]<br/></span>", DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.Title);

                XRLabel xrLblTicketId = new XRLabel();
                xrLblTicketId.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketId);
                xrLblTicketId.BeforePrint += cell2_BeforePrint;
                xrLblTicketId.SizeF = new SizeF(0, 0);
                xrLblTicketId.Visible = false;
                xrTableCell.Name = "TicketId";
                xrTableCell.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrLblTicketId, richText });

            }
            else if (dsd.Fieldname == "ShowPriority")
            {
                xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketPriorityLookup);
            }
            else if (dsd.Fieldname == "ShowStatus")
            {
                xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketStatus);
            }
            else if (dsd.Fieldname == "ShowDescription")
            {
                xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketDescription);
            }
            else if (dsd.Fieldname == "ShowTargetDate")
            {
                xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketDesiredCompletionDate, "{0:MMM-dd-yyyy}");
            }
            else if (dsd.Fieldname == "ShowProjectManagers")
            {
                xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketProjectManager);
            }
            else if (dsd.Fieldname == "ShowProgress")
            {
                xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketStatus);
            }
            else if (dsd.Fieldname == "ShowProjectType")
            {
                xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketRequestTypeLookup);
            }
            else if (dsd.Fieldname == "ShowPercentComplete")
            {
                // xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketPctComplete);
            }
            else if (dsd.Fieldname == "ShowAccomplishment")
            {
                XRRichText richText = new XRRichText();
                richText.StylePriority.UseBorders = false;
                richText.Borders = DevExpress.XtraPrinting.BorderSide.None;
                richText.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                richText.SizeF = new SizeF(fWidth * dsd.Factor, 20);
                xrTableCell.Name = dsd.Fieldname;
                xrTableCell.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { richText });
            }
            else if (dsd.Fieldname == "ShowPlan")
            {
                XRRichText richText = new XRRichText();
                richText.StylePriority.UseBorders = false;
                richText.Borders = DevExpress.XtraPrinting.BorderSide.None;
                richText.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                xrTableCell.Name = dsd.Fieldname;
                richText.SizeF = new SizeF(fWidth * dsd.Factor, 20);
                xrTableCell.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { richText });
            }
            else if (dsd.Fieldname == "ShowIssues")
            {
                XRRichText richText = new XRRichText();
                richText.StylePriority.UseBorders = false;
                richText.Borders = DevExpress.XtraPrinting.BorderSide.None;
                richText.SizeF = new SizeF(fWidth * dsd.Factor, 20);
                richText.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                xrTableCell.Name = dsd.Fieldname;
                xrTableCell.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { richText });
            }
            else if (dsd.Fieldname == "ShowRisk")
            {
                XRRichText richText = new XRRichText();
                richText.StylePriority.UseBorders = false;
                richText.Borders = DevExpress.XtraPrinting.BorderSide.None;
                richText.SizeF = new SizeF(fWidth * dsd.Factor, 20);
                richText.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                xrTableCell.Name = dsd.Fieldname;
                xrTableCell.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { richText });
            }

            else
            {
                xrTableCell.DataBindings.Add("Text", null, dsd.Fieldname);
                xrTableCell.Name = dsd.Fieldname;
            }

            xrTableCell.TextAlignment = dsd.TextAlignment;
            xrTableCell.SizeF = new SizeF(fWidth * dsd.Factor, 20);
            return xrTableCell;
        }

        private static XRRichText CreateRichText(DataSourceDefinition dsd, int fWidth, int incShift)
        {
            XRRichText richText = new XRRichText();
            richText.Location = new Point(incShift, 0);
            richText.Size = new Size(fWidth * dsd.Factor, 20);
            return richText;
        }

        private List<DataSourceDefinition> GenerateDataSourceDefinition(object myComplexObject)
        {
            List<DataSourceDefinition> dsdl = new List<DataSourceDefinition>();

            if (myComplexObject is NPRProjectReportEntity)
            {
                NPRProjectReportEntity nprEntity = myComplexObject as NPRProjectReportEntity;

                foreach (Reportable field in nprEntity.Fields)
                {
                    DataSourceDefinition dsd = new DataSourceDefinition();
                    dsd.CaptionName = string.IsNullOrWhiteSpace(field.AlternateName) ? field.FName : field.AlternateName;
                    dsd.Fieldname = field.FName;
                    dsd.Factor = field.LenFactor == 0 ? 1 : field.LenFactor;
                    dsd.TextAlignment = field.TextAlignment;
                    dsdl.Add(dsd);
                }
            }
            else
            {
                PropertyInfo[] pi = myComplexObject.GetType().GetProperties();
                for (int i = 0; i < pi.Length; i++)
                {
                    Reportable[] r = pi[i].GetCustomAttributes(typeof(Reportable), false) as Reportable[];
                    if (r.Length > 0)
                    {
                        if (Convert.ToBoolean(pi[i].GetValue(myComplexObject, pi[i].GetIndexParameters())) || pi[i].Name == DatabaseObjects.Columns.TicketId)
                        {
                            DataSourceDefinition dsd = new DataSourceDefinition();
                            dsd.CaptionName = r[0].AlternateName == null ? pi[i].Name : r[0].AlternateName;
                            dsd.Fieldname = pi[i].Name;
                            dsd.Factor = r[0].LenFactor == 0 ? 1 : r[0].LenFactor;
                            dsd.TextAlignment = r[0].TextAlignment;
                            dsdl.Add(dsd);
                        }
                    }
                }
            }

            return dsdl;
        }

        public void InitBands(XtraReport rep)
        {
            // Create bands
            DetailBand detail = new DetailBand();
            detail.HeightF = 25F;
            detail.Name = "Detail";
            detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;

            XRTableRow xrTableRow1 = new XRTableRow();
            xrTableRow1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            xrTableRow1.Name = "xrTableRow1";
            xrTableRow1.StylePriority.UseFont = false;
            //xrTableRow1.Weight = 1D;

            XRTableRow xrTableRow2 = new XRTableRow();
            xrTableRow2.Name = "xrTableRow2";
            xrTableRow2.Tag = "details";
            //xrTableRow2.Weight = 1D;

            XRTable xrTblProHeader = new XRTable();
            xrTblProHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            xrTblProHeader.BorderColor = System.Drawing.Color.Gray;
            xrTblProHeader.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
            | DevExpress.XtraPrinting.BorderSide.Right)
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            xrTblProHeader.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            xrTblProHeader.Name = "xrTblProHeader";
            xrTblProHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            xrTblProHeader.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            xrTableRow1});
            xrTblProHeader.SizeF = new System.Drawing.SizeF(rep.PageWidth - ReportOffset, 25F);
            xrTblProHeader.StylePriority.UseBackColor = false;
            xrTblProHeader.StylePriority.UseBorderColor = false;
            xrTblProHeader.StylePriority.UseBorders = false;
            xrTblProHeader.StylePriority.UsePadding = false;

            XRTable xrTblProjectDetails = new XRTable();
            xrTblProjectDetails.BorderColor = System.Drawing.Color.Gray;
            xrTblProjectDetails.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            xrTblProjectDetails.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            xrTblProjectDetails.Name = "xrTblProjectDetails";
            xrTblProjectDetails.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            xrTblProjectDetails.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            xrTableRow2});
            xrTblProjectDetails.SizeF = new System.Drawing.SizeF(rep.PageWidth - ReportOffset, 25F);
            xrTblProjectDetails.StylePriority.UseBorderColor = false;
            xrTblProjectDetails.StylePriority.UseBorders = false;
            xrTblProjectDetails.StylePriority.UsePadding = false;

            PageHeaderBand pageHeader = new PageHeaderBand();
            pageHeader.KeepTogether = true;
            ReportFooterBand reportFooter = new ReportFooterBand();
            reportFooter.KeepTogether = true;
            detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            xrTblProjectDetails});

            pageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrTblProHeader
            });
            pageHeader.HeightF = 25F;
            pageHeader.Name = "PageHeader";

            detail.Height = 20;
            reportFooter.Height = 380;
            pageHeader.Height = 20;

            XRLabel xrLabel1 = new XRLabel();
            xrLabel1.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 21.95834F);
            xrLabel1.Name = "xrLabel1";
            xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            xrLabel1.SizeF = new System.Drawing.SizeF(rep.PageWidth - ReportOffset, 41.75F);
            xrLabel1.StylePriority.UseFont = false;
            xrLabel1.StylePriority.UseTextAlignment = false;
            xrLabel1.Text = "Project Summary Report";
            xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;

            ReportHeaderBand ReportHeader = new ReportHeaderBand();
            ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            xrLabel1});
            ReportHeader.HeightF = 97.91666F;
            ReportHeader.Name = "ReportHeader";

            // Place the bands onto a report
            rep.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] { ReportHeader, detail, pageHeader });
        }
    }
    public class DataSourceDefinition
    {
        string fieldname;

        public string Fieldname
        {
            get { return fieldname; }
            set { fieldname = value; }
        }
        string captionName;

        public string CaptionName
        {
            get { return captionName; }
            set { captionName = value; }
        }
        int factor;

        public int Factor
        {
            get { return factor; }
            set { factor = value; }
        }

        private TextAlignment textAlignment;
        public TextAlignment TextAlignment
        {
            get { return this.textAlignment; }
            set { this.textAlignment = value; }
        }
    }
}