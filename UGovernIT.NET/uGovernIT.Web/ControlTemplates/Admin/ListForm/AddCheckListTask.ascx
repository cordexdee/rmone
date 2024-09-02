    <%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddCheckListTask.ascx.cs" Inherits="uGovernIT.Web.AddCheckListTask" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<div id="tb1" class="ms-formtable accomp-popup col-md-12 col-sm-12 col-xs-12 my-2 noPadding">
    <div class="row">
        <div class="ms-formlabel">
            <p class="budget_fieldLabel">Task Name<b style="color: Red;">*</b></p>
        </div>
        <div class="ms-formbody">
            <asp:TextBox ID="txtTaskName" runat="server" CssClass="asptextbox-asp"></asp:TextBox>
            <div>
                <asp:RequiredFieldValidator ID="rfvTaskName" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTaskName"
                    ErrorMessage="Field required." Display="Dynamic" ValidationGroup="SaveCheckListTask"></asp:RequiredFieldValidator>
            </div>
        </div>
    </div>
    <div class="d-flex justify-content-between align-items-center mt-2">
        <dx:ASPxButton ID="lnkDeleteCheckListTask" Visible="false" runat="server" Text="Delete" CssClass="btn-danger1" ToolTip="Delete"  OnClick="lnkDeleteCheckListTask_Click">
            <ClientSideEvents Click="function(s, e){return confirm('Are you sure you want to delete?');}" />
        </dx:ASPxButton>
        <div>
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSaveCheckListTask" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" ValidationGroup="SaveCheckListTask" OnClick="btnSaveCheckListTask_Click"></dx:ASPxButton>
        </div>
    </div>
</div>
