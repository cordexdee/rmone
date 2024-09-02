using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Web;
using System.Collections.Generic;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class UGITLogViewer : UserControl
    {
        private string TenantID = string.Empty;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        UGITLogManager logManager = null;

        protected override void OnInit(EventArgs e)
        {
            //TenantID = Session["TenantID"].ToString();
            logManager = new UGITLogManager(context);

            grid.SettingsPager.AlwaysShowPager = false;
            grid.SettingsPager.Mode = GridViewPagerMode.ShowPager;
            grid.SettingsPager.ShowDisabledButtons = true;
            grid.SettingsPager.ShowNumericButtons = true;
            grid.SettingsPager.ShowSeparators = true;
            grid.SettingsPager.ShowDefaultImages = true;
            grid.AutoGenerateColumns = false;
            grid.SettingsText.EmptyHeaders = "  ";
            grid.SettingsPager.PageSizeItemSettings.Visible = true;
            grid.SettingsPager.PageSizeItemSettings.Position = DevExpress.Web.PagerPageSizePosition.Right;
            grid.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            grid.Styles.AlternatingRow.CssClass = "ms-alternatingstrong";
            grid.Settings.GridLines = GridLines.Horizontal;
            grid.SettingsBehavior.AllowDragDrop = false;
            grid.Settings.ShowHeaderFilterBlankItems = false;
            grid.SettingsPager.PageSize = 20;
            grid.Settings.ShowHeaderFilterButton = true;
           
            GenerateColumns();

            List<UGITLog> data = logManager.Load().OrderByDescending(x => x.Created).ToList();

            grid.DataSource = data;
            grid.DataBind();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {

        }

        protected void grid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

        }

        protected void grid_BeforeHeaderFilterFillItems(object sender, DevExpress.Web.ASPxGridViewBeforeHeaderFilterFillItemsEventArgs e)
        {

        }

        protected void grid_HeaderFilterFillItems(object sender, DevExpress.Web.ASPxGridViewHeaderFilterEventArgs e)
        {

        }

        private void GenerateColumns()
        {
            GridViewDataDateColumn dateTimeColumn = new GridViewDataDateColumn();
            dateTimeColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            dateTimeColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            dateTimeColumn.FieldName = DatabaseObjects.Columns.Created;
            dateTimeColumn.Caption = "Date";
            dateTimeColumn.PropertiesEdit.DisplayFormatString = "{0:yyyy-MM-dd HH:mm:ss}";
            dateTimeColumn.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
            dateTimeColumn.HeaderStyle.Font.Bold = true;
            dateTimeColumn.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            dateTimeColumn.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            grid.Columns.Add(dateTimeColumn);

            GridViewDataTextColumn colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = "ItemUser"; //DatabaseObjects.Columns.TicketUser;
            colId.Caption = "User";
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            grid.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.CategoryName;
            colId.Caption = "Category";
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            grid.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.Severity;
            colId.Caption = DatabaseObjects.Columns.Severity;
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            grid.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.TicketId;
            colId.Caption = "Ticket ID";
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            grid.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.UGITDescription;
            colId.Caption = "Message";
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            grid.Columns.Add(colId);
        }

        protected void grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == DatabaseObjects.Columns.UGITDescription)
            {
                e.DisplayText = UGITUtility.TruncateWithEllipsis(Convert.ToString(e.Value), 50);   //uHelper.TruncateWithEllipsis(Convert.ToString(e.Value), 50);
            }
        }
    }
}
