<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllocationGantt.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMM.AllocationGantt" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    html {
        scroll-behavior: smooth;
    }

    #header {
        text-align: left;
        /*height: 30px;*/
        float: left;
        padding: 0px 2px;
    }

    .shadow-effect {
        box-shadow: 0 6px 14px #ddd;
        border-radius: 6px;
        padding-top: 10px;
    }

    .workExbar .dx-icon {
        width: 34px;
        height: 27px;
    }

    .dx-popup-content {
        padding-top: 10px;
        padding-bottom: 10px;
    }

    .workExbar .dx-button-has-icon .dx-button-content {
        padding: 4px;
    }

    .workExbar .dx-button-mode-contained.state-active {
        background-color: #979797;
    }

    .workExbar .dx-button-mode-contained.state-active .dx-icon {
            filter: brightness(100);
        }

    #content {
        width: 100%;
    }

    a, img {
        border: 0px;
    }

        a:hover {
            text-decoration: underline;
        }

    .aAddItem_Top {
        padding-left: 10px;
    }

    .dxgvHeader_CustomMaterial {
    }

    .dx-checkbox-container {
        align-items: flex-start;
    }

    .projectTeam_linkWrap {
        margin-bottom: 12px;
    }

    .dataView_btn {
        margin-top: 15px;
        margin-bottom: 12px;
    }

    .dx-calendar-body thead th {
        width: 0px;
    }

    .isa_info, .isa_success, .isa_warning, .isa_error {
        margin: 10px 0px;
        padding: 12px;
    }

    .isa_info {
        color: #00529B;
        background-color: #BDE5F8;
    }

    .isa_success {
        color: #4F8A10;
        background-color: #DFF2BF;
    }

    .isa_warning {
        color: #9F6000;
        background-color: #FEEFB3;
    }

    .isa_error {
        color: #D8000C;
        background-color: #FFD2D2;
    }

        .isa_info i, .isa_success i, .isa_warning i, .isa_error i {
            margin: 10px 30px;
            font-size: 2em;
            vertical-align: middle;
        }

    .chkFilterCheck {
        /*padding-top:5px;*/
        padding-left: 2px;
        padding-right: 2px;
        width: unset;
    }

    .tileViewContainer .dx-tile {
        text-align: center;
    }

    .ClassNoLevel {
        background-color: #82e186;
        padding-bottom: 5px;
        padding-top: 5px;
        height: inherit;
    }

    .ClassLevel1 {
        background-color: #d0ff3f;
        padding-bottom: 5px;
        padding-top: 5px;
        height: inherit;
    }

    .ClassLevel2 {
        background-color: #82e186;
        padding-bottom: 5px;
        padding-top: 5px;
        height: inherit;
    }

    .ClassLevel3 {
        background-color: #f68d67;
        padding-bottom: 5px;
        padding-top: 5px;
        height: inherit;
    }

    .btnProceed, .btnProceed:hover {
        background-color: #4A6EE2;
        margin-top: 20px;
        color: #fff;
    }

    /*.btnAddNew, .btnAddNew:hover {
        background-color: #4A6EE2;
        margin-top: 20px;
        color: #fff;
    }*/

    #tileViewContainer .dx-empty-message {
        text-align: center;
        padding-top: 62px;
    }

    #tileViewContainer .capacityblock {
        float: left;
        width: 74px;
        text-align: center;
        height: 20px;
    }

        #tileViewContainer .capacityblock:first-child {
            border-right: 1px solid #c3c3c3;
        }

    #tileViewContainer .allocation-v0 {
        background: #ffffff;
    }

    #tileViewContainer .allocation-v1 {
        background: #fcf7b5;
    }

    #tileViewContainer .allocation-v2 {
        background: #f8ac4a;
    }

    #tileViewContainer .allocation-r0 {
    }

    #tileViewContainer .allocation-r1 {
        background: #baf0d7;
    }

    #tileViewContainer .allocation-r2 {
        background: #fcf7b5;
    }

    #tileViewContainer .allocation-r3 {
        background: #f8ac4a;
    }

    #tileViewContainer .allocation-r4 {
        background: #ff0d0d;
        color: #e8e6e6;
    }

    #tileViewContainer .allocation-c0 {
        background: #ffffff;
    }

    #tileViewContainer .allocation-c1 {
        background: #fcf7b5;
    }

    #tileViewContainer .allocation-c2 {
        background: #f8ac4a;
    }

    #tileViewContainer .allocation-block {
        height: 63px;
        display: flex;
        justify-content: center;
        align-items: center;
    }

        #tileViewContainer .allocation-block .timesheet {
            position: absolute;
            top: 1px;
            left: 87%;
            cursor: pointer;
        }

    #tileViewContainer .dx-tile {
        border: 1px solid #c3c3c3;
    }

    #tileViewContainer .capacitymain {
        border-top: 1px solid #c3c3c3;
    }

    /*#btnAddNew .dx-icon {
        font-size: 20px;
        color: white;
    }

    #btnSaveAllocation .dx-icon {
        font-size: 20px;
        color: white;
    }

    #btnSaveAsTemplate .dx-icon {
        font-size: 20px;
        color: white;
    }

    #btnImportTemplate .dx-icon {
        font-size: 20px;
        color: white;
    }*/
    .filterlb-jobtitle {
        float: left;
        padding-left: 15px;
        float: left;
        padding-top: 7px;
        margin-top: 5px;
        width: 80px;
    }

    .filterctrl-jobtitle {
        /*padding-left: 25px;*/
        width: 30%;
        float: left;
        /*margin-left: 10px;*/
        margin-top: 6px;
    }

    .filterlb-jobDepartment {
        float: left;
        clear: both;
        padding-left: 15px;
        float: left;
        padding-top: 7px;
        padding-right: 7px;
        margin-top: 5px;
        width: 100px;
    }

    .filterctrl-jobDepartment {
        clear: both;
        float: left;
        width: 30%;
        margin-top: 6px;
        /*margin-left:12px;*/
    }

    .filterctrl-userpicker {
        width: 30%;
        float: left;
        margin-left: 10px;
        margin-top: 6px;
    }

    .cls .dx-datagrid-revert-tooltip {
        display: none;
    }

    .imgDelete {
    }

    .PopupCustomPosition {
        transform: translate(100%, 100px) scale(1) !important;
        margin-left: 5% !important;
        margin-top: 0% !important;
    }

    @media (min-width: 992px) {
        .col-cus1 {
            width: 10%;
        }

        .pt-cus1 {
            padding-top: 29px;
        }

        .noPadd1 {
            padding-left: 0;
            padding-right: 0;
        }
    }

    .custom-task-color-0 {
        background-color: #4A6EE2;
    }

    .custom-task-color-1{
        background-color: #8a8a8a;
    }

    .custom-task-color-2{
        background-color: #97DA97;
    }

    .custom-task-color-3{
        background-color: #9DC186;
    }

    .custom-task-color-4{
        background-color: #6BA538;
    }

    .custom-task-color-5{
        background-color: #F2BC57;
    }

    .custom-task-color-6{
        background-color: #E62F2F;
    }

    .custom-task {
        max-height: 38px;
        height: 100%;
        display: block;
        overflow: hidden;
    }

    .custom-task-wrapper {
        padding: 5px;
        color: #fff;
    }


        .custom-task-wrapper > * {
            display: block;
            overflow: hidden;
            text-overflow: ellipsis;
        }

    .custom-task-img-wrapper {
        float: left;
        width: 26px;
        height: 26px;
        border-radius: 50%;
        margin: 6px;
        background-color: #fff;
        overflow: hidden;
    }

    .custom-task-img {
        width: 28px;
    }

    .custom-task-title {
        font-weight: 600;
        font-size: 13px;
    }

    .custom-task-row {
        font-size: 11px;
    }

    .custom-task-progress {
        position: absolute;
        left: 0;
        bottom: 0;
        width: 0%;
        height: 6px;
        background: #00b2ef;
    }

    .dx-gantt .dx-row {
        height: 38px;
    }

    .divMarginTop {
        margin-top: 15px;
    }

    .divMarginBottom {
        margin-bottom: 15px;
    }

    #divRadioChoice {
        margin-top: 15px;
    }

    .dx-button-mode-contained .dx-icon {
        color: dodgerblue;
    }

    .dx-button-has-text .dx-button-content {
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
        padding: 7px 5px 8px;
    }

    .paddingBottom{
        padding-bottom: 30px;
    }

    .continueClass{
        margin:15px; 
        float:right;
        margin-right:0px;
    }

    .grouptitle {
        float: left;
        font-size: 16px;
    font-weight: 800;
    }

    .grouprole {
        float: left;
        padding-left: 10px;
        font-weight: 500;
    color: grey;
    }

    .groupimg {
        float: left;
        padding-left: 10px;
    }

    div.expand{
        height:20px;
    }
    /*div.expand::before {
        content: url('/Content/Images/downarrow_new.png');
    }

    div.collapse::before {
        content: url('content/images/uparrow_new.png');
    }*/
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var baseUrl = ugitConfig.apiBaseUrl;
    var date = new Date();

    $(function () {
        $.get(baseUrl + "/api/rmmapi/GetResourceAllocations?userid=" + '<%=SelectedUser%>' + "&selecteduserids=" + '<%=SelectedUsers%>' + "&selectedyear=" + '<%=SelectedYear%>'
            + "&chkIncludeClosed=" + '<%=IncludeClosed%>' + "&userall=" + '<%=UserAll%>', function (data) {
            var ganttSource = data;
            var maxDaterange = new Date(Math.max.apply(null, ganttSource.map(function (e) {
                return new Date(e.AllocationEndDate);

            })));

            var minDaterange = new Date(Math.min.apply(null, ganttSource.map(function (e) {
                return new Date(e.AllocationStartDate);

            })));
            
            $("#divAllocationGantt").dxGantt({
                height:610,
                tasks: {
                    dataSource: ganttSource,
                    endExpr: "AllocationEndDate",
                    keyExpr: "ChildId",
                    parentIdExpr: "ParentId",
                    progressExpr: "PctAllocation",
                    startExpr: "AllocationStartDate",
                    titleExpr: "Title"
                },
                resources: {
                    dataSource: ganttSource,
                    keyExpr: "Id",
                    textExpr: "SubWorkItem"
                },
                //resourceAssignments: {
                //    dataSource: ganttSource,
                //    keyExpr: "Id",
                //    textExpr: "SubWorkItem"
                //},
                showResources: true,
                editing: {
                    enabled: false,
                },
                validation: {
                    autoUpdateParentTasks: false,
                },
                columns: [{
                    dataField: 'Title',
                    caption: 'Name',
                    width: 200,
                    encodeHtml: false,
                    cellTemplate: function (container, options) {
                        debugger;
                        let title = options.data.Title;
                        let role = options.data.SubWorkItem;
                        let parentid = options.data.ParentId;
                        let workitemlink = options.data.WorkItemLink;
                        if (parentid == 0) {
                            if (options.row.isExpanded) {
                                $(`<div id='titleDiv'>`).append(
                                    $(`<div class='grouptitle'>${title}</div>`),
                                    $(`<div class='grouprole' />`).append(`(${role})`),
                                    $(`<div class='groupimg' />`).append(
                                        $("<img>", { "src": "/content/images/plus-blue.png", "height": "20px", "width": "20px" }),
                                    ),
                                    $(`<div />`).append(
                                        $("<img>", { "src": "/content/images/uparrow_new.png", "id":`${options.key}`, "class":"imgCollapse", "height": "20px", "width": "28px" }),
                                    ),
                                )
                                    .appendTo(container);
                            } else {
                                $(`<div id='titleDiv'>`).append(
                                    $(`<div class='grouptitle'>${title}</div>`),
                                    $(`<div class='grouprole' />`).append(`(${role})`),
                                    $(`<div class='groupimg' />`).append(
                                        $("<img>", { "src": "/content/images/plus-blue.png", "height": "20px", "width": "20px" }),
                                    ),
                                    $(`<div />`).append(
                                        $("<img>", { "src": "/content/images/downarrow_new.png", "id": `${options.key}`, "class":"imgExpand", "height": "20px", "width": "28px" }),
                                    ),
                                )
                                    .appendTo(container);
                            }
                            
                        }
                        else
                            container.append(`<div><div>${workitemlink}</div></div>`);
                    },
                    headerCellTemplate: function (container, options) {
                        $(`<div style="float: right; padding-top: 13px; padding-bottom: 13px;">`).
                            append(
                                $("<a>", { "id": "aAddItem", "style": "padding: 8px; padding-top: 7px; background: #4fa1d6; margin-left: 5px;border-radius:12px;", "class": "btn btn-sm db-quickTicket svcDashboard_addTicketBtn" }).append("+ Add Project"),
                                $("<a>", { "id": "btnAddMultiAllocation", "style": "padding: 8px; padding-top: 7px; background: #4fa1d6; margin-left: 5px;border-radius:12px;", "class": "btn btn-sm db-quickTicket svcDashboard_addTicketBtn" }).append("+ Add Allocation")
                                    )
                            .appendTo(container)
                    }
                }, {
                    dataField: 'AllocationStartDate',
                    caption: 'Start Date',
                    format: 'MMM d, yyyy',
                    dataType: "date",
                    visible: false,
                }, {
                    dataField: 'AllocationEndDate',
                    caption: 'End Date',
                    format: 'MMM d, yyyy',
                    dataType: "date",
                    visible:false,
                }, {
                    dataField: 'PctAllocation',
                    caption: '%',
                    width: 60,
                    alignment: 'center',
                    visible: false,
                    }, {
                    dataField: 'Color',
                    visible: false
                    }],
                toolbar: {
                    items: [
                        'fullScreen',
                        'separator',
                        'collapseAll',
                        'expandAll',
                        'separator',
                        'zoomIn',
                        'zoomOut',
                        {
                            widget: 'dxButton',
                            options: {
                                text: 'Today',
                                icon: 'spinnext',
                                stylingMode: 'text',
                                onClick(e) {
                                    var gantt = $("#divAllocationGantt").dxGantt("instance");
                                    gantt.scrollToDate(new Date(date.getFullYear(), date.getMonth(), 01));
                                },
                            },
                        },
                    ],
                },
                scaleType: 'months',
                taskListWidth: 500,
                taskContentTemplate: getTaskContentTemplate,
                startDateRange: new Date(minDaterange.getFullYear(), minDaterange.getMonth() - 1, 01),
                endDateRange: new Date(maxDaterange.getFullYear(), 11, 31),
                onScaleCellPrepared: function (e) {
                    
                    if (e.scaleIndex === 0) {
                        var scaleElement = e.scaleElement[0];
                        //scaleElement.style.width = "20px";
                        //let startMonth = e.startDate.getMonth() + 1;
                        //let endMonth = e.endDate.getMonth() + 1;
                        //if (e.scaleType == 'days') {
                        //    scaleElement.textContent = e.startDate.getDate();
                        //    //scaleElement.style.width = "20px";
                        //}
                        //if (e.scaleType == 'weeks') {
                        //    scaleElement.textContent = e.startDate.getDate() + '-' + e.endDate.getDate();
                        //    //scaleElement.style.width = "50px";
                        //}
                        //else if (e.scaleType == 'months') {
                        //    scaleElement.textContent = startMonth;
                        //    //scaleElement.style.width = "20px";
                        //}
                        //else if (e.scaleType == 'quarters') {
                        //    scaleElement.textContent = startMonth + '-' + endMonth;
                        //    //scaleElement.style.width = "50px";
                        //}
                        
                        
                    } else {
                        var scaleElement1 = e.scaleElement[0];
                        //scaleElement1.style.width = ;
                        //scaleElement.innerText = "top";
                    }
                }
            });

                    
            });
        
    });

    function getTaskContentTemplate(item) {

        
        const resource = item.taskResources[0];
        const img = "/Content/Images/girlprofilepic.jpg";
       
        const color = item.taskData.Color;
        const taskWidth = `${item.taskSize.width}px;`;
        const $customContainer = $(document.createElement('div'))
            .addClass('custom-task')
            .attr('style', `width:${taskWidth}`)
            .addClass(`custom-task-color-${color}`);

        const $wrapper = $(document.createElement('div'))
            .addClass('custom-task-wrapper')
            .appendTo($customContainer);

        $(document.createElement('div'))
            .addClass('custom-task-title')
            .text(`${item.taskData.Title} ${getPercentageText(item.taskData.PctAllocation)}`)
            .appendTo($wrapper);
        //$(document.createElement('div'))
        //    .addClass('custom-task-row')
        //    .text(`${item.taskData.ResourceUser} (${item.taskData.PctAllocation}%)`)
        //    .appendTo($wrapper);

        return $customContainer;
    }

    function getPercentageText(pct) {
        if (pct == "null" || pct == null || pct == "undefined")
            return "";
        else
            return `(${pct}%)`;
    }

    $("img.imgCollapse").click(function () {
        debugger;
        let key = $(this).attr("key");
        var gantt = $("#divAllocationGantt").dxGantt("instance");
        gantt.collapseTask(key);
    });

    $("img.imgExpand").click(function () {
        debugger;
        let key = $(this).attr("key");
        var gantt = $("#divAllocationGantt").dxGantt("instance");
        gantt.expandTask("task_key");
    });

    $('#aAddItem').click(function () {
        var url = '<%= AddNewUrl%>';
        window.parent.UgitOpenPopupDialog(url, '', '', '880px', '90', 0, '');
    });

    $('#btnAddMultiAllocation').click(function () {
        var url = '<%= MultiAddUrl%>';
        window.parent.UgitOpenPopupDialog(url, '', '', '880px', '90', 0, '');
    });
</script>
<div>
    <div id="divAllocationGanttPopup">

</div>
    <div id="divAllocationGantt">

    </div>
</div>