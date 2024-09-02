<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupScorecard.ascx.cs" Inherits="uGovernIT.Web.GroupScorecard" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v22.1.Web, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.XtraCharts.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"  Namespace="DevExpress.XtraCharts" TagPrefix="dxcharts" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .width190 {
        width: 190px;
    }

    .font-bold {
        font-weight: bold;
    }

    table.exteranlfilter {
        border-collapse: collapse;
        border: 1px solid #9da0aa;
        border-bottom: 0px;
    }
    .imagecss {
    background-repeat:no-repeat !important;
    border:0px !important;

    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function GetSummary(s, e) {
        debugger;
        if (e.additionalHitObject != null) {
            // var config = customUGITSLADConfig[s.name];
            var title = null;
            if (e.additionalHitObject.argument != undefined) {
                title = e.additionalHitObject.argument
            }
            else {
                title = e.additionalHitObject.name;
            }
                
            if (title != "") {
                var params = "Module=" + "<%= Module%>" + "&Title=" + escape(title);
                window.parent.UgitOpenPopupDialog('<%= drilDownData %>', params, title, '90', '80', 0, escape("<%= Request.Url.AbsolutePath %>"));
            }
        }
    }



    function GetAgentPerformanceTickets(s, e) {

        if (e.additionalHitObject != null) {
            // var config = customUGITSLADConfig[s.name];
            var title = e.additionalHitObject.argument;
            if (title != "") {
                var popupTitle = "Resolved By " + title;
                var params = "dashboardControl=AgentPerformance&PRPName=" + escape(title);
                window.parent.UgitOpenPopupDialog('<%= drillDownUrl %>' + "DashboardControlDrillDown", params, popupTitle, '90', '80', 0, escape("<%= Request.Url.AbsolutePath %>"));
            }
        }
    }

    function OpenTicket(vndpublicid, title,url) {
        window.parent.UgitOpenPopupDialog(url, '', title, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }


    function GetTicketCreatedByWeek(s, e) {
        if (e.additionalHitObject != null) {
            var title = e.additionalHitObject.argument;
            if (title != "") {
                var popupTitle = "Tickets: ";
                var weeks = 0;
                var weekvalue = txtTicketCreatedWeekTrendValue.GetValue();
                if (weekvalue != "") {
                    weeks = weekvalue;

                    var params = "weeks=" + weeks;
                    window.parent.UgitOpenPopupDialog('<%= drillDownUrl %>' + "TicketCreatedWeeklyTrend", params, popupTitle, '90', '80', 0, escape("<%= Request.Url.AbsolutePath %>"));

                }
            }
        }
    }
  
</script>
<!--Start Group Scorecard-->
<asp:Panel ID="pnlControls" runat="server" ScrollBars="Auto">

    <asp:Panel ID="pnlHeaderFilter" runat="server" Visible="false" CssClass="fright">
        <table class="exteranlfilter">
            <tr>
                <td class="ms-formbody" style="padding-top: 5px;">Start Date:
                </td>
                <td class="ms-formbody">
                    <dx:ASPxDateEdit ID="deStartdt" Width="100px" runat="server"></dx:ASPxDateEdit>
                </td>
                <td class="ms-formbody" style="padding-top: 5px;">End Date:
                </td>
                <td class="ms-formbody">
                    <dx:ASPxDateEdit ID="deEnddt" Width="100px" runat="server"></dx:ASPxDateEdit>
                </td>
                <td class="ms-formbody">
                    <dx:ASPxButton ID="btnSubmit" runat="server" ToolTip="Refresh" CssClass="imagecss" BackgroundImage-ImageUrl="/Content/images/refresh-icon.png" Image-AlternateText="Refresh"></dx:ASPxButton>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <%--<asp:Label ID="lblHeader" Text="Group Scorecard - Last 30 Days" CssClass="font-bold" runat="server" />--%>
    <dx:ASPxGridView ID="gvScoreBoard" runat="server" Width="100%" OnHtmlDataCellPrepared="gvScoreBoard_HtmlDataCellPrepared">
        <Columns>
            <dx:GridViewDataColumn FieldName="TicketGroup" Caption="Functional Area">
            </dx:GridViewDataColumn>
            <dx:GridViewDataTextColumn FieldName="Reopen" Caption="# Re-Open" Width="50px" PropertiesTextEdit-DisplayFormatString="{0:0.##}"></dx:GridViewDataTextColumn>
            <dx:GridViewDataColumn FieldName="SolvedTickets" Caption="# Solved Tickets" Width="100px"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn FieldName="UnsolvedTickets" Caption="# Unsolved Tickets" Width="100px"></dx:GridViewDataColumn>
        </Columns>
        <SettingsPopup>
            <HeaderFilter Height="200" />
        </SettingsPopup>
        <SettingsBehavior AllowSort="true" />
        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
        <Styles>
            <Header Font-Bold="true"></Header>
            <AlternatingRow BackColor="#F7F7F7"></AlternatingRow>
        </Styles>
    </dx:ASPxGridView>

    <%--</asp:Panel>--%>
    <!--End Group Scorecard-->

    <!--Start Ticket Flow by Group-->
    <%--<asp:Panel ID="pnlTicketFlow" runat="server">--%>

    <asp:Panel ID="pnlHeaderTicketFlow" runat="server" Visible="false" CssClass="fright">
        <table class="exteranlfilter">
            <tr>
                <td class="ms-formbody" style="padding-top: 5px;">Start Date:
                </td>
                <td class="ms-formbody">
                    <dx:ASPxDateEdit ID="deStartDateTicketFlow" Width="100px" runat="server"></dx:ASPxDateEdit>
                </td>
                <td class="ms-formbody" style="padding-top: 5px;">End Date:
                </td>
                <td class="ms-formbody">
                    <dx:ASPxDateEdit ID="deEndDateTicketFlow" Width="100px" runat="server"></dx:ASPxDateEdit>
                </td>
                <td class="ms-formbody ">
                    <dx:ASPxButton ID="btnSubmitTicketFlow" runat="server" CssClass="imagecss" ToolTip="Refresh" Image-AlternateText="Refresh" BackgroundImage-ImageUrl="/Content/images/refresh-icon.png"></dx:ASPxButton>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <%--<asp:Label ID="lblHeaderTicketFlow" Text="Ticket Flow By Group - Last Week" CssClass="font-bold" runat="server" />--%>
    <dx:ASPxGridView ID="gvTicketFlow" runat="server" Width="100%" OnHtmlDataCellPrepared="gvTicketFlow_HtmlDataCellPrepared">
        <Columns>
            <dx:GridViewDataColumn FieldName="TicketGroup" Caption="Functional Area"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn FieldName="Created" Caption="Created Last Week" Width="100px"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn FieldName="Solved" Caption="Solved Last Week" Width="100px"></dx:GridViewDataColumn>
            <dx:GridViewDataTextColumn FieldName="SolvedPct" Caption="% Solved" Width="50px" PropertiesTextEdit-DisplayFormatString="{0:0.0%}"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="BacklogImpact" Caption="Backlog Impact" Width="80px" PropertiesTextEdit-DisplayFormatString="+#;-#;0"></dx:GridViewDataTextColumn>
            <dx:GridViewDataColumn FieldName="Backlog" Caption="Backlog" Width="50px"></dx:GridViewDataColumn>
        </Columns>
        <SettingsPopup>
            <HeaderFilter Height="200" />
        </SettingsPopup>
        <SettingsBehavior AllowSort="true" />
        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
        <Styles>
            <Header Font-Bold="true"></Header>
            <AlternatingRow BackColor="#F7F7F7"></AlternatingRow>
        </Styles>
    </dx:ASPxGridView>

    <%--</asp:Panel>--%>
    <!--End Ticket Flow by Group-->

    <!--Start Weekly Rolling Average-->
    <%--<asp:Panel ID="pnlLastWeekvs12Week" runat="server">--%>

    <asp:Panel ID="pnlLastWeekvs12WeekHeader" runat="server" Visible="false" CssClass="fright">
        <table class="exteranlfilter">
            <tr>
                <td class="ms-formbody" style="padding-top: 5px;">Weekly Average:
                </td>
                <td class="ms-formbody">
                    <dx:ASPxTextBox ID="txtLastWeekvs12Week" runat="server" Width="100px"></dx:ASPxTextBox>
                </td>

                <td class="ms-formbody">
                    <%-- <asp:ImageButton ID="btRefreshItem" runat="server" CommandArgument="<%# Eval(DatabaseObjects.Columns.Id) %>"
                                OnClick="BtRefreshItem_Click" AlternateText="Refresh" ImageUrl="/_layouts/15/images/ugovernit/refresh-icon.png" />--%>
                    <dx:ASPxButton ID="btnLastWeekvs12Week" CssClass="imagecss" ToolTip="Refresh" runat="server" BackgroundImage-ImageUrl="/Content/images/refresh-icon.png" Image-AlternateText="Refresh"></dx:ASPxButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--<asp:Label ID="lblLastWeekvs12WeekHeader" Text="Last Week vs 12-Week Rolling Average" CssClass="font-bold" runat="server" />--%>
    <dx:ASPxGridView ID="gvLastWeekvs12Week" runat="server" Width="100%" OnHtmlDataCellPrepared="gvLastWeekvs12Week_HtmlDataCellPrepared" KeyFieldName="TicketAssignee">
        <Columns>
            <dx:GridViewDataColumn FieldName="TicketAssignee" Caption="Ticket Assignee"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn FieldName="Solved" Caption="# Solved Ticket" Width="100px"></dx:GridViewDataColumn>
            <dx:GridViewDataTextColumn FieldName="WeeklyAverage" Caption="Weekly Average" Width="100px" PropertiesTextEdit-DisplayFormatString="{0:0.00}"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Performance" Caption="Performance" Width="50px" PropertiesTextEdit-DisplayFormatString="{0:+#.#%;-#.#%;0.0%}"></dx:GridViewDataTextColumn>
        </Columns>
        <SettingsPopup>
            <HeaderFilter Height="200" />
        </SettingsPopup>
        <SettingsBehavior AllowSort="true" />
        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
        <Styles>
            <Header Font-Bold="true"></Header>
            <AlternatingRow BackColor="#F7F7F7"></AlternatingRow>
        </Styles>
    </dx:ASPxGridView>


    <%--</asp:Panel>--%>
    <!--End Weekly Rolling Average-->

    <!--Start Predicted Backlog-->
    <%--<asp:Panel ID="pnlPredictedBacklog" runat="server">--%>

    <asp:Panel ID="pnlPredictedBacklogHeader" runat="server" Visible="false" CssClass="fright">
        <table class="exteranlfilter">
            <tr>
                <td class="ms-formbody" style="padding-top: 5px;">Number of Weeks:</td>
                <td class="ms-formbody">
                    <dx:ASPxTextBox ID="txtPredictedBackLogFilter" runat="server" Width="100px"></dx:ASPxTextBox>
                </td>
                <td class="ms-formbody" style="display: none;">
                    <dx:ASPxTextBox ID="txtPredictedBacklogWeeks" runat="server" Width="100px"></dx:ASPxTextBox>
                </td>
                <td class="ms-formbody ">
                    <dx:ASPxButton ID="btnPredictedBacklogFilter" runat="server" CssClass="imagecss" ToolTip="Refresh" Image-AlternateText="Refresh" BackgroundImage-ImageUrl="/Content/images/refresh-icon.png"></dx:ASPxButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <dx:ASPxGridView ID="gvPredictedBacklog" runat="server" Width="100%" OnHtmlRowPrepared="gvPredictedBacklog_HtmlRowPrepared"
        OnHtmlDataCellPrepared="gvPredictedBacklog_HtmlDataCellPrepared" KeyFieldName="TicketGroup">
        <Columns>
            <dx:GridViewDataColumn FieldName="TicketGroup" Caption="Functional Area"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn FieldName="UnsolvedTicket" Caption="# Unsolved Tickets" Width="100px"></dx:GridViewDataColumn>
            <dx:GridViewDataTextColumn FieldName="SolvedWeeklyAvg" Caption="# Tickets Solved/Week[Avg]" Width="100px" PropertiesTextEdit-DisplayFormatString="{0:0}"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Backlog" Caption="Backlog" Width="70px" PropertiesTextEdit-DisplayFormatString="{0:0.0 weeks}"></dx:GridViewDataTextColumn>
        </Columns>
        <SettingsPopup>
            <HeaderFilter Height="200" />
        </SettingsPopup>
        <SettingsBehavior AllowSort="true" />
        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
        <Styles>
            <Header Font-Bold="true"></Header>
            <AlternatingRow BackColor="#F7F7F7"></AlternatingRow>
        </Styles>
    </dx:ASPxGridView>


    <%--</asp:Panel>--%>
    <!--End Predicted Backlog-->

    <!--Start Ticket created by Week Chart-->
    <%--<asp:Panel ID="pnlTicketCreatedByWeekChart" runat="server">--%>

    <asp:Panel ID="pnlTicketCreatedByWeekChartFilter" runat="server" Visible="false" CssClass="fright">
        <table class="exteranlfilter">
            <tr>
                <td class="ms-formbody" style="padding-top: 5px;">Number of Weeks:</td>
                <td class="ms-formbody">
                    <dx:ASPxTextBox ID="txtTicketCreatedWeekTrend" ClientInstanceName="txtTicketCreatedWeekTrendValue" runat="server" Width="100px"></dx:ASPxTextBox>
                </td>
                <td class="ms-formbody ">
                    <dx:ASPxButton ID="ASPxButton1" runat="server" CssClass="imagecss" ToolTip="Refresh" Image-AlternateText="Refresh" BackgroundImage-ImageUrl="/Content/images/refresh-icon.png" Height="29px"></dx:ASPxButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--<asp:Label ID="lbltcbyWeekHeader" Text="Tickets Created By Week" CssClass="font-bold" runat="server" />--%>
    <dxchartsui:WebChartControl ID="wccTicketsCreatedByWeek" runat="server" Width="1000px" CrosshairEnabled="True" Height="200px" ClientSideEvents-ObjectSelected="function(s,e){GetTicketCreatedByWeek(s,e);}"></dxchartsui:WebChartControl>
    <%--</asp:Panel>--%>
    <!--End Ticket created by Week Chart-->

    <!--Start Group UnSolved Tickets-->
    <%--<asp:Panel ID="pnlGrpUnsolvedTickets" runat="server">--%>
    <%--<asp:Label ID="Label1" Text="Groups - Unsolved Tickets" CssClass="font-bold" runat="server" />--%>
    <dx:ASPxGridView ID="gvGrpUnsolvedTickets" runat="server" Width="100%" KeyFieldName="TicketGroup" OnHtmlDataCellPrepared="gvGrpUnsolvedTickets_HtmlDataCellPrepared">
        <Columns>
            <dx:GridViewDataColumn FieldName="TicketGroup" Caption="Functional Area"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn FieldName="UnsolvedTicket" Caption="# Unsolved Tickets" Width="100px"></dx:GridViewDataColumn>
        </Columns>
        <SettingsPopup>
            <HeaderFilter Height="200" />
        </SettingsPopup>
        <SettingsBehavior AllowSort="true" />
        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
        <Styles>
            <Header Font-Bold="true"></Header>
            <AlternatingRow BackColor="#F7F7F7"></AlternatingRow>
        </Styles>
    </dx:ASPxGridView>


    <%--</asp:Panel>--%>
    <!--End Group UnSolved Tickets-->


    <!--Start Oldest UnSolved Tickets-->
    <%--<asp:Panel ID="pnlOldestUnsolved" runat="server">--%>

    <%-- <asp:Label ID="lblOldestUnsolvedHeader" Text="Oldest Unsolved Tickets" CssClass="font-bold" runat="server" />--%>
    <dx:ASPxGridView ID="gvOldestUnsolved" runat="server" Width="100%" KeyFieldName="TicketId" OnHtmlDataCellPrepared="gvOldestUnsolved_HtmlDataCellPrepared">
        <Columns>
            <dx:GridViewDataColumn FieldName="TicketId" Caption="Ticket Id" Width="100px"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn FieldName="Title" Caption="Title"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn FieldName="TicketAssignee" Caption="Ticket Assignee" Width="100px"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn FieldName="TicketStatus" Caption="Ticket Status" Width="100px"></dx:GridViewDataColumn>
            <dx:GridViewDataTextColumn FieldName="TicketAge" Caption="Ticket Age[MAX]" Width="100px" SortOrder="Descending" PropertiesTextEdit-DisplayFormatString="{0:0 days}"></dx:GridViewDataTextColumn>
        </Columns>
        <SettingsPopup>
            <HeaderFilter Height="200" />
        </SettingsPopup>
        <SettingsBehavior AllowSort="true" />
        <SettingsPager Mode="ShowPager" PageSize="10"></SettingsPager>
        <Styles>
            <Header Font-Bold="true"></Header>
            <AlternatingRow BackColor="#F7F7F7"></AlternatingRow>
        </Styles>
    </dx:ASPxGridView>


    <%--</asp:Panel>--%>
    <!--End Oldest UnSolved Tickets-->

    <!--Start Problem Report-->
    <%--<asp:Panel ID="pnlProblemReport" runat="server">--%>

    <%--<asp:Label ID="Label2" Text="Problem Report(Tags)" CssClass="font-bold" runat="server" />--%>
    <span id="showemptymessage" visible="false" runat="server" style="color:red;">No data to display</span>
    <dxchartsui:WebChartControl ID="wccProblemReport" ClientSideEvents-ObjectSelected="function(s,e){GetSummary(s,e);}" runat="server" Width="1200px" ClientInstanceName="ganttChart" PaletteName="Nature Colors" PaletteBaseColorNumber="0"  CrosshairEnabled="True" EnableClientSideAPI="true"  AppearanceNameSerializable="Default" AutoLayout="True">
        
    </dxchartsui:WebChartControl>
<%--</asp:Panel>--%>
    <!--End Problem Report-->

    <!--Start Request Report-->
    <%--<asp:Panel ID="pnlRequestReport" runat="server">--%>

    <%--<asp:Label ID="Label3" Text="Request Report(Tags)" CssClass="font-bold" runat="server" />--%>
    <dxchartsui:WebChartControl ID="wccRequestReport" runat="server" Width="1200px" CrosshairEnabled="True" Height="200px"></dxchartsui:WebChartControl>

<!--End Request Report-->

<%--Start Agent Performance--%>
   <dxchartsui:WebChartControl Visible="false" ID="wccAgentPerformance" EmptyChartText-Text="No data to display" ClientSideEvents-ObjectSelected="function(s,e){GetAgentPerformanceTickets(s,e);}" runat="server" Width="1200px" ClientInstanceName="wccAgentPerformance" PaletteBaseColorNumber="1" CrosshairEnabled="True" EnableClientSideAPI="true"  AppearanceNameSerializable="Default" AutoLayout="True">
        
    </dxchartsui:WebChartControl>
<%--End Agent Performance--%>

    </asp:Panel>