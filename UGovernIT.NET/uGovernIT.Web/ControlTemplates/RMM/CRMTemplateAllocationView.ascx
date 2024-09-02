<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CRMTemplateAllocationView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMM.CRMTemplateAllocationView" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    

    a:hover {
            text-decoration: underline;
        }
    .aAddItem_Top {
        padding-left: 10px;
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

   
         
    .btnSaveAsTemplate .dx-item.dx-buttongroup-item {
        color: #333 !important;
    }

    .btnShowGanttView .dx-button-content {
        padding: 0;
    }

    .filterlb-jobtitle {
        float: left;
        padding-left: 15px;
        float: left;
        padding-top: 7px;
        margin-top: 5px;
        width: 80px;
    }

    .filterctrl-jobtitle {
        width: 25%;
        display: inline-block;
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

    .filterctrl-jobDepartment, .filterctrl-userpicker {
        width: 25%;
        display: inline-block;
        margin-top: 6px;
    }

    .filterctrl-experiencedTag {
        /*clear: both;
        float: left;*/
        width: 25.3%;
        display: inline-block;
        margin-top: -6px;
        margin-left: 12px;
    }

    .filterctrl-addexperiencedTag {
        /*clear: both;
        float: left;*/
        width: 25.3%;
        display: inline-block;
        margin-top: -6px;
        margin-left: 12px;
        border-style: none !important;
    }

    .filterctrl-userpicker .dx-dropdowneditor-icon::before {
        content: "\f027";
    }

    .cls .dx-datagrid-revert-tooltip {
        display: none;
    }

    .display-flex {
        display: flex;
        padding-right: 18px;
    }
    .d-flex-modified {
        display: flex;
        justify-content: space-between;
        align-items: center;
    }

    .imgDelete {
    }

    .PopupCustomPosition {
        transform: translate(100%, 100px) scale(1) !important;
        margin-left: 5% !important;
        margin-top: 0% !important;
    }

    @media (min-width: 992px) {

        .noPadd1 {
            padding-left: 0;
            padding-right: 0;
        }
    }

    .custom-task-color-0 {
        background-color: #4A6EE2;
    }

    .custom-task-color-1 {
        background-color: #8a8a8a;
    }

    .custom-task-color-2 {
        background-color: #97DA97;
    }

    .custom-task-color-3 {
        background-color: #9DC186;
    }

    .custom-task-color-4 {
        background-color: #6BA538;
    }

    .custom-task-color-5 {
        background-color: #F2BC57;
    }

    .custom-task-color-6 {
        background-color: #E62F2F;
    }

    .custom-task-color-Precon {
        background-color: #52BED9 !important;
    }

    .custom-task-color-Const {
        background-color: #005C9B !important;
    }

    .custom-task-color-Closeout {
        background-color: #351B82 !important;
    }

    .custom-task {
        max-height: 28px;
        height: 100%;
        display: block;
        overflow: hidden;
        border-radius: 10px;
    }

    .custom-task-wrapper {
        padding: 5px;
        color: #fff;
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

    .paddingBottom {
        padding-bottom: 30px;
    }

    .continueClass {
        margin: 11px;
        float: right;
        margin-right: 0px;
    }

    tr.dx-virtual-row {
        height: 0px !important;
        display: none !important;
    }

    .date-w {
        width: 140px;
    }

    .w-70p {
        width: 70px;
    }

    .w-100p {
        width: 100px;
    }

    .clsSmartFilter {
        float: left;
        padding-top: 2px;
        font-weight: 500;
        padding-left: 10px;
    }

    .dx-datagrid-content .dx-datagrid-table .dx-row .dx-editor-cell {
        padding: 7px;
    }

    .Precon_Btn {
        color: white;
        background-color: #52BED9 !important;
    }

    .Const_Btn {
        color: white;
        background-color: #005C9B !important;
    }

    .Closeout_Btn {
        color: white;
        background-color: #351B82 !important;
    }

    .precon_Btn_White_Box {
        border-radius: 8px;
        color: #52BED9;
        border: 2px solid #52BED9;
        width: 85px;
        font-weight: 600;
    }

    .const_Btn_White_Box {
        border-radius: 8px;
        color: #005C9B;
        border: 2px solid #005C9B;
        width: 85px;
        font-weight: 600;
    }

    .closeout_Btn_White_Box {
        border-radius: 8px;
        color: #351B82;
        border: 2px solid #351B82;
        width: 85px;
        font-weight: 600;
    }

    #compareResume .dx-icon, #openGanttView .dx-icon, #compareTags .dx-icon {
        width: 25px !important;
        height: 25px !important;
    }

    #compareResume .dx-button-mode-contained.dx-state-hover, #openGanttView .dx-button-mode-contained.dx-state-hover, #compareTags .dx-button-mode-contained.dx-state-hover {
        background-color: white !important;
    }

    .innerCheckbox {
        position: absolute;
        right: 0px;
        height: 100%;
        bottom: 0px;
        height: 14px;
        width: 14px;
        padding: 1em 1.2em !important;
        padding-bottom: 5em !important;
    }

    .btnAddNew .dx-icon {
        filter: brightness(10);
    }

    #btnAddNew .dx-icon {
        margin-right: 4px;
        transform: scale(1.4);
        filter: none;
    }

    .btnAddNew .dx-button-content {
        margin: 0px 5px;
    }

    #btnShowMultiAllocation .dx-icon {
        margin-right: 4px;
        transform: scale(1.4);
        filter: none;
    }

    .reschedule-subtitle {
        text-align: center;
        padding: 5px;
    }

    .reschedule-title {
        text-align: center;
        margin-bottom: 0px;
        margin-top: 5px;
    }

    .cancelClass .dx-icon-clear {
        float: right;
        margin-left: 10px !important;
        padding: 1px !important;
        color: red !important;
        margin-top: 2px;
        font-size: 22px !important;
    }

    .cancelClass .dx-button-text, .continueClass .dx-button-text {
        margin: 0px 11px;
        font-size: 18px;
        display: grid;
        align-items: center;
    }

    .cancelClass, .continueClass {
        margin: 6px;
        float: left;
        border-radius: 13px;
        align-items: baseline;
        display: flex;
        font-weight: 500;
    }

        .continueClass .dx-icon {
            width: 22px;
            height: 22px;
            float: right;
            margin-left: 10px !important;
            padding: 1px !important;
            margin-top: 2px;
        }

    .btnWrapper {
        display: flex;
        justify-content: flex-end;
    }

    .chkFilterCheck {
        margin-top: 3px;
        padding-top: 0px;
    }

    #divPopupGantt .precon-phase {
        border-left: 2px solid #289ab6;
        background-color: rgb(82 190 217 / 22%);
    }

    #divPopupGantt .const-phase {
        border-left: 2px solid #002f4f;
        background-color: rgb(0 92 155 / 31%);
    }

    #divPopupGantt .closeout-phase {
        border-left: 2px solid #1f104e;
        background-color: rgb(53 27 130 / 32%);
    }

    .lbl-checkbox {
        display: inline-block;
        padding: 5em;
        background: #eee;
    }

    .preconCellStyle {
        background-color: #52BED9;
        border: 5px solid #fff !important;
        font-weight: 500;
    }

    .constCellStyle {
        color: #fff;
        background-color: #005C9B;
        border: 5px solid #fff !important;
    }

    .closeoutCellStyle {
        color: #fff;
        background-color: #351B82;
        border: 5px solid #fff !important;
    }

    .noDateCellStyle {
        color: #000000;
        background-color: #D6DAD9;
        border: 5px solid #fff !important;
    }

    .v-align {
        vertical-align: middle !important;
    }

    #btnShowProfile img {
        /*[+][SANKET][12/10/2023][Commented and added height and width]*/
        /*filter:invert(1);*/
        height: 30px;
        width: 23px;
    }

    .d-flex-justify {
        display: flex;
        justify-content: space-evenly;
    }

    .dateText {
        font-size: 11px;
        text-align: center;
    }

    .redCellColorStyle a {
        color: red !important;
    }

    .schedule-label {
        font-size: 15px;
        font-weight: 500;
        width: 170px;
    }

    .preconborderbox {
        border-style: dashed;
        border-width: 1.5px;
        border-radius: 5px;
        border-color: #52BED9;
    }

    .constborderbox {
        border-style: dashed;
        border-width: 1.5px;
        border-radius: 5px;
        border-color: #005C9B;
    }

    .closeoutborderbox {
        border-style: dashed;
        border-width: 1.5px;
        border-radius: 5px;
        border-color: #351B82;
    }

   /* #gridTemplateContainer .dx-datebox-calendar .dx-texteditor-input, #compactGridTemplateContainer .dx-datebox-calendar .dx-texteditor-input {
        text-align: left !important;
    }*/

    #btnSaveAllocation.dx-button-mode-contained:not(.dx-state-hover):hover {
        opacity: 0.7;
        background-color: #4FA1D6 !important;
    }

    .dx-checkbox-text {
        margin-top: 2px;
    }

   
</style>

