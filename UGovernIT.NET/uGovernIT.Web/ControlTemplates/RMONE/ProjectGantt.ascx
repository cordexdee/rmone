<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectGantt.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMONE.ProjectGantt" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>



<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .resourceAllocationBlue {
    height: 33px;
    width: 42px;
    display: block;
    border: 2px #4FA1D6 solid;
    padding-top: 9px;
    border-radius: 11px;
    font-weight: 800;
    font-size: 11px;
    color: black;
    float: left;
}
.resourceAllocationBlue-c {
    height: 27px;
    width: 39px;
    display: block;
    border: 2px #4FA1D6 solid;
    padding-top: 5px;
    border-radius: 8px;
    font-weight: 600;
    font-size: 11px;
    color: black;
    float: left;
}
.resourceAllocationRed {
    height: 33px;
    width: 42px;
    display: block;
    border: 2px red solid;
    padding-top: 9px;
    border-radius: 11px;
    font-weight: 800;
    font-size: 11px;
    color: black;
    float: left;
}
.resourceAllocationRed-c {
    height: 27px;
    width: 39px;
    display: block;
    border: 2px red solid;
    padding-top: 5px;
    border-radius: 8px;
    font-weight: 600;
    font-size: 11px;
    color: black;
    float: left;
}
.labelChartHeading{
    font-size: 18px;
    font-family: "Segoe UI Light", "Helvetica Neue Light", "Segoe UI", "Helvetica Neue", "Trebuchet MS", Verdana, sans-serif;
    font-weight: 800;
    fill: rgb(35, 35, 35);
    cursor: default;
    margin-top: 4px;
    align-items: center;
}
    .labelNumber {
        font-size: 14px;
        font-weight: 700;
    }

    td, th {
        /*border: 1px solid #dddddd;*/
        text-align: left;
        padding: 4px;
        /*text-align:center;*/
    }

    th {
        font-weight: normal;
    }

    .bg-g {
        background-color: #DFDFDF;
    }
    .bg-white {
        background-color: #FFF;
    }

    .whiteBgRadius {
        /*box-shadow: 0 6px 12px #00000020;*/
        background-color: #fff;
        border-radius: 6px;
    }

    .title-t {
        color: #000;
        font-size: 16px;
        font-weight: 600;
    }
