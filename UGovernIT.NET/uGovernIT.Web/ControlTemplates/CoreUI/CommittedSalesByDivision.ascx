<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommittedSalesByDivision.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.CoreUI.CommittedSalesByDivision" %>
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

    var CDivisionproducts = [];
    var topCDivisionproducts = [];
    var lastCDivisionproducts = [];
    var baseUrl = ugitConfig.apiBaseUrl;

    $(() => {
        $(".gaugeContainer").height('<%=Height%>');
        $(".gaugeContainer").width('<%=Width%>');


        $.get(baseUrl + "/api/CoreRMM/GetRevenueTooltipData?datatype=Division", function (Divisiondata) {
           
            CDivisionproducts = Divisiondata;
            topCDivisionproducts = CDivisionproducts.slice(-5);
            if (CDivisionproducts.length > 5) {
                lastCDivisionproducts = CDivisionproducts.slice(0, CDivisionproducts.length - 5);
                if (lastCDivisionproducts.length > 0)
                    topCDivisionproducts.push({
                        Name: 'Others',
                        ResourceCount: topCDivisionproducts.map(item => item.ResourceCount).reduce((prev, next) => prev + next),
                        HotProject: topCDivisionproducts.map(item => item.HotProject).reduce((prev, next) => prev + next),
                        HotRevenue: topCDivisionproducts.map(item => item.HotRevenue).reduce((prev, next) => prev + next),
                        CommittedProjects: topCDivisionproducts.map(item => item.CommittedProjects).reduce((prev, next) => prev + next),
                        Value: topCDivisionproducts.map(item => item.Value).reduce((prev, next) => prev + next),
                        PastRevenue: topCDivisionproducts.map(item => item.PastRevenue).reduce((prev, next) => prev + next),
                        Utilization: topCDivisionproducts.map(item => item.Utilization).reduce((prev, next) => prev + next),
                        DataType: 'Division'
                    });
            }
            
            $('#committedPiechart').dxPieChart({
                size: {
                    height: '<%=Height%>',
                    width: '<%=Width%>'
                },
                palette: 'bright',
                dataSource: topCDivisionproducts,
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
                            
                            let CdivisionItem = [];

                            if (pointInfo.point.data.Name == "Others") {
                                CdivisionItem = topCDivisionproducts.filter(item => item.Name == pointInfo.point.data.Name);
                            }
                            else {
                                CdivisionItem = CDivisionproducts.filter(item => item.Name == pointInfo.point.data.Name);
                            }

                            const contentItems = [`<div><div class='division-tooltip'>${pointInfo.point.data.Name}</div>`,
                                `<div class='capital'><span class='caption'>Resources</span>: ${CdivisionItem[0].ResourceCount}</div>`,
                                `<div class='capital'><span class='caption'>Utilization</span>: ${CdivisionItem[0].Utilization} %</div>`,
                                `<div class='capital'><span class='caption'>Hot Projects</span>: ${CdivisionItem[0].HotProject}</div>`,
                                `<div class='capital'><span class='caption'>Hot Revenue</span>: ${convertToMillion(CdivisionItem[0].HotRevenue)}</div>`,
                                `<div class='capital'><span class='caption'>Committed Projects</span>: ${CdivisionItem[0].CommittedProjects}</div>`,
                                `<div class='capital'><span class='caption'>Committed Revenue</span>: ${convertToMillion(CdivisionItem[0].Value)}</div>`,
                                `<div class='capital'><span class='caption'>Past Revenue</span>: ${convertToMillion(CdivisionItem[0].PastRevenue)}</div>`,
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
                        console.log(lastCDivisionproducts);
                        console.log(piechart);
                        piechart.option("dataSource", lastCDivisionproducts);
                        $(".committedByDivisiontitle").text('Committed By Other Divisions');
                            piechart.option("palette", "bright");
                        $("#btnCSwitchtoMain").show();
                        
                    } else {
                        if (point.data.DataType == "Division") {
                            let selectedCDivision = point.data.Name;
                            $.get(baseUrl + "/api/CoreRMM/GetCommittedSubData?type=division&name=" + selectedCDivision, function (data) {

                                piechart.option("dataSource", data);
                                $(".committedByDivisiontitle").text('Sales By ' + selectedCDivision);
                                piechart.option("palette", "Harmony Light");
                                $("#btnCSwitchtoMain").show();

                                $("#btnCSwitchtoMain").dxButton({
                                    icon: '/Content/Images/sectorIcon.png',
                                    height: 35,
                                    width: 35,
                                    onClick: function (e) {
                                        var piechart = $('#committedPiechart').dxPieChart("instance");
                                        
                                        piechart.option("dataSource", topCDivisionproducts);
                                        $(".committedByDivisiontitle").text('Committed By Division');
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
        return `<a>${CDivisionproducts[item.index].Name}</a> `;
    }

    function getTooltipText(item, text) {
        return `${CDivisionproducts[item.index].Name}: ${convertToMillion(text)}<br/><br/><div onclick="reloadDetails()">click here</div>`;
    }

    function reloadDetails() {
        
    }
</script>
<div class="gaugeContainer Container" >
    <div class="row">
        <div class="committedByDivisiontitle margin fontsize" style="float:left;width:80%;">Committed By Division</div>
        <div class="margin" style="float:right;">
            <div id="btnCSwitchtoMain">
                
            </div>
            
        </div>
    </div>
    <div class="row">
        <div id="divisionChart">
        <div id="committedPiechart"></div>
      </div>
    </div>
      
    </div>