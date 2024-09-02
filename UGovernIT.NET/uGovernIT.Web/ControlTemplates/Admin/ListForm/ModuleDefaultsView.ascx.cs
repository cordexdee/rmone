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
using System.Linq;
using System.Web;
using uGovernIT.Manager.Core;
using System.Collections.Generic;
using uGovernIT.Util.Cache;

namespace uGovernIT.Web
{
    public partial class ModuleDefaultsView : UserControl
    {
       // SPList spModuleDefaults;
        string addNewItem;

        private string absoluteUrlEdit = "/layouts/ugovernit/DelegateControl.aspx?control={0}&ID={1}&ModuleName={2}";
        private string absoluteUrlView = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&module={2}";
        private string formTitle = "Module Default Values";
        private string viewParam = "moduledefaultvalues";
        private string editParam = "moduledefaultsedit";
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleDefaultValueManager ObjModuleDefaultValueManager = new ModuleDefaultValueManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            //addNewItem =  UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, "0"));
            //aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','450',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            //aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','450',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            BindModule();

            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["module"] != null)
                {
                    ddlModule.SelectedValue = Convert.ToString(Request["module"]);
                }
            }

            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, "0", ddlModule.SelectedValue));
            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','400',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','400',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            BindGridView(ddlModule.SelectedValue);
            base.OnLoad(e);
        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            string module = ddlModule.SelectedValue;
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, viewParam, formTitle, module));
            Response.Redirect(url);
        }

        void BindModule()
        {
           List<ModuleDefaultValue> moduleList = ObjModuleDefaultValueManager.Load();
            ddlModule.Items.Clear();
            if (moduleList != null && moduleList.Count > 0)
            {
                List<UGITModule> ugitModuleList = ObjModuleViewManager.Load(x=>x.EnableModule == true).OrderBy(x=>x.ModuleName).ToList();
                foreach(var moduleRow in ugitModuleList)
                {
                    string moduleName = Convert.ToString(moduleRow.ModuleName);
                    List<ModuleDefaultValue> dr = moduleList.Where(x=>x.ModuleNameLookup.Equals(moduleName,StringComparison.CurrentCultureIgnoreCase)).ToList();
                    if (dr.Count > 0)
                    {
                        ddlModule.Items.Add(new ListItem { Text = Convert.ToString(moduleRow.Title), Value = moduleName });
                    }
                }
                ddlModule.DataBind();
            }
        }

        void BindGridView(string Module)
        {
            ModuleDefaultValueManager moduleDefaultValueManager = new ModuleDefaultValueManager(HttpContext.Current.GetManagerContext());
            List<ModuleDefaultValue> defaultValueList = moduleDefaultValueManager.Load(x => x.ModuleNameLookup == Module).ToList();
            LifeCycleStageManager stagesManager = new LifeCycleStageManager(HttpContext.Current.GetManagerContext()) ;
            var stageList = stagesManager.Load(x => x.ModuleNameLookup.Equals(Module, StringComparison.CurrentCultureIgnoreCase)).OrderBy(x => x.StageStep).ToArray();
            int rowCounter = 0;
            if (defaultValueList.Count > 0)
            {
                var defaultValueTempList = defaultValueList;
                foreach (var row in defaultValueTempList)
                {
                    var currentStage = stageList.FirstOrDefault(x => Convert.ToString(x.ID).Equals(Convert.ToString(row.ModuleStepLookup)));
                    if (currentStage != null && defaultValueList.Count > rowCounter)
                    {
                        defaultValueTempList[rowCounter].ModuleStageName = $"{ currentStage.StageStep}" + ':' + $"{ currentStage.StageTitle }";
                    }
                    rowCounter++;
                }
                ASPxGridViewAdminDefault.DataSource = defaultValueTempList;
                ASPxGridViewAdminDefault.DataBind();
            }
        }

        protected void ASPxGridViewAdminDefault_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            var currentRow = (ModuleDefaultValue)ASPxGridViewAdminDefault.GetRow(e.VisibleIndex);
            string func = string.Empty;
            string adminModuleDefaultId = string.Empty;
            if (currentRow.ID > -1)
            {
                adminModuleDefaultId = currentRow.ID.ToString().Trim();
            }
            func = string.Format("openDialog('{0}','{1}','{2}','{3}','{4}', 0)", UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, adminModuleDefaultId, ddlModule.SelectedValue)), "", "Edit Module Default ", "600px", "400px");
            e.Row.Attributes.Add("onClick", func);
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
           List<ModuleDefaultValue> ModuleDefaultValueList=  ObjModuleDefaultValueManager.Load(x=>x.ModuleNameLookup== ddlModule.SelectedValue);
            UGITModule module = ObjModuleViewManager.GetByName(ddlModule.SelectedValue);
            if (module != null)
            {
                module.List_DefaultValues = ModuleDefaultValueList;
                CacheHelper<UGITModule>.AddOrUpdate(module.ModuleName, context.TenantID, module);
            }
        }
    }
}
