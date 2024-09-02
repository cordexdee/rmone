using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
using DevExpress.XtraScheduler;
using DevExpress.Web;

namespace uGovernIT.Manager
{
    public class PMMEventViewTemplateContainer : AppointmentFormTemplateContainer
    {
        public PMMEventViewTemplateContainer(ASPxScheduler control)
            : base(control)
        {
        }
        public string MeetingType { get { return Convert.ToString(Appointment.CustomFields["MeetingType"]); } }
    }


    #region #userappointmentformcontroller
    public class PMMEventViewController : AppointmentFormController
    {
        public PMMEventViewController(ASPxScheduler control, Appointment apt)
            : base(control, apt)
        {
        }
        public string MeetingType
        {
            get { return (string)EditedAppointmentCopy.CustomFields["MeetingType"]; }
            set { EditedAppointmentCopy.CustomFields["MeetingType"] = value; }
        }
        string SourceField2
        {
            get { return (string)SourceAppointment.CustomFields["MeetingType"]; }
            set { SourceAppointment.CustomFields["MeetingType"] = value; }
        }

        protected override void ApplyCustomFieldsValues()
        {
            SourceField2 = MeetingType;
        }
    }
    #endregion #userappointmentformcontroller


    #region #userappointmentsavecallbackcommand
    public class PMMEventSaveCallbackCommand : AppointmentFormSaveCallbackCommand
    {
        public PMMEventSaveCallbackCommand(ASPxScheduler control)
            : base(control)
        {
        }
        protected internal new PMMEventViewController Controller
        {
            get { return (PMMEventViewController)base.Controller; }
        }

        protected override void AssignControllerValues()
        {
            ASPxTextBox tbSubject = (ASPxTextBox)FindControlByID("tbSubject");
            ASPxTextBox tbLocation = (ASPxTextBox)FindControlByID("tbLocation");
            ASPxMemo tbDescription = (ASPxMemo)FindControlByID("tbDescription");
            ASPxDateEdit edtStartDate = (ASPxDateEdit)FindControlByID("edtStartDate");
            ASPxDateEdit edtEndDate = (ASPxDateEdit)FindControlByID("edtEndDate");
            ASPxDateEdit edtStartDateAllDay = (ASPxDateEdit)FindControlByID("edtStartDateAllDay");
            ASPxDateEdit edtEndDateAllDay = (ASPxDateEdit)FindControlByID("edtEndDateAllDay");
            ASPxCheckBox chkAllDay = (ASPxCheckBox)FindControlByID("chkAllDay");
            ASPxComboBox edtShowTimeAs = (ASPxComboBox)FindControlByID("edtStatus");
            ASPxComboBox edtLabel = (ASPxComboBox)FindControlByID("edtLabel");
            ASPxTextBox MeetingType = (ASPxTextBox)FindControlByID("txtMeetingType");
           

            if (chkAllDay != null)
                Controller.AllDay = chkAllDay.Checked;
            DateTime clientStart = DateTime.MinValue;
            DateTime clientEnd = DateTime.MinValue;
            if (edtStartDate != null)
            {
                if (Controller.AllDay)
                {
                    Controller.EditedAppointmentCopy.Start = edtStartDate.Date;
                }
                else
                {
                    clientStart = FromClientTime(edtStartDate.Date);
                    Controller.EditedAppointmentCopy.Start = clientStart;
                }
            }
            if (edtEndDate != null)
            {
                if (Controller.AllDay)
                {
                    Controller.EditedAppointmentCopy.End = Convert.ToDateTime(edtEndDateAllDay.Value).AddDays(1);
                }
                else
                {
                    clientEnd = FromClientTime(edtEndDate.Date);
                    Controller.EditedAppointmentCopy.End = clientEnd;
                }
            }
            if (tbSubject != null)
                Controller.Subject = tbSubject.Text;
            if (tbLocation != null)
                Controller.Location = tbLocation.Text;
            if (tbDescription != null)
                Controller.Description = tbDescription.Text;
            if (edtShowTimeAs != null)
                Controller.StatusKey = Convert.ToInt32(edtShowTimeAs.Value);
            if (edtLabel != null)
                Controller.LabelKey = Convert.ToInt32(edtLabel.Value);
            Controller.MeetingType = MeetingType.Text;
            AssignControllerRecurrenceValues(clientStart);
        }

        //protected override void AssignControllerValues()
        //{
        //    ASPxTextBox MeetingType = (ASPxTextBox)FindControlByID("txtMeetingType");
        //    Controller.MeetingType = MeetingType.Text;

        //    base.AssignControllerValues();
        //}
        protected override AppointmentFormController CreateAppointmentFormController
            (Appointment apt)
        {
            return new PMMEventViewController(Control, apt);
        }
    }
    #endregion #userappointmentsavecallbackcommand

}
