<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleConstraintsListDx.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMONE.ModuleConstraintsListDx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    #btnMoreTask{
        margin-left:35%;
    }
    .noDataText{
        text-align: center;
        font-size: 17px;
        font-weight: 500;
        color: black;
        padding-top: 40px;
    }
    #taskGrid .namestyle {
    margin-bottom: 3px;
    font-size:12px;
    font-family: 'Roboto', sans-serif !important;
    }
    #taskGrid .dx-datagrid{  
    font-size:12px;
    font-family: 'Roboto', sans-serif !important;
}

    .profileImage {
        margin: 0px 0px 0px 5px;
        font-size: 18px;
        border: 2px solid lightgray;
        border-radius: 26px;
        height: 20px;
        width: 20px;
    }
    .roboto-font-family {
        font-family: 'Roboto', sans-serif !important;
    }

    .dx-button-has-icon .dx-icon {
        width: 24px;
        height: 24px;
        background-position: 0 0;
        background-size: 18px 18px;
        padding: 0;
        font-size: 18px;
        text-align: center;
        line-height: 18px;
        margin-right: 0;
        margin-left: 0;
    }

    .addCommentButton{
        border:none;
        background:none;
        float:right;
        /*margin-top:7px;*/
    }

    .comment-title {
        text-align: center;
        font-size: 24px;
        font-weight: 600;
        /*padding: 10px;*/
        color: black;
    }

    .ui-widget.ui-widget-content {
        z-index: 1501;
    }
</style>

