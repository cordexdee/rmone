using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility.Entities;
using DevExpress.Web;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
using DevExpress.XtraScheduler;
using System.Collections.Generic;
using DevExpress.XtraScheduler.Localization;
using DevExpress.Web.ASPxScheduler.Localization;
using DevExpress.Utils;

namespace uGovernIT.Web
{
    public partial class AppointmentForm : SchedulerFormControl 
    {
        public override string ClassName { get { return "ASPxAppointmentForm"; } }
        public bool CanShowReminders
        {
            get
            {
                return ((AppointmentFormTemplateContainer)Parent).Control.Storage.EnableReminders;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Localize();
            tbSubject.Focus();
        }
        void Localize()
        {
            lblSubject.Text = ASPxSchedulerLocalizer.GetString(ASPxSchedulerStringId.Form_Subject);
            lblLocation.Text = ASPxSchedulerLocalizer.GetString(ASPxSchedulerStringId.Form_Location);
            lblLabel.Text = ASPxSchedulerLocalizer.GetString(ASPxSchedulerStringId.Form_Label);
            lblStartDate.Text = ASPxSchedulerLocalizer.GetString(ASPxSchedulerStringId.Form_StartTime);
            lblEndDate.Text = ASPxSchedulerLocalizer.GetString(ASPxSchedulerStringId.Form_EndTime);
            lblStatus.Text = ASPxSchedulerLocalizer.GetString(ASPxSchedulerStringId.Form_Status);
            lblAllDay.Text = ASPxSchedulerLocalizer.GetString(ASPxSchedulerStringId.Form_AllDayEvent);

            btnOk.Text = ASPxSchedulerLocalizer.GetString(ASPxSchedulerStringId.Form_ButtonOk);
            btnCancel.Text = ASPxSchedulerLocalizer.GetString(ASPxSchedulerStringId.Form_ButtonCancel);
            btnDelete.Text = ASPxSchedulerLocalizer.GetString(ASPxSchedulerStringId.Form_ButtonDelete);
            btnOk.Wrap = DefaultBoolean.False;
            btnCancel.Wrap = DefaultBoolean.False;
            btnDelete.Wrap = DefaultBoolean.False;
        }
        public override void DataBind()
        {
            base.DataBind();

            AppointmentFormTemplateContainer container = (AppointmentFormTemplateContainer)Parent;
            DevExpress.XtraScheduler.Appointment apt = container.Appointment;

            edtStartDateAllDay.ClientVisible = container.Appointment.AllDay;
            edtStartDate.ClientVisible = !container.Appointment.AllDay;
            edtEndDateAllDay.ClientVisible = container.Appointment.AllDay;
            edtEndDate.ClientVisible = !container.Appointment.AllDay;

            if (container.Appointment.AllDay)
            {
                edtEndDateAllDay.Value = Convert.ToDateTime(edtEndDate.Value).AddDays(-1);
            }


            edtLabel.SelectedIndex = UGITUtility.StringToInt(apt.LabelKey);
            edtStatus.SelectedIndex = UGITUtility.StringToInt(apt.StatusKey);
            AppointmentRecurrenceForm1.Visible = container.ShouldShowRecurrence;

            btnOk.ClientSideEvents.Click = container.SaveHandler;
            btnCancel.ClientSideEvents.Click = container.CancelHandler;
            btnDelete.ClientSideEvents.Click = container.DeleteHandler;
        }
        protected override void PrepareChildControls()
        {
            AppointmentFormTemplateContainer container = (AppointmentFormTemplateContainer)Parent;
            ASPxScheduler control = container.Control;

            AppointmentRecurrenceForm1.EditorsInfo = new EditorsInfo(control, control.Styles.FormEditors, control.Images.FormEditors, control.Styles.Buttons);
            base.PrepareChildControls();
        }
        protected override ASPxEditBase[] GetChildEditors()
        {
            ASPxEditBase[] edits = new ASPxEditBase[] {
			lblSubject, tbSubject,
			lblLocation, tbLocation,
			lblLabel, edtLabel,
			lblStartDate, edtStartDate,edtStartDateAllDay,
			lblEndDate, edtEndDate,edtEndDateAllDay,
			lblStatus, edtStatus,
			lblAllDay, chkAllDay,
			tbDescription
            ,txtMeetingType
		};
            return edits;
        }
        protected override ASPxButton[] GetChildButtons()
        {
            ASPxButton[] buttons = new ASPxButton[] {
			btnOk, btnCancel, btnDelete
		};
            return buttons;
        }
      
    }
}
