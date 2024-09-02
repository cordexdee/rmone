using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Manager;
using DevExpress.Web;
using System.Web;
using uGovernIT.Manager.Core;

namespace uGovernIT.Web
{ 
    public partial class ModuleStageTaskTemplateEdit : UserControl
    {
        ModuleStageConstraintTemplates taskTemplateItem;
        public int TemplateId = 0;
        List<ModuleStageConstraintTemplates> templateList;
        List<LifeCycleStage> moduleStages;
        ModuleStageConstraintTemplatesManager objModuleStageConstraintTemplatesManager = new ModuleStageConstraintTemplatesManager(HttpContext.Current.GetManagerContext());
        LifeCycleStageManager lifeCycleStageManager = new LifeCycleStageManager(HttpContext.Current.GetManagerContext());
        #region page events
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                BindModuleName();
                if (Request["moduleName"] != null)
                {
                    ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(Request["moduleName"]));
                }

                BindModuleStep(ddlModule.SelectedValue);
               
                //  BindRoleTypeByModule();

                FillFieldName();
            }
            templateList = objModuleStageConstraintTemplatesManager.Load();
            moduleStages = lifeCycleStageManager.Load();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DdlUserGroups(ddlModule.SelectedValue);
            if (!Page.IsPostBack)
            {
                if (TemplateId != 0)
                {
                    if (templateList != null && templateList.Count > 0)
                    {
                        taskTemplateItem = objModuleStageConstraintTemplatesManager.Load($"id= {TemplateId}" ).FirstOrDefault();
                        LoadData(taskTemplateItem);
                    }
                }
            }
           
            if (TemplateId > 0)
                tdDelete.Visible = true;
            else
                tdDelete.Visible = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        #endregion

        #region control events
        protected void btSaveTask_Click(object sender, EventArgs e)
        {
            int moduleStepId = 0;
            LifeCycleStage moduleStageItem = null;
            if (moduleStages != null)
            {
                moduleStageItem = moduleStages.Where(x => x.StageStep == Convert.ToInt32(ddlModuleStep.SelectedValue)).FirstOrDefault();
                if (moduleStageItem != null)
                {
                    moduleStepId = moduleStageItem.StageStep;
                }
            }

            ModuleStageConstraintTemplates task = new ModuleStageConstraintTemplates();
            task.Body = UGITUtility.StripHTML(txtDescription.Text);
            task.EstimatedHours = Convert.ToInt32(UGITUtility.StringToDouble(txtEstimatedHours.Text));
            if (!string.IsNullOrEmpty(txtItemOrder.Text))
            {
                task.ItemOrder = Convert.ToInt32(txtItemOrder.Text);
            }
            string assignedUser = string.Empty;
            List<string> assignedUsers = new List<string>();
            //SPFieldUserValueCollection userMultiLookup = new SPFieldUserValueCollection();
            //List<string> userMultiLookup = new List<string>();
            //if (trAssignedTo.Visible && peAssignedTo.Accounts.Count > 0)
            //{
            //    for (int i = 0; i < peAssignedTo.Accounts.Count; i++)
            //    {
            //        PickerEntity entity = (PickerEntity)peAssignedTo.Entities[i];
            //        if (entity != null && entity.Key != null)
            //        {
            //            SPUser user = UserProfile.GetUserByName(entity.Key, SPPrincipalType.User);
            //            if (user != null)
            //            {
            //                SPFieldUserValue userLookup = new SPFieldUserValue();
            //                userLookup.LookupId = user.ID;
            //                userMultiLookup.Add(userLookup);
            //            }
            //        }
            //    }
            //}

            task.ID = TemplateId;
            //task.StartDate = DateTime.Now;
            //task.TaskDueDate = DateTime.Now;
            task.Body = UGITUtility.StripHTML(txtDescription.Text);
            task.TaskActualHours = Convert.ToInt32(UGITUtility.StringToDouble(txtEstimatedHours.Text));
            task.AssignedTo = peAssignedTo.GetValues();
            task.Title = txtTitle.Text.Trim();
            task.ModuleStep = Convert.ToInt32(moduleStepId);
            task.ModuleNameLookup = ddlModule.SelectedValue;

            if (ddlModuleDates.SelectedValue != null && ddlModuleDates.SelectedValue != "Select")
            {
                // need disscusion
                if (!string.IsNullOrEmpty(txtDays.Text))
                    task.DateExpression = ddlModuleDates.SelectedValue + Constants.Separator + ddlOperator.SelectedValue + Constants.Separator1 + txtDays.Text;
                else
                    task.DateExpression = ddlModuleDates.SelectedValue + Constants.Separator + ddlOperator.SelectedValue + Constants.Separator1 + "1";
            }

            if (glUserType.Value != null && glUserType.Value.ToString() != "")
                task.UserRoleType = glUserType.Value.ToString();
            else
                task.UserRoleType = null;
            if (task.ID > 0)
                objModuleStageConstraintTemplatesManager.Update(task);
            else
            {
                objModuleStageConstraintTemplatesManager.Insert(task);
            }
               
            //UGITModuleConstraint.SaveTask(SPContext.Current.Web, ref task, DatabaseObjects.Lists.ModuleStageConstraintTemplates);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindModuleStep(ddlModule.SelectedValue);
            //BindRoleTypeByModule();
            FillFieldName();
            DdlUserGroups(ddlModule.SelectedValue);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
           
            if (templateList != null)
            {
                ModuleStageConstraintTemplates objclass = objModuleStageConstraintTemplatesManager.LoadByID(TemplateId);
                if (objclass != null)
                { 
                    if(objclass.Deleted)
                        objclass.Deleted = false;
                    else
                        objclass.Deleted = true;
                }
                    
                objModuleStageConstraintTemplatesManager.Update(objclass);
                //objModuleStageConstraintTemplatesManager.Delete(objclass);
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        #endregion

        #region helpers

        private void BindModuleName()
        {
            ModuleViewManager objModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            List<UGITModule> lstModule = objModuleViewManager.Load(x => x.EnableModule).OrderBy(x => x.ModuleName).ToList();
            ddlModule.Items.Clear();
            if (lstModule.Count > 0)
            {
                ddlModule.DataSource = lstModule;
                ddlModule.DataTextField = DatabaseObjects.Columns.Title;
                ddlModule.DataValueField = DatabaseObjects.Columns.ModuleName;
                ddlModule.DataBind();
                ddlModule.SelectedIndex = 0;
                
            }

        }

        void BindModuleStep(string selectedModule)
        {
            try
            {
                ddlModuleStep.ClearSelection();
                ddlModuleStep.Items.Clear();
                var listModuleStep = lifeCycleStageManager.Load();
                var list = listModuleStep.Where(x => x.ModuleNameLookup == selectedModule).OrderBy(y => y.StageStep).ToList();

                if (list.Count > 0)
                {
                    int rowCounter = 1;
                    foreach (var row in list)
                    {
                        //This is done to remove the first and last module stage from the list.
                        if (rowCounter != 1 && rowCounter < list.Count)
                            ddlModuleStep.Items.Add(new ListItem(row.StageTitle, row.StageStep.ToString()));
                        rowCounter++;
                    }
                }

                ddlModuleStep.Items.Insert(0, new ListItem("Select",""));
            }
            catch (Exception)
            {
            }
        }

        private void LoadData(ModuleStageConstraintTemplates itemTemplate)
        {
            txtTitle.Text = Convert.ToString(taskTemplateItem.Title);
            txtItemOrder.Text = Convert.ToString(taskTemplateItem.ItemOrder);
            if (taskTemplateItem.Deleted)
            {
                hdnUserEnabledStatus.Set("isEnabled", "0"); 
                LnkbtnDelete.Text = "Enable";
            }
            else
            {
                hdnUserEnabledStatus.Set("isEnabled", "1");
                LnkbtnDelete.Text = "Disable";
            }
            
            txtDescription.Text = UGITUtility.StripHTML(Convert.ToString(taskTemplateItem.Body));
            {
                string ModuleName = Convert.ToString(taskTemplateItem.ModuleNameLookup);
                ddlModule.SelectedValue = ModuleName;
                BindModuleStep(ModuleName);
                DdlUserGroups(ModuleName);
                //BindRoleTypeByModule();
                FillFieldName();
            }

            if (!string.IsNullOrEmpty(itemTemplate.AssignedTo))
            {
                string userLookups = Convert.ToString(itemTemplate.AssignedTo);
                if (userLookups != null)
                {
                    peAssignedTo.SetValues(userLookups);
                    lbAssignedTo.Text = HttpContext.Current.GetUserManager().CommaSeparatedNamesFrom(userLookups);
                }
            }

            if (Convert.ToString(itemTemplate.EstimatedHours) != string.Empty)
                txtEstimatedHours.Text = Convert.ToString(itemTemplate.EstimatedHours);

            if (!string.IsNullOrEmpty(taskTemplateItem.DateExpression))
            {
                string[] dateExp = Convert.ToString(taskTemplateItem.DateExpression).Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                string[] dateDaysExp = dateExp[1].Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries);

                ddlModuleDates.SelectedIndex = ddlModuleDates.Items.IndexOf(ddlModuleDates.Items.FindByValue(dateExp[0].Trim()));

                ddlOperator.SelectedValue = Convert.ToString(ddlOperator.Items.FindByValue(dateDaysExp[0].Trim()));
                txtDays.Text = Convert.ToString(dateDaysExp[1]);

            }

            if (!string.IsNullOrEmpty(taskTemplateItem.UserRoleType))
            {
                DataTable dataTable = (DataTable)glUserType.DataSource;
                List<string> actionUsers = new List<string>();
                DataRow row = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("Type") == "User Type" && x.Field<string>("Name") == Convert.ToString(itemTemplate.UserRoleType).Trim());
                if (row != null)
                    actionUsers.Add(Convert.ToString(row["UserType"]));
                else
                    //actionUsers.Add(Convert.ToString(itemTemplate[DatabaseObjects.Columns.UserRoleType]).Trim());
                    actionUsers.Add(Convert.ToString(itemTemplate.UserRoleType).Trim());
                glUserType.Text = string.Join("; ", actionUsers.ToArray());
            }

            var moduleStageItems = moduleStages.Where(x => x.ModuleNameLookup == ddlModule.SelectedValue && x.StageStep == taskTemplateItem.ModuleStep).ToList();
            if (moduleStageItems != null && moduleStageItems.Count() > 0)
            {
                if (moduleStageItems[0] != null)
                {
                    ddlModuleStep.SelectedValue = Convert.ToString(moduleStageItems[0].StageStep);
                }
            }
        }

        protected void DdlUserGroups(string moduleName)
        {
            glUserType.DataSource = GetActionUser(moduleName);
            glUserType.DataBind();
        }

        DataTable GetActionUser(string moduleName)
        {
            ModuleUserTypeManager objModuleUserTypeManager = new ModuleUserTypeManager(HttpContext.Current.GetManagerContext());
            DataTable dtGroups = new DataTable();
            dtGroups.Columns.Add("ID");
            dtGroups.Columns.Add("Name");
            dtGroups.Columns.Add("NameRole");
            dtGroups.Columns.Add("UserType");
            dtGroups.Columns.Add("Type");

            EnumerableRowCollection<DataRow> rows = objModuleUserTypeManager.GetDataTable().AsEnumerable().Where(x => !string.IsNullOrEmpty(x.Field<string>(DatabaseObjects.Columns.ColumnName)) && x.Field<string>(DatabaseObjects.Columns.ModuleNameLookup) == moduleName);
            if (rows != null && rows.Count() > 0)
            {
                DataTable dtUserTypes = rows.CopyToDataTable();
                foreach (DataRow drUserTypes in dtUserTypes.Rows)
                {
                    DataRow dr = dtGroups.NewRow();
                    dr["ID"] = drUserTypes[DatabaseObjects.Columns.Id];
                    dr["Name"] = drUserTypes[DatabaseObjects.Columns.ColumnName];
                    dr["UserType"] = drUserTypes[DatabaseObjects.Columns.UserTypes];
                    dr["NameRole"] = string.Format("{0} ({1})", drUserTypes[DatabaseObjects.Columns.UserTypes], drUserTypes[DatabaseObjects.Columns.ColumnName]);
                    dr["Type"] = "User Type";
                    dtGroups.Rows.Add(dr);
                }
            }

            //SPGroupCollection collGroups = SPContext.Current.Web.SiteGroups;
            //foreach (SPGroup oGroup in collGroups)
            //{
            //    DataRow dr = dtGroups.NewRow();
            //    dr["ID"] = oGroup.ID;
            //    dr["Name"] = oGroup.Name;
            //    dr["UserType"] = oGroup.Name;
            //    dr["NameRole"] = oGroup.Name;
            //    dr["Type"] = "Group";
            //    dtGroups.Rows.Add(dr);
            //}

            return dtGroups;
        }

        private void FillFieldName()
        {
            ModuleViewManager objModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            UGITModule lstModule = objModuleViewManager.Load(x => x.ModuleName.Equals(ddlModule.SelectedValue)).FirstOrDefault();

            //SPListItem spListItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.Modules, ddlModule.SelectedItem.Text);
            DataTable SPTickets = null;
            if (lstModule != null)
            {
                SPTickets = objModuleViewManager.GetDataTable();
               // SPTickets = GetTableDataManager.GetTableData(lstModule.ModuleTable);// SPListHelper.GetSPList(Convert.ToString(spListItem[DatabaseObjects.Columns.ModuleTicketTable]));
            }
            List<string> fields = new List<string>();
            if (SPTickets != null)
            {
                foreach (DataColumn spField in SPTickets.Columns)
                {
                    if (Convert.ToString(spField.DataType) == "System.DateTime" || Convert.ToString(spField.DataType) == "DateTime" || Convert.ToString(spField.DataType) == "Date")
                    {
                        fields.Add(spField.ColumnName);
                    }
                }
                fields.Sort();
            }
            ddlModuleDates.DataSource = fields;
            ddlModuleDates.DataBind();
            ddlModuleDates.Items.Insert(0, "Select");
        }
        #endregion
    }
}
