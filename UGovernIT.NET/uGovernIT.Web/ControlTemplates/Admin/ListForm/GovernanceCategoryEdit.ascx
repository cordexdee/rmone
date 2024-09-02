<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GovernanceCategoryEdit.ascx.cs" Inherits="uGovernIT.Web.GovernanceCategoryEdit" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }
    
    .ms-formlabel {
        text-align: right;
        width:190px;
        vertical-align:top;
    }
    .ms-standardheader {
        text-align: right;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function hideddlCategory() {
        $("#ctl00_PlaceHolderMain_ctl00_ddlCategory").get(0).selectedIndex = 0;
        $(".ddlCategory").hide();
        $("#ctl00_PlaceHolderMain_ctl00_hdnCategory").val('1');

    }

    function showddlCategory() {
        $(".ddlCategory").show();
        $("#ctl00_PlaceHolderMain_ctl00_hdnCategory").val('0');
    }
</script>
 <asp:HiddenField ID="hdnCategory" runat="server" />
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Category Name<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="categoryName" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="categoryName"
                        ErrorMessage="Enter Category Name" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>               
            </div>
        </div>
       
         <div class="row" id="tr3" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Order</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtOrder" runat="server"/>
                <div>
                    <asp:RegularExpressionValidator ID="regextxtOrder" ValidationExpression="^([0-9]+)$" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtOrder" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="Save"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rqrdImgUrl" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtOrder" 
                        ErrorMessage="Enter Image Url " Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row" id="tr6" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Image Url</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtImageUrl" runat="server"/>
                <div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtImageUrl" ErrorMessage="Enter Navigation Url" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" ValidationGroup="Save" OnClick="btnSave_Click"></dx:ASPxButton>
        </div>
    </div>
</div>
