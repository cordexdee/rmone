<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PTOMultiAllocationsJS.ascx.cs" Inherits="uGovernIT.Web.PTOMultiAllocationsJS" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .dx-datagrid-revert-tooltip {
        display: none;
    }

    #data-grid-demo {
        /*min-height: 500px;*/
    }

    #gridDeleteSelected {
        position: absolute;
        z-index: 1;
        right: 0;
        margin: 1px;
        top: 0;
    }

        #gridDeleteSelected .dx-button-text {
            line-height: 0;
        }

    .preconBG {
        background-color: #52BED9 !important;
        color: white;
    }
    .constBG {
        background-color: #005C9B !important;
        color: white;
    }
    .closeoutBG {
        background-color: #351B82 !important;
        color: white;
    }
    #lblConStart{
        color:#52BED9 !important;
    }
    #lblConstructionStart{
        color: #005C9B !important;
    }
    #lblCloseOut{
        color: #351B82 !important;
    }
    .btnAddNew .dx-icon {
    filter:brightness(10);
}
    #btnAddNewMultiAllocation .dx-icon {
    transform: scale(1.5);
    filter:none;
    }
    .btnAddNew.dx-button, .grid-template-container, .history_date_time {
    font-family: 'Roboto', sans-serif;
    font-size: 14px;
    font-weight: 500;
    letter-spacing: 0.01rem;
    }
</style>


