<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddUserRoles.ascx.cs" Inherits="uGovernIT.Web.AddUserRoles" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function LnkbtnDelete(s, e) {
        if (confirm('Are you sure you want to delete?')) {
            e.processOnServer = true;
        }
        else {
            e.processOnServer = false;
        }
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding">
    <div class="ms-formtable accomp-popup">
        <div class="row" id="tr12" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Page Title</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtUserRole" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtUserRole" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtUserRole"
                        ErrorMessage="Enter User Role" Display="Dynamic" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>

        <div class="row" id="tr13" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtDescription" TextMode="MultiLine" CssClass="ms-long" runat="server" Rows="4"  />
            </div>
        </div>

        <div class="row" id="tr9" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Landing Page</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlLandingPage" runat="server" CssClass=" itsmDropDownList aspxDropDownList"></asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
        </div>
        <div class="d-flex justify-content-between align-items-center px-1">
            <dx:ASPxButton ID="lnkDelete" runat="server" Visible="false" CssClass="btn-danger1" Text="Delete" ToolTip="Delete" OnClick="lnkDelete_Click">
                 <ClientSideEvents Click="LnkbtnDelete" />
            </dx:ASPxButton>
            <div>
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" ToolTip="Cancel" OnClick="btnCancel_Click"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" CssClass="primary-blueBtn" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click"></dx:ASPxButton>
            </div>
        </div>
        
    </div>
</div>
