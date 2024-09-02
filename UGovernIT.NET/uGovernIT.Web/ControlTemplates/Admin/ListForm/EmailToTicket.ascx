<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailToTicket.ascx.cs" Inherits="uGovernIT.Web.EmailToTicket" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<% if (hidecontrol)
    { %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .textboxwidth {
        width: 220px;
    }

    .tablemargin {
        margin-left: 75px;
    }

    .saveclose {
        float: right;
        margin-right: 8px;
    }
</style>
<% } %>

<% else
    { %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .textboxwidth {
        width: 135px;
    }

    .tablemargin {
        margin-left: 1px;
    }

    .saveclose {
        float: right;
    }
</style>
<% } %>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row">
        <div class="crm-checkWrap" style="padding: 10px 5px 10px;">
            <asp:CheckBox ID="chkEnableEmailToTicket" runat="server" Text="Enable Email-To-Ticket" AutoPostBack="true" OnCheckedChanged="chkEnableEmailToTicket_CheckedChanged" />
        </div>
        <div class="row" id="hideserverconfiguration" runat="server">
            <div class="ms-formtable accomp-popup col-md-12 col-sm-12 col-xs-12 noPadding">
                <div class="row">
                    <% if (!hidecontrol)
                        { %>
                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                        <% } %>

                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">IMAP Server<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtServerName" CssClass="textboxwidth" runat="server" ValidationGroup="ctr"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqdServerName" runat="server" ValidationGroup="ctr" ControlToValidate="txtServerName"
                                Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter IMAP server."></asp:RequiredFieldValidator>
                        </div>
                        <% if (!hidecontrol)
                            { %>
                    </div>
                    <% } %>
                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding" runat="server" id="dvtenantid" visible="false">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Tenant Id<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtTenantId" CssClass="textboxwidth" runat="server" ValidationGroup="ctr"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="ctr" ControlToValidate="txtTenantId"
                                Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter tenantId."></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <% if (!hidecontrol)
                        { %>
                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                        <% } %>
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">User Name<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtUserName" CssClass="textboxwidth" runat="server" ValidationGroup="ctr"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqdUserName" runat="server" ValidationGroup="ctr" ControlToValidate="txtUserName"
                                Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter user name."></asp:RequiredFieldValidator>
                        </div>
                   <% if (!hidecontrol)
                            { %>
                    </div>
                    <% } %>

                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding" runat="server" id="dvclientid" visible="false">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Client Id<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtClientId" CssClass="textboxwidth" runat="server" ValidationGroup="ctr"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="ctr" ControlToValidate="txtClientId"
                                Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter client Id."></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="row">
                     <% if (!hidecontrol)
                        { %>
                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding" style="display:none">
                        <% } %>
                        <div class="ms-formlabel" runat="server" id="lblpassword">
                            <h3 class="ms-standardheader budget_fieldLabel">Password<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtPassword" TextMode="Password" CssClass="textboxwidth" ValidationGroup="ctr" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqdPassword" runat="server" ValidationGroup="ctr" ControlToValidate="txtPassword"
                                Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter password."></asp:RequiredFieldValidator>
                        </div>
                    <% if (!hidecontrol)
                            { %>
                    </div>
                    <% } %>

                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding" runat="server" id="dvsecretid" visible="false">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Secret Id<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtSecretId" CssClass="textboxwidth" runat="server" ValidationGroup="ctr"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="ctr" ControlToValidate="txtSecretId"
                                Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter secret Id."></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="row crm-checkWrap" style="padding-left: 5px;">
                    <asp:CheckBox ID="chkIsDelete" Text="Delete Processed Email" runat="server" TextAlign="Right" />
                </div>
                <div class="row">
                    <div class="addEditPopup-btnWrap">
                        <dx:ASPxButton ID="btnClose" CssClass="secondary-cancelBtn" runat="server" Text="Close" OnClick="btnClose_Click"></dx:ASPxButton>
                        <dx:ASPxButton ID="btnSave" runat="server" ValidationGroup="ctr" Text="Save" OnClick="btnSave_Click" CssClass="primary-blueBtn"></dx:ASPxButton>
                        <dx:ASPxButton ID="btnEnableEmailToTicketSave" CssClass="primary-blueBtn" runat="server" Text="Save" OnClick="btnEnableEmailToTicketSave_Click"></dx:ASPxButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
