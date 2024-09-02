using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class LeadCriteriaAddEdit : UserControl
    {
        public string CriteriaId { get; set; }

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        LeadCriteriaManager leadCriteriaManager = null;
        ConfigurationVariableManager configurationVariableMgr;
        protected void Page_Load(object sender, EventArgs e)
        {
            leadCriteriaManager = new LeadCriteriaManager(context);
            configurationVariableMgr = new ConfigurationVariableManager(context);
            BindCriteria();

            if (!IsPostBack)
            {
                long Id = Convert.ToInt64(CriteriaId);
                if (Id > 0)
                {
                    LeadCriteria leadCriteria = leadCriteriaManager.LoadByID(Id);

                    //cmbCriteria.Items.FindByText(leadCriteria.Priority);
                    cmbCriteria.SelectedIndex = cmbCriteria.Items.FindByText(leadCriteria.Priority).Index;
                    seMinValue.Value = leadCriteria.MinValue;
                    seMaxValue.Value = leadCriteria.MaxValue;
                    chkDeleted.Checked = leadCriteria.Deleted;
                }
            }
        }

        private void BindCriteria()
        {
            var items = configurationVariableMgr.GetValue(DatabaseObjects.Columns.SuccessChance);

            if (!string.IsNullOrEmpty(items))
            {
                string[] priority = UGITUtility.SplitString(items, Constants.Separator);
                cmbCriteria.DataSource = priority;
            }
            else
            {
                cmbCriteria.DataSource = null;
            }

            cmbCriteria.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            long Id = Convert.ToInt64(CriteriaId);
            LeadCriteria leadCriteria = new LeadCriteria();
            if (Id > 0)
            {
                leadCriteria = leadCriteriaManager.LoadByID(Id);
                leadCriteria.Priority = Convert.ToString(cmbCriteria.SelectedItem.Value);
                leadCriteria.MinValue = Convert.ToDecimal(seMinValue.Value);
                leadCriteria.MaxValue = Convert.ToDecimal(seMaxValue.Value);
                leadCriteria.Deleted = Convert.ToBoolean(chkDeleted.Checked);
                bool status = leadCriteriaManager.Update(leadCriteria);

                if (status)
                {
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Error in updating Lead Criteria";
                }
            }
            else
            {
                leadCriteria.Priority = Convert.ToString(cmbCriteria.SelectedItem.Value);
                leadCriteria.MinValue = Convert.ToDecimal(seMinValue.Value);
                leadCriteria.MaxValue = Convert.ToDecimal(seMaxValue.Value);
                leadCriteria.Deleted = Convert.ToBoolean(chkDeleted.Checked);
                long status = leadCriteriaManager.Insert(leadCriteria);

                if (status != 0)
                {
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Error in creating Lead Criteria";
                }
            }            
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}
