<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeadCriteriaAddEdit.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.LeadCriteriaAddEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <dx:ASPxLabel ID="lblError" runat="server" ForeColor="Red" Visible="false"></dx:ASPxLabel>
        </div>       
        <div class="row" id="tr3" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Priority<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <%--<asp:TextBox ID="txtImpact" runat="server"  Width="300px"  Height="30px"/>--%>
                <dx:ASPxComboBox ID="cmbCriteria" runat="server" CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox" Width="100%"></dx:ASPxComboBox>
                <div>                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1"  ForeColor="Red" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="cmbCriteria" ErrorMessage="Priority required" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>

        </div>
        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Minimum Value<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxSpinEdit ID="seMinValue" runat="server" CssClass="aspxSpinEdit-dropDown" Number="00.00" NumberType="Float" DecimalPlaces="2"
                    Width="100%" Height="30px" />
                <div>                    
                    <asp:RequiredFieldValidator ID="revMinValue"  ForeColor="Red" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="seMinValue" ErrorMessage="Minimum Value required" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>

         <div class="row" id="tr6" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Maximum Value<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxSpinEdit ID="seMaxValue" runat="server" Number="00.00" DecimalPlaces="2" NumberType="Float" Width="100%" 
                    Height="30px" CssClass="aspxSpinEdit-dropDown" />                
                <div>                    
                    <asp:RequiredFieldValidator ID="revMaxValue"  ForeColor="Red" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="seMaxValue" ErrorMessage="Maximum Value required" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>

        <div class="row" id="tr5" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
            </div>
            <div class="ms-formbody accomp_inputField crm-checkWrap">
                <asp:CheckBox ID ="chkDeleted" runat="server" Text="(Prevent use for new item)" TextAlign="Left" />
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" 
                OnClick="btnSave_Click" CssClass="primary-blueBtn">
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn"
                ToolTip="Cancel" OnClick="btnCancel_Click">
            </dx:ASPxButton>
        </div>
    </div>
</div>