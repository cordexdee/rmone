<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectComplexityAddEdit.ascx.cs" Inherits="uGovernIT.Web.ProjectComplexityAddEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<div style="float:right; width: 98%; padding-left: 10px;">
    <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
        <tr>
            <td colspan="2">
                <dx:ASPxLabel ID="lblError" runat="server" ForeColor="Red" Visible="false"></dx:ASPxLabel>
            </td>
        </tr>       
        <tr id="tr3" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Project Complexity<b style="color: Red;">*</b>
                </h3>
            </td>
            <td class="ms-formbody">                
                <dx:ASPxComboBox ID="cmbProjectComplexity" runat="server"  Width="300px"></dx:ASPxComboBox>
                <div>                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1"  ForeColor="Red" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="cmbProjectComplexity" ErrorMessage="Project Complexity" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </td>

        </tr>
        <tr id="tr1" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Min. Approx. Contract Value<b style="color: Red;">*</b>
                </h3>
            </td>
            <td class="ms-formbody">
                <dx:ASPxSpinEdit ID="seMinValue" runat="server" Number="0" NumberType="Float" DecimalPlaces="0" Width="300px" Height="30px" />
                <div>                    
                    <asp:RequiredFieldValidator ID="revMinValue"  ForeColor="Red" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="seMinValue" ErrorMessage="Minimum Value required" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>

         <tr id="tr6" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Max. Approx. Contract  Value<b style="color: Red;">*</b>
                </h3>
            </td>
            <td class="ms-formbody">
                <dx:ASPxSpinEdit ID="seMaxValue" runat="server" Number="0" DecimalPlaces="0" NumberType="Float"  Width="300px" Height="30px" />                
                <div>                    
                    <asp:RequiredFieldValidator ID="revMaxValue"  ForeColor="Red" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="seMaxValue" ErrorMessage="Maximum Value required" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td><br /></td>
        </tr>
        <tr id="tr5" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Delete
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:CheckBox ID ="chkDeleted" runat="server" Text="(Prevent use for new item)" />
            </td>
        </tr>
        <tr id="tr4" runat="server">
            <td colspan="2" class="ms-formlabel">                
            </td>
        </tr>
    </table>
    <table  width="100%">
        <tr id="tr2" runat="server">
            <td  align="right" style="padding-top: 5px;padding-right: 24px;">
                <div style="float: right;">
                    <dx:ASPxButton ID="btnSave" CssClass="primary-blueBtn" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" 
                        OnClick="btnSave_Click" Image-Url="/Content/ButtonImages/save.png">
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btnCancel" CssClass="secondary-cancelBtn" runat="server" Text="Cancel"
                        ToolTip="Cancel" OnClick="btnCancel_Click" Image-Url="/Content/images/cancelwhite.png">
                    </dx:ASPxButton>
                </div>
            </td>
        </tr>
    </table>
</div>