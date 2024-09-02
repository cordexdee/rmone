
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ITGBudgetManagement.ascx.cs"  Inherits="uGovernIT.Web.ITGBudgetManagement" %>

<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Import Namespace="uGovernIT.Helpers" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script type="text/javascript" src="https://vadikom.com/demos/poshytip/src/jquery.poshytip.js"></script>
<link rel="stylesheet" href="https://vadikom.com/demos/poshytip/src/tip-skyblue/tip-skyblue.css" type="text/css" />
<div id="cssDiv">
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">

      fieldset {
        border: 1px solid black;
        margin: 0 2px;
        padding: 0.35em 0.625em 0.75em;
    }

    legend {
        display: unset;
        width: unset;
        padding: unset;
        margin-bottom: unset;
        font-size: unset;
        line-height: unset;
       color: #000;
        border: unset;
        border-bottom: unset;
    }
     .gridWidth
    {
        float: left;
        width: 100%;
    }

    .totalborderhorizontal
    {
        border-top: 1px solid #6c6e70 !important;
        border-bottom: 1px solid #6c6e70 !important;
        font-weight: bold;
    }
    .detailviewmain
    {
        float: left;
        width: 100%;
    }

    .worksheetmessage-m
    {
        text-align: center;
        position: relative;
        top: -10px;
        display: none;
    }

    .worksheetmessage-m1
    {
        float: left;
        padding-left: 7px;
        width: 99%;
    }

    .worksheetheading-m
    {
    }

    .worksheetheading
    {
    }

    .worksheetpanel
    {
    }

    .worksheetpanel-m
    {
        float: left;
        padding: 7px;
        width: 99%;
    }

    .worksheettable
    {
    }

    .worksheetheader
    {
    }

    .paddingfirstcell
    {
    }

    .alnright
    {
        text-align: right;
    }

    .editpanel
    {
        float: left;
    }

    .editinputwidth
    {
        width: 40px;
        height: 12px;
        text-align: right;
    }

    .editdropdownwidth
    {
        width: 168px;
    }

    .alnright
    {
        text-align: right;
    }

    .totalborderhorisontal
    {
        border-bottom: 1px solid #6c6e70 !important;
        font-weight: bold;
    }

    .totalbordervartical
    {
        border-left: 1px solid #6c6e70 !important;
        border-right: 1px solid #6c6e70 !important;
        font-weight: bold;
    }

    .filteredcalender-m
    {
        float: left;
        position: relative;
        width: 300px;
    }

    .calendertxtbox
    {
        margin-right: 46px;
        visibility: hidden;
    }

    .calendernextweekbt
    {
        float: left;
        left: 194px;
        position: absolute;
        padding-top: 3px;
    }

    .calenderpreweekbt
    {
        float: left;
        padding-right: 4px;
        padding-top: 3px;
    }

    .calenderweektxt
    {
        float: left;
        font-weight: bold;
        left: 17px;
        position: absolute;
        text-align: center;
        top: 1px;
        width: 175px;
        padding-top: 3px;
    }

    .categories-list
    {
        float: left;
        max-height: inherit;
        /*min-height: 600px;*/
        overflow-y: auto;
        width: 100%;
    }

    .cbsubcategory label
    {
        display: none;
    }

    .subtotalrowstyle
    {
        font-weight: bold;
        border-color: #000 !important;
        border-top: 2px solid !important;
        border-bottom: 1px solid !important;
    }

    .subtotalrowstyle-first
    {
        font-weight: bold;
        border-color: #000 !important;
        border-top: 2px solid !important;
        border-bottom: 1px solid !important;
        border-left: 1px solid !important;
    }

    .subtotalrowstyle-last
    {
        font-weight: bold;
        border-color: #000 !important;
        border-top: 2px solid !important;
        border-bottom: 1px solid !important;
        border-right: 1px solid !important;
    }

    .ms-viewheadertr th[scope="col"]
    {
        font-weight: bold;
        background: none;
    }

    .hide
    {
        display: none;
    }

    .labelHead
    {
        font-weight: bold;
    }

    .subtotal
    {
        border-bottom: 1px solid #CCCCCC;
        border-top: 1px solid #CCCCCC;
        font-weight: bold;
    }

    .categoryBackground
    {
        background-color: #BED0E5;
    }

    .bitemtable .lastcolumn
    {
       padding-right:3px;
    }

    .headborder
    {
        border-top:1px solid;
        border-right:1px solid;
    }
    .topborder
    {
        border-top:1px solid;
    }
    .rightborder
    {
        border-right:1px solid;
    }
    .leftborder
    {
        border-left:1px solid;
    }
    .clickableLabel
    {
        cursor:pointer;
        color:blue;
    }


  
    .variancerow td
    {
        border-top:1px solid black !important;
        border-bottom:1px solid black !important;
    }

    .variancerow td:first-child
    {
        font-weight:bold;
    }
