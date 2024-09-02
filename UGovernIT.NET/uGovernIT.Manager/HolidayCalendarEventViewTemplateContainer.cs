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
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class HolidayCalendarEventViewTemplateContainer : AppointmentFormTemplateContainer
    {
        public HolidayCalendarEventViewTemplateContainer(ASPxScheduler control)
            : base(control)
        {

        }
    }

    #region #userappointmentformcontroller
    public class HolidayCalendarEventViewController : AppointmentFormController
    {
        public HolidayCalendarEventViewController(ASPxScheduler control, DevExpress.XtraScheduler.Appointment apt)
            : base(control, apt)
        {
        }

        protected override void ApplyCustomFieldsValues()
        {
        }

    }
    #endregion #userappointmentformcontroller


    #region #userappointmentsavecallbackcommand
    public class HolidayCalendarEventSaveCallbackCommand : AppointmentFormSaveCallbackCommand
    {
        public HolidayCalendarEventSaveCallbackCommand(ASPxScheduler control)
            : base(control)
        {
        }
        protected internal new HolidayCalendarEventViewController Controller
        {
            get { return (HolidayCalendarEventViewController)base.Controller; }
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
            ASPxDateEdit edtEndDateTime = (ASPxDateEdit)FindControlByID("edtEndDateTime");

            if (chkAllDay != null)
                Controller.AllDay = chkAllDay.Checked;
            DateTime clientStart = DateTime.MinValue;
            DateTime clientEnd = DateTime.MinValue;

            if (Controller.AllDay)
            {
                clientStart = FromClientTime(edtStartDateAllDay.Date);
                Controller.EditedAppointmentCopy.Start = clientStart;
            }
            else
            {
                clientStart = FromClientTime(edtStartDate.Date);
                Controller.EditedAppointmentCopy.Start = clientStart;
            }





            if (tbSubject != null)
                Controller.Subject = tbSubject.Text;
            if (tbLocation != null)
                Controller.Location = tbLocation.Text;
            if (tbDescription != null)
                Controller.Description = tbDescription.Text;
            if (edtShowTimeAs != null)
                Controller.StatusKey = UGITUtility.StringToInt(edtShowTimeAs.Value);
            if (edtLabel != null)
                Controller.LabelKey = UGITUtility.StringToInt(edtLabel.Value);
            AssignControllerRecurrenceValues(clientStart);

            if (Controller.EditedAppointmentCopy.IsRecurring)
            {


                if (Controller.EditedAppointmentCopy.RecurrenceInfo.Range == RecurrenceRange.NoEndDate)
                {
                    if (Controller.EditedPattern.RecurrenceInfo.Type == RecurrenceType.Daily)
                    {
                        Controller.EditedPattern.RecurrenceInfo.End = Controller.EditedAppointmentCopy.End.Date.AddYears(2);
                    }
                    else if (Controller.EditedPattern.RecurrenceInfo.Type == RecurrenceType.Weekly)
                    {
                        Controller.EditedPattern.RecurrenceInfo.End = Controller.EditedAppointmentCopy.End.Date.AddYears(20);
                    }
                    else if (Controller.EditedPattern.RecurrenceInfo.Type == RecurrenceType.Monthly)
                    {
                        Controller.EditedPattern.RecurrenceInfo.End = Controller.EditedAppointmentCopy.End.Date.AddYears(84);
                    }
                    else if (Controller.EditedPattern.RecurrenceInfo.Type == RecurrenceType.Yearly)
                    {
                        Controller.EditedPattern.RecurrenceInfo.End = Controller.EditedAppointmentCopy.End.Date.AddYears(150);
                    }
                }

                if (!Controller.AllDay)
                {
                    Controller.EditedPattern.RecurrenceInfo.End = new DateTime(Controller.EditedPattern.RecurrenceInfo.End.Date.Year, Controller.EditedPattern.RecurrenceInfo.End.Date.Month, Controller.EditedPattern.RecurrenceInfo.End.Date.Day, edtEndDateTime.Date.Hour, edtEndDateTime.Date.Minute, edtEndDateTime.Date.Second);
                }
            }
            else
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
        }

        //protected override void AssignControllerValues()
        //{
        //    base.AssignControllerValues();

        //    //if (this.Controller.AllDay)
        //    //{
        //    //    this.Controller.End = this.Controller.End.AddHours(1);
        //    //    //this.Controller.DisplayEnd = this.Controller.DisplayEnd.AddHours(-1);
        //    //}

        //}
        protected override AppointmentFormController CreateAppointmentFormController
            (DevExpress.XtraScheduler.Appointment apt)
        {
            return new HolidayCalendarEventViewController(Control, apt);
        }
    }
    #endregion #userappointmentsavecallbackcommand
}
