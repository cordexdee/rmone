<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompareUsersProfile.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMM.CompareUsersProfile" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.4/jspdf.min.js"></script>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    
 .divUserHistoryList .dx-list-group-header{
    border-left: 1px solid #ddd;
    border-right: 1px solid #ddd;
 }
 .divUserHistoryList{
     border: 1px solid #ddd;
 }

 .listcontainer{
     margin-top:25px;
 }
 
 .datesdiv{
    color: #6e6e6e;
    display: inline-block;
    float: right;
    margin-bottom: 15px;

 }
 .dateWrap{
     display:inline-block;
     margin-right:15px;
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
 #divIncludeCurrent{
     font-family: 'Roboto', sans-serif ;
    font-size: 13px;
    color: black;
    /*margin-top: 74px;
    padding-left: 80px;*/
 }
 .divTitle{
    font-size: 13px;
    font-weight: 600;
    display: inline-block;
    margin-bottom: 15px;
 }
 .dataDescription{
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
 .data-value{
      color: black;
    font-size: 13px;
    font-weight: 400;
        margin:7px 0;
 }
 .userLabel{
     font-size:13px;
     color:#4A6EE2;
 }
 #divIncludeCurrent .dx-checkbox-container .dx-checkbox-icon{
    width: 18px;
    height: 18px;
    border-radius: 2px;
    border: 1px solid black;
 }
 #divIncludeCurrent .dx-checkbox-container .dx-checkbox-text {
    /*margin-left: -17px;*/
 }
 .dx-checkbox-checked .dx-checkbox-icon{
     font: 13px/13px DXIcons;
    color: #4A6EE2;
    text-align: center;
 }
 .dx-checkbox-checked .dx-checkbox-icon:before{
         margin-top: -7px;
 }

 .buildAction{
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

#divUserHistoryList .descriptiondetails {
    color: #333;
    cursor: pointer;
    font-size: 14px;
    height: 20px;
    text-align: center;
    font-weight: 600;
}

#divUserHistoryList .icon-style {
    color: black;
    vertical-align: middle;
    margin-left: 3px;
    margin-top: -1px;
    font-size:1.125em;
}

.dx-list:not(.dx-list-select-decorator-enabled) .dx-list-item.dx-state-active {
    background-color: rgba(0,0,0,.04);
    color: #333;
}
.white-space-preline {
    white-space: pre-line;
    padding-right:2%;
    padding-left:1%;
    text-align:justify;
}
.whiteSpace {
    white-space: pre-line;
    font-weight:500;
}
.data-content-hide::before {
    display:none;
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
#divGanttView .dx-icon {
        width:30px !important;
        height:30px !important;
    }
    #divGanttView.dx-button-mode-contained.dx-state-hover {
        background-color: white !important;
        border-color:white !important;
    }
    #divGanttView.dx-button-mode-contained {
    background-color: #fff;
    border-color: white;
    color: #333;
}
    #divGanttView .dx-button-content {
        margin-top: 48px;
        margin-right: 20px;
    }
    #divUserHistoryList .dx-scrollable-content {
    overflow-x:scroll;
    }
    .userImageStyle {
width: 90px;
height: 90px;
border-radius: 50%
}
.display-flex-center{
    display: flex;
    align-items: center;
}
.userLabelNew {
    font-size:16px;
            
    font-weight: 400;
}
.skillStyle {
    border: 1px solid lightgray;
    padding: 7px 18px;
    border-radius: 10px;
    font-size: 14px;
    font-weight: 500;
}
.projectStyle {
    text-align:center;
    font-size:17px;
}

 #divUserHistoryList .dx-scrollable-content::-webkit-scrollbar {
  display: none;
}       

@media (min-width: 576px) { #divUserHistoryList .dx-scrollview-content {
  display: inline-flex;
}

#divUserHistoryList .dx-scrollview-content>.dx-list-group {
  width: 100%;
} 
}

@media (min-width: 1100px) { #divUserHistoryList .dx-scrollview-content {
  display: inline-flex;
}

#divUserHistoryList .dx-scrollview-content>.dx-list-group {
    width: 45.75vw;
} 
}


@media (min-width: 1440px) { #divUserHistoryList .dx-scrollview-content {
  display: inline-flex;
}

