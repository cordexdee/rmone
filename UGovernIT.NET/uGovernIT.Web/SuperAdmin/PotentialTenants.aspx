<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/master/Main.master" CodeBehind="PotentialTenants.aspx.cs" Inherits="uGovernIT.Web.PotentialTenants" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderContainer" runat="server">
    <div id="controlload" runat="server"></div>
    <div id="unAuthorizedUser" runat="server" visible="false" style="text-align:center; width:85%;font-size: 13px;padding: 13px;">
        You are not Authorized to View this Page, Please contact your Administrator.
    </div>
    
</asp:Content>
