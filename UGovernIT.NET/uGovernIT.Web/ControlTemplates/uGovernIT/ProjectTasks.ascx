<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectTasks.ascx.cs" Inherits="uGovernIT.Web.ProjectTasks" %>
<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%--<SharePoint:ScriptLink ID="ScriptLink2" runat="server" Name="/_layouts/15/uGovernIT/JS/jquery.cookie.js" Language="javascript">
</SharePoint:ScriptLink>
<SharePoint:ScriptLink ID="ScriptLink3" runat="server" Name="/_layouts/15/uGovernIT/JS/jquery.hotkeys.js" Language="javascript">
</SharePoint:ScriptLink>--%>
<script src="../../Scripts/jquery.hotkeys.js?v=<%=UGITUtility.AssemblyVersion %>"></script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $('.aspTreeView-ctrl').parents().addClass('aspTreeView-ctrlParent');
        $('.aspTreeView-ctrlParent').find('legend').addClass('admin-legendLabel');
    });
</script>

<div class="col-md-12 col-sm-12 col-xs-12">
    <asp:Panel ID="pTreeView" runat="server" GroupingText="Project Tasks">
        <dx:ASPxTreeView ID="treeView" runat="server" CssClass="aspTreeView-ctrl" AllowSelectNode="false" TextField="Title" NavigateUrlField="HelpUrl" ImageUrlField="NodeTypeImage">
            <images>
                <NodeImage Width="16px" Height="16px"  ></NodeImage>
            </images>
            <styles>
                <NodeImage Paddings-PaddingTop="0px"></NodeImage>
            </styles>
        </dx:ASPxTreeView>
        <asp:Label Visible="false" ID="lblMessage" runat="server" Text="No tasks exist"></asp:Label>
    </asp:Panel>
</div>
