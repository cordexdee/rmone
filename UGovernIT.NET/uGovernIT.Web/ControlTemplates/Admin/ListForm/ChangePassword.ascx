
<%--<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>--%>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.ascx.cs" Inherits="uGovernIT.Web.ChangePassword" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        /*background: none repeat scroll 0 0 #E8EDED;*/
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: middle;
    }

    .ms-formlabel {
        text-align: right;
        width: 120px;
        vertical-align: middle;
    }

    .ms-standardheader {
        text-align: right;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }
          .button-red
{
        color: white;
        background: url("/Content/images/uGovernIT/firstnavbgRed.png") repeat-x scroll 0 0 transparent;
        float: left;
        margin: 1px;
        padding:4px 6px 6px;
        cursor: pointer;
 }
    .password-change-success {
        color: green;
    padding: 5px;
    background: white;
    font-weight: bold;
    padding-top: 20px;
    text-align: left;
    font-size: 20px;
    margin-left: 17px
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $('.chngPwd-popupWrap').parent().addClass('chngPwd-popupContainer')
    });
</script>
<div style="text-align: center; width: 100%;">
    <asp:Literal ID="lblError" runat="server"></asp:Literal>
    <asp:Literal runat="server" ID="ErrorMessage" />
</div>
<asp:ValidationSummary ID="vsPasswordMessage" runat="server" ValidationGroup="Save"
    ShowSummary="true" EnableClientScript="true" DisplayMode="BulletList" HeaderText="Validation Failed:" />
    <div id="MainTable" runat="server" class="chngPwd-popupWrap">
        <fieldset>
            <%--<legend>Change Password</legend>--%>
                <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                    <div id="tableChangePassword" runat="server" class=" row ms-formtable accomp-popup">
                            <div class="col-md-12 col-sm-12 col-xs-12" id="usernameTR" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">User Name</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                   <%-- <input id="inpUserName" type="text"  runat="server" readonly="readonly" value="Domain\UserName" tabindex="-1" disabled="disabled" style="width:140px" />--%>
                                    <asp:Label runat="server" ID="UserName"></asp:Label>
                                </div>
                            </div>

                            <div class="col-md-12 col-sm-12 col-xs-12" runat="server" id="oldPasswordTR">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Old Password<b style="color: Red;">*</b>
                                    </h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" Width="140px"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="rfvOldPassword" runat="server" Text="*" ControlToValidate="txtOldPassword"
                                        Display="Dynamic" ValidationGroup="Save" ErrorMessage="Old password required." ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>

                            <div class="col-md-12 col-sm-12 col-xs-12" id="trTitle" runat="server">
                                <div class="ms-formlabel"> 
                                    <h3 class="ms-standardheader budget_fieldLabel">New Password<b style="color: Red;">*</b></h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" Width="140px" ControlToCompare="txtNewPassword"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" Text="*"
                                        ControlToValidate="txtNewPassword" ValidationGroup="Save" Display="Dynamic"
                                        ErrorMessage="New password required." ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ValidationGroup="Save" Display="Dynamic" Text="*" runat="server" ID="revNewPassword"
                                        ErrorMessage="The password does not meet the password policy requirements." ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$" ControlToValidate="txtNewPassword"></asp:RegularExpressionValidator>
                                </div>
                            </div>

                            <div class="col-md-12 col-sm-12 col-xs-12" id="trPassword" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Confirm Password<b style="color: Red;">*</b>
                                    </h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" Width="140px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvConfirmPassword"
                                        runat="server" Text="*" ControlToValidate="txtConfirmPassword"
                                        ValidationGroup="Save" ErrorMessage="Confirm password required."
                                        Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="comparePasswordValidator" runat="server" ErrorMessage="Password and confirm password must be same" ControlToCompare="txtNewPassword" Display="Dynamic" ControlToValidate="txtConfirmPassword"></asp:CompareValidator>
                                </div>
                            </div>
                            <div class="row chngPwd-btnWrap" id="tr2" runat="server">
                                <div class="chngPwd-btnContainer">
                                    <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" CssClass="secondary-cancelBtn">
                                    </dx:ASPxButton>
                                    <dx:ASPxButton ID="btnChangePassword" runat="server" Text="Change Password" CssClass="primary-blueBtn" ValidationGroup="Save" 
                                        OnClick="btnChangePassword_Click">
                                    </dx:ASPxButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
        </div>
    <div>
    <div class="col-md-12 col-sm-12 col-xs-12" style="padding-left:20px;">
        <dx:ASPxLabel ID="policy" runat="server" CssClass="chngPwd-note"></dx:ASPxLabel>
    </div>
        <%--  <%=ConfigurationVariable.GetValue(ConfigConstants.PasswordPolicy)--%>
    </div>