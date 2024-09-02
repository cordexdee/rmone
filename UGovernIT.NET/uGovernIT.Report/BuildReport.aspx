<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BuildReport.aspx.cs" Inherits="uGovernIT.Report.BuildReport" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/content/themes/base/jquery-ui.css" rel="stylesheet" />
    <link href="/content/bootstrap.css" rel="stylesheet" />
    <link href="/content/bootstrap-theme.css" rel="stylesheet" />
    <link href="/content/site.css" rel="stylesheet" type="text/css" />
    <link href="/content/UGIT_GrayBG.css" rel="stylesheet" />
<%-- <link href="/content/ugitthemable.css" rel="stylesheet" />--%>
    <link href="/content/uGITCommon.css" rel="stylesheet" />
    <link href="/Content/UgitNewUI.css" rel="stylesheet" />
    <script src="/scripts/jquery-3.1.1.min.js"></script>
    <script src="/scripts/jquery.cookie.js"></script>
    <script src="/scripts/jquery-ui-1.12.1.min.js"></script>
    <script src="/scripts/underscore.min.js"></script>
    <link href="/Content/uGITStageGraphic.css" rel="stylesheet" />
    <script src="/Scripts/jquery.tablednd.js"></script>
    <script src="/Scripts/interaction.js"></script>
    <script src="/Scripts/uGITCommon.js"></script>
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
