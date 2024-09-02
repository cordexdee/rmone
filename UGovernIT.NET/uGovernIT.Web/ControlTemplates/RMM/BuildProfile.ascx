<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuildProfile.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMM.BuildProfile" %>
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
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var ProjectID = '<%= ProjectID%>';
    var UserDropdown = '';
    var userdxList = '';
    var selectedUser = [];
    var strSelectedUserId = '';
    var projectTypes = [];
    var listOfCompany = [];
    var listOfRoles = [];
    var listComplexity = [];
    var includeCurrentProject = false;
    var actionItems = [{ id: 1, name: "Pdf", icon: "/Content/images/icons/pdf-icon.png" }, { id: 2, name: "Email", icon: "/Content/images/icons/email-icon-white.png" }, { id: 3, name: "Print", icon: "/Content/images/icons/print-icon.png" }];
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
        return (treatAsUTC(endDate) - treatAsUTC(startDate)) / millisecondsPerDay;
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
            $("#filterContent").show(1000);
            $("#imgAdvanceMode").attr("src", "/Content/Images/red-filter.png");
        }
        else {
            $("#filterContent").hide(1000);
            $("#imgAdvanceMode").attr("src", "/Content/Images/Newfilter.png");
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
        $.get("/api/buildprofile/GetUsersHistoryProfile?projectID=" + ProjectID + "&IncludeCurrentProject="
            + includeCurrentProject + "&selectedUserId=" + strSelectedUserId + "&selectedCompany=" + selectedValues.Company
            + "&selectedRole=" + selectedValues.Role + "&selectedType=" + selectedValues.ProjectType
            + "&selectedComplexity=" + selectedValues.Complexity + "&fromDate=" + selectedValues.FromDate
            + "&toDate=" + selectedValues.ToDate, function (data, status) {
                if (data) {
                    userdxList.option('dataSource', data);
                }
                else {
                    data = [];
                    userdxList.option('dataSource', data);
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

    function SendEmailToUser() {
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
    }

    $(document).ready(function () {

        $.get("/api/buildprofile/GetUsers?projectID=" + ProjectID, function (data, status) {
            if (data) {

                UserDropdown.option('items', data);
                UserDropdown.option('value', data);

            }
            else {
                data = [];
            }
        });


    });

    $(function () {
        UserDropdown = $("#divUsersDropdown").dxTagBox({
            //items: listAgent,
            displayExpr: "AssignedToName",
            placeholder: "users",
            showSelectionControls: true,
            // applyValueMode: "useButtons",
            applyValueMode: "useButtons",
            searchEnabled: true,
            value: selectedUser,
            text: 'AssignedToName',
            onValueChanged: function (e) {
                var userlist = e.value;
                strSelectedUserId = '';
                var promise = userlist.forEach(function (row, index, arr) {
                    strSelectedUserId = row.AssignedTo + ';' + strSelectedUserId;
                });
                $.get("/api/buildprofile/GetUsersHistoryProfile?projectID=" + ProjectID + "&IncludeCurrentProject="
                    + includeCurrentProject + "&selectedUserId=" + strSelectedUserId + "&selectedCompany=" + selectedValues.Company
                    + "&selectedRole=" + selectedValues.Role + "&selectedType=" + selectedValues.ProjectType
                    + "&selectedComplexity=" + selectedValues.Complexity + "&fromDate=" + selectedValues.FromDate
                    + "&toDate=" + selectedValues.ToDate, function (data, status) {
                        if (data) {
                            userdxList.option('dataSource', data);
                            GetData(data);
                        }
                        else {
                            data = [];
                            userdxList.option('dataSource', data);
                        }

                    });
            }
        }).dxTagBox('instance');

        $("#divTypeDropdown").dxTagBox({
            items: projectTypes,
            showSelectionControls: true,
            applyValueMode: "useButtons",
            searchEnabled: true,
            onValueChanged: function (e) {
                selectedValues.ProjectType = '';
                e.value.forEach(function (row, index, arr) {
                    selectedValues.ProjectType = row + ';' + selectedValues.ProjectType;
                });
                GetUsersHistoryProfile();
            }
        }).dxTagBox('instance');

        $("#divCompanyDropdown").dxTagBox({
            items: listOfCompany,
            showSelectionControls: true,
            applyValueMode: "useButtons",
            searchEnabled: true,
            onValueChanged: function (e) {
                selectedValues.Company = '';
                e.value.forEach(function (row, index, arr) {
                    selectedValues.Company = row + ';' + selectedValues.Company;
                });
                GetUsersHistoryProfile();
            }
        }).dxTagBox('instance');
        $("#divRoleDropdown").dxTagBox({
            items: listOfRoles,
            showSelectionControls: true,
            applyValueMode: "useButtons",
            searchEnabled: true,
            onValueChanged: function (e) {
                selectedValues.Role = '';
                e.value.forEach(function (row, index, arr) {
                    selectedValues.Role = row + ';' + selectedValues.Role;
                });
                GetUsersHistoryProfile();
            }
        }).dxTagBox('instance');

        $("#divComplexityDropdown").dxTagBox({
            items: listComplexity,
            showSelectionControls: true,
            applyValueMode: "useButtons",
            searchEnabled: true,
            onValueChanged: function (e) {
                selectedValues.Complexity = '';
                e.value.forEach(function (row, index, arr) {
                    selectedValues.Complexity = row + ';' + selectedValues.Complexity;
                });
                GetUsersHistoryProfile();
            }
        }).dxTagBox('instance');

        $('#fromdate').dxDateBox({
            type: 'date',
            onValueChanged: function (e) {
                selectedValues.FromDate = '';
                selectedValues.FromDate = formatDate(e.value);
                GetUsersHistoryProfile();
            }
        });

        $('#todate').dxDateBox({
            type: 'date',
            onValueChanged: function (e) {
                selectedValues.ToDate = '';
                selectedValues.ToDate = formatDate(e.value);
                GetUsersHistoryProfile();
            }
        });

        $("#divIncludeCurrent").dxCheckBox({
            value: false,
            text: "Include Current Projects",
            onValueChanged: function (objcheckbox) {
                if (objcheckbox.value)
                    includeCurrentProject = true;
                else
                    includeCurrentProject = false;

                $.get("/api/buildprofile/GetUsersHistoryProfile?projectID=" + ProjectID + "&IncludeCurrentProject=" + includeCurrentProject + "&selectedUserId=" + strSelectedUserId, function (data, status) {
                    if (data) {
                        userdxList.option('dataSource', data);
                    }
                    else {
                        data = [];
                        userdxList.option('dataSource', data);
                    }
                });
            }
        });

        userdxList = $("#divUserHistoryList").dxList({
            // dataSource: employees,
            height: "100%",
            grouped: true,
            collapsibleGroups: true,
            showScrollbar: 'never',
            groupTemplate: function (data) {
                return $(`<div class='d-flex align-items-center'>${data.UserDatailsControl}</div>`);
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
                                    <div class='col-md-1 data-value inline-grid-data'><div class='dataTitle'>Length  </div><div class='whiteSpace'> ${daysBetween(data.PreconStartDate, data.PreconEndDate)}</div></div>
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

        var actionButton = $("#divActionItem").dxDropDownButton({
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
        }).dxDropDownButton('instance');
    })


</script>
<div class="col-md-12 col-sm-12 col-xs-12 global-searchWrap">
    <div class="row">
        <%-- <h4  id="NodataFound" style="text-align: center;" runat="server" visible="false"> Sorry, we couldn't find any matching result for  "<%=searchText%>". Please try another search.</h4>--%>
    </div>
    <div class="row headercontainer">
        <div class="col-md-3 noPadding">
            <div id="divIncludeCurrent"></div>
        </div>
        <div class="col-md-6">
            <label class="userLabel">User</label>
            <div id="divUsersDropdown"></div>
        </div>
        <div class="col-md-1 noPadding" style="margin-top: 20px;">
            <div id="divFilter">
                <img src="/Content/Images/Newfilter.png" id="imgAdvanceMode" onclick="ShowFilter()" style="width: 20px;" title="Advanced Filter">
            </div>
        </div>
        <div class="col-md-2">
            <div id="divActionItem" class="buildAction"></div>
        </div>
    </div>
    <div id="filterContent" style="display: none; border: 2px solid #ddd; margin: 14px;">
        <div class="borderText whiteSpace">Advanced Filter</div>
        <div class="row mt-4">
            <div class="col-md-4">
                <label class="userLabel">Company</label>
                <div id="divCompanyDropdown"></div>
            </div>
            <div class="col-md-4">
                <label class="userLabel">Type</label>
                <div id="divTypeDropdown"></div>
            </div>
            <div class="col-md-4">
                <label class="userLabel">Complexity</label>
                <div id="divComplexityDropdown"></div>
            </div>
        </div>
        <div class="row mt-2 mb-4">
            <div class="col-md-4">
                <label class="userLabel">From Date</label>
                <div id="fromdate"></div>
            </div>
            <div class="col-md-4">
                <label class="userLabel">To Date</label>
                <div id="todate"></div>
            </div>
            <div class="col-md-4">
                <label class="userLabel">Role</label>
                <div id="divRoleDropdown"></div>
            </div>
        </div>
    </div>
    <div class="row listcontainer">
        <div class="col-md-12">
            <div id="divUserHistoryList" class="divUserHistoryList"></div>
        </div>
    </div>
</div>