</style>
</div>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    window.itgGrid = window.itgGrid || {};

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequest);
    prm.add_beginRequest(BeginRequestHandler);
    prm.add_endRequest(EndRequest);

    function InitializeRequest(sender, args) {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        // alert(args.get_postBackElement().id);
        // alert(args.get_postBackElement().id);

        //   args.get_postBackElement().value = "saveing..";
        // alert(args.get_request());
        //  alert(prm.get_isInAsyncPostBack());
        // alert(sender);
        // sender.abortPostBack();
    }
    var notifyId = "";
    function AddNotification(msg) {
        //if (notifyId == "") {
        //    notifyId = SP.UI.Notify.addNotification(msg, true);
        //}
    }
    function RemoveNotification() {
        //SP.UI.Notify.removeNotification(notifyId);
        notifyId = '';
    }
    function BeginRequestHandler(sender, args) {
        //AddNotification("Processing ..");
        //alert("begin");
    }

    function EndRequest(sender, args) {
        window.parent.adjustIFrameWithHeight("<%=FrameId %>", $(".managementcontrol-main").height());
        // alert("End Request");
        var s = sender;
        var a = args;
        var msg = null;
        if (a._error != null) {
            switch (args._error.name) {
                case "Sys.WebForms.PageRequestManagerServerErrorException":
                    msg = "PageRequestManagerServerErrorException";
                    break;
                case "Sys.WebForms.PageRequestManagerParserErrorException":
                    msg = "PageRequestManagerParserErrorException";
                    break;
                case "Sys.WebForms.PageRequestManagerTimeoutException":
                    msg = "PageRequestManagerTimeoutException";
                    break;
            }
            args._error.message = "My Custom Error Message " + msg;
            args.set_errorHandled(true);

        }
        else {
            RemoveNotification();
            $(".datetimectr111").parents("table").find("img").bind("click", function (e) {
                addHeightToCalculateFrameHeight(this, 170);
            });

            try {
                window.itgGrid.collapseDefault();
                clickUpdateSize();
            }
            catch (ex) {
            }
        }
    }


    $(function () {

        $.each($(".bitemtable .subcategory"), function (i, item) {
            var desc = $.trim($(item).find(".itemdescription").html());
            if (desc != "") {
                $(item).find(".action-description").poshytip({ className: 'tip-yellow', bgImageFrameSize: 9, content: desc });
            }
        });

        $(".bitemtable .subcategory").mouseover(function () {
            var desc = $.trim($(this).find(".itemdescription").html());
            if (desc != "") {
                $(this).find(".action-description").css("visibility", "visible");
            }
        });

        $(".bitemtable .subcategory").mouseout(function () {
            $(this).find(".action-description").css("visibility", "hidden");
        });


        $.each($(".bitemtable .ms-viewheadertr th span"), function (i, item) {
            var title = $.trim($(item).attr("tooltip"));
            if (title != "") {
                $(item).poshytip({
                    className: 'tip-yellow', bgImageFrameSize: 9, content: title
                });
            }
        });
       

        window.itgGrid.collapseDefault();

    });

    

  

    function changeCalendarViewType(obj) {
        var url = window.location.href;
        url = url.replace("&calendarViewType=1", "").replace("&calendarViewType=2", "");
        url += "&calendarViewType=" + obj.value;
        //AddNotification("Processing ..");
        window.location.href = url;
    }

    function previewYearBudgetData(obj) {
        var url = window.location.href;
        var currentYear = $(".currentyearsval").html();
        currentYear = Number($.trim(currentYear));
     
        url = url.replace("&calendarYear=" + currentYear, "");
        currentYear += -1;
        url += "&calendarYear=" + currentYear;
        AddNotification("Processing ..");
        window.location.href = url;
    }

    function nextYearBudgetData(obj) {
        var url = window.location.href;
        var currentYear = $(".currentyearsval").html();
        currentYear = Number($.trim(currentYear));
      
        url = url.replace("&calendarYear=" + currentYear, "");
        currentYear += 1;
        url += "&calendarYear=" + currentYear;
        AddNotification("Processing ..");
        window.location.href = url;
    }

    function importBudgetExcel(obj) {
        var path = "<%= importExcelPagePath %>";
        UgitOpenPopupDialog(path, "", "Import ITG  Budget Excel", 50, 20, false, escape(window.location.href));
        return false;
    }

    function exportBudgetExcel(obj) {
        var path = "<%= exportListPagePath %>";
        UgitOpenPopupDialog(path, "", "Export ITG  Budget Excel", 40, 20, false, escape(window.location.href));
        return false;
    }


    //clickUpdateSize();

    window.itgGrid = {

        expandByTr: function (trObj) {
            var startLevel = 1;
            if ($(".penablecategorytype").length > 0) {
                startLevel = 0;
            }

            var currentTr = $(trObj);
            var currentLevel = parseInt(currentTr.attr("level"));
            var trList = $(".bitemtable .bitem");
            var currentIndex = $.inArray(currentTr.get(0), trList)

            currentTr.attr("isExpand", "true")
            currentTr.find(".expandcollapse-icon").attr("src", "/Content/images/minus.png");

            for (var i = currentIndex + 1; i < trList.length; i++) {
                var level = parseInt($(trList[i]).attr("level"));

                if (level > currentLevel) {
                    if ($(trList[i]).attr("isExpand") && $(trList[i]).attr("isExpand") == "true") {
                        $(trList[i]).removeAttr("isExpand");
                        $(trList[i]).find(".expandcollapse-icon").attr("src", "/Content/images/plus.png");
                    }
                    if (level == currentLevel + 1) {
                        $(trList[i]).removeClass("hide");
                    }
                    else {
                        $(trList[i]).addClass("hide");
                    }
                }
                else {
                    break;
                }
            }

        },
        expandCollpaseBudgetCategory: function (obj) {

            var startLevel = 1;
            if ($(".penablecategorytype").length > 0) {
                startLevel = 0;
            }

            var currentTr = $($(obj).parents("tr").get(0));
            var currentLevel = parseInt(currentTr.attr("level"));
            var trList = $(".bitemtable .bitem");
            var currentIndex = $.inArray(currentTr.get(0), trList)
            var doExpand = false;


            if (currentTr.attr("isExpand") == "true") {
                currentTr.removeAttr("isExpand")

            }
            else {
                doExpand = true;
                currentTr.attr("isExpand", "true")

            }


            if (doExpand) {
                currentTr.find(".expandcollapse-icon").attr("src", "/Content/images/minus.png");

                for (var i = currentIndex + 1; i < trList.length; i++) {
                    var level = parseInt($(trList[i]).attr("level"));

                    if (level > currentLevel) {

                        if ($(trList[i]).attr("isExpand") && $(trList[i]).attr("isExpand") == "true") {
                            $(trList[i]).removeAttr("isExpand");
                            $(trList[i]).find(".expandcollapse-icon").attr("src", "/Content/images/plus.png");
                        }

                        if (level == currentLevel + 1) {
                            $(trList[i]).removeClass("hide");
                        }
                        else {
                            $(trList[i]).addClass("hide");
                        }
                    }
                    else {
                        break;
                    }
                }
            }
            else {
                currentTr.find(".expandcollapse-icon").attr("src", "/Content/images/plus.png");

                for (var i = currentIndex + 1; i < trList.length; i++) {
                    var level = parseInt($(trList[i]).attr("level"));

                    if (level > currentLevel) {

                        if ($(trList[i]).attr("isExpand") && $(trList[i]).attr("isExpand") == "true") {
                            $(trList[i]).removeAttr("isExpand");
                            $(trList[i]).find(".expandcollapse-icon").attr("src", "/Content/images/plus.png");
                        }
                        $(trList[i]).addClass("hide");
                    }
                    else {
                        break;
                    }
                }
            }

            window.itgGrid.rememberCollapsedRowsRows();
        },

        collapseDefault: function () {
            var startLevel = 1;
            if ($(".penablecategorytype").length > 0) {
                startLevel = 0;
            }

            var trList = $(".bitemtable .bitem");
            var startLevelItems = $(".bitemtable .bitem[level='" + startLevel + "']");

            if (startLevelItems.length > 3) {
                for (var i = 0; i < trList.length; i++) {
                    var level = parseInt($(trList[i]).attr("level"));
                    var parentNode = $(trList[i]).find(".expandcollapse-icon");
                    if (parentNode.length > 0) {
                        $(trList[i]).removeAttr("isExpand");
                        $(trList[i]).find(".expandcollapse-icon").attr("src", "/Content/images/plus.png");
                    }
                    if (level == startLevel) {
                        $(trList[i]).removeClass("hide");
                    }
                    else {
                        $(trList[i]).addClass("hide");
                    }
                }
            }
            else {

                for (var i = 0; i < trList.length; i++) {
                    var level = parseInt($(trList[i]).attr("level"));

                    if (level == startLevel) {
                        $(trList[i]).removeClass("hide");

                        var parentNode = $(trList[i]).find(".expandcollapse-icon");

                        if (parentNode.length > 0) {
                            $(trList[i]).attr("isExpand", "true");
                            parentNode.attr("src", "/Content/images/minus.png");
                        }
                    }
                    else if (level == startLevel + 1) {
                        $(trList[i]).removeClass("hide");

                        var parentNode = $(trList[i]).find(".expandcollapse-icon");
                        if (parentNode.length > 0) {
                            $(trList[i]).removeAttr("isExpand");
                            parentNode.attr("src", "/Content/images/plus.png");
                        }
                    }
                    else {
                        $(trList[i]).addClass("hide");

                        var parentNode = $(trList[i]).find(".expandcollapse-icon");
                        if (parentNode.length > 0) {
                            $(trList[i]).removeAttr("isExpand");
                            parentNode.attr("src", "/Content/images/plus.png");
                        }
                    }
                }

            }


            var trIndexs = $(".treeStateSpan :hidden").val().split(",");
            if (trIndexs.length > 0) {
                var trList = $(".bitemtable .bitem");
                for (var j = 0; j < trIndexs.length; j++) {

                    var expectedTr = $(".bitemtable .bitem[current='" + trIndexs[j] + "']");
                    if (expectedTr.length > 0) {
                        this.expandByTr(expectedTr.get(0));
                    }
                }
            }
        },
        expandAll: function () {
            var startLevel = 1;
            if ($(".penablecategorytype").length > 0) {
                startLevel = 0;
            }

            var trList = $(".bitemtable .bitem");
            for (var i = 0; i < trList.length; i++) {
                $(trList[i]).removeClass("hide");
                var parentNode = $(trList[i]).find(".expandcollapse-icon");
                if (parentNode.length > 0) {
                    $(trList[i]).attr("isExpand", "true");
                    parentNode.attr("src", "/Content/images/minus.png");
                }
            }
        },

        rememberCollapsedRowsRows: function () {
            var expandedTrList = $(".bitemtable .bitem[isExpand='true']");
            var visibleIndexes = "";
            for (var i = 0; i < expandedTrList.length; i++) {
                if (i != 0) {
                    visibleIndexes += ",";
                }
                visibleIndexes += $(expandedTrList[i]).attr("current");
            }
            $(".treeStateSpan :hidden").val(visibleIndexes);

        },
        forgetVisibleRows: function () {
            $(".treeStateSpan :hidden").val("");
        }
    }

    function startPrint(obj) {
        //window.itgGrid.expandAll();
        var calendarType = $.trim ($('#<%=yearType.ClientID%> :selected').text());
        var calendarYear = $.trim ($('.currentyearsval').text());

        $("#printTD").html($("#tdBudgetDetail").get(0).innerHTML);
        var cssDiv = document.createElement("div");
        $(cssDiv).append($("#cssDiv").get(0).innerHTML);
        $(cssDiv).addClass("printfont");
        var css = new Array();
        css.push('<style type="text/css">');
        css.push('.ms-alternatingstrong{width:40px;background:#F2F2F2 !important;}');
        css.push('.ms-viewheadertr {background-color: #F2F2F2 !important;}');
        css.push('.printfont{font-size:10px; !important;}');
        css.push('</style>');
        $(cssDiv).append(css.join(" "));
        Popup($("#printTD").get(0).innerHTML, cssDiv.innerHTML, calendarType, calendarYear);
    }

    function Popup(data, headerData, calendarType, calendarYear) {
        var mywindow = window.open();
        mywindow.document.write('<html><head><title>Planned Vs Actual Report</title>');
        mywindow.document.write(headerData);
        mywindow.document.write('</head><body class="printfont">');
        mywindow.document.write('<span style="float:left"><H2>Planned vs Actual Report </H2></span> <span style="float:right;"><H2>' + calendarType + '&nbsp;' + calendarYear + '</H2></span>');
        mywindow.document.write(data);
        mywindow.document.write('</body></html>');
        mywindow.document.close();
        mywindow.focus();
        mywindow.print();
      //  mywindow.close();
        return true;
    }
  
