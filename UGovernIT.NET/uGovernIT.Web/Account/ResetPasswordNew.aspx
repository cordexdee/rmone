<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPasswordNew.aspx.cs" Inherits="uGovernIT.Web.ResetPasswordNew" %>

<%@ Register Src="~/ControlTemplates/Admin/ListForm/ChangePassword.ascx" TagPrefix="ugit" TagName="ChangePassword" %>
<%@ Import Namespace="uGovernIT.Utility" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="/content/themes/base/jquery-ui.css" rel="stylesheet" />
   <link href="/content/bootstrap.css" rel="stylesheet" />
   <link href="/content/bootstrap-theme.css" rel="stylesheet" />
    <script src="/scripts/jquery-3.1.1.min.js"></script>
    <script src="/scripts/jquery.cookie.js"></script>
    <script src="/scripts/jquery-ui-1.12.1.min.js"></script>
    <script src="/scripts/underscore.min.js"></script>
    <asp:PlaceHolder runat="server">
        <%= UGITUtility.LoadStyleSheet("/content/site.css") %>
        <%= UGITUtility.LoadStyleSheet("/content/ugit_graybg.css") %>
        <%= UGITUtility.LoadStyleSheet("/content/ugitcommon.css") %>
        <%= UGITUtility.LoadStyleSheet("/Content/uGITStageGraphic.css") %>
    </asp:PlaceHolder>
    
    <script src="/Scripts/jquery.tablednd.js"></script>
    <script src="/Scripts/interaction.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/ScrollToFixed/1.0.8/jquery-scrolltofixed-min.js"></script>
    <script src="/Scripts/jquery.dialogextend.js"></script>
     <asp:PlaceHolder ID="header" runat="server">
      <script type="text/javascript">
                var ugitConfig = {
                    apiBaseUrl: "<%: ConfigurationManager.AppSettings["apiBaseUrl"] %>"
                }
      </script>

      <%: Scripts.Render("~/bundles/ugitcommon") %>
      <%: Scripts.Render("~/bundles/ugitdashboard") %>
      <%: Scripts.Render("~/bundles/ugitmodule") %>
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
        <div id="divChangePassword">
            <ugit:ChangePassword runat="server" ID="ChangePassword" />
        </div>
    </form>
</body>
</html>
