using System;
using System.Drawing;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UI.PivotGrid;
using System.Data;
using DevExpress.XtraPrinting;
using uGovernIT.Report.Entities;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Configuration;

namespace uGovernIT.Report.DxReport
{
    public partial class SummaryByTechnician_Report : DevExpress.XtraReports.UI.XtraReport
    {
        TicketSummaryByPRPEntity dataEntity;
        ApplicationContext Context;
        public SummaryByTechnician_Report(TicketSummaryByPRPEntity entity)
        {
            InitializeComponent();
            dataEntity = entity;
            BindData();
        }

        public SummaryByTechnician_Report(ApplicationContext context, TicketSummaryByPRPEntity entity)
        {
            Context = context;
            InitializeComponent();
            dataEntity = entity;
            BindData();
        }

        private void BindData()
        {
            XRPivotGridField field = null;
            if (!string.IsNullOrWhiteSpace(dataEntity.StartDate) && !string.IsNullOrWhiteSpace(dataEntity.EndDate))
            {
                xrLblHeader.Text = string.Format("{0} ({1} to {2})", xrLblHeader.Text, dataEntity.StartDate, dataEntity.EndDate);
            }
            else if (!string.IsNullOrWhiteSpace(dataEntity.StartDate))
            {
                xrLblHeader.Text = string.Format("{0} (from {1})", xrLblHeader.Text, dataEntity.StartDate);
            }
            else if (!string.IsNullOrWhiteSpace(dataEntity.EndDate))
            {
                xrLblHeader.Text = string.Format("{0} (till {1})", xrLblHeader.Text, dataEntity.EndDate);
            }
            else
            {
                xrLblHeader.Text = string.Format("{0} (All)", xrLblHeader.Text);
            }
            if (dataEntity.ModuleName.Contains(";")) { 
                moduleNameHeader.Text = dataEntity.ModuleName.Split(';')[1];
                dataEntity.ModuleName = dataEntity.ModuleName.Split(';')[0];
            }
            else
                moduleNameHeader.Text = dataEntity.ModuleName;
            if (dataEntity.GroupByCategory)
            {
                xrLblHeader.ResetPadding();
                xrLblHeader.Padding = new PaddingInfo(90, 0, 0, 0);
                field = new XRPivotGridField(DatabaseObjects.Columns.Category, DevExpress.XtraPivotGrid.PivotArea.RowArea);
                field.SortOrder = DevExpress.XtraPivotGrid.PivotSortOrder.Ascending;
                field.Caption = "Category";
                field.AreaIndex = 0;
                field.MinWidth = 170;
                field.Width = 170;
                field.Appearance.FieldHeader.WordWrap = true;
                field.Appearance.FieldValue.WordWrap = true;
                field.Appearance.FieldValue.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
                field.Appearance.FieldValue.BackColor = Color.White;
                field.Appearance.FieldValue.Font = new Font(field.Appearance.FieldValue.Font, FontStyle.Bold);
                // field.Appearance.FieldHeader.BackColor = Color.White;
                field.Appearance.FieldHeader.Font = new Font(field.Appearance.FieldHeader.Font, FontStyle.Bold);
                field.Appearance.FieldHeader.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
                field.Appearance.Cell.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
                field.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
                field.Appearance.Cell.TextVerticalAlignment = DevExpress.Utils.VertAlignment.Center;
                field.Appearance.GrandTotalCell.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
                field.Appearance.FieldValueGrandTotal.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
                xrPivotGrid.Fields.Add(field);
            }

            field = new XRPivotGridField(DatabaseObjects.Columns.PRP, DevExpress.XtraPivotGrid.PivotArea.RowArea);
            field.SortOrder = DevExpress.XtraPivotGrid.PivotSortOrder.Ascending;
            field.Caption = "Technician"; //"Primary Responsible Person (PRP)";
            field.MinWidth = 230;
            field.Width = 230;
            field.Appearance.FieldHeader.WordWrap = true;
            field.Appearance.FieldValue.WordWrap = true;
            field.Appearance.FieldValue.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.FieldValue.BackColor = Color.White;
            field.Appearance.FieldValue.Font = new Font(field.Appearance.FieldValue.Font, FontStyle.Bold);

            field.Appearance.FieldHeader.Font = new Font(field.Appearance.FieldHeader.Font, FontStyle.Bold);
            field.Appearance.FieldHeader.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.GrandTotalCell.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.FieldValueGrandTotal.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.Cell.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.FieldValueGrandTotal.Font = new Font(field.Appearance.FieldValueGrandTotal.Font, FontStyle.Bold);
            field.Appearance.GrandTotalCell.BackColor = Color.White;
            xrPivotGrid.Fields.Add(field);

            field = new XRPivotGridField(DatabaseObjects.Columns.Status, DevExpress.XtraPivotGrid.PivotArea.ColumnArea);
            field.Caption = "Status";
            field.Appearance.FieldHeader.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.FieldHeader.Font = new Font(field.Appearance.FieldHeader.Font, FontStyle.Bold);
            field.Appearance.GrandTotalCell.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.FieldValueGrandTotal.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.FieldHeader.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.Cell.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.FieldValue.Font = new Font(field.Appearance.FieldValue.Font, FontStyle.Bold);
            field.Appearance.FieldValueGrandTotal.Font = new Font(field.Appearance.FieldValueGrandTotal.Font, FontStyle.Bold);

            xrPivotGrid.Fields.Add(field);

            field = new XRPivotGridField("Count", DevExpress.XtraPivotGrid.PivotArea.DataArea);
            field.Caption = string.Empty;

            field.Appearance.FieldHeader.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.Cell.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            xrPivotGrid.Fields.Add(field);


            xrPivotGrid.OptionsView.ShowDataHeaders = false;
            xrPivotGrid.OptionsView.ShowColumnGrandTotalHeader = false;
            xrPivotGrid.OptionsView.ShowGrandTotalsForSingleValues = false;
            xrPivotGrid.OptionsView.ShowColumnGrandTotals = false;
            xrPivotGrid.OptionsView.ShowColumnTotals = false;
            xrPivotGrid.OptionsView.ShowGrandTotalsForSingleValues = false;
            xrPivotGrid.OptionsView.ShowColumnHeaders = false;
            xrPivotGrid.OptionsView.ShowTotalsForSingleValues = false;
            xrPivotGrid.OptionsView.ShowRowGrandTotals = true;
            xrPivotGrid.OptionsView.ShowColumnGrandTotals = true;


            xrPivotGrid.Appearance.FieldValueGrandTotal.Font = new Font(xrPivotGrid.Appearance.FieldValueGrandTotal.Font, FontStyle.Bold);
            xrPivotGrid.Appearance.FieldValueGrandTotal.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            xrPivotGrid.Appearance.FieldValueGrandTotal.TextVerticalAlignment = DevExpress.Utils.VertAlignment.Center;
            xrPivotGrid.Appearance.FieldHeader.WordWrap = true;
            xrPivotGrid.Appearance.FieldHeader.Font = new Font(xrPivotGrid.Appearance.FieldValue.Font, FontStyle.Bold);
            xrPivotGrid.Appearance.FieldHeader.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            xrPivotGrid.Appearance.FieldHeader.TextVerticalAlignment = DevExpress.Utils.VertAlignment.Center;
            xrPivotGrid.Appearance.FieldValue.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            xrPivotGrid.Appearance.FieldValue.TextVerticalAlignment = DevExpress.Utils.VertAlignment.Center;

            xrPivotGrid.Appearance.Cell.BorderWidth = 0.5f;
            xrPivotGrid.Appearance.FieldValue.BorderWidth = 0.5f;
            xrPivotGrid.Appearance.FieldValueGrandTotal.BorderWidth = 0.5f;
            xrPivotGrid.Appearance.FieldValueTotal.BorderWidth = 0.5f;
            xrPivotGrid.Appearance.FieldHeader.BorderWidth = 0.5f;

            xrPivotGrid.FieldValueDisplayText += XrPivotGrid_FieldValueDisplayText;
            xrPivotGrid.PrintCell += XrPivotGrid_PrintCell;
            xrPivotGrid.PrintHeader += XrPivotGrid_PrintHeader;
            xrPivotGrid.PrintFieldValue += XrPivotGrid_PrintFieldValue;
            xrPivotGrid.DataSource = dataEntity.Data;

            //Report.DataSource = data;
        }

