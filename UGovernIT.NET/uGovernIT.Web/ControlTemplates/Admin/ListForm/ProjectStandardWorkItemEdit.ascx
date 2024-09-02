<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectStandardWorkItemEdit.ascx.cs" Inherits="uGovernIT.Web.ProjectStandardWorkItemEdit" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
function DeleteCongigVariable() {
        <%--if (confirm('Are you sure want to delete?')) {
            <%=Page.ClientScript.GetPostBackEventReference(lnkDelete1,string.Empty)%>
         }--%>
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12 noPadding configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <div>
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtProjectWorkItem" runat="server" Width="100%" ValidationGroup="Save" />
                <asp:RequiredFieldValidator ID="rfvProjectWorkItem" Display="Dynamic" ErrorMessage="Enter Title" CssClass="error" ControlToValidate="txtProjectWorkItem" runat="server" ValidationGroup="Save" />
                <asp:CustomValidator ID="rfcProjectWorkItem" runat="server" Display="Dynamic" CssClass="error" ControlToValidate="txtProjectWorkItem" ErrorMessage="Title should be unique." ValidationGroup="Save" OnServerValidate="rfcProjectWorkItem_ServerValidate"></asp:CustomValidator>
            </div>
        </div>
        <div class="row" id="tr12" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Budget Category</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:Label ID="lbBudgetCategory" runat="server"></asp:Label>
                <asp:UpdatePanel ID="budgetPanel" runat="server">
                    <ContentTemplate>
                        <div class="col-md-6 col-sm-6 col-xs-6 noLeftPadding">
                            <asp:DropDownList ID="ddlBudgetCategory" AutoPostBack="true" runat="server" Width="100%" CssClass="itsmDropDownList aspxDropDownList" 
                            OnSelectedIndexChanged="DDLBudgetCategory_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-6 noRightPadding">
                            <asp:DropDownList ID="ddlSubBudgetCategory" runat="server" Width="100%" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noLeftPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Order</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtItemOrder" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noRightPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Code</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row" id="deletePanel" runat="server">
            <div class="ms-formbody crm-checkWrap" style="padding-left:5px;">
                <asp:CheckBox ID="chkDelete" Text="Deleted" TextAlign="Right" runat="server" />
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ValidationGroup="btnSave" ID="ASPxButton1" Visible="true" runat="server" Text="Save"
                ToolTip="Save as Template" CssClass="primary-blueBtn"  OnClick="btnSave_Click">
            </dx:ASPxButton>
        </div>
    </div>
</div>
