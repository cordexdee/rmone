<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EstimatedBillingHoursAnalytic.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMONE.EstimatedBillingHoursAnalytic" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .bg-g {
        background-color: #DFDFDF;
    }
    .whiteBgRadius {
        /*box-shadow: 0 6px 12px #00000020;*/
        background-color: #fff;
        border-radius: 6px;
    }

    .title-t {
        color: #000;
        font-size: 16px;
        font-weight: 600;
    }
    .color-dBlue {
        color: #030644;
    }
    .color-lBlue {
        color: #00b9ff;
    }
    .color-Gray {
        color: #a6a6a6;
    }
    .cusTitle {
        font-size: 22px;
        font-weight: 600;
        text-align: center;
        margin-top: 0;
        margin-bottom: 6px;
    }
    .textt {
        color: #000;
        font-size: 12px;
        text-align: center;
        margin-bottom: 6px;
    }
    .bdr {
        border-right: 2px solid #DFDFDF;
    }

    .fws {
        font-size: 16px;
    }
/*
    #projectsDoughnut{
        height:700px;
    }*/

    .discription-title {
        text-align: center;
        font-size: 24px;
        font-weight: 600;
        color: black;
        font-family: 'Roboto', sans-serif !important;
        background-color: white;
        margin-right: 15px;
        margin-left: 15px;
        border-radius: 7px;
        margin-bottom: 10px;
        padding: 10px;
    }
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var request = {
        Filter: 'Division',
        Base: 'Sector',
        Type: 'Hours',
        Selection: '',
        Period: "Yearly",
        Division: '0',
        Role: 'All',
        Duration: 'Yearly'
    }

    var periodSelectBoxItems = [
        { text: "6 Months", value: "HalfYearly" },
        { text: "1 Year", value: "Yearly" },
        { text: "2 Year", value: "TwoYear" },
    ];

    var filterSelectBoxItems = [
        { text: "<%=DivisionLabel%>", value: 0 },
        { text: "Sector", value: 1 },
    ];
    const SqftArea = ["XS", "S", "M", "L", "XL"];
    var baseUrl = ugitConfig.apiBaseUrl;
    var Divisions = ["All Divisions", "San Francisco", "Las Vegas"];
    var Roles = ["All Roles", "Project Manager"];
    var Timeframes = ["Yearly", "Quaterly", "HalfYearly"];
    var WeekRendered = [];
    const monthNames = ["JAN", "FEB", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUG", "SEP", "OCT", "NOV", "DEC"];
    var date = new Date();
    var prevMonday = new Date(date.setDate(date.getDate() - (date.getDay() + 6) % 7));
    prevMonday = prevMonday.toUGITDateFormat('dd/mmm/yyyy');
    var startDate = new Date(date.setMonth(date.getMonth() - 3));
    var endDate = new Date(date.setMonth(date.getMonth() + 3));
    
    var staffColorCodes = [
        //'#105401',
        //'#006A3A',
        //'#007F6D',
        '#759cc9',
        '#8fb1cc',
        '#aeaeae',
        '#949494',
        '#7aada0',
        '#7198a9',
        '#00929E',
        '#00A2C9',
        '#4A5857',
        '#00929E',
        '#00A2C9',
        '#4A5857',
        '#737474',
        '#999A9B',
        '#C2C3C3',
        '#ECEDED'
    ];

    var contextMenuItems = [
        { text: 'Print To PDF' },
        { text: 'Print To PNG' },
        { text: 'Switch To %' },
        { text: 'Switch To Hours' }
    ];

    var NCOChartlabel = '';

    $(document).ready(function () {

        $("#btnYaxis").dxDropDownButton({
            items: contextMenuItems,
            text: 'Menu',
            icon: 'menu',
            width: 120,
            showArrowIcon: false,
            stylingMode: 'text',
            useItemTextAsTitle: true,
            onItemClick: function (e) {
                const itemText = e.itemData.text;
                var trendChart = $("#chart").dxChart("instance");
                if (itemText == 'Switch To Hours') {

                    trendChart.option({
                        series: [
                            { valueField: 'A1', name: 'Opportunities', color: '#A7BEE1' },
                            { valueField: 'A2', name: 'Project', color: '#D39061' },
                        ],
                        valueAxis: {
                            label: {
                                font: {
                                    color: '#000000'
                                }
                            },
                            tick: {
                                visible: true
                            },
                            title: {
                                text: 'Hours',
                            },
                            minorTick: {
                                visible: false
                            },
                            visible: true,
                            axisDivisionFactor: 40
                        },
                    });
                } else if (itemText == 'Switch To %') {

                    trendChart.option({
                        series: [
                            { valueField: 'AvgPctA1', name: 'Opportunities', color: '#A7BEE1' },
                            { valueField: 'AvgPctA2', name: 'Project', color: '#D39061' },
                        ],
                        valueAxis: {
                            label: {
                                font: {
                                    color: '#000000'
                                }
                            },
                            tick: {
                                visible: true
                            },
                            title: {
                                text: '% Allocation',
                            },
                            minorTick: {
                                visible: false
                            },
                            visible: true,
                            axisDivisionFactor: 40
                        },
                    });
                } else if (itemText == 'Print To PDF') {
                    trendChart.exportTo('Exported Chart', 'PDF');
                } else if (itemText == 'Print To PNG') {
                    trendChart.exportTo('Exported Chart', 'PNG');
                }

            }
        });
        $("#ddlFilter").dxSelectBox({
            dataSource: filterSelectBoxItems,
            width:295,
            displayExpr: "text",
            value: filterSelectBoxItems[1].value,
            layout: "horizontal",
            onValueChanged: function (e) {
                NCOChartlabel = 'NCO Hrs By ' + e.value.text;
                if (e.value.value == 0)
                    request.Filter = "Division";
                else
                    request.Filter = "Sector";
                LoadHoursDoughnut();
                var bar = $('#projectsDoughnut').dxChart("instance");
                bar.render();
            }
        });
        $("#ddlPeriod").dxSelectBox({
            dataSource: periodSelectBoxItems,
            displayExpr: "text",
            width:295,
            value: _.findWhere(periodSelectBoxItems, { value: request.Period }),
            layout: "horizontal",
            onValueChanged: function (e) {
                request.Period = e.value.value;
                LoadTrendChart();
                LoadHoursDoughnut();
            }
        });

        LoadHoursDoughnut();
        LoadTrendChart();
    });

    function LoadTrendChart() {
        $.get(`/api/RMOne/GetResourceAllocationTrendChart?Division=${request.Division}&Role=${request.Role}&Filter=${request.Period}&Base=${request.Base}`, function (data, status) {
            var dataSource = data.sort(function (a, b) {
                return new Date(a.week) - new Date(b.week);
            });
            let firstDate = new Date(dataSource[0].week)
            //WeekRendered.push(monthNames[firstDate.getMonth()]);
            dataSource = GetAllocationSlope(dataSource);

            const chart = $('#chart').dxChart({
                dataSource,
                height: 430,
                commonSeriesSettings: {
                    type: 'area',
                    argumentField: 'week',
                    grid: {
                        visible: false,
                    }
                },
                commonAxisSettings: {
                    grid: {
                        visible: false,
                    }
                },
                export: {
                    enabled: false,
                    printingEnabled: false
                },
                series: [
                    { valueField: 'A1', name: 'Opportunities', color: '#A7BEE1' },
                    { valueField: 'A2', name: 'Projects', color: '#D39061' }, //#A7BEE1
                ],
                title: {
                    text: 'Q1 Estimated Resource Hours',
                    font: {
                        color: '#000',
                        size: 16,
                        weight: 500,
                        family: 'Roboto, sans-serif !important',
                    }
                },
                argumentAxis: {
                    label: {
                        font: {
                            color: '#000000'
                        },
                        customizeText: function (e) {
                            const myArray = e.value.split("/");
                            let currentDate = new Date(e.value);

                            let word = '';
                            //if (WeekRendered.includes(monthNames[currentDate.getMonth()]))
                            //    word = '';
                            //else {
                            word = monthNames[currentDate.getMonth()] + '-' + currentDate.getFullYear().toString().substring(2);
                            //    WeekRendered.push(monthNames[currentDate.getMonth()]);
                            //}
                            return word;
                        },
                    },
                    tick: {
                        visible: false
                    },
                    title: {
                        text: '',
                    },
                    minorTick: {
                        visible: true
                    },
                    visible: false,
                    constantLines: [{
                        value: prevMonday,
                        color: '#737373',
                        dashStyle: 'dash',
                        width: 2,
                        label: { text: 'Today' },
                    }],
                },
                valueAxis: {
                    label: {
                        font: {
                            color: '#000000'
                        }
                    },
                    tick: {
                        visible: true
                    },
                    title: {
                        text: '',
                    },
                    minorTick: {
                        visible: false
                    },
                    visible: false,
                    axisDivisionFactor: 40
                },
                legend: {
                    verticalAlignment: 'bottom',
                    horizontalAlignment: 'center',
                },
            }).dxChart('instance');
        });
    }


    function LoadHoursDoughnut() {

        $.get(baseUrl + "/api/RMONE/GetNCOHours?Filter=" + request.Filter + "&Period=" + request.Period
            , function (datagraph) {
                $('#hoursDoughnut').dxPieChart({
                    type: 'doughnut',
                    palette: 'Green Mist',
                    dataSource: datagraph.slice(0,5),
                    series: [
                        {
                            argumentField: 'Title',
                            valueField: 'TotalHrs',
                            label: {
                                visible: true,
                                position: 'outside',
                                connector: {
                                    visible: true,
                                    width: 1,
                                },
                                format: "largeNumber",
                                font: {
                                    size: 16
                                },
                            },
                            minSegmentSize: 1000000,
                            customizeText(pointInfo) {
                                let color = isColorDark(point.point._options.color);
                                if (point.percent == 0)
                                    color = '#000000';
                                return `<div style='color:${color}'>${pointInfo.argumentText.replace(/ /g, "\r\n")}<br><div style='width:100%'>${pointInfo.value}</div></div>`;
                            },
                        },
                    ],
                    centerTemplate(pieChart, container) {
                        const total = pieChart
                            .getAllSeries()[0]
                            .getVisiblePoints()
                            .reduce((s, p) => s + p.originalValue, 0);
                        const content = $(`<svg><circle cx="100" cy="100" fill="#fff" r="${pieChart.getInnerRadius() - 6}"></circle>`
                            + '<text text-anchor="middle" style="font-size: 16px; font-weight: bold;" x="100" y="100" ">'
                            + `<tspan x="100" >NCO Hours <br>By Division</tspan></text></svg>`);

                        container.appendChild(content.get(0));
                    },
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
                            //    piechart.option("dataSource", lastProucts);
                            //    piechart.option("palette", "Soft Blue");
                            }

                        } else {
                            
                            if (point.data.DataType == request.Base) {
                                let selectedSector = encodeURIComponent(point.data.Lookup); //point.data.Lookup;

                                var base = 'Role'
                                if (request.Base == 'Role')
                                    base = 'Sector';

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
                        var text = pointInfo.argument;
                        let index = SqftArea.indexOf(text);
                        return { color: colorCodes[index], hoverStyle: { color: colorCodes[index] } };
                    },
                });

                $('#projectsDoughnut').dxChart({
                    dataSource: datagraph.slice(0, 9),
                    resolveLabelOverlapping: 'hide',
                    //width: 700,
                    palette: 'Green Mist',
                    rotated: true,
                    //scrollBar: {
                    //    visible: true,
                    //},
                    //inverted:true,
                    legend: {
                        visible: false
                    },
                    title: {
                        text: NCOChartlabel,
                        size: 12,
                        font: {
                            color: '#000',
                            size: 16,
                            weight: 500,
                            family: 'Roboto, sans-serif !important',
                        }
                    },
                    customizePoint(e) {
                        return { color: staffColorCodes[e.index], hoverStyle: { color: staffColorCodes[e.index] } };
                    },
                    commonAxisSettings: {
                        grid: {
                            visible: false,
                        }
                    },
                    argumentAxis: {
                        label: {
                            visible: false
                        },
                        tick: {
                            visible: false,
                        },
                        title: {
                            text: '',
                        },
                        minorTick: {
                            visible: false
                        },
                        visible: false
                    },
                    valueAxis: {
                        label: {
                            visible: true
                        },
                        tick: {
                            visible: false,
                        },
                        title: {
                            text: '',
                        },
                        minorTick: {
                            visible: false
                        },
                        visible: false
                    },
                    series: {
                        argumentField: 'Title',
                        valueField: 'TotalHrs',
                        type: 'bar',
                        //barWidth:25,
                        label: {
                            position: 'outside',
                            visible: true,
                            font: {
                                size: 14,
                                color: '#000000',
                            },
                            customizeText(point) {
                                return `${point.argumentText} (${point.point.data.TotalHrsLabel})`;
                            },
                            backgroundColor: 'none',
                        },
                    }
                });

                $('#dataGrid').dxDataGrid({
                    dataSource: datagraph,
                    height: 304,
                    scrolling: {
                        mode: 'infinite',
                    },
                    columns: [
                        {
                            dataField: 'Title',
                            caption: '<%=DivisionLabel%>'
                        },
                        {
                            dataField: 'TotalHrsLabel',
                            caption: 'Project Hrs'
                        },
                        {
                            dataField: '#Projects',
                            caption: '# Projects'
                        },
                        {
                            dataField: '#Resources',
                            caption: '# Resources'
                        }
                    ],
                    remoteOperations: false,
                    searchPanel: {
                        visible: false,
                        highlightCaseSensitive: true,
                    },
                    allowColumnReordering: true,
                    rowAlternationEnabled: true,
                    showBorders: true
                });
            });
    }

    
    function GetAllocationSlope(data) {
        // A1 is OPM
        //A2 is CPR
        let FTEavgTE1 = data.reduce((total, next) => total + parseFloat(next.A1), 2) / data.length;
        let FTEavgTE2 = data.reduce((total, next) => total + parseFloat(next.A2), 2) / data.length;
        const N = data.length;
        let TE1Sum = 0; let TE2Sum = 0;
        data.forEach(function (item, index) {
            let itemDate = new Date(item.week);
            if (itemDate >= new Date()) {
                let TE1 = 0; let TE2 = 0;
                TE1 = (index - (N + 1) / 2) * (parseFloat(item.A1) - parseFloat(FTEavgTE1));
                TE1Sum = TE1Sum + TE1;
                TE2 = (index - (N + 1) / 2) * (index - (N + 1) / 2);
                TE2Sum = TE2Sum + TE2;
                //if (new Date(item.week) >= new Date('08/01/2022'))
                item.TrendSlope = parseFloat(Math.abs(TE1Sum / TE1));
                item.Month = monthNames[itemDate.getDate()];
                //else
                //    item.TrendSlope = null;

            } else {
                item.TrendSlope = null;
            }

            //converting string values to float for chart
            item.A1 = parseFloat(item.A1) + parseFloat(item.A2);
            item.A2 = parseFloat(item.A2);
        });

        return data;
    }
    
