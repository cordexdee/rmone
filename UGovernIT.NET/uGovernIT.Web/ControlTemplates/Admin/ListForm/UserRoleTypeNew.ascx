<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserRoleTypeNew.ascx.cs" Inherits="uGovernIT.Web.UserRoleTypeNew" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function hideddlUserType(action) {
        debugger;
        var category = $("#ctl00_ctl00_MainContent_ContentPlaceHolderContainer_ctl00_ddlUserTypes_I").val();
        $(".clsusertype").hide();
        $("#<%=ddlUserTypes.ClientID%>").val('1');
         if (action == 1) {
             $("#<%=hdnRequestSubCategory.ClientID%>").val("1");
            $("#<%=txtUserType.ClientID%>").val(category);
        }
        else {
            $("#<%=hdnRequestSubCategory.ClientID%>").val("");
            $("#<%=txtUserType.ClientID%>").val("");
        }
    }
    function showddlSubCategory() {
        $(".clsusertype").show();
        $("#ctl00_PlaceHolderMain_ctl00_hdnCategory").val('0');
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12 noPadding configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Module<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlModule" CssClass="itsmDropDownList aspxDropDownList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged"></asp:DropDownList>
                <div>
                    <asp:RequiredFieldValidator ID="rfvddlModule" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlModule"
                        ErrorMessage="Select Module " InitialValue="0" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row" id="tr12" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Column Name<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxComboBox ID="cmbFieldName" runat="server" Width="100%" CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox"
                    DropDownStyle="DropDownList" TextFormatString="{0}"
                    ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0" EnableSynchronization="True"
                    CallbackPageSize="10">
                    <Columns>
                    </Columns>
                </dx:ASPxComboBox>
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtColumnName" ForeColor="Red" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="cmbFieldName" ErrorMessage="Enter Column Name" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row" id="tr7" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Role<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <div class="clsusertype row">
                    <div class="col-xs-10 noPadding">
                        <dx:ASPxComboBox ID="ddlUserTypes" DropDownStyle="DropDown" runat="server" Width="100%" CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox"
                            ValidationGroup="Save">
                        </dx:ASPxComboBox>
                        <div>
                            <asp:RequiredFieldValidator ID="rfvddlUserTypes" CssClass="error" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlUserTypes"
                                ErrorMessage="Please select a role." InitialValue="0" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-xs-2 noPadding">
                        <img alt="Edit Category" runat="server" class="editicon" id="btCategoryEdit"
                            src="/content/images/editNewIcon.png" width="16" style="cursor: pointer; position: relative; float: right;"
                            onclick="javascript:$('.divUserType').attr('style','display:block');hideddlUserType(1)" />
                        <img alt="Add Category" id="Img1" width="16" src="/content/images/plus-blue.png" style="cursor: pointer; float: right; margin-right: 10px;"
                            onclick="javascript:$('.divUserType').attr('style','display:block');hideddlUserType(0);" />
                    </div>
                </div>
                <div runat="server" id="divSubCategory" class="divUserType" style="display: none; float: left;">
                    <div class="col-xs-10 noPadding">
                        <asp:TextBox runat="server" ID="txtUserType" CssClass="txtUserType"></asp:TextBox>
                        <asp:HiddenField runat="server" ID="hdnRequestSubCategory"></asp:HiddenField>
                    </div>
                    <div class="col-xs-2 noPadding">
                        <img alt="Cancel Category" style="float: right" width="16" src="/content/images/close-blue.png" class="cancelModule"
                            onclick="javascript:$('.divUserType').attr('style','display:none');showddlSubCategory();" />
                    </div>
                </div>
                <div style="width: auto; padding: 4px 4px 0px; display: inline-block;">
                    <asp:CustomValidator ID="csvdivSubCategory" ForeColor="Red" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtUserType" ErrorMessage="Select Category" Display="Dynamic" ValidationGroup="RequestTypeGroup"></asp:CustomValidator>
                </div>
            </div>
        </div>
        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Default User</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <ugit:UserValueBox ID="ppeDefaultUser" SelectionSet="User" runat="server" CssClass="assignto-userToken" />
                <div>
                    <asp:CustomValidator ID="cvDefaultUser" runat="server" Enabled="true"></asp:CustomValidator>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr5" runat="server" style="padding-left: 3px;">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkITOnly" runat="server" Text="IT Only" TextAlign="Right" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr8" runat="server">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkManagerOnly" runat="server" Text="Manager Only" TextAlign="Right" />
                </div>
            </div>
        </div>


        <div class="row" id="tr6" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">User Groups</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <ugit:UserValueBox ID="ppeUserGroups" runat="server" SelectionSet="Group" isMulti="true" CssClass="assignto-userToken" />
                <div>
                    <asp:CustomValidator ID="cvUserGroups" runat="server" Enabled="true"></asp:CustomValidator>
                </div>
            </div>
        </div>
        <div class="row" id="tr9" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Custom Properties</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtCustomProperties" TextMode="MultiLine" CssClass="ms-long" runat="server" Rows="6" cols="20"></asp:TextBox>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton CssClass="secondary-cancelBtn" ID="btnCancel1" runat="server" Text="Cancel" ValidationGroup="Save" OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave1" runat="server" CssClass="primary-blueBtn" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click"></dx:ASPxButton>
        </div>

    </div>
</div>
