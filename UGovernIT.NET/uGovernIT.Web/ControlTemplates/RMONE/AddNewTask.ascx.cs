using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class AddNewTask : System.Web.UI.UserControl
    {
        private ApplicationContext context = HttpContext.Current.GetManagerContext();
        private ConfigurationVariableManager _configVariableMGR = null;
        ModuleViewManager moduleViewManager = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            _configVariableMGR = new ConfigurationVariableManager(context);
            moduleViewManager = new ModuleViewManager(context);
        }
        protected void ddlLevel1_Load(object sender, EventArgs e)
        {
            DropDownList level1 = (DropDownList)sender;
            if (level1.Items.Count <= 0)
            {
                List<UGITModule> listModules = moduleViewManager.Load(x => x.EnableRMMAllocation).OrderBy(y => y.Title).ToList();
                level1.DataSource = listModules;
                level1.DataTextField = DatabaseObjects.Columns.Title;
                level1.DataValueField = DatabaseObjects.Columns.ModuleName;
                level1.DataBind();                
            }
            cbLevel2_Load(sender, e);
        }

        protected void ddlLevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbLevel2_Load(sender, e);
        }

        protected void cbLevel2_Load(object sender, EventArgs e)
        {
            bool fromModule = false;
            DataTable resultedTableLevel2 = null;
            if (ddlLevel1.SelectedItem != null && ddlLevel1.SelectedItem.Text != "--Select--")
            {
                DataRow[] drModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, ddlLevel1.SelectedItem.Text.Trim()));
                fromModule = drModules != null && drModules.Length > 0;
                if (fromModule)
                {
                    string moduleName = UGITUtility.ObjectToString(drModules[0][DatabaseObjects.Columns.ModuleName]);
                }
                resultedTableLevel2 = AllocationTypeManager.LoadLevel2(context, ddlLevel1.SelectedItem.Text, fromModule);
                if (resultedTableLevel2 != null)
                {
                    bool ShowERPJobID = _configVariableMGR.GetValueAsBool(ConfigConstants.ShowERPJobID);
                    string ERPJobIDName = _configVariableMGR.GetValue(ConfigConstants.ERPJobIDName);
                    if (fromModule)
                    {
                        cbLevel2.Columns.Clear();
                        cbLevel2.Columns.Add("LevelTitle");
                        cbLevel2.Columns[0].Caption = "Item ID";
                        cbLevel2.Columns[0].Width = new Unit("95px");

                        if (ShowERPJobID)
                        {
                            cbLevel2.Columns.Add(DatabaseObjects.Columns.ERPJobID);
                            cbLevel2.Columns[1].Caption = ERPJobIDName;
                            cbLevel2.Columns.Add(DatabaseObjects.Columns.Title);
                            cbLevel2.Columns[2].Width = new Unit("306px");
                            cbLevel2.Columns.Add(DatabaseObjects.Columns.TicketStatus);
                            cbLevel2.Columns[3].Caption = "Status";
                            cbLevel2.DropDownWidth = Unit.Empty;
                            cbLevel2.TextFormatString = "{0} : {1} : {2}";
                        }
                        else
                        {
                            cbLevel2.Columns.Add(DatabaseObjects.Columns.Title);
                            cbLevel2.Columns[1].Width = new Unit("306px");
                            cbLevel2.Columns.Add(DatabaseObjects.Columns.TicketStatus);
                            cbLevel2.Columns[2].Caption = "Status";
                            cbLevel2.DropDownWidth = Unit.Empty;
                            cbLevel2.TextFormatString = "{0} : {1}";
                        }
                    }
                    else
                    {
                        cbLevel2.Columns.Clear();
                        cbLevel2.DropDownWidth = cbLevel2.Width;
                        cbLevel2.ValueField = "LevelTitle";
                        cbLevel2.ValueType = typeof(string);
                        cbLevel2.TextField = "LevelTitle";
                    }

                    cbLevel2.DataSource = resultedTableLevel2;
                    cbLevel2.DataBind();

                    if (cbLevel2.Items.Count > 1 && !fromModule)
                    {
                        // Else add the dummy item on top
                        cbLevel2.Items.Insert(0, new ListEditItem("--Select--", ""));
                    }
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}