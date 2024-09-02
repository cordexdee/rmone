<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotLoginPassword.aspx.cs" Inherits="uGovernIT.Web.Account.ForgotLoginPassword" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password</title>
      <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" />

    <!-- jQuery library -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>

    <!-- Latest compiled JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
    <asp:PlaceHolder runat="server">
        <%= UGITUtility.LoadStyleSheet("../Content/UGITNewUI.css") %>
    </asp:PlaceHolder>
</head>
<body>
<%if(DefaultTenant.Equals("uGovernIT")) { %>
    <asp:PlaceHolder runat="server">
        <%= UGITUtility.LoadStyleSheet("/Content/LoginPage.css") %>
    </asp:PlaceHolder>
    <style data-v="<%=UGITUtility.AssemblyVersion %>">
        .forgotPwd-container {
            margin: auto;
            top: 0px;
            margin-top: 30% !important;
            left: 0px;
            right: 0px;
            background: #fff;
            height: 320px !important;
            box-shadow: 0px 0px 6px rgba(0, 0, 0, 0.3);
            padding-left: 35px;
            margin-left: 20% !important;
            width: 60% !important;
            border-radius: 25px !important;
        }

        #accountIdTextBox tr td input[type="text"].dxeEditArea_UGITGreenDevEx.dxeEditAreaSys.dxh2, #userNameTextBox tr td input[type="text"].dxeEditArea_UGITGreenDevEx.dxeEditAreaSys.dxh2 {
            margin-left: 23px;
            height: 22px;
        }

        .fpbtnContainer {
            clear: both;
            position: absolute;
            bottom: 15px;
            width: 84.5%;
        }

        .forgotPwd-backBtn a {
            font-family: 'Roboto', sans-serif !important;
            font-size: 14px !important;
            color: gray !important;
            text-decoration: underline;
        }

        .forgotPwd-backBtn {
            display: inline-block;
            float: left;
            border: 1px solid #fff;
            color: #4A6EE2;
            border-radius: 4px;
            padding: 8px 0px;
            font-family: 'Roboto', sans-serif;
            font-size: 14px;
            margin-right: 4%;
        }

        .forgotPwd-title {
            text-align: center;
        }

        .dxh0 {
            height: 22px;
        }

        #accountIdTextBox, #userNameTextBox {
            width: 100%;
            background-size: 14px;
            padding: 5px 9px;
            border-radius: 4px;
            font-family: 'Roboto', sans-serif;
            background-position: 10px center !important;
        }

        @media only screen and (max-height: 800px) {
            .forgotPwd-container {
                margin: auto;
                top: 0px;
                margin-top: 20% !important;
                left: 0px;
                right: 0px;
                background: #fff;
                height: 330px !important;
                box-shadow: 0px 0px 6px rgba(0, 0, 0, 0.3);
                padding-left: 35px;
                margin-left: 20% !important;
                width: 60% !important;
                border-radius: 25px !important;
            }
        }

        .margin-right-2 {
            margin-right: 2rem !important;
        }
        .forgetPwd-successMsg {
            font-size: 18px !important;
        }
    </style>
<%}%>
    <form id="form1" runat="server">
    <div class="row">
        <div class="col-md-6 imageClass-1">
        <div><asp:Image ID="logoImage" src="/content/images/RMONE/rm-one-logo.png" class="logo-img header-logo" runat="server" Visible="false"/></div>
            <div class="col-md-12 col-sm-12 col-xs-12 forgotPwd-container">
            <h3 class="forgotPwd-title">Forgot Password ?</h3>
            <div class="row">
               <%-- <div class="fPlabel">
                    <label>Account ID</label>
                </div>--%>
                <div class="FPinputField">
                <dx:ASPxTextBox NullTextDisplayMode="UnfocusedAndFocused" ClientInstanceName="accountIdTextBox"  NullTextStyle-CssClass="accId-placeHolder"
                    NullText="Account Id" ID="accountIdTextBox" runat="server" RootStyle-CssClass="forgotPwd-accId">
                    <NullTextStyle>
                        <BackgroundImage ImageUrl="../Content/Images/userGray.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center"/>
                    </NullTextStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField IsRequired="true" ErrorText="Account Id is required." />
                    </ValidationSettings>
<%--                    <ClientSideEvents GotFocus="function(s,e){ lblMessage.SetText(''); }" />--%>
                </dx:ASPxTextBox>
                   <dx:ASPxLabel ID="lblMesgForAccount" runat="server" ClientInstanceName="lblMesgForAccount" Text="" Style="padding-top: 5px;" ForeColor="Red">
                    </dx:ASPxLabel>

                </div>
            </div>
            <div class="row">
                <%--<div class="fPlabel">
                    <label>User Name / Email</label>
                </div>--%>
                <div class="FPinputField">
                <dx:ASPxTextBox NullTextDisplayMode="UnfocusedAndFocused" ClientInstanceName="userNameTextBox" NullText="User Name / Email" ID="userNameTextBox"
                    runat="server" RootStyle-CssClass="forgotPwd-accId" NullTextStyle-CssClass="accId-placeHolder">
                    <NullTextStyle>
                        <BackgroundImage ImageUrl="../Content/Images/emailGray.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center"/>
                    </NullTextStyle>
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField IsRequired="true" ErrorText="User Name / Email is required." />
                    </ValidationSettings>
                    <ClientSideEvents GotFocus="function(s,e){ lblMessage.SetText(''); }" />
                </dx:ASPxTextBox>
                  <dx:ASPxLabel ID="lblMesgForUserName" runat="server" ClientInstanceName="lblMesgForUserName" Text="" Style="padding-top: 5px;" ForeColor="Red">
                    </dx:ASPxLabel>

                </div>
            </div>
            <div class="row" style="padding-right:20px;">
                <dx:ASPxLabel ID="lblMessage" runat="server" ClientInstanceName="lblMessage" Text="" CssClass="forgetPwd-successMsg">
                    </dx:ASPxLabel>
            </div>
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                    <dx:LayoutItemNestedControlContainer>
                        <dx:ASPxButton ID="submitButton" runat="server" CssClass="forgotPwd-submitBtn primary-LoginBtn margin-right-2" Text="Submit"  onclick="ForgetPasswordAction" AutoPostBack="false" />

                         <div class="forgotPwd-backBtn">
                            <dx:ASPxHyperLink ID="backToLogin" runat="server" NavigateUrl="~/Account/Login.aspx" Text="Back" />
                        </div>
                        
                    </dx:LayoutItemNestedControlContainer>
                </div>
            </div>
        </div>
        </div>
        <div class="col-md-6 nopadding">
            <asp:Image ID="leftImage" class="imageClass" src="/Content/Images/RMONE/LoginImage-1.png" alt="" runat="server" Visible="false" />
        </div>
    </div>
    </form>
</body>
</html>