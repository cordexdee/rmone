<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BenchReport.ascx.cs" Inherits="uGovernIT.Web.BenchReport" %>

<style>
    .displayModeDiv {
        display: flex;
        justify-content: space-around;
    }
</style>
<script>
    const baseUrl = ugitConfig.apiBaseUrl;
    const tabSource = [
        {
            icon: 'description',
            title: 'Bench(5) ',
        },
        {
            icon: 'taskhelpneeded',
            title: 'Over Allocated(5)',
        }
    ];
    var globaldata = [];
    var windowFilters = {
        IncludeAllResources: 0,
        IncludeClosedProject: 0,
        DisplayMode: "HRS",
        Department: "",
        Function: "",
        StartDate: "",
        EndDate: "",
        ResourceManager: "",
        AllocationType: "",
        LevelName: "",
        GlobalRoleId: "",
        Mode: "",
    };
    var displayModes = ["HRS","PCT"];

    // Get current date
    var currentDate = new Date();
    var currentYear = currentDate.getFullYear();
    var startDate = new Date('<%=StartDate%>'); 
    var endDate = new Date('<%=EndDate%>');
    var dtBenchAndOverAllocatedResources;
    var findResource = {
        ID:"",
        ModuleName :"",
        ProjectID :"",
        GroupID: "",
        AllocationStartDate: startDate.toISOString(),
        AllocationEndDate: endDate.toISOString(),
        ResourceAvailability:"FullyAvailable",
        Complexity :"",
        ProjectVolume :"",
        ProjectCount :"",
        Type :"",
        PctAllocation :"",
        RequestTypes :"",
        ModuleIncludes :"",    
        JobTitles :"",
        departments :"",
        SelectedUserID :"",
        isAllocationView :"",
        Customer :"",
        CompanyLookup :"",
        Sector :"",
        SectorName :"",
        SelectedTags :"",
        PctAllocationCloseOut :"",
        PctAllocationConst :"",
        DivisionId: "",
        FunctionId: "",
        ResourceManager: ""
    };

    $(() => {

        $('#rdbDisplayMode').dxRadioGroup({
            items: displayModes,
            value: displayModes[0],
            layout: 'horizontal',
            visible:false,
            itemTemplate: function (itemData, itemIndex, itemElement) {
                console.log(itemData);
                if (itemData == "HRS")
                    itemElement.text("Hours");
                else if (itemData == "PCT")
                    itemElement.text("Percentage");
            },
            onValueChanged(e) {
                windowFilters.DisplayMode = e.value;
                LoadChart();
            },
        });

        LoadChart();
        LoadBenchAndOverAllocatedResources();
    });
