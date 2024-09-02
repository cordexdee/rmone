
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMMEventView.ascx.cs" Inherits="uGovernIT.Web.AppointmentForm" %>
<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxScheduler.Controls" TagPrefix="dxwschsc" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v22.1.Core, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v22.1.Core.Desktop, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<table class="dxscAppointmentForm" cellpadding="0" cellspacing="0" style="width: 100%; height: 230px;">
    <tr>
        <td class="dxscDoubleCell" colspan="2">
            <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="dxscLabelCell">
                        <dx:ASPxLabel ID="lblSubject" runat="server" AssociatedControlID="tbSubject" Text="Title:">
                        </dx:ASPxLabel>
                    </td>
                    <td class="dxscControlCell">
                        <dx:ASPxTextBox ClientInstanceName="_dx" ID="tbSubject" runat="server" Width="100%" Text='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).Appointment.Subject %>'>
                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField ErrorText="The Title must contain at least one character." IsRequired="True" />
                            </ValidationSettings>
                            <ClientSideEvents ValueChanged="function(s, e) { OnUpdateControlValue(s, e); }" />
                        </dx:ASPxTextBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="dxscSingleCell">
            <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="dxscLabelCell">
                        <dx:ASPxLabel ID="lblLocation" runat="server" AssociatedControlID="tbLocation" Text="Location:">
                        </dx:ASPxLabel>
                    </td>
                    <td class="dxscControlCell">
                        <dx:ASPxTextBox ClientInstanceName="_dx" ID="tbLocation" runat="server" Width="100%" Text='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).Appointment.Location %>'>
                        </dx:ASPxTextBox>
                    </td>
                </tr>
            </table>
        </td>
        <td class="dxscSingleCell">
            <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="dxscLabelCell" style="padding-left: 25px;">
                        <dx:ASPxLabel ID="lblLabel" runat="server" AssociatedControlID="edtLabel" Text="Meeting Type:">
                        </dx:ASPxLabel>
                    </td>
                    <td class="dxscControlCell">
                        <table>
                            <tr>
                                <td>
                                    <dx:ASPxComboBox ClientInstanceName="dxeditLabel" EncodeHtml="false" ID="edtLabel" runat="server" Width="184px"  DataSource='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).LabelDataSource %>'>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxTextBox ID="txtMeetingType" runat="server" ClientInstanceName="dxMeetingType" ClientVisible="false">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxImage ID="imgadd" ClientInstanceName="dximgadd" runat="server" ImageUrl="/Content/Images/add_icon.png">
                                        <ClientSideEvents Click="function(s,e) { OnClickShowControl(s,e); }" />
                                    </dx:ASPxImage>
                                    <dx:ASPxImage ID="imgCancel" ClientVisible="false" ClientInstanceName="dximgcancel" runat="server" ImageUrl="/Content/Images/cancel-icon.png">
                                        <ClientSideEvents Click="function(s,e) { OnClickShowDDLControl(s,e); }" />
                                    </dx:ASPxImage>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="dxscSingleCell">
            <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="dxscLabelCell">
                        <dx:ASPxLabel ID="lblStartDate" runat="server" AssociatedControlID="edtStartDate" Text="Start time:" Wrap="false">
                        </dx:ASPxLabel>
                    </td>
                    <td class="dxscControlCell">
                        <dx:ASPxDateEdit ID="edtStartDate" runat="server" Width="100%" Date='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).Start %>' EditFormat="DateTime" DateOnError="Undo" AllowNull="false" EnableClientSideAPI="true" >
                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" EnableCustomValidation="True" Display="Dynamic"
                                ValidationGroup="DateValidatoinGroup">
                            </ValidationSettings>
                        </dx:ASPxDateEdit>
                        <dx:ASPxDateEdit ID="edtStartDateAllDay" runat="server" Width="100%" Date='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).Start %>' EditFormat="DateTime" DateOnError="Undo" AllowNull="false" EnableClientSideAPI="true" >
                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" EnableCustomValidation="True" Display="Dynamic"
                                ValidationGroup="DateValidatoinGroup">
                            </ValidationSettings>
                        </dx:ASPxDateEdit>
                    </td>
                </tr>
            </table>
        </td>
        <td class="dxscSingleCell">
            <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="dxscLabelCell" style="padding-left: 25px;">
                        <dx:ASPxLabel runat="server" ID="lblEndDate" Text="End time:" Wrap="false" AssociatedControlID="edtEndDate" />
                    </td>
                    <td class="dxscControlCell">
                        <dx:ASPxDateEdit id="edtEndDate" runat="server" Date='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).End %>' EditFormat="DateTime" Width="100%" DateOnError="Undo" AllowNull="false" EnableClientSideAPI="true">
                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" EnableCustomValidation="True" Display="Dynamic"
                                ValidationGroup="DateValidatoinGroup">
                            </ValidationSettings>
                        </dx:ASPxDateEdit>
                        <dx:ASPxDateEdit id="edtEndDateAllDay" runat="server" Date='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).End %>' EditFormat="DateTime" Width="100%" DateOnError="Undo" AllowNull="false" EnableClientSideAPI="true">
                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" EnableCustomValidation="True" Display="Dynamic"
                                ValidationGroup="DateValidatoinGroup">
                            </ValidationSettings>
                        </dx:ASPxDateEdit>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="dxscSingleCell">
            <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="dxscLabelCell">
                        <dx:ASPxLabel ID="lblStatus" runat="server" AssociatedControlID="edtStatus" Text="Show time as:" Wrap="false">
                        </dx:ASPxLabel>
                    </td>
                    <td class="dxscControlCell">
                        <dx:ASPxComboBox ClientInstanceName="_dx" ID="edtStatus" runat="server" Width="100%" DataSource='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).StatusDataSource %>' />
                    </td>
                </tr>
            </table>
        </td>
        <td class="dxscSingleCell" style="padding-left: 22px;">
            <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 20px; height: 20px;">
                        <dx:ASPxCheckBox ClientInstanceName="_dx" ID="chkAllDay" runat="server" Checked='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).Appointment.AllDay %>' />
                    </td>
                    <td style="padding-left: 2px;">
                        <dx:ASPxLabel ID="lblAllDay" runat="server" Text="All day event" AssociatedControlID="chkAllDay" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <%--<tr>
        <% if (CanShowReminders)
           { %>
        <td class="dxscSingleCell">
            <% }
           else
           { %>
        <td class="dxscDoubleCell" colspan="2">
            <% } %>
            <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="dxscLabelCell">
                        <dx:ASPxLabel ID="lblResource" runat="server" AssociatedControlID="edtResource" Text="Resource:">
                        </dx:ASPxLabel>
                    </td>
                    <td class="dxscControlCell">
                        <dx:ASPxComboBox ClientInstanceName="_dx" ID="edtResource" runat="server" Width="100%" DataSource='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).ResourceDataSource %>' Enabled='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).CanEditResource %>' />
                    </td>
                </tr>
            </table>
        </td>
        <% if (CanShowReminders)
           { %>
        <td class="dxscSingleCell">
            <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="dxscLabelCell" style="padding-left: 22px;">
                        <table class="dxscLabelControlPair" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 20px; height: 20px;">
                                    <dx:ASPxCheckBox ClientInstanceName="_dx" ID="chkReminder" runat="server">
                                        <ClientSideEvents CheckedChanged="function(s, e) { OnChkReminderCheckedChanged(s, e); }" />
                                    </dx:ASPxCheckBox>
                                </td>
                                <td style="padding-left: 2px;">
                                    <dx:ASPxLabel ID="lblReminder" runat="server" Text="Reminder" AssociatedControlID="chkReminder" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="dxscControlCell" style="padding-left: 3px">
                        <dx:ASPxComboBox ID="cbReminder" ClientInstanceName="_dxAppointmentForm_cbReminder" runat="server" Width="100%" DataSource='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).ReminderDataSource %>' />
                    </td>
                </tr>
            </table>
        </td>
        <% } %>
    </tr>--%>
    <tr>
        <td class="dxscDoubleCell" colspan="2" style="height: 90px;">
            <dx:ASPxMemo ClientInstanceName="_dx" ID="tbDescription" runat="server" Width="100%" Rows="3" Text='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).Appointment.Description %>' />
            &nbsp;
        </td>
    </tr>
