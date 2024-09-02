<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserResume.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMONE.UserResume" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.4/jspdf.min.js"></script>
<%--<script src="js/jsPDF/dist/jspdf.min.js"></script>--%>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .divUserHistoryList .dx-list-group-header {
        /*margin-bottom: 9px;*/
    }

    .divUserHistoryList {
        border: 1px solid #ddd;
    }

    .listcontainer {
        margin-top: 25px;
    }

    .datesdiv {
        color: #6e6e6e;
        display: inline-block;
        float: right;
        margin-bottom: 15px;
    }

    .dateWrap {
        display: inline-block;
        margin-right: 15px;
    }

    .divTicketId {
        font-size: 12px;
        background: #4fa1d6;
        color: white;
        padding: 5px 10px;
        border-radius: 15px;
        margin-left: 8px;
        display: inline-block;
        clear: both;
        font-weight: 500;
    }

    #divIncludeCurrent {
        font-family: 'Roboto', sans-serif;
        font-size: 13px;
        color: black;
        margin-top: 20px;
        padding-left: 15px;
    }

    .divTitle {
        font-size: 13px;
        font-weight: 600;
        display: inline-block;
        margin-bottom: 15px;
        margin-left: 12px;
    }

    .dataDescription {
        word-break: break-all;
        width: 90%;
        white-space: normal;
        margin-top: 7px;
        margin-bottom: 7px;
        color: #6e6e6e;
    }

    .dataTitle {
        color: gray;
        font-size: 13px;
    }

    .data-value {
        color: black;
        font-size: 13px;
        font-weight: 400;
        margin: 7px 0;
    }

    .userLabel {
        font-size: 13px;
        color: #4A6EE2;
    }

    #divIncludeCurrent .dx-checkbox-container .dx-checkbox-icon {
        width: 18px;
        height: 18px;
        border-radius: 2px;
        border: 1px solid black;
    }

    #divIncludeCurrent .dx-checkbox-container .dx-checkbox-text {
        /*margin-left: -17px;*/
    }

    .dx-checkbox-checked .dx-checkbox-icon {
        font: 13px/13px DXIcons;
        color: #4A6EE2;
        text-align: center;
    }

        .dx-checkbox-checked .dx-checkbox-icon:before {
            margin-top: -7px;
        }

    .buildAction {
        float: right;
        margin-top: 19px;
    }

    .dx-list-group-header {
        font-weight: 500;
        padding: 10px 10px 10px;
        border-bottom: 2px solid #ddd;
        background: rgba(238,238,238,.05);
        color: #333;
    }

    #scrollContent {
        overflow-x: hidden !important;
    }

    .marginLeft {
        margin-left: 30px !important;
    }

    .dx-list-group-header::before {
        margin-top: 65px;
        margin-right: 10px;
    }

    .inline-grid-data {
        display: inline-grid;
        margin-right: 1%;
    }

    .detailsOuterDiv {
        display: flex;
        text-align: center;
        justify-content: flex-end;
    }

    #divUserHistoryList .accordion > input[type="checkbox"] {
        position: absolute;
        left: -100vw;
    }

    #divUserHistoryList .accordion .content {
        overflow-y: hidden;
        height: 0;
        transition: height 0.3s ease;
        white-space: pre-line;
    }

    #divUserHistoryList .accordion > input[type="checkbox"]:checked ~ .content {
        height: auto;
        overflow: visible;
        padding-left: 15px;
    }

    #divUserHistoryList .accordion {
        margin-bottom: 1em;
    }

        #divUserHistoryList .accordion .handle {
            margin: 0;
            font-size: 1.125em;
            line-height: 1.2em;
        }

        #divUserHistoryList .accordion label {
            color: #333;
            cursor: pointer;
            font-size: 14px;
            height: 20px;
            text-align: center;
            font-weight: 600;
            padding-top: 14px;
            display: flex;
            flex-wrap: wrap;
            justify-content: center;
        }

        #divUserHistoryList .accordion .handle label:after {
            font-family: 'Glyphicons Halflings';
            content: "\e114";
            display: inline-block;
            margin-left: 10px;
            font-size: 16px;
        }

        #divUserHistoryList .accordion > input[type="checkbox"]:checked ~ .handle label:after {
            content: "\e113";
        }

    .dx-list:not(.dx-list-select-decorator-enabled) .dx-list-item.dx-state-active {
        background-color: rgba(0,0,0,.04);
        color: #333;
    }

    .whiteSpace {
        white-space: pre-line;
        font-weight: 500;
    }

    .data-content-hide::before {
        display: none;
    }

    .borderText {
        display: flex;
        margin: -10px -6px -15px 44px;
        font-size: 14px;
        float: left;
        color: black;
        background: white;
    }

    .dx-widget.dx-dropdownbutton {
        border: none;
        margin-right: 0px;
    }

    .dx-button-has-icon .dx-icon {
        width: 23px;
        height: 23px;
    }

    .userImageStyle {
        width: 90px;
        height: 90px;
        border-radius: 50%
    }

    .display-flex-center {
        display: flex;
        align-items: center;
    }

    .userLabelNew {
        font-size: 16px;
        font-weight: 500;
    }

    .inner-container {
        display: flex;
        flex-direction: column;
        justify-content: center;
    }

    .dashboard-panel-new {
        border: none !important;
        padding-top: 6px;
        padding-bottom: 6px
    }
    .dashboard-panel-main, .dashboard-panel-main-mini, .dashboard-panel-main-notmove:not(.panelDashboard) {
        width: 100%;
        background-color: #FFFFFF;
        border-radius: 0px;
        box-shadow: 0px 1px 2px 3px #ddd;
        height: 100%;
    }

    .dx-list-collapsible-groups .dx-list-group-header::before {
        content: none !important;
    }
    .projectLabel {
        font-size: 14px;
    }

    .allocation-bar-outline {
        width: 100%;
        outline: 1px lightgrey solid;
        border-radius: 5px;
        outline-offset: -2px;
    }

    .emptyProgressBar {
        background-color: #ededed;
        height: 30px;
        border: 1px solid #ededed;
        background-size: 100% 100%;
        color: white;
        overflow: hidden;
    }

    .progressbarhold {
        background: #F93E6A url(/content/images/AgeRectRed.png) no-repeat;
        height: 30px !important;
        font-size: 14px !important;
        background-size: 100% 100%;
        color: white !important;
    }

    .progressbar {
    background: #6ba538;
    height: 30px !important;
    font-size: 14px !important;
    background-size: 100% 100%;
    color: black;
}

    .allocation-bar-outline strong {
        position: absolute;
        font-size: 12px !important;
        left: 2px;
        width: 100%;
        text-align: center;
        top: 7px !important;
        z-index: 1;
        color: #FFF;
    }

    .dx-widget.dx-dropdownbutton .dx-item.dx-buttongroup-item {
        /* color: #4A6EE2; */
        font-family: 'Roboto', sans-serif !important;
        text-align: left;
        padding: 0px 0px 13px 3px;
        font-size: 12px;
        border: none;
    }
    .dx-overlay-content .dx-loadpanel-content-wrapper  {
        font-size: 18px !important;
        font-weight: 500;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var IsFilterExpanded = false;
    var UserAdvancedData = null;
    var UserDropdown = '';
    var userdxList = '';
    var selectedUser = [];
    var strSelectedUserId = '<%=UserID%>';
    var projectTypes = [];
    var listOfCompany = [];
    var listOfRoles = [];
    var listComplexity = [];
    var includeCurrentProject = true;
    var actionItems = [{ id: 1, name: "Pdf", icon: "/Content/images/icons/pdf-icon.png" }/*, { id: 2, name: "Email", icon: "/Content/images/icons/email-icon-white.png" }*/, { id: 3, name: "Print", icon: "/Content/images/icons/print-icon.png" }];
    var selectedValues = { Company: '', ProjectType: '', Complexity: '', FromDate: '', ToDate: '', Role: '' };

    function treatAsUTC(date) {
        var result = new Date(date);
        result.setMinutes(result.getMinutes() - result.getTimezoneOffset());
        return result;
    }

    function daysBetween(startDate, endDate) {
        if (startDate == "" || endDate == "") {
            return 0;
        }
        var millisecondsPerDay = 24 * 60 * 60 * 1000;
        return Math.round((treatAsUTC(endDate) - treatAsUTC(startDate)) / millisecondsPerDay/7);
    }

    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {


        var ticketid = params.split('=')[1];


       <%--//set_cookie('UseManageStateCookies', 'true', null, "<%= SPContext.Current.Web.ServerRelativeUrl %>");--%>
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }

    function formatDate(date) {
        if (date != null) {
            var d = new Date(date),
                month = '' + (d.getMonth() + 1),
                day = '' + d.getDate(),
                year = d.getFullYear();

            if (month.length < 2)
                month = '0' + month;
            if (day.length < 2)
                day = '0' + day;

            return [month, day, year].join('/');
        }
        return '';
    }

    function ShowFilter() {
        if (!$("#filterContent").is(":visible")) {
            $("#filterContent").show();
            $("#imgAdvanceMode").attr("src", "/Content/Images/red-filter.png");
            IsFilterExpanded = true;
        }
        else {
            $("#filterContent").hide();
            $("#imgAdvanceMode").attr("src", "/Content/Images/Newfilter.png");
            IsFilterExpanded = false;
        }
    }

    function PrintResume() {
        html2canvas($("#divUserHistoryList"), {
            onrendered: function (canvas) {
                let myImage = canvas.toDataURL("image/png");
                window.print(myImage);
            }
        });
    }
    
    function SavePdf() {
        var element = $("#divUserHistoryList"); // global variable
        html2canvas(element, {
            onrendered: function (canvas) {
                var imgData = canvas.toDataURL('image/png');
                var doc = new jspdf({ format: [2899.37, 3976.54], unit: 'mm', orientation: 'p', putOnlyUsedFonts: true }); //new jsPDF('l', 'cm', [1000,1000]); //210mm wide and 297mm high
                doc.addImage(imgData, 'PNG', 10, 10);
                doc.save('BuildProfile.pdf');
            }
        });
    }

    function GetData(data) {
        jQuery.each(data, function (i, val) {
            jQuery.each(val.items, function (i, value) {
                if (!projectTypes.includes(value.ProjectType) && value.ProjectType != null && value.ProjectType != '') {
                    projectTypes.push(value.ProjectType);
                }
                if (!listOfCompany.includes(value.CompanyName) && value.CompanyName != null && value.CompanyName != '') {
                    listOfCompany.push(value.CompanyName);
                }
                if (!listOfRoles.includes(value.RoleName) && value.RoleName != null && value.RoleName != '') {
                    listOfRoles.push(value.RoleName);
                }
                if (!listComplexity.includes(value.ProjectComplexityChoice) && value.ProjectComplexityChoice != null && value.ProjectComplexityChoice != '') {
                    listComplexity.push(value.ProjectComplexityChoice);
                }
            });
        });
        projectTypes = projectTypes.sort();
        listOfCompany = listOfCompany.sort();
        listOfRoles = listOfRoles.sort();
        listComplexity = listComplexity.sort();
    }
    function GetUsersHistoryProfile() {
        
        $.get("/api/buildprofile/GetUsersProfileForComparison?IncludeCurrentProject="
            + includeCurrentProject + "&selectedUserId=" + strSelectedUserId + "&selectedCompany=" + encodeURIComponent(selectedValues.Company)
            + "&selectedRole=" + selectedValues.Role + "&selectedType=" + encodeURIComponent(selectedValues.ProjectType)
            + "&selectedComplexity=" + selectedValues.Complexity + "&fromDate=" + selectedValues.FromDate
            + "&toDate=" + selectedValues.ToDate + "&selectedModule=CPR", function (data, status) {
                if (data) {
                    userdxList.option('dataSource', data);
                }
                else {
                    data = [];
                    userdxList.option('dataSource', data);
                }
                $("#divUserHistoryList .dx-list-group-body").addClass("dashboard-panel-new noPadding ");
                $("#divUserHistoryList .dx-list-item").addClass("dashboard-panel-main");
                $(".dx-list-group-header").addClass("noPadding");
                if (IsFilterExpanded) {
                    ShowFilter();
                }
            });

    }

    function SavePdfToServer() {
        html2canvas($("#divUserHistoryList"), {
            onrendered: function (canvas) {
                let image = canvas.toDataURL("image/png");
                image = image.replace('data:image/png;base64,', '');
                $.ajax({
                    url: "/api/buildprofile/SavePdfToServer",
                    data: JSON.stringify(image),
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                });
            }
        });
    }


    <%--function SendEmailToUser() {
        let userlist = $("#divUsersDropdown").dxTagBox("instance").option("selectedItems");
        let userListText = '';

        userlist.forEach(function (row, index, arr) {
            userListText = row.AssignedToName + ', ' + userListText;
        });

        let moduleName = '<%=ModuleName%>';
        let subject = "Biography of " + userListText;
        let body = "Attached the biography of " + userListText
        let emailLink = '<%=ResumeManualEscalationUrl%>';

        params = "sendresume=1&usersubject=" + subject + "&userbody=" + body + "&ModuleName=" + moduleName + "&ids=" + "<%=ProjectID%>";
        window.parent.UgitOpenPopupDialog(emailLink, params, "Send Email ", "700px", "700px", false, escape(window.location.href));
    }--%>

    $(document).ready(function () {
        $("#loadpanel").dxLoadPanel({
            message: "Loading...",
            visible: true,
            showPane: false,
        });
        $.get("/api/buildprofile/GetUsersProfileForComparison?IncludeCurrentProject="
            + includeCurrentProject + "&selectedUserId=" + strSelectedUserId + "&selectedCompany=" + encodeURIComponent(selectedValues.Company)
            + "&selectedRole=" + selectedValues.Role + "&selectedType=" + encodeURIComponent(selectedValues.ProjectType)
            + "&selectedComplexity=" + selectedValues.Complexity + "&fromDate=" + selectedValues.FromDate
            + "&toDate=" + selectedValues.ToDate + "&selectedModule=CPR", function (data, status) {
                if (data) {
                    $.get("/api/rmmcard/GetManager?hdnChildOf=" + strSelectedUserId + "&hdnParentOf=&Year=" + new Date().getFullYear(), function (advancedData, status) {
                        if (advancedData) {
                            UserAdvancedData = advancedData;
                            userdxList.option('dataSource', data);
                        }
                        $("#divUserHistoryList .dx-list-group-body").addClass("dashboard-panel-new noPadding ");
                        $("#divUserHistoryList .dx-list-item").addClass("dashboard-panel-main");
                        $(".dx-list-group-header").addClass("noPadding");
                        if (IsFilterExpanded) {
                            ShowFilter();
                        }
                        $("#loadpanel").dxLoadPanel("hide");
                    });
                }
                else {
                    data = [];
                    userdxList.option('dataSource', data);
                    $("#loadpanel").dxLoadPanel("hide");
                }
            });


        $.get("/api/buildprofile/GetUsersProfileForComparison?IncludeCurrentProject=true&selectedUserId=" + strSelectedUserId + "&selectedModule=CPR", function (data, status) {
            if (data) {
                GetData(data);
            }
        });


    });

    $(function () {
        userdxList = $("#divUserHistoryList").dxList({
            // dataSource: employees,
            height: "100%",
            grouped: true,
            collapsibleGroups: false,
            noDataText:'',
            showScrollbar: 'never',
            groupTemplate: function (data) {
                let container = $('<div></div>');
                let firstRow = $('<div class="row" style="display:flex;"></div>').append(
                    $('<div class="col-md-6 noPadding dashboard-panel-new"></div>').append(
                        $('<div class="dashboard-panel-main p-2"></div>').append(
                            $(`<div class="display-flex-center">
                                            <div>
                                                <img id="userDisplayImage" class="userImageStyle" alt="User Photo" src="${data.Picture}">
                                            </div>
                                            <div class="userLabelNew ml-3">
                                                <div class="m-1" style="font-size:18px;font-weight:500;">${data.UserName}</div>
                                                <div class="m-1">${data.UserGlobalRole}</div>
                                                <div class="m-1 display-flex-center" style="width:150%;">
                                                    <div class="mr-2 allocation-bar-outline">${UserAdvancedData.AllocationBar}</div>
                                                </div>
                                            </div>
                                        </div>`),

                            $(`<div class="display-flex-center userLabelNew mt-2">
                                            <div class="projectStyle ml-2 mr-4"># Direct Reports: ${UserAdvancedData.AssitantCount}</div>
                                            <div class="projectStyle ml-2"># Allocations: ${UserAdvancedData.ProjectCount} </div>
                                        </div>`)
                        )
                    ),
                    $('<div class="col-md-6 noPadding dashboard-panel-new"></div>').append(
                        $(`<div class="dashboard-panel-main p-2 inner-container"><div class="mr-2 ml-2 mt-2 mb-3 "><span class="dataTitle userLabelNew">Division: </span><span class="whiteSpace userLabelNew">${data.Division}</span></div>
                                        <div class="mr-2 ml-2 mb-3"><span class="dataTitle userLabelNew">Office: </span><span class="whiteSpace userLabelNew">${data.Location}</span></div>
                                        <div class="mr-2 ml-2 mb-3"><span class="dataTitle userLabelNew">Roles: </span><span class="whiteSpace userLabelNew">${data.Roles != "" && data.Roles != null ? data.Roles.slice(0, -2) : ""}</span></div></div>`)
                    )
                )
                container.append(firstRow);


                let secondRow = $(`${data.UserDatailsControl}`);
                container.append(secondRow);

                let thirdRow = $(`<div class=" headercontainer noPadding dashboard-panel-new">`).append(
                    $(`<div class="row dashboard-panel-main">`).append(
                        $('<div class="col-md-6 noPadding" style="margin-top: 20px;">').append(
                            $(`<div id="divFilter">`).append(
                                $(`<span class="projectLabel mr-2 ml-3" style="font-size:16px;">Project Resume: ${data.items.length}</span>`),
                                `<img src="/Content/Images/Newfilter.png" id="imgAdvanceMode" onclick="ShowFilter()" style="width: 18px;" title="Advanced Filter">`
                            ),
                        ),
                        $(`<div class="col-md-offset-3 col-md-3">`).append(
                            $(`<div id="divActionItem" class="buildAction">`).dxDropDownButton({
                                icon: "/Content/images/export-Black.png",
                                showArrowIcon: false,
                                dropDownOptions: {
                                    width: 100
                                },
                                hint: "Export",
                                displayExpr: "name",
                                onItemClick: function (e) {
                                    if (e.itemData.name == "Pdf") {
                                        SavePdf();
                                    }
                                    if (e.itemData.name == "Email") {
                                        SavePdfToServer();
                                        SendEmailToUser();
                                    }
                                    if (e.itemData.name == "Print") {
                                        PrintResume();
                                    }
                                },
                                items: actionItems
                            })
                        )
                    )
                )
                container.append(thirdRow);

                let forthRow = $(`<div id="filterContent" style="display: none; border: 2px solid #ddd; margin: 17px;">`).append(
                    $(`<div class="borderText whiteSpace">Advanced Filter</div>`),
                    $(`<div class="row mt-4">`).append(
                        $(`<div class="col-md-4">`).append(
                            $(`<label class="userLabel">Company</label>`),
                            $(`<div id="divCompanyDropdown">`).dxTagBox({
                                items: listOfCompany,
                                showSelectionControls: true,
                                applyValueMode: "useButtons",
                                searchEnabled: true,
                                value: selectedValues.Company != "" ? selectedValues.Company.split(';').filter(n => n) : [],
                                onValueChanged: function (e) {
                                    selectedValues.Company = '';
                                    e.value.forEach(function (row, index, arr) {
                                        selectedValues.Company = row + ';' + selectedValues.Company;
                                    });
                                    GetUsersHistoryProfile();
                                }
                            })
                        ),
                        $(`<div class="col-md-4">`).append(
                            $(`<label class="userLabel">Type</label>`),
                            $(`<div id="divTypeDropdown">`).dxTagBox({
                                items: projectTypes,
                                showSelectionControls: true,
                                applyValueMode: "useButtons",
                                searchEnabled: true,
                                value: selectedValues.ProjectType != "" ? selectedValues.ProjectType.split(';').filter(n => n) : [],
                                onValueChanged: function (e) {
                                    selectedValues.ProjectType = '';
                                    e.value.forEach(function (row, index, arr) {
                                        selectedValues.ProjectType = row + ';' + selectedValues.ProjectType;
                                    });
                                    GetUsersHistoryProfile();
                                }
                            })
                        ),
                        $(`<div class="col-md-4">`).append(
                            $(`<label class="userLabel">Complexity</label>`),
                            $(`<div id="divComplexityDropdown">`).dxTagBox({
                                items: listComplexity,
                                showSelectionControls: true,
                                applyValueMode: "useButtons",
                                searchEnabled: true,
                                value: selectedValues.Complexity != "" ? selectedValues.Complexity.split(';').filter(n => n) : [],
                                onValueChanged: function (e) {
                                    selectedValues.Complexity = '';
                                    e.value.forEach(function (row, index, arr) {
                                        selectedValues.Complexity = row + ';' + selectedValues.Complexity;
                                    });
                                    GetUsersHistoryProfile();
                                }
                            })
                        ),
                    ),
                    $(`<div class="row mt-2 mb-4">`).append(
                        $(`<div class="col-md-4">`).append(
                            $(`<label class="userLabel">From Date</label>`),
                            $(`<div id="fromdate">`).dxDateBox({
                                type: 'date',
                                value: selectedValues.FromDate,
                                onValueChanged: function (e) {
                                    selectedValues.FromDate = '';
                                    selectedValues.FromDate = formatDate(e.value);
                                    if (!(e.value == null && e.previousValue == '')) {
                                        GetUsersHistoryProfile();
                                    }
                                }
                            })
                        ),
                        $(`<div class="col-md-4">`).append(
                            $(`<label class="userLabel">To Date</label>`),
                            $(`<div id="todate">`).dxDateBox({
                                type: 'date',
                                value: selectedValues.ToDate,
                                onValueChanged: function (e) {
                                    selectedValues.ToDate = '';
                                    selectedValues.ToDate = formatDate(e.value);
                                    if (!(e.value == null && e.previousValue == '')) {
                                        GetUsersHistoryProfile();
                                    }
                                }
                            })
                        ),
                        $(`<div class="col-md-4">`).append(
                            $(`<label class="userLabel">Role</label>`),
                            $(`<div id="divRoleDropdown">`).dxTagBox({
                                items: listOfRoles,
                                showSelectionControls: true,
                                applyValueMode: "useButtons",
                                searchEnabled: true,
                                value: selectedValues.Role != "" ? selectedValues.Role.split(';').filter(n => n) : [],
                                onValueChanged: function (e) {
                                    selectedValues.Role = '';
                                    e.value.forEach(function (row, index, arr) {
                                        selectedValues.Role = row + ';' + selectedValues.Role;
                                    });
                                    GetUsersHistoryProfile();
                                }
                            })
                        ),
                    )
                )
                container.append(forthRow);

                return container;
            },
            itemTemplate: function (data, index) {
                var container = $('<div></div>');
                var title = $(`<div class='divTitle'>${data.ProjectTitle}</div>`);
                var TicketId = $(`<div class='divTicketId' onclick="${data.Url}">${data.TicketId}</div>`);
                var secondRow = $(`<div class='row' style='text-align:center;'>
                                    <div class='col-md-2 data-value inline-grid-data'><div class='dataTitle'> Role: </div><div class='whiteSpace'> ${data.RoleName}</div> </div>
                                    <div class='col-md-2 data-value inline-grid-data'><div class='dataTitle'> Company  </div><div class='whiteSpace'> ${data.CompanyName}</div></div>
                                    <div class='col-md-1 data-value inline-grid-data'><div class='dataTitle'>Contract value </div><div class='whiteSpace'> ${data.ContractValue}</div></div>
                                    <div class='col-md-2 data-value inline-grid-data'><div class='dataTitle'>Type  </div> <div class='whiteSpace'> ${data.ProjectType}</div></div>
                                    <div class='col-md-1 data-value inline-grid-data'><div class='dataTitle'>Length (wks) </div><div class='whiteSpace'> ${daysBetween(data.PreconStartDate, data.PreconEndDate)}</div></div>
                                    <div class='col-md-1 data-value inline-grid-data'><div class='dataTitle'>Start Date  </div><div class='whiteSpace'> ${data.PreconStartDate}</div></div>
                                    <div class='col-md-1 data-value inline-grid-data'><div class='dataTitle'>End Date  </div><div class='whiteSpace'> ${data.PreconEndDate}</div></div>
                                    <div class='col-md-1 data-value inline-grid-data'><div class='dataTitle'>Complexity  </div><div class='whiteSpace'> ${data.ProjectComplexityChoice}</div></div>
                                </div>`);
                var description = $(`<section class="accordion">
                                <input type="checkbox" name="collapse" id="${data.TicketId}">
                                <h2 class="handle">
                                    <label for="${data.TicketId}">Job Description</label>
                                </h2>
                                <div class="content">
                                ${data.Description}
                                </div>
                                </section>
                                `);
                container.append(title);
                container.append(TicketId);
                container.append(secondRow);
                if (data.Description != '') {
                    container.append(description);
                }
                if (data.IsEmpty == true) {
                    container = null;
                }
                return container;
            },
            onItemRendered: function (e) {
                var itemContent = e.itemElement.find('.dx-list-item-content');
                if (itemContent != null && itemContent.html().length == 0) {
                    itemContent.hide();
                    itemContent.parent().parent().prev().addClass('data-content-hide');
                }
            },
        }).dxList('instance');
    })


</script>
<div id="loadpanel"></div>
<div class="col-md-12 col-sm-12 col-xs-12 global-searchWrap">
    <div class="row listcontainer">
        <div class="col-md-12">
            <div id="divUserHistoryList" class="divUserHistoryList"></div>
        </div>
    </div>
</div>
