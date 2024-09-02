<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserChartPanel.ascx.cs" Inherits="uGovernIT.Web.UserChartPanel" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #divTreemap rect {
        stroke: #fff;
        stroke-opacity: 1;
        stroke-width: 5;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function getAllDevision() {
        $.ajax({
            type: "GET",
            url: "/api/CoreRMM/GetDivisions",
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (message) {
                $("#ddlDonutChoice").empty();
                $('#ddlDonutChoice').append($('<option>').text("All Division").attr('value', ""));
                $.each(message, function (index, value) {
                    debugger;
                    $('#ddlDonutChoice').append($('<option>').text(value.Title).attr('value', value.ID));
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr);
            }
        });
    }
    function getAllStudio(division) {
        $.ajax({
            type: "GET",
            url: "/api/CoreRMM/GetStudios?division=" + division,
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (message) {
                $("#ddlTreeMapChoice").empty();
                $('#ddlTreeMapChoice').append($('<option>').text("All Studio").attr('value', ""));
                $.each(message, function (index, value) {
                    debugger;
                    $('#ddlTreeMapChoice').append($('<option>').text(value.Title).attr('value', value.ID));
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr);
            }
        });
    }
    $(document).ready(function () {
        GetDonutChartData('division', '', '');
        GetTreeMapChartData('sector', '', '');
        getAllDevision();
        loadDivisionStudioList('division', '', '');
        loadSectorList('sector', '', '');
        hideChartDetailControl();

        setTimeout(function () {
            $("#divTreemap g text").attr({
                "x": "3",
                "y": "2"
            });

            /*$("#divTreemap g text tspan").attr("x", "3");*/
            $("#divTreemap g text tspan:first-child").attr("y", "1");
        }, 1000);

    });
    $(document).on("change", "#ddlDonutChoice", function () {
        var mode = $("#ddlDonutChoice option:selected").val();

        if (mode == '') {
            mode = 'division';
            $("#ddlTreeMapChoice").empty();
            $('#ddlTreeMapChoice').append($('<option>').text("All Studio").attr('value', ""));
            GetTreeMapChartData('sector', '', '');
            GetDonutChartData(mode, '', '');
            loadDivisionStudioList(mode, '', '');
            loadSectorList('sector', '', '');
        }
        else {
            mode = 'studio';
            var division = $("#ddlDonutChoice option:selected").val();
            getAllStudio(division);
            GetDonutChartData(mode, '', division);
            loadDivisionStudioList(mode, '', division);
            GetTreeMapChartData(mode, '', division);
            loadSectorList(mode, '', division);
        }

    });
    $(document).on("change", "#ddlTreeMapChoice", function () {
        var mode = $("#ddlTreeMapChoice option:selected").val();
        if (mode = '') {
            mode = 'studio';
            GetDonutChartData(mode, '', '');
            loadSectorList('sector', '', '');
            loadDivisionStudioList(mode, '', '');
        }
        else {
            var studio = $("#ddlTreeMapChoice option:selected").val();
            var division = $("#ddlDonutChoice option:selected").val();
            GetDonutChartData('studio', studio, division);
            GetTreeMapChartData('sector', studio, division);
            loadSectorList('sector', studio, division);
            loadDivisionStudioList('studio', studio, division);
        }

    });

    function GetDonutChartData(mode, studio, division) {
        $.ajax({
            type: "GET",
            url: "/api/CoreRMM/GetUserChartData?mode=" + mode + "&studio=" + studio + "&division=" + division,
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (message) {
                //console.log(division);
                var chartData = [];
                for (var i = 0; i < message.length; i++) {
                    var obj = new Object();
                    obj.region = message[i].Name;
                    obj.val = message[i].Value;
                    obj.color = '';
                    obj.country = 'BCCI';
                    chartData.push(obj);
                }
                $('#divpiechart').dxPieChart({
                    type: 'doughnut',
                    palette: "Soft Pastel",
                    dataSource: chartData,
                    onPointClick: function (info) {
                        
                    },
                    tooltip: {
                        enabled: true,
                        format: 'millions',
                        customizeTooltip: function (arg) {
                            var argData = convertToMillion(arg.value);
                            return {
                                text: arg.argumentText + " (" + (argData) + ")"
                            };
                        }
                    },
                    legend: {
                        visible: false,
                        orientation: "horizontal" // or "horizontal"
                    },
                    "export": {
                        enabled: false
                    },
                    size: {
                        height: 400,
                        width: 350
                    },
                    adaptiveLayout: {
                        keepLabels: true,
                        height: 400,
                        width: 400
                    },
                    series: [{
                        argumentField: "region",
                        alignment: 'center',//'center' | 'left' | 'right'
                        label: {
                            visible: true,
                            backgroundColor: 'none',
                            position: "inside", // or "columns"| "inside" | "outside"
                            textOverflow: 'ellipsis',//'ellipsis' | 'hide' | 'none'
                            wordWrap: 'normal',// 'normal' | 'breakWord' | 'none'
                            useNodeColors: true,
                            customizeText: function (pointInfo) {
                                var argData = convertToMillion(pointInfo.value);
                                return pointInfo.argumentText + "\n" + argData;
                            },
                            format: {
                                type: "currency",
                                precision: 1
                            },
                            font: {
                                size: 11
                            },
                            connector: {
                                visible: true
                            }
                        },
                        smallValuesGrouping: {
                            mode: "smallValueThreshold",
                            threshold: division != ""? 0 : 50000000
                        }
                    }]
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr);
            }
        });
    }
    function GetTreeMapChartData(mode, studio, division) {
        
        $.ajax({
            type: "GET",
            url: "/api/CoreRMM/GetSectorChartData?mode=" + mode + "&studio=" + studio + "&division=" + division,
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (message) {

                var chartData = [];
                for (var i = 0; i < message.length; i++) {
                    var obj = new Object();
                    obj.region = message[i].Name;
                    obj.val = message[i].Value;
                    obj.color = '';
                    obj.country = 'BCCI';
                    chartData.push(obj);
                }
                $('#divTreemap').dxTreeMap({
                    dataSource: chartData,
                    labelField: 'region',
                    valueField: 'val',

                    tile: {
                        border: {
                            color: '#E8E8E8',
                            //  color: '#0000FF',
                            width: 10
                        },
                        label: {
                            location: "left",
                            alignment: 'center',
                            visible: true,
                        }
                    },
                    tooltip: {
                        enabled: true,
                        //format: "millions",
                        customizeTooltip: function (arg) {
                            var data = arg.node.data,
                                result = null;
                            // console.log(arg.valueText);
                            var argData = convertToMillion(arg.valueText);
                            // console.log(argData);
                            if (arg.node.isLeaf()) {
                                result = "<center><span class='city'>" + data.region + "</span> (" +
                                    argData + ")</center><br/>";
                            }

                            return {
                                text: result
                            };
                        }
                    }
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr);
            }
        });
    }
    function refreshChart() {
        var chartElements = document.getElementsByClassName("ChartDetailsChildElement");
        $.each(chartElements, function (index, value) {
            var chartInstance = $("#" + value.id).dxChart("instance");
            chartInstance.refresh();
        });
    }
    function showChartDetailControl() {
        $(".ChartDetails").css("display", "block");
        $("#divpiechart").css("display", "none");
        $("#divTreemap").css("display", "none");
        $("#divDivisionStudioListHolder").css("display", "none");
        $("#ddlDonutChoice").css("display", "none");
        $("#ddlTreeMapChoice").css("display", "none");
        $('#divTreeMapChoice').addClass('justify-content-end').removeClass('justify-content-between');

        refreshChart();
    }
    function hideChartDetailControl() {
        $(".ChartDetails").css("display", "none");
        $("#divpiechart").css("display", "block");
        $("#divTreemap").css("display", "block");
        $("#divDivisionStudioListHolder").css("display", "block");
        $("#ddlDonutChoice").css("display", "block");
        $("#ddlTreeMapChoice").css("display", "block");
        $('#divTreeMapChoice').addClass('justify-content-between').removeClass('justify-content-end');
    }
    $(document).on("change", "#radioChartDetails", function () {
       
        var isChecked = $(this).prop('checked');
        if (isChecked) {
            showChartDetailControl();
        }

    });
    $(document).on("change", "#radioChart", function () {
        //debugger;
        var isChecked = $(this).prop('checked');
        if (isChecked) {
            hideChartDetailControl();
        }

    });
    function loadDivisionStudioList(mode, studio, division) {
        $("#divDivisionStudioList").dxDataGrid({
            dataSource: "/api/CoreRMM/GetUserChartData?mode=" + mode + "&studio=" + studio + "&division=" + division,
            columns: [
                {
                    dataField: "Name",
                    caption:"Division"
                },
                {
                    dataField: "Value",
                    caption: "Revenue",
                    dataType: "number",
                    format: "currency",
                    alignment: "right",
                }],
            summary: {
                totalItems: [{
                    column: "Value",
                    summaryType: "sum",
                    displayFormat: "Total: {0}",
                    valueFormat: "currency"
                }]
            }
        });
    }
    function loadSectorList(mode, studio, division) {
        $("#divSectorList").dxDataGrid({
            dataSource: "/api/CoreRMM/GetSectorChartData?mode=" + mode + "&studio=" + studio + "&division=" + division,
            columns: [
                {
                    dataField: "Name",
                    caption:"Sector"
                },
                {
                    dataField: "Value",
                    caption: "Revenue",
                    dataType: "number",
                    format: "currency",
                    alignment: "right",
                }],
            summary: {
                totalItems: [{
                    column: "Value",
                    summaryType: "sum",
                    displayFormat: "Total: {0}",
                    valueFormat: "currency"
                }]
            }
        });
    }



</script>
<div class="row">
    <div class="col-md-6 py-3" id="divDDLDonutChoice">
        <select id="ddlDonutChoice" class="selectDropd w-50 ml-auto mb-3">
            <option value="studio">All Studio</option>
            <option value="division">All Division</option>
            <option value="sector">All Sector</option>
        </select>
        <div id="divpiechart"></div>
    </div>
    <div class="col-md-6 py-3">
        <div id="divTreeMapChoice" class="d-flex justify-content-between align-items-center mb-3">
            <select id="ddlTreeMapChoice" class="selectDropd w-50">
                <option value="">All Studio</option>
                <%-- <option value="division">All Division</option>
                <option value="sector">All Sector</option>--%>
            </select>
            <div id="divChartType">
                <label for="db1">Dashboard</label><input id="radioChart" type="radio" name="display" value="Dashboard" checked="" />
                <label for="db2">Details</label><input id="radioChartDetails" type="radio" name="display" value="Details" />
            </div>
        </div>
        <div id="divTreemap"></div>
    </div>
</div>
<div id="divDivisionStudioListHolder">
    <div class="col-md-6 py-3">
        <div id="divDivisionStudioList">

        </div>
    </div>
    <div class="col-md-6 py-3">
        <div id="divSectorList">

        </div>
    </div>
</div>