</table>

<dxwschsc:AppointmentRecurrenceForm ID="AppointmentRecurrenceForm1" runat="server"
    IsRecurring='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).Appointment.IsRecurring %>'
    DayNumber='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).RecurrenceDayNumber %>'
    End='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).RecurrenceEnd %>'
    Month='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).RecurrenceMonth %>'
    OccurrenceCount='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).RecurrenceOccurrenceCount %>'
    Periodicity='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).RecurrencePeriodicity %>'
    RecurrenceRange='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).RecurrenceRange %>'
    Start='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).RecurrenceStart %>'
    WeekDays='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).RecurrenceWeekDays %>'
    WeekOfMonth='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).RecurrenceWeekOfMonth %>'
    RecurrenceType='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).RecurrenceType %>'
    IsFormRecreated='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).IsFormRecreated %>'>
</dxwschsc:AppointmentRecurrenceForm>

<table cellpadding="0" cellspacing="0" style="width: 100%; height: 35px;">
    <tr>
        <td style="width: 100%; height: 100%;" align="center">
            <table style="height: 100%;">
                <tr>
                    <td>
                        <dx:ASPxButton runat="server" ClientInstanceName="btnMyAppointmentFormOk" ID="btnOk" Text="OK" UseSubmitBehavior="false" AutoPostBack="false"
                            EnableViewState="false" Width="91px" ValidationGroup="MyValidationGroup">
                            <ClientSideEvents Init="function(s, e) { OnUpdateControlValue(s, e); }" />
                        </dx:ASPxButton>
                    </td>
                    <td style="padding-left:10px;">
                        <dx:ASPxButton runat="server" ClientInstanceName="_dx" ID="btnCancel" Text="Cancel" UseSubmitBehavior="false" AutoPostBack="false" EnableViewState="false"
                            Width="91px" CausesValidation="False" />
                    </td>
                    <td style="padding-left:10px;">
                        <dx:ASPxButton runat="server" ClientInstanceName="_dx" ID="btnDelete" Text="Delete" UseSubmitBehavior="false"
                            AutoPostBack="false" EnableViewState="false" Width="91px"
                            Enabled='<%# ((uGovernIT.Manager.PMMEventViewTemplateContainer)Container).CanDeleteAppointment %>'
                            CausesValidation="False" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<table cellpadding="0" cellspacing="0" style="width: 100%;">
    <tr>
        <td style="width: 100%;" align="left">
            <dxwschsc:ASPxSchedulerStatusInfo runat="server" ID="schedulerStatusInfo" Priority="1" MasterControlID='<%# ((DevExpress.Web.ASPxScheduler.AppointmentFormTemplateContainer)Container).ControlId %>' />
        </td>
    </tr>
