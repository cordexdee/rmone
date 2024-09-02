<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleEdit.ascx.cs" Inherits="uGovernIT.Web.ModuleEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register TagPrefix="ugit" Namespace="uGovernIT.Web" Assembly="uGovernIT.Web" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function showWiki() {
        <%--$("#<%=txtHelp.ClientID %>").show();
    
        $("#<%=fileupload.ClientID %>").hide();--%>
        
        LinkType.Set("LinkType", "WIKI");

    }

    function HideUploadLabel() {
        $("#<%=lblUploadedFile.ClientID %>").hide();
        $("#<%=fileUploadControl.ClientID %>").show();
        $("#<%=ImgEditFile.ClientID%>").hide();
        return false;
    }
    function showUploadControl() {
<%--        $("#<%=txtHelp.ClientID %>").hide();
        $("#<%=fileupload.ClientID %>").show()--%>
    }
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $('.modules-popupWrap').parent().addClass('modulesAddItem-popupContainer');
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
        $('.lookupValueBox-drpDwnRow').parent().parent().addClass('lookupValueBox-drpDwnRowWrap');
    });
</script>

<div class="col-md-12 col-sm-12 col-xs-12 noPadding modules-popupWrap my-2">
    <div class="ms-formtable accomp-popup ">
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Module Name<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <div>
                    <asp:TextBox ID="txtModuleName" runat="server" Enabled="false" />
                    <asp:RequiredFieldValidator ID="rfvtxtModuleName" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtModuleName"
                        ErrorMessage="Enter Module Name" Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr3" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Module Id<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <div>
                        <asp:TextBox ID="txtModuleId" runat="server" Enabled="false" />
                        <asp:RequiredFieldValidator ID="rfvtxtModuleId" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtModuleId"
                            ErrorMessage="Enter Module Id" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
             <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Short Name<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <div>
                        <asp:TextBox ID="txtShortName" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvtxtShortName" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtShortName"
                            ErrorMessage="Enter Short Name" Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        
       
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Last Sequence</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtLastSequence" runat="server" />
                    <asp:RegularExpressionValidator ID="regexLastSequence" ErrorMessage="Please enter integer >= 0" ControlToValidate="txtLastSequence" runat="server" ValidationExpression="^([0-9]+)$" ValidateEmptyText="true" ValidationGroup="Save" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Last Sequence Date</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <dx:ASPxDateEdit ID="dtcLastSequenceDate" runat="server" CssClass="CRMDueDate_inputField" DropDownButton-Image-Width="18px"
                        DropDownButton-Image-Url="~/Content/Images/calendarNew.png">
                    </dx:ASPxDateEdit>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4 col-sm-4 col-xs-4 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Module Type</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <ugit:LookUpValueBox ID="ddlmoduletype" runat="server" FieldName="ModuleType" 
                        CssClass="lookupValueBox-dropown"></ugit:LookUpValueBox>
                
                </div>
            </div>
            <div class="col-md-4 col-sm-4 col-xs-4 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Item Order</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtItemOrder" runat="server" />
                    <asp:RegularExpressionValidator ID="regexItemOrder" ErrorMessage="Enter positive integer" ControlToValidate="txtItemOrder" runat="server" ValidationExpression="^([0-9]+)$" ValidateEmptyText="true" ValidationGroup="Save" />
                </div>
            </div>
            <div class="col-md-4 col-sm-4 col-xs-4 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Alternate Item ID</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <dx:ASPxComboBox ID="cmbFieldName" runat="server" width="100%" CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox"
                        DropDownStyle="DropDownList" TextFormatString="{0}" ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0"
                        EnableSynchronization="True" CallbackPageSize="10" OnSelectedIndexChanged="cmbFieldName_SelectedIndexChanged" AutoPostBack="true">
                        <Columns>
                        </Columns>
                    </dx:ASPxComboBox>
                    
                </div>
            </div>
        </div>
        
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkEnable" runat="server" Text="Enable Module" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkEnableCache" runat="server" Text="Enable Cache" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkEnableEventReceivers" runat="server" Text="Enable User Statistics" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkEnableNewButtonOnHomePage" runat="server" AutoPostBack="false" Text="Enable NewTickets Button On Home Page"></asp:CheckBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkEnableTicketImport" runat="server" AutoPostBack="false" Text="Enable Item Import"></asp:CheckBox>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkKeepItemOpen" runat="server" Text="Keep Item Open" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkReturnCommentOptional" runat="server" Text="Return Comment Optional" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkAllowDraftMode" runat="server" Text="Allow Draft Mode" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkAuroApprove" runat="server" Text="Auto Approve" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkStoreTicketEmail" runat="server" Text="Store Ticket Email" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkUseInGlobalSearch" runat="server" Text="Use In Global Search" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkEnableLayout" runat="server" Text="Enable Layout" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkEnableQuickTicket" runat="server" Text="Enable Quick Item" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkEnableWorkflow" runat="server" Text="Enable Workflow" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkShowComment" runat="server" Text="Show Comment" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkShowNextSLA" runat="server" Text="Show Next SLA" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkSyncAppsToRequestType" runat="server" Text="Sync Apps To RequestType" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkAutoCreateLibrary" runat="server" Text="Auto Create Document Library" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkAllowChangeTicketType" runat="server" Text="Allow Change Item Type" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkAllowBatchEdit" runat="server" Text="Allow Batch Edit" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkAllowBatchCreate" runat="server" Text="Allow Batch Create" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkAllowReassign" runat="server" Text="Allow Reassign From List" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkAllowEscalation" runat="server" Text="Allow Escalation From List" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkShowTicketSummary" runat="server" Text="Show Item Summary" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkNotifyActionUsersOnComment" runat="server" Text="Notify Action Users on Comment" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkNotifyRequestorOnComment" runat="server" Text="Notify Requestor on Comment" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkNotifyInitiatorOnComment" runat="server" Text="Notify Initiator on Comment" />
                </div>
            </div>
            <%--<div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkNotifyActionUsersOnCancel" runat="server" Text="Notify Action Users on Reject/Cancel" />
                </div>
            </div>--%>
        </div>
        <%--<div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkNotifyRequestorOnCancel" runat="server" Text="Notify Requestor on Reject/Cancel" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkNotifyInitiatorOnCancel" runat="server" Text="Notify Initiator on Reject/Cancel" />
                </div>
            </div>
        </div>--%>

        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkWaitingOnMeIncludeGroups" runat="server" Text="Waiting On Me Includes Groups" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkWaitingOnMeExcludeResolved" runat="server" Text="Waiting On Me Excludes Resolved" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkShowBottleNeckChart" runat="server" Text="Show Bottleneck Chart" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkAllowTicketDelete" runat="server" Text="Allow Item Delete" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkAllowBatchClose" runat="server" Text="Allow Batch Close" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkHideWorkFlow" runat="server" Text="Hide WorkFlow" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkEnableRMMAllocation" runat="server" Text="Enable RMM Allocation" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkPreloadTabs" runat="server" Text="Preload All Tabs" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkActualHourUser" runat="server" Text="Allow Actual Hours By User" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkEnableCloseonHoldExpiration" runat="server" Text="Enable Close on Hold Expiration" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkModuleAgent" runat="server" Text="Enable Module Agent" />
                </div>
            </div>

            <div id="dvKeepTicketCount" runat="server" class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkKeepTicketCount" runat="server" Text="keep Item Counts" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkShowBaseline" runat="server" Text="Enable Baseline" />
                </div>
            </div>

            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkEnableLinkSimilarTickets" runat="server" Text="Enable Link Similar Items" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkShowTasksInProjectTasks" runat="server" Text="Show Tasks In Project Tasks" />
                </div>
            </div>

            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkEnableIcon" runat="server" Text="Enable Icon" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="IsAllocationTypeHard_Soft" runat="server" Text="Default Allocation Soft" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Enable User Statistics" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkEnableAddNewButton" runat="server" AutoPostBack="false" Text="Enable Add New  Button"></asp:CheckBox>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Owner Binding</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <ugit:LookUpValueBox ID="ddlownerbinding" runat="server" FieldName="OwnerBindingChoice" CssClass="lookupValueBox-dropown"></ugit:LookUpValueBox>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Hold Max Stage</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtHoldMaxStage" runat="server" />
                    <asp:RegularExpressionValidator ID="regextxtHoldMaxStage" ErrorMessage="Only numberic allow." ControlToValidate="txtHoldMaxStage" runat="server"
                        ValidationExpression="^([0-9]+)$" ValidateEmptyText="true" ValidationGroup="Save" />
                </div>
            </div>
        </div>
        <div class="row">
             <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                  <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Open Item Chart</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtOpentTicketChart" runat="server" />
                </div>
             </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Close Item Chart</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtCloseTicketChart" runat="server" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Theme Color</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlThemeColor" runat="server" Enabled="false" CssClass="itsmDropDownList aspxDropDownList">
                        <asp:ListItem Text="Accent1" Value="Accent1"></asp:ListItem>
                        <asp:ListItem Text="Accent2" Value="Accent2"></asp:ListItem>
                        <asp:ListItem Text="Accent3" Value="Accent3"></asp:ListItem>
                        <asp:ListItem Text="Accent4" Value="Accent4"></asp:ListItem>
                        <asp:ListItem Text="Accent5" Value="Accent5"></asp:ListItem>
                        <asp:ListItem Text="Accent6" Value="Accent6"></asp:ListItem>
                     <asp:ListItem Text="Accent1" Value="Accent1"></asp:ListItem>
                     <asp:ListItem Text="Accent2" Value="Accent2"></asp:ListItem>
                        <asp:ListItem Text="Accent3" Value="Accent3"></asp:ListItem>
                        <asp:ListItem Text="Accent4" Value="Accent4"></asp:ListItem>
                        <asp:ListItem Text="Accent5" Value="Accent5"></asp:ListItem>
                        <asp:ListItem Text="Accent6" Value="Accent6"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
             <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Item Table</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtTicketTable" runat="server" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Relative Path</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtRelativePath" runat="server" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Static Module Page Path</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtStaticModule" runat="server" />
                </div>
            </div>
        </div>
         <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Category Name</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtCategoryName" runat="server" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Auto Refresh Frequency</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <dx:ASPxSpinEdit runat="server" ID="spinedit" Width="100%" CssClass="aspxSpinEdit-dropDown" NumberType="Integer" MaxValue="50000000" MinValue="0"></dx:ASPxSpinEdit>
                </div>
            </div>
        </div>
         <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Authorized To View</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <ugit:UserValueBox ID="ppeAuthorizedToView" runat="server" CssClass="userValueBox-dropDown"></ugit:UserValueBox>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Authorized To Create</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <ugit:UserValueBox ID="ppeAuthorizedToCreate" runat="server" CssClass="userValueBox-dropDown"></ugit:UserValueBox>
                </div>
            </div>
        </div>
        <div class="row" id="tr1" runat="server">
            <div class="col-md-12 col-sm- col-xs-12 noPadding" >
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server" />
                </div>
            </div>
        </div>
         <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr5" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Custom Properties</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtCustomProperties" TextMode="MultiLine" runat="server" />
                </div>
            </div>
             <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader">Link Type</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlTargetType" CssClass="target_section itsmDropDownList aspxDropDownList" Width="100%"
                        runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTargetType_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
        </div>
       
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trFileUpload" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader">File</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:Label ID="lblUploadedFile" runat="server"></asp:Label>
                    <asp:FileUpload ID="fileUploadControl" CssClass="fileUploader" Width="200px" ToolTip="Browse and upload file" runat="server" Style="display: none;" />
                    <img alt="Edit File" title="Edit File" runat="server" id="ImgEditFile" src="/content/Images/editNewIcon.png" style="cursor: pointer; width:16px;"
                        onclick="HideUploadLabel();" />
                    <div>
                        <asp:RequiredFieldValidator ID="rfvFileUpload" CssClass="rfvdFileUploader" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="fileUploadControl" ErrorMessage="Upload a file." Display="Dynamic" ValidationGroup="fileSave"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trLink" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader">Link URL</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtFileLink" CssClass="fileUploaderLink" runat="server" Width="386px" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trWiki" runat="server">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader">Select Wiki</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtWiki" runat="server" Width="290" />
                    <a id="aAddItem" runat="server" style="cursor: pointer;">
                        <img alt="Add Wiki" title="Add Wiki" runat="server" id="imgWiki" src="/content/Images/editNewIcon.png" style="cursor: pointer; width:16px;" />
                    </a>                    
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trHelpCard" runat="server">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader">Select Help Card</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtHelpCard" runat="server" Width="290" />
                    <a id="aAddHelpCard" runat="server" style="cursor: pointer;">
                        <img alt="Add Help Card" title="Add Help Card" runat="server" width="16" id="img" src="/content/Images/editNewIcon.png" style="cursor: pointer;" />
                    </a>
                </div>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap " id="tr2" runat="server" style="padding-bottom: 7px">
            <div class="">
                <dx:ASPxButton ID="btnApplyChanges" runat="server" CssClass="primary-blueBtn mr-1" Text="Apply Changes" ToolTip="Apply Changes" OnClick="btnApply_Click">
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" ToolTip="Cancel" OnClick="btnCancel_Click"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" CssClass="primary-blueBtn" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click"></dx:ASPxButton>
            </div>
            <div class="" style="margin-right:10px;">
                
            </div>
        </div>
    </div>
</div>
