<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomResourceAllocation.ascx.cs" Inherits="uGovernIT.Web.CustomResourceAllocation" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGantt.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGantt" TagPrefix="dx" %>

<%@ Register Src="~/CONTROLTEMPLATES/RMM/CustomResourceAllocationCard.ascx" TagPrefix="uGovernIT"  TagName="CustomResourceAllocationCard" %>
<%@ Register Src="~/controltemplates/uGovernIT/Task/TaskList.ascx" TagPrefix="uc1" TagName="TaskList" %>
<%@ Register Src="~/ControlTemplates/RMONE/ResourceAllocationGridNew.ascx" TagPrefix="uc1" TagName="ResourceAllocationGridNew" %>

<%@ Import Namespace="uGovernIT.Utility" %>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    /*.detailviewmain {
        float: left;
        width: 100%;
        min-height: 550px;
    }*/
    .dxgvCommandColumnItem_UGITBlackDevEx {
        margin: 0px 0px;
    }
    /*.allocationblockinner {
        float: left;
        background: #fff;
        width: 100%;
    }*/

    /*.allocationblockinner_sub {       
        float: left;
        margin: 7px;
        width: 100%;
        margin-top: 6px;
    }*/

    .searchview-m {
        float: left;
        max-height: 350px;
        overflow-x: hidden;
        overflow-y: auto;
        width: 300px;
    }

    .detailview-m {
        float: left;
        padding: 12px;
        width: 97%;
        padding-top: 0px;
    }

    /*.detailviewresource-m {
        float: left;
        width: 100%;
    }*/

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
        width: 104%;
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
        float: right;
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
        font-weight: 500 !important;
        font-size: 16px !important;
    }

    .fright {
        float: right;
    }


    /*.fleft {
        float: left;
    }

    .detailviewresource-main {
        float: left;
        width: 100%;
        margin: 7px 0px;
    }*/

    .ms-viewheadertr .ms-vh2-gridview {
        height: 25px;
        text-align: left;
    }

    th.ms-vh2 {
        font-weight: bold !important;
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

    .ms-listviewtable {
        background: #F8F8F8 !important;
    }

    .allowcationdetail {
        border: 1px solid #DCDCDC !important;
        border-collapse: separate !important;
    }

    .width70 {
        width: 70px;
    }

    .width50 {
        width: 50px;
    }

    .width45 {
        width: 45px;
        text-align: right;
    }

    .width25 {
        width: 25px;
        text-align: right;
    }

    .action-container {
        float: left;
        width: 20px;
    }

    .hide {
        display: none;
    }

    .filteredlist table.ms-listviewtable {
        border: 1px solid #9DA0AA !important;
        border-collapse: separate !important;
    }

    .editallocation {
        text-align: center;
        border: 0px;
    }

    .editallocationMonthTable {
        border-collapse: collapse;
        width: 100%;
    }

        .editallocationMonthTable th {
            cursor: default;
            font-weight: bold;
        }

        .editallocationMonthTable th, .editallocationMonthTable td {
            text-align: center;
            border: 1px solid #CACBD3;
        }

    .cross-headerth {
        background: #3E4F50;
        padding: 0px !important;
        width: 100px;
    }

    .cross-header {
        background: url("/Content/images/pm_gridtopcorner.png") no-repeat;
        width: 105px;
        height: 32px;
        color: #F2F2F2;
    }

    /*.monthedittd {
        padding: 3px 0px;
        float: left;
        width: 100%;
    }*/

    .subordiate-head {
        padding-left: 57px;
        text-align: left;
    }

    .subordiate-data {
        float: left;
        margin-left: 10px;
    }

    #ms-belltown-table {
        padding-left: 6px !important;
    }

    .rmm-project-title-workitem {
        width: 100%;
        min-height: 45px;
        display: grid;
        align-content: center;
        text-align: center;
        background: #f2f3f4;
        color: black;
        padding: 10px;
        border-radius: 10px;
        float: right;
    }

    .rmm-project-title-workitem-header table {
        width: 100% !important;
        float: right;
    }

    .rmm-project-title-workitem a {
        color: black;
        font-size: 14px;
    }

    .rmm-project-title-workitem-1 {
        width: 100%;
        min-height: 45px;
        display: grid;
        align-content: center;
        background: #f2f3f4;
        color: black;
        padding: 10px;
        border-radius: 10px;
        float: right;
        text-align: center;
    }

        .rmm-project-title-workitem-1 a {
            text-align: center;
            font-size: 14px;
            color: black;
        }

    .lineHorizontal__container {
        align-items: center;
        display: flex;
        height: 80px;
    }

    .lineHorizontal {
        border-top: 2px solid rgb(30, 30, 30);
        width: 100%;
    }

    .groupStyle {
        color: black;
        border-radius: 10px;
        padding: 9px;
        text-align: center;
        border: 2px solid;
    }

    .rmm-project-title-cmic {
        height: 45px;
        display: grid;
        align-content: center;
        float: right;
        width: 100%;
        text-align: center;
        background: #4FA1D6;
        color: white;
        padding: 10px;
        border-radius: 10px;
        font-weight: bold;
    }

        .rmm-project-title-cmic a {
            color: white;
            font-weight: bold;
        }

    .rmm-project-common {
        height: 45px;
        display: grid;
        align-content: center;
        text-align: center;
        background: #f2f3f4;
        color: black;
        padding: 7px;
        font-size: 14px;
    }

    .estReport-gridHeaderRow table tr td {
        text-align: center !important;
        color: #343232;
        font-size: 16px !important;
        font-family: 'Roboto', sans-serif !important;
        font-weight: 500 !important;
        padding: 5px;
    }

    .dxgvGroupRow_UGITNavyBlueDevEx td.dxgv, .dxgvFocusedGroupRow_UGITNavyBlueDevEx td.dxgv {
        border: 0 none;
        border-bottom: none;
    }

    .estReportRA-dataRow td {
        border-bottom: 0px solid #f6f7fb !important;
        border-right: none !important;
        /* text-align: left !important; */
        padding-right: 10px !important;
        font-weight: 500;
        color: #201f35;
        font: 14px 'Roboto', sans-serif !important;
        padding: 5px 10px 6px;
        word-break: break-word;
        background: #fff !important;
        text-align: left;
    }

    .dxgvEditFormDisplayRow_UGITNavyBlueDevEx td.dxgv, .dxgvDataRow_UGITNavyBlueDevEx td.dxgv, .dxgvDataRowAlt_UGITNavyBlueDevEx td.dxgv, .dxgvSelectedRow_UGITNavyBlueDevEx td.dxgv, .dxgvFocusedRow_UGITNavyBlueDevEx td.dxgv {
        overflow: hidden;
        border-bottom: 0px solid #d9dae0;
        border-right: 1px solid #d9dae0;
        border-top-width: 0;
        border-left-width: 0;
        padding: 4px 6px;
    }

    .collapseClass {
        transform: rotate(270deg);
        background-image: none !important;
        margin-bottom: 1px;
        margin-right: 2px;
        margin-left: 2px;
    }

    .expandClass {
        transform: rotate(90deg);
        background-image: none !important;
        margin-bottom: 1px;
        margin-right: 2px;
        margin-left: 2px;
    }

    .innerGroup {
        color: black;
        padding: 5px 25px;
    }

    .imgHeight {
        height: 30px;
        margin-left: 15px;
    }

    .resourceAllocationgridClass {
        width: 80%;
        padding-left: 15px;
    }

    .ResourceListTDClass {
        padding-top: 22px;
        vertical-align: top;
        width: 20%;
    }

    .btnClose {
        float: right;
        padding-top: 10px;
    }

    .allocationblockinner {
        min-height: 530px;
        padding: 0px 10px;
    }

    custom-task-color-0 {
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

    .paddingBottom {
        padding-bottom: 30px;
    }

    .continueClass {
        margin: 15px;
        float: right;
        margin-right: 0px;
    }

    .dxgvHeader_CustomMaterial {
    }

    .dx-checkbox-container {
        align-items: flex-start;
    }

    .dx-popup-content {
        padding-top: 10px;
        padding-bottom: 10px;
    }

    .CRMstatusGrid_headerRow {
        border: none;
        font-size: 18px;
        padding: 5px 0px;
    }

        .CRMstatusGrid_headerRow a {
            border: none;
            font-size: 18px;
            padding: 5px 0px;
        }

    .myClass {
        visibility: hidden;
        margin-left: -5px;
        width: 18px !important;
    }

    .dxh1h .myClass {
        visibility: visible;
    }

    .dxgv .myClass:hover {
        visibility: visible;
    }
    /*   .CRMstatusGrid_row td.dxgv:first-child {
    border-right : 4px solid #f6f7fb  !important;
    }*/
    .CRMstatusGrid_row td.dxgv {
        border: 0px solid #f6f7fb !important;
        margin-top: 5px;
    }
    .dxgvEditFormDisplayRow_DevEx td.dxgv, .dxgvDetailCell_DevEx td.dxgv, .dxgvAdaptiveDetailCell_DevEx td.dxgv, .dxgvDataRow_DevEx td.dxgv, .dxgvAdaptiveDetailRow_DevEx td.dxgvAIC {
    overflow: hidden;
    border-bottom: 1px solid #d9dae0;
    border-right: 1px solid #d9dae0;
    border-top-width: 0;
    border-left-width: 0;
    padding: 4px 0px 2px 6px;
}

    .CRMstatusGrid_headerRow table tr td {
        /* text-align: left !important; */
        font-size: 16px !important;
        color: #4b4b4b;
        padding: 5px;
        font-weight: 500 !important;
    }

    .CRMstatusGrid_row td.dxgv a {
        font-size: 14px;
        color: black;
    }

    .detailviewresource-main {
        float: left;
        width: 100%;
        display: flex;
        margin: 16px 0px 0px;
        align-items: center;
    }

    .dxgvHeader_UGITNavyBlueDevEx {
        border: none;
        border-bottom: 0px solid #d9dae0;
    }

    @media only screen and (max-width: 1441px) {
        .rmm-project-title-workitem {
            width: 100%;
            height: fit-content;
            display: grid;
            align-content: center;
            background: #f2f3f4;
            color: black;
            padding: 5px 10px;
            border-radius: 10px;
            float: right;
        }

        .rmm-project-title-workitem-header table {
            width: 100% !important;
            float: right;
        }

        .innerGroup {
            color: black;
            padding: 5px 0px;
        }

        .CRMstatusGrid_row td.dxgv a, .rmm-project-common, .rmm-project-title-workitem a, .rmm-project-title-workitem-1 a {
            font-size: 13px;
        }
    }

    .display-inline-flex {
        display: inline-flex;
        justify-content: space-between;
    }

    .btnAllocation {
        padding: 2px 12px 2px 3px !important;
        background: #4fa1d6;
        margin-right: 5px;
        border-radius: 8px;
        margin-top: 5px;
        color: #FFFFFF !important;
    }

        .btnAllocation span {
            font-size: 15px;
            vertical-align: middle;
        }

    .btnTimesheet {
        transform: scale(1.3);
        padding-top: 6px;
        margin-right: 8px;
        margin-left: 22px;
    }

    .dxGridView_gvCollapsedButton_UGITNavyBlueDevEx {
        background-position: -42px -157px;
        width: 9px;
        height: unset;
    }

    .dxGridView_gvExpandedButton_UGITNavyBlueDevEx {
        background-position: -56px -157px;
        width: 9px;
        height: unset;
    }

    .resourceCheckbox input {
        margin-left: 6px;
        margin-top: -2px !important;
    }

    .dx-overlay-content {
        font-size: 18px !important;
        font-weight: 500;
    }
    /* .CRMstatusGrid_row > td {
      padding: 0px !important;
    }*/
    .showTimeOffViewIcon {
        width: 28px;
        margin: 10px;
        float: right;
    }
    /*.scrollContent::-webkit-scrollbar {
        width: 0.5em;
    }

    .scrollContent::-webkit-scrollbar-track {
        box-shadow: inset 0 0 6px rgba(0, 0, 0, 0.3);
    }

    .scrollContent::-webkit-scrollbar-thumb {
        background-color: #555;
    }

    .scrollContent:hover::-webkit-scrollbar {
        width: 0.7em;
    }*/

    .dxgvCSD::-webkit-scrollbar {
        width: 1em;
        height: 1em;
    }

    .dxgvCSD::-webkit-scrollbar-thumb {
        background-color: #555;
    }

    .dxgvCSD:hover::-webkit-scrollbar-thumb {
        background-color: #555;
    }

    .customgridview .dxgvCSD {
        background: white;
    }

    .alloctype {
        display: flex;
        justify-content: space-between;
        align-content: space-around;
        align-items: center;
    }

    .preconclass {
        border: 2px solid #52BED9;
        padding: 10px 20px;
        border-radius: 10px;
        float: right;
        font-weight: 500;
    }

        .preconclass a {
            color: #52BED9 !important;
        }

    .constclass a {
        color: #005C9B !important;
    }

    .constclass {
        border: 2px solid #005C9B;
        padding: 10px 20px;
        border-radius: 10px;
        float: right;
        font-weight: 500;
    }

    .closeoutclass a {
        color: #351B82 !important;
    }

    .closeoutclass {
        border: 2px solid #351B82;
        padding: 10px 20px;
        border-radius: 10px;
        float: right;
        font-weight: 500;
    }

    .datePreconClass {
        height: 45px;
        display: grid;
        align-content: center;
        text-align: center;
        color: #52BED9;
        padding: 7px;
        border: 2px solid #52BED9;
        font-size: 14px;
    }

    .dateConstClass {
        height: 45px;
        display: grid;
        align-content: center;
        text-align: center;
        color: #005C9B;
        padding: 7px;
        border: 2px solid #005C9B;
        font-size: 14px;
    }

    .dateCloseoutClass {
        height: 45px;
        display: grid;
        align-content: center;
        text-align: center;
        color: #351B82;
        padding: 7px;
        border: 2px solid #351B82;
        font-size: 14px;
    }

    .dxgvFocusedRow_UGITNavyBlueDevEx td.dxgvIndentCell, .dxgvFocusedGroupRow_UGITNavyBlueDevEx td.dxgvIndentCell, .dxgvSelectedRow_UGITNavyBlueDevEx td.dxgvIndentCell {
        background: white !important;
    }

    .allocationblockinner_sub{
        padding-top:0px;
    }

    .roleLabel {
        /* clear: both; */
        color: grey;
        /* position: absolute; */
        margin-left: 38px;
    }
</style>


<%if (!gvFilteredList.Visible)
    {%>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .rmm-project-title-workitem-header table {
        width: 100% !important;
        float: right;
    }

    .lineHorizontal__container {
        width: 100%;
        align-items: center;
        display: flex;
        height: 80px;
        float: right;
    }

    .innerGroup {
        color: black;
        padding: 5px 0px;
    }
</style>
<%} %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var baseUrl = ugitConfig.apiBaseUrl;
    var ganttSource = [];
    var date = new Date();
    var currentYearR = $.cookie('SelectedYear') != null && $.cookie('SelectedYear') != '' && $.isNumeric($.cookie('SelectedYear')) ? parseInt($.cookie('SelectedYear')) : date.getFullYear();
    $.cookie('SelectedYear', currentYearR);

    function ShowLoader() {
        $("#loadPanelContainer").dxLoadPanel("show");
    }
    function HideLoader() {
        $("#loadPanelContainer").dxLoadPanel("hide");
    }
    $(function () {
        $("#loadPanelContainer").dxLoadPanel({
            hideOnOutsideClick: false,
            message: "Loading...",
            shadingColor: "rgba(0, 0, 0, 0.2)",
            showPane: true,
        });
    });

    function OpenEditPopup(value, userid) {
        window.parent.UgitOpenPopupDialog('<%=editUrl %>&allocId=' + value, "", "Edit Resource Allocation", "650px", "500px");
    }
    function openworkitemingrid(value, username) {
        SaveUserInfo();
        //gridAllocation.SelectRowOnPage(1, true);
        var showTimeOff = "<%=this.ShowTimeOffOnly%>";
        var addRefreshPage = "&refreshpage=0";
        <% if (Height.IsEmpty || Width.IsEmpty)
    {%>
        addRefreshPage = "";
        <%}%>
        if (showTimeOff == "True") {
            window.parent.UgitOpenPopupDialog('<%=editUrl %>&allocId=' + value, "", "Edit Resource Allocation for " + username, "650px", "500px");
        }
        else {
            window.parent.UgitOpenPopupDialog('<%=editUrl %>&allocId=' + value + addRefreshPage, "", "Edit Resource Allocation for " + username, "650px", "500px");
        }
    }
    function CloseAndUpdatePage() {
        gridAllocation.Refresh();
        CloseWindowCallback();
        if ($("#<%=pnlAllocationTimeline.ClientID%>").is(":visible")) {
            grid.Refresh();
        }
    }
    function deleteworkitem(value) {

        var result = DevExpress.ui.dialog.confirm("<div>Are you sure you want to delete this allocation?</div>", "Confirm changes");
        result.done(function (dialogResult) {
            if (dialogResult) {
                callbackControl.PerformCallback(value);
                ShowLoader();
            }
        });
    }
    $(document).ready(function () {
        ChangeLocation();
        $(".rmm-project-title-cmic a").each(function () {
            if ($(this).text().length == 0) {
                $(this).parent().hide();
            }
        });
        $("#<%=resourceChk.ClientID%>").on("click", function () {
            ShowLoader();
            CheckUser('1', this.id, true);
        });
        let s_width = get_cookie("screenWidth");
        if (s_width < 1700) {
            $("#<%=ResourceListGrid.ClientID%>").removeClass("col-md-10").addClass("col-md-9");
            $("#<%=ResourceListTD.ClientID%>").removeClass("col-md-2").addClass("col-md-3");
        }
        <%if (!gvFilteredList.Visible)
    { %>
        $("#<%=ResourceListGrid.ClientID%>").removeClass("col-md-9").removeClass("col-md-10").addClass("col-md-12");
        $("#<%=ResourceListTD.ClientID%>").hide();
        <%}%>
        SaveUserInfo();
        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }
        ChangeWorkItemStyle();
    });
    function ChangeWorkItemStyle() {
        ChangeLocation();
        $(".rmm-project-title-workitem, .rmm-project-title-workitem-1").each(function () {
            if ($(this).attr("IsSummaryRow") != "True" && $(this).find("a").length) {
                if ($(this).attr("isallocinprecon") == "True" || $(this).attr("IsStartDateBeforePrecon") == "True") {
                    $(this).find("a").text("Precon");
                    $(this).attr("IsStartDateBeforePrecon") == "True" ? $(this).hide() : "" //$(this).css("border", "2px dashed #52BED9") : "";
                    $(this).removeClass("rmm-project-title-workitem rmm-project-title-workitem-1").addClass("preconclass");
                }
                else if ($(this).attr("isallocinconst") == "True" || $(this).attr("IsStartDateBetweenPreconAndConst") == "True") {
                    $(this).find("a").text("Const");
                    $(this).removeClass("rmm-project-title-workitem rmm-project-title-workitem-1").addClass("constclass");
                    $(this).attr("IsStartDateBetweenPreconAndConst") == "True" ? $(this).hide() : "" // $(this).css("border", "2px dashed #005C9B") : "";
                }
                else if ($(this).attr("isallocincloseout") == "True" || $(this).attr("IsStartDateBetweenConstAndCloseOut") == "True") {
                    $(this).find("a").text("CloseOut");
                    $(this).removeClass("rmm-project-title-workitem rmm-project-title-workitem-1").addClass("closeoutclass");
                    $(this).attr("IsStartDateBetweenConstAndCloseOut") == "True" ? $(this).hide() : "" // $(this).css("border", "2px dashed #351B82") : "";
                }
                else {
                    $(this).find("a").text(" ");
                    $(this).removeClass("rmm-project-title-workitem rmm-project-title-workitem-1");
                }
            }
            if ($(this).attr("isonhold") == "1") {
                $(this).find("a").css("color", "red");
            }
        });
    }
    function ChangeLocation() {
        $(".dxGridView_gvExpandedButton_UGITNavyBlueDevEx").attr("src", "/content/images/RMONE/left-arrow.png");
        $(".dxGridView_gvExpandedButton_UGITNavyBlueDevEx").addClass("expandClass");
        $(".dxGridView_gvCollapsedButton_UGITNavyBlueDevEx").attr("src", "/content/images/RMONE/left-arrow.png");
        $(".dxGridView_gvCollapsedButton_UGITNavyBlueDevEx").addClass("collapseClass");
        $("[id$='_gridAllocation_DXMainTable']").find("tr td.dxgv img").each(function () {
            $(this).addClass("imgHeightWidth");
            $(this).parent().next().find(".groupStyle").prepend(this);
            $(this).parent().next().find(".innerGroup").append(this);
        });
    }
    function NextYear() {
        let currentYear = parseInt($.cookie('SelectedYear')) + 1;
        $.cookie('SelectedYear', currentYear);
    }
    function PreviousYear() {
        let currentYear = parseInt($.cookie('SelectedYear')) - 1;
        $.cookie('SelectedYear', currentYear);
    }
    $(function () {
        
        try {
            var isNotPostBack = '<%=!Page.IsPostBack%>';
            if ($.cookie('activeview') != null && $.cookie('activeview') != "" && $.cookie('activeview') != "null" && $.cookie('activeview') != undefined) {
                var activeview = $.cookie('activeview');
                var btn = btnChangeView.GetMainElement();
                const btnTitle = btn.title;
                if (activeview == "cards") {
                    btnChangeView.SetVisible(true);
                    btnCardView.SetVisible(false);
                    $(".selectedYear").hide();
                    $("#<%=dvAssociateCardView.ClientID%>").show();
                    $("#<%=ResourceListGrid.ClientID%>").hide();
                    $("#<%=ResourceListTD.ClientID%>").hide();
                    $("#DivAllocationTimelinePanel").hide();
                }
                else if (activeview == "gantt") {
                    btnChangeView.SetVisible(false);
                    btnCardView.SetVisible(true);
                    $(".selectedYear").hide();
                    $("#<%=dvAssociateCardView.ClientID%>").hide();
                    $("#<%=ResourceListGrid.ClientID%>").hide();
                    $("#<%=ResourceListTD.ClientID%>").hide();
                    $("#DivAllocationTimelinePanel").show();
                }
                else {
                    btnChangeView.SetVisible(false);
                    btnCardView.SetVisible(true);
                    $("#<%=dvAssociateCardView.ClientID%>").hide();
                    $("#<%=ResourceListGrid.ClientID%>").show();
                    $("#<%=ResourceListTD.ClientID%>").show();
                    $("#DivAllocationTimelinePanel").hide();
                }
            }
            else {
                <%--$.cookie('activeview', "grid");
                btnChangeView.SetVisible(false);
                btnCardView.SetVisible(true);
                $("#<%=dvAssociateCardView.ClientID%>").hide();
                $("#<%=ResourceListGrid.ClientID%>").show();
                $("#<%=ResourceListTD.ClientID%>").show();
                $("#DivAllocationTimelinePanel").hide();--%>
                $.cookie('activeview', "cards");
                btnChangeView.SetVisible(true);
                btnCardView.SetVisible(false);
                $(".selectedYear").hide();
                $("#<%=dvAssociateCardView.ClientID%>").show();
                $("#<%=ResourceListGrid.ClientID%>").hide();
                $("#<%=ResourceListTD.ClientID%>").hide();
                $("#DivAllocationTimelinePanel").hide();
            }


            var urlParams = new URLSearchParams(window.location.search);
            var redirectFromCard = urlParams.get('isRedirectFromCardView');
            if (redirectFromCard == 'true') {
                $("#<%=dvAssociateCardView.ClientID%>").hide();
                $("#<%=dvAllocationHeader.ClientID%>").hide();
                $("#<%=ResourceListGrid.ClientID%>").show();
                $("#<%=ResourceListTD.ClientID%>").hide();
            }

            if ($("#associateCard").length > 0 && $("#associateCard").is(":visible")) {
                $("#associateCard").dxScrollView('instance').scrollBy(10);
                $("#associateCard").dxScrollView('instance').scrollBy(-10);
            }

            $('#<%=chkIncludeClosed.ClientID%>').click(function (e) {
                ShowLoader();
            });
            var showTimeOff = "<%=this.ShowTimeOffOnly%>";
            if (showTimeOff == "True") {
                $("#<%=dvAssociateCardView.ClientID%>").hide();
                $("#<%=ResourceListGrid.ClientID%>").hide();
                //$("#<%=ResourceListTD.ClientID%>").hide();
                $("#DivAllocationTimelinePanel").hide();
                $("#<%=dvAllocationHeader.ClientID%>").hide();
                //$(".allocationrow").hide();
                $(".selectedYear").hide();
                $("#<%=ResourceListGrid.ClientID%>").removeClass("col-md-9").removeClass("col-md-10").addClass("col-md-12");
            }
            var isRedirectFromUtilization = "<%=IsRedirectFromUtilization%>";
            if (isRedirectFromUtilization == "True") {

                $("#DivAllocationTimelinePanel").hide();
                $("#<%=dvAllocationHeader.ClientID%>").hide();
                //$(".allocationrow").hide();
                $(".selectedYear").hide();
            }
            var redirectFromCapacityReport = urlParams.get('capacityreport');
            if (redirectFromCapacityReport == "true") {
                btnCardView.SetVisible(false);
                $(".timeoffClass").hide();
            }
        }
        catch (ex) {
            console.log(ex);
        }


        $("[id$='_gridAllocation_DXMainTable']").find('tr').each(function (i, item) {
            if ($('#<%=hndAllocation.ClientID%>').val() == 'ExpandAll') {
                if ($(item).attr('workitem') != undefined) {
                    if ($(item).find("#imgMin").length > 0) {
                        $(item).find("#imgMin").attr('src', '/contents/images/minimise.gif');
                        $(item).find("#imgMin").attr('colexpand', 'true');
                    }
                    else {
                        $(item).css('display', '');
                    }
                }
            }
            else if ($('#<%=hndAllocation.ClientID%>').val() == 'CollapseAll') {
                if ($(item).attr('workitem') != undefined) {
                    if ($(item).find("#imgMin").length > 0) {
                        $(item).find("#imgMin").attr('src', '/content/images/maximise.gif');
                        $(item).find("#imgMin").attr('colexpand', 'true');
                    }
                    else {
                        $(item).css('display', 'none');
                    }
                }
            }
            else if ($(item).attr('workitem') == $('#<%=hndAllocation.ClientID%>').val()) {
                if ($(item).find("#imgMin").length > 0) {
                    $(item).find("#imgMin").attr('src', '/content/images/minimise.gif');
                    $(item).find("#imgMin").attr('colexpand', 'true');
                }
                else {
                    $(item).css('display', '');
                }
            }
        });
        $(".datetimectr111").parents("table").find("img").bind("click", function (e) {
            addHeightToCalculateFrameHeight(this, 200);
        });

        //Put &nbsp; in action container if empty
        $.each($(".action-container"), function () {
            if ($.trim($(this).html()) == "") {
                $(this).html("&nbsp;");
            }
        });

        $("input:text").bind("keypress", function (event) {
            return event.keyCode != 13;
        });


        try {
            $(".jqtooltip").tooltip({
                hide: { duration: 4000, effect: "fadeOut", easing: "easeInExpo" },
                content: function () {
                    var title = $(this).attr("title");
                    if (title)
                        return title.replace(/\n/g, "<br/>");
                }
            });
        }
        catch (ex) {
            console.log(ex);
        }
    });
    function toggleTimeOffView() {
        let elem = $("#<%=ResourceListGrid.ClientID%>");
        let toggleElem = $(".toggleTimeOff img");
        if (elem.is(":visible")) {
            elem.hide();
            $("#DivTimeOffCardView").show();
            toggleElem.attr("src", "/content/Images/gridBlackNew.png");
            toggleElem.attr("title", 'Show Grid View');
        }
        else {
            $("#DivTimeOffCardView").hide();
            elem.show();
            toggleElem.attr("src", "/content/Images/cardViewBlack-new.png");
            toggleElem.attr("title", 'Show Card View');
        }
    }
    function ChangeView(s, e) {

        pnlAllocationTimeline.SetVisible(false);
        if (typeof btnChangeView != "undefined") {

            if ($.cookie('activeview') != null && $.cookie('activeview') != "" && $.cookie('activeview') != "null" && $.cookie('activeview') != undefined) {
                var activeview = $.cookie('activeview');
                var btn = btnChangeView.GetMainElement();
                const btnTitle = btn.title;

                if (activeview == "cards") {
                    $.cookie('activeview', "grid");
                    btnChangeView.SetImageUrl("/content/Images/cardViewBlack-new.png");
                    btnChangeView.title = 'Show Card View';
                    $("#<%=dvAssociateCardView.ClientID%>").hide();
                    $("#<%=ResourceListGrid.ClientID%>").show();
                    $("#<%=ResourceListTD.ClientID%>").show();
                }
                else {
                    $.cookie('activeview', "cards");
                    btnChangeView.title = 'Show Grid View';
                    btnChangeView.SetImageUrl("/content/Images/gridBlackNew.png");
                    $("#<%=dvAssociateCardView.ClientID%>").show();
                    $("#<%=ResourceListGrid.ClientID%>").hide();
                    $("#<%=ResourceListTD.ClientID%>").hide();
                }
            }
            else {
                $.cookie('activeview', "cards");
                btnChangeView.title = 'Show Grid View';
                btnChangeView.SetImageUrl("/content/Images/gridBlackNew.png");
                $("#<%=dvAssociateCardView.ClientID%>").show();
                $("#<%=ResourceListGrid.ClientID%>").hide();
                $("#<%=ResourceListTD.ClientID%>").hide();      //allocationPanel.Visible = true;
            }
        }

    }

    function ChangeToGridView(s, e) {
        ShowLoader();
        $.cookie('activeview', "grid");
        btnChangeView.SetVisible(false);
        btnCardView.SetVisible(true);
        $(".selectedYear").show();
        $("#<%=dvAssociateCardView.ClientID%>").hide();
        $("#<%=ResourceListGrid.ClientID%>").show();
        $("#<%=ResourceListTD.ClientID%>").show();
        $("#DivAllocationTimelinePanel").hide();
        hideCommonViews();
        if ($.cookie('SelectedYear') != null && $.cookie('SelectedYear') != '')
            $("#<%=lblSelectedYear.ClientID%>").text($.cookie('SelectedYear'));
        CloseAndUpdatePage();
        setTimeout(HideLoader, 500);
    }

    function ChangeToCardView(s, e) {
        $.cookie('activeview', "cards");
        btnChangeView.SetVisible(true);
        btnCardView.SetVisible(false);
        $(".selectedYear").hide();
        $("#<%=dvAssociateCardView.ClientID%>").show();
        $("#<%=ResourceListGrid.ClientID%>").hide();
        $("#<%=ResourceListTD.ClientID%>").hide();
        $("#DivAllocationTimelinePanel").hide();
        hideCommonViews()
    }

    function ChangeToGanttView(s, e) {
        ShowLoader();
        $.cookie('activeview', "gantt");
        $("#<%=dvAssociateCardView.ClientID%>").hide();
        $("#<%=ResourceListGrid.ClientID%>").hide();
        $("#<%=ResourceListTD.ClientID%>").hide();
        $("#DivAllocationTimelinePanel").show();
        hideCommonViews();
        SaveUserInfo();
        grid.Refresh();
        setTimeout(HideLoader, 500);
    }

    function hideCommonViews() {
        $("#<%=btnCollapseAll.ClientID%>").hide();
        $("#<%=btnExpandAll.ClientID%>").hide();
        $("#<%=btExpandAllAllocation.ClientID%>").show();
        $("#<%=btCollapseAllAllocation.ClientID%>").show();

        if ($("#associateCard").length > 0) {
            $("#associateCard").dxScrollView('instance').scrollBy(10);
            $("#associateCard").dxScrollView('instance').scrollBy(-10);
        }
    }

    function adjustControlSize() {
        setTimeout(function () {
            $("#s4-workspace").width("100%");
            var height = $(window).height();
            $("#s4-workspace").height(height);
        }, 10);
    }

    function DeleteCookies(name, location) {
        if ($.cookie('SelectedUsers') != null && $.cookie('SelectedUsers') != "" && $.cookie('SelectedUsers') != "null" && $.cookie('SelectedUsers') != undefined) {
            $.cookie("SelectedUsers", null);
        }
    }
    function SaveUserInfo() {
        var hControl = document.getElementById("<%=hdnSelectedUserList.ClientID %>");
        var showBt = document.getElementById("<%=btShowAllocation.ClientID %>");
        var hControlManager = document.getElementById("<%=hdnCmbResourceManager.ClientID%>");
        try {
            var sallCtr = document.getElementById("<%=hdnSelectedAllocation.ClientID%>");
            var swiCtr = document.getElementById("<%=hdnSelectedWorkItem.ClientID%>");
            sallCtr.value = "";
            swiCtr.value = "";
        } catch (ex) {
        }

        var checkboxes = $(".usercheck11");
        var vals = "";
        var checkedUsers = checkboxes.filter(":checked");
        checkedUsers.each(function (i, item) {
            if (i != 0) {
                vals += ",";
            }

            var userrarray = item.id.split('-');
            userrarray.splice(0, 1);
            var userifno = userrarray.join('-');
            vals += userifno;
        });
        hControl.value = vals;
        if ($.cookie('userall') != null && $.cookie('userall') != "" && $.cookie('userall') != "null" && $.cookie('userall') != undefined) {
            $.cookie("userall", null);
        }
        if ($.cookie('SelectedUsers') != null && !$.cookie('SelectedUsers') != "" && $.cookie('SelectedUsers') != "null" && $.cookie('SelectedUsers') != undefined) {
            $.cookie("SelectedUsers", null);
        }
        if (typeof cmbResourceManager != 'undefined') {
            var managerval = cmbResourceManager.GetValue();
            if ($.cookie('usermanagers') != null && $.cookie('usermanagers') != "" && $.cookie('usermanagers') != "null" && $.cookie('usermanagers') != undefined) {
                $.cookie("usermanagers", null);
            }
            hControlManager.value = managerval;
            if ($.cookie('newresource') != null && $.cookie('newresource') != "" && $.cookie('newresource') != "null" && $.cookie('newresource') != undefined) {
                $.cookie("newresource", null);
            }
            $.cookie("newresource", managerval);
            $.cookie("usermanagers", managerval);
            $.cookie("userall", vals);
            $.cookie("SelectedUsers", vals);
        }
        else {
            return;
        }
    }

    function CheckUser(title, controlId, isCheckboxEvent) {
        ShowLoader();
        var control = document.getElementById(controlId);
        var hControl = document.getElementById("<%=hdnSelectedUserList.ClientID %>");
        var showBt = document.getElementById("<%=btShowAllocation.ClientID %>");
        var hControlManager = document.getElementById("<%=hdnCmbResourceManager.ClientID%>");
        try {
            var sallCtr = document.getElementById("<%=hdnSelectedAllocation.ClientID%>");
            var swiCtr = document.getElementById("<%=hdnSelectedWorkItem.ClientID%>");
            sallCtr.value = "";
            swiCtr.value = "";
        } catch (ex) {
        }

        var checkboxes = $(".usercheck11");
        if (title == "all") {
            hControl.value = "";

            var vals = "";
            $.each(checkboxes, function (i, item) {

                if (control.checked) {
                    item.checked = true;
                    if (i != 0)
                        vals += ",";
                    var userrarray = item.id.split('-');
                    userrarray.splice(0, 1);
                    var userifno = userrarray.join('-');
                    vals += userifno;
                }
                else {
                    item.checked = false;
                }
            });
            if (control.checked) {
                hControl.value = vals;
            }
            else {
                hControl.value = "";
            }

            if ($.cookie('userall') != null && $.cookie('userall') != "" && $.cookie('userall') != "null" && $.cookie('userall') != undefined) {
                $.cookie("userall", null);
            }
            if ($.cookie('SelectedUsers') != null && $.cookie('SelectedUsers') != "" && $.cookie('SelectedUsers') != "null" && $.cookie('SelectedUsers') != undefined) {
                $.cookie("SelectedUsers", null);
            }
            var managerval = cmbResourceManager.GetValue();
            if ($.cookie('usermanagers') != null && $.cookie('usermanagers') != "" && $.cookie('usermanagers') != "null" && $.cookie('usermanagers') != undefined) {
                $.cookie("usermanagers", null);
            }
            hControlManager.value = managerval;
            $.cookie("usermanagers", managerval);
            $.cookie("userall", vals);
            $.cookie("SelectedUsers", vals);
        }
        else {
            var vals = "";
            if (isCheckboxEvent == true) { //Condition added, to know if allocation details requested on click of checkbox or linkbutton.
                var checkedUsers = checkboxes.filter(":checked");
                checkedUsers.each(function (i, item) {
                    if (i != 0) {
                        vals += ",";
                    }

                    var userrarray = item.id.split('-');
                    userrarray.splice(0, 1);
                    var userifno = userrarray.join('-');
                    vals += userifno;
                });
            }
            else {
                vals = title;
            }
            hControl.value = vals;
            if ($.cookie('userall') != null && $.cookie('userall') != "" && $.cookie('userall') != "null" && $.cookie('userall') != undefined) {
                $.cookie("userall", null);
            }
            if ($.cookie('SelectedUsers') != null && !$.cookie('SelectedUsers') != "" && $.cookie('SelectedUsers') != "null" && $.cookie('SelectedUsers') != undefined) {
                $.cookie("SelectedUsers", null);
            }
            var managerval = cmbResourceManager.GetValue();
            if ($.cookie('usermanagers') != null && $.cookie('usermanagers') != "" && $.cookie('usermanagers') != "null" && $.cookie('usermanagers') != undefined) {
                $.cookie("usermanagers", null);
            }
            hControlManager.value = managerval;
            $.cookie("usermanagers", managerval);
            $.cookie("userall", vals);
            $.cookie("SelectedUsers", vals);
        }
        AddNotification("Processing ..");
        showBt.click();
    }

    function setcheckbox(obj) {
        var arrySelectedUsers = []
        var checkboxes = $(".usercardcheck");

        var selectedUsers = $.cookie('SelectedUsers');
        var allUserChkBoxCount = checkboxes != null ? checkboxes.length : 0;

        if (selectedUsers != null && selectedUsers != '') {
            arrySelectedUsers = selectedUsers.split(',');
        }
        var userall = $.cookie("userall");
        $.each(checkboxes, function (i, item) {

            var userrarray = item.id.split('-');
            userrarray.splice(0, 1);
            var userifno = userrarray.join('-');
            if (arrySelectedUsers.indexOf(userifno) > -1) {
                item.checked = true;
            }
        });
        var checkedUsers = checkboxes.filter(":checked");
        if (checkedUsers != null && checkedUsers.length > 0) {
            $(".dvShowAllocationBtn").css("display", "block");
            if (checkedUsers.length == allUserChkBoxCount) {
                checkAllAssociate.option('value', true);
            }
            else
                checkAllAssociate.option('value', false);
        }
        else
            $(".dvShowAllocationBtn").css("display", "none");

    }

    function CheckCardUser(title, controlId) {
        var control = document.getElementById(controlId);
        var hControl = document.getElementById("<%=hdnSelectedUserList.ClientID %>");
        var showBt = document.getElementById("<%=btShowAllocation.ClientID %>");
        try {
            var sallCtr = document.getElementById("<%=hdnSelectedAllocation.ClientID%>");
            var swiCtr = document.getElementById("<%=hdnSelectedWorkItem.ClientID%>");
            sallCtr.value = "";
            swiCtr.value = "";
        } catch (ex) {
        }

        var checkboxes = $(".usercardcheck");
        if (title == "all") {
            hControl.value = "";

            var vals = "";
            $.each(checkboxes, function (i, item) {

                if (checkAllAssociate.option('value')) {
                    item.checked = true;
                    if (i != 0)
                        vals += ",";
                    var userrarray = item.id.split('-');
                    userrarray.splice(0, 1);
                    var userifno = userrarray.join('-');
                    vals += userifno;
                    $(".dvShowAllocationBtn").css("display", "block");
                }
                else {
                    item.checked = false;
                    $(".dvShowAllocationBtn").css("display", "none");
                }
            });
            if (checkAllAssociate.option('value')) {
                hControl.value = vals;
            }
            else {
                hControl.value = "";
            }

            if ($.cookie('userall') != null && $.cookie('userall') != "" && $.cookie('userall') != "null" && $.cookie('userall') != undefined) {
                $.cookie("userall", null);
            }
            if ($.cookie('SelectedUsers') != null && $.cookie('SelectedUsers') != "" && $.cookie('SelectedUsers') != "null" && $.cookie('SelectedUsers') != undefined) {
                $.cookie("SelectedUsers", null);
            }
            var managerval = cmbResourceManager.GetValue();
            if ($.cookie('usermanagers') != null && $.cookie('usermanagers') != "" && $.cookie('usermanagers') != "null" && $.cookie('usermanagers') != undefined) {
                $.cookie("usermanagers", null);
            }
            $.cookie("usermanagers", managerval);
            $.cookie("userall", vals);
            $.cookie("SelectedUsers", vals);
        }
        else {
            var checkedUsers = checkboxes.filter(":checked");
            if (checkedUsers != null && checkedUsers.length > 0)
                $(".dvShowAllocationBtn").css("display", "block");
            else
                $(".dvShowAllocationBtn").css("display", "none");

            var vals = "";
            checkedUsers.each(function (i, item) {
                if (i != 0) {
                    vals += ",";
                }
                var userrarray = item.id.split('-');
                userrarray.splice(0, 1);
                var userifno = userrarray.join('-');
                vals += userifno;
            });
            //hControl.value = vals;        
            if ($.cookie('userall') != null && $.cookie('userall') != "" && $.cookie('userall') != "null" && $.cookie('userall') != undefined) {
                $.cookie("userall", null);
            }
            if ($.cookie('SelectedUsers') != null && !$.cookie('SelectedUsers') != "" && $.cookie('SelectedUsers') != "null" && $.cookie('SelectedUsers') != undefined) {
                $.cookie("SelectedUsers", null);
            }
            var managerval = cmbResourceManager.GetValue();
            if ($.cookie('usermanagers') != null && $.cookie('usermanagers') != "" && $.cookie('usermanagers') != "null" && $.cookie('usermanagers') != undefined) {
                $.cookie("usermanagers", null);
            }
            $.cookie("usermanagers", managerval);
            $.cookie("userall", vals);
            $.cookie("SelectedUsers", vals);
        }
        // AddNotification("Processing ..");
        // showBt.click();
    }

    function ShowAllocationOfSelectedUser(checkboxId, user) {
        var showBt = document.getElementById("<%=btShowAllocation.ClientID %>");
        var box = document.getElementById(checkboxId);
        if (box.checked == true) {
            box.checked = false;
        }
        else {
            box.checked = true;
        }
        CheckUser("", box.id, true);
    }

    function MoveUp(user) {
        MoveDown(user);
    <%--ShowLoader();
    try {
        var sallCtr = document.getElementById("<%=hdnSelectedAllocation.ClientID%>");
        var swiCtr = document.getElementById("<%=hdnSelectedWorkItem.ClientID%>");
        sallCtr.value = "";
        swiCtr.value = "";
    } catch (ex) {
    }
    var parentOf = document.getElementById("<%=hdnParentOf.ClientID %>");
    var childOf = document.getElementById("<%=hdnChildOf.ClientID %>");
    var showBt = document.getElementById("<%=btShowAllocation.ClientID %>");
    var hControl = document.getElementById("<%=hdnSelectedUserList.ClientID %>");
    if (user != undefined && user != '' && user != null)
        cmbResourceManager.SetValue(user);
    else
        user = cmbResourceManager.GetValue();
    hControl.value = "";
    parentOf.value = user;
    childOf.value = "";
    if ($.cookie('managerchanged') != null && $.cookie('managerchanged') != "" && $.cookie('managerchanged') != "null" && $.cookie('managerchanged') != undefined) {
        $.cookie("managerchanged", null);
    }
    $.cookie("managerchanged", "true");
    AddNotification("Processing ..");
    showBt.click();--%>
    }

    function MoveDown(user) {
        ShowLoader();
        if (user == null || user == undefined || user == '') {
            user = cmbResourceManager.GetValue();
        }
        else {
            cmbResourceManager.SetValue(user);

        }

        setusercookies(user);
        try {
            var sallCtr = document.getElementById("<%=hdnSelectedAllocation.ClientID%>");
        var swiCtr = document.getElementById("<%=hdnSelectedWorkItem.ClientID%>");
            sallCtr.value = "";
            swiCtr.value = "";
        } catch (ex) {
        }
        var childOf = document.getElementById("<%=hdnChildOf.ClientID %>");
        var parentOf = document.getElementById("<%=hdnParentOf.ClientID %>");
        var showBt = document.getElementById("<%=btShowAllocation.ClientID %>");
        var hControl = document.getElementById("<%=hdnSelectedUserList.ClientID %>");
        if (hControl != null)
            hControl.value = "";
        if (parentOf != null)
            parentOf.value = "";
        if (childOf != null)
            childOf.value = user;
        AddNotification("Processing ..");
        if (showBt != null)
            showBt.click();
    }

    var notifyId = "";
    function AddNotification(msg) {
        ShowLoader();
    }
    ShowLoader();
    function RemoveNotification() {
        try {
            SP.UI.Notify.removeNotification(notifyId);
            notifyId = '';
        }
        catch (ex) { }
    }

    function editMonthDistribution() {

        //var editBt = $(".btnEditDistribution");
        //if (editBt.length > 0) {
        //    $.globalEval(editBt.attr("href"));
        //}
    }

    function OpenCalendar() {
        var userid = cmbResourceManager.GetValue();
        window.parent.UgitOpenPopupDialog('<%=absoluteCalendarUrl %>&selecteduser=' + userid, "", "Out-Of-Office Team Calendar", "90", "90");
    }

    function OpenTimesheet() {
        window.parent.UgitOpenPopupDialog('<%=viewTimesheetPath%>', '', 'Timesheet', '85%', '900', 0, "", true, true);
    }

    function gridAllocation_ContextMenuItemClick(sender, args) {
        if (args.item.name == "Sync") {
            ShowLoader();
            args.processOnServer = true;
            args.usePostBack = true;
        }
    }

    function OnContextMenu(s, e) {

        if (e.objectType == "row") {
            var menuItemSelectedApprove = e.menu.GetItemByName("Sync");
            var workItemType = s.cpWorkItemType[e.index];
            var flag = false;
            if (workItemType == "<%=uGovernIT.Manager.uHelper.GetModuleTitle("PMM")%>" || workItemType == "<%=uGovernIT.Manager.uHelper.GetModuleTitle("NPR")%>" || workItemType == "<%=uGovernIT.Manager.uHelper.GetModuleTitle("TSK")%>")
                menuItemSelectedApprove.SetVisible(true);
            else
                menuItemSelectedApprove.SetVisible(false);
        }
        else {
            var menuItemSelectedApprove = e.menu.GetItemByName("Sync");
            menuItemSelectedApprove.SetVisible(false);

        }
    }



    function setusercookies(userid) {

        if ($.cookie('newresource') != null && $.cookie('newresource') != "" && $.cookie('newresource') != "null" && $.cookie('newresource') != undefined) {
            $.cookie("newresource", null);
        }
        $.cookie("newresource", userid);
        if ($.cookie('selectedResource') != null && $.cookie('selectedResource') != "" && $.cookie('selectedResource') != "null" && $.cookie('selectedResource') != undefined) {
            $.cookie("selectedResource", null);
        }
        $.cookie("selectedResource", userid);
        if ($.cookie('userall') != null && $.cookie('userall') != "" && $.cookie('userall') != "null" && $.cookie('userall') != undefined) {
            $.cookie("userall", null);
        }
        if ($.cookie('SelectedUsers') != null && !$.cookie('SelectedUsers') != "" && $.cookie('SelectedUsers') != "null" && $.cookie('SelectedUsers') != undefined) {
            $.cookie("SelectedUsers", null);
        }
        if ($.cookie('managerchanged') != null && $.cookie('managerchanged') != "" && $.cookie('managerchanged') != "null" && $.cookie('managerchanged') != undefined) {
            $.cookie("managerchanged", null);
        }
        $.cookie("managerchanged", "true");

        //$.cookie("SelectedYear", null);

        if ($.cookie('IsGanttViewExpanded') != null)
            $.cookie('IsGanttViewExpanded', "1", { path: '/' });
        if ($.cookie('GanttViewExpandedUsers') != null)
            $.cookie('GanttViewExpandedUsers', "all", { path: '/' });
        if ($.cookie('GanttViewCollapsedUsers') != null)
            $.cookie('GanttViewCollapsedUsers', "", { path: '/' });

    }

    function PendingTimesheetApprovals(Id, Name) {
        window.parent.UgitOpenPopupDialog('<%=timesheetPendingAprvlPath %>&Id=' + Id, "", "Timesheet Pending Approvals for " + Name, "600px", "500px");
    }

    function OpenAddAllocationPopup(obj, userName, workitemtype) {
        window.parent.UgitOpenPopupDialog('<%= AddOpenAllocationUrl %>' + '&SelectedUsersList=' + obj + '&WorkItemType=' + workitemtype, "", 'Add Allocation for ' + userName, '680px', '500px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
    function OpenMultiAllocationPopup(obj, userName, workitemtype) {
        if (workitemtype == "Time Off") {
            let str = "/Layouts/ugovernit/DelegateControl.aspx?control=ptomultiallocationjs&pageTitle=Add Non Project Allocations&Type=time off&SelectedUsersList=" + obj + "&IncludeClosed=false";
            window.parent.parent.UgitOpenPopupDialog(str + '&SelectedUsersList=' + obj + '&WorkItemType=' + workitemtype, "", 'Add Allocation for ' + userName, '880px', '700px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        }
        else {
            window.parent.parent.UgitOpenPopupDialog('<%= AddCombineMultiAllocationUrl %>' + '&SelectedUsersList=' + obj + '&WorkItemType=' + workitemtype, "", 'Add Allocation for ' + userName, '880px', '700px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        }
    }
    function OpenCombinedMultiAllocationPopup(obj, userName, workitemtype) {
        window.parent.parent.UgitOpenPopupDialog('<%= AddCombineMultiAllocationUrl %>' + '&SelectedUsersList=' + obj + '&WorkItemType=' + workitemtype, "", 'Add Allocation for ' + userName, '880px', '700px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
    function OpenUserResume(userId) {
        window.parent.parent.UgitOpenPopupDialog('<%= OpenUserResumeUrl %>' + '&SelectedUser=' + userId, "", 'User Resume', '95', '95', "", false);
    }
</script>

<script data-v="<%=UGITUtility.AssemblyVersion %>">


    function OpenAllocationGantt() {
        let selectedUsers = $.cookie("SelectedUsers");
        let userall = $.cookie("userall");
        //let selectedUser = $.cookie("selectedResource");
        let selectedUser = '<%=cmbResourceManager.SelectedItem.Value%>';
        let selectedYear = encodeURIComponent("<%=lblSelectedYear.Text%>");
        let includeClosed = '<%=chkIncludeClosed.Checked%>';
        let params = `userall=${userall}&selectedUser=${selectedUser}&selectedYear=${selectedYear}&includeClosed=${includeClosed}&selectedUsers=${selectedUsers}`;
        window.parent.UgitOpenPopupDialog('<%=allocationGanttUrl %>', params, "Resource Allocation Timeline", "90", "655px");

    }

    var ganttPopupDetails = null;
    var ganttPopupDetailOptions = {
        width: 1100,
        height: 500,
        contentTemplate: function () {

            //var maxDaterange = new Date(Math.max.apply(null, ganttSource.map(function (e) {
            //    return new Date(e.AllocationEndDate);

            //})));

            //var minDaterange = new Date(Math.min.apply(null, ganttSource.map(function (e) {
            //    return new Date(e.AllocationStartDate);

            //})));

            return $("<div />").append(
                $("#divAllocationGantt").dxGantt({
                    tasks: {
                        dataSource: ganttSource,
                        endExpr: "AllocationEndDate",
                        keyExpr: "ChildId",
                        parentIdExpr: "ParentId",
                        progressExpr: "PctAllocation",
                        startExpr: "AllocationStartDate",
                        titleExpr: "ResourceUser"
                    },
                    resources: {
                        dataSource: ganttSource,
                        keyExpr: "Id",
                        textExpr: "SubWorkItem"
                    },
                    resourceAssignments: {
                        dataSource: ganttSource,
                        keyExpr: "Id",
                        textExpr: "ResourceUser"
                    },
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
                    toolbar: {
                        items: [
                            'separator',
                            'collapseAll',
                            'expandAll',
                            'separator',
                            'separator',
                            'zoomIn',
                            'zoomOut',
                        ],
                    },
                    scaleType: 'months',
                    taskListWidth: 500,
                    taskContentTemplate: getTaskContentTemplate,
                    //startDateRange: new Date(minDaterange.getFullYear(), minDaterange.getMonth() - 1, 01),
                    //endDateRange: new Date(maxDaterange.getFullYear(), 11, 31)
                })
            );
        },
        showTitle: true,
        title: "Resource Allocations",
        dragEnabled: true,
        hideOnOutsideClick: true
    };

    var showResourceAllocationGantt = function () {
        if (ganttPopupDetails) {
            ganttPopupDetails.option("contentTemplate", ganttPopupDetailOptions.contentTemplate.bind(this));
        } else {
            ganttPopupDetails = $("#divAllocationGanttPopup").dxPopup(ganttPopupDetailOptions).dxPopup("instance");
        }

        ganttPopupDetails.show();
    };

    function getTaskContentTemplate(item) {

        const resource = item.taskResources[0];
        const img = "/Content/Images/girlprofilepic.jpg";
        const color = 0;
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
            .text(item.taskData.Title)
            .appendTo($wrapper);
        $(document.createElement('div'))
            .addClass('custom-task-row')
            .text(`${item.taskData.ResourceUser} (${item.taskData.PctAllocation}%)`)
            .appendTo($wrapper);

        return $customContainer;
    }

    function btnAddMultiAllocation_Click(s, e) {
        var url = '<%= MultiAddUrl%>';
        window.parent.UgitOpenPopupDialog(url, '', '', '880px', '90', 0, '');
    }

    function btCollapseAllAllocation() {
        //alert('working collapse');
        return false;
    }

    function btExpandAllAllocation() {
        //alert('working expand')
        return false;
    }
    function showTimeOff() {
        let src = '<%=timeOffAllocationLink%>'
        let selectedYear = get_cookie("SelectedYear");
        if (selectedYear != null && selectedYear != undefined) {
            src += "&selectedYear=" + selectedYear;
        }
        window.parent.UgitOpenPopupDialog(src, '', 'Time Off', '75%', '900', 0, "", true, true);
    }

    function OpenGanttScreen() {
        SaveUserInfo();
        let src = '<%=GanttPopupLink%>'
        src += "&RequestFromManagerScreen=true&includeClosed=" + '<%=chkIncludeClosed.Checked%>';
        $.cookie('SelectedUsersForGantt', $("#<%=hdnSelectedUsersForGantt.ClientID%>").val(), { path: '/' });
        window.parent.UgitOpenPopupDialog(src, "", "Timeline", "95", "95", "", false);
    }
</script>
<asp:HiddenField ID="hdnSelectedUsersForGantt" runat="server" Value=""/>
<asp:HiddenField ID="hndAllocation" Value="" runat="server" />
<asp:HiddenField ID="hdnSelectedAllocation" runat="server" />
                        <asp:HiddenField ID="hdnSelectedWorkItem" runat="server" />
                        <asp:HiddenField ID="hdnSelectedUserList" runat="server" />
                        <asp:HiddenField ID="hdnParentOf" runat="server" />
                        <asp:HiddenField ID="hdnChildOf" runat="server" />
                        <asp:HiddenField ID="hdnCmbResourceManager" runat="server" />
<dx:ASPxLoadingPanel ID="CustomResourceLoadingPanel" runat="server" Text=" Please Wait ..." ClientInstanceName="CustomResourceLoadingPanel" Modal="True">
<Image Url="~/Content/Images/ajax-loader.gif"></Image>
</dx:ASPxLoadingPanel>
<div id="loadPanelContainer"></div>
<% if (!Height.IsEmpty && !Width.IsEmpty)
    { %>
<div class="row scrollContent"  style="width: <%=Width%>; height: <%=Height%>;">
        <% }
            else
            { %>
<div class="row">
      <% }%>
     <div style="display: none;">
            <dx:ASPxDateEdit ID="hiddenDate" runat="server"></dx:ASPxDateEdit>
        </div>
    <dx:ASPxCallback ID="callbackControl" runat="server"  ClientInstanceName="callbackControl" OnCallback="callbackControl_Callback" >
        <ClientSideEvents EndCallback="function(s, e){ window.location.reload(); }" />
    </dx:ASPxCallback>
    <div class="allocationblockinner col-md-12 col-xs-12 col-sm-12 noSidePadding">
        <div class="detailviewresource-main row g-5"  runat="server" id="dvAllocationHeader">
           <%-- <div class="detailviewresource-m">
                <div class="width100 bordercolps row">--%>
                    <div class="col-md-1" style="padding-left:0px;padding-right:5px;text-align:initial;">      
                        <span>
                            <asp:Label ID="lbSearchViewDrop" CssClass="selectedresourcelbheading" runat="server" Text="Resource:"></asp:Label>
                            <asp:CheckBox ID="resourceChk" CssClass="resourceCheckbox" runat="server"/>
                        </span>
                            
                            
                     </div>
            <div class="col-md-3" style="padding-left:5px;padding-right:5px;">  
                <div style="float:left;">
                             <dx:ASPxComboBox ID="cmbResourceManager" DropDownHeight="300px" CssClass="managerdropdown resourceDrpDown" EnableClientSideAPI="true" 
                                ClientInstanceName="cmbResourceManager" runat="server">
                                 <ClientSideEvents SelectedIndexChanged="function(s, e) { MoveDown(); }" />
                            </dx:ASPxComboBox>
                    
                               
                       </div>
                <span style="float:left;" class="mt-1">
                <img align='absmiddle' style="width:22px;float:right;cursor: pointer; margin-left:8px;" src="/content/images/plus-blue-new.png" title="New Allocation" onclick="javascript:event.cancelBubble=true; OpenCombinedMultiAllocationPopup(cmbResourceManager.GetValue(), cmbResourceManager.GetText(), '')">

                <img src="/content/images/uparrow_new.png" width="22"
                                    align='absmiddle' style="cursor: pointer; margin-left:8px;" id="moveUp" runat="server" alt="Up"
                                    title="Move up" />
                    </span>
                </div>
                <div class="col-md-6 display-inline-flex">
                    <div>
                          <asp:Label ID="lbDivision" CssClass="resourceLabel" runat="server"></asp:Label>
                          <asp:Label ID="lbDivisionVal" runat="server" Text="" CssClass="resouceValue" Font-Bold="True"></asp:Label>
                    </div>
                    <div>
                          <asp:Label ID="lbDetail3" CssClass="resourceLabel" runat="server" Text="Department:&nbsp;"></asp:Label>
                          <asp:Label ID="lbDetail3Val" runat="server" Text="" CssClass="resouceValue"></asp:Label>
                    </div>
                    <div>
                         <asp:Label ID="lbDetail2" CssClass="resourceLabel" runat="server" Text="Location:&nbsp;"></asp:Label>
                         <asp:Label ID="lbDetail2Val" runat="server" Text="" CssClass="resouceValue"></asp:Label>
                    </div>
                </div>
                    <div class="col-md-2" style="padding-left:5px;padding-right:5px;">
                        <div style="float:right;">
                            
                        </div>
                        <div style="float:right; padding-right:10px;">
                                    
                                <span class="fleft" style="padding-left: 5px; display:none;">
                                    <img src="/content/images/analytics.png" visible="false" id="resourceChart" runat="server" style="border: none; width: 20px;" alt="" title="Chart" />
                                </span>                           
                        </div>
                        
                    </div>
        </div>
        <div class="row" >
            <div class="col-md-6 col-xs-12 col-sm-12">
                <a id="aAddItem" runat="server" class="btn btn-sm db-quickTicket svcDashboard_addTicketBtn btnAllocation">
                    <image height="28" src="/Content/images/plus-blue-new.png"></image>
                    <asp:Label ID="LblAddItem" runat="server" Text="New Allocation "></asp:Label>
                </a>
             <dx:ASPxButton ID="btnAddMultiAllocation" ClientInstanceName="btnAddMultiAllocation" RenderMode="Link" runat="server" CssClass="btn btn-sm db-quickTicket svcDashboard_addTicketBtn btnAllocation" AutoPostBack="false" Image-Url="/Content/images/plus-blue-new.png"
                    Text="Project Allocation" Font-Size="12px" BackColor="#4fa1d6" style="padding:6px 12px 6px 8px !important;" Image-Height="22px">
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnAddNonProjectAllocation" ClientInstanceName="btnAddNonProjectAllocation" RenderMode="Link" runat="server"
                     CssClass="btn btn-sm db-quickTicket svcDashboard_addTicketBtn btnAllocation" AutoPostBack="false" Image-Url="/Content/images/plus-blue-new.png"
                    Text="Non Project Allocation" Font-Size="12px" BackColor="#4fa1d6" style="padding:6px 12px 6px 8px !important;" Image-Height="22px">
                </dx:ASPxButton>
            </div>
            
        </div> 
        
        <div class="row toggleTimeOff" <%=!this.ShowTimeOffOnly ? "style=display:none" : "" %>>
            <img class="showTimeOffViewIcon" src="/content/Images/gridBlackNew.png" onclick="toggleTimeOffView()"/>
        </div>
        <div class="row">
            
        </div>
        <div class="allocationblockinner_sub row" style="clear:both">
            <div class="col-md-12 col-sm-12 col-xs-12 noSidePadding" style="width: 100%; border-collapse: collapse;">
                <div class="d-flex justify-content-between pt-2">
                                <div>
                                <span class="allocationPannel-wrap">
                                    
                                    &nbsp;&nbsp;
                                    <asp:CheckBox ID="chkIncludeClosed" runat="server" Text="Include Closed Projects" OnCheckedChanged="chkIncludeClosed_CheckedChanged" AutoPostBack="true" CssClass="checkboxbutton RMM-checkWrap" style="padding-top:11px;"/>
                                </span>
                                <asp:Label ID="lbAllocationError" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                                <div class="selectedYear" style="float:right;padding-top: 12px;">
                                    <span style="padding-right: 5px;">
                                        <asp:ImageButton ImageUrl="/content/images/RMONE/left-arrow.png" Width="7" style="margin-top:-5px" ID="previousYear" ToolTip="Prevoius Year" runat="server" 
                                            OnClick="previousYear_Click" OnClientClick="PreviousYear();ShowLoader();" CssClass="resource-img" />
                                    </span>
                                    <asp:Label ID="lblSelectedYear" runat="server" Style="font-size:16px;font-weight:500;"></asp:Label>
                                    <span style="padding-left: 5px;margin-right:5px;">
                                        <asp:ImageButton ImageUrl="/content/images/RMONE/left-arrow.png" Width="7" style="transform:rotate(180deg);margin-top:-5px" ID="nextYear" ToolTip="Next Year" runat="server" 
                                            OnClick="nextYear_Click" OnClientClick="NextYear();ShowLoader();" CssClass="resource-img" />
                                    </span>
                                </div>
                                <div class="allocationrow" style="padding-bottom:2px;">
                                    <div style="text-align: end;">
                                        <asp:ImageButton ToolTip="Expand All" ImageUrl="/content/images/zoomin-new.png" ID="btExpandAllAllocation" runat="server"
                                            OnClientClick="return btExpandAllAllocation_click()" Visible="false" CssClass="imgHeight" />
                                        <asp:ImageButton ToolTip="Collapse All" ImageUrl="/content/images/zoomout-new.png" ID="btCollapseAllAllocation" runat="server"
                                            OnClientClick="return btCollapseAllAllocation_click()" Visible="false" CssClass="imgHeight" />

                                        <dx:ASPxButton ID="btnExpandAll" runat="server" AutoPostBack="False" Visible="False" RenderMode="Link" Image-Height="30" style="margin-left:15px;">
                                            <Image Url="/content/images/zoomin-new.png" ></Image>
                                            <ClientSideEvents Click="function(s, e) { grid.ExpandAll();}" />
                                        </dx:ASPxButton>

                                        <dx:ASPxButton ID="btnCollapseAll" runat="server" AutoPostBack="False" Visible="False" RenderMode="Link" Image-Height="30" style="margin-left:15px;">
                                            <Image Url="/content/images/zoomout-new.png"  ></Image>
                                            <ClientSideEvents Click="function(s, e) {grid.CollapseAll();}" />
                                        </dx:ASPxButton>
                                        <span>
                                            <img src="/content/Images/out_of_office2.png"  class="imgHeight timeoffClass" title="Out-Of-Office Team Calendar" alt="" onclick="showTimeOff()" />
                                        </span>
                                        <span >
                                            <img src="/Content/Images/time_sheet_icon.png" class="imgHeight btnTimesheet" title="Timesheet" alt="" onclick="OpenTimesheet()" />
                                        </span>
                
                                        <dx:ASPxButton ID="btnAllocationtimeline" runat="server" Image-Height="30" style="margin-left:15px;" AutoPostBack="false" ClientVisible="false"
                                            Image-Width="30" Image-Url="/Content/Images/ganttBlackNew.png" RenderMode="Link" ToolTip="Show Gantt View" >
                                            <ClientSideEvents Click="ChangeToGanttView" />
                                        </dx:ASPxButton>
                
                                        <dx:ASPxButton ID="btnOpenGantt" runat="server" Image-Url="/content/Images/ganttBlackNew.png" BackColor="White" RenderMode="Link"
                                             AutoPostBack="false" Image-Height="30" style="margin-left:15px;" ClientInstanceName="btnOpenGantt" ToolTip="Open Gantt Window">
                                            <ClientSideEvents Click="OpenGanttScreen" />
                                        </dx:ASPxButton>
                                        <span>
                                            <img style="display:none" src="/Content/Images/ganttBlackNew.png" title="Timeline View" class="imgHeight" alt="" onclick="OpenAllocationGantt()" />
                                        </span>
                                        <dx:ASPxButton runat="server" ID="btnChangeView" Image-Url="/content/Images/gridBlackNew.png" ClientVisible="false"
                                             BackColor="White" RenderMode="Link" AutoPostBack="false" Image-Height="30" style="margin-left:15px;"
                                            ClientInstanceName="btnChangeView">
                                            <ClientSideEvents Click="ChangeToGridView" />
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnCardView" runat="server" ClientInstanceName="btnCardView" Image-Url="/content/Images/cardViewBlack-new.png"
                                             BackColor="White" RenderMode="Link" AutoPostBack="false" Image-Height="30" style="margin-left:15px;">
                                            <ClientSideEvents Click="ChangeToCardView" />
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                 </div>
                    
                <div class="row pt-1">
                        
                    <asp:Button ID="btShowAllocation" runat="server" CssClass="hide" Text="Show Allocation" OnClick="btShowAllocation_Click"></asp:Button>
                    <div id="ResourceListTD" class="col-md-2 col-sm-3 noSidePadding ResourceListTD" runat="server" style="display:none; vertical-align: top;">                        
                        <asp:Panel ID="searchViewPanel" runat="server" style="padding-right:20px; width:100%; margin-right:5px; margin-top:-10px;">
                            
                            <div class="searchview-m-m2">
                                <asp:Label ID="lbResourceStatus" runat="server"></asp:Label>
                            </div>
                            <div class="searchview-m-m1" style="margin-right:5px;">
                                <div class="filteredlist">
                                    <ugit:ASPxGridView ID="gvFilteredList" Width="100%"  ClientInstanceName="gvFilteredList" runat="server" KeyFieldName="ID" 
                                        OnDataBound="gvFilteredList_DataBound"  OnHtmlRowPrepared="gvFilteredList_HtmlRowPrepared" CssClass="customgridview homeGrid" Theme="DevEx">
                                        <Columns>
                                            <dx:GridViewDataColumn>                                                            
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="allCheck" runat="server" OnClick="CheckUser('all', this.id, true);" EnableViewState="true" CssClass="ml-2" /><span style="font-size:16px;padding:5px;padding-left:10px;font-weight:500;">Direct Reports</span>
                                                </HeaderTemplate>                                                            
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn HeaderStyle-CssClass="subordiate-head" CellStyle-CssClass="subordiate-data" Width="10" Caption="Alloc%"  HeaderStyle-Font-Bold="true"></dx:GridViewDataColumn>

                                        </Columns>
                                        <Styles>
                                            <Row CssClass="CRMstatusGrid_row"></Row>
                                            <Header CssClass="CRMstatusGrid_headerRow"></Header>
                                        </Styles>
                                        <SettingsPager PageSize="15"></SettingsPager>
                                    </ugit:ASPxGridView>                                                
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="row col-md-10 col-sm-9" style="display:none;" id="ResourceListGrid" runat="server">
                        <asp:Panel ID="allocationPanel" runat="server" >
                            
                                <ugit:ASPxGridView EnableCallBacks="true" ID="gridAllocation" runat="server" AutoGenerateColumns="False" KeyFieldName="Id" EnableRowsCache="false"
                                    OnDataBinding="gridAllocation_DataBinding" ClientInstanceName="gridAllocation" SettingsLoadingPanel-Mode="Disabled"
                                    Width="100%"  OnCustomJSProperties="gridAllocation_CustomJSProperties"
                                    OnCellEditorInitialize="gridAllocation_CellEditorInitialize"
                                    OnRowDeleting="gridAllocation_RowDeleting" OnCustomGroupDisplayText="gridAllocation_CustomGroupDisplayText"
                                    OnHtmlRowPrepared="gridAllocation_HtmlRowPrepared" OnContextMenuItemClick="gridAllocation_ContextMenuItemClick"
                                    OnFillContextMenuItems="gridAllocation_FillContextMenuItems" OnHtmlDataCellPrepared="gridAllocation_HtmlDataCellPrepared" 
                                    OnSelectionChanged="gridAllocation_SelectionChanged"
                                    SettingsText-EmptyDataRow="No Allocations" CssClass="customgridview homeGrid">
                                    <ClientSideEvents ContextMenu="OnContextMenu" EndCallback="function (s, e){ChangeWorkItemStyle();}" ContextMenuItemClick="gridAllocation_ContextMenuItemClick"  />
                                    <Columns>
                                                  
                                    </Columns>
                                    <Templates >
                                        <GroupRowContent  >
                                            <div id="outerGroup" runat="server" visible="false">
                                                <div class="lineHorizontal__container">
                                                    <div class="groupStyle">
                                                        <a href='javascript:window.parent.parent.UgitOpenPopupDialog'  onclick='javascript:event.cancelBubble=true;OpenUserResume("<%#Eval("ResourceId")%>")'>
                                                            <dx:ASPxLabel ID="lblGroupLabel" runat="server" Font-Size="16px" style="font-weight:500;margin:0px 3px;vertical-align:middle;" Text='<%# Container.GroupText%>'>
                                                            </dx:ASPxLabel>
                                                        </a>
                                                        <a href='javascript:window.parent.parent.UgitOpenPopupDialog'  onclick='javascript:event.cancelBubble=true;OpenMultiAllocationPopup("<%#Eval("ResourceId")%>","<%#Eval("ResourceUser").ToString().Replace("'", "`")%>","<%#Eval("WorkItemType")%>")'>
                                                            <dx:ASPxImage ID="imgOpenAddAllocation" runat="server" CssClass="clickEventLink" ToolTip="New Allocation" Height="18px" Width="18px"
                                                                ImageUrl="/content/images/plus-blue-new.png" OnInit="imgOpenAddAllocation_Init">
                                                            </dx:ASPxImage>
                                                       </a>
                                                    </div>
                                                    <div class="lineHorizontal"></div>
                                                </div>
                                            </div>
                                            <div id="innerGroup" runat="server" visible="false">
                                                 <div class="innerGroup">
                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="16px" style="font-weight:500;vertical-align:middle;margin:0px 3px;" Text='<%# Container.GroupText%>'>
                                                </dx:ASPxLabel>
                                                </div>
                                                <%--<dx:ASPxImage ID="ASPxImage1" runat="server" ToolTip="New Allocation" Height="16px" Width="16px"
                                                    ImageUrl="/content/images/plus-blue.png" OnInit="imgOpenAddAllocation_Init">
                                                </dx:ASPxImage>--%>
                                            </div>
                                        </GroupRowContent>
                                    </Templates>
                                    <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                                        <RowHotTrack BackColor="Gold"></RowHotTrack> 
                                        <GroupRow Font-Bold="true" CssClass="estReport-gridGroupRow"></GroupRow>
                                        <GroupPanel CssClass="reportGrid-groupPannel"></GroupPanel>
                                        <Row HorizontalAlign="Left" CssClass="estReportRA-dataRow"></Row>
                                        <Header Font-Bold="true" CssClass="estReport-gridHeaderRow"></Header>
                                        <SelectedRow BackColor="#DBEAF9"></SelectedRow>
                                        <GroupFooter CssClass="estReport-groupFooterRow"></GroupFooter>
                                        <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
                                        <EmptyDataRow Font-Size="18" Font-Bold="True"></EmptyDataRow>
                                    </Styles>
                                <SettingsBehavior EnableRowHotTrack="true" /> 
                                    <Settings GroupFormat="{1}" />
                                    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                    <SettingsEditing Mode="Inline" />
                                    <SettingsBehavior AllowFocusedRow="true" ProcessFocusedRowChangedOnServer="true" />
                                    <SettingsBehavior AllowSort="false"  ConfirmDelete="true" AllowDragDrop="false" AutoExpandAllGroups="true" ColumnResizeMode="Disabled" />
                                    <SettingsText ConfirmDelete="Are you sure you want to delete this allocation?" />
                                    <SettingsDataSecurity AllowDelete="true" />           
                                    <SettingsPopup HeaderFilter-Height="200"></SettingsPopup>
                                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                                
                                </ugit:ASPxGridView>

                        </asp:Panel>
                 </div>
                </div>
                
                <div class="row" style="clear:both;" >
                    <div class="fullwidth dvAssociateCardView" id="dvAssociateCardView" runat="server" style="display:none;">
                           <asp:Panel ID="associateTileView" runat="server"></asp:Panel>
                     </div>
                </div>
                    <div class="row" id="DivTimeOffCardView">
                          <dx:ASPxPanel ID="pnlTimeOffCardView" runat="server" ClientInstanceName="pnlTimeOffCardView" ClientVisible="true">
                          </dx:ASPxPanel>
                     </div>
                
                <div class="row" id="trAllocationDistribution" runat="server" >
                    <div style="padding-top: 15px;">
                        <div class="fullwidth">
                            <div class="fullwidth" style="padding-bottom: 4px;">
                                <asp:Label ID="lbAllocationDistributionMsg" runat="server" CssClass="resAllocation-msgTitle"></asp:Label>
                                <asp:Label ID="lbDistributionError" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                            <div class="fullwidth">
                                <asp:Repeater ID="rAllocationDistForm" runat="server" >
                                    <HeaderTemplate>
                                        <table ondblclick="editMonthDistribution()" class="editallocationMonthTable" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <th class="dxgvHeader_DevEx cross-headerth">
                                                    <div class="cross-header">
                                                        <p class="month-label">Month</p>
                                                        <br>
                                                        <p class="year-label">Year</p>
                                                    </div>
                                                </th>
                                                <th class="dxgvHeader_DevEx">Jan</th>
                                                <th class="dxgvHeader_DevEx">Feb</th>
                                                <th class="dxgvHeader_DevEx">Mar</th>
                                                <th class="dxgvHeader_DevEx">Apr</th>
                                                <th class="dxgvHeader_DevEx">May</th>
                                                <th class="dxgvHeader_DevEx">Jun</th>
                                                <th class="dxgvHeader_DevEx">Jul</th>
                                                <th class="dxgvHeader_DevEx">Aug</th>
                                                <th class="dxgvHeader_DevEx">Sep</th>
                                                <th class="dxgvHeader_DevEx">Oct</th>
                                                <th class="dxgvHeader_DevEx">Nov</th>
                                                <th class="dxgvHeader_DevEx">Dec</th>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="border-bottom: 1px solid #CACBD3; font-weight: bold;"><%# Eval("Year") %></td>
                                            <td>
                                                <dx:ASPxLabel ID="lb1" runat="server" CssClass="monthedittd" Text='<%# string.Format("{0:0}%", Eval("Jan")) %>'></dx:ASPxLabel>
                                                <dx:ASPxTextBox MaskSettings-Mask="<0..100>%" Visible="false" ValidationSettings-ErrorDisplayMode="None" HorizontalAlign="Center" CssClass="editallocation" ID="txt1" runat="server" Text='<%# Eval("Jan") %>'>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel ID="lb2" CssClass="monthedittd" runat="server" Text='<%# string.Format("{0:0}%", Eval("Feb")) %>'></dx:ASPxLabel>
                                                <dx:ASPxTextBox MaskSettings-Mask="<0..100>%" Visible="false" ValidationSettings-ErrorDisplayMode="None" HorizontalAlign="Center" CssClass="editallocation" ID="txt2" runat="server" Text='<%# Eval("Feb") %>'></dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel ID="lb3" CssClass="monthedittd" runat="server" Text='<%# string.Format("{0:0}%", Eval("Mar")) %>'></dx:ASPxLabel>
                                                <dx:ASPxTextBox MaskSettings-Mask="<0..100>%" Visible="false" ValidationSettings-ErrorDisplayMode="None" HorizontalAlign="Center" CssClass="editallocation" ID="txt3" runat="server" Text='<%# Eval("Mar") %>'></dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel CssClass="monthedittd" ID="lb4" runat="server" Text='<%# string.Format("{0:0}%", Eval("Apr")) %>'></dx:ASPxLabel>
                                                <dx:ASPxTextBox MaskSettings-Mask="<0..100>%" Visible="false" ValidationSettings-ErrorDisplayMode="None" HorizontalAlign="Center" CssClass="editallocation" ID="txt4" runat="server" Text='<%# Eval("Apr") %>'></dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel CssClass="monthedittd" ID="lb5" runat="server" Text='<%# string.Format("{0:0}%", Eval("May")) %>'></dx:ASPxLabel>
                                                <dx:ASPxTextBox MaskSettings-Mask="<0..100>%" Visible="false" ValidationSettings-ErrorDisplayMode="None" HorizontalAlign="Center" CssClass="editallocation" ID="txt5" runat="server" Text='<%# Eval("May") %>'></dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel CssClass="monthedittd" ID="lb6" runat="server" Text='<%# string.Format("{0:0}%", Eval("Jun")) %>'></dx:ASPxLabel>
                                                <dx:ASPxTextBox MaskSettings-Mask="<0..100>%" Visible="false" ValidationSettings-ErrorDisplayMode="None" HorizontalAlign="Center" CssClass="editallocation" ID="txt6" runat="server" Text='<%# Eval("Jun") %>'></dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel CssClass="monthedittd" ID="lb7" runat="server" Text='<%# string.Format("{0:0}%", Eval("Jul")) %>'></dx:ASPxLabel>
                                                <dx:ASPxTextBox MaskSettings-Mask="<0..100>%" Visible="false" ValidationSettings-ErrorDisplayMode="None" HorizontalAlign="Center" CssClass="editallocation" ID="txt7" runat="server" Text='<%# Eval("Jul") %>'></dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel CssClass="monthedittd" ID="lb8" runat="server" Text='<%# string.Format("{0:0}%", Eval("Aug")) %>'></dx:ASPxLabel>
                                                <dx:ASPxTextBox MaskSettings-Mask="<0..100>%" Visible="false" ValidationSettings-ErrorDisplayMode="None" HorizontalAlign="Center" CssClass="editallocation" ID="txt8" runat="server" Text='<%# Eval("Aug") %>'></dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel CssClass="monthedittd" ID="lb9" runat="server" Text='<%# string.Format("{0:0}%", Eval("Sep")) %>'></dx:ASPxLabel>
                                                <dx:ASPxTextBox MaskSettings-Mask="<0..100>%" Visible="false" ValidationSettings-ErrorDisplayMode="None" HorizontalAlign="Center" CssClass="editallocation" ID="txt9" runat="server" Text='<%# Eval("Sep") %>'></dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel CssClass="monthedittd" ID="lb10" runat="server" Text='<%# string.Format("{0:0}%", Eval("Oct")) %>'></dx:ASPxLabel>
                                                <dx:ASPxTextBox MaskSettings-Mask="<0..100>%" Visible="false" ValidationSettings-ErrorDisplayMode="None" HorizontalAlign="Center" CssClass="editallocation" ID="txt10" runat="server" Text='<%# Eval("Oct") %>'></dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel ID="lb11" CssClass="monthedittd" runat="server" Text='<%# string.Format("{0:0}%", Eval("Nov")) %>'></dx:ASPxLabel>
                                                <dx:ASPxTextBox MaskSettings-Mask="<0..100>%" Visible="false" ValidationSettings-ErrorDisplayMode="None" HorizontalAlign="Center" CssClass="editallocation" ID="txt11" runat="server" Text='<%# Eval("Nov") %>'></dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel ID="lb12" CssClass="monthedittd" runat="server" Text='<%# string.Format("{0}%", Eval("Dec")) %>'></dx:ASPxLabel>
                                                <dx:ASPxTextBox MaskSettings-Mask="<0..100>%" Visible="false" ValidationSettings-ErrorDisplayMode="None" HorizontalAlign="Center" CssClass="editallocation" ID="txt12" runat="server" Text='<%# Eval("Dec") %>'></dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="fullwidth">
                                <div style="float: right; padding-top: 10px;">
                                    <asp:LinkButton ID="btnEditDistribution" CssClass="btnEditDistribution" EnableViewState="true" runat="server" Text="&nbsp;&nbsp;Edit&nbsp;&nbsp;" ToolTip="Edit" OnClick="btnEditDistribution_Click">
                                <span class="resourceUti-editBtn">
                                    <b style='font-weight: normal;''>
                                        Edit</b>
                                </span>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnSaveAllocationDistribution" EnableViewState="true" Visible="false" runat="server" Text="&nbsp;&nbsp;Save&nbsp;&nbsp;" ToolTip="Save" ValidationGroup="Save" OnClick="btnSaveAllocationDistribution_Click">
                                <span class="resourceUti-editBtn">
                                    <b style="font-weight: normal;">
                                        Save</b>
                                </span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" id="trTaskList" runat="server" style="display:none;" >
                    <div colspan="2" style="padding-top: 15px;">
                        <div class="fullwidth">
                            <div class="fullwidth" style="padding-bottom: 4px;">
                                <asp:Label ID="lblProjectTaskAllocationMsg" runat="server" CssClass="resAllocation-msgTitle"></asp:Label>
                            </div>
                            <div class="fullwidth">
                                <asp:Panel ID="TasklistControl" runat="server"></asp:Panel>
                            </div>
                            </div>
                    </div>
                </div>
                <div class="row btnClose"">
                    <div>
                    <dx:ASPxButton ID="btnclose" runat="server" Text="Close" CssClass="secondary-cancelBtn"  OnClick="btnclose_Click"></dx:ASPxButton>
                        </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="row" id="DivAllocationTimelinePanel" style="display: none;">
                <dx:ASPxPanel ID="pnlAllocationTimeline" runat="server" ClientInstanceName="pnlAllocationTimeline">
                </dx:ASPxPanel>

            </div>
        </div>
    </div>
</div>


<dx:ASPxPopupControl ClientInstanceName="pcAllocationGantt" Width="1000px" Height="500px" Enabled="false"
        MinHeight="150px" MinWidth="150px" ID="pcAllocationGantt" Theme="UGITNavyBlueDevEx" EnableTheming="true" 
        HeaderText="Allocation Timeline" EncodeHtml="false" 
        runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <div>
                    <dx:ASPxGantt ID="allocationGantt" runat="server" EnableTheming="true" Height="500px" Theme="MetropolisBlue" Width="100%" EncodeHtml="false" ClientInstanceName="clientGantt" EnableViewState="false">
                        <SettingsTaskList>
                            <Columns>
                                <dx:GanttTextColumn FieldName="WorkItemLink" Caption="Project" Width="360" PropertiesTextEdit-EncodeHtml="false" />
                                <dx:GanttDateTimeColumn FieldName="AllocationStartDate" Caption="Start Date" Width="100" DisplayFormat="MM\/dd\/yyyy" />
                                <dx:GanttDateTimeColumn FieldName="AllocationEndDate" Caption="Due Date" Width="100" DisplayFormat="MM\/dd\/yyyy" />
                            </Columns>
                        </SettingsTaskList>
                        <Mappings>
                            <Task Key="ID" Title="Allocation" Start="AllocationStartDate" End="AllocationEndDate" Progress="PctAllocation" />
                            <Resource Key="ID" Name="ResourceUser" />
                            <ResourceAssignment Key="ID" TaskKey="ID" ResourceKey="ResourceId" />
                        </Mappings>
                        <SettingsGanttView ViewType="Weeks" />
                    </dx:ASPxGantt>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="function(s, e) { SetImageState(false); }" PopUp="function(s, e) { SetImageState(true); }" />
    </dx:ASPxPopupControl>


    