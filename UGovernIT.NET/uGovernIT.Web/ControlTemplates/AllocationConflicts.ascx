<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllocationConflicts.ascx.cs" Inherits="uGovernIT.Web.AllocationConflicts" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #divAllocationConflict .dx-tile {
        position: static; 
        border: none;
    }
    .allocConflictItemMsg {
        font-size: 12px;
        text-align: center;
        font-family: 'Poppins', sans-serif;
    }
    .dx-popup-content{
        overflow-y: auto;
    }

    .tileViewContainercnf .dx-tile {
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

    #tileViewContainercnf .dx-empty-message {
        text-align: center;
        padding-top: 62px;
    }

    #tileViewContainercnf .capacityblock {
        float: left;
        width: 74px;
        text-align: center;
        height: 20px;
    }

        #tileViewContainercnf .capacityblock:first-child {
            border-right: 1px solid #c3c3c3;
        }

    #tileViewContainercnf .allocation-v0 {
        background: #ffffff;
    }

    #tileViewContainercnf .allocation-v1 {
        background: #fcf7b5;
    }

    #tileViewContainercnf .allocation-v2 {
        background: #f8ac4a;
    }

    #tileViewContainercnf .allocation-r0 {
    }

    #tileViewContainercnf .allocation-r1 {
        background: #baf0d7;
    }

    #tileViewContainercnf .allocation-r2 {
        background: #fcf7b5;
    }

    #tileViewContainercnf .allocation-r3 {
        background: #f8ac4a;
    }

    #tileViewContainercnf .allocation-r4 {
        background: #ff0d0d;
        color: #e8e6e6;
    }

    #tileViewContainercnf .allocation-c0 {
        background: #ffffff;
    }

    #tileViewContainercnf .allocation-c1 {
        background: #fcf7b5;
    }

    #tileViewContainercnf .allocation-c2 {
        background: #f8ac4a;
    }

    #tileViewContainercnf .allocation-block {
        height: 63px;
        display: flex;
        justify-content: center;
        align-items: center;
    }

        #tileViewContainercnf .allocation-block .timesheet {
            position: absolute;
            top: 1px;
            left: 87%;
            cursor: pointer;
        }

    #tileViewContainercnf .dx-tile {
        border: 1px solid #c3c3c3;
    }

    #tileViewContainercnf .capacitymain {
        border-top: 1px solid #c3c3c3;
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
        /*padding-left: 25px;*/
        width: 30%;
        float: left;
        /*margin-left: 10px;*/
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

    .filterctrl-jobDepartment {
        clear: both;
        float: left;
        width: 30%;
        margin-top: 6px;
        /*margin-left:12px;*/
    }

    .filterctrl-userpicker {
        width: 30%;
        float: left;
        margin-left: 10px;
        margin-top: 6px;
    }
    .strip{
        height: 50px;
        width:50px;
        display:flex;
        align-items:center;
        justify-content:center;
        margin-bottom:5px;
        border-radius:50%;
        color: grey;
        font-size:16px;
        margin-left:auto;
        margin-right:auto;
        font-weight: 600;
        background-color: white;
    }
    #divAllocationConflict .dx-tile {
    color:grey !important;
    }
    .allocConflictItem {
    border: 3px solid #ff3535;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var baseUrl = ugitConfig.apiBaseUrl;
    var popupFilters = {};
    var projectID = '';
    var globaldata = [];
    var DS = [];

    var ShowByUsersDivisionCnf = '<%=ShowByUsersDivision%>';

    var radioGroupItems = [
        { text: "Fully Available", value: 0 },
        { text: "Partially Available", value: 1 },
        { text: "All Resources", value: 2 }
    ];

    function openDialog(path, params, titleVal, width, height, stopRefresh) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh);
    }

    $("#divAllocationConflict").height('<%=Height%>' == '' ? '30px' : '<%=Height%>');
    $("#divAllocationConflict").width('<%=Width%>' == '' ? '30px' : '<%=Width%>');

    BindAllocationsCnf();

    function BindAllocationsCnf() {
        $.get(baseUrl + "/api/RMMAPI/GetAllocationConflicts?ShowByUsersDivision=" + ShowByUsersDivisionCnf, function (ytddata) {

            var tileDataSource = Object.entries(ytddata);
            var divYTDmargin = $("#divAllocationConflict").dxTileView({
                dataSource: tileDataSource,
                showScrollbar: false,
                baseItemHeight: 100,
                baseItemWidth: 100,
                itemMargin: 0,
                direction: "vertical",
                width: 100,
                height: 100,
                itemTemplate: function (itemData, itemIndex, itemElement) {
                    
                    var item = itemData[1];
                    itemElement.addClass("tileContainer");
                    html = new Array();
                    if (item.totalAlloc <= 0)  //totalAllocThisWeek
                        html.push(`<div class='strip allocConflictItem'>${item.totalAlloc}</div>`);
                    else
                        html.push(`<div class='strip allocConflictItem'>${item.totalAlloc}</div>`);

                    html.push("<div class='allocConflictItemMsg'>");
                    html.push('Time Off');
                    html.push('<br>');
                    html.push('Conflicts');
                    html.push("</div>");
                    itemElement.append(html.join(""));
                },
                onItemClick: function (e) {
                    var popupConflicts = $("#popupConflicts").dxPopup(popupConflictsOptions).dxPopup("instance");
                    popupConflicts.show();
                }
            });
        });
    }


    $.get(baseUrl + "/api/RMMAPI/GetAllocationConflictProjects", function (data) {
        ////debugger;
        DS = data;
    });

    var popupConflictsOptions = {
        width: 1200,
        height: 650,
        contentTemplate: function () {
            return $("<div />").append(
                $("<Div />").dxDataGrid({
                    showColumnLines: false,
                    showRowLines: true,
                    rowAlternationEnabled: true,
                    showBorders: true,
                    dataSource: "/api/rmmapi/GetOtherAllocationDetails?AllocationType=conflicts&ShowByUsersDivision=" + ShowByUsersDivisionCnf,
                    keyExpr: 'ID',
                    grouping: {
                        autoExpandAll: true,
                    },
                    scrolling: {
                        mode: 'infinite',
                    },
                    groupPanel: {
                        visible: false
                    },
                    columns: [
                        {
                            dataField: "User",
                            caption: "Name",
                            width: 250
                        },
                        {
                            dataField: "TicketID",
                            caption: "PTO",
                            width: 250
                        },
                        {
                            dataField: "Type",
                            caption: "PTO Type",
                            width: 250
                        },
                        {
                            dataField: "AllocationStartDate",
                            caption: "Start Date",
                            format: 'MMM d, yyyy',
                            dataType: "date",
                            width: 180
                        },
                        {
                            dataField: "AllocationEndDate",
                            caption: "End Date",
                            format: 'MMM d, yyyy',
                            dataType: "date",
                            width: 180
                        }
                    ],
                    masterDetail: {
                        enabled: true,
                        template(container, options) {
                            //debugger;
                            data = DS.filter(item => item.ResourceUser == options.data.AssignedToUser);
                            StartDt = options.data.AllocationStartDate;
                            EndDt = options.data.AllocationEndDate;
                            OldAssignedToUserId = options.data.AssignedToUser;
                            OldAssignedToUserName = options.data.User;
                            PTOid = options.data.ID;

                            $('<div>')
                                .addClass('master-detail-caption')
                                /*.text(`${currentEmployeeData.FirstName} ${currentEmployeeData.LastName}'s Tasks:`)*/
                                .text('Conflicting Projects:')
                                .appendTo(container);

                            $('<div>')
                                .dxDataGrid({
                                    columnAutoWidth: true,
                                    showBorders: true,
                                    columns: [
                                        {
                                            caption: 'ProjectId',
                                            dataField: 'ProjectId',
                                            cellTemplate: function (container, options) {
                                                $(`<span><a style ='cursor:pointer' onclick="openDialog('<%= ProjectTeamPath%>','ticketId=${options.data.TicketID}&module=${options.data.Module}&isdlg=1&isudlg=1',' ${options.data.ProjectId} :  ${options.data.TicketID} ${options.data.Title}','90','90',false)">${options.data.ProjectId}</a></span >`)
                                                    .appendTo(container);
	                                        },
                                        },
                                        {
	                                        caption: 'TicketId',
	                                        dataField: 'TicketID',
	                                        cellTemplate: function (container, options) {
                                                $(`<span><a style ='cursor:pointer' onclick="openDialog('<%= ProjectTeamPath%>','ticketId=${options.data.TicketID}&module=${options.data.Module}&isdlg=1&isudlg=1',' ${options.data.ProjectId} :  ${options.data.TicketID} ${options.data.Title}','90','90',false)">${options.data.TicketID}</a></span >`)
                                                    .appendTo(container);
                                            },
                                        },
                                        {
                                            dataField: 'Title',
                                            caption: 'Project',
                                        },
                                        'Role',
                                        {
                                            dataField: 'AllocationStartDate',
                                            dataType: 'date',
                                            caption: 'Start Date'
                                        },
                                        {
                                            dataField: 'AllocationEndDate',
                                            dataType: 'date',
                                            caption: 'End Date'
                                        },
                                        'PctAllocation',                                        ,
                                        {
                                            dataField: "",
                                            dataType: "text",
                                            caption: "",
                                            width: 70,
                                            allowEditing: false,
                                            cellTemplate: function (container, options) {
                                                $('.dx-header-row').addClass('devExtDataGrid-headerRow');
                                                $('.dx-data-row').addClass('devExtDataGrid-DataRow');

                                                $("<div id='dataId'>")
                                                    .append("<span style='float: left;overflow: auto;'></span>")
                                                    .append($("<img>", { "src": "/content/images/moreoptions_blue.png", "Title": "Find Resource", "ticketid": options.data.TicketID, "projectTitle": options.data.Title, "pctallocation": options.data.PctAllocation, "ID": options.data.ID, "PTOid": PTOid, "group": options.data.RoleId, "rolename": options.data.Role, "startDate": StartDt, "endDate": EndDt, "AllocationStartDate": options.data.AllocationStartDate, "AllocationEndDate": options.data.AllocationEndDate, "OldAssignedToUserId": OldAssignedToUserId, "OldAssignedToUserName": OldAssignedToUserName, "style": "float: right;overflow: auto;cursor: pointer;", "class": "assigneeToImgCnf" }))
                                                    .appendTo(container);
                                            }
                                        }
                                        ],
                                    dataSource: new DevExpress.data.DataSource({
                                        store: new DevExpress.data.ArrayStore({
                                            key: 'ID',
                                            data: data,
                                        }),
                                        /*filter: ['ResourceUser', '=', options.key],*/
                                    }),
                                }).appendTo(container);
                        },
                    }
                })
            );
        },
        showTitle: true,
        title: "Time Off Conflicts",
        dragEnabled: true,
        hideOnOutsideClick: false
    };

    $(document).on("click", "img.assigneeToImgCnf", function (e) {
        //debugger;

        var groupid = $(this).attr("id");
        var dataid = e.target.id;
        var data = _.find(globaldata, function (s) { return s.ID.toString() === dataid; });
        projectID = $(this).attr("ticketid");
        popupFilters.ID = dataid;
        popupFilters.PTOid = $(this).attr("PTOid");
        popupFilters.projectID = projectID;
        popupFilters.projectTitle = $(this).attr("projectTitle");
        popupFilters.resourceAvailability = 2;
        popupFilters.complexity = false;
        popupFilters.projectVolume = false;
        popupFilters.projectCount = false;
        popupFilters.RequestTypes = false;
        popupFilters.Customer = false;
        popupFilters.Sector = false;
        popupFilters.groupID = (data != null && data != undefined) ? data.Type : $(this).attr("group");
        popupFilters.startDate = new Date($(this).attr("startDate"));
        popupFilters.endDate = new Date($(this).attr("endDate"));

        popupFilters.allocationStartDate = new Date($(this).attr("AllocationStartDate"));
        popupFilters.allocationEndDate = new Date($(this).attr("AllocationEndDate"));
        popupFilters.pctAllocation = $(this).attr("pctallocation"); //data.pctAllocation;
        popupFilters.isAllocationView = false;
        popupFilters.OldAssignedToUserId = $(this).attr("OldAssignedToUserId");
        popupFilters.OldAssignedToUserName = $(this).attr("OldAssignedToUserName");

        if (projectID.startsWith("OPM")) {
            popupFilters.ModuleIncludes = false;
        }
        else {
            popupFilters.ModuleIncludes = false;
        }
        popupFilters.JobTitles = "";
        popupFilters.departments = "";
        popupFilters.SelectedUserID = "";
        popupFilters.ID = $(this).attr("ID");
        RowId = $(this).attr("ID");

        var popupTitle = "Available Resource";
        if (data)
            popupTitle = "Available " + data.TypeName + "s";
        else if ($(this).attr("rolename") != '' || $(this).attr("rolename") != undefined)
            popupTitle = "Available " + $(this).attr("rolename") + "s";

        $("#popupContainerConflicts").dxPopup({
            title: popupTitle,
            width: "95%",
            height: "90%",
            visible: true,
            scrolling: true,
            dragEnabled: true,
            resizeEnabled: true,
            /*position: { my: "left top", at: "left top", of: window },*/
            contentTemplate: function (contentElement) {

                contentElement.append(

                    $("<div class='px-3 shadow-effect mb-2' />").append(
                        $("<div class='row fleft pt-4 mt-1' />").dxRadioGroup({
                            dataSource: radioGroupItems,
                            displayExpr: "text",
                            value: _.findWhere(radioGroupItems, { value: popupFilters.resourceAvailability }),
                            layout: "horizontal",
                            onValueChanged: function (e) {

                                popupFilters.resourceAvailability = e.value.value;
                                bindDatapopupCnf(popupFilters);
                            }
                        }),

                        $("<div id='divworkExbar' class='fleft pb-3 workExbar' />").append(
                            $("<div style='margin-top: 22px; margin-left: 15px;'>").dxButton({
                                icon: "/content/images/work-experience.png",
                                hint: 'Show Experience',
                                //type: "success",                                
                                onClick: function (e) {

                                    $("#divworkExbar .dx-button-mode-contained").toggleClass("state-active");
                                    if (popupFilters.projectVolume) {
                                        popupFilters.projectVolume = false;
                                        popupFilters.projectCount = false;
                                        bindDatapopupCnf(popupFilters);
                                        e.component.option("hint", "Show Experience");
                                    } else {
                                        popupFilters.projectVolume = true;
                                        popupFilters.projectCount = true;
                                        bindDatapopupCnf(popupFilters);
                                        e.component.option("hint", "Hide Experience");
                                    }
                                }
                            })
                        ),

                        $("<div class='row bs clearfix' />").append(

                            $("<div class='col-md-2 col-sm-4 col-xs-12  pb-3'>").append(
                                $("<label class='lblStartDate' style='width:100%;'>Start Date</label>"),
                                $("<div id='dtStartDate' class='chkFilterCheck' style='padding-left:5px;width:100%;' />").dxDateBox({
                                    type: "date",
                                    value: popupFilters.startDate,
                                    displayFormat: 'MMM d, yyyy',
                                    onValueChanged: function (data) {

                                        popupFilters.startDate = data.value;
                                        var endDateObj = $("#dtEndDate").dxDateBox("instance");
                                        if (typeof endDateObj != "undefined") {
                                            var enddate = endDateObj.option('value');
                                            if (new Date(enddate) < new Date(data.value)) {
                                                popupFilters.endDate = popupFilters.startDate;
                                                endDateObj.option('value', popupFilters.startDate);
                                            }
                                        }
                                        bindDatapopupCnf(popupFilters);
                                    }

                                })

                            ),

                            $("<div class='col-md-2 col-sm-4 col-xs-12 pb-3 noPadd1'>").append(
                                $("<label class='lblStartDate' style='width:100%;'>End Date</label>"),
                                $("<div id='dtEndDate' class='chkFilterCheck' style='width:100%;' />").dxDateBox({
                                    type: "date",
                                    value: popupFilters.endDate,
                                    displayFormat: 'MMM d, yyyy',
                                    onValueChanged: function (data) {

                                        popupFilters.endDate = data.value;
                                        var startDateObj = $("#dtStartDate").dxDateBox("instance");
                                        if (typeof startDateObj != "undefined") {
                                            var startdate = startDateObj.option('value');
                                            if (new Date(startdate) > new Date(data.value)) {
                                                popupFilters.startDate = popupFilters.endDate;
                                                startDateObj.option('value', popupFilters.endDate);
                                            }
                                        }
                                        bindDatapopupCnf(popupFilters);
                                    }
                                })
                            ),

                            $("<div class='col-cus1 col-sm-4 col-xs-12 pb-3'>").append(
                                $("<label class='lblStartDate' style='width:100%;'>% Allocation</label>"),
                                $("<div class='chkFilterCheck' style='width:100%;' />").dxTextBox({
                                    value: popupFilters.pctAllocation,
                                    onValueChanged: function (data) {
                                        popupFilters.pctAllocation = data.value;
                                    }
                                })

                            ),
                            $("<div style='margin-top: 22px;'>").dxButton({
                                icon: "/content/images/changeViewGreen.png",
                                hint: 'change view',
                                //type: "success",                                
                                onClick: function (e) {
                                    var popup = $('#popupContainerConflicts').dxPopup('instance');
                                    if (popupFilters.isAllocationView == true) {
                                        popupFilters.isAllocationView = false;
                                        var popupTitle = "Available Resource";
                                        if (data)
                                            popupTitle = "Available " + data.TypeName + "s";
                                        popup.option('title', popupTitle);
                                        e.component.option('icon', "/content/images/changeViewGreen.png");
                                    }
                                    else {
                                        popupFilters.isAllocationView = true;

                                        var popupTitle = "Allocated Resource";
                                        if (data)
                                            popupTitle = "Allocated " + data.TypeName + "s";
                                        popup.option('title', popupTitle)
                                        e.component.option('icon', "/content/images/changeView.png");
                                    }


                                    bindDatapopupCnf(popupFilters);
                                }
                            }),


                        ),
                    ),


                    $("<div class='shadow-effect' style='padding-bottom: 10px;' />").append(

                        /* $("<h5>Filter by</h5>").append(),*/
                        $("<div id='filterChecks' class='clearfix pb-3' />").append(
                            $("<div id='chkComplexity' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                text: "Complexity",

                                value: popupFilters.complexity,
                                onValueChanged: function (e) {

                                    popupFilters.complexity = e.value;
                                    bindDatapopupCnf(popupFilters);
                                },
                            }),


                            $("<div id='chkCustomer' title='Customer' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                text: 'Customer',
                                value: popupFilters.Customer,
                                hint: 'Customer',
                                onValueChanged: function (e) {
                                    popupFilters.Customer = e.value;
                                    bindDatapopupCnf(popupFilters);
                                },
                            }),

                            $("<div id='chkSector' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                text: 'Sector',
                                hint: 'Sector',
                                value: popupFilters.Sector,
                                onValueChanged: function (e) {
                                    popupFilters.Sector = e.value;
                                    bindDatapopupCnf(popupFilters);
                                },
                            }),
                        ),

                        $("<div id='dropdownFilters' class='row' />").append(

                            $("<div class='filterctrl-jobDepartment' />").dxSelectBox({
                                placeholder: "Limit By Department",
                                valueExpr: "ID",
                                displayExpr: "Title",
                                dataSource: "/api/rmmapi/GetDepartments?GroupID=" + popupFilters.groupID,
                                searchEnabled: true,
                                onSelectionChanged: function (selectedItems) {
                                    var items = selectedItems.selectedItem.ID;

                                    popupFilters.departments = items;
                                    $.get("/api/rmmapi/GetGroupTitles?GroupID=" + popupFilters.groupID + "&DepartmentID=" + items, function (data, status) {

                                        JobTitleData = data;
                                        var tagBox = $("#tagboxTitles").dxTagBox("instance");
                                        tagBox.option("dataSource", JobTitleData);
                                        tagBox.reset();
                                    });

                                    bindDatapopupCnf(popupFilters);
                                },
                                onContentReady: function (e) {

                                }
                            }),

                            $("<div id='tagboxTitles'  class='filterctrl-jobtitle' />").dxTagBox({
                                text: "Job Titles",
                                placeholder: "Limit By Job Title",
                                //valueExpr: "Title",
                                //displayExpr: "Title",
                                searchEnabled: true,
                                dataSource: "/api/rmmapi/GetGroupTitles?GroupID=" + popupFilters.groupID + "&DepartmentID=0",
                                maxDisplayedTags: 2,
                                onSelectionChanged: function (selectedItems) {

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
                                    bindDatapopupCnf(popupFilters);
                                }
                            }),

                            $("<div class='filterctrl-userpicker' />").dxSelectBox({
                                placeholder: "Choose Any User",
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
                                        bindDatapopupCnf(popupFilters);
                                    } else {
                                        var items = selectedItems.selectedItem.Id;
                                        popupFilters.SelectedUserID = items;
                                        popupFilters.projectID = projectID;
                                        popupFilters.complexity = false;
                                        bindDatapopupCnf(popupFilters);
                                    }
                                }
                            }),
                        )
                    ),

                    bindDatapopupCnf(popupFilters)
                )

            },
            itemClick: function (e) {
            }
        });

        var popupInstanceCnf = $('#popupContainerConflicts').dxPopup('instance');
        popupInstanceCnf.option('title', popupTitle);
    });

    function bindDatapopupCnf(popupFilters) {
        var titleViewCnf = null;
        if (typeof (popupFilters.allocationStartDate) == "object") {
            if (popupFilters.allocationStartDate != null)
                popupFilters.allocationStartDate = (popupFilters.allocationStartDate).toISOString();
        }

        if (typeof (popupFilters.allocationEndDate) == "object") {
            if (popupFilters.allocationEndDate != null)
                popupFilters.allocationEndDate = (popupFilters.allocationEndDate).toISOString();
        }

        if ($("#tileViewContainercnf").length > 0) {

            var titleViewObjCnf = $('#tileViewContainercnf').dxTileView('instance');
            if (titleViewObjCnf) {
                titleViewObjCnf.option("dataSource", "/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters));
                titleViewObjCnf._refresh();
            }
        }
        else {

            titleViewCnf = $("<div id='tileViewContainercnf' style='clear:both;padding-bottom:100px;' />").dxTileView({
                height: window.innerHeight - 120,
                width: window.innerWidth - 150,
                showScrollbar: true,
                baseItemHeight: 65,
                baseItemWidth: 150,
                itemMargin: 15,
                direction: "vertical",
                elementAttr: { "class": "tileViewContainercnf" },
                noDataText: "No resource available",
                dataSource: "/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters),
                itemTemplate: function (itemData, itemIndex, itemElement) {
                    itemData.PctAllocation = Math.round(itemData.PctAllocation);
                    var html = new Array();
                    var str = itemData.AssignedTo + "','" + itemData.AssignedToName;
                    var strwithspace = str.replace(/\s/g, '&nbsp;'); //str.replace(" ", "&nbsp;")
                    html.push("<div class='timesheet'><img src='/content/images/Pipeline-By-Estimator.png' height='20px' title='Allocation Timeline' onclick=openResourceTimeSheet('" + strwithspace + "'); />");
                    html.push("</div>");
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
                    else if (itemData.PctAllocation > 0 && !popupFilters.isAllocationView) {
                        html.push("<div style='padding-bottom:3px;'>");
                        html.push("(" + (100 - Number(itemData.PctAllocation)) + "%)");
                        //html.push("(" + (Number(itemData.PctAllocation)) + "%)");
                        html.push("</div>");
                    }
                    if (popupFilters.isAllocationView && itemData.PctAllocation < 100) {
                        html.push("<div style='padding-bottom:3px;'>");
                        html.push("(" + (Number(itemData.PctAllocation)) + "%)");
                        html.push("</div>");
                    }
                    if (popupFilters.projectCount || popupFilters.projectVolume) {
                        if (itemData.PctAllocation > 0) {
                            html.push("<div class='capacitymain'>");
                            html.push("<div class='capacityblock allocation-v" + itemData.TotalVolumeRange + "'>");
                            html.push(itemData.TotalVolume);
                            html.push("</div>");

                            html.push("<div class='capacityblock allocation-c" + itemData.projectCountRange + "''>");
                            html.push("#" + itemData.ProjectCount);
                            html.push("</div>");
                            html.push("</div>");
                        }
                    }
                    html.push("</div>");
                    html.push("</div>");

                    itemElement.attr("class", "allocation-block allocation-r" + itemData.AllocationRange);
                    itemElement.attr("title", itemData.JobTitle);
                    itemElement.append(html.join(""));

                    $("#loadpanel").dxLoadPanel({
                        message: "Loading...",
                        visible: false
                    });
                },
                onItemClick: function (e) {
                    //debugger;
                    var data = e.itemData;

                    var customDialogCnf = DevExpress.ui.dialog.custom({
                        title: "Alert",
                        message: "Replace <i>" + popupFilters.OldAssignedToUserName +  "</i> with  <i>" + data.AssignedToName + "</i> on <i>" + projectID + " : " + popupFilters.projectTitle + "</i> from",
                        buttons: [
                            { text: new Date().format('MMM dd, yyyy') + " to " + new Date(popupFilters.allocationEndDate).format('MMM dd, yyyy') , onClick: function () { return "Option1" } },
                            { text: popupFilters.startDate.format('MMM dd, yyyy') + " to " + popupFilters.endDate.format('MMM dd, yyyy') , onClick: function () { return "Option2" } },
                            { text: "Cancel", onClick: function () { return "Cancel" } }
                        ]
                    });
                    customDialogCnf.show().done(function (dialogResult) {
                        if (dialogResult == "Option1" || dialogResult == "Option2") {
                            
                            $.get(baseUrl + "/api/RMMAPI/AllocateConflictingResource?userid=" + data.AssignedTo + "&id=" + popupFilters.ID + "&projectid=" + projectID + "&PTOid=" + popupFilters.PTOid + "&opt=" + dialogResult, function (ytddata) {

                                $('#popupContainerConflicts').dxPopup('instance').hide();
                                $("#popupConflicts").dxPopup(popupConflictsOptions).dxPopup("instance").hide();
                                BindAllocationsCnf();
                            });
                        }
                        else if (dialogResult == "Cancel") {
                            return false;
                        }
                        else
                            return false;
                    });
                    
                },
                onInitialized: function (e) {
                    $("#loadpanel").dxLoadPanel({
                        message: "Loading...",
                        visible: true
                    });
                },
                noDataText: function (e) {
                    $('.dx-empty-message').html('No resource available');
                    $("#loadpanel").dxLoadPanel({
                        message: "Loading...",
                        visible: false
                    });
                }
            });
        }

        return titleViewCnf;
    };

    function AllocateResource() {
        var grid = $('#gridTemplateContainer').dxDataGrid('instance');
        grid.option('dataSource', globaldata);
        globaldata = [];
        grid._refresh();

        $('#popupContainerConflicts').dxPopup('instance').hide();
    }
</script>

<div  id="divAllocationConflict" style="cursor:pointer">
</div>
<div id="popupConflicts"></div>

<div id="popupContainerConflicts" style='display: none;' >
</div>
<div id="filterChecks" style='display: none;'>
    <div id="chkCapacity"  style='float: left; display: none;'></div>
    <div id='chkComplexity'  style='float: left;'></div>
    <div id="chkRequestType"  style='float: left;'></div>
    <div id='chkCustomer' title='Customer'  style='float: left;'></div>
    <div id='chkSector'  style='float: left;'></div>
</div>
<div id="dropdownFilters" class="row" style='clear: both; margin-bottom: 0px; display: none;'>
    <div class='filterctrl-jobDepartment'></div>
    <div class='filterctrl-userpicker'></div>
</div>