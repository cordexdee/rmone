<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uGovernITDashboardChartsUserControl.ascx.cs" Inherits="uGovernIT.Web.uGovernITDashboardChartsUserControl" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .dxpnlControl_UGITNavyBlueDevEx{
        font-size:12px;
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">

    if (typeof (ctrarr) == "undefined")
        var ctrarr = [];
    ctrarr.push({ clienid: "<%=this.ClientID%>" })

    function SetHeightChart(increaseBy) {
        if (ctrarr.length >=1) {
            var currentId = ctrarr[0].clienid;
            $('#' + currentId + '_divchartcontainer').height(increaseBy);
            ctrarr.splice(0, 1);
        }
    }

</script>
<div id="divchartcontainer" runat="server" style="float: left; width: 99%; position: relative; color:black;">
    <table cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse; /*margin-top: 2px*/"
        width="100%">
        <tr id="helpTextRow" runat="server">
            <td class="paddingleft8 helptext-cont" align="right">
                <asp:Panel ID="helpTextContainer" runat="server" CssClass="help-container">
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="/*padding-left: 6px*/">
                <asp:PlaceHolder ID="dashboardView" runat="server"></asp:PlaceHolder>

            </td>
        </tr>
    </table>

</div>

    <div id="spNotAuthorizedUser" runat="server" visible="false" style="text-align:center; width:85%;font-size: 13px;padding: 13px;">
                      You are not Authorized to View this DashBoard, Please contact your Administrator.
    </div>

 