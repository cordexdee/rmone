<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CRMLeadEstimatorView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMONE.CRMLeadEstimatorView" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
   
    .dx-list-group:first-of-type .dx-list-group-header {
        border-top: 1px solid #ddd;
    }

    .f-display-1 {
        font-size: 16px;
    }

    .f-display-2 {
        font-size: 14px;
    }

    .f-display-3 {
        font-size: 15px;
    }

    .w33 {
        width: 21%;
    }

    .w66 {
        width: 63%;
    }

    .data-title {
        color: gray;
        font-size: 13px;
    }

    .data-value {
        color: black;
        font-size: 13px;
        font-weight: 500;
        margin: 1px 0;
    }

    .resourceGrid {
    }

    .row .dx-list-item .dx-list-item-content {
        border: 2px solid #ddd !important;
        padding: 16px 8px;
        cursor: default;
    }

    .row .dx-list:not(.dx-list-select-decorator-enabled) .dx-list-item.dx-state-hover {
        background-color: white;
        color: #333;
    }

    .dx-progressbar-container {
        height: 20px;
        border: 1px solid #ddd;
        background-color: #ddd;
        border-radius: 12px;
    }

    .dx-progressbar-range {
        position: relative;
        border: none;
        background-color: #4fa1d6;
        -webkit-box-sizing: content-box;
        box-sizing: content-box;
        border-radius: 15px !important;
    }

    .dx-progressbar-status {
        position: relative;
        top: -20px;
        left: 10px;
        width: auto;
        color: white;
        height: 20px;
        font-size: 13px;
        font-weight: 500;
    }

    .allocation-blue {
        display: flex;
        height: 45px;
        font-weight: 600;
        width: 50px;
        border: 2px solid #4fa1d6;
        border-radius: 10px;
        align-items: center;
        justify-content: center;
        font-size: 14px;
        margin-left: 11%;
        color:#4b4b4b;
    }

    .allocation-yellow {
        display: flex;
        height: 45px;
        font-weight: 600;
        width: 50px;
        border: 2px solid #ffd800;
        border-radius: 10px;
        align-items: center;
        justify-content: center;
        font-size: 14px;
        margin-left: 11%;
    }

    .dx-list-group-header {
        font-weight: 700;
        padding: 5px 10px 5px;
        border-left: 2px solid #ddd !important;
        border-top: 2px solid #ddd !important;
        border-right: 2px solid #ddd !important;
        background: rgba(238,238,238,.05);
        color: #333;
        margin-top: 9px;
    }

    .dx-list-group-header::before {
        border-top-color: #333;
        margin-top: 25px;
    }

    .dx-list:not(.dx-list-select-decorator-enabled) .dx-list-item.dx-state-active {
        background-color: #fff;
        color: #333;
    }

    .btnAddNew .dx-icon {
        margin-right: 10px !important;
        filter: brightness(4);
        transform: scale(1);
    }

    .dx-list-group {
        border-right: 1px solid #ddd;
        border-left: 1px solid #ddd;
    }

    #divUnfilledProjects .dx-list-group {
        border-right: 4px solid #adaaaa;
    }

    .fw500 {
        font-weight: 500;
    }
    .rightPX {
        /*right: 23px;*/
        right: 30px;
    }

    .filetrDiv {
        border: 2px solid #ddd;
        margin: 4px;
        position: fixed;
        width: 100%;
        margin-top: -50px;
        z-index: 999;
        background: #fff;
    }
    .btnBlue {
        background-color: #789CCE !important;
        color: white;
    }
    .btnNormal {
        background-color: white !important;
        color: black;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>" >
    var baseUrl = ugitConfig.apiBaseUrl;
    var request = {
        Sector: '',
        Division: '',
    }
    var userDetails = [];
    var tempUserData = [];
    var allUsersData = [];
    var unfilledProjects = [];
    var filledProjects = [];
    var colData1 = [];
    var colData2 = [];
    var colData3 = [];

    function GenerateColumnWiseData() {
        let tempData = JSON.parse(JSON.stringify(allUsersData));
        if (request.Sector != '' || request.Division != '') {
            tempData.forEach(function (x) {
                if (request.Sector != '') {
                    x.items = JSON.parse(JSON.stringify(x.items.filter(y => y.Studio != null && y.Studio.split('>').length > 1 ? y.Studio.split('>')[1].trim() == request.Sector : y.Studio == request.Sector)))
                }
                if (request.Division != '') {
                    x.items = JSON.parse(JSON.stringify(x.items.filter(y => y.Division == request.Division)))
                }
            });
        }

        if (tempData != null && tempData.length > 0) {
            filledProjects = tempData.filter(x => x.UserName != null && x.items != null && x.items.length > 0);
            unfilledProjects = tempData.filter(x => x.UserName == null && x.items != null && x.items.length > 0);
            colData1 = [];
            colData2 = [];
            colData3 = [];
            let pushToCol1 = true;
            let pushToCol2 = false;
            let pushToCol3 = false;
            filledProjects.forEach(function (value, index) {
                if (pushToCol1) {
                    colData1.push(value);
                    pushToCol1 = false;
                    pushToCol2 = true;
                    pushToCol3 = false;
                } else if (pushToCol2) {
                    colData2.push(value);
                    pushToCol1 = false;
                    pushToCol2 = false;
                    pushToCol3 = true;
                } else if (pushToCol3) {
                    colData3.push(value);
                    pushToCol1 = true;
                    pushToCol2 = false;
                    pushToCol3 = false;
                }
            });
        }
        $("#divcol1FilledProjects").dxList("instance").option("dataSource", colData1);
        $("#divcol2FilledProjects").dxList("instance").option("dataSource", colData2);
        $("#divcol3FilledProjects").dxList("instance").option("dataSource", colData3);
        $("#divUnfilledProjects").dxList("instance").option("dataSource", unfilledProjects);
        $("#loadpanel").dxLoadPanel("hide");
    }

    function GetLeadEstimatorWithProjectDetails() {
        $.get(baseUrl + "/api/RMONE/GetLeadEstimatorWithProjectDetails", function (data, status) {
            //console.log(data);
            allUsersData = data.estimatorDetails;
            userDetails = data.lstUserProfiles;
            GenerateColumnWiseData();
            loadingPanel.Hide();
        });
    }

    function openUserEditPopup(ticketId, userName, projectLeadUserId, leadEstimatorUserId, leadSuperintendentUserId, preConStartDate, preConEndDate, constStartDate, constEndDate) {
        //if (userName == null || userName == '' || userName == 'null') {
            $('#userFieldPopup').dxPopup({
                visible: false,
                hideOnOutsideClick: true,
                showTitle: true,
                showCloseButton: true,
                title: "Lead Selection",
                width: "300",
                height: "auto",
                resizeEnabled: true,
                dragEnabled: true,
                position: {
                    at: 'center',
                    my: 'center',
                    offset: '0 188',
                    of: `#user${ticketId}`
                },
                contentTemplate: () => {
                    const content = $("<div />");
                    content.append(
                        $(`<div style='padding-bottom: 10px' />`).append(
                            $(`<div>Project Lead</div>`),
                            $("<div id='divProjectLead' />").dxSelectBox({
                                placeholder: "Project Lead",
                                valueExpr: "Id",
                                displayExpr: "Name",
                                searchEnabled: true,
                                showClearButton: true,
                                dataSource: new DevExpress.data.DataSource({
                                    store: userDetails,
                                    paginate: true,
                                }),
                                value: projectLeadUserId != null ? projectLeadUserId : "",
                                width: '100%',
                            }),
                        ),
                        $(`<div style='padding-bottom: 10px' />`).append(
                            $(`<div>Lead Estimator</div>`),
                            $("<div id='divLeadEstimator' />").dxSelectBox({
                                placeholder: "Lead Estimator",
                                valueExpr: "Id",
                                displayExpr: "Name",
                                searchEnabled: true,
                                showClearButton: true,
                                dataSource: new DevExpress.data.DataSource({
                                    store: userDetails,
                                    paginate: true,
                                }),
                                value: leadEstimatorUserId != null ? leadEstimatorUserId : "",
                                width: '100%',
                            }),
                        ),
                        $(`<div style='padding-bottom: 10px' />`).append(
                            $(`<div>Lead Superintendent</div>`),
                            $("<div id='divLeadSuprintendent' />").dxSelectBox({
                                placeholder: "Lead Superintendent",
                                valueExpr: "Id",
                                displayExpr: "Name",
                                searchEnabled: true,
                                showClearButton: true,
                                dataSource: new DevExpress.data.DataSource({
                                    store: userDetails,
                                    paginate: true,
                                }),
                                value: leadSuperintendentUserId != null ? leadSuperintendentUserId : "",
                                width: '100%',
                            }),
                        ),
                        $(`<div style='float:right;' />`).append(
                            $(`<div id='divSaveUserFields' class='btnAddNew' />`).dxButton({
                                text: 'Save',
                                icon: '/content/Images/save-open-new-wind.png',
                                onClick: function (e) {
                                    var AllocStartDate = null;
                                    var AllocEndDate = null;
                                    var curDate = new Date();
                                    if (preConStartDate != '-' && preConEndDate != '-') {
                                        AllocStartDate = new Date(preConStartDate);
                                        AllocEndDate = new Date(preConEndDate);
                                    }
                                    else if (constStartDate != '-' && constEndDate != '-') {
                                        AllocStartDate = new Date(constStartDate);
                                        AllocEndDate = new Date(constEndDate);
                                    }
                                    if (AllocStartDate < curDate && AllocEndDate < curDate) {
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
                                                loadingPanel.Show();
                                                /*$("#loadpanel").dxLoadPanel("show");*/
                                                ProjectLead = $("#divProjectLead").dxSelectBox("instance").option('value');
                                                LeadEstimator = $("#divLeadEstimator").dxSelectBox("instance").option('value');
                                                LeadSuperintendent = $("#divLeadSuprintendent").dxSelectBox("instance").option('value');
                                                $.post(baseUrl + `/api/OPMWizard/UpdateLeadUserFields?TicketId=${ticketId}&ProjectLead=${ProjectLead == null ? '' : ProjectLead}&LeadEstimator=${LeadEstimator == null ? '' : LeadEstimator}&Superintendent=${LeadSuperintendent == null ? '' : LeadSuperintendent}`, function (result) {
                                                    if (result.IsSuccess && result.ErrorMessages.length > 0) {
                                                        var errorMessage = "Resources are assigned. Below are the errors occurred while creating allocations for them.<br/><br/>";
                                                        result.ErrorMessages.forEach(function (item, index) {
                                                            errorMessage = errorMessage + item + "<br/>";
                                                        });
                                                        var res = DevExpress.ui.dialog.alert(errorMessage, "Assign Resources & Create Allocation.");
                                                        res.done(function () {
                                                            GetLeadEstimatorWithProjectDetails();
                                                            const popup = $("#userFieldPopup").dxPopup("instance");
                                                            popup.hide();
                                                            return;
                                                        });
                                                        return;
                                                    } else if (result.IsSuccess && result.ErrorMessages.length == 0) {
                                                        GetLeadEstimatorWithProjectDetails();
                                                        const popup = $("#userFieldPopup").dxPopup("instance");
                                                        popup.hide();
                                                    } else {
                                                        loadingPanel.Hide();
                                                        DevExpress.ui.dialog.alert("Failed to Assign Resources or Create Allocations.", "Error");
                                                        return;
                                                    }
                                                });
                                            }
                                        });
                                    }
                                    else {
                                        loadingPanel.Show();
                                        /*$("#loadpanel").dxLoadPanel("show");*/
                                        ProjectLead = $("#divProjectLead").dxSelectBox("instance").option('value');
                                        LeadEstimator = $("#divLeadEstimator").dxSelectBox("instance").option('value');
                                        LeadSuperintendent = $("#divLeadSuprintendent").dxSelectBox("instance").option('value');
                                        $.post(baseUrl + `/api/OPMWizard/UpdateLeadUserFields?TicketId=${ticketId}&ProjectLead=${ProjectLead == null ? '' : ProjectLead}&LeadEstimator=${LeadEstimator == null ? '' : LeadEstimator}&Superintendent=${LeadSuperintendent == null ? '' : LeadSuperintendent}`, function (result) {
                                            if (result.IsSuccess && result.ErrorMessages.length > 0) {
                                                var errorMessage = "Resources are assigned. Below are the errors occurred while creating allocations for them.<br/><br/>";
                                                result.ErrorMessages.forEach(function (item, index) {
                                                    errorMessage = errorMessage + item + "<br/>";
                                                });
                                                var res = DevExpress.ui.dialog.alert(errorMessage, "Assign Resources & Create Allocation.");
                                                res.done(function () {
                                                    GetLeadEstimatorWithProjectDetails();
                                                    const popup = $("#userFieldPopup").dxPopup("instance");
                                                    popup.hide();
                                                    return;
                                                });
                                                return;
                                            } else if (result.IsSuccess && result.ErrorMessages.length == 0) {
                                                GetLeadEstimatorWithProjectDetails();
                                                const popup = $("#userFieldPopup").dxPopup("instance");
                                                popup.hide();
                                            } else {
                                                loadingPanel.Hide();
                                                DevExpress.ui.dialog.alert("Failed to Assign Resources or Create Allocations.", "Error");
                                                return;
                                            }
                                        });
                                    }
                                    //const popup = $("#userFieldPopup").dxPopup("instance");
                                    //popup.hide();
                                }
                            }),
                        ),
                    );
                    return content;
                },
            });
            const popup = $("#userFieldPopup").dxPopup("instance");
            popup.show();
            if (!(projectLeadUserId == null || projectLeadUserId == undefined || projectLeadUserId == 'null')) {
                var selectBox = $("#divProjectLead").dxSelectBox("instance");
                selectBox.option('value', projectLeadUserId);
            }
            if (!(leadEstimatorUserId == null || leadEstimatorUserId == undefined || leadEstimatorUserId == 'null')) {
                var selectBox = $("#divLeadEstimator").dxSelectBox("instance");
                selectBox.option('value', leadEstimatorUserId);
            }
            if (!(leadSuperintendentUserId == null || leadSuperintendentUserId == undefined || leadSuperintendentUserId == 'null')) {
                var selectBox = $("#divLeadSuprintendent").dxSelectBox("instance");
                selectBox.option('value', leadSuperintendentUserId);
            }
        //}
    }

    function OpenProjectAllocation(ticketid, title) {
        let path = "/Layouts/uGovernIT/DelegateControl.aspx?isdlg=0&isudlg=1&control=CRMProjectAllocationNew&ConfirmBeforeClose=true&isreadonly=true&ticketId=" + ticketid + "&module=" + ticketid.substring(0, 3);
        window.parent.UgitOpenPopupDialog(path, "moduleName=" + ticketid.substring(0, 3), title, '95', '95', 0, 0);
        window.parent.$(".ui-draggable.ui-resizable").css("top", "30px");
    }

    $(document).ready(function () {

        GetLeadEstimatorWithProjectDetails();

        $.get(baseUrl + "/api/CoreRMM/GetSectorStudioDivisionData?dataRequiredFor=division", function (data, status) {
            $('#ddlDivision').dxSelectBox({
                dataSource: data.Table,
                placeholder: '<%=DivisionLabel%>',
                showClearButton: true,
                valueExpr: "Title",
                displayExpr: "Title",
                onValueChanged: function (e) {
                    request.Division = e.value == null ? '' : e.value;
                    $("#loadpanel").dxLoadPanel("show");
                    GenerateColumnWiseData();
                }
            });
        });

        $.get(baseUrl + "/api/CoreRMM/GetSectorStudioDivisionData?dataRequiredFor=studio", function (data, status) {
            {
                $('#ddlStudio').dxSelectBox({
                    dataSource: data.Table,
                    placeholder: '<%=StudioLabel%>',
                    showClearButton: true,
                    valueExpr: "Title",
                    displayExpr: "Title",
                    onValueChanged: function (e) {
                        request.Sector = e.value == null ? '' : e.value;
                        $("#loadpanel").dxLoadPanel("show");
                        GenerateColumnWiseData();
                    }
                });
            }
        });

        $("#loadpanel").dxLoadPanel({
            message: "Loading...",
            visible: true,
            showPane: true,
            shading: true,
        });

        const commonProjectGroupTemplate = function (data) {
            console.log(data);
            let showUserImage = "none";
            if (data.UserName != null && data.UserName != '' && data.UserImageURL != null && data.UserImageURL != '') {
                showUserImage = "block";
            }
            let container = $(`<div class="d-flex flex-column justify-content-center align-items-center"></div>`);
            container.append(
                $(`<div class='f-display-1'>${data.UserName == null ? "Unfilled" : data.UserName}</div>`),
                $(`<div class='f-display-2 fw500' ${data.UserName == null ? "style='color:white;'" : ''}>${data.Role == "" ? "-" : data.Role}</div>`),
                $(`<div class='f-display-2'>${data.UserName == null ? data.items.length : data.PctAllocations + "%"}</div>`),

                $(`<div id="user${data.TicketId}" class='rightPX' style='display:${showUserImage};width:12%;position:absolute;'><image width="50" height="50" style="border-radius:50%;" title='${data.UserName}' src='${data.UserImageURL}' onclick='javascript:event.cancelBubble=true;OpenUserResume(\"${data.ResourceId}\")'></a></div>`)
            )
            return container;
        };

        const commonProjectItemTemplate = function (data, index) {
            let startDate = '-';
            if (data.StartDate != "0001-01-01T00:00:00") {
                startDate = new Date(data.StartDate).format("MMM d, yyyy");
            }
            let endDate = '-';
            if (data.EndDate != "0001-01-01T00:00:00") {
                endDate = new Date(data.EndDate).format("MMM d, yyyy");
            }
            let dueDate = '-';
            if (data.DueDate != "0001-01-01T00:00:00") {
                dueDate = new Date(data.DueDate).format("MMM d, yyyy");
            }
            let preConStartDate = '-';
            if (data.PreConStartDate != "0001-01-01T00:00:00") {
                preConStartDate = new Date(data.PreConStartDate).format("MMM d, yyyy");
            }
            let preConEndDate = '-';
            if (data.PreConEndDate != "0001-01-01T00:00:00") {
                preConEndDate = new Date(data.PreConEndDate).format("MMM d, yyyy");
            }
            let groupData = allUsersData.filter(x => x.items.filter(y => y.TicketId == data.TicketId).length > 0)[0];
            let imageURL = '';
            let leadUserName = '';
            let leadrole = '';
            if (data.LeadUserImageUrl == null || data.LeadUserImageUrl == "null" || data.LeadUserImageUrl == "") {
                imageURL = "/Content/Images/userNew.png";
                leadrole = '';
            }
            else {
                imageURL = data.LeadUserImageUrl;
                leadUserName = data.LeadUserName;
                leadrole = data.LeadRole;
            }
            let allocationClass = parseInt(data.FilledAllocations) == 0 && groupData.UserName != null ? "allocation-yellow" : "allocation-blue";


            let container = $(`<div class="resourceGrid"></div>`);
            container.append(
                $(`<div class='d-flex justify-content-between align-items-center text-center mb-3'></div>`).append(
                    $(`<div style='width:15%'><a href='#' onclick='OpenProjectAllocation("${data.TicketId}","${data.TicketId + ": " + data.ProjectName}")'><div class='${allocationClass}'>${data.FilledAllocations}/${data.TotalAllocations}</div></a></div>`),
                    $(`<div class='w66' style='white-space:normal;word-break:break-word;'></div>`).append(
                        $(`<div class='data-value'>${data.CompanyName}</div>`),
                        $(`<div class='data-value' onclick=\"${data.LeadProjectUrl}\">${data.ProjectName}</div>`)
                    ),
                    $(`<div id="user${data.TicketId}" style='width:14%;margin-right:3%;'><a href="#" ${groupData.UserName != null ? "style=cursor:default" : ''}  
                            onclick="openUserEditPopup('${data.TicketId}','${groupData.UserName}',
                                    '${data.LeadUsers.find((x) => x.Field == `ProjectLeadUser`).UserId}',
                                    '${data.LeadUsers.find((x) => x.Field == `LeadEstimatorUser`).UserId}',
                                    '${data.LeadUsers.find((x) => x.Field == `LeadSuperintendentUser`).UserId}','${preConStartDate}','${preConEndDate}','${startDate}','${endDate}')"><image width="50" height="50" style="border-radius:50%;" title='${leadUserName}-${leadrole}' src='${imageURL}'></a></div>`),
                ),
                $(`<div class='d-flex justify-content-between align-items-center text-center mb-3'></div>`).append(
                    $(`<div class='w33'></div>`).append(
                        $(`<div class='data-title'>Priority</div>`),
                        $(`<div class='data-value'></div>`)
                    ),
                    $(`<div class='w33'></div>`).append(
                        $(`<div class='data-title'>Type</div>`),
                        $(`<div class='data-value'>${data.Type}</div>`)
                    ),
                    $(`<div class='w33'></div>`).append(
                        $(`<div class='data-title'>Volume</div>`),
                        $(`<div class='data-value'>$${data.Volume}</div>`)
                    )
                ),
                $(`<div class='d-flex justify-content-between align-items-center text-center mb-3'></div>`).append(
                    $(`<div class='w33'></div>`).append(
                        $(`<div class='data-title'>Precon Start</div>`),
                        $(`<div class='data-value'>${preConStartDate}</div>`)
                    ),
                    $(`<div class='w33'></div>`).append(
                        $(`<div class='data-title'>Precon End</div>`),
                        $(`<div class='data-value'>${preConEndDate}</div>`)
                    ),
                    $(`<div class='w33'></div>`).append(
                        $(`<div class='data-title'>Due</div>`),
                        $(`<div class='data-value'>${dueDate}</div>`)
                    )
                ),
                $(`<div class='d-flex justify-content-between align-items-center text-center mb-3'></div>`).append(
                    $(`<div class='w33'></div>`).append(
                        $(`<div class='data-title'>Const Start</div>`),
                        $(`<div class='data-value'>${startDate}</div>`)
                    ),
                    $(`<div class='w33'></div>`).append(
                        $(`<div class='data-title'>Const End</div>`),
                        $(`<div class='data-value'>${endDate}</div>`)
                    ),
                    $(`<div class='w33'></div>`).append(
                        $(`<div class='data-title' style='text-align:center;'>Progress</div>`),
                        $(`<div class='data-value' style='padding-left:3%;width:100%' id='progressbar${index}'/>`).dxProgressBar({
                            min: 0,
                            max: parseInt(data.TotalTasks),
                            value: parseInt(data.CompletedTasks),
                            height: 25,
                            allowhtml: true,
                            statusFormat(ratio) {
                                return data.CompletedTasks + "/" + data.TotalTasks;
                            },
                        })
                    ),
                )
            );
            return container;
        };

        const commonProjectItemRenederedTemplate = function (e) {
            if (parseInt(e.itemData.TotalTasks) == 0 || (parseInt(e.itemData.CompletedTasks) / parseInt(e.itemData.TotalTasks) < 0.2)) {
                var itemContent = e.itemElement.find('.dx-progressbar-status');
                if (itemContent != null) {
                    itemContent.css('color', 'black');
                }
            }

        };

        const commonGroupRenderedTemplate = function (e) {
            if (filledProjects != null && filledProjects.length > 3) {
                if (tempUserData == null || tempUserData.length == 0) {
                    e.component.collapseGroup(e.groupIndex);
                } else if (!tempUserData.includes(e.groupData.UserName)) {
                    e.component.collapseGroup(e.groupIndex);
                }
            }
            e.groupElement.on("click", function () {
                if (tempUserData.includes(e.groupData.UserName)) {
                    tempUserData = tempUserData.filter(x => x != e.groupData.UserName);
                }
                else {
                    tempUserData.push(e.groupData.UserName);
                }
            });
        }

        $("#divcol1FilledProjects").dxList({
            grouped: true,
            collapsibleGroups: true,
            groupTemplate: commonProjectGroupTemplate,
            itemTemplate: commonProjectItemTemplate,
            onItemRendered: commonProjectItemRenederedTemplate,
            onGroupRendered: commonGroupRenderedTemplate,
        });

        $("#divcol2FilledProjects").dxList({
            grouped: true,
            collapsibleGroups: true,
            noDataText: '',
            groupTemplate: commonProjectGroupTemplate,
            itemTemplate: commonProjectItemTemplate,
            onItemRendered: commonProjectItemRenederedTemplate,
            onGroupRendered: commonGroupRenderedTemplate,
        });

        $("#divcol3FilledProjects").dxList({
            grouped: true,
            collapsibleGroups: true,
            noDataText: '',
            groupTemplate: commonProjectGroupTemplate,
            itemTemplate: commonProjectItemTemplate,
            onItemRendered: commonProjectItemRenederedTemplate,
            onGroupRendered: commonGroupRenderedTemplate,
        });

        $("#divUnfilledProjects").dxList({
            grouped: true,
            collapsibleGroups: true,
            groupTemplate: commonProjectGroupTemplate,
            itemTemplate: commonProjectItemTemplate,
            onItemRendered: commonProjectItemRenederedTemplate
        });
    });

    function OpenUserResume(userId) {
        if (userId != "") {
            window.parent.parent.UgitOpenPopupDialog('<%= OpenUserResumeUrl %>' + '&SelectedUser=' + userId, "", 'User Resume', '95', '95', "", false);
        }
    }

    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }
