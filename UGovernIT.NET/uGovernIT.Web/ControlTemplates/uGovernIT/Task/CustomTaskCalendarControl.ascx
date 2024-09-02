
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomTaskCalendarControl.ascx.cs" Inherits="uGovernIT.Web.CustomTaskCalendarControl" %>
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
    .dxsc-dat-top.dxsc-dat-callout, .dxsc-dat-bottom.dxsc-dat-callout,.dxsc-dat-left.dxsc-dat-callout, .dxsc-dat-right.dxsc-dat-callout    
    {
        box-shadow: none;
        background-color: white;
    }
    .dxsc-detailed-apt-tooltip
    {
        background-color: white;
    }
    .dxscApt.dxsc-selected .dxsc-apt-content-layer span
    {
        color: #fff; /*#cb7c05;*/
        font-weight: bold;
    }
    .dxsc-dat-buttons-container .dxbButtonSys:first-child {
        border-right: 1px solid #ccc !important;
    }
#ctl00_ctl00_MainContent_ContentPlaceHolderContainer_ctl00_formLayout_calendarTaskView .dxsc-al0 {
        background-color: #d5ae73;
}
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    <%--$(function () {
        var strUN = ASPxHFCalendar.Get("tooltipdata");
        var json = $.parseJSON(strUN);
        $(".CalenderDiv > div").css("overflow", "");

        var className = $('.ms-cal-topdayfocus');
        if(className != null)
            {
        $(className).removeClass("ms-cal-topdayfocus").addClass("ms-cal-topday");
        $(className).unbind("mouseout");
        $(className).unbind("mouseout");
        $(className).bind("mouseout", function (e) {
            $(className).removeClass("ms-cal-topdayfocus").addClass("ms-cal-topday");
                    return false;
                });
        }

        $.each($(".ms-cal-gempty a"), function (i, item) {
           
            $.each($(json),function(j,jsonitem)
            {
                if ($(item).attr("href") != null && $(item).attr("href").indexOf("moduleName=" + jsonitem.module) >= 0 && $(item).attr("href").indexOf("taskID="+jsonitem.taskid) >= 0)
                {
                    $(item).attr('title', jsonitem.tooltip);
                    try {
                        $(item).tooltip({
                            hide: { duration: 1000, effect: "fadeOut", easing: "easeInExpo" },
                            content: function () {
                                            var title = $(this).attr("title");
                                            if (title)
                                                return title.replace(/\n/g, "<br/>");
                            }
                        });
                    }
                    catch (ex) {
                    }
                    return;
                }
            });

            $(item).bind("click", function (e) {
                e.preventDefault();
                var tasktitle;
                $.each($(json), function (j, jsonitem) {
                    if ($(item).attr("href") != null && $(item).attr("href").indexOf("moduleName=" + jsonitem.module) >= 0 && $(item).attr("href").indexOf("taskID="+jsonitem.taskid) >= 0)
                    {
                        tasktitle = jsonitem.tasktitle;
                        return;
                    }
                });
                OpenTicketDetail($(item).attr("href"),tasktitle);
                return false;
            });
        });
    });--%>
    
    
    function OpenTaskDetail(s, e) {

        e.handled = true;
        if (e.itemName == undefined || e.itemName == ASPx.SchedulerMenuItemId.OpenAppointment || e.itemName == ASPx.SchedulerMenuItemId.EditSeries) {
            var apt = GetSelectedAppointment(calendarTaskView);
            var url = "/Layouts/uGovernIT/DelegateControl.aspx?control=taskedit&TaskId=" + apt.appointmentId + "&TicketId=" + '<%=ProjectPublicId%>' + "&ModuleName=" + '<%=ModuleName%>';
            //url = url.replace("?ID=", "");
            window.parent.UgitOpenPopupDialog(url, "", "Task: " + apt.subject, '70', '100', false, "");
        }
        if (e.itemName == ASPx.SchedulerMenuItemId.NewAppointment) {
            var url = "/Layouts/uGovernIT/DelegateControl.aspx?control=taskedit&TaskId=" + 0 + "&TicketId=" + '<%=ProjectPublicId%>' + "&ModuleName=" + '<%=ModuleName%>';
            //url = url.replace("?ID=", "");
            window.parent.UgitOpenPopupDialog(url, "", "Task: ", '70', '100', false, "");
        }
    }

    function GetSelectedAppointment(calendarTaskView) {
        var aptIds = calendarTaskView.GetSelectedAppointmentIds();
        if (aptIds.length == 0)
            return;
        var apt = calendarTaskView.GetAppointment(aptIds[0]);
        return apt;
    }
    <%--function BindTodayTask()
    {
        var viewType = $('#<%=ddlViewType.ClientID%> :selected').val();
        var d = new Date();
        var month = d.getMonth() +1;
        var day = d.getDate();
        var year = d.getFullYear()

        if (viewType == 0) {
            javascript: MoveToViewDate('month\u002fday\u002fyear', null);
        }
        else if (viewType == 1) {
            javascript: MoveToDate('month\u002fday\u002fyear');
        }
        else {
            javascript: MoveToDate('month\u002fday\u002fyear');
        }
        return false;
    }--%>

