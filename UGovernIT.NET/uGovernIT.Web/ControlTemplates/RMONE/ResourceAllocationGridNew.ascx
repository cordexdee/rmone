<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResourceAllocationGridNew.ascx.cs" Inherits="uGovernIT.Web.ResourceAllocationGridNew" %>
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

    .nameLabel{
        font-weight:800;
        float:left;
        color:black;
        font-size:17px;
    }
    .roleLabel{
        /*clear:both;*/
        color:grey;
        /*position:absolute;*/
        margin-left:38px;
    }
    .user-image {
    float: left;
    height: 33px;
    width: 33px;
    border-radius: 50px;
    margin-top: 2px;
    margin-right: 5px;
    margin-bottom: 2px;
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
    .custom-task-color-7 {
        background-color: #F9AA33;
    }
    .custom-task-color-8 {
        background-color: #ACB8C0;
    }
    .custom-task-color-9 {
        background-color: #FF5757;
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
    }
    .glabel2{
        clear:both;
        font-size:14px;
        float:right;
        font-weight:600;
    }
    .glabel2red{
        clear:both;
        font-size:14px;
        float:right;
        font-weight:600;
    }
    .ganttdataRow td {
        border-bottom: 0px solid #f6f7fb !important;
    }
    /*a{
        font-weight:800;
    }*/
    .preconbgcolor-constbgcolor{
        color:#FFFFFF !important;
        background: linear-gradient(to right, rgba(82, 190, 217, 1), rgba(0, 92, 155, 1));
    }

    .constbgcolor-closeoutbgcolor{
        color:#FFFFFF !important;
        background: linear-gradient(to right, rgba(0, 92, 155, 1), rgba(53, 27, 130, 1));
    }
    .nobgcolor-preconbgcolor{
    color:#FFFFFF !important;
    background: linear-gradient(to right, rgba(214, 218, 217, 1),rgba(82, 190, 217, 1), rgba(82, 190, 217, 1));
    padding-top:4px;
}
    .preconbgcolor{
        background-color:#52BED9 !important;
        color:#ffffff !important;
    }
    .constbgcolor{
        background-color:#005C9B !important;
        color:#ffffff !important;
    }
    .closeoutbgcolor{
        background-color:#351B82 !important;
        color:#ffffff !important;
    }
    .ptobgcolor{
        background-color:#909090 !important;
    }
    .nobgcolor{
        background-color:#D6DAD9 !important;
        /*color:#ffffff !important;*/
    }
   /* .softallocationbgcolor{
        background-color:#ecf1f9 !important;
        border: 1.5px dashed !important;
        border-left: hidden !important;
        border-right: hidden !important;
        color:#000000 !important
    }

    .RoundLeftSideCorner.softallocationbgcolor{
        background-color:#ecf1f9 !important;
        border:1.5px dashed !important;
        opacity:0.8;
        border-right:hidden !important;
        margin-left: auto;
    }
    .RoundRightSideCorner.softallocationbgcolor{
        background-color:#ecf1f9 !important;
        border:1.5px dashed !important;
        opacity:0.8;
        border-left:hidden !important;
    }*/

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
    .cmicnoLabelred{
        font-weight:600;
        color:red;
    }
    .dxgvGroupRow_UGITNavyBlueDevEx td.dxgv{
        padding:0px 0px;
    }
    /*.RoundLeftSideCorner{
        border-top-left-radius: 9px;
        border-bottom-left-radius: 9px;
        margin-left: auto;
    }
    .RoundRightSideCorner{
        border-top-right-radius:9px;
        border-bottom-right-radius:9px;
    }*/
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
    vertical-align:middle;
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
        transform:rotate(270deg);
        background-image:none !important;
        margin-left: 7px;
    }
    .expandClassGantt {
        transform:rotate(90deg);
        background-image:none !important;
        margin-left:7px;
    }

    .dxgvHeader_UGITNavyBlueDevEx {
        cursor:context-menu;
    }

    .hand-cursor {
        cursor: pointer;
    }

    .date-info-tooltip {
            padding:3px;
        }

    .lead-user-tooltip {
        margin-top: -5px;
        font-weight: 700;
    }
    /*.flex-container {
        display: flex;
        justify-content:space-between;
    }*/
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
    .glabel2red a {
    color:red !important;
    }
    #toggleResumeTimeline .dx-button-content {
        padding:0px !important;
    }
    #toggleResumeTimeline .dx-icon {
        width: 18px;
        height: 18px;
    }

    .btnAddNew .dx-icon {
        filter: brightness(10);
    }

    .btnAddNew .dx-button-content {
        margin: 0px 3px;
    }

    #loadpanelUpdate .dx-loadpanel-content {
    user-select: none;
    border: 1px solid #ddd;
    background: #fff;
    border-radius: 6px;
   
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

