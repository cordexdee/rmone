<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SalesBySectorChart.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.CoreUI.BarGaugeChart" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #gaugeChart {
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

    var products = [];
    var topProducts = [];
    var lastProucts = [];
    var baseUrl = ugitConfig.apiBaseUrl;

    $(() => {
        $(".gaugeContainer").height('<%=Height%>');
        $(".gaugeContainer").width('<%=Width%>');


        $.get(baseUrl + "/api/CoreRMM/GetTooltipData?datatype=Sector", function (data) {
            
            products = data;
            topProducts = products.slice(-5);
            if (products.length > 5) {
                lastProucts = products.slice(0, products.length - 5);
                if (lastProucts.length > 0)
                    topProducts.push({
                        Name: 'Others',
                        ResourceCount: lastProucts.map(item => item.ResourceCount).reduce((prev, next) => prev + next),
                        Utilization: lastProucts.map(item => item.Utilization).reduce((prev, next) => prev + next),
                        HotProject: lastProucts.map(item => item.HotProject).reduce((prev, next) => prev + next),
                        HotRevenue: lastProucts.map(item => item.HotRevenue).reduce((prev, next) => prev + next),
                        CommittedProjects: lastProucts.map(item => item.CommittedProjects).reduce((prev, next) => prev + next),
                        Value: lastProucts.map(item => item.Value).reduce((prev, next) => prev + next),
                        PastRevenue: lastProucts.map(item => item.PastRevenue).reduce((prev, next) => prev + next),
                        DataType: 'Sector'
                    });
            }

            $('#gauge').dxPieChart({
                size: {
                    height: '<%=Height%>',
                    width: '<%=Width%>'
                },
                palette: 'bright',
                dataSource: topProducts,
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
                    visible:false,
                },
                export: {
                    enabled: false,
                },
                tooltip: {
                    enabled: true,
                    contentTemplate(pointInfo, container) {
                        
                        if (pointInfo.point.data.DataType == "Sector") {

                            let sectoritem = [];

                            if (pointInfo.point.data.Name == "Others") {
                                sectoritem = topProducts.filter(item => item.Name == pointInfo.point.data.Name);
                            }
                            else {
                                sectoritem = products.filter(item => item.Name == pointInfo.point.data.Name);
                            }

                            const contentItems = [`<div><div class='sector-tooltip'>${pointInfo.point.data.Name}</div>`,
                            `<div class='capital'><span class='caption'>Resources</span>: ${sectoritem[0].ResourceCount}</div>`,
                            `<div class='capital'><span class='caption'>Utilization</span>: ${sectoritem[0].Utilization} %</div>`,
                            `<div class='capital'><span class='caption'>Hot Projects</span>: ${sectoritem[0].HotProject}</div>`,
                            `<div class='capital'><span class='caption'>Hot Revenue</span>: ${convertToMillion(sectoritem[0].HotRevenue)}</div>`,
                            `<div class='capital'><span class='caption'>Committed Projects</span>: ${sectoritem[0].CommittedProjects}</div>`,
                            `<div class='capital'><span class='caption'>Committed Revenue</span>: ${convertToMillion(sectoritem[0].Value)}</div>`,
                            `<div class='capital'><span class='caption'>Past Revenue</span>: ${convertToMillion(sectoritem[0].PastRevenue)}</div>`,
                                '</div>'];

                            const content = $(contentItems.join(''));
                            content.find('.state').text(pointInfo.argument);
                            content.appendTo(container);
                        } else {
                            var piechart = $('#gauge').dxPieChart("instance");
                            
                            let chartData = piechart.option("dataSource");
                            
                            let subdivisionitem = chartData.filter(item => item.Name == pointInfo.point.data.Name);
                            
                            const contentItems = [`<div><div class='sector-tooltip'>${pointInfo.point.data.Name}</div>`,
                                `<div class='capital'><span class='caption'>Resources</span>: ${subdivisionitem[0].ResourceCount}</div>`,
                                `<div class='capital'><span class='caption'>Utilization</span>: ${subdivisionitem[0].Utilization} %</div>`,
                                `<div class='capital'><span class='caption'>Hot Projects</span>: ${subdivisionitem[0].HotProject}</div>`,
                                `<div class='capital'><span class='caption'>Hot Revenue</span>: ${convertToMillion(subdivisionitem[0].HotRevenue)}</div>`,
                                `<div class='capital'><span class='caption'>Committed Project</span>: ${subdivisionitem[0].CommittedProjects}</div>`,
                                `<div class='capital'><span class='caption'>Committed Revenue</span>: ${convertToMillion(subdivisionitem[0].Value)}</div>`,
                                `<div class='capital'><span class='caption'>Past Revenue</span>: ${convertToMillion(subdivisionitem[0].PastRevenue)}</div>`,
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
                        
                            piechart.option("dataSource", lastProucts);
                            $(".SectorChartTitle").text('Pipeline By Other Sectors');
                            piechart.option("palette", "bright");
                            $("#btnSectorSwitch").show();
                        
                    } else {
                        if (point.data.DataType == "Sector") {
                            let selectedSector = point.data.Name;
                            $.get(baseUrl + "/api/CoreRMM/GetChartSubData?type=sector&name=" + selectedSector, function (data) {
                                
                                piechart.option("dataSource", data);
                                $(".SectorChartTitle").text('Pipeline By ' + selectedSector);
                                piechart.option("palette", "Harmony Light");
                                $("#btnSectorSwitch").show();

                                $("#btnSectorSwitch").dxButton({
                                    icon: '/Content/Images/sectorIcon.png',
                                    height: 35,
                                    width: 35,
                                    onClick: function (e) {
                                        var piechart = $('#gauge').dxPieChart("instance");

                                        piechart.option("dataSource", topProducts);
                                            $(".SectorChartTitle").text('Pipeline By Sectors');
                                            piechart.option("palette", "bright");

                                    }
                                });
                            });
                        }
                        //else {
                            
                        //    piechart.option("dataSource", topProducts);
                        //        $(".SectorChartTitle").text('Pipeline By Sectors');
                        //        piechart.option("palette", "bright");
                        //}
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
        <div class="SectorChartTitle margin fontsize" style="float:left;width:80%;">Pipeline By Sector</div>
        <div class="margin" style="float:right">
            <div id="btnSectorSwitch">
                
            </div>
            
        </div>
    </div>
    <div class="row">
        <div id="gaugeChart">
        <div id="gauge"></div>
      </div>
    </div>
      
    </div>