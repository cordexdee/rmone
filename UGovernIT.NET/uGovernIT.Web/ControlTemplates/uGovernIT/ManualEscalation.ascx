<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManualEscalation.ascx.cs" Inherits="uGovernIT.Web.ManualEscalation" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Src="~/ControlTemplates/uGovernIT/HtmlEditorControl.ascx" TagPrefix="uc1" TagName="HtmlEditorControl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxSpellChecker" Assembly="DevExpress.Web.ASPxSpellChecker.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Src="~/ControlTemplates/Utility/EmailContactsDropDown.ascx" TagPrefix="uc1" TagName="EmailContactsDropDown" %>

<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        /*background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;*/
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    /*.ms-formlabel {
        text-align: right;
        width: 190px;
        vertical-align: top;
    }

    .ms-standardheader {
        text-align: right;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }*/
</style>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
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
            uploadContainer.append('<div class="fileitem fileupload"><span><input type="file" name="pAttachment1" /></span><label onclick="removeAttachment(this)"><img src="/Content/images/redNew_delete.png" alt="Delete"/></label></div>');
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
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $('#<%=lblEmailToActionUser.ClientID%>').attr('for', $('#<%=cbIncludeActionUser.ClientID%>').attr('id'));
        $('#<%=lblSendMailFromLoggedInUser.ClientID%>').attr('for', $('#<%=chkSendMailFromLoggedInUser.ClientID%>').attr('id'));
        $('#emailLabelkeepCopy').attr('for', $('#<%=chbTicketCopy.ClientID%>').attr('id'));
    });