</script>
<script>
    function LoadChart() {
        $.get(baseUrl + "/api/Bench/GetBenchChartData?" + $.param(windowFilters), function (datagraph) {
            //adding same color code from pie chart data

            $('#divbarchart').dxChart({
                dataSource: datagraph,
                width: 600,
                height: 600,
                palette: 'soft',
                rotated: true,
                legend: {
                    visible: false
                },
                title: {
                    text: 'Availability/Over Utilization',
                    font: {
                        color: '#000',
                        size: 16,
                        weight: 500,
                        family: 'Roboto, sans-serif !important',
                    }
                },
                argumentAxis: {
                    label: {
                        visible: true
                    }
                },
                series: {
                    argumentField: 'Month',
                    valueField: 'Utilization',
                    type: 'bar',
                    label: {
                        position: 'outside',
                        visible: true,
                        font: {
                            size: 16,
                        },
                        customizeText(point) {

                            let argText = " " + point.argumentText;
                            return `<div>${point.value}</div>`;
                        },
                        backgroundColor: 'none',
                    },
                },
                onPointClick(e) {

                },
            });

        });

    }
    function LoadBenchAndOverAllocatedResources() {
        
        const tabPanel = $('#divTab').dxTabPanel({
            height: 450,
            dataSource: tabSource,
            selectedIndex: 0,
            loop: false,
            animationEnabled: true,
            swipeEnabled: true,
            itemTemplate: function (itemData, itemIndex, itemElement) {
                let pctColumnCaption = "Avail.";
                if (itemIndex == 0) {
                    findResource.ResourceAvailability = "FullyAvailable";
                    pctColumnCaption = "Avg Avail.";
                }
                else {
                    findResource.ResourceAvailability = "AllResource";
                    pctColumnCaption = "Over Alloc.";
                }
                $.get("/api/Bench/GetBenchAndOverAllocatedResources?" + $.param(findResource), function (data, status) {
                    
                    var grid = $(`<div id='divResourceGrid' />`).dxDataGrid({
                        dataSource: data,
                        remoteOperations: false,
                        searchPanel: {
                            visible: false,
                        },
                        rowAlternationEnabled: true,
                        wordWrapEnabled: true,
                        showBorders: false,
                        showColumnHeaders: true,
                        showColumnLines: false,
                        showRowLines: false,
                        TextEncoder: true,
                        noDataText: "No Data Found",
                        columns: [
                            {
                                dataField: "AssignedToName",
                                dataType: "text",
                                caption: "Team Member",
                                allowEditing: false,
                                sortIndex: "0",
                                sortOrder: "asc",
                                width: "40%",
                                cellTemplate: function (container, options) {
                                    container.css('display', 'flex');
                                    let imageUrl = "";
                                    if (options.data.UserImagePath && options.data.UserImagePath.indexOf("userNew.png") != -1) {
                                        imageUrl = "/Content/Images/RMONE/blankImg.png";
                                    }
                                    else {
                                        imageUrl = options.data.UserImagePath;
                                    }
                                    $(`<img src="${imageUrl}" style="width:48px; height:48px; border-radius: 50%;border:1px solid black;"><div><div style='margin-left:5px;font-weight:600;'>${options.data.AssignedToName}</div><div style='margin-left:5px;'>${options.data.JobTitle}</div></div>`).appendTo(container);
                                },
                                headerCellTemplate: function (header, info) {
                                    $(`<div style="color: black;font-size:14px;font-weight:600;margin-left:28px;">${info.column.caption}</div>`).appendTo(header);
                                }
                            },
                            {
                                dataField: "PctAllocation",
                                dataType: "text",
                                caption: pctColumnCaption,
                                width: "20%",
                                cellTemplate: function (container, options) {
                                    $("<div>(" + Math.abs(100 - Number(options.data.PctAllocation)).toFixed(1) + " %)</div>").appendTo(container);
                                }
                            },
                            {
                                dataField: "RoleName",
                                dataType: "text",
                                caption: "Role",
                                sortIndex: "1",
                                sortOrder: "asc",
                                width: "40%",
                                headerCellTemplate: function (header, info) {
                                    $(`<div style="color: black;font-size:14px;font-weight:600;">${info.column.caption}</div>`).appendTo(header);
                                }
                            },


                        ]
                    });
                    itemElement.append(grid);

                });
            },
            onSelectionChanged(e) {
                debugger;

            },
        }).dxTabPanel('instance');

    }
    function LoadTabs(itemElement) {
        
    }
    function onDepartmentChanged(ccID) {
        debugger;
        var cmbDepartment = $("#" + ccID + " span");
        var selectedDepartments = cmbDepartment.attr("id");
        windowFilters.Department = selectedDepartments;
        findResource.departments = selectedDepartments;
        LoadChart();
        LoadBenchAndOverAllocatedResources();
    }

    function cmbFunctionRole_Changed(s, e) {
        debugger;
        var functionid = s.GetValue();
        windowFilters.Function = functionid;
        findResource.FunctionId = functionid;
        LoadChart();
        LoadBenchAndOverAllocatedResources();
    }

    function cmbRole_onSelectedIndexChanged(s, e) {
        debugger;
        var roleid = s.GetValue();
        windowFilters.GlobalRoleId = roleid;
        findResource.Type = roleid;
        LoadChart();
        LoadBenchAndOverAllocatedResources();
    }

    function OnCmbManager_IndexChanged(s, e) {
        debugger;
        var selectedManager = cmbResourceManager.GetValue();
        windowFilters.ResourceManager = selectedManager;
        findResource.ResourceManager = selectedManager;
        LoadChart();
        LoadBenchAndOverAllocatedResources();
    }
