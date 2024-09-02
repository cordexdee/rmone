
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarView.ascx.cs" Inherits="uGovernIT.Web.CalendarView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v22.1.Core, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v22.1.Core.Desktop, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>


<dx:ASPxPanel ID="ASPxPanelCalendar" runat="server" Width="100%">
    <PanelCollection>
        <dx:PanelContent ID="PanelContentCalendar" runat="server">
            <dxwschs:ASPxScheduler ID="ASPxSchedulerCalendar" runat="server" Width="99%"  ActiveViewType="WorkWeek"
              GroupType="Resource" ClientInstanceName="schedulerCalendar" OptionsCustomization-AllowAppointmentDrag="None" OptionsCustomization-AllowAppointmentResize="None"
                OnPopupMenuShowing="ASPxSchedulerCalendar_PopupMenuShowing" OptionsCustomization-AllowInplaceEditor="None" OptionsToolTips-ShowAppointmentToolTip="false">
                <OptionsCookies Enabled="true" CookiesID="CalendarSchedulerCookies" StoreActiveViewType="true" />
               <%-- <OptionsToolTips AppointmentToolTipUrl="~/_CONTROLTEMPLATES/15/uGovernIT/CustomCalendarViewToolTip.ascx"/>--%>
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
                        </WeekViewStyles>
                    </WeekView>
                    <WeekView ResourcesPerPage="1">
                    </WeekView>

                    <MonthView AppointmentDisplayOptions-EndTimeVisibility="Never">
                        <MonthViewStyles>
                        </MonthViewStyles>
                    </MonthView>
                </Views>
                <OptionsBehavior RemindersFormDefaultAction="Custom" ShowRemindersForm="False" />
            </dxwschs:ASPxScheduler>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxPanel>
<asp:ObjectDataSource ID="CalendarDataSource" runat="server" />