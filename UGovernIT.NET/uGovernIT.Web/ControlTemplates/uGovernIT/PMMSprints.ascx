<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMMSprints.ascx.cs" Inherits="uGovernIT.Web.PMMSprints" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .sampleClass {
        display: none;
    }

    .productBackLog {
        top: 25px;
        /*display: inline;*/
        position: absolute;
        vertical-align: top;
    }

        .productBackLog label {
            vertical-align: bottom;
        }

    .pickervaluevontainer {
        background: #fff;
        float: left;
        width: 212px;
        height: 190px;
        display: none;
        z-index: 10000;
        position: absolute;
        overflow-y: auto;
        overflow-x: auto;
        border: 1px solid;
        top: 10px;
        left: 190px;
    }

    .disableButton {
        opacity: 0.5 !important;
    }

    .lnkTitle, .lnkTitleSprint, .lnkTitleRelease {
        color: black !important;
    }

        .lnkTitle:hover, .lnkTitleSprint:hover, .lnkTitleRelease:hover {
            text-decoration: underline;
            cursor: pointer;
        }

    .btnDelete {
        float: left;
        margin: 1px;
        color: #fff !important;
        background: url(/Content/images/firstnavbgRed.png) repeat-x;
        cursor: pointer;
        padding: 6px;
        padding-top: 3px;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        width: 160px;
    }

    .ms-standardheader {
        text-align: right;
    }

    .full-width {
        width: 98%;
    }

    .dxmodalSys .dxmodalTableSys.dxpc-contentWrapper .dxpc-content {
        display: inline-block;
    }

    /*.popupheight {
        height:500px;
    }*/

