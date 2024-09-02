
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using System.Linq;
using System.Text;
using System.Collections;
using DevExpress.Web;
using System.Collections.Generic;
using uGovernIT.Core;
using System.Drawing;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using DevExpress.Web.Rendering;
using System.Web.UI.HtmlControls;

namespace uGovernIT.Web
{
    public partial class TicketTemplateList : UserControl
    {

        public string Module { get; set; }
      // public DataTable TemplateTable { get; set; }
        protected string ticketURL = string.Empty;
        protected string sourceURL = string.Empty;
        protected string SaveAsTemplateURL = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=TicketTemplate&ItemID=");
        protected string moveToProductionUrl = "";
        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        ConfigurationVariableManager ConfigManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        TicketTemplateManager TemplateManager = new TicketTemplateManager(HttpContext.Current.GetManagerContext());
         List<TicketTemplate> templateList;
       
        protected override void OnPreRender(EventArgs e) {            
           
            grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;            
            grid.AutoGenerateColumns = false;
            grid.SettingsBehavior.AllowDragDrop = false;
            grid.SettingsText.EmptyHeaders = "  ";
            grid.SettingsPager.PageSizeItemSettings.Visible = true;
            grid.SettingsPager.PageSizeItemSettings.Position = DevExpress.Web.PagerPageSizePosition.Right;
            grid.SettingsBehavior.AllowSort = false;
            grid.SettingsBehavior.AllowSelectByRowClick = false;
            grid.SettingsBehavior.EnableRowHotTrack = true;
            grid.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            grid.Styles.AlternatingRow.CssClass = "ugitlight1lightest";
            grid.Styles.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");
            grid.Styles.RowHotTrack.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");

            ddlModule.devexListBox.ValidationSettings.RequiredField.IsRequired = true;
            ddlModule.devexListBox.ValidationSettings.RequiredField.ErrorText= "Please Select Module Name";
            ddlModule.devexListBox.ValidationSettings.ValidationGroup = "Save";
            ddlModule.devexListBox.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
            BindGridView();
        }

        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }

