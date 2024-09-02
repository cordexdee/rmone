<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SalesByDivisionChart.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.CoreUI.SalesByDivisionChart" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #divisionChart {
        /*height: 440px;*/
        width: 100%;
    }

    #piechart {
        width: 100%;
        height: 100%;
        margin-top: 20px;
        float: left;
    }

    #panel {
        width: 150px;
        text-align: left;
        margin-top: 20px;
        float: left;
    }

    .margin{
        margin:10px;
    }

    .fontsize{
        font-size:18px;
    }

    .gaugeContainer{
        
    }

    .division-tooltip{
        font-weight: bold;
        text-align:center;
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">

    var Divisionproducts = [];
    var topDivisionproducts = [];
    var lastDivisionproducts = [];
    var baseUrl = ugitConfig.apiBaseUrl;

    $(() => {
        $(".gaugeContainer").height('<%=Height%>');
        $(".gaugeContainer").width('<%=Width%>');


        $.get(baseUrl + "/api/CoreRMM/GetTooltipData?datatype=Division", function (Divisiondata) {

            Divisionproducts = Divisiondata;
            topDivisionproducts = Divisionproducts.slice(-5);
            if (Divisionproducts.length > 5) {
                lastDivisionproducts = Divisionproducts.slice(0, Divisionproducts.length - 5);
                if (lastDivisionproducts.length > 0)
                    topDivisionproducts.push({
                        Name: 'Others',
                        ResourceCount: lastDivisionproducts.map(item => item.ResourceCount).reduce((prev, next) => prev + next),
                        HotProject: lastDivisionproducts.map(item => item.HotProject).reduce((prev, next) => prev + next),
                        HotRevenue: lastDivisionproducts.map(item => item.HotRevenue).reduce((prev, next) => prev + next),
                        CommittedProjects: lastDivisionproducts.map(item => item.CommittedProjects).reduce((prev, next) => prev + next),
                        Value: lastDivisionproducts.map(item => item.Value).reduce((prev, next) => prev + next),
                        PastRevenue: lastDivisionproducts.map(item => item.PastRevenue).reduce((prev, next) => prev + next),
                        Utilization: lastDivisionproducts.map(item => item.Utilization).reduce((prev, next) => prev + next),
                        DataType: 'Sector'
                    });
            }
            
            $('#piechart').dxPieChart({
                size: {
                    height: '<%=Height%>',
                    width: '<%=Width%>'
                },
                palette: 'bright',
                dataSource: topDivisionproducts,
                //commonSeriesSettings: {
                //    label: {
                //        customizeText(point) {
                //            return `${point.argumentText}: ${point.valueText}`;
                //        },
                //    }
                //},
                series: [
                    {
                        argumentField: 'Name',
                        valueField: 'Value',
                        label: {
                            visible: true,
                            connector: {
                                visible: true,
                                width: 1,
                            },
                            customizeText(point) {
                                return `${point.argumentText}<br>${convertToMillion(point.value)}`;
                            },
                        },
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
                        if (pointInfo.point.data.DataType == "Division") {

                            let divisionItem = [];

                            if (pointInfo.point.data.Name == "Others") {
                                divisionItem = topDivisionproducts.filter(item => item.Name == pointInfo.point.data.Name);
                            }
                            else {
                                divisionItem = Divisionproducts.filter(item => item.Name == pointInfo.point.data.Name);
                            }

                            const contentItems = [`<div><div class='division-tooltip'>${pointInfo.point.data.Name}</div>`,
                            `<div class='capital'><span class='caption'>Resources</span>: ${divisionItem[0].ResourceCount}</div>`,
                            `<div class='capital'><span class='caption'>Utilization</span>: ${divisionItem[0].Utilization} %</div>`,
                            `<div class='capital'><span class='caption'>Hot Projects</span>: ${divisionItem[0].HotProject}</div>`,
                            `<div class='capital'><span class='caption'>Hot Revenue</span>: ${convertToMillion(divisionItem[0].HotRevenue)}</div>`,
                            `<div class='capital'><span class='caption'>Committed Projects</span>: ${divisionItem[0].CommittedProjects}</div>`,
                            `<div class='capital'><span class='caption'>Committed Revenue</span>: ${convertToMillion(divisionItem[0].Value)}</div>`,
                            `<div class='capital'><span class='caption'>Past Revenue</span>: ${convertToMillion(divisionItem[0].PastRevenue)}</div>`,
                                '</div>'];

                            const content = $(contentItems.join(''));
                            content.find('.state').text(pointInfo.argument);
                            content.appendTo(container);
                        } else {
                            var piechart = $('#piechart').dxPieChart("instance");
                            let chartData = piechart.option("dataSource");
                            let subsectoritem = chartData.filter(item => item.Name == pointInfo.point.data.Name);

                            const contentItems = [`<div><div class='sector-tooltip'>${pointInfo.point.data.Name}</div>`,
                                `<div class='capital'><span class='caption'>Resources</span>: ${subsectoritem[0].ResourceCount}</div>`,
                                `<div class='capital'><span class='caption'>Utilization</span>: ${subsectoritem[0].Utilization} %</div>`,
                                `<div class='capital'><span class='caption'>Hot Projects</span>: ${subsectoritem[0].HotProject}</div>`,
                                `<div class='capital'><span class='caption'>Hot Revenue</span>: ${convertToMillion(subsectoritem[0].HotRevenue)}</div>`,
                                `<div class='capital'><span class='caption'>Committed Project</span>: ${subsectoritem[0].CommittedProjects}</div>`,
                                `<div class='capital'><span class='caption'>Committed Revenue</span>: ${convertToMillion(subsectoritem[0].Value)}</div>`,
                                `<div class='capital'><span class='caption'>Past Revenue</span>: ${convertToMillion(subsectoritem[0].PastRevenue)}</div>`,
                                '</div>'];

                            const content = $(contentItems.join(''));
                            content.find('.state').text(pointInfo.argument);
                            content.appendTo(container);
                        }
                    },
                },
                onPointClick(e) {
                    
                    const point = e.target;
                    var piechart = e.component;
                    if (point.data.Name == "Others") {
                       
                        piechart.option("dataSource", lastDivisionproducts);
                            $(".title").text('Pipeline By Other Divisions');
                            piechart.option("palette", "bright");
                            $("#btnSwitchtoMain").show();
                        
                    } else {
                        if (point.data.DataType == "Division") {
                            let selectedDivision = point.data.Name;
                            $.get(baseUrl + "/api/CoreRMM/GetChartSubData?type=division&name=" + selectedDivision, function (data) {

                                piechart.option("dataSource", data);
                                $(".title").text('Pipeline By ' + selectedDivision);
                                piechart.option("palette", "Harmony Light");
                                $("#btnSwitchtoMain").show();

                                $("#btnSwitchtoMain").dxButton({
                                    icon: '/Content/Images/sectorIcon.png',
                                    height: 35,
                                    width: 35,
                                    onClick: function (e) {
                                        var piechart = $('#piechart').dxPieChart("instance");
                                        
                                        piechart.option("dataSource", topDivisionproducts);
                                            $(".title").text('Pipeline By Division');
                                            piechart.option("palette", "bright");
                                        
                                    }
                                });
                            });
                        }

                    }
                },
                onLegendClick(e) {
                    const arg = e.target;
                    
                },
            });

            
        });


    });

    function getText(item, text) {
        return `<a>${Divisionproducts[item.index].Name}</a> `;
    }

    function getTooltipText(item, text) {
        return `${Divisionproducts[item.index].Name}: ${convertToMillion(text)}<br/><br/><div onclick="reloadDetails()">click here</div>`;
    }

    function reloadDetails() {
        
    }
</script>
<div class="gaugeContainer Container" >
    <div class="row">
        <div class="title margin fontsize" style="float:left;width:80%;">Pipeline By Division</div>
        <div class="margin" style="float:right;">
            <div id="btnSwitchtoMain">
                
            </div>
            
        </div>
    </div>
    <div class="row">
        <div id="divisionChart">
        <div id="piechart"></div>
      </div>
    </div>
      
    </div>