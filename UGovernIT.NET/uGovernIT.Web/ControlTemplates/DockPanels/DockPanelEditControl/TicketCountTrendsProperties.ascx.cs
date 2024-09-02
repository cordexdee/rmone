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
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web.ControlTemplates.DockPanels.DockPanelEditControl
{
    public partial class TicketCountTrendsProperties : System.Web.UI.UserControl
    {
        public TicketCountTrendsDocPanelSetting TicketCountTrendsDocPanelSetting { get; set; }
        public ApplicationContext context = HttpContext.Current.GetManagerContext();
        ASPxListBox list;
        Dictionary<string, string> dicSelectedModule;

        protected override void OnInit(EventArgs e)
        {
            dicSelectedModule = new Dictionary<string, string>();
            chkTitle.Checked = TicketCountTrendsDocPanelSetting.ShowTitle;
            txtTitle.Text = TicketCountTrendsDocPanelSetting.Title;

            if (!string.IsNullOrEmpty(TicketCountTrendsDocPanelSetting.StringOfSelectedModule))
                ddeModules_Init(ddeModules, null);

            if (!IsPostBack)
            {
                ddeModules_Init(ddeModules, null);
            }
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TicketCountTrendsDocPanelSetting.ShowTitle = chkTitle.Checked;
            TicketCountTrendsDocPanelSetting.Title = txtTitle.Text;
            TicketCountTrendsDocPanelSetting.StringOfSelectedModule = string.Empty;
            TicketCountTrendsDocPanelSetting.DefaultModule = string.Empty;
            
            if (list != null && list.SelectedValues.Count > 0)
            {
                List<ListEditItem> moduleList = list.SelectedItems.Cast<ListEditItem>().Select(x => x).Distinct().ToList();
                List<string> lstSelectedModule = new List<string>();
                foreach (ListEditItem item in moduleList)
                {
                    string key = Convert.ToString(item.Value);
                    if (!dicSelectedModule.ContainsKey(key))
                    {
                        dicSelectedModule.Add(key, item.Text);
                        lstSelectedModule.Add(string.Format("{0}", item.Text));
                    }
                }
                TicketCountTrendsDocPanelSetting.StringOfSelectedModule = string.Join(";", lstSelectedModule);
            }

            ListEditItem defaultItem = cmbdefaultModule.SelectedItem;
            if (defaultItem != null)
                TicketCountTrendsDocPanelSetting.DefaultModule = defaultItem.Text;
        }

        public void CopyFromWebpart(TicketCountTrendsDocPanelSetting webpart)
        {
            if (TicketCountTrendsDocPanelSetting != null)
            {
                webpart.ShowTitle = TicketCountTrendsDocPanelSetting.ShowTitle;
                webpart.Title = TicketCountTrendsDocPanelSetting.Title;
                webpart.DefaultModule = TicketCountTrendsDocPanelSetting.DefaultModule;
                webpart.StringOfSelectedModule = TicketCountTrendsDocPanelSetting.StringOfSelectedModule;
            }
            else
            {
                webpart.ShowTitle = chkTitle.Checked;
                webpart.Title = txtTitle.Text;
                webpart.DefaultModule = string.Empty;
                webpart.StringOfSelectedModule = string.Empty;

                ListEditItem defaultItem = cmbdefaultModule.SelectedItem;
                if (defaultItem != null)
                    webpart.DefaultModule = defaultItem.Text;

                if (list != null && list.SelectedValues.Count > 0)
                {
                    List<ListEditItem> moduleList = list.SelectedItems.Cast<ListEditItem>().Select(x => x).Distinct().ToList();
                    List<string> lstSelectedModule = new List<string>();
                    foreach (ListEditItem item in moduleList)
                    {
                        lstSelectedModule.Add(string.Format("{0}", item.Text));
                    }

                    webpart.StringOfSelectedModule = string.Join(";", lstSelectedModule);
                }
                TicketCountTrendsDocPanelSetting = webpart;
            }
        }

        public void CopyFromControl(TicketCountTrendsDocPanelSetting webpart)
        {
            if (TicketCountTrendsDocPanelSetting != null)
            {
                webpart.ShowTitle = TicketCountTrendsDocPanelSetting.ShowTitle;
                webpart.Title = TicketCountTrendsDocPanelSetting.Title;
                webpart.DefaultModule = TicketCountTrendsDocPanelSetting.DefaultModule;
                webpart.StringOfSelectedModule = TicketCountTrendsDocPanelSetting.StringOfSelectedModule;
            }
            else
            {
                webpart.ShowTitle = chkTitle.Checked;
                webpart.Title = txtTitle.Text;
                webpart.DefaultModule = string.Empty;
                webpart.StringOfSelectedModule = string.Empty;

                ListEditItem defaultItem = cmbdefaultModule.SelectedItem;
                if (defaultItem != null)
                    webpart.DefaultModule = defaultItem.Text;

                if (list != null && list.SelectedValues.Count > 0)
                {
                    List<ListEditItem> moduleList = list.SelectedItems.Cast<ListEditItem>().Select(x => x).Distinct().ToList();
                    List<string> lstSelectedModule = new List<string>();
                    foreach (ListEditItem item in moduleList)
                    {
                        lstSelectedModule.Add(string.Format("{0}", item.Text));
                    }

                    webpart.StringOfSelectedModule = string.Join(";", lstSelectedModule);
                }
                TicketCountTrendsDocPanelSetting = webpart;
            }
        }

        private void BindModule(object ctr)
        {
            if (ctr != null && ctr is ASPxListBox)
            {
                list = ctr as ASPxListBox;
                if (list != null && list.Items.Count <= 0)
                {
                    ModuleViewManager moduleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                    DataTable dtModule = moduleManager.GetDataTable();

                    if (dtModule != null && dtModule.Rows.Count > 0)
                    {
                        dtModule.DefaultView.Sort = DatabaseObjects.Columns.ModuleName + " ASC";
                        dtModule = dtModule.DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.ModuleName, DatabaseObjects.Columns.ID, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.EnableModule, DatabaseObjects.Columns.KeepTicketCounts });
                        DataRow[] moduleRows = dtModule.Select(string.Format("{0}='True' and {1}='True' and {2}<>'{3}'", DatabaseObjects.Columns.EnableModule, DatabaseObjects.Columns.KeepTicketCounts, DatabaseObjects.Columns.ModuleName, ModuleNames.RMM));
                        
                        foreach (DataRow moduleRow in moduleRows)
                        {
                            string moduleName = Convert.ToString(moduleRow[DatabaseObjects.Columns.ModuleName]);

                            list.Items.Add(new ListEditItem { Text = Convert.ToString(moduleRow[DatabaseObjects.Columns.Title]), Value = moduleName });
                            cmbdefaultModule.Items.Add(new ListEditItem { Text = Convert.ToString(moduleRow[DatabaseObjects.Columns.Title]), Value = moduleName });
                        }
                    }
                }

                if (!string.IsNullOrEmpty(TicketCountTrendsDocPanelSetting.StringOfSelectedModule))
                {
                    string[] arrOfSplitModules = TicketCountTrendsDocPanelSetting.StringOfSelectedModule.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in arrOfSplitModules)
                    {
                        string[] arrKeyVal = str.Split('(');
                        string keyVal = arrKeyVal[0];
                        string val = arrKeyVal[1].Replace(')', ' ').Trim();
                        ListEditItem editItem = list.Items.FindByValue(val);
                        if (editItem != null)
                            editItem.Selected = true;
                    }
                }

                if (!string.IsNullOrEmpty(TicketCountTrendsDocPanelSetting.DefaultModule))
                {
                    string[] arrKeyVal = TicketCountTrendsDocPanelSetting.DefaultModule.Split('(');
                    string keyVal = arrKeyVal[0];
                    string val = keyVal;

                    if (arrKeyVal.Length > 1)
                        val = arrKeyVal[1].Replace(')', ' ').Trim();
                    cmbdefaultModule.SelectedIndex = cmbdefaultModule.Items.IndexOf(cmbdefaultModule.Items.FindByValue(val));
                }
            }
        }

        protected void ddeModules_Init(object sender, EventArgs e)
        {
            ASPxDropDownEdit dropdownedit = sender as ASPxDropDownEdit;
            list = dropdownedit.FindControl("listBox") as ASPxListBox;
            BindModule(list);
        }
    }
}