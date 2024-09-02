<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScheduleActionsEdit.ascx.cs" Inherits="uGovernIT.Web.ScheduleActionsEdit" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%--<%@ Register Src="~/_CONTROLTEMPLATES/15/uGovernIT/WeeklyPrfReportFilter.ascx" TagPrefix="uGovernIT" TagName="WeeklyPrfReportFilter" %>--%>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxSpellChecker" Assembly="DevExpress.Web.ASPxSpellChecker.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%--<%@ Register Src="~/_CONTROLTEMPLATES/15/uGovernIT/NonPeakHrPrfReportFilters.ascx" TagPrefix="uGovernIT" TagName="NonPeakHrPrfReportFilters" %>
<%@ Register Src="~/_CONTROLTEMPLATES/15/uGovernIT/SummaryByTechnicianFilters.ascx" TagPrefix="uGovernIT" TagName="SummaryByTechnicianFilters" %>--%>
<%--<SharePoint:ScriptLink ID="ScriptLink5" runat="server" Name="/_layouts/15/uGovernIT/JS/jquery-ui.min.js"
    Language="javascript">
</SharePoint:ScriptLink>--%>

<%--<SharePoint:CssLink ID="CssLink2" runat="server">
    <%--<SharePoint:CssRegistration ID="CssRegistration2" runat="server" Name="/_layouts/15/uGovernIT/JS/jquery-ui/jquery-ui-1.10.3.css">
    </SharePoint:CssRegistration>
