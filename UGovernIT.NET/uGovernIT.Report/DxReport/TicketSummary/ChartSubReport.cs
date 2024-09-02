using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using uGovernIT.Utility;

namespace uGovernIT.Report.DxReport
{
    public partial class ChartSubReport : DevExpress.XtraReports.UI.XtraReport
    {

        public ChartSubReport(List<ChartEntity> chartUrl)
        {
            InitializeComponent();
            this.PageHeight = chartUrl[0].Height;
            this.PageWidth = chartUrl[0].Width;
            CreateCharts(chartUrl);
        }
        private void CreateCharts(List<ChartEntity> chartUrl)
        {
            //if (chartUrl != null && chartUrl.Count > 0)
            //{
            xrLblChart1.DataBindings.Add("Text", null, "Title");
            xrPbChart1.DataBindings.Add("ImageUrl", null, "ChartUrl");
            //rPbChart1.LocationF =new PointF( 10,chartUrl[0].Top+15);
            xrPbChart1.SizeF =new System.Drawing.SizeF(chartUrl[0].Width,chartUrl[0].Height-25) ;
            xrLblChart1.SizeF = new System.Drawing.SizeF(chartUrl[0].Width, 25);
            Report.DataSource = chartUrl;
            //}
        }

        
    }
}
