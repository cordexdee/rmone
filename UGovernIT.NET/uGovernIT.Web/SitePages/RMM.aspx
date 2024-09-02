<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true" CodeBehind="RMM.aspx.cs" Inherits="uGovernIT.Web.SitePages.RMM" %>
<%@ Register Src="~/ControlTemplates/RMM/CustomGroupsAndUsersInfo.ascx" TagPrefix="uc1" TagName="CustomGroupsAndUsersInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderContainer" runat="server">
<uc1:CustomGroupsAndUsersInfo runat="server" id="CustomGroupsAndUsersInfo" />
</asp:Content>
