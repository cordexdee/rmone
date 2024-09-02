<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UnfilledPipelineAllocations.ascx.cs" Inherits="uGovernIT.Web.UnfilledPipelineAllocations" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #divUnfilledPipelineAllocation .dx-tile {
        position: static; 
        border: none;
    }

    .unfilledPipelineItemMsg {
        font-size: 12px;
        text-align: center;
        font-family: 'Poppins', sans-serif;
    }

    .tileViewContainerppl .dx-tile {
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

    #tileViewContainerppl .dx-empty-message {
        text-align: center;
        padding-top: 62px;
    }

    #tileViewContainerppl .capacityblock {
        float: left;
        width: 74px;
        text-align: center;
        height: 20px;
    }

        #tileViewContainerppl .capacityblock:first-child {
            border-right: 1px solid #c3c3c3;
        }

    #tileViewContainerppl .allocation-v0 {
        background: #ffffff;
    }

    #tileViewContainerppl .allocation-v1 {
        background: #fcf7b5;
    }

    #tileViewContainerppl .allocation-v2 {
        background: #f8ac4a;
    }

    #tileViewContainerppl .dx-tile {
        border: none; 
    }

    #tileViewContainerppl .allocation-r4 {
        background: #ff0d0d;
        color: #e8e6e6;
    }

     #tileViewContainerppl .allocation-r0 {
        background: #57A71D;
        color: #fff;
    }

    #tileViewContainerppl .allocation-r1 {
        background: #A9C23F;
        color: #000;
    }

    #tileViewContainerppl .allocation-r2 {
        background: #FFC100;
        color: #000;
    }