</script>
<%--<div class="">
    <div class="discription-title">Financial Analytics</div>
</div>--%>
<div class="estimatedHoursContainer py-3 bg-g">
    <div class="row">
        <div class="discription-title">Staff Analytics</div>
    </div>
    <div class="row">
        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
            <div class="text-center bg-g">
                <div class="d-flex justify-content-betwen pb-3">
                    <div id="ddlFilter"></div>
                    <div class="ml-1" id="ddlPeriod"></div>
                </div>
                <div class="whiteBgRadius d-flex justify-content-center pb-3">
                    <div id="hoursDoughnut"></div>
                </div>
                <div class="whiteBgRadius d-flex justify-content-center pb-5 pt-3">
                    <div id="projectsDoughnut"></div>
                </div>
            </div>
        </div>
        <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
            <%--<h5 class="text-center mt-0 title-t">Q1 Estimated Resource Billing</h5>--%>
            <div class="whiteBgRadius p-3">
                <div id="btnYaxis" style="float:right">Options</div>
                <div id="chart"></div>
            </div>
            <div class="whiteBgRadius py-3 my-3">
                <div class="row d-flex flex-wrap">
                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-6 bdr">
                        <h4 class="color-dBlue cusTitle">68K Hrs</h4>
                        <p class="textt">Proj. Use On 2/12</p>
                        <div class="d-flex justify-content-center align-items-center">
                            <i class="fas fa-circle color-dBlue fws"></i>
                            <p class="textt mb-0 ml-1">Q1 Low</p>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-6 bdr">
                        <h4 class="color-lBlue cusTitle">95K Hrs</h4>
                        <p class="textt">Proj. Use On 3/9</p>
                        <div class="d-flex justify-content-center align-items-center">
                            <i class="fas fa-circle color-lBlue fws"></i>
                            <p class="textt mb-0 ml-1">Q1 High</p>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-6 bdr">
                        <h4 class="color-Gray cusTitle">95K Hrs</h4>
                        <p class="textt">Proj. Use On 2/12</p>
                    </div>
                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-6">
                        <h4 class="color-Gray cusTitle">95K Hrs</h4>
                        <p class="textt">Proj. Use On 2/12</p>
                    </div>
                </div>
            </div>

            <div class="whiteBgRadius p-3">
                <div id="dataGrid"></div>
            </div>
        </div>
    </div>
</div>