<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LinkConfiguratorEdit.ascx.cs" Inherits="uGovernIT.Web.LinkConfiguratorEdit" %>
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

   

    function HideUploadLabel() {
        $("#<%=lblUploadedFile.ClientID %>").hide();
        $("#<%=fileUploadControl.ClientID %>").show();
        $("#<%=ImgEditFile.ClientID%>").hide();
        return false;
    }

</script>
<asp:HiddenField ID="hdnCategory" runat="server" />
<div class="my-2">
    <table class="ms-formtable accomp-popup" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
        <tr id="trTitle" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Section<b style="color: Red;">*</b>
                </h3>
            </td>
            <td class="ms-formbody">
                <div class="ddlCategory" id="divddlCategory" runat="server">
                    <table style="width:100%">
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlCategory" runat="server" Width="290px" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td style="width: 43px;">
                                <span style="margin-left: 6px;display: flex;justify-content: space-between;">
                                    <a id="aAddCategory" runat="server" href="">
                                        <img alt="Add Category" title="Add Category" id="addCategory" runat="server" src="/Content/images/plus-cicle.png" style="cursor: pointer;float:left;" width="16"/>
                                    </a>
                                    <a id="aEditCategory" runat="server" href="">
                                        <img id="editCategory" title="Edit Category" runat="server" alt="Edit Category" src="/content/images/editNewIcon.png" style="cursor: pointer;float:left;" width="16">
                                    </a>
                                </span>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr id="tr7" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Target Type
                </h3>
            </td>
            <td class="ms-formbody pt-3 d-block">
                <asp:DropDownList ID="ddlTargetType" Width="290px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTargetType_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>

        <tr id="trModel" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Analytic
                </h3>
            </td>
            <td class="ms-formbody">
                <div>
                    <asp:DropDownList ID="ddlModel" Width="290px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlModel_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlModel"
                        ErrorMessage="Please select an analytic." Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
        <tr id="trDashboard" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Analytic Dashboard
                </h3>
            </td>
            <td class="ms-formbody">
                <div>
                    <asp:DropDownList ID="ddlDashbaord" Width="290px" runat="server" OnSelectedIndexChanged="ddlDashbaord_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlDashbaord"
                        ErrorMessage="Please select a dashboard." Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>

        <tr id="trFileUpload" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">File
                </h3>
            </td>
            <td class="ms-formbody pt-3 d-block">
                <asp:Label ID="lblUploadedFile" runat="server"></asp:Label>
                 <%--<ugit:FileUploadControl ID="fileUploadControl" runat="server" MaxFile="1"   Multi="false"/>--%>
                <asp:FileUpload ID="fileUploadControl" CssClass="fileUploader" Width="200px" ToolTip="Browse and upload file" runat="server" style="display:none;"  />
                 <img alt="Edit File" title="Edit File" runat="server" id="ImgEditFile" src="/content/images/editNewIcon.png" style="cursor: pointer; width:16px;" onclick="HideUploadLabel();" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvFileUpload" CssClass="rfvdFileUploader" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="fileUploadControl" ErrorMessage="Upload a file." Display="Dynamic" ValidationGroup="fileSave"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
        <tr id="trLink" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Link
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:TextBox ID="txtFileLink" CssClass="fileUploaderLink" runat="server" Width="386px" />
                <div>
                </div>
            </td>

        </tr>

        <tr id="trControlsList" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Control
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:DropDownList ID="ddlControls" Width="290px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlControls_SelectedIndexChanged"></asp:DropDownList>

            </td>
        </tr>

         <tr id="trWiki" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Wiki<br />
                    <span class="lightText">(pick wiki)</span>
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:TextBox ID="txtWiki" runat="server" Width="290" />
               <img alt="Add Wiki" title="Add Wiki" runat="server" id="AddWikiItem" src="/content/images/editNewIcon.png" style="cursor: pointer;" />
            </td>
        </tr>

           <tr id="trList" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">List Name<b style="color: Red;">*</b>
                </h3>
            </td>
            <td class="ms-formbody">

                 <asp:DropDownList ID="ddlListName" CssClass="target_section" Width="290px" runat="server">
                 </asp:DropDownList>
                </td>

        </tr>
       

        <tr id="tr3" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Order<b style="color: Red;">*</b>
                </h3>
            </td>
            <td class="ms-formbody pt-3 d-block">
                <asp:TextBox ID="txtSequence" runat="server"/>
                <div>
                    <asp:RegularExpressionValidator ID="regextxtSequence" ValidationExpression="^([0-9]+)$" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtSequence" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="Save"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rqrdImgUrl" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtSequence"
                        ErrorMessage="Enter Sequence" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </td>

        </tr>
        <%--<tr id="trImage" runat="server">
                    <td class="ms-formlabel">
                        <h3 class="ms-standardheader">Image Url<b style="color: Red;">*</b>
                        </h3>
                    </td>
                    <td class="ms-formbody">
                        <asp:TextBox ID="txtImageUrl" runat="server" Width="386px" />
                        <div>
                            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtImageUrl"
                                ErrorMessage="Enter Image Url" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </div>
                    </td>

                </tr>--%>
        <tr id="tr1" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Item Name<b style="color: Red;">*</b>
                </h3>
            </td>
            <td class="ms-formbody pt-3 d-block">
                <asp:TextBox ID="txtItemName" runat="server" Width="386px" />
                <div>
                    <asp:RequiredFieldValidator ID="revtxtItemName" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtItemName" ErrorMessage="Enter Item Name" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
         <tr>
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Authorized To View</h3>
            </td>
            <td class="ms-formbody pt-3 d-block authorizedTbleWidth">
                <ugit:UserValueBox ID="ppeAuthorizedToView" runat="server" isMulti="true" />             
            </td>
        </tr>
        <tr id="tr5" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Description
                </h3>
            </td>
            <td class="ms-formbody py-3 d-block">
                <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server" Height="90px" />
                <div>
                </div>
            </td>
        </tr>



        <tr id="tr4" runat="server">
            <td colspan="2" class="ms-formlabel"></td>
        </tr>
    </table>
    <asp:Label ID="lblMessage" runat="server" Visible="false" ForeColor="Red"></asp:Label>
    <table width="100%">
        <tr id="tr2" runat="server" class="d-flex justify-content-between align-items-center px-1">
            <td>
                <div class="dxbButton_UGITNavyBlueDevEx btn-danger1">
                    <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" ToolTip="Delete" OnClick="lnkDelete_Click" CssClass="dxb d-block" OnClientClick="return confirm('Are you sure you want to delete?');">Delete</asp:LinkButton>
                </div>
            </td>
            <td>
                <div class="primary-blueBtn d-inline-block">
                    <asp:LinkButton ID="btAnalyticSync" Visible="false" runat="server" Text="Sync Analytics" ToolTip="Sync Aanlytics" OnClick="BtAnalyticSync_Click">
                        <span class="button-bg">
                            <b style="float: left; font-weight: normal;">Sync Analytics</b>
                        </span>
                    </asp:LinkButton>
                </div>
                <div class="dxbButton_UGITNavyBlueDevEx secondary-cancelBtn d-inline-block">
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" CssClass="dxb d-block">Cancel</asp:LinkButton>
                </div>
                <div class="primary-blueBtn d-inline-block">
                    <asp:LinkButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click" OnClientClick="return validation();" CssClass="dxb">Save</asp:LinkButton>
                </div>
            </td>
        </tr>
    </table>

<%--    <asp:HiddenField ID="hdnWikiFileType" runat="server" /> --%>
</div>
