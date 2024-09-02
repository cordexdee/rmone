<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BillingWorkMonth.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.CoreUI.BillingWorkMonth" %>
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
        $("#topBillingWorkMonthContainer").css("height", '<%=Height%>');
        $("#topBillingWorkMonthContainer").css("width", '<%=Width%>');

        $.get(baseUrl + "/api/RMMAPI/GetBilledWorkMonth", function (data) {
            var monthviewdata = [];
            monthviewdata.push(data.find(item => item.StartMonth == 'Jan'));
            var ytdDataList = data.filter(item => item.MonthNumber <= date.getMonth());
            var ytdData = [];
            ytdData.push({
                StartMonth: '',
                BilledWorkMonth: ytdDataList.map(item => item.BilledWorkMonth).reduce((prev, next) => prev + next),
                UnBilledWorkMonth: ytdDataList.map(item => item.UnBilledWorkMonth).reduce((prev, next) => prev + next),
                Utilization: ytdDataList.map(item => item.Utilization).reduce((prev, next) => prev + next),
                TotalBillingLaborRate: ytdDataList.map(item => item.TotalBillingLaborRate).reduce((prev, next) => prev + next),
                TotalEmployeeCostRate: ytdDataList.map(item => item.TotalEmployeeCostRate).reduce((prev, next) => prev + next),
                GrossMargin: ytdDataList.map(item => item.GrossMargin).reduce((prev, next) => prev + next)
            });
            var divbillignandmargin = $("#divBillingsMonth1").dxTileView({
                dataSource: monthviewdata,
                showScrollbar: false,
                baseItemHeight: 250,
                baseItemWidth: 280,
                itemMargin: 32,
                direction: "vertical",
                width: 320,
                height: 300,
                itemTemplate: function (itemData, itemIndex, itemElement) {
                    
                    itemElement.attr("class", "TileStyle");
                    itemElement.append(`<Div class='MonthTitle'>${date.getFullYear()} ${itemData.StartMonth}</Div><br>`);
                    itemElement.append("<div class='rowitem'><b>No of Billed Work Months: </b><div style='float:right;'>" + itemData.BilledWorkMonth.toFixed(2) + "</div ></div >");
                    itemElement.append("<div class='rowitem'><b>No of Unbilled Work Months: </b><div style='float:right;'>" + itemData.UnBilledWorkMonth.toFixed(2) + "</div ></div >");

                    itemElement.append("<div class='rowitem'><b>Utilization %: </b><div style='float:right;'>" + itemData.Utilization.toFixed(2) + "%</div ></div></div>");
                    itemElement.append("<div class='rowitem'><b>Resource Billings: </b><div style='float:right;'>" + convertToMillion(itemData.TotalBillingLaborRate) + "</div ></div >");
                    itemElement.append("<div class='rowitem'><b>Resource Cost: </b><div style='float:right;'>" + convertToMillion(itemData.TotalEmployeeCostRate) + "</div ></div >");
                    //itemElement.append("<span><b>Project Count: </b>" + itemData.TotalProjects + "</span ><br> ");
                    //itemElement.append("<span><b>Project Capacity: </b>" + itemData.TotalCapacity + "</span ><br> </div> ");
                    itemElement.append("<div class='rowitem'><b>Gross Margin: </b><div style='float:right;'>" + convertToMillion(itemData.GrossMargin) + "</div ></div></div>");
                },
                onItemClick: function (e) {
                    //debugger;
                    var data = e.itemData;
                    window.parent.UgitOpenPopupDialog('/layouts/ugovernit/delegatecontrol.aspx?control=BillingAndMarginsReport', '', 'Resource Management', '90', '90', '', true);
                }
            });

            var divYTDmargin = $("#divBillingYTD1").dxTileView({
                dataSource: ytdData,
                showScrollbar: false,
                baseItemHeight: 250,
                baseItemWidth: 280,
                itemMargin: 32,
                direction: "vertical",
                width: 320,
                height: 300,
                itemTemplate: function (itemData, itemIndex, itemElement) {
                    
                    itemElement.attr("class", "TileStyle");
                    itemElement.append(`<Div class='MonthTitle'>${date.getFullYear()} YTD</Div><br>`);
                    itemElement.append("<div class='rowitem'><b>No of Billed Work Months: </b><div style='float:right;'>" + (itemData.BilledWorkMonth / data.length).toFixed(2)  + "</div ></div >");
                    itemElement.append("<div class='rowitem'><b>No of Unbilled Work Months: </b><div style='float:right;'>" + (itemData.UnBilledWorkMonth / data.length).toFixed(2) + "</div ></div >");

                    itemElement.append("<div class='rowitem'><b>Utilization %: </b><div style='float:right;'>" + (itemData.Utilization).toFixed(2) + "%</div ></div></div>");
                    itemElement.append("<div class='rowitem'><b>Resource Billings: </b><div style='float:right;'>" + convertToMillion(itemData.TotalBillingLaborRate) + "</div ></div >");
                    itemElement.append("<div class='rowitem'><b>Resource Cost: </b><div style='float:right;'>" + convertToMillion(itemData.TotalEmployeeCostRate) + "</div ></div >");
                    //itemElement.append("<span><b>Project Count: </b>" + itemData.TotalProjects + "</span ><br> ");
                    //itemElement.append("<span><b>Project Capacity: </b>" + itemData.TotalCapacity + "</span ><br> </div> ");
                    itemElement.append("<div class='rowitem'><b>Gross Margin: </b><div style='float:right;'>" + convertToMillion(itemData.GrossMargin) + "</div ></div></div>");
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