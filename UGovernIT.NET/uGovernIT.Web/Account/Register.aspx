<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/master/AnonymousMaster.Master" CodeBehind="Register.aspx.cs" Inherits="uGovernIT.Web.Register" %>

<asp:Content ID="ClientArea" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="align-content: center; width:100%">
        <div class="accountHeader">
            <h2>Create a New Account</h2>
            <p>Use the form below to create a new account.</p>
            <p style="color: red">
                <asp:Literal runat="server" ID="ErrorMessage" />
            </p>
        </div>
        <dx:ASPxTextBox ID="tbUserName" runat="server" Width="200px" Caption="User Name">
            <CaptionSettings Position="Top" />
            <ValidationSettings ValidationGroup="RegisterUserValidationGroup" Display="Dynamic" ErrorTextPosition="Bottom" ErrorDisplayMode="Text">
                <RequiredField ErrorText="User Name is required." IsRequired="true" />
            </ValidationSettings>
        </dx:ASPxTextBox>
        <dx:ASPxTextBox ID="tbEmail" runat="server" Width="200px" Caption="E-mail">
            <CaptionSettings Position="Top" />
            <ValidationSettings ValidationGroup="RegisterUserValidationGroup" Display="Dynamic" ErrorTextPosition="Bottom" ErrorDisplayMode="Text">
                <RequiredField ErrorText="E-mail is required." IsRequired="true" />
                <RegularExpression ErrorText="Email validation failed" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
            </ValidationSettings>
        </dx:ASPxTextBox>
        <dx:ASPxTextBox ID="tbPassword" ClientInstanceName="Password" Password="true" runat="server" Width="200px" Caption="Password">
            <CaptionSettings Position="Top" />
            <ValidationSettings ValidationGroup="RegisterUserValidationGroup" Display="Dynamic" ErrorTextPosition="Bottom" ErrorDisplayMode="Text">
                <RequiredField ErrorText="Password is required." IsRequired="true" />
            </ValidationSettings>
        </dx:ASPxTextBox>
        <dx:ASPxTextBox ID="tbConfirmPassword" Password="true" runat="server" Width="200px" Caption="Confirm password">
            <CaptionSettings Position="Top" />
            <ValidationSettings ValidationGroup="RegisterUserValidationGroup" Display="Dynamic" ErrorTextPosition="Bottom" ErrorDisplayMode="Text">
                <RequiredField ErrorText="Confirm Password is required." IsRequired="true" />
            </ValidationSettings>
            <ClientSideEvents Validation="function(s, e) {
            var originalPasswd = Password.GetText();
            var currentPasswd = s.GetText();
            e.isValid = (originalPasswd  == currentPasswd );
            e.errorText = 'The Password and Confirmation Password must match.';
        }" />
        </dx:ASPxTextBox>
        <asp:DropDownList runat="server" ID="ddlUserType" CssClass="form-control">
            <asp:ListItem>Windows</asp:ListItem>
            <asp:ListItem>Forms</asp:ListItem>
        </asp:DropDownList>



        <br />
        <dx:ASPxButton ID="btnCreateUser" runat="server" Text="Create User" ValidationGroup="RegisterUserValidationGroup"
            OnClick="btnCreateUser_Click">
        </dx:ASPxButton>
    </div>
</asp:Content>
