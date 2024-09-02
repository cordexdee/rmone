<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PotentialAllocationsList.ascx.cs" Inherits="uGovernIT.Web.PotentialAllocationsList" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    body {
        font-family: 'Roboto', sans-serif !important;        
    }
/*    .FullyAvailablecircle {
        border-radius: 50%;
        background-color: #57A71D;
        width: 20px;
        height: 20px;
    }
    .NearToFullyAvailablecircle {
        border-radius: 50%;
        background-color: #A9C23F;
        width: 20px;
        height: 20px;
    }
    .PartiallyAvailablecircle {
        border-radius: 50%;
        background-color: #FFC100;
        width: 20px;
        height: 20px;
    }
    .NotAvailablecircle {
        border-radius: 50%;
        background-color: #FF3535;
        width: 20px;
        height: 20px;
    }*/
    
    .dx-datagrid-content .dx-datagrid-table .dx-row > td, .dx-datagrid-content .dx-datagrid-table .dx-row > tr > td {
        vertical-align: middle;
    }
    .dx-datagrid .dx-row > td {
    padding: 4px;
    }
    .dx-datagrid-rowsview .dx-selection.dx-row:not(.dx-row-focused) > td {
        background-color: #f9f9f9;
    }
    .preconDateStyle {
        background-color: #52BED9;
        border: 2px solid #52BED9;
        color: #fff;
    }
    .constDateStyle {
        background-color: #005C9B;
        border: 2px solid #005C9B;
        color: #fff;
    }
    .closeoutDateStyle {
        background-color: #351B82;
        border: 2px solid #351B82;
        color: #fff;
    }
    .otherDateStyle {
        background-color: #fff;
        border: 2px solid #aca9a9;
        color: #000000de;
    }

    .dateConstClass {
        display: grid;
        align-content: center;
        text-align: center;
        padding: 8px;
        font-size: 13px;
    }
    .dx-tag-content-1 {
        position: relative;
        display: inline-block;
        text-align: center;
        border: 2px dotted gray !important;
        margin: 4px 0 0 4px;
        padding: 3px 21px 4px 9px !important;
        min-width: 40px;
        background-color: #ededed;
        border-radius: 28px;
        color: #333 !important;
        font-weight: 500;
        font-size: 13px !important;
    }
    .dx-tag-content-1-fit {
        position: relative;
        display: inline-block;
        text-align: center;
        border: 2px dotted gray !important;
        margin: 4px 0 0 4px;
        padding: 3px 21px 4px 9px !important;
        min-width: 40px;
        background-color: #caefa5;
        border-radius: 28px;
        color: #333 !important;
        font-weight: 500;
        font-size: 14px !important;
    }
    .dx-tag-content {
        position: relative;
        display: inline-block;
        text-align: center;
        border: 2px #ddd;
        border-radius: 28px;
        margin: 4px 0 0 4px;
        padding: 5px 21px 6px 12px;
        min-width: 40px;
        background-color: #ededed;
        color: #333;
        font-weight: 500 !important;
        font-size:13px !important
    }

    #tboxAssignedRoles .dx-tag-content {
        position: relative;
        display: inline-block;
        text-align: center;
        border: 2px #ddd;
        border-radius: 28px;
        cursor: pointer;
        margin: 4px 0 0 4px;
        padding: 2px 21px 6px 12px;
        min-width: 40px;
        background-color: #ededed;
        color: #333;
        font-weight: 500 !important;
        font-size: 13px !important
    }


    .dx-tag-content-fit {
        position: relative;
        display: inline-block;
        text-align: center;
        border: 2px #ddd;
        border-radius: 28px;
        margin: 4px 0 0 4px;
        padding: 5px 21px 6px 12px;
        min-width: 40px;
        background-color: #caefa5;
        color: #333;
        font-weight: 500 !important;
        font-size: 14px !important
    }
    .btnAddNew .dx-icon {
    filter:brightness(10);
}
    .btnAddNew.dx-button{
    font-family: 'Roboto', sans-serif;
    font-size: 13px;
    font-weight: 500;
    letter-spacing: 0.02rem;
    }

    .searchTextbox {
        border-radius: 7px;
        border: 1px solid #D3D3D3;
        /*padding-top: 5px;*/
        height: 38px;
        font-size: 13px;
    }
    .searchRoundControl {
        border-radius: 7px;
        border: 1px solid #D3D3D3;
    }
    .searchLabel {
        color: gray;
        font-size: 13px;
        font-weight:500;
    }
    .lblMaxAllocation {
        width: 10% !important;
    }
    /*.dx-button-has-text .dx-button-content {
        color: #fff;
        padding: 10px;*/
        /*background: #4FA1D6;*/
    /*}*/
    .title-opm {
        padding: 4px;
        border-radius: 11px;
        border: 2px solid #4fa1d6;
        font-weight: 500;
        font-size: 13px;
        background: #fff;
        color: #000;
        text-align: center;
        width: 100%;
        min-height: 38px;        
    }
    .title-cpr {
        border: none;
        padding: 5px;
        border-radius: 11px;
        font-weight: 500;
        font-size: 13px;
        background: #4fa1d6;
        color: white !important;
        text-align: center;
        width: 100%;
        min-height: 38px;
    }

    .grdCell {
        padding: 10px;
        font-weight: 400;
        font-size: 13px;
        background: #ededed;
        color: #4b4b4b;
        text-align: center;
    }
    .grdCellFit {
        padding: 10px;
        background: #ededed;
        text-align: center;
        color: #4b4b4b;
        font-weight: 700;
        font-size: 12px;
        text-decoration: underline;
    }
    .grdCellPostAlloc {
        padding: 10px;
        font-weight: 500;
        font-size: 13px;
        background: #e6eff7;
        color: #4b4b4b;
        text-align: center;
    }
    .grdCellConflicts {
        padding: 10px;
        font-weight: 400;
        font-size: 13px;
        background: #bbdefb;
        color: #4b4b4b;
        text-align: center;
    }

    .grdHeader {
        color: #4b4b4b;
        font-size: 14px;
        font-weight: 600;
    }
    #ConflictWeekDataGridDialog .dx-popup-content, #ConflictWeekDataGridDialogSummary .dx-popup-content {
        margin-top: -10px;
    }
    #ConflictWeekDataGridDialog .dx-popup-bottom.dx-toolbar, #ConflictWeekDataGridDialogSummary .dx-popup-bottom.dx-toolbar {
        padding-top: 0px !important;
    }

    #ConflictWeekDataGridDialog .dx-popup-title.dx-has-close-button, #ConflictWeekDataGridDialogSummary .dx-popup-title.dx-has-close-button {
        padding-top: 9px;
        padding-bottom: 9px;
        font-size: 16px;
        font-weight: 500;
        background-color: #f0f0f0;
    }
    #ConflictWeekDataGridDialog .close-btn, #ConflictWeekDataGridDialogSummary .close-btn {
        position: absolute;
        right: 15px;
        margin-top: -3px;
        cursor: pointer;
    }
    .color-style {
        color: #4A6EE2;
        text-decoration: underline;
        font-weight: 500;
        text-align: center;
    }
    .header-style {
    display: flex;
    align-items: center;
    justify-content: flex-end;
    }

    .btnBlue {
        background-color: #789CCE !important;
        color: white;
    }
    .btnNormal {
        background-color: white !important;
        color: black;
    }
    .container-flex {
    display: flex;
    justify-content: space-between;
    flex-wrap: wrap;
    align-items: center;
    align-content: stretch;
    }

    .dx-datagrid-borders .dx-datagrid-rowsview, .dx-datagrid-headers + .dx-datagrid-rowsview, .dx-datagrid-rowsview.dx-datagrid-after-headers {
    border-top: 1px solid #ddd;
    border-bottom: 1px solid #ddd;
}
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var baseUrl = ugitConfig.apiBaseUrl;
    var dtAlloc;
    var dtDefaultDivType;
    var dtMasterAllocations;
    var showBenchProjectDepartment = "<%=ShowBenchProjectDepartment%>" == "False" ? false : true;
    var division = "<%=division%>";
    var department = "<%=department%>";
    allocModel = {};
    allocModel.UserId = "<%=UserId%>"; //"a9a6495c-fe3d-4689-94c9-6b18442ac591";
    allocModel.UserName = "<%=UserName%>"; //"a9a6495c-fe3d-4689-94c9-6b18442ac591";
    allocModel.RoleId = "<%=RoleId%>"; //"20EB6220-399C-4A3E-B050-EA258AE49378";
    allocModel.StartDate = "<%=StartDate%>"; //"2024-02-01";
    allocModel.EndDate = "<%=EndDate%>"; //new Date(); //.format('MM/dd/yyyy')
    allocModel.IncludeSoftAllocation = true;
    allocModel.RoleName = ["<%=RoleName%>"];
    allocModel.Allocations = [];
    var experienceTags = [];
    var certifications = [];
    var lstWeekWiseAvailability = [];
    var IsAllocationSaved = false;
    var isSoftAllocation = true;
    var FnRoleList = [];
    var CallOnChange = true;
    var startDate = "<%=StartDate%>";
    $(() => {
        loadingPanel.Show();
        $.post("/api/Bench/FindPotentialAllocationsForResource/", allocModel).then(function (response) {
            dtMasterAllocations = response.dtUnfilledAllocations;
            lstWeekWiseAvailability = response.lstResourceWeekWiseAvailability;
            dtDefaultDivType = response.dtDefaultDivisionRole;
            if (dtMasterAllocations != null)
                RefreshDisplayedAllocations(true);
            DisplayPotentialAllocations();
            $.get("/api/Bench/GetFunctionRoleMapping/").then(function (data) {
                if (data != "No Data Found") {
                    FnRoleList = data;
                }
                LoadFilters();
                loadingPanel.Hide();
            });
        });
        
        $("#lblSearchStart").dxDateBox({
            type: 'date',
            width: '100%',
            value: startDate != null ? new Date(startDate).toDateString().slice(4) : '',
            displayFormat: "MMM dd, yyyy",
            inputAttr: { 'aria-label': 'Date Time' },
            onValueChanged: function (e) {
                allocModel.StartDate = $("#lblSearchStart").dxDateBox('instance').option('value');
                allocModel.EndDate = new Date(allocModel.StartDate).addDays(7).toDateString().slice(4);
                loadingPanel.Show();
                $.post("/api/Bench/FindPotentialAllocationsForResource/", allocModel).then(function (response) {
                    dtMasterAllocations = response.dtUnfilledAllocations;
                    lstWeekWiseAvailability = response.lstResourceWeekWiseAvailability;
                    if (dtMasterAllocations != null)
                        RefreshDisplayedAllocations();
                    else {
                        var grid = $('#gridContainer').dxDataGrid("instance");
                        grid.option("dataSource", null);
                        let tagboxDivision = $('#ddlDivision').dxSelectBox("instance");
                        let tagboxFunctionalRoles = $('#tboxFunctionalRoles').dxTagBox('instance');
                        let tagboxRoles = $('#tboxAssignedRoles').dxTagBox("instance");
                        tagboxDivision.option("value", null);
                        tagboxFunctionalRoles.option("value", null);
                        tagboxRoles.option("value", null);
                        tagboxDivision.option("dataSource", []);
                        tagboxFunctionalRoles.option("dataSource", []);
                        tagboxRoles.option("dataSource", []);
                    }
                    loadingPanel.Hide();
                });
            }
        });
    });

    function DisplayPotentialAllocations() {
        let height = $(window).height();
        let gridHeight = parseInt(65 * $(window).height() / 100);;
        if (height > 600) {
            gridHeight = parseInt(81 * $(window).height() / 100)
        }
        $('#gridContainer').css("max-height", gridHeight + "px");
        $('#gridContainer').dxDataGrid({
            keyExpr: 'ID',
            showBorders: false,
            showRowLines: false,
            showColumnLines: false,
            paging: false,
            remoteOperations: false,
            cellHintEnabled: true,
            /*rowAlternationEnabled: true,*/
            wordWrapEnabled: true,
            noDataText: "No allocations available.",
            selection: {
                mode: 'multiple',
                showCheckBoxesMode: 'always',
            },

            columns: [
                {
                    dataField: 'ModuleName',
                    sortIndex: 0,
                    sortOrder: 'asc',
                    visible: false
                },
                {
                    dataField: 'Title',
                    sortIndex: 1,
                    sortOrder: 'asc',
                    cellTemplate: function (container, options) {
                        if (options.data.Title != "") {
                            let className = "title-opm";
                            if (options.data.ModuleName == "CPR")
                                className = "title-cpr";
                            if (options.data.isSummary == 1 || options.data.isSummary == 2) {
                                $(`<button class='${className}' onclick = \"${options.data.ProjectLink}\" >${options.data.Title}</button>`).appendTo(container);
                            }
                            else {
                                $(`<button class='${className}' style='display:none' onclick = \"${options.data.ProjectLink}\" >${options.data.Title}</button>`).appendTo(container);
                            }
                        }
                    },
                    headerCellTemplate: function (header, info) {
                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                    }
                },
                {
                    caption: '<%=ERPJobIDName%>',
                    dataField: "ERPJobID",
                    alignment: 'center',
                    width: "8%",
                    cellTemplate: function (container, options) {

                        if (options.data.ERPJobID != null && options.data.ERPJobID != "" && (options.data.isSummary == 1 || options.data.isSummary == 2)) {
                            $("<div class='grdCell'>" + options.data.ERPJobID + "</div>").appendTo(container);
                        }
                    },
                    headerCellTemplate: function (header, info) {
                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                    }
                },
                {
                    caption: 'Role',
                    dataField: 'TypeName',
                    width: "18%",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        /*container.css('display', 'flex');*/
                        if (options.data.TypeName != "" && (options.data.isSummary == 1 || options.data.isSummary == 2)) {
                            $("<div class='grdCell'>" + options.data.TypeName + "</div>").appendTo(container);
                        }
                    },
                    headerCellTemplate: function (header, info) {
                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                    }
                },
                {
                    caption: 'Fit',
                    dataField: 'Fit',
                    width: "5%",
                    alignment: 'center',
                    cellTemplate(container, options) {
                        $("<div class='grdCellFit'><a style='color:#4b4b4b;font-weight:700;font-size:12px;text-decoration:underline;' href='javascript:void(0);' onclick=openTagPopup('" + options.data.UserTags
                            + "','" + options.data.ProjectTags + "','" + options.data.UserCertifications + "','" + options.data.ProjectCertifications + "');>"
                            + options.data.Fit + "</a></div>").appendTo(container);
                    },
                    headerCellTemplate: function (header, info) {
                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                    }
                },
                {
                    caption: 'Start Date',
                    dataField: 'AllocationStartDate',
                    width: "9%",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        if (options.data.AllocationStartDate != "") {
                            let bkColorcss = getDateBackgroundColor(new Date(options.data.AllocationStartDate), options.data.PreconStartDate, options.data.PreconEndDate,
                                options.data.EstimatedConstructionStart, options.data.EstimatedConstructionEnd, options.data.CloseoutStartDate, options.data.CloseoutDate);

                            $("<div class='dateConstClass " + bkColorcss + "'>" + new Date(options.data.AllocationStartDate).format('MMM dd, yyyy') + "</div>").appendTo(container);
                        }
                    },
                    headerCellTemplate: function (header, info) {
                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                    }
                },
                {
                    caption: 'End Date',
                    dataField: 'AllocationEndDate',
                    width: "9%",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        if (options.data.AllocationEndDate != "") {
                            let bkColorCSS = getDateBackgroundColor(new Date(options.data.AllocationEndDate), options.data.PreconStartDate, options.data.PreconEndDate,
                                options.data.EstimatedConstructionStart, options.data.EstimatedConstructionEnd, options.data.CloseoutStartDate, options.data.CloseoutDate);

                            $("<div class='dateConstClass " + bkColorCSS + "'>" + new Date(options.data.AllocationEndDate).format('MMM dd, yyyy') + "</div>").appendTo(container);
                        }
                    },
                    headerCellTemplate: function (header, info) {
                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                    }
                },
                {
                    caption: 'Len(Wks)',
                    dataField: 'Length',
                    alignment: 'center',
                    width: "5%",
                    cellTemplate: function (container, options) {
                        if (options.data.Length == "0") {
                            $("<div class='grdCell'>1</div>").appendTo(container);
                        }
                        else if (options.data.Length != "") {
                            $("<div class='grdCell'>" + options.data.Length + "</div>").appendTo(container);
                        }
                    },
                    headerCellTemplate: function (header, info) {
                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                    }
                },
                {
                    caption: 'Alloc%',
                    dataField: "PctAllocation",
                    alignment: 'center',
                    width: "5%",
                    cellTemplate: function (container, options) {
                        if (options.data.PctAllocation != "") {
                            $("<div class='grdCell'>" + Math.round(options.data.PctAllocation) + "</div>").appendTo(container);
                        }
                    },
                    headerCellTemplate: function (header, info) {
                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                    }
                },
                {
                    caption: 'UsrAlloc%',
                    dataField: "UserAlloc",
                    width: "5%",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        if (options.data.UserAlloc != "") {
                            $("<div class='grdCell'>" + options.data.UserAlloc + "</div>").appendTo(container);
                        }
                    },
                    headerCellTemplate: function (header, info) {
                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                    }
                },
                {
                    caption: 'PostAlloc%',
                    dataField: "Availability",
                    width: "5%",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        /*container.css('display', 'flex');*/
                        if (typeof options.data.Availability != 'undefined' && options.data.isSummary != 1) {
                            $("<div class='grdCellPostAlloc'>" + options.data.Availability + "</div>").appendTo(container);

                        }
                    },
                    headerCellTemplate: function (header, info) {
                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                    }
                },
                {
                    caption: 'Conflicts',
                    dataField: "Conflicts",
                    width: "5%",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        /*container.css('display', 'flex');*/
                        if (options.data.Conflicts != "" && options.data.isSummary != 1) {
                            //$("<div class='grdCellConflicts'>" + options.data.Conflicts + "</div>").appendTo(container);
                            $(`<div class='grdCellConflicts'><a style='color:#4b4b4b;font-weight:700;font-size:12px;text-decoration:underline;' href='javascript:void(0);' onclick=OpenConflictWeekData('${options.data.ID}');>${options.data.Conflicts}</a></div>`).appendTo(container);
                        }
                    },
                    headerCellTemplate: function (header, info) {
                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                    }
                },
            ],
            onSelectionChanged(selectedItems) {
                if (IsAllocationSaved) {
                    IsAllocationSaved = false;
                    return;
                    //    setTimeout(function () { loadingPanel.Show();}, 3000);
                }
                const selectedAlloc = selectedItems.selectedRowsData;
                allocModel.Allocations = selectedAlloc;
                loadingPanel.Show();
                $.post("/api/Bench/FindPotentialAllocationsForResource/", allocModel).then(function (response) {
                    dtMasterAllocations = response.dtUnfilledAllocations;
                    lstWeekWiseAvailability = response.lstResourceWeekWiseAvailability;
                    if (dtMasterAllocations != null)
                        RefreshDisplayedAllocations();
                    loadingPanel.Hide();

                });
            },

            onCellPrepared: function (e) {
                if (e.rowType === "header") {
                    e.cellElement.attr("title", e.column.hint);
                }
                if (e.rowType === "data" && e.columnIndex === 0 && e.data.isSummary === "1") {
                    e.cellElement.find('.dx-select-checkbox').dxCheckBox("instance").option("visible", false);
                }
            },
            onRowPrepared: function (e) {
                if (e.rowType == "data") {
                    if (e.data.Deleted != false) {
                        e.rowElement.addClass('clssdeletedrow');
                    }
                }
            },

        });
    }
    function RefreshDisplayedAllocations(includeSoftAlloc) {
        if (includeSoftAlloc != undefined && !includeSoftAlloc)
            dtAlloc = dtMasterAllocations.filter(x => !x.SoftAllocation);
        else if (includeSoftAlloc != undefined && includeSoftAlloc) {
            dtAlloc = dtMasterAllocations;
        }
        else {

            SetFiltersDD();
            //var chkIncludesoft = $('#chkIncludeSoft').dxCheckBox("instance");
            //includeSoftAlloc = chkIncludesoft.option("value");
            //if (includeSoftAlloc)
            //    dtAlloc = dtMasterAllocations;
            //else
            //    dtAlloc = dtMasterAllocations.filter(x => !x.SoftAllocation);

            //let tempDt = JSON.parse(JSON.stringify(dtAlloc));
            
            //let tboxAssignedRoles = $("#tboxAssignedRoles").dxTagBox("instance");
            //let selectedRoles = tboxAssignedRoles.option("value");
            //if (selectedRoles != null && selectedRoles.length > 0) {
            //    tempDt = JSON.parse(JSON.stringify(dtAlloc.filter(x => selectedRoles.includes(x.TypeName))));
            //}

            //let ddlDivision = $('#ddlDivision').dxTagBox("instance");
            //let selectedDivision = ddlDivision.option("value");
            //if (selectedDivision != null && selectedDivision.length > 0) {
            //    tempDt = JSON.parse(JSON.stringify(tempDt.filter(x => selectedDivision.includes(x.Division))));
            //}

            //var grid = $('#gridContainer').dxDataGrid("instance");
            //grid.option("dataSource", tempDt);
        }
    }
    function LoadFilters() {
        $('#ddlDivision').dxSelectBox({
            placeholder: 'Division',
            showClearButton: true,
            onValueChanged: function (e) {
                if (CallOnChange) {
                    SetFiltersDD(undefined, undefined, undefined, false, true);
                }
            }
        });

        if (!showBenchProjectDepartment) {
            $(".departmentDiv").hide();
        }

        $('#ddlDepartment').dxTagBox({
            showSelectionControls: true,
            placeholder: 'Department',
            showClearButton: true,
            searchEnabled: true,
            visible: showBenchProjectDepartment,
            maxDisplayedTags: 1,
            onValueChanged: function (e) {
                if (CallOnChange) {
                    SetFiltersDD(undefined, undefined, undefined, false, false, false, undefined, true);
                }
            }
        });


        const tagboxassignedRoles = $('#tboxAssignedRoles').dxTagBox({
            showSelectionControls: true,
            applyValueMode: 'instantly',
            searchEnabled: true,
            maxDisplayedTags: 1,
            onValueChanged: function (data) {
                allocModel.RoleName = data.value;
                if (CallOnChange) {
                    SetFiltersDD(undefined, undefined, undefined, false, false, true);
                }
            }
        });

        const tagboxFunctionalRoles = $('#tboxFunctionalRoles').dxTagBox({
            showSelectionControls: true,
            applyValueMode: 'instantly',
            searchEnabled: true,
            maxDisplayedTags: 1,
            onValueChanged: function (data) {
                if (CallOnChange) {
                    SetFiltersDD(undefined, undefined, undefined, true);
                }
            }
        });
        const chkIncludeSoft = $("#chkIncludeSoft").dxCheckBox({
            value: true,
            text: "Include Soft Allocations",
            onValueChanged: function (e) {
                allocModel.IncludeSoftAllocation = e.value;
                isSoftAllocation = e.value;
                RefreshDisplayedAllocations();
            }
        });
        let userFunctionName = '';
        if (allocModel.RoleName != "" && allocModel.RoleName != "null") {
            userFunctionName = FnRoleList.filter(x => x.RoleName == allocModel.RoleName)[0]?.FunctionName;
        }
        debugger;
        SetFiltersDD(division, userFunctionName == null || userFunctionName == '' ? [] : [userFunctionName], [allocModel.RoleName], undefined, undefined, undefined, [department]);

        const currAlloc = $("#currentAllocation").dxButton({
            hint: "Display Current Allocations",
            text: 'Current Allocation',
            visible: 'true',
            //width: 40,
            focusStateEnabled: true,
            onClick() {
                window.parent.UgitOpenPopupDialog('<%=customResourceAllocationURL %>', "", "Current Allocation", "90", "90");
            }
        });

        const saveAlloc = $("#saveAllocation").dxButton({
            hint: "Save Allocation",
            text: 'Save Allocation',
            icon: "/content/Images/save-open-new-wind.png",
            visible: 'true',
            focusStateEnabled: true,
            onClick() {
                var count = 0;
                var grid = $('#gridContainer').dxDataGrid("instance");
                var Selectedrows = grid.getSelectedRowsData();
                if (Selectedrows.length <= 0) {
                    DevExpress.ui.dialog.alert(`Please select record to save allocations`, 'Error');
                    return false;
                }
                var curDate = new Date();
                var isStartDateGreater = false;
                Selectedrows.forEach(function (row, index) {
                    if (new Date(row.AllocationStartDate) < curDate && new Date(row.AllocationEndDate) >= curDate) {
                        if (isStartDateGreater == false) {
                            isStartDateGreater = true;
                        }
                    }
                });
                if (isStartDateGreater) {
                    var result = DevExpress.ui.dialog.custom({
                        title: "Warning!",
                        message: "RM One will adjust the start date of this allocation to Current Date",
                        buttons: [
                            { text: "Retain as Start Date", onClick: function () { return false }, elementAttr: { "class": "btnBlue" } },
                            { text: "Change Date to " + new Date(curDate).toLocaleDateString("en-US"), onClick: function () { return true }, elementAttr: { "class": "btnNormal" } }
                        ]
                    });
                    result.show().done(function (dialogResult) {
                        loadingPanel.Show();
                        Selectedrows.forEach(function (row, index) {
                            count++;
                            var list = {};
                            list.Allocations = [];
                            list.PreConStart = row.PreconStartDate;
                            list.PreConEnd = row.PreconEndDate;
                            list.ConstStart = row.EstimatedConstructionStart;
                            list.ConstEnd = row.EstimatedConstructionEnd;
                            list.ProjectID = row.ProjectId;
                            list.CalledFrom = "Bench > Potential Allocation > Save";

                            var objAllocations = {};
                            objAllocations.AllocationStartDate = row.AllocationStartDate;
                            objAllocations.AllocationEndDate = row.AllocationEndDate;
                            objAllocations.AssignedTo = allocModel.UserId;
                            objAllocations.Type = allocModel.RoleId;
                            objAllocations.PctAllocation = row.PctAllocation;
                            objAllocations.ProjectID = row.ProjectId;
                            objAllocations.Title = row.Title;
                            objAllocations.ID = row.ProjectEstimatedAllocationId;
                            objAllocations.TypeName = row.TypeName;
                            objAllocations.SoftAllocation = row.SoftAllocation;
                            objAllocations.NonChargeable = row.NonChargeable;
                            objAllocations.IsLocked = row.IsLocked;
                            objAllocations.isChangeStartDate = dialogResult
                            list.Allocations[0] = objAllocations;

                            $.post("/api/rmmapi/UpdateCRMAllocation/", list).then(function (response) {
                                console.log("Save Allocation Method Ends for " + row.ProjectEstimatedAllocationId);
                                if (response.includes("OverlappingAllocation")) {
                                    DevExpress.ui.notify(response, "error");
                                    loadingPanel.Hide();
                                }
                                IsAllocationSaved = true;
                                dtMasterAllocations = dtMasterAllocations.filter(x => x.ID != row.ID);
                                var dtAlloc = null;
                                var SameDataExist = dtMasterAllocations.filter(x => x.Title == row.Title && x.ERPJobID == row.ERPJobID && x.TypeName == row.TypeName && x.ProjectId == row.ProjectId && x.isSummary == 0)
                                if (SameDataExist != null && SameDataExist.length == 1) {
                                    dtMasterAllocations = dtMasterAllocations.filter(x => !(x.TypeName == String(row.TypeName).trim() && x.ProjectId == String(row.ProjectId).trim() && x.isSummary == "1" && x.ID == null));
                                    dtAlloc = dtMasterAllocations.find(x => x.Title == row.Title && x.ERPJobID == row.ERPJobID && x.TypeName == row.TypeName && x.ProjectId == row.ProjectId && x.isSummary == 0);
                                    dtAlloc.isSummary = "2";
                                }
                                if (Selectedrows.length == 1) {
                                    setTimeout(function () {
                                        RefreshDisplayedAllocations();
                                        loadingPanel.Hide();
                                        if (Selectedrows.length == count) {
                                            DevExpress.ui.notify('Allocation Saved Successfully.', "success");
                                        }
                                    }, 4000);
                                }
                                else {
                                    setTimeout(function () {
                                        RefreshDisplayedAllocations();
                                        loadingPanel.Hide();
                                        if (Selectedrows.length == count) {
                                            DevExpress.ui.notify('Allocation Saved Successfully.', "success");
                                        }
                                    }, 10000);
                                }

                            },
                                function (error) {
                                    loadingPanel.Hide();
                                });
                        });
                    });
                }
                else {
                    loadingPanel.Show();
                    Selectedrows.forEach(function (row, index) {
                        count++;
                        var list = {};
                        list.Allocations = [];
                        list.PreConStart = row.PreconStartDate;
                        list.PreConEnd = row.PreconEndDate;
                        list.ConstStart = row.EstimatedConstructionStart;
                        list.ConstEnd = row.EstimatedConstructionEnd;
                        list.ProjectID = row.ProjectId;
                        list.CalledFrom = "Bench > Potential Allocation > Save";

                        var objAllocations = {};
                        objAllocations.AllocationStartDate = row.AllocationStartDate;
                        objAllocations.AllocationEndDate = row.AllocationEndDate;
                        objAllocations.AssignedTo = allocModel.UserId;
                        objAllocations.Type = allocModel.RoleId;
                        objAllocations.PctAllocation = row.PctAllocation;
                        objAllocations.ProjectID = row.ProjectId;
                        objAllocations.Title = row.Title;
                        objAllocations.ID = row.ProjectEstimatedAllocationId;
                        objAllocations.TypeName = row.TypeName;
                        objAllocations.SoftAllocation = row.SoftAllocation;
                        objAllocations.NonChargeable = row.NonChargeable;
                        objAllocations.IsLocked = row.IsLocked;
                        objAllocations.isChangeStartDate = false;
                        list.Allocations[0] = objAllocations;

                        $.post("/api/rmmapi/UpdateCRMAllocation/", list).then(function (response) {
                            console.log("Save Allocation Method Ends for " + row.ProjectEstimatedAllocationId);
                            if (response.includes("OverlappingAllocation")) {
                                DevExpress.ui.notify(response, "error");
                            }
                            IsAllocationSaved = true;

                            var dtAlloc = null;
                            dtMasterAllocations = dtMasterAllocations.filter(x => x.ID != row.ID);
                            var SameDataExist = dtMasterAllocations.filter(x => x.Title == row.Title && x.ERPJobID == row.ERPJobID && x.TypeName == row.TypeName && x.ProjectId == row.ProjectId && x.isSummary == 0)
                            if (SameDataExist != null && SameDataExist.length == 1) {
                                dtMasterAllocations = dtMasterAllocations.filter(x => !(x.TypeName == String(row.TypeName).trim() && x.ProjectId == String(row.ProjectId).trim() && x.isSummary == "1" && x.ID == null));
                                dtAlloc = dtMasterAllocations.find(x => x.Title == row.Title && x.ERPJobID == row.ERPJobID && x.TypeName == row.TypeName && x.ProjectId == row.ProjectId && x.isSummary == 0);
                                dtAlloc.isSummary = "2";
                            }
                            if (Selectedrows.length == 1) {
                                setTimeout(function () {
                                    RefreshDisplayedAllocations();
                                    loadingPanel.Hide();
                                    if (Selectedrows.length == count) {
                                        DevExpress.ui.notify('Allocation Saved Successfully.', "success");
                                    }
                                }, 4000);
                            }
                            else {
                                setTimeout(function () {
                                    RefreshDisplayedAllocations();
                                    loadingPanel.Hide();
                                    if (Selectedrows.length == count) {
                                        DevExpress.ui.notify('Allocation Saved Successfully.', "success");
                                    }
                                }, 10000);
                            }



                        },
                            function (error) {
                                loadingPanel.Hide();
                            });

                    });
                }
                if (IsAllocationSaved == true) {
                    DevExpress.ui.notify('Allocation Saved Successfully.', "success");
                }
            }
        });

        $('#tagsPopup').dxPopup({
            visible: false,
            hideOnOutsideClick: true,
            showTitle: true,
            showCloseButton: true,
            title: "Matching Tags",
            width: "350",
            height: "auto",
            resizeEnabled: true,
            dragEnabled: true,
            contentTemplate: () => {
                const content = $("<div id='divpopup' />");
                content.append(
                    $(`<div style='padding-bottom: 10px' />`).append(
                        $(`<div>Tags</div>`),
                        $('<div id="projectExpTags" style="display:flex;flex-wrap:wrap;margin:5px;" />'),
                        $(`<div id = "certLabel">Certifications</div>`),
                        $('<div id="projectCertifications" style="display:flex;flex-wrap:wrap;margin:5px;" />')
                    ),
                );
                return content;
            },
            onHidden: function (e) {
            }
        });

        GetExperienceData();

    }
    function commonGridFilter(dtAlloc, filterRoles) {

        grid = $("#gridContainer").dxDataGrid("instance");
        tboxAssignedRoles = $("#tboxAssignedRoles").dxTagBox("instance");
        let SelectedRoles = tboxAssignedRoles.option("value");
        ddlDivision = $("#ddlDivision").dxSelectBox("instance");
        let SelectedDivn = ddlDivision.option("value");
        if (filterRoles == undefined) {
            filterRoles = false;
        }
        var data = dtAlloc;
        if (isSoftAllocation == false) {
            if (data != undefined && (SelectedRoles != null && SelectedRoles.length > 0) && (SelectedDivn != null && SelectedDivn.length > 0)) {
                data = data.filter(alloc => SelectedRoles.includes(alloc.TypeName) && SelectedDivn.includes(alloc.Division) && alloc.SoftAllocation == isSoftAllocation);
            }
            else if (data != undefined && SelectedRoles != null && SelectedRoles.length > 0)
                data = data.filter(alloc => SelectedRoles.includes(alloc.TypeName) && alloc.SoftAllocation == isSoftAllocation);
            else if (data != undefined && SelectedDivn != null && SelectedDivn.length > 0)
                data = data.filter(alloc => SelectedDivn.includes(alloc.Division) && alloc.SoftAllocation == isSoftAllocation);
        }
        else {
            if (data != undefined && (SelectedRoles != null && SelectedRoles.length > 0) && (SelectedDivn != null && SelectedDivn.length > 0)) {
                data = data.filter(alloc => SelectedRoles.includes(alloc.TypeName) && SelectedDivn.includes(alloc.Division));
            }
            else if (data != undefined && SelectedRoles != null && SelectedRoles.length > 0)
                data = data.filter(alloc => SelectedRoles.includes(alloc.TypeName));
            else if (data != undefined && SelectedDivn != null && SelectedDivn.length > 0)
                data = data.filter(alloc => SelectedDivn.includes(alloc.Division));
        }

        if (filterRoles) {
            if (data != undefined)
                $("#tboxAssignedRoles").dxTagBox('option', 'dataSource', [...new Set(data.map(item => item.TypeName))]);
        }

        grid.option("dataSource", data);
    }

    function SetFiltersDD(division, userFunctionName, roleName, functionalRolesChanged, divisionChanged, roleChanged, department, departmentChanged) {
        let tagboxDivision = $('#ddlDivision').dxSelectBox("instance");
        let tagboxDepartment = $('#ddlDepartment').dxTagBox("instance");
        let tagboxFunctionalRoles = $('#tboxFunctionalRoles').dxTagBox('instance');
        let tagboxRoles = $('#tboxAssignedRoles').dxTagBox("instance");
        let alldata = JSON.parse(JSON.stringify(dtMasterAllocations));
        var chkIncludesoft = $('#chkIncludeSoft').dxCheckBox("instance");
        includeSoftAlloc = chkIncludesoft.option("value");
        if (!includeSoftAlloc && alldata?.length > 0)
            alldata = alldata.filter(x => !x.SoftAllocation);
        if (functionalRolesChanged == undefined) {
            functionalRolesChanged = false;
        }
        if (divisionChanged == undefined) {
            divisionChanged = false;
        }
        if (departmentChanged == undefined) {
            departmentChanged = false;
        }
        if (roleChanged == undefined) {
            roleChanged = false;
        }
        if (division == undefined) {
            division = tagboxDivision.option("value");
        }
        if (userFunctionName == undefined) {
            userFunctionName = tagboxFunctionalRoles.option("value");
        }
        if (roleName == undefined) {
            roleName = tagboxRoles.option("value");
        }
        if (department == undefined) {
            department = tagboxDepartment.option("value");
        }
        CallOnChange = false;
        if (alldata?.length > 0) {
            if (showBenchProjectDepartment) {
                tagboxDivision.option('dataSource', [...new Set(alldata.map(x => x.DepartmentDivision))].filter(x => x != null && x != ''));
                if (division != null && division != '' && alldata.filter(x => x.DepartmentDivision == division)?.length > 0) {
                    alldata = alldata.filter(x => x.DepartmentDivision == division);
                    tagboxDivision.option('value', division);
                    if (divisionChanged) {
                        department = [...new Set(alldata.map(x => x.Department))];
                        userFunctionName = [...new Set(alldata.map(x => x.FunctionalName))].filter(x => x != null && x != '');
                        if (userFunctionName != null && userFunctionName.length > 0) {
                            roleName = [...new Set(FnRoleList.filter(x => userFunctionName.includes(x.FunctionName)).map(y => y.RoleName))];
                        }
                    }

                }
                tagboxDepartment.option('dataSource', [...new Set(alldata.map(x => x.Department))].filter(x => x != null && x != ''));
                if (department.length > 0 && alldata?.filter(x => department.includes(x.Department))?.length > 0) {
                    alldata = alldata.filter(x => department.includes(x.Department));
                    tagboxDepartment.option('value', [...new Set(alldata.map(x => x.Department))].filter(x => x != null && x != ''));
                    if (departmentChanged) {
                        userFunctionName = [...new Set(alldata.map(x => x.FunctionalName))].filter(x => x != null && x != '');
                        if (userFunctionName != null && userFunctionName.length > 0) {
                            roleName = [...new Set(FnRoleList.filter(x => userFunctionName.includes(x.FunctionName)).map(y => y.RoleName))];
                        }
                    }
                }
            }
            else {
                tagboxDivision.option('dataSource', [...new Set(alldata.map(x => x.Division))].filter(x => x != null && x != ''));
                if (division != null && division != '' && alldata.filter(x => x.Division == division)?.length > 0) {
                    alldata = alldata.filter(x => x.Division == division);
                    tagboxDivision.option('value', division);
                    if (divisionChanged) {
                        userFunctionName = [...new Set(alldata.map(x => x.FunctionalName))].filter(x => x != null && x != '');
                        if (userFunctionName != null && userFunctionName.length > 0) {
                            roleName = [...new Set(FnRoleList.filter(x => userFunctionName.includes(x.FunctionName)).map(y => y.RoleName))];
                        }
                    }
                }
            }
            tagboxFunctionalRoles.option('dataSource', [...new Set(alldata.map(x => x.FunctionalName))].filter(x => x != null && x != ''));

            if (userFunctionName.length > 0 && alldata?.filter(x => userFunctionName.includes(x.FunctionalName))?.length > 0) {
                alldata = alldata.filter(x => userFunctionName.includes(x.FunctionalName));
                tagboxFunctionalRoles.option('value', [...new Set(alldata.map(x => x.FunctionalName))].filter(x => x != null && x != ''));
                if (functionalRolesChanged) {
                    roleName = [...new Set(FnRoleList.filter(x => userFunctionName.includes(x.FunctionName)).map(y => y.RoleName))];
                }
            }
            else {
                tagboxFunctionalRoles.option('value', []);
            }
            tagboxRoles.option('dataSource', [...new Set(alldata.map(x => x.TypeName))]);
            if (roleName.length > 0 && alldata?.some(x => roleName.includes(x.TypeName))) {
                alldata = alldata.filter(x => roleName.includes(x.TypeName));
            }
            let tempSelectedRoles = [...new Set(alldata.map(x => x.TypeName))];
            let selectedRoles = allocModel.RoleName?.length > 0 || roleChanged
                ? allocModel.RoleName : tempSelectedRoles;
            if (selectedRoles?.length > 0) {
                if (alldata?.filter(x => selectedRoles.includes(x.TypeName))?.length > 0) {
                    alldata = alldata.filter(x => selectedRoles.includes(x.TypeName));
                    tagboxRoles.option('value', selectedRoles.filter(x => alldata.map(y => y.TypeName).includes(x)));
                }
                else {
                    tagboxRoles.option('value', tempSelectedRoles);
                }
            }

            CallOnChange = true;
            var grid = $('#gridContainer').dxDataGrid("instance");
            grid.option("dataSource", alldata);
        }
    }
    function getDateBackgroundColor(cellValue, preconstartDate, preconEndDate, conststartDate, constEndDate, closeoutstartDate, closeoutEndDate) {
        preconstartDate = new Date(preconstartDate);
        preconEndDate = new Date(preconEndDate);
        conststartDate = new Date(conststartDate);
        constEndDate = new Date(constEndDate);
        closeoutstartDate = new Date(closeoutstartDate);
        closeoutEndDate = new Date(closeoutEndDate);
        if (isDateValid(closeoutstartDate) && isDateValid(closeoutEndDate)
            && cellValue >= closeoutstartDate && cellValue <= closeoutEndDate) {
            return 'closeoutDateStyle';
        }
        else if (isDateValid(conststartDate) && isDateValid(constEndDate) &&
            cellValue <= constEndDate && cellValue >= conststartDate) {
            return 'constDateStyle';
        }
        else if (isDateValid(preconstartDate) && isDateValid(preconEndDate)
            && cellValue >= preconstartDate && cellValue <= preconEndDate) {
            return 'preconDateStyle';
        }
        else
            return 'otherDateStyle';
    }

    function GetExperienceData() {
        $.get("/api/rmmapi/GetExperiencedTagList?tagMultiLookup=All", function (data) {
            experienceTags = data;
            //console.log(experienceTags);
            GetCertificationsData();
        });
    }
    function GetCertificationsData() {
        $.get("/api/rmmapi/GetCertificationsList", function (data) {
            //console.log(certifications);
            certifications = data;
        });
    }
    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }
    function OpenConflictWeekData(allocID) {
        let conflictData = lstWeekWiseAvailability.filter(x => x.ID == allocID)[0].WeekWiseAllocations.filter(x => !x.IsAvailable);
        var selectedYear = new Date(Math.min.apply(null, conflictData.map(x => new Date(x.WeekStartDate)))).getFullYear();
        let allocation = dtAlloc.filter(x => x.ID == allocID)[0];
        let projectLink = allocation.TicketURL;
        let erpJobId = allocation.ERPJobID != '' && allocation.ERPJobID != null ? allocation.ERPJobID : allocation.ProjectId;
        if (conflictData != null && conflictData.length > 0) {
            const ConflictWeekDataGridTemplate = function () {
                let container = $("<div>");
                container.append(
                    $(`<div style='margin-bottom:10px;'>Weekly 'Alloc %' shown includes prospective <span onclick="${projectLink.replaceAll("\t", "")}" style='color:#4fa1d6;text-decoration:underline;cursor:pointer;'>${erpJobId}</span> allocation</div>`)
                );
                let windowHeight = parseInt(70 * $(window).height() / 100);
                let datagrid = $(`<div id='ConflictWeekDataGrid' style='max-height:${windowHeight}px'>`).dxDataGrid({
                    dataSource: conflictData,
                    ID: "grdConflictWeekData",
                    editing: {
                        mode: "cell",
                        allowEditing: true,
                        allowUpdating: true
                    },
                    sorting: {
                        mode: "multiple"
                    },
                    paging: {
                        enabled: false,
                    },
                    scrolling: {
                        mode: 'Standard',
                    },
                    columns: [
                        {
                            dataField: "WeekStartDate",
                            dataType: "date",
                            caption: "Start Date",
                            allowEditing: false,
                            sortIndex: "0",
                            sortOrder: "asc",
                            format: 'MMM d, yyyy',
                        },
                        {
                            caption: "End Date",
                            dataType: "date",
                            format: 'MMM d, yyyy',
                            calculateCellValue: function (rowData) {
                                return new Date(rowData.WeekStartDate).addDays(parseInt(rowData.NoOfDays) - 1);
                            }
                        },
                        {
                            dataField: "PctAllocation",
                            caption: "% Alloc",
                            dataType: "text",
                            width: "20%",
                        },
                        {
                            dataField: "PostPctAllocation",
                            caption: "% Post Alloc",
                            dataType: "text",
                            width: "20%",
                            allowEditing: false,
                        },
                    ],
                    onCellClick: function (e) {
                        OpenConflictWeekDetailSummary(e.data.WeekdetailedSummaries);
                    },
                    onRowPrepared: function (info) {
                        if (info.rowType === 'data') {
                            info.rowElement.css("cursor", 'pointer');
                        }
                    },
                    onCellPrepared: function (e) {
                        if (e.row == undefined) return;
                        if (e.row.key.WeekdetailedSummaries?.length > 1 && e.column.name == "PostPctAllocation") {
                            e.cellElement.addClass('color-style');
                        }
                    },
                    showBorders: true,
                    showRowLines: true,
                });
                container.append(datagrid);
                return container;
            };
            const popup = $("#ConflictWeekDataGridDialog").dxPopup({
                contentTemplate: ConflictWeekDataGridTemplate,
                width: "600",
                height: "auto",
                showTitle: true,
                visible: false,
                dragEnabled: true,
                hideOnOutsideClick: true,
                showCloseButton: true,
                position: {
                    at: 'center',
                    my: 'center',
                },
                titleTemplate: function () {
                    let headerData = $(`<span style="float: left;overflow: auto;">Conflicting Weeks: <a href="javascript:void(0);" onclick="openResourceTimeSheet('${allocModel.UserId}','${allocModel.UserName}','${selectedYear}');">${allocModel.UserName}</a></span>`);
                    headerData.append(
                        $(`<span title="Close" class="dx-button-content close-btn" onclick="$('#ConflictWeekDataGridDialog').dxPopup('instance').hide()"><i class="dx-icon dx-icon-close" style='font-size:20px;'></i></span>`)
                    );
                    return headerData;
                },
                wrapperAttr: {
                    id: "ConflictWeekDataGridDialog",
                    class: "class-name"
                },
                onHiding: function () {

                }
            }).dxPopup('instance');
            popup.option({
                contentTemplate: () => ConflictWeekDataGridTemplate()

            });
            popup.show();
        }
    }

    function OpenConflictWeekDetailSummary(data) {

        const ConflictWeekDataGridTemplateSummary = function () {
            let container = $("<div>");
            let windowHeight = parseInt(75 * $(window).height() / 100);
            let datagrid = $(`<div id='ConflictWeekDataSummaryGrid' style='max-height:${windowHeight}px'>`).dxDataGrid({
                dataSource: data,
                ID: "grdConflictWeekDataSummary",
                editing: {
                    mode: "cell",
                    allowEditing: true,
                    allowUpdating: true
                },
                sorting: {
                    mode: "multiple"
                },
                paging: {
                    enabled: false,
                },
                scrolling: {
                    mode: 'Standard',
                },
                columns: [
                    {
                        dataField: "Title",
                        caption: "Title",
                        dataType: "text",
                        width: "60%",
                        sortIndex: "0",
                        sortOrder: "asc",
                    },
                    {
                        dataField: "Role",
                        caption: "Role",
                        dataType: "text",
                    }
                ],
                showBorders: true,
                showRowLines: true,
            });
            container.append(datagrid);
            return container;
        };

        const popup = $("#ConflictWeekDataGridDialogSummary").dxPopup({
            contentTemplate: ConflictWeekDataGridTemplateSummary,
            width: "500",
            height: "auto",
            showTitle: true,
            visible: false,
            dragEnabled: true,
            hideOnOutsideClick: true,
            showCloseButton: true,
            position: {
                at: 'center',
                my: 'center',
            },
            titleTemplate: function () {
                let headerData = $(`<span style="float: left;overflow: auto;">Conflict Week Summary</span>`);
                headerData.append(
                    $(`<span title="Close" class="dx-button-content close-btn" onclick="$('#ConflictWeekDataGridDialogSummary').dxPopup('instance').hide()"><i class="dx-icon dx-icon-close" style='font-size:20px;'></i></span>`)
                );
                return headerData;
            },
            wrapperAttr: {
                id: "ConflictWeekDataGridDialogSummary",
                class: "class-name"
            },
            onHiding: function () {

            }
        }).dxPopup('instance');

        popup.option({
            contentTemplate: () => ConflictWeekDataGridTemplateSummary()

        });
        popup.show();
    }
    function openTagPopup(userTags, projectTags, userCertifications, projectCertifications) {
    var arruserTags = userTags.split(",").filter(i => i);
    var arrprojectTags = projectTags.split(",").filter(i => i);
    var arruserCertifications = userCertifications.split(",").filter(i => i);
    var arrprojectCertifications = projectCertifications.split(",").filter(i => i);
    const popupAddTags = function () {
        let container = $('<div class="roboto-font-family">');
        let divTags = $('<div id="projectExpTags" style="display:flex;flex-wrap:wrap;margin:5px;" />');
        if (projectTags.length > 0 && arrprojectTags != null && arrprojectTags.length > 0) {
            arrprojectTags.forEach(function (value, index) {
                let cssClass = arruserTags.includes(value) ? "dx-tag-content-fit" : "dx-tag-content";
                let title = experienceTags.filter(x => x.ID == value)[0].Title;
                //let title = experiencTag.Title;
                let element = $(`<div class="dx-tag-1"><div class="${cssClass}"><span>${title}</span></div></div>`);
                divTags.append(element);
                //$("#projectExpTags").append(element);
            });
            container.append(
                $(`<div>Tags</div>`),
            );
            container.append(divTags);
        }
        else {
            container.append(
                $(`<div>No Tags available.</div>`),
            );
        }
        let lblCert = $(`<div id="certLabel">Certifications</div>`);
        let divCertifications = $(`<div id="projectCertifications" style="display:flex;flex-wrap:wrap;margin:5px;" />`);
        if (projectCertifications.length > 0 && arrprojectCertifications != null && arrprojectCertifications.length > 0) {
            //$("#projectExpTags").append(certLabel);
            arrprojectCertifications.forEach(function (value, index) {
                let cssClass = arruserCertifications.includes(value) ? "dx-tag-content-1-fit" : "dx-tag-content-1";
                let title = certifications.filter(x => x.ID == value)[0].Title;
                //let title = cert.Title;
                let element = $(`<div class="dx-tag-1"><div class="${cssClass}"><span>${title}</span></div></div>`);
                divCertifications.append(element);
            });
            container.append(lblCert);
            container.append(divCertifications);
        }
        
        return container;
    };


    const popup = $('#tagsPopup').dxPopup({
        contentTemplate: popupAddTags,
        visible: false,
        hideOnOutsideClick: false,
        showTitle: true,
        showCloseButton: true,
        title: "Matching Tags",
        minWidth: 230,
        maxWidth: 600,
        width: "auto",
        height: "auto",
        resizeEnabled: true,
        dragEnabled: true,
    }).dxPopup('instance');

    popup.option({
        contentTemplate: () => popupAddTags()
    });
    popup.show();

    }
    function RenderProjectTagsOnFrame() {
        $("#projectExpTags").html(""); 
        if (existingProjectTags != null && existingProjectTags.length > 0) {
            existingProjectTags.forEach(function (value, index) {
                if (CheckTagExist(value.Type, value.TagId)) {
                    let experiencTag = value.Type == 2 ? experienceTags.filter(x => x.ID == value.TagId)[0] : certifications.filter(x => x.ID == value.TagId)[0];
                    let cssClass = value.MinValue > 0 ? "dx-tag-content" : "dx-tag-content-1";
                    let title = /*value.MinValue > 0 ? experiencTag.Title + " &ge; " + value.MinValue :*/ experiencTag.Title;
                    let element = $(`<div class="dx-tag-1"><div class="${cssClass}"><span>${title}</span><div onclick="DeleteProjectTag(${value.TagId}, ${value.Type})"></div></div></div>`);
                    $("#projectExpTags").append(element);
                }
            });
        }
    }
    function findMatch(arrProj, arrUser) {
        return arrProj.some(item => arrUser.includes(item))
    }
    function openResourceTimeSheet(assignedTo, assignedToName, selectedYear) {
        showTimeSheet = true;
        //param isRedirectFromCardView is used to hide card view and show allocation grid
        //param ShowUserResume is used to show user resume page.
        if (selectedYear == undefined) {
            selectedYear = new Date().getFullYear();
        }
        var url = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGridNew&ViewName=FindAvailability&isRedirectFromCardView=true&showuserresume=true&selectedYear=" + selectedYear + "&selecteddepartment=-1&SelectedResource=" + assignedTo;
        window.parent.UgitOpenPopupDialog(url, "", "Timeline for User : " + assignedToName, "95", "95", "", false);
    }

    function getNextMonday() {
        var startDate = new Date($("#lblSearchStart").dxDateBox('instance').option('value'));
        var daysUntilNextMonday = (1 - startDate.getDay() + 7);
        allocModel.StartDate = new Date(startDate.setDate(startDate.getDate() + daysUntilNextMonday)).toDateString().slice(4);
        allocModel.EndDate = new Date(allocModel.StartDate).addDays(7).toDateString().slice(4);
        $("#lblSearchStart").dxDateBox({ "value": new Date(allocModel.StartDate).toDateString().slice(4) });
    }

    function getPreviousMonday() {
        var startDate = new Date($("#lblSearchStart").dxDateBox('instance').option('value'));
        allocModel.StartDate = new Date(startDate.setDate(startDate.getDate() - (startDate.getDay() + 6))).toDateString().slice(4);
        allocModel.EndDate = new Date(allocModel.StartDate).addDays(7).toDateString().slice(4);
        $("#lblSearchStart").dxDateBox({ "value": new Date(allocModel.StartDate).toDateString().slice(4) });
    }
