<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FunctionalAreaEdit.ascx.cs" Inherits="uGovernIT.Web.FunctionalAreaEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register TagPrefix="ugit" Namespace="uGovernIT.Web" Assembly="uGovernIT.Web" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
    });
</script>
<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer noPadding">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6" id="trTitle" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Functional Area<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtTitle" runat="server"/>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvtxtTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                            ErrorMessage="Enter Title" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel"><%=departmentLabel %></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <ugit:LookUpValueBox ID="ddlDepartment" runat="server" FieldName="DepartmentLookup" IsMandatory="true" ValidationGroup="Save" Width="100%" 
                        CssClass="lookupValueBox-dropown functionalArea-lookUpValueBox"></ugit:LookUpValueBox>
                </div>
            </div>
        </div>
       <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Owner<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <ugit:UserValueBox ID="ppeOwner" runat="server" isMulti="true" IsMandatory="true" ValidationGroup="Save" 
                        CssClass="functionalArea-userValueBox userValueBox-dropDown"></ugit:UserValueBox>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Functional Area Description</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server" />
                </div>
            </div>
       </div>
        <div class="row">
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
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" ToolTip="Cancel" OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" ValidationGroup="Save" OnClick="btnSave_Click"></dx:ASPxButton>
        </div>
    </div>
</div>
