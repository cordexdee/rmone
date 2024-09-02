using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class DateVariableCtrl : UserControl
    {

        string defaultDateFunction;
        int defaultNumberOfDays;

        public int SetNumberOfDays
        {
            get
            {
                return defaultNumberOfDays;
            }
            set
            {
                defaultNumberOfDays = value;
                SetDefaultDaysAdded();
            }
        }
        public string SetDateFunction
        {
            get
            {
                return defaultDateFunction;
            }
            set
            {
                defaultDateFunction = value;
                SetDefaultFunction();
            }
        }

        public int GetNumberOfDaysAdded { get { return GetNumberOfDays(); } }
        public string GetDateFunction { get { return GetFunction(); } }

        protected override void OnInit(EventArgs e)
        {
            BindDDLCount();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        private void SetDefaultFunction()
        {
            if (defaultDateFunction == null || defaultDateFunction.Trim() == string.Empty)
                return;

            if (ddlCount != null)
            {
                string func = defaultDateFunction.ToLower();
                string daysAdded = func.Replace("f:adddays([$today$],", string.Empty);
                daysAdded = daysAdded.Replace(")", string.Empty).Trim();

                int days = UGITUtility.StringToInt(daysAdded); ;
                defaultNumberOfDays = days;

                int checkIntervalType = defaultNumberOfDays % 7;
                if (checkIntervalType != 0)
                {
                    ddlCount.SelectedValue = Convert.ToString(defaultNumberOfDays).Replace('-', ' ').Trim();
                    ddlTimeInterval.SelectedValue = "Days";
                }
                else
                {
                    ddlCount.SelectedValue = Convert.ToString(defaultNumberOfDays / 7).Replace('-', ' ').Trim();
                    ddlTimeInterval.SelectedValue = "Weeks";
                }
                if (Convert.ToString(defaultNumberOfDays).Contains("-"))
                {
                    ddlIntervalType.SelectedValue = "-";
                }
                else
                {
                    ddlIntervalType.SelectedValue = "+";
                }
            }
        }
        private void SetDefaultDaysAdded()
        {
            if (ddlCount != null)
            {
                int checkIntervalType = defaultNumberOfDays % 7;
                if (checkIntervalType != 0)
                {
                    ddlCount.SelectedValue = Convert.ToString(defaultNumberOfDays).Replace('-', ' ').Trim();
                    ddlTimeInterval.SelectedValue = "Days";
                }
                else
                {
                    ddlCount.SelectedValue = Convert.ToString(defaultNumberOfDays / 7).Replace('-', ' ').Trim();
                    ddlTimeInterval.SelectedValue = "Weeks";
                }
                if (Convert.ToString(defaultNumberOfDays).Contains("-"))
                {
                    ddlIntervalType.SelectedValue = "-";
                }
                else
                {
                    ddlIntervalType.SelectedValue = "+";
                }
            }
        }
        private string GetFunction()
        {
            int totalDays = 0;
            if (ddlTimeInterval.SelectedValue == "Days")
            {
                totalDays = UGITUtility.StringToInt(ddlIntervalType.SelectedValue + ddlCount.SelectedValue);
            }
            else
            {
                totalDays = UGITUtility.StringToInt(ddlIntervalType.SelectedValue + ddlCount.SelectedValue) * 7;
            }
            return string.Format("f:adddays([$Today$],{0})", totalDays);
        }
        private int GetNumberOfDays()
        {
            int totalDays = 0;
            if (ddlTimeInterval.SelectedValue == "Days")
            {
                totalDays = UGITUtility.StringToInt(ddlIntervalType.SelectedValue + ddlCount.SelectedValue);
            }
            else
            {
                totalDays = UGITUtility.StringToInt(ddlIntervalType.SelectedValue + ddlCount.SelectedValue) * 7;
            }
            return totalDays;
        }

        public void BindDDLCount()
        {
            ddlCount.Items.Clear();
            for (int i = 0; i < 32; i++)
            {
                ddlCount.Items.Add(i.ToString());
            }
        }
    }
}
