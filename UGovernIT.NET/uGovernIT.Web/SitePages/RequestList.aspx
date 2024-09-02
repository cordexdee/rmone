<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true" CodeBehind="RequestList.aspx.cs" Inherits="uGovernIT.Web.Modules.RequestList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderContainer" runat="server">
    <ugit:CustomFilteredTickets runat="server" ID="CustomFilteredTickets" />
</asp:Content>
