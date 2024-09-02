<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HolidayCalendar.ascx.cs" Inherits="uGovernIT.Web.HolidayCalendar" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v22.1.Core, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v22.1.Core.Desktop, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .fleft {
        float: left;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .pctcomplete {
        text-align: right;
    }

    .estimatedhours {
        text-align: right;
    }

    .actualhours {
        text-align: right;
    }

    .full-width {
        width: 98%;
    }

    .ms-formlabel {
        text-align: right;
    }

    .existing-section-c {
        float: left;
    }

    .new-section-c {
        float: left;
    }

    .existing-section-a {
        float: left;
        padding: 0px 5px;
    }

        .existing-section-a img {
            cursor: pointer;
        }

    .new-section-a {
        float: left;
        padding-left: 5px;
    }

        .new-section-a img {
            cursor: pointer;
        }

    .ms-standardheader {
        text-align: right;
    }

    
</style>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    table.mylist input {
        /*width: 4%;*/
        display: block;
        float: left;
    }

    table.mylist label {
        width: 96%;
        display: block;
        float: left;
        padding-top: 4px;
    }

    /*span.dxToggle
    {
        height: 13px;
        width: 25px;
        border-radius: 7px;
        background-image: grey!important;
        transition: background-color 0.2s;
        cursor: pointer;
        text-align: left;
    }*/
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function getIndex(Text) {
        if (Text == 'Bool') {
            $(".trText").hide();
            $(".trBool").show();
            $(".trDate").hide();

        }
        else if (Text == 'Text') {
            $(".trText").show();
            $(".trBool").hide();
            $(".trDate").hide();

        }
        else if (Text == 'Date') {
            $(".trText").hide();
            $(".trBool").hide();
            $(".trDate").show();

        }
    }
    function hideIndex() {

        $("#ctl00_PlaceHolderMain_ctl00_categoryDD").get(0).selectedIndex = 0;
    }

    function SaveConfiguration(s, e) {
        if (StartTimeEdit.GetDate() == null || EndTimeEdit.GetDate() == null) {
            EndTimeEdit.SetIsValid(false);
            EndTimeEdit.SetErrorText('Start Time and End Time Cannot be Empty.');
        }
        else if (StartTimeEdit.GetDate() < EndTimeEdit.GetDate()) {
            ASPxCallbackPanel1.PerformCallback();
        }
        else {
            EndTimeEdit.SetIsValid(false);
            EndTimeEdit.SetErrorText('Start Time must be smaller then End Time.');
        }
    }

    function HidePopup() {
        SchedulerSettingPopup.Hide();
        window.location.reload(true);
    }

    function ShowPopUp() {
        SchedulerSettingPopup.Show();
    }
</script>

<div>
<dx:ASPxButton ID="ASPxButton1" runat="server" Text="Calendar Settings" CssClass="primary-blueBtn" UseSubmitBehavior="false" AutoPostBack="false">
    <ClientSideEvents Click="ShowPopUp" />
</dx:ASPxButton>
</div>

