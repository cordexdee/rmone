<%@ Control Language="C#" AutoEventWireup="true" ValidateRequestMode="Disabled" CodeBehind="ModuleStageEdit.ascx.cs" Inherits="uGovernIT.Web.ModuleStageEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formlabel{
        padding:0;
        width: 160px;
    }
    .ms-standardheader.budget_fieldLabel{
        float:right !important;
        margin-top:-12px;
        margin-bottom:0;
    }
    .chkbox input[type="checkbox"]{
        margin: 6px 0px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function OpenConditionPicker() {
        var Url = '<%= SkipConditionUrl%>';
        if (hdnSkipOnCondition.Get("SkipCondition") != undefined) {
            Url += "&SkipCondition=" + escape(hdnSkipOnCondition.Get("SkipCondition"));
        }
        javascript: UgitOpenPopupDialog(Url, '', 'Skip Rule', '85', '90', 0, '');
    }

    function OnEndCallback(s, e) {
        ResetGridTr();
    }

    $(function () {
        ResetGridTr();
    });

    function ResetGridTr() {
        $(".stagesctionusers tr").each(function () {

            if ($(this).css('display') == 'none') {
                $(this).css('display', '');
            }
        });
    }

</script>

<div class="configVariable-popupWrap" style="float: left; width: 100%;">
    <asp:Panel runat="server" ID="lifecycleForm" CssClass="full-width">
        <div class="full-width">
            <asp:Label ID="lbMessage" runat="server" ForeColor="Green"></asp:Label>
        </div>

        <div class="full-width" style="height: 100px">
            <asp:Panel ID="lcsgraphics" runat="server" CssClass="lcsgraphics">
            </asp:Panel>
        </div>

        <div id="dvBasicFields" class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
                <tbody>
                    <tr id="trTitle" runat="server">
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtTitle" Width="100%" CssClass="asptextbox-asp" runat="server" ValidationGroup="Task"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ValidationGroup="Task" ControlToValidate="txtTitle"
                                Display="Dynamic" ErrorMessage="Please enter title." ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvTitle" ValidationGroup="Task" runat="server" ControlToValidate="txtTitle" ErrorMessage="Stage with this name already exists"
                                OnServerValidate="cvTitle_ServerValidate" Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
                        </td>
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Step<b style="color: Red;">*</b></h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                            <asp:TextBox CssClass="asptextbox-asp" ID="txtStep" Width="100%" runat="server" ValidationGroup="Task"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvStep" runat="server" ValidationGroup="Task" ControlToValidate="txtStep" Display="Dynamic" ErrorMessage="Please enter Step"
                             ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvStep" ValidationGroup="Task" runat="server" ControlToValidate="txtStep" MinimumValue="1" MaximumValue="100"
                            ErrorMessage="Step number must be between 1-100." Display="Dynamic" Type="Integer" ForeColor="Red"></asp:RangeValidator>
                            <asp:CustomValidator ID="cvStep" ValidationGroup="Task" runat="server" ControlToValidate="txtStep" ErrorMessage="Step Number is already assigned to other stage."
                            OnServerValidate="cvStep_ServerValidate" Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
                        </td>
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Weight</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                            <asp:TextBox CssClass="asptextbox-asp" Width="100%"  ID="txtWeight" runat="server" ValidationGroup="Task"></asp:TextBox>
                            <asp:RegularExpressionValidator ValidationGroup="Task" ID="revWeight" Display="Dynamic" runat="server" ControlToValidate="txtWeight" ValidationExpression="[0-9]*"
                                ErrorMessage="Weight must be a positive whole number."></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Normal Capacity</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                             <asp:TextBox CssClass="asptextbox-asp" Width="100%" ID="txtCapacityNormal" runat="server" ValidationGroup="Task"></asp:TextBox>
                            <asp:RegularExpressionValidator ValidationGroup="Task" ID="RegularExpressionValidator1" Display="Dynamic" runat="server"
                                ControlToValidate="txtCapacityNormal" ValidationExpression="[0-9]*"
                                ErrorMessage="Normal Capacity must be a positive whole number."></asp:RegularExpressionValidator>
                        </td>
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Max Capacity</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                            <asp:TextBox CssClass="asptextbox-asp" Width="100%" ID="txtCapacityMax" runat="server" ValidationGroup="Task"></asp:TextBox>
                            <asp:RegularExpressionValidator ValidationGroup="Task" ID="RegularExpressionValidator2" Display="Dynamic" runat="server"
                                ControlToValidate="txtCapacityMax" ValidationExpression="[0-9]*"
                                ErrorMessage="Max Capacity must be a positive whole number."></asp:RegularExpressionValidator>
                        </td>
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Icon Url</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                             <ugit:UGITFileUploadManager ID="UGITFileUploadManager1" width="100%" runat="server"
                             AnchorLabel="Upload Icon" hideWiki="true" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding" style="padding-top:30px;">
            <table class="ms-formtable accomp-popup" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
                <tbody>
                    <tr>
                        <td class="ms-formlabel">
                             <h3 class="ms-standardheader budget_fieldLabel">Action User(s)</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                            <dx:ASPxGridLookup  Visible="true" Width="100%" CssClass="stagesctionusers aspxGridLookUp-dropDown"  
                                TextFormatString="{3}" SelectionMode="Multiple" ID="glActionUser" runat="server"
                                KeyFieldName="Name" multitextseparator="; " DropDownWindowStyle-CssClass="aspxGridLookup-dropDown">
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="Type" GroupIndex="0"  Visible="false"></dx:GridViewDataTextColumn>
                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="28px" />
                                    <dx:GridViewDataTextColumn FieldName="NameRole" Width="300px" Caption="Choose Action Users:">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="Type" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="Role" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                </Columns>

                                <GridViewProperties>
                                    <Settings ShowGroupedColumns="false" ShowFilterRow="true" VerticalScrollBarMode="Auto" />
                                    <SettingsBehavior AllowSort="false" AllowGroup="true" AutoExpandAllGroups="true" />
                                    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                </GridViewProperties>
                                <ClientSideEvents EndCallback="OnEndCallback" />
                            </dx:ASPxGridLookup>
                        </td>
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Action</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                            <asp:TextBox CssClass="asptextbox-asp" Width="100%" ID="txtAction" runat="server" ValidationGroup="Task"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Data Editor(s)</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                             <dx:ASPxGridLookup  Visible="true" CssClass="stagesctionusers aspxGridLookUp-dropDown" Width="100%" TextFormatString="{3}" SelectionMode="Multiple" ID="glDataEditors" runat="server" KeyFieldName="Name" MultiTextSeparator="; ">
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="Type" GroupIndex="0" SortOrder="Descending" Visible="false"></dx:GridViewDataTextColumn>
                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="28px" />
                                    <dx:GridViewDataTextColumn FieldName="NameRole" Width="300px" Caption="Choose ActionUsers:">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="Type" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="Role" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <GridViewProperties>
                                    <Settings ShowGroupedColumns="false" ShowFilterRow="true" VerticalScrollBarMode="Auto" />
                                    <SettingsBehavior AllowSort="false" AllowGroup="true" AutoExpandAllGroups="true" />
                                    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                </GridViewProperties>
                                <ClientSideEvents EndCallback="OnEndCallback" />
                            </dx:ASPxGridLookup>
                        </td>
                         <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Default Selected Tab</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                            <asp:DropDownList ID="ddlSelectedTabs" Width="100%" CssClass="itsmDropDownList aspxDropDownList" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Stage Type</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                            <ugit:LookUpValueBox ID="ddlStageType" Width="100%" runat="server" FieldName="StageTypeChoice" CssClass="lookupValueBox-dropown" 
                            AllowNull="true"></ugit:LookUpValueBox>
                        </td>
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">User Workflow Status</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                            <asp:TextBox CssClass="asptextbox-asp" Width="100%" ID="txtUserWorkflowStatus" runat="server" ValidationGroup="Task"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                         <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">User Prompt</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtUserPrompt" Width="100%" runat="server" ValidationGroup="Task"></asp:TextBox>
                        </td>
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Short Stage Title</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                            <asp:TextBox  ID="txtShortStageTitle" Width="100%" runat="server" ValidationGroup="Task"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Skip On Condition</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                             <asp:Label Width="100%" ID="lblSkipOnCondition" CssClass="skipcondition text-right" runat="server"></asp:Label>
                            <img id="Img1" runat="server" src="/Content/Images/editNewIcon.png" width="16"
                                onclick="OpenConditionPicker();" style="cursor: pointer;margin-top: -28px;" />
                            <dx:ASPxHiddenField ID="hdnSkipOnCondition" runat="server" ClientInstanceName="hdnSkipOnCondition"></dx:ASPxHiddenField>
                        </td>
                        <td class="ms-formlabel" id="trdisablelbl" runat="server">
                            <h3 class="ms-standardheader budget_fieldLabel">Disable Auto-Approve</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField" id="trdisablechk" runat="server" style="margin-bottom: 18px;">
                            <asp:CheckBox ID="chkDisableAutoApprove" runat="server" />
                        </td>
                    </tr>
                    <tr id="trFileUpload" runat="server">
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Link Type</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                            <asp:DropDownList ID="ddlTargetType" Width="100%" CssClass="itsmDropDownList aspxDropDownList"
                                runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTargetType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                         <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">File</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField" style="margin-bottom: 18px;">
                            <asp:Label ID="lblUploadedFile" runat="server"></asp:Label>
                            <asp:FileUpload ID="fileUploadControl" CssClass="fileUploader" ToolTip="Browse and upload file" runat="server" Style="display: none;" />
                            <img alt="Edit File" title="Edit File" runat="server" id="ImgEditFile" src="/content/Images/editNewIcon.png" width="16" style="cursor: pointer;" onclick="HideUploadLabel();" />
                            <div>
                                <asp:RequiredFieldValidator ID="rfvFileUpload" CssClass="rfvdFileUploader" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="fileUploadControl" ErrorMessage="Upload a file." Display="Dynamic" ValidationGroup="fileSave"></asp:RequiredFieldValidator>
                            </div>
                        </td>
                    </tr>
                    <tr id="trLink" runat="server">
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Link URL</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtFileLink" CssClass="fileUploaderLink" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr id="trWiki" runat="server">
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Select Wiki</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtWiki"  runat="server" Width="100%" />
                            <a id="aAddItem" runat="server" style="cursor: pointer;">
                                <img alt="Add Wiki" title="Add Wiki" runat="server" id="imgWiki" width="16" 
                                    src="/content/Images/editNewIcon.png" style="cursor: pointer;" />
                            </a>                    
                        </td>
                    </tr>
                    <tr id="trHelpCard" runat="server">
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Select Help Card</h3>
                        </td>
                        <td class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtHelpCard" runat="server" Width="100%" />
                            <a id="aAddHelpCard" runat="server" style="cursor: pointer;">
                                <img alt="Add Help Card" title="Add Help Card" runat="server" id="img" width="16" 
                                    src="/content/Images/editNewIcon.png" style="cursor: pointer;" />
                            </a>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div class="col-md-12 col-sm-12 col-xs-12 noPadding" style="padding-top: 30px; min-height: 110px">
            <div style="float: left; width: 33%; padding-right: 2px">
                <table class="ms-formtable accomp-popup" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
                    <tbody>
                        <tr id="tr1" runat="server">
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Approved Status</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:DropDownList ID="ddlApprovedStatus" Width="100%" CssClass=" itsmDropDownList aspxDropDownList" 
                                    runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Approve Button Name</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtApproveBtnName" Width="100%" runat="server" ValidationGroup="Task"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Approve Icon URL</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtApprovedIcon" Width="100%" CssClass="" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Approve Action Description</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtApproveActionDesc" runat="server" ValidationGroup="Task"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Approve Action Tooltip</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtApproveActionTooltip" runat="server" ValidationGroup="Task"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">All Approvals Required</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:CheckBox ID="chkAllApporvalRequired" runat="server" TextAlign="Right" CssClass="chkbox" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div style="float: left; width: 33%; padding-right: 2px">
                <table class="ms-formtable accomp-popup" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
                    <tbody>
                        <tr id="tr2" runat="server">
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Return Status</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:DropDownList ID="ddlReturnStatus" CssClass="itsmDropDownList aspxDropDownList" 
                                    runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Return Button Name</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtReturnBtnName" runat="server" ValidationGroup="Task"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Return Icon URL</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtReturnIcon" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Return Action Description</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtReturnActionDesc" runat="server" ValidationGroup="Task"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Return Action Tooltip</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtReturnActionTooltip" runat="server" ValidationGroup="Task"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="tr5" runat="server">
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Return to Any Stage</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                 <asp:CheckBox ID="chkReturnAnyStage" runat="server" Text="" CssClass="chkbox" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div style="float: left; width: 33%;">
                <table class="ms-formtable accomp-popup" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
                    <tbody>
                        <tr id="tr3" runat="server">
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Rejected Status</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:DropDownList ID="ddlRejectedStatus" CssClass=" itsmDropDownList aspxDropDownList"
                                    runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                         <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Reject Button Name:</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtRejectBtnName" runat="server" ValidationGroup="Task"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Reject Icon URL:</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtRejectIcon" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Reject Action Description</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtRejectActionDesc" Width="100%" runat="server" ValidationGroup="Task"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Reject Action Tooltip</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtRejectActionTooltip" runat="server" ValidationGroup="Task"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h3 class="ms-standardheader budget_fieldLabel">Auto Approve On Stage Tasks</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:CheckBox ID="chkAutoApproveOnStageTasks" Text="" runat="server" CssClass="chkbox"  />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
          
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding" style="padding-top:1px">
            <div style="float: left; width: 33%; padding-right: 2px">
                <table class="ms-formtable accomp-popup" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%"">
                    <tbody>
                        <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Allow Reassign</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:CheckBox ID="chkAllowReassignFromList" Text="" runat="server" CssClass="chkbox" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div style="float: left; width: 66%; padding-right: 2px">
                <table class="ms-formtable accomp-popup" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%"">
                    <tbody>
                        <tr>
                            <td class="ms-formlabel" width="25%">
                                <h3 class="ms-standardheader budget_fieldLabel">Custom Properties</h3>
                            </td>
                            <td class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtCustomProperties" runat="server" ValidationGroup="Task"></asp:TextBox>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12">
            <div class="d-flex justify-content-between align-items-center">
                <dx:ASPxButton ID="LnkbtnDelete" runat="server" CssClass="btn-danger1" Text="Delete" ToolTip="Delete" OnClick="LnkbtnDelete_Click">
                    <ClientSideEvents Click=" function(s, e) {e.processOnServer = confirm('Are you sure?');}" />
                </dx:ASPxButton>
                <div>
                    <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
                    <dx:ASPxButton ValidationGroup="Task" ID="btSaveLifeCycleStage" Visible="true" runat="server" Text="Save" ToolTip="Save as Template" CssClass="primary-blueBtn"  OnClick="btSaveLifeCycleStage_Click"></dx:ASPxButton>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>
