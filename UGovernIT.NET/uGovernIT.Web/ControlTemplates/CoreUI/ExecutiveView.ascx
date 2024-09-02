<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExecutiveView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.CoreUI.ExecutiveView" %>
<%@ Register Src="~/ControlTemplates/UserWelcomePanel.ascx" TagPrefix="ugit" TagName="UserWelcomePanel" %>
<%@ Register Src="~/ControlTemplates/CoreUI/RoleAllocationsView.ascx" TagPrefix="ugit" TagName="RoleAllocationsView" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #filters{
        /*padding-left:5px;*/
        height:600px;
        padding:0px;
    }

    .piechartclass rect{
        opacity:0;
    }

    ul#filters > li {
        margin: 15px;
        /*box-shadow: 0 1px 2px #ccc;*/
        border: 1px solid lightgrey;
        padding-top: 13px;
        padding-bottom: 10px;
        list-style: none;
        border-radius: 12px;
        margin-top:30px;
    }

    .radioGroup .dx-radiobutton{
            display: flex;
            flex-direction: column;
            align-items: center;
    }

    .radioGroup .dx-radio-value-container{
        /*padding-right:0px;*/
    }

    .sector-tooltip{
        font-weight: bold;
        text-align:center;
    }

    
    .dx-radiobutton-icon-checked .dx-radiobutton-icon-dot{
        margin-left:0px;
    }