<div style="padding-top:2px;">
<dx:ASPxScheduler ID="SchedulerCalendar" runat="server"  Theme="Office2003Blue" AppointmentDataSourceID="appointmentDataSource" ClientIDMode="AutoID" 
    OnAppointmentsInserted="SchedulerCalendar_AppointmentsInserted"
    OnBeforeExecuteCallbackCommand="ASPxSchedulerHolidayCalendar_BeforeExecuteCallbackCommand" 
    OnAppointmentRowInserted="SchedulerCalendar_AppointmentRowInserted"
    OnPopupMenuShowing="SchedulerCalendar_PopupMenuShowing" 
    OnAppointmentFormShowing="ASPxSchedulerHolidayCalendar_AppointmentFormShowing">
    <OptionsForms AppointmentFormTemplateUrl="/ControlTemplates/Admin/ListForm/HolidayCalendarEventView.ascx" />
    <Storage Appointments-AutoReload="true">
        <Appointments AutoReload="true" AutoRetrieveId="true">
            <Mappings AppointmentId="ID" Start="StartTime" End="EndTime" Subject="Subject" AllDay="AllDay"
                Description="Description" Label="Label" Location="Location" RecurrenceInfo="RecurrenceInfo"
                ReminderInfo="ReminderInfo" Status="Status" Type="EventType" />
            <labels>
                            <dx:AppointmentLabel Color="Window" DisplayName="None" MenuCaption="&amp;None" />
                            <dx:AppointmentLabel Color="255, 194, 190" DisplayName="Important" MenuCaption="&amp;Important" />
                            <dx:AppointmentLabel Color="168, 213, 255" DisplayName="Business" MenuCaption="&amp;Business" />
                            <dx:AppointmentLabel Color="193, 244, 156" DisplayName="Personal" MenuCaption="&amp;Personal" />
                            <dx:AppointmentLabel Color="243, 228, 199" DisplayName="Vacation" MenuCaption="&amp;Vacation" />
                            <dx:AppointmentLabel Color="244, 206, 147" DisplayName="Must Attend" MenuCaption="Must &amp;Attend" />
                            <dx:AppointmentLabel Color="199, 244, 255" DisplayName="Travel Required" MenuCaption="&amp;Travel Required" />
                            <dx:AppointmentLabel Color="207, 219, 152" DisplayName="Needs Preparation" MenuCaption="&amp;Needs Preparation" />
                            <dx:AppointmentLabel Color="224, 207, 233" DisplayName="Birthday" MenuCaption="&amp;Birthday" />
                            <dx:AppointmentLabel Color="141, 233, 223" DisplayName="Anniversary" MenuCaption="&amp;Anniversary" />
                            <dx:AppointmentLabel Color="255, 247, 165" DisplayName="Phone Call" MenuCaption="Phone &amp;Call" />
                        </labels>
        </Appointments>
    </Storage>

</dx:ASPxScheduler>
</div>

<asp:ObjectDataSource ID="appointmentDataSource" runat="server" DataObjectTypeName="uGovernIT.Utility.Appointment"
    TypeName="uGovernIT.Manager.CustomEventDataSource" DeleteMethod="DeleteMethodHandler" SelectMethod="SelectMethodHandler"
    InsertMethod="InsertMethodHandler" UpdateMethod="UpdateMethodHandler" OnObjectCreated="appointmentDataSource_ObjectCreated"
    OnInserted="appointmentDataSource_Inserted" />

