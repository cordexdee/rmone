<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyEdit.ascx.cs" Inherits="uGovernIT.Web.CompanyEdit" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function confirmDelete(s, e) {
        $('#dialog-confirm').dialog(
            {
                modal: true,
                buttons: {
                    'Parent & Child': function () {
                        __doPostBack('DeleteChildItem', 1);
                        $(this).dialog("close");
                    },
                    'Parent Only': function () {
                        __doPostBack('DeleteChildItem', 0);
                        $(this).dialog("close");
                    },
                    'Cancel': function () {
                        $(this).dialog("close");
                    }
                }
            });
        e.processOnServer = false;
    }

    function updateShortName(Name) {
        // Get the short name by extracting the letters of the department
        document.getElementById('<%= txtshortName.ClientID %>').value = Name;
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel"><%= companyLabel %> Name<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtTitle" runat="server" OnKeyUp="updateShortName(this.value)" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                        ErrorMessage="Please enter title" Display="Dynamic" CssClass="error" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvtxtTitle" runat="server" ValidationGroup="Save" ControlToValidate="txtTitle" Display="Dynamic" CssClass="error" ErrorMessage="Name already exists"
                        OnServerValidate="cvtxtTitle_ServerValidate">
                    </asp:CustomValidator>
                </div>
            </div>
        </div>
        <div class="row" id="trShortName" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Short Name<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtshortName" runat="server" ClientIDMode="Static" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtShortName" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtshortName"
                        ErrorMessage="Enter Short Name" Display="Dynamic" CssClass="error" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtDescription" TextMode="MultiLine" CssClass="ms-long" runat="server" Rows="6" cols="20" />
            </div>
        </div>
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">GL Code</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtGLCode" runat="server" />
            </div>
        </div>
        <%--<div class="col-lg-6 col-md-6" id="tr11" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
                </div>
            <div class="ms-formbody accomp_inputField crm-checkWrap">
                <asp:CheckBox ID="chkDeleted" runat="server" Text="(Prevent use for new item)" />
            </div>
        </div>--%>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Close" ToolTip="Close" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click">
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" runat="server" CssClass="primary-blueBtn" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click">
            </dx:ASPxButton>
            <dx:ASPxButton ID="btDelete" runat="server" CssClass="primary-blueBtn" Visible="false" Text="Delete" ToolTip="Deactivate">
                <ClientSideEvents Click="confirmDelete" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="btUnDelete" runat="server" CssClass="primary-blueBtn" Visible="false" Text="Un-Delete" ToolTip="Reactivate this company" OnClick="btUnDelete_Click">
            </dx:ASPxButton>

        </div>
    </div>
    <div id="dialog-confirm" title="Confirm Delete" style="display: none">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 12px 12px 20px 0;"></span>Please select the delete action.</p>
    </div>

</div>
