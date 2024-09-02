<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomGroupDashboardPanel.ascx.cs" Inherits="uGovernIT.Web.CustomGroupDashboardPanel" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%--<script type="text/javascript" src="/Scripts/uGITDashboard.js"></script>--%>
 <asp:Panel ID="ctrContainers" runat="server" CssClass="ctrcontainer">
 <table  cellpadding="0" cellspacing="0" style="border-collapse:collapse;">
<asp:Repeater ID="rDashboardGroup" runat="server" OnItemDataBound="rDashboardGroup_ItemDataBound">
<ItemTemplate>
<tr id="topType1" runat="server">
<td  class="cg-topleft-corner" align="left">
</td>
<td class="cg-topmiddle-line" align="right">
</td>
<td class="cg-topright-corner" align="right">
</td>
</tr>
<tr>
<td class="cg-middleleft-line"> </td>
<td style="background:#F8F8F8;position:relative;" valign="top" >
<div class="dashboard-panel"  onmouseover="dashboardShowActions(this);" onmouseout="dashboardHideActions(this);">
<asp:Literal ID="scriptCtr" runat="server"></asp:Literal>
<asp:Panel ID="dashboardMainContainer" runat="server" class="cg-d-main dashboardpanelcontainer ">
<asp:Panel ID="dashboardTopAction" runat="server" CssClass="cg-dashboardtopaction-type1">
 <span class="cg-d-returnactionc">
  <img class="cg-drilldownback drilldownback" onclick='returnFromDrillDown(this)' id="chartReturnIcon" runat="server" style="display:none" src="/content/images/return.png" alt="Back"/>
  <span class='drilldownback-filters' style="display:none;"></span>
 </span>
 <span style="float:right;">
   <asp:PlaceHolder ID="dashboardActionIcons" runat="server"></asp:PlaceHolder>
 </span>
</asp:Panel>
<asp:Panel ID="dashboardDescription" runat="server" CssClass="cg-d-description">
<table style="width:100%;">
<tbody>
<tr>
<td valign="top" align="center">
  <div>
        <asp:Image ImageUrl="" ID="cdpIcon" runat="server" CssClass="fleft"  Visible="false" />
         <b class='dashboardtitle111' ID="cdpTitleContainer" runat="server">
            <asp:Literal ID="cdpTitle" runat="server"></asp:Literal>
          </b>
        <asp:Label ID="cdpDesciption" runat="server"></asp:Label>
    </div>
    <div style="float:left;width:100%;position:relative;"><div class='chartbreadcrumb'></div></div>
 </td>
 </tr>
 </tbody>
 </table>
</asp:Panel>
<asp:Panel id="dashboardContainerMain" runat="server" CssClass="cg-d-contentc">
  <table cellspacing="0" cellpadding="0" style="border-collapse: collapse; width: 100%; height: 100%;">
            <tbody>
                <tr>
                    <td valign="middle" align="center" id="tdMiddleContent" runat="server">
                        
                        <asp:Panel ID="panelDashboardContent" runat="server" CssClass="d-content" >
                           
                        </asp:Panel>
                    </td>
                </tr>
            </tbody>
        </table>
</asp:Panel>
<asp:Panel ID="dashboardBottomAction" runat="server"  CssClass="cg-dashboardbottomaction">
  <asp:Panel ID="dashboardLocalContainer" runat="server" CssClass="dashboardbottom-super"></asp:Panel>
</asp:Panel>
</asp:Panel>
</div>
</td>
<td class="cg-middleright-line"></td>
</tr>
<tr id="bottomType1" runat="server">
<td class="cg-bottomleft-corner" align="left" valign="top"></td>
<td class="cg-bottommiddle-line" align="right" valign="top"></td>
<td align="right" valign="top" class="cg-bottomright-corner"></td>
</tr>
<tr id="bottomType2" runat="server" visible="false">
<td class="cg-centerleft-line"></td>
<td class="cg-centermiddle-line" align="right" valign="top"></td>
<td class="cg-mcenterright-line"></td>
</tr>
</ItemTemplate>
</asp:Repeater>

</table>
</asp:Panel>