<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CRMEstimatorView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMONE.CRMEstimatorView" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style>
    .heightImage {
        height: 13px !important;
    }

    .justify-content {
        display: flex;
        justify-content: flex-start;
        align-items: center;
    }

    .lineHorizontal__container {
        align-items: center;
        display: flex;
        height: 80px;
    }

    .groupStyle {
        color: black;
        border-radius: 10px;
        padding: 9px;
        text-align: center;
        border: 2px solid;
        width: 205px;
    }

    .lineHorizontal {
        border-top: 2px solid black;
        width: 100%;
    }

    .title-outer-class {
        border: 2px solid darkgray;
        border-radius: 10px;
        padding: 5px 10px;
        text-align: center;
    }

    @keyframes spinner {
        from {
            transform: rotate(360deg);
        }

        to {
            transform: rotate(0deg);
        }
    }

    .spinner {
        background-color: transparent;
        border: 10px inset #fff;
        border-radius: 50%;
        width: 80px;
        height: 80px;
    }

    .progress-bar {
        width: 48px;
        height: 48px;
        border-radius: 50% !important;
    }

    .circle-text {
        color: #333333;
        margin-top: 14px;
        font-weight: 500;
    }

    .circle-alignment {
        display: flex;
        justify-content: space-around;
    }

    .alloc-outer-class {
        border: 2px solid #ddd;
        text-align: center;
        margin-left: 24%;
        border-radius: 7px;
        font-weight: 500;
        width: 50px;
        height: 38px;
        line-height: 36px;
        margin-top: 4px;
        vertical-align: middle;
    }

    .expandClass {
        transform: rotate(90deg);
        background-image: none !important;
        margin-right: 5px;
        margin-left: 2px;
    }

    .dx-datagrid .dx-column-lines > td {
        border-left: 0px solid #ddd;
        border-right: 0px solid #ddd;
    }

    .dx-datagrid-borders .dx-datagrid-rowsview, .dx-datagrid-headers + .dx-datagrid-rowsview {
        border-top: 0px solid #ddd;
    }

    .dx-datagrid-borders > .dx-datagrid-rowsview {
        border-bottom: 0px solid #ddd;
    }

    .dx-datagrid-borders > .dx-datagrid-rowsview {
        border-left: 0px solid #ddd;
        border-right: 0px solid #ddd;
    }

    .dx-datagrid-headers {
        color: black;
        background-color: #ddd;
        font-weight: 500;
        -ms-touch-action: pinch-zoom;
        touch-action: pinch-zoom;
        border-bottom: 1px solid #ddd;
    }

    .dx-datagrid {
        font-family: 'Roboto', sans-serif !important;
    }

    .myClass {
        width: 18px !important;
    }

    .dx-checkbox-icon {
        width: 22px;
        height: 22px;
        border-radius: 2px;
        border: 2px solid #ddd;
        background-color: #fff;
        border-radius: 14px;
    }

    .dx-checkbox-checked .dx-checkbox-icon {
        font: 16px/1em DXIcons;
        color: white;
        background-color: lightgreen;
        text-align: center;
        border: 2px solid lightgreen;
    }

    .dx-checkbox.dx-state-hover .dx-checkbox-icon {
        border: 1px solid lightgreen;
    }

    .dx-checkbox.dx-state-focused .dx-checkbox-icon {
        border: 1px solid #ddd;
    }

    .dx-datagrid .dx-row.dx-header-row > td {
        padding: 4px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var baseUrl = ugitConfig.apiBaseUrl;
    var UserProjectsData = [];
    var userProfilePath = "<%=ApplicationContext.CurrentUser.Picture%>";
    function GetLeadEstimatorWithProjectDetails() {
        $("#loadpanel").dxLoadPanel("show");
        $.get(baseUrl + "/api/RMONE/GetEstimatorProjectDetails", function (data, status) {
            //console.log(data);
            data.ProjectDataModels.forEach(function (e) {
                if (e.Partners != null && e.Partners.length > 0) {
                    e.Partners = e.Partners.slice(0, -2);
                }
                if (e.PartnersType != null && e.PartnersType.length > 0) {
                    e.PartnersType = e.PartnersType.slice(0, -2);
                }
            });
            UserProjectsData = data.ProjectDataModels;

            if (data.UserUtilization.CPR != undefined && data.UserUtilization.CPR != null) {
                $("#CPRPct").text(data.UserUtilization.CPR + "%");
            }
            if (data.UserUtilization.OPM != undefined && data.UserUtilization.OPM != null) {
                $("#OPMPct").text(data.UserUtilization.OPM + "%");
            }
            if (data.UserUtilization.CNS != undefined && data.UserUtilization.CNS != null) {
                $("#CNSPct").text(data.UserUtilization.CNS + "%");
            }
            BindGrid();
        });
    }

    function BindGrid() {
        if (UserProjectsData.filter(x => x.ModuleName == "CPR").length) {
            $('#CPRDetailsDiv').dxDataGrid('instance').option("dataSource", UserProjectsData.filter(x => x.ModuleName == "CPR"));
            //$(".cpr-outer-box").show();
        }
        if (UserProjectsData.filter(x => x.ModuleName == "OPM").length) {
            $('#OPMDetailsDiv').dxDataGrid('instance').option("dataSource", UserProjectsData.filter(x => x.ModuleName == "OPM"));
            //$(".opm-outer-box").show();
        }
        if (UserProjectsData.filter(x => x.ModuleName == "CNS").length) {
            $('#CNSDetailsDiv').dxDataGrid('instance').option("dataSource", UserProjectsData.filter(x => x.ModuleName == "CNS"));
            //$(".cns-outer-box").show();
        }
        $("#loadpanel").dxLoadPanel("hide");
    }
    function togglElement(elem, elem1) {
        if (elem.is(":visible")) {
            elem.hide(1000);
            elem1.css("transform", "rotate(270deg)");
        }
        else {
            elem.show(1000);
            elem1.css("transform", "rotate(90deg)");
        }
    }

    function GetNextTask(data) {
        const minElement = new Date(Math.min(...data.filter(x => x.TaskStatus != null ? x.TaskStatus.toLowerCase() : "" != "completed" && !x.Deleted).map(x => new Date(x.TaskDueDate)))); //Math.min.apply(null, );
        return data.filter(x => x.TaskStatus != null ? x.TaskStatus.toLowerCase() : "" != "completed" && !x.Deleted && new Date(x.TaskDueDate).format("mm/dd/yyyy") == minElement.format("mm/dd/yyyy"))[0];
    }

    function UpdateGlobalData(ticketid, taskid, updateTo) {
        UserProjectsData.filter(x => x.TicketId == ticketid)[0].UserTasks.forEach(function (e) {
            if (updateTo == "Deleted") {
                if (e.ID == taskid) {
                    e.Deleted = true;
                }
            }
            else if (e.ID == taskid) {
                e.TaskStatus = updateTo;
            }
        });
        BindGrid();
    }

    function DeleteTask(TicketId, TaskId) {
        var postData = {};
        postData.TicketId = TicketId;
        postData.TaskId = TaskId;
        postData.mode = 'ModuleStageConstraints';
        var result = DevExpress.ui.dialog.confirm("Are you sure you want to delete task?", "");
        result.done(function (dialogResult) {
            if (dialogResult) {
                $("#loadpanel").dxLoadPanel("show");
                $.ajax({
                    url: baseUrl + "/api/module/DeleteHomePageTask",
                    method: "DELETE",
                    data: { TaskKeys: postData, TicketPublicId: TicketId },
                    success: function (data) {
                        UpdateGlobalData(TicketId, TaskId, "Deleted");
                        const popup = $("#userTaskFieldPopup").dxPopup("instance");
                        popup.hide();

                        var sourceURL = "<%= Request["source"] %>";
                        sourceURL += "**refreshDataOnly";
                        window.parent.CloseWindowCallback(1, sourceURL);
                    },
                    error: function (error) {
                        $("#loadpanel").dxLoadPanel("hide");
                    }
                });
            }
        });
    }

    function MarkTaskAsComplete(ticketid, taskid) {
        $("#loadpanel").dxLoadPanel("show");
        $.ajax({
            url: baseUrl + "/api/module/MarkTaskAsComplete",
            method: "POST",
            data: { TaskKeys: taskid, TicketPublicId: ticketid, TaskType: 'ModuleStageConstraints' },
            success: function (data) {
                UpdateGlobalData(ticketid, taskid, "Completed");
            },
            error: function (error) {
                $("#loadpanel").dxLoadPanel("hide");
            }
        });
    }

    function MarkTaskAsInProgress(ticketid, taskid) {
        $("#loadpanel").dxLoadPanel("show");
        $.ajax({
            url: baseUrl + "/api/module/MarkTaskAsInProgress",
            method: "POST",
            data: { TaskKeys: taskid, TicketPublicId: ticketid, TaskType: 'ModuleStageConstraints' },
            success: function (data) {
                UpdateGlobalData(ticketid, taskid, "In Progress");
            },
            error: function (error) {
                $("#loadpanel").dxLoadPanel("hide");
            }
        });
    }

    function OpenUserTasks(ticketId) {
        if (UserProjectsData.filter(x => x.TicketId == ticketId)[0].UserTasks.filter(y => !y.Deleted).length > 0) {
            $('#userTaskFieldPopup').dxPopup({
                visible: false,
                hideOnOutsideClick: true,
                showTitle: false,
                showCloseButton: false,
                hideOnOutsideClick: true,
                title: "",
                width: "800",
                height: "500",
                resizeEnabled: true,
                dragEnabled: true,
                position: {
                    at: 'center',
                    my: 'center',
                    offset: '0 100',
                    of: `#user${ticketId}`
                },
                contentTemplate: () => {
                    let taskData = UserProjectsData.filter(x => x.TicketId == ticketId && !x.Deleted)[0].UserTasks.filter(y => !y.Deleted);
                    const content = $("<div />");
                    content.append(
                        $('<div id="TaskDetails" />').dxDataGrid({
                            dataSource: taskData,
                            wordWrapEnabled: true,
                            height:"450",
                            columns: [
                                {
                                    dataField: 'AssignedToName',
                                    caption: 'Assigned',
                                    width: "40%",
                                    alignment: 'center',
                                    cellTemplate: function (container, options) {
                                        const profilepics = options.data.ProfilePics != null ? options.data.ProfilePics.split(',') : '';
                                        container.css('position', 'relative');
                                        var html = "";
                                        var leftpx = 3;
                                        if (profilepics.length > 0) {
                                            profilepics.forEach(function (item, index) {
                                                $(`<img src="${item}" style="width:28px; height:28px; border-radius: 50%; position:absolute; left:${leftpx}px">`).appendTo(container);
                                                leftpx += 15;
                                            });
                                        }
                                        leftpx = 46;
                                        const assignedToName = options.data.AssignedToName != null ? options.data.AssignedToName.split(',') : '';
                                        if (assignedToName.length > 0) {
                                            assignedToName.forEach(function (item, index) {
                                                if (index + 1 == assignedToName.length) {
                                                    html += `<p class='namestyle'>${item}</p>`;
                                                } else {
                                                    html += `<p class='namestyle'>${item},</p>`;
                                                }
                                            });
                                            $(`<div style="display: flex;flex-direction: column;margin-left: 45%;">${html}</div>`).appendTo(container);
                                        }
                                    },
                                    headerCellTemplate: function (header, info) {
                                        header.css('margin-left', '45%');
                                        $(`<div style="color: black;font-size:14px;font-weight:600;" title=${info.column.caption}>${info.column.caption}</div>`).appendTo(header);
                                    }
                                    //cellTemplate: function (container, options) {
                                    //    $(`<img src="${userProfilePath}" width="35" />`).appendTo(container);
                                    //}
                                },
                                {
                                    dataField: 'Title',
                                    caption: 'Title',
                                    alignment: 'center',
                                    width: "25%",
                                },
                                {
                                    dataField: 'TaskStatus',
                                    caption: 'Status',
                                    alignment: 'center',
                                    width: "20%",
                                    //cellTemplate: function (container, options) {
                                    //    $("<div id='chk'/>").dxCheckBox({
                                    //        text: '',
                                    //        hint: 'Mark a Task as complete',
                                    //        iconSize: 25,
                                    //        value: options.data.TaskStatus == "Completed" ? true : false,
                                    //        onValueChanged: function (e) {
                                    //            if (e.value) {
                                    //                MarkTaskAsComplete(options.data.TicketId, options.data.ID)
                                    //                //console.log(e + options.data.ID);
                                    //            }
                                    //            else {
                                    //                MarkTaskAsInProgress(options.data.TicketId, options.data.ID)
                                    //                //console.log(e + options.data.ID);
                                    //            }
                                    //        },
                                    //    }).appendTo(container);
                                    //}

                                },
                                {
                                    dataField: 'StartDate',
                                    caption: 'Date',
                                    dataType: 'date',
                                    alignment: 'center',
                                    width: "15%",
                                },
                                //{
                                //    dataField: 'ID',
                                //    caption: '',
                                //    alignment: 'center',
                                //    width: "8%",
                                //    cellTemplate: function (container, options) {
                                //        $(`<img style="transform: scale(1.2);display:none;" class="myClass" src="/content/images/deleteIcon-new.png" onclick="DeleteTask('${options.data.TicketId}','${options.data.ID}')" width="27px;">`).appendTo(container);
                                //    },
                                //},
                            ],
                            //onRowPrepared(e) {
                            //    e.rowElement.css({ "height": "50px" });
                            //},
                            //onRowClick(e) {
                            //    editTicketTask(e.data.ModuleNameLookup, e.data.TicketId, e.data.ModuleStep, e.data.ID, e.data.Title);
                            //},
                            onCellPrepared: function (e) {
                                //if (e.rowType == "header" || e.rowType == "data") {
                                //    e.cellElement.css("text-align", "center");
                                //}
                                //if (e.rowType == "data" && (e.column.name == 'TotalAllocations')) {
                                //    e.cellElement.addClass("circle-alignment");
                                //}
                            },
                            onCellHoverChanged: function (e) {
                                if ($(e.event.target).find(".myClass").is(":visible")) {
                                    $(e.event.target).find(".myClass").hide();
                                } else {
                                    $(e.event.target).find(".myClass").show();
                                }
                            },
                            showBorders: true,
                        })
                    )
                    return content;
                },
            });
            const popup = $("#userTaskFieldPopup").dxPopup("instance");
            popup.show();
        }
    }


    function editTicketTask(moduleName, ticketID, moduleStage, taskID, title) {
        var taskUrl = '<%=ConstraintTaskUrl%>';
        var taskParams = "";
        taskParams = "module=" + moduleName + "&ticketId=" + ticketID + "&conditionType='ModuleTaskCT'&moduleStage=" + moduleStage + "&taskID=" + taskID + "&type='ExistingConstraint'" + "&viewType=0";
        window.UgitOpenPopupDialog(taskUrl, taskParams, title, '800px', '700px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }


    $(document).ready(function () {
        $("#loadpanel").dxLoadPanel({
            message: "Loading...",
            visible: true,
            showPane: true,
            shading: true,
        });
        GetLeadEstimatorWithProjectDetails();
        const projectTitleTemplate = function (container, options) {
            $(`<div class='title-outer-class ml-2 mr-2' onclick=\"${options.data.moduleLink}\">`)
                .append(`<div style='font-weight:500;'>${options.data.ProjectName}</div>`)
                .append(`<div>${options.data.ProjectShortName != null && options.data.ProjectShortName != "" ? options.data.ProjectShortName : ""}</div>`)
                .appendTo(container);
        };
        const projectTaskTemplate = function (container, options) {
            let percentage = parseInt(options.data.TotalTasks) == 0
                ? 100 : parseInt(parseInt(options.data.CompletedTasks) / parseInt(options.data.TotalTasks) * 100);
            $(`<div id="user${options.data.TicketId}" onclick="OpenUserTasks('${options.data.TicketId}')" class='progress-bar' style='margin-left: 20px;background:radial-gradient(closest-side, white 70%, transparent 75% 100%),conic-gradient(#4fa1d6 ${percentage}%, #ddd 0)'>
            <div class='circle-text'>${options.data.CompletedTasks}/${options.data.TotalTasks}</div></div>`).appendTo(container);


            //let completedTasks = 0;
            //let totalTasks = 0;
            //if (options.data.UserTasks != null && options.data.UserTasks.length > 0) {
            //    totalTasks = options.data.UserTasks.filter(x => !x.Deleted).length;
            //    completedTasks = options.data.UserTasks.filter(x => !x.Deleted && x.TaskStatus == "Completed").length;
            //}
            //let borderColor = parseInt(totalTasks) == 0 ? "#4fa1d6" : "#4fa1d6";
            //$(`<div id="user${options.data.TicketId}" onclick="OpenUserTasks('${options.data.TicketId}')" class='alloc-outer-class' style='border-color:${borderColor};color:${borderColor}'>${completedTasks}/${totalTasks}</div>`)
            //    .appendTo(container);
        };
        const projectAllocTemplate = function (container, options) {
            let FilledAllocations = options.data.FilledAllocations;
            let TotalAllocations = options.data.TotalAllocations;
            let borderColor = "red";
            if (options.data.TotalAllocations == options.data.FilledAllocations && options.data.TotalAllocations != 0) {
                borderColor = "#4fa1d6";
            }
            //let borderColor = parseInt(TotalAllocations) == 0 ? "#4fa1d6" : "#4fa1d6";
            $(`<div onclick=\"${options.data.AllocationLink}\" class='alloc-outer-class' style='border-color:${borderColor};color:black'>${options.data.FilledAllocations}/${options.data.TotalAllocations}</div>`)
                .appendTo(container);
        }

        const requestTypeTemplate = function (container, options) {
            if (options.data.RequestType != "") {
                $(`<div title=\"${options.data.RequestTypeTitle}\">` + options.data.RequestType + `</div>`).appendTo(container);
            }
        }

        const dueDateTemplate = function (container, options) {
            if (options.data.DueDate != "") {
                var dueDate = options.data.DueDate == "0001-01-01T00:00:00" ? "" : options.data.DueDate;
                dueDate = dueDate != "" ? new Date(dueDate).toLocaleDateString('en-US') : "";
                $(`<div>` + dueDate + `</div>`).appendTo(container);
            }
        }

        const projectAllocHrsTemplate = function (container, options) {
            $(`<div>${options.data.ProjectAllocHrs}hrs</div>`)
                .appendTo(container);
        };
        const projectNextTaskTemplate = function (container, options) {
            let nextTask = GetNextTask(options.value);
            $(`<div>${nextTask != undefined && nextTask != null ? nextTask.Title : ""}</div>`)
                .appendTo(container);
        };
        const projectDateTemplate = function (container, options) {
            let nextTask = GetNextTask(options.data.UserTasks);
            var daysdifference = 0;
            if (nextTask != undefined && nextTask.TaskDueDate != '0001-01-01T00:00:00') {
                let millisecondsPerDay = 1000 * 60 * 60 * 24;
                let millisBetween = new Date(nextTask.TaskDueDate).getTime() - new Date().getTime();
                let days = millisBetween / millisecondsPerDay;
                daysdifference = Math.ceil(days);
            }
            let dueDate = nextTask != undefined && nextTask != null && nextTask.TaskDueDate != '0001-01-01T00:00:00' ? new Date(nextTask.TaskDueDate).format("MMM d, yyyy") : '';
            $(`<div>${daysdifference == 0 ? '' : daysdifference + " Days"}</div><div>${dueDate}</div>`)
                .appendTo(container);
        };
        const projectDurationTemplate = function (container, options) {
            let nextTask = GetNextTask(options.value);
            var daysdifference = 0;
            if (nextTask != undefined && nextTask.TaskDueDate != '0001-01-01T00:00:00' && nextTask.StartDate != '0001-01-01T00:00:00') {
                let millisecondsPerDay = 1000 * 60 * 60 * 24;
                let millisBetween = new Date(nextTask.TaskDueDate).getTime() - new Date(nextTask.StartDate).getTime();
                let days = millisBetween / millisecondsPerDay;
                daysdifference = Math.floor(days);
            }
            $(`<div>${daysdifference == 0 ? '-' : daysdifference + " Days"}</div>`)
                .appendTo(container);
        };
        const projectVolumeTemplate = function (container, options) {
            $(`<div>$${options.data.Volume}</div>`)
                .appendTo(container);
        };
        const projectTypeTemplate = function (container, options) {
            $(`<div>${options.data.Type != null && options.data.Type.length > 0 ? options.data.Type.replaceAll(";#", ", ") : ''}</div>`)
                .appendTo(container);
        };
        $('#OPMDetailsDiv').dxDataGrid({
            dataSource: UserProjectsData,
            wordWrapEnabled: true,
            columns: [
                {
                    dataField: 'ProjectName',
                    caption: 'Project Title',
                    width: "24%",
                    cellTemplate: projectTitleTemplate,
                },
                {
                    dataField: 'ClientName',
                    caption: 'Client',
                    width: "8%",
                },
                {
                    dataField: 'TotalAllocations',
                    caption: 'Project Team',
                    width: "6%",
                    cellTemplate: projectAllocTemplate,
                },
                {
                    dataField: 'RequestType',
                    caption: 'Opportunity Type',
                    width: "8%",
                    cellTemplate: requestTypeTemplate,
                },
                {
                    dataField: 'DueDate',
                    caption: 'Due Date',
                    width: "6%",
                    cellTemplate: dueDateTemplate,
                },
                {
                    dataField: 'ChanceOfSuccess',
                    caption: '% Chance',
                    width: "6%",
                },
                {
                    dataField: 'TotalTasks',
                    caption: 'Tasks',
                    width: "6%",
                    cellTemplate: projectTaskTemplate,
                },
                {
                    dataField: 'UserTasks',
                    caption: 'Next Task',
                    alignment: 'center',
                    cellTemplate: projectNextTaskTemplate,
                },
                {
                    dataField: 'ERPJobId',
                    caption: 'Job #',
                },
                //{
                //    dataField: 'Type',
                //    caption: 'Type',
                //    cellTemplate: projectTypeTemplate,
                //},
                {
                    dataField: 'Volume',
                    caption: 'Volume',
                    cellTemplate: projectVolumeTemplate,
                },
                
                'Partners',
                {
                    dataField: 'PartnersType',
                    caption: 'Subs',
                },
                
                //{
                //    dataField: 'DueDate',
                //    caption: 'Due',
                //    cellTemplate: projectDateTemplate,
                //},
            ],
            onCellPrepared: function (e) {
                if (e.rowType == "header" || e.rowType == "data") {
                    e.cellElement.css("text-align", "center");
                }
                if (e.rowType == "data" && (e.column.name == 'TotalAllocations')) {
                    e.cellElement.addClass("circle-alignment");
                }
            },
            showBorders: true,
        });
        $('#CPRDetailsDiv').dxDataGrid({
            dataSource: UserProjectsData,
            wordWrapEnabled: true,
            columns: [
                {
                    dataField: 'ProjectName',
                    caption: 'Project Title',
                    width: "24%",
                    cellTemplate: projectTitleTemplate,
                },
                {
                    dataField: 'ClientName',
                    caption: 'Client',
                    width: "8%",
                },
                {
                    dataField: 'TotalAllocations',
                    caption: 'Project Team',
                    width: "6%",
                    cellTemplate: projectAllocTemplate,
                },
                {
                    dataField: 'DueDate',
                    caption: 'Due Date',
                    width: "6%",
                    cellTemplate: dueDateTemplate,
                },
                {
                    dataField: 'TotalTasks',
                    caption: 'Tasks',
                    width: "6%",
                    cellTemplate: projectTaskTemplate,
                },
                {
                    dataField: 'UserTasks',
                    caption: 'Next Task',
                    alignment: 'center',
                    cellTemplate: projectNextTaskTemplate,
                },
                {
                    dataField: 'ERPJobId',
                    caption: 'Job #',
                },
                //{
                //    dataField: 'Type',
                //    caption: 'Type',
                //    cellTemplate: projectTypeTemplate,
                //},
                {
                    dataField: 'Volume',
                    caption: 'Volume',
                    cellTemplate: projectVolumeTemplate,
                },
                
                'Partners',
                {
                    dataField: 'PartnersType',
                    caption: 'Subs',
                },
                
                //{
                //    dataField: 'DueDate',
                //    caption: 'Due',
                //    cellTemplate: projectDateTemplate,
                //},
            ],
            onCellPrepared: function (e) {
                if (e.rowType == "header" || e.rowType == "data") {
                    e.cellElement.css("text-align", "center");
                }
                if (e.rowType == "data" && (e.column.name == 'TotalAllocations')) {
                    e.cellElement.addClass("circle-alignment");
                }
            },
            showBorders: true,
        });
        $('#CNSDetailsDiv').dxDataGrid({
            dataSource: UserProjectsData,
            wordWrapEnabled: true,
            columns: [
                {
                    dataField: 'ProjectName',
                    caption: 'Project Title',
                    width: "24%",
                    cellTemplate: projectTitleTemplate,
                },
                {
                    dataField: 'TotalAllocations',
                    caption: 'Project Team',
                    width: "6%",
                    cellTemplate: projectAllocTemplate,
                },
                {
                    dataField: 'TotalTasks',
                    caption: 'Task',
                    width: "6%",
                    cellTemplate: projectTaskTemplate,
                },
                {
                    dataField: 'Type',
                    caption: 'Type',
                    cellTemplate: projectTypeTemplate,
                },
                {
                    dataField: 'Volume',
                    caption: 'Volume',
                    cellTemplate: projectVolumeTemplate,
                },
                {
                    dataField: 'ERPJobId',
                    caption: 'Job #',
                },
                'Partners',
                {
                    dataField: 'PartnersType',
                    caption: 'Subs',
                },
                {
                    dataField: 'UserTasks',
                    caption: 'Next Task',
                    alignment: 'center',
                    cellTemplate: projectNextTaskTemplate,
                },
                {
                    dataField: 'DueDate',
                    caption: 'Due',
                    cellTemplate: projectDateTemplate,
                },
            ],
            onCellPrepared: function (e) {
                if (e.rowType == "header" || e.rowType == "data") {
                    e.cellElement.css("text-align", "center");
                }
                if (e.rowType == "data" && (e.column.name == 'TotalAllocations')) {
                    e.cellElement.addClass("circle-alignment");
                }
            },
            showBorders: true,
        });
    });


    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }
</script>
<div id="loadpanel"></div>
<div id="userTaskFieldPopup"></div>
<div class="opm-outer-box">
    <div class="row">
        <div class="col-md-12">
            <div class="lineHorizontal__container">
                <div class="groupStyle justify-content">
                    <img class="dxGridView_gvExpandedButton_UGITNavyBlueDevEx expandClass" onclick="togglElement($('.toggle-outer-OPM'), $(this));" src="/content/images/RMONE/left-arrow.png" alt="Collapse" style="cursor: pointer; width: 10px;">
                    <span class="dxeBase_UGITNavyBlueDevEx" style="font-size: 16px; font-weight: 500; margin: 0px 3px; vertical-align: middle; white-space: nowrap;">Opportunities</span>
                </div>
                <div class="lineHorizontal"></div>
            </div>
        </div>
    </div>
    <div class="toggle-outer-OPM">
        <div class="row">
            <div class="col-md-12">
                <div id="OPMDetailsDiv"></div>
            </div>
        </div>
    </div>
</div>
<div class="cpr-outer-box">
    <div class="row">
        <div class="col-md-12">
            <div class="lineHorizontal__container">
                <div class="groupStyle justify-content">
                    <img class="dxGridView_gvExpandedButton_UGITNavyBlueDevEx expandClass" onclick="togglElement($('.toggle-outer-CPR'), $(this));" src="/content/images/RMONE/left-arrow.png" alt="Collapse" style="cursor: pointer; width: 10px;">
                    <span class="dxeBase_UGITNavyBlueDevEx" style="font-size: 16px; font-weight: 500; margin: 0px 3px; vertical-align: middle; white-space: nowrap;">Construction</span>
                </div>
                <div class="lineHorizontal"></div>
            </div>
        </div>
    </div>
    <div class="toggle-outer-CPR">
        <div class="row">
            <div class="col-md-12">
                <div id="CPRDetailsDiv"></div>
            </div>
        </div>
    </div>
</div>
<div class="cns-outer-box">
    <div class="row">
        <div class="col-md-12">
            <div class="lineHorizontal__container">
                <div class="groupStyle justify-content">
                    <img class="dxGridView_gvExpandedButton_UGITNavyBlueDevEx expandClass" onclick="togglElement($('.toggle-outer-Other'), $(this));" src="/content/images/RMONE/left-arrow.png" alt="Collapse" style="cursor: pointer; width: 10px;">
                    <span class="dxeBase_UGITNavyBlueDevEx" style="font-size: 16px; font-weight: 500; margin: 0px 3px; vertical-align: middle; white-space: nowrap;">Other</span>
                </div>
                <div class="lineHorizontal"></div>
            </div>
        </div>
    </div>
    <div class="toggle-outer-Other">
        <div class="row">
            <div class="col-md-12">
                <div id="CNSDetailsDiv"></div>
            </div>
        </div>
    </div>
</div>
