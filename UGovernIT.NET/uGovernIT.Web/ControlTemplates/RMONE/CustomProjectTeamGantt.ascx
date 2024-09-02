<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomProjectTeamGantt.ascx.cs" Inherits="uGovernIT.Web.CustomProjectTeamGantt" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>


<script type="text/javascript" src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
<link href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-sweetalert/1.0.1/sweetalert.css" rel="stylesheet" />
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.1.1/animate.min.css" />
<style data-v="<%=UGITUtility.AssemblyVersion %>">
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

    .nameLabel{
        font-weight:800;
        float:left;
        color:black;
        font-size:17px;
    }
    .roleLabel{
        clear:both;
        color:grey;
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

    .summaryTextContainer {
        padding-left: 0px;
        text-align: center;
    }

    .headerColorBox{       
        float:left;width:100%;height: 38px; color:white;padding-top: 5px;
    }
    .glabel1{
        display:flex;
        color:grey;
        font-size:14px;
        float:right;
        margin-top:10px;
    }
    .glabel2{
       /* clear:both;*/
        font-size:16px;
        float:right;
        font-weight:600;
    }
    .ganttdataRow td {
        border-bottom: 0px solid #f6f7fb !important;
    }
    /*a{
        font-weight:800;
    }*/

    
    .preconbgcolor{
        background-color:#52BED9 !important;
        color:#ffffff !important;
        padding-top:4px;
    }
    .constbgcolor{
        background-color:#005C9B !important;
        color:#ffffff !important;
        padding-top:4px;
    }
    .closeoutbgcolor{
        background-color:#351B82 !important;
        color:#ffffff !important;
        padding-top:4px;
    }
    .ptobgcolor{
        background-color:#909090 !important;
        padding-top:4px;
    }
    .nobgcolor{
        background-color:#D6DAD9 !important;
        padding-top:4px;
        /*color:#ffffff !important;*/
    }
    .softallocationbgcolor{
        background-color:#ecf1f9 !important;
        border:1.5px dashed !important;
        border-left: hidden !important;
        border-right: hidden !important;
        color:#000000 !important;
        padding-top:4px;padding-top:4px;
    }

    .RoundLeftSideCorner.softallocationbgcolor{
        background-color:#ecf1f9 !important;
        border:1.5px dashed !important;
        opacity:0.8;
        border-right:hidden !important;
        padding-top:4px;
    }
    .RoundRightSideCorner.softallocationbgcolor{
        background-color:#ecf1f9 !important;
        border:1.5px dashed !important;
        opacity:0.8;
        border-left:hidden !important;
        padding-top:4px;
    }
    .cmicnoLabel{
        font-weight:600;
        color:black;
    }
    .dxgvGroupRow_UGITNavyBlueDevEx td.dxgv{
        padding:0px 0px;
    }
    .RoundLeftSideCorner{
        border-top-left-radius: 9px;
        border-bottom-left-radius: 9px;
        float:right;
        padding-top:4px;
    }
    .RoundRightSideCorner{
        border-top-right-radius:9px;
        border-bottom-right-radius:9px;
        float:left;
        padding-top:4px;
    }

    /*.RoundLeftSideCorner.softallocationbgcolor{
         border-left:3px dashed black !important;
    }
    .RoundRightSideCorner.softallocationbgcolor{
         border-left:3px dashed black !important;
    }*/
    /*.RoundLeftSideCorner:not(.constbgcolor){
        border-left:3px dashed black !important;
    }
    .RoundRightSideCorner:not(.constbgcolor){
        border-right:3px dashed black !important;
    }*/

    /*.RoundLeftSideCorner:not(.preconbgcolor){
        border-left:3px dashed black !important;
    }
    .RoundRightSideCorner:not(.preconbgcolor){
        border-right:3px dashed black !important;
    }*/

    /*.RoundLeftSideCorner:not(.closeoutbgcolor){
        border-left:3px dashed black !important;
    }
    .RoundRightSideCorner:not(.closeoutbgcolor){
        border-right:3px dashed black !important;
    }*/

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

    /*gvPreview::-webkit-scrollbar-thumb {
        border-radius: 5px;
        background-color: black;
    }
    gvPreview::-webkit-scrollbar {
        -webkit-appearance: none;
        width: 10px;
    }*/
    .ptoAlignmentClass {
    vertical-align:initial;
    }

    .date-info-tooltip {
            padding-bottom:3px;
        }

    .flex-container {
        display: flex;
        justify-content:space-between;
    }
    .preconbgcolor-constbgcolor{
    color:#FFFFFF !important;
    background: linear-gradient(to right, rgba(82, 190, 217, 1), rgba(0, 92, 155, 1));
    padding-top:4px;
    }
    .nobgcolor-preconbgcolor{
        color:#FFFFFF !important;
        background: linear-gradient(to right, rgba(214, 218, 217, 1), rgba(82, 190, 217, 1));
        padding-top:4px;
    }

    .constbgcolor-closeoutbgcolor{
        color:#FFFFFF !important;
        background: linear-gradient(to right, rgba(0, 92, 155, 1), rgba(53, 27, 130, 1));
        padding-top:4px;
    }
    .dx-tooltip-wrapper .dx-overlay-content .dx-popup-content {
    display: inline-block;
    padding: 7px 14px;
    font-size: .85em;
    line-height: normal;
    white-space: nowrap;
}
    .preconCellStyle{
        background-color: #52BED9;
        border: 5px solid #fff!important;
        font-weight:500;
    }
    .constCellStyle{
        color:#fff;
        background-color: #005C9B;
        border:5px solid #fff !important;
    }
    .closeoutCellStyle{
        color:#fff;
        background-color: #351B82;
        border:5px solid #fff !important;
    }
    .noDateCellStyle{
        color:#000000;
        background-color: #D6DAD9;
        border:5px solid #fff !important;
    }
    .v-align {
        vertical-align: middle!important;
    }
    .pointer{
        cursor:default;
    }
    .glabel2 img{
        width: 35px;
        height: 35px;
        border-radius: 20px;
        margin-right: 5px;
    }

    #loadpanelUpdate .dx-loadpanel-content {
        user-select: none;
        border: 1px solid #ddd;
        background: #fff;
        border-radius: 6px;
   
    }
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        ChangeLocation();
        setJquerytoolTip();
        setTimeout(function () {
            grid.PerformCallback();
            setTimeout(function () {
                grid.PerformCallback();
            }, 2000);
        }, 2000);
    });
    var AllocationData = [];
    function ChangeLocation() {
        $(".dxGridView_gvExpandedButton_UGITNavyBlueDevEx").attr("src", "/content/images/icons/right-angle-arrow.png");
        $(".dxGridView_gvCollapsedButton_UGITNavyBlueDevEx").attr("src", "/content/images/icons/down-angle-arrow.png");
        $("[id$='_gridAllocation_DXMainTable']").find("tr td.dxgv img").each(function () {
            $(this).addClass("imgHeightWidth");
            $(this).parent().next().find(".groupStyle").prepend(this);
            $(this).parent().next().find(".innerGroup").append(this);
        });
    }
    function setJquerytoolTip() {
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
        }
    }
    function OpenTimeOffAllocation(value, username) {
        SaveUserInfo();
        window.parent.UgitOpenPopupDialog('<%=editAllocationUrl %>&allocId=' + value + '&refreshpage=0', "", "Edit Resource Allocation for " + username, "650px", "500px");
    }
    var isZoomIn;
    function GZoomIn() {
        
        <%--if ($('#<%= hdndisplayMode.ClientID%>').val() == "Yearly") {
            $('#<%= hdndisplayMode.ClientID%>').val("HalfYearly")
        }
        else if ($('#<%= hdndisplayMode.ClientID%>').val() == "HalfYearly") {
            $('#<%= hdndisplayMode.ClientID%>').val("Quarterly")
        }
        else if ($('#<%= hdndisplayMode.ClientID%>').val() == "Quarterly") {
            $('#<%= hdndisplayMode.ClientID%>').val("Monthly")
        }
        else if ($('#<%= hdndisplayMode.ClientID%>').val() == "Monthly") {
            $('#<%= hdndisplayMode.ClientID%>').val("Weekly")
        }--%>

        if ($('#<%= hdndisplayMode.ClientID%>').val() == "Monthly") {
            $('#<%= hdndisplayMode.ClientID%>').val("Weekly");

            onChange();
            grid.PerformCallback();
        }
        
    }

    function GZoomOut() {
        <%--if ($('#<%= hdndisplayMode.ClientID%>').val() == "Weekly") {
            $('#<%= hdndisplayMode.ClientID%>').val("Monthly")
        }
        else if ($('#<%= hdndisplayMode.ClientID%>').val() == "Monthly") {
            $('#<%= hdndisplayMode.ClientID%>').val("Quarterly")
        }
        else if ($('#<%= hdndisplayMode.ClientID%>').val() == "Quarterly") {
            $('#<%= hdndisplayMode.ClientID%>').val("HalfYearly")
        }
        else if ($('#<%= hdndisplayMode.ClientID%>').val() == "HalfYearly") {
            $('#<%= hdndisplayMode.ClientID%>').val("Yearly")
        }--%>
        if ($('#<%= hdndisplayMode.ClientID%>').val() == "Weekly") {
            $('#<%= hdndisplayMode.ClientID%>').val("Monthly");

            onChange();
            grid.PerformCallback();
        }
    }

    function readCookie(cookieName) {
        var re = new RegExp('[; ]' + cookieName + '=([^\\s;]*)');
        var sMatch = (' ' + document.cookie).match(re);
        if (cookieName && sMatch) return unescape(sMatch[1]);
        return '';
    }
    function adjustGridHeight() {

    }
    function setScrollPosition() {
        if ($('#<%= hdndisplayMode.ClientID%>').val() == "Monthly") {
            var scrollPositionInPixex = readCookie("ScrollPositionInPixex");
            if (scrollPositionInPixex != "" && scrollPositionInPixex != "0") {
                $(".homeGrid .dxgvFCSD").scrollLeft(scrollPositionInPixex);
                set_cookie("ScrollPositionInPixex", 0);
            }
            else {
                var scrollPosition = readCookie("ScrollPosition");
                var offsetLeft = $(".current-scroll")[0].offsetWidth;
                $(".homeGrid .dxgvFCSD").scrollLeft(offsetLeft * scrollPosition);
            }
        }
    }
    $(document).ready(function () {
        var isAllocationtimeline;
        var urlParams = new URLSearchParams(window.location.href.split('?')[1]);
        for (let pair of urlParams.entries()) {
            if (pair[1] == 'resourceallocationgrid')
                isAllocationtimeline = true;
        }
        if (isAllocationtimeline) {
            grid.SetHeight($(window).height() + 100);
        }
        else {
            adjustGridHeight();
        }

        $(".resourceUti-gridFooterRow").find("td:eq(1)").addClass("allocationperworkitem");

        var tooltip = $("#tooltip").dxTooltip({
            target: $(".homeGrid"),
            contentTemplate: function (contentElement) {
                contentElement.append(
                    
                )
            }
        });

        //setScrollPosition();
    });


    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        window.parent.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }

    function next() {

        onChange();
    }

    function prev() {

        onChange();
    }

    function onChange() {
        LoadingPanel.SetText('Loading ...');
        LoadingPanel.Show();
    }

    function CloseUp(s, e) {
        if ($('#<%= hdnfilterTypeLoader.ClientID%>').val() != s.GetText()) {
            $('#<%= hdnfilterTypeLoader.ClientID%>').val(s.GetText());
            onChange();
        }
    }

    function OpenAddAllocationPopup(obj, userName) {
        window.parent.UgitOpenPopupDialog('<%= absoluteUrlView %>' + '&SelectedUsersList=' + obj, "", 'Add Allocation for ' + userName, '850px', '550px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

   
    function openResourceAllocationDialog(path, titleVal, returnUrl) {
        //window.parent.UgitOpenPopupDialog(path, '', titleVal, 60, 60, false, returnUrl);
        UgitOpenPopupDialog(path, '', titleVal, 60, 60, false, returnUrl);
    }

    function ShowEditImage(objthis) {
        //$(objthis).find('img').css('visibility', 'visible');
    }
    function HideEditImage(objthis) {
        //$(objthis).find('img').css('visibility', 'hidden');
    }

    function Grid_Init(s, e) {
        const month = parseInt('<%=ScrollMonth%>');
        const monthWidth = parseInt('<%=MonthColWidth%>');
        grid.SetHorizontalScrollPosition((month - 1) * monthWidth);
    }

    function Grid_EndCallback(s, e) {
        LoadingPanel.Hide();
        ChangeLocation();
        setJquerytoolTip();
    }

    function Grid_ColumnResized(s, e) {
        //AdjustSummaryTable();
    }

    

    function AdjustSummaryTable() {
        RemoveCustomStyleElement();

        var styleRules = [];
        var headerRow = GetGridHeaderRow(grid);
        if (typeof headerRow !== "undefined") {
            var bandheaderRow = headerRow.nextSibling;
            var headerCells = headerRow.getElementsByClassName("gridHeader");
            var bandheaderCells = bandheaderRow.getElementsByClassName("gridHeader");
            var totalWidth = 0;
            var count = 0;
            for (var i = 0; i < headerCells.length; i++) {
                if ($(headerCells[i]).prop("colSpan") <= 1) {

                    styleRules.push(CreateStyleRule(headerCells[i], i));
                    count++;
                }
            }

            for (var i = 0; i < bandheaderCells.length; i++) {
                styleRules.push(CreateStyleRule(bandheaderCells[i], i + count));
            }

            AppendStyleToHeader(styleRules);
        }
    }

    function CreateStyleRule(headerCell, headerIndex) {
        var width = headerCell.offsetWidth;
        var cellRule = ".summaryCell_" + headerIndex;
        return cellRule + ", " + cellRule + " .summaryTextContainer" + "{ width:" + width + "px; }";
    }

    function GetGridHeaderRow(grid) {

        var headers = grid.GetMainElement().getElementsByClassName("gridVisibleColumn");
        if (headers.length > 0)
            return headers[0].parentNode;
    }

    var customStyleElement = null;
    function AppendStyleToHeader(styleRules) {
        var container = document.createElement("DIV");
        container.innerHTML = "<style type='text/css'>" + styleRules.join("") + "</style>";

        var head = document.getElementsByTagName("HEAD")[0];
        customStyleElement = container.getElementsByTagName("STYLE")[0];

        head.appendChild(customStyleElement);
    }
    function RemoveCustomStyleElement() {
        if (customStyleElement) {
            customStyleElement.parentNode.removeChild(customStyleElement);
            customStyleElement = null;
        }
    }

    $(function () {
        $(".checkboxbutton").click(function () {
            onChange();
        });
    });



    function showLoader() {
        onChange();
    }


    function InitiateGridCallback(s, e) {
        if (!s.InCallback())
            grid.PerformCallback();
        //  LoadingPanel.Hide();
    }
    function reloadGrid() {
        cp.PerformCallback('', function (s) { adjustGridHeight(); });
    }

    function JSalert(selectedDepts) {
        if (selectedDepts == "") {
            Swal.fire({
                title: 'This process will take some time to completed!!',
                showClass: {
                    popup: 'animate__animated animate__fadeInDown'
                },
                hideClass: {
                    popup: 'animate__animated animate__fadeOutUp'
                },
                text: "Press proceed to continue!!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Proceed!'
            }).then((result) => {
                if (result.isConfirmed) {
                    cbpManagers.PerformCallback(selectedDepts);
                    showLoader();
                }
            })
        }
        else {
            cbpManagers.PerformCallback(selectedDepts);
            showLoader();
        }
    }

    function btnAddMultiAllocation_Click(s, e) {
        var url = '<%= MultiAddUrl%>';
        window.parent.UgitOpenPopupDialog(url, '', '', '880px', '90', 0, '');
    }

    function ClickOnDrillDown(obj, fieldname, caption) {
        hdnMonth.Set("selectedDate", fieldname);
        GZoomIn();
    }

    function ClickOnDrillUP(obj, fieldname, caption) {
        $('#<%= hndYear.ClientID%>').val(fieldname);
        GZoomOut();
    }

    //#region timesheet methods
    function openResourceTimeSheet(assignedTo, assignedToName) {
        showTimeSheet = true;
        //param isRedirectFromCardView is used to hide card view and show allocation grid
        var url = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGridNew&ViewName=FindAvailability&isRedirectFromCardView=true&selecteddepartment=-1&SelectedResource=" + assignedTo;
        window.parent.UgitOpenPopupDialog(url, "", "Timeline for User : " + assignedToName, "95", "95", "", false);
    }

    var expandAdaptiveDetailRow = function (key, dataGridInstance) {
        if (!dataGridInstance.isAdaptiveDetailRowExpanded(key)) {
            dataGridInstance.expandAdaptiveDetailRow(key);
        }
    }
    //#endregion

    function showTooltip(element, startDate, endDate, name, estAlloc)
    {
        var tooltip = $("#tooltip").dxTooltip("instance");
        tooltip.option({
            target: "#" + $(element).attr("id"),
            contentTemplate: function (contentElement) {
                contentElement.append(
                    $("<div />").addClass("date-info-tooltip").append(
                        $("<span style='font-weight:600;' />").text(name + " (" + estAlloc + ")")
                    ),
                    $("<div />").append(
                        $("<span />").text(startDate != endDate ? startDate + " to " + endDate : startDate)
                    ),
                );
            }
        });
        tooltip.show();

    }
    function hideTooltip(element) {        
        var tooltip = $("#tooltip").dxTooltip("instance");
        tooltip.hide();
    }

    function OpenAddAllocationPopup(obj, userName) {
        window.parent.UgitOpenPopupDialog('<%= absoluteUrlView %>' + '&SelectedUsersList=' + obj, "", 'Add Allocation for ' + userName, '850px', '550px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
    
    function openworkitem(workitem, username, subworkitem, preconStart, preconEnd, constStart, constEnd, closeOutStart, closeOutEnd) {
        var scrollPositionInPixex = $(".dxgvFCSD").scrollLeft();
        set_cookie("ScrollPositionInPixex", scrollPositionInPixex);
        $.get("/api/rmmapi/GetAllocationByWorkitem?AllocationID=" + workitem, function (data, status) {
            data.AssignedToName = username;
            data.TypeName = subworkitem;
            AllocationData = [];
            AllocationData.push(data);
            const popupContentTemplate1 = function () {
                let container = $("<div>");
                let datagrid = $("<div id='confirmationGrid'>").dxDataGrid({
                    dataSource: AllocationData,
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
                        },
                        {
                            dataField: "TypeName",
                            dataType: "text",
                            allowEditing: false,
                            caption: "Role",
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
                            allowEditing: true,
                            format: 'MMM d, yyyy',
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
                            allowEditing: true,
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
                            setCellValue: function (newData, value, currentRowData) {
                                let currentDate = new Date(new Date().format('yyyy-MM-dd') + "T00:00:00");
                                if (new Date(currentRowData.AllocationStartDate) < currentDate && new Date(currentRowData.AllocationEndDate) < currentDate) {
                                    //do nothing
                                }
                                else if (new Date(currentRowData.AllocationStartDate) < currentDate && new Date(currentRowData.AllocationEndDate) >= currentDate && currentRowData.AssignedToName != "") {
                                    let alloc1 = AllocationData.find(x => x.ID == currentRowData.ID);
                                    let alloc2 = JSON.parse(JSON.stringify(alloc1));
                                    alloc2.ID = -Math.abs(AllocationData.length + 1);
                                    alloc2.AllocationStartDate = new Date().format('yyyy-MM-dd') + "T00:00:00";
                                    alloc2.PctAllocation = value;
                                    alloc1.AllocationEndDate = new Date().addDays(-1).format('yyyy-MM-dd') + "T00:00:00";
                                    AllocationData.push(alloc2);
                                } else {
                                    let alloc1 = AllocationData.find(x => x.ID == currentRowData.ID);
                                    alloc1.PctAllocation = value;
                                }
                            }
                        },
                    ],
                    showBorders: true,
                    showRowLines: true,
                    onCellPrepared: function (e) {
                        if (e.rowType === 'data') {
                            var preconstartDate = new Date(preconStart);
                            var preconEndDate = new Date(preconEnd);

                            var conststartDate = new Date(constStart);
                            var constEndDate = new Date(constEnd);

                            var closeoutstartDate = new Date(closeOutStart);
                            var closeoutEndDate = new Date(closeOutEnd);

                            if (e.column.dataField == 'AllocationStartDate') {
                                let cellValue = new Date(e.data.AllocationStartDate)
                                if (isDateValid(preconEndDate) && cellValue <= preconEndDate) {
                                    e.cellElement.addClass('preconCellStyle');
                                }
                                else if (isDateValid(conststartDate) && isDateValid(constEndDate) &&
                                    cellValue <= constEndDate && cellValue >= conststartDate) {
                                    e.cellElement.addClass('constCellStyle');
                                }
                                else if (isDateValid(closeoutstartDate) && cellValue >= closeoutstartDate) {
                                    e.cellElement.addClass('closeoutCellStyle');
                                }
                                else
                                    e.cellElement.addClass('noDateCellStyle');
                            }
                            if (e.column.dataField == 'AllocationEndDate') {
                                let cellValue = new Date(e.data.AllocationEndDate)
                                if (isDateValid(preconEndDate) && cellValue <= preconEndDate) {
                                    e.cellElement.addClass('preconCellStyle');
                                }
                                else if (isDateValid(conststartDate) && isDateValid(constEndDate)
                                    && cellValue <= constEndDate && cellValue > conststartDate) {
                                    e.cellElement.addClass('constCellStyle');
                                }
                                else if (isDateValid(closeoutstartDate) && cellValue >= closeoutstartDate) {
                                    e.cellElement.addClass('closeoutCellStyle');
                                }
                                else
                                    e.cellElement.addClass('noDateCellStyle');
                            }
                        }
                    }
                });
                let confirmBtn = $(`<div class="mt-4 btnAddNew" style='float:right;padding: 0px 10px;font-size: 14px;' />`).dxButton({
                    text: "Save",
                    hint: 'Save Allocation',
                    visible: true,
                    onClick: function (e) {
                        var grid = $("#confirmationGrid").dxDataGrid("instance");
                        grid.saveEditData();
                        UpdateAllocation(data.TicketId, preconStart, preconEnd, constStart, constEnd);
                        popup.hide();
                    }
                })
                let cancelBtn = $(`<div class="mt-4 btnAddNew" style='float:right;padding: 0px 10px;font-size: 14px;' />`).dxButton({
                    text: "Cancel",
                    visible: true,
                    onClick: function (e) {
                        set_cookie("ScrollPositionInPixex", 0);
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
                title: "Edit Resource Allocation For " + username,
                visible: false,
                dragEnabled: true,
                hideOnOutsideClick: true,
                showCloseButton: true,
                position: {
                    at: 'center',
                    my: 'center',
                },
                onHiding: function () {
                    //CheckPhaseConstraints(true);
                }
            }).dxPopup('instance');

            popup.option({
                contentTemplate: () => popupContentTemplate1()

            });
            popup.show();
        });
    }
    function isDateValid(dateString) {
        var date = new Date(dateString);
        return date.toString() !== 'Invalid Date';
    }
    function UpdateAllocation(projectID, preconStart, preconEnd, constStart, constEnd) {
        var isValid = true;
        $.each(AllocationData, function (i, s) {
            if (s.TypeName == '') {
                DevExpress.ui.dialog.alert("Role is Required", "Error!");
                isValid = false;
                return;
            }

            if (typeof (s.AllocationStartDate) == "object") {
                if (s.AllocationStartDate != null) {
                    s.AllocationStartDate = (s.AllocationStartDate).toISOString();
                }
                else {
                    DevExpress.ui.dialog.alert("Start Date should not be blank.", "Error!");
                    isValid = false;
                    return;
                }
            } else if (s.AllocationStartDate == '') {
                DevExpress.ui.dialog.alert("Start Date should not be blank.", "Error!");
                isValid = false;
                return;
            }

            if (typeof (s.AllocationEndDate) == "object") {
                if (s.AllocationEndDate != null) {
                    s.AllocationEndDate = (s.AllocationEndDate).toISOString();
                }
                else {
                    DevExpress.ui.dialog.alert("End Date should not be blank.", "Error!");
                    isValid = false;
                    return;
                }
            } else if (s.AllocationEndDate == '') {
                DevExpress.ui.dialog.alert("End Date should not be blank.", "Error!");
                isValid = false;
                return;
            }

            if (new Date(s.AllocationEndDate) < new Date(s.AllocationStartDate)) {
                DevExpress.ui.dialog.alert("Start Date should be less than End Date.", "Error!");
                isValid = false;
                return;
            }
            if (typeof s.PctAllocation !== "undefined") {
                if (parseInt(s.PctAllocation) <= 0) {
                    DevExpress.ui.dialog.alert("Cannot make Allocation with 0 Percentage.", "Error!");
                    isValid = false;
                    return;
                }
                if (!s.PctAllocation) {
                    DevExpress.ui.dialog.alert("Cannot make Allocation with 0 Percentage.", "Error!");
                    isValid = false;
                    return;
                }
                if (parseInt(s.PctAllocation) > 100) {
                    DevExpress.ui.dialog.alert("Cannot make Allocation with Greater than 100 Percentage.", "Error!");
                    isValid = false;
                    return;
                }
            }
        });
        if (isValid) {
            $("#loadpanel").dxLoadPanel({
                message: "Making Change throughout the system. Change will take a few seconds...",
                visible: true,
                height: "auto",
                wrapperAttr: {
                    id: "loadpanelUpdate",
                    class: "class-name"
                }
            });
            $.post("/api/rmmapi/UpdateBatchCRMAllocations/", { Allocations: AllocationData, ProjectID: projectID, CalledFrom: 'CustomProjectTeamGantt', UpdateAllProjectAllocations: false, UseThreading: false, NeedReturnData: false }).then(function (response) {
                $("#loadpanel").dxLoadPanel("hide");
                if (response.includes("BlankAllocation")) {
                    DevExpress.ui.dialog.alert("Overlapping allocations are not permitted. Save unsuccessful.", "Error");
                }
                else if (response.includes("OverlappingAllocation")) {
                    DevExpress.ui.dialog.alert("Overlapping allocations are not permitted. Save unsuccessful.", "Error");
                }
                else if (response.includes("AllocationOutofbounds")) {
                    var resultvalues = response.split("~");
                    DevExpress.ui.dialog.alert("Allocation date entered is either prior to start date or after the end date of the resource.<br/>Name: " + resultvalues[4] + "<br/>Start Date: " + resultvalues[2] + " End Date: " + resultvalues[3] + ". <br/>Please enter valid dates.", "Error");
                }
                else if (response.includes("DateNotValid")) {
                    DevExpress.ui.dialog.alert("Start Date should be less than End Date. Save unsuccessful.", "Alert!");
                }
                else {                    
                    $("#loadpanel").dxLoadPanel("show");
                    grid.PerformCallback();
                    $("#loadpanel").dxLoadPanel("hide");
                    setTimeout(function () {
                        grid.PerformCallback();
                        $("#loadpanel").dxLoadPanel("hide");
                    }, 2000);
                }
                $.cookie("dataChanged", 0, { path: "/" });
            }, function (error) { $("#loadpanel").dxLoadPanel("hide"); });
        }
    }
</script>


<asp:HiddenField ID="hdnfilterTypeLoader" runat="server" />
<asp:HiddenField ID="hdndisplayMode" runat="server" Value="Monthly" />
<asp:HiddenField ID="hdnIndex" runat="server" />
<asp:HiddenField ID="hndYear" runat="server" />
<dx:ASPxHiddenField ID="hdnMonth" runat="server" ClientInstanceName="hdnMonth"></dx:ASPxHiddenField>
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="True">
    <Image Url="~/Content/Images/ajax-loader.gif"></Image>
</dx:ASPxLoadingPanel>
<div id="divLoadingPanel">

</div>

<% if (!Height.IsEmpty && !Width.IsEmpty)
    { %>
<div class="col-md-12 col-sm-12 col-xs-12 noSidePadding reourceUti-container resourceAllocationGrid" style="padding-left:10px;padding-top:10px;overflow-y: scroll; width: <%=Width%>; height: <%=Height%>;">
    <% }
        else
        { %>
    <div class="col-md-12 col-sm-12 col-xs-12 noSidePadding reourceUti-container">
        <% }%>
        <div id="trFilter" runat="server" visible="false" class="row">
            <div class="col-md-2 noSidePadding allocationTime-exportOptionWrap" style="margin-top: 14px">
               
                

            </div>
        </div>
        <dx:ASPxCallbackPanel ID="cp" runat="server" ClientInstanceName="cp" OnCallback="cp_Callback">
            <PanelCollection>
                <dx:PanelContent>
                    <div class="row" id="divGridTypeFilter" style="padding-top: 5px; padding-bottom: 2px;">
                        <div class="col-md-10 noSidePadding">
                            <div class="resourceAllo-gridchkBox" style="float: left; padding-left: 2px;display:none">
                                <div class="crm-checkWrap">
                                    <asp:CheckBox ID="chkIncludeClosed" Text="Include Closed Projects" runat="server" AutoPostBack="true"
                                         TextAlign="Right" CssClass="checkboxbutton RMM-checkWrap" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 col-sm-3 col-xs-12 px-0">
                                    <div id="divExpandCollapse" runat="server" class="resourceAllo-gridBtn" style="display:none">
                                        <div>
                                            <dx:ASPxButton ID="btnExpandAll" runat="server" AutoPostBack="False" RenderMode="Link">
                                                <Image Url="/content/images/expand-all-new.png" Width="18px"></Image>
                                                <ClientSideEvents Click="function(s, e) { grid.ExpandAll();}" />
                                            </dx:ASPxButton>

                                            <dx:ASPxButton ID="btnCollapseAll" runat="server" AutoPostBack="False" RenderMode="Link">
                                                <Image Url="/content/images/collapse-all-new.png" Width="18px"></Image>
                                                <ClientSideEvents Click="function(s, e) {grid.CollapseAll();}" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                    <div id="dvZoomLevel" runat="server" style="padding-top: 2px;">

                                        <asp:Button runat="server" ID="bZoomIn" OnClick="bZoomIn_Click" Text="click btn" Style="display: none" />
                                        <asp:Button runat="server" ID="bZoomOut" OnClick="bZoomOut_Click" Text="click btn" Style="display: none" />
                                        
                                        <dx:ASPxButton ID="btZoomIn" ClientInstanceName="btZoomIn" runat="server" Visible="false" RenderMode="Link" AutoPostBack="false">
                                            <Image Url="/content/images/zoomin-new.png" Height="24" Width="24" ></Image>
                                            <ClientSideEvents Click="function(s, e){ GZoomIn(); }" />
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btZoomOut" ClientInstanceName="btZoomOut" runat="server" Visible="false" RenderMode="Link" AutoPostBack="false">
                                            <Image Url="/content/images/zoomout-new.png" Height="24" Width="24" ></Image>
                                            <ClientSideEvents Click="function(s, e){ GZoomOut(); }" />
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-3 col-xs-12">
                                </div>
                                <div class="col-md-3 col-sm-3 col-xs-12">
                                    <div class=" bC-radioBtnWrap" style="display: none">
                                        <asp:RadioButton ID="rbtnFTE" runat="server" AutoPostBack="false" Text="FTE" CssClass="radiobutton importChk-radioBtn" onchange="reloadGrid()" GroupName="BarPercentage" />
                                        <asp:RadioButton ID="rbtnPercentage" runat="server" AutoPostBack="false" Text="%" CssClass="radiobutton importChk-radioBtn" onchange="reloadGrid()" GroupName="BarPercentage" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2 noSidePadding">
                            <div id="dvYearNavigation" runat="server" class="dvYearNavigation" style="display:none">
                                <span style="padding-right: 5px;">
                                    <asp:ImageButton ImageUrl="/content/images/icons/right-angle-arrow.png" Width="12" Style="transform: rotate(180deg);" ID="previousYear" ToolTip="Prevoius" runat="server" OnClientClick="prev()"
                                        OnClick="previousYear_Click" CssClass="resource-img" />
                                </span>
                                <asp:Label ID="lblSelectedYear" runat="server" Style=""></asp:Label>
                                <span style="padding-left: 5px;">
                                    <asp:ImageButton ImageUrl="/content/images/icons/right-angle-arrow.png" Width="12" ID="nextYear" ToolTip="Next" runat="server" OnClientClick="next()"
                                        OnClick="nextYear_Click" CssClass="resource-img" />
                                </span>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12 noSidePadding">
                            <dx:ASPxGridView ID="gvPreview" runat="server" AutoGenerateColumns="false" CssClass="gvPreview homeGrid" KeyFieldName="WorkItemID"
                                Width="100%" OnDataBinding="gvPreview_DataBinding" ClientInstanceName="grid" EnableCallBacks="true" SettingsBehavior-SortMode="Custom"
                                OnCustomSummaryCalculate="gvPreview_CustomSummaryCalculate" ClientVisible="true"
                                 OnClientLayout="gvPreview_ClientLayout"
                               OnHtmlDataCellPrepared="gvPreview_HtmlDataCellPrepared"
                                OnCustomColumnSort="gvPreview_CustomColumnSort"
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
                                    Init="Grid_Init" EndCallback="Grid_EndCallback" ColumnResized="Grid_ColumnResized" />
                                <Styles>
                                    <Row CssClass="ganttdataRow"></Row>
                                    <Header CssClass="gridHeader RMM-resourceUti-gridHeaderRow" />
                                    <GroupPanel CssClass="reportGrid-groupPannel"></GroupPanel>
                                    <%--<GroupRow CssClass="gridGroupRow estReport-gridGroupRow" />--%>
                                    <GroupFooter CssClass="estReport-groupFooterRow"></GroupFooter>
                                    <Footer Font-Bold="true" HorizontalAlign="Center" Border-BorderColor="#D9DAE0" Border-BorderStyle="Solid" Border-BorderWidth="1px"
                                        BorderRight-BorderWidth=".5px" CssClass="resourceUti-gridFooterRow">
                                    </Footer>
                                    <Table CssClass="timeline_gridview"></Table>
                                    <Cell Wrap="True"></Cell>
                                </Styles>
                                <SettingsDataSecurity AllowDelete="false" AllowEdit="false" />
                                <SettingsBehavior AllowGroup="true" AutoExpandAllGroups="true" />
                                <SettingsLoadingPanel Mode="Disabled" />
                                <SettingsPager PageSizeItemSettings-ShowAllItem="true">
                                </SettingsPager>
                                <Settings GroupFormat="{1}" ShowFooter="true" ShowStatusBar="Visible" HorizontalScrollBarMode="Visible" />
                            </dx:ASPxGridView>

                        </div>
                    </div>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxCallbackPanel>
    </div>

<div id="tooltip" class="tooltip">
    

</div>
<div id="confirmationDialog"></div>
<div id="loadpanel">
</div>