
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddTicketEmail.ascx.cs" Inherits="uGovernIT.Web.AddTicketEmail" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpellChecker.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpellChecker" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var toolbarSize;
    function OnInit(s, e) {
        var editorSize = htmlEditorTicketEmailBody.GetHeight();
        var documentSize = $(htmlEditorTicketEmailBody.GetDesignViewDocument()).height();
        toolbarSize = Math.abs(editorSize - documentSize);
        var size = $(htmlEditorTicketEmailBody.GetDesignViewDocument()).height() + toolbarSize + 50;
        htmlEditorTicketEmailBody.SetHeight(size > 400 ? size : 400);
    }

    function closePopup(s, e) {
        var sourceURL = "<%= Request["source"] %>";
        window.parent.CloseWindowCallback(sourceURL);
    }

     $(document).ready(function () {
         $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
         $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
         $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
     });  
</script>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup ">
        <div class="row" id="tremailto" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">To<b style="color: Red;">*</b>:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <ugit:UserValueBox ID="ppeEmailTo" CssClass="userValueBox-dropDown" runat="server" IsMandatory="true" ValidationGroup="SendTicketEmail" 
                    isMulti="true"/>
            </div>
        </div>
        <div class="row" id="tremailcc" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">CC:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox runat="server" ID="txtEmailCC"></asp:TextBox>
                <asp:RegularExpressionValidator ID="rfvEmailCC" ControlToValidate="txtEmailCC" runat="server" ErrorMessage="Invalid Email." 
                    Display="Dynamic" ValidationGroup="SendTicketEmail" 
                    ValidationExpression="^((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,;\s])*)*$"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Subject:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox runat="server" ID="txtSubject"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Body:</h3>
            </div>
            <div class="ms-formbody accomp_inputField htmleditorcss">
                <dx:ASPxHtmlEditor ID="htmlEditorTicketEmailBody" ClientInstanceName="htmlEditorTicketEmailBody" OnImageFileSaving="htmlEditorTicketEmailBody_ImageFileSaving" OnHtmlCorrecting="htmlEditorTicketEmailBody_HtmlCorrecting" runat="server" Width="100%">
                    <ClientSideEvents Init="OnInit" />
                    <Settings AllowHtmlView="false" AllowPreview="false" />
                     <SettingsDialogs>
                        <InsertImageDialog>
                            <SettingsImageUpload UseAdvancedUploadMode="true" AdvancedUploadModeTemporaryFolder="~/content/images/ugovernittemp" UploadFolder="/Content/images/UploadedFiles/"></SettingsImageUpload>
                        </InsertImageDialog>
                    </SettingsDialogs>
                </dx:ASPxHtmlEditor>
            </div>
        </div>
        <div class="row" id="trattachment" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Attachment(s):</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:Panel ID="pAttachmentContainer" runat="server" CssClass="attachment-container" Visible="true">
                    <div class="attachmentform">
                        <asp:Panel ID="pAttachment" runat="server" CssClass="oldattachment">
                        </asp:Panel>
                        <asp:Panel ID="pNewAttachment" runat="server" CssClass="newattachment">
                        </asp:Panel>
                        <asp:Panel ID="pAddattachment" runat="server" CssClass="addattachment">
                            <a href="javascript:void(0);" onclick="addAttachment()">Add Files</a>
                        </asp:Panel>
                    </div>
                </asp:Panel>
                <asp:Label ID="lblerror" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </div>
        </div>
         <div class="row" id="trTicketCopy" runat="server">
            <div class="ms-formbody accomp_inputField crm-checkWrap">
                <asp:CheckBox ID="chbTicketCopy" runat="server" Text=" Keep a Copy" />
            </div>
        </div>
        <div class="row addEditPopup-btnWrap" id="tractionbuttons" runat="server">
            <dx:ASPxButton ID="btnSave" runat="server" Text="Send" ToolTip="Send Email" ValidationGroup="SendTicketEmail" CssClass="primary-blueBtn"
                OnClick="btnSave_Click">
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn">
                <ClientSideEvents Click="closePopup" />
            </dx:ASPxButton>
        </div>
    </div>
</div>


<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
        .attachment-container {
            float: left;
            width: 100%;
            padding-top: 7px;
        }

            .attachment-container .label {
                float: left;
                width: 24%;
                padding-left: 23px;
            }

            .attachment-container .attachmentform {
                float: left;
                width: 70%;
            }

        .attachmentform .oldattachment, .attachmentform .newattachment {
            float: left;
            width: 100%;
        }

        .attachmentform .fileupload {
            float: left;
            width: 100%;
        }

        .fileupload span, .fileread span {
            float: left;
        }

        .fileupload label, .fileread label {
            float: left;
            padding-left: 5px;
            font-weight: bold;
            padding-top: 3px;
            cursor: pointer;
        }

        .attachmentform .fileread {
            float: left;
            width: 100%;
        }

        .attachment-container .addattachment {
            float: left;
        }

            .attachment-container .addattachment img {
                border: 1px outset;
                padding: 3px;
            }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function addAttachment() {
        var uploadFiles = $(".attachment-container .fileitem");
        var uploadContainer = $(".attachment-container .newattachment");
        var addIcon = $(".attachment-container .addattachment");

        if (uploadFiles.length <= 5) {
            if (uploadFiles.length == 4) {
                addIcon.css("visibility", "hidden");
            }
            uploadContainer.append('<div class="fileitem fileupload"><span><input type="file" name="pAttachment1" /></span><label onclick="removeAttachment(this)"><img src="/Content/images/delete-icon.png" alt="Delete"/></label></div>');
        }
    }

    function removeAttachment(obj) {

        var fileName = $(obj).find("span").text()
        var deleteCtr = $(".attachment-container .label :text");
        deleteCtr.val(deleteCtr.val() + "~" + fileName);

        var addIcon = $(".attachment-container .addattachment");
        $(obj).parent().remove();
        addIcon.css("visibility", "visible");
    }

</script>