</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function setSelectedDocumentDetails(documentData, documentId) {


        //Link multiple files
       <%-- for (var index = 0; index < documentData.length; index++) {

            $("div#<%=bindMultipleLink.ClientID%>").append('<a id=file_' + documentId[index] + ' onclick="return downloadSelectedFile(' + documentId[index] + ')" style="cursor: pointer;">' + documentData[index] +
                '</a>' + "<img src='/content/images/close-red.png' id= img_" + documentId[index] + " class='' onclick='deleteLinkedDocument(\"" + documentId[index] + "\")'></img><br/>");

        }
        //close Document folder page
        window.parent.CloseWindowCallback(document.location.href);

        $("#<%=fileAttchmentId.ClientID%>").val(documentId.join(","));--%>
    }
    var rowKey = -1;
    var focussedRowIndex = 0;
    var sourceGrid = "";
    var selectedTask = "";
    var sourceRowId = -1;
    var isExists = false;
    var selectedSprint = "";
    function InitalizejQuery() {

        //if (document.URL.indexOf("viewType=1") < 0) {
        if (1 == 1) {

            $('.sprintgrid-row').draggable({
                opacity: 1,
                cursor: "move",
                multiple: true,
                helper: function () {
                    var gridId = "";
                    var id = $(this).attr("id");

                    if (id.indexOf("_gvProductBacklog_") != -1) {
                        gridId = "<%=gvProductBacklog.ClientID%>";
                        var selected = $("#" + gridId + " tr.sprintgrid-selectedrow");
                        if (selected.length === 0) {
                            selected = $("#" + gridId + " tr.sprintgrid-focusedrow");
                        }
                        if (selected.length === 1) {
                            selected = $(this).clone();
                            $.when($(selected).find('td').hide()).done(function () { });
                            $($(selected).find('td')[0]).show();
                            $($(selected).find('td')[1]).show();
                            $($(selected).find('td')[1]).find('a').css('padding-right', '10px');
                            $($(selected).find('td')[1]).find('a').css('padding-left', '10px');
                            $(selected).css('border', '1px solid grey');
                            return selected;
                        }

                        var container = $('<div/>').attr('id', 'draggingContainer');
                        var clone = selected.clone();

                        $(clone).each(function (i, item) {
                            $.when($(item).find('td').hide()).done(function () { });
                            $($(item).find('td')[0]).show();
                            $($(item).find('td')[1]).show();
                            $($(item).find('td')[1]).find('a').css('padding-right', '10px');
                            $($(item).find('td')[1]).find('a').css('padding-left', '10px');
                            $(item).css('border', '1px solid grey');
                        });


                        container.append(clone);
                        return container;

                    }
                    else {
                        var selected = $(this).clone();
                        $.when($(selected).find('td').hide()).done(function () { });
                        $($(selected).find('td')[0]).show();
                        $($(selected).find('td')[1]).show();
                        $($(selected).find('td')[1]).find('a').css('padding-right', '10px');
                        $($(selected).find('td')[1]).find('a').css('padding-left', '10px');
                        $(selected).css('border', '1px solid grey');
                        return selected;
                    }
                },
                start: function (ev, ui) {
                    var $sourceElement = $(ui.helper.context);
                    var $draggingElement = $(ui.helper);

                    var $sourceElement = $(this);

                    //$.when($(ui.helper).find('td').hide()).done(function () { });
                    //$($(ui.helper).find('td')[0]).show();
                    //$($(ui.helper).find('td')[1]).show();
                    //$($(ui.helper).find('td')[1]).find('a').css('padding-right', '10px');
                    //$($(ui.helper).find('td')[1]).find('a').css('padding-left', '10px');
                    //$(ui.helper).css('border', '1px solid grey');



                    //find key
                    isExists = false;
                    var id = $(this).attr("id");
                    if (id.indexOf("_gvProductBacklog_") != -1) {
                        sourceGrid = "gvProductBacklog";
                        sourceRowId = $sourceElement.index() - 1;
                        var selectedRowKeys = gvProductBacklog
                        rowKey = gvProductBacklog.GetRowKey($sourceElement.index() - 1);
                        if (gvProductBacklog.GetSelectedRowCount() > 0) {
                            gvProductBacklog.GetSelectedFieldValues('ID;Title', OnGetSelectedFieldValues);
                        }
                        else {
                            gvProductBacklog.GetRowValues(sourceRowId, 'Title', OnGetRowValues);
                        }
                    }
                    else if (id.indexOf("_gvSprints_") != -1) {
                        sourceGrid = "gvSprints";
                        rowKey = gvSprints.GetRowKey($sourceElement.index() - 1);
                    }
                    else if (id.indexOf("_gvSprintTasks_") != -1) {
                        sourceGrid = "gvSprintTasks";
                        sourceRowId = $sourceElement.index() - 1;
                        gvSprintTasks.GetRowValues(sourceRowId, 'Title', OnGetRowValues);
                        rowKey = gvSprintTasks.GetRowKey($sourceElement.index() - 1);
                    }
                },
                cursorAt: { top: 7, left: 30 }
            });
            var settings = function () {
                return {
                    tolerance: "intersect",
                    drop: function (ev, ui) {

                        focussedRowIndex = gvSprints.GetFocusedRowIndex();
                        var $targetElement = $(this);
                        var rowid = $(this).attr("id");
                        if (rowid != undefined) {
                            if (rowid.indexOf("_gvSprints_") != -1 && sourceGrid == "gvProductBacklog") {
                                isExists = true;
                                gvSprints.GetRowValues($targetElement.index() - 1, 'Title', function (value) {
                                    if (selectedTask instanceof Array) {
                                        for (var i = 0; i < selectedTask.length; i++) {
                                            var sprintID = gvSprints.GetRowKey($targetElement.index() - 1);
                                            CallbackPanel.PerformCallback("SprintDrop|" + selectedTask[i][0] + '|' + sprintID);
                                        }
                                    }
                                    else {
                                        var sprintID = gvSprints.GetRowKey($targetElement.index() - 1);
                                        CallbackPanel.PerformCallback("SprintDrop|" + rowKey + '|' + sprintID);
                                    }
                                });
                            }
                            else if (rowid.indexOf("_gvSprintTasks_") != -1 && sourceGrid == "gvProductBacklog") {
                                isExists = true;

                                gvSprints.GetRowValues(focussedRowIndex, 'Title', function (value) {
                                    if (selectedTask instanceof Array) {
                                        for (var i = 0; i < selectedTask.length; i++) {

                                            //var sprintTaskID = gvSprintTasks.GetRowKey($targetElement.index() - 1);
                                            var sprintTaskID = gvSprints.GetRowKey(focussedRowIndex);
                                            CallbackPanel.PerformCallback("SprintTaskDrop|" + selectedTask[i][0] + '|' + sprintTaskID);
                                        }
                                    }
                                    else {

                                        //var sprintTaskID = gvSprintTasks.GetRowKey($targetElement.index() - 1);
                                        var sprintTaskID = gvSprints.GetRowKey(focussedRowIndex);
                                        CallbackPanel.PerformCallback("SprintTaskDrop|" + rowKey + '|' + sprintTaskID);
                                    }
                                });
                            }
                            else if (rowid.indexOf("_gvProductBacklog_") != -1 && sourceGrid == "gvSprintTasks") {
                                isExists = true;
                                gvSprints.GetRowValues(focussedRowIndex, 'Title', function (value) {
                                    var sprintTaskID = gvProductBacklog.GetRowKey($targetElement.index() - 1);
                                    CallbackPanel.PerformCallback("RemoveSprintTask|" + rowKey + '|' + sprintTaskID);
                                });
                            }
                            else if (rowid.indexOf("_gvReleases_") != -1 && sourceGrid == "gvProductBacklog") {
                                isExists = true;
                                gvReleases.GetRowValues($targetElement.index() - 1, 'Title', function (value) {
                                    if (selectedTask instanceof Array) {
                                        for (var i = 0; i < selectedTask.length; i++) {
                                            var releaseID = gvReleases.GetRowKey($targetElement.index() - 1);
                                            CallbackPanel.PerformCallback("ProjectReleaseDrop|" + selectedTask[i][0] + '|' + releaseID);
                                        }
                                    }
                                    else {
                                        var releaseID = gvReleases.GetRowKey($targetElement.index() - 1);
                                        CallbackPanel.PerformCallback("ProjectReleaseDrop|" + rowKey + '|' + releaseID);
                                    }
                                });
                            }
                            else if (!isExists) {
                                if (sourceGrid == "gvProductBacklog") {
                                    isExists = true;
                                    if (selectedTask instanceof Array) {
                                        var taskArray = [];
                                        for (var i = 0; i < selectedTask.length; i++) {
                                            taskArray.push(selectedTask[i][0]);
                                        }
                                        var droppedRowId = gvProductBacklog.GetRowKey($targetElement.index() - 1);
                                        CallbackPanel.PerformCallback("RowReOrder|gvProductBacklog|" + taskArray + '|' + droppedRowId);
                                    }
                                    else {
                                        var droppedRowId = gvProductBacklog.GetRowKey($targetElement.index() - 1);
                                        CallbackPanel.PerformCallback("RowReOrder|gvProductBacklog|" + rowKey + '|' + droppedRowId);
                                    }
                                }
                                else if (sourceGrid == "gvSprints") {
                                    isExists = true;
                                    var droppedRowId = gvSprints.GetRowKey($targetElement.index() - 1);
                                    CallbackPanel.PerformCallback("RowReOrder|gvSprints|" + rowKey + '|' + droppedRowId);
                                }
                                else if (sourceGrid == "gvSprintTasks") {
                                    isExists = true;
                                    gvSprints.GetRowValues(focussedRowIndex, 'Title', function (value) {
                                        var droppedRowId = gvSprintTasks.GetRowKey($targetElement.index() - 1);
                                        CallbackPanel.PerformCallback("RowReOrder|gvSprintTasks|" + rowKey + '|' + droppedRowId);
                                    });
                                }
                                else if (sourceGrid == "gvReleases") {
                                    isExists = true;
                                    gvReleases.GetRowValues(focussedRowIndex, 'Title', function (value) {
                                        var droppedRowId = gvReleases.GetRowKey($targetElement.index() - 1);
                                        CallbackPanel.PerformCallback("RowReOrder|gvReleases|" + rowKey + '|' + droppedRowId);
                                    });
                                }
                            }
                        }
                        else if ($targetElement.hasClass("sprintgrid-emptyrow") != -1 && sourceGrid == "gvProductBacklog") {
                            isExists = true;
                            gvSprints.GetRowValues(focussedRowIndex, 'Title', function (value) {
                                if (selectedTask instanceof Array) {
                                    for (var i = 0; i < selectedTask.length; i++) {
                                        CallbackPanel.PerformCallback("FirstSprintTaskDrop|" + selectedTask[i][0]);
                                    }
                                }
                                else {
                                    CallbackPanel.PerformCallback("FirstSprintTaskDrop|" + rowKey);
                                }
                            });
                        }
                    }
                };
            };
            $('.sprintgrid-row').droppable(settings());
            //    $("#ctl00_PlaceHolderMain_ctl00_CallbackPanel_contenSplitter_gvSprintTasks_DXMainTable").droppable(settings());
            if (contenSplitter.GetPaneByName("ProductBackLog").IsCollapsed()) {
                $("#divSprintAction").css("left", "276px");
                $("#divShowProductBacklog").show();
            }
            else {
                $("#divSprintAction").css("left", "664px");
                $("#divShowProductBacklog").hide();
            }

            $("#divSprintPoup").parent().css("padding", "0px");
            //$("#divDurationPopup").parent().css("padding", "0px");
            var checkboxes = $('#<%=chkSprints.ClientID %>').find('input:checkbox');
            checkboxes.bind('click', function () {
                var selectedIndex = checkboxes.index($(this));

                var items = $('#<% = chkSprints.ClientID %> input:checkbox');
                for (i = 0; i < items.length; i++) {
                    if (i == selectedIndex)
                        items[i].checked = true;
                    else
                        items[i].checked = false;
                }
            });

             $(".lnkEditSprint").bind("click", function () {

              var sprintID=$(this).attr('SprintId');
              var sprintTitle=$(this).attr('SprintTitle');

                var url = "<%=SprintRequestUrl%>";
                url = url + "&IsNew=false&folderName=Sprints&isTabActive=true&sprintID=" +sprintID+"&sprintTitle="+sprintTitle;
                window.parent.UgitOpenPopupDialog(url, '', 'Edit Sprint - ' + sprintTitle, '500px', '300px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        
            });
            $(".lnkEditRelease").bind("click", function () {

              var releaseId=$(this).attr('ReleaseId');
                var url = "<%=ReleaseRequestUrl%>";
                url = url + "&IsNew=false&folderName=Sprints&isTabActive=true&releaseID=" +releaseId;
                window.parent.UgitOpenPopupDialog(url, '', '', '500px', '300px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        
            });



            $(".lnkTitleSprint").bind("click", function () {
                $("#<%=hdnSprintId.ClientID%>").val($(this).attr('SprintId'));
                $("#<%=hdnSprintTitle.ClientID%>").val($(this).text());
                $("#<%=hdnSource.ClientID%>").val("Sprint");
                $("#<%=btnHidden.ClientID%>").click();
            });

            $(".lnkTitleRelease").unbind("click").bind("click", function () {

                $("#<%=hdnReleaseId.ClientID%>").val($(this).attr('ReleaseId'));
                $("#<%=hdnReleaseTitle.ClientID%>").val($(this).text());
                $("#<%=hdnSource.ClientID%>").val("Release");
                $("#<%=btnHidden.ClientID%>").click();
            });
            $(".lnkTitle").unbind('click').bind("click", function () {
                var url = "<%=RequestUrl%>";
                url = url + "&IsNew=false&folderName=Sprints&isTabActive=true&TaskId=" + $(this).attr("TaskId");
                window.parent.UgitOpenPopupDialog(url, '', 'Edit Task - ' + $(this).attr("TitleItemOrder") + ": " + $(this).text(), '500px', '550px', 0, escape("<%= Request.Url.AbsolutePath %>"));
            });
            $(".dateControl").bind("change", function () {
                if ($(this).attr('id').indexOf("_dtcStartDate_") > 0) {
                    $("#<%=lbStartDate.ClientID%>").css("display", "none");
                }
                else if ($(this).attr('id').indexOf("_dtcEndDate_") > 0) {
                    $("#<%=dtcDateError.ClientID%>").css("display", "none");
                }
            });
            if (gvSprintTasks.GetVisibleRowsOnPage() <= 0 && $(".hdnSprintTaskContainer").length == 0) {
                var gridId = $("#<%=gvSprintTasks.ClientID%>");
                $("#<%=gvSprintTasks.ClientID%> .sprintgrid-emptyrow").droppable(settings());
            }
        }

        if (document.URL.indexOf("viewType=1") > 0) {
            $(".lnkTitle").unbind('click').bind("click", function () {
                var url = "<%=RequestUrl%>";
                url = url + "&IsNew=false&TaskId=" + $(this).attr("TaskId");
                window.parent.UgitOpenPopupDialog(url, '', 'Edit Task - ' + $(this).attr("TitleItemOrder") + ": " + $(this).text(), '500px', '550px', 0, escape("<%= Request.Url.AbsolutePath %>"));
            });
        }
    }

    function txtTitleChange() {
        $("#<%=lblTitleError.ClientID%>").css("display", "none");
    }
    function txtDataChange(obj) {

        errorLabelObj = $(obj).siblings('span[errorLabel]');

        if (errorLabelObj != undefined) {
            $(errorLabelObj).text('');
        }
    }

    function openDialog(html, titleVal, source, stopRefesh) {
        var divContanier = document.createElement("div");
        divContanier.innerHTML = unescape(html).replace(/\+/g, " ").replace(/\~/g, "'");
        var htmlObj = $("body").append(divContanier);
        $(divContanier).width(550);
        $(divContanier).height(250);
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
        SP.UI.ModalDialog.showModalDialog(options);

        if (closePopupNotificationId != null) {
            if (navigator.appName == "Microsoft Internet Explorer") {
                window.document.execCommand('Stop');
            }

            SP.UI.Notify.removeNotification(closePopupNotificationId);
            closePopupNotificationId = null;
        }
    }
    function OnGetSelectedFieldValues(values) {
        selectedTask = values;
    }
    function OnGetRowValues(value) {
        selectedTask = value
    }

    function OnRowClickEventTask(s, e) {
        if (s.IsRowSelectedOnPage(e.visibleIndex)) {
            s.UnselectRows(e.visibleIndex);
        }
        else {
            s.SelectRowOnPage(e.visibleIndex);
        }
        return false;
    }
    function OnSelectionChanged(s, e) {
        if (s.GetSelectedRowCount() > 0) {
            $("#btMoveTasks").removeAttr('disabled');
            $("#btMoveTasks").removeClass("disableButton");
        }
        else {
            $("#btMoveTasks").attr('disabled', 'disabled');
            $("#btMoveTasks").addClass("disableButton");
        }
    }
    function OnSelectionChangedSprintTasks(s, e) {
        if (gvSprintTasks.GetSelectedRowCount() > 0) {
            $("#<%=lnkMoveToBackLog.ClientID%>").removeAttr('disabled');
            $("#<%=lnkMoveToBackLog.ClientID%>").removeClass("disableButton");
        }
        else {
            $("#<%=lnkMoveToBackLog.ClientID%>").attr('disabled', 'disabled');
            $("#<%=lnkMoveToBackLog.ClientID%>").addClass("disableButton");
        }
    }
    function btMoveTasksClick() {
        if ($("#btMoveTasks").attr("disabled") == "disabled") {

        }
        else {
            if ($("#divSprintPicker").css("display") == "none") {
                $("[id*=chkSprints] input:checkbox").prop('checked', false);
                $("#divSprintPicker").show();
            }
            else {
                HidePopUp();
            }
        }

    }
    function btMoveTasksToBackLogClick() {
        if (gvSprintTasks.GetSelectedRowCount() > 0) {
            if (confirm("Are you sure you want to move the selected task(s) to the product backlog?")) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }

    function HidePopUp() {

        $("#divSprintPicker").hide();
    }
    function btAddTaskClick() {
        $("#divAddTask").show();
    }
    function lnkDeleteTask() {

        if ($("#<%=lnkDeleteTask.ClientID%>").attr("disabled") == "disabled") {
            return false;
        }
        else {

            if (gvProductBacklog.GetSelectedRowCount() > 0) {
                if (confirm("Are you sure you want to delete the selected task(s)?")) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                alert("Please select task(s) to delete.")
                return false;
            }
        }
    }
    function lnkDeleteRelease() {
        var id = "<%=lnkDeleteReleases.ClientID%>";
        return lnkDeleteItem(id, gvReleases, "release");
    }

    function lnkDeleteItem(id, grid, key) {
        if ($("#" + id).attr("disabled") == "disabled") {
            return false;
        }
        else {

            if (grid.GetSelectedRowCount() > 0 || grid.GetFocusedRowIndex() > -1) {
                if (confirm("Are you sure you want to delete the selected " + key + "(s)?")) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                alert("Please select " + key + "(s) to delete.")
                return false;
            }
        }
    }

    function lnkDeleteSprint() {

       if ($("#<%=lnkDeleteSprint.ClientID%>").attr("disabled") == "disabled") {
            return false;
        }
       else {

           if (gvSprints.GetSelectedRowCount() > 0) {
               if (confirm("Are you sure you want to delete the selected task(s)?")) {
                   return true;
               }
               else {
                   return false;
               }
           }
           else {
               alert("Please select task(s) to delete.")
               return false;
           }

            //if (gvSprints.GetSelectedRowCount() > 0) {

            //    gvSprints.GetSelectedFieldValues('Title', function (value) {
            //        if (value.length > 0) {
            //            selectedSprint = value[0];
            //        }
            //    });
            //}
            //else if (gvSprints.GetFocusedRowIndex() > -1) {
            //    gvSprints.GetRowValues(gvSprints.GetFocusedRowIndex(), 'Title', function (value) {
            //        selectedSprint = value;
            //    });
            //}

            //if (selectedSprint.trim() != "") {
            //    if (confirm("Are you sure you want to delete " + selectedSprint + " ?")) {
            //        return true;
            //    }
            //    else {
            //        return false;
            //    }
            //}
            //else {
            //    alert("Please select Sprint to delete.")
            //    return false;
            //}
        }
    }

    function SaveDuration(s, e) {
        lnkSaveDuration.DoClick();
    }

    function HideDurationPopUp() {
        ClientPopUpSprintDuration.Hide();
    }

    $(document).ready(function () {


        if ($.cookie("SelectedVisibleIndexSprintId") != undefined && $.cookie("SelectedVisibleIndexSprintId") != null) {
            gvSprints.SetFocusedRowIndex(parseInt($.cookie("SelectedVisibleIndexSprintId")));
        }
        else {
            if (gvSprints.GetVisibleRowsOnPage() > 1) {
                gvSprints.SetFocusedRowIndex(0);
            }
        }


        if (document.URL.indexOf("viewType=1") > 0) {
            $("#divShowProductBacklog").hide();
            //var framHeight = $(window).height() - 20;
            //$('#hdnFrameHeight').val(framHeight);

        }
        else {
            $(".splitterPanel").css("min-height", "375px");
            $("#divShowProductBacklog").css("min-height", "391px");
            $("#divShowProductBacklog").height((parseFloat($(".splitterPanel").parent().parent().height())));


            if (document.URL.indexOf("projectTaskView=1") > 0) {
                contenSplitter.GetPaneByName("ProductBackLog").Expand();
                $("#divShowProductBacklog").hide();
            }

            if (gvProductBacklog.GetSelectedRowCount() > 0) {
                $("#btMoveTasks").removeAttr('disabled');
                $("#btMoveTasks").removeClass("disableButton");
            }
            else {
                $("#btMoveTasks").attr('disabled', 'disabled');
                $("#btMoveTasks").addClass("disableButton");
            }
            if (gvSprintTasks.GetSelectedRowCount() > 0) {
                $("#<%=lnkMoveToBackLog.ClientID%>").removeAttr('disabled');
                $("#<%=lnkMoveToBackLog.ClientID%>").removeClass("disableButton");
            }
            else {
                $("#<%=lnkMoveToBackLog.ClientID%>").attr('disabled', 'disabled');
                $("#<%=lnkMoveToBackLog.ClientID%>").addClass("disableButton");
            }
            if (gvSprints.GetSelectedRowCount() > 0) {
                gvSprints.GetSelectedFieldValues('Title', function (value) { if (value.length > 0) { selectedSprint = value[0]; } });
            }
            else if (gvSprints.GetFocusedRowIndex() > -1) {
                gvSprints.GetRowValues(gvSprints.GetFocusedRowIndex(), 'Title', function (value) { selectedSprint = value; });
            }
            $(".dtStartDate").bind("blur", function () {
                var startDate = $(".dtStartDate").val();
                var noOfWorkingDays = "<%=noOfWorkingDays%>"

                //alert('' + '"startDateRaw":"' + lastStartDate + '","endDateRaw":"' + endDate + '","newStartDateRaw":"' + startDate + '"');
                var paramsInJson = '{' + '"startDate":"' + startDate + '","noOfWorkingDays":"' + noOfWorkingDays + '"}';
                $.ajax({
                    type: "POST",
                    url: "<%=ajaxHelperURL %>/GetEndDateByWorkingDays",
                data: paramsInJson,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    var resultJson = $.parseJSON(message.d);
                    if (resultJson.messagecode == 2) {
                        if ($(".dtStartDate").length > 0) { $(".dtStartDate").val(resultJson.startdate); }
                        if ($(".dtEndDate").length > 0) { $(".dtEndDate").val(resultJson.enddate); }
                        if ($("#<%=lblSprintDuration.ClientID%>").length > 0) { $("#<%=lblSprintDuration.ClientID%>").text(resultJson.duration); }
                    }
                    else {

                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });

            });


            $(".dtEndDate").bind("blur", function () {
                var startDate = $(".dtStartDate").val();
                var endDate = $(".dtEndDate").val();
                var noOfWorkingDays = "<%=noOfWorkingDays%>"

                //alert('' + '"startDateRaw":"' + lastStartDate + '","endDateRaw":"' + endDate + '","newStartDateRaw":"' + startDate + '"');
                var paramsInJson = '{' + '"startDate":"' + startDate + '","endDate":"' + endDate + '"}';
                $.ajax({
                    type: "POST",
                    url: "<%=ajaxHelperURL %>/GetDuration",
                data: paramsInJson,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    var resultJson = $.parseJSON(message.d);
                    if (resultJson.messagecode == 2) {
                        if ($("#<%=lblSprintDuration.ClientID%>").length > 0) { $("#<%=lblSprintDuration.ClientID%>").text(resultJson.duration); }
                    }
                    else {

                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });

            });
        }
    });

    $(".dateChangeRelease").blur(function () {
        $("#<%=lblReleaseDate.ClientID%>").val('');

    });

    function setSprintCookie(key, value) {
        $.cookie(key, value, { expires: 9999 });
    }

    function OnInit(s, e) {
        $('#tableproductbacklog').css("height", contenSplitter.GetPaneByName("ProductBackLog").lastHeight);
        gvProductBacklog.SetHeight($(window).height() - 50);
        $('.divinnerproductbacklog').css("height", contenSplitter.GetPaneByName("ProductBackLog").lastHeight)

    }

    function OnInitSprintTask(s, e) {
        gvSprintTasks.SetHeight($(window).height() - 70);
    }


    function adjustControlSize() {
        setTimeout(function () {
            $("#s4-workspace").width("100%");
            contenSplitter.AdjustControl();
            gvProductBacklog.AdjustControl();
            gvSprints.AdjustControl();
            gvReleases.AdjustControl();
            gvSprintTasks.AdjustControl();
        }, 10);
    }
</script>
<asp:Button ID="btnHidden" runat="server" Style="display: none;" OnClick="btnHidden_Click" />
<asp:HiddenField ID="hdnSprintId" runat="server" />
<asp:HiddenField ID="hdnSprintTitle" runat="server" />
<asp:HiddenField ID="hdnReleaseId" runat="server" />
<asp:HiddenField ID="hdnReleaseTitle" runat="server" />
<asp:HiddenField ID="hdnSource" runat="server" />
<dx:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="CallbackPanel"
    Width="100%">
    <PanelCollection>
        <dx:PanelContent runat="server">
            <div style="display: inline;">
                <div style="display: inline; float: left; border: 1px solid; border-color: #8c8c8c; border-right: 0px;" id="divShowProductBacklog">
                    <a id="lnkShowProductBackLog" style="word-wrap: break-word; text-align: center; font-weight: bold; padding: 10px 3px 0px 3px; width: 21px; cursor: pointer; display: inline-block;" onclick="ExpandProductBackLogPane();">PRODUCT &nbsp;&nbsp;BACKLOG</a>
                </div>
                <div style="display: -webkit-box;">
                    <dx:ASPxSplitter ID="contenSplitter" ResizingMode="Postponed" runat="server" Width="98%" ClientInstanceName="contenSplitter" SaveStateToCookies="true">

                        <Panes>
                            <dx:SplitterPane Name="ProductBackLog" AutoHeight="true" AutoWidth="true" Size="35%" MinSize="420px" ScrollBars="Auto" Collapsed="True" ShowCollapseBackwardButton="True">
                                <ContentCollection>
                                    <dx:SplitterContentControl runat="server">
                                        <div class="splitterPanel">

                                            <label style="font-weight: bold;">Product Backlog</label>
                                            <asp:RadioButtonList ID="rdProductBackLog" CssClass="productBackLog pmmScrum_prodBack_RadioBtnwrap" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rdProductBackLog_SelectedIndexChanged" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="All" Value="all" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Unallocated" Value="unallocated"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <div class="PmmScrum_btnContainer" style="float: right; margin-top: -8px; padding-top: 2px;">
                                                <a id="lnkAddTask" text="&nbsp;&nbsp;Add Tasks&nbsp;&nbsp;" runat="server"
                                                    tooltip="Add Task" href="javascript:void(0);">
                                                    <span class="pmmScrum_addBtn">
                                                        <b style="float: left; font-weight: normal;">Add</b>
                                                        <i
                                                            style="float: left; position: relative; top: 0px; left: 4px">
                                                            <img src="/Content/Images/newAdd.png" class="pmmScrum_addIcon" title="" alt="" /></i>
                                                    </span>
                                                </a>
                                                <asp:LinkButton runat="server" ID="lnkDeleteTask" Enabled="true" Text="&nbsp;&nbsp;Delete&nbsp;&nbsp;" OnClick="lnkDeleteTask_Click" OnClientClick="javascript:return lnkDeleteTask();" CssClass="btnDelete">

                                          <span>
                                            <b style="float: left; font-weight: normal;">Delete</b>
                                            <i
                                                style="float: left; position: relative; top:-1px; left: 4px">
                                                <img  src="/Content/Images/newDelete.png" class="pmmScrum_deleteIcon" title="" alt="" /></i>
                                        </span>
                                                </asp:LinkButton>
                                                <%--   <a id="btMoveTasks" text="&nbsp;&nbsp;Move Tasks&nbsp;&nbsp;" class="disableButton" disabled="disabled"
                                                    tooltip="MoveTasks" href="javascript:void(0);" onclick="btMoveTasksClick();">
                                                    <span class="pmmScrum_moveBtn">
                                                        <b style="float: left; font-weight: normal;">Move</b>
                                                        <i
                                                            style="float: left; position: relative; top: 0px; left: 4px">
                                                            <img src="/Content/Images/add_icon.png" style="border: none;" title="" alt="" /></i>
                                                    </span>
                                                </a>--%>
                                            </div>


                                            <table id="tableproductbacklog" style="width: 100%;">
                                                <tr>
                                                    <td style="vertical-align: top;">
                                                        <script type="text/javascript">
                                                            function UpdateGridHeight() {
                                                                try {
                                                                    gvProductBacklog.SetHeight(0);
                                                                    var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
                                                                    if (document.body.scrollHeight > containerHeight)
                                                                        containerHeight = document.body.scrollHeight;
                                                                    gvProductBacklog.SetHeight(containerHeight);
                                                                } catch (e) {

                                                                }
                                                            }
                                                            window.addEventListener('resize', function (evt) {
                                                                if (!ASPxClientUtils.androidPlatform)
                                                                    return;
                                                                var activeElement = document.activeElement;
                                                                if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                                                                    window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
                                                            });
                                                        </script>
                                                        <dx:ASPxGridView ID="gvProductBacklog" runat="server" AutoGenerateColumns="False" SettingsText-CommandClearFilter="" EnableCallBacks="true"
                                                            OnDataBinding="gvProductBacklog_DataBinding" SettingsText-EmptyDataRow="There are no items to show in this view." Styles-SelectedRow-BackColor="#d9e4fd"
                                                            ClientInstanceName="gvProductBacklog" Width="100%" KeyFieldName="Id" Style="margin-top: 14px;" Styles-Cell-HorizontalAlign="Center" Styles-Header-HorizontalAlign="Center" CssClass="pmmScrum_productBacklogGrid">
                                                            <SettingsAdaptivity AdaptivityMode="Off" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
                                                            <Columns>
                                                                <dx:GridViewDataColumn FieldName="ItemOrder" Caption="#" VisibleIndex="1" Width="15px" ShowInCustomizationForm="True">
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="ID" VisibleIndex="-1" Caption="ID" Width="15px" ShowInCustomizationForm="True" Visible="false"></dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn VisibleIndex="2" Caption="Title" ShowInCustomizationForm="True" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                    <DataItemTemplate>
                                                                        <dx:ASPxHyperLink ID="aProductBackLogTitle" runat="server" TitleItemOrder='<%#Eval("ItemOrder") %>' TaskId='<%#Eval("Id") %>' Text='<%#Eval("Title") %>' CssClass="lnkTitle">
                                                                        </dx:ASPxHyperLink>
                                                                    </DataItemTemplate>

                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                                                                    <CellStyle HorizontalAlign="Left"></CellStyle>

                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="SprintTitle" VisibleIndex="3" Caption="Sprint" ShowInCustomizationForm="True"></dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="ReleaseTitle" VisibleIndex="4" Caption="Release" ShowInCustomizationForm="True"></dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="EstimatedHours" VisibleIndex="5" Caption="Est Hrs" Width="30px" ShowInCustomizationForm="True"></dx:GridViewDataColumn>
                                                            </Columns>
                                                            <SettingsCommandButton>
                                                                <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="nprPlanningGrid_btn"></ShowAdaptiveDetailButton>
                                                                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="nprPlanningGrid_btn"></HideAdaptiveDetailButton>
                                                            </SettingsCommandButton>
                                                            <Settings ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
                                                            <SettingsBehavior AllowSelectByRowClick="false" />
                                                            <SettingsText EmptyDataRow="There are no items to show in this view."></SettingsText>

                                                            <SettingsPopup>
                                                                <HeaderFilter Height="200px" Width="220px" />
                                                            </SettingsPopup>
                                                            <SettingsPager Position="TopAndBottom" Mode="ShowAllRecords" AlwaysShowPager="false" Visible="false">
                                                                <%--<PageSizeItemSettings Items="5, 10, 15, 20, 25, 50, 75, 100" Visible="True" />--%>
                                                            </SettingsPager>
                                                            <Styles>

                                                                <AlternatingRow Enabled="True" CssClass="ugitlight1lightest"></AlternatingRow>
                                                                <Header Font-Bold="True" />
                                                                <Row CssClass="productBacklog_gridRow"></Row>
                                                                <SelectedRow BackColor="#D9E4FD" CssClass="sprintgrid-selectedrow"></SelectedRow>
                                                                <FocusedRow CssClass="sprintgrid-focusedrow"></FocusedRow>

                                                                <Cell HorizontalAlign="Center"></Cell>
                                                                <EmptyDataRow CssClass="sprintgrid-emptyrow"></EmptyDataRow>
                                                            </Styles>
                                                            <ClientSideEvents RowClick="OnRowClickEventTask" SelectionChanged="OnSelectionChanged" Init="OnInit" />
                                                        </dx:ASPxGridView>
                                                        <script type="text/javascript">
                                                            ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                                                                UpdateGridHeight();
                                                            });
                                                            ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                                                                UpdateGridHeight();
                                                            });
                                                        </script>
                                                    </td>
                                                    <td style="vertical-align: top; position: relative">
                                                        <div class="divinnerproductbacklog" style="float: right; cursor: pointer; width: 10px; right: -10px; position: absolute; background-image: url('/Content/Images/New-hover-arrow.png'); background-repeat: repeat-y;" onclick="HideProductbacklogPanel(this)">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>



                                            <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" ClientInstanceName="LoadingPanel" Modal="True">
                                            </dx:ASPxLoadingPanel>


                                            <div class="pickervaluevontainer" id="divSprintPicker">
                                                <asp:Panel ID="pnlSprintsPicker" runat="server">
                                                    <asp:CheckBoxList ID="chkSprints" runat="server"></asp:CheckBoxList>
                                                    <asp:LinkButton ID="btDone" Visible="true" runat="server" Text="&nbsp;&nbsp;Submit&nbsp;&nbsp;" OnClick="btDone_Click" OnClientClick="HidePopUp()"
                                                        ToolTip="Submit" Style="float: right; bottom: 0px; right: 6px; position: absolute;">
                                                        <span class="button-bg">
                                                         <b style="float: left; font-weight: normal;">
                                                         Done</b>
                                                         <i
                                                        style="float: left; position: relative; top: -2px;left:2px">
                                                        <img src="/Content/images/save.png"  style="border:none;" title="" alt=""/></i> 
                                                         </span>
                                                    </asp:LinkButton>

                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </dx:SplitterContentControl>
                                </ContentCollection>
                            </dx:SplitterPane>


                            <dx:SplitterPane Name="PaneReleaseSprint" AutoWidth="true" MinSize="250px" Size="25%" ScrollBars="Auto">
                                <Panes>
                                    <dx:SplitterPane Name="Sprints" ScrollBars="Auto" Size="60%" MinSize="250px" Collapsed="false">
                                        <ContentCollection>
                                            <dx:SplitterContentControl ID="SplitterContentControl1" runat="server">
                                                <%--  <div style="float: left; width: 100%;" class="divHorizontalPane">--%>
                                                <asp:HiddenField ID="hdnFocussedRowIndex" runat="server" />
                                                <div>
                                                    <label style="font-weight: bold;">Sprints</label>

                                                    <asp:ImageButton runat="server" ID="imgBtnDuration" Style="vertical-align: middle;" OnClick="imgBtnDuration_Click" ImageUrl="/Content/Images/newClock.png" />
                                                </div>
                                                <div class="pmmScrum_sprintBtnWrap" id="divSprintAction">
                                                  <%--  <asp:LinkButton ID="lnkAddSprint" Text="&nbsp;&nbsp;Add Tasks&nbsp;&nbsp;" runat="server" OnClick="lnkOpenSprintPopUp_Click"
                                                        ToolTip="Add Task">
                                            <span class="pmmScrum_addBtn">
                                                <b style="float: left; font-weight: normal;">Add</b>
                                                <i
                                                    style="float: left; position: relative; top: 0px; left: 4px">
                                                    <img src="/Content/Images/newAdd.png" class="pmmScrum_addIcon" title="" alt="" /></i>
                                            </span>
                                                    </asp:LinkButton>--%>

                                                   <a id="lnkAddNewSprint" text="&nbsp;&nbsp;Add Tasks&nbsp;&nbsp;" runat="server"
                                                    tooltip="Add Task" href="javascript:void(0);">
                                                    <span class="pmmScrum_addBtn">
                                                        <b style="float: left; font-weight: normal;">Add</b>
                                                        <i
                                                            style="float: left; position: relative; top: 0px; left: 4px">
                                                            <img src="/Content/Images/newAdd.png" class="pmmScrum_addIcon" title="" alt="" /></i>
                                                    </span>
                                                </a>

                                                    <asp:LinkButton runat="server" ID="lnkDeleteSprint" Enabled="true" Text="&nbsp;&nbsp;Delete Sprint&nbsp;&nbsp;" OnClick="lnkDeleteSprint_Click" OnClientClick="javascript:return lnkDeleteSprint();" CssClass="btnDelete">
                                          <span>
                                            <b style="float: left; font-weight: normal;">Delete</b>
                                            <i
                                                style="float: left; position: relative; top:-1px; left: 4px">
                                                <img  src="/Content/Images/newDelete.png" class="pmmScrum_deleteIcon" title="" alt="" /></i>
                                        </span>
                                                    </asp:LinkButton>
                                                </div>
                                                <script type="text/javascript">
                                                    function UpdateGridHeight() {
                                                        gvSprints.SetHeight(0);
                                                        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
                                                        if (document.body.scrollHeight > containerHeight)
                                                            containerHeight = document.body.scrollHeight;
                                                        gvSprints.SetHeight(containerHeight);
                                                    }
                                                    window.addEventListener('resize', function (evt) {
                                                        if (!ASPxClientUtils.androidPlatform)
                                                            return;
                                                        var activeElement = document.activeElement;
                                                        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                                                            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
                                                    });
                                                </script>

                                                <dx:ASPxGridView ID="gvSprints" CssClass="customgridview homeGrid" runat="server" AutoGenerateColumns="false" SettingsText-CommandClearFilter="" Styles-SelectedRow-BackColor="#d9e4fd" 
                                                    EnableCallBacks="true" OnDataBinding="gvSprints_DataBinding" Width="100%" ClientInstanceName="gvSprints" KeyFieldName="ID">
                                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
                                                    <Columns>
                                                        <dx:GridViewDataColumn VisibleIndex="-1" Visible="false" Caption="Title" FieldName="ID" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Left">
                                                        </dx:GridViewDataColumn>

                                                        <dx:GridViewDataColumn FieldName="ItemOrder" Width="40px" Caption="#" VisibleIndex="1" ShowInCustomizationForm="True">
                                                        </dx:GridViewDataColumn>

                                                        <dx:GridViewDataColumn VisibleIndex="2" Caption="Title" ShowInCustomizationForm="True" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <DataItemTemplate>

                                                                   <dx:ASPxHyperLink ID="aSprint" runat="server" SprintId='<%#Eval("ID") %>' SprintTitle='<%#Eval("Title") %>'  Text='<%#Eval("Title") %>' CssClass="lnkEditSprint">
                                                                </dx:ASPxHyperLink>

                                                             <%--   <dx:ASPxHyperLink ID="aSprint" runat="server" SprintId='<%#Eval("ID") %>' SprintName='<%#Eval("Title") %>'  Text='<%#Eval("Title") %>' CssClass="lnkTitleSprint">
                                                                </dx:ASPxHyperLink>--%>
                                                            </DataItemTemplate>

                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                                                            <CellStyle HorizontalAlign="Left"></CellStyle>
                                                        </dx:GridViewDataColumn>
                                                        <dx:GridViewDataDateColumn FieldName="StartDate" VisibleIndex="3" Caption="Start Date" Width="120px" PropertiesDateEdit-DisplayFormatString="MMM-dd-yyy">
                                                            <PropertiesDateEdit DisplayFormatString="MMM-dd-yyy"></PropertiesDateEdit>
                                                        </dx:GridViewDataDateColumn>
                                                        <dx:GridViewDataDateColumn FieldName="EndDate" VisibleIndex="4" Caption="End Date" Width="120px" PropertiesDateEdit-DisplayFormatString="MMM-dd-yyy">
                                                            <PropertiesDateEdit DisplayFormatString="MMM-dd-yyy"></PropertiesDateEdit>
                                                        </dx:GridViewDataDateColumn>
                                                        <dx:GridViewDataColumn FieldName="ID" VisibleIndex="-1" Caption="ID" Visible="false"></dx:GridViewDataColumn>
                                                    </Columns>
                                                    <SettingsCommandButton>
                                                        <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                                                        <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                                                    </SettingsCommandButton>

                                                    <Settings ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
                                                    <SettingsBehavior AllowSelectByRowClick="false" />

                                                    <SettingsText EmptyDataRow="There are no items to show in this view."></SettingsText>

                                                    <SettingsPopup>
                                                        <HeaderFilter Height="200px" />
                                                    </SettingsPopup>
                                                  <%--  <Styles>
                                                        <AlternatingRow Enabled="True"></AlternatingRow>
                                                        <Header Font-Bold="True" CssClass="homeGrid_headerColumn" />
                                                        <Row CssClass="homeGrid_dataRow"></Row>
                                                    </Styles>
                                                    <ClientSideEvents SelectionChanged="function(s, e) {
                                                            if (e.isSelected) {
                                                              var key = s.GetRowKey(e.visibleIndex);
                                                                setSprintCookie('SelectedVisibleIndexSprintId', e.visibleIndex);
                                                                    LoadingPanel.SetText('Loading....');
                                                                    LoadingPanel.Show();      
                                                                javascript:__doPostBack(); 
                                
                                                            }                             
                                                        }" />--%>
                                                      <Styles>

                                                                <AlternatingRow Enabled="True" CssClass="ugitlight1lightest"></AlternatingRow>
                                                                <Header Font-Bold="True" />
                                                                <Row CssClass="productBacklog_gridRow"></Row>
                                                                <SelectedRow BackColor="#D9E4FD" CssClass="sprintgrid-selectedrow"></SelectedRow>
                                                                <FocusedRow CssClass="sprintgrid-focusedrow"></FocusedRow>

                                                                <Cell HorizontalAlign="Center"></Cell>
                                                                <EmptyDataRow CssClass="sprintgrid-emptyrow"></EmptyDataRow>
                                                            </Styles>
                                                            <ClientSideEvents RowClick="OnRowClickEventTask" SelectionChanged="OnSelectionChanged" Init="OnInit" />

                                                </dx:ASPxGridView>
                                                <script type="text/javascript">
                                                    ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                                                        UpdateGridHeight();
                                                    });
                                                    ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                                                        UpdateGridHeight();
                                                    });
                                                </script>

                                            </dx:SplitterContentControl>

                                        </ContentCollection>

                                    </dx:SplitterPane>
                                    <dx:SplitterPane Name="Releases" Size="40%" ScrollBars="Auto" MinSize="150px" Collapsed="false">
                                        <ContentCollection>
                                            <dx:SplitterContentControl ID="SplitterContentControl2" runat="server">
                                                <%--    <div style="float: left; width: 100%; " class="divHorizontalPane">--%>
                                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                                <div>
                                                    <label style="font-weight: bold;">Releases</label>
                                                </div>
                                                <div class="pmmScrum_releasesBtnWrap" id="divReleaseActions">
                                                  <%--  <asp:LinkButton ID="lnkAddReleases" Text="&nbsp;&nbsp;Add Release&nbsp;&nbsp;" runat="server" OnClick="lnkOpenReleasePopUp_Click"
                                                        ToolTip="Add Task">
                                                        <span class="pmmScrum_addBtn">
                                                            <b style="float: left; font-weight: normal;">Add</b>
                                                            <i
                                                                style="float: left; position: relative; top: 0px; left: 4px">
                                                                <img src="/Content/Images/newAdd.png" class="pmmScrum_addIcon" title="" alt="" /></i>
                                                        </span>
                                                    </asp:LinkButton>--%>
                                                     <a id="lnkAddNewRelease" text="&nbsp;&nbsp;Add Tasks&nbsp;&nbsp;" runat="server"
                                                    tooltip="Add Task" href="javascript:void(0);">
                                                    <span class="pmmScrum_addBtn">
                                                        <b style="float: left; font-weight: normal;">Add</b>
                                                        <i
                                                            style="float: left; position: relative; top: 0px; left: 4px">
                                                            <img src="/Content/Images/newAdd.png" class="pmmScrum_addIcon" title="" alt="" /></i>
                                                    </span>
                                                </a>
 
                                                    <asp:LinkButton runat="server" ID="lnkDeleteReleases" Enabled="true" Text="&nbsp;&nbsp;Delete Release&nbsp;&nbsp;" OnClick="lnkDeleteReleases_Click" OnClientClick="javascript:return lnkDeleteRelease();" CssClass="pmmScrum_deleteBtn">
                                          <span>
                                            <b style="float: left; font-weight: normal;">Delete</b>
                                            <i
                                                style="float: left; position: relative; top:-1px; left: 4px">
                                                <img  src="/Content/Images/newDelete.png" class="pmmScrum_deleteIcon" title="" alt="" /></i>
                                        </span>
                                                    </asp:LinkButton>
                                                </div>
                                                <script type="text/javascript">
                                                    function UpdateGridHeight() {
                                                        try {
                                                            gvReleases.SetHeight(0);
                                                            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
                                                            if (document.body.scrollHeight > containerHeight)
                                                                containerHeight = document.body.scrollHeight;
                                                            gvReleases.SetHeight(containerHeight);
                                                        } catch (e) {

                                                        }
                                                    }
                                                    window.addEventListener('resize', function (evt) {
                                                        if (!ASPxClientUtils.androidPlatform)
                                                            return;
                                                        var activeElement = document.activeElement;
                                                        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                                                            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
                                                    });
                                                </script>
                                                <dx:ASPxGridView ID="gvReleases" CssClass="customgridview homeGrid" runat="server" AutoGenerateColumns="false" EnableCallBacks="true" Styles-SelectedRow-BackColor="#d9e4fd"
                                                    OnDataBinding="gvReleases_DataBinding" ClientInstanceName="gvReleases" KeyFieldName="ID" Width="100%">
                                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
                                                    <Columns>
                                                        <dx:GridViewDataDateColumn FieldName="ReleaseID" VisibleIndex="1" Caption="Release ID" Width="40px"></dx:GridViewDataDateColumn>
                                                        <dx:GridViewDataColumn Caption="Title" ShowInCustomizationForm="True" VisibleIndex="2">
                                                            <DataItemTemplate>
                                                               <%-- <dx:ASPxHyperLink ID="aRelease" runat="server" ReleaseId='<%#Eval("ID") %>' Text='<%#Eval("Title") %>' CssClass="lnkTitleRelease">
                                                                </dx:ASPxHyperLink>--%>
                                                                 <dx:ASPxHyperLink ID="aRelease" runat="server" ReleaseId='<%#Eval("ID") %>' Text='<%#Eval("Title") %>' CssClass="lnkEditRelease">
                                                                </dx:ASPxHyperLink>
                                                            </DataItemTemplate>

                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                                                            <CellStyle HorizontalAlign="Left"></CellStyle>
                                                        </dx:GridViewDataColumn>
                                                        <dx:GridViewDataDateColumn FieldName="ReleaseDate" VisibleIndex="3" Caption="Date" Width="120px" PropertiesDateEdit-DisplayFormatString="MMM-dd-yyy">
                                                            <PropertiesDateEdit DisplayFormatString="MMM-dd-yyy"></PropertiesDateEdit>
                                                        </dx:GridViewDataDateColumn>

                                                        <dx:GridViewDataColumn FieldName="ItemOrder" Caption="#" VisibleIndex="-1" Visible="false" ShowInCustomizationForm="True">
                                                        </dx:GridViewDataColumn>
                                                        <dx:GridViewDataColumn FieldName="ID" Caption="ID" Visible="false"></dx:GridViewDataColumn>
                                                    </Columns>
                                                    <SettingsCommandButton>
                                                        <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                                                        <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                                                    </SettingsCommandButton>
                                                    <Settings ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
                                                    <%--<SettingsBehavior AllowFocusedRow="True" AllowSort="true" AllowSelectByRowClick="true" />--%>
                                                    <SettingsBehavior AllowSelectByRowClick="false" />

                                                    <SettingsText EmptyDataRow="There are no items to show in this view."></SettingsText>

                                                    <SettingsPopup>
                                                        <HeaderFilter Height="200px" Width="220px" />
                                                    </SettingsPopup>
                                           <%--    <Styles>
                                                       
                                                        <Header Font-Bold="True" CssClass="homeGrid_headerColumn" />
                                                        <FocusedRow CssClass="sprintgrid-focusedrow"></FocusedRow>
                                                        <SelectedRow BackColor="#D9E4FD" CssClass="sprintgrid-selectedrow"></SelectedRow>
                                                        <Row CssClass="homeGrid_dataRow"></Row>
                                                        <EmptyDataRow CssClass="sprintgrid-emptyrow"></EmptyDataRow>
                                                        <Cell HorizontalAlign="Center"></Cell>
                                                    </Styles>--%>
                                                 <%--   <ClientSideEvents SelectionChanged="function(s, e) {
                            if (e.isSelected) {
                                var key = s.GetRowKey(e.visibleIndex);
                                
                                    LoadingPanel.SetText('Loading....');
                                    LoadingPanel.Show();      
                                javascript:__doPostBack();  
                                
                            }                             
                        }" />--%>
                                                      <Styles>

                                                                <AlternatingRow Enabled="True" CssClass="ugitlight1lightest"></AlternatingRow>
                                                                <Header Font-Bold="True" />
                                                                <Row CssClass="productBacklog_gridRow"></Row>
                                                                <SelectedRow BackColor="#D9E4FD" CssClass="sprintgrid-selectedrow"></SelectedRow>
                                                                <FocusedRow CssClass="sprintgrid-focusedrow"></FocusedRow>

                                                                <Cell HorizontalAlign="Center"></Cell>
                                                                <EmptyDataRow CssClass="sprintgrid-emptyrow"></EmptyDataRow>
                                                            </Styles>
                                     <ClientSideEvents RowClick="OnRowClickEventTask" SelectionChanged="OnSelectionChanged" Init="OnInit" />
                                 </dx:ASPxGridView>
                                                <script type="text/javascript">
                                                    ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                                                        UpdateGridHeight();
                                                    });
                                                    ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                                                        UpdateGridHeight();
                                                    });
                                                </script>
                                                <%--    </div>--%>
                                            </dx:SplitterContentControl>
                                        </ContentCollection>
                                    </dx:SplitterPane>
                                </Panes>
                                <ContentCollection>
                                    <dx:SplitterContentControl runat="server"></dx:SplitterContentControl>
                                </ContentCollection>
                            </dx:SplitterPane>

                            <dx:SplitterPane Name="SprintTasks" AutoHeight="true" AutoWidth="true" Size="40%"
                                MinSize="450px" ScrollBars="Auto" AllowResize="True">
                                <ContentCollection>
                                    <dx:SplitterContentControl runat="server">
                                        <div class="splitterPanel">
                                            <div style="display: block; text-align: center;" class="setTextWarp">

                                                <div style="float: right; margin-top: -3px;" id="divActionSprintTask">

                                                    <span class="fleft" style="padding-right: 4px; padding-top: 4px; float: left;">
                                                        <img src="/Content/images/analytics.png" id="sprintChart" runat="server" style="border: none; width: 16px;" alt="" title="Sprint Burndown Chart" />
                                                    </span>

                                                    <div style="float: right">
                                                        <asp:LinkButton EnableViewState="false" runat="server" ID="lnkMoveToBackLog" Text="&nbsp;&nbsp;Remove Tasks&nbsp;&nbsp;" CssClass="disableButton" disabled="true"
                                                            ToolTip="Move to Product BackLog" OnClientClick="javascript:return btMoveTasksToBackLogClick();" OnClick="lnkMoveToBackLog_Click">
                                        <span class="pmmScrum_deleteBtn" style="padding-bottom:3px;">
                                            <b style="float: left; font-weight: normal;">Remove</b>
                                            <i style="float: left; position: relative; top: -3px; left: 4px">
                                                <img src="/Content/Images/newDelete.png" class="pmmScrum_deleteIcon" title="" alt="" /></i>
                                        </span>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>




                                                <div style="padding-bottom: 13px; float: left" id="divSprintTask" runat="server">
                                                    <asp:Label ID="lblSprintTask" runat="server" Style="font-weight: bold;"></asp:Label>
                                                    <%--<asp:Label ID="lblSprintStartDate" runat="server" Style="font-weight: bold;"></asp:Label>
                                                    <b>to</b>
                                                    <asp:Label ID="lblSprintEndDate" runat="server" Style="font-weight: bold;"></asp:Label>--%>
                                                </div>

                                                <div style="display: inline-block; text-align: center; vertical-align: top;">
                                                    <asp:Label ID="lblTotalHours" runat="server" Style="font-weight: bold;"></asp:Label>
                                                </div>

                                            </div>

                                            <br />
                                            <script type="text/javascript">
                                                function UpdateGridHeight() {
                                                    try {
                                                        gvSprintTasks.SetHeight(0);
                                                        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
                                                        if (document.body.scrollHeight > containerHeight)
                                                            containerHeight = document.body.scrollHeight;
                                                        gvSprintTasks.SetHeight(containerHeight);
                                                    } catch (e) {

                                                    }
                                                }
                                                window.addEventListener('resize', function (evt) {
                                                    if (!ASPxClientUtils.androidPlatform)
                                                        return;
                                                    var activeElement = document.activeElement;
                                                    if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                                                        window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
                                                });
                                            </script>
                                            <dx:ASPxGridView ID="gvSprintTasks" runat="server" AutoGenerateColumns="False" SettingsText-CommandClearFilter="" EnableCallBacks="true"
                                                OnDataBinding="gvSprintTasks_DataBinding" CssClass="customgridview homeGrid"
                                                ClientInstanceName="gvSprintTasks" OnHeaderFilterFillItems="gvSprintTasks_HeaderFilterFillItems" KeyFieldName="Id" Width="100%">
                                                <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
                                                <Columns>
                                                    <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="false">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataColumn FieldName="SprintOrder" Visible="false" VisibleIndex="-1" ShowInCustomizationForm="True"></dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="ItemOrder" VisibleIndex="1" Caption="#" ShowInCustomizationForm="True"></dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="ID" VisibleIndex="-1" Caption="ID" ShowInCustomizationForm="True" Visible="false"></dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn VisibleIndex="2" Caption="Title" ShowInCustomizationForm="True" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                        <DataItemTemplate>
                                                            <dx:ASPxHyperLink ID="aSprintTaskTitle" runat="server" TitleItemOrder='<%#Eval("ItemOrder") %>' TaskId='<%#Eval("ID") %>' Text='<%#Eval("Title") %>' CssClass="lnkTitle">
                                                            </dx:ASPxHyperLink>
                                                        </DataItemTemplate>

                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                                                        <CellStyle HorizontalAlign="Left"></CellStyle>
                                                    </dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="PercentComplete" VisibleIndex="3" Caption="% Comp" ShowInCustomizationForm="True"></dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="Status" VisibleIndex="4" Caption="Status" ShowInCustomizationForm="True"></dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="AssignedToUser" VisibleIndex="5" Caption="Assigned To" ShowInCustomizationForm="True"></dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="TaskEstimatedHours" VisibleIndex="-1" Caption="Est Hrs" Visible="false" CellStyle-HorizontalAlign="Left" ShowInCustomizationForm="True">
                                                        <CellStyle HorizontalAlign="Left"></CellStyle>
                                                    </dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="RemainingHours" VisibleIndex="6" Caption="Rem Hrs" ShowInCustomizationForm="True"></dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="SprintId" VisibleIndex="-1" Caption="SprintId" Visible="false"></dx:GridViewDataColumn>
                                                </Columns>
                                                <SettingsCommandButton>
                                                    <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                                                    <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                                                </SettingsCommandButton>
                                                <Settings ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowHeaderFilterBlankItems="true" GroupFormat="{1}" />
                                                <SettingsBehavior AllowSelectByRowClick="true" />
                                                <SettingsText EmptyDataRow="There are no items to show in this view."></SettingsText>

                                                <SettingsPopup>
                                                    <HeaderFilter Height="200px" Width="220px" />
                                                </SettingsPopup>
                                                <SettingsPager Position="TopAndBottom" Mode="ShowAllRecords" AlwaysShowPager="false" Visible="false">
                                                    <%--<PageSizeItemSettings Items="5, 10, 15, 20, 25, 50, 75, 100" Visible="True" />--%>
                                                </SettingsPager>
                                                <Styles>
                                                    <AlternatingRow Enabled="True" CssClass="ugitlight1lightest"></AlternatingRow>
                                                    <Header Font-Bold="True" CssClass="homeGrid_headerColumn" />
                                                    <FocusedRow CssClass="sprintgrid-focusedrow"></FocusedRow>
                                                    <SelectedRow BackColor="#D9E4FD" CssClass="sprintgrid-selectedrow"></SelectedRow>
                                                    <Table CssClass="droppableLeft"></Table>
                                                    <Row CssClass="homeGrid_dataRow"></Row>
                                                    <Cell HorizontalAlign="Center"></Cell>
                                                    <EmptyDataRow CssClass="sprintgrid-emptyrow"></EmptyDataRow>
                                                </Styles>
                                                <ClientSideEvents SelectionChanged="OnSelectionChangedSprintTasks" RowClick="OnRowClickEventTask" Init="OnInitSprintTask" />
                                            </dx:ASPxGridView>
                                            <script type="text/javascript">
                                                ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                                                    UpdateGridHeight();
                                                });
                                                ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                                                    UpdateGridHeight();
                                                });
                                            </script>
                                            <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="True">
                                            </dx:ASPxLoadingPanel>
                                            <%--<div style="vertical-align:bottom;padding:5px 5px 5px 5px;">
                                            <asp:Label runat="server" Text="test"></asp:Label>
                                                </div>--%>
                                        </div>
                                    </dx:SplitterContentControl>
                                </ContentCollection>
                            </dx:SplitterPane>
                        </Panes>

                        <ClientSideEvents PaneCollapsed="function(s,e){OnPaneCollapsed(s,e);}" PaneExpanded="function(s,e){OnPaneExpanded(s,e);}" PaneExpanding="function(s,e){OnPaneExpanding(s,e);}" PaneResizeCompleted="function(s,e){OnPaneResize(s,e);}" PaneResized="function(s,e){OnPaneResized(s,e);}" />

                    </dx:ASPxSplitter>

                    <dx:ASPxPopupControl ID="PopupControl" runat="server" CloseAction="CloseButton" Modal="true" CssClass="aspxPopup"
                        SettingsAdaptivity-Mode="Always" PopupElementID="" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter"
                        HeaderStyle-Font-Bold="true" Width="650px" Height="300px" HeaderText="Add New Sprint" ClientInstanceName="ClientPopupControl"
                        EnableViewState="false">
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl" runat="server">
                                <div id="divSprintPoup" class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
                                    <div class="ms-formtable accomp-popup">
                                        <div class="row">
                                            <div class="ms-formlabel">
                                                <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                                            </div>
                                            <div class="ms-formbody accomp_inputField">
                                                <asp:TextBox ID="txtTitle" runat="server" ValidationGroup="Task" onchange="txtTitleChange()"></asp:TextBox>
                                                <asp:Label ID="lblTitleError" runat="server" Style="display: none;" ForeColor="Red"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rfvText" runat="server" ValidationGroup="Task" ControlToValidate="txtTitle"
                                                    Display="Dynamic" ErrorMessage="Please enter title." ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="colXs col-md-6 noLeftPadding">
                                                <div class="ms-formlabel">
                                                    <h3 class="ms-standardheader budget_fieldLabel">Start Date<b style="color: Red;">*</b>
                                                    </h3>
                                                </div>
                                                <div class="ms-formbody accomp_inputField">
                                                    <%--<SharePoint:DateTimeControl DateOnly="true" CssClassTextBox="dateControl dtStartDate"
                                                    ID="dtcStartDate" runat="server" SelectedDate="10/10/2014 12:35:17"></SharePoint:DateTimeControl>--%>
                                                    <dx:ASPxDateEdit ID="dtcStartDate" ClientInstanceName="dtcStartDate" EditFormat="Date"
                                                        CssClass="CRMDueDate_inputField" TextBox="dateControl dtStartDate" runat="server"
                                                        AutoPostBack="false" DropDownButton-Image-Width="18"
                                                        DropDownButton-Image-Url="~/Content/Images/calendarNew.png">
                                                    </dx:ASPxDateEdit>
                                                    <asp:Label ID="lbStartDate" runat="server" Style="display: none;" ForeColor="Red"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-6 noRightPadding">
                                                <div class="ms-formlabel">
                                                    <h3 class="ms-standardheader budget_fieldLabel">End Date<b style="color: Red;">*</b>
                                                    </h3>
                                                </div>
                                                <div class="ms-formbody accomp_inputField">
                                                    <%--<SharePoint:DateTimeControl DateOnly="true"
                                                    ID="dtcEndDate" runat="server" CssClassTextBox="dateControl dtEndDate" SelectedDate="10/10/2014 12:35:17"></SharePoint:DateTimeControl>--%>
                                                    <dx:ASPxDateEdit ID="dtcEndDate" CssClassTextBox="dateControl dtEndDate" runat="server"
                                                        EditFormatString="MM/dd/yyyy" NullText="MM/dd/yyyy" AutoPostBack="false"
                                                        CssClass="CRMDueDate_inputField" DropDownButton-Image-Width="18"
                                                        DropDownButton-Image-Url="~/Content/Images/calendarNew.png">
                                                    </dx:ASPxDateEdit>
                                                    <asp:Label ID="dtcDateError" runat="server" Style="display: none;" ForeColor="Red"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row SprintPdding">
                                            <div class="ms-formlabel">
                                                <h3 class="ms-standardheader budget_fieldLabel">Duration<b style="color: Red;">*</b></h3>
                                            </div>
                                            <div class="ms-formbody accomp_inputField">
                                                <asp:Label ID="lblSprintDuration" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row addEditPopup-btnWrap">
                                            <dx:ASPxButton runat="server" ID="lnkDeleteSprintPopUp" Visible="false" Text="Delete Tasks"
                                                OnClick="lnkDeleteTask_Click" CssClass="secondary-cancelBtn">
                                                <ClientSideEvents Click="return lnkDeleteTask();" />
                                            </dx:ASPxButton>
                                            <dx:ASPxButton CssClass="primary-blueBtn" AutoPostBack="true" ImagePosition="Right" ValidationGroup="Task"
                                                OnClick="lnkAddSprint_Click" ID="lnkSaveSprint" runat="server" Text="Save">
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                    </dx:ASPxPopupControl>


                    <dx:ASPxPopupControl ID="popupSprintDuration" runat="server" CloseAction="CloseButton" Modal="true" SettingsAdaptivity-Mode="Always"
                        PopupElementID="" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" HeaderStyle-Font-Bold="true"
                        Width="400px" Height="150px" HeaderText="Sprint Duration" ClientInstanceName="ClientPopUpSprintDuration" EnableViewState="false"
                        CssClass="aspxPopup">
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="popUpContentSprintDuration" runat="server">
                                <div id="divDurationPopup" class="col-md-12 col-sm-12 col-xs-12 noPadding configVariable-popupWrap">
                                    <div class="ms-formtable accomp-popup ">
                                        <div class="row">
                                            <div class="ms-formlabel">
                                                <h3 class="ms-standardheader budget_fieldLabel">Default Sprint Duration</h3>
                                            </div>
                                            <div class="ms-formbody accomp_inputField">
                                                <asp:TextBox ID="txtduration" runat="server" CssClass="marginBT"></asp:TextBox>
                                                <asp:DropDownList ID="ddlDuration" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                                    <asp:ListItem Text="Days" Value="days"></asp:ListItem>
                                                    <asp:ListItem Text="Weeks" Value="weeks"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="row fieldWrap addEditPopup-btnWrap">
                                            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" ImagePosition="Right"
                                                CssClass="secondary-cancelBtn">
                                                <ClientSideEvents Click="HideDurationPopUp" />
                                            </dx:ASPxButton>
                                            <dx:ASPxButton CssClass="primary-blueBtn" AutoPostBack="true" ImagePosition="Right" ClientInstanceName="lnkSaveDuration"
                                                OnClick="lnkSaveDuration_Click1" ID="lnkSaveDuration" runat="server" Text="Save">
                                                <ClientSideEvents Click="SaveDuration" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                    </dx:ASPxPopupControl>

                    <dx:ASPxPopupControl ID="popUpControlRelease" runat="server" CloseAction="CloseButton" Modal="true" CssClass="aspxPopup"
                        PopupElementID="" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" HeaderStyle-Font-Bold="true"
                        Width="550px" SettingsAdaptivity-Mode="Always" HeaderText="Add New Release" ClientInstanceName="ClientPopupControlRelease"
                        EnableViewState="false">
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControlRelease" runat="server">
                                <div id="div2" class="col-md-12 col-sm-12 col-xs-12 noPadding configVariable-popupWrap popupheight">
                                    <div class="ms-formtable accomp-popup">
                                        <div class="row">
                                            <div class="colXs noLeftPadding">
                                                <div class="ms-formlabel">
                                                    <h3 class="ms-standardheader budget_fieldLabel">Release ID<b style="color: Red;">*</b>
                                                    </h3>
                                                </div>
                                                <div class="ms-formbody accomp_inputField">
                                                    <asp:TextBox ID="txtReleaseID" runat="server" onchange="txtDataChange(this)"></asp:TextBox>
                                                    <asp:Label ID="lblReleaseID" runat="server" Style="display: none;" ForeColor="Red" errorLabel="true"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="colXs noRightPadding">
                                                <div class="ms-formlabel">
                                                    <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b>
                                                    </h3>
                                                </div>
                                                <div class="ms-formbody accomp_inputField">
                                                    <asp:TextBox ID="txtRelease" runat="server" onchange="txtDataChange(this)"></asp:TextBox>
                                                    <asp:Label ID="lblRelease" runat="server" Style="display: none;" ForeColor="Red" errorLabel="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="colXs noLeftPadding">
                                                <div class="ms-formlabel">
                                                    <h3 class="ms-standardheader budget_fieldLabel">Release Date<b style="color: Red;">*</b>
                                                    </h3>
                                                </div>
                                                <div class="ms-formbody accomp_inputField">
                                                    <%--<SharePoint:DateTimeControl DateOnly="true" CssClassTextBox="dateChangeRelease"
                                                        ID="dtcReleaseDate" runat="server" SelectedDate="10/10/2014 12:35:17"></SharePoint:DateTimeControl>--%>
                                                    <dx:ASPxDateEdit ID="dtcReleaseDate" ClientInstanceName="dtcReleaseDate" EditFormat="Date"
                                                        CssClass="CRMDueDate_inputField" CssClassTextBox="dateControl dtStartDate" runat="server"
                                                        AutoPostBack="false" DropDownButton-Image-Width="18"
                                                        DropDownButton-Image-Url="~/Content/Images/calendarNew.png">
                                                    </dx:ASPxDateEdit>
                                                    <asp:Label ID="lblReleaseDate" runat="server" Style="display: none;" ForeColor="Red" errorLabel="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row addEditPopup-btnWrap">
                                            <dx:ASPxButton runat="server" ID="lnkDeleteReleasePopup" Visible="false" Text="Delete Release"
                                                OnClick="lnkDeleteReleasePopup_Click" CssClass="secondary-cancelBtn">
                                                <ClientSideEvents Click="return lnkDeleteRelease();" />
                                            </dx:ASPxButton>
                                            <dx:ASPxButton ID="lnkSaveRelease" Visible="true" runat="server" Text="Save" OnClick="lnkSaveRelease_Click"
                                                CssClass="primary-blueBtn" ToolTip="Submit">
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                            </dx:PopupControlContentControl>

                        </ContentCollection>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                    </dx:ASPxPopupControl>
                </div>
            </div>
        </dx:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="InitalizejQuery"></ClientSideEvents>
