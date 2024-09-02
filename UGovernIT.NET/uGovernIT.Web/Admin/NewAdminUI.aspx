<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/master/Main.master" CodeBehind="NewAdminUI.aspx.cs" Inherits="uGovernIT.Web.NewAdminUI" %>
<%@ Register Src="~/ControlTemplates/Admin/AdministerCore.ascx" TagPrefix="uc1" TagName="AdministerCore" %>
<asp:content ID="content1" ContentPlaceHolderID="ContentPlaceHolderContainer" runat="server">
    <div id="controlLoad" runat="server"></div>
    <div id="notAuthorized" runat="server" visible="false" style="text-align:center; width:85%;font-size: 13px;padding: 13px;">
         You are not Authorized to View this Page, Please contact your Administrator.
    </div>
</asp:content>
