<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DRQSystemAreaNew.ascx.cs" Inherits="uGovernIT.Web.DRQSystemAreaNew" %>

<div class="col-md-12 col-sm-12 col-xs-12 noPadding configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
               <asp:TextBox ID="txtTitle" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                        ErrorMessage="Enter Title" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row" id="tr11" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
            </div>
            <div class="ms-formbody accomp_inputField crm-checkWrap">
                <asp:CheckBox ID="chkDeleted" runat="server" Text="(Prevent use for new item)" TextAlign="Right" />
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn"  OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" ValidationGroup="Save" OnClick="btnSave_Click"></dx:ASPxButton>
        </div>
    </div>
</div>