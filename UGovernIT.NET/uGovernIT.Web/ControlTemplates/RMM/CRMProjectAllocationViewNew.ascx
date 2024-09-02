<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CRMProjectAllocationViewNew.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMM.CRMProjectAllocationViewNew" %>
<%@ Register Src="~/ControlTemplates/RMM/SaveAllocationAsTemplate.ascx" TagPrefix="ugit" TagName="SaveAllocationAsTemplate" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    html {
        scroll-behavior: smooth;
    }
    /*#gridTemplateContainer .dx-overlay-content{
        display: none;
    }*/
    .dx-datagrid-revert-tooltip {
        display: none;
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
        padding-bottom: 15px;
    }

    .workExbar .dx-icon {
        width: 34px;
        height: 27px;
    }

    .dx-popup-content {
        padding: 20px;
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

    .tableTileView .dx-tile, .tileViewContainer .dx-tile {
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

    .tableTileView .dx-empty-message, #tileViewContainer .dx-empty-message {
        text-align: center;
        padding-top: 62px;
    }

    .tableTileView .capacityblock, #tileViewContainer .capacityblock {
        float: left;
        width: 74px;
        text-align: center;
        /*height: 20px;*/
        padding-top: 2px;
        padding-bottom: 2px;
    }

        .tableTileView .capacityblock:first-child, #tileViewContainer .capacityblock:first-child {
            border-right: 1px solid #c3c3c3;
        }

    .tableTileView .capacityblock-1, #tileViewContainer .capacityblock-1 {
        float: left;
        width: 148px;
        text-align: center;
        /*height: 20px;*/
        padding-top: 2px;
        padding-bottom: 2px;
    }

        .tableTileView .capacityblock-1:first-child, #tileViewContainer .capacityblock-1:first-child {
            border-right: 1px solid #c3c3c3;
        }

    .tableTileView .allocation-v0, #tileViewContainer .allocation-v0 {
        background: #ffffff;
        color: #000;
    }

    .tableTileView .allocation-v1, #tileViewContainer .allocation-v1 {
        background: #fcf7b5;
        color: #000;
    }

    .tableTileView .allocation-v2, #tileViewContainer .allocation-v2 {
        background: #f8ac4a;
        color: #000;
    }

    .tableTileView .allocation-r0, #tileViewContainer .allocation-r0 {
        /*background: #57A71D;*/
        background:#6BA538;
        color: #fff;
    }

    .tableTileView .allocation-r1, #tileViewContainer .allocation-r1 {
        /*background: #A9C23F;*/
        background: #6BA538;
        color: #fff;
    }

    .tableTileView .allocation-r2, #tileViewContainer .allocation-r2 {
        background: #FF3535;
        color: #ffffff;
    }

    /*    #tileViewContainer .allocation-r3 {
        background: #f8ac4a;
    }
*/
    .tableTileView .allocation-r3, #tileViewContainer .allocation-r3 {
        background: #FF3535;
        color: #ffffff;
    }

    .tableTileView .allocation-c0, #tileViewContainer .allocation-c0 {
        background: #ffffff;
        color: #000;
    }

    .tableTileView .allocation-c1, #tileViewContainer .allocation-c1 {
        background: #fcf7b5;
        color: #fff;
    }

    .tableTileView .allocation-c2, #tileViewContainer .allocation-c2 {
        background: #f8ac4a;
        color: #000;
    }

    .highlightedBox {
        background: #cbc500 !important;
        border: 2px solid #375268;
    }

    .tableTileView .allocation-block, #tileViewContainer .allocation-block {
        height: 59px;
        display: flex;
        justify-content: center;
        align-items: center;
        border-radius: 7px;
    }

        .tableTileView .allocation-block .timesheet, #tileViewContainer .allocation-block .timesheet {
            position: absolute;
            top: 4px;
            left: 86%;
            cursor: pointer;
        }

    .tableTileView .dx-tile, #tileViewContainer .dx-tile {
        border: none;
    }

    .tableTileView .capacitymain, #tileViewContainer .capacitymain {
        border-top: 1px solid #c3c3c3;
        overflow: hidden;
        border-bottom-left-radius: 5px;
        border-bottom-right-radius: 5px;
        margin-top: 3px;
    }

    /*.dx-popup-bottom .dx-button {
        background-color: #4FA1D6 !important;
        color: white !important;
    }*/
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

    .paddingBottom {
        padding-bottom: 10px;
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

    #compareResume .dx-icon, #openGanttView .dx-icon, #compareTags .dx-icon{
        width: 25px !important;
        height: 25px !important;
    }
    #openCommonProject .dx-icon {
        width: 32px !important;
        height: 25px !important;
    }
    #compareResume .dx-button-mode-contained.dx-state-hover, #openGanttView .dx-button-mode-contained.dx-state-hover, #compareTags .dx-button-mode-contained.dx-state-hover
    , #openCommonProject .dx-button-mode-contained.dx-state-hover{
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
        justify-content: space-between;
    }
    .popup-title {
        font-size: 18px;
        text-align: justify;
    }
    .dateText {
        font-size: 11px;
        text-align: center;
    }

    .redCellColorStyle a {
        font-weight: 500 !important;
    }

    .schedule-label {
        font-size: 15px;
        font-weight: 500;
        width: 170px;
        display: none;
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

    #gridTemplateContainer .dx-datebox-calendar .dx-texteditor-input, #compactGridTemplateContainer .dx-datebox-calendar .dx-texteditor-input {
        text-align: left !important;
    }

    #btnSaveAllocation.dx-button-mode-contained:not(.dx-state-hover):hover {
        opacity: 0.7;
        background-color: #4FA1D6 !important;
    }

    .dx-checkbox-text {
        margin-top: 2px;
    }

    .custom-icon .icon {
        font-size: 17px;
        color: #f05b41;
        margin-right: 2px;
    }

    .dx-field {
        margin-bottom: 50px;
    }

    .availibility-1 {
        width: 200px;
        margin-top: 9px;
    }

    .availibility-2 {
        text-align: center;
        width: 180px;
    }
    .conflict-circle {
        width: 30px;
        height: 30px;
        border-radius: 20px;
        position: absolute;
        right: -10px;
        bottom: -2px;
        color: black;
        padding-top: 7px;
        background-color: orange;
        font-weight: 600;
    }

    .date-box {
        width: 50%;
        border: 1px solid #ddd !important;
        padding: 3px;
    }

    .date-box-1 {
        border: 2px solid #ddd;
        padding: 1px;
        border-top: 0px;
    }

    .overflow {
        overflow-x: hidden !important;
    }

    #divProjectView .dx-scrollable-content {
        top: 11px !important;
    }

    a:visited {
        color: #4A6EE2;
        text-decoration: none;
    }

    .paneldiv .panelImgDiv1 {
        background: gray;
        width: 50px;
        height: 50px;
        margin-left: auto;
        margin-right: auto;
        border-radius: 50%;
        margin-bottom: 10px;
        overflow: hidden;
    }

    .paneldiv p {
        font-size: 12px;
    }

    #divProjectView .dx-tile {
        left: auto !important;
        margin-top: -21px;
    }

    #divProjectView .dx-scrollview-content.dx-tileview-wrapper .dx-item.dx-tile, #divLeftPanel .dx-list-item .detailblock .dx-scrollview-content.dx-tileview-wrapper .dx-item.dx-tile {
        position: relative;
        height: 110px !important;
        width: 200px !important;
        margin-right: 10px;
    }

    #divProjectView .dx-scrollview-content.dx-tileview-wrapper, #divLeftPanel .dx-list-item .detailblock .dx-scrollview-content.dx-tileview-wrapper {
        display: flex;
        height: auto;
        width: auto !important;
        justify-content: flex-start;
    }

    .outer-border-date-box {
        border: 2px solid #ddd;
        height: 69px;
    }
    #ConflictWeekDataGridDialog .dx-popup-content, #ConflictWeekDataGridDialogSummary .dx-popup-content {
        margin-top:-10px;
    }
    .dx-overlay-wrapper.dx-overlay-shader.dx-loadpanel-wrapper{
      z-index:200000001 !important
    }
</style>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .switch {
        position: relative;
        display: inline-block;
        width: 44px;
        height: 18px;
    }

        .switch input {
            opacity: 0;
            width: 0;
            height: 0;
        }

    .slider {
        position: absolute;
        cursor: pointer;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background-color: #ccc;
        -webkit-transition: .4s;
        transition: .4s;
    }

        .slider:before {
            position: absolute;
            content: "";
            height: 18px;
            width: 18px;
            left: 0px;
            bottom: 0px;
            background-color: #4fa1d6;
            -webkit-transition: .4s;
            transition: .4s;
        }

    input:checked + .slider {
        background-color: #ccc;
    }

    input:focus + .slider {
        box-shadow: 0 0 1px #ccc;
    }

    input:checked + .slider:before {
        -webkit-transform: translateX(26px);
        -ms-transform: translateX(26px);
        transform: translateX(26px);
    }

    /* Rounded sliders */
    .slider.round {
        border-radius: 34px;
    }

        .slider.round:before {
            border-radius: 34px;
        }


    .tag-container {
        display: flex;
        align-items: center;
        margin-bottom: 4px;
        justify-content: space-between;
    }

    .projectExperiencePopup .dx-tag-content-1, #projectExpTags .dx-tag-content-1 {
        position: relative;
        display: inline-block;
        text-align: center;
        border: 2px dotted gray !important;
        margin: 4px 0 0 4px;
        padding: 3px 21px 4px 9px !important;
        min-width: 40px;
        background-color: #fff !important;
        border-radius: 28px;
        color: #333 !important;
        font-weight: 500;
    }

    .projectExperiencePopup .dx-tag-content, #projectExpTags .dx-tag-content {
        position: relative;
        display: inline-block;
        text-align: center;
        border: 2px #ddd;
        border-radius: 28px;
        cursor: auto;
        margin: 4px 3px 4px 4px;
        padding: 5px 21px 6px 12px;
        min-width: 40px;
        background-color: #ededed;
        /* border-radius: 2px; */
        color: #333;
        font-weight: 500;
    }

    .projectExperiencePopup .dx-overlay-wrapper {
        font-family: 'Roboto', sans-serif !important;
        color: black !important;
        /* color: black; */
    }

    .projectExperiencePopup .font-size-class {
        font-size: 15px;
    }

    .boxProp {
        box-shadow: 0 6px 14px #ccc;
        padding: 15px;
        height: 100%;
        width: 100%;
        /* display: flex; */
        /* align-items: center; */
    }

    #projectExpTags.dx-texteditor.dx-editor-outlined {
        background: #fff;
        border: 0px solid #ddd;
        border-radius: 4px;
    }

    #addExpTags.dx-button-has-icon .dx-icon {
        width: 25px;
        height: 25px;
    }

    #clearExpTags.dx-button-has-icon .dx-icon {
        width: 40px;
        height: 25px;
    }

    .th-header {
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 90px;
    }

    .count-box {
        padding: 10px 0px;
        background: aquamarine;
        margin: 5px;
        font-weight: 500;
        font-size: 14px;
        width: 60px;
        text-align: center;
    }

    .tag-name {
        margin-top: 13px;
        font-weight: 500;
        font-size: 14px;
    }

    .userExperiencePopup {
        font-family: 'Roboto', sans-serif !important;
    }

    .tr-border {
        border-bottom: 2px solid gray;
    }

    .tag-container-1 {
        display: flex;
        align-items: center;
        margin-bottom: 10px;
        justify-content: center;
    }

    .availibility-4 {
        text-align: center;
        width: 134px;
    }

    .availibility-3 {
        width: 146px;
        margin-top: 9px;
    }

    #compareTags .dx-icon {
        filter: invert(1);
        border: 2px solid white;
    }

    .tag-container-2 {
        display: flex;
        justify-content: flex-start;
        align-items: center;
    }

    .dx-tag-1 {
        /*margin-top: -4px;*/
    }

    .dx-popup-content-popover {
        padding: 10px 15px !important;
    }

    .userImageStyle {
        width: 35px;
        border-radius: 50%;
    }

    .rounded-circle {
        background: #ededed;
        border-radius: 15px;
        padding: 5px 10px;
        margin-left: 2px;
        margin-right: 2px;
        text-align: center;
    }

    .rounded-circle-dotted {
        background: white;
        border: 1px dotted black;
        border-radius: 15px;
        padding: 5px 10px;
        margin-left: 2px;
        margin-right: 2px;
        text-align: center;
    }

    #form .dx-layout-manager .dx-label-h-align.dx-flex-layout {
        display: -webkit-box;
        display: -ms-flexbox;
        display: flex;
        flex-direction: column;
    }

    #form .dx-form-group-with-caption > .dx-form-group-content {
        padding-top: 0px !important;
        margin-top: 0px !important;
        border-top: 0px solid #ddd !important;
        padding-bottom: 15px !important;
    }

    #divMixedSwitch .dx-button-content {
        padding: 2px 5px 2px !important;
        background-color:#f3eded;
    }

    .dx-item.dx-toolbar-item.dx-toolbar-label{
        max-width:100% !important;
    }
    #ConflictWeekDataGridDialog .dx-popup-bottom.dx-toolbar, #ConflictWeekDataGridDialogSummary .dx-popup-bottom.dx-toolbar {
        padding-top: 0px !important;
    }

    #ConflictWeekDataGridDialog .dx-popup-title.dx-has-close-button, #ConflictWeekDataGridDialogSummary .dx-popup-title.dx-has-close-button {
        padding-top: 9px;
        padding-bottom: 9px;
        font-size: 16px;
        font-weight: 500;
        background-color: #f0f0f0;
    }
    #ConflictWeekDataGridDialog .close-btn, #ConflictWeekDataGridDialogSummary .close-btn {
        position: absolute;
        right: 15px;
        margin-top: -3px;
        cursor:pointer;
    }
    .color-style {
        color: #4A6EE2;
        text-decoration: underline;
        font-weight: 500;
        text-align: center;
    }
    .profileUserImg{
        height:35px;
        width:35px;
        border-radius:35px;
    }
    #commonUserProject .dx-datagrid-headers.dx-datagrid-nowrap {
        padding-right: 0px;
        background-color: #ddd;
        color: black;
        font-size: 13px;
        font-weight: 500;
    }
    #commonUserProject .dx-datagrid .dx-column-lines > td {
        border-left: 0px solid #ddd;
        border-right: 0px solid #ddd;
        cursor:pointer;
    }
    #commonUserProject .dx-row.dx-data-row.dx-column-lines {
        border-bottom: 2px solid #ddd;
    }
    #commonUserProject .exptags-style {
        text-align: center;
        width: 40px;
        border: 2px solid #ddd;
        border-radius: 5px;
        padding: 5px 0px;
        padding-top: 4px;
        font-size: 14px;
        font-weight: 500;
    }
    /*.dx-popup-bottom .dx-button {
        min-width: 100px;
        background-color: #789CCE;
        color: white;
    }*/
    .btnBlue {
        background-color: #789CCE !important;
        color: white;
    }
    .btnNormal {
        background-color: white !important;
        color: black;
    }
    .btnCancelAlert {
        background-color: #f65d50 !important;
        color: white;
    }
    .btnSaveAllocation {
        background-color: #789CCE !important;
        color: white;
        background-image: url(/content/Images/SaveWhite.png) !important;
        background-repeat: no-repeat !important;
        background-size: 15px !important;
        top: 10px;
        background-position: 4px 7px !important;
    }

    #btnSaveClose .dx-button-text {
        margin-left: 20px;     
    }

    .dx-button-text {
           font-family: 'Roboto', sans-serif !important;
           font-size:12px;
    }

    #updatePhaseDates .dx-checkbox-container {
    display: flex;
    flex-direction: row-reverse;
    font-weight:500;

  }
    #updatePhaseDates .dx-checkbox-icon {
    width: 22px;
    height: 22px;
    margin-top: 2px;
    border-radius: 2px;
    border: 1px solid #ddd;
    background-color: #fff;
    margin-left: 5px;
}
 /*   .dx-popup-bottom .dx-button {
    min-width: 100px;
    background-color: #789CCE;
    color: white;
}*/
     /*.dx-overlay-content.dx-popup-normal.dx-resizable.dx-popup-flex-height {
     transform:translate(476px, 3px) scale(1) !important;
 }*/
</style>
<script id="dxss_inlineCtrScriptResource" data-v="<%=UGITUtility.AssemblyVersion %>">
    var dataModel = {
        preconStartDate: "",
        preconEndDate: "",
        constStartDate: "",
        constEndDate: "",
        closeoutStartDate: "",
        closeoutEndDate: "",
        onHold: false,
        preconDuration: "",
        constDuration: "",
        closeOutDuration: "",
        closed: false
    };
    var tempdataModel = null;
    var isTicketClosed = "<%= isTicketClosed%>";
    var Model = {
        RecordId: "<%= Request["ticketId"]%>" == "" ? "<%= TicketID%>" : "<%= Request["ticketId"]%>",
        Fields: [{
            FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.PreconStartDate%>",
            Value: dataModel.preconStartDate
        }, {
                FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.PreconEndDate%>",
                Value: dataModel.preconEndDate
            }, {
                FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.EstimatedConstructionStart%>",
                Value: dataModel.constStartDate
            }, {
                FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.EstimatedConstructionEnd%>",
                Value: dataModel.constEndDate
            }, {
                FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.CloseoutDate%>",
                Value: dataModel.closeoutEndDate
            }, {
                FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.TicketOnHold%>",
                Value: dataModel.onHold
            }, {
                FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.Closed%>",
                Value: dataModel.closed
            }]
    };
    var pctValueChanged = false;
    var isRequestFromFindTeams = false;
    var flag = false;
    var strSelectedAllocationIDs = "";
    var selectionObject;
    var popupDetails = null;
    var lastEditedRow = "";
    var isAllocationSplitted = false;
    const priorities = ['Precon', 'Construction', 'CloseOut'];
    var EnforcePhaseConstraints = "<%=this.EnforcePhaseConstraints%>" == "True" ? true : false;
    var PhaseSummaryView = "<%=this.PhaseSummaryView%>" == "True" ? true : false;
    var ScheduleDateOverLap = "<%=this.ScheduleDateOverLap%>" == "True" ? true : false;
    var showLockedColumn = "<%=this.EnableLockUnlockAllocation%>" == "True" ? true : false;
    var ajaxHelperPage = "<%=ajaxHelperPage%>";
    var openSummaryView = "<%= Request["opensummaryview"]%>" == "true" ? true : false;
    var allowGroupFilterOnExpTags = "<%=this.AllowGroupFilterOnExpTags%>" == "True" ? true : false;
    var OverlappingAllocationInPhases = [];
    var UserConflictData = [];
    var isAlertActive = false;
    var selectBoxItems = [
        { text: "All Resources", value: 2 },
        { text: "Fully Available", value: 0 },
        { text: "Partially Available", value: 1 }
    ];
    var radioGroupItems = [
        { text: "Allocation", value: 1 },
        { text: "Availablity", value: 0 }
    ];

    var checkboxGroupItems = [

        { text: "1", value: "Complexity" },
        { text: "2", value: "Count" },
        { text: "3", value: "Voulme" }
    ];
    const allocationTemplateSettings = [
        //{ id: 1, name: 'Save As Template', icon: 'save' },
        {
            id: 2, name: 'Show Allocation Templates', icon: 'import'
        },
    ];

    var searchTeamCriteria = {
        CommonExperiences: false,
        WorkedTogether: false,
        CommonClient: false,
        CommonSector: false,
        SelectedTags: [],
        SelectedCertifications: "",
    };
    var selectedResources = [];
    var tempGloblaDataStorage = [];
    var tempRefIds = [];
    var popupFilters = {};
    var IsFirstRequest = false;
    var GroupsData = [];
    var UsersData = [];
    var projectID = "<%= Request["ticketId"]%>" == "" ? "<%= TicketID%>" : "<%= Request["ticketId"]%>";
    var globaldata = [];
    var compactTempData = [];
    var validateOverlap = false;
    var JobTitleData = [];
    var UserProfileData = [];
    var showTimeSheet = false;
    var deletedData = [];
    var currentDate = new Date();
    var isGridInValidState = true;
    var ResourceTagCount = [];
    var hasAccessToAddTags = "<%=HasAccessToAddTags%>" == "False" ? false : true;
    var tagType = "";
    var projectExperiencModel = {};
    projectExperiencModel.ProjectId = projectID;
    projectExperiencModel.ProjectTags = [];
    var experienceTags = [];
    var projectExperiencTags = [];
    var existingProjectTags = [];
    var cmicNumber = "<%=CMICNumber%>";
    var tempGlobaldata = [];
    var ticketUrl = "<%=TicketUrl%>";
    var showAllocationTemplate = '<%=!HideAllocationTemplate%>' == 'True' && ('<%=ModuleName%>' == 'OPM' || '<%=ModuleName%>' == 'CPR') ? true : false;
    var updateAllProjectAllocations = true;
    var useThreading = true;
    $.cookie("opensummaryview", false);
    function GetExperienceTagsData() {
        $.get("/api/rmmapi/GetExperiencedTagList?tagMultiLookup=All", function (data) {
            experienceTags = data;
            GetCertificationsData();
        });
    }
    function GetCertificationsData() {
        $.get("/api/rmmapi/GetCertificationsList", function (data) {
            certifications = data;
            GetProjectExperienceTagList()
        });
    }

    function GetProjectExperienceTagList() {
        $.get("/api/rmmapi/GetProjectExperienceTagList?projectId=" + projectExperiencModel.ProjectId, function (data) {
            if (data != "EmptyProjectTags" && data != "null" && data != null) {
                existingProjectTags = data.filter(x => x.Type == 2);
                projectExperiencModel.ProjectTags = JSON.parse(JSON.stringify(existingProjectTags));
                GenerateData();
            }
        });
    }

    GetExperienceTagsData();

    function GenerateData() {
        projectExperiencTags = [];
        existingProjectTags.forEach(function (value, index) {
            let tag = {};
            if (CheckTagExist(value.Type, value.TagId)) {
                let expTag = value.Type == 2 ? experienceTags.filter(x => x.ID == value.TagId)[0] : certifications.filter(x => x.ID == value.TagId)[0];
                tag.TagId = value.TagId;
                tag.ID = String(value.Type) + String(value.TagId);
                tag.Type = value.Type;
                tag.Title = expTag.Title;
                tag.MinValue = parseInt(value.MinValue) > 1 ? 1 : value.MinValue;
                tag.IsMandatory = value.IsMandatory;
                projectExperiencTags.push(tag);
            }
        });
        if ($("#projectExpTags").dxTagBox("instance") != undefined) {
            $("#projectExpTags").dxTagBox("instance").option("value", []);
            $("#projectExpTags").dxTagBox("instance").option("dataSource", projectExperiencTags);
            $("#projectExpTags").dxTagBox("instance").option("value", projectExperiencTags.map(x => x.ID));
            globaldata.forEach(function (value, index) {
                value.Tags = projectExperiencTags.filter(x => x.Type == 2).map(x => x.TagId).join();
            });
            //projectExperiencModel.ProjectTags = existingProjectTags != null && existingProjectTags.length > 0 ? JSON.parse(JSON.stringify(existingProjectTags)) : [];
        }
    }
    function RenderProjectTags() {
        $(".experience-tags-row").html("");
        if (projectExperiencModel.ProjectTags != null && projectExperiencModel.ProjectTags.length > 0) {
            projectExperiencModel.ProjectTags.forEach(function (value, index) {
                if (CheckTagExist(value.Type, value.TagId)) {
                    let experiencTag = value.Type == 2 ? experienceTags.filter(x => x.ID == value.TagId)[0] : certifications.filter(x => x.ID == value.TagId)[0];
                    let cssClass = value.IsMandatory ? "dx-tag-content" : "dx-tag-content-1";
                    let title = value.MinValue > 0 ? experiencTag.Title + " &ge; " + value.MinValue : experiencTag.Title
                    let element = $(`<div class="dx-tag"><div class="${cssClass}"><span>${title}</span><div onclick="RemoveProjectTags(${value.TagId}, ${value.Type})" class="dx-tag-remove-button"></div></div></div>`);
                    $(".experience-tags-row").append(element);
                }
            });
        }
    }
    function RenderProjectTagsOnFrame(bindData) {
        if (bindData == undefined)
            bindData = true;
        $("#projectExpTags").html("");
        if (popupFilters.SelectedTags != null && popupFilters.SelectedTags.length > 0) {
            popupFilters.SelectedTags.forEach(function (value, index) {
                if (CheckTagExist(value.Type, value.TagId)) {
                    let experiencTag = value.Type == 2 ? experienceTags.filter(x => x.ID == parseInt(value.TagId))[0] : certifications.filter(x => x.ID == parseInt(value.TagId))[0];
                    let cssClass = value.MinValue > 0 ? "dx-tag-content" : "dx-tag-content-1";
                    let title = value.MinValue > 1 ? experiencTag.Title + " > " + String(parseInt(value.MinValue) - 1) : experiencTag.Title;
                    let style = value.MinValue > 0 ? 'style="cursor:pointer;"' : "";

                    let element = allowGroupFilterOnExpTags
                        ? $(`<div class="dx-tag-1"><div class="${cssClass}"><span id=tag${value.TagId}${value.Type} ${style} onclick="OpenProjectTag(${value.TagId}, ${value.Type})">${title}</span><div onclick="DeleteProjectTag(${value.TagId}, ${value.Type})" style="cursor:pointer;" class="dx-tag-remove-button"></div></div></div>`)
                        : $(`<div class="dx-tag-1"><div class="${cssClass}"><span id=tag${value.TagId}${value.Type}>${title}</span></div></div>`);
                    $("#projectExpTags").append(element);
                }
            });
        }
        if (isRequestFromFindTeams) {
            searchTeamCriteria.SelectedTags = popupFilters.SelectedTags;
            startSearch();
        }
        else {
            if (bindData)
                bindDatapopup(popupFilters);
        }
    }

    function DeleteProjectTag(tagId, type) {
        popupFilters.SelectedTags = popupFilters.SelectedTags.removeByValue(popupFilters.SelectedTags.filter(x => x.TagId == tagId && x.Type == type)[0]);
        RenderProjectTagsOnFrame();
    }
    function OpenProjectTag(tagId, type) {
        let expTag = popupFilters.SelectedTags.filter(x => x.TagId == tagId && x.Type == type)[0];
        if (expTag.MinValue != 0) {
            const popupUsersExperienceTags = function () {
                let container = $("<div class='divPopover'>");
                container.append(
                    $("<Label>Tag Name:</Label>"),
                    $("<div id='tagName'>").dxTextBox({
                        placeholder: "Min # of Project Experience",
                        value: expTag.Title,
                        disabled: true,
                    }),
                    $("<Label class='mt-1'>Min # of Project Experience:</Label>"),
                    $("<div id='tagMinValue'>").dxNumberBox({
                        placeholder: "Min # of Project Experience",
                        value: expTag.MinValue,
                        width: 200,
                        min: 1,
                        max: 1000,
                        hint: "Min # of Project Experience",
                        inputAttr: { 'aria-label': 'Min And Max' },
                    }),
                    $(`<div />`).append(
                        $(`<div id="addBtn" class="mt-2 mb-2 btnAddNew" style="float:right;padding: 0px 10px;font-size: 14px;" />`).dxButton({
                            text: "Apply",
                            width: "100%",
                            hint: 'Add Experience Tags',
                            onClick: function (e) {
                                let minValue = $("#tagMinValue").dxNumberBox("instance").option("value") != "" ? parseInt($("#tagMinValue").dxNumberBox("instance").option("value")) : 1;
                                expTag.MinValue = parseInt(minValue);
                                RenderProjectTagsOnFrame();
                                popup.hide();
                            }
                        })
                    ),
                )
                return container;
            };

            const popup = $("#usersExperienceTagsPopover").dxPopup({
                contentTemplate: popupUsersExperienceTags,
                width: "auto",
                height: "auto",
                showTitle: false,
                title: "",
                visible: false,
                dragEnabled: true,
                hideOnOutsideClick: true,
                showCloseButton: true,
                position: {
                    at: 'center',
                    my: 'center',
                    offset: '0 110',
                },
                onHiding: function () {

                },
                onContentReady: function () {
                    $(".divPopover").parent().addClass("dx-popup-content-popover");
                }
            }).dxPopup('instance');

            popup.option({
                contentTemplate: () => popupUsersExperienceTags(),
                'position.of': `#tag${tagId}${type}`,
            });
            popup.show();
        }
    }

    function RemoveProjectTags(tagId, type) {
        if (projectExperiencModel.ProjectTags != null && projectExperiencModel.ProjectTags.length > 0) {
            projectExperiencModel.ProjectTags = projectExperiencModel.ProjectTags.removeByValue(projectExperiencModel.ProjectTags.filter(x => x.TagId == tagId && x.Type == type)[0]);
            RenderProjectTags();
            let doneBtn = $("#doneBtn").dxButton("instance");
            doneBtn.option("visible", true);
        }
    }
    Array.prototype.removeByValue = function (val) {
        for (var i = 0; i < this.length; i++) {
            if (this[i].TagId === val.TagId && this[i].Type === val.Type) {
                this.splice(i, 1);
                i--;
            }
        }
        return this;
    }

    function CheckTagExist(tagType, tagId) {
        if (tagType == 2) {
            return experienceTags.filter(x => x.ID == tagId).length > 0 ? true : false;
        }
        if (tagType == 1) {
            return certifications.filter(x => x.ID == tagId).length > 0 ? true : false;
        }
    }
    function showTooltip(element, assignedTo) {
        var tooltip = $("#tooltip").dxTooltip("instance");
        let userTags = ResourceTagCount.filter(x => x.UserId == assignedTo);
        if (userTags != null && userTags.length > 0) {
            tooltip.option({
                target: "#" + element,
                contentTemplate: function (contentElement) {
                    userTags.forEach(function (value) {
                        contentElement.append(
                            $("<div />").addClass("date-info-tooltip").append(
                                $("<span style='font-weight:600;' />").text(value.Title + "- " + value.TagCount)
                            )
                        );
                    })
                }
            });
            tooltip.show();
        }
    }
    function hideTooltip(element) {
        var tooltip = $("#tooltip").dxTooltip("instance");
        tooltip.hide();
    }

    function OpenExperienceTagPopup() {
        let checkedUser = [];
        $(".innerCheckbox input").each(function () {
            if ($(this).is(":checked")) {
                checkedUser.push($(this).attr("id"));
            }
        });

        if (checkedUser.length <= 1) {
            DevExpress.ui.dialog.alert("Select at least two user.", "Warning!");
            return false;
        }
        let tags = popupFilters.SelectedTags.filter(x => x.IsMandatory);
        if (tags == null || tags.length == 0) {
            DevExpress.ui.dialog.alert("Please select at least one tag from the tags filter or add a new tag.", "Warning!");
            return false;
        }
        const popupUsersExperienceTags = function () {
            let container = $("<div class='userExperiencePopup'>");
            let html = [];
            let resourcesTag = JSON.parse(JSON.stringify(ResourceTagCount.filter(x => checkedUser.includes(x.UserId))));
            let userIds = [...new Set(resourcesTag.map(x => x.UserId))];

            html.push("<table>");
            html.push(`<tr><th></th>`);
            html.push(`<th><div class="th-header mb-1">${popupFilters.isAllocationView ? "Allocated" : "Availability"}</div></th>`);
            tags.forEach(function (value, index) {
                let cssClass = value.MinValue > 0 ? "rounded-circle" : "rounded-circle-dotted";
                html.push(`<th><div class="th-header ${cssClass} mb-1">${value.MinValue > 1 ? value.Title + " > " + (value.MinValue - 1) : value.Title}</div ></th >`)
            });
            html.push("</tr>");

            userIds.forEach(function (value, index) {
                let color = "";
                let userData = resourcesTag.filter(x => x.UserId == value)[0];
                html.push("<tr class='tr-border'>");
                html.push(`<td style="display:flex;align-items:flex-start;justify-content:flex-start;"><img id="userDisplayImage" class="userImageStyle mt-2" alt="User Photo" src="${userData.UserPicture}">
                <div class="mt-2 ml-3 mr-3" style="text-align:center;width:70%"><div style="font-weight:500;">${userData.UserName}</div><div>${userData.RoleName}</div></div></td>`);
                color = parseInt(userData.Availability) > 71 ? "#81B622" : parseInt(userData.Availability) > 0 && parseInt(userData.Availability) <= 71 ? "#A4DE02" : "#D4E8C1";
                html.push(`<td><div class="th-header"><div class="count-box" style="background-color:${color}">${popupFilters.isAllocationView ? userData.Allocation : userData.Availability}%</div ></div ></td >`);
                tags.forEach(function (tag) {
                    let data = resourcesTag.filter(x => x.TagId == tag.TagId && x.Type == tag.Type && x.UserId == value)[0];
                    let maxCount = Math.max.apply(Math, resourcesTag.filter(x => x.TagId == tag.TagId && x.Type == tag.Type).map(x => x.TagCount));
                    let count = parseInt(data.TagCount);
                    if (data.Type == 1 || maxCount == count) {
                        color = "#81B622";
                    }
                    else if (count > (maxCount / 2)) {
                        color = "#A4DE02";
                    }
                    else {
                        color = "#D4E8C1";
                    }
                    html.push(`<td><div class="th-header"><div onclick="OpenExperienceTagProjects(${tag.TagId},'${value}');" class="count-box" style="background-color:${color}">${data.TagCount}</div></div></td>`);
                });
                html.push("</tr>");
            });
            html.push("</table>");
            container.append(html.join(""));
            return container;
        };

        const popup = $("#usersExperienceTagsDialog").dxPopup({
            contentTemplate: popupUsersExperienceTags,
            width: "auto",
            height: "auto",
            showTitle: true,
            title: "",
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
            contentTemplate: () => popupUsersExperienceTags()

        });
        popup.show();
    }

    function OpenExperienceTagProjects(tagId, userId) {
        <%--let url = '<%=experienceTagProjectsURL%>' + '&ExperienceTagLookup=' + tagId + '&ExperienceTagUser=' + userId + "&Module=CPR";
        window.parent.UgitOpenPopupDialog(url, '', 'Resume Comparison: ', '95', '95', "", false);--%>
    }
    function ChangeAvailability(elem, type) {
        var popup = $('#popupContainer').dxPopup('instance');
        if (popupFilters.isAllocationView == 1) {
            popupFilters.isAllocationView = 0;
            var popupTitle = "Available Resource";
            if (type != undefined || type == "")
                popupTitle = "Available " + type + "s";
            popup.option('title', popupTitle);
        }
        else {
            popupFilters.isAllocationView = 1;

            var popupTitle = "Allocated Resource";
            if (type != undefined || type == "")
                //popupTitle = "Allocated " + type + "s";
                popupTitle = "Allocate " + type;
            popup.option('title', popupTitle)
        }
        bindDatapopup(popupFilters);
    }

    function ChangeProjectExperience() {
        if (popupFilters.projectVolume) {
            popupFilters.projectVolume = false;
            popupFilters.projectCount = false;
            bindDatapopup(popupFilters);
        } else {
            popupFilters.projectVolume = true;
            popupFilters.projectCount = true;
            bindDatapopup(popupFilters);
        }
    }

    function GetProjectDates() {
        $.get("/api/rmone/GetProjectDates?TicketId=" + projectID, function (data, status) {
            dataModel.preconStartDate = data.PreconStart == '0001-01-01T00:00:00' ? '' : data.PreconStart;
            dataModel.preconEndDate = data.PreconEnd == '0001-01-01T00:00:00' ? '' : data.PreconEnd;
            dataModel.constStartDate = data.ConstStart == '0001-01-01T00:00:00' ? '' : data.ConstStart;
            dataModel.constEndDate = data.ConstEnd == '0001-01-01T00:00:00' ? '' : data.ConstEnd;
            dataModel.closeoutStartDate = data.CloseoutStart == '0001-01-01T00:00:00' ? '' : data.CloseoutStart;
            dataModel.closeoutEndDate = data.Closeout == '0001-01-01T00:00:00' ? '' : data.Closeout;
            dataModel.onHold = data.OnHold;
            dataModel.preconDuration = parseInt(data.PreconDuration) > 0 ? data.PreconDuration : "";
            dataModel.constDuration = parseInt(data.ConstDuration) > 0 ? data.ConstDuration : "";
            dataModel.closeOutDuration = parseInt(data.CloseOutDuration) > 0 ? data.CloseOutDuration : "";
            SetVisibilityPhaseDates();
            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
            dataGrid.option("dataSource", globaldata);
        });
    }
    GetProjectDates();
    function SetVisibilityPhaseDates() {
        if (dataModel.preconStartDate == '' || dataModel.preconEndDate == '') {
            $("#pPrecon").hide();
            $("#pPreconNoDate").show();
        }
        else {
            $("#pPreconNoDate").hide();
            $("#pPrecon").text(new Date(dataModel.preconStartDate).format("MMM d, yyyy") + " To " + new Date(dataModel.preconEndDate).format("MMM d, yyyy"));
            $("#pPrecon").show();
            $(".pPrecon").text(new Date(dataModel.preconStartDate).format("MMM d, yyyy") + " To " + new Date(dataModel.preconEndDate).format("MMM d, yyyy"));
            $(".pPrecon").show();
        }
        if (dataModel.constStartDate == '' || dataModel.constEndDate == '') {
            $("#pConst").hide();
            $("#pConstNoDate").show();
        }
        else {
            $("#pConstNoDate").hide();
            $("#pConst").text(new Date(dataModel.constStartDate).format("MMM d, yyyy") + " To " + new Date(dataModel.constEndDate).format("MMM d, yyyy"));
            $("#pConst").show();
            $(".pConst").text(new Date(dataModel.constStartDate).format("MMM d, yyyy") + " To " + new Date(dataModel.constEndDate).format("MMM d, yyyy"));
            $(".pConst").show();
        }
        if (dataModel.closeoutStartDate == '' || dataModel.closeoutEndDate == '') {
            $("#pCloseout").hide();
            $("#pCloseoutNoDate").show();
        }
        else {
            $("#pCloseoutNoDate").hide();
            $("#pCloseout").text(new Date(dataModel.closeoutStartDate).format("MMM d, yyyy") + " To " + new Date(dataModel.closeoutEndDate).format("MMM d, yyyy"));
            $("#pCloseout").show();
            $(".pCloseout").text(new Date(dataModel.closeoutStartDate).format("MMM d, yyyy") + " To " + new Date(dataModel.closeoutEndDate).format("MMM d, yyyy"));
            $(".pCloseout").show();
        }
    }
    var selectedResourcesToProcess = [];
    var currentProcessingResourceIndex = 0;
    var isOriginatedFromFindTeamsSaveProcess = false;

    function saveAllocationsPerRole() {
        var canSave = false;
        var multipleResourcesErrorFound = false;
        selectedResourcesToProcess = [];
        var roleTileContainers = $('.tileViewContainer');
        roleTileContainers.each((roleIndex, roleItem) => {
            var titleViewObj = $(roleItem).dxTileView('instance');
            if (titleViewObj) {
                var tiles = titleViewObj._$container.contents();
                var noOfResourcesSelected = 0;
                var uniqueResources = [];
                titleViewObj._dataSource._items.forEach((ele) => {
                    if (uniqueResources.filter(x => x.AssignedTo == ele.AssignedTo).length == 0)
                        uniqueResources.push(ele);
                });
                tiles.each((resourceIndex, resourceItem) => {
                    var resourceBox = $(resourceItem).find('div.allocation-block');
                    if (resourceBox && $(resourceBox).length > 0) {
                        if ($(resourceBox).hasClass('highlightedBox')) {
                            noOfResourcesSelected += 1;
                            selectedResourcesToProcess.push({
                                RoleIndex: roleIndex,
                                ResourceIndex: resourceIndex,
                                ID: $(resourceBox).attr('itemid'),
                                Data: titleViewObj._dataSource._items[resourceIndex],
                                UniqueResources: uniqueResources
                            });
                        }
                    }
                });
            }
            if (noOfResourcesSelected > 1) {
                multipleResourcesErrorFound = true;
                var conflictDialog = DevExpress.ui.dialog.custom({
                    title: "Error",
                    message: `Multiple Resources Selected for Roles. You can select one resource per role when saving team allocations.`,
                    buttons: [
                        { text: "Okay", onClick: function () { return "Ok" } },
                    ]
                });
                conflictDialog.show().done(function (dialogResult) { });
                return false;
            } else if (noOfResourcesSelected == 1) {
                canSave = true;
            }
        });

        if (multipleResourcesErrorFound)
            return;

        if (canSave) {
            console.log(selectedResourcesToProcess);
            isOriginatedFromFindTeamsSaveProcess = true;
            currentProcessingResourceIndex = 0;
            processResource(selectedResourcesToProcess[currentProcessingResourceIndex]);
            //selectedResources.forEach((selectedResourceItem, selectedResourceIndex) => {
                
            //});
            //$('#popupContainer').dxPopup('instance').hide();
        } else {
            var conflictDialog = DevExpress.ui.dialog.custom({
                title: "Error",
                message: `Please select at least one resource when saving team allocations.`,
                buttons: [
                    { text: "Okay", onClick: function () { return "Ok" } },
                ]
            });
            conflictDialog.show().done(function (dialogResult) { });
        }
    }

    function checkAndProcessResource() {
        if (isOriginatedFromFindTeamsSaveProcess) {
            currentProcessingResourceIndex = currentProcessingResourceIndex + 1;
            if (selectedResourcesToProcess.length > 1 && currentProcessingResourceIndex < selectedResourcesToProcess.length) {
                var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                if (globaldata != null && globaldata.length == 0 && dataGrid != null)
                    globaldata = dataGrid.option('dataSource');
                processResource(selectedResourcesToProcess[currentProcessingResourceIndex]);
            } else {
                var popupInstance = $('#popupContainer').dxPopup('instance');
                var title = popupInstance.option('title');
                if (title != undefined && title != null && title == "Find Teams")
                    $('#popupContainer').dxPopup('instance').hide();
                selectedResourcesToProcess = [];
                currentProcessingResourceIndex = 0;
                isOriginatedFromFindTeamsSaveProcess = false;
            }
        }
    }

    function processResource(selectedResourceItem) {
        var data = selectedResourceItem.Data;
        if (selectedResourceItem.UniqueResources.filter(x => x.WeekWiseAllocations?.every(y => y.IsAvailable))?.length > 0 && !data.WeekWiseAllocations?.every(x => x.IsAvailable)) {
            var conflictDialog = DevExpress.ui.dialog.custom({
                title: "Alert",
                message: `${data.AssignedToName.replaceAll("'", "`")} has conflict weeks for the current duration. Do you want to proceed?`,
                buttons: [
                    { text: "Yes", onClick: function () { return "Ok" }, elementAttr: { "class": "btnBlue" } },
                    { text: "No", onClick: function () { return "Cancel" }, elementAttr: { "class": "btnNormal" } }
                ]
            });
            conflictDialog.show().done(function (dialogResult) {
                if (dialogResult == "Ok") {
                    AllocateResourceToAllocation(data, selectedResourceItem.ID);
                }
                else if (dialogResult == "Cancel") {
                    OpenConflictWeekData(data.WeekWiseAllocations, data.AssignedToName.replaceAll("'", "`"), data.AssignedTo, data.UserImageUrl, selectedResourceItem.UniqueResources, selectedResourceItem.ID);
                    return false;
                }
                else
                    return false;
            });
        }
        else
            AllocateResourceToAllocation(data, selectedResourceItem.ID);
    }

    function startSearch() {
        $("#loadpanel").dxLoadPanel({
            message: "Please wait...",
            visible: true
        });
        console.log(searchTeamCriteria);
        $('#teamTableBody').empty();
        bindTeams();
    }

    function getTeamResourcesTable() {
        var content = $('<div id="divTeamTable" style="width: 100%;" />').append(
            $('<table id="teamTable" style="width: 100%;margin: 15px 0px 0px 0px;box-shadow: 0 6px 14px #ddd;border-radius: 6px;table-layout: fixed;display:block;height:' + (window.innerHeight - 280) + 'px;overflow:auto;" />').append(
                $('<thead/><tbody id="teamTableBody" style="display:grid"/>')
            ),
            $('<div id="saveAllocations" style="width: 100%;margin: 15px 0px 0px 0px;box-shadow: 0 6px 14px #ddd;border-radius: 6px;table-layout: fixed;display:block;height:40px;" />')
                .append(
                    $("<div style='border:none;-webkit-box-shadow: none;float:right;margin:6px 10px 5px 0px;' class='btnAddNew'>").dxButton({
                        hint: 'Save Allocations',
                        text: 'Save Allocations',
                        onClick() {
                            saveAllocationsPerRole();
                        },
                }))
        );
        return content;
    }

    function getTableRows(response) {
        let preconstartDate = new Date(dataModel.preconStartDate);
        let conststartDate = new Date(dataModel.constStartDate);
        let constEndDate = new Date(dataModel.constEndDate);
        let closeoutstartDate = new Date(dataModel.closeoutStartDate);

        response.forEach((item, index) => {
            $('#teamTableBody').append('<tr style="display: inline;border-bottom:1px solid lightgray;"><td style="width:12%; padding:30px 15px;display:inline-block;word-wrap: break-word;' +
                'text-align:left;vertical-align:top;"><b>' + item.RoleName + '</b></td>' +
                '<td style="width:14%;padding:30px 15px;display:inline-block;word-wrap:break-word;vertical-align:top;" id="allocationDates' + index + '"></td>' +
                '<td style="width: 74%;padding: 15px;display: inline-block;"><div class="tableTileView" id="tileViewContainer' + index + '_' + item.RoleID + '"></td></tr>');

            let classNameSt = getDateBackgroundColor(new Date(selectedResources[index].AllocationStartDate), preconstartDate, conststartDate, constEndDate, closeoutstartDate);
            let classNameEd = getDateBackgroundColor(new Date(selectedResources[index].AllocationEndDate), preconstartDate, conststartDate, constEndDate, closeoutstartDate);

            $('#allocationDates' + index).append(
                $(`<div class='paneldiv tooltipp'>`).append(
                    $(`<div class='d-flex'>`).append(
                        $(`<div class='date-box ${classNameSt}'>${new Date(selectedResources[index].AllocationStartDate).format('MMM d, yyyy')}</div>`),
                        $(`<div class='date-box ${classNameEd}'>${new Date(selectedResources[index].AllocationEndDate).format('MMM d, yyyy')}</div>`),
                    ),
                    $(`<div class='date-box-1'>${selectedResources[index].RequiredPctAllocation}%</div>`),
                    $(`<span class='tooltiptext' style="display:none !important"></span> `)
                )
            );
            $('#tileViewContainer' + index + '_' + item.RoleID).dxTileView({
                height: 175, //210
                width: "100%", //150
                showScrollbar: true,
                baseItemHeight: 65,
                baseItemWidth: 150,
                itemMargin: 15,
                direction: "vertical",
                elementAttr: { "class": "tileViewContainer" },
                noDataText: "No resources available",
                dataSource: item.Resources,
                itemTemplate: function (itemData, itemIndex, itemElement) {
                    console.log(itemData);
                    itemData.PctAllocation = Math.round(itemData.PctAllocation);
                    itemData.SoftPctAllocation = Math.round(itemData.SoftPctAllocation);
                    itemData.TotalPctAllocation = Math.round(itemData.TotalPctAllocation);
                    var html = new Array();
                    html.push("<label class='innerCheckbox'>");
                    html.push("<input type='checkbox' allocaionview='0' pctallocation='" + itemData.PctAllocation + "' id='" + itemData.AssignedTo + "' name='" + itemData.JobTitle + "' onclick=preventClick()>");
                    html.push("</label>");
                    html.push("<div class='UserDetails'>");
                    html.push("<div id='" + itemData.AssignedTo + "'>");
                    html.push("<div class='AssignedToName'>");
                    html.push(itemData.AssignedToName);
                    html.push("</div>");

                    html.push("<div>");
                    html.push("(" + (100 - Number(itemData.PctAllocation)) + "%)");
                    html.push("</div>");

                    if (itemData.WeekWiseAllocations?.length > 0 && itemData.WeekWiseAllocations?.filter(x => !x.IsAvailable).length > 0) {
                        html.push(`<div style='cursor:pointer;' onclick='callbackOpenConflictWeekData(${index},"${item.RoleID}", ${itemIndex}, "${item.ID}");' >`);
                        html.push("<div class='conflict-circle'>");
                        html.push(itemData.WeekWiseAllocations.filter(x => !x.IsAvailable).length);
                        html.push("</div>");
                        html.push("</div>");
                    }
                    html.push("</div>");
                    html.push("</div>");
                    itemElement.attr("class", "allocation-block allocation-r" + itemData.AllocationRange);
                    itemElement.attr("id", "allocation-block" + itemIndex);
                    itemElement.attr("itemid", item.ID);
                    itemElement.attr("onmouseover", "showTooltip('allocation-block" + itemIndex + "','" + itemData.AssignedTo + "')");
                    itemElement.attr("onmouseout", "hideTooltip()");
                    itemElement.append(html.join(""));

                },
                onItemClick: function (e) {
                    var resourceBox = $(e.itemElement[0]).find('div.allocation-block');
                    if (resourceBox && $(resourceBox).length > 0) {
                        $(resourceBox).toggleClass('highlightedBox')
                    }
                },
                onInitialized: function (e) {

                },
                onContentReady: function (e) {
                    $("#popuploader").dxLoadPanel({
                        message: "Loading...",
                        visible: false
                    });
                },
                noDataText: function (e) {
                    $('.dx-empty-message').html('No resource available');
                    $("#popuploader").dxLoadPanel({
                        message: "Loading...",
                        visible: false
                    });
                }
            });
        });
    }

    function bindTeams() {
        var data = {
            TicketID: '<%=TicketID%>',
            SearchTeamCriteria: searchTeamCriteria,
            SelectedResources: selectedResources
        };
        $.post("/api/rmmapi/FindTeams/", data)
            .then(function (response) {
                console.log(response);
                getTableRows(response);
                $("#loadpanel").dxLoadPanel({
                    message: "Please wait...",
                    visible: false
                });
            });
    };

    function clearSearchTeamCriteria() {
        searchTeamCriteria.CommonExperiences = false;
        searchTeamCriteria.WorkedTogether = false;
        searchTeamCriteria.CommonClient = false;
        searchTeamCriteria.CommonSector = false;
        isRequestFromFindTeams = false;
        popupFilters.SelectedTags = [];
        searchTeamCriteria.SelectedTags = [];
        searchTeamCriteria.SelectedCertifications = "";
        popupFilters.SelectedCertifications = "";
    };

    $(function () {
        var RowId = 0;
        var AssignedToName = "";
        var resultData = [];
        var typename;
        $.cookie("dataChanged", 0, { path: "/" });
        $.cookie("projTeamAllocSaved", 0, { path: "/" });

        $("#toast").dxToast({
            message: "Allocations Saved Successfully. ",
            type: "info",
            displayTime: 1000,
            position: "{ my: 'center', at: 'center', of: window }"
        });

        $("#toastwarning").dxToast({
            message: "Please select at least one record. ",
            type: "warning",
            displayTime: 1000,
            position: "{ my: 'center', at: 'center', of: window }"
        });
        $("#toastOverlap").dxToast({
            message: "",
            type: "info",
            displayTime: 600,
            position: "{ my: 'center', at: 'center', of: window }"
        });

        $("#toastBlankAllocation").dxToast({
            message: "Overlapping allocations are not permitted. Save unsuccessful.",
            type: "info",
            displayTime: 600,
            position: "{ my: 'center', at: 'center', of: window }"
        });

        $("#exeperiencetoast").dxToast({
            message: "Experience Tags Saved Successfully. ",
            type: "info",
            displayTime: 1000,
            position: "{ my: 'center', at: 'center', of: window }"
        });
        var tooltip = $("#tooltip").dxTooltip({
            target: $(".homeGrid"),
            contentTemplate: function (contentElement) {
                contentElement.append(

                )
            }
        });
       
        LoadGlobalDataObject();
        $("#btnFindTeam").dxButton({
            text: "Find Team",
            icon: "/Content/images/search-white.png",
            focusStateEnabled: false,
            onClick: function (s, e) {
                var selectedAllocationsCount = 0;
                selectedResources = [];
                var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                var rows = dataGrid.getVisibleRows();
                rows.forEach(function (item, index) {
                    console.log(item);
                    if (item.values[0]
                        && item.values[2] != ''
                        && item.values[3] != undefined
                        && typeof item.values[3] == 'object'
                        && item.values[4] != undefined
                        && typeof item.values[4] == 'object'
                        && !(item.values[3] < new Date() && item.values[4] < new Date())
                    ) {
                        console.log(item.key.AssignedToName + ' - ' + item.cells[0].value);
                        selectedAllocationsCount += 1;
                        var data = globaldata[index];
                        selectedResources.push({
                            ID: data.ID,
                            Index: index,
                            TypeName: data.TypeName,
                            UserID: item.key.AssignedTo,
                            RoleID: item.key.Type,
                            AllocationStartDate: new Date(dataGrid.getDataSource()._items[index].AllocationStartDate).toISOString(),
                            AllocationEndDate: new Date(dataGrid.getDataSource()._items[index].AllocationEndDate).toISOString(),
                            RequiredPctAllocation: parseFloat(item.key.PctAllocation)
                        });
                    }
                });
                if (selectedAllocationsCount < 2) {
                    DevExpress.ui.dialog.alert("Please select at least two allocations with Role, Start Date and End Date (should be in future).", "Alert!");
                    return false;
                }
                isRequestFromFindTeams = true;
                var popupTitle = 'Find Teams';
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
                            //$("<div id='popuploader' />").dxLoadPanel({
                            //    message: "Loading...",
                            //    visible: true
                            //}), 
                            $("<div class='shadow-effect' style='padding-bottom: 10px;' />").append(
                                $("<div id='filterChecks' class='clearfix pt-2' />").append(
                                    $("<Label class='clsSmartFilter pt-2 mt-1' >Suggested Filters:</Label>"),
                                    //$("<div id='chkCommonExperiences' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                    //    text: "Common Experiences",
                                    //    value: searchTeamCriteria.CommonExperiences,
                                    //    onValueChanged: function (e) {
                                    //        searchTeamCriteria.CommonExperiences = e.value;
                                    //        startSearch();
                                    //    },
                                    //}),
                                    //$("<div id='chkWorkedTogether' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                    //    /*text: "Project Type",*/
                                    //    text: "Worked Together",
                                    //    value: searchTeamCriteria.WorkedTogether,
                                    //    onValueChanged: function (e) {
                                    //        searchTeamCriteria.WorkedTogether = e.value;
                                    //    }
                                    //}),
                                    $("<div id='chkCommonClient' title='Customer' class='chkFilterCheck pl-3 pt-2' style='float:left;' />").dxCheckBox({
                                        text: "<%=Customer%>",
                                        visible: ("<%=Customer%>" != '' && "<%=Customer%>" != null) == true ? true : false,
                                        value: searchTeamCriteria.CommonClient,
                                        hint: 'Customer',
                                        onValueChanged: function (e) {
                                            searchTeamCriteria.CommonClient = e.value;
                                            startSearch();
                                        },
                                    }),
                                    $("<div id='chkCommonSector' class='chkFilterCheck pl-3 pt-2' style='float:left;' />").dxCheckBox({
                                        text: "<%=Sector%>",
                                        visible: ("<%=Sector%>" != '' && "<%=Sector%>" != null) == true ? true : false,
                                        hint: 'Sector',
                                        value: searchTeamCriteria.CommonSector,
                                        onValueChanged: function (e) {
                                            searchTeamCriteria.CommonSector = e.value;
                                            startSearch();
                                        },
                                    }),
                                    $("<div id='chkCertification' class='chkFilterCheck pl-3 pt-2' style='float:left;' />").dxCheckBox({
                                        text: "Certification",
                                        visible: true,
                                        hint: 'Certification',
                                        onValueChanged: function (e) {
                                            if (e.value) {
                                                e.component.option("text", "");
                                                $("#certificationTxt").dxTagBox("option", "visible", true);
                                            }
                                            else {
                                                e.component.option("text", "Certification");
                                                $("#certificationTxt").dxTagBox("option", "visible", false);
                                                $("#certificationTxt").dxTagBox("option", "value", []);
                                            }
                                        },
                                    }),
                                    $("<div id='certificationTxt' class='filterctrl-jobDepartment pt-2' style='margin-top:-6px;padding-left:5px;' />").dxTagBox({  //dxSelectBox
                                        showSelectionControls: true,
                                        placeholder: "Certification",
                                        valueExpr: "ID",
                                        grouped: true,
                                        visible: false,
                                        displayExpr: "Title",
                                        dataSource: new DevExpress.data.DataSource({
                                            store: certifications,
                                            key: 'ID',
                                            group: 'CategoryName',
                                        }),
                                        maxDisplayedTags: 1,
                                        searchEnabled: true,
                                        onValueChanged: function (selectedItems) {
                                            searchTeamCriteria.SelectedCertifications = selectedItems.value.join();
                                            startSearch();
                                        },
                                    }),
                                    //$("<div id='searchTeams' class='dx-widget dx-button dx-button-mode-contained dx-button-normal dx-button-has-text dx-button-has-icon btnAddNew' style='float:right;margin-right:15px' />").dxButton({
                                    //    text: "Search Teams",
                                    //    icon: "/Content/images/search-white.png",
                                    //    focusStateEnabled: false,
                                    //    onClick: function (s, e) {
                                    //        startSearch();
                                    //    }
                                    //}),
                                    $("<div id='openGanttView' style='float:right;margin-right:25px' />").append(
                                        $("<div style='border:none;-webkit-box-shadow: none'>").dxButton({
                                            icon: '/content/Images/ganttBlackNew.png',
                                            hint: 'Open Gantt View',
                                            onClick() {
                                                OpenUsersGanttView();
                                            },
                                        })
                                    ),
                                    $("<div id='compareResume'  style='float:right;margin-right:15px'/>").append(
                                        $("<div style='border:none;-webkit-box-shadow: none'>").dxButton({
                                            icon: '/content/Images/RMONE/compareresume.png',
                                            hint: 'Compare Resume',
                                            onClick() {
                                                OpenResumeCompare(null, true);
                                            },
                                        })
                                    ),
                                    $("<div id='openCommonProject'  style='float:right;margin-right:15px'/>").append(
                                        $("<div style='border:none;-webkit-box-shadow: none'>").dxButton({
                                            icon: '/content/Images/RMONE/user-black.png',
                                            hint: 'Find work together',
                                            onClick() {
                                                OpenUserCommonProject();
                                            },
                                        })
                                    ),
                                ),
                                $("<div class='clearfix pt-1' />").append(
                                    $("<Label class='clsSmartFilter pr-4' style='margin-top:10px;' >Tags:</Label>"),
                                    $('<div class="tag-container mr-3" style="border: 2px solid #ddd;min-height:45px;">').append(
                                        $('<div class="tag-container-2 mr-3">').append(
                                            $("<div id='addExpTags' style='border: none; -webkit-box-shadow: none' />").dxButton({
                                                icon: "/Content/Images/plus-blue-new.png",
                                                hint: "Reset Experience Tags",
                                                visible: allowGroupFilterOnExpTags,
                                                focusStateEnabled: false,
                                                onClick() {
                                                    searchTeamCriteria.SelectedTags = JSON.parse(JSON.stringify(projectExperiencTags));
                                                    popupFilters.SelectedTags = JSON.parse(JSON.stringify(projectExperiencTags));
                                                    RenderProjectTagsOnFrame();
                                                }
                                            }),
                                            $('<div id="projectExpTags" style="display:flex;flex-wrap:wrap;" />'),
                                        ),
                                        $("<div id='clearExpTags' style='border: none; -webkit-box-shadow: none' />").dxButton({
                                            icon: "/Content/Images/RMONE/clear-icon.png",
                                            hint: "Clear Experience Tags",
                                            visible: allowGroupFilterOnExpTags,
                                            onClick: function () {
                                                searchTeamCriteria.SelectedTags = [];
                                                popupFilters.SelectedTags = [];
                                                RenderProjectTagsOnFrame();
                                            }
                                        }),
                                    ),

                                ),

                            ),
                            getTeamResourcesTable()
                        )},
                        itemClick: function (e) {
                        },
                        onHiding: function (e) {
                            clearSearchTeamCriteria();
                    },
                    onContentReady: function (e) {
                        startSearch();
                    },
                    });
                var popupInstance = $('#popupContainer').dxPopup('instance');
                popupInstance.option('title', popupTitle);
            }
        });

        

        $("#btnAddNew").dxButton({
            text: "Add New",
            disabled: isTicketClosed == 'True' ? true : false,
            icon: "/content/Images/plus-blue-new.png",
            focusStateEnabled: false,
            onClick: function (s, e) {
                if ($("#gridTemplateContainer").is(":visible") || IsPhaseDatesAvailable()) {
                    var projectStartdate;
                    if (dataModel.preconStartDate == '' || $("#gridTemplateContainer").is(":visible"))
                        projectStartdate = undefined;
                    else
                        projectStartdate = dataModel.preconStartDate;

                    var projectEnddate;
                    if (dataModel.closeoutEndDate == '' || $("#gridTemplateContainer").is(":visible"))
                        projectEnddate = undefined;
                    else
                        projectEnddate = dataModel.closeoutEndDate;

                    var grid = $("#gridTemplateContainer").dxDataGrid("instance");
                    var sum = 0;
                    //debugger;
                    sum = globaldata.length + 1;
                    globaldata.push({ "ID": -Math.abs(sum), "AssignedTo": "", "AssignedToName": "", "AllocationStartDate": projectStartdate, "AllocationEndDate": projectEnddate, "PctAllocation": 100, "SoftAllocation": '<%= IsAllocationTypeHard_Soft%>' == 'False' ? false : true, "NonChargeable": 0, "Type": 'TYPE-' + sum, "TypeName": '', "Tags": existingProjectTags.filter(y => y.Type == 2).map(x => x.TagId).join() });
                    compactTempData.push({ "ID": -Math.abs(sum), "PreconId": -Math.abs(sum), "AssignedTo": "", "AssignedToName": "", "AllocationStartDate": projectStartdate, "AllocationEndDate": projectEnddate, "PctAllocation": 100, "PctAllocationCloseOut": 100, "PctAllocationConst": 100, "SoftAllocation": '<%= IsAllocationTypeHard_Soft%>' == 'False' ? false : true, "NonChargeable": 0, "Type": '', "TypeName": '', "Tags": existingProjectTags.filter(y => y.Type == 2).map(x => x.TagId).join() });
                    grid.option("dataSource", globaldata);

                    var compactgrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                    compactgrid.option("dataSource", compactTempData);
                    if (!$("#gridTemplateContainer").is(":visible")) {
                        CheckPhaseConstraints(false);
                        //alert("step12");
                        CompactPhaseConstraints();
                    }
                    $.cookie("dataChanged", 1, { path: "/" });
                    $('#btnCancelChanges').dxButton({ visible: true });
                    $.cookie("projTeamAllocSaved", 0, { path: "/" });
                }
                else {
                    const popup = $("#popup").dxPopup("instance");
                    popup.option("title", "Please Enter Valid Dates On Project.");
                    openDateAgent(Model.RecordId);
                }
            }
        });

        $("#btnSaveAllocation").dxButton({
            text: "Save Allocations",
            icon: "/content/Images/save-open-new-wind.png",
            disabled: isTicketClosed == 'True' ? true : false,
            focusStateEnabled: false,
            onClick: async function (e) {
                var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");

                dataGrid.saveEditData();
                if (!isGridInValidState) {
                    return false;
                }

                SaveAllocations();
                e.event.stopPropagation();

            }
        });

        $("#btnSaveAsTemplate").dxButton({
            icon: "import",
            text: "Allocation Templates",
            focusStateEnabled: false,
            onClick: function (e) {
                UgitOpenPopupDialog('<%=importAllocationTemplateUrl%>', '', 'Select Project Team Template', '660', '450', '0', escape("<%= Request.Url.AbsolutePath %>"), 'true');
            }
        });

        <%--$('#btnSaveAsTemplate').dxDropDownButton({
            dataSource: allocationTemplateSettings,
            text: 'Allocation Template',
            showArrowIcon: true,
            splitButton: false,
            useSelectMode: false,
            displayExpr: 'name',
            keyExpr: 'id',
            dropDownOptions: {
                width: 200,
            },
            //width:150,
            onItemClick(e) {
                let preconstartDate = new Date('<%=PreConStartDateString%>');
                let preconEndDate = new Date('<%=PreConEndDateString%>');

                let conststartDate = new Date('<%=ConstructionStartDateString%>');
                let constEndDate = new Date('<%=ConstructionEndDateString%>');

                let closeoutstartDate = new Date('<%=CloseOutStartDateString%>');
                let closeoutEndDate = new Date('<%=CloseOutEndDateString%>');


                if (preconstartDate == "Invalid Date" || preconstartDate == '' || preconEndDate == "Invalid Date" || preconEndDate == '') {
                    DevExpress.ui.dialog.alert("Please Enter Valid Dates On Project.", "Error");
                    return;
                }
                if (conststartDate == "Invalid Date" || conststartDate == '' || constEndDate == "Invalid Date" || constEndDate == '') {
                    DevExpress.ui.dialog.alert("Please Enter Valid Dates On Project.", "Error");
                    return;
                }
                if (closeoutstartDate == "Invalid Date" || closeoutstartDate == '' || closeoutEndDate == "Invalid Date" || closeoutEndDate == '') {
                    DevExpress.ui.dialog.alert("Please Enter Valid Dates On Project.", "Error");
                    return;
                }

                if (e.itemData.name == "Save As Template") {
                    st_resetCtrl();

                    if (compactTempData == [] || compactTempData.length == 0) {
                        DevExpress.ui.dialog.alert("No Allocation found to save Allocations as template.", "Error");
                        return;
                    }


                    pcSaveAsTemplate.Show();
                }

                if (e.itemData.name == "Show Allocation Templates") {
                    UgitOpenPopupDialog('<%=importAllocationTemplateUrl%>', '', 'Select Project Team Template', '660', '450', '0', escape("<%= Request.Url.AbsolutePath %>"), 'true');
                }
            },
        });--%>

        $("#btnShowGanttView").dxButton({
            icon: "/content/Images/ganttBlackNew.png",
            hint: "Gantt View",
            focusStateEnabled: false,
            onClick: function (e) {
                //old code to switch to devextreme gantt chart
                //$("#ganttview").show();
                //$("#gridview").hide();
                //LoadGanttChart(globaldata);

                //code to show new custom allocation timeline
                let selectedUsers = $.cookie("SelectedUsers");
                let userall = $.cookie("userall");

                let params = `userall=${userall}&TicketID='<%=TicketID%>'&selectedUsers=${selectedUsers}&IsPhaseSummaryView=${PhaseSummaryView}`;
                UgitOpenPopupDialog('<%=allocationGanttUrl %>', params, "Resource Allocation Timeline", "90", "655px")
            }
        });

        function updateDatesInGrid(startDate, EndDate, color, hidePreconPhase, hideConstPhase) {
            if (startDate == "Invalid Date") {
                startDate = '';
            }
            if (EndDate == "Invalid Date") {
                EndDate = '';
            }
            flag = true;
            if (strSelectedAllocationIDs) {
                if (isDateValid(startDate) && isDateValid(EndDate)) {
                    var Ids = strSelectedAllocationIDs.split(",");
                    var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                    var rows = dataGrid.getVisibleRows();
                    rows.forEach(function (item, index) {
                        if (Ids.indexOf(item.data.ID.toString()) != -1) {
                            if (item.data.AllocationStartDate == undefined && item.data.AssignedTo != '' && startDate != ''
                                && EndDate != '' && new Date(startDate) < new Date() && new Date(EndDate) >= new Date()) {
                                dataGrid.cellValue(index, "AllocationStartDate", new Date());
                            }
                            else {
                                dataGrid.cellValue(index, "AllocationStartDate", startDate);
                            }
                            dataGrid.cellValue(index, "AllocationEndDate", EndDate);
                            var cellElementStartDate = dataGrid.getCellElement(index, "AllocationStartDate");
                            cellElementStartDate.css('color', color);
                            var cellElementEndDate = dataGrid.getCellElement(index, "AllocationEndDate");
                            cellElementEndDate.css('color', color);
                        }
                    });

                    selectionObject.component.clearSelection();
                    dataGrid.saveEditData();
                }
                else {
                    openDateAgent(Model.RecordId, hidePreconPhase, hideConstPhase);
                }

            } else {
                $("#toastwarning").dxToast("show");
            }
        }

        $("#btnPrecondate").dxButton({
            text: "Select Precon Dates",
            icon: "/content/Images/RMONE/calender_activeWhite.png",
            focusStateEnabled: false,
            elementAttr: {
                class: 'Precon_Btn'
            },
            onClick: function (e) {
                var startDate = new Date(dataModel.preconStartDate);
                var EndDate = new Date(dataModel.preconEndDate);
                updateDatesInGrid(startDate, EndDate, 'black', false, true);
                $('#btnCancelChanges').dxButton({ visible: true });
            }
        });
        $("#btnClose").dxButton({
            text: "Close",
            icon: "/content/Images/close-blue.png",
            focusStateEnabled: false,
            visible: false,
            onClick: function (e) {
                var sourceURL = "<%= Request["source"] %>";
                sourceURL += "**refreshDataOnly";
                window.parent.CloseWindowCallback(1, sourceURL);
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
                var startDate = new Date(dataModel.constStartDate);
                var EndDate = new Date(dataModel.constEndDate);
                updateDatesInGrid(startDate, EndDate, '#fff', true, false);
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
                var startDate = dataModel.closeoutStartDate;
                var EndDate = dataModel.closeoutEndDate;
                updateDatesInGrid(startDate, EndDate, '#fff', true, false);
                $('#btnCancelChanges').dxButton({ visible: true });
            }
        });

        $("#btnShowMultiAllocation").dxButton({
            text: "New Allocation",
            icon: "/content/Images/plus-blue-new.png",
            focusStateEnabled: false,
            onClick: function (e) {
                var url = '<%= MultiAddUrl%>';
                window.parent.UgitOpenPopupDialog(url, '', '', '900px', '95', 0, '');
            }
        });
        $("#btnFixOverlaps").dxButton({
            text: "Split Phases",
            focusStateEnabled: false,
            visible: !ScheduleDateOverLap,
            onClick: function (e) {
                if (ScheduleDateOverLap) {
                    DevExpress.ui.dialog.alert("Allocations cannot be automatically fixed since phases precon and construction phase have overlapping schedules", "Error");
                }
                else {
                    if (IsPhaseDatesAvailable()) {
                        CheckPhaseConstraints(true);
                        OpenSplitAllocationConfirmationDialog();
                    }
                    else
                    {
                        const popup = $("#popup").dxPopup("instance");
                        popup.option("title", "Please Enter Valid Dates On Project.");
                        openDateAgent(Model.RecordId);
                    }
                }
            }
        });
        $("#btnDetailedSummary").dxButton({
            text: "Summary View",
            focusStateEnabled: false,
            visible: !ScheduleDateOverLap,
            onClick: function (e) {
                if (ScheduleDateOverLap) {
                    DevExpress.ui.dialog.alert("Not allowed to switch to summary view since phases precon and construction phase have overlapping schedules", "Error");
                    return;
                }
                if (IsPhaseDatesAvailable()) {

                    //debugger;
                    //Popup message changes added for summary view
                    if (isAlertActive == false) {
                        CheckPhaseConstraints(false);
                        CompactPhaseConstraints();
                    }
                    //alert("step13");

                    if ($("#gridTemplateContainer").is(":visible")) {
                        $("#gridTemplateContainer").hide();
                        $("#compactGridTemplateContainer").show();
                        $("#btnDetailedSummary span").text("Detailed View");
                        $("#btnPrecondate, #btnConstructionDate, #btnCloseoutDate").hide();
                        $(".schedule-label").show();
                        $("#btnSaveAsTemplate").hide();
                        $("#btnFindTeam").hide();
                        if (showAllocationTemplate)
                            $("#btnSaveAsTemplate").show();
                    }
                    else {
                        showDetailViewGrid();
                    }
                }
                else
                {
                    const popup = $("#popup").dxPopup("instance");
                    popup.option("title", "Please Enter Valid Dates On Project.");
                    openDateAgent(Model.RecordId);
                }
            }
        });
        $("#btnShowDataView").dxButton({
            hint: "Data View",
            icon: "/content/Images/dataView.png",
            focusStateEnabled: false,
            onClick: function (e) {
                $("#ganttview").hide();
                $("#gridview").show();
            }
        });
        $("#btnUpdateSchedule").dxButton({
            text: "Reschedule",
            icon: "/content/Images/rmone/calender_activewhite.png",
            onClick: function (e) {

                const popup = $("#UpdateDatesPopup").dxPopup({
                    contentTemplate: popupContentTemplate,
                    width: "30%",
                    height: 280,
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
            visible: false,
            onClick: function (e) {
                var result = DevExpress.ui.dialog.custom({
                    title: "Unsaved Changes",
                    message: "Do you want to go back to previous allocations?",
                    buttons: [
                        { text: "Go back to Previous", onClick: function () { return true }, elementAttr: { "class": "btnBlue" } },
                        { text: "Stay & Continue", onClick: function () { return false }, elementAttr: { "class": "btnNormal" } }
                    ]
                });
                result.show().done(function (dialogResult) {
                    //debugger;
                    if (dialogResult) {
                        var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                        /*dataGrid.cancelEditData();*/
                        location.reload();
                    }
                });
                
            }
        });

        if ('<%=ModuleName%>' == "OPM" || '<%=ModuleName%>' == "CPR" || '<%=ModuleName%>' == "CNS" || '<%=ModuleName%>' == "NPR" || '<%=ModuleName%>' == "PMM") {
            $("#btnShowProfile").dxButton({
                //[+][SANKET][12/10/2023][Commented by Sanket]
                /*text: "Build Resume",*/
                icon: "/content/Images/RMONE/compareresume.png",
                focusStateEnabled: false,
                onClick: function (e) {
                    window.parent.UgitOpenPopupDialog('<%=buildProfileUrl%>', '', 'Build Profile', '1200', '700', '0', escape("<%= Request.Url.AbsolutePath %>"), 'true');
                }
            });

        }
        //debugger;
        $("#gridTemplateContainer").dxDataGrid({
            //columnHidingEnabled: true,
            dataSource: globaldata,
            ID: "grdTemplate",
            editing: {
                mode: "cell",
                allowEditing:  isTicketClosed == 'True' ? false : true,
                allowUpdating: isTicketClosed == 'True' ? false : true
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
                    dataField: "AssignedToName",
                    dataType: "text",
                    caption: "Assigned To",
                    //allowEditing: false,
                    sortIndex: "0",
                    sortOrder: "asc",
                    cellTemplate: function (container, options) {
                        //debugger;
                        $('.dx-header-row').addClass('devExtDataGrid-headerRow');
                        $('.dx-data-row').addClass('devExtDataGrid-DataRow');
                        if (options.key.ID > 0) {
                            var str = options.data.AssignedTo + "','" + options.data.AssignedToName.replace("'", "`");
                            var strwithspace = str.replace(/ /g, "&nbsp;")
                            $("<div id='dataId'>")
                                .append("<span style='float: left;overflow: auto;'>" + (options.data.UserImageUrl != null && options.data.UserImageUrl != "" ? "<img src=" + options.data.UserImageUrl + " class='profileUserImg' />" + '  ' : '') + "<a href='javascript:void(0);' onclick=openResourceTimeSheet('" + strwithspace + "');>"
                                    + (options.data.IsResourceDisabled ? "<span style='color:red;'>" + options.value + "</span>" : options.value) + "</a></span>")
                                .append($("<img>", { "src": "/content/images/moreoptions_blue.png", "ID": options.data.ID, "group": options.data.Type, "startDate": options.data.AllocationStartDate, "endDate": options.data.AllocationEndDate, "assignedTo": options.data.AssignedToName, "style": "float: right;overflow: auto;cursor: pointer;", "class": "assigneeToImg" }))
                                .appendTo(container);
                        }

                        if (options.key.ID <= 0) {
                            var str = options.data.AssignedTo + "','" + options.data.AssignedToName.replace("'", "`");
                            var strwithspace = str.replace(" ", "&nbsp;")
                            $("<div id='dataId'>")
                                .append("<span style='float: left;overflow: auto;'>" + (options.data.UserImageUrl != null && options.data.UserImageUrl != "" ? "<img src=" + options.data.UserImageUrl + " class='profileUserImg' />" + '  ' : '') + "<a href='javascript:void(0);' onclick=openResourceTimeSheet('" + strwithspace + "');>"
                                    + (options.data.IsResourceDisabled ? "<span style='color:red;'>" + options.value + "</span>" : options.value) + "</a></span>")
                                .append($("<img>", { "src": "/content/images/moreoptions_blue.png", "ID": options.data.ID, "group": options.data.Type, "startDate": options.data.AllocationStartDate, "endDate": options.data.AllocationEndDate, "assignedTo": options.data.AssignedToName, "style": "float: right;overflow: auto;cursor: pointer;", "class": "assigneeToImg" }))
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
                    width: "10%",
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
                        //debugger;
                        if (parseInt(value) <= 0) {
                            let data = globaldata.filter(x => x.ID == currentRowData.ID)[0];
                            DeleteAllocation(data);
                            newData.PctAllocation = currentRowData.PctAllocation;
                        }
                        else {
                            var currentDate = new Date();
                            //Popup message changes added for detailed view
                            if (new Date(currentRowData.AllocationStartDate) < currentDate && new Date(currentRowData.AllocationEndDate) < currentDate) {
                                //var result = DevExpress.ui.dialog.confirm("Are you sure you want to change the allocation percentage in a past allocation? ", "Warning!");
                                var result = DevExpress.ui.dialog.custom({
                                    title: "Warning!",
                                    message: "Are you sure you want to change the allocation percentage in a past allocation? ",
                                    buttons: [
                                        { text: "Yes", onClick: function () { return true }, elementAttr: { "class": "btnBlue" } },
                                        { text: "No", onClick: function () { return false }, elementAttr: { "class": "btnNormal" } }
                                    ]
                                });
                                result.show().done(function (dialogResult) {
                                    if (dialogResult) {
                                        SplitAllocationOnPctChange(currentRowData.ID, value, undefined, true);
                                        $("#gridTemplateContainer").dxDataGrid("instance").option("dataSource", globaldata);
                                        //alert("step14");
                                        CompactPhaseConstraints();
                                    }
                                });
                            }
                            else {
                                SplitAllocationOnPctChange(currentRowData.ID, value);
                                $("#gridTemplateContainer").dxDataGrid("instance").option("dataSource", globaldata);
                                //alert("step15");
                                CompactPhaseConstraints();
                            }
                            
                        }
                    }
                },
                {
                    width: "5%",
                    visible: showLockedColumn,
                    cellTemplate: function (container, options) {

                        let imagesrc = "/content/images/RMONE/Unlocked-image.png";
                        if (String(options.data.IsLocked) == "true") {
                            imagesrc = "/content/images/RMONE/locked-image.png";
                        }
                        $("<div id='rowIsLocked' style='text-align:center;'>")
                            .append($("<img>", {
                                "src": imagesrc, "ID": options.data.ID, "IsLocked": options.data.IsLocked, "TemplateID": options.data.TemplateID,
                                "style": "overflow: auto;cursor: pointer;", "class": "imgLocked", "width": "23px"
                            }))
                            .appendTo(container);
                    }
                },
                {
                    width: "8%",
                    fieldName: "SoftAllocation",
                    name: 'SoftAllocation',
                    caption: "",
                    dataType: "text",
                    //showEditorAlways: false,  
                    cellTemplate: function (container, options) {

                        $("<div id='divSoftAllocation' style='padding-left:10px;padding-right:10px;' >").append(
                            $("<div id='divSwitch' />").dxSwitch({
                                switchedOffText: 'Hard',
                                switchedOnText: 'Soft',
                                width: 60,
                                value: options.data.SoftAllocation,
                                onValueChanged(data) {
                                    $.cookie("dataChanged", 1, { path: "/" });
                                    $('#btnCancelChanges').dxButton({ visible: true });
                                },
                            })
                        ).appendTo(container);
                    },

                },
                {
                    width: "4%",
                    fieldName: "NonChargeable",
                    caption: 'NCO',
                    dataType: 'text',
                    cellTemplate: function (container, options) {
                        $("<div id='divNonChargeable' style='padding-left:10px;padding-right:10px;' >").append(
                            $("<div id='divSwitchNonChargeable' />").dxCheckBox({
                                //switchedOffText: 'BillHr',
                                //switchedOnText: 'NCO',
                                width: 30,
                                value: options.data.NonChargeable,
                                onValueChanged(data) {                                    
                                    $.cookie("dataChanged", 1, { path: "/" });
                                    $('#btnCancelChanges').dxButton({ visible: true });
                                },
                            })
                        ).appendTo(container);
                    },
                },
                {
                    width: "5%",
                    <%--visible:<%=IsGroupAdmin.ToString().ToLower()%>,   // need this line if we want to make delete based on admin group--%>
                    cellTemplate: function (container, options) {
                        var preconStartdate = dataModel.preconStartDate;
                        var preconEnddate = dataModel.preconEndDate;
                        if (preconEnddate == '' || preconStartdate == '') {
                            $("<div id='rowDelete' style='text-align:center;'>")
                                .append($("<img>", {
                                    "src": "/content/images/deleteIcon-new.png", "ID": options.data.ID, "TemplateID": options.data.TemplateID,
                                    "UserID": options.data.AssignedTo, "UserName": options.data.AssignedToName, "TypeName": options.data.TypeName, "Tags": options.data.Tags,
                                    "StartDate": options.data.AllocationStartDate, "EndDate": options.data.AllocationEndDate,
                                    "style": "overflow: auto;cursor: pointer;", "class": "imgDeleteNew", "title": "Delete", "width": "23px"
                                }))
                                .appendTo(container);
                        } else {
                            $("<div id='rowDelete' style='text-align:center;'>")
                                //.append($("<img>", { "src": "/Content/images/PreconCal2.png", "Index": options.rowIndex, "style": "overflow: auto;cursor: pointer;height:20px;width:20px;", "class": "imgPreconDate", "title":"Set Precon Date" }))
                                .append($("<img>", {
                                    "src": "/content/images/deleteIcon-new.png", "ID": options.data.ID, "TemplateID": options.data.TemplateID,
                                    "UserID": options.data.AssignedTo, "UserName": options.data.AssignedToName, "TypeName": options.data.TypeName, "Tags": options.data.Tags,
                                    "StartDate": options.data.AllocationStartDate, "EndDate": options.data.AllocationEndDate,
                                    "style": "overflow: auto;cursor: pointer;", "class": "imgDeleteNew", "title": "Delete", "width": "23px"
                                }))
                                .appendTo(container);
                        }
                    }
                },
                <%--{
                    width: "5%",
                    cellTemplate: function (container, options) {
                        var preconStartdate = '<%= PreConStartDate%>';
                        var preconEnddate = '<%= PreConEndDate%>';
                        if (preconEnddate == 'Jan 1, 0001' || preconStartdate == 'Jan 1, 0001') {
                            $("<div id='rowDelete' style='text-align:center;'>")
                                .append($("<img>", {
                                    "src": "/content/images/delete-icon-new.png", "ID": options.data.ID, "TemplateID": options.data.TemplateID,
                                    "UserID": options.data.AssignedTo, "UserName": options.data.AssignedToName, "TypeName": options.data.TypeName,/* "Tags": options.data.Tags,*/
                                    "style": "overflow: auto;cursor: pointer;", "class": "imgDeleteNew", "title": "Delete", "width": "23px"
                                }))
                                .appendTo(container);
                        } else {
                            $("<div id='rowDelete' style='text-align:center;'>")
                                //.append($("<img>", { "src": "/Content/images/PreconCal2.png", "Index": options.rowIndex, "style": "overflow: auto;cursor: pointer;height:20px;width:20px;", "class": "imgPreconDate", "title":"Set Precon Date" }))
                                .append($("<img>", {
                                    "src": "/content/images/delete-icon-new.png", "ID": options.data.ID, "TemplateID": options.data.TemplateID,
                                    "UserID": options.data.AssignedTo, "UserName": options.data.AssignedToName, "TypeName": options.data.TypeName, /*"Tags": options.data.Tags,*/
                                    "style": "overflow: auto;cursor: pointer;", "class": "imgDeleteNew", "title": "Delete", "width": "23px"
                                }))
                                .appendTo(container);
                        }
                    },
                },--%>
                //{
                //    dataField: "Tags",
                //    dataType: "text",
                //    visible: false,
                //},

            ],
            showBorders: true,
            showRowLines: true,
            selection: {
                mode: ('<%=ModuleName%>' == "OPM" || '<%=ModuleName%>' == "CPR" || '<%=ModuleName%>' == "CNS") == true ? 'multiple' : 'none',
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
                UpdateWorkingDays(e);
                lastEditedRow = e.key.ID;
                openDateConfirmationDialog();
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

                clickUpdateSize();
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
                            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
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
                if (e.parentType === 'dataRow' && e.dataField === 'AssignedToName') {
                    var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                    let rType = dataGrid.getDataSource()._items[e.row.rowIndex]?.Type;
                    let uData = rType != '' && rType != null && !rType.startsWith("TYPE-") ? UsersData.filter(x => x.GlobalRoleId == rType && x.Enabled == true).sort((a, b) => (a.Name > b.Name) ? 1 : -1)
                        : UsersData.filter(x => x.Enabled == true).sort((a, b) => (a.Name > b.Name) ? 1 : -1);

                    e.editorElement.dxSelectBox({
                        dataSource: uData,
                        valueExpr: "Id",
                        displayExpr: "Name",
                        value: e.row.data.AssignedTo,
                        searchEnabled: true,
                        onValueChanged: function (ea) {                          
                            $.each(ea.component._dataSource._items, function (i, v) {
                                if (v.Id === ea.value) {
                                    let currentDate = new Date();
                                    //Popup message changes added for detailed view AssignToName ="" and Start date is past date and end date is future date
                                    if (e.row.data.AssignedToName == "" && Date(e.row.data.AllocationStartDate) < currentDate) {
                                        var result = DevExpress.ui.dialog.custom({
                                            title: "Warning!",
                                            message: "RM One will adjust the start date of this allocation to Current Date",
                                            buttons: [
                                                { text: "Retain " + new Date(e.row.data.AllocationStartDate).toLocaleDateString("en-US") + " as Start Date", onClick: function () { return false }, elementAttr: { "class": "btnBlue" } },
                                                { text: "Change Date to " + new Date(currentDate).toLocaleDateString("en-US"), onClick: function () { return true }, elementAttr: { "class": "btnBlue" } }
                                            ]
                                        });
                                        result.show().done(function (dialogResult) {
                                            //debugger;
                                            if (dialogResult) {
                                                $.cookie("dataChanged", 1, { path: "/" });
                                                $('#btnCancelChanges').dxButton({ visible: true });
                                                SetDatesOfSummaryView(e.row.data.ID);
                                                if (popupFilters.Allocations?.length > 0) {
                                                    let newIds = CheckAndUpdatePastAllocations();
                                                    e.setValue(v.Name);
                                                    let alloc = null;
                                                    if (newIds?.length > 0) {
                                                        alloc = globaldata.find(x => newIds.includes(String(x.ID)));
                                                    } else {
                                                        alloc = globaldata.find(x => x.ID == e.row.data.ID);
                                                    }

                                                    alloc.AssignedTo = v.Id;
                                                    alloc.AssignedToName = v.Name;
                                                    alloc.UserImageUrl = v.Picture;
                                                    alloc.AllocationStartDate = currentDate;
                                                    alloc.IsResourceDisabled = false;

                                                    let x = GroupsData.filter(x => x.Id == v.GlobalRoleId);
                                                    if (x != null && x.length > 0) {
                                                        alloc.Type = x[0].Id;
                                                        alloc.TypeName = x[0].Name;
                                                    }

                                                    if (isDateValid(alloc.AllocationStartDate)
                                                        && isDateValid(alloc.AllocationEndDate)) {
                                                        popupFilters.allocationStartDate = new Date(alloc.AllocationStartDate).toISOString();
                                                        popupFilters.allocationEndDate = new Date(alloc.AllocationEndDate).toISOString();
                                                        popupFilters.SelectedUserID = v.Id;
                                                        popupFilters.projectID = projectID;
                                                        popupFilters.ResourceAvailability = "AllResource";
                                                        $.get("/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters), function (data, status) {
                                                            let x = globaldata.filter(x => x.ID == e.row.data.ID)[0]
                                                            if (data.length > 0 && parseInt(data[0].AllocationRange) > 1) {
                                                                x.color = "red";
                                                            }
                                                            else {
                                                                x.color = "";
                                                            }
                                                            dataGrid.option("dataSource", globaldata);
                                                            //alert("step16");
                                                            CompactPhaseConstraints();
                                                        });
                                                    }

                                                    dataGrid.option("dataSource", globaldata);
                                                    //alert("step17");
                                                    CompactPhaseConstraints();
                                                }
                                            }
                                            else if (!dialogResult) {
                                                $.cookie("dataChanged", 1, { path: "/" });
                                                $('#btnCancelChanges').dxButton({ visible: true });
                                                SetDatesOfSummaryView(e.row.data.ID);
                                                if (popupFilters.Allocations?.length > 0) {
                                                    let newIds = CheckAndUpdatePastAllocations();
                                                    e.setValue(v.Name);
                                                    let alloc = null;
                                                    if (newIds?.length > 0) {
                                                        alloc = globaldata.find(x => newIds.includes(String(x.ID)));
                                                    } else {
                                                        alloc = globaldata.find(x => x.ID == e.row.data.ID);
                                                    }

                                                    alloc.AssignedTo = v.Id;
                                                    alloc.AssignedToName = v.Name;
                                                    alloc.UserImageUrl = v.Picture;
                                                    alloc.AllocationStartDate = new Date(e.row.data.AllocationStartDate);
                                                    alloc.IsResourceDisabled = false;
                                                    let x = GroupsData.filter(x => x.Id == v.GlobalRoleId);
                                                    if (x != null && x.length > 0) {
                                                        alloc.Type = x[0].Id;
                                                        alloc.TypeName = x[0].Name;
                                                    }

                                                    if (isDateValid(alloc.AllocationStartDate)
                                                        && isDateValid(alloc.AllocationEndDate)) {
                                                        popupFilters.allocationStartDate = new Date(alloc.AllocationStartDate).toISOString();
                                                        popupFilters.allocationEndDate = new Date(alloc.AllocationEndDate).toISOString();
                                                        popupFilters.SelectedUserID = v.Id;
                                                        popupFilters.projectID = projectID;
                                                        popupFilters.ResourceAvailability = "AllResource";
                                                        $.get("/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters), function (data, status) {
                                                            let x = globaldata.filter(x => x.ID == e.row.data.ID)[0]
                                                            if (data.length > 0 && parseInt(data[0].AllocationRange) > 1) {
                                                                x.color = "red";
                                                            }
                                                            else {
                                                                x.color = "";
                                                            }
                                                            dataGrid.option("dataSource", globaldata);
                                                            //alert("step18");
                                                            CompactPhaseConstraints();
                                                        });
                                                    }
                                                    dataGrid.option("dataSource", globaldata);
                                                    //alert("step19");
                                                    CompactPhaseConstraints();
                                                }
                                            }
                                        });
                                    }
                                    else if (e.row.data.AssignedToName != "" && new Date(e.row.data.AllocationStartDate) < currentDate && new Date(e.row.data.AllocationEndDate) < currentDate) {
                                        //Popup message changes added for detailed view AssignToName ="" and bot Start date and end date are in past
                                        var result = DevExpress.ui.dialog.custom({
                                            title: "Warning!",
                                            message: "Are you sure you want to allocate to a past allocation?",
                                            buttons: [
                                                { text: "Yes", onClick: function () { return true }, elementAttr: { "class": "btnBlue" } },
                                                { text: "No", onClick: function () { return false }, elementAttr: { "class": "btnNormal" } }
                                            ]
                                        });
                                        result.show().done(function (dialogResult) {
                                            //debugger;
                                            if (dialogResult) {
                                                //debugger;
                                                $.cookie("dataChanged", 1, { path: "/" });
                                                $('#btnCancelChanges').dxButton({ visible: true });
                                                let alloc = null;
                                                alloc = globaldata.find(x => x.ID == e.row.data.ID);
                                                alloc.AssignedTo = v.Id;
                                                alloc.AssignedToName = v.Name;
                                                alloc.UserImageUrl = v.Picture;
                                                alloc.IsResourceDisabled = false;
                                                let x = GroupsData.filter(x => x.Id == v.GlobalRoleId);
                                                if (x != null && x.length > 0) {
                                                    alloc.Type = x[0].Id;
                                                    alloc.TypeName = x[0].Name;
                                                }

                                                if (isDateValid(alloc.AllocationStartDate)
                                                    && isDateValid(alloc.AllocationEndDate)) {
                                                    popupFilters.allocationStartDate = new Date(alloc.AllocationStartDate).toISOString();
                                                    popupFilters.allocationEndDate = new Date(alloc.AllocationEndDate).toISOString();
                                                    popupFilters.SelectedUserID = v.Id;
                                                    popupFilters.projectID = projectID;
                                                    popupFilters.ResourceAvailability = "AllResource";
                                                    $.get("/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters), function (data, status) {
                                                        let x = globaldata.filter(x => x.ID == e.row.data.ID)[0]
                                                        if (data.length > 0 && parseInt(data[0].AllocationRange) > 1) {
                                                            x.color = "red";
                                                        }
                                                        else {
                                                            x.color = "";
                                                        }
                                                        dataGrid.option("dataSource", globaldata);
                                                        //alert("step20");
                                                        CompactPhaseConstraints();
                                                    });
                                                }
                                                $("#gridTemplateContainer").dxDataGrid("instance").option("dataSource", globaldata);
                                                //alert("step21");
                                                CompactPhaseConstraints();
                                            }
                                        });
                                    }
                                    else {
                                        $.cookie("dataChanged", 1, { path: "/" });
                                        $('#btnCancelChanges').dxButton({ visible: true });
                                        SetDatesOfSummaryView(e.row.data.ID);
                                        //debugger;
                                        if (popupFilters.Allocations?.length > 0) {
                                            let newIds = CheckAndUpdatePastAllocations();
                                            e.setValue(v.Name);
                                            let alloc = null;
                                            if (newIds?.length > 0) {
                                                alloc = globaldata.find(x => newIds.includes(String(x.ID)));
                                            } else {
                                                alloc = globaldata.find(x => x.ID == e.row.data.ID);
                                            }
                                            alloc.AssignedTo = v.Id;
                                            alloc.AssignedToName = v.Name;
                                            alloc.UserImageUrl = v.Picture;
                                            alloc.IsResourceDisabled = false;

                                            let x = GroupsData.filter(x => x.Id == v.GlobalRoleId);
                                            if (x != null && x.length > 0) {
                                                alloc.Type = x[0].Id;
                                                alloc.TypeName = x[0].Name;
                                            }

                                            if (isDateValid(alloc.AllocationStartDate)
                                                && isDateValid(alloc.AllocationEndDate)) {
                                                popupFilters.allocationStartDate = new Date(alloc.AllocationStartDate).toISOString();
                                                popupFilters.allocationEndDate = new Date(alloc.AllocationEndDate).toISOString();
                                                popupFilters.SelectedUserID = v.Id;
                                                popupFilters.projectID = projectID;
                                                popupFilters.ResourceAvailability = "AllResource";
                                                $.get("/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters), function (data, status) {
                                                    let x = globaldata.filter(x => x.ID == e.row.data.ID)[0]
                                                    if (data.length > 0 && parseInt(data[0].AllocationRange) > 1) {
                                                        x.color = "red";
                                                    }
                                                    else {
                                                        x.color = "";
                                                    }
                                                    dataGrid.option("dataSource", globaldata);
                                                    //alert("step22");
                                                    CompactPhaseConstraints();
                                                });
                                            }
                                            dataGrid.option("dataSource", globaldata);
                                            //alert("step23");
                                            CompactPhaseConstraints();
                                        }
                                        else if (isStartEndDateLesserThanCurrDate) {
                                            debugger;
                                            DevExpress.ui.dialog.alert(`The end date should be in the future.`, 'Error');
                                        }
                                    }
                                }
                            });
                        }
                    });
                    e.cancel = true;
                }

            },
            onRowValidating: function (e) {
                if (!flag) {
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
                        if (new Date(e.key.AllocationEndDate) < new Date(e.newData.AllocationStartDate)) {
                            e.isValid = false;
                            e.errorText = "StartDate should be less than EndDate";
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
                }
                else {
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
                        if (new Date(e.newData.AllocationEndDate) < new Date(e.newData.AllocationStartDate)) {
                            e.isValid = false;
                            e.errorText = "StartDate should be less than EndDate";
                            //e.key.AllocationStartDate = e.newData.AllocationEndDate;
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
                        if (new Date(e.newData.AllocationEndDate) < new Date(e.newData.AllocationStartDate)) {
                            e.isValid = false;
                            e.errorText = "StartDate should be less than EndDate";
                            //e.key.AllocationEndDate = e.newData.AllocationStartDate;
                        }
                    }
                    //openConfirmationDialog(e.newData.AllocationStartDate, e.newData.AllocationEndDate);
                }
                $.cookie("dataChanged", 1, { path: "/" });
                $('#btnCancelChanges').dxButton({ visible: true });
                $.cookie("projTeamAllocSaved", 0, { path: "/" });
                $('#btnCancelChanges').dxButton({ visible: true });
                isGridInValidState = e.isValid;
            },
            onCellPrepared: function (e) {
                if (e.rowType === 'data') {
                    var preconstartDate = new Date(dataModel.preconStartDate);
                    var preconEndDate = new Date(dataModel.preconEndDate);

                    var conststartDate = new Date(dataModel.constStartDate);
                    var constEndDate = new Date(dataModel.constEndDate);

                    var closeoutstartDate = new Date(dataModel.closeoutStartDate);
                    var closeoutEndDate = new Date(dataModel.closeoutEndDate);

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
                    if (e.column.dataField == 'AssignedToName') {
                        if (e.data.color == "red") {
                            e.cellElement.addClass('redCellColorStyle');
                        }
                    }
                }
            }
        });

        function isDateFormatValid(dateString) {
            const dateRegex = /^(0[1-9]|1[0-2])\/(0[1-9]|[12][0-9]|3[01])\/\d{2}$/;
            return dateRegex.test(dateString);
        }
        //debugger;
        $("#compactGridTemplateContainer").dxDataGrid({
            //columnHidingEnabled: true,
            dataSource: compactTempData,
            ID: "grdCompactTemplate",
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
                    dataField: "AssignedToName",
                    dataType: "text",
                    caption: "Assigned To",
                    sortIndex: "0",
                    sortOrder: "asc",
                    cellTemplate: function (container, options) {
                        //debugger;
                        $('.dx-header-row').addClass('devExtDataGrid-headerRow');
                        $('.dx-data-row').addClass('devExtDataGrid-DataRow');
                        if (options.key.ID > 0) {

                            var str = options.data.AssignedTo + "','" + options.data.AssignedToName.replace("'","`");
                            var strwithspace = str.replace(/ /g, "&nbsp;")
                            $("<div id='dataId'>")
                                .append("<span style='float: left;overflow: auto;'>" + (options.data.UserImageUrl != null && options.data.UserImageUrl != "" ? "<img src=" + options.data.UserImageUrl + " class='profileUserImg' />" + '  ' : '') + "<a href='javascript:void(0);' onclick=openResourceTimeSheet('" + strwithspace + "');>"
                                + (options.data.IsResourceDisabled ? "<span style='color:red;'>" + options.value + "</span>" : options.value) + "</a></span>")
                                .append($("<img>", { "src": "/content/images/moreoptions_blue.png", "ID": options.data.ID, "group": options.data.Type, "startDate": options.data.AllocationStartDate, "endDate": options.data.AllocationEndDate, "assignedTo": options.data.AssignedToName, "style": "float: right;overflow: auto;cursor: pointer;", "class": "assigneeToImg" }))
                                .appendTo(container);
                        }

                        if (options.key.ID <= 0) {
                            var str = options.data.AssignedTo + "','" + options.data.AssignedToName.replace("'","`");
                            var strwithspace = str.replace(" ", "&nbsp;")
                            $("<div id='dataId'>")
                                .append("<span style='float: left;overflow: auto;'>" + (options.data.UserImageUrl != null && options.data.UserImageUrl != "" ? "<img src=" + options.data.UserImageUrl + " class='profileUserImg' />" + '  ' : '') + "<a href='javascript:void(0);' onclick=openResourceTimeSheet('" + strwithspace + "');>"
                                + (options.data.IsResourceDisabled ? "<span style='color:red;'>" + options.value + "</span>" : options.value) + "</a></span>")
                                .append($("<img>", { "src": "/content/images/moreoptions_blue.png", "ID": options.data.ID, "group": options.data.Type, "startDate": options.data.AllocationStartDate, "endDate": options.data.AllocationEndDate, "assignedTo": options.data.AssignedToName, "style": "float: right;overflow: auto;cursor: pointer;", "class": "assigneeToImg" }))
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
                    width: "24%",
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
                    allowEditing: false,
                    alignment: 'center',
                    cssClass: "v-align",
                    validationRules: [{ type: "required", message: '', }],
                    format: 'MMM d, yyyy',
                    sortIndex: "2",
                    sortOrder: "asc",
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
                    allowEditing: false,
                    alignment: 'center',
                    cssClass: "v-align",
                    width: "10%",
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
                                .append("<span style='float: left;overflow: auto;text-decoration:underline;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.preconRefIds + "');>" + options.data.PctAllocation + "</a></span>")
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
                                .append("<span style='float: left;overflow: auto;text-decoration:underline;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.constRefIds + "');>" + options.data.PctAllocationConst + "</a></span>")
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
                                .append("<span style='float: left;overflow: auto;text-decoration:underline;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.closeOutRefIds + "');>" + options.data.PctAllocationCloseOut + "</a></span>")
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
                    dataType: "text",
                    //showEditorAlways: false,  
                    cellTemplate: function (container, options) {

                        $("<div id='divSoftAllocation' style='padding-left:10px;padding-right:10px;' >").append(
                            $("<div id='divSwitch' />").dxSwitch({
                                switchedOffText: 'Hard',
                                switchedOnText: 'Soft',
                                width: 60,
                                value: options.data.SoftAllocation,
                                onValueChanged(data) {
                                    $.cookie("dataChanged", 1, { path: "/" });
                                    $('#btnCancelChanges').dxButton({ visible: true });
                                },
                            })
                        ).appendTo(container);
                    },

                },
                {
                    width: "8%",
                    fieldName: "NonChargeable",
                    caption: 'NCO',
                    dataType: 'text',
                    cellTemplate: function (container, options) {
                        if (isAllNCOAllocationsSame(options)) {
                            $("<div id='divNonChargeable' style='padding-left:10px;padding-right:10px;' >").append(
                                $("<div id='divSwitchNonChargeable' />").dxCheckBox({
                                    width: 30,
                                    value: options.data.NonChargeable,
                                    onValueChanged(data) {
                                        $.cookie("dataChanged", 1, { path: "/" });
                                        $('#btnCancelChanges').dxButton({ visible: true });
                                    },
                                })
                            ).appendTo(container);
                        } else {
                            $("<div id='divNonChargeable' style='padding-left:10px;padding-right:10px;' >").append(
                                $("<div id='divMixedNCOSwitch' />").dxButton({
                                    text: 'Mixed',
                                    width: 60,
                                    stylingMode: 'contained',
                                    onClick: function (e) {
                                        $.cookie("dataChanged", 1, { path: "/" });
                                        $('#btnCancelChanges').dxButton({ visible: true });
                                        showDetailViewGrid();
                                    }
                                })
                            ).appendTo(container);
                        }
                    },
                },
                {
                    width: "5%",
                    <%--visible:<%=IsGroupAdmin.ToString().ToLower()%>,   // need this line if we want to make delete based on admin group--%>
                    cellTemplate: function (container, options) {
                        var preconStartdate = dataModel.preconStartDate;
                        var preconEnddate = dataModel.preconEndDate;
                        if (preconEnddate == '' || preconStartdate == '') {
                            $("<div id='rowDelete' style='text-align:center;'>")
                                .append($("<img>", {
                                    "src": "/content/images/deleteIcon-new.png", "ID": options.data.ID, "TemplateID": options.data.TemplateID,
                                    "UserID": options.data.AssignedTo, "UserName": options.data.AssignedToName, "TypeName": options.data.TypeName, "Tags": options.data.Tags,
                                    "StartDate": options.data.AllocationStartDate, "EndDate": options.data.AllocationEndDate,
                                    "style": "overflow: auto;cursor: pointer;", "class": "imgDeleteNew", "title": "Delete", "width": "23px"
                                }))
                                .appendTo(container);
                        } else {
                            $("<div id='rowDelete' style='text-align:center;'>")
                                .append($("<img>", {
                                    "src": "/content/images/deleteIcon-new.png", "ID": options.data.ID, "TemplateID": options.data.TemplateID,
                                    "UserID": options.data.AssignedTo, "UserName": options.data.AssignedToName, "TypeName": options.data.TypeName, "Tags": options.data.Tags,
                                    "StartDate": options.data.AllocationStartDate, "EndDate": options.data.AllocationEndDate,
                                    "style": "overflow: auto;cursor: pointer;", "class": "imgDeleteNew", "title": "Delete", "width": "23px"
                                }))
                                .appendTo(container);
                        }
                    }
                },
                //{
                //    dataField: "Tags",
                //    dataType: "text",
                //    visible: false,
                //},

            ],
            showBorders: true,
            showRowLines: true,
            onRowUpdating: function (e) {
                if (e.newData.AssignedToName == undefined) {
                    SaveToGlobalData(e);
                }
            },
            onCellClick: function (e) {
                if (e.column.fieldName == "SoftAllocation") {
                    if (isAllAllocationsSame(e)) {
                        compactTempData.forEach(function (part, index, theArray) {
                            if (part.ID == e.data.ID) {
                                part.SoftAllocation = !part.SoftAllocation;
                            }
                        });

                        var compactElement = _.findWhere(compactTempData, { ID: parseInt(e.data.ID) });
                        let uniqueIds = GetUniqueIds(compactElement);

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

                        var compactElement = _.findWhere(compactTempData, { ID: parseInt(e.data.ID) });
                        let uniqueIds = GetUniqueIds(compactElement);

                        globaldata.forEach(function (part, index, theArray) {
                            if (uniqueIds.includes(String(part.ID))) {
                                part.NonChargeable = !part.NonChargeable;
                            }
                        });
                    }
                }
            },
            onContentReady: function (e) {
                var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                if (dataGrid.getDataSource() !== null)
                    globaldata = dataGrid.getDataSource()._items;

                clickUpdateSize();
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
                if (e.parentType === 'dataRow' && e.dataField === 'AssignedToName') {
                    var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                    let rName = dataGrid.getDataSource()._items[e.row.rowIndex]?.TypeName;
                    let rType = rName != '' && rName != null ? GroupsData.filter(x => x.Name == rName)[0].Id : '';
                    let uData = rType != '' && rType != null ? UsersData.filter(x => x.GlobalRoleId == rType && x.Enabled == true).sort((a, b) => (a.Name > b.Name) ? 1 : -1)
                        : UsersData.filter(x => x.Enabled == true).sort((a, b) => (a.Name > b.Name) ? 1 : -1);
                    e.editorElement.dxSelectBox({
                        dataSource: uData,
                        valueExpr: "Id",
                        displayExpr: "Name",
                        value: e.row.data.AssignedTo,
                        searchEnabled: true,
                        onValueChanged: function (ea) {
                            $.each(ea.component._dataSource._items, function (i, v) {                                
                                //alert("test111");
                                let currentDate = new Date();

                                if (v.Id === ea.value) {
                                    let preCompactAlloc = compactTempData.find(x => x.ID == e.row.data.ID);
                                    let preIds = GetUniqueIds(preCompactAlloc);
                                    let preAllocCheck = globaldata.filter(x => preIds.includes(String(x.ID)));
                                    let preGlobalData = null;
                                    preAllocCheck.forEach(function (e) {
                                        if (new Date(e.AllocationStartDate) < new Date() && new Date(e.AllocationEndDate) < new Date()) {
                                            preGlobalData = preAllocCheck.filter(x => x.ID != e.ID);
                                        }
                                    });
                                    let MinDates = new Date(Math.min.apply(null, preGlobalData.map(x => new Date(x.AllocationStartDate))));
                                    if (MinDates > currentDate) {
                                        currentDate = MinDates;
                                    }

                                    //Popup message changes added for summary view AssignToName ="" and Start date is past date and end date is future date
                                    if (e.row.data.AssignedToName == "" && Date(e.row.data.AllocationStartDate) < currentDate ) {
                                        var result = DevExpress.ui.dialog.custom({
                                            title: "Warning!",
                                            message: "RM One will adjust the start date of this allocation to Current Date",
                                            buttons: [
                                                { text: "Retain " + new Date(e.row.data.AllocationStartDate).toLocaleDateString("en-US") + " as Start Date", onClick: function () { return false }, elementAttr: { "class": "btnBlue" } },
                                                { text: "Change Date to " + new Date(currentDate).toLocaleDateString("en-US"), onClick: function () { return true }, elementAttr: { "class": "btnBlue" } }
                                            ]
                                        });
                                        result.show().done(function (dialogResult) {
                                            let newIds = null;
                                            //debugger;
                                            e.setValue(v.Name);
                                            let alloc = null;
                                            let CompactAlloc = compactTempData.find(x => x.ID == e.row.data.ID);
                                            let ids = GetUniqueIds(CompactAlloc);
                                                
                                            alloc = globaldata.filter(x => ids.includes(String(x.ID)));
                                            let roleData = GroupsData.filter(x => x.Id == v.GlobalRoleId);
                                            isAlertActive = true;
                                            //}
                                            if (dialogResult) {
                                                $.cookie("dataChanged", 1, { path: "/" });
                                                $('#btnCancelChanges').dxButton({ visible: true });
                                                alloc.forEach(function (e) {
                                                    //debugger;
                                                    if (ids.includes(String(e.ID)) && new Date(e.AllocationStartDate) < new Date() && new Date(e.AllocationEndDate) >= new Date()) {
                                                        //debugger;
                                                        e.AssignedTo = v.Id;
                                                        e.AssignedToName = v.Name;
                                                        if (roleData != null && roleData.length > 0) {
                                                            e.Type = roleData[0].Id;
                                                            e.TypeName = roleData[0].Name;
                                                        }
                                                        e.UserImageUrl = v.Picture;
                                                        e.IsResourceDisabled = false;
                                                        e.AllocationStartDate = currentDate;
                                                    }
                                                    else if (ids.includes(String(e.ID)) && new Date(e.AllocationStartDate) >= new Date() && new Date(e.AllocationEndDate) >= new Date()) {
                                                        e.AssignedTo = v.Id;
                                                        e.AssignedToName = v.Name;
                                                        if (roleData != null && roleData.length > 0) {
                                                            e.Type = roleData[0].Id;
                                                            e.TypeName = roleData[0].Name;
                                                        }
                                                        e.UserImageUrl = v.Picture;
                                                        e.IsResourceDisabled = false;
                                                    }
                                                    else if (new Date(e.AllocationStartDate) < new Date() && new Date(e.AllocationEndDate) < new Date()) {
                                                        globaldata = globaldata.filter(x => x.ID != e.ID);
                                                    }
                                                });

                                                if (CompactAlloc != null) {
                                                    if (roleData != null && roleData.length > 0) {
                                                        CompactAlloc.Type = roleData[0].Id;
                                                        CompactAlloc.TypeName = roleData[0].Name;
                                                    }
                                                    CompactAlloc.AssignedTo = v.Id;
                                                    CompactAlloc.AssignedToName = v.Name;
                                                    CompactAlloc.UserImageUrl = v.Picture;
                                                    CompactAlloc.IsResourceDisabled = false;
                                                    CompactAlloc.AllocationStartDate = currentDate;

                                                    if (isDateValid(dataGrid.getDataSource()._items[e.row.rowIndex].AllocationStartDate)
                                                        && isDateValid(dataGrid.getDataSource()._items[e.row.rowIndex].AllocationEndDate)) {
                                                        popupFilters.allocationStartDate = new Date(dataGrid.getDataSource()._items[e.row.rowIndex].AllocationStartDate).toISOString();
                                                        popupFilters.allocationEndDate = new Date(dataGrid.getDataSource()._items[e.row.rowIndex].AllocationEndDate).toISOString();
                                                        popupFilters.SelectedUserID = v.Id;
                                                        popupFilters.projectID = projectID;
                                                        popupFilters.ResourceAvailability = "AllResource";
                                                        $.get("/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters), function (data, status) {
                                                            $.each(alloc, function (key, value) {
                                                                if (data.length > 0 && parseInt(data[0].AllocationRange) > 1) {
                                                                    value.color = "red";
                                                                }
                                                                else {
                                                                    value.color = "";
                                                                }
                                                            });
                                                            if (data.length > 0 && parseInt(data[0].AllocationRange) > 1) {
                                                                CompactAlloc.color = "red";
                                                            }
                                                            else {
                                                                CompactAlloc.color = "";
                                                            }
                                                            dataGrid.option("dataSource", compactTempData);
                                                            //CompactPhaseConstraints();
                                                        });
                                                    }

                                                    $("#gridTemplateContainer").dxDataGrid("instance").option('dataSource', globaldata);
                                                    //CompactPhaseConstraints();
                                                }


                                            }
                                            else if (!dialogResult) {
                                                $.cookie("dataChanged", 1, { path: "/" });
                                                $('#btnCancelChanges').dxButton({ visible: true });
                                                alloc.forEach(function (a) {
                                                    //debugger;
                                                    if (ids.includes(String(a.ID))) {
                                                        //debugger;
                                                        a.AssignedTo = v.Id;
                                                        a.AssignedToName = v.Name;
                                                        if (roleData != null && roleData.length > 0) {
                                                            a.Type = roleData[0].Id;
                                                            a.TypeName = roleData[0].Name;
                                                        }
                                                        a.UserImageUrl = v.Picture;
                                                        a.IsResourceDisabled = false;
                                                    }
                                                });

                                                if (CompactAlloc != null) {
                                                    if (roleData != null && roleData.length > 0) {
                                                        CompactAlloc.Type = roleData[0].Id;
                                                        CompactAlloc.TypeName = roleData[0].Name;
                                                    }
                                                    CompactAlloc.AssignedTo = v.Id;
                                                    CompactAlloc.AssignedToName = v.Name;
                                                    CompactAlloc.UserImageUrl = v.Picture;
                                                    CompactAlloc.IsResourceDisabled = false;
                                                    CompactAlloc.AllocationStartDate = new Date(e.row.data.AllocationStartDate);

                                                    if (isDateValid(dataGrid.getDataSource()._items[e.row.rowIndex].AllocationStartDate)
                                                        && isDateValid(dataGrid.getDataSource()._items[e.row.rowIndex].AllocationEndDate)) {
                                                        popupFilters.allocationStartDate = new Date(dataGrid.getDataSource()._items[e.row.rowIndex].AllocationStartDate).toISOString();
                                                        popupFilters.allocationEndDate = new Date(dataGrid.getDataSource()._items[e.row.rowIndex].AllocationEndDate).toISOString();
                                                        popupFilters.SelectedUserID = v.Id;
                                                        popupFilters.projectID = projectID;
                                                        popupFilters.ResourceAvailability = "AllResource";
                                                        $.get("/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters), function (data, status) {
                                                            $.each(alloc, function (key, value) {
                                                                if (data.length > 0 && parseInt(data[0].AllocationRange) > 1) {
                                                                    value.color = "red";
                                                                }
                                                                else {
                                                                    value.color = "";
                                                                }
                                                            });
                                                            if (data.length > 0 && parseInt(data[0].AllocationRange) > 1) {
                                                                CompactAlloc.color = "red";
                                                            }
                                                            else {
                                                                CompactAlloc.color = "";
                                                            }
                                                            dataGrid.option("dataSource", compactTempData);
                                                            //CompactPhaseConstraints();
                                                        });
                                                    }

                                                    $("#gridTemplateContainer").dxDataGrid("instance").option('dataSource', globaldata);
                                                    //CompactPhaseConstraints();
                                                }
                                            }
                                        });
                                    }
                                    else if (e.row.data.AssignedToName != "" && new Date(e.row.data.AllocationStartDate) < currentDate && new Date(e.row.data.AllocationEndDate) < currentDate) {
                                        //Popup message changes added for summary view AssignToName ="" and Start date and end date are in past
                                        var result = DevExpress.ui.dialog.custom({
                                            title: "Warning!",
                                            message: "Are you sure you want to allocate to a past allocation?",
                                            buttons: [
                                                { text: "Yes", onClick: function () { return true }, elementAttr: { "class": "btnBlue" } },
                                                { text: "No", onClick: function () { return false }, elementAttr: { "class": "btnNormal" } }
                                            ]
                                        });
                                        result.show().done(function (dialogResult) {
                                            if (dialogResult) {
                                                //debugger;
                                                $.cookie("dataChanged", 1, { path: "/" });
                                                $('#btnCancelChanges').dxButton({ visible: true });
                                                let alloc = null;
                                                let CompactAlloc = compactTempData.find(x => x.ID == e.row.data.ID);
                                                let ids = GetUniqueIds(CompactAlloc);
                                                alloc = globaldata.filter(x => ids.includes(String(x.ID)));
                                                let roleData = GroupsData.filter(x => x.Id == v.GlobalRoleId);
                                                //isAlertActive = true;

                                                alloc.forEach(function (a) {
                                                    //debugger;
                                                    if (ids.includes(String(a.ID)) && new Date(a.AllocationStartDate) < new Date() && new Date(a.AllocationEndDate) < new Date()) {
                                                        //debugger;
                                                        a.AssignedTo = v.Id;
                                                        a.AssignedToName = v.Name;
                                                        if (roleData != null && roleData.length > 0) {
                                                            a.Type = roleData[0].Id;
                                                            a.TypeName = roleData[0].Name;
                                                        }
                                                        a.UserImageUrl = v.Picture;
                                                        a.IsResourceDisabled = false;
                                                        a.AllocationStartDate = new Date(e.row.data.AllocationStartDate);
                                                    }
                                                });

                                                if (CompactAlloc != null) {
                                                    if (roleData != null && roleData.length > 0) {
                                                        CompactAlloc.Type = roleData[0].Id;
                                                        CompactAlloc.TypeName = roleData[0].Name;
                                                    }
                                                    CompactAlloc.AssignedTo = v.Id;
                                                    CompactAlloc.AssignedToName = v.Name;
                                                    CompactAlloc.UserImageUrl = v.Picture;
                                                    CompactAlloc.IsResourceDisabled = false;

                                                    if (isDateValid(dataGrid.getDataSource()._items[e.row.rowIndex].AllocationStartDate)
                                                        && isDateValid(dataGrid.getDataSource()._items[e.row.rowIndex].AllocationEndDate)) {
                                                        popupFilters.allocationStartDate = new Date(dataGrid.getDataSource()._items[e.row.rowIndex].AllocationStartDate).toISOString();
                                                        popupFilters.allocationEndDate = new Date(dataGrid.getDataSource()._items[e.row.rowIndex].AllocationEndDate).toISOString();
                                                        popupFilters.SelectedUserID = v.Id;
                                                        popupFilters.projectID = projectID;
                                                        popupFilters.ResourceAvailability = "AllResource";
                                                        $.get("/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters), function (data, status) {
                                                            $.each(alloc, function (key, value) {
                                                                if (data.length > 0 && parseInt(data[0].AllocationRange) > 1) {
                                                                    value.color = "red";
                                                                }
                                                                else {
                                                                    value.color = "";
                                                                }
                                                            });
                                                            if (data.length > 0 && parseInt(data[0].AllocationRange) > 1) {
                                                                CompactAlloc.color = "red";
                                                            }
                                                            else {
                                                                CompactAlloc.color = "";
                                                            }
                                                            dataGrid.option("dataSource", compactTempData);
                                                            //CompactPhaseConstraints();
                                                        });
                                                    }

                                                    $("#gridTemplateContainer").dxDataGrid("instance").option('dataSource', globaldata);
                                                    //CompactPhaseConstraints();
                                                }
                                            }
                                        });
                                    }
                                    else {
                                        isAlertActive = false;
                                        let coData = compactTempData.find(x => x.ID == e.row.data.ID);
                                        SetDatesOfSummaryView(e.row.data.ID);
                                        let newIds = CheckAndUpdatePastAllocations();
                                        if (popupFilters.Allocations?.length > 0) {
                                            //e.setValue(v.Name);
                                            let ids = GetUniqueIds(coData);
                                            if (newIds?.length > 0) {
                                                ids = newIds;
                                            }
                                            let allocData = globaldata.filter(x => ids.includes(String(x.ID)));
                                            let roleData = GroupsData.filter(x => x.Id == v.GlobalRoleId);
                                            e.setValue(v.Name);
                                            allocData.forEach(function (e) {
                                                if (ids.includes(String(e.ID)) && new Date(e.AllocationEndDate) > new Date()) {
                                                    e.AssignedTo = v.Id;
                                                    e.AssignedToName = v.Name;
                                                    if (roleData != null && roleData.length > 0) {
                                                        e.Type = roleData[0].Id;
                                                        e.TypeName = roleData[0].Name;
                                                    }
                                                    e.UserImageUrl = v.Picture;
                                                    e.IsResourceDisabled = false;
                                                }
                                            });

                                            if (coData != null) {
                                                if (roleData != null && roleData.length > 0) {
                                                    coData.Type = roleData[0].Id;
                                                    coData.TypeName = roleData[0].Name;
                                                }

                                                if (isDateValid(dataGrid.getDataSource()._items[e.row.rowIndex].AllocationStartDate)
                                                    && isDateValid(dataGrid.getDataSource()._items[e.row.rowIndex].AllocationEndDate)) {
                                                    popupFilters.allocationStartDate = new Date(dataGrid.getDataSource()._items[e.row.rowIndex].AllocationStartDate).toISOString();
                                                    popupFilters.allocationEndDate = new Date(dataGrid.getDataSource()._items[e.row.rowIndex].AllocationEndDate).toISOString();
                                                    popupFilters.SelectedUserID = v.Id;
                                                    popupFilters.projectID = projectID;
                                                    popupFilters.ResourceAvailability = "AllResource";
                                                    $.get("/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters), function (data, status) {
                                                        $.each(allocData, function (key, value) {
                                                            if (data.length > 0 && parseInt(data[0].AllocationRange) > 1) {
                                                                value.color = "red";
                                                            }
                                                            else {
                                                                value.color = "";
                                                            }
                                                        });
                                                        if (data.length > 0 && parseInt(data[0].AllocationRange) > 1) {
                                                            coData.color = "red";
                                                        }
                                                        else {
                                                            coData.color = "";
                                                        }
                                                        //$("#gridTemplateContainer").dxDataGrid("instance").option('dataSource', globaldata);
                                                        //CompactPhaseConstraints();
                                                    });
                                                }
                                                $("#gridTemplateContainer").dxDataGrid("instance").option('dataSource', globaldata);
                                                //alert("step24");
                                                CompactPhaseConstraints();
                                            }
                                        }
                                        else {
                                            debugger;
                                            DevExpress.ui.dialog.alert(`The end date should be in the future.`, 'Error');
                                        }
                                    }
                                }

                            });
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

            },
            onRowValidating: function (e) {
                if (!flag) {
                    if (typeof e.newData.AllocationEndDate !== "undefined") {
                        let value = new Date(e.newData.AllocationEndDate);
                        if (typeof value != undefined) {
                            if (value.getFullYear() < 1000) {
                                e.isValid = false;
                                e.errorText = "Please enter a valid year in format YYYY.";
                            }
                        }
                        if (new Date(e.newData.AllocationEndDate) < new Date(e.key.AllocationStartDate)) {
                            e.isValid = false;
                            e.errorText = "StartDate should be less than EndDate";
                            //e.key.AllocationStartDate = e.newData.AllocationEndDate;
                        }
                    }
                    if (typeof e.newData.AllocationStartDate !== "undefined") {
                        let value = new Date(e.newData.AllocationStartDate);
                        if (typeof value != undefined) {
                            if (value.getFullYear() < 1000) {
                                e.isValid = false;
                                e.errorText = "Please enter a valid year in format YYYY.";
                            }
                        }
                        if (new Date(e.key.AllocationEndDate) < new Date(e.newData.AllocationStartDate)) {
                            e.isValid = false;
                            e.errorText = "StartDate should be less than EndDate";
                            //e.key.AllocationEndDate = e.newData.AllocationStartDate;
                        }
                    }
                }
                else {
                    if (typeof e.newData.AllocationEndDate !== "undefined") {
                        let value = new Date(e.newData.AllocationEndDate);
                        if (typeof value != undefined) {
                            if (value.getFullYear() < 1000) {
                                e.isValid = false;
                                e.errorText = "Please enter a valid year in format YYYY.";
                            }
                        }
                        if (new Date(e.newData.AllocationEndDate) < new Date(e.newData.AllocationStartDate)) {
                            e.isValid = false;
                            e.errorText = "StartDate should be less than EndDate";
                            //e.key.AllocationStartDate = e.newData.AllocationEndDate;
                        }
                    }
                    if (typeof e.newData.AllocationStartDate !== "undefined") {
                        let value = new Date(e.newData.AllocationStartDate);
                        if (typeof value != undefined) {
                            if (value.getFullYear() < 1000) {
                                e.isValid = false;
                                e.errorText = "Please enter a valid year in format YYYY.";
                            }
                        }
                        if (new Date(e.newData.AllocationEndDate) < new Date(e.newData.AllocationStartDate)) {
                            e.isValid = false;
                            e.errorText = "StartDate should be less than EndDate";
                            //e.key.AllocationEndDate = e.newData.AllocationStartDate;
                        }
                    }
                    //openConfirmationDialog(e.newData.AllocationStartDate, e.newData.AllocationEndDate);
                }
                $.cookie("dataChanged", 1, { path: "/" });
                $('#btnCancelChanges').dxButton({ visible: true });
                $.cookie("projTeamAllocSaved", 0, { path: "/" });
                $('#btnCancelChanges').dxButton({ visible: true });
                isGridInValidState = e.isValid;
            },
            onCellPrepared: function (e) {
                if (e.rowType === 'data') {
                    var preconstartDate = new Date(dataModel.preconStartDate);
                    var preconEndDate = new Date(dataModel.preconEndDate);

                    var conststartDate = new Date(dataModel.constStartDate);
                    var constEndDate = new Date(dataModel.constEndDate);

                    var closeoutstartDate = new Date(dataModel.closeoutStartDate);
                    var closeoutEndDate = new Date(dataModel.closeoutEndDate);

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

                    if (e.column.dataField == 'AssignedToName') {
                        if (e.data.color == "red") {
                            e.cellElement.addClass('redCellColorStyle');
                        }
                    }
                }
            }
        });

        $('#popup').dxPopup({
            visible: false,
            hideOnOutsideClick: false,
            showTitle: true,
            showCloseButton: false,
            title: "Update Dates",
            width: "auto",
            height: "auto",
            resizeEnabled: true,
            dragEnabled: true,
            position: {
                my: 'center',
                at: 'top',
                collision: 'fit',
            },
            contentTemplate: () => {
                const content = $("<div />");
                content.append(
                    $("<div id='form' />").dxForm({
                        formData: dataModel,
                        title: 'Update Dates',
                        items: [{
                            itemType: 'group',
                            name: 'group',
                            caption: '',
                            colCount: 3,
                            items: [{
                                dataField: 'preconStartDate',
                                editorType: 'dxDateBox',
                                editorOptions: {
                                    value: undefined,
                                    type: "date",
                                    displayFormat: "MM/dd/yyyy", 
                                    onValueChanged(e) {
                                        var enteredPreconStartDate = e.value;
                                        let newdate = new Date(enteredPreconStartDate);
                                        if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                            //DevExpress.ui.dialog.alert("Please enter a valid date.");
                                            return;
                                        }

                                        if (enteredPreconStartDate != null) {
                                            if (String(enteredPreconStartDate).startsWith("00")) {
                                                enteredPreconStartDate = enteredPreconStartDate.replace(/^.{2}/g, '20');
                                                e.value = enteredPreconStartDate;
                                                dataModel.preconStartDate = enteredPreconStartDate;
                                            }
                                        }
                                        if (dataModel.preconEndDate != '' && dataModel.preconStartDate != '') {
                                            dataModel.preconDuration = GetDurationInWeek(ajaxHelperPage, dataModel.preconStartDate, dataModel.preconEndDate);
                                            $("#form").dxForm("instance").updateData({ 'preconDuration': dataModel.preconDuration });
                                        }
                                    },
                                    displayFormat: {
                                        parser: function (value) {
                                            let parts = value.split('/');
                                            if (3 !== parts.length) {
                                                return;
                                            }
                                            return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                        },
                                        formatter: function (value) {
                                            return DevExpress.localization.date.format(value, 'shortdate');
                                        }

                                    }
                                },
                                //validationRules: [{
                                //    type: 'required',
                                //    message: 'Precon Start date is required',
                                //}],
                                label: {
                                    template: labelTemplate('PreCon Start'),
                                },
                            },
                                {
                                    dataField: 'preconDuration',
                                    editorType: 'dxNumberBox',
                                    editorOptions: {
                                        value: undefined,
                                        onFocusOut(e) {
                                            if (e.component.option('value') != '' && dataModel.preconStartDate != '') {
                                                dataModel.preconEndDate = GetEndDateByWeeks(ajaxHelperPage, dataModel.preconStartDate, e.component.option('value'));
                                                $("#form").dxForm("instance").updateData({ 'preconEndDate': new Date(dataModel.preconEndDate).toLocaleDateString('en-US') });
                                            }
                                        },
                                    },
                                },
                                {
                                    dataField: 'preconEndDate',
                                    editorType: 'dxDateBox',
                                    editorOptions: {
                                        value: undefined,
                                        type: "date",
                                        displayFormat: "MM/dd/yyyy", 
                                        onValueChanged(e) {
                                            var enteredPreconEndDate = e.value;
                                            let newdate = new Date(enteredPreconEndDate);
                                            if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                                //DevExpress.ui.dialog.alert("Please enter a valid date.");
                                                return;
                                            }
                                            if (enteredPreconEndDate != null) {
                                                if (String(enteredPreconEndDate).startsWith("00")) {
                                                    enteredPreconEndDate = enteredPreconEndDate.replace(/^.{2}/g, '20');
                                                    e.value = enteredPreconEndDate;
                                                    dataModel.preconEndDate = enteredPreconEndDate;
                                                }
                                            }
                                            if (dataModel.preconEndDate != '' && dataModel.preconStartDate != '') {
                                                dataModel.preconDuration = GetDurationInWeek(ajaxHelperPage, dataModel.preconStartDate, dataModel.preconEndDate);
                                                $("#form").dxForm("instance").updateData({ 'preconDuration': dataModel.preconDuration });
                                            }
                                        },
                                        displayFormat: {
                                            parser: function (value) {
                                                let parts = value.split('/');
                                                if (3 !== parts.length) {
                                                    return;
                                                }
                                                return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                            },
                                            formatter: function (value) {
                                                return DevExpress.localization.date.format(value, 'shortdate');
                                            }

                                        }
                                    },
                                    //validationRules: [{
                                    //    type: 'required',
                                    //    message: 'Precon End date is required',
                                    //}],
                                    label: {
                                        template: labelTemplate('PreCon End'),
                                    },
                                },
                                {
                                    dataField: 'constStartDate',
                                    editorType: 'dxDateBox',
                                    editorOptions: {
                                        value: dataModel.constStartDate,
                                        type: "date",
                                        displayFormat: "MM/dd/yyyy", 
                                        onValueChanged(e) {
                                            var enteredConstStartDate = e.value;
                                            let newdate = new Date(enteredConstStartDate);
                                            if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                                //DevExpress.ui.dialog.alert("Please enter a valid date.");
                                                return;
                                            }
                                            if (enteredConstStartDate != null) {
                                                if (String(enteredConstStartDate).startsWith("00")) {
                                                    enteredConstStartDate = enteredConstStartDate.replace(/^.{2}/g, '20');
                                                    e.value = enteredConstStartDate;
                                                    dataModel.constStartDate = enteredConstStartDate;
                                                }
                                            }
                                            if (dataModel.constEndDate != '' && dataModel.constStartDate != '') {
                                                dataModel.constDuration = GetDurationInWeek(ajaxHelperPage, dataModel.constStartDate, dataModel.constEndDate);
                                                $("#form").dxForm("instance").updateData({ 'constDuration': dataModel.constDuration });
                                            }
                                        },
                                        displayFormat: {
                                            parser: function (value) {
                                                let parts = value.split('/');
                                                if (3 !== parts.length) {
                                                    return;
                                                }
                                                return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                            },
                                            formatter: function (value) {
                                                return DevExpress.localization.date.format(value, 'shortdate');
                                            }

                                        }
                                    },
                                    //validationRules: [{
                                    //    type: 'required',
                                    //    message: 'Const Start date is required',
                                    //}],
                                    label: {
                                        template: labelTemplate('Const Start'),
                                    },
                                },
                                {
                                    dataField: 'constDuration',
                                    editorType: 'dxNumberBox',
                                    editorOptions: {
                                        value: undefined,
                                        onFocusOut(e) {
                                            if (e.component.option('value') != '' && dataModel.constStartDate != '') {
                                                dataModel.constEndDate = GetEndDateByWeeks(ajaxHelperPage, dataModel.constStartDate, e.component.option('value'));
                                                $("#form").dxForm("instance").updateData({ 'constEndDate': new Date(dataModel.constEndDate).toLocaleDateString('en-US') });
                                            }
                                        },
                                    },
                                    label: {
                                        template: labelTemplate('PreCon Start'),
                                    },
                                },
                                {
                                    dataField: 'constEndDate',
                                    editorType: 'dxDateBox',
                                    editorOptions: {
                                        value: dataModel.constEndDate,
                                        type: "date",
                                        displayFormat: "MM/dd/yyyy", 
                                        onValueChanged(e) {
                                            var enteredConstEndDate = e.value;
                                            let newdate = new Date(enteredConstEndDate);
                                            if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                                //DevExpress.ui.dialog.alert("Please enter a valid date.");
                                                return;
                                            }
                                            if (enteredConstEndDate != null) {
                                                if (String(enteredConstEndDate).startsWith("00")) {
                                                    enteredConstEndDate = enteredConstEndDate.replace(/^.{2}/g, '20');
                                                    e.value = enteredConstEndDate;
                                                }
                                            }
                                            if (dataModel.constEndDate != '' && dataModel.constStartDate != '') {
                                                dataModel.constDuration = GetDurationInWeek(ajaxHelperPage, dataModel.constStartDate, dataModel.constEndDate);
                                                $("#form").dxForm("instance").updateData({ 'constDuration': dataModel.constDuration });
                                            }
                                            if (e.value != null) {
                                                $.ajax({
                                                    type: "GET",
                                                    url: "<%=rmoneControllerUrl%>GetNextWorkingDateAndTime?dateString=" + new Date(e.value).toLocaleDateString('en-US'),
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    async: false,
                                                    success: function (message) {
                                                        dataModel.closeoutStartDate = new Date(message).toLocaleDateString('en-US');
                                                        dataModel.closeoutEndDate = new Date(GetEndDateByWorkingDays(ajaxHelperPage, message, "<%=closeoutperiod%>")).toISOString();
                                                        dataModel.closeOutDuration = GetDurationInWeek(ajaxHelperPage, dataModel.closeoutStartDate, dataModel.closeoutEndDate);
                                                        $("#form").dxForm("instance").updateData({ 'closeoutEndDate': new Date(dataModel.closeoutEndDate).toLocaleDateString('en-US'), 'closeoutStartDate': dataModel.closeoutStartDate, 'closeOutDuration': dataModel.closeOutDuration });
                                                    },
                                                    error: function (xhr, ajaxOptions, thrownError) {

                                                    }
                                                });
                                            }
                                        },
                                        displayFormat: {
                                            parser: function (value) {
                                                let parts = value.split('/');
                                                if (3 !== parts.length) {
                                                    return;
                                                }
                                                return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                            },
                                            formatter: function (value) {
                                                return DevExpress.localization.date.format(value, 'shortdate');
                                            }

                                        }
                                    },
                                    label: {
                                        template: labelTemplate('Const End'),
                                    },
                                },
                                {
                                    dataField: 'closeoutStartDate',
                                    editorType: 'dxDateBox',
                                    editorOptions: {
                                        value: dataModel.closeoutStartDate,
                                        format: 'MMM d, yyyy',
                                        readOnly: true,
                                    },
                                    //validationRules: [{
                                    //    type: 'required',
                                    //    message: 'Closeout date is required',
                                    //}],
                                    label: {
                                        template: labelTemplate('Close Out'),
                                    },
                                },
                                {
                                    dataField: 'closeOutDuration',
                                    editorType: 'dxNumberBox',
                                    editorOptions: {
                                        value: undefined,
                                        onFocusOut(e) {
                                            if (e.component.option('value') != '' && dataModel.closeoutStartDate != '') {
                                                dataModel.closeoutEndDate = GetEndDateByWeeks(ajaxHelperPage, dataModel.closeoutStartDate, e.component.option('value'));
                                                $("#form").dxForm("instance").updateData({ 'closeoutEndDate': new Date(dataModel.closeoutEndDate).toLocaleDateString('en-US') });
                                            }
                                        },
                                    },
                                    label: {
                                        template: labelTemplate('PreCon Start'),
                                    },
                                },
                                {
                                    dataField: 'closeoutEndDate',
                                    editorType: 'dxDateBox',
                                    editorOptions: {
                                        value: dataModel.closeoutEndDate,
                                        type: "date",
                                        displayFormat: "MM/dd/yyyy", 
                                        onValueChanged(e) {
                                            var enteredCloseoutEndDate = e.value;
                                            let newdate = new Date(enteredCloseoutEndDate);
                                            if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                                //DevExpress.ui.dialog.alert("Please enter a valid date.");
                                                return;
                                            }
                                            if (enteredCloseoutEndDate != null) {
                                                if (String(enteredCloseoutEndDate).startsWith("00")) {
                                                    enteredCloseoutEndDate = enteredCloseoutEndDate.replace(/^.{2}/g, '20');
                                                    e.value = enteredCloseoutEndDate;
                                                    dataModel.closeoutEndDate = enteredCloseoutEndDate;
                                                }
                                            }
                                            if (dataModel.closeoutEndDate != '' && dataModel.closeoutStartDate != '') {
                                                dataModel.closeOutDuration = GetDurationInWeek(ajaxHelperPage, dataModel.closeoutStartDate, dataModel.closeoutEndDate);
                                                $("#form").dxForm("instance").updateData({ 'closeOutDuration': dataModel.closeOutDuration });
                                            }
                                        },
                                        displayFormat: {
                                            parser: function (value) {
                                                let parts = value.split('/');
                                                if (3 !== parts.length) {
                                                    return;
                                                }
                                                return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                            },
                                            formatter: function (value) {
                                                return DevExpress.localization.date.format(value, 'shortdate');
                                            }

                                        }
                                    },
                                    //validationRules: [{
                                    //    type: 'required',
                                    //    message: 'Closeout date is required',
                                    //}],
                                    label: {
                                        template: labelTemplate('Close Out End'),
                                    },
                                },
                                {
                                    dataField: 'onHold',
                                    editorType: 'dxSwitch',
                                    visible: false,
                                    editorOptions: {
                                        width: 100,
                                        value: dataModel.onHold,
                                        switchedOffText: "OFF HOLD",
                                        switchedOnText: "ON HOLD",
                                    },
                                    label: {
                                        template: labelTemplate(''),
                                    },
                                }
                            ]
                        }],
                        onContentReady: function (data) {
                            data.element.find("label[for$='preconDuration']").text("Precon Duration(Weeks)");
                            data.element.find("label[for$='constDuration']").text("Const Duration(Weeks)");
                            data.element.find("label[for$='closeOutDuration']").text("Closeout Duration(Weeks)");
                        }
                    }),
                    $("#saveButton").dxButton({
                        text: 'Save',
                        icon: '/content/Images/saveFile_icon.png',
                        onClick: function (e) {
                            UpdateRecord();
                        }
                    }),
                    $(`#cancelButton`).dxButton({
                        text: "Cancel",
                        visible: true,
                        onClick: function (e) {
                            dataModel = tempdataModel;
                            var popup = $("#popup").dxPopup("instance");
                            popup.hide();
                        }
                    })
                );
                return content;
            },
            onDisposing: function (e) {
                dataModel.preconStartDate = "";
                dataModel.preconEndDate = "";
                dataModel.constStartDate = "";
                dataModel.constEndDate = "";
                dataModel.closeoutEndDate = "";
                dataModel.onHold = false;
            }
        });

        $("#pPreconNoDate").click(function (e) {
            openDateAgent(Model.RecordId);
        });

        $("#pConstNoDate").click(function (e) {
            openDateAgent(Model.RecordId);
        });

        $("#pCloseoutNoDate").click(function (e) {
            openDateAgent(Model.RecordId);
        });
    });

    function getDateBackgroundColor(cellValue, preconstartDate, conststartDate, constEndDate, closeoutstartDate) {

        let preconEndDate = new Date(dataModel.preconEndDate);
        let closeoutEndDate = new Date(dataModel.closeoutEndDate);

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
    function IsPhaseDatesAvailable() {
        let preconstartDate = new Date(dataModel.preconStartDate);
        let preconEndDate = new Date(dataModel.preconEndDate);

        let conststartDate = new Date(dataModel.constStartDate);
        let constEndDate = new Date(dataModel.constEndDate);

        let closeoutstartDate = new Date(dataModel.closeoutStartDate);
        let closeoutEndDate = new Date(dataModel.closeoutEndDate);

        if (preconstartDate == "Invalid Date" || preconstartDate == '' || preconEndDate == "Invalid Date" || preconEndDate == '') {
            return false;
        }
        if (conststartDate == "Invalid Date" || conststartDate == '' || constEndDate == "Invalid Date" || constEndDate == '') {
            return false;
        }
        if (closeoutstartDate == "Invalid Date" || closeoutstartDate == '' || closeoutEndDate == "Invalid Date" || closeoutEndDate == '') {
            return false;
        }
        return true;
    }
    function OpenInternalAllocation(refIds) {
        debugger;
        tempGloblaDataStorage = JSON.parse(JSON.stringify(globaldata));
        let ids = refIds.split(";");
        tempRefIds = ids;
        if (ids != null && ids.length > 0) {
            let gdata = globaldata.filter(x => ids.includes(String(x.ID)));
            const popupContentTemplate1 = function () {
                let container = $("<div>");
                let datagrid = $("<div id='InternalAllocationGrid'>").dxDataGrid({
                    dataSource: gdata,
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
                            allowEditing: true,
                            sortIndex: "0",
                            sortOrder: "asc",
                            cellTemplate: function (container, options) {
                                $('.dx-header-row').addClass('devExtDataGrid-headerRow');
                                $('.dx-data-row').addClass('devExtDataGrid-DataRow');
                                if (options.key.ID > 0) {

                                    var str = options.data.AssignedTo + "','" + options.data.AssignedToName.replace("'", "`");
                                    var strwithspace = str.replace(/ /g, "&nbsp;")
                                    $("<div id='dataId'>")
                                        .append("<span style='float: left;overflow: auto;'>" + (options.data.UserImageUrl != null && options.data.UserImageUrl != "" ? "<img src=" + options.data.UserImageUrl + " class='profileUserImg' />" + '  ' : '') + "<a href='javascript:void(0);' onclick=openResourceTimeSheet('" + strwithspace + "');>"
                                        + (options.data.IsResourceDisabled ? "<span style='color:red;'>" + options.value + "</span>" : options.value) + "</a></span>")
                                        .append($("<img>", { "src": "/content/images/moreoptions_blue.png", "ID": options.data.ID, "group": options.data.Type, "startDate": options.data.AllocationStartDate, "endDate": options.data.AllocationEndDate, "assignedTo": options.data.AssignedToName, "style": "float: right;overflow: auto;cursor: pointer;", "class": "assigneeToImg", "checkOnGlobalData": "true" }))
                                        .appendTo(container);
                                }
                                if (options.key.ID <= 0) {
                                    var str = options.data.AssignedTo + "','" + options.data.AssignedToName.replace("'", "`");
                                    var strwithspace = str.replace(" ", "&nbsp;")
                                    $("<div id='dataId'>")
                                        .append("<span style='float: left;overflow: auto;'>" + (options.data.UserImageUrl != null && options.data.UserImageUrl != "" ? "<img src=" + options.data.UserImageUrl + " class='profileUserImg' />" + '  ' : '') + "<a href='javascript:void(0);' onclick=openResourceTimeSheet('" + strwithspace + "');>"
                                        + (options.data.IsResourceDisabled ? "<span style='color:red;'>" + options.value + "</span>" : options.value) + "</a></span>")
                                        .append($("<img>", { "src": "/content/images/moreoptions_blue.png", "ID": options.data.ID, "group": options.data.Type, "startDate": options.data.AllocationStartDate, "endDate": options.data.AllocationEndDate, "assignedTo": options.data.AssignedToName, "style": "float: right;overflow: auto;cursor: pointer;", "class": "assigneeToImg", "checkOnGlobalData": "true" }))
                                        .appendTo(container);
                                }
                            }
                        },
                        {
                            dataField: "TypeName",
                            dataType: "text",
                            allowEditing: false,
                            caption: "Role",
                            sortIndex: "1",
                            sortOrder: "asc",
                            width: "23%",
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
                            width: "15%",
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
                            width: "10%",
                            setCellValue: function (newData, value, currentRowData) {
                                if (parseInt(value) <= 0) {
                                    let data = globaldata.filter(x => x.ID == currentRowData.ID)[0];
                                    DeleteAllocation(data, ids);
                                    newData.PctAllocation = currentRowData.PctAllocation;
                                }
                                else {
                                    SplitAllocationOnPctChange(currentRowData.ID, value, ids);
                                    $("#gridTemplateContainer").dxDataGrid("instance").option("dataSource", globaldata);
                                    //alert("step25");
                                    CompactPhaseConstraints();
                                }
                            }
                        },
                        {
                            width: "10%",
                            fieldName: "SoftAllocation",
                            name: 'SoftAllocation',
                            caption: "",
                            dataType: "text",
                            //showEditorAlways: false,  
                            cellTemplate: function (container, options) {
                                $("<div id='divSoftAllocation' style='padding-left:10px;padding-right:10px;' >").append(
                                    $("<div id='divSwitch' />").dxSwitch({
                                        switchedOffText: 'Hard',
                                        switchedOnText: 'Soft',
                                        width: 60,
                                        value: options.data.SoftAllocation,
                                        onValueChanged(data) {
                                            $.cookie("dataChanged", 1, { path: "/" });
                                            $('#btnCancelChanges').dxButton({ visible: true });
                                        },
                                    })
                                ).appendTo(container);
                            }
                        },
                        {
                            width: "7%",
                            fieldName: "NonChargeable",
                            caption: 'NCO',
                            dataType: 'text',
                            cellTemplate: function (container, options) {
                                $("<div id='divNonChargeable' style='padding-left:10px;padding-right:10px;' >").append(
                                    $("<div id='divSwitchNonChargeable' />").dxCheckBox({
                                        //switchedOffText: 'BillHr',
                                        //switchedOnText: 'NCO',
                                        width: 30,
                                        value: options.data.NonChargeable,
                                        onValueChanged(data) {
                                            $.cookie("dataChanged", 1, { path: "/" });
                                            $('#btnCancelChanges').dxButton({ visible: true });
                                        },
                                    })
                                ).appendTo(container);
                            }
                        }
                    ],
                    showBorders: true,
                    showRowLines: true,
                    onEditorPreparing: function (e) {
                        if (e.parentType === 'dataRow' && e.dataField === 'AssignedToName') {
                            var dataGrid = $("#InternalAllocationGrid").dxDataGrid("instance");
                            let rType = dataGrid.getDataSource()._items[e.row.rowIndex]?.Type;
                            let uData = rType != '' && rType != null && !rType.startsWith("TYPE-") ? UsersData.filter(x => x.GlobalRoleId == rType && x.Enabled == true).sort((a, b) => (a.Name > b.Name) ? 1 : -1)
                                : UsersData.filter(x => x.Enabled == true).sort((a, b) => (a.Name > b.Name) ? 1 : -1);

                            e.editorElement.dxSelectBox({
                                dataSource: uData,
                                valueExpr: "Id",
                                displayExpr: "Name",
                                value: e.row.data.AssignedTo,
                                searchEnabled: true,
                                onValueChanged: function (ea) {
                                    $.each(ea.component._dataSource._items, function (i, v) {
                                        if (v.Id === ea.value) {
                                            SetDatesOfSummaryView(e.row.data.ID, true);
                                            if (popupFilters.Allocations?.length > 0) {
                                                let newIds = CheckAndUpdatePastAllocations();
                                                e.setValue(v.Name);
                                                let alloc = null;
                                                if (newIds?.length > 0) {
                                                    alloc = globaldata.find(x => newIds.includes(String(x.ID)));
                                                } else {
                                                    alloc = globaldata.find(x => x.ID == e.row.data.ID);
                                                }
                                                alloc.AssignedTo = v.Id;
                                                alloc.AssignedToName = v.Name;
                                                alloc.UserImageUrl = v.Picture;
                                                alloc.IsResourceDisabled = false;

                                                let x = GroupsData.filter(x => x.Id == v.GlobalRoleId);
                                                if (x != null && x.length > 0) {
                                                    alloc.Type = x[0].Id;
                                                    alloc.TypeName = x[0].Name;
                                                }
                                                if (tempRefIds.length > 0 && tempRefIds.find(x => String(x) == String(alloc.ID)) == null) {
                                                    tempRefIds.push(String(alloc.ID));
                                                }
                                                $("#gridTemplateContainer").dxDataGrid("instance").option("dataSource", globaldata);
                                                dataGrid.option("dataSource", globaldata.filter(x => tempRefIds.includes(String(x.ID))));
                                                //alert("step26");
                                                CompactPhaseConstraints();
                                            }
                                            else {
                                                DevExpress.ui.dialog.alert(`The end date should be in the future.`, 'Error');
                                            }
                                        }
                                    });
                                }
                            });
                            e.cancel = true;

                        }
                    },
                    onCellPrepared: function (e) {
                        if (e.rowType === 'data') {
                            var preconstartDate = new Date(dataModel.preconStartDate);
                            var preconEndDate = new Date(dataModel.preconEndDate);

                            var conststartDate = new Date(dataModel.constStartDate);
                            var constEndDate = new Date(dataModel.constEndDate);

                            var closeoutstartDate = new Date(dataModel.closeoutStartDate);
                            var closeoutEndDate = new Date(dataModel.closeoutEndDate);

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
                    }
                });
                let confirmBtn = $(`<div class="mt-4 mb-2 btnAddNew" style='float:right;padding: 0px 10px;font-size: 14px;' />`).dxButton({
                    text: "Save",
                    hint: 'Save Allocations',
                    visible: true,
                    onClick: function (e) {
                        let dataGridChild = $("#InternalAllocationGrid").dxDataGrid("instance");
                        let rowschild = dataGridChild.getVisibleRows();
                        globaldata = globaldata.filter(x => !ids.includes(String(x.ID)));
                        $.each(rowschild, function (index, e) {
                            if (parseInt(e.data.PctAllocation) > 0) {
                                globaldata.push(e.data);
                            }
                        });
                        var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                        dataGrid.option("dataSource", globaldata);
                        CompactPhaseConstraints();
                        popup.hide();
                    }
                })
                let cancelBtn = $(`<div class="mt-4 mb-2 btnAddNew" style='float:right;padding: 0px 10px;font-size: 14px;' />`).dxButton({
                    text: "Cancel",
                    visible: true,
                    onClick: function (e) {
                        globaldata = JSON.parse(JSON.stringify(tempGloblaDataStorage));
                        var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                        dataGrid.option("dataSource", globaldata);
                        //alert("step27");
                        CompactPhaseConstraints();
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
                title: "Edit Allocations",
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
        if (parseInt(row.oldData.PctAllocation) == 0 && parseInt(row.newData.PctAllocation) > 0 && dataModel.preconStartDate != '' && dataModel.preconEndDate != '') {
            var sum = 0;
            sum = globaldata.length + 100 + Math.floor((Math.random() * 100) + 1);
            let TotalWorkingDays = GetTotalWorkingDaysBetween(ajaxHelperPage, dataModel.preconStartDate, dataModel.preconEndDate);
            globaldata.push({ "ID": -Math.abs(sum), "AssignedTo": data.AssignedTo, "AssignedToName": data.AssignedToName, "AllocationStartDate": dataModel.preconStartDate, "AllocationEndDate": dataModel.preconEndDate, "PctAllocation": parseInt(row.newData.PctAllocation), "TotalWorkingDays": TotalWorkingDays, "SoftAllocation": '<%= IsAllocationTypeHard_Soft%>' == 'False' ? false : true, "NonChargeable": false, "Type": data.Type, "TypeName": data.TypeName, "Tags": existingProjectTags.filter(y => y.Type == 2).map(x => x.TagId).join() });
        }
        if (parseInt(row.oldData.PctAllocationConst) == 0 && parseInt(row.newData.PctAllocationConst) > 0 && dataModel.constStartDate != '' && dataModel.constEndDate != '') {
            var sum = 0;
            sum = globaldata.length + 200 + Math.floor((Math.random() * 100) + 1);
            let TotalWorkingDays = GetTotalWorkingDaysBetween(ajaxHelperPage, dataModel.constStartDate, dataModel.constEndDate);
            globaldata.push({ "ID": -Math.abs(sum), "AssignedTo": data.AssignedTo, "AssignedToName": data.AssignedToName, "AllocationStartDate": dataModel.constStartDate, "AllocationEndDate": dataModel.constEndDate, "PctAllocation": parseInt(row.newData.PctAllocationConst), "TotalWorkingDays": TotalWorkingDays, "SoftAllocation": '<%= IsAllocationTypeHard_Soft%>' == 'False' ? false : true, "NonChargeable": false, "Type": data.Type, "TypeName": data.TypeName, "Tags": existingProjectTags.filter(y => y.Type == 2).map(x => x.TagId).join() });
        }
        if (parseInt(row.oldData.PctAllocationCloseOut) == 0 && parseInt(row.newData.PctAllocationCloseOut) > 0 && dataModel.closeoutStartDate != '' && dataModel.closeoutEndDate != '') {
            var sum = 0;
            sum = globaldata.length + 300 + Math.floor((Math.random() * 100) + 1);
            let TotalWorkingDays = GetTotalWorkingDaysBetween(ajaxHelperPage, dataModel.closeoutStartDate, dataModel.closeoutEndDate);
            globaldata.push({ "ID": -Math.abs(sum), "AssignedTo": data.AssignedTo, "AssignedToName": data.AssignedToName, "AllocationStartDate": dataModel.closeoutStartDate, "AllocationEndDate": dataModel.closeoutEndDate, "PctAllocation": parseInt(row.newData.PctAllocationCloseOut), "TotalWorkingDays": TotalWorkingDays, "SoftAllocation": '<%= IsAllocationTypeHard_Soft%>' == 'False' ? false : true, "NonChargeable": false, "Type": data.Type, "TypeName": data.TypeName, "Tags": existingProjectTags.filter(y => y.Type == 2).map(x => x.TagId).join()});
        }
        let gdata = globaldata.filter(x => x.ID == data.PreconId || x.ID == data.ConstId || x.ID == data.CloseOutId);
        $.each(gdata, function (index, e) {
            if (e.ID == data.PreconId && row.newData.PctAllocation != null && row.newData.PctAllocation != "") {
                if (parseInt(row.newData.PctAllocation) > 0) {
                    SplitAllocationOnPctChange(data.PreconId, row.newData.PctAllocation)
                    //e.PctAllocation = row.newData.PctAllocation;
                    //data.PctAllocation = e.PctAllocation;
                }
                else {
                    if (parseInt(e.ID) > 0) {
                        DeleteAllocation(e);
                    }
                    else {
                        globaldata = globaldata.filter(x => x.ID != e.ID);
                    }
                }
            }
            if (e.ID == data.ConstId && row.newData.PctAllocationConst != null && row.newData.PctAllocationConst != "") {
                if (parseInt(row.newData.PctAllocationConst) > 0) {
                    SplitAllocationOnPctChange(data.ConstId, row.newData.PctAllocationConst);
                    //e.PctAllocation = row.newData.PctAllocationConst;
                    //data.PctAllocation = e.PctAllocation;
                }
                else {
                    if (parseInt(e.ID) > 0) {
                        DeleteAllocation(e);
                    }
                    else {
                        globaldata = globaldata.filter(x => x.ID != e.ID);
                    }
                }
            }
            if (e.ID == data.CloseOutId && row.newData.PctAllocationCloseOut != null && row.newData.PctAllocationCloseOut != "") {
                if (parseInt(row.newData.PctAllocationCloseOut) > 0) {
                    SplitAllocationOnPctChange(data.CloseOutId, row.newData.PctAllocationCloseOut);
                    //e.PctAllocation = row.newData.PctAllocationCloseOut;
                    //data.PctAllocation = e.PctAllocation;
                }
                else {
                    if (parseInt(e.ID) > 0) {
                        DeleteAllocation(e);
                    }
                    else {
                        globaldata = globaldata.filter(x => x.ID != e.ID);
                    }
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

        var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
        dataGrid.option("dataSource", globaldata);
        //alert("step28");
        CompactPhaseConstraints();
    }
    function CalculatePctAllocation(dataRow, minDate, maxDate) {
        let totalPercentage = 0;

        let preconStartDate = new Date(dataModel.preconStartDate);

        let closeoutEndDate = new Date(dataModel.closeoutEndDate);

        dataRow = dataRow.filter(x => new Date(x.AllocationEndDate) >= preconStartDate && new Date(x.AllocationStartDate) <= closeoutEndDate);
        /*$.each(dataRow, function (index, e) {
            let allocStartdate = new Date(e.AllocationStartDate);
            let allocEnddate = new Date(e.AllocationEndDate);
            if (allocStartdate < minDate) {
                minDate = allocStartdate;
            }
            if (allocEnddate > maxDate) {
                maxDate = allocEnddate;
            }
        });*/
        if (minDate < preconStartDate) {
            minDate = preconStartDate;
        }
        if (maxDate > closeoutEndDate) {
            maxDate = closeoutEndDate;
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

    function CalculatePctAllocationNew(dataRow) {
        let totalPercentage = 0;
        var totalDays = 0;
        $.each(dataRow, function (index, e) {
            var daysDiff = parseInt(e.TotalWorkingDays);
            totalDays += daysDiff;
            let pctAlloc = parseInt(e.PctAllocation);
            totalPercentage += pctAlloc * daysDiff;
        });
        return totalDays > 0 ? Math.ceil(totalPercentage / totalDays) : 0;
    }
    function CompactPhaseConstraints() {
        if (!EnforcePhaseConstraints) {
            return;
        }
        compactTempData = [];
        //debugger;
        let tempData = JSON.parse(JSON.stringify(globaldata));
        let isDateInMultiPhase = false;
        $.each(tempData, function (index, e) {
            let data1 = JSON.parse(JSON.stringify(tempData.filter(x => x.AssignedTo == e.AssignedTo && x.Type == e.Type)));
            let internalPhaseData = [];
            let constStartDate = (new Date(dataModel.constStartDate));
            let constEndDate = (new Date(dataModel.constEndDate));

            let preconStartDate = new Date(dataModel.preconStartDate);
            let preconEndDate = new Date(dataModel.preconEndDate);

            let closeoutStartDate = new Date(dataModel.closeoutStartDate);
            let closeoutEndDate = new Date(dataModel.closeoutEndDate);

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
                        let preconStartDate = new Date(dataModel.preconStartDate);
                        let preconEndDate = new Date(dataModel.preconEndDate);

                        let constStartDate = (new Date(dataModel.constStartDate));
                        let constEndDate = (new Date(dataModel.constEndDate));

                        if (!isDateValid(preconStartDate) && startDate < constStartDate) {
                            preconStartDate = startDate;
                        }
                        if (!isDateValid(preconEndDate) && isDateValid(constStartDate)) {
                            preconEndDate = constStartDate.addDays(-1);
                        }
                        if (startDate < constStartDate && endDate > constEndDate) {
                            data[0].PctAllocation = data[0].PctAllocation;
                            if(new Date(odata[0].AllocationEndDate) < preconEndDate || new Date(odata[0].AllocationStartDate) > preconStartDate)
                            {
                                //let percentage = CalculatePctAllocation([odata[0]], preconStartDate, preconEndDate);
                                //data[0].PctAllocation = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                                data[0].preconRefIds = data[0].preconRefIds == null || data[0].preconRefIds =='' ? data[0].ID : data[0].preconRefIds;
                            }

                            data[0].PctAllocationCloseOut = data[1].PctAllocation;
                            if(new Date(odata[1].AllocationEndDate) < closeoutEndDate || new Date(odata[1].AllocationStartDate) > closeoutStartDate)
                            {
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
                            if(new Date(odata[0].AllocationEndDate) < preconEndDate || new Date(odata[0].AllocationStartDate) > preconStartDate)
                            {
                                //let percentage = CalculatePctAllocation([odata[0]], preconStartDate, preconEndDate);
                                //data[0].PctAllocation = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                                data[0].preconRefIds = data[0].preconRefIds == null || data[0].preconRefIds =='' ? data[0].ID : data[0].preconRefIds;
                            }

                            data[0].PctAllocationConst = data[1].PctAllocation;
                            if(new Date(odata[1].AllocationEndDate) < constEndDate || new Date(odata[1].AllocationStartDate) > constStartDate)
                            {
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
                            if(new Date(odata[0].AllocationEndDate) < constEndDate || new Date(odata[0].AllocationStartDate) > constStartDate)
                            {
                                //data[0].PctAllocationConst = CalculatePctAllocation([odata[0]], constStartDate, constEndDate);
                                data[0].constRefIds = data[0].constRefIds == null || data[0].constRefIds =='' ? data[0].ID : data[0].constRefIds;
                            }

                            data[0].PctAllocationCloseOut = data[1].PctAllocation;
                            if(new Date(odata[1].AllocationEndDate) < closeoutEndDate || new Date(odata[1].AllocationStartDate) > closeoutStartDate)
                            {
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
                        if(new Date(odata[0].AllocationEndDate) < preconEndDate || new Date(odata[0].AllocationStartDate) > preconStartDate)
                        {
                            //let percentage = CalculatePctAllocation([odata[0]], preconStartDate, preconEndDate);
                            //data[0].PctAllocation = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                            data[0].preconRefIds = data[0].preconRefIds == null || data[0].preconRefIds =='' ? data[0].ID : data[0].preconRefIds;
                        }

                        data[0].PctAllocationConst = data[1].PctAllocation;
                        if(new Date(odata[1].AllocationEndDate) < constEndDate || new Date(odata[1].AllocationStartDate) > constStartDate)
                        {
                            //data[0].PctAllocationConst = CalculatePctAllocation([odata[1]], constStartDate, constEndDate);
                            data[0].constRefIds = data[1].constRefIds == null || data[1].constRefIds == '' ? data[1].ID : data[1].constRefIds; //data[1].ID;
                        }

                        data[0].PctAllocationCloseOut = data[2].PctAllocation;
                        if(new Date(odata[2].AllocationEndDate) < closeoutEndDate || new Date(odata[2].AllocationStartDate) > closeoutStartDate)
                        {
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
                    let preconStartDate = new Date(dataModel.preconStartDate);
                    let preconEndDate = new Date(dataModel.preconEndDate);

                    let constStartDate = (new Date(dataModel.constStartDate));
                    let constEndDate = (new Date(dataModel.constEndDate));

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
        var compactDataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
        compactDataGrid.option("dataSource", compactTempData);
        compactDataGrid._refresh();
        $("#btnFixOverlaps").hide();
    }

    function CheckPhaseConstraints(checkDateInMultiPhase, saveAllocation) {
        if (!EnforcePhaseConstraints) {
            return;
        }
        if (saveAllocation == undefined) {
            saveAllocation = false;
        }
        OverlappingAllocationInPhases = [];
        let tempData = JSON.parse(JSON.stringify(globaldata));
        //compactTempData = Object.create(globaldata);
        let isDateInMultiPhase = false;
        $.each(tempData, function (index, e) {
            let preconStartDate = new Date(dataModel.preconStartDate);
            let preconEndDate = new Date(dataModel.preconEndDate);

            let constStartDate = new Date(dataModel.constStartDate);
            let constEndDate = new Date(dataModel.constEndDate);

            let closeoutStartDate = new Date(dataModel.closeoutStartDate);
            let closeoutEndDate = new Date(dataModel.closeoutEndDate);

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
                alloc1.TotalWorkingDays = GetTotalWorkingDaysBetween(ajaxHelperPage, alloc1.AllocationStartDate, alloc1.AllocationEndDate);
                globaldata.push(alloc1);
                alloc2.TotalWorkingDays = GetTotalWorkingDaysBetween(ajaxHelperPage, alloc2.AllocationStartDate, alloc2.AllocationEndDate);
                alloc2.ID = -Math.abs(globaldata.length + 1);
                globaldata.push(alloc2);
                alloc3.TotalWorkingDays = GetTotalWorkingDaysBetween(ajaxHelperPage, alloc3.AllocationStartDate, alloc3.AllocationEndDate);
                alloc3.ID = -Math.abs(globaldata.length + 1);
                globaldata.push(alloc3);
            }
            else {                
                if (overlapDates.precon && overlapDates.const) {
                    //debugger;
                    //alert("adding extra allocation");
                    OverlappingAllocationInPhases.push(e);
                    let alloc1 = JSON.parse(JSON.stringify(e));
                    let alloc2 = JSON.parse(JSON.stringify(e));
                    globaldata = globaldata.filter(x => x.ID != e.ID);
                    if (alloc1.PctAllocationConst != undefined && parseInt(alloc1.PctAllocationConst) > 0) {
                        alloc2.PctAllocation = alloc1.PctAllocationConst;
                    }
                    alloc1.AllocationEndDate = preconEndDate.toISOString();;
                    alloc2.AllocationStartDate = constStartDate.toISOString();;
                    alloc1.TotalWorkingDays = GetTotalWorkingDaysBetween(ajaxHelperPage, alloc1.AllocationStartDate, alloc1.AllocationEndDate);
                    globaldata.push(alloc1);
                    alloc2.TotalWorkingDays = GetTotalWorkingDaysBetween(ajaxHelperPage, alloc2.AllocationStartDate, alloc2.AllocationEndDate);
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
                    alloc1.TotalWorkingDays = GetTotalWorkingDaysBetween(ajaxHelperPage, alloc1.AllocationStartDate, alloc1.AllocationEndDate);
                    globaldata.push(alloc1);
                    alloc2.TotalWorkingDays = GetTotalWorkingDaysBetween(ajaxHelperPage, alloc2.AllocationStartDate, alloc2.AllocationEndDate);
                    alloc2.ID = -Math.abs(globaldata.length + 1);
                    globaldata.push(alloc2);
                }
                if (overlapDates.precon && !overlapDates.const && !overlapDates.closeout) {
                    if (endDate > preconEndDate) {
                        OverlappingAllocationInPhases.push(e);
                        let alloc1 = JSON.parse(JSON.stringify(e));
                        globaldata = globaldata.filter(x => x.ID != e.ID);
                        alloc1.AllocationEndDate = preconEndDate.toISOString();;
                        alloc1.TotalWorkingDays = GetTotalWorkingDaysBetween(ajaxHelperPage, alloc1.AllocationStartDate, alloc1.AllocationEndDate);
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
                        alloc1.TotalWorkingDays = GetTotalWorkingDaysBetween(ajaxHelperPage, alloc1.AllocationStartDate, alloc1.AllocationEndDate);
                        globaldata.push(alloc1);
                    }
                }
                if (!overlapDates.precon && !overlapDates.const && overlapDates.closeout) {
                    if (startDate < closeoutStartDate) {
                        OverlappingAllocationInPhases.push(e);
                        let alloc1 = JSON.parse(JSON.stringify(e));
                        globaldata = globaldata.filter(x => x.ID != e.ID);
                        alloc1.AllocationStartDate = closeoutStartDate.toISOString();;
                        alloc1.TotalWorkingDays = GetTotalWorkingDaysBetween(ajaxHelperPage, alloc1.AllocationStartDate, alloc1.AllocationEndDate);
                        globaldata.push(alloc1);
                    }
                }
            }
        });
        if (!checkDateInMultiPhase) {
            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
            dataGrid.option("dataSource", globaldata);
            dataGrid._refresh();
        }
        else {
            globaldata = tempData;
            if (!isDateInMultiPhase) {
                $("#btnFixOverlaps").hide();
            }
            else {
                $("#btnFixOverlaps").show();
            }
            if (PhaseSummaryView) {
                $("#btnDetailedSummary").show();
            }
        }
        if (saveAllocation) {
            SaveAllocations();
        }
    }
    function SaveAllocations(updatePhaseDates, phaseDates) {
        if (updatePhaseDates == undefined) {
            updatePhaseDates = false;
        }
        var isEmptyField = false;
        var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
        var Preconstartdate;
        if (typeof (changedDates.PreConStart) == "object")
            Preconstartdate = changedDates.PreConStart.toISOString();
        var Preconenddate;
        if (typeof (changedDates.PreConEnd) == "object")
            Preconenddate = changedDates.PreConEnd.toISOString();
        var constartdate;
        if (typeof (changedDates.ConstStart) == "object")
            constartdate = changedDates.ConstStart.toISOString();
        var constenddate;
        if (typeof (changedDates.ConstEnd) == "object")
            constenddate = changedDates.ConstEnd.toISOString();

        if (flag) {
            let newupdateddata = [];
            var rows = dataGrid.getVisibleRows();
            rows.forEach(function (item, index) {
                newupdateddata.push(item.data);
            });
            globaldata = [];
            globaldata = newupdateddata;
        }
        $.each(globaldata, function (i, s) {
            if (s.TypeName == '') {
                var datakey = _.findWhere(globaldata, { ID: s.ID });
                var rowIndex = dataGrid.getRowIndexByKey(datakey);
                DevExpress.ui.notify('Role is Required', "error");
                dataGrid.getRowElement(rowIndex).css("background-color", "#FFCCCB");

                isEmptyField = true;
                //DevExpress.ui.dialog.alert("Role is Required", "Error!");
                return false;
            }

            if (typeof (s.AllocationStartDate) == "object") {
                if (s.AllocationStartDate != null) {
                    s.AllocationStartDate = (s.AllocationStartDate).toISOString();
                }
                else {
                    var datakey = _.findWhere(globaldata, { ID: s.ID });
                    var rowIndex = dataGrid.getRowIndexByKey(datakey);
                    DevExpress.ui.notify('Start Date should not be blank.', "error");
                    dataGrid.getRowElement(rowIndex).css("background-color", "#FFCCCB");

                    isEmptyField = true;
                    //DevExpress.ui.dialog.alert("Start Date should not be blank.", "Error!");
                    return false;
                }
            } else if (s.AllocationStartDate == undefined || s.AllocationStartDate == '') {
                var datakey = _.findWhere(globaldata, { ID: s.ID });
                var rowIndex = dataGrid.getRowIndexByKey(datakey);
                DevExpress.ui.notify('Start Date should not be blank.', "error");
                dataGrid.getRowElement(rowIndex).css("background-color", "#FFCCCB");

                isEmptyField = true;
                //DevExpress.ui.dialog.alert("Start Date should not be blank.", "Error!");
                return false;
            }

            if (typeof (s.AllocationEndDate) == "object") {
                if (s.AllocationEndDate != null) {
                    s.AllocationEndDate = (s.AllocationEndDate).toISOString();
                }
                else {
                    var datakey = _.findWhere(globaldata, { ID: s.ID });
                    var rowIndex = dataGrid.getRowIndexByKey(datakey);
                    DevExpress.ui.notify('End Date should not be blank.', "error");
                    dataGrid.getRowElement(rowIndex).css("background-color", "#FFCCCB");
                    isEmptyField = true;
                    //DevExpress.ui.dialog.alert("End Date should not be blank.", "Error!");
                    return false;
                }
            } else if (s.AllocationEndDate == undefined || s.AllocationEndDate == '') {
                var datakey = _.findWhere(globaldata, { ID: s.ID });
                var rowIndex = dataGrid.getRowIndexByKey(datakey);
                DevExpress.ui.notify('End Date should not be blank.', "error");
                dataGrid.getRowElement(rowIndex).css("background-color", "#FFCCCB");
                isEmptyField = true;
                //DevExpress.ui.dialog.alert("End Date should not be blank.", "Error!");
                return false;
            }

            if (new Date(s.AllocationEndDate) < new Date(s.AllocationStartDate)) {
                var datakey = _.findWhere(globaldata, { ID: s.ID });
                var rowIndex = dataGrid.getRowIndexByKey(datakey);
                DevExpress.ui.notify('Start Date should be less than End Date.', "error");
                dataGrid.getRowElement(rowIndex).css("background-color", "#FFCCCB");
                isEmptyField = true;
                //DevExpress.ui.dialog.alert("Start Date should be less than End Date.", "Error!");
                return false;
            }

        });

        if (isEmptyField == true) {
            $("#loadpanel").dxLoadPanel("hide");
            isEmptyField = false;
            return false;
        }

        deletedData = deletedData.filter(x => x.ID > 0);
        deletedData.forEach(async function (item, index) {
            console.log("Deleted Method called");
            $.ajax({
                type: "post",
                url: "/api/rmmapi/DeleteCRMAllocation",
                async: true,
                data: { ID: item.ID, TicketID: item.TicketID, UserID: item.UserID, RoleName: item.TypeName, UserName: item.UserName, Tags: item.Tags },
                success: function (data) {
                    console.log("Deleted Method End");
                }
            });
        });

        deletedData = [];
        console.log("Save Method called");
        if (!$("#gridTemplateContainer").is(":visible")) {
            lastEditedRow = "";
        }
        var dataChanged = $.cookie("dataChanged");
        if (dataChanged == 1) {
            $("#loadpanel").dxLoadPanel({
                message: "Saving record...",
                visible: true,
                hideOnOutsideClick: false
            });
        }

        $.post("/api/rmmapi/UpdateBatchCRMAllocations/", {
            Allocations: globaldata, ProjectID: projectID, PreConStart: Preconstartdate, PreConEnd: Preconenddate, ConstStart: constartdate, ConstEnd: constenddate, LastEditedRow: lastEditedRow,
            IsAllocationSplitted: isAllocationSplitted, UpdateAllProjectAllocations: false, UseThreading: useThreading
        }).then(function (response) {
            console.log("Save Method End");
            debugger;
            isAllocationSplitted = false;
            $("#loadpanel").dxLoadPanel("hide");
            if (response.includes("BlankAllocation")) {
                dataGrid.option("dataSource", globaldata);
                dataGrid._refresh();
                $("#toastBlankAllocation").dxToast("show");
            }
            else if (response.includes("OverlappingAllocation")) {
                var resultvalues = response.split(":");
                var datakey = _.findWhere(globaldata, { ID: parseInt(resultvalues[1]) });
                var rowIndex = dataGrid.getRowIndexByKey(datakey);
                HighlightSummaryGridOnerror(resultvalues);
                //dataGrid.getCellElement(rowIndex, "AssignedTo").find("input").css("border", "1px solid red");  
                DevExpress.ui.dialog.alert("Overlapping allocations are not permitted. Save unsuccessful.", "Error");
                dataGrid.getRowElement(rowIndex).css("background-color", "#FFCCCB");
                return false;
                //$("#toastOverlap").dxToast("show");
            }
            else if (response.includes("AllocationOutofbounds")) {
                var resultvalues = response.split("~");
                var datakey = _.findWhere(globaldata, { ID: parseInt(resultvalues[1]) });
                var rowIndex = dataGrid.getRowIndexByKey(datakey);
                HighlightSummaryGridOnerror(resultvalues);
                DevExpress.ui.dialog.alert("Allocation date entered is either prior to start date or after the end date of the resource. <br/>Name: " + resultvalues[4] + " <br/>Start Date: " + resultvalues[2] + " End Date: " + resultvalues[3] + ". <br/>Please enter valid dates.", "Error");
                dataGrid.getRowElement(rowIndex).css("background-color", "#FFCCCB");
                return false;
                //$("#toastOverlap").dxToast("show");
            }
            else if (response.includes("DateNotValid")) {
                var resultvalues = response.split(":");
                var datakey = _.findWhere(globaldata, { ID: parseInt(resultvalues[1]) });
                var rowIndex = dataGrid.getRowIndexByKey(datakey);
                DevExpress.ui.dialog.alert("Start Date should be less than End Date. Save unsuccessful.", "Alert!");
                dataGrid.getRowElement(rowIndex).css("background-color", "#FFCCCB");
                return false;
            }
            else {
                dataGrid.option("dataSource", response);
                dataGrid._refresh();
                if (dataChanged == 1) { $("#toast").dxToast("show"); }
                globaldata = response;
                CompactPhaseConstraints();
                if (updatePhaseDates) {
                    var preconStartDate = phaseDates.PreconStartDate != '' ? new Date(phaseDates.preconStartDate) : '';
                    var preconEndDate = phaseDates.PreconEndDate != '' ? new Date(phaseDates.preconEndDate) : '';

                    var constStartDate = phaseDates.constStartDate != '' ? new Date(phaseDates.constStartDate) : '';
                    var constEndDate = phaseDates.constEndDate != '' ? new Date(phaseDates.constEndDate) : '';
                    var closeoutStartDate = phaseDates.closeoutStartDate != '' ? new Date(phaseDates.closeoutStartDate) : '';
                    var closeoutEndDate = phaseDates.closeoutEndDate != '' ? new Date(phaseDates.closeoutEndDate) : '';

                    dataModel.preconStartDate = phaseDates.PreconStartDate == '' ? '' : phaseDates.PreconStartDate;
                    dataModel.preconEndDate = phaseDates.PreconEndDate == '' ? '' : phaseDates.PreconEndDate;
                    if (preconStartDate != '' && preconEndDate != '' && preconEndDate < preconStartDate) {
                        dataModel.preconEndDate = dataModel.preconStartDate;
                    }

                    dataModel.constStartDate = phaseDates.ConstStartDate == '' ? '' : phaseDates.ConstStartDate;
                    dataModel.constEndDate = phaseDates.ConstEndDate == '' ? '' : phaseDates.ConstEndDate;
                    if (constStartDate != '' && constEndDate != '' && constEndDate < constStartDate) {
                        dataModel.constEndDate = dataModel.constStartDate;
                    }

                    dataModel.closeoutStartDate = phaseDates.CloseOutStartDate == '' ? '' : phaseDates.CloseOutStartDate;
                    dataModel.closeoutEndDate = phaseDates.CloseOutEndDate == '' ? '' : phaseDates.CloseOutEndDate;
                    if (closeoutStartDate != '' && closeoutEndDate != '' && closeoutEndDate < closeoutStartDate) {
                        dataModel.closeoutEndDate = dataModel.closeoutStartDate;
                    }
                    CompactPhaseConstraints();
                    UpdateRecord();
                }
                //setTimeout(function () { location.reload(); }, 3000);
                //$.cookie("dataChanged", 0, { path: "/" });
            }
            $('#btnCancelChanges').dxButton({ visible: false });
            $.cookie("dataChanged", 0, { path: "/" });
            // $.cookie("projTeamAllocSaved", 1, { path: "/" });
        }, function (error) { $("#loadpanel").dxLoadPanel("hide"); });

    }

    function HighlightSummaryGridOnerror(resultvalues) {
        if (!$("#gridTemplateContainer").is(":visible")) {
            var compactDataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
            var datakey = _.findWhere(compactTempData, { ID: parseInt(resultvalues[1]) });
            if (datakey == null || datakey == '') {
                datakey = _.findWhere(compactTempData, { ConstId: parseInt(resultvalues[1]) });
            }
            if (datakey == null || datakey == '') {
                datakey = _.findWhere(compactTempData, { CloseOutId: parseInt(resultvalues[1]) });
            }
            var rowIndex = compactDataGrid.getRowIndexByKey(datakey);
            compactDataGrid.getRowElement(rowIndex).css("background-color", "#FFCCCB");
        }
    }

    function openDateConfirmationDialog() {
        if (!EnforcePhaseConstraints) {
            return;
        }
        if (ScheduleDateOverLap) {
            return;
        }

        var dataGridContainer = $("#gridTemplateContainer").dxDataGrid("instance");
        var gridData = dataGridContainer.getVisibleRows();
        var rows = [];

        $.each(gridData, function (index, e) {
            if (e.isEditing) {
                e.data.Type = globaldata.filter(x => x.ID == e.data.ID)[0].Type;
                e.data.TypeName = globaldata.filter(x => x.ID == e.data.ID)[0].TypeName;
                e.data.AssignedTo = globaldata.filter(x => x.ID == e.data.ID)[0].AssignedTo;
                rows.push(e.data);
            }
        });
        let startDate = rows.length > 0 ? new Date(rows[0].AllocationStartDate) : "";
        let endDate = rows.length > 0 ? new Date(rows[0].AllocationEndDate) : "";
        if (!isDateValid(startDate) || !isDateValid(endDate)) {
            return;
        }
        let overlapDates = {
            precon: false,
            const: false,
            closeout: false
        };
        let openDialog = false;

        let preconStartDate = new Date(dataModel.preconStartDate);
        let preconEndDate = new Date(dataModel.preconEndDate);

        let constStartDate = new Date(dataModel.constStartDate);
        let constEndDate = new Date(dataModel.constEndDate);

        let closeoutStartDate = new Date(dataModel.closeoutStartDate);
        let closeoutEndDate = new Date(dataModel.closeoutEndDate);

        if (!isDateValid(preconStartDate) && startDate < constStartDate) {
            preconStartDate = startDate;
        }
        if (!isDateValid(preconEndDate) && isDateValid(constStartDate)) {
            preconEndDate = constStartDate.addDays(-1);
        }
        if (!isDateValid(closeoutEndDate) && endDate > constEndDate) {
            closeoutEndDate = endDate;
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
        if (overlapDates.precon && overlapDates.const && overlapDates.closeout) {
            openDialog = true;
            let alloc1 = JSON.parse(JSON.stringify(rows[0]));
            let alloc2 = JSON.parse(JSON.stringify(rows[0]));
            let alloc3 = JSON.parse(JSON.stringify(rows[0]));
            rows.pop();
            alloc1.AllocationEndDate = preconEndDate.toISOString();;
            alloc2.AllocationStartDate = constStartDate.toISOString();;
            alloc2.AllocationEndDate = constEndDate.toISOString();;
            alloc3.AllocationStartDate = closeoutStartDate.toISOString();;
            alloc2.ID = -Math.abs(globaldata.length + 1);
            alloc3.ID = -Math.abs(globaldata.length + 1) - 1;
            rows.push(alloc1);
            rows.push(alloc2);
            rows.push(alloc3);
        }
        else {
            if (overlapDates.precon && overlapDates.const) {
                openDialog = true;
                let alloc1 = JSON.parse(JSON.stringify(rows[0]));
                let alloc2 = JSON.parse(JSON.stringify(rows[0]));
                rows.pop();
                alloc1.AllocationEndDate = preconEndDate.toISOString();;
                alloc2.AllocationStartDate = constStartDate.toISOString();;
                alloc2.ID = -Math.abs(globaldata.length + 1);
                rows.push(alloc1);
                rows.push(alloc2);
            }
            if (overlapDates.const && overlapDates.closeout) {
                openDialog = true;
                let alloc1 = JSON.parse(JSON.stringify(rows[0]));
                let alloc2 = JSON.parse(JSON.stringify(rows[0]));
                rows.pop();

                alloc1.AllocationStartDate = startDate < constStartDate
                    ? constStartDate.toISOString() : alloc1.AllocationStartDate;

                alloc1.AllocationEndDate = constEndDate.toISOString();;
                alloc2.AllocationStartDate = closeoutStartDate.toISOString();;
                alloc2.ID = -Math.abs(globaldata.length + 1);
                rows.push(alloc1);
                rows.push(alloc2);
            }
        }
        if (openDialog) {
            const popupContentTemplate1 = function () {
                let container = $("<div>");
                let datagrid = $("<div id='confirmationGrid'>").dxDataGrid({
                    dataSource: rows,
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
                        },
                        {
                            dataField: "TypeName",
                            dataType: "text",
                            allowEditing: false,
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
                            width: "15%",
                            alignment: 'center',
                            cssClass: "v-align",
                            allowEditing: false,
                            //validationRules: [{ type: "required", message: '', }],
                            format: 'MMM d, yyyy',
                            sortIndex: "2",
                            sortOrder: "asc",
                            editorOptions: {
                                onFocusOut: function (e, options) {
                                    var dataGrid = $("#confirmationGrid").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                },
                                onClosed: function (e) {
                                    var dataGrid = $("#confirmationGrid").dxDataGrid("instance");
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
                            width: "15%",
                            allowEditing: false,
                            //validationRules: [{ type: "required", message: '', }],
                            format: 'MMM d, yyyy',
                            editorOptions: {
                                onFocusOut: function (e) {
                                    var dataGrid = $("#confirmationGrid").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                },
                                onClosed: function (e) {
                                    var dataGrid = $("#confirmationGrid").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                },
                            }
                        },
                        {
                            dataField: "PctAllocation",
                            caption: "% Alloc",
                            dataType: "text",
                            width: "10%",
                            allowEditing: false,
                        },
                    ],
                    showBorders: true,
                    showRowLines: true,
                    onCellPrepared: function (e) {
                        if (e.rowType === 'data') {
                            var preconstartDate = new Date(dataModel.preconStartDate);
                            var preconEndDate = new Date(dataModel.preconEndDate);

                            var conststartDate = new Date(dataModel.constStartDate);
                            var constEndDate = new Date(dataModel.constEndDate);

                            var closeoutstartDate = new Date(dataModel.closeoutStartDate);
                            var closeoutEndDate = new Date(dataModel.closeoutEndDate);

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
                let confirmBtn = $(`<div class="mt-4 btnAddNew" style='float:right;padding: 0px 10px;font-size: 14px;' />`).dxButton({
                    text: "Accept Changes",
                    hint: 'Accept Changes',
                    visible: true,
                    onClick: function (e) {

                        let dataGridChild = $("#confirmationGrid").dxDataGrid("instance");
                        let rowschild = dataGridChild.getVisibleRows();
                        let tempData = globaldata.filter(e => e.ID != rowschild[0].data.ID);
                        $.each(rowschild, function (index, e) {
                            if (parseInt(e.data.PctAllocation) > 0) {
                                e.data.TotalWorkingDays = GetTotalWorkingDaysBetween(ajaxHelperPage, e.data.AllocationStartDate, e.data.AllocationEndDate);
                                tempData.push(e.data);
                            }
                        });
                        globaldata = tempData;
                        dataGridContainer.option("dataSource", tempData);
                        //alert("step2");
                        CompactPhaseConstraints();
                        CheckPhaseConstraints(true);
                        popup.hide();
                    }
                })
                let cancelBtn = $(`<div class="mt-4 btnAddNew" style='float:right;padding: 0px 10px;font-size: 14px;' />`).dxButton({
                    text: "Cancel",
                    visible: true,
                    onClick: function (e) {
                        $("#btnFixOverlaps").show();
                        //$("#btnDetailedSummary").hide();
                        popup.hide();
                    }
                })
                container.append(datagrid);
                container.append(confirmBtn);
                container.append(cancelBtn);
                return container;
            };

            const popup = $("#confirmationDialog").dxPopup({
                contentTemplate: popupContentTemplate1,
                width: "900",
                height: "300",
                showTitle: true,
                title: "Do you want to breakup this allocation by phase?",
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
    function OpenSplitAllocationConfirmationDialog() {
        const popupSplitAllocation = function () {
            let container = $("<div>");
            let datagrid = $("<div id='splitConfirmationGrid'>").dxDataGrid({
                dataSource: OverlappingAllocationInPhases,
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
                    },
                    {
                        dataField: "TypeName",
                        dataType: "text",
                        allowEditing: false,
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
                        width: "15%",
                        alignment: 'center',
                        cssClass: "v-align",
                        allowEditing: false,
                        //validationRules: [{ type: "required", message: '', }],
                        format: 'MMM d, yyyy',
                        sortIndex: "2",
                        sortOrder: "asc",
                        editorOptions: {
                            onFocusOut: function (e, options) {
                                var dataGrid = $("#splitConfirmationGrid").dxDataGrid("instance");
                                dataGrid.saveEditData();
                            },
                            onClosed: function (e) {
                                var dataGrid = $("#splitConfirmationGrid").dxDataGrid("instance");
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
                        width: "15%",
                        allowEditing: false,
                        //validationRules: [{ type: "required", message: '', }],
                        format: 'MMM d, yyyy',
                        editorOptions: {
                            onFocusOut: function (e) {
                                var dataGrid = $("#splitConfirmationGrid").dxDataGrid("instance");
                                dataGrid.saveEditData();
                            },
                            onClosed: function (e) {
                                var dataGrid = $("#splitConfirmationGrid").dxDataGrid("instance");
                                dataGrid.saveEditData();
                            },
                        }
                    },
                    {
                        dataField: "PctAllocation",
                        caption: "% Alloc",
                        dataType: "text",
                        width: "10%"
                    },
                ],
                showBorders: true,
                showRowLines: true,
                onCellPrepared: function (e) {
                    if (e.rowType === 'data') {
                        var preconstartDate = new Date(dataModel.preconStartDate);
                        var preconEndDate = new Date(dataModel.preconEndDate);

                        var conststartDate = new Date(dataModel.constStartDate);
                        var constEndDate = new Date(dataModel.constEndDate);

                        var closeoutstartDate = new Date(dataModel.closeoutStartDate);
                        var closeoutEndDate = new Date(dataModel.closeoutEndDate);

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
            let splitNote = $(`<div class='mt-2' style='font-weight:500;'>Note: If Yes chosen, split to phases will be executed and saved for all highlighted lines. Once split, cannot be undone.</div>`);
            let confirmBtn = $(`<div class="mt-4 btnAddNew" style='float:right;padding: 0px 10px;font-size: 14px;' />`).dxButton({
                text: "Yes",
                hint: 'Accept Changes',
                visible: true,
                onClick: function (e) {
                    flag = false;
                    isAllocationSplitted = true;
                    CheckPhaseConstraints(false, true);
                    //CompactPhaseConstraints();
                    $("#btnFixOverlaps").hide();
                    if (PhaseSummaryView) {
                        $("#btnDetailedSummary").show();
                    }
                    confirmationPopup.hide();
                }
            })
            let cancelBtn = $(`<div class="mt-4 btnAddNew" style='float:right;padding: 0px 10px;font-size: 14px;' />`).dxButton({
                text: "No",
                visible: true,
                onClick: function (e) {
                    confirmationPopup.hide();
                }
            })
            container.append(datagrid);
            container.append(splitNote);
            container.append(confirmBtn);
            container.append(cancelBtn);
            return container;
        };

        const confirmationPopup = $("#confirmationDialog").dxPopup({
            contentTemplate: popupSplitAllocation,
            width: "900",
            height: "auto",
            showTitle: true,
            title: "Do you want to split allocations by phases?",
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

        confirmationPopup.option({
            contentTemplate: () => popupSplitAllocation()

        });
        confirmationPopup.show();
    }
    function bindDatapopup(popupFilters) {
        if (typeof (popupFilters.allocationStartDate) == "object") {
            if (popupFilters.allocationStartDate != null)
                popupFilters.allocationStartDate = new Date(popupFilters.allocationStartDate).format('MM/dd/yyyy');
        }

        if (typeof (popupFilters.allocationEndDate) == "object") {
            if (popupFilters.allocationEndDate != null)
                popupFilters.allocationEndDate = new Date(popupFilters.allocationEndDate).format('MM/dd/yyyy');
        }
        UserConflictData = [];
        $.post("/api/rmmapi/FindResourceBasedOnGroupNew/", popupFilters).then(function (response) {
            if ($("#tileViewContainer").length > 0) {
                var titleViewObj = $('#tileViewContainer').dxTileView('instance');
                if (titleViewObj) {
                    titleViewObj.option("dataSource", response);
                    titleViewObj._refresh();
                }
                $("#popuploader").dxLoadPanel({
                    message: "Loading...",
                   visible: false
              });
            }
        });
    };
    //Popup message changes added parameter isStartEndDateLesserThanCurrDate
    function AllocateResourceToAllocation(data, allocationID = null, isStartEndDateLesserThanCurrDate, isSummary) {
        //debugger;
        if (allocationID == null) {
            if (showTimeSheet == true) {
                showTimeSheet = false;
                return;
            }
        }
        let obj = null;
        let element = null;
        let compactElement = null;
        let newIds = null;
        //debugger;
        //Popup message changes added based on parameter
        if (isStartEndDateLesserThanCurrDate == undefined || isStartEndDateLesserThanCurrDate == false) {
            newIds = CheckAndUpdatePastAllocations(allocationID);
        }
        let tempData = JSON.parse(JSON.stringify(globaldata));
        if ($("#gridTemplateContainer").is(":visible") || tempRefIds.length > 0) {
            element = _.findWhere(globaldata, { ID: allocationID == null ? parseInt(popupFilters.ID) : parseInt(allocationID) });
            obj = globaldata.find(o => o.AssignedTo == data.AssignedTo && o.Type == element.Type && element.AllocationStartDate <= o.AllocationEndDate && element.AllocationEndDate >= o.AllocationStartDate && o.ID > 0);
        }
        else {
            compactElement = _.findWhere(compactTempData, { ID: allocationID == null ? parseInt(popupFilters.ID) : parseInt(allocationID) });
            obj = globaldata.find(o => o.AssignedTo == data.AssignedTo && o.Type == compactElement.Type && compactElement.AllocationStartDate <= o.AllocationEndDate && compactElement.AllocationEndDate >= o.AllocationStartDate && o.ID > 0);
        }

        if (obj != null && obj != undefined) {
            var customDialog = DevExpress.ui.dialog.custom({
                title: "Alert",
                message: allocationID == null ?
                    "An overlapping allocation exists for this resource and role for this project. Do you want to proceed?"
                    : obj.AssignedToName + " has an overlapping allocation for this project. Do you want to proceed?",
                buttons: [
                    { text: "Ok", onClick: function () { return "Ok" } },
                    { text: "Cancel", onClick: function () { return "Cancel" } }
                ]
            });
            customDialog.show().done(function (dialogResult) {
                if (dialogResult == "Ok") {
                    $.cookie("dataChanged", 1, { path: "/" });
                    $('#btnCancelChanges').dxButton({ visible: true });
                    if ($("#gridTemplateContainer").is(":visible") || tempRefIds.length > 0) {
                        if (newIds?.length > 0 && globaldata.find(x => newIds.includes(String(x.ID))) != null) {
                            element = globaldata.find(x => newIds.includes(String(x.ID)));
                        } 
                        element.UserImageUrl = data.UserImagePath;
                        element.AssignedTo = data.AssignedTo;
                        element.AssignedToName = data.AssignedToName;
                        element.IsResourceDisabled = false;
                        element.Tags = popupFilters.SelectedTags?.map(x => x.TagId).join();
                        if (tempRefIds.length > 0 && tempRefIds.find(x => String(x) == String(element.ID)) == null) {
                            tempRefIds.push(String(element.ID));
                        }
                    }
                    else {
                        let ids = GetUniqueIds(compactElement);
                        if (newIds?.length > 0) {
                            ids = newIds;
                        }
                        globaldata.forEach(function (e) {
                            if (ids.includes(String(e.ID)) && new Date(e.AllocationEndDate) > new Date()) {
                                e.AssignedTo = data.AssignedTo;
                                e.UserImageUrl = data.UserImagePath;
                                e.AssignedToName = data.AssignedToName;
                                e.IsResourceDisabled = false;
                                if (pctValueChanged) {
                                    e.PctAllocation = popupFilters.pctAllocation;
                                }
                            }
                        });
                        //alert("step3");
                        CompactPhaseConstraints();
                    }
                    AllocateResource();
                }
                else if (dialogResult == "Cancel") {
                    globaldata = JSON.parse(JSON.stringify(tempData));
                    checkAndProcessResource();
                    return false;
                }
                else {
                    globaldata = JSON.parse(JSON.stringify(tempData));
                    checkAndProcessResource();
                    return false;
                }
            });

            return false;
        }
        else {
            //debugger;
            //don't need to change Assignee when role is change, user is playing selected role in current project.
            //element.Type = data.GroupID;
            //var roledata = _.findWhere(GroupsData, { Id: data.GroupID });
            //if(typeof roledata.Name !== "undefined")
            //    element.TypeName = roledata.Name;

            //var grid = $('#gridTemplateContainer').dxDataGrid('instance');
            //grid.option('dataSource', globaldata);
            //globaldata = [];
            //grid._refresh();

            //$('#popupContainer').dxPopup('instance').hide();
            //Popup message changes added
            compactTempCopy = compactTempData;
            //Popup message changes added for Detailed View
            if (isStartEndDateLesserThanCurrDate != undefined && isStartEndDateLesserThanCurrDate == true) {
                //debugger;
                $.cookie("dataChanged", 1, { path: "/" });
                $('#btnCancelChanges').dxButton({ visible: true });
                let alloc = null;
                alloc = globaldata.find(x => x.ID == oldAssignToID);
                alloc.AssignedTo = data.AssignedTo;
                alloc.AssignedToName = data.AssignedToName;
                alloc.UserImageUrl = data.UserImagePath;
                alloc.IsResourceDisabled = false;
                alloc.AllocationEndDate = oldEndDate;
                var compactgrid = $('#gridTemplateContainer').dxDataGrid('instance');
                compactgrid.option('dataSource', globaldata);
                compactgrid._refresh();
                $('#popupContainer').dxPopup('instance').hide();
                //alert("step4");
                CompactPhaseConstraints();
            }
            else {
                $.cookie("dataChanged", 1, { path: "/" });
                $('#btnCancelChanges').dxButton({ visible: true });
                if ($("#gridTemplateContainer").is(":visible") || tempRefIds.length > 0) {
                    if (newIds?.length > 0) {
                        element = globaldata.find(x => newIds.includes(String(x.ID)));
                    }
                    element.AssignedTo = data.AssignedTo;
                    element.AssignedToName = data.AssignedToName;
                    element.UserImageUrl = data.UserImagePath;
                    element.IsResourceDisabled = false;
                    element.Tags = popupFilters.SelectedTags?.map(x => x.TagId).join();
                    if (isStartEndDateLesserThanCurrDate != undefined && isStartEndDateLesserThanCurrDate == true) {
                        element.AllocationStartDate = oldStartDate;
                    }
                    if (tempRefIds.length > 0 && tempRefIds.find(x => String(x) == String(element.ID)) == null) {
                        tempRefIds.push(String(element.ID));
                    }
                }
                else {
                    let ids = GetUniqueIds(compactElement);
                    if (newIds?.length > 0) {
                        ids = newIds;
                    }
                    globaldata.forEach(function (e) {
                        if (ids.includes(String(e.ID)) && new Date(e.AllocationEndDate) > new Date()) {
                            e.AssignedTo = data.AssignedTo;
                            e.AssignedToName = data.AssignedToName;
                            e.UserImageUrl = data.UserImagePath;
                            e.IsResourceDisabled = false;
                            if (pctValueChanged) {
                                e.PctAllocation = popupFilters.pctAllocation;
                            }
                        }
                    });
                    //alert("step5");
                    CompactPhaseConstraints();
                }
                //Popup message changes added parameters
                AllocateResource(newIds, data, isSummary);
            }
        }
    }
    //Popup message changes added parameters
    function AllocateResource(eID, data, isSummary) {
        let element = null;
        let CompactElement = null;
        //Popup message changes added for Summary View
        if (isSummary == true && (eID != undefined && data != undefined && oldAssignTo == "") && Date(oldStartDate) < curDate) {
            let preCompactAlloc = compactTempData.find(x => x.ID == oldAssignToID);
            let preIds = GetUniqueIds(preCompactAlloc);
            let preAllocCheck = globaldata.filter(x => preIds.includes(String(x.ID)));
            let preGlobalData = null;
            preAllocCheck.forEach(function (e) {
                if (new Date(e.AllocationStartDate) < new Date() && new Date(e.AllocationEndDate) < new Date()) {
                    preGlobalData = preAllocCheck.filter(x => x.ID != e.ID);
                }
            });
            let MinDates = new Date(Math.min.apply(null, preGlobalData.map(x => new Date(x.AllocationStartDate))));
            if (MinDates > curDate) {
                curDate = MinDates;
            }
            var result1 = DevExpress.ui.dialog.custom({
                title: "Warning!",
                message: "RM One will adjust the start date of this allocation to Current Date",
                buttons: [
                    { text: "Retain " + new Date(oldStartDate).toLocaleDateString("en-US") + " as Start Date", onClick: function () { return false }, elementAttr: { "class": "btnBlue" } },
                    { text: "Change Date to " + new Date(curDate).toLocaleDateString("en-US"), onClick: function () { return true }, elementAttr: { "class": "btnBlue" } }
                ]
            });
            result1.show().done(function (dialogResult) {
                //debugger;
                isAlertActive = true;
                let alloc = null;
                let CompactAlloc = compactTempCopy.find(x => x.ID == oldAssignToID);
                let ids = GetUniqueIds(CompactAlloc);
                alloc = globaldata.filter(x => ids.includes(String(x.ID)));
                let roleData = GroupsData.filter(x => x.Id == data.GlobalRoleId);
                if (dialogResult) {
                    $.cookie("dataChanged", 1, { path: "/" });
                    $('#btnCancelChanges').dxButton({ visible: true });
                    alloc.forEach(function (e) {
                        //debugger;
                        if (ids.includes(String(e.ID)) && new Date(e.AllocationStartDate) < new Date() && new Date(e.AllocationEndDate) >= new Date()) {
                            e.AssignedTo = data.AssignedTo;
                            e.AssignedToName = data.AssignedToName;
                            if (roleData != null && roleData.length > 0) {
                                e.Type = roleData[0].Id;
                                e.TypeName = roleData[0].Name;
                            }
                            e.UserImageUrl = data.UserImagePath != undefined ? data.UserImagePath : data.UserImageUrl;
                            e.IsResourceDisabled = false;
                            e.AllocationStartDate = curDate;
                        }
                        else if (ids.includes(String(e.ID)) && new Date(e.AllocationStartDate) >= new Date() && new Date(e.AllocationEndDate) >= new Date()) {
                            e.AssignedTo = data.AssignedTo;
                            e.AssignedToName = data.AssignedToName;
                            if (roleData != null && roleData.length > 0) {
                                e.Type = roleData[0].Id;
                                e.TypeName = roleData[0].Name;
                            }
                            e.UserImageUrl = data.UserImagePath != undefined ? data.UserImagePath : data.UserImageUrl;
                            e.IsResourceDisabled = false;
                        }
                        else if (new Date(e.AllocationStartDate) < new Date() && new Date(e.AllocationEndDate) < new Date()) {
                            globaldata = globaldata.filter(x => x.ID != e.ID);
                        }
                    });

                    if (CompactAlloc != null) {
                        if (roleData != null && roleData.length > 0) {
                            CompactAlloc.Type = roleData[0].Id;
                            CompactAlloc.TypeName = roleData[0].Name;
                        }
                        CompactAlloc.AssignedTo = data.AssignedTo;
                        CompactAlloc.AssignedToName = data.AssignedToName;
                        CompactAlloc.UserImageUrl = data.UserImagePath != undefined ? data.UserImagePath : data.UserImageUrl;
                        CompactAlloc.IsResourceDisabled = false;
                        CompactAlloc.AllocationStartDate = curDate;
                    }
                    var compactgrid = $('#compactGridTemplateContainer').dxDataGrid('instance');
                    compactgrid.option('dataSource', compactTempCopy);
                    compactgrid._refresh();
                    var grid = $('#gridTemplateContainer').dxDataGrid('instance');
                    grid.option('dataSource', globaldata);
                    globaldata = [];
                    grid._refresh();

                    $('#popupContainer').dxPopup('instance').hide();
                }
                else if (!dialogResult) {
                    $.cookie("dataChanged", 1, { path: "/" });
                    $('#btnCancelChanges').dxButton({ visible: true });
                    alloc.forEach(function (e) {
                        //debugger;
                        if (ids.includes(String(e.ID))) {
                            //debugger;
                            e.AssignedTo = data.AssignedTo;
                            e.AssignedToName = data.AssignedToName;
                            if (roleData != null && roleData.length > 0) {
                                e.Type = roleData[0].Id;
                                e.TypeName = roleData[0].Name;
                            }
                            e.UserImageUrl = data.UserImagePath != undefined ? data.UserImagePath : data.UserImageUrl;
                            e.IsResourceDisabled = false;
                        }
                    });

                    if (CompactAlloc != null) {
                        if (roleData != null && roleData.length > 0) {
                            CompactAlloc.Type = roleData[0].Id;
                            CompactAlloc.TypeName = roleData[0].Name;
                        }
                        CompactAlloc.AssignedTo = data.AssignedTo;
                        CompactAlloc.AssignedToName = data.AssignedToName;
                        CompactAlloc.UserImageUrl = data.UserImagePath != undefined ? data.UserImagePath : data.UserImageUrl;
                        CompactAlloc.IsResourceDisabled = false;
                        CompactAlloc.AllocationStartDate = oldStartDate;
                    }
                    var compactgrid = $('#compactGridTemplateContainer').dxDataGrid('instance');
                    compactgrid.option('dataSource', compactTempCopy);
                    compactgrid._refresh();
                    var grid = $('#gridTemplateContainer').dxDataGrid('instance');
                    grid.option('dataSource', globaldata);
                    globaldata = [];
                    grid._refresh();

                    $('#popupContainer').dxPopup('instance').hide();
                }
            });
        }
        else if ((eID != undefined && data != undefined && oldAssignTo == "") && Date(oldStartDate) < curDate ) {
            //Popup message changes added for Detailed View
            var result1 = DevExpress.ui.dialog.custom({
                title: "Warning!",
                message: "RM One will adjust the start date of this allocation to Current Date",
                buttons: [
                    { text: "Retain " + new Date(oldStartDate).toLocaleDateString("en-US") + " as Start Date", onClick: function () { return false }, elementAttr: { "class": "btnBlue" } },
                    { text: "Change Date to " + new Date(curDate).toLocaleDateString("en-US"), onClick: function () { return true }, elementAttr: { "class": "btnBlue" } }
                ]
            });
            result1.show().done(function (dialogResult) {
                //debugger;
                isAlertActive = false;
                if (eID?.length > 0) {
                    element = globaldata.find(x => eID.includes(String(x.ID)));
                }

                element.AssignedTo = data.AssignedTo;
                element.AssignedToName = data.AssignedToName;
                element.UserImageUrl = data.UserImagePath != undefined ? data.UserImagePath : data.UserImageUrl;
                element.IsResourceDisabled = false;

                if (!dialogResult) {
                    //debugger;
                    $.cookie("dataChanged", 1, { path: "/" });
                    $('#btnCancelChanges').dxButton({ visible: true });
                    element.AllocationStartDate = oldStartDate;
                    var grid = $('#gridTemplateContainer').dxDataGrid('instance');
                    grid.option('dataSource', globaldata);
                    globaldata = [];
                    grid._refresh();

                    var compactgrid = $('#compactGridTemplateContainer').dxDataGrid('instance');
                    compactgrid.option('dataSource', compactTempData);
                    //console.log(compactTempData);

                    compactgrid._refresh();

                    $('#popupContainer').dxPopup('instance').hide();
                }
                else if (dialogResult) {
                    $.cookie("dataChanged", 1, { path: "/" });
                    $('#btnCancelChanges').dxButton({ visible: true });
                    element.AllocationStartDate = curDate;

                    //console.log("3");
                    //console.log(globaldata);
                    var grid = $('#gridTemplateContainer').dxDataGrid('instance');
                    grid.option('dataSource', globaldata);
                    globaldata = [];
                    grid._refresh();

                    var compactgrid = $('#compactGridTemplateContainer').dxDataGrid('instance');
                    compactgrid.option('dataSource', compactTempData);

                    compactgrid._refresh();

                    $('#popupContainer').dxPopup('instance').hide();
                }
            });
        }
        else {
            isAlertActive = false;
            if (tempRefIds.length > 0 && $('#InternalAllocationGrid').dxDataGrid('instance') != undefined) {
                $('#InternalAllocationGrid').dxDataGrid('instance').option('dataSource', globaldata.filter(x => tempRefIds.includes(String(x.ID))));
                //alert("step6");
                CompactPhaseConstraints();
            }
            var grid = $('#gridTemplateContainer').dxDataGrid('instance');
            grid.option('dataSource', globaldata);
            globaldata = [];
            grid._refresh();

            var compactgrid = $('#compactGridTemplateContainer').dxDataGrid('instance');
            compactgrid.option('dataSource', compactTempData);
            //console.log(compactTempData);

            compactgrid._refresh();

            var popupInstance = $('#popupContainer').dxPopup('instance');
            var title = popupInstance.option('title');
            if (!(title != undefined && title != null && title == "Find Teams"))
                $('#popupContainer').dxPopup('instance').hide();

            checkAndProcessResource();
        }
    }

    //Popup message changes added parameter
    function SetDatesOfSummaryView(dataid, checkOnGlobalData, isBothDatesLesserThanCurrent) {
        //debugger;
        if (checkOnGlobalData == undefined) {
            checkOnGlobalData = false
        }
        popupFilters.IsRequestFromSummaryView = false;
        if (!$("#gridTemplateContainer").is(":visible") && compactTempData != null && compactTempData.length > 0 && !checkOnGlobalData) {
            var cdata = _.find(compactTempData, function (s) { return s.ID.toString() === dataid.toString(); });
            let ids = GetUniqueIds(cdata);
            popupFilters.Allocations = [];
            globaldata.forEach(function (e) {
                if (!(new Date(e.AllocationStartDate) < new Date() && new Date(e.AllocationEndDate) < new Date()) && ids.includes(String(e.ID))) {
                    {
                        popupFilters.Allocations.push({ StartDate: new Date(e.AllocationStartDate) < new Date() ? new Date().format('yyyy-MM-dd') + "T00:00:00" : e.AllocationStartDate, EndDate: e.AllocationEndDate, RequiredPctAllocation: parseFloat(e.PctAllocation), ID: e.ID });
                    }
                }
                else if (isBothDatesLesserThanCurrent && (new Date(e.AllocationStartDate) < new Date() && new Date(e.AllocationEndDate) < new Date()) && ids.includes(String(e.ID))) {
                    popupFilters.Allocations.push({ StartDate: new Date(e.AllocationStartDate) < new Date() ? new Date().format('yyyy-MM-dd') + "T00:00:00" : e.AllocationStartDate, EndDate: e.AllocationEndDate, RequiredPctAllocation: parseFloat(e.PctAllocation), ID: e.ID });
                }
            });
            popupFilters.IsRequestFromSummaryView = true;
        }        
        else {
            popupFilters.Allocations = [];
            //Popup message changes added
            if (isBothDatesLesserThanCurrent) {
                globaldata.forEach(function (e) {
                    if ((new Date(e.AllocationStartDate) < new Date() && new Date(e.AllocationEndDate) < new Date()) && dataid.toString() == e.ID.toString()) {
                        //debugger;
                        popupFilters.Allocations.push({ StartDate: new Date(e.AllocationStartDate) < new Date() ? new Date().format('yyyy-MM-dd') + "T00:00:00" : e.AllocationStartDate, EndDate: e.AllocationEndDate, RequiredPctAllocation: parseFloat(e.PctAllocation), ID: e.ID });
                    }
                });
            }
            else {
                globaldata.forEach(function (e) {
                    if (!(new Date(e.AllocationStartDate) < new Date() && new Date(e.AllocationEndDate) < new Date()) && dataid.toString() == e.ID.toString()) {
                        //debugger;
                        popupFilters.Allocations.push({ StartDate: new Date(e.AllocationStartDate) < new Date() ? new Date().format('yyyy-MM-dd') + "T00:00:00" : e.AllocationStartDate, EndDate: e.AllocationEndDate, RequiredPctAllocation: parseFloat(e.PctAllocation), ID: e.ID });
                    }
                });
            }
        }
    }
    let oldAssignTo = null;
    let oldAssignToID = null;
    let oldStartDate = null;
    let oldEndDate = null;
    var isStartEndDateLesserThanCurrDate = false;
    let curDate = new Date();
    let compactTempCopy = null;
    let selectedID = null;

    $(document).on("click", "img.assigneeToImg", function (e) {
        //Popup message changes added assign parameters value to variables
        oldAssignTo = $(this).attr("assignedTo");
        oldAssignToID = $(this).attr("ID");
        oldStartDate = $(this).attr("startDate");
        oldEndDate = $(this).attr("endDate");
        selectedID = e.target.id;
        debugger;
        //Popup message changes added for "Are you sure you want to allocate to a past allocation?"
        if ($(this).attr("startDate") != undefined && $(this).attr("endDate") != undefined &&  Date($(this).attr("startDate")) < curDate && oldAssignTo != "") {
            //debugger;
            var result = DevExpress.ui.dialog.custom({
                title: "Warning!",
                message: "Are you sure you want to allocate to a past allocation?",
                buttons: [
                    { text: "Yes", onClick: function () { return true }, elementAttr: { "class": "btnBlue" } },
                    { text: "No", onClick: function () { return false }, elementAttr: { "class": "btnNormal" } }
                ]
            });
            result.show().done(function (dialogResult) {
                if (dialogResult) {
                    $.cookie("dataChanged", 1, { path: "/" });
                    $('#btnCancelChanges').dxButton({ visible: true });
                    isStartEndDateLesserThanCurrDate = true;
                    SetDatesOfSummaryView(e.target.id, undefined, true);
                    if (popupFilters.Allocations?.length > 0) {
                        var groupid = $(this).attr("id");
                        var dataid = e.target.id;
                        var data = _.find(globaldata, function (s) { return s.ID.toString() === dataid; });

                        pctValueChanged = false;
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
                        popupFilters.isAllocationView = 1;
                        if (projectID.startsWith("OPM")) {
                            popupFilters.ModuleIncludes = false;
                        }
                        else {
                            popupFilters.ModuleIncludes = false;
                        }
                        popupFilters.JobTitles = "";
                        popupFilters.departments = "";
                        popupFilters.DivisionId = "";
                        popupFilters.SelectedUserID = "";
                        debugger;
                        popupFilters.ID = $(this).attr("ID");
                        RowId = $(this).attr("ID");
                        var popupTitle = "Available Resource";
                        if (data)
                            //popupTitle = "Allocated " + data.TypeName + "s";
                            popupTitle = "Allocate " + data.TypeName;
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
                                let preconstartDate = new Date(dataModel.preconStartDate);

                                let conststartDate = new Date(dataModel.constStartDate);
                                let constEndDate = new Date(dataModel.constEndDate);

                                let closeoutstartDate = new Date(dataModel.closeoutStartDate);
                                let windowHeight = parseInt(60 * $(window).height() / 100);
                                let ganttInfoSDate = null;
                                let ganttInfoEDate = null;
                                if ($(window).height() < 700) {
                                    windowHeight = parseInt(45 * $(window).height() / 100);
                                }

                                let classNameSt = null;
                                let classNameEd = null;
                                let ganttProjSD = null;
                                let ganttProjED = null;
                                let ganttProjReqAloc = null;
                                //popupFilters.SelectedTags = JSON.parse(JSON.stringify(projectExperiencTags));
                                contentElement.append(
                                    $("<div id='popuploader' />").dxLoadPanel({
                                        message: "Loading...",
                                        visible: true
                                    }),
                                    $("<div class='d-flex row px-3 shadow-effect mb-3' />").append(
                                        //$("<div class='col-md-10 d-flex'>").append(
                                        //$("<div class='pr-2 pt-4'>").append(
                                        //    $(`<div class="mr-3 precon_Btn_White_Box"/>`).dxButton({
                                        //        text: "Precon",
                                        //        hint: 'Use Precon Dates',
                                        //        visible: true,
                                        //        disabled: !$("#gridTemplateContainer").is(":visible"),
                                        //        onClick: function (e) {
                                        //            var result = DevExpress.ui.dialog.confirm('Are you sure you want to switch to Precon Dates?', 'Confirm');
                                        //            var popup = $(document.getElementsByClassName('dx-popup-inherit-height')[0]);
                                        //            popup.addClass("PopupCustomPosition");
                                        //            result.done(function (confirmation) {
                                        //                if (confirmation) {
                                        //                    var startDatebox = $("#dtStartDate").dxDateBox("instance");
                                        //                    var endDatebox = $("#dtEndDate").dxDateBox("instance");

                                        //                    var preconStartdate = dataModel.preconStartDate;
                                        //                    var preconEnddate = dataModel.preconEndDate;
                                        //                    startDatebox.option("value", preconStartdate);
                                        //                    endDatebox.option("value", preconEnddate);
                                        //                }
                                        //            });
                                        //        }
                                        //    }),
                                        //    $(`<div class="mr-3 const_Btn_White_Box" />`).dxButton({
                                        //        text: "Const",
                                        //        hint: 'Use Construction Dates',
                                        //        visible: true,
                                        //        disabled: !$("#gridTemplateContainer").is(":visible"),
                                        //        onClick: function (e) {
                                        //            var result = DevExpress.ui.dialog.confirm('Are you sure you want to switch to Construction Dates?', 'Confirm');
                                        //            var popup = $(document.getElementsByClassName('dx-popup-inherit-height')[0]);
                                        //            popup.addClass("PopupCustomPosition");
                                        //            result.done(function (confirmation) {
                                        //                if (confirmation) {

                                        //                    var startDatebox = $("#dtStartDate").dxDateBox("instance");
                                        //                    var endDatebox = $("#dtEndDate").dxDateBox("instance");

                                        //                    var constStartdate = dataModel.constStartDate;
                                        //                    var constEnddate = dataModel.constEndDate;
                                        //                    startDatebox.option("value", constStartdate);
                                        //                    endDatebox.option("value", constEnddate);

                                        //                }
                                        //            });
                                        //        }
                                        //    }),
                                        //    $(`<div class="mr-1 closeout_Btn_White_Box" />`).dxButton({
                                        //        text: "Closeout",
                                        //        hint: 'Use Closeout Dates',
                                        //        visible: true,
                                        //        disabled: !$("#gridTemplateContainer").is(":visible"),
                                        //        onClick: function (e) {
                                        //            var result = DevExpress.ui.dialog.confirm('Are you sure you want to switch to Closeout Dates?', 'Confirm');
                                        //            var popup = $(document.getElementsByClassName('dx-popup-inherit-height')[0]);
                                        //            popup.addClass("PopupCustomPosition");
                                        //            result.done(function (confirmation) {
                                        //                if (confirmation) {

                                        //                    var startDatebox = $("#dtStartDate").dxDateBox("instance");
                                        //                    var endDatebox = $("#dtEndDate").dxDateBox("instance");

                                        //                    var closeoutStartdate = dataModel.closeoutStartDate;
                                        //                    var closeoutEnddate = dataModel.closeoutEndDate;
                                        //                    startDatebox.option("value", closeoutStartdate);
                                        //                    endDatebox.option("value", closeoutEnddate);

                                        //                }
                                        //            });
                                        //        }
                                        //    })
                                        //),
                                        //$("<div class='pr-2 date-w'>").append(
                                        //    $("<label class='lblStartDate' style='width:100%;'>Start Date</label>"),
                                        //    $("<div id='dtStartDate' class='chkFilterCheck' style='padding-left:5px;width:100%;' />").dxDateBox({
                                        //        type: "date",
                                        //        value: popupFilters.allocationStartDate,
                                        //        displayFormat: 'MMM d, yyyy',
                                        //        disabled: !$("#gridTemplateContainer").is(":visible"),
                                        //        onValueChanged: function (data) {
                                        //            popupFilters.allocationStartDate = data.value;
                                        //            var endDateObj = $("#dtEndDate").dxDateBox("instance");
                                        //            if (typeof endDateObj != "undefined") {
                                        //                var enddate = endDateObj.option('value');
                                        //                if (new Date(enddate) < new Date(data.value)) {
                                        //                    popupFilters.allocationEndDate = popupFilters.allocationStartDate;
                                        //                    endDateObj.option('value', popupFilters.allocationStartDate);
                                        //                }
                                        //            }
                                        //            bindDatapopup(popupFilters);
                                        //        }

                                        //    })

                                        //),
                                        //$("<div class='pr-2 date-w'>").append(
                                        //    $("<label class='lblStartDate' style='width:100%;'>End Date</label>"),
                                        //    $("<div id='dtEndDate' class='chkFilterCheck' style='width:100%;' />").dxDateBox({
                                        //        type: "date",
                                        //        value: popupFilters.allocationEndDate,
                                        //        displayFormat: 'MMM d, yyyy',
                                        //        disabled: !$("#gridTemplateContainer").is(":visible"),
                                        //        onValueChanged: function (data) {
                                        //            popupFilters.allocationEndDate = data.value;
                                        //            var startDateObj = $("#dtStartDate").dxDateBox("instance");
                                        //            if (typeof startDateObj != "undefined") {
                                        //                var startdate = startDateObj.option('value');
                                        //                if (new Date(startdate) > new Date(data.value)) {
                                        //                    popupFilters.allocationStartDate = popupFilters.allocationEndDate;
                                        //                    startDateObj.option('value', popupFilters.allocationEndDate);
                                        //                }
                                        //            }
                                        //            bindDatapopup(popupFilters);
                                        //        }
                                        //    })
                                        //),
                                        //$("<div class='w-100p pr-2'>").append(
                                        //    $("<label class='lblStartDate' style='width:100%;'>% Allocation</label>"),
                                        //    $("<div class='chkFilterCheck' style='width:100%;' />").dxNumberBox({
                                        //        value: $("#gridTemplateContainer").is(":visible") ? popupFilters.pctAllocation : GetPctAllocationAcrossPhases(popupFilters.ID),
                                        //        min: 1,
                                        //        disabled: !$("#gridTemplateContainer").is(":visible"),
                                        //        onValueChanged: function (data) {
                                        //            if (!$("#gridTemplateContainer").is(":visible")) {
                                        //                pctValueChanged = true;
                                        //            }
                                        //            popupFilters.pctAllocation = data.value;
                                        //            bindDatapopup(popupFilters);
                                        //        }
                                        //    })

                                        //),

                                        //),
                                        $("<div class='col-md-7 col-sm-6 pl-1 pr-0 d-flex outer-border-date-box justify-content-between align-items-center'>").append(
                                            $(`<a class="pr-1" id="myProjectLeftIcon" style="visibility:hidden;" href="#" onclick="moveLeft()"><i class="glyphicon glyphicon-chevron-left" style="color:black;"></i></a>`),
                                            $(`<div id="divProjectView" style='width:400px' class="overflow-hidden overflow"></div>`).dxTileView({
                                                items: popupFilters.Allocations,
                                                height: '65px',
                                                width: function () {
                                                    return window.innerWidth / 2.1;
                                                },
                                                showScrollbar: true,
                                                itemTemplate: function (itemData, itemIndex, itemElement) {
                                                    classNameSt = getDateBackgroundColor(new Date(itemData.StartDate), preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                                                    classNameEd = getDateBackgroundColor(new Date(itemData.EndDate), preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                                                    ganttProjSD = new Date(itemData.StartDate).format('MMM d, yyyy');
                                                    ganttProjED = new Date(itemData.EndDate).format('MMM d, yyyy');
                                                    ganttProjReqAloc = itemData.RequiredPctAllocation;
                                                    itemElement.append(
                                                        $(`<div class='paneldiv tooltipp'>`).append(
                                                            $(`<div class='d-flex'>`).append(
                                                                $(`<div class='date-box ${classNameSt}'>${new Date(itemData.StartDate).format('MMM d, yyyy')}</div>`),
                                                                $(`<div class='date-box ${classNameEd}'>${new Date(itemData.EndDate).format('MMM d, yyyy')}</div>`),
                                                            ),
                                                            $(`<div class='date-box-1'>${itemData.RequiredPctAllocation}%</div>`),
                                                            $(`<span class='tooltiptext' style="display:none !important"></span> `)
                                                        )
                                                    )
                                                },
                                                onItemRendered: function (e) {
                                                    setTimeout(checkScrollEnd, 1500);
                                                },
                                            }),
                                            $(`<a id="myProjectRightIcon" style="visibility:hidden;" class="pl-1 pr-1" href="#" onclick="moveRight()"><i class="glyphicon glyphicon-chevron-right" style="color:black;"></i></a>`)
                                        ),
                                        $("<div class='col-md-5 col-sm-6 pl-1 pr-0 d-flex justify-content-between outer-border-date-box'>").append(
                                            $("<div class='col-md-9 noPadding d-flex align-items-end justify-content-around'>").append(
                                                $("<div>").append(
                                                    $("<label class='availibility-2'>% View</label>"),
                                                    $(`<div class="tag-container-1 availibility-1 font-size-class-1"><div class="mr-2">Availability</div><label class="switch"><input id='AvailabilityChk' type="checkbox" onclick='ChangeAvailability(this, "${data.TypeName}");' checked><span class="slider round"></span></label>
                        <div class="ml-2">Allocation</div></div>`)
                                                ),
                                                $("<div>").append(
                                                    $("<label class='availibility-4'>Project Experience</label>"),
                                                    $(`<div class="tag-container-1 availibility-3 font-size-class-1"><div class="mr-2">Show</div><label class="switch"><input id='ProjectExperienceChk' onclick='ChangeProjectExperience();' type="checkbox" checked><span class="slider round"></span></label>
                        <div class="ml-2">Hide</div></div>`)
                                                )
                                            ),
                                            $("<div class='col-md-3 noPadding mr-1 d-flex justify-content-end'>").append(
                                                $("<div id='compareTags' class='pt-3 mt-1' />").append(
                                                    $("<div style='border:none;-webkit-box-shadow: none'>").dxButton({
                                                        icon: '/content/Images/RMONE/comparetags.png',
                                                        hint: 'Open Tag Matrix',
                                                        onClick() {
                                                            OpenExperienceTagPopup();
                                                        },
                                                    })
                                                ),
                                                $("<div id='compareResume' class='pt-3 mt-1' />").append(
                                                    $("<div style='border:none;-webkit-box-shadow: none'>").dxButton({
                                                        icon: '/content/Images/RMONE/compareresume.png',
                                                        hint: 'Compare Resume',
                                                        onClick() {
                                                            OpenResumeCompare(data.TypeName);
                                                        },
                                                    })
                                                ),
                                                $("<div id='openGanttView' class='pt-3 mt-1' />").append(
                                                    $("<div style='border:none;-webkit-box-shadow: none'>").dxButton({
                                                        icon: '/content/Images/ganttBlackNew.png',
                                                        hint: 'Open Gantt View',
                                                        onClick() {
                                                            OpenUsersGanttView(ganttProjSD, ganttProjED, classNameSt, classNameEd, ganttProjReqAloc);
                                                        },
                                                    })
                                                )
                                            )
                                        )
                                    ),
                                    $("<div class='shadow-effect' style='padding-bottom: 10px;' />").append(
                                        $("<div id='filterChecks' class='clearfix pb-2 pt-2' />").append(
                                            $("<Label class='clsSmartFilter' >Suggested Filters:</Label>"),
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
                                                text: "<%=Customer%>",
                                                visible: ("<%=Customer%>" != '' && "<%=Customer%>" != null) == true ? true : false,
                                                value: popupFilters.Customer,
                                                hint: 'Customer',
                                                onValueChanged: function (e) {
                                                    popupFilters.CompanyLookup = "<%=CompanyLookup%>";
                                                    popupFilters.Customer = e.value;
                                                    bindDatapopup(popupFilters);
                                                },
                                            }),

                                            $("<div id='chkSector' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                                text: "<%=Sector%>",
                                                visible: ("<%=Sector%>" != '' && "<%=Sector%>" != null) == true ? true : false,
                                                hint: 'Sector',
                                                value: popupFilters.Sector,
                                                onValueChanged: function (e) {
                                                    popupFilters.SectorName = "<%=Sector%>";
                                                    popupFilters.Sector = e.value;
                                                    bindDatapopup(popupFilters);
                                                },
                                            }),

                                            $("<div id='chkCertification' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                                text: "Certification",
                                                visible: true,
                                                hint: 'Certification',
                                                onValueChanged: function (e) {
                                                    if (e.value) {
                                                        e.component.option("text", "");
                                                        $("#certificationTxt").dxTagBox("option", "visible", true);
                                                    }
                                                    else {
                                                        e.component.option("text", "Certification");
                                                        $("#certificationTxt").dxTagBox("option", "visible", false);
                                                        $("#certificationTxt").dxTagBox("option", "value", []);
                                                    }
                                                },
                                            }),
                                            $("<div id='certificationTxt' class='filterctrl-jobDepartment' style='margin-top:-6px;padding-left:5px;' />").dxTagBox({  //dxSelectBox
                                                showSelectionControls: true,
                                                placeholder: "Certification",
                                                valueExpr: "ID",
                                                grouped: true,
                                                visible: false,
                                                displayExpr: "Title",
                                                dataSource: new DevExpress.data.DataSource({
                                                    store: certifications,
                                                    key: 'ID',
                                                    group: 'CategoryName',
                                                }),
                                                maxDisplayedTags: 1,
                                                searchEnabled: true,
                                                onValueChanged: function (selectedItems) {
                                                    popupFilters.SelectedCertifications = selectedItems.value.join();
                                                    bindDatapopup(popupFilters);
                                                },
                                            }),
                                        ),

                                        $("<div class='clearfix pt-1' />").append(
                                            $("<Label class='clsSmartFilter pr-4' style='margin-top:10px;' >Tags:</Label>"),
                                            $('<div class="tag-container mr-3" style="border: 2px solid #ddd;min-height:45px;">').append(
                                                $('<div class="tag-container-2 mr-3">').append(
                                                    $("<div id='addExpTags' style='border: none; -webkit-box-shadow: none' />").dxButton({
                                                        icon: "/Content/Images/plus-blue-new.png",
                                                        hint: "Reset Experience Tags",
                                                        visible: hasAccessToAddTags,
                                                        visible: allowGroupFilterOnExpTags,
                                                        focusStateEnabled: false,
                                                        onClick() {
                                                            popupFilters.SelectedTags = JSON.parse(JSON.stringify(projectExperiencTags));
                                                            RenderProjectTagsOnFrame();
                                                        }
                                                    }),
                                                    $('<div id="projectExpTags" style="display:flex;flex-wrap:wrap;" />'),
                                                ),
                                                $("<div id='clearExpTags' style='border: none; -webkit-box-shadow: none' />").dxButton({
                                                    icon: "/Content/Images/RMONE/clear-icon.png",
                                                    hint: "Clear Experience Tags",
                                                    visible: allowGroupFilterOnExpTags,
                                                    onClick: function () {
                                                        popupFilters.SelectedTags = [];
                                                        RenderProjectTagsOnFrame();
                                                    }
                                                }),
                                            ),

                                        ),
                                        $("<div class='clearfix pb-1' />").append(
                                            $("<Label class='clsSmartFilter' style='margin-top:10px;' >Filters:</Label>"),
                                            $("<div id='dropdownFilters' class='d-flex justify-content-between' />").append(
                                                $("<div class='flex-grow-1 display-flex' />").append(

                                                    $("<div class='filterctrl-jobDepartment' />").dxTagBox({  //dxSelectBox
                                                        showSelectionControls: true,
                                                        placeholder: "Division",
                                                        valueExpr: "ID",
                                                        displayExpr: "Title",
                                                        dataSource: "/api/CoreRMM/GetDivisions?OnlyEnabled=1",
                                                        maxDisplayedTags: 1,
                                                        searchEnabled: true,
                                                        onValueChanged: function (selectedItems) {
                                                            //var items = selectedItems.addedItems[0].ID;
                                                            let divisionId = '0';
                                                            var items = selectedItems.component._selectedItems;
                                                            if (items.length > 0) {
                                                                var lstItems = items.map(function (i) {
                                                                    return i.ID;
                                                                });
                                                                divisionId = lstItems.join(',');
                                                            }
                                                            popupFilters.DivisionId = divisionId;
                                                            //popupFilters.departments = items;
                                                            $.get("/api/rmmapi/GetGroupTitles?GroupID=" + popupFilters.groupID + "&DivisionID=" + divisionId, function (data, status) {

                                                                JobTitleData = data;
                                                                var tagBox = $("#tagboxTitles").dxTagBox("instance");
                                                                tagBox.option("dataSource", JobTitleData);
                                                                tagBox.reset();
                                                            });
                                                            bindDatapopup(popupFilters);
                                                        },
                                                    }),
                                                    $("<div id='tagboxTitles' class='filterctrl-jobtitle' />").dxTagBox({
                                                        showSelectionControls: true,
                                                        text: "Job Titles",
                                                        placeholder: "Title",
                                                        searchEnabled: true,
                                                        dataSource: "/api/rmmapi/GetGroupTitles?GroupID=" + popupFilters.groupID + "&DivisionID=0",
                                                        maxDisplayedTags: 1,
                                                        onValueChanged: function (selectedItems) {
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

                                                    $("<div class='filterctrl-jobDepartment' />").dxSelectBox({
                                                        dataSource: selectBoxItems,
                                                        displayExpr: "text",
                                                        value: _.findWhere(selectBoxItems, { value: popupFilters.resourceAvailability }),
                                                        layout: "horizontal",
                                                        onValueChanged: function (e) {
                                                            popupFilters.resourceAvailability = e.value.value;
                                                            bindDatapopup(popupFilters);
                                                        }
                                                    }),

                                                    $("<div class='filterctrl-userpicker' />").dxSelectBox({
                                                        placeholder: "Search team member",
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
                                                ),
                                            )
                                        )
                                    ),
                                    $(`<div id='tileViewContainer' style='clear:both;padding-bottom:10px;max-height:${windowHeight}px;' />`).dxTileView({
                                        width: window.innerWidth - 100, //150
                                        showScrollbar: true,
                                        baseItemHeight: 65,
                                        baseItemWidth: 150,
                                        itemMargin: 15,
                                        direction: "vertical",
                                        elementAttr: { "class": "tileViewContainer" },
                                        noDataText: "No resource available",
                                        itemTemplate: function (itemData, itemIndex, itemElement) {
                                            if (UserConflictData.filter(x => x.AssignedTo == itemData.AssignedTo).length == 0)
                                                UserConflictData.push(itemData);
                                            itemData.PctAllocation = Math.round(itemData.PctAllocation);
                                            itemData.SoftPctAllocation = Math.round(itemData.SoftPctAllocation);
                                            itemData.TotalPctAllocation = Math.round(itemData.TotalPctAllocation);
                                            var html = new Array();
                                            var str = itemData.AssignedTo + "','" + itemData.AssignedToName;
                                            if (itemData.ResourceTags != null) {
                                                itemData.ResourceTags.forEach(function (value, index) {
                                                    if (CheckTagExist(value.Type, value.TagId)) {
                                                        let expTag = value.Type == 2 ? experienceTags.filter(x => x.ID == value.TagId)[0] : certifications.filter(x => x.ID == value.TagId)[0];
                                                        if (ResourceTagCount.length == 0 || ResourceTagCount.filter(x => x.TagId == value.TagId && x.Type == value.Type && x.UserId == itemData.AssignedTo).length == 0) {
                                                            ResourceTagCount.push({
                                                                TagId: value.TagId,
                                                                Title: expTag.Title,
                                                                TagCount: value.TagCount,
                                                                Type: value.Type,
                                                                UserId: itemData.AssignedTo,
                                                                UserName: itemData.AssignedToName,
                                                                Allocation: itemData.PctAllocation,
                                                                UserPicture: itemData.UserImagePath,
                                                                RoleName: itemData.RoleName,
                                                                Availability: (100 - Number(itemData.PctAllocation)),
                                                            });
                                                        }
                                                    }
                                                });
                                            }
                                            var strwithspace = str.replace(/\s/g, '&nbsp;'); //str.replace(" ", "&nbsp;")
                                            //html.push("<div class='timesheet'><img src='/content/images/icon_three_black_dots.png' height='5px' title='Allocation Timeline' onclick=openResourceTimeSheet('" + strwithspace + "'); />");
                                            html.push("<label class='innerCheckbox' onclick=storeCheckedUser()>");
                                            html.push("<input type='checkbox' allocaionview='" + popupFilters.isAllocationView + "' pctallocation='" + itemData.PctAllocation + "' id='" + itemData.AssignedTo + "' name='" + itemData.JobTitle + "' onclick=storeCheckedUser()>");
                                            html.push("</label>");
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
                                            else if (!popupFilters.isAllocationView) {
                                                html.push("<div>");
                                                html.push("(" + (100 - Number(itemData.PctAllocation)) + "%)");
                                                //html.push("(" + (Number(itemData.PctAllocation)) + "%)");
                                                html.push("</div>");
                                            }
                                            if (popupFilters.isAllocationView && itemData.PctAllocation < 100) {
                                                html.push("<div>");
                                                html.push("(" + (Number(itemData.PctAllocation)) + "%)");
                                                html.push("</div>");
                                            }
                                            if (popupFilters.projectCount || popupFilters.projectVolume) {
                                                if (itemData.PctAllocation > 0) {
                                                    html.push("<div class='capacitymain'>");
                                                    html.push("<div class='capacityblock allocation-v" + itemData.TotalVolumeRange + "'>");
                                                    html.push(itemData.TotalVolume == null ? "$0" : itemData.TotalVolume);
                                                    html.push("</div>");
                                                    html.push("<div class='capacityblock allocation-c" + itemData.projectCountRange + "''>");
                                                    html.push("#" + itemData.ProjectCount);
                                                    html.push("</div>");
                                                    html.push("</div>");
                                                }
                                            }
                                            else {
    <%--if (itemData.SoftPctAllocation > 0) {
        if (popupFilters.isAllocationView) {
            html.push("<div class='capacitymain'>");
            <%if (ShowTotalAllocationsInSearch) { %>
            html.push("<div class='capacityblock cc allocation-v" + itemData.TotalVolumeRange + "'>");
            html.push("T: " + Number(itemData.TotalPctAllocation) + "%");
            html.push("</div>");
            html.push("<div class='capacityblock allocation-c" + itemData.projectCountRange + "''>");
            <%} else { %>
            html.push("<div class='capacityblock-1 allocation-c" + itemData.projectCountRange + "''>");
            <%}%>
            html.push("S: " + Number(itemData.SoftPctAllocation) + "%");
            html.push("</div>");
            html.push("</div>");
           
        }
        else {
            html.push("<div class='capacitymain'>");
             <%if (ShowTotalAllocationsInSearch) { %>
            html.push("<div class='capacityblock cc allocation-v" + itemData.TotalVolumeRange + "'>");
            html.push("T: " + (100 - Number(itemData.TotalPctAllocation)) + "%");
            html.push("</div>");
            html.push("<div class='capacityblock allocation-c" + itemData.projectCountRange + "''>");
            <%} else { %>
            html.push("<div class='capacityblock-1 allocation-c" + itemData.projectCountRange + "''>");
            <%}%>
            html.push("<div class='capacityblock allocation-c" + itemData.projectCountRange + "''>");
            html.push("S: " + (100 - Number(itemData.SoftPctAllocation)) + "%");
            html.push("</div>");
            html.push("</div>");

        }
    }--%>

                                            }
                                            if (itemData.WeekWiseAllocations?.length > 0 && itemData.WeekWiseAllocations?.filter(x => !x.IsAvailable).length > 0) {
                                                html.push(`<div style='cursor:pointer;' onclick='storeCheckedUser();OpenConflictWeekData(${JSON.stringify(itemData.WeekWiseAllocations).replaceAll("'", "`")}, "${itemData.AssignedToName.replaceAll("'", "`")}", "${itemData.AssignedTo}", "${itemData.UserImagePath}");'>`);
                                                html.push("<div class='conflict-circle'>");
                                                html.push(itemData.WeekWiseAllocations.filter(x => !x.IsAvailable).length);
                                                html.push("</div>");
                                                html.push("</div>");
                                            }
                                            html.push("</div>");
                                            html.push("</div>");
                                            itemElement.attr("class", "allocation-block allocation-r" + itemData.AllocationRange);
                                            //itemElement.attr("title", itemData.JobTitle);
                                            itemElement.attr("id", "allocation-block" + itemIndex);
                                            itemElement.attr("onmouseover", "showTooltip('allocation-block" + itemIndex + "','" + itemData.AssignedTo + "')");
                                            itemElement.attr("onmouseout", "hideTooltip()");
                                            itemElement.append(html.join(""));

                                        },
                                        onItemClick: function (e) {
                                            //debugger;
                                            let checkedUser = 0;
                                            var data = e.itemData;
                                            $(".innerCheckbox input").each(function () {
                                                if ($(this).is(":checked")) {
                                                    checkedUser++;
                                                }
                                            });
                                            if (checkedUser > 0) {
                                                if (showTimeSheet == false)
                                                    $("#" + data.AssignedTo).click();
                                                if (showTimeSheet == true)
                                                    showTimeSheet = false;
                                            }
                                            else {
                                                if (showTimeSheet == true) {
                                                    showTimeSheet = false;
                                                    return;
                                                }
                                                if (UserConflictData.filter(x => x.WeekWiseAllocations?.every(y => y.IsAvailable))?.length > 0 && !data.WeekWiseAllocations?.every(x => x.IsAvailable)) {
                                                    var conflictDialog = DevExpress.ui.dialog.custom({
                                                        title: "Alert",
                                                        message: `${data.AssignedToName.replaceAll("'", "`")} has conflict weeks for the current duration. Do you want to proceed?`,
                                                        buttons: [
                                                            { text: "Yes", onClick: function () { return "Ok" }, elementAttr: {"class": "btnBlue" } },
                                                            { text: "No", onClick: function () { return "Cancel" }, elementAttr: { "class": "btnNormal" } }
                                                        ]
                                                    });
                                                    conflictDialog.show().done(function (dialogResult) {
                                                        if (dialogResult == "Ok") {
                                                            AllocateResourceToAllocation(data, null, isStartEndDateLesserThanCurrDate);
                                                        }
                                                        else if (dialogResult == "Cancel") {
                                                            OpenConflictWeekData(data.WeekWiseAllocations, data.AssignedToName.replaceAll("'", "`"), data.AssignedTo, data.UserImageUrl);
                                                            return false;
                                                        }
                                                        else
                                                            return false;
                                                    });
                                                }
                                                else
                                                    AllocateResourceToAllocation(data, null, isStartEndDateLesserThanCurrDate);
                                            }
                                        },
                                        noDataText: function (e) {
                                            $('.dx-empty-message').html('No resource available');
                                            $("#popuploader").dxLoadPanel({
                                                message: "Loading...",
                                                visible: false
                                            });
                                        }
                                    })

                                )
                            },
                            onContentReady: function () {
                                bindDatapopup(popupFilters);
                            },
                            itemClick: function (e) {
                            },
                            onHiding: function (e) {
                                popupFilters.SelectedTags = [];
                                popupFilters.SelectedCertifications = "";
                            }
                        });
                        RenderProjectTagsOnFrame(false);
                        var popupInstance = $('#popupContainer').dxPopup('instance');
                        popupInstance.option('title', popupTitle);

                        //}
                    } else {
                        DevExpress.ui.dialog.alert(`The end date should be in the future.`, 'Error');
                    }
                }
            });
        }
        else {
            SetDatesOfSummaryView(e.target.id);
            if (popupFilters.Allocations?.length > 0) {
                if ($(this).attr("startDate") == undefined || $(this).attr("startDate") == '' || $(this).attr("endDate") == undefined || $(this).attr("endDate") == '') {
                    let allocId = $(this).attr("id");
                    $("#confirmPopupContainer").dxPopup({
                        width: "550",
                        height: "auto",
                        title: "Select dates by clicking phase or manually enter/change dates",
                        visible: true,
                        scrolling: true,
                        dragEnabled: true,
                        resizeEnabled: true,
                        hideOnOutsideClick: true,
                        showTitle: false,
                        position: {
                            my: 'center',
                            at: 'top',
                            collision: 'fit',
                        },
                        contentTemplate: function (contentElement) {

                            contentElement.append(
                                $('<div class="popup-title">Select dates by clicking phase or manually enter/change dates.</div><hr/>'),
                                $("<div class='d-flex-justify'>").append(
                                    $("<div>").append($(`<div/>`).dxButton({
                                        text: "Select Precon Dates",
                                        hint: 'Use Precon Dates',
                                        icon: "/content/Images/RMONE/calender_activeWhite.png",
                                        visible: true,
                                        elementAttr: {
                                            class: 'Precon_Btn'
                                        },
                                        onClick: function (e) {
                                            var startDatebox = $("#dtStartDateC").dxDateBox("instance");
                                            var endDatebox = $("#dtEndDateC").dxDateBox("instance");
                                            var preconStartdate = dataModel.preconStartDate;
                                            var preconEnddate = dataModel.preconEndDate;
                                            if (!isDateValid(new Date(preconStartdate)) || !isDateValid(new Date(preconEnddate))) {
                                                openDateAgent(Model.RecordId, false, true);
                                            } else {
                                                startDatebox.option("value", preconStartdate);
                                                endDatebox.option("value", preconEnddate);
                                            }
                                        }
                                    }),
                                        $(`<p class='dateText pPrecon' ${dataModel.preconStartDate == '' || dataModel.preconEndDate == '' ? 'style=display:none;' : ''}>${new Date(dataModel.preconStartDate).format("MMM d, yyyy")} to ${new Date(dataModel.preconEndDate).format("MMM d, yyyy")}</p>`)),
                                    $("<div>").append($(`<div />`).dxButton({
                                        text: "Select Const. Dates",
                                        icon: "/content/Images/RMONE/calender_activeWhite.png",
                                        hint: 'Use Construction Dates',
                                        visible: true,
                                        elementAttr: {
                                            class: 'Const_Btn'
                                        },
                                        onClick: function (e) {
                                            var startDatebox = $("#dtStartDateC").dxDateBox("instance");
                                            var endDatebox = $("#dtEndDateC").dxDateBox("instance");

                                            var constStartdate = dataModel.constStartDate;
                                            var constEnddate = dataModel.constEndDate;
                                            if (!isDateValid(new Date(constStartdate)) || !isDateValid(new Date(constEnddate))) {
                                                openDateAgent(Model.RecordId, true, false);
                                            }
                                            else {
                                                startDatebox.option("value", constStartdate);
                                                endDatebox.option("value", constEnddate);
                                            }
                                        }
                                    }),
                                        $(`<p class='dateText pConst' ${dataModel.constStartDate == '' || dataModel.constEndDate == '' ? 'style=display:none;' : ''}>${new Date(dataModel.constStartDate).format("MMM d, yyyy")} to ${new Date(dataModel.constEndDate).format("MMM d, yyyy")}</p>`)),
                                    $("<div>").append($(`<div />`).dxButton({
                                        text: "Select Closeout Dates",
                                        icon: "/content/Images/RMONE/calender_activeWhite.png",
                                        hint: 'Use Closeout Dates',
                                        visible: true,
                                        elementAttr: {
                                            class: 'Closeout_Btn'
                                        },
                                        onClick: function (e) {
                                            var startDatebox = $("#dtStartDateC").dxDateBox("instance");
                                            var endDatebox = $("#dtEndDateC").dxDateBox("instance");

                                            var closeoutStartdate = dataModel.closeoutStartDate;
                                            var closeoutEnddate = dataModel.closeoutEndDate;
                                            if (!isDateValid(new Date(closeoutStartdate)) || !isDateValid(new Date(closeoutEnddate))) {
                                                openDateAgent(Model.RecordId, true, false);
                                            }
                                            else {
                                                startDatebox.option("value", closeoutStartdate);
                                                endDatebox.option("value", closeoutEnddate);
                                            }
                                        }
                                    }),
                                        $(`<p class='dateText pCloseout' ${dataModel.closeoutStartDate == '' || dataModel.closeoutEndDate == '' ? 'style=display:none;' : ''}>${new Date(dataModel.closeoutStartDate).format("MMM d, yyyy")} to ${new Date(dataModel.closeoutEndDate).format("MMM d, yyyy")}</p>`)),
                                ),
                                $("<div class='d-flex-justify row mb-3' />").append(
                                    $("<div class='pr-2'>").append(
                                        $("<label class='lblStartDate' style='width:100%;'>Start Date</label>"),
                                        $("<div id='dtStartDateC' class='chkFilterCheck' style='padding-left:5px;width:100%;' />").dxDateBox({
                                            type: "date",
                                            displayFormat: "MM/dd/yyyy",
                                            displayFormat: {
                                                parser: function (value) {
                                                    let parts = value.split('/');
                                                    if (3 !== parts.length) {
                                                        return;
                                                    }
                                                    return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                                },
                                                formatter: function (value) {
                                                    return DevExpress.localization.date.format(value, 'shortdate');
                                                }

                                            }
                                        })

                                    ),
                                    $("<div>").append(
                                        $("<label class='lblStartDate' style='width:100%;'>End Date</label>"),
                                        $("<div id='dtEndDateC' class='chkFilterCheck' style='width:100%;' />").dxDateBox({
                                            type: "date",
                                            displayFormat: "MM/dd/yyyy",
                                            displayFormat: {
                                                parser: function (value) {
                                                    let parts = value.split('/');
                                                    if (3 !== parts.length) {
                                                        return;
                                                    }
                                                    return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                                },
                                                formatter: function (value) {
                                                    return DevExpress.localization.date.format(value, 'shortdate');
                                                }
                                            }
                                        })
                                    )),
                                $("<div>").append(
                                    $(`<div class="mt-4 btnAddNew" style='float:right;padding: 0px 10px;font-size: 14px;' />`).dxButton({
                                        text: "Ok",
                                        hint: 'Confirm Alloc Dates',
                                        visible: true,
                                        onClick: function (e) {
                                            let tempData = globaldata.filter(e => e.ID == allocId)[0];
                                            var startDatebox = $("#dtStartDateC").dxDateBox("instance");
                                            var endDatebox = $("#dtEndDateC").dxDateBox("instance");
                                            if (startDatebox.option("value") == null || endDatebox.option("value") == null) {
                                                if (startDatebox.option("value") == null)
                                                    $("#dtStartDateC").css("border", "1px solid red");
                                                if (endDatebox.option("value") == null)
                                                    $("#dtEndDateC").css("border", "1px solid red");
                                            }
                                            else {
                                                tempData.AllocationStartDate = startDatebox.option("value");
                                                tempData.AllocationEndDate = endDatebox.option("value");
                                                var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                                                dataGrid.option("dataSource", globaldata);
                                                var popup = $("#confirmPopupContainer").dxPopup("instance");
                                                popup.hide();
                                                //alert("step7");
                                                CompactPhaseConstraints();
                                            }
                                        }
                                    }),
                                    $(`<div class="mt-4 btnAddNew" style='float:right;padding: 0px 10px;font-size: 14px;' />`).dxButton({
                                        text: "Cancel",
                                        visible: true,
                                        onClick: function (e) {
                                            var popup = $("#confirmPopupContainer").dxPopup("instance");
                                            popup.hide();
                                        }
                                    })
                                )
                            )
                        },
                        itemClick: function (e) {
                        }
                    });
                    //DevExpress.ui.dialog.alert("Select Both Start and End Dates!", "Error!");
                    return false;
                }
                else if ($(this).attr("group") == '' || ($(this).attr("group") != '' && $(this).attr("group").startsWith("TYPE-"))) {
                    DevExpress.ui.dialog.alert(`Please select the role.`, 'Error');
                }
                else {
                    var groupid = $(this).attr("id");
                    var dataid = e.target.id;
                    var data = _.find(globaldata, function (s) { return s.ID.toString() === dataid; });

                    pctValueChanged = false;
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
                    popupFilters.isAllocationView = 1;
                    if (projectID.startsWith("OPM")) {
                        popupFilters.ModuleIncludes = false;
                    }
                    else {
                        popupFilters.ModuleIncludes = false;
                    }
                    popupFilters.JobTitles = "";
                    popupFilters.departments = "";
                    popupFilters.DivisionId = "";
                    popupFilters.SelectedUserID = "";
                    popupFilters.ID = $(this).attr("ID");
                    RowId = $(this).attr("ID");
                    var popupTitle = "Available Resource";
                    if (data)
                        //popupTitle = "Allocated " + data.TypeName + "s";
                        popupTitle = "Allocate " + data.TypeName;

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
                            let preconstartDate = new Date(dataModel.preconStartDate);

                            let conststartDate = new Date(dataModel.constStartDate);
                            let constEndDate = new Date(dataModel.constEndDate);

                            let closeoutstartDate = new Date(dataModel.closeoutStartDate);
                            let windowHeight = parseInt(60 * $(window).height() / 100);
                            let classNameSt = null;
                            let classNameEd = null;
                            let ganttProjSD = null;
                            let ganttProjED = null;
                            let ganttProjReqAloc = null;
                            if ($(window).height() < 700) {
                                windowHeight = parseInt(45 * $(window).height() / 100);
                            }
                            //popupFilters.SelectedTags = JSON.parse(JSON.stringify(projectExperiencTags));
                            contentElement.append(
                                $("<div id='popuploader' />").dxLoadPanel({
                                    message: "Loading...",
                                    visible: true
                                }),
                                $("<div class='d-flex row px-3 shadow-effect mb-3' />").append(
                                    //$("<div class='col-md-10 d-flex'>").append(
                                    //$("<div class='pr-2 pt-4'>").append(
                                    //    $(`<div class="mr-3 precon_Btn_White_Box"/>`).dxButton({
                                    //        text: "Precon",
                                    //        hint: 'Use Precon Dates',
                                    //        visible: true,
                                    //        disabled: !$("#gridTemplateContainer").is(":visible"),
                                    //        onClick: function (e) {
                                    //            var result = DevExpress.ui.dialog.confirm('Are you sure you want to switch to Precon Dates?', 'Confirm');
                                    //            var popup = $(document.getElementsByClassName('dx-popup-inherit-height')[0]);
                                    //            popup.addClass("PopupCustomPosition");
                                    //            result.done(function (confirmation) {
                                    //                if (confirmation) {
                                    //                    var startDatebox = $("#dtStartDate").dxDateBox("instance");
                                    //                    var endDatebox = $("#dtEndDate").dxDateBox("instance");

                                    //                    var preconStartdate = dataModel.preconStartDate;
                                    //                    var preconEnddate = dataModel.preconEndDate;
                                    //                    startDatebox.option("value", preconStartdate);
                                    //                    endDatebox.option("value", preconEnddate);
                                    //                }
                                    //            });
                                    //        }
                                    //    }),
                                    //    $(`<div class="mr-3 const_Btn_White_Box" />`).dxButton({
                                    //        text: "Const",
                                    //        hint: 'Use Construction Dates',
                                    //        visible: true,
                                    //        disabled: !$("#gridTemplateContainer").is(":visible"),
                                    //        onClick: function (e) {
                                    //            var result = DevExpress.ui.dialog.confirm('Are you sure you want to switch to Construction Dates?', 'Confirm');
                                    //            var popup = $(document.getElementsByClassName('dx-popup-inherit-height')[0]);
                                    //            popup.addClass("PopupCustomPosition");
                                    //            result.done(function (confirmation) {
                                    //                if (confirmation) {

                                    //                    var startDatebox = $("#dtStartDate").dxDateBox("instance");
                                    //                    var endDatebox = $("#dtEndDate").dxDateBox("instance");

                                    //                    var constStartdate = dataModel.constStartDate;
                                    //                    var constEnddate = dataModel.constEndDate;
                                    //                    startDatebox.option("value", constStartdate);
                                    //                    endDatebox.option("value", constEnddate);

                                    //                }
                                    //            });
                                    //        }
                                    //    }),
                                    //    $(`<div class="mr-1 closeout_Btn_White_Box" />`).dxButton({
                                    //        text: "Closeout",
                                    //        hint: 'Use Closeout Dates',
                                    //        visible: true,
                                    //        disabled: !$("#gridTemplateContainer").is(":visible"),
                                    //        onClick: function (e) {
                                    //            var result = DevExpress.ui.dialog.confirm('Are you sure you want to switch to Closeout Dates?', 'Confirm');
                                    //            var popup = $(document.getElementsByClassName('dx-popup-inherit-height')[0]);
                                    //            popup.addClass("PopupCustomPosition");
                                    //            result.done(function (confirmation) {
                                    //                if (confirmation) {

                                    //                    var startDatebox = $("#dtStartDate").dxDateBox("instance");
                                    //                    var endDatebox = $("#dtEndDate").dxDateBox("instance");

                                    //                    var closeoutStartdate = dataModel.closeoutStartDate;
                                    //                    var closeoutEnddate = dataModel.closeoutEndDate;
                                    //                    startDatebox.option("value", closeoutStartdate);
                                    //                    endDatebox.option("value", closeoutEnddate);

                                    //                }
                                    //            });
                                    //        }
                                    //    })
                                    //),
                                    //$("<div class='pr-2 date-w'>").append(
                                    //    $("<label class='lblStartDate' style='width:100%;'>Start Date</label>"),
                                    //    $("<div id='dtStartDate' class='chkFilterCheck' style='padding-left:5px;width:100%;' />").dxDateBox({
                                    //        type: "date",
                                    //        value: popupFilters.allocationStartDate,
                                    //        displayFormat: 'MMM d, yyyy',
                                    //        disabled: !$("#gridTemplateContainer").is(":visible"),
                                    //        onValueChanged: function (data) {
                                    //            popupFilters.allocationStartDate = data.value;
                                    //            var endDateObj = $("#dtEndDate").dxDateBox("instance");
                                    //            if (typeof endDateObj != "undefined") {
                                    //                var enddate = endDateObj.option('value');
                                    //                if (new Date(enddate) < new Date(data.value)) {
                                    //                    popupFilters.allocationEndDate = popupFilters.allocationStartDate;
                                    //                    endDateObj.option('value', popupFilters.allocationStartDate);
                                    //                }
                                    //            }
                                    //            bindDatapopup(popupFilters);
                                    //        }

                                    //    })

                                    //),
                                    //$("<div class='pr-2 date-w'>").append(
                                    //    $("<label class='lblStartDate' style='width:100%;'>End Date</label>"),
                                    //    $("<div id='dtEndDate' class='chkFilterCheck' style='width:100%;' />").dxDateBox({
                                    //        type: "date",
                                    //        value: popupFilters.allocationEndDate,
                                    //        displayFormat: 'MMM d, yyyy',
                                    //        disabled: !$("#gridTemplateContainer").is(":visible"),
                                    //        onValueChanged: function (data) {
                                    //            popupFilters.allocationEndDate = data.value;
                                    //            var startDateObj = $("#dtStartDate").dxDateBox("instance");
                                    //            if (typeof startDateObj != "undefined") {
                                    //                var startdate = startDateObj.option('value');
                                    //                if (new Date(startdate) > new Date(data.value)) {
                                    //                    popupFilters.allocationStartDate = popupFilters.allocationEndDate;
                                    //                    startDateObj.option('value', popupFilters.allocationEndDate);
                                    //                }
                                    //            }
                                    //            bindDatapopup(popupFilters);
                                    //        }
                                    //    })
                                    //),
                                    //$("<div class='w-100p pr-2'>").append(
                                    //    $("<label class='lblStartDate' style='width:100%;'>% Allocation</label>"),
                                    //    $("<div class='chkFilterCheck' style='width:100%;' />").dxNumberBox({
                                    //        value: $("#gridTemplateContainer").is(":visible") ? popupFilters.pctAllocation : GetPctAllocationAcrossPhases(popupFilters.ID),
                                    //        min: 1,
                                    //        disabled: !$("#gridTemplateContainer").is(":visible"),
                                    //        onValueChanged: function (data) {
                                    //            if (!$("#gridTemplateContainer").is(":visible")) {
                                    //                pctValueChanged = true;
                                    //            }
                                    //            popupFilters.pctAllocation = data.value;
                                    //            bindDatapopup(popupFilters);
                                    //        }
                                    //    })

                                    //),

                                    //),
                                    $("<div class='col-md-7 col-sm-6 pl-1 pr-0 d-flex outer-border-date-box justify-content-between align-items-center'>").append(
                                        $(`<a class="pr-1" id="myProjectLeftIcon" style="visibility:hidden;" href="#" onclick="moveLeft()"><i class="glyphicon glyphicon-chevron-left" style="color:black;"></i></a>`),
                                        $(`<div id="divProjectView" style='width:400px' class="overflow-hidden overflow"></div>`).dxTileView({
                                            items: popupFilters.Allocations,
                                            height: '65px',
                                            width: function () {
                                                return window.innerWidth / 2.1;
                                            },
                                            showScrollbar: true,
                                            itemTemplate: function (itemData, itemIndex, itemElement) {
                                                classNameSt = getDateBackgroundColor(new Date(itemData.StartDate), preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                                                classNameEd = getDateBackgroundColor(new Date(itemData.EndDate), preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                                                ganttProjSD = new Date(itemData.StartDate).format('MMM d, yyyy');
                                                ganttProjED = new Date(itemData.EndDate).format('MMM d, yyyy');
                                                ganttProjReqAloc = itemData.RequiredPctAllocation;

                                                itemElement.append(
                                                    $(`<div class='paneldiv tooltipp'>`).append(
                                                        $(`<div class='d-flex'>`).append(
                                                            $(`<div class='date-box ${classNameSt}'>${new Date(itemData.StartDate).format('MMM d, yyyy')}</div>`),
                                                            $(`<div class='date-box ${classNameEd}'>${new Date(itemData.EndDate).format('MMM d, yyyy')}</div>`),
                                                        ),
                                                        $(`<div class='date-box-1'>${itemData.RequiredPctAllocation}%</div>`),
                                                        $(`<span class='tooltiptext' style="display:none !important"></span> `)
                                                    )
                                                )
                                            },
                                            onItemRendered: function (e) {
                                                setTimeout(checkScrollEnd, 1500);
                                            },
                                        }),
                                        $(`<a id="myProjectRightIcon" style="visibility:hidden;" class="pl-1 pr-1" href="#" onclick="moveRight()"><i class="glyphicon glyphicon-chevron-right" style="color:black;"></i></a>`)
                                    ),
                                    $("<div class='col-md-5 col-sm-6 pl-1 pr-0 d-flex justify-content-between outer-border-date-box'>").append(
                                        $("<div class='col-md-9 noPadding d-flex align-items-end justify-content-around'>").append(
                                            $("<div>").append(
                                                $("<label class='availibility-2'>% View</label>"),
                                                $(`<div class="tag-container-1 availibility-1 font-size-class-1"><div class="mr-2">Availability</div><label class="switch"><input id='AvailabilityChk' type="checkbox" onclick='ChangeAvailability(this, "${data.TypeName}");' checked><span class="slider round"></span></label>
                                    <div class="ml-2">Allocation</div></div>`)
                                            ),
                                            $("<div>").append(
                                                $("<label class='availibility-4'>Project Experience</label>"),
                                                $(`<div class="tag-container-1 availibility-3 font-size-class-1"><div class="mr-2">Show</div><label class="switch"><input id='ProjectExperienceChk' onclick='ChangeProjectExperience();' type="checkbox" checked><span class="slider round"></span></label>
                                    <div class="ml-2">Hide</div></div>`)
                                            )
                                        ),
                                        $("<div class='col-md-3 noPadding mr-1 d-flex justify-content-end'>").append(
                                            $("<div id='compareTags' class='pt-3 mt-1' />").append(
                                                $("<div style='border:none;-webkit-box-shadow: none'>").dxButton({
                                                    icon: '/content/Images/RMONE/comparetags.png',
                                                    hint: 'Open Tag Matrix',
                                                    onClick() {
                                                        OpenExperienceTagPopup();
                                                    },
                                                })
                                            ),
                                            $("<div id='compareResume' class='pt-3 mt-1' />").append(
                                                $("<div style='border:none;-webkit-box-shadow: none'>").dxButton({
                                                    icon: '/content/Images/RMONE/compareresume.png',
                                                    hint: 'Compare Resume',
                                                    onClick() {
                                                        OpenResumeCompare(data.TypeName);
                                                    },
                                                })
                                            ),
                                            $("<div id='openGanttView' class='pt-3 mt-1' />").append(
                                                $("<div style='border:none;-webkit-box-shadow: none'>").dxButton({
                                                    icon: '/content/Images/ganttBlackNew.png',
                                                    hint: 'Open Gantt View',
                                                    onClick() {
                                                        OpenUsersGanttView(ganttProjSD, ganttProjED, classNameSt, classNameEd, ganttProjReqAloc);
                                                    },
                                                })
                                            )
                                        )
                                    )
                                ),
                                $("<div class='shadow-effect' style='padding-bottom: 10px;' />").append(
                                    $("<div id='filterChecks' class='clearfix pb-2 pt-2' />").append(
                                        $("<Label class='clsSmartFilter' >Suggested Filters:</Label>"),
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
                                            text: "<%=Customer%>",
                                            visible: ("<%=Customer%>" != '' && "<%=Customer%>" != null) == true ? true : false,
                                            value: popupFilters.Customer,
                                            hint: 'Customer',
                                            onValueChanged: function (e) {
                                                popupFilters.CompanyLookup = "<%=CompanyLookup%>";
                                                popupFilters.Customer = e.value;
                                                bindDatapopup(popupFilters);
                                            },
                                        }),

                                        $("<div id='chkSector' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                            text: "<%=Sector%>",
                                            visible: ("<%=Sector%>" != '' && "<%=Sector%>" != null) == true ? true : false,
                                            hint: 'Sector',
                                            value: popupFilters.Sector,
                                            onValueChanged: function (e) {
                                                popupFilters.SectorName = "<%=Sector%>";
                                                popupFilters.Sector = e.value;
                                                bindDatapopup(popupFilters);
                                            },
                                        }),

                                        $("<div id='chkCertification' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                            text: "Certification",
                                            visible: true,
                                            hint: 'Certification',
                                            onValueChanged: function (e) {
                                                if (e.value) {
                                                    e.component.option("text", "");
                                                    $("#certificationTxt").dxTagBox("option", "visible", true);
                                                }
                                                else {
                                                    e.component.option("text", "Certification");
                                                    $("#certificationTxt").dxTagBox("option", "visible", false);
                                                    $("#certificationTxt").dxTagBox("option", "value", []);
                                                }
                                            },
                                        }),
                                        $("<div id='certificationTxt' class='filterctrl-jobDepartment' style='margin-top:-6px;padding-left:5px;' />").dxTagBox({  //dxSelectBox
                                            showSelectionControls: true,
                                            placeholder: "Certification",
                                            valueExpr: "ID",
                                            grouped: true,
                                            visible: false,
                                            displayExpr: "Title",
                                            dataSource: new DevExpress.data.DataSource({
                                                store: certifications,
                                                key: 'ID',
                                                group: 'CategoryName',
                                            }),
                                            maxDisplayedTags: 1,
                                            searchEnabled: true,
                                            onValueChanged: function (selectedItems) {
                                                popupFilters.SelectedCertifications = selectedItems.value.join();
                                                bindDatapopup(popupFilters);
                                            },
                                        }),
                                    ),

                                    $("<div class='clearfix pt-1' />").append(
                                        $("<Label class='clsSmartFilter pr-4' style='margin-top:10px;' >Tags:</Label>"),
                                        $('<div class="tag-container mr-3" style="border: 2px solid #ddd;min-height:45px;">').append(
                                            $('<div class="tag-container-2 mr-3">').append(
                                                $("<div id='addExpTags' style='border: none; -webkit-box-shadow: none' />").dxButton({
                                                    icon: "/Content/Images/plus-blue-new.png",
                                                    hint: "Reset Experience Tags",
                                                    visible: hasAccessToAddTags,
                                                    visible: allowGroupFilterOnExpTags,
                                                    focusStateEnabled: false,
                                                    onClick() {
                                                        popupFilters.SelectedTags = JSON.parse(JSON.stringify(projectExperiencTags));
                                                        RenderProjectTagsOnFrame();
                                                    }
                                                }),
                                                $('<div id="projectExpTags" style="display:flex;flex-wrap:wrap;" />'),
                                            ),
                                            $("<div id='clearExpTags' style='border: none; -webkit-box-shadow: none' />").dxButton({
                                                icon: "/Content/Images/RMONE/clear-icon.png",
                                                hint: "Clear Experience Tags",
                                                visible: allowGroupFilterOnExpTags,
                                                onClick: function () {
                                                    popupFilters.SelectedTags = [];
                                                    RenderProjectTagsOnFrame();
                                                }
                                            }),
                                        ),

                                    ),
                                    $("<div class='clearfix pb-1' />").append(
                                        $("<Label class='clsSmartFilter' style='margin-top:10px;' >Filters:</Label>"),
                                        $("<div id='dropdownFilters' class='d-flex justify-content-between' />").append(
                                            $("<div class='flex-grow-1 display-flex' />").append(

                                                $("<div class='filterctrl-jobDepartment' />").dxSelectBox({  //dxSelectBox
                                                    placeholder: "Division",
                                                    valueExpr: "ID",
                                                    displayExpr: "Title",
                                                    showClearButton: true,
                                                    dataSource: "/api/CoreRMM/GetDivisions?OnlyEnabled=1",
                                                    searchEnabled: true,
                                                    onValueChanged: function (selectedItems) {
                                                        let divisionId = '0';
                                                        if (selectedItems.value != null && selectedItems.value != '') {
                                                            divisionId = selectedItems.value;
                                                        }

                                                        popupFilters.DivisionId = divisionId;
                                                        $.get("/api/rmmapi/GetGroupTitles?GroupID=" + popupFilters.groupID + "&DivisionID=" + divisionId, function (data, status) {

                                                            JobTitleData = data;
                                                            var tagBox = $("#tagboxTitles").dxTagBox("instance");
                                                            tagBox.option("dataSource", JobTitleData);
                                                            tagBox.reset();
                                                        });

                                                        if (divisionId == '0') {
                                                            var tagBoxDepartment = $("#tagboxDepartment").dxTagBox("instance");
                                                            tagBoxDepartment.option("dataSource", []);
                                                            tagBoxDepartment.reset();
                                                        } else {
                                                            $.get("/api/rmmapi/GetDepartmentsForDivision?DivisionId=" + divisionId, function (data, status) {
                                                                var tagBoxDepartment = $("#tagboxDepartment").dxTagBox("instance");
                                                                tagBoxDepartment.option("dataSource", data);
                                                                tagBoxDepartment.reset();
                                                            });
                                                        }
                                                        bindDatapopup(popupFilters);
                                                    },
                                                }),
                                                $("<div id='tagboxDepartment' class='filterctrl-jobDepartment' />").dxTagBox({  //dxSelectBox
                                                    showSelectionControls: true,
                                                    placeholder: "Department",
                                                    valueExpr: "ID",
                                                    displayExpr: "Title",
                                                    maxDisplayedTags: 1,
                                                    searchEnabled: true,
                                                    onValueChanged: function (selectedItems) {
                                                        let departmentId = '0';
                                                        var items = selectedItems.component._selectedItems;
                                                        if (items.length > 0) {
                                                            var lstItems = items.map(function (i) {
                                                                return i.ID;
                                                            });
                                                            departmentId = lstItems.join(',');
                                                        }
                                                        popupFilters.DepartmentId = departmentId;
                                                        bindDatapopup(popupFilters);
                                                    },
                                                }),
                                                $("<div id='tagboxTitles' class='filterctrl-jobtitle' />").dxTagBox({
                                                    showSelectionControls: true,
                                                    text: "Job Titles",
                                                    placeholder: "Title",
                                                    searchEnabled: true,
                                                    dataSource: "/api/rmmapi/GetGroupTitles?GroupID=" + popupFilters.groupID + "&DivisionID=0",
                                                    maxDisplayedTags: 1,
                                                    onValueChanged: function (selectedItems) {
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

                                                $("<div class='filterctrl-jobDepartment' />").dxSelectBox({
                                                    dataSource: selectBoxItems,
                                                    displayExpr: "text",
                                                    value: _.findWhere(selectBoxItems, { value: popupFilters.resourceAvailability }),
                                                    layout: "horizontal",
                                                    onValueChanged: function (e) {
                                                        popupFilters.resourceAvailability = e.value.value;
                                                        bindDatapopup(popupFilters);
                                                    }
                                                }),

                                                $("<div class='filterctrl-userpicker' />").dxSelectBox({
                                                    placeholder: "Search team member",
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
                                            ),
                                        )
                                    )
                                ),
                                $(`<div id='tileViewContainer' style='clear:both;padding-bottom:10px;max-height:${windowHeight}px;' />`).dxTileView({
                                    width: window.innerWidth - 100, //150
                                    showScrollbar: true,
                                    baseItemHeight: 65,
                                    baseItemWidth: 150,
                                    itemMargin: 15,
                                    direction: "vertical",
                                    elementAttr: { "class": "tileViewContainer" },
                                    noDataText: "No resource available",
                                    itemTemplate: function (itemData, itemIndex, itemElement) {
                                        if (UserConflictData.filter(x => x.AssignedTo == itemData.AssignedTo).length == 0)
                                            UserConflictData.push(itemData);
                                        itemData.PctAllocation = Math.round(itemData.PctAllocation);
                                        itemData.SoftPctAllocation = Math.round(itemData.SoftPctAllocation);
                                        itemData.TotalPctAllocation = Math.round(itemData.TotalPctAllocation);
                                        var html = new Array();
                                        var str = itemData.AssignedTo + "','" + itemData.AssignedToName;
                                        if (itemData.ResourceTags != null) {
                                            itemData.ResourceTags.forEach(function (value, index) {
                                                if (CheckTagExist(value.Type, value.TagId)) {
                                                    //debugger;
                                                    let expTag = value.Type == 2 ? experienceTags.filter(x => x.ID == value.TagId)[0] : certifications.filter(x => x.ID == value.TagId)[0];
                                                    if (ResourceTagCount.length == 0 || ResourceTagCount.filter(x => x.TagId == value.TagId && x.Type == value.Type && x.UserId == itemData.AssignedTo).length == 0) {
                                                        ResourceTagCount.push({
                                                            TagId: value.TagId,
                                                            Title: expTag.Title,
                                                            TagCount: value.TagCount,
                                                            Type: value.Type,
                                                            UserId: itemData.AssignedTo,
                                                            UserName: itemData.AssignedToName,
                                                            Allocation: itemData.PctAllocation,
                                                            UserPicture: itemData.UserImagePath,
                                                            RoleName: itemData.RoleName,
                                                            Availability: (100 - Number(itemData.PctAllocation)),
                                                        });
                                                    }
                                                }
                                            });
                                        }
                                        var strwithspace = str.replace(/\s/g, '&nbsp;'); //str.replace(" ", "&nbsp;")
                                        //html.push("<div class='timesheet'><img src='/content/images/icon_three_black_dots.png' height='5px' title='Allocation Timeline' onclick=openResourceTimeSheet('" + strwithspace + "'); />");
                                        html.push("<label class='innerCheckbox' onclick=storeCheckedUser()>");
                                        html.push("<input type='checkbox' allocaionview='" + popupFilters.isAllocationView + "' pctallocation='" + itemData.PctAllocation + "' id='" + itemData.AssignedTo + "' name='" + itemData.JobTitle + "' onclick=storeCheckedUser()>");
                                        html.push("</label>");
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
                                        else if (!popupFilters.isAllocationView) {
                                            html.push("<div>");
                                            html.push("(" + (100 - Number(itemData.PctAllocation)) + "%)");
                                            //html.push("(" + (Number(itemData.PctAllocation)) + "%)");
                                            html.push("</div>");
                                        }
                                        if (popupFilters.isAllocationView && itemData.PctAllocation < 100) {
                                            html.push("<div>");
                                            html.push("(" + (Number(itemData.PctAllocation)) + "%)");
                                            html.push("</div>");
                                        }
                                        if (popupFilters.projectCount || popupFilters.projectVolume) {
                                            if (itemData.PctAllocation > 0) {
                                                html.push("<div class='capacitymain'>");
                                                html.push("<div class='capacityblock allocation-v" + itemData.TotalVolumeRange + "'>");
                                                html.push(itemData.TotalVolume == null ? "$0" : itemData.TotalVolume);
                                                html.push("</div>");
                                                html.push("<div class='capacityblock allocation-c" + itemData.projectCountRange + "''>");
                                                html.push("#" + itemData.ProjectCount);
                                                html.push("</div>");
                                                html.push("</div>");
                                            }
                                        }
                                        else {
                <%--if (itemData.SoftPctAllocation > 0) {
                    if (popupFilters.isAllocationView) {
                        html.push("<div class='capacitymain'>");
                        <%if (ShowTotalAllocationsInSearch) { %>
                        html.push("<div class='capacityblock cc allocation-v" + itemData.TotalVolumeRange + "'>");
                        html.push("T: " + Number(itemData.TotalPctAllocation) + "%");
                        html.push("</div>");
                        html.push("<div class='capacityblock allocation-c" + itemData.projectCountRange + "''>");
                        <%} else { %>
                        html.push("<div class='capacityblock-1 allocation-c" + itemData.projectCountRange + "''>");
                        <%}%>
                        html.push("S: " + Number(itemData.SoftPctAllocation) + "%");
                        html.push("</div>");
                        html.push("</div>");
                       
                    }
                    else {
                        html.push("<div class='capacitymain'>");
                         <%if (ShowTotalAllocationsInSearch) { %>
                        html.push("<div class='capacityblock cc allocation-v" + itemData.TotalVolumeRange + "'>");
                        html.push("T: " + (100 - Number(itemData.TotalPctAllocation)) + "%");
                        html.push("</div>");
                        html.push("<div class='capacityblock allocation-c" + itemData.projectCountRange + "''>");
                        <%} else { %>
                        html.push("<div class='capacityblock-1 allocation-c" + itemData.projectCountRange + "''>");
                        <%}%>
                        html.push("<div class='capacityblock allocation-c" + itemData.projectCountRange + "''>");
                        html.push("S: " + (100 - Number(itemData.SoftPctAllocation)) + "%");
                        html.push("</div>");
                        html.push("</div>");

                    }
                }--%>

                                        }
                                        if (itemData.WeekWiseAllocations?.length > 0 && itemData.WeekWiseAllocations?.filter(x => !x.IsAvailable).length > 0) {
                                            html.push(`<div style='cursor:pointer;' onclick='storeCheckedUser();OpenConflictWeekData(${JSON.stringify(itemData.WeekWiseAllocations).replaceAll("'", "`")}, "${itemData.AssignedToName.replaceAll("'", "`")}", "${itemData.AssignedTo}", "${itemData.UserImagePath}");'>`);
                                            html.push("<div class='conflict-circle'>");
                                            html.push(itemData.WeekWiseAllocations.filter(x => !x.IsAvailable).length);
                                            html.push("</div>");
                                            html.push("</div>");
                                        }
                                        html.push("</div>");
                                        html.push("</div>");
                                        itemElement.attr("class", "allocation-block allocation-r" + itemData.AllocationRange);
                                        //itemElement.attr("title", itemData.JobTitle);
                                        itemElement.attr("id", "allocation-block" + itemIndex);
                                        itemElement.attr("onmouseover", "showTooltip('allocation-block" + itemIndex + "','" + itemData.AssignedTo + "')");
                                        itemElement.attr("onmouseout", "hideTooltip()");
                                        itemElement.append(html.join(""));

                                    },
                                    onItemClick: function (e) {
                                        //debugger;
                                        let checkedUser = 0;
                                        var data = e.itemData;
                                        $(".innerCheckbox input").each(function () {
                                            if ($(this).is(":checked")) {
                                                checkedUser++;
                                            }
                                        });
                                        if (checkedUser > 0) {
                                            if (showTimeSheet == false)
                                                $("#" + data.AssignedTo).click();
                                            if (showTimeSheet == true)
                                                showTimeSheet = false;
                                        }
                                        else {
                                            if (showTimeSheet == true) {
                                                showTimeSheet = false;
                                                return;
                                            }
                                            if (UserConflictData.filter(x => x.WeekWiseAllocations?.every(y => y.IsAvailable))?.length > 0 && !data.WeekWiseAllocations?.every(x => x.IsAvailable)) {
                                                //alert("conflict in summary");
                                                var isSummary = false;
                                                if ($("#btnDetailedSummary").text() == "Detailed View") {
                                                    isSummary = true;
                                                }
                                                var conflictDialog = DevExpress.ui.dialog.custom({
                                                    title: "Alert",
                                                    message: `${data.AssignedToName.replaceAll("'", "`")} has conflict weeks for the current duration. Do you want to proceed?`,
                                                    buttons: [
                                                        { text: "Yes", onClick: function () { return "Ok" }, elementAttr: { "class": "btnBlue" } },
                                                        { text: "No", onClick: function () { return "Cancel" }, elementAttr: { "class": "btnNormal" } }
                                                    ]
                                                });
                                                conflictDialog.show().done(function (dialogResult) {
                                                    if (dialogResult == "Ok") {
                                                        AllocateResourceToAllocation(data, null, isStartEndDateLesserThanCurrDate, isSummary);
                                                    }
                                                    else if (dialogResult == "Cancel") {
                                                        OpenConflictWeekData(data.WeekWiseAllocations, data.AssignedToName.replaceAll("'", "`"), data.AssignedTo, data.UserImageUrl);
                                                        return false;
                                                    }
                                                    else
                                                        return false;
                                                });
                                            }
                                            else {
                                                //alert("test2");
                                                var isSummary = false;
                                                if ($("#btnDetailedSummary").text() == "Detailed View") {
                                                    isSummary = true;
                                                }
                                                AllocateResourceToAllocation(data, null, isStartEndDateLesserThanCurrDate, isSummary);
                                            }
                                                
                                        }
                                    },
                                    noDataText: function (e) {
                                        $('.dx-empty-message').html('No resource available');
                                        $("#popuploader").dxLoadPanel({
                                            message: "Loading...",
                                            visible: false
                                        });
                                    }
                                })

                            )
                        },
                        onContentReady: function () {
                            bindDatapopup(popupFilters);
                        },
                        itemClick: function (e) {
                        },
                        onHiding: function (e) {
                            popupFilters.SelectedTags = [];
                            popupFilters.SelectedCertifications = "";
                        }
                    });
                    RenderProjectTagsOnFrame(false);
                    var popupInstance = $('#popupContainer').dxPopup('instance');
                    popupInstance.option('title', popupTitle);

                }
            } else {
                DevExpress.ui.dialog.alert(`Cannot edit past schedules.`, 'Warning!');
            }
        }
        
    });

    function moveLeft() {
        $('#divProjectView').animate({ scrollLeft: '-=' + $("#divProjectView").width() }, 1000);
        setTimeout(checkScrollEnd, 1500);
        }
    function moveRight() {
        $('#divProjectView').animate({ scrollLeft: '+=' + $("#divProjectView").width() }, 1000);
        setTimeout(checkScrollEnd, 1500);
    }
    function checkScrollEnd() {
        let elem = $('#divProjectView');
        if (elem[0].scrollWidth - elem.scrollLeft() <= Math.ceil(elem.outerWidth())) {
            $("#myProjectRightIcon").css("visibility", "hidden");
        }
        else {
            $("#myProjectRightIcon").css("visibility", "visible");
        }
        if (elem.scrollLeft() == 0) {
            $("#myProjectLeftIcon").css("visibility", "hidden");
        }
        else {
            $("#myProjectLeftIcon").css("visibility", "visible");
        }
    }
    $(document).on("click", "img.imgLocked", function (e) {
        var ID = $(this).attr("ID");
        var IsLocked = $(this).attr("IsLocked");
        if (IsLocked == "true") {
            $(this).attr("src", "/content/images/RMONE/Unlocked-image.png");
            $(this).attr("IsLocked", "false");
        }
        else {
            $(this).attr("src", "/content/images/RMONE/Locked-image.png");
            $(this).attr("IsLocked", "true");
        }
        globaldata.forEach(function (part, index, theArray) {
            if (part.ID == ID) {
                part.IsLocked = !part.IsLocked;
            }
        });
        var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
        dataGrid.option("dataSource", globaldata);
        dataGrid._refresh();
    });

    $(document).on("click", "img.imgDelete", function (e) {

        var userID = $(this).attr("UserID");
        var UserName = $(this).attr("UserName");
        var TypeName = $(this).attr("TypeName");
        var Tags = $(this).attr("Tags");
        var dataid = e.target.id;
        var result;
        if (UserName == '')
            result = DevExpress.ui.dialog.confirm(`Do you want to delete this allocation?`, 'Confirm');
        else
            result = DevExpress.ui.dialog.confirm(`Do you want to delete the allocation for ${UserName} for ${TypeName}?`, 'Confirm');
        var popup = $(document.getElementsByClassName('dx-popup-inherit-height')[0]);
        popup.addClass("PopupCustomPosition");
        result.done(function (confirmation) {
            if (confirmation) {
                if ($("#gridTemplateContainer").is(":visible")) {
                    globaldata = _.reject(globaldata, function (globaldata) { return globaldata.ID == dataid; });
                    var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                    dataGrid.option("dataSource", globaldata);
                    deletedData.push({ "ID": dataid, "TicketID": projectID, "UserID": userID, "TypeName": TypeName, "UserName": UserName, "Tags": Tags });
                    $('#btnCancelChanges').dxButton({ visible: true });
                }
                else {
                    var compactElement = _.findWhere(compactTempData, { ID: parseInt(dataid) });
                    globaldata = _.reject(globaldata, function (globaldata) { return globaldata.ID == compactElement.ID || globaldata.ID == compactElement.ConstId || globaldata.ID == compactElement.CloseOutId; });
                    compactTempData = _.reject(compactTempData, function (compactTempData) { return compactTempData.ID == dataid; });
                    var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                    dataGrid.option("dataSource", globaldata);
                    var compactgrid = $('#compactGridTemplateContainer').dxDataGrid('instance');
                    compactgrid.option('dataSource', compactTempData);
                    compactgrid._refresh();

                    deletedData.push({ "ID": compactElement.ID, "TicketID": projectID, "UserID": userID, "TypeName": TypeName, "UserName": UserName, "Tags": Tags });
                    deletedData.push({ "ID": compactElement.ConstId, "TicketID": projectID, "UserID": userID, "TypeName": TypeName, "UserName": UserName, "Tags": Tags });
                    deletedData.push({ "ID": compactElement.CloseOutId, "TicketID": projectID, "UserID": userID, "TypeName": TypeName, "UserName": UserName, "Tags": Tags });
                    $('#btnCancelChanges').dxButton({ visible: true });
                }

            }
        });
    });

    $(document).on("click", "img.imgDeleteNew", function (e) {
        var UserName = $(this).attr("UserName");
        var TypeName = $(this).attr("TypeName");
        var userID = $(this).attr("UserID");
        var ProjEstID = $(this).attr("ID");
        var Tags = $(this).attr("Tags");
        var TicketID = projectID;
        var lstData = [];
        var StartDate = new Date($(this).attr("StartDate"));
        var EndDate = new Date($(this).attr("EndDate"));
        var dataid = e.target.id;
        if ($("#gridTemplateContainer").is(":visible")) {
            lstData.push({ ID: ProjEstID, TicketID: TicketID, UserID: userID, UserName: UserName, RoleName: TypeName });
        }
        else {
            var compactElement = _.findWhere(compactTempData, { ID: parseInt(dataid) });
            let uniqueIds = GetUniqueIds(compactElement);
            if (uniqueIds != null && uniqueIds.length > 0) {
                uniqueIds.forEach(function (id) {
                    lstData.push({ "ID": id, "TicketID": projectID, "UserID": userID, "TypeName": TypeName, "UserName": UserName });
                });
            }
        }
        var message;
        if (parseInt(ProjEstID) < 0) {
            RefreshGrids(lstData)
            //globaldata = globaldata.filter(x => x.ID != parseInt(ProjEstID))
            //var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
            //dataGrid.option("dataSource", globaldata);
            //dataGrid._refresh();
            return;
        }
        if (UserName == '')
            message = "Do you want to delete the allocation (" + StartDate.format("MMM d,yyyy") + " - " + EndDate.format("MMM d,yyyy") + ") for " + TypeName + "?";
        else
            message = "Do you want to delete the allocation (" + StartDate.format("MMM d,yyyy") + " - " + EndDate.format("MMM d,yyyy") + ") for " + UserName + " for " + TypeName + "?";
        
        DevExpress.ui.dialog.confirm({
            messageHtml: message,/*"Do you want to delete?"*/
            title: "Confirm",
            buttons: [
                {
                    text: "Yes", onClick: function (e) {
                        if (parseInt(ProjEstID) < 0) {
                            RefreshGrids(lstData)
                            //globaldata = globaldata.filter(x => x.ID != parseInt(ProjEstID))
                            //var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                            //dataGrid.option("dataSource", globaldata);
                            //dataGrid._refresh();
                            return;
                        }
                        var popup = $(document.getElementsByClassName('dx-popup-inherit-height')[0]);
                        popup.addClass("PopupCustomPosition");
                        $("#loadpanel").dxLoadPanel({
                            message: "Deleting record please wait ...",
                            visible: true,
                            hideOnOutsideClick: false
                        });
                        $.ajax({
                            type: "post",
                            url: "/api/rmmapi/DeleteAllocation_New",
                            async: true,
                            contentType: 'application/json; charset=utf-8',
                            //data: { ID: ProjEstID, TicketID: TicketID, UserID: userID, UserName: UserName, RoleName: TypeName },
                            data: JSON.stringify(lstData),
                            success: function (data) {
                                $("#loadpanel").dxLoadPanel("hide");
                                $.post("/api/rmmapi/AddProjectExperienceTagList/", projectExperiencModel).then(function (response) {
                                    
                                });
                                RefreshGrids(lstData)
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                
                            }
                        });
                    }, elementAttr: { "class": "btnBlue" }
                },
                {
                    text: "Cancel", onClick: function (e) {
                        // do nothing
                    }, elementAttr: { "class": "btnNormal" }
                }
            ]
        });
    });

    function isAllAllocationsSame(options) {
        let uniqueIds = GetUniqueIds(options.data);
        return globaldata.filter(x => uniqueIds.includes(String(x.ID))).every(value => value.SoftAllocation === options.data.SoftAllocation);
    }

    function isAllNCOAllocationsSame(options) {
        let uniqueIds = GetUniqueIds(options.data);
        return globaldata.filter(x => uniqueIds.includes(String(x.ID))).every(value => value.NonChargeable === options.data.NonChargeable);
    }

    function GetPctAllocationAcrossPhases(compactElementId) {
        let compactElementData = _.findWhere(compactTempData, { ID: parseInt(compactElementId) });
        let uniqueIds = GetUniqueIds(compactElementData);
        let data = JSON.parse(JSON.stringify(globaldata.filter(x => uniqueIds.includes(String(x.ID)))));
        let maxDate = new Date(Math.max.apply(null, data.map(x => new Date(x.AllocationEndDate))))
        let minDate = new Date(Math.min.apply(null, data.map(x => new Date(x.AllocationStartDate))))
        return CalculatePctAllocationNew(data);
    }

    function GetUniqueIds(compactElement) {
        let ids = [];
        if (parseInt(compactElement.ID) != 0) {
            ids.push(String(compactElement.ID));
        }
        if (parseInt(compactElement.ConstId) != 0) {
            ids.push(String(compactElement.ConstId));
        }
        if (parseInt(compactElement.CloseOutId) != 0) {
            ids.push(String(compactElement.CloseOutId));
        }
        if (compactElement.closeOutRefIds != undefined && compactElement.closeOutRefIds != '') {
            let rids = String(compactElement.closeOutRefIds).split(";");
            if (rids != null && rids.length > 0) {
                rids.forEach(function (id) {
                    ids.push(String(id));
                });
            }
        }
        if (compactElement.preconRefIds != undefined && compactElement.preconRefIds != '') {
            let rids = String(compactElement.preconRefIds).split(";");
            if (rids != null && rids.length > 0) {
                rids.forEach(function (id) {
                    ids.push(String(id));
                });
            }
        }
        if (compactElement.constRefIds != undefined && compactElement.constRefIds != '') {
            let rids = String(compactElement.constRefIds).split(";");
            if (rids != null && rids.length > 0) {
                rids.forEach(function (id) {
                    ids.push(String(id));
                });
            }
        }
        return [...new Set(ids)];
    }

    // Only used >> If users enter 0 in % Allocation then this method will call.
    function DeleteAllocation(data, refIds) {
        var UserName = data.AssignedToName;
        var TypeName = data.TypeName;
        var userID = data.AssignedTo;;
        var ProjEstID = data.ID;
        var Tags = data.Tags;
        var TicketID = projectID;
        var lstData = [];
        var StartDate = new Date(data.AllocationStartDate);
        var EndDate = new Date(data.AllocationEndDate);

        lstData.push({ ID: ProjEstID, TicketID: TicketID, UserID: userID, UserName: UserName, RoleName: TypeName });
        
        var message;
        
        if (UserName == '')
            message = "Do you want to delete the allocation (" + StartDate.format("MMM d,yyyy") + " - " + EndDate.format("MMM d,yyyy") + ") for " + TypeName + "?";
        else
            message = "Do you want to delete the allocation (" + StartDate.format("MMM d,yyyy") + " - " + EndDate.format("MMM d,yyyy") + ") for " + UserName + " for " + TypeName + "?";
        if (parseInt(ProjEstID) < 0) {
            globaldata = globaldata.filter(x => x.ID != ProjEstID);
            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
            dataGrid.option("dataSource", globaldata);
            dataGrid._refresh();
            if (refIds != undefined) {
                let gdata = globaldata.filter(x => refIds.includes(String(x.ID)));
                let dataGridChild = $("#InternalAllocationGrid").dxDataGrid("instance");
                dataGridChild.option("dataSource", gdata);
            }
            //alert("step8");
            CompactPhaseConstraints();
        }
        else {
            DevExpress.ui.dialog.confirm({
                messageHtml: message,/*"Do you want to delete?"*/
                title: "Confirm",
                buttons: [
                    {
                        text: "Yes", onClick: function (e) {
                            var popup = $(document.getElementsByClassName('dx-popup-inherit-height')[0]);
                            popup.addClass("PopupCustomPosition");
                            $("#loadpanel").dxLoadPanel({
                                message: "Deleting record please wait ...",
                                visible: true,
                                hideOnOutsideClick: false
                            });

                            $.ajax({
                                type: "post",
                                url: "/api/rmmapi/DeleteAllocation_New",
                                async: true,
                                contentType: 'application/json; charset=utf-8',
                                data: JSON.stringify(lstData),
                                success: function (data) {
                                    $("#loadpanel").dxLoadPanel("hide");

                                    $.post("/api/rmmapi/AddProjectExperienceTagList/", projectExperiencModel).then(function (response) {

                                    });
                                    globaldata = globaldata.filter(x => x.ID != ProjEstID);
                                    var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                                    dataGrid.option("dataSource", globaldata);
                                    dataGrid._refresh();
                                    if (refIds != undefined) {
                                        let gdata = globaldata.filter(x => refIds.includes(String(x.ID)));
                                        let dataGridChild = $("#InternalAllocationGrid").dxDataGrid("instance");
                                        dataGridChild.option("dataSource", gdata);
                                        tempGloblaDataStorage = JSON.parse(JSON.stringify(globaldata));
                                    }
                                    //alert("step9");
                                    CompactPhaseConstraints();
                                },
                                error: function (xhr, ajaxOptions, thrownError) {

                                }
                            });
                        }, elementAttr: { "class": "btnBlue" }
                    },
                    {
                        text: "Cancel", onClick: function (e) {

                        }, elementAttr: { "class": "btnNormal" }
                    }
                ]
            });
        }
    }

    $(document).on("click", "img.imgPreconDate", function (e) {
        //var index = $(this).attr("Index");
        var result = DevExpress.ui.dialog.confirm('Are you sure you want to switch to Precon Dates?', 'Confirm');
        var popup = $(document.getElementsByClassName('dx-popup-inherit-height')[0]);
        popup.addClass("PopupCustomPosition");
        result.done(function (confirmation) {
            if (confirmation) {
                var startDatebox = $("#dtStartDate").dxDateBox("instance");
                var endDatebox = $("#dtEndDate").dxDateBox("instance");

                var preconStartdate = dataModel.preconStartDate;
                var preconEnddate = dataModel.preconEndDate;
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

                var constStartdate = dataModel.constStartDate;
                var constEnddate = dataModel.constEndDate;
                startDatebox.option("value", constStartdate);
                endDatebox.option("value", constEnddate);

            }
        });
    });

    $.get("/api/rmmapi/GetGroupTitles?GroupID=" + popupFilters.groupID + "&DivisionID=0", function (data, status) {
        JobTitleData = data;
    });

    function storeCheckedUser() {
        showTimeSheet = true;
    }

    function CheckAndUpdatePastAllocations(allocationID = null) {
        //debugger;
        let tempdata = JSON.parse(JSON.stringify(globaldata));
        var allocations = [];
        let newIds = [];
        if (allocationID == null) {
            allocations = popupFilters.Allocations;
        } else {
            var allocation = globaldata.filter(x => x.ID == allocationID)[0];
            if (!(new Date(allocation.AllocationStartDate) < new Date() && new Date(allocation.AllocationEndDate) < new Date())) {
                allocations.push({ StartDate: new Date(allocation.AllocationStartDate) < new Date() ? new Date().format('yyyy-MM-dd') + "T00:00:00" : allocation.AllocationStartDate, EndDate: allocation.AllocationEndDate, RequiredPctAllocation: parseFloat(allocation.PctAllocation), ID: allocation.ID });
            }
        }
        allocations.forEach(x => {
            let sDate = new Date(x.StartDate);
            let eDate = new Date(x.EndDate);
            let alloc = tempdata.find(y => y.ID == x.ID);
            if (alloc != null) {
                if (new Date(alloc.AllocationStartDate) < sDate) {
                    if (alloc.AssignedToName != null && alloc.AssignedToName != '' && alloc.AssignedToName.length > 0) {
                        let alloc2 = JSON.parse(JSON.stringify(alloc));
                        alloc2.ID = -Math.abs(tempdata.length + 1);
                        newIds.push(String(alloc2.ID));
                        alloc2.AllocationStartDate = sDate.format('yyyy-MM-dd') + "T00:00:00";
                        tempdata.push(alloc2);
                        alloc.AllocationEndDate = new Date().addDays(-1).format('yyyy-MM-dd') + "T00:00:00";
                        updateAllProjectAllocations = false;
                        useThreading = false;
                    } else {
                        alloc.AllocationStartDate = sDate.format('yyyy-MM-dd') + "T00:00:00";
                        newIds.push(String(alloc.ID));
                    }
                }
                else {
                    newIds.push(String(alloc.ID));
                }
            }
        });
        globaldata = JSON.parse(JSON.stringify(tempdata));
        return newIds;
    }
    function AddOrUpdateGlobalData(isExistingAlloc, sDate, eDate, AllocID, conflictUserId, conflictUserName, conflictUserImage, newUserId, newUserName, newUserImageUrl, isAvailable) {
        //debugger;
        let data, ID;
        if (isExistingAlloc) {
            data = tempGlobaldata.filter(z => z.ID == AllocID)[0];
        }
        else {
            ID = -Math.abs(tempGlobaldata.length + 1);
            data = JSON.parse(JSON.stringify(tempGlobaldata.filter(z => z.ID == AllocID)[0]));
            data.ID = ID;
            if (tempRefIds.length > 0 && tempRefIds.find(x => String(x) == String(data.ID)) == null) {
                tempRefIds.push(String(data.ID));
            }
        }
        data.AllocationStartDate = sDate.format('yyyy-MM-dd') + "T00:00:00";
        data.AllocationEndDate = eDate.format('yyyy-MM-dd') + "T00:00:00";
        if (isAvailable) {
            data.AssignedTo = newUserId;
            data.AssignedToName = newUserName;
            data.UserImageUrl = newUserImageUrl;
        }
        else {
            data.AssignedTo = conflictUserId;
            data.AssignedToName = conflictUserName;
            data.UserImageUrl = conflictUserImage;
        }

        let obj = globaldata.find(o => o.AssignedTo == data.AssignedTo && o.Type == data.Type && new Date(data.AllocationStartDate) <= new Date(o.AllocationEndDate) && new Date(data.AllocationEndDate) >= new Date(o.AllocationStartDate));

        if (!isExistingAlloc) {
            tempGlobaldata.push(data);
        }
        return { "UserName": data.AssignedToName, "IsOverlapping": obj != null && obj != undefined ? true : false };
    }

    //Popup message changes added parameters like newAllocationWithPastSDate, filteredUsers, isSummary, selectedID
    function SplitConflictAllocations(data, conflictUserId, conflictUserName, newUserId, newUserName, newUserImageUrl, isOriginatedFromFindTeams = false, allocationID = null, newAllocationWithPastSDate, filteredUsers, isSummary, selectedID, conflictUserImage) {
        //debugger;        
        let OverLappingData = [];
        let newIds = CheckAndUpdatePastAllocations(allocationID);
        let refIdscounter = 0;
        tempGlobaldata = JSON.parse(JSON.stringify(globaldata));
        let elements = tempGlobaldata.filter(z => newIds.includes(String(z.ID)));
        var allocations = [];
        //Popup message changes added for summary view
        if (isSummary == true) {
            if (filteredUsers != null && filteredUsers != undefined) {
                compactTempCopy = compactTempData;
                let alloc = null;
                alloc = globaldata.find(x => x.ID == selectedID);
                alloc.AssignedTo = filteredUsers.AssignedTo;
                alloc.AssignedToName = filteredUsers.AssignedToName;
                alloc.UserImageUrl = filteredUsers.UserImagePath;
                alloc.IsResourceDisabled = false;
                $("#ConflictWeekDataGridDialog").dxPopup('instance').hide();
                var ddlSelectedResource = globaldata.find(x => x.ID == selectedID);;
                AllocateResource(selectedID, ddlSelectedResource, isSummary);
            }
        }            
        else if (newAllocationWithPastSDate != undefined && newAllocationWithPastSDate == true) {
            //Popup message changes added for detailed view
            if (filteredUsers != null && filteredUsers != undefined) {
                let alloc = null;
                alloc = globaldata.find(x => x.ID == newIds);
                alloc.AssignedTo = filteredUsers.AssignedTo;
                alloc.AssignedToName = filteredUsers.AssignedToName;
                alloc.UserImageUrl = filteredUsers.UserImagePath;
                alloc.IsResourceDisabled = false;
                $("#ConflictWeekDataGridDialog").dxPopup('instance').hide();
                var ddlSelectedResource = globaldata.find(x => x.ID == newIds);;
                AllocateResource(newIds, ddlSelectedResource);
            }
        }
        else {
            if (!isOriginatedFromFindTeams) {
                allocations = popupFilters.Allocations;
            } else {
                var allocation = globaldata.filter(x => x.ID == allocationID)[0];
                if (!(new Date(allocation.AllocationStartDate) < new Date() && new Date(allocation.AllocationEndDate) < new Date())) {
                    allocations.push({ StartDate: new Date(allocation.AllocationStartDate) < new Date() ? new Date().format('yyyy-MM-dd') + "T00:00:00" : allocation.AllocationStartDate, EndDate: allocation.AllocationEndDate, RequiredPctAllocation: parseFloat(allocation.PctAllocation), ID: allocation.ID });
                }
            }
            allocations.forEach(x => {
                let sDate = new Date(x.StartDate);
                let eDate = new Date(x.EndDate);
                let eId = x.ID;
                elements.forEach(function (element) {
                    if (new Date(element.AllocationStartDate).format('yyyy-MM-dd') == sDate.format('yyyy-MM-dd') && new Date(element.AllocationEndDate).format('yyyy-MM-dd') == eDate.format('yyyy-MM-dd')) {
                        eId = element.ID;
                    }
                });
                let tempSDate = sDate;
                let tempEDate = eDate;
                let pAvailable = true;
                let isExistingAlloc = true;
                let counter = 0;
                data.forEach((y, index) => {
                    if (new Date(y.WeekStartDate) >= sDate && new Date(y.WeekStartDate) <= eDate) {
                        if (counter == 0) {
                            pAvailable = y.IsAvailable;
                            tempSDate = new Date(sDate);
                            counter++;
                        }
                        else {
                            if (pAvailable != y.IsAvailable) {
                                OverLappingData.push(AddOrUpdateGlobalData(isExistingAlloc, tempSDate, new Date(y.WeekStartDate).addDays(-3), eId, conflictUserId
                                    , conflictUserName, conflictUserImage, newUserId, newUserName, newUserImageUrl, pAvailable));
                                if (!isExistingAlloc) {
                                    refIdscounter++;
                                }
                                if (isExistingAlloc) {
                                    isExistingAlloc = false;
                                }

                                pAvailable = y.IsAvailable;
                                tempSDate = new Date(y.WeekStartDate);
                            }
                        }
                    }
                    if (data.length == index + 1) {
                        OverLappingData.push(AddOrUpdateGlobalData(isExistingAlloc, tempSDate, eDate, eId, conflictUserId
                            , conflictUserName, conflictUserImage, newUserId, newUserName, newUserImageUrl, pAvailable));
                        if (!isExistingAlloc) {
                            refIdscounter++;
                        }
                        if (isExistingAlloc) {
                            isExistingAlloc = false;
                        }
                    }
                });
            });
            let filteredData = OverLappingData.filter(x => x.IsOverlapping).map(x => x.UserName);

            if (filteredData != null && filteredData != undefined && filteredData.length > 0) {
                var customDialog = DevExpress.ui.dialog.custom({
                    title: "Alert",
                    message: `An overlapping allocation exists for ${filteredData.join(" and ")}. Do you want to proceed?`,
                    buttons: [
                        { text: "Ok", onClick: function () { return "Ok" } },
                        { text: "Cancel", onClick: function () { return "Cancel" } }
                    ]
                });
                customDialog.show().done(function (dialogResult) {
                    if (dialogResult == "Ok") {
                        $.cookie("dataChanged", 1, { path: "/" });
                        $('#btnCancelChanges').dxButton({ visible: true });
                        globaldata = tempGlobaldata;
                        //alert("step10");
                        CompactPhaseConstraints();
                        $("#ConflictWeekDataGridDialog").dxPopup('instance').hide();
                        AllocateResource();
                    }
                    else if (dialogResult == "Cancel") {
                        tempGlobaldata = [];
                        tempRefIds = tempRefIds.filter((x, i) => i + refIdscounter < tempRefIds.length);
                        return false;
                    }
                    else {
                        tempGlobaldata = [];
                        tempRefIds = tempRefIds.filter((x, i) => i + refIdscounter < tempRefIds.length);
                        return false;
                    }
                });

                return false;
            }
            else {
                $.cookie("dataChanged", 1, { path: "/" });
                $('#btnCancelChanges').dxButton({ visible: true });
                globaldata = tempGlobaldata;
                //alert("step11");
                CompactPhaseConstraints();
                $("#ConflictWeekDataGridDialog").dxPopup('instance').hide();
                AllocateResource();
            }
        }
    }
    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }

    function preventClick() {
        event.stopPropagation();

    }

    function callbackOpenConflictWeekData(index, roleID, itemIndex, allocationID) {
        var titleViewObj = $('#tileViewContainer' + index + '_' + roleID).dxTileView('instance');
        if (titleViewObj) {
            var dataSource = titleViewObj.option("dataSource");
            var uniqueResources = [];
            dataSource.forEach((ele) => {
                if (uniqueResources.filter(x => x.AssignedTo == ele.AssignedTo).length == 0)
                    uniqueResources.push(ele);
            });
            var data = dataSource[itemIndex];
            OpenConflictWeekData(data.WeekWiseAllocations, data.AssignedToName.replaceAll("'", "`"), data.AssignedTo, data.UserImageUrl, uniqueResources, allocationID);
            event.stopPropagation();
        };
    };
    function OpenConflictWeekData(data, name, userId, UserImageUrl, uniqueResources = null, allocationID = null) {
        //debugger;
        let conflictData = data.filter(x => !x.IsAvailable);
        var selectedYear = new Date(Math.min.apply(null, conflictData.map(x => new Date(x.WeekStartDate)))).getFullYear();
        let filteredUsers = [];
        if (uniqueResources != null) {
            filteredUsers = uniqueResources.filter(x => x.WeekWiseAllocations.every(y => y.IsAvailable));
        } else {
            filteredUsers = UserConflictData.filter(x => x.WeekWiseAllocations.every(y => y.IsAvailable));
        }
        const ConflictWeekDataGridTemplate = function () {
            let container = $("<div>");
            container.append(
                $(`<div style='margin-bottom:10px;'>Weekly 'Alloc %' shown includes prospective <span onclick="${ticketUrl.replaceAll("\t", "")}" style='color:#4fa1d6;text-decoration:underline;cursor:pointer;'>${cmicNumber}</span> allocation</div>`)
            );
            let windowHeight = parseInt(70 * $(window).height()/100);
            let datagrid = $(`<div id='ConflictWeekDataGrid' style='max-height:${windowHeight}px'>`).dxDataGrid({
                dataSource: conflictData,
                ID: "grdConflictWeekData",
                editing: {
                    mode: "cell",
                    allowEditing: true,
                    allowUpdating: true
                },
                sorting: {
                    mode: "multiple"
                },
                paging: {
                    enabled: false,
                },
                scrolling: {
                    mode: 'Standard',
                },
                columns: [
                    {
                        dataField: "WeekStartDate",
                        dataType: "date",
                        caption: "Start Date",
                        allowEditing: false,
                        sortIndex: "0",
                        sortOrder: "asc",
                        format: 'MMM d, yyyy',
                    },
                    {
                        caption: "End Date",
                        dataType: "date",
                        format: 'MMM d, yyyy',
                        calculateCellValue: function (rowData) {
                            return new Date(rowData.WeekStartDate).addDays(parseInt(rowData.NoOfDays) - 1);
                        }
                    },
                    {
                        dataField: "PctAllocation",
                        caption: "% Alloc",
                        dataType: "text",
                        width: "20%",
                        allowEditing: false,
                    },
                    {
                        dataField: "PostPctAllocation",
                        caption: "% Post Alloc",
                        dataType: "text",
                        width: "20%",
                        allowEditing: false,
                    }
                ],
                onCellClick: function (e) {
                    OpenConflictWeekDetailSummary(e.data.WeekdetailedSummaries);
                },
                onRowPrepared: function (info) {
                    if (info.rowType === 'data') {
                        info.rowElement.css("cursor", 'pointer');
                    }
                },
                onCellPrepared: function (e) {

                    if (e.rowType === 'data') {
                        var preconstartDate = new Date(dataModel.preconStartDate);
                        var preconEndDate = new Date(dataModel.preconEndDate);

                        var conststartDate = new Date(dataModel.constStartDate);
                        var constEndDate = new Date(dataModel.constEndDate);

                        var closeoutstartDate = new Date(dataModel.closeoutStartDate);
                        var closeoutEndDate = new Date(dataModel.closeoutEndDate);

                        if (e.column.dataField == 'WeekStartDate') {
                            let cellValue = new Date(e.data.WeekStartDate)
                            let className = getDateBackgroundColor(cellValue, preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                            e.cellElement.addClass(className);
                        }
                        if (typeof e.column.dataField == "undefined") {
                            let cellValue = new Date(e.data.WeekStartDate).addDays(parseInt(e.data.NoOfDays) - 1);
                            let className = getDateBackgroundColor(cellValue, preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                            e.cellElement.addClass(className);
                        }                        
                    }

                    if (e.row == undefined) return;
                    if (e.row.key.WeekdetailedSummaries?.length > 1 && e.column.name == "PostPctAllocation") {
                        e.cellElement.addClass('color-style');
                    }
                },
                showBorders: true,
                showRowLines: true,
            });
            container.append(datagrid);
            if (filteredUsers != null && filteredUsers.length > 0) {
                container.append(
                    $(`<div style="margin-top: 20px;border-top: 2px solid #ddd;padding-top: 10px;">Do you want to proceed with available resources for conflict weeks?</div>`)
                );

                let rowData = $("<div class='d-flex justify-content-between align-items-end'>").append(
                    $("<div id='conflictUserdd' class='filterctrl-jobDepartment' style='width:80%;padding-left:0px;' />").dxSelectBox({
                        dataSource: new DevExpress.data.DataSource({
                            store: filteredUsers,
                            sort: 'AssignedToName'
                        }),
                        displayExpr: 'AssignedToName',
                        valueExpr: 'AssignedTo',
                        placeholder: 'Select User',
                        layout: "horizontal",
                        onValueChanged: function (e) {
                            $("#btnProceed").dxButton('instance').option("visible", true);
                        }
                    }),

                    $(`<div id='btnProceed' style="padding: 2px 4px;width:18%;height:36px;" class="btnAddNew" />`).dxButton({
                        text: "Submit",
                        visible: false,
                        onClick: function () {
                            //debugger;
                            //alert("test222");
                            //Popup message changes added for summary view
                            var isSummary = false;
                            if ($("#btnDetailedSummary").text() == "Detailed View") {
                                isSummary = true;
                            }
                            if (isSummary == true && new Date(oldStartDate) < curDate && new Date(oldEndDate) > curDate && oldAssignTo == "") {
                                let userDD = $("#conflictUserdd").dxSelectBox('instance');
                                FilUser = filteredUsers.find(x => x.AssignedTo == userDD.option("value"));
                                SplitConflictAllocations(data, userDD.option("value"), userDD.option("text"), userId, name, UserImageUrl, uniqueResources != null ? true : false, uniqueResources != null ? allocationID : null, true, FilUser, isSummary, selectedID);
                            }
                            else if (new Date(oldStartDate) < curDate && new Date(oldEndDate) > curDate && oldAssignTo == "") {
                                //Popup message changes added for detailed view
                                let userDD = $("#conflictUserdd").dxSelectBox('instance');
                                FilUser = filteredUsers.find(x => x.AssignedTo == userDD.option("value"));
                                SplitConflictAllocations(data, userDD.option("value"), userDD.option("text"), userId, name, UserImageUrl, uniqueResources != null ? true : false, uniqueResources != null ? allocationID : null, true, FilUser);
                                /* });*/
                            }
                            else {
                                let userDD = $("#conflictUserdd").dxSelectBox('instance');
                                FilUser = filteredUsers.find(x => x.AssignedTo == userDD.option("value"));
                                SplitConflictAllocations(data, userDD.option("value"), userDD.option("text"), userId, name, UserImageUrl, uniqueResources != null ? true : false, uniqueResources != null ? allocationID : null, undefined, undefined, undefined, undefined, FilUser.UserImagePath);
                            }                            
                        }
                    })
                );
                container.append(rowData);
            }
            return container;
        };

        const popup = $("#ConflictWeekDataGridDialog").dxPopup({
            contentTemplate: ConflictWeekDataGridTemplate,
            width: "600",
            height: "auto",
            showTitle: true,
            visible: false,
            dragEnabled: true,
            hideOnOutsideClick: true,
            showCloseButton: true,
            position: {
                at: 'center',
                my: 'center',
            },
            titleTemplate: function () {
                let headerData = $(`<span style="float: left;overflow: auto;">Conflict Weeks: <a href="javascript:void(0);" onclick="openResourceTimeSheet('${userId}','${name}','${selectedYear}');">${name}</a></span>`);
                headerData.append(
                    $(`<span title="Close" class="dx-button-content close-btn" onclick="$('#ConflictWeekDataGridDialog').dxPopup('instance').hide()"><i class="dx-icon dx-icon-close" style='font-size:20px;'></i></span>`)
                );
                return headerData;
            },
            wrapperAttr: {
                id: "ConflictWeekDataGridDialog",
                class: "class-name"
            },
            onHiding: function () {
                checkAndProcessResource();
            }
        }).dxPopup('instance');

        popup.option({
            contentTemplate: () => ConflictWeekDataGridTemplate()

        });
        popup.show();
    }

    function OpenConflictWeekDetailSummary(data) {
       
        const ConflictWeekDataGridTemplateSummary = function () {
            let container = $("<div>");
            let windowHeight = parseInt(75 * $(window).height() / 100);
            let datagrid = $(`<div id='ConflictWeekDataSummaryGrid' style='max-height:${windowHeight}px'>`).dxDataGrid({
                dataSource: data,
                ID: "grdConflictWeekDataSummary",
                editing: {
                    mode: "cell",
                    allowEditing: true,
                    allowUpdating: true
                },
                sorting: {
                    mode: "multiple"
                },
                paging: {
                    enabled: false,
                },
                scrolling: {
                    mode: 'Standard',
                },
                columns: [
                    {
                        dataField: "Title",
                        caption: "Title",
                        dataType: "text",
                        width: "60%",
                        sortIndex: "0",
                        sortOrder: "asc",
                    },
                    {
                        dataField: "Role",
                        caption: "Role",
                        dataType: "text",
                    }
                ],
                showBorders: true,
                showRowLines: true,
            });
            container.append(datagrid);
            return container;
        };

        const popup = $("#ConflictWeekDataGridDialogSummary").dxPopup({
            contentTemplate: ConflictWeekDataGridTemplateSummary,
            width: "500",
            height: "auto",
            showTitle: true,
            visible: false,
            dragEnabled: true,
            hideOnOutsideClick: true,
            showCloseButton: true,
            position: {
                at: 'center',
                my: 'center',
            },
            titleTemplate: function () {
                let headerData = $(`<span style="float: left;overflow: auto;">Conflict Week Summary</span>`);
                headerData.append(
                    $(`<span title="Close" class="dx-button-content close-btn" onclick="$('#ConflictWeekDataGridDialogSummary').dxPopup('instance').hide()"><i class="dx-icon dx-icon-close" style='font-size:20px;'></i></span>`)
                );
                return headerData;
            },
            wrapperAttr: {
                id: "ConflictWeekDataGridDialogSummary",
                class: "class-name"
            },
            onHiding: function () {

            }
        }).dxPopup('instance');

        popup.option({
            contentTemplate: () => ConflictWeekDataGridTemplateSummary()

        });
        popup.show();
    }
    function OpenResumeCompare(Type, isOriginatedFromFindTeams = false) {
        let checkedUser = [];
        let additionalData = [];
        let typeName;
        $(".innerCheckbox input").each(function () {
            if ($(this).is(":checked")) {
                checkedUser.push($(this).attr("id"));
                typeName = $(this).attr("name");
                additionalData.push({ Id: $(this).attr("id"), pctAllcation: $(this).attr("pctallocation"), allocaionView: $(this).attr("allocaionview") });
            }
        });

        $.cookie("additionalData", '');
        if (checkedUser.length == 0) {
            DevExpress.ui.dialog.alert("Select at least one user.", "Warning!");
            return false;
        }
        $.cookie("additionalData", JSON.stringify(additionalData));
        let url = '<%=compareProfileUrl%>' + '&SelectedUsers=' + checkedUser.join(';');

        if (!isOriginatedFromFindTeams) {
            if (popupFilters.Customer) {
                url += "&companyname=" + encodeURIComponent("<%=Customer%>");
            }
            if (popupFilters.RequestTypes) {
                url += "&requesttype=" + "<%=RequestType%>";
            }
            if (popupFilters.complexity) {
                url += "&projectcomplexity=" + "<%=ProjectComplexity%>";
            }
            url += "&modulename=" + "<%=ModuleName%>";
            window.parent.UgitOpenPopupDialog(url, '', 'Resume Comparison: ' + Type, '95', '95', "", false);
        } else {
            if (searchTeamCriteria.CommonClient) {
                url += "&companyname=" + encodeURIComponent("<%=Customer%>");
            }
            window.parent.UgitOpenPopupDialog(url, '', 'Resume Comparison', '95', '95', "", false);
        }
    }
    function OpenProjectSummaryPage(ticketId, ticketitle) {
        var params = "TicketId=" + ticketId + "&tickettitle=" + ticketitle + "&IsSummary=true";
        window.parent.UgitOpenPopupDialog('<%=NewProjectSummaryPageUrl%>', params, ticketitle, '95', '95', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
    }

    function OpenUserCommonProject() {
        let checkedUser = [];
        $(".innerCheckbox input").each(function () {
            if ($(this).is(":checked")) {
                checkedUser.push($(this).attr("id"));
            }
        });

        if (checkedUser.length <= 1) {
            DevExpress.ui.dialog.alert("Select at least two user.", "Warning!");
            return false;
        }
        $.post("/api/RMOne/GetUsersCommonProjects?userIds=" + checkedUser.join(',') + "&ticketId=" + projectID).then(function (response) {
            const popupContentTemplate1 = function () {
                let windowHeight = parseInt(75 * $(window).height() / 100);

                if ($(window).height() < 700) {
                    windowHeight = parseInt(65 * $(window).height() / 100);
                }
                let container = $("<div>");
                let datagrid = $(`<div id='commonUserProject' style='max-height:${windowHeight}px;'>`).dxDataGrid({
                    dataSource: response,
                    columns: [
                        {
                            dataField: "Title",
                            width: "30%",
                            sortIndex: "0",
                            sortOrder: "asc",
                            cssClass:"v-align"
                        },
                        {
                            dataField: "ERPJobID",
                            caption: "CMIC #",
                            width: "13%",
                            cellTemplate: function (container, e) {
                                let ticketId = (e.data.ERPJobID != "" && e.data.ERPJobID != null ? e.data.ERPJobID : (e.data.ERPJobIDNC != "" && e.data.ERPJobIDNC != null ? e.data.ERPJobIDNC : ""));
                                $(`<div>${ticketId}</div>`)
                                    .appendTo(container);
                            },
                            cssClass: "v-align"
                        },
                        {
                            dataField: "Status",
                            width: "13%",
                            cssClass: "v-align"
                        },
                        {
                            dataField: "Client",
                            width: "13%",
                            cssClass: "v-align"
                        },
                        {
                            dataField: "MatchedTag",
                            caption: "Tags",
                            width: "7%",
                            cellTemplate: function (container, options) {
                                $(`<div class='exptags-style'>${options.data.MatchedTag}/${options.data.TotalTag}</div>`)
                                    .appendTo(container);
                            },
                            cssClass: "v-align"
                        },
                        {
                            dataField: "StartDate",
                            dataType: "date",
                            format: 'MMM d, yyyy',
                            width: "12%",
                            caption: "Start Date",
                            cellTemplate: function (container, options) {
                                if (options.data.StartDate != "0001-01-01T00:00:00") {
                                    $(`<div>${new Date(options.data.StartDate).format('MMM d, yyyy')}</div>`)
                                        .appendTo(container);
                                }
                            },
                            cssClass: "v-align"
                        },
                        {
                            dataField: "CloseDate",
                            dataType: "date",
                            format: 'MMM d, yyyy',
                            width: "12%",
                            caption: "End Date",
                            cellTemplate: function (container, options) {
                                if (options.data.CloseDate != "0001-01-01T00:00:00") {
                                    $(`<div>${new Date(options.data.CloseDate).format('MMM d, yyyy')}</div>`)
                                        .appendTo(container);
                                }
                            },
                            cssClass: "v-align"
                        }],
                    onRowClick: function (e) {
                        console.log(e.data);
                        let ticketTitle = (e.data.ERPJobID != "" && e.data.ERPJobID != null ? e.data.ERPJobID : (e.data.ERPJobIDNC != "" && e.data.ERPJobIDNC != null ? e.data.ERPJobIDNC : e.data.TicketId));
                        ticketTitle += ": " + e.data.Title;
                        OpenProjectSummaryPage(e.data.TicketId, ticketTitle);
                    }
                });
                
                container.append(datagrid);
                return container;
            };

            const popup = $("#commonUserProjectDialog").dxPopup({
                contentTemplate: popupContentTemplate1,
                width: "70%",
                height: "auto",
                showTitle: true,
                title: "Common Projects",
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
        });
    }

    function OpenUsersGanttView(ganttProjSD, ganttProjED, classNameSt, classNameEd, ganttProjReqAloc) {
        let checkedUser = [];
        let typeName;
        $(".innerCheckbox input").each(function () {
            if ($(this).is(":checked")) {
                checkedUser.push($(this).attr("id"));
                typeName = $(this).attr("name");
            }
        });
        if (checkedUser.length == 0) {
            DevExpress.ui.dialog.alert("Select at least one user.", "Warning!");
            return false;
        }
        //delete_cookie("SelectedUser");
        ////if ($.cookie('SelectedUser') != null && !$.cookie('SelectedUser') != "" && $.cookie('SelectedUser') != "null" && $.cookie('SelectedUser') != undefined) {
        ////    $.cookie("SelectedUser", null);
        ////}
        //set_cookie("SelectedUser", string(checkedUser.join(','))); 
        if (ganttProjSD != undefined && ganttProjED != undefined && classNameSt != undefined && classNameEd != undefined && ganttProjReqAloc != undefined && ganttProjSD != null && ganttProjED != null && classNameSt != null && classNameEd != null && ganttProjReqAloc != null) {
            var url = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGridNew&RequestFromProjectAllocation=true&SelectedUsers=" + checkedUser.join(',') + "&classNameSt=" + classNameSt + "&classNameEd=" + classNameEd + "&ganttProjSD=" + ganttProjSD + "&ganttProjED=" + ganttProjED + "&ganttProjReqAloc=" + ganttProjReqAloc;
            window.parent.UgitOpenPopupDialog(url, "", "Timeline for User", "95", "95", "", false);
        }
        else {
            var url = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGridNew&RequestFromProjectAllocation=true&SelectedUsers=" + checkedUser.join(',') + "&ShowDateInfo=false";
            window.parent.UgitOpenPopupDialog(url, "", "Timeline for User", "95", "95", "", false);
        }        
    }

    const LoadGanttChart = function (ganttSource) {

        var maxDaterange = new Date(Math.max.apply(null, ganttSource.map(function (e) {
            return new Date(e.AllocationEndDate);

        })));

        var minDaterange = new Date(Math.min.apply(null, ganttSource.map(function (e) {
            return new Date(e.AllocationStartDate);

        })));

        var heightInPx = function (ganttSource) {
            var lengthInPx = ((ganttSource.length + 1) * 38) + 12;
            if (lengthInPx > 600)
                return 600;
            else
                return lengthInPx;

        }

        var gantt = $("#divPopupGantt").dxGantt({
            height: heightInPx(ganttSource),
            tasks: {
                dataSource: ganttSource,
                endExpr: "AllocationEndDate",
                keyExpr: "ID",
                //progressExpr: "PctAllocation",
                startExpr: "AllocationStartDate",
                titleExpr: "AssignedToName"
            },
            export: {
                enabled: true,
                printingEnabled: false
            },
            toolbar: {
                items: [
                    'zoomIn',
                    'zoomOut',
                    'fullScreen'
                ]
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
                dataField: 'TypeName',
                caption: 'Role',
                width: 200,
            },
            {
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
                visible: false,
            }, {
                dataField: 'PctAllocation',
                caption: '%',
                width: 30,
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
        const resource = item.taskResources[0];
        const startDate = new Date(item.taskData.AllocationStartDate);
        const endDate = new Date(item.taskData.AllocationEndDate);
        const img = "/Content/Images/girlprofilepic.jpg";
        let colorcode = 'Precon';
        if (startDate >= new Date('<%=PreConStartDateString%>') && endDate <= new Date('<%=PreConEndDateString%>'))
            colorcode = 'Precon';
        else if (startDate >= new Date('<%=ConstructionStartDateString%>') && endDate <= new Date('<%=ConstructionEndDateString%>'))
            colorcode = 'Const';
        else if (startDate >= new Date('<%=CloseOutStartDateString%>') && endDate <= new Date('<%=CloseOutEndDateString%>'))
            colorcode = 'Closeout';

        const taskWidth = `${item.taskSize.width}px;`;
        const $customContainer = $(document.createElement('div'))
            .addClass('custom-task')
            .attr('style', `width:${taskWidth}`)
            .addClass(`custom-task-color-${colorcode}`);

        if (JSON.parse("<%=EnableGanttProfilePic%>".toLowerCase())) {

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

        return $customContainer;
    }


    var changedDates = {};
    const popupContentTemplate = function () {
        const timeFrameTypes = ["Days", "Weeks", "Months"];

        return $('<div>').append(
            $(`<h2 class='reschedule-title'><span>Team Reschedule</span></h2><p class='reschedule-subtitle'><span>Move All Allocations Forward (+) or Backward (-).</span></p>`),

            $(`<div id='divInputContainer' class='d-flex justify-content-space-evenly align-items-center flex-wrap paddingBottom' />`).append(
                $(`<div id='btnSpin' class='divMarginTop mr-3' />`).dxNumberBox({
                    value: 0,
                    //min: 0,
                    //max: 365,
                    showSpinButtons: true,
                    showClearButton: true,
                    stylingMode: 'outlined',
                    placeholder: 'select days, months or years',
                    label: 'Input',
                    width: "45%",
                    height: 40,
                    value: 0,
                }),

                $(`<div id='divTimeframe' class='divMarginTop' />`).dxSelectBox({
                    items: timeFrameTypes,
                    placeholder: 'Choose Timeframe',
                    showClearButton: true,
                    stylingMode: 'outlined',
                    label: 'Time',
                    width: "45%",
                    height: 40,
                    value: 'Days',
                }),
            ),
            $("<div class='ml-2' id='updatePhaseDates'>").dxCheckBox({
                value: false,
                text: "Change Phase Dates:"
            }),
            $(`<div class='btnWrapper mr-2 mt-3'/ >`).append(
                $(`<div class='cancelClass' style='float:right;' />`).dxButton({
                    text: 'Cancel',
                    icon: 'clear',
                    onClick: function (e) {
                        var popup = $("#UpdateDatesPopup").dxPopup("instance");
                        popup.hide();
                    }
                }),
                $(`<div class='continueClass' />`).dxButton({
                    text: 'Confirm ',
                    icon: '/Content/images/check-icon.png',
                    onClick: function (e) {
                        var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                        var timeframectrl = $("#divTimeframe").dxSelectBox("instance");
                        var spinctrl = $("#btnSpin").dxNumberBox("instance");
                        //var radioChoice = $("#divChoice").dxSelectBox("instance");
                        var inputValue = parseInt(spinctrl.option("value"));
                        var timeframeValue = timeframectrl.option("value");
                        var updatePhaseDates = $("#updatePhaseDates").dxCheckBox('option', 'value')
                        var rows = dataGrid.getVisibleRows();
                        //var radioChoiceValue = radioChoice.option("value");
                        var openConfirmationD = false;
                        if (inputValue != 0) {
                            globaldata.forEach(function (item, index) {
                                var oldenddate = new Date(item.AllocationStartDate);
                                var oldstartdate = new Date(item.AllocationEndDate);

                                if ((oldenddate < new Date() || oldstartdate < new Date()) && !item.IsLocked) {
                                    openConfirmationD = true;
                                }

                            });
                            if (timeframeValue == "Days") {
                                inputValue = inputValue >= 0
                                    ? inputValue + 1
                                    : inputValue - 1
                            }
                            if (openConfirmationD && inputValue > 0) {
                                var conflictDialog = DevExpress.ui.dialog.custom({
                                    title: "Alert",
                                    message: `Do you want to shift the past dates?`,
                                    buttons: [
                                        { text: "Yes", onClick: function () { return "Ok" }, elementAttr: { "class": "btnBlue" } },
                                        { text: "No", onClick: function () { return "Cancel" }, elementAttr: { "class": "btnNormal" } }
                                    ]
                                });
                                conflictDialog.show().done(function (dialogResult) {
                                    if (dialogResult == "Ok") {
                                        ShiftDates(timeframeValue, inputValue, updatePhaseDates, "PastAndFuture");
                                        var popup = $("#UpdateDatesPopup").dxPopup("instance");
                                        popup.hide();
                                    }
                                    else if (dialogResult == "Cancel") {
                                        ShiftDates(timeframeValue, inputValue, updatePhaseDates, "Future");
                                        var popup = $("#UpdateDatesPopup").dxPopup("instance");
                                        popup.hide();
                                    }
                                });
                            }
                            else {
                                ShiftDates(timeframeValue, inputValue, true, "Future");
                                var popup = $("#UpdateDatesPopup").dxPopup("instance");
                                popup.hide();
                            }
                        }
                    }
                }),
            ),

        );
    };


    Date.prototype.addDays = function (days) {
        var date = new Date(this.valueOf());
        date.setDate(date.getDate() + days);
        return date;
    }
    function ShiftDates(timeFrame, duration, updatePhaseDates, uType) {
        $.post("/api/rmmapi/UpdateAllocationsAndPhaseDates/", {
            Allocations: globaldata, TimeFrame: timeFrame, Duration: duration, UpdatePhaseDates: updatePhaseDates, UType: uType,
            PreconStartDate: dataModel.preconStartDate != '' ? new Date(dataModel.preconStartDate).format("MM/dd/yyyy") : '',
            PreconEndDate: dataModel.preconEndDate != '' ? new Date(dataModel.preconEndDate).format("MM/dd/yyyy") : '',
            ConstStartDate: dataModel.constStartDate != '' ? new Date(dataModel.constStartDate).format("MM/dd/yyyy"): '',
            ConstEndDate: dataModel.constEndDate != '' ? new Date(dataModel.constEndDate).format("MM/dd/yyyy") : '',
            CloseOutStartDate: dataModel.closeoutStartDate != '' ? new Date(dataModel.closeoutStartDate).format("MM/dd/yyyy") : '',
            CloseOutEndDate: dataModel.closeoutStartDate != '' ? new Date(dataModel.closeoutEndDate).format("MM/dd/yyyy") : ''
        }).then(function (response) {
            if (duration < 0) {
                let openConfirmationD = false;
                response.Allocations.forEach(function (item, index) {
                    var oldenddate = new Date(item.AllocationStartDate);
                    var oldstartdate = new Date(item.AllocationEndDate);
                    if ((oldenddate < new Date() || oldstartdate < new Date()) && !item.IsLocked) {
                        openConfirmationD = true;
                    }
                });
                if (openConfirmationD) {
                    var conflictDialog = DevExpress.ui.dialog.custom({
                        title: "Alert",
                        message: `After shifting the dates backward few dates were in the past. Do you want to proceed?`,
                        buttons: [
                            { text: "Yes", onClick: function () { return "Ok" }, elementAttr: { "class": "btnBlue" } },
                            { text: "No", onClick: function () { return "Cancel" }, elementAttr: { "class": "btnNormal" } }
                        ]
                    });
                    conflictDialog.show().done(function (dialogResult) {
                        if (dialogResult == "Ok") {
                            UpdateDatesAllocationAndPhase(response, updatePhaseDates)
                        }
                    });
                }
                else {
                    UpdateDatesAllocationAndPhase(response, updatePhaseDates)
                }
            }
            else {
                UpdateDatesAllocationAndPhase(response, updatePhaseDates)
            }
        });
    }
    function UpdateDatesAllocationAndPhase(response, updatePhaseDates) {
        response.Allocations.forEach(function (item, index) {
            let data = globaldata.find(x => x.ID == item.ID);
            if (data != null) {
                var newStartDate = new Date(item.AllocationStartDate);
                var newEndDate = new Date(item.AllocationEndDate);
                data.AllocationStartDate = item.AllocationStartDate;
                data.AllocationEndDate = newEndDate < newStartDate ? newStartDate : item.AllocationEndDate;
            }
        });
        
        $.cookie("dataChanged", 1, { path: "/" });
        SaveAllocations(updatePhaseDates, response);
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

    //#region timesheet methods
    function openResourceTimeSheet(assignedTo, assignedToName, selectedYear) {
        showTimeSheet = true;
        //param isRedirectFromCardView is used to hide card view and show allocation grid
        //param ShowUserResume is used to show user resume page.
        if (selectedYear == undefined) {
            selectedYear = new Date().getFullYear();
        }
        var url = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGridNew&ViewName=FindAvailability&isRedirectFromCardView=true&showuserresume=true&selectedYear=" + selectedYear +"&selecteddepartment=-1&SelectedResource=" + assignedTo;
        window.parent.UgitOpenPopupDialog(url, "", "Gantt: " + assignedToName, "95", "95", "", false);
    }

    var expandAdaptiveDetailRow = function (key, dataGridInstance) {
        if (!dataGridInstance.isAdaptiveDetailRowExpanded(key)) {
            dataGridInstance.expandAdaptiveDetailRow(key);
        }
    }

    function openDateAgent(ticketid, hidePreconPhase, hideConstPhase) {
        Model.RecordId = ticketid;
        const popup = $("#popup").dxPopup("instance");
        popup.show();
        tempdataModel = JSON.parse(JSON.stringify(dataModel));
        let form = $("#form").dxForm("instance");
        if (hidePreconPhase != undefined && hidePreconPhase) {
            form.itemOption('group.preconDuration', 'visible', false);
            form.itemOption('group.preconStartDate', 'visible', false);
            form.itemOption('group.preconEndDate', 'visible', false);
        }
        else {
            form.itemOption('group.preconDuration', 'visible', true);
            form.itemOption('group.preconStartDate', 'visible', true);
            form.itemOption('group.preconEndDate', 'visible', true);
        }

        if (hideConstPhase != undefined && hideConstPhase) {
            form.itemOption('group.constStartDate', 'visible', false);
            form.itemOption('group.constEndDate', 'visible', false);
            form.itemOption('group.constDuration', 'visible', false);
            form.itemOption('group.closeOutDuration', 'visible', false);
            form.itemOption('group.closeoutEndDate', 'visible', false);
            form.itemOption('group.closeoutStartDate', 'visible', false);
        }
        else {
            form.itemOption('group.constStartDate', 'visible', true);
            form.itemOption('group.constEndDate', 'visible', true);
            form.itemOption('group.constDuration', 'visible', true);
            form.itemOption('group.closeOutDuration', 'visible', true);
            form.itemOption('group.closeoutEndDate', 'visible', true);
            form.itemOption('group.closeoutStartDate', 'visible', true);
        }
        form.option("formData", dataModel);
        //form.repaint();
        //form.itemOption("group", "caption", title);
    }

    function labelTemplate(iconName) {
        return (data) => $(`<div><i class='dx-icon dx-icon-${iconName}'></i>${data.text}</div>`);
    }

    function UpdateRecord() {

        if ((dataModel.preconEndDate != null && dataModel.preconEndDate != "") && (dataModel.preconStartDate == null || dataModel.preconStartDate == "")) {
            DevExpress.ui.dialog.alert("Entry of Precon Start Date is required.", "Error!");
            return;
        }
        if (dataModel.preconEndDate != null) {
            if (new Date(dataModel.preconStartDate) > new Date(dataModel.preconEndDate)) {
                DevExpress.ui.dialog.alert("Precon End Date should be after the Precon Start Date.", "Error!");
                return;
            }
        }

        if ((dataModel.constEndDate != null && dataModel.constEndDate != "") && (dataModel.constStartDate == null || dataModel.constStartDate == "")) {
            DevExpress.ui.dialog.alert("Entry of Construction Start Date is required.", "Error!");
            return;
        }
        if (new Date(dataModel.constStartDate) > new Date(dataModel.constEndDate)) {

            if (dataModel.constEndDate == null || dataModel.constEndDate == "") {
                DevExpress.ui.dialog.alert("Entry of Construction End Date is required.", "Error!");
                return;
            }
            DevExpress.ui.dialog.alert("Construction End Date should be after the Construction Start Date.", "Error!");
            return;
        }

        if (new Date(dataModel.constEndDate) > new Date(dataModel.closeoutEndDate)) {
            DevExpress.ui.dialog.alert("Closeout Date should be after the Construction End Date.", "Error!");
            return;
        }
        if (new Date(dataModel.closeoutStartDate) > new Date(dataModel.closeoutEndDate)) {
            DevExpress.ui.dialog.alert("Closeout End Date should be after the Closeout Start Date.", "Error!");
            return;
        }

        if (dataModel.constEndDate == null || dataModel.constEndDate == "") {
            dataModel.closeoutStartDate = null;
            dataModel.closeoutEndDate = null;
        }

        var arrDates = [
            ['Precon Start Date', dataModel.preconStartDate == null ? "" : dataModel.preconStartDate],
            ['Precon End Date', dataModel.preconEndDate == null ? "" : dataModel.preconEndDate],
            ['Construction Start Date', dataModel.constStartDate == null ? "" : dataModel.constStartDate],
            ['Construction End Date', dataModel.constEndDate == null ? "" : dataModel.constEndDate],
            ['Closeout End Date', dataModel.closeoutEndDate == null ? "" : dataModel.closeoutEndDate]
        ];
        for (let i = 0; i < arrDates.length; i++) {
            let newdate = new Date(arrDates[i][1]);
            if (arrDates[i][1] != "") {
                if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                    DevExpress.ui.dialog.alert("Please enter a valid " + arrDates[i][0]);
                    return;
                }
                Model.Fields[i].Value = newdate.toLocaleDateString('en-US');
            }
            else {
                Model.Fields[i].Value = "";
            }
        }

        Model.Fields[5].Value = dataModel.onHold == true ? '1' : '0';
        $.ajax({
            type: "POST",
            url: "<%= rmoneControllerUrl %>UpdateRecord",
            data: JSON.stringify(Model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (message) {
                $("#toast").dxToast("show");
                const popup = $("#popup").dxPopup("instance");
                popup.hide();
                GetProjectDates();
                if (window.parent.estimatedconstructionstartclientname != undefined) {
                    if (dataModel.constStartDate != '') {
                        window.parent.estimatedconstructionstartclientname.SetValue(new Date(dataModel.constStartDate));
                        window.parent.$(".field_estimatedconstructionstart_view").text(new Date(dataModel.constStartDate).format('MMM dd, yyyy'))
                    }
                    else {
                        window.parent.estimatedconstructionstartclientname.SetValue();
                        window.parent.$(".field_estimatedconstructionstart_view").text('')
                    }
                }
                else {
                    if (dataModel.constStartDate != '') {
                        window.parent.$(".field_estimatedconstructionstart_view").text(new Date(dataModel.constStartDate).format('MMM dd, yyyy'))
                    }
                    else {
                        window.parent.$(".field_estimatedconstructionstart_view").text('')
                    }
                }
                if (window.parent.preconstartdateclientname != undefined) {
                    if (dataModel.preconStartDate != '') {
                        window.parent.preconstartdateclientname.SetValue(new Date(dataModel.preconStartDate));
                        window.parent.$(".field_preconstartdate_view").text(new Date(dataModel.preconStartDate).format('MMM dd, yyyy'));
                    }
                    else {
                        window.parent.preconstartdateclientname.SetValue();
                        window.parent.$(".field_preconstartdate_view").text('');
                    }
                }
                else {
                    if (dataModel.preconStartDate != '') {
                        window.parent.$(".field_preconstartdate_view").text(new Date(dataModel.preconStartDate).format('MMM dd, yyyy'));
                    }
                    else {
                        window.parent.$(".field_preconstartdate_view").text('');
                    }
                }
                if (window.parent.preconenddateclientname != undefined) {
                    if (dataModel.preconEndDate != '') {
                        window.parent.preconenddateclientname.SetValue(new Date(dataModel.preconEndDate));
                        window.parent.$(".field_preconenddate_view").text(new Date(dataModel.preconEndDate).format('MMM dd, yyyy'));
                    } else {
                        window.parent.preconenddateclientname.SetValue();
                        window.parent.$(".field_preconenddate_view").text('');
                    }
                }
                else {
                    if (dataModel.preconEndDate != '') {
                        window.parent.$(".field_preconenddate_view").text(new Date(dataModel.preconEndDate).format('MMM dd, yyyy'));
                    } else {
                        window.parent.$(".field_preconenddate_view").text('');
                    }
                }
                if (window.parent.estimatedconstructionendclientname != undefined) {
                    if (dataModel.constEndDate != '') {
                        window.parent.estimatedconstructionendclientname.SetValue(new Date(dataModel.constEndDate));
                        window.parent.$(".field_estimatedconstructionend_view").text(new Date(dataModel.constEndDate).format('MMM dd, yyyy'));
                        window.parent.$('.field_CRMDuration_view').text(dataModel.preconDuration);
                        window.parent.$('.CRMDuration').val(dataModel.preconDuration);
                    } else {
                        window.parent.estimatedconstructionendclientname.SetValue();
                        window.parent.$(".field_estimatedconstructionend_view").text('');
                    }
                }
                else {
                    if (dataModel.constEndDate != '') {
                        window.parent.$(".field_estimatedconstructionend_view").text(new Date(dataModel.constEndDate).format('MMM dd, yyyy'));
                        window.parent.$('.field_CRMDuration_view').text(dataModel.preconDuration);
                        window.parent.$('.CRMDuration').val(dataModel.preconDuration);
                    } else {
                        window.parent.$(".field_estimatedconstructionend_view").text('');
                    }
                }

                if (window.parent.closeoutdateclientname != undefined) {
                    if (dataModel.closeoutEndDate != '') {
                        window.parent.closeoutdateclientname.SetValue(new Date(dataModel.closeoutEndDate));
                        window.parent.$(".field_closeoutdate_view").text(new Date(dataModel.closeoutEndDate).format('MMM dd, yyyy'));
                        window.parent.$('.field_closeoutstartdate_view').text(new Date(dataModel.closeoutStartDate).format('MMM dd, yyyy'));
                    } else {
                        window.parent.closeoutdateclientname.SetValue();
                        window.parent.$(".field_closeoutdate_view").text('');
                        window.parent.$('.field_closeoutstartdate_view').text('');
                    }
                }
                else {
                    if (dataModel.closeoutEndDate != '') {
                        window.parent.$(".field_closeoutdate_view").text(new Date(dataModel.closeoutEndDate).format('MMM dd, yyyy'));
                        window.parent.$('.field_closeoutstartdate_view').text(new Date(dataModel.closeoutStartDate).format('MMM dd, yyyy'));
                    } else {
                        window.parent.$(".field_closeoutdate_view").text('');
                        window.parent.$('.field_closeoutstartdate_view').text('');
                    }
                }
                //setTimeout(function () { location.reload(); }, 1000);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
    function RefreshGrids(lstData) {
        $.each(lstData, function (index, value) {
            globaldata = jQuery.grep(globaldata, function (obj) {
                return obj.ID != value.ID;
            });
            //alert(index + ": " + value.ID);
        });
        $.each(lstData, function (index, value) {
            compactTempData = jQuery.grep(compactTempData, function (obj) {
                return obj.ID != value.ID;
            });
            //alert(index + ": " + value.ID);
        });
        var compactDataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
        compactDataGrid.option("dataSource", compactTempData);
        compactDataGrid._refresh();

        var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
        dataGrid.option("dataSource", globaldata);
        dataGrid._refresh();
    }

    //Popup message changes added parameter like isBothDatesLesserThanCurrent
    function SplitAllocationOnPctChange(ID, newValue, refIds, isBothDatesLesserThanCurrent) {
        //Popup message changes added for Detailed View
        SetDatesOfSummaryView(ID, true, isBothDatesLesserThanCurrent);
        if (isBothDatesLesserThanCurrent) {
            let alloc = null;
            alloc = globaldata.find(x => x.ID == ID);
            alloc.PctAllocation = newValue;
        }
        else if (popupFilters.Allocations?.length > 0) {
            let newIds = CheckAndUpdatePastAllocations();
            let alloc = null;
            if (newIds?.length > 0) {
                alloc = globaldata.find(x => newIds.includes(String(x.ID)));
            } else {
                alloc = globaldata.find(x => x.ID == ID);
            }
            alloc.PctAllocation = newValue;
            if (refIds != undefined) {
                let gdata = globaldata.filter(x => refIds.includes(String(x.ID)) || x.ID == alloc.ID);
                let dataGridChild = $("#InternalAllocationGrid").dxDataGrid("instance");
                dataGridChild.option("dataSource", gdata);
            }
        }        
        else {
            DevExpress.ui.dialog.alert(`The end date should be in the future.`, 'Error');
        }
    }
//#endregion

    function showDetailViewGrid() {
        $("#compactGridTemplateContainer").hide();
        $("#gridTemplateContainer").show();
        $("#btnDetailedSummary span").text("Summary View");
        $("#btnPrecondate, #btnConstructionDate, #btnCloseoutDate").show();
        $(".schedule-label").hide();
        $("#btnSaveAsTemplate").hide();
        $("#btnFindTeam").show();
    }
    

    function LoadGlobalDataObject() {
        
        $.get("/api/rmmapi/GetProjectAllocations?projectID=<%= TicketID %>", function (data, status) {
            globaldata = data.Allocations;

            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
            dataGrid.option("dataSource", globaldata);
            GroupsData = data.Roles;
            UsersData = data.UserProfiles;

            if (globaldata.length < 7)
                $("#divMainGrid").height("745px");
            CheckPhaseConstraints(true);
            GenerateData();
            setTimeout(() => {
                if (openSummaryView)
                    $("#btnDetailedSummary").click();
            }, 500);

        });
    }

    function UpdateWorkingDays(e) {
        let totalWorkingDays = "0";
        let startDate = e.newData.AllocationStartDate != undefined ? e.newData.AllocationStartDate : e.oldData.AllocationStartDate;
        let endDate = e.newData.AllocationEndDate != undefined ? e.newData.AllocationEndDate : e.oldData.AllocationEndDate;
        totalWorkingDays = GetTotalWorkingDaysBetween(ajaxHelperPage, startDate, endDate);
        if (parseInt(totalWorkingDays) > 0) {
            globaldata.forEach(function (z) {
                if (z.ID == e.key.ID) {
                    z.TotalWorkingDays = parseInt(totalWorkingDays)
                }
            });
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
            <div class="divMarginTop divMarginBottom d-flex justify-content-between align-items-center flex-wrap">
                <div class="pr-2">
                    <div id="btnShowGanttView" class="btnIcon"></div>
                    <div id="btnShowProfile" class="btnAddNewNoBackground"></div>
                </div>
                <div style="display: flex; margin-top: 27px;" class="text-center allocdatesbtn" <%=EnableMultiAllocation%>>
                    <div class="col-md-4 px-1">
                        <div id="btnPrecondate"></div>
                        <div class="schedule-label preconborderbox" style="color: #52BED9;">Precon Dates</div>
                        <p id="pPrecon"><%=PreConStartDateString %> to <%=PreConEndDateString %></p>
                        <div id="pPreconNoDate" style="display: none;">No Date Entered</div>
                    </div>
                    <div class="col-md-4 px-1">
                        <div id="btnConstructionDate"></div>
                        <div class="schedule-label constborderbox" style="color: #005C9B;">Const. Dates</div>
                        <p id="pConst"><%=ConstructionStartDateString %> to <%=ConstructionEndDateString%></p>
                        <div id="pConstNoDate" style="display: none;">No Date Entered</div>
                    </div>
                    <div class="col-md-4 px-1">
                        <div id="btnCloseoutDate" style="max-width: initial !important"></div>
                        <div class="schedule-label closeoutborderbox" style="color: #351B82;">Closeout Dates</div>
                        <p id="pCloseout"><%=CloseOutStartDateString %> to <%=CloseOutEndDateString%></p>
                        <div id="pCloseoutNoDate" style="display: none;">No Date Entered</div>
                    </div>
                </div>
                <script>
                    if ("<%=HidePrecon%>".toUpperCase() == "HIDDEN") {
                        $("#pPrecon").hide();
                        $("#pPreconNoDate").show();
                    }
                    else {
                        $("#pPreconNoDate").hide();
                        $("#pPrecon").show();
                    }
                    if ("<%=HideConst%>".toUpperCase() == "HIDDEN") {
                        $("#pConst").hide();
                        $("#pConstNoDate").show();
                    }
                    else {
                        $("#pConstNoDate").hide();
                        $("#pConst").show();
                    }
                    if ("<%=HideCloseout%>".toUpperCase() == "HIDDEN") {
                        $("#pCloseout").hide();
                        $("#pCloseoutNoDate").show();
                    }
                    else {
                        $("#pCloseoutNoDate").hide();
                        $("#pCloseout").show();
                    }
                </script>
                <div class="px-0">
                    <div id="btnAddNew" class="btnAddNew"></div>
                    <div id="btnFindTeam" class="btnAddNew"></div>
                    <div id="btnShowMultiAllocation" style="display: none" class="btnAddNew" <%=EnableMultiAllocation%>></div>
                </div>
                <div class="pl-2" style="display: flex;">
                    <%--<div id="btnSaveAsTemplate" class="btnSaveAsTemplate" <%=HideAllocationTemplate ? "style='visibility:hidden;'" : "" %> ></div>--%>
                    <div id="btnSaveAsTemplate" class="btnAddNew" style="display: none;margin-right:5px;"></div>
                    <div id="btnUpdateSchedule" class="btnAddNew"></div>

                    <div id="btnClose" class="btnAddNew"></div>
                </div>
            </div>

            <div class="row">
                <div id="toast"></div>
                <div id="toastOverlap"></div>
                <div id="toastBlankAllocation"></div>
                <div id="toastwarning"></div>
            </div>
            <div class="row">
                <div id="divMainGrid">
                    <div class="row">
                        <div id="gridTemplateContainer" class="grid-template-container" style="width: 100%; float: left;">
                        </div>
                        <div id="compactGridTemplateContainer" class="grid-template-container" style="width: 100%; float: left; display: none;">
                        </div>
                    </div>
                    <div class="row">
                        <div class="divMarginTop divMarginBottom d-flex justify-content-between flex-wrap col-md-12 px-0" <%=SetAlignment%>>

                            <div class="pr-2">
                                <div id="btnFixOverlaps" style="padding: 2px 4px; display: none;" class="btnAddNew"></div>
                                <div id="btnDetailedSummary" style="padding: 2px 4px; display: none;" class="btnAddNew"></div>
                            </div>
                            <div>
                                <div id="btnCancelChanges" class="btnAddNew"></div>
                                <div id="btnSaveAllocation" class="btnAddNew"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <div id="ganttview" class="col-md-12 col-sm-12 col-xs-12 noPadd1" style="display: none">
            <div class="row">
                <div id="btnShowDataView" class="btnIcon dataView_btn"></div>
            </div>
            <div class="row">
                <div id="divPopupGantt"></div>
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
<div id="confirmPopupContainer">
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
    <%--<div id='tagboxTitles' class='filterctrl-jobtitle'></div>--%>
    <div class='filterctrl-userpicker'></div>
</div>
<div id="popupMutitpleAllocation"></div>

<div id="filterone"></div>
<div id="UpdateDatesPopup">
</div>
<div id="confirmationDialog"></div>
<div id="InternalAllocationGridDialog"></div>
<div id="ConflictWeekDataGridDialog"></div>
<div id="ConflictWeekDataGridDialogSummary"></div>
<div id="addExperienceTagsDialog"></div>
<div id="exeperiencetoast"></div>
<div id="tooltip"></div>
<div id="usersExperienceTagsDialog"></div>
<div id="popup"></div>
<div id="saveButton" class="btnAddNew mb-3" style="float: right; font-size: 14px; margin-right: -5px;">
</div>
<div id="cancelButton" class="btnAddNew mb-3" style="float: right; font-size: 15px; margin-right: 2px;">
</div>
<div id="usersExperienceTagsPopover"></div>
<div id="commonUserProjectDialog"></div>
<div id="usersDateConfirmationDialog"></div>
