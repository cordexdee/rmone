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
using uGovernIT.Manager.Managers;
using System.Web;

namespace uGovernIT.Web
{
    public partial class ProjectTemplateView : UserControl
    {
        TaskTemplateManager TaskTemplateMGR = new TaskTemplateManager(HttpContext.Current.GetManagerContext());
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            GenerateColumns();
            GridDataBind();
            hdnInformation.Set("UpdateUrl", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=projecttemplateedit"));
            hdnInformation.Set("RequestUrl", Request.Url.AbsoluteUri);
            base.OnInit(e);
        }

        private void GridDataBind()
        {
            LifeCycleManager lifeCycleManager = new LifeCycleManager(HttpContext.Current.GetManagerContext());
            //SPListHelper.GetDataTable(DatabaseObjects.Lists.UGITTaskTemplates);
            DataTable templates = TaskTemplateMGR.GetDataTable(); // GetTableDataManager.GetTableData(DatabaseObjects.Tables.UGITTaskTemplates);
            if (templates != null)
            {
                int cnt = 0;
                foreach (DataRow dr in templates.Rows)
                {
                    if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(templates.Rows[cnt][DatabaseObjects.Columns.ProjectLifeCycleLookup])))
                    {
                        var lifeCycleName = lifeCycleManager.Get($"Where ID={templates.Rows[cnt][DatabaseObjects.Columns.ProjectLifeCycleLookup]}");
                        dr[DatabaseObjects.Columns.ProjectLifeCycleLookup] = Convert.ToString(lifeCycleName.Name);
                    }
                    cnt++;
                }


                grid.DataSource = templates;
                grid.DataBind();
            }
        }

        private void GenerateColumns()
        {
            if (grid.Columns.Count <= 0)
            {

                GridViewDataTextColumn colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.ProjectLifeCycleLookup;
                colId.Caption = "Lifecycle";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.GroupIndex = 0;
                grid.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.Title;
                colId.Caption = DatabaseObjects.Columns.Title;
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.PropertiesTextEdit.EncodeHtml = false;
                grid.Columns.Add(colId);
               
               
                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.UGITDescription;
                colId.Caption = "Description";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.PropertiesTextEdit.EncodeHtml = false;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns.Add(colId);

                //colId = new GridViewDataTextColumn();
                //colId.FieldName = DatabaseObjects.Columns.Author;
                //colId.Caption = "Created By";
                //colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                //colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                //colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                //colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                //colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                //grid.Columns.Add(colId);

                GridViewDataDateColumn colIdDate = new GridViewDataDateColumn();
                colIdDate.FieldName = DatabaseObjects.Columns.Created;
                colIdDate.Caption = "Created On";
                colIdDate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colIdDate.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colIdDate.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                colIdDate.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colIdDate.PropertiesDateEdit.EditFormatString = "MMM-dd-yyy";
                colIdDate.PropertiesEdit.DisplayFormatString = "MMM-dd-yyy";
                grid.Columns.Add(colIdDate);


                GridViewCommandColumn colAction = new GridViewCommandColumn();
                colAction.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colAction.ButtonRenderMode = GridCommandButtonRenderMode.Image;
                colAction.Caption = " ";
                GridViewCommandColumnCustomButton customButton = new GridViewCommandColumnCustomButton();
                customButton.Image.Url = "/Content/images/editNewIcon.png";
                customButton.Image.AlternateText = "Edit";
                customButton.Image.ToolTip = "Edit Template";
                customButton.Image.Width = 16;
                customButton.ID = "editButton";
                customButton.Index = 2;
                colAction.Width = 60;
                colAction.CustomButtons.Add(customButton);

                colAction.ShowDeleteButton = true;
                grid.SettingsCommandButton.DeleteButton.Image.Url = "/Content/images/grayDelete.png";
                grid.SettingsCommandButton.DeleteButton.Image.AlternateText = "Delete";
                grid.SettingsCommandButton.DeleteButton.Image.Width = 16;

                grid.Columns.Add(colAction);
            }
        }

        protected void grid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int itemID = Convert.ToInt32(e.Keys[0]);
            TaskTemplate templates = TaskTemplateMGR.LoadByID(itemID); //GetTableDataManager.GetTableData(DatabaseObjects.Tables.UGITTaskTemplates);
            //SPListItem item = SPListHelper.GetSPListItem(DatabaseObjects.Lists.UGITTaskTemplates, itemID);
            if (templates != null)
            {
                TaskTemplateMGR.Delete(templates);
                e.Cancel = true;
                GridDataBind();
            }
        }

        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                if (e.Row.Cells.Count > 2 && e.Row.Cells[2].GetType().FullName == "DevExpress.Web.Rendering.GridViewTableDataCell")
                {
                    DataRow row = grid.GetDataRow(e.VisibleIndex);
                    e.Row.Cells[1].Text = string.Format("<a href='javascript:openEditBox({1})'>{0}</a>", row[DatabaseObjects.Columns.Title], row[DatabaseObjects.Columns.Id]);
                }
            }
        }
    }
}
