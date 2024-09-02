<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmtpSetting.ascx.cs" Inherits="uGovernIT.Web.SmtpSetting" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding">
    <div class="row">
        <div class="crm-checkWrap" style="padding-top: 5px; padding-bottom:5px;">
            <asp:CheckBox ID="chkEnableSmtpSetting" runat="server" Text="Enable Smtp" AutoPostBack="true" OnCheckedChanged="chkEnableSmtpSetting_CheckedChanged" Visible="false" />
        </div>
    </div>
    <div id="hideSmtpSetting" runat="server" class="row">
        <div class="ms-formtable accomp-popup col-md-12 col-sm-12 col-xs-12 noPadding">
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">SMTP From: <b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtSmtpServerName" CssClass="textboxwidth" ValidationGroup="ctr" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqdServerName" runat="server" ValidationGroup="ctr" ControlToValidate="txtSmtpServerName"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter Smtp server."></asp:RequiredFieldValidator>
                    </div>
                </div>
                 <div class="col-md-6 col-sm-6 col-xs-6">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">NetWork Host:<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                          <dx:ASPxTextBox ID="txtHostName" CssClass="textboxwidth  asptextBox-input" ValidationSettings-ValidationGroup="ctr" Width="100%" runat="server"></dx:ASPxTextBox>
                          <asp:RequiredFieldValidator ID="rqdHostName" runat="server" ValidationGroup="ctr" ControlToValidate="txtHostName" Display="Dynamic" ForeColor="Red" 
                            ErrorMessage="Please Enter Host Setting"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <div class="ms-formbody accomp_inputField crm-checkWrap">
                        <asp:CheckBox ID="chkdefaultCredentials" runat="server" Text="Default Credentials" TextAlign="Right" AutoPostBack="true" 
                            OnCheckedChanged="chkdefaultCredentials_CheckedChanged"/>
                    </div>
                </div>
                 <div class="col-md-6 col-sm-6 col-xs-6" id="trusername" runat="server" visible="true">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">User Name: <b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxTextBox ID="txtUserName" CssClass="textboxwidth asptextBox-input" Width="100%" runat="server" ValidationSettings-ValidationGroup="ctr"></dx:ASPxTextBox>
                        <asp:RequiredFieldValidator ID="rqdUserName" runat="server" ValidationGroup="ctr" ControlToValidate="txtUserName"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter user name."></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-6" id="trpassword" runat="server" visible="true">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Password: <b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtPassword" TextMode="Password" Width="100%" CssClass="textboxwidth asptextbox-asp" ValidationGroup="ctr" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqdPassword" runat="server" ValidationGroup="ctr" ControlToValidate="txtPassword"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter password."></asp:RequiredFieldValidator>
                    </div>
                </div>
                 <div class="col-md-6 col-sm-6 col-xs-6">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Port :<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxTextBox ID="txtPortNumber" CssClass="textboxwidth asptextBox-input" ValidationSettings-ValidationGroup="ctr" Width="100%" runat="server"></dx:ASPxTextBox>
                        <asp:RequiredFieldValidator ID="rqdPortNumber" runat="server" ValidationGroup="ctr" ControlToValidate="txtPortNumber" Display="Dynamic" ForeColor="Red" 
                            ErrorMessage="Please Enter Port Number"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <div class="ms-formbody accomp_inputField crm-checkWrap">
                       <asp:CheckBox ID="chkEnableSSl" runat="server" Text="Enable SSl" TextAlign="Right" />
                    </div>
                </div>
            </div>
        </div>
    </div>
     <div class="row addEditPopup-btnWrap">
        <div class="col-md-12 col-sm-12 col-xs-12">
            <dx:ASPxButton ID="btnClose" runat="server" Text="Close" ToolTip="Close" CssClass="secondary-cancelBtn" OnClick="btnClose_Click"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" ValidationGroup="ctr" CssClass="primary-blueBtn" AutoPostBack="true" runat="server" Text="Save" ToolTip="Save" OnClick="btnSave_Click"></dx:ASPxButton>
        </div>
    </div>
</div>