</script>
<asp:Panel ID="pEnableCategoryType" runat="server" CssClass="penablecategorytype" Visible="false">
</asp:Panel>

<table cellpadding="0" cellspacing="0" style="border-collapse: collapse;" width="100%" >
    <tr>
        <td>
            <div>
                <span style="display: none;" class="treeStateSpan">
                    <asp:HiddenField ID="hfTreeState" runat="server" />
                </span>

                <asp:Label ID="lbMessage" runat="server" EnableViewState="false" ForeColor="Blue"></asp:Label>
                <asp:HiddenField runat="server" ID="yearHidden" />
                <asp:HiddenField ID="currentSubCategoryNameHidden" runat="server" />
                <asp:HiddenField ID="currentSubCategoryHidden" runat="server" />
                <asp:HiddenField ID="sortingExp" runat="server" />
                 <fieldset id="fldsetNPRResource" runat="server">
                    <legend style="font-weight: bold;">Budget vs Actuals</legend>
                <%--<asp:Panel runat="server" ID="budgetBoxCotainer" GroupingText="Budget vs Actuals">--%>
                    <div class="worksheetmessage-m1" style="text-align: right; float: left">
                        <table cellpadding="2" cellspacing="0" class="bordercolps" width="100%; float:left;">
                            <tr>
                                <td align="left">
                                    <div style="float: left; padding-right: 3px; padding-top: 1px;">
                                        <span style="float: left;">
                                            <img src="/Content/images/expand-all.png" title="Expand All" onclick="window.itgGrid.expandAll()" style="cursor: pointer;" />
                                        </span>
                                        <span style="float: left">
                                            <img onclick="window.itgGrid.collapseDefault()" style="cursor: pointer;" src="/Content/images/collapse-all.png" title="Collapse All" />
                                        </span>
                                    </div>
                                    <div style="float: left;" id="importExportBtns" runat="server">
                                        <span style="float: left;">
                                            <asp:ImageButton ID="btImport" runat="server" ImageUrl="/Content/images/import.png"
                                                alt="import" OnClientClick="return importBudgetExcel(this)" />
                                        </span>
                                        <span style="float: left;">
                                            <asp:ImageButton ID="btExport" runat="server" ImageUrl="/Content/images/export.png"
                                                alt="Export" OnClientClick="return exportBudgetExcel(this)" />
                                        </span>
                                    </div>
                                </td>
                                <td>
                                    <div style="float: right">
                                        <div style="float: left; padding-right: 20px">
                                            <span>
                                                <asp:DropDownList runat="server" ID="yearType" AutoPostBack="false">
                                                    <asp:ListItem Value="1">Calendar Year</asp:ListItem>
                                                    <asp:ListItem Value="2">Fiscal Year</asp:ListItem>
                                                </asp:DropDownList>
                                            </span>
                                        </div>
                                        <div style="float: left;">

                                            <span style="float: left;">
                                                <img src="/Content/images/Previous16x16.png" style="cursor: pointer;"
                                                    onclick="previewYearBudgetData(this)" alt="Pre" title="Previous Year" />
                                            </span><span style="text-align: center; font-weight: bold; float: left; padding: 1px 2px 0px 2px;"
                                                class="currentyearsval">
                                                <%= currentYear %>
                                            </span>
                                            <span style="float: left;">
                                                <img src="/Content/images/Next16x16.png" style="cursor: pointer;" onclick="nextYearBudgetData(this)"
                                                    alt="Next" title="Next Year" />
                                            </span>
                                            <asp:Label CssClass="calenderweektxt" ID="lbWeekDuration" runat="server"></asp:Label>
                                             <span style="float:left;padding-left:10px;" >
                                                <img  src="/Content/images/print-icon.png" alt="Print" height="15" width="15" title="Print"  style="cursor:pointer;" onclick="startPrint(this);"/>
                                            </span>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField runat="server" ID="currentStartDate" Value="<%=Convert.ToString(currentYearStartDate) %>" />
                        <asp:HiddenField runat="server" ID="currentEndDate" Value="<%=Convert.ToString(currentYearEndDate) %>" />
                        <asp:HiddenField runat="server" ID="currentYearHidden" Value="<%=DateTime.Today.Year%>" />
                    </div>

                   <div id="tdBudgetDetail">
                    <asp:Panel runat="server" ID="fixBudgetGridHeight" CssClass='categories-list printfont'>
                        <%-- Budget Information--%>
                        <asp:Repeater ID="rBudgetInfo" runat="server" OnItemDataBound="RBudgetInfo_ItemDataBound" OnPreRender="RBudgetInfo_PreRender">
                            <HeaderTemplate>
                                <table width="100%" cellpadding="0" cellspacing="0" class="bitemtable printfont">
                                    <tr class="ms-viewheadertr">
                                         <th  class="headborder leftborder" style="padding-left:2px; vertical-align:middle;text-align:center;" rowspan="2">Category</th>
                                         <th class="headborder leftborder"  style="padding-left:2px; vertical-align:middle;text-align:center;" rowspan="2"><span tooltip="GL Code for Category">GL Code</span></th>
                                         <th colspan="3" align="center" class="headborder">Non-Project </th>
                                         <th colspan="3" align="center" class="headborder">Project</th>
                                    </tr>
                                    <tr class="ms-viewheadertr">
                                        <th align="right" class="topborder"><span tooltip="Non-Project Planned Budget Amount">Planned</span></th>
                                        <th align="right" class="topborder" ><span tooltip="Non-Project Actual spent so far">Actual</span></th>
                                        <th align="right" class="lastcolumn topborder rightborder"><span tooltip="Non-Project Planned minus Actual">Variance</span></th>
                                        <th align="right" class="topborder"><span tooltip="Project Planned Budget Amount">Planned</span></th>
                                        <th align="right" class="topborder"><span tooltip="Project Actual spent so far">Actual</span></th>
                                        <th align="right" class="lastcolumn topborder rightborder"><span tooltip="Project Planned minus Actual">Variance</span></th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr id="trCategoryType" runat="server" visible="false">
                                    <td colspan="2" align="left" valign="top" class="subtotal">
                                        <img src="/Content/images/minus.png" alt="add" class='expandcollapse-icon' onclick="window.itgGrid.expandCollpaseBudgetCategory(this);" />
                                        <asp:Label ID="lbCategoryType" runat="server" CssClass="labelHead"></asp:Label>
                                    </td>
                                    <td class="subtotal" align="right">
                                        <asp:Label ID="lbCTotalAmount" runat="server"></asp:Label></td>
                                    <td class="subtotal" align="right">
                                        <asp:Label ID="lbCTotalActuals" runat="server"></asp:Label></td>
                                    <td class="subtotal lastcolumn" align="right">
                                        <asp:Label ID="lbCTotalVariance" runat="server"></asp:Label></td>
                                    <td class="subtotal" align="right">
                                        <asp:Label ID="lblProjectPlannedTotal" runat="server"></asp:Label></td>
                                    <td class="subtotal lastcolumn" align="right">
                                        <asp:Label ID="lblProjectActualTotal" runat="server"></asp:Label></td>
                                    <td class="subtotal lastcolumn" align="right">
                                        <asp:Label ID="lblProjectVariancelTotal" runat="server"></asp:Label></td>
                                </tr>
                                <tr id="trCategory" runat="server" visible="false">
                                    <td colspan="2" align="left" valign="top" id="tdCategory" runat="server" class="subtotal">
                                        <img src="/Content/images/minus.png" alt="add" class='expandcollapse-icon' onclick="window.itgGrid.expandCollpaseBudgetCategory(this);" />
                                        <asp:Label ID="lbCategory" runat="server" CssClass="labelHead"></asp:Label>
                                    </td>
                                    <td class="subtotal" align="right">
                                        <asp:Label ID="lbSubCTotalAmount" runat="server"></asp:Label></td>
                                    <td class="subtotal" align="right">
                                        <asp:Label ID="lbSubCTotalActuals" runat="server"></asp:Label></td>
                                    <td class="subtotal lastcolumn" align="right">
                                        <asp:Label ID="lbSubCTotalVariance" runat="server"></asp:Label></td>
                                    <td class="subtotal" align="right">
                                        <asp:Label ID="lblSubTotalPlanned" runat="server"></asp:Label></td>
                                    <td class="subtotal lastcolumn" align="right">
                                        <asp:Label ID="lblSubTotalActual" runat="server"></asp:Label></td>
                                    <td class="subtotal lastcolumn" align="right">
                                        <asp:Label ID="lblSubTotalVariance" runat="server"></asp:Label></td>
                                </tr>
                                <tr id="trSubCategory" runat="server" visible="false" >
                                    <td id="tdSubCategory" runat="server">
                                       <div style="width:100%;float:left;">
                                        <asp:HiddenField ID="hfSubCategoryID" runat="server"  />
                                         <asp:CheckBox ID="cbSubCategory" runat="server" AutoPostBack="true" />
                                        <asp:Label ID="lblCapEx" runat="server" style="color:green;font-weight:bold"></asp:Label>
                                        <asp:Label ID="lbSubCategory" runat="server"></asp:Label>
                                        <asp:Label ID="lbItemDescription" runat="server" CssClass="itemdescription hide"></asp:Label>
                                        <span  class='action-description' style='position:relative;float:right;padding-right:2px;right:0px;top:0px;visibility:hidden;'>
                                            <img src='/Content/buttonimages/comments.png' style='cursor:help;'/>
                                        </span>
                                       </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbBudgetCOA" runat="server"></asp:Label></td>
                                    <td align="right">
                                        <asp:Label ID="lbAmount" runat="server"></asp:Label></td>
                                    <td align="right">
                                        <asp:Label ID="lbActuals" runat="server"></asp:Label></td>
                                    <td align="right" class="lastcolumn">
                                        <asp:Label ID="lbVariance" runat="server"></asp:Label></td>
                                    <td align="right">
                                        <asp:Label ID="lblProjectPlanned" runat="server"></asp:Label></td>
                                    <td align="right" class="lastcolumn">
                                        <asp:Label ID="lblProjectActual" runat="server"></asp:Label></td>
                                    <td align="right" class="lastcolumn">
                                        <asp:Label ID="lblProjectVariance" runat="server"></asp:Label></td>
                                </tr>

                            </ItemTemplate>
                            <FooterTemplate>
                                <tr id="trTotal" runat="server" visible="true">
                                   
                                    <td colspan="2" class="subtotal" valign="top" align="right" style="font-weight: bold;">Grand Total:</td>
                                    <td class="subtotal" align="right">
                                        <asp:Label ID="lbTotalAmount" runat="server"></asp:Label></td>
                                    <td class="subtotal" align="right">
                                        <asp:Label ID="lbTotalActuals" runat="server"></asp:Label></td>
                                    <td class="subtotal lastcolumn" align="right">
                                        <asp:Label ID="lbTotalVariance" runat="server"></asp:Label></td>
                                    <td class="subtotal" align="right">
                                        <asp:Label ID="lblProjectTotal" runat="server"></asp:Label></td>
                                    <td class="subtotal lastcolumn" align="right">
                                        <asp:Label ID="lblProjectActual" runat="server"></asp:Label></td>
                                    <td class="subtotal lastcolumn" align="right">
                                        <asp:Label ID="lblProjectVariance" runat="server"></asp:Label></td>
                                </tr>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </asp:Panel>
                   </div>
                     </fieldset>
               <%-- </asp:Panel>--%>
                  <%--Monthly Budgets ListView--%>
                <fieldset id="Fieldset1" runat="server">
                    <legend style="font-weight: bold;">Non-Project Budget</legend>
                <%--<asp:Panel ID="pNonProjectBudget" runat="server" GroupingText="Non-Project Budget">--%>
                      <div class="worksheetpanel-m">
                            <asp:ListView ID="lvNonProjecdtBudget" Visible="True" runat="server" ItemPlaceholderID="PlaceHolder2"
                                >
                                <LayoutTemplate>
                                    <table class="worksheettable" style="border-collapse: collapse"
                                        width="100%" cellpadding="0" cellspacing="0">
                                        <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr style='<%#  Container.DataItemIndex == 0 ? "font-weight:bold": "" %>' class="<%#  Container.DataItemIndex == 3 ? "variancerow": "" %>">
                                        <td class="ms-vb2 paddingfirstcell">
                                           
                                                <%#  Eval("Title") %>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month1")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month2")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month3")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%#  Eval("Month4")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month5")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%#  Eval("Month6")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month7")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%#Eval("Month8")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month9")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%#  Eval("Month10")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month11") %>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month12")%>
                                        </td>
                                        <td class="ms-vb2 alnright totalbordervartical">
                                            <%#  Eval("Total")%>
                                        </td>
                                      
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="ms-alternatingstrong  <%#  Container.DataItemIndex == 3 ? "variancerow": "" %>">
                                        <td class="ms-vb2 paddingfirstcell">
                                          
                                                <%#  Eval("Title") %>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month1")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month2")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month3")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%#  Eval("Month4")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month5")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%#  Eval("Month6")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month7")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%#Eval("Month8")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month9")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%#  Eval("Month10")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month11") %>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month12")%>
                                        </td>
                                        <td class="ms-vb2 alnright totalbordervartical">
                                            <%#  Eval("Total")%>
                                        </td>
                                       
                                    </tr>
                                </AlternatingItemTemplate>
                                
                            </asp:ListView>
                        </div>
                <%--</asp:Panel>--%>
                    </fieldset>
                 <fieldset id="Fieldset2" runat="server">
                    <legend style="font-weight: bold;">Project Budget</legend>
                <%--  <asp:Panel ID="pProjectBudget" runat="server" GroupingText="Project Budget">--%>
                      <div class="worksheetpanel-m">
                            <asp:ListView ID="lvProjectBudget" Visible="True" runat="server" ItemPlaceholderID="PlaceHolder2"
                               >
                                <LayoutTemplate>
                                    <table class="worksheettable" style="border-collapse: collapse"
                                        width="100%" cellpadding="0" cellspacing="0">
                                        <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr  style='<%#  Container.DataItemIndex < 1 ? "font-weight:bold": "" %>' class="<%#  Container.DataItemIndex == 3 ? "variancerow": "" %>">
                                       <td class="ms-vb2 paddingfirstcell">
                                          
                                                <%#  Eval("Title") %>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month1")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month2")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month3")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%#  Eval("Month4")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month5")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%#  Eval("Month6")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month7")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%#Eval("Month8")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month9")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%#  Eval("Month10")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month11") %>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month12")%>
                                        </td>
                                        <td class="ms-vb2 alnright totalbordervartical">
                                            <%#  Eval("Total")%>
                                        </td>
                                       
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="ms-alternatingstrong <%#  Container.DataItemIndex == 3 ? "variancerow": "" %>" >
                                      <td class="ms-vb2 paddingfirstcell">
                                          
                                                <%#  Eval("Title") %>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month1")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month2")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month3")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%#  Eval("Month4")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month5")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%#  Eval("Month6")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month7")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%#Eval("Month8")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month9")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%#  Eval("Month10")%>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month11") %>
                                        </td>
                                        <td class="ms-vb2 alnright">
                                            <%# Eval("Month12")%>
                                        </td>
                                        <td class="ms-vb2 alnright totalbordervartical">
                                            <%#  Eval("Total")%>
                                        </td>
                                       
                                    </tr>
                                </AlternatingItemTemplate>
                      
                            </asp:ListView>
                        </div>
           <%--     </asp:Panel>--%>
                     </fieldset>
             
            </div>
        </td>
    </tr>
</table>
 <table id="printTable" style="display:none; font-size:8px !important;">
        <tr>
            <td id="printTD" >

            </td>
        </tr>
 </table>