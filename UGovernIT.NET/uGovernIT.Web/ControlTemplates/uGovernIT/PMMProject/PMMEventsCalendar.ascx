
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMMEventsCalendar.ascx.cs" Inherits="uGovernIT.Web.PMMEventsCalendar" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>

<%@ Register Assembly="DevExpress.XtraScheduler.v22.1.Core, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v22.1.Core.Desktop, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>
<%@ Register Src="~/ControlTemplates/uGovernIT/PMMProject/PMMEventView.ascx" TagPrefix="ugit" TagName="PMMEventView" %>



<script lang="cs">
    function OnClientPopupMenuShowing(s, e) {
        
        var selectedApptIDs = scheduler.GetSelectedAppointmentIds();
        if (selectedApptIDs.length > 0) {
            var selectedAppointment = scheduler.GetAppointmentById(selectedApptIDs[0]);
            var apptSubject = selectedAppointment.subject;
            var apptid = selectedAppointment.appointmentId;
            for (menuItemId in e.item.items) {
                if (e.item.items[menuItemId].name != "OpenAppointment") {
                    if (apptid.indexOf("ProjectTask") > 0) {
                        e.item.items[menuItemId].SetVisible(false);
                    }
                }

                if (e.item.items[menuItemId].name == "LabelSubMenu") {
                    e.item.items[menuItemId].SetVisible(false);
                }
              
            }
        }
    }

    function OnAppointmentMenuItemClick(scheduler, s, e) {
       
        var selectedApptIDs = scheduler.GetSelectedAppointmentIds();
        if (selectedApptIDs.length > 0) {
            var selectedAppointment = scheduler.GetAppointmentById(scheduler.GetSelectedAppointmentIds()[0]);
            var apptSubject = selectedAppointment.subject;
            var apptid = selectedAppointment.appointmentId;
            if (e.item.name == "OpenAppointment" && apptid.indexOf("ProjectTask") >= 0) {
                var ret = apptid.split(":");
                var str1 = ret[0];
                var str2 = ret[1];
                var url = "<%=ProjectTaskURL%>";
                UgitOpenPopupDialog(url + '?projectID=' + '<%=ProjectId%>' + '&ModuleName=PMM&taskId=' + str1, '', 'Edit Task :' + '<%=ProjectPublicID%>', '90', '150', 0, true);
            }
            else {
                e.handled = true;
                scheduler.RaiseCallback('MNUAPT|' + e.item.name);
            }
        }
        else {
            e.handled = true;
            scheduler.RaiseCallback('MNUAPT|' + e.item.name);
        }
    }
    </script>

<dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%">
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <dxwschs:ASPxScheduler ID="ASPxScheduler1" runat="server" Width="99%"  ActiveViewType="WorkWeek"
                OnAppointmentFormShowing="ASPxScheduler1_AppointmentFormShowing" OptionsCustomization-AllowAppointmentDrag="None" OptionsCustomization-AllowAppointmentResize="None" 
                GroupType="Resource" ClientInstanceName="scheduler" OnBeforeExecuteCallbackCommand="ASPxScheduler1_BeforeExecuteCallbackCommand" 
                OnInitClientAppointment="ASPxScheduler1_InitClientAppointment"
                OnPopupMenuShowing="ASPxScheduler1_PopupMenuShowing" >
                <OptionsForms AppointmentFormTemplateUrl="~/CONTROLTEMPLATES/uGovernIt/PMMProject/PMMEventView.ascx" />
                <OptionsCookies Enabled="true" CookiesID="SchedulerCookies" StoreActiveViewType="true" />
                <Views>
                    <DayView ResourcesPerPage="1" DayCount="3" TimeScale="60">
                        <TimeRulers>
                            <cc1:TimeRuler />
                        </TimeRulers>
                    </DayView>

                    <WorkWeekView TimeScale="60">
                        <TimeRulers>
                            <cc1:TimeRuler />
                        </TimeRulers>
                    </WorkWeekView>

                    <WeekView AppointmentDisplayOptions-EndTimeVisibility="Never">
                        <WeekViewStyles>
                            <DateCellBody Height="60px" />
                        </WeekViewStyles>
                    </WeekView>
                    <WeekView ResourcesPerPage="1">
                    </WeekView>

                    <MonthView AppointmentDisplayOptions-EndTimeVisibility="Never">
                        <MonthViewStyles>
                            <DateCellBody Height="30px" />
                        </MonthViewStyles>
                    </MonthView>
                </Views>
                <OptionsBehavior RemindersFormDefaultAction="Custom" ShowRemindersForm="False" />
                <ClientSideEvents AppointmentDoubleClick="function(s,e){e.handled = true;}" />
            </dxwschs:ASPxScheduler>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxPanel>
<asp:ObjectDataSource ID="appointmentDataSource" runat="server" />
