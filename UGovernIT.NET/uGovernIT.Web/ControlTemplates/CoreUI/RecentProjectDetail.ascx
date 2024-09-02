<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecentProjectDetail.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.CoreUI.RecentProjectDetail" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function ShowProjectDetail() {
        var path = '<%=TicketPath%>';
        var params = "&TicketID=" + '<%=TicketId%>';
        window.parent.UgitOpenPopupDialog(path, params, '<%=TicketId%>' + ': ' + '<%=Title%>', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
    }
</script>
<div id="parentContainer">
    <div class="d-flex mb-1">
        <div class="panelImgDiv mr-3">
            <dx:ASPxImage id="imgProjectIcon" ClientInstanceName="imgProjectIcon" runat="server">
                <ClientSideEvents Click="ShowProjectDetail" />
            </dx:ASPxImage>
        </div>
        <div class="titleState">
            <dx:ASPxLabel ID="lblProjectTitle" runat="server"></dx:ASPxLabel>
            <div class="secStat mt-1">
                <dx:ASPxLabel ID="lblStatus" runat="server" Visible="false"></dx:ASPxLabel>
                <dx:ASPxLabel ID="lblSector" runat="server"></dx:ASPxLabel>
            </div>
        </div>
        <div class="panelImgDiv pmImage ml-3">
            <dx:ASPxImage ID="imgProjectManager" ClientInstanceName="imgProjectManager" runat="server">

            </dx:ASPxImage>
        </div>
    </div>
    <div class="dRange">
        <dx:ASPxLabel ID="lblDateRange" runat="server"></dx:ASPxLabel>
    </div>
</div>