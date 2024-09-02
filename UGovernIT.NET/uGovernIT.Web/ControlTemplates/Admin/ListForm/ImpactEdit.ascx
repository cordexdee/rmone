
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpactEdit.ascx.cs" Inherits="uGovernIT.Web.ImpactEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        /*background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;*/
        padding: 3px 6px 4px;
        /*vertical-align: top;*/
    }
    
    /*.ms-formlabel {
        text-align: right;
        width:190px;
        vertical-align:top;
    }
    .ms-standardheader {
        text-align: right;
    }*/

</style>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <div>
                <dx:ASPxLabel ID="lblError" runat="server" ForeColor="Red" Visible="false"></dx:ASPxLabel>
            </div>
        </div>
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Module<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <ugit:LookUpValueBox ID="ddlModule" runat="server" CssClass="lookupValueBox-dropown" FieldName="ModuleNameLookup"  FilterExpression="EnableModule=1" IsMandatory="true" ValidationGroup="Save"/>
            </div>
        </div>
        <div class="row" id="tr3" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtImpact" runat="server"/>
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtImpact" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtImpact" 
                        ErrorMessage="Enter Title" ForeColor="Red" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>

        </div>
        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Item Order<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtItemOrder" runat="server"/>
                <div>
                    <asp:RegularExpressionValidator ID="regextxtItemOrder" ValidationExpression="^([0-9]+)$" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtItemOrder" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="Save"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="revtxtItemOrder"  ForeColor="Red" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtItemOrder" ErrorMessage="Enter Item Order" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row" id="tr5" runat="server">
            <div class="ms-formbody accomp_inputField">
                <div class=" crm-checkWrap">
                    <asp:CheckBox ID ="chkDeleted" runat="server" Text="(Prevent use for new item)" />
                </div>
                
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" ToolTip="Cancel" OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" runat="server" Text="Save" CssClass="primary-blueBtn" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click"></dx:ASPxButton>
        </div>
    </div>
</div>
