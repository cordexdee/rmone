using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using Utils;
using System.Data.SqlClient;
using DevExpress.Web;
using DevExpress.Web.Rendering;
using System.Linq;
using uGovernIT.DAL;
using uGovernIT.Utility.Entities;
using System.Web;
using uGovernIT.Manager.Core;

namespace uGovernIT.Web
{
    public partial class RequestRoleWriteAccess : UserControl
    {      
        protected string addNewItemUrl;
        public string Module { get; set; }
        public int ItemID { get; set; } 
        List<ModuleRoleWriteAccess> roleWriteAccess;
        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        RequestRoleWriteAccessManager ObjRequestRoleWriteAccessManager = new RequestRoleWriteAccessManager(HttpContext.Current.GetManagerContext());
        LifeCycleStageManager ObjLifeCycleStageManager = new LifeCycleStageManager(HttpContext.Current.GetManagerContext());
        FormLayoutManager ObjFormLayoutManager = new FormLayoutManager(HttpContext.Current.GetManagerContext());
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        protected override void OnInit(EventArgs e)
        {
            grid.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            grid.Styles.AlternatingRow.CssClass = "ms-alternatingstrong";
            BindModule();
            BindGridView();
           
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {          
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        private void BindGridView()
        {            
            List<ModuleRoleWriteAccess> dtRoleWriteAccess = ObjRequestRoleWriteAccessManager.Load();
            List<ModuleRoleWriteAccess> target = new List<ModuleRoleWriteAccess>();
            //dtRoleWriteAccess.Columns.Add("StageTitle", typeof(System.String));
            List<LifeCycleStage> dtModuleStep = ObjLifeCycleStageManager.Load(x => x.ModuleNameLookup == Module);
            LifeCycleStage drZeroStep = new LifeCycleStage(); 
            drZeroStep.StageStep = 0;
            dtModuleStep.Add(drZeroStep);
            List<ModuleFormLayout> dtFormLayout = ObjFormLayoutManager.LoadLayout().Where(x => x.ID == ItemID).ToList();
            if (dtFormLayout != null && dtFormLayout.Count > 0)
                roleWriteAccess = ObjRequestRoleWriteAccessManager.Load(x => x.ModuleNameLookup == Module && (x.FieldName == dtFormLayout[0].FieldName || (x.FieldName == dtFormLayout[0].FieldDisplayName && dtFormLayout[0].FieldName.StartsWith("#"))));

            if (roleWriteAccess == null)
                roleWriteAccess = new List<ModuleRoleWriteAccess>();

            if (Module.EqualsIgnoreCase(ModuleNames.PMM))
            {
                ModuleRoleWriteAccess drResult = new ModuleRoleWriteAccess();                              
                drResult.Title = null;                
                drResult = roleWriteAccess.FirstOrDefault(m => m.StageStep == Convert.ToInt16(drZeroStep.StageStep));
                if (drResult == null)
                {
                    drResult = new ModuleRoleWriteAccess();
                    drResult.StageStep = drZeroStep.StageStep;
                    drResult.Title = drZeroStep.StageTitle;
                }
                else
                {
                    drResult.Title = drZeroStep.StageTitle;
                }
                target.Add(drResult);
            }
            else
            {                
                foreach (LifeCycleStage dr in dtModuleStep)
                {

                    ModuleRoleWriteAccess drResult = roleWriteAccess.FirstOrDefault(m => m.StageStep == Convert.ToInt16(dr.StageStep));
                    if (drResult == null)
                    {
                        drResult = new ModuleRoleWriteAccess();
                        drResult.StageStep = dr.StageStep;
                        drResult.Title = dr.StageTitle;
                    }
                    else
                    {
                        drResult.Title = dr.StageTitle;
                    }
                    target.Add(drResult);
                }
            }
            
            grid.DataSource = target.OrderBy(x => x.StageStep);
            grid.DataBind();
        }

        private void BindModule()
        {
            List<UGITModule> dtModule = ObjModuleViewManager.LoadAllModule().OrderBy(x=>x.ModuleName).ToList();
            ddlModule.Items.Clear();
            if (dtModule!=null && dtModule.Count > 0)
            {             
                for (int i = 0; i < dtModule.Count; i++)
                {
                    ddlModule.Items.Add(new ListItem { Text = dtModule[i].Title.ToString(), Value = dtModule[i].ModuleName.ToString() });
                }
                ddlModule.DataBind();
            }
        }
        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            string moduleName = ddlModule.SelectedValue;
            string url = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=formlayoutandpermission&pageTitle=Form Layout&isdlg=1&isudlg=1&module=" + moduleName);
            Response.Redirect(url);
        }

        DataTable GetActionUser()
        {
            DataTable dtGroups = new DataTable();
            dtGroups.Columns.Add("ID");
            dtGroups.Columns.Add("Name");
            dtGroups.Columns.Add("Type");
            ModuleUserTypeManager ObjModuleUserTypeManager = new ModuleUserTypeManager(HttpContext.Current.GetManagerContext());
            List<ModuleUserType> dtModuleUserType = ObjModuleUserTypeManager.Load(x => x.ModuleNameLookup == Module);
           
            if(dtModuleUserType != null && dtModuleUserType.Count() > 0)
            {
                List<ModuleUserType> dtUserTypes = dtModuleUserType;
                foreach (ModuleUserType drUserTypes in dtUserTypes)
                {
                    DataRow dr = dtGroups.NewRow();
                    dr["ID"] = drUserTypes.ID;
                    dr["Name"] = drUserTypes.ColumnName;
                    dr["Type"] = "Role";
                    dtGroups.Rows.Add(dr);
                }
            }
            UserRoleManager roleManager = new UserRoleManager(context);
            foreach (Role uRole in roleManager.Load().ToList())
            {
                DataRow dr = dtGroups.NewRow();
                dr["ID"] = uRole.Id;
                dr["Name"] = uRole.Name;
                dr["Type"] = "Group";
                dtGroups.Rows.Add(dr);
            }
            return dtGroups;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
           // bool updatedAll = false;

            for (int i = 0; i < grid.VisibleRowCount; i++)
            {
                int ID = grid.GetRowValues(i, grid.KeyFieldName) is DBNull ? 0 : Convert.ToInt32(grid.GetRowValues(i, grid.KeyFieldName));
                CheckBox cbShowEditButton, cbShowWithCheckBox, cbHideInMapping, cbFieldMandatory, chkEditable;
                ASPxGridLookup glActionUser;
                //checking if selected All, All always will be on 0 index
                bool selectedAll= (grid.FindRowCellTemplateControl(0, grid.Columns["Editable"] as GridViewDataColumn, "chkEditable") as CheckBox).Checked;

                if (selectedAll)
                {
                    cbShowEditButton = (CheckBox)grid.FindRowCellTemplateControl(0, grid.Columns[DatabaseObjects.Columns.ShowEditButton] as GridViewDataColumn, "cbShowEditButton");
                    cbShowWithCheckBox = (CheckBox)grid.FindRowCellTemplateControl(0, grid.Columns[DatabaseObjects.Columns.ShowWithCheckBox] as GridViewDataColumn, "cbShowWithCheckBox");
                    cbHideInMapping = (CheckBox)grid.FindRowCellTemplateControl(0, grid.Columns[DatabaseObjects.Columns.HideInServiceMapping] as GridViewDataColumn, "cbHideInService");
                    cbFieldMandatory = (CheckBox)grid.FindRowCellTemplateControl(0, grid.Columns[DatabaseObjects.Columns.FieldMandatory] as GridViewDataColumn, "cbFieldMandatory");
                    glActionUser = (ASPxGridLookup)grid.FindRowCellTemplateControl(0, grid.Columns["ActionUser"] as GridViewDataColumn, "glActionUser");
                    chkEditable = grid.FindRowCellTemplateControl(0, grid.Columns["Editable"] as GridViewDataColumn, "chkEditable") as CheckBox;
                }
                else
                {
                    cbShowEditButton = (CheckBox)grid.FindRowCellTemplateControl(i, grid.Columns[DatabaseObjects.Columns.ShowEditButton] as GridViewDataColumn, "cbShowEditButton");
                    cbShowWithCheckBox = (CheckBox)grid.FindRowCellTemplateControl(i, grid.Columns[DatabaseObjects.Columns.ShowWithCheckBox] as GridViewDataColumn, "cbShowWithCheckBox");
                    cbHideInMapping = (CheckBox)grid.FindRowCellTemplateControl(i, grid.Columns[DatabaseObjects.Columns.HideInServiceMapping] as GridViewDataColumn, "cbHideInService");
                    cbFieldMandatory = (CheckBox)grid.FindRowCellTemplateControl(i, grid.Columns[DatabaseObjects.Columns.FieldMandatory] as GridViewDataColumn, "cbFieldMandatory");
                    glActionUser = (ASPxGridLookup)grid.FindRowCellTemplateControl(i, grid.Columns["ActionUser"] as GridViewDataColumn, "glActionUser");
                    chkEditable = grid.FindRowCellTemplateControl(i, grid.Columns["Editable"] as GridViewDataColumn, "chkEditable") as CheckBox;
                }

                ModuleRoleWriteAccess moduleRoleWriteAccess = ObjRequestRoleWriteAccessManager.LoadByID(ID);
                if (moduleRoleWriteAccess !=null)
                {
                    // For updating existing entry.
                    if (chkEditable.Checked)
                    {
                        moduleRoleWriteAccess.ShowEditButton= cbShowEditButton.Checked;
                        moduleRoleWriteAccess.ShowWithCheckbox= cbShowWithCheckBox.Checked;
                        moduleRoleWriteAccess.FieldMandatory= cbFieldMandatory.Checked;
                        moduleRoleWriteAccess.HideInServiceMapping= cbHideInMapping.Checked;
                        if (glActionUser != null)
                            moduleRoleWriteAccess.ActionUser = glActionUser.Text.Replace(";", ";#");
                        ObjRequestRoleWriteAccessManager.AddOrUpdate(moduleRoleWriteAccess);
                        //spListItem.Update();
                    }
                    else
                    {
                        ObjRequestRoleWriteAccessManager.Delete(moduleRoleWriteAccess);
                    }
                }
                else
                {
                    // For new entry.
                    if (chkEditable.Checked)
                    {
                        string fieldName = string.Empty;
                        //SPListItem spFormLayoutItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.FormLayout, ItemID);
                        ModuleFormLayout moduleFormLayout = ObjFormLayoutManager.LoadLayout().FirstOrDefault(x => x.ID == ItemID);
                      
                        if (moduleFormLayout != null)
                        {
                            fieldName = Convert.ToString(moduleFormLayout.FieldName);
                        }

                        if (fieldName == "#Control#")
                        {
                            fieldName = Convert.ToString(moduleFormLayout.FieldDisplayName);
                        }

                        if (!string.IsNullOrEmpty(fieldName))
                        {
                            //spListItem = SPListHelper.GetSPList(DatabaseObjects.Lists.RequestRoleWriteAccess).AddItem();
                            ModuleRoleWriteAccess ObjModuleRoleWriteAccess = new ModuleRoleWriteAccess();
                            Dictionary<string, object> values = new Dictionary<string, object>();
                            if (cbShowEditButton != null)
                                ObjModuleRoleWriteAccess.ShowEditButton= cbShowEditButton.Checked;
                            if (cbShowWithCheckBox != null)
                                ObjModuleRoleWriteAccess.ShowWithCheckbox=cbShowWithCheckBox.Checked;

                            if (cbFieldMandatory != null)
                                ObjModuleRoleWriteAccess.FieldMandatory=cbFieldMandatory.Checked;

                            if (glActionUser != null)
                                ObjModuleRoleWriteAccess.ActionUser=glActionUser.Text.Replace(";", ";#");

                            ObjModuleRoleWriteAccess.HideInServiceMapping= cbHideInMapping.Checked;
                            ObjModuleRoleWriteAccess.ModuleNameLookup= Module;
                            ObjModuleRoleWriteAccess.StageStep= UGITUtility.StringToInt(grid.GetRowValues(i, DatabaseObjects.Columns.ModuleStep));
                            ObjModuleRoleWriteAccess.Title= string.Format("{0}- {1}", Convert.ToString(grid.GetRowValues(i, DatabaseObjects.Columns.ModuleStep)), fieldName);
                            ObjModuleRoleWriteAccess.FieldName= fieldName;
                            ObjRequestRoleWriteAccessManager.AddOrUpdate(ObjModuleRoleWriteAccess);
                        }
                    }
                }
            }
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
        

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.ShowEditButton)
            {
                CheckBox cbASPxCheckBox = grid.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "cbShowEditButton") as CheckBox;
                cbASPxCheckBox.Checked = Convert.ToString(e.CellValue) == "True" ? true : false;
                cbASPxCheckBox.Enabled = !(UGITUtility.StringToInt(e.KeyValue) == 0);
            }
            else if (e.DataColumn.FieldName == DatabaseObjects.Columns.ShowWithCheckbox)
            {
                CheckBox cbShowWithCheckBox = grid.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "cbShowWithCheckBox") as CheckBox;
                cbShowWithCheckBox.Checked = Convert.ToString(e.CellValue) == "True" ? true : false;
                cbShowWithCheckBox.Enabled = !(UGITUtility.StringToInt(e.KeyValue) == 0);
            }
            else if (e.DataColumn.FieldName == DatabaseObjects.Columns.HideInServiceMapping)
            {
                CheckBox cbHideInService = grid.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "cbHideInService") as CheckBox;
                cbHideInService.Checked = Convert.ToString(e.CellValue) == "True" ? true : false;
                cbHideInService.Enabled = !(UGITUtility.StringToInt(e.KeyValue) == 0);
            }
            else if (e.DataColumn.FieldName == DatabaseObjects.Columns.FieldMandatory)
            {
                CheckBox cbFieldMandatory = grid.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "cbFieldMandatory") as CheckBox;
                cbFieldMandatory.Checked = Convert.ToString(e.CellValue) == "True" ? true : false;
                cbFieldMandatory.Enabled = !(UGITUtility.StringToInt(e.KeyValue) == 0); 
            }
            else if (e.DataColumn.FieldName == "ID")
            {
                CheckBox chkEditable = grid.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkEditable") as CheckBox;
                chkEditable.Checked =UGITUtility.StringToInt(e.CellValue) == 0 ? false : true;
                chkEditable.Attributes.Add("onchange", "OnEditableCheckedChanged(this," + e.VisibleIndex + ");");
            }
        }

        protected void glActionUser_Load(object sender, EventArgs e)
        {
            ASPxGridLookup glActionUser = sender as ASPxGridLookup;
            GridViewDataItemTemplateContainer container = glActionUser.NamingContainer as GridViewDataItemTemplateContainer;

            ModuleRoleWriteAccess dr = grid.GetRow(container.VisibleIndex) as ModuleRoleWriteAccess;
            glActionUser.DataSource = GetActionUser();
            glActionUser.DataBind();

            if (!IsPostBack)
            {
                if (!string.IsNullOrWhiteSpace(dr.ActionUser)){
                    foreach (string val in UGITUtility.ObjectToString(dr.ActionUser).Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        glActionUser.GridView.Selection.SelectRowByKey(val);
                    }
                }
            }               
        }

        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            if (Convert.ToString(e.GetValue(DatabaseObjects.Columns.ModuleStep)) == "0")
            {
                foreach (object cell in e.Row.Cells)
                {
                    if (cell is GridViewTableDataCell)
                    {
                        GridViewTableDataCell editCell = (GridViewTableDataCell)cell;

                        if (((GridViewDataColumn)editCell.Column).FieldName == DatabaseObjects.Columns.ModuleStep)
                        {
                            e.Row.Cells[((GridViewDataColumn)editCell.Column).Index].Text = "ALL";
                        }

                    }
                }
            }
        }
      
    }
}
