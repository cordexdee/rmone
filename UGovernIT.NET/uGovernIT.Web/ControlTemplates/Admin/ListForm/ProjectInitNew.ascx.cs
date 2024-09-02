
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Collections.Generic;

namespace uGovernIT.Web
{
    public partial class ProjectInitNew : UserControl
    {

        private ProjectInitiative _SPListItem;
        //List<BusinessStrategy> spBSList;
        public string BSTitle;
        ProjectInitiativeViewManager PIManager = new ProjectInitiativeViewManager(HttpContext.Current.GetManagerContext());
        ConfigurationVariableManager ConfigVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        public long Id { get; set; }

        protected override void OnInit(EventArgs e)
        {
            ddlBusinessStrategy.devexListBox.ValidationSettings.RequiredField.IsRequired = true;
            ddlBusinessStrategy.devexListBox.ValidationSettings.ValidationGroup = "Save";
            ddlBusinessStrategy.devexListBox.ValidationSettings.ErrorDisplayMode = DevExpress.Web.ErrorDisplayMode.ImageWithText;
            ddlBusinessStrategy.devexListBox.ValidationSettings.ErrorText = "Please Select Business Strategy";
            if (Id > 0)
            {
                _SPListItem = PIManager.LoadByID(Id);
                Fill();
            }
            else
            {
                _SPListItem = new ProjectInitiative();
            }
            //spBSList = SPListHelper.GetSPList(DatabaseObjects.Lists.BusinessStrategy, spWeb);
            BSTitle = string.IsNullOrEmpty(ConfigVariableManager.GetValue(ConfigConstants.InitiativeLevel1Name)) ? "Business Initiative" : ConfigVariableManager.GetValue(ConfigConstants.InitiativeLevel1Name);
            //BindBSDropDown();
            base.OnInit(e);
        }

        private void Fill()
        {
            txtTitle.Text = Convert.ToString(_SPListItem.Title);
            txtProjectNote.Text = Convert.ToString(_SPListItem.ProjectNote);
            chkDeleted.Checked = Convert.ToBoolean(_SPListItem.Deleted);
            
            ddlBusinessStrategy.SetValues(Convert.ToString(_SPListItem.BusinessStrategyLookup));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            _SPListItem.Title = txtTitle.Text.Trim();
            _SPListItem.ProjectNote = txtProjectNote.Text.Trim();
            _SPListItem.Deleted = chkDeleted.Checked;
            
            _SPListItem.BusinessStrategyLookup = Convert.ToInt64(ddlBusinessStrategy.GetValues());

            if (_SPListItem.ID > 0)
                PIManager.Update(_SPListItem);
            else
                PIManager.Insert(_SPListItem);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