        private void XrPivotGrid_PrintFieldValue(object sender, CustomExportFieldValueEventArgs e)
        {
            if (!e.IsColumn && ((e.Field != null && e.Field.FieldName == "PRP") || e.ValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal))
            {
                e.Appearance.BackColor = Color.White;
                if (e.MinIndex % 2 == 0)
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#EAEAEA");
            }

        }

        private void XrPivotGrid_PrintHeader(object sender, CustomExportHeaderEventArgs e)
        {

        }

        private void XrPivotGrid_PrintCell(object sender, CustomExportCellEventArgs e)
        {
            if (e.RowIndex % 2 == 0)
                e.Appearance.BackColor = ColorTranslator.FromHtml("#EAEAEA");

            if (e.DataField != null && e.DataField.Area == DevExpress.XtraPivotGrid.PivotArea.DataArea && e.DataField != null && e.DataField.FieldName == "Count")
            {
                if (!string.IsNullOrEmpty(e.Text) && Convert.ToInt32(e.Text) > 0)
                {

                    string category = string.Empty;
                    string stageNames = e.ColumnValue.DisplayText.ToLower().Trim() == "total" ? dataEntity.IncludeCounts : e.ColumnValue.DisplayText;

                    if (dataEntity.GroupByCategory)
                    {
                        XRPivotGridField field = (XRPivotGridField)((XRPivotGrid)sender).GetFieldByArea(DevExpress.XtraPivotGrid.PivotArea.RowArea, 0);
                        if (field != null)
                        {
                            category = Convert.ToString(((XRPivotGrid)sender).GetFieldValue(field, e.RowIndex));
                        }
                    }
                    string param = "?control=summarybytechnician&stage=" + stageNames + "&category=" + category + "&technician=" + e.RowValue.DisplayText + "&startDate=" + dataEntity.StartDate + "&endDate=" + dataEntity.EndDate + "&moduleName=" + dataEntity.ModuleName + "&includeTechnician=" + dataEntity.IncludeTechnician;

                    //string delegateUrl = Context.SiteUrl +  "/Layouts/uGovernIT/delegatecontrol.aspx" + param;
                    string delegateUrl = ConfigurationManager.AppSettings["SiteUrl"] + "/Layouts/uGovernIT/delegatecontrol.aspx" + param;

                    ((TextBrick)e.Brick).Url = delegateUrl;
                    e.Appearance.ForeColor = Color.Blue;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold | FontStyle.Underline);
                }
            }

        }

        private void XrPivotGrid_FieldValueDisplayText(object sender, PivotFieldDisplayTextEventArgs e)
        {
            if (e.ValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal)
            {
                e.DisplayText = "Total";
            }
        }
    }
}