</script>

<asp:Panel ID="TaskContainer" runat="server">
    <%--<div style="margin-top:4px;margin-left:5px;">
        <asp:DropDownList ID="ddlViewType" runat="server" AutoPostBack="true">
            <asp:ListItem Text="Monthly View" Value="0" ></asp:ListItem>
            <asp:ListItem Text="Weekly View" Value="1" ></asp:ListItem>
            <asp:ListItem Text="Daily View" Value="2" ></asp:ListItem>
        </asp:DropDownList>
    </div>

    <div style="margin-top:4px;margin-left:120px;">
        <asp:Button ID="btnToday" runat="server" Text="Today" style="font-size:10px;" OnClientClick="BindTodayTask()"/>
    </div>--%>

    <div>
        <dx:ASPxFormLayout runat="server" ID="formLayout" CssClass="formLayout">
            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="500" />
              <Items>
                   <dx:LayoutItem >
                        <LayoutItemNestedControlCollection>
                             <dx:LayoutItemNestedControlContainer>
        <dxwschs:ASPxScheduler ID="calendarTaskView" ClientInstanceName="calendarTaskView" runat="server" CssClass="CalenderDiv"
             OnAppointmentRowInserted="calendarTaskView_AppointmentRowInserted" OnAppointmentsInserted="calendarTaskView_AppointmentsInserted"
             AppointmentDataSourceID="taskDataSource" OnPopupMenuShowing="calendarTaskView_PopupMenuShowing" ActiveViewType="Month" >
            
            <OptionsBehavior ShowFloatingActionButton ="false" />

            <ClientSideEvents AppointmentDoubleClick="function(s, e) { OpenTaskDetail(s,e); }" MenuItemClicked="function(s, e) { OpenTaskDetail(s,e); }" />
            <Storage>
                <Appointments>
                    <Mappings AppointmentId="ID" Start="StartDate" End="DueDate" Description="Description" 
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
            </Views>
            <%--<OptionsForms AppointmentFormTemplateUrl="BudgetActual.ascx" />--%>
            <OptionsCustomization AllowInplaceEditor="None" AllowAppointmentCopy="None" AllowAppointmentDrag="None" AllowAppointmentDragBetweenResources="None" 
                AllowAppointmentMultiSelect="False" />
            <OptionsToolTips ShowSelectionToolTip="false" ShowAppointmentDragToolTip="false" ShowAppointmentToolTip="false" />
        </dxwschs:ASPxScheduler>
         </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
             </Items>
        </dx:ASPxFormLayout>

        <%--<SharePoint:SPCalendarView ID="calendarTaskView" runat="server" CssClass="CalenderDiv"></SharePoint:SPCalendarView>--%>
    </div>
    <asp:ObjectDataSource ID="taskDataSource" runat="server" DataObjectTypeName="uGovernIT.Utility.UGITTask" TypeName="uGovernIT.Manager.CustomTaskDataSource" 
        DeleteMethod="DeleteMethodHandler" InsertMethod="InsertMethodHandler" UpdateMethod="UpdateMethodHandler" 
        SelectMethod="SelectMethodHandler" OnObjectCreated="taskDataSource_ObjectCreated" OnInserted="taskDataSource_Inserted"></asp:ObjectDataSource>
    <dx:ASPxHiddenField ID="ASPxHFCalendar" runat="server" ClientInstanceName="ASPxHFCalendar"></dx:ASPxHiddenField>
</asp:Panel>