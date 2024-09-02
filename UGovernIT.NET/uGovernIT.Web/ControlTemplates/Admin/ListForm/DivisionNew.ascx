<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DivisionNew.ascx.cs" Inherits="uGovernIT.Web.DivisionNew" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script data-v="<%=UGITUtility.AssemblyVersion %>">

    function updateShortName(Name) {
        // Get the short name by extracting the letters of the department
        document.getElementById('<%= txtshortName.ClientID %>').value = Name;
    }
</script>


<div class="col-md-12 col-sm-12 col-xs-12 noPadding configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6" id="trTitle" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Name<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                   <asp:TextBox ID="txtTitle" runat="server" OnKeyUp="updateShortName(this.value)"/>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvtxtTitle"  CssClass="error" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                            ErrorMessage="Enter Division" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvtxtTitle" runat="server" ControlToValidate="txtTitle" Display="Dynamic" ErrorMessage="Name already exists"
                             OnServerValidate="cvtxtTitle_ServerValidate">
                        </asp:CustomValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6" id="trShortName" runat="server">
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
           
        </div>
       <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Manager</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                     <ugit:UserValueBox ID="ppeManager" runat="server" CssClass="division-manager" width="100%"></ugit:UserValueBox>
                </div>
            </div>
           <div class="col-md-6 col-sm-6 col-xs-6" >
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtDescription" TextMode="MultiLine" CssClass="ms-long" runat="server" Rows="6" cols="20" />
                </div>
            </div>
           
       </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6" >
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel"><%= companyLabel %><b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList runat="server" ID="ddlCompany" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                    <div>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="error"  ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlCompany"
                            ErrorMessage="Select Company." InitialValue="0" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">GL Code</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtGLCode" runat="server" />
                </div>
            </div>
        </div>
        
        <div class="row addEditPopup-btnWrap">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel"  OnClick="btnCancel_Click" CssClass="secondary-cancelBtn">
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" CssClass="primary-blueBtn" OnClick="btnSave_Click">
                </dx:ASPxButton>
            </div>
        </div>
    </div>
</div>