</script>
<div class="container">
    <div class="row" style="background-color:#dfdfdf;">
        <div id="divFilter" runat="server" >
            <div class="col-md-12 col-sm-12 col-xs-12 noSidePadding">
                <div class="row resourceUti-filterWarp">
                    <div class="col-xl-2 col-md-3 col-sm-3 col-xs-12 colForTabView">
                        <div class="resourceUti-label">
                            <label>Department:</label>
                        </div>
                        <div class="resourceUti-inputField">
                            <ugit:LookupValueBoxEdit ID="ddlDepartment" runat="server" IsMulti="true" CssClass="rmmLookup-valueBoxEdit rmmDepartment-drpDown" 
                                FieldName="DepartmentLookup" JsCallbackEvent="onDepartmentChanged" />
                        </div>
                    </div>
                    <div class="col-xl-2 col-md-3 col-sm-3 col-xs-12 colForTabView">
                        <div class="resourceUti-label">
                            <label>Function:</label>
                        </div>
                        <div class="resourceUti-inputField">
                            <dx:ASPxComboBox ID="cmbFunctionRole" runat="server" Width="100%">
                                <ClientSideEvents SelectedIndexChanged="cmbFunctionRole_Changed" />
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    
                    <div class="col-xl-2 col-md-3 col-sm-3 col-xs-12 colForTabView">
                        <div class="resourceUti-label">
                            <label>Roles:</label>
                        </div>
                        <div class="resourceUti-inputField">
                            <dx:ASPxCallbackPanel ID="cbpResourceAvailability" ClientInstanceName="cbpResourceAvailability" runat="server"  SettingsLoadingPanel-Enabled="false">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <dx:ASPxComboBox ID="cmbRole" ClientInstanceName="cmbRole" runat="server" DropDownStyle="DropDown" IncrementalFilteringMode="StartsWith"
                                             EnableSynchronization="True" CssClass="aspxComboBox-dropdown" ListBoxStyle-CssClass="aspxComboBox-listBox" AutoPostBack="false">
                                            <ClientSideEvents SelectedIndexChanged="cmbRole_onSelectedIndexChanged" />
                                        </dx:ASPxComboBox>
                                        <asp:HiddenField ID="hdnaspDepartment" runat="server" />
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxCallbackPanel>                       

                        </div>
                    </div>
                    <div class="col-xl-2 col-md-3 col-sm-3 col-xs-12 colForTabView clearBoth-forSM">
                        <div class="resourceUti-label" id="tdManager" runat="server">
                            <label>Manager:</label>
                        </div>
                        <div class="resourceUti-inputField" id="tdManagerDropDown" runat="server">
                            <dx:ASPxComboBox ClientInstanceName="cmbResourceManager" runat="server" ID="cmbResourceManager"
                                            DropDownStyle="DropDown" IncrementalFilteringMode="StartsWith" EnableSynchronization="true" CssClass="aspxComboBox-dropdown"
                                            ListBoxStyle-CssClass="aspxComboBox-listBox" AutoPostBack="false">
                                            <ClientSideEvents SelectedIndexChanged="OnCmbManager_IndexChanged" />
                                        </dx:ASPxComboBox>
                        </div>
                    </div>
                 
                </div>
            </div>
        </div>
    </div>
    <div class="row pt-2">
        <div class="col-lg-7 col-md-7 col-sm-7 col-xs-7">
            <div class="row">    
                <div id="rdbDisplayMode" class="displayModeDiv"></div>
            </div>
            <div class="row">
                <div id="divbarchart"></div>
            </div>
        </div>
        <div class="col-lg-5 col-md-5 col-sm-5 col-xs-5">
            <div id="divTab"></div>
        </div>
    </div>
</div>