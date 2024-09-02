<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyTasks.ascx.cs" Inherits="uGovernIT.Web.MyTasks" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" src="https://vadikom.com/demos/poshytip/src/jquery.poshytip.js"></script>
<link rel="stylesheet" href="https://vadikom.com/demos/poshytip/src/tip-skyblue/tip-skyblue.css" type="text/css" />

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .hide {
        display: none;
    }
    .ucontentdiv a {
        color: #4A6EE2 !important;
        font-size: 12px;
        font-family: 'Poppins', sans-serif;
    }

    .pagerBox td table tr td span {
        /* font-size : larger; */
        border: 1px solid black;
        padding: 0px 3px;
    }

    table.ms-listviewtable > tbody > tr > td {
        border: none;
    }

    .ms-viewheadertr .ms-vh2-gridview {
        background: transparent !important;
        height: 22px;
    }

    .ms-vh2 .ms-selectedtitle .ms-vb, .ms-vh2 .ms-unselectedtitle .ms-vb {
        text-align: left;
    }

    .ms-listviewtable .ms-vb2, .ms-summarystandardbody .ms-vb2 {
        text-align: left;
    }

    .pctcompletecolumn {
        padding-right: 10px;
        text-align: center;
    }

    .fleft {
        float: left;
    }


    .action-container {
        background: none repeat scroll 0 0 #FFFFAA;
        border: 1px solid #FFD47F;
        float: left;
        padding: 1px 5px 0;
        position: absolute;
        z-index: 1000;
        margin-top: -4px;
        margin-left: 3px;
        right: 0px;
        top: 0px;
    }

    .ucontentdiv {
        background: #FFF;
        color: #4A6EE2;
        border: 1px solid #4A6EE2;
        border-radius: 4px;
        padding: 5px;
        margin-left: 6px;
        font-family: 'Poppins', sans-serif;
        margin-right: 6px;
    }

    .statuscolumn {
        vertical-align: top;
        padding-top: 4px;
    }

    .pctcompletecolumn {
        vertical-align: top;
        padding-top: 4px;
    }

    .clsChangeBackgroundOnHold {
        color: red;
        font-weight: bold;
    }
    .uborderdiv{
        display:inline-block;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function adjustControlSize() {
        setTimeout(function () {
            $("#s4-workspace").width("100%");
            // gridClientInstance.AdjustControl();
            var height = $(window).height();
            $("#s4-workspace").height(height);
        }, 10);
    }
    $(function () {
        try {
            window.parent.RefreshTabCount("mytask", parseInt('<%=openTaskCount%>'));
        }
        catch (ex) { }
    });
</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        gvFilteredList.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        gvFilteredList.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>

<div class="col-md-12 col-sm-12 col-xs-12" style="padding-left:10px;">
    <div class="row">
        <div id="rootDiv" runat="server">
            <asp:Label ID="lbMessage" runat="server"></asp:Label>
            <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                <div class="row">
                    <div class="ugit-contentcontainer" style="padding-top: 3px">
                        <input type="hidden" id="myTaskViewType" name="myTaskViewType" />
                        <div id="Div1" style="float: left; margin-left: 5px; margin-top: 3px; margin-right: 2px;" runat="server">
                            <img src="/Content/Images/calendarNew.png" width="18" title="Calendar View" onclick="javascript:return DisplayTaskCalendarView(this)" style="cursor: pointer;" />
                        </div>
                        <div class="uborderdiv">
                            <div class="ucontentdiv ugitlinkbg" id="myTaskByProject" runat="server">
                                <span>
                                    <asp:LinkButton ID="btMyTaskByProject" OnClientClick="javascript:return changeMyTaskView('byproject')"
                                        runat="server" Text="By Project" OnClick="btMyTaskByProject_Click"></asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <div class="uborderdiv">
                            <div class="ucontentdiv ugitlinkbg" id="myTaskByDueDate" runat="server">
                                <span>
                                    <asp:LinkButton ID="btMyTaskByDueDate" runat="server" Text="By Due Date" OnClick="btMyTaskByDueDate_Click"
                                        OnClientClick="javascript:return changeMyTaskView('byduedate')"></asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <div class="uborderdiv">
                            <div class="ucontentdiv ugitlinkbg" id="myTaskByProgress" runat="server">
                                <span>
                                    <asp:LinkButton ID="btMyTaskByProgress" runat="server" Text="By Progress" OnClick="btMyTaskByProgress_Click"
                                        OnClientClick="javascript:return changeMyTaskView('byprogress')"></asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <div class="uborderdiv">
                            <div class="ucontentdiv ugitlinkbg" id="myTaskByCompleted" runat="server">
                                <span>
                                    <asp:LinkButton ID="btMyTaskByCompleted" runat="server" Text="By Completed Tasks" OnClick="btMyTaskByCompleted_Click"
                                        OnClientClick="javascript:return changeMyTaskView('bycompletedtasks')"></asp:LinkButton>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                <ugit:ASPxGridView ID="gvFilteredList" ClientInstanceName="gvFilteredList" runat="server" AutoGenerateColumns="false" Width="100%"
                    EnableSortingAndPagingCallbacks="false" EmptyDataText="No tasks" OnInit="GvFilteredList_Init" OnPreRender="GvFilteredList_PreRender"
                    OnDataBinding="gvFilteredList_DataBinding" OnHtmlRowPrepared="gvFilteredList_HtmlRowPrepared" CssClass="customgridview homeGrid"
                        OnCustomGroupDisplayText="gvFilteredList_CustomGroupDisplayText">
                    <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                    <SettingsBehavior AutoExpandAllGroups="true" />
                    <SettingsPager Position="TopAndBottom">
                    </SettingsPager>
                        <settingscommandbutton>
                            <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                            <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                        </settingscommandbutton>
                    <Styles>
                        <Row CssClass="homeGrid_dataRow"></Row>
                        <Header CssClass="homeGrid_headerColumn"></Header>
                        <GroupRow CssClass="homeGrid-groupRow"></GroupRow>
                    </Styles>
                </ugit:ASPxGridView>
            </div>

            <asp:Panel ID="moreOptionPanel" runat="server" CssClass="ms-alternatingstrong">
                <div style="float: left; width: 100%;">
                    <span style="float: right; padding: 5px 2px 2px 0px;">
                        <strong>
                            <asp:HyperLink ID="btMoreOption" runat="server" Text="More >>">
                            </asp:HyperLink>
                        </strong>
                    </span>
                </div>
            </asp:Panel>

            <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                function changeMyTaskView(viewType) {
                    $("#myTaskViewType").val(viewType);
                    set_cookie("mytaskviewtype", viewType);
                    return true;
                }
                function showTasksActions(trObj) {
                    if ($(trObj).hasClass("editrow")) {
                        return;
                    }

                    var taskTds = $(trObj).find("td");
                    var lastColumn = $(trObj).find(".taskactions");

                    var actionContainer = $(trObj).find(".action-container");
                    //actionContainer.show();
                    actionContainer.removeClass("hide");
                    var actions = actionContainer.find("img");
                    if (actions.length == 1)
                        actionContainer.width("25px");
                    else if (actions.length == 2)
                        actionContainer.width("40px");
                    else
                        actionContainer.width("56px");


                    //show description icon
                    var desc = $.trim(unescape($(trObj).attr("taskDesc")).replace(/\+/g, " "));
                    $(trObj).find(".action-description").css("visibility", "visible");
                    $(trObj).find(".action-description").poshytip({ className: 'tip-yellow', bgImageFrameSize: 9, content: desc });
                }

                function hideTasksActions(trObj) {
                    var actionContainer = $(trObj).find(".action-container");
                    //actionContainer.hide();
                    actionContainer.addClass("hide");
                    //show description icon
                    $(trObj).find(".action-description").css("visibility", "hidden");
                }

                function setMessage(message, color, removeAfter) {
                    var lbMessage = $("#<%=lbMessage.ClientID %>");
                    lbMessage.addClass("message-container");
                    lbMessage.html(message);
                    if (color == undefined || color == null || color == "") {
                        color = "blue";
                    }
                    lbMessage.css("color", color);
                    if (removeAfter != undefined && removeAfter != null && removeAfter > 0) {
                        setTimeout("removeMessage()", removeAfter * 1000);
                    }
                }

                function removeMessage() {
                    var lbMessage = $("#<%=lbMessage.ClientID %>");
                    lbMessage.removeClass("message-container");
                    lbMessage.html("");
                }

                function doStageExitCriteriaComplete(obj) {

                }

                var selectedTask = {};

                function doTaskComplete(obj, actualHours) {
                    selectedTask = {};
                    var keepActualHourMandatorys = $.parseJSON('<%= keepActualHourMandatorys%>');

                    var trObj = $(obj).parents("tr").get(0);
                    var firstTd = $($(trObj).find("td").get(0));
                    var title = $(trObj).attr("taskTitle").replace(/\+/g, " ");

                    if (confirm("Are you sure you want to mark task [" + title + "] as completed?")) {

                        var moduleName = $(trObj).attr("moduleName");
                        var projectID = $(trObj).attr("projectID");
                        var taskID = $(trObj).attr("taskID");
                        selectedTask = { moduleName: moduleName, projectID: projectID, taskID: taskID, object: obj };
                        if (keepActualHourMandatorys[moduleName] == "True") {
                            txtActualHours.SetText(actualHours);
                            pcTaskActualHours.Show();
                        }
                        else {
                            markTaskAsComplete();
                        }
                    }

                }

                function markTaskAsComplete() {
                    var obj = selectedTask.object;
                    var trObj = $(obj).parents("tr").get(0);
                    var firstTd = $($(trObj).find("td").get(0));
                    var title = $(trObj).attr("taskTitle").replace(/\+/g, " ");
                    firstTd.append("<b><img src='/Content/images/loadingcirclests16.gif'/></b>");

                    selectedTask.actualHours = txtActualHours.GetText();
                    var dataVar = "{ 'moduleName' : \"" + selectedTask.moduleName + "\",\"projectID\":\"" + selectedTask.projectID + "\",\"taskID\":\"" + selectedTask.taskID + "\",\"actualHours\":\"" + selectedTask.actualHours + "\"}";
                    var jsonData = ""
                    $.ajax({
                        url: ugitConfig.apiBaseUrl + "/api/module/MarkTaskAsComplete",
                        method: "POST",
                        data: { TaskKeys: selectedTask.taskID, TicketPublicId: selectedTask.projectID, TaskType: "ModuleTasks" },
                        success: function (data) {
                            gvFilteredList.Refresh();
                        },
                        error: function (error) { }
                    });
                }

                function btnTaskCompleteOk_Validation(s, e) {
                    if (ASPxClientEdit.ValidateGroup('actualHours')) {
                        markTaskAsComplete();
                        pcTaskActualHours.Hide();
                    }

                }


                function doTaskEdit(obj) {
                    var trObj = $(obj).parents("tr").get(0);
                    var parentTr = $(trObj);
                    var itemJson = $.parseJSON($.trim(parentTr.find(".budgetiteminfo").html()));
                    var params = "taskType=task&viewtype=1&projectID=" + itemJson.projectid + "&taskID=" + itemJson.itemid + "&moduleName=" + itemJson.modulename;

                    var title = "Edit Task";
                    if (itemJson.modulename == "ExitCriteria") {
                        title = "Edit Issue";
                    }

                    window.parent.UgitOpenPopupDialog('<%= editTaskFormUrl %>', params, title, '1000px', '100', 0, escape("<%= sourceURL %>"));
                }


                function doTaskAdd(obj) {
                    debugger;
                    var trObj = $(obj).parents("tr").get(0);
                    var parentTr = $(trObj);
                    var itemJson = $.parseJSON($.trim(parentTr.find(".budgetiteminfo").html()));
                    var params = "taskType=mytask&projectID=" + itemJson.projectid + "&parentTaskID=" + itemJson.itemid + "&moduleName=" + itemJson.modulename;

                    var title = "Add Task";
                    if (itemJson.modulename == "ExitCriteria") {
                        title = "Add Issue";
                    }

                    window.parent.UgitOpenPopupDialog('<%= editTaskFormUrl %>', params, title, '1000px', '100', 0, escape("<%= sourceURL %>"));
                }
                function DisplayTaskCalendarView(obj) {
                    var calendarURL = '<%=calendarURL %>';
                    var url = calendarURL;
                    window.parent.UgitOpenPopupDialog(url, '', 'Calendar Task View', '850px', '640px', 0, '');
                    return false;
                }


            </script>
            <script data-v="<%=UGITUtility.AssemblyVersion %>">
                ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
                ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
            </script>

            <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                window.myTaskEditor = window.myTaskEditor || {};


                window.myTaskEditor = {
                    tableRow: null,
                    iJson: {},

                    clearForm: function () {

                        $("#editMytaskPopup .trfmytaskstatus #hfEditProjectID").val("");
                        $("#editMytaskPopup .trfmytaskstatus #hfEditModuleName").val("");
                        $("#editMytaskPopup .trfmytaskstatus #hfEditTaskID").val("");
                        $("#editMytaskPopup .trfmytaskstatus select").get(0).selectedIndex = 0;
                        $("#editMytaskPopup .trfmytaskpct :text").val("");
                        $("#editMytaskPopup .trfmytaskcomment  textarea").val("");

                    },
                    closePopup: function (clearForm) {
                        if (clearForm) {
                            this.clearForm();
                        }
                        $("#editMytaskPopup").hide();
                    },

                    editTask: function (trObj) {
                        this.tableRow = trObj;
                        this.clearForm();
                        var parentTr = $(trObj);
                        var itemJson = $.parseJSON($.trim(parentTr.find(".budgetiteminfo").html()));
                        this.iJson = itemJson;

                        var title = itemJson.title.replace(/\+/g, " ");
                        $("#editMytaskPopup .ugit-ms-dlgTitleText").html("Task: " + title);
                        $("#editMytaskPopup .trfmytaskstatus #hfEditProjectID").val(itemJson.projectid);
                        $("#editMytaskPopup .trfmytaskstatus #hfEditModuleName").val(itemJson.modulename);
                        $("#editMytaskPopup .trfmytaskstatus #hfEditTaskID").val(itemJson.itemid);
                        $("#editMytaskPopup .trfmytaskstatus select option[value='" + itemJson.status + "']").attr("selected", "selected");
                        $("#editMytaskPopup .trfmytaskpct :text").val(itemJson.pctcomplete);
                        $("#editMytaskPopup .trfmytaskcomment  textarea").val("");

                        this.setPopupPosition(trObj);
                        $("#editMytaskPopup").show();
                    },
                    saveTask: function () {
                        var itemRow = this.tableRow;

                        var projectID = $("#editMytaskPopup .trfmytaskstatus #hfEditProjectID").val();
                        var moduleName = $("#editMytaskPopup .trfmytaskstatus #hfEditModuleName").val();
                        var taskID = $("#editMytaskPopup .trfmytaskstatus #hfEditTaskID").val();

                        var status = $("#editMytaskPopup .trfmytaskstatus select").val();
                        var pctComplete = $("#editMytaskPopup .trfmytaskpct :text").val();
                        var comment = $("#editMytaskPopup .trfmytaskcomment  textarea").val();

                        this.iJson.status = status;
                        this.iJson.pctcomplete = pctComplete;

                        var refresh = false;
                        if (status.toLowerCase() == "completed" || parseInt($.trim(pctComplete)) >= 100) {
                            refresh = true;

                            this.iJson.status = "Completed";
                            this.iJson.pctcomplete = 100;
                        }

                        var firstTd = $($(itemRow).find("td").get(0));
                        firstTd.append("<b><img src='/Content/images/loadingcirclests16.gif'/></b>");

                        this.closePopup(true);

                        var jsonItem = "{\"itemid\":" + this.iJson.itemid + ",\"status\":\"" + this.iJson.status + "\",\"pctcomplete\":" + this.iJson.pctcomplete + ",\"title\":\"" + this.iJson.title + "\",\"projectid\":\"" + this.iJson.projectid + "\",\"modulename\":\"" + this.iJson.modulename + "\"}";
                        var dataVar = "{ 'moduleName' : \"" + moduleName + "\",\"projectID\":\"" + projectID + "\",\"taskID\":\"" + taskID + "\",\"status\":\"" + status + "\",\"pctComplete\":\"" + pctComplete + "\",\"latestComment\":\"" + escape(comment) + "\"}";

                        $.ajax({
                            type: "POST",
                            url: "<%= ajaxPageURL %>/SaveTaskStatus",
                            data: dataVar,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (message) {
                                prsHours = message.d;
                                var resultJson = $.parseJSON(message.d);
                                if (resultJson.messagecode = 0) {
                                    setMessage(resultJson.message, "red", 0);
                                }
                                else if (resultJson.messagecode = 1) {

                                    if (refresh) {
                                        window.location.href = window.location.href;
                                    }

                                    if (itemRow != null) {
                                        $(itemRow).find(".budgetiteminfo").html(jsonItem)
                                        var statucColumn = $(itemRow).find(".statuscolumn");
                                        var pctCompleteColumn = $(itemRow).find(".pctcompletecolumn");
                                        statucColumn.html(status);
                                        pctCompleteColumn.html(pctComplete + "%");
                                    }
                                    firstTd.find("b").remove();
                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                setMessage("Error Occurred", "ren", 2);
                            }
                        });

                    },
                    setPopupPosition: function (trObj) {

                        var currentLeft = $(trObj).find(".budgetiteminfo").parent().position().left;
                        var currentTop = $(trObj).find(".budgetiteminfo").parent().position().top;
                        var windowWidth = $(window).width();
                        var windowHeight = $(window).height();

                        var newLeft = 0;
                        var newTop = 0;

                        newLeft = currentLeft;
                        newTop = currentTop;

                        $("#editMytaskPopup").css({ "left": newLeft + "px", "top": newTop + "px" });
                    }
                }

                $(function () {
                    removeEmptyRow();
                });

                function removeEmptyRow() {
                    $(".taskactions").each(function (idx, item) {
                        if ($.trim($(item).html().replace("&nbsp;", "")) == '') {
                            $(item).parent().css('display', 'none');
                        }
                    });
                }
            </script>
            <div style="background: #ffffff; float: left; border: 1px solid #7C684D; position: absolute; z-index: 1000; display: none;" id="editMytaskPopup">
                <div class="ugit-ms-dlgTitle" style="cursor: default; background: #191919; height: 32px; overflow: hidden; white-space: nowrap;">
                    <span title="Edit Note" class="ugit-ms-dlgTitleText" style="width: 286px; color: white; float: left; padding-left: 5px; padding-top: 6px;">Edit Task</span><span class="ms-dlgTitleBtns"><a
                        class="ms-dlgCloseBtn" title="Close" href="javascript:;" accesskey="C"><span class="s4-clust"
                            style="height: 18px; width: 18px; position: relative; display: inline-block; overflow: hidden;">
                            <img class="ms-dlgCloseBtnImg" style="left: -0px !important; top: -658px !important; position: absolute;"
                                src="/Content/images/fgimg.png" onclick="window.myTaskEditor.closePopup(true);">
                        </span></a></span>
                </div>

                <div>
                    <div style="float: left; width: 100%;">
                        <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse"
                            width="350px">
                            <tr class="trfmytaskstatus">
                                <td class="ms-formlabel">
                                    <input type="hidden" id="hfEditProjectID" />
                                    <input type="hidden" id="hfEditModuleName" />
                                    <input type="hidden" id="hfEditTaskID" />
                                    <h3 class="ms-standardheader">Status<b style="color: Red;">*</b>
                                    </h3>
                                </td>
                                <td class="ms-formbody">
                                    <select class="taskstatus">
                                        <option value="Not Started">Not Started</option>
                                        <option value="In Progress">In Progress</option>
                                        <option value="Completed">Completed</option>
                                        <option value="Deferred">Deferred</option>
                                        <option value="Waiting on someone else">Waiting on someone else</option>
                                    </select>

                                </td>
                            </tr>
                            <tr class="trfmytaskpct">
                                <td class="ms-formlabel">
                                    <h3 class="ms-standardheader">Pct Complete<b style="color: Red;">*</b>
                                    </h3>
                                </td>
                                <td class="ms-formbody">
                                    <asp:TextBox ID="txtBudgetItemVal" runat="server" Width="70"></asp:TextBox>

                                </td>
                            </tr>
                            <tr class="trfmytaskcomment">
                                <td class="ms-formlabel">
                                    <h3 class="ms-standardheader">Comment
                                    </h3>
                                </td>
                                <td class="ms-formbody">
                                    <asp:TextBox ID="txtBudgetAmountf" CssClass="fullwidth94" TextMode="MultiLine" runat="server"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2" style="text-align: right; padding: 5px 5px 5px 0px;">
                                    <input type="button" value="Save" onclick="window.myTaskEditor.saveTask();" />
                                    <input type="button" value="Cancel" onclick="window.myTaskEditor.closePopup(true);" />

                                </td>

                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<dx:ASPxPopupControl ID="pcTaskActualHours" runat="server" CloseAction="CloseButton" Modal="True" Width="370px"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcTaskActualHours"
    HeaderText="Enter Actual Hours:" AllowDragging="false" PopupAnimationType="None" EnableViewState="False">
    <ContentCollection>
        <dx:PopupControlContentControl ID="pcccTaskActualHours" runat="server">
            <dx:ASPxPanel ID="ASPxPanel2" runat="server" DefaultButton="btnTaskCompleteOk">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent2" runat="server">
                        <table style="width: 100%; height: 70px;">
                            <tr>
                                <td class="pcmCellCaption">
                                    <dx:ASPxTextBox ID="txtActualHours" runat="server" NullText="Actual Hours" ClientInstanceName="txtActualHours">
                                        <ValidationSettings ValidationGroup="actualHours" ErrorDisplayMode="Text">
                                            <RequiredField IsRequired="true" ErrorText="Please enter actual hours." />
                                            <RegularExpression ErrorText="Please enter actual hours." ValidationExpression="^(0*[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*)$" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="buttoncell">
                                        <span style="float: right; padding-right: 1px;">
                                            <dx:ASPxButton ID="btActualHoursCancel" runat="server" ClientInstanceName="btActualHoursCancel" Text="Cancel" Width="50px" ToolTip="Cancel" AutoPostBack="false"
                                                CausesValidation="false" Style="float: right; margin-right: 1px;">
                                                <ClientSideEvents Click="function(s, e) { pcTaskActualHours.Hide(); }" />
                                            </dx:ASPxButton>
                                        </span>

                                        <span style="float: right; padding-right: 1px;">
                                            <dx:ASPxButton ID="btnTaskCompleteOk" ValidationGroup="actualHours" ClientInstanceName="btnTaskCompleteOk" runat="server"
                                                Text="Task Complete" Width="50px" Style="float: right; margin-right: 5px" ToolTip="Complete Task"
                                                AutoPostBack="false">
                                                <ClientSideEvents Click="function(s,e){btnTaskCompleteOk_Validation(s,e);}" />
                                            </dx:ASPxButton>
                                        </span>
                                    </div>

                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ContentStyle>
        <Paddings PaddingBottom="5px" />
    </ContentStyle>
</dx:ASPxPopupControl>
