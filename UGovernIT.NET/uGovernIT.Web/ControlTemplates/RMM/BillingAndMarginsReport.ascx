<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BillingAndMarginsReport.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMM.BillingAndMarginsReport" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.4/jspdf.min.js"></script>
<%--<script src="js/jsPDF/dist/jspdf.min.js"></script>--%>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .TileStyle {
        border: 10px;
        padding: 15px;
        height: 220px;
        font-size: 13px;
        box-shadow: 3px 1px 5px rgb(0 0 0 / 20%);
        border: 1px solid #CCC;
        border-radius: 10px;
    }

    .currentMonth{
        border: 1.5pt solid #4A6EE2 !important;
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
    var reprotTypes = [
        "Billing And Margins",
        "Missed Revenue"
    ];

    var period = [
        
        "Monthly",
        "Quarterly",
        "Half Yearly"
    ];

    var SelectedYear = new Date().getFullYear();
    var CurrentYear = new Date().getFullYear();
    var lstYear = [];
    for (var i = CurrentYear; i >= 2018; i--) {
        lstYear.push(i);
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
        Closed: true,
        Billable: true,
        Overhead: false
    };
    

    var popupBilling = null;
    var popupBillingOptions = {
        width: 850,
        height: 500,
        contentTemplate: function () {
            return $("<div />").append(
                $("<Div />").dxDataGrid({
                    showColumnLines: false,
                    showRowLines: true,
                    rowAlternationEnabled: true,
                    showBorders: true,
                    dataSource: "/api/rmmapi/GetBillingDetails?Mode=" + reportParam.Mode + "&StartDate=" + reportParam.StartDate.toLocaleDateString() + "&EndDate=" + reportParam.EndDate.toLocaleDateString()
                        + "&CPR=" + reportParam.CPR + "&OPM=" + reportParam.OPM + "&CNS=" + reportParam.CNS +
                        "&Pipeline=" + reportParam.Pipeline + "&Current=" + reportParam.Current + "&Closed=" + reportParam.Closed,
                    grouping: {
                        autoExpandAll: true,
                    },
                    paging: {
                        pageSize: 10
                    },
                    groupPanel: {
                        visible: true
                    },
                    columns: [
                        {
                            dataField: "Name",
                            caption: "Name",
                            width: 200,
                            groupIndex:0
                        },
                        {
                            dataField: "JobProfile",
                            caption: "Job Profile",
                            width:200
                        },
                        {
                            dataField: "AllocationHour",
                            caption: "Allocation Hours",
                            alignment:"center"
                        },
                        {
                            dataField: "WorkItem",
                            caption: "Project",
                            width:120
                        },
                        {
                            dataField: "TotalBillingLaborRate",
                            caption: "Labor Rate",
                            alignment:"center"
                        },
                        {
                            dataField: "TotalEmployeeCostRate",
                            caption: "Cost Rate",
                            alignment:"center"
                        },
                        {
                            dataField: "GrossMargin",
                            caption: "Gross Margin",
                            alignment:"center"
                        }
                    ]
                })
            );
        },
        showTitle: false,
        title: "Allocation Billings",
        dragEnabled: false,
        hideOnOutsideClick: true
    };

    var showInfo = function () {
        if(popupBilling) {
            popupBilling.option("contentTemplate", popupBillingOptions.contentTemplate.bind(this));
        } else {
            popupBilling = $("#popup").dxPopup(popupBillingOptions).dxPopup("instance");
        }

        popupBilling.show();
    };

    var popupMissedRevenue = null;
    var popupMissedRevenueOptions = {
        width: 850,
        height: 500,
        contentTemplate: function () {
            return $("<div />").append(
                $("<Div />").dxDataGrid({
                    showColumnLines: false,
                    showRowLines: true,
                    rowAlternationEnabled: true,
                    showBorders: true,
                    dataSource: "/api/rmmapi/GetMissedRevenueDetails?Mode=" + reportParam.Mode + "&StartDate=" + reportParam.StartDate.toLocaleDateString() + "&EndDate=" + reportParam.EndDate.toLocaleDateString(),
                    grouping: {
                        autoExpandAll: true,
                    },
                    paging: {
                        pageSize: 10
                    },
                    columns: [
                        {
                            dataField: "Name",
                            caption: "Name",
                            width: 200
                        },
                        {
                        dataField: "JobProfile",
                        caption:"Job Profile"
                        },
                        {
                        dataField: "EmpLaborRate",
                        caption:"Labor Rate"
                        },
                        {
                            dataField: "EmpCostRate",
                            caption: "Cost Rate"
                        },
                        {
                            dataField: "TotalEmpBillingCost",
                            caption:"Total Billing"
                        },
                        {
                            dataField: "TotalEmpCost",
                            caption:"Total Cost"
                        },
                        {
                            dataField: "ResourceAavailablity",
                            caption:"Availability"
                        }
                    ]
                })
            );
        },
        showTitle: false,
        title: "Missed Resource Revenue",
        dragEnabled: false,
        hideOnOutsideClick: true
    };

    var showMissedRevenueInfo = function () {
        if(popupMissedRevenue) {
            popupMissedRevenue.option("contentTemplate", popupMissedRevenueOptions.contentTemplate.bind(this));
        } else {
            popupMissedRevenue = $("#popup").dxPopup(popupMissedRevenueOptions).dxPopup("instance");
        }

        popupMissedRevenue.show();
    };
    
    $(function () {

        $("#ddlYear").dxSelectBox({
            items: lstYear,
            value: lstYear[0],
            onValueChanged: function (e) {       
                SelectedYear = e.value;
                reportParam.StartDate = new Date(e.value, 0, 1);
                reportParam.EndDate = new Date(e.value, 11, 31);
                rebindTilesView();
            }
        });

        $("#ddReportType").dxSelectBox({
            items: reprotTypes,
            value: reprotTypes[0],
            onValueChanged: function (e) {
                if (e.value == "Billing And Margins") {
                    $("#DivBillingAndMargin").show();
                    $("#DivMissedRevenue").hide();
                }
                else {
                    $("#DivBillingAndMargin").hide();
                    $("#DivMissedRevenue").show();
                }
            }
        });

        $("#DivOpportunities").dxCheckBox({
            value: true,
            text: "Opportunity",
            visible: false,
            onValueChanged: function (e) {
                reportParam.OPM = e.value;
                rebindTilesView();
            }
        });

        $("#DivProjects").dxCheckBox({
            value: true,
            text:"Project",
            onValueChanged: function (e) {
                reportParam.CPR = e.value;
                rebindTilesView();
            }
        });

        $("#DivServices").dxCheckBox({
            value: true,
            text:"Service",
            onValueChanged: function (e) {
                reportParam.CNS = e.value;
                rebindTilesView();
            }
        });

        $("#DivPipeline").dxCheckBox({
            value: true,
            text:"Pipeline",
            onValueChanged: function (e) {
                reportParam.Pipeline = e.value;
                rebindTilesView();
            }
        });

        $("#DivCurrent").dxCheckBox({
            value: true,
            text:"Current",
            onValueChanged: function (e) {
                reportParam.Current = e.value;
                rebindTilesView();
            }
        });

        $("#DivClosed").dxCheckBox({
            value: true,
            text:"Closed",
            onValueChanged: function (e) {
                reportParam.Closed = e.value;
                rebindTilesView();
            }
        });

        $("#divBillable").dxCheckBox({
            value: true,
            text: "Billable",
            onValueChanged: function (e) {
                reportParam.Billable = e.value;
                rebindTilesView();
            }
        });

        $("#divOverhead").dxCheckBox({
            value: false,
            text: "Overhead",
            onValueChanged: function (e) {
                reportParam.Overhead = e.value;
                rebindTilesView();
            }
        });

        $("#DivExportToPdf").dxButton({
            text: "Export to Pdf",
            focusStateEnabled: false,
            onClick: function (e) {
                var element = $("#DivBillingAndMargin"); // global variable
                html2canvas(element, {
                    onrendered: function (canvas) {
                        var imgData = canvas.toDataURL('image/png');
                        //console.log('Report Image URL: ' + imgData);
                        var doc = new jspdf({ format: [500, 500], unit: 'mm', orientation: 'p', putOnlyUsedFonts: true }); //new jsPDF('l', 'cm', [1000,1000]); //210mm wide and 297mm high

                        doc.addImage(imgData, 'PNG', 10, 20);
                        doc.setFontSize(16);
                        doc.setTextColor(74, 110, 226)
                        doc.text(180, 15, 'Billing and Margins ' + SelectedYear);
                        doc.save('BillingAndMargins_' + SelectedYear +'.pdf');
                    }
                });
            }
        });

        $("#DivBillingAndMargin").dxTileView({
            dataSource: "/api/rmmapi/GetBillingAndMargins?Mode=" + reportParam.Mode + "&StartDate=" + reportParam.StartDate.toLocaleDateString() 
                + "&EndDate=" + reportParam.EndDate.toLocaleDateString() + "&CPR=" + reportParam.CPR + "&OPM=" + reportParam.OPM + "&CNS=" + reportParam.CNS
                + "&Pipeline=" + reportParam.Pipeline + "&Current=" + reportParam.Current + "&Closed=" + reportParam.Closed + "&Billable=true&Overhead=false",
            showScrollbar: true,
                baseItemHeight: 220,
                baseItemWidth: 260,
                itemMargin: 15,
            direction: "vertical",
            itemTemplate: function (itemData, itemIndex, itemElement) {
                if (new Date().toLocaleString('default', { month: 'short' }) === itemData.Month)
                    itemElement.attr("class", "TileStyle currentMonth");
                else
                    itemElement.attr("class", "TileStyle");

                itemElement.append("<Div class='MonthTitle'>" + itemData.Month + "</Div><br>");
                //itemElement.append("<div class='rowitem'><b>Billed Resources: </b><div style='float:right;'>" + itemData.BilledResources + "</div ></div >");
                //itemElement.append("<div class='rowitem'><b>Unbilled Resources: </b><div style='float:right;'>" + itemData.UnbilledResources + "</div ></div >");
                //itemElement.append("<div class='rowitem'><b>Resource Billings: </b><div style='float:right;'>" + itemData.TotalBillingLaborRate + "</div ></div >");
                //itemElement.append("<div class='rowitem'><b>Resource Cost: </b><div style='float:right;'>" + itemData.TotalEmployeeCostRate + "</div ></div >");
                //itemElement.append("<div class='rowitem'><b>Gross Margin: </b><div style='float:right;'>" + itemData.GrossMargin + "</div ></div></div>");
                //itemElement.append("<div class='rowitem'><b>Missed Revenues: </b><div style='float:right;'>" + itemData.MissedRevenues + "</div ></div></div>");

                itemElement.append("<div class='rowitem'><b>#Billed Work Month: </b><div style='float:right;'>" + itemData.BilledWorkMonth + "</div ></div >");
                itemElement.append("<div class='rowitem'><b>#Unbilled Work Month: </b><div style='float:right;'>" + itemData.UnBilledWorkMonth + "</div ></div >");
                itemElement.append("<div class='rowitem'><b>Utilization %: </b><div style='float:right;'>" + itemData.Utilization + "%</div ></div></div>");
                itemElement.append("<div class='rowitem'><b>Resource Billings: </b><div style='float:right;'>" + itemData.TotalBillingLaborRate + "</div ></div >");
                itemElement.append("<div class='rowitem'><b>Resource Cost: </b><div style='float:right;'>" + itemData.TotalEmployeeCostRate + "</div ></div >");
                itemElement.append("<div class='rowitem'><b>Gross Margin: </b><div style='float:right;'>" + itemData.GrossMargin + "</div ></div></div>");
            },
            onItemClick: function (e) {
                var data = e.itemData;
                if (data.Month == "Jan") {
                    reportParam.StartDate = new Date(SelectedYear, 0, 1);
                    reportParam.EndDate = new Date(SelectedYear, 0, 31);
                } else if (data.Month == "Feb") {
                    reportParam.StartDate = new Date(SelectedYear, 1, 1);
                    reportParam.EndDate = new Date(SelectedYear, 1, 28);
                } else if (data.Month == "Mar") {
                    reportParam.StartDate = new Date(SelectedYear, 1, 1);
                    reportParam.EndDate = new Date(SelectedYear, 2, 31);
                } else if (data.Month == "Apr") {
                    reportParam.StartDate = new Date(SelectedYear, 3, 1);
                    reportParam.EndDate = new Date(SelectedYear, 3, 30);
                } else if (data.Month == "May") {
                    reportParam.StartDate = new Date(SelectedYear, 4, 1);
                    reportParam.EndDate = new Date(SelectedYear, 4, 31);
                } else if (data.Month == "Jun") {
                    reportParam.StartDate = new Date(SelectedYear, 5, 1);
                    reportParam.EndDate = new Date(SelectedYear, 5, 30);
                } else if (data.Month == "Jul") {
                    reportParam.StartDate = new Date(SelectedYear, 6, 1);
                    reportParam.EndDate = new Date(SelectedYear, 6, 31);
                } else if (data.Month == "Aug") {
                    reportParam.StartDate = new Date(SelectedYear, 7, 1);
                    reportParam.EndDate = new Date(SelectedYear, 7, 31);
                } else if (data.Month == "Sep") {
                    reportParam.StartDate = new Date(SelectedYear, 8, 1);
                    reportParam.EndDate = new Date(SelectedYear, 8, 30);
                } else if (data.Month == "Oct") {
                    reportParam.StartDate = new Date(SelectedYear, 9, 1);
                    reportParam.EndDate = new Date(SelectedYear, 9, 31);
                } else if (data.Month == "Nov") {
                    reportParam.StartDate = new Date(SelectedYear, 10, 1);
                    reportParam.EndDate = new Date(SelectedYear, 10, 30);
                } else if (data.Month == "Dec") {
                    reportParam.StartDate = new Date(SelectedYear, 11, 1);
                    reportParam.EndDate = new Date(SelectedYear, 11, 31);
                }
                showInfo();
            }
        });

        $("#DivMissedRevenue").dxTileView({
            dataSource: "/api/rmmapi/GetMissedRevenues?Mode=" + reportParam.Mode + "&StartDate=" + reportParam.StartDate.toLocaleDateString() + "&EndDate=" + reportParam.EndDate.toLocaleDateString(),
            showScrollbar: true,
                baseItemHeight: 220,
                baseItemWidth: 220,
                itemMargin: 15,
            direction: "vertical",
            itemTemplate: function (itemData, itemIndex, itemElement) {
                itemElement.attr("class", "TileStyle");
                itemElement.append("<Div class='MonthTitle'>" + itemData.MonthName + "</Div><br>");
                itemElement.append("<span><b>Unbilled Resources: </b>" + itemData.ResourceNotBilled + "</span > <br>");
                itemElement.append("<span><b>Resource Potential Billings: </b>" + itemData.TotalMissedBilling + "</span ><br> ");
                itemElement.append("<span><b>Resource Costs: </b>" + itemData.TotalMissedCost + "</span ><br> ");
                itemElement.append("<span><b>Gross Margin: </b>" + itemData.GrossMargin + "</span ><br> </div> ");
            },
            onItemClick: function (e) {
                showMissedRevenueInfo();
            }
        });


    });

    function rebindTilesView() {
        
        var billinggrid = $("#DivBillingAndMargin").dxTileView("instance");
                billinggrid.option("dataSource", "/api/rmmapi/GetBillingAndMargins?Mode=" + reportParam.Mode + "&StartDate=" + reportParam.StartDate.toLocaleDateString()
                + "&EndDate=" + reportParam.EndDate.toLocaleDateString() + "&CPR=" + reportParam.CPR + "&OPM=" + reportParam.OPM + "&CNS=" + reportParam.CNS
                    + "&Pipeline=" + reportParam.Pipeline + "&Current=" + reportParam.Current + "&Closed=" + reportParam.Closed + "&Billable=" + reportParam.Billable + "&Overhead=" + reportParam.Overhead);
                billinggrid._refresh();
    }

</script>
<div style="padding-left: 15px; padding-right: 15px">
    
<%--    <div style="float:left; padding-top:20px; ">
        <div style="float:left;" id="ddReportType">

        </div>
        <div style="float:left;display:none;" id="ddPeriod">

        </div>
    </div>--%>

    <div style="float:left;padding-left:20px;padding-top:20px;">
        <div id="ddlYear"></div>
    </div>

    <div style="float:left; padding-left:15px;padding-top:20px;">
        <div style="float:left;">
        <div id="DivOpportunities"></div>
        <div style="padding-left:5px;" id="DivProjects"></div>
        <div style="padding-left:5px;" id="DivServices"></div>
            </div>
        <div style="float:left; padding-left:100px;">
            <div id="DivPipeline"></div>
            <div style="padding-left:5px;" id="DivCurrent"></div>
            <div style="padding-left:5px;" id="DivClosed"></div>
        </div>

        <div style="float:left; padding-left:100px;">
            <div id="divBillable"></div>
            <div style="padding-left:5px;" id="divOverhead"></div>
        </div>

        <div style="float:left; padding-left:100px;">
            <div id="DivExportToPdf"></div>
        </div>
    </div>
    <div style="float:left; clear:both; display:none;" id="BillingGrid">

    </div>
    <div style="float:left; clear:both;height:10px;"></div>
    <div style="float:left; clear:both; width:100%; height:500px;" id="DivBillingAndMargin">

    </div>
<%--    <div style="float:left; clear:both;height:10px;"></div>
    <div style="float:left; clear:both; width:100%; height:500px; display:none;" id="DivMissedRevenue">
    </div>--%>
    <div id="popup"></div>
</div>