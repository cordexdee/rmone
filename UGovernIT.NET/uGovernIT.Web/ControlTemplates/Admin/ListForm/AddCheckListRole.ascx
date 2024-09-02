<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddCheckListRole.ascx.cs" Inherits="uGovernIT.Web.AddCheckListRole" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>


<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function SaveCheckListRole() {
        if ($("#<%=rbtnSubContractor.ClientID%>").is(':checked')) {
            if (confirm("This action will replace existing roles.  Are you sure you want to proceed?")) {
            }
            else
                return false;
        }
    }

</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
            $('.chklistRole_container').parent().addClass("cprPopup-container");
            $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
            $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
    });

</script>

<div id="tb1" class="ms-formtable accomp-popup col-md-12 col-sm-12 col-xs-12 chklistRole_container noPadding">
    <div class="row mt-2" id="trRole" runat="server">
        <div class="ms-formlabel">
            <p class="budget_fieldLabel">Role<b style="color: Red;">*</b></p>
        </div>
        <div class="ms-formbody">
            <asp:TextBox ID="txtRoleName" runat="server" CssClass="asptextbox-asp"></asp:TextBox>
            <div>
                <asp:RequiredFieldValidator ID="rfvRoleName" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtRoleName"
                    ErrorMessage="Field required." Display="Dynamic" ValidationGroup="SaveCheckListRole"></asp:RequiredFieldValidator>
            </div>
        </div>
    </div>

    <div class="row mt-2" id="trrbFieldType" runat="server">
        <div class="ms-formlabel">
            <p class="budget_fieldLabel">Role Type</p>
        </div>

        <div class="ms-formbody bC-radioBtnWrap">
            <asp:RadioButton ID="rbtnUserField" runat="server" Text="User Field" GroupName="FieldType" AutoPostBack="true" CssClass="importChk-radioBtn"/>
            <asp:RadioButton ID="rbtnTextField" runat="server" Text="Text Field" GroupName="FieldType" AutoPostBack="true" CssClass="importChk-radioBtn"/>
            <asp:RadioButton ID="rbtnContact" runat="server" Text="Contact" GroupName="FieldType" AutoPostBack="true" CssClass="importChk-radioBtn" />

            <asp:RadioButton ID="rbtnSubContractor" runat="server" Text="Import Subcontractors" GroupName="FieldType" AutoPostBack="true" 
                CssClass="importChk-radioBtn"/>
        </div>
    </div>

    <div class="row  mt-2" id="trEmail" runat="server">
        <div class="ms-formlabel">
            <p class="budget_fieldLabel">Email</p>
        </div>
        <div class="ms-formbody">
            <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="asptextbox-asp" />
            <div>
                <asp:RegularExpressionValidator ID="revEmail" ControlToValidate="txtEmailAddress" runat="server" ErrorMessage="Invalid Email." Display="Dynamic" ValidationGroup="SaveCheckListRole" ValidationExpression="^((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,;\s])*)*$"></asp:RegularExpressionValidator>
            </div>
        </div>
    </div>
    <div class="row" id="trUserField" runat="server">
        <div class="ms-formlabel">
            <p class="budget_fieldLabel mt-2">User</p>
        </div>
        <div class="ms-formbody">
            <ugit:UserValueBox ID="peUserTo" MaximumHeight="30" CssClass="userValueBox-dropDown" runat="server" isMulti="false"  />
        </div>
    </div>
    <div class="row" id="trContact" runat="server">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Contact</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <dx:ASPxComboBox ID="cmbContact" runat="server" Width="100%"  CallbackPageSize="100" ListBoxStyle-CssClass="aspxComboBox-listBox"
                OnSelectedIndexChanged="cmbContact_SelectedIndexChanged" OnItemsRequestedByFilterCondition="cmbContact_ItemsRequestedByFilterCondition" OnItemRequestedByValue="cmbContact_ItemRequestedByValue"
                EnableCallbackMode="True" DropDownStyle="DropDown" TextFormatString="{0}"  ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0"
                EnableSynchronization="True" CssClass="aspxComBox-dropDown" ItemStyle-Wrap ="True">
                <Columns>
                </Columns>
            </dx:ASPxComboBox>
        </div>
    </div>
    <div class="row">
        <div>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
        </div>
    </div>
    <div class="d-flex justify-content-between align-items-center mt-2">
        <dx:ASPxButton ID="lnkDeleteCheckListRole" Visible="false" runat="server" CssClass="btn-danger1" 
            Text="Delete" ToolTip="Delete" OnClick="lnkDeleteCheckListRole_Click">
            <ClientSideEvents Click="function(s, e){return confirm('Are you sure you want to delete?');}" />
        </dx:ASPxButton>
        <div>
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSaveCheckListRole" runat="server" CssClass="primary-blueBtn" Text="Save" ToolTip="Save"
                ValidationGroup="SaveCheckListRole" OnClick="btnSaveCheckListRole_Click">
                <ClientSideEvents Click="function(s, e){return SaveCheckListRole();}" />
            </dx:ASPxButton>
        </div>
    </div>
    <%--<div class="row clear-custom-addcheck-list" >
        <div class="col-md-12 col-sm-12 col-xs-12 deleteSaveBtn_wrap">
            <div class="activityDelete_btnWrap">
                <asp:LinkButton   OnClientClick="return confirm('Are you sure you want to delete?');">
                    <span class="activityDelete_btn">
                        <b style="font-weight: 500;">Delete</b>
                    </span>
                </asp:LinkButton>
            </div>
            <div class="CancelBtn_wrap">
                <asp:LinkButton >
                        <span class="activityCancel_btn">
                            <b style="font-weight: 500;">Cancel</b>
                        </span>
                </asp:LinkButton>
            </div>
            <div class="activitySave_btnWrap">
                <asp:LinkButton  OnClientClick="">
                        <span class="activitySave_btn">
                            <b style="font-weight: 500;">Save</b>
                        </span>
                </asp:LinkButton>
            </div>
        </div>
    </div>--%>
</div>