    <%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewProjectTask.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.NewProjectTask" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js?v=<%=UGITUtility.AssemblyVersion %>"></script>
<script src="https://unpkg.com/jspdf-autotable@3.5.20/dist/jspdf.plugin.autotable.js"></script>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .gantView-gridWrp {
        overflow: hidden;
        text-overflow: ellipsis;
    }

    .cell-font-bold {
        vertical-align: middle !important;
    }

    .cell-summary-PercentComplete {
        vertical-align: middle !important;
        padding-right: 97px !important;
    }

    .dx-link-delete {
        color: red !important;
    }

    #header {
        text-align: left;
        /*height: 30px;*/
        float: left;
        padding: 0px 2px;
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
        background-color: #4fa1d6;
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

    #tileViewContainer .dx-tile {
        border: 1px solid #c3c3c3;
    }

    #tileViewContainer .capacitymain {
        border-top: 1px solid #c3c3c3;
    }

    .pmmEdit-listGrid .dx-treelist-expanded {
/*        display: none;
*/    }

    .pmmEdit-listGrid .dx-treelist-collapsed {
        /*display: none;*/
    }
    /*BTS-22-000750	Scrollbar removed from inner task list. Outer page of compact view will scroll same as Full view.*/
    .pmmEdit-listGrid {
        max-height: unset !important;
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

    /*.filterctrl-jobDepartment {
        clear: both;
        float: left;
        width: 30%;
        margin-top: 6px;
        margin-left: 12px;
    }*/

    .filterctrl-userpicker {
        width: 30%;
        float: left;
        margin-left: 10px;
        margin-top: 6px;
    }

    .cls .dx-datagrid-revert-tooltip {
        display: none;
    }

    /*.toolbar {
        padding-top: 7px;
    }*/

    #mppFileUploader {
        float: left;
        width: 28% !important;
        height: 60px;
    }

        #mppFileUploader .dx-fileuploader-input-wrapper, #mppFileUploader .dx-fileuploader-wrapper {
            padding: 0px;
        }

    #buttonContainer {
        margin: 0 0px;
        float: left;
        width: 50%;
        height: 60px;
        border: none;
    }

        #buttonContainer .dx-button-content {
            border: 1px solid #ddd;
            padding: 5px 44px;
            display: inline-block;
            margin-top: 3px;
            border-radius: 4px;
            height: 37px;
            float: left;
        }

    #chkImportDates, #chkDontImportAssignee, #chkCalculateEstmHrs {
        float: left;
        margin-right: 10px;
        padding-left: 5px;
    }

    #mppFileUploader .dx-fileuploader-file {
        z-index: 123;
        bottom: 13px;
        position: absolute;
        padding-left: 5px;
    }

    #mppFileUploader .dx-fileuploader-file-info {
        width: auto;
        margin-right: 6px;
    }

    .dx-highlight-outline {
        display: inline-block;
        width: 100%;
        margin-top: 3px;
    }

        .dx-highlight-outline::after {
            height: 100%;
        }

    .dx-treelist-rowsview .dx-treelist-checkbox-size .dx-checkbox-icon {
        margin-top: 12px;
    }

    .dx-toolbar-before .dx-toolbar-button:first-child .dx-button-has-icon {
        margin-left: 6px;
    }

    .dx-toolbar-after {
        margin-right: 6px;
    }
    /*#tree-list {
        max-height: 390px;
    }*/
    .dx-tag-content {
        padding: 1px;
        text-align: left;
        width: 145px;
    }

    .dx-gantt-task {
        background-color: #337ab7;
        border-radius: 4px;
        text-align: center;
    }

    .selectedScale {
        background-color: cornflowerblue;
    }

    .unselectedScale {
    }

    .ugit-highlight-outline {
        border: 2px solid #A1DAAE !important;
        margin-top: 3px;
        margin-bottom: 2px;
        margin-left: 1px;
        margin-right: 1px;
    }

    #divTaskListSummary .dx-datagrid .dx-column-lines > td {
        border-left: none;
        border-right: none;
    }
    .dx-treelist-icon-container.dx-editor-inline-block{
        padding-right: 6px;
    }
   .dx-toolbar-button .dx-menu .dx-menu-item .dx-menu-item-content{
        border: 1px solid #ddd;
        border-radius: 4px;
        padding: 5px;
    }

</style>