<script>
    var GroupsData = [];
    var globaldata = [];
    var compactTempData = [];
    var OverlappingAllocationInPhases = [];
    var PreconStartDate = null;
    var PreconEndDate = null;
    var ConstStartDate = null;
    var ConstEndDate = null;
    var CloseoutStartDate = null;
    var CloseoutEndDate = null;
    var projectID = null;
    var templateId = null;
    var moduleName = null;
    var templateName = null;
    var isGridInValidState = true;
    var ajaxHelperPage = "<%=ajaxHelperPage%>";
    var isReadOnly = "<%=Request["readonly"]%>" == "true" ? true : false;
    var requestFromRMM = "<%=Request["requestFromRMM"]%>" == "true" ? true : false;
    var ProjectTemplates = [];
    var strSelectedAllocationIDs = "";
    var selectionObject;
    $(document).on("click", "img.imgDeleteNew", function (e) {
        var refIds = $(this).attr("ID");
        let ids = refIds.split(";");
        globaldata = globaldata.filter(x => !ids.includes(String(x.ID)));
        let dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
        dataGrid.option("dataSource", globaldata);
        dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
        dataGrid.option("dataSource", CompactPhaseConstraints());
    });
    function GetTemplateDetails(templateID) {
        $.get("/api/rmmapi/GetTemplateDetails?id=" + templateID, function (data, status) {
            console.log(data);
            GroupsData = data.roles;
            globaldata = JSON.parse(data.templateData.Template);
            PreconStartDate = new Date(data.templateData.PreconStartDate);
            PreconEndDate = new Date(data.templateData.PreconEndDate);
            ConstStartDate = new Date(data.templateData.ConstStartDate);
            ConstEndDate = new Date(data.templateData.ConstEndDate);
            CloseoutStartDate = new Date(data.templateData.CloseOutStartDate);
            CloseoutEndDate = new Date(data.templateData.CloseOutEndDate);
            templateId = data.templateData.ID;
            projectID = data.templateData.TicketID;
            moduleName = data.templateData.ModuleName;
            templateName = data.templateData.Name;
            $("#pPrecon").text(PreconStartDate.format("MMM d, yyyy") + " To " + PreconEndDate.format("MMM d, yyyy"));
            $("#pConst").text(ConstStartDate.format("MMM d, yyyy") + " To " + ConstEndDate.format("MMM d, yyyy"));
            $("#pCloseout").text(CloseoutStartDate.format("MMM d, yyyy") + " To " + CloseoutEndDate.format("MMM d, yyyy"));
            globaldata.forEach(function (data, index) {
                data.TypeName = GetTypeName(data.Type);
                if (data.AssignedTo == '') {
                    let internaldata = globaldata.filter(x => x.Type == data.Type && x.AssignedTo != '')[0];
                    if (internaldata != null && internaldata.AssignedTo != '') {
                        data.AssignedTo = internaldata.AssignedTo;
                    }
                    else {
                        data.AssignedTo = "User" + index;
                    }
                }
                if (data.ID < 0) {
                    data.ID = -2000 + index;
                }
            });
            compactTempData = CompactPhaseConstraints();
            BindControls();
        });
    }
    function GetProjectTemplates() {
        $.get("/api/rmmapi/GetProjectTemplates", function (data, status) {
            ProjectTemplates = data;
        });
    }
    function GetTypeName(id) {
        return GroupsData.filter(x => x.Id == id).length > 0 ? GroupsData.filter(x => x.Id == id)[0].Name : "";
    }
    function BindControls() {
        $("#toast").dxToast({
            message: "Template Saved Successfully. ",
            type: "success",
            displayTime: 1000,
            position: "{ my: 'center', at: 'center', of: window }"
        });
        $("#compactGridTemplateContainer").dxDataGrid({
            //columnHidingEnabled: true,
            dataSource: compactTempData,
            ID: "grdCompactTemplate",
            editing: {
                mode: "cell",
                allowEditing: !isReadOnly,
                allowUpdating: !isReadOnly
            },
            sorting: {
                mode: "multiple" // or "multiple" | "none"
            },
            scrolling: {
                mode: 'Standard',
            },
            paging: { enabled: false },
            columns: [
                {
                    dataField: "TypeName",
                    dataType: "text",
                    caption: "Role",
                    sortIndex: "1",
                    sortOrder: "asc",
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
                    width: "15%",
                    alignment: 'center',
                    cssClass: "v-align",
                    validationRules: [{ type: "required", message: '', }],
                    format: 'MMM d, yyyy',
                    sortIndex: "2",
                    sortOrder: "asc",
                    allowEditing: false,
                    editorOptions: {
                        onFocusOut: function (e, options) {
                            var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                            dataGrid.saveEditData();
                        },
                        onClosed: function (e) {
                            var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                            dataGrid.saveEditData();
                        },
                    }
                },
                {
                    dataField: "AllocationEndDate",
                    caption: "End Date",
                    dataType: "date",
                    alignment: 'center',
                    cssClass: "v-align",
                    allowEditing: false,
                    width: "15%",
                    validationRules: [{ type: "required", message: '', }],
                    format: 'MMM d, yyyy',
                    editorOptions: {
                        onFocusOut: function (e) {
                            var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                            dataGrid.saveEditData();
                        },
                        onClosed: function (e) {
                            var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                            dataGrid.saveEditData();
                        },
                    }
                },
                {
                    dataField: "PctAllocation",
                    caption: "% Precon",
                    dataType: "text",
                    cssClass: "v-align",
                    width: "7%",
                    cellTemplate: function (container, options) {
                        if (options.data.preconRefIds != null) {
                            $("<div id='dataId'>")
                                .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.preconRefIds + "');>" + options.data.PctAllocation + "</a></span>")
                                .appendTo(container);
                        }
                        else {
                            $("<div id='dataId'>")
                                .append("<span style='float: left;overflow: auto;'>" + options.data.PctAllocation + "</span>")
                                .appendTo(container);
                        }
                    }
                },
                {
                    dataField: "PctAllocationConst",
                    caption: "% Const.",
                    dataType: "text",
                    cssClass: "v-align",
                    width: "7%",
                    cellTemplate: function (container, options) {
                        if (options.data.constRefIds != null) {
                            $("<div id='dataId'>")
                                .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.constRefIds + "');>" + options.data.PctAllocationConst + "</a></span>")
                                .appendTo(container);
                        }
                        else {
                            $("<div id='dataId'>")
                                .append("<span style='float: left;overflow: auto;'>" + options.data.PctAllocationConst + "</span>")
                                .appendTo(container);
                        }
                    }
                },
                {
                    dataField: "PctAllocationCloseOut",
                    caption: "% Closeout",
                    dataType: "text",
                    cssClass: "v-align",
                    width: "7%",
                    cellTemplate: function (container, options) {
                        if (options.data.closeOutRefIds != null) {
                            $("<div id='dataId'>")
                                .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.closeOutRefIds + "');>" + options.data.PctAllocationCloseOut + "</a></span>")
                                .appendTo(container);
                        }
                        else {
                            $("<div id='dataId'>")
                                .append("<span style='float: left;overflow: auto;'>" + options.data.PctAllocationCloseOut + "</span>")
                                .appendTo(container);
                        }
                    }
                },
                {
                    width: "8%",
                    fieldName: "SoftAllocation",
                    name: 'SoftAllocation',
                    caption: "",
                    dataType: 'text',
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        if (isAllAllocationsSame(options)) {
                            $("<div id='divSoftAllocation'>").append(
                                $("<div id='divSwitch' />").dxSwitch({
                                    switchedOffText: 'Hard',
                                    switchedOnText: 'Soft',
                                    width: 60,
                                    disabled: isReadOnly,
                                    value: options.data.SoftAllocation,
                                    onValueChanged(data) {

                                    },
                                })
                            ).appendTo(container);
                        } else {
                            if (isReadOnly) {
                                $("<div id='divSoftAllocation' style='padding-left:10px;padding-right:10px;margin-top:4px;' ><span>Mixed</span></div>").appendTo(container);
                            } else {
                                $("<div id='divSoftAllocation' style='padding-left:10px;padding-right:10px;margin-top:4px;' ><span style='cursor:pointer;' onclick=$('#btnDetailedSummary').click()>Mixed</span></div>").appendTo(container);
                            }//$("<div id='divSoftAllocation' style='padding-left:10px;padding-right:10px;' >").append(
                            //    $("<div id='divMixedSwitch' />").dxButton({
                            //        text: 'Mixed',
                            //        width: 60,
                            //        stylingMode: 'contained',
                            //        onClick: function (e) {
                            //            $("#btnDetailedSummary").click();
                            //        }
                            //    })
                            //).appendTo(container);
                        }
                    },

                },
                {
                    width: "8%",
                    fieldName: "NonChargeable",
                    caption: 'NCO',
                    dataType: 'text',
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        if (isAllNCOAllocationsSame(options)) {
                            $("<div id='divNonChargeable'>").append(
                                $("<div id='divSwitchNonChargeable' />").dxCheckBox({
                                    width: 30,
                                    disabled: isReadOnly,
                                    value: options.data.NonChargeable,
                                    onValueChanged(data) {

                                    },
                                })
                            ).appendTo(container);
                        } else {
                            if (isReadOnly) {
                                $("<div id='divSoftAllocation' style='padding-left:10px;padding-right:10px;margin-top:4px;' ><span>Mixed</span></div>").appendTo(container);
                            } else {
                                $("<div id='divSoftAllocation' style='padding-left:10px;padding-right:10px;margin-top:4px;' ><span style='cursor:pointer;' onclick=$('#btnDetailedSummary').click()>Mixed</span></div>").appendTo(container);
                            }//$("<div id='divNonChargeable' style='padding-left:10px;padding-right:10px;' >").append(
                            //    $("<div id='divMixedNCOSwitch' />").dxButton({
                            //        text: 'Mixed',
                            //        width: 60,
                            //        stylingMode: 'contained',
                            //        onClick: function (e) {
                            //            $("#btnDetailedSummary").click();
                            //        }
                            //    })
                            //).appendTo(container);
                        }
                    },
                },
                {
                    width: "5%",
                    visible: !isReadOnly,
                    cellTemplate: function (container, options) {
                        var preconStartdate = PreconStartDate;
                        var preconEnddate = PreconEndDate;
                        if (preconEnddate == 'Jan 1, 0001' || preconStartdate == 'Jan 1, 0001') {
                            $("<div id='rowDelete' style='text-align:center;'>")
                                .append($("<img>", {
                                    "src": "/content/images/deleteIcon-new.png", "ID": options.data.IdsForDelete,
                                    "style": "overflow: auto;cursor: pointer;", "class": "imgDeleteNew", "title": "Delete", "width": "23px"
                                }))
                                .appendTo(container);
                        } else {
                            $("<div id='rowDelete' style='text-align:center;'>")
                                .append($("<img>", {
                                    "src": "/content/images/deleteIcon-new.png", "ID": options.data.IdsForDelete,
                                    "style": "overflow: auto;cursor: pointer;", "class": "imgDeleteNew", "title": "Delete", "width": "23px"
                                }))
                                .appendTo(container);
                        }
                    }
                }
                

            ],
            showBorders: true,
            showRowLines: true,
            onRowUpdating: function (e) {
                SaveToGlobalData(e);
            },
            onCellClick: function (e) {
                let compactElement = _.findWhere(compactTempData, { ID: parseInt(e.data.ID) });
                let uniqueIds = [...new Set(compactElement.IdsForDelete.split(';'))];
                if (e.column.fieldName == "SoftAllocation") {
                    if (isAllAllocationsSame(e)) {
                        compactTempData.forEach(function (part, index, theArray) {
                            if (part.ID == e.data.ID) {
                                part.SoftAllocation = !part.SoftAllocation;
                            }
                        });
                        globaldata.forEach(function (part, index, theArray) {
                            if (uniqueIds.includes(String(part.ID))) {
                                part.SoftAllocation = !part.SoftAllocation;
                            }
                        });
                    }
                }
                if (e.column.fieldName == "NonChargeable") {
                    if (isAllNCOAllocationsSame(e)) {
                        compactTempData.forEach(function (part, index, theArray) {
                            if (part.ID == e.data.ID) {
                                part.NonChargeable = !part.NonChargeable;
                            }
                        });
                        globaldata.forEach(function (part, index, theArray) {
                            if (uniqueIds.includes(String(part.ID))) {
                                part.NonChargeable = !part.NonChargeable;
                            }
                        });
                    }
                }
            },
            onContentReady: function (e) {
                //var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                //if (dataGrid.getDataSource() !== null)
                //    globaldata = dataGrid.getDataSource()._items;

                //clickUpdateSize();
            },
            toolbar: function (e) {
                e.toolbarOptions.visible = false;
            },
            onEditorPreparing: function (e) {
                if (e.parentType === 'dataRow' && e.dataField === 'TypeName') {

                    e.editorElement.dxSelectBox({

                        dataSource: _.sortBy(GroupsData, 'Name'),
                        valueExpr: "Id",
                        displayExpr: "Name",
                        value: e.row.data.Type,
                        searchEnabled: true,
                        onValueChanged: function (ea) {
                            var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                            $.each(ea.component._dataSource._items, function (i, v) {
                                if (v.Id === ea.value) {
                                    e.setValue(v.Name);
                                    //don't need to change Assignee when role is change, user is playing selected role in current project.
                                    //dataGrid.getDataSource()._items[e.row.rowIndex].AssignedTo = "";
                                    //dataGrid.getDataSource()._items[e.row.rowIndex].AssignedToName = "";
                                    dataGrid.getDataSource()._items[e.row.rowIndex].Type = v.Id;
                                    dataGrid.getDataSource()._items[e.row.rowIndex].TypeName = v.Name;
                                    let allocData = globaldata.filter(x => x.AssignedTo == e.row.data.AssignedTo && x.Type == e.row.data.Type);
                                    $.each(allocData, function (key, value) {
                                        value.Type = v.Id;
                                        value.TypeName = v.Name;
                                    });
                                }
                            });
                            dataGrid.saveEditData();
                        }
                    });
                    e.cancel = true;

                }
                if (e.parentType == "dataRow" && e.dataField == "PctAllocation") {
                    if (e.row.key.preconRefIds != null) {
                        e.editorOptions.disabled = true;
                    }
                }
                if (e.parentType == "dataRow" && e.dataField == "PctAllocationConst") {
                    if (e.row.key.constRefIds != null) {
                        e.editorOptions.disabled = true;
                    }
                }
                if (e.parentType == "dataRow" && e.dataField == "PctAllocationCloseOut") {
                    if (e.row.key.closeOutRefIds != null) {
                        e.editorOptions.disabled = true;
                    }
                }
                if (e.parentType == "dataRow" && e.dataField == "SoftAllocation") {
                    e.editorName = "dxSwitch";
                }
            },
            onRowValidating: function (e) {
                if (typeof e.newData.AllocationEndDate !== "undefined") {
                    let value = new Date(e.newData.AllocationEndDate);
                    if (value.format() == 'Invalid Date' || String(value.getFullYear()).length > 4) {
                        let newdate = convertToValidDate(e.newData.AllocationEndDate);
                        if (newdate == 'Invalid year') {
                            e.isValid = false;
                            e.errorText = "Please enter a valid year in format YYYY.";
                        }
                        else {
                            if (yearpart < 1000) {
                                e.isValid = false;
                                e.errorText = "Please enter a valid year in format YYYY.";
                            }
                        }

                    }
                    if (typeof value != undefined) {
                        let yearpart = removeLeadingZeros(value.getFullYear());
                        let monthpart = value.getMonth();
                        let daypart = value.getDate();
                        if (isTwoDigitNumber(yearpart)) {
                            let firsttwodigits = getFirstTwoDigitsOfYear(new Date());
                            let newyearpart = parseInt(firsttwodigits.toString() + yearpart.toString())
                            var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                            var rowIndex = e.component.getRowIndexByKey(e.key);
                            e.component.cellValue(rowIndex, "AllocationEndDate", new Date(newyearpart, monthpart, daypart));
                            dataGrid.saveEditData();
                        }
                        else {
                            if (yearpart < 1000) {
                                e.isValid = false;
                                e.errorText = "Please enter a valid year in format YYYY.";
                            }
                        }

                    }
                    if (new Date(e.newData.AllocationEndDate) < new Date(e.key.AllocationStartDate)) {
                        e.isValid = false;
                        e.errorText = "StartDate should be less than EndDate";
                    }
                }
                if (typeof e.newData.AllocationStartDate !== "undefined") {
                    let value = new Date(e.newData.AllocationStartDate);
                    if (value.format() == 'Invalid Date' || String(value.getFullYear()).length > 4) {
                        let newdate = convertToValidDate(e.newData.AllocationStartDate);
                        if (newdate == 'Invalid year') {
                            e.isValid = false;
                            e.errorText = "Please enter a valid year in format YYYY.";
                        }
                        else {
                            e.isValid = false;
                            e.errorText = "Please enter a valid Start date.";
                        }
                    }
                    if (typeof value != undefined) {
                        let yearpart = removeLeadingZeros(value.getFullYear());
                        let monthpart = value.getMonth();
                        let daypart = value.getDate();
                        if (isTwoDigitNumber(yearpart)) {

                            let firsttwodigits = getFirstTwoDigitsOfYear(new Date());
                            let newyearpart = parseInt(firsttwodigits.toString() + yearpart.toString())
                            var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                            var rowIndex = e.component.getRowIndexByKey(e.key);
                            e.component.cellValue(rowIndex, "AllocationStartDate", new Date(newyearpart, monthpart, daypart));
                            dataGrid.saveEditData();
                        }
                        else {
                            if (yearpart < 1000) {
                                e.isValid = false;
                                e.errorText = "Please enter a valid year in format YYYY.";
                            }
                        }
                    }
                    if (new Date(e.key.AllocationEndDate) < new Date(e.newData.AllocationStartDate)) {
                        e.isValid = false;
                        e.errorText = "StartDate should be less than EndDate";
                    }
                }

                isGridInValidState = e.isValid;
            },
            onCellPrepared: function (e) {
                if (e.rowType === 'data') {
                    var preconstartDate = PreconStartDate;
                    var preconEndDate = PreconEndDate;

                    var conststartDate = ConstStartDate;
                    var constEndDate = ConstEndDate;

                    var closeoutstartDate = CloseoutStartDate;
                    var closeoutEndDate = CloseoutEndDate;

                    if (e.column.dataField == 'AllocationStartDate') {
                        let cellValue = new Date(e.data.AllocationStartDate)
                        let className = getDateBackgroundColor(cellValue, preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                        e.cellElement.addClass(className);
                    }
                    if (e.column.dataField == 'AllocationEndDate') {
                        let cellValue = new Date(e.data.AllocationEndDate)
                        let className = getDateBackgroundColor(cellValue, preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                        e.cellElement.addClass(className);
                    }
                }
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
                mode: 'Standard',
            },
            paging: { enabled: false },
            columns: [
                {
                    dataField: "TypeName",
                    dataType: "text",
                    caption: "Role",
                    sortIndex: "1",
                    sortOrder: "asc",
                    width: "50%",
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
                    width: "15%",
                    alignment: 'center',
                    cssClass: "v-align",
                    validationRules: [{ type: "required", message: '', }],
                    format: 'MMM d, yyyy',
                    sortIndex: "2",
                    sortOrder: "asc",
                    editorOptions: {
                        onFocusOut: function (e, options) {
                            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                            dataGrid.saveEditData();
                        },
                        onClosed: function (e) {
                            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                            dataGrid.saveEditData();
                        },
                    },
                },
                {
                    dataField: "AllocationEndDate",
                    caption: "End Date",
                    dataType: "date",
                    alignment: 'center',
                    cssClass: "v-align",
                    width: "15%",
                    validationRules: [{ type: "required", message: '', }],
                    format: 'MMM d, yyyy',
                    editorOptions: {
                        onFocusOut: function (e) {
                            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                            dataGrid.saveEditData();
                        },
                        onClosed: function (e) {
                            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                            dataGrid.saveEditData();
                        },
                    }
                },
                {
                    dataField: "PctAllocation",
                    caption: "% Alloc",
                    dataType: "text",
                    width: "5%",
                    setCellValue: function (newData, value, currentRowData) {
                        if (parseInt(value) <= 0) {
                            globaldata = globaldata.filter(x => x.ID != currentRowData.ID);
                            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                            dataGrid.option("dataSource", globaldata);
                        }
                        else {
                            newData.PctAllocation = value;
                        }
                    }
                },
                {
                    width: "8%",
                    fieldName: "SoftAllocation",
                    name: 'SoftAllocation',
                    caption: "",
                    dataType: "text",
                    alignment: 'center',
                    cellTemplate: function (container, options) {

                        $("<div id='divSoftAllocation' >").append(
                            $("<div id='divSwitch' />").dxSwitch({
                                switchedOffText: 'Hard',
                                switchedOnText: 'Soft',
                                width: 60,
                                value: options.data.SoftAllocation,
                                onValueChanged(data) {

                                },
                            })
                        ).appendTo(container);
                    },

                },
                {
                    width: "8%",
                    fieldName: "NonChargeable",
                    caption: 'NCO',
                    alignment: 'center',
                    dataType: 'text',
                    cellTemplate: function (container, options) {
                        $("<div id='divNonChargeable'>").append(
                            $("<div id='divSwitchNonChargeable' />").dxCheckBox({
                                //switchedOffText: 'BillHr',
                                //switchedOnText: 'NCO',
                                width: 30,
                                value: options.data.NonChargeable,
                                onValueChanged(data) {

                                },
                            })
                        ).appendTo(container);
                    },
                },
                {
                    width: "5%",
                    cellTemplate: function (container, options) {
                        var preconStartdate = PreconStartDate;
                        var preconEnddate = PreconEndDate;
                        if (preconEnddate == 'Jan 1, 0001' || preconStartdate == 'Jan 1, 0001') {
                            $("<div id='rowDelete' style='text-align:center;'>")
                                .append($("<img>", {
                                    "src": "/content/images/deleteIcon-new.png", "ID": options.data.ID, "TemplateID": options.data.TemplateID,
                                    "UserID": options.data.AssignedTo, "TypeName": options.data.TypeName, "Tags": options.data.Tags,
                                    "StartDate": options.data.AllocationStartDate, "EndDate": options.data.AllocationEndDate,
                                    "style": "overflow: auto;cursor: pointer;", "class": "imgDeleteNew", "title": "Delete", "width": "23px"
                                }))
                                .appendTo(container);
                        } else {
                            $("<div id='rowDelete' style='text-align:center;'>")
                                //.append($("<img>", { "src": "/Content/images/PreconCal2.png", "Index": options.rowIndex, "style": "overflow: auto;cursor: pointer;height:20px;width:20px;", "class": "imgPreconDate", "title":"Set Precon Date" }))
                                .append($("<img>", {
                                    "src": "/content/images/deleteIcon-new.png", "ID": options.data.ID, "TemplateID": options.data.TemplateID,
                                    "UserID": options.data.AssignedTo, "TypeName": options.data.TypeName, "Tags": options.data.Tags,
                                    "StartDate": options.data.AllocationStartDate, "EndDate": options.data.AllocationEndDate,
                                    "style": "overflow: auto;cursor: pointer;", "class": "imgDeleteNew", "title": "Delete", "width": "23px"
                                }))
                                .appendTo(container);
                        }
                    }
                },
            ],
            showBorders: true,
            showRowLines: true,
            selection: {
                mode: 'multiple',
                showCheckBoxesMode: 'always',
            },
            onSelectionChanged(e) {
                selectionObject = e;
                const data = e.selectedRowsData;
                strSelectedAllocationIDs = "";

                $.each(data, function (i, item) {
                    if (item.ID)
                        strSelectedAllocationIDs += item.ID + ",";
                });
            },
            onRowUpdating: function (e) {
                //lastEditedRow = e.key.ID;
                //openDateConfirmationDialog();
            },
            onCellClick: function (e) {
                if (e.column.fieldName == "SoftAllocation") {
                    globaldata.forEach(function (part, index, theArray) {
                        if (part.ID == e.data.ID) {
                            part.SoftAllocation = !part.SoftAllocation;
                        }
                    });
                }
                if (e.column.fieldName == "NonChargeable") {
                    globaldata.forEach(function (part, index, theArray) {
                        if (part.ID == e.data.ID) {
                            part.NonChargeable = !part.NonChargeable;
                        }
                    });
                }

            },
            onContentReady: function (e) {
                var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                if (dataGrid.getDataSource() !== null)
                    globaldata = dataGrid.getDataSource()._items;

                //clickUpdateSize();
            },
            toolbar: function (e) {
                e.toolbarOptions.visible = false;
            },
            onEditorPreparing: function (e) {
                if (e.parentType === 'dataRow' && e.dataField === 'TypeName') {

                    e.editorElement.dxSelectBox({

                        dataSource: _.sortBy(GroupsData, 'Name'),
                        valueExpr: "Id",
                        displayExpr: "Name",
                        value: e.row.data.Type,
                        searchEnabled: true,
                        onValueChanged: function (ea) {
                            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                            //let preconStartDate = PreconStartDate;
                            //let preconEndDate = PreconEndDate;

                            //let constStartDate = ConstStartDate;
                            //let constEndDate = ConstEndDate;

                            //let closeoutStartDate = CloseoutStartDate;
                            //let closeoutEndDate = CloseoutEndDate;
                            //let fData = globaldata.filter(x => x.Type == ea.value && x.AssignedTo != dataGrid.getDataSource()._items[e.row.rowIndex].AssignedTo);
                            //let record = globaldata.filter(x => x.ID == dataGrid.getDataSource()._items[e.row.rowIndex].ID)[0]; 
                            //if (fData != null && fData.length > 0) {
                            //    let overlapDates = {
                            //        precon: false,
                            //        const: false,
                            //        closeout: false
                            //    };
                            //    $.each(fData, function (index, e) {
                            //        let startDate = new Date(e.AllocationStartDate);
                            //        let endDate = new Date(e.AllocationEndDate);

                            //        if (!isDateValid(startDate) || !isDateValid(endDate)) {
                            //            return;
                            //        }
                            //        if (startDate <= preconEndDate && endDate >= preconStartDate) {
                            //            overlapDates.precon = true;
                            //        }
                            //        if (startDate <= constEndDate && endDate >= constStartDate) {
                            //            overlapDates.const = true;
                            //        }
                            //        if (startDate <= closeoutEndDate && endDate >= closeoutStartDate) {
                            //            overlapDates.closeout = true;
                            //        }
                            //    });

                            //    let startDate = new Date(record.AllocationStartDate);
                            //    let endDate = new Date(record.AllocationEndDate);

                            //    if (!isDateValid(startDate) || !isDateValid(endDate)) {
                            //        return;
                            //    }
                            //    if (startDate <= preconEndDate && endDate >= preconStartDate) {
                            //        if (!overlapDates.precon) {
                            //            dataGrid.getDataSource()._items[e.row.rowIndex].AssignedTo = fData[0].AssignedTo;
                            //        }
                            //    }
                            //    if (startDate <= constEndDate && endDate >= constStartDate) {
                            //        if (!overlapDates.const) {
                            //            dataGrid.getDataSource()._items[e.row.rowIndex].AssignedTo = fData[0].AssignedTo;
                            //        }
                            //    }
                            //    if (startDate <= closeoutEndDate && endDate >= closeoutStartDate) {
                            //        if (!overlapDates.closeout) {
                            //            dataGrid.getDataSource()._items[e.row.rowIndex].AssignedTo = fData[0].AssignedTo;
                            //        }
                            //    }
                            //}
                            dataGrid.getDataSource()._items[e.row.rowIndex].AssignedTo = '';
                            $.each(ea.component._dataSource._items, function (i, v) {
                                if (v.Id === ea.value) {
                                    e.setValue(v.Name);
                                    dataGrid.getDataSource()._items[e.row.rowIndex].Type = v.Id;
                                    dataGrid.getDataSource()._items[e.row.rowIndex].TypeName = v.Name;
                                }
                            });
                            dataGrid.saveEditData();
                        }
                    });
                    e.cancel = true;

                }
            },
            onRowValidating: function (e) {
                if (typeof e.newData.AllocationEndDate !== "undefined") {
                    let value = new Date(e.newData.AllocationEndDate);
                    if (value.format() == 'Invalid Date' || String(value.getFullYear()).length > 4) {
                        let newdate = convertToValidDate(e.newData.AllocationEndDate);
                        if (newdate == 'Invalid year') {
                            e.isValid = false;
                            e.errorText = "Please enter a valid year in format YYYY.";
                        }
                        else {
                            e.isValid = false;
                            e.errorText = "Please enter a valid End date.";
                        }
                    }
                    if (typeof value != undefined) {
                        let yearpart = removeLeadingZeros(value.getFullYear());
                        let monthpart = value.getMonth();
                        let daypart = value.getDate();
                        if (isTwoDigitNumber(yearpart)) {
                            let firsttwodigits = getFirstTwoDigitsOfYear(new Date());
                            let newyearpart = parseInt(firsttwodigits.toString() + yearpart.toString())
                            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                            var rowIndex = e.component.getRowIndexByKey(e.key);
                            e.component.cellValue(rowIndex, "AllocationEndDate", new Date(newyearpart, monthpart, daypart));
                            dataGrid.saveEditData();
                        }
                        else {
                            if (yearpart < 1000) {
                                e.isValid = false;
                                e.errorText = "Please enter a valid year in format YYYY.";
                            }
                        }

                    }

                }
                if (typeof e.newData.AllocationStartDate !== "undefined") {
                    let value = new Date(e.newData.AllocationStartDate);
                    if (value.format() == 'Invalid Date' || String(value.getFullYear()).length > 4) {
                        let newdate = convertToValidDate(e.newData.AllocationStartDate);
                        if (newdate == 'Invalid year') {
                            e.isValid = false;
                            e.errorText = "Please enter a valid year in format YYYY.";
                        }
                        else {
                            e.isValid = false;
                            e.errorText = "Please enter a valid Start date.";
                        }
                    }
                    if (typeof value != undefined) {
                        let yearpart = removeLeadingZeros(value.getFullYear());
                        let monthpart = value.getMonth();
                        let daypart = value.getDate();
                        if (isTwoDigitNumber(yearpart)) {

                            let firsttwodigits = getFirstTwoDigitsOfYear(new Date());
                            let newyearpart = parseInt(firsttwodigits.toString() + yearpart.toString())
                            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                            var rowIndex = e.component.getRowIndexByKey(e.key);
                            e.component.cellValue(rowIndex, "AllocationStartDate", new Date(newyearpart, monthpart, daypart));
                            dataGrid.saveEditData();
                        }
                        else {
                            if (yearpart < 1000) {
                                e.isValid = false;
                                e.errorText = "Please enter a valid year in format YYYY.";
                            }
                        }
                    }
                }
                $.cookie("dataChanged", 1, { path: "/" });
                $.cookie("projTeamAllocSaved", 0, { path: "/" });
                $('#btnCancelChanges').dxButton({ visible: true });
                isGridInValidState = e.isValid;
            },
            onCellPrepared: function (e) {
                if (e.rowType === 'data') {
                    var preconstartDate = PreconStartDate;
                    var preconEndDate = PreconEndDate;

                    var conststartDate = ConstStartDate;
                    var constEndDate = ConstEndDate;

                    var closeoutstartDate = CloseoutStartDate;
                    var closeoutEndDate = CloseoutEndDate;

                    if (e.column.dataField == 'AllocationStartDate') {

                        let cellValue = new Date(e.data.AllocationStartDate)
                        let className = getDateBackgroundColor(cellValue, preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                        e.cellElement.addClass(className);

                    }
                    if (e.column.dataField == 'AllocationEndDate') {
                        let cellValue = new Date(e.data.AllocationEndDate)
                        let className = getDateBackgroundColor(cellValue, preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                        e.cellElement.addClass(className);
                    }
                }
            }
        });

        $("#btnDetailedSummary").dxButton({
            text: "Detailed View",
            focusStateEnabled: false,
            visible: !isReadOnly,
            onClick: function (e) {
                CheckPhaseConstraints(false);
                CompactPhaseConstraints();
                if ($("#gridTemplateContainer").is(":visible")) {
                    $("#gridTemplateContainer").hide();
                    $("#compactGridTemplateContainer").show();
                    $("#btnDetailedSummary span").text("Detailed View");
                    $("#btnPrecondate, #btnConstructionDate, #btnCloseoutDate").hide();
                    $(".schedule-label").show();
                    $("#btnSaveAsTemplate").hide();
                    //$("#btnAddNew").show();
                    var compactgrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                    compactgrid.option("dataSource", CompactPhaseConstraints());
                }
                else {
                    $("#compactGridTemplateContainer").hide();
                    $("#gridTemplateContainer").show();
                    $("#btnDetailedSummary span").text("Summary View");
                    //$("#btnAddNew").hide();
                    $("#btnPrecondate, #btnConstructionDate, #btnCloseoutDate").show();
                    $(".schedule-label").hide();
                    var compactgrid = $("#gridTemplateContainer").dxDataGrid("instance");
                    compactgrid.option("dataSource", globaldata);
                }
            }
        });

        $("#btnAddNew").dxButton({
            text: "Add New",
            icon: "/content/Images/plus-blue-new.png",
            focusStateEnabled: false,
            visible: !isReadOnly,
            onClick: function (s, e) {
                var projectStartdate = PreconStartDate;
                var projectEnddate = CloseoutEndDate;
                var sum = 0;
                if ($("#gridTemplateContainer").is(":visible")) {
                    projectStartdate = undefined;
                    projectEnddate = undefined;
                }
                //var minValue = Math.min(...globaldata.map(x => x.ID));
                //if (parseInt(minValue) < 0) {
                //    sum = minValue - 1;
                //}
                //else {
                sum = globaldata.length + 1;
                //}
                globaldata.push({ "ID": -Math.abs(sum), "AssignedTo": "User" + sum, "AssignedToName": "", "AllocationStartDate": projectStartdate, "AllocationEndDate": projectEnddate, "PctAllocation": 100, "SoftAllocation": false, "NonChargeable": 0, "Type": 'TYPE-' + sum, "TypeName": '', "Tags": '' });

                var grid = $("#gridTemplateContainer").dxDataGrid("instance");
                grid.option("dataSource", globaldata);

                var compactgrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                compactgrid.option("dataSource", CompactPhaseConstraints());
            }
        });

        $("#btnSaveAllocation").dxButton({
            text: "Save Template",
            icon: "/content/Images/save-open-new-wind.png",
            focusStateEnabled: false,
            visible: !isReadOnly,
            onClick: async function (e) {
                var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                dataGrid.saveEditData();
                if (!isGridInValidState) {
                    return false;
                }
                SaveTemplate();
                e.event.stopPropagation();
            }
        });

        $('#createNewTemplateCHK').dxCheckBox({
            value: false,
            text: 'Save as New Template',
            visible: !isReadOnly && requestFromRMM
        });

        $('#templateNameTxt').dxTextBox({
            value: templateName,
            height: 33,
            inputAttr: { 'aria-label': 'Name' },
            visible: requestFromRMM,
            disabled: isReadOnly,
        });

        $("#btnPrecondate").dxButton({
            text: "Select Precon Dates",
            icon: "/content/Images/RMONE/calender_activeWhite.png",
            focusStateEnabled: false,
            elementAttr: {
                class: 'Precon_Btn'
            },
            onClick: function (e) {
                var startDate = PreconStartDate;
                var EndDate = PreconEndDate;
                updateDatesInGrid(startDate, EndDate, 'black');
                $('#btnCancelChanges').dxButton({ visible: true });
            }
        });
        $("#btnConstructionDate").dxButton({
            text: "Select Const. Dates",
            icon: "/content/Images/RMONE/calender_activeWhite.png",
            focusStateEnabled: false,
            elementAttr: {
                class: 'Const_Btn'
            },
            onClick: function (e) {
                var startDate = ConstStartDate;
                var EndDate = ConstEndDate;
                updateDatesInGrid(startDate, EndDate, '#fff');
                $('#btnCancelChanges').dxButton({ visible: true });
            }
        });

        $("#btnCloseoutDate").dxButton({
            text: "Select Closeout Dates",
            icon: "/content/Images/RMONE/calender_activeWhite.png",
            focusStateEnabled: false,
            elementAttr: {
                class: 'Closeout_Btn'
            },
            onClick: function (e) {
                var startDate = CloseoutStartDate;
                var EndDate = CloseoutEndDate;
                updateDatesInGrid(startDate, EndDate, '#fff');
                $('#btnCancelChanges').dxButton({ visible: true });
            }
        });
    }
    $(function () {
        GetTemplateDetails(<%=TemplateId%>);
        if (!requestFromRMM) {
            $(".templateHeader").hide();
        }
        GetProjectTemplates();
    });

    function SaveTemplate() {
        let isValid = true;
        let oldTemplateName = templateName;
        let saveOnExiting = $('#createNewTemplateCHK').dxCheckBox('instance').option("visible")
            ? !$('#createNewTemplateCHK').dxCheckBox('instance').option("value") : true;
        let tName = $('#templateNameTxt').dxTextBox('instance').option("visible")
            ? $('#templateNameTxt').dxTextBox('instance').option("value") : templateName;

        if (tName.trim() == '') {
            DevExpress.ui.dialog.alert("Please Enter Template Name", "Error!");
            isValid = false;
            return false;
        }
        if (($('#createNewTemplateCHK').dxCheckBox('instance').option("value") == true || oldTemplateName.trim().toLowerCase() != tName.trim().toLowerCase()) && ProjectTemplates.filter(x => x.Name.trim().toLowerCase() == tName.trim().toLowerCase()).length > 0) {
            DevExpress.ui.dialog.alert("Template name already exists.", "Error!");
            isValid = false;
            return false;
        }

        if (globaldata == null || globaldata.length == 0) {
            DevExpress.ui.dialog.alert("No allocation exists to be saved as a Template.", "Error!");
            isValid = false;
            return false;
        }
        
        $.each(globaldata, function (i, s) {
            if (s.TypeName == '') {
                DevExpress.ui.dialog.alert("Role is Required", "Error!");
                isValid = false;
                return false;
            }

            if (typeof (s.AllocationStartDate) == "object") {
                if (s.AllocationStartDate != null) {
                    s.AllocationStartDate = (s.AllocationStartDate).toISOString();
                }
                else {
                    DevExpress.ui.dialog.alert("Start Date should not be blank.", "Error!");
                    isValid = false;
                    return false;
                }
            } else if (s.AllocationStartDate == '') {
                DevExpress.ui.dialog.alert("Start Date should not be blank.", "Error!");
                isValid = false;
                return false;
            }

            if (typeof (s.AllocationEndDate) == "object") {
                if (s.AllocationEndDate != null) {
                    s.AllocationEndDate = (s.AllocationEndDate).toISOString();
                }
                else {
                    DevExpress.ui.dialog.alert("End Date should not be blank.", "Error!");
                    isValid = false;
                    return false;
                }
            } else if (s.AllocationEndDate == '') {
                DevExpress.ui.dialog.alert("End Date should not be blank.", "Error!");
                isValid = false;
                return false;
            }

            if (new Date(s.AllocationEndDate) < new Date(s.AllocationStartDate)) {
                DevExpress.ui.dialog.alert("Start Date should be less than End Date.", "Error!");
                isValid = false;
                return false;
            }
        });
        if (isValid) {
            if (oldTemplateName.trim().toLowerCase() != tName.trim().toLowerCase() || $('#createNewTemplateCHK').dxCheckBox('instance').option("value") == true) {
                set_cookie("templatenamechanged", "true");
            }
            else {
                set_cookie("templatenamechanged", "");
            }
            var data = {
                Name: tName, TicketID: projectID, ModuleName: moduleName, StartDate: PreconStartDate.toISOString(),
                EndDate: CloseoutEndDate.toISOString(), Duration: GetDurationInWeek(ajaxHelperPage, PreconStartDate, CloseoutEndDate), SaveOnExiting: saveOnExiting, ID: templateId,
                PreconStartDate: PreconStartDate.toISOString(), PreconEndDate: PreconEndDate.toISOString(), ConstStartDate: ConstStartDate.toISOString(),
                ConstEndDate: ConstEndDate.toISOString(), CloseOutStartDate: CloseoutStartDate.toISOString(), CloseOutEndDate: CloseoutEndDate.toISOString(),
                Allocations: JSON.stringify(globaldata)
            };
            $.post(ugitConfig.apiBaseUrl + "/api/RMMAPI/SaveAllocationAsTemplate", data).then(function (response) {
                //console.log(response);
                //templateId = response;
                $('#createNewTemplateCHK').dxCheckBox('instance').option("value", false);
                $("#toast").dxToast("show");
                GetTemplateDetails(response);
                GetProjectTemplates();
            });
        }
    }
    function getDateBackgroundColor(cellValue, preconstartDate, conststartDate, constEndDate, closeoutstartDate) {

        let preconEndDate = PreconEndDate;
        let closeoutEndDate = CloseoutEndDate;

        if (isDateValid(closeoutstartDate) && isDateValid(closeoutEndDate)
            && cellValue >= closeoutstartDate && cellValue <= closeoutEndDate) {
            return 'closeoutCellStyle';
        }
        else if (isDateValid(conststartDate) && isDateValid(constEndDate) &&
            cellValue <= constEndDate && cellValue >= conststartDate) {
            return 'constCellStyle';
        }
        else if (isDateValid(preconstartDate) && isDateValid(preconEndDate)
            && cellValue >= preconstartDate && cellValue <= preconEndDate) {
            return 'preconCellStyle';
        }
        else
            return 'noDateCellStyle';
    }
    function OpenInternalAllocation(refIds) {
        let ids = refIds.split(";");
        if (ids != null && ids.length > 0) {
            let gdata = globaldata.filter(x => ids.includes(String(x.ID)));
            const popupContentTemplate1 = function () {
                let container = $("<div>");
                let datagrid = $("<div id='InternalAllocationGrid'>").dxDataGrid({
                    dataSource: gdata,
                    ID: "grdTemplate",
                    editing: {
                        mode: "cell",
                        allowEditing: !isReadOnly,
                        allowUpdating: !isReadOnly
                    },
                    sorting: {
                        mode: "multiple" // or "multiple" | "none"
                    },
                    scrolling: {
                        mode: 'infinite',
                    },
                    columns: [
                        {
                            dataField: "TypeName",
                            dataType: "text",
                            allowEditing: false,
                            caption: "Role",
                            sortIndex: "1",
                            sortOrder: "asc",
                            width: "50%",
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
                            width: "20%",
                            alignment: 'center',
                            cssClass: "v-align",
                            allowEditing: false,
                            //validationRules: [{ type: "required", message: '', }],
                            format: 'MMM d, yyyy',
                            sortIndex: "2",
                            sortOrder: "asc",
                            editorOptions: {
                                onFocusOut: function (e, options) {
                                    var dataGrid = $("#InternalAllocationGrid").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                },
                                onClosed: function (e) {
                                    var dataGrid = $("#InternalAllocationGrid").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                }
                            }
                        },
                        {
                            dataField: "AllocationEndDate",
                            caption: "End Date",
                            dataType: "date",
                            alignment: 'center',
                            cssClass: "v-align",
                            width: "20%",
                            allowEditing: false,
                            //validationRules: [{ type: "required", message: '', }],
                            format: 'MMM d, yyyy',
                            editorOptions: {
                                onFocusOut: function (e) {
                                    var dataGrid = $("#InternalAllocationGrid").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                },
                                onClosed: function (e) {
                                    var dataGrid = $("#InternalAllocationGrid").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                },
                            }
                        },
                        {
                            dataField: "PctAllocation",
                            caption: "% Alloc",
                            dataType: "text",
                            setCellValue: function (newData, value, currentRowData) {
                                if (parseInt(value) <= 0) {
                                    globaldata = globaldata.filter(x => x.ID != currentRowData.ID);
                                    let gdata = globaldata.filter(x => refIds.includes(String(x.ID)));
                                    let dataGridChild = $("#InternalAllocationGrid").dxDataGrid("instance");
                                    dataGridChild.option("dataSource", gdata);
                                    var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                                    dataGrid.option("dataSource", CompactPhaseConstraints());
                                }
                                else {
                                    newData.PctAllocation = value;
                                }
                            }
                        },
                    ],
                    showBorders: true,
                    showRowLines: true,
                    onCellPrepared: function (e) {
                        if (e.rowType === 'data') {
                            var preconstartDate = PreconStartDate;
                            var preconEndDate = PreconEndDate;

                            var conststartDate = ConstStartDate;
                            var constEndDate = ConstEndDate;

                            var closeoutstartDate = CloseoutStartDate;
                            var closeoutEndDate = CloseoutEndDate;

                        if (e.column.dataField == 'AllocationStartDate') {
                            let cellValue = new Date(e.data.AllocationStartDate)
                            let className = getDateBackgroundColor(cellValue, preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                            e.cellElement.addClass(className);
                        }
                        if (e.column.dataField == 'AllocationEndDate') {
                            let cellValue = new Date(e.data.AllocationEndDate)
                            let className = getDateBackgroundColor(cellValue, preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                            e.cellElement.addClass(className);
                        }
                    }
                }
            });
                let confirmBtn = $(`<div class="mt-4 mb-2 btnAddNew" style='float:right;padding: 0px 10px;font-size: 14px;' />`).dxButton({
                    text: "Save",
                    hint: 'Save Allocations',
                    visible: !isReadOnly,
                    onClick: function (e) {
                        let dataGridChild = $("#InternalAllocationGrid").dxDataGrid("instance");
                        let rowschild = dataGridChild.getVisibleRows();
                        globaldata = globaldata.filter(x => !ids.includes(String(x.ID)));
                        $.each(rowschild, function (index, e) {
                            if (parseInt(e.data.PctAllocation) > 0) {
                                globaldata.push(e.data);
                            }
                        });
                        var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                        dataGrid.option("dataSource", CompactPhaseConstraints());
                        //var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                        //dataGrid.option("dataSource", globaldata);
                        //CompactPhaseConstraints();
                        popup.hide();
                    }
                })
                let cancelBtn = $(`<div class="mt-4 mb-2 btnAddNew" style='float:right;padding: 0px 10px;font-size: 14px;' />`).dxButton({
                    text: "Cancel",
                    visible: true,
                    onClick: function (e) {
                        popup.hide();
                    }
                })
                container.append(datagrid);
                container.append(confirmBtn);
                container.append(cancelBtn);
                return container;
            };

            const popup = $("#InternalAllocationGridDialog").dxPopup({
                contentTemplate: popupContentTemplate1,
                width: "900",
                height: "auto",
                showTitle: true,
                title: isReadOnly ? "View Allocations" : "Edit Allocations",
                visible: false,
                dragEnabled: true,
                hideOnOutsideClick: true,
                showCloseButton: true,
                position: {
                    at: 'center',
                    my: 'center',
                },
                onHiding: function () {

                }
            }).dxPopup('instance');

            popup.option({
                contentTemplate: () => popupContentTemplate1()

            });
            popup.show();
        }
    }
    function SaveToGlobalData(row) {
        let data = row.key;
        if (parseInt(row.oldData.PctAllocation) == 0 && parseInt(row.newData.PctAllocation) > 0) {
            var sum = 0;
            sum = globaldata.length + 100 + Math.floor((Math.random() * 100) + 1);
            globaldata.push({ "ID": -Math.abs(sum), "AssignedTo": data.AssignedTo, "AssignedToName": data.AssignedToName, "AllocationStartDate": PreconStartDate, "AllocationEndDate": PreconEndDate, "PctAllocation": parseInt(row.newData.PctAllocation), "SoftAllocation": false, "NonChargeable": false, "Type": data.Type, "TypeName": data.TypeName, "Tags": ''});
        }
        if (parseInt(row.oldData.PctAllocationConst) == 0 && parseInt(row.newData.PctAllocationConst) > 0) {
            var sum = 0;
            sum = globaldata.length + 200 + Math.floor((Math.random() * 100) + 1);
            globaldata.push({ "ID": -Math.abs(sum), "AssignedTo": data.AssignedTo, "AssignedToName": data.AssignedToName, "AllocationStartDate": ConstStartDate, "AllocationEndDate": ConstEndDate, "PctAllocation": parseInt(row.newData.PctAllocationConst), "SoftAllocation":false, "NonChargeable": false, "Type": data.Type, "TypeName": data.TypeName, "Tags": ''});
        }
        if (parseInt(row.oldData.PctAllocationCloseOut) == 0 && parseInt(row.newData.PctAllocationCloseOut) > 0) {
            var sum = 0;
            sum = globaldata.length + 300 + Math.floor((Math.random() * 100) + 1);
            globaldata.push({ "ID": -Math.abs(sum), "AssignedTo": data.AssignedTo, "AssignedToName": data.AssignedToName, "AllocationStartDate": CloseoutStartDate, "AllocationEndDate": CloseoutEndDate, "PctAllocation": parseInt(row.newData.PctAllocationCloseOut), "SoftAllocation": false, "NonChargeable": false, "Type": data.Type, "TypeName": data.TypeName, "Tags": '' });
        }
        let gdata = globaldata.filter(x => x.ID == data.PreconId || x.ID == data.ConstId || x.ID == data.CloseOutId);
        $.each(gdata, function (index, e) {
            if (e.ID == data.PreconId && row.newData.PctAllocation != null && row.newData.PctAllocation != "") {
                if (parseInt(row.newData.PctAllocation) > 0) {
                    e.PctAllocation = row.newData.PctAllocation;
                    data.PctAllocation = e.PctAllocation;
                }
                else {
                    globaldata = globaldata.filter(x => x.ID != e.ID);
                }
            }
            if (e.ID == data.ConstId && row.newData.PctAllocationConst != null && row.newData.PctAllocationConst != "") {
                if (parseInt(row.newData.PctAllocationConst) > 0) {
                    e.PctAllocation = row.newData.PctAllocationConst;
                    data.PctAllocation = e.PctAllocation;
                }
                else {
                    globaldata = globaldata.filter(x => x.ID != e.ID);
                }
            }
            if (e.ID == data.CloseOutId && row.newData.PctAllocationCloseOut != null && row.newData.PctAllocationCloseOut != "") {
                if (parseInt(row.newData.PctAllocationCloseOut) > 0) {
                    e.PctAllocation = row.newData.PctAllocationCloseOut;
                    data.PctAllocation = e.PctAllocation;
                }
                else {
                    globaldata = globaldata.filter(x => x.ID != e.ID);
                }
            }
            if (row.newData.TypeName != null) {
                e.TypeName = row.newData.TypeName;
            }
        });
        if (row.newData.AllocationEndDate != null) {
            data.AllocationEndDate = row.newData.AllocationEndDate;
            data.AllocationStartDate = row.newData.AllocationStartDate != "" && row.newData.AllocationStartDate != undefined
                ? row.newData.AllocationStartDate : data.AllocationStartDate;
            globaldata = globaldata.filter(x => x.ID != data.PreconId && x.ID != data.ConstId && x.ID != data.CloseOutId);
            globaldata.push(data);
            CheckPhaseConstraints(false);
        }
        else if (row.newData.AllocationStartDate != null) {
            data.AllocationStartDate = row.newData.AllocationStartDate;
            globaldata = globaldata.filter(x => x.ID != data.PreconId && x.ID != data.ConstId && x.ID != data.CloseOutId);
            globaldata.push(data);
            CheckPhaseConstraints(false);
        }

        var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
        dataGrid.option("dataSource", CompactPhaseConstraints());
    }
    function CompactPhaseConstraints() {
        CheckPhaseConstraints(false);
        compactTempData = [];
        let tempData = JSON.parse(JSON.stringify(globaldata));
        let isDateInMultiPhase = false;
        $.each(tempData, function (index, e) {
            let data1 = JSON.parse(JSON.stringify(tempData.filter(x => x.AssignedTo == e.AssignedTo && x.Type == e.Type)));
            let internalPhaseData = [];
            let constStartDate = ConstStartDate;
            let constEndDate = ConstEndDate;

            let preconStartDate = PreconStartDate;
            let preconEndDate = PreconEndDate;

            let closeoutStartDate = CloseoutStartDate;
            let closeoutEndDate = CloseoutEndDate;

            if (!isDateValid(constStartDate) && !isDateValid(constEndDate) && isDateValid(preconStartDate) && isDateValid(preconEndDate)) {
                constStartDate = preconStartDate;
                constEndDate = preconEndDate;
            }

            let internalPrecon = JSON.parse(JSON.stringify(data1.filter(x => new Date(x.AllocationStartDate) < constStartDate)));
            let internalConst = JSON.parse(JSON.stringify(data1.filter(x => new Date(x.AllocationStartDate) >= constStartDate && new Date(x.AllocationEndDate) <= constEndDate)));
            let internalCloseOut = JSON.parse(JSON.stringify(data1.filter(x => new Date(x.AllocationStartDate) > constEndDate)));
            if (internalPrecon.length > 1) {
                let internalPreconTemp = JSON.parse(JSON.stringify(internalPrecon[0]));
                let ids = [];
                let endDateForPctCal = new Date(Math.max.apply(null, internalPrecon.filter(x => new Date(x.AllocationEndDate) >= preconStartDate).map(x => new Date(x.AllocationEndDate))));
                let startDateForPctCal = new Date(Math.min.apply(null, internalPrecon.filter(x => new Date(x.AllocationEndDate) >= preconStartDate).map(x => new Date(x.AllocationStartDate))));
                internalPreconTemp.AllocationEndDate = new Date(Math.max.apply(null, internalPrecon.map(x => new Date(x.AllocationEndDate))));
                internalPreconTemp.AllocationStartDate = new Date(Math.min.apply(null, internalPrecon.map(x => new Date(x.AllocationStartDate))));
                let percentage = CalculatePctAllocation(internalPrecon, startDateForPctCal, endDateForPctCal);
                internalPreconTemp.PctAllocation = percentage <= 0 ? Math.max.apply(null, internalPrecon.map(x => parseInt(x.PctAllocation))) : percentage;

                $.each(internalPrecon, function (index, e) {
                    ids.push(e.ID);
                });
                internalPreconTemp.preconRefIds = ids.join(';');
                internalPhaseData.push(internalPreconTemp);
            }
            else if (internalPrecon.length) {
                internalPhaseData.push(internalPrecon[0]);
            }

            if (internalConst.length > 1) {
                let internalConstTemp = JSON.parse(JSON.stringify(internalConst[0]));
                let ids = [];
                internalConstTemp.AllocationEndDate = new Date(Math.max.apply(null, internalConst.map(x => new Date(x.AllocationEndDate))));
                internalConstTemp.AllocationStartDate = new Date(Math.min.apply(null, internalConst.map(x => new Date(x.AllocationStartDate))));
                internalConstTemp.PctAllocation = CalculatePctAllocation(internalConst, internalConstTemp.AllocationStartDate, internalConstTemp.AllocationEndDate);
                $.each(internalConst, function (index, e) {
                    ids.push(e.ID);
                });
                internalConstTemp.constRefIds = ids.join(';');
                internalPhaseData.push(internalConstTemp);
            }
            else if (internalConst.length) {
                internalPhaseData.push(internalConst[0]);
            }

            if (internalCloseOut.length > 1) {
                let internalCloseOutTemp = JSON.parse(JSON.stringify(internalCloseOut[0]));
                let ids = [];
                let endDateForPctCal = new Date(Math.max.apply(null, internalCloseOut.filter(x => new Date(x.AllocationStartDate) <= closeoutEndDate).map(x => new Date(x.AllocationEndDate))));
                let startDateForPctCal = new Date(Math.min.apply(null, internalCloseOut.filter(x => new Date(x.AllocationStartDate) <= closeoutEndDate).map(x => new Date(x.AllocationStartDate))));
                internalCloseOutTemp.AllocationEndDate = new Date(Math.max.apply(null, internalCloseOut.map(x => new Date(x.AllocationEndDate))));
                internalCloseOutTemp.AllocationStartDate = new Date(Math.min.apply(null, internalCloseOut.map(x => new Date(x.AllocationStartDate))));

                let percentage = CalculatePctAllocation(internalCloseOut, startDateForPctCal, endDateForPctCal);
                internalCloseOutTemp.PctAllocation = percentage <= 0 ? Math.max.apply(null, internalCloseOut.map(x => parseInt(x.PctAllocation))) : percentage;

                $.each(internalCloseOut, function (index, e) {
                    ids.push(e.ID);
                });
                internalCloseOutTemp.closeOutRefIds = ids.join(';');
                internalPhaseData.push(internalCloseOutTemp);
            }
            else if (internalCloseOut.length) {
                internalPhaseData.push(internalCloseOut[0]);
            }
            let remainingData = data1.filter(x => x.AllocationStartDate == "" && x.AllocationEndDate == "");
            if (remainingData.length > 0) {
                internalPhaseData.push(remainingData[0]);
            }
            let data = JSON.parse(JSON.stringify(internalPhaseData));
            let odata = JSON.parse(JSON.stringify(internalPhaseData));
            if (data.length > 0) {
                if (data.length > 1) {
                    data[0].AllocationEndDate = data[data.length - 1].AllocationEndDate
                    if (data.length == 2) {
                        let startDate = new Date(data[0].AllocationStartDate);
                        let endDate = new Date(data[0].AllocationEndDate);

                        let constStartDate = ConstStartDate;
                        let constEndDate = ConstEndDate;

                        let preconStartDate = PreconStartDate;
                        let preconEndDate = PreconEndDate;


                        if (!isDateValid(preconStartDate) && startDate < constStartDate) {
                            preconStartDate = startDate;
                        }
                        if (!isDateValid(preconEndDate) && isDateValid(constStartDate)) {
                            preconEndDate = constStartDate.addDays(-1);
                        }
                        if (startDate < constStartDate && endDate > constEndDate) {
                            data[0].PctAllocation = data[0].PctAllocation;
                            if (new Date(odata[0].AllocationEndDate) < preconEndDate || new Date(odata[0].AllocationStartDate) > preconStartDate) {
                                //let percentage = CalculatePctAllocation([odata[0]], preconStartDate, preconEndDate);
                                //data[0].PctAllocation = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                                data[0].preconRefIds = data[0].preconRefIds == null || data[0].preconRefIds == '' ? data[0].ID : data[0].preconRefIds;
                            }

                            data[0].PctAllocationCloseOut = data[1].PctAllocation;
                            if (new Date(odata[1].AllocationEndDate) < closeoutEndDate || new Date(odata[1].AllocationStartDate) > closeoutStartDate) {
                                //let percentage = CalculatePctAllocation([odata[1]], closeoutStartDate, closeoutEndDate);
                                //data[0].PctAllocationCloseOut = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                                data[0].closeOutRefIds = data[1].closeOutRefIds == null || data[1].closeOutRefIds == '' ? data[1].ID : data[1].closeOutRefIds;//data[1].ID;
                            }

                            data[0].PctAllocationConst = 0;
                            data[0].PreconId = data[0].ID;
                            data[0].ConstId = 0;
                            data[0].CloseOutId = data[1].ID;
                            if (data[1].closeOutRefIds != null)
                                data[0].closeOutRefIds = data[1].closeOutRefIds;
                        }
                        else if (startDate < constStartDate) {

                            data[0].PctAllocation = data[0].PctAllocation;
                            if (new Date(odata[0].AllocationEndDate) < preconEndDate || new Date(odata[0].AllocationStartDate) > preconStartDate) {
                                //let percentage = CalculatePctAllocation([odata[0]], preconStartDate, preconEndDate);
                                //data[0].PctAllocation = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                                data[0].preconRefIds = data[0].preconRefIds == null || data[0].preconRefIds == '' ? data[0].ID : data[0].preconRefIds;
                            }

                            data[0].PctAllocationConst = data[1].PctAllocation;
                            if (new Date(odata[1].AllocationEndDate) < constEndDate || new Date(odata[1].AllocationStartDate) > constStartDate) {
                                //data[0].PctAllocationConst = CalculatePctAllocation([odata[1]], constStartDate, constEndDate);
                                data[0].constRefIds = data[1].constRefIds == null || data[1].constRefIds == '' ? data[1].ID : data[1].constRefIds; //data[1].ID;
                            }

                            data[0].PctAllocationCloseOut = 0;
                            data[0].PreconId = data[0].ID;
                            data[0].ConstId = data[1].ID;
                            data[0].CloseOutId = 0;
                            if (data[1].constRefIds != null)
                                data[0].constRefIds = data[1].constRefIds;
                        }
                        else {

                            data[0].PctAllocationConst = data[0].PctAllocation;
                            if (new Date(odata[0].AllocationEndDate) < constEndDate || new Date(odata[0].AllocationStartDate) > constStartDate) {
                                //data[0].PctAllocationConst = CalculatePctAllocation([odata[0]], constStartDate, constEndDate);
                                data[0].constRefIds = data[0].constRefIds == null || data[0].constRefIds == '' ? data[0].ID : data[0].constRefIds;
                            }

                            data[0].PctAllocationCloseOut = data[1].PctAllocation;
                            if (new Date(odata[1].AllocationEndDate) < closeoutEndDate || new Date(odata[1].AllocationStartDate) > closeoutStartDate) {
                                //let percentage = CalculatePctAllocation([odata[1]], closeoutStartDate, closeoutEndDate);
                                //data[0].PctAllocationCloseOut = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                                data[0].closeOutRefIds = data[1].closeOutRefIds == null || data[1].closeOutRefIds == '' ? data[1].ID : data[1].closeOutRefIds; //data[1].ID;
                            }

                            data[0].PctAllocation = 0;
                            data[0].PreconId = 0;
                            data[0].ConstId = data[0].ID;
                            data[0].CloseOutId = data[1].ID;
                            if (data[1].closeOutRefIds != null)
                                data[0].closeOutRefIds = data[1].closeOutRefIds;
                        }
                    }
                    else {
                        data[0].PctAllocation = data[0].PctAllocation;
                        if (new Date(odata[0].AllocationEndDate) < preconEndDate || new Date(odata[0].AllocationStartDate) > preconStartDate) {
                            //let percentage = CalculatePctAllocation([odata[0]], preconStartDate, preconEndDate);
                            //data[0].PctAllocation = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                            data[0].preconRefIds = data[0].preconRefIds == null || data[0].preconRefIds == '' ? data[0].ID : data[0].preconRefIds;
                        }

                        data[0].PctAllocationConst = data[1].PctAllocation;
                        if (new Date(odata[1].AllocationEndDate) < constEndDate || new Date(odata[1].AllocationStartDate) > constStartDate) {
                            //data[0].PctAllocationConst = CalculatePctAllocation([odata[1]], constStartDate, constEndDate);
                            data[0].constRefIds = data[1].constRefIds == null || data[1].constRefIds == '' ? data[1].ID : data[1].constRefIds; //data[1].ID;
                        }

                        data[0].PctAllocationCloseOut = data[2].PctAllocation;
                        if (new Date(odata[2].AllocationEndDate) < closeoutEndDate || new Date(odata[2].AllocationStartDate) > closeoutStartDate) {
                            //let percentage = CalculatePctAllocation([odata[2]], closeoutStartDate, closeoutEndDate);
                            //data[0].PctAllocationCloseOut = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                            data[0].closeOutRefIds = data[2].closeOutRefIds == null || data[2].closeOutRefIds == '' ? data[2].ID : data[2].closeOutRefIds; //data[2].ID;
                        }

                        data[0].PreconId = data[0].ID;
                        data[0].ConstId = data[1].ID;
                        data[0].CloseOutId = data[2].ID;
                        if (data[1].constRefIds != null)
                            data[0].constRefIds = data[1].constRefIds;
                        if (data[2].closeOutRefIds != null)
                            data[0].closeOutRefIds = data[2].closeOutRefIds;
                    }
                }
                else {
                    let startDate = new Date(data[0].AllocationStartDate);
                    let endDate = new Date(data[0].AllocationEndDate);
                    let constStartDate = ConstStartDate;
                    let constEndDate = ConstEndDate;

                    let preconStartDate = PreconStartDate;
                    let preconEndDate = PreconEndDate;

                    if (!isDateValid(preconStartDate) && startDate < constStartDate) {
                        preconStartDate = startDate;
                    }
                    if (!isDateValid(preconEndDate) && isDateValid(constStartDate)) {
                        preconEndDate = constStartDate.addDays(-1);
                    }
                    if (startDate >= constStartDate && endDate <= constEndDate) {

                        data[0].PctAllocationConst = data[0].PctAllocation;
                        //if(new Date(odata[0].AllocationEndDate) < constEndDate || new Date(odata[0].AllocationStartDate) > constStartDate)
                        //{
                        //    data[0].PctAllocationConst = CalculatePctAllocation([odata[0]], constStartDate, constEndDate);
                        //    data[0].constRefIds = data[0].constRefIds == null || data[0].constRefIds == '' ? data[0].ID : data[0].constRefIds; //data[0].ID;
                        //}

                        data[0].PctAllocation = 0;
                        data[0].PctAllocationCloseOut = 0;
                        data[0].PreconId = 0;
                        data[0].ConstId = data[0].ID;
                        data[0].CloseOutId = 0;
                    }
                    else if (constEndDate < startDate) {
                        data[0].PctAllocationCloseOut = data[0].PctAllocation;
                        //if(new Date(odata[0].AllocationEndDate) < closeoutEndDate || new Date(odata[0].AllocationStartDate) > closeoutStartDate)
                        //{
                        //    let percentage = CalculatePctAllocation([odata[0]], closeoutStartDate, closeoutEndDate);
                        //    data[0].PctAllocationCloseOut = percentage <= 0 ? data[0].PctAllocationCloseOut : percentage;
                        //    data[0].closeOutRefIds = data[0].closeOutRefIds == null || data[0].closeOutRefIds == '' ? data[0].ID : data[0].closeOutRefIds; //data[0].ID;
                        //}
                        data[0].PctAllocation = 0;
                        data[0].PctAllocationConst = 0;
                        data[0].PreconId = 0;
                        data[0].ConstId = 0;
                        data[0].CloseOutId = data[0].ID;
                    }
                    else {
                        //if(new Date(odata[0].AllocationEndDate) < preconEndDate || new Date(odata[0].AllocationStartDate) > preconStartDate)
                        //{
                        //    let percentage = CalculatePctAllocation([odata[0]], preconStartDate, preconEndDate);
                        //    data[0].PctAllocation = percentage <= 0 ? data[0].PctAllocation : percentage;
                        //    data[0].preconRefIds = data[0].preconRefIds == null || data[0].preconRefIds == '' ? data[0].ID : data[0].preconRefIds; //data[0].ID;
                        //}
                        data[0].PctAllocationConst = 0;
                        data[0].PctAllocationCloseOut = 0;
                        data[0].PreconId = data[0].ID;
                        data[0].ConstId = 0;
                        data[0].CloseOutId = 0;
                    }
                }
                if (compactTempData.length == 0) {
                    compactTempData.push(JSON.parse(JSON.stringify(data[0])));
                }
                else if (compactTempData.filter(x => x.AssignedTo == e.AssignedTo && x.Type == e.Type).length == 0) {
                    compactTempData.push(JSON.parse(JSON.stringify(data[0])));
                }
            }
        });

        compactTempData.forEach(function (data) {
            let ids = [];
            if (parseInt(data.PreconId) != 0) {
                ids.push(data.PreconId);
            }
            if (parseInt(data.ConstId) != 0) {
                ids.push(data.ConstId);
            }
            if (parseInt(data.CloseOutId) != 0) {
                ids.push(data.CloseOutId);
            }
            if (data.closeOutRefIds != undefined && data.closeOutRefIds != '') {
                let rids = String(data.closeOutRefIds).split(";");
                if (rids != null && rids.length > 0) {
                    rids.forEach(function (id) {
                        ids.push(id);
                    });
                }
            }
            if (data.preconRefIds != undefined && data.preconRefIds != '') {
                let rids = String(data.preconRefIds).split(";");
                if (rids != null && rids.length > 0) {
                    rids.forEach(function (id) {
                        ids.push(id);
                    });
                }
            }
            if (data.constRefIds != undefined && data.constRefIds != '') {
                let rids = String(data.constRefIds).split(";");
                if (rids != null && rids.length > 0) {
                    rids.forEach(function (id) {
                        ids.push(id);
                    });
                }
            }
            data.IdsForDelete = ids.join(";");
        });

        return compactTempData;
    }
    function CheckPhaseConstraints(checkDateInMultiPhase) {
        OverlappingAllocationInPhases = [];
        let tempData = JSON.parse(JSON.stringify(globaldata));
        $.each(tempData, function (index, e) {
            let preconStartDate = PreconStartDate;
            let preconEndDate = PreconEndDate;

            let constStartDate = ConstStartDate;
            let constEndDate = ConstEndDate;

            let closeoutStartDate = CloseoutStartDate;
            let closeoutEndDate = CloseoutEndDate;

            let startDate = new Date(e.AllocationStartDate);
            let endDate = new Date(e.AllocationEndDate);

            if (!isDateValid(startDate) || !isDateValid(endDate)) {
                return;
            }
            let overlapDates = {
                precon: false,
                const: false,
                closeout: false
            };
            if (checkDateInMultiPhase) {
                if (!isDateValid(preconStartDate) && startDate <= constStartDate) {
                    preconStartDate = startDate;
                }
            }
            else {
                if (!isDateValid(preconStartDate) && startDate < constStartDate) {
                    preconStartDate = startDate;
                }
            }
            if (!isDateValid(preconEndDate) && isDateValid(constStartDate)) {
                preconEndDate = constStartDate.addDays(-1);
            }
            if (checkDateInMultiPhase) {
                if (!isDateValid(closeoutEndDate) && endDate >= constEndDate) {
                    closeoutEndDate = endDate;
                }
            }
            else {
                if (!isDateValid(closeoutEndDate) && endDate > constEndDate) {
                    closeoutEndDate = endDate;
                }
            }
            if (!isDateValid(closeoutStartDate) && isDateValid(constEndDate)) {
                closeoutStartDate = constEndDate.addDays(1);
            }

            if (startDate <= preconEndDate && endDate >= preconStartDate) {
                overlapDates.precon = true;
            }
            if (startDate <= constEndDate && endDate >= constStartDate) {
                overlapDates.const = true;
            }
            if (startDate <= closeoutEndDate && endDate >= closeoutStartDate) {
                overlapDates.closeout = true;
            }
            if (checkDateInMultiPhase) {
                if (overlapDates.precon && overlapDates.const && overlapDates.closeout) {
                    isDateInMultiPhase = true;
                }
                else {
                    if (overlapDates.precon && overlapDates.const) {
                        isDateInMultiPhase = true;
                    }
                    if (overlapDates.const && overlapDates.closeout) {
                        isDateInMultiPhase = true;
                    }
                    if (overlapDates.precon && !overlapDates.const && !overlapDates.closeout) {
                        if (endDate > preconEndDate) {
                            isDateInMultiPhase = true;
                        }
                    }
                    if (!overlapDates.precon && overlapDates.const && !overlapDates.closeout) {
                        if (startDate < constStartDate || endDate > constEndDate) {
                            isDateInMultiPhase = true;
                        }
                    }
                    if (!overlapDates.precon && !overlapDates.const && overlapDates.closeout) {
                        if (startDate < closeoutStartDate) {
                            isDateInMultiPhase = true;
                        }
                    }
                }
            }
            if (overlapDates.precon && overlapDates.const && overlapDates.closeout) {
                OverlappingAllocationInPhases.push(e);
                let alloc1 = JSON.parse(JSON.stringify(e));
                let alloc2 = JSON.parse(JSON.stringify(e));
                let alloc3 = JSON.parse(JSON.stringify(e));
                globaldata = globaldata.filter(x => x.ID != e.ID);
                alloc1.AllocationEndDate = preconEndDate.toISOString();
                alloc2.AllocationStartDate = constStartDate.toISOString();
                alloc2.AllocationEndDate = constEndDate.toISOString();
                alloc3.AllocationStartDate = closeoutStartDate.toISOString();
                if (alloc1.PctAllocationConst != undefined && parseInt(alloc1.PctAllocationConst) > 0) {
                    alloc2.PctAllocation = alloc1.PctAllocationConst;
                    alloc3.PctAllocation = alloc1.PctAllocationCloseOut;
                }
                globaldata.push(alloc1);
                alloc2.ID = -Math.abs(globaldata.length + 1);
                globaldata.push(alloc2);
                alloc3.ID = -Math.abs(globaldata.length + 1);
                globaldata.push(alloc3);
            }
            else {
                if (overlapDates.precon && overlapDates.const) {
                    OverlappingAllocationInPhases.push(e);
                    let alloc1 = JSON.parse(JSON.stringify(e));
                    let alloc2 = JSON.parse(JSON.stringify(e));
                    globaldata = globaldata.filter(x => x.ID != e.ID);
                    if (alloc1.PctAllocationConst != undefined && parseInt(alloc1.PctAllocationConst) > 0) {
                        alloc2.PctAllocation = alloc1.PctAllocationConst;
                    }
                    alloc1.AllocationEndDate = preconEndDate.toISOString();;
                    alloc2.AllocationStartDate = constStartDate.toISOString();;
                    globaldata.push(alloc1);
                    alloc2.ID = -Math.abs(globaldata.length + 1);
                    globaldata.push(alloc2);
                }
                if (overlapDates.const && overlapDates.closeout) {
                    OverlappingAllocationInPhases.push(e);
                    let alloc1 = JSON.parse(JSON.stringify(e));
                    let alloc2 = JSON.parse(JSON.stringify(e));
                    globaldata = globaldata.filter(x => x.ID != e.ID);
                    if (alloc1.PctAllocationCloseOut != undefined && parseInt(alloc1.PctAllocationCloseOut) > 0) {
                        alloc2.PctAllocation = alloc1.PctAllocationCloseOut;
                    }

                    alloc1.AllocationStartDate = startDate < constStartDate
                        ? constStartDate.toISOString() : alloc1.AllocationStartDate;

                    alloc1.AllocationEndDate = constEndDate.toISOString();
                    alloc2.AllocationStartDate = closeoutStartDate.toISOString();
                    globaldata.push(alloc1);
                    alloc2.ID = -Math.abs(globaldata.length + 1);
                    globaldata.push(alloc2);
                }
                if (overlapDates.precon && !overlapDates.const && !overlapDates.closeout) {
                    if (endDate > preconEndDate) {
                        OverlappingAllocationInPhases.push(e);
                        let alloc1 = JSON.parse(JSON.stringify(e));
                        globaldata = globaldata.filter(x => x.ID != e.ID);
                        alloc1.AllocationEndDate = preconEndDate.toISOString();;
                        globaldata.push(alloc1);
                    }
                }
                if (!overlapDates.precon && overlapDates.const && !overlapDates.closeout) {
                    if (startDate < constStartDate || endDate > constEndDate) {
                        OverlappingAllocationInPhases.push(e);
                        let alloc1 = JSON.parse(JSON.stringify(e));
                        globaldata = globaldata.filter(x => x.ID != e.ID);
                        alloc1.AllocationStartDate = startDate < constStartDate
                            ? constStartDate.toISOString() : alloc1.AllocationStartDate;
                        alloc1.AllocationEndDate = endDate > constEndDate
                            ? constEndDate.toISOString() : alloc1.AllocationEndDate;
                        globaldata.push(alloc1);
                    }
                }
                if (!overlapDates.precon && !overlapDates.const && overlapDates.closeout) {
                    if (startDate < closeoutStartDate) {
                        OverlappingAllocationInPhases.push(e);
                        let alloc1 = JSON.parse(JSON.stringify(e));
                        globaldata = globaldata.filter(x => x.ID != e.ID);
                        alloc1.AllocationStartDate = closeoutStartDate.toISOString();;
                        globaldata.push(alloc1);
                    }
                }
            }
        });
    }
    function CalculatePctAllocation(dataRow, minDate, maxDate) {
        let totalPercentage = 0;
        dataRow = dataRow.filter(x => new Date(x.AllocationEndDate) >= PreconStartDate && new Date(x.AllocationStartDate) <= CloseoutEndDate);
        if (minDate < PreconStartDate) {
            minDate = PreconStartDate;
        }
        if (maxDate > CloseoutEndDate) {
            maxDate = CloseoutEndDate;
        }
        let milli_secs_total = maxDate.getTime() - minDate.getTime();

        // Convert the milli seconds to Days 
        var totalDays = (milli_secs_total / (1000 * 3600 * 24)) + 1;

        $.each(dataRow, function (index, e) {
            let allocStartdate = new Date(e.AllocationStartDate);
            let allocEnddate = new Date(e.AllocationEndDate);
            if (allocStartdate < minDate) {
                allocStartdate = minDate;
            }
            if (allocEnddate > maxDate) {
                allocEnddate = maxDate;
            }
            var milli_secs = allocEnddate.getTime() - allocStartdate.getTime();

            // Convert the milli seconds to Days 
            var daysDiff = (milli_secs / (1000 * 3600 * 24)) + 1;

            let pctAlloc = parseInt(e.PctAllocation);
            totalPercentage += pctAlloc * daysDiff;
        });

        return totalDays > 0 ? Math.ceil(totalPercentage / totalDays) : 0;
    }

    function isAllAllocationsSame(options) {
        let allSoftAllocationValues = []
        if (options.data.ID != null) {
            var idData = _.findWhere(globaldata, { ID: parseInt(options.data.ID) });
            if (idData)
                allSoftAllocationValues.push(idData.SoftAllocation);
        }
        if (options.data.ConstId != null) {
            var idData = _.findWhere(globaldata, { ID: parseInt(options.data.ConstId) });
            if (idData)
                allSoftAllocationValues.push(idData.SoftAllocation);
        }
        if (options.data.CloseOutId != null) {
            var idData = _.findWhere(globaldata, { ID: parseInt(options.data.CloseOutId) });
            if (idData)
                allSoftAllocationValues.push(idData.SoftAllocation);
        }
        if (options.data.preconRefIds != null) {
            let rids = String(options.data.preconRefIds).split(";");
            if (rids != null && rids.length > 0) {
                rids.forEach(function (id) {
                    var idData = _.findWhere(globaldata, { ID: parseInt(id) });
                    if (idData)
                        allSoftAllocationValues.push(idData.SoftAllocation);
                });
            }
        }
        if (options.data.constRefIds != null) {
            let rids = String(options.data.constRefIds).split(";");
            if (rids != null && rids.length > 0) {
                rids.forEach(function (id) {
                    var idData = _.findWhere(globaldata, { ID: parseInt(id) });
                    if (idData)
                        allSoftAllocationValues.push(idData.SoftAllocation);
                });
            }
        }
        if (options.data.closeOutRefIds != null) {
            let rids = String(options.data.closeOutRefIds).split(";");
            if (rids != null && rids.length > 0) {
                rids.forEach(function (id) {
                    var idData = _.findWhere(globaldata, { ID: parseInt(id) });
                    if (idData)
                        allSoftAllocationValues.push(idData.SoftAllocation);
                });
            }
        }

        const areAllSame = allSoftAllocationValues.every(value => value === allSoftAllocationValues[0]);

        return areAllSame;
    }

    function isAllNCOAllocationsSame(options) {
        let allNCOAllocationValues = []
        if (options.data.ID != null) {
            var idData = _.findWhere(globaldata, { ID: parseInt(options.data.ID) });
            if (idData)
                allNCOAllocationValues.push(idData.NonChargeable);
        }
        if (options.data.ConstId != null) {
            var idData = _.findWhere(globaldata, { ID: parseInt(options.data.ConstId) });
            if (idData)
                allNCOAllocationValues.push(idData.NonChargeable);
        }
        if (options.data.CloseOutId != null) {
            var idData = _.findWhere(globaldata, { ID: parseInt(options.data.CloseOutId) });
            if (idData)
                allNCOAllocationValues.push(idData.NonChargeable);
        }
        if (options.data.preconRefIds != null) {
            let rids = String(options.data.preconRefIds).split(";");
            if (rids != null && rids.length > 0) {
                rids.forEach(function (id) {
                    var idData = _.findWhere(globaldata, { ID: parseInt(id) });
                    if (idData)
                        allNCOAllocationValues.push(idData.NonChargeable);
                });
            }
        }
        if (options.data.constRefIds != null) {
            let rids = String(options.data.constRefIds).split(";");
            if (rids != null && rids.length > 0) {
                rids.forEach(function (id) {
                    var idData = _.findWhere(globaldata, { ID: parseInt(id) });
                    if (idData)
                        allNCOAllocationValues.push(idData.NonChargeable);
                });
            }
        }
        if (options.data.closeOutRefIds != null) {
            let rids = String(options.data.closeOutRefIds).split(";");
            if (rids != null && rids.length > 0) {
                rids.forEach(function (id) {
                    var idData = _.findWhere(globaldata, { ID: parseInt(id) });
                    if (idData)
                        allNCOAllocationValues.push(idData.NonChargeable);
                });
            }
        }

        const areAllSame = allNCOAllocationValues.every(value => value === allNCOAllocationValues[0]);

        return areAllSame;
    }

    function updateDatesInGrid(startDate, EndDate, color) {
        if (startDate == "Invalid Date") {
            startDate = '';
        }
        if (EndDate == "Invalid Date") {
            EndDate = '';
        }
        flag = true;
        if (strSelectedAllocationIDs) {
            var Ids = strSelectedAllocationIDs.split(",");
            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
            var rows = dataGrid.getVisibleRows();
            rows.forEach(function (item, index) {
                if (Ids.indexOf(item.data.ID.toString()) != -1) {
                    dataGrid.cellValue(index, "AllocationStartDate", startDate);
                    dataGrid.cellValue(index, "AllocationEndDate", EndDate);

                    var cellElementStartDate = dataGrid.getCellElement(index, "AllocationStartDate");
                    cellElementStartDate.css('color', color);
                    var cellElementEndDate = dataGrid.getCellElement(index, "AllocationEndDate");
                    cellElementEndDate.css('color', color);
                }
            });

            selectionObject.component.clearSelection();
            dataGrid.saveEditData();

        } else {
            $("#toastwarning").dxToast("show");
        }
    }
</script>
<div id="toast"></div>
<div id="InternalAllocationGridDialog"></div>
    <div id="content">
        <div id="gridview">
            <div class="d-flex-modified">
                <div class="templateHeader" style="display:flex;align-items: center;">
                    <div style="margin-right:10px;">
                        <div style="font-size: 13px;color: #959595;font-family: 'Roboto', sans-serif;">Template Name:</div>
                        <div id="templateNameTxt"></div>
                    </div>
                    <div>
                        <div id="createNewTemplateCHK" style="font-family:'Roboto', sans-serif;margin-top:15px;"></div>
                    </div>
                </div>
                <div style="display: flex; margin-top: 27px;" class="text-center allocdatesbtn">
                    <div class="col-md-4 px-1">
                        <div id="btnPrecondate" style="display:none;"></div>
                        <div class="schedule-label preconborderbox" style="color: #52BED9;">Precon Dates</div>
                        <p id="pPrecon"></p>
                    </div>
                    <div class="col-md-4 px-1">
                        <div id="btnConstructionDate" style="display:none;"></div>
                        <div class="schedule-label constborderbox" style="color: #005C9B;">Const. Dates</div>
                        <p id="pConst"></p>
                    </div>
                    <div class="col-md-4 px-1">
                        <div id="btnCloseoutDate" style="max-width: initial !important;display:none;"></div>
                        <div class="schedule-label closeoutborderbox" style="color: #351B82;">Closeout Dates</div>
                        <p id="pCloseout"></p>
                    </div>
                </div>
                <div id="btnAddNew" class="btnAddNew"></div>
            </div>
            <div class="row">
                <div id="divMainGrid">
                    <div class="row">
                        <div id="gridTemplateContainer" class="grid-template-container" style="width: 100%; float: left;display:none;">
                        </div>
                        <div id="compactGridTemplateContainer" class="grid-template-container" style="width: 100%; float: left;">
                        </div>
                    </div>
                    <div class="row">
                        <div class="divMarginTop divMarginBottom d-flex justify-content-between flex-wrap col-md-12 px-0">
                            <div class="pr-2">
                                <div id="btnDetailedSummary" style="padding: 2px 4px;" class="btnAddNew"></div>
                                
                            </div>
                            <div>
                                <div id="btnSaveAllocation" class="btnAddNew"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
