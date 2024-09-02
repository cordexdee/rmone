<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskList.ascx.cs" Inherits="uGovernIT.Web.TaskList" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">


    var errors = "";
    function Grid_BatchEditRowValidating(s, e) {
        if ("<%= keepActualHourMandatory%>" == "True") {
            var statusColumn = s.GetColumnByField("Status");
            var statusValue = e.validationInfo[statusColumn.index].value;

            var pctCompleteColumn = s.GetColumnByField("PercentComplete");
            var pctCompleteValue = e.validationInfo[pctCompleteColumn.index].value;

            var actualHoursColumn = s.GetColumnByField("TaskActualHours");
            var actualHoursValidInfo = e.validationInfo[actualHoursColumn.index];
            if ((statusValue == "Completed" || pctCompleteValue == 100) && (actualHoursValidInfo.value <= 0)) {
                actualHoursValidInfo.isValid = false;
                actualHoursValidInfo.errorText = "Please enter actual hours.";
            }
        }
        var assignedToColumn = s.GetColumnByField("AssignedTo");
        var assignedValidInfo = e.validationInfo[assignedToColumn.index];
        var rowIndex = e.visibleIndex;

        if (assignedValidInfo.value == null)
            return;

        var ss = s.batchEditApi;
        if (errors == "") {
            $.when(ResolveUsers(assignedValidInfo.value)).then(
                function (result) {
                    errors = result.errors;
                    var value = result.resolvedUsers;
                    var status = result.status;
                    ss.SetCellValue(rowIndex, "AssignedTo", value);
                    if (status == 'resolvedwitherrors')
                        gridTaskList.batchEditApi.ValidateRow(rowIndex);

                },
                function (status) { alert(status + ", you fail this time"); },
                function (status) { $("body").append(status); });
        }
        else if (errors != '') {
            assignedValidInfo.isValid = false;
            assignedValidInfo.errorText = errors;
            errors = '';
        }
    }


    $(document).ready(function () {
        var mtdwidth = $(".tasktoolbar td.middle").width();
        var mdivwidth = $(".tasktoolbar td.middle").find("div.middle").width();
        var divleftmargin = (mtdwidth / 2) - (mdivwidth / 2);
        $(".tasktoolbar td.middle").find("div.middle").css("margin-left", divleftmargin + "px");
        var actionheight = $('#divActionButtons').height();


        $('#<%= btMarkComplete.ClientID%>').addClass('disabled');
        $('#<%= lnkEdit.ClientID%>').addClass('disabled');
        $('#<%= lnkDecIndent.ClientID%>').addClass('disabled');
        $('#<%= lnkIncIndent.ClientID%>').addClass('disabled');
        $('#<%= lnkDelete.ClientID%>').addClass('disabled');
        $('#<%= lnkDuplicate.ClientID%>').addClass('disabled');

        $('#<%= imgLnkUp.ClientID%>').addClass('disabled');
        $('#<%= imgLnkDown.ClientID%>').addClass('disabled');
        ASPxPopupMenuAction.GetItem(4).SetEnabled(false);
        var item = '<%=DisableNewRecuringTask%>';
        var moduleName = '<%=ModuleName%>';
        if (item == "True" && moduleName == "SVCConfig") {
            ASPxPopupMenuAction.GetItem(4).SetEnabled(true);
            ASPxPopupMenuAction.GetItem(3).SetEnabled(false);
            // ASPxPopupMenuAction.GetItem(3).SetText("New Ticket");
        }

        ASPxPopupMenuAction.GetItem(1).SetEnabled(false);
        ASPxPopupMenuAction.GetItem(2).SetEnabled(false);


        var heightCSD = $("#s4-workspace").height() - 120;
        $(".dxgvCSD").css("height", heightCSD + "px");
        $(".dxgvCSD").css("background-color", "#fff");

        if ($('#<%= btMarkComplete.ClientID%>').hasClass('disabled')) {
            var buttonid = $('#<%= btMarkComplete.ClientID%>');
            $(buttonid).css('cursor', 'default');
        }
        if ($('#<%= lnkDelete.ClientID%>').hasClass('disabled')) {
            var buttonid = $('#<%= lnkDelete.ClientID%>');
            $(buttonid).css('cursor', 'default');
        }
        //ManageStageManagement();

    });



    var _closechildren = "closechildren<%=TicketPublicId %>";
    var _openchildren = "openchildren<%=TicketPublicId %>";
    var _selectedrowonpage = "selectedrowonpage<%=TicketPublicId %>";
    function InitalizejQuery() {
        window.parent.parent.$(".ms-dlgOverlay").height("inherit");

        //hide element which are coming as hidden from server side.
        //Server side hide is not working some reason.
        $(".hideElement").addClass("hide").removeClass("hideElement");

        $('.tasklistview-row').draggable({
            scroll: true,
            helper: 'clone',
            opacity: 1,
            handle: ".taskSNo",
            cursor: "move",
            cursorAt: { top: 7, left: 30 },
            drag: function (event, ui) {
                $.when($(ui.helper).find('td').hide()).done(function () { });
                if ($(ui.helper).find("[id^='div_title_']").parent().get(0).nodeName == 'TD') {
                    $(ui.helper).find("[id^='div_title_']").parent().prev().show();
                    $(ui.helper).find("[id^='div_title_']").parent().show();
                }
                else {
                    $(ui.helper).find("[id^='div_title_']").parent().parent().prev().show();
                    $(ui.helper).find("[id^='div_title_']").parent().parent().show();
                }


                $(ui.helper).find("[id^='div_title_']").find('.task-title').css('padding-right', '10px');
                $(ui.helper).find("[id^='div_title_']").find('.task-title').css('padding-left', '10px');
                $(ui.helper).find("[id^='div_title_']").find("[id^='actionButtons']").hide();
                $(ui.helper).css('border', '1px solid grey');

                if (ui.originalPosition.top != window.FindPosition(this).Top) {
                    ui.position.top = event.clientY - 7 + ui.originalPosition.top - window.FindPosition(this).Top;
                }
            }
        });

        $('.tasklistview-row').droppable({
            activeClass: "hover",
            accept: ".tasklistview-row",
            drop: function (event, ui) {

                var draggingRowKey = ui.draggable.find("input[type='hidden']").val();
                var targetRowKey = $(this).find("input[type='hidden']").val();
                gridTaskList.PerformCallback("DRAGROW|" + draggingRowKey + '|' + targetRowKey);
            }
        });
    }

    var addNewTaskExtraData = [];
    var currentColumn;
    var tempstartDate;
    function OnBatchStartEdit(s, e) {
        if (gridTaskList.GetSelectedKeysOnPage()[0] != $(gridTaskList.GetRow(e.visibleIndex)).attr("task")) {
            $.cookie(_selectedrowonpage, $(gridTaskList.GetRow(e.visibleIndex)).attr("task"));

            e.cancel = true;
        }

        $(".dxlbd").css('z-index', 9999)
        focusedcolumnindex = e.focusedColumn.index;
        currentColumn = e.focusedColumn.fieldName
        //client processing
        var keyIndex = s.GetColumnByField("Title").index;
        var key = e.rowValues[keyIndex].value;

        var condition;
        if (!key || key.indexOf("(Sprint:") == -1)
            condition = true;
        else
            condition = false;

        if (!condition) e.cancel = true;


        if (currentColumn == "StartDate") {
            tempstartDate = convertDate(new Date(s.batchEditApi.GetCellValue(e.visibleIndex, "StartDate")));
        }
        var jsObj = $(s.GetEditorByColumnIndex(5).inputElement);

    }
    var edit = false;
    function OnBatchEditEndEditing(s, e) {

        edit = true;
        if (currentColumn == "PercentComplete") {
            window.setTimeout(function () {

                var percentcomplete = s.batchEditApi.GetCellValue(e.visibleIndex, "PercentComplete");
                if (percentcomplete <= 0) {
                    s.batchEditApi.SetCellValue(e.visibleIndex, "Status", "Not Started");
                }
                else if (percentcomplete >= 100) {
                    s.batchEditApi.SetCellValue(e.visibleIndex, "Status", "Completed");
                }
                else {
                    s.batchEditApi.SetCellValue(e.visibleIndex, "Status", "In Progress");
                }

                var actualHours = s.batchEditApi.GetCellValue(e.visibleIndex, "TaskActualHours");
                if (percentcomplete != 0 && actualHours != 0) {
                    s.batchEditApi.SetCellValue(e.visibleIndex, "EstimatedRemainingHours", (((actualHours / percentcomplete) * 100).toFixed(1) - actualHours).toFixed(1) < 0 ? 0 : (((actualHours / percentcomplete) * 100).toFixed(1) - actualHours).toFixed(1));
                }

            }, 10);
        }
        if (currentColumn == "Status") {
            window.setTimeout(function () {

                var status = s.batchEditApi.GetCellValue(e.visibleIndex, "Status");


                if (status == "Not Started") {
                    s.batchEditApi.SetCellValue(e.visibleIndex, "PercentComplete", "0");
                }
                else if (status == "Completed") {
                    s.batchEditApi.SetCellValue(e.visibleIndex, "PercentComplete", "100");
                }
                else if (status != "Completed" && percentcomplete >= 100) {
                    s.batchEditApi.SetCellValue(e.visibleIndex, "PercentComplete", "90");
                }

                var percentcomplete = s.batchEditApi.GetCellValue(e.visibleIndex, "PercentComplete");
                var actualHours = s.batchEditApi.GetCellValue(e.visibleIndex, "TaskActualHours");
                if (percentcomplete != 0 && actualHours != 0) {
                    s.batchEditApi.SetCellValue(e.visibleIndex, "EstimatedRemainingHours", (((actualHours / percentcomplete) * 100).toFixed(1) - actualHours).toFixed(1) < 0 ? 0 : (((actualHours / percentcomplete) * 100).toFixed(1) - actualHours).toFixed(1));
                }

            }, 10);
        }


        if (currentColumn == "TaskEstimatedHours" || currentColumn == "TaskActualHours") {
            window.setTimeout(function () {
                var percentcomplete = s.batchEditApi.GetCellValue(e.visibleIndex, "PercentComplete");
                var estimateHours = s.batchEditApi.GetCellValue(e.visibleIndex, "TaskEstimatedHours");
                var actualHours = s.batchEditApi.GetCellValue(e.visibleIndex, "TaskActualHours");

                if (percentcomplete != 0 && actualHours != 0) {
                    s.batchEditApi.SetCellValue(e.visibleIndex, "EstimatedRemainingHours", (((actualHours / percentcomplete) * 100).toFixed(1) - actualHours).toFixed(1) < 0 ? 0 : (((actualHours / percentcomplete) * 100).toFixed(1) - actualHours).toFixed(1));
                }
                else {
                    s.batchEditApi.SetCellValue(e.visibleIndex, "EstimatedRemainingHours", (estimateHours - actualHours) < 0 ? 0 : (estimateHours - actualHours));
                }
            }, 10);
        }



    }


    function convertDate(str) {
        var date = new Date(str),
            mnth = ("0" + (date.getMonth() + 1)).slice(-2),
            day = ("0" + date.getDate()).slice(-2);
        return [mnth, day, date.getFullYear()].join("/");
    }

    function addNewItem_Click(visibleIndex) {
        if (typeof (gridTaskList) == 'undefined')
            return;
        var s = gridTaskList;

        var currentRow = s.GetRow(visibleIndex);
        var previousRow = currentRow;
        var nextRow = currentRow.nextSibling;

        var imageUrl = $(currentRow).find(".task-title img").attr("src");
        if ($(currentRow).attr("mode") == "collapse" && imageUrl && imageUrl != "" && imageUrl.indexOf("maximise.gif") != -1) {

            var currentRowParentTask = Number($(currentRow).attr("parenttask"));
            var nextRowParentTask = Number($(nextRow).attr("parenttask"));

            while (currentRowParentTask != nextRowParentTask) {
                nextRow = nextRow.nextSibling;
                nextRowParentTask = Number($(nextRow).attr("parenttask"));
            }
        }

        var level = 0;
        var parentTaskID = 0;
        var order = 0;
        var childCount = 0;
        var previousTaskID = 0;
        var nextTaskID = 0;
        if (previousRow != null) {
            //previousTaskID = Number($(previousRow).attr("task"));
        }
        s.AddNewRow();
        s.cpNewRowIndex = (s.cpNewRowIndex == undefined) ? -1 : s.cpNewRowIndex - 1;
        var newRow = s.GetRow(s.cpNewRowIndex);

        s.batchEditApi.SetCellValue(s.cpNewRowIndex, "PercentComplete", 0);
        s.batchEditApi.SetCellValue(s.cpNewRowIndex, "Status", "Not Started");
        s.batchEditApi.SetCellValue(s.cpNewRowIndex, "TaskEstimatedHours", 8);
        s.batchEditApi.SetCellValue(s.cpNewRowIndex, "TaskActualHours", 0);
        s.batchEditApi.SetCellValue(s.cpNewRowIndex, "EstimatedRemainingHours", 8);
        s.batchEditApi.SetCellValue(s.cpNewRowIndex, "StartDate", new Date("<%= DateTime.Now.ToString("MM-dd-yyyy")%>"));
        s.batchEditApi.SetCellValue(s.cpNewRowIndex, "DueDate", new Date("<%= DateTime.Now.ToString("MM-dd-yyyy")%>"));
        s.batchEditApi.SetCellValue(s.cpNewRowIndex, "Title", "New Task" + Math.abs(s.cpNewRowIndex));

        $(newRow).attr("parenttask", parentTaskID);
        $(newRow).attr("level", level);

        addNewTaskExtraData.push({ PreviousTaskID: previousTaskID, NextTaskID: nextTaskID, Index: s.cpNewRowIndex });

        gridExtraData.Set("batchAddItemsData", JSON.stringify(addNewTaskExtraData));

        currentRow.parentNode.insertBefore(newRow, nextRow);
        newRow.className += " newRow";

        setTimeout(function () {
            gridTaskList.batchEditApi.SetCellValue(gridTaskList.cpNewRowIndex, "Title", "New Task" + Math.abs(gridTaskList.cpNewRowIndex));
        }, 10);
    }

    function gridTaskList_BatchUpdate() {
        gridTaskList.UpdateEdit();
        gridTaskList.cpNewRowIndex = null;
        addNewTaskExtraData = [];
    }

    function gridTaskList_CancelBatchUpdate() {
        gridTaskList.cpNewRowIndex = null;
        addNewTaskExtraData = [];
        gridExtraData.Set("batchAddItemsData", "");
        gridTaskList.CancelEdit();
    }

    function OnInit(s, e) {
        AdjustSize();
        //document.getElementById("gridContainer").style.visibility = "";
    }

    function AdjustSize() {
        var height = Math.max(0, document.documentElement.clientHeight);
        gridTaskList.SetHeight(height);
    }

    function editTask(taskID, order) {
        var projectID = "<%= TicketPublicId %>";
        var moduleName = "<%= ModuleName %>";
        //order = $.trim(order);
        var params = "projectID=" + projectID + "&taskID=" + taskID + "&moduleName=" + moduleName;

        set_cookie("taskProjectID", "<%= TicketPublicId %>");
        set_cookie("projectTask", taskID);

        UgitOpenPopupDialog('<%= editTaskFormUrl %>', params, 'Edit Task ' + order, '1000px', '100', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function parseQuery(queryString) {
        var query = {};
        var pairs = (queryString[0] === '?' ? queryString.substr(1) : queryString).split('&');
        for (var i = 0; i < pairs.length; i++) {
            var pair = pairs[i].split('=');
            query[decodeURIComponent(pair[0])] = decodeURIComponent(pair[1] || '');
        }
        return query;
    }

    function openTaskEditDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        var param = parseQuery(params);
        var maxLength = 81;

        if (titleVal.length > maxLength)
            titleVal = titleVal.substring(0, maxLength) + "...";

        if (param.moduleName == "SVC") {
            window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
        }
        else {
            UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
        }

    }

    function popupMenuItemClick(s, e) {

        if (e.item.name == "NewTaskAtEnd")
            newTask(0);
        if (e.item.name == "NewTaskBelowCurrent")
            GetNewTask();
        if (e.item.name == "NewSubTask")
            newSubTask();
        if (e.item.name == "NewRecurringTask")
            newRepeatableTask(0);
        if (e.item.name == "NewTicket")
            editSVCTask(true, 0, 'ticket');
    }
    function newTask(taskID) {
        var projectID = "<%= TicketID %>";
        var moduleName = "<%= ModuleName %>";
        var publicId = "<%= TicketPublicId %>"
        var params = "control=taskedit&projectID=" + publicId + "&parentTaskID=" + taskID + "&moduleName=" + moduleName + "&ticketId=" + publicId + "&taskType=task";
        var sourceparams = "?control=TasksList";
        set_cookie("taskProjectID", "<%= TicketPublicId %>");
        set_cookie("projectTask", taskID);

        window.parent.UgitOpenPopupDialog('<%= editTaskFormUrl %>', params, 'New Task', '1000px', '100', 0, escape("<%= Request.Url.AbsolutePath %>") + sourceparams);
    }



    function editSVCTask(isNew, taskID, type) {
        var projectID = "<%= TicketID %>";
        var moduleName = "SVCConfig";
        var title = "Edit Task";
        if (isNew) {
            title = "New";
        }
        var height = "700px";
        var width = "1000px";
        if (type == "task") {
            height = "82";
        }
        var params = "control=taskedit&ticketId=" + projectID + "&projectID=" + projectID + "&taskID=" + taskID + "&moduleName=" + moduleName + "&taskType=" + type;
        window.parent.UgitOpenPopupDialog('<%= editTaskFormUrl %>', params, title, width, height, 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
    function GetNewTask() {

        var taskkey = gridTaskList.GetSelectedKeysOnPage()
        newTask(taskkey);
    }

    function newSubTask() {
        var taskkeyID = gridTaskList.GetSelectedKeysOnPage()
        //newTask(taskkey);
        var projectID = "<%= TicketID %>";
        var moduleName = "<%= ModuleName %>";
        var publicId = "<%= TicketPublicId %>"
        var params = "projectID=" + projectID + "&SubTask=true&parentTaskID=" + taskkeyID + "&moduleName=" + moduleName + "&ticketId=" + publicId + "&control=taskedit";

        set_cookie("taskProjectID", "<%= TicketPublicId %>");
        set_cookie("projectTask", taskkeyID);

        UgitOpenPopupDialog('<%= editTaskFormUrl %>', params, 'New Task', '800px', '100', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }


    function GetDuplicateTask() {
        if ($('#<%= lnkDuplicate.ClientID%>').hasClass('disabled')) {
            e.preventDefault();
            return false;
        }
        else {
            var taskkey = gridTaskList.GetSelectedKeysOnPage()
            var childcount = hdntask.Get("UGITChildCount");
            //var title = hdntask.Get("Title");
            hdntask.Set("taskid", taskkey);
            if (childcount > 0) {
                pcDuplicate.Show();
                return;
            }

            $('#<%= btnDuplicate.ClientID%>').trigger('click');
        }
    }


    function confirmBeforeDelete(obj) {
        if ($('#<%= lnkDelete.ClientID%>').hasClass('disabled')) {
            // a.preventDefault();
            return false;
        }
        else {
            var taskkey = gridTaskList.GetSelectedKeysOnPage()

            if (taskkey.length > 0) {
                var keys = gridTaskList.keys;
                var visibleRow = 0;
                $.each(keys, function (index, value) {
                    if (value == taskkey) {
                        visibleRow = index;
                        return;
                    }
                });

                var taskTitle = $(gridTaskList.GetRow(visibleRow)).find(".task-title span").text()

                if (confirm("Are you sure you want to delete task \"" + taskTitle + "\"? This will also delete any child task(s).")) {
                    loadingScreen();
                    return true;
                }
                else {
                    return false;
                }
            }
        }
    }

    function loadingScreen() {

        return true;
    }

    function GetEditTask(obj) {
        if ($('#<%= lnkEdit.ClientID%>').hasClass('disabled')) {
            e.preventDefault();
            return false;
        }
        else {

            var taskkey = gridTaskList.GetSelectedKeysOnPage()
            if (taskkey.length > 0) {
                var keys = gridTaskList.keys;
                var visibleRow = 0;
                $.each(keys, function (index, value) {
                    if (value == taskkey) {
                        visibleRow = index;
                        return;
                    }
                });

                // var taskTitle = $(gridTaskList.GetRow(visibleRow)).find(".task-title span").text()
                var taskItemOrder = $(gridTaskList.GetRow(visibleRow)).find(".taskSNo").text()
                taskItemOrder = $.trim(taskItemOrder);
                editTask(taskkey, taskItemOrder)
            }

        }
    }

    function editTask(taskID, order) {
        var projectID = "<%= TicketID %>";
        var moduleName = "<%= ModuleName %>";
        var TicketPublicId = "<%= TicketPublicId %>";
        //order = $.trim(order);
        var params = "projectID=" + TicketPublicId + "&ticketId=" + TicketPublicId + "&taskID=" + taskID + "&moduleName=" + moduleName + "&control=taskedit&taskType=task";

        set_cookie("taskProjectID", "<%= TicketPublicId %>");
        set_cookie("projectTask", taskID);

        window.parent.parent.parent.parent.UgitOpenPopupDialog('<%= editTaskFormUrl %>', params, 'Edit Task ' + order, '100', '100', 0);
    }
    var holdDataList = {};
    function showTaskActions(trObj, taskID) {
        $("#actionButtons" + taskID).css("display", "block");
        showHoldDetail(trObj);
        //var desc = $.trim(unescape($(trObj).find(".taskDesc").html()).replace(/\+/g, " "));
    }

    function hideTaskActions(trObj, taskID) {
        $("#actionButtons" + taskID).css("display", "none");
        aspxPopupHoldTooltip.Hide();
    }
    function showHoldDetail(trObj) {
        var holdObj = $(trObj).find(".progressbarhold");
        var status = $(trObj).attr("status");

        if (holdObj.length <= 0 && status == "On Hold") {
            var taskID = $(trObj).attr("task");

            var holdData = holdDataList[taskID.toString()];


            if (holdData) {
                $(".aspxPopupHoldTooltipContent").html(holdData.message);
            }
            else {
                $(".aspxPopupHoldTooltipContent").html("Loading..");
            }

            aspxPopupHoldTooltip.ShowAtElement(holdObj.get(0));
            var jsonData = { "name": "SVC", "id": taskID };
            var ajaxUrl = "GetTaskHoldReason";
            if ($(trObj).attr("type") == "Ticket") {
                ajaxUrl = "GetTicketHoldReason";
                var ticketID = $(trObj).attr("ticketid");
                jsonData = { "name": ticketID.split("-")[0], "id": ticketID };
            }
            $.ajax({
                type: "POST",
                url: "<%=ajaxHelperURL %>/" + ajaxUrl,
                data: JSON.stringify(jsonData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    try {
                        var json = $.parseJSON(message);
                        var htmlData = [];
                        htmlData.push("<div><b>On Hold Till: </b><span>" + json.holdtilldate + "</span></div>");
                        htmlData.push("<div><b>Reason: </b><span>" + json.holdreason + "</span></div>");
                        if (json.holdcomment != "")
                            htmlData.push("<div><b>Comment: </b><span>" + json.holdcomment + "</span></div>");
                        holdDataList[taskID.toString()] = { message: htmlData.join(""), json: json };
                        $(".aspxPopupHoldTooltipContent").html(htmlData.join(""));
                    } catch (ex) {
                        $(".aspxPopupHoldTooltipContent").html("No Data Avaiable");
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });


        }
    }

    function showTaskActionButtons(trObj) {
        //$("#divActionButtons #overlay").hide();
        //$('#divActionButtons td').removeClass("disabled");
        $('#<%= btMarkComplete.ClientID%>').removeClass('disabled');
        $('#<%= btMarkComplete.ClientID%>').css('cursor', 'pointer');
        $('#<%= lnkEdit.ClientID%>').removeClass('disabled');
        $('#<%= lnkDecIndent.ClientID%>').removeClass('disabled');
        $('#<%= lnkIncIndent.ClientID%>').removeClass('disabled');
        $('#<%= lnkDelete.ClientID%>').removeClass('disabled');
        $('#<%= lnkDelete.ClientID%>').css('cursor', 'pointer');
        $('#<%= lnkDuplicate.ClientID%>').removeClass('disabled');

        $('#<%= imgLnkUp.ClientID%>').removeClass('disabled');
        $('#<%= imgLnkDown.ClientID%>').removeClass('disabled');

        ASPxPopupMenuAction.GetItem(1).SetEnabled(true);
        ASPxPopupMenuAction.GetItem(2).SetEnabled(true);


    }

    function expandAllTask(doDefault) {

        var trs = $(".tasklistview .tasklistview-row[parentTask='0']")
        var minLength = 2;
        if (!doDefault) {
            minLength = 0;
        }

        if (trs.length > minLength) {
            $.each(trs, function (i, item) {
                var maxMinIcon = $(item).find(".task-title img[colexpand]");
                var parentID = $(item).attr("parenttask");
                var taskID = $(item).attr("task");
                var level = $(item).attr("level");
                if (maxMinIcon.length > 0) {
                    OpenChildren(level, taskID, maxMinIcon.get(0))
                }
            });
        }

        $('.dxgvHSDC').css('padding-right', '17px');
        $('.dxgvHSDC div').css('width', '100%');
    }

    function collapseAllTask(doDefault) {

        var trs = $(".tasklistview .tasklistview-row[parentTask='0']")
        var minLength = 2;
        if (!doDefault) {
            minLength = 0;
        }
        if (trs.length > minLength) {
            $.each(trs, function (i, item) {
                var maxMinIcon = $(item).find(".task-title img[colexpand]");
                var parentID = $(item).attr("parenttask");
                var taskID = $(item).attr("task");
                var level = $(item).attr("level");
                if (maxMinIcon.length > 0) {
                    CloseChildren(level, taskID, maxMinIcon.get(0))
                }
            });
        }
        if ($('.dxgvCSD table').height() <= $('.dxgvCSD').height()) {
            $('.dxgvHSDC').css('padding-right', '0px');
        }
        $('.dxgvHSDC div').css('width', '100%');
    }


    function CloseChildren(level, id, imageObj) {

        imageObj.setAttribute("src", "/Content/images/maximise.gif");
        imageObj.setAttribute("onclick", "event.cancelBubble=true; OpenChildren('" + level + "', '" + id + "', this)");

        var currentTask = $(".tasklistview .tasklistview-row[task='" + id + "']");
        currentTask.attr("mode", "collapse");

        var childTasks = $(".tasklistview .tasklistview-row[parenttask='" + id + "']");
        for (var i = 0; i < childTasks.length; i++) {
            var task = childTasks.get(i);
            //$(task).css("display", "none");
            $(task).addClass("hide");
            //$(task).addClass(hideElement);
            if ($(task).find(".task-title img[colexpand]").length > 0) {
                CloseChildren($(task).attr("level"), $(task).attr("task"), $(task).find(".task-title img[colexpand]").get(0))
            }
        }
        var closechildren = $.cookie(_closechildren);
        if (closechildren == null || closechildren.length == 0) {
            closechildren = [];
        }
        else {
            closechildren = $.parseJSON(closechildren);
        }
        var isexists = false;
        $(closechildren).each(function () {
            if (this.ID == id) {
                isexists = true;
                return false;
            }
        });
        if (!isexists) {
            closechildren.push({ ID: id });
        }
        var jsonString = JSON.stringify(closechildren);
        $.cookie(_closechildren, jsonString, { path: "/" });

        ///openchildren remove code
        var openchildren = $.cookie(_openchildren);
        if (openchildren == null || openchildren.length == 0) {
            openchildren = [];
        }
        else {
            openchildren = $.parseJSON(openchildren)
        }

        for (var i = openchildren.length - 1; i >= 0; i--) {
            var obj = openchildren[i];
            if (obj.ID == id) {
                openchildren.splice(i, 1);
                break;
            }
        }
        jsonString = JSON.stringify(openchildren);
        $.cookie(_openchildren, jsonString, { path: "/" });
    }

    function OpenChildren(level, id, imageObj) {

        imageObj.setAttribute("src", "/Content/images/minimise.gif");
        imageObj.setAttribute("onclick", "event.cancelBubble=true; CloseChildren('" + level + "', '" + id + "', this)");

        var currentTask = $(".tasklistview .tasklistview-row[task='" + id + "']");
        currentTask.attr("mode", "expand");

        var childTasks = $(".tasklistview .tasklistview-row[parenttask='" + id + "']");
        for (var i = 0; i < childTasks.length; i++) {
            var task = childTasks.get(i);
            $(task).removeClass("hide");
            //$(task).removeClass("hideElement");
            if ($(task).find(".task-title img[colexpand]").length > 0) {
                OpenChildren($(task).attr("level"), $(task).attr("task"), $(task).find(".task-title img").get(0))
            }
        }
        var openchildren = $.cookie(_openchildren);
        if (openchildren == null || openchildren.length == 0) {
            openchildren = [];
        }
        else {
            openchildren = $.parseJSON(openchildren)
        }
        var isexists = false;
        $(openchildren).each(function () {
            if (this.ID == id) {
                isexists = true;
                return false;
            }
        });
        if (!isexists) {
            openchildren.push({ ID: id });
        }
        var jsonString = JSON.stringify(openchildren);
        $.cookie(_openchildren, jsonString, { path: "/" });

        ///closechildren remove code
        var closechildren = $.cookie(_closechildren);
        if (closechildren == null || closechildren.length == 0) {
            closechildren = [];
        }
        else {
            closechildren = $.parseJSON(closechildren)
        }
        for (var i = closechildren.length - 1; i >= 0; i--) {
            var obj = closechildren[i];
            if (obj.ID == id) {
                closechildren.splice(i, 1);
                break;
            }
        }
        jsonString = JSON.stringify(closechildren);
        $.cookie(_closechildren, jsonString, { path: "/" });

    }

    function confirmTaskDeleteMessage() {
        var message = "This will DELETE all tasks for this project.\nThis will also delete ALL resource allocations for this project.\n\nAre you sure want to proceed?";
        if (confirm(message)) {
            //loadingScreen();
            return true;
        }
        else {
            return false;
        }
    }

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


    function moveUp(s, e) {
        if ($('#<%= imgLnkUp.ClientID%>').hasClass('disabled')) {
            e.preventDefault();
            return false;
        }
        else {

            var trowfound = false;
            var secondrowId = "";
            var selectedrowId = gridTaskList.GetSelectedKeysOnPage()[0];
            var leveltrow = $("tr[task=" + gridTaskList.GetSelectedKeysOnPage()[0] + "]")[0].attributes['level'];
            var trows = $("tr[level=" + leveltrow.value + "]");
            for (var i = 0; i < trows.length; i++) {
                var taskid = $(trows[i]).attr('task');
                if (taskid == selectedrowId) {
                    if (i == 0) {
                        //alert('This is first Task you cant move up.');
                        return false;
                    }
                    secondrowId = $(trows[i - 1]).attr('task');
                    break;
                }
            }
            gridTaskList.PerformCallback("MOVEUP|" + selectedrowId + '|' + secondrowId);
        }
    }

    function moveDown(s, e) {
        if ($('#<%= imgLnkDown.ClientID%>').hasClass('disabled')) {
            e.preventDefault();
            return false;
        }
        else {
            var trowfound = false;
            var secondrowId = "";
            var selectedrowId = gridTaskList.GetSelectedKeysOnPage()[0];
            var leveltrow = $("tr[task=" + gridTaskList.GetSelectedKeysOnPage()[0] + "]")[0].attributes['level'];
            var trows = $("tr[level=" + leveltrow.value + "]");
            trows.each(function () {
                if (trowfound) {
                    secondrowId = $(this).attr('task');
                    return false;
                }
                else {
                    var taskid = $(this).attr('task');
                    if (taskid == selectedrowId) {
                        trowfound = true;
                    }
                }
            });

            gridTaskList.PerformCallback("MOVEDOWN|" + selectedrowId + '|' + secondrowId);
        }
    }


    function DecIndent(e, obj) {
        if ($('#<%= lnkDecIndent.ClientID%>').hasClass('disabled')) {
            e.preventDefault();
            return false;
        }
        else {
        }
    }

    function IncIndent(e, obj) {
        if ($('#<%= lnkIncIndent.ClientID%>').hasClass('disabled')) {
            e.preventDefault();
            return false;
        }
        else {
        }
    }

    function getGanttChart(obj) {
        var Module = '<%=ModuleName%>';
        var ganttType = "2"; /* for PMM task */
        var ticketPublicID = '<%=TicketPublicId %>';  /* PMM Ticket Id */

        var params = "GanttType=" + ganttType + "&TicketID=" + ticketPublicID + "&Module=" + Module;

        var url = '<%=ganttReportUrl %>';
        window.parent.UgitOpenPopupDialog(url, params, 'Gantt Chart', '100', '100', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function DisplayTaskCalendarView(obj) {
        var ModuleName = '<%=ModuleName%>';
        var projectPublicId = '<%=TicketPublicId %>';  /* PMM Ticket Id */
        var calendarURL = '<%=calendarURL %>';
        // var url = "/_layouts/15/ugovernit/delegatecontrol.aspx?control=taskcalender&moduleName=" + ModuleName + "&ProjectPublicId=" + projectPublicId;
        var url = calendarURL + "?control=taskcalender&moduleName=" + ModuleName + "&ProjectPublicId=" + projectPublicId;
        window.parent.UgitOpenPopupDialog(url, '', 'Calendar Task View', '850px', '640px', 0, '');
        return false;
    }

    var focusedcolumnindex = 0;
    function onGridRowClick(s, e) {
        if (e.visibleIndex != -1) {
            var datarow = s.GetDataRow(e.visibleIndex);
            $(datarow).removeClass("hideElement");

            var taskid = $(s.GetDataRow(e.visibleIndex)).attr("task");
            var behaviour = $(s.GetDataRow(e.visibleIndex)).attr("behaviour");
            if (behaviour == "Ticket") {
                $('#<%= btMarkComplete.ClientID%>').addClass('disabled');
                $('#<%= lnkEdit.ClientID%>').addClass('disabled');
                $('#<%= lnkDecIndent.ClientID%>').addClass('disabled');
                $('#<%= lnkIncIndent.ClientID%>').addClass('disabled');
            //  $('#<%= lnkDelete.ClientID%>').addClass('disabled');
                $('#<%= lnkDuplicate.ClientID%>').addClass('disabled');

                $('#<%= imgLnkUp.ClientID%>').addClass('disabled');
                $('#<%= imgLnkDown.ClientID%>').addClass('disabled');
            }
            var hdnchildcount = $(s.GetDataRow(e.visibleIndex)).find("#hdnchildcount" + taskid)
            var childcount = $(hdnchildcount).val();
            hdntask.Set("UGITChildCount", childcount);
            focusedcolumnindex = e.visibleIndex;
            EnableDisableUpDownButton(e.visibleIndex);
        }
        else {
            $('#<%= btMarkComplete.ClientID%>').addClass('disabled');
            $('#<%= lnkEdit.ClientID%>').addClass('disabled');
            $('#<%= lnkDecIndent.ClientID%>').addClass('disabled');
            $('#<%= lnkIncIndent.ClientID%>').addClass('disabled');
            //  $('#<%= lnkDelete.ClientID%>').addClass('disabled');
            $('#<%= lnkDuplicate.ClientID%>').addClass('disabled');

            $('#<%= imgLnkUp.ClientID%>').addClass('disabled');
            $('#<%= imgLnkDown.ClientID%>').addClass('disabled');
        }

        // ManageStageManagement();
        // s.AddNewRow();
    }

    function EnableDisableUpDownButton(visibleIndex) {

        var taskid = $(gridTaskList.GetDataRow(visibleIndex)).attr('task');
        var leveltrow = $("tr[task=" + taskid + "]")[0].attributes['level'];
        var trows = $("tr[level=" + leveltrow.value + "]");
        for (var i = 0; i < trows.length; i++) {

            var taskidnew = $(trows[i]).attr('task');
            if (taskidnew == taskid) {
                if (i == 0) {
                    $('#<%= imgLnkUp.ClientID%>').addClass('disabled');

                }
                if (i == trows.length - 1) {
                    $('#<%= imgLnkDown.ClientID%>').addClass('disabled');
                }
                if (gridTaskList.GetDataRow(visibleIndex + 1) == null)
                    $('#<%= imgLnkDown.ClientID%>').addClass('disabled');
            }
        }
    }


    function MarkAsComplete(event) {
        //debugger;

        <%--if ($('#<%= btMarkComplete.ClientID%>').hasClass('disabled')) {

            // e.preventDefault();
            return false;
        }
        else {--%>
        //var selectedid = gridTaskList.GetSelectedKeysOnPage()[0];
        $('.registrationRequest-loading').show();
        var selectedid = $(event.target || event.srcElement || event.mainElement).closest('tr').attr('task');
        var object = gridTaskList.GetRow(focusedcolumnindex);
        return doTaskCompleted(object, selectedid);
    }

    var baselineTaskThreshold = Number("<%=baselineTaskThreshold %>");
    function doTaskCompleted(obj, taskID) {
        //debugger;
        set_cookie("taskProjectID", "<%= TicketPublicId %>");
        set_cookie("projectTask", taskID);

        var returnBit = true;
        var taskTr = $("#PMMTaskRowLevel_" + taskID);
        var childTaskCount = Number(taskTr.attr("childcount"));

        var baselinePrompt = false;
        var confirmChildTaskCompletePromt = false;
        if (baselineTaskThreshold && baselineTaskThreshold > 0 && baselineTaskThreshold <= childTaskCount) {
            baselinePrompt = true;
        }

        if (childTaskCount && childTaskCount > 0) {
            confirmChildTaskCompletePromt = true;
        }

        if (confirmChildTaskCompletePromt) {
            if (!confirm("This action will also mark all child tasks as Completed. Would you like to proceed?")) {
                returnBit = false;
            }
        }

        if (baselinePrompt && returnBit) {
            if (confirm("You are changing the status of " + childTaskCount + " tasks to Completed. Would you like to create a baseline first?")) {
                $("#<%= hdnActionType.ClientID %>").val("BaselineAndSave");
            }
        }

        var keys = $.parseJSON(gridTaskList.GetKeyValues());
        var rowIndex = keys.indexOf(taskID);
        gridTaskList.SelectRowOnPage(rowIndex, true);

        if ("<%= keepActualHourMandatory%>" == "True") {
            txtActualHours.SetText("");
            gridTaskList.GetSelectedFieldValues("TaskActualHours", loadTaskActualHour_data)

            returnBit = false;
        }

        if (returnBit) {
            __doPostBack('<%= btMarkComplete.ClientID %>', taskID);
            loadingScreen();
        }

        return returnBit;
    }

    function editTaskForRelationship(isNew, taskID, type) {
        newTask(0);
    }

    function OpenNewTicketWindow() {
        ClientPopupControl.Hide();
        var obj = $("select[name$='ddlModuleDetail']");
        var Url = $(obj).find("option[value='" + $(obj).val() + "']").attr("Url");
        javascript: (window.parent) ? window.parent.UgitOpenPopupDialog(Url, "TicketId=0&ParentId=<%=TicketPublicId %>&SourceTicketId=<%=TicketPublicId %>", "New " + $(obj).val() + " Ticket", "auto", "auto", "<%=Server.UrlEncode(Request.Url.AbsolutePath)%>", '') : UgitOpenPopupDialog(Url, "TicketId=0&parentId=<%=TicketPublicId %>&SourceTicketId=<%=TicketPublicId %>", "New " + $(obj).val() + " Ticket", "auto", "auto", "<%=Server.UrlEncode(Request.Url.AbsolutePath)%>", ''); return false;

    }

    function gotoTaskWorkFlow() {
        var serviceworkflowlink = '<%=ServiceTaskWorkFlow%>';
        var params = "TicketId=<%=TicketPublicId%>" + "&ModuleName=" + "<%=ModuleName%>";
        window.parent.UgitOpenPopupDialog(serviceworkflowlink, params, "Workflow", 90, 120, false, escape(window.location.href));
    }
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $('.SVCEdit-taskGrid').parents().eq(3).addClass('taskGrid-body');
    });
    function UpdateGridHeight() {
        gridTaskList.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        gridTaskList.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>
<asp:HiddenField ID="exportURL" runat="server" />
<asp:Panel ID="projectTasksEditMode" runat="server" CssClass="SVCEdit-taskGrid">
    <table cellpadding="0" cellspacing="0" style="border-collapse: collapse;" width="100%">
        <tr>
            <td>
                <style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
                    .allocationblockmian {
                        float: left;
                        width: auto;
                    }

                    .allocationblockinner {
                        float: left;
                        margin: 5px;
                        border: 1px solid #DCDCDC;
                    }

                    .allocationblockinner_sub {
                        float: left;
                        margin: 11px;
                        border: 1px solid #DCDCDC;
                        width: 98%;
                        margin-top: 0px;
                    }

                    .searchview-m {
                        float: left;
                        width: 97%;
                        padding: 12px;
                        border: 1px solid #DCDCDC;
                    }

                    .detailview-m {
                        float: left;
                        padding: 12px;
                        width: 97%;
                        padding-top: 0px;
                    }

                    .detailviewresource-m {
                        float: left;
                        width: 100%;
                    }

                    .detailviewallocation-m {
                        float: left;
                        width: 100%;
                    }

                    .editview-m {
                        float: left;
                        width: 100%;
                        padding: 4px;
                    }

                    .searchview-m-m {
                        float: left;
                        width: 100%;
                        padding-bottom: 15px;
                    }

                    .searchview-m-m1 {
                        float: left;
                        width: 100%;
                    }

                    .searchview-m-m2 {
                        float: left;
                        width: 100%;
                    }

                    .inputwidth {
                        width: 200px;
                    }

                    .heading {
                        font-weight: bold;
                    }

                    .editforminputwidth {
                        width: 150px;
                    }

                    .allocationlistviewpager {
                        padding: 3px 5px 3px 5px;
                    }

                    .editviewtable td, .editviewtable th {
                        border: 1px solid #DCDCDC;
                        text-align: center;
                        padding: 2px;
                    }

                        .editviewtable td td {
                            border: none;
                        }

                    .closeeditpupup {
                        display: inline-block;
                        height: 12px;
                        overflow: hidden;
                        position: relative;
                        width: 16px;
                    }

                    .closeeditpupupimg {
                        border-width: 0;
                        left: 0 !important;
                        position: absolute;
                        top: -661px !important;
                    }

                    .editviewheader-m {
                        border: 1px solid #DCDCDC;
                        border-bottom: none;
                    }

                    .editformtitletd {
                        padding-left: 5px;
                    }

                    .editformtitle {
                        font-weight: bold;
                    }

                    .pupopsavebt {
                        text-align: right !important;
                        padding-right: 5px;
                    }

                    widhtfirstcell {
                        width: 100px;
                    }

                    .detailviewheader th {
                        text-align: left;
                    }

                    .detailviewedititem td {
                        text-align: left;
                    }

                    .detailviewinsertitem td {
                        text-align: left;
                    }

                    .detailviewitem td {
                        text-align: left;
                    }

                    .newbuttontd {
                        padding-right: 4px;
                        padding-top: 3px;
                        text-align: right;
                    }

                    .filteredlist {
                        float: left;
                        width: 100%;
                    }

                    .buttonpanel {
                        padding: 2px 5px;
                    }

                    .allocationpagerpanel {
                        float: left;
                        width: 100%;
                    }

                    .selectedresourcelb {
                        font-weight: bold;
                        padding: 2px 0px 0px 2px;
                    }

                    .selectedresourcelbheading {
                        padding: 2px 5px 0px 2px;
                    }

                    /*.fleft {
                        float: left;
                    }*/

                    .detailviewresource-main {
                        float: left;
                        width: 98%;
                        margin: 11px;
                        margin-bottom: 0px;
                    }

                    .ms-viewheadertr .ms-vh2-gridview {
                    }

                    .ms-listviewtable {
                        border: 1px solid #DCDCDC !important;
                        border-collapse: separate !important;
                        background: #F8F8F8 !important;
                    }

                    .paddingfirstcell {
                        padding-left: 6px !important;
                    }

                    .managerdropdown {
                        min-width: 150px;
                    }

                    .detailviewheading-m {
                        float: left;
                        width: 98%;
                        padding: 0px 12px;
                    }

                    .detailviewheading {
                        font-weight: bold;
                    }


                    .firstcellwidth {
                        width: 60px;
                    }

                    .datetimectr {
                        width: 65px;
                    }

                    body select {
                        font-size: 12px;
                    }

                    .lbparentid {
                        float: left;
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

                    .ShowMe {
                        display: block;
                    }

                    .HideMe {
                        display: none;
                    }

                    .rightalign {
                        text-align: right;
                    }

                    .ms-listviewtable > tbody > tr > th {
                        border: 1px solid #fff !important;
                    }

                    .ms-listviewtable > tbody > tr > td {
                        border: 1px solid #fff !important;
                        height: 28px;
                    }

                    .edit-block {
                        float: left;
                        width: 100%;
                    }

                    .edit-block1 {
                        float: left;
                        width: 100%;
                        padding-left: 10px;
                    }

                    .edit-block2 {
                        float: left;
                        width: 100%;
                    }

                    .edit-block3 {
                        float: left;
                        width: 100%;
                    }

                    .edit-element-block {
                        float: left;
                        width: 100%;
                    }

                        .edit-element-block > label {
                            font-weight: bold;
                            float: left;
                            width: 100%;
                        }

                        .edit-element-block > span {
                            float: left;
                            width: 150px;
                        }

                    .edit-actionblock {
                        float: left;
                        width: 50px;
                        height: 115px;
                        text-align: center;
                        padding-top: 50px;
                    }

                        .edit-actionblock > span {
                        }

                    .edit-splitblock {
                        float: left;
                        width: 46%;
                    }

                        .edit-splitblock label {
                            float: left;
                            width: 100%;
                            font-weight: bold;
                        }

                        .edit-splitblock span {
                            float: left;
                            width: 100%;
                        }

                    .edit-title {
                        width: 300px;
                    }

                    .edit-percentcomplete {
                        width: 100px;
                    }

                    .edit-status {
                        width: 150px;
                    }

                    .edit-assignto {
                        width: 300px;
                    }

                    .ms-inputuserfield {
                        width: 300px !important;
                    }

                    .ms-usereditor {
                        width: 300px !important;
                    }

                        .ms-usereditor .ms-error {
                            display: none;
                        }

                    .edit-note {
                        width: 250px;
                        height: 46px;
                    }

                    .edit-startdate {
                        width: 150px;
                    }

                    .edit-duedate {
                        width: 150px;
                    }

                    .edit-predecessor {
                        width: 300px;
                    }

                    tr.myDragClass td {
                        color: yellow;
                        background-color: black;
                    }

                    .showDrag {
                        background-image: url(/_layouts/15/images/uGovernIT/updown.gif);
                        background-repeat: no-repeat;
                        background-position: right;
                        cursor: move;
                        z-index: 1000000;
                    }

                    .dragHandle {
                        width: 18px;
                    }

                    .resource-action {
                        float: right;
                        padding-left: 2px;
                    }

                    .ModuleBlock {
                        background: none repeat scroll 0 0 #ECE8D3;
                        border: 4px double #FCCE92;
                        position: absolute;
                        z-index: 1;
                    }

                    /*.fright {
                        float: right;
                    }*/

                    /*.saveastemplatediv {
                        float: right;
                        padding: 2px 0px 0px 2px;
                    }*/

                        .saveastemplatediv img {
                            cursor: pointer;
                        }

                    .percentcomplete {
                        text-align: center;
                    }

                    .status {
                        text-align: center;
                    }

                    .assignedto {
                        text-align: center;
                    }

                    .predecessor {
                        text-align: center;
                    }

                    .estimatedhours {
                        text-align: center;
                    }

                    .actualhours {
                        text-align: center;
                    }

                    .startdate {
                        text-align: center;
                    }

                    .duedate {
                        text-align: center;
                    }

                    .duration {
                        text-align: center;
                    }

                    .CellProperty {
                        overflow: visible !important;
                    }

                    #ms-belltown-table {
                        width: 100% !important;
                    }

                    /*task toolbar*/
                    /*.tasktoolbar {
                        border: none;
                        width: 100%;
                        margin-left: 2px;
                    }*/

                        /*.tasktoolbar div.fleft {
                            float: left;
                            display: inline;
                        }*/

                        /*.tasktoolbar div.fright {
                            float: right;
                            display: inline;
                        }*/

                        .tasktoolbar div.middle {
                            float: left;
                            display: inline;
                            text-align: center;
                            position: relative;
                        }

                        .tasktoolbar div img,
                        .tasktoolbar div input {
                            float: left;
                            cursor: pointer;
                            margin: 0px 3px;
                        }

                            .tasktoolbar div img.calender {
                                position: relative;
                                top: 1px;
                            }

                        /*.tasktoolbar .criticalbutton {
                            margin-left: 5px;
                            margin-top: 2px;
                        }*/

                        .tasktoolbar div.deleteallbutton {
                            /*margin-right:50px;*/
                        }



                        /*.tasktoolbar div.fright input.AutoAdjustSchedules {
                            margin-top: 4px;
                        }*/

                        .tasktoolbar td {
                            border-width: 1px;
                            border-color: #808080;
                        }

                    .disabled {
                        background-color: #fff !important;
                        opacity: 0.3;
                    }

                    .tasktoolbar fieldset {
                        padding: 0px 5px 4px;
                    }

                    /*.tasktoolbar .left {
                        margin-left: -3px;
                    }*/

                    .tasktoolbar .right {
                        margin-right: -3px;
                    }

                    /*.tasktoolbar .border {
                        background-color: #fff;
                        border: 1px solid #B8B8B8;
                        padding: 3px 15px;
                        border-radius:4px;
                    }*/

                    .reportitem {
                        border-bottom: 1px solid black;
                        float: left;
                        padding: 5px;
                        padding-top: 5px;
                        width: 94%;
                        cursor: pointer;
                        color: black;
                    }

                    .tasklistview-row:hover .taskSNo {
                        background: url("/content/images/Sorting-16.png") no-repeat;
                        background-position: left 0px top 9px;
                        cursor: pointer;
                    }
                </style>
                <div style="float: left; width: 100%;">
                </div>
                <div style="float: left; width: 100%;">
                    <div id="tasktoolbar" runat="server" class="tasktoolbar">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 col-xs-12 noPadding-xs">

                                <div class="fleft border left" title="Views">
                                    <img src="/Content/images/expand-all-new.png" title="Expand All" onclick="expandAllTask(false)" />
                                    <img onclick="collapseAllTask(false)" src="/Content/images/collapse-all-new.png" title="Collapse All" />

                                    <div id="ganttDiv" runat="server" class="fleft">
                                        <img src="/Content/images/GanttChartNew.png" title="Gantt View" onclick="javascript:return getGanttChart(this)">
                                        <img src="/Content/images/calendarNew.png" title="Calendar View" class="calender" onclick="javascript:return DisplayTaskCalendarView(this)" />
                                    </div>

                                    <input type="hidden" id="iscritical" runat="server" />
                                    <asp:ImageButton ID="btncriticalpath" class="criticalbutton" runat="server" OnClientClick="loadingPanel.Show();" OnClick="btncriticalpath_Click"
                                        ToolTip="Show Critical Path" CommandArgument="1" ImageUrl="~/Content/images/criticalInactiveNew.png" />
                                </div>

                            </div>
                             
                            <div class="middle col-md-4 col-sm-4 col-xs-12 noPadding-xs">

                                <div id="divActionButtons" class="middle border" title="Task Actions">
                                    <table>
                                        <tr>
                                            <td>
                                                <img id="imgAction" runat="server" src="/Content/images/newTask.png" alt="New Task" title="New Task" style="cursor: pointer;" />

                                                <dx:ASPxPopupMenu ID="ASPxPopupMenuAction" GutterWidth="0px" runat="server" PopupElementID="imgAction" ShowPopOutImages="True"
                                                    ClientInstanceName="ASPxPopupMenuAction" CloseAction="MouseOut"
                                                    PopupHorizontalAlign="OutsideRight" PopupVerticalAlign="TopSides" PopupAction="LeftMouseClick">
                                                    <Items>
                                                        <dx:MenuItem Name="NewTaskAtEnd" Text="New Task At End" ItemStyle-BorderBottom-BorderWidth="1px" ItemStyle-BorderBottom-BorderColor="Black" ItemStyle-BorderBottom-BorderStyle="Solid">
                                                        </dx:MenuItem>
                                                        <dx:MenuItem Name="NewTaskBelowCurrent" Text="New Task Below Current" ItemStyle-BorderBottom-BorderWidth="1px" ItemStyle-BorderBottom-BorderColor="Black" ItemStyle-BorderBottom-BorderStyle="Solid"></dx:MenuItem>
                                                        <dx:MenuItem Name="NewSubTask" Text="New Sub Task" ItemStyle-BorderBottom-BorderWidth="1px" ItemStyle-BorderBottom-BorderColor="Black" ItemStyle-BorderBottom-BorderStyle="Solid"></dx:MenuItem>
                                                        <dx:MenuItem Name="NewRecurringTask" Text="New Recurring Task"></dx:MenuItem>
                                                        <dx:MenuItem Name="NewTicket" Text="New Ticket"></dx:MenuItem>
                                                    </Items>
                                                    <ClientSideEvents ItemClick="function(s, e) { popupMenuItemClick(s,e);}" />
                                                    <ItemStyle Width="160px"></ItemStyle>
                                                </dx:ASPxPopupMenu>

                                            </td>
                                            <td>
                                                <asp:ImageButton OnClientClick="return MarkAsComplete(event);" CommandName="MarkAsComplete" ID="btMarkComplete"
                                                    CssClass="markascomplete-action markAsCompleteBtn" ToolTip="Mark as Complete"
                                                    runat="server" ImageUrl="/Content/images/accept-symbol.png" OnClick="btMarkComplete_Click" />
                                            </td>
                                            <td>
                                                <img id="lnkEdit" runat="server" src="/Content/images/editNewIcon.png" onclick="GetEditTask()" alt="Edit" title="Edit" style="cursor: pointer;" />

                                            </td>
                                            <td>
                                                <asp:ImageButton runat="server" ID="lnkDecIndent" OnClick="lnkDecIndent_Click" OnClientClick="DecIndent(event,this)"
                                                    ImageUrl="/Content/images/d-indent.png" BorderWidth="0"
                                                    ToolTip="Decrease Indent" />
                                            </td>
                                            <td>
                                                <asp:ImageButton runat="server" ID="lnkIncIndent" OnClick="lnkIncIndent_Click" OnClientClick="IncIndent(event,this)"
                                                    ImageUrl="/Content/images/i-indent.png" BorderWidth="0"
                                                    ToolTip="Increase Indent" />
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="lnkDelete" runat="server" Text="Delete" OnClick="lnkDelete_Click" CssClass="projectPlan_deleteIcon"
                                                    ImageUrl='/Content/images/redNew_delete.png' BorderWidth="0" ForeColor="Brown" ToolTip="Delete"
                                                    OnClientClick="return confirmBeforeDelete(this)" />
                                            </td>
                                            <td>
                                                <img id="lnkDuplicate" runat="server" src="/Content/images/duplicateNew.png" onclick="GetDuplicateTask()" alt="Duplicate" title="Duplicate" style="cursor: pointer;" />
                                            </td>
                                            <td>

                                                <img id="imgLnkUp" runat="server" src="/Content/images/move-up.png" onclick="moveUp()" alt="UP" title="Move UP" style="cursor: pointer;" />

                                            </td>
                                            <td>
                                                <img id="imgLnkDown" runat="server" src="/Content/images/move-down.png" onclick="moveDown()" alt="Down" title="Move Down" style="cursor: pointer;" />

                                            </td>
                                            <td>
                                                <asp:Button Text="" ID="btnDuplicate" Style="display: none" runat="server" OnClick="btnDuplicate_Click" />

                                                <asp:HiddenField ID="hdncopyChild" runat="server" />
                                                <dx:ASPxPopupControl ID="pcDuplicate" runat="server" ClientInstanceName="pcDuplicate"
                                                    HeaderText="Duplicate Summary Task" CloseAction="CloseButton" Modal="True"
                                                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                                                    <ContentCollection>
                                                        <dx:PopupControlContentControl>
                                                            <dx:ASPxPanel ID="pnlcontrol" runat="server" Width="300px" DefaultButton="btnOK">
                                                                <PanelCollection>
                                                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                                                        <asp:Table ID="tParameter" runat="server" Width="100%">
                                                                            <asp:TableRow>
                                                                                <asp:TableCell ColumnSpan="2">
                                                                                    <asp:Label ID="lblMsg" Text="Do you want to duplicate the child task(s) also?" runat="server" />
                                                                                </asp:TableCell>
                                                                            </asp:TableRow>
                                                                            <asp:TableRow>
                                                                                <asp:TableCell ColumnSpan="2">
                                                                                    &nbsp;
                                                                                </asp:TableCell>
                                                                            </asp:TableRow>
                                                                            <asp:TableRow ID="TableRow1" runat="server">
                                                                                <asp:TableCell ID="TableCell2" runat="server">
                                                                                    <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" AutoPostBack="false"
                                                                                        UseSubmitBehavior="false" Style="text-align: left; margin-right: 8px">
                                                                                        <ClientSideEvents Click="function(s, e) { pcDuplicate.Hide();}" />
                                                                                    </dx:ASPxButton>
                                                                                </asp:TableCell><asp:TableCell ID="TableCell1" runat="server">
                                                                                    <dx:ASPxButton ID="btnNo" runat="server" Text="  No  " AutoPostBack="true"
                                                                                        Style="float: right; margin-right: 8px; display: inline" OnClick="btnNo_Click">
                                                                                        <ClientSideEvents Click="function(s,e){ pcDuplicate.Hide();  }" />
                                                                                    </dx:ASPxButton>
                                                                                    <dx:ASPxButton ID="btnOK" runat="server" Text="  Yes  " AutoPostBack="true"
                                                                                        Style="float: right; margin-right: 8px; display: inline" OnClick="btnOK_Click">
                                                                                        <ClientSideEvents Click="function(s,e){pcDuplicate.Hide(); }" />
                                                                                    </dx:ASPxButton>
                                                                                </asp:TableCell>
                                                                            </asp:TableRow>
                                                                        </asp:Table>
                                                                    </dx:PanelContent>
                                                                </PanelCollection>
                                                            </dx:ASPxPanel>
                                                        </dx:PopupControlContentControl>
                                                    </ContentCollection>
                                                </dx:ASPxPopupControl>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12 noPadding-xs">

                                <div id="rightToolDiv" runat="server" class="fright border right xsrightToolDiv" title="Project Actions">
                                    <asp:Panel ID="pTemplateActions" runat="server" CssClass="saveastemplatediv fright">
                                        <img src="/Content/images/importTasks.png" title="Import Task" id="btImportTaskTemplate" runat="server" />
                                        <img src="/Content/images/importTasks.png" title="Load Template" id="btLoadTempate" runat="server" visible="false" />
                                        <img src="/Content/images/saveAsTemplate.png" title="Save As Template" id="btSaveAsTemplate" runat="server" />
                                    </asp:Panel>
                                    <asp:Panel ID="exportImportPanel" runat="server" CssClass="fright">
                                    </asp:Panel>
                                    <asp:ImageButton ID="btnAutoAdjustSchedules" class="AutoAdjustSchedules" runat="server" OnClientClick="loadingPanel.Show();"
                                        OnClick="btnAutoAdjustSchedules_Click" ToolTip="Auto Adjust Schedule" ImageUrl="/Content/images/autoAdjustSchedule.png" />
                                    <div class="fright deleteallbutton">
                                        <asp:ImageButton ID="btnTaskDelete" runat="server" OnClick="btnTaskDelete_Click" OnClientClick="return confirmTaskDeleteMessage();"
                                            ToolTip="DELETE All Tasks" ImageUrl="~/Content/images/redNew_delete.png" />
                                    </div>

                                </div>

                            </div>
                        </div>

                    </div>
                    
                   
                   
                                
                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                        <asp:Panel ID="PMMlistPanel" runat="server" CssClass="listcontainerdiv" Width="100%">
                            <div>

                                <asp:HiddenField ID="hdnActionType" runat="server" />
                                <asp:HiddenField ID="order" runat="server" Value="" />
                                <asp:HiddenField ID="changeOrder" runat="server" />
                                <asp:HiddenField ID="hdnPreventEstimateHours" runat="server" />
                                <dx:ASPxHiddenField ID="hdntask" runat="server" ClientInstanceName="hdntask"></dx:ASPxHiddenField>

                            </div>
                            <dx:ASPxHiddenField ID="gridExtraData" runat="server" ClientInstanceName="gridExtraData"></dx:ASPxHiddenField>
                            <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel" Text="Please Wait .." Modal="true"></dx:ASPxLoadingPanel>
                            <div id="addTasksContainer" class="gridLinkbutton" runat="server" visible="false">
                                <table style="align-content:center">
                                    <tr>
                                    <td>
                                        <a id="btnGraphicView" class="newTask_link" runat="server" href="javascript:" visible="false">
                                            <div class="btn-holder">
                                                <div class="add-link-btn">
                                                    <img src="/Content/Images/workflow.png"/>
                                                </div>
                                                <div class="link-lable"><span>Graphic View</span></div>
                                            </div>
                                        </a>
                                    </td>
                                        <td><a id="btNewTask" class="newTask_link" runat="server" style="display: none;" href="javascript:" onclick="editTaskForRelationship(true, 0, 'task')">
                                    <div class="btn-holder">
                                        <div class="add-link-btn">
                                            <img src="/Content/Images/new_task.png"/>
                                        </div>
                                        <div class="link-lable"><span>New Task</span></div>
                                    </div>
                                </a></td>
                                        <td><a id="aAddItem" class="relatedToExistingTicket_link" runat="server" style="display: none;">
                                    <div class="btn-holder">
                                        <div class="add-link-btn">
                                            <img src="/Content/Images/related-link.png" />
                                        </div>
                                        <div class="link-lable"><span>Relate To Existing Ticket</span></div>
                                    </div>
                                </a></td>
                                        <td><a id="aAddNewSubTicket" class="newSubTaslk_link" runat="server" style="padding-left: 10px; display: none;" href="javascript:;">
                                    <div class="btn-holder">
                                        <div class="add-link-btn">
                                            <img src="/Content/Images/plus-blue.png" />
                                        </div>
                                        <div class="link-lable"><span>Add New Sub Ticket</span></div>
                                    </div>
                                </a></td>
                                    </tr>
                                </table>
                                
                                
                                
                                <dx:ASPxPopupControl ID="PopupControl" runat="server" CloseAction="CloseButton" CssClass="subTicket_PopUp_Container"
                                    PopupElementID="aAddNewSubTicket" PopupVerticalAlign="Above" PopupHorizontalAlign="RightSides"
                                    ShowFooter="True" Width="300px" Height="100px" HeaderText="Add new sub Ticket" ClientInstanceName="ClientPopupControl" CloseButtonImage-Url="~/Content/Images/close-red-big.png">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl ID="PopupControlContentControl" runat="server">
                                            <div class="all-select">
                                                <label class="dropDown-fieldLabel">Select Module</label>
                                                <asp:DropDownList ID="ddlModuleDetail" AutoPostBack="false" CssClass="aspxDropDownList" OnLoad="DdlModuleDetail_Load" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                        </dx:PopupControlContentControl>
                                    </ContentCollection>
                                    <FooterTemplate>
                                        <div class="actionBtn-wrap">
                                            <dx:ASPxButton ID="UpdateButton" runat="server" Text="Create" AutoPostBack="False"
                                                ClientSideEvents-Click="function(s, e) {OpenNewTicketWindow();}" cssClass="primary-blueBtn">
                                            </dx:ASPxButton>
                                        </div>
                                    </FooterTemplate>

                                </dx:ASPxPopupControl>
                            </div>
                    <%-- Spinner for Complete Task Popup --%>
                            <div class="spiner_wrap">
                                 <div class="registrationRequest-loading" style="display:none;"><i class="grid_spinnerIcon icon-spinner icon-spin icon-2x"></i></div>
                            </div>
                            <ugit:ASPxGridView ID="gridTaskList" Width="100%" CssClass="customgridview homeGrid"  OnRowCommand="gridTaskList_RowCommand"  ClientInstanceName="gridTaskList" runat="server" KeyFieldName="ID" EnableCallBacks="true"
                                Styles-Cell-Wrap="False" OnCustomColumnDisplayText="gridTaskList_CustomColumnDisplayText" OnHtmlDataCellPrepared="gridTaskList_HtmlDataCellPrepared">
                                <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                                <columns></columns>
                                 <settingscommandbutton>
                                    <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                                    <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                                </settingscommandbutton>
                                <TotalSummary>

                                    <dx:ASPxSummaryItem FieldName="EstimatedRemainingHours" SummaryType="Sum" DisplayFormat="{0}" />
                                    <dx:ASPxSummaryItem FieldName="ActualHours" SummaryType="Sum" DisplayFormat="{0}" />
                                    <dx:ASPxSummaryItem FieldName="EstimatedHours" SummaryType="Sum" DisplayFormat="{0}" />
                                    <dx:ASPxSummaryItem FieldName="Duration" SummaryType="Sum" DisplayFormat="{0} Days" />
                                    <%--<dx:ASPxSummaryItem FieldName="PercentComplete" SummaryType="Average" DisplayFormat="Tasks are {0}% Complete" />--%>
                                </TotalSummary>

                                <SettingsLoadingPanel Mode="Disabled" />
                                <SettingsPager Mode="ShowAllRecords">
                                </SettingsPager>

                                <SettingsBehavior AllowSort="false" AllowSelectSingleRowOnly="true" AllowSelectByRowClick="true" />
                                <Settings ShowFooter="false" ShowColumnHeaders="true" ShowStatusBar="Hidden" />
                                <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                                    <SelectedRow BackColor="#D9D5D5" ForeColor="Black"></SelectedRow>
                                    <Table CssClass="tasklistview"></Table>
                                    <Row CssClass="homeGrid_dataRow"></Row>
                                    <Header CssClass="homeGrid_headerColumn"></Header>
                                </Styles>
                                <ClientSideEvents Init="OnInit" RowClick="onGridRowClick" />

                                <%--BatchEditEndEditing="OnBatchEditEndEditing" BatchEditRowValidating="function(s, e) { Grid_BatchEditRowValidating(s,e);  }" 
                                    Init="function(s, e) { AdjustSize(s);  }" EndCallback="function(s, e) { AdjustSize(s); }" RowClick="onGridRowClick"
                                    CustomButtonClick="function(s,e){ CustomButtonClick_Click(s,e); }" />--%>
                            </ugit:ASPxGridView>
                            <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                                ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                                    UpdateGridHeight();
                                });
                                ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                                    UpdateGridHeight();
                                });
                            </script>
                            <dx:ASPxGlobalEvents ID="ge" runat="server">
                                <ClientSideEvents ControlsInitialized="InitalizejQuery" BeginCallback="function(s, e) { loadingPanel.Show(); }" EndCallback="function(s, e) { InitalizejQuery(); loadingPanel.Hide(); }" />
                            </dx:ASPxGlobalEvents>
                            <%--<table>
                                <tr id="addTasksContainer" runat="server" visible="false">
                                    <td>
                                        <div style="float: left; margin: -25px 0 0 0;">
                                            <a id="btNewTask" runat="server" style="display: none;" href="javascript:" onclick="editTaskForRelationship(true, 0, 'task')">
                                                <img id="Img3" runat="server" src="/Content/images/add_icon.png" />
                                                New Task
                                            </a>

                                            <a id="aAddItem" runat="server" style="padding-left: 10px; display: none;" href="">
                                                <img id="Img1" runat="server" src="/Content/images/add_icon.png" />
                                                <asp:Label ID="LblAddItem" runat="server" Text="Relate To Existing Ticket"></asp:Label>
                                            </a>
                                            <a id="aAddNewSubTicket" runat="server" style="padding-left: 10px; display: none;" href="javascript:;">
                                                <img id="Img2" runat="server" src="/Content/images/add_icon.png" />
                                                <asp:Label ID="Label1" runat="server" Text="Add New Sub Ticket"></asp:Label>
                                            </a>

                                            <dx:ASPxPopupControl ID="PopupControl" runat="server" CloseAction="CloseButton"
                                                PopupElementID="aAddNewSubTicket" PopupVerticalAlign="Above" PopupHorizontalAlign="RightSides"
                                                ShowFooter="True" Width="300px" Height="85px" HeaderText="Add New Sub Ticket" ClientInstanceName="ClientPopupControl">
                                                <ContentCollection>
                                                    <dx:PopupControlContentControl ID="PopupControlContentControl" runat="server">
                                                        <div style="vertical-align: middle">
                                                            <asp:DropDownList Height="22px" ID="ddlModuleDetail" AutoPostBack="false" OnLoad="DdlModuleDetail_Load" runat="server">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </dx:PopupControlContentControl>
                                                </ContentCollection>
                                                <FooterTemplate>
                                                    <div style="float: right; margin: 3px;">
                                                        <dx:ASPxButton ID="UpdateButton" runat="server" Text="Create" AutoPostBack="False"
                                                            ClientSideEvents-Click="function(s, e) {OpenNewTicketWindow();}" />
                                                    </div>

                                                </FooterTemplate>
                                            </dx:ASPxPopupControl>

                                        </div>
                                    
                                    </td>
                                </tr>
                            </table>--%>
                        </asp:Panel>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <dx:ASPxPopupControl ID="aspxPopupHoldTooltip" runat="server" CloseAction="CloseButton"
        PopupVerticalAlign="Above" PopupHorizontalAlign="RightSides"
        ShowFooter="false" ShowHeader="false" Width="300px" Height="50px" HeaderText="Hold Details" ClientInstanceName="aspxPopupHoldTooltip">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div style="vertical-align: middle" class="aspxPopupHoldTooltipContent">
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Panel>
