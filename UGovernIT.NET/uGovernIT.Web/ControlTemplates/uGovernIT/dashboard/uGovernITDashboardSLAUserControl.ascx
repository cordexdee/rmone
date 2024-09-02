<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uGovernITDashboardSLAUserControl.ascx.cs" Inherits="uGovernIT.Web.uGovernITDashboardSLAUserControl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .filter_stssummary {
        background-image: url(/_layouts/15/images/FILTER.gif);
        background-repeat: no-repeat;
        background-attachment: inherit;
        background-position: 10% 50%;
    }

    .clearfilter_stssummary {
        background-image: url(/_layouts/15/images/FILTEROFFDISABLED.gif);
        background-repeat: no-repeat;
        background-attachment: inherit;
        background-position: 14% 50%;
    }

    .ugit_chartfilter {
        float: left;
        padding-top: 6px;
        padding-left: 10px;
        font-size: 12px;
        font-weight: bold;
    }

    .requesttype_td {
        padding-left: 10px;
        padding-right: 10px;
        width: 50px;
    }

    .reportingperiodtd {
        padding-left: 10px;
    }

    .reportingperiod_span {
        float: left;
        width: 25%;
    }

    .userrequest_span {
        float: left;
        width: 40%;
    }

    .userrequest_innerspan {
        float: left;
        width: 100%;
    }

    .userrequest_listbox_span {
        float: left;
        width: 100%;
        padding-top: 5px;
    }

    .s4-toplinks .s4-tn a.selected {
        padding-left: 10px;
        padding-right: 10px;
    }

    .sla_table {
        /*width: 100%;*/
        border: 1px solid black;
        border-collapse: collapse !important;
    }

        .sla_table tr {
            border-collapse: collapse !important;
        }

            .sla_table tr td {
                border: 1px solid black !important;
                border-collapse: collapse !important;
            }

    .periodCustomtd {
        border: 1px solid #BABABA;
        border-bottom: none;
    }

    .startDatetd {
        border: 1px solid #BABABA;
        border-top: none;
        border-right: none;
        padding-left: 10px;
        height: 45px;
    }

    .endDatetd {
        border: 1px solid #BABABA;
        border-left: none;
        border-right: none;
    }

    .updatetd {
        border: 1px solid #BABABA;
        border-left: none;
    }

    .legenddiv {
        /*float: right;*/
        padding-top: 20px;
    }

    .legendText {
        padding-bottom: 5px;
    }

    .legendnotationtd {
        width: 20px;
        height: 10px;
    }

    .legendtexttd {
        font-size: 12px;
        padding-left: 5px;
        padding-right: 5px;
    }

    .slalegendtable {
        border-collapse: collapse;
    }

    .metricdateSectionCreatedOn {
        float: left;
        display: none;
        padding-top: 2px;
        padding-left: 5px;
    }
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
   function ShowDrillDown(title, category, module, filter, ruleId, svcId) {
       
        var paramsvpm = '&CategoryChoice=' + category + '&Module=' + module + '&filter=' + escape(filter) + '&SLAName=' + title + '&SLARuleId=' + ruleId;
        if (filter.split('~#')[0] == 'Custom') {
            var customFilterDate = escape(filter.split('~#')[2]);
            paramsvpm = paramsvpm + '&CustomDateFilter=' + customFilterDate;
        }

        if (module.toLowerCase() == 'svc')
            paramsvpm = paramsvpm + "&ForSVCMetrics=" + svcId;

        window.parent.UgitOpenPopupDialog('<%= slaMetricsDrillDownUrl %>', paramsvpm, title, '95', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function CustomFilterCheckedmetric(s, e, ctrId) {
        var val = s.GetValue();
        slametricscustompopup.Hide();
        if (val == 'Custom' && ctrId == 'reportingPeriod') {
            e.processOnServer = false;
            slametricscustompopup.Show();
        }
        else {
            slametriccallbackpanel.PerformCallback();
        }
    }

    $(function () {
        $('.metricdateSectionCreatedOn').hide();
        if (reportingPeriod.GetValue() == 'Custom') {
            $('.metricdateSectionCreatedOn').show();
        }
        HideShowSLAName();
    });

    function DropDownChange() {
        slametriccallbackpanel.PerformCallback();
    }

    var showslametricdatesection = false;
    function SetCustomDateFilterSLAMetric(s, e) {
        slametricscustompopup.Hide();
        slametriccallbackpanel.PerformCallback('slametricokcreatedon');
    }

    function HideShowSLAMetricCustomDateSection(s, e) {
        $('.metricdateSectionCreatedOn').hide();
        if (reportingPeriod.GetValue() == 'Custom') {
            $('.metricdateSectionCreatedOn').show();
        }
        HideShowSLAName();
    }

    function HideShowSLAName() {
        <%if (!ShowSLAName)
    {%>
        $('.clsShowSLARule').css('display', 'none');
        <%}%>
    }

    function OpenSLA(title, itemId) {
        var url = '<%=ruleEditUrl%>';
        url = url + "&ID=" + itemId + "&ReadOnly=" + true;
        window.parent.UgitOpenPopupDialog(url, '', title, '600px', '350px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
</script>
<dx:ASPxCallbackPanel ID="slametriccallbackpanel" runat="server" ClientInstanceName="slametriccallbackpanel" OnCallback="slametriccallbackpanel_Callback">
    <ClientSideEvents EndCallback="function(s,e){HideShowSLAMetricCustomDateSection(s,e);}" />
    <PanelCollection>
        <dx:PanelContent ID="slametricpnl" runat="server">
            <dx:ASPxPopupControl ID="slametricscustompopup" runat="server" Modal="true" PopupHorizontalAlign="LeftSides" HeaderText="Custom Filter" PopupVerticalAlign="Below" PopupElementID="spnmetricreportingPeriod" CloseAction="CloseButton" ClientInstanceName="slametricscustompopup">
                <ClientSideEvents PopUp="function(s,e){ASPxClientEdit.ClearGroup('grpClear');}" />
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <dx:ASPxPanel ID="pnlfilter" runat="server" DefaultButton="btnOk">                            
                            <PanelCollection>
                                <dx:PanelContent runat="server">
                                    <div style="float: left;">
                                        <div style="float: right;">
                                            <dx:ASPxDateEdit ID="dtStartdate" runat="server" ClientInstanceName="dtStartdate" Caption="Start Date" ValidationSettings-ValidationGroup="grpClear">
                                                <TimeSectionProperties Visible="false"></TimeSectionProperties>
                                                <ValidationSettings ValidationGroup="grpClear"></ValidationSettings>
                                            </dx:ASPxDateEdit>
                                        </div>
                                        <div style="float: right; padding-top: 5px;">
                                            <dx:ASPxDateEdit ID="dtEndDate" runat="server" ClientInstanceName="dtEndDate" Caption="End Date" Width="170px" ValidationSettings-ValidationGroup="grpClear">
                                                <TimeSectionProperties Visible="false"></TimeSectionProperties>
                                                <ValidationSettings ValidationGroup="grpClear"></ValidationSettings>
                                            </dx:ASPxDateEdit>
                                        </div>
                                    </div>
                                    <div style="float: left; width: 100%; padding-top: 5px;">
                                        <div style="float: right;">
                                            <dx:ASPxButton ID="btnOk" runat="server" Text="Apply" CssClass="primary-blueBtn" AutoPostBack="false" CommandName="CreatedOne">
                                                <ClientSideEvents Click="function(s,e){SetCustomDateFilterSLAMetric(s,e);}" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <table cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse; margin: 2px 0 50px 10px; color:black; font-size:12px;" width="100%">
                <tr id="helpTextRow" runat="server">

                    <td class="paddingleft8 helptext-cont" align="right">
                        <asp:Panel ID="helpTextContainer" runat="server">
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--Filter--%>

                        <asp:Panel ID="filterPanel" runat="server" CssClass="ms-viewheadertr" style="margin-left:-10px;" >
                            <table cellpadding="0" cellspacing="0" width="100%" class="ms-vh2-gridview">
                                <tr style="padding-bottom: 20px;">

                                    <td class="requesttype_td" runat="server" id="tdmodule">
                                        <asp:Label ID="Label1" runat="server" Text="Module: " Font-Bold="true"></asp:Label>
                                        <br />
                                        <asp:DropDownList Width="225" ID="requestType" runat="server" AutoPostBack="false" onchange="DropDownChange();" OnPreRender="RequestType_PreRender" OnLoad="RequestType_Load">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="reportingperiodtd" id="reportingPeriodTd" runat="server">
                                        <div>
                                            <div>
                                                <asp:Label ID="reportingPeriodLb" runat="server" Text="Created On: " Font-Bold="true"></asp:Label>
                                            </div>
                                            <div style="float: left;">
                                                <div style="float: left;">
                                                    <dx:ASPxComboBox ID="reportingPeriod" runat="server" OnValueChanged="ddlCreatedOn_ValueChanged" ClientInstanceName="reportingPeriod" Width="120px" AutoPostBack="false">
                                                        <ClientSideEvents ValueChanged="function(s,e){CustomFilterCheckedmetric(s,e,'reportingPeriod');}" />
                                                    </dx:ASPxComboBox>
                                                    <span id="spnmetricreportingPeriod"></span>
                                                </div>

                                                <div class="metricdateSectionCreatedOn">
                                                    <div style="float: left; padding-left: 4px;">
                                                        <dx:ASPxLabel ID="lblmetricdatecStartDate" runat="server" ClientInstanceName="lblmetricdatecStartDate"></dx:ASPxLabel>
                                                    </div>
                                                    <div style="float: left; padding-left: 2px; padding-right: 2px;"><span><b>to</b></span></div>
                                                    <div style="float: left; padding-left: 2px;">
                                                        <dx:ASPxLabel ID="lblmetricdatecEndDate" runat="server" ClientInstanceName="lblmetricdatecEndDate"></dx:ASPxLabel>
                                                    </div>
                                                    <div style="float: left; padding-left: 4px;">
                                                        <img id="imgcreatedon" runat="server" src="/Content/Images/edit-icon.png" onclick="slametricscustompopup.Show();" />
                                                        &nbsp;
                                                    </div>
                                                </div>
                                            </div>
                                            <div style="float: left; padding-left: 61px;">
                                                <div style="float: left; padding-left: 5px;"><span style="vertical-align: middle;"><b>Include Open Tickets</b></span></div>
                                                <div style="float: right; padding-left: 5px;">
                                                    <dx:ASPxCheckBox ID="chkShowOpen" runat="server" OnCheckedChanged="chkShowOpen_CheckedChanged" AutoPostBack="false" ClientInstanceName="chkShowOpen">
                                                        <ClientSideEvents ValueChanged="function(s,e){slametriccallbackpanel.PerformCallback();}" />
                                                    </dx:ASPxCheckBox>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td colspan="3"></td>

                                </tr>
                                <tr id="advanceFilterPanel" visible="false" runat="server">
                                    <td>&nbsp;</td>
                                    <td class="startDatetd">&nbsp;        
                            <span>
                                <asp:Label ID="aFilterStartDateLb" runat="server" Text="Start Date"></asp:Label>
                                <dx:ASPxCalendar ID="aFilterStartDate" runat="server"  AutoPostBack="false"></dx:ASPxCalendar>
                            </span>
                                    </td>
                                    <td width="20%" class="endDatetd">
                                        <asp:Label ID="aFilterEndDateLb" runat="server" Text="End Date"></asp:Label>
                                        <%--<SharePoint:DateTimeControl ID="aFilterEndDate" runat="server" DateOnly="true"></SharePoint:DateTimeControl>--%>
                                        <dx:ASPxCalendar ID="aFilterEndDate" runat="server" AutoPostBack="false"></dx:ASPxCalendar>
                                    </td>
                                    <td align="center" class="updatetd" width="20%">
                                        <span>&nbsp;</span><br />
                                        <asp:Button ID="filter" runat="server" Text="Update" Width="93px" OnClick="Filter_Click" CssClass="Filter_stssummary" />
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel>

                        <br />

                        <asp:Panel ID="sLAPanel" runat="server">
                            <asp:Table ID="sLATable" runat="server" CssClass="ms-listviewtable sla_table" CellPadding="4" OnLoad="SlaTable_Load" OnPreRender="SlaTable_PreRender">
                            </asp:Table>
                            <asp:Table ID="svcslaTable" runat="server" CssClass="ms-listviewtable sla_table" CellPadding="4" OnLoad="SlaTable_Load" OnPreRender="SlaTable_PreRender">
                            </asp:Table>
                        </asp:Panel>

                        <asp:Panel ID="sLALegendPanel" runat="server" CssClass="legenddiv">
                            <asp:Table CellPadding="0" CellSpacing="0" BorderWidth="1" BorderColor="black" ID="SLALegend" runat="server" CssClass="ms-listviewtable sla_table ">
                                <asp:TableRow>
                                    <asp:TableCell CssClass="legendtexttd" Font-Bold="true" Height="25">LEGEND:</asp:TableCell>
                                    <asp:TableCell BackColor="LightGreen" CssClass="legendtexttd">SLA Met</asp:TableCell>
                                    <asp:TableCell BackColor="yellow" CssClass="legendtexttd">
                                        Within
                                        <asp:Literal ID="slaXPctMeet" runat="server"></asp:Literal>

                                        % of SLA
                                    </asp:TableCell>
                                    <asp:TableCell BackColor="#ce2f2f" CssClass="legendtexttd" ForeColor="White">SLA Not Met</asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>