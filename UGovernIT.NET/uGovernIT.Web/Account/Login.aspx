<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/master/AnonymousMaster.Master" CodeBehind="Login.aspx.cs" Inherits="uGovernIT.Web.Login" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>
<%@ Import Namespace="uGovernIT.Utility" %>



<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <asp:PlaceHolder runat="server">
        <%= UGITUtility.LoadScript("../../Scripts/uGITCommon.js") %>
    </asp:PlaceHolder>
    <script data-v="<%=UGITUtility.AssemblyVersion %>">
        $(document).ready(function () {
            set_cookie("screenWidth", $(window).width() - 55);
            //55px is length of LeftMenu Navigation.
        });

    </script>
    <style data-v="<%=UGITUtility.AssemblyVersion %>">
        .forgetPwdLink {
            padding-top: 8px;
            font-family: 'Poppins', sans-serif;
        }

            .forgetPwdLink a.dxeHyperlink_UGITGreenDevEx {
                font-family: 'Poppins', sans-serif;
                font-size: 14px;
                font-weight: 500;
            }
    </style>

    <%if(DefaultTenant.Equals("uGovernIT")) { %>
    <asp:PlaceHolder runat="server">
        <%= UGITUtility.LoadStyleSheet("/Content/LoginPage.css") %>
    </asp:PlaceHolder>
    <%} %>
    <div class="row nopadding">
        <div class="col-md-6 nopadding">
            <div class="imageClass-1">
                <div>
                    <asp:Image ID="logoImage" src="/content/images/RMONE/rm-one-logo.png" class="logo-img header-logo" runat="server" Visible="false" />
                </div>
                <dx:aspxpopupcontrol id="loginContainer" clientinstancename="loginContainer" autoupdateposition="true" closeaction="None" runat="server" showheader="true" headertext="Login"
                    showclosebutton="false" popuphorizontalalign="WindowCenter" popupverticalalign="WindowCenter" showonpageload="true" minwidth="400">
                    <headerstyle cssclass="loginContainer-header" />
                    <settingsadaptivity mode="OnWindowInnerWidth" verticalalign="WindowCenter" />
                    <contentcollection>
                        <dx:popupcontrolcontentcontrol>
                            <dx:aspxformlayout requiredmark="" id="loginForm" runat="server">
                                <styles>
                                    <layoutitem cssclass="layout-item"></layoutitem>
                                </styles>
                                <settingsadaptivity adaptivitymode="SingleColumnWindowLimit" switchtosinglecolumnatwindowinnerwidth="420" />
                                <items>
                                    <dx:layoutitem fieldname="AccountId" caption="Account Id" showcaption="False">
                                        <layoutitemnestedcontrolcollection>
                                            <dx:layoutitemnestedcontrolcontainer>
                                                <label>Account:<span style="color: red;">*</span></label>
                                                <dx:aspxtextbox nulltextdisplaymode="UnfocusedAndFocused" clientinstancename="accountIdTextBox" id="accountIdTextBox"
                                                    runat="server" cssclass="login-ASPXtextBox">
                                                    <validationsettings errordisplaymode="ImageWithTooltip">
                                                        <requiredfield isrequired="true" errortext="Account is required." />
                                                    </validationsettings>
                                                    <clientsideevents gotfocus="function(s,e){ lblMessage.SetText(''); }" />
                                                </dx:aspxtextbox>
                                            </dx:layoutitemnestedcontrolcontainer>
                                        </layoutitemnestedcontrolcollection>
                                    </dx:layoutitem>
                                    <dx:layoutitem fieldname="UserName" caption="User Name / Email" showcaption="False">
                                        <layoutitemnestedcontrolcollection>
                                            <dx:layoutitemnestedcontrolcontainer>
                                                <label>User Name:<span style="color: red;">*</span></label>
                                                <dx:aspxtextbox nulltextdisplaymode="UnfocusedAndFocused" clientinstancename="userNameTextBox"
                                                    id="userNameTextBox" runat="server" cssclass="login-ASPXtextBox">
                                                    <validationsettings errordisplaymode="ImageWithTooltip">
                                                        <requiredfield isrequired="true" errortext="User Name / Email is required." />
                                                    </validationsettings>
                                                    <clientsideevents gotfocus="function(s,e){ lblMessage.SetText(''); }"
                                                        keydown="function(s, e){ if(e.htmlEvent.keyCode == 13){ submitButton.DoClick(); } }" />
                                                </dx:aspxtextbox>
                                            </dx:layoutitemnestedcontrolcontainer>
                                        </layoutitemnestedcontrolcollection>
                                    </dx:layoutitem>
                                    <dx:layoutitem fieldname="Password" showcaption="False">
                                        <layoutitemnestedcontrolcollection>
                                            <dx:layoutitemnestedcontrolcontainer>
                                                <label>Password:<span style="color: red;">*</span></label>
                                                <dx:aspxtextbox nulltextdisplaymode="UnfocusedAndFocused" clientinstancename="passwordTextBox" id="passwordTextBox"
                                                    password="true" runat="server" cssclass="login-ASPXtextBox">
                                                    <validationsettings errordisplaymode="ImageWithTooltip">
                                                        <requiredfield isrequired="true" errortext="Minimum 6 digit password is required." />
                                                    </validationsettings>
                                                    <clientsideevents gotfocus="function(s,e){ lblMessage.SetText(''); }" keydown="function(s, e){ if(e.htmlEvent.keyCode == 13){
                                         submitButton.DoClick();
                                         } 
                                         }" />
                                                </dx:aspxtextbox>
                                            </dx:layoutitemnestedcontrolcontainer>
                                        </layoutitemnestedcontrolcollection>
                                    </dx:layoutitem>
                                    <dx:layoutitem fieldname="Password" showcaption="False" cssclass="btnLayoutItem">
                                        <layoutitemnestedcontrolcollection>
                                            <dx:layoutitemnestedcontrolcontainer>
                                                <dx:aspxbutton id="submitButton" usesubmitbehavior="true" clientinstancename="submitButton" runat="server" onclick="btnLogin_Click" cssclass="primary-LoginBtn" text="Login" />
                                                <dx:aspxlabel id="lblMessage" runat="server" clientinstancename="lblMessage" text="" style="padding-top: 5px;" forecolor="Red">
                                                </dx:aspxlabel>
                                                <div class="forgetPwdLink">
                                                    <dx:aspxhyperlink id="ForgetPassword" runat="server" navigateurl="~/Account/ForgotLoginPassword.aspx" text="Forgot Password ?" />
                                                </div>
                                            </dx:layoutitemnestedcontrolcontainer>
                                        </layoutitemnestedcontrolcollection>
                                    </dx:layoutitem>
                                </items>
                            </dx:aspxformlayout>

                        </dx:popupcontrolcontentcontrol>
                    </contentcollection>
                </dx:aspxpopupcontrol>
            </div>
        </div>
        <div class="col-md-6 nopadding">
            <asp:Image ID="leftImage" class="imageClass" src="/Content/Images/RMONE/LoginImage-1.png" alt="" runat="server" Visible="false" />
        </div>
    </div>
</asp:Content>