</table>

<script id="dxss_ASPxSchedulerAppoinmentForm" type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
   
    ASPxAppointmentForm = ASPx.CreateClass(ASPxClientFormBase, {
        Initialize: function () {
            this.controls.edtStartDate.Validation.AddHandler(ASPx.CreateDelegate(this.OnEdtStartDateValidate, this));
            this.controls.edtEndDate.Validation.AddHandler(ASPx.CreateDelegate(this.OnEdtEndDateValidate, this));
            this.controls.edtStartDateAllDay.Validation.AddHandler(ASPx.CreateDelegate(this.OnEdtStartDateAllDayValidate, this));
            this.controls.edtEndDateAllDay.Validation.AddHandler(ASPx.CreateDelegate(this.OnEdtEndDateAllDayValidate, this));
            this.controls.edtStartDate.ValueChanged.AddHandler(ASPx.CreateDelegate(this.OnUpdateStartEndValue, this));
            this.controls.edtEndDate.ValueChanged.AddHandler(ASPx.CreateDelegate(this.OnUpdateStartEndValue, this));
            this.controls.edtStartDateAllDay.ValueChanged.AddHandler(ASPx.CreateDelegate(this.OnUpdateStartEndValue, this));
            this.controls.edtEndDateAllDay.ValueChanged.AddHandler(ASPx.CreateDelegate(this.OnUpdateStartEndValue, this));
            this.controls.chkAllDay.CheckedChanged.AddHandler(ASPx.CreateDelegate(this.OnChkAllDayCheckedChanged, this));
        },
        OnEdtStartDateValidate: function (s, e) {
            if (!e.isValid)
                return;
            var startDate = this.controls.edtStartDate.GetDate();
            var endDate = this.controls.edtEndDate.GetDate();
            e.isValid = startDate == null || endDate == null || startDate <= endDate;
            e.errorText = "The Start Date must precede the End Date.";
        },
        OnEdtEndDateValidate: function (s, e) {
            if (!e.isValid)
                return;
            var startDate = this.controls.edtStartDate.GetDate();
            var endDate = this.controls.edtEndDate.GetDate();
            e.isValid = startDate == null || endDate == null || startDate <= endDate;
            e.errorText = "The Start Date must precede the End Date.";
        },
        OnEdtStartDateAllDayValidate: function (s, e) {
            if (!e.isValid)
                return;
            var startDate = this.controls.edtStartDateAllDay.GetDate();
            var endDate = this.controls.edtEndDateAllDay.GetDate();
            e.isValid = startDate == null || endDate == null || startDate <= endDate;
            e.errorText = "The Start Date must precede the End Date.";
        },
        OnEdtEndDateAllDayValidate: function (s, e) {
            if (!e.isValid)
                return;
            var startDate = this.controls.edtStartDateAllDay.GetDate();
            var endDate = this.controls.edtEndDateAllDay.GetDate();
            e.isValid = startDate == null || endDate == null || startDate <= endDate;
            e.errorText = "The Start Date must precede the End Date.";
        },
        OnUpdateStartEndValue: function (s, e) {
            var isValid = ASPxClientEdit.ValidateEditorsInContainer(null);
            this.controls.btnOk.SetEnabled(isValid);
        },
        OnChkAllDayCheckedChanged: function (s, e) {
            var isAllDayEnabled = this.controls.chkAllDay.GetValue();
            this.controls.edtStartDate.SetVisible(!isAllDayEnabled);
            this.controls.edtStartDateAllDay.SetVisible(isAllDayEnabled);
            this.controls.edtEndDate.SetVisible(!isAllDayEnabled);
            this.controls.edtEndDateAllDay.SetVisible(isAllDayEnabled);
            if (isAllDayEnabled) {
                var currentDate = this.controls.edtEndDate.GetValue();
                var result = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate());
                if (currentDate.toString() == result.toString()) {
                    this.controls.edtEndDateAllDay.SetValue(new Date(result.getTime() - 86400000));
                }
                else {
                    this.controls.edtEndDateAllDay.SetValue(result);
                }
            }
            else {
                var currentDate = this.controls.edtEndDateAllDay.GetValue();
                var result = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate());
                this.controls.edtEndDate.SetValue(new Date(result.getTime() + 86400000));
            }
        }
    });

    function OnUpdateControlValue(s, e) {
        
        var isValid = ASPxClientEdit.ValidateEditorsInContainer(null);
        btnMyAppointmentFormOk.SetEnabled(isValid)
    }

    function OnClickShowControl(s, e) {
        dxeditLabel.SetVisible(false);
        dxMeetingType.SetVisible(true);
    }

       function OnClickShowControl(s, e) {
        dxeditLabel.SetVisible(false);
        dxMeetingType.SetVisible(true);
        dximgadd.SetVisible(false);
        dximgcancel.SetVisible(true);
    }

    function OnClickShowDDLControl(s, e) {
        dxeditLabel.SetVisible(true);
        dxMeetingType.SetVisible(false);
        dximgadd.SetVisible(true);
        dximgcancel.SetVisible(false);
        dxMeetingType.SetText('');
    }
    window.setTimeout("ASPxClientEdit.ValidateEditorsInContainer(null)", 0);
</script>



