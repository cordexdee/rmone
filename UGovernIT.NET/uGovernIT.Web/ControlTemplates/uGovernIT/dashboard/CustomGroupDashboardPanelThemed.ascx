
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomGroupDashboardPanelThemed.ascx.cs"
    Inherits="uGovernIT.Web.CustomGroupDashboardPanelThemed" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%--<SharePoint:ScriptLink ID="uGITDashboardScript" runat="server" Name="/_layouts/15/uGovernIT/JS/uGITDashboard.js" Language="javascript">
</SharePoint:ScriptLink>--%>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    /* top tr*/
    .cg-topleft-corner {
        background: url(/Content/images/cg-topleft-corner.png) no-repeat;
        width: 15px;
        height: 15px;
    }

    .cg-topmiddle-line {
        background: url(/Content/images/cg-topmiddle-line.png) repeat-x;
        height: 15px;
    }

    .cg-topright-corner {
        background: url(/Content/images/cg-topright-corner.png) no-repeat;
        width: 18px;
        height: 15px;
    }

    /*middle tr*/
    .cg-middleleft-line {
        background: url(/Content/images/cg-middleleft-line.png) repeat-y;
        width: 15px;
    }

    .cg-middleright-line {
        background: url(/Content/images/cg-middleright-line.png) repeat-y;
        width: 18px;
    }

    /*Bottom tr type1*/
    .cg-bottomleft-corner {
        background: url(/Content/images/cg-bottomleft-corner.png) no-repeat;
        width: 15px;
        height: 18px;
    }

    .cg-bottommiddle-line {
        background: url(/Content/images/cg-bottommiddle-line.png) repeat-x;
        width: 1px;
        height: 18px;
    }

    .cg-bottomright-corner {
        background: url(/Content/images/cg-bottomright-corner.png) no-repeat;
        width: 18px;
        height: 18px;
    }

    /*Bottom tr type2*/
    .cg-centerleft-line {
        background: url(/Content/images/cg-centerleft-line.png) no-repeat;
        width: 15px;
        height: 3px;
    }

    .cg-centermiddle-line {
        background: url(/Content/images/cg-center-line.png) repeat-x;
        width: 1px;
        height: 3px;
    }

    .cg-mcenterright-line {
        background: url(/Content/images/cg-centerright-line.png) no-repeat;
        width: 18px;
        height: 3px;
    }


    .fleft {
        float: left;
    }

    .cg-d-main {
        position: relative;
    }

    .ctrcontainer {
        float: left;
        padding-left: 10px;
    }

    .ctrcontainer-sidebar {
    }

    .cg-dashboardaction-icon {
        float: right;
        padding-left: 3px;
        position: relative;
        right: -14px;
        top: 2px;
        z-index: 1000;
    }

    .cg-drilldownback {
        width: 16px;
        height: 16px;
        float: left;
        padding-left: 2px;
        position: relative;
        top: 3px;
        left: 3px;
    }

    .cg-dashboardtopaction-type1 {
        position: relative;
        top: -12px;
        right: 6px;
    }

    .cg-dashboardtopaction-type2 {
        position: relative;
        top: 0px;
        right: 6px;
    }

    .cg-dashboardbottomaction {
        float: right;
        position: relative;
        right: -10px;
        top: -5px;
        width: 100%;
    }

    .cg-d-contentc {
        position: relative;
    }

    .cg-d-description {
        position: absolute;
        float: left;
        font-weight: bold;
        width: 100%;
        top: -12px;
         z-index:10;
    }

    .cg-d-description1 {
        position: absolute;
        float: left;
        font-weight: bold;
        width: 100%;
         z-index:10;
    }

    .cg-d-returnactionc {
        float: left;
        position: absolute;
        left: -8px;
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

    .dashboardkpi-txt {
        white-space: pre-line;
        width: 7px;
    }

    .d-content {
      
        /*padding-left: 10px;
        white-space: pre-line;
        width: 64px;

        font-weight: bold;
        word-break: break-all;
        word-wrap: break-word;*/
    }

    .dashboardkpi-txt:hover {
        color: #000;
    }

    .dashboardkpi-td {
        /*border:2px outset #F4F4F4;*/
        padding: 0px 2px;
    }

    .dashboardkpi-a {
        font-size: 12px;
    }

    .dashboardkpi-a-min {
        font-size: 10px;
    }

    .dashboardaction-icon {
        float: right;
        padding-left: 3px;
        position: relative;
        right: -14px;
        top: 2px;
    }
</style>
<%--<SharePoint:ScriptLink ID="CustomDashboardThemedScript" runat="server">
    function SetSelectedDashBoard(href, PrmThis) { alert(1);  }
</SharePoint:ScriptLink>--%>
<asp:Panel ID="ctrContainer" runat="server" CssClass="ctrcontainer">
    <table cellpadding="0" cellspacing="1" style="border-collapse: collapse;">
       <tr>

                <td style="position: relative;" valign="top">
                    <div class="dashboard-panel" id="divPanel" runat="server">
                        <asp:Literal ID="scriptCtr" runat="server"></asp:Literal>
                        <asp:Panel ID="dashboardMainContainer" runat="server" class="cg-d-main dashboardpanelcontainer ">
                            <asp:Panel ID="dashboardTopAction" runat="server" CssClass="cg-dashboardtopaction-type1">
                                <span class="cg-d-returnactionc">
                                    <img class="cg-drilldownback drilldownback" onclick='returnFromDrillDown(this)' id="chartReturnIcon"
                                        runat="server" style="display: none" src="/Content/images/return.png"
                                        alt="Back" />
                                    <span class='drilldownback-filters' style="display: none;"></span></span><span style="float: right;">
                                        <asp:PlaceHolder ID="dashboardActionIcons" runat="server"></asp:PlaceHolder>
                                    </span>
                            </asp:Panel>
                            <asp:Panel ID="dashboardDescription" runat="server" CssClass="cg-d-description">
                                <div style="width: 100%">
                                    <b class='dashboardtitle111' id="cdpTitleContainer" runat="server">
                                        <asp:Literal ID="cdpTitle" runat="server"></asp:Literal>
                                    </b>
                                    <asp:Label ID="cdpDesciption" runat="server"></asp:Label>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="dashboardContainerMain" runat="server" CssClass="cg-d-contentc">
                                <div style="border-collapse: collapse; width: 100%; height: 100%;">
                                    <div style=" padding-top: 10px; text-align:center;">
                                        
                                        <asp:Image runat="server" Width="50px" ID="imgIcon"  />
                                               
                                    </div>
                                    <div style="vertical-align: middle; text-align: center;">
                                        <asp:Panel ID="panelDashboardContent" runat="server" CssClass="d-content" Style="margin-top:5px;">
                                        </asp:Panel>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="dashboardBottomAction" runat="server" CssClass="cg-dashboardbottomaction">
                                <asp:Panel ID="dashboardLocalContainer" runat="server">
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>
                    </div>
                </td>

     </tr>
    </table>
</asp:Panel>
