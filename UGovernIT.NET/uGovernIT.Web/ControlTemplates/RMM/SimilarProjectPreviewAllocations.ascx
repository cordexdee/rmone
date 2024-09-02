<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SimilarProjectPreviewAllocations.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMM.SimilarProjectPreviewAllocations" %>
<%@ Register Src="~/ControlTemplates/RMM/SaveAllocationAsTemplate.ascx" TagPrefix="ugit" TagName="SaveAllocationAsTemplate" %>
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

    .custom-task {
        max-height: 28px;
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

    .marcal {
        float: left;
        clear: both;
        width: 100%;
        text-align: center;
        margin-bottom: 240px;
        margin-top: 20px;
    }    
</style>

<script id="dxss_inlineCtrScriptResource" data-v="<%=UGITUtility.AssemblyVersion %>">



    var radioGroupItems = [
        { text: "Fully Available", value: 0 },
        { text: "Partially Available", value: 1 },
        { text: "All Resources", value: 2 }
    ];
    var checkboxGroupItems = [

        { text: "1", value: "Complexity" },
        { text: "2", value: "Count" },
        { text: "3", value: "Voulme" }
    ];


    var popupFilters = {};
    var IsFirstRequest = false;
    var GroupsData = [];
    var UsersData = [];
    var projectID = "<%= Request["ActualProjectID"]%>" == "" ? "<%= ActualProjectID%>" : "<%= Request["ActualProjectID"]%>";
    var globaldata = [];
    var validateOverlap = false;
    var JobTitleData = [];
    var UserProfileData = [];
    var showTimeSheet = false;

    $(function () {
        var RowId = 0;
        var AssignedToName = "";
        var resultData = [];
        var typename;

        $("#toast").dxToast({
            message: "Allocations Saved Successfully. ",
            type: "info",
            displayTime: 600,
            position: "{ my: 'center', at: 'center', of: window }"
        });
        $("#toastOverlap").dxToast({
            message: "Some Allocations are Overlap with Exiting, Save process unsuccessful. ",
            type: "info",
            displayTime: 600,
            position: "{ my: 'center', at: 'center', of: window }"
        });

        $("#toastBlankAllocation").dxToast({
            message: "Some Allocations are Blank, Save process unsuccessful. ",
            type: "info",
            displayTime: 600,
            position: "{ my: 'center', at: 'center', of: window }"
        });

        //$("#toastFindResource").dxToast({
        //    message: "Finding the best resource...",
        //    type: "info",
        //    displayTime: 1000,
        //    position: "{ my: 'center', at: 'center', of: window }"
        //});

        $.get("/api/rmmapi/GetProjectAllocations?projectID=<%= ticketID %>", function (data, status) {
             
            for (let i = 0; i < data.length; i++) {
                data[i]['AssignedTo'] = '';
                data[i]['AssignedToName'] = '';
            }

            globaldata = data;
            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
            dataGrid.option("dataSource", globaldata);

        });

        $("#btnAddNew").dxButton({

            text: "Add New Allocation",
            icon: "/content/Images/plus-blue.png",
            focusStateEnabled: false,
            onClick: function (s, e) {
                var projectStartdate = '<%= StartDateString%>';
                var projectEnddate = '<%= EndDateString%>';
                var grid = $("#gridTemplateContainer").dxDataGrid("instance");
                var sum = 0;
                sum = globaldata.length + 1;
                globaldata.push({ "ID": -Math.abs(sum), "AssignedTo": "", "AssignedToName": "", "AllocationStartDate": projectStartdate, "AllocationEndDate": projectEnddate, "PctAllocation": 100, "Type": GroupsData[0].Id, "TypeName": GroupsData[0].Name });
                grid.option("dataSource", globaldata);

                $.cookie("dataChanged", 1, { path: "/" });
                $.cookie("projTeamAllocSaved", 0, { path: "/" });
            }
        });


        //$("#btnProceed").dxButton({
        //    text: "Save Allocations",
        //    icon: "/content/Images/save-open-new-wind.png",
        //    focusStateEnabled: false,
        //    onClick: function (e) {
        //        var isEmptyField = false;
        //        var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
        //        dataGrid.saveEditData();
        //        $("#loadpanel").dxLoadPanel({
        //            message: "Loading...",
        //            visible: true
        //        });

                
        //        var Preconstartdate;
        //        if (typeof (changedDates.PreConStart) == "object")
        //            Preconstartdate = changedDates.PreConStart.toISOString();
        //        var Preconenddate;
        //        if (typeof (changedDates.PreConEnd) == "object")
        //            Preconenddate = changedDates.PreConEnd.toISOString();
        //        var constartdate;
        //        if (typeof (changedDates.ConstStart) == "object")
        //            constartdate = changedDates.ConstStart.toISOString();
        //        var constenddate;
        //        if (typeof (changedDates.ConstEnd) == "object")
        //            constenddate = changedDates.ConstEnd.toISOString();

        //        $.each(globaldata, function (i, s) {
        //            if (typeof (s.AllocationStartDate) == "object") {
        //                if (s.AllocationStartDate != null) {
        //                    s.AllocationStartDate = (s.AllocationStartDate).toISOString();
        //                }
        //                else {
        //                    isEmptyField = true;
        //                    DevExpress.ui.dialog.alert("Start Date is Empty.", "Alert!");
        //                    return false;
        //                }
        //            }

        //            if (typeof (s.AllocationEndDate) == "object") {
        //                if (s.AllocationEndDate != null) {
        //                    s.AllocationEndDate = (s.AllocationEndDate).toISOString();
        //                }
        //                else {
        //                    isEmptyField = true;
        //                    DevExpress.ui.dialog.alert("End Date is Empty.", "Alert!");
        //                    return false;
        //                }
        //            }

        //        });

        //        if (isEmptyField == true) {
        //            $("#loadpanel").dxLoadPanel("hide");
        //            isEmptyField = false;
        //            return false;
        //        }

        //        $.post("/api/rmmapi/UpdateBatchCRMAllocations/", { Allocations: globaldata, ProjectID: projectID, PreConStart: Preconstartdate, PreConEnd: Preconenddate, ConstStart: constartdate, ConstEnd: constenddate }).then(function (response) {
        //            //$("#loadpanel").dxLoadPanel("hide");
        //            //if (response.includes("BlankAllocation")) {
        //            //    dataGrid.option("dataSource", globaldata);
        //            //    dataGrid._refresh();
        //            //    $("#toastBlankAllocation").dxToast("show");
        //            //}
        //            //else if (response.includes("OverlappingAllocation")) {
        //            //    var resultvalues = response.split(":");
        //            //    var datakey = _.findWhere(globaldata, { ID: parseInt(resultvalues[1]) });
        //            //    var rowIndex = dataGrid.getRowIndexByKey(datakey);
        //            //    //dataGrid.getCellElement(rowIndex, "AssignedTo").find("input").css("border", "1px solid red");  
        //            //    DevExpress.ui.dialog.alert("Overlapping allocations are not permitted. Save unsuccessful.", "Error");
        //            //    dataGrid.getRowElement(rowIndex).css("background-color", "#FFCCCB");
        //            //    //$("#toastOverlap").dxToast("show");
        //            //}
        //            //else {
        //            //    dataGrid.option("dataSource", response);
        //            //    dataGrid._refresh();
        //            //    $("#toast").dxToast("show");

        //            //    //$.cookie("dataChanged", 0, { path: "/" });
        //            //}
        //            //$.cookie("dataChanged", 0, { path: "/" });
        //            //$.cookie("projTeamAllocSaved", 1, { path: "/" });

        //            window.parent.CloseWindowCallback(1, document.location.href);
        //        }, function (error) { $("#loadpanel").dxLoadPanel("hide"); });

        //        e.event.stopPropagation();

        //    }
        //});


        $("#btnProceed").dxButton({
            text: "Allocate Resources",
            icon: "/content/Images/ResourceMgmtBlue.png",
            onClick: function (e) {
                var projectStartdate = '<%= StartDate%>';
                   var projectEnddate = '<%= EndDate%>';
                   var result = DevExpress.ui.dialog.confirm('Selected Allocations will override current allocations. Do you want to proceed?', 'Confirm');
                   result.done(function (dialogResult) {
                       if (dialogResult) { // Yes: confirm close
                           for (let i = 0; i < globaldata.length; i++) {
                               globaldata[i]['ProjectID'] = projectID;
                           }
                           Object.keys(globaldata).forEach((key) => (globaldata[key] == null) && delete globaldata[key]);

                           $("#loadpanel").dxLoadPanel({
                               message: "Please wait...",
                               visible: true
                           });

                           $.post("/api/rmmapi/UpdateFromTemplateAllocation", { Allocations: globaldata, ProjectStartDate: projectStartdate, ProjectEndDate: projectEnddate }).then(function (response) {

                               window.parent.parent.parent.CloseWindowCallback(1, "");                               

                           }, function (error) { });
                       }
                       else {
                           //options.cancel = true;
                       }
                   });
               }
        });

        $("#btnAutofillAllocations").dxButton({
            text: "Autofill Allocations",
            icon: "/content/Images/autofill-allocations.png",
            onClick: function (e) {
                for (let i = 0; i < globaldata.length; i++) {
                    globaldata[i]['ProjectID'] = projectID;
                }

                $("#loadpanel").dxLoadPanel({
                    message: "Finding the best resource...",
                    visible: true
                });

                $.post("/api/rmmapi/SelectMostAvailableResource", { TemplateAllocations: globaldata }).then(function (response) {

                    var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                    dataGrid.option("dataSource", response);
                    globaldata = response;

                    $("#loadpanel").dxLoadPanel({visible: false});
                }, function (error) { }).done(function () { $("#loadPanel").dxLoadPanel({ visible: false }); }).fail(function () { $("#loadPanel").dxLoadPanel({ visible: false }); });
            }
        });

        $("#btnUpdateSchedule").dxButton({
            text: "Reschedule",
            icon: "/content/Images/calender_active.png",
            onClick: function (e) {

                const popup = $("#UpdateDatesPopup").dxPopup({
                    contentTemplate: popupContentTemplate,
                    width: "30%",
                    height: 300,
                    showTitle: true,
                    visible: false,
                    dragEnabled: true,
                    showTitle: false,
                    hideOnOutsideClick: true,
                    showCloseButton: true,
                    position: {
                        at: 'center',
                        my: 'center',
                    },
                }).dxPopup('instance');

                popup.option({
                    contentTemplate: () => popupContentTemplate()

                });
                popup.show();

            }
        });

        $("#btnCancelChanges").dxButton({
            text: 'Cancel',
            icon: 'undo',
            onClick: function (e) {
                var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                dataGrid.cancelEditData();
            }
        });

        $("#gridTemplateContainer").dxDataGrid({
            //columnHidingEnabled: true,
            dataSource: globaldata,
            ID: "grdTemplate",
            editing: {
                mode: "cell",
                allowEditing: true,
                allowUpdating: true
            },
            sorting: {
                mode: "multiple" // or "multiple" | "none"
            },
            scrolling: {
                mode: 'infinite',
            },
            columns: [
                {
                    dataField: "AssignedToName",
                    dataType: "text",
                    caption: "Assigned To",
                    allowEditing: false,
                    sortIndex: "0",
                    sortOrder: "asc",
                    cellTemplate: function (container, options) {
                        $('.dx-header-row').addClass('devExtDataGrid-headerRow');
                        $('.dx-data-row').addClass('devExtDataGrid-DataRow');
                        if (options.key.ID > 0) {

                            var str = options.data.AssignedTo + "','" + options.data.AssignedToName;
                            var strwithspace = str.replace(" ", "&nbsp;")
                            $("<div id='dataId'>")
                                .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=openResourceTimeSheet('" + strwithspace + "');>" + options.value + "</a></span>")
                                .append($("<img>", { "src": "/content/images/moreoptions_blue.png", "ID": options.data.ID, "group": options.data.Type, "startDate": options.data.AllocationStartDate, "endDate": options.data.AllocationEndDate, "style": "float: right;overflow: auto;cursor: pointer;", "class": "assigneeToImg" }))
                                .appendTo(container);
                        }

                        if (options.key.ID <= 0) {
                            var str = options.data.AssignedTo + "','" + options.data.AssignedToName;
                            var strwithspace = str.replace(" ", "&nbsp;")
                            $("<div id='dataId'>")
                                .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=openResourceTimeSheet('" + strwithspace + "');>" + options.value + "</a></span>")
                                .append($("<img>", { "src": "/content/images/moreoptions_blue.png", "ID": options.data.ID, "group": options.data.Type, "startDate": options.data.AllocationStartDate, "endDate": options.data.AllocationEndDate, "style": "float: right;overflow: auto;cursor: pointer;", "class": "assigneeToImg" }))
                                .appendTo(container);
                        }
                    }
                },
                {
                    dataField: "TypeName",
                    dataType: "text",
                    caption: "Role",
                    sortIndex: "1",
                    sortOrder: "asc",
                    width: "30%",
                    cssClass: "cls",
                },
                {
                    dataField: "Type",
                    dataType: "text",
                    visible: false
                },
                {
                    dataField: "AllocationStartDate",
                    caption: "Start Date",
                    dataType: "date",
                    width: "10%",
                    validationRules: [{ type: "required" }],
                    format: 'MMM d, yyyy',
                    sortIndex: "2",
                    sortOrder: "asc",
                    editorOptions: {
                        onFocusOut: function (e) {
                            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                            dataGrid.saveEditData();
                        },
                        onClosed: function (e) {
                            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                            dataGrid.saveEditData();
                        }
                    }
                },
                {
                    dataField: "AllocationEndDate",
                    caption: "End Date",
                    dataType: "date",
                    width: "10%",
                    validationRules: [{ type: "required" }],
                    format: 'MMM d, yyyy',
                    editorOptions: {
                        onFocusOut: function (e) {
                            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                            dataGrid.saveEditData();
                        },
                        onClosed: function (e) {
                            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                            dataGrid.saveEditData();
                        }
                    }
                },
                {
                    dataField: "PctAllocation",
                    caption: "% Alloc",
                    dataType: "text",
                    width: "5%"
                },
                {
                    width: "7%",
                    <%--visible:<%=IsGroupAdmin.ToString().ToLower()%>,   // need this line if we want to make delete based on admin group--%>
                    cellTemplate: function (container, options) {
                        var preconStartdate = '<%= PreConStartDate%>';
                        var preconEnddate = '<%= PreConEndDate%>';
                        if (preconEnddate == 'Jan 1, 0001' || preconStartdate == 'Jan 1, 0001') {
                            $("<div id='rowDelete' style='text-align:center;'>")
                                .append($("<img>", { "src": "/content/images/redNew_delete.png", "ID": options.data.ID, "TemplateID": options.data.TemplateID, "UserID": options.data.AssignedTo, "style": "overflow: auto;cursor: pointer;padding-left:5px;", "class": "imgDelete", "title": "Delete" }))
                                .appendTo(container);
                        } else {
                            $("<div id='rowDelete' style='text-align:center;'>")
                                //.append($("<img>", { "src": "/Content/images/PreconCal2.png", "Index": options.rowIndex, "style": "overflow: auto;cursor: pointer;height:20px;width:20px;", "class": "imgPreconDate", "title":"Set Precon Date" }))
                                .append($("<img>", { "src": "/content/images/redNew_delete.png", "ID": options.data.ID, "TemplateID": options.data.TemplateID, "UserID": options.data.AssignedTo, "style": "overflow: auto;cursor: pointer;padding-left:15px;", "class": "imgDelete", "title": "Delete" }))
                                .appendTo(container);
                        }
                    }
                }

            ],
            showBorders: true,
            showRowLines: true,
            onContentReady: function (e) {
                var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                if (dataGrid.getDataSource() !== null)
                    globaldata = dataGrid.getDataSource()._items;
                //e.component.columnOption("command:edit", "visibleIndex", -1);

            },
            toolbar: function (e) {
                e.toolbarOptions.visible = false;
            },
            onEditorPreparing: function (e) {
                if (e.parentType === 'dataRow' && e.dataField === 'TypeName') {

                    e.editorElement.dxSelectBox({

                        dataSource: GroupsData,
                        valueExpr: "Id",
                        displayExpr: "Name",
                        value: e.row.data.Type,
                        searchEnabled: true,
                        onValueChanged: function (ea) {
                            $.each(ea.component._dataSource._items, function (i, v) {
                                var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                                if (v.Id === ea.value) {
                                    e.setValue(v.Name);

                                    //don't need to change Assignee when role is change, user is playing selected role in current project.
                                    //dataGrid.getDataSource()._items[e.row.rowIndex].AssignedTo = "";
                                    //dataGrid.getDataSource()._items[e.row.rowIndex].AssignedToName = "";
                                    dataGrid.getDataSource()._items[e.row.rowIndex].Type = v.Id;
                                    dataGrid.getDataSource()._items[e.row.rowIndex].TypeName = v.Name;

                                }
                            });
                        }
                    });
                    e.cancel = true;

                }
                if (e.parentType == "dataRow" && e.dataField == "AssignedToName") {

                }

            },
            onRowValidating: function (e) {
                if (typeof e.newData.AllocationEndDate !== "undefined") {

                    if (new Date(e.newData.AllocationEndDate) < new Date(e.key.AllocationStartDate)) {
                        //e.isValid = false;
                        //e.errorText = "StartDate should be less then EndDate";
                        e.key.AllocationStartDate = e.newData.AllocationEndDate;
                    }
                }
                if (typeof e.newData.AllocationStartDate !== "undefined") {

                    if (new Date(e.key.AllocationEndDate) < new Date(e.newData.AllocationStartDate)) {
                        //e.isValid = false;
                        //e.errorText = "StartDate should be less then EndDate";
                        e.key.AllocationEndDate = e.newData.AllocationStartDate;
                    }
                }
                if (typeof e.newData.PctAllocation !== "undefined") {
                    if (parseInt(e.newData.PctAllocation) <= 0) {
                        e.isValid = false;
                        e.errorText = "Cannot make Allocation with 0 Percentage.";
                    }
                    if (!e.newData.PctAllocation) {
                        e.isValid = false;
                        e.errorText = "Cannot make Allocation with 0 Percentage.";
                    }
                    if (parseInt(e.newData.PctAllocation) > 100) {
                        e.isValid = false;
                        e.errorText = "Cannot make Allocation with Greater than 100 Percentage.";
                    }
                }

                $.cookie("dataChanged", 1, { path: "/" });
                $.cookie("projTeamAllocSaved", 0, { path: "/" });
            }
        });

    });


    function bindDatapopup(popupFilters) {
        var titleView = null;
        if (typeof (popupFilters.allocationStartDate) == "object") {
            if (popupFilters.allocationStartDate != null)
                popupFilters.allocationStartDate = (popupFilters.allocationStartDate).toISOString();
        }

        if (typeof (popupFilters.allocationEndDate) == "object") {
            if (popupFilters.allocationEndDate != null)
                popupFilters.allocationEndDate = (popupFilters.allocationEndDate).toISOString();
        }

        if ($("#tileViewContainer").length > 0) {

            var titleViewObj = $('#tileViewContainer').dxTileView('instance');
            if (titleViewObj) {
                titleViewObj.option("dataSource", "/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters));
                titleViewObj._refresh();
            }
        }
        else {

            titleView = $("<div id='tileViewContainer' style='clear:both;padding-bottom:100px;' />").dxTileView({
                height: window.innerHeight - 120,
                width: window.innerWidth - 150,
                showScrollbar: true,
                baseItemHeight: 65,
                baseItemWidth: 150,
                itemMargin: 15,
                direction: "vertical",
                elementAttr: { "class": "tileViewContainer" },
                noDataText: "No resource available",
                dataSource: "/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters),
                itemTemplate: function (itemData, itemIndex, itemElement) {
                    itemData.PctAllocation = Math.round(itemData.PctAllocation);
                    var html = new Array();
                    var str = itemData.AssignedTo + "','" + itemData.AssignedToName;
                    var strwithspace = str.replace(/\s/g, '&nbsp;'); //str.replace(" ", "&nbsp;")
                    html.push("<div class='timesheet'><img src='/content/images/Pipeline-By-Estimator.png' height='20px' title='Allocation Timeline' onclick=openResourceTimeSheet('" + strwithspace + "'); />");
                    html.push("</div>");
                    html.push("<div class='UserDetails'>");
                    html.push("<div id='" + itemData.AssignedTo + "'>");
                    //   html.push("<div id='" + itemData.AssignedTo + "'>");
                    html.push("<div class='AssignedToName'>");
                    html.push(itemData.AssignedToName);
                    html.push("</div>");

                    if (itemData.PctAllocation >= 100 && popupFilters.isAllocationView) {
                        html.push("<div>");
                        html.push("(" + itemData.PctAllocation + "%)");
                        html.push("</div>");
                    }
                    else if (itemData.PctAllocation > 0 && !popupFilters.isAllocationView) {
                        html.push("<div style='padding-bottom:3px;'>");
                        html.push("(" + (100 - Number(itemData.PctAllocation)) + "%)");
                        //html.push("(" + (Number(itemData.PctAllocation)) + "%)");
                        html.push("</div>");
                    }
                    if (popupFilters.isAllocationView && itemData.PctAllocation < 100) {
                        html.push("<div style='padding-bottom:3px;'>");
                        html.push("(" + (Number(itemData.PctAllocation)) + "%)");
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
                    itemElement.attr("title", itemData.JobTitle);
                    itemElement.append(html.join(""));

                    $("#loadpanel").dxLoadPanel({
                        message: "Loading...",
                        visible: false
                    });
                },
                onItemClick: function (e) {
                    if (showTimeSheet == true) {
                        showTimeSheet = false;
                        return;
                    }

                    var data = e.itemData;
                    var element = _.findWhere(globaldata, { ID: parseInt(popupFilters.ID) });
                    element.AssignedTo = data.AssignedTo;
                    element.AssignedToName = data.AssignedToName;
                    element.AllocationEndDate = popupFilters.allocationEndDate;
                    element.AllocationStartDate = popupFilters.allocationStartDate;
                    element.PctAllocation = popupFilters.pctAllocation;

                    AllocateResource();
                },
                onInitialized: function (e) {
                    $("#loadpanel").dxLoadPanel({
                        message: "Loading...",
                        visible: true
                    });
                },
                noDataText: function (e) {
                    $('.dx-empty-message').html('No resource available');
                    $("#loadpanel").dxLoadPanel({
                        message: "Loading...",
                        visible: false
                    });
                }
            });
        }

        return titleView;
    };

    function AllocateResource() {
        var grid = $('#gridTemplateContainer').dxDataGrid('instance');
        grid.option('dataSource', globaldata);
        globaldata = [];
        grid._refresh();

        $('#popupContainer').dxPopup('instance').hide();
    }

    $(document).on("click", "img.assigneeToImg", function (e) {
         
        var groupid = $(this).attr("id");
        var dataid = e.target.id;
        var data = _.find(globaldata, function (s) { return s.ID.toString() === dataid; });
        popupFilters.projectID = projectID;
        popupFilters.resourceAvailability = 2;
        popupFilters.complexity = false;
        popupFilters.projectVolume = false;
        popupFilters.projectCount = false;
        popupFilters.RequestTypes = false;
        popupFilters.Customer = false;
        popupFilters.Sector = false;
        popupFilters.groupID = (data != null && data != undefined) ? data.Type : $(this).attr("group");
        popupFilters.allocationStartDate = new Date($(this).attr("startDate"));
        popupFilters.allocationEndDate = new Date($(this).attr("endDate"));
        popupFilters.pctAllocation = data.PctAllocation;
        popupFilters.isAllocationView = false;
        if (projectID.startsWith("OPM")) {
            popupFilters.ModuleIncludes = false;
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
        if (data)
            popupTitle = "Available " + data.TypeName + "s";

        $("#popupContainer").dxPopup({
            title: popupTitle,
            width: "98%",
            height: "100%",
            visible: true,
            scrolling: true,
            dragEnabled: true,
            resizeEnabled: true,
            position: { my: "left top", at: "left top", of: window },
            contentTemplate: function (contentElement) {

                contentElement.append(

                    $("<div class='px-3 shadow-effect mb-2' />").append(
                        $("<div class='row fleft pt-4 mt-1' />").dxRadioGroup({
                            dataSource: radioGroupItems,
                            displayExpr: "text",
                            value: _.findWhere(radioGroupItems, { value: popupFilters.resourceAvailability }),
                            layout: "horizontal",
                            onValueChanged: function (e) {

                                popupFilters.resourceAvailability = e.value.value;
                                bindDatapopup(popupFilters);
                            }
                        }),

                        $("<div id='divworkExbar' class='fleft pb-3 workExbar' />").append(
                            $("<div style='margin-top: 22px; margin-left: 15px;'>").dxButton({
                                icon: "/content/images/work-experience.png",
                                hint: 'Show Experience',
                                //type: "success",                                
                                onClick: function (e) {

                                    $("#divworkExbar .dx-button-mode-contained").toggleClass("state-active");
                                    if (popupFilters.projectVolume) {
                                        popupFilters.projectVolume = false;
                                        popupFilters.projectCount = false;
                                        bindDatapopup(popupFilters);
                                        e.component.option("hint", "Show Experience");
                                    } else {
                                        popupFilters.projectVolume = true;
                                        popupFilters.projectCount = true;
                                        bindDatapopup(popupFilters);
                                        e.component.option("hint", "Hide Experience");
                                    }
                                }
                            })
                        ),

                        $("<div class='row bs clearfix' />").append(

                            $("<div class='col-md-2 col-sm-4 col-xs-12  pb-3'>").append(
                                $("<label class='lblStartDate' style='width:100%;'>Start Date</label>"),
                                $("<div id='dtStartDate' class='chkFilterCheck' style='padding-left:5px;width:100%;' />").dxDateBox({
                                    type: "date",
                                    value: popupFilters.allocationStartDate,
                                    displayFormat: 'MMM d, yyyy',
                                    onValueChanged: function (data) {

                                        popupFilters.allocationStartDate = data.value;
                                        var endDateObj = $("#dtEndDate").dxDateBox("instance");
                                        if (typeof endDateObj != "undefined") {
                                            var enddate = endDateObj.option('value');
                                            if (new Date(enddate) < new Date(data.value)) {
                                                popupFilters.allocationEndDate = popupFilters.allocationStartDate;
                                                endDateObj.option('value', popupFilters.allocationStartDate);
                                            }
                                        }
                                        bindDatapopup(popupFilters);
                                    }

                                })

                            ),

                            $("<div class='col-md-2 col-sm-4 col-xs-12 pb-3 noPadd1'>").append(
                                $("<label class='lblStartDate' style='width:100%;'>End Date</label>"),
                                $("<div id='dtEndDate' class='chkFilterCheck' style='width:100%;' />").dxDateBox({
                                    type: "date",
                                    value: popupFilters.allocationEndDate,
                                    displayFormat: 'MMM d, yyyy',
                                    onValueChanged: function (data) {

                                        popupFilters.allocationEndDate = data.value;
                                        var startDateObj = $("#dtStartDate").dxDateBox("instance");
                                        if (typeof startDateObj != "undefined") {
                                            var startdate = startDateObj.option('value');
                                            if (new Date(startdate) > new Date(data.value)) {
                                                popupFilters.allocationStartDate = popupFilters.allocationEndDate;
                                                startDateObj.option('value', popupFilters.allocationEndDate);
                                            }
                                        }
                                        bindDatapopup(popupFilters);
                                    }
                                })
                            ),

                            $("<div class='col-cus1 col-sm-4 col-xs-12 pb-3'>").append(
                                $("<label class='lblStartDate' style='width:100%;'>% Allocation</label>"),
                                $("<div class='chkFilterCheck' style='width:100%;' />").dxTextBox({
                                    value: popupFilters.pctAllocation,
                                    onValueChanged: function (data) {
                                        popupFilters.pctAllocation = data.value;
                                    }
                                })

                            ),
                            $("<div style='margin-top: 22px;'>").dxButton({
                                icon: "/content/images/changeViewGreen.png",
                                hint: 'change view',
                                //type: "success",                                
                                onClick: function (e) {
                                    var popup = $('#popupContainer').dxPopup('instance');
                                    if (popupFilters.isAllocationView == true) {
                                        popupFilters.isAllocationView = false;
                                        var popupTitle = "Available Resource";
                                        if (data)
                                            popupTitle = "Available " + data.TypeName + "s";
                                        popup.option('title', popupTitle);
                                        e.component.option('icon', "/content/images/changeViewGreen.png");
                                    }
                                    else {
                                        popupFilters.isAllocationView = true;

                                        var popupTitle = "Allocated Resource";
                                        if (data)
                                            popupTitle = "Allocated " + data.TypeName + "s";
                                        popup.option('title', popupTitle)
                                        e.component.option('icon', "/content/images/changeView.png");
                                    }


                                    bindDatapopup(popupFilters);
                                }
                            }),

                            $("<div class='col-cus1 col-sm-4 col-xs-12 pb-3 pt-cus1'>").append(
                                $(`<img <%=HidePrecon%> src='/Content/images/PreconCal2.png' style='cursor: pointer;width:20px;' class='imgPreconDate' title='Set Precon Date'>`),
                                $(`<img <%=HideConst%> src='/Content/images/ConstDate.png' style='cursor: pointer;width:20px;' class='imgConstDate ml-4' title='Set Construction Date'>`)
                            )
                        ),
                    ),


                    $("<div class='shadow-effect' style='padding-bottom: 10px;' />").append(

                        /* $("<h5>Filter by</h5>").append(),*/
                        $("<div id='filterChecks' class='clearfix pb-3' />").append(
                            $("<div id='chkComplexity' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                text: "Complexity " + "<%=ProjectComplexity%>",
                                visible: ('<%=ModuleName%>' == "OPM" || '<%=ModuleName%>' == "CPR" || '<%=ModuleName%>' == "CNS") == true ? true : false,
                                value: popupFilters.complexity,
                                onValueChanged: function (e) {

                                    popupFilters.complexity = e.value;
                                    bindDatapopup(popupFilters);
                                },
                            }),

                            $("<div id='chkRequestType' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                /*text: "Project Type",*/
                                text: "<%=RequestType%>",
                                hint: 'Project Type',
                                visible: ('<%=RequestType%>' != '' && '<%=RequestType%>' != null) == true ? true : false,
                                onValueChanged: function (e) {
                                    popupFilters.RequestTypes = e.value;
                                    bindDatapopup(popupFilters);
                                }
                            }),

                            $("<div id='chkCustomer' title='Customer' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                text: '<%=Customer%>',
                                visible: ('<%=Customer%>' != '' && '<%=Customer%>' != null) == true ? true : false,
                                value: popupFilters.Customer,
                                hint: 'Customer',
                                onValueChanged: function (e) {
                                    popupFilters.CompanyLookup = '<%=CompanyLookup%>';
                                    popupFilters.Customer = e.value;
                                    bindDatapopup(popupFilters);
                                },
                            }),

                            $("<div id='chkSector' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                text: '<%=Sector%>',
                                visible: ('<%=Sector%>' != '' && '<%=Sector%>' != null) == true ? true : false,
                                hint: 'Sector',
                                value: popupFilters.Sector,
                                onValueChanged: function (e) {
                                    popupFilters.SectorName = '<%=Sector%>';
                                    popupFilters.Sector = e.value;
                                    bindDatapopup(popupFilters);
                                },
                            }),
                        ),

                        $("<div id='dropdownFilters' class='row' />").append(

                            $("<div class='filterctrl-jobDepartment' />").dxSelectBox({
                                placeholder: "Limit By Department",
                                valueExpr: "ID",
                                displayExpr: "Title",
                                dataSource: "/api/rmmapi/GetDepartments?GroupID=" + popupFilters.groupID,
                                onSelectionChanged: function (selectedItems) {
                                    var items = selectedItems.selectedItem.ID;

                                    popupFilters.departments = items;
                                    $.get("/api/rmmapi/GetGroupTitles?GroupID=" + popupFilters.groupID + "&DepartmentID=" + items, function (data, status) {
                                        JobTitleData = data;
                                        var tagBox = $("#tagboxTitles").dxTagBox("instance");
                                        tagBox.option("dataSource", JobTitleData);
                                        tagBox.reset();
                                    });

                                    bindDatapopup(popupFilters);
                                },
                                onContentReady: function (e) {

                                }
                            }),

                            $("<div id='tagboxTitles'  class='filterctrl-jobtitle' />").dxTagBox({
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

                            $("<div class='filterctrl-userpicker' />").dxSelectBox({
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
                                        //var checkcomplexity = $("#chkComplexity").dxCheckBox("instance");
                                        //checkcomplexity.option("value", true);
                                        bindDatapopup(popupFilters);
                                    } else {
                                        var items = selectedItems.selectedItem.Id;
                                        popupFilters.SelectedUserID = items;
                                        popupFilters.projectID = projectID;
                                        popupFilters.complexity = false;
                                        bindDatapopup(popupFilters);
                                    }
                                }
                            }),
                        )
                    ),

                    bindDatapopup(popupFilters)
                )

            },
            itemClick: function (e) {
            }
        });

        var popupInstance = $('#popupContainer').dxPopup('instance');
        popupInstance.option('title', popupTitle);
    })

    $(document).on("click", "img.imgDelete", function (e) {

        var userID = $(this).attr("UserID");
        var dataid = e.target.id;
        var result = DevExpress.ui.dialog.confirm('Are you sure you want to delete selected allocation?', 'Confirm');
        var popup = $(document.getElementsByClassName('dx-popup-inherit-height')[0]);
        popup.addClass("PopupCustomPosition");
        result.done(function (confirmation) {
            if (confirmation) {
                globaldata = _.reject(globaldata, function (globaldata) { return globaldata.ID == dataid; });
                var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                dataGrid.option("dataSource", globaldata);

                $.post("/api/rmmapi/DeleteCRMAllocation", { ID: dataid, TicketID: projectID, UserID: userID }).then(function (response) {
                    //window.parent.CloseWindowCallback(1, document.location.href);
                }, function (error) { });
            }
        });
    });

    $(document).on("click", "img.imgPreconDate", function (e) {
        //var index = $(this).attr("Index");
        var result = DevExpress.ui.dialog.confirm('Are you sure you want to switch to Precon Dates?', 'Confirm');
        var popup = $(document.getElementsByClassName('dx-popup-inherit-height')[0]);
        popup.addClass("PopupCustomPosition");
        result.done(function (confirmation) {
            if (confirmation) {
                var startDatebox = $("#dtStartDate").dxDateBox("instance");
                var endDatebox = $("#dtEndDate").dxDateBox("instance");

                var preconStartdate = '<%= PreConStartDate%>';
                var preconEnddate = '<%= PreConEndDate%>';
                startDatebox.option("value", preconStartdate);
                endDatebox.option("value", preconEnddate);

            }
        });
    });

    $(document).on("click", "img.imgConstDate", function (e) {
        //var index = $(this).attr("Index");
        var result = DevExpress.ui.dialog.confirm('Are you sure you want to switch to Construction Dates?', 'Confirm');
        var popup = $(document.getElementsByClassName('dx-popup-inherit-height')[0]);
        popup.addClass("PopupCustomPosition");
        result.done(function (confirmation) {
            if (confirmation) {
                var startDatebox = $("#dtStartDate").dxDateBox("instance");
                var endDatebox = $("#dtEndDate").dxDateBox("instance");

                var constStartdate = '<%= StartDate%>';
                var constEnddate = '<%= EndDate%>';
                startDatebox.option("value", constStartdate);
                endDatebox.option("value", constEnddate);

            }
        });
    });

    $.get("/api/rmmapi/GetGroupsOrResource", function (data, status) {
        GroupsData = data;
    });

    $.get("/api/rmmapi/GetUserProfilesData", function (data, status) {
        UsersData = data;
    });

    $.get("/api/rmmapi/GetGroupTitles?GroupID=" + popupFilters.groupID + "&DepartmentID=0", function (data, status) {
        JobTitleData = data;
    });

    function openResourceTimeSheet(assignedTo, assignedToName) {
        showTimeSheet = true;
        //param isRedirectFromCardView is used to hide card view and show allocation grid
        var url = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGrid&ViewName=FindAvailability&isRedirectFromCardView=true&selecteddepartment=-1&SelectedResource=" + assignedTo;
        window.parent.UgitOpenPopupDialog(url, "", "Timeline for User : " + assignedToName, "95", "95", false, "");
    }

    var expandAdaptiveDetailRow = function (key, dataGridInstance) {
        if (!dataGridInstance.isAdaptiveDetailRowExpanded(key)) {
            dataGridInstance.expandAdaptiveDetailRow(key);
        }
    }


    const LoadGanttChart = function (ganttSource) {

        var maxDaterange = new Date(Math.max.apply(null, ganttSource.map(function (e) {
            return new Date(e.AllocationEndDate);

        })));

        var minDaterange = new Date(Math.min.apply(null, ganttSource.map(function (e) {
            return new Date(e.AllocationStartDate);

        })));

        var gantt = $("#divPopupGantt").dxGantt({
            tasks: {
                dataSource: ganttSource,
                endExpr: "AllocationEndDate",
                keyExpr: "ID",
                //progressExpr: "PctAllocation",
                startExpr: "AllocationStartDate",
                titleExpr: "AssignedToName"
            },
            resources: {
                dataSource: ganttSource,
                //keyExpr: "ID",
                //textExpr: "TypeName"
            },
            resourceAssignments: {
                dataSource: ganttSource,
                keyExpr: "ID",
                textExpr: "TypeName"
            },
            showResources: true,
            editing: {
                enabled: false,
            },
            validation: {
                autoUpdateParentTasks: false,
            },
            columns: [{
                dataField: 'AssignedToName',
                caption: 'Name',
                width: 200,
            }, {
                dataField: 'AllocationStartDate',
                caption: 'Start Date',
                format: 'MMM d, yyyy',
                dataType: "date"
            }, {
                dataField: 'AllocationEndDate',
                caption: 'End Date',
                format: 'MMM d, yyyy',
                dataType: "date"
            }, {
                dataField: 'PctAllocation',
                caption: '%',
                width: 60,
                alignment: 'center',
            }],
            scaleType: 'months',
            taskListWidth: 500,
            taskContentTemplate: getTaskContentTemplate,
            startDateRange: new Date(minDaterange.getFullYear(), minDaterange.getMonth() - 1, 01),
            endDateRange: new Date(maxDaterange.getFullYear(), 11, 31)
        });
    };

    function getTaskContentTemplate(item) {
        //$('.dx-data-row').Class('devExtDataGrid-DataRow');
        const resource = item.taskResources[0];
        const img = "/Content/Images/girlprofilepic.jpg";
        const color = 0;
        const taskWidth = `${item.taskSize.width}px;`;
        const $customContainer = $(document.createElement('div'))
            .addClass('custom-task')
            .attr('style', `width:${taskWidth}`)
            .addClass(`custom-task-color-${color}`);

        if (JSON.parse("<%=EnableGanttProfilePic%>".toLowerCase())) {
            //const $imgWrapper = $(document.createElement('div'))
            //    .addClass('custom-task-img-wrapper')
            //    .appendTo($customContainer);
            //$(document.createElement('img'))
            //    .addClass('custom-task-img')
            //    .attr({
            //        src: resource ? img : '/Content/Images/girlprofilepic.jpg',
            //        alt: 'imageAlt',
            //    })
            //    .appendTo($imgWrapper);
        }

        const $wrapper = $(document.createElement('div'))
            .addClass('custom-task-wrapper')
            .appendTo($customContainer);

        $(document.createElement('div'))
            .addClass('custom-task-title')
            .text(item.taskData.title)
            .appendTo($wrapper);
        $(document.createElement('div'))
            .addClass('custom-task-row')
            .text(`${item.taskData.AssignedToName} (${item.taskData.PctAllocation}%)`)
            .appendTo($wrapper);

        //$(document.createElement('div'))
        //    .addClass('custom-task-progress')
        //    .attr('style', `width:${parseFloat(item.taskData.PctAllocation)}%;`)
        //    .appendTo($customContainer);

        return $customContainer;
    }


    var changedDates = {};
    const popupContentTemplate = function () {
        const timeFrameTypes = ["Days", "Weeks", "Months"];


        return $('<div>').append(
            $(`<p><span>Adjust all allocations by specifying the change in days, weeks or months.  For moving the schedule backward use the – sign.  Only dates that are current or in the future will be changed.  Dates in the past cannot be changed.</span></p>`),

            $(`<div id='divInputContainer' class='d-flex justify-content-between align-items-center flex-wrap paddingBottom' />`).append(
                $(`<div id='btnSpin' class='divMarginTop' />`).dxNumberBox({
                    value: 0,
                    //min: 0,
                    //max: 365,
                    showSpinButtons: true,
                    showClearButton: true,
                    stylingMode: 'outlined',
                    placeholder: 'select days, months or years',
                    label: 'Input',
                    width: 140,
                    value:0,
                }),

                $(`<div id='divTimeframe' class='divMarginTop' />`).dxSelectBox({
                    items: timeFrameTypes,
                    placeholder: 'Choose Timeframe',
                    showClearButton: true,
                    stylingMode: 'outlined',
                    label: 'Time',
                    width: 140,
                    value:'Days',
                }),

                //$(`<div id='divChoice' class='divMarginTop' />`).dxSelectBox({
                //    items: ["+ Plus", "- Minus"],
                //    placeholder: '',
                //    showClearButton: true,
                //    stylingMode: 'outlined',
                //    label: '+ or -',
                //    width: 140,
                //    value: '+ Plus',
                //}),
            ),

            $(`<div class='continueClass' />`).dxButton({
                text: 'Continue',
                icon: 'chevrondoubleright',
                onClick: function (e) {
                    
                    var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                    var timeframectrl = $("#divTimeframe").dxSelectBox("instance");
                    var spinctrl = $("#btnSpin").dxNumberBox("instance");
                    //var radioChoice = $("#divChoice").dxSelectBox("instance");
                    var inputValue = parseInt(spinctrl.option("value"));
                    var timeframeValue = timeframectrl.option("value");
                    var rows = dataGrid.getVisibleRows();
                    //var radioChoiceValue = radioChoice.option("value");

                    rows.forEach(function (item, index) {
                        var oldenddate = new Date(dataGrid.cellValue(index, "AllocationEndDate"));
                        if (oldenddate >= new Date()) {
                            dataGrid.cellValue(index, "AllocationEndDate", GetNewDate(oldenddate, timeframeValue, inputValue));
                        }

                        var oldstartdate = new Date(dataGrid.cellValue(index, "AllocationStartDate"));
                        if (oldstartdate >= new Date()) {
                            dataGrid.cellValue(index, "AllocationStartDate", GetNewDate(oldstartdate, timeframeValue, inputValue));
                        }
                    });

                    //update project dates
                    var currentDate = (new Date()).setHours(0, 0, 0, 0);
                    if (new Date('<%=PreConStartDate%>') >= currentDate) {
                        changedDates['PreConStart'] = GetNewDate(new Date('<%=PreConStartDate%>'), timeframeValue, inputValue);
                    }
                    if (new Date('<%=PreConEndDate%>') >= currentDate) {
                        changedDates['PreConEnd'] = GetNewDate(new Date('<%=PreConEndDate%>'), timeframeValue, inputValue);
                    }
                    if (new Date('<%=StartDateString%>') >= currentDate) {
                        changedDates['ConstStart'] = GetNewDate(new Date('<%=StartDateString%>'), timeframeValue, inputValue);
                    }
                    if (new Date('<%=EndDateString%>') >= currentDate) {
                        changedDates['ConstEnd'] = GetNewDate(new Date('<%=EndDateString%>'), timeframeValue, inputValue);
                    }

                    var popup = $("#UpdateDatesPopup").dxPopup("instance");
                    popup.hide();
                }
            }),

            $(`<div style='margin:15px;float:right;' />`).dxButton({
                text: 'Cancel',
                icon:'close',
                onClick: function (e) {
                    var popup = $("#UpdateDatesPopup").dxPopup("instance");
                    popup.hide();
                }
            }),

            

            //$(`<div id='divDetails' />`).append(`<b>${changedDates.forEach(function (item, index) { return item; })}</b>`),
        );
    };


    Date.prototype.addDays = function (days) {
        var date = new Date(this.valueOf());
        date.setDate(date.getDate() + days);
        return date;
    }

    function GetNewDate(oldenddate, timeframeValue, inputValue) {
        if (timeframeValue == "Days") {
            return oldenddate.addDays(inputValue);
        } else if (timeframeValue == "Weeks") {
            return oldenddate.addDays(inputValue * 7);
        } else if (timeframeValue == "Months") {
            return new Date(oldenddate.setMonth(oldenddate.getMonth() + inputValue));
        }
    }
