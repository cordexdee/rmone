
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMMProjectStatus.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.PMMProject.PMMProjectStatus" %>
<%@ Register Src="~/ControlTemplates/uGovernIT/HtmlEditorControl.ascx" TagPrefix="uc1" TagName="HtmlEditorControl" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/ControlTemplates/uGovernIT/PMMProject/ExecutiveSummary.ascx" TagPrefix="uc1" TagName="ExecutiveSummary" %>
<%@ Register Src="~/ControlTemplates/uGovernIT/PMMProject/ProjectMonitors.ascx" TagPrefix="uc1" TagName="ProjectMonitors" %>
<%@ Register Src="~/ControlTemplates/uGovernIT/PMMProject/PhaseChanges.ascx" TagPrefix="uc1" TagName="PhaseChanges" %>
<%@ Register Src="~/ControlTemplates/uGovernIT/PMMProject/Accomplishments.ascx" TagPrefix="uc1" TagName="Accomplishments" %>
<%@ Register Src="~/ControlTemplates/uGovernIT/PMMProject/ImmediatePlans.ascx" TagPrefix="uc1" TagName="ImmediatePlans" %>
<%@ Register Src="~/ControlTemplates/uGovernIT/PMMProject/ProjectRisks.ascx" TagPrefix="uc1" TagName="ProjectRisks" %>
<%@ Register Src="~/ControlTemplates/uGovernIT/PMMProject/Issues.ascx" TagPrefix="uc1" TagName="Issues" %>
<%@ Register Src="~/ControlTemplates/uGovernIT/PMMProject/DecisionLogList.ascx" TagPrefix="uc1" TagName="DecisionLogList" %>


  <style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
        /*.ugitlight1lighter{*/
	/* [ReplaceColor(themeColor:"ContentAccent2",themeTint:"0.3")]  background-color:#81E4FF;
}*/
        /*.monitorContainer {
            border: 1px black solid;
            border-bottom: none;
            width: auto;
            float: left;
            position: relative;
            top: 3px;
            padding: 3px 10px;
        }*/

        .heading {
            font-weight: bold;
            padding-right: 5px;
            padding-top: 2px;
        }

        /*.scoreheading {
            font-weight: bold;
            padding-right: 5px;
        }*/

        .topborder {
            border-top: 1px solid black;
        }

        .fleft {
            float: left;
        }

        linkbutton {
            FONT-WEIGHT: bold;
            FONT-SIZE: 7pt;
            TEXT-TRANSFORM: capitalize;
            COLOR: white;
            FONT-FAMILY: Verdana;
        }

        .doubleWidth {
            width: 99%;
        }

        .s4-toplinks .s4-tn a.selected {
            padding-left: 10px;
            padding-right: 10px;
        }

        .leftBox {
            width: 1%;
            height: 54px;
            text-align: right;
            background: url(/_layouts/15/images/uGovernIT/box_left.gif) no-repeat;
        }

        .rightBox {
            width: 1%;
            height: 54px;
            background: url(/_layouts/15/images/uGovernIT/box_right.gif) no-repeat;
        }

        .middleBox {
            width: 100%;
            height: 44px;
            padding-top: 10px;
            text-align: left;
            float: left;
            margin-top: 1px;
            margin-left: -1px;
            background: url(/_layouts/15/images/uGovernIT/box_mid.gif) repeat-x;
        }

        .width25 {
            width: 25%;
        }

        /*.mainblock {
            border: inset 2px black;
        }*/

        .fullwidth {
            width: 100%;
        }

        .issuedate {
            height: 20px;
        }

        .errormessage-block {
            text-align: center;
            display: block;
        }

            .errormessage-block ol, .errormessage-block ol {
                list-style-type: none;
                color: Red;
                margin: 0px;
            }


        .btshowaddprojectsummary {
            float: left;
            padding-right: 2px;
            position: relative;
            top: -2px;
        }

        .alnright {
            text-align: right !important;
            padding-right: 5px;
        }

        .alncenter {
            text-align: center !important;
            padding-left: 0 !important;
            padding-right: 0 !important;
        }

        .archived-item {
            background: #A53421;
        }

            .archived-item td {
                color: white;
            }

        .RowAction {
            padding: 0px 1px 0px 10px;
            visibility: hidden;
        }

        tr:hover .RowAction {
            visibility: visible;
        }

        .hideRightBorder {
            border-right-width: 0px;
        }
    </style>
