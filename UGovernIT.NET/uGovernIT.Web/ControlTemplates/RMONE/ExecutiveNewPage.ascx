<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExecutiveNewPage.ascx.cs" Inherits="uGovernIT.Web.ExecutiveNewPage" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .bg-g {
        background-color: #DFDFDF;
    }
    .filterDiv, .subDiv1, .subDiv2 {
        /*box-shadow: 0 6px 12px #00000020;*/
        background-color: #fff;
        border-radius: 6px;
    }
    .subDiv {
        background-color: #fff;
        border-radius: 6px;
        height: calc(100% - 27px);
    }

    .kpitext{
        font-size: 36px;
        font-weight: 600;
    }
    .kpitext2{
        font-size: 18px;
        font-weight: 500;
    }
    .kpilabel{
        font-size:12px;
    }
    .brr {
        border-right: 2px solid #eee;
        height: 100%;
    }
    .title-t {
        color: #000;
        font-size: 16px;
        /*font-weight: 600;*/
        font-family: 'Roboto', sans-serif !important;
    }
    .color-b {
        color: #444;
    }
    .color-g {
        color: #6DB958;
    }
    .color-r {
        color: #FF0000;
    }
    .filterDiv .dx-texteditor-input {
        font-size: 12px;
    }
    .executivePane{
        height:95%;
    }
   #resourceUtilizationChart{
        height:34.5vh;
    }
    #StaffingChart{
        height:33.5vh;
    }
    .utilizationBar{
        height:38.5vh;
    }
    .staffingBar{
        height:33.5vh;
    }
    .textalign{        
        text-align:center;
    }
    .trendChart{
        height:69.5vh;
    }
    #chart {
        height:66.5vh;
    }
    .m-top {
    margin-top: 29px;
    }

    .discription-title {
        text-align: center;
        font-size: 24px;
        font-weight: 600;
        color: black;
        font-family: 'Roboto', sans-serif !important;
        background-color: white;
        margin-right: 30px;
        margin-left: 30px;
        border-radius: 7px;
        margin-bottom: 10px;
        padding: 10px;
    }

    .discription-value {
        text-align: center;
        font-size: 14px;
        padding: 5px 15px 10px 15px;
        color: black;
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var baseUrl = ugitConfig.apiBaseUrl;
    //var colorList = ['#57A71D', '#BA6047', '#FF3535', '#FA6F00', '#47DA42', '#4FA1D6', '#1C323B', '#70757C', '#B8BBC6', '#FFBD59'];

    var contextMenuItems = [
        { text: 'Print To PDF' },
        { text: 'Print To PNG' },
        { text: 'Switch To Per(%)' },
        { text: 'Switch To Hours' }
    ];

    var Divisions = ["All Divisions", "San Francisco", "Las Vegas"];
    var Roles = ["All Roles", "Project Manager"];
    //var Timeframes = ["HalfYearly", "Yearly", "TwoYear"];
    var Timeframes = [
        {
            "key": "HalfYearly",
            "value": "6 Months"
        },
        {
            "key": "Yearly",
            "value": "1 Year"
        },
        {
            "key": "TwoYear",
            "value": "2 Year"
        }
    ];
    var WeekRendered = [];
    const monthNames = ["JAN", "FEB", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUG", "SEP", "OCT", "NOV", "DEC"];

    var date = new Date();
    date.setUTCDate(0, 0, 0, 0);
    var today = date.toISOString().substring(0, 19);
    var prevMonday = new Date(date.setDate(date.getDate() - (date.getDay() + 6) % 7));
    prevMonday.setUTCHours(0, 0, 0, 0);
    prevMonday = prevMonday.toISOString();
    prevMonday = prevMonday.substring(0, 19);
    
    var datecal = new Date();
    var startDate = new Date(datecal.setMonth(0));
    startDate.setDate(1);
    var endDate = new Date(datecal.setMonth(11));
    endDate.setDate(31);
    
    var request = {
        Division: '0',
        Role: 'All',
        Filter: 'Yearly',
        StartDate: startDate.toUTCString(),
        EndDate: endDate.toUTCString()
    }

    function addMonths(isoDate, numberMonths) {
        var dateObject = new Date(isoDate),
        day = dateObject.getDate(); 
        dateObject.setHours(20);
        dateObject.setMonth(dateObject.getMonth() + numberMonths + 1, 0);
        dateObject.setDate(Math.min(day, dateObject.getDate()));
        return dateObject.toISOString().split('T')[0];
    };

    $(() => {
        $('#txtStartDate').dxDateBox({
            type: 'date',
            value: startDate,
            onValueChanged(data) {
                
                var endDate = $("#txtEndDate").dxDateBox("instance");
                var enddateString = endDate.option("value");
                request.StartDate = data.value.toLocaleString();
                request.EndDate = enddateString.toLocaleString();
                GetResourceAllocationTrendChart();
                GetKeyIndicators();
                GetStaffingData();
                GetResourceUtilizationData("", "division", "", request.StartDate, request.EndDate);
            },
        });

        $('#txtEndDate').dxDateBox({
            type: 'date',
            value: endDate,
            onValueChanged(data) {
                var startDate = $("#txtStartDate").dxDateBox("instance");
                var startdateString = startDate.option("value");
                request.StartDate = startdateString.toLocaleString();
                request.EndDate = data.value.toLocaleString();
                GetResourceAllocationTrendChart();
                GetKeyIndicators(); GetStaffingData();
                GetResourceUtilizationData("", "division", "", request.StartDate, request.EndDate);
            },
        });

    });

    function GetResourceUtilizationData(Filter, Base, Type, StartDate, EndDate) {

        var URL = baseUrl + "/api/CoreRMM/GetExecutiveChartData?Filter=" + Filter + "&Base=" +
            Base + "&Type=" + Type + "&StartDate=" + StartDate + "&EndDate=" + EndDate;

        $.get(URL, function (data) {
            var index = 0;
            $('#resourceUtilizationChart').dxChart({
                rotated: true,
                height: 250,
                dataSource: data,
                series: {
                    label: {
                        position: 'inside',
                        visible: true,
                        font: {
                            size: 16,
                            color: '#000000',
                        },
                        customizeText(point) {
                            return `${point.argumentText} ${point.value}%`;
                        },
                        backgroundColor: 'none',
                    },
                    argumentField: 'Name',
                    valueField: 'Capacity',
                    type: 'bar',
                    cornerRadius: 15,
                    name: "Series",
                    title: ""
                },
                commonSeriesSettings: {
                    argumentField: 'Name',
                    barPadding: 0.1
                },
                commonAxisSettings: {
                    grid: {
                        visible: false,
                    }
                },
                legend: {
                    visible: false
                }, 
                argumentAxis: {
                    label: {
                        visible: false
                    },
                    tick: {
                        visible:false,
                    },
                    title: {
                        text:'',
                    },
                    minorTick: {
                        visible: false
                    },
                    visible:false
                },
                valueAxis: {
                    visible: false,
                    min:100,
                    minVisualRangeLength: 100,
                    label: {
                        font: {
                            color: '#000000'
                        }
                    },
                    constantLines: [{
                        value: 80,
                        color: '#000000',
                        dashStyle: 'dash',
                        width: 2,
                        label: {
                            text: '80', position: 'outside', font: { size: '10' } },
                    }],
                },
                title: {
                    text: 'Resource Utilization',
                    font: {
                        color: '#000',
                        size: 16,
                        weight: 500,
                        family: 'Roboto, sans-serif !important',
                    }
                },
                customizePoint: function (_e) {
                    if (colorCodes.length <= index) {
                        index = 0;
                    }

                    return { color: colorCodes[index++] };
                },
                onLegendClick: function (e) {
                    var series = e.target;
                }
            });

        });
    }

    function GetStaffingData() {
        var URL = `/api/RMOne/GetStaffingChartData?Division=${request.Division}&Role=${request.Role}&Filter=${request.Filter}&StartDate=${request.StartDate}&EndDate=${request.EndDate}`;

        $.get(URL, function (data) {
            data.forEach(function (item, index) {
                return item.FullLength = 100;
            });
            var index = 0;
            $('#StaffingChart').dxChart({
                rotated: true,
                title: {
                    text: 'Staffing',
                    font: {
                        color: '#000',
                        size: 16,
                        weight: 500,
                        family: 'Roboto, sans-serif !important',
                    }
                },
                dataSource: data,
                series: {
                    label: {
                        position: 'inside',
                        visible: true,
                        font: {
                            size: 16,
                            color: '#000000',
                            family: 'Roboto, sans-serif !important',
                        },
                        customizeText(point) {
                            return `${point.argumentText}: ${point.point.data.capacity}`;
                        },
                        backgroundColor: 'none',
                    },
                    argumentField: 'Name',
                    valueField: 'FullLength',
                    type: 'bar',
                    cornerRadius: 2,
                    name: "Series",
                    color: 'white',
                    border: {
                        color: 'black',
                        width: '1',
                        visible: true
                    },
                },
                commonSeriesSettings: {
                    argumentField: 'Name',
                    grid: {
                        visible: false
                    },
                    hoverStyle: {
                        border: {
                            color: 'black',
                            width: '1',
                            visible: true
                        },
                    } 
                },
                legend: {
                    visible: false
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
                onLegendClick: function (e) {
                    var series = e.target;
                }
            });

        });
    }

    $(document).ready(function () {

        $.get("/api/CoreRMM/GetDivisions", function (data, status) {
            Divisions = data;
            const division = $('#divDivision').dxSelectBox({
                dataSource: Divisions,
                placeholder: 'Division',
                showClearButton: true,
                valueExpr: "ID",
                displayExpr: "Title",
                onValueChanged: function (e) {
                    request.Division = e.value == null ? '0' : e.value;
                    GetKeyIndicators();
                    GetStaffingData();
                }
            });
        });

        $.get("/api/rmmapi/GetGroupsOrResource", function (data, status) {
            Roles = data;
            const role = $('#divRole').dxSelectBox({
                dataSource: Roles,
                valueExpr: "Id",
                displayExpr: "Name",
                placeholder: 'Role',
                showClearButton: true,
                onValueChanged: function (e) {
                    request.Division = e.value == null ? '0' : e.value;
                    //GetKeyIndicators();
                    GetStaffingData();
                }
            });
        });

        GetResourceAllocationTrendChart();
       
        const timeframe = $('#divTimeframe').dxSelectBox({
            dataSource: Timeframes,
            valueExpr: "key",
            displayExpr: "value",
            showClearButton: true,
            onValueChanged: function (e) {
                request.Filter = e.value == null ? 'HalfYearly' : e.value;
                GetKeyIndicators();
                GetStaffingData();
                GetResourceAllocationTrendChart();
                if (request.Filter == 'HalfYearly') {
                    startDate = addMonths(new Date(), -6);
                } else if (request.Filter == 'Yearly') {
                    startDate = addMonths(new Date(), -12);
                    
                } else if (request.Filter == 'TwoYear') {
                    startDate = addMonths(new Date(), -24);
                }
                endDate = addMonths(new Date(), 0);
                GetResourceUtilizationData("", "division", "", request.StartDate, request.EndDate);
            }
        });

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
                            { valueField: 'A1', name: 'Opportunities', color: '#97B3CA' },
                            { valueField: 'A2', name: 'Project', color: '#7AAA6C' },
                            { valueField: 'A3', name: 'Actuals', color: '#65CFBD' },
                            {
                                type: 'spline',
                                valueField: 'Trendline',
                                ignoreEmptyPoints: true,
                                label: {
                                    visible: true,
                                    verticalOffset: -50,
                                    horizontalOffset: 130,
                                    customizeText: function () {
                                        if (this.point.index == 0)
                                            return `<b>${dataSource.percentageChange}</b>%`;
                                        else
                                            return '';
                                    },
                                },
                                //axis: 'week',
                                name: 'Trendline',
                                color: '#737373',
                            }
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
                                text: 'Actual Hours',
                            },
                            minorTick: {
                                visible: false
                            },
                            visible: true,
                            axisDivisionFactor: 40
                        }
                    });
                } else if (itemText == 'Switch To Per(%)') {


                    trendChart.option({
                        series: [
                            { valueField: 'AvgPctA1', name: 'Opportunities', color: '#97B3CA' },
                            { valueField: 'AvgPctA2', name: 'Project', color: '#7AAA6C' },
                            { valueField: 'AvgPctA3', name: 'Actuals', color: '#65CFBD' },
                            {
                                type: 'spline',
                                valueField: 'PercentageTrendline',
                                ignoreEmptyPoints: true,
                                label: {
                                    visible: true,
                                    verticalOffset: -50,
                                    horizontalOffset: 130,
                                    customizeText: function () {
                                        if (this.point.index == 0)
                                            return `<b>${dataSource.percentageChange1}</b>%`;
                                        else
                                            return '';
                                    },
                                },
                                //axis: 'week',
                                name: 'PercentageTrendline',
                                color: '#737373',
                            }
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

        var currDate = new Date();
        GetResourceUtilizationData("", "division", "", request.StartDate, request.EndDate);
        GetStaffingData();
        GetKeyIndicators();
    });

    function GetResourceAllocationTrendChart() {
        $.get(`/api/RMOne/GetResourceAllocationTrendChart?Division=${request.Division}&Role=${request.Role}&Filter=${request.Filter}&StartDate=${request.StartDate}&EndDate=${request.EndDate}`, function (data, status) {
            
            dataSource = GetCombinedAllocationSlope(data);
            const chart = $('#chart').dxChart({
                dataSource: dataSource.data,
                height: 480,
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
                series: [
                    { valueField: 'A1', name: 'Opportunities', color: '#97B3CA' },
                    { valueField: 'A2', name: 'Project', color: '#7AAA6C' }, 
                    { valueField: 'A3', name: 'Actuals', color: '#65CFBD' }, 
                    {
                        type: 'spline',
                        valueField: 'Trendline',
                        ignoreEmptyPoints: true,
                        label: {
                            visible: true,
                            verticalOffset: -50,
                            horizontalOffset:130,
                            customizeText: function () {
                                if (this.point.index == 0)
                                    return `<b>${dataSource.percentageChange}</b>%`;
                                else
                                    return '';
                            },
                        },
                        //axis: 'week',
                        name: 'Trendline',
                        color: '#737373',
                    }
                ],
                title: {
                    text: 'Work Allocation Trend',
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
                        value: today,
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
                        text: 'Actual Hours',
                    },
                    minorTick: {
                        visible: false
                    },
                    visible: true,
                    axisDivisionFactor: 40
                },
                export: {
                    enabled: false,
                },
                legend: {
                    verticalAlignment: 'bottom',
                    horizontalAlignment: 'center',
                },
            }).dxChart('instance');
        });
    }
    function GetKeyIndicators() {
        $.get(`/api/RMOne/GetKeyIndicators?Division=${request.Division}&Role=${request.Role}&Filter=${request.Filter}&StartDate=${request.StartDate}&EndDate=${request.EndDate}`, function (data, error) {
            
            if (typeof data != "undefined") {
                $("#acqRatioDiv").text(data.AcqRatio == '' ? '0' : data.AcqRatio);
                $("#billedHrsDiv").text(data.BilledHours == '' ? '0' : Math.round(parseFloat(data.BilledHours.replaceAll(',', ''))).toLocaleString());
                $("#acqHrsDiv").text(data.AcqHours == '' ? '0' : Math.round(parseFloat(data.AcqHours.replaceAll(',', ''))).toLocaleString());
                $("#opmDiv").text(data.OpmLastMonth);
                $("#opmDivText").text(data.OpmLastMonth - data.OpmAverage);
                if (parseInt(data.OpmLastMonth) == parseInt(data.OpmAverage)) {
                    $("#opmDiv").prev().removeClass("glyphicon-triangle-top").removeClass("glyphicon-triangle-bottom");
                    $("#opmDiv").parent().removeClass("color-r").removeClass("color-g").addClass("color-blue");
                    $("#opmDiv").prev().removeClass("color-r").removeClass("color-g").addClass("color-blue");
                }
                else if (parseInt(data.OpmLastMonth) < parseInt(data.OpmAverage)) {
                    $("#opmDiv").prev().removeClass("glyphicon-triangle-top").addClass("glyphicon-triangle-bottom");
                    $("#opmDiv").parent().addClass("color-r").removeClass("color-g").removeClass("color-blue");
                    $("#opmDiv").prev().addClass("color-r").removeClass("color-g").removeClass("color-blue");
                }
                else {
                    $("#opmDiv").prev().removeClass("glyphicon-triangle-bottom").addClass("glyphicon-triangle-top");
                    $("#opmDiv").parent().removeClass("color-r").addClass("color-g").removeClass("color-blue");
                    $("#opmDiv").prev().removeClass("color-r").addClass("color-g").removeClass("color-blue");

                }
                $("#cprDiv").text(data.CPRLastMonth);
                $("#cprDivText").text(data.CPRLastMonth - data.CPRAverage);
                if (parseInt(data.CPRLastMonth) == parseInt(data.CPRAverage)) {
                    $("#cprDiv").prev().removeClass("glyphicon-triangle-top").removeClass("glyphicon-triangle-bottom");
                    $("#cprDiv").parent().removeClass("color-r").removeClass("color-g").addClass("color-blue");
                    $("#cprDiv").prev().removeClass("color-r").removeClass("color-g").addClass("color-blue");
                }
                else if (parseInt(data.CPRLastMonth) < parseInt(data.CPRAverage)) {
                    $("#cprDiv").prev().removeClass("glyphicon-triangle-top").addClass("glyphicon-triangle-bottom");
                    $("#cprDiv").parent().addClass("color-r").removeClass("color-g").removeClass("color-blue");
                    $("#cprDiv").prev().addClass("color-r").removeClass("color-g").removeClass("color-blue");
                }
                else {
                    $("#cprDiv").prev().removeClass("glyphicon-triangle-bottom").addClass("glyphicon-triangle-top");
                    $("#cprDiv").parent().removeClass("color-r").addClass("color-g").removeClass("color-blue");
                    $("#cprDiv").prev().removeClass("color-r").addClass("color-g").removeClass("color-blue");

                }
                $("#lastDaysDiv").text(Math.ceil(data.cprwon) + "%");
                $("#nextDaysDiv").text(data.NextDay);
            }
        });
    }

    function GetAllocationSlope(data) {
        
        //old code to get slope not in use now
        const N = data.length;
        let midIndex = Math.floor(data.length / 2);
        let midValue = data.length % 2 !== 0 ? data[midIndex].A1 : (data[midIndex - 1].A1 + data[midIndex].A1) / 2;
        let FTEavgTE1 = data.slice(0, midIndex).reduce((total, next) => total + next.A1, 2) / data.length;
        //let FTEavgTE2 = data.slice(0, midIndex).reduce((total, next) => total + next.A2, 2) / data.length;

        let TE1Sum = 0; let TE2Sum = 0;
        
        data.forEach(function (item, index) {
            let x = index + 1;
            
            if (new Date(item.week) <= new Date()) {
                let TE1 = (x - (midIndex + 1) / 2) * (item.A1 - FTEavgTE1);
                TE1Sum = TE1Sum + TE1;
                let TE2 = (x - (midIndex + 1) / 2) * (x - (midIndex + 1) / 2);
                TE2Sum = TE2Sum + TE2;
            } 
            item.Trendline = null;
        });
        let slope = TE1Sum / TE2Sum;
        data[midIndex].Trendline = midValue;
        data[data.length - 1].Trendline = midValue + slope * midIndex;

        // Calculate the change in percentage of the trendline
        let initialTrendline = data[midIndex].Trendline;
        let finalTrendline = data[data.length - 1].Trendline;
        let percentageChange = ((finalTrendline - initialTrendline) / initialTrendline) * 100;

       
        return {
            data: data,
            percentageChange: percentageChange.toFixed(2)
        };
    }

    function GetCombinedAllocationSlope(data) {
        //new combined code to calculate two slopes in same
        
        const N = data.length;
        let midIndex = Math.floor(data.length / 2);
        let midValue = data.length % 2 !== 0 ? data[midIndex].A1 : (data[midIndex - 1].A1 + data[midIndex].A1) / 2;
        let midPercentageValue = data.length % 2 !== 0 ? data[midIndex].AvgPctA1 : (data[midIndex - 1].AvgPctA1 + data[midIndex].AvgPctA1) / 2;

        let FTEavgTE1 = data.slice(0, midIndex).reduce((total, next) => total + next.A1, 2) / midIndex;
        let PercentageFTEavgTE1 = data.slice(0, midIndex).reduce((total, next) => total + next.AvgPctA1, 2) / midIndex;

        let TE1Sum = 0;
        let TE2Sum = 0;
        let PercentageTE1Sum = 0;
        let PercentageTE2Sum = 0;

        data.forEach(function (item, index) {
            let x = index + 1;
            
            if (new Date(item.week) <= new Date()) {
                let TE1 = (x - (midIndex + 1) / 2) * (item.A1 - FTEavgTE1);
                let PercentageTE1 = (x - (midIndex + 1) / 2) * (item.AvgPctA1 - PercentageFTEavgTE1);
                TE1Sum += TE1;
                PercentageTE1Sum += PercentageTE1;
                let TE2 = (x - (midIndex + 1) / 2) * (x - (midIndex + 1) / 2);
                let PercentageTE2 = (x - (midIndex + 1) / 2) * (x - (midIndex + 1) / 2);
                TE2Sum += TE2;
                PercentageTE2Sum += PercentageTE2;
            }

            item.Trendline = null;
            item.PercentageTrendline = null;
        });

        let slope = TE1Sum / TE2Sum;
        let percentageSlope = PercentageTE1Sum / PercentageTE2Sum;

        data[midIndex].Trendline = midValue;
        data[midIndex].PercentageTrendline = midPercentageValue;

        data[data.length - 1].Trendline = midValue + slope * midIndex;
        data[data.length - 1].PercentageTrendline = midPercentageValue + percentageSlope * midIndex;

        let initialTrendline = data[midIndex].Trendline;
        let finalTrendline = data[data.length - 1].Trendline;
        let percentageChange = ((finalTrendline - initialTrendline) / initialTrendline) * 100;

        let initialPercentageTrendline = data[midIndex].PercentageTrendline;
        let finalPercentageTrendline = data[data.length - 1].PercentageTrendline;
        let percentageChange1 = ((finalPercentageTrendline - initialPercentageTrendline) / initialPercentageTrendline) * 100;

        return {
            data: data,
            percentageChange: percentageChange.toFixed(2),
            percentageChange1: percentageChange1.toFixed(2)
        };
    }

</script>
<div class="bg-g py-4 executivePane">
    <div class="row">
        <div class="discription-title">Executive Analytics</div>
    </div>
    <div class="row discription-value">
        <div class="col-sm-8 col-md-8">
            <div class="pb-4">
                <div class="row bs d-flex flex-wrap">
                    <div class="col-md-4">
                        <h5 class="text-center mt-0 title-t">Acquisition Ratio </h5>
                        <div class="subDiv d-flex align-items-center">
                            <div class="flex-fill-equal p-2">
                                <div class="kpitext color-g text-center">
                                    <i class="glyphicon glyphicon-triangle-top color-g" style="font-size: 24px;"></i>
                                    <span id="acqRatioDiv">8.8x</span>
                                </div>
                            </div>
                            <div class="brr"></div>
                            <div class="flex-fill-equal p-2 color-b">
                                <div id="billedHrsDiv" class="kpitext2">1478</div>
                                <div class="color-b kpilabel">Billed Hours</div>
                                <hr class="my-2" />
                                <div id="acqHrsDiv" class="kpitext2">168</div>
                                <div class="kpilabel">Acquisition Hours</div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <h5 class="text-center mt-0 title-t">Total </h5>
                        <div class="subDiv d-flex align-items-center">
                            <div class="flex-fill-equal p-2 text-center">
                                <div class="kpitext color-g">
                                    <i class="glyphicon glyphicon-triangle-top color-g" style="font-size: 24px;"></i>
                                    <span id="opmDiv">0</span>
                                </div>
                                <div class="color-b kpilabel">
                                    Opportunities
                                        <br />
                                    <span id="opmDivText"></span>last 30 days
                                </div>
                            </div>
                            <div class="brr"></div>
                            <div class="flex-fill-equal p-2 text-center">
                                <div class="kpitext color-r">
                                    <i class="glyphicon glyphicon-triangle-bottom color-r" style="font-size: 24px;"></i>
                                    <span id="cprDiv">0</span>
                                </div>
                                <div class="color-b kpilabel">
                                    Projects
                                        <br />
                                    <span id="cprDivText"></span>last 30 days
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <h5 class="text-center mt-0 title-t">Conversion Rate</h5>
                        <div class="subDiv d-flex align-items-center">
                            <div class="flex-fill-equal p-2 text-center">
                                <div class="kpitext color-r">
                                    <i class="glyphicon glyphicon-triangle-bottom color-r" style="font-size: 24px;"></i>
                                    <span id="lastDaysDiv">0%</span>
                                </div>
                                <div class="color-b kpilabel">Last 30 days</div>
                            </div>
                            <div class="brr"></div>
                            <div class="flex-fill-equal p-2 text-center">
                                <div class="color-g kpitext">
                                    <i class="glyphicon glyphicon-triangle-top color-g" style="font-size: 24px;"></i>
                                    <span id="nextDaysDiv">0%</span>
                                </div>
                                <div class="color-b kpilabel">Next 30 days</div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <div>
                <div>
                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-3 pr-1">
                     <div id="txtStartDate" style="margin: 5px;width: 135px;"></div>
                    </div>
                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-3 pl-1" <%--style="margin-left: 10px"--%>>
                     <div id="txtEndDate" style="margin: 5px;width: 135px;"></div>
                    </div>
                </div>
                <div class="subDiv2 p-2  trendChart">
                    <div id="btnYaxis" style="float: right"></div>
                    <div id="chart"></div>
                </div>
            </div>
        </div>
        <div class="col-sm-4 col-md-4">

            <div class="pb-4">
                <div class="row filterDiv py-2 m-top">
                    <div class="col-md-4 px-2">
                        <div id="divDivision"></div>
                    </div>
                    <div class="col-md-4 px-0">
                        <div id="divRole"></div>
                    </div>
                    <div class="col-md-4 px-2">
                        <div id="divTimeframe"></div>
                    </div>
                </div>
            </div>
            <div class="pb-4">
                <div class="subDiv1 p-2">
                    <div class="row staffingBar">
                        <div id="StaffingChart">
                        </div>
                    </div>
                </div>
            </div>
            <div class="pb-4">
                <div class="subDiv1 p-2 utilizationBar">
                    <div id="resourceUtilizationChart"></div>
                </div>
            </div>
        </div>
    </div>
</div>