</script>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    fieldset {
        border: 1px solid black;
        margin: 0 2px;
        padding: 0.35em 0.625em 0.75em;
    }

    legend {
        display: unset;
        width: unset;
        padding: unset;
        margin-bottom: unset;
        font-size: unset;
        line-height: unset;
        color: #000;
        border: unset;
        border-bottom: unset;
    }
</style>
<asp:Panel ID="pnlContainer" runat="server" ScrollBars="Auto">
    <div id="content">
        <div id="gridview">
            <div class="row">
                <div id="toast"></div>
                <div id="toastOverlap"></div>
                <div id="toastBlankAllocation"></div>
            </div>
            <div class="row">
                <div>
                    <div id="gridTemplateContainer" class="grid-template-container" style="width: 100%; float: left;">
                    </div>
                        <div class="marcal">       
                            <div id="btnAddNew" class="btnAddNew" style="display:inline-block;"></div>
                            <div id="btnAutofillAllocations" class="btnAddNew" style="display:inline-block;"></div>
                            <div id="btnProceed" class="btnAddNew" style="display:inline-block;">
                            </div>
                                        
                        </div>
                </div>
            </div>

        </div>
    </div>
</asp:Panel>
<div id="loadpanel">
</div>
<dx:ASPxPopupControl ID="pcSaveAsTemplate" runat="server" Modal="True"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcSaveAsTemplate" AllowDragging="true"
    HeaderText="Save Allocation as Template" CloseOnEscape="true" CloseAction="CloseButton" PopupAnimationType="None" EnableViewState="False" CssClass="unsaved_popUp">
    <SettingsAdaptivity Mode="Always" VerticalAlign="WindowCenter" MaxWidth="450px" />
    <ContentCollection>
        <dx:PopupControlContentControl ID="pcccRequestTypeChange" runat="server">
            <ugit:SaveAllocationAsTemplate runat="server" ID="ctrSaveAllocationAsTemplate" />
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ContentStyle>
        <Paddings PaddingBottom="5px" />
    </ContentStyle>
</dx:ASPxPopupControl>

<div id="popupContainer">
</div>
<div id="filterChecks" class="clearfix pb-3">
    <div id="chkCapacity" class='pl-3' style='float: left; display: none;'></div>
    <div id='chkComplexity' class='pl-3' style='float: left;'></div>
    <div id="chkRequestType" class='pl-3' style='float: left;'></div>
    <div id='chkCustomer' title='Customer' class='pl-3' style='float: left;'></div>
    <div id='chkSector' class='pl-3' style='float: left;'></div>
</div>
<div id="dropdownFilters" class="row" style='clear: both; margin-bottom: 15px;'>
    <div class='filterctrl-jobDepartment'></div>
    <div id='tagboxTitles' class='filterctrl-jobtitle'></div>
    <div class='filterctrl-userpicker'></div>
</div>

<div id="UpdateDatesPopup">
</div>