<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var workItemData = [];
    var strSelectedIds = "";
    var flag = false;
    var allocationData = [];
    var GroupsData = [];
    var userRoleId = '';
    var ddlTypeSelectedType = "";
    var distinctSubCategoriesarray = [];

    $(document).ready(function () {

        var userID = "<%= UserID %>";

        if (userID) {
            var typeDataSource = JSON.parse($("[id$='hdnLstType']").val());
            $("#dvManagerView").show();
            $("#dvNonManagerView").hide();
            $("#ddlType").dxSelectBox({
                items: typeDataSource,
                displayExpr: "LevelTitle",
                valueExpr: "LevelName",
                placeholder: "Select a Type...",
                searchEnabled: true,
                searchExpr: "LevelName",
                searchMode: 'Contains',
                searchTimeout: 500,
                showClearButton: false,
                activeStateEnabled: true,
                onSelectionChanged: function (selectedItem) {
                    var tennantID = "<%= TennantID %>";
                    ddlTypeSelectedType = selectedItem.selectedItem.LevelName;
                    var URL = "<%= ajaxPageURL %>/GetddlLevel2Data?SelectedItem=" + selectedItem.selectedItem.LevelTitle + "&TennantID=" + tennantID + "";
                    var ddlWorkItemDataSource = $("#ddlWorkItem").dxSelectBox("instance");
                    ddlWorkItemDataSource.option("dataSource", []);

                    $.ajax({
                        type: "Get",
                        url: URL,
                        contentType: "application/json; charset=utf-8",
                        dataType: "JSON",
                        success: function (data) {
                            ddlWorkItemDataSource.option("dataSource", data);
                            ddlWorkItemDataSource._refresh();
                            workItemData = data;
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(JSON.stringify(xhr));
                        }
                    });
                }
            });
        }
        else {
            var dataSource = JSON.parse($("[id$='hdnlstUserProfiles']").val());
            dataSource = sortResults(dataSource, 'Name', true);
            $("#dvManagerView").hide();
            $("#dvNonManagerView").show();
            $("#userDropDown").dxSelectBox({
                items: dataSource,
                displayExpr: "Name",
                valueExpr: "Id",
                placeholder: "Select a User Name...",
                searchEnabled: true,
                searchExpr: "Name",
                searchMode: 'Contains',
                searchTimeout: 500,
                showClearButton: true,
                activeStateEnabled: true,
                onSelectionChanged: function (data) {
                   
                    userRoleId = data.selectedItem.GlobalRoleId;
                    allocationData.forEach(function (item, index) {
                        if (item.RoleId == null || item.RoleId == '') { item.RoleId = userRoleId };
                    });
                    var grid = $("#gridMultiAllocationCrud").dxDataGrid("instance");
                    grid.option("dataSource", allocationData);
                }
            });
        }

        $("#ddlSubCategory").dxSelectBox({
            placeholder: 'Select Sub WorkItem',
            dataSource: distinctSubCategoriesarray,
            visible: false,
            onSelectionChanged: function (data) {
                debugger;
                let levelTitleToFilter = data.selectedItem;
                allocationData.WorkItem = levelTitleToFilter;
            }
        });

        const loadPanel = $('#loadpanel').dxLoadPanel({
            shadingColor: 'rgba(0,0,0,0.4)',
            visible: false,
            showIndicator: true,
            showPane: true,
            shading: true,
            hideOnOutsideClick: false,
        }).dxLoadPanel('instance');

        function sortResults(data, prop, asc) {
            data.sort(function (a, b) {
                if (asc) {
                    return (a[prop] > b[prop]) ? 1 : ((a[prop] < b[prop]) ? -1 : 0);
                } else {
                    return (b[prop] > a[prop]) ? 1 : ((b[prop] < a[prop]) ? -1 : 0);
                }
            });
            return data;
        }


        $('#ddlWorkItem').dxSelectBox({
            valueExpr: 'LevelText',
            placeholder: 'Select a Workitem...',
            displayExpr: 'LevelText',
            dataSource: workItemData,
            onSelectionChanged: function (data) {
                let levelTitleToFilter = data.selectedItem.LevelTitle;
                allocationData.WorkItem = levelTitleToFilter;
                let filteredArray = workItemData.filter(obj => obj.LevelTitle === levelTitleToFilter);

                let distinctSubCategories = new Set(filteredArray.map(obj => obj.SubCategory).filter(subCat => subCat !== null));

                distinctSubCategoriesarray = Array.from(distinctSubCategories);
                if (distinctSubCategoriesarray.length > 0) {

                    var ddlSubCategory = $("#ddlSubCategory").dxSelectBox("instance");
                    ddlSubCategory.option("visible", true);
                    ddlSubCategory.option("dataSource", distinctSubCategoriesarray);

                } else {
                    var ddlSubCategory = $("#ddlSubCategory").dxSelectBox("instance");
                    ddlSubCategory.option("visible", false);

                }
            }
        });



        $.get("/api/rmmapi/GetProjectAllocations?projectID=<%= ticketID %>", function (data, status) {
            GroupsData = data.Roles;
            LoadMultiAllocationGrid();
            $("#btnAddNewMultiAllocation").click();
        });

        $(document).on("click", '.deleteRow', function () {
            var key = $(this).attr("key");
            var grid = $("#gridMultiAllocationCrud").dxDataGrid("instance");
            allocationData = jQuery.grep(allocationData, function (objEmp) {
                return objEmp.key != key;
            });
            grid.option("dataSource", allocationData);
        });


        $("#btnAddNewMultiAllocation").dxButton({
            text: "Add New",
            icon: "/content/Images/plus-blue-new.png",
            focusStateEnabled: false,
            onClick: function (e) {
                
                var grid = $("#gridMultiAllocationCrud").dxDataGrid("instance");
                var counter = allocationData.length + 1;
                allocationData.push({ "key": Math.abs(counter), "AllocationStartDate": "", "AllocationEndDate": "", "RoleId": getUserRoleId(), "PctAllocation": "" });
                grid.option("dataSource", allocationData);

            }
        });

        $("#btnSaveMultiAllocation").dxButton({
            text: "Save  Allocatiions",
            icon: "/content/Images/save-open-new-wind.png",
            focusStateEnabled: false,
            onClick: function (e) {
                var userID = "<%= UserID %>";
                var workItem = "<%= WorkItem %>";
                
                var lstMultiAllocations = [];
                var dataGrid = $("#gridMultiAllocationCrud").dxDataGrid("instance");
                var rows = dataGrid.getVisibleRows();
                rows.forEach(function (item, index) {
                    var data = item.data;
                    var RoleData = GroupsData.find(x => x.Id == data.RoleId);
                    var objMultiAllocations = {};
                    objMultiAllocations.key = data.key;
                    
                    if (workItem) {
                        objMultiAllocations.WorkItem = workItem;
                        objMultiAllocations.WorkItemType = "<%= ModuleName %>";
                        objMultiAllocations.ModuleName = "<%= ModuleName %>";
                        objMultiAllocations.UserID = $("#userDropDown").dxSelectBox('instance').option('value');
                        if (objMultiAllocations.UserID == null || objMultiAllocations.UserID == '') {
                            DevExpress.ui.dialog.alert("Select User.", "Error");
                            return;
                        }
                      
                    } else {
                        objMultiAllocations.WorkItemType = $("#ddlType").dxSelectBox('instance').option('value');
                        objMultiAllocations.ModuleName = $("#ddlType").dxSelectBox('instance').option('value');
                        objMultiAllocations.WorkItem = allocationData.WorkItem; //  $("#ddlWorkItem").dxSelectBox('instance').option('text');
                        objMultiAllocations.UserID = "<%=UserID%>";
                        if (!objMultiAllocations.WorkItemType || !objMultiAllocations.WorkItem) {
                            DevExpress.ui.dialog.alert("Select both Type and WorkItem.", "Error");
                            return;
                        }
                    }

                    if (!data.AllocationStartDate || data.AllocationStartDate == '') {
                        DevExpress.ui.dialog.alert("Start Date should not be blank.", "Error!");
                        return;
                    }
                    if (!data.AllocationEndDate || data.AllocationEndDate == '') {
                        DevExpress.ui.dialog.alert("End Date should not be blank.", "Error!");
                        return;
                    }
                    if (data.RoleId == null || data.RoleId == '' || typeof (data.RoleId) == "undefined") {
                        DevExpress.ui.dialog.alert("Role Id should not be blank.", "Error!");
                        return;
                    }
                    const pctAllocation = parseInt(data.PctAllocation)
                    if (isNaN(pctAllocation) || pctAllocation < 0) {
                        DevExpress.ui.dialog.alert("% Allocation should not be blank  or zero.", "Error!");
                        return;
                    }

                    objMultiAllocations.StartDate = data.AllocationStartDate;
                    objMultiAllocations.EndDate = data.AllocationEndDate;
                    objMultiAllocations.Role = RoleData.Id;
                    objMultiAllocations.RoleName = RoleData.Name;
                    objMultiAllocations.PctAllocation = data.PctAllocation;
                    objMultiAllocations.SoftAllocation = data.SoftAllocation;
                    objMultiAllocations.NonChargeable = data.NonChargeable;

                    lstMultiAllocations.push(objMultiAllocations);

                });

                if (lstMultiAllocations.length > 0) {
                    loadPanel.show();
                    $.ajax({
                        type: "POST",
                        url: "<%= ajaxPageURL %>/SaveAllocationAs",
                        data: JSON.stringify(lstMultiAllocations),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (message) {
                            
                            loadPanel.hide();
                            if (message.startsWith("Overlapping Allocations")) {
                                
                                let overlappingID = parseInt(message.split("~")[1]);
                                var gridObj = $("#gridMultiAllocationCrud").dxDataGrid("instance");
                                var rowIndex = gridObj.getRowIndexByKey(overlappingID); 
                                DevExpress.ui.dialog.alert("Overlapping allocations are not permitted. Save unsuccessful.", "Error");
                                gridObj.getRowElement(rowIndex).css("background-color", "#FFCCCB");
                            }
                            if (message == "Record updated successfully") {
                                DevExpress.ui.dialog.alert("Allocations Saved Successfully. ", "Error");
                                //window.location.reload();

                                var sourceURL = "<%= Request["source"] %>";
                                sourceURL += "**refreshDataOnly";
                                window.parent.CloseWindowCallback(1, sourceURL);
                            }
                            if (message == "Error occured") {

                            }                           
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            DevExpress.ui.dialog.alert("Save unsuccessful.", "Error");
                            loadPanel.hide();
                            console.log(JSON.stringify(xhr));
                        }
                    });
                }
            }
        });


        function LoadMultiAllocationGrid() {
            const allocationStore = new DevExpress.data.ArrayStore({
                key: 'key',
                data: allocationData,
            });
            const dataGrid = $('#gridMultiAllocationCrud').dxDataGrid({
                dataSource: allocationStore,
                showBorders: true,
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
                showBorders: true,
                showRowLines: true,
                repaintChangesOnly: true,
                toolbar: function (e) {
                    e.toolbarOptions.visible = false;
                },
                //selection: {
                //    mode: 'multiple',
                //    showCheckBoxesMode: 'always',
                //},
                columns: [
                    {
                        dataField: 'key',
                        dataType: 'number',
                        visible:false,
                    },
                    {
                        caption: 'Start Date',
                        dataField: 'AllocationStartDate',
                        dataType: 'date',
                        validationRules: [{ type: "required", message: '', }],
                        alignment:'center'
                    },
                    {
                        caption: 'End Date',
                        dataField: 'AllocationEndDate',
                        dataType: 'date',
                        validationRules: [{ type: "required", message: '', }],
                        alignment: 'center'
                    },

                    //{
                    //    dataField: 'RoleId',
                    //    caption: 'Role',
                    //    width: 250,
                    //    validationRules: [{ type: "required", message: '', }],
                    //    lookup: {
                    //        dataSource: GroupsData,
                    //        displayExpr: 'Name',
                    //        valueExpr: "Id"
                    //    },
                    //    onSelectionChanged: function (data) {
                    //        //console.log(data);
                    //    }
                    //},
                    {
                        caption: '% Alloc',
                        dataField: 'PctAllocation',
                        //validationRules: [{ type: "required", message: '', }],
                        width: "80px",
                    },
                    {
                        width: "80px",
                        fieldName: "SoftAllocation",
                        name: 'SoftAllocation',
                        caption: "",
                        dataType: "text",
                        visible: false,  
                        cellTemplate: function (container, options) {
                            $("<div id='divSoftAllocation' style='padding-left:10px;' >").append(
                                $("<div id='divSwitch' />").dxSwitch({
                                    switchedOffText: 'Soft',
                                    switchedOnText: 'Hard',
                                    width: 60,
                                    value: options.data.SoftAllocation,
                                    onValueChanged(data) {

                                    },
                                })
                            ).appendTo(container);
                        },

                    },
                    {
                        width: "50px",
                        fieldName: "NonChargeable",
                        caption: 'NCO',
                        dataType: 'text',
                        visible: false,
                        cellTemplate: function (container, options) {
                            $("<div id='divNCO' style='padding-left:10px;' >").append(
                                $("<div id='divSwitchNCO' />").dxCheckBox({
                                    //switchedOffText: 'BillHr',
                                    //switchedOnText: 'NCO',
                                    width: 30,
                                    value: options.data.NonChargeable,
                                    onValueChanged(data) {

                                    },
                                })
                            ).appendTo(container);
                        },
                    },
                    {
                        width: "8%",
                        cellTemplate: function (container, options) {

                            $("<div id='rowDelete' style='text-align:center;'>")
                                .append($("<img>", {
                                    "src": "/content/images/deleteIcon-new.png", "key": options.data.key,
                                    "style": "overflow: auto;cursor: pointer;", "class": "imgDelete", "title": "Delete",
                                    "class": "deleteRow", "width": "22px"
                                }))
                                .appendTo(container);
                        }
                    }
                ],
                toolbar: {
                    items: [
                       
                    ],
                },
                onSelectionChanged(e) {
                    const data = e.selectedRowsData;
                    strSelectedIds = "";
                    $.each(data, function (i, item) {
                        if (item.key)
                            strSelectedIds += item.key + ",";
                    });
                },
                onRowValidating: function (e) {

                    if (typeof e.newData.AllocationEndDate !== "undefined" && typeof e.newData.AllocationStartDate !== "undefined") {
                        if (new Date(e.newData.AllocationEndDate) < new Date(e.newData.AllocationStartDate)) {

                            e.isValid = false;
                            e.errorText = "StartDate should be less then EndDate";
                        }
                    }
                    if (typeof e.oldData.AllocationEndDate !== "undefined" && typeof e.oldData.AllocationStartDate !== "undefined") {
                        if (new Date(e.oldData.AllocationEndDate) < new Date(e.oldData.AllocationStartDate)) {

                            e.isValid = false;
                            e.errorText = "StartDate should be less then EndDate";
                        }
                    }
                    if (typeof e.oldData.AllocationEndDate !== "undefined" || typeof e.newData.AllocationStartDate !== "undefined") {
                        if (new Date(e.oldData.AllocationEndDate) < new Date(e.newData.AllocationStartDate)) {

                            e.isValid = false;
                            e.errorText = "StartDate should be less then EndDate";
                        }
                    }
                    if (typeof e.oldData.AllocationEndDate !== "undefined" || typeof e.newData.AllocationStartDate !== "undefined") {

                        if (new Date(e.newData.AllocationEndDate) < new Date(e.oldData.AllocationStartDate)) {

                            e.isValid = false;
                            e.errorText = "StartDate should be less then EndDate";
                        }
                    }



                    if (typeof e.newData.PctAllocation !== "undefined") {
                        if (parseInt(e.newData.PctAllocation) <= 0) {
                            e.isValid = false;
                            e.errorText = "Cannot make Allocation with 0 Percentage.";
                        }
                        if (!e.newData.PctAllocation) {
                            e.isValid = false;
                            e.errorText = "Cannot make Allocation with 0 Percentage.";
                        }
                    }

                },
                onCellClick: function (e) {
                    
                    if (e.column.fieldName == "SoftAllocation") {
                        allocationData.forEach(function (part, index, theArray) {
                            if (part.ID == e.data.ID) {
                                part.SoftAllocation = !part.SoftAllocation;
                            }
                        });
                    }
                    if (e.column.fieldName == "NonChargeable") {
                        allocationData.forEach(function (part, index, theArray) {
                            if (part.ID == e.data.ID) {
                                part.NonChargeable = !part.NonChargeable;
                            }
                        });
                    }
                    if (e.column.fieldName == "PctAllocation") {
                        allocationData.forEach(function (part, index, theArray) {
                            if (part.ID == e.data.ID) {
                                part.PctAllocation = 100;
                            }
                        });
                    }
                },
            }).dxDataGrid('instance');


        }


        function updateDatesInGrid(startDate, EndDate) {
            
            if (startDate == "Invalid Date") {
                startDate = '';
            }
            if (EndDate == "Invalid Date") {
                EndDate = '';
            }
            flag = true;
            if (strSelectedIds) {
                var Ids = strSelectedIds.split(",");
                var dataGrid = $("#gridMultiAllocationCrud").dxDataGrid("instance");
                var rows = dataGrid.getVisibleRows();
                rows.forEach(function (item, index) {
                    if (Ids.indexOf(item.data.key.toString()) != -1) {
                        dataGrid.cellValue(index, "AllocationStartDate", startDate);
                        dataGrid.cellValue(index, "AllocationEndDate", EndDate);
                    }
                });
                dataGrid.clearSelection();
            }
            else {

                $("#toastwarning").dxToast("show");
            }
        }

    });

    function getUserRoleId() {
        
        var urlParams = new URLSearchParams(window.location.search);
        var IsRedirectFromTeamAgent = urlParams.get('IsRedirectFromTeamAgent');
        
        if (IsRedirectFromTeamAgent) {
            return userRoleId;
        } else {
            return '<%= UserRole%>'
        }
        //GetUsersList
        
    }
