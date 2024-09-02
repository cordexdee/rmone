using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UI.PivotGrid;
using DevExpress.Data.PivotGrid;
using DevExpress.XtraPivotGrid;
using System.Data;
using uGovernIT.Entities;
using uGovernIT.Utility;

namespace uGovernIT.DxReport
{
    public partial class WeeklyTeamReport_Report : DevExpress.XtraReports.UI.XtraReport
    {
        WeeklyTeamPrfReportEntity dataEntity;
        public WeeklyTeamReport_Report(WeeklyTeamPrfReportEntity entity)
        {
            InitializeComponent();
            dataEntity = entity;
            BindData();
        }
        private void XrPivotGrid_CustomFieldSort(object sender, DevExpress.XtraReports.UI.PivotGrid.PivotGridCustomFieldSortEventArgs e)
        {
            if (e.Field.FieldName == DatabaseObjects.Columns.Status)
            {
                object orderValue1 = e.GetListSourceColumnValue(e.ListSourceRowIndex1, "Sequence"),
                    orderValue2 = e.GetListSourceColumnValue(e.ListSourceRowIndex2, "Sequence");
                e.Result = Comparer.Default.Compare(orderValue1, orderValue2);
                e.Handled = true;
            }
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
            field = new XRPivotGridField("Sequence", DevExpress.XtraPivotGrid.PivotArea.RowArea);
            field.SortOrder = DevExpress.XtraPivotGrid.PivotSortOrder.Ascending;
            field.Visible = false;

            field = new XRPivotGridField(DatabaseObjects.Columns.Category, DevExpress.XtraPivotGrid.PivotArea.RowArea);
            field.SortOrder = DevExpress.XtraPivotGrid.PivotSortOrder.Ascending;
            field.Caption = "Category";

            field.MinWidth = 120;
            field.Width = 120;
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

            field = new XRPivotGridField(DatabaseObjects.Columns.Status, DevExpress.XtraPivotGrid.PivotArea.RowArea);
            field.SortMode = DevExpress.XtraPivotGrid.PivotSortMode.Custom;
            xrPivotGrid.CustomFieldSort += XrPivotGrid_CustomFieldSort;
            field.Caption = "Status"; //"Primary Responsible Person (PRP)";
            field.MinWidth = 70;
            field.Width = 70;
            field.Appearance.FieldHeader.WordWrap = true;
            field.Appearance.FieldValue.WordWrap = true;
            field.Appearance.FieldValue.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            // field.Appearance.FieldValue.BackColor = Color.White;
            field.Appearance.FieldValue.Font = new Font(field.Appearance.FieldValue.Font, FontStyle.Bold);
            field.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;

            field.Appearance.FieldHeader.Font = new Font(field.Appearance.FieldHeader.Font, FontStyle.Bold);
            field.Appearance.FieldHeader.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.GrandTotalCell.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.FieldValueGrandTotal.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.Cell.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.FieldValueGrandTotal.Font = new Font(field.Appearance.FieldValueGrandTotal.Font, FontStyle.Bold);
            field.Appearance.GrandTotalCell.BackColor = Color.White;

            xrPivotGrid.Fields.Add(field);


            field = new XRPivotGridField(DatabaseObjects.Columns.Priority, DevExpress.XtraPivotGrid.PivotArea.ColumnArea);
            field.SortOrder = DevExpress.XtraPivotGrid.PivotSortOrder.Ascending;
            field.Caption = "";

            field.MinWidth = 70;
            field.Width = 70;
            field.Appearance.FieldHeader.WordWrap = true;
            field.Appearance.FieldValue.WordWrap = true;
            field.Appearance.FieldValue.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            //  field.Appearance.FieldValue.BackColor = Color.White;

            field.Appearance.FieldValue.Font = new Font(field.Appearance.FieldValue.Font, FontStyle.Bold);
            field.Appearance.FieldHeader.BackColor = Color.Gray;
            field.Appearance.FieldHeader.Font = new Font(field.Appearance.FieldHeader.Font, FontStyle.Bold);
            field.Appearance.FieldHeader.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.Cell.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.CustomTotals;
            field.Appearance.Cell.TextVerticalAlignment = DevExpress.Utils.VertAlignment.Center;
            field.Appearance.GrandTotalCell.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.GrandTotalCell.Font = new Font(field.Appearance.FieldValue.Font, FontStyle.Bold);
            field.Appearance.FieldValueGrandTotal.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.FieldValueGrandTotal.Font = new Font(field.Appearance.FieldValue.Font, FontStyle.Bold);
          //  field.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Custom;

            //XRPivotGridCustomTotal cs = new XRPivotGridCustomTotal();
            //cs.Tag = "Created";
            //cs.Format.FormatString = "Created {0} ";
            //cs.Format.FormatType = DevExpress.Utils.FormatType.Custom;
            //field.CustomTotals.Add(cs);
            //XRPivotGridCustomTotal closed = new XRPivotGridCustomTotal();
            //closed.Tag = "Custom";
            //closed.Format.FormatString = "Closed {0} ";
            //closed.Format.FormatType = DevExpress.Utils.FormatType.Custom;
            //field.CustomTotals.Add(closed);


            xrPivotGrid.Fields.Add(field);


            field = new XRPivotGridField("Count", DevExpress.XtraPivotGrid.PivotArea.DataArea);
            field.Caption = string.Empty;
            field.Appearance.FieldHeader.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            field.Appearance.Cell.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
           field.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Custom;

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
           xrPivotGrid.CustomSummary += XrPivotGrid_CustomSummary;

       
        }

