<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomCalendarView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.CustomCalendarView" %>
<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.XtraScheduler.v22.1.Core, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraScheduler" tagprefix="cc1" %>
<%@ Register assembly="DevExpress.XtraScheduler.v22.1.Core.Desktop, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraScheduler" tagprefix="cc1" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
   body #s4-ribbonrow {display:none !important;} 
      #s4-ribboncont{display:none !important;} 
    #s4-titlerow{ display:none !important;}
   .ms-menutoolbar {display:none !important;}
   .ms-cal-nav-buttonsltr{display:none !important;}

    .backgroundColorRed, .backgroundColorRedsel {
        background-color:#FFAAAA;
        font-family: Tahoma;
        font-size: 8pt;
        text-align: left;
    }

    .backgroundNormalTask, .backgroundNormalTasksel {
     font-family: Tahoma;
        font-size: 8pt;
        text-align: left;
    }
    .dxflHALSys.dxflVATSys.dxflCaptionCell_UGITNavyBlueDevEx.dxflCaptionCellSys {
        display: table-caption;
    }
   .dxscApt .dxsc-apt-gradient:not(.dxsc-apt-custom-bg) {
        background: linear-gradient(to bottom, #708eef, rgba(255, 255, 255, 0.25));
    }
    
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var ticketTitle = null;
    function ToolTipShowing(s, e) {
        e.cancel = true;
    }
    function OnAppointmentsSelectionChanged(s, appointmentIds) {
        s.appointmentToolTip = null
        if(appointmentIds != null && appointmentIds.length == 1) {
            s.GetAppointmentProperties(appointmentIds[0], 'Subject', OnGetAppointmentProps);
        } else
         OnGetAppointmentProps(null);
    }

    function OnGetAppointmentProps(values) {
        if(values != null)
            ticketTitle = values[0];
    }

    function OpenDetail(s, e) {
        s.appointmentToolTip = null;
        var apt = GetSelectedAppointment(calendarView);
        var url = '<%=ticketURL%>' + "?TicketId=" + e.appointmentId;
        var titleVal =  e.appointmentId + ": " + ticketTitle;
        window.parent.UgitOpenPopupDialog(url, "", titleVal, 80, 90, false, "");
    }

    function GetSelectedAppointment(calendarView) {
        var aptIds = calendarView.GetSelectedAppointmentIds();
        if (aptIds.length == 0)
            return;
        var apt = calendarView.GetAppointment(aptIds[0]);
        return apt;
    }

</script>

<asp:Panel ID="projectPlanContainer" runat="server">
    <div>
        <dx:ASPxFormLayout runat="server" ID="formLayout" CssClass="formLayout">
            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="500" />
              <Items>
                   <dx:LayoutItem >
                        <LayoutItemNestedControlCollection>
                             <dx:LayoutItemNestedControlContainer>
                                <dxwschs:ASPxScheduler ID="calendarView" ClientInstanceName="calendarView" runat="server" CssClass="CalenderDiv"
                                     OnAppointmentRowInserted="calendarView_AppointmentRowInserted" OnAppointmentsInserted="calendarView_AppointmentsInserted"
                                     AppointmentDataSourceID="drqDataSource" OnPopupMenuShowing="calendarView_PopupMenuShowing" OnInitAppointmentDisplayText="calendarView_InitAppointmentDisplayText">
                                    <ClientSideEvents AppointmentToolTipShowing="function(s, e) { ToolTipShowing(s,e); }"
                                        AppointmentClick="function(s, e) { OpenDetail(s,e); }"  
                                        AppointmentsSelectionChanged="function(s, e) { OnAppointmentsSelectionChanged(s, e.appointmentIds); }"
                                        MenuItemClicked="function(s, e) { OpenTaskDetail(s,e); }" />
                                    <Storage>
                                        <Appointments>
                                            <Mappings AppointmentId="TicketId" Start="ActualStartDate" End="ActualCompletionDate" Description="Description" Label="Title"
                                                 Status="Status" Subject="Title" />
                                        </Appointments>
                                    </Storage>
                                    <Views>
                                        <DayView>
                                            <TimeRulers>
                                                <cc1:TimeRuler />
                                            </TimeRulers>
                                            <AppointmentDisplayOptions ColumnPadding-Left="2" ColumnPadding-Right="4" />
                                        </DayView>
                                        <WorkWeekView>
                                            <TimeRulers>
                                                <cc1:TimeRuler />
                                            </TimeRulers>
                                            <AppointmentDisplayOptions ColumnPadding-Left="2" ColumnPadding-Right="4" />
                                        </WorkWeekView>
                                        <WeekView Enabled="false"></WeekView>
                                        <FullWeekView Enabled="true">
                                            <TimeRulers>
                                                <cc1:TimeRuler />
                                            </TimeRulers>
                                            <AppointmentDisplayOptions ColumnPadding-Left="2" ColumnPadding-Right="4" />
                                        </FullWeekView>
                                        <TimelineView Enabled="false"/>
                                        <AgendaView Enabled="false"/>
                                    </Views>
                                    <OptionsCustomization AllowInplaceEditor="None" AllowAppointmentCopy="None" AllowAppointmentDrag="None" AllowAppointmentDragBetweenResources="None" 
                                          AllowAppointmentEdit="None" AllowAppointmentDelete="None" AllowAppointmentMultiSelect="False" />
                                </dxwschs:ASPxScheduler>
                          </dx:LayoutItemNestedControlContainer>
                      </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
             </Items>
        </dx:ASPxFormLayout>
    </div>
    <asp:ObjectDataSource ID="drqDataSource" runat="server" DataObjectTypeName="CustomDRQEvent" TypeName="uGovernIT.Manager.Managers.CustomDRQDataSource" 
        DeleteMethod="DeleteMethodHandler" InsertMethod="InsertMethodHandler" UpdateMethod="UpdateMethodHandler" 
        SelectMethod="SelectMethodHandler" OnObjectCreated="drqDataSource_ObjectCreated" OnInserted="drqDataSource_Inserted"></asp:ObjectDataSource>   
</asp:Panel>