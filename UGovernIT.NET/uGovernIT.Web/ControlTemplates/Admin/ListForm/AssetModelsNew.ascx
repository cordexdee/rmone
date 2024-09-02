
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetModelsNew.ascx.cs" Inherits="uGovernIT.Web.AssetModelsNew" %>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding">
    <div class="ms-formtable accomp-popup">
         <div class="row">
             <div class="col-md-12 col-sm-12 col-xs-12">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Model Name<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtModelName" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvtxtModelName" ErrorMessage="Enter Model Name" ControlToValidate="txtModelName" runat="server" ValidationGroup="Save" ForeColor="Red" />
                </div>
             </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6" >
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Vendor</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList runat="server" ID="ddlVendor" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                    <asp:RequiredFieldValidator ErrorMessage="Select Vendor" ControlToValidate="ddlVendor" runat="server" ValidationGroup="Save"  ForeColor="Red" />
                </div>
            </div>
             <div class="col-md-6 col-sm-6 col-xs-6" >
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Model Description</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6" >
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Budget Item</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList runat="server" ID="ddlBudgetItem" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                </div>
            </div> 
             <div class="col-md-6 col-sm-6 col-xs-6" id="tr11" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
                </div>
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkDeleted" runat="server" Text="(Prevent use for new item)" />
                </div>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn"  OnClick="btnCancel_Click"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" ValidationGroup="Save" OnClick="btnSave_Click"></dx:ASPxButton>
            </div>
        </div>
    </div>
</div>