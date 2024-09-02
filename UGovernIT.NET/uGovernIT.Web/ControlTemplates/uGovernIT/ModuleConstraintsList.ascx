<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleConstraintsList.ascx.cs" Inherits="uGovernIT.Web.ModuleConstraintsList" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<%--<%@ Import Namespace="uGovernIT.Helpers" %>
<%@ Import Namespace="uGovernIT.Manager.Utilities" %>--%>
<%--<SharePoint:ScriptLink ID="ScriptLink2" runat="server" Name="/_layouts/15/uGovernIT/JS/jquery.tablednd.js" Language="javascript">
</SharePoint:ScriptLink>--%>
<%--<SharePoint:CssRegistration ID="CssRegistration2" Name="/_layouts/15/uGovernIT/JS/poshytip/tip-yellow/tip-yellow.css" runat="server" />--%>
<%--<SharePoint:ScriptLink ID="ScriptLink4" runat="server" Name="/_layouts/15/uGovernIT/JS/poshytip/jquery.poshytip.min.js" Language="javascript">--%>


<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    $(function () {

        if ($("#<%=refreshPage.ClientID %>").val() == "true") {
            window.parent.document.location.href = window.parent.document.location.href;
        }
        else if ($("#<%=refreshPage.ClientID %>").val() == "false" && $("#<%=approveErrorMessage.ClientID %>").val() != '') {
            var errorLiteral = window.parent.document.getElementById("errorMsgContainer");
            var stageDescriptor = window.parent.document.getElementsByClassName("stageDescriptor");
            $(stageDescriptor).css("display", "none");
            $(errorLiteral).children("span").text($("#<%=approveErrorMessage.ClientID %>").val());

        }

    });

    function MoveUp(user) {

        hControl.value = "";
        parentOf.value = user;
        childOf.value = "";
        showBt.click();
    }
    function MoveDown(user) {

        hControl.value = "";
        parentOf.value = "";
        childOf.value = user;
        showBt.click();
    }

    function HideAllocationMessages() {

        if (control && control.innerHTML.trim() != "") {
            setTimeout("HideMessage()", 400);
        }
    }

    function HideMessage() {

        control.innerHTML = "";
    }

    function EditTaskItemOnbdClick(rowIndex) {
          <%--  var listViewId = "<%= projectTaskList.ClientID%>";
            var editBt = document.getElementById(listViewId + "_ctrl" + rowIndex + "_lnkEdit");
            if (editBt) {
                editBt.click();
            } --%>
    }

    function taskDeleteConfirm() {

        return confirm('Are you sure you want to delete this task?')

    }

    function showTaskActions(trObj, taskID) {
        //show description icon
        $("#actionButtons" + taskID).css("visibility", "visible");
        var desc = $.trim(unescape($(trObj).find(".taskDesc").html()).replace(/\+/g, " "));
        if ($.trim(desc) != "") {
            $(trObj).find(".action-description").css("visibility", "visible");
            //$(trObj).find(".action-description").poshytip({ className: 'tip-yellow', bgImageFrameSize: 9, content: desc });
        }
    }

    function hideTaskActions(trObj, taskID) {
        //show description icon
        $("#actionButtons" + taskID).css("visibility", "hidden");
        var desc = $.trim(unescape($(trObj).find(".taskDesc").html()).replace(/\+/g, " "));
        $(trObj).find(".action-description").css("visibility", "hidden");
        // $(trObj).find(".action-description").poshytip({ className: 'tip-yellow', bgImageFrameSize: 9, content: desc });
    }

    function editTask(taskID, type, title, viewType) {

        var moduleName = "<%= ModuleName %>";
        var ticketID = "<%= TicketPublicID %>"
        var moduleStage = "<%=ModuleStageId %>";
        var taskUrl = '<%= ConstraintTaskUrl %>';
        var ruleUrl = '<%= ConstraintRuleUrl %>';

        var taskParams = "";
        var ruleParams = "";
        taskParams = "module=" + moduleName + "&ticketId=" + ticketID + "&conditionType='ModuleTaskCT'&moduleStage=" + moduleStage + "&taskID=" + taskID + "&type='ExistingConstraint'" + "&viewType=" + viewType + "";
        ruleParams = "module=" + moduleName + "&ticketId=" + ticketID + "&conditionType='ModuleRuleCT'&moduleStage=" + moduleStage + "&taskID=" + taskID + "&type='ExistingConstraint'";
        if (type != null && type == "ModuleTaskCT") {

            window.parent.UgitOpenPopupDialog(taskUrl, taskParams, title, '800px', '700px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        }

        else if (type != null && type == "ModuleRuleCT") {

            window.parent.UgitOpenPopupDialog(ruleParams, params, title, '800px', '700px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        }
    }


    function openDialog(html, titleVal, source, stopRefesh) {
        var divContanier = document.createElement("div");
        divContanier.innerHTML = unescape(html).replace(/\+/g, " ").replace(/\~/g, "'");
        var htmlObj = $("body").append(divContanier);
        $(divContanier).width(250);
        $(divContanier).height(130);
        $(divContanier).css("float", "left");
        var refreshParent = stopRefesh ? 0 : 1;
        //  $(divContanier).append("<div style='float:right;width:96%;' ><span style='float:right;margin-right:6px;padding:5px 14px;border:1px solid;' class='ugitsellinkbg '><a href='javascript:void(0);' onclick='SP.UI.ModalDialog.commonModalDialogClose(" + refreshParent + ", \"" + source + "\")'>Close</a></span></div>");
        var options = {
            html: divContanier,
            width: $(divContanier).width() + 5,
            height: $(divContanier).height() + 20,
            title: titleVal,
            allowMaximize: false,
            showClose: true,
            dialogReturnValueCallback: UgitOpenHTMLPopupDialogClose
        };

        if (closePopupNotificationId != null) {
            if (navigator.appName != "Microsoft Internet Explorer") {
                window.stop();
            }
        }
        //SP.UI.ModalDialog.showModalDialog(options);
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options)

        if (closePopupNotificationId != null) {
            if (navigator.appName == "Microsoft Internet Explorer") {
                window.document.execCommand('Stop');
            }

            SP.UI.Notify.removeNotification(closePopupNotificationId);
            closePopupNotificationId = null;
        }
    }


    function doTaskCompleted(obj, taskID) {

        return true;
    }

    function newTask(objthis) {
        var moduleName = "<%= ModuleName %>";
        var ticketID = "<%= TicketPublicID %>"
        var moduleStage = "<%=ModuleStageId %>";

        var params = "";
        if (objthis != null) {
            var moduleStepId = $(objthis).attr("moduleStepId");
            params = "module=" + moduleName + "&ticketId=" + ticketID + "&conditionType=ModuleTaskCT&moduleStage=" + moduleStage + "&ID=0&type=NewConstraint&moduleStepId=" + moduleStepId;
        }
        else {
            params = "module=" + moduleName + "&ticketId=" + ticketID + "&conditionType=ModuleTaskCT&moduleStage=" + moduleStage + "&ID=0&type=NewConstraint";
        }
        window.parent.UgitOpenPopupDialog('<%= ConstraintTaskUrl %>', params, 'New Task', '700px', '600px', 0, escape("<%= Request.Url.AbsolutePath %>"));

    }

    function newConstraint() {
        var html = $("#constraintType").html();
        openDialog(html, "New Constraint", "", false);
    }


    function createConstraint() {
        closepopup();
        var taskCheckbox = $("#newTaskChoice");
        var ruleCheckbox = $("#newRuleChoice");
        var docCheckbox = $("#newDocumentChoice");
        var moduleName = "<%= ModuleName %>";
        var ticketID = "<%= TicketPublicID %>"
        var moduleStage = "<%=ModuleStageId %>";

        var params = '';
        if (taskCheckbox.attr('checked')) {
            params = "module=" + moduleName + "&ticketId=" + ticketID + "&conditionType=ModuleTaskCT&moduleStage=" + moduleStage + "&ID=0&type=NewConstraint";
            window.parent.UgitOpenPopupDialog('<%= ConstraintTaskUrl %>', params, 'New Task', '700px', '600px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        }
        else if (ruleCheckbox.attr('checked')) {
            params = "module=" + moduleName + "&ticketId=" + ticketID + "&conditionType=ModuleRuleCT&moduleStage=" + moduleStage + "&ID=0&type=NewConstraint";
            window.parent.UgitOpenPopupDialog('<%= ConstraintRuleUrl %>', params, 'New Task', '700px', '600px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        }
        else if (docCheckbox.attr('checked')) {
            params = "module=" + moduleName + "&ticketId=" + ticketID + "&control=modulestagereview&conditionType=ModuleDocumentApprovalCT&moduleStage=" + moduleStage + "&ID=0&type=NewConstraint";
        }
    }

    function loadingScreen() {
        SP.UI.ModalDialog.showWaitScreenWithNoClose("Processing...", "Please wait while data is being loaded...", 60, 280);
        ExecuteOrDelayUntilScriptLoaded(closeWaitScreen, "sp.ui.dialog.js");
        return true;
    }

    function closeWaitScreen() {
        window.parent.waitDialog.close(SP.UI.DialogResult.OK);
    }

    function confirmBeforeDelete(obj) {
        var taskTitle = $(obj).attr("tasktitle");
        if (taskTitle != "") {
            taskTitle = taskTitle.replace("'", "\'").replace("\"", "\\\"");
        }

        if (confirm("Are you sure you want to delete task \"" + taskTitle + "\"?")) {
            loadingScreen();
            return true;
        }
        else {
            return false;
        }
    }
    function closepopup() {
        SP.UI.ModalDialog.commonModalDialogClose();
    }

    function spGridViewExpandViewGroup(gridviewId, groupIndex) {
        var groupTitle = $('#<%=hiddenGroupName.ClientID %>').val();
        var gridview = $("[id$='" + gridviewId + "']");
        var rows = gridview.find("tr[isexp]");

        for (var i = 0; i < rows.length; i++) {
            if ($(rows[i]).attr('isexp') == 'true') {
                var link = $(rows[i]).find("a");
                if (link.attr('title').indexOf(groupTitle) < 0)
                    link.click();
            }
        }


    }


</script>
<%--<style type="text/css">
    .StaticMenuStyle a
    {
        border-width: 4px;
        font: menu 16px arial;
        height: 0;
        padding: 2px 40px;
        text-align: center;
        width: auto;
    }

    #header
    {
        text-align: center;
        /*height: 30px;*/
        float: left;
        padding: 0px 2px;
    }

    #content
    {
        width: 100%;
    }

    .gridheader
    {
        height: 20px;
        background-color: #CED8D9;
        text-align: left;
        font-weight: normal;
    }

    a:hover
    {
        text-decoration: underline;
    }
     .aa {
        height: 20px;
    }

     a, img {
        border:0px;
    }
     .rowalign {
       text-align:center !important;
       vertical-align:top;
       padding-top:8px;
    }
     .rowbgcolor
    {
        background-color: #E5EAEB;
    }
</style>--%>
<asp:HiddenField ID="refreshPage" runat="server" Value="false" />
<asp:HiddenField ID="approveErrorMessage" runat="server" />
<asp:HiddenField ID="hiddenGroupName" runat="server" />
<div class="col-md-12 col-sm-12 col-xs-12  noPadding">
    <div class="row">
        <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
            function UpdateGridHeight() {
                spGridConstraintList.SetHeight(0);
                var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
                if (document.body.scrollHeight > containerHeight)
                    containerHeight = document.body.scrollHeight;
                spGridConstraintList.SetHeight(containerHeight);
            }
            window.addEventListener('resize', function (evt) {
                if (!ASPxClientUtils.androidPlatform)
                    return;
                var activeElement = document.activeElement;
                if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                    window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
            });
        </script>
        <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
            try {
                ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
                ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
            } catch (ex) {
                ex.message;
            }
        </script>
        <ugit:ASPxGridView ID="spGridConstraintList" runat="server" KeyFieldName="ID" AutoGenerateColumns="false" EnableViewState="false" Width="100%" SettingsBehavior-SortMode="Custom"
            OnHtmlRowCreated="spGridConstraintList_HtmlRowCreated" OnRowCommand="spGridConstraintList_RowCommand1" CssClass="customgridview homeGrid tdPos" OnCustomColumnSort="spGridConstraintList_CustomColumnSort" >
            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
            <Columns>
            <%-- <dx:GridViewDataColumn Caption="Type"></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="Stage" Caption="Stage" GroupIndex="1"> 

                        </dx:GridViewDataColumn>--%>

                <dx:GridViewDataColumn FieldName="Stage" Caption="" GroupIndex="0"></dx:GridViewDataColumn>
                       <dx:GridViewDataColumn Caption="Type">
                           <DataItemTemplate>
                                <div style="text-align:center">                                     
                                <span><img id="taskType" runat="server" src="~/Content/Images/note.png" style="margin-top:-10px; width:18px;" title="" alt="" /></span></div>
                           </DataItemTemplate>
                       </dx:GridViewDataColumn>
                <dx:GridViewDataColumn Caption="Title">
                    <DataItemTemplate>
                        <asp:HiddenField runat="server" ID="hiddenContentType" Value='<%# Eval("Title") %>' />
                        
                                        <div id="actionButtonsSection" runat="server" visible="false">
                                            <div id='actionButtons<%# Eval("ID") %>'" style="background-color: white;position: absolute; right: 0">
                                                <asp:HiddenField runat="server" ID="pmmTaskId" Value='<%# Eval("ID") %>' />
                                                <asp:ImageButton CommandArgument='<%# Eval("ID") %>' Width="16"    ToolTip="Mark as Complete" CommandName="MarkAsComplete" ID="btMarkComplete" runat="server" ImageUrl="/Content/images/accept-symbol.png" />
                                                <asp:HyperLink runat="server" ID="lnkEdit" ImageWidth="16" ImageUrl="/Content/images/editNewIcon.png" ToolTip="Edit" style="display:inline-block;cursor:pointer;" ></asp:HyperLink>
                                                <asp:ImageButton ID="lnkDelete" runat="server" Width="16" CommandArgument='<%# Eval("ID") %>' Text="Delete" ToolTip="Delete" OnClick="lnkDelete_Click" tasktitle='<%# Eval("Title") %>'
                                                    ImageUrl='/content/images/grayDelete.png' BorderWidth="0" ForeColor="Brown" OnClientClick="return confirmBeforeDelete(this)"   />
                                                 <asp:HyperLink ID="btNewPMMTask" ImageUrl="/Content/images/plus-cicle.png" ImageWidth="16" runat="server" moduleStepId='<%# Eval("ModuleStep") %>'
                                                    href="javascript:void(0);" onclick="newTask(this);" ToolTip="New Task">
                                                </asp:HyperLink>

                                                <%--<img style="visibility: hidden;" class="action-description" src='/Content/Images/help_22x22.png' title="Help" alt="Help" style='cursor: help;' />--%>
                                                <%--<div class="taskDesc" style="display: none;">
                                                    <%# uHelper.StripHTML(Convert.ToString(Eval(DatabaseObjects.Columns.Body)))%> 
                                            </div>--%>
                                            </div>

                                        </div>

                                     
                                        <div>
                                            <asp:HyperLink Style="cursor: pointer" ID="anchrTitle" title="Title" Text='<%# Eval("Title") %>' runat="server"></asp:HyperLink>
                                        </div>
                                 

                    </DataItemTemplate>
                </dx:GridViewDataColumn>
                 
                <dx:GridViewDataColumn FieldName="TaskStatus" Caption="Task Status"></dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="AssignedTo" Caption="Assigned To"></dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="PercentComplete" Caption="% Complete" CellStyle-HorizontalAlign="Left"></dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="TaskDueDate" Caption="Task Due Date" SettingsHeaderFilter-DateRangePickerSettings-DisplayFormatString="{0:MMM-dd-yyyy}"></dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="CompletionDate" Caption="Completion Date" SettingsHeaderFilter-DateRangePickerSettings-DisplayFormatString="{0:MMM-dd-yyyy}"></dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="CompletedBy" Caption="Completed By"></dx:GridViewDataColumn>
            </Columns>
            <SettingsCommandButton>
                <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="nprPlanningGrid_btn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="nprPlanningGrid_btn"></HideAdaptiveDetailButton>
            </SettingsCommandButton>
            <SettingsBehavior AutoExpandAllGroups="true" />
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <GroupRow CssClass="homeGrid-groupRow"></GroupRow>
                <Header CssClass=" homeGrid_headerColumn" Font-Bold="true"></Header>
            </Styles>
        </ugit:ASPxGridView>
    </div>
    <div class="row">
        <div class="detailviewedititem firstcellwidth" id="addNewLinkPanel" runat="server">
            <div class="ms-vb2 paddingfirstcell col-md-6 col-sm-6 col-xs-6" style="color: #545454">
                <asp:Label ID="lblMessage" Text="The project will not move to the next stage till these tasks are complete." runat="server" CssClass="nprLifeCycle_msg"></asp:Label>
            </div>
            <div id="newConstraint" runat="server" visible="false" class="ms-vb2 paddingfirstcell col-md-4 col-sm-4 col-xs-12 newTask-btn" style="border: none !important;float:right;">
                <a href="javascript:void(0);" onclick="newTask(null);"  style="float: right; padding-left: 10px;">
                    <span class="crmEditTask_addNewBtn">
                        <i style="float: left; position: relative; top: -1px; left: -6px">
                            <img src="/Content/Images/Puzzle.png" style="border: none;" title="" alt="" /></i>
                        <b style="float: left; font-weight: 500; font-size:13px;">New</b>
                    </span>
                </a>
            </div>
        </div>
    </div>
</div>

<%-- Modal dialog box for constraint type. --%>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .pctcomplete {
        text-align: right;
    }

    .estimatedhours {
        text-align: right;
    }

    .actualhours {
        text-align: right;
    }

    .full-width {
        width: 98%;
    }

    .ms-formlabel {
        width: 160px;
    }

    .ms-standardheader {
        text-align: right;
    }

    .fullwidth {
        float: left;
        width: 100%;
    }
</style>
<div style="width: 500px; height: 200px; padding-left: 10px; display: none" id="constraintType">

    <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse"
        width="100%">
        <tr id="trTitle" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">New Task
                </h3>
            </td>
            <td class="ms-formbody">
                <input type="radio" id="newTaskChoice" name="constraint" checked="checked" />

            </td>
        </tr>
        <tr id="trComment" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">New Rule
                </h3>
            </td>
            <td class="ms-formbody">
                <input type="radio" id="newRuleChoice" name="constraint" />
            </td>
        </tr>
        <tr id="tr2" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Document Review
                </h3>
            </td>
            <td class="ms-formbody">
                <input type="radio" id="Radio2" name="constraint" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right" style="padding-top: 5px;">
                <div style="float: right;">
                    <a href="javascript:void(0);"
                        style="float: right; padding-top: 10px;" onclick="createConstraint()">
                        <span class="button-bg">
                            <b style="float: left; font-weight: normal;">Create</b>
                            <i style="float: left; position: relative; top: -2px; left: 2px">
                                <img src="/content/images/uGovernIT/add_icon.png" style="border: none;" title="" alt="" /></i>
                        </span>
                    </a>
                    <a href="javascript:void(0);" onclick="javascript:closepopup();"
                        title="Cancel" style="float: right; padding-top: 10px;">
                        <span class="button-bg">
                            <b style="float: left; font-weight: normal;">Cancel</b>
                            <i
                                style="float: left; position: relative; top: -2px; left: 2px">
                                <img src="/content/images/cancel.png" style="border: none;" title="" alt="" /></i>
                        </span>
                    </a>
                </div>
            </td>
        </tr>
    </table>
</div>
