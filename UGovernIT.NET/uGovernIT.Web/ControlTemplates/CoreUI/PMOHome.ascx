<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMOHome.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.CoreUI.PMOHome" %>

<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #filters{
        padding-left:5px;
        height:600px;
    }

    .piechartclass rect{
        opacity:0;
    }

    ul#filters > li {
        margin: 15px;
        box-shadow: 0 1px 2px #ccc;
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
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var filterone = ["Project Requests", "Open", "Closed"];
    var filtertwo = ["Project Type", "Priority", "Project Class"];

    var request = {
        Filter: 'Project Requests',
        Base: 'Project Type',
        Type: 'Project',
        Selection: '',
    }

    var date = new Date();
    var reportParam =
    {
        StartDate: new Date(date.getFullYear(), 0, 1),
        EndDate: new Date(date.getFullYear(), 11, 31),
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

    $(() => {

        $(".resourceViewContainer").height('<%=Height%>' == '' ? '300px' : '<%=Height%>');
        $(".resourceViewContainer").width('<%=Width%>' == '' ? '300px' : '<%=Width%>');
       
        $("#filterone").dxRadioGroup({
            items: filterone,
            value: filterone[0],
            itemTemplate: function (itemData, itemIndex, itemElement) {
                itemElement.append(
                    $("<div />").attr("style", "padding:15px;")
                        .text(itemData)
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
            items: filtertwo,
            value: filtertwo[0],
            itemTemplate: function (itemData, itemIndex, itemElement) {
                itemElement.append(
                    $("<div />").attr("style", "padding:15px;")
                        .text(itemData)
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

        

        $("#btnSwitchtoMain").dxButton({
            icon: '/Content/Images/sectorIcon.png',
            height: 35,
            width: 35,
            onClick: function (e) {
                let filter1 = $("#filterone").dxRadioGroup("instance");
                let filter2 = $("#filtertwo").dxRadioGroup("instance");

                request.Filter = filter1.option('value');
                request.Base = filter2.option('value');
                request.Selection = '';
                loadCardApis(request.Filter, request.Base, request.Type, '', '');
                LoadPieChart();

            }
        });

        LoadPieChart();
        loadCardApis(request.Filter, request.Base, request.Type, '', '');
    });

    function LoadPieChart() {

        $.get(baseUrl + "/api/CoreRMM/GetPMOChartData?Filter=" + request.Filter + "&Base=" + request.Base + "&Type=" + request.Type, function (data) {
            products = data;
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

            $('#piechart').dxPieChart({
                size: {
                    height: 550,
                    width: 550,
                },
                palette: 'Soft Blue',
                dataSource: topProducts,
                series: [
                    {
                        argumentField: 'Title',
                        valueField: valueField,
                        label: {
                            visible: true,
                            position:'inside',
                            connector: {
                                visible: true,
                                width: 1,
                            },
                            font: {
                                size: 22
                            },
                            customizeText(point) {
                                
                                if (request.Base == 'Division')
                                    return `${point.argumentText}<br>${point.value}`;
                                else
                                    return `${point.argumentText.replace(/ /g, "\r\n")}<br>${point.value}`;
                            },
                        },
                        minSegmentSize: 1000000,
                        smallValuesGrouping: {
                            groupName: "others",
                            mode: "none",
                        },
                        //hoverStyle: {
                        //    hatching: {
                        //        step: 2
                        //    }
                        //},
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
                    enabled: false
                },
                onPointClick(e) {
                    const point = e.target;
                    var piechart = e.component;
                    if (point.data.Name == "Others") {

                        piechart.option("dataSource", lastProucts);
                        piechart.option("palette", "Soft Blue");


                    } else {
                        if (point.data.DataType == request.Base) {
                            let selectedSector = point.data.Name;
                            
                            var base = 'Division'
                            if (request.Base == 'Division')
                                base = 'Sector';
                            if (request.Base == 'Studio')
                                base = 'Division'

                            request.Selection = request.Base + "~" + selectedSector;
                            request.ParentTitle = point.data.Title;
                            //loadCardApis(request.Filter, base, request.Type, selectedSector, point.data.DataType);
                            $.get(baseUrl + "/api/CoreRMM/GetProjectChartData?Filter=" + request.Filter + "&Base=" + base + "&Type=" + request.Type
                                + "&Selection=" + selectedSector + "&SelectionDataType=" + point.data.DataType,
                                function (breakupdata) {
                      
                                    products = breakupdata;
                                    topProducts = products.slice(-5);
                                    if (products.length > 5) {
                                        lastProucts = products.slice(0, products.length - 5);
                                        if (lastProucts.length > 0)
                                            topProducts.push({
                                                Name: 'Others',
                                                Title:'Others',
                                                Amount: lastProucts.map(item => item.Amount).reduce((prev, next) => prev + next),
                                                DataType: lastProucts[0].DataType
                                            });
                                    }
                                    piechart.option("dataSource", topProducts);
                                    piechart.option("palette", "Harmony Light");
                                    //piechart.option("selectionMode", "none");
                                    //piechart.series[0]._seriesModes.seriesSelectionMode = 'none'
                                });
                        }
                        else {
                            //last drill down
                            request.ParentSelection = request.Selection;
                            request.ChildSelection = point.data.Name;
                            request.DataType = point.data.DataType;

                            var title = request.ParentTitle + "  >  " + point.data.Title;

                            window.parent.UgitOpenPopupDialog("/Layouts/uGovernIT/DelegateControl.aspx?control=ProjectListDrillDown&Filter=" + request.Filter + "&Base=" + request.DataType + "&Type=" + request.Type + "&ParentSelection="
                                + request.ParentSelection + "&ChildSelection=" + request.ChildSelection, "", title, 90, 90, 0,'');
                        }
                    }
                },
                customizePoint: function (pointInfo) {
                   
                },
            });


        });
    }

    function getText(item, text) {
        return `<a>${products[item.index].Name}</a> `;
    }

    function loadCardApis(filter, base, type, selectedPoint, selectionDataType) {

        var viewtype = '<%=HeadType%>';
        $.get(baseUrl + "/api/CoreRMM/GetPMOCardKpis?Period=Now&Filter=" + filter + "&Base=" + base + "&Type=" + viewtype
            + "&Selection=" + selectedPoint + "&SelectionDataType=" + selectionDataType, function (monthlydata) {

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
                        itemElement.append(`<Div class='MonthTitle'>${date.toLocaleString('default', { month: 'short' })} ${date.getFullYear()}</Div><br>`);
                        for (i = 0; i < item.length; i++) {
                            itemElement.append(`<div class='rowitem'><b>${item[i].HeadName}: </b><div style='float:right;'>${item[i].HeadCount}</div ></div >`);
                        }

                    },
                    onItemClick: function (e) {
                        //
                        var data = e.itemData;
                        //window.parent.UgitOpenPopupDialog('/layouts/ugovernit/delegatecontrol.aspx?control=BillingAndMarginsReport', '', 'Resource Management', '90', '90', '', true);
                    }
                });

                
            });

        //load YTD card after monthy card to stop api timeout problem
        $.get(baseUrl + "/api/CoreRMM/GetPMOCardKpis?Period=monthly&Filter=" + filter + "&Base=" + base + "&Type=" + viewtype
            + "&Selection=" + selectedPoint + "&SelectionDataType=" + selectionDataType, function (ytddata) {

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
                        itemElement.attr("class", "TileStyle");
                        itemElement.append(`<Div class='MonthTitle'>${date.getFullYear()} ${date.toLocaleString('default', { month: 'short' })} YTD</Div><br>`);
                        for (i = 0; i < item.length; i++) {
                            itemElement.append(`<div class='rowitem'><b>${item[i].HeadName}: </b><div style='float:right;'>${item[i].HeadCount}</div ></div >`);
                        }
                    },
                    onItemClick: function (e) {
                        //
                        var data = e.itemData;
                        //window.parent.UgitOpenPopupDialog('/layouts/ugovernit/delegatecontrol.aspx?control=BillingAndMarginsReport', '', 'Resource Management', '90', '90', '', true);
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

<div class="resourceViewContainer">
    <div>
        <div class="row itsmCtrl-buttonlist">
            <a href="/Pages/PMMProjects" class="btn btn-md ITSMButton-secondary" id="NPR">Project Requests</a>
            <a href="/Pages/NPRRequests" class="btn btn-md ITSMButton-secondary" id="PMM">Active Projects</a>
            <a href="/Pages/Initiatives" class="btn btn-md ITSMButton-secondary" id="Initiatives">Business Initiatives</a>
          </div>
        <div class="row">
            <div class="margin" style="float:right;">
                    <div id="btnSwitchtoMain">
                
                    </div>
                </div>
        </div>
        <div class="row">
            <div class="col-md-2 col-sm-2 col-xs-2 noPadding">
                <ul id="filters">
                    <li><div  id="filterone"></div></li>
                    <li><div  id="filtertwo"></div></li>
                </ul>
            </div>

            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
            
                <div class="piechartclass" id="piechart">

                </div>
            </div>

            <div class="col-md-3 col-sm-3 col-xs-3 noPadding">
                <div  id="divBillingsMonth1">

                </div>
                <div  id="divBillingYTD1">

                </div>
            </div>
        </div>
    </div>

</div>
<div id="popup"></div>