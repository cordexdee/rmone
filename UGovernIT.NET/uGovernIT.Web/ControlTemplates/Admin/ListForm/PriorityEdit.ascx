
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PriorityEdit.ascx.cs" Inherits="uGovernIT.Web.PriorityEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <div>
                <dx:ASPxLabel ID="lblError" runat="server" ForeColor="Red" Visible="false"></dx:ASPxLabel>
            </div>
        </div>
        <div class="row" id="trTitle" runat="server">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Module<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <ugit:LookUpValueBox ID="ddlModule" CssClass="lookupValueBox-dropown"  runat="server" FieldName="ModuleNameLookup"  FilterExpression="EnableModule=1" IsMandatory="true"  ValidationGroup="Save"/>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr3" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtImpact" runat="server" />
                    <div>
                        <asp:RequiredFieldValidator ID="rfvtxtImpact" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtImpact" 
                            ErrorMessage="Enter Title" ForeColor="Red" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr1" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Item Order<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtItemOrder" runat="server" />
                    <div>
                        <asp:RegularExpressionValidator ID="regextxtItemOrder" ForeColor="Red" ValidationExpression="^([0-9]+)$" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtItemOrder" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="Save"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="revtxtItemOrder" ForeColor="Red" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtItemOrder" ErrorMessage="Enter Item Order" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr6" runat="server">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID ="chkVIP" runat="server" Text="VIP/Critical" />
                </div>
            </div>
             <div class="col-md-6 col-sm-6 col-xs-6" id="tr8" runat="server">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                   <asp:CheckBox ID ="chkAllowPlainText" runat="server" Text="Notify (e-mail)" />
                </div>
            </div>
        </div>
         <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr7" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Notify (e-mail) </h3>
                </div>
                <div class="ms-formbody accomp_inputField"> 
                  <asp:TextBox ID="txtNotifyTo" runat="server"  />
                </div>
            </div>
             <div class="col-md-6 col-sm-6 col-xs-6" id="tr9" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Background Color</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <dx:ASPxColorEdit runat="server" ID="ColorEditHeaderFontColor" CssClass="aspxColorEdit-dropDwon" Width="100%" ClearButton-DisplayMode="Never" EnableCustomColors="true">    
                    </dx:ASPxColorEdit>
                
                </div>
            </div>
         </div>
         <div class="row">
              <div class="col-md-6 col-sm-6 col-xs-6" id="tr5" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
                </div>
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID ="chkDeleted" runat="server" Text="(Prevent use for new item)" />
                </div>
            </div>
         </div>
        <div class="row addEditPopup-btnWrap">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" CssClass=" primary-blueBtn" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click"></dx:ASPxButton>
            </div>
        </div>
    </div>
</div>
