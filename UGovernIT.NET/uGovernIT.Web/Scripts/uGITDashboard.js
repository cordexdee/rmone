var dashboardImpUrl2010 = {};
var dashboardCongiurationData = [];
var charColors = ["#E24A7A", "#5DE9BF", "#4A6EE2", "#FFA500", "#1f4ad4", "#52ce37", "#3adc16"];
var DonotChartElement = '';
var DonotOnlyGlobalConfig = [];
var DonotOnlyUIElement = {};
var currentRequest = null;
var cnt = 0;
function ugitDashboardData(key, data) {

    if (!key && !data)
        return dashboardCongiurationData;

    if (key && !data) {
        var retData = {};
        try {
            retData = dashboardCongiurationData[key];
        } catch (ex) {
            // continue on error 
        }
        return retData;
    }

    if (key && data) {
        dashboardCongiurationData[key] = data;
    }
}


function configurDashboardUrls(siteUrl, filterTicketUrl, viewUrl) {

    dashboardImpUrl2010 = { weburl: siteUrl, filterticketurl: filterTicketUrl, viewurl: viewUrl };
}

function getParameterValue(Key) {
    var pVal = "";
    var url = window.location.href;
    KeysValues = url.split(/[\?&]+/);
    for (i = 0; i < KeysValues.length; i++) {
        KeyValue = KeysValues[i].split("=");
        if (KeyValue[0] == Key) {
            pVal = KeyValue[1];
            break;
        }
    }
    return pVal;
}