        private void XrPivotGrid_CustomSummary(object sender, DevExpress.XtraReports.UI.PivotGrid.PivotGridCustomSummaryEventArgs e)
        {
            XRPivotGrid pivot = sender as XRPivotGrid;
            PivotDrillDownDataSource obj =  pivot.DataSource as PivotDrillDownDataSource;
           
            //this is Grand Total cell
            // e.CustomValue = "Grand Total";
            PivotDrillDownDataSource ds = e.CreateDrillDownDataSource(); 
           
            int sumCreated = 0;
            int sumClosed = 0;
            for (int i = 0; i < ds.RowCount; i++)
            {
                PivotDrillDownDataRow row = ds[i];//
                
                if (ReferenceEquals(e.ColumnField, null) && ReferenceEquals(e.RowField, null))
                {
                    if (row[DatabaseObjects.Columns.Status] != null && Convert.ToString(row[DatabaseObjects.Columns.Status]) == "Created")
                    {
                        sumCreated += Convert.ToInt32(row["Count"]);
                    }
                    else if (row[DatabaseObjects.Columns.Status] != null && Convert.ToString(row[DatabaseObjects.Columns.Status]) == "Closed")
                    {
                        sumClosed += Convert.ToInt32(row["Count"]);
                    }
                    e.CustomValue = sumCreated + "/" + sumClosed;
                }
                else if(!ReferenceEquals(e.ColumnField, null) && ReferenceEquals(e.RowField, null))
                {
                    if (row[DatabaseObjects.Columns.Status] != null && Convert.ToString(row[DatabaseObjects.Columns.Status]) == "Created")
                    {
                        sumCreated += Convert.ToInt32(row["Count"]);
                    }
                    else if (row[DatabaseObjects.Columns.Status] != null && Convert.ToString(row[DatabaseObjects.Columns.Status]) == "Closed")
                    {
                        sumClosed += Convert.ToInt32(row["Count"]);
                    }
                    e.CustomValue = sumCreated + "/" + sumClosed;
                }
                else
                {
                    sumCreated += Convert.ToInt32(row["Count"]);
                    e.CustomValue = sumCreated;
                }
                   

            }
           
        }


        private void XrPivotGrid_PrintFieldValue(object sender, DevExpress.XtraReports.UI.PivotGrid.CustomExportFieldValueEventArgs e)
        {
            if (!e.IsColumn && ((e.Field != null && e.Field.FieldName == DatabaseObjects.Columns.Status) || e.ValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal))
            {
                e.Appearance.BackColor = Color.White;
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);

                if (e.MinIndex % 2 == 0)
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#EAEAEA");
            }


        }
        private void XrPivotGrid_PrintHeader(object sender, DevExpress.XtraReports.UI.PivotGrid.CustomExportHeaderEventArgs e)
        {

        }

        private void XrPivotGrid_PrintCell(object sender, DevExpress.XtraReports.UI.PivotGrid.CustomExportCellEventArgs e)
        {
            if (e.RowIndex % 2 == 0)
                e.Appearance.BackColor = ColorTranslator.FromHtml("#EAEAEA");
        }

        private void XrPivotGrid_FieldValueDisplayText(object sender, DevExpress.XtraReports.UI.PivotGrid.PivotFieldDisplayTextEventArgs e)
        {
            if (e.ValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal && e.IsColumn)
            {
                e.DisplayText = "Total";
                //  e.Field.Appearance.FieldValueGrandTotal.TextHorizontalAlignment= DevExpress.Utils.HorzAlignment.Far;
            }
            else if (e.ValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal && !e.IsColumn)
            {
                e.DisplayText = "Total (Created/Closed)";
              //  e.Field.Appearance.FieldValueGrandTotal.Font = new Font(e.Field.Appearance.FieldValue.Font, FontStyle.Bold);
            }
        }
    }
}
