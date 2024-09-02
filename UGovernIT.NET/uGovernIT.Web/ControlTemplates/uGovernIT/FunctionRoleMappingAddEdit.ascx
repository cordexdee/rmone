<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FunctionRoleMappingAddEdit.ascx.cs" Inherits="uGovernIT.Web.FunctionRoleMappingAddEdit" %>

<script>

</script>
<div class="ms-formtable accomp-popup py-2">
    <div class="row">
        <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Function</h3>
            </div>
            <div class="ms-formbody pb-2">
                <dx:ASPxComboBox ID="cmbFunction" ClientInstanceName="cmbFunction" runat="server" Width="100%" ListBoxStyle-CssClass="aspxComboBox-listBox"></dx:ASPxComboBox>
         </div>
    </div>
    <div class="row">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader">Select Role</h3>
        </div>
        <div class="ms-formbody">
            <dx:ASPxListBox runat="server" ID="lstRoles" ClientInstanceName="lstRoles" Width="100%" Height="200" TextField="Name" ValueField="Id" SelectionMode="CheckColumn">
                <FilteringSettings ShowSearchUI="true" />
            </dx:ASPxListBox>
            <%--<dx:ASPxComboBox ID="cmbRole" ClientInstanceName="cmbRole" runat="server" ListBoxStyle-CssClass="aspxComboBox-listBox"></dx:ASPxComboBox>--%>
        </div>
    </div>
    <div class="row pt-2 d-flex flex-row-reverse">
        <dx:ASPxButton ID="btnSaveMapping" runat="server" Text="Save" CssClass="primary-blueBtn" OnClick="btnSaveMapping_Click"></dx:ASPxButton>
    </div>
</div>