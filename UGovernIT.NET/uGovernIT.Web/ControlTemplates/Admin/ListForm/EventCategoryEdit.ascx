<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventCategoryEdit.ascx.cs" Inherits="uGovernIT.Web.EventCategoryEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<div class="col-md-12 col-sm-12 col-xs-12">
    <div class="ms-formtable accomp-popup">
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Meeting Type<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtTitle" runat="server" Width="100%" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                        ErrorMessage="Enter meeting type" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
                <div>
                     <asp:CustomValidator ID="cvtxtTitle" runat="server" ControlToValidate="txtTitle" OnServerValidate="cvtxtTitle_ServerValidate"
                        ErrorMessage="Meeting type allready exist" Display="Dynamic" ValidationGroup="Save" Enabled="true"></asp:CustomValidator>
                </div>
            </div>
        </div>
        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Color</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxColorEdit ID="ASPxColorEdit1" runat="server" Width="100%" CssClass="aspxColorEdit-dropDwon"></dx:ASPxColorEdit>
            </div>
        </div>
        <div class="row" id="tr3" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Item Order</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtItemOrder" runat="server" Width="100%"></asp:TextBox>
                 <div>
                    <asp:CompareValidator ID="cv" Enabled="true" runat="server" ControlToValidate="txtItemOrder" Type="Integer" Display="Dynamic" ValidationGroup="Save"
                        Operator="DataTypeCheck" ErrorMessage="Value must be an integer!"></asp:CompareValidator>
                </div>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnDelete" runat="server" CssClass="secondary-cancelBtn" Text="Delete" ToolTip="Delete" 
                OnClick="btnDelete_Click"></dx:ASPxButton>
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" ToolTip="Cancel" OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" runat="server" ToolTip="Save" Text="Save" ValidationGroup="Save" CssClass="primary-blueBtn"
                OnClick="btnSave_Click"></dx:ASPxButton>
        </div>
    </div>
</div>