</script>
<dx:ASPxLoadingPanel ID="loadingPanel" runat="server" Text=" Please Wait ..." ClientInstanceName="loadingPanel" Modal="True">
    <Image Url="~/Content/Images/ajax-loader.gif"></Image>
</dx:ASPxLoadingPanel>
<div id="loadpanel"></div>
<div id="userFieldPopup"></div>
<div>
    <div class="row p-2 filetrDiv">
        <div class="col-md-offset-4 col-lg-4 col-md-4 col-sm-offset-3 col-sm-6 col-xs-12">
            <div class="row d-flex justify-content-betwen pb-1">
                <div class="ml-1" id="ddlStudio" style="width: 100%;"></div>
                <div class="ml-1" id="ddlDivision" style="width: 100%;"></div>
            </div>
        </div>
    </div>
    <div class="row" style="margin-top:50px;">
        <div class="col-md-3 col-sm-6 col-xs-12 p-1">
            <div id="divUnfilledProjects"></div>
        </div>
        <div class="col-md-3 col-sm-6 col-xs-12 p-1">
            <div id="divcol1FilledProjects"></div>
        </div>
        <div class="col-md-3 col-sm-6 col-xs-12 p-1">
            <div id="divcol2FilledProjects"></div>
        </div>
        <div class="col-md-3 col-sm-6 col-xs-12 p-1">
            <div id="divcol3FilledProjects"></div>
        </div>
    </div>
</div>
