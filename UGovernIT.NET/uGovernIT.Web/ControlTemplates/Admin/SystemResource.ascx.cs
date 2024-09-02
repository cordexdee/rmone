using System;
using System.Data;
using System.Web.UI;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class SystemResource : UserControl
    {

        public string linkImpact = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=ticketimpact&mode=Impact");
        public string linkSeverity = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=ticketimpact&mode=Severity");
        public string linkPriority = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=ticketpriority&mode=Priority");
        public string linkPriortyMapping = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=priortymapping");
        public string linkSLARule = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=slarule");
        public string linkEscalationRule = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=escalationrule");

        public string linkHolidayCalendarEvent = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=holidaycalendarevent");
        public string linkEmailToTicket = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=emailtoticket");
        public string linkEmailNotification = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=emailnotification");
        public string linkSMTPMailSetting = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=smtpmailsetting");
        public string linkUGITLog = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=UGITLog");
        public string linkDeleteTickets = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=deletetickets");

        public string linkMenuNavigationView = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=menunavigationview");
        public string linkPageEditorView = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=pageeditorview");
        public string linkCustomControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?");
        //Resources
        public string linkAssetModelView = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=assetmodelview");
        public string linkBgetCatView = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=bgetcatview");
        public string linkContract = UGITUtility.GetAbsoluteURL("");//show message
        public string linkAssetVendorView = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=assetvendorview");

        //survey and schedule
        public string scheduleaction = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=scheduleactionslist");
        public string survey = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=services&ServiceType=SurveyFeedback");
        public string surveyfeedback = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=surveyfeedback&param=config");

        
        public string licenseUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configlicense");
        public string serviceurl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=services");
        public string cacheurl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configcache");

        protected void Page_Load(object sender, EventArgs e)
        {
            ImageSlider.DataSource = PriorityAndSLAs();
            ImageSlider.DataBind();

            ASPxImageSlider1.DataSource = SystemResources();
            ASPxImageSlider1.DataBind();

            ASPxImageSlider2.DataSource = UIConfiguration();
            ASPxImageSlider2.DataBind();

            ASPxImageSlider3.DataSource = Resources();
            ASPxImageSlider3.DataBind();

            

        }

        public DataTable PriorityAndSLAs()
        {
            var dt = new DataTable();

            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("Link", typeof(string));
            dt.Columns.Add("HrClass", typeof(string));


            DataRow dr = dt.NewRow();
            dr["Text"] = "Impact";
            dr["Image"] = "/Content/Images/NewAdmin/impact.png";
            dr["Link"] = "";
            dt.Rows.Add(dr);



            DataRow dr1 = dt.NewRow();
            dr1["Text"] = "Severity";
            dr1["Image"] = "/Content/Images/NewAdmin/severity.png";
            dr1["Link"] = "";
            dt.Rows.Add(dr1);



            DataRow dr3 = dt.NewRow();
            dr3["Text"] = "Priority";
            dr3["Image"] = "/Content/Images/NewAdmin/priority.png";
            dr3["Link"] = "";
            dt.Rows.Add(dr3);

            DataRow dr4 = dt.NewRow();
            dr4["Text"] = "Priority Mapping";
            dr4["Image"] = "/Content/Images/NewAdmin/priority-mapping.png";
            dr4["Link"] = "";
            dt.Rows.Add(dr4);

            DataRow dr5 = dt.NewRow();
            dr5["Text"] = "SLA Rules";
            dr5["Image"] = "/Content/Images/NewAdmin/SLA-rules.png";
            dr5["Link"] = "";
            dt.Rows.Add(dr5);

            DataRow dr6 = dt.NewRow();
            dr6["Text"] = "SLA Escalations";
            dr6["Image"] = "/Content/Images/NewAdmin/SLA-escalations.png";
            dr6["Link"] = "";
            dr6["HrClass"] = "lastHr";
            dt.Rows.Add(dr6);

            return dt;
        }

        public DataTable SystemResources()
        {
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Text", typeof(string));
            dt1.Columns.Add("Image", typeof(string));
            dt1.Columns.Add("Link", typeof(string));
            dt1.Columns.Add("HrClass", typeof(string));


            DataRow dr = dt1.NewRow();
            dr["Text"] = "SMTP Credentials";
            dr["Image"] = "/Content/Images/userNew.png";
            dr["Link"] = "";
            dt1.Rows.Add(dr);



            DataRow dr2 = dt1.NewRow();
            dr2["Text"] = "Logs";
            dr2["Image"] = "/Content/Images/NewAdmin/logs.png";
            dr2["Link"] = "";
            dt1.Rows.Add(dr2);



            DataRow dr3 = dt1.NewRow();
            dr3["Text"] = "Delete items";
            dr3["Image"] = "/Content/Images/NewAdmin/delete-items.png";
            dr3["Link"] = "";
            dt1.Rows.Add(dr3);

            DataRow dr4 = dt1.NewRow();
            dr4["Text"] = "Cache";
            dr4["Image"] = "/Content/Images/userNew.png";
            dr4["Link"] = "";
            dt1.Rows.Add(dr4);

            DataRow dr5 = dt1.NewRow();
            dr5["Text"] = "Licenses";
            dr5["Image"] = "/Content/Images/userNew.png";
            dr5["Link"] = "";
            dt1.Rows.Add(dr5);

            DataRow dr6 = dt1.NewRow();
            dr6["Text"] = "Scheduled Actions";
            dr6["Image"] = "/Content/Images/userNew.png";
            dr6["Link"] = "";
            dr6["HrClass"] = "lastHr";
            dt1.Rows.Add(dr6);


            return dt1;
        }

        public DataTable UIConfiguration()
        {
            var dt2 = new DataTable();
            dt2.Columns.Add("Text", typeof(string));
            dt2.Columns.Add("Image", typeof(string));
            dt2.Columns.Add("Link", typeof(string));
            dt2.Columns.Add("HrClass", typeof(string));


            DataRow dr = dt2.NewRow();
            dr["Text"] = "Menu Navigation";
            dr["Image"] = "/Content/Images/NewAdmin/menu-navigation.png";
            dr["Link"] = "";
            dt2.Rows.Add(dr);



            DataRow dr2 = dt2.NewRow();
            dr2["Text"] = "Page Editor";
            dr2["Image"] = "/Content/Images/NewAdmin/page-editor.png";
            dr2["Link"] = "";
            dt2.Rows.Add(dr2);



            DataRow dr3 = dt2.NewRow();
            dr3["Text"] = "Custom Controls";
            dr3["Image"] = "/Content/Images/NewAdmin/custom-control.png";
            dr3["Link"] = "";
            dr3["HrClass"] = "lastHr";
            dt2.Rows.Add(dr3);
            return dt2;
        }

        public DataTable Resources()
        {
            var dt = new DataTable();

            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("Link", typeof(string));
            dt.Columns.Add("HrClass", typeof(string));


            DataRow dr = dt.NewRow();
            dr["Text"] = "Assets";
            dr["Image"] = "/Content/Images/NewAdmin/accets.png";
            dr["Link"] = "";
            dt.Rows.Add(dr);



            DataRow dr2 = dt.NewRow();
            dr2["Text"] = "Budget categories";
            dr2["Image"] = "/Content/Images/NewAdmin/budget-catagories.png";
            dr2["Link"] = "";
            dt.Rows.Add(dr2);



            DataRow dr3 = dt.NewRow();
            dr3["Text"] = "Contracts";
            dr3["Image"] = "/Content/Images/NewAdmin/contracts.png";
            dr3["Link"] = "";
            dt.Rows.Add(dr3);

            DataRow dr4 = dt.NewRow();
            dr4["Text"] = "Vendors";
            dr4["Image"] = "/Content/Images/NewAdmin/vendor.png";
            dr4["Link"] = "";
            dr4["HrClass"] = "lastHr";
            dt.Rows.Add(dr4);

            return dt;
        }



        //public DataTable SchedulerandSurvey()
        //{
        //    var dt2 = new DataTable();
        //    dt2.Columns.Add("Text", typeof(string));
        //    dt2.Columns.Add("Image", typeof(string));
        //    dt2.Columns.Add("Link", typeof(string));



        //    DataRow dr = dt2.NewRow();
        //    dr["Text"] = "Scheduled Actions";
        //    dr["Image"] = "/Content/Images/userNew.png";
        //    dr["Link"] = "";
        //    dt2.Rows.Add(dr);



        //    DataRow dr2 = dt2.NewRow();
        //    dr2["Text"] = "Survey";
        //    dr2["Image"] = "/Content/Images/userNew.png";
        //    dr2["Link"] = "";
        //    dt2.Rows.Add(dr2);



        //    DataRow dr3 = dt2.NewRow();
        //    dr3["Text"] = "Survey Feedback";
        //    dr3["Image"] = "/Content/Images/userNew.png";
        //    dr3["Link"] = "";
        //    dt2.Rows.Add(dr3);
        //    return dt2;
        //}


    }
}