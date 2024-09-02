<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePasswordNew.aspx.cs" Inherits="uGovernIT.Web.ChangePasswordNew" %>
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

    <script src="/Scripts/jquery.tablednd.js"></script>
    <script src="/Scripts/interaction.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/ScrollToFixed/1.0.8/jquery-scrolltofixed-min.js"></script>
    <script src="/Scripts/jquery.dialogextend.js"></script>
    <asp:PlaceHolder runat="server">
        <%= UGITUtility.LoadStyleSheet("/content/site.css") %>
        <%= UGITUtility.LoadStyleSheet("/content/ugit_graybg.css") %>
        <%= UGITUtility.LoadStyleSheet("/content/ugitcommon.css") %>      
        <%= UGITUtility.LoadStyleSheet("/Content/uGITStageGraphic.css") %>
        <%= UGITUtility.LoadStyleSheet("/Content/UGITNewUI.css") %>
    </asp:PlaceHolder>

     <asp:PlaceHolder ID="header" runat="server">
      <script type="text/javascript">
          var ugitConfig = {
              siteUrl: "<%: ConfigurationManager.AppSettings["SiteUrl"] %>"
          }
          //$(document).ready(function () {
          //    $("#ChangePassword_btnCancel").click(function () {
          //        window.location.href = ugitConfig.siteUrl + "/Account/Login.aspx";
          //    })
          //})
      </script>

      <%: Scripts.Render("~/bundles/ugitcommon") %>
      <%: Scripts.Render("~/bundles/ugitdashboard") %>
      <%: Scripts.Render("~/bundles/ugitmodule") %>
    </asp:PlaceHolder>
    
</head>
<body>
    <%if (DefaultTenant.Equals("uGovernIT")) {  %>
    <asp:PlaceHolder runat="server">
        <%= UGITUtility.LoadStyleSheet("/Content/LoginPage.css") %>
    </asp:PlaceHolder>
    <style>
        fieldset {
    border: 1px solid #fff;
    padding: 0.35em 0.625em 0.75em;
    margin: inherit;
}
        .chngPwd-popupContainer {
        width: 60%;
    margin-left: 20%;
    margin-top: 5%;
    border-radius: 33px;
    border: 20px solid white;
    border-bottom: 140px solid white;
        }
        .primary-blueBtn
        {
            float: right;
    background: #6BA538 !important;
    color: #FFF;
    border-radius: 10px !important;
    font-family: 'Roboto', sans-serif !important;
    font-size: 16px !important;
    font-weight: 500;
    padding: 5px 8px !important;
    width: auto !important;
        }
        .primary-blueBtn .dxb {
    background: #6BA538;
    color: #FFF;
    border: 1px solid #6BA538 !important;
    border-radius: 4px;
    padding: 5px 8px 5px 8px !important;
    font-size: 14px;
    font-family: 'Roboto', sans-serif;
    font-weight: 500;
}
        .secondary-cancelBtn {
    background: none;
    border: 0px solid #4A6EE2;
    border-radius: 4px;
    margin-left: 8px;
    margin-top: 8px;
    padding: 0;
    text-decoration:underline;
    float:left;
}
        .secondary-cancelBtn .dxb{
                padding: 4px 10px 3px !important;
    color: grey;
    font-size: 14px;
    font-family: 'Roboto', sans-serif;
    font-weight: 500;
        }
        .chngPwd-btnContainer {
        width:100%;
        }
        #ChangePassword_vsPasswordMessage {
            padding-left: 29px;
    color: red;
    padding-top: 20px;
        }
    </style>
    <%} else { %>
    <style>
        .chngPwd-popupContainer {
        position: fixed;
    top: 25%;
    left: 30%;
    width: 40%;
    padding: 19px;
        }
    </style>
    <%} %>
     <div class="row nopadding">
        <div class="col-md-6 nopadding">
            <div class="imageClass-1">
        <div><asp:Image ID="logoImage" src="/content/images/RMONE/rm-one-logo.png" class="logo-img header-logo" runat="server" Visible="false"/></div>
        <form id="form1" runat="server">
        <div id="divChangePassword">
            <ugit:ChangePassword runat="server" ID="ChangePassword" />
        </div>
    </form>
        </div>
            </div>
        <div class="col-md-6 nopadding">
            <asp:Image ID="leftImage" class="imageClass" src="/Content/Images/RMONE/LoginImage-1.png" alt="" runat="server" Visible="false" />
        </div>
    </div>
</body>
</html>

