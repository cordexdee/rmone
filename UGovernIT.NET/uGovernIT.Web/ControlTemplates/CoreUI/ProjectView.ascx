<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.CoreUI.ProjectView" %>
<%@ Register Src="~/ControlTemplates/CoreUI/SalesBySectorChart.ascx" TagPrefix="ugit" TagName="SalesBySectorChart" %>

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
        border:1px solid lightgrey;
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
        /*margin-right: 10px;*/
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

    .dx-button-has-icon .dx-button-content {
        padding: 2px;
    }

    .dx-button-has-icon .dx-icon {
        width: 26px;
        height: 26px;
        background-position: 0 0;
        background-size: 26px 26px;
        padding: 0;
        font-size: 26px;
        text-align: center;
        line-height: 26px;
        margin-right: 0;
        margin-left: 0;
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
    var dataSource = [];
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
    var projectDataSource = [];
    var baseUrl = ugitConfig.apiBaseUrl;
    var complexityColorCodes = ['#6BE5E8', '#41B8D6', '#2C8BB9', '#2E5F99', '#30356D'];
    complexityColorCodes.reverse();
    var hideCards = false;

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
                loadCardApis(request.Filter, request.Base, request.Type, '', '');
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
                loadCardApis(request.Filter, request.Base, request.Type, '', '');
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
                loadCardApis(request.Filter, request.Base, request.Type, '', '');
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
                loadCardApis(request.Filter, request.Base, request.Type, '', '');
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
                loadCardApis(request.Filter, request.Base, request.Type, '', '');
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
            hint:'Switch to Main',
            onClick: function (e) {
                let filter1 = $("#filterone").dxRadioGroup("instance");
                let filter2 = $("#filtertwo").dxRadioGroup("instance");

                request.Filter = filter1.option('value');
                request.Base = filter2.option('value');
                request.Selection = '';
                request.StartDate = new Date(startDate).toUTCString();
                request.EndDate = new Date(endDate).toUTCString();
                loadCardApis(request.Filter, request.Base, request.Type, '', '');
                LoadPieChart();

            }
        });

        $("#btnProjectComplexity").dxButton({
            icon: '/Content/Images/Analytics/Complexity2.png',
            height: 35,
            width: 35,
            hint:'Show Complexity',
            onClick: function (e) {
                window.parent.UgitOpenPopupDialog("/layouts/uGovernIT/DelegateControl.aspx?control=ResourceProjectComplexity&AllocationViewType=RMMAllocation&isdlg=1&isudlg=1", "", "Project Complexity", 90, 90, 0, '');
            }
        });

        $("#btnShowCards").dxButton({
            icon: '/content/Images/cardViewBlack-new.png',
            height: 35,
            width: 35,
            hint: 'Show Cards',
            onClick: function (e) {

                if (!hideCards) {
                    e.component.option('icon', '/Content/Images/Analytics/piebarChart.png');
                    e.component.option('hint', 'Show Graph');
                    $(".barPanel").hide();
                    $(".cardPanel").show();
                    hideCards = true;
                } else {
                    e.component.option('icon', '/content/Images/cardViewBlack-new.png');
                    e.component.option('hint', 'Show Cards');
                    $(".barPanel").show();
                    $(".cardPanel").hide();

                    hideCards = false;

                }
            }
        });

        LoadPieChart();
        loadCardApis(request.Filter, request.Base, request.Type, '', '');
    });

    function LoadPieChart() {

        $.get(baseUrl + "/api/CoreRMM/GetProjectChartData?Filter=" + request.Filter + "&Base=Complexity&Type=" + request.Type
            + "&StartDate=" + request.StartDate + "&EndDate=" + request.EndDate, function (data) {
            //adding custom color codes for each item
            data = data.reverse();
            data.forEach(function (item, index) {
                item.Color = complexityColorCodes[index];
            });
            products = data.reverse();
            topProducts = products.slice(-5);
            if (products.length > 6) {
                lastProucts = products.slice(0, products.length - 5);
                if (lastProucts.length > 0)
                    topProducts.push({
                        Title: 'Others',
                        Name:'Others',
                        Amount: lastProucts.map(item => item.Amount).reduce((prev, next) => prev + next),
                        DataType: 'Sector'
                    });
            } else {
                topProducts = products;
            }

            let valueField = "Amount";
            if (request.Type == "Resource")
                valueField = "ResourceCount";

            if (products.length > 0)
                dataSource = products.slice(-products.length)
            
            $('#piechart').dxPieChart({
                title: {
                    text: 'Project Complexity',
                    font: {
                        color: '#000',
                        size: 16,
                        weight: 500,
                        family: 'Roboto, sans-serif !important',
                    }
                },
                size: {
                    height: 550,
                    width: 550,
                },
                dataSource: topProducts,
                series: [
                    {
                        argumentField: 'Name',
                        valueField: valueField,
                        label: {
                            visible: true,
                            position: 'inside',
                            //alignment: 'center',
                            connector: {
                                visible: true,
                                width: 1,
                            },
                            font: {
                                size: 16,
                            },
                            customizeText(point) {
                                if (products.length == 1) {
                                    var argText = getFirstNWords(point.argumentText, 4);
                                    if (request.Base == 'Division')
                                        return `&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;${argText}<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<div style='border: 3px solid green;text-align: center;font-size:22px'>(${point.value})</div>`;
                                    else
                                        return `&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;${point.argumentText}<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<div style='border: 3px solid green;text-align: center;font-size:22px'>(${point.value})</div>`;
                                }
                                else {
                                    var argText = getFirstNWords(point.argumentText, 4);
                                    if (request.Base == 'Division')
                                        return `${argText}<br>&nbsp;&nbsp;<div style='border: 3px solid green;text-align: center;font-size:22px'>(${point.value})</div>`;
                                    else
                                        return `${point.argumentText}<br>&nbsp;&nbsp;<div style='border: 3px solid green;text-align: center;font-size:22px'>(${point.value})</div>`;
                                }
                            },
                        },
                        minSegmentSize: 1000000,
                        smallValuesGrouping: {
                            groupName: "others",
                            mode: "none",
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
                        return `${pointInfo.argumentText.replace(/ /g, "\r\n")}<br><div style='width:100%'>(${pointInfo.value})</div>`;
                    },
                },
                resolveLabelOverlapping:'shift',               
                customizePoint: function (pointInfo) {
                    return { color: pointInfo.data.Color, hoverStyle: { color: pointInfo.data.Color } };
                },
            });

            $.get(baseUrl + "/api/CoreRMM/GetProjectChartData?Filter=" + request.Filter + "&Base=" + request.Base + "&Type=" + request.Type
                + "&StartDate=" + request.StartDate + "&EndDate=" + request.EndDate, function (datagraph) {
                //adding same color code from pie chart data
                    datagraph.reverse();
                    datagraph.forEach(function (y, index) {
                            y.Color = colorCodes[index];
                    });
                    datagraph.reverse();
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
                            text: 'Project Count',
                            font: {
                                color: '#000',
                                size: 16,
                                weight: 500,
                                family: 'Roboto, sans-serif !important',
                            }
                        },
                        customizePoint(e) {

                            return { color: e.data.Color, hoverStyle: { color: e.data.Color } };
                        },
                        argumentAxis: {
                            label: {
                                visible: false
                            }
                        },
                        series: {
                            argumentField: 'Title',
                            valueField: 'Amount',
                            type: 'bar',
                            label: {
                                position: 'inside',
                                visible: true,
                                font: {
                                    size: 16,
                                },
                                customizeText(point) {
                                    let color = isColorDark(point.point._options.color);
                                    if (point.percent == 0)
                                        color = '#000000';
                                    let argText = " " + point.argumentText;
                                    return `<div style='color:${color}'><span style="font-size:16px;color:#000;font-family:Roboto, sans-serif !important;">${argText}</span>: ${point.value}</div>`;
                                },
                                backgroundColor: 'none',
                            },
                        },
                        onPointClick(e) {
                            const point = e.target;
                            var piechart = $('#piechart').dxPieChart("instance");

                            if (point.data.DataType == request.Base) {

                                let selectedSector = encodeURIComponent(point.data.Name);
                                var base = 'Division'
                                if (request.Base == 'Division')
                                    base = 'Sector';
                                if (request.Base == 'Studio')
                                    base = 'Division'

                                request.Selection = request.Base + "~" + selectedSector;
                                request.ParentTitle = point.data.Title;
                                //loadCardApis(request.Filter, base, request.Type, selectedSector, point.data.DataType);
                                $.get(baseUrl + "/api/CoreRMM/GetProjectChartData?Filter=" + request.Filter + "&Base=Complexity&Type=" + request.Type
                                    + "&Selection=" + selectedSector + "&SelectionDataType=" + point.data.DataType + "&StartDate=" + request.StartDate + "&EndDate=" + request.EndDate,
                                    function (breakupdata) {

                                        products = breakupdata;
                                        topProducts = products.slice(-5);
                                        if (products.length > 5) {
                                            lastProucts = products.slice(0, products.length - 5);
                                            if (lastProucts.length > 0)
                                                topProducts.push({
                                                    Name: 'Others',
                                                    Title: 'Others',
                                                    Amount: lastProucts.map(item => item.Amount).reduce((prev, next) => prev + next),
                                                    DataType: lastProucts[0].DataType
                                                });
                                        }
                                        topProducts.reverse();
                                        topProducts.forEach(function (y, index) {
                                            y.Color = complexityColorCodes[index];
                                        });
                                        piechart.option("dataSource", topProducts);
                                    });
                            }
                            else {
                                //last drill down
                                request.ParentSelection = request.Selection;
                                request.ChildSelection = point.data.Name;
                                request.DataType = point.data.DataType;

                                var title = request.ParentTitle + "  >  " + point.data.Title;

                                window.parent.UgitOpenPopupDialog("/Layouts/uGovernIT/DelegateControl.aspx?control=ProjectListDrillDown&Filter=" + request.Filter + "&Base=" + request.DataType + "&Type=" + request.Type + "&ParentSelection="
                                    + request.ParentSelection + "&ChildSelection=" + request.ChildSelection + "&StartDate=" + request.StartDate + "&EndDate=" + request.EndDate, "", title, 90, 90, 0, '');
                            }

                        },
                    });

                });
        });

        
    }

    function getFirstNWords(str, n) {
        return str.split(' ').slice(0, n).join(' ')
    }

    function getText(item, text) {
        return `<a>${products[item.index].Name}</a> `;
    }

    function loadCardApis(filter, base, type, selectedPoint, selectionDataType) {
        let selection = encodeURIComponent(selectedPoint);
        var viewtype = '<%=HeadType%>';
        $.get(baseUrl + "/api/CoreRMM/GetCardKpis?Period=Now&Filter=" + filter + "&Base=" + base + "&Type=" + viewtype
            + "&Selection=" + selection + "&SelectionDataType=" + selectionDataType, function (monthlydata) {

                var monthDataSource = Object.entries(monthlydata);
                var divbillignandmargin = $("#divBillingsMonth1").dxTileView({
                    dataSource: monthDataSource,
                    showScrollbar: false,
                    baseItemHeight: 250,
                    baseItemWidth: 280,
                    itemMargin: 32,
                    direction: "vertical",
                    width: 320,
                    height: 300,
                    itemTemplate: function (itemData, itemIndex, itemElement) {

                        var item = itemData[1];
                        itemElement.attr("class", "TileStyle");
                        itemElement.append(`<Div class='MonthTitle'>${date.getFullYear()} ${date.toLocaleString('default', { month: 'short' })}</Div><br>`);
                        for (i = 0; i < item.length; i++) {
                            itemElement.append(`<div class='rowitem'><b>${item[i].HeadName}: </b><div style='float:right;'>${item[i].HeadCount}</div ></div >`);
                        }

                    },
                    onItemClick: function (e) {
                        //
                        var data = e.itemData;
                        window.parent.UgitOpenPopupDialog('/layouts/ugovernit/delegatecontrol.aspx?control=BillingAndMarginsReport', '', 'Resource Management', '90', '90', '', true);
                    }
                });

                
            });

        //load YTD card after monthy card to stop api timeout problem
        $.get(baseUrl + "/api/CoreRMM/GetCardKpis?Period=monthly&Filter=" + filter + "&Base=" + base + "&Type=" + viewtype
            + "&Selection=" + selection + "&SelectionDataType=" + selectionDataType + "&StartDate=" + request.StartDate + "&EndDate=" + request.EndDate, function (ytddata) {

                var tileDataSource = Object.entries(ytddata);
                var divYTDmargin = $("#divBillingYTD1").dxTileView({
                    dataSource: tileDataSource,
                    showScrollbar: false,
                    baseItemHeight: 250,
                    baseItemWidth: 280,
                    itemMargin: 32,
                    direction: "vertical",
                    width: 320,
                    height: 300,
                    itemTemplate: function (itemData, itemIndex, itemElement) {
                        var item = itemData[1];
                        const sDate = new Date(request.StartDate);
                        const eDate = new Date(request.EndDate);
                        itemElement.attr("class", "TileStyle");
                        itemElement.append(`<Div class='MonthTitle'>${sDate.getFullYear()} ${sDate.toLocaleString('default', { month: 'short' })} TO
                                                            ${eDate.getFullYear()} ${eDate.toLocaleString('default', { month: 'short' })}</Div><br>`);
                        for (i = 0; i < item.length; i++) {
                            itemElement.append(`<div class='rowitem'><b>${item[i].HeadName}: </b><div style='float:right;'>${item[i].HeadCount}</div ></div >`);
                        }
                    },
                    onItemClick: function (e) {
                        //
                        var data = e.itemData;
                        window.parent.UgitOpenPopupDialog('/layouts/ugovernit/delegatecontrol.aspx?control=BillingAndMarginsReport', '', 'Resource Management', '90', '90', '', true);
                    }
                });

            });
    }

    var popupDetails = null;
    var popupDetailsOptions = {
        width: 850,
        height: 500,
        contentTemplate: function () {

            return $("<div />").append(
                $("<div id='projectListGrid' />").dxDataGrid({
                    dataSource: baseUrl + "/api/CoreRMM/GetProjectList?Filter=" + request.Filter + "&Base=" + request.DataType + "&Type=" + request.Type + "&ParentSelection="
                        + request.ParentSelection + "&ChildSelection=" + request.ChildSelection,
                    columns: [{
                        dataField: "TicketId",
                        caption:"ProjectId"
                    },
                        {
                            dataField: "Title",
                            caption:"Project"
                        }
                    ],
                })
            );
        },
        showTitle: false,
        title: "Project List",
        dragEnabled: false,
        hideOnOutsideClick: true
    };

    var showProjectList = function () {

        if (popupDetails) {
            popupDetails.option("contentTemplate", popupDetailsOptions.contentTemplate.bind(this));
        } else {
            popupDetails = $("#popup").dxPopup(popupDetailsOptions).dxPopup("instance");
        }

        popupDetails.show();
    };
