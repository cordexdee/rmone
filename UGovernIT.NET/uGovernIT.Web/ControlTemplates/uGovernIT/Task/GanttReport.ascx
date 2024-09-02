<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GanttReport.ascx.cs" Inherits="uGovernIT.Web.GanttReport" %>


<dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="200px">
    <PanelCollection>
        <dx:PanelContent>          
            <%--<div style="float: right; padding-top: 5px;margin-bottom:-18px;z-index:1000;position:relative;">
                <img src="/Content/Images/zoom_plus.png" alt="Zoom In" id="btnZoomIn" style="width: 24px; height: 24px; padding-right: 8px;" onclick="ZoomIn()" title="Zoom In" />
                <img src="/Content/Images/zoom_minus.png" alt="Zoom Out" id="btnZoomOut" style="width: 24px; height: 24px; padding-right: 24px;" onclick="ZoomOut()" title="Zoom Out" />
            </div>--%>
            <div id="projectPlanContainer" runat="server" style="padding-top: 0px;">
                
            </div>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxPanel>
