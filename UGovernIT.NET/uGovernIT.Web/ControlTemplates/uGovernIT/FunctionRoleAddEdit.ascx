<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FunctionRoleAddEdit.ascx.cs" Inherits="uGovernIT.Web.FunctionRoleAddEdit" %>


<style>

</style>
<script>

</script>

<div class="ms-formtable accomp-popup py-2">
    <div class="row">
        <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Title</h3>
            </div>
            <div class="ms-formbody pb-2">
                <dx:ASPxTextBox ID="txtTitle" runat="server" ClientInstanceName="txtTitle"></dx:ASPxTextBox>
         </div>
    </div>
    <div class="row">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader">Description</h3>
        </div>
        <div class="ms-formbody">
            <dx:ASPxMemo ID="memoDescription" ClientInstanceName="memoDescription" runat="server"></dx:ASPxMemo>
        </div>
    </div>
    <div class="row pt-2">
        <dx:ASPxButton ID="btnSave" runat="server" Text="Save" CssClass="primary-blueBtn" OnClick="btnSave_Click"></dx:ASPxButton>
    </div>
</div>
