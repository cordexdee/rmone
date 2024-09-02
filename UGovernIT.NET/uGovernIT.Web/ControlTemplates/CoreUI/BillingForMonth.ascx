<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BillingForMonth.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.CoreUI.BillingForMonth" %>
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
        $("#topBillingForMonthContainer").css("height", '<%=Height%>');
        $("#topBillingForMonthContainer").css("width", '<%=Width%>');

        //debugger;
        var divbillignandmargin = $("#divBillingsMonth").dxTileView ({
            dataSource: "/api/rmmapi/GetBillingAndMargins?Mode=Monthly&StartDate=" + reportParam.currentmonthStartDate.toDateString()
                + "&EndDate=" + reportParam.currentmonthEndDate.toDateString() + "&CPR=true&OPM=true&CNS=true&Pipeline=true&Current=true&Closed=false&Billable=true&Overhead=false",
            showScrollbar: false,
            baseItemHeight: 250,
            baseItemWidth: 280,
            itemMargin: 32,
            direction: "vertical",
            width: 320,
            height: 300,
            itemTemplate: function (itemData, itemIndex, itemElement) {
                itemElement.attr("class", "TileStyle");
                itemElement.append(`<Div class='MonthTitle'>${date.getFullYear()} ${itemData.Month}</Div><br>`);
                itemElement.append("<div class='rowitem'><b>Billed Resources: </b><div style='float:right;'>" + itemData.BilledResources + "</div ></div >");
                itemElement.append("<div class='rowitem'><b>Unbilled Resources: </b><div style='float:right;'>" + itemData.UnbilledResources + "</div ></div >");
                itemElement.append("<div class='rowitem'><b>Resource Billings: </b><div style='float:right;'>" + itemData.TotalBillingLaborRate + "</div ></div >");
                itemElement.append("<div class='rowitem'><b>Resource Cost: </b><div style='float:right;'>" + itemData.TotalEmployeeCostRate + "</div ></div >");
                //itemElement.append("<span><b>Project Count: </b>" + itemData.TotalProjects + "</span ><br> ");
                //itemElement.append("<span><b>Project Capacity: </b>" + itemData.TotalCapacity + "</span ><br> </div> ");
                itemElement.append("<div class='rowitem'><b>Gross Margin: </b><div style='float:right;'>" + itemData.GrossMargin + "</div ></div></div>");
                itemElement.append("<div class='rowitem'><b>Missed Revenues: </b><div style='float:right;'>" + itemData.MissedRevenues + "</div ></div></div>");
            },
            onItemClick: function (e) {
                //debugger;
                var data = e.itemData;
                window.parent.UgitOpenPopupDialog('/layouts/ugovernit/delegatecontrol.aspx?control=BillingAndMarginsReport', '', 'Resource Management', '90', '90', '', true);
            }
        });

        var divYTDmargin = $("#divBillingYTD").dxTileView({
            dataSource: "/api/rmmapi/GetBillingAndMarginsYTD?Mode=Monthly&StartDate=" + reportParam.currentyearStartDate.toDateString()
                + "&EndDate=" + reportParam.currentDate.toDateString() + "&CPR=true&OPM=true&CNS=true&Pipeline=true&Current=true&Closed=false&Billable=true&Overhead=false",
            showScrollbar: false,
            baseItemHeight: 250,
            baseItemWidth: 280,
            itemMargin: 32,
            direction: "vertical",
            width: 320,
            height:300,
            itemTemplate: function (itemData, itemIndex, itemElement) {
                itemElement.attr("class", "TileStyle");
                itemElement.append(`<Div class='MonthTitle'>${date.getFullYear()} ${date.format('MMM').toUpperCase()} YTD</Div><br>`);
                itemElement.append("<div class='rowitem'><b>Billed Resources: </b><div style='float:right;'>" + itemData.BilledResources + "</div></div >");
                itemElement.append("<div class='rowitem'><b>Unbilled Resources: </b><div style='float:right;'>" + itemData.UnbilledResources + "</div></div >");
                itemElement.append("<div class='rowitem'><b>Resource Billings: </b><div style='float:right;'>" + itemData.TotalBillingLaborRate + "</div></div >");
                itemElement.append("<div class='rowitem'><b>Resource Cost: </b><div style='float:right;'>" + itemData.TotalEmployeeCostRate + "</div></div >");
                //itemElement.append("<span><b>Project Count: </b>" + itemData.TotalProjects + "</span ><br> ");
                //itemElement.append("<span><b>Project Capacity: </b>" + itemData.TotalCapacity + "</span ><br> </div> ");
                itemElement.append("<div class='rowitem'><b>Gross Margin: </b><div style='float:right;'>" + itemData.GrossMargin + "</div></div></div>");
                itemElement.append("<div class='rowitem'><b>Missed Revenues: </b><div style='float:right;'>" + itemData.MissedRevenues + "</div></div></div>");
            },
            onItemClick: function (e) {
                //debugger;
                var data = e.itemData;
                window.parent.UgitOpenPopupDialog('/layouts/ugovernit/delegatecontrol.aspx?control=BillingAndMarginsReport', '', 'Resource Management', '90', '90', '', true);
            }
        });
    });
</script>
<div id="topBillingForMonthContainer">
    <div id="divBillingsMonth" style="width:25%;float:left;">

    </div>
    <div id="divBillingYTD" style="width:25%;float:left">

    </div>
</div>