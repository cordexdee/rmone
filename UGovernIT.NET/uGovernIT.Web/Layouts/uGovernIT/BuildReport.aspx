<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BuildReport.aspx.cs" Inherits="uGovernIT.BuildReport" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/content/themes/base/jquery-ui.css" rel="stylesheet" />
    <link href="/content/bootstrap.css" rel="stylesheet" />
    <link href="/content/bootstrap-theme.css" rel="stylesheet" />
<%-- <link href="/content/ugitthemable.css" rel="stylesheet" />--%>
    <script src="/scripts/jquery-3.1.1.min.js"></script>
    <script src="/scripts/jquery.cookie.js"></script>
    <script src="/scripts/jquery-ui-1.12.1.min.js"></script>
    <script src="/scripts/underscore.min.js"></script>
    <script src="/Scripts/jquery.tablednd.js"></script>
    <script src="/Scripts/interaction.js"></script>
    <asp:PlaceHolder runat="server">
        <%= UGITUtility.LoadStyleSheet("/content/site.css") %>
        <%= UGITUtility.LoadStyleSheet("/content/UGIT_GrayBG.css") %>
        <%= UGITUtility.LoadStyleSheet("/content/uGITCommon.css") %>
        <%= UGITUtility.LoadStyleSheet("/content/UgitNewUI.css") %>
        <%= UGITUtility.LoadStyleSheet("/Content/uGITStageGraphic.css") %>
        <%= UGITUtility.LoadScript("/Scripts/uGITCommon.js") %>
    </asp:PlaceHolder>
    <title></title>

    <style>
        .top_container {
            padding: 15px;
            float:left;
            width:100%;
            background:#ffffff;
        }

        select {
            background-color: white;
            border: 1px solid #9da0aa;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
       <asp:Panel ID="DelegatePanel" CssClass="top_container" runat="server"></asp:Panel>
    </form>
</body>
</html>