//Function modified by Inderjeet Kaur on 23/09/2022
//As discussed with Anurag, since panel cannot be found using panelKey, we are now passing panelId to this method to find the panel.
//New panelKey(GUID) gets generated everytime the page refreshes, whereas panelId(Dashboard ID) remains the same for a panel.
function updateKPIs(panelKey, panelID = "") {
    addToolkit();
    if ($(".dashboardpanelcontainer[panelInstanceID=" + panelKey + "]").length > 0 || $(".dashboardpanelcontainer[panelID=" + panelID + "]").length > 0) {
        var dashboardPanel = ugitDashboardData(panelKey);
        var dirtyKpi = "";
        var duplicateKPI = "";

        dirtyKpi = $($(".dashboardpanelcontainer[panelInstanceID=" + panelKey + "]").get(0)).find(".kpitr");
        duplicateKPI = $(".dashboardpanelcontainer[panelInstanceID=" + panelKey + "]").find(".kpitr");

        if (panelID != "" && panelID > 0)
        {
            if ($(".dashboardpanelcontainer[panelID=" + panelID + "]").length > 0)
            {
                dirtyKpi = $($(".dashboardpanelcontainer[panelID=" + panelID + "]").get(0)).find(".kpitr");
                duplicateKPI = $(".dashboardpanelcontainer[panelID=" + panelID + "]").find(".kpitr");
            }
        }
        //console.log(dashboardPanel.PanelID);
        if (dirtyKpi.length > 0) {
            var kpiIds = "";
            for (var i = 0; i < dirtyKpi.length; i++) {
                if (i != 0) {
                    kpiIds += ",";
                }
                kpiIds += $(dirtyKpi.get(i)).attr("kpilinkid");
            }
            duplicateKPI.find(".kpiresult").html("<strong style='color:green;'>loading...</strong>");
            var globalFilter = "";
            if (dashboardPanel.GlobalFilter) {
                globalFilter = dashboardPanel.GlobalFilter;
            }

            var externalFilter = getParameterValue("externalfilter");
            if (externalFilter != "" && globalFilter.indexOf(";*;") == -1) {
                globalFilter = globalFilter + ";*;" + externalFilter.replace(/'/g, "%27");
                dashboardPanel.GlobalFilter = globalFilter;
            }
            globalFilter = escape(globalFilter);

            var panelId = dashboardPanel.PanelID;
            var viewID = dashboardPanel.ViewID;
            var dataVar = "{'viewID':'" + viewID + "', 'panelID':'" + panelId + "','kpiIDs':'" + kpiIds + "','globalFilter':'" + globalFilter + "'}";
            $.ajax({
                type: "POST",
                url: "/api/Account/GetDashbaordPanelKPI",
                data: dataVar,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    //message.d is the result comming from your ServerSideMethod
                    if (message != null && message != "" && message != undefined) {
                        var Data = [];
                        var resultJson = $.parseJSON(message);
                        var nodata = true;
                        var chartType = resultJson.chartType;
                        if (resultJson != null && resultJson.messageCode == 2) {
                            Data = [];

                            if (resultJson.kpisInfo.length > 0)
                                nodata = false;
                            else
                                nodata = true;
                            for (var i = 0; i < resultJson.kpisInfo.length; i++) {
                                var obj = new Object();
                                if (typeof resultJson.kpisInfo[i].KpiID == "undefined")
                                    continue;
                                var kpiTr = duplicateKPI.filter("[kpilinkid='" + resultJson.kpisInfo[i].KpiID + "']");
                                var fstyle = '';
                                if ($(kpiTr).attr('fstyle') != '' && $(kpiTr).attr('fstyle') != undefined) {
                                    var vals = $(kpiTr).attr('fstyle').split(';#');
                                    fstyle = "style = 'color:" + vals[2] + ";font-weight: " + vals[0] + ";font-size: " + vals[1] + ";'"
                                }
                                if (resultJson.panelViewType == 'Bars') {
                                    kpiTr.find(".kpiresult").html("<span class='dashboardkpi-txt' " + fstyle + ">" + resultJson.kpisInfo[i].kpiTitle.substr(0, resultJson.kpisInfo[i].kpiTitle.lastIndexOf('&nbsp;')) + "</span><div class='chartCount'>" + FormatNumber(resultJson.kpisInfo[i].kpiTitle.split('&nbsp;').pop().trim()) + "</div>");
                                    kpiTr.find("#dynamic_" + resultJson.kpisInfo[i].KpiID).css("width", resultJson.kpisInfo[i].ProgressPct + "%")
                                    //kpiTr.find("#dynamic_" + resultJson.kpisInfo[i].KpiID).css("width", "90%");
                                    kpiTr.find("#dynamic_" + resultJson.kpisInfo[i].KpiID).css({ "width": resultJson.kpisInfo[i].ProgressPct + "%", "background-color": charColors[i] })
                                }
                                else if (resultJson.panelViewType == 'Circular') {
                                    kpiTr.find(".kpiresult").html("<span class='dashboardkpi-txt' " + fstyle + ">" + resultJson.kpisInfo[i].kpiTitle.substr(0, resultJson.kpisInfo[i].kpiTitle.lastIndexOf('&nbsp;')) + "</span>");
                                    kpiTr.find("#dynamic_" + resultJson.kpisInfo[i].KpiID).css("width", resultJson.kpisInfo[i].ProgressPct + "%")
                                    //kpiTr.find("#dynamic_" + resultJson.kpisInfo[i].KpiID).css("width", "90%");
                                    kpiTr.find("#dynamic_" + resultJson.kpisInfo[i].KpiID).css({ "width": resultJson.kpisInfo[i].ProgressPct + "%", "background-color": charColors[i] })
                                    kpiTr.find("#current-progress").html("<div>" + resultJson.kpisInfo[i].kpiTitle.split('&nbsp;').pop().trim() + "</div>")
                                }
                                else if (chartType == 'DoughnutOnly' || chartType == 'PieOnly' || chartType == 'linechart' || chartType == 'squarechart') {
                                    var divIdToHide = '#ctl00_ctl00_MainContent_ContentPlaceHolderContainer_dashboardPanelDock_dashboardView' + viewID + '_paneCustomDashboard' + panelId + '_divProgressChart';
                                    DonotChartElement = '#ctl00_ctl00_MainContent_ContentPlaceHolderContainer_dashboardPanelDock_dashboardView' + viewID + '_paneCustomDashboard' + panelId + '_tdDounutChart';
                                    $(divIdToHide).hide();
                                    DonotOnlyUIElement.viewID = viewID;
                                    DonotOnlyUIElement.panelId = panelId;
                                    DonotOnlyUIElement.kpiIds = kpiIds;

                                }
                                else {
                                    kpiTr.find(".kpiresult").html("<span class='dashboardkpi-txt' " + fstyle + ">" + resultJson.kpisInfo[i].kpiTitle + "</span>");
                                    kpiTr.find(".kpiresult").html("<span class='dashboardkpi-txt' " + fstyle + ">" + resultJson.kpisInfo[i].kpiTitle.substr(0, resultJson.kpisInfo[i].kpiTitle.lastIndexOf('&nbsp;')) + "</span><span style='float:right;color:#232C49;font-family:Catamaran-Bold;'>" + FormatNumber(resultJson.kpisInfo[i].kpiTitle.split('&nbsp;').pop().trim()) + "</span>");
                                    // above line moved in below if condition

                                    // Show progressbar with Completion %
                                    kpiTr.find("#dynamic_" + resultJson.kpisInfo[i].KpiID).css("width", resultJson.kpisInfo[i].ProgressPct + "%")

                                    kpiTr.find(".kpiresult").html("<span class='dashboardkpi-txt' " + fstyle + ">" + resultJson.kpisInfo[i].kpiTitle.substr(0, resultJson.kpisInfo[i].kpiTitle.lastIndexOf('&nbsp;')) + "</span><span style='float:right;color:#909FB1;font-family:Catamaran-Bold;'>" + FormatNumber(resultJson.kpisInfo[i].kpiTitle.split('&nbsp;').pop().trim()) + "</span>");
                                    kpiTr.find("#dynamic_" + resultJson.kpisInfo[i].KpiID).css({ "width": resultJson.kpisInfo[i].ProgressPct + "%", "background-color": charColors[i] })
                                    //}
                                    //else {
                                    //kpiTr.find(".kpiresult").html("<span class='dashboardkpi-txt' " + fstyle + ">" + resultJson.kpisInfo[i].kpiTitle.substr(0, resultJson.kpisInfo[i].kpiTitle.lastIndexOf('&nbsp;')) + "</span><span style='float:right;color:#232C49;font-family:Catamaran-Bold;'>" + resultJson.kpisInfo[i].kpiTitle.split('&nbsp;').pop().trim() + "</span>");
                                    //}
                                    kpiTr.find("#dynamic_" + resultJson.kpisInfo[i].KpiID).attr("Title", resultJson.kpisInfo[i].ProgressPct + "%")

                                }



                                if (i < resultJson.kpisInfo.length - 1) {


                                    obj.value = parseInt(resultJson.kpisInfo[i].kpiTitle.split('&nbsp;').pop().trim().replace('$', ''));
                                    obj.color = charColors[i];
                                    obj.label = resultJson.kpisInfo[i].kpiTitle.substr(0, resultJson.kpisInfo[i].kpiTitle.lastIndexOf('&nbsp;'));
                                    Data.push(obj);
                                }

                            }
                        }

                        if (Data != '') {
                            CreateChart(Data, chartType, resultJson.panelID);
                        }
                        if (nodata) {
                            duplicateKPI.find(".kpiresult").html("<strong style='font-weight:normal;'>0</strong>");
                        }
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    //   alert(ajaxOptions);
                    duplicateKPI.find(".kpiresult").html("<strong style='font-weight:normal;'>0</strong>");
                }
            });
        }
    }

}
var sleepcount = 0;
function sleep(ms) {
    sleepcount = 1;
    return new Promise(resolve => setTimeout(resolve, ms));
}

//Update chart dashboard
async function updateCharts(panelKey, whoTriggered) {
    
    addToolkit();
    var dashboardPanel = ugitDashboardData(panelKey);
    //var uniqueID = ugitDashboardData(panelKey).PanelID;
    if ($(".dashboardpanelcontainer[panelInstanceID=" + panelKey + "]").length > 0) {
        var chartContainer = $(".dashboardpanelcontainer[panelInstanceID=" + panelKey + "] .d-content");

        //chartContainer.append("<strong style='color:green;position:absolute;top:" + chartContainer.height() / 2 + "px;left:" + chartContainer.width() / 2 + "px;' class='loadingchart'><img src='/_layouts/15/images/gears_anv4.gif' style='background:white;' alt='loading..'/></strong>&nbsp;");
        chartContainer.css("position", "relative");

        //var panelId = ugitDashboardData(panelKey).PanelID;

        var drilldownFilter = "";
        if (dashboardPanel.DimensionFilter) {
            drilldownFilter = dashboardPanel.DimensionFilter;
        }

        var localFilter = "";
        if (dashboardPanel.LocalFilter) {
            localFilter = dashboardPanel.LocalFilter;
        }


        var datapointFilter = "";
        if (dashboardPanel.ExpressionFilter) {

            datapointFilter = dashboardPanel.ExpressionFilter;
        }
        var globalFilter = "";
        if (dashboardPanel.GlobalFilter) {
            globalFilter = dashboardPanel.GlobalFilter;
        }
        var externalFilter = getParameterValue("externalfilter");
        if (externalFilter != "" && globalFilter.indexOf(";*;") == -1) {
            globalFilter = globalFilter + ";*;" + externalFilter.replace(/'/g, "%27");
            dashboardPanel.GlobalFilter = globalFilter;
        }
        globalFilter = escape(globalFilter);

        var whoTriggeredEvent = 0;
        if (whoTriggered) {
            whoTriggeredEvent = whoTriggered;
        }

        var viewType = "0";
        if (dashboardPanel.ViewType) {
            viewType = dashboardPanel.ViewType;
        }

        var viewID = dashboardPanel.ViewID;
        var dataVar = "{'viewID':'" + viewID + "', 'panelID':'" + dashboardPanel.PanelID + "','sidebar':'" + dashboardPanel.Sidebar + "','width':'" + dashboardPanel.Width + "','height':'" + dashboardPanel.Height + "','viewType':'" + viewType + "','datapointFilter':'" + escape(datapointFilter) + "','drillDown':'" + drilldownFilter + "','localFilter':'" + localFilter + "','globalFilter':'" + globalFilter + "', 'whoTriggered':'" + whoTriggeredEvent + "', 'zoom':'" + dashboardPanel.Zoom + "'}";
        var dashbaordId = $(".dashboardpanelcontainer[panelInstanceID=" + panelKey + "] .d-content").find(".devexpressChart").attr('id');
        var dashbaord = ASPxClientControl.GetControlCollection().GetByName(dashbaordId);
        //var globalFiltersAndViewID = "";
        window.setTimeout(function () {
            if (dashbaord != null) {
                dashbaord.PerformCallback(dataVar);
                //$.when(dashbaord.PerformCallback(dataVar)).done(function (a) {
                //});
            }
        }, 100);
    }
}
 
//Perform callback and set new data point filter on each end callback for DevExpress xtra Chart control
async function OnEndCallBack(s, e) {
    //debugger;
    var cnt = 0;
    if (sleepcount === 0) {
        sleepcount = 1
        cnt=200;
    }
  window.setTimeout(function () {
        $(".dxlpLoadingPanel").hide();

        var dashboard = $(s.mainElement).parents("div.dashboard-panel");
        var loadingPanel = dashboard.find(".ugitchart_loadingPanel");
        loadingPanel.hide();

        var drillDownIcon = dashboard.find(".drilldownback");

        var filterAndClickEventType = s.chart.cssPostfix;
        s.chart.cssPostfix = "";
        var titleval = '';

        try {
            titleval = s.chart.titles[0].lines[0];
        } catch (ex) { }

        var chartContainer = $(s.mainElement).parents(".dashboardpanelcontainer").find(".dashboardtitle111");
        var titleBox = chartContainer.parents(".dashboardpanelcontainer").find(".dashboardtitle111");
        title = unescape($.trim(titleBox.attr("titletext")));
        if (title != "") {
            if ($.trim(titleval) != "") {
                title = title.replace("$Date$", titleval);
            }
            else {
                title = title.replace("$Date$", "");
            }
        }

        titleBox.html(title);
        titleBox.addClass("labeltooltip");

        if (filterAndClickEventType == "") { return; }

        if (filterAndClickEventType != "") {

            var filter = filterAndClickEventType.split("::")[0];
            var ClickEventType = filterAndClickEventType.split("::")[1];

            setDatapointFilter(s, filter, ClickEventType);

        }
  }, 200);
    ////await sleep(cnt);
    //console.log("Hello");
}

function returnFromDrillDown(obj) {
    //Shows return icon to go back grom drilldown

    var dashboard = $(obj).parents("div.dashboard-panel");
    var drillDownIcon = dashboard.find(".drilldownback");
    var panelInstanceID = dashboard.find(".dashboardpanelcontainer").attr("panelInstanceID");
    if (drillDownIcon.length > 0) {
        var filtersSpan = dashboard.find(".drilldownback-filters");

        var panelData = ugitDashboardData(panelInstanceID);

        var filterExpressions = filtersSpan.find("span");
        if (filterExpressions.length > 0) {
            if (filterExpressions.length > 1) {
                panelData.ExpressionFilter = $(filterExpressions.get(filterExpressions.length - 2)).html();
            }
            else {
                panelData.ExpressionFilter = "";
            }

            updateCharts(panelInstanceID);
            $(filterExpressions.get(filterExpressions.length - 1)).remove();

            showBreadCrumb(dashboard);
        }
        else {
            panelData.ExpressionFilter = "";
            updateCharts(panelInstanceID);
        }

        if (filtersSpan.find("span").length > 0) {
            drillDownIcon.css("display", "block");
        }
        else {
            drillDownIcon.css("display", "none");
        }
    }
}

function setDatapointFilter(obj, filter, eventType) {
    var dashboard = $(obj.mainElement).parents("div.dashboard-panel");
    var panelInstanceID = dashboard.find(".dashboardpanelcontainer").attr("panelInstanceID");
    var panelData = ugitDashboardData(panelInstanceID);
    if (eventType == 1) {

        panelData.DimensionFilter = "";
        var dimensionBox = dashboard.find(".dimensionmenu");
        if (dimensionBox.length > 0)
            dimensionBox.get(0).selectedIndex = 0;

        panelData.ExpressionFilter = filter;
        //Shows return icon to go back grom drilldown
        var drillDownIcon = dashboard.find(".drilldownback");

        if (drillDownIcon.length > 0) {
            drillDownIcon.css("display", "block");
            var filtersSpan = dashboard.find(".drilldownback-filters");

            // control breadcumb:: start 
            if (filtersSpan.find("span").length > 0) {
                var i = filtersSpan.find("span").length - 1;
                var oldfiltervalue = filtersSpan.find("span").get(i).innerText;

                if (filter != oldfiltervalue) { filtersSpan.append("<span>" + filter + "</span>"); }
            }
            else {
                filtersSpan.append("<span>" + filter + "</span>");
            }
            // control breadcumb:: end 
        }

        showBreadCrumb(dashboard);
        updateCharts(panelInstanceID, eventType);
    }
    else if (eventType == 2) {

        var id = panelData.PanelID;
        var lFilter = panelData.LocalFilter ? escape(panelData.LocalFilter) : "";
        var gFilter = panelData.GlobalFilter ? escape(panelData.GlobalFilter) : "";
        var dimension = panelData.DimensionFilter ? escape(panelData.DimensionFilter) : "";
        var dashboardViewID = panelData.ViewID;
        var eFilter = filter ? escape(filter) : "";
        var title = "Filtered List";

        //Get Title of the dashboard
        var chartContainer = $(obj.mainElement).parents(".dashboardpanelcontainer").find(".dashboardtitle111");
        if (chartContainer && $.trim(chartContainer.html()) != "") {
            title = chartContainer.html();
        }

        //Get Expression Filter x-axis point and datapoint
        var eFilterArray = eFilter.split("*");
        if (eFilterArray.length > 5) {
            title = unescape(unescape(eFilterArray[5])).replace(/\+/, " ") + " - " + unescape(unescape(eFilterArray[3])).replace(/\+/, " ");
        }

        var params = "&isdlg=1&isudlg=1&isDashboard=true&dashboardViewID=" + dashboardViewID + "&dashboardID=" + id + "&dimension=" + dimension + "&lFilter=" + lFilter + "&gFilter=" + gFilter + "&eFilter=" + eFilter;
        window.parent.UgitOpenPopupDialog(dashboardImpUrl2010.filterticketurl, params, title, 80, 80);
    }
}

function setDrillDownFilter(obj, filter) {

    var dashboard = $(obj).parents("div[class='dashboard-panel']");
    var panelInstanceID = dashboard.find(".dashboardpanelcontainer").attr("panelInstanceID");
    var panelData = ugitDashboardData(panelInstanceID);

    panelData.DimensionFilter = filter;
    panelData.ExpressionFilter = "";
    var filtersSpan = dashboard.find(".drilldownback-filters");
    filtersSpan.html("");
    var drillDownIcon = dashboard.find(".drilldownback");
    drillDownIcon.hide();
    updateCharts(dashboard.find(".dashboardpanelcontainer").attr("panelInstanceID"), 3);
}

function setLocalDateFilter(obj, panelIntanceID) {

    var panelData = ugitDashboardData(panelIntanceID);
    panelData.LocalFilter = $(obj).val();
    updateCharts(panelIntanceID, 2);
}

function maximizeDashboard(obj, panelIntanceID) {

    getDashboardFilterInfo(obj, "zoom", panelIntanceID, true, 0, 0);
}
function convertDashboardInTable(obj, panelIntanceID) {

    getDashboardFilterInfo(obj, "table", panelIntanceID, true, 1, 0);
}

function getCSVOfDashboard(obj, panelIntanceID) {

    getDashboardFilterInfo(obj, "csvdownload", panelIntanceID, true, 1, 1);
}

function getDashboardFilterInfo(obj, type, panelIntanceID, isMaximize, dashboardView, downloadView) {
    var title = "Filtered View";
    if (type == "zoom") {
        title = "Zoom View";
    }
    else if (type == "table") {
        title = "Table View";
    }
    else if (type == "csvdownload") {
        title = "CSV Download View";
    }

    var chartContainer = $("div.dashboardpanelcontainer[panelInstanceID=" + panelIntanceID + "]");
    var titleContainer = chartContainer.find(".dashboardtitle111"); //$(".dashboardpanelcontainer div[panelInstanceID=" + panelIntanceID + "]").find(".dashboardtitle111");
    if (chartContainer && $.trim(titleContainer.html()) != "") {
        title = titleContainer.html();
    }

    var panelData = ugitDashboardData(panelIntanceID);
    var id = panelData.PanelID;
    var lFilter = panelData.LocalFilter ? panelData.LocalFilter : "";
    var gFilter = panelData.GlobalFilter ? escape(panelData.GlobalFilter) : "";
    var dimension = panelData.DimensionFilter ? panelData.DimensionFilter : "";
    var eFilter = panelData.ExpressionFilter ? escape(panelData.ExpressionFilter) : "";
    var borderStyle = panelData.BorderStyle ? escape(panelData.BorderStyle) : "";
    var dashboardViewID = panelData.ViewID;
    var dTitle = panelData.Title;
    if (dTitle == undefined || dTitle == '') {
        dTitle = title;
    }

    var drilldown = "";
    if (type == "zoom") {
        var dashboard = $(chartContainer.find("img").get(0)).parents("div[class='dashboard-panel']");
        var drillDownArray = new Array();
        dashboard.find(".drilldownback-filters span").each(function (i, s) {
            drillDownArray.push($(s).text().trim());
        });
        if (drillDownArray.length > 0) {
            drilldown = escape(drillDownArray.join(";#"));
        }
    }

    var zoomWidth = 0;
    var zoomHeight = 0;
    if (type == "zoom") {


        var chartWidth = Number(panelData.Width.replace('px', ''));
        var chartHeight = Number(panelData.Height.replace('px', ''));

        var formFator = chartWidth / chartHeight;
        if (chartWidth < chartHeight) {
            formFator = chartHeight / chartWidth;
        }

        //90% of window height
        var popupHeight = ($(window).height() * 85) / 100;
        var popupWidth = popupHeight * formFator - 14;

        zoomWidth = parseInt(popupWidth)
        zoomHeight = parseInt(popupHeight);
    }

    var dashboardInfo = escape("[{'DashboardViewID':'" + dashboardViewID + "', 'DashboardID':" + id + ",'GlobalFilter':'" + gFilter + "','LocalFilter':'" + lFilter + "','DimensionFilter':'" + dimension + "','ExpressionFilter':'" + eFilter + "','Title':'" + dTitle + "','BorderStyle':'" + borderStyle + "','ZoomView':" + isMaximize + ",'View':" + dashboardView + ",'DownloadView':" + downloadView + ",'DrillDownArray':'" + drilldown + "','ZoomWidth':" + zoomWidth + ",'ZoomHeight':" + zoomHeight + "}]");

    var subTitle = "";
    var eFilterArray = eFilter.split("*");
    if (eFilterArray.length > 4) {
        subTitle = ":" + unescape(unescape(eFilterArray[1])).replace(/\+/, " ") + " - " + unescape(unescape(eFilterArray[3])).replace(/\+/, " ");
    }

    title = title + subTitle;

    var width = 50;
    var height = 50;
    var fixedSize = false;
    if (type == "zoom") {
        width = zoomWidth;
        height = zoomHeight;
        width += 35
        width = width + "px";
        height += 20
        height = height + "px";
        fixedSize = true;
    }
    window.parent.UgitOpenPopupDialog(dashboardImpUrl2010.viewurl, "dashboards=" + dashboardInfo, title, width, height);
}

function UpdateGlobalFilterData(viewID, filters) {

    var dataVar = "{'viewID':'" + viewID + "','globalFilter':'" + filters + "'}";
    currentRequest = $.ajax({
        type: "POST",
        crossDomain: true,
        timeout: 40000,
        cache: false,
        url: "/api/Account/GetDashboardFilters",
        data: dataVar,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        beforeSend: function () {
            if (currentRequest != null) {
                currentRequest.abort();
            }
        },
        success: function (message) {
            if (message != null && message != undefined && message != "") {
                var resultJson = $.parseJSON(message);
                if (resultJson != null && resultJson != undefined && resultJson != "" && resultJson.length > 0) {
                    for (var j = 0; j < resultJson.length; j++) {
                        var item = resultJson[j];
                        var valArry = item.Values.split(",");
                        var optionArry = new Array();
                        var jsFilterCtr = $(".globalfilterlistbox").filter("[globalfilterid='" + item.Key + "']");
                        if (jsFilterCtr.length > 0) {
                            jsFilterCtr.html("");
                            if (valArry.length > 0) {
                                for (var i = 0; i < valArry.length; i++) {
                                    optionArry.push("<option value='" + valArry[i] + "'>" + valArry[i] + "</option>");
                                }
                                jsFilterCtr.html(optionArry.join(""));
                            }
                        }
                    }
                }
            }
        },

        error: function (xhr, ajaxOptions, thrownError) {
            //alert(ajaxOptions);
        }
    });
}

function pmenuDimensionItemClick(obj) {
    var selectedDimension = $(obj).val();
    setDrillDownFilter(obj, selectedDimension);
}

function dashboardShowActions(obj) {
    $(obj).find(".dashboardacton-moreaction").show();
}

function dashboardHideActions(obj) {
    $(obj).find(".dashboardacton-moreaction").hide();
}

function onBreadCrumbShowChart(obj, index) {
    //Shows return icon to go back grom drilldown
    var dashboard = $(obj).parents("div.dashboard-panel");
    var drillDownIcon = dashboard.find(".drilldownback");
    var panelInstanceID = dashboard.find(".dashboardpanelcontainer").attr("panelInstanceID");
    if (drillDownIcon.length > 0) {
        var filtersSpan = dashboard.find(".drilldownback-filters");

        var panelData = ugitDashboardData(panelInstanceID);

        var filterExpressions = filtersSpan.find("span");
        if (index >= 0) {

            panelData.ExpressionFilter = "";
            if (index > 0)
                panelData.ExpressionFilter = $(filterExpressions.get(index - 1)).html();

            updateCharts(panelInstanceID);
            if (filtersSpan.find("span").length > index) {
                for (var i = filtersSpan.find("span").length - 1; i >= index; i--) {

                    filtersSpan.find("span").get(i).remove();
                }
            }
            showBreadCrumb(dashboard);
        }
        else {
            filtersSpan.find("span").remove();
            panelData.ExpressionFilter = "";
            updateCharts(panelInstanceID);
        }


        if (filtersSpan.find("span").length > 0) {
            drillDownIcon.css("display", "block");
        }
        else {
            drillDownIcon.css("display", "none");
        }
    }
}

function showBreadCrumb(dashboard) {

    addToolkit();
    var filters = dashboard.find(".drilldownback-filters span");
    var breadcrumbDiv = dashboard.find(".chartbreadcrumb");
    breadcrumbDiv.html("");
    var breadcrumbArray = new Array();
    if (filters.length > 0) {
        filters.each(function (i, s) {
            var fv = unescape($(s).html());
            var fvArray = fv.split("*");
            breadcrumbArray.push("<a href='javascript:' class='breadcrumbAnchor' onclick='onBreadCrumbShowChart(this, " + i + ");'>" + fvArray[fvArray.length - 1] + "</a>");

        });
        breadcrumbDiv.html(breadcrumbArray.join(" > "));
    }
}

function addToolkit() {
    try {
        $(".dashboardtitle111").tooltip({
            hide: { effect: "fadeOut", easing: "easeInExpo" },
            content: function () {

                var title = $(this).attr("title");
                if (title)

                    return unescape(title).replace("\n\r", "<br/>");
            }
        });

    } catch (ex) { }
}
function getDoughnutConfig(viewID, panelId, kpiIds, scallback, ecallback) {
    var dataVar = "{'viewID':'" + viewID + "', 'panelID':'" + panelId + "','kpiIDs':'" + kpiIds + "','globalFilter':''}";
    $.ajax({
        type: "POST",
        url: "/api/Account/getDoughnutOnlyConfiguration",
        data: dataVar,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            scallback(data);
        },
        error: function (err) {
            ecallback(err);
        }
    });
}
function drawLineChart(data) {
    var parentDivElement = "#ctl00_ctl00_MainContent_ContentPlaceHolderContainer_dashboardPanelDock_dashboardView" + DonotOnlyUIElement.viewID + "_paneCustomDashboard" + DonotOnlyUIElement.panelId + "_dashboardMainContainer";
    var chartData = [];
    for (var i = 0; i < data.length; i++) {
        var obj = new Object();
        obj.city = data[i].label;
        obj.value = data[i].value;
        obj.color = data[i].color;
        chartData.push(obj);
    }
    var chart = $(DonotChartElement + ' #devExtremedoughnutChart').dxChart({
        palette: "Violet",
        dataSource: chartData,
        commonSeriesSettings: {
            argumentField: "city",
            type: 'line'
        },
        margin: {
            bottom: 20,
            height: $(parentDivElement).height(),
            width: $(parentDivElement).width()
        },
        argumentAxis: {
            valueMarginsEnabled: false,
            discreteAxisDivisionMode: "crossLabels",
            grid: {
                visible: true
            }
        },
        series: [
            { valueField: "value", name: "city" },
        ],
        legend: {
            verticalAlignment: "bottom",
            horizontalAlignment: "center",
            itemTextPosition: "bottom"
        },
        title: {
            //text: "Line Chart"
            //subtitle: {
            //    text: "(Millions of Tons, Oil Equivalent)"
            //}
        },
        "export": {
            enabled: false
        },
        tooltip: {
            enabled: true
        }
    }).dxChart("instance");
}
function drawHierarchicalDataStructure(argData) {
    var parentDivElement = "#ctl00_ctl00_MainContent_ContentPlaceHolderContainer_dashboardPanelDock_dashboardView" + DonotOnlyUIElement.viewID + "_paneCustomDashboard" + DonotOnlyUIElement.panelId + "_dashboardMainContainer";
    var chartData = [];
    for (var i = 0; i < argData.length; i++) {
        var obj = new Object();
        obj.city = argData[i].label;
        obj.value = argData[i].value;
        obj.color = argData[i].color;
        chartData.push(obj);
    }
    $(DonotChartElement + ' #devExtremedoughnutChart').dxTreeMap({
        dataSource: chartData,
        labelField: 'city',
        valueField: 'value',
        // title: "The Most Populated Cities by Continents",
        //size: {
        //    height: $(parentDivElement).height(),
        //    width: $(parentDivElement).width()
        //},
        tooltip: {
            enabled: true,
            format: "thousands",
            customizeTooltip: function (arg) {
                var data = arg.node.data,
                    result = null;

                if (arg.node.isLeaf()) {
                    result = "<span class='city'>" + data.city + "</span> (" +
                        arg.valueText + ")<br/>";
                }

                return {
                    text: result
                };
            }
        },
    });
}
function drawDevExtremeDoughnut(data, chartType) {
    var helper;
    var parentDivElement = "#ctl00_ctl00_MainContent_ContentPlaceHolderContainer_dashboardPanelDock_dashboardView" + DonotOnlyUIElement.viewID + "_paneCustomDashboard" + DonotOnlyUIElement.panelId + "_dashboardMainContainer";
    //$(DonotChartElement).removeClass("col-md-4 col-sm-6 col-xs-12 noPadding");
    getDoughnutConfig(DonotOnlyUIElement.viewID, DonotOnlyUIElement.panelId, DonotOnlyUIElement.kpiIds,
        function (success) {
            console.log(success);
            var resultJson = $.parseJSON(success);//JSON.parse(JSON.stringify(success));
            helper = new Object();
            helper.horizontalAlignment = resultJson.hAlign;
            helper.verticalAlignment = resultJson.vAlign;
            helper.legendvisible = resultJson.Legendvisible;
            helper.textFormat = resultJson.textFormat;
            helper.displayValueType = resultJson.displayValueType;
            helper.centreTitle = resultJson.centreTitle;
            helper.height = $(parentDivElement).height();
            helper.width = $(parentDivElement).width()
        },
        function (error) {
            console.log(error);
        });
    //console.log(DonotChartElement);
    var chartData = [];
    for (var i = 0; i < data.length; i++) {
        var obj = new Object();
        obj.region = data[i].label;
        obj.val = data[i].value;
        obj.color = data[i].color;
        obj.country = helper.centreTitle;
        chartData.push(obj);
    }
    var formatNumber = new Intl.NumberFormat("en-US", { maximumFractionDigits: 0 }).format;

    var commonSettings = {
        innerRadius: 0.65,
        resolveLabelOverlapping: "shift",
        centerTemplate: function (pieChart, container) {
            if (chartType == 'pie') {
                var total = pieChart.getAllSeries()[0].getVisiblePoints().reduce(function (s, p) { return s + p.originalValue; }, 0),
                    country = pieChart.getAllSeries()[0].getVisiblePoints()[0].data.country,
                    content = $('<svg><circle cx="100" cy="100" fill="transparent" r="' + (pieChart.getInnerRadius() - 6) + '"></circle>' +
                        //'<image x="70" y="58" width="60" height="40" href="' + helper.centreImageUrl + '"/>' +
                        '<text text-anchor="middle" style="font-size: 18px" x="100" y="95" fill="#494949">' +
                        '<tspan x="100" ></tspan>' +
                        '<tspan x="100" dy="16px" fill="#fff" style="font-weight: 50;font-size:13px;">' +
                        '$' + convertToInternationalCurrencySystem(total) +
                        '</tspan></text></svg>');
            }
            else {
                var total = pieChart.getAllSeries()[0].getVisiblePoints().reduce(function (s, p) { return s + p.originalValue; }, 0),
                    country = pieChart.getAllSeries()[0].getVisiblePoints()[0].data.country,
                    content = $('<svg><circle cx="100" cy="100" fill="transparent" r="' + (pieChart.getInnerRadius() - 6) + '"></circle>' +
                        //'<image x="70" y="58" width="60" height="40" href="' + helper.centreImageUrl + '"/>' +
                        '<text text-anchor="middle" style="font-size: 18px" x="100" y="95" fill="#494949">' +
                        '<tspan x="100" >' + country + '</tspan>' +
                        '<tspan x="100" dy="16px" style="font-weight: 50;font-size:13px;color:#fff;">' +
                        '$' + convertToInternationalCurrencySystem(total) +
                        '</tspan></text></svg>');
            }


            container.appendChild(content.get(0));
        }
    };
    var elementName = DonotChartElement + ' #devExtremedoughnutChart';
    $(elementName).dxPieChart($.extend({}, commonSettings, {
        type: chartType,
        palette: "Soft Pastel",
        dataSource: chartData,
        onPointClick: function (info) {
            alert();
            console.log(info);
        },
        //title: "PIPELINE BY BUSINESS SECTOR",
        tooltip: {
            enabled: true,
            format: helper.textFormat,
            customizeTooltip: function (arg) {
                if (helper.textFormat == 'currency') {
                    var argData = formatNumber(arg.value.toFixed(1));
                    console.log(argData);
                    return {
                        text: arg.argumentText + " ($" + (argData) + ")"
                    };
                }
                else if (helper.textFormat == 'millions') {
                    var argData = convertToMillion(arg.value);
                    console.log(argData);
                    return {
                        text: arg.argumentText + " (" + (argData) + ")"
                    };
                }
                else if (helper.displayValueType == "percentage") {
                    return {
                        text: arg.argumentText + " (" + (arg.percent * 100).toFixed(2) + "%" + ")"
                    };
                }
                else {
                    return {
                        text: arg.argumentText + " (" + (arg.value).toFixed(1) + ")"
                    };
                }
            }
        },
        legend: {
            horizontalAlignment: helper.horizontalAlignment,
            verticalAlignment: helper.verticalAlignment,
            visible: helper.legendvisible == 'true' ? true : false,
            orientation: "vertical" // or "horizontal"


        },
        "export": {
            enabled: false
        },
        size: {
            height: helper.height,
            width: helper.width
        },
        adaptiveLayout: {
            keepLabels: true,
            height: helper.height,
            width: helper.width
        },
        series: [{
            argumentField: "region",
            alignment: 'center',//'center' | 'left' | 'right'
            label: {
                visible: true,
                backgroundColor: 'none',
                //radialOffset:50,
                //rotationAngle: 300,
                textOverflow: 'ellipsis',//'ellipsis' | 'hide' | 'none'
                wordWrap: 'normal',// 'normal' | 'breakWord' | 'none'
                useNodeColors: true,
                customizeText: function (pointInfo) {
                    if (helper.textFormat == 'currency') {
                        var argData = formatNumber(pointInfo.value.toFixed(1));
                        return pointInfo.argumentText + "\n$" + argData;
                    }
                    else if (helper.textFormat == 'millions') {
                        var argData = convertToInternationalCurrencySystem(pointInfo.value);
                        return pointInfo.argumentText + "\n" + argData;
                    }
                    else if (helper.textFormat == "percentage") {
                        return pointInfo.argumentText + "\n" + (pointInfo.percent * 100).toFixed(2) + "%";
                    }
                    else {
                        return pointInfo.argumentText + "\n" + pointInfo.value;
                    }
                    //return  '$'+convertToInternationalCurrencySystem(pointInfo.value);
                },
                position: "outside",
                format: {
                    type: helper.textFormat,//"millions",
                    precision: 1,
                    currency: 'USD'
                },
                font: {
                    size: 11
                },
                connector: {
                    visible: true
                }
            }
        }]
    }));
}
function convertToMillion(value) {
    return (Math.abs(Number(value)) / 1.0e+6).toFixed(2) + " M$"
}
function convertToInternationalCurrencySystem(labelValue) {

    // Nine Zeroes for Billions
    return Math.abs(Number(labelValue)) >= 1.0e+9

        ? (Math.abs(Number(labelValue)) / 1.0e+9).toFixed(2) + "B"
        // Six Zeroes for Millions 
        : Math.abs(Number(labelValue)) >= 1.0e+6

            ? (Math.abs(Number(labelValue)) / 1.0e+6).toFixed(2) + "M"
            // Three Zeroes for Thousands
            : Math.abs(Number(labelValue)) >= 1.0e+3

                ? (Math.abs(Number(labelValue)) / 1.0e+3).toFixed(2) + "K"

                : Math.abs(Number(labelValue));

}

function CreateChartOnPanel(data, chartType, panelId) {
    var chart = null;
    var eleChartJquery = $("canvas[id$='" + panelId + "_doughnutChart']");
    var eleChart = eleChartJquery[0];

    if (eleChart !== undefined && eleChart !== null) {
        //var td = document.getElementById("tdDounutChart").getContext('2d');
        var ctx = eleChart.getContext('2d');

        var options = {
            segmentShowStroke: false,
            animateScale: false,
            animateRotate: false
        };

        if (chartType == "Doughnut") {
            chart = new Chart(ctx).Doughnut(data, options);
        }
        else if (chartType == "Pie") {
            chart = new Chart(ctx).Pie(data, options);
        }
    }
}

function CreateChart(data, chartType, panelID) {
    var chart = null;
    var eleChartJquery = $("canvas[id$='" + panelID + "_doughnutChart']");
    var eleChart = eleChartJquery[0];
    /*var eleChart = document.getElementById("tdDounutChart");*/

    if (eleChart !== undefined && eleChart !== null) {
        //var td = document.getElementById("tdDounutChart").getContext('2d');
        var ctx = eleChart.getContext('2d');

        var options = {
            segmentShowStroke: false,
            animateScale: false,
            animateRotate: false
        };

        if (chartType == "Doughnut") {
            chart = new Chart(ctx).Doughnut(data, options);
        }
        else if (chartType == "DoughnutOnly") {
            eleChart.remove();
            var a = new drawDevExtremeDoughnut(data, 'Doughnut');
        }
        else if (chartType == "PieOnly") {
            eleChart.remove();
            var b = new drawDevExtremeDoughnut(data, 'pie');
        }
        else if (chartType == "linechart") {
            eleChart.remove();
            chart = new drawLineChart(data, 'linechart');
        }
        else if (chartType == "squarechart") {
            eleChart.remove();
            chart = new drawHierarchicalDataStructure(data);
        }
        else if (chartType == "Pie") {
            chart = new Chart(ctx).Pie(data, options);
        }
    }
}

// to queue all functions and avoid duplicate call
//var uGITApp = {
//    arrFunction: [],

//    wrapFunction: function (fn, context, params) {
//        return function () {
//            fn.apply(context, params);
//        };
//    },

//    // check and add
//    queueFunction: function (pFunction, pKey, pValue) {
//        debugger;
//        var found = this.arrFunction.some(function(el) {
//            return el.paramKey === pKey;
//        });
//        if (!found) {
//            var wFunction = this.wrapFunction(pFunction, this, [pKey, pValue]);
//            this.arrFunction.push({
//                paramKey: pKey,
//                paramFunction: wFunction
//            });
//            //wFunction();
//        }
//    },
//    callFunction: function() {
//        while (this.arrFunction.length > 0) {
//            // execute functions
//            (this.arrFunction.shift().paramFunction)();
//        }
//    }
//};

//// on document ready call all functions
//document.onreadystatechange = () => {
//    if (document.readyState === 'complete') {
//        if (uGITApp.arrFunction.length > 0) {
//            uGITApp.callFunction();
//        }
//        else {
//            setTimeout(function () { uGITApp.callFunction(); }, 3000);
//        }
//    }
//};

function showPanel_filterData(url, data, title, panelKey) {
    var dashboardPanel = ugitDashboardData(panelKey);
    var globalFilter = "";
    if (dashboardPanel.GlobalFilter) {
        globalFilter = dashboardPanel.GlobalFilter;
    }
    globalFilter = escape(globalFilter);
    data += "&ViewID=" + dashboardPanel.ViewID + "&globalFilter=" + globalFilter;
    window.parent.UgitOpenPopupDialog(url, data, title, 90, 90, 0);
}

function triggerRefreshDashboard() {
    var panelDivs = $(".dashboardpanelcontainer").filter("[panelInstanceID]");
    panelDivs.each(function (index, item) {

        var panelKey = $(item).attr("panelInstanceID");
        var dashboard = ugitDashboardData(panelKey);


        if (panelKey && dashboard.Type == 'chart') {

            updateCharts(panelKey, 5);
        }
        else if (panelKey && dashboard.Type == 'panel') {

            updateKPIs(panelKey);
        }

    });
}

// Inserts commas in string containing Number eg., $1234567 to $1,234,567 &  1234567 to 1,234,567
function FormatNumber(number) {
    if (number != undefined && number != '') {
        //return number[0] + parseInt(number.split('$').pop()).toLocaleString('en-US', { maximumFractionDigits: 2 });
        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
    return number;
}

function CreateDevExtremeChart(data, chartType, panelId) {
    var paneldivs = $("div[id$='" + panelId + "_divpiechart']");
    $("div[id$='" + panelId + "_divpiechart']").dxPieChart({
        type: "doughnut",
        palette: "Soft Pastel",
        dataSource: data,
        series: [{
            argumentField: "label",
            valueField: "value",
            label: {
                visible: true,
                connector: {
                    visible: true
                }
            }
        }]
    });
}


function GetChartDetails(viewId, panelid) {
    var element = 'divpiechart1_' + viewId + '_' + panelid;
    $.ajax({
        type: "POST",
        url: "/api/CoreRMM/GetUserChartDetails?ViewID=" + viewId + "&PanelId=" + panelid,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (message) {
            var json = JSON.parse(message);
            // if Month column will be present then sort with month
            sortByMonth(json.aoData);         
            new createDxChart(element, json.aoData, json.valueAxis, json.series, json.dashboardTitle, json.textFormat, json.legend);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr);
        }
    });
}

function createDxChart(element, dataSource, valueAxsis, Series, title, textFormat, legend) {
    $("#" + element).dxChart({
        //palette: "violet",
        palette: 'Harmony Light',
        dataSource: dataSource,
        title: {
            text: title,
            horizontalAlignment: 'center',//'center' | 'left' | 'right'
            verticalAlignment: 'top', //'bottom' | 'top'
        },
        argumentAxis: {
            label: {
                wordWrap: "none",
                overlappingBehavior: "rotate",//"stagger"
            }
        },
        tooltip: {
            enabled: true,
            format: textFormat

        },
        valueAxis: valueAxsis,
        scrollBar: {
            visible: false
        },
        zoomAndPan: {
            argumentAxis: "both"
        },
        series: Series,
        legend: legend
    });
}

function sortByMonth(arr) {
    var monthAbbr = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var month = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    let exist = Object.keys(arr[0]).filter(key => key.includes('Month')).length > 0;
    if (exist) {
        if (monthAbbr.indexOf(arr[0].Month) > -1) {
            arr.sort(function (a, b) {
                return monthAbbr.indexOf(a.Month) - monthAbbr.indexOf(b.Month);
            });
        }
        else if (month.indexOf(arr[0].Month) > -1) {
            arr.sort(function (a, b) {
                return month.indexOf(a.Month) - month.indexOf(b.Month);
            });
        }
    }
}