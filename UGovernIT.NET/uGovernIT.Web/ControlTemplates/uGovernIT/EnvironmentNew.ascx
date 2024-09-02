
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnvironmentNew.ascx.cs" Inherits="uGovernIT.Web.EnvironmentNew" %>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtTitle" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                        ErrorMessage="Enter Title" ForeColor="Red" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel">
                 <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                 <asp:TextBox ID="txtDescription" TextMode="MultiLine" CssClass="ms-long" runat="server" Rows="6" cols="20" /></td>
            </div>
        </div>
        <div class="row" id="tr11" runat="server">
            <div class="ms-formlabel">
                 <h3 class="ms-standardheader">Delete</h3>
            </div>
            <div class="ms-formbody accomp_inputField  crm-checkWrap">
                <asp:CheckBox ID="chkDeleted" runat="server" Text="(Prevent use for new item)" />
            </div>
        </div>
        <div class="row addEditPopup-btnWrap" id="tr2" runat="server">
            <dx:ASPxButton ID="btnCancel" runat="server" Text="&nbsp;&nbsp;Cancel&nbsp;&nbsp;" CssClass="secondary-cancelBtn" ToolTip="Cancel" OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" runat="server" Text="&nbsp;&nbsp;Save&nbsp;&nbsp;" CssClass="primary-blueBtn"  ToolTip="Save" ValidationGroup="Save" 
                OnClick="btnSave_Click"></dx:ASPxButton>
        </div>
    </div>
</div>