</SharePoint:CssLink>--%>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .dxeRadioButtonList_UGITNavyBlueDevEx td.dxe {
        padding: 0;
    }
    .dxeCaptionCell_UGITNavyBlueDevEx {
        color: #4A6EE2;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(function () {
        $("#<%= ddlTypeofReport.ClientID %>").bind("change", function () {
            //callSelectionCriteria($(this).val());
            return false;
        });
        //callSelectionCriteria($("#<%= ddlTypeofReport.ClientID %>").val());
        bindQueryPreview();

        ///case Reminder if ticket id add to a Schedule action
        var ticketid = $("#<%= txtTicketId.ClientID %>").val();
        if (ticketid != null && ticketid.length > 0) {
            //$('#aticket').unbind("click");
            var url = hdnConfiguration.Get('TicketUrl');
            $('#aticket').html(ticketid);
            $('#aticket').attr("href", "javascript:OpenTicketWindow('" + url + "', '" + ticketid + "');");
        }
    });
    function checkValidationGroup(valGrp) {
        var rtnVal = true;
        for (i = 0; i < Page_Validators.length; i++) {
            if (Page_Validators[i].validationGroup == valGrp) {
                ValidatorValidate(Page_Validators[i]);
                if (!Page_Validators[i].isvalid) { //at least one is not valid.
                    rtnVal = false;
                    //  break; //exit for-loop, we are done.
                }
            }
        }
        return rtnVal;
    }

    function callSelectionCriteria(value) {
        $("#<%= abutton.ClientID %>").unbind("click");
        switch (value) {
            case "TicketSummary":
                ticketsummaryselector();
                break;
            case "ProjectReport":
                projectreportselector();
                break;
            case "TSKProjectReport":
                tskprojectreportselector();
                break;
            case "TaskSummary":
                tsksummaryselector();
                break;
            case "GanttView":
                ganttview();
                break;
            case "WeeklyTeamReport":
                ShowWeeklyTeamPerformance();
                break;
            case "NonPeakHoursPerformance":
                ShowNonPeakHrPrf();
                break;
            case "SummaryByTechnician":
                ShowTicketReportsByPRPPopup();
                break;
            case "SurveyFeedbackReport":
                ShowSurveyFeedbackReport();
                break;
            default:
                break;
        }
    }

    function ShowTicketReportsByPRPPopup(s) {
        var url = hdnConfiguration.Get("SummaryByTechnician");
        var params = "";
        var popupHeader = "Ticket Summary By Technician";
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        window.parent.UgitOpenPopupDialog(url, params, popupHeader, '60', '70', 0, escape(requestUrl));
        return false;
    }

    function ShowWeeklyTeamPerformance() {
        var url = hdnConfiguration.Get("WeeklyTeamReport");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        var params = "";
        window.parent.UgitOpenPopupDialog(url, params, 'Weekly Team Report (TSR)', '600px', '300px', 0, escape(requestUrl));
        return false;
    }

    function ShowNonPeakHrPrf() {
        var url = hdnConfiguration.Get("NonPeakHoursPerformance");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        var param = "";
        window.parent.UgitOpenPopupDialog(url, param, 'Non-Peak Hours Performance (TSR)', '500px', '300px', 0, escape(requestUrl));
        return false;
    }

    function bindQueryPreview() {
        $('.preview').unbind('click');
        $('.preview').bind('click', function () { previewQuery(); });
    }

    function ticketsummaryselector() {
        var url = hdnConfiguration.Get("TicketSummary");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        window.parent.UgitOpenPopupDialog(url, '', 'Ticket Summary', '600px', '300px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function projectreportselector() {
        var url = hdnConfiguration.Get("ProjectReport");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        var param = "&Module=PMM";
        window.parent.UgitOpenPopupDialog(url, param, 'Project Report', '90', '95', 0, escape(requestUrl));
        return false;
    }

    function tskprojectreportselector() {
        var url = hdnConfiguration.Get("TSKProjectReport");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        var param = "&Module=TSK";
        window.parent.UgitOpenPopupDialog(url, param, 'Project Report', '80', '90', 0, escape(requestUrl));
        return false;
    }

    function tsksummaryselector() {
        var url = hdnConfiguration.Get("TaskSummary");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        window.parent.UgitOpenPopupDialog(url, '', 'Task Summary', '830px', '90', 0, escape(requestUrl));
        return false;
    }

    function ganttview() {
        var url = hdnConfiguration.Get("GanttView");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        window.parent.UgitOpenPopupDialog(url, '', 'Gantt View', '600px', '300px', 0, escape(requestUrl));
        return false;
    }

    function previewQuery() {
        if (cmbQuery.GetSelectedItem() != null && cmbQuery.GetSelectedItem().value != '') {
            var url = hdnConfiguration.Get("QueryPreview");
            var requestUrl = hdnConfiguration.Get("RequestUrl");
            var whereFilter = hdnfield.Get("Where");
            var param = '&ItemId=' + cmbQuery.GetSelectedItem().value + '&whereFilter=' + whereFilter;
            window.parent.UgitOpenPopupDialog(url, param, 'Query Preview', '90', '90', 0, escape(requestUrl));
        }
        else {
            alert('Please select valid query');
        }
        return false;
    }

    function SetValuesForQuery() {

        $('#<%= hdnControls.ClientID%>').val('true')
        hdnfield.Set("QueryId", cmbQuery.GetSelectedItem().value);
    }

    function OpenTicketWindow(ticketurl, ticketId) {
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        var param = '';
        window.parent.UgitOpenPopupDialog(ticketurl, param, 'Ticket - ' + ticketId, '90', '90', 0, escape(requestUrl));
    }

    var lastModule = null;
    function OnmoduleChange(cmbmodule) {
        Loadingpanel.Show();
        if (cmbtemplate.InCallback())
            lastModule = cmbmodule.GetValue().toString();
        else
            cmbtemplate.PerformCallback(cmbmodule.GetValue().toString());
    }
    function OnEndCallback(s, e) {
        Loadingpanel.Hide();
        if (lastModule) {
            cmbtemplate.PerformCallback(lastModule);
            lastModule = null;
        }
    }

    function ShowSurveyFeedbackReport() {
        var url = hdnConfiguration.Get("SurveyFeedbackReport");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        var param = '';
        window.parent.UgitOpenPopupDialog(url, '', 'Survey Feedback Report Filter', '600px', '300px', 0, escape(requestUrl));
    }

    function DeleteScheduleActions() {
        if (confirm('Are you sure want to delete?')) {
            <%=Page.ClientScript.GetPostBackEventReference(lnkDelete1,string.Empty)%>
        }
    }

    function Validation() {
        debugger;
       <%-- var val = $("#<%= ddlTypeofReport.ClientID %>").val();
        callSelectionCriteria(val);--%>

        var scheduleActionID = <%= requestId%>;
        if (parseInt(scheduleActionID) > 0) {
            var val = $("#<%= ddlTypeofReport.ClientID %>").val();
            callSelectionCriteria(val);
        }
        else {
            DevExpress.ui.notify({
                message: "Please save the schedule action for report and then select criteria.",
                type: "error",
                width: 400,
                position: "{ my: 'center', at: 'center', of: window }",
            });
            return false;
        }

    }
</script>
<dx:ASPxLoadingPanel ID="Loadingpanel" ClientInstanceName="Loadingpanel" Modal="True" runat="server" Text="Please Wait..."></dx:ASPxLoadingPanel>
<dx:ASPxHiddenField ID="hdnConfiguration" runat="server" ClientInstanceName="hdnConfiguration"></dx:ASPxHiddenField>
<dx:ASPxHiddenField ID="hdnfield" runat="server" ClientInstanceName="hdnfield"></dx:ASPxHiddenField>
<asp:HiddenField ID="hdnControls" runat="server" />
<dx:ASPxHiddenField ID="hdnSchdeuleId" runat="server" ClientInstanceName="hdnSchdeuleId"></dx:ASPxHiddenField>
<asp:Panel ID="pnlcontrol" runat="server"></asp:Panel>

<div class="py-3">
    <div class="configVariable-popupWrap">
        <asp:Label ID="lblMsg" Text="" runat="server" CssClass="error" />
        <%--  <fieldset>
        <legend class="admin-legendLabel">Schedule</legend>--%>
        <div class="ms-formtable accomp-popup">
            <div class="row bs">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Schedule Action Type<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField px-0">
                        <asp:DropDownList ID="ddlScheduleActionType" runat="server" CssClass="itsmDropDownList aspxDropDownList" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlScheduleActionType_SelectedIndexChanged">
                            <asp:ListItem Text="Alert" />
                            <asp:ListItem Text="Query" Selected="True" />
                            <asp:ListItem Text="Reminder" />
                            <asp:ListItem Text="Report" />
                            <asp:ListItem Text="Schedule Ticket" Value="ScheduledTicket"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row" id="trmodule" runat="server">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Select Module<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField px-0">
                        <dx:ASPxComboBox ID="cmbmodule" runat="server" Width="100%" DropDownStyle="DropDownList" TextField="Title" ValueField="ModuleName" EnableSynchronization="False"
                            ClientInstanceName="cmbmodule" CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox">
                            <ClientSideEvents SelectedIndexChanged="function(s,e){OnmoduleChange(s);}" />
                        </dx:ASPxComboBox>
                    </div>
                </div>
            </div>
            <div class="row" id="trtemplate" runat="server">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Select Template<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField px-0">
                        <dx:ASPxComboBox ID="cmbtemplate" Width="100%" CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox" DropDownStyle="DropDown" OnCallback="cmbmodule_Callback" TextField="Title"
                            ValueField="ID" runat="server" EnableSynchronization="False" ClientInstanceName="cmbtemplate">
                            <ClientSideEvents EndCallback=" OnEndCallback" />
                        </dx:ASPxComboBox>
                    </div>
                </div>
            </div>
            <!--Start Schedule action Query Related Table Rows-->
            <div class="row bs" runat="server" id="trQuery" visible="false">
                <div class="ms-formlabel" style="padding-left: 15px;">
                    <h3 class="ms-standardheader budget_fieldLabel">Query<b style="color: Red;">*</b></h3>
                </div>
                <div class="d-flex align-items-center flex-wrap ms-formbody accomp_inputField px-0">
                    <div class="col-md-10 col-sm-10 col-xs-10">
                        <dx:ASPxComboBox ID="cmbQuery" runat="server" Width="100%" DropDownStyle="DropDownList" OnLoad="cmbQuery_Load" CssClass="aspxComBox-dropDown"
                            ValueField="Id" TextField="Title" TextFormatString="{0}" ValueType="System.String" ListBoxStyle-CssClass="aspxComboBox-listBox"
                            IncrementalFilteringMode="Contains" FilterMinLength="0" EnableSynchronization="True" AutoPostBack="true"
                            CallbackPageSize="10" ClientInstanceName="cmbQuery" OnSelectedIndexChanged="cmbQuery_SelectedIndexChanged">
                            <Columns>
                            </Columns>
                            <ClientSideEvents SelectedIndexChanged="function(s, e) { bindQueryPreview(); SetValuesForQuery(); }" />
                        </dx:ASPxComboBox>
                    </div>
                    <div class="col-md-2 col-sm-2 col-xs-2 ">
                        <a class="action-btn" style="cursor: pointer">Preview</a>
                    </div>
                </div>
            </div>
            <div class="row bs" runat="server" id="trParameter" visible="false">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Parameters</h3>
                    </div>
                    <div id="tdParameter" runat="server" class="ms-formbody accomp_inputField px-0" style="color: red"></div>
                </div>
            </div>
            <!--End Schedule action Query Related Table Rows-->
            <!--Start Schedule action Alert Related Table Rows-->
            <div class="row bs" runat="server" id="trCondition" visible="false">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Condition (e.g. '[Value]>0')</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField px-0">
                        <asp:TextBox ID="txtCondition" runat="server" />
                    </div>
                </div>
            </div>
            <!--End Schedule action Alert Related Table Rows-->
            <!--Start Schedule action Reminder Related Table Rows-->
            <div class="row bs" runat="server" id="trTicketPicker" visible="false">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Ticket</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField px-0">
                        <a id="aticket"></a>
                        <input runat="server" type="hidden" name="ticketId" id="txtTicketId" value="" />
                        <span style="float: left;">
                            <img id="aTicket" style="cursor: pointer; width: 16px;" runat="server" src="/content/images/editNewIcon.png" title="Select Ticket" /></span>
                    </div>
                </div>
            </div>
            <!--End Schedule action Reminder Related Table Rows-->
            <!--Start Schedule action Report Related Table Rows-->
            <div class="row bs" runat="server" id="trReportType" visible="false">
                <div class="ms-formlabel" style="padding-left: 15px;">
                    <h3 class="ms-standardheader budget_fieldLabel">Report Type<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField px-0">
                    <div class="col-md-6 col-sm-6 col-xs-8">
                        <asp:DropDownList ID="ddlTypeofReport" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <dx:ASPxButton ID="abutton" runat="server" CssClass="selectlist criteria primary-blueBtn fright" Text="Select Criteria" AutoPostBack="false" ClientInstanceName="abutton">
                            <ClientSideEvents Click="function(s,e){Validation()}" />
                        </dx:ASPxButton>
                    </div>
                </div>
            </div>
            <!--End Schedule action Reminder Related Table Rows-->
            <div class="row bs" runat="server" id="trAttachformat" visible="false">
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Attachment Format</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField px-0">
                        <asp:DropDownList ID="ddlAttachFormat" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                            <asp:ListItem Text="Excel" Value="xls" />
                            <asp:ListItem Text="Pdf" Value="pdf" />
                            <asp:ListItem Text="CSV" Value="csv" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">File Location</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField px-0">
                        <asp:DropDownList ID="ddlFileLocation" CssClass="clsattachement" runat="server" OnSelectedIndexChanged="ddlFileLocation_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="Email Attachment" Value="Email Attachment" Selected="True" />
                            <asp:ListItem Text="FTP" Value="FTP" />
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row bs" id="dvftp1" runat="server" visible="false">
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">FTP Url<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField px-0">
                        <asp:TextBox ID="txtftpurl" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvftpurl" ErrorMessage="Enter FTP URL." Display="Dynamic" ForeColor="Red" ControlToValidate="txtftpurl" runat="server" ValidationGroup="Save" />
                    </div>

                </div>
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">File Name<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField px-0">
                        <asp:TextBox ID="txtFileName" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Enter File Name." Display="Dynamic" ForeColor="Red" ControlToValidate="txtFileName" runat="server" ValidationGroup="Save" />
                    </div>
                </div>

            </div>
            <div class="row bs" id="dvftp2" runat="server" visible="false">
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">User Name<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField px-0">
                        <asp:TextBox ID="txtuserName" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvUserName" ErrorMessage="Enter User Name." ForeColor="Red" Display="Dynamic" ControlToValidate="txtuserName" runat="server" ValidationGroup="Save" />
                    </div>

                </div>
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Password<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField px-0">
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="rfvPassword" ErrorMessage="Enter Password." Display="Dynamic" ControlToValidate="txtPassword" ForeColor="Red" runat="server" ValidationGroup="Save" />
                    </div>
                </div>
            </div>
            <!--Start Common fields Table Rows-->
            <div class="row bs">
                <div class="col-md-6 col-sm-6 col-xs-6" id="trTitle" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField px-0">
                        <asp:TextBox ID="txtScheduleTitle" runat="server" />
                        <div>
                            <asp:RequiredFieldValidator ID="rfvScheduleTitle"
                                Enabled="true" runat="server" ControlToValidate="txtScheduleTitle"
                                ErrorMessage="Enter Title " Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Start Time<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField px-0">
                        <dx:ASPxDateEdit ID="dtcScheduleStartTime" runat="server" TimeSectionProperties-Visible="true" TimeSectionProperties-Adaptive="true"
                            TimeEditProperties-EditFormat="EditFormat.Custom" TimeEditProperties-EditFormatString="hh:mm tt" CssClass="CRMDueDate_inputField"
                            DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="16" Width="100%">
                            <%--<ValidationSettings ValidationGroup="Save">
                                <RequiredField IsRequired="true" ErrorText="Enter Start Date" />
                            </ValidationSettings>--%>
                        </dx:ASPxDateEdit>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                            Enabled="true" runat="server" ControlToValidate="dtcScheduleStartTime"
                            ErrorMessage="Enter Title " Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6" id="trsendnotification" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Send Notification</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField px-0">
                        <dx:ASPxCheckBox ID="chkSendNotification" runat="server" ClientInstanceName="chkSendNotification"></dx:ASPxCheckBox>
                    </div>
                </div>
            </div>

            <div class="row bs" id="trEmailTo" runat="server">
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Email To<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField px-0">
                        <asp:TextBox ID="txtScheduleEmailTo" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvScheduleEmailTo" ErrorMessage="Enter Email Id." Display="Dynamic" ControlToValidate="txtScheduleEmailTo" runat="server" ValidationGroup="Save" ForeColor="Red" />
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Email CC</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField px-0">
                        <asp:TextBox ID="txtScheduleEmailCC" runat="server" />
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Email Subject<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField px-0">
                        <asp:TextBox ID="txtScheduleSubject" runat="server" />
                        <div>
                            <asp:RequiredFieldValidator ID="rfvScheduleSubject" ErrorMessage="Enter Subject." Display="Dynamic" ControlToValidate="txtScheduleSubject"
                                runat="server" ValidationGroup="Save" ForeColor="Red" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Email Body</h3>
                    </div>
                    <div id="tdHtmlEditor" runat="server" class="ms-formbody accomp_inputField px-0"></div>
                </div>
            </div>


            <div class="row bs d-flex flex-wrap align-items-center accomp_inputField px-0 pt-0">
                <div class="px-3 py-1">
                    <div class="ms-formbody">
                        <asp:CheckBox ID="chkScheduleRecurring" Text="Recurring" TextAlign="Right" runat="server" AutoPostBack="true" OnCheckedChanged="chkScheduleRecurring_CheckedChanged" ValidationGroup="Save"/>
                    </div>
                </div>
                <div class="px-3">
                    <div class="ms-formbody px-0 mb-0" id="divfrequencylist" runat="server" visible="false">
                        <dx:ASPxRadioButtonList ID="rdnfrequencylist" CssClass="clsfrequencylist" RepeatDirection="Horizontal" AutoPostBack="true" OnValueChanged="rdnfrequencylist_ValueChanged" RootStyle-VerticalAlign="Top" runat="server" ClientInstanceName="rdnfrequencylist">
                            <Items>
                                <dx:ListEditItem Text="Fixed Frequency" Value="1" Selected="true" />
                                <dx:ListEditItem Text="By Day of Month" Value="2" />
                                <dx:ListEditItem Text="By Day of Week" Value="3" />
                            </Items>
                            <Border BorderStyle="None" />

                        </dx:ASPxRadioButtonList>
                    </div>
                </div>
            </div>
            <div class="row bs">
                <div class="col-md-12 col-sm-12 col-xs-12" id="recurrTable" runat="server" visible="false">
                    <div>
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel mb-0">Recurring Interval</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField px-0">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row bs">
                                        <div id="divScheduleRecurrInterval" runat="server" class="col-md-6 col-sm-6 col-xs-6">
                                            <dx:ASPxTextBox ID="txtScheduleRecurrInterval" Width="100%" runat="server">
                                                <MaskSettings IncludeLiterals="None" Mask="<1..9999999g>" />
                                                <ValidationSettings ErrorDisplayMode="Text" SetFocusOnError="true" ValidateOnLeave="true"
                                                    Display="Dynamic" ErrorTextPosition="Bottom" ValidationGroup="save">
                                                    <RequiredField ErrorText="Please enter occurrence frequency" IsRequired="true" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </div>
                                        <div class="col-md-6 col-sm-6 col-xs-6">
                                            <asp:DropDownList ID="ddlIntervalUnit" AutoPostBack="true" OnSelectedIndexChanged="ddlIntervalUnit_SelectedIndexChanged"
                                                runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                                <asp:ListItem Text="Minutes" Value="Minutes" />
                                                <asp:ListItem Text="Hours" Value="Hours" />
                                                <asp:ListItem Text="Days" Value="Days" />
                                                <asp:ListItem Text="Custom" Value="Custom" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <asp:CheckBox ID="chkbxBusinessHours" Checked="true" Text="Business Hours" runat="server" CssClass="mt-2 d-block"/>
                                        </div>
                                    </div>
                                    <div id="divMonth" runat="server" visible="false">
                                            <asp:Panel ID="panelByMonth" runat="server" CssClass="row bs">
                                                <div class="col-md-4 col-sm-4 col-xs-4">
                                                    <asp:DropDownList ID="ddlMonthFrequencyType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMonthFrequencyType_SelectedIndexChanged">
                                                        <%-- OnSelectedIndexChanged="ddlMonthFrequencyType_SelectedIndexChanged"--%>
                                                        <asp:ListItem Value="0" Text="Day of month"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Business Day of month"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-2 col-sm-2 col-xs-2 pl-0">
                                                    <asp:DropDownList ID="ddlDayOfMonth" runat="server"></asp:DropDownList>
                                                    <asp:DropDownList ID="ddlBusinessDayOfMonth" runat="server" Visible="false"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-6 col-sm-6 col-xs-6">
                                                    <div class="d-flex align-items-center">
                                                        <dx:ASPxSpinEdit ID="spnEditEveryOfMonth" Caption="Every" CaptionSettings-VerticalAlign="Middle" runat="server" MinValue="1" MaxValue="12" ClientInstanceName="spnEditEveryOfMonth">
                                                        </dx:ASPxSpinEdit>
                                                        <div class="ml-2">Month(s)</div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    <span class="d-block mt-2" id="spanCustomRecInterval" runat="server" visible="false">
                                        <asp:CheckBoxList ID="chkbxCustomRecurranceInterval" RepeatLayout="Table" RepeatColumns="7" RepeatDirection="Horizontal" runat="server">
                                            <asp:ListItem Value="0" Text="Sun"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Mon"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Tue"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Wed"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Thu"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Fri"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="Sat"></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </span>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="row bs">
                        <div class="col-md-6 col-sm-6 col-xs-6">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Recurring End Date</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField px-0 mb-0">
                                <dx:ASPxDateEdit ID="dtcRecurrEndDate" runat="server" CssClass="CRMDueDate_inputField"
                                    DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="16">
                                </dx:ASPxDateEdit>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--  </fieldset>--%>

        <div class="d-flex justify-content-end">
            <dx:ASPxButton ID="lnkDelete" runat="server" Text="Delete" AutoPostBack="false" ToolTip="Delete" CssClass="secondary-cancelBtn">
                <ClientSideEvents Click="function(s,e){DeleteScheduleActions();}" />
            </dx:ASPxButton>
            <asp:LinkButton runat="server" ID="lnkDelete1"
                OnClick="lnkDelete_Click" />
            <dx:ASPxButton ID="lnkCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" OnClick="lnkCancel_Click"></dx:ASPxButton>
            <%-- <dx:ASPxButton ID="lnkExecute" runat="server" Text="Run Now" CssClass="primary-blueBtn" OnClick="lnkExecute_Click" Visible="false"></dx:ASPxButton>--%>
            <dx:ASPxButton ID="lnkSave" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" OnClick="lnkSave_Click" ValidationGroup="Save" ClientInstanceName="lnkSave"></dx:ASPxButton>
        </div>
    </div>
</div>