</script>
<asp:HiddenField ID="hdnAgentUrl" runat="server" />
<div class="row ms-formtable accomp-popup" id="tblMain" runat="server">
    <div class="col-md-6 col-sm-6 colForXS" id="tr5" runat="server" visible="false">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">To<b style="color: Red;">*</b>:</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <asp:TextBox ID="txtEscalationEmail" runat="server" Width="350px" />
            <asp:RequiredFieldValidator ID="rfvEmailTo" ControlToValidate="txtEscalationEmail" runat="server" ErrorMessage="Field required." Display="Dynamic" ValidationGroup="SendTicketEmail"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revEmailTo" ControlToValidate="txtEscalationEmail" runat="server" ErrorMessage="Invalid Email." Display="Dynamic" ValidationGroup="SendTicketEmail" ValidationExpression="^((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,;\s])*)*$"></asp:RegularExpressionValidator>
        </div>
    </div>
    <div class="col-md-6 col-sm-6 colForXS" id="trUserTo" runat="server">
        <div class="ms-formlabel nonSvcEmail_fieldLabel">
            <h3 class="ms-standardheader budget_fieldLabel ">To<b style="color: Red;">*</b>:</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <uc1:EmailContactsDropDown runat="server" ID="pEditorTo" />
            <%--<ugit:UserValueBox ID="pEditorTo" CssClass="userValueBox-dropDown" isMulti="true" runat="server" />--%>
            <div style="color: red;">
                <asp:CustomValidator ID="cvTO" OnServerValidate="cvTO_ServerValidate" ValidateEmptyText="true" Enabled="true" runat="server" ErrorMessage="Field Required." Display="Dynamic" ValidationGroup="SendTicketEmail"></asp:CustomValidator>
            </div>
        </div>
    </div>
    <div class="col-md-6 col-sm-6 colForXS" id="trMailToCC" runat="server">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Mail CC:</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <asp:TextBox ID="txtMailTOCC" runat="server" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtMailTOCC" runat="server" ErrorMessage="Invalid Email." Display="Dynamic" ValidationGroup="SendTicketEmail" ValidationExpression="^((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,;\s])*)*$"></asp:RegularExpressionValidator>
        </div>
    </div>
    <div class="ms-formbody accomp_inputField crm-checkWrap col-md-12 col-sm-12 col-xs-12" id="tr9" runat="server">
        <asp:CheckBox ID="cbIncludeActionUser" runat="server" />
        <label id="lblEmailToActionUser" runat="server">&nbsp; CC to Action User</label>
        <asp:Label ID="lblActionUser" runat="server" Visible="false"></asp:Label>
        <asp:HiddenField ID="hdnActionUser" runat="server" />
        <asp:CheckBox ID="chkSendMailFromLoggedInUser" Checked="false" runat="server" />
        <label id="lblSendMailFromLoggedInUser" runat="server">&nbsp; Send Mail from Logged In User</label>
    </div>


    <div class="col-md-12 col-sm-12 col-xs-12 noPadding fieldWrap" id="tr8" runat="server">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Mail Subject</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <asp:TextBox ID="txtMailSubject" Text="Ticket [$TicketId$] escalation" runat="server" />
            <div>
                <asp:RequiredFieldValidator ID="rfvtxtMailSubject" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtMailSubject" ErrorMessage="Enter Mail Subject" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
            </div>
        </div>
    </div>
    <div class="col-md-12 col-sm-12 col-xs-12 noPadding" id="tr6" runat="server">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Email Body</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <div style="height: 460px; overflow-y: auto;">
                <uc1:HtmlEditorControl runat="server" ID="htmlEditor" />
                <dx:ASPxHtmlEditor ID="EmailHtmlBody" runat="server" Width="100%" Height="450px" Theme="DevEx" Visible="false" SettingsAdaptivity-Enabled="true">
                    <Settings AllowHtmlView="false" AllowPreview="false"></Settings>
                </dx:ASPxHtmlEditor>
            </div>
        </div>
    </div>
    <div class="row">
    <div class="col-md-4 col-sm-4 col-xs-4 noPadding" id="trattachment" runat="server">
        <div class="ms-formbody ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Attachment(s): <a href="javascript:void(0);" onclick="addAttachment()" class="addFile-label">Add Files</a> </h3>
        </div>
        
    </div>
    <div class="col-md-8 col-sm-8 col-xs-8 noPadding">
        <div class="nonSvcEmail_fieldInput">
            <asp:Panel ID="pAttachmentContainer" runat="server" CssClass="attachment-container" Visible="true">
                <div class="attachmentform">
                    <asp:Panel ID="pAttachment" runat="server" CssClass="oldattachment">
                    </asp:Panel>
                    <asp:Panel ID="pNewAttachment" runat="server" CssClass="newattachment">
                    </asp:Panel>
                    
                </div>
            </asp:Panel>
            <asp:Label ID="lblerror" runat="server" ForeColor="Red" Visible="false"></asp:Label>
        </div>
    </div>
        </div>
    <div class="col-md-8 col-sm-8 col-xs-8" id="trTicketCopy" runat="server" style="padding-left:10px;">
        <div class="ms-formbody crm-checkWrap accomp_inputField">
            <asp:CheckBox ID="chbTicketCopy" runat="server" />
            <label id="emailLabelkeepCopy">&nbsp; Keep a Copy  &nbsp</label>
            <asp:CheckBox ID="chkPlainText" CssClass="clsshowhide" runat="server" Text="Plain Text" />
            <asp:CheckBox ID="chkDisableTicketLink" runat="server" CssClass="clsshowhide" Text="Disable Link" />
        </div>
        
    </div>

    <div class="col-md-12 col-sm-12 col-xs-12 addEditPopup-btnWrap">
        <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
        <dx:ASPxButton ID="btnUpdate" runat="server" Text="Send" ToolTip="Send" CssClass="primary-blueBtn" OnClick="btnUpdate_Click" ValidationGroup="SendTicketEmail"></dx:ASPxButton>
    </div>
</div>


<table cellpadding="0" cellspacing="0" style="border-collapse: collapse; width: 100%" id="tblErrorMessage" runat="server" visible="false">
    <tr>
        <td style="height: 400px; text-align: center">
            <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Text="sfsdfs"></asp:Label>
        </td>

    </tr>
    <tr>
        <td style="float: right;">
            <asp:LinkButton ID="LinkButton1" runat="server" Text="&nbsp;&nbsp;Cancel&nbsp;&nbsp;"
                ToolTip="Cancel" OnClientClick="window.frameElement.commitPopup();">
                        <span class="button-bg">
                            <b style="float: left; font-weight: normal;">
                                Cancel</b>
                            <i style="float: left; position: relative; top: -3px;left:2px">
                                <img src="/Content/images/uGovernIT/ButtonImages/cancelwhite.png"  style="border:none;" title="" alt=""/>
                            </i> 
                        </span>
            </asp:LinkButton>
        </td>
    </tr>
</table>