/*    #tileViewContainer .allocation-r3 {
        background: #f8ac4a;
    }
*/
    #tileViewContainerppl .allocation-r3 {
        background: #FF3535;
        color: #ffffff;
    }

    #tileViewContainerppl .allocation-c0 {
        background: #ffffff;
    }

    #tileViewContainerppl .allocation-c1 {
        background: #fcf7b5;
    }

    #tileViewContainerppl .allocation-c2 {
        background: #f8ac4a;
    }

    #tileViewContainerppl .allocation-block {
        height: 63px;
        display: flex;
        justify-content: center;
        align-items: center;
        border-radius:7px;
    }

        #tileViewContainerppl .allocation-block .timesheet {
            position: absolute;
            top: 1px;
            left: 85%;
            cursor: pointer;
        }

    #tileViewContainerppl .dx-tile {
        border: none;
    }

    #tileViewContainerppl .capacitymain {
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

    .dx-popup-content .dx-datagrid{
        max-height:500px;
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
    #divUnfilledPipelineAllocation .dx-tile {
        color:grey !important;
    }
    .unfilledItem {
        border: 3px solid #57a71d;
    }
    #divUnfilledPipelineAllocation .dx-empty-message {
        margin-top: 0px !important;
        font-weight: unset !important;
        color: #808080;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var baseUrl = ugitConfig.apiBaseUrl;

    var CaptionPpn = '<%=Caption%>';
    <%--var UnfilledAllocationType = '<%=UnfilledAllocationType%>';--%>

    var ShowByUsersDivisionPpl = '<%=ShowByUsersDivision%>';

    var popupFilters = {};
    var projectID = '';
    var globaldata = [];

    var radioGroupItems = [
        { text: "Fully Available", value: 0 },
        { text: "Partially Available", value: 1 },
        { text: "All Resources", value: 2 }
    ];

    $("#divUnfilledPipelineAllocation").height('<%=Height%>' == '' ? '30px' : '<%=Height%>');
    $("#divUnfilledPipelineAllocation").width('<%=Width%>' == '' ? '30px' : '<%=Width%>');

    BindAllocationsPpl();

    function BindAllocationsPpl() {
        $.get(baseUrl + "/api/RMMAPI/GetOtherAllocationDetails?AllocationType=unfilledPipeline&ResultType=count&ShowByUsersDivision=" + ShowByUsersDivisionPpl, function (ytddata) {
            var tileDataSource = Object.entries(ytddata);
            var divYTDmargin = $("#divUnfilledPipelineAllocation").dxTileView({
                dataSource: tileDataSource,
                showScrollbar: false,
                baseItemHeight: 100,
                baseItemWidth: 100,
                itemMargin: 0,
                direction: "vertical",
                width: 100,
                height: 100,
                noDataText: "<div class='strip unfilledItem'>0</div><div class='unfilledPipelineItemMsg'>Unfilled<br>Pipeline/Tracked Work</div>",
                itemTemplate: function (itemData, itemIndex, itemElement) {
                    var item = itemData[1];

                    html = new Array();
                    if(item.Count <= 0)
                        html.push(`<div class='strip unfilledItem'>${item.Count}</div>`);
                    else
                        html.push(`<div class='strip unfilledItem'>${item.Count}</div>`);

                    html.push("<div class='unfilledPipelineItemMsg'>");
                    html.push("Unfilled");
                    html.push("<br>");
                    html.push("Pipeline/Tracked Work");
                    html.push("</div>");
                    itemElement.append(html.join(""));
                },
                onItemClick: function (e) {
                    var popupUnfilledPipeline = $("#popupUnfilledPipeline").dxPopup(popupUnfilledPipelineOptions).dxPopup("instance");
                    popupUnfilledPipeline.show();
                }
            });
        });
    }

    var popupUnfilledPipelineOptions = {
        width: 950,
        height: 600,
        contentTemplate: function () {
            return $("<div />").append(
                $("<Div />").dxDataGrid({
                    showColumnLines: false,
                    showRowLines: true,
                    rowAlternationEnabled: true,
                    showBorders: true,
                    dataSource: "/api/rmmapi/GetOtherAllocationDetails?AllocationType=unfilledPipeline&ResultType=records&ShowByUsersDivision=" + ShowByUsersDivisionPpl,
                    grouping: {
                        autoExpandAll: true,
                    },
                    paging: {
                        pageSize: 10,
                        enabled: false
                    },
                    pager: {
                        visible: false,
                    },
                    groupPanel: {
                        visible: false
                    },
                    columns: [
                        {
                            dataField: "Project",
                            caption: "Item",
                            width: 400
                        },
                        {
                            dataField: "RoleName",
                            groupIndex: 0,
                            caption: "Role",
                            width: 150
                        },
                        {
                            dataField: "AllocationStartDate",
                            caption: "Start Date",
                            format: 'MMM d, yyyy',
                            dataType: "date",
                            width: 200
                        },
                        {
                            dataField: "AllocationEndDate",
                            caption: "End Date",
                            format: 'MMM d, yyyy',
                            dataType: "date",
                            width: 200
                        }
                        ,
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
                                    .append($("<img>", { "src": "/content/images/moreoptions_blue.png", "Title": "Find Resource", "ticketid": options.data.TicketID, "projectTitle": options.data.Title, "pctallocation": options.data.PctAllocation, "ID": options.data.ID, "group": options.data.Type, "rolename": options.data.RoleName, "startDate": options.data.AllocationStartDate, "endDate": options.data.AllocationEndDate, "style": "float: right;overflow: auto;cursor: pointer;", "class": "assigneeToImgPln" }))
                                    .appendTo(container); 
                            }
                        }
                    ]
                })
            );
        },
        showTitle: true,
        title: CaptionPpn,
        dragEnabled: true,
        hideOnOutsideClick: false
    };


    $(document).on("click", "img.assigneeToImgPln", function (e) {
        

        var groupid = $(this).attr("id");
        var dataid = e.target.id;
        var data = _.find(globaldata, function (s) { return s.ID.toString() === dataid; });
        projectID = $(this).attr("ticketid");
        popupFilters.ID = dataid;
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
        popupFilters.allocationStartDate = new Date($(this).attr("startDate"));
        popupFilters.allocationEndDate = new Date($(this).attr("endDate"));
        popupFilters.pctAllocation = $(this).attr("pctallocation"); //data.pctAllocation;
        popupFilters.isAllocationView = false;
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

        $("#popupContainerPipeline").dxPopup({
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
                                bindDatapopupPpl(popupFilters);
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
                                        bindDatapopupPpl(popupFilters);
                                        e.component.option("hint", "Show Experience");
                                    } else {
                                        popupFilters.projectVolume = true;
                                        popupFilters.projectCount = true;
                                        bindDatapopupPpl(popupFilters);
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
                                    value: popupFilters.allocationStartDate,
                                    displayFormat: 'MMM d, yyyy',
                                    onValueChanged: function (data) {

                                        popupFilters.allocationStartDate = data.value;
                                        var endDateObj = $("#dtEndDate").dxDateBox("instance");
                                        if (typeof endDateObj != "undefined") {
                                            var enddate = endDateObj.option('value');
                                            if (new Date(enddate) < new Date(data.value)) {
                                                popupFilters.allocationEndDate = popupFilters.allocationStartDate;
                                                endDateObj.option('value', popupFilters.allocationStartDate);
                                            }
                                        }
                                        bindDatapopupPpl(popupFilters);
                                    }

                                })

                            ),

                            $("<div class='col-md-2 col-sm-4 col-xs-12 pb-3 noPadd1'>").append(
                                $("<label class='lblStartDate' style='width:100%;'>End Date</label>"),
                                $("<div id='dtEndDate' class='chkFilterCheck' style='width:100%;' />").dxDateBox({
                                    type: "date",
                                    value: popupFilters.allocationEndDate,
                                    displayFormat: 'MMM d, yyyy',
                                    onValueChanged: function (data) {

                                        popupFilters.allocationEndDate = data.value;
                                        var startDateObj = $("#dtStartDate").dxDateBox("instance");
                                        if (typeof startDateObj != "undefined") {
                                            var startdate = startDateObj.option('value');
                                            if (new Date(startdate) > new Date(data.value)) {
                                                popupFilters.allocationStartDate = popupFilters.allocationEndDate;
                                                startDateObj.option('value', popupFilters.allocationEndDate);
                                            }
                                        }
                                        bindDatapopupPpl(popupFilters);
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
                                    var popup = $('#popupContainerPipeline').dxPopup('instance');
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


                                    bindDatapopupPpl(popupFilters);
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
                                    bindDatapopupPpl(popupFilters);
                                },
                            }),


                            $("<div id='chkCustomer' title='Customer' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                text: 'Customer',
                                value: popupFilters.Customer,
                                hint: 'Customer',
                                onValueChanged: function (e) {
                                    popupFilters.Customer = e.value;
                                    bindDatapopupPpl(popupFilters);
                                },
                            }),

                            $("<div id='chkSector' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                text: 'Sector',
                                hint: 'Sector',
                                value: popupFilters.Sector,
                                onValueChanged: function (e) {
                                    popupFilters.Sector = e.value;
                                    bindDatapopupPpl(popupFilters);
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

                                    bindDatapopupPpl(popupFilters);
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
                                    bindDatapopupPpl(popupFilters);
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
                                        bindDatapopupPpl(popupFilters);
                                    } else {
                                        var items = selectedItems.selectedItem.Id;
                                        popupFilters.SelectedUserID = items;
                                        popupFilters.projectID = projectID;
                                        popupFilters.complexity = false;
                                        bindDatapopupPpl(popupFilters);
                                    }
                                }
                            }),
                        )
                    ),

                    bindDatapopupPpl(popupFilters)
                )

            },
            itemClick: function (e) {
            }
        });

        var popupInstance = $('#popupContainerPipeline').dxPopup('instance');
        popupInstance.option('title', popupTitle);
    });

    function bindDatapopupPpl(popupFilters) {
        var titleView = null;
        if (typeof (popupFilters.allocationStartDate) == "object") {
            if (popupFilters.allocationStartDate != null)
                popupFilters.allocationStartDate = (popupFilters.allocationStartDate).toISOString();
        }

        if (typeof (popupFilters.allocationEndDate) == "object") {
            if (popupFilters.allocationEndDate != null)
                popupFilters.allocationEndDate = (popupFilters.allocationEndDate).toISOString();
        }

        if ($("#tileViewContainerppl").length > 0) {

            var titleViewObj = $('#tileViewContainerppl').dxTileView('instance');
            if (titleViewObj) {
                titleViewObj.option("dataSource", "/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters));
                titleViewObj._refresh();
            }
        }
        else {

            titleView = $("<div id='tileViewContainerppl' style='clear:both;padding-bottom:100px;' />").dxTileView({
                height: window.innerHeight - 120,
                width: window.innerWidth - 150,
                showScrollbar: true,
                baseItemHeight: 65,
                baseItemWidth: 150,
                itemMargin: 15,
                direction: "vertical",
                elementAttr: { "class": "tileViewContainerppl" },
                noDataText: "No resource available",
                dataSource: "/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters),
                itemTemplate: function (itemData, itemIndex, itemElement) {
                    itemData.PctAllocation = Math.round(itemData.PctAllocation);
                    var html = new Array();
                    var str = itemData.AssignedTo + "','" + itemData.AssignedToName;
                    var strwithspace = str.replace(/\s/g, '&nbsp;'); //str.replace(" ", "&nbsp;")
                    html.push("<div class='timesheet'><img src='/content/images/icon_three_black_dots.png' height='5px' title='Allocation Timeline' onclick=openResourceTimeSheet('" + strwithspace + "'); />");
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

        return titleView;
    };
    function openResourceTimeSheet(assignedTo, assignedToName) {
        var url = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGridNew&ViewName=FindAvailability&isRedirectFromCardView=true&selecteddepartment=-1&SelectedResource=" + assignedTo;
        window.parent.UgitOpenPopupDialog(url, "", "Timeline for User : " + assignedToName, "95", "95", "", false);
        $(".ui-dialog-titlebar").parent().css('z-index', 1505);
        $(".ui-dialog-titlebar-close").on("click", function(){
            $(".dx-popup-content").each(function(){ $(this).scrollTop(0,0)});
        });
    }
    function AllocateResource() {
       var grid = $('#gridTemplateContainer').dxDataGrid('instance');
        grid.option('dataSource', globaldata);
        globaldata = [];
        grid._refresh();
        $('#popupContainerPipeline').dxPopup('instance').hide();
    }
</script>

<div id="divUnfilledPipelineAllocation" style="cursor:pointer">
</div>
<div id="popupUnfilledPipeline"></div>
<div id="popupContainerPipeline" style='display: none;' >
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