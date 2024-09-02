

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectClassEdit.ascx.cs" Inherits="uGovernIT.Web.ProjectClassEdit" %>

<div class="col-md-12 col-sm-12 col-xs-12 noPadding configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
               <asp:TextBox ID="txtTitle" runat="server" Width="100%" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                        ErrorMessage="Enter Title" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Project Note</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtProjectNote" TextMode="MultiLine" Width="100%" runat="server" Rows="6" cols="20" /></div>
        </div>
         <div class="row" id="tr11" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
            </div>
            <div class="ms-formbody crm-checkWrap" style="padding-left:7px;">
                <asp:CheckBox ID="chkDeleted" TextAlign="Right" runat="server" Text="(Prevent use for new item)" />
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnCancelNew" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ValidationGroup="Task" ID="btnSaveNew" Visible="true" runat="server" Text="Save"
                ToolTip="Save as Template" CssClass="primary-blueBtn"  OnClick="btnSave_Click">
            </dx:ASPxButton>
        </div>
    </div>
</div>