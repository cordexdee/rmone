<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LinkConfiguratorAdd.ascx.cs" Inherits="uGovernIT.Web.LinkConfiguratorAdd" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    $(function () {
        if ($("#<%=ddlCategory.ClientID %>").get(0).selectedIndex > -1) {
            $("#<%=aEditCategory.ClientID %>").show();
        }
        else {
            $("#<%=aEditCategory.ClientID %>").hide();
        }
    })

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
            $(".labelError").text("Upload a file.");
            return false;
        }
        if ($(".target_section").val() == "Link" && $.trim($(".fileUploaderLink").val()) == "") {
            $(".labelError").text("Link is required.");
            return false;
        }
        return true;

    }

    

</script>
<asp:HiddenField ID="hdnCategory" runat="server" />
<asp:HiddenField ID="hdnWikiFileType" runat="server" /> 

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Section<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <div class="row ddlCategory ddlCategoryCus" id="divddlCategory" runat="server">
                    <div class="col-md-11 col-sm-11 col-xs-11 noPadding">
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="itsmDropDownList aspxDropDownList" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvddlCategory" runat="server" ErrorMessage="please select link category " ControlToValidate="ddlCategory" 
                                        ValidationGroup="Save" InitialValue="" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-1 col-sm-1 col-xs-1 noPadding">
                        <a id="aAddCategory" runat="server" href="">
                            <img alt="Add Category" title="Add Category" id="addCategory" runat="server" src="/Content/images/plus-cicle.png" style="cursor: pointer; margin-left:5px; width:16px;" />
                        </a>
                         <a id="aEditCategory" runat="server" href="">
                            <img id="editCategory" title="Edit Category" runat="server" alt="Edit Category" src="/Content/images/editNewIcon.png" style="cursor: pointer; float: right; width:16px;">
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
                <div>
                    <asp:DropDownList ID="ddlModel" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList" runat="server" OnSelectedIndexChanged="ddlModel_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlModel"
                        ErrorMessage="Please select an analytic." Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row" id="trDashboard" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Analytic Dashboard</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <div>
                    <asp:DropDownList ID="ddlDashbaord" runat="server" CssClass="itsmDropDownList aspxDropDownList" OnSelectedIndexChanged="ddlDashbaord_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlDashbaord"
                        ErrorMessage="Please select a dashboard." Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>

        <div class="row" id="trFileUpload" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">File</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                    <asp:FileUpload ID="fileUploadControl" CssClass="fileUploader" ToolTip="Browse and upload file" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvFileUpload" CssClass="rfvdFileUploader" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="fileUploadControl" ErrorMessage="Upload a file." Display="Dynamic" ValidationGroup="fileSave"></asp:RequiredFieldValidator>
                </div>
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
                <asp:DropDownList ID="ddlControls" runat="server" CssClass="itsmDropDownList aspxDropDownList" AutoPostBack="true" OnSelectedIndexChanged="ddlControls_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>

        <div class="row" id="trWiki" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Wiki<br />
                    <span class="lightText">(pick wiki)</span>
                </h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtWiki" runat="server" />
                <img alt="Add Wiki" title="Add Wiki" runat="server" id="AddWikiItem" src="/Content/images/editNewIcon.png" style="cursor: pointer; width:16px;" />
            </div>
        </div>

      <%--  <tr id="trWikiControl" runat="server">
            <td>s
                <a id="aAddItem" runat="server" onclick="showWiki()" style="cursor: pointer;">Add Wiki</a> 
            </td>
        </tr>--%>

        
          <div class="row" id="trList" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">List Name<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                 <asp:DropDownList ID="ddlListName" CssClass="target_section itsmDropDownList aspxDropDownList" runat="server">
                 </asp:DropDownList>
            </div>
        </div>

        <div class="row" id="tr3" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Order<b style="color: Red;">*</b></h3>
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
         <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Authorized To View</h3>
            </div>
            <div class="ms-formbody accomp_inputField">                
               <ugit:UserValueBox ID="ppeAuthorizedToView" runat="server" Width="100%" CssClass="assignto-userToken"  SelectionSet="User,SPGroup" isMulti="true" />
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
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ID="btAnalyticSync" Visible="false" runat="server" Text="Sync Analytics" ToolTip="Sync Aanlytics" OnClick="BtAnalyticSync_Click"></dx:ASPxButton>
            <dx:ASPxButton ValidationGroup="Save" ID="btnSave" Visible="true" runat="server" Text="Save"
                ToolTip="Save as Template" Style="float:left;margin-right:16px;" CssClass="primary-blueBtn"  OnClick="btnSave_Click">
                <ClientSideEvents Click="function(s,e){validation();}" />
            </dx:ASPxButton>
        </div>
    </div>
    <asp:Label ID="lblMessage" runat="server" Visible="false" ForeColor="Red"></asp:Label>
</div>
