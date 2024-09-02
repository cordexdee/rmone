
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectTemplateEdit.ascx.cs" Inherits="uGovernIT.Web.ProjectTemplateEdit" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
function DeleteCongigVariable() {
        if (confirm('Are you sure want to delete?')) {
            <%=Page.ClientScript.GetPostBackEventReference(lnkDelete1,string.Empty)%>
         }
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding">
    <div class="ms-formtable accomp-popup">
        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Lifecycle</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:Label ID="lbLifeCycle" runat="server"></asp:Label>
            </div>
       </div>
       <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Name<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
               <asp:TextBox ID="txtTitle" runat="server" Width="100%" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                        ErrorMessage="Please enter template name" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvtxtTitle" runat="server" ValidationGroup="Save" ControlToValidate="txtTitle" Display="Dynamic" ErrorMessage="Template with this name already exists"
                         OnServerValidate="cvtxtTitle_ServerValidate">
                    </asp:CustomValidator>
                </div>
            </div>
        </div>
         <div class="row" >
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtDescription" TextMode="MultiLine" CssClass="ms-long" runat="server" Rows="6" cols="20" />
            </div>
        </div>
        <div class="row" id="tr4" runat="server">
            <div class="ms-formlabel" style="text-align:left;">
               <asp:Panel ID="pTaskTreeView" runat="server" CssClass="ptasktreeview"></asp:Panel>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="lnkDelete" runat="server" Visible="true" Text="Delete" ToolTip="Delete" AutoPostBack="false" CssClass="secondary-cancelBtn">
                <ClientSideEvents Click="function(s,e){DeleteCongigVariable();}" />
            </dx:ASPxButton>
            <asp:LinkButton ID="lnkDelete1" runat="server" OnClick="btnDelete_Click"></asp:LinkButton>
            
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ValidationGroup="Save" ID="btnSave" Visible="true" runat="server" Text="Save"
                ToolTip="Save as Template" CssClass="primary-blueBtn"  OnClick="btnSave_Click">
            </dx:ASPxButton>
        </div>
    </div>
</div>