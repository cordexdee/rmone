<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommittedSalesBySector.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.CoreUI.CommittedSalesBySector" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #CSectorChart {
        height: 440px;
        width: 100%;
    }

    #gauge {
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

    .sector-tooltip{
        font-weight: bold;
        text-align:center;
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">

    var committedSectors = [];
    var topcommittedSectors = [];
    var lastcommittedSectors = [];
    var baseUrl = ugitConfig.apiBaseUrl;

    $(() => {
        $(".gaugeContainer").height('<%=Height%>');
        $(".gaugeContainer").width('<%=Width%>');


        $.get(baseUrl + "/api/CoreRMM/GetRevenueTooltipData?datatype=Sector", function (data) {
            
            committedSectors = data;
            topcommittedSectors = committedSectors.slice(-5);
            if (committedSectors.length > 5) {
                lastcommittedSectors = committedSectors.slice(0, committedSectors.length - 5);
                if (lastcommittedSectors.length > 0)
                    topcommittedSectors.push({
                        Name: 'Others',
                        ResourceCount: lastcommittedSectors.map(item => item.ResourceCount).reduce((prev, next) => prev + next),
                        Utilization: lastcommittedSectors.map(item => item.Utilization).reduce((prev, next) => prev + next),
                        HotProject: lastcommittedSectors.map(item => item.HotProject).reduce((prev, next) => prev + next),
                        HotRevenue: lastcommittedSectors.map(item => item.HotRevenue).reduce((prev, next) => prev + next),
                        CommittedProjects: lastcommittedSectors.map(item => item.CommittedProjects).reduce((prev, next) => prev + next),
                        Value: lastcommittedSectors.map(item => item.Value).reduce((prev, next) => prev + next),
                        PastRevenue: lastcommittedSectors.map(item => item.PastRevenue).reduce((prev, next) => prev + next),
                        DataType: 'Sector'
                    });
            }

            $('#CSector').dxPieChart({
                size: {
                    height: '<%=Height%>',
                    width: '<%=Width%>'
                },
                palette: 'bright',
                dataSource: topcommittedSectors,
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
                        if (pointInfo.point.data.DataType == "Sector") {
                            
                            let sectoritemC = [];

                            if (pointInfo.point.data.Name == "Others") {
                                sectoritemC = topcommittedSectors.filter(item => item.Name == pointInfo.point.data.Name);
                            }
                            else {
                                sectoritemC = committedSectors.filter(item => item.Name == pointInfo.point.data.Name);
                            }

                              
                            const contentItems = [`<div><div class='sector-tooltip'>${pointInfo.point.data.Name}</div>`,
                                `<div class='capital'><span class='caption'>Resources</span>: ${sectoritemC[0].ResourceCount}</div>`,
                                `<div class='capital'><span class='caption'>Utilization</span>: ${sectoritemC[0].Utilization} %</div>`,
                                `<div class='capital'><span class='caption'>Hot Projects</span>: ${sectoritemC[0].HotProject}</div>`,
                                `<div class='capital'><span class='caption'>Hot Revenue</span>: ${convertToMillion(sectoritemC[0].HotRevenue)}</div>`,
                                `<div class='capital'><span class='caption'>Committed Project</span>: ${sectoritemC[0].CommittedProjects}</div>`,
                                `<div class='capital'><span class='caption'>Committed Revenue</span>: ${convertToMillion(sectoritemC[0].Value)}</div>`,
                                `<div class='capital'><span class='caption'>Past Revenue</span>: ${convertToMillion(sectoritemC[0].PastRevenue)}</div>`,
                                '</div>'];

                            const content = $(contentItems.join(''));
                            content.find('.state').text(pointInfo.argument);
                            content.appendTo(container);
                        } else {
                            var piechart = $('#committedPiechart').dxPieChart("instance");
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

                        piechart.option("dataSource", lastcommittedSectors);
                        $(".CommittedSectorChartTitle").text('Committed By Other Sectors');
                            piechart.option("palette", "bright");
                        $("#btnCSectorSwitch").show();
                        
                    } else {
                        if (point.data.DataType == "Sector") {
                            let selectedSector = point.data.Name;
                            $.get(baseUrl + "/api/CoreRMM/GetCommittedSubData?type=sector&name=" + selectedSector, function (data) {
                                
                                piechart.option("dataSource", data);
                                $(".CommittedSectorChartTitle").text('Sales By ' + selectedSector);
                                piechart.option("palette", "Harmony Light");
                                $("#btnCSectorSwitch").show();

                                $("#btnCSectorSwitch").dxButton({
                                    icon: '/Content/Images/sectorIcon.png',
                                    height: 35,
                                    width: 35,
                                    onClick: function (e) {
                                        var piechart = $('#CSector').dxPieChart("instance");

                                        piechart.option("dataSource", topcommittedSectors);
                                        $(".CommittedSectorChartTitle").text('Committed By Sectors');
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
        return `<a>${products[item.index].Name}</a> `;
    }

    function getTooltipText(name, type) {
        
        
    }

    function reloadDetails() {
        
    }
</script>
<div class="gaugeContainer Container" >
    <div class="row">
        <div class="CommittedSectorChartTitle margin fontsize" style="float:left;width:80%;">Committed By Sector</div>
        <div class="margin" style="float:right">
            <div id="btnCSectorSwitch">
                
            </div>
            
        </div>
    </div>
    <div class="row">
        <div id="CSectorChart">
        <div id="CSector"></div>
      </div>
    </div>
      
    </div>