<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResourceAllocationGrid.ascx.cs" Inherits="uGovernIT.Web.ResourceAllocationGrid" %>
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
        /*color: inherit;*/
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
    /*.excelicon, .pdf-icon, .sendmail, .savetofolder {
        background-repeat: no-repeat;
        width: 22px;
        height: 22px;
    }

    .excelicon {
        background-image: url(/content/images/excel_icon.png);
    }

    .pdf-icon {
        background-image: url(/content/images/pdf_icon.png);
    }

    .sendmail {
        background-image: url(/content/Images/MailTo.png);
    }

    .savetofolder {
        background-image: url(/content/images/saveToFolder.png);
    }*/

    .summaryTable {
        border-width: 0;
        border-spacing: 0;
        font-weight: bold;
    }

        .summaryTable td {
            padding: 0;
        }

    /*.summaryTable td .summaryTextContainer {
                overflow: hidden;
                text-align: center;
            }*/

    .gridGroupRow td:last-child {
        padding: 0;
    }

    .SummaryHeaderAdjustment {
        margin-top: -19px;
        position: relative;
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
    /*.valueViewMode {
        border: solid;border-width: 1px;border-color: #D9DAE0;padding-left: 10px;padding-right: 10px;height: 24px;padding-top: 1px;padding-bottom: 5px;
    }
    .valueViewMode label {
        position:relative;
        top:2px;
    }*/
    .dvYearNavigation {
        float: right;
        padding-right: 15px;
        padding-top: 5px;
        filter:brightness(0);
    }

    /*.btnSetTypeColor .dxb {
        padding: 0px !important;
    font-size: 10px;
    }*/

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

    .reportPanelImage {
        display: inline-block;
        padding-left:5px;            
    }

    .displayHide{
        display:none;
    }
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var isZoomIn;
    function GZoomIn() {
        if ($('#<%= hdndisplayMode.ClientID%>').val() == "Yearly") {
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
        }

        onChange();
        //btnZoomIn.DoClick();
        document.getElementById('<%= bZoomIn.ClientID %>').click();

    }

    function GZoomOut() {
        if ($('#<%= hdndisplayMode.ClientID%>').val() == "Weekly") {
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
        }

        onChange();
        document.getElementById('<%= bZoomOut.ClientID %>').click();
    }

    function readCookie(cookieName) {
        var re = new RegExp('[; ]' + cookieName + '=([^\\s;]*)');
        var sMatch = (' ' + document.cookie).match(re);
        if (cookieName && sMatch) return unescape(sMatch[1]);
        return '';
    }
    function adjustGridHeight() {
        
        var windowHeight = $(window).height();
        if (grid.GetVisibleRowsOnPage() == 0) {
            grid.SetHeight(0);
        }
        else {
            $('.dxgvCSD').height('auto');
            //var mainContainer = $('.resourceAllocationGrid').height();
            //var filterHeight = $('.setAllocationTabButtonWidth').innerHeight();
            //var GridTypeFilterHeight = $('#divGridTypeFilter').innerHeight();
            //var row = $('.estReport-dataRow');
            //var groupRow = $('.gridGroupRow').length;
            //var groupHeight = $('.gridGroupRow').innerHeight();
            //if (typeof groupHeight != "undefined" && typeof groupRow != "undefined") {
            //    groupHeight = groupHeight * groupRow;
            //}
            //var rowHeight = 110;
            //if (typeof row != "undefined") {
            //    var spaceHeight = grid.GetVisibleRowsOnPage() * 7;
            //    rowHeight = row.innerHeight() + spaceHeight;
            //}
            //var gridHeight = (grid.GetVisibleRowsOnPage() * rowHeight) + groupHeight + GridTypeFilterHeight;


            //if (filterHeight != undefined) {
            //    gridHeight = gridHeight + filterHeight;
            //}
            //if (GridTypeFilterHeight != undefined) {
            //    gridHeight = gridHeight + GridTypeFilterHeight;
            //}
            //if (mainContainer > gridHeight) {
            //    grid.SetHeight(gridHeight);
            //}
            //else {
            //    if (filterHeight == undefined) {
            //        grid.SetHeight(mainContainer - 60);
            //    }
            //    else {
            //        if (mainContainer == undefined) {
            //            grid.SetHeight(windowHeight - 50);
            //            console.log("windows :" + windowHeight - 50);
            //        }
            //        else {
            //            grid.SetHeight(mainContainer - 60 - filterHeight);
            //        }
            //    }
            //}
        }
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

        $(".reportPanelImage").click(function () {
            debugger;
            if ($(".ExportOption-btns").is(":visible") == true)
                $(".ExportOption-btns").hide();
            else
                $(".ExportOption-btns").show();
          
        });
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
        window.parent.UgitOpenPopupDialog('<%= absoluteUrlView %>' + '&SelectedUsersList=' + obj, "", 'Add Allocation for ' + userName, '650px', '500px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function CancelUserAllocationTimelineReport() {
        $("#divUserAllocationTimelineReport").css("display", "none");
        return false;
    }

    function RunUserAllocationTimelineReportPDFRun() {
        btnPdfExport.onClick();
        $("#divUserAllocationTimelineReport").css("display", "none");
        return false;
    }

    function RunUserAllocationTimelineReportExcelRun() {
        btnExcelExport.onClick()
        $("#divUserAllocationTimelineReport").css("display", "none");
        return false;
    }


    function ExportPDFReport(obj) {
        $("#divUserAllocationTimelineReport").css({ 'top': ($(obj).position().top - 10) + 'px', 'display': 'block', 'left': ($(obj).position().left - $("#divUserAllocationTimelineReport").width()) + $(obj).width() + 'px' });
        $('#<%= btnUserAllocationTimelineReportPDFRun.ClientID%>').css("display", "block");
        $('#<%= btnUserAllocationTimelineReportExcelRun.ClientID%>').css("display", "none");
        return false;
    }

    function ExportExcelReport(obj) {
        $("#divUserAllocationTimelineReport").css({ 'top': ($(obj).position().top - 10) + 'px', 'display': 'block', 'left': ($(obj).position().left - $("#divUserAllocationTimelineReport").width()) + $(obj).width() + 'px' });
        $('#<%= btnUserAllocationTimelineReportPDFRun.ClientID%>').css("display", "none");
        $('#<%= btnUserAllocationTimelineReportExcelRun.ClientID%>').css("display", "block");
        return false;
    }

    function openResourceAllocationDialog(path, titleVal, returnUrl) {
        //window.parent.UgitOpenPopupDialog(path, '', titleVal, 60, 60, false, returnUrl);
        UgitOpenPopupDialog(path, '', titleVal, 60, 60, false, returnUrl);
    }

    function ShowEditImage(objthis) {
        $(objthis).find('img').css('visibility', 'visible');
    }
    function HideEditImage(objthis) {
        $(objthis).find('img').css('visibility', 'hidden');
    }

    function Grid_Init(s, e) {
        AdjustSummaryTable();

    }

    function Grid_EndCallback(s, e) {

        AdjustSummaryTable();
        LoadingPanel.Hide();
    }

    function Grid_ColumnResized(s, e) {
        AdjustSummaryTable();
    }

    function Grid_BatchEditStartEditing(s, e) {
        var rbtnPercentage = document.getElementById('<%= rbtnPercentage.ClientID %>').checked;
        if (rbtnPercentage == false) {
            e.CancelEdit();
            $('#editControlBtnContainer').hide();
        }
        else {
            $('#editControlBtnContainer').show();

        }
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

    function grid_BatchUpdate() {
        grid.UpdateEdit();
        $('#editControlBtnContainer').hide();
    }

    function grid_CancelBatchUpdate() {
        grid.CancelEdit();
        $('#editControlBtnContainer').hide();
    }

    function showLoader() {
        onChange();
    }

    function LoaderOnExport() {
        LoadingPanel.SetText('Exporting ...');
        LoadingPanel.Show();
        setTimeout(function () { LoadingPanel.Hide(); }, 3000);
    }

    function LoaderOnPrint() {
        showLoader();
        setTimeout(function () { LoadingPanel.Hide(); }, 3000);
    }

    function onDepartmentChanged(ccID) {
        var cmbDepartment = $("#" + ccID + " span");
        var selectedDepts = "";
        for (i = 0; i < cmbDepartment.length; i++)
            selectedDepts = selectedDepts + cmbDepartment[i].id + ",";
        var selectedDepartments = cmbDepartment.attr("id");
        JSalert(selectedDepts);
        //if (confirm("This process will take few minutues, Click Ok to proceed else cancel !!")) {
        //    cbpManagers.PerformCallback(selectedDepts);
        //    showLoader();
        //}
        //else
        //    return false;




        //grid.PerformCallback(selectedDepartments);
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

    function OpenNewAllocationGantt(s, e) {
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
                window.parent.UgitOpenPopupDialog('<%=AllocationGanttURL%>', "", "Allocation Timeline", "95", "95", false, "");
            }
        })
    }
</script>

<%--<asp:HiddenField ID="hdnSelectedResourceId" runat="server" />
<asp:HiddenField ID="hdnSelectedTicketId" runat="server" />
<asp:Button ID="btnDeleteAllocation" runat="server" OnClick="btnDeleteAllocation_Click" style="display:none;"/>--%>


<asp:HiddenField ID="hdnfilterTypeLoader" runat="server" />
<asp:HiddenField ID="hdndisplayMode" runat="server" Value="Monthly" />
<asp:HiddenField ID="hdnIndex" runat="server" />
<asp:HiddenField ID="hndYear" runat="server" />
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Text="Loading..." ImagePosition="Top" CssClass ="customeLoader" Modal="True">
    <Image Url="~/Content/Images/ajaxloader.gif"></Image>
</dx:ASPxLoadingPanel>
<div id="divUserAllocationTimelineReport" style="display: none; height: 200px; width: 400px;" class="ModuleBlock">
    <fieldset>
        <legend>User Allocation Timeline Report</legend>
        <table cellspacing="10px" cellpadding="5px">
            <tr>
                <td class="titleTD" width="100%" align="center" style="text-align: center">
                    <asp:Panel ID="Panel2" runat="server" Style="float: left; padding-left: 3px; width: 100%; text-align: center;">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td align="center">
                                        <table width="400px" class="filterpopup-table" frame="box" cellpadding="0" cellspacing="0" border="0">

                                            <tr id="tr1">
                                                <td class="ms-formlabel labelClass" style="width: 30%; text-align: left;">

                                                    <asp:Label ID="lblHeader" CssClass="filter-label" Text="Header:" runat="server"></asp:Label>

                                                </td>
                                                <td class="ms-formbody textBoxClass" style="width: 70%; text-align: left;">
                                                    <asp:TextBox ID="txtHeader" runat="server" Width="200px"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr id="tr3">
                                                <td class="ms-formlabel labelClass" style="width: 30%; text-align: left;">
                                                    <asp:Label ID="lblFooter" CssClass="filter-label" Text="Footer:" runat="server"></asp:Label>
                                                </td>
                                                <td class="ms-formbody textBoxClass" style="width: 70%; text-align: left;">
                                                    <asp:TextBox ID="txtFooter" runat="server" Width="200px"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr id="tr2">
                                                <td class="ms-formlabel labelClass" style="width: 30%; text-align: left;">

                                                    <asp:Label ID="lblShowCompanyLogo" CssClass="filter-label" Text="Show Company Logo:" runat="server"></asp:Label>

                                                </td>
                                                <td class="ms-formbody textBoxClass" style="width: 70%; text-align: left;">
                                                    <asp:CheckBox ID="chkShowCompanyLogo" runat="server" />
                                                </td>
                                            </tr>

                                            <tr id="tr4">
                                                <td class="ms-formlabel labelClass" style="width: 30%; text-align: left;">
                                                    <asp:Label ID="lblShowDateInFooter" CssClass="filter-label" Text="Show Date In Footer:" runat="server"></asp:Label>
                                                </td>
                                                <td class="ms-formbody textBoxClass" style="width: 70%; text-align: left;">
                                                    <asp:CheckBox ID="chkShowDateInFooter" runat="server" />
                                                </td>
                                            </tr>

                                            <tr id="tr5">
                                                <td class="ms-formlabel labelClass" style="width: 30%; text-align: left;">
                                                    <asp:Label ID="lblAdditionalInfo" CssClass="filter-label" Text="Additional Info Header:" runat="server"></asp:Label>
                                                </td>
                                                <td class="ms-formbody textBoxClass" style="width: 70%; text-align: left;">
                                                    <asp:TextBox ID="txtAdditionalInfo" runat="server" Width="200px"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr id="tr6">
                                                <td class="ms-formlabel labelClass" style="width: 30%; text-align: left;">
                                                    <asp:Label ID="lblAdditionalFooterInfo" CssClass="filter-label" Text="Additional Footer Info:" runat="server"></asp:Label>
                                                </td>
                                                <td class="ms-formbody textBoxClass" style="width: 70%; text-align: left;">
                                                    <asp:TextBox ID="txtAdditionalFooterInfo" runat="server" Width="200px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ms-formlabel labelClass" style="padding-top: 5px;">
                                        <div style="float: right;">
                                            <asp:Button ID="btnUserAllocationTimelineReportPDFRun" runat="server" Style="float: left; padding-right: 5px; margin-right: 5px;" Text="Run" OnClientClick="return RunUserAllocationTimelineReportPDFRun(this)" />
                                            <asp:Button ID="btnUserAllocationTimelineReportExcelRun" runat="server" Style="float: left; padding-right: 5px; margin-right: 5px;" Text="Run" OnClientClick="return RunUserAllocationTimelineReportExcelRun(this)" />
                                            <asp:Button ID="btnUserAllocationTimelineReportCancel" runat="server" Style="float: left;" Text="Cancel" OnClientClick="return CancelUserAllocationTimelineReport(this);" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </asp:Panel>
                    <asp:Label ID="lblUserAllocationTimelineErrorMessage" runat="server" Visible="false" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </fieldset>
</div>

<% if (!Height.IsEmpty && !Width.IsEmpty)
    { %>
<div class="col-md-12 col-sm-12 col-xs-12 noSidePadding reourceUti-container resourceAllocationGrid" style="overflow-y: scroll; width: <%=Width%>; height: <%=Height%>;">
    <% }
        else
        { %>
    <div class="col-md-12 col-sm-12 col-xs-12 noSidePadding reourceUti-container">
        <% }%>
        <div id="trFilter" runat="server" visible="false" class="row">
            <div class="col-md-10 noSidePadding">
                <div id="divFilter" runat="server" style="width: 100%;">
                    <div class="col-md-12 col-sm-12 col-xs-12 noSidePadding setAllocationTabButtonWidth">
                        <div class="row resourceUti-filterWarp">
                            <div class="col-md-3 col-sm-3 col-xs-12">
                                <div class="resourceUti-label">
                                    <label>Department:</label>
                                </div>
                                <div class="resourceUti-inputField">
                                    <ugit:LookupValueBoxEdit ID="ddlDepartment" runat="server" IsMulti="true" CssClass="rmmLookup-valueBoxEdit" FieldName="DepartmentLookup" JsCallbackEvent="onDepartmentChanged" />

                                </div>
                            </div>
                            <div class="col-md-2" style="display: none;">
                                <div class="resourceUti-label">
                                    <label>Functional Area:</label>
                                </div>
                                <div class="resourceUti-inputField">
                                    <asp:DropDownList CssClass="txtbox-halfwidth aspxDropDownList rmm-dropDownList" ID="ddlFunctionalArea" runat="server" onchange="onChange()"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlFunctionalArea_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-3 col-sm-3 col-xs-12">
                                <div class="resourceUti-label">
                                    <label>Manager:</label>
                                </div>
                                <div class="resourceUti-inputField">
                                    <dx:ASPxCallbackPanel ID="cbpManagers" runat="server" ClientInstanceName="cbpManagers" OnCallback="cbpManagers_Callback" SettingsLoadingPanel-Enabled="false">
                                        <PanelCollection>
                                            <dx:PanelContent>
                                                <asp:DropDownList ID="ddlResourceManager" CssClass="managerdropdown aspxDropDownList rmm-dropDownList" onchange="onChange()" runat="server"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlResourceManager_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdnaspDepartment" runat="server" />
                                            </dx:PanelContent>
                                        </PanelCollection>
                                        <ClientSideEvents EndCallback="InitiateGridCallback" />
                                    </dx:ASPxCallbackPanel>
                                </div>
                            </div>
                            <div class="col-md-3 col-sm-3 col-xs-12">
                                <div class="resourceUti-label">
                                    <label>Type:</label>
                                </div>
                                <div class="resourceUti-inputField">
                                    <div>
                                        <dx:ASPxGridLookup Visible="true" TextFormatString="{0}" Width="100%" SelectionMode="Multiple" ID="glType" runat="server"
                                            KeyFieldName="LevelTitle" MultiTextSeparator="; " AutoPostBack="false" CssClass="rmmGridLookup"
                                            DropDownWindowStyle-CssClass="RMMaspxGridLookup-dropDown" GridViewStyles-Row-CssClass="aspxGridloookUp-drpDownRow"
                                            GridViewStyles-FilterRow-CssClass="aspxGridLookUp-FilerWrap"
                                            GridViewStyles-FilterCell-CssClass="aspxGridLookUp-FilerCell" GridViewProperties-Settings-VerticalScrollableHeight="200">
                                            <Columns>
                                                <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="28px" />
                                                <dx:GridViewDataTextColumn FieldName="LevelTitle" Width="200px" Caption="Choose Type:">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="ModuleName" Visible="false"></dx:GridViewDataTextColumn>
                                            </Columns>
                                            <GridViewProperties>
                                                <Settings ShowGroupedColumns="false" ShowFilterRow="true" VerticalScrollBarMode="Auto" />
                                                <SettingsBehavior AllowSort="false" />
                                                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                            </GridViewProperties>
                                            <ClientSideEvents CloseUp= "CloseUp" EndCallback="InitiateGridCallback"/>
                                        </dx:ASPxGridLookup>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-1 col-sm-2 col-xs-12">

                                <%--<dx:ASPxButton ID="btnSetTypeColor" CssClass=" primary-blueBtn" runat="server" AutoPostBack="false" Text="Set Color">
                            </dx:ASPxButton>--%>
                            </div>

                            <%--   <td style="padding-left: 10px;">Group:</td>
                        <td style="padding-right: 5px;">
                            <asp:DropDownList ID="ddlUserGroup" runat="server" AutoPostBack="true" onchange="onChange()" OnSelectedIndexChanged="ddlUserGroup_SelectedIndexChanged"></asp:DropDownList>
                        </td>--%>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-2 noSidePadding allocationTime-exportOptionWrap" style="margin-top: 14px">
               
                <div id="exportOption" runat="server" class="rmmExport-optionBtnWrap">
                    <div class="ExportOption-btns displayHide" style="padding-right:5px;">
                        <dx:ASPxButton ID="imgNewGanttAllocation" runat="server" AutoPostBack="false" EnableTheming="false" ToolTip="Allocation Gantt"
                            Image-Url="/Content/Images/ganttBlackNew.png" RenderMode="Link" Image-Height="20" Image-Width="20">
                            <ClientSideEvents Click="OpenNewAllocationGantt" />
                        </dx:ASPxButton>
                    </div>
                    <div class="ExportOption-btns displayHide">
                        <asp:Image ID="btnSetTypeColor" runat="server" Width="18" ToolTip="Set Color" CssClass="RMMBtn-setColor" ImageUrl="~/Content/Images/setColor.png" />
                    </div>
                    <div class="ExportOption-btns displayHide">
                        <%--<dx:ASPxButton ID="btnExcelExport" ClientInstanceName="btnExcelExport" runat="server" EnableTheming="false" UseSubmitBehavior="False"
                            OnClick="btnExcelExport_Click" RenderMode="Link" ToolTip="Export to Excel">
                            <Image>
                                <SpriteProperties CssClass="excelicon" />
                            </Image>
                            <ClientSideEvents Click="function(s, e) { LoaderOnExport(); _spFormOnSubmitCalled=false; }" />
                        </dx:ASPxButton>--%>
                            <dx:ASPxMenu ID="mnuExportOptions" runat="server" AllowSelectItem="false"
                            OnItemClick="mnuExportOptions_ItemClick"
                                ShowPopOutImages="True" BackColor="White" Border-BorderColor="White" SubMenuItemStyle-DropDownButtonStyle-Paddings-PaddingTop="40px" BorderLeft-BorderStyle="None" HorizontalPopOutImage-IconID="none">
                            <Items>
                                <dx:MenuItem ItemStyle-BorderRight-BorderStyle="None" Text="" ItemStyle-DropDownButtonStyle-Paddings-PaddingTop="40px" ItemStyle-Height="0px" ItemStyle-BackColor="White" ItemStyle-Width="39px" ItemStyle-Border-BorderColor="White">
                                    <Items>
                                        <dx:MenuItem Text="Excel" ToolTip="Export to Excel">
                                            <SubMenuItemStyle Width="242px" Paddings-PaddingTop="20px"></SubMenuItemStyle>
                                            <Image>
                                                <SpriteProperties CssClass="excelicon"/>
                                            </Image>
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="CSV" ToolTip="Export to CSV">
                                            <Image>
                                                <SpriteProperties CssClass="csvicon"/>
                                            </Image>
                                        </dx:MenuItem>
                                    </Items>
                                    <Image>
                                        <SpriteProperties CssClass="excelicon"/>
                                    </Image>
                                </dx:MenuItem>
                            </Items>
                            <ClientSideEvents ItemClick="function(s, e) { LoaderOnExport(); _spFormOnSubmitCalled=false; }" />
                        </dx:ASPxMenu>
                    </div>
                    <div class="ExportOption-btns displayHide">
                        <dx:ASPxButton ID="btnPdfExport" ClientInstanceName="btnPdfExport" runat="server" CssClass="export-buttons" EnableTheming="false" UseSubmitBehavior="False"
                            RenderMode="Link" OnClick="btnPdfExport_Click" ToolTip="Export to Pdf">
                            <Image>
                                <SpriteProperties CssClass="pdf-icon" />
                            </Image>
                            <ClientSideEvents Click="function(s, e) { LoaderOnExport(); _spFormOnSubmitCalled=false; }" />
                        </dx:ASPxButton>
                    </div>

                    <div class="ExportOption-btns displayHide">
                        <asp:ImageButton AlternateText="Print" ID="imgPrint" ImageUrl="~/content/images/print-icon.png" runat="server" OnClick="imgPrint_Click" OnClientClick="LoaderOnPrint()" CssClass="resource-printImg" ToolTip="Print" />
                    </div>
                    <div class="reportPanelImage">
                        <dx:ASPxButton ID="imgReport" runat="server" RenderMode="Link" AutoPostBack="false" Image-Width="25" Image-Height="25"
                            Image-Url="~/Content/Images/reports-Black.png">

                        </dx:ASPxButton>                      
                    </div>
                </div>

            </div>
            
        </div>
        <dx:ASPxCallbackPanel ID="cp" runat="server" ClientInstanceName="cp" OnCallback="cp_Callback">
            <PanelCollection>
                <dx:PanelContent>
                    <div class="row" id="divGridTypeFilter" style="padding-top: 5px; padding-bottom: 7px;">
                        <div class="col-md-10 noSidePadding">
                            <div class="row">
                                <div class="col-md-3 col-sm-3 col-xs-12">
                                    <div id="divExpandCollapse" runat="server" class="resourceAllo-gridBtn">
                                        <div style="filter:brightness(0)">
                                            <dx:ASPxButton ID="btnExpandAll" runat="server" AutoPostBack="False" RenderMode="Link">
                                                <Image Url="/content/images/expand-all-new.png" Width="22px"></Image>
                                                <ClientSideEvents Click="function(s, e) { grid.ExpandAll();}" />
                                            </dx:ASPxButton>

                                            <dx:ASPxButton ID="btnCollapseAll" runat="server" AutoPostBack="False" RenderMode="Link">
                                                <Image Url="/content/images/collapse-all-new.png" Width="22px"></Image>
                                                <ClientSideEvents Click="function(s, e) {grid.CollapseAll();}" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                    <div id="dvZoomLevel" runat="server" style="padding-top: 2px;filter:brightness(0)">
                                        <dx:ASPxButton ID="btnZoomIn" runat="server" AutoPostBack="false" ClientInstanceName="btnZoomIn" RenderMode="Link" OnClick="btnZoomIn_Click">
                                            <Image Url="/content/Images/zoom-inBlue.png" Width="22px"></Image>
                                            <ClientSideEvents Click="function(s, e) {GZoomIn();}" />
                                        </dx:ASPxButton>

                                        <dx:ASPxButton ID="btnZoomOut" runat="server" AutoPostBack="true" RenderMode="Link" OnClick="btnZoomOut_Click">
                                            <Image Url="/content/Images/zoom-outBlue.png" Width="22px"></Image>
                                            <ClientSideEvents Click="function(s, e) {GZoomOut();}" />
                                        </dx:ASPxButton>
                                        <asp:Button runat="server" ID="bZoomIn" OnClick="bZoomIn_Click" Text="click btn" Style="display: none" />
                                        <asp:Button runat="server" ID="bZoomOut" OnClick="bZoomOut_Click" Text="click btn" Style="display: none" />
                                        
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-3 col-xs-12">
                                    <div class="resourceAllo-gridchkBox">
                                        <div class="crm-checkWrap">
                                            <asp:CheckBox ID="chkIncludeClosed" Text="Include Closed Projects" runat="server" AutoPostBack="true"
                                                OnCheckedChanged="chkIncludeClosed_CheckedChanged" TextAlign="Right" CssClass="checkboxbutton RMM-checkWrap" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-3 col-xs-12">
                                    <div class=" bC-radioBtnWrap">
                                        <asp:RadioButton ID="rbtnFTE" runat="server" AutoPostBack="false" Text="FTE" CssClass="radiobutton importChk-radioBtn" onchange="reloadGrid()" GroupName="BarPercentage" />
                                        <asp:RadioButton ID="rbtnPercentage" runat="server" AutoPostBack="false" Text="%" CssClass="radiobutton importChk-radioBtn" onchange="reloadGrid()" GroupName="BarPercentage" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2 noSidePadding">
                            <div id="dvYearNavigation" runat="server" class="dvYearNavigation">
                                <span style="padding-right: 5px;">
                                    <asp:ImageButton ImageUrl="/content/images/back-arrowBlue.png" ID="previousYear" ToolTip="Prevoius" runat="server" OnClientClick="prev()"
                                        OnClick="previousYear_Click" CssClass="resource-img" />
                                </span>
                                <asp:Label ID="lblSelectedYear" runat="server" Style=""></asp:Label>
                                <span style="padding-left: 5px;">
                                    <asp:ImageButton ImageUrl="/content/images/next-arrowBlue.png" ID="nextYear" ToolTip="Next" runat="server" OnClientClick="next()"
                                        OnClick="nextYear_Click" CssClass="resource-img" />
                                </span>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <dx:ASPxGridView ID="gvPreview" runat="server" AutoGenerateColumns="false" CssClass="homeGrid" KeyFieldName="WorkItemID"
                                Width="100%" OnDataBinding="gvPreview_DataBinding" ClientInstanceName="grid" EnableCallBacks="true" SettingsBehavior-SortMode="Custom"
                                OnCustomSummaryCalculate="gvPreview_CustomSummaryCalculate" ClientVisible="true"
                                OnBatchUpdate="gvPreview_BatchUpdate"
                                OnInit="gvPreview_Init"
                                OnRowValidating="gvPreview_RowValidating"
                                OnHtmlRowPrepared="gvPreview_HtmlRowPrepared"
                                OnClientLayout="gvPreview_ClientLayout"
                                OnHtmlDataCellPrepared="gvPreview_HtmlDataCellPrepared"
                                OnHtmlRowCreated="gvPreview_HtmlRowCreated"
                                OnCustomCallback="gvPreview_CustomCallback"
                                OnCustomColumnSort="gvPreview_CustomColumnSort"
                                OnCellEditorInitialize="gvPreview_CellEditorInitialize">
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

                                <ClientSideEvents Init="Grid_Init" EndCallback="Grid_EndCallback" ColumnResized="Grid_ColumnResized" BatchEditStartEditing="Grid_BatchEditStartEditing" />
                                <Styles>
                                    <Row CssClass="estReport-dataRow"></Row>
                                    <Header CssClass="gridHeader RMM-resourceUti-gridHeaderRow" />
                                    <GroupPanel CssClass="reportGrid-groupPannel"></GroupPanel>
                                    <GroupRow CssClass="gridGroupRow estReport-gridGroupRow" />
                                    <GroupFooter CssClass="estReport-groupFooterRow"></GroupFooter>
                                    <Footer Font-Bold="true" HorizontalAlign="Center" Border-BorderColor="#D9DAE0" Border-BorderStyle="Solid" Border-BorderWidth="1px"
                                        BorderRight-BorderWidth=".5px" CssClass="resourceUti-gridFooterRow">
                                    </Footer>
                                    <Table CssClass="timeline_gridview"></Table>
                                    <Cell Wrap="True"></Cell>
                                </Styles>
                                <SettingsDataSecurity AllowDelete="false" />
                                <SettingsBehavior AllowGroup="true" AutoExpandAllGroups="true" />
                                <SettingsLoadingPanel Mode="Disabled" />
                                <SettingsPager PageSizeItemSettings-ShowAllItem="true">
                                </SettingsPager>
                                <Settings GroupFormat="{1}" ShowFooter="true" ShowStatusBar="Visible"
                                    VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto" />
                            </dx:ASPxGridView>

                            <dx:ASPxGridViewExporter ID="gridExporter" runat="server" GridViewID="gvPreview" OnRenderBrick="gridExporter_RenderBrick"></dx:ASPxGridViewExporter>
                        </div>
                    </div>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxCallbackPanel>
    </div>

    <dx:ASPxPopupControl ClientInstanceName="pcSetColor" Modal="true"
        PopupElementID="btnSetTypeColor" ID="pcSetColor" Width="600px" Height="400px"
        ShowFooter="false" ShowHeader="true" CssClass="aspxPopup" HeaderText="Set Color"
        runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="Middle" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl13" runat="server">

                <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                    <div class="row">
                        <dx:ASPxGridView ID="grdColor" runat="server" AutoGenerateColumns="false" CssClass="customgridview homeGrid"
                            Width="100%" ClientInstanceName="grdColor" OnHtmlDataCellPrepared="grdColor_HtmlDataCellPrepared"
                            EnableViewState="false"
                            EnableRowsCache="false" KeyFieldName="ModuleName">
                            <SettingsCookies Enabled="false" />
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="Type" PropertiesTextEdit-EncodeHtml="true" FieldName="LevelTitle" Width="200px" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left"></dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="EstimatedColor" ShowInCustomizationForm="True" VisibleIndex="2" Caption="Estimate Color">
                                    <DataItemTemplate>
                                        <dx:ASPxColorEdit ID="WebEstimatedColorEditor" CssClass="aspxColorEdit-gridDropDown" runat="server" Text='<%# GetColor(Eval("EstimatedColor")) %>' EnableCustomColors="true"></dx:ASPxColorEdit>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="AssignedColor" ShowInCustomizationForm="True" VisibleIndex="3" Caption="Assigned Color">
                                    <DataItemTemplate>
                                        <dx:ASPxColorEdit ID="WebAssignedColorEditor" CssClass="aspxColorEdit-gridDropDown" runat="server" Text='<%# GetColor(Eval("AssignedColor")) %>' EnableCustomColors="true"></dx:ASPxColorEdit>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <SettingsBehavior AllowGroup="true" AutoExpandAllGroups="true" />
                            <SettingsPager PageSizeItemSettings-ShowAllItem="true"></SettingsPager>
                            <Styles>
                                <Row CssClass="homeGrid_dataRow"></Row>
                                <Header CssClass="homeGrid_headerColumn"></Header>
                                <GroupRow CssClass="homeGrid-groupRow" Font-Bold="true"></GroupRow>
                                <SelectedRow BackColor="#DBEAF9"></SelectedRow>
                            </Styles>
                        </dx:ASPxGridView>
                    </div>
                    <div class="row" style="padding-top: 15px;">
                        <span><b>Note: Changes apply to all users.</b></span>
                    </div>
                    <div class="row addEditPopup-btnWrap">
                        <dx:ASPxButton CssClass="secondary-cancelBtn" runat="server" ID="Cancel" Text="Cancel" ToolTip="Cancel">
                            <ClientSideEvents Click="function(s, e){pcSetColor.Hide();}" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btnSaveColor2" runat="server" ValidationGroup="ctr" OnClick="btnSaveColor_Click"
                            ToolTip="Save" Text="Save" CssClass="primary-blueBtn">
                        </dx:ASPxButton>
                    </div>

                </div>
                <%-- <div style="width: 100%">
                <div style="float: left;">
                    
                </div>
                <div class="col-md-12 col-sm-12 col-xs-12 noPadding setColor-btnContainer">
                    <div class="rmm-setColor-btnWrap">
                        <div class="setColor-saveBtn">
                            <asp:Button  CssClass="" ValidationGroup="ctr" runat="server"   />
                        </div>
                        <div class="setColor-closeBtn">
                            <input type="button" value="Close" class=""  />
                        </div>
                    </div>
                </div>
            </div>--%>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
