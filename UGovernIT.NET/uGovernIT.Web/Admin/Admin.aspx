<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="uGovernIT.Web.Admin" %>
<%@ Register Src="~/ControlTemplates/Admin/AdminCatalog.ascx" TagPrefix="uc1" TagName="AdminCatalog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderContainer" runat="server">
    <div id="controlload" runat="server"></div>
    <div id="spNotAuthorizedUser" runat="server" visible="false" style="text-align:center; width:85%;font-size: 13px;padding: 13px;">
        You are not Authorized to View this Page, Please contact your Administrator.
    </div>
</asp:Content>