<script id="dxss_inlineCtrProjectStatus" type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        var prevSelectedMonitor = null;
    var prevSelectedMonitorDetails = null;
   // var folderNameWithIsUpload = "&folderName=Status&isTabActive=true&ticketId=" + projectID + "&ModuleName=PMM";

    
        function showMonitorDropDown(monitorId) {
            document.getElementById('<%= selectedMonitorId.ClientID %>').value = monitorId;
            if (prevSelectedMonitor != null && prevSelectedMonitorDetails != null) {
                prevSelectedMonitor.className = "";
                prevSelectedMonitorDetails.style.display = "none";
                if (prevSelectedMonitor.id.indexOf(monitorId) > -1) {
                    prevSelectedMonitor = null;
                    return;
                }
            }

            var monitorDropDown = document.getElementById('monitorContainer' + monitorId);
            var monitorDiv = document.getElementById('monitorContainerDiv' + monitorId);

            if (monitorDropDown != null) {
                if (monitorDropDown.style.display == "none") {
                    monitorDropDown.style.display = "";
                    monitorDiv.className = "monitorContainer ugitlight1lighter";
                    prevSelectedMonitor = monitorDiv;
                    prevSelectedMonitorDetails = monitorDropDown;
                }
                else {
                    monitorDropDown.style.display = "none";
                    monitorDiv.className = "";
                }
            }
        }
        function changeMonitor(selectedValue, monitorId) {
            document.getElementById('monitorColor' + monitorId).className = selectedValue;
            document.getElementById('monitorContainer' + monitorId).style.display = "none";
            if (prevSelectedMonitor != null && prevSelectedMonitorDetails != null) {
                prevSelectedMonitor.className = "";
                prevSelectedMonitorDetails.style.display = "none";
            }
        }

        function unselectSelectedMonitor() {
            if (prevSelectedMonitor != null && prevSelectedMonitorDetails != null) {
                prevSelectedMonitor.className = "";
                prevSelectedMonitorDetails.style.display = "none";
            }
        }

        function showUserField(fieldName) {
            var userField = document.getElementById(fieldName + "Edit");
            var userLabel = document.getElementById(fieldName + "LabelC");
            if (userField.style.display == "none") {
                userField.style.display = "";
                userLabel.style.display = "none";
            }
            else {
                userField.style.display = "none";
                userLabel.style.display = "";
            }
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_pageLoading(MyPageLoading);
        prm.add_endRequest(EndRequest);
        var btnId;
        function InitializeRequest(sender, args) {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
        }
        var notifyId = "";
        function AddNotification(msg) {
          
            loadingPanel.Show();
            
        }
        function RemoveNotification() {
           
            loadingPanel.Hide();
        }
        function BeginRequestHandler(sender, args) {
            AddNotification("Processing ..");
           
        }

        function EndRequest(sender, args) {
            
            window.parent.adjustIFrameWithHeight("<%=FrameId %>", $(".managementcontrol-main").height());
          
            var s = sender;
            var a = args;
            var msg = null;
            if (a._error != null) {
                switch (args._error.name) {
                    case "Sys.WebForms.PageRequestManagerServerErrorException":
                        msg = "PageRequestManagerServerErrorException";
                        break;
                    case "Sys.WebForms.PageRequestManagerParserErrorException":
                        msg = "PageRequestManagerParserErrorException";
                        break;
                    case "Sys.WebForms.PageRequestManagerTimeoutException":
                        msg = "PageRequestManagerTimeoutException";
                        break;
                }
                args._error.message = "My Custom Error Message " + msg;
                args.set_errorHandled(true);

            }
            else {
                RemoveNotification();
                refreshDetailPage();
                $(".datetimectr111").parents("table").find("img").bind("click", function (e) {
                    addHeightToCalculateFrameHeight(this, 170);
                });
            }
        }

        function refreshDetailPage() {
            
            if ($("#<%=refreshPage.ClientID %>").val() == "true") {
                window.parent.document.location.href = window.parent.document.location.href;
            }
            if ($(<%=PhaseChanges.FindControl("refreshPhase").ClientID%>).val() == "true") {
                window.parent.document.location.href = window.parent.document.location.href;
            }
            
        }

        function MyPageLoading(sender, args) {
        }

        function DeleteAccomplishment(id) {
            if (confirm('Are you sure you want to delete this accomplishment?')) {
                gridAccomplishment.PerformCallback("DELETE|" + id);
            }
        }

        function UnArchiveAccomplishment(id) {
            if (confirm('Are you sure you want to unarchive this accomplishment?')) {
                gridAccomplishment.PerformCallback("UNARCHIVE|" + id);
            }
        }

        function ArchiveAccomplishment(id) {
            if (confirm('Are you sure you want to archive this accomplishment?')) {
                gridAccomplishment.PerformCallback("ARCHIVE|" + id);
            }
        }

        function DeleteImmediatePlansItem(id) {
            if (confirm('Are you sure you want to delete this planned item?')) {
                gridImmediatePlans.PerformCallback("DELETE|" + id);
            }
        }

        function ArchiveImmediatePlansItem(id) {
            if (confirm('Are you sure you want to archive this planned item?')) {
                gridImmediatePlans.PerformCallback("ARCHIVE|" + id);
            }
        }

        function UnArchiveImmediatePlansItem(id) {
            if (confirm('Are you sure you want to unarchive this planned item?')) {
                gridImmediatePlans.PerformCallback("UNARCHIVE|" + id);
            }
        }

        var refreshPageCallBack = false;
        function MoveToAccomp(id) {
            if (confirm('Are you sure you want to mark this item as accomplished?')) {
                refreshPageCallBack = true;
                gridImmediatePlans.PerformCallback("MOVETOACCOMP|" + id);
            }
        }

        function refreshPageAfterCallback() {
            if (refreshPageCallBack) {
                refreshPageCallBack = false;
                window.location.reload(true);
            }
        }

        function DeleteRisk(id) {
            if (confirm('Are you sure you want to delete this risk?')) {
                gridRisks.PerformCallback("DELETE|" + id);
            }
        }

        function ArchiveRisk(id) {
            if (confirm('Are you sure you want to archive this risk?')) {
                gridRisks.PerformCallback("ARCHIVE|" + id);
            }
        }

        function UnArchiveRisk(id) {
            if (confirm('Are you sure you want to unarchive this risk?')) {
                gridRisks.PerformCallback("UNARCHIVE|" + id);
            }
        }

        function DeleteIssue(id) {
            if (confirm('Are you sure you want to delete this issue?')) {
                gridIssues.PerformCallback("DELETE|" + id);
            }
        }

        function UnArchiveIssue(id) {
            if (confirm('Are you sure you want to unarchive this issue?')) {
                gridIssues.PerformCallback("UNARCHIVE|" + id);
            }
        }


    function issueCheckArchived(id) {
        var control = document.getElementById(id);
            if (control.checked)
            {
                id = true;
            }
            else
            {
                id = false;
            }
           gridIssues.PerformCallback("SHOWARCHIVE|" + id);           
        }

     function riskCheckArchived(id) {
        var control = document.getElementById(id);
            if (control.checked)
            {
                id = true;
            }
            else
            {
                id = false;
            }
           gridRisks.PerformCallback("SHOWARCHIVE|" + id);           
        }
    function accomplishmentCheckArchived(id) {
        var control = document.getElementById(id);
            if (control.checked)
            {
                id = true;
            }
            else
            {
                id = false;
            }
            gridAccomplishment.PerformCallback("SHOWARCHIVE|" + id);           
        }
     function decesionCheckArchived(id) {
        var control = document.getElementById(id);
            if (control.checked)
            {
                id = true;
            }
            else
            {
                id = false;
            }
            gridDecisionLog.PerformCallback("SHOWARCHIVE|" + id);           
     }
     function plannedCheckArchived(id) {
        var control = document.getElementById(id);
            if (control.checked)
            {
                id = true;
            }
            else
            {
                id = false;
            }
            gridImmediatePlans.PerformCallback("SHOWARCHIVE|" + id);           
     }

        function ArchiveIssue(id) {
            if (confirm('Are you sure you want to archive this issue?')) {
                gridIssues.PerformCallback("ARCHIVE|" + id);
            }
        }

        function DeleteDecisionLog(id) {
            if (confirm('Are you sure you want to delete this decision log?')) {
                gridDecisionLog.PerformCallback("DELETE|" + id);
            }
        }

        function UnArchiveDecisionLog(id) {
            if (confirm('Are you sure you want to unarchive this decision log?')) {
                gridDecisionLog.PerformCallback("UNARCHIVE|" + id);
            }
        }

        function ArchiveDecisionLog(id) {
            if (confirm('Are you sure you want to archive this decision log?')) {
                gridDecisionLog.PerformCallback("ARCHIVE|" + id);
            }
        }

        function EditItemIssueOnbdClick(issueID) {
            editDecisionLog(issueID);
        }

        function EditItemDecisionLogOnbdClick(decisionlogID) {
            decisionlogid(decisionlogID);
        }

        function newAccomplishmentItem() {
            var ctrl = getUrlParam("ctrl", '');
            var projectID = "<%= pmmPublicId %>";
            var params = "PublicTicketID=" + projectID + "&projectID=" + projectID + "&itemType=Accomplishment&viewType=0";
            if (ctrl == "PMM.ProjectCompactView")
                UgitOpenPopupDialog('<%= itemFormUrl %>', params, 'New Accomplishment', '50', '75', 0, escape("<%= Request.Url.AbsolutePath %>"));
            else
                window.parent.UgitOpenPopupDialog('<%= itemFormUrl %>', params, 'New Accomplishment', '50', '75', 0, escape("<%= Request.Url.AbsoluteUri %>"));
            return false;
        }

        function newImmediatePlansItem() {
            var ctrl = getUrlParam("ctrl", '');
            var projectID = "<%= pmmPublicId %>";
            var params = "PublicTicketID=" + projectID + "&projectID=" + projectID + "&itemType=ImmediatePlans&viewType=0";
            if (ctrl == "PMM.ProjectCompactView")
                UgitOpenPopupDialog('<%= itemFormUrl %>', params, 'New Planned Item', '50', '75', 0, escape("<%= Request.Url.AbsolutePath %>"));
            else
                window.parent.UgitOpenPopupDialog('<%= itemFormUrl %>', params, 'New Planned Item', '50', '75', 0, escape("<%= Request.Url.AbsoluteUri %>"));
            return false;
        }

        function editImmediatePlansItem(itemId) {
            var ctrl = getUrlParam("ctrl", '');
            var projectID = "<%= pmmPublicId %>";
            var params = "projectID=" + projectID + "&itemId=" + itemId + "&itemType=ImmediatePlans&viewType=1"+"&folderName=Status&isTabActive=true&ticketId=" + projectID + "&ModuleName=PMM";
            if (ctrl == "PMM.ProjectCompactView")
                UgitOpenPopupDialog('<%= itemFormUrl %>', params, 'Edit Planned Item', '50', '75', 0, escape("<%= Request.Url.AbsolutePath %>"));
            else
                window.parent.UgitOpenPopupDialog('<%= itemFormUrl %>', params, 'Edit Planned Item', '50', '75', 0, escape("<%= Request.Url.AbsoluteUri %>"));
        return false;
        }

        function newRiskItem() {
            var ctrl = getUrlParam("ctrl", '');
            var projectID = "<%= pmmPublicId %>";
            var params = "projectID=" + projectID + "&Module=PMM";
            if (ctrl == "PMM.ProjectCompactView")
                UgitOpenPopupDialog('<%= newRiskFormUrl %>', params, 'New Risk', '80', '80', 0, escape("<%= Request.Url.AbsolutePath %>"));
            else
                window.parent.UgitOpenPopupDialog('<%= newRiskFormUrl %>', params, 'New Risk', '80', '80', 0, escape("<%= Request.Url.AbsoluteUri %>"));
        return false;
        }

        function editRiskItem(itemId) {
            var ctrl = getUrlParam("ctrl", '');
            var projectID = "<%= pmmPublicId %>";
            var params = "projectID=" + projectID + "&taskid=" + itemId + "&Module=PMM"+"&folderName=Status&isTabActive=true&ticketId=" + projectID + "&ModuleName=PMM";
            if (ctrl == "PMM.ProjectCompactView")
                UgitOpenPopupDialog('<%= editRiskFormUrl %>', params, 'Edit Risk', '80', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
            else
                 window.parent.UgitOpenPopupDialog('<%= editRiskFormUrl %>', params, 'Edit Risk', '80', '90', 0, escape("<%= Request.Url.AbsoluteUri %>"));
        }

    function editIssue(issueid) {
            var ctrl = getUrlParam("ctrl", '');
            var projectID = "<%= pmmPublicId %>";
            var params = "projectID=" + projectID + "&taskid=" + issueid+"&folderName=Status&isTabActive=true&ticketId=" + projectID + "&ModuleName=PMM";
            if (ctrl == "PMM.ProjectCompactView")
                UgitOpenPopupDialog('<%= editIssueFormUrl %>', params, 'Edit Issue', '80', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
            else
                window.parent.UgitOpenPopupDialog('<%= editIssueFormUrl %>', params, 'Edit Issue', '80', '90', 0, escape("<%= Request.Url.AbsoluteUri %>"));
            return false;
        }

        function newIssue() {
            var ctrl = getUrlParam("ctrl", '');

            var projectID = "<%= pmmPublicId %>";
            var params = "projectID=" + projectID;
            if (ctrl == "PMM.ProjectCompactView")
                UgitOpenPopupDialog('<%= newIssueFormUrl %>', params, 'New Issue', '80', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
            else
                window.parent.UgitOpenPopupDialog('<%= newIssueFormUrl %>', params, 'New Issue', '80', '90', 0, escape("<%= Request.Url.AbsoluteUri %>"));
            return false;
        }

        function CancelEvent(evt) {
			return ASPxClientUtils.PreventEventAndBubble(evt);
		}

        function newDecisionLog() {
            var ctrl = getUrlParam("ctrl", '');
            var projectID = "<%= pmmPublicId %>";
            var params = "projectID=" + projectID;
            if (ctrl == "PMM.ProjectCompactView")
                UgitOpenPopupDialog('<%= newDecisionLogFormUrl %>', params, 'New Decision Log', '80', '110', 0, escape("<%= Request.Url.AbsolutePath %>"));
            else
                window.parent.UgitOpenPopupDialog('<%= newDecisionLogFormUrl %>', params, 'New Decision Log', '80', '110', 0, escape("<%= Request.Url.AbsoluteUri %>"));
            return false;
        }

        function editDecisionLog(decisionlogid) {
            var ctrl = getUrlParam("ctrl", '');
            var projectID = "<%= pmmPublicId %>";
            var params = "projectID=" + projectID + "&taskid=" + decisionlogid+"&folderName=Status&isTabActive=true&ticketId=" + projectID + "&ModuleName=PMM";
            if (ctrl == "PMM.ProjectCompactView")
                UgitOpenPopupDialog('<%= editDecisionLogFormUrl %>', params, 'Edit Decision Log', '80', '110', 0, escape("<%= Request.Url.AbsolutePath %>"));
            else
                window.parent.UgitOpenPopupDialog('<%= editDecisionLogFormUrl %>', params, 'Edit Decision Log', '80', '110', 0, escape("<%= Request.Url.AbsoluteUri %>"));
            return false;
        }


        function editAccomplishmentItem(itemId) {
            var ctrl = getUrlParam("ctrl", '');
            var projectID = "<%= pmmPublicId %>";
            //var params = "projectID=" + projectID + "&itemId=" + itemId + "&itemType=Accomplishment&viewType=1&folderName=Status&isTabActive=true&ticketId="+projectID+"&ModuleName=PMM";//dynamic moduleName
            var params = "projectID=" + projectID + "&itemId=" + itemId + "&itemType=Accomplishment&viewType=1"+"&folderName=Status&isTabActive=true&ticketId=" + projectID + "&ModuleName=PMM";//dynamic moduleName
            if (ctrl == "PMM.ProjectCompactView")
                UgitOpenPopupDialog('<%= itemFormUrl %>', params, 'Edit Accomplishment', '50', '75', 0, escape("<%= Request.Url.AbsolutePath %>"));
            else
                window.parent.UgitOpenPopupDialog('<%= itemFormUrl %>', params, 'Edit Accomplishment', '50', '75', 0, escape("<%= Request.Url.AbsoluteUri %>"));
            return false;
        }


        function ConfirmCloseTasks(action) {

            //loadingPanel.Show();
            confirmCloseTasks.Hide();
            $("#<%=hdnConfirmCloseTasksAction.ClientID%>").val(action);

        }

        
    </script>
<dx:ASPxLoadingPanel ID="loadingPanel" Modal="True" ClientInstanceName="loadingPanel" runat="server" Text="Processing.."></dx:ASPxLoadingPanel>

<asp:Panel ID="projectStatusEditMode" runat="server" CssClass="projectStatusEditMode-container">

  

    

    <asp:HiddenField ID="hdnConfirmCloseTasksAction" runat="server" />

    <div style="display: none">
        <dx:ASPxDataView runat="server" ID="dummyCalendar" Visible="true"></dx:ASPxDataView>
        <ugit:UserValueBox runat="server" ID="dummyPeopleEditor" />
    </div>
    <div class="monitorblock col-md-12">
        <div class="row">
            <div class="col-md-9" style="padding-left:0">
                <uc1:ProjectMonitors runat="server" id="ProjectMonitors" />
            </div>
            <div class="col-md-3" style="padding-right:0">
                <uc1:PhaseChanges runat="server" id="PhaseChanges" />
            </div>
           
        </div>
        <div class="row">
             <br />
           <uc1:ExecutiveSummary runat="server" id="ExecutiveSummary" />
            <br />
        </div> 
        <div class="row">
            <uc1:Accomplishments runat="server" ID="Accomplishments" />
            <br />
        </div> 
        <div class="row">
            <uc1:ImmediatePlans runat="server" id="ImmediatePlans" />
            <br />
        </div> 
        <div class="row">
            <uc1:ProjectRisks runat="server" id="ProjectRisks" />
            <br />
        </div>
        <div class="row">
            <uc1:Issues runat="server" id="Issues" />
            <br />
        </div>
        <div class="row">
            <uc1:decisionloglist runat="server" id="DecisionLogList" />
        </div>

        
        
        
        
        
        
    </div>
    <div style="display:none;">
        <asp:HiddenField ID="selectedMonitorId" runat="server" />
        <asp:HiddenField ID="refreshPage" runat="server" Value="false" />
    </div>
</asp:Panel>