.dx-tooltip-wrapper .dx-overlay-content {
    background-color: #F5F5F5;
}
    .userGanttview .dxgvFCSD {
    overflow : hidden !important;
    }
</style>

<script>
    
    function showTooltipPTO(element, startDate, endDate) {
        var tooltip = $("#tooltip").dxTooltip("instance");
        tooltip.option({
            target: "#" + $(element).attr("id"),
            contentTemplate: function (contentElement) {
                contentElement.append(
                    $("<div />").addClass("date-info-tooltip").append(
                        $("<div />").text(startDate + " to " + endDate)
                    )
                );

            }
        });
        tooltip.show();
    }
    function showTooltip(element, startDate, endDate, name, estalloc, roleName, chanceOfSuccess) {
        var startDateParts = startDate.split("</br>");
        var tooltip = $("#tooltip").dxTooltip("instance");
        tooltip.option({
            target: "#" + $(element).attr("id"),
            contentTemplate: function (contentElement) {
                contentElement.append(
                    $("<div />").addClass("date-info-tooltip").append(
                        $("<div />").text(name + " (" + estalloc + ")")
                    )
                );
                if (chanceOfSuccess != '') {
                    contentElement.append(
                        $("<div />").addClass("date-info-tooltip").append(
                            $("<div />").text(chanceOfSuccess)
                        )
                    );
                }
                contentElement.append(
                    $("<div />").addClass("date-info-tooltip").append(
                        $("<div />").text(roleName)
                    )
                );
                contentElement.append(
                    $("<div />").addClass("date-info-tooltip").append(
                        $("<div />").text(startDate + " to " + endDate)
                    )
                );
                
            }
        });
        tooltip.show();

    }

    function ShowLeadTooltip(element, ProjectLeadUser, LeadEstimatorUser, ProjectManagerUser) {        
        var tooltip = $("#tooltip").dxTooltip("instance");
        tooltip.option({            
            target: "#" + $(element).attr("id"),            
            contentTemplate: function (contentElement) {
                if (ProjectLeadUser != "" || LeadEstimatorUser != "" || ProjectManagerUser != "") {
                    contentElement.append(
                        $("<div />").addClass("date-info-tooltip").append(
                            $("<div />").text("Project Lead:")
                        )
                    );
                    contentElement.append(
                        $("<div />").addClass("date-info-tooltip lead-user-tooltip").append(
                            $("<div />").text(ProjectLeadUser)
                        )
                    );

                    contentElement.append(
                        $("<div />").addClass("date-info-tooltip").append(
                            $("<div />").text("Lead PM:")
                        )
                    );
                    contentElement.append(
                        $("<div />").addClass("date-info-tooltip lead-user-tooltip").append(
                            $("<div />").text(ProjectManagerUser)
                        )
                    );

                    contentElement.append(
                        $("<div />").addClass("date-info-tooltip").append(
                            $("<div />").text("Lead Estimator:")
                        )
                    );
                    contentElement.append(
                        $("<div />").addClass("date-info-tooltip lead-user-tooltip").append(
                            $("<div />").text(LeadEstimatorUser)
                        )
                    );
                }

            }
        });
        if (ProjectLeadUser != "" || LeadEstimatorUser != "" || ProjectManagerUser != "") {
            tooltip.show();
        } else {
            tooltip.hide();
        }
    }

    function showTooltipdoubleclick(element, tooltipText) {
        var tooltip = $("#tooltip").dxTooltip("instance");
        tooltip.option({
            target: "#" + $(element).attr("id"),
            contentTemplate: function (contentElement) {
                contentElement.append(
                    $("<div />").addClass("date-info-tooltip").append(
                        $("<div />").text(tooltipText)
                    )
                );
            }
        });
        tooltip.show();

    }

    function hideTooltip(element) {
        
        var tooltip = $("#tooltip").dxTooltip("instance");
        tooltip.hide();
    }
