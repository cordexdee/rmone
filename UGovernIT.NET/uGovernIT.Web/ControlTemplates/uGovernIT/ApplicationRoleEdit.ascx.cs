using DevExpress.Web;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using System.Linq;
using uGovernIT.Utility;
using System.Web;
using System.Collections.Generic;

namespace uGovernIT.Web
{
    public partial class ApplicationRoleEdit : UserControl
    {
        public int id { get; set; }
        public int appid { get; set; }
        int maxItemOrder = 1;
        int oldOrder = 1;
        //int updatedOrder = 0;
        ApplicationRole spitem;
        List<ApplicationRole> dtAppRoles = new List<ApplicationRole>();
        ApplicationRoleManager appRoleManager = new ApplicationRoleManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            dtAppRoles = appRoleManager.Load().Where(x => x.APPTitleLookup == appid && x.ID == id).ToList();
            LookUpValueBox checkComboBox = new LookUpValueBox();
            checkComboBox.ID = "checkComboBox";
            checkComboBox.FieldName = DatabaseObjects.Columns.ApplicationModulesLookup; // "ApplicationModules";
            checkComboBox.IsMulti = true;
            //checkComboBox.devexListBox.KeyFieldName = DatabaseObjects.Columns.ID;
               
            checkComboBox.Adddefaultvalue = true;
            checkComboBox.devexListBox.AutoPostBack = false;
            checkComboBox.devexListBox.ValidationSettings.RequiredField.IsRequired = true;
            checkComboBox.devexListBox.ValidationSettings.RequiredField.ErrorText = "Must Select Module";
            checkComboBox.devexListBox.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
            checkComboBox.devexListBox.ValidationSettings.ValidationGroup = "Save";
            if (appid > 0)
            {
                checkComboBox.FilterExpression = $"APPTitleLookup = {appid}";
                //checkComboBox.SetValues(UGITUtility.ObjectToString(id));
                if (dtAppRoles.Count>0)
                {
                    if(!string.IsNullOrEmpty(dtAppRoles[0].ApplicationRoleModuleLookup))
                        checkComboBox.SetValues(dtAppRoles[0].ApplicationRoleModuleLookup);
                }
                else
                {
                    checkComboBox.SetValues("0");
                }
                
            }


            //asPxListBox.Items.Insert(0, new ListEditItem("All", "0"));
            divAppModules.Controls.Add(checkComboBox);
            
            if (dtAppRoles != null && dtAppRoles.Count > 0) {
               // dtAppRoles = dtAppRoles.Where(x => x.APPTitleLookup == appid).ToList();

                //maxItemOrder = dtAppRoles.Count;
                if (dtAppRoles.Count > 0)
                {
                    maxItemOrder = dtAppRoles.Count;
                }
                foreach (ApplicationRole dr in dtAppRoles)
                {
                    int itemOrder = Convert.ToInt32(dr.ItemOrder);
                    maxItemOrder = Math.Max(maxItemOrder, itemOrder);
                }
            }
            if (id > 0)
            {
                spitem = dtAppRoles.FirstOrDefault(x => x.ID == id);
                oldOrder = Convert.ToInt32(spitem.ItemOrder);
                if (oldOrder == 0)
                    oldOrder = Convert.ToInt32(Request["itemOrder"]);
                else if (Request["itemOrder"] != "")
                {
                    oldOrder = Convert.ToInt32(Request["itemOrder"]);
                }
            }
            else
            {
                spitem = new ApplicationRole();
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
            //BindModuleGrid();
            Fill();
            base.OnInit(e);
        }

        void Fill()
        {
            //ASPxListBox asPxListBox = checkComboBox.FindControl("listBox") as ASPxListBox;
            txtTitle.Text = spitem.Title;
            txtDescription.Text = spitem.Description;
            string itemOrder = Convert.ToString(spitem.ItemOrder);
            if (Request["itemOrder"] != "")
            {
                itemOrder = Request["itemOrder"];
            }
            ddlItemOrder.SelectedIndex = ddlItemOrder.Items.IndexOf(ddlItemOrder.Items.FindByValue(itemOrder));
            //checkComboBox.SetValues(UGITUtility.ObjectToString(appid));
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
            

        }
        private void BindModuleGrid()
        {
            //ASPxListBox asPxListBox = checkComboBox.FindControl("listBox") as ASPxListBox;
            //SPQuery spQuery = new SPQuery();
            //spQuery.Query = string.Format("<OrderBy><FieldRef Name='{2}' Ascending='False' /></OrderBy></<Where><Eq><FieldRef Name='{0}' LookupId='True'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.APPTitleLookup, appid,DatabaseObjects.Columns.Title);
            //spQuery.ViewFields = string.Format("<FieldRef Name='{0}'  Nullable='TRUE'/><FieldRef Name='{1}' />", DatabaseObjects.Columns.Title, DatabaseObjects.Columns.Id);
            //spQuery.ViewFieldsOnly = true;
            //SPListItemCollection spListItemColl = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ApplicationModules, spQuery);
            //if (spListItemColl != null && spListItemColl.Count > 0)
            //{
            //    DataTable dt = spListItemColl.GetDataTable();
            //    if (dt != null)
            //    {
            //        asPxListBox.TextField = DatabaseObjects.Columns.Title;
            //        asPxListBox.ValueField = DatabaseObjects.Columns.Id;
            //        asPxListBox.DataSource = dt;
            //        asPxListBox.DataBind();
            //    }
            //}

            //asPxListBox.Items.Insert(0, new ListEditItem("All", "0"));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
          
            spitem.Title = txtTitle.Text.Trim();
            spitem.APPTitleLookup = appid;
            spitem.Description = txtDescription.Text.Trim();
            spitem.ItemOrder = Convert.ToInt32(ddlItemOrder.SelectedValue);
            LookUpValueBox valueBox = (LookUpValueBox)divAppModules.FindControl("checkComboBox");
            if (!string.IsNullOrEmpty(valueBox.GetValues()))
                spitem.ApplicationRoleModuleLookup = valueBox.GetValues();

            if (spitem.ID > 0)
                appRoleManager.Update(spitem);
            else
                appRoleManager.Insert(spitem);
            
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context);
        }

        protected void csvTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {
            List<ApplicationRole> dtAppRoles_ = null;
            
            dtAppRoles_ = appRoleManager.Load().Where(x => x.Title == txtTitle.Text.Trim() && x.TenantID== HttpContext.Current.GetManagerContext().TenantID && x.APPTitleLookup== appid).ToList();
            if (dtAppRoles_ != null && dtAppRoles_.Count > 0)
            {
                if (dtAppRoles_.AsEnumerable().Any(x => x.ID != id && x.Title == txtTitle.Text.Trim()))
                    args.IsValid = false;
            }
        }

    }
}
