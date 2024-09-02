<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecruitmentView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.CoreUI.RecruitmentView" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #filters{
        padding-left:5px;
    }

    ul#filters > li {
        margin: 5px;
        /*box-shadow: 0 1px 2px #ccc;*/
        border: 1px solid lightgrey;
        padding-top: 13px;
        padding-bottom: 10px;
        list-style: none;
        border-radius: 12px;
        margin-top: 47px;
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

    .startDate {
        float: left;
        width: 49.5%;
        margin-top: 10px;
    }

    .endDate {
        float: right;
        width: 49.5%;
        margin-top: 10px;
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

    .dashboard-panel-new {
        border: none !important;
        padding: 15px;
        background-color: #DFDFDF !important;
    }

    .cardPanelFilterSpacing {
        border-right: 10px solid #DFDFDF;
    }

    .cardPanelDDLSpacing {
        border-left: 10px solid #DFDFDF;
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var filterone = [{ Name: "Tracked Work", Value: "Pipeline" }, { Name: "Contracted", Value: "Contracted" }, { Name: "Closed", Value: "Closed" }];
    var filtertwo = [{ Name: "Sector", Value: "Sector" }, { Name: '<%=DivisionLabel%>', Value: "Division" }, { Name: '<%=StudioLabel%>', Value: "Studio" }];
    var filterthree = ["Date Range", "3 Month", "6 Month"];

    //var colorCodes = ['#839BDA', '#E78683', '#F4CB86', '#97DA97', '#7AB8EB', '#5AB7BE', '#4EE2EC', '#50C878'];

    var date = new Date();
    var startDate = addMonths(new Date(), - 12);
    var endDate = addMonths(new Date(), 0);
    var request = {
        Filter: 'Pipeline',
        Base: 'Sector',
        Type: 'Recruitment',
        Selection: '',
        Period: 'Date Range',
        StartDate: new Date(startDate).toUTCString(),
        EndDate: new Date(endDate).toUTCString(),
        Studio: '',
        Division: '',
        Sector: ''
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

 
    var selectBoxData = [];
    var products = [];
    var topProducts = [];
    var lastProucts = [];
    var baseUrl = ugitConfig.apiBaseUrl;

    $(() => {

        $(".resourceViewContainer").height('<%=Height%>' == '' ? '600px' : '<%=Height%>');
        $(".resourceViewContainer").width('<%=Width%>' == '' ? '800px' : '<%=Width%>');

        $.get("/api/CoreRMM/GetDivisions?OnlyEnabled=1", function (data, status) {
            Divisions = data;
            const division = $('#ddlDivision').dxSelectBox({
                dataSource: Divisions,
                placeholder: '<%=DivisionLabel%>',
                showClearButton: true,
                valueExpr: "ID",
                displayExpr: "Title",
                onValueChanged: function (e) {
                    if (e.value == 'All' || e.value == 'null') {
                        e.value = '';
                    }
                    request.Division = e.value || 0;
                    LoadPieChart("", "");
                    if ('<%=EnableStudioDivisionHierarchy%>'.toUpperCase() == "TRUE") {
                        $.get(baseUrl + "/api/CoreRMM/GetStudios?division=" + request.Division, function (studioData, status) {
                            var studiobox = $("#ddlStudio").dxSelectBox("instance");
                            studiobox.option("dataSource", studioData);
                        });
                    }
                }
            });
        });


        $.get(baseUrl + "/api/CoreRMM/GetStudios?division=" + request.Division, function (data, status) {
            const studio = $('#ddlStudio').dxSelectBox({
                dataSource: data,
                placeholder: '<%=StudioLabel%>',
                showClearButton: true,
                displayExpr: '<%=EnableStudioDivisionHierarchy%>'.toUpperCase() == "TRUE" ? "FieldDisplayName" : "Title",
                valueExpr: 'ID',
                onValueChanged: function (e) {
                    if (e.value == 'All' || e.value == 'null')
                        e.value = '';
                    request.Studio = e.value || '';
                    LoadPieChart("", "");
                }
            });
        })


        $.get(baseUrl + "/api/CoreRMM/GetSectorStudioData", function (data, status) {
            if (data.length > 1) {

                const sector = $('#ddlSector').dxSelectBox({
                    dataSource: data[0].Data.split(";#"),
                    placeholder: 'Sector',
                    showClearButton: true,
                    onValueChanged: function (e) {
                        if (e.value == 'All' || e.value == 'null')
                            e.value = '';
                        request.Sector = e.value || '';
                        LoadPieChart("", "");
                    }
                });
            }

        });
        

        $('#txtStartDate').dxDateBox({
            type: 'date',
            value: request.StartDate,
            onValueChanged(data) {
                var endDate = $("#txtEndDate").dxDateBox("instance");
                var enddateString = endDate.option("value");
                request.StartDate = data.value.toLocaleString();
                request.EndDate = enddateString.toLocaleString();
                LoadPieChart(data.value.toLocaleString(), enddateString.toLocaleString());
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
                LoadPieChart(startdateString.toLocaleString(), data.value.toLocaleString());
            },
        });
       
        $("#filterone").dxRadioGroup({
            dataSource: filterone,
            value: filterone[0].Name,
            displayExpr: 'Name',
            valueExpr: 'Value',
            itemTemplate: function (itemData, itemIndex, itemElement) {
                itemElement.append(
                    $("<div />").attr("style", "padding:5px;")
                        .text(itemData.Name)
                );
            },
            onValueChanged(e) {       
                request.Filter = e.value;
                LoadPieChart("", "");
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
                    $("<div />").attr("style", "padding:5px;")
                        .text(itemData.Name)
                );
            },
            onValueChanged(e) {
                request.Base = e.value;
                LoadPieChart("", "");
            },
            elementAttr: {
                class: 'radioGroup'
            },
        });

        $("#filterthree").dxRadioGroup({
            items: filterthree,
            value: filterthree[0],
            itemTemplate: function (itemData, itemIndex, itemElement) {
                itemElement.append(
                    $("<div />").attr("style", "padding:5px;")
                        .text(itemData)
                );
            },
            onValueChanged(e) {
                request.Period = e.value;

                if (e.value == 'Date Range') {
                    var eDate = new Date();
                    $("#txtStartDate").dxDateBox("instance").option({ value: startDate });
                    $("#txtEndDate").dxDateBox("instance").option({ value: eDate });
                }
               else if (e.value == '3 Month') {
                    var eDate = new Date(); // Now
                   eDate.setDate(eDate.getDate() + 90);
                   $("#txtEndDate").dxDateBox("instance").option({ value: eDate });
                   var sDate = new Date();
                   $("#txtStartDate").dxDateBox("instance").option({ value: sDate });
                }
                else if (e.value == '6 Month') {
                   var eDate = new Date(); // Now
                   eDate.setDate(eDate.getDate() + 180);
                   $("#txtEndDate").dxDateBox("instance").option({ value: eDate });
                   var sDate = new Date();
                   $("#txtStartDate").dxDateBox("instance").option({ value: sDate });
                }

                LoadPieChart("", "");
            },
            elementAttr: {
                class:'radioGroup'
            }
        });

        $("#btnSwitchtoMain").dxButton({
            icon: '/Content/Images/sectorIcon.png',
            height: 35,
            width: 35,
            onClick: function (e) {
                let filter1 = $("#filterone").dxRadioGroup("instance");
                //let filter2 = $("#filtertwo").dxRadioGroup("instance");
                let filter3 = $("#filterthree").dxRadioGroup("instance");
                
                request.Filter = filter1.option('value');
                //request.Base = filter2.option('value');
                
                request.Selection = '';
                LoadPieChart("", "");

                $("#txtStartDate").dxDateBox("instance").option({ value: startDate });
                $("#txtEndDate").dxDateBox("instance").option({ value: endDate });
                filter3.option('value','Date Range')
            }
        });

        $("#btnProjectComplexity").dxButton({
            icon: '/Content/Images/complex.png',
            height: 35,
            width: 35,
            hint: 'Show Capacity',
            onClick: function (e) {
                const sDate = new Date(request.StartDate);
                const eDate = new Date(request.EndDate);
                const headerTitle = `Capacity From ${sDate.getMonth()}/${sDate.getFullYear()} To ${eDate.getMonth()}/${eDate.getFullYear()}`;
                window.parent.UgitOpenPopupDialog("/layouts/uGovernIT/DelegateControl.aspx?control=capcityreport&IsResourceDrillDown=true&Filter="
                    + request.Filter + "&pStartDate=" + request.StartDate + "&pEndDate=" + request.EndDate, "", headerTitle, 90, 90, 0, '');
            }
        });

        LoadPieChart("","");
    });

    function LoadPieChart(startDate, endDate) {
        $.get(baseUrl + "/api/CoreRMM/GetResourceRequired?Period=" + request.Period + "&Filter=" + request.Filter + "&Base=" + request.Base + "&Type=" + request.Type
            + "&StartDate=" + request.StartDate + "&EndDate=" + request.EndDate + "&Studio=" + request.Studio + "&Division=" + request.Division + "&Sector=" + request.Sector,
            function (result) {
                var dataSource = result?.data;
                var maxResourceRequired = result?.maxResourceRequired;
                $('#barchart').dxChart({
                    dataSource,
                    //width: 600,
                    //height:600,
                    palette: 'soft',
                    rotated: true,
                    legend: {
                        visible: false
                    },
                    //title: {
                    //    //text: 'Resources needed over current year',
                    //    font: {
                    //        color: '#000',
                    //        size: 16,
                    //        weight: 500,
                    //        family: 'Roboto, sans-serif !important',
                    //    }
                    //},
                    customizePoint(e) {
                        //var theRandomNumber = Math.floor(Math.random() * 10) + 1;
                        var index = e.index;
                        if (index > 9)
                            index = index - 9;
                        return {
                            color: colorCodes[index], hoverStyle: { color: colorCodes[index] } 
                        };
                    },
                    argumentAxis: {
                        label: {
                            visible: false
                        }
                    },
                    valueAxis: {
                        label: {
                            visible: true
                        },
                        visualRange: {
                            startValue: -1 * maxResourceRequired,
                            endValue: maxResourceRequired,
                        },
                    },
                    series: [{
                        argumentField: 'RoleId',
                        valueField: 'ResourceRequired',
                        type: 'bar',
                        barWidth:30,
                        label: {
                            position: 'outside',
                            visible: true,
                            font: {
                                size: 16,
                            },
                            customizeText(point) {
                                let color = isColorDark(point.point._options.color);
                                if (point.percent == 0)
                                    color = '#000000';
                                return `<div style='color:${color}'><span style="font-size:16px;color:#000;font-family:Roboto, sans-serif !important;">${point.argumentText}</span> (${point.value})</div>`;
                            },
                            backgroundColor: 'none',
                        },
                    
                    }]
                });

        });
    }

    function getText(item, text) {
        return `<a>${products[item.index].Name}</a> `;
    }


</script>
<div class="row" style="margin-top:-4px;">
    <div class="discription-title">Recruitment Analytics</div>
    <div class="resourceViewContainer" style="overflow-y: auto">
        <div class="title margin fontsize">
            <div class="row pb-1 secondRow">
                <div class="col-lg-4 col-md-3 col-sm-3 col-xs-3"></div>
                <div class="col-lg-2 col-md-3 col-sm-3 col-xs-3 pr-1" <%--style="margin-right: 10px"--%>>
                    <div id="txtStartDate" style="margin: 5px;"></div>
                </div>
                <div class="col-lg-2 col-md-3 col-sm-3 col-xs-3 pl-1" <%--style="margin-left: 10px"--%>>
                    <div id="txtEndDate" style="margin: 5px;"></div>
                </div>
                <div class="margin" style="float: right;margin: 5px;">
                    <div id="btnProjectComplexity" style="display: none">
                    </div>
                    <div id="btnSwitchtoMain">
                    </div>
                </div>
            </div>
            <div class="row d-flex justify-content-between" style="padding-top:10px;">
                <div class="noPadding thirdRow cardPanelFilterSpacing" style="width:10%;">
                    <ul id="filters">
                        <li>
                            <div id="filterone"></div>
                        </li>
                        <%--<li><div  id="filtertwo"></div></li>--%>
                        <li>
                            <div id="filterthree"></div>
                        </li>
                    </ul>
                </div>
                <div class="pl-2 thirdRow" style="width:70%;padding-top:25px;">
                    <div id="barchart" class="row" style="height: 600px;">
                    </div>
                    <%--<div class="row">
                        
                    </div>--%>
                </div>
                <div class="pl-0 thirdRow cardPanelDDLSpacing" style="width:20%;">
                    <div class="mb-2" style="margin:10px;" id="ddlDivision"></div>
                    <div class="mb-2" style="margin:10px;" id="ddlStudio"></div>
                    <div class="mb-2" style="margin:10px;" id="ddlSector"></div>
                </div>
            </div>

        </div>
    </div>
</div>
<div id="popup"></div>