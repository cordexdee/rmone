<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyNew.ascx.cs" Inherits="uGovernIT.Web.CompanyNew" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script data-v="<%=UGITUtility.AssemblyVersion %>">

    function updateShortName(Name) {
        // Get the short name by extracting the letters of the department
        document.getElementById('<%= txtshortName.ClientID %>').value = Name;
    }
</script>



<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup ">
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel"><%= companyLabel %> Name<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtTitle" runat="server" OnKeyUp="updateShortName(this.value)" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                        ErrorMessage="Please enter title" Display="Dynamic" CssClass="error" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvtxtTitle" runat="server" ValidationGroup="Save" CssClass="error" ControlToValidate="txtTitle" Display="Dynamic" ErrorMessage="Name is already exists"
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
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnCancel" CssClass="secondary-cancelBtn" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click">
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" CssClass="primary-blueBtn" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click">
            </dx:ASPxButton>
        </div>
    </div>
</div>
