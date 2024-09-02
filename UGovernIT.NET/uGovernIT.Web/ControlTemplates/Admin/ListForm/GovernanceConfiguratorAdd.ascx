<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GovernanceConfiguratorAdd.ascx.cs" Inherits="uGovernIT.Web.GovernanceConfiguratorAdd" %>
<%@ Import Namespace="uGovernIT.Utility" %>
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
    function validation() {
        $(".labelError").text("");
        if (($(".target_section").val() == "File" && $.trim($(".fileUploader").val()) == "")) {
           
        }

        if ($(".target_section").val() == "Link" && $.trim($(".fileUploaderLink").val()) == "") {
            $(".labelError").text("Link is required.");
            return false;
        }
            return true;
        
    }
</script>
<asp:HiddenField ID="hdnCategory" runat="server" />
<div class="col-md-12 col-sm-12 col-xs-12 noPadding configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Category Name<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <div class="ddlCategory" id="divddlCategory" runat="server">
                    <div class="col-md-11 col-sm-11 col-xs-11 noPadding">
                         <asp:DropDownList ID="ddlCategory" runat="server" CssClass="itsmDropDownList aspxDropDownList" AutoPostBack="true" 
                                    OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div class="col-md-1 col-sm-1 col-xs-1 noPadding">
                        <a id="aAddCategory" runat="server" href="">
                            <img alt="Add Category" id="addCategory" width="16" runat="server" src="/Content/Images/plus-cicle.png" style="cursor: pointer; margin-left:5px;" />
                        </a>
                         <a id="aEditCategory" runat="server" href="">
                            <img id="editCategory" runat="server" alt="Edit Category" width="16" src="/Content/Images/editNewIcon.png" style="cursor: pointer">
                        </a>
                    </div>
                </div>
            </div>
        </div>
       
        <div class="row" id="tr7" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Target Type</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlTargetType" CssClass="target_section itsmDropDownList aspxDropDownList" runat="server" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlTargetType_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>
        <div class="row" id="trModel" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Analytic</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlModel" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList" runat="server" OnSelectedIndexChanged="ddlModel_SelectedIndexChanged"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlModel"
                ErrorMessage="Please select an analytic." Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
            </div>
        </div>
      
        <div class="row" id="trDashboard" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Analytic Dashboard</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlDashbaord" runat="server" CssClass="itsmDropDownList aspxDropDownList" OnSelectedIndexChanged="ddlDashbaord_SelectedIndexChanged"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlDashbaord"
                    ErrorMessage="Please select a dashboard." Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row" id="trFileUpload" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">File</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <ugit:FileUploadControl ID="fileUploadControl" runat="server" IsMandate="true" ValidationGroup="Save" DisplayText="Upload Image"   />
            </div>
        </div>
        
        <div class="row" id="trLink" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Link</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtFileLink" CssClass="fileUploaderLink" runat="server" />
            </div>
        </div>
        <div class="row" id="trControlsList" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Control</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlControls" runat="server" CssClass="itsmDropDownList aspxDropDownList" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlControls_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>

        <div class="row" id="tr3" runat="server">
            <div class="ms-formlabel"> 
                <h3 class="ms-standardheader budget_fieldLabel">Sequence<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtSequence" runat="server" />
                <div>
                        <asp:RegularExpressionValidator ID="regextxtSequence" ValidationExpression="^([0-9]+)$" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtSequence" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="Save"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rqrdImgUrl" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtSequence"
                        ErrorMessage="Enter Sequence" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row" id="trImage" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Image Url<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtImageUrl" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtImageUrl"
                        ErrorMessage="Enter Image Url" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        
        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Item Name<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtItemName" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="revtxtItemName" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtItemName" ErrorMessage="Enter Item Name" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row" id="tr5" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server" />
            </div>
        </div>
            
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btAnalyticSync" Visible="false" runat="server" Text="Sync Analytics" CssClass="primary-blueBtn" ToolTip="Sync Aanlytics" OnClick="BtAnalyticSync_Click"></dx:ASPxButton>
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ValidationGroup="Save" ID="btnSave" Visible="true" runat="server" Text="Save" ToolTip="Save as Template" CssClass="primary-blueBtn" OnClick="btnSave_Click" ></dx:ASPxButton>
        </div>       
    </div>
</div>
