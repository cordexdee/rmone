using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UI.PivotGrid;
using System.Data;
using uGovernIT.Manager.Entities;
using uGovernIT.Utility;

namespace uGovernIT.Manager.Reports
{
    public partial class NonPeakHoursPerformance_Report : DevExpress.XtraReports.UI.XtraReport
    {
        HelpDeskPrfReportEntity dataEntity;
        public NonPeakHoursPerformance_Report(HelpDeskPrfReportEntity entity)
        {
            InitializeComponent();
            dataEntity = entity;
            BindData();
        }
        private void BindData()
        {
            XRPivotGridField field = null;
            if (!string.IsNullOrWhiteSpace(dataEntity.StartDate))
            {
                xrLblHeader.Text = string.Format("{0} - {1}", xrLblHeader.Text, dataEntity.StartDate);
            }
            field = new XRPivotGridField("Sequence", DevExpress.XtraPivotGrid.PivotArea.RowArea);
            field.SortOrder = DevExpress.XtraPivotGrid.PivotSortOrder.Ascending;
            field.Visible = false;

            field.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            xrPivotGrid.Fields.Add(field);

            field = new XRPivotGridField(DatabaseObjects.Columns.Status, DevExpress.XtraPivotGrid.PivotArea.RowArea);
            field.SortMode = DevExpress.XtraPivotGrid.PivotSortMode.Custom;
            xrPivotGrid.CustomFieldSort += XrPivotGrid_CustomFieldSort;
            field.Caption = "KPI";
            field.MinWidth = 350;
            field.Width = 350;
            field.Appearance.FieldHeader.WordWrap = true;
            field.Appearance.FieldValue.WordWrap = true;
            field.Appearance.FieldValue.Font = new Font(xrPivotGrid.Appearance.FieldValueGrandTotal.Font, FontStyle.Regular);
            field.Appearance.FieldValue.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Near;
            field.Appearance.FieldValue.BackColor = Color.White;
            field.Appearance.FieldHeader.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.Cell.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.Cell.Font = new Font(xrPivotGrid.Appearance.FieldValueGrandTotal.Font, FontStyle.Regular);
            field.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            field.Appearance.Cell.TextVerticalAlignment = DevExpress.Utils.VertAlignment.Center;
            field.Appearance.GrandTotalCell.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.FieldValueGrandTotal.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            xrPivotGrid.Fields.Add(field);

            field = new XRPivotGridField("Count", DevExpress.XtraPivotGrid.PivotArea.DataArea);
            field.Caption = string.Empty;

            field.Appearance.FieldHeader.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.Cell.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            xrPivotGrid.Fields.Add(field);



            xrPivotGrid.OptionsView.ShowDataHeaders = false;
            xrPivotGrid.OptionsView.ShowColumnGrandTotalHeader = false;


            xrPivotGrid.OptionsView.ShowColumnTotals = false;
            xrPivotGrid.OptionsView.ShowGrandTotalsForSingleValues = true;
            xrPivotGrid.OptionsView.ShowColumnHeaders = false;
            xrPivotGrid.OptionsView.ShowTotalsForSingleValues = true;

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
            xrPivotGrid.CustomFieldSort += XrPivotGrid_CustomFieldSort;
            xrPivotGrid.DataSource = dataEntity.Data;
        }

        private void XrPivotGrid_CustomFieldSort(object sender, PivotGridCustomFieldSortEventArgs e)
        {
            if (e.Field.FieldName == DatabaseObjects.Columns.Status)
            {
                object orderValue1 = e.GetListSourceColumnValue(e.ListSourceRowIndex1, "Sequence"),
                    orderValue2 = e.GetListSourceColumnValue(e.ListSourceRowIndex2, "Sequence");
                e.Result = Comparer.Default.Compare(orderValue1, orderValue2);
                e.Handled = true;
            }
        }

        private void XrPivotGrid_PrintFieldValue(object sender, CustomExportFieldValueEventArgs e)
        {
            if (!e.IsColumn && ((e.Field != null && e.Field.FieldName == DatabaseObjects.Columns.Status) || e.ValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal))
            {
                e.Appearance.BackColor = Color.White;
                //e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);

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
        }

        private void XrPivotGrid_FieldValueDisplayText(object sender, PivotFieldDisplayTextEventArgs e)
        {
            if (e.ValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal)
            {
                if (e.IsColumn)
                    e.DisplayText = "#";
                else
                    e.DisplayText = "Total";
                //  e.Field.Appearance.FieldValueGrandTotal.TextHorizontalAlignment= DevExpress.Utils.HorzAlignment.Far;
            }

        }
    }
}
