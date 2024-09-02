<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BusinessInitiative.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.BusinessInitiative" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function FrameLoadOnDemand(control) {
        try {
            control.contentWindow.clickUpdateSize();
            control.contentWindow.adjustControlSize();
        }
        catch (ex) {
        }
    }
</script>
<asp:Panel ID="pnlInitiatives" runat="server" Width="100%" Height="100%">
</asp:Panel>