</script>

<div class="row" style="margin-top:-4px;">
    <div class="discription-title">Project Analytics</div>
    <div class="resourceViewContainer" style="overflow-y: auto">
        <div class="title margin fontsize">
            <div class="row secondRow">
                <div class="col-lg-4 col-md-3 col-sm-3 col-xs-3"></div>
                <div class="col-lg-2 col-md-3 col-sm-3 col-xs-3 pr-1" <%--style="margin-right: 10px"--%>>
                    <div id="txtStartDate" style="margin: 5px;"></div>
                </div>
                <div class="col-lg-2 col-md-3 col-sm-3 col-xs-3 pl-1" <%--style="margin-left: 10px"--%>>
                    <div id="txtEndDate" style="margin: 5px;"></div>
                </div>
                <div class="margin" style="float: right; margin: 5px;">
                    <div id="btnShowCards">
                    </div>
                    <div id="btnProjectComplexity">
                    </div>
                    <div id="btnSwitchtoMain">
                    </div>
                </div>
            </div>
            <div class="row d-flex justify-content-between" style="padding-top: 10px;">
                <div class="noPadding thirdRow" style="width:44%;">

                    <div class="piechartclass row" id="piechart">
                    </div>
                    <div class="row" style="padding-top: 80px;">
                    </div>
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
                    <div class="barChart barPanel row" id="divbarchart" style="height: 600px; width: 600px;">
                    </div>
                    <div class="cardPanel" style="display: none;">
                        <div id="divBillingsMonth1">
                        </div>
                        <div id="divBillingYTD1">
                        </div>
                    </div>

                </div>
            </div>
        </div>

    </div>
</div>
<div id="popup"></div>