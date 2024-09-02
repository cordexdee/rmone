<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UnfilledAllocations.ascx.cs" Inherits="uGovernIT.Web.UnfilledAllocations" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .itemMsg {
        font-size: 22px;
        text-align: center;
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
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var baseUrl = ugitConfig.apiBaseUrl;

    var Caption = '<%=Caption%>';
    <%--var UnfilledAllocationType = '<%=UnfilledAllocationType%>';--%>

    var popupFilters = {};
    var projectID = '';
    var globaldata = [];

    var radioGroupItems = [
        { text: "Fully Available", value: 0 },
        { text: "Partially Available", value: 1 },
        { text: "All Resources", value: 2 }
    ];

    $("#divUnfilledAllocation").height('<%=Height%>' == '' ? '30px' : '<%=Height%>');
    $("#divUnfilledAllocation").width('<%=Width%>' == '' ? '30px' : '<%=Width%>');

    $.get(baseUrl + "/api/RMMAPI/GetUnfilledAllocations", function (ytddata) {
        var tileDataSource = Object.entries(ytddata);
        var divYTDmargin = $("#divUnfilledAllocation").dxTileView({
            dataSource: tileDataSource,
            showScrollbar: false,
            baseItemHeight: 70,
            baseItemWidth: 250,
            //itemMargin: 32,
            direction: "vertical",
            width: 250,
            height: 70,
            itemTemplate: function (itemData, itemIndex, itemElement) {
                //debugger;
                var item = itemData[1];

                html = new Array();
                if (item.totalAlloc <= 0)  //totalAllocThisWeek
                    html.push("<div class='strip' style='height: 6px;background-color:lightgreen;margin-bottom:10px;'></div>");
                else
                    html.push("<div class='strip' style='height: 6px;background-color:#ffa5a1;margin-bottom:10px;'></div>");

                html.push("<div class='itemMsg'>");
                html.push(item.totalAlloc + ' ' + Caption);
                html.push("</div>");
                itemElement.append(html.join(""));
            },
            onItemClick: function (e) {
                var popupUnfilled = $("#popupUnfilled").dxPopup(popupUnfilledOptions).dxPopup("instance");
                popupUnfilled.show();
            }
        });
    });

    var popupUnfilledOptions = {
        width: 850,
        height: 500,
        contentTemplate: function () {
            return $("<div />").append(
                $("<Div />").dxDataGrid({
                    showColumnLines: false,
                    showRowLines: true,
                    rowAlternationEnabled: true,
                    showBorders: true,
                    dataSource: "/api/rmmapi/GetOtherAllocationDetails?AllocationType=unfilled",
                    grouping: {
                        autoExpandAll: true,
                    },
                    paging: {
                        pageSize: 10
                    },
                    pager: {
                        visible: false,
                    },
                    groupPanel: {
                        visible: false
                    },
                    columns: [
                        {
                            dataField: "TicketID",
                            caption: "Item ID",
                            groupIndex: 0,
                            width: 150
                        },
                        {
                            dataField: "Title",
                            caption: "Project",
                            width: 200
                        },
                        {
                            dataField: "RoleName",
                            caption: "Role",
                            width: 200
                        },
                        {
                            dataField: "AllocationStartDate",
                            caption: "Start Date",
                            format: 'MMM d, yyyy',
                            dataType: "date",
                            width: 150
                        },
                        {
                            dataField: "AllocationEndDate",
                            caption: "End Date",
                            format: 'MMM d, yyyy',
                            dataType: "date",
                            width: 150
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
                                    .append($("<img>", { "src": "/content/images/moreoptions_blue.png", "Title": "Find Resource", "ticketid": options.data.TicketID, "pctallocation": options.data.PctAllocation, "ID": options.data.ID, "group": options.data.Type, "startDate": options.data.AllocationStartDate, "endDate": options.data.AllocationEndDate, "style": "float: right;overflow: auto;cursor: pointer;", "class": "assigneeToImg" }))
                                    .appendTo(container);
                            }
                        }
                    ]
                })
            );
        },
        showTitle: true,
        title: "Unfilled Allocations",
        dragEnabled: true,
        hideOnOutsideClick: false
    };


    $(document).on("click", "img.assigneeToImg", function (e) {
        var groupid = $(this).attr("id");
        var dataid = e.target.id;
        var data = _.find(globaldata, function (s) { return s.ID.toString() === dataid; });
        projectID = $(this).attr("ticketid");
        popupFilters.projectID = projectID;
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

        $("#popupContainer").dxPopup({
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
                                bindDatapopup(popupFilters);
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
                                        bindDatapopup(popupFilters);
                                        e.component.option("hint", "Show Experience");
                                    } else {
                                        popupFilters.projectVolume = true;
                                        popupFilters.projectCount = true;
                                        bindDatapopup(popupFilters);
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
                                        bindDatapopup(popupFilters);
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
                                        bindDatapopup(popupFilters);
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
                                    var popup = $('#popupContainer').dxPopup('instance');
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


                                    bindDatapopup(popupFilters);
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
                                    bindDatapopup(popupFilters);
                                },
                            }),


                            $("<div id='chkCustomer' title='Customer' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                text: 'Customer',
                                value: popupFilters.Customer,
                                hint: 'Customer',
                                onValueChanged: function (e) {
                                    popupFilters.Customer = e.value;
                                    bindDatapopup(popupFilters);
                                },
                            }),

                            $("<div id='chkSector' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                text: 'Sector',
                                hint: 'Sector',
                                value: popupFilters.Sector,
                                onValueChanged: function (e) {
                                    popupFilters.Sector = e.value;
                                    bindDatapopup(popupFilters);
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

                                    bindDatapopup(popupFilters);
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
                                    bindDatapopup(popupFilters);
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
                                        bindDatapopup(popupFilters);
                                    } else {
                                        var items = selectedItems.selectedItem.Id;
                                        popupFilters.SelectedUserID = items;
                                        popupFilters.projectID = projectID;
                                        popupFilters.complexity = false;
                                        bindDatapopup(popupFilters);
                                    }
                                }
                            }),
                        )
                    ),

                    bindDatapopup(popupFilters)
                )

            },
            itemClick: function (e) {
            }
        });

        var popupInstance = $('#popupContainer').dxPopup('instance');
        popupInstance.option('title', popupTitle);
    });

    function bindDatapopup(popupFilters) {
        var titleView = null;
        if (typeof (popupFilters.allocationStartDate) == "object") {
            if (popupFilters.allocationStartDate != null)
                popupFilters.allocationStartDate = (popupFilters.allocationStartDate).toISOString();
        }

        if (typeof (popupFilters.allocationEndDate) == "object") {
            if (popupFilters.allocationEndDate != null)
                popupFilters.allocationEndDate = (popupFilters.allocationEndDate).toISOString();
        }

        if ($("#tileViewContainer").length > 0) {

            var titleViewObj = $('#tileViewContainer').dxTileView('instance');
            if (titleViewObj) {
                titleViewObj.option("dataSource", "/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters));
                titleViewObj._refresh();
            }
        }
        else {

            titleView = $("<div id='tileViewContainer' style='clear:both;padding-bottom:100px;' />").dxTileView({
                height: window.innerHeight - 120,
                width: window.innerWidth - 150,
                showScrollbar: true,
                baseItemHeight: 65,
                baseItemWidth: 150,
                itemMargin: 15,
                direction: "vertical",
                elementAttr: { "class": "tileViewContainer" },
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
                    var data = e.itemData;
                    var element = _.findWhere(globaldata, { ID: parseInt(popupFilters.ID) });
                    element.AssignedTo = data.AssignedTo;
                    element.AssignedToName = data.AssignedToName;
                    element.AllocationEndDate = popupFilters.allocationEndDate;
                    element.AllocationStartDate = popupFilters.allocationStartDate;
                    element.PctAllocation = popupFilters.pctAllocation;

                    let obj = globaldata.find(o => o.AssignedTo == element.AssignedTo && o.Type == element.Type && element.AllocationStartDate <= o.AllocationEndDate && element.AllocationEndDate >= o.AllocationStartDate && o.ID > 0);;
                    if (obj != null && obj != undefined) {
                        var customDialog = DevExpress.ui.dialog.custom({
                            title: "Alert",
                            message: "An overlapping allocation exists for this resource and role for this project. Do you want to proceed?",
                            buttons: [
                                { text: "Ok", onClick: function () { return "Ok" } },
                                { text: "Cancel", onClick: function () { return "Cancel" } }
                            ]
                        });
                        customDialog.show().done(function (dialogResult) {
                            if (dialogResult == "Ok") {
                                AllocateResource();
                            }
                            else if (dialogResult == "Cancel") {
                                return false;
                            }
                            else
                                return false;
                        });

                        return false;
                    }
                    else {
                        //don't need to change Assignee when role is change, user is playing selected role in current project.
                        //element.Type = data.GroupID;
                        //var roledata = _.findWhere(GroupsData, { Id: data.GroupID });
                        //if(typeof roledata.Name !== "undefined")
                        //    element.TypeName = roledata.Name;

                        //var grid = $('#gridTemplateContainer').dxDataGrid('instance');
                        //grid.option('dataSource', globaldata);
                        //globaldata = [];
                        //grid._refresh();

                        //$('#popupContainer').dxPopup('instance').hide();

                        AllocateResource();
                    }
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

    function AllocateResource() {
        var grid = $('#gridTemplateContainer').dxDataGrid('instance');
        grid.option('dataSource', globaldata);
        globaldata = [];
        grid._refresh();

        $('#popupContainer').dxPopup('instance').hide();
    }
</script>

<div id="divUnfilledAllocation">
</div>
<div id="popupUnfilled"></div>
<div id="popupContainer">
</div>
<div id="filterChecks">
    <div id="chkCapacity"  style='float: left; display: none;'></div>
    <div id='chkComplexity'  style='float: left;'></div>
    <div id="chkRequestType"  style='float: left;'></div>
    <div id='chkCustomer' title='Customer'  style='float: left;'></div>
    <div id='chkSector'  style='float: left;'></div>
</div>
<div id="dropdownFilters" class="row" style='clear: both; margin-bottom: 0px;'>
    <div class='filterctrl-jobDepartment'></div>
    <div class='filterctrl-userpicker'></div>
</div>