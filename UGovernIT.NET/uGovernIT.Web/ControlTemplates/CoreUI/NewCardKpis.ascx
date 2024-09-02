<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewCardKpis.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.CoreUI.NewCardKpis" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
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
    var baseUrl = ugitConfig.apiBaseUrl;

    var SelectedYear = new Date().getFullYear();
    var CurrentYear = new Date().getFullYear();

    var date = new Date();
    var reportParam =
    {
        currentmonthStartDate: new Date(date.getFullYear(), date.getMonth(), 1),
        currentmonthEndDate: new Date(date.getFullYear(), date.getMonth() + 1, 0),
        currentyearStartDate: new Date(date.getFullYear(), 0, 1),
        currentyearEndDate: new Date(date.getFullYear(), date.getMonth(), date.getDay()),
        currentDate: new Date(date.getFullYear(), date.getMonth(), date.getDay())
    };

    $(document).ready(function () {
        $("#topBillingWorkMonthContainer").css("height", '<%=Height%>');
        $("#topBillingWorkMonthContainer").css("width", '<%=Width%>');
        var viewtype = '<%=HeadType%>';
        $.get(baseUrl + "/api/CoreRMM/GetCardKpis?filter=monthly&Type=" + viewtype, function (data) {
            
            var monthDataSource = Object.entries(data);
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
                    itemElement.append(`<Div class='MonthTitle'>${date.getMonth()} ${date.getFullYear()}</Div><br>`);
                    for (i = 0; i < item.length; i++) {
                        itemElement.append(`<div class='rowitem'><b>${item[i].HeadName}: </b><div style='float:right;'>${item[i].HeadCount}</div ></div >`);
                    }
                    
                },
                onItemClick: function (e) {
                    //debugger;
                    var data = e.itemData;
                    window.parent.UgitOpenPopupDialog('/layouts/ugovernit/delegatecontrol.aspx?control=BillingAndMarginsReport', '', 'Resource Management', '90', '90', '', true);
                }
            });

        });

        $.get(baseUrl + "/api/CoreRMM/GetCardKpis?filter=yearly&Type=" + viewtype, function (data) {
            
            var tileDataSource = Object.entries(data);
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
                    debugger;
                    var item = itemData[1];
                    itemElement.attr("class", "TileStyle");
                    itemElement.append(`<Div class='MonthTitle'>${date.getFullYear()} YTD</Div><br>`);
                    for (i = 0; i < item.length; i++) {
                        itemElement.append(`<div class='rowitem'><b>${item[i].HeadName}: </b><div style='float:right;'>${item[i].HeadCount}</div ></div >`);
                    }
                },
                onItemClick: function (e) {
                    //debugger;
                    var data = e.itemData;
                    window.parent.UgitOpenPopupDialog('/layouts/ugovernit/delegatecontrol.aspx?control=BillingAndMarginsReport', '', 'Resource Management', '90', '90', '', true);
                }
            });

        });
        
    });
</script>
<div id="topBillingWorkMonthContainer">
    <div id="divBillingsMonth1" style="width:25%;float:left;">

    </div>
    <div id="divBillingYTD1" style="width:25%;float:left">

    </div>
</div>