/*.captionCenter{
    text-align: center;
}
*/    .color-dBlue {
        color: #030644;
    }

    .color-lBlue {
        color: #00b9ff;
    }

    .color-Gray {
        color: #a6a6a6;
    }

    .cusTitle {
        font-size: 22px;
        font-weight: 600;
        text-align: center;
        margin-top: 0;
        margin-bottom: 6px;
    }

    .textt {
        color: #000;
        font-size: 12px;
        text-align: center;
        margin-bottom: 6px;
    }

    .bdr {
        border-right: 2px solid #DFDFDF;
    }

    .fws {
        font-size: 16px;
    }

    .state-tooltip .caption {
        font-weight: bold;
    }

    .captionCenter {
        text-align: center !important;
    }

    .dxeEditAreaSys,
    .dxeMemoEditAreaSys, /*Bootstrap correction*/
    input[type="text"].dxeEditAreaSys, /*Bootstrap correction*/
    input[type="password"].dxeEditAreaSys /*Bootstrap correction*/ {
        font: inherit;
        line-height: normal;
        outline: 0;
        color: inherit;
    }

    .excel-button {
        background-image: url('/content/images/excel_icon.png');
    }

    .export-buttons {
        padding-left: 3px;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        border-top: 1px solid #A5A5A5;
        color: #000000;
        padding-bottom: 6px;
        padding-right: 8px;
        padding-top: 3px;
        width: 150px;
        text-align: right;
    }

    .panel-parameter {
        width: 400px;
        border: solid 1px #000000;
    }

    .table-header {
        background-color: #E8EDED;
    }

    a:hover {
        text-decoration: none !important;
    }

    .swal2-title {
        position: relative;
        max-width: 100%;
        margin: 0;
        padding: 0.8em 1em 0;
        color: inherit;
        font-size: 18px !important;
        font-weight: 600;
        text-align: center;
        text-transform: none;
        word-wrap: break-word
    }

    .swal2-styled.swal2-confirm {
        border: 0;
        border-radius: 0.25em;
        background: initial;
        background-color: green !important;
        color: #fff;
        font-size: 1em;
    }

    .swal2-container.swal2-center > .swal2-popup {
        grid-column: 2;
        grid-row: 1;
        align-self: center;
        justify-self: center;
    }

    .swal2-icon {
        position: relative;
        box-sizing: content-box;
        justify-content: center;
        width: 3em;
        height: 3em;
        margin: 2.5em auto 0.6em;
        border: 0.25em solid transparent;
        border-radius: 50%;
        border-color: #000;
        font-family: inherit;
        line-height: 5em;
        cursor: default;
        -webkit-user-select: none;
        -moz-user-select: none;
        -ms-user-select: none;
        user-select: none;
    }


    .summaryTable {
        border-width: 0;
        border-spacing: 0;
        font-weight: bold;
    }

        .summaryTable td {
            padding: 0;
        }



    .gridGroupRow td:last-child {
        padding: 0;
    }

    .SummaryHeaderAdjustment {
        /*margin-top: -19px;
        position: relative;*/
    }

    .radiobutton label {
        margin-left: 5px;
        margin-right: 5px;
    }

    .timeline_gridview {
    }

        .timeline_gridview td {
        }

        .timeline_gridview .timeline-td {
            border-left: none;
            border-right: none !important;
            padding-left: 0px !important;
            padding-right: 0px !important;
        }

    .dvYearNavigation {
        float: right;
        padding-right: 15px;
        padding-top: 5px;
    }


    .rmmLookup-valueBoxEdit table.department tr td.dxic input[type="text"] {
        height: 20px !important;
        background: #FFF;
    }

    .allocationperworkitem {
        text-align: left !important
    }

    .savepaddingleft {
        padding: 10px 12px;
        /*padding-left: 915px;*/
        float: right;
    }

    .newResource_inputField table tr td {
        background: #ecf1f9;
    }

    .nameLabel {
        font-weight: 800;
        float: left;
        color: black;
        font-size: 17px;
    }

    .roleLabel {
        clear: both;
        color: grey;
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

    .summaryTextContainer {
        padding-left: 0px;
        text-align: center;
    }

    .headerColorBox {
        float: left;
        width: 100%;
        height: 38px;
        color: white;
        padding-top: 5px;
    }

    .glabel1 {
        display: flex;
        color: grey;
        font-size: 14px;
        float: right;
    }

    .glabel2 {
        clear: both;
        font-size: 14px;
        float: right;
        font-weight: 600;
    }
    .glabelleftalign {
        display: flex;
        color: grey;
        font-size: 14px;
        float: left;
    }
    .ganttdataRow td {
        border-bottom: 0px solid #f6f7fb !important;
    }
    /*a{
        font-weight:800;
    }*/
    .preconbgcolor-constbgcolor {
        color: #FFFFFF !important;
        background: linear-gradient(to right, rgba(82, 190, 217, 1), rgba(0, 92, 155, 1));
    }

    .constbgcolor-closeoutbgcolor {
        color: #FFFFFF !important;
        background: linear-gradient(to right, rgba(0, 92, 155, 1), rgba(53, 27, 130, 1));
    }

    .preconbgcolor {
        background-color: #52BED9 !important;
        color: #ffffff !important;
    }

    .constbgcolor {
        background-color: #005C9B !important;
        color: #ffffff !important;
    }

    .closeoutbgcolor {
        background-color: #351B82 !important;
        color: #ffffff !important;
    }

    .ptobgcolor {
        background-color: #909090 !important;
    }

    .nobgcolor {
        background-color: #D6DAD9 !important;
        /*color:#ffffff !important;*/
    }

    .softallocationbgcolor {
        background-color: #ecf1f9 !important;
        border: 1.5px dashed !important;
        border-left: hidden !important;
        border-right: hidden !important;
        color: #000000 !important
    }

    .RoundLeftSideCorner.softallocationbgcolor {
        background-color: #ecf1f9 !important;
        border: 1.5px dashed !important;
        opacity: 0.8;
        border-right: hidden !important;
        margin-left: auto;
    }

    .RoundRightSideCorner.softallocationbgcolor {
        background-color: #ecf1f9 !important;
        border: 1.5px dashed !important;
        opacity: 0.8;
        border-left: hidden !important;
    }

    .cmicnoLabel {
        font-weight: 600;
        color: black;
    }

    .dxgvGroupRow_UGITNavyBlueDevEx td.dxgv {
        padding: 0px 0px;
    }

    .RoundLeftSideCorner {
        border-top-left-radius: 9px;
        border-bottom-left-radius: 9px;
        margin-left: auto;
    }

    .RoundRightSideCorner {
        border-top-right-radius: 9px;
        border-bottom-right-radius: 9px;
    }

    .dx-loadpanel-content {
        user-select: none;
        border: none;
        background: transparent;
        border-radius: 0px;
        -webkit-box-shadow: none;
        box-shadow: none;
    }

    .dxlpLoadingPanel_UGITNavyBlueDevEx, .dxlpLoadingPanelWithContent_UGITNavyBlueDevEx {
        background-color: transparent;
        border: none;
    }

    .ptoAlignmentClass {
        vertical-align: initial;
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

    .collapseClassGantt {
        transform: rotate(270deg);
        background-image: none !important;
        margin-left: 7px;
    }

    .expandClassGantt {
        transform: rotate(90deg);
        background-image: none !important;
        margin-left: 7px;
    }

    .dxgvHeader_UGITNavyBlueDevEx {
        cursor: context-menu;
    }

    .hand-cursor {
        cursor: pointer;
    }

    .date-info-tooltip {
        padding: 3px;
    }

    .flex-container {
        display: flex;
        /*justify-content: space-between; Commented to apply margin-left correctly for BTS-24-001497.*/
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

    .dx-tooltip-wrapper .dx-overlay-content {
        background-color: lightgrey;
    }

     .dxGridView_gvHeaderSortUp_UGITNavyBlueDevEx {  
                display: none !important;  
        }  
         .dxGridView_gvHeaderSortDown_UGITNavyBlueDevEx {  
                display: none !important;  
        }

    .glabel2red {
        clear: both;
        font-size: 14px;
        float: right;
        font-weight: 600;
    }

    .cmicnoLabelred {
        font-weight: 600;
        color: red;
    }

    .dx-tooltip-wrapper .dx-overlay-content {
        background-color: #F5F5F5;
    }
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var baseUrl = ugitConfig.apiBaseUrl;
    const ProjectStatus = ["1Open Opportunities", "1Tracked Work", "1Construction Projects", "1Closed"];
    var projectColorCodes = [
        '#D3D3D3',
        '#52BED9',
        '#005C9B',
        '#351B82'
    ];

    var request = {
        StartDate: '',
        EndDate: '',
        Sector: '',
        Division: 0,
        Studio: 0
    }
    var activeview = $.cookie('activeview');
    if (activeview == "ResourceUtil") {
        $("#divResourceUtil").show();
        $("#divGanttView").hide();
    }
    else {
        $("#divResourceUtil").hide();
        $("#divGanttView").show();
    }

    $(document).ready(function () {
        var activeview = $.cookie('activeview');
        if (activeview == "ResourceUtil") {
            $("#divResourceUtil").show();
            $("#divGanttView").hide();
        }
        else {
            $("#divResourceUtil").hide();
            $("#divGanttView").show();
        }

        var tooltip = $("#tooltip").dxTooltip({
            target: $(".homeGrid"),
            contentTemplate: function (contentElement) {
                contentElement.append(

                )
            }
        });
        $("#btnSwitchtoMain").dxButton({
            icon: '/Content/Images/sectorIcon.png',
            height: 35,
            width: 35,
            hint:'Show All',
            onClick: function (e) {
                let chart = $('#hoursChart').dxChart("instance");
                chart.clearSelection();
                
                hdnDataFilters.Set("PieSlice", "");
                loadingPanel.Show();
                grid.PerformCallback();
                //this.option("visible", false);
                $("#btnSwitchtoMain").css('visibility', 'hidden');
            }
        });
        $.get(baseUrl + "/api/CoreRMM/GetSectorStudioDivisionData?dataRequiredFor=", function (data, status) {
            Sector = data.Table;
            Divisions = data.Table1;
            const division = $('#ddlDivision').dxSelectBox({
                dataSource: Divisions,
                placeholder: '<%=DivisionLabel%>',
                showClearButton: true,
                valueExpr: "ID",
                displayExpr: "Title",
                onValueChanged: function (e) {                    
                    loadingPanel.Show();
                    request.Division = e.value == null ? 0 : e.value;
                    request.Studio = 0;
                    //Whenever we enable Studio Dropdown control then only we have to uncomment below lines
                    <%--var enableStudioDivisionHierarchy = '<%=enableStudioDivisionHierarchy%>';
                    if (enableStudioDivisionHierarchy == 'True') {
                        LoadStudios();
                    }--%>
                    LoadHoursChart();
                    var currentDate = new Date();
                    var currentYear = currentDate.getFullYear();
                    LoadUtilizationForecast(currentYear);
                    hdnDataFilters.Set("Division", request.Division);
                    hdnDataFilters.Set("PieSlice", "");
                    grid.PerformCallback();
                    gvResourceAvailablity.PerformCallback();
                    //LoadDepartments();
                    
                    //let deptCtrlId = 'ctl00_ctl00_MainContent_ContentPlaceHolderContainer_DirectorViewDockPanel_pnlResourceUtil_ctl03_ddlDepartment_BoxEditCallBack_customdropdownedit_DDD_DDTC_department';
                    let deptCtrlId = '<%=deptCtrlId%>';
                    $('#' + deptCtrlId + '_cmbDivision').val(request.Division);
                    var element = $('#' + deptCtrlId + '_cmbDivision');
                    let lastDivision = $('#' + deptCtrlId + '_cmbDivision').val();
                    //element.trigger("selectedIndexChanged");
                    OnDivisionChanged(deptCtrlId, '', request.Division);
                }
            });

            const sector = $('#ddlSector').dxSelectBox({
                dataSource: Sector,
                placeholder: 'Sector',
                showClearButton: true,
                valueExpr: "Title",
                displayExpr: "Title",
                onValueChanged: function (e) { //debugger                    
                    request.Sector = e.value == null ? '' : e.value;
                    hdnDataFilters.Set("Sector", request.Sector);
                    LoadHoursChart();
                    grid.PerformCallback();
                }
            });
        });

        if ('<%=SelectedYear%>' == '') {
            var currentDate = new Date();
            var currentYear = currentDate.getFullYear();
            LoadUtilizationForecast(currentYear);
        }
        else {
            LoadUtilizationForecast('<%=SelectedYear%>');
        }
        LoadHoursChart();

    });


    function LoadDepartments() {
        $.get(baseUrl + "/api/rmmapi/GetDepartmentsForDivision?DivisionId=" + request.Division, function (data, status) {
            const studio = $('#ddlDepartments').dxSelectBox({
                dataSource: data,
                placeholder: 'Department',
                valueExpr: "ID",
                displayExpr: "Title",
                showClearButton: true,
                onValueChanged: function (e) {
                }
            });
        });
    }


    function LoadHoursChart() {
        let btnSwitchtoMain = $("#btnSwitchtoMain").dxButton("instance");
        //btnSwitchtoMain.option("visible", false);
        $("#btnSwitchtoMain").css('visibility', 'hidden');
        $.get(baseUrl + "/api/RMOne/GetProjectChartData?StartDate=" + request.StartDate + "&EndDate=" + request.EndDate + "&Studio=" + request.Studio
            + "&Division=" + request.Division + "&Sector=" + request.Sector,
            function (dataSource) {
                //debugger
                if (dataSource[0].OPM == null && dataSource[0].CPR == null && dataSource[0].TP == null) {
                    console.log(dataSource);
                    $("#chartEmpty").css("display", "block");
                    $("#hoursChart").css("display", "none");
                } else {
                    $("#chartEmpty").css("display", "none");
                    $("#hoursChart").css("display", "block");
                }
                $('#hoursChart').dxChart({
                    dataSource,
                    size: {
                        height: 242,
                        width: '100%',
                    },
                    commonSeriesSettings: {
                        argumentField: 'category',
                        type: 'stackedBar',
                        ignoreEmptyPoints: 'true',
                          label: {
                            visible: true,
                              alignment: "center",
                                font: {
                                    size: 14,
                                    weight: 600,
                                    color: '#000000',
                                },
                          },
                        tick: { visible: false },
                    },
                    series: [
                        { valueField: 'OPM', name: 'Opportunities', color: '#DBDDDD' },
                        { valueField: 'TP', name: 'Tracked Projects', color: '#A9C23F' },
                        { valueField: 'CPR', name: 'Construction', color: '#6BA539' },
                    ],
                    legend: {
                      verticalAlignment: 'bottom',
                      horizontalAlignment: 'center',
                      itemTextPosition: 'top',
                    },
                    valueAxis: {
                        position: 'right',
                        visible: false, 
                        label: {
                            visible: false 
                        },
                        grid: {
                            visible: false 
                        },
                        minorTick: { visible: false },
                        majorTick: { visible: false },
                        tick: { visible: false },

                    },
                    argumentAxis: {
                        visible: false, 
                        label: {
                            visible: false 
                        },
                        grid: {
                            visible: false 
                        },
                        minorTick: { visible: false },
                        majorTick: { visible: false },
                        tick: { visible: false },
                        label: { visible: false },
                    },
                    //title: {
                    //    text: 'Project Flow',
                    //    font: {
                    //        size: 18,
                    //        weight: 800
                    //    }
                    //},
                    rotated: true,
                    onPointClick(e) {
                        const point = e.target;
                        //debugger
                        e.target.select();
                        hdnDataFilters.Set("PieSlice", point.series.name);
                        $("#btnSwitchtoMain").css('visibility', 'visible');
                        loadingPanel.Show();
                        grid.PerformCallback();
                        clearFilterFromList();
                    },
                    tooltip: {
                      enabled: true,
                      location: 'edge',
                      customizeTooltip(arg) {
                        return {
                          text: `${arg.seriesName}: ${arg.valueText}`,
                        };
                      },
                    },
              });

            });
    }
    function createElement(type, attributes) {
        const element = document.createElementNS('http://www.w3.org/2000/svg', type);
        Object.keys(attributes).forEach((attributeName) => {
            element.setAttribute(attributeName, attributes[attributeName]);
        });
        return element;
    }

    function createText(attributes, contents) {
        const text = createElement('text', attributes);

        const tspan1 = createElement('tspan', { x: attributes.x, dy: 0 });
        tspan1.textContent = contents[0];

        const tspan2 = createElement('tspan', { x: attributes.x, dy: 20, 'font-weight': 600 });
        tspan2.textContent = contents[1];

        text.appendChild(tspan1);
        text.appendChild(tspan2);

        return text;
    }

    function ClickOnPrevious_PG() {
        var currentDate = new Date();
        var currentYear = currentDate.getFullYear();
        var year = $('#<%= hndYear.ClientID%>').val();
            var pattern = /^\d{4}$/;
            if (pattern.test(year)) {
                year = parseInt(year) - 1;
                $('#<%= hndYear.ClientID%>').val(year);
        } else {
            year = currentYear;
            $('#<%= hndYear.ClientID%>').val(year);
        }
        loadingPanel.Show();
        grid.PerformCallback();
        LoadUtilizationForecast(year);
    }

    function ClickOnNext_PG() {
        var currentDate = new Date();
        var currentYear = currentDate.getFullYear();
        var year = $('#<%= hndYear.ClientID%>').val();
        var pattern = /^\d{4}$/;
        if (pattern.test(year)) {
            year = parseInt(year) + 1;
            $('#<%= hndYear.ClientID%>').val(year);
        } else {
            year = currentYear;
            $('#<%= hndYear.ClientID%>').val(year);
        }
        loadingPanel.Show();
        grid.PerformCallback();
        LoadUtilizationForecast(year);
    }
    function ClickOnDrillDown_PG(obj, fieldname, caption) {
        $.cookie("selectedDate", fieldname);
        GZoomIn_PG();
    }

    function ClickOnDrillUP_PG(obj, fieldname, caption) {
        $('#<%= hndYear.ClientID%>').val(fieldname);
        GZoomOut_PG();
    }


    function GZoomIn_PG() {
        if ($('#<%= hdndisplayMode.ClientID%>').val() == "Monthly") {
            $('#<%= hdndisplayMode.ClientID%>').val("Weekly");
            $('#<%= hdnZoomClicked.ClientID%>').val("YES");
            loadingPanel.Show();
            grid.PerformCallback();
        }
    }
    function GZoomOut_PG() {
        if ($('#<%= hdndisplayMode.ClientID%>').val() == "Weekly") {
            $('#<%= hdndisplayMode.ClientID%>').val("Monthly");
            $('#<%= hdnZoomClicked.ClientID%>').val("YES");
            loadingPanel.Show();
            grid.PerformCallback();
        }
    }
    function LoadUtilizationForecast(year) {
        $.get(baseUrl + "/api/RMONE/GetUtilizationForecastData?Year=" + year + "&Division=" + request.Division
            , function (dataUtilizationForecast) {
                $('#dataGrid').dxChart({
                    dataSource: dataUtilizationForecast,
                    size: {
                        height: 325,
                    },
                    commonAxisSettings: {
                        grid: {
                            visible: false
                        },
                        tick: false,
                        minorTick: {
                            visible: false
                        },
                        visible: false
                    },
                    commonSeriesSettings: {
                        argumentField: 'Month',
                        valueField: 'Pct' + '%'
                    },
                    argumentAxis: {
                        valueMarginsEnabled: true,
                        discreteAxisDivisionMode: 'crossLabels',
                        minorTick: {
                            visible: false
                        },
                    },
                    valueAxis: {
                        //visualRange: {
                        //    startValue: 0,
                        //    endValue: 30,
                        //},
                        grid: {
                            visible: false
                        },
                        minorTick: {
                            visible: false
                        },
                        name: 'percentage',
                        position: 'left',
                        showZero: true,
                        label: {
                            customizeText(info) {
                                return `${info.valueText}%`;
                            },
                        },
                    },
                    series: [
                        { valueField: 'Pct', name: 'Pct' },
                    ],
                    legend: {
                        verticalAlignment: 'bottom',
                        horizontalAlignment: 'center',
                        itemTextPosition: 'bottom',
                        visible: false,
                    },
                    title: {
                        text: 'Utilization Forecast',
                        font: {
                            size: 18,
                            weight: 800
                        }
                    },
                    export: {
                        enabled: false,
                    },
                    tooltip: {
                        enabled: true,
                        contentTemplate(info, container) {
                            //return `${pointInfo.argumentText.replace(/ /g, "\r\n")}<br><div style='width:100%'>${pointInfo.value}</div>`;
                            const contentItems = [`<div class=''>`,
                                /*"<h4 class='state'></h4>",*/
                                "<div>",
                                "<div style='float:left;' class='projects caption'><span>Projects</span>: </div>",
                                "<div class='space'></div>",
                                "<div style='float:right;' class='resources caption'><span>Resources</span>: </div>",
                                '</div></div></div>'];

                            const content = $(contentItems.join(''));
                            /*content.find('.state').text(info.argument);*/
                            content.find('.projects').append(document.createTextNode(info.point.data.Projects));
                            content.find('.space').append(document.createTextNode(" "));
                            content.find('.resources').append(document.createTextNode(info.point.data.Resources));
                            content.appendTo(container);
                        },
                    },
                });
            });


    }

    function Grid_Init(s, e) {
        //AdjustSummaryTable();

    }

    function Grid_EndCallback(s, e) {
        //AdjustSummaryTable();
        loadingPanel.Hide();
    }

    function Grid_ColumnResized(s, e) {
        //AdjustSummaryTable();
    }

    function showTooltip(element, startDate, endDate, name, estalloc, roleName, backgroundColor) { 
        var tooltip = $("#tooltip").dxTooltip("instance");
        tooltip.option({
            target: "#" + $(element).attr("id"),
            contentTemplate: function (contentElement) {
                //var snewDate = new Date(startDate);
                //var sdate = snewDate.getDate();
                //var smonth = snewDate.getMonth() + 1;
                //var syear = snewDate.getFullYear();
                ////syear = syear.slice(0,2);
                //startDate = smonth + '/' + sdate + '/' + syear;

                //var enewDate = new Date(endDate);
                //var edate = enewDate.getDate();
                //var emonth = enewDate.getMonth() + 1;
                //var eyear = enewDate.getFullYear();
                ////eyear = eyear.substring(eyear.length - 2);
                //endDate = emonth + '/' + edate + '/' + eyear;

                var phase = backgroundColor.slice(0, -7);
                phase = phase.substring(0, 1).toUpperCase() + phase.substring(1);
                var colour;
                if (phase == 'Const') {
                    phase = 'Construction: ';
                    colour = '#005C9B';
                }
                else if (phase == 'Precon') {
                    phase = 'Precon: ';
                    colour = '#52BED9';
                }
                else if (phase == 'Closeout') {
                    phase = 'Closeout: ';
                    colour = '#351B82';
                }
                else if (phase == 'Constbgcolor-closeout') {
                    phase = "Closeout: ";
                    colour = 'linear - gradient(to right, rgba(0, 92, 155, 1), rgba(53, 27, 130, 1))';
                }
                else if (phase == 'Preconbgcolor-const') {
                    phase = 'Construction: ';
                    colour = 'linear-gradient(to right, rgba(82, 190, 217, 1), rgba(0, 92, 155, 1))';
                }
                else if (phase == 'No') {
                    phase = '';
                }
                contentElement.html("<div><b>" + "<span style='color:" + colour + "'>" + phase + "</span>" + "</b>" + startDate + " - " +
                    endDate );
            }
        });
        tooltip.show();

    }
    function hideTooltip(element) {

        var tooltip = $("#tooltip").dxTooltip("instance");
        tooltip.hide();
    }
    function ChangeToGanttView(s, e) {
        $.cookie('activeview', "Gantt");
        $("#divResourceUtil").hide();
        $("#divGanttView").show();
        $('#<%= divGanttgvPreview.ClientID%>').removeClass("d-none");
    }
    function ChangeToResourceUtilView(s, e) {        
        $.cookie('activeview', "ResourceUtil");
        $("#divResourceUtil").show();
        $("#divGanttView").hide(); 
        $("#<%=pnlResourceUtil.ClientID%>").removeClass("d-none");
    }

    function chkIncludeClosed_ValueChanged(s, e) {
        grid.PerformCallback();
    }

    function Grid_BeginCallback(s, e) {
        loadingPanel.Show();
    }

    function clearFilterFromList(s, e) {
        grid.ClearFilter();
    }
</script>

<asp:HiddenField ID="hdnfilterTypeLoader" runat="server" />
<asp:HiddenField ID="hdndisplayMode" runat="server" Value="Monthly" />
<asp:HiddenField ID="hndYear" runat="server" />
<asp:HiddenField ID="hdnZoomClicked" runat="server" />
<dx:ASPxHiddenField ID="hdnDataFilters" runat="server" ClientInstanceName="hdnDataFilters"></dx:ASPxHiddenField>
<dx:ASPxLoadingPanel ID="loadingPanel" runat="server" Modal="true" Text="Loading..." ClientInstanceName="loadingPanel" 
    Image-Url="~/Content/Images/ajax-loader.gif"></dx:ASPxLoadingPanel>

<div class="estimatedHoursContainer py-1 bg-g">
    <div class="row pb-2">
        <%--<div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">--%>
        <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
            <div class="row d-flex justify-content-betwen pb-1">
                <div class="ml-1" id="ddlSector" style="width: 100%;"></div>
                <div class="ml-1" id="ddlDivision" style="width: 100%;"></div>
                <%--<div class="ml-1" id="ddlStudio" style="width: 100%;"></div>--%>
                </div>
            <div class="row whiteBgRadius" style="display: flex; min-height: 35px;">
                <div id="btnSwitchtoMain" ></div>
                <div class="justify-centre" style="margin-left: 35%"><h1 class="labelChartHeading">Project Flow</h1></div>

            </div>
            <div class="row text-center">
               <div class="whiteBgRadius d-flex justify-content-center pb-1 pt-1">
                <span id="chartEmpty" style="top:290px;color: #808080bf; font-size: 18px; width:320px; text-align:center; display: flex; flex-direction: column; align-items: center;">No data</span>
                    <div id="hoursChart"></div>
                </div>
            </div>
            
        </div>
        <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12 pl-0">
                <div class="whiteBgRadius d-flex justify-content-center">
                    <div id="dataGrid" style="width: 97%;"></div>
                </div>
            </div>
        </div>
       
    <div class="row ">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-4 ">
            <div class="whiteBgRadius pt-1 pb-1 d-flex">
                <dx:ASPxButton ID="btnShowGantt" runat="server" Image-Height="23" style="margin-left:15px;" AutoPostBack="false"
                    Image-Width="30" Image-Url="/Content/Images/projectBlack.png" RenderMode="Link" ToolTip="Show Gantt View" >
                    <ClientSideEvents Click="ChangeToGanttView" />
                </dx:ASPxButton>

                <dx:ASPxButton ID="btnShowResourceUtil" runat="server" AutoPostBack="False" RenderMode="Link" Image-Height="23" 
                    style="margin-left:15px;" Image-Url="/Content/Images/resourceMngBlack.png" ToolTip="Show Resource Utilization">
                    <ClientSideEvents Click="ChangeToResourceUtilView" />
                </dx:ASPxButton>

                <div class="pl-3">
                    <dx:ASPxCheckBox ID="chkIncludeClosed" runat="server" AutoPostBack="true" Text="Include Closed" Font-Bold="false" 
                        Font-Size="Medium" RootStyle-Paddings-PaddingLeft="5px"
                         OnValueChanged="chkIncludeClosed_ValueChanged">

                    </dx:ASPxCheckBox>
                </div>
                
            <div id ="ddlDepartments"></div>
                </div>
        </div>
    </div>

        <div class="row">
            <div class="row " id="divResourceUtil" style="display:none">
                    <dx:ASPxPanel ID="pnlResourceUtil" runat="server" ClientInstanceName="pnlResourceUtil">
                        
                    </dx:ASPxPanel>

                </div>
        </div>

    <div class="row pt-2">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div id="divGanttView" class="whiteBgRadius">
                <div id="divGanttgvPreview" runat="server">
                    <dx:ASPxGridView ID="gvPreview" runat="server" AutoGenerateColumns="false" CssClass="gvPreview homeGrid" KeyFieldName="TicketId"
                        Width="100%" OnDataBinding="gvPreview_DataBinding" ClientInstanceName="grid" EnableCallBacks="true" 
                        ClientVisible="true"  OnCustomColumnSort="gvPreview_CustomColumnSort"
                        OnHtmlDataCellPrepared="gvPreview_HtmlDataCellPrepared"  SettingsBehavior-SortMode="Custom"
                        OnHtmlRowCreated="gvPreview_HtmlRowCreated" 
                        OnCustomCallback="gvPreview_CustomCallback"
                        OnHtmlRowPrepared="gvPreview_HtmlRowPrepared">
                        <Columns>
                        </Columns>
                        <Templates>

                            <StatusBar>
                                <div id="editControlBtnContainer" style="display: none;">
                                    <asp:HyperLink ID="updateTask" runat="server" Text="Save Task Changes" CssClass="fleft updateTask savepaddingleft" OnClick="grid_BatchUpdate();">
                                                <span class="alloTimeSave-gridBtn">
                                                    <b style="font-weight: normal;">Save Changes</b>
                                                </span>
                                    </asp:HyperLink>
                                    <asp:HyperLink ID="cancelTask" runat="server" Style="padding: 10px 5px; float: right;" Text="Cancel Changes" CssClass="fleft" OnClick="grid_CancelBatchUpdate();">
                                                <span class="alloTimeCancel-gridBtn">
                                                    <b style="font-weight: 600;">Cancel Changes</b>
                                                </span>
                                    </asp:HyperLink>
                                </div>
                            </StatusBar>
                        </Templates>

                        <ClientSideEvents
                            Init="Grid_Init" EndCallback="Grid_EndCallback" BeginCallback="Grid_BeginCallback" ColumnResized="Grid_ColumnResized" />
                        <Styles>
                            <Row CssClass="ganttdataRow"></Row>
                            <Header CssClass="gridHeader RMM-resourceUti-gridHeaderRow" />
                            <GroupPanel CssClass="reportGrid-groupPannel"></GroupPanel>
                            <GroupFooter CssClass="estReport-groupFooterRow"></GroupFooter>
                            <Footer Font-Bold="true" HorizontalAlign="Center" Border-BorderColor="#D9DAE0" Border-BorderStyle="Solid" Border-BorderWidth="1px"
                                BorderRight-BorderWidth=".5px" CssClass="resourceUti-gridFooterRow">
                            </Footer>
                            <Table CssClass="timeline_gridview"></Table>
                            <Cell Wrap="True"></Cell>
                        </Styles>
                        <SettingsDataSecurity AllowDelete="false" AllowEdit="false" />
                        <SettingsBehavior AutoExpandAllGroups="true" />
                        <Settings VerticalScrollableHeight="460" VerticalScrollBarMode="Auto" ShowFilterRow="false" ShowHeaderFilterButton="true" />
                        <SettingsLoadingPanel Mode="Disabled" />
                        <SettingsPager PageSizeItemSettings-ShowAllItem="true">
                        </SettingsPager>
                    </dx:ASPxGridView>


                </div>
            </div>
            <div id="tooltip" class="tooltip">
            </div>
        </div>

    </div>
</div>