</script>

<div id="loadpanel"></div>
<div class="container col-12" style="padding-top: 30px;">
</div>
<input type="hidden" id="hdnlstUserProfiles" runat="server" />
<input type="hidden" id="hdnLstType" runat="server" />

<div class="row bs">
    <div id="dvNonManagerView">
        <div class="col-md-4 col-sm-4">
            <div id="userDropDown"></div>
        </div>
    </div>
    <div id="dvManagerView"  class="row">
        <div class="col-sm-6 col-xs-6">
            <div id="ddlType"></div>
        </div>
        <div class="col-sm-6 col-xs-6">
            <div id="ddlWorkItem"></div>
        </div>

    </div>
    <div id="divSubCategory" class="row" >
        <div class="col-sm-6 col-xs-6" style="padding-top:20px;">
            <div id="ddlSubCategory"></div>
        </div>
    </div>
</div>

<div class="row" style="padding-bottom: 15px;">
</div>

<div class="row">
    <div class="divMarginTop divMarginBottom d-flex justify-content-between pt-2 pb-3" <%=SetAlignment%>>
        <div <%=EnableMultiAllocation%>>
            <div class="pl-0 col-md-4 col-sm-4">
                <div id="btnPrecondate" class="preconBG"></div>
                <p id="lblConStart" <%=HidePrecon%>><%=PreConStartDateString %>  <%=PreConEndDateString ==null?"":"to" %>  <%=PreConEndDateString %></p>
            </div>
            <div class="pl-0 col-md-4 col-sm-4">
                <div id="btnConstructionDate" class="constBG"></div>
                <p id="lblConstructionStart" <%=HideConst%>><%=ConstructionStartDateString %>  <%=ConstructionEndDateString ==null?"":"to" %> <%=ConstructionEndDateString%></p>
            </div>
            <div class="pl-0 col-md-4 col-sm-4">
                <div id="btnCloseoutDate" class="closeoutBG" style="max-width: initial !important"></div>
                <p id="lblCloseOut"><%=CloseOutStartDateString %>  <%=CloseOutEndDateString ==null?"":"to" %> <%=CloseOutEndDateString%> </p>
            </div>
        </div>
        <div class="pl-2">
            <div id="btnAddNewMultiAllocation" class="btnAddNew"></div>
        </div>
    </div>
</div>
<div id="data-grid-demo" class="row">
    <div id="gridDeleteSelected"></div>
    <div id="gridMultiAllocationCrud"></div>
</div>

<div class="row bs pt-2">
    <div class="col-md-12 col-sm-12 pt-2" style="text-align: right;">
        <div id="btnSaveMultiAllocation" class="btnAddNew text-right"></div>
    </div>
</div>

<input type="hidden" id="hdnPreConStartDate" runat="server" />
<input type="hidden" id="hdnPreConEndDate" runat="server" />
<input type="hidden" id="hdnConstStartDate" runat="server" />
<input type="hidden" id="hdnConstEndDate" runat="server" />
<input type="hidden" id="hdnCloseOutEndDate" runat="server" />



