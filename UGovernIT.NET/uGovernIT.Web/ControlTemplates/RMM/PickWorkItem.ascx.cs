using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.RMM
{
    public partial class PickWorkItem : System.Web.UI.UserControl
    {
        class DropDownVal
        {
            public string Level1 { get; set; }
            public string Level1Text { get; set; }
            public string Level2 { get; set; }
            public string Level3 { get; set; }
            public bool IsModule { get; set; }
            public string ModuleName { get; set; }
            public bool Level3MultiSelect { get; set; }
            public string Level3Label { get; set; }
        }

        ResourceAllocationManager allocManager = null;
        ResourceWorkItemsManager workManager = null;
        ApplicationContext context = null;
        DropDownVal dropdownVal = new DropDownVal();
        protected DateTime startDate;
        protected DateTime endDate;
      

        protected override void OnInit(EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            allocManager = new ResourceAllocationManager(context);
            workManager = new ResourceWorkItemsManager(context);
            startDate = new DateTime(DateTime.Now.Year, 1, 1);
            endDate = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12));

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           

        }

        protected void ddlLevel1_Load(object sender, EventArgs e)
        {
            DropDownList level1 = (DropDownList)sender;
            if (level1.Items.Count <= 0)
            {
                DataTable resultedTable = AllocationTypeManager.LoadLevel1(context);
                if (resultedTable != null)
                {
                    level1.DataSource = resultedTable;
                    level1.DataValueField = "LevelName";
                    level1.DataTextField = "LevelTitle";
                    level1.DataBind();
                    level1.Items.Insert(0, new ListItem("--Select--", ""));
                }
            }

            
            if (!IsPostBack)
                ddlLevel1.SelectedIndex = ddlLevel1.Items.IndexOf(ddlLevel1.Items.FindByValue("CPR"));

            if (!string.IsNullOrWhiteSpace(ddlLevel1.SelectedValue))
            {
                dropdownVal.Level1 = ddlLevel1.SelectedValue;
                dropdownVal.Level1Text = ddlLevel1.SelectedItem.Text;
                dropdownVal.Level3Label = "Sub Item";
                ModuleViewManager moduleManager = new ModuleViewManager(context);
                UGITModule module = moduleManager.GetByName(ddlLevel1.SelectedValue);
                if (module != null)
                {
                    dropdownVal.IsModule = true;
                    dropdownVal.ModuleName = module.ModuleName;
                    if (dropdownVal.ModuleName == "CPR" || dropdownVal.ModuleName == "OPM" || dropdownVal.ModuleName == "CNS")
                    {
                        dropdownVal.Level3MultiSelect = true;
                        dropdownVal.Level3Label = "Role";
                    }

                }
            }

            if (!string.IsNullOrWhiteSpace(Convert.ToString(cbLevel2.Value)))
            {
                dropdownVal.Level2 = Convert.ToString(cbLevel2.Value);
            }

            if (!string.IsNullOrWhiteSpace(Convert.ToString(cbLevel3.Value)))
            {
                dropdownVal.Level3 = Convert.ToString(cbLevel3.Value);
            }
        }

        protected void cbLevel2_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlLevel1.SelectedValue))
                return;

            DataTable resultedTable = AllocationTypeManager.LoadLevel2(context, dropdownVal.Level1Text, dropdownVal.IsModule);
            if (resultedTable != null)
            {
                lbSubitem.InnerText = dropdownVal.Level3Label;
                if (dropdownVal.IsModule)
                {
                    cbLevel2.Columns.Clear();
                    cbLevel2.Columns.Add("LevelTitle");
                    cbLevel2.Columns[0].Caption = "Ticket ID";
                    cbLevel2.Columns[0].Width = new Unit("120px");
                    cbLevel2.Columns.Add(DatabaseObjects.Columns.Title);
                    cbLevel2.Columns[1].Width = new Unit("170px");
                    cbLevel2.Columns.Add(DatabaseObjects.Columns.TicketStatus);
                    cbLevel2.Columns[2].Caption = "Status";
                    cbLevel2.DropDownWidth = new Unit("5px");

                }
                else
                {
                    cbLevel2.Columns.Clear();
                    cbLevel2.DropDownWidth = cbLevel2.Width;
                    cbLevel2.ValueField = "LevelTitle";
                    cbLevel2.ValueType = typeof(string);
                    cbLevel2.TextField = "LevelTitle";
                }

                cbLevel2.DataSource = resultedTable;
                cbLevel2.DataBind();

                if (cbLevel2.Items.Count > 1 && !dropdownVal.IsModule)
                {
                    // Else add the dummy item on top
                    cbLevel2.Items.Insert(0, new ListEditItem("--Select--", ""));
                }
            }
        }

        protected void cbLevel3_Load(object sender, EventArgs e)
        {
            cbLevel3.Visible = true;
            cbLevel3.Items.Clear();
            if (string.IsNullOrWhiteSpace(dropdownVal.Level1) || string.IsNullOrWhiteSpace(dropdownVal.Level2))
                return;

            TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            UGITModule module = moduleManager.GetByName(ddlLevel1.SelectedValue);
            if (module!=null && (module.ModuleName=="CPR" || module.ModuleName=="OPM")) {
                DataRow ticketRow = ticketManager.GetByTicketID(module, dropdownVal.Level2);
                if (ticketRow != null)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(ticketRow["EstimatedConstructionStart"])) && !string.IsNullOrEmpty(Convert.ToString(ticketRow["EstimatedConstructionEnd"])))
                    {
                        startDate = Convert.ToDateTime(ticketRow["EstimatedConstructionStart"]);
                        endDate = Convert.ToDateTime(ticketRow["EstimatedConstructionEnd"]);
                    }
                }
            }

            DataTable resultedTable = AllocationTypeManager.LoadLevel3(context, dropdownVal.Level1, dropdownVal.Level2, string.Empty, dropdownVal.IsModule);
            if (resultedTable != null)
            {
                // Hide level 3 if module is selected
                // For non-PMM, always load 3rd leve
                cbLevel3.DataSource = resultedTable;
                cbLevel3.ValueField = "LevelId";
                cbLevel3.ValueType = typeof(string);
                cbLevel3.TextField = "LevelName";
                cbLevel3.DataBind();

                // Add selection placeholder
                cbLevel3.Items.Insert(0, new DevExpress.Web.ListEditItem("--Select--", ""));

                // Hide level 3 if we have no items (except --Select-- place-holder) 
                // OR if we have single level 3 item with same name as level 2 (indicating place-holder)
                if (cbLevel3.Items.Count == 1 || (cbLevel3.Items.Count == 2 && dropdownVal.Level2 == Convert.ToString(cbLevel3.Items[1].Value)))
                    cbLevel3.Visible = false;
            }
            else
            {
                // No data
                cbLevel3.Visible = false;
            }
        }
    }
}