<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleDefaultsEdit.ascx.cs" Inherits="uGovernIT.Web.ModuleDefaultsEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer noPadding my-2">
    <div class="ms-formtable accomp-popup">
        <div class="row bs">
            <div class="col-md-6 col-sm-6 col-xs-6" id="trModule" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Module<b style="color: Red;">*</b>
                    </h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlModule" runat="server" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvddlModule" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlModule"
                            ErrorMessage="Select Module " InitialValue="0" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr1" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Field<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                     <dx:ASPxComboBox ID="cmbFieldName" runat="server" CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox"
                            DropDownStyle="DropDown" TextFormatString="{0}" Width="100%"
                            ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0" EnableSynchronization="True"
                            CallbackPageSize="10">
                            <Columns>
                            </Columns>
                        </dx:ASPxComboBox>
                        <div>
                            <asp:RequiredFieldValidator ID="rfvFieldName" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="cmbFieldName"
                                ErrorMessage="Enter Field Name" ForeColor="Red" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </div>
                </div>
            </div>
        </div>

        <div class="row bs" id="tr3" runat="server">
            <div class="ms-formlabel" style="padding-left:15px;">
                <h3 class="ms-standardheader budget_fieldLabel">Key Value<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <asp:DropDownList runat="server" ID="ddlKeyValue" AutoPostBack="true" OnSelectedIndexChanged="ddlKeyValue_SelectedIndexChanged" CssClass="itsmDropDownList aspxDropDownList">
                        <asp:ListItem Text="LoggedInUser" Value="LoggedInUser"></asp:ListItem>
                        <asp:ListItem Text="LoggedInUserLocation" Value="LoggedInUserLocation"></asp:ListItem>
                        <asp:ListItem Text="LoggedInUserDeskLocation" Value="LoggedInUserDeskLocation"></asp:ListItem>
                        <asp:ListItem Text="LoggedInUserDepartment" Value="LoggedInUserDepartment"></asp:ListItem>
                        <asp:ListItem Text="LoggedInUserManager" Value="LoggedInUserManager"></asp:ListItem>
                        <asp:ListItem Text="LoggedInUserManagerIfNotManager" Value="LoggedInUserManagerIfNotManager"></asp:ListItem>
                        <asp:ListItem Text="LoggedInUserDepartmentManager" Value="LoggedInUserDepartmentManager"></asp:ListItem>
                        <asp:ListItem Text="LoggedInUserDivisionManager" Value="LoggedInUserDivisionManager"></asp:ListItem>
                        <asp:ListItem Text="RequestorLocation" Value="RequestorLocation"></asp:ListItem>
                        <asp:ListItem Text="RequestorDeskLocation" Value="RequestorDeskLocation"></asp:ListItem>
                        <asp:ListItem Text="RequestorDepartment" Value="RequestorDepartment"></asp:ListItem>
                        <asp:ListItem Text="RequestorManager" Value="RequestorManager"></asp:ListItem>
                        <asp:ListItem Text="RequestorManagerIfNotManager" Value="RequestorManagerIfNotManager"></asp:ListItem>
                        <asp:ListItem Text="RequestorManagersManager" Value="RequestorManagersManager"></asp:ListItem>
                        <asp:ListItem Text="RequestorDepartmentManager" Value="RequestorDepartmentManager"></asp:ListItem>
                        <asp:ListItem Text="RequestorDivisionManager" Value="RequestorDivisionManager"></asp:ListItem>
                        <asp:ListItem Text="TodaysDate" Value="TodaysDate"></asp:ListItem>
                        <asp:ListItem Text="TomorrowsDate" Value="TomorrowsDate"></asp:ListItem>
                        <asp:ListItem Text="Text" Value="Text"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <asp:TextBox ID="txtKeyValue" runat="server" Visible="false" />
                </div>
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtKeyValue" ForeColor="Red" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtKeyValue" ErrorMessage="Enter key Value" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>

        <div class="row bs">
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr5" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Stage</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlModuleStep" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr9" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Custom Properties</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtCustomProperties" TextMode="MultiLine" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        
        <div class="d-flex justify-content-between align-items-center px-1" id="tr2" runat="server">
            <dx:ASPxButton ID="lnkDelete" Visible="false" runat="server" Text="Delete" ToolTip="Delete" OnClick="lnkDelete_Click" CssClass="btn-danger1">
                <ClientSideEvents Click="function(s,e){return confirm('Are you sure you want to delete?');}" />
            </dx:ASPxButton>
            <div>
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" CssClass="secondary-cancelBtn" ></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" CssClass="primary-blueBtn" OnClick="btnSave_Click"></dx:ASPxButton>
            </div>
        </div>
    </div>
</div>