</dx:ASPxCallbackPanel>
<dx:ASPxGlobalEvents ID="ge" runat="server">
    <ClientSideEvents ControlsInitialized="InitalizejQuery" />
</dx:ASPxGlobalEvents>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function OnPaneCollapsed(s, e) {
        if (e.pane.name == "ProductBackLog") {
            set_cookie('IsPaneExpanded', false, null, "<%= HttpContext.Current.Request.Url.PathAndQuery %>");
            $("#divShowProductBacklog").show();
            $($(".dxsplControl .dxsplPaneCollapsed")[0]).css("width", "1px")
        }
        $("#divShowProductBacklog").height((parseFloat($(".splitterPanel").parent().parent().height())));
    }
    function OnPaneExpanding(s, e) {

        if (e.pane.name == "ProductBackLog") {
            set_cookie('IsPaneExpanded', true, null, "<%= HttpContext.Current.Request.Url.PathAndQuery %>");
            $("#divShowProductBacklog").hide();
            // $(".dxsplControl tr:first td:first").css("display", "Block");
            $("#divShowProductBacklog").height((parseFloat($(".splitterPanel").parent().parent().height())));
            $("#divShowReleases").css("margin-top", ($("#divShowProductBacklog").height() - $("#divSprintAction").height() - 20));
        }
    }
    function OnPaneExpanded(s, e) {

        $("#divShowProductBacklog").height((parseFloat($(".splitterPanel").parent().parent().height())));
        if (e.pane.name == "ProductBackLog") {
            set_cookie('IsPaneExpanded', true, null, "<%= HttpContext.Current.Request.Url.PathAndQuery %>");
            $("#divShowReleases").css("margin-top", ($("#divShowProductBacklog").height() - $("#divSprintAction").height() - 20));
        }
        var sprintTaskWidth = contenSplitter.GetPaneByName("SprintTasks").offsetWidth;
        var divwidth = $("#ctl00_PlaceHolderMain_ctl00_CallbackPanel_contenSplitter_divSprintTask").width();

        if (sprintTaskWidth < (divwidth + 400)) {
            $('.setTextWarp').removeAttr('style');
            $('.setTextWarp').css('display', 'block');
        }
        else {
            $('.setTextWarp').css('text-align', 'center');
        }
    }
    function OnPaneResize(s, e) {

        var maxPanelHeight = 0;
        var productBacklogHeight = contenSplitter.GetPaneByName("ProductBackLog").GetClientHeight();
        var PaneReleaseSprintHeight = contenSplitter.GetPaneByName("PaneReleaseSprint").GetClientHeight();
        var sprintTaskHeight = contenSplitter.GetPaneByName("SprintTasks").GetClientHeight();
        maxPanelHeight = Math.max(parseFloat(productBacklogHeight), parseFloat(PaneReleaseSprintHeight), parseFloat(sprintTaskHeight));
        if (maxPanelHeight != 0) {
            $(".splitterPanel").height(maxPanelHeight - 20);
            //  $(".divHorizontalPane").height((maxPanelHeight - 20) / 2);
            $("#divShowProductBacklog").height((parseFloat($(".splitterPanel").parent().parent().height())));
        }

        //new line of code..
        var sprintTaskWidth = contenSplitter.GetPaneByName("SprintTasks").offsetWidth;
        var divwidth = $("#ctl00_PlaceHolderMain_ctl00_CallbackPanel_contenSplitter_divSprintTask").width();

        if (sprintTaskWidth < (divwidth + 400)) {
            $('.setTextWarp').removeAttr('style');
            $('.setTextWarp').css('display', 'block');
        }
        else {
            $('.setTextWarp').css('text-align', 'center');
        }
    }

    function ExpandProductBackLogPane() {

        contenSplitter.GetPaneByName("ProductBackLog").Expand();
        $("#divShowProductBacklog").hide();
        return false;
    }

    function OnPaneResized(s, e) {
        if ($("#divShowProductBacklog").css("display") == "block") {
            $($(".dxsplControl .dxsplPaneCollapsed")[0]).css("width", "1px")

        }
    }

    function HideProductbacklogPanel() {

        var pane = contenSplitter.GetPaneByName("ProductBackLog");
        var secondPane = contenSplitter.GetPaneByName("PaneReleaseSprint");
        pane.Collapse(secondPane);
        $("#divShowProductBacklog").show();
        return false;
    }

</script>
