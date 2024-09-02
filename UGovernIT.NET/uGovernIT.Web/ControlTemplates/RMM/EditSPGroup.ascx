
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditSPGroup.ascx.cs" Inherits="uGovernIT.Web.EditSPGroup" %>
<%@ Register TagPrefix="ugit" Assembly="uGovernIT.Web" Namespace="uGovernIT.Web" %>
<%@ Import Namespace="uGovernIT.Utility" %>


<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function DeleteGroup(s, e)
    {
        var numMember = parseInt($("#<%=usercount.ClientID%>").val());
        if (numMember > 0) {
            if (confirm("This group contains members, are you sure want to delete it?")) {
                e.processOnServer = true;
                return true;
            }
        }
        else {
            if (confirm("Are you sure want to delete this group?")) {
                e.processOnServer = true;
                return true;
            }
        }

        e.processOnServer = false;
        return false;
    }
</script>
<h3 id="grpLiteral" runat="server"><asp:Literal ID="msgLiteral" runat="server"></asp:Literal></h3>
<div class="col-md-12 col-sm-12 col-xs-12 noPadding configVariable-popupWrap">
    <div id="groupTable" runat="server" class="ms-formtable accomp-popup row">
        <div id="tr1" class="col-md-6 col-sm-6 col-xs-6" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Group Name<b style="color: Red">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtName" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="txtNameRequired" runat="server" ControlToValidate="txtName" ValidationGroup="savechanges" Display="Dynamic" ErrorMessage="Please enter group name">
                </asp:RequiredFieldValidator>
                <asp:CustomValidator ID="txtNameCustomValidator" ControlToValidate="txtName" ValidationGroup="savechanges" CssClass="error" runat="server" OnServerValidate="txtNameCustomValidator_ServerValidate" ErrorMessage="Group with this name already exists" Display="Dynamic"></asp:CustomValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                    runat="server" Display="dynamic"
                    ControlToValidate="txtName"
                     ValidationGroup="savechanges"
                    ValidationExpression="^([\S\s]{0,50})$"
                    ErrorMessage="Enter maximum 50 characters for group name"
                    CssClass="error" >
                </asp:RegularExpressionValidator>
            </div>
        </div>
       
        <div id="Div2" class=" col-md-6 col-sm-6 col-xs-6" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel ">Menu Link</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlLandingPage" runat="server" CssClass="aspxDropDownList">
                    </asp:DropDownList>
            </div>
        </div>
        <div id="tr2" class="col-md-12 col-sm-12 col-xs-12" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel ">Description
                </h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtDesc" CssClass="txtbox-width" TextMode="MultiLine" ValidationGroup="savechanges" runat="server"></asp:TextBox>
            </div>
        </div>
        <div id="tr3" class="col-md-12 col-sm-12 col-xs-12" runat="server">
            <div class="addEditPopup-btnWrap">
                <dx:ASPxButton ID="btnDelete" runat="server" Text="Delete From Site" Visible="false" 
                    OnClick="btnDelete_Click" CssClass="secondary-cancelBtn">
                    <ClientSideEvents Click="DeleteGroup" />
                </dx:ASPxButton> 
                <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Delete From Site" Visible="false" 
                    OnClick="btnDelete_Click" CssClass="secondary-cancelBtn">
                    <ClientSideEvents Click="DeleteGroup" />
                </dx:ASPxButton> 
                <dx:ASPxButton ID="btSave" runat="server" Text="Save" OnClick="btSave_Click" ValidationGroup="savechanges"  
                    CssClass="primary-blueBtn">
                </dx:ASPxButton>
                <asp:HiddenField   id="usercount" runat="server" />
            </div>
        </div>
    </div>
</div>
