using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;

namespace uGovernIT.Report.DxReport
{
    public class NPRProjectReportEntity
    {
        public List<Reportable> Fields { get; set; }
        public string ProjectName { get; set; }
        public string CompanyLogo { get; set; }
        public int LogoHeight { get; set; }
        public int LogoWidth { get; set; }
        public DataTable Projects { get; set; }
        public Image CompanyLogoBitmap { get; set; }
    }
}