</style>
<style id="cardsScript" data-v="<%=UGITUtility.AssemblyVersion %>">
    .TileStyle {
        border: 10px;
        padding: 15px;
        height: 250px;
        font-size: 13px;
        box-shadow: 3px 1px 5px rgb(0 0 0 / 20%);
        border: 1px solid #CCC;
        border-radius: 10px;
        cursor: pointer;
    }
    .MonthTitle {
        font-family: 'Poppins', sans-serif !important;
        font-size: 18px;
        font-weight: 600;
        padding-left: 10px;
        padding-right: 0px;
        color: #4A6EE2;
        text-transform: uppercase;
        text-align:center;
    }
    .SelectItem{
        padding:5px;
    }
    .rowitem{
        margin:6px 0px 6px 0px;
    }
    b{
        font-weight:bold;
        color:grey;
    }

    .discription-title {
        text-align: center;
        font-size: 24px;
        font-weight: 600;
        margin-bottom: 10px;
        color: black;
        font-family: 'Roboto', sans-serif !important;
        background-color: white;
        /*border-radius: 7px;*/
        padding: 5px;
        /*margin-right: 10px*/
    }
    .dashboard-panel-new {
        border: none !important;
        padding: 15px;
        background-color: #DFDFDF !important;
    }

    .thirdRow {
        background-color: white;
        /*border-radius:7px;
margin-right:10px !important;*/
        height: 640px;
    }

    .secondRow {
        background-color: white;
        /*border-radius: 7px;
margin-right: 10px;*/
    }

    .cardPanelSpacing {
        border-left: 10px solid #DFDFDF;
        border-right: 10px solid #DFDFDF;
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var filterone = [{ Name: "Tracked Work", Value: "Pipeline" }, { Name: "Contracted", Value: "Contracted" }, { Name: "Closed", Value: "Closed" }];
    var filtertwo = [{ Name: "Sector", Value: "Sector" }, { Name: '<%=DivisionLabel%>', Value: "Division" }, { Name: '<%=StudioLabel%>', Value: "Studio" }];
    var filterthree = ["Financial", "Resource"];

    var date = new Date();
    var startDate = addMonths(new Date(), - 12);
    var endDate = addMonths(new Date(), 0);
    var request = {
        Filter: 'Pipeline',
        Base: 'Sector',
        Type: 'Financial',
        Selection: '',
        StartDate: new Date(startDate).toUTCString(),
        EndDate: new Date(endDate).toUTCString()
    }

    
    var reportParam =
    {
        StartDate: new Date(startDate).toUTCString(),
        EndDate: new Date(endDate).toUTCString(),
        Mode: "Monthly",
        OPM: true,
        CPR: true,
        CNS: true,
        Pipeline: true,
        Current: true,
        Closed: true
    };

    var products = [];
    var topProducts = [];
    var lastProucts = [];
    var baseUrl = ugitConfig.apiBaseUrl;

    $(() => {

        $(".resourceViewContainer").height('<%=Height%>' == '' ? '300px' : '<%=Height%>');
        $(".resourceViewContainer").width('<%=Width%>' == '' ? '300px' : '<%=Width%>');

        $('#txtStartDate').dxDateBox({
            type: 'date',
            value: request.StartDate,
            onValueChanged(data) {
                var endDate = $("#txtEndDate").dxDateBox("instance");
                var enddateString = endDate.option("value");
                request.StartDate = data.value.toLocaleString();
                request.EndDate = enddateString.toLocaleString();
                LoadRolesGrid(request.Filter, request.Base, request.Type, '', '');
                LoadPieChart();
            },
        });

        $('#txtEndDate').dxDateBox({
            type: 'date',
            value: request.EndDate,
            onValueChanged(data) {
                var startDate = $("#txtStartDate").dxDateBox("instance");
                var startdateString = startDate.option("value");
                request.StartDate = startdateString.toLocaleString();
                request.EndDate = data.value.toLocaleString();
                LoadRolesGrid(request.Filter, request.Base, request.Type, '', '');
                LoadPieChart();
            },
        });

        $("#filterone").dxRadioGroup({
            dataSource: filterone,
            value: filterone[0].Name,
            displayExpr: 'Name',
            valueExpr: 'Value',
            itemTemplate: function (itemData, itemIndex, itemElement) {
                itemElement.append(
                    $("<div />").attr("style", "padding:15px;")
                        .text(itemData.Name)
                );
            },
            onValueChanged(e) {
                request.Filter = e.value;
                LoadRolesGrid(request.Filter, request.Base, request.Type, '', '');
                LoadPieChart();
            },
            elementAttr: {
                class:'radioGroup'
            },
        });

        $("#filtertwo").dxRadioGroup({
            dataSource: filtertwo,
            value: filtertwo[0].Name,
            displayExpr: 'Name',
            valueExpr: 'Value',
            itemTemplate: function (itemData, itemIndex, itemElement) {
                itemElement.append(
                    $("<div />").attr("style", "padding:15px;")
                        .text(itemData.Name)
                );
            },
            onValueChanged(e) {
                request.Base = e.value;
                LoadRolesGrid(request.Filter, request.Base, request.Type, '', '');
                LoadPieChart();
            },
            elementAttr: {
                class: 'radioGroup'
            },
        });

        $("#filterthree").dxRadioGroup({
            items: filterthree,
            value: filterthree[0],
            visible:false,
            itemTemplate: function (itemData, itemIndex, itemElement) {
                itemElement.append(
                    $("<div />").attr("style", "padding:5px;")
                        .text(itemData)
                );
            },
            onValueChanged(e) {
                request.Type = e.value;
                
                LoadPieChart();
            },
            elementAttr: {
                class: 'radioGroup'
            },
        });

        $("#btnSwitchtoMain").dxButton({
            icon: '/Content/Images/sectorIcon.png',
            height: 35,
            width: 35,
            hint:'Switch To Main',
            onClick: function (e) {
                let filter1 = $("#filterone").dxRadioGroup("instance");
                let filter2 = $("#filtertwo").dxRadioGroup("instance");
                
                request.Filter = filter1.option('value');
                request.Base = 'Sector'; // filter2.option('value');
                filter2.option('value', 'Sector');
                request.Selection = '';
                LoadRolesGrid(request.Filter, request.Base, request.Type);
                LoadPieChart();
            }
        });

        $("#btnProjectComplexity").dxButton({
            icon: '/Content/Images/Analytics/capacity.png',
            height: 35,
            width: 35,
            hint: 'Role Report',
            onClick: function (e) {
                window.parent.UgitOpenPopupDialog("/layouts/uGovernIT/DelegateControl.aspx?control=capcityreport&pDepartmentName=&IsChartDrillDown=true&Filter=" + request.Filter, "", "Capacity Report", 90, 90, 0, '');
            }
        });

        $("#btnUtilization").dxButton({
            icon: '/Content/Images/Analytics/utilization.png',
            height: 35,
            width: 35,
            hint: 'Resource Report',
            onClick: function (e) {
                window.parent.UgitOpenPopupDialog("/layouts/uGovernIT/DelegateControl.aspx?control=resourceavailability&pDepartmentName=&IsChartDrillDown=true&Filter=" + request.Filter, "", "Resource Utilization", 90, 90, 0, '');
            }
        });

        //$("#divRoleGrid").click(function () {
        //    window.parent.UgitOpenPopupDialog('/layouts/ugovernit/delegatecontrol.aspx?control=capcityreport', '', 'Resource Utilization', '90', '90', '', true);
        //});

        LoadPieChart();
        LoadRolesGrid(request.Filter, request.Base, request.Type);
    });

    function LoadPieChart() {

        $.get(baseUrl + "/api/CoreRMM/GetExecutiveChartData?Filter=" + request.Filter + "&Base=" + request.Base + "&Type=" + request.Type
            + "&StartDate=" + request.StartDate + "&EndDate=" + request.EndDate, function (data) {
            products = data;
            topProducts = products.slice(-5);
            if (products.length > 6) {
                lastProucts = products.slice(0, products.length - 5);
                if (lastProucts.length > 0)
                    topProducts.push({
                        Name: 'Others',
                        Utilization: lastProucts.map(item => item.Utilization).reduce((prev, next) => prev + next) / lastProucts.length,
                        Capacity: lastProucts.map(item => item.Capacity).reduce((prev, next) => prev + next),
                        DataType: 'Roles'
                    });
            } else {
                topProducts = products;
            }

            let valueField = "Utilization";

            $('#piechart').dxPieChart({
                size: {
                    height: 550,
                    width: 550,
                },
                palette: 'Soft Blue',
                dataSource: topProducts,
                series: [
                    {
                        argumentField: 'Name',
                        valueField: valueField,
                        label: {
                            visible: true,
                            position:'inside',
                            connector: {
                                visible: true,
                                width: 1,
                            },
                            font: {
                                size:16
                            },
                            customizeText(point) {
                                var argText = getFirstNWords(point.argumentText, 4);
                                return `${argText.replace(/ /g, "\r\n")}<br>${point.value.toFixed(1)} %: HC ${point.point.data.Capacity}`;
                            },
                        },
                        minSegmentSize: 1000000,
                        smallValuesGrouping: {
                            groupName: "others",
                            mode: "none",
                        },
                        //hoverMode: "none"
                    },
                ],
                legend: {
                    orientation: 'horizontal',
                    verticalAlignment: 'bottom',
                    horizontalAlignment: 'center',
                    visible: false,
                },
                export: {
                    enabled: false,
                },
                tooltip: {
                    enabled: true,
                    contentTemplate(pointInfo, container) {
                        return `${pointInfo.argumentText.replace(/ /g, "\r\n")}<br><div style='width:100%'>${pointInfo.value}</div>`;
                    },
                },
                resolveLabelOverlapping: 'shift',
                onPointClick(e) {
                    const point = e.target;
                    var piechart = e.component;
                    if (point.data.Name == "Others") {
                        //do not allow to show others categories if they are too large
                        if (lastProucts.length < 10) {
                            piechart.option("dataSource", lastProucts);
                            piechart.option("palette", "Soft Blue");
                        }

                    } else {
                        //debugger;
                        if (point.data.DataType == request.Base) {
                            let selectedSector = encodeURIComponent(point.data.Lookup); //point.data.Lookup;
                            
                            var base = 'Role'
                            if (request.Base == 'Role')
                                base = 'Sector';
                          
                            //$.get(baseUrl + "/api/CoreRMM/GetExecutiveChartData?Filter=" + request.Filter + "&Base=" + base + "&Type=" + request.Type
                            //    + "&Selection=" + selectedSector + "&SelectionDataType=" + point.data.DataType,
                            //    function (breakupdata) {
                                    
                            //        products = breakupdata;
                            //        topProducts = products.slice(-5);
                            //        if (products.length > 5) {
                            //            lastProucts = products.slice(0, products.length - 5);
                            //            if (lastProucts.length > 0)
                            //                topProducts.push({
                            //                    Name: 'Others',
                            //                    Utilization: lastProucts.map(item => item.Utilization).reduce((prev, next) => prev + next) / lastProucts.length,
                            //                    Capacity: lastProucts.map(item => item.Capacity).reduce((prev, next) => prev + next),
                            //                    DataType: lastProucts[0].DataType
                            //                });
                            //        }
                            //        piechart.option("dataSource", topProducts);
                            //        piechart.option("palette", "Harmony Light");

                            //    });
                            LoadRolesGrid(request.Filter, base, request.Type, selectedSector, point.data.DataType);
                            const param = "";
                            let headertitle = "Capacity Report: " + point.data.Name;
                            if (point.data.DataType == "Sector")
                                window.parent.UgitOpenPopupDialog("/layouts/uGovernIT/DelegateControl.aspx?control=capcityreport&pDepartmentName=&IsChartDrillDown=true&Filter="
                                    + request.Filter + "&Sector=" + selectedSector, "", headertitle, 90, 90, 0, '');
                            else if (point.data.DataType == "Division")
                                window.parent.UgitOpenPopupDialog("/layouts/uGovernIT/DelegateControl.aspx?control=capcityreport&pDepartmentName=&IsChartDrillDown=true&Filter="
                                    + request.Filter + "&Division=" + selectedSector, "", headertitle, 90, 90, 0, '');
                            else if (point.data.DataType == "Studio")
                                window.parent.UgitOpenPopupDialog("/layouts/uGovernIT/DelegateControl.aspx?control=capcityreport&pDepartmentName=&IsChartDrillDown=true&Filter="
                                    + request.Filter + "&Studio=" + selectedSector, "", headertitle, 90, 90, 0, '');

                        }
                        else {
                            //last drill down
                        }
                    }
                },
                customizePoint: function (pointInfo) {
                    var index = pointInfo.index;
                    if (index > 9)
                        index = index - 9;
                    return { color: colorCodes[index], hoverStyle: { color: colorCodes[index] } };
                },
            });


        });
    }

    function getText(item, text) {
        return `<a>${products[item.index].Name}</a> `;
    }

    function LoadRolesGrid(filter, base, type, selection, selectiondatatype) {
        $.get(baseUrl + "/api/CoreRMM/GetExecutiveChartData?Filter=" + filter + "&Base=Role&Type=" + type
            + "&Selection=" + selection + "&SelectionDataType=" + selectiondatatype + "&StartDate=" + request.StartDate + "&EndDate=" + request.EndDate,
            function (gridbreakupdata) {
                var roleAllocations = $("#divRoleGrid").dxDataGrid({
                    dataSource: gridbreakupdata,
                    width: 450,
                    height: 597,
                    showRowLines: true,
                    showBorders: true,
                    paging: {
                        enabled: false,
                    },
                    sorting: {
                        mode: 'multiple',
                    },
                    columns: [
                        {
                            dataField: 'Name',
                            caption: 'Role',
                            width: 200,
                        },
                        {
                            dataField: 'Capacity',
                            caption: 'Head Count',
                            sortOrder: 'desc',
                            sortIndex: 0,
                            alignment:'center'
                        },
                        {
                            dataField: 'Utilization',
                            caption: 'YTD %',
                            format: "#0.#",
                            alignment:'center',
                            customizeText: (options) => {
                                return options.valueText + '%';
                            }
                        }
                    ],
                    onRowPrepared: function (e) {
                        //debugger;
                        if (e.rowType == 'header') {
                            e.rowElement.css({ font: 'bold !important' })
                        }
                        if (e.rowType == 'data') {
                            //e.rowElement.css({ height: 35 });
                            if (e.data.RoleName == 'Overall')
                                e.rowElement.css({ color: '#4A6EE2' })
                        }
                    }
                });
                
            });

    }
    
</script>

<div class="row" style="margin-top:-4px;">
    <div class="discription-title">Utilization Analytics</div>
    <div class="resourceViewContainer" style="overflow: auto">
        <div class="title margin fontsize">
            <div class="row secondRow">
                <div class="col-lg-4 col-md-3 col-sm-3 col-xs-3"></div>
                <div class="col-lg-2 col-md-3 col-sm-3 col-xs-3 pr-1" <%--style="margin-right: 10px"--%>>
                    <div id="txtStartDate" style="margin: 5px;width: 127px"></div>
                </div>
                <div class="col-lg-2 col-md-3 col-sm-3 col-xs-3 pl-1" <%--style="margin-left: 10px"--%>>
                    <div id="txtEndDate" style="margin: 5px;width: 127px"></div>
                </div>
                <div class="margin" style="float: right;margin:5px;">
                    <div id="btnUtilization">
                    </div>
                    <div id="btnProjectComplexity">
                    </div>
                    <div id="btnSwitchtoMain">
                    </div>
                </div>
            </div>

            <div class="row d-flex justify-content-between" style="padding-top: 10px;">

                <div class="noPadding thirdRow" style="width:44%;">
                    <div class="piechartclass" id="piechart" style="padding-top:30px;">
                    </div>
                    <%--<div class="row" style="padding-top: 20px;">
                        
                    </div>--%>
                </div>
                <div class="noPadding thirdRow cardPanelSpacing" style="width:12%;">
                    <ul id="filters">
                        <li>
                            <div id="filterone"></div>
                        </li>
                        <li>
                            <div id="filtertwo"></div>
                        </li>
                    </ul>
                </div>
                <div class="noPadding thirdRow" style="width:44%;">
                    <div id="divRoleGrid" style="margin-top:10px;">
                    </div>
                </div>
            </div>
        </div>

    </div>
</div>
<div id="popup"></div>