</script>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var gridExpanded = true;
    var AllocationData = [];
    $(document).ready(function () {
        ChangeLocationGantt();
        setJquerytoolTip();
        setGridState();
    });
    function ChangeLocationGantt() {
        $(".dxGridView_gvExpandedButton_UGITNavyBlueDevEx").attr("src", "/content/images/RMONE/left-arrow.png");
        $(".dxGridView_gvExpandedButton_UGITNavyBlueDevEx").addClass("expandClassGantt");
        $(".dxGridView_gvCollapsedButton_UGITNavyBlueDevEx").attr("src", "/content/images/RMONE/left-arrow.png");
        $(".dxGridView_gvCollapsedButton_UGITNavyBlueDevEx").addClass("collapseClassGantt");
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
    function OpenUserResume(userId) {
        window.parent.parent.UgitOpenPopupDialog('<%= OpenUserResumeUrl %>' + '&SelectedUser=' + userId, "", 'User Resume', '95', '95', "", false);
    }
    function OpenTimeOffAllocation(value, username) {
        //DevExpress.ui.dialog.alert("Cannot edit past schedules", "Warning!");
        //isValid = false;
        //return;
        
        window.parent.UgitOpenPopupDialog('<%=editUrl %>&allocId=' + value, "", "Edit Resource Allocation for " + username, "650px", "500px");
    }
    var isZoomIn;
    function GZoomIn() {
        //disable all view except weekly and monthly for now: mk required by subbu
        <%--if ($('#<%= hdndisplayMode.ClientID%>').val() == "Yearly") {
            $('#<%= hdndisplayMode.ClientID%>').val("HalfYearly")
        }
        else if ($('#<%= hdndisplayMode.ClientID%>').val() == "HalfYearly") {
            $('#<%= hdndisplayMode.ClientID%>').val("Quarterly")
        }
        else if ($('#<%= hdndisplayMode.ClientID%>').val() == "Quarterly") {
            $('#<%= hdndisplayMode.ClientID%>').val("Monthly")
        }
        else--%>
        if ($('#<%= hdndisplayMode.ClientID%>').val() == "Monthly") {
            $('#<%= hdndisplayMode.ClientID%>').val("Weekly");

            onChange();
            grid.PerformCallback();
        }

        
        //btnZoomIn.DoClick();
        //document.getElementById('<%= bZoomIn.ClientID %>').click();

    }

    function GZoomOut() {
        if ($('#<%= hdndisplayMode.ClientID%>').val() == "Weekly") {
            $('#<%= hdndisplayMode.ClientID%>').val("Monthly");

            onChange();
            grid.PerformCallback();
        }
        //disable all view except weekly and monthly for now: mk required by subbu
        <%--else if ($('#<%= hdndisplayMode.ClientID%>').val() == "Monthly") {
            $('#<%= hdndisplayMode.ClientID%>').val("Quarterly")
        }
        else if ($('#<%= hdndisplayMode.ClientID%>').val() == "Quarterly") {
            $('#<%= hdndisplayMode.ClientID%>').val("HalfYearly")
        }
        else if ($('#<%= hdndisplayMode.ClientID%>').val() == "HalfYearly") {
            $('#<%= hdndisplayMode.ClientID%>').val("Yearly")
        }--%>

        
    }

    function readCookie(cookieName) {
        var re = new RegExp('[; ]' + cookieName + '=([^\\s;]*)');
        var sMatch = (' ' + document.cookie).match(re);
        if (cookieName && sMatch) return unescape(sMatch[1]);
        return '';
    }
    function adjustGridHeight() {
        
    }
    $(document).ready(function () {
        //adjustGridHeight();
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
        $("#toast").dxToast({
            message: "Allocations Saved Successfully. ",
            type: "info",
            displayTime: 1000,
            position: "{ my: 'center', at: 'center', of: window }"
        });

        $("#toggleResumeTimeline").dxButton({
            text: "Resume",
            icon: "/content/Images/RMONE/compareresume_white.png",
            focusStateEnabled: false,
            visible: "<%=ShowUserResume%>" == "True" ? true : false,
            hint:"User Resume",
            onClick: function (e) {
                if ($(".userResume").is(":visible")) {
                    $(".userGanttview").show();
                    $(".userResume").hide();
                    //var headerLabel = $(window.parent.document).find(".ui-dialog-title")[1].innerHTML.replace("Resume:", "Gantt:");
                    //$(window.parent.document).find(".ui-dialog-title")[1].innerHTML = headerLabel;
                    $("#<%=divExpandCollapse.ClientID%>").css("display", "block");
                    e.component.option("icon", "/content/Images/RMONE/compareresume_white.png");
                    e.component.option("hint", "User Resume");
                    e.component.option("text", "Resume");

                }
                else {
                    $(".userGanttview").hide();
                    $(".userResume").show();
                    $(".global-searchWrap").addClass("noPadding");
                    $(".listcontainer").removeClass("listcontainer");
                    //var headerLabel = $(window.parent.document).find(".ui-dialog-title")[1].innerHTML.replace("Gantt:", "Resume:");
                    //$(window.parent.document).find(".ui-dialog-title")[1].innerHTML = headerLabel;
                    $("#<%=divExpandCollapse.ClientID%>").css("display", "none");
                    e.component.option("icon", "/content/Images/gantt_white.png");
                    e.component.option("hint", "User Gantt view");
                    e.component.option("text", "Gantt");
                }
            }
        });
    });


    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        window.parent.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }

    
    function setGridState() {

        var gridExpanded = $.cookie("GanttGridState") === "1" ? true : false;
        //if (gridExpanded == true)
        //    grid.ExpandAll();
        //else if (gridExpanded == false)
        //    grid.CollapseAll();
        //else
        //    grid.ExpandAll();
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
        //AdjustSummaryTable();

    }

    function Grid_EndCallback(s, e) {
        //AdjustSummaryTable();
        LoadingPanel.Hide();
        ChangeLocationGantt();
        setJquerytoolTip();
        setGridState();
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

    //function expandCollapseRow(s, e) {
    //    
    //    LoadingPanel.Show();
    //    if (s.IsGroupRow(e.visibleIndex) && !s.IsGroupRowExpanded(e.visibleIndex))
    //        s.ExpandRow(e.visibleIndex);
    //    else
    //        s.CollapseRow(e.visibleIndex);
    //}

    function ClickOnPrevious(obj, caption, mode) {
        if (mode == 'Monthly') {
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
            LoadingPanel.Show();
            grid.PerformCallback();
        } else {
            //let date = new Date(caption.substr(0, 4) + "01-" + caption.substr(4, 2));
            let date = new Date($.cookie("selectedDate"));
            date = addMonths(date, -1);
            $.cookie("selectedDate", new Date(date).format("MMM-dd-yy"));
            onChange();
            grid.PerformCallback();
        }
        
    }

    function ClickOnNext(obj, caption, mode) {
        if (mode == 'Monthly') {
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
            LoadingPanel.Show();
            grid.PerformCallback();
        } else {

            //let date = new Date(caption.substr(0, 4) + "01-" + caption.substr(4, 2));
            let date = new Date($.cookie("selectedDate"));
            date = addMonths(date, 1);
            $.cookie("selectedDate", new Date(date).format("MMM-dd-yy"));
            onChange();
            grid.PerformCallback();
        }
    }

    function ClickOnDrillDown(obj, fieldname, caption) {
        //hdnMonth.Set("selectedDate", fieldname);
        $.cookie("selectedDate", fieldname);
        GZoomIn();
    }

    function ClickOnDrillUP(obj, fieldname) {
        let year = ('' + new Date().getFullYear()).substr(0, 2)  + fieldname.substr(4);
        $('#<%= hndYear.ClientID%>').val(year);
        GZoomOut();
    }

    function getMonthNumber(dateString) {
        const monthPart = dateString.substring(0, 3).toLowerCase();
        const monthNames = [
            "jan", "feb", "mar", "apr", "may", "jun",
            "jul", "aug", "sep", "oct", "nov", "dec"
        ];

        const monthIndex = monthNames.indexOf(monthPart);
        if (monthIndex === -1) {
            throw new Error("Invalid month part in the provided date string.");
        }

        // Adding 1 to the monthIndex because JavaScript months are zero-based (0 for January).
        return monthIndex + 1;
    }
    function openworkitem(workitem, username, subworkitem, preconStart, preconEnd, constStart, constEnd, closeOutStart, closeOutEnd, startDate, endDate) {
        $.get("/api/rmmapi/GetAllocationByWorkitemID?WorkitemID=" + workitem, function (data, status) {
            debugger;
            data.forEach(function (item, index) {
                item.AssignedToName = username;
                item.TypeName = subworkitem;
                item.ProjectID = item.TicketId;
                item.AllocationStartDate = new Date(item.AllocationStartDate).format("MM/dd/yyyy");
                item.AllocationEndDate = new Date(item.AllocationEndDate).format("MM/dd/yyyy");
                return item;
            })
            AllocationData = data.filter(x => x.AllocationStartDate == new Date(startDate).format("MM/dd/yyyy") && x.AllocationEndDate == new Date(endDate).format("MM/dd/yyyy"));

            //data.AssignedToName = username;
            //data.TypeName = subworkitem;
            //AllocationData = [];
            //AllocationData.push(data);
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
                            sortIndex: 0,
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
                                //if (new Date(currentRowData.AllocationStartDate) < currentDate && new Date(currentRowData.AllocationEndDate) < currentDate) {
                                //    //do nothing
                                //    DevExpress.ui.dialog.alert("Cannot edit past schedules", "Warning!");
                                //    isValid = false;
                                //    return;
                                //}
                                //else
                                    if (new Date(currentRowData.AllocationStartDate) < currentDate && new Date(currentRowData.AllocationEndDate) >= currentDate) {
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
                        let anyItemWithTicketId = data.find(function (item) { return item.TicketId });
                        UpdateAllocation(anyItemWithTicketId.TicketId, preconStart, preconEnd, constStart, constEnd);
                        popup.hide();
                    }
                })
                let cancelBtn = $(`<div class="mt-4 btnAddNew" style='float:right;padding: 0px 10px;font-size: 14px;' />`).dxButton({
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

            const popup = $("#confirmationDialog").dxPopup({
                contentTemplate: popupContentTemplate1,
                width: "900",
                height: "auto",
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
                height:"auto",
                wrapperAttr: {
                    id: "loadpanelUpdate",
                    class: "class-name"
                }
            });
            $.post("/api/rmmapi/UpdateBatchCRMAllocations/", { Allocations: AllocationData, ProjectID: projectID, CalledFrom: 'ResourceAllocationGridNew', UpdateAllProjectAllocations: false, UseThreading: false, NeedReturnData: false }).then(function (response) {
                if (response.includes("BlankAllocation")) {
                    $("#loadpanel").dxLoadPanel("hide");
                    DevExpress.ui.dialog.alert("Overlapping allocations are not permitted. Save unsuccessful.", "Error");
                }
                else if (response.includes("OverlappingAllocation")) {
                    $("#loadpanel").dxLoadPanel("hide");
                    DevExpress.ui.dialog.alert("Overlapping allocations are not permitted. Save unsuccessful.", "Error");
                }
                else if (response.includes("AllocationOutofbounds")) {
                    $("#loadpanel").dxLoadPanel("hide");
                    var resultvalues = response.split("~");
                    DevExpress.ui.dialog.alert("Allocation date entered is either prior to start date or after the end date of the resource. <br/>Start Date: " + resultvalues[2] + " End Date: " + resultvalues[3] + ". <br/>Please enter vallid dates.", "Error");
                }
                else if (response.includes("DateNotValid")) {
                    $("#loadpanel").dxLoadPanel("hide");
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
                    <div class="row" id="divGridTypeFilter" style="padding-top: 7px; padding-bottom: 7px;">
                        <div class="col-md-10 noSidePadding">
                            <div class="resourceAllo-gridchkBox" style="float: left; padding-left: 2px;display:none">
                                <div class="crm-checkWrap">
                                    <asp:CheckBox ID="chkIncludeClosed" Text="Include Closed Projects" runat="server" AutoPostBack="true"
                                        OnCheckedChanged="chkIncludeClosed_CheckedChanged" TextAlign="Right" CssClass="checkboxbutton RMM-checkWrap" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2 col-sm-2 col-xs-12">
                                    <div id="divExpandCollapse" runat="server" class="resourceAllo-gridBtn">
                                        <div>
                                            <dx:ASPxButton ID="btnExpandAll" runat="server" AutoPostBack="False" RenderMode="Link" ToolTip="Expand All">
                                                <Image Url="/content/images/expand-all-new.png" Width="18px"></Image>
                                                <ClientSideEvents Click="function(s, e) { grid.ExpandAll(); $.cookie('IsGanttViewExpanded', 1, { path: '/' });}" />
                                            </dx:ASPxButton>

                                            <dx:ASPxButton ID="btnCollapseAll" runat="server" AutoPostBack="False" RenderMode="Link"  ToolTip="Collapse All">
                                                <Image Url="/content/images/collapse-all-new.png" Width="18px"></Image>
                                                <ClientSideEvents Click="function(s, e) {grid.CollapseAll(); $.cookie('IsGanttViewExpanded', 0, { path: '/' });}" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                    <div id="dvZoomLevel" runat="server" style="padding-top: 2px;display:none;">

                                        <asp:Button runat="server" ID="bZoomIn" OnClick="bZoomIn_Click" Text="click btn" Style="display: none" />
                                        <asp:Button runat="server" ID="bZoomOut" OnClick="bZoomOut_Click" Text="click btn" Style="display: none" />
                                        
                                        <dx:ASPxButton ID="btZoomIn" ClientInstanceName="btZoomIn" runat="server" RenderMode="Link" AutoPostBack="false" ToolTip="Zoom In">
                                            <Image Url="/content/images/zoomin-new.png" Height="24" Width="24" ></Image>
                                            <ClientSideEvents Click="function(s, e){ GZoomIn(); }" />
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btZoomOut" ClientInstanceName="btZoomOut" runat="server" RenderMode="Link" AutoPostBack="false" ToolTip="Zoom Out">
                                            <Image Url="/content/images/zoomout-new.png" Height="24" Width="24" ></Image>
                                            <ClientSideEvents Click="function(s, e){ GZoomOut(); }" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div id="toggleResumeTimeline" class="btnAddNew" style="border:0px;padding:2px;"></div>
                                </div>
                                <div class="col-md-3 col-sm-3 col-xs-12">
                                    <div id="divDateInfo" class="paneldiv tooltipp" style="width:200px;display:none;">
                                        <div class="d-flex">
                                            <div class="date-box <%=classNameSt %>"><%=ganttProjSD %></div>
                                            <div class="date-box <%=classNameEd %>"><%=ganttProjED %></div>
                                        </div>
                                            <div class='date-box-1'><%=ganttProjReqAloc %></div>
                                    </div>
                                </div>
                                <script>
                                    //debugger;
                                    var ShowDateInfo = '<%=ShowDateInfo%>';
                                    var RequestFromProjectAllocation = '<%=RequestFromProjectAllocation%>';
                                    if (RequestFromProjectAllocation == 'True' && ShowDateInfo == 'True') {
                                        $("#divDateInfo").show();
                                    }
                                    else {
                                        $("#divDateInfo").hide();
                                    }
                                </script>
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
                                    <dx:ASPxButton ID="btnPreviousYear" runat="server" ToolTip="Previous" RenderMode="Link" Width="7" style="margin-top:-5px" 
                                        Image-Url="/content/images/RMONE/left-arrow.png" AutoPostBack="false">
           
                                    </dx:ASPxButton>
                                </span>
                                <asp:Label ID="lblSelectedYear" runat="server"  Style="font-size:16px;font-weight:500;"></asp:Label>
                                <span style="padding-left: 5px;">
                                    <dx:ASPxButton ID="btnNextYear" runat="server" ToolTip="Next" Width="7" style="transform:rotate(180deg);margin-top:-5px"
                                         Image-Url="/content/images/RMONE/left-arrow.png" AutoPostBack="false">
            
                                    </dx:ASPxButton>
                                </span>
                            </div>
                        </div>
                    </div>

                    <div class="row userGanttview">
                        <div class="col-md-12 col-sm-12 col-xs-12 noSidePadding ">
                            <dx:ASPxGridView ID="gvPreview" runat="server" AutoGenerateColumns="false" CssClass="gvPreview homeGrid" KeyFieldName="WorkItemID"
                                Width="100%" OnDataBinding="gvPreview_DataBinding" ClientInstanceName="grid" EnableCallBacks="true" SettingsBehavior-SortMode="Custom"
                                OnCustomSummaryCalculate="gvPreview_CustomSummaryCalculate" ClientVisible="true"
                                 OnClientLayout="gvPreview_ClientLayout"
                               OnHtmlDataCellPrepared="gvPreview_HtmlDataCellPrepared"
                                OnHtmlRowCreated="gvPreview_HtmlRowCreated"
                                OnCustomColumnSort="gvPreview_CustomColumnSort"
                                OnCustomCallback="gvPreview_CustomCallback"
                                OnHtmlRowPrepared="gvPreview_HtmlRowPrepared"
                                OnAfterPerformCallback="gvPreview_AfterPerformCallback">
                                
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
                                <Settings GroupFormat="{1}" ShowFooter="true" ShowStatusBar="Visible" UseFixedTableLayout="true" HorizontalScrollBarMode="Visible" />
                            </dx:ASPxGridView>

                        </div>
                    </div>
                    <div class="row userResume" style="display:none;">
                        <div class="col-md-12 col-sm-12 col-xs-12 noSidePadding ">
                            <asp:PlaceHolder runat="server" ID="UserResumePH"></asp:PlaceHolder>
                        </div>
                    </div>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxCallbackPanel>
    </div>

<div id="tooltip" class="tooltip">
</div>
    <div id="EditAllocationDiv">

    </div>
    <div id="confirmationDialog"></div>
<div id="toast"></div>
<div id="loadpanel">
</div>