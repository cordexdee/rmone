using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using uGovernIT.Manager.Entities;
using System.Data;
namespace uGovernIT.Manager.Reports
{
    public partial class ApplicationReportR : DevExpress.XtraReports.UI.XtraReport
    {
        DataTable applicationReportEntityList;
        public ApplicationReportR(DataTable applicationReport, bool isDetail, bool isUser)
        {
            InitializeComponent();
            if (!isUser)
            {
                xrLblHeader.Text = "Applications Report";
                applicationReportEntityList = applicationReport;
                if (applicationReportEntityList != null && applicationReportEntityList.Rows.Count > 0)
                {
                    xrTcModule.DataBindings.Add("Text", null, "Module");
                    xrTcUserName.DataBindings.Add("Text", null, "UserName");
                    xrTcCategory.DataBindings.Add("Text", null, "CategoryName");
                    xrTcApplications.DataBindings.Add("Text", null, "ApplicationName");
                    grpHeaderCategory.GroupFields.Add(new GroupField("CategoryName"));
                    grpHeaderApplications.GroupFields.Add(new GroupField("ApplicationName"));
                    this.DataSource = applicationReport;
                }
            }
            else
            {
                xrLblHeader.Text = "User Report";
                applicationReportEntityList = applicationReport;
                if (applicationReportEntityList != null && applicationReportEntityList.Rows.Count > 0)
                {
                    xrTcModule.DataBindings.Add("Text", null, "Module");
                    xrTcUserName.DataBindings.Add("Text", null, "ApplicationName");
                    xrTcCategory.DataBindings.Add("Text", null, "UserName");
                    xrTableCell6.Text = "Application";
                    xrTcApplications.Visible = false;
                    grpHeaderCategory.GroupFields.Add(new GroupField("UserName"));
                    grpHeaderApplications.Visible = false;
                    this.DataSource = applicationReport;
                }
            }
        }

    }
}