#divUserHistoryList .dx-scrollview-content>.dx-list-group {
    width: 31.3vw;
} 

}
#divUserHistoryList .dx-list-group:first-of-type .dx-list-group-header {
    border-top: 1px solid #ddd !important;
}
.moreIcon{
    z-index: 9;
    color: #4A6EE2;
    font-size: 18px;
    width: 26px;
    text-align: center;
}
#divUserHistoryList .dx-list-item {
border-left:1px solid #ddd;
border-right:1px solid #ddd;
    border-bottom:1px solid #ddd;
}

    .resourceGrid{
        height: 145px;
    }

    .experienceHeight{
        height:200px;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var ProjectID = '';
    var UserDropdown = '';
    var userdxList = '';
    var selectedUser = [];
    var strSelectedUserId = "<%=this.SelectedUsers%>";
    var projectTypes = [];
    var listOfCompany = [];
    var listOfRoles = [];
    var listOfModules = [];
    var listComplexity = [];
    var includeCurrentProject = true;
    var actionItems = [{ id: 1, name: "Pdf", icon: "/Content/images/icons/pdf-icon.png" }, { id: 2, name: "Email", icon: "/Content/images/icons/email-icon-white.png" }, { id: 3, name: "Print", icon: "/Content/images/icons/print-icon.png" }];
    var selectedValues = { Company: '', ProjectType: '', Complexity: '', FromDate: '', ToDate: '', Role: '', Module:'' };
    
    $.urlParam = function (name) {
        var results = new RegExp('[\?&]' + name + '=([^&#]*)')
                      .exec(window.location.search);
        return (results !== null) ? results[1] || 0 : false;
    }
    
    var companyName = [];
    if($.urlParam('companyname'))
    {
       /* debugger;*/
        var comp = decodeURIComponent($.urlParam('companyname'));
        companyName.push(comp.replace(/%20/g, ' '));
        selectedValues.Company = companyName[0] + ";";
    }
    
    var projectcomplexity =  [];
    if($.urlParam('projectcomplexity'))
    {
        for (let i = parseInt($.urlParam('projectcomplexity').replace(/%20/g, ' ')); i <= 5; i++) {
            projectcomplexity.push(String(i));
            selectedValues.Complexity += String(i) + ";"
        }
    }

    var projecttype = [];
    if ($.urlParam('requesttype')) {
        projecttype.push($.urlParam('requesttype').replace(/%20/g, ' '));
        selectedValues.ProjectType = projecttype[0] + ";";
    }
    var modulename = [];
    if ($.urlParam('modulename')) {
        modulename.push($.urlParam('modulename').replace(/%20/g, ' '));
        //selectedValues.Module = modulename[0] + ";";
    }
    var additionalData = [];
    if ($.cookie("additionalData") != "") {
        additionalData = JSON.parse($.cookie("additionalData"));
    }
    
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
        if(date != null)
        {
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

    function ShowFilter(){
        if(!$("#filterContent").is(":visible"))
            {
            $("#filterContent").show(1000);
                $("#imgAdvanceMode").attr("src", "/Content/Images/red-filter.png"); 
            }
        else
        {
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
        /*debugger;*/
        jQuery.each(data, function (i, val) {
            jQuery.each(val.items, function (i, value) {
                if (!projectTypes.includes(value.ProjectType) && value.ProjectType != null &&  value.ProjectType != '') {
                    projectTypes.push(value.ProjectType);
                }
                if (!listOfCompany.includes(value.CompanyName) && value.CompanyName != null && value.CompanyName != '') {
                    listOfCompany.push(value.CompanyName);
                }
                if (!listOfRoles.includes(value.RoleName) && value.RoleName != null && value.RoleName != '' ) {
                    listOfRoles.push(value.RoleName);
                }
                if (value.TicketId != null && value.TicketId != '' && !listOfModules.includes(value.TicketId.slice(0, 3))) {
                    listOfModules.push(value.TicketId.slice(0, 3));
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
        listOfModules = listOfModules.sort();

        let companyDropdown = $("#divCompanyDropdown").dxTagBox('instance');
        companyDropdown.option('items', listOfCompany);
        companyDropdown.option('value', companyName);

        let typeDropdown = $("#divTypeDropdown").dxTagBox('instance');
        typeDropdown.option('items', projectTypes);
        typeDropdown.option('value', projecttype);

        let complexityDropdown = $("#divComplexityDropdown").dxTagBox('instance');
        complexityDropdown.option('items', listComplexity);
        complexityDropdown.option('value', projectcomplexity);

        let moduleDropdown = $("#divModuleDropdown").dxTagBox('instance');
        moduleDropdown.option('items', listOfModules);
        //moduleDropdown.option('value', modulename);
    }
    function GetUsersHistoryProfile() {
        $.get("/api/buildprofile/GetUsersProfileForComparison?IncludeCurrentProject=" 
            + includeCurrentProject + "&selectedUserId=" + strSelectedUserId + "&selectedCompany=" + encodeURIComponent(selectedValues.Company)
            + "&selectedRole=" + selectedValues.Role + "&selectedType=" + encodeURIComponent(selectedValues.ProjectType) 
            + "&selectedComplexity=" + selectedValues.Complexity + "&fromDate=" + selectedValues.FromDate
            + "&toDate=" + selectedValues.ToDate + "&selectedModule=" + selectedValues.Module , function (data, status) {                                           
                if (data) { 
                userdxList.option('dataSource', data);   
                }
                else {
                    data = [];                              
                    userdxList.option('dataSource', data);               
                }
            });
        
    }

    function SavePdfToServer(){
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
        $.get("/api/buildprofile/GetUsersProfileForComparison?IncludeCurrentProject="
            + includeCurrentProject + "&selectedUserId=" + strSelectedUserId + "&selectedCompany=" + encodeURIComponent(selectedValues.Company)
            + "&selectedRole=" + selectedValues.Role + "&selectedType=" + encodeURIComponent(selectedValues.ProjectType)
            + "&selectedComplexity=" + selectedValues.Complexity + "&fromDate=" + selectedValues.FromDate
            + "&toDate=" + selectedValues.ToDate + "&selectedModule=" + selectedValues.Module, function (data, status) {
                if (data) {
                    userdxList.option('dataSource', data);
                }
                else {
                    data = [];
                    userdxList.option('dataSource', data);
                }
        });

        $.get("/api/buildprofile/GetUsersProfileForComparison?IncludeCurrentProject=true&selectedUserId=" + strSelectedUserId, function (data, status) {
            if (data) {
                GetData(data);
            }
        });

    });
    function moveLeft() {
        $('#divUserHistoryList .dx-scrollable-content').animate({ scrollLeft: '-=' + $("#divUserHistoryList .dx-scrollable-content").width() }, 1000);
        setTimeout(checkScrollEnd, 1500);
    }
    function moveRight() {
        $('#divUserHistoryList .dx-scrollable-content').animate({ scrollLeft: '+=' + $("#divUserHistoryList .dx-scrollable-content").width() }, 1000);
        setTimeout(checkScrollEnd, 1500);
    }
    function checkScrollEnd() {
        let elem = $('#divUserHistoryList .dx-scrollable-content');
        if (elem[0].scrollWidth - elem.scrollLeft() <= elem.outerWidth()) {
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
    function OpenGanttView() {
        var url = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGridNew&RequestFromProjectAllocation=true&SelectedUsers=" + strSelectedUserId.replaceAll(';', ',');
        window.parent.UgitOpenPopupDialog(url, "", "Timeline for User", "95", "95", "", false);
    }
    function displayDetails(elem, details) {
        if (elem.find("i").hasClass("glyphicon-chevron-up")) {
            elem.find("i").addClass("glyphicon-chevron-down").removeClass("glyphicon-chevron-up");
            elem.parent().addClass("resourceGrid");
            elem.next().text("");
        } else {
            elem.find("i").addClass("glyphicon-chevron-up").removeClass("glyphicon-chevron-down");
            elem.parent().removeClass("resourceGrid");
            elem.next().text(details);
        }
    }
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
                    + "&toDate=" + selectedValues.ToDate + "&selectedModule=" + selectedValues.Module, function (data, status) {
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

        $("#divGanttView").dxButton({
            icon: '/content/Images/ganttBlackNew.png',
            onClick() {
                OpenGanttView();
                //DevExpress.ui.notify('The button was clicked');
            },
        }).dxButton('instance');

        $("#divTypeDropdown").dxTagBox({
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
        
        $("#divModuleDropdown").dxTagBox({
            showSelectionControls: true,
            applyValueMode: "useButtons",
            searchEnabled: true,
            onValueChanged: function (e) {
                selectedValues.Module = '';
                e.value.forEach(function (row, index, arr) {
                    selectedValues.Module = row + ';' + selectedValues.Module;
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
            value: true,
            text: "Include Current Projects",
            onValueChanged: function (objcheckbox) {
                if (objcheckbox.value)
                    includeCurrentProject = true;
                else
                    includeCurrentProject = false;
                GetUsersHistoryProfile();
            }
        });

        var isExperienceTagAvail = false;
        userdxList = $("#divUserHistoryList").dxList({
            // dataSource: employees,
            height: "100%",
            grouped: true,
            collapsibleGroups: true,
            showScrollbar: 'never',
            groupTemplate: function (data) {
                let container = $('<div class="setHeight"></div>');
                let allocationLabel = "Allocation";
                let userPctAllocation = "";
                let userData = additionalData.filter(o => o.Id == data.UserId)[0];
                if (userData != null) {
                    userPctAllocation = userData.pctAllcation;
                    if (userData.allocaionView == '0') {
                        allocationLabel = "Availablity";
                        userPctAllocation = (100 - Number(userData.pctAllcation))
                    }
                }
                let uppersection = $(`<div class="display-flex-center"><div>
                                        <img id="userDisplayImage" class="userImageStyle" alt="User Photo" onclick="openUserProfile()" src="${data.Picture}">
                                    </div>
                                    <div class="userLabelNew ml-3">
                                        <div class="m-1" style="font-size:18px;font-weight:500;">${data.UserName}</div>
                                        <div class="m-1">${data.UserGlobalRole}</div>
                                    </div>
                                    </div>`);
                container.append(uppersection);

                if (data.Certificates != null && data.Certificates != "") {
                    isExperienceTagAvail = true;
                    let certificateData = "";
                    data.Certificates.forEach(function (item, index) {
                        certificateData += "<div class='skillStyle m-2'>" + item + "</div>";
                    })
                    let middleSection = $(`<div class="display-flex-center">
                                        ${certificateData}
                                    </div>`);
                    container.append(middleSection);
                }

                let bottomSection = $(`<div class="display-flex-center userLabelNew mt-2">
                                    <div class="projectStyle ml-2 mr-4">Total Project Experience <br /> ${data.UserProjectCount}</div>
                                    <div class="projectStyle ml-2">Filtered Project <br /> ${data.UserFilteredProjectCount} </div>
                                    </div>`);                
                container.append(bottomSection);                
                return container;
            },
            itemTemplate: function (data, index) {
                var container = $('<div class="resourceGrid"></div>');
                if (data.ProjectTitle != "") {
                    let trucateData = data.ProjectTitle.split(":")[1];
                    let titleData = data.ProjectTitle.split(":")[0];
                    var title = $(`<div class='divTitle' title="${titleData}">${trucateData}</div>`);
                    container.append(title);
                }
                var TicketId = $(`<div class='divTicketId' onclick="${data.Url}">${data.TicketId}</div>`);
                var secondRow = $(`<div class='row' style='text-align:center;'>
                                    <div class='col-md-2 data-value inline-grid-data'><div class='dataTitle'> Role: </div><div class='whiteSpace'> ${data.RoleName}</div> </div>
                                    <div class='col-md-2 data-value inline-grid-data'><div class='dataTitle'> Company  </div><div class='whiteSpace' title='${data.CompanyName}'> ${data.DisplayCompanyName}</div></div>
                                    <div class='col-md-2 data-value inline-grid-data'><div class='dataTitle'>Contract value </div><div class='whiteSpace'> ${data.ContractValue != null ? data.ContractValue.split('.')[0] : ''}</div></div>
                                    <div class='col-md-2 data-value inline-grid-data'><div class='dataTitle'>Type  </div> <div class='whiteSpace'> ${data.ProjectType}</div></div>
                                    <div class='col-md-1 data-value inline-grid-data'><div class='dataTitle'>Length  </div><div class='whiteSpace'> ${daysBetween(data.PreconStartDate, data.PreconEndDate)}</div></div>
                                    <div class='col-md-2 data-value inline-grid-data'><div class='dataTitle'>Complexity  </div><div class='whiteSpace'> ${data.ProjectComplexityChoice}</div></div>
                                </div>`);
                var description = $(`<div class="descriptiondetails"
                                          onclick="displayDetails($(this),\`${data.Description != null && data.Description != "" ? data.Description.replaceAll('"',"'") : ""}\`)">Job Description
                                        <i class="icon-style glyphicon glyphicon-chevron-down"></i></div>
                                    <div class="white-space-preline"></div>
                                `);
                if (isExperienceTagAvail == true) {
                    $(".setHeight").addClass("experienceHeight");
                }
                else {
                    $(".setHeight").removeClass("experienceHeight");
                }
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
                setTimeout(checkScrollEnd, 1500);
            },
        }).dxList('instance');

        //var actionButton = $("#divActionItem").dxDropDownButton({
        //    icon: "/Content/images/export-Black.png",
        //    showArrowIcon: false,
        //    dropDownOptions: {
        //        width: 100
        //    },
        //    hint: "Export",
        //    displayExpr: "name",
        //    onItemClick: function (e) {
        //        if (e.itemData.name == "Pdf") {
        //            SavePdf();
        //        }
        //        if (e.itemData.name == "Email") {
        //            SavePdfToServer();
        //            SendEmailToUser();
        //        }
        //        if (e.itemData.name == "Print") {
        //            PrintResume();
        //        }
        //    },
        //    items: actionItems
        //}).dxList('instance');
    })
    

</script>
   
<div class="col-md-12 col-sm-12 col-xs-12 global-searchWrap">
    <div class="row">
        <%-- <h4  id="NodataFound" style="text-align: center;" runat="server" visible="false"> Sorry, we couldn't find any matching result for  "<%=searchText%>". Please try another search.</h4>--%>
    </div>
    <div class="row headercontainer">
        <%--<div class="col-md-2 noPadding">
            <div id="divIncludeCurrent"></div>
        </div>--%>
        <div class="col-md-11 noPadding">
            <div class="row mt-4">
                <div class="col-md-3">
                    <label class="userLabel" style="visibility:hidden;">Company</label><br />
                    <div id="divIncludeCurrent"></div>
                </div>
                <div class="col-md-3">
                    <label class="userLabel">Company</label>
                    <div id="divCompanyDropdown"></div>
                </div>
                <div class="col-md-3">
                    <label class="userLabel">Type</label>
                    <div id="divTypeDropdown"></div>
                </div>
                <div class="col-md-3">
                    <label class="userLabel">Complexity</label>
                    <div id="divComplexityDropdown"></div>
                </div>
            </div>
            <div class="row mt-2 mb-4">
                <div class="col-md-3">
                    <label class="userLabel">Module</label>
                    <div id="divModuleDropdown"></div>
                </div>
                <div class="col-md-3">
                    <label class="userLabel">From Date</label>
                    <div id="fromdate"></div>
                </div>
                <div class="col-md-3">
                    <label class="userLabel">To Date</label>
                    <div id="todate"></div>
                </div>
                <div class="col-md-3">
                    <label class="userLabel">Role</label>
                    <div id="divRoleDropdown"></div>
                </div>
            </div>
        </div>
        <div class="col-md-1 mt-5 mb-4 noPadding">
            <div id="divGanttView" class="buildAction mt-2"></div>
        </div>
    </div>
    <div id="filterContent" style="display: none; border: 2px solid #ddd; margin: 14px;">
        <div class="borderText whiteSpace">Advanced Filter</div>
    </div>
    <div class="row listcontainer">
        <div class="col-md-12">
            <a class="moreIcon" id="myProjectLeftIcon" style="visibility: hidden;" href="#" onclick="moveLeft()"><i class="glyphicon glyphicon-chevron-left" style="color: black;"></i></a>
            <a class="moreIcon" style="float: right; visibility: hidden;" id="myProjectRightIcon" href="#" onclick="moveRight()"><i class="glyphicon glyphicon-chevron-right" style="color: black;"></i></a>
            <div id="divUserHistoryList" class="divUserHistoryList"></div>
        </div>
    </div>
</div>
