
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceCategoryEditor.ascx.cs" Inherits="uGovernIT.Web.ServiceCategoryEditor" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

   <%-- function pickSiteAsset(Url) {
        var siteAsset = unescape(Url);
    }--%>

</script>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        /*background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;*/
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    /*ms-formlabel {
        text-align: right;
        width: 190px;
        vertical-align: top;
    }

    .ms-standardheader {
        text-align: right;
    }*/
</style>

<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer">
    <div class="ms-formtable accomp-popup ">
        <div class="row" id="tr3" runat="server">
             <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Title</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtTitle" runat="server"/>
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtImpact" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                        ErrorMessage="Please enter title "  ForeColor="Red" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                    <asp:CustomValidator ID="rfvCustomImpact" runat="server" ControlToValidate="txtTitle" ErrorMessage="Title must be unique"
                        OnServerValidate="RFVCustomImpact_ServerValidate"></asp:CustomValidator>
                </div>
            </div>
        </div>
        <div class="row" id="tr2" runat="server">
             <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Icon</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <ugit:UGITFileUploadManager ID="fileUploadIcon" runat="server" hideWiki="true" />
                   <%-- <asp:TextBox runat="server" ID="txtFile" Width="320px" />
                    <br />
                    <asp:LinkButton ID="lnkbtnPickAssets" runat="server" Font-Size="10px" Text="PickFromAsset">Pick From Library</asp:LinkButton>--%>
                    <%--<asp:Button Text="PickFromAsset" ID="btnPickfromAsset"  runat="server"/>--%>
            </div>
        </div>
        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Item Order</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtItemOrder" runat="server" CssClass="asptextbox-asp" />
                <div>
                    <asp:RegularExpressionValidator ID="regextxtItemOrder" ValidationExpression="^([0-9]+)$" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtItemOrder" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="Save"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="revtxtItemOrder" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtItemOrder" ErrorMessage="Enter Item Order" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                <div style="display:inline-block;" id="tdDeleteAction" runat="server" visible="false">
                    <dx:ASPxButton ID="LnkbtnDelete" runat="server" Text="Delete" ToolTip="Delete" OnClick="LnkbtnDelete_Click" CssClass="secondary-cancelBtn">
                        <ClientSideEvents Click="function(s, e){return confirm('Are you sure you want to delete?');}" />
                    </dx:ASPxButton>
                </div>
                <div style="display:inline-block;">
                    <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
                </div>
                <div style="display:inline-block;">
                    <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click" CssClass="primary-blueBtn" ></dx:ASPxButton>
                </div>
            </div>
        </div>
    </div>
</div>

    