</script>

<dx:ASPxHiddenField ID="hdnDataFilters" runat="server" ClientInstanceName="hdnDataFilters"></dx:ASPxHiddenField>
<dx:ASPxLoadingPanel ID="loadingPanel" runat="server" Modal="true" Text="Refreshing.." ClientInstanceName="loadingPanel" 
    Image-Url="~/Content/Images/ajax-loader.gif"></dx:ASPxLoadingPanel>

<div class="container-flex pt-1 pl-3 pr-2 pb-1">
    <div>
        <img width="85" height="85" style="border-radius: 50px; padding: 10px;" src="<%=ImageURL%>" />
    </div>
    <div>
        <div class="searchLabel" style="margin-left:25px;">Search Week</div>
        <div style="display: flex;">
            <div id="divPreviousDate" onclick="getPreviousMonday()" style="padding-right: 5px; margin-top: 8px;">
                <img src="/Content/images/back-arrowBlue.png" style="width: 20px;filter: brightness(0) invert(0);" /></div>
            <div id="lblSearchStart" class="searchTextbox" style="width: 170px;"></div>
            <div id="divStartDate" onclick="getNextMonday()" style="padding-left: 5px; margin-top: 8px;">
                <img src="/Content/images/next-arrowBlue.png" style="width: 20px;filter: brightness(0) invert(0);" /></div>
        </div>
    </div>
    <div>
        <div class="searchLabel">Division</div>
        <div id="ddlDivision" class="searchRoundControl" style="width: 250px;"></div>
    </div>
    <div class="departmentDiv">
        <div class="searchLabel ">Department</div>
        <div id="ddlDepartment" class="searchRoundControl" style="width: 250px;"></div>
    </div>
    <div>
        <div class="searchLabel">Function</div>
        <div id="tboxFunctionalRoles" class="searchRoundControl" style="width: 250px;"></div>
    </div>
    <div>
        <div class="searchLabel">Roles</div>
        <div id="tboxAssignedRoles" class="searchRoundControl" style="width: 250px;"></div>
    </div>
    <div>
        <div class="searchLabel" style="color: white;">.</div>
        <div id="chkIncludeSoft" class="text-right"></div>
    </div>
    <div>
        <div class="searchLabel" style="color: white;">.</div>
        <div id="currentAllocation" class="btnAddNew text-right"></div>
    </div>
</div>

<div class="row pt-2 pr-2 pl-3">
    <div id="gridContainer"></div>
</div>

<div class="row mt-2 pr-2">
    <div class="col-md-12 col-sm-12 p-0" style="text-align: right;">
        <div id="saveAllocation" class="btnAddNew text-right"></div>
    </div>
</div>
<div id="tagsPopup"></div>
<div id="ConflictWeekDataGridDialog"></div>
<div id="toast"></div>
<div id="ConflictWeekDataGridDialogSummary"></div>