            UGITModule objModule = ModuleManager.LoadByName(ModuleNames.PRS);  // uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, "PRS");
            if (objModule != null)
                ticketURL = UGITUtility.GetAbsoluteURL(objModule.DetailPageUrl);
            //Migrate Url
            moveToProductionUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/uGovernITConfiguration.aspx?control=movestagetoproduction&list={0}&module={1}", DatabaseObjects.Tables.TicketTemplates, Module));
            EnableMigrate();
        }

        #region Data & Binding

        private void LoadData()
        {
            templateList = TemplateManager.Load();
            if (templateList != null)
            {
                var moduleNames = templateList.Select(x => x.ModuleNameLookup).Distinct();
                foreach (var row in moduleNames)
                {
                    UGITModule moduleDetail = ModuleManager.LoadByName(row);
                    if (moduleDetail != null)
                    {
                        var rows = templateList.Where(x => x.ModuleNameLookup == row).OrderBy(x => x.Title).ThenBy(x => x.ModuleDescription);

                        foreach (var r in rows)
                        {
                            r.ModuleDescription = moduleDetail.Title;
                        }
                    }
                }
            }

        }
        

        private void BindGridView()
        {
          grid.DataSource = templateList;
            grid.DataBind();
        }

        #endregion

        #region Events
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (templateList == null || templateList.Count == 0)
            {
                LoadData();
            }
            grid.DataSource = templateList;
        }

        protected void aEdit_Load(object sender, EventArgs e)
        {
            HtmlAnchor aHtml = (HtmlAnchor)sender;
            string lsDataKeyValue = (aHtml.NamingContainer as GridViewDataItemTemplateContainer).KeyValue.ToString();
            string editItem = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/DelegateControl.aspx?control=TicketTemplate&ItemID=" + lsDataKeyValue);
            string Url = string.Format("javascript:UgitOpenPopupDialog('{0}','','Ticket Template - {1}','950px','680px','0')", editItem, "Edit Item"); //,0,'{1}','false'
            aHtml.Attributes.Add("href", Url);            
        }

        protected void aDelete_Load(object sender, EventArgs e)
        {
            HtmlAnchor aHtml = (HtmlAnchor)sender;
            string lsDataKeyValue = (aHtml.NamingContainer as GridViewDataItemTemplateContainer).KeyValue.ToString();
            string title = string.Empty;
            if (templateList == null || templateList.Count == 0)
            {
                LoadData();
            }
            List<TicketTemplate> dataList = templateList.Where(x => x.ID.Equals(Convert.ToInt32(lsDataKeyValue))).ToList();
            if (dataList.Count > 0)
            {
                title = Convert.ToString(dataList.Select(x=>x.Title).FirstOrDefault());

            }
            aHtml.Attributes.Add("onclick", "DeleteTemplate(" + lsDataKeyValue + ",'" +   Server.UrlEncode(title) + "');");
        }

        #endregion

        #region Enable Migration
        private void EnableMigrate()
        {
            if (ConfigManager.GetValueAsBool(ConfigConstants.EnableMigrate))
            {
                btnMigrateQuickMacro.Visible = true;
            }
        }
        #endregion

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameters) && e.Parameters.Contains("|"))
            { 
              string templateId = e.Parameters.Split(new string[]{"|"},StringSplitOptions.RemoveEmptyEntries)[1];
              if (templateList == null || templateList.Count == 0)
              {
                  LoadData();
              }

                TicketTemplate spItem = TemplateManager.LoadByID(UGITUtility.StringToInt(templateId)); // SPListHelper.GetSPListItem(DatabaseObjects.Lists.TicketTemplates, uHelper.StringToInt(templateId));
              if (spItem != null)
              {
                  TemplateManager.Delete(spItem);    ///spItem.Delete();
                  LoadData();
                  BindGridView();
              }
            }
        }

        private bool checkForDuplicateTemplate()
        {
            if (templateList == null || templateList.Count == 0)
            {
                LoadData();
            }

            if (templateList != null && templateList.Count > 0)
            {
                var drs= templateList.Where(x => x.Title.Equals(txtTemplateName.Text.Trim()));
                if (drs != null && drs.Count() > 0)
                {
                    hndTemplateId.Value = "0";
                    return true;
                }
            }

         return false;
        }

        protected void PopupControl_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            if (!checkForDuplicateTemplate())
            {
                TicketTemplate spTemplateItem = new TicketTemplate();   // SPListHelper.GetSPList(DatabaseObjects.Lists.TicketTemplates).AddItem();
                spTemplateItem.Title = txtTemplateName.Text;
                spTemplateItem.ModuleNameLookup = uHelper.getModuleNameByModuleId(HttpContext.Current.GetManagerContext(), Convert.ToInt32( ddlModule.GetValues()));
                spTemplateItem.TemplateType = "Ticket";
                TemplateManager.Insert(spTemplateItem); // spTemplateItem.Update();
                if (spTemplateItem.ID > 0)
                {
                    hndTemplateId.Value = Convert.ToString(spTemplateItem.ID);
                }
            }
            else if (!string.IsNullOrEmpty(e.Parameter) && e.Parameter.Contains("overwrite"))
            {
                TicketTemplate spTemplateItem = TemplateManager.Load(x => x.Title == txtTemplateName.Text).FirstOrDefault();  // SPListHelper.GetListItem(DatabaseObjects.Lists.TicketTemplates, DatabaseObjects.Columns.Title, txtTemplateName.Text, "Text", SPContext.Current.Web);
                    //GetSPList(DatabaseObjects.Lists.TicketTemplates)AddItem();
                spTemplateItem.Title = txtTemplateName.Text;
                spTemplateItem.TemplateType = "Ticket";
                spTemplateItem.ModuleNameLookup = uHelper.getModuleNameByModuleId(HttpContext.Current.GetManagerContext(), Convert.ToInt32(ddlModule.GetValues())); // uHelper.getModuleIdByModuleName(ddlModule.GetValues());
                TemplateManager.Update(spTemplateItem);  //spTemplateItem.Update();
                if (spTemplateItem.ID > 0)
                {
                    hndTemplateId.Value = Convert.ToString(spTemplateItem.ID);
                }            
            }
        }

        protected void aTitle_Load(object sender, EventArgs e)
        {
            HtmlAnchor aHtml = (HtmlAnchor)sender;
            string lsDataKeyValue = (aHtml.NamingContainer as GridViewDataItemTemplateContainer).KeyValue.ToString();
            string editItem = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/DelegateControl.aspx?control=TicketTemplate&ItemID=" + lsDataKeyValue);
            string Url = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','Ticket Template - {1}','950px','680px','0')", editItem, "Edit Item"); 

            aHtml.Attributes.Add("href", Url);
            aHtml.InnerText = Server.HtmlDecode((aHtml.NamingContainer as GridViewDataItemTemplateContainer).Text.ToString());
        }
       
    }
}
