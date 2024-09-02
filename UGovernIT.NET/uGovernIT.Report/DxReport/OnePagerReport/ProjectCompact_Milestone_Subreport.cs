using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using uGovernIT.Manager.Report.Entities;
using System.Linq;
using System.Data;
using uGovernIT.Utility;

namespace uGovernIT.Report.DxReport
{
    public partial class ProjectCompact_Milestone_Subreport : DevExpress.XtraReports.UI.XtraReport
    {
        public ProjectCompact_Milestone_Subreport(ProjectCompactReportEntity entity)
        {
            InitializeComponent();

            if (entity.MileStone != null && entity.MileStone.Rows.Count > 0)
            {
                xrMilestone.DataBindings.Add("Text", null,DatabaseObjects.Columns.Title);
                xrMileTargetDate.DataBindings.Add("Text", null,DatabaseObjects.Columns.DueDate, uGITFormatConstants.DateFormat);
                xrMilePercen.DataBindings.Add("Text", null, DatabaseObjects.Columns.PercentComplete, "{0:0%}");
                if (entity.MileStone != null && entity.MileStone.Rows.Count > 0)
                {
                    entity.MileStone.Rows.Cast<DataRow>().AsEnumerable().ToList().ForEach(x=>x.SetField(DatabaseObjects.Columns.PercentComplete,GetCalculatedValue(x[DatabaseObjects.Columns.PercentComplete])));
                }
                this.DataSource = entity.MileStone;
            }
        }

        private double GetCalculatedValue(object oldPer) 
        {
            double per = 0F;
            double.TryParse(Convert.ToString(oldPer), out per);
            if (per > 0)
                per = per / 100;
            return per;
        }

    }
}