<script id="dxss_inlineCtrScript" data-v="<%=UGITUtility.AssemblyVersion %>">

    var radioGroupItems = [
        { text: "Fully Available", value: 0 },
        { text: "Partially Available", value: 1 },
        { text: "All Resources", value: 2 }
    ];
    var gridDataSource = [];
    var ganttDataSource = [];
    var gridDataSourceData = [];
    var cellData = null;
    var IsCalledOnce = false;
    var popupFilters = {};
    var minDate = "0001-01-01T00:00:00";
    var minStartDate = new Date('<%= ProjectStartDate%>');
    var filename = null;
    var ticketID = '<%= TicketID %>';
    var IsCompactView = '<%= IsCompactView%>';
    var moduleName = '<%= ModuleName %>';
    var projectID = "<%= Request["TicketId"]%>";
    var EditingAllow = false;
    var TaskParameter = 'All Tasks';
    var apiUrl = "/api/module/GetTaskData?ticketID=<%= TicketID %>";
    var apiSummaryUrl = "/api/module/GetSummaryData?ticketID=<%= TicketID %>&BaselineID=0";
    var sExpand = "ParentTaskID";
    var selectionModeChk = "multiple";
    var bAllowAdding = true;
    var bActUser = false;
    var bAddNewTaskDisabled = false;
    var bDeletingAllow = false;
    var bDisableActions = false;
    var currentRowIndex = 0;
    var currentRow;
    var currentTasKIndex = <%= tasKIndex %>;
    var strResourceSelectFilter = '<%=ResourceSelectFilter%>';
    var IsCriticalTask = false;
    var showBaseline = false;
    var allowDrag = false;
    var allowSelection = false;
    var taskIndex;
    var taskTitle;
    var allowAutoAdjustSchedule = false;
    var itemorder = -1;
    var tasklistrowindex = 0;
    var Y = 0;
    var isKanBanViewActive = false;
    var KanBanCategoryIds = [];
    var isCritical = "CriticalOff";
    var viewList = [{ id: 1, name: "List View" }, { id: 2, name: "Gantt View" }, { id: 3, name: "Kanban View" }, { id: 4, name: "Calendar" }];
    var isKanBanViewLoaded = false;
    var collapseImage = "'/content/images/minus-square.png'";
    var expandImage = "'/content/images/plus-square.png'";
    var widthOfSummaryDurationColumn = '120px';
    var IframeHeight = 0;
    var maxHeightOfTaskListGrid = 0;
    var ajaxHelperUrl = '<%=ajaxHelper%>';
    var itemKey = 1;
    var itemKeyView = 1;
    var firstColWidth = "60px";

    $(function () {



        if (TaskParameter != "") {
            TaskParameter = $.cookie("TaskParameter");
            itemKeyView = $.cookie('itemKeyView');
            if (itemKeyView == '' || itemKeyView == null)
                itemKeyView = 1;
            if (navigator.appVersion.indexOf("Mac") != -1) {
                //console.log(navigator.appVersion.indexOf("Mac"), "isMac");
                // widthOfSummaryDurationColumn = '134px';
            }
            if (IsCompactView.toLowerCase() != "true") {
                widthOfSummaryDurationColumn = '120px';
            }
            LoadTaskTreeList(TaskParameter);
        }
    });


    function LoadTaskTreeList(TaskParameter) {

        bActUser = '<%= bActionUser %>';
        showBaseline = '<%= showBaseline%>';

        if (showBaseline.toLowerCase() == "true") {
            $("#projectTeam_linkWrap").hide();
            bAllowAdding = false;
            EditingAllow = false;
            bAddNewTaskDisabled = true;
            bDeletingAllow = false
            selectionModeChk = "none";
            bDisableActions = true;
            allowDrag = false;
            allowSelection = false;
            allowAutoAdjustSchedule = false;
            apiUrl = "/api/module/GetTaskData?ticketID=<%= TicketID %>&BaselineID=<%=BaseLineID%>";
            apiSummaryUrl =  "/api/module/GetSummaryData?ticketID=<%= TicketID %>&BaselineID=<%=BaseLineID%>";
            sExpand = "ParentTaskID";
        }
        else {

            if (TaskParameter == 'My Tasks') {
                EditingAllow = true;
                sExpand = "NoExpansion";
                selectionModeChk = "none";
                bAllowAdding = false;
                bAddNewTaskDisabled = true;
                bDeletingAllow = true;
                bDisableActions = false;
                allowAutoAdjustSchedule = false;
                allowDrag = false;
                allowSelection = true;
                apiUrl = "/api/module/GetTaskDataByUser?ticketID=<%= TicketID %>";
                apiSummaryUrl = "/api/module/GetSummaryData?ticketID=<%= TicketID %>&BaselineID=0";

            }
            else {
                apiUrl = "/api/module/GetTaskData?ticketID=<%= TicketID %>&BaselineID=<%=BaseLineID%>";
                apiSummaryUrl =  "/api/module/GetSummaryData?ticketID=<%= TicketID %>&BaselineID=<%=BaseLineID%>";
                sExpand = "ParentTaskID";
                if (bActUser.toLowerCase() == "false") {
                    bAllowAdding = false;
                    EditingAllow = false;
                    bAddNewTaskDisabled = true;
                    bDeletingAllow = false
                    bDisableActions = true;
                    allowSelection = true;
                    allowDrag = false;
                    selectionModeChk = "none";
                    allowAutoAdjustSchedule = false;

                }
                else {
                    bAllowAdding = true;
                    EditingAllow = true;
                    bAddNewTaskDisabled = false;
                    bDeletingAllow = true;
                    bDisableActions = false;
                    allowSelection = true;
                    allowDrag = true;
                    selectionModeChk = "multiple";
                    allowAutoAdjustSchedule = true;


                }

            }

        }
        var loadPanel = $("#loadpanel").dxLoadPanel({
            shadingColor: "rgba(0,0,0,0.4)",
            position: { of: "#tree-list" },
            visible: false,
            showIndicator: true,
            showPane: true,
            shading: true,
            hideOnOutsideClick: false
        }).dxLoadPanel("instance");
        var getNodeKeys = function (node) {
            var keys = [];
            keys.push(node.key);
            node.children.forEach(function (item) {
                keys = keys.concat(getNodeKeys(item));
            });
            return keys;
        }

        $("#popupContainer").dxPopup({
            visible: false,
            width: 350,
            height: 260,
            contentTemplate: function (contentElement) {
                contentElement.append(
                    $("<p />").text("Do you want to duplicate the Child Task(s) also?"),
                    $("<div />").attr("id", "buttonContainer").dxButton({
                        text: "Yes",
                        onClick: function (e) {
                            var treeL = $("#tree-list").dxTreeList("instance");
                            var selectedRowskeys = treeL.getSelectedRowKeys("all");
                            var copyChild = true;
                            if (selectedRowskeys.length > 0) {
                                $.ajax({
                                    url: ugitConfig.apiBaseUrl + "/api/module/DuplicateTask",
                                    method: "POST",
                                    data: { TaskKeys: selectedRowskeys, copyChild: copyChild, ticketId: '<%= TicketID%>' },
                                    success: function (data) {
                                        treeL.clearSelection();
                                        treeL.refresh();
                                    },
                                    error: function (error) { }
                                })
                                $("#popupContainer").dxPopup("instance").hide();
                            }
                        }
                    }),
                    $("<div />").attr("id", "buttonContainer").dxButton({
                        text: "No",
                        onClick: function (e) {
                            var treeL = $("#tree-list").dxTreeList("instance");
                            var selectedRowsdata = treeL.getSelectedRowsData("all");
                            var parentTaskData = selectedRowsdata.filter(function (item) {
                                return item.ParentTaskID == 0;
                            });
                            var parentTaskKey = parentTaskData.map(x => x.ID);
                            var copyChild = false;
                            if (parentTaskKey.length > 0) {
                                $.ajax({
                                    url: ugitConfig.apiBaseUrl + "/api/module/DuplicateTask",
                                    method: "POST",
                                    data: { TaskKeys: parentTaskKey, copyChild: copyChild, ticketId: '<%= TicketID%>' },
                                    success: function (data) {
                                        treeL.clearSelection();
                                        treeL.refresh();
                                    },
                                    error: function (error) { }
                                });
                                $("#popupContainer").dxPopup("instance").hide();
                            }
                        }
                    }));
            }
        });

        var ChkimportDatesOnly = false;
        var chkDontImportAssignee = false;
        var chkCalculateEstmHrs = false;

        var mppfileuploaderpopup = $("#mppfileuploaderpopup").dxPopup({
            visible: false,
            width: 618,
            height: 200,
            title: "Import Task",
            contentTemplate: function (contentElement) {
                contentElement.append(
                    $("<div />").attr("id", "mppFileUploader").dxFileUploader({
                        selectButtonText: "Upload Project File",
                        labelText: "",
                        uploadMethod: "POST",
                        chunkSize: 200000,
                        width: 300,
                        uploadMode: "instantly",
                        uploadUrl: ugitConfig.apiBaseUrl + "api/pmmapi/UploadFile",
                        allowedFileExtensions: [".mpp", ".xml"],
                        onUploaded: function (e) {
                            if (typeof e.request != "undefined") {
                                if (typeof e.file.name != "undefined") {
                                    filename = e.request.response;

                                    IsCalledOnce = true;

                                }

                            }
                        }
                    }),
                    $("<div >").attr("id", "buttonContainer").dxButton({
                        text: "Import",
                        activeStateEnabled: false,
                        focusStateEnabled: false,
                        hoverStateEnabled: false,
                        onClick: function (e) {

                            var treeL = $("#tree-list").dxTreeList("instance");
                            if (IsCalledOnce) {
                                loadPanel.show();
                                $.ajax({
                                    url: ugitConfig.apiBaseUrl + "/api/module/ImportTasksMPP?ticketID=<%= TicketID %>" + "&filename=" + filename + "&importDatesOnly=" + ChkimportDatesOnly + "&dontImportAssignee=" + chkDontImportAssignee + "&calculateEstHrs=" + chkCalculateEstmHrs,
                                    method: "POST",
                                    success: function (data) {
                                        $("#mppFileUploader").dxFileUploader("instance").reset();
                                        treeL.refresh();
                                        loadPanel.hide();
                                    },
                                    error: function (error) { }
                                })
                                $("#mppfileuploaderpopup").dxPopup("instance").hide();
                            }
                            else {
                                DevExpress.ui.notify({
                                    message: "Please Upload a Project File",
                                    type: "warning",
                                    width: 300,
                                    position: "{ my: 'top', at: 'bottom', of: window }",
                                });
                            }
                        }
                    }),

                    $("<div >").attr("id", "chkImportDates").dxCheckBox({
                        text: "Don't Import Predecessors ",
                        visible: true,
                        onValueChanged: function (e) {
                            ChkimportDatesOnly = e.value;

                        }
                    }),
                    $("<div >").attr("id", "chkDontImportAssignee").dxCheckBox({
                        text: "Don't Import Assignees ",
                        visible: true,
                        onValueChanged: function (e) {
                            chkDontImportAssignee = e.value;
                        }
                    }),
                    $("<div >").attr("id", "chkCalculateEstmHrs").dxCheckBox({
                        text: "Recalculate Est. Hrs. ",
                        visible: true,
                        onValueChanged: function (e) {
                            chkCalculateEstmHrs = e.value;
                        }
                    }),
                );


            }
        });

        var taskList = [{ id: 1, name: "All Tasks" }, { id: 2, name: "My Tasks" }];
        var scaleTypes = [{ id: 1, name: "Days" }, { id: 2, name: "Weeks" }, { id: 3, name: "Months" }, { id: 4, name: "Quarters" }, { id: 5, name: "Years" }];
        var toolbar = $("#toolbar").dxToolbar({
            items: [
                {

                    location: 'after',
                    widget: 'dxDropDownButton',
                    name: 'taskType',
                    options: {
                        displayExpr: "name",
                        keyExpr: "id",
                        selectedItemKey: itemKey,
                        width: 100,
                        stylingMode: "text",
                        useSelectMode: true,
                        visible: moduleName == "NPR" ? false : true,
                        onItemClick: function (e) {
                            TaskParameter = e.itemData.name;
                            itemKey = e.itemData.id;
                            LoadTaskTreeList(TaskParameter);
                        },

                        items: taskList
                    }
                },
                {
                    location: 'after',
                    widget: 'dxDropDownButton',
                    name: 'viewDropDown',
                    cssClass: "toolbar-view-dropDown",
                    options: {
                        displayExpr: "name",
                        keyExpr: "id",
                        selectedItemKey: itemKeyView,
                        width: 135,
                        stylingMode: "text",
                        useSelectMode: true,
                        visible: moduleName == "NPR" ? false : true,
                        items: viewList,
                        onItemClick: function (e) {
                            TaskParameter = e.itemData.name;
                            itemKeyView = e.itemData.id;
                            $.cookie('TaskParameter', TaskParameter);
                            $.cookie('itemKeyView', itemKeyView);

                            if (TaskParameter == "Gantt View") {
                                loadGanttView();
                                $("#treelist").hide();
                                $("#gantt").show();
                                $("#kanbanView").hide();
                                $('.monitorpanel').show();
                                var ganttinstance = viewList[1].dxComponent;
                                isKanBanViewActive = false;
                                toolbar._options._optionManager._options.items.find(x => x.name == 'taskType') ? toolbar._options._optionManager._options.items.find(x => x.name == 'taskType').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'expandAll') ? toolbar._options._optionManager._options.items.find(x => x.name == 'expandAll').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'collapseAll') ? toolbar._options._optionManager._options.items.find(x => x.name == 'collapseAll').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'criticalTask') ? toolbar._options._optionManager._options.items.find(x => x.name == 'criticalTask').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'resourceUsage') ? toolbar._options._optionManager._options.items.find(x => x.name == 'resourceUsage').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'autoAdjustSchedule') ? toolbar._options._optionManager._options.items.find(x => x.name == 'autoAdjustSchedule').visible = false : '';

                                toolbar._options._optionManager._options.items.find(x => x.name == 'markAsComplete') ? toolbar._options._optionManager._options.items.find(x => x.name == 'markAsComplete').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'duplicateTask') ? toolbar._options._optionManager._options.items.find(x => x.name == 'duplicateTask').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'decreaseIndent') ? toolbar._options._optionManager._options.items.find(x => x.name == 'decreaseIndent').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'increaseIndent') ? toolbar._options._optionManager._options.items.find(x => x.name == 'increaseIndent').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'deleteSelectedTask') ? toolbar._options._optionManager._options.items.find(x => x.name == 'deleteSelectedTask').visible = false : '';

                                toolbar._options._optionManager._options.items.find(x => x.name == 'saveAsTemplate') ? toolbar._options._optionManager._options.items.find(x => x.name == 'saveAsTemplate').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'exportTasksFromMPP') ? toolbar._options._optionManager._options.items.find(x => x.name == 'exportTasksFromMPP').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'importTasksFromMPP') ? toolbar._options._optionManager._options.items.find(x => x.name == 'importTasksFromMPP').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'importTasksfromTemplate') ? toolbar._options._optionManager._options.items.find(x => x.name == 'importTasksfromTemplate').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'deleteAll') ? toolbar._options._optionManager._options.items.find(x => x.name == 'deleteAll').visible = false : '';

                                toolbar._options._optionManager._options.items.find(x => x.name == "scaleDropDown") ? toolbar._options._optionManager._options.items.find(x => x.name == 'scaleDropDown').visible = true : '';

                                toolbar._getToolbarItems().filter(x => x.name == 'viewDropDown')[0].options.selectedItemKey = e.itemData.id;
                                toolbar._options._optionManager._options.items.find(x => x.name == 'moveup') ? toolbar._options._optionManager._options.items.find(x => x.name == 'moveup').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'movedown') ? toolbar._options._optionManager._options.items.find(x => x.name == 'movedown').visible = false : '';
                                toolbar._options._optionManager._options.items[21].visible = false;

                                toolbar.repaint();

                                ganttinstance.repaint();

                            }
                            else if (TaskParameter == "List View") {
                                loadListView();
                                $("#treelist").show();
                                $("#gantt").hide();
                                $("#kanbanView").hide();
                                $('.monitorpanel').show();
                                var treeL = $("#tree-list").dxTreeList("instance");
                                treeL.refresh();
                                isKanBanViewActive = false;
                                toolbar._options._optionManager._options.items.find(x => x.name == 'taskType') ? toolbar._options._optionManager._options.items.find(x => x.name == 'taskType').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'expandAll') ? toolbar._options._optionManager._options.items.find(x => x.name == 'expandAll').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'collapseAll') ? toolbar._options._optionManager._options.items.find(x => x.name == 'collapseAll').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'criticalTask') ? toolbar._options._optionManager._options.items.find(x => x.name == 'criticalTask').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'resourceUsage') ? toolbar._options._optionManager._options.items.find(x => x.name == 'resourceUsage').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'autoAdjustSchedule') ? toolbar._options._optionManager._options.items.find(x => x.name == 'autoAdjustSchedule').visible = true : '';

                                toolbar._options._optionManager._options.items.find(x => x.name == 'markAsComplete') ? toolbar._options._optionManager._options.items.find(x => x.name == 'markAsComplete').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'duplicateTask') ? toolbar._options._optionManager._options.items.find(x => x.name == 'duplicateTask').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'decreaseIndent') ? toolbar._options._optionManager._options.items.find(x => x.name == 'decreaseIndent').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'increaseIndent') ? toolbar._options._optionManager._options.items.find(x => x.name == 'increaseIndent').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'deleteSelectedTask') ? toolbar._options._optionManager._options.items.find(x => x.name == 'deleteSelectedTask').visible = true : '';

                                toolbar._options._optionManager._options.items.find(x => x.name == 'saveAsTemplate') ? toolbar._options._optionManager._options.items.find(x => x.name == 'saveAsTemplate').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'exportTasksFromMPP') ? toolbar._options._optionManager._options.items.find(x => x.name == 'exportTasksFromMPP').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'importTasksFromMPP') ? toolbar._options._optionManager._options.items.find(x => x.name == 'importTasksFromMPP').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'importTasksfromTemplate') ? toolbar._options._optionManager._options.items.find(x => x.name == 'importTasksfromTemplate').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'deleteAll') ? toolbar._options._optionManager._options.items.find(x => x.name == 'deleteAll').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'moveup') ? toolbar._options._optionManager._options.items.find(x => x.name == 'moveup').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'movedown') ? toolbar._options._optionManager._options.items.find(x => x.name == 'movedown').visible = true : '';
                                toolbar._options._optionManager._options.items[21].visible = true;

                                //disable
                                toolbar._options._optionManager._options.items.find(x => x.name == 'markAsComplete') ? toolbar._options._optionManager._options.items.find(x => x.name == 'markAsComplete').disabled = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'duplicateTask') ? toolbar._options._optionManager._options.items.find(x => x.name == 'duplicateTask').disabled = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'decreaseIndent') ? toolbar._options._optionManager._options.items.find(x => x.name == 'decreaseIndent').disabled = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'increaseIndent') ? toolbar._options._optionManager._options.items.find(x => x.name == 'increaseIndent').disabled = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'deleteSelectedTask') ? toolbar._options._optionManager._options.items.find(x => x.name == 'deleteSelectedTask').disabled = true : '';

                                toolbar._options._optionManager._options.items.find(x => x.name == "scaleDropDown") ? toolbar._options._optionManager._options.items.find(x => x.name == 'scaleDropDown').visible = false : '';

                                toolbar._getToolbarItems().filter(x => x.name == 'viewDropDown')[0].options.selectedItemKey = e.itemData.id;
                                toolbar._options._optionManager._options.items.find(x => x.name == 'moveup') ? toolbar._options._optionManager._options.items.find(x => x.name == 'moveup').disabled = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'movedown') ? toolbar._options._optionManager._options.items.find(x => x.name == 'movedown').disabled = true : '';
                                toolbar._options._optionManager._options.items[21].options.items[0].items[1].disabled = true;
                                toolbar._options._optionManager._options.items[21].options.items[0].items[2].disabled = true;
                                document.getElementById("btn-Preview-Image").style.display = "none";

                                toolbar.repaint();

                            }
                            else if (TaskParameter == "Calendar") {
                                //Set the dropdown value to List view for the next time. 
                                //Because the dropdown value had to be clicked again to open the calendar page.
                                TaskParameter = 'List View';
                                itemKeyView = 1;
                                $.cookie('TaskParameter', TaskParameter);
                                $.cookie('itemKeyView', itemKeyView);

                                var ModuleName = '<%=ModuleName%>';
                                var projectPublicId = '<%=TicketID %>';
                                var calendarURL = '<%=calendarURL %>';
                                var url = calendarURL + "?control=taskcalender&moduleName=" + ModuleName + "&ProjectPublicId=" + projectPublicId + "&minStartDate=" + minStartDate;
                                window.parent.UgitOpenPopupDialog(url, '', 'Calendar Task View', '870px', '770px', 0, "<%=Server.UrlEncode(Request.Url.AbsoluteUri) %>");
                            }
                            else {

                                RepaintTaskWorkflowKanBan();
                                loadPanel.show();
                                isKanBanViewActive = true;
                                $("#treelist").hide();
                                $("#gantt").hide();
                                $("#kanbanView").show();
                                $('.monitorpanel').hide();

                                toolbar._options._optionManager._options.items.find(x => x.name == 'taskType') ? toolbar._options._optionManager._options.items.find(x => x.name == 'taskType').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'expandAll') ? toolbar._options._optionManager._options.items.find(x => x.name == 'expandAll').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'collapseAll') ? toolbar._options._optionManager._options.items.find(x => x.name == 'collapseAll').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'criticalTask') ? toolbar._options._optionManager._options.items.find(x => x.name == 'criticalTask').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'resourceUsage') ? toolbar._options._optionManager._options.items.find(x => x.name == 'resourceUsage').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'autoAdjustSchedule') ? toolbar._options._optionManager._options.items.find(x => x.name == 'autoAdjustSchedule').visible = true : '';

                                toolbar._options._optionManager._options.items.find(x => x.name == 'markAsComplete') ? toolbar._options._optionManager._options.items.find(x => x.name == 'markAsComplete').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'duplicateTask') ? toolbar._options._optionManager._options.items.find(x => x.name == 'duplicateTask').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'decreaseIndent') ? toolbar._options._optionManager._options.items.find(x => x.name == 'decreaseIndent').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'increaseIndent') ? toolbar._options._optionManager._options.items.find(x => x.name == 'increaseIndent').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'deleteSelectedTask') ? toolbar._options._optionManager._options.items.find(x => x.name == 'deleteSelectedTask').visible = false : '';

                                toolbar._options._optionManager._options.items.find(x => x.name == 'saveAsTemplate') ? toolbar._options._optionManager._options.items.find(x => x.name == 'saveAsTemplate').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'exportTasksFromMPP') ? toolbar._options._optionManager._options.items.find(x => x.name == 'exportTasksFromMPP').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'importTasksFromMPP') ? toolbar._options._optionManager._options.items.find(x => x.name == 'importTasksFromMPP').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'importTasksfromTemplate') ? toolbar._options._optionManager._options.items.find(x => x.name == 'importTasksfromTemplate').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'deleteAll') ? toolbar._options._optionManager._options.items.find(x => x.name == 'deleteAll').visible = true : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'moveup') ? toolbar._options._optionManager._options.items.find(x => x.name == 'moveup').visible = false : '';
                                toolbar._options._optionManager._options.items.find(x => x.name == 'movedown') ? toolbar._options._optionManager._options.items.find(x => x.name == 'movedown').visible = false : '';
                                toolbar._options._optionManager._options.items[21].visible = false;
                                toolbar._options._optionManager._options.items.find(x => x.name == "scaleDropDown") ? toolbar._options._optionManager._options.items.find(x => x.name == 'scaleDropDown').visible = false : '';

                                toolbar._getToolbarItems().filter(x => x.name == 'viewDropDown')[0].options.selectedItemKey = e.itemData.id;
                                toolbar.repaint();

                                loadPanel.hide();
                                document.getElementById("btn-Preview-Image").style.display = "none";

                            }

                        }

                    }
                }, {
                    location: 'after',
                    widget: 'dxMenu',
                    name: 'imgExportActionCompact',
                    options: {
                        hint: "Reports",
                        items: [
                            {
                                icon: "/Content/images/Reports.svg",
                                items: [
                                    {
                                        icon: "/Content/images/Active-Projects.png",
                                        text: "Project Report",
                                        name: "ProjectStatusReport"
                                    },
                                    {
                                        icon: "/Content/images/Active-Projects.png",
                                        text: "1-Pager Report",
                                        name: "OnePagerReport"
                                    },
                                    {
                                        icon: "/Content/images/Active-Projects.png",
                                        text: "Resource Hours",
                                        name: "ProjectResourceReport"
                                    },
                                    {
                                        icon: "/Content/images/Active-Projects.png",
                                        text: "ERH Report",
                                        name: "EstimatedRemainingHoursReport"
                                    },
                                    {
                                        icon: "/Content/images/Active-Projects.png",
                                        text: "Budget Report",
                                        name: "ProjectBudgetReport"
                                    },
                                    {
                                        icon: "/Content/images/Active-Projects.png",
                                        text: "Actuals Report",
                                        name: "ActualsReport",
                                        visible: false
                                    },
                                    {
                                        icon: "/Content/images/Active-Projects.png",
                                        text: "Project stage history",
                                        name: "ProjectStageHistory"
                                    },
                                    {
                                        icon: "/Content/images/Active-Projects.png",
                                        text: "Project Actuals Report",
                                        name: "ProjectActualsReport"
                                    }
                                ]
                            }
                        ],
                        onItemClick: onReportItemClick
                    }
                },
                {
                    location: 'before',
                    widget: 'dxButton',
                    name: 'expandAll',
                    options: {
                        icon: "/Content/images/expandNew.png",
                        hint: 'Expand-All',
                        onClick: function (e) {
                            var treeL = $("#tree-list").dxTreeList("instance");
                            var keys = getNodeKeys(treeL.getRootNode());
                            treeL.beginUpdate();
                            keys.forEach(function (key) {
                                treeL.expandRow(`${key}`);
                            });
                            treeL.endUpdate();
                            if (isKanBanViewActive) {
                                KanBanCategoryIds.forEach(function (kanBabCategoryId) {
                                    var KanbanTree = $("#" + kanBabCategoryId).dxTreeList("instance");
                                    var keys = getNodeKeys(KanbanTree.getRootNode());
                                    KanbanTree.beginUpdate();
                                    keys.forEach(function (key) {
                                        KanbanTree.expandRow(key);
                                    });
                                    KanbanTree.endUpdate();
                                });
                            }
                        }
                    }
                },
                {
                    location: 'before',
                    widget: 'dxButton',
                    name: 'collapseAll',
                    locateInMenu: 'auto',
                    options: {
                        icon: "/Content/images/collapseNew.png",
                        hint: 'Collapse-All',
                        onClick: function (e) {
                            var treeL = $("#tree-list").dxTreeList("instance");
                            var keys = getNodeKeys(treeL.getRootNode());
                            treeL.beginUpdate();
                            keys.forEach(function (key) {
                                treeL.collapseRow(`${key}`);
                            });
                            treeL.endUpdate();
                            if (isKanBanViewActive) {
                                KanBanCategoryIds.forEach(function (kanBabCategoryId) {
                                    var KanbanTree = $("#" + kanBabCategoryId).dxTreeList("instance");
                                    var keys = getNodeKeys(KanbanTree.getRootNode());
                                    KanbanTree.beginUpdate();
                                    keys.forEach(function (key) {
                                        KanbanTree.collapseRow(key);
                                    });
                                    KanbanTree.endUpdate();
                                });
                            }
                        }
                    }
                },
                {
                    location: 'before',
                    widget: 'dxButton',
                    locateInMenu: 'auto',
                    visible: false,
                    options: {
                        icon: "/Content/images/calendarNew.png",
                        hint: 'Calendar View',
                        onClick: function (e) {
                            var ModuleName = '<%=ModuleName%>';
                            var projectPublicId = '<%=TicketID %>';  /* PMM Ticket Id */
                            var calendarURL = '<%=calendarURL %>';
                            var url = calendarURL + "?control=taskcalender&moduleName=" + ModuleName + "&ProjectPublicId=" + projectPublicId;
                            window.parent.UgitOpenPopupDialog(url, '', 'Calendar Task View', '850px', '640px', 0, '');
                            return false;
                        }
                    }
                },
                {
                    name: 'criticalTask',
                    location: 'before',
                    widget: 'dxButton',
                    locateInMenu: 'auto',
                    options: {
                        icon: "/Content/images/criticalInactiveNew.png",
                        hint: 'Critical Task',
                        onClick: function (e) {
                            if (!IsCriticalTask) {
                                IsCriticalTask = true;
                            }
                            else {
                                IsCriticalTask = false;
                            }
                            var treeL = $("#tree-list").dxTreeList("instance");
                            var modifiedSource = treeL.getVisibleRows();
                            var criticalTask = _.filter(modifiedSource[0].data, function (current) { return current.data.isCritical == true; });
                            if (criticalTask.length > 0) {
                                criticalTask.forEach(function (e) {
                                    var element = treeL.getRowElement(treeL.getRowIndexByKey(criticalTask.key));
                                    if (IsCriticalTask) {
                                        //element[0].style.backgroundColor = "#ffc1a4";
                                        element[0].style.border = "2px solid #ffc1a4";
                                    }
                                    else {
                                        //element[0].style.backgroundColor = "";
                                        element[0].style.border = "";
                                    }
                                });
                            }
                            if (isKanBanViewActive) {
                                isCritical = "CriticalOn";
                                RepaintTaskWorkflowKanBan();
                            }
                        }
                    }
                },
                {
                    location: 'before',
                    widget: 'dxButton',
                    name: 'resourceUsage',
                    options: {
                        icon: "/Content/Images/People16X16.png", //15
                        hint: 'Resource Usage',
                        onClick: function (e) {
                            var moduleName = "<%= ModuleName %>";
                            var popupHeader = "Resource Usage Report";
                            var params = "";
                            var url = '<%= reportUrl %>' + "?reportName=ResourceUsage" + "&Module=" + moduleName + '&userId=<%= userID %>' + '&TicketPublicId=' + '<%= TicketID%>';
                            window.parent.UgitOpenPopupDialog(url, params, popupHeader, '850px', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
                        }
                    }
                },
                {
                    location: 'before',
                    widget: 'dxButton',
                    locateInMenu: 'auto',
                    name: 'autoAdjustSchedule',
                    visible: allowAutoAdjustSchedule,
                    options: {
                        icon: "/Content/images/autoAdjustSchedule.png",
                        hint: 'Auto Adjust Schedule',
                        onClick: function (e) {
                            $("#gantt").dxGantt("dispose");
                            if (!isKanBanViewActive)
                                loadPanel.show();


                            $.ajax({
                                url: ugitConfig.apiBaseUrl + "/api/module/AutoAdjustSchedule?ticketID=<%= TicketID %>",
                                method: "POST",
                                success: function (data) {
                                    if (isKanBanViewActive) {
                                        RepaintTaskWorkflowKanBan();
                                    }
                                    else {
                                        LoadTaskTreeList(TaskParameter);
                                        loadPanel.hide();
                                    }

                                },
                                error: function (error) { }
                            })
                            if (isKanBanViewActive) {
                                RepaintTaskWorkflowKanBan();
                            }
                        }
                    }
                },
                {
                    location: 'before',
                    widget: 'dxButton',
                    locateInMenu: 'auto',
                    disabled: true,
                    name: 'markAsComplete',
                    cssClass: "toolbar-accept-symbol",
                    options: {
                        icon: "/Content/images/accept-symbol.png",
                        hint: 'Mark Task As Complete',
                        onClick: function (e) {
                            var treeL = $("#tree-list").dxTreeList("instance");
                            var selectedRowskeys = treeL.getSelectedRowKeys("all");
                            if (selectedRowskeys.length > 0) {
                                loadPanel.show();
                                $.ajax({
                                    url: ugitConfig.apiBaseUrl + "/api/module/MarkTaskAsComplete",
                                    method: "POST",
                                    data: { TaskKeys: selectedRowskeys, TicketPublicId: '<%= TicketID%>', TaskType: "ModuleTasks" },
                                    success: function (data) {
                                        treeL.clearSelection();

                                        treeL.refresh();
                                        loadPanel.hide();
                                    },
                                    error: function (error) { }
                                })
                            }
                        }
                    }
                },
                {
                    location: 'before',
                    widget: 'dxButton',
                    locateInMenu: 'auto',
                    // disabled: bDisableActions,
                    options: {
                        icon: "/Content/images/GanttChartNew.png",
                        hint: 'Change View',
                        visible: false,
                        onClick: function (e) {
                            if (IsCalledOnce) {
                                $("#treelist").show();
                                $("#gantt").hide();
                                var treeL = $("#tree-list").dxTreeList("instance");
                                toolbar._options._optionManager._options.items[0].disabled = false;
                                toolbar._options._optionManager._options.items[1].disabled = false;
                                toolbar._options._optionManager._options.items[5].disabled = false;
                                toolbar._options._optionManager._options.items[6].disabled = false;
                                toolbar._options._optionManager._options.items[7].disabled = false;
                                toolbar._options._optionManager._options.items[8].disabled = false;
                                toolbar._options._optionManager._options.items[9].disabled = false;
                                toolbar._options._optionManager._options.items[10].disabled = false;
                                toolbar._options._optionManager._options.items[11].disabled = false;
                                toolbar._options._optionManager._options.items[12].disabled = false;
                                toolbar._options._optionManager._options.items[15].disabled = false;
                                toolbar._options._optionManager._options.items[16].disabled = false;
                                toolbar._options._optionManager._options.items[21].options.items[0].items[1].disabled = false;
                                toolbar._options._optionManager._options.items[21].options.items[0].items[2].disabled = false;
                                toolbar.repaint();
                                treeL.repaint();
                                // IsCalledOnce = false;
                            }
                            else {
                                var ganttinstance = $("#gantt").dxGantt('instance');

                                $("#treelist").hide();
                                $("#gantt").show();
                                toolbar._options._optionManager._options.items[0].disabled = true;
                                toolbar._options._optionManager._options.items[1].disabled = true;
                                toolbar._options._optionManager._options.items[2].disabled = true;
                                //toolbar._options._optionManager._options.items[4].disabled = true;
                                toolbar._options._optionManager._options.items[5].disabled = true;
                                toolbar._options._optionManager._options.items[6].disabled = true;
                                toolbar._options._optionManager._options.items[7].disabled = true;
                                toolbar._options._optionManager._options.items[8].disabled = true;
                                toolbar._options._optionManager._options.items[9].disabled = true;
                                toolbar._options._optionManager._options.items[10].disabled = true;
                                toolbar._options._optionManager._options.items[11].disabled = true;
                                toolbar._options._optionManager._options.items[12].disabled = true;
                                toolbar._options._optionManager._options.items[13].disabled = true;
                                toolbar._options._optionManager._options.items[14].disabled = true;
                                toolbar._options._optionManager._options.items[15].disabled = true;
                                toolbar._options._optionManager._options.items[16].disabled = true;
                                toolbar._options._optionManager._options.items[17].disabled = true;
                                toolbar._options._optionManager._options.items[21].options.items[0].items[1].disabled = true;
                                toolbar._options._optionManager._options.items[21].options.items[0].items[2].disabled = true;


                                toolbar.repaint();
                                ganttinstance.repaint();
                                IsCalledOnce = true;
                            }
                        }
                    }
                },
                {
                    location: 'before',
                    widget: 'dxButton',
                    locateInMenu: 'auto',
                    name: 'duplicateTask',
                    disabled: true,
                    options: {
                        icon: "/Content/images/duplicateNew.png",
                        hint: 'Duplicate Task',
                        onClick: function (e) {
                            var treeL = $("#tree-list").dxTreeList("instance");
                            var selectedRowsdata = treeL.getSelectedRowsData();
                            var selectedRowskeys = treeL.getSelectedRowKeys("all");
                            var checkChildcount = _.find(selectedRowsdata, function (current) { return current.ChildCount > 0; });
                            if (typeof checkChildcount != "undefined") {
                                $("#popupContainer").dxPopup("instance").option("visible", true);
                            }
                            else {
                                var copyChild = false;
                                if (selectedRowskeys.length > 0) {
                                    loadPanel.show();
                                    $.ajax({
                                        url: ugitConfig.apiBaseUrl + "/api/module/DuplicateTask",
                                        method: "POST",
                                        data: { TaskKeys: selectedRowskeys, copyChild: copyChild, ticketId: '<%= TicketID%>' },
                                        success: function (data) {
                                            treeL.refresh();
                                            treeL.clearSelection();
                                            loadPanel.hide();
                                        },
                                        error: function (error) { }
                                    })
                                }
                            }
                            return;
                        }
                    }
                }, {
                    locateInMenu: 'always',
                    widget: 'dxButton',
                    //cssClass: "toolbar-list-item",
                    name: "deleteAll",
                    options: {
                        icon: "/Content/images/grayDelete.png",
                        hint: 'Delete All Tasks',
                        text: 'Delete All Tasks',
                        onClick: function () {
                            var r = confirm("Would you like to delete this ??");
                            if (r == true) {
                                var treeL = $("#tree-list").dxTreeList("instance");
                                var selectedRowskeys = treeL.getSelectedRowKeys("all");
                                if (selectedRowskeys.length > 0) {
                                    loadPanel.show();
                                    $.ajax({
                                        url: ugitConfig.apiBaseUrl + "/api/module/DeleteSelectedTask",
                                        method: "DELETE",
                                        data: { TaskKeys: selectedRowskeys, TicketPublicId: '<%= TicketID%>' },
                                        success: function (data) {
                                            LoadTaskTreeList(TaskParameter);
                                            treeL.refresh(true);
                                            treeL.clearSelection();

                                            loadPanel.hide();
                                        },
                                        error: function (error) { }
                                    })
                                }

                            } else {
                                alert("You pressed Cancel!");
                            }

                        }
                    }

                },
                {
                    name: 'saveAsTemplate',
                    locateInMenu: 'always',
                    icon: "/Content/images/saveAsTemplate.png",
                    widget: 'dxButton',

                    cssClass: "toolbar-list-item",
                    options: {
                        hint: 'Save as Template',
                        text: 'Save as Template',
                        icon: "/Content/images/saveAsTemplate.png",
                        onClick: function () {
                            openSaveAsTemplate(ticketID, moduleName);
                        }
                    }

                },
                {
                    name: "importTasksfromTemplate",
                    locateInMenu: 'always',
                    widget: 'dxButton',
                    cssClass: "toolbar-list-item",
                    options: {
                        icon: "/Content/images/importTasks.png",
                        text: 'Import Tasks from Template',
                        onClick: function () {
                            openImportTemplate(ticketID, moduleName);
                        }
                    }
                },
                {
                    name: 'importTasksFromMPP',
                    locateInMenu: 'always',
                    widget: 'dxButton',
                    cssClass: "toolbar-list-item",
                    options: {
                        icon: "/Content/images/importTasks.png",
                        text: 'Import Tasks from MPP',
                        onClick: function () {
                            $("#mppfileuploaderpopup").dxPopup("instance").option("visible", true);
                        }
                    }

                },
                {
                    name: 'exportTasksFromMPP',
                    locateInMenu: 'always',
                    widget: 'dxButton',
                    cssClass: "toolbar-list-item",
                    options: {
                        icon: "/Content/images/exportTasks.png",
                        text: 'Export Tasks to MPP',
                        onClick: function () {
                            $.ajax({
                                url: ugitConfig.apiBaseUrl + "/api/module/ExportTasksMPP?ticketID=<%= TicketID %>",
                                method: "GET",
                                success: function (data) {
                                    if (data != null) {
                                        const a = document.createElement('a');
                                        a.style.display = 'none';
                                        a.href = data.fullFilePath;
                                        // the filename you want
                                        a.download = data.fileName;
                                        document.body.appendChild(a);
                                        a.click();
                                        window.URL.revokeObjectURL(data.fullFilePath);
                                    }
                                },
                                error: function (error) { }
                            })
                        }
                    }
                },
                {
                    location: 'before',
                    widget: 'dxButton',
                    locateInMenu: 'auto',
                    name: "decreaseIndent",
                    disabled: true,
                    options: {
                        icon: "/Content/images/d-indent.png",
                        hint: 'Decrease Indent', //13
                        onClick: function (e) {
                            var treeL = $("#tree-list").dxTreeList("instance");
                            var selectedRowskeys = treeL.getSelectedRowKeys("all");
                            if (selectedRowskeys.length > 0) {
                                loadPanel.show();
                                $.ajax({
                                    url: ugitConfig.apiBaseUrl + "/api/module/DecreaseIndent",
                                    method: "POST",
                                    data: { TaskKeys: selectedRowskeys, TicketPublicId: '<%= TicketID%>' },
                                    success: function (data) {
                                        treeL.clearSelection();
                                        treeL.refresh();
                                        loadPanel.hide();
                                    },
                                    error: function (error) { }
                                })
                            }
                        }
                    }
                },
                {
                    location: 'before',
                    widget: 'dxButton',
                    locateInMenu: 'auto',
                    disabled: true,
                    name: 'increaseIndent',
                    options: {
                        icon: "/Content/images/i-indent.png", 
                        hint: 'Increase Indent',
                        onClick: function (e) {
                            var treeL = $("#tree-list").dxTreeList("instance");
                            var saveThisData = false;
                            var selectedRowskeys = treeL.getSelectedRowKeys("all"); 
                            if (selectedRowskeys.length > 0) {
                                $.each(selectedRowskeys, function (key, value) {
                                    if (value < 0)
                                        saveThisData = true;
                                });
                                if (saveThisData == true) {
                                    treeL.saveEditData();
                                }
                                loadPanel.show();
                                $.ajax({
                                    url: ugitConfig.apiBaseUrl + "/api/module/IncreaseIndent",
                                    method: "POST",
                                    data: { TaskKeys: selectedRowskeys, TicketPublicId: '<%= TicketID%>' },
                                    success: function (data) {
                                        treeL.clearSelection();
                                        treeL.refresh();
                                        loadPanel.hide();
                                    },
                                    error: function (error) { }
                                })
                            }
                        }
                    }
                },
                {
                    location: 'before',
                    widget: 'dxButton',
                    locateInMenu: 'auto',
                    disabled: true,
                    name: "deleteSelectedTask",
                    options: {
                        icon: "/Content/images/grayDelete.png", //17
                        hint: 'Delete Selected Task',
                        onClick: function (e) {
                            var treeL = $("#tree-list").dxTreeList("instance");
                            var selectedRowskeys = treeL.getSelectedRowKeys("all");
                            var childData = '';
                            //let taskCount = 0;
                            var totalKeys = [];
                            //var parentCnt = 0;
                            var deleteMsg = "";
                            totalKeys = totalKeys.concat(selectedRowskeys);
                            var isChildExist = false;
                            if (selectedRowskeys.length > 0) {
                                selectedRowskeys.forEach(function (key) {
                                    //var currentObj = gridDataSourceData.filter(function (task) { return task.ID == key; });
                                    //if (currentObj[0].ParentTaskID == 0) {
                                    //    parentCnt++;
                                    //}
                                    childData = gridDataSourceData.filter(function (task) { return task.ParentTaskID == key; });

                                    childData.forEach(function (childKey) {
                                        if (totalKeys.indexOf(childKey.ID) == -1) {
                                            totalKeys.push(childKey.ID);
                                            isChildExist = true;
                                        }
                                    });

                                    //taskCount = taskCount + childData.length + 1; //add count of child tasks + 1 parent task.
                                });
                            }
                            if (isChildExist) {
                                deleteMsg = "Deletion of parent tasks will delete all child tasks/all subchild tasks, would you like to continue?";
                            }
                            else {
                                deleteMsg = "Are you sure you want to delete " + totalKeys.length + " task(s)?";
                            }
                            var r = confirm(deleteMsg);
                            if (r == true) {
                                if (selectedRowskeys.length > 0) {
                                    loadPanel.show();
                                    $.ajax({
                                        url: ugitConfig.apiBaseUrl + "/api/module/DeleteSelectedTask",
                                        method: "DELETE",
                                        data: { TaskKeys: selectedRowskeys, TicketPublicId: '<%= TicketID%>' },
                                        success: function (data) {
                                            LoadTaskTreeList(TaskParameter);
                                            treeL.refresh();
                                            treeL.clearSelection();
                                            loadPanel.hide();
                                        },
                                        error: function (error) { }
                                    })
                                }
                            } 

                        }
                    }
                },
                {
                    location: 'before',
                    widget: 'dxDropDownButton',
                    name: 'scaleDropDown',
                    cssClass: "toolbar-view-dropDown",
                    visible: false,
                    options: {
                        displayExpr: "name",
                        keyExpr: "id",
                        useSelectMode: true,
                        selectedItemKey: 3,
                        items: scaleTypes,
                        width: 120,
                        onSelectionChanged: function (e) {
                            var ganttchart = $("#gantt").dxGantt("instance");
                            
                            if (e.item.name == "Days")
                                ganttchart.option('scaleType', 'days');
                            if (e.item.name == "Weeks")
                                ganttchart.option('scaleType', 'weeks');
                            if (e.item.name == "Months")
                                ganttchart.option('scaleType', 'months');
                            if (e.item.name == "Quarters")
                                ganttchart.option('scaleType', 'quarters');
                            if (e.item.name == "Years")
                                ganttchart.option('scaleType', 'years');
                        }
                    }
                }
                ,
                {
                    location: 'before',
                    widget: 'dxMenu',
                    name: 'NewTaskOptions',
                    options: {
                        hint: "Add New Task",
                        items: [
                            {
                                icon: "/Content/images/plus-cicle.png",
                                items: [
                                    {
                                        icon: "/Content/images/Active-Projects.png",
                                        text: "New Task At End",
                                        name: "NewTaskAtEnd"
                                    },
                                    {
                                        icon: "/Content/images/Active-Projects.png",
                                        text: "New Task Below Current",
                                        name: "NewTaskBelowCurrent",
                                        disabled: true
                                    },
                                    {
                                        icon: "/Content/images/Active-Projects.png",
                                        text: "New Sub Task",
                                        name: "NewSubTask",
                                        disabled: true
                                   },
                                    {
                                        icon: "/Content/images/Active-Projects.png",
                                        text: "New Recurrring Task",
                                        name: "NewRecurrringTask"
                                    },
                                ]
                            }
                        ],
                        onItemClick: onNewTaskClick
                    }
                }
                ,
                {
                    location: 'before',
                    widget: 'dxButton',
                    locateInMenu: 'auto',
                    name: "moveup",
                    disabled: true,
                    options: {
                        icon: "/Content/images/move-up.png",
                        hint: 'Move Up', //22
                        onClick: function (e)
                        {
                            var treeL = $("#tree-list").dxTreeList("instance");
                            var selectedRowskeys = treeL.getSelectedRowKeys("all");
                            let currentRowIndex = -1;
                            if (selectedRowskeys.length > 0) 
                            {
                                currentRowIndex = treeL.getRowIndexByKey(selectedRowskeys[0]);
                            }
                            if (currentRowIndex > 0)
                            { 
                                var visibleRowskeys = treeL.getVisibleRows("all");
                                var toKey = visibleRowskeys[currentRowIndex].key;
                                loadPanel.show();
                                $.ajax({
                                    url: ugitConfig.apiBaseUrl + "/api/module/DragAndDrop?TicketPublicId=<%= TicketID %>" + "&toKey=" + selectedRowskeys[0] + "&fromKey=" + visibleRowskeys[currentRowIndex - 1].key,
                                    method: "POST",
                                    success: function (data) {
                                        var treeL = $("#tree-list").dxTreeList("instance");
                                        treeL.refresh();
                                        loadPanel.hide();
                                    },
                                    error: function (error) { }
                                })
                            }
                        }
                    }
                }
                ,
                {
                    location: 'before',
                    widget: 'dxButton',
                    locateInMenu: 'auto',
                    name: "movedown",
                    disabled: true,
                    options: {
                        icon: "/Content/images/move-down.png",
                        hint: 'Move Down', //22
                        onClick: function (e) {
                            var treeL = $("#tree-list").dxTreeList("instance");
                            var selectedRowskeys = treeL.getSelectedRowKeys("all");
                            let currentRowIndex = -1;
                            if (selectedRowskeys.length > 0) {
                                currentRowIndex = treeL.getRowIndexByKey(selectedRowskeys[0]);
                            }
                            var visibleRowskeys = treeL.getVisibleRows("all");
                            if (currentRowIndex < visibleRowskeys.length-1) {
                                var toKey = visibleRowskeys[currentRowIndex].key;
                                loadPanel.show();
                                $.ajax({
                                    url: ugitConfig.apiBaseUrl + "/api/module/DragAndDrop?TicketPublicId=<%= TicketID %>" + "&toKey=" + visibleRowskeys[currentRowIndex + 1].key + "&fromKey=" + selectedRowskeys[0],
                                    method: "POST",
                                    success: function (data) {
                                        var treeL = $("#tree-list").dxTreeList("instance");
                                        treeL.refresh();
                                        loadPanel.hide();
                                    },
                                    error: function (error) { }
                                })
                            }
                        }
                    }
                }
                               
            ]
        }).dxToolbar("instance");

        if (TaskParameter != "Gantt View") {
            loadListView()
        }









        $("#btnSave").dxButton({
            text: "Save Changes",
            icon: "/content/Images/save-open-new-wind.png",
            focusStateEnabled: false,
            disabled: true,
            onClick: function (e) {
                var treeL = $("#tree-list").dxTreeList("instance");
                treeL.saveEditData();
                var buttonSave = $("#btnSave").dxButton("instance");
                buttonSave.option('disabled', true);
                var buttonDiscard = $("#btnDiscard").dxButton("instance");
                buttonDiscard.option('disabled', true);
            }
        });

        $("#btnDiscard").dxButton({
            text: "Discard Changes",
            icon: "/Content/Images/close-blue.png",
            focusStateEnabled: false,
            disabled: true,
            onClick: function (e) {
                var treeL = $("#tree-list").dxTreeList("instance");
                treeL.cancelEditData();
                var buttonSave = $("#btnSave").dxButton("instance");
                buttonSave.option('disabled', true);
                var buttonDiscard = $("#btnDiscard").dxButton("instance");
                buttonDiscard.option('disabled', true);
            }
        });

        $("#btnAdd").dxButton({
            text: "Add New Task",
            icon: "/content/Images/plus-blue.png",
            focusStateEnabled: false,
            disabled: bAddNewTaskDisabled,
            onClick: function (e) {
                var treeL = $("#tree-list").dxTreeList("instance");
                var dataSource = treeL.getDataSource();
                let itemorder = dataSource._items.length;
                dataSource.store().push([
                    { type: "insert", key: -(itemorder), data: { ID: -(itemorder), PercentComplete: null, Status: null, StartDate: null, DueDate: null, EstimatedHours: null, ActualHours: null, EstimatedRemainingHours: null, Duration: null, Contribution: 0, Title: "", ParentTaskID: 0 } }
                ]);
            }
        });
        if (TaskParameter == "Gantt View") {
            loadGanttView();
            showHideToolbarIconsGanttView();
        }
        else {
            $("#treelist").show();
            $("#gantt").hide();
        }

    }
    function loadListView() {
        var toolbar = $("#toolbar").dxToolbar("instance");
        gridDataSource = new DevExpress.data.DataSource({
            key: "ID",
            paginate: false,
            reshapeOnPush: true,
            load: function (loadOptions) {
                var deferred = $.Deferred();
                $.getJSON(apiUrl, loadOptions, function (data) {
                    gridDataSourceData = data;
                    $.get(apiSummaryUrl, function (summary, status) {
                        if (summary) {
                            if (summary.StartDate == minDate || summary.StartDate == "Jan-01-0001") {
                                summary.StartDate = "";
                            }
                            if (summary.DueDate == minDate || summary.DueDate == "Jan-01-0001") {
                                summary.DueDate = "";
                            }
                        } 
                        minStartDate = new Date(summary.StartDate);
                        $("#divTaskListSummary").dxDataGrid("instance").option('dataSource', [summary]);
                    });
                    var treeL = $("#tree-list").dxTreeList("instance");
                    
                    firstColWidth = "31px";
                    if (data.length > 0) {
                        // If StartDate and DueDate are Min Date then set Null
                        $.each(data, function (index, value) {

                            if (value.StartDate == minDate)
                                value.StartDate = null;
                            if (value.DueDate == minDate)
                                value.DueDate = null;
                            if (value.StageStep > 0)
                                firstColWidth = "60px";
                        });

                        if (treeL.hasEditData())
                            treeL.cancelEditData();
                    }

                    if (Y > 0) {
                        window.scrollTo(0, Y);
                        Y = 0;
                    }
                    return data;
                }).done(function (data) {
                    deferred.resolve({ data: data, totalCount: data.length });
                });
                return deferred.promise();
            },
            postProcess: function (data) {

                var addDefaultRow = false;
                var lastDataElement = data[data.length - 1];  //it is last element on current page
                var lastGridElement = gridDataSourceData[gridDataSourceData.length - 1];  // last element of data source bind to grid
                if (gridDataSourceData.length > 0) {
                    if (typeof lastDataElement != "undefined") {
                        if (lastDataElement.key == lastGridElement.ID || lastDataElement.key <= 0) {
                            addDefaultRow = true;
                        }
                        else {
                            let lastElement = getLastNode(lastDataElement);
                            if (lastElement.key == lastGridElement.ID)
                                addDefaultRow = true;
                        }
                    }
                } else {
                    addDefaultRow = true;
                }

                if (addDefaultRow) {
                    //let key = Math.max(...data.map(item => item.key))
                    data.push({ type: "insert", key: 0, data: { ID: 0, PercentComplete: 0, Status: "", StartDate: null, DueDate: null, EstimatedHours: null, ActualHours: null, EstimatedRemainingHours: null, Duration: null, Contribution: null, Title: "", ParentTaskID: 0 } })

                }
                return data;
            },
            insert: function (values) {
                return $.ajax({
                    url: ugitConfig.apiBaseUrl + "/api/module/InsertTask",
                    type: "POST",
                    data: { values: JSON.stringify(values), TicketId: '<%= TicketID%>' },
                    async: false,
                    success: function (data) {
                        var treeL = $("#tree-list").dxTreeList("instance");
                        treeL.refresh();
                    }
                })
                    },
                    remove: function (key) {
                        return $.ajax({
                            url: ugitConfig.apiBaseUrl + "/api/module/DeleteTask",
                            type: "DELETE",
                            data: { key: key, TicketId: '<%= TicketID%>' },
                    async: false,
                    success: function (data) {
                        var treeL = $("#tree-list").dxTreeList("instance");
                        treeL.refresh();
                    }
                })
            },
            update: function (key, values) {

                if (values.hasOwnProperty('StartDate') && values.StartDate == null)
                    values.StartDate = minDate;
                if (values.hasOwnProperty('DueDate') && values.DueDate == null)
                    values.DueDate = minDate;
                if (values.hasOwnProperty('EstimatedHours') && values.EstimatedHours == null)
                    values.EstimatedHours = 0;
                if (values.hasOwnProperty('EstimatedRemainingHours') && values.EstimatedRemainingHours == null)
                    values.EstimatedRemainingHours = 0;
                if (values.hasOwnProperty('ActualHours') && values.ActualHours == null)
                    values.ActualHours = 0;
                if (values.hasOwnProperty('PercentComplete') && values.PercentComplete == null)
                    values.PercentComplete = 0;
                if (values.hasOwnProperty('Status') && values.Status == null)
                    values.Status = "Not Started";
                if (values.hasOwnProperty("Duration") && values.Duration == null)
                    values.Duration = 0;

                return $.ajax({
                    url: ugitConfig.apiBaseUrl + "/api/module/UpdateTask",
                    type: "PUT",
                    data: { key: JSON.stringify(key), values: JSON.stringify(values), TicketId: '<%= TicketID%>' },
                    async: false,
                    success: function (data) {
                        var treeL = $("#tree-list").dxTreeList("instance");
                        treeL.refresh(true);
                    }
                })
                    }
                });

        var MyDataField = "";
      
        if (!TaskParameter  || TaskParameter == "All Tasks") {
            MyDataField = "AssignToPct";
        }
        else {
            MyDataField = "AssignedToName";
        }

        viewList[0].dxComponent = $("#tree-list").dxTreeList({
            // columnHidingEnabled: true,
            dataSource: gridDataSource,
            keyExpr: "ID",
            parentIdExpr: sExpand,
            hasItemsExpr: "Has_Items",
            expandedRowKeys: [1, 2],
            rowDragging: {
                allowDropInsideItem: allowDrag,
                allowReordering: allowDrag,
                onDragChange: function (e) {
                    if (e.itemData.ID != 0) {
                        var treeL = $("#tree-list").dxTreeList("instance");
                        var visibleRows = treeL.getVisibleRows();
                        sourceNode = treeL.getNodeByKey(e.itemData.ID);
                        targetNode = visibleRows[e.toIndex].node;
                        while (targetNode && targetNode.data) {
                            if (targetNode.data.ID === sourceNode.data.ID) {
                                e.cancel = true;
                                break;
                            }
                            targetNode = targetNode.parent;
                        }
                    }
                },
                onReorder: function (e) {
                    if (e.itemData.ID != 0) {
                        var visibleRows = e.component.getVisibleRows();
                        sourceData = e.itemData;
                        targetData = visibleRows[e.toIndex].data;
                        $.ajax({
                            url: ugitConfig.apiBaseUrl + "/api/module/DragAndDrop?TicketPublicId=<%= TicketID %>" + "&toKey=" + sourceData.ID + "&fromKey=" + targetData.ID,
                                method: "POST",
                                success: function (data) {
                                    var treeL = $("#tree-list").dxTreeList("instance");
                                    treeL.refresh();
                                },
                                error: function (error) { }
                            })
                                }
                            }
                        },
                        searchPanel: {
                            visible: true
                        },
                        headerFilter: {
                            visible: true,
                            allowSearch: true
                        },
                        editing: {
                            mode: "batch",
                            allowUpdating: true,
                            useIcons: true,
                            refreshMode: "reshape",
                        },
                        selection: {
                            allowSelectAll: allowSelection,
                            mode: selectionModeChk,
                            recursive: false
                        },
                        scrolling: {
                            mode: "virtual",
                            useNative: false,
                            showScrollbar: 'onHover'
                        },
                        columns: [
                            {
                                dataField: "",
                                caption: "",
                                allowFiltering: false,
                                allowEditing: false,
                                alignment: "center",
                                visible: true,
                                //alignment: "right",
                                width: firstColWidth,
                                minWidth: firstColWidth,
                                cellTemplate: function (container, options) {
                                    if (options.data.StageStep !== 0 && options.data.StageStep !== "" && options.data.StageStep !== undefined) {
                                        var stageStep =<%=stageStep%>;
                                        options.column.width = "60px";

                                        if (stageStep != 0 && options.data.StageStep == stageStep)
                                            $("<div id='dataIdA' style='position:relative;height:20px;width:20px;vertical-align:middle;text-align:center;border-radius:15px;background-color: #23af23;color:  #e8e5e5;margin: 0px 0px 0px 3px;padding:3px'>")
                                            .append("<span style='padding-right:0px;'>" + options.data.StageStep + "</span>")
                                            .appendTo(container);
                                        else
                                            $("<div id='dataIdA' style='position:relative;height:20px;width:20px;vertical-align:middle;text-align:center;border-radius: 15px;background-color: #e9e9e9;color: #000;margin: 0px 0px 0px 3px;padding:3px'>")
                                            .append("<span style='padding-right:0px;'>" + options.data.StageStep + "</span>")
                                            .appendTo(container);
                            }

                        }

                    },
                    {
                        dataField: "ItemOrder",
                        caption: "#",
                        alignment: "center",
                        allowFiltering: false,
                        allowEditing: false,
                        allowSearch: true,
                        width: '40px',
                        minWidth: '40px',
                    },
                    {
                        dataField: "Title",
                        caption: "Title",
                        allowEditing: EditingAllow,
                        minWidth: '220px',
                        validationRules: [{ type: "required" }],
                        cellTemplate: function (container, options) {

                            if (typeof options.data.Title != "undefined" && options.data.Title != null) {
                                var MasterDiv = '';
                                var nodeLevel = 0;

                                if (options.row) {

                                    nodeLevel = options.row.node.level;
                                }

                                if (options.data.ChildCount != null && options.data.ChildCount != 0) {
                                    if (options.row.isExpanded) {
                                        MasterDiv = $(`<div id='dataTitle' style='padding-left:${nodeLevel * 25}px'>`)
                                            .append("<div dataKey=" + options.data.ID + " onclick='event.stopPropagation();collapse(this)' class=collapseDiv" + options.data.ID + " style='float:left; margin-right: 8px;'><img src=" + collapseImage + " width='11'/></div>")
                                    }
                                    else {
                                        MasterDiv = $(`<div id='dataTitle' style='padding-left:${nodeLevel * 25}px'>`)
                                            .append("<div dataKey=" + options.data.ID + " onclick='event.stopPropagation();expand(this)' class=expandDiv" + options.data.ID + "  style='float:left; margin-right: 8px;'><img src=" + expandImage + " width='11'></div>")
                                    }
                                }
                                else if (options.data.ChildCount == 0) {

                                    MasterDiv = $(`<div id='dataTitle' style='padding-left:${nodeLevel * 25}px'>`)
                                }

                                if (options.data.Behaviour && options.data.Behaviour != "Task" && options.data.Behaviour != "Action") {
                                    var behaviorType = "";
                                    if (options.data.Behaviour == "Milestone") {
                                        behaviorType = "/Content/Images/milestone_icon.png";
                                    }
                                    else if (options.data.Behaviour == "Deliverable") {
                                        behaviorType = "/Content/Images/edit-task-deliverable.png";
                                    }
                                    else if (options.data.Behaviour == "Receivable") {
                                        behaviorType = "/Content/Images/edit-task-receiveable.png";
                                    }
                                    else if (options.data.Behaviour == "Ticket") {
                                        behaviorType = "/Content/Images/edit-task-ticket.png";
                                    }
                                    MasterDiv.append(`<img src=${behaviorType} width="11px" height="11px" style='margin-right: 4px;' />`)
                                }

                                if (showBaseline.toLowerCase() != "true" && bActUser.toLowerCase() != 'false' && MasterDiv != "") {
                                    var params = options.data.ID + "','" + options.data.ItemOrder + "','" + options.data.ParentTaskID + "','" + escape(options.data.Title);
                                    var prms = options.rowIndex + "','" + options.data.ID;
                                    MasterDiv.append("<span style='overflow: auto;'><a href='javascript:void(0);' onclick=openTask('" + params + "');>" + options.data.Title + "</a></span>")
                                    var actionDiv = $("<div class='pmmGrid-actionBtnWrap' id='actionButtons" + options.data.ID + "'>")
                                    //MasterDiv.append("");
                                    //actionDiv.append("<input type='hidden' name='pmmTaskId' id='pmmTaskId' value='" + options.data.ID + "'><img class='action-description pmm-action-description' src='../../Content/Images/editNewIcon.png' title='" + options.data.Description + "' alt='Help'>");
                                    actionDiv.append("<img id='btnAddNew' value='" + options.data.ID + "' class='action-add pmm-actionAdd' src='/Content/Images/plus-square.png' alt='Add' title='Add task' onclick=addNew('0','" + options.data.ID + "');>");
                                    actionDiv.append("<input type='image' name='btMarkComplete' id='btMarkComplete' value='" + options.data.ID + "' onclick=MarkTaskAsComplete('" + prms + "'); style='float: right;width: 14px;margin-top: 1px;' src='/Content/Images/accept-symbol.png'></div>");
                                    actionDiv.appendTo(MasterDiv);
                                    MasterDiv.appendTo(container);
                                }
                                else {
                                    MasterDiv = $('<div></div>');
                                    MasterDiv.append("<span style='overflow: auto;'>" + options.data.Title + "</span>")
                                        .appendTo(container);
                                }


                            }
                        },
                    },
                            {
                        dataField: MyDataField,
                        caption: "Resource",
                        allowEditing: true,
                        minWidth: '150px',
                        width: '150px',
                        editCellTemplate: function (cellElement, cellInfo) {
                            var selectedUsers = cellInfo.displayValue;
                            if (typeof (selectedUsers) != "object") {
                                selectedUsers = ast_convertStrObject(cellInfo.displayValue);
                            }

                            var pickerBt = $("<img style='width:16px;margin-top:4px;position:relative;top:-5px;right:5px;float:right;cursor:pointer;' class='assigneeToImg' title='Find user based on availability' src='/Content/Images/add-groupBlue.png'  />", { id: cellInfo.data.ID, "group": cellInfo.data.ParentTaskID, "startDate": cellInfo.data.StartDate, "endDate": cellInfo.data.DueDate });
                            var userTagBox = $("<div />").dxTagBox({
                                width: 150,
                                dataSource: ast_dataSource,
                                searchTimeout: 1000,
                                displayExpr: "displayValue",
                                searchEnabled: true,
                                value: selectedUsers,
                                searchExpr: "displayValue",
                                showDropDownButton: true,
                                tagTemplate: function (tagData, container) {

                                    if (!tagData.pct)
                                        tagData.pct = "100";
                                    var editBt = $("<img style='width:16px' src='/Content/Images/editNewIcon.png' class='" + tagData.id + "_txtbox'  />");
                                    var editBox = $("<input type='text' style='width:105px;' class='" + tagData.id + "_txtBt'    />");

                                    editBox.val(tagData.displayValue + "[" + tagData.pct + "%]");
                                    editBox.bind("click", function (e) {
                                        this.focus();
                                        var startIndex = $(this).val().indexOf("[") + 1;
                                        var endIndex = $(this).val().indexOf("%");
                                        ast_txtboxCreateSelection(this, startIndex, endIndex);
                                        e.stopPropagation();
                                    });
                                    editBox.bind("keyup", function (e) {
                                        var es = /\[[0-9%]*\]/g;
                                        var matchPct = es.exec(editBox.val())
                                        var pct = "100";
                                        if (matchPct && matchPct.length > 0) {
                                            val = matchPct[0].replace(/[\[%\]]*/g, '')
                                            pct = $.trim(val);
                                        }

                                        var tagBoxCtl = userTagBox.dxTagBox("instance")
                                        if (tagBoxCtl) {
                                            var selectedTags = tagBoxCtl.option("value");
                                            var pctUser = _.findWhere(selectedTags, { id: tagData.id });
                                            if (pctUser) {
                                                pctUser.pct = pct;
                                                var userStr = ast_convertObjToStr(selectedTags);
                                                cellInfo.setValue(userStr);
                                            }
                                        }
                                    });
                                    editBt.bind("click", function (e) {
                                        editBox.focus();
                                        var startIndex = editBox.val().indexOf("[") + 1;
                                        var endIndex = editBox.val().indexOf("%");
                                        ast_txtboxCreateSelection(editBox.get(0), startIndex, endIndex);
                                        e.stopPropagation();
                                    });
                                    return $("<div />")
                                        .addClass("dx-tag-content")
                                        .append(
                                            editBox,
                                            editBt,
                                            $("<div />").addClass("dx-tag-remove-button")
                                        );
                                },
                                onValueChanged: function (e) {
                                    var userStr = ast_convertObjToStr(e.value);
                                    cellInfo.setValue(userStr);
                                }

                            });

                            var mainContainer = $("<div />")
                                .append(userTagBox,
                                    pickerBt);

                            mainContainer.appendTo(cellElement);
                        },
                        cellTemplate: function (container, options) {

                            var users = options.displayValue;
                            if (typeof (users) != "object") {
                                users = ast_convertStrObject(options.displayValue);
                            }
                            var ctn = $("<div />");
                            if (users) {
                                var usersLink = [];
                                users.forEach(function (s) {
                                    var uformat = "";
                                    if (!isNaN(Number(s.pct)) && Number(s.pct) < 100 && Number(s.pct) > 0) {
                                        uformat = s.displayValue + "[" + s.pct + "%]";
                                    }
                                    else {
                                        uformat = s.displayValue;
                                    }
                                    usersLink.push($("<a style='padding-right:3px; color: #1a399d;' href='javascript:' onclick='openResourceTimeSheet(\"" + s.id + "\", \"" + s.displayValue.replace("'", "") + "\")' title=\"" + uformat + "\"  />").text(uformat));
                                });
                                usersLink.forEach(function (s, i) {
                                    if (usersLink.length > 1 && usersLink.length - 1 == i) {
                                        ctn.append($("<span />").text("; "));
                                    }
                                    ctn.append(s);
                                });
                            }
                            else {
                                ctn.append($("<span/>").text())
                            }
                            ctn.appendTo(container)
                        }
                    },
                    {
                        dataField: "PercentComplete",
                        caption: "% Comp.",
                        alignment: "center",
                        allowEditing: EditingAllow,
                        allowFiltering: false,
                        visible: moduleName == "NPR" ? false : true,
                        width: '75px',
                        minWidth: '75px',
                    },
                    {
                        dataField: "Status",
                        caption: "Status",
                        alignment: "center",
                        allowEditing: EditingAllow,
                        width: '90px',
                        minWidth: '90px',
                        alignment: "center",
                        visible: moduleName == "NPR" ? false : true,
                        lookup: {
                            dataSource: [
                                "Not Started",
                                "In Progress",
                                "Completed",
                                "Waiting",
                            ]
                        }
                    },
                    {
                        dataField: "Predecessors",
                        caption: "Pred",
                        allowEditing: true,
                        allowFiltering: false,
                        width: '55px',
                        minWidth: '55px',
                        cellTemplate: function (container, options) {

                            if (options.data.Predecessors != null) {
                                $("<div id='dataId'>")
                                    .append("<div style='float: left;word-break: break-word;'>" + options.data.Predecessors + "</div>")
                                    .appendTo(container);
                            }
                        }
                    },
                    {
                        dataField: "EstimatedHours",
                        caption: "Est Hrs.",
                        allowEditing: EditingAllow,
                        allowFiltering: false,
                        dataType: "number",
                        alignment: "center",
                        width: '62px',
                        minWidth: '62px',
                    },
                    {
                        dataField: "ActualHours",
                        caption: "Act Hrs.",
                        alignment: "center",
                        allowEditing: '<%=actualHoursByUser%>' == 'True' ? true : false,
                        allowFiltering: false,
                        dataType: "number",
                        visible: moduleName == "NPR" ? false : true,
                        width: '60px',
                        minWidth: '60px',
                    },
                    {
                        dataField: "EstimatedRemainingHours",
                        caption: "ERH",
                        allowEditing: EditingAllow,
                        allowFiltering: false,
                        dataType: "number",
                        visible: moduleName == "NPR" ? false : true,
                        width: '60px',
                        minWidth: '60px',
                        alignment: "center",
                    },
                    {
                        dataField: "StartDate",
                        caption: "Start Date",
                        allowEditing: EditingAllow,
                        dataType: "date",
                        allowFiltering: false,
                        cssClass: "treeList-dateFeild",
                        minWidth: '100px',
                        width: '100px',
                        alignment: "center",
                        validationRules: [{ type: "required" }],
                        format: 'MMM d, yyyy',
                        editorOptions: {
                            showClearButton: true,
                            onOpened: function (e) {

                                e.component._popup.option("position", {
                                    my: "center",
                                    at: "center",
                                });

                            }
                        }
                    },
                    {
                        dataField: "DueDate",
                        caption: "Due Date",
                        allowEditing: EditingAllow,
                        allowFiltering: false,
                        dataType: "date",
                        minWidth: '100px',
                        width: '100px',
                        cssClass: "treeList-dateFeild",
                        alignment: "center",
                        validationRules: [{ type: "required" }],
                        format: 'MMM d, yyyy',
                        editorOptions: {
                            showClearButton: true,
                            onOpened: function (e) {

                                e.component._popup.option("position", {
                                    my: "center",
                                    at: "center",
                                });

                            }
                        },
                        cellTemplate: function (container, options) {
                            if (options.data.DueDate != undefined && options.data.DueDate != minDate && options.data.DueDate != null) {
                                var MasterDiv = $("<div id='dataDueDate'>" + formatDate(options.data.DueDate) + "</div>");
                                if (options.data.Status != "Completed") {
                                    var DiffTime = new Date().getTime() - new Date(options.data.DueDate).getTime();
                                    var DiffDays = DiffTime / (1000 * 3600 * 24);
                                    if (DiffDays > 1) {
                                        MasterDiv = $("<div id='dataDueDate' style='color:#FF0000;font-weight:bold;'>" + formatDate(options.data.DueDate) + "</div>")
                                    }
                                }
                                MasterDiv.appendTo(container);
                            }

                        }

                    },
                    {
                        dataField: "Duration",
                        caption: "Duration",
                        allowEditing: false,
                        allowFiltering: false,
                        minWidth: '90px',
                        alignment: "center",
                        width: '90px',
                        cellTemplate: function (container, options) {

                            var strDuration = "";
                            var strContribution = "";
                            if (options.data.Duration != null) {
                                if (options.data.Duration <= 1) {
                                    strDuration = options.data.Duration + " Day";
                                }
                                else {
                                    strDuration = options.data.Duration + " Days";
                                }

                                //if (options.data.Contribution >= -1) {
                                    strContribution = " (" + options.data.Contribution + "%)";
                                //}
                                $("<div id='dataId'>")
                                    .append("<span style='overflow: auto;padding-right: 3px;'>" + strDuration + "</span>")
                                    .append("<span style='overflow: auto;'>" + strContribution + "</span>")
                                    .appendTo(container);
                            }
                        }
                    },

                ],
                cacheEnabled: true,
                remoteOperations: false,
                showRowLines: true,
                showBorders: true,
                columnAutoWidth: false,
                wordWrapEnabled: true,
                onToolbarPreparing: function (e) {
                    e.toolbarOptions.visible = false;
                },
                onRowValidating: function (e) {
                    if (e.isValid == false) {
                        if (e.brokenRules != undefined && e.brokenRules.length > 0) {
                            e.errorText = e.brokenRules[0].message;
                        }
                        else
                            e.errorText = "Title is required.";
                    }

                    if (typeof e.newData.StartDate !== "undefined" && typeof e.newData.DueDate !== "undefined") {
                        if (new Date(e.newData.DueDate).setHours(0, 0, 0, 0) < new Date(e.newData.StartDate).setHours(0, 0, 0, 0)) {
                            e.isValid = false;
                            e.errorText = "StartDate should be less then EndDate";
                        }
                    }
                    else {
                        if (typeof e.newData.StartDate == "undefined") {
                            if (new Date(e.newData.DueDate).setHours(0, 0, 0, 0) < new Date(e.oldData.StartDate).setHours(0, 0, 0, 0)) {
                                e.isValid = false;
                                e.errorText = "StartDate should be less then EndDate";

                            }
                        }
                        if (typeof e.newData.DueDate == "undefined") {
                            if (new Date(e.oldData.DueDate).setHours(0, 0, 0, 0) < new Date(e.newData.StartDate).setHours(0, 0, 0, 0)) {
                                e.isValid = false;
                                e.errorText = "StartDate should be less then EndDate";
                            }
                        }
                    }


                },
                onInitNewRow: function (e) {

                    var data = e.component.getDataSource()._items;
                    let itemorder = e.component.getVisibleRows().length;
                    var values = {
                        ID: 0,
                        PercentComplete: 0,
                        Status: "Not Started",
                        StartDate: new Date(),
                        DueDate: new Date(),
                        EstimatedHours: 0,
                        ActualHours: 0,
                        EstimatedRemainingHours: 0,
                        Duration: 1,
                        Contribution: 0,
                        Title: e.data.ParentTaskID == 0 ? "NewTask" : "NewChildTask",
                        ParentTaskID: e.data.ParentTaskID,
                        IsNextNewRowSet: false,
                        ItemOrder: itemorder + 1
                    }
                    e.data = values;
                },
                onEditingStart: function (e) {
                    if (e.data != null && e.data != undefined && e.data.ChildCount > 0 && e.key != undefined) {
                        if (e.column.dataField == "Predecessors" || e.column.dataField == "EstimatedHours" || e.column.dataField == "ActualHours" || e.column.dataField == "EstimatedRemainingHours" || e.column.dataField == "PercentComplete" || e.column.dataField == "StartDate" || e.column.dataField == "DueDate" || e.column.dataField == "Status" || e.column.dataField == "AssignToPct") {
                            e.cancel = true;
                        }
                    }
                    if (e.data.ID == 0 && e.column.dataField == "Predecessors") {
                        e.cancel = true;
                    }
                    else if (e.data.ID <= 0 && e.column.dataField == "Title") {
                        var dataSource = e.component.getDataSource();
                        let itemorder = dataSource._items.length;
                        if (itemorder - 1 <= -(e.data.ID)) {
                            dataSource.store().push([
                                { type: "insert", key: -(itemorder), data: { ID: -(itemorder), PercentComplete: null, Status: null, StartDate: null, DueDate: null, EstimatedHours: null, ActualHours: null, EstimatedRemainingHours: null, Duration: null, Contribution: 0, Title: "", ParentTaskID: 0 } }
                            ]);
                        }

                        var element = e.component.getCellElement(e.component.getRowIndexByKey(e.key), "Title");
                        element.text("");
                        e.component.focus(element);
                    }
                },
                onEditorPreparing: function (e) {
                    var treeL = $("#tree-list").dxTreeList("instance");
                    if (e.parentType === 'dataRow' && e.dataField === 'Title') {
                        e.editorOptions.onValueChanged = function (obj) {
                            currentRowIndex = treeL.getRowIndexByKey(e.row.key);
                            cellData = e.row.data;
                            treeL.cellValue(currentRowIndex, "Title", obj.value);

                            if (obj.value != null && obj.value !== '' && typeof obj.value != "undefined") {
                                if (isNaN(parseInt(cellData.EstimatedHours)) || parseInt(cellData.EstimatedHours) <= 0) {
                                    cellData.EstimatedHours = 8;
                                    cellData.EstimatedRemainingHours = 8;
                                    treeL.cellValue(currentRowIndex, "EstimatedHours", "8");
                                    treeL.cellValue(currentRowIndex, "EstimatedRemainingHours", "8");
                                }
                            }
                        }
                    }
                    if (e.parentType === 'dataRow' && (e.dataField === 'StartDate' || e.dataField === 'DueDate')) {
                        if (gridDataSourceData.length < 5) {
                            $("#treelist").height("300px");
                        }
                    }
                    if (e.parentType === 'dataRow' && e.dataField === 'StartDate') {
                        cellData = e.row.data;
                        currentRowIndex = treeL.getRowIndexByKey(e.row.key);
                        e.editorOptions.onValueChanged = function (obj) {
                            if (obj && obj.value) {
                                var date = new Date(obj.value),
                                    yr = date.getFullYear(),
                                    month = date.getMonth() + 1,
                                    day = date.getDate(),
                                    newDate = month + "/" + day + "/" + yr;
                                obj.value = new Date(newDate);
                            }
                            if (obj.value == null) {
                                if (currentRowIndex < 0) {
                                    cellData.StartDate = obj.value;
                                    cellData.DueDate = obj.value;
                                }
                                else {
                                    treeL.cellValue(currentRowIndex, "StartDate", obj.value);
                                }
                            }
                            else {

                                if (obj.previousValue == null) {
                                    if (currentRowIndex < 0) {
                                        cellData.StartDate = obj.value;
                                        cellData.DueDate = obj.value;
                                    }
                                    else {
                                        treeL.cellValue(currentRowIndex, "StartDate", obj.value);
                                        treeL.cellValue(currentRowIndex, "DueDate", obj.value);
                                    }
                                }
                                else {
                                    if (currentRowIndex < 0) {
                                        cellData.StartDate = obj.value;
                                        cellData.DueDate = obj.value;
                                        //treeL.cellValue(currentRowIndex, "Duration", DifferenceInDays(obj.value, cellData.DueDate));
                                    }
                                    else {
                                        treeL.cellValue(currentRowIndex, "StartDate", obj.value);

                                        if (typeof cellData.Duration != "undefined" && cellData.Duration > 0) {
                                            var newduedate;
                                            var strtDte = new Date(obj.value);
                                            var paramsInJson = '{' + '"startDate":"' + strtDte.toLocaleDateString() + '","noOfWorkingDays":"' + cellData.Duration + '"}';

                                            $.ajax({
                                                type: "POST",
                                                url: ajaxHelperUrl + "/GetEndDateByWorkingDays",
                                                data: paramsInJson,
                                                contentType: "application/json; charset=utf-8",
                                                dataType: "json",
                                                success: function (message) {
                                                    var resultJson = $.parseJSON(message.d);
                                                    if (resultJson.messagecode == 2) {
                                                        newduedate = resultJson.enddate;
                                                        treeL.cellValue(currentRowIndex, "DueDate", newduedate);
                                                    }
                                                },
                                                error: function (xhr, ajaxOptions, thrownError) {
                                                }
                                            });

                                        }


                                    }
                                }
                            }

                        }
                    }
                    if (e.parentType === 'dataRow' && e.dataField === 'DueDate') {
                        cellData = e.row.data;
                        currentRowIndex = treeL.getRowIndexByKey(e.row.key);
                        e.editorOptions.onValueChanged = function (obj) {
                            if (obj && obj.value) {
                                var date = new Date(obj.value),
                                    yr = date.getFullYear(),
                                    month = date.getMonth() + 1,
                                    day = date.getDate(),
                                    newDate = month + "/" + day + "/" + yr;
                                obj.value = new Date(newDate);
                            }
                            if (obj.value == null) {
                                if (currentRowIndex < 0) {
                                    cellData.StartDate = obj.value;
                                    cellData.DueDate = obj.value;
                                }
                                else {
                                    treeL.cellValue(currentRowIndex, "DueDate", obj.value);
                                }
                            }
                            else {
                                if (obj.previousValue == null) {
                                    if (currentRowIndex < 0) {
                                        cellData.StartDate = obj.value;
                                        cellData.DueDate = obj.value;
                                    }
                                    else {
                                        treeL.cellValue(currentRowIndex, "DueDate", obj.value);
                                        treeL.cellValue(currentRowIndex, "StartDate", obj.value);
                                    }
                                }
                                else {
                                    if (currentRowIndex < 0) {
                                        cellData.StartDate = obj.value;
                                        cellData.DueDate = obj.value;
                                    }
                                    else {

                                        treeL.cellValue(currentRowIndex, "DueDate", obj.value);
                                        //treeL.cellValue(currentRowIndex, "Duration", DifferenceInDays(cellData.StartDate, obj.value));
                                    }
                                }
                            }
                            //treeL.cellValue(currentRowIndex, "Duration", DifferenceInDays(cellData.StartDate, obj.value));
                        }
                    }

                    if (e.parentType === 'dataRow' && e.dataField === 'PercentComplete') {
                        e.editorOptions.min = 0;
                        e.editorOptions.max = 100;
                        cellData = e.row.data;
                        currentRowIndex = treeL.getRowIndexByKey(e.row.key);
                        e.editorOptions.onValueChanged = function (obj) {
                            //var d = _.find(treeL._options.dataSource._items, function (current) { return current.key == cellData.ID; });
                            if (obj.value >= 100) {
                                if (currentRowIndex < 0) {
                                    cellData.PercentComplete = obj.value;
                                    cellData.Status = "Completed";
                                }
                                treeL.cellValue(currentRowIndex, "PercentComplete", obj.value);
                                treeL.cellValue(currentRowIndex, "Status", "Completed");
                            }
                            if (obj.value <= 0) {
                                if (currentRowIndex < 0) {
                                    cellData.PercentComplete = obj.value;
                                    cellData.Status = "Not Started";
                                }
                                treeL.cellValue(currentRowIndex, "PercentComplete", obj.value);
                                treeL.cellValue(currentRowIndex, "Status", "Not Started");
                            }
                            if (obj.value > 0 && obj.value < 100) {
                                if (currentRowIndex < 0) {
                                    cellData.PercentComplete = obj.value;
                                    cellData.Status = "In Progress";
                                }
                                treeL.cellValue(currentRowIndex, "PercentComplete", obj.value);
                                treeL.cellValue(currentRowIndex, "Status", "In Progress");
                            }
                        }
                    }
                    if (e.parentType === 'dataRow' && e.dataField === 'Status') {
                        cellData = e.row.data;
                        currentRowIndex = treeL.getRowIndexByKey(e.row.key);
                        e.editorOptions.onValueChanged = function (obj) {
                            //var d = _.find(treeL._options.dataSource._items, function (current) { return current.key == cellData.ID; });
                            if (obj.value == "Completed") {
                                if (currentRowIndex < 0) {
                                    cellData.Status = obj.value;
                                    cellData.PercentComplete = 100;
                                }
                                treeL.cellValue(currentRowIndex, "Status", obj.value);
                                treeL.cellValue(currentRowIndex, "PercentComplete", 100);
                            }
                            if (obj.value == "Not Started") {
                                if (currentRowIndex < 0) {
                                    cellData.Status = obj.value;
                                    cellData.PercentComplete = 0;
                                }
                                treeL.cellValue(currentRowIndex, "Status", obj.value);
                                treeL.cellValue(currentRowIndex, "PercentComplete", 0);
                            }
                            if (obj.value == "In Progress") {
                                if (currentRowIndex < 0) {
                                    cellData.Status = obj.value;
                                    cellData.PercentComplete = 0;
                                }
                                if (obj.previousValue == "Completed") {
                                    treeL.cellValue(currentRowIndex, "Status", obj.value);
                                    treeL.cellValue(currentRowIndex, "PercentComplete", 90);
                                }
                                if (obj.previousValue == "Not Started") {
                                    treeL.cellValue(currentRowIndex, "Status", obj.value);
                                    treeL.cellValue(currentRowIndex, "PercentComplete", 0);
                                }
                            }
                            if (obj.value == "Waiting") {
                                if (currentRowIndex < 0) {
                                    cellData.Status = obj.value;
                                }
                                treeL.cellValue(currentRowIndex, "Status", obj.value);
                            }
                        }
                    }
                    if (e.parentType === 'dataRow' && e.dataField === 'EstimatedHours') {
                        cellData = e.row.data;
                        currentRowIndex = treeL.getRowIndexByKey(e.row.key);
                        e.editorOptions.onValueChanged = function (obj) {
                            if (currentRowIndex < 0) {
                                cellData.EstimatedHours = obj.value;
                                cellData.EstimatedRemainingHours = obj.value;
                            }
                            cellData.EstimatedRemainingHours = obj.value - cellData.ActualHours;
                            treeL.cellValue(currentRowIndex, "EstimatedHours", obj.value)
                            treeL.cellValue(currentRowIndex, "EstimatedRemainingHours", cellData.EstimatedRemainingHours)
                            if (cellData.StartDate == null || cellData.StartDate == '') {
                                var todayDate = new Date();
                                treeL.cellValue(currentRowIndex, "StartDate", todayDate);
                                todayDate.setDate(todayDate.getDate() + (cellData.EstimatedHours / 8))
                                treeL.cellValue(currentRowIndex, "DueDate", todayDate);
                            } else {
                                var todayDate = new Date(cellData.StartDate);
                                treeL.cellValue(currentRowIndex, "StartDate", todayDate);
                                todayDate.setDate(todayDate.getDate() + (cellData.EstimatedHours / 8))
                                treeL.cellValue(currentRowIndex, "DueDate", todayDate);
                            }
                        }
                    }
                    if (e.parentType === 'dataRow' && e.dataField === 'EstimatedRemainingHours') {
                        cellData = e.row.data;
                        currentRowIndex = treeL.getRowIndexByKey(e.row.key);
                        e.editorOptions.onValueChanged = function (obj) {
                            if (currentRowIndex < 0) {
                                cellData.EstimatedRemainingHours = obj.value;
                                cellData.EstimatedHours = obj.value;
                            }
                            cellData.EstimatedHours = obj.value + cellData.ActualHours;
                            treeL.cellValue(currentRowIndex, "EstimatedRemainingHours", obj.value)
                            treeL.cellValue(currentRowIndex, "EstimatedHours", cellData.EstimatedHours)
                        }
                    }
                    if (e.parentType === 'dataRow' && e.row.rowType == 'data' && e.row.isNewRow && e.dataField === 'ItemOrder') {
                        //treeL._options.rowDragging.allowReordering = false;
                        //treeL._options.rowDragging.allowDropInsideItem = false;
                        treeL._options._optionManager._options.allowColumnReordering = false;
                        treeL._options._optionManager._options.allowDropInsideItem = false;
                    }
                    else {
                        //treeL._options.rowDragging.allowReordering = true;
                        //treeL._options.rowDragging.allowDropInsideItem = true;
                        treeL._options._optionManager._options.allowColumnReordering = true;
                        treeL._options._optionManager._options.allowDropInsideItem = true;
                    }
                },
                onContentReady: function (e) {
                    var treeL = $("#tree-list").dxTreeList("instance");
                    var dxSummaryGrid = $("#divTaskListSummary").dxDataGrid("instance");

                    if (parseInt($('#tree-list').height()) < (maxHeightOfTaskListGrid - 160) && navigator.appVersion.indexOf("Mac") != -1 && IsCompactView.toLowerCase() == "true") {
                        //dxSummaryGrid.columnOption(6, 'width', '120px');
                    }
                    treeL.columnOption("command:edit", "width", 80);
                    var modifiedrows = treeL.getVisibleRows();
                    var criticalTask = _.filter(modifiedrows, function (current) { return current.data.isCritical == true; });
                    if (criticalTask.length > 0) {
                        criticalTask.forEach(function (e) {
                            var element = treeL.getRowElement(treeL.getRowIndexByKey(criticalTask.key));
                            if (IsCriticalTask) {
                                element[0].style.border = "2px solid #ffc1a4";
                            }
                            else {
                                element[0].style.border = "";
                            }
                        });
                    }
                    if (IsCompactView.toLowerCase() != "true")
                        window.parent.adjustIFrameWithHeight("<%=FrameId %>", $(".managementcontrol-main").height());

                    if (currentTasKIndex > -1) {
                        window.scrollTo(0, currentTasKIndex);
                        currentTasKIndex = -1;
                    }
                    if (e.component.hasEditData()) {
                        var buttonSave = $("#btnSave").dxButton("instance");
                        buttonSave.option('disabled', false);
                        var buttonDiscard = $("#btnDiscard").dxButton("instance");
                        buttonDiscard.option('disabled', false);
                    }
                    if (bActUser.toLowerCase() == "false") {
                        toolbar._options._optionManager._options.items[8].disabled = true;
                        toolbar._options._optionManager._options.items[9].disabled = true;
                        toolbar._options._optionManager._options.items[10].disabled = true;
                        toolbar._options._optionManager._options.items[11].disabled = true;
                        toolbar._options._optionManager._options.items[12].disabled = true;
                        toolbar._options._optionManager._options.items[21].options.items[0].items[1].disabled = true;
                        toolbar._options._optionManager._options.items[21].options.items[0].items[2].disabled = true;
                    }
                    if (TaskParameter == "My Tasks") {
                        toolbar._options._optionManager._options.items[8].disabled = true;
                        toolbar._options._optionManager._options.items[9].disabled = true;
                        toolbar._options._optionManager._options.items[10].disabled = true;
                        toolbar._options._optionManager._options.items[11].disabled = true;
                        toolbar._options._optionManager._options.items[12].disabled = true;
                        toolbar._options._optionManager._options.items[21].options.items[0].items[1].disabled = true;
                        toolbar._options._optionManager._options.items[21].options.items[0].items[2].disabled = true;
                    }
                    if (showBaseline.toLowerCase() == "true") {
                        toolbar._options._optionManager._options.items[2].disabled = true;
                        toolbar._options._optionManager._options.items[3].disabled = true;
                        toolbar._options._optionManager._options.items[4].disabled = true;
                        toolbar._options._optionManager._options.items[5].disabled = true;
                        toolbar._options._optionManager._options.items[6].disabled = true;
                        toolbar._options._optionManager._options.items[7].disabled = true;
                        toolbar._options._optionManager._options.items[8].disabled = true;
                        toolbar._options._optionManager._options.items[10].disabled = true;
                        toolbar._options._optionManager._options.items[11].disabled = true;
                        toolbar._options._optionManager._options.items[13].disabled = true;
                        toolbar._options._optionManager._options.items[14].disabled = true;
                        toolbar._options._optionManager._options.items[15].disabled = true;
                        toolbar._options._optionManager._options.items[16].disabled = true;
                        toolbar._options._optionManager._options.items[17].disabled = true;
                        toolbar._options._optionManager._options.items[22].disabled = true;
                        toolbar._options._optionManager._options.items[23].disabled = true;
                        toolbar._options._optionManager._options.items[21].options.items[0].items[1].disabled = true;
                        toolbar._options._optionManager._options.items[21].options.items[0].items[2].disabled = true;
                        toolbar.repaint();
                    }
                },
                onSelectionChanged: function (e) {
                    if (showBaseline.toLowerCase() != "true") {
                        if (e.selectedRowKeys.length == 1) {
                            toolbar._options._optionManager._options.items[2].disabled = false;
                            toolbar._options._optionManager._options.items[4].disabled = false;
                            toolbar._options._optionManager._options.items[8].disabled = false;
                            toolbar._options._optionManager._options.items[9].disabled = false;
                            toolbar._options._optionManager._options.items[10].disabled = false;
                            toolbar._options._optionManager._options.items[11].disabled = false;
                            toolbar._options._optionManager._options.items[16].disabled = false;
                            toolbar._options._optionManager._options.items[13].disabled = false;
                            toolbar._options._optionManager._options.items[14].disabled = false;
                            toolbar._options._optionManager._options.items[17].disabled = false;
                            toolbar._options._optionManager._options.items[18].disabled = false;
                            toolbar._options._optionManager._options.items[19].disabled = false;
                            toolbar._options._optionManager._options.items[22].disabled = false;
                            toolbar._options._optionManager._options.items[23].disabled = false;
                            toolbar._options._optionManager._options.items[21].options.items[0].items[1].disabled = false;
                            toolbar._options._optionManager._options.items[21].options.items[0].items[2].disabled = false;
                            toolbar.repaint();
                        }
                        else if (e.selectedRowKeys.length > 1) {
                            toolbar._options._optionManager._options.items[2].disabled = false;
                            toolbar._options._optionManager._options.items[4].disabled = false;
                            toolbar._options._optionManager._options.items[8].disabled = false;
                            toolbar._options._optionManager._options.items[9].disabled = false;
                            toolbar._options._optionManager._options.items[10].disabled = false;
                            toolbar._options._optionManager._options.items[11].disabled = false;
                            toolbar._options._optionManager._options.items[16].disabled = false;
                            toolbar._options._optionManager._options.items[13].disabled = false;
                            toolbar._options._optionManager._options.items[14].disabled = false;
                            toolbar._options._optionManager._options.items[17].disabled = false;
                            toolbar._options._optionManager._options.items[18].disabled = false;
                            toolbar._options._optionManager._options.items[19].disabled = false;
                            toolbar._options._optionManager._options.items[22].disabled = true;
                            toolbar._options._optionManager._options.items[23].disabled = true;
                            toolbar._options._optionManager._options.items[21].options.items[0].items[1].disabled = true;
                            toolbar._options._optionManager._options.items[21].options.items[0].items[2].disabled = true;
                            toolbar.repaint();
                        }
                        else {
                            //toolbar._options._optionManager._options.items[2].disabled = true;
                            toolbar._options._optionManager._options.items[4].disabled = true;
                            toolbar._options._optionManager._options.items[8].disabled = true;
                            toolbar._options._optionManager._options.items[9].disabled = false;
                            toolbar._options._optionManager._options.items[10].disabled = true;
                            toolbar._options._optionManager._options.items[11].disabled = true;
                            toolbar._options._optionManager._options.items[16].disabled = true;
                            toolbar._options._optionManager._options.items[13].disabled = true;
                            toolbar._options._optionManager._options.items[14].disabled = true;
                            toolbar._options._optionManager._options.items[17].disabled = true;
                            toolbar._options._optionManager._options.items[18].disabled = true;
                            toolbar._options._optionManager._options.items[19].disabled = true;
                            toolbar._options._optionManager._options.items[22].disabled = true;
                            toolbar._options._optionManager._options.items[23].disabled = true;
                            toolbar._options._optionManager._options.items[21].options.items[0].items[1].disabled = true;
                            toolbar._options._optionManager._options.items[21].options.items[0].items[2].disabled = true;
                            toolbar.repaint();
                        }
                    }
                },
                onRowPrepared: function (e) {
                    $(e.rowElement).attr({ 'data-row-index': e.rowIndex });
                },
                onCellPrepared: function (e) {
                    if (e.rowType === "data" && (e.column.dataField === "Title" || e.column.dataField === "Status" || e.column.dataField === "StartDate" || e.column.dataField === "Duration" ||
                        e.column.dataField === "DueDate" || e.column.dataField === "PercentComplete" || e.column.dataField === "EstimatedHours" || e.column.dataField === "ActualHours")) {

                        if (e.data.ID <= 0 && e.data.Title === "")
                            e.cellElement.addClass("ugit-highlight-outline");
                    }
                },
                onCellClick: function (e) {
                    tasklistrowindex = e.rowIndex;
                    Y = window.pageYOffset;
                },
                onInitialized: function (e) {

                },
                onSaved: function (e) {
                <%--gridDataSourceData.forEach(function (rowObj) {
                    var pred = rowObj.Predecessors;
                    if (pred == 0 || pred == null) {
                        return;
                    }
                    var projectid = '<%=TicketID%>';
                    var moduleName = '<%=ModuleName%>';
                    var taskid = rowObj.ID;

                        if (projectid != "" && moduleName != "" && taskid != "") {
                            var paramsInJson = '{"taskid":' + taskid + ',"projectId":' + projectid + ',"moduleName":"' + moduleName + '"}';
                            $.ajax({
                                type: "POST",
                                url: ugitConfig.apiBaseUrl + "/api/module/OnPredecessorChange?taskid=" + taskid + "&projectId=" + projectid + "&moduleName=" + moduleName,
                                data: paramsInJson,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (message) {
                                    
                                    var resultJson = $.parseJSON(message);
                                    if (resultJson.messagecode == 2) {
                                        var deptasks = resultJson.value;
                                        var deptaskArray = deptasks.split(",");
                                        var maxitemorder = resultJson.maxitemorder;
                                        //predecessorValidation(s, e, deptaskArray, maxitemorder);
                                    }
                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                }
                            });
                        }
                });--%>
                        }
                    }).dxTreeList("instance");
        var taskListSummary = $("#divTaskListSummary").dxDataGrid({
            dataSource: gridDataSourceData,
            columnMinWidth: 10,
            scrolling: {
                mode: "virtual",
            },
            columns: [
                {
                    dataField: "ProjectCompletion",
                    alignment: 'right',
                    cssClass: "cell-summary-PercentComplete"
                    //Width: 300
                },
                {
                    dataField: "EstimatedHours",
                    width: '62px',
                    alignment: 'center',
                    cssClass: "cell-font-bold"
                },

                {
                    dataField: "ActualHours",
                    width: '60px',
                    alignment: 'center',
                    cssClass: "cell-font-bold",
                },
                {
                    dataField: "EstimatedRemainingHours",
                    width: '60px',
                    alignment: 'center',
                    cssClass: "cell-font-bold"
                },
                {
                    dataField: "StartDate",
                    width: '120px',
                    alignment: 'center',
                    cssClass: "cell-font-bold"
                },
                {
                    dataField: "DueDate",
                    width: '120px',
                    alignment: 'center',
                    cssClass: "cell-font-bold"
                },
                {
                    dataField: "Duration",
                    width: widthOfSummaryDurationColumn,
                    alignment: 'center',
                    cssClass: "cell-font-bold"
                }],
            showBorders: true,
            onRowPrepared: function (e) {
                if (e.rowType == "header") {
                    e.rowElement.css({ "display": 'none', "height": '200px' });
                }
                e.rowElement.css({ "height": '40', "font-weight": "bold", "font-size": "12px" });
            }
        });

    }
    function showHideToolbarIconsGanttView() {
        $("#treelist").hide();
        $("#gantt").show();
        $("#kanbanView").hide();
        $('.monitorpanel').show();
        var toolbar = $("#toolbar").dxToolbar("instance");
        var ganttinstance = viewList[1].dxComponent;
        isKanBanViewActive = false;
        toolbar._options._optionManager._options.items.find(x => x.name == 'taskType') ? toolbar._options._optionManager._options.items.find(x => x.name == 'taskType').visible = false : '';
        toolbar._options._optionManager._options.items.find(x => x.name == 'expandAll') ? toolbar._options._optionManager._options.items.find(x => x.name == 'expandAll').visible = false : '';
        toolbar._options._optionManager._options.items.find(x => x.name == 'collapseAll') ? toolbar._options._optionManager._options.items.find(x => x.name == 'collapseAll').visible = false : '';
        toolbar._options._optionManager._options.items.find(x => x.name == 'criticalTask') ? toolbar._options._optionManager._options.items.find(x => x.name == 'criticalTask').visible = false : '';
        toolbar._options._optionManager._options.items.find(x => x.name == 'resourceUsage') ? toolbar._options._optionManager._options.items.find(x => x.name == 'resourceUsage').visible = false : '';
        toolbar._options._optionManager._options.items.find(x => x.name == 'autoAdjustSchedule') ? toolbar._options._optionManager._options.items.find(x => x.name == 'autoAdjustSchedule').visible = false : '';

        toolbar._options._optionManager._options.items.find(x => x.name == 'markAsComplete') ? toolbar._options._optionManager._options.items.find(x => x.name == 'markAsComplete').visible = false : '';
        toolbar._options._optionManager._options.items.find(x => x.name == 'duplicateTask') ? toolbar._options._optionManager._options.items.find(x => x.name == 'duplicateTask').visible = false : '';
        toolbar._options._optionManager._options.items.find(x => x.name == 'decreaseIndent') ? toolbar._options._optionManager._options.items.find(x => x.name == 'decreaseIndent').visible = false : '';
        toolbar._options._optionManager._options.items.find(x => x.name == 'increaseIndent') ? toolbar._options._optionManager._options.items.find(x => x.name == 'increaseIndent').visible = false : '';
        toolbar._options._optionManager._options.items.find(x => x.name == 'deleteSelectedTask') ? toolbar._options._optionManager._options.items.find(x => x.name == 'deleteSelectedTask').visible = false : '';

        toolbar._options._optionManager._options.items.find(x => x.name == 'saveAsTemplate') ? toolbar._options._optionManager._options.items.find(x => x.name == 'saveAsTemplate').visible = false : '';
        toolbar._options._optionManager._options.items.find(x => x.name == 'exportTasksFromMPP') ? toolbar._options._optionManager._options.items.find(x => x.name == 'exportTasksFromMPP').visible = false : '';
        toolbar._options._optionManager._options.items.find(x => x.name == 'importTasksFromMPP') ? toolbar._options._optionManager._options.items.find(x => x.name == 'importTasksFromMPP').visible = false : '';
        toolbar._options._optionManager._options.items.find(x => x.name == 'importTasksfromTemplate') ? toolbar._options._optionManager._options.items.find(x => x.name == 'importTasksfromTemplate').visible = false : '';
        toolbar._options._optionManager._options.items.find(x => x.name == 'deleteAll') ? toolbar._options._optionManager._options.items.find(x => x.name == 'deleteAll').visible = false : '';

        toolbar._options._optionManager._options.items.find(x => x.name == "scaleDropDown") ? toolbar._options._optionManager._options.items.find(x => x.name == 'scaleDropDown').visible = true : '';

        toolbar._getToolbarItems().filter(x => x.name == 'viewDropDown')[0].options.selectedItemKey = itemKeyView;
        toolbar._options._optionManager._options.items.find(x => x.name == 'moveup') ? toolbar._options._optionManager._options.items.find(x => x.name == 'moveup').visible = false : '';
        toolbar._options._optionManager._options.items.find(x => x.name == 'movedown') ? toolbar._options._optionManager._options.items.find(x => x.name == 'movedown').visible = false : '';
        toolbar._options._optionManager._options.items[21].visible = false;

        toolbar.repaint();

        ganttinstance.repaint();



    }
    function loadGanttView() {

        //if (viewList[1].dxComponent) {
        //    return;
        //}

        ganttDataSource = new DevExpress.data.CustomStore({
            key: "ID",
            load: function (loadOptions) {
                var deferred = $.Deferred();
                $.getJSON(apiUrl, loadOptions, function (data) {
                    gridDataSourceData = data;
                    //$.get(apiSummaryUrl, function (summary, status) {
                    //    $("#divTaskListSummary").dxDataGrid("instance").option('dataSource', [summary]);
                    //});

                    if (Y > 0) {
                        window.scrollTo(0, Y);
                        Y = 0;
                    }
                    return data;
                }).done(function (data) {
                    deferred.resolve({ data: data, totalCount: data.length });
                });
                return deferred.promise();
            },
            update: function (key, values) {
                values = JSON.stringify(values);
                return $.ajax({
                    url: "/api/module/UpdateTaskData?key=" + key + "&values=" + values,
                    method: "POST"
                });
            },
            insert: function (values) {
                values = JSON.stringify(values);
                return $.ajax({
                    url: "/api/module/InsertTaskData?ticketID=<%= TicketID %>" + "&values=" + values,
                    method: "POST"
                });
            },
            remove: function (key) {
                return $.ajax({
                    url: "/api/module/DeleteTaskData?key=" + key,
                    method: "DELETE"
                });
            }
        });
        viewList[1].dxComponent = $("#gantt").dxGantt({
            tasks: {
                dataSource: ganttDataSource,
                endExpr: "DueDate",
                keyExpr: "ID",
                progressExpr: "PercentComplete",
                parentIdExpr: sExpand,
                startExpr: "StartDate",
                titleExpr: "Title"
            },
            export: {
                enabled: true,
                printingEnabled: false
            },
            editing: {
                enabled: true,
                allowTaskAdding: true,
                allowTaskDeleting: true,
                allowTaskUpdating: true,
                allowDependencyAdding: false,
                allowDependencyDeleting: false,
                allowDependencyUpdating: false,
                allowResourceAdding: false,
                allowResourceDeleting: false,
                allowResourceUpdating: false
            },
            contextMenu: {
                enabled: true
            },
            columns: [{
                dataField: "Title",
                caption: "Title",
                width: 290,
                cellTemplate: function (container, options) {
                    var MasterDiv = $(`<div id='dataTitle' class="gantView-gridWrp" style='padding-left:20px; '>`);
                    var params = options.data.ID + "','" + options.data.ItemOrder + "','" + options.data.ParentTaskID + "','" + escape(options.data.Title);
                    var prms = options.rowIndex + "','" + options.data.ID;
                    MasterDiv.append("<span><a href='javascript:void(0);' title='" + options.data.Title + "' onclick=openTask('" + params + "');>" + options.data.Title + "</a></span>");
                    MasterDiv.appendTo(container);
                }
            }, {
                dataField: "StartDate",
                caption: "Start Date",
                dataType: "date",
                width: 100,
            }, {
                dataField: "DueDate",
                caption: "End Date",
                dataType: "date",
                width: 100,
            }, {
                dataField: "PercentComplete",
                caption: "% Complete",
                width: 60,
            }],
            //scrolling: {
            //    showScrollbar: 'never',
            //    useNative: false
            //},
            scaleType: "months", 
            startDateRange: new Date(minStartDate.getFullYear(), minStartDate.getMonth() - 1, 01),
            focusStateEnabled: true,
            hoverStateEnabled: true,
            //width: $(window).width(),
            width: '100%',
            taskListWidth: 600,
            onToolbarPreparing: function (e) {
                e.toolbarOptions.visible = true;
            },
            onSelectionChanged: function (e) {
                e.component._$contextMenu[0].hidden = true;
                if (e.component._dialogInstance != undefined)
                    e.component._dialogInstance._popupInstance.dispose();
            }
        }).dxGantt("instance");
       
        var getCanvas; // global variable
        document.getElementById("btn-Preview-Image").style.display = "block";
        $("#btn-Preview-Image").on('click', function () {
            exportGantt();
        });
        //$("#btn-Preview-Image").on('click', function () {
        //    var mindate = new Date(Math.max.apply(null, gridDataSourceData.map(function (e) {
        //        return new Date(e.DueDate);

        //    })));
        //   let _mindate = formatDate(mindate);
        //    var maxdate = new Date(Math.min.apply(null, gridDataSourceData.map(function (e) {
        //        return new Date(e.StartDate);
        //    })));
        //    let _maxdate = formatDate(maxdate);
        //    const dataRangeMode = 'custom';
        //    let dataRange;
        //    if (dataRangeMode === 'custom') {
        //        dataRange = {
        //            startIndex: "0",
        //            endIndex: "10000",
        //            startDate: _maxdate ,//"12/10/2020",//startdate,_startdate.toString(),
        //            endDate: _mindate //"04/26/2021"//_mindate.toString() ////mindate
        //        }
        //    }
        //    var ganttchart = $("#gantt").dxGantt("instance");
        //    ganttchart.exportToPdf({
        //        format: "auto",
        //        landscape: true,
        //        exportMode: "all",
        //        dateRange: dataRange
            
        //    }).then(doc => {
        //        doc.save(ticketID + '_' + mindate.toLocaleDateString() + '_to_' + maxdate.toLocaleDateString() + '_gantt.pdf');
        //    });
        //});

    }
    function exportGantt() {
        var ganttInstance = $("#gantt").dxGantt("instance");
            var mindate = new Date(Math.min.apply(null, gridDataSourceData.map(function (e) {
                return new Date(e.DueDate);

            })));
            var maxdate = new Date(Math.max.apply(null, gridDataSourceData.map(function (e) {
                return new Date(e.StartDate);
            })));

        DevExpress.pdfExporter.exportGantt({
            component: ganttInstance,
            createDocumentMethod: (args) => new jspdf.jsPDF(args),
            format: 'auto',
            landscape: true,
            exportMode: 'all',
            dateRange: 'all'
        },
        ).then((doc) => {
              doc.save(ticketID + '_' + mindate.toLocaleDateString() + '_to_' + maxdate.toLocaleDateString() + '_gantt.pdf');
        });
    }

    function getLastNode(element) {
        if (element.hasChildren)
            return getLastNode(element.children.reduceRight(x => x))
        else
            return element;
    }

    function isValidDate(str) {
        if (Object.prototype.toString.call(str) === "[object Date]") {
            // it is a date
            if (isNaN(str.getTime())) {
                return false;
            }
            else {
                return true; // date is valid
            }
        }
        else {
            return true; // not a date
        }
    }

    function DifferenceInDays(sDate, dDate) {
        if (sDate == null || dDate == null)
            return 0;
        var StartDate = new Date(sDate);
        var DueDate = new Date(dDate);
        if (StartDate.getDate() == DueDate.getDate())
            return 1;
        var DiffTime = DueDate.getTime() - StartDate.getTime();
        var DiffDays = DiffTime / (1000 * 3600 * 24);
        return Math.round(DiffDays);
    }

    Date.prototype.addDays = function (days) {
        var date = new Date(this.valueOf());
        date.setDate(date.getDate() + days);
        return date;
    }

    function openSaveAsTemplate(ticketID, moduleName) {
        var saveAsTemplatePageUrl = "/layouts/ugovernit/delegatecontrol.aspx?control=savetasktemplates";
        var params = "&projectid=" + ticketID + "&moduleName=" + moduleName;
        window.parent.UgitOpenPopupDialog(saveAsTemplatePageUrl, params, "Save As Template", "400px", "300px", false, "");
    }

    function openImportTemplate(ticketID, moduleName) {
        var saveAsTemplatePageUrl = "/layouts/ugovernit/delegatecontrol.aspx?control=importtasktemplate";
        var params = "&projectid=" + ticketID + "&moduleName=" + moduleName;
        window.parent.UgitOpenPopupDialog(saveAsTemplatePageUrl, params, "Import Task Template", "700px", "550px", false, "");
    }

    //function openLoadTemplate(ticketID, moduleName) {
    //    var saveAsTemplatePageUrl = "/layouts/ugovernit/delegatecontrol.aspx?control=loadtasktemplate";
    //    var params = "&projectid=" + ticketID + "&moduleName=" + moduleName;
    //    window.parent.UgitOpenPopupDialog(saveAsTemplatePageUrl, params, "Load Task Template", "500px", "80", false, "");
    //}

    function openResourceTimeSheet(assignedTo, assignedToName) {
        var url = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGrid&SelectedResource=" + assignedTo;
        window.parent.UgitOpenPopupDialog(url, "", "Timeline for User : " + assignedToName, "95", "95", false, "");
    }

    function bindDatapopup(popupFilters) {
        var titleView = null;
        if ($("#tileViewContainer").length > 0) {

            var titleViewObj = $('#tileViewContainer').dxTileView('instance');
            if (titleViewObj) {
                titleViewObj.option("dataSource", "/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters));
                titleViewObj._refresh();
            }
        }
        else {

            titleView = $("<div id='tileViewContainer' style='clear:both;padding-top: 30px' />").dxTileView({
                height: window.innerHeight - 210,
                //width: window.innerWidth - 250,
                showScrollbar: true,
                baseItemHeight: 65,
                baseItemWidth: 150,
                itemMargin: 15,
                direction: "vertical",
                // showScrollbar: true,
                elementAttr: { "class": "tileViewContainer" },
                noDataText: "No resource available",
                dataSource: "/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters),
                itemTemplate: function (itemData, itemIndex, itemElement) {
                    var html = new Array();
                    html.push("<div class='UserDetails'>");
                    html.push("<div id='" + itemData.AssignedTo + "'>");
                    //   html.push("<div id='" + itemData.AssignedTo + "'>");
                    html.push("<div class='AssignedToName'>");
                    html.push(itemData.AssignedToName);
                    html.push("</div>");
                    if (itemData.PctAllocation >= 100) {
                        html.push("<div>");
                        html.push("(Over Allocated: " + itemData.PctAllocation + "%)");
                        html.push("</div>");
                    }
                    else if (itemData.PctAllocation > 0) {
                        html.push("<div style='padding-bottom:3px;'>");
                        html.push("(" + (100 - Number(itemData.PctAllocation)) + "%)");
                        html.push("</div>");
                    }
                    if (popupFilters.projectCount || popupFilters.projectVolume) {
                        if (itemData.PctAllocation > 0) {
                            html.push("<div class='capacitymain'>");
                            html.push("<div class='capacityblock allocation-v" + itemData.TotalVolumeRange + "'>");
                            html.push(itemData.TotalVolume);
                            html.push("</div>");

                            html.push("<div class='capacityblock allocation-c" + itemData.projectCountRange + "''>");
                            html.push("#" + itemData.ProjectCount);
                            html.push("</div>");
                            html.push("</div>");
                        }
                    }
                    html.push("</div>");
                    html.push("</div>");

                    itemElement.attr("class", "allocation-block allocation-r" + itemData.AllocationRange);
                    itemElement.append(html.join(""));

                },
                onItemClick: function (e) {
                    var data = e.itemData;
                    var element = _.findWhere(gridDataSource._items, { key: parseInt(popupFilters.ID) });
                    var treeList = $("#tree-list").dxTreeList("instance");
                    if (typeof element != "undefined")
                        treeList.cellValue(treeList.getRowIndexByKey(element.key), "AssignToPct", data.AssignedTo + ";~100;~" + data.AssignedToName);
                    else {
                        var preData = treeList.cellValue(tasklistrowindex, "AssignToPct");
                        var assignedUsers = ast_convertStrObject(preData);
                        if (!assignedUsers)
                            assignedUsers = [];
                        if (_.findWhere(assignedUsers, { id: data.AssignedTo }) == null) {
                            assignedUsers.push({ id: data.AssignedTo, pct: "100", displayValue: data.AssignedToName });
                        }

                        var userStrFormat = ast_convertObjToStr(assignedUsers);
                        treeList.cellValue(tasklistrowindex, "AssignToPct", userStrFormat);
                    }

                    $('#popupContainer').dxPopup('instance').hide();
                }

            });
        }

        return titleView;
    };

    $(document).on("click", "img.assigneeToImg", function (e) {
        var groupid = $(this).attr("id");
        var dataid = $(this).parents().eq(3).attr('data-row-index');// e.target.id;
        var data = _.find(gridDataSourceData, function (s) { return gridDataSourceData[dataid]; }); // _.find(gridDataSource._items, function (s) { return s.key.toString() === dataid; });

        var row = gridDataSourceData[dataid];

        popupFilters.projectID = ticketID;
        popupFilters.resourceAvailability = 2;
        popupFilters.complexity = false;
        popupFilters.projectVolume = false;
        popupFilters.projectCount = false;
        popupFilters.RequestTypes = false;
        popupFilters.groupID = $(this).attr("group");

        //popupFilters.allocationStartDate = $(this).attr("startDate");
        //popupFilters.allocationEndDate = $(this).attr("endDate");
        if (row != undefined) {
            popupFilters.allocationStartDate = row.StartDate;
            popupFilters.allocationEndDate = row.DueDate;
        }
        else {
            popupFilters.allocationStartDate = $(this).attr("startDate");
            popupFilters.allocationEndDate = $(this).attr("endDate");
        }

        if (ticketID.startsWith("OPM") || ticketID.startsWith("PMM")) {
            popupFilters.ModuleIncludes = true;
        }
        else {
            popupFilters.ModuleIncludes = false;
        }
        popupFilters.JobTitles = "";
        popupFilters.departments = "";
        popupFilters.SelectedUserID = "";
        popupFilters.ID = $(this).attr("ID");
        RowId = $(this).attr("ID");
        var popupTitle = "Available Resource";
        if (data && data.TypeName)
            popupTitle = "Available " + data.TypeName + "s";

        $("#popupContainer").dxPopup({
            title: popupTitle,
            width: "85%",
            height: "90%",
            visible: true,
            scrolling: true,
            contentTemplate: function (contentElement) {
                contentElement.append(
                    $("<div class='col-md-6 colsm-6 col-xs-12 devExtRadioGroup' />").dxRadioGroup({
                        dataSource: radioGroupItems,
                        displayExpr: "text",
                        value: _.findWhere(radioGroupItems, { value: popupFilters.resourceAvailability }),
                        layout: "horizontal",
                        onValueChanged: function (e) {
                            popupFilters.resourceAvailability = e.value.value;
                            bindDatapopup(popupFilters);
                        }
                    }),
                    $("<div class='col-md-6 col-sm-6 col-xs-12 devExtChkBox-wrap'>").append(
                        $("<div id='projecttype' class='chkFilterCheck col-md-3 noPadding'  />").dxCheckBox({
                            text: "Project Type",
                            onValueChanged: function (e) {
                                popupFilters.RequestTypes = e.value;
                                bindDatapopup(popupFilters);
                            }
                        }),

                        $("<div id='capacity' class='chkFilterCheck col-md-3 noPadding' />").dxCheckBox({
                            text: "Capacity",
                            value: popupFilters.projectVolume,
                            onValueChanged: function (e) {
                                popupFilters.projectVolume = e.value;
                                popupFilters.projectCount = e.value;
                                bindDatapopup(popupFilters);
                            }
                        }),

                        $("<div id='complexity' class='chkFilterCheck col-md-3 noPadding' />").dxCheckBox({
                            text: "Complexity",
                            value: popupFilters.complexity,
                            onValueChanged: function (e) {

                                popupFilters.complexity = e.value;
                                bindDatapopup(popupFilters);
                            },
                        }),

                        $("<div id='opportunities' class='chkFilterCheck col-md-3 noPadding'  />").dxCheckBox({
                            text: "Opportunities",
                            value: popupFilters.ModuleIncludes,
                            onValueChanged: function (e) {
                                popupFilters.ModuleIncludes = e.value;
                                bindDatapopup(popupFilters);
                            }
                        }),

                    ),


                    $("<div class='filterctrl-jobDepartment col-md-4 col-sm-6 col-xs-12' />").dxSelectBox({
                        placeholder: "Limit By Department",
                        valueExpr: "ID",
                        displayExpr: "Title",
                        dataSource: "/api/rmmapi/GetDepartments?GroupID=" + popupFilters.groupID,
                        onSelectionChanged: function (selectedItems) {
                            var items = selectedItems.selectedItem.ID;
                            var tagBox = $("#tagboxTitles").dxTagBox("instance");
                            popupFilters.departments = items;
                            $.get("/api/rmmapi/GetGroupTitles?GroupID=" + popupFilters.groupID + "&DepartmentID=" + items, function (data, status) {
                                JobTitleData = data;
                                tagBox.option("dataSource", JobTitleData);
                                tagBox.reset();
                            });

                            bindDatapopup(popupFilters);
                        },
                        onContentReady: function (e) {

                        }
                    }),
                    $("<div id='tagboxTitles'  class='filterctrl-jobtitle col-md-4 col-sm-6 col-xs-12 noPadding'/>").dxTagBox({
                        text: "Job Titles",
                        placeholder: "Limit By Job Title",
                        //valueExpr: "Title",
                        //displayExpr: "Title",

                        dataSource: "/api/rmmapi/GetGroupTitles?GroupID=" + popupFilters.groupID + "&DepartmentID=0",
                        maxDisplayedTags: 2,
                        onSelectionChanged: function (selectedItems) {
                            var items = selectedItems.component._selectedItems;
                            if (items.length > 0) {
                                var lstItems = items.map(function (i) {
                                    return i;
                                });
                                popupFilters.JobTitles = lstItems.join(';#');
                            }
                            else {
                                popupFilters.JobTitles = '';
                            }
                            bindDatapopup(popupFilters);
                        }
                    }),

                    $("<div class='filterctrl-userpicker col-md-4 col-sm-6 col-xs-12 noPadding' />").dxSelectBox({
                        placeholder: "Choose Any User",
                        valueExpr: "Id",
                        displayExpr: "Name",
                        searchEnabled: true,
                        showClearButton: true,
                        dataSource: "/api/rmmapi/GetUserList",
                        onSelectionChanged: function (selectedItems) {
                            if (selectedItems.selectedItem == null) {
                                popupFilters.SelectedUserID = "";
                                popupFilters.complexity = false;
                                //var checkcomplexity = $("#complexity").dxCheckBox("instance");
                                //checkcomplexity.option("value", true);
                                bindDatapopup(popupFilters);
                            } else {
                                var items = selectedItems.selectedItem.Id;
                                popupFilters.SelectedUserID = items;
                                popupFilters.complexity = false;
                                bindDatapopup(popupFilters);
                            }
                        }
                    }),
                    bindDatapopup(popupFilters)
                )

            },
            itemClick: function (e) {

            },
            onContentReady: function (e) {
                strResourceSelectFilter = '<%=ResourceSelectFilter%>';
                var arrResourceSelectFilter = [];
                if (strResourceSelectFilter)
                    arrResourceSelectFilter = strResourceSelectFilter.split(';#');

                arrResourceSelectFilter.forEach(function (x, i) {
                    var arrfilter = x.split('=');
                    if (arrfilter && arrfilter.length == 2 && arrfilter[1] == 'true') {
                        var chck = $('#' + arrfilter[0].toLowerCase());
                        chck.css({ 'display': 'block' });
                    }
                    else {
                        var chck = $('#' + arrfilter[0].toLowerCase());
                        chck.css({ 'display': 'none' });
                    }
                });
            }
        });
        var popupInstance = $('#popupContainer').dxPopup('instance');
        popupInstance.option('title', popupTitle);
    });

    function openTask(taskId, itemOrder, parentTaskID, taskTitle) {
        var path = "/layouts/ugovernit/delegatecontrol.aspx?control=taskedit";
        var params = "&taskID=" + taskId + "&parentTaskID=" + parentTaskID + "&ItemOrder=" + itemOrder + "&moduleName=" + "<%= ModuleName %>" + "&ticketId=" + "<%= TicketID %>" + "&taskIndex=" + window.pageYOffset + "&folderName=&isTabActive=true";

        if (taskTitle != null && taskTitle != undefined && taskTitle != "") {
            taskTitle = unescape(taskTitle);
            var maxLength = 70;

            if (taskTitle.length > maxLength)
                taskTitle = taskTitle.substring(0, maxLength) + "...";
        }
        else {
            taskTitle = "";
        }
        window.parent.UgitOpenPopupDialog(path, params, "Edit Task: " + taskTitle, "70", "90", false);
    }

    $(function () {
        loadKanBanView();
    });
    function loadKanBanView() {
        $.ajax({
            type: "POST",
            url: "/fetchKanbanView/GetKanBanViewHtmlData",
            data: '{TicketId: "' + ticketID + '" }',
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $('#kanbanView').append(response);
                return true;
            },
            failure: function (response) {

            },
            error: function (response) {

            }
        });
    }

    function ast_convertStrObject(assigneeTo) {
        if (!assigneeTo)
            return null;
        var userObj = [];
        var usersWithPct = assigneeTo.split(";#");
        usersWithPct.forEach(function (s) {
            var upct = s.split(";~");
            var u = {};
            u.id = upct[0];
            u.pct = upct[1];
            if (!upct[1])
                u.pct = "100";
            u.displayValue = upct[0];
            if (upct.length >= 3) {
                u.displayValue = upct[2];
            }

            userObj.push(u);
        });
        return userObj;
    }

    function ast_convertObjToStr(assigneeTo) {
        if (!assigneeTo)
            return null;
        var userObj = [];

        assigneeTo.forEach(function (s) {
            var upct = [];
            upct.push(s.id);
            if (!s.pct)
                s.pct = "100";
            upct.push(s.pct);
            if (s.displayValue)
                upct.push(s.displayValue)
            userObj.push(upct.join(";~"));
        });
        return userObj.join(";#");
    }

    function ast_txtboxCreateSelection(field, start, end) {
        if (field.createTextRange) {
            var selRange = field.createTextRange();
            selRange.collapse(true);
            selRange.moveStart('character', start);
            selRange.moveEnd('character', end);
            selRange.select();
            field.focus();
        } else if (field.setSelectionRange) {
            field.focus();
            field.setSelectionRange(start, end);
        } else if (typeof field.selectionStart != 'undefined') {
            field.selectionStart = start;
            field.selectionEnd = end;
            field.focus();
        }
    }

    var ast_dataSource = new DevExpress.data.DataSource({
        store: new DevExpress.data.CustomStore({
            key: "id",
            loadMode: "raw",
            load: function (loadOptions) {
                return $.get("/api/rmmapi/GetUsersInfo");
            }
        }),
        pageSize: 15,
        paginate: true
    });

    function expand(div) {
        var TaskID = div.attributes.dataKey.value;
        var treelist = $("#tree-list").dxTreeList('instance');
        treelist.expandRow(TaskID);
    }

    function collapse(div) {
        var TaskID = div.attributes.dataKey.value;
        var treelist = $("#tree-list").dxTreeList('instance');
        treelist.collapseRow(TaskID);

    }
    function addNew(a, b) {
        //var treeL = $("#tree-list").dxTreeList("instance");
        //treeL.addRow(b);
        var values = {
            ID: a == '' ? 0 : a,
            PercentComplete: 0,
            Status: "Not Started",
            StartDate: new Date(),
            DueDate: new Date(),
            EstimatedHours: 0,
            ActualHours: 0,
            EstimatedRemainingHours: 0,
            Duration: 1,
            Contribution: 0,
            Title: b == 0 ? "NewTask" : "NewChildTask",
            ParentTaskID: b
        }
        $.ajax({
            url: ugitConfig.apiBaseUrl + "/api/module/InsertTask",
            type: "POST",
            data: { values: JSON.stringify(values), TicketId: '<%= TicketID%>' },
            async: false,
            success: function (response) {
                LoadTaskTreeList(TaskParameter);
                var treeL = $("#tree-list").dxTreeList("instance");
                if (typeof response != "undefined") {
                    response.forEach(function (key) {
                        treeL.expandRow(key);
                        treeL.option("focusedRowKey", response);
                    });
                }
                treeL.clearSelection();
                //treeL.expandRow(35390);
                //treeL.expandRow(35392);
                //treeL.expandRow(35405);
            }
        });
    }

    function MarkTaskAsComplete(index, taskID) {
        var treeL = $("#tree-list").dxTreeList("instance");
        //var selectedRowskeys = treeL.getSelectedRowKeys("all");
        //if (selectedRowskeys.length > 0) {

        $.ajax({
            url: ugitConfig.apiBaseUrl + "/api/module/MarkTaskAsComplete",
            method: "POST",
            data: { TaskKeys: taskID, TicketPublicId: '<%= TicketID%>', TaskType: "ModuleTasks" },
            success: function (data) {
                treeL.clearSelection();

                //if (IsCompactView.toLowerCase() == "true")
                // RepaintTaskWorkflow();

                treeL.refresh();
                loadPanel.hide();
            },
            error: function (error) { }
        })
        //}
    }


    $(document).ready(function () {
        var parentIframe = window.parent.document;
        $(window.frameElement).addClass('tsk-parentFram');
        IframeHeight = parseInt($(window.frameElement).height());
        maxHeightOfTaskListGrid = IframeHeight - (parseInt($('#projectToolBar').height()) + parseInt($('#toolbar').height()));
        //if (maxHeightOfTaskListGrid == undefined || isNaN(maxHeightOfTaskListGrid) || maxHeightOfTaskListGrid == '' ) {
        //    maxHeightOfTaskListGrid = 470;
        //}
        //console.log(maxHeightOfTaskListGrid, "indocuments maxHeightOfTaskListGrid");
        $('#tree-list').css({ 'max-height': (maxHeightOfTaskListGrid - 160) + 'px' });

    });

    var reportType = '';
    function onReportItemClick(args) {
        if (args.itemData.name) {
            reportType = args.itemData.name;
            GetSelectedProjects();
        }
    }
    function GetSelectedProjects() {
        var Id = <%=Ids%>;
        values = '<%=TicketID%>';
        var params = "";
        if (values.length > 0)
            params = "alltickets=" + values;
        var moduleName = "<%= ModuleName %>";
        var url = '<%= reportUrl %>' + "?reportName=" + reportType + "&Module=" + moduleName;
        var title = '';

        switch (reportType) {
            case "ProjectStatusReport":
                var url = '';
                if (moduleName == 'TSK') {
                    url = '<%= reportUrl %>' + "?reportName=tskprojectreportview&TSKId=" + Id + "&Module=" + moduleName + '&userId=<%=userID %>' + "&" + params;
                } else {
                    url = '<%= reportUrl %>' + "?reportName=" + reportType + "&Module=" + moduleName + '&userId=<%=userID %>' + "&" + params;
                }
                var popupHeader = "Project Status Report";
                params = params + "&individual=&Filter=";
                window.parent.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
                break;
            case "OnePagerReport":
                var url = '<%= reportUrl %>' + "?reportName=" + reportType + "&Module=" + moduleName + '&userId=<%= userID %>' + "&" + params;
                var popupHeader = "1-Pager Project Report";
                params = params + "&Viewer=&PMMIds=" + '<%=Ids%>';
                window.parent.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
                break;
            case "ProjectResourceReport":

                var url = '<%=reportUrl%>' + "?reportName=" + reportType + "&Module=" + moduleName + '&userId=<%= userID %>' + "&" + params;
                var popupHeader = "Resource Hours Report";
                params = params + "&Filter=";
                window.parent.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
                break;
            case "EstimatedRemainingHoursReport":
                var url = '<%=reportUrl%>' + "?reportName=" + reportType + "&Module=" + moduleName + '&userId=<%= userID %>' + "&" + params;
                var popupHeader = "Estimated Remaining Hours Report";
                params = params + "&Filter=";
                window.parent.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
                break;
            case "ProjectBudgetReport":
                var url = '<%=reportUrl%>' + "?reportName=" + reportType + "&Module=" + moduleName + '&userId=<%= userID %>' + "&" + params;
                var popupHeader = "Project Budget Summary Report";
                if (moduleName == 'ITG')
                    popupHeader = 'Non-Project Budget Summary Report';
                params = params + "&TicketId=" + '<%=TicketID%>' + "&Filter=";
                window.parent.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
                break;
            case "ProjectStageHistory":
                OpenTrackprojectStageReport();
                break;
            case "ProjectActualsReport":
                var url = '<%=reportUrl%>' + "?reportName=" + reportType + "&Module=" + moduleName + '&userId=<%= userID %>' + "&" + params;
                var popupHeader = "ProjectActualsReport";
                params = params + "&Filter=";
                window.parent.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
            default:
                break;
        }

        return false;
    }

    function OpenTrackprojectStageReport() {
        var trackProjectStageUrl = '<%=TrackProjectStageUrl%>&publicTicketId=<%=TicketID%>';
        window.parent.UgitOpenPopupDialog(trackProjectStageUrl, '', 'Project Stage History', '800px', '500px');
    }
    function onNewTaskClick(args) { //kopo
        var newTaskType = "";
        if (args.itemData.name) {
            newTaskType = args.itemData.name;
            newTaskActions(newTaskType);
        }
    }
    function formatDate(date) {
        const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

        var d = new Date(date),
            month = months[d.getMonth()],
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (day.length < 2)
            day = '0' + day;
        let formatted_date = month + ' ' + day + ', ' + year;
        return formatted_date;
    }

    function newTaskActions(newTaskType) {
        var treeL = $("#tree-list").dxTreeList("instance");
        var dataSource = treeL.getDataSource();
        let itemorder = 0;
        switch (newTaskType) {
            case "NewTaskAtEnd":
                itemorder = dataSource._items.length;
                dataSource.store().push([
                    { type: "insert", key: -(itemorder), data: { ID: -(itemorder), PercentComplete: null, Status: null, StartDate: null, DueDate: null, EstimatedHours: null, ActualHours: null, EstimatedRemainingHours: null, Duration: null, Contribution: 0, Title: "", ParentTaskID: 0 } }
                ]);
                break;
            case "NewTaskBelowCurrent":
                var selectedRowskeys = treeL.getSelectedRowKeys("all");
                var itemId = selectedRowskeys[0]; 
                addNew(-itemId, 0);
                break;
            case "NewSubTask":
                var selectedRowskeys = treeL.getSelectedRowKeys("all");
                addNew(0, selectedRowskeys[0]);
                break;
            case "NewRecurrringTask":
                var path = "/layouts/ugovernit/delegatecontrol.aspx?control=taskedit";
                var params = "&RepeatableTask=true&parentTaskID=0&moduleName=" + "<%= ModuleName %>" + "&ticketId=" + "<%= TicketID %>" + "&folderName=&isTabActive=true";
                window.parent.UgitOpenPopupDialog(path, params, "New Recurring Task", "90", "90", false);
                break;
            default:
                break;                
        }
    }


</script>
<div id="toolbar" class="toolbar"></div>

<div id="contextMenuContainer" class="contextMenuContainer"></div>

<div id="popupContainer"></div>

<div id="mppfileuploaderpopup"></div>
<input id="btn-Preview-Image" type="button" value="Get Pdf" style="display: none" class="" />
<div id="output"></div>
<%--<a id="btn-Convert-Html2Image" href="#">Download</a>
<br />
<h3>Preview :</h3>
<div id="previewImage">
</div>--%>
<div id="GanttSwitch" style="margin-bottom: 6px"></div>
<div id="gantt" class="gantt-container"></div>

<div id="treelist" class="pmmEdit-listGridWrap" style="width: 100%">
    <div id="tree-list" class="pmmEdit-listGrid"></div>
    <div id="divTaskListSummary"></div>
    <div>
        <div id="projectTeam_linkWrap" class="pmmEdit-linkWrap">
            <div id="btnAdd" class="btnAddNew"></div>
            <div id="btnSave" class="btnAddNew"></div>
            <div id="btnDiscard" class="btnAddNew"></div>
        </div>
    </div>
</div>
<div id="kanbanView"></div>
<div id="loadpanel"></div>

