<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomSideBarLink.ascx.cs" Inherits="uGovernIT.Web.CustomSideBarLink" %>
<asp:Panel ID="previewPanel" runat="server"  HorizontalAlign="Center">
<table id="linkTable" runat="server" style="table-layout:fixed; width:100%;">
<tr>
    <td>
        <asp:HyperLink CssClass="itemdiv"   runat="server"  ID="hypTitleUrl" >
            <asp:Image ID="imgTile" runat="server"  />
            <asp:Label CssClass="charttitlespan" ID="lbTile" runat="server"></asp:Label>
        </asp:HyperLink>
    </td>
</tr>
</table>
</asp:Panel>
  