<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomDashboardPanel.ascx.cs" Inherits="uGovernIT.Web.CustomDashboardPanel" %>

<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    /*.dashboard-panel-main, .dashboard-panel-main-mini, .dashboard-panel-main-notmove{float:left;width:100%;padding-top:5px;padding-right:5px; cursor:pointer;}
  .dashboard-panel-main-mini{padding-top:0px;padding-right:0px}
  .panel-content-header{position: relative;margin-right:5px;font-size:13px; width:100% !important;}*/
    .dashboard-panel-main a, .dashboard-panel-main a:hover, dashboard-panel-main-mini a, dashboard-panel-main-mini a:hover, dashboard-panel-main-notmove a, dashboard-panel-main-notmove a:hover {
        text-decoration: none !important;
    }

    .panel-content {
        position: relative;
    }

    .panel-content-main {
        float: left;
        width: 100%;
    }

    .dashboard-desc {
        font-weight: normal;
        float: left;
        padding-left: 4px;
        font-size: 11px;
    }

    .dashboardkpi-main {
        margin-bottom: 5px;
        display: block;
    }

    .dashboardkpi-main-min {
        margin-bottom: 1px;
        display: block;
    }
    /*.dashboardkpi-txt{font-family: 'Poppins'!important; font-size: 15px !important;font-weight:400!important;color:#232C49!important;}*/
    /*.dashboardkpi-txt:hover{color:#000;}*/
    .dashboardkpi-td {
        padding: 0px 2px;
    }

    .dashboardkpi-a {
        font-size: 12px;
    }

    .dashboardkpi-a-min {
        font-size: 10px;
    }

    /*.dxpc-contentWrapper{background-color: #F3F4F5 !important;}*/

    .drilldownback {
        width: 13px;
        height: 14px;
        float: left;
        padding-left: 2px;
        position: relative;
        top: 3px;
        left: 3px;
        z-index: 100;
    }

    .fleft {
        float: left;
    }

    .chartbreadcrumb {
        padding: 0px;
        margin: 0px;
        border: none;
        font-size: 9px;
        text-align: center;
        padding-top: 2px;
        z-index: 100;
        position: absolute;
        left: 3px;
        top: -3px;
    }
    /*.dashboardtitle111 { text-transform: uppercase !important; color:#4A90E2 !important; font-family: 'Poppins' !important; font-size:13px !important ; font-style: normal !important; font-weight:600!important ;}*/
    /*.progress-number {font-family: 'Poppins', sans-serif !important; font-size:large !important;}*/

    .dashboard-panel .dashboard-panel-main.panelDashboard:hover {
        position: relative;
        left: 0px !important;
        top: 0px !important;
    }

    .dx-pivotgrid {
        font-size: 20px;
        font-family: 'Poppins' !important;
    }
    
    .float-popup-xs.main-page-section {
        padding-left: 0px;
        padding-right: 0px;
        float: left;
        width: 100%;
        min-height: 100%;
        /* height: 100%; */
        background-color: #DFDFDF;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    $(function () {

        var panelInstanceId = "<%=panelInstanceID %>";
        var panelID = null;
        configurDashboardUrls("<%= Request.Url %>", "<%= filteredDataDetailPage %>", "<%= dasbhoardViewPage %>");
        ugitDashboardData(panelInstanceId, { ViewID: "<%= ViewID %>", PanelID: "<%=dashboard != null ? Convert.ToString(dashboard.ID) : string.Empty %>", Sidebar: "<%= Sidebar %>", Width: "<%=PanelWidth %>", Height: "<%=PanelHeight%>", viewType: "0", Type: '<%= dashboard.DashboardType == DashboardType.Chart ? "chart" : "panel" %>', LocalFilter: "<%=localFilterValue %>", DimensionFilter: "<%= DrillDownIndex%>", ExpressionFilter: "<%=ExpressionFilter %>", GlobalFilter: "<%= GlobalFilter %>", Zoom: "<%= IsZoom %>", Title: "<%= Uri.EscapeUriString(cdpTitle.Text.Replace("'","+")) %>" });
        panelID = "<%=dashboard != null ? Convert.ToString(dashboard.ID) : string.Empty %>";

        <%if (dashboard.DashboardType == DashboardType.Chart)
    {

        if (UseAjax)
        {%>
        updateCharts(panelInstanceId, 5);
        //uGITApp.queueFunction(updateCharts, panelInstanceId);
        <% }
    }
    else
    {
          %>
        updateKPIs(panelInstanceId, panelID);
        <%
    } %>

        $(".dashboard-panel-main").bind("mouseover", function () {
            $(this).css({ "position": "relative", "top": "-2px", "left": "-2px" });
        });

        $(".dashboard-panel-main").bind("mouseout", function () {
            $(this).css({ "position": "inherit", "top": "0px", "left": "0px" });
        });
        if (document.getElementById("<%=hdnChartHide.ClientID %>").value == '1') {
            document.getElementById("<%=doughnutChart.ClientID %>").style.display = "block";
        }
        else document.getElementById("<%=doughnutChart.ClientID %>").style.display = "none";

     });
</script>

<asp:Literal ID="startFromNewLine" runat="server"></asp:Literal>
<%--<div class="dashboard-panel" onmouseover="dashboardShowActions(this);" onmouseout="dashboardHideActions(this);">--%>
<div id="dashboard_MainContainer" class="dashboard-panel" runat="server">
    <div class="dashboard-panel-main homeDashboard_panel_main" id="panelDashbaordPLink" runat="server">
        <div style="height: 16px; position: relative;" class="<%=spTheme %>-panel-topleft-corner" align="left">
            <img class="drilldownback" onclick='returnFromDrillDown(this)' id="chartReturnIcon" runat="server" style="display: none; position: relative;" src="/content/images/return.png" alt="Back" />
            <span class='drilldownback-filters' id="drillDownbackFilters" runat="server" style="display: none;"></span>
        </div>
        <div class="<%=spTheme %>-middletop-rep">
            <asp:PlaceHolder ID="dashboardActionIcons" runat="server"></asp:PlaceHolder>
        </div>
        <div class="<%=spTheme %>-panel-topright-corner" align="right"></div>

        <div class="panel-content">
            <div class="row">
                <div class="<%=spTheme %>-leftside-border">
                </div>
                <div align="center">
                    <asp:Panel ID="dashboardMainContainer" runat="server" CssClass="panel-content-header dashboardpanelcontainer homeDashboard_chartContent">
                        <div>
                            <div class="row" id="dashboardDetail" runat="server">
                                <div valign="top" class="chartTitle" align="center" style="padding-bottom: 3px;">
                                    <div style="float: left; width: 100%; font-weight: bold;" id="dashboardInfoBlock" runat="server">
                                        <asp:Image ImageUrl="" ID="cdpIcon" runat="server" CssClass="fleft" Visible="false" />
                                        <b class='dashboardtitle111' id="cdpTitleContainer" runat="server">
                                            <asp:Literal ID="cdpTitle" EnableViewState="false" runat="server"></asp:Literal>
                                        </b>
                                        <asp:Literal ID="cdpDesciption" runat="server" Visible="false"></asp:Literal>
                                    </div>
                                    <div style="float: left; width: 100%; position: relative;">
                                        <div class='chartbreadcrumb'></div>
                                    </div>
                                </div>
                            </div>
                            <div class="d-flex align-items-center">
                                <div runat="server" visible="false" id="tdDounutChart" valign="top">
                                    <%--<div id="divpiechart" runat="server" width="100px" height="100px"></div>--%>
                                    <canvas id="doughnutChart" runat="server" class="homeDashboard_donutChart" width="100" height="100"></canvas>
                                </div>
                                <asp:HiddenField ID="hdnChartHide" runat="server" />

                                <div valign="top" align="center" runat="server" id="divProgressChart" class="flex-grow-1">
                                    <asp:Panel ID="panelDashboardContent" runat="server" CssClass="mt-0 d-content homeDashboard_chartContent_elementWrap">
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div class="<%=spTheme %>-rightside-border"></div>
            </div>

            <div class="row">
                <div class="<%=spTheme %>-panel-bottomleft-corner" align="left" valign="top">
                </div>
                <div class="<%=spTheme %>-middledown-rep" align="right" valign="top">
                    <asp:Panel ID="dashboardLocalContainer" runat="server"></asp:Panel>
                </div>
                <div align="right" valign="top" class="<%=spTheme %>-panel-bottomright-corner">
                </div>
            </div>
        </div>
    </div>
</div>
<asp:Panel ID="dashboardScriptPanel" runat="server"></asp:Panel>