<%if (IsRequestFromSummary == false){%>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .dx-overlay-content.dx-popup-normal.dx-popup-draggable.dx-resizable.dx-popup-inherit-height {
        transform:translate(476px, 3px) scale(1) !important;
    }
</style>
<%}%>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var CommentDetails = null;
    var data = null;
    $(function () {

        $.get("/api/RMOne/GetModuleConstraintsList?projectID=" + '<%=TicketId%>', function (ConstrainDetails, status) {
            if (ConstrainDetails.lstModuleStageConstraints.length == 0) {
                $('#taskGrid').html("<div class='noDataText'>No Data Found</div>");
            }
            //console.log(ConstrainDetails);
            data = ConstrainDetails != "Unable to Fetch" ? ConstrainDetails.lstModuleStageConstraints : null;
            CommentDetails = ConstrainDetails.ListComments;
            //BindCommentDetails();

            <%--if ('<%=IsRequestFromSummaryOrTask%>' == 'True') {                
                BindTaskDetails();
            }--%>

            //console.log(CommentDetails);
            if (data.length > 0) {
                $.each(data, function (index, item) {
                    if (item.TaskStatus == "Completed") {
                        item.TaskDueDate = item.CompletionDate;
                    }
                });
                var moreTaskItems = data.length - 10;
                var topTasks = data.slice(0, 10);
                $('#taskGrid').dxDataGrid({
                    dataSource: topTasks,
                    remoteOperations: false,
                    height: 608,
                    //width: 450,
                    searchPanel: {
                        visible: false,
                    },
                    rowAlternationEnabled: true,
                    wordWrapEnabled: true,
                    showBorders: false,
                    showColumnHeaders: true,
                    showColumnLines: false,
                    showRowLines: false,
                    columns: [
                        {
                            dataField: "",
                            dataType: "",
                            icon: "comment",
                            headerCellTemplate: function (header, info) {
                                header.css('margin-left', '45%');
                                $(`<div><a href='#' style="color: black;font-size:14px;font-weight:600;" onclick=changeDiv(0)><img src="/Content/Images/comment-icon.png" style="height: 25px;"/></a></div>`).appendTo(header);
                            }

                        },
                        {
                            dataField: "AssignedToName",
                            dataType: "text",
                            caption: "Assigned",
                            sortIndex: "1",
                            sortOrder: "asc",
                            width: "37%",
                            cellTemplate: function (container, options) {
                                const profilepics = options.data.ProfilePics != null ? options.data.ProfilePics.split(',') : '';
                                container.css('position', 'relative');
                                var html = "";
                                var leftpx = 3;
                                if (profilepics.length > 0) {
                                    profilepics.forEach(function (item, index) {
                                        if (item != "") {
                                            $(`<img src="${item}" style="width:28px; height:28px; border-radius: 50%; position:absolute; left:${leftpx}px">`).appendTo(container);
                                        }
                                        else {
                                            $(`<img src="/Content/Images/userNew.png" style="width:28px; height:28px; border-radius: 50%; position:absolute; left:${leftpx}px">`).appendTo(container);
                                        }
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
                        },
                        {
                            dataField: "Title",
                            dataType: "text",
                            caption: "Task",
                            sortIndex: "1",
                            sortOrder: "asc",
                            width: "27%",
                            headerCellTemplate: function (header, info) {
                                $(`<div style="color: black;font-size:14px;font-weight:600;" title=${info.column.caption}>${info.column.caption}</div>`).appendTo(header);
                            }
                        },
                        {
                            dataField: "TaskStatus",
                            dataType: "text",
                            caption: "Status",
                            alignment: 'left',
                            //cellTemplate: function (container, options) {

                            //    if (options.value != null) {
                            //        if (options.value < 0)
                            //            $(`<div><i class='fa fa-circle' style='font-size: 10px; color:#E62F2F'></i>&nbsp;${options.value}&nbsp;Days</div>`).appendTo(container);
                            //        else if (options.value >= 0 && options.value <= 10)
                            //            $(`<div><i class='fa fa-circle' style='font-size: 10px; color:#F2BC57'></i>&nbsp;${options.value}&nbsp;Days</div>`).appendTo(container);
                            //        else if (options.value > 10 && options.value <= 20)
                            //            $(`<div><i class='fa fa-circle' style='font-size: 10px; color:#A9C23F'></i>&nbsp;${options.value}&nbsp;Days</div>`).appendTo(container);
                            //        else
                            //            $(`<div><i class='fa fa-circle' style='font-size: 10px; color:#6BA538'></i>&nbsp;${options.value}&nbsp;Days</div>`).appendTo(container);
                            //    }
                            //},
                            allowFiltering: false,
                            allowSorting: false,
                            headerCellTemplate: function (header, info) {
                                $(`<div style="color: black;font-size:14px;font-weight:600;" title=${info.column.caption}>${info.column.caption}</div>`).appendTo(header);
                            }
                        },
                        {
                            dataField: "TaskDueDate",
                            dataType: "date",
                            caption: "Date",
                            allowFiltering: false,
                            allowSorting: false,
                            headerCellTemplate: function (header, info) {
                                $(`<div style="color: black;font-size:14px;font-weight:600;" title=${info.column.caption}>${info.column.caption}</div>`).appendTo(header);
                            },
                            //cellTemplate: function (container, options) {
                            //    if (options.value != null)
                            //        $(`<div>${new Date(options.value)}</div>`).appendTo(container);
                            //}
                        },
                    ],
                    onRowPrepared(e) {
                        e.rowElement.css({ "height": "50px" });
                    },
                    onRowClick(e) {
                        editTicketTask(e.data.ModuleNameLookup, e.data.TicketId, e.data.ModuleStep, e.data.ID, e.data.Title);
                    }
                });

                if (moreTaskItems > 0) {
                    $("#btnMoreTask").dxButton({
                        text: 'More: ' + moreTaskItems,
                        stylingMode: 'text',
                        icon: 'chevrondown',
                        focusStateEnabled: false,
                        onClick: function (e) {
                            var taskGrid = $('#taskGrid').dxDataGrid("instance");
                            taskGrid.option('dataSource', data);
                            e.component.option('visible', false);
                        }
                    });
                }
            }
            /*if (CommentDetails.length > 0) {*/
                BindCommentDetails();
            /*}*/
            changeDiv(0);
        });

        

    });
    <%--function editTicketTask(moduleName, ticketID, moduleStage, taskID, title) {
        var taskUrl = '<%=ConstraintTaskUrl%>';
        var taskParams = "";
        taskParams = "module=" + moduleName + "&ticketId=" + ticketID + "&conditionType='ModuleTaskCT'&moduleStage=" + moduleStage + "&taskID=" + taskID + "&type='ExistingConstraint'" + "&viewType=0" + "&IsReqFromSummary=true";
        window.UgitOpenPopupDialog(taskUrl, taskParams, title, '800px', '700px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }--%>

    function editTicketTask(moduleName, ticketID, moduleStage, taskID, title) {
        var taskUrl = '<%=ConstraintTaskUrl%>';
        var taskParams = "";
        taskParams = "module=" + moduleName + "&ticketId=" + ticketID + "&conditionType='ModuleTaskCT'&moduleStage=" + moduleStage + "&taskID=" + taskID + "&type='ExistingConstraint'" + "&viewType=0"+"&isModuleConstraint=1";
        window.UgitOpenPopupDialog(taskUrl, taskParams, title, '800px', '700px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
       

    function changeDiv(type) {
        if ($.cookie("fromCommentTask") != null && $.cookie("fromCommentTask") != "" && $.cookie("fromCommentTask") != "2") {
            if ($.cookie("fromCommentTask") == "0") {
                $("#commentGrid").show();
                $("#taskGrid").hide();
            }
            else if ($.cookie("fromCommentTask") == "1") {
                $("#taskGrid").show();
                $("#commentGrid").hide();
            }
            $.cookie("fromCommentTask", "2");
        }
        else {
            if (type == 0) {
                $("#commentGrid").show();
                $("#taskGrid").hide();
            }
            else if (type == 1) {
                $("#taskGrid").show();
                $("#commentGrid").hide();
            }
        }
    }

    //function CloseCommentDetails() {
    //    const popup = $("#taskDetailsDialog").dxPopup("instance");
    //    popup.hide();
    //}

    <%--function BindTaskDetails() {
        $("#taskDetailsDialog").html('');
        var moreTaskItems = data.length - 10;
        var topTasks = data.slice(0, 10);
        const popupTaskDetails = function () {
            let container = $('<div class="roboto-font-family">');
            container.append(
                $("<div style='display:flex;' class='mb-2'>").append(
                    $(`<div class="col-md-4"><a href='#' style="color: black;font-size:14px;font-weight:600;" onclick=CloseCommentDetails()><img src="/Content/Images/comment-icon.png" style="height: 25px;"/></a></div>
                    <div class="col-md-4 comment-title"><Span>Task</Span></div>`)
                ),
                $("<div style='display:flex;' class='mb-2'>").append(
                    $("<div class='mr-1'>").append(
                        $('<div id="TaskGrid">').dxDataGrid({
                            dataSource: topTasks,
                            remoteOperations: false,
                            height: 350,
                            width: 700,
                            searchPanel: {
                                visible: false,
                            },
                            rowAlternationEnabled: true,
                            wordWrapEnabled: true,
                            showBorders: false,
                            showColumnHeaders: true,
                            showColumnLines: false,
                            showRowLines: false,
                            columns: [
                                //{
                                //    dataField: "",
                                //    dataType: "",
                                //    icon: "comment",
                                //    headerCellTemplate: function (header, info) {
                                //        header.css('margin-left', '45%');
                                //        $(`<div><a href='#' style="color: black;font-size:14px;font-weight:600;" onclick=OpenCommentDetails()><img src="/Content/Images/comment-icon.png" style="height: 25px;"/></a></div>`).appendTo(header);
                                //    }

                                //},
                                {
                                    dataField: "AssignedToName",
                                    dataType: "text",
                                    caption: "Assigned",
                                    sortIndex: "1",
                                    sortOrder: "asc",
                                    width: "37%",
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
                                },
                                {
                                    dataField: "Title",
                                    dataType: "text",
                                    caption: "Task",
                                    sortIndex: "1",
                                    sortOrder: "asc",
                                    width: "27%",
                                    headerCellTemplate: function (header, info) {
                                        $(`<div style="color: black;font-size:14px;font-weight:600;" title=${info.column.caption}>${info.column.caption}</div>`).appendTo(header);
                                    }
                                },
                                {
                                    dataField: "TaskStatus",
                                    dataType: "text",
                                    caption: "Status",
                                    alignment: 'left',
                                    //cellTemplate: function (container, options) {

                                    //    if (options.value != null) {
                                    //        if (options.value < 0)
                                    //            $(`<div><i class='fa fa-circle' style='font-size: 10px; color:#E62F2F'></i>&nbsp;${options.value}&nbsp;Days</div>`).appendTo(container);
                                    //        else if (options.value >= 0 && options.value <= 10)
                                    //            $(`<div><i class='fa fa-circle' style='font-size: 10px; color:#F2BC57'></i>&nbsp;${options.value}&nbsp;Days</div>`).appendTo(container);
                                    //        else if (options.value > 10 && options.value <= 20)
                                    //            $(`<div><i class='fa fa-circle' style='font-size: 10px; color:#A9C23F'></i>&nbsp;${options.value}&nbsp;Days</div>`).appendTo(container);
                                    //        else
                                    //            $(`<div><i class='fa fa-circle' style='font-size: 10px; color:#6BA538'></i>&nbsp;${options.value}&nbsp;Days</div>`).appendTo(container);
                                    //    }
                                    //},
                                    allowFiltering: false,
                                    allowSorting: false,
                                    headerCellTemplate: function (header, info) {
                                        $(`<div style="color: black;font-size:14px;font-weight:600;" title=${info.column.caption}>${info.column.caption}</div>`).appendTo(header);
                                    }
                                },
                                {
                                    dataField: "TaskDueDate",
                                    dataType: "date",
                                    caption: "Date",
                                    allowFiltering: false,
                                    allowSorting: false,
                                    headerCellTemplate: function (header, info) {
                                        $(`<div style="color: black;font-size:14px;font-weight:600;" title=${info.column.caption}>${info.column.caption}</div>`).appendTo(header);
                                    },
                                    //cellTemplate: function (container, options) {
                                    //    if (options.value != null)
                                    //        $(`<div>${new Date(options.value)}</div>`).appendTo(container);
                                    //}
                                },
                            ],
                            onRowPrepared(e) {
                                e.rowElement.css({ "height": "50px" });
                            },
                            onRowClick(e) {
                                editTicketTask(e.data.ModuleNameLookup, e.data.TicketId, e.data.ModuleStep, e.data.ID, e.data.Title);
                            }
                        })
                    )
                ),
            );
            let cancelBtn = $(`<div class="mt-2 mb-3 btnAddNew" style='float:right;font-size: 14px;' />`).dxButton({
                text: "Close",
                visible: true,
                onClick: function (e) {
                    //debugger;
                    if ('<%=IsRequestFromSummary%>' == 'False') {
                        setTimeout(function () {
                            showLoader();
                        }, 1000);
                        $.cookie("TicketSelectedTab", 1, { path: "/" });
                        window.parent.location.reload();
                    }
                    else {
                        const popup = $("#taskDetailsDialog").dxPopup("instance");
                        popup.hide();
                    }
                    
                }
            })
            //debugger;
            let moreBtn = "";
            if (moreTaskItems > 0) {
                moreBtn = $(`<div class="mt-2 mb-3 btnAddNew" id='btnMoreTask' style='font-size: 14px;' />`).dxButton({
                    text: 'More: ' + moreTaskItems,
                    stylingMode: 'text',
                    icon: 'chevrondown',
                    focusStateEnabled: false,
                    onClick: function (e) {
                        var taskGrid = $('#TaskGrid').dxDataGrid("instance");
                        taskGrid.option('dataSource', data);
                        e.component.option('visible', false);
                    }
                });
            }

            container.append(cancelBtn);
            if (moreTaskItems > 0) {
                container.append(moreBtn);
            }
            return container;
        };

        const popup = $("#taskDetailsDialog").dxPopup({
            contentTemplate: popupTaskDetails,
            width: "auto",
            height: "auto",
            showTitle: false,
            //title: "Comments",
            visible: false,
            dragEnabled: true,
            hideOnOutsideClick: false,
            //showCloseButton: true,
            position: {
                at: 'center',
                my: 'center',
            },
            onHiding: function () {

            }
        }).dxPopup('instance');

        popup.option({
            contentTemplate: () => popupTaskDetails()
        });
        popup.show();
    }--%>

    function BindCommentDetails() {
        //debugger;
        $('#commentGrid').html('');
        let container = $('#commentGrid');
        container.append(
            /*$("<div style='display:flex;' class='mb-2'>").append(*/
            $("<div class='row col-md-12' style='padding:0px;'>").append(
                    $(`<div class="col-md-4" style="margin-top:5px;" onclick=changeDiv(1)><img src="/Content/Images/task-list.png" style="height:25px;" /></div>
                 <div class="col-md-4 comment-title"><Span>Comments</Span></div>`),
                    $(`<div class="col-md-4" style="float:right;">`).append(
                        $(`<div id="btnAddComments" class="addCommentButton">`).dxButton({
                            icon: '/content/images/plus-blue-new.png',
                            onClick() {
                                OpenAddCommentPopup();
                            },
                        })
                    )
                ),
            //),
            //$("<div style='display:flex;' class='mb-2'>").append(
                $("<div class='row col-md-12' style='padding:0px;'>").append(
                    $('<div id="commentsGrid">').dxDataGrid({
                        dataSource: CommentDetails,
                        remoteOperations: false,
                        searchPanel: {
                            visible: false,
                        },
                        height: 610,
                        //width: 440,
                        rowAlternationEnabled: true,
                        wordWrapEnabled: true,
                        showBorders: false,
                        showColumnHeaders: false,
                        showColumnLines: false,
                        showRowLines: true,
                        noDataText: "No Data Found",
                        columns: [
                            {
                                dataField: "CommentedBy",
                                dataType: "text",
                                width: "18%",
                                caption: '',
                                cellTemplate: function (container, options) {
                                    if (options.data != null) {
                                        $(`<div class="col-md-6 roboto-font-family"><img src="${options.data.Picture}" class="profileImage"><span style='margin-left:5px;'>${options.data.createdBy}</span></div>`).appendTo(container);
                                        $(`<div class="col-md-6 roboto-font-family"><span style='margin-left:5px;float: right;font-size:12px;'>${options.data.created}</span></div>`).appendTo(container);
                                        $(`<div class="col-md-12 roboto-font-family" style="padding-top: 5px;"><span style='margin-left:5px;'>${options.data.entry}</span></div>`).appendTo(container);
                                    }
                                }
                            },
                        ],
                        onRowClick(e) {
                            editComment(e.data.entry, e.data.Index);
                        }
                    })
                )
            //)
        );
        return container;
    }

    function OpenAddCommentPopup() {
        const popupAddComment = function () {
            let container = $('<div class="roboto-font-family">');
            container.append(
                $("<div id='commentDescription' />").dxTextArea({
                    height: 120,
                    inputAttr: { 'aria-label': 'Comment' },
                }).dxValidator({
                    validationGroup: "addCommentValidate",
                    validationRules: [{
                        type: "required",
                        message: "Enter Comment"
                    }]
                })
            );
            let confirmBtn = $(`<div class="mt-2 mb-3 ml-2 btnAddNew" style='float:right;font-size: 14px;' />`).dxButton({
                text: "Save",
                hint: 'Save',
                visible: true,
                onClick: AddComments
            })
            let cancelBtn = $(`<div class="mt-2 mb-3 btnAddNew" style='float:right;font-size: 14px;' />`).dxButton({
                text: "Cancel",
                visible: true,
                onClick: function (e) {
                    const popup = $("#addCommentPopup").dxPopup("instance");
                    popup.hide();
                }
            })
            container.append(confirmBtn);
            container.append(cancelBtn);
            return container;
        };

        const popup = $('#addCommentPopup').dxPopup({
            contentTemplate: popupAddComment,
            visible: false,
            hideOnOutsideClick: false,
            showTitle: true,
            showCloseButton: true,
            title: "Add Comment",
            width: 500,
            height: "auto",
            resizeEnabled: true,
            dragEnabled: true,
        }).dxPopup('instance');

        popup.option({
            contentTemplate: () => popupAddComment()
        });
        popup.show();
    }

    function editComment(OldComments, Indexs) {
        const popupUpdateComment = function () {
            let container = $('<div class="roboto-font-family">');
            container.append(
                $("<div id='updateCommentDescription' />").dxTextArea({
                    height: 120,
                    value: OldComments,
                    inputAttr: { 'aria-label': 'Comment' },
                }).dxValidator({
                    validationGroup: "updateCommentValidate",
                    validationRules: [{
                        type: "required",
                        message: "Enter Comment"
                    }]
                })
            );
            let confirmBtn = $(`<div class="mt-2 mb-3 ml-2 btnAddNew" style='float:right;font-size: 14px;' />`).dxButton({
                text: "Save",
                hint: 'Save',
                visible: true,
                onClick: function (e) {
                    UpdateComments(OldComments, Indexs)
                }
            })
            let cancelBtn = $(`<div class="mt-2 mb-3 btnAddNew" style='float:right;font-size: 14px;' />`).dxButton({
                text: "Cancel",
                visible: true,
                onClick: function (e) {
                    const popup = $("#updateCommentPopup").dxPopup("instance");
                    popup.hide();
                }
            })
            container.append(confirmBtn);
            container.append(cancelBtn);
            return container;
        };

        const popup = $('#updateCommentPopup').dxPopup({
            contentTemplate: popupUpdateComment,
            visible: false,
            hideOnOutsideClick: false,
            showTitle: true,
            showCloseButton: true,
            title: "Update Comment",
            width: 500,
            height: "auto",
            resizeEnabled: true,
            dragEnabled: true,
        }).dxPopup('instance');

        popup.option({
            contentTemplate: () => popupUpdateComment()
        });
        popup.show();
    }

    function AddComments() {
        //debugger;
        var result = DevExpress.validationEngine.validateGroup("addCommentValidate").isValid;
        if (result) {
            var Comment = $("#commentDescription").dxTextArea('instance').option('value');
            var TicketID = '<%=TicketId%>';
            $.post("/api/RMOne/AddCommentDetails?Comment=" + encodeURIComponent(Comment) + "&TicketID=" + TicketID).then(function (response) {
                //debugger;
                const popup = $("#addCommentPopup").dxPopup("instance");
                popup.hide();
                //debugger;
                $.cookie("fromCommentTask", "0", { path: "/" });
                <%--if ('<%=IsRequestFromSummary%>' == 'False') {
                    setTimeout(function () {
                        showLoader();
                    }, 2000);

                    $.cookie("TicketSelectedTab", $.cookie("TicketSelectedTabValue"), { path: "/" });
                    window.parent.location.reload();
                }
                else {
                    const popup = $("#addCommentPopup").dxPopup("instance");
                    popup.hide();--%>
                    $.get("/api/RMOne/GetModuleConstraintsList?projectID=" + '<%=TicketId%>', function (ConstrainDetails, status) {
                        CommentDetails = ConstrainDetails.ListComments;
                        BindCommentDetails();
                    });                    
                //}
                
            });
        }
    }
    
    function UpdateComments(OldComments, Indexs) {
        var result = DevExpress.validationEngine.validateGroup("updateCommentValidate").isValid;
        if (result) {
            var Comments = $("#updateCommentDescription").dxTextArea('instance').option('value');
            var TicketID = '<%=TicketId%>';
            $.post("/api/RMOne/UpdateCommentDetails?Comment=" + encodeURIComponent(Comments) + "&Index=" + Indexs + "&TicketID=" + TicketID).then(function (response) {
                <%--if ('<%=IsRequestFromSummary%>' == 'True') {--%>
                const popup = $("#updateCommentPopup").dxPopup("instance");
                popup.hide();
                $.cookie("fromCommentTask", "0", { path: "/" });
                //debugger;
                <%--if ('<%=IsRequestFromSummary%>' == 'False') {
                    setTimeout(function () {
                        showLoader();
                    }, 2000);
                    $.cookie("TicketSelectedTab", $.cookie("TicketSelectedTabValue"), { path: "/" });
                    window.parent.location.reload();
                }
                else {
                    const popup = $("#updateCommentPopup").dxPopup("instance");
                    popup.hide();--%>
                    $.get("/api/RMOne/GetModuleConstraintsList?projectID=" + '<%=TicketId%>', function (ConstrainDetails, status) {
                        CommentDetails = ConstrainDetails.ListComments;
                        BindCommentDetails();
                    });

                //}

            });
        }
    }
    function showLoader() {
        ResourceAvailabilityloadingPanel.SetText('Loading...');
        var width = window.innerWidth;
        var height = window.outerHeight;
        ResourceAvailabilityloadingPanel.ShowAtPos((width / 2), (height / 4));
    }
</script>
<dx:ASPxLoadingPanel ID="ResourceAvailabilityloadingPanel" runat="server" ClientInstanceName="ResourceAvailabilityloadingPanel" Text="Loading..." ImagePosition="Top" CssClass="customeLoader" Modal="True">
    <Image Url="~/Content/Images/ajaxloader.gif"></Image>
</dx:ASPxLoadingPanel>
<div id="taskGrid" style="display:none;"></div>
<div id="commentGrid"></div>
<div id="btnMoreTask">    
</div>
<div id="addCommentPopup"></div>
<div id="updateCommentPopup"></div>
<div id="CommentDetailsDialog"></div>
<div id="taskDetailsDialog"></div>