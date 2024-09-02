<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateVariableCtrl.ascx.cs" Inherits="uGovernIT.Web.DateVariableCtrl" %>
<table>
    <tr>
        <td> 
            <asp:DropDownList ID="ddlCount" runat="server">
            </asp:DropDownList>
        <asp:DropDownList ID="ddlTimeInterval" runat="server">
            <asp:ListItem Value="Days" Text="Days"></asp:ListItem>
            <asp:ListItem Value="Weeks" Text="Weeks"></asp:ListItem>
            </asp:DropDownList>
             <asp:DropDownList ID="ddlIntervalType" runat="server">
                <asp:ListItem Text="Before" Value="-"></asp:ListItem>
                <asp:ListItem Text="After" Value="+"></asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="lblDueDateText" runat="server" Text="Today"></asp:Label>
        </td>
    </tr>
</table>
