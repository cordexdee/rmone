
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;

namespace uGovernIT.Web
{
    public partial class ApplicationModulesEdit : UserControl
    {
        public int id { get; set; }
        int oldOrder = 1;
        public string itemOrder { get; set; }
        public int appid { get; set; }
        int maxItemOrder = 1;
        ApplicationModule spitem;
        ConfigurationVariableManager configManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        bool enableAppModuleRoles;
        List<ApplicationModule> dtAppModules = new List<ApplicationModule>();
        ApplicationModuleManager applicationModuleManager = new ApplicationModuleManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            enableAppModuleRoles = configManager.GetValueAsBool(ConfigConstants.EnableAppModuleRoles);

            dtAppModules = applicationModuleManager.Load(x => x.APPTitleLookup == appid).ToList();
            // Set max order to be module count, or max value whichever is greater + 1
            if (dtAppModules != null && dtAppModules.Count > 0)
            {
                maxItemOrder = dtAppModules.Count;
                foreach (ApplicationModule dr in dtAppModules)
                {
                    int itemOrder = Convert.ToInt32(dr.ItemOrder);
                    maxItemOrder = Math.Max(maxItemOrder, itemOrder);
                }
            }
            if (id > 0) {
                spitem = dtAppModules.First(x => x.ID == id);

                oldOrder = Convert.ToInt32(spitem.ItemOrder);
                if (oldOrder == 0)
                    if (string.IsNullOrEmpty(Request["itemOrder"]))
                    {
                       oldOrder=1;
                    }
                    else
                    {
                        oldOrder = Convert.ToInt32(Request["itemOrder"]);
                    }
                    
                else if (Request["itemOrder"] !="") {
                    oldOrder = Convert.ToInt32(Request["itemOrder"]);
                }
            }
            else
            {
                spitem = new ApplicationModule();
                if (maxItemOrder > 0)
                    maxItemOrder = maxItemOrder + 1;
            }
            int i = 1;
            while (i < maxItemOrder + 1)
            {
                string val = i.ToString();
                ddlItemOrder.Items.Add(new ListItem(val, val));
                i++;
            }
            if (enableAppModuleRoles == false)
            {
                trOwner.Visible = false;
                trSupportedBy.Visible = false;
                trAppAccessAdmin.Visible = false;
                trApprovers.Visible = false;
                trApprovalNeeded.Visible = false;
            }

            if (!IsPostBack)
                Fill();

            base.OnInit(e);
        }

        void Fill()
        {
            if (spitem != null)
            {
                txtTitle.Text = spitem.Title;
                txtDescription.Text = spitem.Description;
                string itemOrder = Convert.ToString(spitem.ItemOrder);
                if (Request["itemOrder"] != "")
                {
                    itemOrder = Request["itemOrder"];
                }
                ddlItemOrder.SelectedIndex = ddlItemOrder.Items.IndexOf(ddlItemOrder.Items.FindByValue(itemOrder));
                if (string.IsNullOrEmpty(itemOrder))
                {
                    if (spitem.ID == 0)
                    {
                        ddlItemOrder.SelectedIndex = ddlItemOrder.Items.IndexOf(ddlItemOrder.Items.FindByValue(maxItemOrder.ToString()));
                        oldOrder = maxItemOrder;
                    }
                    else if (Request["itemOrder"] != null)
                        ddlItemOrder.SelectedIndex = ddlItemOrder.Items.IndexOf(ddlItemOrder.Items.FindByValue(Request["itemOrder"]));
                }
                if (enableAppModuleRoles)
                {
                    ppeOwner.SetValues(spitem.Owner);
                    ppesupportedBy.SetValues(spitem.SupportedBy);
                    ppeAppAccessAdmin.SetValues(spitem.AccessAdmin);
                    ppeAppApprovers.SetValues(spitem.Approver);
                    ddlApprovalNeeded.SetValues(spitem.ApprovalTypeChoice);
                    //SPFieldUserValue arrSupportedBy = new SPFieldUserValue(SPContext.Current.Web, Convert.ToString(spitem[DatabaseObjects.Columns.SupportedBy]));
                    //ppesupportedBy.UpdateEntities(uHelper.getUsersListFromCollection(arrSupportedBy));
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            spitem.Title = txtTitle.Text.Trim();
            spitem.APPTitleLookup = appid;
            spitem.Description = txtDescription.Text.Trim();
            spitem.ItemOrder = Convert.ToInt32(ddlItemOrder.SelectedValue);
            if (enableAppModuleRoles)
            {  
                spitem.Owner = ppeOwner.GetValues();
                spitem.SupportedBy = ppesupportedBy.GetValues();
                spitem.AccessAdmin = ppeAppAccessAdmin.GetValues();
                spitem.Approver = ppeAppApprovers.GetValues();
                spitem.ApprovalTypeChoice = ddlApprovalNeeded.GetValues();
            }

            if (spitem.ID > 0)
                applicationModuleManager.Update(spitem);
            else
                applicationModuleManager.Insert(spitem);

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context);
        }

        protected void csvTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if(dtAppModules!=null && dtAppModules.Count>0)
            {
                if(dtAppModules.AsEnumerable().Any(x=> x.ID != id && x.Title==txtTitle.Text.Trim()))
                {
                    args.IsValid = false;
                }
            }
        }
    }
}