<dx:ASPxPopupControl ID="SchedulerSettingPopup" ClientInstanceName="SchedulerSettingPopup" runat="server" PopupVerticalAlign="WindowCenter" Modal="true"
    PopupHorizontalAlign="WindowCenter" AllowDragging="true" CloseAction="CloseButton">
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" ClientInstanceName="ASPxCallbackPanel1" runat="server" Width="100%" OnCallback="ASPxCallbackPanel1_Callback">
                <ClientSideEvents EndCallback="HidePopup" />
                <PanelCollection>
                    <dx:PanelContent>
                        <div style="float: left; width: 98%; padding-left: 10px;">
                            <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
                                <tr id="tr3" runat="server">
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">Work Time
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                        <div style="float: left;">
                                            <dx:ASPxTimeEdit ID="StartTimeEdit" ClientInstanceName="StartTimeEdit" runat="server" Width="90px">
                                                <ValidationSettings EnableCustomValidation="true" ErrorDisplayMode="ImageWithTooltip" ErrorText="Invalid Time">
                                                </ValidationSettings>
                                            </dx:ASPxTimeEdit>
                                        </div>
                                        <div style="float: left;">
                                            &nbsp;&nbsp;To:&nbsp;&nbsp;
                                        </div>
                                        <div style="float: left;">
                                            <dx:ASPxTimeEdit ID="EndTimeEdit" ClientInstanceName="EndTimeEdit" runat="server" Width="90px">
                                                <ValidationSettings EnableCustomValidation="true" ErrorDisplayMode="ImageWithTooltip" ErrorText="Invalid Time"></ValidationSettings>
                                            </dx:ASPxTimeEdit>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="tr1" runat="server">
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">Work Week Days
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                        <dx:ASPxCheckBoxList ID="chkWeekDays" ClientInstanceName="chkWeekDays" RepeatDirection="Horizontal" runat="server" ValueType="System.String">
                                            <Items>
                                                <dx:ListEditItem Text="Monday" Value="Monday" />
                                                <dx:ListEditItem Text="Tuesday" Value="Tuesday" />
                                                <dx:ListEditItem Text="Wednesday" Value="Wednesday" />
                                                <dx:ListEditItem Text="Thursday" Value="Thursday" />
                                                <dx:ListEditItem Text="Friday" Value="Friday" />
                                                <dx:ListEditItem Text="Saturday" Value="Saturday" />
                                                <dx:ListEditItem Text="Sunday" Value="Sunday" />
                                            </Items>
                                        </dx:ASPxCheckBoxList>
                                    </td>
                                </tr>

                                <tr id="tr2" runat="server">
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">First Day Of Week
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                        <dx:ASPxRadioButtonList ID="rblFirstDayOfWeek" RepeatDirection="Horizontal" ClientInstanceName="rblFirstDayOfWeek" runat="server" ValueType="System.String">
                                            <Items>
                                                <dx:ListEditItem Text="Monday" Value="Monday" />
                                                <dx:ListEditItem Text="Tuesday" Value="Tuesday" />
                                                <dx:ListEditItem Text="Wednesday" Value="Wednesday" />
                                                <dx:ListEditItem Text="Thursday" Value="Thursday" />
                                                <dx:ListEditItem Text="Friday" Value="Friday" />
                                                <dx:ListEditItem Text="Saturday" Value="Saturday" />
                                                <dx:ListEditItem Text="Sunday" Value="Sunday" />
                                            </Items>

                                        </dx:ASPxRadioButtonList>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <div>
                                            <div style="float: left;">
                                                <%--<asp:LinkButton ID="lnkDelete" runat="server" Visible="false" Text="&nbsp;&nbsp;Delete&nbsp;&nbsp;" ToolTip="Delete" OnClick="lnkDelete_Click" CssClass="button-red" OnClientClick="return confirm('Are you sure you want to delete?');">
                        <span>
                            <b style="float: left; font-weight: normal;color:white;">
                                Delete</b>
                            <i style="float: left; position: relative; top: -2px;left:4px">
                                <img src="/Content/images/uGovernIT/delete-icon.png"  style="border:none;" title="" alt=""/>
                            </i> 
                        </span>
                        </asp:LinkButton>--%>


                                                <dx:ASPxButton ID="lnkDelete" runat="server" Visible="false" Text="Delete" ToolTip="Delete" OnClick="lnkDelete_Click">
                                                    <ClientSideEvents Click="function(s,e){return confirm('Are you sure you want to delete?');}" />
                                                    <Image Url="/content/images/delete-icon.png" Width="20px" Height="20px"></Image>
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </td>
                                    <td align="right" style="padding-top: 5px;">

                                        <div style="float: right;">

                                            <%--<asp:LinkButton ID="btnSave" runat="server" Text="&nbsp;&nbsp;Save&nbsp;&nbsp;" ToolTip="Save" ValidationGroup="serviceIntitial" OnClick="btSave_Click">
                        <span class="button-bg">
                            <b style="float: left; font-weight: normal;">
                                Save</b>
                            <i style="float: left; position: relative; top: -3px;left:2px">
                                <img src="/Content/images/uGovernIT/ButtonImages/save.png"  style="border:none;" title="" alt=""/>
                            </i> 
                        </span>
                    </asp:LinkButton>--%>
                                            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" AutoPostBack="false" >
                                                <ClientSideEvents Click="function(s, e){ SchedulerSettingPopup.Hide(); }" />
                                                <Image Url="/content/ButtonImages/cancelwhite.png"></Image>
                                            </dx:ASPxButton>
                                            <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" ValidationGroup="Validate" AutoPostBack="false" >
                                                <ClientSideEvents Click="SaveConfiguration" />
                                                <Image Url="/content/images/save.png" Width="20px" Height="20px"></Image>
                                            </dx:ASPxButton>
                                            
                                            
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>

        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>



