using System;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class ProjectComplexityAddEdit : UserControl
    {
        public string CriteriaId { get; set; }

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ProjectComplexityManager projectComplexityManager = null;
        FieldConfigurationManager fieldConfigurationManager = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            projectComplexityManager = new ProjectComplexityManager(context);
            fieldConfigurationManager = new FieldConfigurationManager(context);

            BindCriteria();

            if (!IsPostBack)
            {
                long Id = Convert.ToInt64(CriteriaId);
                if (Id > 0)
                {
                    ProjectComplexity projectComplexity = projectComplexityManager.LoadByID(Id);
                                        
                    cmbProjectComplexity.SelectedIndex = cmbProjectComplexity.Items.FindByText(projectComplexity.CRMProjectComplexity).Index;
                    seMinValue.Value = projectComplexity.MinValue;
                    seMaxValue.Value = projectComplexity.MaxValue;
                    chkDeleted.Checked = projectComplexity.Deleted;
                }
            }
        }

        private void BindCriteria()
        {
            var items = fieldConfigurationManager.Load(x => x.FieldName.Equals(DatabaseObjects.Columns.CRMProjectComplexity, StringComparison.InvariantCultureIgnoreCase) && x.TableName == DatabaseObjects.Tables.ProjectComplexity).ToList(); 

            //if (!string.IsNullOrEmpty(items))
            if (items != null & items.Count > 0)
            {
                string[] priority = UGITUtility.SplitString(items[0].Data, Constants.Separator);
                cmbProjectComplexity.DataSource = priority;
            }
            else
            {
                cmbProjectComplexity.DataSource = null;
            }

            cmbProjectComplexity.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            long Id = Convert.ToInt64(CriteriaId);
            ProjectComplexity projectComplexity = new ProjectComplexity();
            if (Id > 0)
            {
                projectComplexity = projectComplexityManager.LoadByID(Id);
                projectComplexity.CRMProjectComplexity = Convert.ToString(cmbProjectComplexity.SelectedItem.Value);
                projectComplexity.MinValue = Convert.ToDouble(seMinValue.Value);
                projectComplexity.MaxValue = Convert.ToDouble(seMaxValue.Value);
                projectComplexity.Deleted = Convert.ToBoolean(chkDeleted.Checked);
                bool status = projectComplexityManager.Update(projectComplexity);

                if (status)
                {
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Error in updating Project Complexity";
                }
            }
            else
            {
                projectComplexity.CRMProjectComplexity = Convert.ToString(cmbProjectComplexity.SelectedItem.Value);
                projectComplexity.MinValue = Convert.ToDouble(seMinValue.Value);
                projectComplexity.MaxValue = Convert.ToDouble(seMaxValue.Value);
                projectComplexity.Deleted = Convert.ToBoolean(chkDeleted.Checked);
                long status = projectComplexityManager.Insert(projectComplexity);

                if (status != 0)
                {
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Error in creating Project Complexity.";
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}