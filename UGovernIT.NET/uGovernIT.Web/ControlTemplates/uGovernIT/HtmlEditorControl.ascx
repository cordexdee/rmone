<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HtmlEditorControl.ascx.cs" Inherits="uGovernIT.Web.HtmlEditorControl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxSpellChecker" Assembly="DevExpress.Web.ASPxSpellChecker.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" id="dxss_inlineCtrHtmlEditor" data-v="<%=UGITUtility.AssemblyVersion %>">
    function ShowPopup() {
        pcHtmlEditor.Show();
        HtmlBody.SetHtml('<%=HtmlData%>');
    }
    var i = parseInt(1);
    function OnCustomCommand(s, e) {

        if (e.commandName == "picktoken") {
            if (i % 2 == 0) {
                popupPickTokenInEditor.Hide();
            }
            else {
                $('#<%=ddlTokenType.ClientID%>').val("0");
                $('#<%=trAgents.ClientID%>').hide();
                popupPickTokenInEditor.ShowAtElement($(".picktoken-sprite").parent().get(0));
            }
            i++;
        }
        else
            i = 1;
    }
    function OnCommandExecuted(s, e) {
        alert("sd");
    }
    function TokenTypeChange(obj) {
        hdnTokenType.Set("tokentype", obj.selectedIndex);
        upcallbackpnl.PerformCallback();
    }
    function ShowHideDaysInput(s, e) {
        aspxdayscallback.PerformCallback(s.GetValue()[0]);
    }
    function insertTextInHTMLEditorAtCursor() {
        debugger;
        var Token = "";
        if ($('#<%=ddlTokenType.ClientID%> option:selected').val() == '<%=uGovernIT.Utility.Constants.ModuleAgent%>') {
            Token = " " + $('#<%=ddlAgents.ClientID%> option:selected').val();
        }
        else if ($('#<%=ddlTokenType.ClientID%> option:selected').val() == '<%=uGovernIT.Utility.Constants.ModuleField%>') {
            var trObj = $('#<%=trDays.ClientID%>');
            if (trObj.length > 0 && !trObj[0].hidden) {
                var signObj = $('#<%=ddlPlusMinusDefaultDate.ClientID%>');
                var days = 0;
                if (signObj != null && signObj.length > 0) {
                    days = (speDefaultDateNoofDays.GetValue() == null || speDefaultDateNoofDays.GetValue() == undefined) ? 0 : speDefaultDateNoofDays.GetValue();

                    if (signObj[0].value == "1")
                        days = -days;
                }

                Token = "[$" + glModuleFields.GetValue()[0] + '|bdays|' + days + "$]"
            }
            else
                Token = "[$" + glModuleFields.GetValue()[0] + "$]";
        }
        else if ($('#<%=ddlTokenType.ClientID%> option:selected').val() == '<%=uGovernIT.Utility.Constants.ModuleAgent%>') {
            Token = "[$IncludeActionButtons$]";
        }
        else {
            Token = " [$SurveyLink|Please click here to provide your feedback$]";
        }
        Token = Token.trim();
        if (Token != undefined && Token != null && Token != "undefined")
            HtmlBody.ExecuteCommand(ASPxClientCommandConsts.PASTEHTML_COMMAND, Token);
        popupPickTokenInEditor.Hide();
        return false;
    }
    <%--function insertTextInHTMLEditorAtCursor() {
        debugger;
        var Token = "";
        if ($('#<%=ddlTokenType.ClientID%> option:selected').val() == '<%= uGovernIT.Utility.Constants.ModuleAgent%>') {
            Token = " " + $('#<%=ddlAgents.ClientID%> option:selected').val();
        }
        else {
            Token = " [$SurveyLink|Please click here to provide your feedback$]";
        }
        Token = Token.trim();
        if (Token != undefined && Token != null && Token != "undefined")
            HtmlBody.ExecuteCommand(ASPxClientCommandConsts.PASTEHTML_COMMAND, Token);
        popupPickTokenInEditor.Hide();
        return false;
    }--%>
    function hidepopup() {
        popupPickTokenInEditor.Hide();
    }


    var oldattrHeight;
    var oldcssHeight;
    function changesize(isVisible, ctrID) {
        var frame = window.frameElement;
        var height = $(".managementcontrol-main").height();
        var addHeight = Number($(frame).attr("addheight"));
        var minheight = Number($(frame).attr("minheight"));
        if (!isNumber(minheight))
            minheight = 0;

        if (!isNumber(addHeight))
            addHeight = 10;

        if (height && height > 0) {
            var totalHeight = height + addHeight;
            if (minheight > 0 && totalHeight < minheight)
                totalHeight = minheight;

            if (oldattrHeight == '' || oldattrHeight == undefined)
                oldattrHeight = $(frame).attr("height");

            if (oldcssHeight == '' || oldcssHeight == undefined)
                oldcssHeight = $(frame).height();

            if (isVisible) {
                $(frame).css("height", oldcssHeight + 180 + "px");
                if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {

                    var pcHtmlEditorPopup = ASPxClientControl.GetControlCollection().GetByName(ctrID + '_pcHtmlEditor');
                    var elementPositionId = $("span.commonresizeclass[humtum='setcss']").attr('id');
                    if (elementPositionId != null && elementPositionId != undefined) {
                        pcHtmlEditorPopup.ShowAtElementByID(elementPositionId);
                    }
                }
                else {
                    setTimeout(function () {

                        var pcHtmlEditorPopup = ASPxClientControl.GetControlCollection().GetByName(ctrID + '_pcHtmlEditor');
                        pcHtmlEditorPopup.UpdatePosition();

                    }, 200);
                }
            }
            else {
                $(frame).css("height", oldcssHeight + "px");
            }
        }
    }


    var updateHtml = '';
    var processCommand = 0;
    function SetHtml(s, modifiedHtml) {
        var express = /(font-family|mso-bidi-font-family|mso-ascii-font-family|mso-hansi-font-family|mso-fareast-font-family):[^;"]*(|;)?/g;
        modifiedHtml = modifiedHtml.replace(/&quot;/g, "'");
        modifiedHtml = modifiedHtml.replace(/font-size:[^;]*(;)?/g, '').trim();
        modifiedHtml.replace(/<\/H1>$/);
        modifiedHtml.replace(/<\/H2>$/);
        modifiedHtml.replace(/<\/H3>$/);
        modifiedHtml.replace(express, '').trim();
        updateHtml = modifiedHtml;
        s.SetHtml(updateHtml);
        updateHtml = '';
    }
    function StripStyleAttribute() {
        SetHtml(HtmlBody, HtmlBody.GetHtml());
        changesize(false);
    } 
</script>

<div>
    <div class="emailBody-editIcon">
        <a runat="server" id="aEditbutton" href="javascript:void(0)">
            <img src="/Content/images/editNewIcon.png" width="16" onclick="ShowPopup()" alt="edit" title="Edit" />
        </a> 
    </div>
    <div runat="server" id="tdvalues"></div>
    
</div>

<dx:ASPxPopupControl ID="pcHtmlEditor" runat="server" ClientInstanceName="pcHtmlEditor"
    CloseAction="CloseButton" Modal="True" AllowDragging="true" SettingsAdaptivity-Mode="Always"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssClass="aspxPopup">
    <ContentCollection>
        <dx:PopupControlContentControl>
            <dx:ASPxPanel ID="pnlcontrol" runat="server" Width="470px" DefaultButton="btnOK">
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
                            <div class="row">
                                <dx:ASPxHtmlEditor ClientInstanceName="HtmlBody" ClientSideEvents-ContextMenuShowing="" ID="HtmlBody" runat="server" Width="100%" Height="300px">
                                    <Settings AllowHtmlView="false" AllowPreview="false" />
                                    <ClientSideEvents CustomCommand="OnCustomCommand" />
                                    <Toolbars>
                                        <dx:HtmlEditorToolbar>
                                            <Items>
                                                <dx:ToolbarCutButton></dx:ToolbarCutButton>
                                                <dx:ToolbarCopyButton></dx:ToolbarCopyButton>
                                                <dx:ToolbarPasteButton></dx:ToolbarPasteButton>
                                                <dx:ToolbarPasteFromWordButton></dx:ToolbarPasteFromWordButton>
                                                <dx:ToolbarUndoButton></dx:ToolbarUndoButton>
                                                <dx:ToolbarRedoButton></dx:ToolbarRedoButton>
                                                <dx:ToolbarInsertLinkDialogButton></dx:ToolbarInsertLinkDialogButton>
                                                <dx:ToolbarUnlinkButton></dx:ToolbarUnlinkButton>
                                                <dx:ToolbarIndentButton></dx:ToolbarIndentButton>
                                                <dx:ToolbarOutdentButton></dx:ToolbarOutdentButton>

                                                <dx:ToolbarBoldButton></dx:ToolbarBoldButton>
                                                <dx:ToolbarItalicButton></dx:ToolbarItalicButton>
                                                <dx:ToolbarUnderlineButton></dx:ToolbarUnderlineButton>

                                                <dx:ToolbarJustifyLeftButton></dx:ToolbarJustifyLeftButton>
                                                <dx:ToolbarJustifyCenterButton></dx:ToolbarJustifyCenterButton>
                                                <dx:ToolbarJustifyRightButton></dx:ToolbarJustifyRightButton>
                                                <dx:ToolbarBackColorButton></dx:ToolbarBackColorButton>
                                                <dx:ToolbarFontColorButton></dx:ToolbarFontColorButton>
                                                <dx:ToolbarCheckSpellingButton></dx:ToolbarCheckSpellingButton>
                                                <dx:ToolbarFullscreenButton></dx:ToolbarFullscreenButton>
                                                <dx:CustomToolbarButton Visible="false" CommandName="picktoken" Image-SpriteProperties-CssClass="picktoken-sprite" ToolTip="Pick Token(s)" Text="Pick Token(s)">
                                                </dx:CustomToolbarButton>
                                            </Items>
                                        </dx:HtmlEditorToolbar>
                                    </Toolbars>
                                </dx:ASPxHtmlEditor>
                            </div>
                            <div class="row addEditPopup-btnWrap">
                                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" AutoPostBack="false"
                                    UseSubmitBehavior="false" CssClass="secondary-cancelBtn">
                                    <ClientSideEvents Click="function(s, e) { pcHtmlEditor.Hide();}" />
                                </dx:ASPxButton>
                                <dx:ASPxButton ID="btnOK" runat="server" Text="Submit"
                                    OnClick="btnOK_Click" CssClass="primary-blueBtn">
                                    <ClientSideEvents Click="function(s,e){StripStyleAttribute();}" />
                                </dx:ASPxButton>
                            </div>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="popupPickTokenInEditor" runat="server" Modal="false" Width="300px" PopupElementID="btn"
    ClientInstanceName="popupPickTokenInEditor"
    HeaderText="Pick Token" AllowDragging="false" ShowHeader="false" PopupAnimationType="None" EnableViewState="False" PopupHorizontalAlign="LeftSides" PopupVerticalAlign="Below" EnableHierarchyRecreation="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl10" runat="server" >

            <div>
                <div style="float: left; width: 100%;">
                    <asp:UpdatePanel ID="updPnlPickToken" runat="server">
                        <ContentTemplate>
                            <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
                                <tr class="trTitle">
                                    <td class="ms-formlabel" style="text-align: left;">
                                        <h3 class="ms-standardheader">Token Type<b style="color: Red;">*</b>
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                        <asp:DropDownList ID="ddlTokenType" runat="server"  OnSelectedIndexChanged="ddlTokenType_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="--Pick Token--" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Module Survey" Value="~ModuleFeedback~"></asp:ListItem>
                                            <asp:ListItem Text="Module Agent" Value="~ModuleAgent~"></asp:ListItem>
                                            <asp:ListItem Text="Action Buttons Link" Value="~ActionButtons~"></asp:ListItem>
                                            <asp:ListItem Text="Module Fields" Value="~ModuleField~"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trAgents" runat="server" visible="false">
                                    <td class="ms-formlabel" style="text-align: left;">
                                        <h3 class="ms-standardheader">Agent Name<b style="color: Red;">*</b>
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                        <asp:DropDownList ID="ddlAgents" runat="server">
                                        </asp:DropDownList>

                                    </td>
                                </tr>
                                   <tr id="trModuleFields" runat="server" visible="false">
                                        <td class="ms-formlabel" style="text-align: left;">
                                            <h3 class="ms-standardheader">Fields<b style="color: Red;">*</b>
                                            </h3>
                                        </td>
                                        <td class="ms-formbody">
                                            <dx:ASPxGridLookup Visible="true" CssClass="stagesctionusers"  TextFormatString="{0}" ClientInstanceName="glModuleFields" SelectionMode="Single" ID="glModuleFields" runat="server" KeyFieldName="InternalName" MultiTextSeparator="; ">
                                                <ClientSideEvents ValueChanged="function(s,e){ShowHideDaysInput(s,e);}" />
                                                <Columns>
                                                    <dx:GridViewDataTextColumn FieldName="FieldName" Width="230px" Caption="Choose Fields:" SortOrder="Ascending">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="InternalName" Visible="false">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="FieldType" Visible="false">
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>

                                                <GridViewProperties>
                                                    <Settings ShowGroupedColumns="false" ShowFilterRow="false" VerticalScrollBarMode="Auto" />
                                                    <SettingsBehavior AllowSort="false" AllowGroup="true" AutoExpandAllGroups="true" />
                                                    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                                </GridViewProperties>
                                                <ClientSideEvents />
                                            </dx:ASPxGridLookup>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td></td>
                                        <td class="ms-formbody">
                                            <dx:ASPxCallbackPanel ID="aspxdayscallback" ClientInstanceName="aspxdayscallback" EnableHierarchyRecreation="false" runat="server" RenderMode="Table">
                                                <PanelCollection>
                                                    <dx:PanelContent>
                                                        <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
                                                            <tr id="trDays" runat="server" visible="false">
                                                                <td>
                                                                    <asp:DropDownList ID="ddlPlusMinusDefaultDate" Style="height: 21px; float: left; margin-right: 2px;" runat="server">
                                                                        <asp:ListItem Value="0">+</asp:ListItem>
                                                                        <asp:ListItem Value="1">-</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <dx:ASPxSpinEdit ID="speDefaultDateNoofDays" ClientInstanceName="speDefaultDateNoofDays" HelpTextSettings-VerticalAlign="Middle" HelpText="Day(s)" HelpTextSettings-Position="Right" Width="75px" MinValue="0" MaxValue="365" runat="server" ShowIncrementButtons="False" SpinButtons-ShowLargeIncrementButtons="false" Paddings-Padding="0px">
                                                                    </dx:ASPxSpinEdit>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxCallbackPanel>

                                        </td>
                                    </tr>
                                <tr>
                                    <td colspan="2" style="text-align: right; padding: 5px 5px 5px 0px;">

                                        <div style="float: right; padding-top: 10px;">

                                            <asp:LinkButton ID="btPickToken" Visible="true" runat="server" Text="Pick Token"
                                                ToolTip="Pick Token" Style="padding-top: 10px;" PostBackUrl="javascript:" OnClientClick="return insertTextInHTMLEditorAtCursor();">
                                        <span class="button-bg">
                                         <b style="float: left; font-weight: normal;">
                                         Pick Token</b>
                                         <%--<i
                                        style="float: left; position: relative; top: 1px;left:2px">
                                        <img src="/Content/images/add_icon.png"  style="border:none;" title="" alt=""/></i> --%>
                                         </span>
                                            </asp:LinkButton>

                                            <span class="button-bg" onclick="hidepopup();">
                                                <b style="float: left; font-weight: normal;">Cancel</b>
                                                <%--<i style="float: left; position: relative; top: -2px; left: 2px">
                                                    <img src="/Content/ButtonImages/cancel.png" style="border: none;" title="" alt="" /></i>--%>
                                            </span>

                                        </div>
                                    </td>

                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>


